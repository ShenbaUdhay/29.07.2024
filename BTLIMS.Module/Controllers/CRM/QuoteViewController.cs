using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.Base;
using DevExpress.Web;
using DevExpress.XtraReports.UI;
using DynamicDesigner;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Accounts;
//using Modules.BusinessObjects.ContractManagement;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.Quotes;
using Modules.BusinessObjects.TestPricing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Component = Modules.BusinessObjects.Setting.Component;
using Method = Modules.BusinessObjects.Setting.Method;
using Priority = Modules.BusinessObjects.Setting.Priority;

namespace LDM.Module.Controllers.Settings.Quotes
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class QuoteViewController : ViewController, IXafCallbackHandler
    {
        ShowNavigationItemController ShowNavigationController;
        QuotesInfo quotesinfo = new QuotesInfo();
        TATinfo tatinfo = new TATinfo();
        MessageTimer timer = new MessageTimer();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        bool IsDVQuotesCreated = false;
        bool Isgridloaded = false;
        string strfocusedcolumn = string.Empty;
        DataTable dtPriceCode = new DataTable();

        public QuoteViewController()
        {
            InitializeComponent();
            TargetViewId = "QuotesPriceCode;" + "CRMQuotes_AnalysisPricing_ListViewPopup;" + "AnalysisPricing_ListView_Quotes;" + "CRMQuotes_AnalysisPricing_ListView;" + "CRMQuotes_DetailView;" + "CRMQuotes_ListView;" + "CRMQuotes_ListView_Cancel;"
                + "CRMQuotes_ListView_Review;" + "CRMQuotes_ListView_pendingsubmission;" + "CRMQuotes_DetailView_Submitted;" + "CRMQuotes_ListView_PendingReview;" + "CRMQuotes_ListView_Expired;"
                + "AnalysisPricing_LookupListView;" + "CRMQuotes_DetailView_Cancel_Expired;" + "CRMQuotes_DetailView_Reviewed;" + "DummyClass_DetailView_Reasons;" + "ProspectClient_DetailView;" + "Testparameter_LookupListView_Quotes;" + "CRMQuotes_ItemChargePricing_ListView;"
                + "GroupTestMethod_ListView_Quotes;" + "Testparameter_LookupListView_Quotes;" + "QuotesItemChargePrice_ListView_Quotes;" + "TestPriceSurcharge_ListView_Quotes_Popup;"
                + "AnalysisPricing_ListView_QuotesPopup;" + "ItemChargePricing_ListView_Quotes_Popup;" + "CRMQuotes_AnalysisPricing_ListView;" + "CRMQuotes_ListView_DataCenter;" + "CRMQuotes_QuotesItemChargePrice_ListView;" + "TurnAroundTime_ListView_Quotes;"
                + "CRMQuotes_ListView_SubmittedHistory;" + "CRMQuotes_ListView_ReviewHistory;" + "CRMQuotes_DetailView_Submitted_History;" + "VisualMatrix_ListView_Quotes;" + "AnalysisPricing_DetailView_QuotePopup;" + "CRMQuotes_ListView_SubmittedQuotes_History;" + "CRMQuotes_DetailView_Submitted_History;"
                + "CRMQuotes_DetailView_SubmittedQuotes_History;";
            QuoteSubmit.TargetViewId = "CRMQuotes_DetailView;" + "CRMQuotes_ListView_pendingsubmission;";
            QuoteSubmit.TargetObjectsCriteria = "[Status] = 'PendingSubmission' ";
            QuoteReview.TargetViewId = "CRMQuotes_ListView_PendingReview;" + "CRMQuotes_DetailView_Submitted";
            QuoteReactive.TargetViewId = "CRMQuotes_ListView_Cancel;" + "CRMQuotes_ListView_Expired;" + "CRMQuotes_DetailView_Cancel_Expired";
            QuoteReactive.ImageName = "Action_Workflow_Activate";
            QuoteRollback.TargetViewId = "CRMQuotes_ListView_PendingReview;" + "CRMQuotes_DetailView_Submitted;" + "CRMQuotes_ListView_ReviewHistory;" + "CRMQuotes_ListView_SubmittedQuotes_History;"+ "CRMQuotes_DetailView_SubmittedQuotes_History;"+ "CRMQuotes_DetailView_Submitted_History;";
            QuoteRollback.TargetObjectsCriteria = "[Status] = 'PendingReview' or [Status] = 'Reviewed' or [Status] = 'QuoteSubmited'";
            QuotesHistory.TargetViewId = "CRMQuotes_ListView_PendingReview;" + "CRMQuotes_ListView_pendingsubmission;";
            QuotesDateFilter.TargetViewId = "CRMQuotes_ListView_pendingsubmission;" + "CRMQuotes_ListView_PendingReview" + "CRMQuotes_ListView_Review";
            QuoteSaveAs.TargetViewId = "CRMQuotes_ListView_ReviewHistory;"+ "CRMQuotes_ListView_SubmittedQuotes_History;" /*+ "CRMQuotes_ListView_PendingReview;"*/;
            SimpleAction btnAddpricecode = new SimpleAction(this, "btnAddpricecode", PredefinedCategory.Unspecified);
            btnAddpricecode.Caption = "Add";
            btnAddpricecode.Execute += btnAddpricecode_Execute;
            btnAddpricecode.TargetViewId = "CRMQuotes_QuotesItemChargePrice_ListView;" + "CRMQuotes_AnalysisPricing_ListView;";
            btnAddpricecode.ImageName = "Add.png";
            SimpleAction btnRemovepricecode = new SimpleAction(this, "btnRemovepricecode", PredefinedCategory.Unspecified);
            btnRemovepricecode.Caption = "Remove";
            btnRemovepricecode.Execute += btnRemovepricecode_Execute;
            btnRemovepricecode.TargetViewId = "CRMQuotes_QuotesItemChargePrice_ListView;" + "CRMQuotes_AnalysisPricing_ListView;";
            btnRemovepricecode.ImageName = "Remove.png";

            //SimpleAction btnparameterpricecode = new SimpleAction(this, "btnparameterpricecode", PredefinedCategory.Unspecified);
            //btnparameterpricecode.Caption = "Parameter";
            //btnparameterpricecode.Execute += btnparameterpricecode_Execute;
            //btnparameterpricecode.TargetViewId = "AnalysisPricing_ListView_Quotes;" + "CRMQuotes_AnalysisPricing_ListView;";
            //btnparameterpricecode.TargetObjectsCriteria = "[IsGroup] = False And [ChargeType] = 'Parameter'";
            //btnparameterpricecode.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //btnparameterpricecode.ImageName = "mSeting_16x16.png";

            //SimpleAction btngrouptest = new SimpleAction(this, "btngrouptest", PredefinedCategory.Unspecified);
            //btngrouptest.Caption = "Group Test";
            //btngrouptest.Execute += btngrouptest_Execute;
            //btngrouptest.TargetViewId = "AnalysisPricing_ListView_Quotes;" + "CRMQuotes_AnalysisPricing_ListView;";
            //btngrouptest.TargetObjectsCriteria = "[IsGroup] = True";
            //btngrouptest.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //btngrouptest.ImageName = "mSeting_16x16.png";

            //SimpleAction btntierpricedetails = new SimpleAction(this, "btntierpricedetails", PredefinedCategory.Unspecified);
            //btntierpricedetails.Caption = "Price Details";
            //btntierpricedetails.Execute += btntierpricedetails_Execute;
            //btntierpricedetails.TargetViewId = "CRMQuotes_AnalysisPricing_ListView;";
            //btntierpricedetails.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //btntierpricedetails.ImageName = "Business_Dollar.png";

            SimpleAction btnqrpreview = new SimpleAction(this, "btnqrpreview", PredefinedCategory.Unspecified);
            btnqrpreview.Caption = "Preview";
            btnqrpreview.Execute += Btnqrpreview_Execute;
            btnqrpreview.TargetViewId = "CRMQuotes_ListView_pendingsubmission;" + "CRMQuotes_DetailView;" + "CRMQuotes_ListView_PendingReview;" + "CRMQuotes_ListView_Reviewed;"
                                        + "CRMQuotes_ListView_Expired;" + "CRMQuotes_ListView_Cancel;" + "CRMQuotes_ListView_SubmittedHistory;";
            btnqrpreview.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            btnqrpreview.ImageName = "Action_Report_Object_Inplace_Preview";
            btnqrpreview.TargetObjectsCriteria = "Not IsNullOrEmpty([QuoteID])";
            btnqrpreview.Category = "Edit";
            btnqrpreview.Id = "btnqrpreview";

            SimpleAction btnpricecodesave = new SimpleAction(this, "btnpricecodesave", PredefinedCategory.Unspecified);
            btnpricecodesave.Caption = "Save";
            btnpricecodesave.Execute += btnpricecodesave_Execute;
            btnpricecodesave.TargetViewId = "CRMQuotes_QuotesItemChargePrice_ListView;" + "CRMQuotes_AnalysisPricing_ListView;";
            btnpricecodesave.ImageName = "Save_16x16.png";
        }

        private void btnpricecodesave_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView")
                {
                    if (Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "CRMQuotes_DetailView")
                    {
                        Application.MainWindow.View.ObjectSpace.CommitChanges();
                        IObjectSpace os = Application.CreateObjectSpace();
                        CRMQuotes curtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                        if (curtquotes != null)
                        {
                            CRMQuotes objquotes = os.FindObject<CRMQuotes>(CriteriaOperator.Parse("[Oid] = ?", curtquotes.Oid));
                            if (objquotes != null)
                            {
                                ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                                if (gridlisteditor != null && gridlisteditor.Grid != null)
                                {
                                    gridlisteditor.Grid.UpdateEdit();
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage("Save quotes.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            }
                        }
                    }
                    else
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (nestedFrame != null && nestedFrame.ViewItem.View != null && nestedFrame.ViewItem.View.Id == "CRMQuotes_DetailView")
                        {
                            nestedFrame.ViewItem.View.ObjectSpace.CommitChanges();
                            IObjectSpace os = Application.CreateObjectSpace();
                            CRMQuotes curtquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                            if (curtquotes != null)
                            {
                                CRMQuotes objquotes = os.FindObject<CRMQuotes>(CriteriaOperator.Parse("[Oid] = ?", curtquotes.Oid));
                                if (objquotes != null)
                                {
                                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                                    {
                                        gridlisteditor.Grid.UpdateEdit();
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Save quotes.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                }
                            }
                        }
                    }
                    if (((ListView)View).CollectionSource != null && ((ListView)View).CollectionSource.List != null && ((ListView)View).CollectionSource.List.Count > 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Please add pricecode.", InformationType.Warning, 3000, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void btntierpricedetails_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    AnalysisPricing objanalysprice = (AnalysisPricing)View.CurrentObject;
                    //if (objanalysprice != null && objanalysprice.Matrix != null && objanalysprice.Test != null && objanalysprice.Method != null && objanalysprice.Component != null)
                    //{
                    //    DefaultPricing objdefprice = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ? And [IsGroup] = 'No'", objanalysprice.Matrix.MatrixName, objanalysprice.Test.TestName, objanalysprice.Method.MethodNumber, objanalysprice.Component.Components));
                    //    ConstituentPricing objconsprice = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ? And [IsGroup] = False", objanalysprice.Matrix.MatrixName, objanalysprice.Test.TestName, objanalysprice.Method.MethodNumber, objanalysprice.Component.Components));
                    //    //DefaultPricing objdefprice = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ? And [IsGroup] = False", objanalysprice.Test.Oid));
                    //    //ConstituentPricing objconsprice = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ? And [IsGroup] = False", objanalysprice.Test.Oid));
                    //    if (objdefprice != null)
                    //    {
                    //        IObjectSpace os = Application.CreateObjectSpace(typeof(DefaultPricing));
                    //        DetailView dvparameter = Application.CreateDetailView(View.ObjectSpace, "DefaultPricing_DetailView_Quotes", false, objdefprice);
                    //        ShowViewParameters showViewParameters = new ShowViewParameters(dvparameter);
                    //        showViewParameters.CreatedView = dvparameter;
                    //        showViewParameters.Context = TemplateContext.PopupWindow;
                    //        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //        DialogController dc = Application.CreateController<DialogController>();
                    //        dc.SaveOnAccept = false;
                    //        dc.CloseOnCurrentObjectProcessing = false;
                    //        dc.AcceptAction.Active.SetItemValue("OK", false);
                    //        showViewParameters.Controllers.Add(dc);
                    //        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    //    }
                    //    else if (objconsprice != null)
                    //    {
                    //        IObjectSpace os = Application.CreateObjectSpace(typeof(ConstituentPricing));
                    //        DetailView dvparameter = Application.CreateDetailView(View.ObjectSpace, "ConstituentPricing_DetailView_Quotes", false, objconsprice);
                    //        ShowViewParameters showViewParameters = new ShowViewParameters(dvparameter);
                    //        showViewParameters.CreatedView = dvparameter;
                    //        showViewParameters.Context = TemplateContext.PopupWindow;
                    //        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //        DialogController dc = Application.CreateController<DialogController>();
                    //        dc.SaveOnAccept = false;
                    //        dc.CloseOnCurrentObjectProcessing = false;
                    //        dc.AcceptAction.Active.SetItemValue("OK", false);
                    //        showViewParameters.Controllers.Add(dc);
                    //        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    //    }
                    //}
                    //else if (objanalysprice != null && objanalysprice.Matrix != null && objanalysprice.Test != null && objanalysprice.IsGroup == true)
                    //{
                    //    DefaultPricing objdefprice = View.ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ?  And [IsGroup] = 'Yes'", objanalysprice.Matrix.MatrixName, objanalysprice.Test.TestName));
                    //    ConstituentPricing objconsprice = View.ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ?  And [IsGroup] = True", objanalysprice.Matrix.MatrixName, objanalysprice.Test.TestName));
                    //    //DefaultPricing objdefprice = View.ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ? And [IsGroup] = True", objanalysprice.Test.Oid));
                    //    //ConstituentPricing objconsprice = View.ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ? And [IsGroup] = True", objanalysprice.Test.Oid));
                    //    if (objdefprice != null)
                    //    {
                    //        IObjectSpace os = Application.CreateObjectSpace(typeof(DefaultPricing));
                    //        DetailView dvparameter = Application.CreateDetailView(View.ObjectSpace, "DefaultPricing_DetailView_Quotes", false, objdefprice);
                    //        ShowViewParameters showViewParameters = new ShowViewParameters(dvparameter);
                    //        showViewParameters.CreatedView = dvparameter;
                    //        showViewParameters.Context = TemplateContext.PopupWindow;
                    //        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //        DialogController dc = Application.CreateController<DialogController>();
                    //        dc.SaveOnAccept = false;
                    //        dc.CloseOnCurrentObjectProcessing = false;
                    //        dc.AcceptAction.Active.SetItemValue("OK", false);
                    //        showViewParameters.Controllers.Add(dc);
                    //        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    //    }
                    //    else if (objconsprice != null)
                    //    {
                    //        IObjectSpace os = Application.CreateObjectSpace();
                    //        DetailView dvparameter = Application.CreateDetailView(View.ObjectSpace, "ConstituentPricing_DetailView_Quotes", false, objconsprice);
                    //        dvparameter.ViewEditMode = ViewEditMode.View;
                    //        ShowViewParameters showViewParameters = new ShowViewParameters(dvparameter);
                    //        showViewParameters.CreatedView = dvparameter;
                    //        showViewParameters.Context = TemplateContext.PopupWindow;
                    //        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //        DialogController dc = Application.CreateController<DialogController>();
                    //        dc.SaveOnAccept = false;
                    //        dc.CloseOnCurrentObjectProcessing = false;
                    //        dc.AcceptAction.Active.SetItemValue("OK", false);
                    //        showViewParameters.Controllers.Add(dc);
                    //        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    //    }
                    //}
                    //else
                    //{

                    //}
                    if (objanalysprice != null && objanalysprice.IsGroup == false && objanalysprice.Matrix != null && objanalysprice.Test != null && objanalysprice.Method != null && objanalysprice.Component != null)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void btngrouptest_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(GroupTestMethod));
                    AnalysisPricing objcrtanaprice = (AnalysisPricing)View.CurrentObject;
                    if (objcrtanaprice.Matrix != null && objcrtanaprice.Test != null && objcrtanaprice.IsGroup == true)
                    {
                        TestMethod objtm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [IsGroup] = True", objcrtanaprice.Matrix.MatrixName, objcrtanaprice.Test.TestName));
                        if (objtm != null)
                        {
                            cs.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid);
                            ListView lvparameter = Application.CreateListView("GroupTestMethod_ListView_Quotes", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                            showViewParameters.CreatedView = lvparameter;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.AcceptAction.Active.SetItemValue("OK", false);
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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
                string[] connectionstring = objDRDCInfo.WebConfigConn.Split(';');
                objDRDCInfo.LDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                objDRDCInfo.LDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                objDRDCInfo.LDMSQLUserID = connectionstring[2].Split('=').GetValue(1).ToString();
                objDRDCInfo.LDMSQLPassword = connectionstring[3].Split('=').GetValue(1).ToString();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Btnqrpreview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "CRMQuotes_ListView_pendingsubmission" || View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_ListView_PendingReview" || View.Id == "CRMQuotes_ListView_Reviewed"
              || View.Id == "CRMQuotes_ListView_Expired" || View.Id == "CRMQuotes_ListView_Cancel" || View.Id == "CRMQuotes_ListView_DataCenter" || View.Id == "CRMQuotes_ListView_SubmittedHistory")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        if (View.Id == "CRMQuotes_DetailView")
                        {
                            ObjectSpace.CommitChanges();
                        }
                        string stringQuoteID = string.Empty;
                        foreach (CRMQuotes reqId in View.SelectedObjects)
                        {
                            if (stringQuoteID == string.Empty)
                            {
                                stringQuoteID = "'" + reqId.QuoteID + "'";
                            }
                            else
                            {
                                stringQuoteID = stringQuoteID + ",'" + reqId.QuoteID + "'";
                            }

                            if (reqId.AnalysisPricing != null && reqId.AnalysisPricing.Count == 0)
                            {
                                Application.ShowViewStrategy.ShowMessage("Add pricecode and continue.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        string strTempPath = Path.GetTempPath();
                        String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\"));
                        }
                        string strExportedPath = HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\" + timeStamp + ".pdf");
                        XtraReport xtraReport = new XtraReport();

                        objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        SetConnectionString();

                        DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                        //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                        ObjReportingInfo.stringQuoteID = stringQuoteID;
                        Company cmp = ObjectSpace.FindObject<Company>(CriteriaOperator.Parse(""));
                        if (cmp != null && cmp.Logo != null)
                        {
                            GlobalReportSourceCode.strLogo = Convert.ToBase64String(cmp.Logo);
                        }
                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("xtrQuoteReport", ObjReportingInfo, false);
                        //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                        xtraReport.ExportToPdf(strExportedPath);
                        string[] path = strExportedPath.Split('\\');
                        int arrcount = path.Count();
                        int sc = arrcount - 3;
                        string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                        //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open(window.location.href.split('{1}')[0]+'{0}');", OriginalPath, View.Id + "/"));
                        WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }

                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void btnparameterpricecode_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if ((View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView") && View.SelectedObjects.Count == 1)
                {
                    foreach (AnalysisPricing objanaprice in View.SelectedObjects)
                    {
                        AnalysisPricing objaprice = ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", objanaprice.Oid));
                        if (objaprice != null && !string.IsNullOrEmpty(objaprice.ParameterGuid))
                        {
                            string[] strarrparaoid = objanaprice.ParameterGuid.Split(';');
                            foreach (string strparaoid in strarrparaoid)
                            {
                                if (!quotesinfo.lsttempparamsoid.Contains(new Guid(strparaoid.Trim())))
                                {
                                    quotesinfo.lsttempparamsoid.Add(new Guid(strparaoid.Trim()));
                                }
                            }
                            quotesinfo.strtempparamsstatus = string.Empty;
                        }
                        else
                        {
                            quotesinfo.strtempparamsstatus = "Allparams";
                        }
                    }
                    AnalysisPricing objcrtanaprice = (AnalysisPricing)View.CurrentObject;

                    IObjectSpace os = Application.CreateObjectSpace(typeof(Testparameter));
                    Testparameter objcrtdummy = os.CreateObject<Testparameter>();
                    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Testparameter));
                    cs.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objcrtanaprice.Matrix.MatrixName, objcrtanaprice.Test.TestName, objcrtanaprice.Method.MethodNumber, objcrtanaprice.Component.Components);
                    ListView lvparameter = Application.CreateListView("Testparameter_LookupListView_Quotes", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                    showViewParameters.CreatedView = lvparameter;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += Dcparameter_Accepting;
                    //dc.AcceptAction.Executed += Dcparameter_AcceptAction_Executed;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    return;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dcparameter_AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                    if (crtquotes != null)
                    {
                        decimal finalamt = 0;
                        decimal totalprice = 0;
                        decimal disamt = 0;
                        decimal dispr = 0;
                        quotesinfo.IsobjChangedpropertyinQuotes = true;
                        List<AnalysisPricing> lstanalysisprice = View.ObjectSpace.GetObjects<AnalysisPricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ?", crtquotes.Oid)).ToList();
                        foreach (AnalysisPricing objanalysisPricing in lstanalysisprice.ToList())
                        {
                            finalamt = finalamt + objanalysisPricing.FinalAmount;
                            totalprice = totalprice + objanalysisPricing.TotalTierPrice;
                        }
                        //CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                        if (crtquotes != null)
                        {
                            if (finalamt != 0 && totalprice != 0)
                            {
                                disamt = totalprice - finalamt;
                                //disamt = finalamt - totalprice;
                                if (disamt != 0)
                                {
                                    dispr = ((disamt) / totalprice) * 100;
                                }
                                //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                //disamt = finalamt - totalprice;

                                quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                            }
                            else if (finalamt == 0 && totalprice == 0)
                            {
                                crtquotes.DetailedAmount = 0;
                                crtquotes.TotalAmount = 0;
                                crtquotes.Discount = 0;
                                crtquotes.DiscountAmount = 0;
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

        private void btnRemovepricecode_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    if (quotesinfo.lsttempAnalysisPricing == null)
                    {
                        quotesinfo.lsttempAnalysisPricing = new List<AnalysisPricing>();
                    }
                    if (View.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (Application.MainWindow.View.Id == "CRMQuotes_DetailView" || Application.MainWindow.View.Id == "CRMQuotes_DetailView_Submitted_History")
                        {
                            CRMQuotes crtquotesobj = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                            foreach (AnalysisPricing objanapricing in View.SelectedObjects)
                            {
                                ((ListView)View).CollectionSource.Remove(objanapricing);
                                View.ObjectSpace.Delete(View.ObjectSpace.GetObject(objanapricing));
                                //quotesinfo.lsttempAnalysisPricing.Remove(objanapricing);

                            }
                            //View.ObjectSpace.CommitChanges();
                            //objectSpace.CommitChanges();
                            int sortno = 0;
                            foreach (AnalysisPricing objap in ((ListView)View).CollectionSource.List.Cast<AnalysisPricing>().OrderBy(i => i.Test))
                            {
                                objap.Sort = Convert.ToUInt16(sortno + 1);
                                sortno++;
                            }
                            quotesinfo.IsobjChangedpropertyinQuotes = true;
                            decimal finalamt = 0;
                            decimal totalprice = 0;
                            decimal disamt = 0;
                            decimal dispr = 0;
                            bool isTestnull = false;
                            ListPropertyEditor lstitemprice = ((DetailView)Application.MainWindow.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                            ListPropertyEditor lstAnalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                            if (lstitemprice != null)
                            {
                                if (lstitemprice.ListView == null)
                                {
                                    lstitemprice.CreateControl();
                                }
                                if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                {
                                    finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.FinalAmount);
                                    //totalprice = totalprice + lstitemprice.Select(i => i.UnitPrice * i.Qty).Sum(i => i);
                                    totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.UnitPrice * i.Qty);
                                }
                            }
                            if (lstAnalysisprice != null)
                            {
                                if (lstAnalysisprice.ListView == null)
                                {
                                    lstAnalysisprice.CreateControl();
                                }
                                if (lstAnalysisprice.ListView.CollectionSource.GetCount() > 0)
                                {
                                    finalamt = finalamt + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.FinalAmount);
                                    totalprice = totalprice + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.TotalTierPrice * i.Qty);
                                }
                                //finalamt = finalamt + objanapricing.FinalAmount;
                                //totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                            }
                            quotesinfo.IsTabDiscountChanged = true;
                            CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                            if (crtquotes != null)
                            {
                                if (finalamt != 0 && totalprice != 0)
                                {
                                    disamt = totalprice - finalamt;
                                    //disamt = finalamt - totalprice;
                                    if (disamt != 0)
                                    {
                                        dispr = ((disamt) / totalprice) * 100;
                                    }
                                    //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                    //disamt = finalamt - totalprice;
                                    quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                }
                                else if (finalamt == 0 && totalprice == 0)
                                {
                                    crtquotes.DetailedAmount = 0;
                                    crtquotes.TotalAmount = 0;
                                    crtquotes.Discount = 0;
                                    crtquotes.DiscountAmount = 0;
                                    crtquotes.QuotedAmount = 0;
                                    crtquotes.IsGobalDiscount = true;
                                    quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                }
                            }
                            CRMQuotes objquotes = Application.MainWindow.View.ObjectSpace.FindObject<CRMQuotes>(CriteriaOperator.Parse("[Oid] = ?", crtquotes.Oid));
                            if (objquotes != null)
                            {
                                objquotes.DetailedAmount = Math.Round(totalprice, 2);
                                objquotes.TotalAmount = Math.Round(finalamt, 2);
                                objquotes.QuotedAmount = Math.Round(finalamt, 2);
                                objquotes.Discount = Math.Round(dispr, 2);
                                objquotes.DiscountAmount = Math.Round(disamt, 2);
                                crtquotes.IsGobalDiscount = true;
                            }
                            //objectSpace.CommitChanges();
                            Application.MainWindow.View.Refresh();
                        }
                        else if (nestedFrame != null && nestedFrame.ViewItem.View != null && (nestedFrame.ViewItem.View.Id == "CRMQuotes_DetailView" || nestedFrame.ViewItem.View.Id == "CRMQuotes_DetailView_Submitted_History"))
                        {
                            CRMQuotes crtquotesobj = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                            if (crtquotesobj != null)
                            {
                                quotesinfo.IsTabDiscountChanged = true;
                                foreach (AnalysisPricing objanapricing in View.SelectedObjects)
                                {
                                    ((ListView)View).CollectionSource.Remove(objanapricing);
                                    View.ObjectSpace.Delete(View.ObjectSpace.GetObject(objanapricing));
                                }
                                int sortno = 0;
                                foreach (AnalysisPricing objap in ((ListView)View).CollectionSource.List.Cast<AnalysisPricing>().OrderBy(i => i.Test))
                                {
                                    objap.Sort = Convert.ToUInt16(sortno + 1);
                                    sortno++;
                                }
                                quotesinfo.IsobjChangedpropertyinQuotes = true;
                                decimal finalamt = 0;
                                decimal totalprice = 0;
                                decimal disamt = 0;
                                decimal dispr = 0;
                                bool isTestnull = false;
                                ListPropertyEditor lstitemprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                ListPropertyEditor lstAnalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                if (lstitemprice != null)
                                {
                                    if (lstitemprice.ListView == null)
                                    {
                                        lstitemprice.CreateControl();
                                    }
                                    if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                    {
                                        finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.FinalAmount);
                                        totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.UnitPrice * i.Qty);
                                    }
                                }
                                if (lstAnalysisprice != null)
                                {
                                    if (lstAnalysisprice.ListView == null)
                                    {
                                        lstAnalysisprice.CreateControl();
                                    }
                                    if (lstAnalysisprice.ListView.CollectionSource.GetCount() > 0)
                                    {
                                        finalamt = finalamt + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.FinalAmount);
                                        totalprice = totalprice + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.TotalTierPrice * i.Qty);
                                    }
                                }
                                CRMQuotes crtquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                                if (crtquotes != null)
                                {
                                    if (finalamt != 0 && totalprice != 0)
                                    {
                                        disamt = totalprice - finalamt;
                                        //disamt = finalamt - totalprice;
                                        if (disamt != 0)
                                        {
                                            dispr = ((disamt) / totalprice) * 100;
                                        }
                                        quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                    }
                                    else if (finalamt == 0 && totalprice == 0)
                                    {
                                        crtquotes.DetailedAmount = 0;
                                        crtquotes.TotalAmount = 0;
                                        crtquotes.Discount = 0;
                                        crtquotes.DiscountAmount = 0;
                                        crtquotes.QuotedAmount = 0;
                                        crtquotes.IsGobalDiscount = true;
                                        quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                    }
                                }
                                CRMQuotes objquotes = nestedFrame.ViewItem.View.ObjectSpace.FindObject<CRMQuotes>(CriteriaOperator.Parse("[Oid] = ?", crtquotes.Oid));
                                if (objquotes != null)
                                {
                                    objquotes.DetailedAmount = Math.Round(totalprice, 2);
                                    objquotes.TotalAmount = Math.Round(finalamt, 2);
                                    objquotes.QuotedAmount = Math.Round(finalamt, 2);
                                    objquotes.Discount = Math.Round(dispr, 2);
                                    objquotes.DiscountAmount = Math.Round(disamt, 2);
                                    crtquotes.IsGobalDiscount = true;
                                }
                                nestedFrame.ViewItem.View.Refresh();
                            }
                        }
                    }
                }
                else if (View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView")
                {
                    if (quotesinfo.lsttempItemPricing == null)
                    {
                        quotesinfo.lsttempItemPricing = new List<QuotesItemChargePrice>();
                    }
                    if (quotesinfo.lsttempItemPricing.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        quotesinfo.IsTabDiscountChanged = true;
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (Application.MainWindow.View.Id == "CRMQuotes_DetailView" || Application.MainWindow.View.Id == "CRMQuotes_DetailView_Submitted_History")
                        {
                            CRMQuotes crtquotesobj = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                            quotesinfo.lsttempItemPricing.ToList().ForEach(i => { i.CRMQuotes = null; });
                            foreach (QuotesItemChargePrice obj in quotesinfo.lsttempItemPricing.ToList())
                            {
                                ((ListView)View).CollectionSource.Remove(obj);
                            }
                            ((ListView)View).Refresh();
                            decimal finalamt = 0;
                            decimal totalprice = 0;
                            decimal disamt = 0;
                            decimal dispr = 0;
                            ListPropertyEditor lstitemprice = ((DetailView)Application.MainWindow.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                            ListPropertyEditor lstAnalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                            if (lstitemprice != null)
                            {
                                if (lstitemprice.ListView == null)
                                {
                                    lstitemprice.CreateControl();
                                }
                                if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                {
                                    finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.FinalAmount);
                                    totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.UnitPrice * i.Qty);
                                }
                            }
                            if (lstAnalysisprice != null)
                            {
                                if (lstAnalysisprice.ListView == null)
                                {
                                    lstAnalysisprice.CreateControl();
                                }
                                if (lstAnalysisprice.ListView.CollectionSource.GetCount() > 0)
                                {
                                    finalamt = finalamt + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.FinalAmount);
                                    totalprice = totalprice + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.TotalTierPrice * i.Qty);
                                }
                            }
                            CRMQuotes crtquotes = null;
                            if (Application.MainWindow.View.Id == "CRMQuotes_DetailView" || Application.MainWindow.View.Id == "CRMQuotes_DetailView_Submitted_History")
                            {
                                crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                            }
                            else if (View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_DetailView_Submitted_History")
                            {
                                crtquotes = (CRMQuotes)View.CurrentObject;
                            }
                            else if (quotesinfo.popupcurtquote != null)
                            {
                                crtquotes = quotesinfo.popupcurtquote;
                            }
                            if (crtquotes != null)
                            {
                                if (finalamt != 0 && totalprice != 0)
                                {
                                    disamt = totalprice - finalamt;
                                    if (disamt != 0)
                                    {
                                        dispr = ((disamt) / totalprice) * 100;
                                    }
                                    quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                }
                                else if (finalamt == 0 && totalprice == 0)
                                {
                                    crtquotes.DetailedAmount = 0;
                                    crtquotes.TotalAmount = 0;
                                    crtquotes.Discount = 0;
                                    crtquotes.DiscountAmount = 0;
                                    crtquotes.QuotedAmount = 0;
                                    crtquotes.IsGobalDiscount = true;
                                }
                            }
                            CRMQuotes objquotes = Application.MainWindow.View.ObjectSpace.FindObject<CRMQuotes>(CriteriaOperator.Parse("[Oid] = ?", crtquotes.Oid));
                            if (objquotes != null)
                            {
                                objquotes.DetailedAmount = Math.Round(totalprice, 2);
                                objquotes.TotalAmount = Math.Round(finalamt, 2);
                                objquotes.QuotedAmount = Math.Round(finalamt, 2);
                                objquotes.Discount = Math.Round(dispr, 2);
                                objquotes.DiscountAmount = Math.Round(disamt, 2);
                                crtquotes.IsGobalDiscount = true;
                            }
                            Application.MainWindow.View.Refresh();
                        }
                        else if (nestedFrame != null && nestedFrame.ViewItem.View != null && (nestedFrame.ViewItem.View.Id == "CRMQuotes_DetailView" || nestedFrame.ViewItem.View.Id == "CRMQuotes_DetailView"))
                        {
                            CRMQuotes crtquotesobj = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                            quotesinfo.lsttempItemPricing.ToList().ForEach(i => { i.CRMQuotes = null; });
                            foreach (QuotesItemChargePrice obj in quotesinfo.lsttempItemPricing.ToList())
                            {
                                ((ListView)View).CollectionSource.Remove(obj);
                            }
                            ((ListView)View).Refresh();
                            decimal finalamt = 0;
                            decimal totalprice = 0;
                            decimal disamt = 0;
                            decimal dispr = 0;
                            ListPropertyEditor lstitemprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                            ListPropertyEditor lstAnalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                            if (lstitemprice != null)
                            {
                                if (lstitemprice.ListView == null)
                                {
                                    lstitemprice.CreateControl();
                                }
                                if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                {
                                    finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.FinalAmount);
                                    totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.UnitPrice * i.Qty);
                                }
                            }
                            if (lstAnalysisprice != null)
                            {
                                if (lstAnalysisprice.ListView == null)
                                {
                                    lstAnalysisprice.CreateControl();
                                }
                                if (lstAnalysisprice.ListView.CollectionSource.GetCount() > 0)
                                {
                                    finalamt = finalamt + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.FinalAmount);
                                    totalprice = totalprice + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.TotalTierPrice * i.Qty);
                                }
                            }
                            CRMQuotes crtquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                            if (crtquotes != null)
                            {
                                if (finalamt != 0 && totalprice != 0)
                                {
                                    disamt = totalprice - finalamt;
                                    //disamt = finalamt - totalprice;
                                    if (disamt != 0)
                                    {
                                        dispr = ((disamt) / totalprice) * 100;
                                    }
                                    quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                }
                                else if (finalamt == 0 && totalprice == 0)
                                {
                                    crtquotes.DetailedAmount = 0;
                                    crtquotes.TotalAmount = 0;
                                    crtquotes.Discount = 0;
                                    crtquotes.DiscountAmount = 0;
                                    crtquotes.QuotedAmount = 0;
                                }
                            }
                            CRMQuotes objquotes = nestedFrame.ViewItem.View.ObjectSpace.FindObject<CRMQuotes>(CriteriaOperator.Parse("[Oid] = ?", crtquotes.Oid));
                            if (objquotes != null)
                            {
                                objquotes.IsGobalDiscount = true;
                                objquotes.DetailedAmount = Math.Round(totalprice, 2);
                                objquotes.TotalAmount = Math.Round(finalamt, 2);
                                objquotes.QuotedAmount = Math.Round(finalamt, 2);
                                objquotes.Discount = Math.Round(dispr, 2);
                                objquotes.DiscountAmount = Math.Round(disamt, 2);
                            }
                            nestedFrame.ViewItem.View.Refresh();
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

        private void btnAddpricecode_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(AnalysisPricing));
                    AnalysisPricing objcrtdummy = os.CreateObject<AnalysisPricing>();
                    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(AnalysisPricing));
                    //ListView lvparameter = Application.CreateListView(/*"AnalysisPricing_ListView_QuotesPopup"*/"AnalysisPricing_ListView_QuotePopupNew", cs, false);
                    DetailView lvparameter = Application.CreateDetailView(os, "AnalysisPricing_DetailView_QuotePopup", false, objcrtdummy);
                    //ListView lvparameter = Application.CreateListView("TestPriceSurcharge_ListView_Quotes_Popup", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                    showViewParameters.CreatedView = lvparameter;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += Dcaddparameter_Accepting;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                    //List<string> lststrtestdts = new List<string>();
                    //List<Guid> lsttestparamsoid = new List<Guid>();
                    //List<Testparameter> lsttestparams = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[GCRecord] Is Null")).ToList();
                    //foreach (Testparameter objtstparams in lsttestparams.ToList())
                    //{
                    //    if (objtstparams.TestMethod != null && objtstparams.TestMethod.MethodName != null && objtstparams.Component != null)
                    //    {
                    //        string strtst = objtstparams.TestMethod.TestName + "|" + objtstparams.TestMethod.MethodName.MethodNumber + "|" + objtstparams.Component.Components;
                    //        if (!lststrtestdts.Contains(strtst))
                    //        {
                    //            lststrtestdts.Add(strtst);
                    //            lsttestparamsoid.Add(objtstparams.Oid);
                    //        }
                    //    }
                    //    else
                    //    if (objtstparams.TestMethod != null && objtstparams.TestMethod.IsGroup == true)
                    //    {
                    //        string strtst = objtstparams.TestMethod.Oid + "|" + objtstparams.IsGroup;
                    //        if (!lststrtestdts.Contains(strtst))
                    //        {
                    //            lststrtestdts.Add(strtst);
                    //            lsttestparamsoid.Add(objtstparams.Oid);
                    //        }
                    //    }
                    //}
                    //IObjectSpace os = Application.CreateObjectSpace(typeof(Testparameter));
                    //Testparameter objcrtdummy = os.CreateObject<Testparameter>();
                    //CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Testparameter));
                    //cs.Criteria["filter"] = new InOperator("Oid", lsttestparamsoid);
                    ////cs.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objcrtanaprice.Matrix.MatrixName, objcrtanaprice.Test.TestName, objcrtanaprice.Method.MethodNumber, objcrtanaprice.Component.Components);
                    //ListView lvparameter = Application.CreateListView("Testparameter_LookupListView_Quotes_PriceCode", cs, false);
                    //ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                    //showViewParameters.CreatedView = lvparameter;
                    //showViewParameters.Context = TemplateContext.PopupWindow;
                    //showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //DialogController dc = Application.CreateController<DialogController>();
                    //dc.SaveOnAccept = false;
                    //dc.CloseOnCurrentObjectProcessing = false;
                    //dc.Accepting += Dcaddparameter_Accepting;
                    //showViewParameters.Controllers.Add(dc);
                    //Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                else if (View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView")
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(ItemChargePricing));
                    ItemChargePricing objcrtdummy = os.CreateObject<ItemChargePricing>();
                    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(ItemChargePricing));
                    //List<Guid> lstitemcode = new List<Guid>();
                    //foreach (QuotesItemChargePrice objitemprice in ((ListView)View).CollectionSource.List)
                    //{
                    //    if(!lstitemcode.Contains(objitemprice.ItemPrice.Oid))
                    //    {
                    //        lstitemcode.Add(objitemprice.ItemPrice.Oid);
                    //    }
                    //}
                    //if(lstitemcode.Count > 0)
                    //{
                    //    cs.Criteria["filter"] = new NotOperator(new InOperator("Oid", lstitemcode));
                    //}                    
                    //cs.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [Component.Components] = ? And [QCType.QCTypeName] = 'Sample'", objcrtanaprice.Matrix.MatrixName, objcrtanaprice.Test.TestName, objcrtanaprice.Method.MethodNumber, objcrtanaprice.Component.Components);
                    ListView lvparameter = Application.CreateListView("ItemChargePricing_ListView_Quotes_Popup", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                    showViewParameters.CreatedView = lvparameter;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += Dcadditemcharge_Accepting;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dcadditemcharge_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                //if (sender != null)
                {
                    decimal finalamt = 0;
                    decimal totalprice = 0;
                    decimal disamt = 0;
                    decimal dispr = 0;
                    if (Application.MainWindow.View.Id == "CRMQuotes_DetailView" || Application.MainWindow.View.Id == "CRMQuotes_DetailView_Submitted_History")
                    {
                        CRMQuotes crtquotesobj = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                        if (quotesinfo.lsttempItemPricingpopup != null && quotesinfo.lsttempItemPricingpopup.Count > 0)
                        {
                            QuotesItemChargePrice objitempriceNew = null;
                            foreach (ItemChargePricing objitemprice in quotesinfo.lsttempItemPricingpopup.ToList())
                            {
                                QuotesItemChargePrice objquoteitemcharge = null;
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    objquoteitemcharge = nestedFrame.View.ObjectSpace.FindObject<QuotesItemChargePrice>(CriteriaOperator.Parse("[ItemPrice.Oid] = ? And [CRMQuotes.Oid] = ?", objitemprice.Oid, crtquotesobj.Oid));
                                    if (objquoteitemcharge == null)
                                    {
                                        objitempriceNew = nestedFrame.View.ObjectSpace.CreateObject<QuotesItemChargePrice>();
                                        objitempriceNew.UnitPrice = objitemprice.UnitPrice;
                                        objitempriceNew.NpUnitPrice = objitemprice.UnitPrice;
                                        objitempriceNew.Qty = 1;
                                        objitempriceNew.Amount = objitemprice.UnitPrice;
                                        objitempriceNew.FinalAmount = objitemprice.UnitPrice;
                                        objitempriceNew.Discount = 0;
                                        objitempriceNew.ItemPrice = nestedFrame.View.ObjectSpace.GetObjectByKey<ItemChargePricing>(objitemprice.Oid);
                                        objitempriceNew.CRMQuotes = View.ObjectSpace.GetObject(crtquotesobj);
                                        objitempriceNew.Description = objitemprice.Description;
                                        //uow.CommitChanges();
                                        ((ListView)View).CollectionSource.Add(objitempriceNew);
                                        crtquotesobj.QuotesItemChargePrice.Add(objitempriceNew);
                                    }
                                }
                                else
                                {
                                    objquoteitemcharge = Application.MainWindow.View.ObjectSpace.FindObject<QuotesItemChargePrice>(CriteriaOperator.Parse("[ItemPrice.Oid] = ? And [CRMQuotes.Oid] = ?", objitemprice.Oid, crtquotesobj.Oid));
                                    if (objquoteitemcharge == null)
                                    {
                                        objitempriceNew = Application.MainWindow.View.ObjectSpace.CreateObject<QuotesItemChargePrice>();
                                        objitempriceNew.UnitPrice = objitemprice.UnitPrice;
                                        objitempriceNew.NpUnitPrice = objitemprice.UnitPrice;
                                        objitempriceNew.Qty = 1;
                                        objitempriceNew.Amount = objitemprice.UnitPrice;
                                        objitempriceNew.FinalAmount = objitemprice.UnitPrice;
                                        objitempriceNew.Discount = 0;
                                        objitempriceNew.ItemPrice = Application.MainWindow.View.ObjectSpace.GetObjectByKey<ItemChargePricing>(objitemprice.Oid);
                                        objitempriceNew.CRMQuotes = View.ObjectSpace.GetObject(crtquotesobj);
                                        objitempriceNew.Description = objitemprice.Description;
                                        //uow.CommitChanges();
                                        ((ListView)View).CollectionSource.Add(objitempriceNew);
                                        crtquotesobj.QuotesItemChargePrice.Add(objitempriceNew);
                                    }
                                }
                            }
                            ((ListView)View).Refresh();
                        }
                        ListPropertyEditor lstAnalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                        ListPropertyEditor lstitemprice = ((DetailView)Application.MainWindow.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                        if (lstitemprice != null)
                        {
                            if (lstitemprice.ListView == null)
                            {
                                lstitemprice.CreateControl();
                            }
                            if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                            {
                                finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.FinalAmount);
                                totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.UnitPrice * i.Qty);
                            }
                        }
                        if (lstAnalysisprice != null)
                        {
                            if (lstAnalysisprice.ListView == null)
                            {
                                lstAnalysisprice.CreateControl();
                            }
                            if (lstAnalysisprice.ListView.CollectionSource.GetCount() > 0)
                            {
                                finalamt = finalamt + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.FinalAmount);
                                totalprice = totalprice + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.TotalTierPrice * i.Qty);
                            }
                        }
                        CRMQuotes crtquotes = null;
                        if (Application.MainWindow.View.Id == "CRMQuotes_DetailView" || Application.MainWindow.View.Id == "CRMQuotes_DetailView_Submitted_History")
                        {
                            crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                        }
                        else if (View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_DetailView_Submitted_History")
                        {
                            crtquotes = (CRMQuotes)View.CurrentObject;
                        }
                        else if (quotesinfo.popupcurtquote != null)
                        {
                            crtquotes = quotesinfo.popupcurtquote;
                        }
                        //CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                        if (crtquotes != null)
                        {
                            quotesinfo.IsTabDiscountChanged = true;
                            if (finalamt != 0 && totalprice != 0)
                            {
                                disamt = totalprice - finalamt;
                                if (disamt != 0)
                                {
                                    dispr = ((disamt) / totalprice) * 100;
                                }
                                //disamt = finalamt - totalprice;
                                quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                crtquotes.Discount = Math.Round(dispr, 2);
                                crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                crtquotes.IsGobalDiscount = true;
                            }
                            else if (finalamt == 0 && totalprice == 0)
                            {
                                crtquotes.DetailedAmount = 0;
                                crtquotes.TotalAmount = 0;
                                crtquotes.Discount = 0;
                                crtquotes.DiscountAmount = 0;
                                crtquotes.QuotedAmount = 0;
                                crtquotes.IsGobalDiscount = true;
                            }
                        }
                        Application.MainWindow.View.Refresh();
                    }
                    else
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (nestedFrame != null && nestedFrame.ViewItem.View != null && (nestedFrame.ViewItem.View.Id == "CRMQuotes_DetailView" || nestedFrame.ViewItem.View.Id == "CRMQuotes_DetailView_Submitted_History"))
                        {
                            CRMQuotes crtquotesobj = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                            if (crtquotesobj != null)
                            {
                                if (quotesinfo.lsttempItemPricingpopup != null && quotesinfo.lsttempItemPricingpopup.Count > 0)
                                {
                                    QuotesItemChargePrice objitempriceNew = null;
                                    foreach (ItemChargePricing objitemprice in quotesinfo.lsttempItemPricingpopup.ToList())
                                    {
                                        QuotesItemChargePrice objquoteitemcharge = nestedFrame.ViewItem.View.ObjectSpace.FindObject<QuotesItemChargePrice>(CriteriaOperator.Parse("[ItemPrice.Oid] = ? And [CRMQuotes.Oid] = ?", objitemprice.Oid, crtquotesobj.Oid));
                                        if (objquoteitemcharge == null)
                                        {
                                            objitempriceNew = nestedFrame.ViewItem.View.ObjectSpace.CreateObject<QuotesItemChargePrice>();
                                            objitempriceNew.UnitPrice = objitemprice.UnitPrice;
                                            objitempriceNew.NpUnitPrice = objitemprice.UnitPrice;
                                            objitempriceNew.Qty = 1;
                                            objitempriceNew.Amount = objitemprice.UnitPrice;
                                            objitempriceNew.FinalAmount = objitemprice.UnitPrice;
                                            objitempriceNew.Discount = 0;
                                            objitempriceNew.ItemPrice = nestedFrame.ViewItem.View.ObjectSpace.GetObjectByKey<ItemChargePricing>(objitemprice.Oid);
                                            objitempriceNew.CRMQuotes = View.ObjectSpace.GetObject(crtquotesobj);
                                            ((ListView)View).CollectionSource.Add(objitempriceNew);
                                            crtquotesobj.QuotesItemChargePrice.Add(objitempriceNew);
                                        }
                                    }
                                    ((ListView)View).Refresh();
                                }
                                ListPropertyEditor lstAnalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                ListPropertyEditor lstitemprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                if (lstitemprice != null)
                                {
                                    if (lstitemprice.ListView == null)
                                    {
                                        lstitemprice.CreateControl();
                                    }
                                    if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                    {
                                        finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.FinalAmount);
                                        totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.UnitPrice * i.Qty);
                                    }
                                }
                                if (lstAnalysisprice != null)
                                {
                                    if (lstAnalysisprice.ListView == null)
                                    {
                                        lstAnalysisprice.CreateControl();
                                    }
                                    if (lstAnalysisprice.ListView.CollectionSource.GetCount() > 0)
                                    {
                                        finalamt = finalamt + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.FinalAmount);
                                        totalprice = totalprice + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.TotalTierPrice * i.Qty);
                                    }
                                }
                                CRMQuotes crtquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                                if (crtquotes != null)
                                {
                                    quotesinfo.IsTabDiscountChanged = true;
                                    if (finalamt != 0 && totalprice != 0)
                                    {
                                        disamt = totalprice - finalamt;
                                        if (disamt != 0)
                                        {
                                            dispr = ((disamt) / totalprice) * 100;
                                        }
                                        quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                        crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                        crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                        crtquotes.Discount = Math.Round(dispr, 2);
                                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                        crtquotes.IsGobalDiscount = true;
                                    }
                                    else if (finalamt == 0 && totalprice == 0)
                                    {
                                        crtquotes.DetailedAmount = 0;
                                        crtquotes.TotalAmount = 0;
                                        crtquotes.Discount = 0;
                                        crtquotes.DiscountAmount = 0;
                                        crtquotes.QuotedAmount = 0;
                                        crtquotes.IsGobalDiscount = true;
                                    }
                                }
                                nestedFrame.ViewItem.View.Refresh();
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

        private void Dcaddparameter_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DialogController dc = sender as DialogController;
                    if (quotesinfo.lsttempAnalysisPricingpopup != null && quotesinfo.lsttempAnalysisPricingpopup.Count > 0)
                    {
                        CRMQuotes objcq = null;
                        if (Application.MainWindow.View.Id == "CRMQuotes_DetailView" || Application.MainWindow.View.Id == "CRMQuotes_DetailView_Submitted_History")
                        {
                            objcq = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                        }
                        else if (View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_DetailView_Submitted_History")
                        {
                            objcq = (CRMQuotes)View.CurrentObject;
                        }
                        else
                        {
                            quotesinfo.IsTabDiscountChanged = true;
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            if (nestedFrame != null && nestedFrame.ViewItem.View != null && (nestedFrame.ViewItem.View.Id == "CRMQuotes_DetailView" || nestedFrame.ViewItem.View.Id == "CRMQuotes_DetailView_Submitted_History"))
                            {
                                objcq = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                                if (objcq != null)
                                {
                                    AnalysisPricing newanalysisprice = null;
                                    foreach (AnalysisPricing objtestparams in quotesinfo.lsttempAnalysisPricingpopup.ToList())
                                    {
                                        if (objtestparams != null && objtestparams.Matrix != null && objtestparams.Test != null && objtestparams.Method != null && objtestparams.Component != null)
                                        {
                                            AnalysisPricing objanalysisprice = nestedFrame.ViewItem.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ? And [Matrix.MatrixName] =? And [Test.TestName] =? And [Method.MethodNumber]=? And [Component.Components]=?", objcq.Oid, objtestparams.Matrix.MatrixName, objtestparams.Test.TestName, objtestparams.Method.MethodNumber, objtestparams.Component.Components)); //And [Priority.Prioritys] = 'Regular'
                                            if (objanalysisprice == null)
                                            {
                                                if (objtestparams.Test != null && objtestparams.Component != null)
                                                {
                                                    TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And[Component.Components] = ? And [Priority.IsRegular] = 'True'", objtestparams.Matrix.MatrixName, objtestparams.Test.TestName, objtestparams.Method.MethodNumber, objtestparams.Component.Components));
                                                    if (objtstpricesur != null)
                                                    {
                                                        newanalysisprice = nestedFrame.ViewItem.View.ObjectSpace.CreateObject<AnalysisPricing>();
                                                        newanalysisprice.PriceCode = objtestparams.PriceCode;
                                                        newanalysisprice.Qty = 1;
                                                        newanalysisprice.Matrix = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objtstpricesur.Matrix);
                                                        newanalysisprice.Test = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objtstpricesur.Test);
                                                        newanalysisprice.TestDescription = objtstpricesur.Test.Comment;
                                                        newanalysisprice.IsGroup = objtstpricesur.IsGroup;
                                                        newanalysisprice.Method = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objtstpricesur.Method.MethodName);
                                                        newanalysisprice.Component = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objtstpricesur.Component);
                                                        newanalysisprice.Parameter = "AllParams";
                                                        newanalysisprice.ChargeType = objtstpricesur.ChargeType;
                                                        if (objtstpricesur.SurchargePrice != null)
                                                        {
                                                            newanalysisprice.UnitPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            newanalysisprice.NPUnitPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            newanalysisprice.NPTotalPrice = (decimal)objtstpricesur.SurchargePrice;
                                                        }
                                                        newanalysisprice.Discount = 0;
                                                        if (objtstpricesur.SurchargePrice != null)
                                                        {
                                                            newanalysisprice.TotalTierPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            newanalysisprice.FinalAmount = (decimal)objtstpricesur.SurchargePrice;
                                                        }
                                                        List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                        List<string> lsttatoid = new List<string>();
                                                        List<Priority> lstpriority = new List<Priority>();
                                                        TurnAroundTime objtpstat = null;
                                                        {
                                                            string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                            if (strTAToidarr.Length > 1)
                                                            {
                                                                foreach (string objoid in strTAToidarr.ToList())
                                                                {
                                                                    lsttatoid.Add((objoid));
                                                                }
                                                            }
                                                            else
                                                            {
                                                                lsttatoid.Add((objtstpricesur.TAT));
                                                            }
                                                        }
                                                        if (lsttatoid.Count > 0)
                                                        {
                                                            IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatoid));
                                                            foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                            {
                                                                newanalysisprice.TAT = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objturntime);
                                                                break;
                                                            }
                                                        }
                                                        Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtestparams.Priority.Oid));
                                                        if (objpriority != null)
                                                        {
                                                            newanalysisprice.Priority = nestedFrame.ViewItem.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                        }
                                                        newanalysisprice.CRMQuotes = nestedFrame.ViewItem.View.ObjectSpace.GetObject<CRMQuotes>(objcq);
                                                        newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                        objcq.AnalysisPricing.Add(nestedFrame.ViewItem.View.ObjectSpace.GetObject(newanalysisprice));
                                                        ((ListView)View).CollectionSource.Add(newanalysisprice);
                                                    }
                                                }
                                                else if (objtestparams.Test != null && objtestparams.Component == null && objtestparams.IsGroup == true)
                                                {
                                                    TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'True' And [Priority.IsRegular] = 'True'", objtestparams.Matrix.MatrixName, objtestparams.Test.TestName)); //IsRegular
                                                    if (objtstpricesur != null)
                                                    {
                                                        newanalysisprice = nestedFrame.ViewItem.View.ObjectSpace.CreateObject<AnalysisPricing>();
                                                        newanalysisprice.PriceCode = objtestparams.PriceCode;
                                                        newanalysisprice.Qty = 1;
                                                        newanalysisprice.Matrix = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objtstpricesur.Matrix);
                                                        newanalysisprice.Test = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objtstpricesur.Test);
                                                        newanalysisprice.TestDescription = objtstpricesur.Test.Comment;
                                                        newanalysisprice.IsGroup = objtstpricesur.IsGroup;
                                                        newanalysisprice.Parameter = "AllParams";
                                                        newanalysisprice.ChargeType = objtstpricesur.ChargeType;
                                                        if (objtstpricesur.SurchargePrice != null)
                                                        {
                                                            newanalysisprice.UnitPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            newanalysisprice.NPUnitPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            newanalysisprice.NPTotalPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            newanalysisprice.TotalTierPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            newanalysisprice.TotalTierPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            newanalysisprice.FinalAmount = (decimal)objtstpricesur.SurchargePrice;
                                                        }
                                                        newanalysisprice.Discount = 0;

                                                        List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                        List<string> lsttatoid = new List<string>();
                                                        List<Priority> lstpriority = new List<Priority>();
                                                        TurnAroundTime objtpstat = null;
                                                        {
                                                            string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                            if (strTAToidarr.Length > 1)
                                                            {
                                                                foreach (string objoid in strTAToidarr.ToList())
                                                                {
                                                                    lsttatoid.Add((objoid));
                                                                }
                                                            }
                                                            else
                                                            {
                                                                lsttatoid.Add((objtstpricesur.TAT));
                                                            }
                                                        }
                                                        if (lsttatoid.Count > 0)
                                                        {
                                                            IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatoid));
                                                            foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                            {
                                                                newanalysisprice.TAT = ObjectSpace.GetObject(objturntime);
                                                                break;
                                                            }
                                                        }
                                                        Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtestparams.Priority.Oid));
                                                        if (objpriority != null)
                                                        {
                                                            newanalysisprice.Priority = nestedFrame.ViewItem.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                        }
                                                        newanalysisprice.CRMQuotes = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objcq);
                                                        newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                        objcq.AnalysisPricing.Add(newanalysisprice);

                                                    }
                                                    ((ListView)View).CollectionSource.Add(newanalysisprice);
                                                }
                                            }
                                        }
                                        else if (objtestparams != null && objtestparams.Matrix != null && objtestparams.Test != null && objtestparams.IsGroup == true)
                                        {
                                            AnalysisPricing objanalysisprice = nestedFrame.ViewItem.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ? And [Matrix.MatrixName] =? And [Test.TestName] =? And [IsGroup] = 'True'", objcq.Oid, objtestparams.Matrix.MatrixName, objtestparams.Test.TestName)); //And [Priority.Prioritys] = 'Regular'
                                            if (objanalysisprice == null)
                                            {
                                                if (objtestparams.Test != null && objtestparams.Component != null)
                                                {
                                                    TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'True' And [Priority.IsRegular] = 'True'", objtestparams.Matrix.MatrixName, objtestparams.Test.TestName)); //IsRegular
                                                    if (objtstpricesur != null)
                                                    {
                                                        newanalysisprice = nestedFrame.ViewItem.View.ObjectSpace.CreateObject<AnalysisPricing>();
                                                        newanalysisprice.PriceCode = objtestparams.PriceCode;
                                                        newanalysisprice.Qty = 1;
                                                        newanalysisprice.Matrix = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objtstpricesur.Matrix);
                                                        newanalysisprice.Test = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objtstpricesur.Test);
                                                        newanalysisprice.TestDescription = objtstpricesur.Test.Comment;
                                                        newanalysisprice.IsGroup = objtstpricesur.IsGroup;
                                                        newanalysisprice.Method = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objtstpricesur.Method.MethodName);
                                                        newanalysisprice.Component = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objtstpricesur.Component);
                                                        newanalysisprice.Parameter = "AllParams";
                                                        DefaultPricing objdefpricing = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objtstpricesur.Test.Oid));
                                                        ConstituentPricing objconstipricing = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And[Test.TestName] = ? And[Method.MethodNumber] = ? And [Component.Components] = ?", objtstpricesur.Matrix.MatrixName, objtstpricesur.Test.TestName, objtstpricesur.Method.MethodName.MethodNumber, objtstpricesur.Component.Components));
                                                        if (objconstipricing != null)
                                                        {
                                                            newanalysisprice.ChargeType = objconstipricing.ChargeType;
                                                            decimal tierprc = 0;
                                                            decimal unitpriceamt = 0;
                                                            int paracnt = 0;
                                                            if (objconstipricing.Test != null && objconstipricing.Component != null)
                                                            {
                                                                List<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Component.Oid] = ? And [QCType.QCTypeName] = 'Sample'", objtestparams.Test.Oid, objtestparams.Component.Oid)).ToList();
                                                                if (lsttestpara.Count > 0)
                                                                {
                                                                    paracnt = lsttestpara.Count;
                                                                }
                                                            }
                                                            if (objconstipricing.ChargeType == ChargeType.Parameter)
                                                            {
                                                                ConstituentPricingTier lstconstituentpricrtier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And[To] >= ?", objconstipricing.Oid, paracnt, paracnt));
                                                                if (lstconstituentpricrtier != null)
                                                                {
                                                                    unitpriceamt = lstconstituentpricrtier.TierPrice * paracnt;
                                                                    newanalysisprice.TierNo = lstconstituentpricrtier.TierNo;
                                                                    newanalysisprice.From = lstconstituentpricrtier.From;
                                                                    newanalysisprice.To = lstconstituentpricrtier.To;
                                                                    newanalysisprice.TierPrice = lstconstituentpricrtier.TierPrice;
                                                                }
                                                            }
                                                            else if (objconstipricing.ChargeType == ChargeType.Test)
                                                            {
                                                                ConstituentPricingTier objtierprice = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And[To] >= ?", objconstipricing.Oid, newanalysisprice.Qty, newanalysisprice.Qty));
                                                                if (objtierprice != null)
                                                                {
                                                                    newanalysisprice.TierNo = objtierprice.TierNo;
                                                                    newanalysisprice.From = objtierprice.From;
                                                                    newanalysisprice.To = objtierprice.To;
                                                                    newanalysisprice.TierPrice = objtierprice.TierPrice;
                                                                    unitpriceamt = objtierprice.TierPrice;
                                                                }
                                                            }
                                                            newanalysisprice.UnitPrice = unitpriceamt;
                                                            newanalysisprice.NPUnitPrice = unitpriceamt;
                                                            newanalysisprice.NPTotalPrice = unitpriceamt /** paracnt*/;
                                                            newanalysisprice.Discount = 0;
                                                            List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                            List<string> lsttatoid = new List<string>();
                                                            List<Guid> lsttatinfo = new List<Guid>();
                                                            List<Priority> lstpriority = new List<Priority>();
                                                            TurnAroundTime objtpstat = null;
                                                            if (objtstpricesur != null && !string.IsNullOrEmpty(objtstpricesur.TAT))
                                                            {
                                                                string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                                if (strTAToidarr.Length > 1)
                                                                {
                                                                    foreach (string objoid in strTAToidarr.ToList())
                                                                    {
                                                                        lsttatoid.Add((objoid));
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    lsttatoid.Add((objtstpricesur.TAT));
                                                                }
                                                            }
                                                            if (lsttatoid.Count > 0)
                                                            {
                                                                IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatoid));
                                                                foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                                {
                                                                    newanalysisprice.TAT = ObjectSpace.GetObject(objturntime);
                                                                    break;
                                                                }
                                                            }
                                                            newanalysisprice.TotalTierPrice = unitpriceamt /** paracnt*/;
                                                            newanalysisprice.FinalAmount = unitpriceamt/* * paracnt*/;
                                                            Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtstpricesur.Priority.Oid));
                                                            if (objpriority != null)
                                                            {
                                                                newanalysisprice.Priority = nestedFrame.ViewItem.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                            }
                                                            newanalysisprice.CRMQuotes = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objcq);
                                                            newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                        }
                                                        else
                                                        if (objdefpricing != null)
                                                        {
                                                            newanalysisprice.ChargeType = objdefpricing.ChargeType;
                                                            newanalysisprice.UnitPrice = objdefpricing.UnitPrice;
                                                            newanalysisprice.NPUnitPrice = objdefpricing.UnitPrice;
                                                            newanalysisprice.NPTotalPrice = objdefpricing.UnitPrice;
                                                            newanalysisprice.Discount = 0;
                                                            if (objtstpricesur.SurchargePrice != null)
                                                            {
                                                                newanalysisprice.TotalTierPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            }
                                                            newanalysisprice.FinalAmount = objdefpricing.UnitPrice;
                                                            List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                            List<string> lsttatoid = new List<string>();
                                                            List<Priority> lstpriority = new List<Priority>();
                                                            TurnAroundTime objtpstat = null;
                                                            {
                                                                string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                                if (strTAToidarr.Length > 1)
                                                                {
                                                                    foreach (string objoid in strTAToidarr.ToList())
                                                                    {
                                                                        lsttatoid.Add((objoid));
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    lsttatoid.Add((objtstpricesur.TAT));
                                                                }
                                                            }
                                                            if (lsttatoid.Count > 0)
                                                            {
                                                                IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatoid));
                                                                foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                                {
                                                                    newanalysisprice.TAT = ObjectSpace.GetObject(objturntime);
                                                                    break;
                                                                }
                                                            }
                                                            Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtstpricesur.Priority.Oid));
                                                            if (objpriority != null)
                                                            {
                                                                newanalysisprice.Priority = nestedFrame.ViewItem.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                            }
                                                            newanalysisprice.CRMQuotes = nestedFrame.ViewItem.View.ObjectSpace.GetObject<CRMQuotes>(objcq);
                                                            newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                        }
                                                        objcq.AnalysisPricing.Add(newanalysisprice);
                                                        ((ListView)View).CollectionSource.Add(newanalysisprice);
                                                    }
                                                }
                                                else if (objtestparams.Test != null && objtestparams.Component == null && objtestparams.IsGroup == true)
                                                {
                                                    TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'True' And [Priority.IsRegular] = 'True'", objtestparams.Matrix.MatrixName, objtestparams.Test.TestName)); //IsRegular
                                                    if (objtstpricesur != null)
                                                    {
                                                        newanalysisprice = nestedFrame.ViewItem.View.ObjectSpace.CreateObject<AnalysisPricing>();
                                                        newanalysisprice.PriceCode = objtestparams.PriceCode;
                                                        newanalysisprice.Qty = 1;
                                                        newanalysisprice.Matrix = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objtstpricesur.Matrix);
                                                        newanalysisprice.Test = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objtstpricesur.Test);
                                                        newanalysisprice.TestDescription = objtstpricesur.Test.Comment;
                                                        newanalysisprice.IsGroup = objtstpricesur.IsGroup;
                                                        newanalysisprice.Parameter = "AllParams";
                                                        newanalysisprice.ChargeType = objtstpricesur.ChargeType;
                                                        if (objtstpricesur.SurchargePrice != null)
                                                        {
                                                            newanalysisprice.UnitPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            newanalysisprice.NPUnitPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            newanalysisprice.NPTotalPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            newanalysisprice.TotalTierPrice = (decimal)objtstpricesur.SurchargePrice;
                                                            newanalysisprice.FinalAmount = (decimal)objtstpricesur.SurchargePrice;
                                                        }
                                                        newanalysisprice.Discount = 0;
                                                        List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                        List<string> lsttatoid = new List<string>();
                                                        List<Priority> lstpriority = new List<Priority>();
                                                        TurnAroundTime objtpstat = null;
                                                        {
                                                            string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                            if (strTAToidarr.Length > 1)
                                                            {
                                                                foreach (string objoid in strTAToidarr.ToList())
                                                                {
                                                                    lsttatoid.Add((objoid));
                                                                }
                                                            }
                                                            else
                                                            {
                                                                lsttatoid.Add((objtstpricesur.TAT));
                                                            }
                                                        }
                                                        if (lsttatoid.Count > 0)
                                                        {
                                                            IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatoid));
                                                            foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                            {
                                                                newanalysisprice.TAT = ObjectSpace.GetObject(objturntime);
                                                                break;
                                                            }
                                                        }
                                                        Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtstpricesur.Priority.Oid));
                                                        if (objpriority != null)
                                                        {
                                                            newanalysisprice.Priority = nestedFrame.ViewItem.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                        }
                                                        newanalysisprice.CRMQuotes = nestedFrame.ViewItem.View.ObjectSpace.GetObject(objcq);
                                                        newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                        objcq.AnalysisPricing.Add(newanalysisprice);
                                                    }
                                                    ((ListView)View).CollectionSource.Add(newanalysisprice);
                                                }
                                            }
                                        }
                                    }
                                ((ListView)View).Refresh();
                                    quotesinfo.IsobjChangedpropertyinQuotes = true;
                                    decimal finalamt = 0;
                                    decimal totalprice = 0;
                                    decimal disamt = 0;
                                    decimal dispr = 0;
                                    ListPropertyEditor lstAnalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                    ListPropertyEditor lstitemprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                    if (lstitemprice != null)
                                    {
                                        if (lstitemprice.ListView == null)
                                        {
                                            lstitemprice.CreateControl();
                                        }
                                        if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                        {
                                            finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.FinalAmount);
                                            totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.UnitPrice * i.Qty);
                                        }
                                    }
                                    if (lstAnalysisprice != null)
                                    {
                                        if (lstAnalysisprice.ListView == null)
                                        {
                                            lstAnalysisprice.CreateControl();
                                        }
                                        if (lstAnalysisprice.ListView.CollectionSource.GetCount() > 0)
                                        {
                                            finalamt = finalamt + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.FinalAmount);
                                            totalprice = totalprice + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.TotalTierPrice * i.Qty);
                                        }
                                    }
                                    CRMQuotes crtquotes = quotesinfo.popupcurtquote;
                                    if (crtquotes != null)
                                    {
                                        if (finalamt != 0 && totalprice != 0)
                                        {
                                            disamt = totalprice - finalamt;
                                            if (disamt != 0)
                                            {
                                                dispr = ((disamt) / totalprice) * 100;
                                            }
                                            quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                            crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Math.Round(dispr, 2);
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                            crtquotes.IsGobalDiscount = true;
                                        }
                                        else if (finalamt == 0 && totalprice == 0)
                                        {
                                            crtquotes.DetailedAmount = 0;
                                            crtquotes.TotalAmount = 0;
                                            crtquotes.Discount = 0;
                                            crtquotes.DiscountAmount = 0;
                                            crtquotes.QuotedAmount = 0;
                                            crtquotes.IsGobalDiscount = true;
                                        }
                                    }
                                    //Application.MainWindow.View.Refresh();
                                    nestedFrame.ViewItem.View.Refresh();
                                }
                            }
                        }
                        if (objcq != null && (Application.MainWindow.View.Id == "CRMQuotes_DetailView" || Application.MainWindow.View.Id == "CRMQuotes_DetailView_Submitted_History"))
                        {
                            AnalysisPricing newanalysisprice = null;
                            foreach (AnalysisPricing objtestparams in quotesinfo.lsttempAnalysisPricingpopup.ToList())
                            {
                                if (objtestparams != null && objtestparams.Matrix != null && objtestparams.Test != null && objtestparams.Method != null && objtestparams.Component != null)
                                {
                                    AnalysisPricing objanalysisprice = null;
                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ? And [Matrix.MatrixName] =? And [Test.TestName] =? And [Method.MethodNumber]=? And [Component.Components]=? And [SampleMatrix.VisualMatrixName]=?", objcq.Oid, objtestparams.Matrix.MatrixName, objtestparams.Test.TestName, objtestparams.Method.MethodNumber, objtestparams.Component.Components, objtestparams.SampleMatrix.VisualMatrixName));
                                    }
                                    else
                                    {
                                        objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ? And [Matrix.MatrixName] =? And [Test.TestName] =? And [Method.MethodNumber]=? And [Component.Components]=?", objcq.Oid, objtestparams.Matrix.MatrixName, objtestparams.Test.TestName, objtestparams.Method.MethodNumber, objtestparams.Component.Components));
                                    }
                                    //AnalysisPricing objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ? And [Matrix.MatrixName] =? And [Test.TestName] =? And [Method.MethodNumber]=? And [Component.Components]=?", objcq.Oid, objtestparams.Matrix.MatrixName, objtestparams.Test.TestName, objtestparams.Method.MethodNumber, objtestparams.Component.Components)); //And [Priority.Prioritys] = 'Regular'
                                    if (objanalysisprice == null)
                                    {
                                        if (objtestparams.Test != null && objtestparams.Component != null)
                                        {
                                            TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And[Component.Components] = ? And [Priority.IsRegular] = 'True'", objtestparams.Matrix.MatrixName, objtestparams.Test.TestName, objtestparams.Method.MethodNumber, objtestparams.Component.Components));
                                            if (objtstpricesur != null)
                                            //List<TestPriceSurcharge> lsttstpricing = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Test.TestName] = ? And [Component.Components] = ?", objtestparams.Matrix.MatrixName, objtestparams.Test.TestName, objtestparams.Method.MethodNumber, objtestparams.Component.Components)).ToList();
                                            //if (lsttstpricing != null && lsttstpricing.Count > 0)
                                            {
                                                newanalysisprice = Application.MainWindow.View.ObjectSpace.CreateObject<AnalysisPricing>();
                                                newanalysisprice.PriceCode = objtestparams.PriceCode;
                                                newanalysisprice.Qty = 1;
                                                newanalysisprice.Matrix = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Matrix);
                                                newanalysisprice.Test = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Test);
                                                newanalysisprice.TestDescription = objtstpricesur.Test.Comment;
                                                newanalysisprice.IsGroup = objtstpricesur.IsGroup;
                                                newanalysisprice.Method = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Method.MethodName);
                                                newanalysisprice.Component = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Component);
                                                newanalysisprice.Parameter = "AllParams";
                                                //DefaultPricing objdefpricing = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objtstpricesur.Matrix.MatrixName, objtstpricesur.Test.TestName, objtstpricesur.Method.MethodName.MethodNumber, objtstpricesur.Component.Components));
                                                //ConstituentPricing objconstipricing = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And[Test.TestName] = ? And[Method.MethodNumber] = ? And [Component.Components] = ?", objtstpricesur.Matrix.MatrixName, objtstpricesur.Test.TestName, objtstpricesur.Method.MethodName.MethodNumber, objtstpricesur.Component.Components));
                                                //DefaultPricing objdefpricing = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objtstpricesur.Test.Oid));
                                                //ConstituentPricing objconstipricing = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objtstpricesur.Test.Oid));
                                                //if (objconstipricing != null)
                                                //{
                                                //    newanalysisprice.ChargeType = objconstipricing.ChargeType;
                                                //    decimal tierprc = 0;
                                                //    decimal unitpriceamt = 0;
                                                //    int paracnt = 0;
                                                //    if (objconstipricing.Test != null && objconstipricing.Component != null)
                                                //    {
                                                //        List<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Component.Oid] = ? And [QCType.QCTypeName] = 'Sample'", objtestparams.Test.Oid, objtestparams.Component.Oid)).ToList();
                                                //        if (lsttestpara.Count > 0)
                                                //        {
                                                //            paracnt = lsttestpara.Count;
                                                //        }
                                                //    }
                                                //    if (objconstipricing.ChargeType == ChargeType.Parameter)
                                                //    {
                                                //        ConstituentPricingTier lstconstituentpricrtier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And[To] >= ?", objconstipricing.Oid, paracnt, paracnt));
                                                //        if (lstconstituentpricrtier != null)
                                                //        {
                                                //            unitpriceamt = lstconstituentpricrtier.TierPrice * paracnt;
                                                //            newanalysisprice.TierNo = lstconstituentpricrtier.TierNo;
                                                //            newanalysisprice.From = lstconstituentpricrtier.From;
                                                //            newanalysisprice.To = lstconstituentpricrtier.To;
                                                //            newanalysisprice.TierPrice = lstconstituentpricrtier.TierPrice;
                                                //        }
                                                //    }
                                                //    else if (objconstipricing.ChargeType == ChargeType.Test)
                                                //    {
                                                //        ConstituentPricingTier objtierprice = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And[To] >= ?", objconstipricing.Oid, newanalysisprice.Qty, newanalysisprice.Qty));
                                                //        //IList<ConstituentPricingTier> objtierprice = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And[To] >= ?", objconstipricing.Oid, newanalysisprice.Qty, newanalysisprice.Qty));
                                                //        if (objtierprice != null)
                                                //        {
                                                //            newanalysisprice.TierNo = objtierprice.TierNo;
                                                //            newanalysisprice.From = objtierprice.From;
                                                //            newanalysisprice.To = objtierprice.To;
                                                //            newanalysisprice.TierPrice = objtierprice.TierPrice;
                                                //            unitpriceamt = objtierprice.TierPrice;
                                                //        }
                                                //    }
                                                //    newanalysisprice.UnitPrice = unitpriceamt;
                                                //    newanalysisprice.NPUnitPrice = unitpriceamt;
                                                //    newanalysisprice.NPTotalPrice = unitpriceamt /** paracnt*/;
                                                //    newanalysisprice.Discount = 0;
                                                //    List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                //    List<Guid> lsttatoid = new List<Guid>();
                                                //    List<Guid> lsttatinfo = new List<Guid>();
                                                //    List<Priority> lstpriority = new List<Priority>();
                                                //    TurnAroundTime objtpstat = null;
                                                //    if (objtstpricesur != null && !string.IsNullOrEmpty(objtstpricesur.TAT))
                                                //    {
                                                //        string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                //        if (strTAToidarr.Length > 1)
                                                //        {
                                                //            foreach (string objoid in strTAToidarr.ToList())
                                                //            {
                                                //                lsttatoid.Add(new Guid(objoid));
                                                //            }
                                                //        }
                                                //        else
                                                //        {
                                                //            lsttatoid.Add(new Guid(objtstpricesur.TAT));
                                                //        }
                                                //    }
                                                //    ////List<TestPriceSurcharge> lsttstpricing = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And[Component.Components] = ?", objtestparams.Matrix.MatrixName, objtestparams.Test.TestName, objtestparams.Method.MethodNumber, objtestparams.Component.Components)).ToList();
                                                //    ////if (lsttstpricing != null && lsttstpricing.Count > 0)
                                                //    ////{
                                                //    ////    foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                                //    ////    {
                                                //    ////        string[] strTAToidarr = objtstpricesurcharge.TAT.Split(';');
                                                //    ////        if (strTAToidarr.Length > 1)
                                                //    ////        {
                                                //    ////            foreach (string objoid in strTAToidarr.ToList())
                                                //    ////            {
                                                //    ////                lsttatinfo.Add(new Guid(objoid));
                                                //    ////            }
                                                //    ////        }
                                                //    ////        else
                                                //    ////        {
                                                //    ////            lsttatinfo.Add(new Guid(objtstpricesurcharge.TAT));
                                                //    ////        }
                                                //    ////    }
                                                //    ////}
                                                //    ////if(lsttatinfo.Count > 0)
                                                //    ////{
                                                //    ////    tatinfo.lsttat = new List<TurnAroundTime>();
                                                //    ////    IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("Oid", lsttatinfo));
                                                //    ////    foreach (TurnAroundTime objturntime in lstturntime.ToList())
                                                //    ////    {
                                                //    ////        tatinfo.lsttat.Add(objturntime);
                                                //    ////    }
                                                //    ////}
                                                //    if (lsttatoid.Count > 0)
                                                //    {
                                                //        IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("Oid", lsttatoid));
                                                //        foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                //        {
                                                //            newanalysisprice.TAT = ObjectSpace.GetObject(objturntime);
                                                //            break;
                                                //        }
                                                //    }
                                                //    //newanalysisprice.TAT = ObjectSpace.GetObject(objtestparams.TAT);
                                                //    newanalysisprice.TotalTierPrice = unitpriceamt /** paracnt*/;
                                                //    newanalysisprice.FinalAmount = unitpriceamt/* * paracnt*/;
                                                //    //Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[IsRegular] = 'True'"));
                                                //    Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtestparams.Priority.Oid));
                                                //    if (objpriority != null)
                                                //    {
                                                //        newanalysisprice.Priority = Application.MainWindow.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                //    }
                                                //    newanalysisprice.CRMQuotes = Application.MainWindow.View.ObjectSpace.GetObject(objcq);
                                                //    newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                //}
                                                //else
                                                //if (objdefpricing != null)
                                                //{
                                                //    newanalysisprice.ChargeType = objdefpricing.ChargeType;
                                                //    newanalysisprice.UnitPrice = objdefpricing.UnitPrice;
                                                //    newanalysisprice.NPUnitPrice = objdefpricing.UnitPrice;
                                                //    newanalysisprice.NPTotalPrice = objdefpricing.UnitPrice;
                                                //    newanalysisprice.Discount = 0;
                                                //    //newanalysisprice.TotalTierPrice = objdefpricing.UnitPrice;
                                                //    newanalysisprice.TotalTierPrice =(decimal) objtstpricesur.SurchargePrice;
                                                //    newanalysisprice.FinalAmount = objdefpricing.UnitPrice;
                                                //    List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                //    List<Guid> lsttatoid = new List<Guid>();
                                                //    List<Priority> lstpriority = new List<Priority>();
                                                //    TurnAroundTime objtpstat = null;
                                                //    //foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                                //    {
                                                //        string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                //        if (strTAToidarr.Length > 1)
                                                //        {
                                                //            foreach (string objoid in strTAToidarr.ToList())
                                                //            {
                                                //                lsttatoid.Add(new Guid(objoid));
                                                //            }
                                                //        }
                                                //        else
                                                //        {
                                                //            lsttatoid.Add(new Guid(objtstpricesur.TAT));
                                                //        }
                                                //    }
                                                //    if (lsttatoid.Count > 0)
                                                //    {
                                                //        IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("Oid", lsttatoid));
                                                //        foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                //        {
                                                //            newanalysisprice.TAT = ObjectSpace.GetObject(objturntime);
                                                //            break;
                                                //        }
                                                //    }
                                                //    //Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[IsRegular] = 'True'"));
                                                //    Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtestparams.Priority.Oid));
                                                //    if (objpriority != null)
                                                //    {
                                                //        newanalysisprice.Priority = Application.MainWindow.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                //    }
                                                //    newanalysisprice.CRMQuotes = Application.MainWindow.View.ObjectSpace.GetObject<CRMQuotes>(objcq);
                                                //    newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                //}
                                                newanalysisprice.ChargeType = objtstpricesur.ChargeType;
                                                if (objtstpricesur.SurchargePrice != null)
                                                {
                                                    newanalysisprice.UnitPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    newanalysisprice.NPUnitPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    newanalysisprice.NPTotalPrice = (decimal)objtstpricesur.SurchargePrice;
                                                }
                                                newanalysisprice.Discount = 0;
                                                //newanalysisprice.TotalTierPrice = objdefpricing.UnitPrice;
                                                if (objtstpricesur.SurchargePrice != null)
                                                {
                                                    newanalysisprice.TotalTierPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    newanalysisprice.FinalAmount = (decimal)objtstpricesur.SurchargePrice;
                                                }
                                                List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                List<string> lsttatoid = new List<string>();
                                                List<Priority> lstpriority = new List<Priority>();
                                                TurnAroundTime objtpstat = null;
                                                //foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                                {
                                                    string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                    if (strTAToidarr.Length > 1)
                                                    {
                                                        foreach (string objoid in strTAToidarr.ToList())
                                                        {
                                                            lsttatoid.Add((objoid));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lsttatoid.Add((objtstpricesur.TAT));
                                                    }
                                                }
                                                if (lsttatoid.Count > 0)
                                                {
                                                    IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatoid));
                                                    foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                    {
                                                        newanalysisprice.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objturntime);
                                                        break;
                                                    }
                                                }
                                                Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtestparams.Priority.Oid));
                                                if (objpriority != null)
                                                {
                                                    newanalysisprice.Priority = Application.MainWindow.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                }
                                                newanalysisprice.CRMQuotes = Application.MainWindow.View.ObjectSpace.GetObject<CRMQuotes>(objcq);
                                                newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                newanalysisprice.SampleMatrix = Application.MainWindow.View.ObjectSpace.GetObject(objtestparams.SampleMatrix);
                                                objcq.AnalysisPricing.Add(newanalysisprice);
                                                ((ListView)View).CollectionSource.Add(newanalysisprice);
                                            }
                                        }
                                        else if (objtestparams.Test != null && objtestparams.Component == null && objtestparams.IsGroup == true)
                                        {
                                            TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'True' And [Priority.IsRegular] = 'True'", objtestparams.Matrix.MatrixName, objtestparams.Test.TestName)); //IsRegular
                                            if (objtstpricesur != null)
                                            //List<TestPriceSurcharge> lsttstpricing = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Test.Oid] = ? And [IsGroup] = 'True'", objtestparams.Test.Oid)).ToList();
                                            //if (lsttstpricing != null && lsttstpricing.Count > 0)
                                            {
                                                //DefaultPricing objdefpricing = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objtestparams.Test.Oid));
                                                //if (objdefpricing != null)
                                                //{
                                                //    newanalysisprice = Application.MainWindow.View.ObjectSpace.CreateObject<AnalysisPricing>();
                                                //    newanalysisprice.PriceCode = objtestparams.PriceCode;
                                                //    newanalysisprice.Qty = 1;
                                                //    newanalysisprice.Matrix = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Matrix);
                                                //    newanalysisprice.Test = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Test);
                                                //    newanalysisprice.TestDescription = objtstpricesur.Test.Comment;
                                                //    newanalysisprice.IsGroup = objtstpricesur.IsGroup;
                                                //    newanalysisprice.Parameter = "AllParams";
                                                //    newanalysisprice.ChargeType = objdefpricing.ChargeType;
                                                //    newanalysisprice.UnitPrice = objdefpricing.UnitPrice;
                                                //    newanalysisprice.NPUnitPrice = objdefpricing.UnitPrice;
                                                //    newanalysisprice.NPTotalPrice = objdefpricing.UnitPrice;
                                                //    newanalysisprice.Discount = 0;
                                                //    //newanalysisprice.TotalTierPrice = objdefpricing.UnitPrice;
                                                //    newanalysisprice.TotalTierPrice = (decimal)objtstpricesur.SurchargePrice;
                                                //    newanalysisprice.FinalAmount = objdefpricing.UnitPrice;
                                                //    List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                //    List<Guid> lsttatoid = new List<Guid>();
                                                //    List<Priority> lstpriority = new List<Priority>();
                                                //    TurnAroundTime objtpstat = null;
                                                //    //foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                                //    {
                                                //        string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                //        if (strTAToidarr.Length > 1)
                                                //        {
                                                //            foreach (string objoid in strTAToidarr.ToList())
                                                //            {
                                                //                lsttatoid.Add(new Guid(objoid));
                                                //            }
                                                //        }
                                                //        else
                                                //        {
                                                //            lsttatoid.Add(new Guid(objtstpricesur.TAT));
                                                //        }
                                                //    }
                                                //    if (lsttatoid.Count > 0)
                                                //    {
                                                //        IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("Oid", lsttatoid));
                                                //        foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                //        {
                                                //            newanalysisprice.TAT = ObjectSpace.GetObject(objturntime);
                                                //            break;
                                                //        }
                                                //    }
                                                //    //Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[IsRegular] = 'True'"));
                                                //    Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtestparams.Priority.Oid));
                                                //    if (objpriority != null)
                                                //    {
                                                //        newanalysisprice.Priority = Application.MainWindow.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                //    }
                                                //    newanalysisprice.CRMQuotes = Application.MainWindow.View.ObjectSpace.GetObject(objcq);
                                                //    newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                //    objcq.AnalysisPricing.Add(newanalysisprice);
                                                //}
                                                newanalysisprice = Application.MainWindow.View.ObjectSpace.CreateObject<AnalysisPricing>();
                                                newanalysisprice.PriceCode = objtestparams.PriceCode;
                                                newanalysisprice.Qty = 1;
                                                newanalysisprice.Matrix = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Matrix);
                                                newanalysisprice.Test = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Test);
                                                newanalysisprice.TestDescription = objtstpricesur.Test.Comment;
                                                newanalysisprice.IsGroup = objtstpricesur.IsGroup;
                                                newanalysisprice.Parameter = "AllParams";
                                                newanalysisprice.ChargeType = objtstpricesur.ChargeType;
                                                if (objtstpricesur.SurchargePrice != null)
                                                {
                                                    newanalysisprice.UnitPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    newanalysisprice.NPUnitPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    newanalysisprice.NPTotalPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    newanalysisprice.TotalTierPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    newanalysisprice.TotalTierPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    newanalysisprice.FinalAmount = (decimal)objtstpricesur.SurchargePrice;
                                                }
                                                newanalysisprice.Discount = 0;

                                                List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                List<string> lsttatoid = new List<string>();
                                                List<Priority> lstpriority = new List<Priority>();
                                                TurnAroundTime objtpstat = null;
                                                //foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                                {
                                                    string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                    if (strTAToidarr.Length > 1)
                                                    {
                                                        foreach (string objoid in strTAToidarr.ToList())
                                                        {
                                                            lsttatoid.Add((objoid));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lsttatoid.Add((objtstpricesur.TAT));
                                                    }
                                                }
                                                if (lsttatoid.Count > 0)
                                                {
                                                    IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatoid));
                                                    foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                    {
                                                        newanalysisprice.TAT = ObjectSpace.GetObject(objturntime);
                                                        break;
                                                    }
                                                }
                                                //Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[IsRegular] = 'True'"));
                                                Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtestparams.Priority.Oid));
                                                if (objpriority != null)
                                                {
                                                    newanalysisprice.Priority = Application.MainWindow.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                }
                                                newanalysisprice.CRMQuotes = Application.MainWindow.View.ObjectSpace.GetObject(objcq);
                                                newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                objcq.AnalysisPricing.Add(newanalysisprice);

                                            }
                                            ((ListView)View).CollectionSource.Add(newanalysisprice);
                                        }
                                    }
                                }
                                else if (objtestparams != null && objtestparams.Matrix != null && objtestparams.Test != null && objtestparams.IsGroup == true)
                                {
                                    //AnalysisPricing objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ? And [Matrix.MatrixName] =? And [Test.TestName] =? And [IsGroup] = 'True'", objcq.Oid, objtestparams.Matrix.MatrixName, objtestparams.Test.TestName)); //And [Priority.Prioritys] = 'Regular'
                                    AnalysisPricing objanalysisprice = null;
                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ? And [Matrix.MatrixName] =? And [Test.TestName] =? And [IsGroup] = 'True'", objcq.Oid, objtestparams.Matrix.MatrixName, objtestparams.Test.TestName));
                                    }
                                    else
                                    {
                                        objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ? And [Matrix.MatrixName] =? And [Test.TestName] =? And [IsGroup] = 'True'", objcq.Oid, objtestparams.Matrix.MatrixName, objtestparams.Test.TestName));
                                    }
                                    if (objanalysisprice == null)
                                    {
                                        if (objtestparams.Test != null && objtestparams.Component != null)
                                        {
                                            TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'True' And [Priority.IsRegular] = 'True'", objtestparams.Matrix.MatrixName, objtestparams.Test.TestName)); //IsRegular
                                            if (objtstpricesur != null)
                                            //List<TestPriceSurcharge> lsttstpricing = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Test.TestName] = ? And [Component.Components] = ?", objtestparams.Matrix.MatrixName, objtestparams.Test.TestName, objtestparams.Method.MethodNumber, objtestparams.Component.Components)).ToList();
                                            //if (lsttstpricing != null && lsttstpricing.Count > 0)
                                            {
                                                newanalysisprice = Application.MainWindow.View.ObjectSpace.CreateObject<AnalysisPricing>();
                                                newanalysisprice.PriceCode = objtestparams.PriceCode;
                                                newanalysisprice.Qty = 1;
                                                newanalysisprice.Matrix = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Matrix);
                                                newanalysisprice.Test = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Test);
                                                newanalysisprice.TestDescription = objtstpricesur.Test.Comment;
                                                newanalysisprice.IsGroup = objtstpricesur.IsGroup;
                                                newanalysisprice.Method = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Method.MethodName);
                                                newanalysisprice.Component = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Component);
                                                newanalysisprice.Parameter = "AllParams";
                                                DefaultPricing objdefpricing = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objtstpricesur.Test.Oid));
                                                //ConstituentPricing objconstipricing = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objtstpricesur.Test.Oid));
                                                ConstituentPricing objconstipricing = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And[Test.TestName] = ? And[Method.MethodNumber] = ? And [Component.Components] = ?", objtstpricesur.Matrix.MatrixName, objtstpricesur.Test.TestName, objtstpricesur.Method.MethodName.MethodNumber, objtstpricesur.Component.Components));
                                                if (objconstipricing != null)
                                                {
                                                    newanalysisprice.ChargeType = objconstipricing.ChargeType;
                                                    decimal tierprc = 0;
                                                    decimal unitpriceamt = 0;
                                                    int paracnt = 0;
                                                    if (objconstipricing.Test != null && objconstipricing.Component != null)
                                                    {
                                                        List<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Component.Oid] = ? And [QCType.QCTypeName] = 'Sample'", objtestparams.Test.Oid, objtestparams.Component.Oid)).ToList();
                                                        if (lsttestpara.Count > 0)
                                                        {
                                                            paracnt = lsttestpara.Count;
                                                        }
                                                    }
                                                    if (objconstipricing.ChargeType == ChargeType.Parameter)
                                                    {
                                                        ConstituentPricingTier lstconstituentpricrtier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And[To] >= ?", objconstipricing.Oid, paracnt, paracnt));
                                                        if (lstconstituentpricrtier != null)
                                                        {
                                                            unitpriceamt = lstconstituentpricrtier.TierPrice * paracnt;
                                                            newanalysisprice.TierNo = lstconstituentpricrtier.TierNo;
                                                            newanalysisprice.From = lstconstituentpricrtier.From;
                                                            newanalysisprice.To = lstconstituentpricrtier.To;
                                                            newanalysisprice.TierPrice = lstconstituentpricrtier.TierPrice;
                                                        }
                                                    }
                                                    else if (objconstipricing.ChargeType == ChargeType.Test)
                                                    {
                                                        ConstituentPricingTier objtierprice = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And[To] >= ?", objconstipricing.Oid, newanalysisprice.Qty, newanalysisprice.Qty));
                                                        if (objtierprice != null)
                                                        {
                                                            newanalysisprice.TierNo = objtierprice.TierNo;
                                                            newanalysisprice.From = objtierprice.From;
                                                            newanalysisprice.To = objtierprice.To;
                                                            newanalysisprice.TierPrice = objtierprice.TierPrice;
                                                            unitpriceamt = objtierprice.TierPrice;
                                                        }
                                                    }
                                                    newanalysisprice.UnitPrice = unitpriceamt;
                                                    newanalysisprice.NPUnitPrice = unitpriceamt;
                                                    newanalysisprice.NPTotalPrice = unitpriceamt /** paracnt*/;
                                                    newanalysisprice.Discount = 0;
                                                    List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                    List<string> lsttatoid = new List<string>();
                                                    List<Guid> lsttatinfo = new List<Guid>();
                                                    List<Priority> lstpriority = new List<Priority>();
                                                    TurnAroundTime objtpstat = null;
                                                    if (objtstpricesur != null && !string.IsNullOrEmpty(objtstpricesur.TAT))
                                                    {
                                                        string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                        if (strTAToidarr.Length > 1)
                                                        {
                                                            foreach (string objoid in strTAToidarr.ToList())
                                                            {
                                                                lsttatoid.Add((objoid));
                                                            }
                                                        }
                                                        else
                                                        {
                                                            lsttatoid.Add((objtstpricesur.TAT));
                                                        }
                                                    }
                                                    ////List<TestPriceSurcharge> lsttstpricing = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And[Component.Components] = ?", objtestparams.Matrix.MatrixName, objtestparams.Test.TestName, objtestparams.Method.MethodNumber, objtestparams.Component.Components)).ToList();
                                                    ////if (lsttstpricing != null && lsttstpricing.Count > 0)
                                                    ////{
                                                    ////    foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                                    ////    {
                                                    ////        string[] strTAToidarr = objtstpricesurcharge.TAT.Split(';');
                                                    ////        if (strTAToidarr.Length > 1)
                                                    ////        {
                                                    ////            foreach (string objoid in strTAToidarr.ToList())
                                                    ////            {
                                                    ////                lsttatinfo.Add(new Guid(objoid));
                                                    ////            }
                                                    ////        }
                                                    ////        else
                                                    ////        {
                                                    ////            lsttatinfo.Add(new Guid(objtstpricesurcharge.TAT));
                                                    ////        }
                                                    ////    }
                                                    ////}
                                                    ////if(lsttatinfo.Count > 0)
                                                    ////{
                                                    ////    tatinfo.lsttat = new List<TurnAroundTime>();
                                                    ////    IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("Oid", lsttatinfo));
                                                    ////    foreach (TurnAroundTime objturntime in lstturntime.ToList())
                                                    ////    {
                                                    ////        tatinfo.lsttat.Add(objturntime);
                                                    ////    }
                                                    ////}
                                                    if (lsttatoid.Count > 0)
                                                    {
                                                        IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatoid));
                                                        foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                        {
                                                            newanalysisprice.TAT = ObjectSpace.GetObject(objturntime);
                                                            break;
                                                        }
                                                    }
                                                    //newanalysisprice.TAT = ObjectSpace.GetObject(objtestparams.TAT);
                                                    newanalysisprice.TotalTierPrice = unitpriceamt /** paracnt*/;
                                                    newanalysisprice.FinalAmount = unitpriceamt/* * paracnt*/;
                                                    //Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[IsRegular] = 'True'"));
                                                    Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtstpricesur.Priority.Oid));
                                                    if (objpriority != null)
                                                    {
                                                        newanalysisprice.Priority = Application.MainWindow.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                    }
                                                    newanalysisprice.CRMQuotes = Application.MainWindow.View.ObjectSpace.GetObject(objcq);
                                                    newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                }
                                                else
                                                if (objdefpricing != null)
                                                {
                                                    newanalysisprice.ChargeType = objdefpricing.ChargeType;
                                                    newanalysisprice.UnitPrice = objdefpricing.UnitPrice;
                                                    newanalysisprice.NPUnitPrice = objdefpricing.UnitPrice;
                                                    newanalysisprice.NPTotalPrice = objdefpricing.UnitPrice;
                                                    newanalysisprice.Discount = 0;
                                                    //newanalysisprice.TotalTierPrice = objdefpricing.UnitPrice;
                                                    if (objtstpricesur.SurchargePrice != null)
                                                    {
                                                        newanalysisprice.TotalTierPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    }
                                                    newanalysisprice.FinalAmount = objdefpricing.UnitPrice;
                                                    List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                    List<string> lsttatoid = new List<string>();
                                                    List<Priority> lstpriority = new List<Priority>();
                                                    TurnAroundTime objtpstat = null;
                                                    //foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                                    {
                                                        string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                        if (strTAToidarr.Length > 1)
                                                        {
                                                            foreach (string objoid in strTAToidarr.ToList())
                                                            {
                                                                lsttatoid.Add((objoid));
                                                            }
                                                        }
                                                        else
                                                        {
                                                            lsttatoid.Add((objtstpricesur.TAT));
                                                        }
                                                    }
                                                    if (lsttatoid.Count > 0)
                                                    {
                                                        IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatoid));
                                                        foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                        {
                                                            newanalysisprice.TAT = ObjectSpace.GetObject(objturntime);
                                                            break;
                                                        }
                                                    }
                                                    //Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[IsRegular] = 'True'"));
                                                    Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtstpricesur.Priority.Oid));
                                                    if (objpriority != null)
                                                    {
                                                        newanalysisprice.Priority = Application.MainWindow.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                    }
                                                    newanalysisprice.CRMQuotes = Application.MainWindow.View.ObjectSpace.GetObject<CRMQuotes>(objcq);
                                                    newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                }
                                                objcq.AnalysisPricing.Add(newanalysisprice);
                                                ((ListView)View).CollectionSource.Add(newanalysisprice);
                                            }
                                        }
                                        else if (objtestparams.Test != null && objtestparams.Component == null && objtestparams.IsGroup == true)
                                        {
                                            TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'True' And [Priority.IsRegular] = 'True'", objtestparams.Matrix.MatrixName, objtestparams.Test.TestName)); //IsRegular
                                            if (objtstpricesur != null)
                                            {
                                                newanalysisprice = Application.MainWindow.View.ObjectSpace.CreateObject<AnalysisPricing>();
                                                newanalysisprice.PriceCode = objtestparams.PriceCode;
                                                newanalysisprice.Qty = 1;
                                                newanalysisprice.Matrix = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Matrix);
                                                newanalysisprice.Test = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Test);
                                                newanalysisprice.SampleMatrix = Application.MainWindow.View.ObjectSpace.GetObject(objtestparams.SampleMatrix);
                                                newanalysisprice.TestDescription = objtstpricesur.Test.Comment;
                                                newanalysisprice.IsGroup = objtstpricesur.IsGroup;
                                                newanalysisprice.Parameter = "AllParams";
                                                newanalysisprice.ChargeType = objtstpricesur.ChargeType;
                                                if (objtstpricesur.SurchargePrice != null)
                                                {
                                                    newanalysisprice.UnitPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    newanalysisprice.NPUnitPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    newanalysisprice.NPTotalPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    newanalysisprice.TotalTierPrice = (decimal)objtstpricesur.SurchargePrice;
                                                    newanalysisprice.FinalAmount = (decimal)objtstpricesur.SurchargePrice;
                                                }
                                                newanalysisprice.Discount = 0;
                                                //newanalysisprice.TotalTierPrice = objdefpricing.UnitPrice;

                                                List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                                List<string> lsttatoid = new List<string>();
                                                List<Priority> lstpriority = new List<Priority>();
                                                TurnAroundTime objtpstat = null;
                                                //foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                                {
                                                    string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                                    if (strTAToidarr.Length > 1)
                                                    {
                                                        foreach (string objoid in strTAToidarr.ToList())
                                                        {
                                                            lsttatoid.Add((objoid));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lsttatoid.Add((objtstpricesur.TAT));
                                                    }
                                                }
                                                if (lsttatoid.Count > 0)
                                                {
                                                    IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatoid));
                                                    foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                                    {
                                                        newanalysisprice.TAT = ObjectSpace.GetObject(objturntime);
                                                        break;
                                                    }
                                                }
                                                //Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[IsRegular] = 'True'"));
                                                Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtstpricesur.Priority.Oid));
                                                if (objpriority != null)
                                                {
                                                    newanalysisprice.Priority = Application.MainWindow.View.ObjectSpace.GetObject<Priority>(objpriority);
                                                }
                                                newanalysisprice.CRMQuotes = Application.MainWindow.View.ObjectSpace.GetObject(objcq);
                                                newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                                objcq.AnalysisPricing.Add(newanalysisprice);
                                            }
                                            //List<TestPriceSurcharge> lsttstpricing = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Test.Oid] = ? And [IsGroup] = 'True'", objtestparams.Test.Oid)).ToList();
                                            //if (lsttstpricing != null && lsttstpricing.Count > 0)
                                            //{
                                            //DefaultPricing objdefpricing = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ?", objtestparams.Test.Oid));
                                            //if (objdefpricing != null)
                                            //{
                                            //    newanalysisprice = Application.MainWindow.View.ObjectSpace.CreateObject<AnalysisPricing>();
                                            //    newanalysisprice.PriceCode = objtestparams.PriceCode;
                                            //    newanalysisprice.Qty = 1;
                                            //    newanalysisprice.Matrix = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Matrix);
                                            //    newanalysisprice.Test = Application.MainWindow.View.ObjectSpace.GetObject(objtstpricesur.Test);
                                            //    newanalysisprice.TestDescription = objtstpricesur.Test.Comment;
                                            //    newanalysisprice.IsGroup = objtstpricesur.IsGroup;
                                            //    newanalysisprice.Parameter = "AllParams";
                                            //    newanalysisprice.ChargeType = objdefpricing.ChargeType;
                                            //    newanalysisprice.UnitPrice = objdefpricing.UnitPrice;
                                            //    newanalysisprice.NPUnitPrice = objdefpricing.UnitPrice;
                                            //    newanalysisprice.NPTotalPrice = objdefpricing.UnitPrice;
                                            //    newanalysisprice.Discount = 0;
                                            //    //newanalysisprice.TotalTierPrice = objdefpricing.UnitPrice;
                                            //    newanalysisprice.TotalTierPrice = (decimal)objtstpricesur.SurchargePrice;
                                            //    newanalysisprice.FinalAmount = objdefpricing.UnitPrice;
                                            //    List<TurnAroundTime> lsttat = new List<TurnAroundTime>();
                                            //    List<Guid> lsttatoid = new List<Guid>();
                                            //    List<Priority> lstpriority = new List<Priority>();
                                            //    TurnAroundTime objtpstat = null;
                                            //    //foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                            //    {
                                            //        string[] strTAToidarr = objtstpricesur.TAT.Split(';');
                                            //        if (strTAToidarr.Length > 1)
                                            //        {
                                            //            foreach (string objoid in strTAToidarr.ToList())
                                            //            {
                                            //                lsttatoid.Add(new Guid(objoid));
                                            //            }
                                            //        }
                                            //        else
                                            //        {
                                            //            lsttatoid.Add(new Guid(objtstpricesur.TAT));
                                            //        }
                                            //    }
                                            //    if (lsttatoid.Count > 0)
                                            //    {
                                            //        IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("Oid", lsttatoid));
                                            //        foreach (TurnAroundTime objturntime in lstturntime.OrderByDescending(i => i.Count))
                                            //        {
                                            //            newanalysisprice.TAT = ObjectSpace.GetObject(objturntime);
                                            //            break;
                                            //        }
                                            //    }
                                            //    //Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[IsRegular] = 'True'"));
                                            //    Priority objpriority = ObjectSpace.FindObject<Priority>(CriteriaOperator.Parse("[Oid] = ?", objtestparams.Priority.Oid));
                                            //    if (objpriority != null)
                                            //    {
                                            //        newanalysisprice.Priority = Application.MainWindow.View.ObjectSpace.GetObject<Priority>(objpriority);
                                            //    }
                                            //    newanalysisprice.CRMQuotes = Application.MainWindow.View.ObjectSpace.GetObject(objcq);
                                            //    newanalysisprice.TestDescription = objtestparams.Test.Comment;
                                            //    objcq.AnalysisPricing.Add(newanalysisprice);
                                            //}
                                            //}
                                            ((ListView)View).CollectionSource.Add(newanalysisprice);
                                        }
                                    }
                                }
                            }
                            ((ListView)View).Refresh();
                            quotesinfo.IsobjChangedpropertyinQuotes = true;
                            decimal finalamt = 0;
                            decimal totalprice = 0;
                            decimal disamt = 0;
                            decimal dispr = 0;
                            ListPropertyEditor lstAnalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                            ListPropertyEditor lstitemprice = ((DetailView)Application.MainWindow.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                            if (lstitemprice != null)
                            {
                                if (lstitemprice.ListView == null)
                                {
                                    lstitemprice.CreateControl();
                                }
                                if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                {
                                    finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.FinalAmount);
                                    totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.UnitPrice * i.Qty);
                                }
                            }
                            if (lstAnalysisprice != null)
                            {
                                if (lstAnalysisprice.ListView == null)
                                {
                                    lstAnalysisprice.CreateControl();
                                }
                                if (lstAnalysisprice.ListView.CollectionSource.GetCount() > 0)
                                {
                                    finalamt = finalamt + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.FinalAmount);
                                    totalprice = totalprice + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.TotalTierPrice * i.Qty);
                                }
                            }
                            /*DetailviewRefresh*/
                            //CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                            quotesinfo.IsTabDiscountChanged = true;
                            CRMQuotes crtquotes = quotesinfo.popupcurtquote;
                            if (crtquotes != null)
                            {
                                if (finalamt != 0 && totalprice != 0)
                                {
                                    disamt = totalprice - finalamt;
                                    if (disamt != 0)
                                    {
                                        dispr = ((disamt) / totalprice) * 100;
                                    }
                                    //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                    //disamt = finalamt - totalprice;
                                    quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                    crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                    crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                    crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                    crtquotes.Discount = Math.Round(dispr, 2);
                                    crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                    crtquotes.IsGobalDiscount = true;
                                }
                                else if (finalamt == 0 && totalprice == 0)
                                {
                                    crtquotes.DetailedAmount = 0;
                                    crtquotes.TotalAmount = 0;
                                    crtquotes.Discount = 0;
                                    crtquotes.DiscountAmount = 0;
                                    crtquotes.QuotedAmount = 0;
                                    crtquotes.IsGobalDiscount = true;
                                }
                            }
                            Application.MainWindow.View.Refresh();
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Select atleast one checkbox.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (View.Id == "CRMQuotes_ListView_pendingsubmission")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing; ;
                }
                //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executed += DeleteAction_Executed;
                if (View.Id == "CRMQuotes_ListView_pendingsubmission" || View.Id == "CRMQuotes_ListView_PendingReview" || View.Id == "CRMQuotes_ListView_Expired" || View.Id == "CRMQuotes_ListView_Cancel")
                {
                    Employee currentUser = SecuritySystem.CurrentUser as Employee;
                    if (currentUser != null && View != null && View.Id != null)
                    {
                        if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                        {
                            objPermissionInfo.OpenQuotesIsWrite = false;
                            objPermissionInfo.QuotesReviewIsWrite = false;
                            objPermissionInfo.CancelledQuotesIsWrite = false;
                            objPermissionInfo.ExpiredQuotesIsWrite = false;
                            if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                            {
                                objPermissionInfo.OpenQuotesIsWrite = true;
                                objPermissionInfo.QuotesReviewIsWrite = true;
                                objPermissionInfo.CancelledQuotesIsWrite = true;
                                objPermissionInfo.ExpiredQuotesIsWrite = true;
                            }
                            else
                            {
                                foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                                {
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "OpenQuotes" && i.Write == true) != null)
                                    {
                                        objPermissionInfo.OpenQuotesIsWrite = true;
                                    }
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "QuotesReview" && i.Write == true) != null)
                                    {
                                        objPermissionInfo.QuotesReviewIsWrite = true;
                                    }
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "CanceledQuotes" && i.Write == true) != null)
                                    {
                                        objPermissionInfo.CancelledQuotesIsWrite = true;
                                    }
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ExpiredQuotes" && i.Write == true) != null)
                                    {
                                        objPermissionInfo.ExpiredQuotesIsWrite = true;
                                    }
                                }
                            }
                        }
                        if (View.Id == "CRMQuotes_ListView_pendingsubmission")
                        {
                            QuoteSubmit.Active["showSubmit"] = objPermissionInfo.OpenQuotesIsWrite;
                        }
                        if (View.Id == "CRMQuotes_ListView_PendingReview")
                        {
                            QuoteRollback.Active["ShowRollback"] = objPermissionInfo.QuotesReviewIsWrite;
                            QuoteReview.Active["ShowReview"] = objPermissionInfo.QuotesReviewIsWrite;
                        }
                        if (View.Id == "CRMQuotes_ListView_Cancel")
                        {
                            QuoteReactive.Active["ShowReactive"] = objPermissionInfo.CancelledQuotesIsWrite;
                        }
                        if (View.Id == "CRMQuotes_ListView_Expired")
                        {
                            QuoteReactive.Active["ShowReactive"] = objPermissionInfo.ExpiredQuotesIsWrite;
                        }

                        //if (objPermissionInfo.ReportDeliveryIsWrite)
                        //{
                        //    QuoteSubmit.Active["showSubmit"] = true;
                        //    QuoteRollback.Active["ShowRollback"] = true;
                        //    QuoteReview.Active["ShowReview"] = true;
                        //    QuoteReactive.Active["ShowReactive"] = true;
                        //}
                        //else
                        //{
                        //    QuoteSubmit.Active["showSubmit"] = false;
                        //    QuoteRollback.Active["ShowRollback"] = false;
                        //    QuoteReview.Active["ShowReview"] = false;
                        //    QuoteReactive.Active["ShowReactive"] = false;
                        //}
                    }
                }
                if (View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_ListView_pendingsubmission")
                {

                    //Frame.GetController<ModificationsController>().SaveAction.Executed += SaveAction_Executed;
                    //Frame.GetController<ModificationsController>().SaveAndCloseAction.Executed += SaveAction_Executed;
                    //Frame.GetController<ModificationsController>().SaveAndNewAction.Executed += SaveAction_Executed;                
                }
                if (View.Id == "AnalysisPricing_DetailView_QuotePopup")
                {
                    if (View.CurrentObject != null)
                    {
                        //AnalysisPricing objAna = (AnalysisPricing)View.CurrentObject;
                        //if (objAna.SampleMatries== null)
                        //{
                        //  IList< VisualMatrix> objVM = ObjectSpace.GetObjects<VisualMatrix>(CriteriaOperator.Parse(""));
                        //    objAna.SampleMatries  = objVM[0].VisualMatrixName;
                        //}
                    }
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                if (View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_DetailView_Submitted_History")
                {
                    ListPropertyEditor lvpricecode = ((DetailView)View).FindItem("AnalysisPricing") as ListPropertyEditor;
                    if (lvpricecode != null && lvpricecode.ListView == null)
                    {
                        lvpricecode.CreateControl();
                    }
                    ListPropertyEditor lvItemCharge = ((DetailView)View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                    if (lvItemCharge != null && lvItemCharge.ListView == null)
                    {
                        lvItemCharge.CreateControl();
                    }
                    Frame.GetController<ModificationsController>().SaveAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executing += SaveAction_Executing;
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                else if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView" || View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                if (View != null && (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView" || View.Id == "CRMQuotes_ItemChargePricing_ListView" || View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView"))
                {
                    ObjectSpace.Committed += ObjectSpace_Committed;
                }
                if (Application != null && Application.MainWindow != null && Application.MainWindow.View.Id == "CRMQuotes_DetailView")
                {
                    ObjectSpace.Committing += ObjectSpace_Committing;
                }
                else if (View != null && View.Id == "CRMQuotes_ListView_pendingsubmission")
                {
                    if (QuotesDateFilter != null && QuotesDateFilter.Items.Count > 0)
                    {
                        QuotesDateFilter.SelectedIndex = 1;
                        QuotesDateFilter.SelectedItemChanged += QuotesDateFilter_SelectedItemChanged;
                    }
                }
                if (Application != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "CRMProspects_DetailView" && Frame.GetType() == typeof(NestedFrame))
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    if (nestedFrame != null && nestedFrame.ViewItem.View != null && nestedFrame.ViewItem.View.Id == "CRMQuotes_DetailView")
                    {
                        quotesinfo.lvDetailedPrice = 0;
                        //quotesinfo.lvFinalPrice = 0;
                        //quotesinfo.lvdiscntpr = 0;
                        //quotesinfo.lvdiscntPrice = 0;
                        ((WebLayoutManager)((DetailView)nestedFrame.ViewItem.View).LayoutManager).ItemCreated += ChangeLayoutGroupCaptionViewController_ItemCreated;
                        //DashboardViewItem dvanalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisPrice") as DashboardViewItem;
                        //if (dvanalysisprice != null && dvanalysisprice.InnerView == null)
                        //{
                        //    dvanalysisprice.CreateControl();
                        //}
                        //if (dvanalysisprice != null && dvanalysisprice.InnerView != null)
                        //{
                        //    CRMQuotes objcrmquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                        //    ((ListView)dvanalysisprice.InnerView).CollectionSource.Criteria["filter"] = new InOperator("CRMQuotes.Oid", objcrmquotes.Oid);
                        //    ((ListView)dvanalysisprice.InnerView).Refresh();
                        //}
                    }
                }

                if (View.Id == "CRMQuotes_DetailView" && Application != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "CRMQuotes_DetailView")
                {
                    quotesinfo.lvDetailedPrice = 0;
                    //quotesinfo.lvFinalPrice = 0;
                    //quotesinfo.lvdiscntpr = 0;
                    //quotesinfo.lvdiscntPrice = 0;
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated += ChangeLayoutGroupCaptionViewController_ItemCreated;
                    //DashboardViewItem dvanalysisprice = ((DetailView)View).FindItem("AnalysisPrice") as DashboardViewItem;
                    //if (dvanalysisprice != null && dvanalysisprice.InnerView == null)
                    //{
                    //    dvanalysisprice.CreateControl();
                    //}
                    //if (dvanalysisprice != null && dvanalysisprice.InnerView != null)
                    //{
                    //    CRMQuotes objcrmquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                    //    ((ListView)dvanalysisprice.InnerView).CollectionSource.Criteria["filter"] = new InOperator("CRMQuotes.Oid", objcrmquotes.Oid);
                    //    ((ListView)dvanalysisprice.InnerView).Refresh();
                    //}
                }
                if (View.Id == "CRMProspects_DetailView")
                {
                    Frame.GetController<DialogController>().Accepting += QuoteViewController_Accepting;
                    //Frame.GetController<DialogController>().SaveOnAccept = false;
                }
                if (View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView")
                {
                    CRMQuotes crtquotes = null;
                    if (Application.MainWindow.View.Id == "CRMQuotes_DetailView")
                    {
                        crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                    }
                    else if (View.Id == "CRMQuotes_DetailView")
                    {
                        crtquotes = (CRMQuotes)View.CurrentObject;
                    }
                    else if (quotesinfo.popupcurtquote != null)
                    {
                        crtquotes = quotesinfo.popupcurtquote;
                    }
                    //CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                    if (crtquotes != null)
                    {
                        ((ListView)View).CollectionSource.Criteria.Clear();
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[CRMQuotes.Oid] = ?", crtquotes.Oid);
                        ((ListView)View).Refresh();
                    }
                }

                else if (View.Id == "AnalysisPricing_ListView_QuotesPopup")
                {
                    quotesinfo.lstInitialtempAnalysisPricingpopup = new List<AnalysisPricing>();
                    quotesinfo.IsAnalycialPricingpopupselectall = false;
                    if (quotesinfo.lstpricecodeempty == null)
                    {
                        quotesinfo.lstpricecodeempty = new List<string>();
                    }
                    List<TestMethod> lsttestmethod = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[GCRecord] is Null And [MethodName.GCRecord] Is Null And [MatrixName.GCRecord] Is Null")).ToList();
                    if (lsttestmethod != null && lsttestmethod.Count > 0)
                    {
                        //foreach (TestMethod objtstmed in lsttestmethod.ToList())
                        //{
                        //    if (objtstmed.MatrixName != null && objtstmed.MatrixName.Oid != null)
                        //    {
                        //        IList<VisualMatrix> lstVM = ObjectSpace.GetObjects<VisualMatrix>(CriteriaOperator.Parse("[GCRecord] is Null and [MatrixName.MatrixName] = ?", objtstmed.MatrixName.MatrixName));
                        //        if (lstVM != null)
                        //        {
                        //            foreach (VisualMatrix objViM in lstVM)
                        //            {
                        //                //AnalysisPricing newanalysisprice = ObjectSpace.CreateObject<AnalysisPricing>();
                        //                if (objtstmed.MatrixName != null && objtstmed.TestName != null && objtstmed.MethodName != null)
                        //                {
                        //                    //DefaultPricing objdefpricing = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ?", objtstmed.MatrixName.MatrixName, objtstmed.TestName, objtstmed.MethodName.MethodNumber));
                        //                    //ConstituentPricing objconstipricing = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And[Test.TestName] = ? And[Method.MethodNumber] = ? ", objtstmed.MatrixName.MatrixName, objtstmed.TestName, objtstmed.MethodName.MethodNumber));
                        //                    //if (objdefpricing != null)
                        //                    //{
                        //                    //    if (objdefpricing.Matrix != null && objdefpricing.Test != null && objdefpricing.Method != null && objdefpricing.Component != null)
                        //                    //    {
                        //                    //        TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.IsRegular] = 'True'", objdefpricing.Matrix.MatrixName, objdefpricing.Test.TestName, objdefpricing.Method.MethodNumber, objdefpricing.Component.Components)); //IsRegular
                        //                    //        if (objtstpricesur != null)
                        //                    //        {
                        //                    //            newanalysisprice.PriceCode = objtstpricesur.PriceCode;
                        //                    //            newanalysisprice.Priority = objtstpricesur.Priority;
                        //                    //        }
                        //                    //    }
                        //                    //    newanalysisprice.ChargeType = objdefpricing.ChargeType;
                        //                    //}
                        //                    //else
                        //                    //if (objconstipricing != null)
                        //                    //{
                        //                    //    if (objconstipricing.Matrix != null && objconstipricing.Test != null && objconstipricing.Method != null && objconstipricing.Component != null)
                        //                    //    {
                        //                    //        TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.IsRegular] = 'True'", objconstipricing.Matrix.MatrixName, objconstipricing.Test.TestName, objconstipricing.Method.MethodNumber, objconstipricing.Component.Components));
                        //                    //        if (objtstpricesur != null)
                        //                    //        {
                        //                    //            newanalysisprice.PriceCode = objtstpricesur.PriceCode;
                        //                    //            newanalysisprice.Priority = objtstpricesur.Priority;
                        //                    //        }
                        //                    //    }
                        //                    //    newanalysisprice.ChargeType = objconstipricing.ChargeType;
                        //                    //}
                        //                    //List<Component> lstCom = ObjectSpace.GetObjects<Component>(CriteriaOperator.Parse("[TestMethod] = ?", objtstmed.Oid)).ToList();
                        //                    //foreach(Component objCom in lstCom)
                        //                    //{
                        //                    //    TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ?  And [Priority.IsRegular] = 'True'", objtstmed.MatrixName.MatrixName, objtstmed.TestName, objtstmed.MethodName.MethodNumber, objCom.Components)); //IsRegular
                        //                    //    if (objtstpricesur != null)
                        //                    //    {
                        //                    //        newanalysisprice.PriceCode = objtstpricesur.PriceCode;
                        //                    //        newanalysisprice.Priority = objtstpricesur.Priority;
                        //                    //        newanalysisprice.ChargeType = objtstpricesur.ChargeType;
                        //                    //    }
                        //                    //}

                        //                }
                        //                else if (objtstmed.MatrixName != null && objtstmed.TestName != null && objtstmed.IsGroup == true)
                        //                {
                        //                    //DefaultPricing objdefpricing = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ?", objtstmed.MatrixName.MatrixName, objtstmed.TestName));
                        //                    //ConstituentPricing objconstipricing = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And[Test.TestName] = ?", objtstmed.MatrixName.MatrixName, objtstmed.TestName));
                        //                    //if (objdefpricing != null)
                        //                    //{
                        //                    //    if (objdefpricing.Matrix != null && objdefpricing.Test != null && objdefpricing.IsGroup == DefaultPricing.ISGroupType.Yes)
                        //                    //    {
                        //                    //        TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Priority.IsRegular] = 'True'", objdefpricing.Matrix.MatrixName, objdefpricing.Test.TestName)); //IsRegular
                        //                    //        if (objtstpricesur != null)
                        //                    //        {
                        //                    //            newanalysisprice.PriceCode = objtstpricesur.PriceCode;
                        //                    //            newanalysisprice.Priority = objtstpricesur.Priority;
                        //                    //        }
                        //                    //    }
                        //                    //    newanalysisprice.ChargeType = objdefpricing.ChargeType;
                        //                    //}
                        //                    //else
                        //                    //if (objconstipricing != null)
                        //                    //{
                        //                    //    if (objconstipricing.Matrix != null && objconstipricing.Test != null && objconstipricing.IsGroup == true)
                        //                    //    {
                        //                    //        TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ?", objconstipricing.Matrix.MatrixName, objconstipricing.Test.TestName));
                        //                    //        if (objtstpricesur != null)
                        //                    //        {
                        //                    //            newanalysisprice.PriceCode = objtstpricesur.PriceCode;
                        //                    //            newanalysisprice.Priority = objtstpricesur.Priority;
                        //                    //        }
                        //                    //    }
                        //                    //    newanalysisprice.ChargeType = objconstipricing.ChargeType;
                        //                    //}
                        //                    AnalysisPricing newanalysisprice = ObjectSpace.CreateObject<AnalysisPricing>();
                        //                    TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = True", objtstmed.MatrixName.MatrixName, objtstmed.TestName));
                        //                    if (objtstpricesur != null)
                        //                    {
                        //                        newanalysisprice.PriceCode = objtstpricesur.PriceCode;
                        //                        newanalysisprice.Priority = objtstpricesur.Priority;
                        //                        newanalysisprice.ChargeType = objtstpricesur.ChargeType;
                        //                    }
                        //                    newanalysisprice.SampleMatrix = ObjectSpace.GetObject<VisualMatrix>(objViM);
                        //                    newanalysisprice.Matrix = ObjectSpace.GetObject<Matrix>(objtstmed.MatrixName);
                        //                    newanalysisprice.Test = ObjectSpace.GetObject<TestMethod>(objtstmed);
                        //                    newanalysisprice.IsGroup = objtstmed.IsGroup;

                        //                    if (string.IsNullOrEmpty(newanalysisprice.PriceCode))
                        //                    {
                        //                        quotesinfo.lstpricecodeempty.Add(newanalysisprice.Oid.ToString());
                        //                    }
                        //                    ((ListView)View).CollectionSource.Add(newanalysisprice);
                        //                }
                        //                //newanalysisprice.Matrix = ObjectSpace.GetObject<Matrix>(objtstmed.MatrixName);
                        //                //newanalysisprice.Test = ObjectSpace.GetObject<TestMethod>(objtstmed);
                        //                //newanalysisprice.IsGroup = objtstmed.IsGroup;
                        //                //((ListView)View).CollectionSource.Add(newanalysisprice);
                        //                //newanalysisprice.Method = ObjectSpace.GetObject<Modules.BusinessObjects.Setting.Method>(objtstmed.MethodName);
                        //                //if (string.IsNullOrEmpty(newanalysisprice.PriceCode))
                        //                //{
                        //                //    quotesinfo.lstpricecodeempty.Add(newanalysisprice.Oid.ToString());
                        //                //}
                        //                if (objtstmed.IsGroup == false)
                        //                {
                        //                    List<Component> lstdefcomponent = ObjectSpace.GetObjects<Component>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtstmed.Oid)).ToList();
                        //                    Component defcompo = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                        //                    if (defcompo != null)
                        //                    {
                        //                        lstdefcomponent.Add(defcompo);
                        //                    }
                        //                    if (lstdefcomponent != null && lstdefcomponent.Count > 0)
                        //                    {
                        //                        foreach (Component defcomp in lstdefcomponent.ToList())
                        //                        {
                        //                            AnalysisPricing newanalysispricecomp = ObjectSpace.CreateObject<AnalysisPricing>();
                        //                            if (objtstmed.MatrixName != null && objtstmed.TestName != null && objtstmed.MethodName != null && objtstmed.IsGroup == false)
                        //                            {
                        //                                //DefaultPricing objdefpricing = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objtstmed.MatrixName.MatrixName, objtstmed.TestName, objtstmed.MethodName.MethodNumber, defcomp.Components));
                        //                                //ConstituentPricing objconstipricing = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And[Test.TestName] = ? And[Method.MethodNumber] = ? And [Component.Components] = ?", objtstmed.MatrixName.MatrixName, objtstmed.TestName, objtstmed.MethodName.MethodNumber, defcomp.Components));
                        //                                //if (objdefpricing != null)
                        //                                //{
                        //                                //    if (objdefpricing.Matrix != null && objdefpricing.Test != null && objdefpricing.Method != null && objdefpricing.Component != null)
                        //                                //    {
                        //                                //        TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And[Component.Components] = ? And [Priority.IsRegular] = 'True'", objdefpricing.Matrix.MatrixName, objdefpricing.Test.TestName, objdefpricing.Method.MethodNumber, objdefpricing.Component.Components)); //IsRegular
                        //                                //        if (objtstpricesur != null)
                        //                                //        {
                        //                                //            newanalysispricecomp.PriceCode = objtstpricesur.PriceCode;
                        //                                //            newanalysispricecomp.Priority = objtstpricesur.Priority;
                        //                                //        }
                        //                                //    }
                        //                                //    newanalysispricecomp.ChargeType = objdefpricing.ChargeType;
                        //                                //}
                        //                                //else
                        //                                //if (objconstipricing != null)
                        //                                //{
                        //                                //    if (objconstipricing.Matrix != null && objconstipricing.Test != null && objconstipricing.Method != null && objconstipricing.Component != null)
                        //                                //    {
                        //                                //        TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And [Priority.IsRegular] = 'True'", objconstipricing.Matrix.MatrixName, objconstipricing.Test.TestName, objconstipricing.Method.MethodNumber, objconstipricing.Component.Components));
                        //                                //        if (objtstpricesur != null)
                        //                                //        {
                        //                                //            newanalysispricecomp.PriceCode = objtstpricesur.PriceCode;
                        //                                //            newanalysispricecomp.Priority = objtstpricesur.Priority;
                        //                                //        }
                        //                                //    }
                        //                                //    newanalysispricecomp.ChargeType = objconstipricing.ChargeType;
                        //                                //}
                        //                                TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And[Component.Components] = ? And [Priority.IsRegular] = 'True'", objtstmed.MatrixName.MatrixName, objtstmed.TestName, objtstmed.MethodName.MethodNumber, defcomp.Components)); //IsRegular
                        //                                if (objtstpricesur != null)
                        //                                {
                        //                                    newanalysispricecomp.PriceCode = objtstpricesur.PriceCode;
                        //                                    newanalysispricecomp.Priority = objtstpricesur.Priority;
                        //                                    newanalysispricecomp.ChargeType = objtstpricesur.ChargeType;
                        //                                }
                        //                            }
                        //                            else if (objtstmed.MatrixName != null && objtstmed.TestName != null && objtstmed.IsGroup == true)
                        //                            {
                        //                                TestPriceSurcharge objtstpricesur = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = True", objtstmed.Matrix.MatrixName, objtstmed.Test.TestName));
                        //                                if (objtstpricesur != null)
                        //                                {
                        //                                    newanalysispricecomp.PriceCode = objtstpricesur.PriceCode;
                        //                                    newanalysispricecomp.Priority = objtstpricesur.Priority;
                        //                                    newanalysispricecomp.ChargeType = objtstpricesur.ChargeType;
                        //                                }
                        //                            }
                        //                            newanalysispricecomp.SampleMatrix = ObjectSpace.GetObject<VisualMatrix>(objViM);
                        //                            newanalysispricecomp.Matrix = ObjectSpace.GetObject<Matrix>(objtstmed.MatrixName);
                        //                            newanalysispricecomp.Test = ObjectSpace.GetObject<TestMethod>(objtstmed);
                        //                            newanalysispricecomp.IsGroup = objtstmed.IsGroup;
                        //                            newanalysispricecomp.Method = ObjectSpace.GetObject<Modules.BusinessObjects.Setting.Method>(objtstmed.MethodName);
                        //                            if (string.IsNullOrEmpty(newanalysispricecomp.PriceCode))
                        //                            {
                        //                                quotesinfo.lstpricecodeempty.Add(newanalysispricecomp.Oid.ToString());
                        //                            }
                        //                            newanalysispricecomp.Component = ObjectSpace.GetObject(defcomp);
                        //                            ((ListView)View).CollectionSource.Add(newanalysispricecomp);
                        //                        }
                        //                    }
                        //                    //else if (lstdefcomponent != null)
                        //                    //{
                        //                    //    Component defcompo = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                        //                    //    if (defcompo != null)
                        //                    //    {
                        //                    //        newanalysisprice.Component = ObjectSpace.GetObject(defcompo);
                        //                    //        ((ListView)View).CollectionSource.Add(newanalysisprice);
                        //                    //    }
                        //                    //}
                        //                }
                        //                //Component defcompo = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                        //                //if (defcompo != null)
                        //                //{
                        //                //    newanalysisprice.Component = ObjectSpace.GetObject(defcompo);
                        //                //    ((ListView)View).CollectionSource.Add(newanalysisprice);
                        //                //}
                        //            }
                        //        }
                        //    }
                        //}
                        //((ListView)View).Refresh();
                        if (quotesinfo.lsttempAnalysisPricingpopup == null)
                        {
                            quotesinfo.lsttempAnalysisPricingpopup = new List<AnalysisPricing>();
                        }
                        //List<string> lsttestpricecode = new List<string>();
                        dtPriceCode = new DataTable();
                        dtPriceCode.Columns.Add("SampleMatrix");
                        dtPriceCode.Columns.Add("PriceCode");
                        ListPropertyEditor lvanalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                        if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                        {
                            foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                            {
                                if (objanapricing.PriceCode != null)
                                {
                                    //string strpricecode = string.Empty;
                                    //string[] strpricecodearr = objanapricing.PriceCode.Split('-');
                                    //if (strpricecodearr.Length > 2)
                                    //{
                                    //    foreach (string strprcode in strpricecodearr.ToList())
                                    //    {
                                    //        if (string.IsNullOrEmpty(strpricecode) && strprcode.Length > 1)
                                    //        {
                                    //            strpricecode = strprcode;
                                    //        }
                                    //        else if (!string.IsNullOrEmpty(strpricecode) && strprcode.Length > 1)
                                    //        {
                                    //            strpricecode = strpricecode + "-" + strprcode;
                                    //        }
                                    //        else if (!string.IsNullOrEmpty(strpricecode) && strprcode.Length == 1)
                                    //        {
                                    //            strpricecode = strpricecode + "-1";
                                    //        }
                                    //    }
                                    //}

                                    DataRow drNew = dtPriceCode.NewRow();
                                    drNew["SampleMatrix"] = objanapricing.SampleMatrix.VisualMatrixName;
                                    drNew["PriceCode"] = objanapricing.PriceCode;
                                    dtPriceCode.Rows.Add(drNew);
                                    //lsttestpricecode.Add(objanapricing.PriceCode);

                                }

                            }
                            if (dtPriceCode != null && dtPriceCode.Rows.Count > 0)
                            {
                                foreach (DataRow drpricecode in dtPriceCode.Rows)
                                {
                                    foreach (AnalysisPricing objanalysisprice in ((ListView)View).CollectionSource.List.Cast<AnalysisPricing>().Where(i => i.PriceCode == drpricecode["PriceCode"].ToString() && i.SampleMatrix == drpricecode["SampleMatrix"]))
                                    {
                                        quotesinfo.lsttempAnalysisPricingpopup.Add(objanalysisprice);
                                        quotesinfo.lstInitialtempAnalysisPricingpopup.Add(objanalysisprice);
                                    }
                                }
                            }
                        }
                    }
                    View.ControlsCreated += View_ControlsCreated;
                }
                if (View.Id == "ItemChargePricing_ListView_Quotes_Popup")
                {
                    quotesinfo.lstinitialtempItemPricingpopup = new List<ItemChargePricing>();
                    quotesinfo.IsItemchargePricingpopupselectall = false;
                    if (quotesinfo.lsttempItemPricingpopup == null)
                    {
                        quotesinfo.lsttempItemPricingpopup = new List<ItemChargePricing>();
                    }
                    //DashboardViewItem dvitemprice = ((DetailView)Application.MainWindow.View).FindItem("DVItemChargePrice") as DashboardViewItem;
                    //if (dvitemprice != null && dvitemprice.InnerView != null)
                    //{
                    //    //foreach (QuotesItemChargePrice objitempricing in ((ListView)dvitemprice.InnerView).CollectionSource.List.Cast<QuotesItemChargePrice>().Where(i=>i.ItemPrice!=null))
                    //    //{
                    //    //    quotesinfo.lsttempItemPricingpopup.Add(objitempricing.ItemPrice);
                    //    //}
                    //    quotesinfo.lsttempItemPricingpopup = ((ListView)dvitemprice.InnerView).CollectionSource.List.Cast<QuotesItemChargePrice>().Where(i => i.ItemPrice != null).Select(i=>i.ItemPrice).ToList();
                    //}
                    ListPropertyEditor lvitemprice = ((DetailView)Application.MainWindow.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                    if (lvitemprice != null && lvitemprice.ListView != null)
                    {
                        //foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                        //{
                        //    if (objitempricing.ItemPrice != null)
                        //    {
                        //        quotesinfo.lsttempItemPricingpopup.Add(objitempricing.ItemPrice);
                        //    }
                        //}
                        quotesinfo.lstinitialtempItemPricingpopup = new List<ItemChargePricing>();
                        quotesinfo.lstinitialtempItemPricingpopup = ((ListView)lvitemprice.ListView).CollectionSource.List.Cast<QuotesItemChargePrice>().Where(i => i.ItemPrice != null).Select(i => i.ItemPrice).ToList();
                        quotesinfo.lsttempItemPricingpopup = ((ListView)lvitemprice.ListView).CollectionSource.List.Cast<QuotesItemChargePrice>().Where(i => i.ItemPrice != null).Select(i => i.ItemPrice).ToList();
                    }
                    View.ControlsCreated += View_ControlsCreated;
                }
                if (View.Id == "CRMQuotes_ListView_pendingsubmission")
                {
                    ((ListView)View).CollectionSource.Criteria.Clear();
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'PendingSubmission' And [ExpirationDate] >= ?", DateTime.Today);
                }
                else if (View.Id == "CRMQuotes_ListView_PendingReview")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[ExpirationDate] >= ?", DateTime.Today);
                }
                else if (View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                }
                else if (View.Id == "Testparameter_LookupListView_Quotes")
                {
                    if (quotesinfo.lsttempparamsoid == null)
                    {
                        quotesinfo.lsttempparamsoid = new List<Guid>();
                    }
                }
                else if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    if (quotesinfo.lsttempparamsoid == null)
                    {
                        quotesinfo.lsttempparamsoid = new List<Guid>();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }
        //private void Grid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        //{
        //    try
        //    {
        //        if (View.Id == "AnalysisPricing_ListView_QuotesPopup")
        //        {
        //            ASPxGridView gridView = sender as ASPxGridView;
        //            if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
        //            {
        //                var curOid = gridView.GetRowValues(e.VisibleIndex, "Oid");
        //                if (curOid != null)
        //                {
        //                    AnalysisPricing analysisPricing = ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(curOid.ToString())));
        //                    if (analysisPricing != null && quotesinfo.lstInitialtempAnalysisPricingpopup != null && quotesinfo.lstInitialtempAnalysisPricingpopup.Count > 0)
        //                    {
        //                        if (quotesinfo.lstInitialtempAnalysisPricingpopup.Contains(analysisPricing))
        //                        {
        //                            e.Visible = false;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else if(View.Id == "ItemChargePricing_ListView_Quotes_Popup")
        //        {
        //            ASPxGridView gridView = sender as ASPxGridView;
        //            if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
        //            {
        //                var curOid = gridView.GetRowValues(e.VisibleIndex, "Oid");
        //                if (curOid != null)
        //                {
        //                    ItemChargePricing analysisPricing = ObjectSpace.FindObject<ItemChargePricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(curOid.ToString())));
        //                    if (analysisPricing != null && quotesinfo.lstinitialtempItemPricingpopup != null && quotesinfo.lstinitialtempItemPricingpopup.Count > 0)
        //                    {
        //                        if (quotesinfo.lstinitialtempItemPricingpopup.Contains(analysisPricing))
        //                        {
        //                            e.Visible = false;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
        private void DeleteAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {

                if (View is ListView && View.SelectedObjects.Count > 0)
                {
                    foreach (CRMQuotes objquotes in View.SelectedObjects.Cast<CRMQuotes>().ToList())
                    {
                        IList<Samplecheckin> lstSC = ObjectSpace.GetObjects<Samplecheckin>(CriteriaOperator.Parse("[QuoteID.Oid] = ?", objquotes.Oid));
                        if (lstSC != null && lstSC.Count > 0)
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteQuote"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;

                        }
                        else
                        {
                            ObjectSpace.Delete(objquotes);
                            ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                        }
                    }
                }

                if (View is DetailView && View.SelectedObjects.Count > 0)
                {
                    foreach (CRMQuotes objquotes in View.SelectedObjects.Cast<CRMQuotes>().ToList())
                    {
                        IList<Samplecheckin> lstSC = ObjectSpace.GetObjects<Samplecheckin>(CriteriaOperator.Parse("[QuoteID.Oid] = ?", objquotes.Oid));
                        if (lstSC != null && lstSC.Count > 0)
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CannotDeleteQuote"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;

                        }
                        else
                        {
                            ObjectSpace.Delete(objquotes);
                            ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

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

        private void DeleteAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                //QuotesNavgCount();
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
                if (Application != null && Application.MainWindow != null && Application.MainWindow.View.Id == "CRMQuotes_DetailView")
                {
                    CRMQuotes objcrmquot = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                    if (objcrmquot != null && objcrmquot.Title != null && ((objcrmquot.IsProspect == true && objcrmquot.ProspectClient != null) || (objcrmquot.IsProspect == false && objcrmquot.Client != null)))
                    {
                        if (objcrmquot != null && objcrmquot.Cancel == false)
                        {
                            objcrmquot.CancelReason = null;
                        }
                    }
                    else if ((View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_DetailView_Submitted_History") && objcrmquot != null && objcrmquot.Title == null)
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Title should not empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    else if ((View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_DetailView_Submitted_History") && objcrmquot != null && objcrmquot.IsProspect == true && objcrmquot.ProspectClient == null)
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Client should not empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    else if ((View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_DetailView_Submitted_History") && objcrmquot != null && ((objcrmquot.IsProspect == true && objcrmquot.ProspectClient == null) || (objcrmquot.IsProspect == false && objcrmquot.Client == null)))
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Client should not empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                quotesinfo.IsQuotesSave = false;
                if (View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_ListView_pendingsubmission")
                {
                    CRMQuotes objcrmquot = (CRMQuotes)View.CurrentObject;
                    DashboardViewItem dvanalysisprice = ((DetailView)View).FindItem("AnalysisPrice") as DashboardViewItem;
                    if (dvanalysisprice != null && dvanalysisprice.InnerView == null)
                    {
                        dvanalysisprice.CreateControl();
                    }
                    if (dvanalysisprice != null && dvanalysisprice.InnerView != null)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        foreach (AnalysisPricing objanalyprice in ((ListView)dvanalysisprice.InnerView).CollectionSource.List)
                        {
                            AnalysisPricing objchkanalyprice = os.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[PriceCode] = ? And [CRMQuotes.Oid] =?", objanalyprice.PriceCode, objcrmquot.Oid));
                            if (objchkanalyprice == null)
                            {
                                AnalysisPricing newanalyprice = os.CreateObject<AnalysisPricing>();
                                newanalyprice.TestDescription = objanalyprice.TestDescription;
                                newanalyprice.PriceCode = objanalyprice.PriceCode;
                                newanalyprice.Matrix = os.GetObject(objanalyprice.Matrix);
                                newanalyprice.Test = os.GetObject(objanalyprice.Test);
                                newanalyprice.Method = os.GetObject(objanalyprice.Method);
                                newanalyprice.Component = os.GetObject(objanalyprice.Component);
                                newanalyprice.ChargeType = objanalyprice.ChargeType;
                                newanalyprice.TAT = os.GetObject(objanalyprice.TAT);
                                newanalyprice.Priority = os.GetObject(objanalyprice.Priority);
                                newanalyprice.NPUnitPrice = objanalyprice.NPUnitPrice;
                                newanalyprice.UnitPrice = objanalyprice.UnitPrice;
                                newanalyprice.NPTotalPrice = objanalyprice.NPTotalPrice;
                                newanalyprice.TotalTierPrice = objanalyprice.TotalTierPrice;
                                newanalyprice.NPSurcharge = objanalyprice.NPSurcharge;
                                newanalyprice.FinalAmount = objanalyprice.FinalAmount;
                                newanalyprice.Qty = objanalyprice.Qty;
                                newanalyprice.IsGroup = objanalyprice.IsGroup;
                                newanalyprice.Parameter = objanalyprice.Parameter;
                                newanalyprice.Discount = objanalyprice.Discount;
                                newanalyprice.Status = objanalyprice.Status;
                                newanalyprice.Remark = objanalyprice.Remark;
                                newanalyprice.CRMQuotes = os.GetObject(objcrmquot);

                            }
                            else
                            {
                                objchkanalyprice.TestDescription = objanalyprice.TestDescription;
                                objchkanalyprice.PriceCode = objanalyprice.PriceCode;
                                objchkanalyprice.Matrix = os.GetObject(objanalyprice.Matrix);
                                objchkanalyprice.Test = os.GetObject(objanalyprice.Test);
                                objchkanalyprice.Method = os.GetObject(objanalyprice.Method);
                                objchkanalyprice.Component = os.GetObject(objanalyprice.Component);
                                objchkanalyprice.ChargeType = objanalyprice.ChargeType;
                                objchkanalyprice.TAT = os.GetObject(objanalyprice.TAT);
                                objchkanalyprice.Priority = os.GetObject(objanalyprice.Priority);
                                objchkanalyprice.NPUnitPrice = objanalyprice.NPUnitPrice;
                                objchkanalyprice.UnitPrice = objanalyprice.UnitPrice;
                                objchkanalyprice.NPTotalPrice = objanalyprice.NPTotalPrice;
                                objchkanalyprice.TotalTierPrice = objanalyprice.TotalTierPrice;
                                objchkanalyprice.NPSurcharge = objanalyprice.NPSurcharge;
                                objchkanalyprice.FinalAmount = objanalyprice.FinalAmount;
                                objchkanalyprice.Qty = objanalyprice.Qty;
                                objchkanalyprice.IsGroup = objanalyprice.IsGroup;
                                objchkanalyprice.Parameter = objanalyprice.Parameter;
                                objchkanalyprice.Discount = objanalyprice.Discount;
                                objchkanalyprice.Status = objanalyprice.Status;
                                objchkanalyprice.Remark = objanalyprice.Remark;
                                objchkanalyprice.CRMQuotes = os.GetObject(objcrmquot);
                            }
                            os.CommitChanges();
                        }
                        ASPxGridListEditor gridlistedit = ((ListView)dvanalysisprice.InnerView).Editor as ASPxGridListEditor;
                        if (gridlistedit != null && gridlistedit.Grid != null)
                        {
                            gridlistedit.Grid.UpdateEdit();
                            dvanalysisprice.InnerView.ObjectSpace.CommitChanges();
                        }
                    }
                    DashboardViewItem dvitemprice = ((DetailView)View).FindItem("DVItemChargePrice") as DashboardViewItem;
                    if (dvitemprice != null && dvitemprice.InnerView == null)
                    {
                        dvitemprice.CreateControl();
                    }
                    if (dvitemprice != null && dvitemprice.InnerView != null)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        foreach (QuotesItemChargePrice objitemprice in ((ListView)dvitemprice.InnerView).CollectionSource.List)
                        {
                            QuotesItemChargePrice quoteitempricechk = os.FindObject<QuotesItemChargePrice>(CriteriaOperator.Parse("[ItemPrice.Oid] = ? And [CRMQuotes.Oid] = ?", objitemprice.ItemPrice.Oid, objcrmquot.Oid));
                            if (quoteitempricechk == null)
                            {
                                QuotesItemChargePrice newitemprice = os.CreateObject<QuotesItemChargePrice>();
                                newitemprice.ItemPrice = os.GetObject(objitemprice.ItemPrice);
                                newitemprice.Amount = objitemprice.Amount;
                                newitemprice.Qty = objitemprice.Qty;
                                newitemprice.UnitPrice = objitemprice.UnitPrice;
                                newitemprice.NpUnitPrice = objitemprice.NpUnitPrice;
                                newitemprice.Discount = objitemprice.Discount;
                                newitemprice.FinalAmount = objitemprice.FinalAmount;
                                newitemprice.CRMQuotes = os.GetObject(objcrmquot);
                                newitemprice.Remark = objitemprice.Remark;
                            }
                            else
                            {
                                quoteitempricechk.ItemPrice = os.GetObject(objitemprice.ItemPrice);
                                quoteitempricechk.Amount = objitemprice.Amount;
                                quoteitempricechk.Qty = objitemprice.Qty;
                                quoteitempricechk.UnitPrice = objitemprice.UnitPrice;
                                quoteitempricechk.NpUnitPrice = objitemprice.NpUnitPrice;
                                quoteitempricechk.Discount = objitemprice.Discount;
                                quoteitempricechk.FinalAmount = objitemprice.FinalAmount;
                                quoteitempricechk.CRMQuotes = os.GetObject(objcrmquot);
                                quoteitempricechk.Remark = objitemprice.Remark;

                            }

                            os.CommitChanges();
                        }
                        ASPxGridListEditor gridlistedit = ((ListView)dvitemprice.InnerView).Editor as ASPxGridListEditor;
                        if (gridlistedit != null && gridlistedit.Grid != null)
                        {
                            gridlistedit.Grid.UpdateEdit();
                        }
                    }
                    //QuotesNavgCount();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void QuotesNavgCount()
        {
            try
            {
                ShowNavigationController = Frame.GetController<ShowNavigationItemController>();

                foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items.Where(i => i.Id == "Crm"))
                {
                    if (parent.Id == "Crm")
                    {
                        foreach (ChoiceActionItem child in parent.Items)
                        {
                            if (child.Id == "Accounts")
                            {
                                foreach (ChoiceActionItem subchild in child.Items)
                                {
                                    if (subchild.Id == "Quotes")
                                    {
                                        foreach (ChoiceActionItem subchilditem in subchild.Items)
                                        {
                                            if (subchilditem.Id == "OpenQuotes")
                                            {
                                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                var count = objectSpace.GetObjectsCount(typeof(CRMQuotes), CriteriaOperator.Parse("([Status] = 0) And [Cancel] = False And [ExpirationDate] >= ?", DateTime.Today));
                                                var cap = subchilditem.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                if (count > 0)
                                                {
                                                    subchilditem.Caption = cap[0] + " (" + count + ")";
                                                }
                                                else
                                                {
                                                    subchilditem.Caption = cap[0];
                                                }
                                            }
                                            if (subchilditem.Id == "QuotesReview")
                                            {
                                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                var count = objectSpace.GetObjectsCount(typeof(CRMQuotes), CriteriaOperator.Parse("([Status] = 1) And [Cancel] = False And [ExpirationDate] >= ?", DateTime.Today));
                                                var cap = subchilditem.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                if (count > 0)
                                                {
                                                    subchilditem.Caption = cap[0] + " (" + count + ")";
                                                }
                                                else
                                                {
                                                    subchilditem.Caption = cap[0];
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //if (parent.Id == "Accounts")
                    //{
                    //    ChoiceActionItem quotes = parent.Items.FirstOrDefault(i => i.Id == "Quotes");

                    //    foreach (ChoiceActionItem subchild in quotes.Items)
                    //    {
                    //        if (subchild.Id == "OpenQuotes")
                    //        {
                    //            IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //            var count = objectSpace.GetObjectsCount(typeof(CRMQuotes), CriteriaOperator.Parse("([Status] = 0) And [Cancel] = False And [ExpirationDate] >= ?", DateTime.Today));
                    //            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //            if (count > 0)
                    //            {
                    //                subchild.Caption = cap[0] + " (" + count + ")";
                    //            }
                    //            else
                    //            {
                    //                subchild.Caption = cap[0];
                    //            }
                    //        }
                    //        if (subchild.Id == "QuotesReview")
                    //        {
                    //            IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //            var count = objectSpace.GetObjectsCount(typeof(CRMQuotes), CriteriaOperator.Parse("([Status] = 1) And [Cancel] = False And [ExpirationDate] >= ?", DateTime.Today));
                    //            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //            if (count > 0)
                    //            {
                    //                subchild.Caption = cap[0] + " (" + count + ")";
                    //            }
                    //            else
                    //            {
                    //                subchild.Caption = cap[0];
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

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            try
            {
                if (View != null && (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView" || View.Id == "CRMQuotes_ItemChargePricing_ListView" || View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView"))
                {
                    WebWindow.CurrentRequestWindow.RegisterClientScript("QuotesDetailedAmount", "RaiseXafCallback(globalCallbackControl, 'Quotes', 'QuotesListAmount', false);");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ChangeLayoutGroupCaptionViewController_ItemCreated(object sender, ItemCreatedEventArgs e)
        {
            try
            {
                if (e.ViewItem is PropertyEditor && ((PropertyEditor)e.ViewItem).PropertyName == "Client")
                {
                    e.TemplateContainer.Load += (s, args) =>
                    {
                        if (e.TemplateContainer.CaptionControl != null)
                        {
                            e.TemplateContainer.CaptionControl.ForeColor = Color.Red;
                        }
                    };
                }
                else if (e.ViewItem is PropertyEditor && ((PropertyEditor)e.ViewItem).PropertyName == "ProspectClient")
                {
                    e.TemplateContainer.Load += (s, args) =>
                    {
                        if (e.TemplateContainer.CaptionControl != null)
                        {
                            e.TemplateContainer.CaptionControl.ForeColor = Color.Red;
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void QuoteViewController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                CRMProspects objPC = (CRMProspects)View.CurrentObject;
                if (objPC != null && objPC.Name != null)
                {
                    Customer objC = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[CustomerName] = ?", objPC.Name));
                    if (objC != null)
                    {
                        Application.ShowViewStrategy.ShowMessage("This Client Already Stored", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void QuotesDateFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "CRMQuotes_ListView_pendingsubmission")
                {
                    string strSelectedItem = ((DevExpress.ExpressApp.Actions.SingleChoiceAction)sender).SelectedItem.Id.ToString();
                    if (strSelectedItem == "1M")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(QuotedDate, Now()) <= 1");
                    }
                    else if (strSelectedItem == "3M")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(QuotedDate, Now()) <= 3");
                    }
                    else if (strSelectedItem == "6M")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(QuotedDate, Now()) <= 6");
                    }
                    else if (strSelectedItem == "1Y")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(QuotedDate, Now()) <= 1");
                    }
                    else if (strSelectedItem == "2Y")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(QuotedDate, Now()) <= 2");
                    }
                    else if (strSelectedItem == "5Y")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(QuotedDate, Now()) <= 5");
                    }
                    else if (strSelectedItem == "All")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[Status] = 0");
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.Id == "CRMQuotes_DetailView")
                {
                    CRMQuotes objcrmquot = (CRMQuotes)View.CurrentObject;
                    if (objcrmquot != null && objcrmquot.Cancel == true && string.IsNullOrEmpty(objcrmquot.CancelReason))
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Quotereason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    else
                    {
                        if (objcrmquot != null && objcrmquot.Title != null && (objcrmquot.ProspectClient != null || objcrmquot.Client != null))
                        {
                            if (objcrmquot != null && objcrmquot.Cancel == false)
                            {
                                objcrmquot.CancelReason = null;
                            }
                            ListPropertyEditor lvItemCharge = ((DetailView)View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                            if (lvItemCharge != null && lvItemCharge.ListView != null)
                            {
                                lvItemCharge.ListView.ObjectSpace.CommitChanges();
                            }
                        }
                        else if (objcrmquot != null && objcrmquot.Title == null)
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage("Title should not empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                        else if (objcrmquot != null && (objcrmquot.ProspectClient == null || objcrmquot.Client == null))
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage("Client should not empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
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

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View == null)
                {
                    return;
                }
                if ((View.Id == "CRMQuotes_DetailView_Submitted_History" || View.Id == "CRMQuotes_DetailView") && View.CurrentObject == e.Object)
                {
                    CRMQuotes objquotes = (CRMQuotes)e.Object;
                    if (e.PropertyName == "Client")
                    {
                        objquotes.ProjectID = null;
                        objquotes.PrimaryContact = null;
                        objquotes.EmailID = null;
                        objquotes.BillCity = null;
                        objquotes.BillCountry = null;
                        objquotes.BillState = null;
                        objquotes.BillStreet1 = null;
                        objquotes.BillStreet2 = null;
                        objquotes.BillZipCode = null;
                        objquotes.CellPhone = null;
                        objquotes.OfficePhone = null;

                        if (View.Id == "CRMQuotes_DetailView")
                        {

                            ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("ProjectID") as ASPxGridLookupPropertyEditor;
                            if (e.NewValue != null)
                            {
                                propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("[customername.Oid] = ? ", objquotes.Client.Oid);
                            }
                            else
                            {
                                propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                            }
                            propertyEditor.Refresh();
                            propertyEditor.RefreshDataSource();
                        }

                    }
                    if (e.PropertyName == "ProspectClient")
                    {
                        objquotes.ProjectID = null;
                        objquotes.PrimaryContact = null;
                        objquotes.EmailID = null;
                        objquotes.BillCity = null;
                        objquotes.BillCountry = null;
                        objquotes.BillState = null;
                        objquotes.BillStreet1 = null;
                        objquotes.BillStreet2 = null;
                        objquotes.BillZipCode = null;
                        objquotes.CellPhone = null;
                        objquotes.OfficePhone = null;
                    }
                    //if(e.PropertyName == "IsProspect")
                    //{
                    //    objquotes.Client = null;
                    //}
                    if (objquotes != null)
                    {
                        if (e.PropertyName == "SameAddress")
                        {
                            if (objquotes.SameAddress == true)
                            {
                                objquotes.ShipStreet1 = objquotes.BillStreet1;
                                objquotes.ShipStreet2 = objquotes.BillStreet2;
                                objquotes.ShipCity = objquotes.BillCity;
                                objquotes.ShipZipCode = objquotes.BillZipCode;
                                objquotes.ShipState = objquotes.BillState;
                                objquotes.ShipCountry = objquotes.BillCountry;
                            }
                            else
                            {
                                objquotes.ShipStreet1 = null;
                                objquotes.ShipStreet2 = null;
                                objquotes.ShipCity = null;
                                objquotes.ShipZipCode = null;
                                objquotes.ShipState = null;
                                objquotes.ShipCountry = null;
                            }
                        }
                        else
                        if (e.PropertyName == "Cancel")
                        {
                            if (objquotes.Cancel == true)
                            {
                                IObjectSpace os = Application.CreateObjectSpace(typeof(DummyClass));
                                DummyClass objcrtdummy = os.CreateObject<DummyClass>();
                                DetailView dv = Application.CreateDetailView(os, "DummyClass_DetailView_Reasons", false, objcrtdummy);
                                dv.ViewEditMode = ViewEditMode.Edit;
                                dv.Caption = "Quote Cancel Reason";
                                ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                                showViewParameters.CreatedView = dv;
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.SaveOnAccept = false;
                                dc.CloseOnCurrentObjectProcessing = false;
                                dc.Accepting += Dcreason_Accepting;
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            }
                            else
                            {
                                objquotes.Status = CRMQuotes.QuoteStatus.PendingSubmission;
                            }
                        }
                        else
                        if (e.PropertyName == "ExpirationDate")
                        {
                            if (objquotes.ExpirationDate.Date < DateTime.Now.Date)
                            {
                                objquotes.ExpirationDate = DateTime.Now;
                                objquotes.Status = CRMQuotes.QuoteStatus.PendingSubmission;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ExpirationDatelessthancurrentdateissue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        else
                        if (e.PropertyName == "IsProspect")
                        {
                            objquotes.Client = null;
                            objquotes.ProspectClient = null;
                            objquotes.ProjectID = null;
                            objquotes.PrimaryContact = null;
                            objquotes.EmailID = null;
                            objquotes.BillCity = null;
                            objquotes.BillCountry = null;
                            objquotes.BillState = null;
                            objquotes.BillStreet1 = null;
                            objquotes.BillStreet2 = null;
                            objquotes.BillZipCode = null;
                            objquotes.CellPhone = null;
                            objquotes.OfficePhone = null;

                            ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("Client") as ASPxGridLookupPropertyEditor;
                            ASPxGridLookupPropertyEditor propertyEditor1 = ((DetailView)View).FindItem("ProspectClient") as ASPxGridLookupPropertyEditor;

                            if (objquotes.IsProspect == true)
                            {
                                propertyEditor1.CollectionSource.Criteria["ProspectClient"] = CriteriaOperator.Parse("[Status] = 'None'");
                                propertyEditor1.Refresh();
                                propertyEditor1.RefreshDataSource();
                            }
                            else
                            {
                                propertyEditor1.CollectionSource.Criteria["ProspectClient"] = CriteriaOperator.Parse("1=2");
                            }


                            //if (objsamplecheckin.ClientName != null)
                            //{
                            //    propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("[customername.Oid] = ? ", objsamplecheckin.ClientName.Oid);
                            //}
                            //else
                            //{
                            //    propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                            //}
                            //propertyEditor.Refresh();
                            //propertyEditor.RefreshDataSource();



                            //List<ProspectClient> lstprosclient = ObjectSpace.GetObjects<ProspectClient>(CriteriaOperator.Parse("[GCRecord] Is Null")).ToList();
                            //ASPxLookupPropertyEditor dropdownEditor = (ASPxLookupPropertyEditor)((DetailView)View).FindItem("ProspectClient");
                            //if(dropdownEditor.DropDownEdit.DropDown.Items.Count > 0)
                            //{
                            //    dropdownEditor.DropDownEdit.DropDown.Items.Clear();
                            //    foreach (ProspectClient prosclient in lstprosclient.ToList())
                            //    {
                            //        dropdownEditor.DropDownEdit.DropDown.Items.Add(prosclient.CustomerName);
                            //    }
                            //    dropdownEditor.RefreshDataSource();
                            //}
                            //foreach (ViewItem item in ((DetailView)View).Items)
                            //{
                            //    if (item.GetType() == typeof(ASPxLookupPropertyEditor) && item.Id == "ProspectClient")
                            //    {
                            //        ASPxLookupPropertyEditor dropdownEditor = (ASPxLookupPropertyEditor)((DetailView)View).FindItem("ProspectClient");
                            //        if (((ASPxComboBox)dropdownEditor.Editor).Items.Count > 0)
                            //        {
                            //            ((ASPxComboBox)dropdownEditor.Editor).Items.Clear();
                            //        }
                            //        foreach (ProspectClient prosclient in lstprosclient.ToList())
                            //        {
                            //            ((ASPxComboBox)dropdownEditor.Editor).Items.Add(prosclient.CustomerName);
                            //        }
                            //    }
                            //}
                        }
                        else if (e.PropertyName == "Discount" && quotesinfo.IsTabDiscountChanged == false)
                        {
                            if (objquotes.AnalysisPricing.Count > 0 || objquotes.QuotesItemChargePrice.Count > 0)
                            {
                                if (objquotes.DetailedAmount != 0)
                                {
                                    objquotes.DiscountAmount = objquotes.Discount * objquotes.DetailedAmount / 100;
                                    if (objquotes.QuotesItemChargePrice != null)
                                    {
                                        ListPropertyEditor lvitemprice = ((DetailView)View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                        foreach (QuotesItemChargePrice objItemCharge in objquotes.QuotesItemChargePrice.ToList())
                                        {
                                            objItemCharge.Discount = objquotes.Discount;
                                            objItemCharge.FinalAmount = objItemCharge.Amount * objItemCharge.Discount / 100;
                                            objItemCharge.FinalAmount = (objItemCharge.Amount - objItemCharge.FinalAmount) * objItemCharge.Qty;
                                        }
                                        ((ListView)lvitemprice.ListView).Refresh();
                                    }
                                    if (objquotes.AnalysisPricing != null)
                                    {
                                        ListPropertyEditor lvanalysisprice = ((DetailView)View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                        foreach (AnalysisPricing objAnalysis in objquotes.AnalysisPricing.ToList())
                                        {
                                            objAnalysis.Discount = objquotes.Discount;
                                            objAnalysis.DiscountAmount = objAnalysis.Discount * objAnalysis.TotalTierPrice / 100;
                                            objAnalysis.FinalAmount = (objAnalysis.TotalTierPrice - objAnalysis.DiscountAmount) * objAnalysis.Qty;
                                        }
                                        ((ListView)lvanalysisprice.ListView).Refresh();
                                    }
                                    objquotes.TotalAmount = objquotes.DetailedAmount - objquotes.DiscountAmount;
                                    objquotes.QuotedAmount = objquotes.DetailedAmount - objquotes.DiscountAmount;
                                }
                                else
                                {
                                    objquotes.Discount = 0;
                                    objquotes.DiscountAmount = 0;
                                }
                            }
                            else
                            {
                                objquotes.Discount = 0;
                                objquotes.DiscountAmount = 0;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "AddAnalysisPriceOrItemCharge"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        else if (e.PropertyName == "DiscountAmount" && quotesinfo.IsTabDiscountChanged == false)
                        {
                            if (objquotes.AnalysisPricing.Count > 0 || objquotes.QuotesItemChargePrice.Count > 0)
                            {
                                if (objquotes.DetailedAmount != 0)
                                {
                                    objquotes.Discount = objquotes.DiscountAmount / objquotes.DetailedAmount * 100;
                                    if (objquotes.QuotesItemChargePrice != null)
                                    {
                                        ListPropertyEditor lvitemprice = ((DetailView)View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                        foreach (QuotesItemChargePrice objItemCharge in objquotes.QuotesItemChargePrice.ToList())
                                        {
                                            objItemCharge.Discount = objquotes.Discount;
                                            objItemCharge.FinalAmount = objItemCharge.Amount * objItemCharge.Discount / 100;
                                            objItemCharge.FinalAmount = (objItemCharge.Amount - objItemCharge.FinalAmount) * objItemCharge.Qty;
                                        }
                                        ((ListView)lvitemprice.ListView).Refresh();
                                    }
                                    if (objquotes.AnalysisPricing != null)
                                    {
                                        ListPropertyEditor lvanalysisprice = ((DetailView)View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                        foreach (AnalysisPricing objAnalysis in objquotes.AnalysisPricing.ToList())
                                        {
                                            objAnalysis.Discount = objquotes.Discount;
                                            objAnalysis.DiscountAmount = objAnalysis.Discount * objAnalysis.TotalTierPrice / 100;
                                            objAnalysis.FinalAmount = (objAnalysis.TotalTierPrice - objAnalysis.DiscountAmount) * objAnalysis.Qty;
                                        }
                                        ((ListView)lvanalysisprice.ListView).Refresh();
                                    }
                                    objquotes.TotalAmount = objquotes.DetailedAmount - objquotes.DiscountAmount;
                                    objquotes.QuotedAmount = objquotes.DetailedAmount - objquotes.DiscountAmount;
                                }
                                else
                                {
                                    objquotes.Discount = 0;
                                    objquotes.DiscountAmount = 0;
                                }
                            }
                            else
                            {
                                objquotes.Discount = 0;
                                objquotes.DiscountAmount = 0;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "AddAnalysisPriceOrItemCharge"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        else if (e.PropertyName == "IsGobalDiscount")
                        {
                            if (objquotes.IsGobalDiscount == true)
                            {
                                quotesinfo.IsTabDiscountChanged = false;
                                objquotes.IsGobalDiscount = false;
                            }
                        }
                    }
                }
                else if (View.Id == "AnalysisPricing_DetailView_QuotePopup")
                {
                    if (e.PropertyName == "SampleMatries")
                    {
                        if (e.NewValue.ToString().Length > 0)
                        {
                            string[] strSampleMatrix = e.NewValue.ToString().Split(';');

                            List<Matrix> lstSRvisualmat = new List<Matrix>();
                            List<VisualMatrix> lstVMselc = new List<VisualMatrix>();
                            foreach (string strvmoid in strSampleMatrix.ToList())
                            {
                                VisualMatrix lstvmatobj = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strvmoid)));
                                if (lstvmatobj != null)
                                {
                                    lstSRvisualmat.Add(lstvmatobj.MatrixName);
                                    lstVMselc.Add(lstvmatobj);
                                }
                            }
                            string straMatrix = string.Join("','", lstSRvisualmat.Select(i => i.MatrixName).ToList().ToArray());

                            straMatrix = "'" + straMatrix + "'";
                            string strVMSelected = string.Join("','", lstVMselc.Select(i => i.VisualMatrixName).ToList().ToArray());
                            strVMSelected = "'" + strVMSelected + "'";
                            //if(((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView) != null && ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).CollectionSource != null && ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).CollectionSource.GetCount()  == 0)
                            //{ 
                            //CriteriaOperator.Parse(string.Format("[Matrix.MatrixName] in ( {0} and [GCRecord] is Null And [MethodName.GCRecord] Is Null And [MatrixName.GCRecord] Is Null )", straMatrix));
                            ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).CollectionSource.ResetCollection();
                            List<TestMethod> lsttestmethod = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse(string.Format("[MatrixName.MatrixName] in ( {0}) and [GCRecord] is Null And [MethodName.GCRecord] Is Null And [MatrixName.GCRecord] Is Null ", straMatrix))).ToList();
                            if (lsttestmethod != null && lsttestmethod.Count > 0)
                            {
                                foreach (TestMethod objtstmed in lsttestmethod.ToList())
                                {
                                    if (objtstmed.MatrixName != null && objtstmed.MatrixName.Oid != null)
                                    {
                                        //CriteriaOperator.Parse(string.Format("[GCRecord] is Null and [MatrixName.MatrixName] = '{0}' And VisualMatrixName in {1}",objtstmed.MatrixName.MatrixName ,strVMSelected));
                                        IList<VisualMatrix> lstVM = ObjectSpace.GetObjects<VisualMatrix>(CriteriaOperator.Parse(string.Format("[GCRecord] is Null and [MatrixName.MatrixName] = '{0}' And VisualMatrixName in ({1})", objtstmed.MatrixName.MatrixName, strVMSelected)));
                                        if (lstVM != null)
                                        {
                                            foreach (VisualMatrix objViM in lstVM)
                                            {

                                                if (objtstmed.MatrixName != null && objtstmed.TestName != null && objtstmed.MethodName != null)
                                                {

                                                }
                                                else if (objtstmed.MatrixName != null && objtstmed.TestName != null && objtstmed.IsGroup == true)
                                                {
                                                    IObjectSpace osA = ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).ObjectSpace;
                                                    AnalysisPricing newanalysisprice = osA.CreateObject<AnalysisPricing>();
                                                    TestPriceSurcharge objtstpricesur = osA.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = True", objtstmed.MatrixName.MatrixName, objtstmed.TestName));
                                                    if (objtstpricesur != null)
                                                    {
                                                        newanalysisprice.PriceCode = objtstpricesur.PriceCode;
                                                        newanalysisprice.Priority = objtstpricesur.Priority;
                                                        newanalysisprice.ChargeType = objtstpricesur.ChargeType;
                                                    }
                                                    newanalysisprice.SampleMatrix = osA.GetObject<VisualMatrix>(objViM);
                                                    newanalysisprice.Matrix = osA.GetObject<Matrix>(objtstmed.MatrixName);
                                                    newanalysisprice.Test = osA.GetObject<TestMethod>(objtstmed);
                                                    newanalysisprice.IsGroup = objtstmed.IsGroup;

                                                    if (string.IsNullOrEmpty(newanalysisprice.PriceCode))
                                                    {
                                                        quotesinfo.lstpricecodeempty.Add(newanalysisprice.Oid.ToString());
                                                    }
                                                    ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).CollectionSource.Add(newanalysisprice);
                                                    //ListPropertyEditor lvanalysisprice = ((DetailView)View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                                    //((ListView)lvanalysisprice.ListView).CollectionSource.Add(newanalysisprice);
                                                    //((ListView)View).CollectionSource.Add(newanalysisprice);
                                                }

                                                if (objtstmed.IsGroup == false)
                                                {
                                                    List<Component> lstdefcomponent = ObjectSpace.GetObjects<Component>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtstmed.Oid)).ToList();
                                                    Component defcompo = ObjectSpace.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                                                    if (defcompo != null)
                                                    {
                                                        lstdefcomponent.Add(defcompo);
                                                    }
                                                    if (lstdefcomponent != null && lstdefcomponent.Count > 0)
                                                    {
                                                        foreach (Component defcomp in lstdefcomponent.ToList())
                                                        {
                                                            IObjectSpace osA = ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).ObjectSpace;
                                                            AnalysisPricing newanalysispricecomp = ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).ObjectSpace.CreateObject<AnalysisPricing>();
                                                            if (objtstmed.MatrixName != null && objtstmed.TestName != null && objtstmed.MethodName != null && objtstmed.IsGroup == false)
                                                            {

                                                                TestPriceSurcharge objtstpricesur = osA.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And[Component.Components] = ? And [Priority.IsRegular] = 'True'", objtstmed.MatrixName.MatrixName, objtstmed.TestName, objtstmed.MethodName.MethodNumber, defcomp.Components)); //IsRegular
                                                                if (objtstpricesur != null)
                                                                {
                                                                    newanalysispricecomp.PriceCode = objtstpricesur.PriceCode;
                                                                    newanalysispricecomp.Priority = objtstpricesur.Priority;
                                                                    newanalysispricecomp.ChargeType = objtstpricesur.ChargeType;
                                                                }
                                                            }
                                                            else if (objtstmed.MatrixName != null && objtstmed.TestName != null && objtstmed.IsGroup == true)
                                                            {
                                                                TestPriceSurcharge objtstpricesur = osA.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = True", objtstmed.Matrix.MatrixName, objtstmed.Test.TestName));
                                                                if (objtstpricesur != null)
                                                                {
                                                                    newanalysispricecomp.PriceCode = objtstpricesur.PriceCode;
                                                                    newanalysispricecomp.Priority = objtstpricesur.Priority;
                                                                    newanalysispricecomp.ChargeType = objtstpricesur.ChargeType;
                                                                }
                                                            }
                                                            newanalysispricecomp.SampleMatrix = osA.GetObject<VisualMatrix>(objViM);
                                                            newanalysispricecomp.Matrix = osA.GetObject<Matrix>(objtstmed.MatrixName);
                                                            newanalysispricecomp.Test = osA.GetObject<TestMethod>(objtstmed);
                                                            newanalysispricecomp.IsGroup = objtstmed.IsGroup;
                                                            newanalysispricecomp.Method = osA.GetObject<Modules.BusinessObjects.Setting.Method>(objtstmed.MethodName);
                                                            if (string.IsNullOrEmpty(newanalysispricecomp.PriceCode))
                                                            {
                                                                quotesinfo.lstpricecodeempty.Add(newanalysispricecomp.Oid.ToString());
                                                            }
                                                            newanalysispricecomp.Component = osA.GetObject(defcomp);
                                                            ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).CollectionSource.Add(newanalysispricecomp);
                                                            //ListPropertyEditor lvanalysisprice =  ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView) as ListPropertyEditor;
                                                            // ((ListView)lvanalysisprice.ListView).CollectionSource.Add(newanalysispricecomp);
                                                            ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).CreateControls();
                                                        }
                                                    }

                                                }

                                            }
                                        }
                                    }
                                }
                            }
                           //((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).CollectionSource.Criteria["fil"] = CriteriaOperator.Parse(string.Format("[Matrix.MatrixName] in ( {0}) and [GCRecord] is Null And [Method.GCRecord] Is Null And [Matrix.GCRecord] Is Null ", straMatrix));
                           ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).Refresh();

                            //}
                            dtPriceCode = new DataTable();
                            dtPriceCode.Columns.Add("SampleMatrix");
                            dtPriceCode.Columns.Add("PriceCode");
                            ListPropertyEditor lvanalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                            if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                            {
                                foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                {
                                    if (objanapricing.PriceCode != null)
                                    {
                                        DataRow drNew = dtPriceCode.NewRow();
                                        drNew["SampleMatrix"] = objanapricing.SampleMatrix.VisualMatrixName;
                                        drNew["PriceCode"] = objanapricing.PriceCode;
                                        dtPriceCode.Rows.Add(drNew);
                                    }

                                }
                                ASPxGridListEditor gridListEditor = ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).Editor as ASPxGridListEditor;
                                if (dtPriceCode != null && dtPriceCode.Rows.Count > 0)
                                {
                                    foreach (DataRow drpricecode in dtPriceCode.Rows)
                                    {


                                        //((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).CollectionSource.
                                        foreach (AnalysisPricing objanalysisprice in ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).CollectionSource.List.Cast<AnalysisPricing>().Where(i => i.PriceCode == drpricecode["PriceCode"].ToString() && i.SampleMatrix.VisualMatrixName == drpricecode["SampleMatrix"].ToString()))
                                        {
                                            if (!quotesinfo.lsttempAnalysisPricingpopup.Contains(objanalysisprice))
                                            {
                                                quotesinfo.lsttempAnalysisPricingpopup.Add(objanalysisprice);
                                                quotesinfo.lstInitialtempAnalysisPricingpopup.Add(objanalysisprice);
                                            }
                                            gridListEditor.Grid.Selection.SelectRowByKey(objanalysisprice.Oid);
                                        }

                                    }
                                    ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).Refresh();
                                }
                            }
                        }
                        else
                        {
                            ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).CollectionSource.Criteria["fil"] = CriteriaOperator.Parse("1=2");
                            ((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).Refresh();
                        }
                    }
                }
                if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    if (e.Object != null && e.Object.GetType() == typeof(AnalysisPricing))
                    {
                        //IObjectSpace os = Application.CreateObjectSpace();
                        AnalysisPricing objanaly = (AnalysisPricing)e.Object;
                        if (objanaly != null)
                        {
                            ////if (e.PropertyName == "TAT")
                            ////{
                            ////    if (objanaly.TAT != null)
                            ////    {
                            ////        if (objanaly.Matrix != null && objanaly.Test != null && objanaly.Method != null && objanaly.Component != null)
                            ////        {
                            ////            quotesinfo.IsobjChangedproperty = true;
                            ////            bool issurcharge = false;
                            ////            decimal surcharge = 0;
                            ////            List<TestPriceSurcharge> lsttps = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ?", objanaly.Matrix.MatrixName, objanaly.Test.TestName, objanaly.Method.MethodNumber, objanaly.Component.Components)).ToList();
                            ////            foreach (TestPriceSurcharge objtps in lsttps.ToList())
                            ////            {
                            ////                if (objtps != null && !string.IsNullOrEmpty(objtps.TAT) && issurcharge == false)
                            ////                {
                            ////                    string[] strtatqrr = objtps.TAT.Split(';');
                            ////                    foreach (string objtpstat in strtatqrr)
                            ////                    {
                            ////                        if (objtpstat.Trim() == objanaly.TAT.Oid.ToString())
                            ////                        {
                            ////                            surcharge = Convert.ToDecimal(objtps.Surcharge);
                            ////                            surcharge = Math.Round(surcharge, 2);
                            ////                            issurcharge = true;
                            ////                            break;
                            ////                        }
                            ////                    }
                            ////                }
                            ////            }
                            ////            if (surcharge > 0)
                            ////            {
                            ////                objanaly.NPSurcharge = surcharge;
                            ////                decimal schargeamt = objanaly.UnitPrice * (surcharge / 100);
                            ////                objanaly.TotalTierPrice = schargeamt + objanaly.UnitPrice;
                            ////                objanaly.Discount = 0;
                            ////                objanaly.FinalAmount = (schargeamt + objanaly.UnitPrice) * objanaly.Qty;
                            ////                //AnalysisPricing objanaprice = View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", objanaly.Oid));
                            ////                //if (objanaprice != null)
                            ////                //{
                            ////                //    objanaly.FinalAmount = (schargeamt + objanaly.UnitPrice) * objanaly.Qty;
                            ////                //}
                            ////            }
                            ////        }
                            ////        if (objanaly.Matrix != null && objanaly.Test != null && objanaly.IsGroup == true)
                            ////        {
                            ////            quotesinfo.IsobjChangedproperty = true;
                            ////            bool issurcharge = false;
                            ////            decimal surcharge = 0;
                            ////            List<TestPriceSurcharge> lsttps = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = True", objanaly.Matrix.MatrixName, objanaly.Test.TestName)).ToList();

                            ////            foreach (TestPriceSurcharge objtps in lsttps.ToList())
                            ////            {
                            ////                if (objtps != null && !string.IsNullOrEmpty(objtps.TAT) && issurcharge == false)
                            ////                {
                            ////                    string[] strtatqrr = objtps.TAT.Split(';');
                            ////                    foreach (string objtpstat in strtatqrr)
                            ////                    {
                            ////                        if (objtpstat.Trim() == objanaly.TAT.Oid.ToString())
                            ////                        {
                            ////                            surcharge = Convert.ToDecimal(objtps.Surcharge);
                            ////                            surcharge = Math.Round(surcharge, 2);
                            ////                            issurcharge = true;
                            ////                            break;
                            ////                        }
                            ////                    }
                            ////                }
                            ////            }

                            ////            if (surcharge > 0)
                            ////            {
                            ////                decimal schargeamt = objanaly.UnitPrice * (surcharge / 100);
                            ////                objanaly.TotalTierPrice = schargeamt + objanaly.UnitPrice;
                            ////                objanaly.Discount = 0;
                            ////                objanaly.FinalAmount = (schargeamt + objanaly.UnitPrice) * objanaly.Qty;
                            ////                objanaly.NPSurcharge = surcharge;
                            ////                AnalysisPricing objanaprice = os.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", objanaly.Oid));
                            ////                if (objanaprice != null)
                            ////                {
                            ////                    objanaly.FinalAmount = (schargeamt + objanaly.UnitPrice) * objanaly.Qty;
                            ////                    os.CommitChanges();
                            ////                    os.Refresh();
                            ////                }
                            ////            }
                            ////        }
                            ////    }
                            ////}
                            ////if (e.PropertyName == "Qty" && quotesinfo.IsobjChangedproperty == false)
                            ////{
                            ////    quotesinfo.IsobjChangedproperty = true;
                            ////    if (objanaly.Qty > 0 && objanaly.Discount == 0)
                            ////    {
                            ////        decimal FinalAmt = objanaly.Qty * objanaly.TotalTierPrice;
                            ////        objanaly.FinalAmount = Math.Round(FinalAmt, 2);
                            ////    }
                            ////    else
                            ////    if (objanaly.Qty > 0 && objanaly.Discount != 0)
                            ////    {
                            ////        decimal discntamount = 0;
                            ////        decimal FinalAmt = 0;
                            ////        if (objanaly.Discount > 0)
                            ////        {
                            ////            discntamount = (objanaly.TotalTierPrice) * (objanaly.Discount / 100);
                            ////            FinalAmt = objanaly.Qty * (objanaly.TotalTierPrice - discntamount);
                            ////            objanaly.FinalAmount = Math.Round(FinalAmt, 2);
                            ////        }
                            ////        else if (objanaly.Discount < 0)
                            ////        {
                            ////            discntamount = (objanaly.TotalTierPrice) * (objanaly.Discount / 100);
                            ////            FinalAmt = objanaly.Qty * (objanaly.TotalTierPrice - discntamount);
                            ////            objanaly.FinalAmount = Math.Round(FinalAmt, 2);
                            ////        }
                            ////    }
                            ////    if (objanaly.Qty <= 0)
                            ////    {
                            ////        objanaly.Qty = 1;
                            ////        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            ////    }
                            ////}
                            ////else
                            ////if (e.PropertyName == "TotalTierPrice" && quotesinfo.IsobjChangedproperty == false)
                            ////{
                            ////    quotesinfo.IsobjChangedproperty = true;
                            ////    decimal newtotalprice = Convert.ToDecimal(e.NewValue);
                            ////    if (newtotalprice > 0 && objanaly.UnitPrice > 0)
                            ////    {
                            ////        decimal discntamount = ((objanaly.UnitPrice - objanaly.TotalTierPrice) / objanaly.UnitPrice) * 100;
                            ////        decimal FinalAmt = objanaly.Qty * objanaly.TotalTierPrice;
                            ////        objanaly.FinalAmount = Math.Round(FinalAmt, 2);
                            ////        objanaly.Discount = Math.Round(discntamount, 2);
                            ////    }
                            ////    if (objanaly.TotalTierPrice < 0)
                            ////    {
                            ////        objanaly.TotalTierPrice = 0;
                            ////        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            ////    }
                            ////}
                            ////else
                            ////if (e.PropertyName == "Discount" && quotesinfo.IsobjChangedproperty == false)
                            ////{
                            ////    quotesinfo.IsobjChangedproperty = true;
                            ////    decimal discntamount = 0;
                            ////    if (objanaly.Discount > 0)
                            ////    {
                            ////        discntamount = (objanaly.TotalTierPrice) * (objanaly.Discount / 100);
                            ////        decimal FinalAmt = objanaly.Qty * (objanaly.TotalTierPrice - discntamount);
                            ////        objanaly.FinalAmount = Math.Round(FinalAmt, 2);
                            ////    }
                            ////    else if (objanaly.Discount < 0)
                            ////    {
                            ////        discntamount = (objanaly.TotalTierPrice) * (objanaly.Discount / 100);
                            ////        decimal FinalAmt = objanaly.Qty * (objanaly.TotalTierPrice - discntamount);
                            ////        objanaly.FinalAmount = Math.Round(FinalAmt, 2);
                            ////        ////decimal totalamt = objanaly.UnitPrice + discntamount;
                            ////        ////objanaly.TotalTierPrice = Convert.ToDecimal(String.Format("{0:0.00}", totalamt));
                            ////    }
                            ////    else if (objanaly.Discount == 0)
                            ////    {
                            ////        decimal FinalAmt = objanaly.Qty * (objanaly.TotalTierPrice);
                            ////        objanaly.FinalAmount = Math.Round(FinalAmt, 2);
                            ////    }
                            ////}
                            ////else
                            ////if (e.PropertyName == "UnitPrice")
                            ////{
                            ////    objanaly.TotalTierPrice = objanaly.UnitPrice;
                            ////}
                            ////else
                            ////if (e.PropertyName == "Matrix")
                            ////{
                            ////    objanaly.Test = null;
                            ////    objanaly.Method = null;
                            ////    objanaly.Component = null;
                            ////    objanaly.IsGroup = false;
                            ////}
                            ////else
                            ////if (e.PropertyName == "IsGroup")
                            ////{
                            ////    quotesinfo.IsobjChangedproperty = true;
                            ////    if (objanaly.Matrix != null && objanaly.Test != null && objanaly.IsGroup == true)
                            ////    {
                            ////        TestMethod objtstmed = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [IsGroup] = 'True'", objanaly.Matrix.MatrixName, objanaly.Test.TestName));
                            ////        if (objtstmed != null)
                            ////        {
                            ////            objanaly.TestDescription = objtstmed.Comment;
                            ////        }
                            ////    }
                            ////    objanaly.Method = null;
                            ////    objanaly.Component = null;
                            ////    if (objanaly.Matrix != null && objanaly.Test != null && objanaly.IsGroup == true)
                            ////    {
                            ////        DefaultPricing objdefaultpricing = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'Yes' ", objanaly.Matrix.MatrixName, objanaly.Test.TestName));
                            ////        ConstituentPricing objconsituteprice = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'True'", objanaly.Matrix.MatrixName, objanaly.Test.TestName));
                            ////        if (objconsituteprice != null)
                            ////        {
                            ////            int grptestcnt = 0;
                            ////            decimal tempprice = 0;
                            ////            decimal tierprc = 0;
                            ////            decimal prep1prc = 0;
                            ////            decimal prep2prc = 0;
                            ////            if (objconsituteprice.Test != null && objconsituteprice.IsGroup == true)
                            ////            {
                            ////                List<GroupTestMethod> lstgrptest = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objanaly.Test.Oid)).ToList();
                            ////                if (lstgrptest.Count > 0)
                            ////                {
                            ////                    grptestcnt = lstgrptest.Count;
                            ////                }
                            ////            }
                            ////            List<ConstituentPricingTier> lstconstituentpricrtier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ?", objconsituteprice.Oid)).ToList();
                            ////            foreach (ConstituentPricingTier objconstier in lstconstituentpricrtier.OrderBy(i => i.TierNo).ToList())
                            ////            {
                            ////                if (objconstier.To > 0 && grptestcnt > 0)
                            ////                {
                            ////                    int tiercnt = Convert.ToInt32(objconstier.To);
                            ////                    grptestcnt = grptestcnt - tiercnt;
                            ////                    tempprice = tempprice + objconstier.TotalTierPrice;
                            ////                    tierprc = tempprice + objconstier.TierPrice;
                            ////                    prep1prc = tempprice + objconstier.Prep1Charge;
                            ////                    prep2prc = tempprice + objconstier.Prep2Charge;
                            ////                }
                            ////            }
                            ////            objanaly.UnitPrice = tempprice;
                            ////            objanaly.NPUnitPrice = tempprice;
                            ////            objanaly.TotalTierPrice = tempprice;
                            ////            objanaly.NPTotalPrice = tempprice;
                            ////            objanaly.FinalAmount = tempprice;
                            ////            objanaly.TierPrice = tierprc;
                            ////            objanaly.Prep1Charge = prep1prc;
                            ////            objanaly.Prep2Charge = prep2prc;
                            ////            objanaly.ChargeType = objconsituteprice.ChargeType;
                            ////        }
                            ////        else
                            ////        if (objdefaultpricing != null)
                            ////        {
                            ////            objanaly.UnitPrice = objdefaultpricing.TotalUnitPrice;
                            ////            objanaly.FinalAmount = objdefaultpricing.TotalUnitPrice;
                            ////            objanaly.NPUnitPrice = objdefaultpricing.TotalUnitPrice;
                            ////            objanaly.NPTotalPrice = objdefaultpricing.TotalUnitPrice;
                            ////            objanaly.ChargeType = objdefaultpricing.ChargeType;
                            ////            objanaly.TierPrice = objdefaultpricing.UnitPrice;
                            ////            objanaly.Prep1Charge  = objdefaultpricing.Prep1Charge;
                            ////            objanaly.Prep2Charge = objdefaultpricing.Prep2Charge;
                            ////        }
                            ////    }
                            ////}
                            ////else
                            ////if (e.PropertyName == "Test")
                            ////{
                            ////    if(objanaly.Test == null)
                            ////    {
                            ////        objanaly.IsGroup = false;
                            ////        objanaly.Method = null;
                            ////        objanaly.Component = null;
                            ////    }   
                            ////    else
                            ////    {
                            ////        TestMethod objtm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [IsGroup] = True", objanaly.Matrix.MatrixName, objanaly.Test.TestName));
                            ////        if (objtm != null)
                            ////        {
                            ////            objanaly.IsGroup = true;
                            ////        }
                            ////    }                              
                            ////}
                            ////else
                            ////if (e.PropertyName == "Method")
                            ////{
                            ////    objanaly.Component = null;
                            ////    if (objanaly.Matrix != null && objanaly.Test != null && objanaly.Method != null)
                            ////    {
                            ////        TestMethod objtstmed = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [MethodName.MethodNumber] = ?", objanaly.Matrix.MatrixName, objanaly.Test.TestName, objanaly.Method.MethodNumber));
                            ////        if (objtstmed != null)
                            ////        {
                            ////            objanaly.TestDescription = objtstmed.Comment;
                            ////        }
                            ////    }
                            ////}
                            ////else
                            ////if (e.PropertyName == "Component")
                            ////{
                            ////    if (objanaly.Matrix != null && objanaly.Test != null && objanaly.IsGroup == false && objanaly.Method != null && objanaly.Component != null)
                            ////    {
                            ////        IObjectSpace objspace = Application.CreateObjectSpace();
                            ////        quotesinfo.IsobjChangedproperty = true;
                            ////        DefaultPricing objdefaultpricing = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objanaly.Matrix.MatrixName, objanaly.Test.TestName, objanaly.Method.MethodNumber, objanaly.Component.Components));
                            ////        ConstituentPricing objconsituteprice = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objanaly.Matrix.MatrixName, objanaly.Test.TestName, objanaly.Method.MethodNumber, objanaly.Component.Components));
                            ////        if (objconsituteprice != null)
                            ////        {
                            ////            decimal tempprice = 0;
                            ////            decimal tierprc = 0;
                            ////            decimal prep1prc = 0;
                            ////            decimal prep2prc = 0;
                            ////            decimal inttotaltierprice = 0;
                            ////            int paracnt = 0;
                            ////            if (objconsituteprice.Test != null && objconsituteprice.Component != null)
                            ////            {
                            ////                List<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Component.Oid] = ?", objanaly.Test.Oid, objanaly.Component.Oid)).ToList();
                            ////                if (lsttestpara.Count > 0)
                            ////                {
                            ////                    paracnt = lsttestpara.Count;
                            ////                }
                            ////            }
                            ////            List<ConstituentPricingTier> lstconstituentpricrtier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ?", objconsituteprice.Oid)).ToList();
                            ////            foreach (ConstituentPricingTier objconstier in lstconstituentpricrtier.OrderBy(i => i.TierNo).ToList())
                            ////            {
                            ////                if (objconstier.To > 0 && paracnt > 0)
                            ////                {
                            ////                    int tiercnt = Convert.ToInt32(objconstier.To);
                            ////                    paracnt = paracnt - tiercnt;
                            ////                    tempprice = tempprice + objconstier.TotalTierPrice;
                            ////                    tierprc = tierprc + objconstier.TierPrice;
                            ////                    prep1prc = tierprc + objconstier.Prep1Charge;
                            ////                    prep2prc = tierprc + objconstier.Prep2Charge;
                            ////                }
                            ////            }
                            ////            objanaly.UnitPrice = tempprice;
                            ////            objanaly.TotalTierPrice = tempprice;
                            ////            objanaly.NPUnitPrice = tempprice;
                            ////            objanaly.NPTotalPrice = tempprice;
                            ////            objanaly.FinalAmount = tempprice;
                            ////            objanaly.TierPrice = tierprc;
                            ////            objanaly.Prep1Charge = prep1prc;
                            ////            objanaly.Prep2Charge = prep2prc;
                            ////            if ( !string.IsNullOrEmpty(e.NewValue.ToString()))
                            ////            {
                            ////                objanaly.Parameter = "AllParams";
                            ////            }
                            ////            else
                            ////            {
                            ////                objanaly.Parameter = "";
                            ////            }
                            ////            objanaly.ChargeType = objconsituteprice.ChargeType;
                            ////        }
                            ////        else
                            ////        if (objdefaultpricing != null)
                            ////        {
                            ////            objanaly.UnitPrice = objdefaultpricing.TotalUnitPrice;
                            ////            objanaly.TotalTierPrice = objdefaultpricing.TotalUnitPrice;
                            ////            objanaly.NPTotalPrice = objdefaultpricing.TotalUnitPrice;
                            ////            objanaly.NPUnitPrice = objdefaultpricing.TotalUnitPrice;
                            ////            objanaly.FinalAmount = objdefaultpricing.TotalUnitPrice;
                            ////            objanaly.TierPrice = objdefaultpricing.UnitPrice;
                            ////            objanaly.Prep1Charge = objdefaultpricing.Prep1Charge;
                            ////            objanaly.Prep2Charge = objdefaultpricing.Prep2Charge;
                            ////            if (!string.IsNullOrEmpty(e.NewValue.ToString()))
                            ////            {
                            ////                objanaly.Parameter = "AllParams";
                            ////            }
                            ////            else
                            ////            {
                            ////                objanaly.Parameter = "";
                            ////            }
                            ////            objanaly.ChargeType = objdefaultpricing.ChargeType;
                            ////        }
                            ////    }
                        }
                    }
                }
                if (View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView")
                {
                    if (e.Object != null && e.Object.GetType() == typeof(ItemChargePricing))
                    {
                        //ItemChargePricing objitemprice = (ItemChargePricing)e.Object;
                        //if (objitemprice != null && e.PropertyName == "CRMQuotes")
                        //{
                        //    objitemprice.Amount = objitemprice.UnitPrice;
                        //    objitemprice.FinalAmount = objitemprice.UnitPrice;
                        //    objitemprice.NpUnitPrice = objitemprice.UnitPrice;
                        //}
                        //if (objitemprice != null && e.PropertyName == "Qty")
                        //{
                        //    objitemprice.Amount = objitemprice.NpUnitPrice * objitemprice.Qty;
                        //    objitemprice.FinalAmount = objitemprice.UnitPrice * objitemprice.Qty;
                        //}
                        //if (objitemprice != null && e.PropertyName == "UnitPrice")
                        //{
                        //    objitemprice.Amount = objitemprice.NpUnitPrice * objitemprice.Qty;
                        //    objitemprice.FinalAmount = objitemprice.UnitPrice * objitemprice.Qty;
                        //    objitemprice.Discount = Math.Round((((objitemprice.NpUnitPrice - objitemprice.UnitPrice) / objitemprice.NpUnitPrice) * 100), 2);
                        //}
                        //if (objitemprice != null && e.PropertyName == "Amount")
                        //{
                        //    objitemprice.Amount = objitemprice.NpUnitPrice * objitemprice.Qty;
                        //    objitemprice.FinalAmount = objitemprice.UnitPrice * objitemprice.Qty;
                        //    objitemprice.Discount = Math.Round((((objitemprice.NpUnitPrice - objitemprice.UnitPrice) / objitemprice.NpUnitPrice) * 100), 2);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dcreason_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                bool IsSubmitted = false;
                if (sender != null)
                {
                    DialogController dc = sender as DialogController;
                    if (dc != null && dc.Window != null && dc.Window.View != null)
                    {
                        DummyClass objdumycls = (DummyClass)dc.Window.View.CurrentObject;
                        if (dc.Window.View.Id == "DummyClass_DetailView_Reasons" && Application.MainWindow.View.Id == "CRMQuotes_DetailView")
                        {
                            CRMQuotes objcrmquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                            if (objcrmquotes != null && objcrmquotes.Cancel == true && objdumycls != null && !string.IsNullOrEmpty(objdumycls.Reason))
                            {
                                objcrmquotes.CancelReason = objdumycls.Reason;
                                objcrmquotes.Status = CRMQuotes.QuoteStatus.Canceled;
                                objcrmquotes.Cancel = true;
                                IsSubmitted = true;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cancellsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                            else if (string.IsNullOrEmpty(objdumycls.Reason))
                            {
                                e.Cancel = true;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Quotereason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        else if (dc.Window.View.Id == "DummyClass_DetailView_Reasons" && Application.MainWindow.View.Id == "CRMQuotes_DetailView_Submitted")
                        {
                            CRMQuotes objcrmquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                            if (objcrmquotes != null && objdumycls != null && !string.IsNullOrEmpty(objdumycls.Reason))
                            {
                                objcrmquotes.RollBackReason = objdumycls.Reason;
                                objcrmquotes.Status = CRMQuotes.QuoteStatus.PendingSubmission;
                                IsSubmitted = true;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                            else if (string.IsNullOrEmpty(objdumycls.Reason))
                            {
                                e.Cancel = true;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Quotereason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        else if (dc.Window.View.Id == "DummyClass_DetailView_Reasons" && Application.MainWindow.View.Id == "CRMQuotes_ListView_PendingReview")
                        {
                            foreach (CRMQuotes objcrmquot in Application.MainWindow.View.SelectedObjects)
                            {
                                if (objcrmquot != null && objdumycls != null && !string.IsNullOrEmpty(objdumycls.Reason))
                                {
                                    objcrmquot.RollBackReason = objdumycls.Reason;
                                    objcrmquot.Status = CRMQuotes.QuoteStatus.PendingSubmission;
                                    IsSubmitted = true;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                                else if (string.IsNullOrEmpty(objdumycls.Reason))
                                {
                                    e.Cancel = true;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Quotereason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                            }
                        }
                        else if (dc.Window.View.Id == "DummyClass_DetailView_Reasons" && Application.MainWindow.View.Id == "CRMQuotes_ListView_ReviewHistory" || View.Id== "CRMQuotes_ListView_SubmittedQuotes_History")
                        {
                            foreach (CRMQuotes objcrmquot in Application.MainWindow.View.SelectedObjects)
                            {
                                if (objcrmquot != null && objdumycls != null && !string.IsNullOrEmpty(objdumycls.Reason))
                                {
                                    objcrmquot.RollBackReason = objdumycls.Reason;
                                    objcrmquot.Status = CRMQuotes.QuoteStatus.PendingSubmission;
                                    IsSubmitted = true;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                                else if (string.IsNullOrEmpty(objdumycls.Reason))
                                {
                                    e.Cancel = true;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Quotereason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                            }
                        }
                        else if (dc.Window.View.Id == "DummyClass_DetailView_Reasons" && Application.MainWindow.View.Id == "CRMQuotes_ListView_Cancel")
                        {
                            foreach (CRMQuotes objcrmquot in Application.MainWindow.View.SelectedObjects)
                            {
                                if (objcrmquot != null && objdumycls != null && !string.IsNullOrEmpty(objdumycls.Reason))
                                {
                                    objcrmquot.ReactiveReason = objdumycls.Reason;
                                    objcrmquot.Cancel = false;
                                    objcrmquot.DateCanceled = DateTime.MinValue;
                                    objcrmquot.CancelReason = null;
                                    objcrmquot.Status = CRMQuotes.QuoteStatus.PendingSubmission;
                                    IsSubmitted = true;
                                    if (objcrmquot.ExpirationDate == DateTime.Today)
                                    {
                                        objcrmquot.ExpirationDate = DateTime.Today;
                                    }
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "reactivatesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                                else if (string.IsNullOrEmpty(objdumycls.Reason))
                                {
                                    e.Cancel = true;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Quotereason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                            }
                        }
                        else if (dc.Window.View.Id == "DummyClass_DetailView_Reasons" && Application.MainWindow.View.Id == "CRMQuotes_ListView_Expired")
                        {
                            foreach (CRMQuotes objcrmquot in Application.MainWindow.View.SelectedObjects)
                            {
                                if (objcrmquot != null && objdumycls != null && !string.IsNullOrEmpty(objdumycls.Reason))
                                {
                                    objcrmquot.ReactiveReason = objdumycls.Reason;
                                    objcrmquot.Status = CRMQuotes.QuoteStatus.PendingSubmission;
                                    IsSubmitted = true;
                                    if (objcrmquot.ExpirationDate <= DateTime.Today)
                                    {
                                        objcrmquot.ExpirationDate = DateTime.Today;
                                    }
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "reactivatesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                                else if (string.IsNullOrEmpty(objdumycls.Reason))
                                {
                                    e.Cancel = true;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Quotereason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                            }
                        }
                        else if (dc.Window.View.Id == "DummyClass_DetailView_Reasons" && Application.MainWindow.View.Id == "CRMQuotes_DetailView_Cancel_Expired")
                        {
                            CRMQuotes objcrmquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                            if (objcrmquotes != null && objdumycls != null && !string.IsNullOrEmpty(objdumycls.Reason))
                            {
                                objcrmquotes.Cancel = false;
                                objcrmquotes.DateCanceled = DateTime.MinValue;
                                objcrmquotes.CancelReason = null;
                                objcrmquotes.RollBackReason = objdumycls.Reason;
                                objcrmquotes.Status = CRMQuotes.QuoteStatus.PendingSubmission;
                                IsSubmitted = true;
                                if (objcrmquotes.ExpirationDate < DateTime.Today)
                                {
                                    objcrmquotes.ExpirationDate = DateTime.Today;
                                }
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                            else if (string.IsNullOrEmpty(objdumycls.Reason))
                            {
                                e.Cancel = true;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Quotereason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        else if (dc.Window.View.Id == "DummyClass_DetailView_Reasons" && Application.MainWindow.View.Id == "CRMQuotes_DetailView_Submitted_History")
                        {
                            CRMQuotes objcrmquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                            if (objcrmquotes != null && objdumycls != null && !string.IsNullOrEmpty(objdumycls.Reason))
                            {
                                objcrmquotes.Cancel = false;
                                objcrmquotes.DateCanceled = DateTime.MinValue;
                                objcrmquotes.CancelReason = null;
                                objcrmquotes.RollBackReason = objdumycls.Reason;
                                objcrmquotes.Status = CRMQuotes.QuoteStatus.PendingSubmission;
                                IsSubmitted = true;
                                if (objcrmquotes.ExpirationDate < DateTime.Today)
                                {
                                    objcrmquotes.ExpirationDate = DateTime.Today;
                                }
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                            else if (string.IsNullOrEmpty(objdumycls.Reason))
                            {
                                e.Cancel = true;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Quotereason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                    }
                }
                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();
                if (IsSubmitted == true)
                {
                    //QuotesNavgCount();
                }
                if (View.Id == "CRMQuotes_DetailView_Submitted")
                {
                    View.Close();
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
                if (View.Id == "AnalysisPricing_ListView_QuotesPopup")
                {
                    //if(quotesinfo.lstInitialtempAnalysisPricingpopup != null && quotesinfo.lstInitialtempAnalysisPricingpopup.Count > 0)
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["filter"] = new NotOperator(new InOperator("Oid", quotesinfo.lstInitialtempAnalysisPricingpopup.ToList()));
                    //}
                    //AnalysisPricing objAna = (AnalysisPricing)View.CurrentObject;
                    //if (objAna.SampleMatries != null)
                    //{
                    //    string[] strSampleMatrix = objAna.SampleMatries.ToString().Split(';');

                    //    List<Matrix> lstSRvisualmat = new List<Matrix>();
                    //    foreach (string strvmoid in strSampleMatrix.ToList())
                    //    {
                    //        VisualMatrix lstvmatobj = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strvmoid)));
                    //        if (lstvmatobj != null)
                    //        {
                    //            lstSRvisualmat.Add(lstvmatobj.MatrixName);
                    //        }
                    //    }
                    //    string straMatrix = string.Join("','", lstSRvisualmat.Select(i => i.MatrixName).ToList().ToArray());

                    //    straMatrix = "'" + straMatrix + "'";


                    //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(string.Format("[MatrixName.MatrixName] in ( {0}) and [GCRecord] is Null And [MethodName.GCRecord] Is Null And [MatrixName.GCRecord] Is Null ", straMatrix));
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("1=2");
                    //}
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    Frame.GetController<RefreshController>().RefreshAction.Active.SetItemValue("RefreshAction", false);
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.Settings.VerticalScrollableHeight = 400;
                        gridlisteditor.Grid.Load += Grid_Load;
                        gridlisteditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("Quotestest", this);
                        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                        if (quotesinfo.IsAnalycialPricingpopupselectall == true)
                        {
                            gridlisteditor.Grid.JSProperties["cpEndCallbackHandlers"] = "selectall";
                        }
                        else
                        {
                            gridlisteditor.Grid.JSProperties["cpEndCallbackHandlers"] = null;
                        }
                        //gridlisteditor.Grid.Load += Grid_Load;
                        //gridlisteditor.Grid.HtmlCommandCellPrepared += GridView_HtmlCommandCellPrepared;
                        gridlisteditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
var chkselect = s.cpEndCallbackHandlers;
                        s.batchEditApi.ResetChanges(e.visibleIndex);
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {     

                            RaiseXafCallback(globalCallbackControl, 'Quotestest', 'Quotestestpopup|Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'Quotestest', 'Quotestestpopup|UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                        //if(chkselect != 'selectall')
                        //  {
                        //        RaiseXafCallback(globalCallbackControl, 'Quotestest', 'Quotestestpopup|Selectall', '', false);         
                        //   }
                        //RaiseXafCallback(globalCallbackControl, 'Quotestest', 'Quotestestpopup|Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'Quotestest', 'Quotestestpopup|UNSelectall', '', false);                        
                      }   
                      //else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.pageRowCount)
                      //{        
                      //  RaiseXafCallback(globalCallbackControl, 'Quotestest', 'Quotestestpopup|Selectall', '', false);     
                      //}
                    }";
                    }
                }
                //else if (View.Id == "TestPriceSurcharge_ListView_Quotes_Popup")
                //{
                //    ASPxGridListEditor gridlistEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //    if (gridlistEditor != null && gridlistEditor.Grid != null)
                //    {
                //        gridlistEditor.Grid.Load += Grid_Load;
                //    }
                //}
                else if (View.Id == "AnalysisPricing_DetailView_QuotePopup")
                {
                    if (View.CurrentObject != null)
                    {
                        AnalysisPricing objAna = (AnalysisPricing)View.CurrentObject;
                        if (objAna.SampleMatrix == null)
                        {
                            //IList<VisualMatrix> objVM = ObjectSpace.GetObjects<VisualMatrix>(CriteriaOperator.Parse(""));
                            //objAna.SampleMatries  = objVM[0].VisualMatrixName;
                        }
                        var editor = (DevExpress.ExpressApp.Web.Editors.WebPropertyEditor)((DetailView)View).FindItem("SampleMatrix");
                        VisualMatrix vm = (VisualMatrix)editor.PropertyValue;
                        //DashboardViewItem dvi =   (DashboardViewItem)((DetailView)View).FindItem("AnalysisPricing");
                        //if (dvi != null && dvi.InnerView != null )
                        //{
                        //    ((ListView)dvi.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(string.Format("[Matrix.Oid] = '{0}'", vm.MatrixName.Oid));
                        //}
                        foreach (ViewItem item in ((DetailView)View).Items)
                        {
                            if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                            {
                                ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                                if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                                ASPxGridLookup lookup = (ASPxGridLookup)propertyEditor.Editor;
                                if (lookup != null && propertyEditor.Id == "SampleMatries")
                                {
                                    //lookup.GridViewProperties.Settings.ShowFilterRow = true;
                                    lookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                    lookup.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                    lookup.GridView.Settings.VerticalScrollableHeight = 200;
                                    lookup.GridViewProperties.SettingsSearchPanel.Visible = true;
                                    foreach (WebColumnBase columns in lookup.GridView.VisibleColumns)
                                    {
                                        if (columns.Index == 1)
                                        {
                                            columns.Caption = "Sample Matrice";
                                        }
                                    }
                                }
                            }
                        }

                    }

                }


                else if (View.Id == "Testparameter_LookupListView_Quotes")
                {
                    //if (quotesinfo.lsttempparamsoid == null)
                    //{
                    //    quotesinfo.lsttempparamsoid = new List<Guid>();
                    //}
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.Load += Grid_Load;
                        gridlisteditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("Quotesparameter", this);
                        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                        gridlisteditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
                        s.batchEditApi.ResetChanges(e.visibleIndex);
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {     

                            RaiseXafCallback(globalCallbackControl, 'Quotesparameter', 'Quotesparameter|Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'Quotesparameter', 'Quotesparameter|UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                        RaiseXafCallback(globalCallbackControl, 'Quotesparameter', 'Quotesparameter|Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'Quotesparameter', 'Quotesparameter|UNSelectall', '', false);                        
                      }   
                      //else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.pageRowCount)
                      //{        
                      //  RaiseXafCallback(globalCallbackControl, 'Quotesparameter', 'Quotesparameter|Selectall', '', false);     
                      //}
                    }";
                    }
                }
                else if (View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor.AllowEdit != null && gridlisteditor.Grid != null)
                    {
                        if (gridlisteditor.Grid.Columns["InlineEditCommandColumn"] != null)
                        {
                            gridlisteditor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                        }
                        if (quotesinfo.IsItemchargePricingpopupselectall == true)
                        {
                            gridlisteditor.Grid.JSProperties["cpEndCallbackHandlers"] = "selectall";
                        }
                        else
                        {
                            gridlisteditor.Grid.JSProperties["cpEndCallbackHandlers"] = null;
                        }
                        //foreach (GridViewColumn column in gridlisteditor.Grid.Columns)
                        //{
                        //    if (column.Name == "InlineEditCommandColumn")
                        //    {
                        //        column.Visible = false;
                        //    }
                        //}
                        gridlisteditor.Grid.Settings.ShowStatusBar = DevExpress.Web.GridViewStatusBarMode.Hidden;
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("QuotesItemCharge", this);
                        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                        gridlisteditor.Grid.JSProperties["cpViewID"] = View.Id;
                        gridlisteditor.Grid.Load += Grid_Load;
                        gridlisteditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                        gridlisteditor.Grid.SettingsContextMenu.Enabled = true;
                        gridlisteditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;

                        gridlisteditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                           {
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
                           }";
                        gridlisteditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
var chkselect = s.cpEndCallbackHandlers;
                        var viewid = s.cpViewID;
                        s.batchEditApi.ResetChanges(e.visibleIndex);
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {     

                            RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                         var viewid = s.cpViewID;
        if(chkselect != 'selectall')
        {
            RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|Selectall', '', false); 
        }
                        //RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                            var viewid = s.cpViewID;
                        RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|UNSelectall', '', false);                        
                      }   
                      ////else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.pageRowCount)
                      ////{   
                      ////   var viewid = s.cpViewID;
                      ////  RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|Selectall', '', false);     
                      ////}
                    }";
                        gridlisteditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function (s, e){
                                                window.setTimeout(function() {
                                                //s.UpdateEdit();  
                                                            var FocusedColumn = sessionStorage.getItem('PrevFocusedColumn');
                                                            var FocusedColumn1 = sessionStorage.getItem('CurrFocusedColumn');
                                                            if (e.visibleIndex != '-1')
                                                            {
                                                                var viewid = s.cpViewID;
                                                                //s.batchEditApi.ResetChanges(e.visibleIndex);
                                                                s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                                                                RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  viewid +'|ValuesChange|' + Oidvalue +'|'+  s.cpViewID +'|' + FocusedColumn +'|'+ FocusedColumn1 , '', false); 
                                                                }); 
                                                            }
                                                }, 20); 
                                            }";
                        if (View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView")
                        {
                            gridlisteditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                                { 
                                if (s.IsRowSelectedOnPage(e.elementIndex)) 
                                {
                                var FocusedColumn = sessionStorage.getItem('CurrFocusedColumn');                                
                                var oid;
                                var text;
//alert(FocusedColumn);
                                if(FocusedColumn=='Qty' || FocusedColumn=='Discount' || FocusedColumn=='FinalAmount')
                                {
                                if(FocusedColumn.includes('.'))
                                {
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;   
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        {             
                                           if (s.IsRowSelectedOnPage(i)) 
                                           {
                                                if(FocusedColumn=='TAT.TAT')
                                                {
                                                  s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);   
                                                }
                                           }
                                         }
                                     }        
                                 }
                                 else
                                 {                                                             
                                    var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        {   
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {
                                                s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                                if(FocusedColumn=='Qty')
                                                {
                                                    var unprice = s.batchEditApi.GetCellValue(i,'UnitPrice');  
                                                    var undiscount = s.batchEditApi.GetCellValue(i,'Discount');  
                                                    if(unprice != null)
                                                    {
                                                        var tempprice = CopyValue * unprice; 
                                                        tempprice = Math.round(tempprice * 100) / 100; 
                                                        //s.batchEditApi.SetCellValue(i,'FinalAmount',tempprice); 
                                                        var FinalTempPrice = tempprice * undiscount / 100;
                                                        FinalTempPrice = tempprice - FinalTempPrice;
                                                        s.batchEditApi.SetCellValue(i,'FinalAmount',FinalTempPrice); 
                                                        s.batchEditApi.SetCellValue(i,'Amount',tempprice);
                                                   }
                                                }
                                                else if(FocusedColumn=='Discount')
                                                { 
//alert(CopyValue);
                                                    var tempprice = s.batchEditApi.GetCellValue(i,'Amount');   
                                                    var undiscount = CopyValue;  
                                                    if(tempprice != null)
                                                    {
                                                        tempprice = Math.round(tempprice * 100) / 100;  
                                                        var FinalTempPrice = tempprice * undiscount / 100;
                                                        FinalTempPrice = tempprice - FinalTempPrice;
                                                        s.batchEditApi.SetCellValue(i,'FinalAmount',FinalTempPrice); 
                                                        s.batchEditApi.SetCellValue(i,'Amount',tempprice);
                                                   }
                                                }
                                                else if(FocusedColumn=='FinalAmount')
                                                {  
//alert(CopyValue);
                                                    var fnlamt = CopyValue; 
                                                    var tempprice = s.batchEditApi.GetCellValue(i,'Amount');  
                                                    if(tempprice != null)
                                                    { 
                                                        tempprice = Math.round(tempprice * 100) / 100; 
                                                        var dicntamt = tempprice - fnlamt ;
                                                        var disper = (dicntamt / tempprice) * 100;
                                                        s.batchEditApi.SetCellValue(i,'Discount',disper);
                                                   }
                                                }
                                            }
                                        }
                                        if(FocusedColumn=='Qty' || FocusedColumn=='FinalAmount' || FocusedColumn=='Discount')
                                        {
                                            RaiseXafCallback(globalCallbackControl, 'QuotesItemCharge',  s.cpViewID +'|CopytoAllCellValuesChange|' + '|'+ FocusedColumn , '', false);                                                     
                                        }
                                    }                            
                                 }
                             }
                             e.processOnServer = false;
                        } }";
                        }
                    }
                }
                else if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    //if (quotesinfo.lsttempparamsoid == null)
                    //{
                    //    quotesinfo.lsttempparamsoid = new List<Guid>();
                    //}
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.Load += Grid_Load;
                        gridlisteditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        gridlisteditor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                        gridlisteditor.Grid.Settings.ShowStatusBar = DevExpress.Web.GridViewStatusBarMode.Hidden;
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("Quotes", this);
                        //gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                        gridlisteditor.Grid.JSProperties["cpViewID"] = View.Id;
                        //gridlisteditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                        //  if (e.visibleIndex != '-1')
                        //  {
                        //    var viewid = s.cpViewID;
                        //    console.log(viewid);
                        //    s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                        //     if (s.IsRowSelectedOnPage(e.visibleIndex)) {     

                        //        RaiseXafCallback(globalCallbackControl, 'Quotes', viewid +'|Selected|' + Oidvalue , '', false);    
                        //     }else{
                        //        RaiseXafCallback(globalCallbackControl, 'Quotes', viewid +'|UNSelected|' + Oidvalue, '', false);    
                        //     }
                        //    }); 
                        //  }
                        //  else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                        //  {  
                        //    RaiseXafCallback(globalCallbackControl, 'Quotes', cpViewID +'|Selectall', '', false);     
                        //  }
                        //  else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                        //  {
                        //    RaiseXafCallback(globalCallbackControl, 'Quotes', cpViewID +'|UNSelectall', '', false);                        
                        //  }   
                        //  else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.pageRowCount)
                        //  {        
                        //    RaiseXafCallback(globalCallbackControl, 'Quotes', cpViewID +'|Selectall', '', false);     
                        //  }
                        //}";
                        gridlisteditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                        gridlisteditor.Grid.SettingsContextMenu.Enabled = true;
                        gridlisteditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        gridlisteditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                            {   
                                //sessionStorage.setItem('FocusedColumn', null); 
                                // var fieldName = e.cellInfo.column.fieldName;                       
                                //    sessionStorage.setItem('FocusedColumn', fieldName);      
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
                            }";
                        gridlisteditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function (s, e){
                        
                        window.setTimeout(function() {  
                        //s.UpdateEdit();   
                        if (s.batchEditApi.HasChanges(e.visibleIndex) && e.visibleIndex != '-1')
                        {
                        var viewid = s.cpViewID;
                        var precolumnname = sessionStorage.getItem('PrevFocusedColumn');  
                        var perviouscolumn = precolumnname;
                        //s.batchEditApi.ResetChanges(e.visibleIndex);
                        var objTAT = s.batchEditApi.GetCellValue(e.visibleIndex, 'TAT');
                        var objqty = s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty');
                        var objunitprice = s.batchEditApi.GetCellValue(e.visibleIndex, 'UnitPrice');
                        var objdiscount = s.batchEditApi.GetCellValue(e.visibleIndex, 'Discount');
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {    
                         RaiseXafCallback(globalCallbackControl, 'Quotes',  viewid +'|ValuesChange|' + Oidvalue +'|'+ objTAT+'|'+ objqty+'|'+ objunitprice+'|'+ objdiscount+'|'+  perviouscolumn, '', false); 
                      s.batchEditApi.ResetChanges(e.visibleIndex);                        
                }); 
                        }
                        }, 20); 
                        }";
                        if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                        {
                            gridlisteditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                                {      
                                if (s.IsRowSelectedOnPage(e.elementIndex))
                                {
                                var FocusedColumn = sessionStorage.getItem('CurrFocusedColumn');                                
                                var oid;
                                var text;
                                if(FocusedColumn=='Qty' || FocusedColumn=='Discount' || FocusedColumn=='TAT.TAT' || FocusedColumn=='TestDescription') //if (s.IsRowSelectedOnPage(e.elementIndex)) 
                                {
                                if(FocusedColumn.includes('.'))
                                { 
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;   
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        if(FocusedColumn=='TAT.TAT')
                                        {
                                            var pricecode;
                                            for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                            {                 
                                                if (s.IsRowSelectedOnPage(i))
                                                {
                                                    if(pricecode != null)
                                                    {
                                                        var pc =s.batchEditApi.GetCellValue(i,'PriceCode');
                                                        pricecode = pricecode + ';' + pc;
                                                    }
                                                    else
                                                    {
                                                        pricecode = s.batchEditApi.GetCellValue(i,'PriceCode');
                                                    }
                                                    //s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);                                                                                 
                                                }
                                            }
                                            RaiseXafCallback(globalCallbackControl, 'Quotes',  s.cpViewID + '|CopytoAllCellTATValuesChange|' + '|'+ FocusedColumn + '|'+ text + '|'+ pricecode , '', false);
                                        }
                                        else
                                        {
                                            for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                            {                 
                                                if (s.IsRowSelectedOnPage(i))
                                                {
                                                    //s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);                                                                                 
                                                }
                                            }
                                        }
                                     }        
                                 }
                                 else
                                 {                                                             
                                    var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);  
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        {   
                                            //s.batchEditApi.ResetChanges(i);    
                                           if (s.IsRowSelectedOnPage(i))
                                           {
                                                if(FocusedColumn=='Qty')
                                                {
                                                    var unprice = s.batchEditApi.GetCellValue(i,'TotalTierPrice');  
                                                    var discount = s.batchEditApi.GetCellValue(i,'Discount');  
                                                    if(unprice != null)
                                                    {
                                                        var tempprice = CopyValue * unprice; 
                                                        tempprice = Math.round(tempprice * 100) / 100; 
                                                        var FinalTempPrice = tempprice * discount / 100;
                                                        FinalTempPrice = tempprice - FinalTempPrice;
                                                        s.batchEditApi.SetCellValue(i,'FinalAmount',FinalTempPrice); 
                                                    }
                                                }   
                                                else if(FocusedColumn=='Discount')
                                                {
                                                    var unprice = s.batchEditApi.GetCellValue(i,'TotalTierPrice');  
                                                    var qtydata = s.batchEditApi.GetCellValue(i,'Qty');  
                                                    var discount = CopyValue;//s.batchEditApi.GetCellValue(i,'Discount'); 
                                                    if(unprice != null && qtydata > 0)
                                                    {
                                                        var tempprice = qtydata * unprice; 
                                                        tempprice = Math.round(tempprice * 100) / 100; 
                                                        var FinalTempPrice = tempprice * discount / 100;
                                                        FinalTempPrice = tempprice - FinalTempPrice;
                                                        s.batchEditApi.SetCellValue(i,'FinalAmount',FinalTempPrice); 
                                                    }
                                                }
                                                s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                           }
                                        }
                                        if(FocusedColumn=='Qty' || FocusedColumn=='Discount')
                                        {
                                            RaiseXafCallback(globalCallbackControl, 'Quotes',  s.cpViewID + '|CopytoAllCellValuesChange|' + '|'+ FocusedColumn , '', false); 
                                        }
                                    }                            
                                 }
                               }
                             e.processOnServer = false;
                        }}";
                        }
                    }
                }
                else if (View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_DetailView_Cancel_Expired" || View.Id == "CRMQuotes_DetailView_Reviewed" || View.Id == "CRMQuotes_DetailView_Submitted")
                {
                    foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible == true))
                    {
                        //ForeColor
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null && (item.Id == "ProjectID" || item.Id == "Title"))
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null && (item.Id == "QuotedDate" || item.Id == "ExpirationDate"))
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                        {
                            ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(FileDataPropertyEditor))
                        {
                            FileDataPropertyEditor propertyEditor = item as FileDataPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                        {
                            ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                        {
                            ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                }
                                else if (propertyEditor.DropDownEdit != null)
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                }
                                else
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                }
                            }
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null && (item.Id == "QuotedBy" || item.Id == "TAT" || item.Id == "Client" || item.Id == "Owner"))
                            {
                                if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                {
                                    propertyEditor.FindEdit.Editor.BackColor = Color.LightYellow;
                                }
                                else if (propertyEditor.DropDownEdit != null)
                                {
                                    propertyEditor.DropDownEdit.DropDown.BackColor = Color.LightYellow;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                            }
                        }
                        else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                        {
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null && (item.Id == "QuotedAmount"))
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDecimalPropertyEditor))
                        {
                            ASPxDecimalPropertyEditor propertyEditor = item as ASPxDecimalPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null && item.Id == "DetailedAmount")
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                                ////propertyEditor.Editor.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                            }
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        if (View.Id == "CRMQuotes_DetailView")
                        {
                            ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("ProjectID") as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.CollectionSource != null)
                            {
                                CRMQuotes objCRM = (CRMQuotes)View.CurrentObject;
                                if (objCRM != null && objCRM.Client != null)
                                {
                                    propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("[customername.Oid] = ? ", objCRM.Client.Oid);
                                }
                                else
                                {
                                    propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                                }
                            }
                        }
                        //backcolor


                        //if (item.GetType() == typeof(ASPxStringPropertyEditor) && (item.Id == "ProjectID" || item.Id == "Title"))
                        //{
                        //    ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                        //    if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        //    {
                        //        propertyEditor.Editor.BackColor = Color.LightYellow;
                        //    }
                        //}
                        //else if (item.GetType() == typeof(ASPxDateTimePropertyEditor) && (item.Id == "QuotedDate" || item.Id == "ExpirationDate"))
                        //{
                        //    ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                        //    if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        //    {
                        //        propertyEditor.Editor.BackColor = Color.LightYellow;
                        //    }
                        //}
                        //else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                        //{
                        //    ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                        //    if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        //    {
                        //        propertyEditor.Editor.BackColor = Color.LightYellow;
                        //    }
                        //}
                        //else if (item.GetType() == typeof(FileDataPropertyEditor))
                        //{
                        //    FileDataPropertyEditor propertyEditor = item as FileDataPropertyEditor;
                        //    if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        //    {
                        //        propertyEditor.Editor.BackColor = Color.LightYellow;
                        //    }
                        //}
                        //else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                        //{
                        //    ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
                        //    if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        //    {
                        //        propertyEditor.Editor.BackColor = Color.LightYellow;
                        //    }
                        //}
                        //else if (item.GetType() == typeof(ASPxLookupPropertyEditor) && (item.Id == "QuotedBy" || item.Id == "TAT" || item.Id == "Client" || item.Id == "Owner"))
                        //{
                        //    ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                        //    if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        //    {
                        //        if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                        //        {
                        //            propertyEditor.FindEdit.Editor.BackColor = Color.LightYellow;
                        //        }
                        //        else if (propertyEditor.DropDownEdit != null)
                        //        {
                        //            propertyEditor.DropDownEdit.DropDown.BackColor = Color.LightYellow;
                        //        }
                        //        else
                        //        {
                        //            propertyEditor.Editor.BackColor = Color.LightYellow;
                        //        }
                        //    }
                        //}
                        //else if (item.GetType() == typeof(ASPxIntPropertyEditor) && (item.Id == "QuotedAmount"))
                        //{
                        //    ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                        //    if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        //    {
                        //        propertyEditor.Editor.BackColor = Color.LightYellow;
                        //    }
                        //}
                        //else if (item.GetType() == typeof(ASPxDecimalPropertyEditor))
                        //{
                        //    ASPxDecimalPropertyEditor propertyEditor = item as ASPxDecimalPropertyEditor;
                        //    if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        //    {
                        //        propertyEditor.Editor.BackColor = Color.LightYellow;
                        //    }
                        //}
                    }
                }
                else if (View.Id == "GroupTestMethod_ListView_Quotes")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    }
                }
                else if (View.Id == "ItemChargePricing_ListView_Quotes_Popup")
                {
                    if (quotesinfo.lstinitialtempItemPricingpopup != null && quotesinfo.lstinitialtempItemPricingpopup.Count > 0)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = new NotOperator(new InOperator("Oid", quotesinfo.lstinitialtempItemPricingpopup.ToList()));
                    }

                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        if (quotesinfo.IsItemchargePricingpopupselectall == true)
                        {
                            gridlisteditor.Grid.JSProperties["cpEndCallbackHandlers"] = "selectall";
                        }
                        else
                        {
                            gridlisteditor.Grid.JSProperties["cpEndCallbackHandlers"] = null;
                        }
                        gridlisteditor.Grid.Settings.VerticalScrollableHeight = 300;
                        gridlisteditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("QuotesItemPrice", this);
                        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                        gridlisteditor.Grid.Load += Grid_Load;
                        gridlisteditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
                        var chkselect = s.cpEndCallbackHandlers;
                        s.batchEditApi.ResetChanges(e.visibleIndex);
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {     

                            RaiseXafCallback(globalCallbackControl, 'QuotesItemPrice', 'QuotesItemPrice|Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'QuotesItemPrice', 'QuotesItemPrice|UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {  
                     if(chkselect != 'selectall')
                       {
                       RaiseXafCallback(globalCallbackControl, 'QuotesItemPrice', 'QuotesItemPrice|Selectall', '', false);        
                        }
                        RaiseXafCallback(globalCallbackControl, 'QuotesItemPrice', 'QuotesItemPrice|Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'QuotesItemPrice', 'QuotesItemPrice|UNSelectall', '', false);                        
                      }   
                      //else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.pageRowCount)
                      //{        
                      //  RaiseXafCallback(globalCallbackControl, 'QuotesItemPrice', 'QuotesItemPrice|Selectall', '', false);     
                      //}
                    }";
                    }
                }
                else if (View.Id == "TurnAroundTime_ListView_Quotes")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                        gridlisteditor.Grid.Load += Grid_Load;
                        gridlisteditor.Grid.Settings.ShowStatusBar = DevExpress.Web.GridViewStatusBarMode.Hidden;
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("Quotes", this); //QuotesTATPopup
                        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                        //gridlisteditor.Grid.Load += Grid_Load;
                        gridlisteditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    }
                }
                else if (View.Id == "VisualMatrix_ListView_Quotes")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                        gridlisteditor.Grid.Load += Grid_Load;
                        gridlisteditor.Grid.Settings.ShowStatusBar = DevExpress.Web.GridViewStatusBarMode.Hidden;
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("Quotes", this); //QuotesTATPopup
                        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                        //gridlisteditor.Grid.Load += Grid_Load;
                        gridlisteditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    }
                }
                else if (View.Id == "CRMQuotes_ListView_SubmittedHistory")
                {
                    ListViewController controller = Frame.GetController<ListViewController>();
                    controller.EditAction.TargetObjectsCriteria = "[Status] = 'PendingReview'";
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'PendingReview' And [ExpirationDate] >= ?", DateTime.Today);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        if (gridListEditor.Grid.Columns["InlineEditCommandColumn"] != null)
                        {
                            gridListEditor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                        }
                    }
                }
                else if (View.Id == "CRMQuotes_ListView_Expired")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'Expired' Or [ExpirationDate] < ?", DateTime.Today); ;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Access and customize the target View control.
        }

        private void Grid_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    e.Items.Add("Copy To All Cell", "CopyToAllCell");

                    GridViewContextMenuItem Edititem = e.Items.FindByName("EditRow");
                    if (Edititem != null)
                        Edititem.Visible = false;
                    GridViewContextMenuItem item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";
                    e.Items.Remove(e.Items.FindByText("Edit"));
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
                if (View.Id == "AnalysisPricing_ListView_QuotesPopup")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //gridListEditor.Grid.CommandButtonInitialize += Grid_CommandButtonInitialize;
                    if (quotesinfo.lsttempAnalysisPricingpopup != null && quotesinfo.lsttempAnalysisPricingpopup.Count > 0)
                    {
                        foreach (AnalysisPricing objanalysisprice in quotesinfo.lsttempAnalysisPricingpopup.ToList())
                        {
                            gridListEditor.Grid.Selection.SelectRowByKey(objanalysisprice.Oid);
                        }
                    }
                }
                else if (View.Id == "ItemChargePricing_ListView_Quotes_Popup")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //gridListEditor.Grid.CommandButtonInitialize += Grid_CommandButtonInitialize;
                    if (quotesinfo.lsttempItemPricingpopup != null && quotesinfo.lsttempItemPricingpopup.Count > 0)
                    {
                        foreach (ItemChargePricing objitemprice in quotesinfo.lsttempItemPricingpopup.ToList())
                        {
                            gridListEditor.Grid.Selection.SelectRowByKey(objitemprice.Oid);
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

        private void GridView_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
        {
            try
            {
                if (View.Id == "AnalysisPricing_ListView_QuotesPopup")
                {
                    ASPxGridView gridView = sender as ASPxGridView;
                    if (gridView != null)
                    {
                        if (e.CommandCellType == GridViewTableCommandCellType.Data)
                        {
                            if (e.CommandColumn.Name == "SelectionCommandColumn")
                            {
                                string strpricecodeoid = gridView.GetRowValuesByKeyValue(e.KeyValue, "Oid").ToString();
                                if (quotesinfo.lstpricecodeempty.Contains(strpricecodeoid))
                                {
                                    //SRInfo.selectionhideGuid.Add(gridView.GetRowValuesByKeyValue(e.KeyValue, "Oid").ToString());
                                    ((System.Web.UI.WebControls.WebControl)e.Cell.Controls[0]).Visible = false;
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
        private void PopupControl_CustomizePopupWindowSize(object sender, CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "TestPriceSurcharge_ListView_Quotes_Popup")
                {
                    e.Width = 1320;
                    e.Height = 610;
                    e.Handled = true;
                }
                if (View != null && View.Id == "AnalysisPricing_ListView_QuotesPopup" || View.Id == "AnalysisPricing_DetailView_QuotePopup")
                {
                    e.Width = 1000;
                    e.Height = 700;
                    e.Handled = true;
                }
                //if (View != null && View.Id == "ItemChargePricing_ListView_Quotes_Popup")
                //{
                //    e.Width = 920;
                //    e.Height = 600;
                //    e.Handled = true;
                //}
                if (View != null && View.Id == "VisualMatrix_ListView_Quotes")
                {
                    e.Width = 920;
                    e.Height = 600;
                    e.Handled = true;
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
                if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    if (e.DataColumn.FieldName == "TAT.TAT")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'Quotes', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                    else if (e.DataColumn.FieldName == "Parameter")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'Quotes', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                    else if (e.DataColumn.FieldName == "SampleMatrix.VisualMatrixName")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'Quotes', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                    else
                    {
                        return;
                    }
                }
                else if (View.Id == "TurnAroundTime_ListView_Quotes")
                {
                    if (e.DataColumn.FieldName == "TAT")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'Quotes', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                }
                else if (View.Id == "VisualMatrix_ListView_Quotes")
                {
                    if (e.DataColumn.FieldName == "VisualMatrixName")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'Quotes', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridview = (ASPxGridView)sender as ASPxGridView;
                if (View.Id == "TurnAroundTime_ListView_Quotes")
                {
                    AnalysisPricing objanalysisprice = null;
                    if (Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    }
                    else
                    {
                        objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    }
                    //AnalysisPricing objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    if (objanalysisprice != null && objanalysisprice.TAT != null)
                    {
                        gridview.Selection.SelectRowByKey(objanalysisprice.TAT.Oid);
                    }
                }
                else if (View.Id == "AnalysisPricing_ListView_QuotesPopup")
                {
                    //gridview.CommandButtonInitialize += Grid_CommandButtonInitialize;
                    var selectionBoxColumn = gridview.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                    if (selectionBoxColumn != null)
                    {
                        selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
                    }
                    //if (quotesinfo.lsttempAnalysisPricingpopup != null && quotesinfo.lsttempAnalysisPricingpopup.Count > 0)
                    //{
                    //    foreach (AnalysisPricing objanalysisprice in quotesinfo.lsttempAnalysisPricingpopup.ToList())
                    //    {
                    //        gridview.Selection.SelectRowByKey(objanalysisprice.Oid);
                    //    }
                    //}
                    if (dtPriceCode != null && dtPriceCode.Rows.Count > 0)
                    {
                        dtPriceCode = new DataTable();
                        dtPriceCode.Columns.Add("SampleMatrix");
                        dtPriceCode.Columns.Add("PriceCode");
                        ListPropertyEditor lvanalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                        if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                        {
                            foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                            {
                                if (objanapricing.PriceCode != null)
                                {
                                    DataRow drNew = dtPriceCode.NewRow();
                                    drNew["SampleMatrix"] = objanapricing.SampleMatrix.VisualMatrixName;
                                    drNew["PriceCode"] = objanapricing.PriceCode;
                                    dtPriceCode.Rows.Add(drNew);
                                }

                            }

                            if (dtPriceCode != null && dtPriceCode.Rows.Count > 0)
                            {
                                foreach (DataRow drpricecode in dtPriceCode.Rows)
                                {
                                    foreach (AnalysisPricing objanalysisprice in ((ListView)View).CollectionSource.List.Cast<AnalysisPricing>().Where(i => i.PriceCode == drpricecode["PriceCode"].ToString() && i.SampleMatrix.VisualMatrixName == drpricecode["SampleMatrix"].ToString()))
                                    {
                                        if (!quotesinfo.lsttempAnalysisPricingpopup.Contains(objanalysisprice))
                                        {
                                            quotesinfo.lsttempAnalysisPricingpopup.Add(objanalysisprice);
                                            quotesinfo.lstInitialtempAnalysisPricingpopup.Add(objanalysisprice);
                                        }
                                        gridview.Selection.SelectRowByKey(objanalysisprice.Oid);
                                    }

                                }
                                //((ListView)(((DetailView)View).FindItem("AnalysisPricing") as DashboardViewItem).InnerView).Refresh();
                            }
                        }

                    }

                    //else if (quotesinfo.lsttempAnalysisPricingpopup != null && quotesinfo.lsttempAnalysisPricingpopup.Count == 0)
                    //{
                    //    gridview.Selection.UnselectAll();
                    //}
                    //List<Guid> lsttestpriceoid = new List<Guid>();
                    //List<string> lsttestpricecode = new List<string>();
                    //DashboardViewItem dvanalysisp
                    //rice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPrice") as DashboardViewItem;
                    //if (dvanalysisprice != null && dvanalysisprice.InnerView != null)
                    //{
                    //    foreach (AnalysisPricing objanapricing in ((ListView)dvanalysisprice.InnerView).CollectionSource.List)
                    //    {
                    //        if (objanapricing.PriceCode != null)
                    //        {
                    //            lsttestpricecode.Add(objanapricing.PriceCode);
                    //        }
                    //        {
                    //            //string strtestdetail = string.Empty;
                    //            //if (objanapricing.Matrix != null && objanapricing.Test != null && objanapricing.Method != null && objanapricing.Component != null && objanapricing.Priority != null)
                    //            //{
                    //            //    strtestdetail = objanapricing.Matrix.MatrixName + "|" + objanapricing.Test.TestName + "|" + objanapricing.Method.MethodNumber + "|" + objanapricing.Component.Components + "|" + objanapricing.Priority.Prioritys;
                    //            //}
                    //            //else
                    //            //if (objanapricing.Matrix != null && objanapricing.Test != null && objanapricing.IsGroup == true && objanapricing.Priority != null)
                    //            //{
                    //            //    strtestdetail = objanapricing.Matrix.MatrixName + "|" + objanapricing.Test.TestName + "|" + objanapricing.Priority.Prioritys;
                    //            //}
                    //            //if (!string.IsNullOrEmpty(strtestdetail))
                    //            //{
                    //            //    lsttestprice.Add(strtestdetail);
                    //            //}
                    //        }
                    //    }
                    //    if(lsttestpricecode != null && lsttestpricecode.Count > 0)
                    //    {
                    //        foreach(string strpricecode in lsttestpricecode.ToList())
                    //        {
                    //            foreach (AnalysisPricing objanalysisprice in ((ListView)View).CollectionSource.List.Cast<AnalysisPricing>().Where(i => i.PriceCode == strpricecode))
                    //            {
                    //                gridview.Selection.SelectRowByKey(objanalysisprice.Oid);
                    //            }
                    //        }                            
                    //    }
                    //}
                    //if (lsttestpriceoid.Count > 0)
                    //{
                    //    foreach (Guid objtpsOid in lsttestpriceoid.ToList())
                    //    {
                    //        AnalysisPricing objtstpricesur = View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", objtpsOid));
                    //        if (objtstpricesur != null)
                    //        {
                    //            gridview.Selection.SelectRowByKey(objtstpricesur.Oid);
                    //        }
                    //    }
                    //}
                }
                else if (View.Id == "Testparameter_LookupListView_Quotes" && (quotesinfo.lsttempparamsoid.Count > 0 || quotesinfo.strtempparamsstatus == "Allparams") && Isgridloaded == false)
                {
                    if (gridview != null)
                    {
                        if (quotesinfo.strtempparamsstatus == "Allparams")
                        {
                            quotesinfo.strtempparamsstatus = string.Empty;
                            gridview.Selection.SelectAll();
                        }
                        else
                        {
                            Isgridloaded = true;
                            List<Guid> lstparaoid = new List<Guid>();
                            foreach (Testparameter objtstpara in ((ListView)View).CollectionSource.List)
                            {
                                if (quotesinfo.lsttempparamsoid.Contains(objtstpara.Parameter.Oid))
                                {
                                    lstparaoid.Add(objtstpara.Oid);
                                }
                            }
                            for (int i = 0; i <= gridview.VisibleRowCount; i++)
                            {
                                var currentOid1 = gridview.GetRowValues(i, "Oid");
                                if (currentOid1 != null)
                                {
                                    if (lstparaoid.Contains(new Guid(currentOid1.ToString())))
                                    {
                                        gridview.Selection.SelectRow(i);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (View.Id == "ItemChargePricing_ListView_Quotes_Popup")
                {
                    //gridview.CommandButtonInitialize += Grid_CommandButtonInitialize;
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                    }
                    ////if (quotesinfo.lsttempItemPricingpopup != null && quotesinfo.lsttempItemPricingpopup.Count > 0)
                    ////{
                    ////    foreach (ItemChargePricing objitemprice in quotesinfo.lsttempItemPricingpopup.ToList())
                    ////    {
                    ////        gridview.Selection.SelectRowByKey(objitemprice.Oid);
                    ////    }
                    ////}
                    //else if (quotesinfo.lsttempItemPricingpopup != null && quotesinfo.lsttempItemPricingpopup.Count == 0)
                    //{
                    //    gridview.Selection.UnselectAll();
                    //}
                    //List<Guid> lstitempriceoid = new List<Guid>();
                    //DashboardViewItem dvitemprice = ((DetailView)Application.MainWindow.View).FindItem("DVItemChargePrice") as DashboardViewItem;
                    //if (dvitemprice != null && dvitemprice.InnerView != null)
                    //{
                    //    foreach (QuotesItemChargePrice objitempricing in ((ListView)dvitemprice.InnerView).CollectionSource.List)
                    //    {
                    //        if (objitempricing.ItemPrice != null)
                    //        {
                    //            lstitempriceoid.Add(objitempricing.ItemPrice.Oid);
                    //        }

                    //    }
                    //}
                    //if (lstitempriceoid.Count > 0)
                    //{
                    //    foreach (Guid objitempriceOid in lstitempriceoid.ToList())
                    //    {
                    //        ItemChargePricing objitemprice = ObjectSpace.FindObject<ItemChargePricing>(CriteriaOperator.Parse("[Oid] = ?", objitempriceOid));
                    //        if (objitemprice != null)
                    //        {
                    //            gridview.Selection.SelectRowByKey(objitemprice.Oid);
                    //        }
                    //    }
                    //}

                }
                else if (View.Id == "VisualMatrix_ListView_Quotes")
                {
                    AnalysisPricing objanalysisprice = null;
                    if (Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    }
                    else
                    {
                        objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    }
                    ASPxGridView grid = sender as ASPxGridView;
                    if (grid != null && grid.Columns.Count > 0)
                    {
                        foreach (WebColumnBase column in grid.VisibleColumns)
                        {
                            if (column.Name == "Edit" || column.Name == "SelectionCommandColumn")
                                column.Visible = false;
                        }
                    }
                    if (objanalysisprice != null && objanalysisprice.SampleMatrix != null)
                    {
                        quotesinfo.QuotePopupCrtAnalysispriceOid = objanalysisprice.Oid;
                        quotesinfo.QuotePopupVMOid = objanalysisprice.SampleMatrix.Oid;
                        gridview.Selection.SelectRowByKey(objanalysisprice.SampleMatrix.Oid);
                    }
                }
                else if (View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                    }
                }
                else if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.JSProperties["cpVisibleRowCount"] = gridlisteditor.Grid.VisibleRowCount;
                    }
                }
                quotesinfo.IsobjChangedproperty = false;
                quotesinfo.IsobjChangedpropertyinQuotes = false;
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
                // Unsubscribe from previously subscribed events and release other references and resources.
                if (View.Id == "CRMQuotes_ListView_pendingsubmission")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                }
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                if (View.Id == "CRMQuotes_ListView_SubmittedHistory")
                {
                    ListViewController controller = Frame.GetController<ListViewController>();
                    controller.EditAction.TargetObjectsCriteria = string.Empty;
                }
                if (View.Id == "Testparameter_LookupListView_Quotes")
                {
                    if (quotesinfo.lsttempparamsoid != null && quotesinfo.lsttempparamsoid.Count > 0)
                    {
                        quotesinfo.lsttempparamsoid = new List<Guid>();
                        quotesinfo.strtempparamsstatus = string.Empty;
                        Isgridloaded = false;
                    }
                }
                else if (View.Id == "CRMQuotes_DetailView")
                {
                    quotesinfo.popupcurtquote = null;
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated -= ChangeLayoutGroupCaptionViewController_ItemCreated;
                    quotesinfo.lvDetailedPrice = 0;
                    //quotesinfo.lvFinalPrice = 0;
                    //quotesinfo.lvdiscntpr = 0;
                    //quotesinfo.lvdiscntPrice = 0;
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    quotesinfo.IsobjChangedproperty = false;
                    quotesinfo.IsobjChangedpropertyinQuotes = false;

                    if (quotesinfo.lsttempAnalysisPricing != null)
                    {
                        quotesinfo.lsttempAnalysisPricing.Clear();
                    }
                    if (quotesinfo.lsttempItemPricing != null)
                    {
                        quotesinfo.lsttempItemPricing.Clear();
                    }
                    Frame.GetController<ModificationsController>().SaveAction.Executing -= SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing -= SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executing -= SaveAction_Executing;
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                }
                else if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    if (quotesinfo.lsttempparamsoid != null && quotesinfo.lsttempparamsoid.Count > 0)
                    {
                        quotesinfo.lsttempparamsoid = new List<Guid>();
                    }
                    if (View.Id == "CRMQuotes_AnalysisPricing_ListView")
                    {
                        Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                    }
                }
                else if (View.Id == "AnalysisPricing_ListView_QuotesPopup")
                {
                    quotesinfo.IsAnalycialPricingpopupselectall = false;
                    if (quotesinfo.lsttempAnalysisPricingpopup != null && quotesinfo.lsttempAnalysisPricingpopup.Count > 0)
                    {
                        quotesinfo.lsttempAnalysisPricingpopup.Clear();
                    }
                    View.ControlsCreated -= View_ControlsCreated;
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = true;
                }
                else if (View.Id == "ItemChargePricing_ListView_Quotes_Popup")
                {
                    quotesinfo.IsItemchargePricingpopupselectall = false;
                    if (quotesinfo.lsttempItemPricingpopup != null && quotesinfo.lsttempItemPricingpopup.Count > 0)
                    {
                        quotesinfo.lsttempItemPricingpopup.Clear();
                    }
                }
                else if (View.Id == "AnalysisPricing_DetailView_QuotePopup")
                {
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                }
                if (View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_ListView_pendingsubmission")
                {
                    //Frame.GetController<ModificationsController>().SaveAction.Executed -= SaveAction_Executed;
                    //Frame.GetController<ModificationsController>().SaveAndCloseAction.Executed -= SaveAction_Executed;
                    //Frame.GetController<ModificationsController>().SaveAndNewAction.Executed -= SaveAction_Executed;

                }
                else if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView" || View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView")
                {
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                }
                if (View != null && (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView" || View.Id == "CRMQuotes_ItemChargePricing_ListView" || View.Id == "QuotesItemChargePrice_ListView_Quotes" || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView"))
                {
                    ObjectSpace.Committed -= ObjectSpace_Committed;
                    quotesinfo.IsItemchargePricingpopupselectall = false;
                }
                if (Application != null && Application.MainWindow != null && Application.MainWindow.View.Id == "CRMQuotes_DetailView")
                {
                    ObjectSpace.Committing -= ObjectSpace_Committing;
                }
                else if (View != null && View.Id == "CRMQuotes_ListView_pendingsubmission")
                {
                    QuotesDateFilter.SelectedItemChanged -= QuotesDateFilter_SelectedItemChanged;
                }
                else if (View.Id == "CRMProspects_DetailView")
                {
                    Frame.GetController<DialogController>().Accepting -= QuoteViewController_Accepting;
                    //Frame.GetController<DialogController>().SaveOnAccept = false;
                }
                else if (View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = true;
                }
                base.OnDeactivated();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void QuoteSubmit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "CRMQuotes_DetailView" || View.Id == "CRMQuotes_ListView_pendingsubmission")
                {
                    bool IsSubmitted = false;
                    if (View is DetailView)
                    {
                       //IList<DefaultSetting> FDdefsetting = View.ObjectSpace.GetObjects<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'QuotesReview' And Not IsNullOrEmpty([NavigationItemName])"));
                       // DefaultSetting FD3defsetting = FDdefsetting.Where(a => a.NavigationItemNameID == "QuotesReview").FirstOrDefault();
                        DefaultSetting objDefault = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='QuotesReview'"));

                        if (objDefault.Select == true)
                        {
                            CRMQuotes objquote = (CRMQuotes)View.CurrentObject;
                            if (objquote.IsProspect == true && objquote.ProspectClient == null)
                            {
                                Application.ShowViewStrategy.ShowMessage("Client must not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            else if (objquote.IsProspect == false && objquote.Client == null)
                            {
                                Application.ShowViewStrategy.ShowMessage("Client must not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            ObjectSpace.CommitChanges();
                            if (objquote != null && objquote.AnalysisPricing.Count > 0)
                            {
                                objquote.Status = CRMQuotes.QuoteStatus.PendingReview;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                IsSubmitted = true;
                            }
                            else if (objquote.AnalysisPricing.Count == 0)
                            {
                                Application.ShowViewStrategy.ShowMessage("Please add price code.", InformationType.Warning, 3000, InformationPosition.Top);
                                return;
                            } 
                        }
                        else
                        {
                        CRMQuotes objquote = (CRMQuotes)View.CurrentObject;
                            if (objquote!=null)
                            {
                                objquote.Status = CRMQuotes.QuoteStatus.QuoteSubmited;
                        if (objquote.IsProspect == true && objquote.ProspectClient == null)
                        {
                            Application.ShowViewStrategy.ShowMessage("Client must not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        else if (objquote.IsProspect == false && objquote.Client == null)
                        {
                            Application.ShowViewStrategy.ShowMessage("Client must not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        ObjectSpace.CommitChanges();
                        if (objquote != null && objquote.AnalysisPricing.Count > 0)
                        {
                            objquote.Status = CRMQuotes.QuoteStatus.PendingReview;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            IsSubmitted = true;
                        }
                        else if (objquote.AnalysisPricing.Count == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage("Please add pricecode.", InformationType.Warning, 3000, InformationPosition.Top);
                            return;
                        }
                    }

                        }
                    }
                   
                    else if (View is ListView)
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            IList<DefaultSetting> FDdefsetting = View.ObjectSpace.GetObjects<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Quotes' And Not IsNullOrEmpty([NavigationItemName])"));
                            DefaultSetting FD3defsetting = FDdefsetting.Where(a => a.NavigationItemNameID == "QuotesReview").FirstOrDefault();
                            if (FD3defsetting.Select == true)
                            {
                                foreach (CRMQuotes objquote in View.SelectedObjects)
                                {
                                    if (objquote.IsProspect == true && objquote.ProspectClient == null)
                                    {
                                        Application.ShowViewStrategy.ShowMessage("Client must not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }
                                    else if (objquote.IsProspect == false && objquote.Client == null)
                                    {
                                        Application.ShowViewStrategy.ShowMessage("Client must not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }
                                    if (objquote.AnalysisPricing.Count > 0)
                                    {
                                        objquote.Status = CRMQuotes.QuoteStatus.PendingReview;
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                        IsSubmitted = true;
                                    }
                                    else if (objquote.AnalysisPricing.Count == 0)
                                    {
                                        Application.ShowViewStrategy.ShowMessage("Please add pricecode.", InformationType.Warning, 3000, InformationPosition.Top);
                                        return;
                                    }
                                } 
                            }
                            else
                            {
                            foreach (CRMQuotes objquote in View.SelectedObjects)
                            {
                                    objquote.Status = CRMQuotes.QuoteStatus.QuoteSubmited;
                                if (objquote.IsProspect == true && objquote.ProspectClient == null)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Client must not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                                else if (objquote.IsProspect == false && objquote.Client == null)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Client must not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                                if (objquote.AnalysisPricing.Count > 0)
                                {
                                    objquote.Status = CRMQuotes.QuoteStatus.PendingReview;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    IsSubmitted = true;
                                }
                                else if (objquote.AnalysisPricing.Count == 0)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Please add pricecode.", InformationType.Warning, 3000, InformationPosition.Top);
                                    return;
                                }
                            }

                            }
                        }
                        else if (View.SelectedObjects.Count == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    ObjectSpace.CommitChanges();
                    ObjectSpace.Refresh();
                    if (IsSubmitted == true)
                    {
                        //QuotesNavgCount();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void QuoteReview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "CRMQuotes_ListView_PendingReview" && View.SelectedObjects.Count > 0)
                {
                    bool IscustomerAvailable = false;
                    foreach (CRMQuotes objquote in View.SelectedObjects)
                    {
                        if (objquote.ProspectClient != null && objquote.IsProspect == true)
                        {
                            CRMProspects ObjPC = ObjectSpace.FindObject<CRMProspects>(CriteriaOperator.Parse("[Oid] = ?", objquote.ProspectClient.Oid));
                            ObjPC.Status = ProspectsStatus.Won;
                            if (ObjPC != null)
                            {
                                Customer customer = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[CustomerName] = ?", ObjPC.Name));
                                if (customer != null)
                                {
                                    IscustomerAvailable = true;
                                    Application.ShowViewStrategy.ShowMessage("Customer name must be unique", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    break;
                                }
                                else
                                {
                                    objquote.Status = CRMQuotes.QuoteStatus.Reviewed;
                                    Customer objNewC = ObjectSpace.CreateObject<Customer>();
                                    objNewC.Address = ObjPC.Address;
                                    objNewC.Address1 = ObjPC.Street1;
                                    //objNewC.CustomerName = ObjPC.Name;
                                    objNewC.CustomerName = ObjPC.Prospects;
                                    objNewC.ClientCode = ObjPC.ClientCode;
                                    objNewC.OfficePhone = ObjPC.OfficePhone;
                                    objNewC.Account = ObjPC.Account;
                                    objNewC.WebSite = ObjPC.WebSite;
                                    //objNewC.Type = ObjPC.Type;
                                    //objNewC.Industry = ObjPC.Industry;
                                    objNewC.Fax = ObjPC.Fax;
                                    objNewC.PrimaryContact = ObjPC.PrimaryContact;
                                    objNewC.Email = ObjPC.Email;
                                    objNewC.Country = ObjPC.Country;
                                    objNewC.State = ObjPC.State;
                                    objNewC.PostCode = ObjPC.Zip;
                                    objNewC.Zip = ObjPC.Zip;
                                    objNewC.City = ObjPC.City;
                                    //objNewC.SICCode = ObjPC.SICCode;
                                    //objNewC.Zone = ObjPC.Zone;
                                    //objNewC.Area = ObjPC.Area;
                                    //objNewC.Country = ObjPC.Country;
                                    //objNewC.SiteMap = ObjPC.SiteMap;
                                    //objNewC.ProducerCode = ObjPC.ProducerCode;
                                    //objNewC.LicenseNumber = ObjPC.LicenseNumber;
                                    //objNewC.METRCCode = ObjPC.METRCCode;
                                    //objNewC.TypeofUse = ObjPC.TypeofUse;
                                    //objNewC.ClientClass = ObjPC.ClientClass;
                                    //if (objNote.Employee.Count > 0)
                                    //{
                                    //    foreach (Employee objUsers in objNote.Employee)
                                    //    {
                                    //        objActivity.Employee.Add(objUsers);
                                    //    }
                                    //}

                                    foreach (Contact objcontact in ObjPC.PrimaryContacts)
                                    {
                                        objcontact.Customer = objNewC;
                                    }
                                    foreach (Notes objNote in ObjPC.Notes)
                                    {
                                        objNote.Customer = objNewC;
                                    }
                                    objquote.Client = objNewC;

                                }
                            }
                        }
                        else
                        {
                            objquote.Status = CRMQuotes.QuoteStatus.Reviewed;
                        }
                    }
                    if (!IscustomerAvailable)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ReviewSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        ObjectSpace.CommitChanges();
                    }

                    //IObjectSpace os = Application.CreateObjectSpace();
                    //foreach(CRMQuotes objquote in View.SelectedObjects)
                    //{
                    //    if(objquote.ProspectClient != null && objquote.ProspectClient.Name != null)
                    //    {
                    //        ProspectClient objprsclient = os.FindObject<ProspectClient>(CriteriaOperator.Parse("[CustomerName] = ?", objquote.ProspectClient.Name));
                    //        if(objprsclient != null)
                    //        {
                    //            os.Delete(objprsclient);
                    //            os.CommitChanges();
                    //        }
                    //    }
                    //}
                }
                else if (View.SelectedObjects.Count == 0)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
                if (View is DetailView)
                {
                    bool IscustomerAvailable = false;
                    CRMQuotes objquote = (CRMQuotes)View.CurrentObject;
                    if (objquote != null && objquote.AnalysisPricing.Count > 0)
                    {
                        if (objquote.ProspectClient != null && objquote.IsProspect == true)
                        {
                            CRMProspects ObjPC = ObjectSpace.FindObject<CRMProspects>(CriteriaOperator.Parse("[Oid] = ?", objquote.ProspectClient.Oid));
                            ObjPC.Status = ProspectsStatus.Won;
                            if (ObjPC != null)
                            {
                                Customer customer = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[CustomerName] = ?", ObjPC.Name));
                                if (customer != null)
                                {
                                    IscustomerAvailable = true;
                                    Application.ShowViewStrategy.ShowMessage("Customer name must be unique", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                }
                                else
                                {
                                    objquote.Status = CRMQuotes.QuoteStatus.Reviewed;
                                    Customer objNewC = ObjectSpace.CreateObject<Customer>();
                                    objNewC.Address = ObjPC.Address;
                                    objNewC.Address1 = ObjPC.Street1;
                                    //objNewC.CustomerName = ObjPC.Name;
                                    objNewC.CustomerName = ObjPC.Prospects;
                                    objNewC.ClientCode = ObjPC.ClientCode;
                                    objNewC.OfficePhone = ObjPC.OfficePhone;
                                    objNewC.Account = ObjPC.Account;
                                    objNewC.WebSite = ObjPC.WebSite;
                                    //objNewC.Type = ObjPC.Type;
                                    //objNewC.Industry = ObjPC.Industry;
                                    objNewC.Fax = ObjPC.Fax;
                                    objNewC.PrimaryContact = ObjPC.PrimaryContact;
                                    objNewC.Email = ObjPC.Email;
                                    objNewC.Country = ObjPC.Country;
                                    objNewC.State = ObjPC.State;
                                    objNewC.PostCode = ObjPC.Zip;
                                    objNewC.Zip = ObjPC.Zip;
                                    objNewC.City = ObjPC.City;
                                    //objNewC.SICCode = ObjPC.SICCode;
                                    //objNewC.Zone = ObjPC.Zone;
                                    //objNewC.Area = ObjPC.Area;
                                    //objNewC.Country = ObjPC.Country;
                                    //objNewC.SiteMap = ObjPC.SiteMap;
                                    //objNewC.ProducerCode = ObjPC.ProducerCode;
                                    //objNewC.LicenseNumber = ObjPC.LicenseNumber;
                                    //objNewC.METRCCode = ObjPC.METRCCode;
                                    //objNewC.TypeofUse = ObjPC.TypeofUse;
                                    //objNewC.ClientClass = ObjPC.ClientClass;
                                    foreach (Contact objcontact in ObjPC.PrimaryContacts)
                                    {
                                        objcontact.Customer = objNewC;
                                    }
                                    foreach (Notes objNote in ObjPC.Notes)
                                    {
                                        objNote.Customer = objNewC;
                                    }
                                    objquote.Client = objNewC;
                                }
                            }
                        }
                        else
                        {
                            objquote.Status = CRMQuotes.QuoteStatus.Reviewed;
                        }
                        if (!IscustomerAvailable)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ReviewSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            ObjectSpace.CommitChanges();
                        }
                        View.Close();
                    }
                }
                ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void QuoteRollback_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "CRMQuotes_ListView_PendingReview" || View.Id == "CRMQuotes_DetailView_Submitted" || View.Id == "CRMQuotes_ListView_ReviewHistory" || View.Id == "CRMQuotes_ListView_SubmittedQuotes_History")
                {
                    if (View is ListView)
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            if (/*View.Id == "CRMQuotes_ListView_PendingReview" &&*/ View.SelectedObjects.Count == 1)
                            {
                                if (View.Id == "CRMQuotes_ListView_ReviewHistory")
                                {
                                    CRMQuotes objQuote = (CRMQuotes)View.CurrentObject;
                                    if (objQuote != null)
                                    {
                                        Samplecheckin objSamplecheckin = View.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[QuoteID]=?", objQuote.Oid));
                                        if (objSamplecheckin != null)
                                        {
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "quotealreadyused"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                            return;
                                        }
                                    }
                                }
                                IObjectSpace os = Application.CreateObjectSpace(typeof(DummyClass));
                                DummyClass objcrtdummy = os.CreateObject<DummyClass>();
                                DetailView dv = Application.CreateDetailView(os, "DummyClass_DetailView_Reasons", false, objcrtdummy);
                                dv.ViewEditMode = ViewEditMode.Edit;
                                dv.Caption = "Rollback";
                                ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                                showViewParameters.CreatedView = dv;
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.SaveOnAccept = false;
                                dc.CloseOnCurrentObjectProcessing = false;
                                dc.Accepting += Dcreason_Accepting;
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }

                    }
                    if (View is DetailView)
                    {
                        IObjectSpace os = Application.CreateObjectSpace(typeof(DummyClass));
                        DummyClass objcrtdummy = os.CreateObject<DummyClass>();
                        DetailView dv = Application.CreateDetailView(os, "DummyClass_DetailView_Reasons", false, objcrtdummy);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.Caption = "Rollback";
                        ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                        showViewParameters.CreatedView = dv;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Dcreason_Accepting;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                }
                else if (View != null && View.SelectedObjects.Count == 0)
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

        private void QuoteReactive_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View is DetailView)
                {
                    CRMQuotes objquote = (CRMQuotes)View.CurrentObject;
                    if (objquote != null && objquote.ExpirationDate.Date >= DateTime.Now.Date)
                    {
                        IObjectSpace os = Application.CreateObjectSpace(typeof(DummyClass));
                        DummyClass objcrtdummy = os.CreateObject<DummyClass>();
                        DetailView dv = Application.CreateDetailView(os, "DummyClass_DetailView_Reasons", false, objcrtdummy);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.Caption = "Reactive Reason";
                        ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                        showViewParameters.CreatedView = dv;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Dcreason_Accepting;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    else if (objquote.ExpirationDate < DateTime.Now)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "QuoteExpDateupdate"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }
                else if (View is ListView)
                {
                    if (View.Id == "CRMQuotes_ListView_Cancel" && View.SelectedObjects.Count > 0)
                    {
                        IObjectSpace os = Application.CreateObjectSpace(typeof(DummyClass));
                        DummyClass objcrtdummy = os.CreateObject<DummyClass>();
                        DetailView dv = Application.CreateDetailView(os, "DummyClass_DetailView_Reasons", false, objcrtdummy);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.Caption = "Reactive Reason";
                        ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                        showViewParameters.CreatedView = dv;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Dcreason_Accepting;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    else if (View.Id == "CRMQuotes_ListView_Expired" && View.SelectedObjects.Count > 0)
                    {
                        bool isexpireupdate = true;
                        foreach (CRMQuotes objquote in View.SelectedObjects)
                        {
                            if (objquote.ExpirationDate < DateTime.Today)
                            {
                                isexpireupdate = false;
                                break;
                            }
                        }
                        if (isexpireupdate == true)
                        {
                            IObjectSpace os = Application.CreateObjectSpace(typeof(DummyClass));
                            DummyClass objcrtdummy = os.CreateObject<DummyClass>();
                            DetailView dv = Application.CreateDetailView(os, "DummyClass_DetailView_Reasons", false, objcrtdummy);
                            dv.ViewEditMode = ViewEditMode.Edit;
                            dv.Caption = "Reactive Reason";
                            ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                            showViewParameters.CreatedView = dv;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += Dcreason_Accepting;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "QuoteExpDateupdate"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else if (View != null && View.SelectedObjects.Count == 0)
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

        private void QuotesHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspa = Application.CreateObjectSpace();
                if (View.Id == "CRMQuotes_ListView_pendingsubmission" || View.Id== "CRMQuotes_ListView_PendingReview")
                {
                    CollectionSource cs = new CollectionSource(objspa, typeof(CRMQuotes));
                    //IList<DefaultSetting> FDdefsetting = View.ObjectSpace.GetObjects<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'QuotesReview' And Not IsNullOrEmpty([NavigationItemName])"));
                    //DefaultSetting FD3defsetting = FDdefsetting.Where(a => a.NavigationItemNameID == "QuotesReview").FirstOrDefault();
                    DefaultSetting objDefault = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='QuotesReview'"));


                if (View.Id == "CRMQuotes_ListView_pendingsubmission")
                {
                       
                            if (objDefault.Select==true)
                            {
                    cs.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'PendingReview' And [ExpirationDate] >= ?", DateTime.Today);
                    ListView lstsubmithistory = Application.CreateListView("CRMQuotes_ListView_SubmittedHistory", cs, true);
                    Frame.SetView(lstsubmithistory);
                }
                            else
                            {
                                cs.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'QuoteSubmited' And [ExpirationDate] >= ?", DateTime.Today );
                                ListView lstsubmithistory = Application.CreateListView("CRMQuotes_ListView_SubmittedQuotes_History", cs, true);
                                Frame.SetView(lstsubmithistory);
                            }
                    }
                else if (View.Id == "CRMQuotes_ListView_PendingReview")
                {
                        //CollectionSource css = new CollectionSource(objspa, typeof(CRMQuotes));
                    cs.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'Reviewed' And [ExpirationDate] >= ?", DateTime.Today);
                    ListView lstreviewhistory = Application.CreateListView("CRMQuotes_ListView_ReviewHistory", cs, true);
                    Frame.SetView(lstreviewhistory);
                }
                   
                }
              
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void QuotesDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "CRMQuotes_ListView_pendingsubmission")
                {
                    string strSelectedItem = ((DevExpress.ExpressApp.Actions.SingleChoiceAction)sender).SelectedItem.Id.ToString();
                    if (strSelectedItem == "1M")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(QuotedDate, Now()) <= 1");
                    }
                    else if (strSelectedItem == "3M")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(QuotedDate, Now()) <= 3");
                    }
                    else if (strSelectedItem == "6M")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(QuotedDate, Now()) <= 6");
                    }
                    else if (strSelectedItem == "1Y")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(QuotedDate, Now()) <= 1");
                    }
                    else if (strSelectedItem == "2Y")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(QuotedDate, Now()) <= 2");
                    }
                    else if (strSelectedItem == "5Y")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(QuotedDate, Now()) <= 5");
                    }
                    else if (strSelectedItem == "All")
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[Status] = 0");
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
                    if (quotesinfo.lsttempAnalysisPricing == null)
                    {
                        quotesinfo.lsttempAnalysisPricing = new List<AnalysisPricing>();
                    }
                    if (quotesinfo.lsttempItemPricing == null)
                    {
                        quotesinfo.lsttempItemPricing = new List<QuotesItemChargePrice>();
                    }
                    if (quotesinfo.lsttempItemPricingpopup == null)
                    {
                        quotesinfo.lsttempItemPricingpopup = new List<ItemChargePricing>();
                    }
                    if (quotesinfo.lsttempAnalysisPricingpopup == null)
                    {
                        quotesinfo.lsttempAnalysisPricingpopup = new List<AnalysisPricing>();
                    }
                    string[] splparm = parameter.Split('|');
                    if (splparm[0] == "Quotesparameter")
                    {
                        ASPxGridListEditor gridlsteditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridlsteditor != null && gridlsteditor.Grid != null)
                        {
                            //Isgridloaded = false;
                            if (splparm[1] == "Selectall")
                            {
                                foreach (Testparameter objtstpara in ((ListView)View).CollectionSource.List)
                                {
                                    if (!quotesinfo.lsttempparamsoid.Contains(objtstpara.Parameter.Oid))
                                    {
                                        quotesinfo.lsttempparamsoid.Add(objtstpara.Parameter.Oid);
                                        gridlsteditor.Grid.Selection.SelectAll();
                                    }
                                }
                            }
                            else if (splparm[1] == "UNSelectall")
                            {
                                quotesinfo.lsttempparamsoid.Clear();
                                gridlsteditor.Grid.Selection.SelectAll();
                            }
                            else if (splparm[1] == "Selected" || splparm[1] == "UNSelected")
                            {
                                if (!string.IsNullOrEmpty(splparm[1]))
                                {
                                    if (splparm[1] == "Selected")
                                    {
                                        Testparameter objtstpara = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", new Guid(splparm[2])));
                                        if (objtstpara != null && !quotesinfo.lsttempparamsoid.Contains(objtstpara.Parameter.Oid))
                                        {
                                            quotesinfo.lsttempparamsoid.Add(objtstpara.Parameter.Oid);
                                            gridlsteditor.Grid.Selection.SelectRowByKey(objtstpara.Oid);
                                        }
                                    }
                                    else if (splparm[1] == "UNSelected")
                                    {
                                        Testparameter objtstpara = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid] = ?", new Guid(splparm[2])));
                                        if (objtstpara != null && quotesinfo.lsttempparamsoid.Contains(objtstpara.Parameter.Oid))
                                        {
                                            quotesinfo.lsttempparamsoid.Remove(objtstpara.Parameter.Oid);
                                            gridlsteditor.Grid.Selection.UnselectRowByKey(objtstpara.Oid);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    if (splparm[0] == "Quotestest")
                    {
                        //Isgridloaded = false;
                        if (splparm[1] == "Selectall")
                        {
                            foreach (TestMethod objtstpara in ((ListView)View).CollectionSource.List)
                            {
                                if (!quotesinfo.lsttempparamsoid.Contains(objtstpara.Oid))
                                {
                                    quotesinfo.lsttempparamsoid.Add(objtstpara.Oid);
                                }
                            }
                        }
                        else if (splparm[1] == "UNSelectall")
                        {
                            quotesinfo.lsttempparamsoid.Clear();
                        }
                        else if (splparm[1] == "Selected" || splparm[1] == "UNSelected")
                        {
                            if (!string.IsNullOrEmpty(splparm[1]))
                            {
                                if (splparm[1] == "Selected")
                                {
                                    TestMethod objtstpara = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", new Guid(splparm[2])));
                                    if (objtstpara != null && !quotesinfo.lsttempparamsoid.Contains(objtstpara.Oid))
                                    {
                                        quotesinfo.lsttempparamsoid.Add(objtstpara.Oid);
                                    }
                                }
                                else if (splparm[1] == "UNSelected")
                                {
                                    TestMethod objtstpara = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", new Guid(splparm[2])));
                                    if (objtstpara != null && quotesinfo.lsttempparamsoid.Contains(objtstpara.Oid))
                                    {
                                        quotesinfo.lsttempparamsoid.Remove(objtstpara.Oid);
                                    }
                                }
                            }
                        }
                    }
                    else
                    if (splparm[0] == "AnalysisPricing_ListView_Quotes" || splparm[0] == "CRMQuotes_AnalysisPricing_ListView")
                    {
                        if (splparm[1] == "ValuesChange")
                        {
                            int intqty = 0;
                            decimal unitamt = 0;
                            decimal discntamt = 0;
                            Guid rowoid = Guid.Empty;
                            Guid tatoid = Guid.Empty;
                            if (splparm[2] != null && !string.IsNullOrEmpty(splparm[2].ToString()))
                            {
                                rowoid = new Guid(splparm[2]);
                            }
                            //if(splparm[3] != null && !string.IsNullOrEmpty(splparm[3].ToString()))
                            //{
                            //    tatoid = new Guid(splparm[3]);
                            //}                            
                            intqty = Convert.ToInt32(splparm[4]);
                            unitamt = Convert.ToDecimal(splparm[5]);
                            discntamt = Convert.ToDecimal(splparm[6]);
                            strfocusedcolumn = splparm[7].ToString();
                            if (rowoid != Guid.Empty /*&& tatoid != Guid.Empty*/)
                            {
                                decimal surchargevalue = 0;
                                decimal surchargeprice = 0;
                                decimal schargeamt = 0;
                                decimal pcdisamt = 0;
                                string tat = string.Empty;
                                AnalysisPricing objap = null;
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    objap = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                }
                                else
                                {
                                    objap = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                }
                                if (objap != null && objap.TAT != null && objap.Test != null && objap.Component != null)
                                {
                                    TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components, objap.TAT.TAT.Trim()));
                                    if (objtps != null)
                                    {
                                        if (intqty <= 0)
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Qty value must be greater than 0.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                            intqty = 1;
                                            objap.Qty = Convert.ToUInt32(intqty);
                                        }
                                        objap.PriceCode = objtps.PriceCode;
                                        if (objtps.Surcharge != null)
                                        {
                                            objap.NPSurcharge = (int)objtps.Surcharge;
                                        }
                                        if (Frame is NestedFrame)
                                        {
                                            NestedFrame nestedFrame = (NestedFrame)Frame;
                                            objap.Priority = nestedFrame.View.ObjectSpace.GetObject(objtps.Priority);
                                        }
                                        else
                                        {
                                            objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                        }
                                        //objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                        if (objtps.Priority.IsRegular == true)
                                        {
                                            surchargevalue = 0;
                                            if (objtps.SurchargePrice != null)
                                            {
                                                surchargeprice = (decimal)objtps.SurchargePrice;
                                            }
                                        }
                                        else
                                        {
                                            if (objtps.Surcharge != null)
                                            {
                                                surchargevalue = (int)objtps.Surcharge;
                                            }
                                            if (objtps.SurchargePrice != null)
                                            {
                                                surchargeprice = (decimal)objtps.SurchargePrice;
                                            }
                                        }
                                    }
                                    //else
                                    //{
                                    //    Application.ShowViewStrategy.ShowMessage("Qty value must be greater than 0.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    //    intqty = 1;
                                    //    objap.Qty = Convert.ToUInt32(intqty);
                                    //    objap.NPSurcharge = 0;
                                    //}
                                }
                                else if (objap != null && objap.TAT != null && objap.Test != null && objap.IsGroup == true)
                                {
                                    TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And Contains([TAT], ?) And [Priority.Oid] = ? And [IsGroup] = 'true'", objap.Matrix.MatrixName, objap.Test.TestName, objap.TAT.TAT.Trim(), objap.Priority.Oid));
                                    if (objtps != null)
                                    {
                                        objap.PriceCode = objtps.PriceCode;
                                        if (objtps.Surcharge != null)
                                        {
                                            objap.NPSurcharge = (int)objtps.Surcharge;
                                        }
                                        if (Frame is NestedFrame)
                                        {
                                            NestedFrame nestedframe = (NestedFrame)Frame;
                                            objap.Priority = nestedframe.View.ObjectSpace.GetObject(objtps.Priority);
                                        }
                                        else
                                        {
                                            objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                        }
                                        //objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                        if (objtps.Surcharge != null)
                                        {
                                            surchargevalue = (int)objtps.Surcharge;
                                        }
                                        if (objtps.SurchargePrice != null)
                                        {
                                            surchargeprice = (decimal)objtps.SurchargePrice;
                                        }
                                        //Application.ShowViewStrategy.ShowMessage("Qty value must be greater than 0.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                        //intqty = 1;
                                        //objap.Qty = Convert.ToUInt32(intqty);
                                    }
                                    else
                                    {
                                        objap.NPSurcharge = 0;
                                        Application.ShowViewStrategy.ShowMessage("Qty value must be greater than 0.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                        intqty = 1;
                                        objap.Qty = Convert.ToUInt32(intqty);
                                    }
                                }
                                if (surchargevalue > 0)
                                {
                                    if (objap.TAT != null && objap.Test != null && objap.Component != null)
                                    {
                                        ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                        if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                        {
                                            ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                                            if (objconpricetier != null)
                                            {
                                                objap.TierNo = objconpricetier.TierNo;
                                                objap.From = objconpricetier.From;
                                                objap.To = objconpricetier.To;
                                                objap.TierPrice = objconpricetier.TierPrice;
                                                if (strfocusedcolumn == "Qty")
                                                {
                                                    unitamt = objconpricetier.TierPrice;
                                                }
                                                schargeamt = unitamt * (surchargevalue / 100);
                                                objap.TotalTierPrice = schargeamt;
                                                //objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                if (discntamt > 0)
                                                {
                                                    pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                    objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                                else if (discntamt < 0)
                                                {
                                                    pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                    objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                                else
                                                {
                                                    objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }

                                            }
                                            else
                                            {
                                                List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                                                foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                                                {
                                                    objap.TierNo = objconprice.TierNo;
                                                    objap.From = objconprice.From;
                                                    objap.To = objconprice.To;
                                                    objap.TierPrice = objconprice.TierPrice;
                                                    if (strfocusedcolumn == "Qty")
                                                    {
                                                        unitamt = objconprice.TierPrice;
                                                    }
                                                    schargeamt = unitamt * (surchargevalue / 100);
                                                    objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                    if (discntamt > 0)
                                                    {
                                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    else if (discntamt < 0)
                                                    {
                                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    else
                                                    {
                                                        objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //schargeamt = unitamt * (surchargevalue / 100);
                                            schargeamt = surchargeprice;
                                            objap.TotalTierPrice = schargeamt;
                                            if (discntamt > 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else if (discntamt < 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else
                                            {
                                                objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                        }
                                    }
                                    else if (objap.TAT != null && objap.Test != null && objap.IsGroup == true)
                                    {
                                        //schargeamt = unitamt * (surchargevalue / 100);
                                        schargeamt = surchargeprice;
                                        objap.TotalTierPrice = schargeamt;
                                        if (discntamt > 0)
                                        {
                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else if (discntamt < 0)
                                        {
                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else
                                        {
                                            objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                    }
                                }
                                else
                                {
                                    if (objap.TAT != null && objap.Test != null && objap.Component != null)
                                    {
                                        ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                        if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                        {
                                            ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                                            if (objconpricetier != null)
                                            {
                                                objap.TierNo = objconpricetier.TierNo;
                                                objap.From = objconpricetier.From;
                                                objap.To = objconpricetier.To;
                                                objap.TierPrice = objconpricetier.TierPrice;
                                                if (strfocusedcolumn == "Qty")
                                                {
                                                    unitamt = objconpricetier.TierPrice;
                                                }
                                                schargeamt = unitamt * (surchargevalue / 100);
                                                objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                //unitamt = objconpricetier.TierPrice;
                                                //schargeamt = unitamt * (surchargevalue / 100);
                                                //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                                if (discntamt > 0)
                                                {
                                                    pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                    objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                                else if (discntamt < 0)
                                                {
                                                    pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                    objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                                else
                                                {
                                                    objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                            }
                                            else
                                            {
                                                List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                                                foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                                                {
                                                    objap.TierNo = objconprice.TierNo;
                                                    objap.From = objconprice.From;
                                                    objap.To = objconprice.To;
                                                    objap.TierPrice = objconprice.TierPrice;
                                                    if (strfocusedcolumn == "Qty")
                                                    {
                                                        unitamt = objconprice.TierPrice;
                                                    }
                                                    schargeamt = unitamt * (surchargevalue / 100);
                                                    objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                    //unitamt = objconpricetier.TierPrice;
                                                    //schargeamt = unitamt * (surchargevalue / 100);
                                                    //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                                    if (discntamt > 0)
                                                    {
                                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    else if (discntamt < 0)
                                                    {
                                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    else
                                                    {
                                                        objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //schargeamt = unitamt * (surchargevalue / 100);
                                            schargeamt = surchargeprice;
                                            objap.TotalTierPrice = schargeamt;
                                            if (discntamt > 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else if (discntamt < 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else
                                            {
                                                objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }

                                        }
                                    }
                                    else if (objap.TAT != null && objap.Test != null && objap.IsGroup == true)
                                    {
                                        //schargeamt = unitamt * (surchargevalue / 100);
                                        schargeamt = surchargeprice;
                                        objap.TotalTierPrice = schargeamt;
                                        if (discntamt > 0)
                                        {
                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                            objap.TotalTierPrice = unitamt;
                                        }
                                        else if (discntamt < 0)
                                        {
                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                            objap.TotalTierPrice = unitamt;
                                        }
                                        else
                                        {
                                            objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                            objap.UnitPrice = unitamt;
                                            objap.TotalTierPrice = unitamt;
                                        }
                                    }
                                }
                                if (splparm[7] == "UnitPrice")
                                {
                                    if (objap.UnitPrice != objap.NPUnitPrice)
                                    {
                                        decimal Disc = objap.NPUnitPrice - objap.UnitPrice;
                                        objap.Discount = Convert.ToInt32(100 * ((objap.NPUnitPrice - objap.UnitPrice) / objap.NPUnitPrice));
                                        objap.TotalTierPrice = objap.UnitPrice;
                                        objap.FinalAmount = objap.TotalTierPrice * objap.Qty;
                                    }
                                }
                                else if (splparm[7] == "Discount")
                                {
                                    quotesinfo.IsTabDiscountChanged = true;
                                    objap.UnitPrice = objap.NPUnitPrice;
                                }
                                else if (splparm[7] == "Qty")
                                {
                                    quotesinfo.IsTabDiscountChanged = true;
                                }
                                if (objap.TAT != null)
                                {
                                    TurnAroundTime objtat = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[Oid] = ?", objap.TAT.Oid));
                                    if (objtat != null)
                                    {
                                        if (Frame is NestedFrame)
                                        {
                                            NestedFrame nestedFrame = (NestedFrame)Frame;
                                            objap.TAT = nestedFrame.View.ObjectSpace.GetObject(objtat);
                                        }
                                        else
                                        {
                                            objap.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                                        }
                                        //objap.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                                    }
                                }

                                ((ListView)View).Refresh();
                                decimal finalamt = 0;
                                decimal totalprice = 0;
                                decimal disamt = 0;
                                decimal dispr = 0;
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    CompositeView cv = nestedFrame.ViewItem.View;
                                    ListPropertyEditor lvanalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                    if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                                    {
                                        foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                        {
                                            finalamt = finalamt + objanapricing.FinalAmount;
                                            totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                                        }
                                    }
                                    ListPropertyEditor lvitemprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                    if (lvitemprice != null && lvitemprice.ListView != null)
                                    {
                                        foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                        {
                                            finalamt = finalamt + objitempricing.FinalAmount;
                                            totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                                        }
                                    }
                                    if (finalamt < 0)
                                    {
                                        objap.Discount = 0;
                                        objap.FinalAmount = 0;
                                        finalamt = 0;
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    }
                                    CRMQuotes crtquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                                    if (crtquotes != null)
                                    {
                                        if (finalamt >= 0 && totalprice >= 0)
                                        {
                                            disamt = totalprice - finalamt;

                                            if (disamt != 0)
                                            {
                                                dispr = ((disamt) / totalprice) * 100;
                                            }
                                            //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                            //disamt = finalamt - totalprice;
                                            quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                            crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Math.Round(dispr, 2);
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                            crtquotes.IsGobalDiscount = true;
                                        }
                                        else
                                        {
                                            disamt = totalprice - finalamt;
                                            //disamt = finalamt - totalprice;
                                            if (disamt != 0)
                                            {
                                                dispr = ((disamt) / totalprice) * 100;
                                            }
                                            //dispr = ((totalprice) / totalprice) * 100;
                                            //disamt = finalamt - totalprice;
                                            quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                            crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Math.Round(dispr, 2);
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                            crtquotes.IsGobalDiscount = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ListPropertyEditor lvanalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                    if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                                    {
                                        foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                        {
                                            finalamt = finalamt + objanapricing.FinalAmount;
                                            totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                                        }
                                    }
                                    ListPropertyEditor lvitemprice = ((DetailView)Application.MainWindow.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                    if (lvitemprice != null && lvitemprice.ListView != null)
                                    {
                                        foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                        {
                                            finalamt = finalamt + objitempricing.FinalAmount;
                                            totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                                        }
                                    }
                                    if (finalamt < 0)
                                    {
                                        objap.Discount = 0;
                                        objap.FinalAmount = 0;
                                        finalamt = 0;
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    }
                                    CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                                    if (crtquotes != null)
                                    {
                                        if (finalamt >= 0 && totalprice >= 0)
                                        {
                                            disamt = totalprice - finalamt;

                                            if (disamt != 0)
                                            {
                                                dispr = ((disamt) / totalprice) * 100;
                                            }
                                            //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                            //disamt = finalamt - totalprice;
                                            quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                            crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Math.Round(dispr, 2);
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                            crtquotes.IsGobalDiscount = true;
                                        }
                                        else
                                        {
                                            disamt = totalprice - finalamt;
                                            //disamt = finalamt - totalprice;
                                            if (disamt != 0)
                                            {
                                                dispr = ((disamt) / totalprice) * 100;
                                            }
                                            //dispr = ((totalprice) / totalprice) * 100;
                                            //disamt = finalamt - totalprice;
                                            quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                            crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Math.Round(dispr, 2);
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                            crtquotes.IsGobalDiscount = true;
                                        }
                                    }
                                }
                            }
                        }
                        if (splparm[1] == "Selectall")
                        {
                            foreach (AnalysisPricing objap in ((ListView)View).CollectionSource.List)
                            {
                                if (!quotesinfo.lsttempAnalysisPricing.Contains(objap))
                                {
                                    quotesinfo.lsttempAnalysisPricing.Add(objap);
                                }
                            }
                        }
                        else if (splparm[1] == "UNSelectall")
                        {
                            quotesinfo.lsttempAnalysisPricing.Clear();
                        }
                        else if (splparm[1] == "Selected" || splparm[1] == "UNSelected")
                        {
                            if (!string.IsNullOrEmpty(splparm[1]))
                            {
                                if (splparm[1] == "Selected")
                                {
                                    AnalysisPricing objap = ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(splparm[2])));
                                    if (objap != null && !quotesinfo.lsttempAnalysisPricing.Contains(objap))
                                    {
                                        quotesinfo.lsttempAnalysisPricing.Add(objap);
                                    }
                                }
                                else if (splparm[1] == "UNSelected")
                                {
                                    AnalysisPricing objap = ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(splparm[2])));
                                    if (objap != null && quotesinfo.lsttempAnalysisPricing.Contains(objap))
                                    {
                                        quotesinfo.lsttempAnalysisPricing.Remove(objap);
                                    }
                                }
                            }
                        }
                        if (splparm[1] == "CopytoAllCellValuesChange")
                        {
                            quotesinfo.IsTabDiscountChanged = true;
                            decimal finalamt = 0;
                            decimal totalprice = 0;
                            decimal disamt = 0;
                            decimal dispr = 0;
                            if (Frame is NestedFrame)
                            {
                                NestedFrame nestedFrame = (NestedFrame)Frame;
                                ListPropertyEditor lvanalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                                {
                                    foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                    {
                                        finalamt = finalamt + objanapricing.FinalAmount;
                                        totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                                    }
                                }
                                ListPropertyEditor lvitemprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                if (lvitemprice != null && lvitemprice.ListView != null)
                                {
                                    foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                    {
                                        finalamt = finalamt + objitempricing.FinalAmount;
                                        totalprice = totalprice + (objitempricing.Amount/* * objitempricing.Qty*/);
                                    }
                                }
                                CRMQuotes crtquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                                if (crtquotes != null)
                                {
                                    if (finalamt != 0 && totalprice != 0)
                                    {
                                        //disamt = finalamt - totalprice;
                                        disamt = totalprice - finalamt;
                                        if (disamt != 0)
                                        {
                                            dispr = ((disamt) / totalprice) * 100;
                                        }
                                        //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                        //disamt = finalamt - totalprice;
                                        quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                        crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                        crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                        crtquotes.Discount = Math.Round(dispr, 2);
                                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                        crtquotes.IsGobalDiscount = true;
                                    }
                                    else if (finalamt == 0 && totalprice == 0)
                                    {
                                        crtquotes.DetailedAmount = 0;
                                        crtquotes.TotalAmount = 0;
                                        crtquotes.Discount = 0;
                                        crtquotes.DiscountAmount = 0;
                                        crtquotes.QuotedAmount = 0;
                                        crtquotes.IsGobalDiscount = true;
                                    }
                                }
                            }
                            else
                            {
                                ListPropertyEditor lvanalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                                {
                                    foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                    {
                                        finalamt = finalamt + objanapricing.FinalAmount;
                                        totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                                    }
                                }
                                ListPropertyEditor lvitemprice = ((DetailView)Application.MainWindow.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                if (lvitemprice != null && lvitemprice.ListView != null)
                                {
                                    foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                    {
                                        finalamt = finalamt + objitempricing.FinalAmount;
                                        totalprice = totalprice + (objitempricing.Amount/* * objitempricing.Qty*/);
                                    }
                                }
                                CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                                if (crtquotes != null)
                                {
                                    if (finalamt != 0 && totalprice != 0)
                                    {
                                        //disamt = finalamt - totalprice;
                                        disamt = totalprice - finalamt;
                                        if (disamt != 0)
                                        {
                                            dispr = ((disamt) / totalprice) * 100;
                                        }
                                        //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                        //disamt = finalamt - totalprice;
                                        quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                        crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                        crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                        crtquotes.Discount = Math.Round(dispr, 2);
                                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                        crtquotes.IsGobalDiscount = true;
                                    }
                                    else if (finalamt == 0 && totalprice == 0)
                                    {
                                        crtquotes.DetailedAmount = 0;
                                        crtquotes.TotalAmount = 0;
                                        crtquotes.Discount = 0;
                                        crtquotes.DiscountAmount = 0;
                                        crtquotes.QuotedAmount = 0;
                                        crtquotes.IsGobalDiscount = true;
                                    }
                                }

                            }
                        }
                        if (splparm[1] == "CopytoAllCellTATValuesChange")
                        {
                            quotesinfo.IsTabDiscountChanged = true;
                            string strtat = splparm[4];
                            string[] stranapc = splparm[5].Split(';');
                            TurnAroundTime turnAroundTime = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[TAT] = ?", strtat));
                            if (turnAroundTime != null)
                            {
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    CRMQuotes cRMQuotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                                    if (cRMQuotes != null && cRMQuotes.AnalysisPricing.Count > 0)
                                    {
                                        bool IsNotTATavl = false;
                                        string strIsNotTATavlTest = string.Empty;
                                        foreach (string strpricecode in stranapc)
                                        {
                                            decimal surchargevalue = 0;
                                            decimal schargeamt = 0;
                                            decimal surchargeprice = 0;
                                            decimal pcdisamt = 0;
                                            string tat = string.Empty;
                                            uint intqty = 0;
                                            decimal unitamt = 0;
                                            decimal discntamt = 0;
                                            AnalysisPricing objap = cRMQuotes.AnalysisPricing.Where(i => i.PriceCode == strpricecode).Select(i => i).FirstOrDefault();
                                            if (objap != null)
                                            {
                                                TestPriceSurcharge objtpsurprice = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components, strtat.Trim()));
                                                if (objtpsurprice != null)
                                                {
                                                    intqty = objap.Qty;
                                                    unitamt = objap.UnitPrice;
                                                    discntamt = objap.Discount;
                                                    objap.TAT = turnAroundTime;
                                                    objap.PriceCode = objtpsurprice.PriceCode;
                                                    if (objtpsurprice.Surcharge != null)
                                                    {
                                                        objap.NPSurcharge = (int)objtpsurprice.Surcharge;
                                                    }
                                                    objap.Priority = nestedFrame.View.ObjectSpace.GetObject(objtpsurprice.Priority);
                                                    if (objtpsurprice.Priority.IsRegular == true)
                                                    {
                                                        surchargevalue = 0;
                                                        if (objtpsurprice.SurchargePrice != null)
                                                        {
                                                            surchargeprice = (decimal)objtpsurprice.SurchargePrice;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (objtpsurprice.Surcharge != null)
                                                        {
                                                            surchargevalue = (int)objtpsurprice.Surcharge;
                                                        }
                                                        if (objtpsurprice.SurchargePrice != null)
                                                        {
                                                            surchargeprice = (decimal)objtpsurprice.SurchargePrice;
                                                        }
                                                    }
                                                    if (surchargevalue > 0)
                                                    {
                                                        if (objap.IsGroup == false)
                                                        {
                                                            ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                                            if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                                            {
                                                                ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                                                                if (objconpricetier != null)
                                                                {
                                                                    objap.TierNo = objconpricetier.TierNo;
                                                                    objap.From = objconpricetier.From;
                                                                    objap.To = objconpricetier.To;
                                                                    objap.TierPrice = objconpricetier.TierPrice;
                                                                    if (strfocusedcolumn == "Qty")
                                                                    {
                                                                        unitamt = objconpricetier.TierPrice;
                                                                    }
                                                                    schargeamt = unitamt * (surchargevalue / 100);
                                                                    objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                                    //unitamt = objconpricetier.TierPrice;
                                                                    //schargeamt = unitamt * (surchargevalue / 100);
                                                                    //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                                                    if (discntamt > 0)
                                                                    {
                                                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                        objap.UnitPrice = unitamt;
                                                                    }
                                                                    else if (discntamt < 0)
                                                                    {
                                                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                        objap.UnitPrice = unitamt;
                                                                    }
                                                                    else
                                                                    {
                                                                        objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                                        objap.UnitPrice = unitamt;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                                                                    foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                                                                    {
                                                                        objap.TierNo = objconpricetier.TierNo;
                                                                        objap.From = objconpricetier.From;
                                                                        objap.To = objconpricetier.To;
                                                                        objap.TierPrice = objconpricetier.TierPrice;
                                                                        if (strfocusedcolumn == "Qty")
                                                                        {
                                                                            unitamt = objconpricetier.TierPrice;
                                                                        }
                                                                        schargeamt = unitamt * (surchargevalue / 100);
                                                                        objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                                        //unitamt = objconpricetier.TierPrice;
                                                                        //schargeamt = unitamt * (surchargevalue / 100);
                                                                        //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                                                        if (discntamt > 0)
                                                                        {
                                                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                            objap.UnitPrice = unitamt;
                                                                        }
                                                                        else if (discntamt < 0)
                                                                        {
                                                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                            objap.UnitPrice = unitamt;
                                                                        }
                                                                        else
                                                                        {
                                                                            objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                                            objap.UnitPrice = unitamt;
                                                                        }
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //schargeamt = unitamt * (surchargevalue / 100);
                                                                schargeamt = surchargeprice;
                                                                objap.TotalTierPrice = schargeamt;
                                                                if (discntamt > 0)
                                                                {
                                                                    pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                    objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                    objap.UnitPrice = unitamt;
                                                                }
                                                                else if (discntamt < 0)
                                                                {
                                                                    pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                    objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                    objap.UnitPrice = unitamt;
                                                                }
                                                                else
                                                                {
                                                                    objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                                    objap.UnitPrice = unitamt;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //schargeamt = unitamt * (surchargevalue / 100);
                                                            schargeamt = surchargeprice;
                                                            objap.TotalTierPrice = schargeamt;
                                                            if (discntamt > 0)
                                                            {
                                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                objap.UnitPrice = unitamt;
                                                            }
                                                            else if (discntamt < 0)
                                                            {
                                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                objap.UnitPrice = unitamt;
                                                            }
                                                            else
                                                            {
                                                                objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                                objap.UnitPrice = unitamt;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (objap.IsGroup == false)
                                                        {
                                                            ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                                            if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                                            {
                                                                ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                                                                if (objconpricetier != null)
                                                                {
                                                                    objap.TierNo = objconpricetier.TierNo;
                                                                    objap.From = objconpricetier.From;
                                                                    objap.To = objconpricetier.To;
                                                                    objap.TierPrice = objconpricetier.TierPrice;
                                                                    //objap.TotalTierPrice = objconpricetier.TierPrice /** intqty*/;
                                                                    //unitamt = objconpricetier.TierPrice;
                                                                    if (strfocusedcolumn == "Qty")
                                                                    {
                                                                        unitamt = objconpricetier.TierPrice;
                                                                    }
                                                                    //schargeamt = unitamt * (surchargevalue / 100);
                                                                    objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                                    if (discntamt > 0)
                                                                    {
                                                                        pcdisamt = (unitamt) * (discntamt / 100);
                                                                        objap.FinalAmount = ((unitamt) - (pcdisamt)) * intqty;
                                                                        objap.UnitPrice = unitamt;
                                                                    }
                                                                    else if (discntamt < 0)
                                                                    {
                                                                        pcdisamt = (unitamt) * (discntamt / 100);
                                                                        objap.FinalAmount = ((unitamt) - (pcdisamt)) * intqty;
                                                                        objap.UnitPrice = unitamt;
                                                                    }
                                                                    else
                                                                    {
                                                                        objap.FinalAmount = (unitamt) * intqty;
                                                                        objap.UnitPrice = unitamt;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                                                                    foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                                                                    {
                                                                        objap.TierNo = objconprice.TierNo;
                                                                        objap.From = objconprice.From;
                                                                        objap.To = objconprice.To;
                                                                        objap.TierPrice = objconprice.TierPrice;
                                                                        //unitamt = objconpricetier.TierPrice;
                                                                        //schargeamt = unitamt * (surchargevalue / 100);
                                                                        //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                                                        if (strfocusedcolumn == "Qty")
                                                                        {
                                                                            unitamt = objconprice.TierPrice;
                                                                        }
                                                                        schargeamt = unitamt * (surchargevalue / 100);
                                                                        objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                                        if (discntamt > 0)
                                                                        {
                                                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                            objap.UnitPrice = unitamt;
                                                                        }
                                                                        else if (discntamt < 0)
                                                                        {
                                                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                            objap.UnitPrice = unitamt;
                                                                        }
                                                                        else
                                                                        {
                                                                            objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                                            objap.UnitPrice = unitamt;
                                                                        }
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //schargeamt = unitamt * (surchargevalue / 100);
                                                                schargeamt = surchargeprice;
                                                                //objap.TotalTierPrice = schargeamt + unitamt;
                                                                objap.TotalTierPrice = schargeamt;
                                                                if (discntamt > 0)
                                                                {
                                                                    pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                    objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                    objap.UnitPrice = unitamt;
                                                                }
                                                                else if (discntamt < 0)
                                                                {
                                                                    pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                    objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                    objap.UnitPrice = unitamt;
                                                                }
                                                                else
                                                                {
                                                                    objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                                    objap.UnitPrice = unitamt;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //schargeamt = unitamt * (surchargevalue / 100);
                                                            schargeamt = surchargeprice;
                                                            objap.TotalTierPrice = schargeamt;
                                                            if (discntamt > 0)
                                                            {
                                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                objap.UnitPrice = unitamt;
                                                            }
                                                            else if (discntamt < 0)
                                                            {
                                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                                objap.UnitPrice = unitamt;
                                                            }
                                                            else
                                                            {
                                                                objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                                objap.UnitPrice = unitamt;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    IsNotTATavl = true;
                                                    if (string.IsNullOrEmpty(strIsNotTATavlTest))
                                                    {
                                                        strIsNotTATavlTest = objap.Test.TestName + ";" + objap.Test.MethodName.MethodNumber;
                                                    }
                                                    else
                                                    {
                                                        strIsNotTATavlTest = strIsNotTATavlTest + ", " + objap.Test.TestName + ";" + objap.Test.MethodName.MethodNumber;
                                                    }
                                                }
                                            }
                                        }
                                        if (IsNotTATavl)
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Copied TAT value not available on selected test " + strIsNotTATavlTest + ".", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                        }
                                        decimal finalamt = 0;
                                        decimal totalprice = 0;
                                        decimal disamt = 0;
                                        decimal dispr = 0;
                                        ListPropertyEditor lstAnalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                        ListPropertyEditor lstitemprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                        if (lstitemprice != null)
                                        {
                                            if (lstitemprice.ListView == null)
                                            {
                                                lstitemprice.CreateControl();
                                            }
                                            if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                            {
                                                finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.FinalAmount);
                                                totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.UnitPrice * i.Qty);
                                            }
                                        }
                                        if (lstAnalysisprice != null)
                                        {
                                            if (lstAnalysisprice.ListView == null)
                                            {
                                                lstAnalysisprice.CreateControl();
                                            }
                                            if (lstAnalysisprice.ListView.CollectionSource.GetCount() > 0)
                                            {
                                                finalamt = finalamt + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.FinalAmount);
                                                totalprice = totalprice + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.TotalTierPrice * i.Qty);
                                            }
                                        }
                                        if (Frame is NestedFrame)
                                        {
                                            //NestedFrame nestedFrame = (NestedFrame)Frame;
                                            CRMQuotes crtquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                                            if (crtquotes != null)
                                            {
                                                if (finalamt >= 0 && totalprice >= 0)
                                                {
                                                    //disamt = finalamt - totalprice;
                                                    disamt = totalprice - finalamt;
                                                    if (disamt != 0)
                                                    {
                                                        dispr = ((disamt) / totalprice) * 100;
                                                    }
                                                    quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                                    crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                                    crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                                    crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                                    crtquotes.Discount = Math.Round(dispr, 2);
                                                    crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                                }
                                                else
                                                {
                                                    dispr = ((totalprice) / totalprice) * 100;
                                                    //disamt = finalamt - totalprice;
                                                    disamt = totalprice - finalamt;
                                                    quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                                    crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                                    crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                                    crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                                    crtquotes.Discount = Math.Round(dispr, 2);
                                                    crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                                            if (crtquotes != null)
                                            {
                                                if (finalamt >= 0 && totalprice >= 0)
                                                {
                                                    //disamt = finalamt - totalprice;
                                                    disamt = totalprice - finalamt;
                                                    if (disamt != 0)
                                                    {
                                                        dispr = ((disamt) / totalprice) * 100;
                                                    }
                                                    quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                                    crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                                    crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                                    crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                                    crtquotes.Discount = Math.Round(dispr, 2);
                                                    crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                                }
                                                else
                                                {
                                                    dispr = ((totalprice) / totalprice) * 100;
                                                    //disamt = finalamt - totalprice;
                                                    disamt = totalprice - finalamt;
                                                    quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                                    crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                                    crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                                    crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                                    crtquotes.Discount = Math.Round(dispr, 2);
                                                    crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                                }
                                            }
                                        }
                                    }
                                }
                                //else
                                //{
                                //    CRMQuotes cRMQuotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                                //    if (cRMQuotes != null && cRMQuotes.AnalysisPricing.Count > 0)
                                //    {
                                //        foreach (string strpricecode in stranapc)
                                //        {
                                //            AnalysisPricing objap = cRMQuotes.AnalysisPricing.Where(i => i.PriceCode == strpricecode).Select(i => i).FirstOrDefault();
                                //            if (objap != null)
                                //            {
                                //                List<TestPriceSurcharge> lsttestprice = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Test.Oid] = ? And [Method.Oid]=? ", objap.Test.Oid, objap.Method.Oid)).ToList();
                                //                if (lsttestprice.Count > 0 && lsttestprice.FirstOrDefault(i => i.TAT.Contains(strtat)) != null)
                                //                {
                                //                    objap.PriceCode = strtat;
                                //                }
                                //            }
                                //        }
                                //    }
                                //}
                            }
                        }
                    }
                    else if (splparm[0] == "QuotesItemChargePrice_ListView_Quotes" || splparm[0] == "CRMQuotes_QuotesItemChargePrice_ListView")
                    {
                        //ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                        ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridlisteditor != null && gridlisteditor.Grid != null)
                        {
                            gridlisteditor.Grid.UpdateEdit();
                        }
                        if (gridlisteditor != null && gridlisteditor.Grid != null)
                        {
                            if (splparm[1] == "ValuesChange")
                            {
                                Guid rowoid = new Guid(splparm[2]);
                                if (rowoid != Guid.Empty)
                                {
                                    decimal surchargevalue = 0;
                                    decimal schargeamt = 0;
                                    decimal pcdisamt = 0;
                                    QuotesItemChargePrice objitemcharge = null;
                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        objitemcharge = nestedFrame.View.ObjectSpace.FindObject<QuotesItemChargePrice>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                    }
                                    else
                                    {
                                        objitemcharge = Application.MainWindow.View.ObjectSpace.FindObject<QuotesItemChargePrice>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                    }
                                    //QuotesItemChargePrice objitemcharge = Application.MainWindow.View.ObjectSpace.FindObject<QuotesItemChargePrice>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                    if (objitemcharge != null)
                                    {
                                        if (splparm[4] == "Qty" || splparm[4] == "FinalAmount" || splparm[4] == "Discount")
                                        {
                                            quotesinfo.IsTabDiscountChanged = true;
                                        }
                                        if (splparm[4] == "FinalAmount"/* || splparm[5] == "FinalAmount"*/)
                                        {
                                            quotesinfo.IsTabDiscountChanged = true;
                                            //if (splparm[4] == "Qty")
                                            //{
                                            //    quotesinfo.IsTabDiscountChanged = true;
                                            //    objitemcharge.FinalAmount = objitemcharge.UnitPrice * objitemcharge.Qty;
                                            //    objitemcharge.Amount = objitemcharge.NpUnitPrice * objitemcharge.Qty;
                                            //    objitemcharge.Discount = ((objitemcharge.Amount - objitemcharge.FinalAmount) / objitemcharge.Amount) * 100;
                                            //    objitemcharge.Discount = Math.Round(objitemcharge.Discount);
                                            //}
                                            //else
                                            {
                                                objitemcharge.Amount = objitemcharge.NpUnitPrice * objitemcharge.Qty;
                                                objitemcharge.Discount = ((objitemcharge.Amount - objitemcharge.FinalAmount) / objitemcharge.Amount) * 100;
                                                objitemcharge.Discount = Math.Round(objitemcharge.Discount);
                                            }
                                        }
                                        else if (splparm[4] == "Qty")
                                        {
                                            objitemcharge.Amount = objitemcharge.NpUnitPrice * objitemcharge.Qty;
                                            if (objitemcharge.Discount > 0)
                                            {
                                                pcdisamt = (objitemcharge.Amount) * (objitemcharge.Discount / 100);
                                                objitemcharge.FinalAmount = ((objitemcharge.Amount) - (pcdisamt));
                                            }
                                            else if (objitemcharge.Discount < 0)
                                            {
                                                pcdisamt = (objitemcharge.Amount) * (objitemcharge.Discount / 100);
                                                objitemcharge.FinalAmount = ((objitemcharge.Amount) - (pcdisamt));
                                            }
                                            else
                                            {
                                                objitemcharge.FinalAmount = (objitemcharge.Amount);
                                            }
                                            //objitemcharge.Discount = ((objitemcharge.Amount - objitemcharge.FinalAmount) / objitemcharge.Amount) * 100;
                                            //objitemcharge.Discount = Math.Round(objitemcharge.Discount);
                                        }
                                        else if (splparm[4] == "Discount")
                                        {
                                            objitemcharge.Amount = objitemcharge.NpUnitPrice * objitemcharge.Qty;
                                            if (objitemcharge.Discount > 0)
                                            {
                                                pcdisamt = (objitemcharge.Amount) * (objitemcharge.Discount / 100);
                                                objitemcharge.FinalAmount = ((objitemcharge.Amount) - (pcdisamt));
                                            }
                                            else if (objitemcharge.Discount < 0)
                                            {
                                                pcdisamt = (objitemcharge.Amount) * (objitemcharge.Discount / 100);
                                                objitemcharge.FinalAmount = ((objitemcharge.Amount) - (pcdisamt));
                                            }
                                            else
                                            {
                                                objitemcharge.FinalAmount = (objitemcharge.Amount);
                                            }
                                        }
                                    }
                                    ((ListView)View).Refresh();
                                    decimal finalamt = 0;
                                    decimal totalprice = 0;
                                    decimal disamt = 0;
                                    decimal dispr = 0;

                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        ListPropertyEditor lvanalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                        if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                                        {
                                            foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                            {
                                                finalamt = finalamt + objanapricing.FinalAmount;
                                                totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                                            }
                                        }
                                        ListPropertyEditor lvitemprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                        if (lvitemprice != null && lvitemprice.ListView != null)
                                        {
                                            foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                            {
                                                finalamt = finalamt + objitempricing.FinalAmount;
                                                totalprice = totalprice + (objitempricing.Amount/* * objitempricing.Qty*/);
                                            }
                                        }
                                        CRMQuotes crtquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                                        if (crtquotes != null)
                                        {
                                            if (finalamt != 0 && totalprice != 0)
                                            {
                                                //disamt = finalamt - totalprice;
                                                disamt = totalprice - finalamt;
                                                if (disamt != 0)
                                                {
                                                    dispr = ((disamt) / totalprice) * 100;
                                                }
                                                //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                                //disamt = finalamt - totalprice;
                                                quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                                crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                                crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                                crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                                crtquotes.Discount = Math.Round(dispr, 2);
                                                crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                                crtquotes.IsGobalDiscount = true;
                                            }
                                            else if (finalamt == 0 && totalprice == 0)
                                            {
                                                crtquotes.DetailedAmount = 0;
                                                crtquotes.TotalAmount = 0;
                                                crtquotes.Discount = 0;
                                                crtquotes.DiscountAmount = 0;
                                                crtquotes.QuotedAmount = 0;
                                                crtquotes.IsGobalDiscount = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ListPropertyEditor lvanalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                        if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                                        {
                                            foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                            {
                                                finalamt = finalamt + objanapricing.FinalAmount;
                                                totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                                            }
                                        }
                                        ListPropertyEditor lvitemprice = ((DetailView)Application.MainWindow.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                        if (lvitemprice != null && lvitemprice.ListView != null)
                                        {
                                            foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                            {
                                                finalamt = finalamt + objitempricing.FinalAmount;
                                                totalprice = totalprice + (objitempricing.Amount/* * objitempricing.Qty*/);
                                            }
                                        }
                                        CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                                        if (crtquotes != null)
                                        {
                                            if (finalamt != 0 && totalprice != 0)
                                            {
                                                //disamt = finalamt - totalprice;
                                                disamt = totalprice - finalamt;
                                                if (disamt != 0)
                                                {
                                                    dispr = ((disamt) / totalprice) * 100;
                                                }
                                                //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                                //disamt = finalamt - totalprice;
                                                quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                                crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                                crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                                crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                                crtquotes.Discount = Math.Round(dispr, 2);
                                                crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                                crtquotes.IsGobalDiscount = true;
                                            }
                                            else if (finalamt == 0 && totalprice == 0)
                                            {
                                                crtquotes.DetailedAmount = 0;
                                                crtquotes.TotalAmount = 0;
                                                crtquotes.Discount = 0;
                                                crtquotes.DiscountAmount = 0;
                                                crtquotes.QuotedAmount = 0;
                                                crtquotes.IsGobalDiscount = true;
                                            }
                                        }

                                    }
                                }
                            }
                            if (splparm[1] == "Selectall" && quotesinfo.IsItemchargePricingpopupselectall == false)
                            {
                                quotesinfo.IsItemchargePricingpopupselectall = true;
                                foreach (QuotesItemChargePrice objitemprice in ((ListView)View).CollectionSource.List)
                                {
                                    if (!quotesinfo.lsttempItemPricing.Contains(objitemprice))
                                    {
                                        quotesinfo.lsttempItemPricing.Add(objitemprice);
                                    }
                                }
                                gridlisteditor.Grid.Selection.SelectAll();
                            }
                            else if (splparm[1] == "UNSelectall")
                            {
                                quotesinfo.lsttempItemPricing.Clear();
                                gridlisteditor.Grid.Selection.UnselectAll();
                                quotesinfo.IsItemchargePricingpopupselectall = false;
                            }
                            else if (splparm[1] == "Selected" || splparm[1] == "UNSelected")
                            {
                                if (!string.IsNullOrEmpty(splparm[1]))
                                {
                                    if (splparm[1] == "Selected")
                                    {
                                        QuotesItemChargePrice objitemprice = ObjectSpace.FindObject<QuotesItemChargePrice>(CriteriaOperator.Parse("[Oid] = ?", new Guid(splparm[2])));
                                        if (objitemprice != null && !quotesinfo.lsttempItemPricing.Contains(objitemprice))
                                        {
                                            quotesinfo.lsttempItemPricing.Add(objitemprice);
                                            gridlisteditor.Grid.Selection.SelectRowByKey(objitemprice);
                                        }
                                        quotesinfo.IsItemchargePricingpopupselectall = false;
                                    }
                                    else if (splparm[1] == "UNSelected")
                                    {
                                        QuotesItemChargePrice objitemprice = ObjectSpace.FindObject<QuotesItemChargePrice>(CriteriaOperator.Parse("[Oid] = ?", new Guid(splparm[2])));
                                        if (objitemprice != null && quotesinfo.lsttempItemPricing.Contains(objitemprice))
                                        {
                                            quotesinfo.lsttempItemPricing.Remove(objitemprice);
                                            gridlisteditor.Grid.Selection.SelectRowByKey(objitemprice);
                                        }
                                        quotesinfo.IsItemchargePricingpopupselectall = false;
                                    }
                                }
                            }
                            if (splparm[1] == "CopytoAllCellValuesChange")
                            {
                                quotesinfo.IsTabDiscountChanged = true;
                                decimal finalamt = 0;
                                decimal totalprice = 0;
                                decimal disamt = 0;
                                decimal dispr = 0;
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    ListPropertyEditor lvanalysisprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                    if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                                    {
                                        foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                        {
                                            finalamt = finalamt + objanapricing.FinalAmount;
                                            totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                                        }
                                    }
                                    ListPropertyEditor lvitemprice = ((DetailView)nestedFrame.ViewItem.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                    if (lvitemprice != null && lvitemprice.ListView != null)
                                    {
                                        foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                        {
                                            finalamt = finalamt + objitempricing.FinalAmount;
                                            totalprice = totalprice + (objitempricing.Amount/* * objitempricing.Qty*/);
                                        }
                                    }
                                    CRMQuotes crtquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                                    if (crtquotes != null)
                                    {
                                        if (finalamt != 0 && totalprice != 0)
                                        {
                                            //disamt = finalamt - totalprice;
                                            disamt = totalprice - finalamt;
                                            if (disamt != 0)
                                            {
                                                dispr = ((disamt) / totalprice) * 100;
                                            }
                                            //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                            //disamt = finalamt - totalprice;
                                            quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                            crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Math.Round(dispr, 2);
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                        }
                                        else if (finalamt == 0 && totalprice == 0)
                                        {
                                            crtquotes.DetailedAmount = 0;
                                            crtquotes.TotalAmount = 0;
                                            crtquotes.Discount = 0;
                                            crtquotes.DiscountAmount = 0;
                                            crtquotes.QuotedAmount = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    ListPropertyEditor lvanalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                                    if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                                    {
                                        foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                        {
                                            finalamt = finalamt + objanapricing.FinalAmount;
                                            totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                                        }
                                    }
                                    ListPropertyEditor lvitemprice = ((DetailView)Application.MainWindow.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                    if (lvitemprice != null && lvitemprice.ListView != null)
                                    {
                                        foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                        {
                                            finalamt = finalamt + objitempricing.FinalAmount;
                                            totalprice = totalprice + (objitempricing.Amount/* * objitempricing.Qty*/);
                                        }
                                    }
                                    CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                                    if (crtquotes != null)
                                    {
                                        if (finalamt != 0 && totalprice != 0)
                                        {
                                            //disamt = finalamt - totalprice;
                                            disamt = totalprice - finalamt;
                                            if (disamt != 0)
                                            {
                                                dispr = ((disamt) / totalprice) * 100;
                                            }
                                            //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                            //disamt = finalamt - totalprice;
                                            quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                            crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Math.Round(dispr, 2);
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                            crtquotes.IsGobalDiscount = true;
                                        }
                                        else if (finalamt == 0 && totalprice == 0)
                                        {
                                            crtquotes.DetailedAmount = 0;
                                            crtquotes.TotalAmount = 0;
                                            crtquotes.Discount = 0;
                                            crtquotes.DiscountAmount = 0;
                                            crtquotes.QuotedAmount = 0;
                                            crtquotes.IsGobalDiscount = true;
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else if (splparm[0] == "Quotestestpopup")
                    {
                        if (View.Id == "AnalysisPricing_ListView_QuotesPopup")
                        {
                            ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (gridlisteditor != null && gridlisteditor.Grid != null)
                            {
                                if (splparm[1] == "Selectall" && quotesinfo.IsAnalycialPricingpopupselectall == false)
                                {
                                    quotesinfo.IsAnalycialPricingpopupselectall = true;
                                    foreach (AnalysisPricing objap in ((ListView)View).CollectionSource.List)
                                    {
                                        if (!quotesinfo.lsttempAnalysisPricingpopup.Contains(objap))
                                        {
                                            quotesinfo.lsttempAnalysisPricingpopup.Add(objap);

                                        }
                                    }
                                    gridlisteditor.Grid.Selection.SelectAll();
                                }
                                else if (splparm[1] == "UNSelectall")
                                {
                                    quotesinfo.IsAnalycialPricingpopupselectall = false;
                                    quotesinfo.lsttempAnalysisPricingpopup.Clear();
                                    gridlisteditor.Grid.Selection.UnselectAll();
                                }
                                else if (splparm[1] == "Selected" || splparm[1] == "UNSelected")
                                {
                                    if (!string.IsNullOrEmpty(splparm[1]))
                                    {
                                        if (splparm[1] == "Selected")
                                        {
                                            quotesinfo.IsAnalycialPricingpopupselectall = false;
                                            AnalysisPricing objap = View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(splparm[2])));
                                            if (objap != null && !quotesinfo.lsttempAnalysisPricingpopup.Contains(objap) && !string.IsNullOrEmpty(objap.PriceCode))
                                            {
                                                quotesinfo.lsttempAnalysisPricingpopup.Add(objap);
                                                gridlisteditor.Grid.Selection.SelectRowByKey(objap.Oid);
                                            }
                                            else if (objap != null && string.IsNullOrEmpty(objap.PriceCode))
                                            {
                                                quotesinfo.IsAnalycialPricingpopupselectall = true;
                                                gridlisteditor.Grid.Selection.UnselectRowByKey(objap.Oid);
                                                Application.ShowViewStrategy.ShowMessage("You are not allowed to select a test without Test Price Code.", InformationType.Warning, timer.Seconds, InformationPosition.Top);

                                            }
                                        }
                                        else if (splparm[1] == "UNSelected")
                                        {
                                            quotesinfo.IsAnalycialPricingpopupselectall = false;
                                            AnalysisPricing objap = View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(splparm[2])));
                                            if (objap != null && quotesinfo.lsttempAnalysisPricingpopup.Contains(objap))
                                            {
                                                quotesinfo.lsttempAnalysisPricingpopup.Remove(objap);
                                                gridlisteditor.Grid.Selection.UnselectRowByKey(objap.Oid);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (splparm[0] == "TAT.TAT")
                    {
                        List<string> lsttatinfo = new List<string>();
                        List<string> lsttatoid = new List<string>();
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(splparm[1]), "Oid");
                            AnalysisPricing objanalysisprice = null;
                            if (Frame is NestedFrame)
                            {
                                NestedFrame nestedFrame = (NestedFrame)Frame;
                                objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            }
                            else
                            {
                                objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            }
                            if (objanalysisprice != null && objanalysisprice.Test != null && objanalysisprice.IsGroup == false)
                            {
                                quotesinfo.QuotePopupCrtAnalysispriceOid = objanalysisprice.Oid;
                                List<TestPriceSurcharge> lsttstpricing = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And[Component.Components] = ?", objanalysisprice.Matrix.MatrixName, objanalysisprice.Test.TestName, objanalysisprice.Method.MethodNumber, objanalysisprice.Component.Components)).ToList();
                                if (lsttstpricing != null && lsttstpricing.Count > 0)
                                {
                                    foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                    {
                                        string[] strTAToidarr = objtstpricesurcharge.TAT.Split(';');
                                        if (strTAToidarr.Length > 1)
                                        {
                                            foreach (string objoid in strTAToidarr.ToList())
                                            {
                                                if (!lsttatinfo.Contains((objoid.Trim())))
                                                {
                                                    lsttatinfo.Add((objoid.Trim()));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!lsttatinfo.Contains((objtstpricesurcharge.TAT.Trim())))
                                            {
                                                lsttatinfo.Add((objtstpricesurcharge.TAT.Trim()));
                                            }
                                        }
                                    }
                                }
                                if (lsttatinfo.Count > 0)
                                {
                                    IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatinfo));
                                    foreach (TurnAroundTime objturntime in lstturntime.ToList())
                                    {
                                        if (!lsttatoid.Contains(objturntime.TAT))
                                        {
                                            lsttatoid.Add(objturntime.TAT);
                                        }
                                    }
                                }
                            }
                            else if (objanalysisprice != null && objanalysisprice.Test != null && objanalysisprice.IsGroup == true)
                            {
                                List<TestPriceSurcharge> lsttstpricing = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'True'", objanalysisprice.Matrix.MatrixName, objanalysisprice.Test.TestName)).ToList();
                                if (lsttstpricing != null && lsttstpricing.Count > 0)
                                {
                                    foreach (TestPriceSurcharge objtstpricesurcharge in lsttstpricing.ToList())
                                    {
                                        string[] strTAToidarr = objtstpricesurcharge.TAT.Split(';');
                                        if (strTAToidarr.Length > 1)
                                        {
                                            foreach (string objoid in strTAToidarr.ToList())
                                            {
                                                if (!lsttatinfo.Contains((objoid.Trim())))
                                                {
                                                    lsttatinfo.Add((objoid.Trim()));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!lsttatinfo.Contains((objtstpricesurcharge.TAT.Trim())))
                                            {
                                                lsttatinfo.Add((objtstpricesurcharge.TAT.Trim()));
                                            }
                                        }
                                    }
                                }
                                if (lsttatinfo.Count > 0)
                                {
                                    IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatinfo));
                                    foreach (TurnAroundTime objturntime in lstturntime.ToList())
                                    {
                                        if (!lsttatoid.Contains(objturntime.TAT))
                                        {
                                            lsttatoid.Add(objturntime.TAT);
                                        }
                                    }
                                }
                            }

                            IObjectSpace os = Application.CreateObjectSpace(typeof(TurnAroundTime));
                            CollectionSource cs = new CollectionSource(os, typeof(TurnAroundTime));
                            if (lsttatoid.Count > 0)
                            {
                                cs.Criteria["filter"] = new InOperator("TAT", lsttatoid);
                            }
                            ListView lvtpstat = Application.CreateListView("TurnAroundTime_ListView_Quotes", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lvtpstat);
                            showViewParameters.CreatedView = lvtpstat;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.Accepting += TAT_dc_Accepting;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }

                    else if (splparm[0] == "SampleMatrix.VisualMatrixName")
                    {
                        List<string> lsttatinfo = new List<string>();
                        List<Guid> lstVMoid = new List<Guid>();
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(splparm[1]), "Oid");
                            AnalysisPricing objanalysisprice = null;
                            if (Frame is NestedFrame)
                            {
                                NestedFrame nestedFrame = (NestedFrame)Frame;
                                objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            }
                            else
                            {
                                objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            }
                            if (objanalysisprice != null && objanalysisprice.Test != null)// && objanalysisprice.IsGroup == false)
                            {
                                if (objanalysisprice.SampleMatrix != null)
                                    quotesinfo.QuotePopupVMOid = objanalysisprice.SampleMatrix.Oid;
                                List<VisualMatrix> lstVisualMatrix = ObjectSpace.GetObjects<VisualMatrix>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? ", objanalysisprice.Matrix.MatrixName)).ToList();
                                if (lstVisualMatrix != null && lstVisualMatrix.Count > 0)
                                {
                                    //foreach (VisualMatrix objtstpricesurcharge in lstVisualMatrix.ToList())
                                    //{
                                    //    string[] strTAToidarr = objtstpricesurcharge.TAT.Split(';');
                                    //    if (strTAToidarr.Length > 1)
                                    //    {
                                    //        foreach (string objoid in strTAToidarr.ToList())
                                    //        {
                                    //            if (!lsttatinfo.Contains((objoid.Trim())))
                                    //            {
                                    //                lsttatinfo.Add((objoid.Trim()));
                                    //            }
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (!lsttatinfo.Contains((objtstpricesurcharge.TAT.Trim())))
                                    //        {
                                    //            lsttatinfo.Add((objtstpricesurcharge.TAT.Trim()));
                                    //        }
                                    //    }
                                    //}
                                    foreach (VisualMatrix objVM in lstVisualMatrix.ToList())
                                    {
                                        if (!lstVMoid.Contains(objVM.Oid))
                                        {
                                            lstVMoid.Add(objVM.Oid);
                                        }
                                    }
                                }
                                //if (lsttatinfo.Count > 0)
                                //{
                                //    IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatinfo));
                                //    foreach (TurnAroundTime objturntime in lstturntime.ToList())
                                //    {
                                //        if (!lsttatoid.Contains(objturntime.TAT))
                                //        {
                                //            lsttatoid.Add(objturntime.TAT);
                                //        }
                                //    }
                                //}
                            }
                            //else if (objanalysisprice != null && objanalysisprice.Test != null && objanalysisprice.IsGroup == true)
                            //{
                            //    List<TestPriceSurcharge> lsttstpricing = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'True'", objanalysisprice.Matrix.MatrixName, objanalysisprice.Test.TestName)).ToList();

                            //    if (lsttatinfo.Count > 0)
                            //    {
                            //        IList<TurnAroundTime> lstturntime = ObjectSpace.GetObjects<TurnAroundTime>(new InOperator("TAT", lsttatinfo));
                            //        foreach (TurnAroundTime objturntime in lstturntime.ToList())
                            //        {
                            //            if (!lsttatoid.Contains(objturntime.TAT))
                            //            {
                            //                lsttatoid.Add(objturntime.TAT);
                            //            }
                            //        }
                            //    }
                            //}

                            IObjectSpace os = Application.CreateObjectSpace(typeof(VisualMatrix));
                            CollectionSource cs = new CollectionSource(os, typeof(VisualMatrix));
                            if (lstVMoid.Count > 0)
                            {
                                cs.Criteria["filter"] = new InOperator("Oid", lstVMoid);
                            }
                            ListView lvtpstat = Application.CreateListView("VisualMatrix_ListView_Quotes", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lvtpstat);
                            showViewParameters.CreatedView = lvtpstat;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.Accepting += dcVM_Accepting;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                    ////else if (splparm[0] == "Parameter")
                    ////{
                    ////    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    ////    if (editor != null)
                    ////    {
                    ////        HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(splparm[1]), "Oid");
                    ////        AnalysisPricing objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    ////        if (objanalysisprice != null && objanalysisprice.IsGroup == false && objanalysisprice.Matrix != null && objanalysisprice.Method != null && objanalysisprice.Test != null)
                    ////        {
                    ////            IList<Testparameter> lstTestParam = View.ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName]=? And [TestMethod.TestName]=? And [TestMethod.MethodName.MethodNumber]=? And [Component.Components]=?", objanalysisprice.Matrix.MatrixName, objanalysisprice.Test.TestName, objanalysisprice.Method.MethodNumber, objanalysisprice.Component.Components));
                    ////            if (lstTestParam != null && lstTestParam.Count > 0)
                    ////            {
                    ////                CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Testparameter));
                    ////                cs.Criteria["Filter"] = new InOperator("Oid", lstTestParam.Select(i => i.Oid).ToList());
                    ////                ListView lv = Application.CreateListView("Testparameter_LookupListView_Quotes_Param", cs, false);
                    ////                ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                    ////                showViewParameters.Context = TemplateContext.PopupWindow;
                    ////                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    ////                DialogController dc = Application.CreateController<DialogController>();
                    ////                dc.SaveOnAccept = false;
                    ////                dc.CancelAction.Active.SetItemValue("Cancel", false);
                    ////                dc.AcceptAction.Active.SetItemValue("ok", false);
                    ////                dc.CloseOnCurrentObjectProcessing = false;
                    ////                showViewParameters.Controllers.Add(dc);
                    ////                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    ////            }
                    ////        }
                    ////        else if (objanalysisprice != null && objanalysisprice.IsGroup == true && objanalysisprice.Matrix != null && objanalysisprice.Test != null)
                    ////        {
                    ////            TestMethod objtm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [IsGroup] = True", objanalysisprice.Matrix.MatrixName, objanalysisprice.Test.TestName));
                    ////            if (objtm != null)
                    ////            {
                    ////                CollectionSource cs = new CollectionSource(ObjectSpace, typeof(GroupTestMethod));
                    ////                cs.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid);
                    ////                ListView lvparameter = Application.CreateListView("GroupTestMethod_ListView_Quotes", cs, false);
                    ////                ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                    ////                showViewParameters.CreatedView = lvparameter;
                    ////                showViewParameters.Context = TemplateContext.PopupWindow;
                    ////                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    ////                DialogController dc = Application.CreateController<DialogController>();
                    ////                dc.SaveOnAccept = false;
                    ////                dc.CloseOnCurrentObjectProcessing = false;
                    ////                dc.AcceptAction.Active.SetItemValue("OK", false);
                    ////                dc.CancelAction.Active.SetItemValue("Cancel", false);
                    ////                showViewParameters.Controllers.Add(dc);
                    ////                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    ////            }
                    ////        }
                    ////    }
                    ////}
                    if (splparm[0] == "QuotesItemPrice")
                    {
                        ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridlisteditor != null && gridlisteditor.Grid != null)
                        {
                            if (View.Id == "ItemChargePricing_ListView_Quotes_Popup")
                            {
                                if (splparm[1] == "Selectall" && quotesinfo.IsItemchargePricingpopupselectall == false)
                                {
                                    quotesinfo.IsItemchargePricingpopupselectall = true;
                                    foreach (ItemChargePricing objitemprice in ((ListView)View).CollectionSource.List)
                                    {
                                        if (!quotesinfo.lsttempItemPricingpopup.Contains(objitemprice))
                                        {
                                            quotesinfo.lsttempItemPricingpopup.Add(objitemprice);
                                        }
                                    }
                                    gridlisteditor.Grid.Selection.SelectAll();
                                }
                                else if (splparm[1] == "UNSelectall")
                                {
                                    quotesinfo.IsItemchargePricingpopupselectall = false;
                                    quotesinfo.lsttempItemPricingpopup.Clear();
                                    gridlisteditor.Grid.Selection.UnselectAll();
                                }
                                else if (splparm[1] == "Selected" || splparm[1] == "UNSelected")
                                {
                                    if (!string.IsNullOrEmpty(splparm[1]))
                                    {
                                        if (splparm[1] == "Selected")
                                        {
                                            quotesinfo.IsItemchargePricingpopupselectall = false;
                                            ItemChargePricing objitemprice = ObjectSpace.FindObject<ItemChargePricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(splparm[2])));
                                            if (objitemprice != null && !quotesinfo.lsttempItemPricingpopup.Contains(objitemprice))
                                            {
                                                quotesinfo.lsttempItemPricingpopup.Add(objitemprice);
                                                gridlisteditor.Grid.Selection.SelectRowByKey(objitemprice);
                                            }
                                        }
                                        else if (splparm[1] == "UNSelected")
                                        {
                                            quotesinfo.IsItemchargePricingpopupselectall = false;
                                            ItemChargePricing objitemprice = ObjectSpace.FindObject<ItemChargePricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(splparm[2])));
                                            if (objitemprice != null && quotesinfo.lsttempItemPricingpopup.Contains(objitemprice))
                                            {
                                                quotesinfo.lsttempItemPricingpopup.Remove(objitemprice);
                                                gridlisteditor.Grid.Selection.UnselectRowByKey(objitemprice);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (splparm[0] == "TAT" && View.Id == "TurnAroundTime_ListView_Quotes")
                    {
                        //AnalysisPricing objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        AnalysisPricing objanalysisprice = null;
                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        }
                        else
                        {
                            objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        }
                        if (objanalysisprice != null)
                        {
                            quotesinfo.QuotePopupCrtAnalysispriceOid = objanalysisprice.Oid;
                        }

                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(splparm[1]), "Oid");
                            TurnAroundTime objitemprice = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objitemprice != null)
                            {
                                quotesinfo.QuotePopupTATOid = objitemprice.Oid;
                                editor.Grid.Selection.SelectRowByKey(objitemprice.Oid);
                            }
                        }
                    }
                    else if (splparm[0] == "VisualMatrixName" && View.Id == "VisualMatrix_ListView_Quotes")
                    {
                        AnalysisPricing objanalysisprice = null;
                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        }
                        else
                        {
                            objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        }
                        if (objanalysisprice != null)
                        {
                            quotesinfo.QuotePopupCrtAnalysispriceOid = objanalysisprice.Oid;
                        }
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(splparm[1]), "Oid");
                            VisualMatrix objitempVM = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objitempVM != null)
                            {
                                quotesinfo.QuotePopupVMOid = objitempVM.Oid;
                                editor.Grid.Selection.SelectRowByKey(objitempVM.Oid);
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

        private void dcVM_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DialogController dc = (DialogController)sender;
                    Guid quoVMoid = quotesinfo.QuotePopupVMOid;
                    Guid quotecrtanalysisoid = quotesinfo.QuotePopupCrtAnalysispriceOid;
                    if (dc != null && dc.Window != null && dc.Window.View != null && dc.Window.View.Id == "VisualMatrix_ListView_Quotes" && quoVMoid != Guid.Empty && quotecrtanalysisoid != Guid.Empty && dc.Window.View.SelectedObjects.Count > 0)
                    {

                        //AnalysisPricing objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", quotecrtanalysisoid));
                        AnalysisPricing objanalysisprice = null;
                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", quotecrtanalysisoid));
                        }
                        else
                        {
                            objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", quotecrtanalysisoid));
                        }
                        if (objanalysisprice != null)
                        {
                            VisualMatrix objquoteVM = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] =?", quoVMoid));
                            if (objquoteVM != null)
                            {
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    objanalysisprice.SampleMatrix = nestedFrame.View.ObjectSpace.GetObject(objquoteVM);
                                }
                                else
                                {
                                    objanalysisprice.SampleMatrix = Application.MainWindow.View.ObjectSpace.GetObject(objquoteVM);
                                }
                                //objanalysisprice.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objquotetat);
                            }
                        }
                    }
                    else
                    {

                        Application.ShowViewStrategy.ShowMessage("Select atleast one sample matrix.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void TAT_dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    quotesinfo.IsTabDiscountChanged = true;
                    DialogController dc = (DialogController)sender;
                    Guid quotetatoid = quotesinfo.QuotePopupTATOid;
                    Guid quotecrtanalysisoid = quotesinfo.QuotePopupCrtAnalysispriceOid;
                    if (dc != null && dc.Window != null && dc.Window.View != null && dc.Window.View.Id == "TurnAroundTime_ListView_Quotes" && quotetatoid != Guid.Empty && quotecrtanalysisoid != Guid.Empty)
                    {
                        //AnalysisPricing objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", quotecrtanalysisoid));
                        AnalysisPricing objanalysisprice = null;
                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", quotecrtanalysisoid));
                        }
                        else
                        {
                            objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", quotecrtanalysisoid));
                        }
                        if (objanalysisprice != null)
                        {
                            TurnAroundTime objquotetat = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[Oid] =?", quotetatoid));
                            if (objquotetat != null)
                            {
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    objanalysisprice.TAT = nestedFrame.View.ObjectSpace.GetObject(objquotetat);
                                }
                                else
                                {
                                    objanalysisprice.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objquotetat);
                                }
                                //objanalysisprice.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objquotetat);
                            }
                            uint intqty = 0;
                            decimal unitamt = 0;
                            decimal discntamt = 0;
                            Guid rowoid = objanalysisprice.Oid;
                            string tatoid = objanalysisprice.TAT.TAT;
                            if (rowoid != Guid.Empty && tatoid != null)
                            {
                                decimal surchargevalue = 0;
                                decimal schargeamt = 0;
                                decimal surchargeprice = 0;
                                decimal pcdisamt = 0;
                                string tat = string.Empty;
                                //AnalysisPricing objap = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                AnalysisPricing objap = null;
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    objap = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                }
                                else
                                {
                                    objap = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                                }
                                if (objap.TAT != null && objap.Test != null && objap.Component != null)
                                {
                                    intqty = objap.Qty;
                                    unitamt = objap.UnitPrice;
                                    discntamt = objap.Discount;
                                    string str = tatoid.ToString().Replace(" ", "");
                                    TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components, tatoid.Trim()));
                                    if (objtps != null)
                                    {
                                        objap.PriceCode = objtps.PriceCode;
                                        if (objtps.Surcharge != null)
                                        {
                                            objap.NPSurcharge = (int)objtps.Surcharge;
                                            surchargevalue = (int)objtps.Surcharge;
                                        }
                                        if (Frame is NestedFrame)
                                        {
                                            NestedFrame nestedFrame = (NestedFrame)Frame;
                                            objap.Priority = nestedFrame.View.ObjectSpace.GetObject(objtps.Priority);
                                        }
                                        else
                                        {
                                            objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                        }
                                        //objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                        if (objtps.SurchargePrice != null)
                                        {
                                            surchargeprice = (decimal)objtps.SurchargePrice;
                                        }
                                    }
                                    else
                                    {
                                        objap.NPSurcharge = 0;
                                    }
                                }
                                else if (objap.TAT != null && objap.Test != null && objap.IsGroup == true)
                                {
                                    intqty = objap.Qty;
                                    unitamt = objap.UnitPrice;
                                    discntamt = objap.Discount;
                                    TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Test.TestName, tatoid.Trim()));
                                    if (objtps != null)
                                    {
                                        objap.PriceCode = objtps.PriceCode;
                                        if (objtps.Surcharge != null)
                                        {
                                            objap.NPSurcharge = (int)objtps.Surcharge;
                                        }
                                        if (Frame is NestedFrame)
                                        {
                                            NestedFrame nestedFrame = (NestedFrame)Frame;
                                            objap.Priority = nestedFrame.View.ObjectSpace.GetObject(objtps.Priority);
                                        }
                                        else
                                        {
                                            objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                        }
                                        //objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                        if (objtps.Surcharge != null)
                                        {
                                            surchargevalue = (int)objtps.Surcharge;
                                        }
                                        if (objtps.SurchargePrice != null)
                                        {
                                            surchargeprice = (decimal)objtps.SurchargePrice;
                                        }
                                    }
                                    else
                                    {
                                        objap.NPSurcharge = 0;
                                    }
                                }
                                if (surchargevalue > 0)
                                {
                                    if (objap.IsGroup == false)
                                    {
                                        ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                        if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                        {
                                            ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                                            if (objconpricetier != null)
                                            {
                                                objap.TierNo = objconpricetier.TierNo;
                                                objap.From = objconpricetier.From;
                                                objap.To = objconpricetier.To;
                                                objap.TierPrice = objconpricetier.TierPrice;
                                                if (strfocusedcolumn == "Qty")
                                                {
                                                    unitamt = objconpricetier.TierPrice;
                                                }
                                                schargeamt = unitamt * (surchargevalue / 100);
                                                objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                //unitamt = objconpricetier.TierPrice;
                                                //schargeamt = unitamt * (surchargevalue / 100);
                                                //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                                if (discntamt > 0)
                                                {
                                                    pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                    objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                                else if (discntamt < 0)
                                                {
                                                    pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                    objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                                else
                                                {
                                                    objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                            }
                                            else
                                            {
                                                List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                                                foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                                                {
                                                    objap.TierNo = objconpricetier.TierNo;
                                                    objap.From = objconpricetier.From;
                                                    objap.To = objconpricetier.To;
                                                    objap.TierPrice = objconpricetier.TierPrice;
                                                    if (strfocusedcolumn == "Qty")
                                                    {
                                                        unitamt = objconpricetier.TierPrice;
                                                    }
                                                    schargeamt = unitamt * (surchargevalue / 100);
                                                    objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                    //unitamt = objconpricetier.TierPrice;
                                                    //schargeamt = unitamt * (surchargevalue / 100);
                                                    //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                                    if (discntamt > 0)
                                                    {
                                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    else if (discntamt < 0)
                                                    {
                                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    else
                                                    {
                                                        objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //schargeamt = unitamt * (surchargevalue / 100);
                                            schargeamt = surchargeprice;
                                            objap.TotalTierPrice = schargeamt;
                                            if (discntamt > 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else if (discntamt < 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else
                                            {
                                                objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //schargeamt = unitamt * (surchargevalue / 100);
                                        schargeamt = surchargeprice;
                                        objap.TotalTierPrice = schargeamt;
                                        if (discntamt > 0)
                                        {
                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else if (discntamt < 0)
                                        {
                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else
                                        {
                                            objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                    }
                                }
                                else
                                {
                                    if (objap.IsGroup == false)
                                    {
                                        ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                        if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                        {
                                            ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                                            if (objconpricetier != null)
                                            {
                                                objap.TierNo = objconpricetier.TierNo;
                                                objap.From = objconpricetier.From;
                                                objap.To = objconpricetier.To;
                                                objap.TierPrice = objconpricetier.TierPrice;
                                                //objap.TotalTierPrice = objconpricetier.TierPrice /** intqty*/;
                                                //unitamt = objconpricetier.TierPrice;
                                                if (strfocusedcolumn == "Qty")
                                                {
                                                    unitamt = objconpricetier.TierPrice;
                                                }
                                                //schargeamt = unitamt * (surchargevalue / 100);
                                                objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                if (discntamt > 0)
                                                {
                                                    pcdisamt = (unitamt) * (discntamt / 100);
                                                    objap.FinalAmount = ((unitamt) - (pcdisamt)) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                                else if (discntamt < 0)
                                                {
                                                    pcdisamt = (unitamt) * (discntamt / 100);
                                                    objap.FinalAmount = ((unitamt) - (pcdisamt)) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                                else
                                                {
                                                    objap.FinalAmount = (unitamt) * intqty;
                                                    objap.UnitPrice = unitamt;
                                                }
                                            }
                                            else
                                            {
                                                List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                                                foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                                                {
                                                    objap.TierNo = objconprice.TierNo;
                                                    objap.From = objconprice.From;
                                                    objap.To = objconprice.To;
                                                    objap.TierPrice = objconprice.TierPrice;
                                                    //unitamt = objconpricetier.TierPrice;
                                                    //schargeamt = unitamt * (surchargevalue / 100);
                                                    //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                                    if (strfocusedcolumn == "Qty")
                                                    {
                                                        unitamt = objconprice.TierPrice;
                                                    }
                                                    schargeamt = unitamt * (surchargevalue / 100);
                                                    objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                                    if (discntamt > 0)
                                                    {
                                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    else if (discntamt < 0)
                                                    {
                                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    else
                                                    {
                                                        objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                        objap.UnitPrice = unitamt;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //schargeamt = unitamt * (surchargevalue / 100);
                                            schargeamt = surchargeprice;
                                            //objap.TotalTierPrice = schargeamt + unitamt;
                                            objap.TotalTierPrice = schargeamt;
                                            if (discntamt > 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else if (discntamt < 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else
                                            {
                                                objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //schargeamt = unitamt * (surchargevalue / 100);
                                        schargeamt = surchargeprice;
                                        objap.TotalTierPrice = schargeamt;
                                        if (discntamt > 0)
                                        {
                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else if (discntamt < 0)
                                        {
                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else
                                        {
                                            objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                    }
                                }
                                TurnAroundTime objtat = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[TAT] = ?", tatoid));
                                if (objtat != null)
                                {
                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        objap.TAT = nestedFrame.View.ObjectSpace.GetObject(objtat);
                                    }
                                    else
                                    {
                                        objap.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                                    }
                                    //objap.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                                }
                                ((ListView)View).Refresh();
                                decimal finalamt = 0;
                                decimal totalprice = 0;
                                decimal disamt = 0;
                                decimal dispr = 0;
                                ListPropertyEditor lstAnalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;

                                {//if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                                 //{
                                 //    foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                 //    {
                                 //        finalamt = finalamt + objanapricing.FinalAmount;
                                 //        totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                                 //    }
                                 //}
                                 //DashboardViewItem dvitemprice = ((DetailView)Application.MainWindow.View).FindItem("DVItemChargePrice") as DashboardViewItem;
                                 //if (dvitemprice != null && dvitemprice.InnerView != null)
                                 //{
                                 //    foreach (QuotesItemChargePrice objitempricing in ((ListView)dvitemprice.InnerView).CollectionSource.List)
                                 //    {
                                 //        finalamt = finalamt + objitempricing.FinalAmount;
                                 //        totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                                 //    }
                                 //}
                                }
                                ListPropertyEditor lstitemprice = ((DetailView)Application.MainWindow.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                                //if (lvitemprice != null && lvitemprice.ListView != null)
                                //{
                                //    foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                //    {
                                //        finalamt = finalamt + objitempricing.FinalAmount;
                                //        totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                                //    }
                                //}
                                if (lstitemprice != null)
                                {
                                    if (lstitemprice.ListView == null)
                                    {
                                        lstitemprice.CreateControl();
                                    }
                                    if (lstitemprice.ListView.CollectionSource.GetCount() > 0)
                                    {
                                        finalamt = finalamt + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.FinalAmount);
                                        totalprice = totalprice + lstitemprice.ListView.CollectionSource.List.Cast<QuotesItemChargePrice>().Sum(i => i.UnitPrice * i.Qty);
                                    }
                                }
                                if (lstAnalysisprice != null)
                                {
                                    if (lstAnalysisprice.ListView == null)
                                    {
                                        lstAnalysisprice.CreateControl();
                                    }
                                    if (lstAnalysisprice.ListView.CollectionSource.GetCount() > 0)
                                    {
                                        finalamt = finalamt + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.FinalAmount);
                                        totalprice = totalprice + lstAnalysisprice.ListView.CollectionSource.List.Cast<AnalysisPricing>().Sum(i => i.TotalTierPrice * i.Qty);
                                    }
                                }
                                if (finalamt < 0)
                                {
                                    objap.Discount = 0;
                                    objap.FinalAmount = 0;
                                    finalamt = 0;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                }
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    CRMQuotes crtquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                                    if (crtquotes != null)
                                    {
                                        if (finalamt >= 0 && totalprice >= 0)
                                        {
                                            //disamt = finalamt - totalprice;
                                            disamt = totalprice - finalamt;
                                            if (disamt != 0)
                                            {
                                                dispr = ((disamt) / totalprice) * 100;
                                            }
                                            quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                            crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Math.Round(dispr, 2);
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                        }
                                        else
                                        {
                                            dispr = ((totalprice) / totalprice) * 100;
                                            //disamt = finalamt - totalprice;
                                            disamt = totalprice - finalamt;
                                            quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                            crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Math.Round(dispr, 2);
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                        }
                                    }
                                }
                                else
                                {
                                    CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                                    if (crtquotes != null)
                                    {
                                        if (finalamt >= 0 && totalprice >= 0)
                                        {
                                            //disamt = finalamt - totalprice;
                                            disamt = totalprice - finalamt;
                                            if (disamt != 0)
                                            {
                                                dispr = ((disamt) / totalprice) * 100;
                                            }
                                            quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                            crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Math.Round(dispr, 2);
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                        }
                                        else
                                        {
                                            dispr = ((totalprice) / totalprice) * 100;
                                            //disamt = finalamt - totalprice;
                                            disamt = totalprice - finalamt;
                                            quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                            crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                            crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                            crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                            crtquotes.Discount = Math.Round(dispr, 2);
                                            crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (View.Id == "CRMQuotes_AnalysisPricing_ListView" && e.AcceptActionArgs.SelectedObjects.Count == 1)
                {
                    AnalysisPricing objanalysisprice = null;
                    if (Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        objanalysisprice = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    }
                    else
                    {
                        objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    }
                    //AnalysisPricing objanalysisprice = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    if (objanalysisprice != null)
                    {
                        foreach (TurnAroundTime objtat in e.AcceptActionArgs.SelectedObjects)
                        {
                            if (Frame is NestedFrame)
                            {
                                NestedFrame nestedFrame = (NestedFrame)Frame;
                                objanalysisprice.TAT = nestedFrame.View.ObjectSpace.GetObject(objtat);
                            }
                            else
                            {
                                objanalysisprice.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                            }
                            //objanalysisprice.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                        }
                        uint intqty = 0;
                        decimal unitamt = 0;
                        decimal discntamt = 0;
                        Guid rowoid = objanalysisprice.Oid;
                        string tatoid = objanalysisprice.TAT.TAT;
                        if (rowoid != Guid.Empty && tatoid != null)
                        {
                            decimal surchargevalue = 0;
                            decimal schargeamt = 0;
                            decimal pcdisamt = 0;
                            string tat = string.Empty;
                            AnalysisPricing objap = null;
                            if (Frame is NestedFrame)
                            {
                                NestedFrame nestedFrame = (NestedFrame)Frame;
                                objap = nestedFrame.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                            }
                            else
                            {
                                objap = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                            }
                            //AnalysisPricing objap = Application.MainWindow.View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", rowoid));
                            if (objap.TAT != null && objap.Test != null && objap.Component != null)
                            {
                                intqty = objap.Qty;
                                unitamt = objap.UnitPrice;
                                discntamt = objap.Discount;
                                TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components, tatoid.Trim()));
                                if (objtps != null)
                                {
                                    objap.PriceCode = objtps.PriceCode;
                                    if (objtps.Surcharge != null)
                                    {
                                        objap.NPSurcharge = (int)objtps.Surcharge;
                                        surchargevalue = (int)objtps.Surcharge;
                                    }
                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        objap.Priority = nestedFrame.View.ObjectSpace.GetObject(objtps.Priority);
                                    }
                                    else
                                    {
                                        objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                    }
                                    //objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                }
                                else
                                {
                                    objap.NPSurcharge = 0;
                                }
                            }
                            else if (objap.TAT != null && objap.Test != null && objap.IsGroup == true)
                            {
                                TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And Contains([TAT], ?)", objap.Matrix.MatrixName, objap.Test.TestName, tatoid.Trim()));
                                if (objtps != null)
                                {
                                    objap.PriceCode = objtps.PriceCode;
                                    if (objtps.Surcharge != null)
                                    {
                                        objap.NPSurcharge = (int)objtps.Surcharge;
                                        surchargevalue = (int)objtps.Surcharge;
                                    }
                                    if (Frame is NestedFrame)
                                    {
                                        NestedFrame nestedFrame = (NestedFrame)Frame;
                                        objap.Priority = nestedFrame.View.ObjectSpace.GetObject(objtps.Priority);
                                    }
                                    else
                                    {
                                        objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                    }
                                    //objap.Priority = Application.MainWindow.View.ObjectSpace.GetObject(objtps.Priority);
                                }
                                else
                                {
                                    objap.NPSurcharge = 0;
                                }
                            }
                            if (surchargevalue > 0)
                            {
                                ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                {
                                    ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                                    if (objconpricetier != null)
                                    {
                                        objap.TierNo = objconpricetier.TierNo;
                                        objap.From = objconpricetier.From;
                                        objap.To = objconpricetier.To;
                                        objap.TierPrice = objconpricetier.TierPrice;
                                        //unitamt = objconpricetier.TierPrice;
                                        //schargeamt = unitamt * (surchargevalue / 100);
                                        //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                        if (strfocusedcolumn == "Qty")
                                        {
                                            unitamt = objconpricetier.TierPrice;
                                        }
                                        schargeamt = unitamt * (surchargevalue / 100);
                                        //objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                        objap.TotalTierPrice = schargeamt;
                                        if (discntamt > 0)
                                        {
                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else if (discntamt < 0)
                                        {
                                            pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                            objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else
                                        {
                                            objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                    }
                                    else
                                    {
                                        List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                                        foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                                        {
                                            objap.TierNo = objconprice.TierNo;
                                            objap.From = objconprice.From;
                                            objap.To = objconprice.To;
                                            objap.TierPrice = objconprice.TierPrice;
                                            if (strfocusedcolumn == "Qty")
                                            {
                                                unitamt = objconprice.TierPrice;
                                            }
                                            schargeamt = unitamt * (surchargevalue / 100);
                                            objap.TotalTierPrice = schargeamt;
                                            //objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                            //unitamt = objconpricetier.TierPrice;
                                            //schargeamt = unitamt * (surchargevalue / 100);
                                            //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                            if (discntamt > 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else if (discntamt < 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else
                                            {
                                                objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    schargeamt = unitamt * (surchargevalue / 100);
                                    objap.TotalTierPrice = schargeamt;
                                    //objap.TotalTierPrice = schargeamt + unitamt;
                                    if (discntamt > 0)
                                    {
                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                        objap.UnitPrice = unitamt;
                                    }
                                    else if (discntamt < 0)
                                    {
                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                        objap.UnitPrice = unitamt;
                                    }
                                    else
                                    {
                                        objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                        objap.UnitPrice = unitamt;
                                    }
                                }
                            }
                            else
                            {
                                ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                {
                                    ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, intqty, intqty));
                                    if (objconpricetier != null)
                                    {
                                        objap.TierNo = objconpricetier.TierNo;
                                        objap.From = objconpricetier.From;
                                        objap.To = objconpricetier.To;
                                        objap.TierPrice = objconpricetier.TierPrice;
                                        //schargeamt = unitamt * (surchargevalue / 100);
                                        //objap.TotalTierPrice = objconpricetier.TierPrice /** intqty*/;
                                        //unitamt = objconpricetier.TierPrice;
                                        if (strfocusedcolumn == "Qty")
                                        {
                                            unitamt = objconpricetier.TierPrice;
                                        }
                                        schargeamt = unitamt * (surchargevalue / 100);
                                        //objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                        objap.TotalTierPrice = surchargevalue;
                                        if (discntamt > 0)
                                        {
                                            pcdisamt = (unitamt) * (discntamt / 100);
                                            objap.FinalAmount = ((unitamt) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else if (discntamt < 0)
                                        {
                                            pcdisamt = (unitamt) * (discntamt / 100);
                                            objap.FinalAmount = ((unitamt) - (pcdisamt)) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                        else
                                        {
                                            objap.FinalAmount = (unitamt) * intqty;
                                            objap.UnitPrice = unitamt;
                                        }
                                    }
                                    else
                                    {
                                        List<ConstituentPricingTier> lstconsttier = ObjectSpace.GetObjects<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? ", objcps.Oid)).ToList();
                                        foreach (ConstituentPricingTier objconprice in lstconsttier.ToList().Cast<ConstituentPricingTier>().OrderByDescending(i => i.To))
                                        {
                                            objap.TierNo = objconprice.TierNo;
                                            objap.From = objconprice.From;
                                            objap.To = objconprice.To;
                                            objap.TierPrice = objconprice.TierPrice;
                                            //unitamt = objconpricetier.TierPrice;
                                            //schargeamt = unitamt * (surchargevalue / 100);
                                            //objap.TotalTierPrice = (objconpricetier.TierPrice + schargeamt)/* * intqty*/;
                                            if (strfocusedcolumn == "Qty")
                                            {
                                                unitamt = objconprice.TierPrice;
                                            }
                                            schargeamt = unitamt * (surchargevalue / 100);
                                            objap.TotalTierPrice = schargeamt;
                                            //objap.TotalTierPrice = (unitamt + schargeamt)/* * intqty*/;
                                            if (discntamt > 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else if (discntamt < 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            else
                                            {
                                                objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                                objap.UnitPrice = unitamt;
                                            }
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    schargeamt = unitamt * (surchargevalue / 100);
                                    objap.TotalTierPrice = schargeamt;
                                    //objap.TotalTierPrice = schargeamt + unitamt;
                                    if (discntamt > 0)
                                    {
                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                        objap.UnitPrice = unitamt;
                                    }
                                    else if (discntamt < 0)
                                    {
                                        pcdisamt = (objap.TotalTierPrice) * (discntamt / 100);
                                        objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * intqty;
                                        objap.UnitPrice = unitamt;
                                    }
                                    else
                                    {
                                        objap.FinalAmount = (objap.TotalTierPrice) * intqty;
                                        objap.UnitPrice = unitamt;
                                    }
                                }
                            }
                            TurnAroundTime objtat = ObjectSpace.FindObject<TurnAroundTime>(CriteriaOperator.Parse("[TAT] = ?", tatoid));
                            if (objtat != null)
                            {
                                if (Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    objap.TAT = nestedFrame.View.ObjectSpace.GetObject(objtat);
                                }
                                else
                                {
                                    objap.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                                }
                                //objap.TAT = Application.MainWindow.View.ObjectSpace.GetObject(objtat);
                            }
                            ((ListView)View).Refresh();
                            decimal finalamt = 0;
                            decimal totalprice = 0;
                            decimal disamt = 0;
                            decimal dispr = 0;
                            ListPropertyEditor lvanalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                            if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                            {
                                foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                                {
                                    finalamt = finalamt + objanapricing.FinalAmount;
                                    totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                                }
                            }
                            //DashboardViewItem dvitemprice = ((DetailView)Application.MainWindow.View).FindItem("DVItemChargePrice") as DashboardViewItem;
                            //if (dvitemprice != null && dvitemprice.InnerView != null)
                            //{
                            //    foreach (QuotesItemChargePrice objitempricing in ((ListView)dvitemprice.InnerView).CollectionSource.List)
                            //    {
                            //        finalamt = finalamt + objitempricing.FinalAmount;
                            //        totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                            //    }
                            //}
                            ListPropertyEditor lvitemprice = ((DetailView)Application.MainWindow.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                            if (lvitemprice != null && lvitemprice.ListView != null)
                            {
                                foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                                {
                                    finalamt = finalamt + objitempricing.FinalAmount;
                                    totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                                }
                            }
                            if (finalamt < 0)
                            {
                                objap.Discount = 0;
                                objap.FinalAmount = 0;
                                finalamt = 0;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowednegativeValue"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            }
                            if (Frame is NestedFrame)
                            {
                                NestedFrame nestedFrame = (NestedFrame)Frame;
                                CRMQuotes crtquotes = (CRMQuotes)nestedFrame.ViewItem.View.CurrentObject;
                                if (crtquotes != null)
                                {
                                    if (finalamt >= 0 && totalprice >= 0)
                                    {
                                        disamt = totalprice - finalamt;
                                        //disamt = finalamt - totalprice;
                                        if (disamt != 0)
                                        {
                                            dispr = ((disamt) / totalprice) * 100;
                                        }
                                        //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                        //disamt = finalamt - totalprice;
                                        quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                        crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                        crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                        crtquotes.Discount = Math.Round(dispr, 2);
                                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                    }
                                    else
                                    {
                                        dispr = ((totalprice) / totalprice) * 100;
                                        //disamt = finalamt - totalprice;
                                        disamt = totalprice - finalamt;
                                        quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                        crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                        crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                        crtquotes.Discount = Math.Round(dispr, 2);
                                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                    }
                                }
                            }
                            else
                            {
                                CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                                if (crtquotes != null)
                                {
                                    if (finalamt >= 0 && totalprice >= 0)
                                    {
                                        disamt = totalprice - finalamt;
                                        //disamt = finalamt - totalprice;
                                        if (disamt != 0)
                                        {
                                            dispr = ((disamt) / totalprice) * 100;
                                        }
                                        //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                        //disamt = finalamt - totalprice;
                                        quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                        crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                        crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                        crtquotes.Discount = Math.Round(dispr, 2);
                                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                    }
                                    else
                                    {
                                        dispr = ((totalprice) / totalprice) * 100;
                                        //disamt = finalamt - totalprice;
                                        disamt = totalprice - finalamt;
                                        quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                        crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                        crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                        crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                        crtquotes.Discount = Math.Round(dispr, 2);
                                        crtquotes.DiscountAmount = Math.Round(disamt, 2);
                                    }
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

        private void Dcparameter_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (sender != null /*&& quotesinfo.lsttempparamsoid != null && quotesinfo.lsttempparamsoid.Count > 0*/)
                {
                    //quotesinfo.lsttempparamsoid = new List<Guid>();
                    IObjectSpace os = Application.CreateObjectSpace();
                    DialogController dc = sender as DialogController;
                    if (dc != null && dc.Window != null && dc.Window.View != null && dc.Window.View.Id == "Testparameter_LookupListView_Quotes" && dc.Window.View.SelectedObjects.Count > 0)
                    {
                        AnalysisPricing objcrtanaprice = (AnalysisPricing)View.CurrentObject;
                        string strtestparameter = string.Empty;
                        ASPxGridListEditor gridlisteditor = ((ListView)dc.Window.View).Editor as ASPxGridListEditor;
                        if (gridlisteditor != null && gridlisteditor.Grid != null)
                        {
                            foreach (Testparameter objpara in dc.Window.View.SelectedObjects)
                            {
                                if (string.IsNullOrEmpty(strtestparameter))
                                {
                                    strtestparameter = objpara.Parameter.Oid.ToString();
                                }
                                else
                                {
                                    strtestparameter = strtestparameter + "; " + objpara.Parameter.Oid.ToString();
                                }
                            }
                            if (View != null && View.CurrentObject != null)
                            {
                                AnalysisPricing objCrtanaprice = (AnalysisPricing)View.CurrentObject;
                                if (objCrtanaprice != null)
                                {
                                    AnalysisPricing objanalysisprice = View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", objCrtanaprice.Oid));
                                    if (objanalysisprice != null)
                                    {
                                        if (gridlisteditor.Grid.VisibleRowCount == dc.Window.View.SelectedObjects.Count)
                                        {
                                            objanalysisprice.Parameter = "AllParams";
                                        }
                                        else
                                        {
                                            objanalysisprice.Parameter = "Customized";
                                        }
                                        objanalysisprice.ParameterGuid = strtestparameter;
                                    }
                                    //gridlisteditor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                                }
                            }
                        }
                        if (objcrtanaprice != null && objcrtanaprice.Test != null && objcrtanaprice.Matrix != null && objcrtanaprice.Method != null && objcrtanaprice.Component != null)
                        {
                            DefaultPricing objdefprice = ObjectSpace.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ? And [IsGroup] = 'No'", objcrtanaprice.Test.Oid));
                            ConstituentPricing objconsprice = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And[Test.TestName] = ? And[Method.MethodNumber] = ? And [Component.Components] = ?", objcrtanaprice.Matrix.MatrixName, objcrtanaprice.Test.TestName, objcrtanaprice.Method.MethodNumber, objcrtanaprice.Component.Components));
                            //ConstituentPricing objconsprice = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ? And [IsGroup] = False", objcrtanaprice.Test.Oid));
                            if (objdefprice != null)
                            {
                                if (dc.Window.View.SelectedObjects.Count > 0)
                                {
                                    decimal paramscnt = dc.Window.View.SelectedObjects.Count;
                                    decimal deftotalamt = objdefprice.UnitPrice;// + objdefprice.Prep1Charge + objdefprice.Prep2Charge;
                                    decimal modparamscnt = paramscnt * deftotalamt;
                                    decimal modtatparamscnt = paramscnt * deftotalamt;
                                    //bool issurcharge = false;
                                    decimal surcharge = 0;
                                    List<TestPriceSurcharge> lsttps = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ?", objcrtanaprice.Matrix.MatrixName, objcrtanaprice.Test.TestName, objcrtanaprice.Method.MethodNumber, objcrtanaprice.Component.Components)).ToList();
                                    foreach (TestPriceSurcharge objtps in lsttps.ToList())
                                    {
                                        if (objtps != null && !string.IsNullOrEmpty(objtps.TAT) /*&& issurcharge == false*/)
                                        {
                                            string[] strtatqrr = objtps.TAT.Split(';');
                                            foreach (string objtpstat in strtatqrr)
                                            {
                                                if (objtpstat.Trim() == objcrtanaprice.TAT.Oid.ToString())
                                                {
                                                    surcharge = Convert.ToDecimal(objtps.Surcharge);
                                                    surcharge = Math.Round(surcharge, 2);
                                                    //issurcharge = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (surcharge > 0)
                                    {
                                        decimal schargeamt = modparamscnt * (surcharge / 100);
                                        modtatparamscnt = schargeamt + modparamscnt;
                                    }
                                    AnalysisPricing objap = os.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", objcrtanaprice.Oid));
                                    if (objap != null)
                                    {
                                        objap.UnitPrice = Math.Round(modparamscnt, 2);
                                        objap.TotalTierPrice = Math.Round(modtatparamscnt, 2);

                                        objap.FinalAmount = objcrtanaprice.Qty * Math.Round(modtatparamscnt, 2);
                                    }
                                }
                            }
                            else if (objconsprice != null)
                            {
                                if (dc.Window.View.SelectedObjects.Count > 0)
                                {
                                    decimal paramscnt = dc.Window.View.SelectedObjects.Count;
                                    //bool issurcharge = false;
                                    decimal surcharge = 0;
                                    decimal modparamscnt = 0;
                                    decimal modtatparamscnt = 0;
                                    decimal pcdisamt = 0;
                                    int tiertyms = 0;
                                    decimal dectotalprice = 0;
                                    decimal schargeamt = 0;
                                    ConstituentPricingTier objconprice = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objconsprice.Oid, dc.Window.View.SelectedObjects.Count, dc.Window.View.SelectedObjects.Count));
                                    if (objconprice != null && (objconsprice != null && objconsprice.ChargeType == ChargeType.Parameter))
                                    {
                                        modparamscnt = (objconprice.TierPrice /*+ objconprice.Prep1Charge + objconprice.Prep2Charge*/) * dc.Window.View.SelectedObjects.Count;

                                    }
                                    else if (objconprice != null && (objconsprice != null && objconsprice.ChargeType == ChargeType.Test))
                                    {
                                        modparamscnt = objconprice.TierPrice;
                                    }
                                    modtatparamscnt = modparamscnt;
                                    //List<TestPriceSurcharge> lsttps = ObjectSpace.GetObjects<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ?", objcrtanaprice.Matrix.MatrixName, objcrtanaprice.Test.TestName, objcrtanaprice.Method.MethodNumber, objcrtanaprice.Component.Components)).ToList();
                                    //foreach (TestPriceSurcharge objtps in lsttps.ToList())
                                    {
                                        TestPriceSurcharge objtps = ObjectSpace.FindObject<TestPriceSurcharge>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ? And Contains([TAT], ?)", objcrtanaprice.Matrix.MatrixName, objcrtanaprice.Test.TestName, objcrtanaprice.Method.MethodNumber, objcrtanaprice.Component.Components, objcrtanaprice.TAT.TAT));
                                        if (objtps != null /*&& !string.IsNullOrEmpty(objtps.TAT)*//* && issurcharge == false*/)
                                        {
                                            if (objtps.Surcharge != null)
                                            {
                                                surcharge = (int)objtps.Surcharge;
                                            }
                                            //string[] strtatqrr = objtps.TAT.Split(';');
                                            //foreach (string objtpstat in strtatqrr)
                                            //{
                                            //    if (objtpstat.Trim() == objcrtanaprice.TAT.Oid.ToString())
                                            //    {
                                            //        surcharge = Convert.ToDecimal(objtps.Surcharge);
                                            //        surcharge = Math.Round(surcharge, 2);
                                            //        //issurcharge = true;
                                            //        break;
                                            //    }
                                            //}
                                        }
                                    }
                                    if (surcharge > 0)
                                    {
                                        schargeamt = modparamscnt * (surcharge / 100);
                                        modtatparamscnt = schargeamt + modparamscnt;
                                        AnalysisPricing objap = View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", objcrtanaprice.Oid));
                                        if (objap != null)
                                        {
                                            ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                            if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                            {
                                                ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, dc.Window.View.SelectedObjects.Count, dc.Window.View.SelectedObjects.Count));
                                                if (objconpricetier != null)
                                                {
                                                    objap.TierNo = objconpricetier.TierNo;
                                                    objap.From = objconpricetier.From;
                                                    objap.To = objconpricetier.To;
                                                    objap.TierPrice = objconpricetier.TierPrice;
                                                }
                                            }
                                            else if (objcps != null && objcps.ChargeType == ChargeType.Parameter)
                                            {
                                                ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, dc.Window.View.SelectedObjects.Count, dc.Window.View.SelectedObjects.Count));
                                                if (objconpricetier != null)
                                                {
                                                    objap.TierNo = objconpricetier.TierNo;
                                                    objap.From = objconpricetier.From;
                                                    objap.To = objconpricetier.To;
                                                    objap.TierPrice = objconpricetier.TierPrice;
                                                }
                                            }
                                            objap.UnitPrice = Math.Round(modparamscnt, 2);
                                            objap.TotalTierPrice = Math.Round(modtatparamscnt, 2);
                                            if (objcrtanaprice.Discount > 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (objcrtanaprice.Discount / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * objcrtanaprice.Qty;
                                            }
                                            else if (objcrtanaprice.Discount < 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (objcrtanaprice.Discount / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * objcrtanaprice.Qty;
                                            }
                                            else
                                            {
                                                objap.FinalAmount = (objap.TotalTierPrice) * objcrtanaprice.Qty;
                                            }
                                            //objap.FinalAmount = objcrtanaprice.Qty * Math.Round(modtatparamscnt, 2);
                                        }
                                    }
                                    else
                                    {
                                        AnalysisPricing objap = View.ObjectSpace.FindObject<AnalysisPricing>(CriteriaOperator.Parse("[Oid] = ?", objcrtanaprice.Oid));
                                        if (objap != null)
                                        {
                                            ConstituentPricing objcps = ObjectSpace.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objap.Matrix.MatrixName, objap.Test.TestName, objap.Method.MethodNumber, objap.Component.Components));
                                            if (objcps != null && objcps.ChargeType == ChargeType.Test)
                                            {
                                                ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, dc.Window.View.SelectedObjects.Count, dc.Window.View.SelectedObjects.Count));
                                                if (objconpricetier != null)
                                                {
                                                    objap.TierNo = objconpricetier.TierNo;
                                                    objap.From = objconpricetier.From;
                                                    objap.To = objconpricetier.To;
                                                    objap.TierPrice = objconpricetier.TierPrice;
                                                }
                                            }
                                            else if (objcps != null && objcps.ChargeType == ChargeType.Parameter)
                                            {
                                                ConstituentPricingTier objconpricetier = ObjectSpace.FindObject<ConstituentPricingTier>(CriteriaOperator.Parse("[ConstituentPricing.Oid] = ? And [From] <= ? And [To] >= ?", objcps.Oid, dc.Window.View.SelectedObjects.Count, dc.Window.View.SelectedObjects.Count));
                                                if (objconpricetier != null)
                                                {
                                                    objap.TierNo = objconpricetier.TierNo;
                                                    objap.From = objconpricetier.From;
                                                    objap.To = objconpricetier.To;
                                                    objap.TierPrice = objconpricetier.TierPrice;
                                                }
                                            }
                                            objap.UnitPrice = Math.Round(modparamscnt, 2);
                                            objap.TotalTierPrice = Math.Round(modtatparamscnt, 2);
                                            if (objcrtanaprice.Discount > 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (objcrtanaprice.Discount / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * objcrtanaprice.Qty;
                                            }
                                            else if (objcrtanaprice.Discount < 0)
                                            {
                                                pcdisamt = (objap.TotalTierPrice) * (objcrtanaprice.Discount / 100);
                                                objap.FinalAmount = ((objap.TotalTierPrice) - (pcdisamt)) * objcrtanaprice.Qty;
                                            }
                                            else
                                            {
                                                objap.FinalAmount = (objap.TotalTierPrice) * objcrtanaprice.Qty;
                                            }
                                            //objap.FinalAmount = objcrtanaprice.Qty * Math.Round(modtatparamscnt, 2);
                                        }
                                    }
                                }
                            }
                            os.CommitChanges();
                            os.Refresh();
                        }
                        //if (View.Id == "AnalysisPricing_ListView_Quotes")
                        {
                            //////                            foreach (AnalysisPricing objanalysisPricing in ((ListView)View).CollectionSource.List)
                            ////CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                            ////List<AnalysisPricing> lstpricing = View.ObjectSpace.GetObjects<AnalysisPricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ?", crtquotes.Oid)).ToList();
                            ////foreach (AnalysisPricing objanalysisPricing in lstpricing.ToList())
                            ////{
                            ////    finalamt = finalamt + objanalysisPricing.FinalAmount;
                            ////    totalprice = totalprice + objanalysisPricing.TotalTierPrice;
                            ////}
                            ////List<ItemChargePricing> lstitemprice = View.ObjectSpace.GetObjects<ItemChargePricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ?", crtquotes.Oid)).ToList();
                            ////foreach (ItemChargePricing objitemprice in lstitemprice.ToList())
                            ////{
                            ////    ItemChargePricing objitemchareprice = os.FindObject<ItemChargePricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ?", crtquotes.Oid));
                            ////    if (objitemchareprice != null)
                            ////    {
                            ////        objitemchareprice.Amount = objitemprice.NpUnitPrice * objitemprice.Qty;
                            ////        objitemchareprice.FinalAmount = objitemprice.UnitPrice * objitemprice.Qty;
                            ////        objitemchareprice.Discount = ((objitemprice.NpUnitPrice - objitemprice.UnitPrice) / objitemprice.UnitPrice) * 100;
                            ////        finalamt = finalamt + objitemchareprice.FinalAmount;
                            ////        totalprice = totalprice + (objitemprice.UnitPrice * objitemprice.Qty);
                            ////        os.CommitChanges();
                            ////        os.Refresh();
                            ////    }
                            ////}                         
                        }

                        //NestedFrame nestedFrame = (NestedFrame)Frame;
                        //CompositeView view = nestedFrame.ViewItem.View;
                        //foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                        //{
                        //    if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                        //    {
                        //        frameContainer.Frame.View.ObjectSpace.Refresh();
                        //    }
                        //}
                        decimal finalamt = 0;
                        decimal totalprice = 0;
                        decimal disamt = 0;
                        decimal dispr = 0;
                        ListPropertyEditor lvanalysisprice = ((DetailView)Application.MainWindow.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                        if (lvanalysisprice != null && lvanalysisprice.ListView != null)
                        {
                            foreach (AnalysisPricing objanapricing in ((ListView)lvanalysisprice.ListView).CollectionSource.List)
                            {
                                finalamt = finalamt + objanapricing.FinalAmount;
                                totalprice = totalprice + (objanapricing.TotalTierPrice * objanapricing.Qty);
                            }
                        }
                        //DashboardViewItem dvitemprice = ((DetailView)Application.MainWindow.View).FindItem("DVItemChargePrice") as DashboardViewItem;
                        //if (dvitemprice != null && dvitemprice.InnerView != null)
                        //{
                        //    foreach (QuotesItemChargePrice objitempricing in ((ListView)dvitemprice.InnerView).CollectionSource.List)
                        //    {
                        //        finalamt = finalamt + objitempricing.FinalAmount;
                        //        totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                        //    }
                        //}
                        ListPropertyEditor lvitemprice = ((DetailView)Application.MainWindow.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                        if (lvitemprice != null && lvitemprice.ListView != null)
                        {
                            foreach (QuotesItemChargePrice objitempricing in ((ListView)lvitemprice.ListView).CollectionSource.List)
                            {
                                finalamt = finalamt + objitempricing.FinalAmount;
                                totalprice = totalprice + (objitempricing.Amount * objitempricing.Qty);
                            }
                        }
                        CRMQuotes crtquotes = (CRMQuotes)Application.MainWindow.View.CurrentObject;
                        if (crtquotes != null)
                        {
                            if (finalamt != 0 && totalprice != 0)
                            {
                                disamt = totalprice - finalamt;
                                //disamt = finalamt - totalprice;
                                if (disamt != 0)
                                {
                                    dispr = ((disamt) / totalprice) * 100;
                                }
                                //dispr = ((totalprice - finalamt) / totalprice) * 100;
                                //disamt = finalamt - totalprice;
                                disamt = totalprice - finalamt;
                                quotesinfo.lvDetailedPrice = Math.Round(totalprice, 2);
                                crtquotes.DetailedAmount = Math.Round(totalprice, 2);
                                crtquotes.TotalAmount = Math.Round(finalamt, 2);
                                crtquotes.QuotedAmount = Math.Round(finalamt, 2);
                                crtquotes.Discount = Math.Round(dispr, 2);
                                crtquotes.DiscountAmount = Math.Round(disamt, 2);
                            }
                            else if (finalamt == 0 && totalprice == 0)
                            {
                                crtquotes.DetailedAmount = 0;
                                crtquotes.TotalAmount = 0;
                                crtquotes.Discount = 0;
                                crtquotes.DiscountAmount = 0;
                                crtquotes.QuotedAmount = 0;
                            }
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                e.Properties["cpVisibleRowCount"] = gridView.VisibleRowCount;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void QuoteSaveAs_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "CRMQuotes_ListView_ReviewHistory" || View.Id == "CRMQuotes_ListView_PendingReview")
                {
                    if (((ListView)View).CollectionSource.GetCount() > 0)
                    {
                        if (View.SelectedObjects.Count == 1)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            CRMQuotes objNewQuotes = os.CreateObject<CRMQuotes>();
                            objNewQuotes.Status = CRMQuotes.QuoteStatus.PendingSubmission;
                            List<CRMQuotes> lstQuotes = View.SelectedObjects.Cast<CRMQuotes>().ToList();
                            List<Guid> lstOid = lstQuotes.Select(i => i.Oid).Distinct().ToList();
                            Guid objQuotes = lstQuotes.OrderByDescending(i => i.QuoteID).Select(i => i.Oid).FirstOrDefault();
                            if (lstQuotes != null)
                            {
                                CRMQuotes oldQuotes = os.GetObjectByKey<CRMQuotes>(objQuotes);
                                objNewQuotes.Title = oldQuotes.Title;
                                objNewQuotes.IsProspect = oldQuotes.IsProspect;
                                objNewQuotes.ExpirationDate = oldQuotes.ExpirationDate;
                                objNewQuotes.ProjectName = oldQuotes.ProjectName;
                                objNewQuotes.QuotedDate = DateTime.Now;
                                objNewQuotes.QuotedAmount = oldQuotes.QuotedAmount;
                                objNewQuotes.TotalAmount = oldQuotes.TotalAmount;
                                objNewQuotes.Remark = oldQuotes.Remark;
                                objNewQuotes.DetailedAmount = oldQuotes.DetailedAmount;
                                objNewQuotes.DiscountAmount = oldQuotes.DiscountAmount;
                                objNewQuotes.Discount = oldQuotes.Discount;
                                //objNewQuotes.FinalAmount = oldQuotes.FinalAmount;
                                objNewQuotes.EmailID = oldQuotes.EmailID;
                                objNewQuotes.CellPhone = oldQuotes.CellPhone;
                                objNewQuotes.OtherPhone = oldQuotes.OtherPhone;
                                objNewQuotes.BillStreet1 = oldQuotes.BillStreet1;
                                objNewQuotes.BillStreet2 = oldQuotes.BillStreet2;
                                objNewQuotes.BillZipCode = oldQuotes.BillZipCode;
                                objNewQuotes.BillState = oldQuotes.BillState;
                                objNewQuotes.BillCountry = oldQuotes.BillCountry;
                                objNewQuotes.BillCity = oldQuotes.BillCity;
                                objNewQuotes.SameAddress = oldQuotes.SameAddress;
                                objNewQuotes.ShipStreet1 = oldQuotes.ShipStreet1;
                                objNewQuotes.ShipStreet2 = oldQuotes.ShipStreet2;
                                objNewQuotes.ShipZipCode = oldQuotes.ShipZipCode;
                                objNewQuotes.ShipState = oldQuotes.ShipState;
                                objNewQuotes.ShipCountry = oldQuotes.ShipCountry;
                                objNewQuotes.ShipCity = oldQuotes.ShipCity;
                                if (oldQuotes.Client != null)
                                {
                                    objNewQuotes.Client = os.GetObjectByKey<Customer>(oldQuotes.Client.Oid);
                                }
                                if (oldQuotes.ProspectClient != null)
                                {
                                    objNewQuotes.ProspectClient = os.GetObjectByKey<CRMProspects>(oldQuotes.ProspectClient.Oid);
                                }
                                if (oldQuotes.TAT != null)
                                {
                                    objNewQuotes.TAT = os.GetObjectByKey<TurnAroundTime>(oldQuotes.TAT.Oid);
                                }
                                if (oldQuotes.PaymentMethod != null)
                                {
                                    objNewQuotes.PaymentMethod = os.GetObjectByKey<PaymentMethod>(oldQuotes.PaymentMethod.Oid);
                                }
                                if (oldQuotes.ProjectID != null)
                                {
                                    objNewQuotes.ProjectID = os.GetObjectByKey<Project>(oldQuotes.ProjectID.Oid);
                                }
                                if (oldQuotes.QuotedBy != null)
                                {
                                    objNewQuotes.QuotedBy = os.GetObjectByKey<Employee>(oldQuotes.QuotedBy.Oid);
                                }
                                if (oldQuotes.PrimaryContact != null)
                                {
                                    objNewQuotes.PrimaryContact = os.GetObjectByKey<Contact>(oldQuotes.PrimaryContact.Oid);
                                }
                                if (oldQuotes.AnalysisPricing != null && oldQuotes.AnalysisPricing.Count > 0)
                                {
                                    List<AnalysisPricing> lstanalysisprice = View.ObjectSpace.GetObjects<AnalysisPricing>(CriteriaOperator.Parse("[CRMQuotes] = ?", oldQuotes.Oid)).ToList();
                                    //List<Guid> lstAPOid = lstanalysisprice.Select(i => i.Oid).Distinct().ToList();
                                    //Guid objAnalysis = lstanalysisprice.OrderByDescending(i => i.CRMQuotes).Select(i => i.Oid).FirstOrDefault();
                                    foreach (AnalysisPricing objAnalysis in lstanalysisprice.ToList())
                                    {
                                        if (lstanalysisprice != null)
                                        {
                                            AnalysisPricing objNewAP = os.CreateObject<AnalysisPricing>();
                                            //IObjectSpace osAP = Application.CreateObjectSpace();
                                            AnalysisPricing oldAP = os.GetObjectByKey<AnalysisPricing>(objAnalysis.Oid);
                                            if (oldAP != null)
                                            {
                                                objNewAP.TestDescription = oldAP.TestDescription;
                                                if (oldAP.Matrix != null)
                                                {
                                                    objNewAP.Matrix = os.GetObjectByKey<Matrix>(oldAP.Matrix.Oid);
                                                }
                                                if (oldAP.SampleMatrix != null)
                                                {
                                                    objNewAP.SampleMatrix = os.GetObjectByKey<VisualMatrix>(oldAP.SampleMatrix.Oid);
                                                }
                                                if (oldAP.Test != null)
                                                {
                                                    objNewAP.Test = os.GetObjectByKey<TestMethod>(oldAP.Test.Oid);
                                                }
                                                objNewAP.PriceCode = oldAP.PriceCode;
                                                objNewAP.IsGroup = oldAP.IsGroup;
                                                if (oldAP.Method != null)
                                                {
                                                    objNewAP.Method = os.GetObjectByKey<Method>(oldAP.Method.Oid);
                                                }
                                                if (oldAP.Component != null)
                                                {
                                                    objNewAP.Component = os.GetObjectByKey<Component>(oldAP.Component.Oid);
                                                }
                                                objNewAP.Parameter = oldAP.Parameter;
                                                objNewAP.ChargeType = oldAP.ChargeType;
                                                if (oldAP.TAT != null)
                                                {
                                                    objNewAP.TAT = os.GetObjectByKey<TurnAroundTime>(oldAP.TAT.Oid);
                                                }
                                                if (oldAP.Priority != null)
                                                {
                                                    objNewAP.Priority = os.GetObjectByKey<Priority>(oldAP.Priority.Oid);
                                                }
                                                objNewAP.Qty = oldAP.Qty;
                                                objNewAP.UnitPrice = oldAP.UnitPrice;
                                                objNewAP.TierNo = oldAP.TierNo;
                                                objNewAP.From = oldAP.From;
                                                objNewAP.To = oldAP.To;
                                                objNewAP.TotalTierPrice = oldAP.TotalTierPrice;
                                                objNewAP.Discount = oldAP.Discount;
                                                objNewAP.FinalAmount = oldAP.FinalAmount;
                                                objNewAP.Remark = oldAP.Remark;
                                                objNewAP.Sort = oldAP.Sort;
                                                objNewAP.CRMQuotes = os.GetObjectByKey<CRMQuotes>(objNewQuotes.Oid);
                                                objNewQuotes.AnalysisPricing.Add(objNewAP);
                                                //((ListView)View).CollectionSource.Add(objNewAP);
                                            }
                                        }
                                    }
                                }
                                if (oldQuotes.QuotesItemChargePrice != null && oldQuotes.QuotesItemChargePrice.Count > 0)
                                {
                                    List<QuotesItemChargePrice> lstItemCharge = View.ObjectSpace.GetObjects<QuotesItemChargePrice>(CriteriaOperator.Parse("[CRMQuotes] = ?", oldQuotes.Oid)).ToList();
                                    foreach (QuotesItemChargePrice objItemCharge in lstItemCharge.ToList())
                                    {
                                        if (lstItemCharge != null)
                                        {
                                            QuotesItemChargePrice objNewIC = os.CreateObject<QuotesItemChargePrice>();
                                            QuotesItemChargePrice oldIC = os.GetObjectByKey<QuotesItemChargePrice>(objItemCharge.Oid);
                                            if (oldIC != null)
                                            {
                                                objNewIC.UnitPrice = oldIC.UnitPrice;
                                                objNewIC.NpUnitPrice = oldIC.NpUnitPrice;
                                                objNewIC.Qty = oldIC.Qty;
                                                objNewIC.Amount = oldIC.Amount;
                                                objNewIC.Discount = oldIC.Discount;
                                                objNewIC.FinalAmount = oldIC.FinalAmount;
                                                objNewIC.Remark = oldIC.Remark;
                                                objNewIC.CRMQuotes = os.GetObject<CRMQuotes>(objNewQuotes);
                                                if (oldIC.ItemPrice != null)
                                                {
                                                    objNewIC.ItemPrice = os.GetObjectByKey<ItemChargePricing>(oldIC.ItemPrice.Oid);
                                                }
                                                //objNewIC.Quotes = os.GetObjectByKey<CRMQuotes>(objNewQuotes.Oid);
                                                objNewIC.Description = oldIC.Description;
                                                objNewQuotes.QuotesItemChargePrice.Add(objNewIC);
                                                //((ListView)View).CollectionSource.Add(objNewIC);
                                            }
                                        }
                                    }
                                    //List<Guid> lstICOid = lstItemCharge.Select(i => i.Oid).Distinct().ToList();
                                    //Guid objItemCharge = lstItemCharge.OrderByDescending(i => i.CRMQuotes).Select(i => i.Oid).FirstOrDefault();

                                }

                                if (oldQuotes.Attachments != null && oldQuotes.Attachments.Count > 0)
                                {
                                    List<Attachment> lstAttachment = View.ObjectSpace.GetObjects<Attachment>(CriteriaOperator.Parse("[CRMQuotes] = ?", oldQuotes.Oid)).ToList();
                                    //List<Guid> lstAttachmentOid = lstAttachment.Select(i => i.Oid).Distinct().ToList();
                                    //Guid objAttachment = lstAttachment.OrderByDescending(i => i.CRMQuotes).Select(i => i.Oid).FirstOrDefault();
                                    foreach (Attachment objAttachment in lstAttachment.ToList())
                                    {
                                        if (lstAttachment != null)
                                        {
                                            Attachment objNewAttachment = os.CreateObject<Attachment>();
                                            Attachment oldAttachment = os.GetObjectByKey<Attachment>(objAttachment.Oid);
                                            if (oldAttachment != null)
                                            {
                                                objNewAttachment.Name = oldAttachment.Name;
                                                objNewAttachment.Category = oldAttachment.Category;
                                                objNewAttachment.Date = oldAttachment.Date;
                                                if (oldAttachment.Operator != null)
                                                {
                                                    objNewAttachment.Operator = os.GetObjectByKey<Employee>(oldAttachment.Operator.Oid);
                                                }
                                                objNewAttachment.Comment = oldAttachment.Comment;
                                                objNewAttachment.CRMQuotes = os.GetObjectByKey<CRMQuotes>(objNewQuotes.Oid);
                                                objNewAttachment.Attachments = oldAttachment.Attachments;
                                                objNewQuotes.Attachments.Add(objNewAttachment);
                                                // ((ListView)View).CollectionSource.Add(objNewAttachment);
                                            }

                                        }
                                    }
                                }
                                if (oldQuotes.Note != null && oldQuotes.Note.Count > 0)
                                {
                                    List<Notes> lstNotes = View.ObjectSpace.GetObjects<Notes>(CriteriaOperator.Parse("[CRMQuotes] = ?", oldQuotes.Oid)).ToList();
                                    foreach (Notes objNotes in lstNotes.ToList())
                                    {
                                        Notes oldNotes = os.GetObjectByKey<Notes>(objNotes.Oid);
                                        if (oldNotes != null)
                                        {
                                            Notes objNewNotes = os.CreateObject<Notes>();
                                            objNewNotes.Title = oldNotes.Title;
                                            objNewNotes.Attachment = oldNotes.Attachment;
                                            objNewNotes.Text = oldNotes.Text;
                                            if (oldNotes.Author != null)
                                            {
                                                objNewNotes.Author = os.GetObjectByKey<Employee>(oldNotes.Author.Oid);
                                            }
                                            objNewNotes.Date = oldNotes.Date;
                                            objNewNotes.CRMQuotes = os.GetObjectByKey<CRMQuotes>(objNewQuotes.Oid);
                                            objNewNotes.FollowUpDate = oldNotes.FollowUpDate;
                                            objNewQuotes.Note.Add(objNewNotes);
                                            //((ListView)View).CollectionSource.Add(objNewNotes);
                                        }
                                    }
                                    //List<Guid> lstNotesOid = lstNotes.Select(i => i.Oid).Distinct().ToList();
                                    //Guid objNotes = lstNotes.OrderByDescending(i => i.CRMQuotes).Select(i => i.Oid).FirstOrDefault();
                                    //if (lstNotes != null)
                                    //{
                                    //    Notes oldNotes = os.GetObjectByKey<Notes>(objNotes);
                                    //    if (oldNotes != null)
                                    //    {
                                    //        objNewNotes.Title = oldNotes.Title;
                                    //        objNewNotes.Attachment = oldNotes.Attachment;
                                    //        objNewNotes.Text = oldNotes.Text;
                                    //        if (oldNotes.Author != null)
                                    //        {
                                    //            objNewNotes.Author = os.GetObjectByKey<Employee>(oldNotes.Author.Oid);
                                    //        }
                                    //        objNewNotes.Date = oldNotes.Date;
                                    //        objNewNotes.CRMQuotes = os.GetObjectByKey<CRMQuotes>(objNewQuotes.Oid);
                                    //        objNewNotes.FollowUpDate = oldNotes.FollowUpDate;
                                    //        objNewQuotes.Note.Add(objNewNotes);
                                    //        //((ListView)View).CollectionSource.Add(objNewNotes);
                                    //    }
                                    //}
                                }
                            }
                            DetailView dv = Application.CreateDetailView(os, "CRMQuotes_DetailView", true, objNewQuotes);
                            dv.ViewEditMode = ViewEditMode.Edit;
                            Frame.SetView(dv);
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
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
    }
}
