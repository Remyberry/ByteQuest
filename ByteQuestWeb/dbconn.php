<?php
$host = "localhost";   // Database host (change if necessary)
$user = "root";        // Database user
$password = "";        // Database password (set your MySQL password)
$dbname = "unitybytequest"; // Database name

// Create connection
$conn = new mysqli($host, $user, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}
?>
