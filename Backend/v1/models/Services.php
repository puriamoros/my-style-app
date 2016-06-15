<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/ModelWithIdBase.php');
require_once(__DIR__.'/Translations.php');

class Services extends ModelWithIdBase
{
	public function __construct()
    {
        $this->table = 'services';
		$this->fields = array(
			'id',
			'idTranslation',
			'idServiceCategory',
			'duration'
		);
		$this->idField = $this->fields[0];
		
		$this->idTranslation = $this->fields[1];
		$this->translationExtraField = 'name';
    }
	
	public function get($queryArray, $queryParams)
    {
		if(count($queryArray) >= 1 && count($queryArray) <= 2) {
			if(count($queryArray) == 1) {
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
		return Translations::getInstance()->getTranslated(
			$this->table, $this->fields, $this->idField, $this->idTranslation, $this->translationExtraField, $queryParams);
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