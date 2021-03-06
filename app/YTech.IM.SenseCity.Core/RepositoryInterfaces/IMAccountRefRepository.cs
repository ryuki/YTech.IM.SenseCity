﻿using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface IMAccountRefRepository : INHibernateRepositoryWithTypedId<MAccountRef, string>
    {

        MAccountRef GetByRefTableId(Enums.EnumReferenceTable enumReferenceTable, string warehouseId);
    }
}
