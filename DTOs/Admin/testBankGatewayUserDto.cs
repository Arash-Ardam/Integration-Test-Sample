using TPG.SI.TadbirPay.URLTest.Enums;

namespace TPG.SI.TadbirPay.URLTest.DTOs.Admin
{
    public class testBankGatewayUserDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BankGatewayId { get; set; }
        public testBankGatewayUserTypeEnum UserType { get; set; }
        public bool IsEnable { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;
    }
}
