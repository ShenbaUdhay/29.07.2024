using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Pdf;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Utils.Extensions;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraReports.UI;
using DynamicDesigner;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.DrinkingWaterQualityReports;
using Modules.BusinessObjects.TaskManagement;

namespace LDM.Module.Controllers.Settings.DWQRReportTemplateSetup
{
    public partial class DWQRReportTemplateSetupViewController1 : ViewController
    {
        MessageTimer timer = new MessageTimer();
        DynamicReportDesignerConnection ObjReportDesignerInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        curlanguage objLanguage = new curlanguage();
        SampleRegistrationInfo Sampleinfo = new SampleRegistrationInfo();

        private string uqOid;
        private string JobID;
        private string SampleID;
        private string QcBatchID;
        private string SampleParameterID;
        private string TestMethodID;
        private string ParameterID;
        bool boolReportSave = false;
        string strReportIDT = string.Empty;
        public DWQRReportTemplateSetupViewController1()
        {
            InitializeComponent();
            TargetViewId = "DWQRReportTemplateSetup_ListView;" + "DWQRReportTemplateSetup_DetailView;" + "DWQRReportTemplateSetup_LookupListView;" + "DWQRReportTemplateSetup_SampleSites_ListView;"
                + "Testparameter_LookupListView_DWQR_Report;" + "DWQRReportTemplateSetup_ParameterCollection_ListView;" + "SampleSites_LookupListView_DWQR;" + "DWQRReportTemplateSetup_DetailView_DateRange;"
                + "DWQRReportTemplateSetup_DetailView_DWQRForm;" + "DWQRReportTemplateSetup_ListView_DWQRForm;" + "DWQRReportTemplateSetup_SampleSites_ListView_DWQR_ReportID;" + "DrinkingWaterQualityReports_DetailView;"
                + "DrinkingWaterQualityReports_ListView;" + "DrinkingWaterQualityReports_ListView_Delivered;" + "DrinkingWaterQualityReports_ListView_PendingApproval;" + "DrinkingWaterQualityReports_ListView_PendingDelivery;"
                + "DrinkingWaterQualityReports_LookupListView;" + "DrinkingWaterQualityReports_DashBoard;" + "DWQRReportTemplateSetup_ParameterCollection_ListView_Report;" + "DrinkingWaterQualityReports_DetailView_PendingApproval;"
                + "DrinkingWaterQualityReports_DetailView_PendingDelivery;" + "DrinkingWaterQualityReports_DetailView_Delivered;";
            btn_DWQRTestData.TargetViewId = "DWQRReportTemplateSetup_DetailView;";
            btn_DWQRPreview.TargetViewId = "DrinkingWaterQualityReports_DetailView;";
            btn_DWQRPreview.TargetObjectsCriteria = "Not IsNullOrEmpty([TemplateName]) And Not IsNullOrEmpty([ReportName]) And [Status] == 'PendingSubmission'";
            btn_DWQRSubmit.TargetViewId = "DrinkingWaterQualityReports_DetailView;";
            btn_DWQRSubmit.TargetObjectsCriteria = "Not IsNullOrEmpty([ReportID]) And [Status] == 'PendingSubmission'";
            btn_DWQRApprove.TargetViewId = "DrinkingWaterQualityReports_DetailView_PendingApproval;";
            btn_DWQRHistory.TargetViewId = "DrinkingWaterQualityReports_ListView_PendingDelivery;";
            btn_DWQRPreviewToApprove.TargetViewId = "DrinkingWaterQualityReports_DetailView_PendingApproval;" + "DrinkingWaterQualityReports_DetailView_PendingDelivery;" + "DrinkingWaterQualityReports_DetailView;";
            btn_DWQRPreviewToApprove.TargetObjectsCriteria = "[Status] <> 'PendingSubmission'";
            btn_DWQRRollBack.TargetViewId = "DrinkingWaterQualityReports_DetailView_PendingApproval;" + "DrinkingWaterQualityReports_DetailView_PendingDelivery;";
            btn_DWQRRollBack.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            btn_DWQRDateFilter.TargetViewId = "DrinkingWaterQualityReports_ListView;" + "DrinkingWaterQualityReports_ListView_Delivered;";
            btn_DWQRDeliverReport.TargetViewId = "DrinkingWaterQualityReports_DetailView_PendingDelivery;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "DWQRReportTemplateSetup_SampleSites_ListView" || View.Id == "DWQRReportTemplateSetup_ParameterCollection_ListView" || View.Id == "Testparameter_LookupListView_DWQR_Report"
                    || View.Id == "DWQRReportTemplateSetup_SampleSites_ListView_DWQR_ReportID" || View.Id == "DWQRReportTemplateSetup_ParameterCollection_ListView_Report")
                {
                    Frame.GetController<WebExportController>().ExportAction.Active.SetItemValue("DisableExport", false);

                    if (View.Id == "DWQRReportTemplateSetup_ParameterCollection_ListView" || View.Id == "DWQRReportTemplateSetup_SampleSites_ListView" || View.Id == "DWQRReportTemplateSetup_SampleSites_ListView_DWQR_ReportID"
                        || View.Id == "DWQRReportTemplateSetup_ParameterCollection_ListView_Report")
                    {
                        Frame.GetController<LinkUnlinkController>().LinkAction.Caption = "Add";
                        Frame.GetController<LinkUnlinkController>().LinkAction.ToolTip = "Add Parameter";
                        Frame.GetController<LinkUnlinkController>().UnlinkAction.Caption = "Remove";
                        Frame.GetController<LinkUnlinkController>().UnlinkAction.ToolTip = "Remove Parameter";
                        Frame.GetController<LinkUnlinkController>().LinkAction.ImageName = "Add";
                        Frame.GetController<LinkUnlinkController>().UnlinkAction.ImageName = "Remove";
                        Frame.GetController<LinkUnlinkController>().LinkAction.CustomizePopupWindowParams += LinkAction_CustomizePopupWindowParams;
                    }
                }

                if (View.Id == "DrinkingWaterQualityReports_DetailView" || View.Id == "DrinkingWaterQualityReports_DetailView_PendingApproval" || View.Id == "DrinkingWaterQualityReports_DetailView_PendingDelivery"
                    || View.Id == "DrinkingWaterQualityReports_DetailView_Delivered")
                {
                    DrinkingWaterQualityReports getObjectsOfDWQRReport = View.CurrentObject as DrinkingWaterQualityReports;
                    if (View.Id == "DrinkingWaterQualityReports_DetailView")
                    {
                        if (getObjectsOfDWQRReport != null && getObjectsOfDWQRReport.Status == DWQRStatus.PendingSubmission)
                        {
                            btn_DWQRPreviewToApprove.Active.SetItemValue("DisablePreviewSavedData", false);
                        }
                        else if (getObjectsOfDWQRReport != null && getObjectsOfDWQRReport.Status != DWQRStatus.PendingSubmission)
                        {
                            btn_DWQRPreview.Active.SetItemValue("DisablePreviewGenerateReport", false);
                        }
                    }
                    DashboardViewItem gridDWQRParameters = ((DetailView)View).FindItem("DWQRParameters_Grid") as DashboardViewItem;
                    DashboardViewItem gridDWQRSampleLocation = ((DetailView)View).FindItem("DWQRSampleLocation_Grid") as DashboardViewItem;
                    if ((gridDWQRParameters != null && gridDWQRParameters.InnerView == null) || (gridDWQRSampleLocation != null && gridDWQRSampleLocation.InnerView == null))
                    {
                        gridDWQRParameters.CreateControl();
                        gridDWQRSampleLocation.CreateControl();
                    }
                    if (gridDWQRParameters != null && gridDWQRParameters.InnerView != null && gridDWQRSampleLocation != null && gridDWQRSampleLocation.InnerView != null && getObjectsOfDWQRReport.TemplateName == null)
                    {
                        if (gridDWQRParameters.InnerView.Id == "DWQRReportTemplateSetup_ParameterCollection_ListView_Report")
                        {
                            ((ListView)gridDWQRParameters.InnerView).CollectionSource.Criteria["DWQRParamsFilter"] = CriteriaOperator.Parse("1=2");
                        }
                        if (gridDWQRSampleLocation.InnerView.Id == "DWQRReportTemplateSetup_SampleSites_ListView_DWQR_ReportID")
                        {
                            ((ListView)gridDWQRSampleLocation.InnerView).CollectionSource.Criteria["DWQRLocationFilter"] = CriteriaOperator.Parse("1=2");
                        }
                    }
                    else
                    {
                        List<Guid> sampleSiteIdList = getObjectsOfDWQRReport.TemplateName.SampleSites.Select(i => i.Oid).ToList();
                        List<Guid> paraMetersIdList = getObjectsOfDWQRReport.TemplateName.ParameterCollection.Select(j => j.Oid).ToList();
                        if (gridDWQRParameters.InnerView.Id == "DWQRReportTemplateSetup_ParameterCollection_ListView_Report")
                        {
                            ((ListView)gridDWQRParameters.InnerView).CollectionSource.Criteria["DWQRParamsFilter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", paraMetersIdList)) + ")");
                        }
                        if (gridDWQRSampleLocation.InnerView.Id == "DWQRReportTemplateSetup_SampleSites_ListView_DWQR_ReportID")
                        {
                            ((ListView)gridDWQRSampleLocation.InnerView).CollectionSource.Criteria["DWQRLocationFilter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", sampleSiteIdList)) + ")");
                        }
                    }
                    ObjectSpace.ObjectChanged += DWQRObjectSpace_ObjectChanged;
                    Frame.GetController<ModificationsController>().SaveAction.Executing += DWQRSaveAction_Executing;
                }

                if (View.Id == "DrinkingWaterQualityReports_ListView" || View.Id == "DrinkingWaterQualityReports_ListView_Delivered")
                {
                    //if (btn_DWQRDateFilter.SelectedItem == null)
                    //{
                    //    btn_DWQRDateFilter.SelectedItem = btn_DWQRDateFilter.Items[2];
                    //}
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (btn_DWQRDateFilter.SelectedItem == null)
                    {
                        if (setting.ReportingWorkFlow == EnumDateFilter.OneMonth)
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateCreated >= ?", DateTime.Today.AddMonths(-1));
                            btn_DWQRDateFilter.SelectedItem = btn_DWQRDateFilter.Items[0];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.ThreeMonth)
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateCreated >= ?", DateTime.Today.AddMonths(-3));
                            btn_DWQRDateFilter.SelectedItem = btn_DWQRDateFilter.Items[1];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.SixMonth)
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-6);
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateCreated >= ?", DateTime.Today.AddMonths(-6));
                            btn_DWQRDateFilter.SelectedItem = btn_DWQRDateFilter.Items[2];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.OneYear)
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-1);
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateCreated >= ?", DateTime.Today.AddYears(-1));
                            btn_DWQRDateFilter.SelectedItem = btn_DWQRDateFilter.Items[3];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.TwoYear)
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-2);
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateCreated >= ?", DateTime.Today.AddYears(-2));
                            btn_DWQRDateFilter.SelectedItem = btn_DWQRDateFilter.Items[4];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.FiveYear)
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-5);
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateCreated >= ?", DateTime.Today.AddYears(-5));
                            btn_DWQRDateFilter.SelectedItem = btn_DWQRDateFilter.Items[5];
                        }
                        else
                        {
                            //objRQPInfo.rgFilterByMonthDate = DateTime.MinValue;
                            btn_DWQRDateFilter.SelectedItem = btn_DWQRDateFilter.Items[6];
                            ((ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                        }
                        //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    }
                    btn_DWQRDateFilter.SelectedItemChanged += Btn_DWQRDateFilter_SelectedItemChanged;
                }
                if (View.Id == "DWQRReportTemplateSetup_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executing += DWQRSetupSaveAction_Executing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DWQRSetupSaveAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.Setting.DWQRReportTemplateSetup.DWQRReportTemplateSetup getObjectsOfDWQRSetup = View.CurrentObject as Modules.BusinessObjects.Setting.DWQRReportTemplateSetup.DWQRReportTemplateSetup;
                if (getObjectsOfDWQRSetup.EmailTo == null)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillEmailTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
                else if (getObjectsOfDWQRSetup.EmailTo != null)
                {
                    //bool isValid = true;
                    string emails = getObjectsOfDWQRSetup.EmailTo;
                    string[] emailArray = emails.Split(',');
                    List<string> lstOfIncorrectEmails = new List<string>();
                    foreach (string email in emailArray)
                    {
                        if (!IsValidEmail(email.Trim()))
                        {
                            lstOfIncorrectEmails.Add(email);
                        }
                    }
                    if (lstOfIncorrectEmails.Count > 0)
                    {
                        string invalidEmails = string.Join(", ", lstOfIncorrectEmails);
                        Application.ShowViewStrategy.ShowMessage("' " + invalidEmails + " ' " + CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "ValidateTheFormatOfEmails"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "SetupIDCreated"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Btn_DWQRDateFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null)
                {
                    if (View.Id == "DrinkingWaterQualityReports_ListView" || View.Id == "DrinkingWaterQualityReports_ListView_Delivered")
                    {
                        if (btn_DWQRDateFilter.SelectedItem != null)
                        {
                            if (btn_DWQRDateFilter.SelectedItem.Id == "1M")
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateCreated >= ?", DateTime.Today.AddMonths(-1));
                            }
                            if (btn_DWQRDateFilter.SelectedItem.Id == "3M")
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateCreated >= ?", DateTime.Today.AddMonths(-3));
                            }
                            else if (btn_DWQRDateFilter.SelectedItem.Id == "6M")
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateCreated >= ?", DateTime.Today.AddMonths(-6));
                            }
                            else if (btn_DWQRDateFilter.SelectedItem.Id == "1Y")
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateCreated >= ?", DateTime.Today.AddYears(-1));
                            }
                            else if (btn_DWQRDateFilter.SelectedItem.Id == "2Y")
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateCreated >= ?", DateTime.Today.AddYears(-2));
                            }
                            else if (btn_DWQRDateFilter.SelectedItem.Id == "5Y")
                            {
                                ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateCreated >= ?", DateTime.Today.AddYears(-5));
                            }
                            else if (btn_DWQRDateFilter.SelectedItem.Id == "All")
                            {
                                ((ListView)View).CollectionSource.Criteria.Remove("dateFilter");
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

        private void DWQRSaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                DrinkingWaterQualityReports getObjectsOfDWQRReport = View.CurrentObject as DrinkingWaterQualityReports;
                if (getObjectsOfDWQRReport != null)
                {
                    if (getObjectsOfDWQRReport.DateCollectedFrom > getObjectsOfDWQRReport.DateCollectedTo)
                    {
                        if (getObjectsOfDWQRReport.DateCollectedTo == DateTime.MinValue)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillDateCollectedTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "!DateCollectedFrom>DateCollectedTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);

                            getObjectsOfDWQRReport.DateCollectedFrom = DateTime.MinValue;
                        }

                    }
                    else if (getObjectsOfDWQRReport.DateCollectedFrom == DateTime.MinValue && getObjectsOfDWQRReport.DateCollectedTo == DateTime.MinValue)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillDateCollectedFrom&DateCollectedTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else if (getObjectsOfDWQRReport.DateCollectedFrom == DateTime.MinValue)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillDateCollectedFrom"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        bool UnApprove = false;
                        List<Guid> oidOfTP = new List<Guid>();
                        List<Guid> oidOfSampleSite = new List<Guid>();
                        DashboardViewItem lstPorpGetParamsDWQR = ((DetailView)Application.MainWindow.View).FindItem("DWQRParameters_Grid") as DashboardViewItem;
                        DashboardViewItem lstPorpGetSampleSiteDWQR = ((DetailView)Application.MainWindow.View).FindItem("DWQRSampleLocation_Grid") as DashboardViewItem;
                        if (lstPorpGetParamsDWQR != null && lstPorpGetParamsDWQR.InnerView != null && lstPorpGetSampleSiteDWQR != null && lstPorpGetSampleSiteDWQR.InnerView != null)
                        {
                            oidOfTP = ((ListView)lstPorpGetParamsDWQR.InnerView).CollectionSource.List.Cast<Testparameter>().Select(tp => tp.Oid).ToList();
                            oidOfSampleSite = ((ListView)lstPorpGetSampleSiteDWQR.InnerView).CollectionSource.List.Cast<SampleSites>().Select(ss => ss.Oid).ToList();
                        }
                        //IList<SampleParameter> listSP = objectSpace.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[Testparameter.Oid] In(" + string.Format("'{0}'", string.Join("','", oidOfTP)) + ")"), CriteriaOperator.Parse("[Samplelogin.SiteName.Oid] In(" + string.Format("'{0}'", string.Join("','", oidOfSampleSite)) + ")"), (CriteriaOperator.Parse("GETDATE([Samplelogin.CollectDate]) BETWEEN('" + getObjectsOfDWQRReport.DateCollectedFrom + "', '" + getObjectsOfDWQRReport.DateCollectedTo + "')"))));
                        IList<SampleParameter> listSP = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status] = 'PendingReporting' And [Testparameter.Oid] In(" + string.Format("'{0}'", string.Join("','", oidOfTP)) + ") And [Samplelogin.StationLocation.Oid] In(" + string.Format("'{0}'", string.Join("','", oidOfSampleSite)) + ") And GETDATE([Samplelogin.CollectDate]) BETWEEN('" + getObjectsOfDWQRReport.DateCollectedFrom + "', '" + getObjectsOfDWQRReport.DateCollectedTo + "')"));
                        if (listSP != null && listSP.Count > 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "ReportIDCreated"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "NoReportIDCreated"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
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
        private bool IsValidEmail(string email)
        {
            try
            {
                // Regular expression to validate email format
                string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                return Regex.IsMatch(email, emailPattern);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return false;
            }
        }

        private void DWQRObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View.Id == "DrinkingWaterQualityReports_DetailView")
                {
                    if (e.PropertyName == "TemplateName" && e.OldValue != e.NewValue)
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();

                        DrinkingWaterQualityReports getCurrentTemplate = View.CurrentObject as DrinkingWaterQualityReports;
                        if (getCurrentTemplate != null && getCurrentTemplate.TemplateName != null)
                        {
                            getCurrentTemplate.ReportName = getCurrentTemplate.TemplateName.ReportName;
                            List<Guid> sampleSiteIdList = getCurrentTemplate.TemplateName.SampleSites.Select(i => i.Oid).ToList();
                            List<Guid> paraMetersIdList = getCurrentTemplate.TemplateName.ParameterCollection.Select(j => j.Oid).ToList();
                            DashboardViewItem dviSampleSite = ((DetailView)View).FindItem("DWQRSampleLocation_Grid") as DashboardViewItem;
                            if (dviSampleSite != null)
                            {
                                if (dviSampleSite.InnerView == null)
                                {
                                    dviSampleSite.CreateControl();
                                }
                                if (e.NewValue != null)
                                {

                                    //((ListView)dviSampleSite.InnerView).CollectionSource.Criteria["DWQRLocationFilter"] = new InOperator("Oid", sampleSiteIdList.Select(i=>i).ToList());
                                    //((ListView)dviSampleSite.InnerView).CollectionSource.Criteria["DWQRLocationFilter"] = CriteriaOperator.Parse("[Oid] = In(" + string.Format("'{0}'", string.Join("','", sampleSiteIdList.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                    ((ListView)dviSampleSite.InnerView).CollectionSource.Criteria["DWQRLocationFilter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", sampleSiteIdList)) + ")");
                                }
                                else
                                {
                                    ((ListView)dviSampleSite.InnerView).CollectionSource.Criteria["DWQRLocationFilter"] = CriteriaOperator.Parse("1=2");
                                }
                            }
                            DashboardViewItem dviTestParameters = ((DetailView)View).FindItem("DWQRParameters_Grid") as DashboardViewItem;
                            if (dviTestParameters != null)
                            {
                                if (dviTestParameters.InnerView == null)
                                {
                                    dviTestParameters.CreateControl();
                                }
                                if (e.NewValue != null)
                                {
                                    //((ListView)dviTestParameters.InnerView).CollectionSource.Criteria["DWQRParamsFilter"] = new InOperator("Oid", paraMetersIdList);
                                    ((ListView)dviTestParameters.InnerView).CollectionSource.Criteria["DWQRParamsFilter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", paraMetersIdList)) + ")");
                                }
                                else
                                {
                                    ((ListView)dviTestParameters.InnerView).CollectionSource.Criteria["DWQRParamsFilter"] = CriteriaOperator.Parse("1=2");
                                }
                            }
                        }
                        objectSpace.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void LinkAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                if (e.View.Id == "Testparameter_LookupListView_DWQR_Report")
                {
                    ((ListView)e.View).CollectionSource.Criteria["HideSelectedParameters"] = new NotOperator(new InOperator("Oid", ((ListView)View).CollectionSource.List.Cast<Testparameter>().Select(tp => tp.Oid)));
                }
                else if (e.View.Id == "SampleSites_LookupListView_DWQR")
                {
                    ((ListView)e.View).CollectionSource.Criteria["HideSelectedLocation"] = new NotOperator(new InOperator("Oid", ((ListView)View).CollectionSource.List.Cast<SampleSites>().Select(ss => ss.Oid)));
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
                if (View.Id == "DWQRReportTemplateSetup_DetailView" || View.Id == "DrinkingWaterQualityReports_DetailView")
                {
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = (ASPxStringPropertyEditor)item;
                            if (propertyEditor != null && propertyEditor.Editor != null && (propertyEditor.PropertyName == "TemplateID" || propertyEditor.PropertyName == "ReportID"))
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
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
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            try
            {
                if (View.Id == "DWQRReportTemplateSetup_SampleSites_ListView" || View.Id == "DWQRReportTemplateSetup_ParameterCollection_ListView" || View.Id == "Testparameter_LookupListView_DWQR_Report"
                    || View.Id == "DWQRReportTemplateSetup_SampleSites_ListView_DWQR_ReportID" || View.Id == "DWQRReportTemplateSetup_ParameterCollection_ListView_Report")
                {
                    Frame.GetController<WebExportController>().ExportAction.Active.SetItemValue("DisableExport", true);
                }
                if (View.Id == "DWQRReportTemplateSetup_ParameterCollection_ListView" || View.Id == "DWQRReportTemplateSetup_SampleSites_ListView" || View.Id == "DWQRReportTemplateSetup_SampleSites_ListView_DWQR_ReportID"
                    || View.Id == "DWQRReportTemplateSetup_ParameterCollection_ListView_Report")
                {
                    Frame.GetController<LinkUnlinkController>().LinkAction.CustomizePopupWindowParams -= LinkAction_CustomizePopupWindowParams;
                }
                if (View.Id == "DrinkingWaterQualityReports_DetailView")
                {
                    ObjectSpace.ObjectChanged -= DWQRObjectSpace_ObjectChanged;
                    Frame.GetController<ModificationsController>().SaveAction.Executing -= DWQRSaveAction_Executing;

                    DrinkingWaterQualityReports getObjectsOfDWQRReport = View.CurrentObject as DrinkingWaterQualityReports;
                    btn_DWQRPreviewToApprove.Active.RemoveItem("DisablePreviewSavedData");
                    btn_DWQRPreview.Active.RemoveItem("DisablePreviewGenerateReport");
                }
                if (View.Id == "DWQRReportTemplateSetup_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executing -= DWQRSetupSaveAction_Executing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void btn_DWQRTestData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id != null)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    Modules.BusinessObjects.Setting.DWQRReportTemplateSetup.DWQRReportTemplateSetup objCurrent = View.CurrentObject as Modules.BusinessObjects.Setting.DWQRReportTemplateSetup.DWQRReportTemplateSetup;
                    if (objCurrent.SampleSites.Count > 0 && objCurrent.ParameterCollection.Count > 0)
                    {
                        View.ObjectSpace.CommitChanges();
                        Modules.BusinessObjects.Setting.DWQRReportTemplateSetup.DWQRReportTemplateSetup getObjectsOfDWQRReport = os.GetObject<Modules.BusinessObjects.Setting.DWQRReportTemplateSetup.DWQRReportTemplateSetup>(objCurrent);
                        DetailView dvDateRange = Application.CreateDetailView(os, "DWQRReportTemplateSetup_DetailView_DateRange", false, getObjectsOfDWQRReport);
                        dvDateRange.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters(dvDateRange);
                        showViewParameters.CreatedView = dvDateRange;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Dc_AcceptingDWQRDateRange;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    else
                    {
                        if (objCurrent.SampleSites.Count == 0 && objCurrent.ParameterCollection.Count == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillSampleLocationAndParameter"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                        else if (objCurrent.SampleSites.Count == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillSampleLocation"), InformationType.Info, timer.Seconds, InformationPosition.Top);

                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillParameter"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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

        private void Dc_AcceptingDWQRDateRange(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.Setting.DWQRReportTemplateSetup.DWQRReportTemplateSetup getObjectsOfDWQRReport = e.AcceptActionArgs.CurrentObject as Modules.BusinessObjects.Setting.DWQRReportTemplateSetup.DWQRReportTemplateSetup;
                if (getObjectsOfDWQRReport != null)
                {
                    if (getObjectsOfDWQRReport.DateFrom > getObjectsOfDWQRReport.DateTo)
                    {
                        if (getObjectsOfDWQRReport.DateTo == DateTime.MinValue)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillDateTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "!DateFrom>DateTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            getObjectsOfDWQRReport.DateFrom = DateTime.MinValue;
                            e.Cancel = true;
                        }
                    }
                    else if (getObjectsOfDWQRReport.DateFrom == DateTime.MinValue && getObjectsOfDWQRReport.DateTo == DateTime.MinValue)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillDateFrom&DateTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                    }
                    else if (getObjectsOfDWQRReport.DateFrom == DateTime.MinValue)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillDateFrom"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                    }
                    else
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        bool UnApprove = false;
                        List<Guid> oidOfTP = new List<Guid>();
                        List<Guid> oidOfSampleSite = new List<Guid>();
                        ListPropertyEditor lstPorpGetParamsDWQR = ((DetailView)Application.MainWindow.View).FindItem("ParameterCollection") as ListPropertyEditor;
                        ListPropertyEditor lstPorpGetSampleSiteDWQR = ((DetailView)Application.MainWindow.View).FindItem("SampleSites") as ListPropertyEditor;
                        if (lstPorpGetParamsDWQR != null && lstPorpGetParamsDWQR.ListView != null && lstPorpGetSampleSiteDWQR != null && lstPorpGetSampleSiteDWQR.ListView != null)
                        {
                            oidOfTP = lstPorpGetParamsDWQR.ListView.CollectionSource.List.Cast<Testparameter>().Select(tp => tp.Oid).ToList();
                            oidOfSampleSite = lstPorpGetSampleSiteDWQR.ListView.CollectionSource.List.Cast<SampleSites>().Select(ss => ss.Oid).ToList();
                        }
                        //IList<SampleParameter> listSP = objectSpace.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, new InOperator("Testparameter.Oid", oidOfTP), new InOperator("Samplelogin.SiteName.Oid", oidOfSampleSite),(CriteriaOperator.Parse("GETDATE([Samplelogin.CollectDate]) BETWEEN('" + getObjectsOfDWQRReport.DateFrom + "', '" + getObjectsOfDWQRReport.DateTo + "')"))));
                        IList<SampleParameter> listSP = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status] = 'PendingReporting' And [Testparameter.Oid] In(" + string.Format("'{0}'", string.Join("','", oidOfTP)) + ") And [Samplelogin.StationLocation.Oid] In(" + string.Format("'{0}'", string.Join("','", oidOfSampleSite)) + ") And GETDATE([Samplelogin.CollectDate]) BETWEEN('" + getObjectsOfDWQRReport.DateFrom + "', '" + getObjectsOfDWQRReport.DateTo + "')"));
                        if (listSP != null)
                        {
                            bool IsAllowMultipleJobID = true;
                            List<string> lstreport = new List<string>();
                            IList<ReportPackage> objrep = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?", getObjectsOfDWQRReport.ReportName));
                            foreach (ReportPackage objrp in objrep.ToList())
                            {
                                lstreport.Add(objrp.ReportName);
                            }

                            var jobid = listSP.Select(i => i.Samplelogin.JobID).Distinct().ToList();
                            if (jobid.Count() > 1)
                            {
                                if (lstreport.Count > 0)
                                {
                                    IList<tbl_Public_CustomReportDesignerDetails> objreport = ObjectSpace.GetObjects<tbl_Public_CustomReportDesignerDetails>(new InOperator("colCustomReportDesignerName", lstreport));
                                    List<tbl_Public_CustomReportDesignerDetails> lstAllowjobidcnt = objreport.Where(i => i.AllowMultipleJOBID == false).ToList();
                                    if (lstAllowjobidcnt.Count > 0)
                                    {
                                        IsAllowMultipleJobID = false;
                                    }
                                }
                            }
                            if (IsAllowMultipleJobID == true)
                            {
                                uqOid = null;
                                foreach (SampleParameter obj in listSP)
                                {
                                    if (obj.NotReport == false)
                                    {
                                        if (uqOid == null)
                                        {
                                            uqOid = "'" + obj.Oid.ToString() + "'";
                                            JobID = "'" + obj.Samplelogin.JobID.JobID + "'";
                                            SampleID = "'" + obj.Samplelogin.SampleID + "'";
                                            SampleParameterID = "'" + obj.Testparameter.Parameter.Oid.ToString() + "'";
                                            TestMethodID = "'" + obj.Testparameter.TestMethod.Oid.ToString() + "'";
                                            ParameterID = "'" + obj.Testparameter.Parameter.Oid.ToString() + "'";
                                        }
                                        else
                                        {
                                            uqOid = uqOid + ",'" + obj.Oid.ToString() + "'";
                                            if (!JobID.Contains(obj.Samplelogin.JobID.JobID))
                                            {
                                                JobID = JobID + ",'" + obj.Samplelogin.JobID.JobID + "'";
                                            }
                                            if (!SampleID.Contains(obj.Samplelogin.SampleID))
                                            {
                                                SampleID = SampleID + ",'" + obj.Samplelogin.SampleID + "'";
                                            }
                                            if (!SampleParameterID.Contains(obj.Testparameter.Parameter.Oid.ToString()))
                                            {
                                                SampleParameterID = SampleParameterID + ",'" + obj.Testparameter.Parameter.Oid.ToString() + "'";
                                            }
                                            //if (obj.QCBatchID != null && obj.QCBatchID.qcseqdetail != null)
                                            //{
                                            //    if (string.IsNullOrEmpty(QcBatchID) == false && !QcBatchID.Contains(obj.QCBatchID.qcseqdetail.AnalyticalBatchID))
                                            //    {
                                            //        QcBatchID = QcBatchID + ",'" + obj.QCBatchID.qcseqdetail.AnalyticalBatchID + "'";
                                            //    }
                                            //}
                                            if (!TestMethodID.Contains(obj.Testparameter.TestMethod.Oid.ToString()))
                                            {
                                                TestMethodID = TestMethodID + ",'" + obj.Testparameter.TestMethod.Oid.ToString() + "'";
                                            }
                                            if (!ParameterID.Contains(obj.Testparameter.Parameter.Oid.ToString()))
                                            {
                                                ParameterID = ParameterID + ",'" + obj.Testparameter.Parameter.Oid.ToString() + "'";
                                            }
                                        }
                                        if (!SampleID.Contains(obj.Samplelogin.SampleID))
                                        {
                                            SampleID = SampleID + ",'" + obj.Samplelogin.SampleID + "'";
                                        }
                                    }
                                    if (obj.Status == Samplestatus.PendingEntry || obj.Status == Samplestatus.PendingValidation || obj.Status == Samplestatus.PendingApproval)
                                    {
                                        UnApprove = true;
                                    }
                                }

                                XtraReport xtraReport = new XtraReport();
                                ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                                ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                SetConnectionString();
                                DynamicReportBusinessLayer.BLCommon.SetDBConnection(ObjReportDesignerInfo.LDMSQLServerName, ObjReportDesignerInfo.LDMSQLDatabaseName, ObjReportDesignerInfo.LDMSQLUserID, ObjReportDesignerInfo.LDMSQLPassword);
                                IObjectSpace objSpace = Application.CreateObjectSpace();
                                Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                                if (boolReportSave == true && string.IsNullOrEmpty(strReportIDT))
                                {
                                    strReportIDT = null;
                                    ReportIDFormat idFormat = ObjectSpace.FindObject<ReportIDFormat>(null);
                                    if (idFormat != null)
                                    {
                                        string jd = null;
                                        var curdate = DateTime.Now;
                                        if (idFormat.Prefixs == YesNoFilters.Yes)
                                        {
                                            strReportIDT = idFormat.PrefixsValue;
                                        }
                                        if (idFormat.ReportIDFormatOption == ReportIDFormatOption.No)
                                        {
                                            foreach (string jobId in View.SelectedObjects.OfType<SampleParameter>().Select(sp => sp.Samplelogin.JobID.JobID).Distinct())
                                            {
                                                jd = jobId;
                                                strReportIDT += jobId;
                                            }
                                            //jd += jobid;
                                            //strReportIDT += jobid;
                                            if (idFormat.SequentialNumber > 0)
                                            {
                                                var latestReport = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>().Where(r => r.JobID.JobID == jd /*&& r.NewReportFormat is true*/).OrderByDescending(r => r.ReportedDate).FirstOrDefault();
                                                if (latestReport != null && latestReport.JobID != null && latestReport.NewReportFormat == true)
                                                {
                                                    //latestReport.NewReportFormat = true;
                                                    string latestJobID = latestReport.JobID.JobID;
                                                    bool isJobIDMatch = latestJobID.Contains(jd);
                                                    if (isJobIDMatch)
                                                    {
                                                        latestReport = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>().Where(r => r.ReportID.Contains(jd)).OrderByDescending(r => r.ReportedDate).FirstOrDefault();
                                                        if (latestReport != null)
                                                        {
                                                            string baseValue = latestReport.ReportID.Substring(0, latestReport.ReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                                                            string lastDigits = latestReport.ReportID.Substring(latestReport.ReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                                                            int nextSequentialNumber = int.Parse(lastDigits) + 1;
                                                            strReportIDT += nextSequentialNumber.ToString().PadLeft(Convert.ToInt32(idFormat.SequentialNumber), '0');
                                                            //for (int i = 0; i < idFormat.SequentialNumber; i++)
                                                            //{
                                                            //    string str = "0";
                                                            //    strReportIDT += str;
                                                            //}
                                                            //string lastDigits = latestReport.ReportID.Substring(latestReport.ReportID.Length - 3);
                                                            //int nextSequentialNumber = int.Parse(lastDigits) + 1;
                                                            //strReportIDT = strReportIDT.Substring(0, strReportIDT.Length - 3) + nextSequentialNumber.ToString().PadLeft(3, '0');

                                                        }
                                                        else
                                                        {
                                                            strReportIDT = strReportIDT + new string('0', Convert.ToInt32(idFormat.SequentialNumber - 1)) + "1";
                                                            //strReportIDT += "001";
                                                            //latestReport.NewReportFormat = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        strReportIDT = strReportIDT + new string('0', Convert.ToInt32(idFormat.SequentialNumber - 1)) + "1";
                                                        //strReportIDT += "001";
                                                        //latestReport.NewReportFormat = true;
                                                    }
                                                }
                                                else
                                                {
                                                    strReportIDT = strReportIDT + new string('0', Convert.ToInt32(idFormat.SequentialNumber - 1)) + "1";
                                                    //latestReport.NewReportFormat = true;
                                                    //strReportIDT += "001";
                                                }
                                            }
                                        }
                                        if (idFormat.ReportIDFormatOption == ReportIDFormatOption.Yes)
                                        {
                                            string currentDateSubstring = "";

                                            if (idFormat.Year == YesNoFilters.Yes)
                                            {
                                                strReportIDT += curdate.ToString(idFormat.YearFormat.ToString());
                                                currentDateSubstring += curdate.ToString(idFormat.YearFormat.ToString());
                                            }
                                            if (idFormat.Month == YesNoFilters.Yes)
                                            {
                                                strReportIDT += curdate.ToString(idFormat.MonthFormat.ToUpper());
                                                currentDateSubstring += curdate.ToString(idFormat.MonthFormat.ToUpper());
                                            }
                                            if (idFormat.Day == YesNoFilters.Yes)
                                            {
                                                strReportIDT += curdate.ToString(idFormat.DayFormat);
                                                currentDateSubstring += curdate.ToString(idFormat.DayFormat);
                                            }
                                            if (idFormat.SequentialNumber > 0)
                                            {

                                                var latestReport = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>().OrderByDescending(r => r.ReportedDate).FirstOrDefault(r => r.ReportID.Contains(currentDateSubstring));
                                                string strReportID = string.Empty;
                                                if (latestReport != null && latestReport.NewReportFormat == true)
                                                {
                                                    strReportID = latestReport.ReportID;
                                                    //latestReport.NewReportFormat = true;
                                                    //for (int i = 0; i < idFormat.SequentialNumber; i++)
                                                    //{
                                                    //    string str = "0";
                                                    //    strReportIDT += str;
                                                    //}
                                                    //string lastDigits = latestReport.ReportID.Substring(latestReport.ReportID.Length - 3);
                                                    //int nextSequentialNumber = int.Parse(lastDigits) + 1;
                                                    //strReportIDT = strReportIDT.Substring(0, strReportIDT.Length - 3) + nextSequentialNumber.ToString().PadLeft(3, '0');
                                                }
                                                if (!string.IsNullOrEmpty(strReportID))
                                                {
                                                    string baseValue = strReportID.Substring(0, strReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                                                    string lastDigits = strReportID.Substring(strReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                                                    int nextSequentialNumber = int.Parse(lastDigits) + 1;
                                                    strReportIDT += nextSequentialNumber.ToString().PadLeft(Convert.ToInt32(idFormat.SequentialNumber), '0');
                                                    // newReportID = newReportID + new string('0', Convert.ToInt32(idFormat.SequentialNumber-1));
                                                    ////for (int i = 0; i < idFormat.SequentialNumber; i++)
                                                    ////{
                                                    ////    string str = "0";
                                                    ////    newReportID += str;
                                                    ////}
                                                    //string lastDigits = strReportID.Substring(strReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                                                    //int nextSequentialNumber = int.Parse(lastDigits) + 1;
                                                    //newReportID = newReportID.Substring(0, newReportID.Length - Convert.ToInt32(idFormat.SequentialNumber)) + nextSequentialNumber.ToString().PadLeft(Convert.ToInt32(idFormat.SequentialNumber), '0');
                                                }
                                                else
                                                {
                                                    strReportIDT = strReportIDT + new string('0', Convert.ToInt32(idFormat.SequentialNumber - 1)) + "1";
                                                    //latestReport.NewReportFormat = true;
                                                    //strReportIDT += "001";
                                                }
                                            }
                                        }
                                        ObjectSpace.CommitChanges();
                                    }
                                    else
                                    {
                                        SelectedData sproc = currentSession.ExecuteSproc("ReportingV5_CreateNewID_SP");
                                        strReportIDT = sproc.ResultSet[0].Rows[0].Values[0].ToString();
                                    }
                                }
                                ObjReportingInfo.struqSampleParameterID = uqOid;
                                ObjReportingInfo.strJobID = JobID;
                                ObjReportingInfo.strSampleID = SampleID;
                                ObjReportingInfo.strLimsReportedDate = DateTime.Now.ToString("MM/dd/yyyy");
                                ObjReportingInfo.strQcBatchID = QcBatchID;
                                ObjReportingInfo.strReportID = strReportIDT;
                                ObjReportingInfo.strTestMethodID = TestMethodID;
                                ObjReportingInfo.strParameterID = ParameterID;
                                ObjReportingInfo.strviewid = View.Id.ToString();

                                GlobalReportSourceCode.strTestMethodID = TestMethodID;
                                GlobalReportSourceCode.strParameterID = ParameterID;
                                DynamicDesigner.GlobalReportSourceCode.struqQCBatchID = QcBatchID;
                                DynamicDesigner.GlobalReportSourceCode.strJobID = JobID;
                                List<string> listPage = new List<string>();
                                int pagenumber = 0;
                                using (MemoryStream newms = new MemoryStream())
                                {
                                    if (objrep != null && objrep.Count > 0)
                                    {
                                        var sortobj = objrep.OrderBy(x => x.sort);
                                        foreach (ReportPackage report in sortobj.Where(i => i.ReportName != null))
                                        {
                                            XtraReport tempxtraReport = new XtraReport();
                                            bool IsReportExist = false;
                                            SelectedData sprocCheckReport = currentSession.ExecuteSproc("CheckReportExists", report.ReportName);
                                            if (sprocCheckReport.ResultSet != null && sprocCheckReport.ResultSet[1] != null && sprocCheckReport.ResultSet[1].Rows[0] != null && sprocCheckReport.ResultSet[1].Rows[0].Values[0] != null)
                                            {
                                                IsReportExist = Convert.ToBoolean(sprocCheckReport.ResultSet[1].Rows[0].Values[0]);
                                            }

                                            if (IsReportExist)
                                            {
                                                if (report.ReportName == "REA_003_WithJobID" || report.ReportName == "REA_006_WithJobID" ||
                                                    report.ReportName == "REA_005_2" || report.ReportName == "REA_005" ||
                                                    report.ReportName == "SXJY_006_检验说明" || //report.ReportName == "SXJY_010_注意事项" ||
                                                    report.ReportName == "SXYS_006_检验说明1" || report.ReportName == "SXYS_007_检验说明2" ||
                                                    report.ReportName == "SPYS_007_检验说明2" || report.ReportName == "SPYS_006_检验说明1")
                                                {
                                                    ObjReportingInfo.struqSampleParameterID = string.Empty;
                                                }
                                                else if (report.ReportName == "SXJY_008_净水产水率试验")
                                                {
                                                    ObjReportingInfo.strSampleID = string.Empty;
                                                }
                                                else
                                                {
                                                    ObjReportingInfo.struqSampleParameterID = uqOid;
                                                    ObjReportingInfo.strSampleID = SampleID;
                                                }
                                                if (report.ReportName == "xrEnvTRRPQCComborptnew")
                                                {
                                                    GlobalReportSourceCode.dsQCDataSource = DynamicReportBusinessLayer.BLCommon.GetQcComboReportTRRp_DataSet("Env_QCPotraitRegular_RPT_SP", ObjReportingInfo.strSampleID, ObjReportingInfo.struqSampleParameterID, ObjReportingInfo.strTestMethodID, ObjReportingInfo.strParameterID);
                                                }
                                                tempxtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(report.ReportName.ToString(), ObjReportingInfo, false);
                                                DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(tempxtraReport, ObjReportingInfo);
                                                tempxtraReport.CreateDocument();
                                                for (int i = 1; i <= tempxtraReport.Pages.Count; i++)
                                                {
                                                    if (report.PageDisplay == true && report.PageCount == true)
                                                    {
                                                        pagenumber += 1;
                                                        listPage.Add(pagenumber.ToString());
                                                    }
                                                    else if (report.PageCount == true)
                                                    {
                                                        pagenumber += 1;
                                                        listPage.Add("");
                                                    }
                                                    else
                                                    {
                                                        listPage.Add("");
                                                    }
                                                }
                                                xtraReport.Pages.AddRange(tempxtraReport.Pages);
                                            }
                                        }
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            xtraReport.ExportToPdf(ms);
                                            using (PdfDocumentProcessor source = new PdfDocumentProcessor())
                                            {
                                                source.LoadDocument(ms);
                                                foreach (DevExpress.Pdf.PdfPage page in source.Document.Pages)
                                                {
                                                    var curpageval = listPage[source.Document.Pages.IndexOf(page)];
                                                    if (curpageval.Length > 0)
                                                    {
                                                        using (DevExpress.Pdf.PdfGraphics graphics = source.CreateGraphics())
                                                        {
                                                            DevExpress.Pdf.PdfRectangle rectangle = page.MediaBox;
                                                            RectangleF r = new RectangleF((float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);
                                                            SolidBrush black = (SolidBrush)Brushes.Black;
                                                            using (Font font = new Font("Microsoft Yahei", 11, FontStyle.Regular))
                                                            {
                                                                string text;
                                                                if (objLanguage.strcurlanguage == "En")
                                                                {
                                                                    text = "Total " + pagenumber + " of " + curpageval + " page";
                                                                }
                                                                else
                                                                {
                                                                    text = "共 " + pagenumber + " 页 第 " + curpageval + " 页";
                                                                }
                                                                graphics.DrawString(text, font, black, r.Width + 48, 170);
                                                            }
                                                            graphics.AddToPageForeground(page);
                                                        }
                                                    }
                                                }
                                                source.SaveDocument(newms);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string strReport = getObjectsOfDWQRReport.ReportName;
                                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(strReport, ObjReportingInfo, false);
                                        DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport, ObjReportingInfo);
                                        xtraReport.ExportToPdf(newms);
                                    }
                                    newms.Position = 0;
                                    if (boolReportSave == true)
                                    {
                                        Sampleinfo.bytevalues = newms.ToArray();
                                        boolReportSave = false;
                                    }
                                    else
                                    {
                                        MemoryStream tempms = new MemoryStream();
                                        NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                                        PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                                        string WatermarkText;
                                        if (objLanguage.strcurlanguage == "En")
                                        {
                                            WatermarkText = "UnApproved";
                                        }
                                        else
                                        {
                                            WatermarkText = ConfigurationManager.AppSettings["ReportWaterMarkText"];
                                        }
                                        using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
                                        {
                                            string fontName = "Microsoft Yahei";
                                            int fontSize = 25;
                                            PdfStringFormat stringFormat = PdfStringFormat.GenericTypographic;
                                            stringFormat.Alignment = PdfStringAlignment.Center;
                                            stringFormat.LineAlignment = PdfStringAlignment.Center;
                                            documentProcessor.LoadDocument(newms);
                                            using (SolidBrush brush = new SolidBrush(Color.FromArgb(63, Color.Black)))
                                            {
                                                using (Font font = new Font(fontName, fontSize))
                                                {
                                                    foreach (var page in documentProcessor.Document.Pages)
                                                    {
                                                        var watermarkSize = page.CropBox.Width * 0.75;
                                                        using (DevExpress.Pdf.PdfGraphics graphics = documentProcessor.CreateGraphics())
                                                        {
                                                            SizeF stringSize = graphics.MeasureString(WatermarkText, font);
                                                            Single scale = Convert.ToSingle(watermarkSize / stringSize.Width);
                                                            graphics.TranslateTransform(Convert.ToSingle(page.CropBox.Width * 0.5), Convert.ToSingle(page.CropBox.Height * 0.5));
                                                            graphics.RotateTransform(-45);
                                                            graphics.TranslateTransform(Convert.ToSingle(-stringSize.Width * scale * 0.5), Convert.ToSingle(-stringSize.Height * scale * 0.5));
                                                            using (Font actualFont = new Font(fontName, fontSize * scale))
                                                            {
                                                                RectangleF rect = new RectangleF(0, 0, stringSize.Width * scale, stringSize.Height * scale);
                                                                graphics.DrawString(WatermarkText, actualFont, brush, rect, stringFormat);
                                                            }
                                                            graphics.AddToPageForeground(page, 72, 72);
                                                        }
                                                    }
                                                }
                                            }
                                            documentProcessor.SaveDocument(tempms);
                                        }
                                        objToShow.PDFData = tempms.ToArray();
                                        Sampleinfo.bytevalues = objToShow.PDFData;
                                        DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                                        CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                                        ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                                        showViewParameters.Context = TemplateContext.PopupWindow;
                                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                        showViewParameters.CreatedView.Caption = "PDFViewer";
                                        DialogController dc = Application.CreateController<DialogController>();
                                        dc.SaveOnAccept = false;
                                        dc.AcceptAction.Active.SetItemValue("disable", false);
                                        dc.CancelAction.Active.SetItemValue("disable", false);
                                        dc.CloseOnCurrentObjectProcessing = false;
                                        showViewParameters.Controllers.Add(dc);
                                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                    }
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowMultipleJOBID"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                            //objSpace.Dispose();
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

        private void SetConnectionString()
        {
            try
            {
                AppSettingsReader config = new AppSettingsReader();
                string serverType, server, database, user, password;
                string[] connectionstring = ObjReportDesignerInfo.WebConfigConn.Split(';');
                ObjReportDesignerInfo.LDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLUserID = connectionstring[2].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLPassword = connectionstring[3].Split('=').GetValue(1).ToString();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void btn_DWQRPreview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DrinkingWaterQualityReports getObjectsOfDWQRReport = View.CurrentObject as DrinkingWaterQualityReports;
                if (getObjectsOfDWQRReport != null)
                {
                    if (getObjectsOfDWQRReport.DateCollectedFrom > getObjectsOfDWQRReport.DateCollectedTo)
                    {
                        if (getObjectsOfDWQRReport.DateCollectedTo == DateTime.MinValue)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillDateCollectedTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "!DateCollectedFrom>DateCollectedTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            getObjectsOfDWQRReport.DateCollectedFrom = DateTime.MinValue;
                        }

                    }
                    else if (getObjectsOfDWQRReport.DateCollectedFrom == DateTime.MinValue && getObjectsOfDWQRReport.DateCollectedTo == DateTime.MinValue)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillDateCollectedFrom&DateCollectedTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else if (getObjectsOfDWQRReport.DateCollectedFrom == DateTime.MinValue)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillDateCollectedFrom"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        bool UnApprove = false;
                        List<Guid> oidOfTP = new List<Guid>();
                        List<Guid> oidOfSampleSite = new List<Guid>();
                        DashboardViewItem lstPorpGetParamsDWQR = ((DetailView)Application.MainWindow.View).FindItem("DWQRParameters_Grid") as DashboardViewItem;
                        DashboardViewItem lstPorpGetSampleSiteDWQR = ((DetailView)Application.MainWindow.View).FindItem("DWQRSampleLocation_Grid") as DashboardViewItem;
                        if (lstPorpGetParamsDWQR != null && lstPorpGetParamsDWQR.InnerView != null && lstPorpGetSampleSiteDWQR != null && lstPorpGetSampleSiteDWQR.InnerView != null)
                        {
                            oidOfTP = ((ListView)lstPorpGetParamsDWQR.InnerView).CollectionSource.List.Cast<Testparameter>().Select(tp => tp.Oid).ToList();
                            oidOfSampleSite = ((ListView)lstPorpGetSampleSiteDWQR.InnerView).CollectionSource.List.Cast<SampleSites>().Select(ss => ss.Oid).ToList();
                        }
                        //IList<SampleParameter> listSP = objectSpace.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[Status] = 'PendingReporting' And [Testparameter.Oid] In(" + string.Format("'{0}'", string.Join("','", oidOfTP)) + ")"), CriteriaOperator.Parse("[Samplelogin.SiteName.Oid] In(" + string.Format("'{0}'", string.Join("','", oidOfSampleSite)) + ")"), (CriteriaOperator.Parse("GETDATE([Samplelogin.CollectDate]) BETWEEN('" + getObjectsOfDWQRReport.DateCollectedFrom + "', '" + getObjectsOfDWQRReport.DateCollectedTo + "')"))));
                        IList<SampleParameter> listSP = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status] = 'PendingReporting' And [Testparameter.Oid] In(" + string.Format("'{0}'", string.Join("','", oidOfTP)) + ") And [Samplelogin.StationLocation.Oid] In(" + string.Format("'{0}'", string.Join("','", oidOfSampleSite)) + ") And GETDATE([Samplelogin.CollectDate]) BETWEEN('" + getObjectsOfDWQRReport.DateCollectedFrom + "', '" + getObjectsOfDWQRReport.DateCollectedTo + "')"));

                        if (listSP != null && listSP.Count > 0)
                        {
                            bool IsAllowMultipleJobID = true;
                            List<string> lstreport = new List<string>();
                            IList<ReportPackage> objrep = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?", getObjectsOfDWQRReport.ReportName));
                            foreach (ReportPackage objrp in objrep.ToList())
                            {
                                lstreport.Add(objrp.ReportName);
                            }

                            var jobid = listSP.Select(i => i.Samplelogin.JobID).Distinct().ToList();
                            if (jobid.Count() > 1)
                            {
                                if (lstreport.Count > 0)
                                {
                                    IList<tbl_Public_CustomReportDesignerDetails> objreport = ObjectSpace.GetObjects<tbl_Public_CustomReportDesignerDetails>(new InOperator("colCustomReportDesignerName", lstreport));
                                    List<tbl_Public_CustomReportDesignerDetails> lstAllowjobidcnt = objreport.Where(i => i.AllowMultipleJOBID == false).ToList();
                                    if (lstAllowjobidcnt.Count > 0)
                                    {
                                        IsAllowMultipleJobID = false;
                                    }
                                }
                            }
                            //else if (jobid.Count() < 1)
                            //{
                            //    Application.ShowViewStrategy.ShowMessage("There is no data available to preview for the given date range", InformationType.Info, timer.Seconds, InformationPosition.Top);
                            //}
                            if (IsAllowMultipleJobID == true)
                            {
                                uqOid = null;
                                foreach (SampleParameter obj in listSP)
                                {
                                    if (obj.NotReport == false)
                                    {
                                        if (uqOid == null)
                                        {
                                            uqOid = "'" + obj.Oid.ToString() + "'";
                                            JobID = "'" + obj.Samplelogin.JobID.JobID + "'";
                                            SampleID = "'" + obj.Samplelogin.SampleID + "'";
                                            SampleParameterID = "'" + obj.Testparameter.Parameter.Oid.ToString() + "'";
                                            TestMethodID = "'" + obj.Testparameter.TestMethod.Oid.ToString() + "'";
                                            ParameterID = "'" + obj.Testparameter.Parameter.Oid.ToString() + "'";
                                        }
                                        else
                                        {
                                            uqOid = uqOid + ",'" + obj.Oid.ToString() + "'";
                                            if (!JobID.Contains(obj.Samplelogin.JobID.JobID))
                                            {
                                                JobID = JobID + ",'" + obj.Samplelogin.JobID.JobID + "'";
                                            }
                                            if (!SampleID.Contains(obj.Samplelogin.SampleID))
                                            {
                                                SampleID = SampleID + ",'" + obj.Samplelogin.SampleID + "'";
                                            }
                                            if (!SampleParameterID.Contains(obj.Testparameter.Parameter.Oid.ToString()))
                                            {
                                                SampleParameterID = SampleParameterID + ",'" + obj.Testparameter.Parameter.Oid.ToString() + "'";
                                            }
                                            //if (obj.QCBatchID != null && obj.QCBatchID.qcseqdetail != null)
                                            //{
                                            //    if (string.IsNullOrEmpty(QcBatchID) == false && !QcBatchID.Contains(obj.QCBatchID.qcseqdetail.AnalyticalBatchID))
                                            //    {
                                            //        QcBatchID = QcBatchID + ",'" + obj.QCBatchID.qcseqdetail.AnalyticalBatchID + "'";
                                            //    }
                                            //}
                                            if (!TestMethodID.Contains(obj.Testparameter.TestMethod.Oid.ToString()))
                                            {
                                                TestMethodID = TestMethodID + ",'" + obj.Testparameter.TestMethod.Oid.ToString() + "'";
                                            }
                                            if (!ParameterID.Contains(obj.Testparameter.Parameter.Oid.ToString()))
                                            {
                                                ParameterID = ParameterID + ",'" + obj.Testparameter.Parameter.Oid.ToString() + "'";
                                            }
                                        }
                                        if (!SampleID.Contains(obj.Samplelogin.SampleID))
                                        {
                                            SampleID = SampleID + ",'" + obj.Samplelogin.SampleID + "'";
                                        }
                                    }
                                    if (obj.Status == Samplestatus.PendingEntry || obj.Status == Samplestatus.PendingValidation || obj.Status == Samplestatus.PendingApproval)
                                    {
                                        UnApprove = true;
                                    }
                                }

                                XtraReport xtraReport = new XtraReport();
                                ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                                ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                SetConnectionString();
                                DynamicReportBusinessLayer.BLCommon.SetDBConnection(ObjReportDesignerInfo.LDMSQLServerName, ObjReportDesignerInfo.LDMSQLDatabaseName, ObjReportDesignerInfo.LDMSQLUserID, ObjReportDesignerInfo.LDMSQLPassword);
                                IObjectSpace objSpace = Application.CreateObjectSpace();
                                Session currentSession = ((XPObjectSpace)(objSpace)).Session;

                                ObjReportingInfo.struqSampleParameterID = uqOid;
                                ObjReportingInfo.strJobID = JobID;
                                ObjReportingInfo.strSampleID = SampleID;
                                ObjReportingInfo.strLimsReportedDate = DateTime.Now.ToString("MM/dd/yyyy");
                                ObjReportingInfo.strQcBatchID = QcBatchID;
                                ObjReportingInfo.strReportID = strReportIDT;
                                ObjReportingInfo.strTestMethodID = TestMethodID;
                                ObjReportingInfo.strParameterID = ParameterID;
                                ObjReportingInfo.strviewid = View.Id.ToString();

                                GlobalReportSourceCode.strTestMethodID = TestMethodID;
                                GlobalReportSourceCode.strParameterID = ParameterID;
                                DynamicDesigner.GlobalReportSourceCode.struqQCBatchID = QcBatchID;
                                DynamicDesigner.GlobalReportSourceCode.strJobID = JobID;
                                List<string> listPage = new List<string>();
                                int pagenumber = 0;
                                using (MemoryStream newms = new MemoryStream())
                                {
                                    if (objrep != null && objrep.Count > 0)
                                    {
                                        var sortobj = objrep.OrderBy(x => x.sort);
                                        foreach (ReportPackage report in sortobj.Where(i => i.ReportName != null))
                                        {
                                            XtraReport tempxtraReport = new XtraReport();
                                            bool IsReportExist = false;
                                            SelectedData sprocCheckReport = currentSession.ExecuteSproc("CheckReportExists", report.ReportName);
                                            if (sprocCheckReport.ResultSet != null && sprocCheckReport.ResultSet[1] != null && sprocCheckReport.ResultSet[1].Rows[0] != null && sprocCheckReport.ResultSet[1].Rows[0].Values[0] != null)
                                            {
                                                IsReportExist = Convert.ToBoolean(sprocCheckReport.ResultSet[1].Rows[0].Values[0]);
                                            }

                                            if (IsReportExist)
                                            {
                                                if (report.ReportName == "REA_003_WithJobID" || report.ReportName == "REA_006_WithJobID" ||
                                                    report.ReportName == "REA_005_2" || report.ReportName == "REA_005" ||
                                                    report.ReportName == "SXJY_006_检验说明" || //report.ReportName == "SXJY_010_注意事项" ||
                                                    report.ReportName == "SXYS_006_检验说明1" || report.ReportName == "SXYS_007_检验说明2" ||
                                                    report.ReportName == "SPYS_007_检验说明2" || report.ReportName == "SPYS_006_检验说明1")
                                                {
                                                    ObjReportingInfo.struqSampleParameterID = string.Empty;
                                                }
                                                else if (report.ReportName == "SXJY_008_净水产水率试验")
                                                {
                                                    ObjReportingInfo.strSampleID = string.Empty;
                                                }
                                                else
                                                {
                                                    ObjReportingInfo.struqSampleParameterID = uqOid;
                                                    ObjReportingInfo.strSampleID = SampleID;
                                                }
                                                if (report.ReportName == "xrEnvTRRPQCComborptnew")
                                                {
                                                    GlobalReportSourceCode.dsQCDataSource = DynamicReportBusinessLayer.BLCommon.GetQcComboReportTRRp_DataSet("Env_QCPotraitRegular_RPT_SP", ObjReportingInfo.strSampleID, ObjReportingInfo.struqSampleParameterID, ObjReportingInfo.strTestMethodID, ObjReportingInfo.strParameterID);
                                                }
                                                tempxtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(report.ReportName.ToString(), ObjReportingInfo, false);
                                                DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(tempxtraReport, ObjReportingInfo);
                                                tempxtraReport.CreateDocument();
                                                for (int i = 1; i <= tempxtraReport.Pages.Count; i++)
                                                {
                                                    if (report.PageDisplay == true && report.PageCount == true)
                                                    {
                                                        pagenumber += 1;
                                                        listPage.Add(pagenumber.ToString());
                                                    }
                                                    else if (report.PageCount == true)
                                                    {
                                                        pagenumber += 1;
                                                        listPage.Add("");
                                                    }
                                                    else
                                                    {
                                                        listPage.Add("");
                                                    }
                                                }
                                                xtraReport.Pages.AddRange(tempxtraReport.Pages);
                                            }
                                        }
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            xtraReport.ExportToPdf(ms);
                                            using (PdfDocumentProcessor source = new PdfDocumentProcessor())
                                            {
                                                source.LoadDocument(ms);
                                                foreach (DevExpress.Pdf.PdfPage page in source.Document.Pages)
                                                {
                                                    var curpageval = listPage[source.Document.Pages.IndexOf(page)];
                                                    if (curpageval.Length > 0)
                                                    {
                                                        using (DevExpress.Pdf.PdfGraphics graphics = source.CreateGraphics())
                                                        {
                                                            DevExpress.Pdf.PdfRectangle rectangle = page.MediaBox;
                                                            RectangleF r = new RectangleF((float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);
                                                            SolidBrush black = (SolidBrush)Brushes.Black;
                                                            using (Font font = new Font("Microsoft Yahei", 11, FontStyle.Regular))
                                                            {
                                                                string text;
                                                                if (objLanguage.strcurlanguage == "En")
                                                                {
                                                                    text = "Total " + pagenumber + " of " + curpageval + " page";
                                                                }
                                                                else
                                                                {
                                                                    text = "共 " + pagenumber + " 页 第 " + curpageval + " 页";
                                                                }
                                                                graphics.DrawString(text, font, black, r.Width + 48, 170);
                                                            }
                                                            graphics.AddToPageForeground(page);
                                                        }
                                                    }
                                                }
                                                source.SaveDocument(newms);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string strReport = getObjectsOfDWQRReport.ReportName;
                                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(strReport, ObjReportingInfo, false);
                                        DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport, ObjReportingInfo);
                                        xtraReport.ExportToPdf(newms);
                                    }
                                    newms.Position = 0;
                                    if (boolReportSave == true)
                                    {
                                        Sampleinfo.bytevalues = newms.ToArray();
                                        boolReportSave = false;

                                        DrinkingWaterQualityReports setStatus = View.CurrentObject as DrinkingWaterQualityReports;
                                    }
                                    else
                                    {
                                        MemoryStream tempms = new MemoryStream();
                                        NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                                        PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                                        string WatermarkText;
                                        if (objLanguage.strcurlanguage == "En")
                                        {
                                            WatermarkText = "UnApproved";
                                        }
                                        else
                                        {
                                            WatermarkText = ConfigurationManager.AppSettings["ReportWaterMarkText"];
                                        }
                                        using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
                                        {
                                            string fontName = "Microsoft Yahei";
                                            int fontSize = 25;
                                            PdfStringFormat stringFormat = PdfStringFormat.GenericTypographic;
                                            stringFormat.Alignment = PdfStringAlignment.Center;
                                            stringFormat.LineAlignment = PdfStringAlignment.Center;
                                            documentProcessor.LoadDocument(newms);
                                            using (SolidBrush brush = new SolidBrush(Color.FromArgb(63, Color.Black)))
                                            {
                                                using (Font font = new Font(fontName, fontSize))
                                                {
                                                    foreach (var page in documentProcessor.Document.Pages)
                                                    {
                                                        var watermarkSize = page.CropBox.Width * 0.75;
                                                        using (DevExpress.Pdf.PdfGraphics graphics = documentProcessor.CreateGraphics())
                                                        {
                                                            SizeF stringSize = graphics.MeasureString(WatermarkText, font);
                                                            Single scale = Convert.ToSingle(watermarkSize / stringSize.Width);
                                                            graphics.TranslateTransform(Convert.ToSingle(page.CropBox.Width * 0.5), Convert.ToSingle(page.CropBox.Height * 0.5));
                                                            graphics.RotateTransform(-45);
                                                            graphics.TranslateTransform(Convert.ToSingle(-stringSize.Width * scale * 0.5), Convert.ToSingle(-stringSize.Height * scale * 0.5));
                                                            using (Font actualFont = new Font(fontName, fontSize * scale))
                                                            {
                                                                RectangleF rect = new RectangleF(0, 0, stringSize.Width * scale, stringSize.Height * scale);
                                                                graphics.DrawString(WatermarkText, actualFont, brush, rect, stringFormat);
                                                            }
                                                            graphics.AddToPageForeground(page, 72, 72);
                                                        }
                                                    }
                                                }
                                            }
                                            documentProcessor.SaveDocument(tempms);
                                        }
                                        objToShow.PDFData = tempms.ToArray();
                                        Sampleinfo.bytevalues = objToShow.PDFData;
                                        DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                                        CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                                        ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                                        showViewParameters.Context = TemplateContext.PopupWindow;
                                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                        showViewParameters.CreatedView.Caption = "PDFViewer";
                                        DialogController dc = Application.CreateController<DialogController>();
                                        dc.SaveOnAccept = false;
                                        dc.AcceptAction.Active.SetItemValue("disable", false);
                                        dc.CancelAction.Active.SetItemValue("disable", false);
                                        dc.CloseOnCurrentObjectProcessing = false;
                                        showViewParameters.Controllers.Add(dc);
                                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                    }
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowMultipleJOBID"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                            //objSpace.Dispose();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "NoDataAvailable"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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
        private void btn_DWQRSubmit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "DrinkingWaterQualityReports_DetailView")
                {
                    DrinkingWaterQualityReports objPendingReports = View.CurrentObject as DrinkingWaterQualityReports;
                    List<Guid> oidOfTP = new List<Guid>();
                    List<Guid> oidOfSampleSite = new List<Guid>();
                    DashboardViewItem lstPorpGetParamsDWQR = ((DetailView)Application.MainWindow.View).FindItem("DWQRParameters_Grid") as DashboardViewItem;
                    DashboardViewItem lstPorpGetSampleSiteDWQR = ((DetailView)Application.MainWindow.View).FindItem("DWQRSampleLocation_Grid") as DashboardViewItem;
                    if (lstPorpGetParamsDWQR != null && lstPorpGetParamsDWQR.InnerView != null && lstPorpGetSampleSiteDWQR != null && lstPorpGetSampleSiteDWQR.InnerView != null)
                    {
                        oidOfTP = ((ListView)lstPorpGetParamsDWQR.InnerView).CollectionSource.List.Cast<Testparameter>().Select(tp => tp.Oid).ToList();
                        oidOfSampleSite = ((ListView)lstPorpGetSampleSiteDWQR.InnerView).CollectionSource.List.Cast<SampleSites>().Select(ss => ss.Oid).ToList();
                    }
                    IList<SampleParameter> listSP = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status] = 'PendingReporting' And [Testparameter.Oid] In(" + string.Format("'{0}'", string.Join("','", oidOfTP)) + ") And [Samplelogin.StationLocation.Oid] In(" + string.Format("'{0}'", string.Join("','", oidOfSampleSite)) + ") And GETDATE([Samplelogin.CollectDate]) BETWEEN('" + objPendingReports.DateCollectedFrom + "', '" + objPendingReports.DateCollectedTo + "')"));

                    if (objPendingReports != null)
                    {
                        if (objPendingReports.DateCreated != DateTime.MinValue && objPendingReports.ReportCreatedBy != null && objPendingReports.Status == DWQRStatus.PendingSubmission)
                        {
                            boolReportSave = true;
                            btn_DWQRPreview_Execute(new object(), new SimpleActionExecuteEventArgs(null, null));
                            //foreach (SampleParameter setStatusOfCurrentObject in listSP)
                            //{
                            //    getObjectsOfDWQRReport.DWQRSampleParameter.Add(setStatusOfCurrentObject);                                
                            //}
                            listSP.ForEach(setStatusOfCurrentObject => objPendingReports.DWQRSampleParameter.Add(setStatusOfCurrentObject));
                            objPendingReports.DWQRFileContent = Sampleinfo.bytevalues;
                            objPendingReports.Status = DWQRStatus.PendingApproval;
                            objPendingReports.DWQRSampleParameter.ForEach(SP => SP.Status = Samplestatus.Reported);
                        }
                    }
                    ObjectSpace.CommitChanges();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "ReportSubmitted"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    View.Close();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void btn_DWQRApprove_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "DrinkingWaterQualityReports_DetailView_PendingApproval")
                {
                    DrinkingWaterQualityReports itemToApprove = View.CurrentObject as DrinkingWaterQualityReports;

                    if (itemToApprove != null)
                    {
                        if (itemToApprove.Status == DWQRStatus.PendingApproval && itemToApprove.DateApproved == DateTime.MinValue && itemToApprove.ApprovedBy == null)
                        {
                            itemToApprove.DateApproved = DateTime.Now;
                            itemToApprove.ApprovedBy = View.ObjectSpace.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                            itemToApprove.Status = DWQRStatus.PendingDelivery;
                        }
                    }
                    ObjectSpace.CommitChanges();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "ReportApproved"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    View.Close();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void btn_DWQRPreviewToApprove_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DrinkingWaterQualityReports objCurrentReport = (DrinkingWaterQualityReports)e.CurrentObject;
                //ShowReport(objCurrentReport.ReportID);
                string WatermarkText;
                if (objCurrentReport.Status == DWQRStatus.PendingApproval)
                {
                    if (objLanguage.strcurlanguage == "En")
                    {
                        WatermarkText = "UnApproved";
                    }
                    else
                    {
                        WatermarkText = ConfigurationManager.AppSettings["ReportWaterMarkText"];
                    }
                }
                MemoryStream ms = new MemoryStream(objCurrentReport.DWQRFileContent);
                NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                objToShow.ReportID = objCurrentReport.ReportID;
                objToShow.PDFData = objCurrentReport.DWQRFileContent;
                objToShow.ViewID = View.Id;
                DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.AcceptAction.Active.SetItemValue("disable", false);
                dc.CancelAction.Active.SetItemValue("disable", false);
                dc.CloseOnCurrentObjectProcessing = false;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void btn_DWQRRollBack_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "DrinkingWaterQualityReports_DetailView_PendingApproval" || View.Id == "DrinkingWaterQualityReports_DetailView_PendingDelivery")
                {
                    DrinkingWaterQualityReports itemToRollBack = View.CurrentObject as DrinkingWaterQualityReports;

                    if (itemToRollBack != null)
                    {
                        if (itemToRollBack.Status == DWQRStatus.PendingApproval || itemToRollBack.Status == DWQRStatus.PendingDelivery)
                        {
                            itemToRollBack.DateApproved = DateTime.MinValue;
                            itemToRollBack.ApprovedBy = null;
                            itemToRollBack.DateCollectedFrom = DateTime.MinValue;
                            itemToRollBack.DateCollectedTo = DateTime.MinValue;
                            itemToRollBack.Status = DWQRStatus.PendingSubmission;
                            itemToRollBack.DWQRSampleParameter.ForEach(sp => sp.Status = Samplestatus.PendingReporting);
                        }
                    }
                    ObjectSpace.CommitChanges();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "RolledBack"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    View.Close();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void btn_DWQRHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //Frame.SetView(Application.CreateListView(typeof(SamplePrepBatch), true));
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objectSpace, typeof(DrinkingWaterQualityReports));
                ListView listview = Application.CreateListView("DrinkingWaterQualityReports_ListView_Delivered", cs, true);
                Frame.SetView(listview);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void btn_DWQRDeliverReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DrinkingWaterQualityReports getCurrentObjects = View.CurrentObject as DrinkingWaterQualityReports;
                SmtpClient sc = new SmtpClient();
                string strlogUsername = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).UserName;
                string strlogpassword = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).Password;
                string strSmtpHost = "Smtp.gmail.com";
                string strMailFromUserName = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromUserName"];
                string strMailFromPassword = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromPassword"];
                string strBody;

                byte[] pdfContent = RetrievePDFContent();
                string[] sendEmailTo = GetListOfRecipients();

                if (sendEmailTo != null)
                {
                    using (MemoryStream memoryStream = new MemoryStream(pdfContent))
                    {
                        MailMessage message = new MailMessage(strMailFromUserName, string.Join(",", sendEmailTo));
                        message.IsBodyHtml = true;
                        //message.Subject = getCurrentObjects.ReportName + "Report";
                        message.Subject = "Report : " + getCurrentObjects.ReportName;
                        //message.Body = "Testing report";
                        message.Body = "";
                        strBody = "Hello, <br> <br> Attached @TEMPLATENAME report with this email is from Alpaca LIMS. <br> <br> Created Date - @CREATEDDATE <br> Created By - @CREATEDBY";

                        if (strBody.ToUpper().Contains("@TEMPLATENAME") && getCurrentObjects.TemplateName != null)
                        {
                            strBody = strBody.Replace("@TEMPLATENAME", getCurrentObjects.TemplateName.TemplateName);
                        }
                        if (strBody.ToUpper().Contains("@CREATEDDATE") && getCurrentObjects.TemplateName != null && getCurrentObjects.CreatedDate != DateTime.MinValue)
                        {
                            strBody = strBody.Replace("@CREATEDDATE", getCurrentObjects.CreatedDate.ToString("MM/dd/yyyy"));
                        }
                        if (strBody.ToUpper().Contains("@CREATEDBY") && getCurrentObjects.TemplateName != null && getCurrentObjects.CreatedBy != null)
                        {
                            strBody = strBody.Replace("@CREATEDBY", getCurrentObjects.CreatedBy.ToString());
                        }
                        message.Body = strBody;
                        message.Attachments.Add(new System.Net.Mail.Attachment(memoryStream, getCurrentObjects.ReportName + ".pdf", "application/pdf"));
                        sc.EnableSsl = true;
                        sc.UseDefaultCredentials = false;
                        NetworkCredential credential = new NetworkCredential();
                        credential.UserName = strMailFromUserName;
                        credential.Password = strMailFromPassword;
                        sc.Credentials = credential;
                        sc.Host = strSmtpHost;
                        sc.Port = Convert.ToInt32(((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailPort"]);
                        //sc.Port = 587;
                        try
                        {
                            if (message.To != null && message.To.Count > 0)
                            {
                                sc.Send(message);
                                if (getCurrentObjects.Status == DWQRStatus.PendingDelivery && getCurrentObjects.DateDelivered == DateTime.MinValue && getCurrentObjects.DeliveredBy == null)
                                {
                                    getCurrentObjects.DateDelivered = DateTime.Now;
                                    getCurrentObjects.DeliveredBy = View.ObjectSpace.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                                    getCurrentObjects.Status = DWQRStatus.Delivered;
                                }
                                ObjectSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "ReportDeliveredViaEmail"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                View.Close();
                            }
                        }
                        catch (SmtpFailedRecipientsException ex)
                        {
                            for (int i = 0; i < ex.InnerExceptions.Length; i++)
                            {
                                SmtpStatusCode exstatus = ex.InnerExceptions[i].StatusCode;
                                if (exstatus == SmtpStatusCode.GeneralFailure || exstatus == SmtpStatusCode.ServiceNotAvailable || exstatus == SmtpStatusCode.SyntaxError || exstatus == SmtpStatusCode.SystemStatus || exstatus == SmtpStatusCode.TransactionFailed)
                                {
                                    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(ex.InnerExceptions[i].FailedRecipient, InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
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

        private string[] GetListOfRecipients()
        {
            try
            {
                string[] recipientsArray = null;
                DrinkingWaterQualityReports getRecipients = View.CurrentObject as DrinkingWaterQualityReports;
                if (getRecipients != null && getRecipients.TemplateName != null && !string.IsNullOrEmpty(getRecipients.EmailTo))
                {
                    string recipientsString = getRecipients.EmailTo;
                    recipientsString = recipientsString.Replace(" ", "");
                    recipientsArray = recipientsString.Split(';');
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "NoRecipientSelected"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
                return recipientsArray;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        private byte[] RetrievePDFContent()
        {
            try
            {
                DrinkingWaterQualityReports pdfToRetrieve = View.CurrentObject as DrinkingWaterQualityReports;
                return pdfToRetrieve.DWQRFileContent;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
    }
}
