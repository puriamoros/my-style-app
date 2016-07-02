<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/../utilities/Tables.php');
require_once(__DIR__.'/ModelWithIdBase.php');
require_once(__DIR__.'/Condition.php');

class Appointments extends ModelWithIdBase
{
	public function __construct()
    {
		// Appointments table data
		$this->appointments = Tables::getInstance()->appointments;
		
		// Users table data
		$this->users = Tables::getInstance()->users;
		
		// Establishments table data
		$this->establishments = Tables::getInstance()->establishments;
		
		// Offer table data
		$this->offer = Tables::getInstance()->offer;
		
		// Fields for extra data
		$this->fromDate = 'from';
		$this->toDate = 'to';
		$this->establishmentName = 'establishmentName';
		$this->servicePrice = 'servicePrice';
		
		// Call parent ctor
		parent::__construct($this->appointments->table, $this->appointments->fields, $this->appointments->id);
    }
	
	public function get($queryArray, $queryParams)
    {
		if(count($queryArray) >= 1 && count($queryArray) <= 2) {
			if(count($queryArray) == 1) {
				if(isset($queryParams[$this->appointments->idClient]) ||
					isset($queryParams[$this->appointments->idEstablishment]) )
					return $this->getElements($queryParams);
			}
			else {
				//return $this->getElement($queryArray[1]);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	protected function authorizeGetElements($queryParams)
	{
		$user = $this->authorizeDefault();
		
		// TODO: autorizar tambien al owner a ver las citas de sus establecimientos
		if($user[$this->users->id] !== $queryParams[$this->appointments->idClient]) {
			throw new ApiException(STATE_NOT_AUTHORIZED, "Not authorized", 401);
		}
	}
	// TODO: hacer authorize para POST, PUT y DELETE
	
	protected function dbGet($queryParams)
	{
		$additionalConditions = null;
		if(isset($queryParams[$this->fromDate]) && isset($queryParams[$this->toDate])) {
			$additionalConditions = array(
				new Condition($this->appointments->date, '>=', $queryParams[$this->fromDate], true),
				new Condition($this->appointments->date, '<=', $queryParams[$this->toDate], true),
				new Condition($this->appointments->table . '.' . $this->appointments->idService, '=', $this->offer->table . '.' . $this->offer->idService, false)
			);
		}
		
		// tables appointments,  establishments and offer have a field called "id", so we need to rename them
		$appointmentsIdRenamed = $this->appointments->table . '.' . $this->appointments->id;
		$appointmentsIdEstablishmentRenamed = $this->appointments->table . '.' . $this->appointments->idEstablishment;
		$appointmentsIdServiceRenamed = $this->appointments->table . '.' . $this->appointments->idService;
		$mixedFields = $this->appointments->fields;
		$mixedFields[array_search($this->appointments->id, $mixedFields)] = $appointmentsIdRenamed;
		$mixedFields[array_search($this->appointments->idEstablishment, $mixedFields)] = $appointmentsIdEstablishmentRenamed;
		$mixedFields[array_search($this->appointments->idService, $mixedFields)] = $appointmentsIdServiceRenamed;
		array_push($mixedFields, $this->establishments->name);
		array_push($mixedFields, $this->offer->price);
		$result = DBCommands::dbGetJoin(
			[$this->appointments->table, $this->establishments->table, $this->offer->table],
			[$this->appointments->idEstablishment, $this->establishments->id, $this->offer->idEstablishment],
			['INNER', 'INNER'],
			$mixedFields, $mixedFields, $queryParams, $additionalConditions);
		
		// restore original field names
		for ($i = 0; $i < count($result); $i++) {
			$result[$i][$this->appointments->id] = $result[$i][$appointmentsIdRenamed];
			unset($result[$i][$appointmentsIdRenamed]);
			
			$result[$i][$this->appointments->idEstablishment] = $result[$i][$appointmentsIdEstablishmentRenamed];
			unset($result[$i][$appointmentsIdEstablishmentRenamed]);
			
			$result[$i][$this->appointments->idService] = $result[$i][$appointmentsIdServiceRenamed];
			unset($result[$i][$appointmentsIdServiceRenamed]);
			
			$result[$i][$this->establishmentName] = $result[$i][$this->establishments->name];
			unset($result[$i][$this->establishments->name]);
			
			$result[$i][$this->servicePrice] = $result[$i][$this->offer->price];
			unset($result[$i][$this->offer->price]);
		}
		
		return $result;
	}
	
	public function put($queryArray)
    {
		if(count($queryArray) == 3) {
			if($queryArray[2] == 'confirm') {
				return $this->updateElement($queryArray[1]);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	protected function dbUpdate($id, $data)
	{
		return DBCommands::dbUpdate($this->table, [$this->appointments->confirmed], $this->idField, $id, $data);
	}
	
	protected function dbCreate($data)
	{
		$data[$this->appointments->confirmed] = 0;
		return DBCommands::dbCreate($this->table, $this->fields, $this->idField, $data);
	}
}