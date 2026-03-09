using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DesarrollosQAS
{
    public partial class SolicitudEspecial : System.Web.UI.Page
    {
        private int? IdSolicitud
        {
            get
            {
                if (ViewState["IdSolicitud"] != null)
                    return (int)ViewState["IdSolicitud"];
                return null;
            }
            set
            {
                ViewState["IdSolicitud"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCatalogos();
                var layoutItemAprobado = FormLayoutSolicitudEspecial.FindItemByFieldName("layoutItemAprobado") as LayoutItem;
                // Verificar si viene un ID por querystring (modo edición)
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out int id))
                {
                    IdSolicitud = id;

                    if (!CargarDatosSolicitud(id))
                    {
                        // Si no se encuentra la solicitud, redirigir
                        Response.Redirect("SolicitudesEspeciales.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                        return;
                    }
                }
                else
                {
                    // Modo creación - limpiar formulario
                    layoutItemAprobado.Visible = true;
                    chkAprobado.Visible = false;
                    chkAprobado.Checked = false;
                    chkAprobado.Enabled = false;
                    layoutItemAprobado.Visible = false;
                    IdSolicitud = null;
                    LimpiarFormulario();
                }
            }

            // Configurar eventos de botones
            btnGuardar.Click += BtnGuardar_Click;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Verificar si hay mensaje para mostrar después de procesar todo
            if (hfMostrarMensaje.Value == "true")
            {
                MostrarPopup(hfTextoMensaje.Value, hfTipoMensaje.Value == "exito");
                hfMostrarMensaje.Value = "false";
            }
        }

        private void CargarCatalogos()
        {
            try
            {
                var repo = new SolicitudRHRepository();

                // Cargar tipos de solicitud
                cboTipoSolicitud.DataSource = repo.ObtenerTiposSolicitud();
                cboTipoSolicitud.TextField = "Visita";
                cboTipoSolicitud.ValueField = "ID_TipoVisita";
                cboTipoSolicitud.DataBind();

                // Cargar proyectos
                cboProyecto.DataSource = repo.ObtenerProyectos();
                cboProyecto.TextField = "NombreProyecto";
                cboProyecto.ValueField = "ID_Proyecto";
                cboProyecto.DataBind();

                // Cargar plantas
                cboPlanta.DataSource = repo.ObtenerPlantas();
                cboPlanta.TextField = "NombrePlanta";
                cboPlanta.ValueField = "ID_Planta";
                cboPlanta.DataBind();

                // Cargar Contratistas
                cboContratista.DataSource = repo.ObtenerContratistas();
                cboContratista.TextField = "Responsable";
                cboContratista.ValueField = "id_contratista";
                cboContratista.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al cargar catálogos: {0}", ex);
                MostrarMensaje("Error al cargar los catálogos: " + ex.Message, false);
            }
        } 

        private bool CargarDatosSolicitud(int id)
        {
            try
            {
                var repo = new SolicitudRHRepository();
                var solicitud = repo.ObtenerSolicitudRHPorId(id);

                if (IdSolicitud.HasValue)
                {
                    chkAprobado.Visible = true;
                    chkAprobado.Checked = solicitud.aprobado;
                    chkAprobado.Enabled = true;
                }
                else {

                }

                if (solicitud != null)
                {
                    cboTipoSolicitud.Value = solicitud.id_TipoSolicitud;
                    cboProyecto.Value = solicitud.id_Proyecto;
                    txtVisitante.Text = solicitud.Visitante;
                    cboPlanta.Value = solicitud.id_Planta;
                    cboContratista.Value = solicitud.id_Contratista;
                    txtAreaTrabajo.Text = solicitud.AreaTrabajo;
                    txtActividad.Text = solicitud.Actividad;
                    txtResponsable.Text = solicitud.Responsable;
                    txtEstancia.Text = solicitud.Estancia;
                    txtRFC.Text = solicitud.RFC;
                    dteFechaInicio.Value = solicitud.FechaInicio;
                    dteFechaFin.Value = solicitud.FechaFin;
                    chkAprobado.Checked = solicitud.aprobado;
                    return true;
                }
                else
                {
                    MostrarMensaje("No se encontró la solicitud.", false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al cargar solicitud: {0}", ex);
                MostrarMensaje("Error al cargar la solicitud: " + ex.Message, false);
                return false;
            }
        }

        private void LimpiarFormulario()
        {
            cboTipoSolicitud.Value = null;
            cboProyecto.Value = null;
            txtVisitante.Text = string.Empty;
            cboPlanta.Value = null;
            cboContratista.Value = null;
            txtAreaTrabajo.Text = string.Empty;
            txtActividad.Text = string.Empty;
            txtResponsable.Text = string.Empty;
            dteFechaInicio.Value = null;
            dteFechaFin.Value = null;
            chkAprobado.Checked = false;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var repo = new SolicitudRHRepository();

                var solicitud = new SolicitudRH
                {
                    id_TipoSolicitud = cboTipoSolicitud.Value != null ? Convert.ToInt32(cboTipoSolicitud.Value) : 0,
                    id_Solicitante = 0,
                    id_Proyecto = cboProyecto.Value != null ? Convert.ToInt32(cboProyecto.Value) : 0,
                    id_Planta = cboPlanta.Value != null ? Convert.ToInt32(cboPlanta.Value) : 0,
                    Visitante = txtVisitante.Text.Trim(),
                    FechaInicio = dteFechaInicio.Value != null ? Convert.ToDateTime(dteFechaInicio.Value) : default(DateTime),
                    FechaFin = dteFechaFin.Value != null ? Convert.ToDateTime(dteFechaFin.Value) : default(DateTime),
                    RFC = txtRFC.Text.Trim(),
                    id_Contratista = cboContratista.Value != null ? Convert.ToInt32(cboContratista.Value) : 0,
                    Responsable = txtResponsable.Text.Trim(),
                    AreaTrabajo = txtAreaTrabajo.Text.Trim(),
                    Actividad = txtActividad.Text.Trim(),
                    Estancia = txtEstancia.Text.Trim(),
                    aprobado = chkAprobado.Checked
                };

                bool resultado = false;

                if (IdSolicitud.HasValue)
                {
                    // Modo edición
                    solicitud.ID_Solicitud = IdSolicitud.Value;
                    resultado = repo.ActualizarSolicitudRH(solicitud);
                    MostrarMensaje(resultado ? "Solicitud actualizada exitosamente." : "Error al actualizar la solicitud.", resultado);
                }
                else
                {
                    // Modo creación
                    solicitud.FechaSolicitud = DateTime.Now;
                    resultado = repo.CrearSolicitudRH(solicitud);
                    MostrarMensaje(resultado ? "Solicitud creada exitosamente." : "Error al crear la solicitud.", resultado);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al guardar solicitud: {0}", ex);
                MostrarMensaje("Error al guardar: " + ex.Message, false);
            }
        }

        private void MostrarMensaje(string mensaje, bool esExito)
        {
            // Configurar HiddenFields para que Page_PreRender los procese
            hfMostrarMensaje.Value = "true";
            hfTipoMensaje.Value = esExito ? "exito" : "error";
            hfTextoMensaje.Value = mensaje;
        }

        private void MostrarPopup(string mensaje, bool esExito)
        {
            // Escapar caracteres especiales para JavaScript
            string mensajeEscapado = mensaje
                .Replace("\\", "\\\\")
                .Replace("'", "\\'")
                .Replace("\"", "\\\"")
                .Replace("\r", "")
                .Replace("\n", "\\n");

            string script = string.Empty;

            if (esExito)
            {
                script = string.Format(@"
                    window.onload = function() {{
                        setTimeout(function() {{
                            if (typeof lblMensajeExitoSolicitud !== 'undefined' && lblMensajeExitoSolicitud) {{
                                lblMensajeExitoSolicitud.SetText('{0}');
                                pcMensajeExitoSolicitud.Show();
                            }} else {{
                                console.error('lblMensajeExitoSolicitud no está definido');
                            }}
                        }}, 100);
                    }};
                ", mensajeEscapado);
            }
            else
            {
                script = string.Format(@"
                    window.onload = function() {{
                        setTimeout(function() {{
                            if (typeof lblMensajeErrorSolicitud !== 'undefined' && lblMensajeErrorSolicitud) {{
                                lblMensajeErrorSolicitud.SetText('{0}');
                                pcMensajeErrorSolicitud.Show();
                            }} else {{
                                console.error('lblMensajeErrorSolicitud no está definido');
                            }}
                        }}, 100);
                    }};
                ", mensajeEscapado);
            }

            // Usar Page.ClientScript que funciona mejor con Master Pages
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "MostrarPopup_" + Guid.NewGuid().ToString(),
                script,
                true
            );
        }
    }
}