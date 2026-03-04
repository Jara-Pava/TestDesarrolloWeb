<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" MasterPageFile="~/Root.master" Inherits="DesarrollosQAS.Usuarios" %>

<asp:Content ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript">
        function MostrarModalCrearUsuario() {
            // Limpiar formulario antes de mostrar
            ASPxClientEdit.ClearGroup('entryGroup');
            pcCrearUsuario.Show();
        }

        function LimpiarYCerrarPopup() {
            // Limpiar validaciones
            ASPxClientEdit.ClearGroup('entryGroup');
            // Cerrar popup
            pcCrearUsuario.Hide();
        }

        // Manejar mensajes después de operaciones del grid
        function OnGridEndCallback(s, e) {
            // Verificar si hay un mensaje del servidor
            if (s.cpMessageType && s.cpMessage) {
                if (s.cpMessageType === "success") {
                    lblMensajeExito.SetText(s.cpMessage);
                    pcMensajeExito.Show();
                } else if (s.cpMessageType === "error") {
                    lblMensajeError.SetText(s.cpMessage);
                    pcMensajeError.Show();
                }

                // Limpiar las propiedades
                delete s.cpMessageType;
                delete s.cpMessage;
            }
        }
    </script>

    <div style="padding-top: 8px">
        <dx:ASPxLabel runat="server" ID="ASPxLabel1" Text="Usuarios" Font-Bold="true" Font-Size="X-Large"></dx:ASPxLabel>
    </div>

    <!-- Botón para mostrar el popup de creación de usuario -->
    <div style="margin: 16px 0; width: 160px; text-align: left; background">
        <dx:ASPxButton ID="btMostrarModalCrearUsuario" runat="server" Text="Crear Usuario" AutoPostBack="False" UseSubmitBehavior="false" Width="100%">
            <ClientSideEvents Click="function(s, e) { MostrarModalCrearUsuario(); }" />
        </dx:ASPxButton>
    </div>

    <!-- Popup para Crear Usuario -->
    <dx:ASPxPopupControl ID="pcCrearUsuario" runat="server" Width="400" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcCrearUsuario" HeaderText="Dar de Alta a un Usuario"
        AllowDragging="True" PopupAnimationType="Fade" EnableViewState="False" AutoUpdatePosition="True" ShowOnPageLoad="false">
        <HeaderStyle BackColor="#353943" ForeColor="White" Font-Bold="true" />
        <ClientSideEvents
            PopUp="function(s,e) { ASPxClientEdit.ClearGroup('entryGroup'); tbNombre.Focus(); }"
            CloseUp="function(s,e) { ASPxClientEdit.ClearGroup('entryGroup'); }" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="PanelCrearUsuario" runat="server" DefaultButton="btnCrearUsuario">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <dx:ASPxFormLayout runat="server" ID="ASPxFormLayoutCrearUsuario" Width="100%" Height="100%">
                                <Items>
                                    <dx:LayoutItem Caption="Nombre">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="tbNombre" runat="server" Width="100%" ClientInstanceName="tbNombre">
                                                    <ValidationSettings EnableCustomValidation="True" ValidationGroup="entryGroup" SetFocusOnError="True"
                                                        ErrorDisplayMode="Text" ErrorTextPosition="Bottom">
                                                        <RequiredField ErrorText="Nombre requerido" IsRequired="True" />
                                                        <RegularExpression ValidationExpression="^(?!\s+$).+" ErrorText="El nombre no puede contener solo espacios" />
                                                        <ErrorFrameStyle Font-Size="10px">
                                                            <ErrorTextPaddings PaddingLeft="0px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>

                                    <dx:LayoutItem Caption="Sigla de Red">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="tbSigla" runat="server" Width="100%">
                                                    <ValidationSettings EnableCustomValidation="True" ValidationGroup="entryGroup" SetFocusOnError="True"
                                                        ErrorDisplayMode="Text" ErrorTextPosition="Bottom">
                                                        <RequiredField ErrorText="Sigla de Red requerido" IsRequired="True" />
                                                        <ErrorFrameStyle Font-Size="10px">
                                                            <ErrorTextPaddings PaddingLeft="0px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>

                                    <dx:LayoutItem Caption="Email">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="tbEmail" runat="server" Width="100%">
                                                    <ValidationSettings EnableCustomValidation="True" ValidationGroup="entryGroup" SetFocusOnError="True"
                                                        ErrorDisplayMode="Text" ErrorTextPosition="Bottom">
                                                        <RequiredField ErrorText="Email requerido" IsRequired="True" />
                                                        <RegularExpression ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorText="Email no válido" />
                                                        <ErrorFrameStyle Font-Size="10px">
                                                            <ErrorTextPaddings PaddingLeft="0px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>

                                    <dx:LayoutItem Caption="Activo">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxCheckBox ID="chbActivo" runat="server" Checked="true" />
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>

                                    <dx:LayoutItem ShowCaption="False">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <div style="text-align: center; padding-top: 10px;">
                                                    <dx:ASPxButton ID="btnCrearUsuario" runat="server" Text="Crear" Width="80px" AutoPostBack="True" Style="display: inline-block; margin-right: 8px" OnClick="btnCrearUsuario_Click">
                                                        <ClientSideEvents Click="function(s, e) { if(!ASPxClientEdit.ValidateGroup('entryGroup')) { e.processOnServer = false; } }" />
                                                    </dx:ASPxButton>
                                                    <dx:ASPxButton ID="btCancel" runat="server" Text="Cancelar" Width="80px" AutoPostBack="False" Style="display: inline-block;">
                                                        <ClientSideEvents Click="function(s, e) { LimpiarYCerrarPopup(); }" />
                                                    </dx:ASPxButton>
                                                </div>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>

                                </Items>
                            </dx:ASPxFormLayout>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <!-- Popup de Éxito -->
    <dx:ASPxPopupControl ID="pcMensajeExito" runat="server" Width="400" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcMensajeExito"
        HeaderText="✓ Operación Exitosa" PopupAnimationType="Fade" ShowFooter="true" ShowOnPageLoad="false">
        <HeaderStyle BackColor="#353943" ForeColor="White" Font-Bold="true" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="padding: 20px; text-align: center;">
                    <dx:ASPxImage ID="imgExito" runat="server" Width="48px" Height="48px" Style="margin-bottom: 15px;">
                        <Border BorderWidth="0" />
                    </dx:ASPxImage>
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
        <HeaderStyle BackColor="#353943" ForeColor="White" Font-Bold="true" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="padding: 20px; text-align: center;">
                    <dx:ASPxImage ID="imgError" runat="server" Width="48px" Height="48px" Style="margin-bottom: 15px;">
                        <Border BorderWidth="0" />
                    </dx:ASPxImage>
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
        OnDataBinding="gridUsuarios_DataBinding"
        OnRowInserting="gridUsuarios_RowInserting"
        OnRowUpdating="gridUsuarios_RowUpdating"
        OnRowDeleting="gridUsuarios_RowDeleting"
        OnHtmlRowPrepared="gridUsuarios_HtmlRowPrepared"
        OnCellEditorInitialize="gridUsuarios_CellEditorInitialize">
        <ClientSideEvents EndCallback="OnGridEndCallback" />
        <Styles>
            <Header BackColor="#353943" ForeColor="White" Font-Bold="true"></Header>
        </Styles>
        <Columns>
            <dx:GridViewCommandColumn Caption="Acciones" Width="100px"
                ShowEditButton="true"
                ShowDeleteButton="true"
                ButtonRenderMode="Image">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="id_usuario" Caption="ID" Visible="false" ReadOnly="true" />
            <dx:GridViewDataTextColumn FieldName="nombre" Caption="Nombre">
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RequiredField IsRequired="true" ErrorText="Nombre requerido" />
                        <RegularExpression ValidationExpression="^(?!\s+$).+" ErrorText="El nombre no puede contener solo espacios" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="sigla_red" Caption="Sigla Red">
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RequiredField IsRequired="true" ErrorText="Sigla de Red requerida" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="activo" Caption="Activo" />
            <dx:GridViewDataTextColumn FieldName="Email" Caption="Email">
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RequiredField IsRequired="true" ErrorText="Email requerido" />
                        <RegularExpression ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorText="Email no válido" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPager PageSize="10" />
        <SettingsEditing Mode="EditForm" />
        <SettingsPopup>
            <EditForm Modal="true" />
        </SettingsPopup>
        <SettingsCommandButton>
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
