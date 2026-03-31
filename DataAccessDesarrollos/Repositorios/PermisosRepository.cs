using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DataAccessDesarrollos.Repositorios
{
    public class PermisosRepository
    {
        /// <summary>
        /// Obtiene los permisos consolidados de todos los roles del usuario.
        /// </summary>
        public List<PermisoModulo> ObtenerPermisosPorUsuario(int idUsuario)
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var lista = da.ExecuteReaderByCode("rhsp_GetPermisosUsuario", rdr => new PermisoModulo
                    {
                        id_modulo_catalogo = Convert.ToInt32(rdr["id_modulo_catalogo"]),
                        nombre_modulo = rdr["nombre_modulo"] != DBNull.Value ? rdr["nombre_modulo"].ToString() : string.Empty,
                        puede_ver = rdr["puede_ver"] != DBNull.Value && Convert.ToBoolean(rdr["puede_ver"]),
                        puede_crear = rdr["puede_crear"] != DBNull.Value && Convert.ToBoolean(rdr["puede_crear"]),
                        puede_editar = rdr["puede_editar"] != DBNull.Value && Convert.ToBoolean(rdr["puede_editar"]),
                        puede_eliminar = rdr["puede_eliminar"] != DBNull.Value && Convert.ToBoolean(rdr["puede_eliminar"]),
                        puede_aprobar = rdr["puede_aprobar"] != DBNull.Value && Convert.ToBoolean(rdr["puede_aprobar"]),
                    }, cmd =>
                    {
                        cmd.Parameters.AddWithValue("@id_usuario", idUsuario);
                    }).ToList();

                    return lista ?? new List<PermisoModulo>();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error ObtenerPermisosPorUsuario: {0}", ex);
                return new List<PermisoModulo>();
            }
        }
    }
}