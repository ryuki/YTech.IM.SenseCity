using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.Transaction.Inventory;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface ITShiftRepository : INHibernateRepositoryWithTypedId<TShift, string>
    {
        TShift GetLastShiftByDate(System.DateTime? shiftDate);

        TShift GetByDateAndShiftNo(System.DateTime? shiftDate, int? shiftNo);
    }
}
