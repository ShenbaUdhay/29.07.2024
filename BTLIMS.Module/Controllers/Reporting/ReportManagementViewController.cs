using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Pdf;
using DevExpress.Web;
using DevExpress.XtraReports.UI;
using iTextSharp.text.pdf;
using LDM.Module.Controllers.Public;
using LDM.Module.Controllers.Public.FTPSetup;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.Invoicing;
using Modules.BusinessObjects.SuboutTracking;
//using Rebex.Net;
//using Rebex.Net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using static DevExpress.Utils.MVVM.Services.DocumentManagerService;
using static DevExpress.XtraPrinting.Native.PageSizeInfo;

namespace LDM.Module.Controllers.Reporting
{
    public partial class ReportManagementViewController : ViewController, IXafCallbackHandler
    {
        public string strFTPServerName = string.Empty;
        public string strFTPPath = string.Empty;
        public string strFTPUserName = string.Empty;
        public string strFTPPassword = string.Empty;
        public int FTPPort = 0;
        public bool strFTPStatus;
        string strExportedPath = string.Empty;
        DynamicReportDesignerConnection objRInfo = new DynamicReportDesignerConnection();
        ReportManagement report = new ReportManagement();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        MessageTimer timer = new MessageTimer();
        IObjectSpace Popupos;
        DefaultSettingInfo objDefaultInfo = new DefaultSettingInfo();
        DefaultSetting objDefaultReportDelivery;
        DefaultSetting objDefaultReportArchive;
        DefaultSetting objDefaultReportprintanddownload;
        string ReportPrintcriteriaString = "[ReportStatus]='PendingDelivery'";
        string ReportDeliveryCriteriaString = "[ReportStatus]='PendingArchive'";
        curlanguage objLanguage = new curlanguage();
        DynamicReportDesignerConnection ObjReportDesignerInfo = new DynamicReportDesignerConnection();
        FtpInfo objFTP = new FtpInfo();

        public ReportManagementViewController()
        {
            InitializeComponent();
            TargetViewId = "Reporting_ListView_PrintDownload;"
                + "Reporting_DetailView_PrintDownload;"
                + "Reporting_ListView_Delivery;"
                + "Reporting_DetailView_Delivery;"
                + "Reporting_ListView_Archieve;"
                + "Reporting_DetailView_Archived;"
                + "Reporting_ListView_Account;"
                + "Reporting_ListView_Recall;"
                + "Reporting_DetailView_Recalled;"
                + "PDFPreview_DetailView;"
                + "Reporting_ListView;"
                + "Reporting_ListView_Copy_ReportApproval;"
                + "Reporting_ListView_Copy_ReportView;"
                + "Reporting_ListView_Level1Review_View;"
                + "Reporting_ListView_Level2Review_View;"
                + "Invoicing_ListView_Review;"
                + "Invoicing_DetailView_Queue;"
                + "Invoicing_ListView_Delivery;"
                + "SubOutSampleRegistrations_ListView_SigningOffHistory;" + "Reporting_ListView_Deliveired;" + "Reporting_ListView_Datacenter;";
            ReportManagementView.TargetViewId = "Reporting_ListView_PrintDownload;" + "Reporting_ListView_Delivery;" + "Reporting_ListView_Archieve;" + "Reporting_ListView_Recall;";
            ReportManagementDelete.TargetViewId = "Reporting_ListView_PrintDownload;" + "Reporting_ListView_Delivery;" + "Reporting_ListView_Archieve;";
            ReportDateFilterAction.TargetViewId = "Reporting_ListView_PrintDownload;" + "Reporting_ListView_Delivery;" + "Reporting_ListView_Archieve;" + "Reporting_ListView_Recall;"
                + "Reporting_ListView_Level1Review_View;" + "Reporting_ListView_Level2Review_View;" + "SubOutSampleRegistrations_ListView_SigningOffHistory;";
            ReportManagementPreview.TargetViewId = "Reporting_DetailView_PrintDownload;" + "Reporting_DetailView_Delivery;" + "Reporting_DetailView_Archived;" + "Reporting_DetailView_Recalled;"
                + "Reporting_ListView_PrintDownload;"/* + "Reporting_ListView_Delivery;"*/ + "Reporting_ListView_Archieve;" + "Reporting_ListView_Recall;" + "Reporting_ListView_Account;"
                + "Reporting_ListView;" + "Reporting_ListView_Copy_ReportApproval;" + "Reporting_ListView_Level1Review_View;" + "Reporting_ListView_Level2Review_View;";
            ReportManagementPreviewDC.TargetViewId = "Reporting_ListView_Datacenter;";
            ReportPrint.TargetViewId = "PDFPreview_DetailView;";
            ReportDeliverySave.TargetViewId = "Reporting_ListView_Delivery;" + "Reporting_ListView_Deliveired;";
            ReportDeliveryPreview.TargetViewId = "Reporting_ListView_Delivery;" + "Reporting_ListView_Deliveired;";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                objDefaultReportDelivery = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportDelivery'"));
                objDefaultReportArchive = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportArchive'"));

                if (objDefaultReportprintanddownload != null && objDefaultReportprintanddownload.Select == true)
                {
                    objDefaultInfo.boolReportPrintDownload = true;
                }
                if (objDefaultReportDelivery != null && objDefaultReportDelivery.Select == true)
                {
                    objDefaultInfo.boolReportdelivery = true;
                }
                if (objDefaultReportArchive != null && objDefaultReportArchive.Select == true)
                {
                    objDefaultInfo.boolReportArchive = true;
                }
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule += RuleSet_CustomNeedToValidateRule;
                if (View.GetType().Name == "ListView")
                {
                    report.Mode = string.Empty;
                    //if (View.Id == "Reporting_ListView_Delivery")
                    //{
                    //    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                    //}
                    ReportDateFilterAction.SelectedItemChanged += ReportDateFilterAction_SelectedItemChanged;
                    //report.ReportFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    //ReportDateFilterAction.SelectedItem = ReportDateFilterAction.Items[0];
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    ReportDateFilterAction.SelectedItem = null;
                    if (ReportDateFilterAction.SelectedItem == null)
                    {                      
                        if (setting.ReportingWorkFlow == EnumDateFilter.OneMonth)
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    report.ReportFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    ReportDateFilterAction.SelectedItem = ReportDateFilterAction.Items[0];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.ThreeMonth)
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                            report.ReportFilterByMonthDate = DateTime.Today.AddMonths(-3);
                            ReportDateFilterAction.SelectedItem = ReportDateFilterAction.Items[1];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.SixMonth)
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-6);
                            report.ReportFilterByMonthDate = DateTime.Today.AddMonths(-6);
                            ReportDateFilterAction.SelectedItem = ReportDateFilterAction.Items[2];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.OneYear)
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-1);
                            report.ReportFilterByMonthDate = DateTime.Today.AddYears(-1);
                            ReportDateFilterAction.SelectedItem = ReportDateFilterAction.Items[3];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.TwoYear)
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-2);
                            report.ReportFilterByMonthDate = DateTime.Today.AddYears(-2);
                            ReportDateFilterAction.SelectedItem = ReportDateFilterAction.Items[4];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.FiveYear)
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-5);
                            report.ReportFilterByMonthDate = DateTime.Today.AddYears(-5);
                            ReportDateFilterAction.SelectedItem = ReportDateFilterAction.Items[5];
                        }
                        else
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.MinValue;
                            report.ReportFilterByMonthDate = DateTime.MinValue;
                            ReportDateFilterAction.SelectedItem = ReportDateFilterAction.Items[6];
                        }
                        if (report.ReportFilterByMonthDate != DateTime.MinValue)
                        {
                            if (View.Id == "Reporting_ListView_PrintDownload")
                            {
                                if (report.Mode == "Enter")
                                {
                                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[ReportedDate] BETWEEN('" + report.ReportFilterByMonthDate + "', '" + DateTime.Now + "')");
                                }
                                else
                                {
                                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DatePrinted] BETWEEN('" + report.ReportFilterByMonthDate + "', '" + DateTime.Now + "')");
                                }
                            }
                            else if (View.Id == "Reporting_ListView_Delivery")
                            {
                                if (report.Mode == "Enter")
                                {
                                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DatePrinted] >= ? and [DatePrinted] < ?", report.ReportFilterByMonthDate, DateTime.Now);
                                }
                                else
                                {
                                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DateDelivered] >= ? and [DateDelivered] < ?", report.ReportFilterByMonthDate, DateTime.Now);
                                }
                            }
                            else if (View.Id == "Reporting_ListView_Archieve")
                            {
                                if (report.Mode == "Enter")
                                {
                                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DateDelivered] >= ? And [DateDelivered] <= ?", report.ReportFilterByMonthDate, DateTime.Now + "')");
                                }
                                else
                                {
                                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DateArchived] BETWEEN('" + report.ReportFilterByMonthDate + "', '" + DateTime.Now + "')");
                                }
                            }
                            else if (View.Id == "Reporting_ListView_Recall")
                            {
                                if (report.Mode == "Enter")
                                {
                                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DateDelivered] >= ? And [DateDelivered] <= ?", report.ReportFilterByMonthDate, DateTime.Now + "')");
                                }
                                else
                                {
                                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DateRecalled] BETWEEN('" + report.ReportFilterByMonthDate + "', '" + DateTime.Now + "')");
                                }
                            }
                            else if (View.Id == "Reporting_ListView_Level1Review_View")
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("ReportedDate>=? and ReportedDate<?", report.ReportFilterByMonthDate, DateTime.Now);
                            }
                            else if (View.Id == "Reporting_ListView_Level2Review_View")
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("ReportedDate>=? and ReportedDate<?", report.ReportFilterByMonthDate, DateTime.Now);
                            }
                        }
                        else
                        {
                            if (((ListView)View).CollectionSource.Criteria.ContainsKey("dateFilter"))
                            {
                                ((ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                            }
                        }
                        //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    }

                }
                else if (View.GetType().Name == "DetailView" && View.ObjectTypeInfo.Type != typeof(Invoicing))
                {
                    Frame.GetController<ModificationsController>().SaveAction.Active.SetItemValue("Enable", false);
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.ExecuteCompleted += SaveAndCloseAction_ExecuteCompleted;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Caption = Frame.GetController<ModificationsController>().SaveAction.Caption;
                    Frame.GetController<RefreshController>().RefreshAction.Executing += RefreshAction_Executing;
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                if (string.IsNullOrEmpty(report.Mode))
                {
                    report.Mode = "Enter";
                    ReportManagementView.Active.SetItemValue("Disable", true);
                    ReportManagementDelete.Active.SetItemValue("Disable", false);
                }
                Employee currentUser = SecuritySystem.CurrentUser as Employee;
                if (currentUser != null && View != null && View.Id != null)
                {

                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.CustomReportingIsWrite = false;
                        objPermissionInfo.CustomReportingIsDelete = false;
                        objPermissionInfo.ReportValidationIsWrite = false;
                        objPermissionInfo.ReportValidationIsDelete = false;
                        objPermissionInfo.ReportApprovalIsWrite = false;
                        objPermissionInfo.ReportApprovalIsDelete = false;
                        objPermissionInfo.ReportViewIsWrite = false;
                        objPermissionInfo.ReportViewIsDelete = false;
                        objPermissionInfo.ReportDeliveryIsWrite = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.CustomReportingIsWrite = true;
                            objPermissionInfo.CustomReportingIsDelete = true;
                            objPermissionInfo.ReportValidationIsWrite = true;
                            objPermissionInfo.ReportValidationIsDelete = true;
                            objPermissionInfo.ReportApprovalIsWrite = true;
                            objPermissionInfo.ReportApprovalIsDelete = true;
                            objPermissionInfo.ReportViewIsWrite = true;
                            objPermissionInfo.ReportViewIsDelete = true;
                            objPermissionInfo.ReportDeliveryIsWrite = true;
                        }
                        else
                        {
                            foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportPrintDownload" && i.Delete == true) != null)
                                {
                                    objPermissionInfo.ReportPrintIsDelete = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportDelivery" && i.Delete == true) != null)
                                {
                                    objPermissionInfo.ReportDeliveryIsDelete = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportDelivery" && i.Write == true) != null)
                                {
                                    objPermissionInfo.ReportDeliveryIsWrite = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportArchive" && i.Delete == true) != null)
                                {
                                    objPermissionInfo.ReportArchiveIsDelete = true;
                                    //return;
                                }
                            }
                        }
                    }
                    if (View.Id == "Reporting_ListView_PrintDownload")
                    {
                        ReportManagementDelete.Active.SetItemValue("ShowReportDelete", objPermissionInfo.ReportPrintIsDelete);
                    }
                    else
                    if (View.Id == "Reporting_ListView_Delivery")
                    {
                        ReportDeliverySave.Active.SetItemValue("ShowReportDeliverysave", objPermissionInfo.ReportDeliveryIsWrite);
                        ReportManagementDelete.Active.SetItemValue("ShowReportDelete", objPermissionInfo.ReportDeliveryIsDelete);
                    }
                    else
                    if (View.Id == "Reporting_ListView_Archieve")
                    {
                        ReportManagementDelete.Active.SetItemValue("ShowReportDelete", objPermissionInfo.ReportArchiveIsDelete);
                    }
                }
                if (View.Id == "Reporting_ListView_Delivery")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReportStatus] = 'PendingDelivery'");
                    report.IsCanRefresh = false;
                    //ObjectSpace.Committing += ObjectSpace_Committing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null)
                {
                    //View.ObjectSpace.CommitChanges();
                    Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)View.CurrentObject;
                    if (objreporting != null)
                    {
                        bool CanCommit = false;
                        if (!string.IsNullOrEmpty(objreporting.EmailAddress))
                        {
                            SendNotificationEmail(objreporting);
                            CanCommit = true;
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "EmailAddress"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                        if (CanCommit)
                        {
                            DefaultSetting objDefaultArchive = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportArchive'"));
                            if (objDefaultArchive != null)
                            {
                                objreporting.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objreporting.DateDelivered = DateTime.Now;
                                objreporting.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objreporting.ReportStatus = ReportStatus.PendingArchive;
                            }
                            else
                            {
                                objreporting.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objreporting.DateDelivered = DateTime.Now;
                                objreporting.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objreporting.DateArchived = DateTime.Now;
                                objreporting.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objreporting.ReportStatus = ReportStatus.Archived;
                            }
                            //View.ObjectSpace.Refresh();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeliverySuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            try
            {
                e.PopupControl.CustomizePopupWindowSize += PopupControl_CustomizePopupWindowSize;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        private void PopupControl_CustomizePopupWindowSize(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame.View.Id == "PDFPreview_DetailView")
                {
                    //e.Width = new System.Web.UI.WebControls.Unit(865);
                    e.Width = new System.Web.UI.WebControls.Unit(870);
                    e.Height = new System.Web.UI.WebControls.Unit(750);
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ReportDateFilterAction_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && ReportDateFilterAction != null && ReportDateFilterAction.SelectedItem != null)
                {
                    if (ReportDateFilterAction.SelectedItem.Id == "1M")
                    {
                        report.ReportFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    }
                    else if (ReportDateFilterAction.SelectedItem.Id == "3M")
                    {
                        report.ReportFilterByMonthDate = DateTime.Today.AddMonths(-3);
                    }
                    else if (ReportDateFilterAction.SelectedItem.Id == "6M")
                    {
                        report.ReportFilterByMonthDate = DateTime.Today.AddMonths(-6);
                    }
                    else if (ReportDateFilterAction.SelectedItem.Id == "1Y")
                    {
                        report.ReportFilterByMonthDate = DateTime.Today.AddYears(-1);
                    }
                    else if (ReportDateFilterAction.SelectedItem.Id == "2Y")
                    {
                        report.ReportFilterByMonthDate = DateTime.Today.AddYears(-2);
                    }
                    else if (ReportDateFilterAction.SelectedItem.Id == "5Y")
                    {
                        report.ReportFilterByMonthDate = DateTime.Today.AddYears(-5);
                    }
                    else
                    {
                        report.ReportFilterByMonthDate = DateTime.MinValue;
                    }

                    if (report.ReportFilterByMonthDate != DateTime.MinValue)
                    {
                        if (View.Id == "Reporting_ListView_PrintDownload")
                        {
                            if (report.Mode == "Enter")
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[ReportedDate] BETWEEN('" + report.ReportFilterByMonthDate + "', '" + DateTime.Now + "')");
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DatePrinted] BETWEEN('" + report.ReportFilterByMonthDate + "', '" + DateTime.Now + "')");
                            }
                        }
                        else if (View.Id == "Reporting_ListView_Delivery")
                        {
                            if (report.Mode == "Enter")
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DatePrinted] >= ? and [DatePrinted] < ?", report.ReportFilterByMonthDate, DateTime.Now);
                            }
                            else
                            {
                                //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDelivered >= ? And DateDelivered < ?", report.ReportFilterByMonthDate, DateTime.Now + "')");
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DateDelivered] >= ? and [DateDelivered] < ?", report.ReportFilterByMonthDate, DateTime.Now);
                            }
                        }
                        else if (View.Id == "Reporting_ListView_Archieve")
                        {
                            if (report.Mode == "Enter")
                            {
                                //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DateDelivered] BETWEEN('" + report.ReportFilterByMonthDate + "', '" + DateTime.Now + "')");
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DateDelivered] >= ? And [DateDelivered] <= ?", report.ReportFilterByMonthDate, DateTime.Now + "')");
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DateArchived] BETWEEN('" + report.ReportFilterByMonthDate + "', '" + DateTime.Now + "')");
                            }
                        }
                        else if (View.Id == "Reporting_ListView_Recall")
                        {
                            if (report.Mode == "Enter")
                            {
                                //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DateDelivered] BETWEEN('" + report.ReportFilterByMonthDate + "', '" + DateTime.Now + "')");
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DateDelivered] >= ? And [DateDelivered] <= ?", report.ReportFilterByMonthDate, DateTime.Now + "')");
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[DateRecalled] BETWEEN('" + report.ReportFilterByMonthDate + "', '" + DateTime.Now + "')");
                            }
                        }
                        else if (View.Id == "Reporting_ListView_Level1Review_View")
                        {
                            //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[ReportedDate] BETWEEN('" + report.ReportFilterByMonthDate + "', '" + DateTime.Now + "')");
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("ReportedDate>=? and ReportedDate<?", report.ReportFilterByMonthDate, DateTime.Now);
                        }
                        else if (View.Id == "Reporting_ListView_Level2Review_View")
                        {
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("ReportedDate>=? and ReportedDate<?", report.ReportFilterByMonthDate, DateTime.Now);
                            //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[ReportedDate] BETWEEN('" + report.ReportFilterByMonthDate + "', '" + DateTime.Now + "')");
                        }
                    }
                    else
                    {
                        if (((ListView)View).CollectionSource.Criteria.ContainsKey("dateFilter"))
                        {
                            ((ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                        }
                    }
                    ((ListView)View).Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void EditAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)View.CurrentObject;
                if (View.Id == "Reporting_ListView_Delivery" && report.Mode == "View")
                {
                    if (objreporting.DateArchived != null && objreporting.ArchivedBy != null)
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ReportIDNotEditable"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View.Id == "Reporting_DetailView_Delivery")
                {
                    Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)e.Object;
                    if (e.PropertyName == "ISEmail" && e.NewValue != e.OldValue)
                    {
                        ASPxStringPropertyEditor propertyEditor = ((DetailView)View).FindItem("EmailAddress") as ASPxStringPropertyEditor;
                        if (propertyEditor != null)
                        {
                            if (Convert.ToBoolean(e.NewValue))
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", true);
                            }
                            else
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                objreporting.EmailAddress = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RefreshAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void SaveAndCloseAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View != null && View.GetType() == typeof(Modules.BusinessObjects.SampleManagement.Reporting))
                {
                    Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)View.CurrentObject;
                    objreporting.LastUpdatedDate = DateTime.Now;
                    objreporting.LastUpdatedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    bool stat;
                    if (View.Id == "Reporting_DetailView_PrintDownload")
                    {
                        //stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                        //if (!stat)
                        //{
                        //    objreporting.DateDelivered = DateTime.Now;
                        //    objreporting.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        //    objreporting.ReportStatus = ReportStatus.PendingArchive;
                        //    stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                        //    if (!stat)
                        //    {
                        //        objreporting.DateArchived = DateTime.Now;
                        //        objreporting.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        //        objreporting.ReportStatus = ReportStatus.Archived;
                        //    }
                        //}

                        if (objDefaultInfo.boolReportdelivery == true)
                        {
                            objreporting.DatePrinted = DateTime.Now;
                            objreporting.PrintedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objreporting.ReportStatus = ReportStatus.PendingDelivery;
                            ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportPrintDownloadShow"]);
                            ////if (!stat)
                            ////{
                            ////    objreporting.DatePrinted = DateTime.Now;
                            ////    objreporting.PrintedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            ////    objreporting.ReportStatus = ReportStatus.PendingDelivery;
                            ////    stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                            ////    if (!stat)
                            ////    {
                            ////        objreporting.DateDelivered = DateTime.Now;
                            ////        objreporting.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            ////        objreporting.ReportStatus = ReportStatus.PendingArchive;
                            ////        stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                            ////        if (!stat)
                            ////        {
                            ////            objreporting.DateArchived = DateTime.Now;
                            ////            objreporting.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            ////            objreporting.ReportStatus = ReportStatus.Archived;
                            ////        }
                            ////    }
                            ////}
                            //AddReportSign(objreporting, IsReported: true, IsValidated: true, IsApproved: true);
                        }
                        else if (objDefaultInfo.boolReportdelivery == false && objDefaultInfo.boolReportArchive == true)
                        {
                            objreporting.DateDelivered = DateTime.Now;
                            objreporting.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objreporting.ReportStatus = ReportStatus.PendingArchive;
                            ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                            ////if (!stat)
                            ////{
                            ////    objreporting.DateDelivered = DateTime.Now;
                            ////    objreporting.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            ////    objreporting.ReportStatus = ReportStatus.PendingArchive;
                            ////    stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                            ////    if (!stat)
                            ////    {
                            ////        objreporting.DateArchived = DateTime.Now;
                            ////        objreporting.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            ////        objreporting.ReportStatus = ReportStatus.Archived;
                            ////    }
                            ////}
                            // AddReportSign(objreporting, IsReported: true, IsValidated: true, IsApproved: true);
                        }
                        else if (objDefaultInfo.boolReportdelivery == false && objDefaultInfo.boolReportArchive == false)
                        {
                            objreporting.DateDelivered = DateTime.Now;
                            objreporting.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objreporting.ReportStatus = ReportStatus.ReportDelivered;
                            ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                            ////if (!stat)
                            ////{
                            ////    objreporting.DateArchived = DateTime.Now;
                            ////    objreporting.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            ////    objreporting.ReportStatus = ReportStatus.Archived;
                            ////}
                            //  AddReportSign(objreporting, IsReported: true, IsValidated: true, IsApproved: true);
                        }

                    }
                    else if (View.Id == "Reporting_DetailView_Delivery")
                    {
                        //stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                        //if (!stat)
                        //{
                        //    objreporting.DateArchived = DateTime.Now;
                        //    objreporting.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        //    objreporting.ReportStatus = ReportStatus.Archived;
                        //}
                        if (objDefaultInfo.boolReportArchive == true)
                        {
                            objreporting.DateDelivered = DateTime.Now;
                            objreporting.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objreporting.ReportStatus = ReportStatus.PendingArchive;
                            ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                            ////if (!stat)
                            ////{
                            ////    objreporting.DateDelivered = DateTime.Now;
                            ////    objreporting.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            ////    objreporting.ReportStatus = ReportStatus.PendingArchive;
                            ////    stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                            ////    if (!stat)
                            ////    {
                            ////        objreporting.DateArchived = DateTime.Now;
                            ////        objreporting.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            ////        objreporting.ReportStatus = ReportStatus.Archived;
                            ////    }
                            ////}
                            // AddReportSign(objreporting, IsReported: true, IsValidated: true, IsApproved: true);
                        }
                        else
                        {
                            objreporting.DateDelivered = DateTime.Now;
                            objreporting.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objreporting.ReportStatus = ReportStatus.ReportDelivered;
                            ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                            ////if (!stat)
                            ////{
                            ////    objreporting.DateArchived = DateTime.Now;
                            ////    objreporting.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            ////    objreporting.ReportStatus = ReportStatus.Archived;
                            ////}
                            //  AddReportSign(objreporting, IsReported: true, IsValidated: true, IsApproved: true);
                        }
                    }
                    else if (View.Id == "Reporting_DetailView_Recalled")
                    {
                        foreach (SampleParameter sampleReport in objreporting.SampleParameter)
                        {
                            sampleReport.Status = Samplestatus.PendingReporting;
                            sampleReport.OSSync = true;
                        }
                    }
                    //Frame.GetController<ReportingWebController>().ResetNavigationCount();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAndCloseAction_ExecuteCompleted(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View.Id == "Reporting_ListView_Recall")
                {
                    //ShowNavigationItemController ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                    //foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                    //{
                    //    if (parent.Id == "Reporting")
                    //    {
                    //        foreach (ChoiceActionItem child in parent.Items)
                    //        {
                    //            if (child.Id == "Custom Reporting")
                    //            {
                    //                int count = 0;
                    //                IObjectSpace objSpaceReport = Application.CreateObjectSpace();
                    //                using (XPView lstview = new XPView(((XPObjectSpace)objSpaceReport).Session, typeof(SampleParameter)))
                    //                {
                    //                    lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingReporting' And Not IsNullOrEmpty([Samplelogin.JobID.JobID])");
                    //                    lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                    //                    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    //                    List<object> jobid = new List<object>();
                    //                    foreach (ViewRecord rec in lstview)
                    //                        jobid.Add(rec["Toid"]);
                    //                    count = jobid.Count;
                    //                }
                    //                var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //                if (count > 0)
                    //                {
                    //                    child.Caption = cap[0] + " (" + count + ")";
                    //                }
                    //                else
                    //                {
                    //                    child.Caption = cap[0];
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RuleSet_CustomNeedToValidateRule(object sender, DevExpress.Persistent.Validation.CustomNeedToValidateRuleEventArgs e)
        {
            try
            {
                if (View.Id == "Reporting_DetailView_PrintDownload" || View.Id == "Reporting_ListView_Delivery")
                {
                    string[] NoNeed = { "RecalledFrom", "ArchiveLocation", "CodeName", "#Box", "ReceivedBy", "DeliveryAddress" };
                    if (NoNeed.Contains(e.Rule.Id))
                    {
                        e.NeedToValidateRule = false;
                        e.Handled = !e.NeedToValidateRule;
                    }
                    if (View.Id == "Reporting_ListView_Delivery")
                    {
                        string[] NoNeeds = { "StoreLocation" };
                        if (NoNeeds.Contains(e.Rule.Id))
                        {
                            e.NeedToValidateRule = false;
                            e.Handled = !e.NeedToValidateRule;
                        }
                    }
                }
                else if (View.Id == "Reporting_DetailView_Delivery" || View.Id == "Reporting_ListView_Archieve")
                {
                    string[] NoNeed = { "RecalledFrom", "ArchiveLocation", "CodeName", "#Box", "StoreLocation" };
                    if (NoNeed.Contains(e.Rule.Id))
                    {
                        e.NeedToValidateRule = false;
                        e.Handled = !e.NeedToValidateRule;
                    }
                }
                else if (View.Id == "Reporting_DetailView_Archived" || View.Id == "Reporting_ListView_Recall")
                {
                    string[] NoNeed = { "RecalledFrom", "ReceivedBy", "DeliveryAddress", "StoreLocation" };
                    if (NoNeed.Contains(e.Rule.Id))
                    {
                        e.NeedToValidateRule = false;
                        e.Handled = !e.NeedToValidateRule;
                    }
                }
                else if (View.Id == "Reporting_DetailView_Recalled")
                {
                    string[] NoNeed = { "ArchiveLocation", "CodeName", "#Box", "ReceivedBy", "DeliveryAddress", "StoreLocation" };
                    if (NoNeed.Contains(e.Rule.Id))
                    {
                        e.NeedToValidateRule = false;
                        e.Handled = !e.NeedToValidateRule;
                    }
                }
                else if (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_PrintDownload")
                {
                    string[] NoNeed = { "RecalledFrom", "ArchiveLocation", "CodeName", "#Box", "ReceivedBy", "DeliveryAddress", "StoreLocation" };
                    if (NoNeed.Contains(e.Rule.Id))
                    {
                        e.NeedToValidateRule = false;
                        e.Handled = !e.NeedToValidateRule;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View.GetType().Name == "ListView" && View.Id != "Reporting_ListView_Delivery")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                }
                if (View.Id == "Reporting_ListView_PrintDownload" && report.Mode == "Enter")
                {
                    if (objLanguage.strcurlanguage == "En")
                    {
                        ((ListView)View).Caption = "Report Print & Download";
                    }
                    else
                    {
                        //((ListView)View).Caption = "报告打印下载 (进入模式)";
                        ((ListView)View).Caption = "报告打印下载";
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReportStatus] = 'PendingPrint'");
                }
                else if (View.Id == "Reporting_ListView_Delivery" && report.Mode == "Enter")
                {
                    if (objLanguage.strcurlanguage == "En")
                    {
                        ((ListView)View).Caption = "Report Delivery";
                    }
                    else
                    {
                        //((ListView)View).Caption = "报告发放 (进入模式)";
                        ((ListView)View).Caption = "报告发放";
                    }
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e){
                                sessionStorage.setItem('CurrFocusedColumn', e.focusedColumn.fieldName);
                              if(e.focusedColumn.fieldName == 'Email' ||e.focusedColumn.fieldName == 'Mail')
                                {
                                   var mail= s.batchEditApi.GetCellValue(e.visibleIndex,'Mail');
                                   var donotdeliver= s.batchEditApi.GetCellValue(e.visibleIndex,'DoNotDeliver');
                                   if(donotdeliver!=null && donotdeliver)
                                     {
                                          e.cancel = true;
                                     }
                                     else if(e.focusedColumn.fieldName == 'Email' && mail!=null && mail)
                                     {
                                          e.cancel = true;
                                     }
                                   
                                }
                 
                      }";
                        if (!report.IsCanRefresh)
                        {
                            gridListEditor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                            report.IsCanRefresh = true;
                        }
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("DeliveryReport", this);
                        //gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s, e) 
                        //{
                                                         
                        //}";
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            window.setTimeout(function() { 
                            var fieldName = sessionStorage.getItem('CurrFocusedColumn');
                            console.log(fieldName);
                            if(fieldName == 'Mail' )
                            {
                                 var value = s.batchEditApi.GetCellValue(e.visibleIndex, 'fieldName');
                                 s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                                                                RaiseXafCallback(globalCallbackControl, 'DeliveryReport',  Oidvalue +'|'+fieldName+'|' +  value, '', false); 
                                                                }); 
                            }
                            }, 20); }";
                    }
                }
                else if (View.Id == "Reporting_ListView_Archieve" && report.Mode == "Enter")
                {
                    if (objLanguage.strcurlanguage == "En")
                    {
                        ((ListView)View).Caption = "Report Archive";
                    }
                    else
                    {
                        //((ListView)View).Caption = "报告归档 (进入模式)";
                        ((ListView)View).Caption = "报告归档";
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReportStatus] = 'PendingArchive'");
                }
                else if (View.Id == "Reporting_ListView_Recall" && report.Mode == "Enter")
                {
                    if (objLanguage.strcurlanguage == "En")
                    {
                        ((ListView)View).Caption = "Report Recall";
                    }
                    else
                    {
                        //((ListView)View).Caption = "报告收回 (进入模式)";
                        ((ListView)View).Caption = "报告收回";
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReportStatus] = 'Archived'");
                }
                else if (View.Id == "Reporting_ListView_Account")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReportStatus] != 'Recalled'");
                }
                else if (View.Id == "Reporting_DetailView_PrintDownload" && report.Mode == "Enter")
                {
                    Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)View.CurrentObject;
                    objreporting.DatePrinted = DateTime.Now;
                    objreporting.PrintedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    objreporting.PrintCopies = 1;
                    objreporting.ReportStatus = ReportStatus.PendingDelivery;
                }
                else if (View.Id == "Reporting_DetailView_Delivery" && report.Mode == "Enter")
                {
                    Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)View.CurrentObject;
                    objreporting.DateDelivered = DateTime.Now;
                    objreporting.ISEmail = false;
                    objreporting.HandledBy = objreporting.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    objreporting.DeliveryCopies = 1;
                    objreporting.ReportStatus = ReportStatus.PendingArchive;
                    ASPxStringPropertyEditor propertyEditor = ((DetailView)View).FindItem("EmailAddress") as ASPxStringPropertyEditor;
                    if (propertyEditor != null)
                    {
                        propertyEditor.AllowEdit.SetItemValue("stat", false);
                    }
                }
                else if (View.Id == "Reporting_DetailView_Archived" && report.Mode == "Enter")
                {
                    Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)View.CurrentObject;
                    objreporting.DateArchived = DateTime.Now;
                    objreporting.ArchivedBy = objreporting.ArchivedReceivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    objreporting.ArchiveCopies = 1;
                    objreporting.ReportStatus = ReportStatus.Archived;
                }
                else if (View.Id == "Reporting_DetailView_Recalled" && report.Mode == "Enter")
                {
                    Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)View.CurrentObject;
                    objreporting.RecalledCopies = objreporting.DeliveryCopies;
                    objreporting.DateRecalled = DateTime.Now;
                    objreporting.RecalledBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    objreporting.ReportStatus = ReportStatus.Recalled;
                }
                else if (View.Id == "Reporting_ListView_Copy_ReportView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        ASPxGridView gridView = (ASPxGridView)gridListEditor.Grid;
                        if (gridView != null)
                        {
                            if (gridView.Columns["ReportID"] != null)
                                gridView.Columns["ReportID"].FixedStyle = GridViewColumnFixedStyle.Left;


                        }
                    }
                }
                else if (View.Id == "PDFPreview_DetailView")
                {
                    PDFPreview objpdf = (PDFPreview)View.CurrentObject;
                    bool printstat = false;
                    if (objpdf.ViewID == "Reporting_ListView_PrintDownload" || objpdf.ViewID == "Reporting_DetailView_PrintDownload")
                    {
                        printstat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportPrintDownloadPrint"]);
                    }
                    else if (objpdf.ViewID == "Reporting_ListView_Delivery" || objpdf.ViewID == "Reporting_DetailView_Delivery")
                    {
                        printstat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryPrint"]);
                    }
                    else if (objpdf.ViewID == "Reporting_ListView_Archieve" || objpdf.ViewID == "Reporting_DetailView_Archived")
                    {
                        printstat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchivePrint"]);
                    }
                    else if (objpdf.ViewID == "Reporting_ListView_Account")
                    {
                        printstat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportAccountPrint"]);
                    }
                    else if (objpdf.ViewID == "Reporting_ListView_Recall" || objpdf.ViewID == "Reporting_DetailView_Recalled")
                    {
                        printstat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportRecallPrint"]);
                    }
                    else if (objpdf.ViewID == "Reporting_ListView")
                    {
                        Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)Application.MainWindow.View.CurrentObject;
                        if (objreporting != null)
                        {
                            if ((objDefaultInfo.DefaultReportApprove != null && objDefaultInfo.DefaultReportApprove.Select == true) || (objDefaultInfo.DefaultReportPrintDownload != null && objDefaultInfo.DefaultReportPrintDownload.Select == true))
                            {
                                printstat = false;
                            }
                            // else if (objDefaultInfo.DefaultReportValidation.Select == true && objreporting.ReportValidatedDate != null && objreporting.ReportValidatedDate !=DateTime.MinValue)
                            else if (objDefaultInfo.DefaultReportValidation != null && objDefaultInfo.DefaultReportValidation.Select == true)
                            {
                                printstat = true;
                            }
                        }
                    }
                    else if (objpdf.ViewID == "Reporting_ListView_Copy_ReportApproval")
                    {
                        Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)Application.MainWindow.View.CurrentObject;
                        if (objreporting != null)
                        {
                            if (objDefaultInfo.DefaultReportPrintDownload != null && objDefaultInfo.DefaultReportPrintDownload.Select == true)
                            {
                                printstat = false;
                            }
                            //  else if (objDefaultInfo.DefaultReportValidation.Select == true && objreporting.ReportApprovedDate != null && objreporting.ReportValidatedDate != DateTime.MinValue)
                            else if (objDefaultInfo.DefaultReportValidation != null && objDefaultInfo.DefaultReportValidation.Select == true)
                            {
                                printstat = true;
                            }
                        }
                    }
                    else if (objpdf.ViewID == "Reporting_ListView_Copy_ReportView")
                    {
                        Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)Application.MainWindow.View.CurrentObject;
                        if (objreporting != null)
                        {
                            if (objDefaultInfo.DefaultReportApprove != null && objDefaultInfo.DefaultReportApprove.Select == true || (objDefaultInfo.DefaultReportPrintDownload != null && objDefaultInfo.DefaultReportPrintDownload.Select == true))
                            {
                                printstat = false;
                            }
                            else if (objDefaultInfo.DefaultReportValidation != null && objDefaultInfo.DefaultReportValidation.Select == true && objreporting.ReportValidatedDate != null && objreporting.ReportValidatedDate != DateTime.MinValue)
                            {
                                printstat = true;
                            }
                        }
                    }

                    else if (objpdf.ViewID == "Reporting_ListView_Level1Review_View" || objpdf.ViewID == "Reporting_ListView_Level2Review_View")
                    {
                        Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)Application.MainWindow.View.CurrentObject;
                        if (objreporting != null)
                        {
                            if (objreporting.ReportApprovedDate != null && objreporting.ReportApprovedDate != DateTime.MinValue)
                            {
                                printstat = true;
                            }
                        }
                    }
                    else if (objpdf.ViewID == "Invoicing_ListView_Review" || objpdf.ViewID == "Invoicing_DetailView_Queue" || objpdf.ViewID == "Invoicing_ListView_Delivery")
                    {
                        printstat = true;
                    }
                    ReportPrint.Enabled.SetItemValue("enb", printstat);
                }
                else if(View.Id== "Reporting_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                        gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                var selected = gridListEditor.GetSelectedObjects();
                if (View.Id== "Reporting_ListView_Delivery")
                {
                foreach (Modules.BusinessObjects.SampleManagement.Reporting objReport in ((ListView)View).CollectionSource.List)
                {
                    if (selected.Contains(objReport))
                    {
                        objReport.DateDelivered = DateTime.Now;
                        objReport.DeliveredBy = View.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);

                        //Samplecheckin objchkin = View.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid] = ?", objReport.Oid));
                        if (objReport != null && objReport.JobID.ClientName.Oid  != null && !objReport.Mail && !objReport.DoNotDeliver && (objReport.Email == null || objReport.Email.Trim().Length == 0))
                        {
                            IList<Contact> objconEmail = View.ObjectSpace.GetObjects<Contact>(CriteriaOperator.Parse("Not IsNullOrEmpty([Email]) And [Customer.Oid] = ? And ReportDelivery = true", objReport.JobID.ClientName.Oid));
                            if (objconEmail != null && objconEmail.Count > 0)
                            {
                                string lstmail = string.Empty;
                                foreach (Contact objContact in objconEmail)
                                {
                                    if (!string.IsNullOrEmpty(objContact.Email))
                                    {
                                        if (string.IsNullOrEmpty(lstmail))
                                        {
                                            lstmail = objContact.Email;
                                        }
                                        else if (!string.IsNullOrEmpty(lstmail))
                                        {
                                            lstmail = lstmail + "; " + objContact.Email;
                                        }
                                    }
                                }
                                objReport.Email = lstmail;
                            }
                        }
                    }
                    else
                    {
                        objReport.DateDelivered = null;
                        objReport.DeliveredBy = null;
                    }
                }
            }
                else if(View.Id== "Reporting_ListView")
                {
                    foreach (Modules.BusinessObjects.SampleManagement.Reporting obj in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(obj))
                        {
                            obj.ReportValidatedDate = DateTime.Now;
                            obj.ReportValidatedBy = View.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        else
                        {
                            obj.ReportValidatedDate = null;
                            obj.ReportValidatedBy = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id != "Invoicing_ListView_Review" && View.Id != "Invoicing_DetailView_Queue" && View.Id != "Invoicing_ListView_Delivery")
                {
                    e.Cell.Attributes.Add("onclick", "event.stopPropagation();");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule -= RuleSet_CustomNeedToValidateRule;
                if (View.GetType().Name == "ListView")
                {
                    Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;
                    ReportDateFilterAction.SelectedItemChanged -= ReportDateFilterAction_SelectedItemChanged;
                    if (View.Id == "Reporting_ListView_PrintDownload" || View.Id == "Reporting_ListView_Delivery")
                    {
                        ListViewController controller = Frame.GetController<ListViewController>();
                        if (controller.EditAction != null)
                        {
                            controller.EditAction.TargetObjectsCriteria = string.Empty;
                        }
                        report.IsCanRefresh = false;
                    }
                }
                else if (View.GetType().Name == "DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Active.SetItemValue("Enable", true);
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.ExecuteCompleted -= SaveAndCloseAction_ExecuteCompleted;

                    if (objLanguage.strcurlanguage == "En")
                    {
                        Frame.GetController<ModificationsController>().SaveAndCloseAction.Caption = "Save and Close";
                    }
                    else
                    {
                        Frame.GetController<ModificationsController>().SaveAndCloseAction.Caption = "保存并关闭";
                    }
                    Frame.GetController<RefreshController>().RefreshAction.Executing -= RefreshAction_Executing;
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ReportManagementView_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //IObjectSpace objSpace = Application.CreateObjectSpace();
                //Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                //SelectedData sproclang = currentSession.ExecuteSproc("getCurrentLanguage", "");
                //var CurrentLanguage = sproclang.ResultSet[1].Rows[0].Values[0].ToString();
                report.Mode = "View";
                ReportManagementView.Active.SetItemValue("Disable", false);
                ReportManagementDelete.Active.SetItemValue("Disable", true);
                if (View.Id == "Reporting_ListView_PrintDownload")
                {
                    if (objLanguage.strcurlanguage == "En")
                    {
                        ((ListView)View).Caption = "Report Print & Download";
                    }
                    else
                    {
                        ((ListView)View).Caption = "报告打印下载";
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReportStatus] IN ('PendingDelivery', 'PendingArchive', 'Archived')");
                    ListViewController controller = Frame.GetController<ListViewController>();
                    controller.EditAction.TargetObjectsCriteria = ReportPrintcriteriaString;
                }
                else if (View.Id == "Reporting_ListView_Delivery")
                {
                    ////if (CurrentLanguage == "En")
                    ////{
                    ////    ((ListView)View).Caption = "Report Delivery";
                    ////}
                    ////else
                    ////{
                    ////    ((ListView)View).Caption = "报告发放";
                    ////}
                    ////((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReportStatus] IN ('PendingArchive', 'Archived')");
                    ////ListViewController controller = Frame.GetController<ListViewController>();
                    ////controller.EditAction.TargetObjectsCriteria = ReportDeliveryCriteriaString;

                    //if (CurrentLanguage == "En")
                    //{
                    //    ((ListView)View).Caption = "Report Delivery";
                    //}
                    //else
                    //{
                    //    ((ListView)View).Caption = "报告发放";
                    //}
                    IObjectSpace os = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(os, typeof(Modules.BusinessObjects.SampleManagement.Reporting));
                    cs.Criteria["filter"] = CriteriaOperator.Parse("[ReportStatus] IN ('PendingArchive', 'Archived','ReportDelivered')");
                    ListView lvrrptdeliverid = Application.CreateListView("Reporting_ListView_Deliveired", cs, false);
                    Frame.SetView(lvrrptdeliverid);
                    //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReportStatus] IN ('PendingArchive', 'Archived')");
                    //ListViewController controller = Frame.GetController<ListViewController>();
                    //controller.EditAction.TargetObjectsCriteria = ReportDeliveryCriteriaString;
                }
                else if (View.Id == "Reporting_ListView_Archieve")
                {
                    if (objLanguage.strcurlanguage == "En")
                    {
                        ((ListView)View).Caption = "Report Archive";
                    }
                    else
                    {
                        ((ListView)View).Caption = "报告归档";
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReportStatus] = 'Archived'");
                }
                else if (View.Id == "Reporting_ListView_Recall")
                {
                    if (objLanguage.strcurlanguage == "En")
                    {
                        ((ListView)View).Caption = "Report Recall";
                    }
                    else
                    {
                        ((ListView)View).Caption = "报告收回";
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReportStatus] = 'Recalled'");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ReportManagementDelete_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)e.CurrentObject;
                if (e.SelectedObjects.Count == 1)
                {
                    if (View.Id == "Reporting_ListView_PrintDownload")
                    {
                        if (objreporting.ReportStatus == ReportStatus.PendingDelivery)
                        {
                            objreporting.ReportStatus = ReportStatus.PendingPrint;
                            objreporting.PrintCopies = 0;
                            objreporting.StoreLocation = null;
                            objreporting.DatePrinted = null;
                            objreporting.PrintedBy = null;
                            objreporting.PrintComments = null;
                            View.ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            View.ObjectSpace.Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ReportIDNotdeleted"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        if (View.Id == "Reporting_ListView_Delivery" && (objreporting.ReportStatus == ReportStatus.ReportDelivered || objreporting.ReportStatus == ReportStatus.Archived || objreporting.ReportStatus == ReportStatus.Recalled))
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ReportIDNotdeleted"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {
                            Popupos = Application.CreateObjectSpace();
                            object objToShow = Popupos.CreateObject(typeof(Modules.BusinessObjects.SampleManagement.Reporting));
                            DetailView CreatedDetailView = Application.CreateDetailView(Popupos, "Reporting_DetailView_DeleteReason", false, objToShow);
                            CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                            ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView.Caption = "Delete Reason";
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += Dc_Accepting;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectReportIDDelete"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                string strreason = ((Modules.BusinessObjects.SampleManagement.Reporting)e.AcceptActionArgs.CurrentObject).Reason;
                if (!string.IsNullOrEmpty(strreason))
                {
                    Popupos.RemoveFromModifiedObjects(e.AcceptActionArgs.CurrentObject);
                    //Popupos.Dispose();
                    Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)View.CurrentObject;
                    if (View.Id == "Reporting_ListView_Delivery")
                    {
                        objreporting.ReportStatus = ReportStatus.PendingDelivery;
                        objreporting.Tracking = null;
                        objreporting.DeliveryAddress = null;
                        objreporting.ReceivedBy = null;
                        objreporting.DeliveryCopies = 0;
                        objreporting.ISEmail = false;
                        objreporting.EmailAddress = null;
                        objreporting.DateDelivered = null;
                        objreporting.DeliveredBy = null;
                        objreporting.HandledBy = null;
                        objreporting.DeliveryComments = null;
                        objreporting.DeliveryDeleteReason = strreason;
                    }
                    else if (View.Id == "Reporting_ListView_Archieve")
                    {
                        objreporting.ReportStatus = ReportStatus.PendingArchive;
                        objreporting.ArchiveCopies = 0;
                        objreporting.Box = null;
                        objreporting.CodeName = null;
                        objreporting.ArchiveLocation = null;
                        objreporting.ArchiveComment = null;
                        objreporting.DateArchived = null;
                        objreporting.ArchivedBy = null;
                        objreporting.ArchivedReceivedBy = null;
                        objreporting.ArchiveDeleteReason = strreason;
                    }
                    View.ObjectSpace.CommitChanges();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    View.ObjectSpace.Refresh();
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteReason"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ReportManagementPreview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (e.SelectedObjects.Count == 1)
                {
                    Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)e.CurrentObject;
                    ShowReport(objreporting.ReportID);
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectReportID"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ShowReport(string ReportID)
        {
            try
            {
                //objRInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                //ReadXmlFile_FTPConc();
                //Ftp _FTP = GetFTPConnection();
                //string currentPath = _FTP.GetCurrentDirectory();
                string strRemotePath = ConfigurationManager.AppSettings["FinalReportPath"];
                strExportedPath = strRemotePath.Replace(@"\", "//") + ReportID + ".pdf";
                //IObjectSpace objSpace = Application.CreateObjectSpace();
                //Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                //SelectedData sproclang = currentSession.ExecuteSproc("getCurrentLanguage", "");
                //var CurrentLanguage = sproclang.ResultSet[1].Rows[0].Values[0].ToString();
                string WatermarkText;
                if (objLanguage.strcurlanguage == "En")
                {
                    WatermarkText = "UnApproved";
                }
                else
                {
                    WatermarkText = ConfigurationManager.AppSettings["ReportWaterMarkText"];
                }
                string Originalpath = string.Empty;
                //if (currentPath != "/")
                //{
                //    Originalpath = currentPath + strExportedPath;
                //}
                //else
                //{
                //    Originalpath = strExportedPath;
                //}
                if (ReportID != null)
                {

                    FileLinkSepDBController objfilelink = Frame.GetController<FileLinkSepDBController>();
                    if (objfilelink != null)
                    {
                        DataTable dt = objfilelink.GetFileLink(ReportID);
                        // List<string> lstreport = new List<string>();
                        //lstreport.Add(Convert.ToString(dt.Rows[0]["FileContent"]));
                        if (dt != null && dt.Rows.Count > 0 && dt.Rows[0] != null && dt.Rows[0]["FileContent"].GetType() == typeof(byte[]))
                        {
                        byte[] objbyte = (byte[])dt.Rows[0]["FileContent"];
                        //objbyte.ToArray();
                        //MemoryStream memory = new MemoryStream();
                        MemoryStream ms = new MemoryStream(objbyte);
                        //byte[] readBytes = ms.ToArray();
                        ////memory.CopyTo(ms);
                        ////XtraReport xtra = new XtraReport();
                        ////ms.Write(objbyte, 0, objbyte.Length);
                        ////xtra.ExportToPdf(ms);
                        //string strreport = dt.Rows[0]["ReportName"].ToString();
                        //BinaryFormatter formatter = new BinaryFormatter();
                        //ms.Position = 0;
                        //ms.ToArray();
                        //using (PdfReader reader = new PdfReader(ms))
                        //{
                        //byte[] bytear = ms.ToArray();
                        //ms.Seek(0, SeekOrigin.Begin);
                        //using (FileStream fileStream = new FileStream(m, FileMode.Create))
                        //{
                        //    ms.CopyTo(fileStream);
                        //}
                        //formatter.Serialize(ms,strreport);
                        // StreamReader reader = new StreamReader(ms);
                        //_FTP.TransferType = FtpTransferType.Binary;
                        //_FTP.GetFile(Originalpath, ms);

                        NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                        PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                        //if (!View.Id.Contains("PrintDownload"))
                        //{
                        //    Modules.BusinessObjects.SampleManagement.Reporting objReportPreview = (Modules.BusinessObjects.SampleManagement.Reporting)Application.MainWindow.View.CurrentObject;
                        //    if (objReportPreview != null && objReportPreview.DatePrinted == null && (objReportPreview.ReportApprovedDate == DateTime.MinValue || objReportPreview.ReportApprovedDate == null))
                        //    {
                        //        using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
                        //        {
                        //            string fontName = "Microsoft Yahei";
                        //            int fontSize = 25;
                        //            PdfStringFormat stringFormat = PdfStringFormat.GenericTypographic;
                        //            stringFormat.Alignment = PdfStringAlignment.Center;
                        //            stringFormat.LineAlignment = PdfStringAlignment.Center;

                        //            documentProcessor.LoadDocument(ms);
                        //            using (SolidBrush brush = new SolidBrush(Color.FromArgb(63, Color.Black)))
                        //            {
                        //                using (Font font = new Font(fontName, fontSize))
                        //                {
                        //                    foreach (var page in documentProcessor.Document.Pages)
                        //                    {
                        //                        var watermarkSize = page.CropBox.Width * 0.75;
                        //                        using (PdfGraphics graphics = documentProcessor.CreateGraphics())
                        //                        {
                        //                            SizeF stringSize = graphics.MeasureString(WatermarkText, font);
                        //                            Single scale = Convert.ToSingle(watermarkSize / stringSize.Width);
                        //                            graphics.TranslateTransform(Convert.ToSingle(page.CropBox.Width * 0.5), Convert.ToSingle(page.CropBox.Height * 0.5));
                        //                            graphics.RotateTransform(-45);
                        //                            graphics.TranslateTransform(Convert.ToSingle(-stringSize.Width * scale * 0.5), Convert.ToSingle(-stringSize.Height * scale * 0.5));
                        //                            using (Font actualFont = new Font(fontName, fontSize * scale))
                        //                            
                        //                                RectangleF rect = new RectangleF(0, 0, stringSize.Width * scale, stringSize.Height * scale);
                        //                                graphics.DrawString(WatermarkText, actualFont, brush, rect, stringFormat);
                        //                            }
                        //                            graphics.AddToPageForeground(page, 72, 72);
                        //                        }

                        //                    }
                        //                }
                        //            }

                        //            ms = new MemoryStream();
                        //            documentProcessor.SaveDocument(ms);
                        //        }
                        //    }
                        //}

                        objToShow.ReportID = ReportID;
                        ObjReportingInfo.strReportID = ReportID;
                        //objToShow.PDFData = ms.ToArray();
                        objToShow.PDFData = objbyte;
                        objToShow.ViewID = View.Id;
                        DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                        CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        //showViewParameters.CreatedView.Caption = "PDFViewer";
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.AcceptAction.Active.SetItemValue("disable", false);
                        dc.CancelAction.Active.SetItemValue("disable", false);
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ReportNotFound"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    //}
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ReportNotFound"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //public void ReadXmlFile_FTPConc()
        //{
        //    try
        //    {
        //        string[] FTPconnectionstring = objRInfo.WebConfigFTPConn.Split(';');
        //        strFTPServerName = FTPconnectionstring[0].Split('=').GetValue(1).ToString();
        //        strFTPUserName = FTPconnectionstring[1].Split('=').GetValue(1).ToString();
        //        strFTPPassword = FTPconnectionstring[2].Split('=').GetValue(1).ToString();
        //        strFTPPath = FTPconnectionstring[3].Split('=').GetValue(1).ToString();
        //        FTPPort = Convert.ToInt32(FTPconnectionstring[4].ToString().Split('=').GetValue(1).ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //public Rebex.Net.Ftp GetFTPConnection()
        //{
        //    try
        //    {
        //        Rebex.Net.Ftp FTP = new Rebex.Net.Ftp();
        //        FTP.TransferType = FtpTransferType.Binary;
        //        if ((!(strFTPServerName == null)
        //                    && ((strFTPServerName.Length > 0)
        //                    && (!(strFTPUserName == null)
        //                    && (strFTPUserName.Length > 0)))))
        //        {
        //            try
        //            {
        //                FTP.Timeout = 3000;
        //                FTP.Connect(strFTPServerName, FTPPort);
        //                FTP.Login(strFTPUserName, strFTPPassword);
        //                strFTPStatus = true;
        //            }
        //            catch (Exception ex)
        //            {
        //                strFTPStatus = false;
        //                return new Rebex.Net.Ftp();
        //            }
        //        }
        //        else
        //        {
        //            strFTPStatus = false;
        //            return new Rebex.Net.Ftp();
        //        }
        //        return FTP;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        private void ReportPrint_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                PDFPreview objpdf = (PDFPreview)View.CurrentObject;
                if (objpdf != null)
                {
                    HttpContext.Current.Response.ContentType = "Application/pdf";
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + objpdf.ReportID + ".pdf");
                    HttpContext.Current.Response.BinaryWrite(objpdf.PDFData);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ReportDeliverySave_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    //if (View.SelectedObjects.Count == 1)
                    //{
                        IList<Modules.BusinessObjects.SampleManagement.Reporting> lstReportDelivery = View.SelectedObjects.Cast<Modules.BusinessObjects.SampleManagement.Reporting>().ToList();
                        Employee objDefaultEmp = View.ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("ReportDeliveryDefault = True"));
                       if (lstReportDelivery.FirstOrDefault(i =>(!i.Mail&&!i.DoNotDeliver) && i.Email == null) != null)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "EmailAddress"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                     IObjectSpace objSpace = Application.CreateObjectSpace();
                        foreach (Modules.BusinessObjects.SampleManagement.Reporting obj in View.SelectedObjects)
                        {
                        if (!obj.DoNotDeliver && !obj.Mail)
                        {
                            SmtpClient sc = new SmtpClient();
                            Employee currentUser = SecuritySystem.CurrentUser as Employee;
                            string strlogUsername = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).UserName;
                            string strlogpassword = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).Password;
                            string strSmtpHost = "Smtp.gmail.com";
                            //string strMailFromUserName = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromUserName"];
                            //string strMailFromPassword = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromPassword"];
                            //string strMailFromUserName = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromUserName"];
                            string strMailFromUserName = currentUser.Email;
                            string strMailFromPassword = currentUser.Password;
                            string strMailto = string.Empty;
                            string strJobID = string.Empty;
                            string reportID = obj.ReportID;
                            //string FilePath = objFTP.GetDocument(reportID);
                            string strBody;
                            MailMessage message = new MailMessage();
                            message.IsBodyHtml = true;
                            eNotificationContentTemplate objEnct = objSpace.FindObject<eNotificationContentTemplate>(CriteriaOperator.Parse("[Reporting] =? ", obj.Oid));
                        int intPort = Convert.ToInt32(((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailPort"]); ;
                        bool EnableSSL = true;
                        if (objDefaultEmp != null && objDefaultEmp.Email != null)
                        {
                            strMailFromUserName = objDefaultEmp.Email;
                            strMailFromPassword = objDefaultEmp.Password;
                            strSmtpHost = objDefaultEmp.MailServerName;
                            intPort = objDefaultEmp.Port;
                            EnableSSL = objDefaultEmp.EnableSSL;

                        }
                        else
                        {
                            strMailFromUserName = currentUser.Email;
                            strMailFromPassword = currentUser.Password;
                            strSmtpHost = currentUser.MailServerName;
                            intPort = currentUser.Port;
                            EnableSSL = currentUser.EnableSSL;
                        }
                        message.From = new MailAddress(strMailFromUserName);
                        if (objEnct == null)
                            {
                                CriteriaOperator cs = CriteriaOperator.Parse("[SampleCheckin] Is  Null");
                                eNotificationContentTemplate objent = ObjectSpace.FindObject<eNotificationContentTemplate>(cs);
                                if (objent == null)
                                {
                                    objent = objSpace.CreateObject<eNotificationContentTemplate>();
                                    objent.Body = "The Attached PDF Report is for Samples received on @RECEIVEDDATE submitted with Chain of Custody @JOBID processed by the Red River Authority of Texas Environmental Services Laboratory in Wichita Falls, Texas.";
                                    //objent.Body = "Report Delivery";
                                }
                                //objent.Body=
                                strBody = "The Attached PDF Report is for Samples received on @RECEIVEDDATE submitted with Chain of Custody @JOBID processed by the Red River Authority of Texas Environmental Services Laboratory in Wichita Falls, Texas.";
                                if (strBody.ToUpper().Contains("@JOBID") && obj.JobID != null)
                                {
                                    strBody = strBody.Replace("@JOBID", obj.JobID.JobID);
                                }
                                if (strBody.ToUpper().Contains("@TAT") && obj.JobID != null && obj.JobID.TAT != null)
                                {
                                    strBody = strBody.Replace("@TAT", obj.JobID.TAT.TAT);
                                }
                                if (strBody.ToUpper().Contains("@PROJECTID") && obj.JobID != null && obj.JobID.ProjectID != null)
                                {
                                    strBody = strBody.Replace("@PROJECTID", obj.JobID.ProjectID.ProjectId);
                                }
                                if (strBody.ToUpper().Contains("@PROJECTNAME") && obj.JobID != null && obj.JobID.ProjectName != null)
                                {
                                    strBody = strBody.Replace("@PROJECTNAME", obj.JobID.ProjectName);
                                }
                                if (strBody.ToUpper().Contains("@RECEIVEDDATE") && obj.JobID != null && obj.JobID.RecievedDate != null)
                                {
                                    strBody = strBody.Replace("@RECEIVEDDATE", obj.JobID.RecievedDate.ToShortDateString());
                                }
                                objEnct = objSpace.CreateObject<eNotificationContentTemplate>();
                                objEnct.Body = strBody;
                                objEnct.Subject = objent.Subject;
                                objEnct.SampleCheckin = objSpace.GetObject(obj.JobID);
                                message.Subject = objEnct.Subject;
                                message.Body = objEnct.Body;
                            }
                            else
                            {
                                message.Subject = objEnct.Subject;
                                strBody = objEnct.Body;
                                message.Body = objEnct.Body;
                            }
                            //Modules.BusinessObjects.SampleManagement.Reporting objrept = (Modules.BusinessObjects.SampleManagement.Reporting)View.CurrentObject;
                            if (obj != null && !string.IsNullOrEmpty(obj.Email))
                            {
                                string[] strrptemailarr = obj.Email.Split(';');
                                foreach (string stremail in strrptemailarr)
                                {
                                    message.To.Add(stremail);
                                }
                            }
                            Contact con = objSpace.GetObject<Contact>(obj.JobID.ContactName);
                            if (con != null && !string.IsNullOrEmpty(con.Email))
                            {
                                message.To.Add(con.Email);
                            }
                            if (message.To.Count == 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "EmailAddress"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }

                            ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                            //ReadXmlFile_FTPConc();
                            //Rebex.Net.Ftp _FTP = GetFTPConnection();
                            //string strRemotePath = ConfigurationManager.AppSettings["FinalReportPath"];
                            //string strExportedPath = strRemotePath.Replace(@"\", "//") + reportID + ".pdf";
                            FileLinkSepDBController objfilelink = Frame.GetController<FileLinkSepDBController>();
                            if (objfilelink != null)
                            
                                //if (_FTP.FileExists(strExportedPath))
                            {
                                DataTable dt = objfilelink.GetFileLink(obj.ReportID);

                                // List<string> lstreport = new List<string>();
                                //lstreport.Add(Convert.ToString(dt.Rows[0]["FileContent"]));
                                byte[] objbyte = (byte[])dt.Rows[0]["FileContent"];
                                MemoryStream ms = new MemoryStream(objbyte);
                                //_FTP.TransferType = FtpTransferType.Binary;
                                //_FTP.GetFile(strExportedPath, ms);
                                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain);

                                MemoryStream pdfstream = new MemoryStream(ms.ToArray());
                                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(pdfstream, reportID + ".pdf");
                                //attachment.ContentDisposition.FileName = reportID + ".pdf";
                                message.Attachments.Add(attachment);


                            }
                            //sc.EnableSsl = true;
                            NetworkCredential credential = new NetworkCredential();
                            credential.UserName = strMailFromUserName;
                            credential.Password = strMailFromPassword;
                            sc.Credentials = credential;
                            sc.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                            sc.DeliveryFormat = SmtpDeliveryFormat.SevenBit;
                            sc.Host = strSmtpHost;
                        sc.Port = intPort;//Convert.ToInt32(((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailPort"]);

                        sc.EnableSsl = EnableSSL;//true;
                            try
                            {
                                if (message.To != null && message.To.Count > 0)
                                {
                                    sc.Send(message);
                                    if (View.Id == "Reporting_ListView_Delivery")
                                    {
                                        DefaultSetting objDefaultArchive = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportArchive'"));
                                        if (objDefaultArchive != null)
                                        {
                                            obj.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                            obj.DateDelivered = DateTime.Now;
                                            obj.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                            obj.ReportStatus = ReportStatus.PendingArchive;
                                        }
                                        else
                                        {
                                            obj.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                            obj.DateDelivered = DateTime.Now;
                                            obj.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                            obj.DateArchived = DateTime.Now;
                                            obj.ReportStatus = ReportStatus.ReportDelivered;
                                        }
                                    }
                                    //IList<Modules.BusinessObjects.SampleManagement.Reporting> reportings = View.ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>().Where(i => i.JobID != null && i.JobID.JobID == objSamplecheckin.JobID).ToList();
                                    //IList<SampleParameter> lstSamples = View.ObjectSpace.GetObjects<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                    //if (lstSamples.Where(i => i.Status != Samplestatus.Reported).Count() == 0 && reportings.Where(i => i.ReportStatus != ReportStatus.ReportDelivered).Count() == 0)
                                    //{
                                    //    StatusDefinition objStatus = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Index] = '125'"));
                                    //    if (objStatus != null)
                                    //    {
                                    //        objSamplecheckin.Index = objStatus;
                                    //        objSamplecheckin.ReportStatus = ReportStatus.ReportDelivered;
                                    //    }
                                    //    ObjectSpace.CommitChanges();
                                    //}
                                }
                            }
                            catch (SmtpFailedRecipientsException ex)
                            {
                                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                                {
                                    SmtpStatusCode exstatus = ex.InnerExceptions[i].StatusCode;
                                    if (exstatus == SmtpStatusCode.GeneralFailure || exstatus == SmtpStatusCode.ServiceNotAvailable || exstatus == SmtpStatusCode.SyntaxError || exstatus == SmtpStatusCode.SystemStatus || exstatus == SmtpStatusCode.TransactionFailed)
                                    {
                                        Application.ShowViewStrategy.ShowMessage(ex.Message);
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage(ex.InnerExceptions[i].FailedRecipient);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (View.Id == "Reporting_ListView_Delivery")
                            {
                                DefaultSetting objDefaultArchive = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportArchive'"));
                                if (objDefaultArchive != null)
                                {
                                    obj.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    obj.DateDelivered = DateTime.Now;
                                    obj.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    obj.ReportStatus = ReportStatus.PendingArchive;
                                }
                                else
                                {
                                    obj.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    obj.DateDelivered = DateTime.Now;
                                    obj.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    obj.DateArchived = DateTime.Now;
                                    obj.ReportStatus = ReportStatus.ReportDelivered;
                                }
                            }
                        }
                            //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "mailsendsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                           
                         
                            #region suboutStatus
                            //if (obj.SampleParameter.ToList().Where(i => i.SubOut == true) != null)
                            //{
                            //    foreach (SubOutSampleRegistrations objSubout in obj.SampleParameter.ToList().Where(i => i.SubOut == true).Select(i => i.SuboutSample).Distinct().ToList())
                            //    {
                            //        if (objSubout.SampleParameter.ToList().FirstOrDefault(i => i.Status == Samplestatus.PendingEntry || i.Status == Samplestatus.SuboutPendingValidation || i.Status == Samplestatus.SuboutPendingApproval || i.Status == Samplestatus.PendingReporting) == null)
                            //        {
                            //            if (objSubout.SubOutQcSample.ToList().FirstOrDefault(i => i.Status == Samplestatus.PendingEntry || i.Status == Samplestatus.SuboutPendingValidation || i.Status == Samplestatus.SuboutPendingApproval) == null)
                            //            {
                            //                SubOutSampleRegistrations objs = View.ObjectSpace.FindObject<SubOutSampleRegistrations>(CriteriaOperator.Parse("[Oid]=?", objSubout.Oid));
                            //                if (objs != null)
                            //                {
                            //                    objs.SuboutStatus = SuboutTrackingStatus.ReportDelivered;
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                            #endregion
                            //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "mailsendsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            objSpace.CommitChanges();
                            View.ObjectSpace.CommitChanges();
                            ObjectSpace.CommitChanges();
                            Samplecheckin objSamplecheckin = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?", obj.JobID.Oid));
                            if (objSamplecheckin != null)
                            {
                                IList<SampleParameter> lstsmples = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Testparameter.QCType.QCTypeName] = 'Sample'", objSamplecheckin.Oid));

                            foreach (SampleParameter sp in lstsmples)
                            {
                                sp.OSSync = true;
                            }
                                List<Modules.BusinessObjects.SampleManagement.Reporting> lstReportSamples = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[JobID.Oid]=? And [DeliveredBy] Is Not Null", objSamplecheckin.Oid)).ToList();
                                int deliveredCount = 0;
                                foreach (Modules.BusinessObjects.SampleManagement.Reporting objReport in lstReportSamples)
                                {
                                    deliveredCount = deliveredCount + objReport.SampleParameter.Count();
                                }
                                if (deliveredCount == lstsmples.Count())
                                {
                                    objSamplecheckin.ReportStatus = ReportStatus.ReportDelivered;
                                    if (lstsmples.Where(i=>i.InvoiceIsDone).Count()== lstsmples.Count())
                                    {
                                        IList<Invoicing> lstInvoice = ObjectSpace.GetObjects<Invoicing>(CriteriaOperator.Parse("Contains([JobID], ?)", objSamplecheckin.JobID.Replace(" ", "")));
                                        if(lstInvoice.FirstOrDefault(i=>i.ReviewedBy==null)!=null)
                                        {
                                            StatusDefinition objStatus = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 26"));
                                            if (objStatus != null)
                                            {
                                                objSamplecheckin.Index = objStatus;
                                                ObjectSpace.CommitChanges();
                                            }
                                        }
                                        else if(lstInvoice.FirstOrDefault(i => i.SentBy == null)!=null)
                                        {
                                            StatusDefinition objStatus = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 27"));
                                            if (objStatus != null)
                                            {
                                                objSamplecheckin.Index = objStatus;
                                                ObjectSpace.CommitChanges();
                                            }
                                        }
                                        else
                                        {
                                            StatusDefinition objStatus = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 28"));
                                            if (objStatus != null)
                                            {
                                                objSamplecheckin.Index = objStatus;
                                                ObjectSpace.CommitChanges();
                                            }
                                        }
                                       
                                    }
                                    else
                                    {
                                    StatusDefinition objStatus = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 25"));
                                    if (objStatus != null)
                                    {
                                        objSamplecheckin.Index = objStatus;
                                        ObjectSpace.CommitChanges();
                                        }
                                    }
                                }

                            }
                            
                            
                            //Frame.GetController<ReportingWebController>().ResetNavigationCount();
                            //SendNotificationEmail(obj);
                        }
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeliverySuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    View.ObjectSpace.Refresh();
                    //}
                    //else
                    //{
                    //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    //}
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DeliveryReportPreview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.SampleManagement.Reporting objreporting = (Modules.BusinessObjects.SampleManagement.Reporting)e.CurrentObject;
                ShowReport(objreporting.ReportID);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SendNotificationEmail(Modules.BusinessObjects.SampleManagement.Reporting DeliveryReport)
        {
            try
            {
                //SmtpClient sc = new SmtpClient();
                //string strlogUsername = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).UserName;
                //string strlogpassword = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).Password;
                //string strSmtpHost = "Smtp.gmail.com";
                //string strMailFromUserName = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromUserName"];
                //string strMailFromPassword = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromPassword"];
                ////string strWebSiteAddress = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["WebSiteAddress"];
                //string strMailto = string.Empty;

                //string strReportIDT = DeliveryReport.ReportID;
                //string strProjectID = string.Empty;
                //string strJobID = string.Empty;
                //string strRollbackReason = DeliveryReport.RollbackReason;
                //string strDeliveredBy = DeliveryReport.DeliveredBy.FullName;
                //string strDateDelivered = DeliveryReport.DateDelivered.ToString();

                //if (DeliveryReport.JobID != null)
                //{
                //    strJobID = DeliveryReport.JobID.JobID;

                //    if (DeliveryReport.JobID.ProjectID != null)
                //    {
                //        strProjectID = DeliveryReport.JobID.ProjectID.ProjectId;
                //    }
                //}

                //MailMessage m = new MailMessage();
                //m.IsBodyHtml = true;
                //m.From = new MailAddress(strMailFromUserName);
                //m.To.Add(DeliveryReport.Email);

                //m.Subject = "ReportID : " + strReportIDT + " - Report Delivery Notification";
                //m.Body = @"ReportID : " + strReportIDT + " has been Delivered.<br><br>"
                //                + "<table border=1 cellpadding=4 cellspacing=0>" +
                //                "<tr><td>ReportID:</td>" +
                //                "<td>" + strReportIDT + "</td></tr>" +
                //                "<tr><td>Project ID:</td>" +
                //                "<td>" + strProjectID + "</td></tr>" +
                //                "<tr><td>Job ID:</td>" +
                //                "<td>" + strJobID + "</td></tr>" +
                //                "<tr><td>Rollback Reason:</td>" +
                //                "<td>" + strRollbackReason + "</td></tr>" +
                //                "<tr><td>Rollback By:</td>" +
                //                "<td>" + strDeliveredBy + "</td></tr>" +
                //                "<tr><td>Date Rollback:</td>" +
                //                "<td>" + strDateDelivered + "</td></tr>" +
                //                "</table>";

                //sc.EnableSsl = true;
                //NetworkCredential credential = new NetworkCredential();
                //credential.UserName = strMailFromUserName;
                //credential.Password = strMailFromPassword;
                //sc.Credentials = credential;
                //sc.Host = strSmtpHost;
                //string strMailport = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailPort"];
                //if(!string.IsNullOrEmpty(strMailport))
                //{
                //    sc.Port = Convert.ToInt32(strMailport);
                //}
                //else
                //{
                //    sc.Port = 587;
                //}
                ////sc.Port = 25;
                ////sc.Port = 587;

                //try
                //{
                //    if (m.To != null && m.To.Count > 0)
                //    {
                //        sc.Send(m);
                //    }
                //}
                //catch (SmtpFailedRecipientsException ex)
                //{
                //    for (int i = 0; i < ex.InnerExceptions.Length; i++)
                //    {
                //        SmtpStatusCode exstatus = ex.InnerExceptions[i].StatusCode;
                //        if (exstatus == SmtpStatusCode.GeneralFailure || exstatus == SmtpStatusCode.ServiceNotAvailable || exstatus == SmtpStatusCode.SyntaxError || exstatus == SmtpStatusCode.SystemStatus || exstatus == SmtpStatusCode.TransactionFailed)
                //        {
                //            Application.ShowViewStrategy.ShowMessage(ex.Message);
                //        }
                //        else
                //        {
                //            Application.ShowViewStrategy.ShowMessage(ex.InnerExceptions[i].FailedRecipient);
                //        }
                //    }
                //}
                IObjectSpace objSpace = Application.CreateObjectSpace();
                SmtpClient sc = new SmtpClient();
                Employee currentUser = SecuritySystem.CurrentUser as Employee;
                string strlogUsername = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).UserName;
                string strlogpassword = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).Password;
                string strSmtpHost = "Smtp.gmail.com";
                //string strMailFromUserName = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromUserName"];
                //string strMailFromPassword = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromPassword"];
                string strMailFromUserName = currentUser.Email;
                string strMailFromPassword = currentUser.Password;
                //string strWebSiteAddress = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["WebSiteAddress"];
                string strMailto = string.Empty;
                string strJobID = string.Empty;
                string reportID = DeliveryReport.ReportID;
                //string FilePath = objFTP.GetDocument(reportID);
                string strBody;
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(strMailFromUserName);
                eNotificationContentTemplate objEnct = objSpace.FindObject<eNotificationContentTemplate>(CriteriaOperator.Parse("[Reporting] =? ", DeliveryReport.Oid));
                if (objEnct == null)
                {
                    CriteriaOperator cs = CriteriaOperator.Parse("[SampleCheckin] Is  Null");
                    eNotificationContentTemplate objent = ObjectSpace.FindObject<eNotificationContentTemplate>(cs);
                    if (objent == null)
                    {
                        objent = objSpace.CreateObject<eNotificationContentTemplate>();
                        objent.Body = "Report Delivery";
                    }
                    strBody = objent.Body;
                    if (strBody.ToUpper().Contains("@JOBID") && DeliveryReport.JobID != null)
                    {
                        strBody = strBody.Replace("@JobID", DeliveryReport.JobID.JobID);
                    }
                    if (strBody.ToUpper().Contains("@TAT") && DeliveryReport.JobID != null && DeliveryReport.JobID.TAT != null)
                    {
                        strBody = strBody.Replace("@TAT", DeliveryReport.JobID.TAT.TAT);
                    }
                    if (strBody.ToUpper().Contains("@PROJECTID") && DeliveryReport.JobID != null && DeliveryReport.JobID.ProjectID != null)
                    {
                        strBody = strBody.Replace("@ProjectID", DeliveryReport.JobID.ProjectID.ProjectId);
                    }
                    if (strBody.ToUpper().Contains("@PROJECTNAME") && DeliveryReport.JobID != null && DeliveryReport.JobID.ProjectName != null)
                    {
                        strBody = strBody.Replace("@ProjectName", DeliveryReport.JobID.ProjectName);
                    }
                    if (strBody.ToUpper().Contains("@RECEIVEDDATE") && DeliveryReport.JobID != null && DeliveryReport.JobID.RecievedDate != null)
                    {
                        strBody = strBody.Replace("@ReceivedDate", DeliveryReport.JobID.RecievedDate.ToString());
                    }
                    objEnct = objSpace.CreateObject<eNotificationContentTemplate>();
                    objEnct.Body = strBody;
                    objEnct.Subject = objent.Subject;
                    objEnct.SampleCheckin = objSpace.GetObject(DeliveryReport.JobID);
                    message.Subject = objEnct.Subject;
                    message.Body = objEnct.Body;
                }
                else
                {
                    message.Subject = objEnct.Subject;
                    strBody = objEnct.Body;
                    message.Body = objEnct.Body;
                }
                //Modules.BusinessObjects.SampleManagement.Reporting objrept = (Modules.BusinessObjects.SampleManagement.Reporting)View.CurrentObject;
                if (DeliveryReport != null && !string.IsNullOrEmpty(DeliveryReport.Email))
                {
                    string[] strrptemailarr = DeliveryReport.Email.Split(';');
                    foreach (string stremail in strrptemailarr)
                    {
                        message.To.Add(stremail);
                    }
                }
                Contact con = objSpace.GetObject<Contact>(DeliveryReport.JobID.ContactName);
                if (con != null && !string.IsNullOrEmpty(con.Email))
                {
                    message.To.Add(con.Email);
                }
                if (message.To.Count == 0)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "EmailAddress"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    return;
                }

                ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                //ReadXmlFile_FTPConc();
                //Rebex.Net.Ftp _FTP = GetFTPConnection();
                string strRemotePath = ConfigurationManager.AppSettings["FinalReportPath"];
                string strExportedPath = strRemotePath.Replace(@"\", "//") + reportID + ".pdf";
                //if (_FTP.FileExists(strExportedPath))
                {
                    MemoryStream ms = new MemoryStream();
                    //_FTP.TransferType = FtpTransferType.Binary;
                    //_FTP.GetFile(strExportedPath, ms);
                    System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain);

                    MemoryStream pdfstream = new MemoryStream(ms.ToArray());
                    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(pdfstream, reportID + ".pdf");
                    //attachment.ContentDisposition.FileName = reportID + ".pdf";
                    message.Attachments.Add(attachment);


                }
                //sc.EnableSsl = true;
                NetworkCredential credential = new NetworkCredential();
                credential.UserName = strMailFromUserName;
                credential.Password = strMailFromPassword;
                sc.Credentials = credential;
                sc.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                sc.DeliveryFormat = SmtpDeliveryFormat.SevenBit;
                sc.Host = strSmtpHost;
                sc.Port = Convert.ToInt32(((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailPort"]);

                sc.EnableSsl = true;
                try
                {
                    if (message.To != null && message.To.Count > 0)
                    {
                        sc.Send(message);
                    }
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    for (int i = 0; i < ex.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode exstatus = ex.InnerExceptions[i].StatusCode;
                        if (exstatus == SmtpStatusCode.GeneralFailure || exstatus == SmtpStatusCode.ServiceNotAvailable || exstatus == SmtpStatusCode.SyntaxError || exstatus == SmtpStatusCode.SystemStatus || exstatus == SmtpStatusCode.TransactionFailed)
                        {
                            Application.ShowViewStrategy.ShowMessage(ex.Message);
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(ex.InnerExceptions[i].FailedRecipient);
                        }
                    }
                }
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "mailsendsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                objSpace.CommitChanges();
                View.ObjectSpace.CommitChanges();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>()
                    .InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void ProcessAction(string parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(parameter))
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    string[] param = parameter.Split('|');
                    if(param[1]=="Mail")
                    {
                        ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                        if(!string.IsNullOrEmpty(param[0]))
                        {
                            Modules.BusinessObjects.SampleManagement.Reporting objreporting = View.ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[Oid]=?", new Guid(param[0])),true);
                            //objreporting = os.FindObject<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[Oid]=?", new Guid(param[0])));
                           
                            if (objreporting != null && objreporting.Mail)
                            {
                                objreporting.Email = null;
                            }
                            else
                            {
                                if (objreporting != null && objreporting.JobID.ClientName.Oid != null && (objreporting.Email == null || objreporting.Email.Trim().Length == 0))
                                {
                                    IList<Contact> objconEmail = View.ObjectSpace.GetObjects<Contact>(CriteriaOperator.Parse("Not IsNullOrEmpty([Email]) And [Customer.Oid] = ? And ReportDelivery = true", objreporting.JobID.ClientName.Oid));
                                    if (objconEmail != null && objconEmail.Count > 0)
                                    {
                                        string lstmail = string.Empty;
                                        foreach (Contact objContact in objconEmail)
                                        {
                                            if (!string.IsNullOrEmpty(objContact.Email))
                                            {
                                                if (string.IsNullOrEmpty(lstmail))
                                                {
                                                    lstmail = objContact.Email;
                                                }
                                                else if (!string.IsNullOrEmpty(lstmail))
                                                {
                                                    lstmail = lstmail + ", " + objContact.Email;
                                                }
                                            }
                                        }
                                        objreporting.Email = lstmail;
                                    }
                                }
                            }
                            View.ObjectSpace.CommitChanges();
                            View.Refresh();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>()
                   .InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
