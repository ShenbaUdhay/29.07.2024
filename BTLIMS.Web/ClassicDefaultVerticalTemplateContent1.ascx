﻿<%@ Control Language="C#" CodeBehind="ClassicDefaultVerticalTemplateContent1.ascx.cs" ClassName="ClassicDefaultVerticalTemplateContent1" Inherits="LDM.Web.ClassicDefaultVerticalTemplateContent1" %>
<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates.ActionContainers"
    TagPrefix="cc2" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates"
    TagPrefix="cc3" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Controls"
    TagPrefix="cc4" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates.Controls"
    TagPrefix="tc" %>
<style type="text/css">
    .auto-style1 {
        width: 246px;
    }
</style>
<script type="text/javascript">
    function btnClick(value) {
        if (value == '13') {
            //alert(value);
            //$('#btnSearchBar').trigger('click');
            //$('#btnSearchBar').click();
            document.getElementById('<%= btnSearchBar.ClientID %>').click();
        }
    }
</script>

<style type="text/css">
    .SHCContainerPadding {
        margin-left: 250px;
    }
</style>

<div class="VerticalTemplate BodyBackColor">
    <cc3:XafUpdatePanel ID="UPPopupWindowControl" runat="server">
        <cc4:XafPopupWindowControl runat="server" ID="PopupWindowControl" />
    </cc3:XafUpdatePanel>
    <dx:ASPxGlobalEvents ID="GE" ClientSideEvents-EndCallback="AdjustSize" runat="server" />
    <table id="MT" border="0" width="100%" cellpadding="0" cellspacing="0" class="dxsplControl_<%= BaseXafPage.CurrentTheme %>">
        <tbody>
            <tr>
                <td style="vertical-align: top; height: 10px;" class="dxsplPane_<%= BaseXafPage.CurrentTheme %>">
                    <div id="VerticalTemplateHeader" class="VerticalTemplateHeader">
                        <table cellpadding="0" cellspacing="0" border="0" class="Top" width="100%">
                            <tr>
                                <td class="Logo">
                                    <asp:HyperLink runat="server" NavigateUrl="#" ID="LogoLink">
                                        <cc4:ThemedImageControl ID="TIC" DefaultThemeImageLocation="App_Themes/{0}/Xaf" ImageName="Logo.png"
                                            BorderWidth="0px" runat="server" />
                                    </asp:HyperLink>
                                </td>
                                <td class="Security">
                                    <cc3:XafUpdatePanel ID="UPSAC" UpdatePanelForASPxGridListCallback="False" runat="server">
                                        <cc2:ActionContainerHolder runat="server" ID="SAC" CssClass="Security"
                                            ContainerStyle="Links" ShowSeparators="True">
                                            <ActionContainers>
                                                <cc2:WebActionContainer ContainerId="Notifications" IsDropDown="false" />
                                                <cc2:WebActionContainer ContainerId="Security" IsDropDown="false" />
                                            </ActionContainers>
                                        </cc2:ActionContainerHolder>
                                    </cc3:XafUpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="ACPanel">
                            <tr class="Content">
                                <td class="Content WithPaddings">
                                    <cc3:XafUpdatePanel ID="UPSHC" runat="server" UpdatePanelForASPxGridListCallback="False">
                                        <cc2:ActionContainerHolder ID="SHC" runat="server" ContainerStyle="Links" CssClass="SHCContainerPadding">
                                            <%-- " CssClass="TabsContainer" "--%>
                                            <ActionContainers>
                                                <cc2:WebActionContainer ContainerId="Search" IsDropDown="false" />
                                                <cc2:WebActionContainer ContainerId="RootObjectsCreation" IsDropDown="false" />
                                                <cc2:WebActionContainer ContainerId="Appearance" IsDropDown="false" />
                                            </ActionContainers>
                                        </cc2:ActionContainerHolder>
                                    </cc3:XafUpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <table id="MRC" style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="LPcell" style="min-width: 250px; vertical-align: top" class="auto-style1">
                                <div id="LP" class="LeftPane">
                                    <cc3:XafUpdatePanel ID="UPNC" runat="server" UpdatePanelForASPxGridListCallback="False">
                                        <asp:TextBox runat="server" ID="txtSearch" placeholder="Search Navigation Item..." Style="margin-left: 10px; margin-bottom: 10px; margin-top: 10px; height: 20px; width: 150px; font-size: 10pt; padding-left: 4px;" onkeyup="btnClick(event.keyCode)" /><%--style="margin-left:10px;margin-bottom:10px;margin-top:10px;height:20px;width:80px;font-size: 10pt;padding-left:4px;"--%>
                                        <asp:Button ID="btnSearchBar" Text="Search" runat="server" OnClick="btnSearchBar_Click" Style="height: 25px; width: 60px; font-size: small;" /><%--style="height:20px;width:60px;"--%>
                                        <%--<cc2:NavigationActionContainer ID="NC" runat="server" CssClass="xafNavigationBarActionContainer" ContainerId="ViewsNavigation" Width="100%" style="overflow-y: scroll; height: 470px;"/>--%>
                                        <cc2:NavigationActionContainer ID="NC" runat="server" CssClass="xafNavigationBarActionContainer" ContainerId="ViewsNavigation" Width="100%" Style="overflow-y: auto; overflow-x: hidden; height: 82vmin" />
                                    </cc3:XafUpdatePanel>
                                    <cc3:XafUpdatePanel ID="UPTP" runat="server">
                                        <div id="TP" runat="server" class="ToolsActionContainerPanel">
                                            <dx:ASPxRoundPanel ID="TRP" runat="server" HeaderText="Tools" CssClass="TRP" Width="185px">
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                                        <cc2:ActionContainerHolder ID="VTC" runat="server" Menu-Width="100%"
                                                            Orientation="Vertical" ContainerStyle="Links" ShowSeparators="False">
                                                            <ActionContainers>
                                                                <cc2:WebActionContainer ContainerId="Tools" IsDropDown="false" />
                                                            </ActionContainers>
                                                        </cc2:ActionContainerHolder>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxRoundPanel>
                                            <cc2:ActionContainerHolder ID="DAC" runat="server" Orientation="Vertical"
                                                BorderWidth="0px" ContainerStyle="Links" ShowSeparators="False">
                                                <ActionContainers>
                                                    <cc2:WebActionContainer ContainerId="Diagnostic" IsDropDown="false" />
                                                </ActionContainers>
                                            </cc2:ActionContainerHolder>
                                            <br />
                                        </div>
                                    </cc3:XafUpdatePanel>
                                </div>
                            </td>
                            <td id="separatorCell" style="width: 6px; border-bottom-style: none; border-top-style: none"
                                class="dxsplVSeparator_<%= BaseXafPage.CurrentTheme %> dxsplPane_<%= BaseXafPage.CurrentTheme %>">
                                <div id="separatorButton" class="dxsplVSeparatorButton_<%= BaseXafPage.CurrentTheme %>" onmouseover="OnMouseEnter('separatorButton')"
                                    onmouseout="OnMouseLeave('separatorButton')" onclick="OnClick('LPcell','separatorImage',true)">
                                    <div id="separatorImage" style="width: 6px;" class="dxWeb_splVCollapseBackwardButton_<%= BaseXafPage.CurrentTheme %>">
                                    </div>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <cc3:XafUpdatePanel ID="UPTB" runat="server">
                                                    <cc2:ActionContainerHolder CssClass="ACH MainToolbar" runat="server" ID="TB" ContainerStyle="ToolBar" Orientation="Horizontal">
                                                        <Menu Width="100%" ItemAutoWidth="False" ClientInstanceName="mainMenu">
                                                            <BorderTop BorderStyle="None" />
                                                            <BorderLeft BorderStyle="None" />
                                                            <BorderRight BorderStyle="None" />
                                                        </Menu>
                                                        <ActionContainers>
                                                            <cc2:WebActionContainer ContainerId="CustomLink" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="ObjectsCreation" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="Edit" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="RecordEdit" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="View" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="ReportsSave" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="ReportsComment" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="Export" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="FullTextSearch" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="Reports" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="Filters" IsDropDown="false" />
                                                        </ActionContainers>
                                                    </cc2:ActionContainerHolder>
                                                </cc3:XafUpdatePanel>
                                                <cc3:XafUpdatePanel ID="UPVH" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                    <table id="VH" border="0" cellpadding="0" cellspacing="0" class="MainContent" width="100%">
                                                        <tr>
                                                            <td class="ViewHeader">
                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="ViewHeader">
                                                                    <tr>
                                                                        <td class="ViewImage">
                                                                            <cc4:ViewImageControl ID="VIC" runat="server" />
                                                                        </td>
                                                                        <td class="ViewCaption">
                                                                            <h1>
                                                                                <cc4:ViewCaptionControl ID="VCC" runat="server" />
                                                                            </h1>
                                                                            <cc2:NavigationHistoryActionContainer ID="VHC" runat="server" CssClass="NavigationHistoryLinks"
                                                                                ContainerId="ViewsHistoryNavigation" Delimiter=" / " />
                                                                        </td>
                                                                        <td align="right">
                                                                            <cc2:ActionContainerHolder runat="server" ID="RNC" ContainerStyle="Links" Orientation="Horizontal"
                                                                                UseLargeImage="True" CssClass="RecordsNavigationContainer">
                                                                                <Menu Width="100%" ItemAutoWidth="False" HorizontalAlign="Right" />
                                                                                <ActionContainers>
                                                                                    <cc2:WebActionContainer ContainerId="RecordsNavigation" IsDropDown="false" />
                                                                                </ActionContainers>
                                                                            </cc2:ActionContainerHolder>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </cc3:XafUpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <cc3:XafUpdatePanel ID="UPEMA" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                    <cc2:ActionContainerHolder runat="server" ID="EMA" ContainerStyle="Links" Orientation="Horizontal" CssClass="EditModeActions">
                                                        <Menu Width="100%" ItemAutoWidth="False" HorizontalAlign="Right" />
                                                        <ActionContainers>
                                                            <cc2:WebActionContainer ContainerId="Save" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="UndoRedo" IsDropDown="false" />
                                                        </ActionContainers>
                                                    </cc2:ActionContainerHolder>
                                                </cc3:XafUpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--<div id="CP" style="overflow-y: scroll; height: 450px; width: 100%;">--%>
                                                <div id="CP" style="overflow:auto; height: 78vmin;">
                                                    <%--<div id="CP">--%>
                                                    <table border="0" cellpadding="0" cellspacing="0" class="MainContent" width="100%">
                                                        <tr class="Content">
                                                            <td class="Content">
                                                                <cc3:XafUpdatePanel ID="UPEI" runat="server" UpdatePanelForASPxGridListCallback="True">
                                                                    <tc:ErrorInfoControl ID="ErrorInfo" Style="margin: 10px 0px 10px 0px" runat="server" />
                                                                </cc3:XafUpdatePanel>
                                                                <cc3:XafUpdatePanel ID="UPVSC" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                                    <cc4:ViewSiteControl ID="VSC" runat="server" />
                                                                    <cc2:ActionContainerHolder runat="server" ID="EditModeActions2" ContainerStyle="Links"
                                                                        Orientation="Horizontal" CssClass="EditModeActions">
                                                                        <Menu Width="100%" ItemAutoWidth="False" HorizontalAlign="Right" Paddings-PaddingTop="15px" />
                                                                        <ActionContainers>
                                                                            <cc2:WebActionContainer ContainerId="Save" IsDropDown="false" />
                                                                            <cc2:WebActionContainer ContainerId="UndoRedo" IsDropDown="false" />
                                                                        </ActionContainers>
                                                                    </cc2:ActionContainerHolder>
                                                                </cc3:XafUpdatePanel>
                                                                <%--<div id="Spacer" class="Spacer">
                                                                </div>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <cc3:XafUpdatePanel ID="UPQC" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                                    <cc2:QuickAccessNavigationActionContainer CssClass="Links NavigationLinks" ID="QC" runat="server"
                                                                        ContainerId="ViewsNavigation" PaintStyle="Caption" ShowSeparators="True" />
                                                                </cc3:XafUpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--<tr>
                <td style="height: 20px; vertical-align: bottom" class="BodyBackColor">
                    <cc3:XafUpdatePanel ID="UPIMP" runat="server" UpdatePanelForASPxGridListCallback="False">
                        <asp:Literal ID="InfoMessagesPanel" runat="server" Text="" Visible="False"></asp:Literal>
                    </cc3:XafUpdatePanel>
                    <div id="Footer" class="Footer">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <div class="FooterCopyright">
                                        <cc4:AboutInfoControl ID="AIC" runat="server">Copyright text</cc4:AboutInfoControl>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>--%>
        </tbody>
    </table>

</div>
