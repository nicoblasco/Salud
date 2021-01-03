using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.ViewModel
{
    public class OrderTrackingViewModel
    {
        public int Id { get; set; }
        public string Fecha { get; set; }
        public string EstadoDesde { get; set; }
        public string EstadoHasta { get; set; }
        public string Usuario { get; set; }
        public string Observacion { get; set; }
    }
}