<?php

require_once(__DIR__.'/views/JsonView.php');
require_once(__DIR__.'/utilities/ApiException.php');
require_once(__DIR__.'/models/Users.php');
require_once(__DIR__.'/data/StatusCodes.php');
require_once(__DIR__.'/models/Establishments.php');
require_once(__DIR__.'/models/Appointments.php');
require_once(__DIR__.'/models/Favourites.php');
require_once(__DIR__.'/models/Provinces.php');
require_once(__DIR__.'/models/ServiceCategories.php');
require_once(__DIR__.'/models/Services.php');
require_once(__DIR__.'/models/Staff.php');

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

parse_str($_SERVER['QUERY_STRING'], $queryParams);
$queryArray = explode("/", $queryParams['PATH_INFO']);
unset($queryParams['PATH_INFO']);

// Get the resource
$resource = $queryArray[0];//array_shift($queryArray);
$knownResources = array(
		"register" => "Users",
		"login" => "Users",
		"users" => "Users",
		"establishments" => "Establishments",
		"appointments" => "Appointments",
		"favourites" => "Favourites",
		"provinces" => "Provinces",
		"servicecategories" => "ServiceCategories",
		"services" => "Services",
		"staff" => "Staff"
	);
	
// Check if the resource exists
if (!array_key_exists($resource, $knownResources)) {
	throw new ApiException(STATE_INVALID_URL, "Invalid URL");
}

// Create class dynamically
$className = $knownResources[$resource];
$instance = new $className();

$method = strtolower($_SERVER['REQUEST_METHOD']);

switch ($method) {
    case 'get':
        $view->printResponse($instance->get($queryArray, $queryParams));
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
