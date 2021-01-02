using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.Controllers
{
    public class CambiarEstaudoViewModel
    {
        public int Id { get; set; }
        public int StatusId { get; set; }

        public string Observation { get; set; }
    }
}