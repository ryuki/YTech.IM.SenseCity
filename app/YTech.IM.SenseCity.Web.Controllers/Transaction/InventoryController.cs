using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Reporting.WebForms;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Inventory;
using YTech.IM.SenseCity.Data.Repository;
using YTech.IM.SenseCity.Enums;
using YTech.IM.SenseCity.Web.Controllers.ViewModel;

namespace YTech.IM.SenseCity.Web.Controllers.Transaction
{
    [HandleError]
    public partial class InventoryController : Controller
    {
        private readonly ITTransRepository _tTransRepository;
        private readonly IMWarehouseRepository _mWarehouseRepository;
        private readonly IMSupplierRepository _mSupplierRepository;
        private readonly IMItemRepository _mItemRepository;
        private readonly ITStockCardRepository _tStockCardRepository;
        private readonly ITStockItemRepository _tStockItemRepository;
        private readonly ITTransRefRepository _tTransRefRepository;
        private readonly ITStockRepository _tStockRepository;
        private readonly ITStockRefRepository _tStockRefRepository;
        private readonly IMCustomerRepository _mCustomerRepository;
        private readonly IMRoomRepository _mRoomRepository;
        private readonly IMEmployeeRepository _mEmployeeRepository;
        private readonly ITTransDetRepository _tTransDetRepository;
        private readonly ITTransRoomRepository _tTransRoomRepository;
        private readonly IMPacketRepository _mPacketRepository;
        private readonly IMPacketItemCatRepository _mPacketItemCatRepository;
        private readonly ITTransDetItemRepository _tTransDetItemRepository;
        private readonly IMPacketCommRepository _mPacketCommRepository;
        private readonly IMAccountRefRepository _mAccountRefRepository;
        private readonly ITJournalRepository _tJournalRepository;
        private readonly ITJournalDetRepository _tJournalDetRepository;
        private readonly IMAccountRepository _mAccountRepository;
        private readonly IMPromoRepository _mPromoRepository;

        public InventoryController(ITTransRepository tTransRepository, IMWarehouseRepository mWarehouseRepository, IMSupplierRepository mSupplierRepository, IMItemRepository mItemRepository, ITStockCardRepository tStockCardRepository, ITStockItemRepository tStockItemRepository, ITTransRefRepository tTransRefRepository, ITStockRepository tStockRepository, ITStockRefRepository tStockRefRepository, IMCustomerRepository mCustomerRepository, IMRoomRepository mRoomRepository, IMEmployeeRepository mEmployeeRepository, ITTransDetRepository tTransDetRepository, ITTransRoomRepository tTransRoomRepository, IMPacketRepository mPacketRepository, IMPacketItemCatRepository mPacketItemCatRepository, ITTransDetItemRepository tTransDetItemRepository, IMPacketCommRepository mPacketCommRepository, IMAccountRefRepository mAccountRefRepository, ITJournalRepository tJournalRepository, ITJournalDetRepository tJournalDetRepository, IMAccountRepository mAccountRepository, IMPromoRepository mPromoRepository)
        {
            Check.Require(tTransRepository != null, "tTransRepository may not be null");
            Check.Require(mWarehouseRepository != null, "mWarehouseRepository may not be null");
            Check.Require(mSupplierRepository != null, "mSupplierRepository may not be null");
            Check.Require(mItemRepository != null, "mItemRepository may not be null");
            Check.Require(tStockCardRepository != null, "tStockCardRepository may not be null");
            Check.Require(tStockItemRepository != null, "tStockItemRepository may not be null");
            Check.Require(tTransRefRepository != null, "tTransRefRepository may not be null");
            Check.Require(tStockRepository != null, "tStockRepository may not be null");
            Check.Require(tStockRefRepository != null, "tStockRefRepository may not be null");
            Check.Require(mCustomerRepository != null, "mCustomerRepository may not be null");
            Check.Require(mRoomRepository != null, "mRoomRepository may not be null");
            Check.Require(mEmployeeRepository != null, "mEmployeeRepository may not be null");
            Check.Require(tTransDetRepository != null, "tTransDetRepository may not be null");
            Check.Require(tTransRoomRepository != null, "tTransRoomRepository may not be null");
            Check.Require(mPacketRepository != null, "mPacketRepository may not be null");
            Check.Require(mPacketItemCatRepository != null, "mPacketItemCatRepository may not be null");
            Check.Require(tTransDetItemRepository != null, "tTransDetItemRepository may not be null");
            Check.Require(mPacketCommRepository != null, "mPacketCommRepository may not be null");
            Check.Require(mAccountRefRepository != null, "mAccountRefRepository may not be null");
            Check.Require(tJournalRepository != null, "tJournalRepository may not be null");
            Check.Require(tJournalDetRepository != null, "tJournalDetRepository may not be null");
            Check.Require(mAccountRepository != null, "mAccountRepository may not be null");
            Check.Require(mPromoRepository != null, "mPromoRepository may not be null");

            this._tTransRepository = tTransRepository;
            this._mWarehouseRepository = mWarehouseRepository;
            this._mSupplierRepository = mSupplierRepository;
            this._mItemRepository = mItemRepository;
            this._tStockCardRepository = tStockCardRepository;
            this._tStockItemRepository = tStockItemRepository;
            this._tTransRefRepository = tTransRefRepository;
            this._tStockRepository = tStockRepository;
            this._tStockRefRepository = tStockRefRepository;
            this._mCustomerRepository = mCustomerRepository;
            this._mRoomRepository = mRoomRepository;
            this._mEmployeeRepository = mEmployeeRepository;
            this._tTransDetRepository = tTransDetRepository;
            this._tTransRoomRepository = tTransRoomRepository;
            this._mPacketRepository = mPacketRepository;
            this._mPacketItemCatRepository = mPacketItemCatRepository;
            this._tTransDetItemRepository = tTransDetItemRepository;
            this._mPacketCommRepository = mPacketCommRepository;
            this._mAccountRefRepository = mAccountRefRepository;
            this._tJournalRepository = tJournalRepository;
            this._tJournalDetRepository = tJournalDetRepository;
            this._mAccountRepository = mAccountRepository;
            this._mPromoRepository = mPromoRepository;

        }

        public ActionResult Index()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.PurchaseOrder);

            ListDetTrans = new List<TTransDet>();

            return View(viewModel);
        }

        private TransactionFormViewModel SetViewModelByStatus(EnumTransactionStatus enumTransactionStatus)
        {
            TransactionFormViewModel viewModel = TransactionFormViewModel.CreateTransactionFormViewModel(enumTransactionStatus, _tTransRepository, _mWarehouseRepository, _mSupplierRepository, _mCustomerRepository);

            ViewData["CurrentItem"] = viewModel.Title;
            //ViewData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.NotSaved;

            return viewModel;
        }

        public ActionResult Purchase()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Purchase);

            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Purchase(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        private ActionResult SaveTransactionRef(TTrans Trans, FormCollection formCollection)
        {
            _tTransRepository.DbContext.BeginTransaction();
            if (Trans == null)
            {
                Trans = new TTrans();
            }
            Trans.SetAssignedIdTo(formCollection["Trans.Id"]);
            Trans.CreatedDate = DateTime.Now;
            Trans.CreatedBy = User.Identity.Name;
            Trans.DataStatus = Enums.EnumDataStatus.New.ToString();
            Trans.TransSubTotal = ListTransRef.Sum(x => x.TransIdRef.TransSubTotal);
            _tTransRepository.Save(Trans);
            _tTransRepository.DbContext.CommitTransaction();

            _tTransRefRepository.DbContext.BeginTransaction();
            TTransRef detToInsert;
            foreach (TTransRef det in ListTransRef)
            {
                detToInsert = new TTransRef();
                detToInsert.SetAssignedIdTo(det.Id);
                detToInsert.TransId = Trans;
                detToInsert.TransIdRef = det.TransIdRef;
                detToInsert.TransRefDesc = det.TransRefDesc;
                detToInsert.TransRefStatus = det.TransRefStatus;

                detToInsert.CreatedBy = User.Identity.Name;
                detToInsert.CreatedDate = DateTime.Now;
                detToInsert.DataStatus = EnumDataStatus.New.ToString();
                _tTransRefRepository.Save(detToInsert);
            }
            try
            {
                _tTransRefRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
            }
            catch (Exception)
            {
                _tTransRefRepository.DbContext.RollbackTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
            }
            if (!Trans.TransStatus.Equals(EnumTransactionStatus.PurchaseOrder.ToString()))
            {
                return RedirectToAction(Trans.TransStatus);
            }
            return RedirectToAction("Index");
        }

        public ActionResult ReturPurchase()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.ReturPurchase);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReturPurchase(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        public ActionResult Using()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Using);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Using(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        public ActionResult Received()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Received);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Received(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        public ActionResult Mutation()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Mutation);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Mutation(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        [Transaction]
        public ActionResult Adjusment()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Adjusment);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Adjusment(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        private List<TTransDet> ListDetTrans
        {
            get
            {
                if (Session["listDetTrans"] == null)
                {
                    Session["listDetTrans"] = new List<TTransDet>();
                }
                return Session["listDetTrans"] as List<TTransDet>;
            }
            set
            {
                Session["listDetTrans"] = value;
            }
        }

        private List<TTransRef> ListTransRef
        {
            get
            {
                if (Session["ListTransRef"] == null)
                {
                    Session["ListTransRef"] = new List<TTransRef>();
                }
                return Session["ListTransRef"] as List<TTransRef>;
            }
            set
            {
                Session["ListTransRef"] = value;
            }
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string usePrice)
        {
            int totalRecords = 0;
            var transDets = ListDetTrans;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var result = (
                           from det in transDets
                           select new
                                      {
                                          i = det.Id.ToString(),
                                          cell = new string[]
                                                     {
                                                         det.Id,
                                                         det.ItemId != null ? det.ItemId.Id : null,
                                                         det.ItemId != null ? det.ItemId.ItemName : null,
                                                         det.TransDetQty.HasValue ?  det.TransDetQty.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                        det.TransDetPrice.HasValue ?  det.TransDetPrice.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                        det.TransDetDisc.HasValue ?   det.TransDetDisc.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                        det.TransDetTotal.HasValue ?   det.TransDetTotal.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                         det.TransDetDesc
                                                     }
                                      });

            decimal? transDetTotal = transDets.Sum(det => det.TransDetTotal);
            decimal? transDetQty = transDets.Sum(det => det.TransDetQty);
            var userdata = new
            {
                ItemName = "Total",
                TransDetQty = transDetQty.Value.ToString(Helper.CommonHelper.NumberFormat),
                TransDetTotal = transDetTotal.Value.ToString(Helper.CommonHelper.NumberFormat)
            };
            if (usePrice.Equals(false.ToString()))
            {
                result = (
                           from det in transDets
                           select new
                           {
                               i = det.Id.ToString(),
                               cell = new string[]
                                                     {
                                                         det.Id,
                                                         det.ItemId != null ? det.ItemId.Id : null,
                                                         det.ItemId != null ? det.ItemId.ItemName : null,
                                                       det.TransDetQty.HasValue ?    det.TransDetQty.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                         det.TransDetDesc
                                                     }
                           });
            }

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = result.ToArray(),
                userdata
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public virtual ActionResult GetListTransRef(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var transRefs = ListTransRef;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from det in transRefs
                    select new
                    {
                        i = det.Id.ToString(),
                        cell = new string[] {
                             det.TransIdRef.Id,
                             det.TransIdRef.Id,
                            det.TransIdRef.TransFactur, 
                            det.TransIdRef.TransDate.HasValue ? det.TransIdRef.TransDate.Value.ToString(Helper.CommonHelper.DateFormat) : null,
                           det.TransIdRef.TransSubTotal.HasValue ?  det.TransIdRef.TransSubTotal.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                            det.TransIdRef.TransDesc
                        }
                    }).ToArray()
                //userdata: {price:1240.00} 
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(TTransDet viewModel, FormCollection formCollection)
        {
            TTransDet transDetToInsert = new TTransDet();
            TransferFormValuesTo(transDetToInsert, viewModel);
            transDetToInsert.SetAssignedIdTo(viewModel.Id);
            transDetToInsert.CreatedDate = DateTime.Now;
            transDetToInsert.CreatedBy = User.Identity.Name;
            transDetToInsert.DataStatus = EnumDataStatus.New.ToString();

            ListDetTrans.Add(transDetToInsert);
            return Content("success");
        }

        public ActionResult Delete(TTransDet viewModel, FormCollection formCollection)
        {
            ListDetTrans.Remove(viewModel);
            return Content("success");
        }

        public ActionResult DeleteTransRef(TTransRef viewModel, FormCollection formCollection)
        {
            ListTransRef.Remove(viewModel);
            return Content("success");
        }

        public ActionResult Insert(TTransDet viewModel, FormCollection formCollection, bool IsAddStock, string warehouseId)
        {
            //format numeric 
            UpdateNumericData(viewModel, formCollection);
            //
            MItem item = _mItemRepository.Get(formCollection["ItemId"]);
            //check stock is enough or not if no add stock 
            //return Content(IsAddStock.ToString());
            if (!IsAddStock)
            {
                MWarehouse warehouse = _mWarehouseRepository.Get(warehouseId);
                bool isStockValid = Helper.CommonHelper.CheckStock(warehouse, item, viewModel.TransDetQty);
                if (!isStockValid)
                {
                    return Content("Kuantitas barang tidak cukup");
                }
            }

            TTransDet transDetToInsert = new TTransDet();
            TransferFormValuesTo(transDetToInsert, viewModel);
            transDetToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
            transDetToInsert.ItemId = item;
            transDetToInsert.SetAssignedIdTo(viewModel.Id);
            transDetToInsert.CreatedDate = DateTime.Now;
            transDetToInsert.CreatedBy = User.Identity.Name;
            transDetToInsert.DataStatus = EnumDataStatus.New.ToString();

            ListDetTrans.Add(transDetToInsert);
            return Content("success");
        }

        private static void UpdateNumericData(TTransDet viewModel, FormCollection formCollection)
        {
            if (!string.IsNullOrEmpty(formCollection["TransDetQty"]))
            {
                string wide = formCollection["TransDetQty"].Replace(",", "");
                decimal? qty = Convert.ToDecimal(wide);
                viewModel.TransDetQty = qty;
            }
            else
            {
                viewModel.TransDetQty = null;
            }
            if (!string.IsNullOrEmpty(formCollection["TransDetPrice"]))
            {
                string wide = formCollection["TransDetPrice"].Replace(",", "");
                decimal? price = Convert.ToDecimal(wide);
                viewModel.TransDetPrice = price;
            }
            else
            {
                viewModel.TransDetPrice = null;
            }
            if (!string.IsNullOrEmpty(formCollection["TransDetDisc"]))
            {
                string wide = formCollection["TransDetDisc"].Replace(",", "");
                decimal? disc = Convert.ToDecimal(wide);
                viewModel.TransDetDisc = disc;
            }
            else
            {
                viewModel.TransDetDisc = null;
            }
            if (!string.IsNullOrEmpty(formCollection["TransDetTotal"]))
            {
                string wide = formCollection["TransDetTotal"].Replace(",", "");
                decimal? total = Convert.ToDecimal(wide);
                viewModel.TransDetTotal = total;
            }
            else
            {
                viewModel.TransDetTotal = null;
            }
        }

        public ActionResult InsertTransRef(TTransRef viewModel, FormCollection formCollection)
        {
            TTransRef transDetToInsert = new TTransRef();

            transDetToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
            transDetToInsert.TransIdRef = _tTransRepository.Get(formCollection["TransIdRef"]);
            //transDetToInsert.TransId = _tTransRepository.Get(formCollection["TransId"]);
            transDetToInsert.CreatedDate = DateTime.Now;
            transDetToInsert.CreatedBy = User.Identity.Name;
            transDetToInsert.DataStatus = EnumDataStatus.New.ToString();

            ListTransRef.Add(transDetToInsert);
            return Content("success");
        }

        private void TransferFormValuesTo(TTransDet transDet, TTransDet viewModel)
        {
            transDet.TransDetNo = ListDetTrans.Count + 1;
            transDet.TransDetQty = viewModel.TransDetQty;
            transDet.TransDetPrice = viewModel.TransDetPrice;
            transDet.TransDetDisc = viewModel.TransDetDisc;
            transDet.TransDetTotal = viewModel.TransDetTotal;
            transDet.TransDetDesc = viewModel.TransDetDesc;
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        private ActionResult SaveTransaction(TTrans Trans, FormCollection formCollection)
        {
            _tTransRepository.DbContext.BeginTransaction();
            if (Trans == null)
            {
                Trans = new TTrans();
            }
            Trans.SetAssignedIdTo(formCollection["Trans.Id"]);
            Trans.WarehouseId = _mWarehouseRepository.Get(formCollection["Trans.WarehouseId"]);
            if (!string.IsNullOrEmpty(formCollection["Trans.WarehouseIdTo"]))
            {
                Trans.WarehouseIdTo = _mWarehouseRepository.Get(formCollection["Trans.WarehouseIdTo"]);
            }
            Trans.CreatedDate = DateTime.Now;
            Trans.CreatedBy = User.Identity.Name;
            Trans.DataStatus = Enums.EnumDataStatus.New.ToString();

            Trans.TransDets.Clear();

            //save stock card
            bool addStock = true;
            bool calculateStock = false;
            EnumTransactionStatus status = (EnumTransactionStatus)Enum.Parse(typeof(EnumTransactionStatus), Trans.TransStatus);
            TransactionFormViewModel.GetIsCalculateStock(status, out addStock, out calculateStock);


            TTransDet detToInsert;
            IList<TTransDet> listDet = new List<TTransDet>();
            decimal total = 0;
            foreach (TTransDet det in ListDetTrans)
            {
                detToInsert = new TTransDet(Trans);
                detToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
                detToInsert.ItemId = det.ItemId;
                detToInsert.ItemUomId = det.ItemUomId;
                detToInsert.TransDetQty = det.TransDetQty;
                detToInsert.TransDetPrice = det.TransDetPrice;
                detToInsert.TransDetDisc = det.TransDetDisc;
                detToInsert.TransDetTotal = det.TransDetTotal;
                detToInsert.CreatedBy = User.Identity.Name;
                detToInsert.CreatedDate = DateTime.Now;
                detToInsert.DataStatus = Enums.EnumDataStatus.New.ToString();
                Trans.TransDets.Add(detToInsert);
                total += det.TransDetTotal.HasValue ? det.TransDetTotal.Value : 0;
                listDet.Add(detToInsert);
            }
            Trans.TransSubTotal = total;
            _tTransRepository.Save(Trans);
            //_tTransRepository.DbContext.CommitTransaction();

            //_tStockCardRepository.DbContext.BeginTransaction();
            if (calculateStock)
            {
                decimal totalHpp = 0;
                foreach (TTransDet det in listDet)
                {
                    //save stock
                    if (Trans.TransStatus.Equals(EnumTransactionStatus.Mutation.ToString()))
                    {
                        SaveStockItem(Trans.TransDate, Trans.TransDesc, det.ItemId, det.TransDetQty, false, Trans.WarehouseId);
                        SaveStockItem(Trans.TransDate, Trans.TransDesc, det.ItemId, det.TransDetQty, true, Trans.WarehouseIdTo);

                        //still to do, for mutation, price of stock must recalculate per stock, 
                        //sum hpp for each stock for stock out
                        totalHpp += UpdateStock(Trans.TransDate, Trans.TransDesc, Trans.TransStatus, det.ItemId, det.TransDetPrice, det.TransDetQty, det, false, Trans.WarehouseId);
                        UpdateStock(Trans.TransDate, Trans.TransDesc, Trans.TransStatus, det.ItemId, det.TransDetPrice, det.TransDetQty, det, true, Trans.WarehouseIdTo);
                    }
                    else
                    {
                        SaveStockItem(Trans.TransDate, Trans.TransDesc, det.ItemId, det.TransDetQty, addStock, Trans.WarehouseId);

                        //sum hpp for each stock
                        totalHpp += UpdateStock(Trans.TransDate, Trans.TransDesc, Trans.TransStatus, det.ItemId, det.TransDetPrice, det.TransDetQty, det, addStock, Trans.WarehouseId);
                    }
                }
                //save journal
                SaveJournal(Trans, totalHpp);
            }


            try
            {
                _tTransRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
            }
            catch (Exception)
            {
                _tTransRepository.DbContext.RollbackTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
            }
            //if (!Trans.TransStatus.Equals(EnumTransactionStatus.PurchaseOrder.ToString()))
            //{
            //    return RedirectToAction(Trans.TransStatus.ToString());
            //}
            //return RedirectToAction("Index");
            return View("Status");
        }

        public ActionResult Budgeting()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Budgeting);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Budgeting(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        public ActionResult Sales()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Sales);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Sales(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        public ActionResult ReturSales()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.ReturSales);

            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReturSales(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        [Transaction]
        public virtual ActionResult GetListTrans(string transStatus, string warehouseId, string transBy)
        {
            IList<TTrans> transes;
            //if (!string.IsNullOrEmpty(transStatus))
            MWarehouse warehouse = _mWarehouseRepository.Get(warehouseId);
            {
                transes = _tTransRepository.GetByWarehouseStatusTransBy(warehouse, transStatus, transBy);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:{1}", string.Empty, "-Pilih Faktur-");
            foreach (TTrans trans in transes)
            {
                sb.AppendFormat(";{0}:{1}", trans.Id, trans.TransFactur);
            }
            return Content(sb.ToString());
        }
    }
}
