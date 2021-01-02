
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.ViewModel
{
    public class OrderProductViewModel
    {
        public int ProductId { get; set; }
        public string Product { get; set; }
        public string ProductType { get; set; }
        public int? Stock { get; set; }
        public int Quantity { get; set; }
        public bool? OutStock { get; set; }

    }
}