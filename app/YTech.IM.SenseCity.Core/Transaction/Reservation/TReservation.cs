using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.SenseCity.Core.Master;

namespace YTech.IM.SenseCity.Core.Transaction.Reservation
{
    public class TReservation : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual bool? ReservationIsMember { get; set; }
        public virtual MCustomer CustomerId { get; set; }
        public virtual string ReservationName { get; set; }
        public virtual string ReservationPhoneNo { get; set; }
        public virtual DateTime? ReservationDate { get; set; }
        public virtual DateTime? ReservationAppoinmentTime { get; set; }
        public virtual int? ReservationNoOfPeople { get; set; }
        public virtual string ReservationStatus { get; set; }
        public virtual string ReservationDesc { get; set; }

        public virtual string DataStatus { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual byte[] RowVersion { get; set; }

        public virtual IList<TReservationDetail> ReservationDetails { get; protected set; }

        #region Implementation of IHasAssignedId<string>

        public virtual void SetAssignedIdTo(string assignedId)
        {
            Check.Require(!string.IsNullOrEmpty(assignedId), "Assigned Id may not be null or empty");
            Id = assignedId.Trim();
        }

        #endregion
    }
}
