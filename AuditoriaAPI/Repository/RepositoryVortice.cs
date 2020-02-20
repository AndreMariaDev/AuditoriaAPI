using System;
using System.Collections.Generic;
using System.Text;
using AuditoriaAPI.Model;
using AuditoriaAPI.Util;
using System.Linq;


namespace AuditoriaAPI.Repository
{
    public class RepositoryVortice
    {
        static FileCreator fileCreator = new FileCreator();
        #region [Gets]
        public IList<Loja> GetLoja()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT * FROM  [Vortice2].[dbo].[tbLoja]");

            var result = Repository<Loja>.getList(sb.ToString(), enumConnectionString.DapperVortice2);
            return null != result ? result.ToList() : null;
        }

        public IList<Loja> GetLojaByListId(String listId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(String.Format( " SELECT * FROM  [Vortice2].[dbo].[tbLoja] where ID in ( {0} )", listId));

            var result = Repository<Loja>.getList(sb.ToString(), enumConnectionString.DapperVortice2);
            return null != result ? result.ToList() : null;
        }

        public IList<Loja> GetSearchLoja(String Nome)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT * FROM  [Vortice2].[dbo].[tbLoja]");
            sb.Append(String.Format( " WHERE UPPER(sNome) like upper('%{0}%');", Nome));

            var result = Repository<Loja>.getList(sb.ToString(), enumConnectionString.DapperVortice2);
            return null != result ? result.ToList() : null;
        }

        public IList<Pessoa> GetPessoaPorLoja(int idLoja)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT P.id, P.sNome ");
            sb.Append(" FROM ");
            sb.Append(" [Vortice2].[dbo].[tbPessoa] P INNER JOIN [Vortice2].[dbo].[tbPessoaFisica] PF ON P.id = PF.idPessoa ");
            sb.Append(" INNER JOIN [Vortice2].[dbo].[tbPessoaFuncionario] PFC ON PF.idPessoa = PFC.idPessoa ");
            sb.Append(" INNER JOIN [Vortice2].[dbo].[tbPessoaFuncionarioLoja] PFL ON PF.ID = PFL.idPessoaFuncionario ");
            sb.Append(" INNER JOIN [Vortice2].[dbo].[tbLoja] L ON L.ID = PFL.idLoja ");
            sb.Append(String.Format( " WHERE L.ID = {0} ;", idLoja));

            var result = Repository<Pessoa>.getList(sb.ToString(), enumConnectionString.DapperVortice2);
            return null != result ? result.ToList() : null;
        }

        public IList<Pessoa> GetPessoaPorListId(String listId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT P.id, P.sNome ");
            sb.Append(" FROM ");
            sb.Append(" [Vortice2].[dbo].[tbPessoa] P");
            sb.Append(String.Format(" WHERE P.id in ( {0} );",listId));

            var result = Repository<Pessoa>.getList(sb.ToString(), enumConnectionString.DapperVortice2);
            return null != result ? result.ToList() : null;
        }
        #endregion
    }
}