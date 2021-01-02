using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.Models
{
    public class OrderTracking
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime Fecha { get; set; }
        public int? SinceStatusId { get; set; }
        public int ToStatusId { get; set; }
        public string Observation { get; set; }
        public int UsuarioId { get; set; }
        public virtual Order Order { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}