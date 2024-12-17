<?php
$host = "localhost";
$user = "root";
$password = ""; // Your database password
$database = "unitybytequest";

$conn = new mysqli($host, $user, $password, $database);

if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Check POST data
if (isset($_POST['username']) && isset($_POST['posX']) && isset($_POST['posY'])) {
    $username = $_POST['username'];
    $posX = $_POST['posX'];
    $posY = $_POST['posY'];

    // Check if user data exists
    $checkQuery = "SELECT * FROM player_data WHERE username='$username'";
    $result = $conn->query($checkQuery);
    
    if ($result->num_rows > 0) {
        // Update position
        $updateQuery = "UPDATE player_data SET posX='$posX', posY='$posY' WHERE username='$username'";
        $conn->query($updateQuery);
        echo "Position updated successfully";
    } else {
        // Insert new position
        $insertQuery = "INSERT INTO player_data (username, posX, posY) VALUES ('$username', '$posX', '$posY')";
        $conn->query($insertQuery);
        echo "Position saved successfully";
    }
} else {
    echo "Missing data";
}

$conn->close();
?>
