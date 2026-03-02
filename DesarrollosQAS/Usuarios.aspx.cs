using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using DevExpress.Web.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
                    MostrarMensajeExito($"El usuario '{usuario.nombre}' fue creado exitosamente.");
                    DataBind();
                }
                else
                {
                    MostrarMensajeError("No se pudo crear el usuario.");
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeError($"Error: {ex.Message}");
            }
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
        protected void gridUsuarios_DataBinding(object sender, EventArgs e)
        {
            var repo = new UsuarioSistemaRepository();
            gridUsuarios.DataSource = repo.ObtenerTodosUsuarios();
        }


        protected void gridUsuarios_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            try
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
                {
                    throw new ApplicationException("No se pudo crear el usuario.");
                }

                e.Cancel = true;
                gridUsuarios.CancelEdit();
            }
            catch (Exception ex)
            {
                MostrarMensajeError($"Error al insertar: {ex.Message}");
                e.Cancel = true;
            }
        }

        protected void gridUsuarios_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            try
            {
                var u = new Usuario
                {
                    id_usuario = Convert.ToInt32(e.Keys["id_usuario"]),
                    nombre = Convert.ToString(e.NewValues["nombre"]),
                    sigla_red = Convert.ToString(e.NewValues["sigla_red"]),
                    activo = e.NewValues["activo"] != null && (bool)e.NewValues["activo"],
                    Email = Convert.ToString(e.NewValues["Email"])
                };

                var repo = new UsuarioSistemaRepository();
                if (!repo.ActualizarUsuario(u))
                {
                }

                e.Cancel = true;
                gridUsuarios.CancelEdit();
                    throw new ApplicationException("No se pudo actualizar el usuario.");
            }
            catch (Exception ex)
            {
                MostrarMensajeError($"Error al actualizar: {ex.Message}");
                e.Cancel = true;
            }
        }

        protected void gridUsuarios_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(e.Keys["id_usuario"]);
                var repo = new UsuarioSistemaRepository();

                if (!repo.EliminarUsuario(id))
                {
                    throw new ApplicationException("No se pudo eliminar el usuario.");
                }
                DataBind();
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                MostrarMensajeError($"Error al eliminar: {ex.Message}");
                e.Cancel = true;
            }
        }
    }
}