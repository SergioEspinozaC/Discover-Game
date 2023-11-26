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

$puntos = $_POST["puntaje"];

// Obtener el ID del último usuario
$getLastUserIdQuery = "SELECT id FROM usuarios ORDER BY id DESC LIMIT 1;";
$getLastUserIdResult = $conn->query($getLastUserIdQuery);

if ($getLastUserIdResult->num_rows > 0) {
    $lastUserIdRow = $getLastUserIdResult->fetch_assoc();
    $lastUserId = $lastUserIdRow["id"];

    // Insertar el puntaje para el último usuario
    $insertPuntajeQuery = "UPDATE usuarios SET puntos = '" . $puntos . "' WHERE id = " . $lastUserId . ";";

    if ($conn->query($insertPuntajeQuery) === TRUE) {
        echo "Puntaje registrado exitosamente para el usuario con ID " . $lastUserId;
    } else {
        echo "Error al registrar el puntaje. Error: " . $insertPuntajeQuery . "<br>" . $conn->error;
    }
} else {
    echo "No se encontraron usuarios";
}

$conn->close();
?>