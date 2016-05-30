<?php

require_once(__DIR__.'/../utilities/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../utilities/Authorization.php');
require_once(__DIR__.'/../utilities/DBCommands.php');

abstract class ModelWithIdBase
{
	protected $table;
    protected $fields;
	protected $idField;

    public function get($queryArray, $queryParams)
    {
		if(count($queryArray) >= 1 && count($queryArray) <= 2) {
			if ($queryArray[0] == $this->table) {
				if(count($queryArray) == 1) {
					return $this->getElements($queryParams);
				}
				else {
					return $this->getElement($queryArray[1]);
				}
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function post($queryArray)
    {
		if(count($queryArray) == 1) {
			if ($queryArray[0] == $this->table) {
				$body = file_get_contents('php://input');
				$data = json_decode($body);
				return $this->createElement($data);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function put($queryArray)
    {
		if(count($queryArray) == 2) {
			if ($queryArray[0] == $this->table) {
				return $this->updateElement($queryArray[1]);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	public function delete($queryArray)
    {
		if(count($queryArray) == 2) {
			if ($queryArray[0] == $this->table) {
				return $this->deleteElement($queryArray[1]);
			}
		}
		
		throw new ApiException(STATE_INVALID_URL, "Invalid URL");
    }
	
	protected function getElements($queryParams)
	{
		// Check authorization
		Authorization::authorizeApiKey();

		// TODO: Validate fields
		
		// Get establishments
		$result = $this->dbGet($queryParams);
		
		// Print response
		http_response_code(200);
		
		return $result;	
	}
	
	protected function getElement($id)
	{
		// Check authorization
		$idUser = Authorization::authorizeApiKey();
		
		// This is to allow users/me using this function without passing the user id
		if(is_null($id)) {
			$id = $idUser;
		}

		// TODO: Validate fields
		
		// Get establishment
		$result = $this->dbGetOne($id);
		
		// Print response
		http_response_code(200);
		return $result;
	}
	
	protected function createElement()
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$data = $this->getBodyData();

		// TODO: Validate fields
		
		// Create user
		$result = $this->dbCreate($data);
		
		// Print response
		http_response_code(201);
		return $result;
	}
	
	protected function updateElement($id)
	{
		// Check authorization
		Authorization::authorizeApiKey();
		
		$data = $this->getBodyData();

		// TODO: Validate fields
		
		// Update user
		$this->dbUpdate($id, $data);
		
		// Print response
		http_response_code(204);
		return;
	}
	
	protected function deleteElement($id)
	{
		// Check authorization
		Authorization::authorizeApiKey();

		// TODO: Validate fields
		
		// Delete user
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