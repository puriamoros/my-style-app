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
					//return $this->getElements($queryParams);
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
	
	protected function authorizeGetElement($id)
	{
		$user = $this->authorizeDefault();
		
		if($user[$this->users->id] !== $id) {
			throw new ApiException(STATE_NOT_AUTHORIZED, "Not authorized", 401);
		}
	}
	
	public function post($queryArray)
    {
		if(count($queryArray) == 1) {
			if ($queryArray[0] == 'register') {
				return $this->register();
			} else {
				//return $this->createElement($queryArray);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function put($queryArray)
    {
		if(count($queryArray) == 2) {
			return $this->updateElement($queryArray[1]);
		}
		if(count($queryArray) == 3) {
			return $this->updatePassword($queryArray[1]);
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function delete($queryArray)
    {
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	private function login()
	{
		// Check authorization
		$apiKey = Authorization::authorizeBasic()[$this->users->apiKey];

		http_response_code(200);
		return array(
			$this->users->apiKey => $apiKey
		);
	}
	
	protected function getElements($queryParams)
	{
		$result = parent::getElements($queryParams);
		for ($i = 0; $i < count($result); $i++) {
			unset($result[$i][$this->users->password]);
			unset($result[$i][$this->users->apiKey]);
		}
		return $result;
	}
	
	private function getMe()
	{
		// Get id from apiKey
		$id = Authorization::authorizeApiKey()[$this->users->id];

		return $this->getElement($id);
	}
	
	protected function getElement($id)
	{
		$result = parent::getElement($id);
		if(!is_null($result)) {
			unset($result[$this->users->password]);
			unset($result[$this->users->apiKey]);
		}
		return $result;
	}
	
	private function register()
	{
		// No authorization needed
	
		$data = $this->getBodyData();
		$data[$this->users->password] = $this->encryptPassword($data[$this->users->password]);
		$data[$this->users->apiKey] = $this->generateApiKey();
		//throw new ApiException(STATE_DB_ERROR, $data);

		// TODO: Validate fields
		
		// Create user
		$result = $this->dbCreate($data);
		
		// Print response
		http_response_code(200);
		return array(
			$this->users->apiKey => $result[$this->users->apiKey]
		);
	}
	
	protected function createElement()
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$data = $this->getBodyData();
		$data[$this->users->password] = $this->encryptPassword($data[$this->users->password]);
		$data[$this->users->apiKey] = $this->generateApiKey();

		// TODO: Validate fields
		
		// Create user
		$result = $this->dbCreate($data);
		unset($result[$this->users->password]);
		unset($result[$this->users->apiKey]);
		
		// Print response
		http_response_code(201);
		
		return $result;
	}
	
	private function updatePassword($id)
	{
		// Check authorization
		Authorization::authorizeBasic();
		
		$data = $this->getBodyData();
		if(isset($data[$this->users->password])) {
			$data[$this->users->password] = $this->encryptPassword($data[$this->users->password]);
			
				// TODO: Validate fields
			
			// Update password
			return DBCommands::dbUpdate($this->table, [$this->users->password], $this->idField, $id, $data);
			
			// Print response
			http_response_code(204);
			return;
		}

		throw new ApiException(STATE_INVALID_DATA, "Invalid data");
	}
	
	protected function dbUpdate($id, $data)
	{
		$fields = array_diff($this->fields, [$this->users->password, $this->users->apiKey]);
		return DBCommands::dbUpdate($this->table, $fields, $this->idField, $id, $data);
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
		$fields = array(
				Tables::getInstance()->users->id,
				Tables::getInstance()->users->userType,
				Tables::getInstance()->users->apiKey
			);
		
		try {
			return DBCommands::dbGetOne(Tables::getInstance()->users->table, $fields, Tables::getInstance()->users->apiKey, $apiKey);
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
			
			$fields = array(
				Tables::getInstance()->users->id,
				Tables::getInstance()->users->userType,
				Tables::getInstance()->users->password,
				Tables::getInstance()->users->apiKey
			);
			
			try {
				$result = DBCommands::dbGetOne(Tables::getInstance()->users->table, $fields, Tables::getInstance()->users->email, $email);
			
				if(password_verify($password, $result[Tables::getInstance()->users->password])) {
					unset($result[Tables::getInstance()->users->password]);
					return $result;
				}
			} catch (PDOException $e) {
			}
		}
		
		return null;
	}
}