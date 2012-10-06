﻿using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Reservation;
using YTech.IM.SenseCity.Enums;

namespace YTech.IM.SenseCity.Data.Repository
{
    public class TReservationDetailRepository : NHibernateRepositoryWithTypedId<TReservationDetail, string>, ITReservationDetailRepository
    {
    }
}
