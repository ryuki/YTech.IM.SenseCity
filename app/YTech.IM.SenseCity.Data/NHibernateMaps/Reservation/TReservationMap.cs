using FluentNHibernate.Automapping;
using YTech.IM.SenseCity.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.HR;
using YTech.IM.SenseCity.Core.Transaction.Reservation;

namespace YTech.IM.SenseCity.Data.NHibernateMaps.Reservation
{
    public class TReservationMap : IAutoMappingOverride<TReservation>
    {
        #region IAutoMappingOverride<TReservation> Members

        public void Override(AutoMapping<TReservation> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_RESERVATION");
            mapping.Id(x => x.Id, "RESERVATION_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.CustomerId, "CUSTOMER_ID");
            mapping.Map(x => x.ReservationIsMember, "RESERVATION_IS_MEMBER");
            mapping.Map(x => x.ReservationName, "RESERVATION_NAME");
            mapping.Map(x => x.ReservationPhoneNo, "RESERVATION_PHONE_NO");
            mapping.Map(x => x.ReservationDate, "RESERVATION_DATE");
            mapping.Map(x => x.ReservationAppoinmentTime, "RESERVATION_APPOINMENT_TIME");
            mapping.Map(x => x.ReservationNoOfPeople, "RESERVATION_NO_OF_PEOPLE");
            mapping.Map(x => x.ReservationStatus, "RESERVATION_STATUS");
            mapping.Map(x => x.ReservationDesc, "RESERVATION_DESC");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Map(x => x.RowVersion, "ROW_VERSION").ReadOnly();

            mapping.HasMany(x => x.ReservationDetails)
                //.Access.Property()
                .AsBag()
                .Inverse()
                .KeyColumn("RESERVATION_ID")
                .Cascade.All();
        }

        #endregion
    }
}
