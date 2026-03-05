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

        // Confirmar antes de eliminar
        function OnCustomButtonClick(s, e) {
            if (e.buttonID === 'btnDelete') {
                e.processOnServer = confirm('¿Está seguro que desea eliminar este usuario?\n\nEsta acción no se puede deshacer.');
            }
        }

        // Prevenir Enter en el formulario de edición
        function OnEditFormKeyDown(s, e) {
            if (e.htmlEvent.keyCode === 13) {
                e.htmlEvent.preventDefault();
                return false;
            }
        }

        // Guardar cambios del formulario de edición
        function GuardarUsuario(s, e) {
            if (ASPxClientEdit.ValidateGroup('EditForm')) {
                gridUsuarios.UpdateEdit();
            }
        }

        // Cancelar edición
        function CancelarEdicion(s, e) {
            gridUsuarios.CancelEdit();
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
    <dx:ASPxGridView ID="gridUsuarios" runat="server"
        KeyFieldName="id_usuario"
        Width="100%"
        ForeColor="Black"
        ClientInstanceName="gridUsuarios"
        OnDataBinding="gridUsuarios_DataBinding"
        OnRowInserting="gridUsuarios_RowInserting"
        OnRowUpdating="gridUsuarios_RowUpdating"
        OnCustomButtonCallback="gridUsuarios_CustomButtonCallback"
        OnHtmlEditFormCreated="gridUsuarios_HtmlEditFormCreated">
        <ClientSideEvents 
            EndCallback="OnGridEndCallback" 
            CustomButtonClick="OnCustomButtonClick" />
        <Styles>
            <Header BackColor="#353943" ForeColor="White" Font-Bold="true"></Header>
        </Styles>
        <Columns>
            <dx:GridViewCommandColumn Caption="Acciones" Width="100px"
                ShowNewButtonInHeader="true"
                ShowEditButton="true"
                ButtonRenderMode="Image">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnDelete" Text="Eliminar">
                        <Image IconID="edit_delete_16x16" ToolTip="Eliminar" />
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="id_usuario" Caption="ID" Visible="false" ReadOnly="true" />
            <dx:GridViewDataTextColumn FieldName="sigla_red" Caption="Sigla Red">
                <PropertiesTextEdit NullText="Ingrese sigla de red">
                    <ClientSideEvents KeyDown="OnEditFormKeyDown" />
                    <ValidationSettings>
                        <RequiredField IsRequired="true" ErrorText="Sigla de Red requerida" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="nombre" Caption="Nombre">
                <PropertiesTextEdit NullText="Ingrese nombre completo">
                    <ClientSideEvents KeyDown="OnEditFormKeyDown" />
                    <ValidationSettings>
                        <RequiredField IsRequired="true" ErrorText="Nombre requerido" />
                        <RegularExpression ValidationExpression="^(?!\s+$).+" ErrorText="El nombre no puede contener solo espacios" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Email" Caption="Email">
                <PropertiesTextEdit NullText="ejemplo@correo.com">
                    <ClientSideEvents KeyDown="OnEditFormKeyDown" />
                    <ValidationSettings>
                        <RequiredField IsRequired="true" ErrorText="Email requerido" />
                        <RegularExpression ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorText="Email no válido" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="activo" Caption="Activo" />
        </Columns>
        
        <Templates>
            <EditForm>
                <div style="padding: 20px;">
                    <table style="width: 100%; border-collapse: collapse;">
                        <tr>
                            <td style="width: 50%; padding: 10px; vertical-align: top;">
                                <dx:ASPxLabel ID="lblSiglaRed" runat="server" Text="Sigla Red:" AssociatedControlID="txtSiglaRed" />
                                <span style="color: red;">*</span>
                                <br />
                                <dx:ASPxTextBox ID="txtSiglaRed" runat="server" Width="95%" 
                                    ClientInstanceName="txtSiglaRed"
                                    Text='<%# Bind("sigla_red") %>' 
                                    NullText="Ingrese sigla de red">
                                    <ClientSideEvents KeyDown="OnEditFormKeyDown" />
                                    <ValidationSettings ValidationGroup="EditForm" Display="Dynamic">
                                        <RequiredField IsRequired="true" ErrorText="Sigla de Red requerida" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td style="width: 50%; padding: 10px; vertical-align: top;">
                                <dx:ASPxLabel ID="lblNombre" runat="server" Text="Nombre:" AssociatedControlID="txtNombre" />
                                <span style="color: red;">*</span>
                                <br />
                                <dx:ASPxTextBox ID="txtNombre" runat="server" Width="95%" 
                                    Text='<%# Bind("nombre") %>' 
                                    NullText="Ingrese nombre completo">
                                    <ClientSideEvents KeyDown="OnEditFormKeyDown" />
                                    <ValidationSettings ValidationGroup="EditForm" Display="Dynamic">
                                        <RequiredField IsRequired="true" ErrorText="Nombre requerido" />
                                        <RegularExpression ValidationExpression="^(?!\s+$).+" ErrorText="El nombre no puede contener solo espacios" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%; padding: 10px; vertical-align: top;">
                                <dx:ASPxLabel ID="lblEmail" runat="server" Text="Email:" AssociatedControlID="txtEmail" />
                                <span style="color: red;">*</span>
                                <br />
                                <dx:ASPxTextBox ID="txtEmail" runat="server" Width="95%" 
                                    Text='<%# Bind("Email") %>' 
                                    NullText="ejemplo@correo.com">
                                    <ClientSideEvents KeyDown="OnEditFormKeyDown" />
                                    <ValidationSettings ValidationGroup="EditForm" Display="Dynamic">
                                        <RequiredField IsRequired="true" ErrorText="Email requerido" />
                                        <RegularExpression ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorText="Email no válido" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td style="width: 50%; padding: 10px; vertical-align: top;">
                                <dx:ASPxLabel ID="lblActivo" runat="server" Text="Activo:" AssociatedControlID="chkActivo" />
                                <br />
                                <dx:ASPxCheckBox ID="chkActivo" runat="server" 
                                    Checked='<%# Eval("activo") == null ? true : (bool)Eval("activo") %>'
                                    Text="" />
                            </td>
                        </tr>
                    </table>
                    
                    <div style="text-align: center; margin-top: 20px; padding-top: 15px; border-top: 1px solid #ddd;">
                        <dx:ASPxButton ID="btnUpdate" runat="server" Text="Guardar" Width="120px" AutoPostBack="false">
                            <Image IconID="actions_apply_16x16" />
                            <ClientSideEvents Click="GuardarUsuario" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancelar" Width="120px" AutoPostBack="false" Style="margin-left: 10px;">
                            <Image IconID="actions_cancel_16x16" />
                            <ClientSideEvents Click="CancelarEdicion" />
                        </dx:ASPxButton>
                    </div>
                </div>
            </EditForm>
        </Templates>
        
        <SettingsPager PageSize="10" />
        <SettingsEditing Mode="PopupEditForm" />
        <SettingsPopup>
            <EditForm Modal="true"
                Width="650px"
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
            <UpdateButton>
                <Image IconID="actions_apply_16x16" ToolTip="Guardar" />
            </UpdateButton>
            <CancelButton>
                <Image IconID="actions_cancel_16x16" ToolTip="Cancelar" />
            </CancelButton>
        </SettingsCommandButton>
    </dx:ASPxGridView>
</asp:Content>