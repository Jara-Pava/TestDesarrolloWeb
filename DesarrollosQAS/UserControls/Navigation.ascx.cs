using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using DesarrollosQAS.Model;
using DevExpress.Web;

namespace DesarrollosQAS.UserControls
{
    public partial class Navigation : System.Web.UI.UserControl
    {

        protected void NavigationTreeView_NodeDataBound(object source, DevExpress.Web.TreeViewNodeEventArgs e)
        {
            XmlNode dataNode = ((e.Node.DataItem as IHierarchyData).Item as XmlNode);

            if (dataNode.Name == "group")
                e.Node.NodeStyle.CssClass += " group";
            if (dataNode.ParentNode != null && dataNode.ParentNode.Name != "group")
                e.Node.NodeStyle.CssClass += " introPage";

            // Ocultar páginas a las que el usuario no tiene permiso de ver (por ID)
            if (dataNode.Name == "page")
            {
                var attrIdModulo = dataNode.Attributes["IdModulo"];
                if (attrIdModulo != null)
                {
                    int idModulo;
                    if (int.TryParse(attrIdModulo.Value, out idModulo))
                    {
                        if (!AuthHelper.TienePermisoVer(idModulo))
                        {
                            e.Node.Visible = false;
                        }
                    }
                }
            }
        }

        protected void NavigationTreeView_PreRender(object sender, EventArgs e)
        {
            // Ocultar grupos vacíos (todos sus hijos fueron ocultados)
            OcultarGruposVacios(NavigationTreeView.Nodes);

            TreeViewNode node = NavigationTreeView.SelectedNode;
            if (node != null)
            {
                NavigationTreeView.ExpandToNode(node);
                node.Expanded = true;
                while (node != null)
                {
                    if (node.Parent != null && node.Parent.Parent == null)
                        node.Expanded = false;
                    node = node.Parent;
                }
            }
        }

        /// <summary>
        /// Oculta los nodos de grupo que no tienen hijos visibles.
        /// </summary>
        private void OcultarGruposVacios(TreeViewNodeCollection nodes)
        {
            foreach (TreeViewNode node in nodes)
            {
                if (node.Nodes.Count > 0)
                {
                    OcultarGruposVacios(node.Nodes);

                    bool tieneHijosVisibles = false;
                    foreach (TreeViewNode child in node.Nodes)
                    {
                        if (child.Visible)
                        {
                            tieneHijosVisibles = true;
                            break;
                        }
                    }

                    if (!tieneHijosVisibles)
                        node.Visible = false;
                }
            }
        }
    }
}