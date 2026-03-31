<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RolUsuarios.aspx.cs" Inherits="DesarrollosQAS.Pages.RolUsuarios" MasterPageFile="~/Root.master"%>

<asp:Content ContentPlaceHolderID="Content" runat="server">
    <style>
        /* Wrapper que alinea título, botones Regresar/Guardar y el container al mismo ancho */
        .page-wrapper {
            max-width: 1200px;
            margin: 0 auto;
            padding: 0 3%;
            box-sizing: border-box;
        }

        /* ---- Layout mobile (por defecto: columna) ---- */
        .container {
            display: flex;
            flex-direction: column;
            align-items: center;
            margin-top: 1%;
        }

        .contentEditors {
            width: 100%;
            max-width: 450px;
            height: 250px; /* Mobile */
        }

            /* El ListBox ocupa todo el alto del contenedor */
            .contentEditors .dxlbd {
                height: 100% !important;
            }

        .contentButtons {
            padding-top: 20px;
            padding-bottom: 10px;
            width: 100%;
            max-width: 450px;
            text-align: center;
        }

        .button {
            width: 100% !important;
            margin-top: 3%;
        }

        .buttonGuardarYRegresar{
            width:100px;
        }

        /* ---- Pantallas medianas (790px – 1199px) ---- */
        @media (min-width: 790px) {
            .container {
                flex-direction: row;
                align-items: stretch;
                justify-content: center;
            }

            .contentEditors {
                flex: 1 1 0;
                min-width: 0;
                max-width: none;
                width: auto;
                height: 280px; /* Medianas */
            }

            .contentButtons {
                flex: 0 0 180px;
                max-width: 180px;
                width: 280px;
                display: flex;
                flex-direction: column;
                justify-content: center;
                align-items: center;
                padding: 0 10px;
                margin-right: 20px;
                margin-left: 20px;
            }

            .button {
                width: 142px !important;
            }
        }

        /* ---- Pantallas grandes (1200px+) ---- */
        @media (min-width: 1400px) {
                    /* Wrapper que alinea título, botones Regresar/Guardar y el container al mismo ancho */
        .page-wrapper {
            max-width: 100%;
            margin: 0 auto;
            padding: 0 3%;
            box-sizing: border-box;
        }
            .contentEditors {
                flex: 0 1 550px;
                max-width: 550px;
                height: calc(60vh); /* Grandes */
            }

            .contentButtons {
                margin-right: 40px;
                margin-left: 40px;
            }
        }

        /* Fila de acciones (Regresar / Guardar) */
        .action-row {
            display: flex;
            justify-content: flex-end;
        }
    </style>

    <script type="text/javascript">

        // Recopila todos los elementos actuales de un cuadro de lista como una matriz de {texto, valor}.
        function CollectItems(listBox) {
            var result = [];
            for (var i = 0; i < listBox.GetItemCount(); i++) {
                var item = listBox.GetItem(i);
                result.push({ text: item.text, value: item.value });
            }
            return result;
        }

        // Reconstruye completamente un cuadro de lista dado un nuevo conjunto de elementos, asegurando que el DOM se actualice correctamente.
        function RebuildListBox(listBox, items) {
            listBox.BeginUpdate();
            listBox.ClearItems();
            for (var i = 0; i < items.length; i++) {
                listBox.AddItem(items[i].text, items[i].value);
            }
            listBox.EndUpdate();
        }

        function AddSelectedItems() {
            MoveSelectedItems(lbRolesDisponibles, lbRolesAsignados);
        }
        function AddAllItems() {
            MoveAllItems(lbRolesDisponibles, lbRolesAsignados);
        }
        function RemoveSelectedItems() {
            MoveSelectedItems(lbRolesAsignados, lbRolesDisponibles);
        }
        function RemoveAllItems() {
            MoveAllItems(lbRolesAsignados, lbRolesDisponibles);
        }

        function MoveSelectedItems(srcListBox, dstListBox) {
            var selectedItems = srcListBox.GetSelectedItems();
            if (selectedItems.length === 0) return;

            var selectedIndices = {};
            var itemsToMove = [];
            for (var i = 0; i < selectedItems.length; i++) {
                selectedIndices[selectedItems[i].index] = true;
                itemsToMove.push({ text: selectedItems[i].text, value: selectedItems[i].value });
            }

            var remainingItems = [];
            for (var i = 0; i < srcListBox.GetItemCount(); i++) {
                if (!selectedIndices[i]) {
                    var item = srcListBox.GetItem(i);
                    remainingItems.push({ text: item.text, value: item.value });
                }
            }

            var dstItems = CollectItems(dstListBox);
            for (var i = 0; i < itemsToMove.length; i++) {
                dstItems.push(itemsToMove[i]);
            }

            RebuildListBox(srcListBox, remainingItems);
            RebuildListBox(dstListBox, dstItems);

            UpdateButtonState();
        }

        function MoveAllItems(srcListBox, dstListBox) {
            if (srcListBox.GetItemCount() === 0) return;

            var srcItems = CollectItems(srcListBox);
            var dstItems = CollectItems(dstListBox);
            for (var i = 0; i < srcItems.length; i++) {
                dstItems.push(srcItems[i]);
            }

            RebuildListBox(srcListBox, []);
            RebuildListBox(dstListBox, dstItems);

            UpdateButtonState();
        }
        // Redirigir a la página de Usuarios
        function RegresarRoles() {
            window.location.href = '<%= ResolveUrl("~/Usuarios.aspx") %>';
        }


        function UpdateButtonState() {
            //btnMoveAllItemsToRight.SetEnabled(lbModulosDisponibles.GetItemCount() > 0);
            //btnMoveAllItemsToLeft.SetEnabled(lbModulosAsignados.GetItemCount() > 0);
            //btnMoveSelectedItemsToRight.SetEnabled(lbModulosDisponibles.GetSelectedItems().length > 0);
            //btnMoveSelectedItemsToLeft.SetEnabled(lbModulosAsignados.GetSelectedItems().length > 0);
        }

        function OnSelectedIndexChanged(s, e) {
            UpdateButtonState();
        }

        // Recopila los IDs de los módulos asignados y los envía al servidor vía callback
        function GuardarRolesAsignados() {
            var items = CollectItems(lbRolesAsignados);
            var ids = [];
            for (var i = 0; i < items.length; i++) {
                ids.push(items[i].value);
            }
            cbGuardar.PerformCallback(ids.join(','));
            LoadingPanel.Show();
        }

        // Callback completado: muestra el popup con el resultado
        function OnGuardarCallbackComplete(s, e) {
            var resultado = e.result;
            if (resultado === 'OK') {
                lblPopupMensaje.SetText('Proceso exitoso, se ha asignado los roles');
            } else {
                lblPopupMensaje.SetText('Proceso no exitoso, no se ha asignado los roles');
            }
            LoadingPanel.Hide();
            popupResultado.Show();
        }

        // Al cerrar el popup, refrescar ambos ListBox desde el servidor
        function OnPopupOkClick() {
            popupResultado.Hide();
            cbRefrescar.PerformCallback('');
        }

        // Callback de refresco completado: reconstruir ambos ListBox
        function OnRefrescarCallbackComplete(s, e) {
            var data = JSON.parse(e.result);
            RebuildListBox(lbRolesDisponibles, data.disponibles);
            RebuildListBox(lbRolesAsignados, data.asignados);
            UpdateButtonState();
        }
    </script>

    <dx:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="function(s, e){ UpdateButtonState(); }" />
    </dx:ASPxGlobalEvents>

    <%-- Callback para guardar los módulos asignados --%>
    <dx:ASPxCallback ID="cbGuardar" runat="server" ClientInstanceName="cbGuardar"
        OnCallback="cbGuardar_Callback">
        <ClientSideEvents CallbackComplete="OnGuardarCallbackComplete" />
    </dx:ASPxCallback>

    <%-- Callback para refrescar ambos ListBox después de cerrar el popup --%>
    <dx:ASPxCallback ID="cbRefrescar" runat="server" ClientInstanceName="cbRefrescar"
        OnCallback="cbRefrescar_Callback">
        <ClientSideEvents CallbackComplete="OnRefrescarCallbackComplete" />
    </dx:ASPxCallback>

    <dx:ASPxPopupControl ID="popupResultado" runat="server" Width="400" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupResultado"
        HeaderText=" " PopupAnimationType="Fade" ShowFooter="true" ShowOnPageLoad="false" ShowCloseButton="false">
        <HeaderStyle BackColor="#353943" ForeColor="White" Font-Bold="true" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="padding: 20px; text-align: center;">
                    <dx:ASPxLabel ID="lblPopupMensaje" runat="server" Font-Size="14px" ClientInstanceName="lblPopupMensaje" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterContentTemplate>
            <div style="text-align: center; padding: 10px;">
                <dx:ASPxButton ID="btnPopupOk" runat="server" Text="OK" Width="100px" AutoPostBack="False" BackColor="Teal" ForeColor="White" Font-Bold="true">
                    <ClientSideEvents Click="function(s, e) { OnPopupOkClick(); }" />
                </dx:ASPxButton>
            </div>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

    <div class="page-wrapper">
        <div style=" text-align: center;">
            <dx:ASPxLabel runat="server" ID="lblNombreUsuario" Text="" Font-Bold="true" Font-Size="X-Large"></dx:ASPxLabel>
        </div>
        <div class="action-row">
            <dx:ASPxButton runat="server" ID="btnRegresar" Text="Regresar"
                Width="100" CssClass="btn" BackColor="#353943" ForeColor="White" Font-Bold="true" AutoPostBack="false">
                <ClientSideEvents Click="RegresarRoles" />
            </dx:ASPxButton>
        </div>
    </div>
    <br />
    <div class="page-wrapper">
        <div class="container">
            <div class="contentEditors">
                <dx:ASPxListBox ID="lbRolesDisponibles" runat="server"
                    ClientInstanceName="lbRolesDisponibles" ValueField="id_rol" TextField="nombre_rol"
                    Width="100%" Height="100%" SelectionMode="CheckColumn" Caption="Roles Disponibles" EnableSynchronization="True"
                    Font-Bold="true" Font-Size="Large" CaptionStyle-ForeColor="#666666"
                    ItemStyle-Font-Bold="false" ItemStyle-Font-Size="Medium">
                    <CaptionSettings Position="Top" HorizontalAlign="Center" />
                    <ClientSideEvents SelectedIndexChanged="OnSelectedIndexChanged" />
<%--                    <FilterEditorStyle Font-Bold="false" Font-Size="Small" ></FilterEditorStyle>
                    <FilteringSettings ShowSearchUI="true" EditorNullText="Ingrese el modulo a buscar ..."/>--%>
                </dx:ASPxListBox>
            </div>
            <div class="contentButtons">
                <div>
                    <dx:ASPxButton ID="btnMoveSelectedItemsToRight" runat="server" ClientInstanceName="btnMoveSelectedItemsToRight" CssClass="button"
                        AutoPostBack="False" Text="Asignar >" BackColor="#1773cd"
                        ToolTip="Agregar roles seleccionados">
                        <ClientSideEvents Click="function(s, e) { AddSelectedItems(); }" />
                    </dx:ASPxButton>
                </div>
                <div class="TopPadding">
                    <dx:ASPxButton ID="btnMoveAllItemsToRight" runat="server" ClientInstanceName="btnMoveAllItemsToRight" CssClass="button"
                        AutoPostBack="False" Text="Asignar Todos >>" BackColor="#1773cd" ToolTip="Agregar todos los moodulos">
                        <ClientSideEvents Click="function(s, e) { AddAllItems(); }" />
                    </dx:ASPxButton>
                </div>
<%--                <div style="height: 12px">
                </div>--%>
                <div>
                    <dx:ASPxButton ID="btnMoveSelectedItemsToLeft" runat="server" ClientInstanceName="btnMoveSelectedItemsToLeft" CssClass="button"
                        AutoPostBack="False" Text="< Quitar" BackColor="DarkRed"
                        ToolTip="Quitar roles seleccionados">
                        <ClientSideEvents Click="function(s, e) { RemoveSelectedItems(); }" />
                    </dx:ASPxButton>
                </div>
                <div class="TopPadding">
                    <dx:ASPxButton ID="btnMoveAllItemsToLeft" runat="server" ClientInstanceName="btnMoveAllItemsToLeft" CssClass="button"
                        AutoPostBack="False" Text="<< Quitar todos" BackColor="DarkRed"
                        ToolTip="Quitar todos los roles">
                        <ClientSideEvents Click="function(s, e) { RemoveAllItems(); }" />
                    </dx:ASPxButton>
                </div>
            </div>
            <div class="contentEditors">
                <dx:ASPxListBox ID="lbRolesAsignados" runat="server" ValueField="id_rol" TextField="nombre_rol"
                    ClientInstanceName="lbRolesAsignados" Width="100%" EnableSynchronization="True"
                    Height="100%" SelectionMode="CheckColumn" Caption="Roles Asignados"
                    Font-Bold="true" Font-Size="Large" CaptionStyle-ForeColor="#666666"
                    ItemStyle-Font-Bold="false" ItemStyle-Font-Size="Medium">
                    <CaptionSettings Position="Top" HorizontalAlign="Center" />
                    <ClientSideEvents SelectedIndexChanged="OnSelectedIndexChanged"></ClientSideEvents>
<%--                    <FilteringSettings ShowSearchUI="true" EditorNullText="Ingrese el rol a buscar ..." />
                    <FilterEditorStyle Font-Bold="false" Font-Size="Small" ></FilterEditorStyle>--%>
                </dx:ASPxListBox>
            </div>
        </div>
    </div>
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
        Modal="True" ForeColor="teal" ShowImage="false">
    </dx:ASPxLoadingPanel>
    <div class="page-wrapper">
        <div class="action-row" style="margin-top: 2%; padding-bottom: 20px;">
            <dx:ASPxButton ID="btnGuardar" runat="server" ClientInstanceName="btnGuardar" CssClass="buttonGuardarYRegresar"
                AutoPostBack="False" Text="Guardar" ClientEnabled="True" BackColor="Teal"
                ToolTip="Guarda los roles asignados al usuario">
                <ClientSideEvents Click="function(s, e) { GuardarRolesAsignados(); }" />
            </dx:ASPxButton>
        </div>
    </div>

</asp:Content>
