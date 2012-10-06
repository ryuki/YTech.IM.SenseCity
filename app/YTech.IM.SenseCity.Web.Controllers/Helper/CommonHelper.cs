﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Inventory;
using YTech.IM.SenseCity.Enums;
using YTech.IM.SenseCity.Data.Repository;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core;

namespace YTech.IM.SenseCity.Web.Controllers.Helper
{
    public class CommonHelper
    {
        private const string CONST_FACTURFORMAT = "SenseCity/[TRANS]/[YEAR]/[MONTH]/[DAY]/[XXX]";
        public const string CONST_VOUCHERNO = "SenseCity/VOUCHER/[YEAR]/[MONTH]/[DAY]/[XXX]";

        public static string DateFormat
        {
            get { return "dd-MMM-yyyy"; }
        }
        public static string DateTimeFormat
        {
            get { return "dd-MMM-yyyy HH:mm"; }
        }
        public static string TimeFormat
        {
            get { return "HH:mm"; }
        }
        public static string NumberFormat
        {
            get { return "N2"; }
        }

        public static TReference GetReference(EnumReferenceType referenceType)
        {
            //check in cache first
            object obj = System.Web.HttpContext.Current.Cache[referenceType.ToString()];
            //if not available, set it first
            if (obj == null)
            {
                ITReferenceRepository referenceRepository = new TReferenceRepository();
                TReference reference = referenceRepository.GetByReferenceType(referenceType);
                if (reference == null)
                {
                    referenceRepository.DbContext.BeginTransaction();
                    reference = new TReference();
                    reference.SetAssignedIdTo(Guid.NewGuid().ToString());
                    reference.ReferenceType = referenceType.ToString();
                    reference.ReferenceValue = "";
                    reference.CreatedDate = DateTime.Now;
                    reference.DataStatus = EnumDataStatus.New.ToString();
                    referenceRepository.Save(reference);
                    referenceRepository.DbContext.CommitTransaction();
                }
                //save to cache
                System.Web.HttpContext.Current.Cache[referenceType.ToString()] = reference;
            }

            //return cache
            return System.Web.HttpContext.Current.Cache[referenceType.ToString()] as TReference;
        }

        public static string GetFacturNo(EnumTransactionStatus transactionStatus)
        {
            return GetFacturNo(transactionStatus, true);
        }

        public static string GetFacturNo(EnumTransactionStatus transactionStatus, bool automatedIncrease)
        {
            TReference refer = GetReference((EnumReferenceType)Enum.Parse(typeof(EnumReferenceType), transactionStatus.ToString()));
            decimal no = Convert.ToDecimal(refer.ReferenceValue) + 1;
            refer.ReferenceValue = no.ToString();
            if (automatedIncrease)
            {
                ITReferenceRepository referenceRepository = new TReferenceRepository();
                referenceRepository.Update(refer);
                referenceRepository.DbContext.CommitChanges();
            }

            string tipeTrans = string.Empty;
            char[] charTransArray = transactionStatus.ToString().ToCharArray();
            char charTrans;

            for (int i = 0; i < transactionStatus.ToString().Length; i++)
            {
                charTrans = charTransArray[i];
                if (char.IsUpper(transactionStatus.ToString(), i))
                    tipeTrans += transactionStatus.ToString().Substring(i, 1);
            }

            StringBuilder result = new StringBuilder();
            result.Append(CONST_FACTURFORMAT);
            result.Replace("[TRANS]", tipeTrans);
            result.Replace("[XXX]", GetFactur(5, no));
            result.Replace("[DAY]", DateTime.Today.Day.ToString());
            result.Replace("[MONTH]", DateTime.Today.ToString("MMM").ToUpper());
            result.Replace("[YEAR]", DateTime.Today.Year.ToString());
            return result.ToString();
        }

        public static string GetVoucherNo()
        {
            return GetVoucherNo(false);
        }

        public static string GetVoucherNo(bool automatedIncrease)
        {
            TReference refer = GetReference(EnumReferenceType.VoucherNo);
            decimal no = Convert.ToDecimal(refer.ReferenceValue) + 1;
            refer.ReferenceValue = no.ToString();
            if (automatedIncrease)
            {
                ITReferenceRepository referenceRepository = new TReferenceRepository();
                referenceRepository.DbContext.BeginTransaction();
                referenceRepository.Update(refer);
                referenceRepository.DbContext.CommitTransaction();
            }

            StringBuilder result = new StringBuilder();
            result.Append(CONST_VOUCHERNO);
            result.Replace("[XXX]", GetFactur(5, no));
            result.Replace("[DAY]", DateTime.Today.Day.ToString());
            result.Replace("[MONTH]", DateTime.Today.ToString("MMM").ToUpper());
            result.Replace("[YEAR]", DateTime.Today.Year.ToString());
            return result.ToString();
        }

        private static string GetFactur(int maxLength, decimal no)
        {
            int len = maxLength - no.ToString().Length;
            string factur = no.ToString();
            for (int i = 0; i < len; i++)
            {
                factur = "0" + factur;
            }
            return factur;
        }

        /// <summary>
        /// get list of enum for jqgrid combobox
        /// </summary>
        /// <typeparam name="T">type of enum</typeparam>
        /// <param name="defaultText">default text for display</param>
        /// <returns>string</returns>
        public static string GetEnumListForGrid<T>(string defaultText)
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("Type object must enum");
            }
            var lists = from T e in Enum.GetValues(typeof(T))
                        select new { ID = e, Name = e.ToString() };
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:{1};", string.Empty, defaultText);

            for (int i = 0; i < lists.Count(); i++)
            {
                var obj = lists.ElementAt(i);
                sb.AppendFormat("{0}:{1}", obj.ID, obj.Name);
                if (i < lists.Count() - 1)
                    sb.Append(";");
            }
            return (sb.ToString());
        }

        /// <summary>
        /// get default warehouse
        /// </summary>
        /// <returns></returns>
        internal static MWarehouse GetDefaultWarehouse()
        {
            object obj = System.Web.HttpContext.Current.Cache[EnumReferenceType.DefaultWarehouse.ToString()];
            if (obj == null)
            {
                TReference refer = GetReference(EnumReferenceType.DefaultWarehouse);
                if (!string.IsNullOrEmpty(refer.ReferenceValue))
                {
                    IMWarehouseRepository warehouseRepository = new MWarehouseRepository();
                    System.Web.HttpContext.Current.Cache[EnumReferenceType.DefaultWarehouse.ToString()] = warehouseRepository.Get(refer.ReferenceValue);
                }
            }

            return System.Web.HttpContext.Current.Cache[EnumReferenceType.DefaultWarehouse.ToString()] as MWarehouse;
        }

        internal static bool CheckStock(MWarehouse mWarehouse, MItem item, decimal? qty)
        {
            ITStockItemRepository stockItemRepository = new TStockItemRepository();
            TStockItem stockItem = stockItemRepository.GetByItemAndWarehouse(item, mWarehouse);
            if (stockItem != null)
            {
                if (stockItem.ItemStock > qty)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
