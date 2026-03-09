<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudEspecial.aspx.cs" Inherits="DesarrollosQAS.SolicitudEspecial" MasterPageFile="~/Root.master" %>

<asp:Content ID="contentSolicitudEspecial" ContentPlaceHolderID="Content" runat="Server">
    <style type="text/css">
        tr > .dxflCaptionCell_Office365{
            padding-bottom: 15px !important;
        }
    </style>
    <div style="padding-top: 20px; padding-left: 20px; text-align: left">
        <dx:ASPxButton runat="server" ID="btnRegresarSolicitudesEspeciales" Text="Regresar"
            Width="200px" CssClass="btn" BackColor="#353943" ForeColor="White" Font-Bold="true" />
    </div>
    <%--Template para solicitud especial, se pueden agregar o quitar campos segun sea necesario --%>
    <dx:ASPxFormLayout runat="server" ID="exampleFormLayout" Paddings-PaddingTop="20px" RequiredMarkDisplayMode="RequiredOnly" EnableViewState="false" EncodeHtml="false" UseDefaultPaddings="false" Width="100%">
        <Styles LayoutGroupBox-Caption-Font-Size="X-Large" LayoutGroupBox-Caption-Font-Bold="true" LayoutGroupBox-Caption-ForeColor="#353943"></Styles>
        <Items>
            <dx:LayoutGroup Caption="Solicitud Especial" ColumnCount="4" SettingsItemCaptions-Location="Top" CellStyle-Font-Size="14px">
                <CellStyle Font-Size="14px" />
                <Items>
                    <%--Combobox Solicitud--%>
                    <dx:LayoutItem Caption="Tipo Solicitud" ColumnSpan="4" Visible="false">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                               <dx:ASPxTextBox runat="server" ID="txtIdSolicitud" Caption="ID" Visible="false" ReadOnly="true"></dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--Combobox Tipo Solicitud--%>
                    <dx:LayoutItem Caption="Tipo Solicitud" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="cboTipoSolicitud" TextField="TipoSolicitud" ValueField="ID_TipoVisita" ValueType="System.Int32" runat="server" Width="100%"></dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--Combobox Proyecto--%>
                    <dx:LayoutItem Caption="Proyecto" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="cboProyecto" TextField="NombreProyecto" ValueField="ID_Proyecto" ValueType="System.Int32" runat="server" Width="100%"></dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--Textbox Visitante--%>
                    <dx:LayoutItem Caption="Visitante" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtVisitante" runat="server" Width="100%" />
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--ComboBox Planta--%>
                    <dx:LayoutItem Caption="Planta" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="cboPlanta" TextField="NombrePlanta" ValueField="ID_Planta" ValueType="System.Int32" runat="server" Width="100%"></dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--ComboBox Contratista--%>
                    <dx:LayoutItem Caption="Contratista" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="cboContratista" TextField="Responsable" ValueField="id_contratista" ValueType="System.Int32" runat="server" Width="100%"></dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--Textbox Area de Trabajo--%>
                    <dx:LayoutItem Caption="Area de Trabajo" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtAreaTrabajo" runat="server" Width="100%" />
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--Memo Actividad--%>
                    <dx:LayoutItem Caption="Actividad" ColumnSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxMemo ID="txtActividad" runat="server" Width="100%" Rows="3" />
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--Textbox Visitante--%>
                    <dx:LayoutItem Caption="Responsable" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox ID="txtResponsable" runat="server" Width="100%" />
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--DateEdit Fecha Inicio--%>
                    <dx:LayoutItem Caption="Fecha Inicio" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxDateEdit ID="dteFechaInicio" runat="server" EditFormat="Date" Width="100%"></dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--DateEdit Fecha Fin--%>
                    <dx:LayoutItem Caption="Fecha Fin" ColumnSpan="2">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxDateEdit ID="dteFechaFin" runat="server" EditFormat="Date" Width="100%"></dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--CheckBox Aprobado--%>
                    <dx:LayoutItem Caption="Aprobado" ColumnSpan="2" CaptionSettings-Location="Left">
                        <ParentContainerStyle Paddings-PaddingRight="12"></ParentContainerStyle>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxCheckBox ID="chkAprobado" runat="server"></dx:ASPxCheckBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--Botón Cancelar--%>
                    <dx:LayoutItem HorizontalAlign="Left" ShowCaption="False" ColumnSpan="2">
                        <Paddings PaddingTop="20" PaddingRight="12" />
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxButton runat="server" ID="btnCancelar" Text="Cancelar"
                                    Width="200px" CssClass="btn" BackColor="#353943" ForeColor="White" Font-Bold="true" />
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%--Botón Guardar--%>
                    <dx:LayoutItem HorizontalAlign="Right" ShowCaption="False" ColumnSpan="2">
                        <Paddings PaddingTop="20" />
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxButton runat="server" ID="btnGuardar" Text="Guardar"
                                    Width="200px" CssClass="btn" BackColor="#353943" ForeColor="White" Font-Bold="true" />
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                </Items>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>
</asp:Content>
<%-- .dxflItemSys.dxflCLTSys > div.dxflCaptionCell_Office365
{
    padding: 6px 0 4px 0;
    padding-bottom: 20p;
} --%>