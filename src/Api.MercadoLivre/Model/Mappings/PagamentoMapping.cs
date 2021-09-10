using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(c => c.Status)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.ToTable("Pagamentos");

        }
    }
}
