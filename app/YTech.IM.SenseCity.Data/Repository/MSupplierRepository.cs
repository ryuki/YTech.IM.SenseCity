﻿using System;
using System.Collections;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using NHibernate;
using NHibernate.Criterion;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class MSupplierRepository : NHibernateRepositoryWithTypedId<MSupplier, string>, IMSupplierRepository
    {
        public IEnumerable<MSupplier> GetPagedSupplierList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MSupplier));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MSupplier))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MSupplier> list = criteria.List<MSupplier>();
            return list;
        }
    }
}
