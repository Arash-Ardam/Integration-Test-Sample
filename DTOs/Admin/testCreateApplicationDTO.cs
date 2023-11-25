using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPG.SI.TadbirPay.URLTest.DTOs.Admin
{
    public class testCreateApplicationDTO
    {
        public Guid? id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsEnable { get; set; }
    }
}
