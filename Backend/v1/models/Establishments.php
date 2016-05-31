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
			'establishmentType',
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
		// Check authorization
		Authorization::authorizeApiKey();
		
		$result = $this->dbGetOne($id);
		
		if(!is_null($result)) {
			$queryParams = array(
				'idEstablishment' => $id
			);
			$services = $this->dbGetServices($queryParams);
			
			$result['services'] = $services;
		}
		
		// Print response
		http_response_code(200);
		return $result;
	}
	
	protected function createElement()
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$data = $this->getBodyData();
		$services = $data['services'];
		unset($data['services']);

		// TODO: Validate fields
		
		// Create
		$result = $this->dbCreate($data);
		
		// Create services
		$id = $result[$this->idField];
		$this->dbCreateServices($id, $services);
		
		// Print response
		http_response_code(201);
		return $result;
	}
	
	protected function updateElement($id)
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$data = $this->getBodyData();
		$services = $data['services'];
		unset($data['services']);

		// TODO: Validate fields
		
		// Update
		$this->dbUpdate($id, $data);
		
		// Update services
		$this->dbUpdateServices($id, $services);
		
		// Print response
		http_response_code(204);
		return;
	}
	
	protected function deleteElement($id)
	{
		// Check authorization
		Authorization::authorizeApiKey();

		// TODO: Validate fields
		
		// Delete
		$result = $this->dbDelete($id);
		
		// Delete services
		$this->dbDeleteServices($id);
		
		// Print response
		http_response_code(204);
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