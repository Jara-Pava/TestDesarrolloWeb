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
                    IdSolicitud = null;
                    LimpiarFormulario();
                }
            }

            // Configurar eventos de botones
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;
            btnRegresarSolicitudesEspeciales.Click += BtnRegresar_Click;
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
                if (!ValidarFormulario())
                    return;

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
                    RFC = string.Empty,
                    id_Contratista = cboContratista.Value != null ? Convert.ToInt32(cboContratista.Value) : 0,
                    Responsable = txtResponsable.Text.Trim(),
                    AreaTrabajo = txtAreaTrabajo.Text.Trim(),
                    Actividad = txtActividad.Text.Trim(),
                    Estancia = string.Empty,
                    aprobado = chkAprobado.Checked
                };

                bool resultado = false;

                if (IdSolicitud.HasValue)
                {
                    // Modo edición
                    solicitud.ID_Solicitud = IdSolicitud.Value;
                    resultado = repo.ActualizarSolicitudRH(solicitud);

                    if (resultado)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "mensaje",
                            "alert('Solicitud actualizada exitosamente.'); window.location.href='SolicitudesEspeciales.aspx';", true);
                    }
                    else
                    {
                        MostrarMensaje("Error al actualizar la solicitud.", false);
                    }
                }
                else
                {
                    // Modo creación
                    solicitud.FechaSolicitud = DateTime.Now;
                    resultado = repo.CrearSolicitudRH(solicitud);

                    if (resultado)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "mensaje",
                            "alert('Solicitud creada exitosamente.'); window.location.href='SolicitudesEspeciales.aspx';", true);
                    }
                    else
                    {
                        MostrarMensaje("Error al crear la solicitud.", false);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al guardar solicitud: {0}", ex);
                MostrarMensaje("Error al guardar: " + ex.Message, false);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("SolicitudesEspeciales.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("SolicitudesEspeciales.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private bool ValidarFormulario()
        {
            if (cboTipoSolicitud.Value == null)
            {
                MostrarMensaje("Debe seleccionar un tipo de solicitud.", false);
                return false;
            }

            if (cboProyecto.Value == null)
            {
                MostrarMensaje("Debe seleccionar un proyecto.", false);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtVisitante.Text))
            {
                MostrarMensaje("Debe ingresar el nombre del visitante.", false);
                return false;
            }

            if (dteFechaInicio.Value == null || dteFechaFin.Value == null)
            {
                MostrarMensaje("Debe ingresar las fechas de inicio y fin.", false);
                return false;
            }

            if (Convert.ToDateTime(dteFechaInicio.Value) > Convert.ToDateTime(dteFechaFin.Value))
            {
                MostrarMensaje("La fecha de inicio no puede ser mayor a la fecha de fin.", false);
                return false;
            }

            return true;
        }

        private void MostrarMensaje(string mensaje, bool esExito)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "mensaje",
                $"alert('{mensaje}');", true);
        }
    }
}