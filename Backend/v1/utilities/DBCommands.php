<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');

class DBCommands
{
	private static function dbFetchToArray($fields, $fetch) {
		$item = array();
		foreach($fields as $field) {
			$item[$field] = $fetch[$field];
		}
		return $item;
	}
	
	public static function dbGet($table, $fields, $searchFields, $queryParams)
	{
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "SELECT " . implode(",", $fields) .
				" FROM " . $table;
			$where = false;
			foreach($searchFields as $field) {
				if(isset($queryParams[$field])) {
					$command .= $where ? " AND " : " WHERE ";
					$command .= $field . "=?";
					$where = true;
				}
			}

			$query = $pdo->prepare($command);
			
			$count = 1;
			foreach($searchFields as $field) {
				if(isset($queryParams[$field])) {
					$query->bindParam($count, $queryParams[$field]);
					$count++;
				}
			}

			$result = $query->execute();

			if ($result) {
				$list = array();
				while($fetch = $query->fetch()) {
					array_push($list, DBCommands::dbFetchToArray($fields, $fetch));
				}
				return $list;
			} else {
				throw new ApiException(STATE_DB_ERROR, "DB error");
			}
		} catch (PDOException $e) {
			throw new ApiException(STATE_DB_ERROR, "PDO exception");
		}
	}
	
	public static function dbGetOne($table, $fields, $idField, $id)
	{
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "SELECT " . implode(",", $fields) .
				" FROM " . $table .
				" WHERE " . $idField . "=?";
			
			$query = $pdo->prepare($command);

			$query->bindParam(1, $id);

			$result = $query->execute();

			if ($result) {
				if($fetch = $query->fetch()) {
					return DBCommands::dbFetchToArray($fields, $fetch);
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
   
	public static function dbCreate($table, $fields, $idField, $data)
	{
		try {
			$fieldsButId = array_diff($fields, [$idField]);
			$fieldsButIdParams = array_fill(0, count($fieldsButId), "?");
			
			$pdo = DBConnection::getInstance()->getDB();
			
			$command = "INSERT INTO " . $table .
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
				$item[$idField] = $pdo->lastInsertId();
				return $item;
			} else {
				throw new ApiException(STATE_DB_ERROR, "DB error");
			}
		} catch (PDOException $e) {
			throw new ApiException(STATE_DB_ERROR, "PDO exception");
		}
	}
	
	public static function dbUpdate($table, $fields, $idField, $id, $data)
	{
		try {
			$fieldsButId = array_diff($fields, [$idField]);
			
			$pdo = DBConnection::getInstance()->getDB();

			$command = "UPDATE " . $table;
			$comma = false;
			foreach($fieldsButId as $field) {
				if(isset($data[$field])) {
					$command .= $comma ? ", " : " SET ";
					$command .= $field . "=?";
					$comma = true;
				}
			}
			$command .= " WHERE " . $idField . "=?";
			

			$query = $pdo->prepare($command);

			$count = 1;
			foreach($fields as $field) {
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
	
	public static function dbDelete($table, $idField, $id)
	{
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "DELETE FROM " . $table .
				" WHERE " . $idField . "=?";

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