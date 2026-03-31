using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DesarrollosQAS.Pages
{
    public partial class RolUsuarios : System.Web.UI.Page
    {
        private int IdUsuario
        {
            get
            {
                if (ViewState["IdUsuario"] != null)
                    return Convert.ToInt32(ViewState["IdUsuario"]);
                return 0;
            }
            set { ViewState["IdUsuario"] = value; }
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
            if (!string.IsNullOrEmpty(Request.QueryString["idUsuario"]))
            {
                IdUsuario = Convert.ToInt32(Request.QueryString["idUsuario"]);
                var repo = new RolUsuariosRepository();
                List<RolUsuario> listaDisponibles = repo.ObtenerRolesDisponiblesPorUsuario(IdUsuario);
                lbRolesDisponibles.DataSource = listaDisponibles;
                List<RolUsuario> listaAsignados = repo.ObtenerRolesAsignadosPorUsuario(IdUsuario);
                lbRolesAsignados.DataSource = listaAsignados;
                lbRolesDisponibles.DataBind();
                lbRolesAsignados.DataBind();
            }
            else
            {
                Response.Redirect("Usuarios.aspx");
                return;
            }
        }

        protected void cbGuardar_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            List<int> idsRoles = new List<int>();
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                idsRoles = e.Parameter
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => Convert.ToInt32(id.Trim()))
                    .ToList();
            }
            int asignado_por = Model.AuthHelper.GetCurrentUserId();
            var repo = new RolUsuariosRepository();
            bool resultado = repo.AsignarRolesPorUsuario(IdUsuario, idsRoles, asignado_por);
            e.Result = resultado ? "OK" : "ERROR";
        }

        protected void cbRefrescar_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            try {
                var repo = new RolUsuariosRepository();
                List<RolUsuario> listaDisponibles = repo.ObtenerRolesDisponiblesPorUsuario(IdUsuario);
                lbRolesDisponibles.DataSource = listaDisponibles;
                List<RolUsuario> listaAsignados = repo.ObtenerRolesAsignadosPorUsuario(IdUsuario);
                lbRolesAsignados.DataSource = listaAsignados;
                lbRolesDisponibles.DataBind();
                lbRolesAsignados.DataBind();
            }
            catch (Exception ex) {
                System.Diagnostics.Trace.TraceError("Error en cbRefrescar_Callback: {0}", ex);
                e.Result = "{}";
            }
        }
    }
}