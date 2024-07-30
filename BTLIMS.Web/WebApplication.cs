using DevExpress.Data.Filtering;
using DevExpress.DataProcessing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using Modules.BusinessObjects.Dashboard;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using static Modules.BusinessObjects.Setting.ScreenAutoLock;

namespace BTLIMS.Web
{
    public partial class BTLIMSAspNetApplication : WebApplication
    {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule module2;
        private BTLIMS.Module.Web.BTLIMSAspNetModule module4;
        private DevExpress.ExpressApp.CloneObject.CloneObjectModule cloneObjectModule;
        private DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule conditionalAppearanceModule;
        private DevExpress.ExpressApp.FileAttachments.Web.FileAttachmentsAspNetModule fileAttachmentsAspNetModule;
        private DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule htmlPropertyEditorAspNetModule;
        private DevExpress.ExpressApp.ReportsV2.ReportsModuleV2 reportsModuleV2;
        private DevExpress.ExpressApp.ReportsV2.Web.ReportsAspNetModuleV2 reportsAspNetModuleV2;
        private DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase treeListEditorsModuleBase;
        private DevExpress.ExpressApp.TreeListEditors.Web.TreeListEditorsAspNetModule treeListEditorsAspNetModule;
        private DevExpress.ExpressApp.Validation.ValidationModule validationModule;
        private Modules.ModulesModule modulesModule1;
        private DevExpress.ExpressApp.Security.SecurityStrategyComplex securityStrategyComplex1;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        private DevExpress.ExpressApp.Chart.ChartModule chartModule1;
        private DevExpress.ExpressApp.Kpi.KpiModule kpiModule1;
        private DevExpress.ExpressApp.PivotGrid.PivotGridModule pivotGridModule1;
        private DevExpress.ExpressApp.Chart.Web.ChartAspNetModule chartAspNetModule1;
        private DevExpress.ExpressApp.PivotGrid.Web.PivotGridAspNetModule pivotGridAspNetModule1;
        private DevExpress.ExpressApp.Dashboards.DashboardsModule dashboardsModule1;
        private DevExpress.ExpressApp.Dashboards.Web.DashboardsAspNetModule dashboardsAspNetModule1;
        private DevExpress.ExpressApp.Security.AuthenticationStandard authenticationStandard1;
        private DevExpress.ExpressApp.Workflow.WorkflowModule workflowModule1;
        private NotificationsModule notificationsModule1;
        //custom login
        //private AuthenticationStandardForFullyCustomLogin authenticationStandardForFullyCustomLogin1;
        private DevExpress.ExpressApp.Notifications.Web.NotificationsAspNetModule notificationsAspNetModule1;
        private DevExpress.ExpressApp.Office.OfficeModule officeModule1;
        private DevExpress.ExpressApp.Office.Web.OfficeAspNetModule officeAspNetModule1;
        private DevExpress.ExpressApp.Scheduler.SchedulerModuleBase schedulerModuleBase1;
        private DevExpress.ExpressApp.Scheduler.Web.SchedulerAspNetModule schedulerAspNetModule1;
        private DevExpress.ExpressApp.Validation.Web.ValidationAspNetModule validationAspNetModule;
        private Module.BTLIMSModule btlimsModule1;
        public BTLIMSAspNetApplication()
        {
            InitializeComponent();
            LinkNewObjectToParentImmediately = false;
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        {
            args.ObjectSpaceProvider = new XPObjectSpaceProvider(GetDataStoreProvider(args.ConnectionString, args.Connection), true);
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));

        }
        private IXpoDataStoreProvider GetDataStoreProvider(string connectionString, System.Data.IDbConnection connection)
        {
            System.Web.HttpApplicationState application = (System.Web.HttpContext.Current != null) ? System.Web.HttpContext.Current.Application : null;
            IXpoDataStoreProvider dataStoreProvider = null;
            if (application != null && application["DataStoreProvider"] != null)
            {
                dataStoreProvider = application["DataStoreProvider"] as IXpoDataStoreProvider;
            }
            else
            {
                if (!String.IsNullOrEmpty(connectionString))
                {
                    //XpoDefault.Session = null;
                    connectionString = DevExpress.Xpo.XpoDefault.GetConnectionPoolString(connectionString);
                    dataStoreProvider = new ConnectionStringDataStoreProvider(connectionString,true);
                    XpoDefault.DataLayer = XpoDefault.GetDataLayer(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
                }
                else if (connection != null)
                {
                    dataStoreProvider = new ConnectionDataStoreProvider(connection);
                }
                if (application != null)
                {
                    application["DataStoreProvider"] = dataStoreProvider;
                }
            }
            ((SecurityStrategy)securityStrategyComplex1).AssociationPermissionsMode = AssociationPermissionsMode.Manual;
            return dataStoreProvider;
        }
        private void BTLIMSAspNetApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e)
        {
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
            if (System.Diagnostics.Debugger.IsAttached)
            {
                e.Updater.Update();
                e.Handled = true;
            }
            else
            {
                string message = "The application cannot connect to the specified database, " +
                    "because the database doesn't exist, its version is older " +
                    "than that of the application or its schema does not match " +
                    "the ORM data model structure. To avoid this error, use one " +
                    "of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";

                if (e.CompatibilityError != null && e.CompatibilityError.Exception != null)
                {
                    message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
                }
                throw new InvalidOperationException(message);
            }
#endif
        }
        private void InitializeComponent()
        {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
            this.cloneObjectModule = new DevExpress.ExpressApp.CloneObject.CloneObjectModule();
            this.conditionalAppearanceModule = new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule();
            this.fileAttachmentsAspNetModule = new DevExpress.ExpressApp.FileAttachments.Web.FileAttachmentsAspNetModule();
            this.htmlPropertyEditorAspNetModule = new DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule();
            this.reportsModuleV2 = new DevExpress.ExpressApp.ReportsV2.ReportsModuleV2();
            this.reportsAspNetModuleV2 = new DevExpress.ExpressApp.ReportsV2.Web.ReportsAspNetModuleV2();
            this.treeListEditorsModuleBase = new DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase();
            this.treeListEditorsAspNetModule = new DevExpress.ExpressApp.TreeListEditors.Web.TreeListEditorsAspNetModule();
            this.validationModule = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.validationAspNetModule = new DevExpress.ExpressApp.Validation.Web.ValidationAspNetModule();
            this.modulesModule1 = new Modules.ModulesModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.securityStrategyComplex1 = new DevExpress.ExpressApp.Security.SecurityStrategyComplex();
            this.authenticationStandard1 = new DevExpress.ExpressApp.Security.AuthenticationStandard();
            this.module4 = new BTLIMS.Module.Web.BTLIMSAspNetModule();
            this.chartModule1 = new DevExpress.ExpressApp.Chart.ChartModule();
            this.kpiModule1 = new DevExpress.ExpressApp.Kpi.KpiModule();
            this.pivotGridModule1 = new DevExpress.ExpressApp.PivotGrid.PivotGridModule();
            this.chartAspNetModule1 = new DevExpress.ExpressApp.Chart.Web.ChartAspNetModule();
            this.pivotGridAspNetModule1 = new DevExpress.ExpressApp.PivotGrid.Web.PivotGridAspNetModule();
            this.dashboardsModule1 = new DevExpress.ExpressApp.Dashboards.DashboardsModule();
            this.dashboardsAspNetModule1 = new DevExpress.ExpressApp.Dashboards.Web.DashboardsAspNetModule();
            this.workflowModule1 = new DevExpress.ExpressApp.Workflow.WorkflowModule();
            this.notificationsModule1 = new DevExpress.ExpressApp.Notifications.NotificationsModule();
            this.notificationsAspNetModule1 = new DevExpress.ExpressApp.Notifications.Web.NotificationsAspNetModule();
            this.officeModule1 = new DevExpress.ExpressApp.Office.OfficeModule();
            this.officeAspNetModule1 = new DevExpress.ExpressApp.Office.Web.OfficeAspNetModule();
            this.schedulerModuleBase1 = new DevExpress.ExpressApp.Scheduler.SchedulerModuleBase();
            this.schedulerAspNetModule1 = new DevExpress.ExpressApp.Scheduler.Web.SchedulerAspNetModule();
            this.btlimsModule1 = new BTLIMS.Module.BTLIMSModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // cloneObjectModule
            // 
            this.cloneObjectModule.ClonerType = null;
            // 
            // reportsModuleV2
            // 
            this.reportsModuleV2.EnableInplaceReports = true;
            this.reportsModuleV2.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportDataV2);
            this.reportsModuleV2.ReportStoreMode = DevExpress.ExpressApp.ReportsV2.ReportStoreModes.XML;
            // 
            // reportsAspNetModuleV2
            // 
            this.reportsAspNetModuleV2.ReportViewerType = DevExpress.ExpressApp.ReportsV2.Web.ReportViewerTypes.HTML5;
            // 
            // validationModule
            // 
            this.validationModule.AllowValidationDetailsAccess = true;
            this.validationModule.IgnoreWarningAndInformationRules = false;
            // 
            // securityStrategyComplex1
            // 
            this.securityStrategyComplex1.AllowAnonymousAccess = false;
            this.securityStrategyComplex1.Authentication = this.authenticationStandard1;
            this.securityStrategyComplex1.PermissionsReloadMode = DevExpress.ExpressApp.Security.PermissionsReloadMode.NoCache;
            this.securityStrategyComplex1.RoleType = typeof(Modules.BusinessObjects.Hr.CustomSystemRole);
            this.securityStrategyComplex1.UseOptimizedPermissionRequestProcessor = false;
            this.securityStrategyComplex1.UserType = typeof(Modules.BusinessObjects.Hr.CustomSystemUser);
            // 
            // authenticationStandard1
            // 
            this.authenticationStandard1.LogonParametersType = typeof(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters);
            // 
            // dashboardsModule1
            // 
            this.dashboardsModule1.DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData);
            // 
            // workflowModule1
            // 
            this.workflowModule1.RunningWorkflowInstanceInfoType = typeof(DevExpress.ExpressApp.Workflow.Xpo.XpoRunningWorkflowInstanceInfo);
            this.workflowModule1.StartWorkflowRequestType = typeof(DevExpress.ExpressApp.Workflow.Xpo.XpoStartWorkflowRequest);
            this.workflowModule1.UserActivityVersionType = typeof(DevExpress.ExpressApp.Workflow.Versioning.XpoUserActivityVersion);
            this.workflowModule1.WorkflowControlCommandRequestType = typeof(DevExpress.ExpressApp.Workflow.Xpo.XpoWorkflowInstanceControlCommandRequest);
            this.workflowModule1.WorkflowDefinitionType = typeof(DevExpress.ExpressApp.Workflow.Xpo.XpoWorkflowDefinition);
            this.workflowModule1.WorkflowInstanceKeyType = typeof(DevExpress.Workflow.Xpo.XpoInstanceKey);
            this.workflowModule1.WorkflowInstanceType = typeof(DevExpress.Workflow.Xpo.XpoWorkflowInstance);
            // 
            // notificationsModule1
            // 
            this.notificationsModule1.CanAccessPostponedItems = false;
            this.notificationsModule1.NotificationsRefreshInterval = System.TimeSpan.Parse("00:31:00");
            this.notificationsModule1.NotificationsStartDelay = System.TimeSpan.Parse("00:00:05");
            this.notificationsModule1.ShowDismissAllAction = false;
            this.notificationsModule1.ShowNotificationsWindow = false;
            this.notificationsModule1.ShowRefreshAction = false;
            // 
            // officeModule1
            // 
            this.officeModule1.RichTextMailMergeDataType = null;
            // 
            // BTLIMSAspNetApplication
            // 
            this.ApplicationName = "BTLIMS";
            this.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.cloneObjectModule);
            this.Modules.Add(this.conditionalAppearanceModule);
            this.Modules.Add(this.reportsModuleV2);
            this.Modules.Add(this.treeListEditorsModuleBase);
            this.Modules.Add(this.validationModule);
            this.Modules.Add(this.schedulerModuleBase1);
            this.Modules.Add(this.schedulerAspNetModule1);
            this.Modules.Add(this.modulesModule1);
            this.Modules.Add(this.chartModule1);
            this.Modules.Add(this.kpiModule1);
            this.Modules.Add(this.pivotGridModule1);
            this.Modules.Add(this.dashboardsModule1);
            this.Modules.Add(this.securityModule1);
            this.Modules.Add(this.notificationsModule1);
            this.Modules.Add(this.fileAttachmentsAspNetModule);
            this.Modules.Add(this.htmlPropertyEditorAspNetModule);
            this.Modules.Add(this.reportsAspNetModuleV2);
            this.Modules.Add(this.treeListEditorsAspNetModule);
            this.Modules.Add(this.validationAspNetModule);
            this.Modules.Add(this.chartAspNetModule1);
            this.Modules.Add(this.pivotGridAspNetModule1);
            this.Modules.Add(this.dashboardsAspNetModule1);
            this.Modules.Add(this.workflowModule1);
            this.Modules.Add(this.notificationsAspNetModule1);
            this.Modules.Add(this.officeModule1);
            this.Modules.Add(this.officeAspNetModule1);
            this.Modules.Add(this.btlimsModule1);
            this.Modules.Add(this.module4);
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Web.Localization.ASPxGridViewControlLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Web.Localization.ASPxGridViewResourceLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Web.Localization.ASPxperienceResourceLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.ReportsV2.Web.ASPxReportControlLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.HtmlPropertyEditor.Web.Localization.ASPxHtmlEditorResourceLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Web.Localization.ASPxImagePropertyEditorLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Web.Localization.ASPxEditorsResourceLocalizer));
            this.Security = this.securityStrategyComplex1;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.BTLIMSAspNetApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        protected override void Logon(PopupWindowShowActionExecuteEventArgs logonWindowArgs)
        {
            base.Logon(logonWindowArgs);
        }

        protected override void OnLoggedOn(LogonEventArgs args)
        {
            base.OnLoggedOn(args);

            curlanguage lang = new curlanguage();
            SampleLogInInfo sLInfo = new SampleLogInInfo();
            lang.strcurlanguage = "En";
            string oldVersionNumber = string.Empty;
            string newVersionNumber = string.Empty;
            Employee currentUser = SecuritySystem.CurrentUser as Employee;

            using (IObjectSpace os = CreateObjectSpace())
            {
                Session currentSession = ((XPObjectSpace)(os)).Session;
                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);

                #region ServiceUser Default Password Changed
                PermissionPolicyUser policyUser = uow.FindObject<PermissionPolicyUser>(CriteriaOperator.Parse("UserName='Service'"));
                if (policyUser != null)
                {
                    var date = DateTime.Now;
                    int q = (date.Month + 2) / 3;
                    string pass = "Alpaca@q" + q + date.ToString("yy");
                    if (!policyUser.ComparePassword(pass))
                    {
                        policyUser.SetPassword(pass);
                        uow.CommitChanges();
                    }
                }
                #endregion

                #region LogInUser Limit
                IList<LoginLog> lstLogiUser = uow.Query<LoginLog>().Where(i => i.Active != null && i.Active == true).ToList();
                IList<LoginLog> lstsignOffUser = lstLogiUser.Where(i => i.LoginDateTime.Date != DateTime.Today && i.LoginDateTime.Date != DateTime.Today.AddDays(-1)).ToList();
                if (lstsignOffUser.Count > 0)
                {
                    lstsignOffUser.ToList().ForEach(i => { i.Active = false; });
                    uow.CommitChanges();
                }
                lstLogiUser = uow.Query<LoginLog>().Where(i => i.Active != null && i.Active == true).ToList();
                ////if (lstLogiUser != null && lstLogiUser.Count > 0 && lstLogiUser.Select(i => i.UserID).Distinct().Count() > 5)
                ////{
                ////    CustomLoginInfo obj = new CustomLoginInfo();
                ////    obj.strMessage = "Already 5 users logged in.";
                ////    WebApplication.Redirect("CustomError.aspx", true);
                ////}
                #endregion

                #region Screen AutoLock
                ScreenAutoLock autoLock = currentSession.FindObject<ScreenAutoLock>(CriteriaOperator.Parse(string.Empty));
                if (autoLock != null)
                {
                    if (autoLock.TimeOut == Minutes.ThirtyMinutes)
                    {
                        WebWindow.CurrentRequestPage.Session.Timeout = 30;
                    }
                    else if (autoLock.TimeOut == Minutes.NinetyMinutes)
                    {
                        WebWindow.CurrentRequestPage.Session.Timeout = 90;
                    }
                    else if (autoLock.TimeOut == Minutes.OneHundredandTwentyMinutes)
                    {
                        WebWindow.CurrentRequestPage.Session.Timeout = 120;
                    }
                    else if (autoLock.TimeOut == Minutes.SixtyMinutes)
                    {
                        WebWindow.CurrentRequestPage.Session.Timeout = 60;
                    }
                }
                #endregion

                #region Current Language
                SelectedData sproc = currentSession.ExecuteSproc("getCurrentLanguage", "");
                var Culture = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                if (Culture == "En")
                {
                    this.SetFormattingCulture("EN-US");
                }
                else
                {
                    this.SetFormattingCulture("zh");
                }
                //WebApplication.Instance.SetLanguage("zh-CN");
                #endregion

                #region JobIDFormat
                JobIDFormat objJobIDFormat = uow.FindObject<JobIDFormat>(CriteriaOperator.Parse(""));
                if (objJobIDFormat != null)
                {
                    sLInfo.SampleIDDigit = objJobIDFormat.SampleIDDigit;
                }
                else
                {
                    objJobIDFormat=new JobIDFormat(uow);
                    objJobIDFormat.Dynamic = true;
                    objJobIDFormat.Year = YesNoFilter.Yes;
                    objJobIDFormat.Month = YesNoFilter.Yes;
                    objJobIDFormat.Day = YesNoFilter.Yes;
                    objJobIDFormat.YearFormat = YearFormat.yy;
                    objJobIDFormat.SequentialNumber = 3;
                    objJobIDFormat.NumberStart = 1;
                    objJobIDFormat.SampleIDDigit = SampleIDDigit.Three;
                    objJobIDFormat.Prefix = YesNoFilter.No;
                    objJobIDFormat.Save();
                    sLInfo.SampleIDDigit = objJobIDFormat.SampleIDDigit;
                    uow.CommitChanges();

                }
                #endregion
                #region ReportIDFormat
                ReportIDFormat objReportIDFormat = uow.FindObject<ReportIDFormat>(CriteriaOperator.Parse(""));
                if (objReportIDFormat == null)
                //{
                //    objSMInfo.ReportIDDigit = objReportIDFormat.ReportIDDigit;
                //}
                //else
                {
                    objReportIDFormat = new ReportIDFormat(uow);
                    objReportIDFormat.Dynamic = true;
                    objReportIDFormat.Year = YesNoFilters.Yes;
                    objReportIDFormat.Month = YesNoFilters.Yes;
                    objReportIDFormat.Day = YesNoFilters.Yes;
                    objReportIDFormat.YearFormat = YearFormats.yy;
                    objReportIDFormat.Prefixs = YesNoFilters.No;
                    objReportIDFormat.SequentialNumber = 3;
                    objReportIDFormat.NumberStart = 1;
                    //objReportIDFormat.ReportIDDigit = ReportIDDigit.Three;
                    objReportIDFormat.Save();
                    //objSMInfo.ReportIDDigit = objReportIDFormat.ReportIDDigit;
                    uow.CommitChanges();
                }
                #endregion
                #region SourceSetup
                SampleSourceSetup objSampleSource= uow.FindObject<SampleSourceSetup>(CriteriaOperator.Parse(""));
                if(objSampleSource==null)
                {
                    objSampleSource = new SampleSourceSetup(uow);
                    objSampleSource.NeedToActivateSampleSourceMode = SampleSourceMode.No;
                    objSampleSource.Save();
                    uow.CommitChanges();
                }
                #endregion

                #region ICM Expiration Alert 
                //currentSession.ExecuteNonQuery("LTExpiryalert_Sp");
                #endregion

                #region Version Number
                if (currentUser.UserName != null)
                {
                    LoginLog loginLog = new LoginLog(uow);
                    loginLog.UserID = uow.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    loginLog.LoginDateTime = DateTime.Now;
                    System.Reflection.Assembly assem;
                    System.Reflection.AssemblyName assemname;
                    System.Version assemVersion;
                    assem = System.Reflection.Assembly.GetExecutingAssembly();
                    assemname = assem.GetName();
                    assemVersion = assemname.Version;
                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assem.Location);
                    string version = fileVersionInfo.ProductVersion;
                    List<string> VersionNumber = assemname.Version.ToString().Split('.').ToList();
                    if (VersionNumber != null && VersionNumber.Count > 0)
                    {
                        if (VersionNumber.Count == 4)
                        {
                            VersionNumber.RemoveAt(3);
                            loginLog.VersionNumber = string.Join(".", VersionNumber.ToArray());
                        }
                    }
                    newVersionNumber = loginLog.VersionNumber;
                    loginLog.Active = true;
                    string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                    if (ip == "::1") ip = "192.168.0.3";
                    System.Net.IPAddress address;
                    if (System.Net.IPAddress.TryParse(ip, out address))
                    {

                    }

                    string strIPAddress;
                    if (!String.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"]))
                        strIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
                    else
                        strIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    loginLog.Save();
                    uow.CommitChanges();
                }
                #endregion

                #region Navigation
                List<Guid> existingNavItems = new List<Guid>();
                List<Guid> existingdefsetting = new List<Guid>();
                //oldVersionNumber = uow.Query<VersionControl>().Where(i => !string.IsNullOrEmpty(i.VersionNumber)).Max(b => b.VersionNumber);
                //VersionControl oldVersion = uow.Query<VersionControl>().Where(i => !string.IsNullOrEmpty(i.VersionNumber) && i.VersionNumber== newVersionNumber).FirstOrDefault();
                VersionControl oldVersion = uow.FindObject<VersionControl>(CriteriaOperator.Parse("[VersionNumber]=?", newVersionNumber));
                if (oldVersion == null)
                {
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        XDocument xWebDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(@"~\Model_zh-CN.xafml"));
                        IModelApplicationNavigationItems navigationItem = WebApplication.Instance.Model as IModelApplicationNavigationItems;
                        if (navigationItem != null)
                        {
                            foreach (IModelNavigationItem item in navigationItem.NavigationItems.AllItems)
                            {
                                bool visible = false;
                                string parent = string.Empty;
                                string parentid = string.Empty;
                                string mainParentid = string.Empty;
                                string subParentid = string.Empty;
                                string mainParentmodulename = string.Empty;
                                string subParentmodulename = string.Empty;
                                string ChildParentmodulename = string.Empty;
                                string childParentid = string.Empty;
                                string strParentCaption = string.Empty;
                                string strparentid = string.Empty;
                                string strsubparentid = string.Empty;
                                string strsubmodule = string.Empty;
                                string strParent = string.Empty;
                                string strmodulepath = string.Empty;
                                string[] arrParentcaption = null;
                                if (item.Parent.Parent.GetValue<string>("Caption") != null && item.Visible == true)
                                {
                                    IEnumerable<string> webNavItems = null;
                                    if (xWebDoc != null)
                                    {
                                        webNavItems = from Items in xWebDoc.Descendants("Item")
                                                      where (string)Items.Attribute("Id") == item.Id
                                                      select (string)Items.Attribute("Caption");
                                    }

                                    string strCNCaption = string.Empty;

                                    if (webNavItems != null && !string.IsNullOrEmpty(webNavItems.FirstOrDefault()))
                                    {
                                        strCNCaption = webNavItems.FirstOrDefault();
                                    }
                                    string strPath = item.GetValue<string>("ItemPath");
                                    string[] strItem = strPath.Split(new[] { "/Items/" }, StringSplitOptions.None);
                                    strParent = strItem[1].ToString();
                                    parent = Findcaption(item.Parent.Parent, parent);
                                    parentid = Findparentid(item.Parent.Parent, parentid);
                                    if (parentid == "Settings\\Utility\\Sample Registration")
                                    {
                                        visible = Findsubitemvisible(item.Parent.Parent, visible);
                                    }
                                    else
                                    {
                                        visible = Findvisible(item.Parent.Parent, visible);
                                    }

                                    if (visible == true)
                                    {
                                        //Modules.BusinessObjects.Setting.NavigationItem UserNavigation = uow.FindObject<Modules.BusinessObjects.Setting.NavigationItem>(new BinaryOperator("NavigationId", item.Id));
                                        Modules.BusinessObjects.Setting.NavigationItem UserNavigation = uow.FindObject<Modules.BusinessObjects.Setting.NavigationItem>(CriteriaOperator.Parse("[NavigationId] = ? And [GCRecord] is NULL", item.Id));
                                        if (UserNavigation == null)
                                        {
                                            UserNavigation = new NavigationItem(uow);
                                            UserNavigation.NavigationId = item.Id;
                                            UserNavigation.NavigationCaption = item.Caption;
                                            if (!string.IsNullOrEmpty(strCNCaption))
                                            {
                                                UserNavigation.NavigationCNCaption = strCNCaption;
                                            }
                                            else
                                            {
                                                UserNavigation.NavigationCNCaption = item.Caption;
                                            }
                                            if (item.View.AsObjectView != null)
                                            {
                                                UserNavigation.NavigationModelClass = item.View.AsObjectView.ModelClass.Name;
                                            }
                                            UserNavigation.NavigationView = item.View.Id.ToString();
                                            UserNavigation.Itempath = strPath;
                                            UserNavigation.Parent = parent;
                                            UserNavigation.Exclude = !item.Visible;
                                            UserNavigation.Exist = true;
                                            UserNavigation.Select = true;
                                            UserNavigation.Save();
                                        }
                                        else
                                        {
                                            UserNavigation.NavigationCaption = item.Caption;
                                            if (!string.IsNullOrEmpty(strCNCaption))
                                            {
                                                UserNavigation.NavigationCNCaption = strCNCaption;
                                            }
                                            else
                                            {
                                                UserNavigation.NavigationCNCaption = item.Caption;
                                            }
                                            if (item.View.AsObjectView != null)
                                            {
                                                UserNavigation.NavigationModelClass = item.View.AsObjectView.ModelClass.Name;
                                            }
                                            UserNavigation.NavigationView = item.View.Id.ToString();
                                            UserNavigation.Itempath = strPath;
                                            UserNavigation.Parent = parent;
                                            UserNavigation.Exclude = !item.Visible;
                                            UserNavigation.Exist = true;
                                            UserNavigation.Select = true;
                                        }
                                        existingNavItems.Add(UserNavigation.Oid);
                                        if (parentid.Contains("\\"))
                                        {
                                            string[] arrParent = parentid.Split('\\');
                                            if (arrParent.Length > 1)
                                            {
                                                strparentid = arrParent[arrParent.Length - 1];
                                            }
                                            else if (arrParent.Length == 1)
                                            {
                                                strparentid = arrParent[0];
                                            }
                                            else
                                            {
                                                strparentid = parentid;
                                            }
                                            if (arrParent.Length == 2)
                                            {
                                                strparentid = arrParent[1];

                                            }
                                            if (arrParent.Length == 3)
                                            {
                                                strparentid = arrParent[1];
                                                strsubparentid = arrParent[2];
                                            }
                                            if (arrParent.Length == 4)
                                            {
                                                childParentid = arrParent[1];
                                                strsubparentid = arrParent[3];
                                            }
                                        }
                                        else
                                        {
                                            strparentid = parentid;
                                        }
                                        if (parent.Contains("\\"))
                                        {
                                            arrParentcaption = parent.Split('\\');
                                            if (arrParentcaption.Length > 1)
                                            {
                                                strParentCaption = arrParentcaption[arrParentcaption.Length - 1];
                                                strmodulepath = arrParentcaption[arrParentcaption.Length - 1];
                                            }
                                            else if (arrParentcaption.Length == 1)
                                            {
                                                strParentCaption = arrParentcaption[0];
                                                strmodulepath = arrParentcaption[0];
                                            }
                                            else
                                            {
                                                strParentCaption = parent;
                                                strmodulepath = parent;

                                            }
                                            if (arrParentcaption.Length == 2)
                                            {
                                                strmodulepath = arrParentcaption[0] + "/" + arrParentcaption[1];
                                                mainParentmodulename = arrParentcaption[0];
                                                subParentmodulename = arrParentcaption[1];
                                                mainParentid = Findmainparentid(item.Parent.Parent, mainParentid);
                                            }
                                            if (arrParentcaption.Length == 3)
                                            {
                                                strmodulepath = arrParentcaption[0] + "/" + arrParentcaption[1] + "/" + arrParentcaption[2];
                                                mainParentmodulename = arrParentcaption[0];
                                                subParentmodulename = arrParentcaption[1];
                                                strsubmodule = arrParentcaption[2];
                                            }
                                            if (arrParentcaption.Length == 4)
                                            {
                                                strmodulepath = arrParentcaption[0] + "/" + arrParentcaption[1] + "/" + arrParentcaption[2] + "/" + arrParentcaption[3];
                                                mainParentmodulename = arrParentcaption[0];
                                                subParentmodulename = arrParentcaption[1];
                                                strsubmodule = arrParentcaption[3];
                                            }
                                        }
                                        else
                                        {
                                            strParentCaption = parent;
                                            strmodulepath = parent;
                                        }
                                        if (!string.IsNullOrEmpty(item.Id) && !string.IsNullOrEmpty(strParentCaption))
                                        {
                                            DefaultSetting objDefaultSetting = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = ? and [ModuleName] = ? and [IsModule] = false", item.Id, strParentCaption));
                                            if (objDefaultSetting == null)
                                            {
                                                objDefaultSetting = new DefaultSetting(uow);
                                                objDefaultSetting.NavigationItemNameID = item.Id;
                                                objDefaultSetting.NavigationItemName = item.Caption;
                                                objDefaultSetting.ModuleName = strParentCaption;
                                                objDefaultSetting.IsModule = false;
                                                objDefaultSetting.Select = true;
                                                objDefaultSetting.ItemPath = strPath;
                                                objDefaultSetting.Save();
                                            }
                                            else
                                            if (objDefaultSetting != null && objDefaultSetting.Select && objDefaultSetting.SortIndex > 0)
                                            {
                                                item.Index = objDefaultSetting.SortIndex;
                                            }
                                            else
                                            {
                                                objDefaultSetting.ItemPath = strPath;
                                            }
                                            existingdefsetting.Add(objDefaultSetting.Oid);
                                        }
                                        //Main ModuleName Creation
                                        if (!string.IsNullOrEmpty(strParentCaption) && string.IsNullOrEmpty(strsubparentid))
                                        {
                                            DefaultSetting objModuleDefaultSetting = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = ? and [IsModule] = True", strParentCaption));
                                            if (objModuleDefaultSetting == null)
                                            {
                                                objModuleDefaultSetting = new DefaultSetting(uow);
                                                objModuleDefaultSetting.ModuleName = strParentCaption;
                                                objModuleDefaultSetting.NavigationItemNameID = strparentid;
                                                objModuleDefaultSetting.IsModule = true;
                                                objModuleDefaultSetting.Select = true;
                                                objModuleDefaultSetting.ItemPath = strmodulepath;
                                                objModuleDefaultSetting.Save();
                                            }
                                            else
                                            {
                                                objModuleDefaultSetting.ItemPath = strmodulepath;
                                            }
                                            existingdefsetting.Add(objModuleDefaultSetting.Oid);
                                        }
                                        else
                                        {
                                            DefaultSetting objModuleDefaultSetting = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = ? and [IsModule] = True", strParentCaption));
                                            if (objModuleDefaultSetting == null)
                                            {
                                                objModuleDefaultSetting = new DefaultSetting(uow);
                                                objModuleDefaultSetting.ModuleName = strParentCaption;
                                                objModuleDefaultSetting.NavigationItemNameID = strsubparentid;
                                                objModuleDefaultSetting.IsModule = true;
                                                objModuleDefaultSetting.Select = true;
                                                objModuleDefaultSetting.ItemPath = strmodulepath;
                                                objModuleDefaultSetting.Save();
                                            }
                                            else
                                            {
                                                objModuleDefaultSetting.ItemPath = strmodulepath;
                                            }
                                            existingdefsetting.Add(objModuleDefaultSetting.Oid);
                                        }

                                        if (arrParentcaption != null && arrParentcaption.Length == 4)
                                        {
                                            DefaultSetting objsubModuleDefaultSetting = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = ? and [NavigationItemNameID] = ?", mainParentmodulename, childParentid));
                                            if (objsubModuleDefaultSetting == null)
                                            {
                                                objsubModuleDefaultSetting = new DefaultSetting(uow);
                                                objsubModuleDefaultSetting.ModuleName = mainParentmodulename;
                                                objsubModuleDefaultSetting.NavigationItemNameID = childParentid;
                                                objsubModuleDefaultSetting.NavigationItemName = subParentmodulename;
                                                objsubModuleDefaultSetting.IsModule = false;
                                                objsubModuleDefaultSetting.Select = true;
                                                objsubModuleDefaultSetting.ItemPath = strPath;
                                                objsubModuleDefaultSetting.Save();
                                            }
                                            else
                                            {
                                                objsubModuleDefaultSetting.ItemPath = strPath;
                                            }
                                            existingdefsetting.Add(objsubModuleDefaultSetting.Oid);
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(mainParentmodulename) && !string.IsNullOrEmpty(strparentid) && !string.IsNullOrEmpty(subParentmodulename))
                                            {
                                                DefaultSetting objsubModuleDefaultSetting = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = ? and [NavigationItemNameID] = ?", mainParentmodulename, strparentid));
                                                if (objsubModuleDefaultSetting == null)
                                                {
                                                    objsubModuleDefaultSetting = new DefaultSetting(uow);
                                                    objsubModuleDefaultSetting.ModuleName = mainParentmodulename;
                                                    objsubModuleDefaultSetting.NavigationItemNameID = strparentid;
                                                    objsubModuleDefaultSetting.NavigationItemName = subParentmodulename;
                                                    objsubModuleDefaultSetting.IsModule = false;
                                                    objsubModuleDefaultSetting.Select = true;
                                                    objsubModuleDefaultSetting.ItemPath = strPath;
                                                    objsubModuleDefaultSetting.Save();
                                                }
                                                else
                                                {
                                                    objsubModuleDefaultSetting.ItemPath = strPath;
                                                }
                                                existingdefsetting.Add(objsubModuleDefaultSetting.Oid);
                                            }
                                            if (!string.IsNullOrEmpty(mainParentid) && !string.IsNullOrEmpty(mainParentmodulename))
                                            {
                                                DefaultSetting objmainprtDefaultSetting = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = ? and [ModuleName] = ?", mainParentid, mainParentmodulename));
                                                if (objmainprtDefaultSetting == null)
                                                {
                                                    objmainprtDefaultSetting = new DefaultSetting(uow);
                                                    objmainprtDefaultSetting.ModuleName = mainParentmodulename;
                                                    objmainprtDefaultSetting.NavigationItemNameID = mainParentid;
                                                    objmainprtDefaultSetting.IsModule = true;
                                                    objmainprtDefaultSetting.Select = true;
                                                    objmainprtDefaultSetting.ItemPath = strmodulepath;
                                                    objmainprtDefaultSetting.Save();
                                                }
                                                else
                                                {
                                                    objmainprtDefaultSetting.ItemPath = strmodulepath;
                                                }
                                                existingdefsetting.Add(objmainprtDefaultSetting.Oid);
                                            }
                                            if (!string.IsNullOrEmpty(subParentmodulename) && !string.IsNullOrEmpty(strparentid))
                                            {
                                                DefaultSetting objmainprtDefaultSetting = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = ? and [ModuleName] = ? and [IsModule] = True", strparentid, subParentmodulename));
                                                if (objmainprtDefaultSetting == null)
                                                {
                                                    objmainprtDefaultSetting = new DefaultSetting(uow);
                                                    objmainprtDefaultSetting.ModuleName = subParentmodulename;
                                                    objmainprtDefaultSetting.NavigationItemNameID = strparentid;
                                                    objmainprtDefaultSetting.IsModule = true;
                                                    objmainprtDefaultSetting.Select = true;
                                                    objmainprtDefaultSetting.ItemPath = strmodulepath;
                                                    objmainprtDefaultSetting.Save();
                                                }
                                                else
                                                {
                                                    objmainprtDefaultSetting.ItemPath = strmodulepath;
                                                }
                                                existingdefsetting.Add(objmainprtDefaultSetting.Oid);
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(subParentmodulename) && !string.IsNullOrEmpty(strsubparentid) && !string.IsNullOrEmpty(strsubmodule))
                                        {
                                            DefaultSetting objmainprtDefaultSetting = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = ? and [ModuleName] = ?", strsubparentid, subParentmodulename));
                                            if (objmainprtDefaultSetting == null)
                                            {
                                                objmainprtDefaultSetting = new DefaultSetting(uow);
                                                objmainprtDefaultSetting.ModuleName = subParentmodulename;
                                                objmainprtDefaultSetting.NavigationItemNameID = strsubparentid;
                                                objmainprtDefaultSetting.NavigationItemName = strsubmodule;
                                                objmainprtDefaultSetting.Select = true;
                                                objmainprtDefaultSetting.ItemPath = strPath;
                                                objmainprtDefaultSetting.Save();
                                            }
                                            else
                                            {
                                                objmainprtDefaultSetting.ItemPath = strPath;
                                            }
                                            existingdefsetting.Add(objmainprtDefaultSetting.Oid);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (existingNavItems != null && existingNavItems.Count > 0)
                    {
                        string strCriteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", existingNavItems.Select(i => i.ToString().Replace("'", "''")))) + ")";
                        if (!string.IsNullOrEmpty(strCriteria))
                        {
                            XPClassInfo NavigationIteminfo;
                            NavigationIteminfo = uow.GetClassInfo(typeof(NavigationItem));
                            IList<NavigationItem> allNavItems = uow.GetObjects(NavigationIteminfo, CriteriaOperator.Parse(strCriteria), new SortingCollection(), 0, 0, false, true).Cast<NavigationItem>().ToList();
                            if (allNavItems != null && allNavItems.Count > 0)
                            {
                                uow.Delete(allNavItems);
                            }
                        }
                    }
                    if (existingdefsetting != null && existingdefsetting.Count > 0)
                    {
                        string strdefCriteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", existingdefsetting.Select(i => i.ToString().Replace("'", "''")).Distinct())) + ")";
                        if (!string.IsNullOrEmpty(strdefCriteria))
                        {
                            XPClassInfo DefaultSettinginfo;
                            DefaultSettinginfo = uow.GetClassInfo(typeof(DefaultSetting));
                            IList<DefaultSetting> alldefItems = uow.GetObjects(DefaultSettinginfo, CriteriaOperator.Parse(strdefCriteria), new SortingCollection(), 0, 0, false, true).Cast<DefaultSetting>().ToList();
                            if (alldefItems != null && alldefItems.Count > 0)
                            {
                                uow.Delete(alldefItems);
                            }
                        }
                    }
                    uow.CommitChanges();
                    Dictionary<string, string> lstDashboardNavigation = new Dictionary<string, string>();
                    lstDashboardNavigation.Add("Analytics", "Analytics");
                    lstDashboardNavigation.Add("SampleBacklog", "Sample Backlog");
                    foreach (KeyValuePair<string, string> objNav in lstDashboardNavigation)
                    {
                        DefaultSetting objModuleDefaultSetting = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] =? and [IsModule] = True", objNav.Key));
                        if (objModuleDefaultSetting == null)
                        {
                            DefaultSetting objNewModule = new DefaultSetting(uow);
                            objNewModule.ModuleName = objNav.Value;
                            objNewModule.NavigationItemNameID = objNav.Key;
                            objNewModule.NavigationItemName = objNav.Key;
                            objNewModule.NavigationCaption = objNav.Value;
                            objNewModule.IsModule = true;
                            objNewModule.ItemPath = objNav.Value;
                            objNewModule.Select = true;
                            if (!existingdefsetting.Contains(objNewModule.Oid))
                            {
                                existingdefsetting.Add(objNewModule.Oid);
                            }
                            objNewModule.Save();
                        }
                        else
                        {
                            objModuleDefaultSetting.ItemPath = objNav.Value;
                            objModuleDefaultSetting.ModuleName = objNav.Value;
                            objModuleDefaultSetting.NavigationItemName = objNav.Key;
                            objModuleDefaultSetting.NavigationCaption = objNav.Value;
                            if (!existingdefsetting.Contains(objModuleDefaultSetting.Oid))
                            {
                                existingdefsetting.Add(objModuleDefaultSetting.Oid);
                            }
                        }
                        //Modules.BusinessObjects.Setting.NavigationItem UserNavigation = uow.FindObject<Modules.BusinessObjects.Setting.NavigationItem>(
                        //         new BinaryOperator("NavigationId", objNav.Key));
                        Modules.BusinessObjects.Setting.NavigationItem UserNavigation = uow.FindObject<Modules.BusinessObjects.Setting.NavigationItem>(CriteriaOperator.Parse("[NavigationId] = ? And [GCRecord] is NULL", objNav.Value));
                        if (UserNavigation == null)
                        {
                            UserNavigation = new NavigationItem(uow);
                            UserNavigation.NavigationId = objNav.Key;
                            UserNavigation.NavigationCaption = objNav.Value;
                            UserNavigation.NavigationCNCaption = objNav.Value;
                            UserNavigation.Parent = objNav.Value; ;
                            UserNavigation.Exclude = false;
                            UserNavigation.Exist = true;
                            UserNavigation.Select = true;
                            if (!existingNavItems.Contains(UserNavigation.Oid))
                            {
                                existingNavItems.Add(UserNavigation.Oid);
                            }
                        }
                        else
                        {
                            UserNavigation.NavigationCaption = objNav.Value;
                            UserNavigation.NavigationCNCaption = objNav.Value;
                            if (!existingNavItems.Contains(UserNavigation.Oid))
                            {
                                existingNavItems.Add(UserNavigation.Oid);
                            }
                        }
                        foreach (DashboardData dashboarddata in uow.Query<DashboardData>())
                        {
                            AssignDashboardToUserDepartment objAdtu = uow.FindObject<AssignDashboardToUserDepartment>(CriteriaOperator.Parse("[DashboardViewName.Oid] = ?", dashboarddata.Oid));
                            if (objAdtu != null)
                            {
                                DefaultSetting objOldDashboardItem = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = ? and [IsModule] = false And NavigationItemNameID=?", objNav.Key, dashboarddata.ToString()));
                                if (objOldDashboardItem == null)
                                {
                                    DefaultSetting objNewDashboard = new DefaultSetting(uow);
                                    objNewDashboard.ModuleName = objNav.Key;
                                    objNewDashboard.NavigationItemNameID = dashboarddata.ToString();
                                    objNewDashboard.NavigationItemName = dashboarddata.ToString();
                                    objNewDashboard.IsModule = false;
                                    objNewDashboard.Select = true;
                                    objNewDashboard.NavigationCaption = dashboarddata.Title;
                                    if (!existingdefsetting.Contains(objNewDashboard.Oid))
                                    {
                                        existingdefsetting.Add(objNewDashboard.Oid);
                                    }
                                    objNewDashboard.Save();
                                }
                                else
                                {
                                    objOldDashboardItem.ModuleName = objNav.Value;
                                    objOldDashboardItem.NavigationItemNameID = dashboarddata.ToString();
                                    objOldDashboardItem.NavigationItemName = dashboarddata.ToString();
                                    objOldDashboardItem.IsModule = false;
                                    objOldDashboardItem.NavigationCaption = dashboarddata.Title;
                                    if (!existingdefsetting.Contains(objOldDashboardItem.Oid))
                                    {
                                        existingdefsetting.Add(objOldDashboardItem.Oid);
                                    }
                                }
                            }
                        }
                        uow.CommitChanges();
                    }
                }
                uow.CommitChanges();
                #endregion

                #region HCPDetails
                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\")) == false)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\"));
                }
                string strLocalFile = HttpContext.Current.Server.MapPath((@"~\HPCDetails.txt"));
                if (File.Exists(strLocalFile))
                {
                    string strHCdetails = "";
                    File.WriteAllText(strLocalFile, strHCdetails);
                }
                #endregion 
            }
            TimeZoneinfo objTimeZone = new TimeZoneinfo();
            //string Culture = string.Empty;
            string strOffSet = string.Empty;
            string strDate = string.Empty;
            if (HttpContext.Current.Request.Cookies.Get("Offset") != null)
            {
                strOffSet = HttpContext.Current.Request.Cookies.Get("Offset").Value;
            }
            if (HttpContext.Current.Request.Cookies.Get("Date") != null)
            {
                strDate = HttpContext.Current.Request.Cookies.Get("Date").Value;
            }
            objTimeZone.Date = strDate;
            if (!string.IsNullOrEmpty(strOffSet))
            {
                string jsNumberOfMinutesOffset = strOffSet;   // sending the above offset
                var timeZones = TimeZoneInfo.GetSystemTimeZones();
                var numberOfMinutes = Int32.Parse(jsNumberOfMinutesOffset) * (-1);
                var timeSpan = TimeSpan.FromMinutes(numberOfMinutes);
                TimeZoneInfo userTimeZone = timeZones.Where(tz => tz.BaseUtcOffset == timeSpan).FirstOrDefault();
                objTimeZone.TimeZone = userTimeZone;
            }

            MessageTimer timer = new MessageTimer();
            timer.Seconds = Convert.ToInt32(ConfigurationManager.AppSettings["MessageTimer"]);

            //NotificationsModule module = this.Modules.FindModule<NotificationsModule>();
            //module.ShowNotificationsWindow = false;
            //module.NotificationsService.Refresh();
        }

        //public void NextRecurrence(COCSettings objCocSetting)
        //{
        //    IObjectSpace os = CreateObjectSpace();
        //    Session currentSession = ((XPObjectSpace)(os)).Session;
        //    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
        //    Tasks objCurrentTasks = new Tasks(uow);
        //    Samplecheckin objNewSampleCheckIn = new Samplecheckin(uow);
        //    if (objCocSetting.Client != null)
        //    {
        //        objNewSampleCheckIn.ClientName = objCurrentTasks.Client = uow.GetObjectByKey<Customer>(objCocSetting.Client.Oid);
        //    }
        //    if (objCocSetting.ProjectID != null)
        //    {
        //        objNewSampleCheckIn.ProjectID = objCurrentTasks.ProjectID = uow.GetObjectByKey<Project>(objCocSetting.ProjectID.Oid);
        //    }

        //    objCurrentTasks.ProjectName = objCocSetting.ProjectName;
        //    objNewSampleCheckIn.ProjectLocation = objCurrentTasks.ProjectLocation = objCocSetting.ProjectLocation;
        //    objNewSampleCheckIn.ClientAddress = objCurrentTasks.ClientAddress = objCocSetting.Address;
        //    objCurrentTasks.Status = TaskManagementStatus.PendingSubmit;
        //    objNewSampleCheckIn.Status = SampleRegistrationSignoffStatus.PendingSubmit;
        //    objNewSampleCheckIn.SampleMatries = objCurrentTasks.SampleMatrix = objCocSetting.SampleMatrix.Oid.ToString();
        //    uow.CommitChanges();
        //    XPClassInfo sampleloginfo;
        //    sampleloginfo = uow.GetClassInfo(typeof(COCSettingsSamples));
        //    IList<COCSettingsSamples> objCocSettingSamples = uow.GetObjects(sampleloginfo, CriteriaOperator.Parse("[COC.Oid]=?", objCocSetting.Oid), null, int.MaxValue, false, true).Cast<COCSettingsSamples>().ToList();
        //    if (objCocSettingSamples != null && objCocSettingSamples.Count > 0)
        //    {
        //        foreach (COCSettingsSamples obj in objCocSettingSamples)
        //        {
        //            SelectedData sproc = currentSession.ExecuteSproc("GetSamplingSampleID", new OperandValue(objCurrentTasks.Oid.ToString()));
        //            Sampling objNewSampling = new Sampling(uow);
        //            Modules.BusinessObjects.SampleManagement.SampleLogIn objNewSample = new Modules.BusinessObjects.SampleManagement.SampleLogIn(uow);
        //            if (sproc.ResultSet[1].Rows[0].Values[0].ToString() != null)
        //            {
        //                objNewSample.SampleNo = objNewSampling.SampleNo = (int)sproc.ResultSet[1].Rows[0].Values[0];
        //                objNewSample.SortOrder = objNewSampling.SortOrder = objNewSampling.SampleNo;
        //            }
        //            if (obj.SiteName != null)
        //            {
        //                objNewSample.StationLocation = objNewSampling.StationLocation = obj.SiteName.StationLocation;
        //            }
        //            if (obj.VisualMatrix != null)
        //            {
        //                objNewSample.VisualMatrix = objNewSampling.VisualMatrix = uow.GetObjectByKey<VisualMatrix>(obj.VisualMatrix.Oid);
        //            }
        //            if (objCurrentTasks != null)
        //            {
        //                objNewSampling.Tasks = uow.GetObjectByKey<Tasks>(objCurrentTasks.Oid);
        //            }
        //            if (objNewSampleCheckIn != null)
        //            {
        //                objNewSample.JobID = uow.GetObjectByKey<Samplecheckin>(objNewSampleCheckIn.Oid);
        //            }
        //            if (obj.Testparameters != null && obj.Testparameters.Count > 0)
        //            {
        //                foreach (Testparameter testparameter in obj.Testparameters)
        //                {
        //                    objNewSampling.Testparameters.Add(uow.GetObjectByKey<Testparameter>(testparameter.Oid));
        //                    objNewSample.Testparameters.Add(uow.GetObjectByKey<Testparameter>(testparameter.Oid));
        //                }
        //            }
        //            if (obj.AssignedBy != null)
        //            {
        //                objNewSample.AssignedBy = objNewSampling.AssignedBy = uow.GetObjectByKey<Employee>(obj.AssignedBy.Oid);

        //            }
        //            objNewSample.AssignTo = objNewSampling.AssignTo = obj.AssignTo;
        //            objNewSample.CollectDate = objNewSampling.CollectDate = obj.CollectDate;
        //            objNewSample.CollectTime = objNewSampling.CollectTime = obj.CollectTime;

        //            if (obj.Collector != null)
        //            {
        //                objNewSample.Collector = objNewSampling.Collector = uow.GetObjectByKey<Collector>(obj.Collector.Oid);
        //            }
        //            objNewSample.Comment = objNewSampling.Comment = obj.Comment;
        //            objNewSample.EquipmentName = objNewSampling.EquipmentName = obj.EquipmentName;

        //            if (obj.Preservetives != null)
        //            {
        //                objNewSample.Preservetives = objNewSampling.Preservetives = obj.Preservetives;
        //            }
        //            if (obj.QCSource != null)
        //            {
        //                objNewSampling.QCSource = uow.GetObjectByKey<QCType>(obj.QCSource.Oid);
        //            }
        //            if (obj.QCType != null)
        //            {
        //                objNewSample.QCType = objNewSampling.QCType = uow.GetObjectByKey<QCType>(obj.QCType.Oid);
        //            }
        //            objNewSampling.Qty = obj.Qty;
        //            objNewSample.Qty = (uint)obj.Qty;

        //            objNewSample.SampleName = objNewSampling.SampleName = obj.SampleName;
        //            if (obj.SampleType != null)
        //            {
        //                objNewSample.SampleType = objNewSampling.SampleType = uow.GetObjectByKey<SampleType>(obj.SampleType.Oid);
        //            }
        //            if (obj.Storage != null)
        //            {
        //                objNewSample.Storage = objNewSampling.Storage = uow.GetObjectByKey<Storage>(obj.Storage.Oid);
        //            }
        //            objNewSample.SubOut = objNewSampling.SubOut = obj.SubOut;
        //            objNewSampling.Timemin = obj.Timemin;
        //            objNewSample.TimeEnd = objNewSampling.TimeEnd = obj.TimeEnd;
        //            objNewSample.TimeStart = objNewSampling.TimeStart = obj.TimeStart;
        //            objNewSample.SiteName = objNewSampling.SiteName = obj.SiteName;
        //            objNewSample.SiteDescription = objNewSampling.SiteDescription = obj.SiteDescription;
        //            objNewSample.PWSID = objNewSampling.PWSID = obj.PWSID;
        //            objNewSample.PWSSystemName = objNewSampling.PWSSystemName = obj.PWSSystemName;
        //            objNewSample.KeyMap = objNewSampling.KeyMap = obj.KeyMap;
        //            objNewSample.ServiceArea = objNewSampling.ServiceArea = obj.ServiceArea;
        //            objNewSample.SiteNameArchived = objNewSampling.SiteNameArchived = obj.SiteNameArchived;
        //            objNewSample.IsActive = objNewSampling.IsActive = obj.IsActive;
        //            objNewSample.City = objNewSampling.City = obj.City;
        //            objNewSample.ZipCode = objNewSampling.ZipCode = obj.ZipCode;
        //            objNewSample.FacilityID = objNewSampling.FacilityID = obj.FacilityID;
        //            objNewSample.FacilityName = objNewSampling.FacilityName = obj.FacilityName;
        //            objNewSample.FacilityType = objNewSampling.FacilityType = obj.FacilityType;
        //            objNewSample.SamplePointType = objNewSampling.SamplePointType = obj.SamplePointType;
        //            objNewSample.WaterType = objNewSampling.WaterType = obj.WaterType;
        //            objNewSample.Longitude = objNewSampling.Longitude = obj.Longitude;
        //            objNewSample.Latitude = objNewSampling.Latitude = obj.Latitude;
        //            objNewSample.MonitoryingRequirement = objNewSampling.MonitoryingRequirement = obj.MonitoryingRequirement;
        //            objNewSample.ParentSampleID = objNewSampling.ParentSampleID = obj.ParentSampleID;
        //            objNewSample.ParentSampleDate = objNewSampling.ParentSampleDate = obj.ParentSampleDate;
        //            objNewSample.RepeatLocation = objNewSampling.RepeatLocation = obj.RepeatLocation;

        //            if (obj.VisualMatrix != null)
        //            {
        //                objNewSample.VisualMatrix = objNewSampling.VisualMatrix = uow.GetObjectByKey<VisualMatrix>(obj.VisualMatrix.Oid);
        //            }
        //            SamplingStation objStation = uow.FindObject<SamplingStation>(CriteriaOperator.Parse("[Sampling.Oid] = ?", objNewSampling.Oid));
        //            if (objStation == null)
        //            {
        //                objStation = new SamplingStation(uow);
        //                objStation.Sampling = objNewSampling;
        //                objStation.SampleLocation = objNewSampling.StationLocation;
        //                objStation.SiteName = objNewSampling.StationLocation;
        //                objStation.Matrix = objNewSampling.VisualMatrix;
        //            }
        //            IList<SampleBottleAllocation> lstsmplbtl = os.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[COCSettings.Oid] = ?", obj.Oid));
        //            if (lstsmplbtl != null && lstsmplbtl.Count > 0)
        //            {
        //                foreach (SampleBottleAllocation objsmpl in lstsmplbtl.ToList())
        //                {
        //                    SampleBottleAllocation newsmplbtl = new SampleBottleAllocation(uow);
        //                    newsmplbtl.BottleSet = objsmpl.BottleSet;
        //                    newsmplbtl.SharedTests = objsmpl.SharedTests;
        //                    newsmplbtl.SharedTestsGuid = objsmpl.SharedTestsGuid;
        //                    newsmplbtl.Qty = objsmpl.Qty;
        //                    newsmplbtl.BottleID = objsmpl.BottleID;
        //                    if (objsmpl.Containers != null)
        //                    {
        //                        newsmplbtl.Containers = uow.GetObjectByKey<Modules.BusinessObjects.Setting.Container>(objsmpl.Containers.Oid);
        //                    }
        //                    if (objsmpl.Preservative != null)
        //                    {
        //                        newsmplbtl.Preservative = uow.GetObjectByKey<Preservative>(objsmpl.Preservative.Oid);
        //                    }
        //                    newsmplbtl.TaskRegistration = objNewSampling;
        //                    newsmplbtl.SampleRegistration = objNewSample;
        //                }
        //            }
        //            uow.CommitChanges();
        //        }
        //    }
        //}
        //public void MonthlyYearly(RecurrenceInfo info, COCSettings objCocSetting, UnitOfWork uow)
        //{
        //    DateTime DT = new DateTime();

        //    for (int a = 0; a <= 7; a++)
        //    {
        //        if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Monday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Monday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }
        //        }
        //        else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Tuesday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Tuesday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }
        //        }
        //        else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Wednesday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Wednesday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }
        //        }
        //        else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Thursday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Thursday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }
        //        }
        //        else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Friday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Friday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }
        //        }
        //        else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Saturday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Saturday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }
        //        }
        //        else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Sunday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Sunday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }
        //        }
        //        if (info.WeekOfMonth == WeekOfMonth.Last)
        //        {
        //            DateTime DT1 = new DateTime();
        //            DT1 = objCocSetting.NextUpdateDate;
        //            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //            if (objCocSetting.NextUpdateDate.Month != DT1.Month)
        //            {
        //                objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                a = 0;
        //            }
        //        }
        //        else
        //        {
        //            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //        }
        //    }
        //}
        private static string Findcaption(IModelNode values, string strParent)
        {
            if (values.Parent != null)
            {
                if (values.GetValue<string>("Id") != "Items" && values.GetValue<string>("Id") != "Application" && values.GetValue<string>("Id") != "NavigationItems")
                {
                    if (strParent.Length == 0)
                    {
                        strParent = values.GetValue<string>("Caption");
                    }
                    else
                    {
                        strParent = values.GetValue<string>("Caption") + @"\" + strParent;
                    }
                }
                strParent = Findcaption(values.Parent, strParent);
                return strParent;
            }
            else
            {
                return strParent;
            }
        }
        private static string Findparentid(IModelNode values, string strParentid)
        {
            if (values.Parent != null)
            {
                if (values.GetValue<string>("Id") != "Items" && values.GetValue<string>("Id") != "Application" && values.GetValue<string>("Id") != "NavigationItems")
                {
                    if (strParentid.Length == 0)
                    {
                        strParentid = values.GetValue<string>("Id");
                    }
                    else
                    {
                        strParentid = values.GetValue<string>("Caption") + @"\" + strParentid;
                    }
                }
                strParentid = Findparentid(values.Parent, strParentid);
                return strParentid;
            }
            else
            {
                return strParentid;
            }
        }

        private static string Findmainparentid(IModelNode values, string strParentid)
        {
            if (values.Parent != null)
            {
                if (values.Parent.GetValue<string>("Id") == "Items")
                {
                    if (strParentid.Length == 0)
                    {
                        strParentid = values.Parent.Parent.GetValue<string>("Id");
                    }
                }
                strParentid = Findmainparentid(values.Parent, strParentid);
                return strParentid;
            }
            else
            {
                return strParentid;
            }
        }

        private static string Findsubparentid(IModelNode values, string strParentid)
        {
            if (values.Parent != null)
            {
                if (values.Parent.GetValue<string>("Id") == "Items")
                {
                    if (strParentid.Length == 0)
                    {
                        strParentid = values.Parent.Parent.GetValue<string>("Id");
                    }
                }
                strParentid = Findsubparentid(values.Parent, strParentid);
                return strParentid;
            }
            else
            {
                return strParentid;
            }
        }

        private static bool Findvisible(IModelNode values, bool strparentid)
        {
            if (values.Parent != null)
            {
                if (values.GetValue<string>("Id") != "Items" && values.GetValue<string>("Id") != "Application" && values.GetValue<string>("Id") != "NavigationItems")
                {
                    strparentid = values.GetValue<bool>("Visible");
                }
                strparentid = Findvisible(values.Parent, strparentid);
                return strparentid;
            }
            else
            {
                return strparentid;
            }
        }
        private static bool Findsubitemvisible(IModelNode values, bool strparentid)
        {
            if (values.Parent != null)
            {
                if (values.GetValue<string>("Id") != "Items" && values.GetValue<string>("Id") != "Application" && values.GetValue<string>("Id") != "NavigationItems")
                {
                    strparentid = values.GetValue<bool>("Visible");
                }
                return strparentid;
            }
            else
            {
                return strparentid;
            }
        }
    }
}
