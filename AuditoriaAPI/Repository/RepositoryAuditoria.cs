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
    public class RepositoryAuditoria
    {
        static FileCreator fileCreator = new FileCreator();

        public void InsertAuditoria(Auditoria dto)
        {
            StringBuilder strInsert = new StringBuilder();

            strInsert.AppendLine(" DECLARE @idAuditoria INT ; ");
            strInsert.AppendLine("        ");
            strInsert.AppendLine(" INSERT INTO [dbAuditoria].[dbo].[tbAuditoria] ");
            strInsert.AppendLine("            ([idLoja] ");
            strInsert.AppendLine("            ,[Nomeloja] ");
            strInsert.AppendLine("            ,[idPessoa] ");
            strInsert.AppendLine("            ,[NomePessoa] ");
            strInsert.AppendLine("            ,[Obs] ");
            strInsert.AppendLine("            ,[DataCadastro] ");
            strInsert.AppendLine("            ,[idUsuario] ) ");
            strInsert.AppendLine("      VALUES ( ");
            strInsert.AppendLine("        " + dto.idLoja);
            strInsert.AppendLine(String.Format("        ,'{0}'", dto.Nomeloja));
            strInsert.AppendLine("        ," + dto.idPessoa);
            strInsert.AppendLine(String.Format("        ,'{0}'", dto.NomePessoa));
            strInsert.AppendLine(String.Format("        ,'{0}'", dto.Obs));
            strInsert.AppendLine(String.Format("        ,'{0}'", DateTime.Now.ToString("yyyy-MM-dd 00:00")));
            strInsert.AppendLine("        ," + dto.idUsuario);
            strInsert.AppendLine("        );");

            if (null != dto.AuditoriaItem  && dto.AuditoriaItem.Count > 0)
            {
                strInsert.AppendLine("        ");

                strInsert.AppendLine("   SET @idAuditoria = (SELECT @@IDENTITY);     ");

                strInsert.AppendLine("        ");

                strInsert.AppendLine(" INSERT INTO [dbAuditoria].[dbo].[tbAuditoriaItem]  ");
                strInsert.AppendLine("            ([idAuditoria]  ");
                strInsert.AppendLine("            ,[idSubTiposDocumentos]  ");
                strInsert.AppendLine("            ,[DocumentosDescricao])  ");
                strInsert.AppendLine("      VALUES  ");

                int total = dto.AuditoriaItem.Count;
                int index = 0;
                foreach (var item in dto.AuditoriaItem)
                {
                    strInsert.AppendLine("        (");

                    strInsert.Append("        @idAuditoria ");
                    strInsert.Append("        ," + item.idSubTiposDocumentos);
                    strInsert.AppendLine(String.Format("        ,'{0}'", item.DocumentosDescricao));

                    if (index + 1 < total)
                    {
                        strInsert.AppendLine("        ),");
                    }
                    else
                    {
                        strInsert.AppendLine("        );");
                    }
                    index++;
                }

                strInsert.AppendLine("        ");
            }

            Repository<Auditoria>.Insert(strInsert.ToString(), enumConnectionString.DapperAuditoria);
        }

        public void UpdateAuditoria(Auditoria dto)
        {
            StringBuilder strUpdate = new StringBuilder();

            strUpdate.AppendLine(" declare @idAuditoria int; ");
            strUpdate.AppendLine(" UPDATE [dbAuditoria].[dbo].[tbAuditoria] ");
            strUpdate.AppendLine("    SET  ");
            strUpdate.AppendLine(String.Format("        [Obs] = '{0}'", dto.Obs));
            strUpdate.AppendLine(String.Format("        WHERE [id] =  {0}; ", dto.id));

            if (null != dto.AuditoriaItem && dto.AuditoriaItem.Count > 0)
            {
                strUpdate.AppendLine("        ");

                strUpdate.AppendLine(" DELETE FROM [dbAuditoria].[dbo].[tbAuditoriaItem] ");
                strUpdate.AppendLine(String.Format("  WHERE [idAuditoria] = {0}; ", dto.id));

                strUpdate.AppendLine(String.Format("     SET @idAuditoria = {0}; ", dto.id));

                strUpdate.AppendLine("        ");

                strUpdate.AppendLine(" INSERT INTO [dbAuditoria].[dbo].[tbAuditoriaItem]  ");
                strUpdate.AppendLine("            ([idAuditoria]  ");
                strUpdate.AppendLine("            ,[idSubTiposDocumentos]  ");
                strUpdate.AppendLine("            ,[DocumentosDescricao])  ");
                strUpdate.AppendLine("      VALUES  ");

                int total = dto.AuditoriaItem.Count;
                int index = 0;
                foreach (var item in dto.AuditoriaItem)
                {
                    strUpdate.AppendLine("        (");

                    strUpdate.Append("        @idAuditoria ");
                    strUpdate.Append("        ," + item.idSubTiposDocumentos);
                    strUpdate.AppendLine(String.Format("        ,'{0}'", item.DocumentosDescricao));

                    if (index + 1 < total)
                    {
                        strUpdate.AppendLine("        ),");
                    }
                    else
                    {
                        strUpdate.AppendLine("        );");
                    }
                    index++;
                }

                strUpdate.AppendLine("        ");
            }

            Repository<Auditoria>.Insert(strUpdate.ToString(), enumConnectionString.DapperAuditoria);
        }

        #region [Gets]
        public IList<SubTiposDocumentos> GetSubTiposDocumentos(int IdTipoDocumento)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" select id as 'IdSubTipoDocumento',IdTipoDocumento ,Descricao from [dbAuditoria].[dbo].[tbSubTiposDocumentos]");
            if(IdTipoDocumento > 0)
                sb.Append( String.Format( " WHERE IdTipoDocumento = {0};", IdTipoDocumento));

            var result = Repository<SubTiposDocumentos>.getList(sb.ToString(), enumConnectionString.DapperAuditoria);
            return null != result ? result.ToList() : null;
        }

        public IList<Auditoria> GetAuditoria(int start, int end)
        {
            StringBuilder sb = new StringBuilder();
            #region
            sb.Append("                                                                 ");
            sb.Append(" BEGIN                                                           ");
            sb.Append(" 	DECLARE @START INT;                                         ");
            sb.Append(" 	DECLARE @END INT;                                           ");
            sb.Append(" 	                                                            ");
            sb.Append(String.Format(" 	SET @START =  {0};                       ", start));
            sb.Append(String.Format(" 	SET @END = {0};                            ", end));
            sb.Append("                                                                 ");
            sb.Append(" 	DECLARE @tbAuditoria TABLE                                  ");
            sb.Append(" 	(                                                           ");
            sb.Append(" 		id int ,                                                ");
            sb.Append(" 		idLoja int,	Nomeloja varchar(300),                                             ");
            sb.Append(" 		idPessoa int,NomePessoa varchar(300),                                           ");
            sb.Append(" 		Obs Varchar(2000),                                      ");
            sb.Append(" 		DataCadastro Datetime,                                  ");
            sb.Append(" 		idUsuario int                                           ");
            sb.Append(" 	);                                                          ");
            sb.Append(" 	                                                            ");
            sb.Append(" 	                                                            ");
            sb.Append(" 	INSERT INTO @tbAuditoria                                    ");
            sb.Append(" 	(                                                           ");
            sb.Append(" 		id ,                                                    ");
            sb.Append(" 		idLoja,  Nomeloja ,                                               ");
            sb.Append(" 		idPessoa , NomePessoa,                                             ");
            sb.Append(" 		Obs ,                                                   ");
            sb.Append(" 		DataCadastro ,                                          ");
            sb.Append(" 		idUsuario                                               ");
            sb.Append(" 	)                                                           ");
            sb.Append(" 	SELECT                                                      ");
            sb.Append(" 			P.id                                                ");
            sb.Append(" 			,P.idLoja  ,P.Nomeloja                                         ");
            sb.Append(" 			,P.idPessoa ,P.NomePessoa                                         ");
            sb.Append(" 			,P.Obs                                              ");
            sb.Append(" 			,P.DataCadastro                                     ");
            sb.Append(" 			,P.idUsuario                                        ");
            sb.Append(" 	FROM                                                        ");
            sb.Append(" 	(                                                           ");
            sb.Append(" 		SELECT                                                  ");
            sb.Append(" 			ROW_NUMBER() OVER(ORDER BY A.id ASC) AS Row#,       ");
            sb.Append(" 			A.id                                                ");
            sb.Append(" 			,A.idLoja ,A.Nomeloja                                          ");
            sb.Append(" 			,A.idPessoa ,A.NomePessoa                                        ");
            sb.Append(" 			,A.Obs                                              ");
            sb.Append(" 			,A.DataCadastro                                     ");
            sb.Append(" 			,A.idUsuario                                        ");
            sb.Append(" 		FROM                                                    ");
            sb.Append(" 		dbo.tbAuditoria A                                       ");
            sb.Append(" 	) AS P                                                      ");
            sb.Append(" 	where P.Row# BETWEEN @START AND @END ;                      ");
            sb.Append(" 	                                                            ");
            sb.Append(" 	SELECT * FROM  @tbAuditoria;                                ");
            sb.Append(" 	                                                            ");
            sb.Append(" END                                                             ");
            #endregion
            var result = Repository<Auditoria>.getList(sb.ToString(), enumConnectionString.DapperAuditoria);
            return null != result ? result.ToList() : null;
        }

        public IList<AuditoriaItem> GetAuditoriaItem(string listAuditoria)
        {
            StringBuilder sb = new StringBuilder();
            #region
            sb.Append(String.Format( " select * from dbo.tbAuditoriaItem where idAuditoria in ({0});      ", listAuditoria));
            #endregion
            var result = Repository<AuditoriaItem>.getList(sb.ToString(), enumConnectionString.DapperAuditoria);
            return null != result ? result.ToList() : null;
        }

        public IList<AuditoriaItem> GetAuditoriaItemByID(int idAuditoria)
        {
            StringBuilder sb = new StringBuilder();
            #region
            sb.Append(String.Format(" select * from dbo.tbAuditoriaItem where idAuditoria = {0};      ", idAuditoria));
            #endregion
            var result = Repository<AuditoriaItem>.getList(sb.ToString(), enumConnectionString.DapperAuditoria);
            return null != result ? result.ToList() : null;
        }

        public Auditoria GetAuditoria(int id)
        {
            StringBuilder strGet = new StringBuilder();

            strGet.AppendLine(" SELECT                                                                              ");
            strGet.AppendLine(" 	A.[id]                                                                             ");
            strGet.AppendLine(" 	,A.[idLoja]                                                                        ");
            strGet.AppendLine(" 	,A.[Nomeloja]                                                                      ");
            strGet.AppendLine(" 	,A.[idPessoa]                                                                      ");
            strGet.AppendLine(" 	,A.[NomePessoa]                                                                    ");
            strGet.AppendLine(" 	,A.[Obs]                                                                           ");
            strGet.AppendLine(" 	,A.[DataCadastro]                                                                  ");
            strGet.AppendLine(" 	,A.[idUsuario]                                                                     ");
            strGet.AppendLine(" 	,AI.[id]                                                                           ");
            strGet.AppendLine(" 	,AI.[idAuditoria]                                                                  ");
            strGet.AppendLine(" 	,AI.[idSubTiposDocumentos]                                                         ");
            strGet.AppendLine(" 	,AI.[DocumentosDescricao]                                                          ");
            strGet.AppendLine(" FROM [dbAuditoria].[dbo].[tbAuditoria] A INNER JOIN [dbAuditoria].[dbo].[tbAuditoriaItem] AI   ");
            strGet.AppendLine(" ON A.[id] = AI.[idAuditoria]                                                       ");
            strGet.AppendLine(String.Format("    WHERE A.[id] =  {0} ;  ", id));

            var result = Repository<Auditoria>.GetAuditoria(strGet.ToString(), enumConnectionString.DapperAuditoria);
            return null != result ? result : null;
        }

        public int GetCountAuditoria()
        {
            StringBuilder sb = new StringBuilder();
            #region
            sb.Append("   SELECT count(id) FROM dbo.tbAuditoria  ");

            #endregion
            return Repository<AuditoriaList>.getCount(sb.ToString(), enumConnectionString.DapperAuditoria);
        }
        #endregion
    }
}