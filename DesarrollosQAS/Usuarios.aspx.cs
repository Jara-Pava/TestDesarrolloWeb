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

        protected void gridUsuarios_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            // Obtener los controles del formulario
            ASPxTextBox txtSiglaRed = gridUsuarios.FindEditFormTemplateControl("txtSiglaRed") as ASPxTextBox;
            ASPxLabel lblActivo = gridUsuarios.FindEditFormTemplateControl("lblActivo") as ASPxLabel;
            ASPxCheckBox chkActivo = gridUsuarios.FindEditFormTemplateControl("chkActivo") as ASPxCheckBox;

            if (gridUsuarios.IsNewRowEditing)
            {
                // Modo CREACIÓN: Ocultar campo Activo
                if (lblActivo != null)
                {
                    lblActivo.Visible = false;
                }
                if (chkActivo != null)
                {
                    chkActivo.Visible = false;
                    chkActivo.Checked = true; // Por defecto activo en creación
                }

                // Habilitar sigla_red en modo creación
                if (txtSiglaRed != null)
                {
                    txtSiglaRed.ClientEnabled = true;
                    txtSiglaRed.BackColor = System.Drawing.Color.White;
                }
            }
            else
            {
                // Modo EDICIÓN: Mostrar campo Activo y deshabilitar sigla_red
                if (lblActivo != null)
                {
                    lblActivo.Visible = true;
                }
                if (chkActivo != null)
                {
                    chkActivo.Visible = true;
                    chkActivo.Enabled = true; // Permitir editar el estado
                }

                // Deshabilitar sigla_red en modo edición
                if (txtSiglaRed != null)
                {
                    txtSiglaRed.ClientEnabled = false;
                    txtSiglaRed.BackColor = System.Drawing.ColorTranslator.FromHtml("#f0f0f0");
                }
            }
        }

        protected void gridUsuarios_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID == "btnDelete")
            {
                try
                {
                    int id = Convert.ToInt32(gridUsuarios.GetRowValues(e.VisibleIndex, "id_usuario"));
                    var repo = new UsuarioSistemaRepository();
                    repo.EliminarUsuario(id);

                    gridUsuarios.DataBind();
                    MostrarExito("Usuario eliminado exitosamente.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("Error al eliminar usuario: {0}", ex);
                    MostrarError($"Error al eliminar: {ex.Message}");
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

                // En modo creación, siempre crear como activo
                bool activo = true;

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

                var repo = new UsuarioSistemaRepository();
                if (repo.ExisteUsuario(siglaRed, email, out string mensajeExistencia))
                {
                    e.Cancel = true;
                    gridUsuarios.CancelEdit();
                    MostrarError(mensajeExistencia);
                    return;
                }

                var usuario = new Usuario
                {
                    nombre = nombre,
                    sigla_red = siglaRed,
                    Email = email,
                    activo = activo // Siempre true en creación
                };

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

                // Obtener el valor del checkbox desde el template
                ASPxCheckBox chkActivo = gridUsuarios.FindEditFormTemplateControl("chkActivo") as ASPxCheckBox;
                bool activo = chkActivo != null ? chkActivo.Checked : false;

                var usuario = new Usuario
                {
                    id_usuario = Convert.ToInt32(e.Keys["id_usuario"]),
                    nombre = nombre,
                    sigla_red = siglaRedOriginal,
                    Email = email,
                    activo = activo
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