using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class MCustomerRepository : NHibernateRepositoryWithTypedId<MCustomer, string>, IMCustomerRepository
    {
        #region Implementation of IMCustomerRepository

        public IEnumerable<MCustomer> GetPagedCustomerList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string searchBy, string searchText)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from MCustomer as cust
                                    left outer join cust.PersonId per ");
            if (!string.IsNullOrEmpty(searchText))
            {
                sql.AppendFormat(@" where {0} like :searchText", searchBy);
            }
            //sql.AppendFormat(@" order by  {0} {1}", orderCol, orderBy);

            string queryCount = string.Format(" select count(cust.Id) {0}", sql);
            IQuery q = Session.CreateQuery(queryCount);
            if (!string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", string.Format("%{0}%", searchText));
            }

            totalRows = Convert.ToInt32(q.UniqueResult());
            //totalRows = (int)q.UniqueResult();// q.FutureValue<int>().Value;


            string query = string.Format(" select cust {0}", sql);
            q = Session.CreateQuery(query);
            if (!string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", string.Format("%{0}%", searchText));
            }
            q.SetMaxResults(maxRows);
            q.SetFirstResult((pageIndex - 1) * maxRows);
            IEnumerable<MCustomer> list = q.List<MCustomer>();
            return list;

            //ICriteria criteria = CreateNewCriteria(searchBy, searchText); 

            ////calculate total rows
            //totalRows =  CreateNewCriteria(searchBy, searchText)
            //    .SetProjection(Projections.RowCount())
            //    .FutureValue<int>().Value;

            ////get list results
            //criteria.SetMaxResults(maxRows)
            //  .SetFirstResult((pageIndex - 1) * maxRows)
            //  .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
            //  ;

            //IEnumerable<MCustomer> list = criteria.List<MCustomer>();
            //return list;
        }

        public IEnumerable<MCustomer> GetPagedActiveCustomerList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string searchBy, string searchText)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from MCustomer as cust
                                    left outer join cust.PersonId per 
                                where cust.CustomerExpiredDate >= :today ");
            if (!string.IsNullOrEmpty(searchText))
            {
                sql.AppendFormat(@" and {0} like :searchText", searchBy);
            }
            //sql.AppendFormat(@" order by  {0} {1}", orderCol, orderBy);

            string queryCount = string.Format(" select count(cust.Id) {0}", sql);
            IQuery q = Session.CreateQuery(queryCount);
            if (!string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", string.Format("%{0}%", searchText));
            }
            q.SetDateTime("today", DateTime.Today);

            totalRows = Convert.ToInt32(q.UniqueResult());
            //totalRows = (int)q.UniqueResult();// q.FutureValue<int>().Value;


            string query = string.Format(" select cust {0}", sql);
            q = Session.CreateQuery(query);
            if (!string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", string.Format("%{0}%", searchText));
            }
            q.SetDateTime("today", DateTime.Today);
            q.SetMaxResults(maxRows);
            q.SetFirstResult((pageIndex - 1) * maxRows);
            IEnumerable<MCustomer> list = q.List<MCustomer>();
            return list;
        }

        public IEnumerable<MCustomer> GetCustomerBirthdayList()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from MCustomer as cust
                                    left outer join cust.PersonId per 
                                where cust.CustomerExpiredDate >= :today 
                                    and day(per.PersonDob) = :todate 
                                    and month(per.PersonDob) = :tomonth
                                "); 
            
            string query = string.Format(" select cust {0}", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetDateTime("today", DateTime.Today);
            q.SetInt32("todate", DateTime.Today.Day);
            q.SetInt32("tomonth", DateTime.Today.Month);
            IEnumerable<MCustomer> list = q.List<MCustomer>();
            return list;
        }

        private ICriteria CreateNewCriteria(string searchBy, string searchText)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MCustomer));
            if (!string.IsNullOrEmpty(searchBy))
            {
                criteria.Add(Expression.Like(searchBy, searchText, MatchMode.Anywhere));
            }
            return criteria;
        }

        #endregion
    }
}
