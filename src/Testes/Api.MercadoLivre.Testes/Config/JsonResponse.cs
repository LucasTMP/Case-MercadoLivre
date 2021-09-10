using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Testes.Config
{
    public class ResponseBase<T>
    {
        public bool sucess { get; set; }
        public T response { get; set; }
    }
}
