<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Portal.ascx.cs" Inherits="BTLIMS.Web.Portal" %>
<%--<asp:Button ID="Button1" runat="server" Text="打开列表" OnClick="Button1_Click" />--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <title>绍兴市柯桥区质量技术监督检测所</title>
    <!---------------------------------- 页面基本设置禁止随意更改 ------------------------------------------>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="index" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <meta name="renderer" content="webkit" />
    <meta http-equiv="Cache-Control" content="no-siteapp" />
    <!---------------------------------- 页面基本设置禁止随意更改 -------------------------------------------->
    <!---------------------------------- 基础CSS类库可随意更改 ---------------------------------------------->
    <link type="text/css" rel="stylesheet" href="css/reset.css" />
    <link type="text/css" rel="stylesheet" href="css/keqiao.css" />
    <!---------------------------------- 基础CSS类库可随意更改结束 -------------------------------------------->
    <!---------------------------------- 基础js类库可随意更改 ---------------------------------------->
    <script type="text/javascript" src="js/jquery.1.11.3.min.js"></script>
    <script type="text/javascript" src="js/keqiao.js"></script>
    <!---------------------------------- 基础js类库可随意更改 ---------------------------------------->
    <%--    <style type="text/css">
     html, body, form {
          height: 100%;
          margin: 0;
          padding: 0;
          overflow: hidden;
     }
</style>--%>
        <style type="text/css">
.fa-stack[data-count]:after{
  position:absolute;
  right:0%;
  top:1%;
  content: attr(data-count);
  font-size:30%;
  padding:.6em;
  border-radius:999px;
  line-height:.75em;
  color: white;
  background:rgba(255,0,0,.85);
  text-align:center;
  min-width:2em;
  font-weight:bold;
  font-size:medium;
}

.center-things {
    height: 150%;
  top: 50%;
    
}
.center-things p {
    margin: 0;
    background: none;
    position: absolute;
    top: 50%;
    left: 50%;
    margin-right: -50%;
    transform: translate(-50%, -50%);
}
    </style>
    <style type="text/css">
        .xafNav {
            display: none;
        }

        .borderBottom {
            display: none;
        }

        .paddings {
            padding: 0;
        }

        #menuAreaDiv {
            display: none;
        }

        .CardGroupContent {
            padding: 0;
        }

        .GroupContent {
            margin: 0;
        }

        .Item {
            padding: 0;
        }

        .xafFooter {
            padding: 6px 40px;
        }

        #Vertical_UPEI {
            display: none;
        }

        html {
            overflow: hidden !important;
        }

        #form2 {
            height: 100%;
            overflow: auto;
        }

        #Content {
            min-height: 100%;
            position: relative;
        }

        #footer {
            position: absolute;
            bottom: 0;
            width: 100%;
            left: 0;
        }

        body {
            background: #e9e9e9;
        }

        @media (min-width: 600px) {
            .CardGroupBase.GroupContent {
                border: 0 !important;
            }
        }

        .imgButtonOutline {
            outline: none;
        }
    </style>
</head>
<body>
    <div class="keqiao-box">
        <div class="keqiao-header">
            <div class="logo">
                <!--<img src="images/Logo.png" />-->
                  <h1 style="color:#3F4A70"> <asp:Label ID="lblCompanyFirstName" runat="server" Text="BTLIMS" Font-Size="XX-Large" ForeColor="Black"></asp:Label>&nbsp<asp:Label ID="lblCompanySecondName" runat="server" Text="The Complete Lab Solution" Font-Size="Medium" ForeColor="Black"></asp:Label></h1>
            </div>
            <div class="person-menu">
                <ul class="person-header-menu">
                    <li>
                        <div class="header-menu-top">
                            <img src="images/dxx.png" />
                            <span class="name"><%--我的账号--%><asp:Literal id="ltlMyAccount" runat="server"></asp:Literal></span>
                        </div>
                        <div class="header-menu-content">
                            <ul>
                                <li>
                                    <a href="javascript:">
                                        <%= DevExpress.ExpressApp.SecuritySystem.CurrentUserName %>
                                    </a>
                                    <asp:Button runat="server" ID="btnMyDetails" CssClass="keqiao-inputlink-button" OnClick="btnMyDetails_OnClick" />
                                </li>
                                <li class="keqiao-inputlink-link">
                                    <a href="javascript:"><%--退出--%><asp:Literal id="ltlLogOff" runat="server"></asp:Literal>
                                    </a>
                                    <asp:Button runat="server" ID="btnLogout" CssClass="keqiao-inputlink-button" OnClick="btnLogout_OnClick" />
                                </li>
                            </ul>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
   <!--     <div class="keqiao-content">
            <div class="keqiao-panel-box" id="divKB" runat="server" visible="False">
                <div class="panel-box-header">
                    <%--管理看板--%><asp:Literal runat="server" ID="ltlDashboard"></asp:Literal>
                </div>
                <div class="panel-box-content">
                    <ul class="keqiao-formlink-box">
                        <li id="liAllocation" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--任务分配--%><asp:Literal runat="server" ID="ltlTaskAssignment"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlAllocation" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnAllocation" CssClass="keqiao-inputlink-button" OnClick="btnAllocation_OnClick" />
                            </div>
                        </li>
                        <li id="liRegistration" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--结果登记--%><asp:Literal runat="server" ID="ltlResultEntry"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlRegistration" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnRegistration" CssClass="keqiao-inputlink-button" OnClick="btnRegistration_OnClick" />
                            </div>
                        </li>
                        <li id="liAudit" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--报告审核--%><asp:Literal runat="server" ID="ltlReportReview"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlAudit" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnAudit" CssClass="keqiao-inputlink-button" OnClick="btnAudit_OnClick" />
                            </div>
                        </li>
                        <li id="liApproval" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--报告批准--%><asp:Literal runat="server" ID="ltlReportApproval"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlApproval" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnApproval" CssClass="keqiao-inputlink-button" OnClick="btnApproval_OnClick" />
                            </div>
                        </li>
                        <li id="liPrinting" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--报告打印--%><asp:Literal runat="server" ID="ltlReportPrint"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlPrinting" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnPrinting" CssClass="keqiao-inputlink-button" OnClick="btnPrinting_OnClick" />
                            </div>
                        </li>
                        <li id="liReception" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--证书接收--%><asp:Literal runat="server" ID="ltlCertificateReception"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlReception" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnReception" CssClass="keqiao-inputlink-button" OnClick="btnReception_OnClick" />
                            </div>
                        </li>
                        <li id="liRelease" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--证书发放--%><asp:Literal runat="server" ID="ltlCertificateRelease"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlRelease" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnRelease" CssClass="keqiao-inputlink-button" OnClick="btnRelease_OnClick" />
                            </div>
                        </li>
                        <li id="liArchiving" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--业务归档--%><asp:Literal runat="server" ID="ltlBusinessArchiving"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlArchiving" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnArchiving" CssClass="keqiao-inputlink-button" OnClick="btnArchiving_OnClick" />
                            </div>
                        </li>
                        <li id="liInStorage" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--器具入库--%><asp:Literal runat="server" ID="ltlInstrumentStorage"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlInStorage" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnInStorage" CssClass="keqiao-inputlink-button" OnClick="btnInStorage_OnClick" />
                            </div>
                        </li>
                        <li id="liUnclaimed" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--任务领取--%><asp:Literal runat="server" ID="ltlTaskReceipt"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlUnclaimed" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnUnclaimed" CssClass="keqiao-inputlink-button" OnClick="btnUnclaimed_OnClick" />
                            </div>
                        </li>
                        <li id="liReturned" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--器具归还--%><asp:Literal runat="server" ID="ltlInstrumentReturn"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlReturned" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnReturned" CssClass="keqiao-inputlink-button" OnClick="btnReturned_OnClick" />
                            </div>
                        </li>
                        <li id="liConfirmation" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--归还确认--%><asp:Literal runat="server" ID="ltlReturnConfirmation"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlConfirmation" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnConfirmation" CssClass="keqiao-inputlink-button" OnClick="btnConfirmation_OnClick" />
                            </div>
                        </li>
                        <li id="liSampleRelease" runat="server" visible="False">
                            <div class="link keqiao-inputlink-link">
                                <span class="name"><%--器具发放--%><asp:Literal runat="server" ID="ltlInstrumentRelease"></asp:Literal></span><span class="number"><asp:Literal runat="server" ID="ltlSampleRelease" Text="0"></asp:Literal></span>
                                <asp:Button runat="server" ID="btnSampleRelease" CssClass="keqiao-inputlink-button" OnClick="btnSampleRelease_OnClick" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>-->
            <div class="keqiao-panel-box" id="divYY" runat="server" visible="False">
                <div class="panel-box-header">
                    <%--应用中心--%><asp:Literal runat="server" ID="ltlApplicationCenter"></asp:Literal>
                </div>
                <div class="panel-box-content">
                    <asp:Literal runat="server" ID="ltlError" Visible="False"></asp:Literal>
                    <!--<div class="keqiao-apply-title" id="divBizTitle" runat="server" visible="False">
                        <img src="images/keqiao-10.png" />
                        <span><%--业务系统--%><asp:Literal runat="server" ID="ltlBusinessSystem"></asp:Literal></span>
                    </div>-->
                    <!--<div class="keqiao-apply-content" id="divBizContent" runat="server" visible="False">
                        <ul>                             
                            <li id="liBizJL" runat="server" visible="False">
                                <div class="image keqiao-inputlink-link">                                      
                                    <img src="images/bizJL.png">
                                    <asp:Button ID="btnBiz" runat="server" CssClass="keqiao-inputlink-button" OnClick="btnBiz_OnClick" />
                                </div>
                                <div class="apply-content-name"><%--计量业务--%><asp:Literal runat="server" ID="ltlMeasurementBusiness"></asp:Literal></div>
                            </li>
                            <li id="liBizZJ" runat="server" visible="False">
                                <div class="image keqiao-inputlink-link">
                                    <img src="images/bizZJ.png">
                                    <asp:Button ID="btnBizZJ" runat="server" CssClass="keqiao-inputlink-button" OnClientClick="javascritp:alert('此功能未上线');" />
                                </div>
                                <div class="apply-content-name"><%--质检业务--%><asp:Literal runat="server" ID="ltlQualityTestingBusiness"></asp:Literal></div>
                            </li>
                        </ul>
                    </div>-->
                    <div class="keqiao-apply-title" id="divDataTitle" runat="server" visible="False">
                        <img src="images/keqiao-10.png" />
                        <span><%--通用数据--%><asp:Literal runat="server" ID="ltlGeneralData"></asp:Literal></span>
                    </div>
                    <div class="keqiao-apply-content" id="divDataContent" runat="server" visible="False">
                        <ul>
                            <li id="liSample" runat="server" visible="False">
                                <div class="image keqiao-inputlink-link">
                                    <!--<span class="fa-stack fa-3x has-badge" data-count="10" id="lblSample" runat="server">-->
                                    <img src="Images/bizZJ.png">
                                    <asp:Button ID="btnSample" runat="server" CssClass="keqiao-inputlink-button" OnClick="btnSample_Click" />
                                     <!--</span>-->
                                </div>
                                <div class="apply-content-name"><%--人力资源--%><asp:Literal runat="server" ID="ltlSample"></asp:Literal></div>
                            </li>
                             <li id="liDataEntry" runat="server" visible="False">
                                <div class="image keqiao-inputlink-link">
                                    <img src="Images/bizJL.png">
                                    <asp:Button ID="btnDataEntry" runat="server" CssClass="keqiao-inputlink-button" OnClick="btnDataEntry_Click" />
                                </div>
                                <div class="apply-content-name"><%--系统管理--%><asp:Literal runat="server" ID="ltlDataEntry"></asp:Literal></div>
                            </li>
                            <li id="liSettings" runat="server" visible="False">
                                <div class="image keqiao-inputlink-link">
                                    <img src="Images/mSeting.png">
                                    <asp:Button ID="btnSystem1" runat="server" CssClass="keqiao-inputlink-button" OnClick="btnSystem1_Click" />
                                </div>
                                <div class="apply-content-name"><%--系统管理--%><asp:Literal runat="server" ID="ltlSetting"></asp:Literal></div>
                            </li>
                            <li id="liHr" runat="server" visible="False">
                                <div class="image keqiao-inputlink-link">
                                    <!--<span class="fa-stack fa-3x has-badge" data-count="10" id="lblCount" runat="server">-->
                                    <img src="images/mHr.png">
                                    <asp:Button ID="btnHr" runat="server" CssClass="keqiao-inputlink-button" OnClick="btnHr_OnClick" />
                                     <!--</span>-->
                                </div>
                                <div class="apply-content-name"><%--人力资源--%><asp:Literal runat="server" ID="ltlHumanResources"></asp:Literal></div>
                            </li>
                            </ul>
                        <ul>
                           <!-- <li id="liAssets" runat="server" visible="False">
                                <div class="image keqiao-inputlink-link">
                                    <img src="images/mAssets.png">
                                    <asp:Button ID="btnAssets" runat="server" CssClass="keqiao-inputlink-button" OnClick="btnAssets_OnClick" />
                                </div>
                                <div class="apply-content-name"><%--资产管理--%><asp:Literal runat="server" ID="ltlAssetManagement"></asp:Literal></div>
                            </li>-->
                            <li id="liCrm" runat="server" visible="False">
                                <div class="image keqiao-inputlink-link">
                                    <img src="images/mCrm.png">
                                    <asp:Button ID="btnCrm" runat="server" CssClass="keqiao-inputlink-button" OnClick="btnCrm_OnClick" />
                                </div>
                                <div class="apply-content-name"><%--客户管理--%><asp:Literal runat="server" ID="ltlCustomerManagement"></asp:Literal></div>
                            </li>
                            <li id="liConfig" runat="server" visible="False">
                                <div class="image keqiao-inputlink-link">
                                    <img src="images/mSeting.png">
                                    <asp:Button ID="btnSeting" runat="server" CssClass="keqiao-inputlink-button" OnClick="btnSeting_OnClick" />
                                </div>
                                <div class="apply-content-name"><%--基础配置--%><asp:Literal runat="server" ID="ltlConfiguration"></asp:Literal></div>
                            </li>
                            <li id="liReport" runat="server" visible="False">
                                <div class="image keqiao-inputlink-link">
                                    <img src="images/mReport.png">
                                    <asp:Button ID="btnReport" runat="server" CssClass="keqiao-inputlink-button" OnClick="btnReport_OnClick" />
                                </div>
                                <div class="apply-content-name"><%--报表--%><asp:Literal runat="server" ID="ltlReports"></asp:Literal></div>
                            </li>
                            <li id="liSystem" runat="server" visible="False">
                                <div class="image keqiao-inputlink-link">
                                    <img src="images/system.png">
                                    <asp:Button ID="btnSystem" runat="server" CssClass="keqiao-inputlink-button" OnClick="btnSystem_OnClick" />
                                </div>
                                <div class="apply-content-name"><%--系统管理--%><asp:Literal runat="server" ID="ltlSystemManagement"></asp:Literal></div>
                            </li>
                           
                        </ul>
                    </div>

                </div>
            </div>
        </div>
    </div>
</body>
</html>

