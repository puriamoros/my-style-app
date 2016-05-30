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
    }
	
	protected function getElement($id)
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$result = parent::dbGetOne($id);
		
		$queryParams = array(
			'idEstablishment' => $id
		);
		$services = $this->dbGetServices($queryParams);
		
		$result['services'] = $services;
		
		// Print response
		http_response_code(200);
		return $result;
	}
	
	private function dbGetServices($queryParams)
	{
		$table = 'offer';
		$fields = array(
			'idService',
			'price'
		);
		$searchFields = array(
			'idEstablishment'
		);
		
		return DBCommands::dbGet($table, $fields, $searchFields, $queryParams);
	}
}