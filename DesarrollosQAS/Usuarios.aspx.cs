using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using DevExpress.Web;
using System;
using System.Text.RegularExpressions;

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

            // Aplicar estilos al popup en cada carga
            gridUsuarios.StylesPopup.EditForm.Header.BackColor = System.Drawing.ColorTranslator.FromHtml("#353943");
            gridUsuarios.StylesPopup.EditForm.Header.ForeColor = System.Drawing.Color.White;
            gridUsuarios.StylesPopup.EditForm.Header.Font.Bold = true;
        }

        private void BindGrid()
        {
            var repo = new UsuarioSistemaRepository();
            gridUsuarios.DataSource = repo.ObtenerTodosUsuarios();
            gridUsuarios.DataBind();
        }

        protected void gridUsuarios_DataBinding(object sender, EventArgs e)
        {
            var repo = new UsuarioSistemaRepository();
            gridUsuarios.DataSource = repo.ObtenerTodosUsuarios();
        }

        protected void gridUsuarios_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            // Deshabilitar Sigla Red en modo edición (no en creación)
            if (e.Column.FieldName == "sigla_red" && gridUsuarios.IsEditing && !gridUsuarios.IsNewRowEditing)
            {
                ASPxTextBox editor = e.Editor as ASPxTextBox;
                if (editor != null)
                {
                    editor.ClientEnabled = false;
                    editor.ReadOnly = true;
                    editor.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                }
            }
        }

        protected void gridUsuarios_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                string nombre = e.NewValues["nombre"]?.ToString()?.Trim();
                string siglaRed = e.NewValues["sigla_red"]?.ToString()?.Trim();
                string email = e.NewValues["Email"]?.ToString()?.Trim();

                if (!ValidarNombre(nombre, out string errorNombre))
                {
                    e.Cancel = true;
                    gridUsuarios.CancelEdit();
                    MostrarError(errorNombre);
                    return;
                }

                if (!ValidarSigla(siglaRed, out string errorSigla))
                {
                    e.Cancel = true;
                    gridUsuarios.CancelEdit();
                    MostrarError(errorSigla);
                    return;
                }

                if (!ValidarEmail(email, out string errorEmail))
                {
                    e.Cancel = true;
                    gridUsuarios.CancelEdit();
                    MostrarError(errorEmail);
                    return;
                }

                var usuario = new Usuario
                {
                    nombre = nombre,
                    sigla_red = siglaRed,
                    Email = email,
                    activo = e.NewValues["activo"] != null && (bool)e.NewValues["activo"]
                };

                var repo = new UsuarioSistemaRepository();
                if (!repo.CrearUsuario(usuario))
                {
                    throw new ApplicationException("No se pudo crear el usuario.");
                }

                e.Cancel = true;
                gridUsuarios.CancelEdit();
                gridUsuarios.DataBind();
                MostrarExito("Usuario creado exitosamente.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en RowInserting: {0}", ex);
                e.Cancel = true;
                gridUsuarios.CancelEdit();
                MostrarError($"Error al insertar: {ex.Message}");
            }
        }

        protected void gridUsuarios_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                string nombre = e.NewValues["nombre"]?.ToString()?.Trim();
                string email = e.NewValues["Email"]?.ToString()?.Trim();

                if (!ValidarNombre(nombre, out string errorNombre))
                {
                    e.Cancel = true;
                    gridUsuarios.CancelEdit();
                    MostrarError(errorNombre);
                    return;
                }

                if (!ValidarEmail(email, out string errorEmail))
                {
                    e.Cancel = true;
                    gridUsuarios.CancelEdit();
                    MostrarError(errorEmail);
                    return;
                }

                string siglaRedOriginal = e.OldValues["sigla_red"]?.ToString();

                var usuario = new Usuario
                {
                    id_usuario = Convert.ToInt32(e.Keys["id_usuario"]),
                    nombre = nombre,
                    sigla_red = siglaRedOriginal,
                    Email = email,
                    activo = e.NewValues["activo"] != null && (bool)e.NewValues["activo"]
                };

                var repo = new UsuarioSistemaRepository();
                if (!repo.ActualizarUsuario(usuario))
                {
                    throw new ApplicationException("No se pudo actualizar el usuario.");
                }

                e.Cancel = true;
                gridUsuarios.CancelEdit();
                gridUsuarios.DataBind();
                MostrarExito("Usuario actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al actualizar usuario: {0}", ex);
                e.Cancel = true;
                gridUsuarios.CancelEdit();
                MostrarError($"Error al actualizar: {ex.Message}");
            }
        }

        protected void gridUsuarios_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(e.Keys["id_usuario"]);
                var repo = new UsuarioSistemaRepository();
                repo.EliminarUsuario(id);

                e.Cancel = true;
                gridUsuarios.CancelEdit();
                gridUsuarios.DataBind();
                MostrarExito("Usuario eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al eliminar usuario: {0}", ex);
                e.Cancel = true;
                gridUsuarios.CancelEdit();
                MostrarError($"Error al eliminar: {ex.Message}");
            }
        }

        #region Métodos de Validación

        private bool ValidarNombre(string nombre, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                mensajeError = "El nombre no puede estar vacío o contener solo espacios.";
                return false;
            }

            if (Regex.IsMatch(nombre, @"^[\W\d_]+$"))
            {
                mensajeError = "El nombre debe contener al menos letras válidas.";
                return false;
            }

            if (nombre.Trim().Length < 2)
            {
                mensajeError = "El nombre debe tener al menos 2 caracteres.";
                return false;
            }

            if (Regex.IsMatch(nombre, @"[<>""';={}()\[\]]"))
            {
                mensajeError = "El nombre contiene caracteres no permitidos.";
                return false;
            }

            if (nombre.Length > 200)
            {
                mensajeError = "El nombre no puede exceder 200 caracteres.";
                return false;
            }

            return true;
        }

        private bool ValidarEmail(string email, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(email))
            {
                mensajeError = "El email es requerido.";
                return false;
            }

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                mensajeError = "El email no tiene un formato válido.";
                return false;
            }

            return true;
        }

        private bool ValidarSigla(string sigla, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(sigla))
            {
                mensajeError = "La sigla de red es requerida.";
                return false;
            }

            if (!Regex.IsMatch(sigla, @"^[a-zA-Z0-9]+$"))
            {
                mensajeError = "La sigla solo puede contener letras y números, sin espacios.";
                return false;
            }

            if (sigla.Length < 2 || sigla.Length > 50)
            {
                mensajeError = "La sigla debe tener entre 2 y 50 caracteres.";
                return false;
            }

            return true;
        }

        #endregion

        #region Métodos de Mensajes

        private void MostrarExito(string mensaje)
        {
            gridUsuarios.JSProperties["cpMessageType"] = "success";
            gridUsuarios.JSProperties["cpMessage"] = mensaje;
        }

        private void MostrarError(string mensaje)
        {
            gridUsuarios.JSProperties["cpMessageType"] = "error";
            gridUsuarios.JSProperties["cpMessage"] = mensaje;
        }

        #endregion
    }
}