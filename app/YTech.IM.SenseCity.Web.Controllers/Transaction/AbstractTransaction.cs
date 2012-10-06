using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Reporting.WebForms;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Accounting;
using YTech.IM.SenseCity.Core.Transaction.Inventory;
using YTech.IM.SenseCity.Data.Repository;
using YTech.IM.SenseCity.Enums;
using YTech.IM.SenseCity.Web.Controllers.ViewModel;

namespace YTech.IM.SenseCity.Web.Controllers.Transaction
{
    public abstract class AbstractTransaction
    {
        public abstract void SaveJournal(TTrans trans, decimal totalHPP);

        public string UserName;
        public IMAccountRefRepository AccountRefRepository;
        public ITJournalRepository JournalRepository;

        protected string Desc = string.Empty;
        protected string NewVoucher = string.Empty;

        protected TJournal SaveJournalHeader(string newVoucher, TTrans trans, string desc)
        {
            TJournal j = new TJournal();
            j.SetAssignedIdTo(Guid.NewGuid().ToString());
            j.CostCenterId = trans.WarehouseId.CostCenterId;
            j.JournalType = EnumJournalType.GeneralLedger.ToString();
            j.JournalVoucherNo = newVoucher;
            j.JournalPic = trans.TransBy;
            j.JournalDate = trans.TransDate;
            j.JournalEvidenceNo = trans.TransFactur;
            //j.JournalAmmount = ammount;
            j.JournalDesc = desc;

            j.DataStatus = EnumDataStatus.New.ToString();
            j.CreatedBy = UserName;
            j.CreatedDate = DateTime.Now;
            j.JournalDets.Clear();
            return j;
        }

        protected void SaveJournalDet(TJournal journal, string newVoucher, MAccount accountId, EnumJournalStatus journalStatus, decimal ammount, TTrans trans, string desc)
        {
            TJournalDet detToInsert = new TJournalDet(journal);
            detToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
            detToInsert.AccountId = accountId;
            detToInsert.JournalDetStatus = journalStatus.ToString();
            detToInsert.JournalDetEvidenceNo = trans.TransFactur;
            detToInsert.JournalDetAmmount = ammount;
            detToInsert.JournalDetNo = 0;
            detToInsert.JournalDetDesc = desc;
            detToInsert.CreatedBy = UserName;
            detToInsert.CreatedDate = DateTime.Now;
            detToInsert.DataStatus = Enums.EnumDataStatus.New.ToString();
            journal.JournalDets.Add(detToInsert);
        }
    }
}
