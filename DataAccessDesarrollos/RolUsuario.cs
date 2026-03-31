using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDesarrollos
{
    public class RolUsuario
    {
        public int id_usuario_rol { get; set; }
        public int id_usuario { get; set; }
        public string nombre_rol { get; set; }
        public int id_rol { get; set; }
        public DateTime fecha_asignacion { get; set; }
        public int asignado_por { get; set; }
    }
}
