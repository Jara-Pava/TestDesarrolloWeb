using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDesarrollos.Interfaces
{
    public interface ISolicitudRHRepository
    {
        List<SolicitudRH> ObtenerTodasSolicitudesRH();
        List<SolicitudRH> ObtenerSolicitudesRHPorUsuario(int empleadoId);
        // Obtener una solicitud por su ID
        SolicitudRH ObtenerSolicitudRHPorId(int id);
        bool CrearSolicitudRH(SolicitudRH solicitud);
        bool ActualizarSolicitudRH(SolicitudRH solicitud);
        void EliminarSolicitudRH(int id);
        bool AprobarSolicitudRH(int idSolicitud, bool aprobado);

        // Catálogo de tipos de solicitud
        List<TipoVisitante> ObtenerTiposSolicitud();
        List<Proyecto> ObtenerProyectos();
        List<Planta> ObtenerPlantas();
        List<EmpresaContratista> ObtenerContratistas();
    }
}
