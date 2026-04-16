using System;
using System.Web.UI;

namespace DesarrollosQAS
{
    public partial class SinAcceso : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblUsuario.Text = Environment.UserName;
        }
    }
}