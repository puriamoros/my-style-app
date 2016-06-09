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
        $this->table = 'service_categories';
		$this->fields = array(
			'id',
			'idTranslation',
			'idEstablishmentType'
		);
		$this->idField = $this->fields[0];
		
		$this->idTranslation = $this->fields[1];
		$this->translationField = 'name';
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
	
	protected function getElements($queryParams)
	{
		// Get
		$result = parent::getElements($queryParams);
		
		$lang = isset($queryParams['lang']) ? $queryParams['lang'] : 'none';
		for ($i = 0; $i < count($result); $i++) {
			if(isset($result[$i][$this->idTranslation])) {
				$result[$i][$this->translationField] = Translations::getInstance()->getTranslation($result[$i][$this->idTranslation], $lang);
				unset($result[$i][$this->idTranslation]);
			}
		}
		
		return $result;	
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