using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.ViewModel
{
    public class OrderReporteInsumoViewModel
    {
        public List<OrderReporteInsumoDetalleViewModel> Detalle { get; set; }
        public int TotalSolicitados { get; set; }

        public int TotalEntregados { get; set; }
    }
}