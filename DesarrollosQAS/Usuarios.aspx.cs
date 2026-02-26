using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using DevExpress.Web.Data;
using System;
using System.Collections.Generic;

namespace DesarrollosQAS
{
    public partial class Usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            var repo = new UsuarioSistemaRepository();
            List<Usuario> usuarios = repo.ObtenerTodosUsuarios();
            gridUsuarios.DataSource = usuarios;
            gridUsuarios.DataBind();
        }

        public void CrearUsuario(Usuario usuario)
        {
            if (usuario == null) return;

            var repo = new UsuarioSistemaRepository();
            bool ok = repo.CrearUsuario(usuario);
            if (ok)
            {
                BindGrid();
            }
            else
            {
                throw new ApplicationException("No se pudo crear el usuario.");
            }
        }

        protected void gridUsuarios_DataBinding(object sender, EventArgs e)
        {
            var repo = new DataAccessDesarrollos.Repositorios.UsuarioSistemaRepository();
            gridUsuarios.DataSource = repo.ObtenerTodosUsuarios();
            // NO llames DataBind() aquí; el grid lo hace durante DataBinding
        }

        protected void btnGuardarNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                var u = new Usuario
                {
                    nombre = txtNombre.Text?.Trim(),
                    sigla_red = txtSigla.Text?.Trim(),
                    activo = chkActivo.Checked,
                    Email = txtEmail.Text?.Trim()
                };

                CrearUsuario(u);

                // Limpia el form y recarga grid
                txtNombre.Text = string.Empty;
                txtSigla.Text = string.Empty;
                txtEmail.Text = string.Empty;
                chkActivo.Checked = true;

                lblMensaje.ForeColor = System.Drawing.Color.Green;
                lblMensaje.Text = "Usuario creado correctamente.";

                BindGrid();
            }
            catch (Exception ex)
            {
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                lblMensaje.Text = ex.Message;
            }
        }


        protected void gridUsuarios_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            var u = new Usuario
            {
                nombre = Convert.ToString(e.NewValues["nombre"]),
                sigla_red = Convert.ToString(e.NewValues["sigla_red"]),
                activo = e.NewValues["activo"] != null && (bool)e.NewValues["activo"],
                Email = Convert.ToString(e.NewValues["Email"])
            };

            var repo = new UsuarioSistemaRepository();
            if (!repo.CrearUsuario(u))
                throw new ApplicationException("No se pudo crear el usuario.");

            e.Cancel = true;
            gridUsuarios.CancelEdit();
            BindGrid();
        }
    }
}