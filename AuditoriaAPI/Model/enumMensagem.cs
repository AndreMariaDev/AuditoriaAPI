using System.ComponentModel;

namespace AuditoriaAPI.Model
{
    public enum enumMensagem
    {
        [Description("Iniciando a aplicação de envio de Emails com @ do mês Ref.:")]
        Iniciar = 0,
        [Description("Realizando consulta ao banco de dados")]
        Realizando = 1,
        [Description("OCORREU UM ERRO AO GERAR O XLS @!")]
        Ocorreu = 2,
        [Description("Pressione ENTER para sair... ")]
        Pressione = 3,
        [Description("Planilhas gerada com sucesso. Em seguida será enviada por email para: ")]
        Planilhas = 4,
        [Description("Relatório enviado com sucesso! ")]
        Sucesso = 5,
        [Description("[ERRO] - Não foi possível criar o @! ")]
        ErroRelatorio = 6,
        [Description("[Warning] - Tabela: @")]
        ExisteTabela = 7,
        [Description("[ERRO] -A tabela @ não existe na base de dados")]
        NaoExisteTabela = 8,
        [Description("[ERRO] -O caminho correspondente a @ não existe!")]
        NaoExisteCaminho = 9
    }
}