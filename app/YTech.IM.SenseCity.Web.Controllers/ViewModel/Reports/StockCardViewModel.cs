using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.IM.SenseCity.Core.Transaction.Accounting;
using YTech.IM.SenseCity.Core.Transaction.Inventory;

namespace YTech.IM.SenseCity.Web.Controllers.ViewModel.Reports
{
    public class StockCardViewModel : TStockCard
    {
        public string ItemName { get; set; }
        public string WarehouseName { get; set; }
    }
}
