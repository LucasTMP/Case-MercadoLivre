using Api.MercadoLivre.Data;
using Api.MercadoLivre.Model;
using Api.MercadoLivre.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.CasaDoCodigo.Configurations
{
    public static class DependencyInjectionConfig
    {

        public static IServiceCollection DependencyInjection(this IServiceCollection services)
        {

            services.AddScoped<ApiDbContext>();
            services.AddScoped<UsuarioRepository>();
            services.AddScoped<CategoriaRepository>();
            services.AddScoped<ProdutoRepository>();
            services.AddScoped<CaracteristicaRepository>();
            services.AddScoped<OpiniaoRepository>();
            services.AddScoped<PerguntaRepository>();
            services.AddScoped<ImagemRepository>();
            services.AddScoped<CompraRepository>();
            services.AddScoped<PagamentoRepository>();

            return services;
        }

    }
}
