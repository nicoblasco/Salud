using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.ViewModel
{
    public class OrderIndexViewModel
    {
        public int Id { get; set; }
        public string NroPedido { get; set; }
        public string Center { get; set; }
        public string Usuario { get; set; }
        public string Estado { get; set; }
        public string Fecha { get; set; }
        public bool Ver { get; set; }
        public bool Modificar { get; set; }
        public bool Eliminar { get; set; }

    }
}