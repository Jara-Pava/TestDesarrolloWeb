<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesRH.aspx.cs" MasterPageFile="~/Root.master" Inherits="DesarrollosQAS.SolicitudesRH" %>

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

        // Confirmar antes de eliminar - Mostrar popup personalizado
        function OnCustomButtonClickSolicitud(s, e) {
            if (e.buttonID === 'btnDeleteSolicitud') {
                e.processOnServer = false; // Cancelar el proceso del servidor
                currentDeleteIndex = e.visibleIndex; // Guardar el índice de la fila
                pcConfirmarEliminacion.Show(); // Mostrar popup de confirmación
            }
        }

        // Variable global para guardar el índice de la fila a eliminar
        var currentDeleteIndex = -1;

        // Confirmar eliminación desde el popup
        function ConfirmarEliminacion() {
            if (currentDeleteIndex >= 0) {
                gridSolicitudesRH.PerformCallback('DELETE|' + currentDeleteIndex);
                pcConfirmarEliminacion.Hide();
            }
        }

        // Cancelar eliminación
        function CancelarEliminacion() {
            currentDeleteIndex = -1;
            pcConfirmarEliminacion.Hide();
        }

    </script>

    <!-- Grid de Solicitudes RH -->
    <asp:Table runat="server" Width="90%" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
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
                                <br /><br />
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
                    HeaderText=" " PopupAnimationType="Fade" ShowFooter="true" ShowOnPageLoad="false">
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
                            <dx:ASPxButton ID="btnCerrarErrorSolicitud" runat="server" Text="Cerrar" Width="100px" AutoPostBack="False" BackColor="#353943" ForeColor="White" Font-Bold="true">
                                <ClientSideEvents Click="function(s, e) { pcMensajeErrorSolicitud.Hide(); }" />
                            </dx:ASPxButton>
                        </div>
                    </FooterContentTemplate>
                </dx:ASPxPopupControl>

                <dx:ASPxGridView ID="gridSolicitudesRH" runat="server"
                    KeyFieldName="ID_Solicitud"
                    Width="100%"
                    ForeColor="Black"
                    ClientInstanceName="gridSolicitudesRH"
                    OnDataBinding="gridSolicitudesRH_DataBinding"
                    OnRowInserting="gridSolicitudesRH_RowInserting"
                    OnRowUpdating="gridSolicitudesRH_RowUpdating"
                    OnCustomButtonCallback="gridSolicitudesRH_CustomButtonCallback"
                    OnCustomCallback="gridSolicitudesRH_CustomCallback"
                    OnHtmlEditFormCreated="gridUsuarios_HtmlEditFormCreated">
                    <ClientSideEvents
                        EndCallback="OnGridSolicitudesEndCallback"
                        CustomButtonClick="OnCustomButtonClickSolicitud" />
                    <Styles>
                        <Header BackColor="#353943" ForeColor="White" Font-Bold="true"></Header>
                    </Styles>
                    <Columns>
                        <dx:GridViewCommandColumn Caption="Acciones" Width="100px"
                            ShowNewButtonInHeader="true"
                            ShowEditButton="true"
                            ButtonRenderMode="Image">
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="btnDeleteSolicitud" Text="Eliminar">
                                  <Image Url="~/Images/delete.png" Width="25px" Height="25px" ToolTip="Eliminar" />
                                </dx:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dx:GridViewCommandColumn>

                        <dx:GridViewDataTextColumn FieldName="ID_Solicitud" Caption="ID" Visible="false" ReadOnly="true" />

                        <dx:GridViewDataComboBoxColumn FieldName="id_TipoSolicitud" Caption="Tipo Solicitud" Width="120px">
                            <PropertiesComboBox TextField="Visita" ValueField="ID_TipoVisita" ValueType="System.Int32" />
                        </dx:GridViewDataComboBoxColumn>

                        <dx:GridViewDataTextColumn FieldName="Visitante" Caption="Visitante" Width="150px" />

                        <dx:GridViewDataComboBoxColumn FieldName="id_Proyecto" Caption="Proyecto" Width="150px">
                            <PropertiesComboBox TextField="NombreProyecto" ValueField="ID_Proyecto" ValueType="System.Int32" />
                        </dx:GridViewDataComboBoxColumn>

                        <dx:GridViewDataComboBoxColumn FieldName="id_Planta" Caption="Planta" Width="120px">
                            <PropertiesComboBox TextField="NombrePlanta" ValueField="ID_Planta" ValueType="System.Int32" />
                        </dx:GridViewDataComboBoxColumn>

                        <dx:GridViewDataComboBoxColumn FieldName="id_Contratista" Caption="Contratista" Width="150px">
                            <PropertiesComboBox TextField="Responsable" ValueField="id_contratista" ValueType="System.Int32" />
                        </dx:GridViewDataComboBoxColumn>

                        <dx:GridViewDataTextColumn FieldName="AreaTrabajo" Caption="Área de Trabajo" Width="150px" />

                        <dx:GridViewDataTextColumn FieldName="Actividad" Caption="Actividad" Width="150px" />
                        
                        <dx:GridViewDataTextColumn FieldName="Estancia" Caption="Estancia" Width="100px" />
                        
                        <dx:GridViewDataDateColumn FieldName="FechaInicio" Caption="Fecha Inicio" Width="110px">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" />
                        </dx:GridViewDataDateColumn>
                        
                        <dx:GridViewDataDateColumn FieldName="FechaFin" Caption="Fecha Fin" Width="110px">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" />
                        </dx:GridViewDataDateColumn>
                        
                        <dx:GridViewDataDateColumn FieldName="FechaSolicitud" Caption="Fecha Solicitud" Width="110px" ReadOnly="true">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" />
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataTextColumn FieldName="RFC" Caption="RFC" Width="120px" />

                        <dx:GridViewDataTextColumn FieldName="Responsable" Caption="Responsable" Width="150px" />

                        <dx:GridViewDataCheckColumn FieldName="aprobado" Caption="Aprobado" Width="80px" />

                    </Columns>
                    <SettingsPager PageSize="10">
                        <PageSizeItemSettings ShowAllItem="true" Visible="true"></PageSizeItemSettings>
                    </SettingsPager>
                    <SettingsEditing Mode="PopupEditForm"/>
                    <SettingsPopup>
                        <EditForm Modal="true"
                            Width="700px"
                            HorizontalAlign="WindowCenter"
                            VerticalAlign="WindowCenter" 
                            ShowCloseButton="false"/>
                    </SettingsPopup>
                    <SettingsText PopupEditFormCaption=" " />
                     <SettingsCommandButton>
                        <NewButton>
                            <Image Url="~/Images/add.png" Width="30px" Height="30px" ToolTip="Nueva Solicitud" />
                        </NewButton>
                        <EditButton>
                            <Image Url="~/Images/edits.png" Width="25px" Height="25px" ToolTip="Editar" />
                        </EditButton>
                        <UpdateButton>
                            <Image IconID="actions_apply_16x16" ToolTip="Guardar" />
                        </UpdateButton>
                        <CancelButton>
                            <Image IconID="actions_cancel_16x16" ToolTip="Cancelar" />
                        </CancelButton>
                    </SettingsCommandButton>
                </dx:ASPxGridView>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>