using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.SenseCity.Core;

namespace YTech.IM.SenseCity.Core.Master
{
    public class MRoom : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        #region IHasAssignedId<string> Members

        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual string RoomName { get; set; }
        public virtual int RoomOrderNo { get; set; }
        public virtual string RoomType { get; set; }
        public virtual string RoomStatus { get; set; }
        public virtual string RoomDesc { get; set; }

        public virtual string DataStatus { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual byte[] RowVersion { get; set; }

        public virtual void SetAssignedIdTo(string assignedId)
        {
            Check.Require(!string.IsNullOrEmpty(assignedId), "Assigned Id may not be null or empty");
            Id = assignedId.Trim();
        }

        #endregion
    }
}
