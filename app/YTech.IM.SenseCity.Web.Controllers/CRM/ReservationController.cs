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
using YTech.IM.SenseCity.Core.Transaction.Reservation;
using YTech.IM.SenseCity.Data.Repository;
using YTech.IM.SenseCity.Enums;
using YTech.IM.SenseCity.Web.Controllers.ViewModel;

namespace YTech.IM.SenseCity.Web.Controllers.CRM
{
    [HandleError]
    public class ReservationController : Controller
    {
        private readonly ITReservationRepository _reservationRepository;
        private readonly ITReservationDetailRepository _reservationDetailRepository;
        private readonly IMCustomerRepository _mCustomerRepository;
        private readonly IMEmployeeRepository _mEmployeeRepository;
        private readonly IMPacketRepository _mPacketRepository;

        public ReservationController(ITReservationRepository reservationRepository, ITReservationDetailRepository reservationDetailRepository, IMCustomerRepository mCustomerRepository, IMEmployeeRepository mEmployeeRepository, IMPacketRepository mPacketRepository)
        {
            Check.Require(reservationRepository != null, "reservationRepository may not be null");
            Check.Require(reservationDetailRepository != null, "reservationDetailRepository may not be null");
            Check.Require(mCustomerRepository != null, "mCustomerRepository may not be null");
            Check.Require(mEmployeeRepository != null, "mEmployeeRepository may not be null");
            Check.Require(mPacketRepository != null, "mPacketRepository may not be null");

            this._reservationRepository = reservationRepository;
            this._reservationDetailRepository = reservationDetailRepository;
            this._mCustomerRepository = mCustomerRepository;
            this._mEmployeeRepository = mEmployeeRepository;
            this._mPacketRepository = mPacketRepository;
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string reservationStatus)
        {
            int totalRecords = 0;
            var reservationList = _reservationRepository.GetPagedReservationList(sidx, sord, page, rows, ref totalRecords, reservationStatus);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from res in reservationList
                    select new
                    {
                        i = res.Id.ToString(),
                        cell = new string[] {
                            res.Id, 
                            res.Id, 
                            res.ReservationName, 
                            res.ReservationPhoneNo,
                            res.ReservationDate.HasValue ? res.ReservationDate.Value.ToString(Helper.CommonHelper.DateFormat) : null,
                            res.ReservationAppoinmentTime.HasValue ? res.ReservationAppoinmentTime.Value.ToString(Helper.CommonHelper.TimeFormat) : null,
                         res.ReservationNoOfPeople.HasValue ?    res.ReservationNoOfPeople.Value.ToString() : null,
                         res.ReservationStatus
                        }
                    }).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult Index()
        {
            var values = from EnumReservationStatus e in Enum.GetValues(typeof(EnumReservationStatus))
                         select new { ID = e, Name = e.ToString() };

            ViewData["ReservationStatusList"] = new SelectList(values, "Id", "Name");
            return View();
        }

        public virtual ActionResult AddNew()
        {
            return View(ReservationFormViewModel.Create());
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult AddNew(TReservation reservation, FormCollection formCollection)
        {
            string Message = "Data reservasi berhasil disimpan";
            bool Success = true;

            _reservationRepository.DbContext.BeginTransaction();
            reservation.SetAssignedIdTo(Guid.NewGuid().ToString());
            if (!string.IsNullOrEmpty(formCollection["CustomerId"]))
            {
                reservation.CustomerId = _mCustomerRepository.Get(formCollection["CustomerId"]);
                reservation.ReservationIsMember = true;
            }
            else
            {
                reservation.ReservationIsMember = false;
            }
            reservation.ReservationStatus = EnumReservationStatus.Baru.ToString();
            reservation.DataStatus = EnumDataStatus.New.ToString();
            reservation.CreatedBy = User.Identity.Name;
            reservation.CreatedDate = DateTime.Now;
            _reservationRepository.Save(reservation);

            TReservationDetail detail;

            //loop ReservationNoOfPeople
            MPacket packet;
            MEmployee employee;
            for (int i = 0; i < reservation.ReservationNoOfPeople; i++)
            {
                detail = new TReservationDetail(reservation);
                detail.SetAssignedIdTo(Guid.NewGuid().ToString());
                detail.ReservationDetailName = formCollection["txtDetailName_" + i.ToString()];
                if (!string.IsNullOrEmpty(formCollection["txtPacketId_" + i.ToString()]))
                    detail.PacketId = _mPacketRepository.Get(formCollection["txtPacketId_" + i.ToString()]);
                if (!string.IsNullOrEmpty(formCollection["txtEmployeeId_" + i.ToString()]))
                    detail.EmployeeId = _mEmployeeRepository.Get(formCollection["txtEmployeeId_" + i.ToString()]);
                detail.DataStatus = EnumDataStatus.New.ToString();
                detail.CreatedBy = User.Identity.Name;
                detail.CreatedDate = DateTime.Now;
                _reservationDetailRepository.Save(detail);
            }

            try
            {
                _reservationRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
            }
            catch (Exception ex)
            {
                Success = false;
                Message = ex.Message;
                _reservationRepository.DbContext.RollbackTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
            }

            var e = new
            {
                Success,
                Message
            };
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public virtual ActionResult Delete(TReservation viewModel, FormCollection formCollection)
        {
            _reservationRepository.DbContext.BeginTransaction();
            TReservation reservation = _reservationRepository.Get(viewModel.Id);

            if (reservation != null)
            {
                _reservationRepository.Delete(reservation);
            }

            try
            {
                _reservationRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _reservationRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data reservasi berhasil dihapus");
        }

        [Transaction]
        public virtual ActionResult ListForSubGrid(string id)
        {
            IList<TReservationDetail> details = new List<TReservationDetail>();

            TReservation reservation = _reservationRepository.Get(id);
            if (reservation != null)
            {
                details = reservation.ReservationDetails;
            }

            var jsonData = new
            {
                rows = (
                    from detail in details
                    select new
                    {
                        i = detail.Id.ToString(),
                        cell = new string[] {
                           detail.ReservationDetailName,
                            detail.PacketId != null ?  detail.PacketId.PacketName : null,
                           detail.EmployeeId != null? detail.EmployeeId.PersonId.PersonName : null
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        //[AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult UpdateStatus(string reservationId, string status)
        {
            _reservationRepository.DbContext.BeginTransaction();
            TReservation reservation = _reservationRepository.Get(reservationId);

            if (reservation != null)
            {
                reservation.ReservationStatus = status;
                reservation.DataStatus = EnumDataStatus.Updated.ToString();
                reservation.ModifiedBy = User.Identity.Name;
                reservation.ModifiedDate = DateTime.Now;
                _reservationRepository.Update(reservation);
            }

            try
            {
                _reservationRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _reservationRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data reservasi berhasil diupdate");
        }
    }
}
