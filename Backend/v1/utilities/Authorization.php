<?php

require_once(__DIR__.'/../models/Users.php');
require_once(__DIR__.'/../data/StatusCodes.php');

class Authorization
{

    public static function authorizeBasic()
    {
		$headers = apache_request_headers();
		$basic = self::getTokenValueFromHeader("Authorization", "Basic");

		if (!is_null($basic)) {
			$apiKey = Users::validateBasic($basic);
			if(!is_null($apiKey)){
				return $apiKey;
			}
		}
		
		throw new ApiException(STATE_NOT_AUTHORIZED, "Not authorized", 401);
    }
	
	public static function authorizeApiKey()
    {
		$headers = apache_request_headers();
		$apiKey = self::getTokenValueFromHeader("Authorization", "ApiKey");

		if (!is_null($apiKey)) {
			if (Users::validateApiKey($apiKey)) {
				return;
			}
		}
		
		throw new ApiException(STATE_NOT_AUTHORIZED, "Not authorized", 401);
    }
	
	private static function getTokenValueFromHeader($header, $token) {
		$headers = apache_request_headers();

		if (isset($headers[$header])) {

			$headerValue = $headers[$header];
			if(strtolower(substr($headerValue, 0, strlen($token))) === strtolower($token)) {
				return trim(substr($headerValue, strlen($token)));
			}
		}
		
		return null;
	}
}