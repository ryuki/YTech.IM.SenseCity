using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.IM.SenseCity.Core.Master;

namespace YTech.IM.SenseCity.Web.Controllers.ViewModel.Reports
{
    public class CustomerViewModel : MCustomer
    {
        public string CustomerId { get; set; }
        public string PersonDob { get; set; }
        public string PersonPhone { get; set; }
        public string PersonName { get; set; }
    }
}
