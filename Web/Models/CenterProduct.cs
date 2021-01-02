using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.Models
{
    public class CenterProduct
    {
        public int Id { get; set; }
        public int CenterId { get; set; }
        public int ProductId { get; set; }
        public int Stock { get; set; }

        public virtual Center Center { get; set; }
        public virtual Product Product { get; set; }
    }
}