using AuditoriaAPI.Model;
using AuditoriaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Swashbuckle.Swagger.Annotations;
using AuditoriaAPI.Commands;
using System.Web.Http.Description;
using System.Threading.Tasks;
using AuditoriaAPI.Repository;
using AuditoriaAPI.Util;
namespace AuditoriaAPI.Controllers
{
    public class UsuarioController : ApiController
    {
        IAuthenticate _IAuthenticate;
        public UsuarioController()
        {
            _IAuthenticate = new AuthenticateConcrete();
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(String))]
        [HttpPost]
        [Route("usuario/InsertUsuario")]
        public IHttpActionResult InsertUsuario([FromBody] UsuarioModel command)
        {
            if (null == command)
            {
                return BadRequest();
            }

            if (String.IsNullOrEmpty(command.TokenSecret))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Not Valid Request");
                return ResponseMessage(message);
            }
            else
            {
                if (!_IAuthenticate.ValidationTimeToken(command.TokenSecret))
                {
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                    message.Content = new StringContent("Toke invalid!");
                    return ResponseMessage(message);
                }
                else
                {
                    try
                    {
                        RepositoryUsuario ru = new RepositoryUsuario();
                        Usuario u = new Usuario();
                        u.Nome = command.Nome;
                        u.Email = command.Email;
                        u.Senha = command.Senha;
                        ru.InsertUsuario(u);
                    }
                    catch (Exception)
                    {
                        var message = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                        message.Content = new StringContent("Erro ao salvar usuário!");
                        return ResponseMessage(message);
                        throw;
                    }
                }
            }
            return Ok("Processo realizado com sucesso");
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(String))]
        [HttpPost]
        [Route("usuario/UpdateUsuario")]
        public IHttpActionResult UpdateUsuario([FromBody] UsuarioModel command)
        {
            if (null == command)
            {
                return BadRequest();
            }

            if (String.IsNullOrEmpty(command.TokenSecret))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Not Valid Request");
                return ResponseMessage(message);
            }
            else
            {
                if (!_IAuthenticate.ValidationTimeToken(command.TokenSecret))
                {
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                    message.Content = new StringContent("Toke invalid!");
                    return ResponseMessage(message);
                }
                else
                {
                    try
                    {
                        RepositoryUsuario ru = new RepositoryUsuario();
                        Usuario u = new Usuario();
                        u.Id = command.Id;
                        u.Nome = command.Nome;
                        u.Email = command.Email;

                        ru.UpdateUsuario(u);
                    }
                    catch (Exception)
                    {
                        var message = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                        message.Content = new StringContent("Erro ao Atualizar Usuário!");
                        return ResponseMessage(message);
                        throw;
                    }
                }
            }
            return Ok("Processo realizado com sucesso");
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(String))]
        [HttpPost]
        [Route("usuario/DeleteUsuario")]
        public IHttpActionResult DeleteUsuario([FromBody] UsuarioModel command)
        {
            if (null == command)
            {
                return BadRequest();
            }

            if (String.IsNullOrEmpty(command.TokenSecret))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Not Valid Request");
                return ResponseMessage(message);
            }
            else
            {
                if (!_IAuthenticate.ValidationTimeToken(command.TokenSecret))
                {
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                    message.Content = new StringContent("Toke invalid!");
                    return ResponseMessage(message);
                }
                else
                {
                    try
                    {
                        RepositoryUsuario ru = new RepositoryUsuario();
                        Usuario u = new Usuario();
                        u.Id = command.Id;
                        u.Deleted = true;

                        ru.UpdateUsuario(u);
                    }
                    catch (Exception)
                    {
                        var message = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                        message.Content = new StringContent("Erro ao Excluir Usuário!");
                        return ResponseMessage(message);
                        throw;
                    }
                }
            }
            return Ok("Processo realizado com sucesso");
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(Usuario))]
        [HttpGet]
        [Route("usuario/GetAllUsuarios")]
        public IHttpActionResult GetAllUsuarios(string TokenSecret)
        {
            if (String.IsNullOrEmpty(TokenSecret))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Not Valid Request");
                return ResponseMessage(message);
            }
            else
            {
                if (!_IAuthenticate.ValidationTimeToken(TokenSecret))
                {
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                    message.Content = new StringContent("Toke invalid!");
                    return ResponseMessage(message);
                }
                else
                {
                    RepositoryUsuario ru = new RepositoryUsuario();
                    List<Usuario> users = ru.GetAllUsuarios().ToList();
                    Response<Usuario> result = new Response<Usuario>();
                    result.Items = users;
                    result.TotalRows = null != users ? users.Count : 0;

                    return Ok(result);
                }
            }
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(Usuario))]
        [HttpGet]
        [Route("usuario/GetUsuarioById")]
        public IHttpActionResult GetUsuarioById(string TokenSecret, int id)
        {
            if (String.IsNullOrEmpty(TokenSecret))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Not Valid Request");
                return ResponseMessage(message);
            }
            else
            {
                if (!_IAuthenticate.ValidationTimeToken(TokenSecret))
                {
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                    message.Content = new StringContent("Toke invalid!");
                    return ResponseMessage(message);
                }
                else
                {
                    RepositoryUsuario ru = new RepositoryUsuario();
                    Usuario users = ru.GetUsuarioById(id);
                    Response<Usuario> result = new Response<Usuario>();
                    result.Item = users;

                    return Ok(result);
                }
            }
        }
    }
}
