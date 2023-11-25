using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPG.SI.TadbirPay.URLTest.Enums;

namespace TPG.SI.TadbirPay.URLTest.DTOs.Payment
{
    public class testRequestTransactionDTO
    {
        public Guid Id { get; set; }

        public List<testPaymentItemRequestDto> Transactions { get; set; }
    }

    public class testPaymentItemRequestDto
    {
        /// <summary>
        /// کد ملی صاحب حساب مقصد
        /// </summary>
        public string NationalCode { get; set; } = string.Empty;

        /// <summary>
        /// شناسه شبای حساب مقصد
        /// </summary>
        public string DestinationIban { get; set; } = string.Empty;


        /// <summary>
        /// شماره حساب مقصد
        /// </summary>
        public string AccountNumber { get; set; } = string.Empty;


        /// <summary>
        /// نام کامل صاحب حساب مقصد که برای انتقال وجه بین بانکی مورد استفاده قرار می گیرد.
        /// </summary>
        public string DestinationAccountOwner { get; set; } = string.Empty;

        /// <summary>
        /// شرح سند واریز به حساب مقصد 
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// مبلغ تراکنش
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// شناسه پرداخت 
        /// </summary>
        public string PaymentNumber { get; set; } = string.Empty;


        /// <summary>
        /// علل پرداخت
        /// </summary>

        public testTransactionReasonEnum ReasonCode { get; set; }
    }

}
