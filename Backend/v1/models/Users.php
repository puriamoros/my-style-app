<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/ModelWithIdBase.php');

class Users extends ModelWithIdBase
{
    // Data from table "users"
    const TABLE_NAME = "users";
	const ID = "id";
    const EMAIL = "email";
    const PASSWORD = "password";
	const API_KEY = "apiKey";
	
	public function __construct()
    {
        $this->table = 'users';
		$this->fields = array(
			'id',
			'email',
			'password',
			'apiKey'
		);
		$this->idField = $this->fields[0];
    }

    public function get($queryArray)
    {
		if(count($queryArray) >= 1 && count($queryArray) <= 2) {
			if ($queryArray[0] == 'login') {
				return $this->login();
			} else if ($queryArray[0] == 'users') {
				if(count($queryArray) == 1) {
					return $this->getElements();
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
			"apiKey" => $apiKey
		);
	}
	
	protected function getElements()
	{
		$result = parent::getElements();
		for ($i = 0; $i < count($result); $i++) {
			unset($result[$i]["password"]);
			unset($result[$i]["apiKey"]);
		}
		return $result;
	}
	
	private function getMe()
	{
		// Check authorization
		$id = Authorization::authorizeApiKey();
		return $this->getElement($id);
	}
	
	protected function getElement($id)
	{
		$result = parent::getElement($id);
		if(!is_null($result)) {
			unset($result["password"]);
			unset($result["apiKey"]);
		}
		return $result;
	}
	
	private function register()
	{
		// No authorization needed
	
		$data = $this->getBodyData();
		$data["password"] = $this->encryptPassword($data["password"]);
		$data["apiKey"] = $this->generateApiKey();
		//throw new ApiException(STATE_DB_ERROR, $data);

		// TODO: Validate fields
		
		// Create user
		$result = $this->dbCreate($data);
		
		// Print response
		http_response_code(200);
		return array(
			"apiKey" => $result["apiKey"]
		);
	}
	
	protected function createElement()
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$data = $this->getBodyData();
		$data["password"] = $this->encryptPassword($data["password"]);
		$data["apiKey"] = $this->generateApiKey();

		// TODO: Validate fields
		
		// Create user
		$result = $this->dbCreate($data);
		unset($result["password"]);
		unset($result["apiKey"]);
		
		// Print response
		http_response_code(201);
		
		return $result;
	}
	
	protected function updateElement($id)
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$data = $this->getBodyData();
		$data["password"] = $this->encryptPassword($data["password"]);
		unset($data["apiKey"]);

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
			$command = "SELECT " . self::ID .
				" FROM " . self::TABLE_NAME .
				" WHERE " . self::API_KEY . "=?";

			$query = DBConnection::getInstance()->getDB()->prepare($command);

			$query->bindParam(1, $apiKey);

			if($query->execute()) {
					$result = $query->fetch();
					return $result[self::ID];
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
				$command = "SELECT " . self::PASSWORD . ", " .self::API_KEY .
					" FROM " . self::TABLE_NAME .
					" WHERE " . self::EMAIL . "=?";

				$query = DBConnection::getInstance()->getDB()->prepare($command);

				$query->bindParam(1, $email);

				if($query->execute()) {
					$result = $query->fetch();
					if(password_verify($password, $result[self::PASSWORD])) {
						return $result[self::API_KEY];
					}
				}

			} catch (PDOException $e) {
			}
		}
		
		return null;
	}
}