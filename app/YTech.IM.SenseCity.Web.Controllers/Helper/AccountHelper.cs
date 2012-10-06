using System;
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
    public class AccountHelper
    {
        private static MAccount GetDefaultAccount(EnumReferenceType referenceType)
        {
            IMAccountRepository mAccountRepository = new MAccountRepository();
            MAccount account = mAccountRepository.Get(Helper.CommonHelper.GetReference(referenceType).ReferenceValue);
            return account;
        }
        private static MAccount GetDefaultAccount(DefaultAccount defaultAccount, EnumReferenceType referenceType)
        {
            //check in cache first
            object obj = System.Web.HttpContext.Current.Cache[defaultAccount.ToString()];
            //if not available, set it first
            if (obj == null)
            {
                MAccount account = GetDefaultAccount(referenceType);
                //save to cache
                System.Web.HttpContext.Current.Cache[defaultAccount.ToString()] = account;
            }
            //return cache
            return System.Web.HttpContext.Current.Cache[defaultAccount.ToString()] as MAccount;
        }

        public static MAccount GetPurchaseAccount()
        {
            return GetDefaultAccount(DefaultAccount.PurchaseAccount,EnumReferenceType.PurchaseAccountId);
        }

        public static MAccount GetCashAccount()
        {
            return GetDefaultAccount(DefaultAccount.CashAccount, EnumReferenceType.CashAccountId);
        }

        public static MAccount GetIkhtiarLRAccount()
        {
            return GetDefaultAccount(DefaultAccount.IkhtiarLRAccount, EnumReferenceType.IkhtiarLRAccountId);
        }

        internal static MAccount GetSalesAccount()
        {
            return GetDefaultAccount(DefaultAccount.SalesAccount, EnumReferenceType.SalesAccountId);
        }

        internal static MAccount GetReturPurchaseAccount()
        {
            return GetDefaultAccount(DefaultAccount.ReturPurchaseAccount, EnumReferenceType.ReturPurchaseAccountId);
        }

        internal static MAccount GetReturSalesAccount()
        {
            return GetDefaultAccount(DefaultAccount.ReturSalesAccount, EnumReferenceType.ReturSalesAccountId);
        }
    }

    internal enum DefaultAccount
    {
        PurchaseAccount,
        CashAccount,
        IkhtiarLRAccount,
        SalesAccount,
        ReturPurchaseAccount,
        ReturSalesAccount
    }
}
