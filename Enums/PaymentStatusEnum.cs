using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPG.SI.TadbirPay.URLTest.Enums
{
   
        public enum PaymentStatusEnum
        {
            /// <summary>
            /// در انتظار تائید امضاداران
            /// </summary>
            [Display(Name = "در انتظار تائید")]
            WaitingForOwnersApproval = 0,


            /// <summary>
            /// تائید شده توسط امضاداران
            /// </summary>
            [Display(Name = "تائید شده")]
            OwnersApproved = 1,


            /// <summary>
            /// ارسال شده به بانک جهت پردازش
            /// </summary>
            [Display(Name = "ارسال شده به بانک")]
            SubmittedToBank = 2,


            /// <summary>
            /// تراکنش با موفقیت انجام شده
            /// 
            /// </summary>
            [Display(Name = "انجام شده")]
            BankSucceeded = 3,


            /// <summary>
            /// عدم تائید توسط امضا داران
            /// </summary>
            [Display(Name = "عدم تائید")]
            OwnerRejected = 4,


            /// <summary>
            /// رد شده توسط بانک
            /// </summary>
            [Display(Name = "رد شده توسط بانک")]
            BankRejected = 5,


            /// <summary>
            /// پیش نویس
            /// درخواست در انتظار تاید از سوی برنامه صادر کننده تراکنش میباشد
            /// </summary>
            [Display(Name = "پیش نویس")]
            Draft = 6
        }
    
}
