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
		
		// Services
		$this->servicesTable = 'offer';
		$this->servicesFields = array(
			'idEstablishment',
			'idService',
			'price'
		);
		$this->idEstablishment = $this->servicesFields[0];
    }
	
	protected function getElement($id)
	{
		$result = parent::getElement($id);
		
		// Add services to $result
		if(!is_null($result)) {
			$queryParams = array(
				'idEstablishment' => $id
			);
			$services = $this->dbGetServices($queryParams);
			
			$result['services'] = $services;
		}
		
		return $result;
	}
	
	protected function createElement()
	{
		$result = parent::createElement();
		
		// Create services
		$data = $this->getBodyData();
		$services = $data['services'];
		$id = $result[$this->idField];
		$this->dbCreateServices($id, $services);
		
		return $result;
	}
	
	protected function updateElement($id)
	{
		parent::updateElement($id);
		
		// Update services
		$data = $this->getBodyData();
		$services = $data['services'];
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
}