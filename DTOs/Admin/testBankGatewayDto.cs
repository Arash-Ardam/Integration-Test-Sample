using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPG.SI.TadbirPay.Infrastructure.Enums;

namespace TPG.SI.TadbirPay.URLTest.DTOs.Admin
{
    public class testBankGatewayDto
    {
        public Guid? id { get; set; }
        public Guid BankId { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid TenantId { get; set; }
        public string Merchant { get; set; } = string.Empty;
        public string TerminalId { get; set; } = string.Empty;
        public AuthenticationTypeEnum AuthenticationType { get; set; }

        /// <summary>
        /// can be : "USerName","ClientCId","ApiKey"
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// can be: "Password","Client Secret"
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// If the bank uses encryption to send data, it must provide the encryption key
        /// </summary>
        public string ShebaNumber { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;

       
        public bool IsEnable { get; set; }
        public BankGatewayTypeEnum Type { get; set; }


        public bool HasCartable { get; set; }
        public short MinimumSignature { get; set; }
    }
}
