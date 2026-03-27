using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace DataAccessDesarrollos.Repositorios
{
    public class RolModulosRepository
    {
        public List<RolModulo> ObtenerModulosDisponiblesPorRol(int idRol)
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var lista = da.ExecuteReaderByCode("rhsp_GetModulosDisponiblesByIdRol", rdr => new RolModulo
                    {
                        id_modulo_catalogo = Convert.ToInt32(rdr["id_modulo_catalogo"]),
                        nombre_catalogo = rdr["nombre_catalogo"] != DBNull.Value ? rdr["nombre_catalogo"].ToString() : null,
                    }, cmd =>
                    {
                        cmd.Parameters.AddWithValue("@id_rol", idRol);
                    }).ToList();

                    return lista ?? new List<RolModulo>();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error ObtenerPorRol RolCatalogos: {0}", ex);
                return new List<RolModulo>();
            }
        }

        public List<RolModulo> ObtenerModulosAsignadosPorIdRol(int idRol) {
            try
            {
                using (var da = new DataAccess())
                {
                    var lista = da.ExecuteReaderByCode("rhsp_GetModulosAsignadosByIdRol", rdr => new RolModulo
                    {
                        id_modulo_catalogo = Convert.ToInt32(rdr["id_modulo_catalogo"]),
                        nombre_catalogo = rdr["nombre_catalogo"] != DBNull.Value ? rdr["nombre_catalogo"].ToString() : null,
                    }, cmd =>
                    {
                        cmd.Parameters.AddWithValue("@id_rol", idRol);
                    }).ToList();

                    return lista ?? new List<RolModulo>();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error ObtenerPorRol RolCatalogos: {0}", ex);
                return new List<RolModulo>();
            }
        }

        public bool CrearRolModuloPorIdRol(RolModulo item)
        {
            if (item == null) return false;

            try
            {
                using (var da = new DataAccess())
                {
                    var result = da.ExecuteScalarByCode("rhsp_InsertRolCatalogoByIdRol", cmd =>
                    {
                        cmd.Parameters.AddWithValue("@id_rol", item.id_rol);
                        cmd.Parameters.AddWithValue("@id_modulo_catalogo", item.id_modulo_catalogo);
                    });

                    return result != null && result != DBNull.Value;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error Crear RolCatalogo: {0}", ex);
                return false;
            }
        }

        public bool Actualizar(RolModulo item)
        {
            if (item == null || item.id_rol_catalogo <= 0) return false;

            try
            {
                using (var da = new DataAccess())
                {
                    int rows = da.ExecuteNonQueryByCode("rhsp_UpdateRolCatalogo", cmd =>
                    {
                        cmd.Parameters.AddWithValue("@id_rol_catalogo", item.id_rol_catalogo);
                        cmd.Parameters.AddWithValue("@id_rol", item.id_rol);
                        cmd.Parameters.AddWithValue("@id_modulo_catalogo", item.id_modulo_catalogo);
                        cmd.Parameters.AddWithValue("@puede_ver", item.puede_ver);
                        cmd.Parameters.AddWithValue("@puede_crear", item.puede_crear);
                        cmd.Parameters.AddWithValue("@puede_editar", item.puede_editar);
                        cmd.Parameters.AddWithValue("@puede_eliminar", item.puede_eliminar);
                        cmd.Parameters.AddWithValue("@puede_aprobar", item.puede_aprobar);
                        cmd.Parameters.Add(new SqlParameter("@modificado_por", (object)item.modificado_por ?? DBNull.Value));
                    });

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error Actualizar RolCatalogo: {0}", ex);
                return false;
            }
        }

        public void Eliminar(int idRol)
        {
            if (idRol <= 0) return;

            try
            {
                using (var da = new DataAccess())
                {
                    da.ExecuteNonQueryByCode("rhsp_DeleteRolCatalogoByIdRol", cmd =>
                    {
                        cmd.Parameters.AddWithValue("@id_rol", idRol);
                    });
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error Eliminar RolCatalogo: {0}", ex);
                throw;
            }
        }

        /// <summary>
        /// Verifica si ya existe una asignación del módulo al rol.
        /// </summary>
        public bool ExisteModuloEnRol(int idRol, int idModuloCatalogo, int? idExcluir = null)
        {
            try
            {
                var items = ObtenerModulosDisponiblesPorRol(idRol);
                return items.Any(x =>
                    x.id_modulo_catalogo == idModuloCatalogo &&
                    (!idExcluir.HasValue || x.id_rol_catalogo != idExcluir.Value));
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error ExisteModuloEnRol: {0}", ex);
                return false;
            }
        }

        public bool GuardarModulosPorRol(int idRol, List<int> idsModulos)
        {
            try
            {
                // 1. Eliminar todas las asignaciones actuales del rol
                Eliminar(idRol);

                // 2. Insertar cada módulo asignado
                foreach (int idModulo in idsModulos)
                {
                    var item = new RolModulo
                    {
                        id_rol = idRol,
                        id_modulo_catalogo = idModulo,
                    };

                    bool creado = CrearRolModuloPorIdRol(item);
                    if (!creado)
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error GuardarModulosPorRol: {0}", ex);
                return false;
            }
        }
    }
}