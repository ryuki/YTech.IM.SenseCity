using FluentNHibernate.Automapping;
using YTech.IM.SenseCity.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Accounting;
using YTech.IM.SenseCity.Core.Transaction.Inventory;

namespace YTech.IM.SenseCity.Data.NHibernateMaps.Transaction
{
    public class TShiftMap : IAutoMappingOverride<TShift>
    {
        #region Implementation of IAutoMappingOverride<TShift>

        public void Override(AutoMapping<TShift> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_SHIFT");
            mapping.Id(x => x.Id, "SHIFT_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.EmployeeId, "EMPLOYEE_ID");
            mapping.Map(x => x.ShiftDate, "SHIFT_DATE");
            mapping.Map(x => x.ShiftNo, "SHIFT_NO");
            mapping.Map(x => x.ShiftDateFrom, "SHIFT_DATE_FROM");
            mapping.Map(x => x.ShiftDateTo, "SHIFT_DATE_TO");
            mapping.Map(x => x.ShiftStatus, "SHIFT_STATUS");
            mapping.Map(x => x.ShiftDesc, "SHIFT_DESC");

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
