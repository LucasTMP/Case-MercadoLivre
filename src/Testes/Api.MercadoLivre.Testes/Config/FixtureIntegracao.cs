using Api.MercadoLivre.Data;
using Api.MercadoLivre.Model;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;



namespace Api.MercadoLivre.Testes.Config
{


    [CollectionDefinition(nameof(IntegracaoCollection))]
    public class IntegracaoCollection : ICollectionFixture<FixtureIntegracao> { }


    public class FixtureIntegracao : IDisposable
    {

        public readonly WebApplicationFactory<Startup> factory;
        private static bool _databaseInitialized;
        //public SqliteConnection connection;
        public string baseUri = "https://localhost:5001";

        public FixtureIntegracao()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<ApiDbContext>();
                    services.AddScoped(_ => ConfigureContext(connection));
                });
            });
        }

        private static ApiDbContext ConfigureContext(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new ApiDbContext(options);

            if (!_databaseInitialized)
            {
                context.Database.EnsureCreated();
                _databaseInitialized = true;
            }

            return context;
        }

        public List<UsuarioAddViewModel> GerarUsuariosValidos(int quantidade)
        {
            var usuarios = new List<UsuarioAddViewModel>();

            Random random = new Random();

            var faker = new Faker("pt_BR");

            for (int i = 0; i <= quantidade; i++)
            {
                var usuario = new Faker<UsuarioAddViewModel>("pt_BR")
                .CustomInstantiator(f => new UsuarioAddViewModel
                {
                    Login = faker.Internet.Email().ToLower(),
                    Senha = faker.Internet.Password(random.Next(6, 13), false, "", "@1Ab")
                });

                usuarios.Add(usuario);
            }

            return usuarios;
        }

        public List<UsuarioAddViewModel> GerarUsuariosInvalidos(int quantidade)
        {
            var usuarios = new List<UsuarioAddViewModel>();

            Random random = new Random();

            var faker = new Faker("pt_BR");

            for (int i = 0; i <= quantidade; i++)
            {
                var usuario = new Faker<UsuarioAddViewModel>("pt_BR")
                .CustomInstantiator(f => new UsuarioAddViewModel
                {
                    Login = i % 2 == 0 ? faker.Name.FullName() : faker.Internet.Email(),
                    Senha = i % 2 == 0 ? faker.Internet.Password(random.Next(6, 13), false, "", "@1Ab") : "1337"
                });

                usuarios.Add(usuario);
            }

            return usuarios;
        }


        public UsuarioAddViewModel GerarUsuarioValido()
        {
            return GerarUsuariosValidos(1).FirstOrDefault();
        }


        public UsuarioAddViewModel GerarUsuarioInvalido()
        {
            return GerarUsuariosInvalidos(1).FirstOrDefault();
        }

        public void Dispose()
        {

        }


    }
}
