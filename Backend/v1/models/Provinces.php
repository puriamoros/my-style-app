<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/ModelWithIdBase.php');

class Provinces extends ModelWithIdBase
{
	public function __construct()
    {
		// Provinces table data
		$this->provinces = Tables::getInstance()->provinces;
		
        // Data for base class
        $this->table = $this->provinces->table;
		$this->fields = $this->provinces->fields;
		$this->idField = $this->provinces->id;
    }
	
	public function get($queryArray, $queryParams)
    {
		if(count($queryArray) >= 1 && count($queryArray) <= 2) {
			if(count($queryArray) == 1) {
				return parent::getElements($queryParams);
			}
			else {
				throw new ApiException(STATE_INVALID_OPERATION, "Invalid Operation");
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function post($queryArray)
    {
		throw new ApiException(STATE_INVALID_OPERATION, "Invalid Operation");
    }
	
	public function put($queryArray)
    {
		throw new ApiException(STATE_INVALID_OPERATION, "Invalid Operation");
    }
	
	public function delete($queryArray)
    {
		throw new ApiException(STATE_INVALID_OPERATION, "Invalid Operation");
    }
}