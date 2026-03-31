using DesarrollosQAS.Model;
using System;

namespace DesarrollosQAS
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var user = AuthHelper.GetLoggedInUserInfo();
                if (user != null)
                {
                    lblNombreUsuario.Text = user.Nombre;
                }

                lblFecha.Text = DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy");
            }
        }
    }
}