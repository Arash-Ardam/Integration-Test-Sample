using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPG.SI.TadbirPay.URLTest.Enums
{
    public enum testPaymentItemStatusEnum
    {

        /// <summary>
        /// در صف پردازش بانک
        /// </summary>
        [Display(Name = "در صف پردازش بانک")]
        WaitForExecution = 1,


        /// <summary>
        /// درانتظار تائید بانک
        /// </summary>
        [Display(Name = "درانتظار تائید بانک")]
        WaitForBank = 2,


        /// <summary>
        /// تراکنش با موفقیت انجام شده
        /// </summary>
        [Display(Name = "تراکنش انجام شده")]
        BankSucceeded = 3,


        /// <summary>
        /// رد شده توسط بانک
        /// </summary>
        [Display(Name = "رد شده توسط بانک")]
        BankRejected = 4,


        /// <summary>
        /// مبلغ به حساب مبدا برگشت داده شده است
        /// </summary>
        [Display(Name = "برگشت مبلغ به حساب مبدا")]
        TransactionRollback = 5,

        /// <summary>
        /// خطا در ارسال به بانک
        /// </summary>
        [Display(Name = "خطا در ارسال به بانک")]
        Failed = 4,
    }
}
