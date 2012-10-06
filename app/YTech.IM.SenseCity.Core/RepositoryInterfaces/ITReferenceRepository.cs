using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Enums;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface ITReferenceRepository : INHibernateRepositoryWithTypedId<TReference, string>
    {
        TReference GetByReferenceType(EnumReferenceType referenceType);

    }
}
