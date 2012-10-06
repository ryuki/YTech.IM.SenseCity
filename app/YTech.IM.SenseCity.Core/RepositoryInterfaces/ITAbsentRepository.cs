using System;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.HR;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface ITAbsentRepository : INHibernateRepositoryWithTypedId<TAbsent, string>
    {
        IList<TAbsent> GetAbsent(DateTime? dayWork);
        IList<TAbsent> GetAbsentByEmployeeId(MEmployee employeeId);
        IList<TAbsent> GetAbsentByEmployeeId(MEmployee employeeId, DateTime? workDay);
        IList<TAbsent> GetAbsentByEmployeeId(MEmployee employeeId, DateTime startPeriod, DateTime endPeriod);
    }
}
