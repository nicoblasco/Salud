using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.ViewModel
{
    public class OrderEditViewModel : OrderCreateViewModel
    {
        public int Id { get; set; }
        public bool ChangeStatus { get; set; }
        public string Center { get; set; }
        public string Date { get; set; }
        public string NroPedido { get; set; }
    }
}