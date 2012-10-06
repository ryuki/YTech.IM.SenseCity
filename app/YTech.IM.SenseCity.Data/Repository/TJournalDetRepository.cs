﻿using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction.Accounting;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class TJournalDetRepository : NHibernateRepositoryWithTypedId<TJournalDet, string>, ITJournalDetRepository
    {
        public IList<TJournalDet> GetForReport(DateTime? dateFrom, DateTime? dateTo, MCostCenter costCenter)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  select det
                                from TJournalDet as det
                                    inner join det.JournalId j");
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                sql.AppendLine(@"   where j.JournalDate between :dateFrom and :dateTo");
            }
            if (costCenter != null)
            {
                sql.AppendLine(@"   and j.CostCenterId = :costCenter");
            }

            IQuery q = Session.CreateQuery(sql.ToString());
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                q.SetDateTime("dateFrom", dateFrom.Value);
                q.SetDateTime("dateTo", dateTo.Value);
            }
            if (costCenter != null)
            {
                q.SetEntity("costCenter", costCenter);
            }


            return q.List<TJournalDet>();
        }
    }
}
