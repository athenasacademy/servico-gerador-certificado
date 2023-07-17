using System.Web.Http;
using WebActivatorEx;
using AthenasAcademy.GeradorCertificado.API;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace AthenasAcademy.GeradorCertificado.API
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "AthenasAcademy.GeradorCertificado.API");
                    })
                .EnableSwaggerUi(c =>
                    {

                    });
        }
    }
}
