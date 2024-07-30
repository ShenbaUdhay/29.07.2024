<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon_DBConnection.aspx.cs" Inherits="LDM.Web.DBConnection.Logon_DBConnection" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="Logon_DBCon_Form" runat="server">
        <div>
            <dx:ASPxLabel ID="Lbl_Servername" runat="server" Text="Server Name">
            </dx:ASPxLabel>
            <dx:ASPxTextBox ID="txt_servername" runat="server" Width="170px">
            </dx:ASPxTextBox>
            <dx:ASPxLabel ID="Lbl_DBname" runat="server" Text="DB Name">
            </dx:ASPxLabel>
            <dx:ASPxTextBox ID="txt_dbname" runat="server" Width="170px">
            </dx:ASPxTextBox>
            <dx:ASPxLabel ID="Lbl_Username" runat="server" Text="Username">
            </dx:ASPxLabel>
            <dx:ASPxTextBox ID="txt_username" runat="server" Width="170px">
            </dx:ASPxTextBox>
            <dx:ASPxLabel ID="Lbl_password" runat="server" Text="Password">
            </dx:ASPxLabel>
            <dx:ASPxTextBox ID="txt_password" runat="server" Width="170px">
            </dx:ASPxTextBox>
             <dx:ASPXbutton ID="btn_save" runat="server" Width="170px" Text="Save">
            </dx:ASPXbutton>
             <dx:ASPXbutton ID="btn_close" runat="server" Width="170px" Text="Close">
            </dx:ASPXbutton>
        </div>
    </form>
</body>
</html>
