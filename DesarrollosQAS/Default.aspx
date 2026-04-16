<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="Default.aspx.cs" Inherits="DesarrollosQAS.Default" Title="Bienvenido" %>

<asp:Content ContentPlaceHolderID="Content" runat="server">
    <style>
        .welcome-wrapper {
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: calc(100vh - 80px);
            padding: 20px;
        }

        .welcome-card {
            background: white;
            border-radius: 10px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.08);
            padding: 50px 40px;
            text-align: center;
            max-width: 650px;
            width: 100%;
        }

        .welcome-icon {
            font-size: 64px;
            margin-bottom: 16px;
        }

        .welcome-title {
            color: #353943;
            font-size: 30px;
            font-weight: bold;
            margin: 0 0 8px 0;
        }

        .welcome-subtitle {
            color: teal;
            font-size: 18px;
            font-weight: 600;
            margin: 0 0 20px 0;
        }

        .welcome-user {
            background-color: #f0f7f7;
            border: 1px solid #d0e8e8;
            border-radius: 6px;
            padding: 14px 20px;
            margin: 20px auto;
            max-width: 400px;
            color: #353943;
            font-size: 16px;
        }

        .welcome-user strong {
            color: teal;
        }

        .welcome-message {
            color: #666;
            font-size: 15px;
            line-height: 1.7;
            margin: 20px 0;
        }

        .welcome-divider {
            border: none;
            border-top: 2px solid #e9ecef;
            margin: 25px auto;
            max-width: 80px;
        }

        .welcome-footer {
            color: #999;
            font-size: 13px;
            margin-top: 20px;
        }

        .welcome-date {
            color: #888;
            font-size: 14px;
            margin-top: 10px;
        }
    </style>

    <div class="welcome-wrapper">
        <div class="welcome-card">
            <h1 class="welcome-title">Bienvenido</h1>
            <p class="welcome-subtitle">Desarrollo Aplicaciones - Techint E&C</p>

            <div class="welcome-user">
                Usuario: <strong><asp:Label ID="lblNombreUsuario" runat="server" /></strong>
            </div>

            <hr class="welcome-divider" />

            <p class="welcome-message">
                Utilice el menú lateral para navegar entre los módulos disponibles.
                <br />
                Los módulos visibles dependen de los permisos asignados a su usuario.
            </p>

            <p class="welcome-date">
                <asp:Label ID="lblFecha" runat="server" />
            </p>

            <p class="welcome-footer">
                Si necesita acceso a un módulo adicional, contacte al administrador del sistema.
            </p>
        </div>
    </div>
</asp:Content>