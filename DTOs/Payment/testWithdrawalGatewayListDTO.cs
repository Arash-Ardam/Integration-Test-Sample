using Elasticsearch.Net;
using Elasticsearch.Net.Specification.SecurityApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPG.SI.TadbirPay.URLTest.Enums;

namespace TPG.SI.TadbirPay.URLTest.DTOs.Payment
{
    public class testWithdrawalGatewayListDTO
    {
        public List<testWithdrawalGatewayDTO> Items { get; set; }
    }

    public class testWithdrawalGatewayDTO
    {
        public Guid BankId { get; set; }
        public string BankName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public string ShebaNumber { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;


        public bool IsEnable { get; set; }

        public bool HasCartable { get; set; }
        public short MinimumSignature { get; set; }

        public List<testBankGatewayUserDto> Users { get; set; }
    }

    public class testBankGatewayUserDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BankGatewayId { get; set; }
        public testBankGatewayUserTypeEnum UserType { get; set; }
        public bool IsEnable { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public DateTimeOffset CreatedDateTime { get; set; }
        public DateTimeOffset UpdatedDateTime { get; set; }
    }

}
