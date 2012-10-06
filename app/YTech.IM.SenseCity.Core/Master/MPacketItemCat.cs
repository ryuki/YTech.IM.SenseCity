using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.SenseCity.Core;

namespace YTech.IM.SenseCity.Core.Master
{
    public class MPacketItemCat : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        public MPacketItemCat() { }

        public MPacketItemCat(MPacket packet)
        {
            Check.Require(packet != null, "packet may not be null");

            PacketId = packet;
        }
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual MPacket PacketId { get; set; }
        public virtual MItemCat ItemCatId { get; set; }
        public virtual decimal? ItemCatQty { get; set; }
        public virtual string PacketItemCatStatus { get; set; }
        public virtual string PacketItemCatDesc { get; set; }

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
