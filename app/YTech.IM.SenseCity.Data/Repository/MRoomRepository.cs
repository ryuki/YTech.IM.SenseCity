using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Enums;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class MRoomRepository : NHibernateRepositoryWithTypedId<MRoom, string>, IMRoomRepository
    {
        #region IMRoomRepository Members

        public IEnumerable<MRoom> GetPagedPacketList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MRoom));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MRoom))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MRoom> list = criteria.List<MRoom>();
            return list;
        }

        public IList<MRoom> GetListByRoomType(EnumRoomType enumRoomType)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select r,troom
                                from TTransRoom as troom
                                right outer join MRoom as r
                                    where troom.RoomStatus = :RoomStatus ");

            ICriteria criteria = Session.CreateCriteria(typeof (MRoom))
                .Add(Expression.Eq("RoomType", enumRoomType.ToString()))
                .Add(Expression.Not(Expression.Eq("RoomStatus", EnumRoomStatus.Off.ToString())))
                .AddOrder(new Order("RoomOrderNo", true))
                ;
            return criteria.List<MRoom>();
        }

        #endregion
    }
}
