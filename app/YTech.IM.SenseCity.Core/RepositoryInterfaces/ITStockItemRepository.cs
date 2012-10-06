using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Inventory;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface ITStockItemRepository : INHibernateRepositoryWithTypedId<TStockItem, string>
    {
        TStockItem GetByItemAndWarehouse(MItem mItem, MWarehouse mWarehouse);

        IList<TStockItem> GetByItemWarehouse(MItem item, MWarehouse warehouse);
    }
}
