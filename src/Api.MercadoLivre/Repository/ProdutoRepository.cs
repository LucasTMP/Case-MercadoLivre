using Api.MercadoLivre.Data;
using Api.MercadoLivre.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Repository
{
    public class ProdutoRepository : Repository<Produto>
    {
        public ProdutoRepository(ApiDbContext db) : base(db)
        {

        }

    }
}
