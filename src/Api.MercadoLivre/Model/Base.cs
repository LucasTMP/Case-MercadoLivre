using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class Base
    {

        public Base()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

    }
}
