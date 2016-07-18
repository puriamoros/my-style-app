<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/../utilities/Tables.php');
require_once(__DIR__.'/ModelWithIdBase.php');
require_once(__DIR__.'/Condition.php');

class Establishments extends ModelWithIdBase
{
	public function __construct()
    {
		// Establishments table data
		$this->establishments = Tables::getInstance()->establishments;
		
		// Call parent ctor
		parent::__construct($this->establishments->table, $this->establishments->fields, $this->establishments->id);
		
		// Offer table data
		$this->offer = Tables::getInstance()->offer;
		
		// Favourites table data
		$this->favourites = Tables::getInstance()->favourites;
		
		// Fields for extra data
		$this->servicesExtraField = 'services';
		$this->favouritesExtraField = 'idFavourite';
		
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
		if(isset($queryParams[$this->offer->idService]) && isset($queryParams[$this->favourites->idClient])) {
			// Request is looking for establishments offering a specific service => join with table offer
			// We also need to know if the establishment is a favourite of the user requesting the info => join with table favourites
			
			// both tables favourites and establishments have a field called "id", so we need to rename them
			$establismentsIdFieldRenamed = $this->establishments->table . '.' . $this->establishments->id;
			$favouritesIdFieldRenamed = $this->favourites->table . '.' . $this->favourites->id;
			
			$mixedFields = $this->establishments->fields;
			$mixedFields[0] = $establismentsIdFieldRenamed;
			array_push($mixedFields, $favouritesIdFieldRenamed);
			array_push($mixedFields, $this->offer->price);
			$searchFields = $mixedFields;
			array_push($searchFields, $this->offer->idService);
			$result = DBCommands::dbGetJoin(
				[$this->establishments->table, $this->offer->table, $this->favourites->table],
				[
					[new Condition($this->establishments->table . '.' . $this->establishments->id, '=', $this->offer->table . '.' . $this->offer->idEstablishment, false)],
					[new Condition($this->offer->table . '.' . $this->offer->idEstablishment, '=', $this->favourites->table . '.' . $this->favourites->idEstablishment, false),
					new Condition($this->favourites->table . '.' . $this->favourites->idClient, '=', $queryParams[$this->favourites->idClient], true)]
				],
				['INNER', 'LEFT'],
				$mixedFields, $searchFields, $queryParams);
				
			// restore original field names
			for ($i = 0; $i < count($result); $i++) {
				$result[$i][$this->establishments->id] = $result[$i][$establismentsIdFieldRenamed];
				unset($result[$i][$establismentsIdFieldRenamed]);
				
				// idFavourite can be null => set it to 0 if it is null
				$result[$i][$this->favouritesExtraField] = is_null($result[$i][$favouritesIdFieldRenamed]) ? '0' : $result[$i][$favouritesIdFieldRenamed];
				unset($result[$i][$favouritesIdFieldRenamed]);
				
				// move idService and price to services
				$service = array(
					$this->offer->idService => $queryParams[$this->offer->idService],
					$this->offer->price => $result[$i][$this->offer->price]
				);
				unset($result[$i][$this->offer->price]);
				$result[$i][$this->servicesExtraField] = [$service];
			}
			
			return $result;
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
				$this->offer->idEstablishment => $id
			);
			$services = $this->dbGetServices($queryParams);
			
			$result[$this->servicesExtraField] = $services;
		}
		
		return $result;
	}
	
	protected function dbGetOne($id)
	{
		// We also need to know if the establishment is a favourite => join with table favourites
			
		// both tables favourites and establishments have a field called "id", so we need to rename them
		$establismentsIdFieldRenamed = $this->establishments->table . '.' . $this->establishments->id;
		$favouritesIdFieldRenamed = $this->favourites->table . '.' . $this->favourites->id;
		
		$mixedFields = $this->establishments->fields;
		$mixedFields[0] = $establismentsIdFieldRenamed;
		array_push($mixedFields, $favouritesIdFieldRenamed);
		
		$result = DBCommands::dbGetOneJoin(
			[$this->establishments->table, $this->favourites->table],
			[
				[new Condition($this->establishments->table . '.' . $this->establishments->id, '=', $this->favourites->table . '.' . $this->favourites->idEstablishment, false)]
			],
			['LEFT'],
			$mixedFields, $establismentsIdFieldRenamed, $id);
			
		// restore original field names
		$result[$this->establishments->id] = $result[$establismentsIdFieldRenamed];
		unset($result[$establismentsIdFieldRenamed]);
		// idFavourite can be null => set it to 0 if it is null
		$result[$this->favouritesExtraField] = is_null($result[$favouritesIdFieldRenamed]) ? '0' : $result[$favouritesIdFieldRenamed];
		unset($result[$favouritesIdFieldRenamed]);
			
		return $result;
	}
	
	protected function createElement()
	{
		$result = parent::createElement();
		
		// Create services
		$data = $this->getBodyData();
		$services = $data[$this->servicesExtraField];
		$id = $result[$this->establishments->id];
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
		$servicesFieldsButId = array_diff($this->offer->fields, [$this->offer->idEstablishment]);
		$searchFields = array(
			$this->offer->idEstablishment
		);
		
		return DBCommands::dbGet($this->offer->table, $servicesFieldsButId, $searchFields, $queryParams);
	}
	
	private function dbCreateServices($idEstablishment, $services)
	{
		foreach($services as $service) {
			$data = $service;
			$data[$this->offer->idEstablishment] = $idEstablishment;
			DBCommands::dbCreateNoId($this->offer->table, $this->offer->fields, $data);
		}
	}
	
	private function dbUpdateServices($idEstablishment, $services)
	{
		$this->dbDeleteServices($idEstablishment);
		$this->dbCreateServices($idEstablishment, $services);
	}
	
	private function dbDeleteServices($idEstablishment)
	{
		DBCommands::dbDelete($this->offer->table, $this->offer->idEstablishment, $idEstablishment);
	}
	
	/*private function dbGetEmployees($queryParams)
	{
		$servicesFieldsButId = array_diff($this->offer->fields, [$this->offer->idEstablishment]);
		$searchFields = array(
			$this->offer->idEstablishment
		);
		
		return DBCommands::dbGet($this->offer->table, $servicesFieldsButId, $searchFields, $queryParams);
	}
	
	private function dbCreateEmployee($idEstablishment, $services)
	{
		foreach($services as $service) {
			$data = $service;
			$data[$this->offer->idEstablishment] = $idEstablishment;
			DBCommands::dbCreateNoId($this->offer->table, $this->offer->fields, $data);
		}
	}
	
	private function dbUpdateEmployee($idEstablishment, $services)
	{
		$this->dbDeleteServices($idEstablishment);
		$this->dbCreateServices($idEstablishment, $services);
	}
	
	private function dbDeleteEmployee($idEstablishment, $idUser)
	{
		DBCommands::dbDelete($this->staffTable, $this->offer->idEstablishment, $idEstablishment);
	}*/
}