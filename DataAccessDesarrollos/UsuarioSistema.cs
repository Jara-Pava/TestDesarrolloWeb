using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDesarrollos
{
    public class Usuario
    {
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string sigla_red { get; set; }
        public bool activo { get; set; }
        public string Email { get; set; }
    }
}
