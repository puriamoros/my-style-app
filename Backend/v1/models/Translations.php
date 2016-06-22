<?php

require_once(__DIR__.'/../utilities/DBCommands.php');

class Translations
{
	private static $instance;

	private function __construct()
    {
        // Translations table data
		$this->translations = Tables::getInstance()->translations;
		
		// Data for base class
        $this->table = $this->translations->table;
		$this->fields = $this->translations->fields;
		$this->idField = $this->translations->id;
    }
	
	public static function getInstance()
	{
		if (is_null(self::$instance)) {
		  self::$instance = new self();
		}
		
		return self::$instance;
	}
	
	public function getTranslation($id, $lang) {
		if(strcmp(strtolower($lang), $this->translations->id) == 0 || !in_array($lang, $this->translations->fields)) {
			return '[to_be_translated]';
		}
		else {
			$result = DBCommands::dbGetOne($this->translations->table, $this->translations->fields, $this->translations->id, $id);
			return isset($result[$lang]) ? $result[$lang] : '[to_be_translated]';
		}
	}
	
	public function getTranslated($table, $fields, $idField, $idTranslation, $translationExtraField, $queryParams)
	{
		$mixedFields = $fields;
		
		$idIndex = array_search($idField, $mixedFields);
		$sameIdField = $idIndex !== false && strcmp(strtolower($this->translations->id), strtolower($idField)) == 0;
		if($sameIdField) {
			$mixedFields[$idIndex] = $table . "." . $idField; // table also have a field called "id"
		}
		
		$fieldsButId = array_diff($this->translations->fields, [$this->translations->id]);
		$lang = isset($queryParams['lang']) && in_array($queryParams['lang'], $fieldsButId) ? $queryParams['lang'] : null;
		if(!is_null($lang)) {
			array_push($mixedFields, $lang);
		}
		
		$result = DBCommands::dbGetJoin(
			[$table, $this->translations->table],
			[$idTranslation, $this->translations->id],
			['INNER'],
			$mixedFields, $mixedFields, $queryParams);
		
		for ($i = 0; $i < count($result); $i++) {
			if($sameIdField) {
				$result[$i][$idField] = $result[$i][$mixedFields[$idIndex]]; // restore original field name
				unset($result[$i][$mixedFields[$idIndex]]);
			}
			unset($result[$i][$idTranslation]);
			
			$translation = '';
			if(isset($result[$i][$lang])) {
				$translation = $result[$i][$lang];
				unset($result[$i][$lang]);
			}

			$result[$i][$translationExtraField] = ($translation !== '') ? $translation : '[to_be_translated]';
		}		
			
		return $result;
	}
}