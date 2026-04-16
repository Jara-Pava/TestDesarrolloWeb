using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDesarrollos.Interfaces
{
    public interface IRolesUsuarios
    {
        // Obtenemos todos los roles disponibles por usuario
        List<RolUsuario> ObtenerRolesDisponiblesPorUsuario(int id);
        // Obtenemos un rol por su ID
        List<RolUsuario> ObtenerRolesAsignadosPorUsuario(int id);

        // Asignamos un rol a un usuario
        bool AsignarRolPorUsuario(RolUsuario rol);

        // Eliminamos los roles asignados a un usuario
        void EliminarAsignacionesRolesPorUsuario(int id);

        // Eliminamos los roles asignados a un usuario por id_rol
        void EliminarAsignacionesRolesPorIdRol(int id);

        // Asignar roles a un usuario
        bool AsignarRolesPorUsuario(int idUsuario, List<int> idsRoles, int asignado_por);
    }
}
