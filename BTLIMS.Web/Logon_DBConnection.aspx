<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon_DBConnection.aspx.cs" Inherits="LDM.Web.Logon_DBConnection" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DBConnection Manager</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" />   
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <style>
        body {
            background-color: #f8f9fa;
        }

        .container {
            margin-top: 50px;
            max-width: 400px; /* Adjusted the maximum width */
            background-color: #ffffff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }

        .form-group {
            margin-bottom: 15px; /* Adjusted the margin */
        }

        .form-control {
            width: 100%; /* Make the input fields and dropdowns take full width */
            padding: 8px; /* Adjusted the padding */
        }     
        .form-group {
            position: relative;
        }
        .password-container {
            position: relative;
        }
        .password-toggle-icon {
            cursor: pointer;
            position: absolute;
            top: 50%;
            transform: translateY(-50%);
            right: 10px; /* Adjust this value based on your design */
        }

    </style>
    <script type="text/javascript">
        function setInitialValues() {
            var txtUserName = document.getElementById('<%= txtUserName.ClientID %>');
            var txtPassword = document.getElementById('<%= txtPassword.ClientID %>');

            // Set initial values
            txtUserName.value = '';
            txtPassword.value = '';
        }

        // Hook into the window's onload event to ensure controls are available
        window.onload = setInitialValues;

        function closePopup() {
            window.close();
        }
        function togglePasswordVisibility(textboxId) {
            var passwordTextbox = document.getElementById(textboxId);
            var eyeIcon = document.getElementById('eyeIcon');

            if (passwordTextbox.type === 'password') {
                passwordTextbox.type = 'text';
                eyeIcon.classList.remove('fa-eye');
                eyeIcon.classList.add('fa-eye-slash');
            } else {
                passwordTextbox.type = 'password';
                eyeIcon.classList.remove('fa-eye-slash');
                eyeIcon.classList.add('fa-eye');
            }
        }

    </script>
</head>
<body>
    <form id="form2" runat="server" autocomplete="off">
        <div>
            <div class="container mt-5">
                <h2>DBConnection Manager</h2>

                <div class="form-group">
                    <label for="ddlTitle">Title:</label>                    
                    <asp:DropDownList runat="server" ID="ddlTitle" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="Title_SelectedIndexChanged" AppendDataBoundItems="true">
                    <asp:ListItem Text="N/A" Value="N/A" />
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label for="ddlServerName">Server Name:</label>
                    <asp:DropDownList runat="server" ID="ddlServerName" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ServerName_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label for="ddlDataBaseName">Database Name:</label>
                    <asp:DropDownList runat="server" ID="ddlDataBaseName" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label for="txtUserName">User Id:</label>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" autocomplete="new-userid"></asp:TextBox>                    
                </div>

                <div class="form-group">
                    <label for="txtPassword">Password:</label>
                    <div class="password-container">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" autocomplete="new-password" />
                        <span class="password-toggle-icon" onclick="togglePasswordVisibility('txtPassword')">
                            <i class="fa fa-eye" id="eyeIcon"></i>
                        </span>
                    </div>
                </div>
         
                <asp:Button runat="server" ID="Button1" Text="Cancel" CssClass="btn btn-secondary" OnClientClick="closePopup();" />
                <asp:Button runat="server" ID="btnSave" Text="Ok" OnClick="btnSave_Click" CssClass="btn btn-primary" />
            </div>
        </div>
    </form>
</body>

</html>
