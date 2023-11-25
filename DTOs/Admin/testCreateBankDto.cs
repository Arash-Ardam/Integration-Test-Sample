using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPG.SI.TadbirPay.URLTest.Enums;

namespace TPG.SI.TadbirPay.URLTest.DTOs.Admin
{
    public class testCreateBankDto
    {
        public testBankEnum ProviderCode { get; set; }
        public string Code { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public bool IsEnable { get; set; }
        public long MinimumSatnaAmount { get; set; }
    }
}
