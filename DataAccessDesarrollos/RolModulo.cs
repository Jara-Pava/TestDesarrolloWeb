using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDesarrollos
{
    public class RolModulo
    {
        public int id_rol_catalogo { get; set; }
        public int id_rol { get; set; }
        public int id_modulo_catalogo { get; set; }
        public string nombre_catalogo { get; set; }
        public bool puede_ver { get; set; }
        public bool puede_crear { get; set; }
        public bool puede_editar { get; set; }
        public bool puede_eliminar { get; set; }
        public bool puede_aprobar { get; set; }
        public int? creado_por { get; set; }
        public string nombre_creador { get; set; }
        public int? modificado_por { get; set; }
        public string nombre_modificador { get; set; }
    }
}
