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
    public class MPromoRepository : NHibernateRepositoryWithTypedId<MPromo, string>, IMPromoRepository
    {
        public IEnumerable<MPromo> GetPagedPromoList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MPromo));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MPromo))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MPromo> list = criteria.List<MPromo>();
            return list;
        }

        public MPromo GetActivePromoByDate(DateTime searchDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select promo
                                from MPromo as promo
                                where promo.PromoStartDate <= :searchDate 
                                    and promo.PromoEndDate >= :searchDate 
            ");
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetDateTime("searchDate", searchDate);

            return q.UniqueResult<MPromo>();
        }
    }
}
