<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "unitybytequest";

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Get input data
$user = $_POST['username']; // Can be username or email
$pass = $_POST['password'];

// Find user by username or email
$sql = "SELECT * FROM players WHERE username='$user'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    $row = $result->fetch_assoc();
    if (password_verify($pass, $row['password'])) {
        echo "Login successful";
    } else {
        echo "Incorrect password";
    }
} else {
    echo "No account found with that Username or Email";
}

$conn->close();
?>
