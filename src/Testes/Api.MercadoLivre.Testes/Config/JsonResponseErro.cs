using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Testes.Config
{
    public class ResponseBaseError
    {
        public bool sucess { get; set; }
        public IEnumerable<string> errors { get; set; }
    }
}
