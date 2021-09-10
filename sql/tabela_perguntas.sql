CREATE TABLE `perguntas` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Titulo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProdutoId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UsuarioId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreatedAt` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Perguntas_ProdutoId` (`ProdutoId`),
  KEY `IX_Perguntas_UsuarioId` (`UsuarioId`),
  CONSTRAINT `FK_Perguntas_Produtos_ProdutoId` FOREIGN KEY (`ProdutoId`) REFERENCES `produtos` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Perguntas_Usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `usuarios` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci