<?php

abstract class ApiView{
    
    // Error code
    public $state;

    public abstract function printResponse($body);
}