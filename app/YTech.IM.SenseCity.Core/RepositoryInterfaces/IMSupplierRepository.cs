﻿using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface IMSupplierRepository : INHibernateRepositoryWithTypedId<MSupplier, string>
    {
        IEnumerable<MSupplier> GetPagedSupplierList(string orderCol,string orderBy,int pageIndex,int maxRows,ref int totalRows);

    }
}