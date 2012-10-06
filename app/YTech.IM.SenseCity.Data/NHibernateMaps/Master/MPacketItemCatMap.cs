using FluentNHibernate.Automapping;
using YTech.IM.SenseCity.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.IM.SenseCity.Data.NHibernateMaps.Master
{
    public class MPacketItemCatMap : IAutoMappingOverride<MPacketItemCat>
    {
        #region Implementation of IAutoMappingOverride<MPacketItemCat>

        public void Override(AutoMapping<MPacketItemCat> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadWrite();

            mapping.Table("dbo.M_PACKET_ITEM_CAT");
            mapping.Id(x => x.Id, "PACKET_ITEM_CAT_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.PacketId, "PACKET_ID");
            mapping.References(x => x.ItemCatId, "ITEM_CAT_ID").Fetch.Join();
            mapping.Map(x => x.ItemCatQty, "ITEM_CAT_QTY");
            mapping.Map(x => x.PacketItemCatStatus, "PACKET_ITEM_CAT_STATUS");
            mapping.Map(x => x.PacketItemCatDesc, "PACKET_ITEM_CAT_DESC");
           
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
