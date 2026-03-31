using DesarrollosQAS.Model;
using DevExpress.Web;
using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace DesarrollosQAS
{
    public partial class Root : MasterPage
    {
        public bool EnableBackButton { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string language = "es-MX";
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

            // Autenticar al usuario de Windows automáticamente (solo una vez por sesión)
            if (!AuthHelper.IsAuthenticated())
            {
                string windowsUser = Environment.UserName;

                if (!AuthHelper.SignIn(windowsUser, null))
                {
                    // El usuario de Windows no existe en SEC_Usuarios, redirigir
                    Response.Redirect("Pages/SinAcceso.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
            }

            // Verificar permisos de acceso a la página actual
            ValidarAccesoPagina();

            // Mostrar el nombre del usuario autenticado
            var user = AuthHelper.GetLoggedInUserInfo();
            if (user != null)
            {
                lblUsuario.Text = user.Sigla_red;
            }
            else
            {
                lblUsuario.Text = Environment.UserName;
            }

            int collapseAtWindowInnerWidth = 1200;
            NavigationPanel.SettingsAdaptivity.CollapseAtWindowInnerWidth = collapseAtWindowInnerWidth;
            NavigationPanel.JSProperties["cpCollapseAtWindowInnerWidth"] = collapseAtWindowInnerWidth;
        }

        /// <summary>
        /// Verifica si el usuario tiene permiso de ver la página actual usando id_modulo_catalogo.
        /// Si la página no está mapeada, se permite el acceso (páginas sin restricción).
        /// </summary>
        private void ValidarAccesoPagina()
        {
            string currentPage = System.IO.Path.GetFileName(Request.Path);

            // Obtener el id_modulo_catalogo asociado a esta página
            int? idModulo = PageModuleMap.GetIdModulo(currentPage);

            // Si la página no está mapeada, no requiere permisos (ej: SinAcceso.aspx, Home.aspx)
            if (!idModulo.HasValue)
                return;

            // Verificar si el usuario tiene permiso de ver este módulo por ID
            if (!AuthHelper.TienePermisoVer(idModulo.Value))
            {
                Response.Redirect("~/Pages/SinAcceso.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Registrar Site.css al final del <head> para que cargue después de los estilos de DevExpress
            var link = new HtmlLink();
            link.Href = ResolveUrl("~/Content/Site.css");
            link.Attributes.Add("rel", "stylesheet");
            link.Attributes.Add("type", "text/css");
            Page.Header.Controls.Add(link);
        }
    }
}