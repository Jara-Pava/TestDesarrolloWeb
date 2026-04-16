<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="SinAcceso.aspx.cs" Inherits="DesarrollosQAS.SinAcceso" Title="Sin Acceso" %>

<asp:Content ContentPlaceHolderID="Content" runat="server">
    <style>
        .sinacceso-wrapper {
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: calc(100vh - 80px);
            padding: 20px;
        }

        .sinacceso-card {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            padding: 50px 40px;
            text-align: center;
            max-width: 500px;
            width: 90%;
        }

        .sinacceso-icon {
            font-size: 64px;
            color: #dc3545;
            margin-bottom: 20px;
        }

        .sinacceso-title {
            color: #353943;
            font-size: 28px;
            font-weight: bold;
            margin-bottom: 10px;
        }

        .sinacceso-message {
            color: #666;
            font-size: 16px;
            line-height: 1.6;
            margin-bottom: 10px;
        }

        .sinacceso-user {
            background-color: #f8f9fa;
            border: 1px solid #e9ecef;
            border-radius: 4px;
            padding: 12px;
            margin: 20px 0;
            color: #353943;
            font-weight: bold;
        }

        .sinacceso-footer {
            color: #999;
            font-size: 13px;
            margin-top: 30px;
        }
    </style>

    <div class="sinacceso-wrapper">
        <div class="sinacceso-card">
            <div class="sinacceso-icon">&#128274;</div>
            <h1 class="sinacceso-title">Acceso Denegado</h1>
            <p class="sinacceso-message">
                Su usuario no tiene permisos, contacte con el administrador
            </p>
            <div class="sinacceso-user">
                Usuario: <asp:Label ID="lblUsuario" runat="server" />
            </div>
            <p class="sinacceso-footer">
                
            </p>
        </div>
    </div>
</asp:Content>