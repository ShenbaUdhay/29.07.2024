<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelpCenter.aspx.cs" Inherits="LDM.Web.HelpCenter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body background-color: rgb(0, 0, 0) style="width: 1087px; height: 353px; margin-right: 54px;">
    <form id="form1" runat="server">
        <div>
        <asp:Panel ID="Heading_Panel" runat="server" BackColor= "#507CD1"  HorizontalAlign="Center" Height="45px" style="margin-left: 0px; ">
            <asp:Image ID="ProjectLogo" runat="server" style="text-align: left;position : absolute;left:11px; top: 10px;" ImageUrl="~/Images/NewAlpacaLogoWhite.png" />
            <asp:Label ID="lbl_FormName0" HorizontalAlign="Center" runat="server" Height="35px" Style="position :relative; top:07px; width: 284px;" Text="Help Center" Font-Names="Tahoma" Font-Size="X-Large" BackColor= "#507CD1" ForeColor="White" Font-Bold="False"></asp:Label>
            <asp:Label ID="lbl_Version" runat="server" Text="Version" style="text-align: right;position : absolute;right:15px;top: 10px;" Font-Names="Tahoma" Font-Size ="14px"></asp:Label>
            <asp:Label ID="lbl_owns" runat="server" Text="Productdtl" style="text-align: right;position : absolute;right:15px; top: 30px;" Font-Names="Tahoma" Font-Size ="14px"></asp:Label>
        </asp:Panel>
            <br />
            <asp:Label ID="lbl_category" runat="server" Text="Select the category " Height="30px" Font-Names="Tahoma" Font-Size ="14px"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="Category_DropDownList" AutoPostBack="true" style ="height: 25px;" width="200px" runat="server" OnSelectedIndexChanged ="DropDownList_SelectedIndexChanged" Font-Names="Tahoma" Font-Size ="14px" BackColor="#A2BDEA" Height="22px">
                <asp:ListItem Selected="True" Value="FAQ">FAQ</asp:ListItem>
                <asp:ListItem>Manual</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="txt_search" runat="server"  style="/*text-align: right;*/position : absolute;right:100px; top: 70px; width: 318px;  height: 20px;" Font-Names="Tahoma" Font-Size ="14px"></asp:TextBox>
            <asp:Button ID="btn_search" runat="server" Text="Search" onclick ="btnsearch_onclick" style="/*text-align: right;*/position : absolute;right:15px; top: 70px; height: 25px;" Font-Names="Tahoma" Font-Size ="14px"/>
            <br />
            <br />
            <asp:GridView ID="HelpCenter_GridView" AutoPostBack="true" OnRowCommand ="CustomersGridView_RowCommand" runat="server" CellPadding="4" BorderColor="#6699FF" BorderStyle ="Solid" BorderWidth="2px" ForeColor="#333333" AutoGenerateColumns="False" Width="761px" Font-Names="Tahoma" Font-Size ="14px">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Question" HeaderText="Question">
                    <ControlStyle BorderColor="#6699FF" BorderStyle="Solid" BorderWidth="2px" />
                    <HeaderStyle Font-Names="Tahoma" Font-Bold ="false"/>
                    <ItemStyle Font-Names="Tahoma" />
                    </asp:BoundField>
                    <asp:ButtonField DataTextField="Question" HeaderText="Question" Text="ManualTopic">
                    <HeaderStyle Font-Names="Tahoma" Font-Bold ="false"/>
                    <ItemStyle Font-Names="Tahoma" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="Article" HeaderText="Article">
                    <ControlStyle BorderColor="#6699FF" BorderStyle="Solid" BorderWidth="2px" />
                    <HeaderStyle Font-Names="Tahoma" Font-Bold ="false"/>
                    <ItemStyle Font-Names="Tahoma"/>
                    </asp:BoundField>
                    <asp:ButtonField DataTextField="FileData" HeaderText="Video" Text="Video">
                    <HeaderStyle Font-Names="Tahoma" Font-Bold ="false" />
                    <ItemStyle Font-Names="Tahoma" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:ButtonField>
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="False" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="False" ForeColor="White" Font-Names="Tahoma" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="False" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
