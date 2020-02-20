using System;
using System.Collections.Generic;
using System.Text;
using AuditoriaAPI.Model;
using AuditoriaAPI.Util;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AuditoriaAPI.Repository
{
    public class RepositoryUsuario
    {
        static FileCreator fileCreator = new FileCreator();

        public void InsertUsuario(Usuario dto)
        {
            StringBuilder strInsert = new StringBuilder();

            strInsert.AppendLine(" INSERT INTO [dbAuditoria].[dbo].[tbUsuario] ");
            strInsert.AppendLine("            ([Nome] ");
            strInsert.AppendLine("            ,[Email] ");
            strInsert.AppendLine("            ,[Senha] ");
            strInsert.AppendLine("            ,[DataCadastro] )");
            strInsert.AppendLine(" VALUES ( ");
            strInsert.AppendLine(String.Format("        '{0}'", dto.Nome));
            strInsert.AppendLine(String.Format("        ,'{0}'", dto.Email));
            strInsert.AppendLine(String.Format("        ,'{0}'", dto.Senha));
            strInsert.AppendLine(String.Format("        ,'{0}'", DateTime.Now.ToString("yyyy-MM-dd 00:00")));
            strInsert.AppendLine("        );");

            strInsert.AppendLine("        ");

            Repository<Auditoria>.Insert(strInsert.ToString(), enumConnectionString.DapperAuditoria);
        }

        public void UpdateUsuario(Usuario dto)
        {
            StringBuilder strUpdate = new StringBuilder();

            strUpdate.AppendLine(" UPDATE [dbAuditoria].[dbo].[tbUsuario] ");
            strUpdate.AppendLine(" SET  ");

            if (dto.Deleted)
            {
                strUpdate.AppendLine(String.Format("        [Deleted] = {0}", 1));
            }
            else
            {
                strUpdate.AppendLine(String.Format("        [Nome] = '{0}'", dto.Nome));
                strUpdate.AppendLine(String.Format("        , [Email] = '{0}'", dto.Email));
            }
            strUpdate.AppendLine(String.Format("         WHERE ID = {0} ", dto.Id));

            strUpdate.AppendLine("        ");

            Repository<Auditoria>.Insert(strUpdate.ToString(), enumConnectionString.DapperAuditoria);
        }

        public IList<Usuario> GetUsuarioValido(Usuario dto)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT [ID]  ");
            sb.AppendLine("       ,[Nome]  ");
            sb.AppendLine("       ,[Email]  ");
            sb.AppendLine("       ,[Senha]  ");
            sb.AppendLine("       ,[DataCadastro]  ");
            sb.AppendLine("       ,[Deleted]  ");
            sb.AppendLine(" FROM [dbAuditoria].[dbo].[tbUsuario]  ");
            sb.AppendLine(String.Format(" WHERE [Email] = '{0}' AND [Senha] = '{1}' and [Deleted] is null", dto.Email, dto.Senha));

            var result = Repository<Usuario>.getList(sb.ToString(), enumConnectionString.DapperAuditoria);
            return null != result ? result.ToList() : null;
        }


        public Usuario GetUsuarioById(int id)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT [ID]  ");
            sb.AppendLine("       ,[Nome]  ");
            sb.AppendLine("       ,[Email]  ");
            sb.AppendLine("       ,[Senha]  ");
            sb.AppendLine("       ,[DataCadastro]  ");
            sb.AppendLine("       ,[Deleted]  ");
            sb.AppendLine(" FROM [dbAuditoria].[dbo].[tbUsuario]  ");
            sb.AppendLine(String.Format(" WHERE [ID] = {0} ", id));

            var result = Repository<Usuario>.getList(sb.ToString(), enumConnectionString.DapperAuditoria);
            return null != result ? result.FirstOrDefault() : null;
        }

        public IList<Usuario> GetAllUsuarios()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT [ID]  ");
            sb.AppendLine("       ,[Nome]  ");
            sb.AppendLine("       ,[Email]  ");
            sb.AppendLine("       ,[DataCadastro]  ");
            sb.AppendLine("       ,[Deleted]  ");
            sb.AppendLine(" FROM [dbAuditoria].[dbo].[tbUsuario]  ");

            var result = Repository<Usuario>.getList(sb.ToString(), enumConnectionString.DapperAuditoria);
            return null != result ? result.ToList() : null;
        }

        #region [Gets]
        public void DeleteUsuario(int id)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" UPDATE [dbAuditoria].[dbo].[tbUsuario] ");
            sb.AppendLine("    SET [Deleted] = 1  ");
            sb.AppendLine(String.Format(" WHERE [ID] = {0}", id));

            Repository<TokensManager>.Update(sb.ToString(), enumConnectionString.DapperAuditoria);
        }
        #endregion
    }
}