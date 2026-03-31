using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDesarrollos
{
    public class PermisoModulo
    {
        public int id_modulo_catalogo { get; set; }
        public string nombre_modulo { get; set; }
        public bool puede_ver { get; set; }
        public bool puede_crear { get; set; }
        public bool puede_editar { get; set; }
        public bool puede_eliminar { get; set; }
        public bool puede_aprobar { get; set; }
    }
}
