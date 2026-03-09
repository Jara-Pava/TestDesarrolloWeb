<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesEspeciales.aspx.cs" Inherits="DesarrollosQAS.SolicitudesEspeciales" MasterPageFile="~/Root.master" %>


<asp:Content ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript">

        // Manejar mensajes después de operaciones del grid
        function OnGridSolicitudesEndCallback(s, e) {
            if (s.cpMessageType && s.cpMessage) {
                if (s.cpMessageType === "success") {
                    lblMensajeExitoSolicitud.SetText(s.cpMessage);
                    pcMensajeExitoSolicitud.Show();
                } else if (s.cpMessageType === "error") {
                    lblMensajeErrorSolicitud.SetText(s.cpMessage);
                    pcMensajeErrorSolicitud.Show();
                }

                delete s.cpMessageType;
                delete s.cpMessage;
            }
        }

        // Redirigir a nueva solicitud
        function NuevaSolicitud() {
            window.location.href = 'SolicitudEspecial.aspx';
        }

        // Manejar botones personalizados (Editar y Eliminar)
        function OnCustomButtonClickSolicitud(s, e) {
            // Botón Editar
            if (e.buttonID === 'btnEditSolicitud') {
                e.processOnServer = false;
                var idSolicitud = s.GetRowKey(e.visibleIndex);

                if (idSolicitud) {
                    window.location.href = 'SolicitudEspecial.aspx?id=' + idSolicitud;
                } else {
                    alert('No se pudo obtener el ID de la solicitud');
                }
                return;
            }

            // Botón Eliminar
            if (e.buttonID === 'btnDeleteSolicitud') {
                e.processOnServer = false;
                currentDeleteIndex = e.visibleIndex;
                pcConfirmarEliminacion.Show();
            }
        }

        // Permitir edición con doble clic
        function OnRowDblClickSolicitud(s, e) {
            if (e.visibleIndex >= 0) {
                var idSolicitud = s.GetRowKey(e.visibleIndex);
                if (idSolicitud) {
                    window.location.href = 'SolicitudEspecial.aspx?id=' + idSolicitud;
                }
            }
        }

        // Variable global para guardar el índice de la fila a eliminar
        var currentDeleteIndex = -1;

        // Confirmar eliminación desde el popup
        function ConfirmarEliminacion() {
            if (currentDeleteIndex >= 0) {
                gridSolicitudesEspeciales.PerformCallback('DELETE|' + currentDeleteIndex);
                pcConfirmarEliminacion.Hide();
            }
        }

        // Cancelar eliminación
        function CancelarEliminacion() {
            currentDeleteIndex = -1;
            pcConfirmarEliminacion.Hide();
        }

    </script>

    <div style="padding-top: 8px">
        <dx:ASPxLabel runat="server" ID="ASPxLabel7" Text="Solicitudes de Visitas" Font-Bold="true" Font-Size="X-Large"></dx:ASPxLabel>
    </div>
    <br />

    <!-- Popup de Confirmación de Eliminación -->
    <dx:ASPxPopupControl ID="pcConfirmarEliminacion" runat="server" Width="450" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcConfirmarEliminacion"
        HeaderText=" " PopupAnimationType="Fade" ShowFooter="true" ShowOnPageLoad="false">
        <HeaderStyle BackColor="#353943" ForeColor="White" Font-Bold="true" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="padding: 30px; text-align: center;">
                    <dx:ASPxLabel runat="server" Text="¿Está seguro que desea eliminar esta solicitud?" Font-Size="16px" Font-Bold="true" />
                    <br />
                    <br />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterContentTemplate>
            <div style="text-align: center; padding: 10px;">
                <dx:ASPxButton ID="btnConfirmarEliminar" runat="server" Text="Sí, Eliminar" Width="120px" AutoPostBack="False"
                    BackColor="#353943" ForeColor="White" Font-Bold="true" Style="margin-left: 10px;">
                    <ClientSideEvents Click="ConfirmarEliminacion" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnCancelarEliminar" runat="server" Text="Cancelar" Width="120px" AutoPostBack="False"
                    BackColor="#353943" ForeColor="White" Font-Bold="true" Style="margin-left: 90px;">
                    <ClientSideEvents Click="CancelarEliminacion" />
                </dx:ASPxButton>
            </div>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

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
                <dx:ASPxButton ID="btnCerrarExitoSolicitud" runat="server" Text="OK" Width="100px" AutoPostBack="False" BackColor="#353943" ForeColor="White" Font-Bold="true">
                    <ClientSideEvents Click="function(s, e) { pcMensajeExitoSolicitud.Hide(); }" />
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
                <dx:ASPxButton ID="btnCerrarErrorSolicitud" runat="server" Text="OK" Width="100px" AutoPostBack="False" BackColor="#353943" ForeColor="White" Font-Bold="true">
                    <ClientSideEvents Click="function(s, e) { pcMensajeErrorSolicitud.Hide(); }" />
                </dx:ASPxButton>
            </div>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

    <!-- Grid de Solicitudes RH -->
    <dx:ASPxGridView ID="gridSolicitudesEspeciales" runat="server"
        KeyFieldName="ID_Solicitud"
        Width="100%"
        ForeColor="Black"
        ClientInstanceName="gridSolicitudesEspeciales"
        OnDataBinding="gridSolicitudesEspeciales_DataBinding"
        OnCustomButtonCallback="gridSolicitudesEspeciales_CustomButtonCallback"
        OnCustomCallback="gridSolicitudesEspeciales_CustomCallback">
        <ClientSideEvents
            EndCallback="OnGridSolicitudesEndCallback"
            CustomButtonClick="function(s, e) { OnCustomButtonClickSolicitud(s, e); }"
            RowDblClick="function(s, e) { OnRowDblClickSolicitud(s, e); }" />
        <Styles>
            <Header BackColor="#353943" ForeColor="White" Font-Bold="true"></Header>
        </Styles>
        <SettingsResizing ColumnResizeMode="Control"/>
        <Columns>
            <dx:GridViewCommandColumn Caption="Acciones" Width="100px" ButtonRenderMode="Image">
                <HeaderTemplate>
                    <div style="text-align: center;">
                        <dx:ASPxButton runat="server" ID="btnNuevoHeader" Text="" AutoPostBack="false"
                            ToolTip="Nueva Solicitud" RenderMode="Link">
                            <Image Url="~/Images/add.png" Width="30px" Height="30px" />
                            <ClientSideEvents Click="NuevaSolicitud" />
                        </dx:ASPxButton>
                    </div>
                </HeaderTemplate>
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnEditSolicitud" Text="Editar">
                        <Image Url="~/Images/edits.png" Width="18px" Height="18px" ToolTip="Editar" />
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="btnDeleteSolicitud" Text="Eliminar">
                        <Image Url="~/Images/delete.png" Width="18px" Height="18px" ToolTip="Eliminar" />
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn FieldName="ID_Solicitud" Caption="ID" Visible="false" ReadOnly="true" />

            <dx:GridViewDataComboBoxColumn FieldName="id_TipoSolicitud" Caption="Tipo Solicitud" Width="180px">
                <PropertiesComboBox TextField="Visita" ValueField="ID_TipoVisita" ValueType="System.Int32"></PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataTextColumn FieldName="Visitante" Caption="Visitante" Width="200px" HeaderStyle-HorizontalAlign="Center">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataComboBoxColumn FieldName="id_Proyecto" Caption="Proyecto" Width="120px">
                <PropertiesComboBox TextField="NombreProyecto" ValueField="ID_Proyecto" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataComboBoxColumn FieldName="id_Planta" Caption="Planta" Width="120px">
                <PropertiesComboBox TextField="NombrePlanta" ValueField="ID_Planta" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataComboBoxColumn FieldName="id_Contratista" Caption="Contratista" Width="150px">
                <PropertiesComboBox TextField="Responsable" ValueField="id_contratista" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataTextColumn FieldName="AreaTrabajo" Caption="Área de Trabajo" Width="150px" />

            <dx:GridViewDataTextColumn FieldName="Actividad" Caption="Actividad" Width="250px" HeaderStyle-HorizontalAlign="Center" />

            <dx:GridViewDataTextColumn FieldName="Estancia" Caption="Estancia" Width="100px" />

            <dx:GridViewDataDateColumn FieldName="FechaInicio" Caption="Fecha Inicio" Width="110px">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataDateColumn FieldName="FechaFin" Caption="Fecha Fin" Width="110px">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataDateColumn FieldName="FechaSolicitud" Caption="Fecha Solicitud" Width="130px" ReadOnly="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataTextColumn FieldName="RFC" Caption="RFC" Width="120px" />

            <dx:GridViewDataTextColumn FieldName="Responsable" Caption="Responsable" Width="200px" />

            <dx:GridViewDataCheckColumn FieldName="aprobado" Caption="Aprobado" Width="80px" />

        </Columns>
        <SettingsPager PageSize="25">
            <PageSizeItemSettings ShowAllItem="true" Visible="true"></PageSizeItemSettings>
        </SettingsPager>
    </dx:ASPxGridView>

</asp:Content>
