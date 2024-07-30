<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomLogin.ascx.cs" Inherits="BTWEB.Web.CustomLogin" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <title>ALPACA LIMS</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <style type="text/css">
        .LogonHeader {
            display: block;
            width: 100%;
            height: 85px;
            background: #393939;
        }
    </style>
    <%--<script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/popper.js"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
    <script type="text/javascript" src="js/main.js"></script>--%>
    <script type="text/javascript">
        function eyebtn(eyeIcon) {
            var passwordField = document.getElementById("Password");
            if (passwordField.type === "text") {
                passwordField.type = "password";
            } else {
                passwordField.type = "text";
            }

            if (eyeIcon.classList.contains("fa-eye")) {
                eyeIcon.classList.remove("fa-eye");
                eyeIcon.classList.add("fa-eye-slash");
            } else {
                eyeIcon.classList.remove("fa-eye-slash");
                eyeIcon.classList.add("fa-eye");
            }
        }

        function ClickLoginButton(event) {
            if (event.keyCode == 13 || event.which == 13) {
                document.getElementById("login").click();
            }
        }
    </script>
</head>
<body>
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false"
        RenderOuterTable="false" OnAuthenticate="LoginUser_Authenticate"
        OnLoggedIn="LoginUser_LoggedIn" OnLoginError="LoginUser_LoginError">
        <LayoutTemplate>
            <div class="LogonHeader">
                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                    <div class="col-md-10 mt-4">
                        <asp:Image ID="BannerImage" runat="server" OnPreRender="BannerImage_PreRender" Style="border-width: 0px;" />
                    </div>
                    <div class="col-md-2 mt-4" style="text-align: right">
                        <div style="font-size: 11px;">
                            <asp:Label ID="Version" runat="server" OnLoad="Version_Load"></asp:Label>
                        </div>
                        <div style="font-size: 12px;">Powered by BTSOFT</div>
                    </div>
                </div>
            </div>
            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" Visible="False" ValidationGroup="LoginUserValidationGroup" />
            <section class="ftco-section">
                <div class="container">
                    <div class="row justify-content-center">
                        <div class="col-md-12 col-lg-10">
                            <div
                                class="d-flex justify-content-center align-items-center mb-2 br-4">
                                <div class="mt-4">
                                    <h2 class="heading-section">
                                        <asp:Label ID="Product" runat="server" OnLoad="Product_Load"></asp:Label>
                                    </h2>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-md-12 col-lg-10">
                            <div class="wrap d-md-flex">
                                <asp:Image ID="LoginImage" runat="server" CssClass="img" OnPreRender="LoginImage_PreRender" />
                                <%--<div
                                    class="img"
                                    style="background-image: url(images/bg-1.jpg)">
                                </div>--%>
                                <div class="login-wrap p-4 p-md-5">
                                    <div class="d-flex">
                                        <div class="w-100">
                                            <h3 class="mb-4">Log In</h3>
                                        </div>
                                    </div>
                                    <div class="form-group mb-3">
                                        <label class="label" for="name">Username</label>
                                        <asp:TextBox ID="UserName" runat="server" CssClass="form-control" placeholder="Username" onkeypress="ClickLoginButton(event)"></asp:TextBox>
                                    </div>
                                    <div class="form-group mb-3 relative">
                                        <label class="label" for="password">Password</label>
                                        <asp:TextBox ID="Password" ClientIDMode="Static" runat="server" CssClass="form-control pr-25" placeholder="Password" TextMode="Password" onkeypress="ClickLoginButton(event)"></asp:TextBox>
                                        <i id="eyePassword" onclick="eyebtn(this)" class="fa fa-eye-slash show-password"></i>
                                    </div>
                                    <div class="form-group">
                                        <asp:Button ID="login" ClientIDMode="Static" runat="server" CssClass="form-control btn btn-primary rounded submit px-3" CommandName="Login" Text="Log In" ValidationGroup="LoginUserValidationGroup" />
                                    </div>
                                    <div class="form-group d-md-flex">
                                        <div class="w-50 text-left">
                                            <label class="checkbox-wrap checkbox-primary mb-0">
                                                Remember Me                    
                                                <input id="chkRememberMe" runat="server" type="checkbox" checked />
                                                <span class="checkmark"></span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="info-link-box !body">
                                        <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                            ErrorMessage="Username cannot be empty" ToolTip="Username cannot be empty"
                                            ValidationGroup="LoginUserValidationGroup" CssClass="info-link-box">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </LayoutTemplate>
    </asp:Login>
</body>
</html>
