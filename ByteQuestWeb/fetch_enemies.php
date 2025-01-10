<?php
// Database credentials (replace with your actual credentials)
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "unitybytequest";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Prepare the SQL query
$sql = "SELECT * FROM enemies";
$result = $conn->query($sql);

$enemiesData = [];

if ($result->num_rows > 0) {
    // Output data of each row
    while($row = $result->fetch_assoc()) {
        $enemiesData[] = $row;
    }
} else {
    echo "No enemies found";
}

$conn->close();

// Encode the data as JSON
$enemiesJson = json_encode($enemiesData);

// Set the content type header
header('Content-Type: application/json');

// Print the JSON data
echo $enemiesJson;
?>