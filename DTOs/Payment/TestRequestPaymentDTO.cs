using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPG.SI.TadbirPay.URLTest.DTOs.Payment
{
    public class TestRequestPaymentDTO
    {
        /// <summary>
        /// عنوان تراکنش
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// مبلغ کل تراکنش
        /// مجموع مبالغ تراکنشها
        /// </summary>
        public long TotalAmount { get; set; }


        /// <summary>
        /// تعداد سطر تراکنش ها
        /// </summary>
        public int NumberOfTransactions { get; set; }

        /// <summary>
        /// شناسه حساب تعریف شده در ادمین
        /// </summary>
        public Guid BankGatewayId { get; set; }
    }
}
