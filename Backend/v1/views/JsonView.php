<?php

require_once(__DIR__.'/ApiView.php');

/**
 * Class to print responses with JSON format
 */
class JsonView extends ApiView
{
    /**
     * Prints the response body and sets the response code
     * @param mixed $body the body to send
     */
    public function printResponse($body)
    {
        header('Content-Type: application/json; charset=utf8');
        echo json_encode($body, JSON_PRETTY_PRINT);
        exit;
    }
}