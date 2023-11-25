using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPG.SI.TadbirPay.URLTest.Enums;

namespace TPG.SI.TadbirPay.URLTest.DTOs.Admin
{
    internal class testBankDto
    {
        public Guid Id { get; set; }
        public testBankEnum ProviderCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsEnable { get; set; }
        public long MinimumSatnaAmount { get; set; }
    }
}
