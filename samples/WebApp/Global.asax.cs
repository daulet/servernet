using Newtonsoft.Json.Converters;
using System;
using System.Web;
using System.Web.Http;

namespace WebApp
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(Register);
        }

        private static void Register(HttpConfiguration config)
        {
            // disable XML media types and formatter
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // use ISO8601Literal date time formatting
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ssZ";

            // serialize enum as its string representation
            config.Formatters.JsonFormatter.SerializerSettings.Converters
                .Add(new StringEnumConverter());

            // setup routing
            config.MapHttpAttributeRoutes();
        }
    }
}