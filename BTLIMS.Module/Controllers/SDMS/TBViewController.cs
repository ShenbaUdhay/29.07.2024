using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Office.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Web.ASPxSpreadsheet;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SDMS;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.SDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LDM.Module.Web.Controllers.Template_Builder
{
    public partial class TBViewController : ViewController, IXafCallbackHandler
    {
        ListViewProcessCurrentObjectController tar;
        //AppearanceController appearanceController;
        public static Dictionary<int, string> diIndexToColumn = new Dictionary<int, string>();
        Templateinfo templateinfo = new Templateinfo();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        MessageTimer timer = new MessageTimer();
        DataTable dtSDMSTemplateInfo = new DataTable();
        bool IsLVCreate = false;
        string EditTemplateCaption = string.Empty;
        public TBViewController()
        {
            InitializeComponent();
            TargetViewId = "SpreadSheetBuilder_FieldSetUp_ListView_RawData_Selected;" + "SpreadSheetBuilder_ScientificData_ListView_RawData_Available;"
                + "SpreadSheetBuilder_FieldSetUp_ListView_Calibration_Selected;" + "SpreadSheetBuilder_ScientificData_ListView_Calibration_Available;"
                + "SpreadSheetBuilder_FieldSetUp_ListView_Final;" + "SpreadSheetBuilder_FieldSetUp_ListView_Final_Calibration;" + "SpreadSheetBuilder_DataTransfer_ListView;"
                + "SpreadSheetBuilder_DataTransfer_DetailView;" + "SpreadSheetBuilder_DataParsing_ListView;" + "SpreadSheetBuilder_TemplateInfo_DetailView_Builder;"
                + "SpreadSheetBuilder_Header_ListView;" + "SpreadSheetBuilder_Detail_ListView;" + "SpreadSheetBuilder_CHeader_ListView;" + "SpreadSheetBuilder_CDetail_ListView;"
                + "Testparameter_LookupListView_TBAvailableTest;" + "Testparameter_LookupListView_TBSelectedParameter;" + "Testparameter_LookupListView_TBSelectedTest;"
                + "TBTest;" + "DummyClass_ListView_TemplateInfo;" + "DummyClass_ListView_TemplateInfo_DataCenter;";
            FullScreen.TargetViewId = "SpreadSheetBuilder_TemplateInfo_DetailView_Builder";
            TestSelectionAdd.TargetViewId = TestSelectionRemove.TargetViewId = TestSelectionSave.TargetViewId = "TBTest";
            EditTemplate.TargetViewId = "DummyClass_ListView_TemplateInfo;";
            AddTB.TargetViewId = "SpreadSheetBuilder_DataParsing_ListView;" + "SpreadSheetBuilder_DataTransfer_ListView;";
            RemoveTB.TargetViewId = "SpreadSheetBuilder_DataParsing_ListView;" + "SpreadSheetBuilder_DataTransfer_ListView;" + "SpreadSheetBuilder_Header_ListView;" + "SpreadSheetBuilder_Detail_ListView;" + "SpreadSheetBuilder_CHeader_ListView;" + "SpreadSheetBuilder_CDetail_ListView;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "DummyClass_ListView_TemplateInfo")
                {
                    EditTemplate.Caption = "New";
                    EditTemplateCaption = "New";
                    EditTemplate.ImageName = "Action_New";
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        if (gridlisteditor.Grid.Columns["InlineEditCommandColumn"] != null)
                        {
                            gridlisteditor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                        }
                        if (gridlisteditor.Grid.Columns["Edit"] != null)
                        {
                            gridlisteditor.Grid.Columns["Edit"].Visible = false;
                        }
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                    IsLVCreate = true;
                    Frame.GetController<RefreshController>().RefreshAction.Executing += RefreshAction_Executing;
                }
                if (View.Id == "DummyClass_ListView_TemplateInfo_DataCenter")
                {
                    IsLVCreate = true;
                }
                if (View.Id != "DummyClass_ListView_TemplateInfo")
                {
                    ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                }
                if (View is ListView && View.Id != "DummyClass_ListView_TemplateInfo" && View.Id != "Testparameter_LookupListView_TBAvailableTest" && View.Id != "Testparameter_LookupListView_TBSelectedParameter" && View.Id != "Testparameter_LookupListView_TBSelectedTest")
                {
                    //Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
                else if (View.Id == "SpreadSheetBuilder_TemplateInfo_DetailView_Builder")
                {
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("SaveAndCloseAction", false);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("ShowSaveAndNewAction", false);
                    Frame.GetController<ModificationsController>().SaveAction.Executing += SaveAction_Executing;
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Active.SetItemValue("NewObjectAction", false);
                    Frame.GetController<RefreshController>().RefreshAction.Active.SetItemValue("RefreshAction", false);
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                    if (View.ObjectSpace.IsNewObject(View.CurrentObject))
                    {
                        Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("DeleteAction", false);
                    }
                    Frame.GetController<WebModificationsController>().EditAction.Executing += EditAction_Executing;
                    //appearanceController = Frame.GetController<AppearanceController>();
                    //if (appearanceController != null)
                    //{
                    //    appearanceController.CustomApplyAppearance += AppearanceController_CustomApplyAppearance;
                    //}
                    //WebWindow.CurrentRequestWindow.RegisterClientScript("alrt", "document.getElementById('separatorButton').setAttribute('onclick', 'tbuirefresh();')");
                    //FullScreen.SetClientScript("tbuirefresh();", false);
                    ASPxSpreadsheetPropertyEditor Spreadsheet = ((DetailView)View).FindItem("SpreadSheet") as ASPxSpreadsheetPropertyEditor;
                    if (Spreadsheet != null)
                    {
                        Spreadsheet.ControlCreated += Spreadsheet_ControlCreated;
                    }
                    ASPxSpreadsheetPropertyEditor SpreadSheet_PD = ((DetailView)View).FindItem("SpreadSheet_PD") as ASPxSpreadsheetPropertyEditor;
                    if (Spreadsheet != null)
                    {
                        SpreadSheet_PD.ControlCreated += SpreadSheet_PD_ControlCreated;
                    }
                    ASPxStringPropertyEditor Test = ((DetailView)View).FindItem("Test") as ASPxStringPropertyEditor;
                    if (Test != null)
                    {
                        Test.ControlCreated += Test_ControlCreated;
                    }
                    if (diIndexToColumn.Count == 0)
                    {
                        intializeIndexColumns();
                    }
                    ((WebLayoutManager)((DetailView)View).LayoutManager).PageControlCreated += TBViewController_PageControlCreated;
                }
                if (Application.MainWindow.View is DetailView && ((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.View)
                {
                    AddTB.Active.SetItemValue("AddTB", false);
                    RemoveTB.Active.SetItemValue("RemoveTB", false);
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

        private void EditAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AddTB.Active.SetItemValue("AddTB", true);
            RemoveTB.Active.SetItemValue("RemoveTB", true);
        }

        private void TBViewController_PageControlCreated(object sender, PageControlCreatedEventArgs e)
        {
            if (e.Model.Id == "Tabs")
            {
                e.PageControl.Callback += PageControl_Callback;
                e.PageControl.Init += PageControl_Init;
            }
        }

        private void PageControl_Init(object sender, EventArgs e)
        {
            ASPxPageControl pageControl = (ASPxPageControl)sender;
            ClientSideEventsHelper.AssignClientHandlerSafe(pageControl, "ActiveTabChanged", "function(s, e) { s.PerformCallback('TabChanged'); }", "Spreadsheettemplate4");
        }

        private void PageControl_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "TabChanged")
            {
                TabPage activePage = ((ASPxPageControl)sender).ActiveTabPage;
                if (activePage.Text == "Parsing Data")
                {
                    SpreadSheetBuilder_TemplateInfo templateInfo = (SpreadSheetBuilder_TemplateInfo)View.CurrentObject;
                    if (templateInfo != null)
                    {
                        ASPxSpreadsheetPropertyEditor aSPxSpreadsheet = ((DetailView)View).FindItem("SpreadSheet") as ASPxSpreadsheetPropertyEditor;
                        if (aSPxSpreadsheet != null && aSPxSpreadsheet.ASPxSpreadsheetControl != null)
                        {
                            templateInfo.SpreadSheet_PD = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.SaveDocument(DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                        }
                        else
                        {
                            templateInfo.SpreadSheet_PD = templateInfo.SpreadSheet;
                        }
                    }
                }
            }
        }

        private void DeleteAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                DashboardViewItem RawDataSelected = (DashboardViewItem)((DetailView)View).FindItem("FieldInfoRawDataSelected");
                DashboardViewItem CalibrationSelected = (DashboardViewItem)((DetailView)View).FindItem("FieldInfoCalibrationSelected");
                DashboardViewItem DataParsing = (DashboardViewItem)((DetailView)View).FindItem("DataParsing");
                DashboardViewItem DataTransfer = (DashboardViewItem)((DetailView)View).FindItem("DataTransfer");
                DashboardViewItem Header = (DashboardViewItem)((DetailView)View).FindItem("Header");
                DashboardViewItem Detail = (DashboardViewItem)((DetailView)View).FindItem("Detail");
                DashboardViewItem CHeader = (DashboardViewItem)((DetailView)View).FindItem("CHeader");
                DashboardViewItem CDetail = (DashboardViewItem)((DetailView)View).FindItem("CDetail");

                SpreadSheetBuilder_TemplateInfo templateInfo = (SpreadSheetBuilder_TemplateInfo)View.CurrentObject;
                if (templateInfo != null)
                {
                    int templateid = templateInfo.TemplateID;
                    if (templateid != 0)
                    {
                        IList<SpreadSheetBuilder_TestParameter> stestParameter = ObjectSpace.GetObjects<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TemplateID]=?", templateid));
                        foreach (SpreadSheetBuilder_TestParameter _TestParameter in stestParameter.ToList())
                        {
                            View.ObjectSpace.Delete(_TestParameter);
                        }
                        View.ObjectSpace.CommitChanges();
                        if (RawDataSelected != null)
                        {
                            if (RawDataSelected.InnerView == null)
                            {
                                RawDataSelected.CreateControl();
                                RawDataSelected.InnerView.CreateControls();
                            }
                            foreach (SpreadSheetBuilder_FieldSetUp fieldSetUp in ((ListView)RawDataSelected.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_FieldSetUp>().ToList())
                            {
                                RawDataSelected.InnerView.ObjectSpace.Delete(fieldSetUp);
                            }
                            RawDataSelected.InnerView.ObjectSpace.CommitChanges();
                        }
                        if (CalibrationSelected != null)
                        {
                            if (CalibrationSelected.InnerView == null)
                            {
                                CalibrationSelected.CreateControl();
                                CalibrationSelected.InnerView.CreateControls();
                            }
                            foreach (SpreadSheetBuilder_FieldSetUp fieldSetUp in ((ListView)CalibrationSelected.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_FieldSetUp>().ToList())
                            {
                                CalibrationSelected.InnerView.ObjectSpace.Delete(fieldSetUp);
                            }
                            CalibrationSelected.InnerView.ObjectSpace.CommitChanges();
                        }
                        if (DataParsing != null)
                        {
                            if (DataParsing.InnerView == null)
                            {
                                DataParsing.CreateControl();
                                DataParsing.InnerView.CreateControls();
                            }
                            foreach (SpreadSheetBuilder_DataParsing dataParsing in ((ListView)DataParsing.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_DataParsing>().ToList())
                            {
                                DataParsing.InnerView.ObjectSpace.Delete(dataParsing);
                            }
                            DataParsing.InnerView.ObjectSpace.CommitChanges();
                        }
                        if (DataTransfer != null)
                        {
                            if (DataTransfer.InnerView == null)
                            {
                                DataTransfer.CreateControl();
                                DataTransfer.InnerView.CreateControls();
                            }
                            foreach (SpreadSheetBuilder_DataTransfer datatransfer in ((ListView)DataTransfer.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_DataTransfer>().ToList())
                            {
                                if (DataTransfer.InnerView.ObjectSpace.IsNewObject(datatransfer))
                                {
                                    DataTransfer.InnerView.ObjectSpace.Delete(datatransfer);
                                }
                            }
                            DataTransfer.InnerView.ObjectSpace.CommitChanges();
                        }
                        if (Header != null)
                        {
                            if (Header.InnerView == null)
                            {
                                Header.CreateControl();
                                Header.InnerView.CreateControls();
                            }
                            foreach (SpreadSheetBuilder_Header header in ((ListView)Header.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_Header>().ToList())
                            {
                                if (Header.InnerView.ObjectSpace.IsNewObject(header))
                                {
                                    Header.InnerView.ObjectSpace.Delete(header);
                                }
                            }
                            Header.InnerView.ObjectSpace.CommitChanges();
                        }
                        if (Detail != null)
                        {
                            if (Detail.InnerView == null)
                            {
                                Detail.CreateControl();
                                Detail.InnerView.CreateControls();
                            }
                            foreach (SpreadSheetBuilder_Detail detail in ((ListView)Detail.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_Detail>().ToList())
                            {
                                Detail.InnerView.ObjectSpace.Delete(detail);
                            }
                            Detail.InnerView.ObjectSpace.CommitChanges();
                            Detail.InnerView.ObjectSpace.Refresh();
                        }
                        if (CHeader != null)
                        {
                            if (CHeader.InnerView == null)
                            {
                                CHeader.CreateControl();
                                CHeader.InnerView.CreateControls();
                            }
                            foreach (SpreadSheetBuilder_CHeader cheader in ((ListView)CHeader.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_CHeader>().ToList())
                            {
                                CHeader.InnerView.ObjectSpace.Delete(cheader);
                            }
                            CHeader.InnerView.ObjectSpace.CommitChanges();
                        }
                        if (CDetail != null)
                        {
                            if (CDetail.InnerView == null)
                            {
                                CDetail.CreateControl();
                                CDetail.InnerView.CreateControls();
                            }
                            foreach (SpreadSheetBuilder_CDetail cdetail in ((ListView)CDetail.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_CDetail>().ToList())
                            {
                                CDetail.InnerView.ObjectSpace.Delete(cdetail);
                            }
                            CDetail.InnerView.ObjectSpace.CommitChanges();
                        }
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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
                if (e.PopupFrame.View.Id == "TBTest")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(1400);
                    e.Height = new System.Web.UI.WebControls.Unit(648);
                    e.Handled = true;
                }
                else if (e.PopupFrame.View.Id == "SpreadSheetBuilder_DataTransfer_DetailView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(1400);
                    e.Height = new System.Web.UI.WebControls.Unit(780);
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Test_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxTextBox textBox = (ASPxTextBox)((ASPxStringPropertyEditor)sender).Editor;
                ICallbackManagerHolder holder = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                holder.CallbackManager.RegisterHandler("TBTest", this);
                if (textBox != null)
                {
                    textBox.ClientSideEvents.GotFocus = @"function(s,e){ RaiseXafCallback(globalCallbackControl, 'TBTest', 'TBTest', '', false); }";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SpreadSheet_PD_ControlCreated(object sender, EventArgs e)
        {
            ASPxSpreadsheet spreadsheet = ((ASPxSpreadsheetPropertyEditor)sender).ASPxSpreadsheetControl;
            ClientSideEventsHelper.AssignClientHandlerSafe(spreadsheet, "CellBeginEdit", "function(s,e) { if(s.cpisedit != null) { e.cancel = s.cpisedit; } }", "Spreadsheettemplate3");
            spreadsheet.RibbonMode = SpreadsheetRibbonMode.None;
        }

        private void Spreadsheet_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxSpreadsheet spreadsheet = ((ASPxSpreadsheetPropertyEditor)sender).ASPxSpreadsheetControl;
                spreadsheet.RibbonTabs.Add("SDMS");
                spreadsheet.RibbonTabs.FindByName("SDMS").Index = 0;
                spreadsheet.ActiveTabIndex = 0;
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.Add("group0");
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Text = "  ";
                var ItemScript = new RibbonButtonItem() { Name = "Script", Text = "Script", Size = RibbonItemSize.Small };
                ItemScript.SmallImage.Url = "~/Images/Script.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.Add(ItemScript);
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.Add("group1");
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Text = "  ";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.Add(new SRFullScreenCommand() { Name = "FullScreen", Text = "Full Screen" });
                ICallbackManagerHolder holder = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                holder.CallbackManager.RegisterHandler("itemname", this);
                string customCommandClickHandler = @"function(s, e) 
                {                                                             
                    RaiseXafCallback(globalCallbackControl, 'itemname', e.commandName + '|' + s.isInFullScreenMode, '', false);                                                              
                }";
                ClientSideEventsHelper.AssignClientHandlerSafe(spreadsheet, "CustomCommandExecuted", customCommandClickHandler, "Spreadsheettemplate1");
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
                DashboardViewItem RawDataSelected = (DashboardViewItem)((DetailView)View).FindItem("FieldInfoRawDataSelected");
                DashboardViewItem CalibrationSelected = (DashboardViewItem)((DetailView)View).FindItem("FieldInfoCalibrationSelected");
                DashboardViewItem DataParsing = (DashboardViewItem)((DetailView)View).FindItem("DataParsing");
                DashboardViewItem DataTransfer = (DashboardViewItem)((DetailView)View).FindItem("DataTransfer");
                DashboardViewItem Header = (DashboardViewItem)((DetailView)View).FindItem("Header");
                DashboardViewItem Detail = (DashboardViewItem)((DetailView)View).FindItem("Detail");
                DashboardViewItem CHeader = (DashboardViewItem)((DetailView)View).FindItem("CHeader");
                DashboardViewItem CDetail = (DashboardViewItem)((DetailView)View).FindItem("CDetail");

                SpreadSheetBuilder_TemplateInfo templateInfo = (SpreadSheetBuilder_TemplateInfo)View.CurrentObject;
                if (templateInfo != null)
                {
                    ASPxSpreadsheetPropertyEditor aSPxSpreadsheet = ((DetailView)View).FindItem("SpreadSheet") as ASPxSpreadsheetPropertyEditor;
                    if (aSPxSpreadsheet != null && aSPxSpreadsheet.ASPxSpreadsheetControl != null)
                    {
                        IWorkbook objworkbook = aSPxSpreadsheet.ASPxSpreadsheetControl.Document;
                        applymailmerge(templateInfo, objworkbook);
                        if (string.IsNullOrEmpty(templateInfo.HeaderRange) && objworkbook.Worksheets[0].DefinedNames.Contains("HEADERRANGE"))
                        {
                            templateInfo.HeaderRange = objworkbook.Worksheets[0]["HEADERRANGE"].GetReferenceA1().Replace("$", "");
                        }
                        if (string.IsNullOrEmpty(templateInfo.DetailRange) && objworkbook.Worksheets[0].DefinedNames.Contains("DETAILRANGE"))
                        {
                            templateInfo.DetailRange = objworkbook.Worksheets[0]["DETAILRANGE"].GetReferenceA1().Replace("$", "");
                        }
                        if (objworkbook.Worksheets.Count > 1)
                        {
                            if (string.IsNullOrEmpty(templateInfo.CHeaderRange) && objworkbook.Worksheets[1].DefinedNames.Contains("CHEADER"))
                            {
                                templateInfo.CHeaderRange = objworkbook.Worksheets[1]["CHEADER"].GetReferenceA1().Replace("$", "");
                            }
                            if (string.IsNullOrEmpty(templateInfo.CDetailRange) && objworkbook.Worksheets[1].DefinedNames.Contains("CDETAIL"))
                            {
                                templateInfo.CDetailRange = objworkbook.Worksheets[1]["CDETAIL"].GetReferenceA1().Replace("$", "");
                            }
                        }
                        templateInfo.SpreadSheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.SaveDocument(DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                    }
                    View.ObjectSpace.CommitChanges();
                    View.ObjectSpace.Refresh();
                    int templateid = templateInfo.TemplateID;
                    if (templateid != 0)
                    {
                        if (SRInfo.lstTestParameter != null)
                        {
                            foreach (Guid objguid in SRInfo.lstTestParameter)
                            {
                                Testparameter testParameter = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(objguid.ToString())));
                                if (testParameter != null)
                                {
                                    SpreadSheetBuilder_TestParameter stestParameter = ObjectSpace.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID]=? and [TestParameterID]=? and [TemplateID]=?", testParameter.TestMethod.Oid, testParameter.Oid, templateid));
                                    if (stestParameter == null)
                                    {
                                        SpreadSheetBuilder_TestParameter _TestParameter = View.ObjectSpace.CreateObject<SpreadSheetBuilder_TestParameter>();
                                        _TestParameter.TemplateID = templateid;
                                        _TestParameter.TestMethodID = testParameter.TestMethod.Oid;
                                        _TestParameter.TestParameterID = testParameter.Oid;
                                        //Parameter;
                                    }
                                }
                            }
                            if (SRInfo.lstRemoveTestParameter != null)
                            {
                                foreach (Guid objguid in SRInfo.lstRemoveTestParameter)
                                {
                                    if (View.ObjectSpace.IsNewObject(objguid))
                                    {
                                        View.ObjectSpace.RemoveFromModifiedObjects(objguid);
                                    }
                                    else
                                    {
                                        View.ObjectSpace.Delete(objguid);
                                    }
                                }
                            }
                            View.ObjectSpace.CommitChanges();
                        }
                        if (RawDataSelected != null && RawDataSelected.InnerView != null)
                        {
                            foreach (SpreadSheetBuilder_FieldSetUp fieldSetUp in ((ListView)RawDataSelected.InnerView).CollectionSource.List)
                            {
                                if (RawDataSelected.InnerView.ObjectSpace.IsNewObject(fieldSetUp))
                                {
                                    fieldSetUp.TemplateID = templateid;
                                    fieldSetUp.uqID = 0;
                                }
                            }
                            RawDataSelected.InnerView.ObjectSpace.CommitChanges();
                            RawDataSelected.InnerView.ObjectSpace.Refresh();
                        }
                        if (CalibrationSelected != null && CalibrationSelected.InnerView != null)
                        {
                            foreach (SpreadSheetBuilder_FieldSetUp fieldSetUp in ((ListView)CalibrationSelected.InnerView).CollectionSource.List)
                            {
                                if (CalibrationSelected.InnerView.ObjectSpace.IsNewObject(fieldSetUp))
                                {
                                    fieldSetUp.TemplateID = templateid;
                                    fieldSetUp.uqID = 0;
                                }
                            }
                            CalibrationSelected.InnerView.ObjectSpace.CommitChanges();
                            CalibrationSelected.InnerView.ObjectSpace.Refresh();
                        }
                        if (DataParsing != null && DataParsing.InnerView != null)
                        {
                            foreach (SpreadSheetBuilder_DataParsing dataParsing in ((ListView)DataParsing.InnerView).CollectionSource.List)
                            {
                                if (DataParsing.InnerView.ObjectSpace.IsNewObject(dataParsing))
                                {
                                    dataParsing.TemplateID = DataParsing.InnerView.ObjectSpace.GetObjectByKey<SpreadSheetBuilder_TemplateInfo>(((SpreadSheetBuilder_TemplateInfo)View.CurrentObject).TemplateID);
                                    dataParsing.uqID = 0;
                                }
                            }
                            DataParsing.InnerView.ObjectSpace.CommitChanges();
                            DataParsing.InnerView.ObjectSpace.Refresh();
                        }
                        if (DataTransfer != null && DataTransfer.InnerView != null)
                        {
                            foreach (SpreadSheetBuilder_DataTransfer datatransfer in ((ListView)DataTransfer.InnerView).CollectionSource.List)
                            {
                                if (DataTransfer.InnerView.ObjectSpace.IsNewObject(datatransfer))
                                {
                                    datatransfer.TemplateID = templateid;
                                    datatransfer.uqID = 0;
                                }
                            }
                            DataTransfer.InnerView.ObjectSpace.CommitChanges();
                            DataTransfer.InnerView.ObjectSpace.Refresh();
                        }
                        if (Header != null && Header.InnerView != null)
                        {
                            foreach (SpreadSheetBuilder_Header header in ((ListView)Header.InnerView).CollectionSource.List)
                            {
                                if (Header.InnerView.ObjectSpace.IsNewObject(header))
                                {
                                    header.TemplateID = templateid;
                                    header.uqID = 0;
                                }
                            }
                            Header.InnerView.ObjectSpace.CommitChanges();
                            Header.InnerView.ObjectSpace.Refresh();
                        }
                        if (Detail != null && Detail.InnerView != null)
                        {
                            foreach (SpreadSheetBuilder_Detail detail in ((ListView)Detail.InnerView).CollectionSource.List)
                            {
                                if (Detail.InnerView.ObjectSpace.IsNewObject(detail))
                                {
                                    detail.TemplateID = templateid;
                                    detail.uqID = 0;
                                }
                            }
                            Detail.InnerView.ObjectSpace.CommitChanges();
                            Detail.InnerView.ObjectSpace.Refresh();
                        }
                        if (CHeader != null && CHeader.InnerView != null)
                        {
                            foreach (SpreadSheetBuilder_CHeader cheader in ((ListView)CHeader.InnerView).CollectionSource.List)
                            {
                                if (CHeader.InnerView.ObjectSpace.IsNewObject(cheader))
                                {
                                    cheader.TemplateID = templateid;
                                    cheader.uqID = 0;
                                }
                            }
                            CHeader.InnerView.ObjectSpace.CommitChanges();
                            CHeader.InnerView.ObjectSpace.Refresh();
                        }
                        if (CDetail != null && CDetail.InnerView != null)
                        {
                            foreach (SpreadSheetBuilder_CDetail cdetail in ((ListView)CDetail.InnerView).CollectionSource.List)
                            {
                                if (CDetail.InnerView.ObjectSpace.IsNewObject(cdetail))
                                {
                                    cdetail.TemplateID = templateid;
                                    cdetail.uqID = 0;
                                }
                            }
                            CDetail.InnerView.ObjectSpace.CommitChanges();
                            CDetail.InnerView.ObjectSpace.Refresh();
                        }
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void AppearanceController_CustomApplyAppearance(object sender, ApplyAppearanceEventArgs e)
        //{
        //    try
        //    {
        //        if (View is DetailView)
        //        {
        //            if ((e.ItemName == "Final" || e.ItemName == "FinalCalibration") && ((DetailView)View).ViewEditMode == ViewEditMode.View)
        //            {
        //                e.AppearanceObject.Visibility = ViewItemVisibility.Hide;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void adddatatogrid(CompositeView cview, DevExpress.ExpressApp.View view, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                SpreadSheetBuilder_FieldSetUp fieldSetUp = view.ObjectSpace.CreateObject<SpreadSheetBuilder_FieldSetUp>();
                fieldSetUp.TemplateID = ((SpreadSheetBuilder_TemplateInfo)cview.CurrentObject).TemplateID;
                fieldSetUp.FieldID = view.ObjectSpace.GetObjectByKey<SpreadSheetBuilder_ScientificData>(((SpreadSheetBuilder_ScientificData)e.InnerArgs.CurrentObject).uqID);
                fieldSetUp.uqID = ((ListView)view).CollectionSource.List.Count > 0 ? ((ListView)view).CollectionSource.List.Cast<SpreadSheetBuilder_FieldSetUp>().Min(a => a.uqID) - 1 : 0;
                fieldSetUp.Caption_EN = ((SpreadSheetBuilder_ScientificData)e.InnerArgs.CurrentObject).Synonym_EN != null && ((SpreadSheetBuilder_ScientificData)e.InnerArgs.CurrentObject).Synonym_EN.Length > 0 ? ((SpreadSheetBuilder_ScientificData)e.InnerArgs.CurrentObject).Synonym_EN : fieldSetUp.FieldID.FieldName;
                fieldSetUp.Caption_CN = ((SpreadSheetBuilder_ScientificData)e.InnerArgs.CurrentObject).Synonym_CN;
                fieldSetUp.Visible = true;
                fieldSetUp.Sort = ((ListView)view).CollectionSource.List.Count + 1;
                ((ListView)view).CollectionSource.Add(fieldSetUp);
                ((ListView)view).Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Tar_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                e.Handled = true;
                NestedFrame nestedFrame = (NestedFrame)Frame;
                CompositeView cview = nestedFrame.ViewItem.View;
                if (cview != null && cview is DetailView && cview.CurrentObject != null && ((DetailView)cview).ViewEditMode == ViewEditMode.Edit)
                {
                    if (View.Id == "SpreadSheetBuilder_ScientificData_ListView_RawData_Available")
                    {
                        DashboardViewItem viewItem = (DashboardViewItem)((DetailView)cview).FindItem("FieldInfoRawDataSelected");
                        DashboardViewItem viewItem1 = (DashboardViewItem)((DetailView)cview).FindItem("Final");
                        if (viewItem != null && viewItem1 != null)
                        {
                            adddatatogrid(cview, viewItem.InnerView, e);
                            adddatatogrid(cview, viewItem1.InnerView, e);
                            ((ListView)View).CollectionSource.Remove(e.InnerArgs.CurrentObject);
                        }
                    }
                    else if (View.Id == "SpreadSheetBuilder_ScientificData_ListView_Calibration_Available")
                    {
                        DashboardViewItem viewItem = (DashboardViewItem)((DetailView)cview).FindItem("FieldInfoCalibrationSelected");
                        DashboardViewItem viewItem1 = (DashboardViewItem)((DetailView)cview).FindItem("FinalCalibration");
                        if (viewItem != null && viewItem1 != null)
                        {
                            adddatatogrid(cview, viewItem.InnerView, e);
                            adddatatogrid(cview, viewItem1.InnerView, e);
                            ((ListView)View).CollectionSource.Remove(e.InnerArgs.CurrentObject);
                        }
                    }
                    else if (View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_Final" || View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_Final_Calibration")
                    {
                        ASPxSpreadsheetPropertyEditor Spreadsheet = ((DetailView)cview).FindItem("SpreadSheet") as ASPxSpreadsheetPropertyEditor;
                        if (Spreadsheet != null)
                        {
                            IWorkbook objworkbook = Spreadsheet.ASPxSpreadsheetControl.Document;
                            CellRange range = objworkbook.Worksheets.ActiveWorksheet.Selection;
                            range.Formula = "=FIELD(\"" + ((SpreadSheetBuilder_FieldSetUp)e.InnerArgs.CurrentObject).FieldID.FieldName.ToUpper().Replace("[", "").Replace("]", "") + "\")";
                        }
                    }
                    else if (View.Id == "SpreadSheetBuilder_DataTransfer_ListView")
                    {
                        SpreadSheetBuilder_DataTransfer objdt = (SpreadSheetBuilder_DataTransfer)View.CurrentObject;
                        if (objdt != null)
                        {
                            DetailView dv = Application.CreateDetailView(View.ObjectSpace, "SpreadSheetBuilder_DataTransfer_DetailView", false, objdt);
                            dv.ViewEditMode = ViewEditMode.Edit;
                            ShowViewParameters showViewParameters = new ShowViewParameters();
                            showViewParameters.CreatedView = dv;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += Dc_Accepting;
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

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View is ListView && View.Id == "DummyClass_ListView_TemplateInfo" || View.Id == "DummyClass_ListView_TemplateInfo_DataCenter")
                {
                    if (IsLVCreate == true)
                    {
                        string tempid = "0";
                        string strtempname = string.Empty;
                        string strtemptest = string.Empty;
                        string strmethod = string.Empty;
                        string strmatrix = string.Empty;
                        string strtemptype = string.Empty;
                        string strsource = string.Empty;
                        string strtempid = string.Empty;
                        dtSDMSTemplateInfo = new DataTable();
                        dtSDMSTemplateInfo.Columns.Add("NpTemplateName");
                        dtSDMSTemplateInfo.Columns.Add("NPTest");
                        dtSDMSTemplateInfo.Columns.Add("NPMethod");
                        dtSDMSTemplateInfo.Columns.Add("NPMatrix");
                        dtSDMSTemplateInfo.Columns.Add("NPTempType");
                        dtSDMSTemplateInfo.Columns.Add("NPsource");
                        dtSDMSTemplateInfo.Columns.Add("NPTempID");
                        SelectedData result = ((XPObjectSpace)ObjectSpace).Session.ExecuteSprocParametrized("SpreadSheetBuilder_Select_SP", new SprocParameter("@TemplateID", tempid));
                        foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                        {
                            if ((string)row.Values[2] != null && (string)row.Values[3] != null && (string)row.Values[4] != null && (string)row.Values[5] != null && (int)row.Values[1] > 0)
                            {
                                dtSDMSTemplateInfo.Rows.Add(new object[] { (string)row.Values[5], (string)row.Values[2], (string)row.Values[3], (string)row.Values[4], (string)row.Values[6], (string)row.Values[7], (int)row.Values[1] });//null, null, null,
                                DummyClass objdumyclass = ObjectSpace.CreateObject<DummyClass>();
                                objdumyclass.NpTemplateName = (string)row.Values[5];
                                objdumyclass.NPTest = (string)row.Values[2];
                                objdumyclass.NPMethod = (string)row.Values[3];
                                objdumyclass.NPMatrix = (string)row.Values[4];
                                objdumyclass.NPTempType = (string)row.Values[6];
                                objdumyclass.NPsource = (string)row.Values[7];
                                objdumyclass.NPTempID = (int)row.Values[1];
                                ((ListView)View).CollectionSource.List.Add(objdumyclass);
                                IsLVCreate = false;
                            }
                        }
                    }
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("TempInfo", this);
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        string strheight = System.Web.HttpContext.Current.Request.Cookies.Get("height").Value;
                        gridlisteditor.Grid.Settings.VerticalScrollableHeight = Convert.ToInt32(strheight) - (Convert.ToInt32(strheight) * 33 / 100);
                        gridlisteditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridlisteditor.Grid.Load += Grid_Load;
                        gridlisteditor.SelectionOnDoubleClick = true;
                        gridlisteditor.Grid.ClientSideEvents.RowDblClick = @"function(s,e) { 
                            s.GetRowValues(e.visibleIndex, 'NPTempID', function(Value) {      
                                RaiseXafCallback(globalCallbackControl, 'TempInfo', 'RDblclck|'+ Value, '', false);                         
                            }); 
                        }";
                        gridlisteditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(OidValue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {
                            RaiseXafCallback(globalCallbackControl, 'TempInfo', 'Selected|'+ OidValue, '', false); 
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'TempInfo', 'UNSelected|' + OidValue, '', false);    
                         }
                        }); 
                      }                    
                    }";
                    }
                }
                if (View is ListView && View.Id != "DummyClass_ListView_TemplateInfo" && View.Id != "DummyClass_ListView_TemplateInfo_DataCenter" && View.Id != "Testparameter_LookupListView_TBAvailableTest" && View.Id != "Testparameter_LookupListView_TBSelectedParameter" && View.Id != "Testparameter_LookupListView_TBSelectedTest"
                    && View.Id != "SpreadSheetBuilder_FieldSetUp_ListView_Final" && View.Id != "SpreadSheetBuilder_FieldSetUp_ListView_Final_Calibration")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 320;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.SettingsPager.NumericButtonCount = 1;
                    gridListEditor.Grid.SettingsPager.PageSizeItemSettings.Visible = false;

                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView cview = nestedFrame.ViewItem.View;
                    if (cview != null && cview is DetailView && cview.CurrentObject != null && cview.Id != "SpreadSheetBuilder_DataTransfer_DetailView")
                    {
                        if ((View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_RawData_Selected"
                        || View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_Calibration_Selected"
                        || View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_Final"
                        || View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_Final_Calibration"
                        || View.Id == "SpreadSheetBuilder_DataParsing_ListView"
                        || View.Id == "SpreadSheetBuilder_Header_ListView"
                        || View.Id == "SpreadSheetBuilder_Detail_ListView"
                        || View.Id == "SpreadSheetBuilder_CHeader_ListView"
                        || View.Id == "SpreadSheetBuilder_CDetail_ListView"
                        || View.Id == "SpreadSheetBuilder_DataTransfer_ListView") && !((ListView)View).CollectionSource.Criteria.ContainsKey("filter"))
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TemplateID]=?", ((SpreadSheetBuilder_TemplateInfo)cview.CurrentObject).TemplateID);
                        }
                        else if (View.Id == "SpreadSheetBuilder_ScientificData_ListView_RawData_Available")
                        {
                            if (!((ListView)View).CollectionSource.Criteria.ContainsKey("filter1"))
                            {
                                filterselectedgrid(cview, View, "FieldInfoRawDataSelected");
                            }
                        }
                        else if (View.Id == "SpreadSheetBuilder_ScientificData_ListView_Calibration_Available")
                        {
                            if (!((ListView)View).CollectionSource.Criteria.ContainsKey("filter1"))
                            {
                                filterselectedgrid(cview, View, "FieldInfoCalibrationSelected");
                            }
                        }
                    }
                    if (View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_RawData_Selected" || View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_Calibration_Selected")
                    {
                        XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                        callbackManager.RegisterHandler(View.Id, this);
                        gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        gridListEditor.Grid.JSProperties["cpViewID"] = View.Id;
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e)
                            {
                                if(e.focusedColumn.fieldName == 'FieldID.uqID')
                                {
                                    e.cancel = true;
                                }
                            }";
                        gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
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
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s,e)
                            {
                                window.setTimeout(function() 
                                {                                   
                                   if(sessionStorage.getItem('PrevFocusedColumn') != null)
                                   {
                                      var precolumnname = sessionStorage.getItem('PrevFocusedColumn');      
                                      if(s.batchEditApi.HasChanges(e.visibleIndex, precolumnname))
                                      {
                                        var precolumnnvalue = s.batchEditApi.GetCellValue(e.visibleIndex, precolumnname);
                                         sessionStorage.getItem(e.cellInfo.column.fieldName)
                                        //s.batchEditApi.ResetChanges(e.visibleIndex);
                                        //sessionStorage.removeItem('PrevFocusedColumn');
                                        //sessionStorage.removeItem('CurrFocusedColumn');
                                        RaiseXafCallback(globalCallbackControl, s.cpViewID, 'Entered|'+e.visibleIndex+'|'+precolumnname+'|'+precolumnnvalue+'|'+s.cpViewID, '', false);
                                      }   
                                   }
                                }, 10);
                            }";
                    }
                    else if (View.Id == "SpreadSheetBuilder_DataParsing_ListView")
                    {
                        XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                        gridListEditor.Grid.JSProperties["cpViewID"] = View.Id;
                        callbackManager.RegisterHandler(View.Id, this);
                        gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e)
                            {
                                if(e.focusedColumn.fieldName == 'FieldName')
                                {
                                     e.cancel = true;
                                }
                            }";
                        gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                           {
                                if(e.cellInfo.column.fieldName != 'FieldName') 
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
                                }
                           }";
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s,e)
                            {
                                window.setTimeout(function() 
                                {                                   
                                   if(sessionStorage.getItem('PrevFocusedColumn') != null)
                                   {
                                      var precolumnname = sessionStorage.getItem('PrevFocusedColumn');      
                                      if(s.batchEditApi.HasChanges(e.visibleIndex, precolumnname))
                                      {
                                        var precolumnnvalue = s.batchEditApi.GetCellValue(e.visibleIndex, precolumnname);
                                        s.batchEditApi.ResetChanges(e.visibleIndex);
                                        sessionStorage.removeItem('PrevFocusedColumn');
                                        sessionStorage.removeItem('CurrFocusedColumn');
                                        RaiseXafCallback(globalCallbackControl, s.cpViewID, 'Entered|'+e.visibleIndex+'|'+precolumnname+'|'+precolumnnvalue+'|'+s.cpViewID, '', false);
                                      }   
                                   }
                                }, 10);
                            }";
                    }
                }
                else if ((View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_Final" || View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_Final_Calibration") && !((ListView)View).CollectionSource.Criteria.ContainsKey("filter"))
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView cview = nestedFrame.ViewItem.View;
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TemplateID]=?", ((SpreadSheetBuilder_TemplateInfo)cview.CurrentObject).TemplateID);
                }
                else if (View.Id == "TBTest" && SRInfo.IsTestcanFilter)
                {
                    SRInfo.IsTestcanFilter = false;
                    List<object> groups = new List<object>();
                    DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
                    DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                    DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Testparameter)))
                    {
                        string criteria = string.Empty;
                        if (SRInfo.lstdupfilterstr != null && SRInfo.lstdupfilterstr.Count > 0)
                        {
                            foreach (string test in SRInfo.lstdupfilterstr)
                            {
                                var testsplit = test.Split('|');
                                IList<Testparameter> testparameters = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("([TestMethod.TestName] ='" + testsplit[0] + "' and [TestMethod.MethodName.MethodName] ='" + testsplit[1] + "')"));
                                if (criteria == string.Empty)
                                {
                                    criteria = "[TestMethod.GCRecord] is NULL and Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testparameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                }
                                else
                                {
                                    criteria = criteria + "and Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testparameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                }
                            }
                            lstview.Criteria = CriteriaOperator.Parse(criteria);
                        }
                        lstview.Properties.Add(new ViewProperty("TTestName", SortDirection.Ascending, "TestMethod.TestName", true, true));
                        lstview.Properties.Add(new ViewProperty("TMethodName", SortDirection.Ascending, "TestMethod.MethodName.MethodName", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        if (SRInfo.lstTestParameter != null && SRInfo.lstTestParameter.Count > 0)
                        {
                            if (SRInfo.lstdupfilterguid != null && SRInfo.lstdupfilterguid.Count > 0)
                            {
                                foreach (Guid guid in SRInfo.lstdupfilterguid)
                                {
                                    groups.Add(guid);
                                }
                            }
                            ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", SRInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                            ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", SRInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        }
                        else
                        {
                            ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                        }
                        ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"] = new InOperator("Oid", groups);
                        ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["dupfilter"] = new InOperator("Oid", groups);
                        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                }
                else if (View.Id == "Testparameter_LookupListView_TBAvailableTest")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                }
                else if (View.Id == "Testparameter_LookupListView_TBSelectedTest")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    gridListEditor.Grid.SettingsPager.AlwaysShowPager = true;
                    gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                    ICallbackManagerHolder seltest = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    string script = seltest.CallbackManager.GetScript();
                    script = string.Format(CultureInfo.InvariantCulture, @"
                        function(s, e) {{ 
                            var xafCallback = function() {{
                            s.EndCallback.RemoveHandler(xafCallback);
                            {0}
                            }};
                            s.EndCallback.AddHandler(xafCallback);
                        }}
                    ", script);
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = script;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e)
                    { 
                    s.SetWidth(400); 
                    s.RowClick.ClearHandlers();
                    }";
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                }
                else if (View.Id == "Testparameter_LookupListView_TBSelectedParameter")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("Test", this);
                    gridListEditor.Grid.SettingsPager.AlwaysShowPager = true;
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                    gridListEditor.Grid.SettingsBehavior.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
                    gridListEditor.Grid.CustomColumnSort += Grid_CustomColumnSort;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e){ 
                    s.SetWidth(400); 
                    s.RowClick.ClearHandlers();
                    }";
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e){
                      if(e.visibleIndex != '-1')
                      {
                        if (s.IsRowSelectedOnPage(e.visibleIndex)) {   
                            var value = 'Testselection|Selected|' + s.GetRowKey(e.visibleIndex);
                            RaiseXafCallback(globalCallbackControl, 'Test', value, '', false);    
                        }else{
                            var value = 'Testselection|UNSelected|' + s.GetRowKey(e.visibleIndex);
                            RaiseXafCallback(globalCallbackControl, 'Test', value, '', false);
                        }
                     }
                     else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                     {        
                        RaiseXafCallback(globalCallbackControl, 'Test', 'Testselection|Selectall', '', false);                        
                     }   
                     else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                     {
                        RaiseXafCallback(globalCallbackControl, 'Test', 'Testselection|UNSelectall', '', false);                        
                     }  
                    }";
                }
                else if (View.Id == "SpreadSheetBuilder_DataTransfer_DetailView")
                {
                    SpreadSheetBuilder_DataTransfer objsdmsdt = (SpreadSheetBuilder_DataTransfer)View.CurrentObject;
                    if (objsdmsdt != null)
                    {
                        if (objsdmsdt.EnteredBy != null)
                        {
                            Employee objempenterby = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[Oid] = ?", new Guid(objsdmsdt.EnteredBy)));
                            if (objempenterby != null)
                            {
                                objsdmsdt.EnteredBy = objempenterby.FullName;
                            }
                            else
                            {
                                objsdmsdt.EnteredBy = string.Empty;
                            }
                        }
                        if (objsdmsdt.ModifiedBy != null)
                        {
                            Employee objempmodifiedby = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[Oid] = ?", new Guid(objsdmsdt.ModifiedBy)));
                            if (objempmodifiedby != null)
                            {
                                objsdmsdt.ModifiedBy = objempmodifiedby.FullName;
                            }
                            else
                            {
                                objsdmsdt.ModifiedBy = string.Empty;
                            }
                        }
                    }
                    DashboardViewItem viewItem = (DashboardViewItem)((DetailView)View).FindItem("Final");
                    if (viewItem != null)
                    {
                        DashboardViewItem mainviewItem = (DashboardViewItem)((DetailView)Application.MainWindow.View).FindItem("Final");
                        {
                            if (mainviewItem != null && mainviewItem.InnerView != null)
                            {
                                ((ListView)viewItem.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TemplateID]=?", ((SpreadSheetBuilder_TemplateInfo)Application.MainWindow.View.CurrentObject).TemplateID);
                                foreach (SpreadSheetBuilder_FieldSetUp fieldSetUp in ((ListView)mainviewItem.InnerView).CollectionSource.List)
                                {
                                    if (mainviewItem.InnerView.ObjectSpace.IsNewObject(fieldSetUp))
                                    {
                                        SpreadSheetBuilder_FieldSetUp fieldSetUp1 = viewItem.InnerView.ObjectSpace.CreateObject<SpreadSheetBuilder_FieldSetUp>();
                                        fieldSetUp1.TemplateID = fieldSetUp.TemplateID;
                                        fieldSetUp1.FieldID = viewItem.InnerView.ObjectSpace.GetObjectByKey<SpreadSheetBuilder_ScientificData>(fieldSetUp.FieldID.uqID);
                                        fieldSetUp1.uqID = fieldSetUp.uqID;
                                        fieldSetUp1.Sort = fieldSetUp.uqID;
                                        ((ListView)viewItem.InnerView).CollectionSource.Add(fieldSetUp1);
                                    }
                                }
                                ((ListView)viewItem.InnerView).Refresh();
                            }
                        }
                    }
                    DashboardViewItem viewItem1 = (DashboardViewItem)((DetailView)View).FindItem("FinalCalibration");
                    if (viewItem1 != null)
                    {
                        DashboardViewItem mainviewItem1 = (DashboardViewItem)((DetailView)Application.MainWindow.View).FindItem("FinalCalibration");
                        {
                            if (mainviewItem1 != null && mainviewItem1.InnerView != null)
                            {
                                ((ListView)viewItem1.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TemplateID]=?", ((SpreadSheetBuilder_TemplateInfo)Application.MainWindow.View.CurrentObject).TemplateID);
                                foreach (SpreadSheetBuilder_FieldSetUp fieldSetUp in ((ListView)mainviewItem1.InnerView).CollectionSource.List)
                                {
                                    if (mainviewItem1.InnerView.ObjectSpace.IsNewObject(fieldSetUp))
                                    {
                                        SpreadSheetBuilder_FieldSetUp fieldSetUp1 = viewItem1.InnerView.ObjectSpace.CreateObject<SpreadSheetBuilder_FieldSetUp>();
                                        fieldSetUp1.TemplateID = fieldSetUp.TemplateID;
                                        fieldSetUp1.FieldID = viewItem1.InnerView.ObjectSpace.GetObjectByKey<SpreadSheetBuilder_ScientificData>(fieldSetUp.FieldID.uqID);
                                        fieldSetUp1.uqID = fieldSetUp.uqID;
                                        fieldSetUp1.Sort = fieldSetUp.uqID;
                                        ((ListView)viewItem1.InnerView).CollectionSource.Add(fieldSetUp1);
                                    }
                                }
                                ((ListView)viewItem1.InnerView).Refresh();
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

        private void Grid_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                bool isRow1Selected = grid.Selection.IsRowSelectedByKey(e.GetRow1Value(grid.KeyFieldName));
                bool isRow2Selected = grid.Selection.IsRowSelectedByKey(e.GetRow2Value(grid.KeyFieldName));
                e.Handled = isRow1Selected != isRow2Selected;
                if (e.Handled)
                {
                    if (e.SortOrder == DevExpress.Data.ColumnSortOrder.Descending)
                        e.Result = isRow1Selected ? 1 : -1;
                    else
                        e.Result = isRow1Selected ? -1 : 1;
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

        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (SRInfo.lstTestParameter != null && SRInfo.lstTestParameter.Count > 0 && SRInfo.strSelectionMode == "Selected")
                {
                    foreach (Guid obj in SRInfo.lstTestParameter)
                    {
                        gridView.Selection.SelectRowByKey(obj);
                    }
                    SRInfo.strSelectionMode = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void griddataload(ASPxGridView grid)
        {
            try
            {
                if (dtSDMSTemplateInfo != null && dtSDMSTemplateInfo.Rows.Count > 0)
                {
                    DataView dv = new DataView(dtSDMSTemplateInfo);
                    dv.Sort = "NPTest Asc";
                    dtSDMSTemplateInfo = dv.ToTable();
                    grid.DataSource = dtSDMSTemplateInfo;
                    grid.DataBind();
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
                if (View.Id == "Testparameter_LookupListView_TBSelectedTest")
                {
                    Testparameter testparameter = (Testparameter)View.CurrentObject;
                    DashboardViewItem TestViewSubChild = ((NestedFrame)Frame).ViewItem.View.FindItem("TestViewSubChild") as DashboardViewItem;
                    if (testparameter != null && TestViewSubChild != null && SRInfo.UseSelchanged)
                    {
                        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.TestName] =? and [TestMethod.MethodName.MethodNumber] =? and [TestMethod.MatrixName.MatrixName] =? and [QCType.QCTypeName] = 'Sample'", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName);
                        SRInfo.strSelectionMode = "Selected";
                    }
                    else
                    {
                        SRInfo.UseSelchanged = true;
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
                if (View.Id == "SpreadSheetBuilder_DataParsing_ListView")
                {
                    if (e.DataColumn.FieldName == "FieldName")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, '{1}', 'FieldNameclk|'+{0}+'|'+'{1}', '', false)", e.VisibleIndex, View.Id));
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (e.DataColumn.FieldName == "FieldID.uqID")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, '{1}', 'Selected|'+{0}+'|'+'{1}', '', false)", e.VisibleIndex, View.Id));
                    }
                    else
                    {
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

        private void filterselectedgrid(CompositeView cview, DevExpress.ExpressApp.View view, string viewid)
        {
            try
            {
                List<int> list = new List<int>();
                DashboardViewItem viewItem = (DashboardViewItem)((DetailView)cview).FindItem(viewid);
                if (viewItem != null)
                {
                    foreach (SpreadSheetBuilder_FieldSetUp fieldSetUp in ((ListView)viewItem.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_FieldSetUp>().Where(a => a.TemplateID == ((SpreadSheetBuilder_TemplateInfo)cview.CurrentObject).TemplateID).ToList())
                    {
                        list.Add(fieldSetUp.FieldID.uqID);
                    }
                }
                ((ListView)view).CollectionSource.Criteria["filter1"] = CriteriaOperator.Parse("Not [uqID] In(" + string.Format("'{0}'", string.Join("','", list.Select(i => i.ToString().Replace("'", "''")))) + ")");
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
                if (View is ListView && View.Id == "DummyClass_ListView_TemplateInfo")
                {
                    IsLVCreate = true;
                }

                if (View is ListView && View.Id != "DummyClass_ListView_TemplateInfo" && View.Id != "Testparameter_LookupListView_TBAvailableTest" && View.Id != "Testparameter_LookupListView_TBSelectedParameter" && View.Id != "Testparameter_LookupListView_TBSelectedTest")
                {
                    //Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = true;
                    if (tar != null)
                    {
                        tar.CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
                    }
                }
                else if (View.Id == "SpreadSheetBuilder_TemplateInfo_DetailView_Builder")
                {
                    //if (appearanceController != null)
                    //{
                    //    appearanceController.CustomApplyAppearance -= AppearanceController_CustomApplyAppearance;
                    //}
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("SaveAndCloseAction", true);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("ShowSaveAndNewAction", true);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Active.SetItemValue("NewObjectAction", true);
                    Frame.GetController<RefreshController>().RefreshAction.Active.SetItemValue("RefreshAction", true);
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("DeleteAction", true);
                    Frame.GetController<ModificationsController>().SaveAction.Executing -= SaveAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                    if (WebWindow.CurrentRequestWindow != null)
                    {
                        WebWindow.CurrentRequestWindow.RegisterClientScript("alrt", "document.getElementById('separatorButton').setAttribute('onclick', 'NavSplit();')");
                    }
                    Frame.GetController<WebModificationsController>().EditAction.Executing -= EditAction_Executing;
                    ((WebLayoutManager)((DetailView)View).LayoutManager).PageControlCreated -= TBViewController_PageControlCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void removedatafromgrid(DashboardViewItem viewItem, DashboardViewItem viewItem1, CompositeView cview, int currentid, string strtable)
        {
            try
            {
                SpreadSheetBuilder_FieldSetUp currrow = ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_FieldSetUp>().Where(a => a.uqID == currentid).FirstOrDefault();
                if (currrow != null)
                {
                    ((ListView)View).CollectionSource.Remove(currrow);
                }
                SpreadSheetBuilder_FieldSetUp currrow1 = ((ListView)viewItem1.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_FieldSetUp>().Where(a => a.uqID == currentid).FirstOrDefault();
                if (currrow1 != null)
                {
                    ((ListView)viewItem1.InnerView).CollectionSource.Remove(currrow1);
                }
                if (View.ObjectSpace.IsNewObject(currrow))
                {
                    View.ObjectSpace.RemoveFromModifiedObjects(currrow);
                }
                else
                {
                    View.ObjectSpace.Delete(currrow);
                }
                List<int> list = new List<int>();
                foreach (SpreadSheetBuilder_FieldSetUp fieldSetUp in ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_FieldSetUp>().Where(a => a.TemplateID == ((SpreadSheetBuilder_TemplateInfo)cview.CurrentObject).TemplateID).ToList())
                {
                    list.Add(fieldSetUp.FieldID.uqID);
                }
                ((ListView)viewItem.InnerView).CollectionSource.Criteria.Clear();
                ((ListView)viewItem.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [uqID] In(" + string.Format("'{0}'", string.Join("','", list.Select(i => i.ToString().Replace("'", "''")))) + ") and [ResultType] = '" + strtable + "'");
                ((ListView)viewItem.InnerView).Refresh();
                ((ListView)viewItem1.InnerView).Refresh();
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
                if (!string.IsNullOrEmpty(parameter))
                {
                    string[] param = parameter.Split('|');
                    if (param[0] == "Script")
                    {
                        ASPxSpreadsheetPropertyEditor aSPxSpreadsheet = ((DetailView)View).FindItem("SpreadSheet") as ASPxSpreadsheetPropertyEditor;
                        if (aSPxSpreadsheet != null)
                        {
                            script(aSPxSpreadsheet.ASPxSpreadsheetControl);
                        }
                    }
                    else if (param[0] == "Testselection")
                    {
                        if (param[1] == "Selected")
                        {
                            Guid curguid = new Guid(param[2]);
                            SRInfo.strSelectionMode = param[1];
                            if (!SRInfo.lstTestParameter.Contains(curguid))
                            {
                                SRInfo.lstTestParameter.Add(curguid);
                            }
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            CompositeView view = nestedFrame.ViewItem.View;
                            Testparameter testparameter = ObjectSpace.GetObjectByKey<Testparameter>(curguid);
                            DashboardViewItem TestViewMain = ((DashboardView)view).FindItem("TestViewMain") as DashboardViewItem;
                            DashboardViewItem TestViewSub = ((DashboardView)view).FindItem("TestViewSub") as DashboardViewItem;
                            DashboardViewItem TestViewSubChild = ((DashboardView)view).FindItem("TestViewSubChild") as DashboardViewItem;
                            bool Oidchange = true;
                            Guid curusedguid = new Guid();
                            foreach (Testparameter objtestparameter in ((ListView)TestViewSub.InnerView).CollectionSource.List)
                            {
                                if (objtestparameter.Oid == testparameter.Oid)
                                {
                                    Oidchange = false;
                                }
                                if (Oidchange && objtestparameter.TestMethod.TestName == testparameter.TestMethod.TestName && objtestparameter.TestMethod.MethodName.MethodNumber == testparameter.TestMethod.MethodName.MethodNumber)
                                {
                                    curusedguid = objtestparameter.Oid;
                                }
                            }
                            if (Oidchange && TestViewSubChild != null && TestViewSubChild.InnerView.SelectedObjects.Count == 1)
                            {
                                Testparameter addnewtestparameter = (Testparameter)TestViewSubChild.InnerView.SelectedObjects[0];
                                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"] = ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["dupfilter"] =
                                CriteriaOperator.Parse(((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"].ToString().Replace(curusedguid.ToString(), addnewtestparameter.Oid.ToString()));
                                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", SRInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", SRInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                ASPxGridListEditor gridListEditor = ((ListView)TestViewSub.InnerView).Editor as ASPxGridListEditor;
                                if (gridListEditor != null && gridListEditor.Grid != null)
                                {
                                    SRInfo.UseSelchanged = false;
                                    gridListEditor.Grid.Selection.SelectRowByKey(addnewtestparameter.Oid);
                                }
                            }
                        }
                        else if (param[1] == "UNSelected")
                        {
                            Guid curguid = new Guid(param[2]);
                            SRInfo.strSelectionMode = param[1];
                            if (SRInfo.lstTestParameter.Contains(curguid))
                            {
                                SRInfo.lstTestParameter.Remove(curguid);
                            }
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            CompositeView view = nestedFrame.ViewItem.View;
                            Testparameter testparameter = ObjectSpace.GetObjectByKey<Testparameter>(curguid);
                            DashboardViewItem TestViewMain = ((DashboardView)view).FindItem("TestViewMain") as DashboardViewItem;
                            DashboardViewItem TestViewSub = ((DashboardView)view).FindItem("TestViewSub") as DashboardViewItem;
                            DashboardViewItem TestViewSubChild = ((DashboardView)view).FindItem("TestViewSubChild") as DashboardViewItem;
                            Testparameter addnewtestparameter = null;
                            foreach (Testparameter objtestparameter in ((ListView)TestViewSub.InnerView).CollectionSource.List)
                            {
                                if (testparameter != null && objtestparameter.Oid == testparameter.Oid)
                                {
                                    if (TestViewSubChild != null && TestViewSubChild.InnerView.SelectedObjects.Count > 0)
                                    {
                                        addnewtestparameter = (Testparameter)TestViewSubChild.InnerView.SelectedObjects[0];
                                    }
                                }
                            }
                            if (addnewtestparameter != null)
                            {
                                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"] = ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["dupfilter"] =
                                CriteriaOperator.Parse(((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"].ToString().Replace(curguid.ToString(), addnewtestparameter.Oid.ToString()));
                                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", SRInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", SRInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                ASPxGridListEditor gridListEditor = ((ListView)TestViewSub.InnerView).Editor as ASPxGridListEditor;
                                if (gridListEditor != null && gridListEditor.Grid != null)
                                {
                                    SRInfo.UseSelchanged = false;
                                    gridListEditor.Grid.Selection.SelectRowByKey(addnewtestparameter.Oid);
                                }
                            }
                        }
                        else if (param[1] == "Selectall")
                        {
                            ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (editor != null && editor.Grid != null)
                            {
                                for (int i = 0; i < editor.Grid.VisibleRowCount; i++)
                                {
                                    Guid curguid = new Guid(editor.Grid.GetRowValues(i, "Oid").ToString());
                                    SRInfo.strSelectionMode = "Selected";
                                    if (!SRInfo.lstTestParameter.Contains(curguid))
                                    {
                                        SRInfo.lstTestParameter.Add(curguid);
                                    }
                                }
                            }
                        }
                        else if (param[1] == "UNSelectall")
                        {
                            ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (editor != null && editor.Grid != null)
                            {
                                for (int i = 0; i < editor.Grid.VisibleRowCount; i++)
                                {
                                    Guid curguid = new Guid(editor.Grid.GetRowValues(i, "Oid").ToString());
                                    SRInfo.strSelectionMode = "UNSelected";
                                    if (SRInfo.lstTestParameter.Contains(curguid) && !SRInfo.lstSavedTestParameter.Contains(curguid))
                                    {
                                        SRInfo.lstTestParameter.Remove(curguid);
                                    }
                                }
                            }
                        }
                    }
                    else if (param[0] == "TBTest")
                    {
                        SpreadSheetBuilder_TemplateInfo templateInfo = (SpreadSheetBuilder_TemplateInfo)View.CurrentObject;
                        if (templateInfo != null)
                        {
                            SRInfo.IsTestAssignmentClosed = false;
                            DashboardView dashboard = Application.CreateDashboardView(ObjectSpace, "TBTest", false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(dashboard);
                            showViewParameters.Context = TemplateContext.NestedFrame;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.AcceptAction.Active.SetItemValue("disable", false);
                            dc.CancelAction.Active.SetItemValue("disable", false);
                            dc.CloseOnCurrentObjectProcessing = false;
                            showViewParameters.Controllers.Add(dc);
                            if (SRInfo.lstTestParameter == null)
                            {
                                SRInfo.lstTestParameter = new List<Guid>();
                                SRInfo.lstSavedTestParameter = new List<Guid>();
                                SRInfo.lstdupfilterguid = new List<Guid>();
                                SRInfo.lstdupfilterstr = new List<string>();
                                IList<SpreadSheetBuilder_TestParameter> objsample = ObjectSpace.GetObjects<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TemplateID]=?", templateInfo.TemplateID));
                                if (objsample != null && objsample.Count > 0)
                                {
                                    foreach (SpreadSheetBuilder_TestParameter sample in objsample.ToList())
                                    {
                                        if (!SRInfo.lstTestParameter.Contains(sample.TestParameterID))
                                        {
                                            Testparameter testparameter = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=? and [TestMethod.GCRecord] is NULL", sample.TestParameterID));
                                            if (testparameter != null)
                                            {
                                                if (!SRInfo.lstdupfilterstr.Contains(testparameter.TestMethod.TestName + "|" + testparameter.TestMethod.MethodName.MethodName))
                                                {
                                                    SRInfo.lstdupfilterstr.Add(testparameter.TestMethod.TestName + "|" + testparameter.TestMethod.MethodName.MethodName);
                                                    SRInfo.lstdupfilterguid.Add(testparameter.Oid);
                                                }
                                                SRInfo.lstSavedTestParameter.Add(sample.TestParameterID);
                                                SRInfo.lstTestParameter.Add(sample.TestParameterID);
                                            }
                                        }
                                    }
                                }
                            }
                            SRInfo.IsTestcanFilter = true;
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                    else if (param[0] == "FieldNameclk" && param[2] == "SpreadSheetBuilder_DataParsing_ListView")
                    {
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            templateinfo.lstfieldname = new List<int>();
                            HttpContext.Current.Session["rowid"] = Convert.ToInt32(editor.Grid.GetRowValues(int.Parse(param[1]), "uqID"));
                            string ResultType = editor.Grid.GetRowValues(int.Parse(param[1]), "RunType").ToString();
                            foreach (SpreadSheetBuilder_DataParsing dataParsing in ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_DataParsing>().Where(a => a.RunType.ToString() == ResultType).ToList())
                            {
                                templateinfo.lstfieldname.Add(dataParsing.FieldID);
                            }
                            IObjectSpace os = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(ObjectSpace, typeof(SpreadSheetBuilder_ScientificData));
                            cs.Criteria["filter"] = CriteriaOperator.Parse("Not [uqID] In(" + string.Format("'{0}'", string.Join("','", templateinfo.lstfieldname.Select(i => i.ToString().Replace("'", "''")))) + ") and [ResultType] =?", ResultType);
                            ListView lv = Application.CreateListView("SpreadSheetBuilder_ScientificData_ListView_Popup", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView.Caption = "FieldName";
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.AcceptAction.Execute += AcceptAction_Execute;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                    else if (View.Id == "DummyClass_ListView_TemplateInfo")
                    {
                        if (param[0] == "Selected")
                        {
                            EditTemplate.Caption = "Edit";
                            EditTemplateCaption = "Edit";
                            EditTemplate.ImageName = "Action_Edit";
                        }
                        else if (param[0] == "UNSelected")
                        {
                            if (View.SelectedObjects.Count == 0)
                            {
                                EditTemplate.Caption = "New";
                                EditTemplateCaption = "New";
                                EditTemplate.ImageName = "Action_New";
                            }
                        }
                        else if (param[0] == "RDblclck")
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            SpreadSheetBuilder_TemplateInfo objtempinfo = os.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID] = ?", int.Parse(param[1])));
                            if (objtempinfo != null)
                            {
                                DetailView detailview = Application.CreateDetailView(os, "SpreadSheetBuilder_TemplateInfo_DetailView_Builder", true, objtempinfo);
                                detailview.ViewEditMode = ViewEditMode.View;
                                Frame.SetView(detailview);
                            }
                        }
                        if (View.SelectedObjects.Count == 0)
                        {
                            EditTemplate.Caption = "New";
                            EditTemplateCaption = "New";
                            EditTemplate.ImageName = "Action_New";
                        }
                    }
                    else
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView cview = nestedFrame.ViewItem.View;
                        if (cview != null && cview is DetailView && cview.CurrentObject != null && ((DetailView)cview).ViewEditMode == ViewEditMode.Edit)
                        {
                            ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                            {
                                if (param[0] == "Selected")
                                {
                                    int currentid = Convert.ToInt32(editor.Grid.GetRowValues(int.Parse(param[1]), "uqID"));
                                    if (param[2] == "SpreadSheetBuilder_FieldSetUp_ListView_RawData_Selected")
                                    {
                                        DashboardViewItem viewItem = (DashboardViewItem)((DetailView)cview).FindItem("FieldInfoRawDataAvailable");
                                        DashboardViewItem viewItem1 = (DashboardViewItem)((DetailView)cview).FindItem("Final");
                                        if (viewItem != null && viewItem1 != null)
                                        {
                                            removedatafromgrid(viewItem, viewItem1, cview, currentid, "RawDataTable");
                                        }
                                    }
                                    else if (param[2] == "SpreadSheetBuilder_FieldSetUp_ListView_Calibration_Selected")
                                    {
                                        DashboardViewItem viewItem = (DashboardViewItem)((DetailView)cview).FindItem("FieldInfoCalibrationAvailable");
                                        DashboardViewItem viewItem1 = (DashboardViewItem)((DetailView)cview).FindItem("FinalCalibration");
                                        if (viewItem != null && viewItem1 != null)
                                        {
                                            removedatafromgrid(viewItem, viewItem1, cview, currentid, "CalibrationTable");
                                        }
                                    }
                                }
                                else if (param[0] == "Entered")
                                {
                                    if (param[4] == "SpreadSheetBuilder_FieldSetUp_ListView_RawData_Selected" || param[4] == "SpreadSheetBuilder_FieldSetUp_ListView_Calibration_Selected")
                                    {
                                        int currentid = Convert.ToInt32(editor.Grid.GetRowValues(int.Parse(param[1]), "uqID"));
                                        if (currentid < 1)
                                        {
                                            SpreadSheetBuilder_FieldSetUp currrow = ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_FieldSetUp>().Where(a => a.uqID == currentid).FirstOrDefault();
                                            if (currrow != null)
                                            {
                                                var sproperty = currrow.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name == param[2]).FirstOrDefault();
                                                if (sproperty != null)
                                                {
                                                    Type type = Type.GetType(sproperty.MemberType.FullName);
                                                    if (type != null)
                                                    {
                                                        sproperty.SetValue(currrow, Convert.ChangeType(param[3], type));
                                                        ((ListView)View).Refresh();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (param[4] == "SpreadSheetBuilder_DataParsing_ListView")
                                    {
                                        int currentid = Convert.ToInt32(editor.Grid.GetRowValues(int.Parse(param[1]), "uqID"));
                                        if (currentid < 1)
                                        {
                                            SpreadSheetBuilder_DataParsing currrow = ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_DataParsing>().Where(a => a.uqID == currentid).FirstOrDefault();
                                            if (currrow != null)
                                            {
                                                if (param[2] != "RunType")
                                                {
                                                    var sproperty = currrow.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name == param[2]).FirstOrDefault();
                                                    if (sproperty != null)
                                                    {
                                                        Type type = Type.GetType(sproperty.MemberType.FullName);
                                                        if (type != null)
                                                        {
                                                            sproperty.SetValue(currrow, Convert.ChangeType(param[3], type));
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    currrow.RunType = (ResultType)System.Enum.Parse(typeof(ResultType), param[3]);
                                                }
                                                ((ListView)View).Refresh();
                                            }
                                        }
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

        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string MonitoredBy = string.Empty;
                if (e.SelectedObjects.Count == 1)
                {
                    if (HttpContext.Current.Session["rowid"] != null)
                    {
                        SpreadSheetBuilder_DataParsing obj = ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_DataParsing>().Where(a => a.uqID == Convert.ToInt32(HttpContext.Current.Session["rowid"].ToString())).First();
                        if (obj != null)
                        {
                            obj.FieldID = ((SpreadSheetBuilder_ScientificData)e.SelectedObjects[0]).uqID;
                        }
                        ((ListView)View).Refresh();
                    }
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

        private void script(ASPxSpreadsheet aSPxSpreadsheet)
        {
            try
            {
                SpreadSheetBuilder_TemplateInfo templateInfo = (SpreadSheetBuilder_TemplateInfo)View.CurrentObject;
                if (templateInfo != null && templateInfo.SpreadSheet != null)
                {
                    CellRange HeaderRange = null;
                    CellRange DetailRange = null;
                    CellRange CHeaderRange = null;
                    CellRange CDetailRange = null;
                    IWorkbook objworkbook = aSPxSpreadsheet.Document;

                    applymailmerge(templateInfo, objworkbook);
                    if (objworkbook.Worksheets.ActiveWorksheet.DefinedNames.Contains("CHEADER") == false)
                    {
                        if (objworkbook.Worksheets.ActiveWorksheet.DefinedNames.Contains("HEADERRANGE"))
                        {
                            HeaderRange = objworkbook.Worksheets.ActiveWorksheet["HEADERRANGE"];
                        }
                        if (objworkbook.Worksheets.ActiveWorksheet.DefinedNames.Contains("DETAILRANGE"))
                        {
                            DetailRange = objworkbook.Worksheets.ActiveWorksheet["DETAILRANGE"];
                        }
                        if (objworkbook.Worksheets.ActiveWorksheet.DefinedNames.Contains("Detail"))
                        {
                            CellRange Detailra = objworkbook.Worksheets.ActiveWorksheet["Detail"];
                        }

                        Dictionary<string, string> HeaderColumns = new Dictionary<string, string>();
                        Dictionary<string, string> DetailColumns = new Dictionary<string, string>();

                        IList<CellRange> currentSelection = objworkbook.Worksheets.ActiveWorksheet.GetSelectedRanges();
                        Worksheet ws = objworkbook.Worksheets.ActiveWorksheet;
                        int wr = ws.DefinedNames[0].Range.BottomRowIndex;
                        int BottomIndex = 0;
                        if (ws != null && ws.DefinedNames.Count >= 3)
                        {
                            BottomIndex = ws.DefinedNames[2].Range.BottomRowIndex;
                        }
                        else if (HeaderRange != null)
                        {
                            BottomIndex = HeaderRange.BottomRowIndex;
                        }

                        foreach (CellRange range in currentSelection)
                        {
                            if (HeaderRange != null && BottomIndex != range.BottomRowIndex)
                            {
                                for (int rowIndex = range.TopRowIndex; rowIndex <= range.BottomRowIndex; rowIndex++)
                                {
                                    for (int columnIndex = range.LeftColumnIndex; columnIndex == range.LeftColumnIndex; columnIndex++)
                                    {
                                        HeaderColumns.Add(ws[rowIndex, columnIndex].Value.ToString(), String.Concat(diIndexToColumn[range.RightColumnIndex + 2], rowIndex + 1));
                                    }
                                }
                            }
                        }
                        foreach (KeyValuePair<string, string> Header in HeaderColumns)
                        {
                            if (!string.IsNullOrEmpty(Header.Key))
                            {
                                DashboardViewItem viewItem = (DashboardViewItem)((DetailView)View).FindItem("Header");
                                if (viewItem != null)
                                {
                                    if (viewItem.InnerView == null)
                                    {
                                        viewItem.CreateControl();
                                        viewItem.InnerView.CreateControls();
                                    }
                                    if (((ListView)viewItem.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_Header>().Where(a => a.Caption == Header.Key).ToList().Count == 0)
                                    {
                                        SpreadSheetBuilder_Header header = viewItem.InnerView.ObjectSpace.CreateObject<SpreadSheetBuilder_Header>();
                                        header.Caption = Header.Key;
                                        header.Position = Header.Value;
                                        header.TemplateID = templateInfo.TemplateID;
                                        ((ListView)viewItem.InnerView).CollectionSource.Add(header);
                                        ((ListView)viewItem.InnerView).Refresh();
                                    }
                                }
                            }
                        }

                        foreach (CellRange range in currentSelection)
                        {
                            if (HeaderRange != null && BottomIndex == range.BottomRowIndex)
                            {
                                for (int rowIndex = range.TopRowIndex; rowIndex <= range.BottomRowIndex; rowIndex++)
                                {
                                    for (int columnIndex = range.LeftColumnIndex; columnIndex == range.LeftColumnIndex; columnIndex++)
                                    {
                                        DetailColumns.Add(range.LeftColumnIndex.ToString(), Regex.Replace(ws[rowIndex, columnIndex].Value.ToString(), @"\t|\n|\r", ""));
                                    }
                                }
                            }
                        }
                        foreach (KeyValuePair<string, string> Detail in DetailColumns)
                        {
                            if (!string.IsNullOrEmpty(Detail.Key))
                            {
                                DashboardViewItem viewItem = (DashboardViewItem)((DetailView)View).FindItem("Detail");
                                if (viewItem != null)
                                {
                                    if (viewItem.InnerView == null)
                                    {
                                        viewItem.CreateControl();
                                        viewItem.InnerView.CreateControls();
                                    }
                                    if (((ListView)viewItem.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_Detail>().Where(a => a.ColumnName == Detail.Value).ToList().Count == 0)
                                    {
                                        SpreadSheetBuilder_Detail detail = viewItem.InnerView.ObjectSpace.CreateObject<SpreadSheetBuilder_Detail>();
                                        detail.ColumnIndex = Convert.ToInt32(Detail.Key);
                                        detail.ColumnName = Detail.Value;
                                        detail.TemplateID = templateInfo.TemplateID;
                                        ((ListView)viewItem.InnerView).CollectionSource.Add(detail);
                                        ((ListView)viewItem.InnerView).Refresh();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (objworkbook.Worksheets.ActiveWorksheet.DefinedNames.Contains("CHEADER"))
                        {
                            CHeaderRange = objworkbook.Worksheets.ActiveWorksheet["CHEADER"];
                        }
                        if (objworkbook.Worksheets.ActiveWorksheet.DefinedNames.Contains("CDETAIL"))
                        {
                            CDetailRange = objworkbook.Worksheets.ActiveWorksheet["CDETAIL"];
                        }

                        Dictionary<string, string> CHeaderColumns = new Dictionary<string, string>();
                        Dictionary<string, string> CDetailColumns = new Dictionary<string, string>();
                        IList<CellRange> currentSelection = objworkbook.Worksheets.ActiveWorksheet.GetSelectedRanges();
                        Worksheet ws = objworkbook.Worksheets.ActiveWorksheet;
                        int RightColumnIndex = CDetailRange.RightColumnIndex;

                        foreach (CellRange range in currentSelection)
                        {
                            if (CHeaderRange != null && range.RightColumnIndex > RightColumnIndex)
                            {
                                for (int rowIndex = range.TopRowIndex; rowIndex <= range.BottomRowIndex; rowIndex++)
                                {
                                    for (int columnIndex = range.LeftColumnIndex; columnIndex == range.LeftColumnIndex; columnIndex++)
                                    {
                                        CHeaderColumns.Add(ws[rowIndex, columnIndex].Value.ToString(), String.Concat(diIndexToColumn[range.RightColumnIndex + 2], rowIndex + 1));
                                    }
                                }
                            }
                        }
                        foreach (KeyValuePair<string, string> Header in CHeaderColumns)
                        {
                            if (!string.IsNullOrEmpty(Header.Key))
                            {
                                DashboardViewItem viewItem = (DashboardViewItem)((DetailView)View).FindItem("CHeader");
                                if (viewItem != null)
                                {
                                    if (viewItem.InnerView == null)
                                    {
                                        viewItem.CreateControl();
                                        viewItem.InnerView.CreateControls();
                                    }
                                    if (((ListView)viewItem.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_CHeader>().Where(a => a.Caption == Header.Key).ToList().Count == 0)
                                    {
                                        SpreadSheetBuilder_CHeader header = viewItem.InnerView.ObjectSpace.CreateObject<SpreadSheetBuilder_CHeader>();
                                        header.Caption = Header.Key;
                                        header.Position = Header.Value;
                                        header.TemplateID = templateInfo.TemplateID;
                                        ((ListView)viewItem.InnerView).CollectionSource.Add(header);
                                        ((ListView)viewItem.InnerView).Refresh();
                                    }
                                }
                            }
                        }

                        foreach (CellRange range in currentSelection)
                        {
                            if (CDetailRange != null && range.RightColumnIndex <= RightColumnIndex)
                            {
                                for (int rowIndex = range.TopRowIndex; rowIndex <= range.BottomRowIndex; rowIndex++)
                                {
                                    for (int columnIndex = range.LeftColumnIndex; columnIndex == range.LeftColumnIndex; columnIndex++)
                                    {
                                        CDetailColumns.Add(range.LeftColumnIndex.ToString(), Regex.Replace(ws[rowIndex, columnIndex].Value.ToString(), @"\t|\n|\r", ""));
                                    }
                                }
                            }
                        }
                        foreach (KeyValuePair<string, string> Detail in CDetailColumns)
                        {
                            if (!string.IsNullOrEmpty(Detail.Key))
                            {
                                DashboardViewItem viewItem = (DashboardViewItem)((DetailView)View).FindItem("CDetail");
                                if (viewItem != null)
                                {
                                    if (viewItem.InnerView == null)
                                    {
                                        viewItem.CreateControl();
                                        viewItem.InnerView.CreateControls();
                                    }
                                    if (((ListView)viewItem.InnerView).CollectionSource.List.Cast<SpreadSheetBuilder_CDetail>().Where(a => a.ColumnName == Detail.Value).ToList().Count == 0)
                                    {
                                        SpreadSheetBuilder_CDetail detail = viewItem.InnerView.ObjectSpace.CreateObject<SpreadSheetBuilder_CDetail>();
                                        detail.ColumnIndex = Convert.ToInt32(Detail.Key);
                                        detail.ColumnName = Detail.Value;
                                        detail.TemplateID = templateInfo.TemplateID;
                                        ((ListView)viewItem.InnerView).CollectionSource.Add(detail);
                                        ((ListView)viewItem.InnerView).Refresh();
                                    }
                                }
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(templateInfo.HeaderRange) && HeaderRange != null)
                    {
                        templateInfo.HeaderRange = HeaderRange.GetReferenceA1().Replace("$", "");
                    }
                    if (string.IsNullOrEmpty(templateInfo.DetailRange) && DetailRange != null)
                    {
                        templateInfo.DetailRange = DetailRange.GetReferenceA1().Replace("$", "");
                    }
                    if (string.IsNullOrEmpty(templateInfo.CHeaderRange) && CHeaderRange != null)
                    {
                        templateInfo.CHeaderRange = CHeaderRange.GetReferenceA1().Replace("$", "");
                    }
                    if (string.IsNullOrEmpty(templateInfo.CDetailRange) && CDetailRange != null)
                    {
                        templateInfo.CDetailRange = CDetailRange.GetReferenceA1().Replace("$", "");
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void applymailmerge(SpreadSheetBuilder_TemplateInfo templateInfo, IWorkbook objworkbook)
        {
            try
            {
                if (objworkbook.Worksheets[0] != null)
                {
                    if (!string.IsNullOrEmpty(templateInfo.HeaderRange) && objworkbook.Worksheets[0].DefinedNames.Contains("HEADERRANGE") == false)
                    {
                        CellRange header = objworkbook.Worksheets[0].Range[templateInfo.HeaderRange];
                        header.Name = "HEADERRANGE";
                    }
                    if (!string.IsNullOrEmpty(templateInfo.DetailRange) && objworkbook.Worksheets[0].DefinedNames.Contains("DETAILRANGE") == false)
                    {
                        CellRange detail = objworkbook.Worksheets[0].Range[templateInfo.DetailRange];
                        detail.Name = "DETAILRANGE";
                    }
                }
                if (objworkbook.Worksheets.Count > 1 && objworkbook.Worksheets[1] != null)
                {
                    if (!string.IsNullOrEmpty(templateInfo.CHeaderRange) && objworkbook.Worksheets[1].DefinedNames.Contains("CHEADER") == false)
                    {
                        CellRange cheader = objworkbook.Worksheets[1].Range[templateInfo.CHeaderRange];
                        cheader.Name = "CHEADER";
                    }
                    if (!string.IsNullOrEmpty(templateInfo.CDetailRange) && objworkbook.Worksheets[1].DefinedNames.Contains("CDETAIL") == false)
                    {
                        CellRange cdetail = objworkbook.Worksheets[1].Range[templateInfo.CDetailRange];
                        cdetail.Name = "CDETAIL";
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void intializeIndexColumns()
        {
            try
            {
                diIndexToColumn.Add(1, "A");
                diIndexToColumn.Add(2, "B");
                diIndexToColumn.Add(3, "C");
                diIndexToColumn.Add(4, "D");
                diIndexToColumn.Add(5, "E");
                diIndexToColumn.Add(6, "F");
                diIndexToColumn.Add(7, "G");
                diIndexToColumn.Add(8, "H");
                diIndexToColumn.Add(9, "I");
                diIndexToColumn.Add(10, "J");
                diIndexToColumn.Add(11, "K");
                diIndexToColumn.Add(12, "L");
                diIndexToColumn.Add(13, "M");
                diIndexToColumn.Add(14, "N");
                diIndexToColumn.Add(15, "O");
                diIndexToColumn.Add(16, "P");
                diIndexToColumn.Add(17, "Q");
                diIndexToColumn.Add(18, "R");
                diIndexToColumn.Add(19, "S");
                diIndexToColumn.Add(20, "T");
                diIndexToColumn.Add(21, "U");
                diIndexToColumn.Add(22, "V");
                diIndexToColumn.Add(23, "W");
                diIndexToColumn.Add(24, "X");
                diIndexToColumn.Add(25, "Y");
                diIndexToColumn.Add(26, "Z");
                diIndexToColumn.Add(27, "AA");
                diIndexToColumn.Add(28, "AB");
                diIndexToColumn.Add(29, "AC");
                diIndexToColumn.Add(30, "AD");
                diIndexToColumn.Add(31, "AE");
                diIndexToColumn.Add(32, "AF");
                diIndexToColumn.Add(33, "AG");
                diIndexToColumn.Add(34, "AH");
                diIndexToColumn.Add(35, "AI");
                diIndexToColumn.Add(36, "AJ");
                diIndexToColumn.Add(37, "AK");
                diIndexToColumn.Add(38, "AL");
                diIndexToColumn.Add(39, "AM");
                diIndexToColumn.Add(40, "AN");
                diIndexToColumn.Add(41, "AO");
                diIndexToColumn.Add(42, "AP");
                diIndexToColumn.Add(43, "AQ");
                diIndexToColumn.Add(44, "AR");
                diIndexToColumn.Add(45, "AS");
                diIndexToColumn.Add(46, "AT");
                diIndexToColumn.Add(47, "AU");
                diIndexToColumn.Add(48, "AV");
                diIndexToColumn.Add(49, "AW");
                diIndexToColumn.Add(50, "AX");
                diIndexToColumn.Add(51, "AY");
                diIndexToColumn.Add(52, "AZ");
                diIndexToColumn.Add(53, "BA");
                diIndexToColumn.Add(54, "BB");
                diIndexToColumn.Add(55, "BC");
                diIndexToColumn.Add(56, "BD");
                diIndexToColumn.Add(57, "BE");
                diIndexToColumn.Add(58, "BF");
                diIndexToColumn.Add(59, "BG");
                diIndexToColumn.Add(60, "BH");
                diIndexToColumn.Add(61, "BI");
                diIndexToColumn.Add(62, "BJ");
                diIndexToColumn.Add(63, "BK");
                diIndexToColumn.Add(64, "BL");
                diIndexToColumn.Add(65, "BM");
                diIndexToColumn.Add(66, "BN");
                diIndexToColumn.Add(67, "BO");
                diIndexToColumn.Add(68, "BP");
                diIndexToColumn.Add(69, "BQ");
                diIndexToColumn.Add(70, "BR");
                diIndexToColumn.Add(71, "BS");
                diIndexToColumn.Add(72, "BT");
                diIndexToColumn.Add(73, "BU");
                diIndexToColumn.Add(74, "BV");
                diIndexToColumn.Add(75, "BW");
                diIndexToColumn.Add(76, "BX");
                diIndexToColumn.Add(77, "BY");
                diIndexToColumn.Add(78, "BZ");
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void TestSelectionAdd_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
                DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
                if (TestViewMain != null && ((ListView)TestViewMain.InnerView).SelectedObjects.Count > 0)
                {
                    foreach (Testparameter testparameter in ((ListView)TestViewMain.InnerView).SelectedObjects)
                    {
                        IList<Testparameter> listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodNumber]=? and [TestMethod.MatrixName.MatrixName] = ?", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName));
                        foreach (Testparameter test in listseltest)
                        {
                            if (!SRInfo.lstTestParameter.Contains(test.Oid))
                            {
                                SRInfo.lstTestParameter.Add(test.Oid);
                            }
                        }
                    }
                    if (TestViewSub != null && SRInfo.lstTestParameter != null && SRInfo.lstTestParameter.Count > 0)
                    {
                        ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", SRInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", SRInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                    ((ASPxGridListEditor)((ListView)TestViewSub.InnerView).Editor).Grid.JSProperties["cpCanGridRefresh"] = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void TestSelectionRemove_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
                DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
                if (TestViewMain != null && TestViewSub != null && ((ListView)TestViewSub.InnerView).SelectedObjects.Count > 0)
                {
                    foreach (Testparameter testparameter in ((ListView)TestViewSub.InnerView).SelectedObjects)
                    {
                        IList<Testparameter> listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodNumber]=? and [TestMethod.MatrixName.MatrixName] = ?", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName));
                        if (SRInfo.lstRemoveTestParameter == null)
                        {
                            SRInfo.lstRemoveTestParameter = new List<Guid>();
                        }
                        foreach (Testparameter test in listseltest)
                        {
                            if (SRInfo.lstTestParameter.Contains(test.Oid))
                            {
                                SRInfo.lstTestParameter.Remove(test.Oid);
                                SRInfo.lstRemoveTestParameter.Add(test.Oid);
                            }
                        }
                    }
                    if (SRInfo.lstTestParameter.Count != 0 && TestViewSubChild != null)
                    {
                        ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", SRInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", SRInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                    else
                    {
                        ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("");
                        ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void TestSelectionSave_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SpreadSheetBuilder_TemplateInfo templateInfo = (SpreadSheetBuilder_TemplateInfo)Application.MainWindow.View.CurrentObject;
                if (templateInfo != null)
                {
                    List<string> testname = new List<string>();
                    templateInfo.Test = string.Empty;
                    foreach (Guid objguid in SRInfo.lstTestParameter)
                    {
                        Testparameter testParameter = ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[Oid]=?", new Guid(objguid.ToString())));
                        if (testParameter != null)
                        {
                            if (!testname.Contains(testParameter.TestMethod.TestName))
                            {
                                testname.Add(testParameter.TestMethod.TestName);
                                if (templateInfo.Test == string.Empty)
                                {
                                    templateInfo.Test = testParameter.TestMethod.TestName;
                                }
                                else
                                {
                                    templateInfo.Test = templateInfo.Test + ", " + testParameter.TestMethod.TestName;
                                }
                            }
                        }
                    }
                }
                (Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(true);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AddTB_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SpreadSheetBuilder_DataParsing_ListView")
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView cview = nestedFrame.ViewItem.View;
                    if (cview != null && cview is DetailView && cview.CurrentObject != null)
                    {
                        SpreadSheetBuilder_DataParsing fieldSetUp = View.ObjectSpace.CreateObject<SpreadSheetBuilder_DataParsing>();
                        fieldSetUp.TemplateID = View.ObjectSpace.GetObjectByKey<SpreadSheetBuilder_TemplateInfo>(((SpreadSheetBuilder_TemplateInfo)cview.CurrentObject).TemplateID);
                        fieldSetUp.uqID = ((ListView)View).CollectionSource.List.Count > 0 ? ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_DataParsing>().Min(a => a.uqID) - 1 : 0;
                        ((ListView)View).CollectionSource.Add(fieldSetUp);
                        ((ListView)View).Refresh();
                    }
                }
                else if (View.Id == "SpreadSheetBuilder_DataTransfer_ListView")
                {
                    SpreadSheetBuilder_DataTransfer objdt = View.ObjectSpace.CreateObject<SpreadSheetBuilder_DataTransfer>();
                    objdt.uqID = ((ListView)View).CollectionSource.List.Count > 0 ? ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_DataParsing>().Min(a => a.uqID) - 1 : 0;
                    DetailView dv = Application.CreateDetailView(View.ObjectSpace, "SpreadSheetBuilder_DataTransfer_DetailView", false, objdt);
                    dv.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters();
                    showViewParameters.CreatedView = dv;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    showViewParameters.CreatedView.Caption = "Data Transfer";
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += Dc_Accepting;
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

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.CurrentObject != null)
                {
                    SpreadSheetBuilder_DataTransfer dataTransfer = (SpreadSheetBuilder_DataTransfer)e.AcceptActionArgs.CurrentObject;
                    if (dataTransfer.SpreadSheet != null)
                    {
                        DialogController dc = sender as DialogController;
                        if (dc != null)
                        {
                            ASPxSpreadsheetPropertyEditor aSPxSpreadsheet = ((DetailView)dc.Window.View).FindItem("SpreadSheet") as ASPxSpreadsheetPropertyEditor;
                            if (aSPxSpreadsheet != null)
                            {
                                IWorkbook objworkbook = aSPxSpreadsheet.ASPxSpreadsheetControl.Document;
                                applymailmergedatatransfer(dataTransfer, objworkbook);
                                if (string.IsNullOrEmpty(dataTransfer.HeaderRange) && objworkbook.Worksheets[0].DefinedNames.Contains("HEADERRANGE"))
                                {
                                    dataTransfer.HeaderRange = objworkbook.Worksheets[0]["HEADERRANGE"].GetReferenceA1().Replace("$", "");
                                }
                                if (string.IsNullOrEmpty(dataTransfer.DetailRange) && objworkbook.Worksheets[0].DefinedNames.Contains("DETAILRANGE"))
                                {
                                    dataTransfer.DetailRange = objworkbook.Worksheets[0]["DETAILRANGE"].GetReferenceA1().Replace("$", "");
                                }
                                if (objworkbook.Worksheets.Count > 1)
                                {
                                    if (string.IsNullOrEmpty(dataTransfer.CHeaderRange) && objworkbook.Worksheets[1].DefinedNames.Contains("CHEADER"))
                                    {
                                        dataTransfer.CHeaderRange = objworkbook.Worksheets[1]["CHEADER"].GetReferenceA1().Replace("$", "");
                                    }
                                    if (string.IsNullOrEmpty(dataTransfer.CDetailRange) && objworkbook.Worksheets[1].DefinedNames.Contains("CDETAIL"))
                                    {
                                        dataTransfer.CDetailRange = objworkbook.Worksheets[1]["CDETAIL"].GetReferenceA1().Replace("$", "");
                                    }
                                }
                                dataTransfer.SpreadSheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.SaveDocument(DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                            }
                        }
                    }
                    ((ListView)View).CollectionSource.Add(dataTransfer);
                    ((ListView)View).Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RemoveTB_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SpreadSheetBuilder_DataParsing_ListView")
                {
                    foreach (SpreadSheetBuilder_DataParsing obj in e.SelectedObjects)
                    {
                        ((ListView)View).CollectionSource.Remove(obj);
                        if (View.ObjectSpace.IsNewObject(obj))
                        {
                            View.ObjectSpace.RemoveFromModifiedObjects(obj);
                        }
                        else
                        {
                            View.ObjectSpace.Delete(obj);
                        }
                        ((ListView)View).Refresh();
                    }
                }
                else if (View.Id == "SpreadSheetBuilder_DataTransfer_ListView")
                {
                    foreach (SpreadSheetBuilder_DataTransfer obj in e.SelectedObjects)
                    {
                        ((ListView)View).CollectionSource.Remove(obj);
                        if (View.ObjectSpace.IsNewObject(obj))
                        {
                            View.ObjectSpace.RemoveFromModifiedObjects(obj);
                        }
                        else
                        {
                            View.ObjectSpace.Delete(obj);
                        }
                        ((ListView)View).Refresh();
                    }
                }
                else if (View.Id == "SpreadSheetBuilder_Header_ListView")
                {
                    foreach (SpreadSheetBuilder_Header obj in e.SelectedObjects)
                    {
                        ((ListView)View).CollectionSource.Remove(obj);
                        if (View.ObjectSpace.IsNewObject(obj))
                        {
                            View.ObjectSpace.RemoveFromModifiedObjects(obj);
                        }
                        else
                        {
                            View.ObjectSpace.Delete(obj);
                        }
                        ((ListView)View).Refresh();
                    }
                }
                else if (View.Id == "SpreadSheetBuilder_Detail_ListView")
                {
                    foreach (SpreadSheetBuilder_Detail obj in e.SelectedObjects)
                    {
                        ((ListView)View).CollectionSource.Remove(obj);
                        if (View.ObjectSpace.IsNewObject(obj))
                        {
                            View.ObjectSpace.RemoveFromModifiedObjects(obj);
                        }
                        else
                        {
                            View.ObjectSpace.Delete(obj);
                        }
                        ((ListView)View).Refresh();
                    }
                }
                else if (View.Id == "SpreadSheetBuilder_CHeader_ListView")
                {
                    foreach (SpreadSheetBuilder_CHeader obj in e.SelectedObjects)
                    {
                        ((ListView)View).CollectionSource.Remove(obj);
                        if (View.ObjectSpace.IsNewObject(obj))
                        {
                            View.ObjectSpace.RemoveFromModifiedObjects(obj);
                        }
                        else
                        {
                            View.ObjectSpace.Delete(obj);
                        }
                        ((ListView)View).Refresh();
                    }
                }
                else if (View.Id == "SpreadSheetBuilder_CDetail_ListView")
                {
                    foreach (SpreadSheetBuilder_CDetail obj in e.SelectedObjects)
                    {
                        ((ListView)View).CollectionSource.Remove(obj);
                        if (View.ObjectSpace.IsNewObject(obj))
                        {
                            View.ObjectSpace.RemoveFromModifiedObjects(obj);
                        }
                        else
                        {
                            View.ObjectSpace.Delete(obj);
                        }
                        ((ListView)View).Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void applymailmergedatatransfer(SpreadSheetBuilder_DataTransfer templateInfo, IWorkbook objworkbook)
        {
            try
            {
                if (objworkbook.Worksheets[0] != null)
                {
                    if (!string.IsNullOrEmpty(templateInfo.HeaderRange) && objworkbook.Worksheets[0].DefinedNames.Contains("HEADERRANGE") == false)
                    {
                        CellRange header = objworkbook.Worksheets[0].Range[templateInfo.HeaderRange];
                        header.Name = "HEADERRANGE";
                    }
                    if (!string.IsNullOrEmpty(templateInfo.DetailRange) && objworkbook.Worksheets[0].DefinedNames.Contains("DETAILRANGE") == false)
                    {
                        CellRange detail = objworkbook.Worksheets[0].Range[templateInfo.DetailRange];
                        detail.Name = "DETAILRANGE";
                    }
                }
                if (objworkbook.Worksheets.Count > 1 && objworkbook.Worksheets[1] != null)
                {
                    if (!string.IsNullOrEmpty(templateInfo.CHeaderRange) && objworkbook.Worksheets[1].DefinedNames.Contains("CHEADER") == false)
                    {
                        CellRange cheader = objworkbook.Worksheets[1].Range[templateInfo.CHeaderRange];
                        cheader.Name = "CHEADER";
                    }
                    if (!string.IsNullOrEmpty(templateInfo.CDetailRange) && objworkbook.Worksheets[1].DefinedNames.Contains("CDETAIL") == false)
                    {
                        CellRange cdetail = objworkbook.Worksheets[1].Range[templateInfo.CDetailRange];
                        cdetail.Name = "CDETAIL";
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void EditTemplate_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "DummyClass_ListView_TemplateInfo" && EditTemplateCaption == "Edit")
                {
                    if (View.SelectedObjects.Count == 1)
                    {
                        if (View != null && View.CurrentObject != null)
                        {
                            DummyClass objdmycls = (DummyClass)View.CurrentObject;
                            IObjectSpace os = Application.CreateObjectSpace();
                            SpreadSheetBuilder_TemplateInfo objtempinfo = os.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID] = ?", objdmycls.NPTempID));
                            if (objtempinfo != null)
                            {
                                DetailView detailview = Application.CreateDetailView(os, "SpreadSheetBuilder_TemplateInfo_DetailView_Builder", true, objtempinfo);
                                detailview.ViewEditMode = ViewEditMode.Edit;
                                Frame.SetView(detailview);
                                WebWindow.CurrentRequestWindow.RegisterClientScript("alrt", "tbuirefresh();");
                                FullScreen.SetClientScript("tbuirefresh();", false);
                            }
                        }
                    }
                    else if (View.SelectedObjects.Count > 1)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlyonechkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else if (View.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }

                }
                else if (View.Id == "DummyClass_ListView_TemplateInfo" && EditTemplateCaption == "New")
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(SpreadSheetBuilder_TemplateInfo));
                    SpreadSheetBuilder_TemplateInfo objcrttempinfo = os.CreateObject<SpreadSheetBuilder_TemplateInfo>();
                    DetailView dvtempinfo = Application.CreateDetailView(os, "SpreadSheetBuilder_TemplateInfo_DetailView_Builder", true, objcrttempinfo);
                    dvtempinfo.ViewEditMode = ViewEditMode.Edit;
                    Frame.SetView(dvtempinfo);
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
