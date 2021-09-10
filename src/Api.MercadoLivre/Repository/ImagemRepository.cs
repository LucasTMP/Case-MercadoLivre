using Api.MercadoLivre.Data;
using Api.MercadoLivre.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Repository
{
    public class ImagemRepository : Repository<Imagem>
    {
        public ImagemRepository(ApiDbContext db) : base(db)
        {
        }
    }
}
