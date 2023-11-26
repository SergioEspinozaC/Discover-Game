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

// Consulta SQL para obtener el top 3 de usuarios por puntaje
$sqlTop3 = "SELECT nombre_completo, edad, puntos FROM usuarios ORDER BY puntos DESC LIMIT 3";

// Consulta SQL para obtener el último usuario registrado
$sqlUltimoUsuario = "SELECT nombre_completo, edad, puntos FROM usuarios ORDER BY id DESC LIMIT 1";

// Consulta SQL para obtener el cuarto usuario si no está en el top 3
$sqlCuartoUsuario = "SELECT nombre_completo, edad, puntos FROM usuarios ORDER BY puntos DESC LIMIT 1 OFFSET 3";

// Unir ambas consultas usando UNION
$sqlCombinado = "($sqlTop3) UNION ($sqlUltimoUsuario)";

// Consulta final para obtener el top 3 y el último usuario (o el top 4 si el último usuario no está en el top 3)
$sqlFinal = "SELECT * FROM (($sqlCombinado) UNION ($sqlCuartoUsuario)) AS combinedTable ORDER BY puntos DESC";

$result = $conn->query($sqlFinal);

// Verificar si hay resultados
if ($result->num_rows > 0) {
    // Almacenar resultados en un array asociativo
    $data = array();
    while ($row = $result->fetch_assoc()) {
        $data[] = implode(",", $row);
    }

    // Imprimir los datos como texto
    echo implode(",", $data);
} else {
    echo "0 resultados";
}

// Cerrar conexión
$conn->close();
?>