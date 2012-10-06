using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.SenseCity.Core.Master;

namespace YTech.IM.SenseCity.Core.Transaction.Reservation
{
    public class TReservationDetail : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        public TReservationDetail() { }

        public TReservationDetail(TReservation reservation)
        {
            Check.Require(reservation != null, "reservation may not be null");

            ReservationId = reservation;
        }

        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual TReservation ReservationId { get; protected set; }
        public virtual string ReservationDetailName { get; set; }
        public virtual MPacket PacketId { get; set; }
        public virtual MEmployee EmployeeId { get; set; }
        public virtual string ReservationDetailStatus { get; set; }
        public virtual string ReservationDetailDesc { get; set; }

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
