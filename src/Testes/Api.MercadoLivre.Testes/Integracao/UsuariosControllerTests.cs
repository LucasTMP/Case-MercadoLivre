using Api.MercadoLivre.Model;
using Api.MercadoLivre.Testes.Config;
using Api.MercadoLivre.Testes.Config.PriorityOrderer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Api.MercadoLivre.Testes
{

    [Collection(nameof(IntegracaoCollection))]
    [TestCaseOrderer("Api.MercadoLivre.Testes.Config.PriorityOrderer", "Api.MercadoLivre.Testes")]
    public class UsuariosControllerTests
    {
        private readonly string _route = "api/v1/usuarios";
        private readonly FixtureIntegracao _fixtureIntegracao;

        public UsuariosControllerTests(FixtureIntegracao fixtureIntegracao)
        {
            _fixtureIntegracao = fixtureIntegracao;
        }



        [Fact(DisplayName = "Adicionar_Usuario_Valido"), TestPriority(0)]
        [Trait("Controller", "Usuarios")]
        public async void AddUsuarioPostJson_AdicionarUsuarioValido_DeveRetornarStatus201ComDadosDoNovoUsuario()
        {

            // Arrange

            var usuario = _fixtureIntegracao.GerarUsuarioValido();

            using (HttpClient client = _fixtureIntegracao.factory.CreateClient())
            {
                client.BaseAddress = new Uri(_fixtureIntegracao.baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //act              
                var responseJson = await client.PostAsJsonAsync(_route, usuario).ConfigureAwait(false);

                // Assert
                var response = JsonConvert.DeserializeObject
                    <ResponseBase<UsuarioReturnViewModel>>(responseJson.Content.ReadAsStringAsync().Result);

                Assert.Equal(201, (int)responseJson.StatusCode);
                Assert.IsType<ResponseBase<UsuarioReturnViewModel>>(response);
                Assert.True(response.sucess);
                Assert.Contains(usuario.Login, response.response.Login);
                Assert.IsType<Guid>(response.response.Id);
            }
        }


        [Theory(DisplayName = "Adicionar_Usuarios_Invalidos")]
        [InlineData("emailinvalido", "123456")]
        [InlineData("email@gmail.com", "123")]
        [InlineData("", "")]
        [InlineData("emailinvalido", "123")]
        [Trait("Controller", "Usuarios")]
        public async void AddUsuarioPostJson_AdicionarUsuarioInValido_DeveRetornarStatus400ComErros(string senha, string login)
        {

            // Arrange

            var usuario = new UsuarioAddViewModel
            {
                Login = login,
                Senha = senha
            };

            using (HttpClient client = _fixtureIntegracao.factory.CreateClient())
            {
                client.BaseAddress = new Uri(_fixtureIntegracao.baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //act              
                var responseJson = await client.PostAsJsonAsync(_route, usuario).ConfigureAwait(false);

                // Assert
                var response = JsonConvert.DeserializeObject
                    <ResponseBaseError>(responseJson.Content.ReadAsStringAsync().Result);

                Assert.Equal(400, (int)responseJson.StatusCode);
                Assert.IsType<ResponseBaseError>(response);
                Assert.False(response.sucess);
            }
        }

    }

}
