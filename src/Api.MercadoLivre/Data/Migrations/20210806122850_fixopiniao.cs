using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.MercadoLivre.Data.Migrations
{
    public partial class fixopiniao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opnioes_Produtos_ProdutoId",
                table: "Opnioes");

            migrationBuilder.DropForeignKey(
                name: "FK_Opnioes_Usuarios_UsuarioId",
                table: "Opnioes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Opnioes",
                table: "Opnioes");

            migrationBuilder.RenameTable(
                name: "Opnioes",
                newName: "Opinioes");

            migrationBuilder.RenameIndex(
                name: "IX_Opnioes_UsuarioId",
                table: "Opinioes",
                newName: "IX_Opinioes_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Opnioes_ProdutoId",
                table: "Opinioes",
                newName: "IX_Opinioes_ProdutoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Opinioes",
                table: "Opinioes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Opinioes_Produtos_ProdutoId",
                table: "Opinioes",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opinioes_Usuarios_UsuarioId",
                table: "Opinioes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opinioes_Produtos_ProdutoId",
                table: "Opinioes");

            migrationBuilder.DropForeignKey(
                name: "FK_Opinioes_Usuarios_UsuarioId",
                table: "Opinioes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Opinioes",
                table: "Opinioes");

            migrationBuilder.RenameTable(
                name: "Opinioes",
                newName: "Opnioes");

            migrationBuilder.RenameIndex(
                name: "IX_Opinioes_UsuarioId",
                table: "Opnioes",
                newName: "IX_Opnioes_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Opinioes_ProdutoId",
                table: "Opnioes",
                newName: "IX_Opnioes_ProdutoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Opnioes",
                table: "Opnioes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Opnioes_Produtos_ProdutoId",
                table: "Opnioes",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opnioes_Usuarios_UsuarioId",
                table: "Opnioes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
