using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using DesarrollosQAS.Model;
using DevExpress.Web;
using DevExpress.Web.Data;
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
                // Obtener el IdRol del query string y cargar las listas de módulos disponibles y asignados
                IdRol = Convert.ToInt32(Request.QueryString["idRol"]);

                // Cargar los módulos disponibles y asignados para el rol
                List<RolModulo> listaDisponibles = new RolModulosRepository().ObtenerModulosDisponiblesPorRol(IdRol);
                lbModulosDisponibles.DataSource = listaDisponibles;

                // Cargar los módulos asignados para el rol
                List<RolModulo> listaAsignados = new RolModulosRepository().ObtenerModulosAsignadosPorIdRol(IdRol);
                lbModulosAsignados.DataSource = listaAsignados;

                // Enlazar los datos a los controles ListBox
                lbModulosDisponibles.DataBind();
                lbModulosAsignados.DataBind();
            }
            else
            {
                // Redirigir a la página de roles si no se proporciona un IdRol válido
                Response.Redirect("Roles.aspx");
                return;
            }
        }
    }
}