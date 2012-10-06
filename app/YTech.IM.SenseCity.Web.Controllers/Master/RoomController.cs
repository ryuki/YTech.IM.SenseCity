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
using YTech.IM.SenseCity.Enums;

namespace YTech.IM.SenseCity.Web.Controllers.Master
{
    [HandleError]
    public class RoomController : Controller
    {
        private readonly IMRoomRepository _mRoomRepository;
        public RoomController(IMRoomRepository mRoomRepository)
        {
            Check.Require(mRoomRepository != null, "mRoomRepository may not be null");

            this._mRoomRepository = mRoomRepository;
        }


        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var packets = _mRoomRepository.GetPagedPacketList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from packet in packets
                    select new
                    {
                        i = packet.Id.ToString(),
                        cell = new string[] {
                            packet.Id, 
                            packet.RoomName, 
                           packet.RoomOrderNo.ToString(),
                           packet.RoomType,
                           packet.RoomStatus,
                           packet.RoomDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(MRoom viewModel, FormCollection formCollection)
        {

            MRoom mRoomToInsert = new MRoom();
            TransferFormValuesTo(mRoomToInsert, viewModel);

            mRoomToInsert.SetAssignedIdTo(viewModel.Id);
            mRoomToInsert.CreatedDate = DateTime.Now;
            mRoomToInsert.CreatedBy = User.Identity.Name;
            mRoomToInsert.DataStatus = EnumDataStatus.New.ToString();
            
            _mRoomRepository.Save(mRoomToInsert);

            try
            {
                _mRoomRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mRoomRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Delete(MRoom viewModel, FormCollection formCollection)
        {
            MRoom mPacketToDelete = _mRoomRepository.Get(viewModel.Id);

            if (mPacketToDelete != null)
            {
                _mRoomRepository.Delete(mPacketToDelete);
            }

            try
            {
                _mRoomRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mRoomRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MRoom viewModel, FormCollection formCollection)
        {
            MRoom mRoomToUpdate = _mRoomRepository.Get(viewModel.Id);
            TransferFormValuesTo(mRoomToUpdate, viewModel);
            mRoomToUpdate.ModifiedDate = DateTime.Now;
            mRoomToUpdate.ModifiedBy = User.Identity.Name;
            mRoomToUpdate.DataStatus = EnumDataStatus.Updated.ToString();
            
            try
            {
                _mRoomRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mRoomRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void TransferFormValuesTo(MRoom mRoomToUpdate, MRoom mRoomFromForm)
        {
            mRoomToUpdate.RoomName = mRoomFromForm.RoomName;
            mRoomToUpdate.RoomOrderNo = mRoomFromForm.RoomOrderNo;
            mRoomToUpdate.RoomType = mRoomFromForm.RoomType;
            mRoomToUpdate.RoomStatus = mRoomFromForm.RoomStatus;
            mRoomToUpdate.RoomDesc = mRoomFromForm.RoomDesc;
        }

        [Transaction]
        public virtual ActionResult GetList()
        {
            var rooms = _mRoomRepository.GetAll();
            StringBuilder sb = new StringBuilder();
            MRoom mRoom = new MRoom();
            sb.AppendFormat("{0}:{1};", string.Empty, "-Pilih Ruangan-");
            for (int i = 0; i < rooms.Count; i++)
            {
                mRoom = rooms[i];
                sb.AppendFormat("{0}:{1}", mRoom.Id, mRoom.RoomName);
                if (i < rooms.Count - 1)
                    sb.Append(";");
            }
            return Content(sb.ToString());
        }

        //[Transaction]
        //public virtual ActionResult GetRoomType()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendFormat("{0}:{1};", string.Empty, "-Pilih Tipe Ruangan-");
        //    sb.AppendFormat("{0}:{1}", EnumRoomType.SpaMan.ToString(), EnumRoomType.SpaMan.ToString());
        //    sb.Append(";");
        //    sb.AppendFormat("{0}:{1}", EnumRoomType.SpaWomen.ToString(), EnumRoomType.SpaWomen.ToString());
            
        //    return Content(sb.ToString());
        //}

        [Transaction]
        public virtual ActionResult Get(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                MRoom mRoom = _mRoomRepository.Get(id);
            }
            return Content("0");
        }

        public virtual ActionResult GetRoomTypeList()
        {
            return Content(Helper.CommonHelper.GetEnumListForGrid<EnumRoomType>("-Pilih Tipe Kamar-"));
        }

        public virtual ActionResult GetRoomStatusList()
        {
            return Content(Helper.CommonHelper.GetEnumListForGrid<EnumRoomStatus>("-Pilih Status Kamar-"));
        }
    }
}
