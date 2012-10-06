using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Accounting;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface ITJournalRepository : INHibernateRepositoryWithTypedId<TJournal, string>
    {
        System.DateTime? GetMinDateJournal();
    }
}
