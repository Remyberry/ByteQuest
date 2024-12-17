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
$user = $_POST['username'];
$email = $_POST['email'];
$pass = $_POST['password'];

// Check if email or username already exists
$sql = "SELECT * FROM players WHERE username='$user' OR email='$email'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    echo "Username or Email already exists";
} else {
    // Hash the password for security
    $hashedPassword = password_hash($pass, PASSWORD_DEFAULT);

    // Insert the new user
    $sql = "INSERT INTO players (username, email, password) VALUES ('$user', '$email', '$hashedPassword')";
    if ($conn->query($sql) === TRUE) {
        echo "Registration successful";
    } else {
        echo "Error: " . $sql . "<br>" . $conn->error;
    }
}

$conn->close();
?>
