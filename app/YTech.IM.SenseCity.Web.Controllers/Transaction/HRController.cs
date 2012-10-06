using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.HR;
using YTech.IM.SenseCity.Core.Transaction.Accounting;
using YTech.IM.SenseCity.Data.Repository;
using YTech.IM.SenseCity.Enums;
using YTech.IM.SenseCity.Web.Controllers.ViewModel;

namespace YTech.IM.SenseCity.Web.Controllers.Transaction
{
    [HandleError]
    public class HRController : Controller
    {
        private readonly ITAbsentRepository _tAbsentRepository;
        private readonly IMEmployeeRepository _mEmployeeRepository;

        public HRController(ITAbsentRepository tAbsentRepository, IMEmployeeRepository mEmployeeRepository)
        {
            Check.Require(tAbsentRepository != null, "tAbsentRepository may not be null");
            Check.Require(mEmployeeRepository != null, "mEmployeeRepository may not be null");

            this._tAbsentRepository = tAbsentRepository;
            this._mEmployeeRepository = mEmployeeRepository;
        }

        public ActionResult Absent()
        {
            ViewData["CurrentItem"] = "Absensi Karyawan";
            return View();
        }

        public virtual ActionResult GetAbsentByDate(DateTime? workDate)
        {
            var items = _mEmployeeRepository.GetAll();

            var jsonData = new
            {
                rows = (
                    from item in items
                    select new
                    {
                        i = item.Id.ToString(),
                        cell = new string[] {
                           item.Id,
                           item.PersonId != null ? item.PersonId.PersonName:"",
                           _tAbsentRepository.GetAbsentByEmployeeId(item,workDate).Count() != 0 ? _tAbsentRepository.GetAbsentByEmployeeId(item,workDate)[0].Status.ToString() : "",
                           _tAbsentRepository.GetAbsentByEmployeeId(item,workDate).Count() != 0 ? GetTime(_tAbsentRepository.GetAbsentByEmployeeId(item,workDate)[0].StartTime.ToString()) : "",
                           _tAbsentRepository.GetAbsentByEmployeeId(item,workDate).Count() != 0 ? GetTime(_tAbsentRepository.GetAbsentByEmployeeId(item,workDate)[0].EndTime.ToString()) : "",
                           _tAbsentRepository.GetAbsentByEmployeeId(item,workDate).Count() != 0 ? _tAbsentRepository.GetAbsentByEmployeeId(item,workDate)[0].AbsentDesc.ToString() : ""
                        }
                    }).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //[ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult GetAbsent(DateTime? workDate, FormCollection formCollection)
        {
            DateTime? inputdate = DateTime.Parse(formCollection["txtWorkDate"]);

            return GetAbsentByDate(workDate);
        }

        [Transaction]
        public ActionResult SaveAbsent(DateTime? workDate, int rowNum, FormCollection formCollection)
        {
            IList<TAbsent> delAbsents = _tAbsentRepository.GetAbsent(workDate);
            for (int i = 0; i < delAbsents.Count; i++)
            {
                TAbsent tAbsentToDelete = _tAbsentRepository.Get(delAbsents[i].Id);

                if (tAbsentToDelete != null)
                {
                    _tAbsentRepository.Delete(tAbsentToDelete);
                }

                try
                {
                    _tAbsentRepository.DbContext.CommitChanges();
                }
                catch (Exception e)
                {

                    _tAbsentRepository.DbContext.RollbackTransaction();
                }
            }
            for (int i = 0; i < rowNum; i++)
            {
                TAbsent tAbsent = new TAbsent();
                if (workDate.HasValue)
                {
                    tAbsent.AbsentDate = workDate.Value;
                }
                tAbsent.SetAssignedIdTo(Guid.NewGuid().ToString());
                tAbsent.EmployeeId = _mEmployeeRepository.Get(formCollection["id" + i]);
                tAbsent.Status = formCollection["selectstatus" + i];
                tAbsent.StartTime = formCollection["starttime" + i] != "" ? DateTime.Parse(formCollection["starttime" + i]) : (DateTime?)null;
                tAbsent.EndTime = formCollection["endtime" + i] != "" ? DateTime.Parse(formCollection["endtime" + i]) : (DateTime?)null;
                tAbsent.AbsentDesc = formCollection["desc" + i];
                tAbsent.CreatedDate = DateTime.Now;
                tAbsent.CreatedBy = User.Identity.Name;
                tAbsent.DataStatus = EnumDataStatus.New.ToString();

                _tAbsentRepository.Save(tAbsent);

                try
                {
                    _tAbsentRepository.DbContext.CommitChanges();
                }
                catch (Exception e)
                {

                    _tAbsentRepository.DbContext.RollbackTransaction();

                    return Content(e.GetBaseException().Message);
                }
            }


            return RedirectToAction("Absent");//Content("success");
        }

        public string GetTime(string fulldatetime)
        {
            int startindex = fulldatetime.IndexOf(" ") + 1;
            int length = fulldatetime.Length - startindex;

            return fulldatetime.Substring(startindex, length);
        }
    }
}
