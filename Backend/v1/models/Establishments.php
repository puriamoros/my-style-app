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
    }
	
	public function put($queryArray)
    {
		if(count($queryArray) == 2) {
			return $this->updateElement($queryArray[1]);
		}
		else if(count($queryArray) == 3 && $queryArray[2] == $this->servicesExtraField) {
			return $this->updateServices($queryArray[1]);
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
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
		if(isset($data[$this->servicesExtraField])) {
			$services = $data[$this->servicesExtraField];
			$id = $result[$this->establishments->id];
			$this->dbCreateServices($id, $services);
			
			// add services
			$result[$this->servicesExtraField] = $services;
		}
		
		return $result;
	}
	
	protected function dbCreate($data)
	{
		$result = parent::dbCreate($data);
		return $result;
	}
	
	protected function updateElement($id)
	{
		parent::updateElement($id);
		
		// Update services
		$data = $this->getBodyData();
		if(isset($data[$this->servicesExtraField])) {
			$services = $data[$this->servicesExtraField];
			$this->dbUpdateServices($id, $services);
		}
		
		return;
	}
	
	protected function deleteElement($id)
	{
		parent::deleteElement($id);
		
		// Delete services
		$this->dbDeleteServices($id);
		
		return;
	}
	
	protected function updateServices($idEstablishment)
	{
		$data = $this->getBodyData();
		
		// Check authorization
		$this->authorizeDefault();

		// TODO: Validate fields
		
		// Update
		if(isset($data[$this->servicesExtraField])) {
			$services = $data[$this->servicesExtraField];
			$this->dbUpdateServices($idEstablishment, $services);
		}
		
		// Print response
		http_response_code(204);
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
}