using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Inventory;
using YTech.IM.SenseCity.Core.Transaction.Reservation;
using YTech.IM.SenseCity.Enums;

namespace YTech.IM.SenseCity.Web.Controllers.ViewModel
{
    public class ReservationFormViewModel
    {
        public static ReservationFormViewModel Create()
        {
            ReservationFormViewModel viewModel = new ReservationFormViewModel();
            TReservation reservation = new TReservation();
            reservation.ReservationDate = DateTime.Today;
            reservation.ReservationAppoinmentTime = DateTime.Now;
            reservation.ReservationIsMember = false;
            reservation.ReservationNoOfPeople = 1;

            viewModel.Reservation = reservation;
            return viewModel;
        }
        public TReservation Reservation { get; internal set; }
    }
}
