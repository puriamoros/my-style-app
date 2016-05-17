<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');

abstract class ModelWithIdBase
{
	protected $table;
    protected $fields;
	protected $idField;

    public function get($queryArray, $queryParams)
    {
		if(count($queryArray) >= 1 && count($queryArray) <= 2) {
			if ($queryArray[0] == $this->table) {
				if(count($queryArray) == 1) {
					return $this->getElements($queryParams);
				}
				else {
					return $this->getElement($queryArray[1]);
				}
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function post($queryArray)
    {
		if(count($queryArray) == 1) {
			if ($queryArray[0] == $this->table) {
				$body = file_get_contents('php://input');
				$data = json_decode($body);
				return $this->createElement($data);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function put($queryArray)
    {
		if(count($queryArray) == 2) {
			if ($queryArray[0] == $this->table) {
				return $this->updateElement($queryArray[1]);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function delete($queryArray)
    {
		if(count($queryArray) == 2) {
			if ($queryArray[0] == $this->table) {
				return $this->deleteElement($queryArray[1]);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	protected function getElements($queryParams)
	{
		// Check authorization
		Authorization::authorizeApiKey();

		// TODO: Validate fields
		
		// Get establishments
		$result = $this->dbGet($queryParams);
		
		// Print response
		http_response_code(200);
		
		return $result;	
	}
	
	protected function getElement($id)
	{
		// Check authorization
		Authorization::authorizeApiKey();

		// TODO: Validate fields
		
		// Get establishment
		$result = $this->dbGetOne($id);
		
		// Print response
		http_response_code(200);
		return $result;
	}
	
	protected function createElement()
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$data = $this->getBodyData();

		// TODO: Validate fields
		
		// Create user
		$result = $this->dbCreate($data);
		
		// Print response
		http_response_code(201);
		return $result;
	}
	
	protected function updateElement($id)
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$data = $this->getBodyData();

		// TODO: Validate fields
		
		// Update user
		$this->dbUpdate($id, $data);
		
		// Print response
		http_response_code(204);
		return;
	}
	
	protected function deleteElement($id)
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
	
	protected function getBodyData() {
		$body = file_get_contents('php://input');
		return json_decode($body, true);
	}
	
	private function dbFetchToArray($fetch) {
		$item = array();
		foreach($this->fields as $field) {
			$item[$field] = $fetch[$field];
		}
		return $item;
	}
	
	protected function dbGet($queryParams)
	{
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "SELECT " . implode(",", $this->fields) .
				" FROM " . $this->table;
			$where = false;
			foreach($this->fields as $field) {
				if(isset($queryParams[$field])) {
					$command .= $where ? " AND " : " WHERE ";
					$command .= $field . "=?";
					$where = true;
				}
			}

			$query = $pdo->prepare($command);
			
			$count = 1;
			foreach($this->fields as $field) {
				if(isset($queryParams[$field])) {
					$query->bindParam($count, $queryParams[$field]);
					$count++;
				}
			}

			$result = $query->execute();

			if ($result) {
				$list = array();
				while($fetch = $query->fetch()) {
					array_push($list, $this->dbFetchToArray($fetch));
				}
				return $list;
			} else {
				throw new ApiException(STATE_DB_ERROR, "DB error");
			}
		} catch (PDOException $e) {
			throw new ApiException(STATE_DB_ERROR, "PDO exception");
		}
	}
	
	protected function dbGetOne($id)
	{
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "SELECT " . implode(",", $this->fields) .
				" FROM " . $this->table .
				" WHERE " . $this->idField . "=?";
			
			$query = $pdo->prepare($command);

			$query->bindParam(1, $id);

			$result = $query->execute();

			if ($result) {
				if($fetch = $query->fetch()) {
					return $this->dbFetchToArray($fetch);
				}
				else {
					return null;
				}
			} else {
				throw new ApiException(STATE_DB_ERROR, "DB error");
			}
		} catch (PDOException $e) {
			throw new ApiException(STATE_DB_ERROR, "PDO exception");
		}
	}
   
	protected function dbCreate($data)
	{
		try {
			$fieldsButId = array_diff($this->fields, [$this->idField]);
			$fieldsButIdParams = array_fill(0, count($fieldsButId), "?");
			
			$pdo = DBConnection::getInstance()->getDB();
			
			$command = "INSERT INTO " . $this->table .
				" (" . implode(",", $fieldsButId) . ")" .
				" VALUES(" . implode(",", $fieldsButIdParams) . ")";

			$query = $pdo->prepare($command);

			$count = 1;
			foreach($fieldsButId as $field) {
				$query->bindParam($count, $data[$field]);
				$count++;
			}
			
			$result = $query->execute();

			if ($result) {
				$item = array();
				foreach($fieldsButId as $field) {
					$item[$field] = $data[$field];
				}
				// Override idField with lastInsertId
				$item[$this->idField] = $pdo->lastInsertId();
				return $item;
			} else {
				throw new ApiException(STATE_DB_ERROR, "DB error");
			}
		} catch (PDOException $e) {
			throw new ApiException(STATE_DB_ERROR, "PDO exception");
		}
	}
	
	protected function dbUpdate($id, $data)
	{
		try {
			$fieldsButId = array_diff($this->fields, [$this->idField]);
			
			$pdo = DBConnection::getInstance()->getDB();

			$command = "UPDATE " . $this->table;
			$comma = false;
			foreach($fieldsButId as $field) {
				if(isset($data[$field])) {
					$command .= $comma ? ", " : " SET ";
					$command .= $field . "=?";
					$comma = true;
				}
			}
			$command .= " WHERE " . $this->idField . "=?";
			

			$query = $pdo->prepare($command);

			$count = 1;
			foreach($this->fields as $field) {
				if(isset($data[$field])) {
					$query->bindParam($count, $data[$field]);
					$count++;
				}
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
	
	protected function dbDelete($id)
	{
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "DELETE FROM " . $this->table.
				" WHERE " . $this->idField . "=?";

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
}