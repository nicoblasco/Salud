using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public bool Enable { get; set; }
    }
}