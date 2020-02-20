using AuditoriaAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Util
{
    public interface IAuthenticate
    {
        bool IsTokenAlreadyExists(String TokenSecret);
        void DeleteGenerateToken(String TokenSecret);
        string GenerateToken(DateTime IssuedOn);
        bool ValidationTimeToken(string tokenSecret);
        bool ValidationTimeToken();
        void SaveToken(TokensManager tokensManager);
    }
}