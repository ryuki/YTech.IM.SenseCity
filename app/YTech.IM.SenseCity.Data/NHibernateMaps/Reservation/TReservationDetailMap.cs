using FluentNHibernate.Automapping;
using YTech.IM.SenseCity.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.HR;
using YTech.IM.SenseCity.Core.Transaction.Reservation;

namespace YTech.IM.SenseCity.Data.NHibernateMaps.Reservation
{
    public class TReservationDetailMap : IAutoMappingOverride<TReservationDetail>
    {
        #region IAutoMappingOverride<TReservationDetail> Members

        public void Override(AutoMapping<TReservationDetail> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_RESERVATION_DETAIL");
            mapping.Id(x => x.Id, "RESERVATION_DETAIL_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.ReservationId, "RESERVATION_ID").Not.Nullable();
            mapping.Map(x => x.ReservationDetailName, "RESERVATION_DETAIL_NAME");
            mapping.References(x => x.PacketId, "PACKET_ID");
            mapping.References(x => x.EmployeeId, "EMPLOYEE_ID");
            mapping.Map(x => x.ReservationDetailStatus, "RESERVATION_DETAIL_STATUS");
            mapping.Map(x => x.ReservationDetailDesc, "RESERVATION_DETAIL_DESC");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Map(x => x.RowVersion, "ROW_VERSION").ReadOnly();
        }

        #endregion
    }
}
