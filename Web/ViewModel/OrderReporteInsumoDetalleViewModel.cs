using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.ViewModel
{
    public class OrderReporteInsumoDetalleViewModel
    {
        public int NroPedido { get; set; }
        public string Date { get; set; }
        public string Center { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public int QuantityDelivered { get; set; }
    }
}