﻿using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface IMWarehouseRepository : INHibernateRepositoryWithTypedId<MWarehouse, string>
    {
        IEnumerable<MWarehouse> GetPagedWarehouseList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows);
    }
}
