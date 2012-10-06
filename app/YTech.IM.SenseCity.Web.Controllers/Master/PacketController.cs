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
    public class PacketController : Controller
    {
        private readonly IMPacketRepository _mPacketRepository;
        public PacketController(IMPacketRepository mPacketRepository)
        {
            Check.Require(mPacketRepository != null, "mPacketRepository may not be null");

            this._mPacketRepository = mPacketRepository;
        }


        public ActionResult Search()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var packets = _mPacketRepository.GetPagedPacketList(sidx, sord, page, rows, ref totalRecords);
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
                            string.Empty,
                            packet.Id, 
                            packet.PacketName, 
                         packet.PacketPrice.HasValue ?  packet.PacketPrice.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                          packet.PacketPriceVip.HasValue ?  packet.PacketPriceVip.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                           packet.PacketStatus,
                           packet.PacketDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(MPacket viewModel, FormCollection formCollection)
        {
            UpdateNumericData(viewModel, formCollection);

            MPacket mPacketToInsert = new MPacket();
            TransferFormValuesTo(mPacketToInsert, viewModel);

            mPacketToInsert.SetAssignedIdTo(viewModel.Id);
            mPacketToInsert.CreatedDate = DateTime.Now;
            mPacketToInsert.CreatedBy = User.Identity.Name;
            mPacketToInsert.DataStatus = EnumDataStatus.New.ToString();
            
            _mPacketRepository.Save(mPacketToInsert);

            try
            {
                _mPacketRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mPacketRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Delete(MItem viewModel, FormCollection formCollection)
        {
            MPacket mPacketToDelete = _mPacketRepository.Get(viewModel.Id); 
            if (mPacketToDelete != null)
            {
                _mPacketRepository.Delete(mPacketToDelete);
            }

            try
            {
                _mPacketRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mPacketRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MPacket viewModel, FormCollection formCollection)
        {
            UpdateNumericData(viewModel, formCollection);
            MPacket mPacketToUpdate = _mPacketRepository.Get(viewModel.Id);
            TransferFormValuesTo(mPacketToUpdate, viewModel);
            mPacketToUpdate.ModifiedDate = DateTime.Now;
            mPacketToUpdate.ModifiedBy = User.Identity.Name;
            mPacketToUpdate.DataStatus = EnumDataStatus.Updated.ToString();
            
            try
            {
                _mPacketRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mPacketRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private static void UpdateNumericData(MPacket viewModel, FormCollection formCollection)
        {
            if (!string.IsNullOrEmpty(formCollection["PacketPrice"]))
            {
                string PacketPrice = formCollection["PacketPrice"].Replace(",", "");
                viewModel.PacketPrice = Convert.ToDecimal(PacketPrice);
            }
            else
            {
                viewModel.PacketPrice = null;
            }
            if (!string.IsNullOrEmpty(formCollection["PacketPriceVip"]))
            {
                string PacketPriceVip = formCollection["PacketPriceVip"].Replace(",", "");
                viewModel.PacketPriceVip = Convert.ToDecimal(PacketPriceVip);
            }
            else
            {
                viewModel.PacketPriceVip = null;
            }
        }

        private void TransferFormValuesTo(MPacket mPacketToUpdate, MPacket mPacketFromForm)
        {
            mPacketToUpdate.PacketName = mPacketFromForm.PacketName;
            mPacketToUpdate.PacketPrice = mPacketFromForm.PacketPrice;
            mPacketToUpdate.PacketPriceVip = mPacketFromForm.PacketPriceVip;
            mPacketToUpdate.PacketStatus = mPacketFromForm.PacketStatus;
            mPacketToUpdate.PacketDesc = mPacketFromForm.PacketDesc;
        }

        [Transaction]
        public virtual ActionResult GetList()
        {
            var packets = _mPacketRepository.GetAll();
            StringBuilder sb = new StringBuilder();
            MPacket mPacket = new MPacket();
            sb.AppendFormat("{0}:{1};", string.Empty, "-Pilih Paket-");
            for (int i = 0; i < packets.Count; i++)
            {
                mPacket = packets[i];
                sb.AppendFormat("{0}:{1}", mPacket.Id, mPacket.PacketName);
                if (i < packets.Count - 1)
                    sb.Append(";");
            }
            return Content(sb.ToString());
        }

        [Transaction]
        public virtual ActionResult Get(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                MPacket mPacket = _mPacketRepository.Get(id);
            }
            return Content("0");
        }
    }
}
