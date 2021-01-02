using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int ProductTypeId { get; set; }
        public int SupplieMedicalId { get; set; }
        public int Stock { get; set; }
        public int StockMin { get; set; }

        public string Presentation { get; set; }

        public bool OutStock { get; set; }

        public bool Enable { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual SupplieMedical SupplieMedical { get; set; }
    }
}