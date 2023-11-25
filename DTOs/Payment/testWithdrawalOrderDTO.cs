using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPG.SI.TadbirPay.URLTest.Enums;

namespace TPG.SI.TadbirPay.URLTest.DTOs.Payment
{
    internal class testWithdrawalOrderDTO
    {


        /// <summary>
        /// کد بانک مبدا
        /// </summary>
        public testBankEnum ProviderCode { get; set; }
        public string BankCode { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string GatewayTitle { get; set; } = string.Empty;



        /// <summary>
        /// کد پیگیری ارائه شده از سوی بانک
        /// </summary>
        public string TrackingId { get; set; } = string.Empty;

        /// <summary>
        /// عنوان تراکنش
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// توضیحات دستور پرداخت
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// شماره شبای حساب مبدا
        /// </summary>

        public string SourceIban { get; set; } = string.Empty;


        /// <summary>
        /// شناسه حساب تعریف شده در ادمین
        /// </summary>
        public Guid BankGatewayId { get; set; }

        /// <summary>
        /// مبلغ کل تراکنش
        /// </summary>

        public string TotalAmount { get; set; }

        /// <summary>
        /// تعداد ردیف تراکنش
        /// </summary>

        public string NumberOfTransactions { get; set; }
        public PaymentStatusEnum Status { get; set; }

        public List<testWithdrawalOrderLogDto> ChangeHistory { get; set; }
        public List<testWithdrawalTransactionDto> Transactions { get; set; }
        public List<testWithdrawalApproverDto> Approvers { get; set; }
    }

    public class testWithdrawalOrderLogDto
    {

        public Guid WithdrawalOrderId { get; set; }

        public PaymentStatusEnum Status { get; set; }

        public string Description { get; set; } = string.Empty;
    }

    public class testWithdrawalTransactionDto
    {


        /// <summary>
        /// شناسه پیگیری تراکنش که باید در هر دستور منحصربفرد باشد. 
        ///این پارامتر اختیاری است و در صورت عدم مقداردهی توسط
        ///کالینت، به صورت خودکار در پاسخ توسط سامانه مقداردهی می شود
        /// </summary>
        public string TrackingId { get; set; } = string.Empty;

        /// <summary>
        /// شناسه شبای حساب مقصد
        /// </summary>

        public string DestinationIban { get; set; } = string.Empty;

        /// <summary>
        /// کد ملی صاحب حساب مقصد
        /// </summary>
        public string NationalCode { get; set; } = string.Empty;

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
        /// پاسخ بانک
        /// </summary>
        public string ProviderMessage { get; set; } = string.Empty;


        /// <summary>
        /// مبلغ تراکنش
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// شناسه پرداخت 
        /// </summary>
        public string PaymentNumber { get; set; } = string.Empty;


        /// <summary>
        /// علل پرداخت
        /// </summary>

        public testTransactionReasonEnum ReasonCode { get; set; }

        /// <summary>
        /// شماره صف ارسال
        /// </summary>
        public int RowNumber { get; set; }

        public testPaymentItemStatusEnum Status { get; set; }

        /// <summary>
        /// نوع تراکنش
        /// </summary>
        public testPaymentMethodsEnum PaymentType { get; set; }

        public Guid WithdrawalOrderId { get; set; }
    }

    public class testWithdrawalApproverDto
    {
        public Guid Id { get; set; }

        public testApproveStatusEnum Status { get; set; }

        public Guid ApproverId { get; set; }

        public string ApproverName { get; set; }

        public Guid WithdrawalOrderId { get; set; }

        public DateTimeOffset CreatedDateTime { get; set; }
    }

}
