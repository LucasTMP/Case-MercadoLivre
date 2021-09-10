using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.Mappings
{
    public class OpiniaoMapping : IEntityTypeConfiguration<Opiniao>
    {
        public void Configure(EntityTypeBuilder<Opiniao> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(c => c.Nota)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(c => c.Titulo)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(c => c.Descricao)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.ToTable("Opinioes");
        }
    }
}
