using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Enums;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class TTransDetRepository : NHibernateRepositoryWithTypedId<TTransDet, string>, ITTransDetRepository
    {
        public IList<TTransDet> GetByItemWarehouse(MItem item, MWarehouse warehouse)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select det
                                from TTransDet as det
                                    left outer join det.TransId trans
                                    where trans.TransStatus = :TransStatus ");
            if (item != null)
            {
                sql.AppendLine(@"   and det.ItemId = :item");
            }
            if (warehouse != null)
            {
                sql.AppendLine(@"   and trans.WarehouseId = :warehouse");
            }
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("TransStatus", Enums.EnumTransactionStatus.Budgeting.ToString());
            if (item != null)
            {
                q.SetEntity("item", item);
            }
            if (warehouse != null)
            {
                q.SetEntity("warehouse", warehouse);
            }
            return q.List<TTransDet>();
        }

        public decimal? GetTotalUsed(MItem item, MWarehouse warehouse)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select sum(det.TransDetQty)
                                from TTransDet as det
                                    left outer join det.TransId trans
                                    where trans.TransStatus = :TransStatus ");
            if (item != null)
            {
                sql.AppendLine(@"   and det.ItemId = :item");
            }
            if (warehouse != null)
            {
                sql.AppendLine(@"   and trans.WarehouseId = :warehouse");
            }

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("TransStatus", Enums.EnumTransactionStatus.Using.ToString());
            if (item != null)
            {
                q.SetEntity("item", item);
            }
            if (warehouse != null)
            {
                q.SetEntity("warehouse", warehouse);
            }
            if (q.UniqueResult() != null)
            {
                 return (decimal)q.UniqueResult();
            }
            return null;
        }

        public IList<TTransDet> GetListByRoom(MRoom room)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select det
                                from TTransDet as det
                                    left outer join det.TransId trans, TTransRoom transRoom
                                        where trans.Id = transRoom.Id
                                    and trans.TransStatus = :TransStatus ");
            if (room != null)
            {
                sql.AppendLine(@"   and transRoom.RoomId = :room");
            }
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("TransStatus", Enums.EnumTransactionStatus.Service.ToString());
            if (room != null)
            {
                q.SetEntity("room", room);
            } 
            return q.List<TTransDet>();
        }

        public IList<TTransDet> GetListByTransId(string transId, Enums.EnumTransactionStatus enumTransactionStatus)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select det
                                from TTransDet as det
                                    left outer join det.TransId trans 
                                        where trans.TransStatus = :TransStatus ");
            if (!string.IsNullOrEmpty(transId))
            {
                sql.AppendLine(@"   and trans.Id = :transId");
            }
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("TransStatus", enumTransactionStatus.ToString());
            if (!string.IsNullOrEmpty(transId))
            {
                q.SetString("transId", transId);
            }
            return q.List<TTransDet>();
        }

        public IList<TTransDet> GetListByTrans(TTrans trans)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TTransDet));
            if (trans != null)
            {
                criteria.Add(Expression.Eq("TransId", trans));
            }

            IList<TTransDet> list = criteria.List<TTransDet>();
            return list;
        }

        public IList<TTransDet> GetListByDate(EnumTransactionStatus TransStatus, DateTime? dateFrom, DateTime? dateTo)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select det
                                from TTransDet as det
                                    left outer join det.TransId trans 
                                        where trans.TransStatus = :TransStatus ");
            sql.AppendLine(@"   and trans.TransDate between :dateFrom and :dateTo ");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("TransStatus", TransStatus.ToString());
            q.SetDateTime("dateFrom", dateFrom.Value);
            q.SetDateTime("dateTo", dateTo.Value);
            return q.List<TTransDet>();
        }
    }
}
