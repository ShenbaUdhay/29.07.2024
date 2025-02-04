﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Pdf;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.XtraReports.UI;
using E_RoundOff;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.QC;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.PermitLibraries;
using Modules.BusinessObjects.Setting.PLM;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using XCRM.Module.BusinessObjects;
using DataColumn = System.Data.DataColumn;
using DataTable = System.Data.DataTable;
using ListView = DevExpress.ExpressApp.ListView;

namespace BTLIMS.Module.Controllers.ResultEntry
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ResultEntryViewController : ViewController, IXafCallbackHandler
    {
        TimeZoneinfo objTimeinfo = new TimeZoneinfo();
        viewInfo tempviewinfo = new viewInfo();
        spreadsheetitemsltno itemsltno = new spreadsheetitemsltno();
        ResultEntrySelectionInfo resultentryinfo = new ResultEntrySelectionInfo();
        private WebExportController webExportController;
        MessageTimer timer = new MessageTimer();
        Roundoff Roundoff = new Roundoff();
        ResultRollback resultRollback = new ResultRollback();
        ShowNavigationItemController ShowNavigationController;
        SampleLogInInfo objSLInfo = new SampleLogInInfo();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        ResultEntryQueryPanelInfo objREInfo = new ResultEntryQueryPanelInfo();
        QCResultValidationQueryPanelInfo objQPInfo = new QCResultValidationQueryPanelInfo();
        DynamicReportDesignerConnection ObjReportDesignerInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        ICallbackManagerHolder ResultCBM;
        IList<SampleParameter> LstSampleParameters;
        int intselectedcount;
        int inttotalcount;
        string strNumericresults = string.Empty;
        string strresult = string.Empty;
        string strunits = string.Empty;
        string strrptlimit = string.Empty;
        string strMDL = string.Empty;
        string strrec = string.Empty;
        string strrpd = string.Empty;
        string strdf = string.Empty;
        string strqcbatchid = string.Empty;
        string strsyssamplecode = string.Empty;
        string strtest = string.Empty;
        string strmethod = string.Empty;
        string strmatrix = string.Empty;
        string strparameter = string.Empty;
        string strSpikeAmountunit = string.Empty;
        double SpikeAmount = 0;
        public string strFTPServerName = string.Empty;
        public string strFTPPath = string.Empty;
        public string strFTPUserName = string.Empty;
        public string strFTPPassword = string.Empty;
        public int FTPPort = 0;
        public bool strFTPStatus;
        bool boolReportSave = false;
        private SingleChoiceAction cmbReportName;
        string strReportID = string.Empty;
        private string uqOid;
        private string JobID;
        private string SampleID;
        private string QcBatchID;
        bool IsSetPageSize = false;
        public XtraReport xrReport = new XtraReport();
        curlanguage objLanguage = new curlanguage();


        #region Constructor
        public ResultEntryViewController()
        {
            InitializeComponent();
            this.TargetViewId = "SampleParameter_ListView_Copy_ResultValidation;" + "SampleParameter_ListView_Copy_ResultApproval;" + "SampleParameter_ListView_Copy_ResultEntry;" +
                "SampleParameter_ListView_Copy_ResultView;" + "Result_Validation;" + "Result_Approval;" + "SampleParameter_ListView_Copy_QCResultView;" +
                "SampleParameter_ListView_Copy_ResultEntry_Main;" + "SampleParameter_ListView_Copy_QCResultEntry;" + "SampleParameter_ListView_Copy_QCResultValidation;"
                 + "SampleParameter_ListView_Copy_QCResultApproval;" + "ResultEntry_Validation;" + "ResultEntry_Approval;" + "ResultEntry_Enter;"
                 + "SampleParameter_ListView_Copy_QCResultValidation_Level1Review;" + "SampleParameter_ListView_Copy_QCResultApproval_Level2Review;"
                 + "ResultEntryQueryPanel_DetailView_Copy;" +/* "Samplecheckin_ListView_Copy_Registration;" +*/ "ResultEntry_Validation_View;" + "ResultEntry_Approval_View;"
                 + "ResultDefaultValue_LookupListView_ResultEntry;" + "SampleParameter_ListView_Copy_ResultEntry_SingleChoice;" + "SampleParameter_ListView_Copy_ResultView_SingleChoice;";
            Rollback.TargetViewId = "SampleParameter_ListView_Copy_ResultView;" + "SampleParameter_ListView_Copy_QCResultView;" +
                 "ResultEntry_Validation;" + "ResultEntry_Approval;" + "ResultEntry_Validation_View;" + "ResultEntry_Approval_View;";
            //+ "SampleParameter_ListView_Copy_ResultValidation;" + "SampleParameter_ListView_Copy_QCResultValidation;" 
            //+ "SampleParameter_ListView_Copy_ResultApproval;" + "SampleParameter_ListView_Copy_QCResultApproval;";
            SampleRegistration.TargetViewId = "SampleParameter_ListView_Copy_ResultEntry;";
            //ResultValidation.TargetViewId = "SampleParameter_ListView_Copy_ResultValidation;" + "SampleParameter_ListView_Copy_QCResultValidation;";
            //ResultApproval.TargetViewId = "SampleParameter_ListView_Copy_ResultApproval;" + "SampleParameter_ListView_Copy_QCResultApproval;";
            ResultValidation.TargetViewId = "ResultEntry_Validation;";
            ResultApproval.TargetViewId = "ResultEntry_Approval;";
            //ResultEnter.TargetViewId = "SampleParameter_ListView_Copy_ResultEntry;" + "SampleParameter_ListView_Copy_QCResultEntry;";
            //ResultEnter.TargetViewId ="SampleParameter_ListView_Copy_ResultEntry_SingleChoice;";
            //ResultDelete.TargetViewId = "SampleParameter_ListView_Copy_ResultEntry;" + "SampleParameter_ListView_Copy_QCResultEntry;";
            ResultDelete.TargetViewId = "ResultEntry_Enter;" + "SampleParameter_ListView_Copy_ResultEntry_SingleChoice;";
            //ResultSubmit.TargetViewId = "SampleParameter_ListView_Copy_ResultEntry;" + "SampleParameter_ListView_Copy_QCResultEntry;";
            ResultSubmit.TargetViewId = "ResultEntry_Enter;" + "SampleParameter_ListView_Copy_ResultEntry_SingleChoice;";
            //Rollback.TargetViewId = "Result_Validation;" + "Result_Approval;" + "Result_View;";
            //RegisterActions(components);

            ExportResulyEntry.TargetViewId = "ResultEntry_Enter;" + "SampleParameter_ListView_Copy_ResultEntry_SingleChoice;";
            ImportResultentry.TargetViewId = "ResultEntry_Enter;" + "SampleParameter_ListView_Copy_ResultEntry_SingleChoice;";
            string jScript = @"
                        for (var i = 0 ; i <= Grid.GetVisibleRowsOnPage() - 1; i++) { 
                            if (Grid.IsRowSelectedOnPage(i)) {
                        Grid.batchEditApi.ResetChanges(i);
                         }
                        }
                       ";
            ResultDelete.SetClientScript(jScript);
        }

        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View.Id == "ResultEntryQueryPanel_DetailView_Copy" || View.Id == "ResultEntryQueryPanel_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Active.SetItemValue("save", false);
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("saveclose", false);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("savenew", false);
                    Frame.GetController<ModificationsController>().CancelAction.Active.SetItemValue("cancel", false);
                }
                ExportResulyEntry.Active["Eport"] = false;
                ImportResultentry.Active["Import"] = false;
                if (resultentryinfo.lstresultentry == null)
                {
                    resultentryinfo.lstresultentry = new List<SampleParameter>();
                }
                else
                {
                    resultentryinfo.lstresultentry.Clear();
                }
                Employee currentUser = SecuritySystem.CurrentUser as Employee;
                if (currentUser != null && View != null && View.Id != null)
                {

                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.ResultEntryIsWrite = false;
                        objPermissionInfo.ResultEntryIsDelete = false;
                        objPermissionInfo.ResultViewIsWrite = false;
                        objPermissionInfo.ResultViewIsDelete = false;
                        objPermissionInfo.ResultValidationIsWrite = false;
                        objPermissionInfo.ResultValidationIsDelete = false;
                        objPermissionInfo.ResultApprovalIsWrite = false;
                        objPermissionInfo.ResultApprovalIsDelete = false;
                        if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                        {
                            if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                            {
                                objPermissionInfo.ResultEntryIsWrite = true;
                                objPermissionInfo.ResultEntryIsDelete = true;
                                objPermissionInfo.ResultViewIsWrite = true;
                                objPermissionInfo.ResultViewIsDelete = true;
                                objPermissionInfo.ResultValidationIsWrite = true;
                                objPermissionInfo.ResultValidationIsDelete = true;
                                objPermissionInfo.ResultApprovalIsWrite = true;
                                objPermissionInfo.ResultApprovalIsDelete = true;
                            }
                            else
                            {
                                foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                                {
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Result Entry" && i.Write == true) != null)
                                    {
                                        objPermissionInfo.ResultEntryIsWrite = true;
                                        //return;
                                    }
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Result Entry" && i.Delete == true) != null)
                                    {
                                        objPermissionInfo.ResultEntryIsDelete = true;
                                        //return;
                                    }
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Result Validation" && i.Write == true) != null)
                                    {
                                        objPermissionInfo.ResultValidationIsWrite = true;
                                        //return;
                                    }
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Result Validation" && i.Delete == true) != null)
                                    {
                                        objPermissionInfo.ResultValidationIsDelete = true;
                                        //return;
                                    }
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Result Approval" && i.Write == true) != null)
                                    {
                                        objPermissionInfo.ResultApprovalIsWrite = true;
                                        //return;
                                    }
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Result Approval" && i.Delete == true) != null)
                                    {
                                        objPermissionInfo.ResultApprovalIsDelete = true;
                                        //return;
                                    }
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ResultView" && i.Write == true) != null)
                                    {
                                        objPermissionInfo.ResultViewIsWrite = true;
                                        //return;
                                    }
                                    //if (objPermissionInfo.ResultEntryIsWrite == true && objPermissionInfo.ResultEntryIsDelete == true)
                                    //{
                                    //    return;
                                    //}
                                }
                            }
                        }
                    }
                    if (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry")
                    {
                        //ResultEnter.Active.SetItemValue("ShowResultEntryEnter", objPermissionInfo.ResultEntryIsWrite);
                        ResultSubmit.Active.SetItemValue("ShowResultEntrySubmit", objPermissionInfo.ResultEntryIsWrite);
                        ResultDelete.Active.SetItemValue("ShowResultEntryDelete", objPermissionInfo.ResultEntryIsDelete);
                    }
                    else
                    if (View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation"
                        || View.Id == "ResultEntry_Validation" || View.Id == "ResultEntry_Validation_View"
                        || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review")
                    {
                        ResultValidation.Active.SetItemValue("ShowResultValidateResultValidation", objPermissionInfo.ResultValidationIsWrite);
                        Rollback.Active.SetItemValue("ShowResultValidateRollBack", objPermissionInfo.ResultValidationIsWrite);
                    }
                    else
                    if (View.Id == "SampleParameter_ListView_Copy_ResultApproval" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                        || View.Id == "ResultEntry_Approval" || View.Id == "ResultEntry_Approval_View"
                        || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review")
                    {
                        ResultApproval.Active.SetItemValue("ShowResultApproveResultApproval", objPermissionInfo.ResultApprovalIsWrite);
                        Rollback.Active.SetItemValue("ShowResultApproveRollBack", objPermissionInfo.ResultApprovalIsWrite);
                    }
                    else if (View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_QCResultView")
                    {
                        Rollback.Active.SetItemValue("ShowResultEntryEnter", objPermissionInfo.ResultEntryIsWrite);
                        //Rollback.Active.SetItemValue("ShowResultViewRollBack", objPermissionInfo.ResultViewIsWrite);
                    }
                    else if (View.Id == "ResultEntry_Enter" || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice")
                    {
                        ResultSubmit.Active.SetItemValue("ShowResultEntrySubmit", objPermissionInfo.ResultEntryIsWrite);
                        ExportResulyEntry.Active.SetItemValue("ShowResultEntrySubmit", objPermissionInfo.ResultEntryIsWrite);
                        ImportResultentry.Active.SetItemValue("ShowResultEntrySubmit", objPermissionInfo.ResultEntryIsWrite);
                        ResultDelete.Active.SetItemValue("ShowResultEntryDelete", objPermissionInfo.ResultEntryIsDelete);
                    }
                    else if (View.Id == "ResultDefaultValue_LookupListView_ResultEntry")
                    {
                        View.ControlsCreated += View_ControlsCreated;
                    }
                }

                //ResultEnter.Executing += ResultEnter_Executing;
                ResultSubmit.Executing += ResultSubmit_Executing;
                ResultSubmit.Executed += ResultSubmit_Executed;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        private void ResultSubmit_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (resultentryinfo.lstresultentry != null)
                {
                    resultentryinfo.lstresultentry.Clear();
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void View_ControlsCreated(object sender, EventArgs e)
        {
            try
            {
                View.ControlsCreated -= View_ControlsCreated;
                if (View.Id == "ResultDefaultValue_LookupListView_ResultEntry")
                {
                    DashboardViewItem dvSampleparam = ((DashboardView)Application.MainWindow.View).FindItem("ResultEntry") as DashboardViewItem;
                    if (dvSampleparam != null && dvSampleparam.InnerView != null)
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        //SampleParameter objTestParam = (SampleParameter)dvSampleparam.InnerView.CurrentObject;
                        SampleParameter objParam = dvSampleparam.InnerView.ObjectSpace.FindObject<SampleParameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (objParam != null && !string.IsNullOrEmpty(objParam.Result))
                        {
                            ResultDefaultValue objVal = View.ObjectSpace.FindObject<ResultDefaultValue>(CriteriaOperator.Parse("[ResultValue]=?", objParam.Result));
                            if (objVal != null)
                            {
                                gridListEditor.Grid.Selection.SelectRowByKey(objVal.Oid);
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

        private void ResultSubmit_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View is ListView)
                {
                    bool boolAnalysis = true;
                    //if (View != null && View.SelectedObjects.Count > 0)
                    if (resultentryinfo.lstresultentry != null && resultentryinfo.lstresultentry.Count > 0)
                    {
                        if (resultentryinfo.lstresultentry.FirstOrDefault(i => i.AnalyzedBy == null && i.AnalyzedDate == null) != null)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "analysisempty"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            boolAnalysis = false;
                            e.Cancel = true;
                            return;
                        }
                        else if (resultentryinfo.lstresultentry.FirstOrDefault(i => i.AnalyzedBy == null) != null)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "analysisbyempty"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            boolAnalysis = false;
                            e.Cancel = true;
                            return;
                        }
                        else if (resultentryinfo.lstresultentry.FirstOrDefault(i => i.AnalyzedDate == null) != null)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "analysisdateempty"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            boolAnalysis = false;
                            e.Cancel = true;
                            return;
                        }
                        else if (resultentryinfo.lstresultentry.FirstOrDefault(i => i.Result == null) != null)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ResultNull"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            boolAnalysis = false;
                            e.Cancel = true;
                            return;
                        }
                        //foreach (Modules.BusinessObjects.SampleManagement.SampleParameter objSampleResult1 in View.SelectedObjects)
                        //foreach (Modules.BusinessObjects.SampleManagement.SampleParameter objSampleResult1 in resultentryinfo.lstresultentry.ToList())
                        //{
                        //    if (objSampleResult1.AnalyzedDate == null && objSampleResult1.AnalyzedBy == null)
                        //    {
                        //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "analysisempty"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //        boolAnalysis = false;
                        //        break;
                        //    }
                        //    else if (objSampleResult1.AnalyzedDate == null)
                        //    {
                        //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "analysisdateempty"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //        boolAnalysis = false;
                        //        break;
                        //    }
                        //    else if (objSampleResult1.AnalyzedBy == null)
                        //    {
                        //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "analysisbyempty"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //        boolAnalysis = false;
                        //        break;
                        //    }
                        //    else if (string.IsNullOrEmpty(objSampleResult1.Result))
                        //    {
                        //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ResultNull"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //        boolAnalysis = false;
                        //        break;
                        //    }
                        //}
                        //IObjectSpace objspc = Application.CreateObjectSpace();
                        //Samplecheckin samplecheckin = (Samplecheckin)objspc.CreateObject(typeof(Samplecheckin));
                        //SpreadSheetEntry_AnalyticalBatch ABspreadSheet = View.ObjectSpace.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[Jobid]=?", samplecheckin.JobID));

                        IList<SampleParameter> lstsmpl2 = View.ObjectSpace.GetObjects<SampleParameter>(new InOperator("Oid", resultentryinfo.lstresultentry.Select(i => i.Oid)));
                        if (lstsmpl2.FirstOrDefault(i => i.ABID != null) == null)
                        {
                            //if (LstSampleParameters.Any(i => i.UQABID != null))
                            ////{
                            //if (LstSampleParameters.Where(a => a.UQABID.Oid == ).Count() >= 0)
                            //{

                            if (boolAnalysis == true)
                            {
                                UnitOfWork uow = new UnitOfWork(((XPObjectSpace)this.ObjectSpace).Session.DataLayer);
                                ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                                Modules.BusinessObjects.Setting.DefaultSetting DRdefsetting = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [IsModule] = True And [GCRecord] is Null"));
                                Modules.BusinessObjects.Setting.DefaultSetting RVdefsetting = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'Result Validation' And [GCRecord] is Null"));
                                Modules.BusinessObjects.Setting.DefaultSetting RAdefsetting = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'Result Approval' And [GCRecord] is Null"));

                                //IObjectSpace objspc = Application.CreateObjectSpace();
                                IList<SampleParameter> lstsmpl = View.ObjectSpace.GetObjects<SampleParameter>(new InOperator("Oid", resultentryinfo.lstresultentry.Select(i => i.Oid)));
                                //SpreadSheetEntry_AnalyticalBatch ABspreadSheet = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", lstsmpl.));
                                if (lstsmpl != null && lstsmpl.Count > 0)
                                {
                                    if (DRdefsetting.Select == true)
                                    {
                                        if (RVdefsetting.Select == true)
                                        {
                                            lstsmpl.ToList().ForEach(i => { i.Status = Samplestatus.PendingValidation; });
                                            lstsmpl.ToList().ForEach(i => { i.OSSync = true; });
                                        }
                                        else if (RVdefsetting.Select == false && RAdefsetting.Select == true)
                                        {
                                            lstsmpl.ToList().ForEach(i => { i.Status = Samplestatus.PendingApproval; });
                                            lstsmpl.ToList().ForEach(i => { i.ValidatedDate = i.AnalyzedDate; });
                                            lstsmpl.ToList().ForEach(i => { i.ValidatedBy = i.AnalyzedBy; });
                                        }
                                        else if (RVdefsetting.Select == false && RAdefsetting.Select == false)
                                        {
                                            lstsmpl.ToList().ForEach(i => { i.Status = Samplestatus.PendingReporting; });
                                            lstsmpl.ToList().ForEach(i => { i.ValidatedDate = i.AnalyzedDate; });
                                            lstsmpl.ToList().ForEach(i => { i.ValidatedBy = i.AnalyzedBy; });
                                            lstsmpl.ToList().ForEach(i => { i.AnalyzedDate = i.AnalyzedDate; });
                                            lstsmpl.ToList().ForEach(i => { i.AnalyzedBy = i.AnalyzedBy; });
                                        }
                                    }
                                    else
                                    {
                                        lstsmpl.ToList().ForEach(i => { i.Status = Samplestatus.PendingReporting; });
                                        lstsmpl.ToList().ForEach(i => { i.ValidatedDate = i.AnalyzedDate; });
                                        lstsmpl.ToList().ForEach(i => { i.ValidatedBy = i.AnalyzedBy; });
                                        lstsmpl.ToList().ForEach(i => { i.AnalyzedDate = i.AnalyzedDate; });
                                        lstsmpl.ToList().ForEach(i => { i.AnalyzedBy = i.AnalyzedBy; });
                                    }
                                    foreach (SampleParameter objpara in lstsmpl.Where(i => i.Testparameter != null && i.Testparameter.Parameter != null && i.Testparameter.Parameter.Limit&&i.Result!="BRL"&&! string.IsNullOrEmpty(i.Result)).ToList())
                                    {
                                        IList<Permitsetup> lstPS = View.ObjectSpace.GetObjects<Permitsetup>(CriteriaOperator.Parse("[Parameter]=?", objpara.Testparameter.Parameter));
                                        foreach (Permitsetup objPS in lstPS)
                                        {
                                            if (!ReachedLimit(objpara, objPS))
                                            {
                                                PermitAlert objAlert = View.ObjectSpace.CreateObject<PermitAlert>(); 
                                                objAlert.Subject = "The parameter " + objpara.Testparameter.Parameter.ParameterName + " has reached out of the result Limit " + objPS.PermitLibrary.PermitName + " Limit" + objpara.SysSampleCode;
                                                objAlert.AlarmTime = DateTime.Now;
                                            } 
                                        }
                                        //objAlert.StartDate = DateTime.Now;
                                        //objAlert.RemindIn = TimeSpan.FromHours(4);
                                        //objAlert.User = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("Oid =?", SecuritySystem.CurrentUserId));
                                        //objAlert.Category = NotificationCategory.NotesAlert;


                                    }
                                    //foreach (SampleParameter objSampleResult1 in lstsmpl)
                                    //{
                                    //    if (objSampleResult1 != null && objSampleResult1.QCBatchID != null && objSampleResult1.QCBatchID.QCType != null && objSampleResult1.QCBatchID.QCType.QCTypeName == "Sample")
                                    //    {
                                    //        IList<SampleParameter> lstsmpl1 = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ?", lstsmpl.FirstOrDefault().Samplelogin.JobID.Oid));
                                    //        if (lstsmpl1.Where(i => i.Status == Samplestatus.PendingEntry).Count() == 0)
                                    //        {
                                    //            StatusDefinition objStatus = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Index] = '117'"));
                                    //            if (objStatus != null)
                                    //            {
                                    //                lstsmpl.FirstOrDefault().Samplelogin.JobID.Index = objStatus;
                                    //                lstsmpl.FirstOrDefault().Status = Samplestatus.PendingValidation;
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    View.ObjectSpace.CommitChanges();
                                    View.ObjectSpace.Refresh();
                                    bool IsSave = false;
                                    foreach (Samplecheckin objSampleResult1 in lstsmpl.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null).Select(i => i.Samplelogin.JobID).Distinct())
                                    {
                                        IList<SampleParameter> lstsmpl1 = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Testparameter.QCType.QCTypeName] = 'Sample'", objSampleResult1.Oid));
                                        if (lstsmpl1.FirstOrDefault(i => i.Status == Samplestatus.PendingEntry) == null)
                                        {
                                            StatusDefinition objStatus = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 17"));
                                            if (objStatus != null)
                                            {
                                                Samplecheckin objJobID = View.ObjectSpace.GetObjectByKey<Samplecheckin>(objSampleResult1.Oid);
                                                objJobID .Index= objStatus;
                                                IsSave = true;
                                            }
                                        }
                                    }
                                    foreach (SpreadSheetEntry_AnalyticalBatch objQcbatch in lstsmpl.Where(i => i.UQABID != null).Select(i => i.UQABID).Distinct())
                                    {
                                        IList<SampleParameter> lstsmpl1 = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[UQABID.Oid] = ? And [Testparameter.QCType.QCTypeName] = 'Sample'", objQcbatch.Oid));
                                        if(lstsmpl1.FirstOrDefault(i=>i.Status== Samplestatus.PendingEntry)==null)
                                        {
                                            SpreadSheetEntry_AnalyticalBatch ABspreadSheet = View.ObjectSpace.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[Oid] = ?", objQcbatch.Oid));
                                            if (ABspreadSheet!=null)
                                            {
                                                ABspreadSheet.Status = 2;
                                                IsSave = true;
                                            }
                                        }
                                    }
                                    if (IsSave)
                                    {
                                        View.ObjectSpace.CommitChanges();
                                        View.ObjectSpace.Refresh();
                                    }
                                }
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "successsubmit"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                //ObjectSpace.Refresh();
                                //ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
                                //ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                                //foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                                //{
                                //    if (parent.Id == "Reporting")
                                //    {
                                //        foreach (ChoiceActionItem child in parent.Items)
                                //        {
                                //            if (child.Id == "Custom Reporting")
                                //            {
                                //                int count = 0;
                                //                IObjectSpace objSpace = Application.CreateObjectSpace();
                                //                using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                                //                {
                                //                    lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingReporting' And [SignOff] = True And Not IsNullOrEmpty([Samplelogin.JobID.JobID])");
                                //                    lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                                //                    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                //                    List<object> jobid = new List<object>();
                                //                    if (lstview != null)
                                //                    {
                                //                        foreach (ViewRecord rec in lstview)
                                //                            jobid.Add(rec["Toid"]);
                                //                    }

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
                                //                break;
                                //            }

                                //        }
                                //        break;
                                //    }
                                //}
                            }
                        }
                        else
                        {
                            string message = "Do not select SDMS progress test."; // Your message here
                            Application.ShowViewStrategy.ShowMessage(message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
                else if (View is DashboardView)
                {
                    int selectedcount = 0;
                    Modules.BusinessObjects.Setting.DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    foreach (DashboardViewItem viewItem in ((DashboardView)View).Items)
                    {
                        bool boolAnalysis = true;
                        if (viewItem.InnerView is ListView && viewItem.InnerView.SelectedObjects.Count > 0)
                        {
                            selectedcount += viewItem.InnerView.SelectedObjects.Count;
                            foreach (SampleParameter objSampleResult1 in viewItem.InnerView.SelectedObjects)
                            {
                                if (objSampleResult1.AnalyzedDate == null && objSampleResult1.AnalyzedBy == null)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "analysisempty"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    boolAnalysis = false;
                                    break;
                                }
                                else if (objSampleResult1.AnalyzedDate == null)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "analysisdateempty"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    boolAnalysis = false;
                                    break;
                                }
                                else if (objSampleResult1.AnalyzedBy == null)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "analysisbyempty"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    boolAnalysis = false;
                                    break;
                                }
                                else if (objSampleResult1.Result == null)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ResultNull"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    boolAnalysis = false;
                                    break;
                                }
                            }
                            if (boolAnalysis == true)
                            {
                                ((ASPxGridListEditor)((ListView)viewItem.InnerView).Editor).Grid.UpdateEdit();
                                Modules.BusinessObjects.Setting.DefaultSetting DRdefsetting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [IsModule] = True And [GCRecord] is Null"));
                                Modules.BusinessObjects.Setting.DefaultSetting RVdefsetting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'Result Validation' And [GCRecord] is Null"));
                                Modules.BusinessObjects.Setting.DefaultSetting RAdefsetting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'Result Approval' And [GCRecord] is Null"));
                                foreach (Modules.BusinessObjects.SampleManagement.SampleParameter objSampleResult in viewItem.InnerView.SelectedObjects)
                                {
                                    //if (setting != null)
                                    {
                                        if (DRdefsetting.Select == true)
                                        {
                                            if (RVdefsetting.Select == true)
                                            {
                                                objSampleResult.Status = Samplestatus.PendingValidation;
                                                objSampleResult.OSSync = true;
                                            }
                                            else if (RVdefsetting.Select == false && RAdefsetting.Select == true)
                                            {
                                                objSampleResult.Status = Samplestatus.PendingApproval;
                                                objSampleResult.OSSync = true;
                                                objSampleResult.ValidatedDate = objSampleResult.AnalyzedDate;
                                                objSampleResult.ValidatedBy = objSampleResult.AnalyzedBy;
                                            }
                                            else if (RVdefsetting.Select == false && RAdefsetting.Select == false)
                                            {
                                                objSampleResult.Status = Samplestatus.PendingReporting;
                                                objSampleResult.OSSync = true;
                                                objSampleResult.ValidatedDate = objSampleResult.AnalyzedDate;
                                                objSampleResult.ValidatedBy = objSampleResult.AnalyzedBy;
                                                objSampleResult.AnalyzedDate = objSampleResult.AnalyzedDate;
                                                objSampleResult.AnalyzedBy = objSampleResult.AnalyzedBy;
                                            }
                                        }
                                        else if (DRdefsetting.Select == false)
                                        {
                                            objSampleResult.Status = Samplestatus.PendingReporting;
                                            objSampleResult.OSSync = true;
                                            objSampleResult.ValidatedDate = objSampleResult.AnalyzedDate;
                                            objSampleResult.ValidatedBy = objSampleResult.AnalyzedBy;
                                            objSampleResult.AnalyzedDate = objSampleResult.AnalyzedDate;
                                            objSampleResult.AnalyzedBy = objSampleResult.AnalyzedBy;
                                        }
                                    }

                                    viewItem.InnerView.ObjectSpace.CommitChanges();
                                }
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "successsubmit"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                viewItem.InnerView.ObjectSpace.Refresh();
                            }
                        }
                    }
                    if (selectedcount == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private bool ReachedLimit(SampleParameter result, Permitsetup limits)
        {
            string limit = limits.Permit;
            string operatorStr = limits.Operator.ToString();
            string resultValue = result.Result;
            switch (operatorStr)
            {
                case "<":
                    if (double.TryParse(limit, out double limitValueLessThan)&& double.TryParse(resultValue, out double NumResultG))
                    {
                        return NumResultG < limitValueLessThan;
                    }
                    else
                    {
                        return true;
                    }
                case "<=":
                    if (double.TryParse(limit, out double limitValueLessThanOrEqual) && double.TryParse(resultValue, out double NumResultGE))
                    {
                        return NumResultGE <= limitValueLessThanOrEqual;
                    }
                    else
                    {
                        return true;
                    }

                case ">":
                    if (double.TryParse(limit, out double limitValueGreaterThan) && double.TryParse(resultValue, out double NumResultL))
                        {
                            return NumResultL > limitValueGreaterThan; 
                        }
                    else
                    {
                        return true;
                    }
                case ">=":
                    if (double.TryParse(limit, out double limitValueGreaterThanOrEqual) && double.TryParse(resultValue, out double NumResultLE))
                    {
                        return NumResultLE >= limitValueGreaterThanOrEqual;
                    }
                    else
                    {
                        return true;
                    }
                case "=":
                        return resultValue == limit;

                case "<>":
                    return resultValue != limit;
                case "Inbetween":
                    string[] limitsArray = limit.Split('-');
                    //if (limitsArray.Length != 2)
                    //{
                    //    throw new ArgumentException("Permit should contain two numbers separated by a hyphen for Inbetween operator.");
                    //}
                    if (double.TryParse(limitsArray[0].Trim(), out double lowerLimit) &&
                        double.TryParse(limitsArray[1].Trim(), out double upperLimit) && double.TryParse(resultValue, out double NumResultB))
                    {
                        return NumResultB >= lowerLimit && NumResultB <= upperLimit;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    return true;
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // For hide Save changes and Cancel Changes
            try
            {
                if (View is ListView)
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                }
                else if (View is DetailView)
                {
                    Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                }
                //ResultCBM = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                //ResultCBM.CallbackManager.RegisterHandler("Rollbackstat", this);
                if (View != null && View is ListView)
                {
                    //ResultEnter.Active.Clear();
                    //ResultSubmit.Active.Clear();
                    //ResultDelete.Active.Clear();
                    //ResultValidation.Active.Clear();
                    //Rollback.Active.Clear();
                    //ResultApproval.Active.Clear();

                    if (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry")
                    {
                        //ResultEnter.Active.SetItemValue("ShowResultEntryEnter", objPermissionInfo.ResultEntryIsWrite);
                        ResultSubmit.Active.SetItemValue("ShowResultEntrySubmit", objPermissionInfo.ResultEntryIsWrite);
                        ResultDelete.Active.SetItemValue("ShowReultEntryDelete", objPermissionInfo.ResultEntryIsDelete);

                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null && ((ListView)View).CollectionSource != null && ((ListView)View).CollectionSource.GetCount() > 0)
                        {
                            if (View.Id == "SampleParameter_ListView_Copy_ResultEntry")
                            {
                                ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_ResultEntry")).PageSize = ((ListView)View).CollectionSource.GetCount();

                            }
                            if (View.Id == "SampleParameter_ListView_Copy_QCResultEntry")
                            {
                                ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_QCResultEntry")).PageSize = ((ListView)View).CollectionSource.GetCount();
                            }
                        }
                    }
                    else
                    if (View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation"
                        || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review")
                    {
                        ResultValidation.Active.SetItemValue("ShowResultValidateResultValidation", objPermissionInfo.ResultValidationIsWrite);
                        Rollback.Active.SetItemValue("ShowResultValidateRollBack", objPermissionInfo.ResultValidationIsWrite);

                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        //editor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        //editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        //editor.Grid.Settings.VerticalScrollableHeight = 300;
                        if (editor != null && editor.Grid != null)
                        {
                            editor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        }
                        if (!IsSetPageSize)
                        {
                            if (editor != null && editor.Grid != null && ((ListView)View).CollectionSource != null && ((ListView)View).CollectionSource.GetCount() > 0)
                            {
                                if (View.Id == "SampleParameter_ListView_Copy_ResultValidation")
                                {
                                    ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_ResultValidation")).PageSize = ((ListView)View).CollectionSource.GetCount();

                                }
                                if (View.Id == "SampleParameter_ListView_Copy_QCResultValidation")
                                {
                                    ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_QCResultValidation")).PageSize = ((ListView)View).CollectionSource.GetCount();
                                }
                                if (View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview")
                                {
                                    ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_ResultValidation_Leve1lReview")).PageSize = ((ListView)View).CollectionSource.GetCount();
                                }
                                if (View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review")
                                {
                                    ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_QCResultValidation_Level1Review")).PageSize = ((ListView)View).CollectionSource.GetCount();
                                }
                            }
                            IsSetPageSize = true;
                        }
                    }
                    else
                    if (View.Id == "SampleParameter_ListView_Copy_ResultApproval" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                        || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review")
                    {
                        ResultApproval.Active.SetItemValue("ShowResultApproveResultApproval", objPermissionInfo.ResultApprovalIsWrite);
                        Rollback.Active.SetItemValue("ResultApprove.RollBack", objPermissionInfo.ResultApprovalIsWrite);

                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            editor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            if (((ListView)View).CollectionSource != null && ((ListView)View).CollectionSource.GetCount() > 0)
                        {
                            if (View.Id == "SampleParameter_ListView_Copy_ResultApproval")
                            {
                                ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_ResultApproval")).PageSize = ((ListView)View).CollectionSource.GetCount();

                            }
                                else if (View.Id == "SampleParameter_ListView_Copy_QCResultApproval")
                            {
                                ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_QCResultApproval")).PageSize = ((ListView)View).CollectionSource.GetCount();
                            }
                                else if (View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review")
                            {
                                ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_ResultApproval_Level2Review")).PageSize = ((ListView)View).CollectionSource.GetCount();
                            }
                                else if (View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review")
                            {
                                ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_QCResultApproval_Level2Review")).PageSize = ((ListView)View).CollectionSource.GetCount();
                                } 
                            }
                        }
                    }
                    else if (View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_QCResultView")
                    {
                        Rollback.Active.SetItemValue("ShowResultViewRollBack", objPermissionInfo.ResultViewIsWrite);
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null && ((ListView)View).CollectionSource != null && ((ListView)View).CollectionSource.GetCount() > 0)
                        {
                            if (View.Id == "SampleParameter_ListView_Copy_ResultView")
                            {
                                ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_ResultView")).PageSize = ((ListView)View).CollectionSource.GetCount();

                            }
                            if (View.Id == "SampleParameter_ListView_Copy_QCResultView")
                            {
                                ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_QCResultView")).PageSize = ((ListView)View).CollectionSource.GetCount();
                            }
                        }
                    }
                    else if (View.Id == "ResultDefaultValue_LookupListView_ResultEntry")
                    {
                        ASPxGridListEditor grid = ((ListView)View).Editor as ASPxGridListEditor;
                        grid.Grid.SettingsBehavior.AllowSelectByRowClick = true;
                    }

                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor.Grid != null)
                    {
                        ASPxGridView gridView = gridListEditor.Grid;
                        if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry" || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice"))
                        {
                            tempviewinfo.strtempviewid = View.Id;
                            gridView.Settings.ShowStatusBar = GridViewStatusBarMode.Visible;
                            gridView.Styles.Row.CssClass = "row";

                            gridListEditor.Grid.CustomDataCallback += Grid_CustomDataCallback;
                            if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"))
                            {
                                gridListEditor.Grid.CommandButtonInitialize += Grid_CommandButtonInitialize;
                                gridView.HtmlCommandCellPrepared += GridView_HtmlCommandCellPrepared;
                            }
                            gridView.FillContextMenuItems += GridView_FillContextMenuItems;
                            gridView.SettingsContextMenu.Enabled = true;
                            gridView.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                            gridView.ClientSideEvents.FocusedCellChanging = @"function (s, e) {
                             sessionStorage.setItem('ResultEntryFocusedColumn', null);
                               if(sessionStorage.getItem('CurrFocusedColumn') == null)
                                {
                                    sessionStorage.setItem('PrevFocusedColumn', e.cellInfo.column.fieldName);
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }
                                else
                                {
                                    var precolumn = sessionStorage.getItem('CurrFocusedColumn');
                                    sessionStorage.setItem('PrevFocusedColumn', precolumn);                           
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }    
    if ((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit')) {
        e.cancel = true;
    } else if (e.cellInfo.column.fieldName == 'AnalyzedDate' ||e.cellInfo.column.fieldName == 'AnalyzedBy.Oid' ||e.cellInfo.column.fieldName == 'ResultNumeric' || e.cellInfo.column.fieldName == 'Result' || e.cellInfo.column.fieldName == 'SurrogateUnits.Oid'
         || e.cellInfo.column.fieldName == 'Units.Oid' || e.cellInfo.column.fieldName == 'Comment' || e.cellInfo.column.fieldName == 'FinalResult' || e.cellInfo.column.fieldName == 'FinalResultUnits.Oid' || e.cellInfo.column.fieldName == 'NumResult') {
        var fieldName = e.cellInfo.column.fieldName;
        sessionStorage.setItem('ResultEntryFocusedColumn', fieldName);
    } else {
        sessionStorage.setItem('ResultEntryFocusedColumn', fieldName);
        //e.cancel = true;
    }
    var name = s.GetEditor('ResultNumeric').name;
if(name != null)
    {
    var grid = name.split('_');
    grid.pop();
    grid.push('DXDataRow');
    name = grid.join('_');
    window.setTimeout(function () {
        var i = s.cpPagesize * s.GetPageIndex();
        var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
        for (i; i < totrow; i++) {
            if (s.batchEditApi.HasChanges(i, 'ResultNumeric') && s.batchEditApi.HasChanges(i, 'Result')) {
                var LOQ = parseFloat(s.batchEditApi.GetCellValue(i, 'LOQ'));
                var UQL = parseFloat(s.batchEditApi.GetCellValue(i, 'UQL'));
                var ResultNumeric = parseFloat(s.batchEditApi.GetCellValue(i, 'ResultNumeric'));
                var RptLimit = parseFloat(s.batchEditApi.GetCellValue(i, 'RptLimit'));
                if (ResultNumeric != null && LOQ != null && UQL != null && RptLimit != null && ResultNumeric > RptLimit) {
                    $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', ResultNumeric < LOQ || ResultNumeric > UQL);
                    $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', false);
                } else if (ResultNumeric == RptLimit) {
                    $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', true);
                    $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', false);
                }
            }
        }
    }, 10);
}
}";
                            gridListEditor.Grid.JSProperties["cpAnalyzeddateCurrentDatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "REAnalyzeddateCurrentDate");
                            gridListEditor.Grid.JSProperties["cpAnalyzeddateReceivedDatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "REAnalyzeddateReceivedDate");
                            gridView.ClientSideEvents.BatchEditEndEditing = @"function (s, e) {
    window.setTimeout(function () {
            var name = s.GetEditor('ResultNumeric').name;
            if(name != null)
              {
            var grid = name.split('_');
            grid.pop();
            grid.push('DXDataRow');
            name = grid.join('_');
        var FocusedColumn = sessionStorage.getItem('PrevFocusedColumn');
        var newvalue = s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric');
        var oldvalue =sessionStorage.getItem('OldValue');
        if(oldvalue=='null')
        {
            oldvalue=null;
         }
        if (FocusedColumn == 'ResultNumeric' && newvalue!=oldvalue) {
            if (s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit').trim().length > 0 && s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric').toString().length > 0) {
                
                var a=s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit');
                var ResultNumeric = s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric');
                var LOQ = s.batchEditApi.GetCellValue(e.visibleIndex, 'LOQ');
				var UQL = s.batchEditApi.GetCellValue(e.visibleIndex, 'UQL');
                var parameter = ResultNumeric + '|' + e.visibleIndex + '|' + LOQ + '|' + UQL;
                window.startProgress();
                s.GetValuesOnCustomCallback(parameter, OnGetValuesOnCustomCallbackComplete);
            } else {
                if ((s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit') == null || (s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit').trim().length ==0)) && s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric').toString().length > 0) {
                    
                    var ResultNumeric = s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric');
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Result', ResultNumeric);
                } else {
                       
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Result', null);
                }
                $('#' + name + e.visibleIndex + ' td[fieldName=Result]').removeClass('redCell');
                $('#' + name + e.visibleIndex + ' td[fieldName=Result]').removeClass('yellowCell');
            }
        } 
        var dateRecieved = s.batchEditApi.GetCellValue(e.visibleIndex, 'SCDateReceived');
        var dateAnalyzed = s.batchEditApi.GetCellValue(e.visibleIndex, 'AnalyzedDate');
        var dt = new Date();
        if(dt != null && dateRecieved != null && dateAnalyzed != null)
        {
            if(dateRecieved > dateAnalyzed)  
            {
                alert(s.cpAnalyzeddateReceivedDatemsg);
                s.batchEditApi.SetCellValue(e.visibleIndex, 'AnalyzedDate', null); 
            }
            else if(dt < dateAnalyzed)
            {
                alert(s.cpAnalyzeddateCurrentDatemsg);
                s.batchEditApi.SetCellValue(e.visibleIndex, 'AnalyzedDate', null); 
            }
        }
    }
    }, 10);
}";

                            if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice")
                            {
                                gridView.KeyboardSupport = true;
                                //gridListEditor.Grid. += gridControl_ProcessGridKey;
                                //gridView.Load += gridControl_ProcessGridKey;
                                if (gridView == null)
                                    gridListEditor.ControlsCreated += editor_ControlsCreated;
                                else SetClientInstanceName(gridView);
                                gridView.ClientSideEvents.ContextMenuItemClick = @"function (s, e) {
    if (s.IsRowSelectedOnPage(e.elementIndex)) {
        var FocusedColumn = sessionStorage.getItem('ResultEntryFocusedColumn');
        var oid;
        var text;
        if (FocusedColumn.includes('.')) {
            oid = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn, false);
            text = s.batchEditApi.GetCellTextContainer(e.elementIndex, FocusedColumn).innerText;
            if (e.item.name == 'CopyToAllCell') {
            var i = 0;
            var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
            for (i; i < s.GetVisibleRowsOnPage(); i++) {
                    if (s.IsRowSelectedOnPage(i)) {
                        s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                    }
                }
            }
        } else {
            var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn);
            var parameter;
            if (e.item.name == 'CopyToAllCell') {
                 var i = s.cpPagesize * s.GetPageIndex();
                 var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                 for (i; i < s.GetVisibleRowsOnPage(); i++) {
                    if (s.IsRowSelectedOnPage(i)) {
                        if (FocusedColumn == 'ResultNumeric') {
                            s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                            var name = s.GetEditor('ResultNumeric').name;
                        if(name != null)
                           {
                            var grid = name.split('_');
                            grid.pop();
                            grid.push('DXDataRow');
                            name = grid.join('_');
                            if (s.batchEditApi.GetCellValue(i, 'RptLimit') != null  && s.batchEditApi.GetCellValue(i, 'RptLimit').trim().length > 0 && s.batchEditApi.GetCellValue(i, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(i, 'ResultNumeric').toString().length > 0) {
                                var ResultNumeric = s.batchEditApi.GetCellValue(i, 'ResultNumeric');
                                var LOQ = s.batchEditApi.GetCellValue(i, 'LOQ');
				                var UQL = s.batchEditApi.GetCellValue(i, 'UQL');
                                if (parameter == null || parameter.length == 0) {
                                    parameter = ResultNumeric + '|' + i  + '|' + LOQ + '|' + UQL;
                                } else {
                                    parameter = parameter + ';' + ResultNumeric + '|' + i +  '|' + LOQ + '|' + UQL;
                                }
                            } else {
                                if ((s.batchEditApi.GetCellValue(i, 'RptLimit') == null || (s.batchEditApi.GetCellValue(i, 'RptLimit') != null && s.batchEditApi.GetCellValue(i, 'RptLimit').trim().length ==0)) && s.batchEditApi.GetCellValue(i, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(i, 'ResultNumeric').toString().length > 0) {
                                    var ResultNumeric = s.batchEditApi.GetCellValue(i, 'ResultNumeric').toString();
                                    s.batchEditApi.SetCellValue(i, 'Result', ResultNumeric);
                                } else {
                                    s.batchEditApi.SetCellValue(i, 'Result', null);
                                }
                                $('#' + name + i + ' td[fieldName=Result]').removeClass('redCell');
                                $('#' + name + i + ' td[fieldName=Result]').removeClass('yellowCell');
                            }
}
                        } else {
                            s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                        }
                    }
                }               
                if (parameter != null) {
                    window.startProgress();
                    s.GetValuesOnCustomCallback(parameter, OnGetValuesOnCustomCallbackComplete);
                }
            }
        }
    }
    e.processOnServer = false;
}";
                                gridListEditor.Grid.ClientSideEvents.BatchEditConfirmShowing = @"function(s,e) 
                                { 
                                  e.cancel = true;
                                }";
                                gridView.ClientSideEvents.BatchEditStartEditing = @"function(s,e)
                               {  
                                   var fieldName = sessionStorage.getItem('PrevFocusedColumn');
                                   if(fieldName=='ResultNumeric')
                                   {
                                       sessionStorage.setItem('OldValue',s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric'));
                                   }
                                  
                               }";
                            }
                            else
                            {
                                gridView.ClientSideEvents.ContextMenuItemClick = @"function (s, e) {
    if (s.IsRowSelectedOnPage(e.elementIndex)) {
        var FocusedColumn = sessionStorage.getItem('ResultEntryFocusedColumn');
        var oid;
        var text;
        if (FocusedColumn.includes('.')) {
            oid = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn, false);
            text = s.batchEditApi.GetCellTextContainer(e.elementIndex, FocusedColumn).innerText;
            if (e.item.name == 'CopyToAllCell') {
            var i = s.cpPagesize * s.GetPageIndex();
            var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
            for (i; i < totrow; i++) {
                    if (s.IsRowSelectedOnPage(i)) {
                        s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                    }
                }
            }
        } else {
            var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn);
            var parameter;
            if (e.item.name == 'CopyToAllCell') {
                 var i = s.cpPagesize * s.GetPageIndex();
                 var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                 for (i; i < totrow; i++) {
                    if (s.IsRowSelectedOnPage(i)) {
                        if (FocusedColumn == 'ResultNumeric') {
                            s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                            var name = s.GetEditor('ResultNumeric').name;
if(name != null)
    {
                            var grid = name.split('_');
                            grid.pop();
                            grid.push('DXDataRow');
                            name = grid.join('_');
                            if (s.batchEditApi.GetCellValue(i, 'RptLimit') != null  && s.batchEditApi.GetCellValue(i, 'RptLimit').trim().length > 0 && s.batchEditApi.GetCellValue(i, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(i, 'ResultNumeric').toString().length > 0) {
                                var ResultNumeric = s.batchEditApi.GetCellValue(i, 'ResultNumeric');
                                var LOQ = s.batchEditApi.GetCellValue(i, 'LOQ');
				                var UQL = s.batchEditApi.GetCellValue(i, 'UQL');
                                if (parameter == null || parameter.length == 0) {
                                    parameter = ResultNumeric + '|' + i  + '|' + LOQ + '|' + UQL;
                                } else {
                                    parameter = parameter + ';' + ResultNumeric + '|' + i +  '|' + LOQ + '|' + UQL;
                                }
                            } else {
                                if ((s.batchEditApi.GetCellValue(i, 'RptLimit') == null || (s.batchEditApi.GetCellValue(i, 'RptLimit') != null && s.batchEditApi.GetCellValue(i, 'RptLimit').trim().length ==0)) && s.batchEditApi.GetCellValue(i, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(i, 'ResultNumeric').toString().length > 0) {
                                    var ResultNumeric = s.batchEditApi.GetCellValue(i, 'ResultNumeric').toString();
                                    s.batchEditApi.SetCellValue(i, 'Result', ResultNumeric);
                                } else {
                                    s.batchEditApi.SetCellValue(i, 'Result', null);
                                }
                                $('#' + name + i + ' td[fieldName=Result]').removeClass('redCell');
                                $('#' + name + i + ' td[fieldName=Result]').removeClass('yellowCell');
                            }
}
                        } else {
                            s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                        }
                    }
                }               
                if (parameter != null) {
                    window.startProgress();
                    s.GetValuesOnCustomCallback(parameter, OnGetValuesOnCustomCallbackComplete);
                }
            }
        }
    }
    e.processOnServer = false;
}";
                            }
                        }

                        gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                        gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                        gridListEditor.Grid.Load += gridView_Load;
                        //XafCallbackManager ResultEntrySelectioncallback = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                        //ResultEntrySelectioncallback.RegisterHandler("ResultEntrySelection", this);
                        //gridView.ClientInstanceName = "ResultEntrysel";
                        gridView.JSProperties["cpRevieweditmsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Revieweditmsg");
                        gridView.JSProperties["cpReporteditmsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Reporteditmsg");
                        gridListEditor.Grid.JSProperties["cpuserid"] = SecuritySystem.CurrentUserId;
                        gridListEditor.Grid.JSProperties["cpusername"] = SecuritySystem.CurrentUserName;
                        gridView.JSProperties["cpPagesize"] = gridView.SettingsPager.PageSize;
                        //Employee employee = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        //if (employee != null)
                        //    gridListEditor.Grid.JSProperties["cpfullname"] = employee.FullName;

                        if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_QCResultView" || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice"))
                        {
                            gridView.ClientSideEvents.BatchEditStartEditing = @"function(s,e)
                            {  
                                if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingReview' || s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingVerify' || s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingEntry')
                                {
                                    e.cancel = true;
                                    alert(s.cpRevieweditmsg);
                                }
                                else if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'Reported' || s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'ReportApproved' || s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingReportValidation' || s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingReportApproval')
                                {
                                    e.cancel = true;
                                    alert(s.cpReporteditmsg);
                                }
                                else if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingValidation' || s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingApproval' || s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingReporting')
                                {
                                    e.cancel = false;
                                }                                
                            }";
                        }

                        if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry" || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice"))
                        {
                            gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = false;
                            gridView.ClientSideEvents.Init = @"function (s, e) {
    s.GetEditor('ResultNumeric').KeyPress.AddHandler(OnResultNumericChanged);
    var i = s.cpPagesize * s.GetPageIndex();
    //var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
     var totrow=s.cpVisibleRowCount;
    for (i; i < totrow; i++) {
        if (s.batchEditApi.GetCellValue(i, 'DF') == null) {
            s.batchEditApi.SetCellValue(i, 'DF', '1');
        }
    }
    window.onclick = function () {
    if (s.GetEditor('ResultNumeric')!=null)
	{
	    var name = s.GetEditor('ResultNumeric').name;
    if(name != null)
    {
        var grid = name.split('_');
        grid.pop();
        grid.push('DXDataRow');
        name = grid.join('_');
        window.setTimeout(function () {
            var i = s.cpPagesize * s.GetPageIndex();
            var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
            for (i; i < totrow; i++) {
                if (s.batchEditApi.HasChanges(i, 'ResultNumeric') && s.batchEditApi.HasChanges(i, 'Result')) {
                    var LOQ = parseFloat(s.batchEditApi.GetCellValue(i, 'LOQ'));
                    var UQL = parseFloat(s.batchEditApi.GetCellValue(i, 'UQL'));
                    var ResultNumeric = parseFloat(s.batchEditApi.GetCellValue(i, 'ResultNumeric'));
                    var RptLimit = parseFloat(s.batchEditApi.GetCellValue(i, 'RptLimit'));
                    if (ResultNumeric != null && LOQ != null && UQL != null && RptLimit != null && ResultNumeric > RptLimit) {
                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', ResultNumeric < LOQ || ResultNumeric > UQL);
                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', false);
                    } else if (ResultNumeric == RptLimit) {
                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', true);
                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', false);
                    }
                }
            }
        }, 10);
    } 
	}
    }
    s.OnScroll = function () {
        if (s.GetHorizontalScrollPosition() < 400) {
            var name = s.GetEditor('ResultNumeric').name;
        if(name != null)
        {
            var grid = name.split('_');
            grid.pop();
            grid.push('DXDataRow');
            name = grid.join('_');
            var i = s.cpPagesize * s.GetPageIndex();
            var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
            for (i; i < totrow; i++) {
                if (s.batchEditApi.HasChanges(i, 'ResultNumeric') && s.batchEditApi.HasChanges(i, 'Result')) {
                    var LOQ = parseFloat(s.batchEditApi.GetCellValue(i, 'LOQ'));
                    var UQL = parseFloat(s.batchEditApi.GetCellValue(i, 'UQL'));
                    var ResultNumeric = parseFloat(s.batchEditApi.GetCellValue(i, 'ResultNumeric'));
                    var RptLimit = parseFloat(s.batchEditApi.GetCellValue(i, 'RptLimit'));
                    if (ResultNumeric != null && LOQ != null && UQL != null && RptLimit != null && ResultNumeric > RptLimit) {
                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', ResultNumeric < LOQ || ResultNumeric > UQL);
                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', false);
                    } else if (ResultNumeric == RptLimit) {
                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', true);
                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', false);
                    }
                }
            }
        }
        }
    };
}";
                            gridView.ClientSideEvents.SelectionChanged = @"function(s,e){
                                var i = s.cpPagesize * s.GetPageIndex();
                                var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                                if (e.visibleIndex != -1)
                                {
                                    s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                                    if (s.IsRowSelectedOnPage(e.visibleIndex)) {   
                                    RaiseXafCallback(globalCallbackControl, 'ResultEntrySelection', 'Selected|' + Oidvalue , '', false);    
                                    }
                                    else{
                                    RaiseXafCallback(globalCallbackControl, 'ResultEntrySelection', 'UNSelected|' + Oidvalue, '', false);    
                                    }
                                    }); 
                                }
                                else if(e.visibleIndex == -1 && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                                {      
                                    RaiseXafCallback(globalCallbackControl, 'ResultEntrySelection', 'Selectall', '', false);     
                                }
                                else if(e.visibleIndex == -1 && s.GetSelectedRowCount() == 0)
                                {
                                    RaiseXafCallback(globalCallbackControl, 'ResultEntrySelection', 'UNSelectall', '', false);                        
                                }    
                               //// if (e.visibleIndex == -1 && !s.IsRowSelectedOnPage(0)) {
                               //// //for (i; i < totrow; i++) {
                               //// for (var i = 0 ; i < s.GetVisibleRowsOnPage(); i++) {
                               ////  if(s.batchEditApi.GetCellValue(i, 'AnalyzedDate') != null && s.batchEditApi.HasChanges(i,'AnalyzedDate')){
                               ////  s.batchEditApi.SetCellValue(i, 'AnalyzedDate', null);}
                               ////  if(s.batchEditApi.GetCellValue(i, 'EnteredDate') != null && s.batchEditApi.HasChanges(i,'EnteredDate')){
                               ////  s.batchEditApi.SetCellValue(i, 'EnteredDate', null);}
                               ////  if(s.batchEditApi.GetCellValue(i, 'AnalyzedBy') != null && s.batchEditApi.HasChanges(i,'AnalyzedBy')){
                               ////  s.batchEditApi.SetCellValue(i, 'AnalyzedBy', null);}
                               ////  if(s.batchEditApi.GetCellValue(i, 'EnteredBy') != null && s.batchEditApi.HasChanges(i,'EnteredBy')){
                               ////  s.batchEditApi.SetCellValue(i, 'EnteredBy', null);}
                               //// }
                               //// }
                               //// if(e.visibleIndex == -1 && s.IsRowSelectedOnPage(0) && s.GetSelectedRowCount() == s.cpVisibleRowCount){
                               //// var today = new Date();
                               //// for (var i = 0 ; i < s.GetVisibleRowsOnPage(); i++) { 
                               //// if(s.batchEditApi.GetCellValue(i, 'AnalyzedDate') == null && !s.batchEditApi.HasChanges(i,'AnalyzedDate')){
                               //// s.batchEditApi.SetCellValue(i, 'AnalyzedDate', today);}
                               //// if(s.batchEditApi.GetCellValue(i, 'EnteredDate') == null && !s.batchEditApi.HasChanges(i,'EnteredDate')){
                               //// s.batchEditApi.SetCellValue(i, 'EnteredDate', today);}
                               //// if(s.batchEditApi.GetCellValue(i, 'AnalyzedBy') == null && !s.batchEditApi.HasChanges(i,'AnalyzedBy')){
                               //// s.batchEditApi.SetCellValue(i, 'AnalyzedBy',s.cpuserid, s.cpfullname, false);}
                               //// if(s.batchEditApi.GetCellValue(i, 'EnteredBy') == null && !s.batchEditApi.HasChanges(i,'EnteredBy')){
                               //// s.batchEditApi.SetCellValue(i, 'EnteredBy',s.cpuserid, s.cpusername, false);}
                               //// }
                               //// }
                               //// else{
                               //// if (s.IsRowSelectedOnPage(e.visibleIndex)) {
                               ////  var today = new Date();
                               ////  if(s.batchEditApi.GetCellValue(e.visibleIndex, 'AnalyzedDate') == null && !s.batchEditApi.HasChanges(e.visibleIndex,'AnalyzedDate')){
                               ////  s.batchEditApi.SetCellValue(e.visibleIndex, 'AnalyzedDate', today);}                               
                               ////  if(s.batchEditApi.GetCellValue(e.visibleIndex, 'EnteredDate') == null && !s.batchEditApi.HasChanges(e.visibleIndex,'EnteredDate')){
                               ////  s.batchEditApi.SetCellValue(e.visibleIndex, 'EnteredDate', today);}                                
                               ////  if(s.batchEditApi.GetCellValue(e.visibleIndex, 'AnalyzedBy') == null && !s.batchEditApi.HasChanges(e.visibleIndex,'AnalyzedBy')){
                               ////  s.batchEditApi.SetCellValue(e.visibleIndex, 'AnalyzedBy',s.cpuserid, s.cpfullname, false);}                         
                               ////  if(s.batchEditApi.GetCellValue(e.visibleIndex, 'EnteredBy') == null && !s.batchEditApi.HasChanges(e.visibleIndex,'EnteredBy')){
                               ////  s.batchEditApi.SetCellValue(e.visibleIndex, 'EnteredBy',s.cpuserid, s.cpusername, false);}                                
                               //// }
                               //// else{
                               //// if(s.batchEditApi.GetCellValue(e.visibleIndex, 'AnalyzedDate') != null && s.batchEditApi.HasChanges(e.visibleIndex,'AnalyzedDate')){
                               ////  s.batchEditApi.SetCellValue(e.visibleIndex, 'AnalyzedDate', null);}                               
                               ////  if(s.batchEditApi.GetCellValue(e.visibleIndex, 'EnteredDate') != null && s.batchEditApi.HasChanges(e.visibleIndex,'EnteredDate')){
                               ////  s.batchEditApi.SetCellValue(e.visibleIndex, 'EnteredDate', null);}                                
                               ////  if(s.batchEditApi.GetCellValue(e.visibleIndex, 'AnalyzedBy') != null && s.batchEditApi.HasChanges(e.visibleIndex,'AnalyzedBy')){
                               ////  s.batchEditApi.SetCellValue(e.visibleIndex, 'AnalyzedBy', null);}                               
                               ////  if(s.batchEditApi.GetCellValue(e.visibleIndex, 'EnteredBy') != null && s.batchEditApi.HasChanges(e.visibleIndex,'EnteredBy')){
                               ////  s.batchEditApi.SetCellValue(e.visibleIndex, 'EnteredBy', null);}                               
                               //// }
                               ////}  
                            }";
                        }
                        //gridView.SelectionChanged += Grid_SelectionChanged;
                        gridView.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                        //}
                        if (objPermissionInfo.ResultEntryIsWrite == false)
                        {
                            gridView.ClientSideEvents.BatchEditStartEditing = @"function(s,e)
                            {  
                                e.cancel = true;
                                alert(s.cpRevieweditmsg);
                            }";
                        }
                        if (View.Id == "SampleParameter_ListView_Copy_ResultEntry")
                        {
                            if (gridView.Columns["IsResultDefaultValue"] != null)
                            {
                                gridView.Columns["IsResultDefaultValue"].Width = 0;
                                gridView.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                        if( e.focusedColumn.fieldName == 'Result')
                                             {
                                                var ResultNumeric = s.batchEditApi.GetCellValue(e.visibleIndex, 'IsResultDefaultValue');
                                                if ((s.batchEditApi.GetCellValue(e.visibleIndex, 'IsResultDefaultValue') == true ))
	                                              {
	                                             	 e.cancel = true; 
                                                  }
                                                  else
                                                    {
                                                      e.cancel = false;
                                                    }
                                             }
                                          else
                                               {
                                                    e.cancel = false;
                                               }
                                           }";
                            }
                            gridView.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                            XafCallbackManager parameter = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                            parameter.RegisterHandler("ResultDefaultValue", this);
                            gridView.ClientInstanceName = "ResultDefault";

                        }

                    }
                }
                else if (View.Id == "ResultEntry_Enter" || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice")
                {
                    ResultSubmit.Active.SetItemValue("ShowResultEntrySubmit", objPermissionInfo.ResultEntryIsWrite);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

            //gv.DataColumns[0].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
        }

        [Obsolete]
        void gridControl_ProcessGridKey(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            DevExpress.XtraGrid.GridControl gc = (DevExpress.XtraGrid.GridControl)sender;
            DevExpress.XtraGrid.Views.Grid.GridView gv = (DevExpress.XtraGrid.Views.Grid.GridView)gc.KeyboardFocusView;

            if ((System.Windows.Forms.Keys.KeyCode == System.Windows.Forms.Keys.Tab) &&
                (gv.FocusedColumn == gv.GetVisibleColumn(gv.VisibleColumns.Count - 1)))
            {
                gv.FocusedColumn = gv.GetVisibleColumn(0);
                if (gv.FocusedRowHandle != gv.RowCount - 1)
                    gv.FocusedRowHandle += 1;
                gv.ShowEditor();
                gv.CloseEditor();
                gv.MoveNext();
                e.Handled = true;
                //System.Windows.Forms.Keys.Enter = true;
            }
            if ((System.Windows.Forms.Keys.KeyCode == System.Windows.Forms.Keys.Tab) &&
                (gv.FocusedColumn == gv.GetVisibleColumn(0)))
            {
                gv.FocusedColumn = gv.GetVisibleColumn(gv.VisibleColumns.Count - 1);
                if (gv.FocusedRowHandle != 0)
                    gv.FocusedRowHandle -= 1;
                gv.CloseEditor();
                gv.MoveNext();
                gv.ShowEditor();
                e.Handled = true;
            }
        }
        private void SetClientInstanceName(ASPxGridView grid)
        {
            try
            {
                grid.ClientInstanceName = "Grid";
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void editor_ControlsCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxGridListEditor editor = (ASPxGridListEditor)sender;
                SetClientInstanceName(editor.Grid);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void GridView_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
        {
            try
            {
                if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"))
                {
                    if (e.CommandCellType == GridViewTableCommandCellType.Data)
                    {
                        if (objPermissionInfo.ResultEntryIsWrite == false)
                        {
                            ((System.Web.UI.WebControls.WebControl)e.Cell.Controls[0]).Enabled = false;
                        }
                        else
                        {
                            ((System.Web.UI.WebControls.WebControl)e.Cell.Controls[0]).Enabled = true;
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

        private void Grid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            try
            {
                if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"))
                {
                    ASPxGridView gridView = sender as ASPxGridView;
                    if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
                    {
                        if (objPermissionInfo.ResultEntryIsWrite == false)
                        {
                            e.Enabled = false;
                        }
                        else
                        {
                            e.Enabled = true;
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

        private void Grid_CustomDataCallback(object sender, ASPxGridViewCustomDataCallbackEventArgs e)
        {
            try
            {
                if (e.Parameters != "CustomCallback")
                {
                    ASPxGridView grid = sender as ASPxGridView;
                    List<string> mainparam = new List<string>();
                    List<string> response = new List<string>();
                    if (e.Parameters.Contains(";"))
                    {
                        mainparam = e.Parameters.Split(';').ToList();
                    }
                    else
                    {
                        mainparam.Add(e.Parameters);
                    }
                    foreach (string mainparamsplit in mainparam)
                    {
                        Testparameter testparameterobj = null;
                        var param = mainparamsplit.Split('|');
                        if (param.Count() > 1)
                        {
                            var Testoid = grid.GetRowValues(Convert.ToInt32(param[1]), "Testparameter.Oid");
                            testparameterobj = ObjectSpace.GetObjectByKey<Testparameter>(Testoid);
                        }
                        if (testparameterobj != null)
                        {
                            string strrptlimit = string.Empty;
                            string result;
                            string resultoutput = string.Empty;
                            if (testparameterobj.RptLimit != null)
                            {
                                string newrpt = Regex.Replace(testparameterobj.RptLimit, "[^.0-9]", "");
                                if (!String.IsNullOrEmpty(newrpt))
                                {
                                    strrptlimit = newrpt.ToString();
                                }
                            }
                            if (!string.IsNullOrEmpty(strrptlimit))
                            {
                                if (!string.IsNullOrEmpty(strrptlimit) && Convert.ToDouble(param[0].ToString().Replace('<', ' ').Replace('>', ' ').Trim()) >= Convert.ToDouble(strrptlimit))
                                {
                                    if (testparameterobj.CutOff != null && testparameterobj.CutOff.Length > 0)
                                    {
                                        if (testparameterobj.SigFig != null && testparameterobj.SigFig.Length > 0 && Convert.ToDouble(param[0].ToString().Replace('<', ' ').Replace('>', ' ').Trim()) >= Convert.ToDouble(testparameterobj.CutOff))
                                        {
                                            var Cal = Roundoff.RoundoffInput(param[0].ToString().Replace('<', ' ').Replace('>', ' ').Trim(), testparameterobj.SigFig).Split('|');
                                            result = Cal[0];
                                        }
                                        else if (testparameterobj.Decimal != null && testparameterobj.Decimal.Length > 0 && Convert.ToDouble(param[0].ToString().Replace('<', ' ').Replace('>', ' ').Trim()) < Convert.ToDouble(testparameterobj.CutOff))
                                        {
                                            result = Roundoff.FormatDecimalValue(param[0].ToString().Replace('<', ' ').Replace('>', ' ').Trim(), Convert.ToInt32(testparameterobj.Decimal));
                                        }
                                        else
                                        {
                                            result = param[0].ToString().Replace('<', ' ').Replace('>', ' ').Trim();
                                        }
                                    }
                                    else
                                    {
                                        if ( !string.IsNullOrEmpty(testparameterobj.SigFig) && testparameterobj.SigFig.Length > 0 && testparameterobj.SigFig != "0")
                                        {
                                            var Cal = Roundoff.RoundoffInput(param[0].ToString().Replace('<', ' ').Replace('>', ' ').Trim(), testparameterobj.SigFig).Split('|');
                                            result = Cal[0];
                                        }
                                        else if (testparameterobj.Decimal != null && testparameterobj.Decimal.Length > 0)
                                        {
                                            result = Roundoff.FormatDecimalValue(param[0].ToString().Replace('<', ' ').Replace('>', ' ').Trim(), Convert.ToInt32(testparameterobj.Decimal));
                                        }
                                        else
                                        {
                                            result = param[0].ToString().Replace('<', ' ').Replace('>', ' ').Trim();
                                        }
                                    }
                                }
                                else
                                {
                                    result = param[0].ToString().Replace('<', ' ').Replace('>', ' ').Trim();
                                }
                                if (!string.IsNullOrEmpty(strrptlimit) && Convert.ToDouble(result) < Convert.ToDouble(strrptlimit))
                                {
                                    resultoutput = testparameterobj.DefaultResult + "|" + param[1] + "|";
                                }
                                else if (!string.IsNullOrEmpty(strrptlimit) && Convert.ToDouble(result) >= Convert.ToDouble(strrptlimit))
                                {
                                    if (string.IsNullOrEmpty(param[2]))
                                    {
                                        param[2] = "0";
                                    }
                                    if (string.IsNullOrEmpty(param[3]))
                                    {
                                        param[3] = "0";
                                    }
                                    if ((param[2] != "null" && param[3] != "null") && (Convert.ToDouble(result) < Convert.ToDouble(param[2]) || Convert.ToDouble(result) > Convert.ToDouble(param[3])))
                                    {
                                        resultoutput = result + "|" + param[1] + "|redcell";
                                    }
                                    else if (Convert.ToDouble(result) == Convert.ToDouble(strrptlimit))
                                    {
                                        resultoutput = result + "|" + param[1] + "|yellowcell";
                                    }
                                    else
                                    {
                                        resultoutput = result + "|" + param[1] + "|";
                                    }
                                }
                                else
                                {
                                    resultoutput = result + "|" + param[1] + "|";
                                }
                            }

                            response.Add(resultoutput);
                        }
                    }
                    if (response.Count > 0)
                    {
                        e.Result = string.Join(";", response.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                e.Result = "error";
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            try
            {
                if (View.Id == "ResultEntryQueryPanel_DetailView_Copy" || View.Id == "ResultEntryQueryPanel_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Active.SetItemValue("save", true);
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("saveclose", true);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("savenew", true);
                    Frame.GetController<ModificationsController>().CancelAction.Active.SetItemValue("cancel", true);
                }
                if (resultentryinfo.lstresultentry == null)
                {
                    resultentryinfo.lstresultentry = new List<SampleParameter>();
                }
                else
                {
                    resultentryinfo.lstresultentry.Clear();
                }
                ResultSubmit.Executing -= ResultSubmit_Executing;
                ResultSubmit.Executed -= ResultSubmit_Executed;
                //ResultEnter.Executing -= ResultEnter_Executing;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        #endregion

        #region SimpleActionEvents
        private void ResultValidation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "ResultEntry_Validation")
                {
                    int selectedcount = 0;
                    //DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    Modules.BusinessObjects.Setting.DefaultSetting RVdefsetting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'Result Validation' And [GCRecord] is Null"));
                    Modules.BusinessObjects.Setting.DefaultSetting RAdefsetting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Data Review' And [NavigationItemNameID] = 'Result Approval' And [GCRecord] is Null"));

                    foreach (DashboardViewItem viewItem in ((DashboardView)View).Items)
                    {
                        if (viewItem.InnerView is ListView && viewItem.InnerView.SelectedObjects.Count > 0)
                        {
                            selectedcount += viewItem.InnerView.SelectedObjects.Count;
                            ((ASPxGridListEditor)((ListView)viewItem.InnerView).Editor).Grid.UpdateEdit();
                            foreach (SampleParameter objSampleResult in viewItem.InnerView.SelectedObjects)
                            {
                                if (RAdefsetting.Select == true)
                                {
                                    objSampleResult.Status = Samplestatus.PendingApproval;
                                    objSampleResult.OSSync = true;
                                }
                                else
                                {
                                    objSampleResult.ApprovedBy = objSampleResult.ValidatedBy;
                                    objSampleResult.ApprovedDate = objSampleResult.ApprovedDate;
                                    objSampleResult.Status = Samplestatus.PendingReporting;
                                    objSampleResult.OSSync = true;
                                    if (objSampleResult.Samplelogin != null && objSampleResult.Samplelogin.JobID != null && objSampleResult.QCBatchID != null && objSampleResult.QCBatchID.QCType != null && objSampleResult.Samplelogin.QCCategory != null
                                        && ( objSampleResult.Samplelogin.QCCategory.QCCategoryName == "PT" || objSampleResult.Samplelogin.QCCategory.QCCategoryName == "DOC" || objSampleResult.Samplelogin.QCCategory.QCCategoryName == "MDL"))
                                    {
                                        IObjectSpace os = Application.CreateObjectSpace();
                                        Session currentSession = ((XPObjectSpace)os).Session;
                                        UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                        string strjobID = string.Empty;
                                        Samplecheckin lstobj = uow.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]= ?", objSampleResult.Samplelogin.JobID.Oid));
                                        DOC Objstudylog = uow.FindObject<DOC>(CriteriaOperator.Parse("[JobID.Oid]= ?", objSampleResult.Samplelogin.JobID.Oid));
                                        if (Objstudylog == null)
                                        {
                                            DOC objPT = new DOC(uow);
                                            objPT.JobID = lstobj;
                                            objPT.strJobID = string.Join(";", viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().Select(i => i.JobID).Distinct());
                                            objPT.Status = DOC.DOCstatus.PendingSubmission;
                                            objSampleResult.Status1 = SampleParameter.PFStatus.None;
                                            if (objSampleResult != null && objSampleResult.Testparameter != null && objSampleResult.Testparameter.TestMethod != null)
                                            {
                                                objPT.Test = uow.GetObjectByKey<TestMethod>(objSampleResult.Testparameter.TestMethod.Oid);
                                            }
                                            if (objSampleResult != null && objSampleResult.Testparameter != null && objSampleResult.Testparameter.TestMethod != null && objSampleResult.Testparameter.TestMethod.MethodName != null)
                                            {
                                                objPT.Method = uow.GetObjectByKey<Method>(objSampleResult.Testparameter.TestMethod.MethodName.Oid);
                                            }
                                            if (objSampleResult != null && objSampleResult.Testparameter != null && objSampleResult.Testparameter.TestMethod != null && objSampleResult.Testparameter.TestMethod.MatrixName != null)
                                            {
                                                objPT.Matrix = uow.GetObjectByKey<Matrix>(objSampleResult.Testparameter.TestMethod.MatrixName.Oid);
                                            }
                                            if (objSampleResult.AnalyzedBy != null)
                                            {
                                                objPT.Analyst = uow.GetObjectByKey<Employee>(objSampleResult.AnalyzedBy.Oid);
                                                objPT.DateAnalyzed = objSampleResult.AnalyzedDate;
                                            }
                                            if (objSampleResult.QCBatchID != null)
                                            {
                                                objPT.QCBatches = objSampleResult.QCBatchID.qcseqdetail.AnalyticalBatchID;
                                            }
                                            List<SampleParameter> lstSamples = viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().ToList();
                                            if (lstSamples.FirstOrDefault(i => i.PrepBatchID != null) != null && lstSamples.FirstOrDefault(i => i.PrepBatchID != null).PrepBatchID != null)
                                            {
                                                List<Guid> lstpep = lstSamples.Where(i => i.PrepBatchID != null).SelectMany(i => i.PrepBatchID.Split(';')).Select(Guid.Parse).Distinct().ToList();
                                                if (lstpep.Count > 0)
                                                {
                                                    IList<SamplePrepBatch> lstPrep = ObjectSpace.GetObjects<SamplePrepBatch>(new InOperator("Oid", lstpep));
                                                    if (lstpep.Count > 0)
                                                    {
                                                        objPT.PrepBatchID = string.Join(";", lstPrep.Select(i => i.PrepBatchID));
                                                    }
                                                }
                                            }
                                            if (lstSamples.FirstOrDefault(i => i.UQABID != null) != null && lstSamples.FirstOrDefault(i => i.UQABID != null).UQABID != null)
                                            {
                                                objPT.AnalyticalInstrument = string.Join(";", lstSamples.Where(i => i.UQABID != null && !string.IsNullOrEmpty(i.UQABID.strInstrument)).SelectMany(i => i.UQABID.strInstrument.Split(';')).Distinct());

                                            }
                                            //if (objSampleResult.PrepBatchID != null)
                                            //{
                                            //    List<Guid> lstpep = objSampleResult.PrepBatchID.Split(';').Select(Guid.Parse).ToList();
                                            //    IList<SamplePrepBatch> lstPrep = ObjectSpace.GetObjects<SamplePrepBatch>(new InOperator("Oid", lstpep));
                                            //    objPT.PrepBatchID = string.Join(";", lstPrep.Select(i => i.PrepBatchID));
                                            //}
                                            //if (objSampleResult.UQABID != null)
                                            //{
                                            //    objPT.AnalyticalInstrument = objSampleResult.UQABID.strInstrument;

                                            //}
                                            objPT.Save();
                                            uow.CommitChanges();
                                        }
                                        else
                                        {
                                            if (objSampleResult.QCBatchID != null && objSampleResult.QCBatchID.qcseqdetail != null && !Objstudylog.QCBatches.Contains(objSampleResult.QCBatchID.qcseqdetail.AnalyticalBatchID))
                                            {
                                                Objstudylog.QCBatches = Objstudylog.QCBatches + ";" + objSampleResult.QCBatchID.qcseqdetail.AnalyticalBatchID;
                                                uow.CommitChanges();
                                            }
                                            List<SampleParameter> lstSamples = viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().ToList();
                                            if (lstSamples.FirstOrDefault(i => i.PrepBatchID != null) != null && lstSamples.FirstOrDefault(i => i.PrepBatchID != null).PrepBatchID != null)
                                            {
                                                List<Guid> lstpep = lstSamples.Where(i => i.PrepBatchID != null).SelectMany(i => i.PrepBatchID.Split(';')).Select(Guid.Parse).Distinct().ToList();
                                                if (lstpep.Count > 0)
                                                {
                                                    IList<SamplePrepBatch> lstPrep = ObjectSpace.GetObjects<SamplePrepBatch>(new InOperator("Oid", lstpep));
                                                    if (lstpep.Count > 0)
                                                    {
                                                        Objstudylog.PrepBatchID = string.Join(";", lstPrep.Select(i => i.PrepBatchID));
                                                    }
                                                }
                                            }
                                            if (lstSamples.FirstOrDefault(i => i.UQABID != null) != null && lstSamples.FirstOrDefault(i => i.UQABID != null).UQABID != null)
                                            {
                                                Objstudylog.AnalyticalInstrument = string.Join(";", lstSamples.Where(i => i.UQABID != null && !string.IsNullOrEmpty(i.UQABID.strInstrument)).SelectMany(i => i.UQABID.strInstrument.Split(';')).Distinct());

                                    }
                                }
                                    }
                                }
                                //if (setting.REApprove == EnumRELevelSetup.Yes)
                                //{
                                //    objSampleResult.Status = Samplestatus.PendingApproval;
                                //}
                                //else if (setting.REValidate == EnumRELevelSetup.No && setting.REApprove == EnumRELevelSetup.No)
                                //{
                                //    objSampleResult.ApprovedBy = objSampleResult.ValidatedBy;
                                //    objSampleResult.ApprovedDate = objSampleResult.ApprovedDate;
                                //    objSampleResult.Status = Samplestatus.PendingReporting;
                                //}
                                //else if (setting.REValidate == EnumRELevelSetup.No && setting.REApprove == EnumRELevelSetup.No && setting.ReportValidate == EnumRELevelSetup.Yes)
                                //{
                                //    objSampleResult.Status = Samplestatus.PendingReportValidation;
                                //}
                                //else if (setting.REValidate == EnumRELevelSetup.No && setting.REApprove == EnumRELevelSetup.No && setting.ReportValidate == EnumRELevelSetup.No
                                //    && setting.ReportApprove == EnumRELevelSetup.Yes)
                                //{
                                //    objSampleResult.Status = Samplestatus.PendingReportApproval;
                                //}                                    
                            }

                            if (viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.ProjectCategory != null
                             && i.Samplelogin.JobID.ProjectCategory.CategoryName != null).FirstOrDefault(i => i.Samplelogin != null
                             && i.Samplelogin.JobID.ProjectCategory.CategoryName == "PT" || i.Samplelogin.JobID.ProjectCategory.CategoryName == "DOC" || i.Samplelogin.JobID.ProjectCategory.CategoryName == "DOC"
                            ) != null)
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                Session currentSession = ((XPObjectSpace)os).Session;
                                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                string strjobID = string.Empty;
                                foreach (SampleParameter objSampleResult in viewItem.InnerView.SelectedObjects)
                                {
                                    PTStudyLogResults objResult = uow.FindObject<PTStudyLogResults>(CriteriaOperator.Parse("[SampleID.Oid]=?", objSampleResult.Oid));
                                    if (objResult != null)
                                    {
                                        objResult.ReportedValue = objSampleResult.Result;
                                        if (objSampleResult.AnalyzedBy != null)
                                        {
                                            objResult.AnalyzedBy = uow.GetObjectByKey<Employee>(objSampleResult.AnalyzedBy.Oid);
                                        }
                                        objResult.DateAnalyzed = objSampleResult.AnalyzedDate;
                                        strjobID = objResult.SampleID.Samplelogin.JobID.JobID;
                                    }

                                }
                                IList<SampleParameter> lstSamples = uow.Query<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID == strjobID).ToList();
                                if (lstSamples.FirstOrDefault(i => i.Result == null || i.AnalyzedBy == null || i.AnalyzedDate == null) == null)
                                {
                                    PTStudyLog objPtStudy = uow.FindObject<PTStudyLog>(CriteriaOperator.Parse("[SampleCheckinJobID.JobID]=?", strjobID));
                                    if (objPtStudy != null)
                                    {
                                        objPtStudy.Status = PTStudyLog.PTStudyLogStatus.PendingPTResultEntry;
                                    }
                                }
                                uow.CommitChanges();
                            }
                            viewItem.InnerView.ObjectSpace.CommitChanges();
                            //if (viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().FirstOrDefault(i=>i.UQABID== null || i.UQABID != null))
                            //{

                            if (viewItem.InnerView.Id == "SampleParameter_ListView_Copy_ResultValidation")
                            {
                                //foreach (Samplecheckin objSample1 in ((ListView)viewItem.InnerView).CollectionSource.List.Cast<SampleParameter>().Where(i => i.Samplelogin.SampleID != null && i.Samplelogin.JobID != null).Select(i => i.Samplelogin.JobID).Distinct().ToList())
                                //{
                                //    IList<SampleParameter> lstSamples = View.ObjectSpace.GetObjects<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID == objSample1.JobID).ToList();
                                //    Samplecheckin sample = View.ObjectSpace.GetObjectByKey<Samplecheckin>(objSample1.Oid);
                                //    //if (lstSamples.Where(i => i.Status == Samplestatus.PendingApproval).Count() == 0)
                                //    if (lstSamples.Where(i => i.Status == Samplestatus.PendingValidation).Count() == 0 && lstSamples.Where(i => i.Status == Samplestatus.PendingEntry).Count() == 0 && lstSamples.Where(i => i.Status == Samplestatus.PendingVerify).Count() == 0 && lstSamples.Where(i => i.Status == Samplestatus.PendingReview).Count() == 0)

                                //    {
                                //        StatusDefinition objStatus = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Index] = '121'"));
                                //        if (objStatus != null)
                                //        {
                                //            sample.Index = objStatus;
                                //        }

                                //    }
                                foreach (Samplecheckin objSampleResult1 in ((ListView)viewItem.InnerView).CollectionSource.List.Cast<SampleParameter>().Where(i => i.Samplelogin.SampleID != null && i.Samplelogin.JobID != null).Select(i => i.Samplelogin.JobID).Distinct().ToList())
                                {
                                    IList<SampleParameter> lstsmpl1 = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Testparameter.QCType.QCTypeName] = 'Sample'", objSampleResult1.Oid));
                                    if (lstsmpl1.FirstOrDefault(i => i.Status == Samplestatus.PendingEntry || i.Status==Samplestatus.PendingValidation) == null)
                                    {
                                        StatusDefinition objStatus = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 21"));
                                        if (objStatus != null)
                                        {
                                            Samplecheckin objJobID = View.ObjectSpace.GetObjectByKey<Samplecheckin>(objSampleResult1.Oid);
                                            objJobID.Index = objStatus;
                                        }
                                    }
                                }
                            }
                         
                            //}
                         
                            View.ObjectSpace.CommitChanges();
                            viewItem.InnerView.ObjectSpace.Refresh();
                        }
                    }
                    if (selectedcount > 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "resultvalidate"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        //NavigationItemCountRefresh();
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ResultApproval_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "ResultEntry_Approval")
                {
                    int selectedcount = 0;
                    //DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    foreach (DashboardViewItem viewItem in ((DashboardView)View).Items)
                    {
                        if (viewItem.InnerView is ListView && viewItem.InnerView.SelectedObjects.Count > 0)
                        {
                            selectedcount += viewItem.InnerView.SelectedObjects.Count;
                            ((ASPxGridListEditor)((ListView)viewItem.InnerView).Editor).Grid.UpdateEdit();
                            foreach (SampleParameter objSampleResult in viewItem.InnerView.SelectedObjects)
                            {
                                objSampleResult.Status = Samplestatus.PendingReporting;
                                objSampleResult.OSSync = true;
                            }
                            if (viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.ProjectCategory != null
                             && i.Samplelogin.JobID.ProjectCategory.CategoryName != null).FirstOrDefault(i => i.Samplelogin != null
                             && i.Samplelogin.JobID.ProjectCategory.CategoryName == "PT" || i.Samplelogin.JobID.ProjectCategory.CategoryName == "DOC" || i.Samplelogin.JobID.ProjectCategory.CategoryName == "DOC"
                            ) != null)
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                Session currentSession = ((XPObjectSpace)os).Session;
                                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                string strjobID = string.Empty;
                                foreach (SampleParameter objSampleResult in viewItem.InnerView.SelectedObjects)
                                {
                                    PTStudyLogResults objResult = uow.FindObject<PTStudyLogResults>(CriteriaOperator.Parse("[SampleID.Oid]=?", objSampleResult.Oid));
                                    if (objResult != null)
                                    {
                                        objResult.ReportedValue = objSampleResult.Result;
                                        if (objSampleResult.AnalyzedBy != null)
                                        {
                                            objResult.AnalyzedBy = uow.GetObjectByKey<Employee>(objSampleResult.AnalyzedBy.Oid);
                                        }
                                        objResult.DateAnalyzed = objSampleResult.AnalyzedDate;
                                        strjobID = objResult.SampleID.Samplelogin.JobID.JobID;
                                    }

                                }
                                IList<SampleParameter> lstSamples = uow.Query<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID == strjobID).ToList();
                                if (lstSamples.FirstOrDefault(i => i.Result == null || i.AnalyzedBy == null || i.AnalyzedDate == null) == null)
                                {
                                    PTStudyLog objPtStudy = uow.FindObject<PTStudyLog>(CriteriaOperator.Parse("[SampleCheckinJobID.JobID]=?", strjobID));
                                    if (objPtStudy != null)
                                    {
                                        objPtStudy.Status = PTStudyLog.PTStudyLogStatus.PendingPTResultEntry;
                                    }
                                }
                                uow.CommitChanges();
                            }
                            viewItem.InnerView.ObjectSpace.CommitChanges();
                            //if (viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().Where(i => i.UQABID == null || i.UQABID != null).Count() > 0)
                            //{
                            if(viewItem.InnerView.Id== "SampleParameter_ListView_Copy_ResultApproval")
                            {
                                //foreach (Samplecheckin objSample1 in ((ListView)viewItem.InnerView).CollectionSource.List.Cast<SampleParameter>().Where(i => i.Samplelogin.SampleID != null && i.Samplelogin.JobID != null).Select(i => i.Samplelogin.JobID).Distinct().ToList())
                                //{
                                //    IList<SampleParameter> lstSamples = View.ObjectSpace.GetObjects<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID == objSample1.JobID).ToList();
                                //    Samplecheckin sample = View.ObjectSpace.GetObjectByKey<Samplecheckin>(objSample1.Oid);
                                //    if (lstSamples.Where(i => i.Status == Samplestatus.PendingValidation).Count() == 0 && lstSamples.Where(i => i.Status == Samplestatus.PendingApproval).Count() == 0 && lstSamples.Where(i => i.Status == Samplestatus.PendingEntry).Count() == 0 && lstSamples.Where(i => i.Status == Samplestatus.PendingReview).Count() == 0 && lstSamples.Where(i => i.Status == Samplestatus.PendingVerify).Count() == 0)
                                //    {
                                //        StatusDefinition objStatus = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Index] = '122'"));
                                //        if (objStatus != null)
                                //        {
                                //            lstSamples.FirstOrDefault().Samplelogin.JobID.Index = objStatus;
                                //        }
                                //    }
                                //}
                                foreach (Samplecheckin objSampleResult1 in ((ListView)viewItem.InnerView).CollectionSource.List.Cast<SampleParameter>().Where(i => i.Samplelogin.SampleID != null && i.Samplelogin.JobID != null).Select(i => i.Samplelogin.JobID).Distinct().ToList())
                                {
                                    IList<SampleParameter> lstsmpl1 = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Testparameter.QCType.QCTypeName] = 'Sample'", objSampleResult1.Oid));
                                    if (lstsmpl1.FirstOrDefault(i => i.Status == Samplestatus.PendingEntry || i.Status == Samplestatus.PendingValidation ||i.Status==Samplestatus.PendingApproval) == null)
                                    {
                                        StatusDefinition objStatus = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 22"));
                                        if (objStatus != null)
                                        {
                                            Samplecheckin objJobID = View.ObjectSpace.GetObjectByKey<Samplecheckin>(objSampleResult1.Oid);
                                            objJobID.Index = objStatus;
                                        }
                                    }
                                }
                            }
                                
                            //}
                            if (viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.QCBatchID != null && i.QCBatchID.QCType != null && i.Samplelogin.QCCategory != null/*&& i.Samplelogin.JobID.ProjectCategory != null && i.Samplelogin.JobID.ProjectCategory.CategoryName != null*/
                            ).FirstOrDefault(i => i.Samplelogin != null  &&(i.Samplelogin.QCCategory.QCCategoryName == "PT" || i.Samplelogin.QCCategory.QCCategoryName == "DOC" || i.Samplelogin.QCCategory.QCCategoryName == "MDL")) != null)
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                Session currentSession = ((XPObjectSpace)os).Session;
                                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                //string strjobID = string.Empty;
                                SampleParameter objSampleResult = viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.QCBatchID != null && i.QCBatchID.QCType != null && i.Samplelogin.QCCategory != null/*&& i.Samplelogin.JobID.ProjectCategory != null && i.Samplelogin.JobID.ProjectCategory.CategoryName != null*/
                            ).FirstOrDefault(i => i.Samplelogin != null && (i.Samplelogin.QCCategory.QCCategoryName == "PT" || i.Samplelogin.QCCategory.QCCategoryName == "DOC" || i.Samplelogin.QCCategory.QCCategoryName == "MDL"));
                                if (objSampleResult!=null)
                                {
                                    Samplecheckin lstobj = uow.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]= ?", objSampleResult.Samplelogin.JobID.Oid));
                                    DOC Objstudylog = uow.FindObject<DOC>(CriteriaOperator.Parse("[JobID.Oid]= ?", objSampleResult.Samplelogin.JobID.Oid));
                                    if (Objstudylog == null)
                                    {
                                        DOC objPT = new DOC(uow);
                                        objPT.JobID = lstobj;
                                        objPT.strJobID = string.Join(";", viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().Select(i => i.JobID).Distinct());
                                        objPT.Status = DOC.DOCstatus.PendingSubmission;
                                        objSampleResult.Status1 = SampleParameter.PFStatus.None;
                                        if (objSampleResult != null && objSampleResult.Testparameter != null && objSampleResult.Testparameter.TestMethod != null)
                                        {
                                            objPT.Test = uow.GetObjectByKey<TestMethod>(objSampleResult.Testparameter.TestMethod.Oid);
                                        }
                                        if (objSampleResult != null && objSampleResult.Testparameter != null && objSampleResult.Testparameter.TestMethod != null && objSampleResult.Testparameter.TestMethod.MethodName != null)
                                        {
                                            objPT.Method = uow.GetObjectByKey<Method>(objSampleResult.Testparameter.TestMethod.MethodName.Oid);
                                        }
                                        if (objSampleResult != null && objSampleResult.Testparameter != null && objSampleResult.Testparameter.TestMethod != null && objSampleResult.Testparameter.TestMethod.MatrixName != null)
                                        {
                                            objPT.Matrix = uow.GetObjectByKey<Matrix>(objSampleResult.Testparameter.TestMethod.MatrixName.Oid);
                                        }
                                        if (objSampleResult.AnalyzedBy != null)
                                        {
                                            objPT.Analyst = uow.GetObjectByKey<Employee>(objSampleResult.AnalyzedBy.Oid);
                                            objPT.DateAnalyzed = objSampleResult.AnalyzedDate;
                                        }
                                        if(objSampleResult.QCBatchID != null)
                                        {
                                            objPT.QCBatches = objSampleResult.QCBatchID.qcseqdetail.AnalyticalBatchID;
                                        }
                                        List<SampleParameter> lstSamples = viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().ToList();
                                        if (lstSamples.FirstOrDefault(i => i.PrepBatchID != null) != null && lstSamples.FirstOrDefault(i => i.PrepBatchID != null).PrepBatchID != null)
                                        {
                                            List<Guid> lstpep = lstSamples.Where(i => i.PrepBatchID != null).SelectMany(i => i.PrepBatchID.Split(';')).Select(Guid.Parse).Distinct().ToList();
                                            if (lstpep.Count > 0)
                                        {
                                            IList<SamplePrepBatch> lstPrep = ObjectSpace.GetObjects<SamplePrepBatch>(new InOperator("Oid", lstpep));
                                                if (lstpep.Count > 0)
                                                {
                                            objPT.PrepBatchID = string.Join(";", lstPrep.Select(i => i.PrepBatchID));
                                        }
                                            }
                                        }
                                        if (lstSamples.FirstOrDefault(i => i.UQABID != null) != null && lstSamples.FirstOrDefault(i => i.UQABID != null).UQABID != null)
                                        {
                                            objPT.AnalyticalInstrument = string.Join(";", lstSamples.Where(i => i.UQABID != null && !string.IsNullOrEmpty(i.UQABID.strInstrument)).SelectMany(i => i.UQABID.strInstrument.Split(';')).Distinct());

                                        }
                                        //if (objSampleResult.PrepBatchID != null)
                                        //{
                                        //    List<Guid> lstpep = objSampleResult.PrepBatchID.Split(';').Select(Guid.Parse).ToList();
                                        //    IList<SamplePrepBatch> lstPrep = ObjectSpace.GetObjects<SamplePrepBatch>(new InOperator("Oid", lstpep));
                                        //    objPT.PrepBatchID = string.Join(";", lstPrep.Select(i => i.PrepBatchID));
                                        //}
                                        //if (objSampleResult.UQABID != null)
                                        //{
                                        //    objPT.AnalyticalInstrument = objSampleResult.UQABID.strInstrument;

                                        //}
                                        objPT.Save();
                                        uow.CommitChanges();
                                    }
                                    else
                                    {
                                        if (objSampleResult.QCBatchID != null && objSampleResult.QCBatchID.qcseqdetail != null && !Objstudylog.QCBatches.Contains(objSampleResult.QCBatchID.qcseqdetail.AnalyticalBatchID))
                                        {
                                            Objstudylog.QCBatches = Objstudylog.QCBatches + ";" + objSampleResult.QCBatchID.qcseqdetail.AnalyticalBatchID;
                                            
                                        }
                                        List<SampleParameter> lstSamples = viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().ToList();
                                        if (lstSamples.FirstOrDefault(i => i.PrepBatchID != null) != null && lstSamples.FirstOrDefault(i => i.PrepBatchID != null).PrepBatchID != null)
                                        {
                                            List<Guid> lstpep = lstSamples.Where(i => i.PrepBatchID != null).SelectMany(i => i.PrepBatchID.Split(';')).Select(Guid.Parse).Distinct().ToList();
                                            if (lstpep.Count > 0)
                                            {
                                                IList<SamplePrepBatch> lstPrep = ObjectSpace.GetObjects<SamplePrepBatch>(new InOperator("Oid", lstpep));
                                                if (lstpep.Count > 0)
                                                {
                                                    Objstudylog.PrepBatchID = string.Join(";", lstPrep.Select(i => i.PrepBatchID));
                                                }
                                            }
                                        }
                                        if (lstSamples.FirstOrDefault(i => i.UQABID != null) != null && lstSamples.FirstOrDefault(i => i.UQABID != null).UQABID != null)
                                        {
                                            Objstudylog.AnalyticalInstrument = string.Join(";", lstSamples.Where(i => i.UQABID != null && !string.IsNullOrEmpty(i.UQABID.strInstrument)).SelectMany(i => i.UQABID.strInstrument.Split(';')).Distinct());

                                        }
                                            uow.CommitChanges();
                                        }
                                    }
                                    //if (objSampleResult.Samplelogin != null && objSampleResult.Samplelogin.QCCategory != null && objSampleResult.Samplelogin.QCCategory.QCCategoryName == "DOC")
                                    //{
                                    //    Samplecheckin lstobj = uow.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]= ?", objSampleResult.Samplelogin.JobID.Oid));
                                    //    DOC Objstudylog = uow.FindObject<DOC>(CriteriaOperator.Parse("[JobID.Oid]= ?", objSampleResult.Samplelogin.JobID.Oid));
                                    //    if (Objstudylog == null)
                                    //    {
                                    //        DOC objPT = new DOC(uow);
                                    //        objPT.JobID = lstobj;
                                    //        objPT.Status = DOC.DOCstatus.PendingSubmission;
                                    //        if (objSampleResult != null && objSampleResult.Testparameter != null && objSampleResult.Testparameter.TestMethod != null)
                                    //        {
                                    //            objPT.Test = uow.GetObjectByKey<TestMethod>(objSampleResult.Testparameter.TestMethod.Oid);
                                    //        }
                                    //        if (objSampleResult != null && objSampleResult.Testparameter != null && objSampleResult.Testparameter.TestMethod != null && objSampleResult.Testparameter.TestMethod.MethodName != null)
                                    //        {
                                    //            objPT.Method = uow.GetObjectByKey<Method>(objSampleResult.Testparameter.TestMethod.MethodName.Oid);
                                    //        }
                                    //        if (objSampleResult.AnalyzedBy != null)
                                    //        {
                                    //            objPT.Analyst = uow.GetObjectByKey<Employee>(objSampleResult.AnalyzedBy.Oid);
                                    //            objPT.DateAnalyzed = objSampleResult.AnalyzedDate;
                                    //        }
                                    //        objPT.Save();
                                    //        uow.CommitChanges();
                                    //    }
                                    //    //else
                                    //    //{
                                    //    //    Objstudylog.JobID = lstobj;
                                    //    //    uow.CommitChanges();
                                    //    //}
                                    //}
                                    //else if (objSampleResult.Samplelogin != null && objSampleResult.Samplelogin.QCCategory != null && objSampleResult.Samplelogin.QCCategory.QCCategoryName == "MDL")
                                    //{
                                    //    Samplecheckin lstobj = uow.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]= ?", objSampleResult.Samplelogin.JobID.Oid));
                                    //    DOC Objstudylog = uow.FindObject<DOC>(CriteriaOperator.Parse("[JobID.Oid]= ?", objSampleResult.Samplelogin.JobID.Oid));
                                    //    if (Objstudylog == null)
                                    //    {
                                    //        DOC objPT = new DOC(uow);
                                    //        objPT.JobID = lstobj;
                                    //        objPT.Status = DOC.DOCstatus.PendingSubmission;
                                    //        if (objSampleResult != null && objSampleResult.Testparameter != null && objSampleResult.Testparameter.TestMethod != null)
                                    //        {
                                    //            objPT.Test = uow.GetObjectByKey<TestMethod>(objSampleResult.Testparameter.TestMethod.Oid);
                                    //        }
                                    //        if (objSampleResult != null && objSampleResult.Testparameter != null && objSampleResult.Testparameter.TestMethod != null && objSampleResult.Testparameter.TestMethod.MethodName != null)
                                    //        {
                                    //            objPT.Method = uow.GetObjectByKey<Method>(objSampleResult.Testparameter.TestMethod.MethodName.Oid);
                                    //        }
                                    //        if (objSampleResult.AnalyzedBy != null)
                                    //        {
                                    //            objPT.Analyst = uow.GetObjectByKey<Employee>(objSampleResult.AnalyzedBy.Oid);
                                    //            objPT.DateAnalyzed = objSampleResult.AnalyzedDate;
                                    //        }
                                    //        objPT.Save();
                                    //        uow.CommitChanges();
                                    //    }
                                    //}
                                    //else if (objSampleResult.Samplelogin != null && objSampleResult.Samplelogin.QCCategory != null && objSampleResult.Samplelogin.QCCategory.QCCategoryName == "PT")
                                    //{
                                    //    Samplecheckin lstobj = uow.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]= ?", objSampleResult.Samplelogin.JobID.Oid));
                                    //    DOC Objstudylog = uow.FindObject<DOC>(CriteriaOperator.Parse("[JobID.Oid]= ?", objSampleResult.Samplelogin.JobID.Oid));
                                    //    if (Objstudylog == null)
                                    //    {
                                    //        DOC objPT = new DOC(uow);
                                    //        objPT.JobID = lstobj;
                                    //        objPT.Status = DOC.DOCstatus.PendingSubmission;
                                    //        if (objSampleResult != null && objSampleResult.Testparameter != null && objSampleResult.Testparameter.TestMethod != null)
                                    //        {
                                    //            objPT.Test = uow.GetObjectByKey<TestMethod>(objSampleResult.Testparameter.TestMethod.Oid);
                                    //        }
                                    //        if (objSampleResult != null && objSampleResult.Testparameter != null && objSampleResult.Testparameter.TestMethod != null && objSampleResult.Testparameter.TestMethod.MethodName != null)
                                    //        {
                                    //            objPT.Method = uow.GetObjectByKey<Method>(objSampleResult.Testparameter.TestMethod.MethodName.Oid);
                                    //        }
                                    //        if (objSampleResult.AnalyzedBy != null)
                                    //        {
                                    //            objPT.Analyst = uow.GetObjectByKey<Employee>(objSampleResult.AnalyzedBy.Oid);
                                    //            objPT.DateAnalyzed = objSampleResult.AnalyzedDate;
                                    //        }
                                    //        objPT.Save();
                                    //        uow.CommitChanges();
                                    //    }
                                    //}

                                //}
                            }
                            View.ObjectSpace.CommitChanges();
                            viewItem.InnerView.ObjectSpace.Refresh();
                        }
                    }
                    if (selectedcount > 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "resultapprove"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        //NavigationItemCountRefresh();
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Rollback_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                bool result = true;
                LstSampleParameters = new List<SampleParameter>();
                intselectedcount = 0;
                inttotalcount = 0;
                bool IsPTSample = true;
                if (View is DashboardView)
                {
                    if (((DashboardViewItem)((DashboardView)View).Items[0]).InnerView == null)
                        ((DashboardViewItem)((DashboardView)View).Items[0]).CreateControl();
                    if (((DashboardViewItem)((DashboardView)View).Items[1]).InnerView == null)
                        ((DashboardViewItem)((DashboardView)View).Items[1]).CreateControl();
                    if (((DashboardViewItem)((DashboardView)View).Items[0]).InnerView.SelectedObjects.Count + ((DashboardViewItem)((DashboardView)View).Items[1]).InnerView.SelectedObjects.Count > 0)
                    {
                        foreach (DashboardViewItem viewItem in ((DashboardView)View).Items)
                        {
                            if (result)
                            {
                                result = CheckReported(viewItem.InnerView);
                                if (IsPTSample)
                                {
                                    SampleParameter objParam = viewItem.InnerView.SelectedObjects.Cast<SampleParameter>().FirstOrDefault(i => i.Samplelogin != null && i.Samplelogin.JobID != null);
                                    if (objParam != null && objParam.Samplelogin.JobID.ProjectCategory != null && (objParam.Samplelogin.JobID.ProjectCategory.CategoryName == "PT" || objParam.Samplelogin.JobID.ProjectCategory.CategoryName == "DOC" || objParam.Samplelogin.JobID.ProjectCategory.CategoryName == "MDL"))
                                    {
                                        Application.ShowViewStrategy.ShowMessage("PTSample cannot be rollback.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }
                                    else
                                    {
                                        IsPTSample = false;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (result)
                        {
                            IObjectSpace Popupos = Application.CreateObjectSpace();
                            object objToShow = Popupos.CreateObject(typeof(SDMSRollback));
                            DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                            CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                            ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView.Caption = Application.MainWindow.View.Caption + " Rollback";
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += Dc_Accepting_Rstvaldapprov;
                            dc.ViewClosed += Dc_ViewClosed;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                            // result = ProcessRollback();
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        result = CheckReported(View);
                        if (result)
                        {
                            IObjectSpace Popupos = Application.CreateObjectSpace();
                            object objToShow = Popupos.CreateObject(typeof(SDMSRollback));
                            DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                            CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                            ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView.Caption = Application.MainWindow.View.Caption + " Rollback";
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += Dc_Accepting_Rstview;
                            dc.ViewClosed += Dc_ViewClosed;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            //result = ProcessRollback();
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
                ////if (result)
                ////{
                ////    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                ////    Viewrefresh();
                ////}
                ////else 
                if (!result && intselectedcount == inttotalcount)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbackfailed"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private bool CheckReported(DevExpress.ExpressApp.View curview)
        {
            try
            {
                ////if (objQPInfo.SelectMode == QueryMode.QC)
                ////{
                ////    intselectedcount += ((ListView)curview).SelectedObjects.Count;
                ////    inttotalcount += ((ListView)curview).CollectionSource.List.Count;
                ////}
                foreach (SampleParameter objSample in ((ListView)curview).SelectedObjects)
                {
                    if (!LstSampleParameters.Contains(objSample))
                    {
                        LstSampleParameters.Add(objSample);
                        if (objSample.Status == Samplestatus.Reported)
                        {
                            return false;
                        }
                    }
                    //if (objSample.ABID == null)
                    //{
                    //    if (!LstSampleParameters.Contains(objSample))
                    //    {
                    //        LstSampleParameters.Add(objSample);
                    //        if (objSample.Status == Samplestatus.Reported)
                    //        {
                    //            return false;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    //if (LstSampleParameters.Where(a => a.UQABID.Oid == objSample.UQABID.Oid).Count() == 0)
                    //    //{
                    //    //    LstSampleParameters.Add(objSample);
                    //    //    int intreportedcount = curview.ObjectSpace.GetObjects<SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID] = ? and [uqSampleParameterID.Status] ='Reported'", objSample.UQABID.Oid)).Count;
                    //    //    if (intreportedcount > 0)
                    //    //    {
                    //    //        return false;
                    //    //    }
                    //    //}
                    //    if(LstSampleParameters.Any(i=>i.UQABID !=null))
                    //    {
                    //        if (LstSampleParameters.Where(a => a.UQABID.Oid == objSample.UQABID.Oid).Count() >= 0)
                    //        {
                    //            return false;
                    //        }
                    //    }
                    //}
                }
                return true;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return false;
            }
        }

        private void Dc_ViewClosed(object sender, EventArgs e)
        {
            try
            {
                Application.MainWindow.View.ObjectSpace.Refresh();
                Viewrefresh();
                resultRollback.StrResultRollback = string.Empty;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Accepting_Rstview(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                bool result = true;
                string strreason = ((SDMSRollback)e.AcceptActionArgs.CurrentObject).PopupRollBackReason;
                if (!string.IsNullOrEmpty(strreason))
                {
                    result = CheckReported(View);
                    if (result)
                    {
                        result = ProcessRollback(strreason);
                        if (result)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbackempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Dc_Accepting_Rstvaldapprov(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                bool result = true;
                string strreason = ((SDMSRollback)e.AcceptActionArgs.CurrentObject).PopupRollBackReason;
                if (!string.IsNullOrEmpty(strreason))
                {
                    foreach (DashboardViewItem viewItem in ((DashboardView)View).Items)
                    {
                        if (result)
                        {
                            result = CheckReported(viewItem.InnerView);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (result)
                    {
                        result = ProcessRollback(strreason);
                        if (result)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbackempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private bool ProcessRollback(string strreason)
        {
            try
            {
                if (objQPInfo != null && /*objQPInfo.SelectMode == QueryMode.QC &&*/ intselectedcount != inttotalcount && LstSampleParameters != null && LstSampleParameters.First() != null && LstSampleParameters.First().UQABID != null)
                {
                    ////WebWindow.CurrentRequestWindow.RegisterClientScript("Rollbackstat", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('Entire ABID data will be rollback. Do you want to continue?'); openconfirm = openconfirm + '|' + '" + LstSampleParameters.First().UQABID.AnalyticalBatchID + "'; {0}", ResultCBM.CallbackManager.GetScript("Rollbackstat", "openconfirm")));
                    ////return false;
                }
                else
                {
                    foreach (SampleParameter sample in LstSampleParameters)
                    {
                        if (sample.ABID == null)
                        {
                            RollBackSample(sample, strreason);
                        }
                        else if (sample.UQABID != null && sample.UQABID.AnalyticalBatchID != null)
                        {
                            resultRollback.StrResultRollback = strreason;
                            RollBackQC(sample.UQABID.AnalyticalBatchID, strreason);
                            RollBackSample(sample, strreason);
                        }
                    }
                   
                    

                }
                ObjectSpace.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return false;
            }
        }

        private void RollBackSample(SampleParameter sample, string strreason)
        {
            try
            {
                SampleParameter Sample = ObjectSpace.GetObjectByKey<SampleParameter>(sample.Oid);
                if (Sample != null)
                {
                    Sample.AnalyzedDate = null;
                    Sample.AnalyzedBy = null;
                    Sample.ValidatedDate = null;
                    Sample.ValidatedBy = null;
                    Sample.ApprovedDate = null;
                    Sample.ApprovedBy = null;
                    Sample.EBFValidatedDate = null;
                    Sample.EBFValidatedBy = null;
                    Sample.EBFAnalyzedDate = null;
                    Sample.EBFAnalyzedBy = null;
                    Sample.EBFApprovedDate = null;
                    Sample.EBFApprovedBy = null;
                    Sample.Status = Samplestatus.PendingEntry;
                    Sample.OSSync = true;
                    Sample.Rollback = strreason;
                    if (Sample.DOCDetail != null && Sample.DOCDetail.DOC != null)
                    {
                        DOC objDOC = ObjectSpace.GetObjectByKey<DOC>(Sample.DOCDetail.DOC.Oid);
                        DOC objDOCDetail = ObjectSpace.GetObjectByKey<DOC>(Sample.DOCDetail.Oid);
                        Sample.DOCDetail = null;
                        if (objDOCDetail != null)
                        {
                            ObjectSpace.Delete(objDOCDetail);
                        }
                        List<DOCDetails> lstDOC = ObjectSpace.GetObjects<DOCDetails>(CriteriaOperator.Parse("[DOC]=?", objDOC)).ToList();
                        if (lstDOC.Count > 0)
                        {
                            List<SampleParameter> lstsamplepar = ObjectSpace.GetObjects<SampleParameter>(new InOperator("DOCDetail", lstDOC)).ToList();
                            if (objDOC != null && lstsamplepar.Count == 1)
                            {
                                ObjectSpace.Delete(objDOC);
                            }
                        }
                    }
                    else if (Sample.Samplelogin != null && Sample.Samplelogin.JobID != null)
                    {
                        DOC objDOC = ObjectSpace.FindObject<DOC>(CriteriaOperator.Parse("[JobID]=?", Sample.Samplelogin.JobID));
                        if (objDOC != null)
                        {
                            List<DOCDetails> lstDOC = ObjectSpace.GetObjects<DOCDetails>(CriteriaOperator.Parse("[DOC]=?", objDOC)).ToList();
                            if (lstDOC.Count > 0)
                            {
                                List<SampleParameter> lstsamplepar = ObjectSpace.GetObjects<SampleParameter>(new InOperator("DOCDetail", lstDOC)).ToList();
                                if (objDOC != null && lstsamplepar.Count == 1)
                                {
                                    ObjectSpace.Delete(objDOC);
                                }
                            }
                            else
                            {
                                ObjectSpace.Delete(objDOC);
                            }
                        }
                    }
                    if (sample.Samplelogin!=null)
                    {
                        IList<Notes> notes = ObjectSpace.GetObjects<Notes>(CriteriaOperator.Parse("[Samplecheckin.Oid] =? AND [NoteSource] <> 'Sample Registration' AND [NoteSource] <> 'QC Batch' AND [NoteSource] <> 'Sample Prepration' ", sample.Samplelogin.JobID.Oid));
                        ObjectSpace.Delete(notes); 
                    }

                }
                //foreach (var jobid in Application.MainWindow.View.SelectedObjects)
                //{
                    if (sample != null && sample.QCBatchID != null && sample.QCBatchID.QCType != null && sample.QCBatchID.QCType.QCTypeName == "Sample")
                    {
                        IList<SampleParameter> lstsmpl1 = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ?", sample.Samplelogin.JobID.Oid));
                        if (lstsmpl1.Where(i => i.Samplelogin.JobID.Status == SampleRegistrationSignoffStatus.PendingSubmit).Count() == 0 && lstsmpl1.Where(i => i.Status == Samplestatus.PendingEntry).Count() > 0)
                        {
                            if (lstsmpl1.FirstOrDefault(i => i.PrepMethodCount > 0) != null)
                            {
                                StatusDefinition statusDefinition = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Index]=116"));
                                if (statusDefinition != null)
                                {
                                    lstsmpl1.FirstOrDefault().Samplelogin.JobID.Index = statusDefinition;
                                }
                            }
                            else
                            {
                                StatusDefinition statusDefinition = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Index]=110"));
                                if (statusDefinition != null)
                                {
                                    lstsmpl1.FirstOrDefault().Samplelogin.JobID.Index = statusDefinition;
                                }
                            }
                            View.ObjectSpace.CommitChanges();
                        }
                    }
                //}
                

               ObjectSpace.CommitChanges();




            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RollBackQC(string QCBatchID, string strreason)
        {
            try
            {
                //CriteriaOperator criteria = CriteriaOperator.Parse("[uqQCBatchID.QCBatchID] =?", QCBatchID);
                CriteriaOperator criteria = CriteriaOperator.Parse("[uqAnalyticalBatchID.AnalyticalBatchID] =?", QCBatchID);
                IList<SpreadSheetEntry> SDMSSample = ObjectSpace.GetObjects<SpreadSheetEntry>(criteria);
                foreach (SpreadSheetEntry objSampleResult in SDMSSample)
                {
                    objSampleResult.AnalyzedDate = null;
                    objSampleResult.AnalyzedBy = null;
                    objSampleResult.ValidatedDate = null;
                    objSampleResult.ValidatedBy = null;
                    objSampleResult.ApprovedDate = null;
                    objSampleResult.ApprovedBy = null;
                    objSampleResult.ReviewedDate = null;
                    objSampleResult.ReviewedBy = null;
                    objSampleResult.Status = Samplestatus.PendingEntry;
                    objSampleResult.IsComplete = false;
                    objSampleResult.IsExported = false;
                    objSampleResult.VerifiedDate = null;
                    objSampleResult.VerifiedBy = null;
                    if (objSampleResult.uqAnalyticalBatchID != null)
                    {
                        objSampleResult.uqAnalyticalBatchID.Status = 1;
                    }
                    if (objSampleResult.uqSampleParameterID != null)
                    {
                        objSampleResult.uqSampleParameterID.Status = Samplestatus.PendingEntry;
                        objSampleResult.uqSampleParameterID.OSSync = true;
                        objSampleResult.uqSampleParameterID.AnalyzedDate = null;
                        objSampleResult.uqSampleParameterID.AnalyzedBy = null;
                        objSampleResult.uqSampleParameterID.EBFAnalyzedDate = null;
                        objSampleResult.uqSampleParameterID.EBFAnalyzedBy = null;
                        objSampleResult.uqSampleParameterID.ValidatedDate = null;
                        objSampleResult.uqSampleParameterID.ValidatedBy = null;
                        objSampleResult.uqSampleParameterID.EBFValidatedDate = null;
                        objSampleResult.uqSampleParameterID.EBFValidatedBy = null;
                        objSampleResult.uqSampleParameterID.ApprovedDate = null;
                        objSampleResult.uqSampleParameterID.ApprovedBy = null;
                        objSampleResult.uqSampleParameterID.EBFApprovedDate = null;
                        objSampleResult.uqSampleParameterID.EBFApprovedBy = null;
                        objSampleResult.uqSampleParameterID.IsComplete = false;
                        objSampleResult.uqSampleParameterID.IsExported = false;
                        objSampleResult.uqSampleParameterID.Rollback = strreason;
                        if (objSampleResult.uqSampleParameterID.DOCDetail != null && objSampleResult.uqSampleParameterID.DOCDetail.DOC != null && objSampleResult.uqQCTypeID!=null&&objSampleResult.uqQCTypeID.QCTypeName=="LCS")
                        {
                            DOC objDOC = ObjectSpace.GetObjectByKey<DOC>(objSampleResult.uqSampleParameterID.DOCDetail.DOC.Oid);
                            DOC objDOCDetail = ObjectSpace.GetObjectByKey<DOC>(objSampleResult.uqSampleParameterID.DOCDetail.Oid);
                            objSampleResult.uqSampleParameterID.DOCDetail = null;
                            if(objDOCDetail!=null)
                            {
                                ObjectSpace.Delete(objDOCDetail);
                            }
                            //List<DOCDetails> lstDOC = ObjectSpace.GetObjects<DOCDetails>(CriteriaOperator.Parse("[DOC]=?", objDOC)).ToList();
                            //if (lstDOC.Count > 0)
                            //{
                                //List<SampleParameter> lstsamplepar = ObjectSpace.GetObjects<SampleParameter>(new InOperator("DOCDetail", lstDOC)).ToList();
                                if (objDOC != null/* && lstsamplepar.Count == 1*/)
                                {
                                    ObjectSpace.Delete(objDOC);
                                }
                            //}
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

        public void ProcessAction(string parameter)
        {
            try
            {
                if (parameter != string.Empty)
                {
                    string[] paramsplit = parameter.Split('|');
                    ////if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice")
                    ////{
                    ////    if (resultentryinfo.lstresultentry == null)
                    ////    {
                    ////        resultentryinfo.lstresultentry = new List<SampleParameter>();
                    ////    }
                    ////    if (paramsplit[0] == "Selected" && !string.IsNullOrEmpty(paramsplit[1]))
                    ////    {
                    ////        SampleParameter objsmplpara = ((ListView)View).ObjectSpace.FindObject<SampleParameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(paramsplit[1])), true);
                    ////        if (objsmplpara != null)
                    ////        {
                    ////            if (!resultentryinfo.lstresultentry.Contains(objsmplpara))
                    ////            {
                    ////                resultentryinfo.lstresultentry.Add(objsmplpara);
                    ////            }
                    ////        }
                    ////    }
                    ////    else if (paramsplit[0] == "UNSelected" && !string.IsNullOrEmpty(paramsplit[1]))
                    ////    {
                    ////        SampleParameter objsmplpara = ((ListView)View).ObjectSpace.FindObject<SampleParameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(paramsplit[1])), true);
                    ////        if (objsmplpara != null)
                    ////        {
                    ////            if (resultentryinfo.lstresultentry.Contains(objsmplpara))
                    ////            {
                    ////                resultentryinfo.lstresultentry.Add(objsmplpara);
                    ////            }
                    ////        }
                    ////    }
                    ////    else if (paramsplit[0] == "Selectall")
                    ////    {
                    ////        foreach (SampleParameter smplpara in ((ListView)View).CollectionSource.List)
                    ////        {
                    ////            if (!resultentryinfo.lstresultentry.Contains(smplpara))
                    ////            {
                    ////                resultentryinfo.lstresultentry.Add(smplpara);
                    ////            }
                    ////        }
                    ////    }
                    ////    else if (paramsplit[0] == "UNSelectall")
                    ////    {
                    ////        if (resultentryinfo.IsResultEntrySelectionChanged == false)
                    ////        {
                    ////            foreach (SampleParameter smplpara in ((ListView)View).CollectionSource.List)
                    ////            {
                    ////                if (resultentryinfo.lstresultentry.Contains(smplpara))
                    ////                {
                    ////                    resultentryinfo.lstresultentry.Remove(smplpara);
                    ////                }
                    ////            }
                    ////        }
                    ////    }
                    ////}

                    if (bool.TryParse(paramsplit[0], out bool rollbackstat))
                    {
                        if (rollbackstat)
                        {
                            RollBackQC(paramsplit[1], resultRollback.StrResultRollback);
                            ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            Viewrefresh();
                        }
                    }
                    else if (paramsplit[0] == "Result")
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        //WebWindow.CurrentRequestWindow.RegisterStartupScript("clear", "setTimeout(function(){ SamplingAssignment.CancelEdit(); }, 0);");
                        if (gridListEditor != null)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(paramsplit[1]), "Oid");
                            SampleParameter objParam = os.FindObject<SampleParameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objParam != null && objParam.Testparameter.ParameterDefaultResults != null)
                            {

                                CollectionSource cs = new CollectionSource(os, typeof(ResultDefaultValue));
                                cs.Criteria["Filter"] = new InOperator("Oid", objParam.Testparameter.ParameterDefaultResults.Split(';').Select(i => new Guid(i)).ToList());
                                ListView lv = Application.CreateListView("ResultDefaultValue_LookupListView_ResultEntry", cs, false);
                                ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                showViewParameters.CreatedView.Caption = SampleID;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.SaveOnAccept = false;
                                dc.CloseOnCurrentObjectProcessing = false;
                                //dc.AcceptAction.Execute += AcceptAction_Execute;
                                dc.Accepting += Dc_Accepting;
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 1)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                    return;
                }
                else
                {
                    ResultDefaultValue objValue = (ResultDefaultValue)e.AcceptActionArgs.CurrentObject;
                    if (HttpContext.Current.Session["rowid"] != null && objValue != null)
                    {
                        SampleParameter objsample = ((ListView)View).CollectionSource.List.Cast<SampleParameter>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                        if (objsample != null)
                        {
                            objsample.Result = objValue.ResultValue;
                        }

                    }
                    else if (objValue == null)
                    {
                        SampleParameter objsample = ((ListView)View).CollectionSource.List.Cast<SampleParameter>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                        if (objsample != null)
                        {
                            objsample.Result = string.Empty;
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

        //private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        if (e.AcceptActionArgs.SelectedObjects.Count == 0)
        //        {
        //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
        //            e.Cancel = true;
        //            return;
        //        }
        //        else if (e.AcceptActionArgs.SelectedObjects.Count > 1)
        //        {
        //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
        //            e.Cancel = true;
        //            return;
        //        }
        //        else
        //        {
        //            ResultDefaultValue objValue = (ResultDefaultValue)e.CurrentObject;
        //            if (HttpContext.Current.Session["rowid"] != null && objValue != null)
        //            {
        //                SampleParameter objsample = ((ListView)View).CollectionSource.List.Cast<SampleParameter>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
        //                if (objsample != null)
        //                {
        //                    objsample.Result = objValue.ResultValue;
        //                }
        //               ((ListView)View).Refresh();
        //            }
        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void Viewrefresh()
        {
            try
            {
                if (View is DashboardView)
                {
                    foreach (DashboardViewItem viewItem in ((DashboardView)View).Items)
                    {
                        viewItem.InnerView.ObjectSpace.Refresh();
                    }
                }
                else
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView view = nestedFrame.ViewItem.View;
                    foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                    {
                        if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                        {
                            frameContainer.Frame.View.ObjectSpace.Refresh();
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
        #endregion

        #region Events
        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                ASPxGridView gv = gridListEditor.Grid;
                IObjectSpace os = this.ObjectSpace;
                Session CS = ((XPObjectSpace)(os)).Session;
                var selected = gridListEditor.GetSelectedObjects();
                // View.SelectedObjects;
                if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice")
                {
                    if (resultentryinfo.lstresultentry == null)
                    {
                        resultentryinfo.lstresultentry = new List<SampleParameter>();
                    }
                    resultentryinfo.lsttempresultentry = new List<SampleParameter>();
                    resultentryinfo.lsttempresultentry = resultentryinfo.lstresultentry;
                    Employee objEmp = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("Oid=?", SecuritySystem.CurrentUserId));
                    foreach (SampleParameter objsmplpara in ((ListView)View).CollectionSource.List)
                    {
                        if (resultentryinfo.IsResultEntrySelectionChanged == false)
                        {
                            if (selected.Contains(objsmplpara))
                            {
                                if (!resultentryinfo.lstresultentry.Contains(objsmplpara))
                                {
                                    resultentryinfo.lstresultentry.Add(objsmplpara);
                                }
                            }
                            else if (!selected.Contains(objsmplpara))
                            {
                                if (resultentryinfo.lstresultentry.Contains(objsmplpara))
                                {
                                    resultentryinfo.lstresultentry.Remove(objsmplpara);
                                }
                            }
                        }
                        if (selected.Contains(objsmplpara))
                        {
                            //DateTime now = DateTime.Now;
                            //DateTime utcStart = DateTime.SpecifyKind(now, DateTimeKind.Unspecified);
                            if (objsmplpara.AnalyzedDate == null)
                            {
                                //DateTime localNow = TimeZoneInfo.ConvertTimeFromUtc(now, TimeZoneInfo.Local);
                                ////objsmplpara.AnalyzedDate = DateTime.SpecifyKind(DateTime.Parse(DateTime.Now.ToString()), DateTimeKind.Utc); //TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now, objTimeinfo.TimeZone); //DateTime.Now;
                                //objsmplpara.AnalyzedDate = TimeZoneInfo.ConvertTimeFromUtc(now, TimeZoneInfo.Local); //TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now, objTimeinfo.TimeZone); //DateTime.Now;
                                //objsmplpara.AnalyzedDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now, objTimeinfo.TimeZone);
                                //objsmplpara.AnalyzedDate = TimeZoneInfo.ConvertTime(now, objTimeinfo.TimeZone, TimeZoneInfo.Local);
                                //DateTime utcStart = DateTime.SpecifyKind(now, DateTimeKind.Unspecified);
                                //myRecord.UTCStartTime = TimeZoneInfo.ConvertTimeToUtc(utcStart, myTimeZone);
                                //objsmplpara.AnalyzedDate = TimeZoneInfo.ConvertTimeFromUtc(utcStart, objTimeinfo.TimeZone);
                                objsmplpara.AnalyzedDate = DateTime.Now;
                            }
                            if (objsmplpara.AnalyzedBy == null)
                            {
                                objsmplpara.AnalyzedBy = objEmp;
                            }
                            //objsmplpara.AnalyzedBy = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);

                            //objsmplpara.EnteredDate = TimeZoneInfo.ConvertTimeFromUtc(utcStart, objTimeinfo.TimeZone);
                            objsmplpara.EnteredDate = DateTime.Now;
                            objsmplpara.EnteredBy = objEmp;
                            //objsmplpara.EnteredBy = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);

                        }
                        else
                        {
                            ////objsmplpara.AnalyzedDate = null;
                            ////objsmplpara.AnalyzedBy = null;
                            objsmplpara.EnteredDate = null;
                            objsmplpara.EnteredBy = null;
                        }
                    }
                }
                else if (View.Id == "SampleParameter_ListView_Copy_QCResultEntry")
                {
                    //foreach (SampleParameter objSampleResult in ((ListView)View).CollectionSource.List)
                    //{
                    //    if (selected.Contains(objSampleResult))
                    //    {
                    //        objSampleResult.AnalyzedDate = DateTime.Now;
                    //        objSampleResult.AnalyzedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                    //        objSampleResult.EnteredDate = DateTime.Now;
                    //        objSampleResult.EnteredBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                    //    }
                    //    else
                    //    {
                    //        objSampleResult.AnalyzedDate = null;
                    //        objSampleResult.AnalyzedBy = null;
                    //        objSampleResult.EnteredDate = null;
                    //        objSampleResult.EnteredBy = null;
                    //    }
                    //}
                    //View.Refresh();
                }
                else if (View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation")
                {
                    Employee objEmp = CS.FindObject<Employee>(CriteriaOperator.Parse("Oid=?", SecuritySystem.CurrentUserId));
                    //Employee objEmp= CS.getobj<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId)
                    if (objEmp != null)
                    {
                        ((ListView)View).CollectionSource.List.Cast<SampleParameter>().Intersect(selected.Cast<SampleParameter>().ToList()).Where(i => i.ValidatedBy == null).ToList().ForEach(i => { if (i.ValidatedDate == null) i.ValidatedDate = DateTime.Now; if (i.ValidatedBy == null) i.ValidatedBy = objEmp; });
                    }
                    ////((ListView)View).CollectionSource.List.Cast<SampleParameter>().Except(selected.Cast<SampleParameter>().ToList()).ToList().ForEach(i => { if (i.ValidatedDate != null) i.ValidatedDate = null; if (i.ValidatedBy != null) i.ValidatedBy = null; });
                    //foreach (SampleParameter objSampleResult in ((ListView)View).CollectionSource.List)
                    //{
                    //    if (selected.Contains(objSampleResult))
                    //    {
                    //        objSampleResult.ValidatedDate = DateTime.Now;
                    //        objSampleResult.ValidatedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                    //        objSampleResult.AnalyzedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                    //        objSampleResult.AnalyzedDate = DateTime.Now;
                    //    }
                    //    else
                    //    {
                    //        objSampleResult.ValidatedDate = null;
                    //        objSampleResult.ValidatedBy = null;
                    //        objSampleResult.AnalyzedBy = null;
                    //        objSampleResult.AnalyzedDate = null;
                    //    }
                    //}
                    View.Refresh();
                }
                else if (View.Id == "SampleParameter_ListView_Copy_ResultApproval" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval")
                {
                    foreach (SampleParameter objSampleResult in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objSampleResult))
                        {
                            if (objSampleResult.ApprovedDate == null)
                            {
                                objSampleResult.ApprovedDate = DateTime.Now;
                            }
                            if (objSampleResult.ApprovedBy == null)
                            {
                                objSampleResult.ApprovedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                            }
                            if (objSampleResult.AnalyzedBy == null)
                            {
                                objSampleResult.AnalyzedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                            }
                            if (objSampleResult.AnalyzedDate == null)
                            {
                                objSampleResult.AnalyzedDate = DateTime.Now;
                            }
                        }
                        ////else
                        ////{
                        ////    objSampleResult.ApprovedDate = null;
                        ////    objSampleResult.ApprovedBy = null;
                        ////    //objSampleResult.AnalyzedBy = null;
                        ////    //objSampleResult.AnalyzedDate = null;
                        ////}
                    }
                    View.Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                //string rowCell = string.Format("'{0};{1}'", e.VisibleIndex, e.DataColumn.FieldName);
                //string cellClickHandler = RenderHelper.EventCancelBubbleCommand + ((BaseXafPage)WebWindow.CurrentRequestPage).CallbackManager.GetScript("ResultEntryWebViewController", rowCell);
                //e.Cell.Attributes.Add("onclick", cellClickHandler);
                if (e.DataColumn.FieldName != "Result") return;
                ASPxGridView grid = sender as ASPxGridView;
                bool value = Convert.ToBoolean(grid.GetRowValues(e.VisibleIndex, "IsResultDefaultValue"));
                if (value)
                {
                    e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'ResultDefaultValue', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        void gridView_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;

                if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice")
                {
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                }
                if (View.Id != "ResultDefaultValue_LookupListView_ResultEntry")
                {
                    gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                    gridView.VisibleColumns[1].FixedStyle = GridViewColumnFixedStyle.Left;
                    gridView.VisibleColumns[2].FixedStyle = GridViewColumnFixedStyle.Left;
                }
                else
                {
                    var selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(i => i.ShowSelectCheckbox).FirstOrDefault();
                    if (selectionBoxColumn != null)
                    {
                        selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        private void ResultDelete_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry" || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice"))
                {
                    if (resultentryinfo.lstresultentry != null && resultentryinfo.lstresultentry.Count > 0)///(View.SelectedObjects.Count > 0)
                    {
                        IList<SampleParameter> lstsmplpara = View.ObjectSpace.GetObjects<SampleParameter>(new InOperator("Oid", resultentryinfo.lstresultentry.Select(i=>i.Oid)));
                        if (lstsmplpara != null && lstsmplpara.Count > 0)
                        {
                            lstsmplpara.ToList().ForEach(i => { i.Result = null; });
                            lstsmplpara.ToList().ForEach(i => { i.ResultNumeric = null; });
                            lstsmplpara.ToList().ForEach(i => { i.AnalyzedBy = null; i.AnalyzedDate = null; });
                        }
                        ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                        View.ObjectSpace.CommitChanges();
                        View.Refresh();
                        //foreach (SampleParameter objsp in View.SelectedObjects)
                        //{
                        //    if (objsp != null)
                        //    {
                        //        if (objsp.Result != null && objsp.Result.Length > 0)
                        //        {
                        //            objsp.Result = null;
                        //            objsp.ResultNumeric = null;
                        //            ObjectSpace.CommitChanges();
                        //        }
                        //    }
                        //}
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "resultdelete"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        //ObjectSpace.Refresh();
                        //ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                        //foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                        //{
                        //    if (parent.Id == "Reporting")
                        //    {
                        //        foreach (ChoiceActionItem child in parent.Items)
                        //        {
                        //            if (child.Id == "Custom Reporting")
                        //            {
                        //                int count = 0;
                        //                IObjectSpace objSpace = Application.CreateObjectSpace();
                        //                using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                        //                {
                        //                    lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingReporting' And [SignOff] = True And Not IsNullOrEmpty([Samplelogin.JobID.JobID])");
                        //                    lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                        //                    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        //                    List<object> jobid = new List<object>();
                        //                    if (lstview != null)
                        //                    {
                        //                        foreach (ViewRecord rec in lstview)
                        //                            jobid.Add(rec["Toid"]);
                        //                    }
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
                        //                break;
                        //            }
                        //        }
                        //        break;
                        //    }
                        //}
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
                else if (View != null && View is DashboardView)
                {
                    int selectedcount = 0;
                    foreach (DashboardViewItem viewItem in ((DashboardView)View).Items)
                    {
                        if (viewItem.InnerView is ListView && viewItem.InnerView.SelectedObjects.Count > 0)
                        {
                            selectedcount += viewItem.InnerView.SelectedObjects.Count;
                            ((ASPxGridListEditor)((ListView)viewItem.InnerView).Editor).Grid.UpdateEdit();
                            foreach (SampleParameter objsp in viewItem.InnerView.SelectedObjects)
                            {
                                if (objsp != null)
                                {
                                    if (objsp.Result != null && objsp.Result.Length > 0)
                                    {
                                        objsp.Result = null;
                                        objsp.ResultNumeric = null;
                                        objsp.AnalyzedBy = null;
                                        objsp.AnalyzedDate = null;
                                        objsp.EnteredBy = null;
                                        objsp.EnteredDate = null;
                                        viewItem.InnerView.ObjectSpace.CommitChanges();
                                    }
                                }
                            }
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "resultdelete"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            viewItem.InnerView.ObjectSpace.Refresh();
                        }
                    }
                    if (selectedcount == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                        foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                        {
                            if (parent.Id == "Reporting")
                            {
                                foreach (ChoiceActionItem child in parent.Items)
                                {
                                    if (child.Id == "Custom Reporting")
                                    {
                                        int count = 0;
                                        IObjectSpace objSpace = Application.CreateObjectSpace();
                                        using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                                        {
                                            lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingReporting' And [SignOff] = True And Not IsNullOrEmpty([Samplelogin.JobID.JobID])");
                                            lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                            List<object> jobid = new List<object>();
                                            if (lstview != null)
                                            {
                                                foreach (ViewRecord rec in lstview)
                                                    jobid.Add(rec["Toid"]);
                                            }
                                            count = jobid.Count;
                                        }
                                        var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                        if (count > 0)
                                        {
                                            child.Caption = cap[0] + " (" + count + ")";
                                        }
                                        else
                                        {
                                            child.Caption = cap[0];
                                        }
                                        break;
                                    }
                                }
                                break;
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

        private void GridView_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {

            try
            {
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    //CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse("Oid is null"));
                    if (objLanguage.strcurlanguage != "En")
                    {
                        e.Items.Add("复制到所有单元格", "CopyToAllCell");
                    }
                    else
                    {
                        e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    }
                    GridViewContextMenuItem Edititem = e.Items.FindByName("EditRow");
                    if (Edititem != null)
                        Edititem.Visible = false;
                    GridViewContextMenuItem item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";//"navigation_home_16x16";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SampleRegistration_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "SampleParameter_ListView_Copy_ResultEntry")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        foreach (SampleParameter objSample in View.SelectedObjects)
                        {
                            SRInfo.strJobID = objSample.Samplelogin.JobID.JobID;
                            objSLInfo.focusedJobID = objSample.Samplelogin.JobID.JobID;
                            DashboardView dv = Application.CreateDashboardView(Application.CreateObjectSpace(), "SampleRegistration", false);
                            if (Frame is NestedFrame)
                            {
                                if (SRInfo.ResultEntryFrame != null && SRInfo.ResultEntryFrame is WebWindow)
                                {
                                    SRInfo.ResultEntryFrame.SetView(dv);
                                }
                            }
                            else
                            {
                                Frame.SetView(dv);
                            }
                            break;
                        }
                    }
                    else if (View.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ExportResulyEntry_Execute(object sender, EventArgs e)
        {
            ////try
            ////{
            ////    string selectedPath = string.Empty;
            ////    Thread t = new Thread((ThreadStart)(() =>
            ////    {
            ////        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            ////        saveFileDialog1.FileName = "ResultEntry";
            ////        //saveFileDialog1.Filter = "Execl files (*.xls)|*.xls";
            ////        saveFileDialog1.Filter = "Excel Files (*.xlsx)|*.xlsx";
            ////        saveFileDialog1.FilterIndex = 2;
            ////        saveFileDialog1.RestoreDirectory = true;

            ////        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            ////        {
            ////            selectedPath = saveFileDialog1.FileName;
            ////        }
            ////    }));
            ////    t.SetApartmentState(ApartmentState.STA);
            ////    t.Start();
            ////    t.Join();

            ////    DashboardViewItem lvresultentry = null;
            ////    DashboardViewItem lvqcresultentry = null; // as DashboardViewItem;
            ////    foreach (DashboardViewItem viewItem in ((DashboardView)View).Items)
            ////    {
            ////        if (viewItem.InnerView is ListView)
            ////        {
            ////            lvresultentry = ((DashboardView)View).FindItem("ResultEntry") as DashboardViewItem;
            ////            lvqcresultentry = ((DashboardView)View).FindItem("QCResultEntry") as DashboardViewItem;
            ////        }
            ////    }
            ////    string strspikeamount = string.Empty;
            ////    string strspikeamountunit = string.Empty;
            ////    string strqcbatchid = string.Empty;
            ////    string strsyssamplecode = string.Empty;
            ////    string strclientid = string.Empty;
            ////    string strtest = string.Empty;
            ////    string strparameter = string.Empty;
            ////    string strnumericresult = string.Empty;
            ////    string strresult = string.Empty;
            ////    string strunits = string.Empty;
            ////    string strrptlimit = string.Empty;
            ////    string strmdl = string.Empty;
            ////    string strrec = string.Empty;
            ////    string strrpd = string.Empty;
            ////    string strdf = string.Empty;
            ////    string dtanalyzeddate = string.Empty;
            ////    string stranalyzedby = string.Empty;
            ////    string strstatus = string.Empty;
            ////    string strmatrix = string.Empty;
            ////    string strmethod = string.Empty;

            ////    Workbook wb = new Workbook();

            ////    if (wb.Worksheets.Count == 0)
            ////    {
            ////        wb.Worksheets.Add();
            ////        wb.Worksheets.Add();
            ////    }
            ////    else if (wb.Worksheets.Count == 1)
            ////    {
            ////        wb.Worksheets.Add();
            ////    }
            ////    Worksheet worksheet0 = wb.Worksheets[0];
            ////    Worksheet worksheet1 = wb.Worksheets[1];
            ////    worksheet0.Name = "SampleResult";
            ////    worksheet1.Name = "QCSampleResult";
            ////    DataTable dtsht1 = new DataTable();
            ////    DataTable dtsht2 = new DataTable();

            ////    if (lvresultentry != null && lvresultentry.InnerView != null)
            ////    {
            ////        ListView lstresultentry = lvresultentry.InnerView as ListView;

            ////        if (lstresultentry != null)
            ////        {
            ////            foreach (IModelColumn column in lstresultentry.Model.Columns)
            ////            {
            ////                if (column.Index >= 0)
            ////                {
            ////                    dtsht1.Columns.Add(column.Caption);
            ////                }
            ////            }
            ////            IList<SampleParameter> gridresultentry = lstresultentry.CollectionSource.List.Cast<SampleParameter>().ToList();
            ////            if (gridresultentry != null && gridresultentry.Count > 0)
            ////            {
            ////                foreach (SampleParameter objsmlpara in gridresultentry.ToList())
            ////                {
            ////                    if (objsmlpara.QCBatchID != null)
            ////                    {
            ////                        strsyssamplecode = objsmlpara.QCBatchID.SYSSamplecode;
            ////                    }
            ////                    if (objsmlpara.Samplelogin != null)
            ////                    {
            ////                        strclientid = objsmlpara.Samplelogin.ClientSampleID;
            ////                    }
            ////                    if (objsmlpara.Testparameter != null && objsmlpara.Testparameter.TestMethod != null)
            ////                    {
            ////                        strtest = objsmlpara.Testparameter.TestMethod.TestName;
            ////                    }
            ////                    if (objsmlpara.Testparameter != null && objsmlpara.Testparameter.Parameter != null)
            ////                    {
            ////                        strparameter = objsmlpara.Testparameter.Parameter.ParameterName;
            ////                    }
            ////                    if (objsmlpara.Units != null)
            ////                    {
            ////                        strunits = objsmlpara.Units.UnitName;
            ////                    }
            ////                    if (objsmlpara.AnalyzedDate != null)
            ////                    {
            ////                        dtanalyzeddate = objsmlpara.AnalyzedDate.ToString();
            ////                    }
            ////                    if (objsmlpara.AnalyzedBy != null)
            ////                    {
            ////                        stranalyzedby = objsmlpara.AnalyzedBy.FullName;
            ////                    }
            ////                    if (objsmlpara.Testparameter != null && objsmlpara.Testparameter.TestMethod != null && objsmlpara.Testparameter.TestMethod.MatrixName != null)
            ////                    {
            ////                        strmatrix = objsmlpara.Testparameter.TestMethod.MatrixName.MatrixName;
            ////                    }
            ////                    if (objsmlpara.Testparameter != null && objsmlpara.Testparameter.TestMethod != null && objsmlpara.Testparameter.TestMethod.MethodName != null)
            ////                    {
            ////                        strmethod = objsmlpara.Testparameter.TestMethod.MethodName.MethodNumber;
            ////                    }

            ////                    strnumericresult = objsmlpara.ResultNumeric;
            ////                    strresult = objsmlpara.Result;
            ////                    strrptlimit = objsmlpara.RptLimit;
            ////                    strmdl = objsmlpara.MDL;
            ////                    strrec = objsmlpara.Rec;
            ////                    strrpd = objsmlpara.RPD;
            ////                    strdf = objsmlpara.DF;


            ////                    strstatus = objsmlpara.Status.ToString();


            ////                    dtsht1.Rows.Add(strsyssamplecode, strclientid, strtest, strparameter, strnumericresult, strresult, strunits, strrptlimit, strmdl,
            ////                        strrec, strrpd, strdf, dtanalyzeddate, stranalyzedby, strstatus, strmatrix, strmethod);
            ////                }
            ////            }
            ////        }

            ////    }
            ////    if (lvqcresultentry != null && lvqcresultentry.InnerView != null)
            ////    {
            ////        strqcbatchid = string.Empty;
            ////        strsyssamplecode = string.Empty;
            ////        strclientid = string.Empty;
            ////        strtest = string.Empty;
            ////        strparameter = string.Empty;
            ////        strnumericresult = string.Empty;
            ////        strresult = string.Empty;
            ////        strunits = string.Empty;
            ////        strrptlimit = string.Empty;
            ////        strmdl = string.Empty;
            ////        strrec = string.Empty;
            ////        strrpd = string.Empty;
            ////        strdf = string.Empty;
            ////        dtanalyzeddate = string.Empty;
            ////        stranalyzedby = string.Empty;
            ////        strstatus = string.Empty;
            ////        strmatrix = string.Empty;
            ////        strmethod = string.Empty;
            ////        strspikeamount = string.Empty;
            ////        strspikeamountunit = string.Empty;
            ////        ListView lstqcresultentry = lvqcresultentry.InnerView as ListView;
            ////        if (lstqcresultentry != null)
            ////        {
            ////            foreach (IModelColumn column in lstqcresultentry.Model.Columns)
            ////            {
            ////                if (column.Index >= 0)
            ////                {
            ////                    dtsht2.Columns.Add(column.Caption);
            ////                }
            ////            }
            ////            IList<SampleParameter> gridresultentry = lstqcresultentry.CollectionSource.List.Cast<SampleParameter>().ToList();
            ////            if (gridresultentry != null && gridresultentry.Count > 0)
            ////            {
            ////                foreach (SampleParameter objsmlpara in gridresultentry.ToList())
            ////                {
            ////                    if (objsmlpara.QCBatchID != null && objsmlpara.QCBatchID.qcseqdetail != null)
            ////                    {
            ////                        strqcbatchid = objsmlpara.QCBatchID.qcseqdetail.AnalyticalBatchID;
            ////                    }
            ////                    if (objsmlpara.QCBatchID != null)
            ////                    {
            ////                        strsyssamplecode = objsmlpara.QCBatchID.SYSSamplecode;
            ////                    }
            ////                    if (objsmlpara.Samplelogin != null)
            ////                    {
            ////                        strclientid = objsmlpara.Samplelogin.ClientSampleID;
            ////                    }
            ////                    if (objsmlpara.Testparameter != null && objsmlpara.Testparameter.TestMethod != null)
            ////                    {
            ////                        strtest = objsmlpara.Testparameter.TestMethod.TestName;
            ////                    }
            ////                    if (objsmlpara.Testparameter != null && objsmlpara.Testparameter.Parameter != null)
            ////                    {
            ////                        strparameter = objsmlpara.Testparameter.Parameter.ParameterName;
            ////                    }
            ////                    if (objsmlpara.Units != null)
            ////                    {
            ////                        strunits = objsmlpara.Units.UnitName;
            ////                    }
            ////                    if (objsmlpara.SpikeAmountUnit != null)
            ////                    {
            ////                        strspikeamountunit = objsmlpara.SpikeAmountUnit.UnitName;
            ////                    }
            ////                    if (objsmlpara.AnalyzedDate != null)
            ////                    {
            ////                        dtanalyzeddate = objsmlpara.AnalyzedDate.ToString();
            ////                    }
            ////                    if (objsmlpara.AnalyzedBy != null)
            ////                    {
            ////                        stranalyzedby = objsmlpara.AnalyzedBy.FullName;
            ////                    }
            ////                    if (objsmlpara.Testparameter != null && objsmlpara.Testparameter.TestMethod != null && objsmlpara.Testparameter.TestMethod.MatrixName != null)
            ////                    {
            ////                        strmatrix = objsmlpara.Testparameter.TestMethod.MatrixName.MatrixName;
            ////                    }
            ////                    if (objsmlpara.Testparameter != null && objsmlpara.Testparameter.TestMethod != null && objsmlpara.Testparameter.TestMethod.MethodName != null)
            ////                    {
            ////                        strmethod = objsmlpara.Testparameter.TestMethod.MethodName.MethodNumber;
            ////                    }

            ////                    strnumericresult = objsmlpara.ResultNumeric;
            ////                    strresult = objsmlpara.Result;
            ////                    strrptlimit = objsmlpara.RptLimit;
            ////                    strmdl = objsmlpara.MDL;
            ////                    strrec = objsmlpara.Rec;
            ////                    strrpd = objsmlpara.RPD;
            ////                    strdf = objsmlpara.DF;
            ////                    strspikeamount = objsmlpara.SpikeAmount.ToString();

            ////                    strstatus = objsmlpara.Status.ToString();


            ////                    dtsht2.Rows.Add(strqcbatchid, strsyssamplecode, strtest, strparameter, strnumericresult, strresult, strunits, strrptlimit, strmdl,
            ////                        strspikeamount, strspikeamountunit, strrec, strrpd, dtanalyzeddate, stranalyzedby, strstatus, strmatrix, strmethod);
            ////                }
            ////            }
            ////        }
            ////    }
            ////    worksheet0.Import(dtsht1, true, 0, 0);
            ////    worksheet1.Import(dtsht2, true, 0, 0);
            ////    wb.SaveDocument(selectedPath);

            ////    FileInfo fileInfo = new FileInfo(selectedPath);

            ////    if (fileInfo.Exists)
            ////    {
            ////        Application.ShowViewStrategy.ShowMessage("File download completed", InformationType.Success, 3000, InformationPosition.Top);
            ////    }
            ////}
            ////catch (Exception ex)
            ////{
            ////    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            ////    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            ////}
        }

        private void ImportFromFileAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace sheetObjectSpace = Application.CreateObjectSpace(typeof(ItemsFileUpload));
                ItemsFileUpload spreadSheet = sheetObjectSpace.CreateObject<ItemsFileUpload>();
                DetailView createdView = Application.CreateDetailView(sheetObjectSpace, spreadSheet);
                createdView.ViewEditMode = ViewEditMode.Edit;
                e.View = createdView;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ImportResultentry_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                ResourceManager rmEnglish = new ResourceManager("Resources.LocalizeResultEntryEnglish", Assembly.Load("App_GlobalResources"));
                ResourceManager rmChinese = new ResourceManager("Resources.LocalizeResultEntryChinese", Assembly.Load("App_GlobalResources"));
                //ItemsFileUpload itemsFile = (ItemsFileUpload)e.AcceptActionArgs.CurrentObject;
                ItemsFileUpload itemsFile = (ItemsFileUpload)e.PopupWindowViewCurrentObject;
                if (itemsFile.InputFile != null)
                {
                    byte[] file = itemsFile.InputFile.Content;
                    string fileExtension = Path.GetExtension(itemsFile.InputFile.FileName);
                    DevExpress.Spreadsheet.Workbook workbook = new DevExpress.Spreadsheet.Workbook();
                    if (fileExtension == ".xlsx")
                    {
                        workbook.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                    }
                    else if (fileExtension == ".xls")
                    {
                        workbook.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xls);
                    }
                    //DevExpress.Spreadsheet.WorksheetCollection worksheets = workbook.Worksheets;



                    if (workbook.Worksheets[0].Name == "SampleResult")
                    {
                        DevExpress.Spreadsheet.Worksheet worksheet = workbook.Worksheets[0];
                        CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                        DataTable dt = worksheet.CreateDataTable(range, true);
                        for (int col = 0; col < range.ColumnCount; col++)
                        {
                            CellValueType cellType = range[0, col].Value.Type;
                            for (int r = 1; r < range.RowCount; r++)
                            {
                                if (cellType != range[r, col].Value.Type)
                                {
                                    dt.Columns[col].DataType = typeof(string);
                                    break;
                                }
                            }
                        }
                        DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
                        exporter.Export();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow row1 = dt.Rows[0];
                            if (row1[0].ToString() == dt.Columns[0].Caption)
                            {
                                row1.Delete();
                                dt.AcceptChanges();
                            }
                            foreach (DataColumn c in dt.Columns)
                                c.ColumnName = c.ColumnName.ToString().Trim();
                        }
                        if (itemsltno.Items == null)
                        {
                            itemsltno.Items = new List<string>();
                        }

                        //objectspace change
                        //IObjectSpace cloneitemObjectSpace = Application.CreateObjectSpace();
                        //CollectionSource source = new CollectionSource(cloneitemObjectSpace, typeof(Items));
                        //source.Criteria["filter"] = CriteriaOperator.Parse("IsNullOrEmpty([ItemCode])");
                        //ListView cloneItemListView = Application.CreateListView("Items_ListView_Copy", source, true);
                        //Frame.SetView(cloneItemListView);

                        //List<Items> lstItems = new List<Items>();
                        foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                        {
                            var isEmpty = row.ItemArray.All(c => c is DBNull);
                            if (!isEmpty)
                            {
                                List<string> errorlist = new List<string>();
                                DateTime dateTime;
                                #region NumericResult
                                if (dt.Columns.Contains(rmChinese.GetString("NumericResult")) && !row.IsNull(rmChinese.GetString("NumericResult")))
                                {
                                    strNumericresults = row[rmChinese.GetString("NumericResult")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("NumericResult")) && !row.IsNull(rmEnglish.GetString("NumericResult")))
                                {
                                    strNumericresults = row[rmEnglish.GetString("NumericResult")].ToString().Trim();
                                }
                                else
                                {
                                    strNumericresults = string.Empty;
                                }
                                #endregion 

                                #region Results
                                if (dt.Columns.Contains(rmChinese.GetString("Result")) && !row.IsNull(rmChinese.GetString("Result")))
                                {
                                    strresult = row[rmChinese.GetString("Result")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("Result")) && !row.IsNull(rmEnglish.GetString("Result")))
                                {
                                    strresult = row[rmEnglish.GetString("Result")].ToString().Trim();
                                }
                                else
                                {
                                    strresult = string.Empty;
                                }
                                #endregion

                                #region Units
                                if (dt.Columns.Contains(rmChinese.GetString("Units")) && !row.IsNull(rmChinese.GetString("Units")))
                                {
                                    strunits = row[rmChinese.GetString("Units")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("Units")) && !row.IsNull(rmEnglish.GetString("Units")))
                                {
                                    strunits = row[rmEnglish.GetString("Units")].ToString().Trim();
                                }
                                else
                                {
                                    strunits = string.Empty;
                                }
                                #endregion

                                #region RptLimit
                                if (dt.Columns.Contains(rmChinese.GetString("RptLimit")) && !row.IsNull(rmChinese.GetString("RptLimit")))
                                {
                                    strrptlimit = row[rmChinese.GetString("RptLimit")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("RptLimit")) && !row.IsNull(rmEnglish.GetString("RptLimit")))
                                {
                                    strrptlimit = row[rmEnglish.GetString("RptLimit")].ToString().Trim();
                                }
                                else
                                {
                                    strrptlimit = string.Empty;
                                }
                                #endregion

                                #region MDL
                                if (dt.Columns.Contains(rmChinese.GetString("MDL")) && !row.IsNull(rmChinese.GetString("MDL")))
                                {
                                    strMDL = row[rmChinese.GetString("MDL")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("MDL")) && !row.IsNull(rmEnglish.GetString("MDL")))
                                {
                                    strMDL = row[rmEnglish.GetString("MDL")].ToString().Trim();
                                }
                                else
                                {
                                    strMDL = string.Empty;
                                }
                                #endregion

                                #region DF
                                if (dt.Columns.Contains(rmChinese.GetString("DF")) && !row.IsNull(rmChinese.GetString("DF")))
                                {
                                    strdf = row[rmChinese.GetString("DF")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("DF")) && !row.IsNull(rmEnglish.GetString("DF")))
                                {
                                    strdf = row[rmEnglish.GetString("DF")].ToString().Trim();
                                }
                                else
                                {
                                    strdf = string.Empty;
                                }
                                #endregion

                                #region Syssamplecode
                                if (dt.Columns.Contains(rmChinese.GetString("SysSampleCode")) && !row.IsNull(rmChinese.GetString("SysSampleCode")))
                                {
                                    strsyssamplecode = row[rmChinese.GetString("SysSampleCode")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("SysSampleCode")) && !row.IsNull(rmEnglish.GetString("SysSampleCode")))
                                {
                                    strsyssamplecode = row[rmEnglish.GetString("SysSampleCode")].ToString().Trim();
                                }
                                else
                                {
                                    strsyssamplecode = string.Empty;
                                }
                                #endregion 
                                #region test
                                if (dt.Columns.Contains(rmChinese.GetString("Test")) && !row.IsNull(rmChinese.GetString("Test")))
                                {
                                    strtest = row[rmChinese.GetString("Test")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("Test")) && !row.IsNull(rmEnglish.GetString("Test")))
                                {
                                    strtest = row[rmEnglish.GetString("Test")].ToString().Trim();
                                }
                                else
                                {
                                    strtest = string.Empty;
                                }
                                #endregion 
                                #region parameter
                                if (dt.Columns.Contains(rmChinese.GetString("Parameter")) && !row.IsNull(rmChinese.GetString("Parameter")))
                                {
                                    strparameter = row[rmChinese.GetString("Parameter")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("Parameter")) && !row.IsNull(rmEnglish.GetString("Parameter")))
                                {
                                    strparameter = row[rmEnglish.GetString("Parameter")].ToString().Trim();
                                }
                                else
                                {
                                    strparameter = string.Empty;
                                }
                                #endregion
                                #region Matrixname
                                if (dt.Columns.Contains(rmChinese.GetString("Matrix")) && !row.IsNull(rmChinese.GetString("Matrix")))
                                {
                                    strmatrix = row[rmChinese.GetString("Matrix")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("Matrix")) && !row.IsNull(rmEnglish.GetString("Matrix")))
                                {
                                    strmatrix = row[rmEnglish.GetString("Matrix")].ToString().Trim();
                                }
                                else
                                {
                                    strmatrix = string.Empty;
                                }
                                #endregion
                                #region methodname
                                if (dt.Columns.Contains(rmChinese.GetString("Method")) && !row.IsNull(rmChinese.GetString("Method")))
                                {
                                    strmethod = row[rmChinese.GetString("Method")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("Method")) && !row.IsNull(rmEnglish.GetString("Method")))
                                {
                                    strmethod = row[rmEnglish.GetString("Method")].ToString().Trim();
                                }
                                else
                                {
                                    strmethod = string.Empty;
                                }
                                #endregion
                                #region Rec
                                if (dt.Columns.Contains(rmChinese.GetString("Rec")) && !row.IsNull(rmChinese.GetString("Rec")))
                                {
                                    strrec = row[rmChinese.GetString("Rec")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("Rec")) && !row.IsNull(rmEnglish.GetString("Rec")))
                                {
                                    strrec = row[rmEnglish.GetString("Rec")].ToString().Trim();
                                }
                                else
                                {
                                    strrec = string.Empty;
                                }
                                #endregion
                                #region RPD
                                if (dt.Columns.Contains(rmChinese.GetString("RPD")) && !row.IsNull(rmChinese.GetString("RPD")))
                                {
                                    strrpd = row[rmChinese.GetString("RPD")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("RPD")) && !row.IsNull(rmEnglish.GetString("RPD")))
                                {
                                    strrpd = row[rmEnglish.GetString("RPD")].ToString().Trim();
                                }
                                else
                                {
                                    strrpd = string.Empty;
                                }
                                #endregion

                                IObjectSpace os = Application.CreateObjectSpace();
                                QCBatchSequence checksamplelogin = os.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[SYSSamplecode] = ?", strsyssamplecode));
                                if (checksamplelogin != null && checksamplelogin.SampleID.JobID.JobID == objREInfo.objJobID)
                                {


                                    //QCBatchSequence qcbatchseq = os.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[SYSSampleCode] = ?", strsyssamplecode));
                                    //Testparameter tstparameter = os.FindObject<Testparameter>(CriteriaOperator.Parse("[Parameter.ParameterName] = ? And [TestMethod.TestName] = ? And [Matrix.MatrixName] = ? And [TestMethod.MethodName.MethodName] = ?", strparameter,strtest,strmatrix,strmethod));
                                    if (!string.IsNullOrEmpty(strsyssamplecode) && !string.IsNullOrEmpty(strtest) && !string.IsNullOrEmpty(strparameter) && !string.IsNullOrEmpty(strmatrix) && !string.IsNullOrEmpty(strmethod))
                                    {

                                        //SampleParameter sampleparameter = os.FindObject<SampleParameter>(CriteriaOperator.Parse("[Testparameter.Oid] = ? And [Samplelogin.Oid] = ?", tstparameter.Oid, qcbatchseq.SampleID));
                                        SampleParameter sampleparameter = os.FindObject<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.SYSSamplecode] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.MethodName.MethodName] = ? And [Testparameter.Parameter.ParameterName] = ? And [QCBatchID.QCType.QCTypeName] = 'Sample'", strsyssamplecode, strtest, strmatrix, strmethod, strparameter));
                                        if (sampleparameter != null)
                                        {
                                            sampleparameter.Result = strresult;
                                            sampleparameter.ResultNumeric = strNumericresults;
                                            sampleparameter.RptLimit = strrptlimit;
                                            sampleparameter.MDL = strMDL;
                                            sampleparameter.Rec = strrec;
                                            sampleparameter.RPD = strrpd;
                                            sampleparameter.DF = strdf;
                                            if (strunits != string.Empty)
                                            {
                                                Unit units = os.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]='" + strunits + "'"));
                                                if (units != null)
                                                {
                                                    sampleparameter.Units = units;
                                                }
                                                else
                                                {
                                                    Unit createunit = os.CreateObject<Unit>();
                                                    createunit.UnitName = strunits;
                                                    sampleparameter.Units = createunit;
                                                }
                                            }
                                            os.CommitChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Imported excel file not match current jobid", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                        }
                    }

                    if (workbook.Worksheets[1].Name == "QCSampleResult")
                    {
                        DevExpress.Spreadsheet.Worksheet worksheet1 = workbook.Worksheets[1];
                        CellRange range = worksheet1.Range.FromLTRB(0, 0, worksheet1.Columns.LastUsedIndex, worksheet1.GetUsedRange().BottomRowIndex);
                        DataTable dt = worksheet1.CreateDataTable(range, true);
                        for (int col = 0; col < range.ColumnCount; col++)
                        {
                            CellValueType cellType = range[0, col].Value.Type;
                            for (int r = 1; r < range.RowCount; r++)
                            {
                                if (cellType != range[r, col].Value.Type)
                                {
                                    dt.Columns[col].DataType = typeof(string);
                                    break;
                                }
                            }
                        }
                        DevExpress.Spreadsheet.Export.DataTableExporter exporter1 = worksheet1.CreateDataTableExporter(range, dt, false);
                        exporter1.Export();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow row1 = dt.Rows[0];
                            if (row1[0].ToString() == dt.Columns[0].Caption)
                            {
                                row1.Delete();
                                dt.AcceptChanges();
                            }
                            foreach (DataColumn c in dt.Columns)
                                c.ColumnName = c.ColumnName.ToString().Trim();
                        }
                        foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                        {
                            var isEmpty = row.ItemArray.All(c => c is DBNull);
                            if (!isEmpty)
                            {
                                List<string> errorlist = new List<string>();
                                DateTime dateTime;

                                #region QCbatchID
                                if (dt.Columns.Contains(rmChinese.GetString("QCBatchID")) && !row.IsNull(rmChinese.GetString("QCBatchID")))
                                {
                                    strqcbatchid = row[rmChinese.GetString("QCBatchID")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("QCBatchID")) && !row.IsNull(rmEnglish.GetString("QCBatchID")))
                                {
                                    strqcbatchid = row[rmEnglish.GetString("QCBatchID")].ToString().Trim();
                                }
                                else
                                {
                                    strqcbatchid = string.Empty;
                                }
                                #endregion

                                #region NumericResult
                                if (dt.Columns.Contains(rmChinese.GetString("NumericResult")) && !row.IsNull(rmChinese.GetString("NumericResult")))
                                {
                                    strNumericresults = row[rmChinese.GetString("NumericResult")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("NumericResult")) && !row.IsNull(rmEnglish.GetString("NumericResult")))
                                {
                                    strNumericresults = row[rmEnglish.GetString("NumericResult")].ToString().Trim();
                                }
                                else
                                {
                                    strNumericresults = string.Empty;
                                }
                                #endregion 

                                #region Results
                                if (dt.Columns.Contains(rmChinese.GetString("Result")) && !row.IsNull(rmChinese.GetString("Result")))
                                {
                                    strresult = row[rmChinese.GetString("Result")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("Result")) && !row.IsNull(rmEnglish.GetString("Result")))
                                {
                                    strresult = row[rmEnglish.GetString("Result")].ToString().Trim();
                                }
                                else
                                {
                                    strresult = string.Empty;
                                }
                                #endregion

                                #region Units
                                if (dt.Columns.Contains(rmChinese.GetString("Units")) && !row.IsNull(rmChinese.GetString("Units")))
                                {
                                    strunits = row[rmChinese.GetString("Units")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("Units")) && !row.IsNull(rmEnglish.GetString("Units")))
                                {
                                    strunits = row[rmEnglish.GetString("Units")].ToString().Trim();
                                }
                                else
                                {
                                    strunits = string.Empty;
                                }
                                #endregion

                                #region RptLimit
                                if (dt.Columns.Contains(rmChinese.GetString("RptLimit")) && !row.IsNull(rmChinese.GetString("RptLimit")))
                                {
                                    strrptlimit = row[rmChinese.GetString("RptLimit")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("RptLimit")) && !row.IsNull(rmEnglish.GetString("RptLimit")))
                                {
                                    strrptlimit = row[rmEnglish.GetString("RptLimit")].ToString().Trim();
                                }
                                else
                                {
                                    strrptlimit = string.Empty;
                                }
                                #endregion

                                #region MDL
                                if (dt.Columns.Contains(rmChinese.GetString("MDL")) && !row.IsNull(rmChinese.GetString("MDL")))
                                {
                                    strMDL = row[rmChinese.GetString("MDL")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("MDL")) && !row.IsNull(rmEnglish.GetString("MDL")))
                                {
                                    strMDL = row[rmEnglish.GetString("MDL")].ToString().Trim();
                                }
                                else
                                {
                                    strMDL = string.Empty;
                                }
                                #endregion

                                #region spikeamount
                                if (dt.Columns.Contains(rmChinese.GetString("SpikeAmount")) && !row.IsNull(rmChinese.GetString("SpikeAmount")))
                                {
                                    SpikeAmount = Convert.ToDouble(row[rmChinese.GetString("SpikeAmount")]);
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("SpikeAmount")) && !row.IsNull(rmEnglish.GetString("SpikeAmount")))
                                {
                                    SpikeAmount = Convert.ToDouble(row[rmChinese.GetString("SpikeAmount")]);
                                }
                                #endregion

                                #region Spikeamountunit
                                if (dt.Columns.Contains(rmChinese.GetString("SpikeAmountUnit")) && !row.IsNull(rmChinese.GetString("SpikeAmountUnit")))
                                {
                                    strSpikeAmountunit = row[rmChinese.GetString("SpikeAmountUnit")].ToString().Trim();
                                }
                                else if (dt.Columns.Contains(rmEnglish.GetString("SpikeAmountUnit")) && !row.IsNull(rmEnglish.GetString("SpikeAmountUnit")))
                                {
                                    strSpikeAmountunit = row[rmEnglish.GetString("SpikeAmountUnit")].ToString().Trim();
                                }
                                else
                                {
                                    strdf = string.Empty;
                                }
                                #endregion

                                #region Syssamplecode
                                if (dt.Columns.Contains(rmChinese.GetString("SysSampleCode")) && !row.IsNull(rmChinese.GetString("SysSampleCode")))
                                {
                                    strsyssamplecode = row[rmChinese.GetString("SysSampleCode")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("SysSampleCode")) && !row.IsNull(rmEnglish.GetString("SysSampleCode")))
                                {
                                    strsyssamplecode = row[rmEnglish.GetString("SysSampleCode")].ToString().Trim();
                                }
                                else
                                {
                                    strsyssamplecode = string.Empty;
                                }
                                #endregion 
                                #region test
                                if (dt.Columns.Contains(rmChinese.GetString("Test")) && !row.IsNull(rmChinese.GetString("Test")))
                                {
                                    strtest = row[rmChinese.GetString("Test")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("Test")) && !row.IsNull(rmEnglish.GetString("Test")))
                                {
                                    strtest = row[rmEnglish.GetString("Test")].ToString().Trim();
                                }
                                else
                                {
                                    strtest = string.Empty;
                                }
                                #endregion 
                                #region parameter
                                if (dt.Columns.Contains(rmChinese.GetString("Parameter")) && !row.IsNull(rmChinese.GetString("Parameter")))
                                {
                                    strparameter = row[rmChinese.GetString("Parameter")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("Parameter")) && !row.IsNull(rmEnglish.GetString("Parameter")))
                                {
                                    strparameter = row[rmEnglish.GetString("Parameter")].ToString().Trim();
                                }
                                else
                                {
                                    strparameter = string.Empty;
                                }
                                #endregion
                                #region Matrixname
                                if (dt.Columns.Contains(rmChinese.GetString("Matrix")) && !row.IsNull(rmChinese.GetString("Matrix")))
                                {
                                    strmatrix = row[rmChinese.GetString("Matrix")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("Matrix")) && !row.IsNull(rmEnglish.GetString("Matrix")))
                                {
                                    strmatrix = row[rmEnglish.GetString("Matrix")].ToString().Trim();
                                }
                                else
                                {
                                    strmatrix = string.Empty;
                                }
                                #endregion
                                #region methodname
                                if (dt.Columns.Contains(rmChinese.GetString("Method")) && !row.IsNull(rmChinese.GetString("Method")))
                                {
                                    strmethod = row[rmChinese.GetString("Method")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("Method")) && !row.IsNull(rmEnglish.GetString("Method")))
                                {
                                    strmethod = row[rmEnglish.GetString("Method")].ToString().Trim();
                                }
                                else
                                {
                                    strmethod = string.Empty;
                                }
                                #endregion

                                #region Rec
                                if (dt.Columns.Contains(rmChinese.GetString("Rec")) && !row.IsNull(rmChinese.GetString("Rec")))
                                {
                                    strrec = row[rmChinese.GetString("Rec")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("Rec")) && !row.IsNull(rmEnglish.GetString("Rec")))
                                {
                                    strrec = row[rmEnglish.GetString("Rec")].ToString().Trim();
                                }
                                else
                                {
                                    strrec = string.Empty;
                                }
                                #endregion
                                #region RPD
                                if (dt.Columns.Contains(rmChinese.GetString("RPD")) && !row.IsNull(rmChinese.GetString("RPD")))
                                {
                                    strrpd = row[rmChinese.GetString("RPD")].ToString().Trim();
                                }
                                else
                                if (dt.Columns.Contains(rmEnglish.GetString("RPD")) && !row.IsNull(rmEnglish.GetString("RPD")))
                                {
                                    strrpd = row[rmEnglish.GetString("RPD")].ToString().Trim();
                                }
                                else
                                {
                                    strrpd = string.Empty;
                                }
                                #endregion
                                IObjectSpace os = Application.CreateObjectSpace();

                                QCBatchSequence chksamplelogin = os.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[qcseqdetail.QCBatchID] = ?", strqcbatchid));

                                if (chksamplelogin != null && chksamplelogin.SampleID.JobID.JobID == objREInfo.objJobID)
                                {
                                    //QCBatchSequence qcbatchseq = os.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[SYSSampleCode] = ?", strsyssamplecode));
                                    //Testparameter tstparameter = os.FindObject<Testparameter>(CriteriaOperator.Parse("[Parameter.ParameterName] = ? And[TestMethod.TestName] = ? And[Matrix.MatrixName] = ? And[TestMethod.MethodName.MethodName] = ? ", strparameter,strtest,strmatrix,strmethod));
                                    if (!string.IsNullOrEmpty(strqcbatchid) && !string.IsNullOrEmpty(strsyssamplecode) && !string.IsNullOrEmpty(strtest) && !string.IsNullOrEmpty(strparameter) && !string.IsNullOrEmpty(strmatrix) && !string.IsNullOrEmpty(strmethod))
                                    {

                                        //SampleParameter sampleparameter = os.FindObject<SampleParameter>(CriteriaOperator.Parse("[Testparameter.Oid] = ? And [Samplelogin.Oid] = ?", tstparameter.Oid, qcbatchseq.SampleID));
                                        SampleParameter sampleparameter = os.FindObject<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.SYSSamplecode] = ? And [QCBatchID.qcseqdetail.QCBatchID] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.MethodName.MethodName] = ? And [Testparameter.Parameter.ParameterName] = ? And [QCBatchID.QCType.QCTypeName] <> 'Sample'", strsyssamplecode, strqcbatchid, strtest, strmatrix, strmethod, strparameter));
                                        if (sampleparameter != null)
                                        {
                                            sampleparameter.Result = strresult;
                                            sampleparameter.ResultNumeric = strNumericresults;
                                            sampleparameter.RptLimit = strrptlimit;
                                            sampleparameter.MDL = strMDL;
                                            sampleparameter.Rec = strrec;
                                            sampleparameter.RPD = strrpd;
                                            sampleparameter.SpikeAmount = SpikeAmount;
                                            if (strSpikeAmountunit != string.Empty)
                                            {
                                                Unit spikeAmountunit = os.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]='" + strSpikeAmountunit + "'"));
                                                if (spikeAmountunit != null)
                                                {
                                                    sampleparameter.SpikeAmountUnit = spikeAmountunit;
                                                }
                                                else
                                                {
                                                    Unit createunit = os.CreateObject<Unit>();
                                                    createunit.UnitName = strSpikeAmountunit;
                                                    sampleparameter.Units = createunit;
                                                }
                                            }
                                            if (strunits != string.Empty)
                                            {
                                                Unit units = os.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]='" + strunits + "'"));
                                                if (units != null)
                                                {
                                                    sampleparameter.Units = units;
                                                }
                                                else
                                                {
                                                    Unit createunit = os.CreateObject<Unit>();
                                                    createunit.UnitName = strunits;
                                                    sampleparameter.Units = createunit;
                                                }
                                            }
                                            os.CommitChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Imported excel file not match current jobid", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "uploadfile"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    return;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
