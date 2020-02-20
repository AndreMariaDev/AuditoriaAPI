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
    public class RepositoryToken
    {
        static FileCreator fileCreator = new FileCreator();

        public void InsertToken(TokensManager dto)
        {
            StringBuilder strInsert = new StringBuilder();

            strInsert.AppendLine(" INSERT INTO [dbAuditoria].[dbo].[tbToken] ");
            strInsert.AppendLine("            ([TokenKey] ");
            strInsert.AppendLine("            ,[IssuedOn] ");
            strInsert.AppendLine("            ,[ExpiresOn] ");
            strInsert.AppendLine("            ,[CreatedOn] ");
            strInsert.AppendLine("            ,[idUsuario]) ");
            strInsert.AppendLine(" VALUES (");
            strInsert.AppendLine(String.Format("        '{0}'", dto.TokenKey));
            strInsert.AppendLine(String.Format("        ,'{0}'", dto.IssuedOn.ToString("yyyy-MM-dd hh:mm")));
            strInsert.AppendLine(String.Format("        ,'{0}'", dto.ExpiresOn.ToString("yyyy-MM-dd hh:mm")));
            strInsert.AppendLine(String.Format("        ,'{0}'", dto.CreatedOn.ToString("yyyy-MM-dd hh:mm")));
            strInsert.AppendLine("        ," + dto.idUsuario);
            strInsert.AppendLine("        );");

            strInsert.AppendLine("        ");

            Repository<Auditoria>.Insert(strInsert.ToString(), enumConnectionString.DapperAuditoria);
        }

        public IList<TokensManager> GetToken(string TokenKey)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT  ");
            sb.AppendLine(" 	[TokenID]  ");
            sb.AppendLine(" 	,[TokenKey]  ");
            sb.AppendLine(" 	,[IssuedOn]  ");
            sb.AppendLine(" 	,[ExpiresOn]  ");
            sb.AppendLine(" 	,[CreatedOn]  ");
            sb.AppendLine(" 	,[idUsuario]  ");
            sb.AppendLine(" FROM [dbAuditoria].[dbo].[tbToken]  ");

            sb.AppendLine(String.Format(" WHERE  Deleted IS NULL AND TokenKey = '{0}'", TokenKey));

            var result = Repository<TokensManager>.getList(sb.ToString(), enumConnectionString.DapperAuditoria);
            return null != result ? result.ToList() : null;
        }

        public IList<TokensManager> GetTokenByUsuario(int idUsuario)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT  ");
            sb.AppendLine(" 	[TokenID]  ");
            sb.AppendLine(" 	,[TokenKey]  ");
            sb.AppendLine(" 	,[IssuedOn]  ");
            sb.AppendLine(" 	,[ExpiresOn]  ");
            sb.AppendLine(" 	,[CreatedOn]  ");
            sb.AppendLine(" 	,[idUsuario]  ");
            sb.AppendLine(" FROM [dbAuditoria].[dbo].[tbToken]  ");

            sb.AppendLine(String.Format(" WHERE  Deleted IS NULL AND idUsuario = {0}", idUsuario));

            var result = Repository<TokensManager>.getList(sb.ToString(), enumConnectionString.DapperAuditoria);
            return null != result ? result.ToList() : null;
        }

        #region [Gets]
        public void DeleteToken(string TokenKey)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" UPDATE [dbAuditoria].[dbo].[tbToken] ");
            sb.AppendLine("    SET [Deleted] = 1  ");
            sb.AppendLine(String.Format(" WHERE [TokenKey] = '{0}'", TokenKey));

            Repository<TokensManager>.Update(sb.ToString(), enumConnectionString.DapperAuditoria);
        }
        #endregion
    }
}