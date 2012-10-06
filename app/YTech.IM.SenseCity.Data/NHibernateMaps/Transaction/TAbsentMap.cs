using FluentNHibernate.Automapping;
using YTech.IM.SenseCity.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.HR;


namespace YTech.IM.SenseCity.Data.NHibernateMaps.Transaction
{
    public class TAbsentMap : IAutoMappingOverride<TAbsent>
    {
        #region IAutoMappingOverride<TAbsent> Members

        public void Override(AutoMapping<TAbsent> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_ABSENT");
            mapping.Id(x => x.Id, "ABSENT_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.EmployeeId, "EMPLOYEE_ID").Not.Nullable();
            mapping.Map(x => x.AbsentDate, "ABSENT_DATE");
            mapping.Map(x => x.StartTime, "START_TIME");
            mapping.Map(x => x.EndTime, "END_TIME");
            mapping.Map(x => x.Status, "STATUS");
            mapping.Map(x => x.AbsentDesc, "ABSENT_DESC");

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
