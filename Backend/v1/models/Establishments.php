<?php

require_once(__DIR__.'/../data/DBConnection.php');
require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/ModelWithIdBase.php');

class Establishments extends ModelWithIdBase
{
	public function __construct()
    {
        $this->table = 'establishments';
		$this->fields = array(
			'id',
			'name',
			'address'
		);
		$this->idField = $this->fields[0];
    }
}