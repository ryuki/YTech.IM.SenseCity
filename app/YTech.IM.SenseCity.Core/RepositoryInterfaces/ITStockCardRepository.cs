﻿using System;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Inventory;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface ITStockCardRepository : INHibernateRepositoryWithTypedId<TStockCard, string>
    {
        IList<TStockCard> GetByDateItemWarehouse(DateTime? dateFrom, DateTime? dateTo, MItem item,MWarehouse warehouse);
    }
}
