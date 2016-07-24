<?php

require_once(__DIR__.'/../utilities/DBCommands.php');
require_once(__DIR__.'/Condition.php');

class Translations
{
	private static $instance;

	private function __construct()
    {
        // Translations table data
		$this->translations = Tables::getInstance()->translations;
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
	
	public function getTranslationsMap($idKeyMap) {
		$ids = array();
		foreach($idKeyMap as $key => $value) {
			array_push($ids, $key);
		}
	
		$additionalConditions = array(
			new Condition($this->translations->id, 'in', '(' . implode(',', $ids) . ')', false));
		$results = DBCommands::dbGet($this->translations->table, $this->translations->fields, [], [], $additionalConditions);
		
		$translatedMap = array();
		foreach($results as $result) {
			$key = $idKeyMap[$result[$this->translations->id]];
			$value = $result;
			unset($value[$this->translations->id]);
			$translatedMap[$key] = $value;
		}
		
		return $translatedMap;
	}
	
	public function getTranslated($table, $fields, $idField, $idTranslation, $translationExtraField, $queryParams)
	{
		$mixedFields = $fields;
		
		$idFieldRenamed = null;
		$idIndex = array_search($idField, $mixedFields);
		$sameIdField = $idIndex !== false && strcmp(strtolower($this->translations->id), strtolower($idField)) == 0;
		if($sameIdField) {
			// table also have a field called "id"
			$idFieldRenamed = $table . '.' . $idField;
			$mixedFields[$idIndex] = $idFieldRenamed;
		}
		
		$fieldsButId = array_diff($this->translations->fields, [$this->translations->id]);
		$lang = isset($queryParams['lang']) && in_array($queryParams['lang'], $fieldsButId) ? $queryParams['lang'] : null;
		if(!is_null($lang)) {
			array_push($mixedFields, $lang);
		}
		
		$result = DBCommands::dbGetJoin(
			[$table, $this->translations->table],
			[
				[new Condition($table . '.' . $idTranslation, '=',  $this->translations->table. '.' . $this->translations->id, false)]
			],
			['INNER'],
			$mixedFields, $mixedFields, $queryParams);
		
		for ($i = 0; $i < count($result); $i++) {
			if($sameIdField) {
				// restore original field name
				$result[$i][$idField] = $result[$i][$idFieldRenamed];
				unset($result[$i][$idFieldRenamed]);
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