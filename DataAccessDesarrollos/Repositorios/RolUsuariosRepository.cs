using DataAccessDesarrollos.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDesarrollos.Repositorios
{
    public class RolUsuariosRepository : IRolesUsuarios
    {
        public bool AsignarRolesPorUsuario(int idUsuario, List<int> idsRoles, int asignado_por)
        {
            EliminarAsignacionesRolesPorUsuario(idUsuario);

            foreach (var idRol in idsRoles)
            {
                var rol = new RolUsuario
                {
                    id_usuario = idUsuario,
                    id_rol = idRol,
                    asignado_por = asignado_por
                };
                bool asignado = AsignarRolPorUsuario(rol);
                if (!asignado) return false;
            }
            return true;
        }

        public bool AsignarRolPorUsuario(RolUsuario rol)
        {
            try { 
                using( var da = new DataAccess()) {
                    da.ExecuteNonQueryByCode("rhsp_InsertRolesAsignadosByUsuario", cmd => {
                        cmd.Parameters.AddWithValue("@id_usuario", rol.id_usuario);
                        cmd.Parameters.AddWithValue("@id_rol", rol.id_rol);
                        cmd.Parameters.AddWithValue("@asignado_por", rol.asignado_por);
                    });
                }
                return true;
            }
            catch (Exception ex) {
                Trace.TraceError("Error al Asignar Roles por Usuario: {0}", ex);
                throw new Exception("Proceso no exitoso, error en AsignarRolesPorUsuario: " + ex.Message);
            }
        }

        public void EliminarAsignacionesRolesPorIdRol(int id)
        {
            if (id <= 0) return;
            try
            {
                using (var da = new DataAccess())
                {
                    da.ExecuteNonQueryByCode("rhsp_DeleteRolesUsuarioByIdRol", cmd => {
                        cmd.Parameters.AddWithValue("@id_rol", id);
                    });
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error al Eliminar Asignaciones de Roles por id_rol: {0}", ex);
                throw new Exception("Proceso no exitoso, error en EliminarAsignacionesRolesPorIdRol: " + ex.Message);
            }
        }

        public void EliminarAsignacionesRolesPorUsuario(int id)
        {
            if(id <= 0) return;
            try {
                using (var da = new DataAccess()) {
                    da.ExecuteNonQueryByCode("rhsp_DeleteRolesAsignadosByIdUsuario", cmd => {
                        cmd.Parameters.AddWithValue("@id_usuario", id);
                    });
                }
            }
            catch (Exception ex) {
                Trace.TraceError("Error al Eliminar Asignaciones de Roles por Usuario: {0}", ex);
                throw new Exception("Proceso no exitoso, error en EliminarAsignacionesRolesPorUsuario: " + ex.Message);
            }
        }

        public List<RolUsuario> ObtenerRolesAsignadosPorUsuario(int id)
        {
            try {
                using (var da = new DataAccess()) {
                    var lista = da.ExecuteReaderByCode("rhsp_GetRolesAsignadosByIdUsuario", rdr => new RolUsuario
                    {
                        id_rol = Convert.ToInt32(rdr["id_rol"]),
                        nombre_rol = rdr["nombre_rol"] != DBNull.Value ? rdr["nombre_rol"].ToString() : string.Empty,
                    },cmd => { 
                        cmd.Parameters.AddWithValue("@id_usuario", id);
                    }
                    ).ToList();
                    return lista ?? new List<RolUsuario>();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error Obtener al Obtener Roles Asignados por Usuario: {0}", ex);
                throw new Exception("Proceso no exitoso, error en ObtenerRolesAsignadosPorUsuario: " + ex.Message);
            }
        }

        public List<RolUsuario> ObtenerRolesDisponiblesPorUsuario(int id)
        {
            try
            {
                using (var da = new DataAccess()) {
                    var lista = da.ExecuteReaderByCode("rhsp_GetRolesDisponiblesByIdUsuario", rdr => new RolUsuario
                    {
                        id_rol = Convert.ToInt32(rdr["id_rol"]),
                        nombre_rol = rdr["nombre_rol"] != DBNull.Value ? rdr["nombre_rol"].ToString() : string.Empty,
                    }, cmd => {
                        cmd.Parameters.AddWithValue("@id_usuario", id);
                    }

                    ).ToList();

                    return lista ?? new List<RolUsuario>();
                }
            }
            catch (Exception ex) {
                Trace.TraceError("Error Obtener al Obtener Roles Disponibles por Usuario: {0}", ex);
                throw new Exception(" Proceso no exitoso, error en ObtenerRolesDisponiblesPorUsuario: " + ex.Message);
            }
        }

        //id_usuario_rol id_usuario  id_rol fecha_asignacion    asignado_por
    }
}
