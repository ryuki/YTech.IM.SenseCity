﻿using FluentNHibernate.Automapping;
using YTech.IM.SenseCity.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.SenseCity.Core.Transaction;

namespace YTech.IM.SenseCity.Data.NHibernateMaps.Transaction
{
    public class TReferenceMap : IAutoMappingOverride<TReference>
    {
        #region Implementation of IAutoMappingOverride<TReference>

        public void Override(AutoMapping<TReference> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_REFERENCE");
            mapping.Id(x => x.Id, "REFERENCE_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.ReferenceType, "REFERENCE_TYPE");
            mapping.Map(x => x.ReferenceValue, "REFERENCE_VALUE");
            mapping.Map(x => x.ReferenceDesc, "REFERENCE_DESC");
            mapping.Map(x => x.ReferenceStatus, "REFERENCE_STATUS");

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
