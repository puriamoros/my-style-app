<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/ModelWithIdBase.php');
require_once(__DIR__.'/Translations.php');

class ServiceCategories extends ModelWithIdBase
{
	public function __construct()
    {
		// ServiceCategories table data
		$this->serviceCategories = Tables::getInstance()->service_categories;
		
		// Call parent ctor
		parent::__construct($this->serviceCategories->table, $this->serviceCategories->fields, $this->serviceCategories->id);
		
		// Translations table data
		$this->translations = Tables::getInstance()->translations;
		
		// Fields for extra data
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
			$this->serviceCategories->table, $this->serviceCategories->fields, $this->serviceCategories->id, $this->serviceCategories->idTranslation, $this->translationExtraField, $queryParams);
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