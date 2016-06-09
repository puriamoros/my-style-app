<?php

require_once(__DIR__.'/../utilities/DBCommands.php');

class Translations
{
	private static $instance;

	private function __construct()
    {
        $this->table = 'translations';
		$this->fields = array(
			'id',
			'en',
			'es'
		);
		$this->idField = $this->fields[0];
    }
	
	public static function getInstance()
	{
		if (is_null(self::$instance)) {
		  self::$instance = new self();
		}
		
		return self::$instance;
	}
	
	public function getTranslation($id, $lang) {
		if(strcmp(strtolower($lang), $this->idField) == 0 || !in_array($lang, $this->fields)) {
			return '[to_be_translated]';
		}
		else {
			$result = DBCommands::dbGetOne($this->table, $this->fields, $this->idField, $id);
			return isset($result[$lang]) ? $result[$lang] : '[to_be_translated]';
		}
	}
}