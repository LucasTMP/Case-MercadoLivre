using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(c => c.Login)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(c => c.Senha)
                .IsRequired()
                .HasColumnType("varchar(60)");

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.ToTable("Usuarios");


            builder.HasIndex(u => u.Login)
            .IsUnique();
        }
    }
}
