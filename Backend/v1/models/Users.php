<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/../utilities/Tables.php');
require_once(__DIR__.'/ModelWithIdBase.php');

class Users extends ModelWithIdBase
{
	public function __construct()
    {
		// Users table data
		$this->users = Tables::getInstance()->users;
		
		// Call parent ctor
		parent::__construct($this->users->table, $this->users->fields, $this->users->id);
    }

    public function get($queryArray, $queryParams)
    {
		if(count($queryArray) >= 1 && count($queryArray) <= 2) {
			if ($queryArray[0] == 'login') {
				return $this->login();
			} else if ($queryArray[0] == 'users') {
				if(count($queryArray) == 1) {
					return $this->getElements($queryParams);
				}
				else {
					if($queryArray[1] == "me") {
						return $this->getMe();
					}
					else {
						return $this->getElement($queryArray[1]);
					}
				}
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function post($queryArray)
    {
		if(count($queryArray) == 1) {
			if ($queryArray[0] == 'register') {
				return $this->register();
			} else {
				return $this->createElement($queryArray);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	private function login()
	{
		// Check authorization
		$apiKey = Authorization::authorizeBasic();
		
		http_response_code(200);
		return array(
			Tables::getInstance()->users->apiKey => $apiKey
		);
	}
	
	protected function getElements($queryParams)
	{
		$result = parent::getElements($queryParams);
		for ($i = 0; $i < count($result); $i++) {
			unset($result[$i][Tables::getInstance()->users->password]);
			unset($result[$i][Tables::getInstance()->users->apiKey]);
		}
		return $result;
	}
	
	private function getMe()
	{
		return $this->getElement(null);
	}
	
	protected function getElement($id)
	{
		$result = parent::getElement($id);
		if(!is_null($result)) {
			unset($result[Tables::getInstance()->users->password]);
			unset($result[Tables::getInstance()->users->apiKey]);
		}
		return $result;
	}
	
	private function register()
	{
		// No authorization needed
	
		$data = $this->getBodyData();
		$data[Tables::getInstance()->users->password] = $this->encryptPassword($data[Tables::getInstance()->users->password]);
		$data[Tables::getInstance()->users->apiKey] = $this->generateApiKey();
		//throw new ApiException(STATE_DB_ERROR, $data);

		// TODO: Validate fields
		
		// Create user
		$result = $this->dbCreate($data);
		
		// Print response
		http_response_code(200);
		return array(
			Tables::getInstance()->users->apiKey => $result[Tables::getInstance()->users->apiKey]
		);
	}
	
	protected function createElement()
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$data = $this->getBodyData();
		$data[Tables::getInstance()->users->password] = $this->encryptPassword($data[Tables::getInstance()->users->password]);
		$data[Tables::getInstance()->users->apiKey] = $this->generateApiKey();

		// TODO: Validate fields
		
		// Create user
		$result = $this->dbCreate($data);
		unset($result[Tables::getInstance()->users->password]);
		unset($result[Tables::getInstance()->users->apiKey]);
		
		// Print response
		http_response_code(201);
		
		return $result;
	}
	
	protected function updateElement($id)
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$data = $this->getBodyData();
		$data[Tables::getInstance()->users->password] = $this->encryptPassword($data[Tables::getInstance()->users->password]);
		unset($data[Tables::getInstance()->users->apiKey]);

		// TODO: Validate fields
		
		// Update user
		$this->dbUpdate($id, $data);
		
		// Print response
		http_response_code(204);
		return;
	}
	
	private function encryptPassword($plainPassword)
	{
		if ($plainPassword)
			return password_hash($plainPassword, PASSWORD_DEFAULT);
		else return null;
	}
	
	private function generateApiKey()
	{
		return md5(microtime().rand());
	}
	
	public static function validateApiKey($apiKey)
	{
		try {
			$command = "SELECT " . Tables::getInstance()->users->id .
				" FROM " . Tables::getInstance()->users->table .
				" WHERE " . Tables::getInstance()->users->apiKey . "=?";

			$query = DBConnection::getInstance()->getDB()->prepare($command);

			$query->bindParam(1, $apiKey);

			if($query->execute()) {
					$result = $query->fetch();
					return $result[Tables::getInstance()->users->id];
				}

			} catch (PDOException $e) {
			}
			
			return null;
	}
	
	public static function validateBasic($basic)
	{
		$basicPlain = base64_decode($basic);
		$basicArray = explode(":", $basicPlain);
		
		if(count($basicArray) == 2) {
			$email = $basicArray[0];
			$password = $basicArray[1];
			
			try {
				$command = "SELECT " . Tables::getInstance()->users->password . ", " .Tables::getInstance()->users->apiKey .
					" FROM " . Tables::getInstance()->users->table .
					" WHERE " . Tables::getInstance()->users->email . "=?";

				$query = DBConnection::getInstance()->getDB()->prepare($command);

				$query->bindParam(1, $email);

				if($query->execute()) {
					$result = $query->fetch();
					if(password_verify($password, $result[Tables::getInstance()->users->password])) {
						return $result[Tables::getInstance()->users->apiKey];
					}
				}

			} catch (PDOException $e) {
			}
		}
		
		return null;
	}
}