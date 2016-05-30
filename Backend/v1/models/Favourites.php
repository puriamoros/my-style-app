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
		$this->fields = array(
			'id',
			'idClient',
			'idEstablishment'
		);
		$this->idField = $this->fields[0];
    }
	
	public function get($queryArray, $queryParams)
    {
		if(count($queryArray) >= 1 && count($queryArray) <= 2) {
			if ($queryArray[0] == $this->table) {
				if(count($queryArray) == 1) {
					return parent::getElements($queryParams);
				}
				else {
					throw new ApiException(STATE_INVALID_OPERATION, "Invalid Operation");
				}
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function put($queryArray)
    {
		throw new ApiException(STATE_INVALID_OPERATION, "Invalid Operation");
    }
}