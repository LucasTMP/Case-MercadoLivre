using Api.MercadoLivre.Data;
using Api.MercadoLivre.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Repository
{
    public class CompraRepository : Repository<Compra>
    {
        public CompraRepository(ApiDbContext db) : base(db)
        {
        }


        public async Task<IEnumerable<Compra>> GetComprasProdutoComprador()
        {
            return await DbSet.AsNoTrackingWithIdentityResolution().Include(o => o.Usuario).Include(o => o.Produto).ThenInclude(o => o.Usuario).ToListAsync();
        }

        public async Task<Compra> GetCompraProdutoComprador(Guid id)
        {
            return await DbSet.AsNoTrackingWithIdentityResolution().Where(o => o.Id == id).Include(o => o.Usuario).Include(o => o.Produto).FirstOrDefaultAsync();
        }

    }
}
