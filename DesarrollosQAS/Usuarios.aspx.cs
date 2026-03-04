using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using DevExpress.Web;
using System;
using System.Text.RegularExpressions;
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
            gridUsuarios.DataSource = repo.ObtenerTodosUsuarios();
            gridUsuarios.DataBind();
        }

        protected void gridUsuarios_DataBinding(object sender, EventArgs e)
        {
            var repo = new UsuarioSistemaRepository();
            gridUsuarios.DataSource = repo.ObtenerTodosUsuarios();
        }

        // Método de validación de nombre
        private bool ValidarNombre(string nombre, out string mensajeError)
        {
            mensajeError = string.Empty;

            // Validar que no sea nulo o vacío
            if (string.IsNullOrWhiteSpace(nombre))
            {
                mensajeError = "El nombre no puede estar vacío o contener solo espacios.";
                return false;
            }

            // Validar que no contenga solo caracteres especiales o números
            if (Regex.IsMatch(nombre, @"^[\W\d_]+$"))
            {
                mensajeError = "El nombre debe contener al menos letras válidas.";
                return false;
            }

            // Validar longitud mínima (al menos 2 caracteres válidos)
            if (nombre.Trim().Length < 2)
            {
                mensajeError = "El nombre debe tener al menos 2 caracteres.";
                return false;
            }

            // Validar que no contenga caracteres peligrosos (inyección, scripts, etc.)
            if (Regex.IsMatch(nombre, @"[<>""';={}()\[\]]"))
            {
                mensajeError = "El nombre contiene caracteres no permitidos.";
                return false;
            }

            // Validar longitud máxima
            if (nombre.Length > 200)
            {
                mensajeError = "El nombre no puede exceder 200 caracteres.";
                return false;
            }

            return true;
        }

        // Método de validación de email
        private bool ValidarEmail(string email, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(email))
            {
                mensajeError = "El email es requerido.";
                return false;
            }

            // Validación robusta de email
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                mensajeError = "El email no tiene un formato válido.";
                return false;
            }

            return true;
        }

        // Método de validación de sigla
        private bool ValidarSigla(string sigla, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(sigla))
            {
                mensajeError = "La sigla de red es requerida.";
                return false;
            }

            // Validar que solo contenga letras y números (sin espacios ni caracteres especiales)
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

        // Deshabilitar Sigla Red en modo edición
        protected void gridUsuarios_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "sigla_red")
            {
                if (gridUsuarios.IsEditing && !gridUsuarios.IsNewRowEditing)
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
        }

        protected void gridUsuarios_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
        }

        protected void gridUsuarios_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                string nombre = e.NewValues["nombre"]?.ToString()?.Trim();
                string siglaRed = e.NewValues["sigla_red"]?.ToString()?.Trim();
                string email = e.NewValues["Email"]?.ToString()?.Trim();

                // Validar nombre
                if (!ValidarNombre(nombre, out string errorNombre))
                {
                    e.Cancel = true;
                    gridUsuarios.JSProperties["cpMessageType"] = "error";
                    gridUsuarios.JSProperties["cpMessage"] = errorNombre;
                    return;
                }

                // Validar sigla
                if (!ValidarSigla(siglaRed, out string errorSigla))
                {
                    e.Cancel = true;
                    gridUsuarios.JSProperties["cpMessageType"] = "error";
                    gridUsuarios.JSProperties["cpMessage"] = errorSigla;
                    return;
                }

                // Validar email
                if (!ValidarEmail(email, out string errorEmail))
                {
                    e.Cancel = true;
                    gridUsuarios.JSProperties["cpMessageType"] = "error";
                    gridUsuarios.JSProperties["cpMessage"] = errorEmail;
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
                BindGrid();

                gridUsuarios.JSProperties["cpMessageType"] = "success";
                gridUsuarios.JSProperties["cpMessage"] = "Usuario creado exitosamente.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en RowInserting: {0}", ex);
                e.Cancel = true;
                gridUsuarios.JSProperties["cpMessageType"] = "error";
                gridUsuarios.JSProperties["cpMessage"] = $"Error al insertar: {ex.Message}";
            }
        }

        protected void gridUsuarios_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                string nombre = e.NewValues["nombre"]?.ToString()?.Trim();
                string email = e.NewValues["Email"]?.ToString()?.Trim();

                // Validar nombre
                if (!ValidarNombre(nombre, out string errorNombre))
                {
                    e.Cancel = true;
                    gridUsuarios.JSProperties["cpMessageType"] = "error";
                    gridUsuarios.JSProperties["cpMessage"] = errorNombre;
                    return;
                }

                // Validar email
                if (!ValidarEmail(email, out string errorEmail))
                {
                    e.Cancel = true;
                    gridUsuarios.JSProperties["cpMessageType"] = "error";
                    gridUsuarios.JSProperties["cpMessage"] = errorEmail;
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
                BindGrid();

                gridUsuarios.JSProperties["cpMessageType"] = "success";
                gridUsuarios.JSProperties["cpMessage"] = "Usuario actualizado exitosamente.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al actualizar usuario: {0}", ex);
                e.Cancel = true;
                gridUsuarios.JSProperties["cpMessageType"] = "error";
                gridUsuarios.JSProperties["cpMessage"] = $"Error al actualizar: {ex.Message}";
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
                BindGrid();

                gridUsuarios.JSProperties["cpMessageType"] = "success";
                gridUsuarios.JSProperties["cpMessage"] = "Usuario eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al eliminar usuario: {0}", ex);
                e.Cancel = true;
                gridUsuarios.JSProperties["cpMessageType"] = "error";
                gridUsuarios.JSProperties["cpMessage"] = $"Error al eliminar: {ex.Message}";
            }
        }

        protected void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = tbNombre.Text?.Trim();
                string siglaRed = tbSigla.Text?.Trim();
                string email = tbEmail.Text?.Trim();

                // Validar nombre
                if (!ValidarNombre(nombre, out string errorNombre))
                {
                    MostrarMensajeError(errorNombre);
                    return;
                }

                // Validar sigla
                if (!ValidarSigla(siglaRed, out string errorSigla))
                {
                    MostrarMensajeError(errorSigla);
                    return;
                }

                // Validar email
                if (!ValidarEmail(email, out string errorEmail))
                {
                    MostrarMensajeError(errorEmail);
                    return;
                }

                var usuario = new Usuario
                {
                    nombre = nombre,
                    sigla_red = siglaRed,
                    Email = email,
                    activo = chbActivo.Checked
                };

                var repo = new UsuarioSistemaRepository();
                if (repo.CrearUsuario(usuario))
                {
                    BindGrid();
                    LimpiarFormulario();
                    pcCrearUsuario.ShowOnPageLoad = false;
                    MostrarMensajeExito("Usuario creado exitosamente.");
                }
                else
                {
                    MostrarMensajeError("No se pudo crear el usuario.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al crear usuario: {0}", ex);
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
            pcMensajeExito.ShowOnPageLoad = true;
        }

        private void MostrarMensajeError(string mensaje)
        {
            lblMensajeError.Text = mensaje;
            pcMensajeError.ShowOnPageLoad = true;
        }
    }
}