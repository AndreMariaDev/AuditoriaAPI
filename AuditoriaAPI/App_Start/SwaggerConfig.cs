using System.Web.Http;
using WebActivatorEx;
using AuditoriaAPI;
using Swashbuckle.Application;
using System;

namespace AuditoriaAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "AuditoriaAPI");
                        c.IncludeXmlComments($@"{AppDomain.CurrentDomain.BaseDirectory}\bin\AuditoriaAPI.xml");
                        c.UseFullTypeNameInSchemaIds();
                        c.DescribeAllEnumsAsStrings();
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DisableValidator();
                    });
        }
    }
}
