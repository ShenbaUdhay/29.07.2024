﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelpCenter_Manual.aspx.cs" Inherits="LDM.Web.HelpCenter_Manual" %>

<%@ Register Assembly="DevExpress.Web.ASPxRichEdit.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRichEdit" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div> 
        </div>
        <dx:ASPxRichEdit ID="ASPxRichEdit1" runat="server" ReadOnly="True" RibbonMode="None" WorkDirectory="~\App_Data\WorkDirectory">
        </dx:ASPxRichEdit>
    </form>
    </body>
</html>
