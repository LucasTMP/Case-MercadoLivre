using Api.MercadoLivre.Data;
using Api.MercadoLivre.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Repository
{
    public class PerguntaRepository : Repository<Pergunta>
    {
        public PerguntaRepository(ApiDbContext db) : base(db)
        {
        }


        public async Task<IEnumerable<Pergunta>> GetPerguntasUsuario(Guid id)
        {
            return await DbSet.AsNoTrackingWithIdentityResolution().Where(o => o.ProdutoId == id).Include(o => o.Usuario).ToListAsync();
        }


    }
}
