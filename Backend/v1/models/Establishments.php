<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/ModelWithIdBase.php');

class Establishments extends ModelWithIdBase
{
	public function __construct()
    {
        $this->table = 'establishments';
		$this->fields = array(
			'id',
			'name',
			'address',
			'idEstablishmentType',
			'idOwner',
			'idProvince'
		);
		$this->idField = $this->fields[0];
		
		// Establishments related tables
		// -------------------------------------
		$this->idEstablishment = 'idEstablishment';
		$this->idService = 'idService';
		
		// Services
		$this->servicesTable = 'offer';
		$this->servicesFields = array(
			$this->idEstablishment,
			$this->idService,
			'price'
		);
		$this->servicesExtraField = 'services';
		
		// Staff
		/*$this->staffTable = 'staff';
		$this->staffFields = array(
			$this->idEstablishment,
			'idUser'
		);*/
    }
	
	/*public function get($queryArray, $queryParams)
    {
		if(count($queryArray) >= 3 && count($queryArray) <= 4) {
			// Staff
			if ($queryArray[2] == $this->staffTable) {
				if(count($queryArray) == 3) {
					return $this->getEmployees($queryArray[1], $queryParams);
				}
				else {
					return $this->getEmployee($queryArray[1], $queryArray[3]);
				}
			}
		}
		
		// Establishments
		return parent::get($queryArray, $queryParams);
    }*/
	
	protected function dbGet($queryParams)
	{
		if(isset($queryParams[$this->idService])) {
			// Request is looking for establishments offering a specific service => join with table offer
			$mixedFields = $this->fields;
			array_push($mixedFields, $this->idService);
			return DBCommands::dbGetInnerJoin($this->table, $this->servicesTable, $this->idField, $this->idEstablishment, $this->fields, $mixedFields, $queryParams);
		}
		else
		{
			return parent::dbGet($queryParams);
		}
	}
	
	protected function getElement($id)
	{
		$result = parent::getElement($id);
		
		// Add services to $result
		if(!is_null($result)) {
			$queryParams = array(
				$this->idEstablishment => $id
			);
			$services = $this->dbGetServices($queryParams);
			
			$result[$this->servicesExtraField] = $services;
		}
		
		return $result;
	}
	
	protected function createElement()
	{
		$result = parent::createElement();
		
		// Create services
		$data = $this->getBodyData();
		$services = $data[$this->servicesExtraField];
		$id = $result[$this->idField];
		$this->dbCreateServices($id, $services);
		
		return $result;
	}
	
	protected function updateElement($id)
	{
		parent::updateElement($id);
		
		// Update services
		$data = $this->getBodyData();
		$services = $data[$this->servicesExtraField];
		$this->dbUpdateServices($id, $services);
		
		return;
	}
	
	protected function deleteElement($id)
	{
		parent::deleteElement($id);
		
		// Delete services
		$this->dbDeleteServices($id);
		
		return;
	}
	
	private function dbGetServices($queryParams)
	{
		$servicesFieldsButId = array_diff($this->servicesFields, [$this->idEstablishment]);
		$searchFields = array(
			$this->idEstablishment
		);
		
		return DBCommands::dbGet($this->servicesTable, $servicesFieldsButId, $searchFields, $queryParams);
	}
	
	private function dbCreateServices($idEstablishment, $services)
	{
		foreach($services as $service) {
			$data = $service;
			$data[$this->idEstablishment] = $idEstablishment;
			DBCommands::dbCreateNoId($this->servicesTable, $this->servicesFields, $data);
		}
	}
	
	private function dbUpdateServices($idEstablishment, $services)
	{
		$this->dbDeleteServices($idEstablishment);
		$this->dbCreateServices($idEstablishment, $services);
	}
	
	private function dbDeleteServices($idEstablishment)
	{
		DBCommands::dbDelete($this->servicesTable, $this->idEstablishment, $idEstablishment);
	}
	
	private function dbGetEmployees($queryParams)
	{
		$servicesFieldsButId = array_diff($this->servicesFields, [$this->idEstablishment]);
		$searchFields = array(
			$this->idEstablishment
		);
		
		return DBCommands::dbGet($this->servicesTable, $servicesFieldsButId, $searchFields, $queryParams);
	}
	
	private function dbCreateEmployee($idEstablishment, $services)
	{
		foreach($services as $service) {
			$data = $service;
			$data[$this->idEstablishment] = $idEstablishment;
			DBCommands::dbCreateNoId($this->servicesTable, $this->servicesFields, $data);
		}
	}
	
	private function dbUpdateEmployee($idEstablishment, $services)
	{
		$this->dbDeleteServices($idEstablishment);
		$this->dbCreateServices($idEstablishment, $services);
	}
	
	private function dbDeleteEmployee($idEstablishment, $idUser)
	{
		DBCommands::dbDelete($this->staffTable, $this->idEstablishment, $idEstablishment);
	}
}