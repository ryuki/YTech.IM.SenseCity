﻿using System;
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
  public  class CashFormViewModel
    {
      public static CashFormViewModel CreateCashFormViewModel(ITJournalRepository tJournalRepository, IMCostCenterRepository mCostCenterRepository,IMAccountRepository mAccountRepository)
        {
            CashFormViewModel viewModel = new CashFormViewModel();

            IList<MCostCenter> list = mCostCenterRepository.GetAll();
            MCostCenter costCenter = new MCostCenter();
            costCenter.CostCenterName = "-Pilih Cost Center-";
            list.Insert(0, costCenter);
            viewModel.CostCenterList = new SelectList(list, "Id", "CostCenterName");

            IList<MAccount> listAcc = mAccountRepository.GetAll();
            MAccount account = new MAccount();
            account.AccountName = "-Pilih Akun Kas-";
            listAcc.Insert(0, account);
            viewModel.AccountList = new SelectList(listAcc, "Id", "AccountName");

            return viewModel;
        }

      public TJournal Journal { get; internal set; }
      public string CashAccountId { get; internal set; }
      public string CashAccountName { get; internal set; }
        public IList<TJournalDet> ListOfJournalDet { get; internal set; }

        public SelectList CostCenterList { get; internal set; }
        public SelectList AccountList { get; internal set; }
        public string Title { get; internal set; }
    }
}
