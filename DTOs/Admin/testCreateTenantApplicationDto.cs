using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPG.SI.TadbirPay.URLTest.DTOs.Admin
{
    public class testCreateTenantApplicationDto
    {
        public Guid TenantId { get; set; }
        public Guid ApplicationId { get; set; }
        public bool IsEnable { get; set; }
    }
}
