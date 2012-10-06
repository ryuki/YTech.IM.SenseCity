﻿using FluentNHibernate.Automapping;
using YTech.IM.SenseCity.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Accounting;

namespace YTech.IM.SenseCity.Data.NHibernateMaps.Transaction
{
    public class TJournalMap : IAutoMappingOverride<TJournal>
    {
        #region Implementation of IAutoMappingOverride<TJournal>

        public void Override(AutoMapping<TJournal> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_JOURNAL");
            mapping.Id(x => x.Id, "JOURNAL_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.CostCenterId, "COST_CENTER_ID").Fetch.Join();
            mapping.Map(x => x.JournalType, "JOURNAL_TYPE");
            mapping.Map(x => x.JournalVoucherNo, "JOURNAL_VOUCHER_NO");
            mapping.Map(x => x.JournalPic, "JOURNAL_PIC");
            mapping.Map(x => x.JournalDate, "JOURNAL_DATE");
            mapping.Map(x => x.JournalEvidenceNo, "JOURNAL_EVIDENCE_NO");
            mapping.Map(x => x.JournalAmmount, "JOURNAL_AMMOUNT");
            mapping.Map(x => x.JournalStatus, "JOURNAL_STATUS");
            mapping.Map(x => x.JournalDesc, "JOURNAL_DESC");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Map(x => x.RowVersion, "ROW_VERSION").ReadOnly();


            mapping.HasMany(x => x.JournalDets)
                .AsBag()
                .Inverse()
                .KeyColumn("JOURNAL_ID")
                .Cascade.All();
        }

        #endregion
    }
}
