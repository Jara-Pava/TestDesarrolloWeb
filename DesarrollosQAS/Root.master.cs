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

            string currentPage = System.IO.Path.GetFileName(Request.Path);
            bool esPaginaSinAcceso = currentPage.Equals("SinAcceso.aspx", StringComparison.OrdinalIgnoreCase);

            // Autenticar al usuario de Windows automáticamente (solo una vez por sesión)
            if (!AuthHelper.IsAuthenticated())
            {
                string windowsUser = Environment.UserName;

                if (!AuthHelper.SignIn(windowsUser, null))
                {
                    // Si ya estamos en SinAcceso.aspx, no redirigir (evitar loop infinito)
                    if (!esPaginaSinAcceso)
                    {
                        if (Page.IsCallback)
                        {
                            ASPxWebControl.RedirectOnCallback("~/Pages/SinAcceso.aspx");
                        }
                        else
                        {
                            Response.Redirect("~/Pages/SinAcceso.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                    }
                    return;
                }
            }

            // Refrescar permisos en cada request para reflejar cambios de roles en tiempo real
            AuthHelper.RefrescarPermisos();

            // Verificar permisos de acceso a la página actual (no aplica para SinAcceso)
            if (!esPaginaSinAcceso)
            {
                ValidarAccesoPagina();
            }

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
            int? idModulo = PageModuleMap.GetIdModulo(currentPage);

            if (!idModulo.HasValue)
                return;

            if (!AuthHelper.TienePermisoVer(idModulo.Value))
            {
                if (Page.IsCallback)
                {
                    ASPxWebControl.RedirectOnCallback("~/Pages/SinAcceso.aspx");
                }
                else
                {
                    Response.Redirect("~/Pages/SinAcceso.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
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