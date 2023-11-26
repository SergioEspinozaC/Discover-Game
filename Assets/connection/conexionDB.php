<?php
$servername = "localhost";
$dbusername = "root";
$dbpassword = "";
$dbname = "discover";

// Create connection
$conn = new mysqli($servername, $dbusername, $dbpassword, $dbname);
// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$nombre_completo = $_POST["nombre_completo"];
$edad = $_POST["edad"];
$email = $_POST["email"];


$sql = "INSERT INTO usuarios (nombre_completo, edad, email) VALUES ('" . $nombre_completo . "', '" . $edad . "', '" . $email . "');";

if ($conn->query($sql) === TRUE) {
  echo "New record created successfully";
} else {
  echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();
?>