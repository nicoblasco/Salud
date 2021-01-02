using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateNBL.Models;

namespace TemplateNBL.ViewModel
{
    public class CenterProductStockViewModel
    {
        public int CenterId { get; set; }

        public virtual List<ProductStockViewModel> ProductStockViewModels { get; set; }

    }
}