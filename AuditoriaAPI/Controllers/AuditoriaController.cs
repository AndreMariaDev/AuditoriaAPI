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
    [RoutePrefix("api")]
    public class AuditoriaController : ApiController
    {
        IAuthenticate _IAuthenticate;
        public AuditoriaController()
        {
            _IAuthenticate = new AuthenticateConcrete();
        }

        #region [Get]

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(Loja))]
        [HttpGet]
        [Route("auditoria/GetAllLoja")]
        public IHttpActionResult GetAllLoja(string TokenSecret)
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
                    RepositoryVortice rv = new RepositoryVortice();
                    rv.GetLoja();
                    List<Loja> lojas = rv.GetLoja().ToList();
                    return Ok(lojas);
                }
            }
        }


        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(Loja))]
        [HttpGet]
        [Route("auditoria/GetSearchLoja")]
        public IHttpActionResult GetSearchLoja(string TokenSecret,String nome)
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
                    RepositoryVortice rv = new RepositoryVortice();
                    rv.GetLoja();
                    List<Loja> lojas = rv.GetSearchLoja(nome).ToList();
                    var result = lojas.Where(x => x.sNome.Contains(nome));
                    return Ok(result);
                }
            }
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(Pessoa))]
        [HttpGet]
        [Route("auditoria/GetPessoaByIdLoja")]
        public IHttpActionResult GetPessoaByIdLoja(string TokenSecret,int idLoja)
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
                    RepositoryVortice rv = new RepositoryVortice();
                    List<Pessoa> Pessoas = rv.GetPessoaPorLoja(idLoja).ToList();
                    return Ok(Pessoas);
                }
            }
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(SubTiposDocumentos))]
        [HttpGet]
        [Route("auditoria/GetSubTiposDocumentos")]
        public IHttpActionResult GetSubTiposDocumentos(string TokenSecret,int IdTipoDocumento)
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
                    RepositoryAuditoria ra = new RepositoryAuditoria();
                    List<SubTiposDocumentos> documentos = ra.GetSubTiposDocumentos(IdTipoDocumento).ToList();
                    return Ok(documentos);
                }
            }
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(SubTiposDocumentos))]
        [HttpGet]
        [Route("auditoria/GetListAuditoria")]
        public IHttpActionResult GetListAuditoria(string TokenSecret,int inicio, int fim)
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
                    List<AuditoriaResultModel> list = null;
                    RepositoryAuditoria ra = new RepositoryAuditoria();

                    var listAuditoria = ra.GetAuditoria(inicio, fim);
                    int total = 0;
                    if (null != listAuditoria && listAuditoria.Count > 0)
                    {
                        string listItemId = String.Join(",", listAuditoria.Select(x => x.id));
                        var listItem = ra.GetAuditoriaItem(listItemId);

                        string listDocumentos = String.Empty;

                        if (null != listItem && listItem.Count > 0)
                        {
                            list = (from a in listAuditoria
                                    select new AuditoriaResultModel()
                                    {
                                        Index = a.id,
                                        NomeLoja = a.Nomeloja,
                                        NomeColaborador = a.NomePessoa,
                                        Obs = a.Obs,
                                        DataCadastro = a.DataCadastro.ToString(),
                                        Documentos = String.Join(",", listItem.Where(x => x.idAuditoria == a.id).Select(y=>y.DocumentosDescricao))
                                    }).ToList();
                        }
                        else
                        {
                            list = (from a in listAuditoria
                                    select new AuditoriaResultModel()
                                    {
                                        Index = a.id,
                                        NomeLoja = a.Nomeloja,
                                        NomeColaborador = a.NomePessoa,
                                        Obs = a.Obs,
                                        DataCadastro = a.DataCadastro.ToString()
                                    }).ToList();
                        }

                        total = ra.GetCountAuditoria();
                    }
                    
                    Response<AuditoriaResultModel> result = new Response<AuditoriaResultModel>();
                    result.TotalRows = total;
                    result.Items = list.ToList();
                    return Ok(result);
                }
            }
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(SubTiposDocumentos))]
        [HttpGet]
        [Route("auditoria/GetAuditoria")]
        public IHttpActionResult GetAuditoria(string TokenSecret, int id)
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
                    AuditoriaResultModel Item = null;
                    RepositoryAuditoria ra = new RepositoryAuditoria();

                    Response<Auditoria> result = new Response<Auditoria>();
                    result.Item = ra.GetAuditoria(id);
                    return Ok(result);
                }
            }
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(Response<AuditoriaItem>))]
        [HttpGet]
        [Route("auditoria/GetAuditoriaItem")]
        public IHttpActionResult GetAuditoriaItem(string TokenSecret, int id)
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
                    AuditoriaResultModel Item = null;
                    RepositoryAuditoria ra = new RepositoryAuditoria();

                    Response<AuditoriaItem> result = new Response<AuditoriaItem>();
                    var list = ra.GetAuditoriaItemByID(id);
                    result.Items = (null != list && list.Count > 0) ? list.ToList() : null;
                    return Ok(result);
                }
            }
        }

        #endregion

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(String))]
        [HttpPost]
        [Route("auditoria/InsertAuditoria")]
        public HttpResponseMessage InsertAuditoria([FromBody] AuditoriaModel command)
        {
            if (null == command)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                message.Content = new StringContent("Not found!");
                return message;
            }

            if (String.IsNullOrEmpty(command.TokenSecret))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Not Valid Request");
                return message;
            }
            else
            {
                if (!_IAuthenticate.ValidationTimeToken(command.TokenSecret))
                {
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                    message.Content = new StringContent("InValid Keys");
                    return message;
                }
                else
                {
                    RepositoryAuditoria ra = new RepositoryAuditoria();
                    RepositoryVortice rv = new RepositoryVortice();
                    var listLojas = rv.GetLojaByListId(command.IdLoja.ToString());

                    var listPessoas = rv.GetPessoaPorListId(command.IdPessoa.ToString());

                    Auditoria auditoria = new Auditoria();
                    auditoria.idLoja = command.IdLoja;
                    auditoria.Nomeloja = listLojas.FirstOrDefault().sNome;
                    auditoria.NomePessoa = listPessoas.FirstOrDefault().sNome;
                    auditoria.idPessoa = command.IdPessoa;
                    auditoria.idUsuario = command.IdUsuario;
                    auditoria.Obs = command.Obs;
                    
                    List<AuditoriaItem> AuditoriaItem = new List<AuditoriaItem>();
                    var items = command.AuditoriaItem.Split(',');
                    var docs = ra.GetSubTiposDocumentos(0);

                    foreach (var item in items)
                    {
                        int id = 0;
                        int.TryParse(item, out id);
                        if(id > 0)
                            AuditoriaItem.Add(new Model.AuditoriaItem() { idSubTiposDocumentos = id ,
                                DocumentosDescricao = docs.Where( x=> x.IdSubTipoDocumento == id).FirstOrDefault().Descricao });
                    }

                    auditoria.AuditoriaItem = AuditoriaItem;

                    try
                    {
                        ra.InsertAuditoria(auditoria);
                        HttpResponseMessage response = new HttpResponseMessage();
                        response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
                        return response;
                    }
                    catch (Exception)
                    {
                        var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                        message.Content = new StringContent("Error in Creating");
                        return message;
                    }
                }
            }
        }

        [SwaggerResponse(400, "BadRequest")]
        [SwaggerResponse(404, "NotFound")]
        [SwaggerResponse(200, "Ok", typeof(String))]
        [HttpPost]
        [Route("auditoria/UpdateAuditoria")]
        public HttpResponseMessage UpdateAuditoria([FromBody] AuditoriaModel command)
        {
            if (null == command)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                message.Content = new StringContent("Not found!");
                return message;
            }

            if (String.IsNullOrEmpty(command.TokenSecret))
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                message.Content = new StringContent("Not Valid Request");
                return message;
            }
            else
            {
                if (!_IAuthenticate.ValidationTimeToken(command.TokenSecret))
                {
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound);
                    message.Content = new StringContent("InValid Keys");
                    return message;
                }
                else
                {
                    RepositoryAuditoria ra = new RepositoryAuditoria();

                    Auditoria auditoria = new Auditoria();
                    auditoria.id = command.Id;
                    auditoria.Obs = command.Obs;

                    List<AuditoriaItem> AuditoriaItem = new List<AuditoriaItem>();
                    var items = command.AuditoriaItem.Split(',');
                    var docs = ra.GetSubTiposDocumentos(0);

                    foreach (var item in items)
                    {
                        int id = 0;
                        int.TryParse(item, out id);
                        if (id > 0)
                            AuditoriaItem.Add(new Model.AuditoriaItem()
                            {
                                idSubTiposDocumentos = id,
                                DocumentosDescricao = docs.Where(x => x.IdSubTipoDocumento == id).FirstOrDefault().Descricao
                            });
                    }

                    auditoria.AuditoriaItem = AuditoriaItem;

                    try
                    {
                        ra.UpdateAuditoria(auditoria);
                        HttpResponseMessage response = new HttpResponseMessage();
                        response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
                        return response;
                    }
                    catch (Exception)
                    {
                        var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                        message.Content = new StringContent("Error in Creating");
                        return message;
                    }
                }
            }
        }
    }
}
