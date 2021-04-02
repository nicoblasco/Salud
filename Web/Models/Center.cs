using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.Models
{
    public class Center
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public bool Enable { get; set; }

        public virtual ICollection<CenterProduct> CenterProducts { get; set; }

        public virtual bool TieneStockSinCargar()
        {
            if (this.CenterProducts == null)
                return false;
            else
            {
                return this.CenterProducts.Sum(x => x.Stock)==0;
            }
        }
    }
}