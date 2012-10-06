﻿using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class MBrandRepository : NHibernateRepositoryWithTypedId<MBrand, string>, IMBrandRepository
    {
        public IEnumerable<MBrand> GetPagedBrandList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MBrand));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MBrand))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MBrand> list = criteria.List<MBrand>();
            return list;
        }
    }
}
