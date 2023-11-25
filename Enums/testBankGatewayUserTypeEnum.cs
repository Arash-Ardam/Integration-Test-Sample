using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPG.SI.TadbirPay.URLTest.Enums
{
    public enum testBankGatewayUserTypeEnum
    {
        /// <summary>
        /// ادمین کارتابل
        /// </summary>
        [Display(Name = "مدیر کارتابل")]
        Admin = 0,

        /// <summary>
        /// امضا کننده حساب
        /// </summary>
        [Display(Name = "امضادار")]
        Approver = 1
    }
}
