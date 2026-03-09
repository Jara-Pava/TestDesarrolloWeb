<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudEspecial.aspx.cs" Inherits="DesarrollosQAS.SolicitudEspecial" MasterPageFile="~/Root.master" %>

<asp:Content ID="contentSolicitudEspecial" ContentPlaceHolderID="Content" runat="Server">
    <style type="text/css">
        tr > .dxflCaptionCell_Office365 {
            padding-bottom: 15px !important;
        }
    </style>

    <script type="text/javascript">
        // Función de validación antes de guardar
        function ValidarYGuardar(s, e) {
            // Validar todos los controles del grupo "Items"
            if (!ASPxClientEdit.ValidateGroup('Items')) {
                e.processOnServer = false;
                return false;
            }

            // Validación adicional de fechas
            var fechaInicio = dteFechaInicio.GetDate();
            var fechaFin = dteFechaFin.GetDate();

            if (fechaInicio && fechaFin && fechaInicio > fechaFin) {
                e.processOnServer = false;
                lblMensajeErrorSolicitud.SetText('La fecha de inicio no puede ser mayor a la fecha de fin.');
                pcMensajeErrorSolicitud.Show();
                return false;
            }

            // Si todo está bien, permitir el postback
            e.processOnServer = true;
        }

        // Función para cancelar
        function CancelarFormulario(s, e) {
            window.location.href = 'SolicitudesEspeciales.aspx';
        }

        // Función para regresar
        function RegresarFormulario(s, e) {
            window.location.href = 'SolicitudesEspeciales.aspx';
        }
    </script>

    <!-- Campos ocultos para comunicación entre servidor y cliente -->
    <asp:HiddenField ID="hfMostrarMensaje" runat="server" Value="false" />
    <asp:HiddenField ID="hfTipoMensaje" runat="server" Value="" />
    <asp:HiddenField ID="hfTextoMensaje" runat="server" Value="" />

    <!-- Popup de Éxito -->
    <dx:ASPxPopupControl ID="pcMensajeExitoSolicitud" runat="server" Width="400" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcMensajeExitoSolicitud"
        HeaderText=" " PopupAnimationType="Fade" ShowFooter="true" ShowOnPageLoad="false" ShowCloseButton="false">
        <HeaderStyle BackColor="#353943" ForeColor="White" Font-Bold="true" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="padding: 20px; text-align: center;">
                    <dx:ASPxLabel ID="lblMensajeExitoSolicitud" runat="server" Font-Size="14px" ClientInstanceName="lblMensajeExitoSolicitud" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterContentTemplate>
            <div style="text-align: center; padding: 10px;">
                <dx:ASPxButton ID="btnCerrarExitoSolicitud" runat="server" Text="Aceptar" Width="100px" AutoPostBack="False" BackColor="#353943" ForeColor="White" Font-Bold="true">
                    <ClientSideEvents Click="function(s, e) { window.location.href='SolicitudesEspeciales.aspx'; }" />
                </dx:ASPxButton>
            </div>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

    <!-- Popup de Error -->
    <dx:ASPxPopupControl ID="pcMensajeErrorSolicitud" runat="server" Width="400" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcMensajeErrorSolicitud"
        HeaderText=" " PopupAnimationType="Fade" ShowFooter="true" ShowOnPageLoad="false" ShowCloseButton="false">
        <HeaderStyle BackColor="#353943" ForeColor="White" Font-Bold="true" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="padding: 20px; text-align: center;">
                    <dx:ASPxLabel ID="lblMensajeErrorSolicitud" runat="server" Font-Size="14px" ClientInstanceName="lblMensajeErrorSolicitud" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterContentTemplate>
            <div style="text-align: center; padding: 10px;">
                <dx:ASPxButton ID="btnCerrarErrorSolicitud" runat="server" Text="Cerrar" Width="100px" AutoPostBack="False" BackColor="#353943" ForeColor="White" Font-Bold="true">
                    <ClientSideEvents Click="function(s, e) { pcMensajeErrorSolicitud.Hide(); }" />
                </dx:ASPxButton>
            </div>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

    <div style="padding-top: 20px; padding-left: 20px; text-align: left">
        <dx:ASPxButton runat="server" ID="btnRegresarSolicitudesEspeciales" Text="Regresar"
            Width="200px" CssClass="btn" BackColor="#353943" ForeColor="White" Font-Bold="true" AutoPostBack="false">
            <ClientSideEvents Click="RegresarFormulario" />
        </dx:ASPxButton>
    </div>

    <%--Template para solicitud especial--%>
    <dx:ASPxFormLayout runat="server" ID="exampleFormLayout" Paddings-PaddingTop="20px" RequiredMarkDisplayMode="RequiredOnly" EnableViewState="true" EncodeHtml="false" UseDefaultPaddings="false" Width="100%">
        <Styles LayoutGroupBox-Caption-Font-Size="X-Large" LayoutGroupBox-Caption-Font-Bold="true" LayoutGroupBox-Caption-ForeColor="#353943"></Styles>
        <Items>
            <dx:LayoutGroup Caption="Solicitud Especial" ColumnCount="4" SettingsItemCaptions-Location="Top" CellStyle-Font-Size="14px">
                <CellStyle Font-Size="14px" />
                <Items>
                    <%--Combobox Tipo Solicitud--%>
                    <dx:LayoutItem Caption="Tipo Solicitud" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="cboTipoSolicitud" TextField="Visita" ValueField="ID_TipoVisita" ValueType="System.Int32" runat="server" Width="100%"
                                    ClientInstanceName="cboTipoSolicitud">
                                    <ValidationSettings ValidationGroup="Items" Display="Dynamic" ErrorDisplayMode="ImageWithText">
                                        <RequiredField IsRequired="true" ErrorText="Requerido" />
                                    </ValidationSettings>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <%--Combobox Proyecto--%>
                    <dx:LayoutItem Caption="Proyecto" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="cboProyecto" TextField="NombreProyecto" ValueField="ID_Proyecto" ValueType="System.Int32" runat="server" Width="100%"
                                    ClientInstanceName="cboProyecto">
                                    <ValidationSettings ValidationGroup="Items" Display="Dynamic" ErrorDisplayMode="ImageWithText">
                                        <RequiredField IsRequired="true" ErrorText="Requerido" />
                                    </ValidationSettings>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <%--Textbox Visitante--%>
                    <dx:LayoutItem Caption="Visitante" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtVisitante" runat="server" Width="100%"
                                    ClientInstanceName="txtVisitante">
                                    <ValidationSettings ValidationGroup="Items" Display="Dynamic" ErrorDisplayMode="ImageWithText">
                                        <RequiredField IsRequired="true" ErrorText="Requerido" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <%--ComboBox Planta--%>
                    <dx:LayoutItem Caption="Planta" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="cboPlanta" TextField="NombrePlanta" ValueField="ID_Planta" ValueType="System.Int32" runat="server" Width="100%"
                                    ClientInstanceName="cboPlanta">
                                    <ValidationSettings ValidationGroup="Items" Display="Dynamic" ErrorDisplayMode="ImageWithText">
                                        <RequiredField IsRequired="true" ErrorText="Requerido" />
                                    </ValidationSettings>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <%--ComboBox Contratista--%>
                    <dx:LayoutItem Caption="Contratista" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="cboContratista" TextField="Responsable" ValueField="id_contratista" ValueType="System.Int32" runat="server" Width="100%"
                                    ClientInstanceName="cboContratista">
                                    <ValidationSettings ValidationGroup="Items" Display="Dynamic" ErrorDisplayMode="ImageWithText">
                                        <RequiredField IsRequired="true" ErrorText="Requerido" />
                                    </ValidationSettings>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <%--Textbox Area de Trabajo--%>
                    <dx:LayoutItem Caption="Área de Trabajo" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtAreaTrabajo" runat="server" Width="100%"
                                    ClientInstanceName="txtAreaTrabajo">
                                    <ValidationSettings ValidationGroup="Items" Display="Dynamic" ErrorDisplayMode="ImageWithText">
                                        <RequiredField IsRequired="true" ErrorText="Requerido" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <%--Memo Actividad--%>
                    <dx:LayoutItem Caption="Actividad" ColumnSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxMemo ID="txtActividad" runat="server" Width="100%" Rows="3"
                                    ClientInstanceName="txtActividad">
                                    <ValidationSettings ValidationGroup="Items" Display="Dynamic" ErrorDisplayMode="ImageWithText">
                                        <RequiredField IsRequired="true" ErrorText="Requerido" />
                                    </ValidationSettings>
                                </dx:ASPxMemo>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <%--Textbox Responsable--%>
                    <dx:LayoutItem Caption="Responsable" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtResponsable" runat="server" Width="100%"
                                    ClientInstanceName="txtResponsable">
                                    <ValidationSettings ValidationGroup="Items" Display="Dynamic" ErrorDisplayMode="ImageWithText">
                                        <RequiredField IsRequired="true" ErrorText="Requerido" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <%--DateEdit Fecha Inicio--%>
                    <dx:LayoutItem Caption="Fecha Inicio" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxDateEdit ID="dteFechaInicio" runat="server" EditFormat="Date" Width="100%"
                                    ClientInstanceName="dteFechaInicio">
                                    <ValidationSettings ValidationGroup="Items" Display="Dynamic" ErrorDisplayMode="ImageWithText">
                                        <RequiredField IsRequired="true" ErrorText="Requerido" />
                                    </ValidationSettings>
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <%--DateEdit Fecha Fin--%>
                    <dx:LayoutItem Caption="Fecha Fin" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxDateEdit ID="dteFechaFin" runat="server" EditFormat="Date" Width="100%"
                                    ClientInstanceName="dteFechaFin">
                                    <ValidationSettings ValidationGroup="Items" Display="Dynamic" ErrorDisplayMode="ImageWithText">
                                        <RequiredField IsRequired="true" ErrorText="Requerido" />
                                    </ValidationSettings>
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <%--CheckBox Aprobado--%>
                    <dx:LayoutItem Caption="Aprobado" ColumnSpan="2" CaptionSettings-Location="Left">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxCheckBox ID="chkAprobado" runat="server" ClientInstanceName="chkAprobado" ReadOnly="true"/>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <%--Botón Cancelar--%>
                    <dx:LayoutItem HorizontalAlign="Left" ShowCaption="False" ColumnSpan="2">
                        <Paddings PaddingTop="20" PaddingRight="12" />
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxButton runat="server" ID="btnCancelar" Text="Cancelar"
                                    Width="200px" CssClass="btn" BackColor="#353943" ForeColor="White" Font-Bold="true"
                                    AutoPostBack="false">
                                    <ClientSideEvents Click="CancelarFormulario" />
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <%--Botón Guardar--%>
                    <dx:LayoutItem HorizontalAlign="Right" ShowCaption="False" ColumnSpan="2">
                        <Paddings PaddingTop="20" />
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxButton runat="server" ID="btnGuardar" Text="Guardar"
                                    Width="200px" CssClass="btn" BackColor="#353943" ForeColor="White" Font-Bold="true"
                                    AutoPostBack="true" ValidationGroup="Items" ClientInstanceName="btnGuardar">
                                    <ClientSideEvents Click="ValidarYGuardar" />
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                </Items>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>
</asp:Content>
