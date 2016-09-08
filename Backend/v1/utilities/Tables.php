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
				'hours2',
				'confirmType',
				'longitude',
				'latitude'
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
			'staff' => array(
				'idUser',
				'idEstablishment'
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
				'pushToken',
				'languageCode'
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
			$this->$key = new \stdClass;
			$this->$key->$table = $key;
			$this->$key->$fields = $value;
			
			foreach($value as $field) {
				$this->$key->$field = $field;
			}
		}
		
		/*$this->appointments->table = 'appointments';
		$this->appointments->fields = array(
			'id',
			'idClient',
			'idEstablishment',
			'idService',
			'date',
			'status',
			'notes'
		);
		$this->appointments->id = 'id';
		$this->appointments->idClient = 'idClient';
		$this->appointments->idEstablishment = 'idEstablishment';
		$this->appointments->idService = 'idService';
		$this->appointments->date = 'date';
		$this->appointments->status = 'status';
		$this->appointments->notes = 'notes';
		
		$this->establishments->table = 'establishments';
		$this->establishments->fields  = array(
			'id',
			'name',
			'address',
			'phone',
			'idEstablishmentType',
			'idOwner',
			'idProvince',
			'concurrence',
			'hours1',
			'hours2',
			'confirmType',
			'longitude',
			'latitude'
		);
		$this->establishments->id = 'id';
		$this->establishments->name = 'name';
		$this->establishments->address = 'address';
		$this->establishments->phone = 'phone';
		$this->establishments->idEstablishmentType = 'idEstablishmentType';
		$this->establishments->idOwner = 'idOwner';
		$this->establishments->idProvince = 'idProvince';
		$this->establishments->concurrence = 'concurrence';
		$this->establishments->hours1 = 'hours1';
		$this->establishments->hours2 = 'hours2';
		$this->establishments->confirmType = 'confirmType';
		$this->establishments->longitude = 'longitude';
		$this->establishments->latitude = 'latitude';
		
		$this->favourites->table = 'favourites';
		$this->favourites->fields =  array(
			'id',
			'idClient',
			'idEstablishment'
		);
		$this->favourites->id = 'id';
		$this->favourites->idClient = 'idClient';
		$this->favourites->idEstablishment = 'idEstablishment';
		
		$this->offer->table = 'offer';
		$this->offer->fields = array(
			'idEstablishment',
			'idService',
			'price'
		);
		$this->offer->idEstablishment = 'idEstablishment';
		$this->offer->idService = 'idService';
		$this->offer->price = 'price';
		
		$this->provinces->table = 'provinces';
		$this->provinces->fields = array(
			'id',
			'name'
		);
		$this->provinces->id = 'id';
		$this->provinces->name = 'name';
		
		$this->service_categories->table = 'service_categories';
		$this->service_categories->fields = array(
			'id',
			'idTranslation',
			'idEstablishmentType'
		);
		$this->service_categories->id = 'id';
		$this->service_categories->idTranslation = 'idTranslation';
		$this->service_categories->idEstablishmentType = 'idEstablishmentType';
		
		$this->services->table = 'services';
		$this->services->fields = array(
			'id',
			'idTranslation',
			'idServiceCategory',
			'duration'
		);
		$this->services->id = 'id';
		$this->services->idTranslation = 'idTranslation';
		$this->services->idServiceCategory = 'idServiceCategory';
		$this->services->duration = 'duration';
		
		$this->staff->table = 'staff';
		$this->staff->fields = array(
			'idUser',
			'idEstablishment'
		);
		$this->staff->idUser = 'idUser';
		$this->staff->idEstablishment = 'idEstablishment';
		
		$this->translations->table = 'translations';
		$this->translations->fields = array(
			'id',
			'en',
			'es'
		);
		$this->translations->id = 'id';
		$this->translations->en = 'en';
		$this->translations->es = 'es';
		
		$this->users->table = 'users';
		$this->users->fields = array(
			'id',
			'name',
			'surname',
			'email',
			'password',
			'apiKey',
			'userType',
			'phone',
			'platform',
			'pushToken',
			'languageCode'
		);
		$this->users->id = 'id';
		$this->users->name = 'name';
		$this->users->surname = 'surname';
		$this->users->email = 'email';
		$this->users->password = 'password';
		$this->users->apiKey = 'apiKey';
		$this->users->userType = 'userType';
		$this->users->phone = 'phone';
		$this->users->platform = 'platform';
		$this->users->pushToken = 'pushToken';
		$this->users->languageCode = 'languageCode';*/
	}

	public static function getInstance()
	{
		if ( is_null( self::$instance ) ) {
			self::$instance = new self();
		}
		return self::$instance;
	}
}