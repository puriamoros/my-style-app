<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/../utilities/Tables.php');
require_once(__DIR__.'/Users.php');
require_once(__DIR__.'/Condition.php');

class Staff extends Users
{
	public function __construct()
    {
		// Staff table data
		$this->staff = Tables::getInstance()->staff;
		
		// Users table data
		$this->users = Tables::getInstance()->users;
		
		// Establishments table data
		$this->establishments = Tables::getInstance()->establishments;
				
		// Fields for extra data
		$this->staffName = 'staffName';		
		
		// Call parent ctor
		parent::__construct();		
    }
	
	public function get($queryArray, $queryParams)
    {
		if(count($queryArray) == 1 &&  isset($queryParams[$this->staff->idEstablishment])) {
			return $this->getElements($queryParams);											
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function post($queryArray)
    {
		if(count($queryArray) == 1) {
			return $this->createElement($queryArray);
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function put($queryArray)
    {
		if(count($queryArray) == 2) {
			return $this->updateElement($queryArray[1]);
		}
				
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function delete($queryArray)
    {
		if(count($queryArray) == 2) {
			return $this->deleteElement($queryArray[1]);
		}
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	protected function dbGet($queryParams)
	{
		$mixedFields = $this->users->fields;
		array_push($mixedFields, $this->staff->idEstablishment);
	
		$result = DBCommands::dbGetJoin(
			[$this->users->table, $this->staff->table],
			[
				[new Condition($this->users->table . '.' . $this->users->id, '=',  $this->staff->table . '.' . $this->staff->idUser, false)]
			],
			['INNER'],
			$mixedFields, [$this->staff->idEstablishment], $queryParams);
		
		for($i=0; $i<count($result); $i++){
			$result[$i][$this->staffName] = $result[$i][$this->users->name] . ' ' . $result[$i][$this->users->surname]; 
		}
		
		return $result;
	}
	
	protected function dbCreate($data)
	{
		if(isset($data[$this->staff->idEstablishment])){
			$result = parent::dbCreate($data);
			$data[$this->staff->idUser] = $result[$this->users->id];
			DBCommands::dbCreateNoId($this->staff->table, $this->staff->fields, $data);
			
			$result[$this->staff->idEstablishment] = $data[$this->staff->idEstablishment];
			$result[$this->staffName] =  $result[$this->users->name] . ' ' . $result[$this->users->surname]; 
			
			return $result;
		}
		
		throw new ApiException(STATE_INVALID_DATA, "idEstablishment not found");
	}
	
	protected function dbUpdate($id, $data)
	{
		$result = parent::dbUpdate($id, $data);
		
		if(isset($data[$this->staff->idEstablishment])){
			DBCommands::dbUpdate($this->staff->table, $this->staff->fields, $this->staff->idUser, $id, $data);
		}
				
		return $result;
	}
	
	protected function dbDelete($id)
	{
		$result = parent::dbDelete($id);
		DBCommands::dbDelete($this->staff->table, $this->staff->idUser, $id);
		
		return $result;
	}
	
	public static function getStaffPlatform($establishment)
	{
		$idOwner = $establishment[Tables::getInstance()->establishments->idOwner];
		$staff = DBCommands::dbGet(
			Tables::getInstance()->staff->table,
			[Tables::getInstance()->staff->idUser],
			[Tables::getInstance()->staff->idEstablishment],
			[Tables::getInstance()->staff->idEstablishment => $establishment[Tables::getInstance()->establishments->id]]);
		array_push($staff, array(Tables::getInstance()->staff->idUser => $idOwner));
		
		$staffIds = array();
		for ($i = 0; $i < count($staff); $i++) {
			array_push($staffIds, $staff[$i][Tables::getInstance()->staff->idUser]);
		}
	
		// Get platform and pushToken
		$fields = array(
			Tables::getInstance()->users->platform,
			Tables::getInstance()->users->pushToken
		);
		$additionalConditions = array(
			new Condition(Tables::getInstance()->users->id, 'in', '(' . implode(',', $staffIds) . ')', false));
			
		return DBCommands::dbGet(Tables::getInstance()->users->table, $fields, [], [], $additionalConditions);
	}
}
	