<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');

class DBCommands
{
	private static function dbFetchToArray($fields, $fetch) {
		$item = array();
		$i = 0;
		foreach($fields as $field) {
			$item[$field] = $fetch[$i];
			$i++;
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
	
	public static function dbGetInnerJoin($table1, $table2, $joinField1, $joinField2, $fields, $searchFields, $queryParams)
	{
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "SELECT " . implode(",", $fields) .
				" FROM " . $table1 . " INNER JOIN " . $table2 .
				" ON " . $table1 . "." . $joinField1 . "=" . $table2 . "." . $joinField2;
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
		$idMap = array();
		$idMap[$idField] = $id;
		return self::dbGetOneMultiId($table, $fields, $idMap);
	}
	
	public static function dbGetOneMultiId($table, $fields, $idMap)
	{
		if(count($idMap) <= 0) {
			throw new ApiException(STATE_DB_ERROR, "DB error");
		}
		
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "SELECT " . implode(",", $fields) .
				" FROM " . $table;
			$where = false;
			foreach($idMap as $key => $value) {
				$command .= $where ? " AND " : " WHERE ";
				$command .= $key . "=?";
				$where = true;
			}
			$query = $pdo->prepare($command);

			$count = 1;
			foreach($idMap as $key => $value) {
				$query->bindParam($count, $value);
				$count++;
			}

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
	
	public static function dbCreateNoId($table, $fields, $data)
	{
		try {
			$fieldsParams = array_fill(0, count($fields), "?");
			
			$pdo = DBConnection::getInstance()->getDB();
			
			$command = "INSERT INTO " . $table .
				" (" . implode(",", $fields) . ")" .
				" VALUES(" . implode(",", $fieldsParams) . ")";

			$query = $pdo->prepare($command);

			$count = 1;
			foreach($fields as $field) {
				$query->bindParam($count, $data[$field]);
				$count++;
			}
			
			$result = $query->execute();

			if ($result) {
				return $data;
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
		$idMap = array();
		$idMap[$idField] = $id;
		self::dbUpdateMultiId($table, $fields, $idMap, $data);
	}
	
	public static function dbUpdateMultiId($table, $fields, $idMap, $data)
	{
		if(count($idMap) <= 0) {
			throw new ApiException(STATE_DB_ERROR, "DB error");
		}
		
		try {
			$idFields = array();
			foreach($idMap as $key => $value) {
				array_push($idFields, $key);
			}
			$fieldsButIds = array_diff($fields, $idFields);
			
			$pdo = DBConnection::getInstance()->getDB();

			$command = "UPDATE " . $table;
			$comma = false;
			foreach($fieldsButIds as $field) {
				if(isset($data[$field])) {
					$command .= $comma ? ", " : " SET ";
					$command .= $field . "=?";
					$comma = true;
				}
			}
			$where = false;
			foreach($idMap as $key => $value) {
				$command .= $where ? " AND " : " WHERE ";
				$command .= $key . "=?";
				$where = true;
			}

			$query = $pdo->prepare($command);

			$count = 1;
			foreach($fields as $field) {
				if(isset($data[$field])) {
					$query->bindParam($count, $data[$field]);
					$count++;
				}
			}
			foreach($idMap as $key => $value) {
				$query->bindParam($count, $value);
				$count++;
			}

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
		$idMap = array();
		$idMap[$idField] = $id;
		self::dbDeleteMultiId($table, $idMap);
	}
	
	public static function dbDeleteMultiId($table, $idMap)
	{
		if(count($idMap) <= 0) {
			throw new ApiException(STATE_DB_ERROR, "DB error");
		}
		
		try {
			$pdo = DBConnection::getInstance()->getDB();

			$command = "DELETE FROM " . $table;
			$where = false;
			foreach($idMap as $key => $value) {
				$command .= $where ? " AND " : " WHERE ";
				$command .= $key . "=?";
				$where = true;
			}

			$query = $pdo->prepare($command);

			$count = 1;
			foreach($idMap as $key => $value) {
				$query->bindParam($count, $value);
				$count++;
			}

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