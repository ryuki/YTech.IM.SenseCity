using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Accounting;
using YTech.IM.SenseCity.Enums;

namespace YTech.IM.SenseCity.Web.Controllers.ViewModel
{
    public class GeneralLedgerViewModel
    {
        public static GeneralLedgerViewModel CreateGeneralLedgerViewModel(ITJournalRepository tJournalRepository, IMCostCenterRepository mCostCenterRepository)
        {
            GeneralLedgerViewModel viewModel = new GeneralLedgerViewModel();

            IList<MCostCenter> list = mCostCenterRepository.GetAll();
            MCostCenter costCenter = new MCostCenter();
            costCenter.CostCenterName = "-Pilih Cost Center-";
            list.Insert(0, costCenter);
            viewModel.CostCenterList = new SelectList(list, "Id", "CostCenterName");
            return viewModel;
        }

        public TJournal Journal { get; internal set; }
        public IList<TJournalDet> ListOfJournalDet { get; internal set; }

        public SelectList CostCenterList { get; internal set; }
        public string Title { get; internal set; }
    }
}
