using Api.MercadoLivre.Testes.Config;
using System;
using Xunit;

namespace Api.MercadoLivre.Testes
{

    [Collection(nameof(IntegracaoCollection))]
    public class UsuarioTests
    {

        private readonly FixtureIntegracao _fixtureIntegracao;

        public UsuarioTests(FixtureIntegracao fixtureIntegracao)
        {
            _fixtureIntegracao = fixtureIntegracao;
        }


        [Fact(DisplayName = "Validar_Usuario_Valido")]
        [Trait("Model", "Usuario")]
        public async void Validar_UsuariosValidos_DeveValidarComSucessoTodosUsuarios()
        {
            var resultTest = false;

            // Arrange Act

            var usuarios = _fixtureIntegracao.GerarUsuariosValidos(10);
            foreach (var usuario in usuarios)
            {
                var ehValido = usuario.IsValid().Result.IsValid;
                if (ehValido)
                {
                    resultTest = true;
                }
                else
                {
                    resultTest = false;
                    break;
                };
            }

            // Assert

            Assert.True(resultTest);

        }


        [Fact(DisplayName = "Validar_Usuario_Invalido")]
        [Trait("Model", "Usuario")]
        public async void Validar_UsuariosInvalidos_DeveInvalidarTodosUsuarios()
        {
            var resultTest = true;

            // Arrange Act

            var usuarios = _fixtureIntegracao.GerarUsuariosInvalidos(10);
            foreach (var usuario in usuarios)
            {
                var ehValido = usuario.IsValid().Result.IsValid;
                if (!ehValido)
                {
                    resultTest = false;
                }
                else
                {
                    resultTest = true;
                    break;
                };
            }

            // Assert

            Assert.False(resultTest);

        }
    }
}
