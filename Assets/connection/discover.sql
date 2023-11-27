/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

CREATE DATABASE IF NOT EXISTS `discover` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `discover`;

CREATE TABLE IF NOT EXISTS `usuarios` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre_completo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `edad` int DEFAULT NULL,
  `puntos` int DEFAULT NULL,
  `fecha_reg` date DEFAULT NULL,
  `email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `estado` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

INSERT INTO `usuarios` (`id`, `nombre_completo`, `edad`, `puntos`, `fecha_reg`, `email`, `estado`) VALUES
	(1, 'PEDRO R', 12, 5, '2023-11-10', 'PEDROR12@GMAIL.COM', 1),
	(2, 'Sergio E', 14, 4, NULL, NULL, NULL),
	(3, 'Sergio', 22, 7, NULL, 'sergio@gmail.com', NULL),
	(4, 'Diego', 15, 1, NULL, 'diego@gmail.com', NULL),
	(5, 'Prueba', 13, 2, NULL, 'prueba@gmail.com', NULL),
	(6, 'Pedro', 8, 0, NULL, 'peedrito@gmail.com', NULL),
	(7, 'Pepe', 12, 6, NULL, 'pepe@gmail.com', NULL),
	(8, 'Edwin', 22, NULL, NULL, 'edwin@gmail.com', NULL),
	(9, 'Camila', 19, 2, NULL, 'camila@gmail.com', NULL),
	(10, 'Edwin', 21, 14, NULL, 'edwin2@gmail.com', NULL),
	(11, 'Nacho', 12, NULL, NULL, 'nacho@gmail.com', NULL),
	(12, 'Pepito', 15, NULL, NULL, 'pepito@gmail.com', NULL),
	(13, 'Jose', 14, 4, NULL, 'jose@gmail.com', NULL);

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
