using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPG.SI.TadbirPay.URLTest.DTOs.Admin
{
    public class testCreateTenantDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsEnable { get; set; }
        public string ConnectionString { get; set; } = string.Empty;
        public DateTime? CustomDate { get; set; }
    }
}
