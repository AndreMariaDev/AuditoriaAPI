using AuditoriaAPI.Model;
using AuditoriaAPI.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using Swashbuckle.Swagger.Annotations;
using AuditoriaAPI.Models;
using AuditoriaAPI.Repository;

namespace AuditoriaAPI.Controllers
{
    [RoutePrefix("api")]
    public class AuthenticateController : ApiController
    {
        IAuthenticate _IAuthenticate;
        public AuthenticateController()
        {
            _IAuthenticate = new AuthenticateConcrete();
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(String))]
        [HttpPost]
        [Route("authenticate/Authenticate")]
        public HttpResponseMessage Authenticate([FromBody]Login ClientKeys)
        {
            if (string.IsNullOrEmpty(ClientKeys.Email) && string.IsNullOrEmpty(ClientKeys.Password))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Not Valid Request");
                return message;
            }
            else
            {
                if (ClientKeys == null)
                {
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                    message.Content = new StringContent("InValid Keys");
                    return message;
                }
                else
                {
                    if (String.IsNullOrEmpty(ClientKeys.TokenSecret))
                    {
                        if (_IAuthenticate.IsTokenAlreadyExists(ClientKeys.TokenSecret))
                        {
                            if (!_IAuthenticate.ValidationTimeToken(ClientKeys.TokenSecret))
                            {
                                _IAuthenticate.DeleteGenerateToken(ClientKeys.TokenSecret);
                                return GenerateandSaveToken(1);
                            }
                            var message = new HttpResponseMessage(HttpStatusCode.Accepted);
                            message.Content = new StringContent("Accepted");
                            return message;
                        }
                        else
                        {
                            return GenerateandSaveToken(1);
                        }
                    }
                    else
                    {
                        return GenerateandSaveToken(1);
                    }
                }
            }
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(String))]
        [HttpPost]
        [Route("authenticate/TokenValidation")]
        public HttpResponseMessage TokenValidation([FromBody]Token token)
        {
            if (string.IsNullOrEmpty(token.TokenSecret))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Not Valid Request");
                return message;
            }
            else
            {
                if (_IAuthenticate.IsTokenAlreadyExists(token.TokenSecret))
                {
                    if (!_IAuthenticate.ValidationTimeToken(token.TokenSecret))
                    {
                        var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                        message.Content = new StringContent("Time up!");
                        _IAuthenticate.DeleteGenerateToken(token.TokenSecret);
                        return message;
                    }
                    else
                    {
                        var message = new HttpResponseMessage(HttpStatusCode.Accepted);
                        message.Content = new StringContent("Accepted");
                        return message;
                    }

                }
                else
                {
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                    message.Content = new StringContent("InValid Keys");
                    return message;
                }
            }
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(String))]
        [HttpPost]
        [Route("authenticate/SignIn")]
        public HttpResponseMessage SignIn([FromBody] Login command)
        {
            if (command == null)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                message.Content = new StringContent("InValid Keys");
                return message;
            }

            if (string.IsNullOrEmpty(command.Email) && string.IsNullOrEmpty(command.Password))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Not Valid Request");
                return message;
            }
            else
            {
                RepositoryUsuario ru = new RepositoryUsuario();
                Usuario dto = new Usuario();
                dto.Email = command.Email;
                dto.Senha = command.Password;

                var result = ru.GetUsuarioValido(dto);

                if (null != result && result.Count > 0)
                {
                    var user = result.FirstOrDefault();

                    if (user.Deleted)
                    {
                        var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                        message.Content = new StringContent("Usuario não consta na base.");
                        return message;
                    }
                    else
                    {
                        RepositoryToken rt = new RepositoryToken();
                        var tokenUser = rt.GetTokenByUsuario(user.Id);

                        if (null != tokenUser && tokenUser.Count > 0)
                        {
                            var tokenResult = tokenUser.FirstOrDefault();

                            if (tokenResult.ExpiresOn > tokenResult.CreatedOn)
                            {
                                HttpResponseMessage response = new HttpResponseMessage();
                                response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
                                response.Headers.Add("Token", String.Format("Tokem = {0} ; IdUsuario = {1}", tokenResult.TokenKey, user.Id));
                                response.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["TokenExpiry"]);
                                response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
                                return response;
                            }
                            else
                            {
                                rt.DeleteToken(tokenResult.TokenKey);
                                return GenerateandSaveToken(user.Id);
                            }
                        }
                        return GenerateandSaveToken(user.Id);
                    }

                }
                else
                {
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                    message.Content = new StringContent("Usuario não consta na base.");
                    return message;
                }
            }
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(String))]
        [HttpPost]
        [Route("authenticate/Logoff")]
        public HttpResponseMessage Logoff([FromBody]Token token)
        {
            if (null == token && String.IsNullOrEmpty(token.TokenSecret))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                message.Content = new StringContent("InValid Keys");
                return message;
            }
            else
            {
                RepositoryToken rt = new RepositoryToken();
                rt.DeleteToken(token.TokenSecret);
                var message = new HttpResponseMessage(HttpStatusCode.Accepted);
                message.Content = new StringContent("Accepted");
                return message;
            }
        }

        [NonAction]
        private HttpResponseMessage GenerateandSaveToken(int idUsuario)
        {
            try
            {
                var IssuedOn = DateTime.Now;
                var newToken = _IAuthenticate.GenerateToken(IssuedOn);
                TokensManager token = new TokensManager();
                token.TokenID = 0;
                token.TokenKey = newToken;
                token.IssuedOn = IssuedOn;
                token.ExpiresOn = DateTime.Now.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpiry"]));
                token.CreatedOn = DateTime.Now;
                token.idUsuario = idUsuario;

                _IAuthenticate.SaveToken(token);

                HttpResponseMessage response = new HttpResponseMessage();
                response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
                response.Headers.Add("Token",String.Format("Tokem = {0} ; IdUsuario = {1}", newToken, idUsuario));
                response.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["TokenExpiry"]);
                response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
                return response;
            }
            catch (Exception)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Error in Creating Token");
                return message;
            }

        }
    }
}
