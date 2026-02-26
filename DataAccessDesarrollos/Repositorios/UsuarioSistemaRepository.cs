using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using DataAccessDesarrollos.Interfaces;

namespace DataAccessDesarrollos.Repositorios
{
    public class UsuarioSistemaRepository : IUsuarioSistema
    {
        // Logical code defined in QASConfig section of web.config
        private const string SpCode_GetAll = "USUARIOS_GETALL";

        public List<Usuario> ObtenerTodosUsuarios()
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var lista = da.ExecuteReaderByCode(SpCode_GetAll, rdr => new Usuario
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
    }
}