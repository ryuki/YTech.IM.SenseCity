﻿using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class MEmployeeRepository : NHibernateRepositoryWithTypedId<MEmployee, string>, IMEmployeeRepository
    {
        public IEnumerable<MEmployee> GetPagedEmployeeList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MEmployee));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MEmployee))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MEmployee> list = criteria.List<MEmployee>();
            return list;
        }
    }
}
