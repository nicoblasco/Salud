using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.ViewModel
{
    public class OrderCreateViewModel
    {
        public int CenterId { get; set; }
        public List<OrderProductViewModel> OrderProductViewModels { get; set; }
    }
}