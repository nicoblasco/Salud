using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateNBL.Models;

namespace TemplateNBL.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int NroPedido { get; set; }

        public DateTime InitialDate { get; set; }
        public int CenterId { get; set; }
        public int UsuarioId { get; set; }

        public int StatusId { get; set; }

        public virtual Center Center { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Status Status { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ICollection<OrderTracking> OrderTrackings { get; set; }
        

    }
}