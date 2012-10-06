using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.SenseCity.Core;

namespace YTech.IM.SenseCity.Core.Master
{
    public class MPacket : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        public MPacket()
        {
            InitMembers();
        }

        /// <summary>
        /// Since we want to leverage automatic properties, init appropriate members here.
        /// </summary>
        private void InitMembers()
        {
            PacketItemCats = new List<MPacketItemCat>();
        }

        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual string PacketName { get; set; }
        public virtual decimal? PacketPrice { get; set; }
        public virtual decimal? PacketPriceVip { get; set; }
        public virtual string PacketStatus { get; set; }
        public virtual string PacketDesc { get; set; }

        public virtual string DataStatus { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual byte[] RowVersion { get; set; }

        public virtual IList<MPacketItemCat> PacketItemCats { get; protected set; }

        #region Implementation of IHasAssignedId<string>

        public virtual void SetAssignedIdTo(string assignedId)
        {
            Check.Require(!string.IsNullOrEmpty(assignedId), "Assigned Id may not be null or empty");
            Id = assignedId.Trim();
        }

        #endregion
    }
}
