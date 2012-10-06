using System;
using System.Collections;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Inventory;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface ITStockRepository : INHibernateRepositoryWithTypedId<TStock, string>
    {
        IList GetSisaStockList(MItem itemId, MWarehouse mWarehouse);
    }
}
