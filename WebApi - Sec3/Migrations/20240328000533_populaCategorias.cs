using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi___Sec3.Migrations
{
    /// <inheritdoc />
    public partial class populaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorias(Nome, ImagemURl) Values('Bebidas','Bebidas.jpg')");
            mb.Sql("Insert into Categorias(Nome, ImagemURl) Values('Sobremesa','Sobremesa.jpg')");
            mb.Sql("Insert into Categorias(Nome, ImagemURl) Values('Lanches','Lanches.jpg')");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
        }
    }
}
