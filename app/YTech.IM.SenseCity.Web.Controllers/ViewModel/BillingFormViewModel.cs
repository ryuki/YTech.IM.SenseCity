using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.RepositoryInterfaces;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Inventory;
using YTech.IM.SenseCity.Enums;

namespace YTech.IM.SenseCity.Web.Controllers.ViewModel
{
    public class BillingFormViewModel
    {
        public static BillingFormViewModel CreateBillingViewModel(IMRoomRepository mRoomRepository, IMEmployeeRepository mEmployeeRepository, IMCustomerRepository mCustomerRepository, ITTransRoomRepository transRoomRepository)
        {
            BillingFormViewModel viewModel = new BillingFormViewModel();

            viewModel.SpaManRoomList = GetRoomViewModel(transRoomRepository, mRoomRepository.GetListByRoomType(EnumRoomType.SpaMan));
            viewModel.SpaWomanRoomList = GetRoomViewModel(transRoomRepository, mRoomRepository.GetListByRoomType(EnumRoomType.SpaWomen));
            viewModel.ReflexyRoomList = GetRoomViewModel(transRoomRepository, mRoomRepository.GetListByRoomType(EnumRoomType.Reflexy));

            //var listCustomer = mCustomerRepository.GetAll();
            //MCustomer mCustomer = new MCustomer();
            ////mCustomer.SupplierName = "-Pilih Supplier-";
            //listCustomer.Insert(0, mCustomer);
            //var custs = from cust in listCustomer
            //            select new { Id = cust.Id, Name = cust.PersonId != null ? cust.PersonId.PersonName : "-Pilih Konsumen-" };
            //viewModel.CustomerList = new SelectList(custs, "Id", "Name");

            //var listEmployee = mEmployeeRepository.GetAll();
            //MEmployee employee = new MEmployee();
            ////mCustomer.SupplierName = "-Pilih Supplier-";
            //listEmployee.Insert(0, employee);
            //var employees = from emp in listEmployee
            //                select new { Id = emp.Id, Name = emp.PersonId != null ? emp.PersonId.PersonName : "-Pilih Terapis-" };
            //viewModel.TherapistList = new SelectList(employees, "Id", "Name");


            TTrans trans = new TTrans();
            trans.SetAssignedIdTo(Guid.NewGuid().ToString());
            trans.TransDiscount = 0;
            trans.TransDate = DateTime.Today;
            viewModel.Trans = trans;

            TTransRoom transRoom = new TTransRoom();
            transRoom.RoomInDate = DateTime.Now;
            transRoom.RoomOutDate = DateTime.Now;
            viewModel.TransRoom = transRoom;

            return viewModel;
        }

        private static IList<RoomViewModel> GetRoomViewModel(ITTransRoomRepository transRoomRepository, IList<MRoom> listRoom)
        {
            IList<RoomViewModel> result = new List<RoomViewModel>();
            RoomViewModel rvm = null;
            MRoom r = null;
            TTransRoom troom = null;
            for (int i = 0; i < listRoom.Count; i++)
            {
                rvm = new RoomViewModel();
                r = listRoom[i];
                rvm.SetAssignedIdTo(r.Id);
                rvm.RoomName = r.RoomName;
                rvm.RoomInUsed = transRoomRepository.GetByRoom(r) != null;

                result.Add(rvm);
            }
            return result;
        }

        public TTrans Trans { get; internal set; }
        public TTransRoom TransRoom { get; internal set; }
        public IList<TTransDet> ListOfTransDet { get; internal set; }

        public IList<RoomViewModel> SpaManRoomList { get; internal set; }
        public IList<RoomViewModel> SpaWomanRoomList { get; internal set; }
        public IList<RoomViewModel> ReflexyRoomList { get; internal set; }
        public SelectList CustomerList { get; internal set; }
        public SelectList TherapistList { get; internal set; }
    }

    public class RoomViewModel : MRoom
    {
        public bool RoomInUsed { get; internal set; }
    }
}
