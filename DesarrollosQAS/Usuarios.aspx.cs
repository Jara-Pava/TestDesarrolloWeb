using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using DevExpress.Web.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.UI;

namespace DesarrollosQAS
{
    public partial class Usuarios : System.Web.UI.Page
    {
        // Claves de sesión para mensajes
        private const string SESSION_SUCCESS_MESSAGE = "UsuarioSuccessMsg";
        private const string SESSION_ERROR_MESSAGE = "UsuarioErrorMsg";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                pcCrearUsuario.ShowOnPageLoad = false;
                pcMensajeExito.ShowOnPageLoad = false;
                pcMensajeError.ShowOnPageLoad = false;
                MostrarMensajesDeSesion();
            }
        }

        private void MostrarMensajesDeSesion()
        {
            if (Session[SESSION_SUCCESS_MESSAGE] != null)
            {
                string mensaje = Session[SESSION_SUCCESS_MESSAGE].ToString();
                Session.Remove(SESSION_SUCCESS_MESSAGE);
                MostrarMensajeExito(mensaje);
            }
            else if (Session[SESSION_ERROR_MESSAGE] != null)
            {
                string mensaje = Session[SESSION_ERROR_MESSAGE].ToString();
                Session.Remove(SESSION_ERROR_MESSAGE);
                MostrarMensajeError(mensaje);
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
                    // ✅ TAMBIÉN REDIRIGIR EN CASO DE ERROR DE VALIDACIÓN
                    Session[SESSION_ERROR_MESSAGE] = "Por favor, completa todos los campos requeridos.";
                    RedirectConJavaScript("Usuarios.aspx");
                    return;
                }

                var repo = new UsuarioSistemaRepository();
                bool ok = repo.CrearUsuario(usuario);

                if (ok)
                {
                    // Guardar mensaje de éxito en sesión
                    Session[SESSION_SUCCESS_MESSAGE] = $"El usuario '{usuario.nombre}' fue creado exitosamente.";
                    RedirectConJavaScript("Usuarios.aspx");
                }
                else
                {
                    Session[SESSION_ERROR_MESSAGE] = "No se pudo crear el usuario. Verifica que no exista un usuario con la misma sigla de red o email.";
                    RedirectConJavaScript("Usuarios.aspx");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al crear usuario: {0}", ex);
                Session[SESSION_ERROR_MESSAGE] = $"Error: {ex.Message}";
                RedirectConJavaScript("Usuarios.aspx");
            }
        }

        private void RedirectConJavaScript(string url)
        {
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "redirect",
                $"window.location.href = '{url}';",
                true
            );
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
                Session[SESSION_SUCCESS_MESSAGE] = $"Usuario '{u.nombre}' creado exitosamente.";
                RedirectConJavaScript("Usuarios.aspx");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en RowInserting: {0}", ex);
                e.Cancel = true;
                Session[SESSION_ERROR_MESSAGE] = $"Error al insertar: {ex.Message}";
                RedirectConJavaScript("Usuarios.aspx");
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
                    throw new ApplicationException("No se pudo actualizar el usuario.");
                }

                e.Cancel = true;
                gridUsuarios.CancelEdit();
                Session[SESSION_SUCCESS_MESSAGE] = $"Usuario '{u.nombre}' actualizado exitosamente.";
                RedirectConJavaScript("Usuarios.aspx");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al actualizar usuario: {0}", ex);
                e.Cancel = true;

                Session[SESSION_ERROR_MESSAGE] = $"Error al actualizar: {ex.Message}";
                RedirectConJavaScript("Usuarios.aspx");
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

                e.Cancel = true;
                Session[SESSION_SUCCESS_MESSAGE] = "Usuario eliminado exitosamente.";
                RedirectConJavaScript("Usuarios.aspx");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al eliminar usuario: {0}", ex);
                e.Cancel = true;

                Session[SESSION_ERROR_MESSAGE] = $"Error al eliminar: {ex.Message}";
                RedirectConJavaScript("Usuarios.aspx");
            }
        }
    }
}