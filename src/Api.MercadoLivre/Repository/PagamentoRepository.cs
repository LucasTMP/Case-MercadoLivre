using Api.MercadoLivre.Data;
using Api.MercadoLivre.Model;
using Api.MercadoLivre.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Repository
{
    public class PagamentoRepository : Repository<Pagamento>
    {
        public PagamentoRepository(ApiDbContext db) : base(db)
        {
        }

        public async Task<Pagamento> BuscarTransacaoSucesso(Guid id)
        {
            return await DbSet.AsNoTrackingWithIdentityResolution().Where(o => o.PagamentoGatewayId == id && o.Status == StatusPagamento.SUCESSO).FirstOrDefaultAsync();
        }

        public async Task<Pagamento> BuscarCompraPaga(Guid id)
        {
            return await DbSet.AsNoTrackingWithIdentityResolution().Where(o => o.CompraId == id && o.Status == StatusPagamento.SUCESSO).FirstOrDefaultAsync();
        }

        public async Task<Pagamento> BuscarPagamentoCompraProdutoUsuario(Guid id)
        {
            return await DbSet.AsNoTrackingWithIdentityResolution().Where(o => o.CompraId == id).Include(o => o.Compra)
                                                                    .ThenInclude(o => o.Usuario)
                                                                    .Include(o => o.Compra)
                                                                    .ThenInclude(o => o.Produto)
                                                                    .ThenInclude(o => o.Usuario)
                                                                    .FirstOrDefaultAsync();
        }

    }
}
