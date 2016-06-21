<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/ModelWithIdBase.php');

class Favourites extends ModelWithIdBase
{
	public function __construct()
    {
        $this->table = 'favourites';
		$this->idClient = 'idClient';
		$this->idEstablishment = 'idEstablishment';
		$this->fields = array(
			'id',
			$this->idClient,
			$this->idEstablishment
		);
		$this->idField = $this->fields[0];
		
		// Establishments related tables
		// -------------------------------------
		
		// Establishments
		$this->establishmentsTable = 'establishments';
		$this->establishmentsFields = array(
			'id',
			'name',
			'address',
			'phone',
			'idEstablishmentType',
			'idOwner',
			'idProvince'
		);
		$this->establishmentsIdField = $this->establishmentsFields[0];
		$this->favouritesExtraField = 'idFavourite';
    }
	
	public function get($queryArray, $queryParams)
    {
		if(count($queryArray) >= 1 && count($queryArray) <= 2) {
			if(count($queryArray) == 1 && isset($queryParams[$this->idClient])) {
				return $this->getElements($queryParams);
			}
			else {
				throw new ApiException(STATE_INVALID_OPERATION, "Invalid Operation");
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	protected function dbGet($queryParams)
	{
		$mixedFields = $this->establishmentsFields;
		
		// both tables favourites and establishments have a field called "id", so we need to rename them
		$establismentsIdFieldRenamed = $this->establishmentsTable . "." . $this->establishmentsIdField;
		$favouritesIdFieldRenamed = $this->table . "." . $this->idField;
		
		$mixedFields[0] = $establismentsIdFieldRenamed;
		array_push($mixedFields, $favouritesIdFieldRenamed);
		
		$result = DBCommands::dbGetJoin(
			[$this->table, $this->establishmentsTable],
			[$this->idEstablishment, $this->establishmentsIdField],
			['INNER'],
			$mixedFields, [$this->idClient], $queryParams);
		
		// restore original field names
		for ($i = 0; $i < count($result); $i++) {
			$result[$i][$this->establishmentsIdField] = $result[$i][$establismentsIdFieldRenamed];
			unset($result[$i][$establismentsIdFieldRenamed]);
			
			$result[$i][$this->favouritesExtraField] = $result[$i][$favouritesIdFieldRenamed];
			unset($result[$i][$favouritesIdFieldRenamed]);
		}
		
		return $result;
	}
	
	public function put($queryArray)
    {
		throw new ApiException(STATE_INVALID_OPERATION, "Invalid Operation");
    }
}