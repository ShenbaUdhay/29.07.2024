﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.Web;
using DevExpress.XtraReports.Security;
using System;
using System.Configuration;
using System.Web.UI.WebControls;

namespace BTLIMS.Web
{
    public class Global : System.Web.HttpApplication
    {
        public Global()
        {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e)
        {
            SecurityAdapterHelper.Enable();
            DevExpress.Web.ASPxSpreadsheet.Internal.SpreadsheetTileHelper.TileColumnCount = 200;
            DevExpress.Web.ASPxSpreadsheet.Internal.SpreadsheetTileHelper.TileRowCount = 200;
            DevExpress.Web.ASPxSpreadsheet.Internal.SpreadsheetTileHelper.WindowHorizontalPadding = 200;
            DevExpress.Web.ASPxSpreadsheet.Internal.SpreadsheetTileHelper.WindowVerticalPadding = 200;
            ASPxWebControl.BackwardCompatibility.DataControlAllowReadUnlistedFieldsFromClientApiDefaultValue = true;
            DevExpress.Security.Resources.AccessSettings.ReportingSpecificResources.SetRules(SerializationFormatRule.Allow(DevExpress.XtraReports.UI.SerializationFormat.Code, DevExpress.XtraReports.UI.SerializationFormat.Xml));
            DevExpress.XtraPrinting.Native.PrintingSettings.UseGdiPlusLineBreakAlgorithm = true;
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif
            XafPopupWindowControl.DefaultHeight = Unit.Percentage(70);
            XafPopupWindowControl.DefaultWidth = Unit.Percentage(60);
            XafPopupWindowControl.PopupTemplateType = PopupTemplateType.FindDialog;
            XafPopupWindowControl.ShowPopupMode = ShowPopupMode.Centered;
        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            WebApplication.SetInstance(Session, new BTLIMSAspNetApplication());
            WebApplication webApplication = WebApplication.Instance;
            webApplication.GetSecurityStrategy().RegisterXPOAdapterProviders();
            WebApplication.Instance.Settings.DefaultVerticalTemplateContentPath = "ClassicDefaultVerticalTemplateContent1.ascx";
            WebApplication.Instance.Settings.LogonTemplateContentPath = "LogonTemplateContent2.ascx";
            ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);
            WebApplicationStyleManager.EnableGridColumnsUpperCase = false;
            DevExpress.ExpressApp.Web.Templates.ActionContainers.NavigationActionContainer.ShowImages = true;
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
#if EASYTEST
                        if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                            WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
                        }
#endif
            if (System.Diagnostics.Debugger.IsAttached && WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
            {
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e)
        {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e)
        {
        }

        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }
}
