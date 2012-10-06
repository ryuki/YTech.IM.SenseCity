using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Inventory;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class TTransRoomRepository : NHibernateRepositoryWithTypedId<TTransRoom, string>, ITTransRoomRepository
    {
        public TTransRoom GetByRoom(MRoom room)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select troom
                                from TTransRoom as troom
                                    where troom.RoomStatus = :RoomStatus ");
            if (room != null)
            {
                sql.AppendLine(@"   and troom.RoomId = :room");
            }
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("RoomStatus", Enums.EnumTransRoomStatus.In.ToString());
            if (room != null)
            {
                q.SetEntity("room", room);
            }
            return q.UniqueResult<TTransRoom>();
        }

        public IList<TTransRoom> GetListByTransDate(DateTime? dateFrom, DateTime? dateTo)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select troom
                                from TTransRoom as troom
                                    where troom.RoomOutDate between :dateFrom and :dateTo ");
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetDateTime("dateFrom", dateFrom.Value);
            q.SetDateTime("dateTo", dateTo.Value);
            return q.List<TTransRoom>();
        }

        public IEnumerable<TTransRoom> GetPagedTransRoomList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string searchBy, string searchText)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TTransRoom as troom
                                    left outer join troom.TransId trans ");
            if (!string.IsNullOrEmpty(searchText))
            {
                sql.AppendFormat(@" where {0} like :searchText", searchBy);
            }
            //sql.AppendFormat(@" order by  {0} {1}", orderCol, orderBy);

            string queryCount = string.Format(" select count(troom.Id) {0}", sql);
            IQuery q = Session.CreateQuery(queryCount);
            if (!string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", string.Format("%{0}%", searchText));
            }

            totalRows = Convert.ToInt32(q.UniqueResult());
            //totalRows = (int)q.UniqueResult();// q.FutureValue<int>().Value;


            string query = string.Format(" select troom {0}", sql);
            q = Session.CreateQuery(query);
            if (!string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", string.Format("%{0}%", searchText));
            }
            q.SetMaxResults(maxRows);
            q.SetFirstResult((pageIndex - 1) * maxRows);
            IEnumerable<TTransRoom> list = q.List<TTransRoom>();
            return list;
        }
    }
}
