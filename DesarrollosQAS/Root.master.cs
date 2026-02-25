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

            lblUsuario.Text = Environment.UserName;

            int collapseAtWindowInnerWidth = 1200;
            NavigationPanel.SettingsAdaptivity.CollapseAtWindowInnerWidth = collapseAtWindowInnerWidth;
            NavigationPanel.JSProperties["cpCollapseAtWindowInnerWidth"] = collapseAtWindowInnerWidth;

        }

    }
}