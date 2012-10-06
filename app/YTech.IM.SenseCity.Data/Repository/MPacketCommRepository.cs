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
    public class MPacketCommRepository : NHibernateRepositoryWithTypedId<MPacketComm, string>, IMPacketCommRepository
    {
        #region IMPacketRepository Members

        public IEnumerable<MPacketComm> GetPagedPacketCommList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string employeeId)
        {
            //get employee entitiy
            //MEmployee emp = Session.

            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from MPacketComm as p ");
            if (!string.IsNullOrEmpty(employeeId))
            {
                sql.AppendLine(@" where p.EmployeeId.Id = :employeeId");
            }

            string queryCount = string.Format(" select count(p.Id) {0}", sql);
            IQuery q = Session.CreateQuery(queryCount);
            if (!string.IsNullOrEmpty(employeeId))
            {
                q.SetString("employeeId", employeeId);
            }

            totalRows = Convert.ToInt32(q.UniqueResult());
            //totalRows = (int)q.UniqueResult();// q.FutureValue<int>().Value;


            sql.AppendFormat(@" order by  p.{0} {1}", orderCol, orderBy);
            string query = string.Format(" select p {0}", sql);
            q = Session.CreateQuery(query);
            if (!string.IsNullOrEmpty(employeeId))
            {
                q.SetString("employeeId", employeeId);
            }
            q.SetMaxResults(maxRows);
            q.SetFirstResult((pageIndex - 1) * maxRows);
            IEnumerable<MPacketComm> list = q.List<MPacketComm>();
            return list;


            //ICriteria criteria = Session.CreateCriteria(typeof(MPacketComm));

            ////calculate total rows
            //totalRows = Session.CreateCriteria(typeof(MPacketComm))
            //    .SetProjection(Projections.RowCount())
            //    .FutureValue<int>().Value;

            ////get list results
            //criteria.SetMaxResults(maxRows)
            //  .SetFirstResult((pageIndex - 1) * maxRows)
            //  .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
            //  ;

            //IEnumerable<MPacketComm> list = criteria.List<MPacketComm>();
            //return list;
        }

        public IList<MPacketComm> GetByEmployeeId(string employeeId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select p
                                from MPacketComm as p");
            if (!string.IsNullOrEmpty(employeeId))
            {
                sql.AppendLine(@" where p.EmployeeId.Id = :employeeId");
            }
            IQuery q = Session.CreateQuery(sql.ToString());
            if (!string.IsNullOrEmpty(employeeId))
            {
                q.SetString("employeeId", employeeId);
            }

            return q.List<MPacketComm>();
        }

        public MPacketComm GetByEmployeeAndPacket(MEmployee emp, MPacket packet)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select p
                                from MPacketComm as p");
            sql.AppendLine(@" where p.EmployeeId = :emp");
            sql.AppendLine(@"   and p.PacketId = :packet");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetEntity("emp", emp);
            q.SetEntity("packet", packet);

            return q.UniqueResult<MPacketComm>();
        }

        #endregion
    }
}
