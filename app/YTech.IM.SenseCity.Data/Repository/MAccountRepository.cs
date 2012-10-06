﻿using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class MAccountRepository : NHibernateRepositoryWithTypedId<MAccount, string>, IMAccountRepository
    {
        public IEnumerable<MAccount> GetPagedAccountList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, MAccountCat accountCat)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MAccount));

            ////calculate total rows
            //totalRows = Session.CreateCriteria(typeof(MAccount))
            //    .SetProjection(Projections.RowCount())
            //    .FutureValue<int>().Value;

            ////get list results
            //if (maxRows != 0)
            //{
            //    criteria.SetMaxResults(maxRows)
            //        .SetFirstResult((pageIndex - 1)*maxRows);
            //}

            //  criteria.AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
            //  ;
            criteria.Add(Expression.Eq("AccountCatId", accountCat));
            criteria.Add(Expression.IsNull("AccountParentId"));
            criteria.SetCacheable(true);
            IEnumerable<MAccount> list = criteria.List<MAccount>();
            return list;

            IQuery q = Session.CreateQuery(
                     @"
            select distinct acc
            from MAccount as acc
                left outer join fetch acc.Children
                where acc.AccountCatId = :accountCatType
               
");
            q.SetEntity("AccountCatId", accountCat);
            return q.List<MAccount>();
        }

        public IList<MAccount> GetByAccountCat(MAccountCat accountCat)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MAccount));
            criteria.Add(Expression.Eq("AccountCatId", accountCat));
            criteria.SetCacheable(true);
            IList<MAccount> list = criteria.List<MAccount>();
            return list;
        }
    }
}
