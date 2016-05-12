<?php

require_once(__DIR__.'/views/JsonView.php');
require_once(__DIR__.'/utilities/ApiException.php');
require_once(__DIR__.'/models/Users.php');
require_once(__DIR__.'/data/StatusCodes.php');

$view = new JsonView();

set_exception_handler(function ($exception) use ($view) {
    $body = array(
        "state" => $exception->state,
        "message" => $exception->getMessage()
    );
    if ($exception->getCode()) {
        $view->state = $exception->getCode();
    } else {
        $view->state = 500;
    }

    $view->printResponse($body);
}
);

// Get the resource
$queryArray = explode("/", $_GET['PATH_INFO']);
$resource = $queryArray[0];//array_shift($queryArray);
$knownResources = array(
		"register" => "Users",
		"login" => "Users",
		"users" => "Users"
	);
	
// Check if the resource exists
if (!array_key_exists($resource, $knownResources)) {
	throw new ApiException(STATE_INVALID_URL, "Invalid URL");
}

// Create class dinamically
$className = $knownResources[$resource];
$instance = new $className();

$method = strtolower($_SERVER['REQUEST_METHOD']);

switch ($method) {
    case 'get':
        $view->printResponse($instance->get($queryArray));
        break;

    case 'post':
        $view->printResponse($instance->post($queryArray));
        break;
		
    case 'put':
        $view->printResponse($instance->put($queryArray));
        break;

    case 'delete':
        $view->printResponse($instance->delete($queryArray));
        break;
		
    default:
        throw new ApiException(STATE_INVALID_METHOD, "Invalid method", 405);
}
