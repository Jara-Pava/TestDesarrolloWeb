using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using DesarrollosQAS.Model;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace DesarrollosQAS.Pages
{
    public partial class RolModulos : System.Web.UI.Page
    {
        private int IdRol
        {
            get
            {
                if (ViewState["IdRol"] != null)
                    return Convert.ToInt32(ViewState["IdRol"]);
                return 0;
            }
            set { ViewState["IdRol"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarListas();
            }
        }

        private void CargarListas()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["idRol"]))
            {
                IdRol = Convert.ToInt32(Request.QueryString["idRol"]);
                Rol rol = new RolesRepository().ObtenerRolPorId(IdRol);
                lblNombreRol.Text = "Rol "+rol.nombre;
                var repo = new RolModulosRepository();

                List<RolModulo> listaDisponibles = repo.ObtenerModulosDisponiblesPorRol(IdRol);
                lbModulosDisponibles.DataSource = listaDisponibles;

                List<RolModulo> listaAsignados = repo.ObtenerModulosAsignadosPorIdRol(IdRol);
                lbModulosAsignados.DataSource = listaAsignados;

                lbModulosDisponibles.DataBind();
                lbModulosAsignados.DataBind();
            }
            else
            {
                Response.Redirect("Roles.aspx");
                return;
            }
        }

        protected void cbGuardar_Callback(object source, CallbackEventArgs e)
        {
            try
            {
                // Parsear los IDs de módulos enviados desde el cliente
                List<int> idsModulos = new List<int>();
                if (!string.IsNullOrEmpty(e.Parameter))
                {
                    idsModulos = e.Parameter
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => Convert.ToInt32(id.Trim()))
                        .ToList();
                }

                // Guardar usando el repositorio
                var repo = new RolModulosRepository();
                bool resultado = repo.GuardarModulosPorRol(IdRol, idsModulos);

                e.Result = resultado ? "OK" : "ERROR";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en cbGuardar_Callback: {0}", ex);
                e.Result = "ERROR";
            }
        }

        protected void cbRefrescar_Callback(object source, CallbackEventArgs e)
        {
            try
            {
                var repo = new RolModulosRepository();

                List<RolModulo> disponibles = repo.ObtenerModulosDisponiblesPorRol(IdRol);
                List<RolModulo> asignados = repo.ObtenerModulosAsignadosPorIdRol(IdRol);

                // Serializar ambas listas como JSON para que el cliente reconstruya los ListBox
                var data = new
                {
                    disponibles = disponibles.Select(m => new { text = m.nombre_catalogo, value = m.id_modulo_catalogo }),
                    asignados = asignados.Select(m => new { text = m.nombre_catalogo, value = m.id_modulo_catalogo })
                };

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                e.Result = serializer.Serialize(data);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en cbRefrescar_Callback: {0}", ex);
                e.Result = "{}";
            }
        }
    }
}