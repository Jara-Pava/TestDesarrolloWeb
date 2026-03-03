using DataAccessDesarrollos.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDesarrollos.Repositorios
{
    public class SolicitudRHRepository : ISolicitudRHRepository
    {
        public bool ActualizarSolicitudRH(SolicitudRH solicitud)
        {
            throw new NotImplementedException();
        }

        public bool AprobarSolicitudRH(int idSolicitud, bool aprobado)
        {
            throw new NotImplementedException();
        }

        public bool CrearSolicitudRH(SolicitudRH solicitud)
        {
            throw new NotImplementedException();
        }

        public void EliminarSolicitudRH(int id)
        {
            throw new NotImplementedException();
        }

        public List<EmpresaContratista> ObtenerContratistas()
        {
            throw new NotImplementedException();
        }

        public List<Planta> ObtenerPlantas()
        {
            throw new NotImplementedException();
        }

        public List<Proyecto> ObtenerProyectos()
        {
            throw new NotImplementedException();
        }

        public List<SolicitudRH> ObtenerSolicitudesRHPorUsuario(int empleadoId)
        {
            throw new NotImplementedException();
        }

        public List<TipoVisitante> ObtenerTiposSolicitud()
        {
            throw new NotImplementedException();
        }

        public List<SolicitudRH> ObtenerTodasSolicitudesRH()
        {
            try {
                using (var da = new DataAccess())
                {
                    var lista = da.ExecuteReaderByCode("rhsp_GetAllSolicitudes", rdr => new SolicitudRH {
                        ID_Solicitud = rdr["ID_Solicitud"] != DBNull.Value ? Convert.ToInt32(rdr["ID_Solicitud"]):0,
                        id_TipoSolicitud = rdr["id_TipoSolicitud"] != DBNull.Value ? Convert.ToInt32(rdr["id_TipoSolicitud"]):0,
                        id_Solicitante = rdr["id_Solicitante"] != DBNull.Value ? Convert.ToInt32(rdr["id_Solicitante"]):0,
                        id_Proyecto = rdr["id_Proyecto"] != DBNull.Value ? Convert.ToInt32(rdr["id_Proyecto"]):0,
                        Visitante = rdr["Visitante"] != DBNull.Value ? rdr["Visitante"].ToString() : string.Empty,
                        FechaInicio = rdr["FechaInicio"] != DBNull.Value ? Convert.ToDateTime(rdr["FechaInicio"]) : default(DateTime),
                        FechaFin = rdr["FechaFin"] != DBNull.Value ? Convert.ToDateTime(rdr["FechaFin"]) : default(DateTime),
                        RFC = rdr["RFC"] != DBNull.Value ? rdr["RFC"].ToString() : string.Empty,
                        id_Contratista = rdr["id_Contratista"] != DBNull.Value ? Convert.ToInt32(rdr["id_Contratista"]):0,
                        Responsable = rdr["Responsable"] != DBNull.Value ? rdr["Responsable"].ToString() : string.Empty,
                        AreaTrabajo = rdr["AreaTrabajo"] != DBNull.Value ? rdr["AreaTrabajo"].ToString() : string.Empty,
                        Actividad = rdr["Actividad"] != DBNull.Value ? rdr["Actividad"].ToString() : string.Empty,
                        Estancia = rdr["Estancia"] != DBNull.Value ? rdr["Estancia"].ToString() : string.Empty,
                        FechaSolicitud = rdr["FechaSolicitud"] != DBNull.Value ? Convert.ToDateTime(rdr["FechaSolicitud"]) : default(DateTime),
                        aprobado = rdr["aprobado"] != DBNull.Value ? Convert.ToBoolean(rdr["aprobado"]) : false

                    });
                    return lista ?? new List<SolicitudRH>();
                }
            }
            catch(Exception ex)
            {
                Trace.TraceError("Error ObtenerTodasSolicitudes: {0}", ex);
                return new List<SolicitudRH>();
            }
        }
    }
}
