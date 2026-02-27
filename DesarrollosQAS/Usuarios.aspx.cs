using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using DevExpress.Web.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.UI;

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

        protected void gridUsuarios_DataBinding(object sender, EventArgs e)
        {
            var repo = new UsuarioSistemaRepository();
            gridUsuarios.DataSource = repo.ObtenerTodosUsuarios();
        }

        protected void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                var usuario = new Usuario
                {
                    nombre = tbNombre.Text?.Trim(),
                    sigla_red = tbSigla.Text?.Trim(),
                    Email = tbEmail.Text?.Trim(),
                    activo = chbActivo.Checked
                };

                if (string.IsNullOrWhiteSpace(usuario.nombre) ||
                    string.IsNullOrWhiteSpace(usuario.sigla_red) ||
                    string.IsNullOrWhiteSpace(usuario.Email))
                {
                    MostrarMensajeError("Por favor, completa todos los campos requeridos.");
                    return;
                }

                var repo = new UsuarioSistemaRepository();
                bool ok = repo.CrearUsuario(usuario);

                if (ok)
                {
                    LimpiarFormulario();
                    BindGrid();
                    MostrarMensajeExito($"El usuario '{usuario.nombre}' fue creado exitosamente.");
                }
                else
                {
                    MostrarMensajeError("No se pudo crear el usuario. Por favor, verifica que no exista un usuario con la misma sigla de red o email.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al crear usuario: {0}", ex);
                MostrarMensajeError($"Ocurrió un error al crear el usuario: {ex.Message}");
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            tbNombre.Text = string.Empty;
            tbSigla.Text = string.Empty;
            tbEmail.Text = string.Empty;
            chbActivo.Checked = true;
        }

        private void MostrarMensajeExito(string mensaje)
        {
            lblMensajeExito.Text = mensaje;
            pcCrearUsuario.ShowOnPageLoad = false;
            pcMensajeExito.ShowOnPageLoad = true;
        }

        private void MostrarMensajeError(string mensaje)
        {
            lblMensajeError.Text = mensaje;
            pcMensajeError.ShowOnPageLoad = true;
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