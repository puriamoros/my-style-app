<?php

class ApiException extends Exception
{
    public $state;

    public function __construct($state, $message, $code = 400)
    {
		http_response_code($code);
        $this->state = $state;
        $this->message = $message;
        $this->code = $code;
    }

}