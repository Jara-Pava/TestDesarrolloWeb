<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesRH.aspx.cs" MasterPageFile="~/Root.master" Inherits="DesarrollosQAS.SolicitudesRH" %>

<asp:Content ContentPlaceHolderID="Content" runat="server">
    <!-- Grid de Solicitudes RH -->
    <asp:Table runat="server" Width="90%" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
                <div style="padding-top: 8px">
                    <dx:ASPxLabel runat="server" ID="ASPxLabel7" Text="Solicitudes de Visitas" Font-Bold="true" Font-Size="X-Large"></dx:ASPxLabel>
                </div>
                <hr />
                <dx:ASPxGridView ID="gridSolicitudesRH" runat="server"
                    KeyFieldName="ID_Solicitud"
                    Width="100%"
                    ForeColor="Black"
                    OnDataBinding="gridSolicitudesRH_DataBinding"
                    OnRowInserting="gridSolicitudesRH_RowInserting"
                    OnRowUpdating="gridSolicitudesRH_RowUpdating"
                    OnRowDeleting="gridSolicitudesRH_RowDeleting">
                    <Styles>
                        <Header BackColor="#353943" ForeColor="White" Font-Bold="true"></Header>
                    </Styles>
                    <Columns>
                        <dx:GridViewCommandColumn Caption="Acciones" Width="140px" ShowNewButtonInHeader="true"
                            ShowEditButton="true" ShowDeleteButton="true" />

                        <dx:GridViewDataTextColumn FieldName="ID_Solicitud" Caption="ID" Visible="false" ReadOnly="true" />

                        <dx:GridViewDataTextColumn FieldName="Visitante" Caption="Visitante" Width="150px" />

                        <dx:GridViewDataComboBoxColumn FieldName="id_TipoSolicitud" Caption="Tipo Solicitud" Width="120px">
                            <PropertiesComboBox TextField="Visita" ValueField="ID_TipoVisita" ValueType="System.Int32" />
                        </dx:GridViewDataComboBoxColumn>

                        <dx:GridViewDataComboBoxColumn FieldName="id_Proyecto" Caption="Proyecto" Width="150px">
                            <PropertiesComboBox TextField="NombreProyecto" ValueField="ID_Proyecto" ValueType="System.Int32" />
                        </dx:GridViewDataComboBoxColumn>

                        <dx:GridViewDataComboBoxColumn FieldName="id_Planta" Caption="Planta" Width="120px">
                            <PropertiesComboBox TextField="NombrePlanta" ValueField="ID_Planta" ValueType="System.Int32" />
                        </dx:GridViewDataComboBoxColumn>

                        <dx:GridViewDataDateColumn FieldName="FechaInicio" Caption="Fecha Inicio" Width="110px">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" />
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataDateColumn FieldName="FechaFin" Caption="Fecha Fin" Width="110px">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" />
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataTextColumn FieldName="RFC" Caption="RFC" Width="120px" />

                        <dx:GridViewDataComboBoxColumn FieldName="id_Contratista" Caption="Contratista" Width="150px">
                            <PropertiesComboBox TextField="Responsable" ValueField="id_contratista" ValueType="System.Int32" />
                        </dx:GridViewDataComboBoxColumn>

                        <dx:GridViewDataTextColumn FieldName="Responsable" Caption="Responsable" Width="150px" />

                        <dx:GridViewDataTextColumn FieldName="AreaTrabajo" Caption="Área de Trabajo" Width="150px" />

                        <dx:GridViewDataTextColumn FieldName="Actividad" Caption="Actividad" Width="150px" />

                        <dx:GridViewDataTextColumn FieldName="Estancia" Caption="Estancia" Width="100px" />

                        <dx:GridViewDataDateColumn FieldName="FechaSolicitud" Caption="Fecha Solicitud" Width="110px">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" />
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataCheckColumn FieldName="aprobado" Caption="Aprobado" Width="80px" />

                    </Columns>
                    <SettingsPager PageSize="10">
                        <PageSizeItemSettings ShowAllItem="true" Visible="true"></PageSizeItemSettings>
                    </SettingsPager>
                    <SettingsEditing Mode="EditForm" />
                    <SettingsPopup>
                        <EditForm Modal="true" Width="600px" />
                    </SettingsPopup>
                </dx:ASPxGridView>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>