using FluentNHibernate.Automapping;
using YTech.IM.SenseCity.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.IM.SenseCity.Data.NHibernateMaps.Master
{
    public class MPacketMap : IAutoMappingOverride<MPacket>
    {
        #region Implementation of IAutoMappingOverride<MPacket>

        public void Override(AutoMapping<MPacket> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            //mapping.Cache.ReadOnly();

            mapping.Table("dbo.M_PACKET");
            mapping.Id(x => x.Id, "PACKET_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.PacketName, "PACKET_NAME");
            mapping.Map(x => x.PacketPrice, "PACKET_PRICE");
            mapping.Map(x => x.PacketPriceVip, "PACKET_PRICE_VIP");
            mapping.Map(x => x.PacketStatus, "PACKET_STATUS");
            mapping.Map(x => x.PacketDesc, "PACKET_DESC");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Map(x => x.RowVersion, "ROW_VERSION").ReadOnly();

            mapping.HasMany(x => x.PacketItemCats)
                //.Access.Property()
                .AsBag()
                .Inverse()
                .KeyColumn("PACKET_ID")
                .Cascade.All();

        }

        #endregion
    }
}
