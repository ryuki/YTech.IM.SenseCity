using FluentNHibernate.Automapping;
using YTech.IM.SenseCity.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.SenseCity.Core.Transaction;

namespace YTech.IM.SenseCity.Data.NHibernateMaps.Transaction
{
    public class TTransRoomMap : IAutoMappingOverride<TTransRoom>
    {
        #region Implementation of IAutoMappingOverride<TTransRoom>

        public void Override(AutoMapping<TTransRoom> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_TRANS_ROOM");
            mapping.Id(x => x.Id, "TRANS_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.TransId, "TRANS_ID").LazyLoad();
            mapping.References(x => x.RoomId, "ROOM_ID").LazyLoad();
            mapping.Map(x => x.RoomBookDate, "ROOM_BOOK_DATE");
            mapping.Map(x => x.RoomInDate, "ROOM_IN_DATE");
            mapping.Map(x => x.RoomOutDate, "ROOM_OUT_DATE");
            mapping.Map(x => x.RoomStatus, "ROOM_STATUS");
            mapping.Map(x => x.RoomCashPaid, "ROOM_CASH_PAID");
            mapping.Map(x => x.RoomCreditPaid, "ROOM_CREDIT_PAID");
            mapping.Map(x => x.RoomVoucherPaid, "ROOM_VOUCHER_PAID");
            mapping.Map(x => x.RoomCommissionProduct, "ROOM_COMMISSION_PRODUCT");
            mapping.Map(x => x.RoomCommissionService, "ROOM_COMMISSION_SERVICE");
            mapping.Map(x => x.RoomDesc, "ROOM_DESC");

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
