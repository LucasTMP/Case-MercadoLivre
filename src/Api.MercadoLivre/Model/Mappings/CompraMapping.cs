using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.Mappings
{
    public class CompraMapping : IEntityTypeConfiguration<Compra>
    {
        public void Configure(EntityTypeBuilder<Compra> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(c => c.Quantidade)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(c => c.Valor)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(c => c.Status)
                .IsRequired();

            builder.Property(c => c.GatewayPagamento)
               .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(c => c.UpdatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.HasOne<Usuario>(s => s.Usuario)
            .WithMany()
            .HasForeignKey(ad => ad.Comprador);

            builder.ToTable("Compras");

        }
    }
}
