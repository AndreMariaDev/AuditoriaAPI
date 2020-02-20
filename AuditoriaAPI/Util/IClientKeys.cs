using AuditoriaAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Util
{
    public interface IClientKeys
    {
        void GenerateUniqueKey(out string ClientID, out string ClientSecert);
    }
}