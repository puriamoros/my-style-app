<?php


class Tables
{
	private static $instance;
	
	private function __construct()
	{
		// Definition of tables and fields
		// This is used to dynamically generate fields (see bellow)
		$tables = array(
			'appointments' => array(
				'id',
				'idClient',
				'idEstablishment',
				'idService',
				'date',
				'status',
				'notes'
			),
			'establishments' => array(
				'id',
				'name',
				'address',
				'phone',
				'idEstablishmentType',
				'idOwner',
				'idProvince',
				'concurrence',
				'hours1',
				'hours2'
			),
			'favourites' => array(
				'id',
				'idClient',
				'idEstablishment'
			),
			'offer' => array(
				'idEstablishment',
				'idService',
				'price'
			),
			'provinces' => array(
				'id',
				'name'
			),
			'service_categories' => array(
				'id',
				'idTranslation',
				'idEstablishmentType'
			),
			'services' => array(
				'id',
				'idTranslation',
				'idServiceCategory',
				'duration'
			),
			'translations' => array(
				'id',
				'en',
				'es'
			),
			'users' => array(
				'id',
				'name',
				'surname',
				'email',
				'password',
				'apiKey',
				'userType',
				'phone',
				'platform',
				'pushToken'
			)
		);
		
		// Dynamic generation of fields
		// For each table:
		// 		- Table name: $this->{table_name}->table
		// 		- Table fields: $this->{table_name}->fields
		// For each field:
		// 		- Field name: $this->{table_name}->{field_name}
		$table = 'table';
		$fields = 'fields';
		foreach($tables as $key => $value) {
			$this->$key->$table = $key;
			$this->$key->$fields = $value;
			
			foreach($value as $field) {
				$this->$key->$field = $field;
			}
		}
	}

	public static function getInstance()
	{
		if ( is_null( self::$instance ) ) {
			self::$instance = new self();
		}
		return self::$instance;
	}
}