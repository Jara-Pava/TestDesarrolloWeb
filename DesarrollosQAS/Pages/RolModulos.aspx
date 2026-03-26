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

        // Recopila todos los elementos actuales de un cuadro de lista como una matriz de {texto, valor}.
        function CollectItems(listBox) {
            var result = [];
            for (var i = 0; i < listBox.GetItemCount(); i++) {
                var item = listBox.GetItem(i);
                result.push({ text: item.text, value: item.value });
            }
            console.log("Colección de modulos seleccionados -> ", result);
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
            MoveSelectedItems(lbModulosDisponibles, lbModulosAsignados);
        }
        function AddAllItems() {
            MoveAllItems(lbModulosDisponibles, lbModulosAsignados);
        }
        function RemoveSelectedItems() {
            MoveSelectedItems(lbModulosAsignados, lbModulosDisponibles);
        }
        function RemoveAllItems() {
            MoveAllItems(lbModulosAsignados, lbModulosDisponibles);
        }

        function MoveSelectedItems(srcListBox, dstListBox) {
            var selectedItems = srcListBox.GetSelectedItems();
            if (selectedItems.length === 0) return;

            // Marcar índices seleccionados para fácil filtrado y recopilar los elementos a mover
            var selectedIndices = {};
            var itemsToMove = [];
            for (var i = 0; i < selectedItems.length; i++) {
                selectedIndices[selectedItems[i].index] = true;
                itemsToMove.push({ text: selectedItems[i].text, value: selectedItems[i].value });
            }

            // Recopilar elementos restantes del origen (no seleccionados)
            var remainingItems = [];
            for (var i = 0; i < srcListBox.GetItemCount(); i++) {
                if (!selectedIndices[i]) {
                    var item = srcListBox.GetItem(i);
                    remainingItems.push({ text: item.text, value: item.value });
                }
            }

            // Recopilar elementos actuales del destino y agregar los nuevos
            var dstItems = CollectItems(dstListBox);
            for (var i = 0; i < itemsToMove.length; i++) {
                dstItems.push(itemsToMove[i]);
            }

            // Rebuild ambos cuadros de lista para reflejar los cambios
            RebuildListBox(srcListBox, remainingItems);

            RebuildListBox(dstListBox, dstItems);

            UpdateButtonState();
        }

        function MoveAllItems(srcListBox, dstListBox) {
            if (srcListBox.GetItemCount() === 0) return;

            // Recopilar todos los elementos del origen y agregarlos al destino
            var srcItems = CollectItems(srcListBox);
            var dstItems = CollectItems(dstListBox);
            for (var i = 0; i < srcItems.length; i++) {
                dstItems.push(srcItems[i]);
            }

            // Rebuild ambos cuadros de lista para reflejar los cambios
            RebuildListBox(srcListBox, []);
            RebuildListBox(dstListBox, dstItems);

            UpdateButtonState();
        }

        function UpdateButtonState() {
            btnMoveAllItemsToRight.SetEnabled(lbModulosDisponibles.GetItemCount() > 0);
            btnMoveAllItemsToLeft.SetEnabled(lbModulosAsignados.GetItemCount() > 0);
            btnMoveSelectedItemsToRight.SetEnabled(lbModulosDisponibles.GetSelectedItems().length > 0);
            btnMoveSelectedItemsToLeft.SetEnabled(lbModulosAsignados.GetSelectedItems().length > 0);
        }

        function OnSelectedIndexChanged(s, e) {
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
                <ClientSideEvents SelectedIndexChanged="OnSelectedIndexChanged" />
                <FilteringSettings ShowSearchUI="true" EditorNullText="Ingrese el modulo a buscar ..." />
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
                <ClientSideEvents SelectedIndexChanged="OnSelectedIndexChanged"></ClientSideEvents>
                <FilteringSettings ShowSearchUI="true" EditorNullText="Ingrese el modulo a buscar ..." />
            </dx:ASPxListBox>
        </div>
    </div>

</asp:Content>
