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
		
		// Services table data
		$this->services = Tables::getInstance()->services;
		
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
		$additionalConditions = array(
			// This condition is necessary for join
			new Condition($this->appointments->table . '.' . $this->appointments->idService, '=', $this->offer->table . '.' . $this->offer->idService, false));
			
		if(isset($queryParams[$this->fromDate])) {
			array_push($additionalConditions, new Condition($this->appointments->date, '>=', $queryParams[$this->fromDate], true));
		}
		if(isset($queryParams[$this->toDate])) {
			array_push($additionalConditions, new Condition($this->appointments->date, '<=', $queryParams[$this->toDate], true));
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
		
		for ($i = 0; $i < count($result); $i++) {
			// restore original field names
			$result[$i][$this->appointments->id] = $result[$i][$appointmentsIdRenamed];
			unset($result[$i][$appointmentsIdRenamed]);
			
			$result[$i][$this->appointments->idEstablishment] = $result[$i][$appointmentsIdEstablishmentRenamed];
			unset($result[$i][$appointmentsIdEstablishmentRenamed]);
			
			$result[$i][$this->appointments->idService] = $result[$i][$appointmentsIdServiceRenamed];
			unset($result[$i][$appointmentsIdServiceRenamed]);
			
			// modify names
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
			if($queryArray[2] == 'status') {
				return $this->updateElement($queryArray[1]);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	protected function dbUpdate($id, $data)
	{
		return DBCommands::dbUpdate($this->appointments->table, [$this->appointments->status], $this->idField, $id, $data);
	}
	
	private function checkCanCreate($data) {
		$establishment = DBCommands::dbGetOne($this->establishments->table, $this->establishments->fields, $this->establishments->id, $data[$this->appointments->idEstablishment]);
		if(is_null($establishment)) {
			throw new ApiException(STATE_DB_ERROR, "Establishment not found");
		}
		
		$service = DBCommands::dbGetOne($this->services->table, $this->services->fields, $this->services->id, $data[$this->appointments->idService]);
		if(is_null($service)) {
			throw new ApiException(STATE_DB_ERROR, "Service not found");
		}
		
		$concurrence = $establishment[$this->establishments->concurrence];
		$duration = $establishment[$this->services->duration];
		$openingHours = array();
		
		$hours1 = $establishment[$this->establishments->hours1];
		$hours2 = $establishment[$this->establishments->hours2];
		$intervals = array();
		if(isset($hours1) && trim($hours1) !== '') {
			array_push($intervals, $hours1);
		}
		if(isset($hours2) && trim($hours2) !== '') {
			array_push($intervals, $hours2);
		}
		
		foreach($intervals as $interval) {
			$splitInterval = explode('-', $interval);
			
			if(count($splitInterval) != 2) {
				throw new ApiException(STATE_DB_ERROR, "Erroneous establishment data");
			}
			
			foreach($splitInterval as $hourStr) {
				$splitHour = explode(':', $hourStr);
			
				if(count($splitHour) != 2) {
					throw new ApiException(STATE_DB_ERROR, "Erroneous establishment data");
				}
				
				$hour = (int) $splitHour[0];
				$minute = (int) $splitHour[1];
				
				if ($hour < 0 || $hour > 23 || ($minute != 0 && $minute != 30))
				{
					throw new ApiException(STATE_DB_ERROR, "Erroneous establishment data");
				}
				
				array_push($openingHours, array(
					'hour' => $hour,
					'minute' => $minute));
					
				throw new ApiException(STATE_DB_ERROR, [$hour,$minute]);
			}
			
			throw new ApiException(STATE_DB_ERROR, $openingHours);
		}
		
		/*$queryParams = array(
			$this->appointments->idEstablishment => $data[$this->appointments->idEstablishment],
			$this->appointments->date => $data[$this->appointments->date]
		);
		$sameDateAppointments = DBCommands::dbGet($this->appointments->table, $this->appointments->fields, $this->appointments->fields, $queryParams);
		
		// TODO: check if there is enough time
		if(count($sameDateAppointments) >= $establishment[$this->establishments->concurrence]) {
			throw new ApiException(STATE_ESTABLISHMENT_FULL, "Establishment full");
		}*/
	}
	
	protected function dbCreate($data)
	{
		$this->checkCanCreate($data);
		$data[$this->appointments->status] = 0;
		return DBCommands::dbCreate($this->appointments->table, $this->appointments->fields, $this->appointments->id, $data);
	}
}