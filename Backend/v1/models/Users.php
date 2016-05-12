<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');

class Users
{
    // Data from table "users"
    const TABLE_NAME = "users";
	const ID = "id";
    const EMAIL = "email";
    const PASSWORD = "password";
	const API_KEY = "apiKey";

    public function get($queryArray)
    {
		if(count($queryArray >= 1)) {
			if ($queryArray[0] == 'login') {
				return $this->login();
			} else if ($queryArray[0] == 'users') {
				if(count($queryArray == 1)) {
					return $this->getUsers();
				}
				else {
					return $this->getUser($queryArray[1]);
				}
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function post($queryArray)
    {
		if(count($queryArray >= 1)) {
			if ($queryArray[0] == 'register') {
				return $this->register();
			} else if ($queryArray[0] == 'users') {
				return $this->createUser();
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function put($queryArray)
    {
		if(count($queryArray >= 2)) {
			if ($queryArray[0] == 'users') {
				return $this->updateUser($queryArray[1]);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function del($queryArray)
    {
		if(count($queryArray >= 2)) {
			if ($queryArray[0] == 'users') {
				return $this->deleteUser($queryArray[1]);
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
	
	private function getUsers()
	{
	}
	
	private function getUser($email)
	{
	}
	
	private function register()
	{
		$body = file_get_contents('php://input');
		$user = json_decode($body);

		// TODO: Validate fields
		
		// Create user
		$result = $this->create($user);
		
		// Print response
		http_response_code(200);
		return array(
				"apiKey" => $result["apiKey"]
			);
	}
	
	private function createUser()
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$body = file_get_contents('php://input');
		$user = json_decode($body);

		// TODO: Validate fields
		
		// Create user
		$result = $this->create($user);
		
		// Print response
		http_response_code(201);
		return $result;
	}
	
	private function updateUser($email)
	{
	}
	
	private function deleteUser($email)
	{
	}
   
	private function create($userData)
	{
		$email = $userData->email;
		$password = $userData->password;
		$encryptedPassword = self::encryptPassword($password);

		$apiKey = $this->generateApiKey();

		try {

			$pdo = DBConnection::getInstance()->getDB();

			$command = "INSERT INTO " . self::TABLE_NAME . " ( " .
				self::EMAIL . "," .
				self::PASSWORD . "," .
				self::API_KEY . ")" .
				" VALUES(?,?,?)";

			$query = $pdo->prepare($command);

			$query->bindParam(1, $email);
			$query->bindParam(2, $encryptedPassword);
			$query->bindParam(3, $apiKey);

			$result = $query->execute();

			if ($result) {
				return array(
					"id" => $pdo->lastInsertId(),
					"email" => $email,
					"apiKey" => $apiKey
				);
			} else {
				throw new ApiException(STATE_CREATION_ERROR, "Creation error");
			}
		} catch (PDOException $e) {
			throw new ApiException(STATE_DB_ERROR, "PDO exception");
		}

	}
	
	private static function encryptPassword($plainPassword)
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
		$command = "SELECT COUNT(" . self::ID . ")" .
			" FROM " . self::TABLE_NAME .
			" WHERE " . self::API_KEY . "=?";

		$query = DBConnection::getInstance()->getDB()->prepare($command);

		$query->bindParam(1, $apiKey);

		$query->execute();

		
		return $query->fetchColumn(0) > 0;
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