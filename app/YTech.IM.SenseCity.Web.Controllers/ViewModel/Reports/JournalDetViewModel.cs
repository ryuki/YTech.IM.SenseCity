﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.IM.SenseCity.Core.Transaction.Accounting;

namespace YTech.IM.SenseCity.Web.Controllers.ViewModel.Reports
{
    public class JournalDetViewModel : TJournalDet
    {
        public string AccountName { get; set; }
        public string JournalDate { get; set; }
        public string CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public string JournalVoucherNo { get; set; }
    }
}
