<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" MasterPageFile="~/Root.master" Inherits="DesarrollosQAS.Usuarios" %>

<asp:Content ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript">

        // Manejar mensajes después de operaciones del grid
        function OnGridEndCallback(s, e) {
            if (s.cpMessageType && s.cpMessage) {
                if (s.cpMessageType === "success") {
                    lblMensajeExito.SetText(s.cpMessage);
                    pcMensajeExito.Show();
                } else if (s.cpMessageType === "error") {
                    lblMensajeError.SetText(s.cpMessage);
                    pcMensajeError.Show();
                }

                delete s.cpMessageType;
                delete s.cpMessage;
            }
        }
    </script>

    <div style="padding-top: 8px">
        <dx:ASPxLabel runat="server" ID="ASPxLabel1" Text="Usuarios" Font-Bold="true" Font-Size="X-Large"></dx:ASPxLabel>
    </div>
    <hr />

    <!-- Popup de Éxito -->
    <dx:ASPxPopupControl ID="pcMensajeExito" runat="server" Width="400" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcMensajeExito"
        HeaderText="✓ Operación Exitosa" PopupAnimationType="Fade" ShowFooter="true" ShowOnPageLoad="false">
        <HeaderStyle BackColor="#353943" ForeColor="White" Font-Bold="true" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="padding: 20px; text-align: center;">
                    <dx:ASPxLabel ID="lblMensajeExito" runat="server" Font-Size="14px" ClientInstanceName="lblMensajeExito" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterContentTemplate>
            <div style="text-align: center; padding: 10px;">
                <dx:ASPxButton ID="btnCerrarExito" runat="server" Text="Aceptar" Width="100px" AutoPostBack="False">
                    <ClientSideEvents Click="function(s, e) { pcMensajeExito.Hide(); }" />
                </dx:ASPxButton>
            </div>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

    <!-- Popup de Error -->
    <dx:ASPxPopupControl ID="pcMensajeError" runat="server" Width="400" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcMensajeError"
        HeaderText="✗ Error" PopupAnimationType="Fade" ShowFooter="true" ShowOnPageLoad="false">
        <HeaderStyle BackColor="#dc3545" ForeColor="White" Font-Bold="true" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="padding: 20px; text-align: center;">
                    <dx:ASPxLabel ID="lblMensajeError" runat="server" Font-Size="14px" ClientInstanceName="lblMensajeError" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterContentTemplate>
            <div style="text-align: center; padding: 10px;">
                <dx:ASPxButton ID="btnCerrarError" runat="server" Text="Cerrar" Width="100px" AutoPostBack="False">
                    <ClientSideEvents Click="function(s, e) { pcMensajeError.Hide(); }" />
                </dx:ASPxButton>
            </div>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

    <!-- Grid de Usuarios -->
    <!-- Grid de Usuarios -->
    <dx:ASPxGridView ID="gridUsuarios" runat="server"
        KeyFieldName="id_usuario"
        Width="100%"
        ForeColor="Black"
        ClientInstanceName="gridUsuarios"
        OnDataBinding="gridUsuarios_DataBinding"
        OnRowInserting="gridUsuarios_RowInserting"
        OnRowUpdating="gridUsuarios_RowUpdating"
        OnRowDeleting="gridUsuarios_RowDeleting"
        OnCellEditorInitialize="gridUsuarios_CellEditorInitialize">
        <ClientSideEvents EndCallback="OnGridEndCallback" />
        <Styles>
            <Header BackColor="#353943" ForeColor="White" Font-Bold="true"></Header>
        </Styles>
        <Columns>
            <dx:GridViewCommandColumn Caption="Acciones" Width="100px"
                ShowNewButtonInHeader="true"
                ShowEditButton="true"
                ShowDeleteButton="true"
                ButtonRenderMode="Image">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="id_usuario" Caption="ID" Visible="false" ReadOnly="true" />
            <dx:GridViewDataTextColumn FieldName="sigla_red" Caption="Sigla Red">
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RequiredField IsRequired="true" ErrorText="Sigla de Red requerida" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="nombre" Caption="Nombre">
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RequiredField IsRequired="true" ErrorText="Nombre requerido" />
                        <RegularExpression ValidationExpression="^(?!\s+$).+" ErrorText="El nombre no puede contener solo espacios" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Email" Caption="Email">
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RequiredField IsRequired="true" ErrorText="Email requerido" />
                        <RegularExpression ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorText="Email no válido" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="activo" Caption="Activo" />
        </Columns>
        <SettingsPager PageSize="10" />
        <SettingsEditing Mode="PopupEditForm" />
        <SettingsPopup>
            <EditForm Modal="true"
                Width="500px"
                HorizontalAlign="WindowCenter"
                VerticalAlign="WindowCenter" />
        </SettingsPopup>
        <SettingsText PopupEditFormCaption="Gestión de Usuario" />
        <SettingsCommandButton>
            <NewButton>
                <Image IconID="actions_add_16x16" ToolTip="Nuevo Usuario" />
            </NewButton>
            <EditButton>
                <Image IconID="edit_edit_16x16" ToolTip="Editar" />
            </EditButton>
            <DeleteButton>
                <Image IconID="edit_delete_16x16" ToolTip="Eliminar" />
            </DeleteButton>
            <UpdateButton>
                <Image IconID="actions_apply_16x16" ToolTip="Guardar" />
            </UpdateButton>
            <CancelButton>
                <Image IconID="actions_cancel_16x16" ToolTip="Cancelar" />
            </CancelButton>
        </SettingsCommandButton>
    </dx:ASPxGridView>
</asp:Content>
