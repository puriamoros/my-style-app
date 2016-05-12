<?php
/**
 * Class managing PDO instance
 */

require_once(__DIR__.'/LoginMySql.php');


class DBConnection
{

    /**
     * Single instance of the class
     */
    private static $db = null;

    /**
     * PDO instance
     */
    private static $pdo;

    final private function __construct()
    {
        try {
            // Create a new PDO connection
            self::getDB();
        } catch (PDOException $e) {
            // Exceptions
        }


    }

    /**
     * Gets a single instance of the class
     * @return DBConnection|null
     */
    public static function getInstance()
    {
        if (self::$db === null) {
            self::$db = new self();
        }
        return self::$db;
    }

    /**
     * Creates a new PDO connection
     * using the connection constants
	 * defined in login_mysql.php
     * @return PDO Object
     */
    public function getDB()
    {
        if (self::$pdo == null) {
            self::$pdo = new PDO(
                'mysql:dbname=' . DB_NAME .
                ';host=' . HOST_NAME . ";",
                USER,
                PASSWORD,
                array(PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES utf8")
            );

            // Enable exceptions
            self::$pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        }

        return self::$pdo;
    }

    /**
     * Disables object conning
     */
    final protected function __clone()
    {
    }

    function _destructor()
    {
        self::$pdo = null;
    }
}