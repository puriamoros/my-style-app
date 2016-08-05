<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/../utilities/Tables.php');
require_once(__DIR__.'/ModelWithIdBase.php');
require_once(__DIR__.'/Condition.php');

class Favourites extends ModelWithIdBase
{
	public function __construct()
    {
		// Favourites table data
		$this->favourites = Tables::getInstance()->favourites;
		
		// Call parent ctor
		parent::__construct($this->favourites->table, $this->favourites->fields, $this->favourites->id);
		
		// Establishments table data
		$this->establishments = Tables::getInstance()->establishments;
		
		// Fields for extra data
		$this->favouritesExtraField = 'idFavourite';
    }
	
	public function get($queryArray, $queryParams)
    {
		if(count($queryArray) >= 1 && count($queryArray) <= 2) {
			if(count($queryArray) == 1 && isset($queryParams[$this->favourites->idClient])) {
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
		$mixedFields = $this->establishments->fields;
		
		// both tables favourites and establishments have a field called "id", so we need to rename them
		$establismentsIdFieldRenamed = $this->establishments->table . "." . $this->establishments->id;
		$favouritesIdFieldRenamed = $this->favourites->table . "." . $this->favourites->id;
		
		$mixedFields[0] = $establismentsIdFieldRenamed;
		array_push($mixedFields, $favouritesIdFieldRenamed);
		
		$result = DBCommands::dbGetJoin(
			[$this->favourites->table, $this->establishments->table],
			[
				[new Condition($this->favourites->table . "." . $this->favourites->idEstablishment, '=', $this->establishments->table . "." . $this->establishments->id, false)]
			],
			['INNER'],
			$mixedFields, [$this->favourites->idClient], $queryParams);
		
		// restore original field names
		for ($i = 0; $i < count($result); $i++) {
			$result[$i][$this->establishments->id] = $result[$i][$establismentsIdFieldRenamed];
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