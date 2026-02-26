<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" MasterPageFile="~/Root.master" Inherits="DesarrollosQAS.Usuarios" %>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    <dx:ASPxGridView ID="gridUsuarios" runat="server" KeyFieldName="id_usuario" Width="100%"  ForeColor="Black">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="id_usuario" Caption="ID" Visible="false" />
            <dx:GridViewDataTextColumn FieldName="nombre" Caption="Nombre" />
            <dx:GridViewDataTextColumn FieldName="sigla_red" Caption="Sigla Red" />
            <dx:GridViewDataCheckColumn FieldName="activo" Caption="Activo" />
            <dx:GridViewDataTextColumn FieldName="Email" Caption="Email" />
        </Columns>
        <Settings ShowFilterRow="false" ShowGroupPanel="false" />
    </dx:ASPxGridView>
</asp:Content>