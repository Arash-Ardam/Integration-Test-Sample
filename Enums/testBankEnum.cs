using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPG.SI.TadbirPay.URLTest.Enums
{
    public enum testBankEnum
    {
        [Display(Name ="BankTest")]
        BankTest = 007,

        [Display(Name = "بانک مجازی تدبیرپرداز")]
        TadbirVBank = 099,

        [Display(Name = "بانک مرکزی")]
        CentralBank = 010,

        [Display(Name = "بانک صنعت و معدن")]
        SanatMadan = 011,

        [Display(Name = "بانک ملت")]
        Mellat = 012,

        [Display(Name = "بانک رفاه")]
        Refah = 013,

        [Display(Name = "بانک مسکن")]
        Maskan = 014,

        [Display(Name = "بانک سپه")]
        Sepah = 015,

        [Display(Name = "بانک کشاورزی")]
        Keshavarzi = 016,

        [Display(Name = "بانک ملی ایران")]
        Melli = 017,

        [Display(Name = "بانک تجارت")]
        Tejarat = 018,

        [Display(Name = "بانک صادرات")]
        Saderat = 019,

        [Display(Name = "بانک توسعه صادرات")]
        TosehSaderat = 020,

        [Display(Name = "پست بانک ایران")]
        PostBank = 021,

        [Display(Name = "بانک توسعه تعاون")]
        TosehTavon = 022,

        [Display(Name = "موسسه اعتباری توسعه")]
        EtebariToseh = 051,

        [Display(Name = "بانک قوامین")]
        Ghavamin = 052,

        [Display(Name = "بانک کارآفرین")]
        KarAfarin = 053,

        [Display(Name = "بانک پارسیان")]
        Parsian = 054,

        [Display(Name = "بانک اقتصاد نوین")]
        EghtesadNovin = 055,

        [Display(Name = "بانک سامان")]
        Saman = 056,

        [Display(Name = "بانک پاسارگاد")]
        Pasargad = 057,

        [Display(Name = "بانک سرمایه")]
        Sarmayeh = 058,

        [Display(Name = "بانک سینا")]
        Sina = 059,

        [Display(Name = "بانک قرض الحسنه مهر")]
        GharzolHasanehMehr = 060,

        [Display(Name = "بانک شهر")]
        Shahr = 061,

        [Display(Name = "بانک تات")]
        Tat = 062,

        [Display(Name = "بانک انصار")]
        Ansar = 063,

        [Display(Name = "بانک گردشگری")]
        Gardeshgari = 064,

        [Display(Name = "بانک حکمت ایرانیان")]
        HekmatIranian = 065,

        [Display(Name = "بانک دی")]
        Dey = 066,

        [Display(Name = "بانک ایران زمین")]
        IranZamin = 069,

        [Display(Name = "بانک رسالت")]
        Resalat = 070,

        [Display(Name = "بانک خاورمیانه")]
        Khavarmiyaneh = 078,

        [Display(Name = "بانک مهر ایران")]
        MehrIran = 090,

        [Display(Name = "بانک ایران و ونزوئلا")]
        IranVenezuela = 095,

        [Display(Name = "موسسه اعتباری ملل")]
        EtebariMellal = 075,

        [Display(Name = "موسسه اعتباری نور")]
        EtebariNoor = 080,

        Unknown = 0
    }
}
