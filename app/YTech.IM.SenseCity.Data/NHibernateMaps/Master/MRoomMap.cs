using FluentNHibernate.Automapping;
using YTech.IM.SenseCity.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.IM.SenseCity.Data.NHibernateMaps.Master
{
    public class MRoomMap : IAutoMappingOverride<MRoom>
    {

        #region IAutoMappingOverride<MRoom> Members

        public void Override(AutoMapping<MRoom> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.M_ROOM");
            mapping.Id(x => x.Id, "ROOM_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.RoomName, "ROOM_NAME");
            mapping.Map(x => x.RoomOrderNo, "ROOM_ORDER_NO");
            mapping.Map(x => x.RoomType, "ROOM_TYPE");
            mapping.Map(x => x.RoomStatus, "ROOM_STATUS");
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
