using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using AuditoriaAPI.Model;
using System.Reflection;
using System.Linq;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web;

namespace AuditoriaAPI.Util
{
    public class SessionControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionControllerHandler(RouteData routeData)
            : base(routeData)
        { }
    }

    public class SessionHttpControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SessionControllerHandler(requestContext.RouteData);
        }
    }

    public static class Util
    {
        static FileCreator fileCreator = new FileCreator();
        public static bool ValidaArquivo(String path)
        {
            bool result = true;
            if (!File.Exists(path))
            {
                string existeResult = Util.GetDescription<enumMensagem>(enumMensagem.ExisteTabela).Replace("@", path);
                Console.WriteLine(existeResult);
                fileCreator.WriteTxtLog(existeResult);
                result = false;
            }
            return result;
        }

        public static string RetornaMes(int iMes)
        {
            if (iMes == 1)
                return "Janeiro";
            else if (iMes == 2)
                return "Fevereiro";
            else if (iMes == 3)
                return "Março";
            else if (iMes == 4)
                return "Abril";
            else if (iMes == 5)
                return "Maio";
            else if (iMes == 6)
                return "Junho";
            else if (iMes == 7)
                return "Julho";
            else if (iMes == 8)
                return "Agosto";
            else if (iMes == 9)
                return "Setembro";
            else if (iMes == 10)
                return "Outubro";
            else if (iMes == 11)
                return "Novembro";
            else if (iMes == 12)
                return "Dezembro";
            else return "";
        }

        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = null;

            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttributes.Length > 0)
                        {
                            description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                        }

                        break;
                    }
                }
            }

            return description;
        }

        public static List<String> GetParams<T>(T e) where T : class
        {
            Type type = e.GetType();
            List<PropertyInfo> properties = type.GetProperties().ToList();
            return properties.Select(x => x.Name).ToList<string>();
        }
    }
}