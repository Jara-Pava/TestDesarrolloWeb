using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DesarrollosQAS
{
    public partial class SolicitudesRH : System.Web.UI.Page
    {
        // Claves de sesión para mensajes
        private const string SESSION_SUCCESS_MESSAGE = "SolicitudRHSuccessMsg";
        private const string SESSION_ERROR_MESSAGE = "SolicitudRHErrorMsg";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCatalogos();
                BindGrid();
                MostrarMensajesDeSesion();
            }
        }

        private void MostrarMensajesDeSesion()
        {
            if (Session[SESSION_SUCCESS_MESSAGE] != null)
            {
                string mensaje = Session[SESSION_SUCCESS_MESSAGE].ToString();
                Session.Remove(SESSION_SUCCESS_MESSAGE);
                // Aquí puedes mostrar el mensaje de éxito en un control de tu página
            }
            else if (Session[SESSION_ERROR_MESSAGE] != null)
            {
                string mensaje = Session[SESSION_ERROR_MESSAGE].ToString();
                Session.Remove(SESSION_ERROR_MESSAGE);
                // Aquí puedes mostrar el mensaje de error en un control de tu página
            }
        }

        private void CargarCatalogos()
        {
            try
            {
                var repo = new SolicitudRHRepository();

                // Cargar tipos de solicitud
                //var tipoSolicitudColumn = gridSolicitudesRH.Columns["id_TipoSolicitud"] as GridViewDataComboBoxColumn;
                //if (tipoSolicitudColumn != null)
                //{
                //    tipoSolicitudColumn.PropertiesComboBox.DataSource = repo.ObtenerTiposSolicitud();
                //    tipoSolicitudColumn.PropertiesComboBox.DataBind();
                //}

                //// Cargar proyectos
                //var proyectoColumn = gridSolicitudesRH.Columns["id_Proyecto"] as GridViewDataComboBoxColumn;
                //if (proyectoColumn != null)
                //{
                //    proyectoColumn.PropertiesComboBox.DataSource = repo.ObtenerProyectos();
                //    proyectoColumn.PropertiesComboBox.DataBind();
                //}

                //// Cargar plantas
                //var plantaColumn = gridSolicitudesRH.Columns["id_Planta"] as GridViewDataComboBoxColumn;
                //if (plantaColumn != null)
                //{
                //    plantaColumn.PropertiesComboBox.DataSource = repo.ObtenerPlantas();
                //    plantaColumn.PropertiesComboBox.DataBind();
                //}

                //// Cargar contratistas
                //var contratistaColumn = gridSolicitudesRH.Columns["id_Contratista"] as GridViewDataComboBoxColumn;
                //if (contratistaColumn != null)
                //{
                //    contratistaColumn.PropertiesComboBox.DataSource = repo.ObtenerContratistas();
                //    contratistaColumn.PropertiesComboBox.DataBind();
                //}
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al cargar catálogos: {0}", ex);
            }
        }

        private void BindGrid()
        {
            var repo = new SolicitudRHRepository();
            List<SolicitudRH> solicitudes = repo.ObtenerTodasSolicitudesRH();
            gridSolicitudesRH.DataSource = solicitudes;
            gridSolicitudesRH.DataBind();
        }

        protected void gridSolicitudesRH_DataBinding(object sender, EventArgs e)
        {
            var repo = new SolicitudRHRepository();
            gridSolicitudesRH.DataSource = repo.ObtenerTodasSolicitudesRH();
        }

        protected void gridSolicitudesRH_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                var solicitud = new SolicitudRH
                {
                    id_TipoSolicitud = e.NewValues["id_TipoSolicitud"] != null ? Convert.ToInt32(e.NewValues["id_TipoSolicitud"]) : 0,
                    id_Solicitante = 0, // Asignar el ID del usuario actual desde sesión o contexto
                    id_Proyecto = e.NewValues["id_Proyecto"] != null ? Convert.ToInt32(e.NewValues["id_Proyecto"]) : 0,
                    id_Planta = e.NewValues["id_Planta"] != null ? Convert.ToInt32(e.NewValues["id_Planta"]) : 0,
                    Visitante = Convert.ToString(e.NewValues["Visitante"]),
                    FechaInicio = e.NewValues["FechaInicio"] != null ? Convert.ToDateTime(e.NewValues["FechaInicio"]) : default(DateTime),
                    FechaFin = e.NewValues["FechaFin"] != null ? Convert.ToDateTime(e.NewValues["FechaFin"]) : default(DateTime),
                    RFC = Convert.ToString(e.NewValues["RFC"]),
                    id_Contratista = e.NewValues["id_Contratista"] != null ? Convert.ToInt32(e.NewValues["id_Contratista"]) : 0,
                    Responsable = Convert.ToString(e.NewValues["Resposable"]),
                    AreaTrabajo = Convert.ToString(e.NewValues["AreaTrabajo"]),
                    Actividad = Convert.ToString(e.NewValues["Actividad"]),
                    Estancia = Convert.ToString(e.NewValues["Estancia"]),
                    FechaSolicitud = DateTime.Now,
                    aprobado = e.NewValues["aprobado"] != null && (bool)e.NewValues["aprobado"]
                };

                var repo = new SolicitudRHRepository();
                if (!repo.CrearSolicitudRH(solicitud))
                {
                    throw new ApplicationException("No se pudo crear la solicitud.");
                }

                e.Cancel = true;
                gridSolicitudesRH.CancelEdit();
                Session[SESSION_SUCCESS_MESSAGE] = "Solicitud creada exitosamente.";
                ASPxWebControl.RedirectOnCallback("SolicitudesRH.aspx");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en RowInserting: {0}", ex);
                e.Cancel = true;
                Session[SESSION_ERROR_MESSAGE] = $"Error al insertar: {ex.Message}";
                ASPxWebControl.RedirectOnCallback("SolicitudesRH.aspx");
            }
        }

        protected void gridSolicitudesRH_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                var solicitud = new SolicitudRH
                {
                    ID_Solicitud = Convert.ToInt32(e.Keys["ID_Solicitud"]),
                    id_TipoSolicitud = e.NewValues["id_TipoSolicitud"] != null ? Convert.ToInt32(e.NewValues["id_TipoSolicitud"]) : 0,
                    id_Solicitante = 0, // Mantener el solicitante original o actualizar según la lógica
                    id_Proyecto = e.NewValues["id_Proyecto"] != null ? Convert.ToInt32(e.NewValues["id_Proyecto"]) : 0,
                    id_Planta = e.NewValues["id_Planta"] != null ? Convert.ToInt32(e.NewValues["id_Planta"]) : 0,
                    Visitante = Convert.ToString(e.NewValues["Visitante"]),
                    FechaInicio = e.NewValues["FechaInicio"] != null ? Convert.ToDateTime(e.NewValues["FechaInicio"]) : default(DateTime),
                    FechaFin = e.NewValues["FechaFin"] != null ? Convert.ToDateTime(e.NewValues["FechaFin"]) : default(DateTime),
                    RFC = Convert.ToString(e.NewValues["RFC"]),
                    id_Contratista = e.NewValues["id_Contratista"] != null ? Convert.ToInt32(e.NewValues["id_Contratista"]) : 0,
                    Responsable = Convert.ToString(e.NewValues["Resposable"]),
                    AreaTrabajo = Convert.ToString(e.NewValues["AreaTrabajo"]),
                    Actividad = Convert.ToString(e.NewValues["Actividad"]),
                    Estancia = Convert.ToString(e.NewValues["Estancia"]),
                    aprobado = e.NewValues["aprobado"] != null && (bool)e.NewValues["aprobado"]
                };

                var repo = new SolicitudRHRepository();
                if (!repo.ActualizarSolicitudRH(solicitud))
                {
                    throw new ApplicationException("No se pudo actualizar la solicitud.");
                }

                e.Cancel = true;
                gridSolicitudesRH.CancelEdit();
                Session[SESSION_SUCCESS_MESSAGE] = "Solicitud actualizada exitosamente.";
                ASPxWebControl.RedirectOnCallback("SolicitudesRH.aspx");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al actualizar solicitud: {0}", ex);
                e.Cancel = true;
                Session[SESSION_ERROR_MESSAGE] = $"Error al actualizar: {ex.Message}";
                ASPxWebControl.RedirectOnCallback("SolicitudesRH.aspx");
            }
        }

        protected void gridSolicitudesRH_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(e.Keys["ID_Solicitud"]);
                var repo = new SolicitudRHRepository();
                repo.EliminarSolicitudRH(id);

                e.Cancel = true;
                gridSolicitudesRH.CancelEdit();
                Session[SESSION_SUCCESS_MESSAGE] = "Solicitud eliminada exitosamente.";
                ASPxWebControl.RedirectOnCallback("SolicitudesRH.aspx");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al eliminar solicitud: {0}", ex);
                e.Cancel = true;
                Session[SESSION_ERROR_MESSAGE] = $"Error al eliminar: {ex.Message}";
                ASPxWebControl.RedirectOnCallback("SolicitudesRH.aspx");
            }
        }
    }
}