<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelpCenter_FileData.aspx.cs" Inherits="LDM.Web.HelpCenter_FileData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body  background-color: rgb(0, 0, 0) style="width: 1087px; height: 353px; margin-right: 54px;">
    <form id="form1" runat="server">
        <div>
             <asp:Panel ID="Heading_Panel" runat="server" BackColor= "#507CD1"  HorizontalAlign="Center" Height="45px" style="margin-left: 0px; margin-right:0px;">
            <asp:Image ID="ProjectLogo" runat="server" style="text-align: center;position : absolute;left:11px; top: 10px;" ImageUrl="~/Images/NewAlpacaLogoWhite.png" />
            <asp:Label ID="lbl_FormName0" HorizontalAlign="Center" runat="server" Height="35px" Style="position :relative; top:07px; width: 284px;" Text="Help Center" Font-Names="Tahoma" Font-Size="X-Large" BackColor= "#507CD1" ForeColor="White" Font-Bold="False"></asp:Label>
            <asp:Label ID="lbl_Version" runat="server" Text="Version" style="text-align: right;position : absolute;right:15px;top: 10px;" Font-Names="Tahoma" Font-Size ="14px"></asp:Label>
            <asp:Label ID="lbl_owns" runat="server" Text="Productdtl" style="text-align: right;position : absolute;right:15px; top: 30px;" Font-Names="Tahoma" Font-Size ="14px"></asp:Label>
        </asp:Panel>
            <br />
             <asp:GridView ID="HelpCenter_FileDate_GridView" AutoPostBack="true" OnRowCommand ="CustomersGridView_RowCommand" runat="server" CellPadding="4" BorderColor="#6699FF" BorderStyle ="Solid" BorderWidth="2px" ForeColor="#333333" AutoGenerateColumns="False" Width="761px" Font-Names="Tahoma">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Title" HeaderText="Title">
                    <ControlStyle BorderColor="#6699FF" BorderStyle="Solid" BorderWidth="2px" />
                    <HeaderStyle Font-Names="Tahoma"  Font-Bold ="false"/>
                    <ItemStyle Font-Names="Tahoma" />
                    </asp:BoundField>
                    <asp:ButtonField DataTextField="FileName" HeaderText="ClickHereToDownload">
                    <HeaderStyle Font-Names="Tahoma"  Font-Bold ="false"/>
                    <ItemStyle Font-Names="Tahoma" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="Size" HeaderText="Size">
                    <ControlStyle BorderColor="#6699FF" BorderStyle="Solid" BorderWidth="2px" />
                    <HeaderStyle Font-Names="Tahoma"  Font-Bold ="false"/>
                    <ItemStyle Font-Names="Tahoma" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Sort" HeaderText="Sort">
                    <HeaderStyle Font-Names="Tahoma"  Font-Bold ="false"/>
                    <ItemStyle Font-Names="Tahoma" />
                    </asp:BoundField>
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="False" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="False" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
