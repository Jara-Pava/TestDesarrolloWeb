<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" MasterPageFile="~/Root.master" Inherits="DesarrollosQAS.Usuarios" %>

<asp:Content ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript">
        function MostrarModalCrearUsuario() {
            pcCrearUsuario.Show();
        }
    </script>

    <div style="margin: 16px auto; width: 160px;">
        <dx:ASPxButton ID="btMostrarModalCrearUsuario" runat="server" Text="Show Modal Window" AutoPostBack="False" UseSubmitBehavior="false" Width="100%">
            <ClientSideEvents Click="function(s, e) { MostrarModalCrearUsuario(); }" />
        </dx:ASPxButton>
    </div>

    <dx:ASPxPopupControl ID="pcCrearUsuario" runat="server" Width="320" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcCrearUsuario" HeaderText="CrearUsuario"
        AllowDragging="True" PopupAnimationType="Fade" EnableViewState="False" AutoUpdatePosition="True">
        <ClientSideEvents PopUp="function(s,e) {ASPxClientEdit.ClearGroup('entryGroup'); tbNombre.Focus(); }" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="PanelCrearUsuario" runat="server" DefaultButton="btOK">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <dx:ASPxFormLayout runat="server" ID="ASPxFormLayoutCrearUsuario" Width="100%" Height="100%">
                                <Items>
                                    <dx:LayoutItem Caption="Nombre">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxTextBox ID="tbNombre" runat="server" Width="100%" ClientInstanceName="tbNombre">
                                                    <ValidationSettings EnableCustomValidation="True" ValidationGroup="entryGroup" SetFocusOnError="True"
                                                        ErrorDisplayMode="Text" ErrorTextPosition="Bottom">
                                                        <RequiredField ErrorText="Nombre requerido" IsRequired="True" />
                                                        <RegularExpression ErrorText="Creacion Usuario requerido" />
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
                                            <dx:LayoutItemNestedControlContainer>
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
                                            <dx:LayoutItemNestedControlContainer>
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
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxCheckBox ID="chbActivo" runat="server" Checked="true" />
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>


                                    <dx:LayoutItem ShowCaption="False">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxButton ID="btnCrearUsuario" runat="server" Text="Crear" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px" OnClick="btnCrearUsuario_Click">
                                                    <ClientSideEvents Click="function(s, e) { if(ASPxClientEdit.ValidateGroup('entryGroup')) pcCrearUsuario.Hide(); }" />
                                                </dx:ASPxButton>

<%--                                                <dx:ASPxButton ID="btnCrearUsuario" runat="server" Text="Crear" AutoPostBack="False" 
                                                    CssClass="dx-btn-primary" OnClick="btnCrearUsuario_Click" />--%>
                                                <dx:ASPxButton ID="btCancel" runat="server" Text="Cancelar" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px">
                                                    <ClientSideEvents Click="function(s, e) { pcCrearUsuario.Hide(); }" />
                                                </dx:ASPxButton>
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

    <dx:ASPxGridView ID="gridUsuarios" runat="server"
        KeyFieldName="id_usuario"
        Width="100%" ForeColor="Black"
        OnRowInserting="gridUsuarios_RowInserting">
        <Columns>
            <dx:GridViewCommandColumn Caption="Acciones" Width="140px"
                ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true" />
            <dx:GridViewDataTextColumn FieldName="id_usuario" Caption="ID" Visible="false" ReadOnly="true" />
            <dx:GridViewDataTextColumn FieldName="nombre" Caption="Nombre" />
            <dx:GridViewDataTextColumn FieldName="sigla_red" Caption="Sigla Red" />
            <dx:GridViewDataCheckColumn FieldName="activo" Caption="Activo" />
            <dx:GridViewDataTextColumn FieldName="Email" Caption="Email" />
        </Columns>
    </dx:ASPxGridView>

</asp:Content>
