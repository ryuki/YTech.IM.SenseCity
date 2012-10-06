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
using YTech.IM.SenseCity.Data.Repository;
using YTech.IM.SenseCity.Enums;

namespace YTech.IM.SenseCity.Web.Controllers.Master
{
    [HandleError]
    public class PromoController : Controller
    {
        //public PromoController()
        //    : this(new MPromoRepository())
        //{
        //}

        public PromoController(IMPromoRepository mPromoRepository)
        {
            Check.Require(mPromoRepository != null, "mPromoRepository may not be null");

            this._mPromoRepository = mPromoRepository;
        }
        private readonly IMPromoRepository _mPromoRepository;


        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var promos = _mPromoRepository.GetPagedPromoList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from promo in promos
                    select new
                    {
                        i = promo.Id.ToString(),
                        cell = new string[] {
                            promo.Id, 
                            promo.PromoName, 
                            promo.PromoStartDate.HasValue ? promo.PromoStartDate.Value.ToString(Helper.CommonHelper.DateFormat):null,
                            promo.PromoEndDate.HasValue ? promo.PromoEndDate.Value.ToString(Helper.CommonHelper.DateFormat):null,
                            promo.PromoValue.HasValue ? promo.PromoValue.Value.ToString(Helper.CommonHelper.NumberFormat):null,
                            promo.PromoDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(MPromo viewModel, FormCollection formCollection)
        {
            if (!(ViewData.ModelState.IsValid && viewModel.IsValid()))
            {

            }
            MPromo mCompanyToInsert = new MPromo();
            TransferFormValuesTo(mCompanyToInsert, viewModel);
            UpdateNumericData(mCompanyToInsert, formCollection);
            mCompanyToInsert.SetAssignedIdTo(viewModel.Id);
            mCompanyToInsert.CreatedDate = DateTime.Now;
            mCompanyToInsert.CreatedBy = User.Identity.Name;
            mCompanyToInsert.DataStatus = EnumDataStatus.New.ToString();
            _mPromoRepository.Save(mCompanyToInsert);

            try
            {
                _mPromoRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mPromoRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Delete(MPromo viewModel, FormCollection formCollection)
        {
            MPromo mCompanyToDelete = _mPromoRepository.Get(viewModel.Id);

            if (mCompanyToDelete != null)
            {
                _mPromoRepository.Delete(mCompanyToDelete);
            }

            try
            {
                _mPromoRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mPromoRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MPromo viewModel, FormCollection formCollection)
        {
            MPromo mCompanyToUpdate = _mPromoRepository.Get(viewModel.Id);
            TransferFormValuesTo(mCompanyToUpdate, viewModel);
            UpdateNumericData(mCompanyToUpdate, formCollection);
            mCompanyToUpdate.ModifiedDate = DateTime.Now;
            mCompanyToUpdate.ModifiedBy = User.Identity.Name;
            mCompanyToUpdate.DataStatus = EnumDataStatus.Updated.ToString();
            _mPromoRepository.Update(mCompanyToUpdate);

            try
            {
                _mPromoRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mPromoRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void TransferFormValuesTo(MPromo mCompanyToUpdate, MPromo mCompanyFromForm)
        {
            mCompanyToUpdate.PromoName = mCompanyFromForm.PromoName;
            mCompanyToUpdate.PromoDesc = mCompanyFromForm.PromoDesc;
            mCompanyToUpdate.PromoStartDate = mCompanyFromForm.PromoStartDate;
            mCompanyToUpdate.PromoEndDate = mCompanyFromForm.PromoEndDate;
        }

        private static void UpdateNumericData(MPromo promo, FormCollection formCollection)
        {
            if (!string.IsNullOrEmpty(formCollection["PromoValue"]))
            {
                string PromoValue = formCollection["PromoValue"].Replace(",", "");
                promo.PromoValue = Convert.ToDecimal(PromoValue);
            }
            else
            {
                promo.PromoValue = null;
            }
        }
    }
}
