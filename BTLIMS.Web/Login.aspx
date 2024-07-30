<%@ Page Language="C#" AutoEventWireup="true" Inherits="LoginPage" EnableViewState="false"
    ValidateRequest="false" CodeBehind="Login.aspx.cs" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.ExpressApp.Web.Templates.ActionContainers" TagPrefix="cc2" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.ExpressApp.Web.Templates.Controls" TagPrefix="tc" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.ExpressApp.Web.Controls" TagPrefix="cc4" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.ExpressApp.Web.Templates" TagPrefix="cc3" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>ALPACA LIMS</title>
   <style>
        /*.LogonContent {
            padding: 0;
        }
        .WelcomeGroupClassCSS {
            position: relative;
        }

        .WelcomeTextClassCSS {
            text-align: left;
            padding: 25px 120px;
            font-size: 20px;
        }*/

        /*.LogonTemplate .WelcomeTextClassCSS {
            background-color: #2c86d3;
            text-align: left;
            padding: 25px 120px;
            font-size: 1.3em;
        }*/
            /*.LogonTemplate .WelcomeTextClassCSS .StaticText {
                color: #fff;
            }
        .LogonTemplate .PasswordHintClassCSS {
            position: absolute;
            background: #feb71e;
            line-height: 1.1em;
            border-radius: 78px;
            height: 120px;
            width: 120px;
            box-sizing: border-box;
            text-align: center;
            padding: 30px 12px 18px;
            right: -53px;
            bottom: -90px;
        }
            .LogonTemplate .PasswordHintClassCSS .StaticText {
                font-weight: bold;
                font-size: 0.85em;
                color: #fff;
            }
        .LogonTemplate .LogonTextClassCSS {
            text-align: center;
            padding-right: 45px;
            padding-left: 45px;
            padding-top: 20px;
            padding-bottom: 20px;
        }
            .LogonTemplate .LogonTextClassCSS .StaticText {
                font-size: 1em;
                color: #9a9a9a;
            }
        .MainGroupClassCSS {
            padding: 0 75px;
        }
        .dxmLite_XafTheme .dxm-main.menuButtons {
            padding: 10px 75px 80px;
        }
        @media (max-width: 600px), (max-height: 600px) {
            .LogonTemplate .PasswordHintClassCSS {
                right: 0;
            }
            .LogonTemplate .LogonTextClassCSS {
                padding-right: 45px;
                padding-left: 45px;
            }
        }
        @media (max-width: 480px), (max-height: 480px) {
            .LogonTemplate .LogonContent {
                padding: 0;
            }
            .LogonTemplate .WelcomeTextClassCSS {
                padding: 25px 108px;
            }
        }*/
    </style>
    <script type="text/javascript">
        window.onload = function () {
            var dt, expires;
            dt = new Date();
            dt.setTime(dt.getTime() + (1 * 24 * 60 * 60 * 1000));
            expires = "; expires=" + dt.toGMTString();
            console.log(new Date().toLocaleDateString('en-US'));
            document.cookie = 'Date =' + new Date().toLocaleDateString('en-US') + expires + '; path=/ '
            document.cookie = 'Offset =' + new Date().getTimezoneOffset() + expires + '; path=/ '
            document.cookie = 'TimeZone =' + new Date().toTimeString().slice(9) + '; path=/ '
            document.cookie = 'screenwidth =' + screen.width + '; path=/ '
            document.cookie = 'width =' + window.innerWidth + '; path=/ '
            document.cookie = 'screenheight =' + screen.height + '; path=/ '
            document.cookie = 'height =' + window.innerHeight + '; path=/ '
        };
    </script>
</head>
<body class="Dialog" style="overflow: hidden">
    <div id="PageContent" class="PageContent DialogPageContent">
        <form id="form1" runat="server">
            <cc4:ASPxProgressControl ID="ProgressControl" runat="server" />
            <div id="Content" runat="server" />
        </form>
    </div>
</body>
</html>
