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
using YTech.IM.SenseCity.Enums;

namespace YTech.IM.SenseCity.Web.Controllers.ViewModel
{
    public class ShiftFormViewModel
    {
        public static ShiftFormViewModel Create(ITShiftRepository tShiftRepository, DateTime? closingDate)
        {
            ShiftFormViewModel viewModel = new ShiftFormViewModel();
            TShift s = new TShift();
            s.ShiftDate = closingDate;
           
            s.ShiftDateTo = DateTime.Now;

            //get lats shift
            TShift lastShift = tShiftRepository.GetLastShiftByDate(s.ShiftDate);
            if (lastShift != null)
            {
                s.ShiftNo = lastShift.ShiftNo + 1;
                s.ShiftDateFrom = lastShift.ShiftDateTo;
            }
            else
            {
                s.ShiftNo = 1;
                s.ShiftDateFrom = DateTime.Today;
            }
            viewModel.Shift = s;
            return viewModel;
        }

        public TShift Shift { get; internal set; }
    }
}
