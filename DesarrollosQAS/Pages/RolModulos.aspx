<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RolModulos.aspx.cs" Inherits="DesarrollosQAS.Pages.RolModulos" MasterPageFile="~/Root.master" %>

<asp:Content ContentPlaceHolderID="Content" runat="server">
    <style>
        .container {
            display: table;
            margin-top: 5%;
            margin-left: auto;
            margin-right: auto;
        }

        .contentButtons {
            padding-top: 20px;
            padding-bottom: 10px;
        }

        .button {
            width: 100% !important;
            min-height: calc(5vh);
            margin-top: 3%;
            background-color: teal;
        }

        @media(min-width:790px) {
            .contentEditors, .contentButtons {
                display: table-cell;
                width: 33.33333333%;
            }

            .button {
                width: 170px !important;
            }

            .contentEditors {
                vertical-align: top;
            }

            .contentButtons {
                vertical-align: middle;
                text-align: center;
            }
        }
    </style>

    <script type="text/javascript">

        var _updateTimer = null;

        function AddSelectedItems() {
            MoveSelectedItems(lbModulosDisponibles, lbModulosAsignados);
            UpdateButtonState();
        }
        function AddAllItems() {
            MoveAllItems(lbModulosDisponibles, lbModulosAsignados);
            UpdateButtonState();
        }
        function RemoveSelectedItems() {
            MoveSelectedItems(lbModulosAsignados, lbModulosDisponibles);
            UpdateButtonState();
        }
        function RemoveAllItems() {
            MoveAllItems(lbModulosAsignados, lbModulosDisponibles);
            UpdateButtonState();
        }
        function MoveSelectedItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            dstListBox.BeginUpdate();
            var items = srcListBox.GetSelectedItems();
            for (var i = items.length - 1; i >= 0; i = i - 1) {
                dstListBox.AddItem(items[i].text, items[i].value);
                srcListBox.RemoveItem(items[i].index);
            }
            srcListBox.EndUpdate();
            dstListBox.EndUpdate();
        }
        function MoveAllItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            var count = srcListBox.GetItemCount();
            for (var i = 0; i < count; i++) {
                var item = srcListBox.GetItem(i);
                dstListBox.AddItem(item.text, item.value);
            }
            srcListBox.EndUpdate();
            srcListBox.ClearItems();
        }
        function UpdateButtonState() {
            if (_updateTimer) {
                clearTimeout(_updateTimer);
            }
            _updateTimer = setTimeout(function () {
                btnMoveAllItemsToRight.SetEnabled(lbModulosDisponibles.GetItemCount() > 0);
                btnMoveAllItemsToLeft.SetEnabled(lbModulosAsignados.GetItemCount() > 0);
                btnMoveSelectedItemsToRight.SetEnabled(lbModulosDisponibles.GetSelectedItems().length > 0);
                btnMoveSelectedItemsToLeft.SetEnabled(lbModulosAsignados.GetSelectedItems().length > 0);
                _updateTimer = null;
            }, 100);
        }

        function RefreshCheckBoxes(listBox) {
            var mainElement = listBox.GetMainElement();
            if (!mainElement) return;
            var checkBoxes = mainElement.querySelectorAll('.dxEditorCell .dxeCheckBoxChecked, .dxEditorCell .dxeCheckBoxUnchecked');
            var selectedIndices = {};
            var selectedItems = listBox.GetSelectedItems();
            for (var i = 0; i < selectedItems.length; i++) {
                selectedIndices[selectedItems[i].index] = true;
            }
            for (var j = 0; j < listBox.GetItemCount(); j++) {
                var item = listBox.GetItem(j);
                var row = mainElement.querySelectorAll('.dxlbd tr.dxeListBoxItem')[j]
                    || mainElement.querySelectorAll('tr.dxeListBoxItemRow')[j];
                if (!row) continue;
                var cb = row.querySelector('[class*="dxeCheckBox"]');
                if (!cb) continue;
                if (selectedIndices[j]) {
                    cb.className = cb.className.replace('dxeCheckBoxUnchecked', 'dxeCheckBoxChecked');
                } else {
                    cb.className = cb.className.replace('dxeCheckBoxChecked', 'dxeCheckBoxUnchecked');
                }
            }
        }
        function OnSelectedIndexChanged(s, e) {
            RefreshCheckBoxes(s);
            UpdateButtonState();
        }
    </script>

    <dx:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="function(s, e){ UpdateButtonState(); }" />
    </dx:ASPxGlobalEvents>

    <div class="container">
        <div class="contentEditors">
            <dx:ASPxListBox ID="lbModulosDisponibles" runat="server"
                ClientInstanceName="lbModulosDisponibles" ValueField="id_modulo_catalogo" TextField="nombre_catalogo"
                Width="300" Height="400" SelectionMode="CheckColumn" Caption="Modulos Disponibles" EnableSynchronization="True">
                <CaptionSettings Position="Top" HorizontalAlign="Center" />
                <ClientSideEvents SelectedIndexChanged="function(s, e) { 
                        var index = e.index;
                        var item = s.GetItem(index);
                        var isSelected = item.selected;
                        console.log('Item: ' + item.text + ' | Value: ' + item.value + ' | Seleccionado: ' + isSelected);
                    UpdateButtonState(); }" />
            </dx:ASPxListBox>
        </div>
        <div class="contentButtons">
            <div>
                <dx:ASPxButton ID="btnMoveSelectedItemsToRight" runat="server" ClientInstanceName="btnMoveSelectedItemsToRight" CssClass="button"
                    AutoPostBack="False" Text="Asignar >" ClientEnabled="False"
                    ToolTip="Add selected items">
                    <ClientSideEvents Click="function(s, e) { AddSelectedItems(); }" />
                </dx:ASPxButton>
            </div>
            <div class="TopPadding">
                <dx:ASPxButton ID="btnMoveAllItemsToRight" runat="server" ClientInstanceName="btnMoveAllItemsToRight" CssClass="button"
                    AutoPostBack="False" Text="Asignar todos >>" ToolTip="Add all items">
                    <ClientSideEvents Click="function(s, e) { AddAllItems(); }" />
                </dx:ASPxButton>
            </div>
            <div style="height: 32px">
            </div>
            <div>
                <dx:ASPxButton ID="btnMoveSelectedItemsToLeft" runat="server" ClientInstanceName="btnMoveSelectedItemsToLeft" CssClass="button"
                    AutoPostBack="False" Text="< Quitar" ClientEnabled="False"
                    ToolTip="Remove selected items">
                    <ClientSideEvents Click="function(s, e) { RemoveSelectedItems(); }" />
                </dx:ASPxButton>
            </div>
            <div class="TopPadding">
                <dx:ASPxButton ID="btnMoveAllItemsToLeft" runat="server" ClientInstanceName="btnMoveAllItemsToLeft" CssClass="button"
                    AutoPostBack="False" Text="<< Quitar todos" ClientEnabled="False"
                    ToolTip="Remove all items">
                    <ClientSideEvents Click="function(s, e) { RemoveAllItems(); }" />
                </dx:ASPxButton>
            </div>
            <div style="height: 32px">
            </div>
            <div class="TopPadding">
                <dx:ASPxButton ID="btnGuardar" runat="server" ClientInstanceName="btnGuardar" CssClass="button"
                    AutoPostBack="False" Text="Guardar" ClientEnabled="True"
                    ToolTip="Guarda los modulos asignados al rol">
                    <%--<ClientSideEvents Click="function(s, e) { RemoveAllItems(); }" />--%>
                </dx:ASPxButton>
            </div>
        </div>
        <div class="contentEditors">
            <dx:ASPxListBox ID="lbModulosAsignados" runat="server" ValueField="id_modulo_catalogo" TextField="nombre_catalogo"
                ClientInstanceName="lbModulosAsignados" Width="300" EnableSynchronization="True"
                Height="400" SelectionMode="CheckColumn" Caption="Módulos Asignados">
                <CaptionSettings Position="Top" HorizontalAlign="Center" />
                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }"></ClientSideEvents>
            </dx:ASPxListBox>
        </div>
    </div>

</asp:Content>
