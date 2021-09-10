using System.Text;
using Api.MercadoLivre.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


namespace DevIO.Api.Configuration
{
    public static class JwtConfiguration
    {
        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {

            //JWT

            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<JwtSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true; //requerer uso https
                x.SaveToken = true;            // deve ser armazenado no http authentication
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // valida se quem emite é o mesmo de quem pediu e a key
                    IssuerSigningKey = new SymmetricSecurityKey(key), //valida e tranforma a chave
                    ValidateIssuer = true, //valida o link de quem emitiu
                    ValidateAudience = true,  //usa validacao de link
                    ValidAudience = appSettings.ValidoEm, //token valido nesse link
                    ValidIssuer = appSettings.Emissor //valida o link de quem emitiu
                };
            });


            return services;
        }
    }
}