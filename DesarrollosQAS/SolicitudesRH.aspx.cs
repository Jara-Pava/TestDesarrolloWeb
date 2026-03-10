using DataAccessDesarrollos;
using DataAccessDesarrollos.Repositorios;
using DevExpress.Web;
using System;
using System.Collections.Generic;

namespace DesarrollosQAS
{
    public partial class SolicitudesRH : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Los catálogos SIEMPRE deben cargarse para que los ComboBox funcionen correctamente
            CargarCatalogos();

            if (!IsPostBack)
            {
                BindGrid();
            }

            // Aplicar estilos al popup en cada carga
            gridSolicitudesRH.StylesPopup.EditForm.Header.BackColor = System.Drawing.ColorTranslator.FromHtml("#353943");
            gridSolicitudesRH.StylesPopup.EditForm.Header.ForeColor = System.Drawing.Color.White;
            gridSolicitudesRH.StylesPopup.EditForm.Header.Font.Bold = true;
        }

        protected void gridSolicitudesRH_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("DELETE|"))
            {
                try
                {
                    string[] parts = e.Parameters.Split('|');
                    int visibleIndex = Convert.ToInt32(parts[1]);

                    int id = Convert.ToInt32(gridSolicitudesRH.GetRowValues(visibleIndex, "ID_Solicitud"));
                    var repo = new SolicitudRHRepository();
                    repo.EliminarSolicitudRH(id);

                    gridSolicitudesRH.DataBind();
                    MostrarExito("Solicitud eliminada exitosamente.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("Error al eliminar solicitud: {0}", ex);
                    MostrarError($"Error al eliminar: {ex.Message}");
                }
            }
        }

        private void CargarCatalogos()
        {
            try
            {
                var repo = new SolicitudRHRepository();

                // Cargar tipos de solicitud
                var tipoSolicitudColumn = gridSolicitudesRH.Columns["id_TipoSolicitud"] as GridViewDataComboBoxColumn;
                if (tipoSolicitudColumn != null)
                {
                    tipoSolicitudColumn.PropertiesComboBox.DataSource = repo.ObtenerTiposSolicitud();
                    tipoSolicitudColumn.PropertiesComboBox.TextField = "Visita";
                    tipoSolicitudColumn.PropertiesComboBox.ValueField = "ID_TipoVisita";
                }

                // Cargar proyectos
                var proyectoColumn = gridSolicitudesRH.Columns["id_Proyecto"] as GridViewDataComboBoxColumn;
                if (proyectoColumn != null)
                {
                    proyectoColumn.PropertiesComboBox.DataSource = repo.ObtenerProyectos();
                    proyectoColumn.PropertiesComboBox.TextField = "NombreProyecto";
                    proyectoColumn.PropertiesComboBox.ValueField = "ID_Proyecto";
                }

                // Cargar plantas
                var plantaColumn = gridSolicitudesRH.Columns["id_Planta"] as GridViewDataComboBoxColumn;
                if (plantaColumn != null)
                {
                    plantaColumn.PropertiesComboBox.DataSource = repo.ObtenerPlantas();
                    plantaColumn.PropertiesComboBox.TextField = "NombrePlanta";
                    plantaColumn.PropertiesComboBox.ValueField = "ID_Planta";
                }

                // Cargar contratistas
                var contratistaColumn = gridSolicitudesRH.Columns["id_Contratista"] as GridViewDataComboBoxColumn;
                if (contratistaColumn != null)
                {
                    contratistaColumn.PropertiesComboBox.DataSource = repo.ObtenerContratistas();
                    contratistaColumn.PropertiesComboBox.TextField = "Responsable";
                    contratistaColumn.PropertiesComboBox.ValueField = "id_contratista";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al cargar catálogos: {0}", ex);
                MostrarError($"Error al cargar catálogos: {ex.Message}");
            }
        }

        private void BindGrid()
        {
            var repo = new SolicitudRHRepository();
            List<SolicitudRH> solicitudes = repo.ObtenerTodasSolicitudesRH();
            gridSolicitudesRH.DataSource = solicitudes;
            gridSolicitudesRH.DataBind();
        }

        protected void gridUsuarios_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            // Cargar catálogos en los ComboBox del EditForm template
            var repo = new SolicitudRHRepository();

            // Tipo Solicitud
            ASPxComboBox cboTipoSolicitud = gridSolicitudesRH.FindEditFormTemplateControl("cboTipoSolicitudEdit") as ASPxComboBox;
            if (cboTipoSolicitud != null)
            {
                cboTipoSolicitud.DataSource = repo.ObtenerTiposSolicitud();
                cboTipoSolicitud.TextField = "Visita";
                cboTipoSolicitud.ValueField = "ID_TipoVisita";
            }

            // Proyecto
            ASPxComboBox cboProyecto = gridSolicitudesRH.FindEditFormTemplateControl("cboProyectoEdit") as ASPxComboBox;
            if (cboProyecto != null)
            {
                cboProyecto.DataSource = repo.ObtenerProyectos();
                cboProyecto.TextField = "NombreProyecto";
                cboProyecto.ValueField = "ID_Proyecto";
            }

            // Planta
            ASPxComboBox cboPlanta = gridSolicitudesRH.FindEditFormTemplateControl("cboPlantaEdit") as ASPxComboBox;
            if (cboPlanta != null)
            {
                cboPlanta.DataSource = repo.ObtenerPlantas();
                cboPlanta.TextField = "NombrePlanta";
                cboPlanta.ValueField = "ID_Planta";
            }

            // Contratista
            ASPxComboBox cboContratista = gridSolicitudesRH.FindEditFormTemplateControl("cboContratistaEdit") as ASPxComboBox;
            if (cboContratista != null)
            {
                cboContratista.DataSource = repo.ObtenerContratistas();
                cboContratista.TextField = "Responsable";
                cboContratista.ValueField = "id_contratista";
            }

            // Controlar visibilidad del checkbox Aprobado según el modo
            ASPxCheckBox chkAprobado = gridSolicitudesRH.FindEditFormTemplateControl("chkAprobadoEdit") as ASPxCheckBox;
            ASPxLabel lblAprobado = gridSolicitudesRH.FindEditFormTemplateControl("lblAprobadoEdit") as ASPxLabel;
            if (chkAprobado != null)
            {
                // Ocultar en modo inserción, mostrar en modo edición
                chkAprobado.Visible = !gridSolicitudesRH.IsNewRowEditing;
                lblAprobado.Visible = !gridSolicitudesRH.IsNewRowEditing;
            }
        }
        protected void gridSolicitudesRH_DataBinding(object sender, EventArgs e)
        {
            var repo = new SolicitudRHRepository();
            gridSolicitudesRH.DataSource = repo.ObtenerTodasSolicitudesRH();
        }

        protected void gridSolicitudesRH_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            // Este método ya no elimina directamente, solo se usa para el evento del cliente
        }

        protected void gridSolicitudesRH_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                var solicitud = new SolicitudRH
                {
                    id_TipoSolicitud = e.NewValues["id_TipoSolicitud"] != null ? Convert.ToInt32(e.NewValues["id_TipoSolicitud"]) : 0,
                    id_Solicitante = 0,
                    id_Proyecto = e.NewValues["id_Proyecto"] != null ? Convert.ToInt32(e.NewValues["id_Proyecto"]) : 0,
                    id_Planta = e.NewValues["id_Planta"] != null ? Convert.ToInt32(e.NewValues["id_Planta"]) : 0,
                    Visitante = Convert.ToString(e.NewValues["Visitante"]),
                    FechaInicio = e.NewValues["FechaInicio"] != null ? Convert.ToDateTime(e.NewValues["FechaInicio"]) : default(DateTime),
                    FechaFin = e.NewValues["FechaFin"] != null ? Convert.ToDateTime(e.NewValues["FechaFin"]) : default(DateTime),
                    RFC = Convert.ToString(e.NewValues["RFC"]),
                    id_Contratista = e.NewValues["id_Contratista"] != null ? Convert.ToInt32(e.NewValues["id_Contratista"]) : 0,
                    Responsable = Convert.ToString(e.NewValues["Responsable"]),
                    AreaTrabajo = Convert.ToString(e.NewValues["AreaTrabajo"]),
                    Actividad = Convert.ToString(e.NewValues["Actividad"]),
                    Estancia = Convert.ToString(e.NewValues["Estancia"]),
                    FechaSolicitud = DateTime.Now,
                    aprobado = false
                };

                var repo = new SolicitudRHRepository();
                if (!repo.CrearSolicitudRH(solicitud))
                {
                    throw new ApplicationException("No se pudo crear la solicitud.");
                }

                e.Cancel = true;
                gridSolicitudesRH.CancelEdit();
                gridSolicitudesRH.DataBind();
                MostrarExito("Solicitud creada exitosamente.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en RowInserting: {0}", ex);
                e.Cancel = true;
                gridSolicitudesRH.CancelEdit();
                MostrarError($"Error al insertar: {ex.Message}");
            }
        }

        protected void gridSolicitudesRH_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                // Leer el valor del checkbox desde el formulario
                ASPxCheckBox chkAprobado = gridSolicitudesRH.FindEditFormTemplateControl("chkAprobadoEdit") as ASPxCheckBox;
                bool aprobadoValue = chkAprobado != null ? chkAprobado.Checked : false;

                var solicitud = new SolicitudRH
                {
                    ID_Solicitud = Convert.ToInt32(e.Keys["ID_Solicitud"]),
                    id_TipoSolicitud = e.NewValues["id_TipoSolicitud"] != null ? Convert.ToInt32(e.NewValues["id_TipoSolicitud"]) : 0,
                    id_Solicitante = 0,
                    id_Proyecto = e.NewValues["id_Proyecto"] != null ? Convert.ToInt32(e.NewValues["id_Proyecto"]) : 0,
                    id_Planta = e.NewValues["id_Planta"] != null ? Convert.ToInt32(e.NewValues["id_Planta"]) : 0,
                    Visitante = Convert.ToString(e.NewValues["Visitante"]),
                    FechaInicio = e.NewValues["FechaInicio"] != null ? Convert.ToDateTime(e.NewValues["FechaInicio"]) : default(DateTime),
                    FechaFin = e.NewValues["FechaFin"] != null ? Convert.ToDateTime(e.NewValues["FechaFin"]) : default(DateTime),
                    RFC = Convert.ToString(e.NewValues["RFC"]),
                    id_Contratista = e.NewValues["id_Contratista"] != null ? Convert.ToInt32(e.NewValues["id_Contratista"]) : 0,
                    Responsable = Convert.ToString(e.NewValues["Responsable"]),
                    AreaTrabajo = Convert.ToString(e.NewValues["AreaTrabajo"]),
                    Actividad = Convert.ToString(e.NewValues["Actividad"]),
                    Estancia = Convert.ToString(e.NewValues["Estancia"]),
                    aprobado = aprobadoValue
                };

                var repo = new SolicitudRHRepository();
                if (!repo.ActualizarSolicitudRH(solicitud))
                {
                    throw new ApplicationException("No se pudo actualizar la solicitud.");
                }

                e.Cancel = true;
                gridSolicitudesRH.CancelEdit();
                gridSolicitudesRH.DataBind();
                MostrarExito("Solicitud actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error al actualizar solicitud: {0}", ex);
                e.Cancel = true;
                gridSolicitudesRH.CancelEdit();
                MostrarError($"Error al actualizar: {ex.Message}");
            }
        }

        #region Métodos de Mensajes

        private void MostrarExito(string mensaje)
        {
            gridSolicitudesRH.JSProperties["cpMessageType"] = "success";
            gridSolicitudesRH.JSProperties["cpMessage"] = mensaje;
        }

        private void MostrarError(string mensaje)
        {
            gridSolicitudesRH.JSProperties["cpMessageType"] = "error";
            gridSolicitudesRH.JSProperties["cpMessage"] = mensaje;
        }

        #endregion
    }
}