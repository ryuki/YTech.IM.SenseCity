using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction.HR;
using System.Text;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class TAbsentRepository : NHibernateRepositoryWithTypedId<TAbsent, string>, ITAbsentRepository
    {

        #region ITAbsentRepository Members

        public IList<TAbsent> GetAbsent(DateTime? dayWork)
        {
            ICriteria criteria = Session.CreateCriteria(typeof (TAbsent))
                .Add(Expression.Eq("AbsentDate", dayWork));
            return criteria.List<TAbsent>();
        }

        public IList<TAbsent> GetAbsentByEmployeeId(MEmployee employeeId)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TAbsent))
                .Add(Expression.Eq("EmployeeId", employeeId));
            return criteria.List<TAbsent>();
        }

        public IList<TAbsent> GetAbsentByEmployeeId(MEmployee employeeId, DateTime? dayWork)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select ta
                                from TAbsent as ta");
            if (employeeId != null)
            {
                sql.AppendLine(
                    @" where ta.EmployeeId = :employeeId");
            }

            if (dayWork.HasValue)
            {
                sql.AppendLine(
                    @"  and (ta.AbsentDate = :dayWork)");
            }

            IQuery q = Session.CreateQuery(sql.ToString());

            q.SetEntity("employeeId", employeeId);
            if (dayWork.HasValue)
            {
                q.SetDateTime("dayWork", dayWork.Value);
            }
            
            return q.List<TAbsent>();
        }

        public IList<TAbsent> GetAbsentByEmployeeId(MEmployee employeeId, DateTime startPeriod, DateTime endPeriod)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select ta
                                from TAbsent as ta");
            if (employeeId != null)
            {
                sql.AppendLine(
                    @" where ta.EmployeeId = :employeeId and (ta.AbsentDate between :startPeriod and :endPeriod)");
            }

            IQuery q = Session.CreateQuery(sql.ToString());
           
            q.SetEntity("employeeId", employeeId);
            q.SetDateTime("startPeriod", startPeriod);
            q.SetDateTime("endPeriod", endPeriod);

            return q.List<TAbsent>();
        }

        #endregion
    }
}
