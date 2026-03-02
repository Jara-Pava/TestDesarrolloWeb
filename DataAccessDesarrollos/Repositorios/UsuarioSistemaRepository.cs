using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using DataAccessDesarrollos.Interfaces;

namespace DataAccessDesarrollos.Repositorios
{
    public class UsuarioSistemaRepository : IUsuarioSistema
    {
        private const string SpCode_GetAll = "USUARIOS_GETALL";
        private const string SpCode_Insert = "USUARIOS_INSERT";
        private const string SpCode_Update = "USUARIOS_UPDATE";
        private const string SpCode_Delete = "USUARIOS_DELETE";

        public List<Usuario> ObtenerTodosUsuarios()
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var lista = da.ExecuteReaderByCode("usp_GetUsers", rdr => new Usuario
                    {
                        id_usuario = rdr["id_usuario"] != DBNull.Value ? Convert.ToInt32(rdr["id_usuario"]) : 0,
                        nombre = rdr["nombre"] != DBNull.Value ? rdr["nombre"].ToString() : string.Empty,
                        sigla_red = rdr["sigla_red"] != DBNull.Value ? rdr["sigla_red"].ToString() : string.Empty,
                        activo = rdr["activo"] != DBNull.Value && Convert.ToBoolean(rdr["activo"]),
                        Email = rdr["Email"] != DBNull.Value ? rdr["Email"].ToString() : string.Empty
                    });

                    return lista ?? new List<Usuario>();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error ObtenerTodosUsuarios: {0}", ex);
                return new List<Usuario>();
            }
        }

        public bool CrearUsuario(Usuario usuario)
        {
            if (usuario == null) return false;

            try
            {
                using (var da = new DataAccess())
                {
                    int rows = da.ExecuteNonQueryByCode("usp_InsertUsers", cmd =>
                    {
                        cmd.Parameters.AddWithValue("@nombre", usuario.nombre ?? string.Empty);
                        cmd.Parameters.AddWithValue("@sigla_red", usuario.sigla_red ?? string.Empty);
                        cmd.Parameters.AddWithValue("@activo", usuario.activo);
                        cmd.Parameters.AddWithValue("@Email", usuario.Email ?? string.Empty);
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error CrearUsuario: {0}", ex);
                return false;
            }
        }

        public bool ActualizarUsuario(Usuario usuario)
        {
            if (usuario == null || usuario.id_usuario <= 0) return false;

            try
            {
                using (var da = new DataAccess())
                {
                    int rows = da.ExecuteNonQueryByCode("usp_UpdateUsers", cmd =>
                    {
                        cmd.Parameters.AddWithValue("@id_usuario", usuario.id_usuario);
                        cmd.Parameters.AddWithValue("@nombre", usuario.nombre ?? string.Empty);
                        cmd.Parameters.AddWithValue("@sigla_red", usuario.sigla_red ?? string.Empty);
                        cmd.Parameters.AddWithValue("@activo", usuario.activo);
                        cmd.Parameters.AddWithValue("@Email", usuario.Email ?? string.Empty);
                    });

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error ActualizarUsuario: {0}", ex);
                return false;
            }
        }

        public bool EliminarUsuario(int id)
        {
            if (id <= 0) return false;

            try
            {
                using (var da = new DataAccess())
                {
                    int rows = da.ExecuteNonQueryByCode("usp_DeleteUsers", cmd =>
                    {
                        cmd.Parameters.AddWithValue("@id_usuario", id);
                    });

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error EliminarUsuario: {0}", ex);
                return false;
            }
        }
    }
}