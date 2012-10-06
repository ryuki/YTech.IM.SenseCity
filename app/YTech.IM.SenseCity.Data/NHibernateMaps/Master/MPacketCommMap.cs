using FluentNHibernate.Automapping;
using YTech.IM.SenseCity.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.IM.SenseCity.Data.NHibernateMaps.Master
{
    public class MPacketCommMap : IAutoMappingOverride<MPacketComm>
    {
        #region Implementation of IAutoMappingOverride<MPacketComm>

        public void Override(AutoMapping<MPacketComm> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            //mapping.Cache.ReadOnly();

            mapping.Table("dbo.M_PACKET_COMM");
            mapping.Id(x => x.Id, "PACKET_COMM_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.PacketId, "PACKET_ID");
            mapping.References(x => x.EmployeeId, "EMPLOYEE_ID");
            mapping.Map(x => x.PacketCommType, "PACKET_COMM_TYPE");
            mapping.Map(x => x.PacketCommVal, "PACKET_COMM_VAL");
            mapping.Map(x => x.PacketCommStatus, "PACKET_COMM_STATUS");
            mapping.Map(x => x.PacketCommDesc, "PACKET_COMM_DESC");

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
