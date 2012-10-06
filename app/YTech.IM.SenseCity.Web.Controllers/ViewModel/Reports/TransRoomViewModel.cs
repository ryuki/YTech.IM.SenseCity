using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.IM.SenseCity.Core.Transaction;

namespace YTech.IM.SenseCity.Web.Controllers.ViewModel.Reports
{
    public class TransRoomViewModel : TTransRoom
    {
        public decimal? TransSubTotal { get; set; }
        public decimal? TransDiscount { get; set; }
        public DateTime? TransDate { get; set; }
        public string TransFactur { get; set; }
        public decimal? PromoValue { get; set; }
    }
}
