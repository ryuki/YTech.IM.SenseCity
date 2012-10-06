using FluentNHibernate.Automapping;
using YTech.IM.SenseCity.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.IM.SenseCity.Data.NHibernateMaps.Master
{
    public class MPromoMap : IAutoMappingOverride<MPromo>
    {
        #region Implementation of IAutoMappingOverride<MPromo>

        public void Override(AutoMapping<MPromo> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadWrite();

            mapping.Table("dbo.M_PROMO");
            mapping.Id(x => x.Id, "PROMO_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.PromoName, "PROMO_NAME");
            mapping.Map(x => x.PromoStartDate, "PROMO_START_DATE");
            mapping.Map(x => x.PromoEndDate, "PROMO_END_DATE");
            mapping.Map(x => x.PromoValue, "PROMO_VALUE");
            mapping.Map(x => x.PromoStatus, "PROMO_STATUS");
            mapping.Map(x => x.PromoDesc, "PROMO_DESC");


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
