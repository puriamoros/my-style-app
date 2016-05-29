<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/ModelWithIdBase.php');

class Appointments extends ModelWithIdBase
{
	public function __construct()
    {
        $this->table = 'appointments';
		$this->fields = array(
			'id',
			'idClient',
			'idEstablishment',
			'idService',
			'date'
		);
		$this->idField = $this->fields[0];
    }
}