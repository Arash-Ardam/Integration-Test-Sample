using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPG.SI.TadbirPay.URLTest.Enums
{
    public enum testApproveStatusEnum
    {

        /// <summary> در انتظار تائید</summary>
        [Display(Name = "در انتظار تائید")]
        WaitForAction = 0,

        /// <summary> تائید شده</summary>
        [Display(Name = "تائید شده")]
        Accepted = 1,

        /// <summary> رد شده</summary>
        [Display(Name = "رد شده")]
        Rejected = 2
    }
}
