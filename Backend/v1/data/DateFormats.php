<?php

class DateFormats
{
	private static $instance;
	
	private function __construct()
	{
		$this->formats = array(
			'default' => 'd/m/Y H:i',
			'en' => 'd/m/Y H:i',
			'es' => 'd/m/Y H:i'
		);
	}
	
	public static function getInstance()
	{
		if ( is_null( self::$instance ) ) {
			self::$instance = new self();
		}
		return self::$instance;
	}
	
	public function getDateFormat($locale)
	{
		if(isset($this->formats[$locale])) {
			return $this->formats[$locale];
		}
		else {
			return $this->formats['default'];
		}
	}
}