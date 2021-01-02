using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateNBL.Models;

namespace TemplateNBL.ViewModel
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int CenterId { get; set; }
        public Center Center { get; set; }
        public Usuario Usuario { get; set; }

        public List<Product> Products { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}