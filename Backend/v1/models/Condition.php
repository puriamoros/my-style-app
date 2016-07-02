<?php

class Condition
{
	public function __construct($field, $operator, $value, $doBindParam)
	{
		$this->field = $field;
		$this->operator = $operator;
		$this->value = $value;
		$this->doBindParam = $doBindParam;
	}
}