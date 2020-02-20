using System.ComponentModel;
using System.Configuration;

namespace AuditoriaAPI.Model
{
    public enum enumConnectionString
    {

        [Description("DapperAuditoria")]
        DapperAuditoria = 0,
        [Description("DapperVortice2")]
        DapperVortice2 = 1,
    }
}