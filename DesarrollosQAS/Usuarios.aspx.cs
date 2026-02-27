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
        }


        protected void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            //lblMensaje.ForeColor = System.Drawing.Color.Red;
            //lblMensaje.Text = string.Empty;

            try
            {
                var usuario = new Usuario
                {
                    nombre = tbNombre.Text?.Trim(),
                    sigla_red = tbSigla.Text?.Trim(),
                    Email = tbEmail.Text?.Trim(),
                    activo = chbActivo.Checked
                };

                // Validación simple del lado servidor (además de la del cliente)
                if (string.IsNullOrWhiteSpace(usuario.nombre) ||
                    string.IsNullOrWhiteSpace(usuario.sigla_red) ||
                    string.IsNullOrWhiteSpace(usuario.Email))
                {
                    //lblMensaje.Text = "Completa los campos requeridos.";
                    return;
                }

                var repo = new UsuarioSistemaRepository();

                // SUGERENCIA: no te bases en rows>0 por NOCOUNT; usa try/catch o retorno booleano del repo.
                bool ok = repo.CrearUsuario(usuario);

                if (ok)
                {
                    //lblMensaje.ForeColor = System.Drawing.Color.Green;
                    //lblMensaje.Text = "Usuario creado correctamente.";
                    LimpiarFormulario();
                    BindGrid();
                }
                else
                {
                    //lblMensaje.Text = "No se pudo crear el usuario.";
                }
            }
            catch (Exception ex)
            {
                // Muestra mensaje amigable y loguea ex según tus políticas
                //lblMensaje.Text = ex.Message;
            }
        }


        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            //lblMensaje.Text = string.Empty;
        }

        private void LimpiarFormulario()
        {
            tbNombre.Text = string.Empty;
            tbSigla.Text = string.Empty;
            tbEmail.Text = string.Empty;
            chbActivo.Checked = true;
        }

        // --- Eventos del grid si los estás usando para Edit/Delete ---
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

        //protected void gridUsuarios_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        //{
        //    var u = new Usuario
        //    {
        //        id_usuario = Convert.ToInt32(e.Keys["id_usuario"]),
        //        nombre = Convert.ToString(e.NewValues["nombre"]),
        //        sigla_red = Convert.ToString(e.NewValues["sigla_red"]),
        //        activo = e.NewValues["activo"] != null && (bool)e.NewValues["activo"],
        //        Email = Convert.ToString(e.NewValues["Email"])
        //    };

        //    var repo = new UsuarioSistemaRepository();
        //    if (!repo.ActualizarUsuario(u))
        //        throw new ApplicationException("No se pudo actualizar el usuario.");

        //    e.Cancel = true;
        //    gridUsuarios.CancelEdit();
        //    BindGrid();
        //}

        //protected void gridUsuarios_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        //{
        //    int id = Convert.ToInt32(e.Keys["id_usuario"]);
        //    var repo = new UsuarioSistemaRepository();
        //    if (!repo.EliminarUsuario(id))
        //        throw new ApplicationException("No se pudo eliminar el usuario.");

        //    e.Cancel = true;
        //    BindGrid();
        //}



        //protected void btnGuardarNuevo_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var u = new Usuario
        //        {
        //            nombre = txtNombre.Text?.Trim(),
        //            sigla_red = txtSigla.Text?.Trim(),
        //            activo = chkActivo.Checked,
        //            Email = txtEmail.Text?.Trim()
        //        };

        //        CrearUsuario(u);

        //        // Limpia el form y recarga grid
        //        txtNombre.Text = string.Empty;
        //        txtSigla.Text = string.Empty;
        //        txtEmail.Text = string.Empty;
        //        chkActivo.Checked = true;

        //        lblMensaje.ForeColor = System.Drawing.Color.Green;
        //        lblMensaje.Text = "Usuario creado correctamente.";

        //        BindGrid();
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMensaje.ForeColor = System.Drawing.Color.Red;
        //        lblMensaje.Text = ex.Message;
        //    }
        //}


        //protected void gridUsuarios_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        //{
        //    var u = new Usuario
        //    {
        //        nombre = Convert.ToString(e.NewValues["nombre"]),
        //        sigla_red = Convert.ToString(e.NewValues["sigla_red"]),
        //        activo = e.NewValues["activo"] != null && (bool)e.NewValues["activo"],
        //        Email = Convert.ToString(e.NewValues["Email"])
        //    };

        //    var repo = new UsuarioSistemaRepository();
        //    if (!repo.CrearUsuario(u))
        //        throw new ApplicationException("No se pudo crear el usuario.");

        //    e.Cancel = true;
        //    gridUsuarios.CancelEdit();
        //    BindGrid();
        //}
    }
}