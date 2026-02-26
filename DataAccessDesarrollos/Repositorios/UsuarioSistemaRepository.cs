using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using DataAccessDesarrollos.Interfaces;

namespace DataAccessDesarrollos.Repositorios
{
    public class UsuarioSistemaRepository : IUsuarioSistema
    {
        /// <summary>
        /// Obtener todos los usuarios de la base de datos.
        /// </summary>
        /// <returns>A lista de objetos Usuario. Returns an empty list if no users are found or if an error occurs.</returns>
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

        /// <summary>
        /// Crear un nuevo usuario en la base de datos.
        /// </summary>
        /// <returns>
        /// a boolean value indicating whether the user was created successfully. Returns false if an error occurs during the creation process.
        /// </returns>
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
            catch (SqlException ex)
            {
                Trace.TraceError("Error Crear Usuario: {0}", ex);
                return false;
            }
        }
    }
}