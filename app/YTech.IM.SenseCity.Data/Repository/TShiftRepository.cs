using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction.Inventory;
using YTech.IM.SenseCity.Enums;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class TShiftRepository : NHibernateRepositoryWithTypedId<TShift, string>, ITShiftRepository
    {
        public TShift GetLastShiftByDate(DateTime? shiftDate)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TShift));
            criteria.Add(Expression.Eq("ShiftDate", shiftDate));
            criteria.AddOrder(Order.Desc("ShiftNo"));
            criteria.SetMaxResults(1);
            IList<TShift> list = criteria.List<TShift>();
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        public TShift GetByDateAndShiftNo(DateTime? shiftDate, int? shiftNo)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TShift));
            criteria.Add(Expression.Eq("ShiftDate", shiftDate));
            criteria.Add(Expression.Eq("ShiftNo", shiftNo));
            IList<TShift> list = criteria.List<TShift>();
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }
    }
}
