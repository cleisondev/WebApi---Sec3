using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi___Sec3.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) " +
                "Values('Coca-Cola Diet','Refrigerante',5.45,'Cocacola.jpg',50,getdate(),1)");

            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) " +
                "Values('Lanche de Atum','Atum com maionese',8.45,'atum.jpg',10,getdate(),2)");

            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) " +
                "Values('Pudim','Pudim 100g',10.45,'pudim.jpg',5,getdate(),3)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Produtos");
        }
    }
}
