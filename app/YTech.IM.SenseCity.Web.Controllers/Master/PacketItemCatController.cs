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

    public class PacketItemCatController : Controller
    {
        private readonly IMPacketItemCatRepository _mPacketItemCatRepository;
        private readonly IMPacketRepository _mPacketRepository;
        private readonly IMItemCatRepository _mItemCatRepository;
        public PacketItemCatController(IMPacketItemCatRepository mPacketItemCatRepository, IMPacketRepository mPacketRepository
            , IMItemCatRepository mItemCatRepository)
        {
            Check.Require(mPacketItemCatRepository != null, "mPacketItemCatRepository may not be null");
            Check.Require(mPacketRepository != null, "mPacketRepository may not be null");
            Check.Require(mItemCatRepository != null, "mItemCatRepository may not be null");

            this._mPacketItemCatRepository = mPacketItemCatRepository;
            this._mPacketRepository = mPacketRepository;
            this._mItemCatRepository = mItemCatRepository;
        }


        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string id)
        {
            int totalRecords = 0;
            var itemCats = _mPacketItemCatRepository.GetPagedItemList(sidx, sord, page, rows, ref totalRecords, id);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from itemCat in itemCats
                    select new
                    {
                        i = itemCat.Id.ToString(),
                        cell = new string[] {
                            itemCat.Id, 
                            //itemCat.PacketId != null ? itemCat.PacketId.Id : null, 
                           itemCat.ItemCatId != null ? itemCat.ItemCatId.Id : null,
                           itemCat.ItemCatId != null ? itemCat.ItemCatId.ItemCatName : null,
                          itemCat.ItemCatQty.HasValue ?  itemCat.ItemCatQty.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                           itemCat.PacketItemCatStatus,
                           itemCat.PacketItemCatDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //[Transaction]
        //public virtual ActionResult List2(string id)
        //{
        //    int totalRecords = 0;
        //    //var itemCats = _mPacketItemCatRepository.GetByPacketId(packetId);

        //    var itemCats = _mPacketItemCatRepository.GetByPacketId(id);
        //    string sidx;
        //    string sord;
        //    int? page = 0;
        //    int? rows = 20;
        //    int? pageSize = rows;
        //    int? totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

        //    var jsonData = new
        //    {
        //        total = totalPages,
        //        page = page,
        //        records = totalRecords,
        //        rows = (
        //            from itemCat in itemCats
        //            select new
        //            {
        //                i = itemCat.Id.ToString(),
        //                cell = new string[] {
        //                   itemCat.Id, 
        //                   //itemCat.PacketId != null ? itemCat.PacketId.Id : null, 
        //                   itemCat.ItemCatId != null ? itemCat.ItemCatId.Id : null,
        //                   itemCat.ItemCatQty.ToString(),
        //                   itemCat.PacketItemCatStatus,
        //                   itemCat.PacketItemCatDesc
        //                }
        //            }).ToArray()
        //    };


        //    return Json(jsonData, JsonRequestBehavior.AllowGet);
        //}

        [Transaction]
        public virtual ActionResult ListForSubGrid(string id)
        {
            var itemCats = _mPacketItemCatRepository.GetByPacketId(id);

            var jsonData = new
            {
                rows = (
                    from itemCat in itemCats
                    select new
                    {
                        i = itemCat.Id.ToString(),
                        cell = new string[] {
                           //itemCat.Id, 
                           //itemCat.PacketId != null ? itemCat.PacketId.Id : null, 
                           itemCat.ItemCatId != null ? itemCat.ItemCatId.ItemCatName : null,
                            itemCat.ItemCatQty.HasValue ?  itemCat.ItemCatQty.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                           itemCat.PacketItemCatStatus,
                           itemCat.PacketItemCatDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public virtual ActionResult AddPacketItemCat()
        {   
            return View();
        }

        [Transaction]
        public ActionResult Insert(MPacketItemCat viewModel, FormCollection formCollection)
        {
            UpdateNumericData(viewModel, formCollection);
            MPacketItemCat mPacketItemCatToInsert = new MPacketItemCat();
            TransferFormValuesTo(mPacketItemCatToInsert, viewModel);
            mPacketItemCatToInsert.ItemCatId = _mItemCatRepository.Get(formCollection["ItemCatId"]);
            mPacketItemCatToInsert.PacketId = _mPacketRepository.Get(formCollection["PacketId"]);

            mPacketItemCatToInsert.SetAssignedIdTo(viewModel.Id);
            mPacketItemCatToInsert.CreatedDate = DateTime.Now;
            mPacketItemCatToInsert.CreatedBy = User.Identity.Name;
            mPacketItemCatToInsert.DataStatus = EnumDataStatus.New.ToString();

            //IList<MItemUom> listItemUom = new List<MItemUom>();
            
            //mItemToInsert.ItemUoms = listItemUom;

            _mPacketItemCatRepository.Save(mPacketItemCatToInsert);

            try
            {
                _mPacketItemCatRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mPacketItemCatRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult PopupInsert(MPacketItemCat viewModel, FormCollection formCollection, string packetId)
        {
            UpdateNumericData(viewModel, formCollection);
            MPacketItemCat mPacketItemCatToInsert = new MPacketItemCat();
            TransferFormValuesTo(mPacketItemCatToInsert, viewModel);
            mPacketItemCatToInsert.ItemCatId = _mItemCatRepository.Get(formCollection["ItemCatId"]);
            mPacketItemCatToInsert.PacketId = _mPacketRepository.Get(packetId);

            mPacketItemCatToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
            mPacketItemCatToInsert.CreatedDate = DateTime.Now;
            mPacketItemCatToInsert.CreatedBy = User.Identity.Name;
            mPacketItemCatToInsert.DataStatus = EnumDataStatus.New.ToString();

            //IList<MItemUom> listItemUom = new List<MItemUom>();

            //mItemToInsert.ItemUoms = listItemUom;

            _mPacketItemCatRepository.Save(mPacketItemCatToInsert);

            try
            {
                _mPacketItemCatRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mPacketItemCatRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Delete(MPacketItemCat viewModel, FormCollection formCollection)
        {
            MPacketItemCat mPacketItemCatToDelete = _mPacketItemCatRepository.Get(viewModel.Id);

            if (mPacketItemCatToDelete != null)
            {
                _mPacketItemCatRepository.Delete(mPacketItemCatToDelete);
            }

            try
            {
                _mPacketItemCatRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mPacketItemCatRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MPacketItemCat viewModel, FormCollection formCollection)
        {
            UpdateNumericData(viewModel, formCollection);
            MPacketItemCat mPacketItemCatToUpdate = _mPacketItemCatRepository.Get(viewModel.Id);
            mPacketItemCatToUpdate.ItemCatId = _mItemCatRepository.Get(formCollection["ItemCatId"]);
            //mPacketItemCatToUpdate.PacketId = _mPacketRepository.Get(formCollection["PacketId"]);
            TransferFormValuesTo(mPacketItemCatToUpdate, viewModel);
            mPacketItemCatToUpdate.ModifiedDate = DateTime.Now;
            mPacketItemCatToUpdate.ModifiedBy = User.Identity.Name;
            mPacketItemCatToUpdate.DataStatus = EnumDataStatus.Updated.ToString();

            _mPacketItemCatRepository.Update(mPacketItemCatToUpdate);

            try
            {
                _mPacketItemCatRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mPacketItemCatRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void UpdateNumericData(MPacketItemCat viewModel, FormCollection formCollection)
        {
            if (!string.IsNullOrEmpty(formCollection["ItemCatQty"]))
            {
                string ItemCatQty = formCollection["ItemCatQty"].Replace(",", "");
                viewModel.ItemCatQty = Convert.ToDecimal(ItemCatQty);
            }
            else
            {
                viewModel.ItemCatQty = null;
            }
        }

        private void TransferFormValuesTo(MPacketItemCat mPacketItemCatToUpdate, MPacketItemCat mPacketItemCatFromForm)
        {
            mPacketItemCatToUpdate.ItemCatQty = mPacketItemCatFromForm.ItemCatQty;
            mPacketItemCatToUpdate.PacketItemCatStatus = mPacketItemCatFromForm.PacketItemCatStatus;
            mPacketItemCatToUpdate.PacketItemCatDesc = mPacketItemCatFromForm.PacketItemCatDesc;
        }

        [Transaction]
        public virtual ActionResult GetList()
        {
            var items = _mPacketItemCatRepository.GetAll();
            StringBuilder sb = new StringBuilder();
            MPacketItemCat mItem = new MPacketItemCat();
            sb.AppendFormat("{0}:{1};", string.Empty, "-Pilih Paket Item Category-");
            for (int i = 0; i < items.Count; i++)
            {
                mItem = items[i];
                sb.AppendFormat("{0}:{1}", mItem.Id, mItem.PacketItemCatDesc);
                if (i < items.Count - 1)
                    sb.Append(";");
            }
            return Content(sb.ToString());
        }

        [Transaction]
        public virtual ActionResult Get(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                MPacketItemCat mItem = _mPacketItemCatRepository.Get(id);
            }
            return Content("0");
        }
    }
}
