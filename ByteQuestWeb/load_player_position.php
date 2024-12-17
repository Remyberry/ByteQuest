<?php
$host = "localhost";
$user = "root";
$password = ""; // Your database password
$database = "unitybytequest";

$conn = new mysqli($host, $user, $password, $database);

if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

if (isset($_POST['username'])) {
    $username = $_POST['username'];

    $query = "SELECT posX, posY FROM player_data WHERE username='$username'";
    $result = $conn->query($query);

    if ($result->num_rows > 0) {
        $data = $result->fetch_assoc();
        echo json_encode($data); // Return position as JSON
    } else {
        echo "No data found";
    }
} else {
    echo "Missing username";
}

$conn->close();
?>
