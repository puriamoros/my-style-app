<?php 

require_once(__DIR__.'/ApiException.php');
require_once(__DIR__.'/../data/StatusCodes.php');
require_once(__DIR__.'/../data/PushConstants.php');
require_once(__DIR__.'/../data/ModelConstants.php');
require_once(__DIR__.'/../models/Users.php');
require_once(__DIR__.'/../models/Staff.php');
require_once(__DIR__.'/Tables.php');

// Server file
class PushNotifications {

	public static function NotifyUser($idUser, $title, $body, $context)
	{
		$result = Users::getUserPlatform($idUser);
		if(!is_null($result)) {
			$platform = $result[Tables::getInstance()->users->platform];
			$pushToken = $result[Tables::getInstance()->users->pushToken];
			self::Notify($platform, $pushToken, $title, $body, $context);
		}
	}
	
	public static function NotifyEstablishment($establisment, $title, $body, $context)
	{
		$results = Staff::getStaffPlatform($establisment);
		foreach($results as $result) {
			$platform = $result[Tables::getInstance()->users->platform];
			$pushToken = $result[Tables::getInstance()->users->pushToken];
			self::Notify($platform, $pushToken, $title, $body, $context);
		}
	}
	
	private static function Notify($platform, $pushToken, $title, $body, $context)
	{
		$method = null;
		if($platform === PLATFORM_WINDOWS || $platform === PLATFORM_WIN_PHONE) {
			$method = 'windows';
		}
		else if($platform === PLATFORM_ANDROID) {
			$method = 'android';
		}
		else if($platform === PLATFORM_IOS) {
			$method = 'iOS';
		}
		
		if(!is_null($method))
		{
			self::$method($pushToken, $title, $body, $context);
		}
	}
	
	private static function iOS($clientToken, $title, $body, $context)
	{
		// Since it is necessary to have an apple development account in order to use push
		// notifications, we won't implement them on iOS
	}
		
	private static function android($clientToken, $title, $body, $context)
	{
		$url = 'https://gcm-http.googleapis.com/gcm/send';
		$message = array(
			'to' => $clientToken,
			'data' => array(
				'title' => $title,
				'body' => $body,
				'context' => $context
			)
		);
		
		$json = json_encode($message);
		$headers = array(
			'Authorization: key=' . ANDROID_SERVER_API_KEY,
			'Content-Type: application/json',
			'Content-Length: ' . strlen($json)
		);

		return self::useCurl($url, $headers, $json);
	}
	
	private static function windows($clientUri, $title, $body, $context)
	{
		$toastNotification ='<?xml version="1.0" encoding="utf-16"?>'.
		'<toast launch="'. $context .'">'.
			'<visual lang="en-US">'.
				'<binding template="ToastText02">'.
					  '<text id="1">' . $title . '</text>'.
					  '<text id="2">' . $body . '</text>'.
				'</binding>'.
			'</visual>'.
			'<context>' . $context . '</context>'.
		'</toast>';
            
		$token= self::getWindowsToken();
		$headers = array(
			'Content-Type: text/xml',
			'Content-Length: ' . strlen($toastNotification),
			'X-WNS-Type: wns/toast',
			'Authorization: Bearer ' . $token
		);
		
		$json = self::useCurl($clientUri, $headers, $toastNotification);
	}
	 
	private static function getWindowsToken()
	{
		$encSid = urlencode(WP_PACKAGE_SID);
		$encSecret = urlencode(WP_SECRET);

		$url ='https://login.live.com/accesstoken.srf';
		$body =
			'grant_type=client_credentials&client_id=' . $encSid .
			'&client_secret=' . $encSecret . '&scope=notify.windows.com';
		$headers = array(
			'Content-Type: application/x-www-form-urlencoded',
			'Content-Length: ' . strlen($body)
		);

		$json = self::useCurl($url, $headers, $body);
		$obj = json_decode($json);
		return $obj->{'access_token'};
	}
	
	private static function useCurl($url, $headers, $body = null)
	{
		if ($url) {
			$ch = curl_init($url);
			curl_setopt($ch, CURLOPT_POST, true);
			curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
			curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
			if ($body) {
				curl_setopt($ch, CURLOPT_POSTFIELDS, $body);
			}
			// Disabling SSL Certificate support temporarily
			curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
	 
			// Execute post
			$response = curl_exec($ch);
			$info = curl_getinfo($ch);
	 
			// Close connection
			curl_close($ch);

			//throw new ApiException(STATE_INVALID_URL, $info['http_code']);
			//throw new ApiException(STATE_INVALID_URL, $response);
			return $response;
        }
    }
}
?>