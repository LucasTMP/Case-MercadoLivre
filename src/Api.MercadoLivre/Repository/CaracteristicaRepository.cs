using Api.MercadoLivre.Data;
using Api.MercadoLivre.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Repository
{
    public class CaracteristicaRepository : Repository<Caracteristica>
    {
        public CaracteristicaRepository(ApiDbContext db) : base(db)
        {
        }
    }
}
