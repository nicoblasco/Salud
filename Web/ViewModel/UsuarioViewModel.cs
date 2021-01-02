using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateNBL.ViewModel
{
    public class UsuarioViewModel
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public string Nombreusuario { get; set; }

        public string Contraseña { get; set; }

        public string Rol { get; set; }

        public string Center { get; set; }

    }
}