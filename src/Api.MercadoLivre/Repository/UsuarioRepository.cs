using Api.MercadoLivre.Data;
using Api.MercadoLivre.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Repository
{
    public class UsuarioRepository : Repository<Usuario>
    {
        public UsuarioRepository(ApiDbContext db) : base(db)
        {
        }

    }
}
