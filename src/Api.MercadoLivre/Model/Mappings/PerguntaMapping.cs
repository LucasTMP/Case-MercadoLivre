using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.Mappings
{
    public class PerguntaMapping : IEntityTypeConfiguration<Pergunta>
    {
        public void Configure(EntityTypeBuilder<Pergunta> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(c => c.Titulo)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.ToTable("Perguntas");
        }
    }
}
