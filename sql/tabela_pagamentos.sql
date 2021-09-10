CREATE TABLE `pagamentos` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CompraId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `PagamentoGatewayId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Status` int NOT NULL,
  `CreatedAt` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Pagamentos_CompraId` (`CompraId`),
  CONSTRAINT `FK_Pagamentos_Compras_CompraId` FOREIGN KEY (`CompraId`) REFERENCES `compras` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci