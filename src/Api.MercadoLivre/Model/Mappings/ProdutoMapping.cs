using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(c => c.Valor)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(c => c.Quantidade)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(c => c.Descricao)
               .IsRequired()
               .HasColumnType("varchar(1000)");

            builder.Property(c => c.CreatedAt)
               .IsRequired()
               .HasColumnType("datetime");

            builder.ToTable("Produtos");

        }
    }
}
