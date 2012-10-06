using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;

namespace YTech.IM.SenseCity.Data.Repository
{
   public class MPacketRepository : NHibernateRepositoryWithTypedId<MPacket, string>, IMPacketRepository
    {
        #region IMPacketRepository Members

        public IEnumerable<MPacket> GetPagedPacketList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MPacket));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MPacket))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MPacket> list = criteria.List<MPacket>();
            return list;
        }

        #endregion
    }
}
