<%@ Control Language="C#" CodeBehind="LogonTemplateContent2.ascx.cs" ClassName="LogonTemplateContent2" Inherits="Labmaster.Web.LogonTemplateContent2"%>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates.ActionContainers"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates.Controls"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Controls"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates"
    TagPrefix="xaf" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<xaf:XafUpdatePanel ID="UPPopupWindowControl" runat="server">
    <xaf:XafPopupWindowControl runat="server" ID="PopupWindowControl" />
</xaf:XafUpdatePanel>
<xaf:XafUpdatePanel ID="UPHeader" runat="server">
    <div class="Header">
        <script type="text/javascript">
            var popupWindow;

            function DBConnection_Load() {
                if (popupWindow && !popupWindow.closed) {
                    // If the popup is already open, focus on it
                    popupWindow.focus();
                } else {
                    // If the popup is not open or closed, open a new one
                    var width = 490;
                    var height = 650;
                    var left = (window.innerWidth - width) / 2;
                    var top = (window.innerHeight - height) / 2;
                    var url = "Logon_DBConnection.aspx";
                    popupWindow = window.open(url, "_blank", "width=" + width + ", height=" + height + ", left=" + left + ", top=" + top + ", addressbar=no,scrollbars=no, menubar=no, resizable=yes, directories=no, location=no");
                }
                return false; // Prevent the default behavior of the link
            }
        </script>
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>

                <td class="ViewImage">
                    <xaf:ViewImageControl ID="viewImageControl" runat="server" />
                </td>

                <td class="ImageCaption">
                    <asp:Image ID ="LogInImageControl"  runat="server" ImageUrl="~/Images/NewAlpacaLogoBlack.png"/>
                </td>
                <td class="Version">
                    <div class="col-md-2 mt-4" style="text-align: right;position : absolute;right:10px; top:25px;">
                        <div style="font-size: 11px; style="margin-left: 2400px"">
                            <asp:Label ID="Version" runat="server" OnLoad="Version_Load"></asp:Label><br/>
                            <asp:Label ID="Owns" runat="server" OnLoad="Owns_Load"></asp:Label><br/>
                            <asp:LinkButton ID="dbConnectionLink" runat="server" OnLoad="Page_Load" onclientclick="return DBConnection_Load()">DB Connection</asp:LinkButton>
                        </div>
                        <%--<div style="font-size: 12px;">Powered by BTSOFT</div>--%>
                        </div>
                    </div>
                </td>
       
               <%-- <td class="ViewCaption">
                    <h1> 
                        <xaf:ViewCaptionControl ID="viewCaptionControl" DetailViewCaptionMode="ViewCaption"
                            runat="server" ShowObjectCaptionFirst="True" /> --%>
            </tr>
        </table>
    </div>
</xaf:XafUpdatePanel>
<table class="DialogContent Content LogonContent" border="0" cellpadding="0" cellspacing="0"
    width="100%">
    <tr>
        <td class="LogonContentCell" align="center">
            <xaf:XafUpdatePanel ID="UPEI" runat="server" >
                <xaf:ErrorInfoControl ID="ErrorInfo" runat="server">
                   <%-- <dx:ASPxButton ID="BtnConnectectSetup" runat="server" Text="Connection Setup" OnClick ="BtnConnectectSetup_Click" style="top:60px; right: 0px; position:absolute">
                    </dx:ASPxButton>--%>
                    <%--<dx:ASPxComboBox ID="DBsetupComboBox" runat="server" ValueType="System.String" AutoPostBack="true"  OnSelectedIndexChanged ="DBConnection_SelectedIndexChanged" style="top:85px; position:center" Caption="Select the Database">
            </dx:ASPxComboBox>--%>
                </xaf:ErrorInfoControl>
            </xaf:XafUpdatePanel>    
            <asp:Table ID="Table1" CssClass="Logon" runat="server" BorderWidth="0px" CellPadding="0"
                CellSpacing="0">
                <asp:TableRow ID="TableRow2" runat="server">
                    <asp:TableCell runat="server" ID="ViewSite">
                        <xaf:XafUpdatePanel ID="UPVSC" runat="server">
                            <xaf:ViewSiteControl ID="viewSiteControl" runat="server" />
                           <%-- <dx:ASPxComboBox ID="DBsetupComboBox" runat="server" ValueType="System.String" AutoPostBack="true"  OnSelectedIndexChanged ="DBConnection_SelectedIndexChanged" style="width:435px" Caption="Database">
            </dx:ASPxComboBox>--%>
                        </xaf:XafUpdatePanel>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow3" runat="server">
                    <asp:TableCell runat="server" ID="TableCell4" HorizontalAlign="Right" Style="padding: 20px 0px 20px 0px">
                        <xaf:XafUpdatePanel ID="UPPopupActions" runat="server">
                            <xaf:ActionContainerHolder ID="PopupActions" runat="server"
                                Style="margin-left: 10px; display: inline" Orientation="Horizontal" ContainerStyle="Buttons">
                                <Menu Width="100%" ItemAutoWidth="False" HorizontalAlign="Right" />
                                <ActionContainers>
                                    <xaf:WebActionContainer ContainerId="PopupActions" IsDropDown="false" />                                                
                                </ActionContainers>                             
                            </xaf:ActionContainerHolder>
                        </xaf:XafUpdatePanel>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </td>
    </tr>
</table>
<p>
    &nbsp;</p>

