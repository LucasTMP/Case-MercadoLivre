CREATE TABLE `compras` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ProdutoId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Quantidade` int NOT NULL,
  `Valor` decimal(10,2) NOT NULL,
  `Status` int NOT NULL,
  `GatewayPagamento` int NOT NULL,
  `Comprador` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `UpdatedAt` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Compras_Comprador` (`Comprador`),
  KEY `IX_Compras_ProdutoId` (`ProdutoId`),
  CONSTRAINT `FK_Compras_Produtos_ProdutoId` FOREIGN KEY (`ProdutoId`) REFERENCES `produtos` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Compras_Usuarios_Comprador` FOREIGN KEY (`Comprador`) REFERENCES `usuarios` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci