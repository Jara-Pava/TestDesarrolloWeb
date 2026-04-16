using System;
using System.Collections.Generic;

namespace DesarrollosQAS
{
    /// <summary>
    /// Mapea las páginas .aspx al id_modulo_catalogo de la tabla RH_ModuloCatalogo.
    /// La clave es el nombre del archivo .aspx (sin ruta).
    /// El valor es el id_modulo_catalogo correspondiente.
    /// 
    /// IMPORTANTE: Ajustar los IDs según los valores reales en tu tabla RH_ModuloCatalogo.
    /// </summary>
    public static class PageModuleMap
    {
        private static readonly Dictionary<string, int> _map =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            // ===== Catálogos =====
            { "empresascontratistas.aspx", 17 },
            { "plantas.aspx",             18 },
            { "proyectos.aspx",           20 },
            { "tiposvisitas.aspx",        21 },

            // ===== Seguridad =====
            { "usuarios.aspx",            14 },
            { "roles.aspx",               15 },
            { "modulos.aspx",             28 },
            { "rolusuarios.aspx",         29 },  // Sub-página de Usuarios
            { "rolmodulos.aspx",          30 },  // Sub-página de Modulos

            // ===== Operación =====
            { "solicitudesespeciales.aspx", 32 },
            { "solicitudespecial.aspx",    32 }   // Sub-página de Solicitudes Especiales (mismo permiso)
        };

        /// <summary>
        /// Obtiene el id_modulo_catalogo asociado a una página.
        /// Retorna null si la página no requiere verificación de permisos.
        /// </summary>
        public static int? GetIdModulo(string pageFileName)
        {
            if (string.IsNullOrWhiteSpace(pageFileName))
                return null;

            string fileName = System.IO.Path.GetFileName(pageFileName);

            int idModulo;
            if (_map.TryGetValue(fileName, out idModulo))
                return idModulo;

            return null;
        }
    }
}