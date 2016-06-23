<?php

require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/../utilities/DBCommands.php');

abstract class ModelWithIdBase
{
	public function __construct($table, $fields, $idField)
	{
		$this->table = $table;
		$this->fields = $fields;
		$this->idField = $idField;
	}

    public function get($queryArray, $queryParams)
    {
		if(count($queryArray) >= 1 && count($queryArray) <= 2) {
			if(count($queryArray) == 1) {
				return $this->getElements($queryParams);
			}
			else {
				return $this->getElement($queryArray[1]);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function post($queryArray)
    {
		if(count($queryArray) == 1) {
			$body = file_get_contents('php://input');
			$data = json_decode($body);
			return $this->createElement($data);
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function put($queryArray)
    {
		if(count($queryArray) == 2) {
			return $this->updateElement($queryArray[1]);
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function delete($queryArray)
    {
		if(count($queryArray) == 2) {
			return $this->deleteElement($queryArray[1]);
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	protected function authorizeDefault()
	{
		return Authorization::authorizeApiKey();
	}
	
	protected function authorizeGetElements()
	{
		return $this->authorizeDefault();
	}
	
	protected function authorizeGetElement()
	{
		return $this->authorizeDefault();
	}
	
	protected function authorizeCreateElement()
	{
		return $this->authorizeDefault();
	}
	
	protected function authorizeUpdateElement()
	{
		return $this->authorizeDefault();
	}
	
	protected function authorizeDeleteElement()
	{
		return $this->authorizeDefault();
	}
	
	protected function getElements($queryParams)
	{
		// Check authorization
		$this->authorizeGetElements();

		// TODO: Validate fields
		
		// Get
		$result = $this->dbGet($queryParams);
		
		// Print response
		http_response_code(200);
		
		return $result;	
	}
	
	protected function getElement($id)
	{
		// Check authorization
		$idUser = $this->authorizeGetElement()[Tables::getInstance()->users->id];
		
		// This is to allow users/me using this function without passing the user id
		if(is_null($id)) {
			$id = $idUser;
		}

		// TODO: Validate fields
		
		// Get
		$result = $this->dbGetOne($id);
		
		// Print response
		http_response_code(200);
		return $result;
	}
	
	protected function createElement()
	{
		// Check authorization
		$this->authorizeCreateElement();
		
		$data = $this->getBodyData();

		// TODO: Validate fields
		
		// Create
		$result = $this->dbCreate($data);
		
		// Print response
		http_response_code(201);
		return $result;
	}
	
	protected function updateElement($id)
	{
		// Check authorization
		$this->authorizeUpdateElement();
		
		$data = $this->getBodyData();

		// TODO: Validate fields
		
		// Update
		$this->dbUpdate($id, $data);
		
		// Print response
		http_response_code(204);
		return;
	}
	
	protected function deleteElement($id)
	{
		// Check authorization
		$this->authorizeDeleteElement();

		// TODO: Validate fields
		
		// Delete
		$result = $this->dbDelete($id);
		
		// Print response
		http_response_code(204);
		return;
	}
	
	protected function getBodyData() {
		$body = file_get_contents('php://input');
		return json_decode($body, true);
	}
	
	private function dbFetchToArray($fetch) {
		$item = array();
		foreach($this->fields as $field) {
			$item[$field] = $fetch[$field];
		}
		return $item;
	}
	
	protected function dbGet($queryParams)
	{
		return DBCommands::dbGet($this->table, $this->fields, $this->fields, $queryParams);
	}
	
	protected function dbGetOne($id)
	{
		return DBCommands::dbGetOne($this->table, $this->fields, $this->idField, $id);
	}
   
	protected function dbCreate($data)
	{
		return DBCommands::dbCreate($this->table, $this->fields, $this->idField, $data);
	}
	
	protected function dbUpdate($id, $data)
	{
		return DBCommands::dbUpdate($this->table, $this->fields, $this->idField, $id, $data);
	}
	
	protected function dbDelete($id)
	{
		return DBCommands::dbDelete($this->table, $this->idField, $id);
	}
}