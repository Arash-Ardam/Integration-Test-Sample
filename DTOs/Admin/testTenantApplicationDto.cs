using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPG.SI.TadbirPay.URLTest.DTOs.Admin
{
    public class testTenantApplicationDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Tenant { get; set; }
        public Guid ApplicationId { get; set; }
        public string Application { get; set; }
        public bool IsEnable { get; set; }
    }
}
