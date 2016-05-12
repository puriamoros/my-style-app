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
		if(count($queryArray) >= 1) {
			if ($queryArray[0] == 'login') {
				return $this->login();
			} else if ($queryArray[0] == 'users') {
				if(count($queryArray) == 1) {
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
		if(count($queryArray) >= 1) {
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
		if(count($queryArray) >= 2) {
			if ($queryArray[0] == 'users') {
				return $this->updateUser($queryArray[1]);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function delete($queryArray)
    {
		if(count($queryArray) >= 2) {
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
		// Check authorization
		Authorization::authorizeApiKey();

		// TODO: Validate fields
		
		// Get user
		$result = $this->dbGet();
		
		// Print response
		http_response_code(200);
		
		$list = array();
		foreach($result as $resultItem) {
			$listItem = array(
				"id" => $resultItem["id"],
				"email" => $resultItem["email"]
			);
			array_push($list, $listItem);
		}
		return $list;	
	}
	
	private function getUser($id)
	{
		// Check authorization
		Authorization::authorizeApiKey();

		// TODO: Validate fields
		
		// Get user
		$result = $this->dbGetOne($id);
		
		// Print response
		http_response_code(200);
		return array(
			"id" => $result["id"],
			"email" => $result["email"]
		);
	}
	
	private function register()
	{
		// No authorization needed
	
		$body = file_get_contents('php://input');
		$user = json_decode($body);

		// TODO: Validate fields
		
		// Create user
		$result = $this->dbCreate($user);
		
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
		$data = json_decode($body);

		// TODO: Validate fields
		
		// Create user
		$result = $this->dbCreate($data);
		
		// Print response
		http_response_code(201);
		return array(
			"id" => $result["id"],
			"email" => $result["email"]
		);
	}
	
	private function updateUser($id)
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$body = file_get_contents('php://input');
		$data = json_decode($body);

		// TODO: Validate fields
		
		// Update user
		$this->dbUpdate($id, $data);
		
		// Print response
		http_response_code(204);
		return;
	}
	
	private function deleteUser($id)
	{
		// Check authorization
		Authorization::authorizeApiKey();

		// TODO: Validate fields
		
		// Delete user
		$result = $this->dbDelete($id);
		
		// Print response
		http_response_code(204);
		return;
	}
	
	private function dbGet()
	{
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "SELECT * FROM " . self::TABLE_NAME;

			$query = $pdo->prepare($command);

			$result = $query->execute();

			if ($result) {
				$list = array();
				while($result = $query->fetch()) {
					$listItem = array(
						"id" => $result[self::ID],
						"email" => $result[self::EMAIL],
						"apiKey" => $result[self::API_KEY]
					);
					array_push($list, $listItem);
				}
				return $list;
			} else {
				throw new ApiException(STATE_DB_ERROR, "DB error");
			}
		} catch (PDOException $e) {
			throw new ApiException(STATE_DB_ERROR, "PDO exception");
		}
	}
	
	private function dbGetOne($id)
	{
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "SELECT * FROM " . self::TABLE_NAME .
				" WHERE " . self::ID . "=?";

			$query = $pdo->prepare($command);

			$query->bindParam(1, $id);

			$result = $query->execute();

			if ($result) {
				$result = $query->fetch();
				return array(
					"id" => $result[self::ID],
					"email" => $result[self::EMAIL],
					"apiKey" => $result[self::API_KEY]
				);
			} else {
				throw new ApiException(STATE_DB_ERROR, "DB error");
			}
		} catch (PDOException $e) {
			throw new ApiException(STATE_DB_ERROR, "PDO exception");
		}
	}
   
	private function dbCreate($data)
	{
		try {
			$pdo = DBConnection::getInstance()->getDB();
			$apiKey = $this->generateApiKey();

			$command = "INSERT INTO " . self::TABLE_NAME . " ( " .
				self::EMAIL . "," .
				self::PASSWORD . "," .
				self::API_KEY . ")" .
				" VALUES(?,?,?)";

			$query = $pdo->prepare($command);

			$query->bindParam(1, $data->email);
			$query->bindParam(2, self::encryptPassword($data->password));
			$query->bindParam(3, $apiKey);

			$result = $query->execute();

			if ($result) {
				return array(
					"id" => $pdo->lastInsertId(),
					"email" => $data->email,
					"apiKey" => $apiKey
				);
			} else {
				throw new ApiException(STATE_DB_ERROR, "DB error");
			}
		} catch (PDOException $e) {
			throw new ApiException(STATE_DB_ERROR, "PDO exception");
		}
	}
	
	private function dbUpdate($id, $data)
	{
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "UPDATE " . self::TABLE_NAME;
			$comma = false;
			if(isset($data->email)) {
				$command .= $comma ? ", " : " SET ";
				$command .= self::EMAIL . "=?";
				$comma = true;
			}
			if(isset($data->password)) {
				$command .= $comma ? ", " : " SET ";
				$command .= self::PASSWORD . "=?";
				$comma = true;
			}
			$command .= " WHERE " . self::ID . "=?";
			

			$query = $pdo->prepare($command);

			$count = 1;
			if(isset($data->email)) {
				$query->bindParam($count, $data->email);
				$count++;
			}
			if(isset($data->password)) {
				$query->bindParam($count, self::encryptPassword($data->password));
				$count++;
			}
			$query->bindParam($count, $id);

			$result = $query->execute();

			if ($result) {
				return;
			} else {
				throw new ApiException(STATE_DB_ERROR, "DB error");
			}
		} catch (PDOException $e) {
			throw new ApiException(STATE_DB_ERROR, "PDO exception");
		}
	}
	
	private function dbDelete($id)
	{
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "DELETE FROM " . self::TABLE_NAME .
				" WHERE " . self::ID . "=?";

			$query = $pdo->prepare($command);

			$query->bindParam(1, $id);

			$result = $query->execute();

			if ($result) {
				return;
			} else {
				throw new ApiException(STATE_DB_ERROR, "DB error");
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