using AuditoriaAPI.Model;
using AuditoriaAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuditoriaAPI.Util
{
    public class AuthenticateConcrete : IAuthenticate
    {
        public bool IsTokenAlreadyExists(String TokenSecret)
        {
            RepositoryToken rt = new RepositoryToken();
            try
            {
                //var session = HttpContext.Current.Session;
                var tokens = rt.GetToken(TokenSecret);

                return (null != tokens && tokens.Count > 0);
            }
            catch (Exception )
            {
                throw;
            }
        }

        public void DeleteGenerateToken(String TokenSecret)
        {
            RepositoryToken rt = new RepositoryToken();
            try
            {
                rt.DeleteToken(TokenSecret);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GenerateToken( DateTime IssuedOn)
        {
            try
            {
                string randomnumber =
                   string.Join(":", new string[]
                   {   
                        KeyGenerator.GetUniqueKey(),
                        Convert.ToString(IssuedOn.Ticks)
                   });

                return randomnumber;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public bool ValidationTimeToken(string tokenSecret)
        {
            RepositoryToken rt = new RepositoryToken();
            try
            {
                //var session = HttpContext.Current.Session;
                var tokens = rt.GetToken(tokenSecret);
                if (null != tokens && tokens.Count > 0)
                {
                    var tokenResult = tokens.FirstOrDefault();

                    if (null != tokenResult)
                    {
                        return tokenResult.ExpiresOn > tokenResult.CreatedOn;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ValidationTimeToken()
        {
            try
            {
                var session = HttpContext.Current.Session;
                var Tokens = (TokensManager)(session["Token"]);

                return (Tokens.ExpiresOn > DateTime.Now);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveToken(TokensManager tokensManager)
        {
            RepositoryToken rt = new RepositoryToken();
            try
            {
                rt.InsertToken(tokensManager);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}