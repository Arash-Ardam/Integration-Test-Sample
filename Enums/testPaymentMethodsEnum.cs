using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPG.SI.TadbirPay.URLTest.Enums
{
    public enum testPaymentMethodsEnum
    {
        /// <summary>
        /// نامشخص
        /// </summary>
        [Display(Name = "نامشخص")]
        Unknown = -1,
        /// <summary>
        /// داخلی
        /// </summary>

        [Display(Name = "داخلی")]
        Internal = 0,

        /// <summary>
        /// پایا
        /// </summary>
        [Display(Name = "پایا")]
        Paya = 1,

        /// <summary>
        /// ساتنا
        /// </summary>

        [Display(Name = "ساتنا")]
        Satna = 2,

        /// <summary>
        /// کارت به کارت
        /// </summary>

        [Display(Name = "کارت به کارت")]
        Card = 3
    }
}
