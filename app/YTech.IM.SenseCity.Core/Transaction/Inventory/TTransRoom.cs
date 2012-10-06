using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.SenseCity.Core.Master;

namespace YTech.IM.SenseCity.Core.Transaction
{
    public class TTransRoom : EntityWithTypedId<string>, IHasAssignedId<string>
    {  
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual TTrans TransId { get; set; }
        public virtual MRoom RoomId { get; set; }
        public virtual DateTime? RoomBookDate { get; set; }
        public virtual DateTime? RoomInDate { get; set; }
        public virtual DateTime? RoomOutDate { get; set; }
        public virtual string RoomStatus { get; set; }
        public virtual decimal? RoomCashPaid { get; set; }
        public virtual decimal? RoomCreditPaid { get; set; }
        public virtual decimal? RoomVoucherPaid { get; set; }
        public virtual decimal? RoomCommissionProduct { get; set; }
        public virtual decimal? RoomCommissionService { get; set; }
        public virtual string RoomDesc { get; set; }

        public virtual string DataStatus { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual byte[] RowVersion { get; set; }

        #region Implementation of IHasAssignedId<string>

        public virtual void SetAssignedIdTo(string assignedId)
        {
            Check.Require(!string.IsNullOrEmpty(assignedId), "Assigned Id may not be null or empty");
            Id = assignedId.Trim();
        }

        #endregion
    }
}
