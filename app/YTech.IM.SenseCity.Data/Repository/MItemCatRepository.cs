﻿using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class MItemCatRepository : NHibernateRepositoryWithTypedId<MItemCat, string>, IMItemCatRepository
    {
        public IEnumerable<MItemCat> GetPagedItemCatList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MItemCat));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MItemCat))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MItemCat> list = criteria.List<MItemCat>();
            return list;
        }
    }
}
