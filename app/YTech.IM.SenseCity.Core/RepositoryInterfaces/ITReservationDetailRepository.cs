using System;
using System.Collections;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Inventory;
using YTech.IM.SenseCity.Core.Transaction.Reservation;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface ITReservationDetailRepository : INHibernateRepositoryWithTypedId<TReservationDetail, string>
    {
    }
}
