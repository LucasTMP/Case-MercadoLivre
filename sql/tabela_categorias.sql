CREATE TABLE `categorias` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Nome` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CategoriaPrincipalId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Nome_UNIQUE` (`Nome`),
  KEY `FK_Categorias_Categorias_CategoriaPrincipalId` (`CategoriaPrincipalId`),
  CONSTRAINT `FK_Categorias_Categorias_CategoriaPrincipalId` FOREIGN KEY (`CategoriaPrincipalId`) REFERENCES `categorias` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci