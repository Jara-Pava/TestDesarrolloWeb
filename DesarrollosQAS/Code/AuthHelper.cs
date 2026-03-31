using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesarrollosQAS.Model
{
    public class ApplicationUser
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Sigla_red { get; set; }
        public bool Activo { get; set; }
        public string Email { get; set; }
        public string Creado_por { get; set; }
        public string Modificado_por { get; set; }
    }

    public static class AuthHelper
    {
        /// <summary>
        /// Autentica al usuario buscándolo en SEC_Usuarios por sigla_red
        /// </summary>
        public static bool SignIn(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return false;

            try
            {
                var repo = new UsuarioSistemaRepository();
                var usuarios = repo.ObtenerTodosUsuarios();
                var usuario = usuarios.FirstOrDefault(u =>
                    u.sigla_red != null &&
                    u.sigla_red.Trim().Equals(userName.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    u.activo);

                if (usuario == null)
                    return false;

                if (!usuario.activo)
                    return false;
                // Guardar el usuario autenticado en sesión
                HttpContext.Current.Session["User"] = new ApplicationUser
                {
                    IdUsuario = usuario.id_usuario,
                    Nombre = usuario.nombre,
                    Sigla_red = usuario.sigla_red,
                    Email = usuario.Email,
                    Activo = usuario.activo,
                    Creado_por = usuario.creado_por,
                    Modificado_por = usuario.modificado_por
                };

                // Cargar los permisos del usuario en sesión
                CargarPermisos(usuario.id_usuario);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en SignIn: {0}", ex);
                return false;
            }
        }

        public static void SignOut()
        {
            HttpContext.Current.Session["User"] = null;
            HttpContext.Current.Session["Permisos"] = null;
        }

        public static bool IsAuthenticated()
        {
            return GetLoggedInUserInfo() != null;
        }

        public static ApplicationUser GetLoggedInUserInfo()
        {
            return HttpContext.Current.Session["User"] as ApplicationUser;
        }

        /// <summary>
        /// Obtiene el ID del usuario logueado actualmente
        /// </summary>
        public static int GetCurrentUserId()
        {
            var user = GetLoggedInUserInfo();
            return user?.IdUsuario ?? 0;
        }

        // ===================== PERMISOS =====================

        /// <summary>
        /// Carga los permisos consolidados del usuario en la sesión.
        /// </summary>
        private static void CargarPermisos(int idUsuario)
        {
            try
            {
                var repo = new PermisosRepository();
                var permisos = repo.ObtenerPermisosPorUsuario(idUsuario);
                HttpContext.Current.Session["Permisos"] = permisos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al cargar permisos: {0}", ex);
                HttpContext.Current.Session["Permisos"] = new List<PermisoModulo>();
            }
        }

        /// <summary>
        /// Obtiene la lista de permisos del usuario desde la sesión.
        /// </summary>
        public static List<PermisoModulo> GetPermisos()
        {
            return HttpContext.Current.Session["Permisos"] as List<PermisoModulo>
                ?? new List<PermisoModulo>();
        }

        /// <summary>
        /// Verifica si el usuario tiene permiso de ver un módulo por su id_modulo_catalogo.
        /// </summary>
        public static bool TienePermisoVer(int idModuloCatalogo)
        {
            var permisos = GetPermisos();
            var permiso = permisos.FirstOrDefault(p => p.id_modulo_catalogo == idModuloCatalogo);
            return permiso != null && permiso.puede_ver;
        }

        /// <summary>
        /// Obtiene el permiso completo de un módulo por su id_modulo_catalogo.
        /// Retorna null si no tiene el módulo asignado.
        /// </summary>
        public static PermisoModulo GetPermiso(int idModuloCatalogo)
        {
            var permisos = GetPermisos();
            return permisos.FirstOrDefault(p => p.id_modulo_catalogo == idModuloCatalogo);
        }

        /// <summary>
        /// Fuerza la recarga de permisos (útil después de cambiar roles).
        /// </summary>
        public static void RefrescarPermisos()
        {
            var user = GetLoggedInUserInfo();
            if (user != null)
                CargarPermisos(user.IdUsuario);
        }
    }
}