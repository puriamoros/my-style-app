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
		$this->checkCanUpdate($id, $data);
		return DBCommands::dbUpdate($this->appointments->table, [$this->appointments->status], $this->idField, $id, $data);
	}
	
	protected function dbCreate($data)
	{
		$this->checkCanCreate($data);
		$data[$this->appointments->status] = 0;
		return DBCommands::dbCreate($this->appointments->table, $this->appointments->fields, $this->appointments->id, $data);
	}
	
	private function checkCanUpdate($id, $data) {
		$status = $data[$this->appointments->status];
		if($status == 2) { // Cancel status
			$appointment = DBCommands::dbGetOne($this->appointments->table, $this->appointments->fields, $this->appointments->id, $id);
			if(is_null($appointment)) {
				throw new ApiException(STATE_DB_ERROR, "Appointment not found");
			}
			
			$date = $appointment[$this->appointments->date];
			$intervalInMinutes = (date_create($date)->getTimestamp() - date_create()->getTimestamp()) / 60;
			
			if($intervalInMinutes < 24*60) {
				throw new ApiException(STATE_APPOINTMENT_CANCELLATION_ERROR, "Too late to cancel the appointment");
			}
		}
	}
	
	private function checkCanCreate($data) {
		$establishment = DBCommands::dbGetOne($this->establishments->table, $this->establishments->fields, $this->establishments->id, $data[$this->appointments->idEstablishment]);
		if(is_null($establishment)) {
			throw new ApiException(STATE_DB_ERROR, "Establishment not found");
		}
		
		$offer = DBCommands::dbGetOneMultiId($this->offer->table, $this->offer->fields,
			[$this->offer->idEstablishment => $data[$this->appointments->idEstablishment], $this->offer->idService => $data[$this->appointments->idService]]);
		if(is_null($offer)) {	
			throw new ApiException(STATE_DB_ERROR, "Establishment not offering the service");
		}
		
		$service = DBCommands::dbGetOne($this->services->table, $this->services->fields, $this->services->id, $data[$this->appointments->idService]);
		if(is_null($service)) {
			throw new ApiException(STATE_DB_ERROR, "Service not found");
		}
		
		$date = $data[$this->appointments->date];
		$concurrence = (int) $establishment[$this->establishments->concurrence];
		$duration = (int) $service[$this->services->duration];
		$slotsNeeded = $duration / 30;
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
			}
		}
		
		$openingDates = array();
		for ($i = 0; $i < count($openingHours)-1; $i+=2) {
			$from = date_create($date);
			date_time_set($from, $openingHours[$i]['hour'], $openingHours[i]['minute']);
			$to = date_create($date);
			date_time_set($to, $openingHours[$i+1]['hour'], $openingHours[i+1]['minute']);
			
			array_push($openingDates, array(
				'from' => $from,
				'to' => $to
			));
		}
		
		$start = date_create($date);
		date_time_set($start, 0, 0);
		$end = date_create($date);
		date_time_set($end, 23, 59, 59);
		
		$fields = [$this->appointments->date, 'COUNT(*)'];
		$groupBy = [$this->appointments->date];
		$queryParams = array(
			$this->appointments->idEstablishment => $data[$this->appointments->idEstablishment]
		);
		$additionalConditions = array(
			new Condition($this->appointments->date, '>=', date_format($start, 'Y-m-d H:i:s'), true),
			new Condition($this->appointments->date, '<=', date_format($end, 'Y-m-d H:i:s'), true),
			new Condition($this->appointments->status, '<>', 2, true));
		$appointmentsGrouped = DBCommands::dbGet($this->appointments->table, $fields, $this->appointments->fields, $queryParams, $additionalConditions, $groupBy);
		
		$appointmentMap = array();
		foreach($appointmentsGrouped as $group) {
			$appointmentMap[$group[$this->appointments->date]] = $group['COUNT(*)'];
		}
		
		$slots = array();
		foreach($openingDates as $openingDate) {
			$formDate = clone $openingDate['from'];
			$toDate = clone $openingDate['to'];
			while($formDate < $toDate){
				$count = 0;
				if(isset($appointmentMap[date_format($formDate, 'Y-m-d H:i:s')])) {
					$count = $appointmentMap[date_format($formDate, 'Y-m-d H:i:s')];
				}
				$slots[date_format($formDate, 'Y-m-d H:i:s')] = (int) $count;
				date_add($formDate, date_interval_create_from_date_string('30 minutes'));
			}
		}
		
		$slotsKeys = array_keys($slots);
		$index = array_search($date, $slotsKeys);
		if($index === false) {
			throw new ApiException(STATE_ESTABLISHMENT_CLOSED, 'The establishment is closed at that hour');
		}
		
		$enoughTime = ($index+$slotsNeeded <= count($slots));
		for ($j = $index; $j < count($slots) && ($j - $index) < $slotsNeeded && $enoughTime; $j++) {
			 $enoughConcurrence = ($slots[$slotsKeys[$j]] < $concurrence);
			 $prevSlotTogether = ($j == $index || 
				((date_create($slotsKeys[$j])->getTimestamp() - date_create($slotsKeys[$j-1])->getTimestamp()) / 60) === 30);
			
			if (!$enoughConcurrence || !$prevSlotTogether)
			{
				$enoughTime = false;
			}
		}
		
		if(!$enoughTime) {
			throw new ApiException(STATE_ESTABLISHMENT_FULL, 'The establishment is full at that hour interval');
		}
	}
}