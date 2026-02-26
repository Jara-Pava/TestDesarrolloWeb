<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" MasterPageFile="~/Root.master" Inherits="DesarrollosQAS.Usuarios" %>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    <div class="card" style="padding:10px;margin-bottom:15px;">
        <div class="row">
            <div class="col">
                <dx:ASPxTextBox ID="txtNombre" runat="server" Caption="Nombre" Width="300px">
                    <ValidationSettings RequiredField-IsRequired="true" />
                </dx:ASPxTextBox>
            </div>
            <div class="col">
                <dx:ASPxTextBox ID="txtSigla" runat="server" Caption="Sigla Red" Width="200px">
                    <ValidationSettings RequiredField-IsRequired="true" />
                </dx:ASPxTextBox>
            </div>
            <div class="col">
                <dx:ASPxTextBox ID="txtEmail" runat="server" Caption="Email" Width="300px">
                    <ValidationSettings RequiredField-IsRequired="true">
                        <RegularExpression ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorText="Email no válido" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </div>
            <div class="col">
                <dx:ASPxCheckBox ID="chkActivo" runat="server" Checked="true" Text="Activo" />
            </div>
            <div class="col">
                <dx:ASPxButton ID="btnGuardarNuevo" runat="server" Text="Crear usuario" OnClick="btnGuardarNuevo_Click" />
            </div>
        </div>
        <dx:ASPxLabel ID="lblMensaje" runat="server" ForeColor="Red" />
    </div>
    <dx:ASPxGridView ID="gridUsuarios" runat="server"
        KeyFieldName="id_usuario" Width="100%" ForeColor="Black"
        OnDataBinding="gridUsuarios_DataBinding">
        <Columns>
            <dx:GridViewCommandColumn Caption="Acciones" Width="120px"
                ShowEditButton="true" ShowDeleteButton="true" />
            <dx:GridViewDataTextColumn FieldName="id_usuario" Caption="ID" Visible="false" ReadOnly="true" />
            <dx:GridViewDataTextColumn FieldName="nombre" Caption="Nombre" />
            <dx:GridViewDataTextColumn FieldName="sigla_red" Caption="Sigla Red" />
            <dx:GridViewDataCheckColumn FieldName="activo" Caption="Activo" />
            <dx:GridViewDataTextColumn FieldName="Email" Caption="Email" />
        </Columns>
        <SettingsEditing Mode="Inline" />
    </dx:ASPxGridView>

</asp:Content>