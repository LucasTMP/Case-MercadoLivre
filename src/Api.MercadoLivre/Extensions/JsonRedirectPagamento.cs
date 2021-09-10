using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Extensions
{
    public class Response
    {
        public string id { get; set; }
        public string compraId { get; set; }
        public string pagamentoGatewayId { get; set; }
        public int status { get; set; }
        public DateTime createdAt { get; set; }
    }

    public class Raiz
    {
        public bool sucess { get; set; }
        public Response response { get; set; }
    }
}
