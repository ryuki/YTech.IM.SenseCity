using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Accounting;
using YTech.IM.SenseCity.Core.Transaction.Inventory;
using YTech.IM.SenseCity.Data.Repository;
using YTech.IM.SenseCity.Enums;
using YTech.IM.SenseCity.Web.Controllers.ViewModel;

namespace YTech.IM.SenseCity.Web.Controllers.Utility
{
    [HandleError]
    public class ShiftController : Controller
    {
        private readonly ITShiftRepository _tShiftRepository;
        public ShiftController(ITShiftRepository tShiftRepository)
        {
            Check.Require(tShiftRepository != null, "tShiftRepository may not be null");

            this._tShiftRepository = tShiftRepository;
        }

        [Transaction]
        public ActionResult Closing()
        {
            ShiftFormViewModel viewModel = ShiftFormViewModel.Create(_tShiftRepository,DateTime.Today);

            ViewData["CurrentItem"] = "Tutup Shift";
            return View(viewModel);
        }

        [Transaction]
        public ActionResult GetJSONLastClosing(DateTime? closingDate)
        {
            ShiftFormViewModel viewModel = ShiftFormViewModel.Create(_tShiftRepository, closingDate);

            return Json(viewModel,JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Closing(TShift viewModel, FormCollection formCollection)
        {
            _tShiftRepository.DbContext.BeginTransaction();

            TShift s = new TShift();
            s.SetAssignedIdTo(Guid.NewGuid().ToString());
            s.ShiftDate = viewModel.ShiftDate;
            s.ShiftDateFrom = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy} {1:HH:mm}", s.ShiftDate.Value, viewModel.ShiftDateFrom.Value));
            s.ShiftDateTo = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy} {1:HH:mm}", s.ShiftDate.Value, viewModel.ShiftDateTo.Value));
            s.ShiftNo = viewModel.ShiftNo;

            s.CreatedBy = User.Identity.Name;
            s.CreatedDate = DateTime.Now;
            s.DataStatus = EnumDataStatus.New.ToString();

            _tShiftRepository.Save(s);
            bool Success;
            string Message;
            try
            {
                _tShiftRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
                Success = true;
                Message = "Data berhasil disimpan";
            }
            catch (Exception ex)
            {
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
                Success = false;
                Message = ex.Message;
            }
            var e = new { Success, Message };
            return Json(e, JsonRequestBehavior.AllowGet);
        }
    }
}
