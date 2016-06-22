<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/../utilities/Tables.php');
require_once(__DIR__.'/ModelWithIdBase.php');

class Appointments extends ModelWithIdBase
{
	public function __construct()
    {
		// Appointments table data
		$this->appointments = Tables::getInstance()->appointments;
		
		// Data for base class
        $this->table = $this->appointments->table;
		$this->fields = $this->appointments->fields;
		$this->idField = $this->appointments->id;
    }
}