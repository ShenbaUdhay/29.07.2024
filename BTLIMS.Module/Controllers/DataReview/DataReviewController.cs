using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Office.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Web.ASPxSpreadsheet;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.QC;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;

namespace LDM.Module.Controllers.DataReview
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DataReviewController : ViewController
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        Qcbatchinfo qcbatchinfo = new Qcbatchinfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        ShowNavigationItemController ShowNavigationController;
        string constr = string.Empty;
        #endregion
        public DataReviewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview;" + "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview;" + "SDMSDCSpreadsheet_DetailView_RawDataBatchReview;"
                + "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview_History;" + "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview_History;" + "SpreadSheetEntry_AnalyticalBatch_ListView;";
            DataReviewBatchDetailsAction.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview;" + "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview;" + "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview_History;" + "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview_History;";
            DataReviewBatchReviewAction.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview;" + "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview;";
            DataReviewBatchRollbackAction.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview_History;" + "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview_History;" + "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview;" + "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview;";
            DataReviewBatchHistoryAction.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview;" + "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview;";
            DataReviewBatchHistoryDateFilter.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview_History;" + "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview_History;";
            //AnalyticalBatchDateFilter.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView;";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                // Perform various tasks depending on the target View.
                constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (View.Id == "SDMSDCSpreadsheet_DetailView_RawDataBatchReview")
                {
                    ASPxSpreadsheetPropertyEditor Spreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                    if (Spreadsheet != null)
                    {
                        Spreadsheet.ControlCreated += Spreadsheet_ControlCreated;
                    }
                }
                else
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview")
                {
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    if (currentUser != null && View != null)
                    {
                        if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                        {
                            objPermissionInfo.RawDataBatchLevel2ReviewIsWrite = false;
                            objPermissionInfo.RawDataBatchLevel3ReviewIsWrite = false;
                            if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                            {
                                objPermissionInfo.RawDataBatchLevel2ReviewIsWrite = true;
                                objPermissionInfo.RawDataBatchLevel3ReviewIsWrite = true;
                            }
                            else
                            {
                                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview")
                                {
                                    foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                                    {
                                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId.Trim() == "RawDataLevel2BatchReview" && i.Write == true) != null)
                                        {
                                            objPermissionInfo.RawDataBatchLevel2ReviewIsWrite = true;
                                            break;
                                        }
                                    }
                                    DataReviewBatchReviewAction.Active["ShowDataBatchReviewAction"] = objPermissionInfo.RawDataBatchLevel2ReviewIsWrite;
                                }
                                else
                                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview")
                                {
                                    foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                                    {
                                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId.Trim() == "RawDataLevel3BatchReview" && i.Write == true) != null)
                                        {
                                            objPermissionInfo.RawDataBatchLevel3ReviewIsWrite = true;
                                            break;
                                        }
                                    }
                                    DataReviewBatchReviewAction.Active["ShowDataBatchReviewAction"] = objPermissionInfo.RawDataBatchLevel3ReviewIsWrite;
                                }
                            }
                        }
                    }
                    if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview")
                    {
                        DataReviewBatchReviewAction.Caption = "Review";
                        DataReviewBatchReviewAction.ToolTip = "Review";
                        DataReviewBatchReviewAction.ImageName = "Action_Validation_Validate";
                    }
                    else
                    {
                        DataReviewBatchReviewAction.Caption = "Verify";
                        DataReviewBatchReviewAction.ToolTip = "Verify";
                        DataReviewBatchReviewAction.ImageName = "State_Task_Completed";
                    }
                    bool Administrator = false;
                    List<Guid> lstTestMethodOid = new List<Guid>();
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        Administrator = true;
                    }
                    else
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                        if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview")
                        {
                            lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] =True", currentUser.Oid));
                        }
                        else
                        {
                            lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultApproval] =True", currentUser.Oid));
                        }
                        if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                        {
                            foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                            {
                                foreach (TestMethod testMethod in departmentChain.TestMethods)
                                {
                                    if (!lstTestMethodOid.Contains(testMethod.Oid))
                                    {
                                        lstTestMethodOid.Add(testMethod.Oid);
                                    }
                                }
                            }
                        }
                    }
                    if (!Administrator)
                    {
                        ((ListView)View).CollectionSource.Criteria["ADCFilter"] = new InOperator("Test.Oid", lstTestMethodOid);
                    }
                    if (((ListView)View).CollectionSource.List.Count > 0)
                    {
                        List<Guid> ABIDOid = new List<Guid>();
                        foreach (SpreadSheetEntry_AnalyticalBatch objABID in ((ListView)View).CollectionSource.List)
                        {
                            IList<Modules.BusinessObjects.SampleManagement.SampleParameter> ObjSampleParameter = View.ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleParameter>(CriteriaOperator.Parse("([TestHold] = False Or [TestHold] Is Null) And [UQABID.Oid] = ?", objABID.Oid));
                            if (ObjSampleParameter.Count > 0)
                            {
                                ABIDOid.Add(objABID.Oid);
                            }
                        }
                        ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", ABIDOid);
                    }
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview_History")
                {
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (DataReviewBatchHistoryDateFilter.SelectedItem == null)
                    {
                        if (setting.AnalysisReviewLevel == EnumDateFilter.OneMonth)
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[0];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.ThreeMonth)
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[1];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 3 And [CreatedDate] Is Not Null");
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.SixMonth)
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[2];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 6 And [CreatedDate] Is Not Null");
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.OneYear)
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[3];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.TwoYear)
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[4];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 2 And [CreatedDate] Is Not Null");
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.FiveYear)
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[5];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 5 And [CreatedDate] Is Not Null");
                        }
                        else
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[6];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[CreatedDate] Is Not Null");

                        }
                        //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    }
                    //DataReviewBatchHistoryDateFilter.SelectedIndex = 1;
                    //((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 3 And [CreatedDate] Is Not Null");
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview_History")
                {
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (DataReviewBatchHistoryDateFilter.SelectedItem == null)
                    {
                        if (setting.AnalysisReviewLevel == EnumDateFilter.OneMonth)
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[0];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.ThreeMonth)
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[1];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 3 And [CreatedDate] Is Not Null");
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.SixMonth)
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[2];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 6 And [CreatedDate] Is Not Null");
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.OneYear)
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[3];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.TwoYear)
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[4];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 2 And [CreatedDate] Is Not Null");
                        }
                        else if (setting.AnalysisReviewLevel == EnumDateFilter.FiveYear)
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[5];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 5 And [CreatedDate] Is Not Null");
                        }
                        else
                        {
                            DataReviewBatchHistoryDateFilter.SelectedItem = DataReviewBatchHistoryDateFilter.Items[6];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[CreatedDate] Is Not Null");
                        }
                        //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    }
                    //DataReviewBatchHistoryDateFilter.SelectedIndex = 1;
                    //((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 3 And [CreatedDate] Is Not Null");
                }
                //else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView")
                //{
                //    AnalyticalBatchDateFilter.SelectedIndex = 0;
                //    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffDay(CreatedDate,Now()) <= 30 And [CreatedDate] Is Not Null");
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }



        }



        private void Spreadsheet_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxSpreadsheet spreadsheet = ((ASPxSpreadsheetPropertyEditor)sender).ASPxSpreadsheetControl;
                if (spreadsheet != null)
                {
                    spreadsheet.Load += Spreadsheet_Load;
                    for (int i = 0; i < spreadsheet.RibbonTabs.Count; i++)
                    {
                        RibbonTab ribbonTab = spreadsheet.RibbonTabs[i];
                        if (ribbonTab != null)
                        {
                            ribbonTab.Visible = false;
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

        private void Spreadsheet_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxSpreadsheet spreadsheet = (ASPxSpreadsheet)sender;
                //IWorkbook objworkbook = spreadsheet.Document;
                spreadsheet.ReadOnly = true;
                spreadsheet.RibbonMode = SpreadsheetRibbonMode.None;
                spreadsheet.ShowFormulaBar = false;
                spreadsheet.ShowSheetTabs = false;
                spreadsheet.Document.Worksheets.ActiveWorksheet.Cells.FillColor = Color.Transparent;
                spreadsheet.Document.Worksheets.ActiveWorksheet.ActiveView.ShowGridlines = false;
                spreadsheet.Document.Worksheets.ActiveWorksheet.ActiveView.ShowHeadings = false;
                CellRange RanData = spreadsheet.Document.Worksheets[0].GetDataRange();
                int lastusedcolindex = RanData.RightColumnIndex;
                int lastusedrowindex = RanData.BottomRowIndex;
                ColumnCollection cols = spreadsheet.Document.Worksheets[0].Columns;
                //for (int i = lastusedcolindex + 1; i <= spreadsheet.Document.Worksheets[0].Columns)
                //{

                //}
                spreadsheet.Document.Worksheets.ActiveWorksheet["$A:$XFD"].Protection.Locked = true;
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
                if (e.PopupFrame.View.Id == "SDMSDCSpreadsheet_DetailView_RawDataBatchReview")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(1200);
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

        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                // Access and customize the target View control.
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.SelectionChanged += Grid_SelectionChanged;
                        editor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                        editor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        if (editor.Grid.Columns["Template"] != null)
                        {
                            editor.Grid.Columns["Template"].CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                        }
                        //if (editor.Grid.Columns["DataReviewBatchDetailsAction"] != null)
                        //{
                        //    editor.Grid.Columns["DataReviewBatchDetailsAction"].Caption = "Details";
                        //}
                    }
                }
                else
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview_History" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview_History")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
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
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview")
                {
                    foreach (SpreadSheetEntry_AnalyticalBatch objAB in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objAB))
                        {
                            objAB.ReviewedDate = DateTime.Now;
                            objAB.ReviewedBy = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        else
                        {
                            objAB.ReviewedDate = null;
                            objAB.ReviewedBy = null;
                        }
                    }
                    View.Refresh();
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview")
                {
                    foreach (SpreadSheetEntry_AnalyticalBatch objAB in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objAB))
                        {
                            objAB.VerifiedDate = DateTime.Now;
                            objAB.VerifiedBy = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        else
                        {
                            objAB.VerifiedDate = null;
                            objAB.VerifiedBy = null;
                        }
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

        protected override void OnDeactivated()
        {
            try
            {
                // Unsubscribe from previously subscribed events and release other references and resources.
                base.OnDeactivated();
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                if (View.Id == "SDMSDCSpreadsheet_DetailView_RawDataBatchReview")
                {
                    ASPxSpreadsheetPropertyEditor Spreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                    if (Spreadsheet != null)
                    {
                        Spreadsheet.ControlCreated -= Spreadsheet_ControlCreated;
                    }
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview" && DataReviewBatchReviewAction.Active.Contains("ShowDataBatchReviewAction"))
                {
                    DataReviewBatchReviewAction.Active.RemoveItem("ShowDataBatchReviewAction");
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview" && DataReviewBatchReviewAction.Active.Contains("ShowDataBatchReviewAction"))
                {
                    DataReviewBatchReviewAction.Active.RemoveItem("ShowDataBatchReviewAction");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DataReviewBatchReviewAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview")
                {
                    if (((ListView)View).SelectedObjects.Count > 0)
                    {
                        DefaultSetting defaultSetting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Spreadsheet'"));
                        DefaultSetting resultValidationSetting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Result Validation'"));
                        DefaultSetting resultApprovalSetting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Result Approval'"));
                        List<SpreadSheetEntry_AnalyticalBatch> lstAB = ((ListView)View).SelectedObjects.Cast<SpreadSheetEntry_AnalyticalBatch>().ToList();
                        for (int i = 0; i < lstAB.Count; i++)
                        {
                            SpreadSheetEntry_AnalyticalBatch objAB = lstAB[i];
                            if (objAB != null)
                            {
                                if (objAB.Test != null && objAB.Test.IsPLMTest)
                                {
                                    if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview")
                                    {
                                        if (defaultSetting.Verify == EnumRELevelSetup.Yes)
                                        {
                                            objAB.Status = 3;
                                            objAB.ReviewedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                            objAB.ReviewedDate = DateTime.Now;
                                        }
                                        else if (defaultSetting.Verify == EnumRELevelSetup.No)
                                        {
                                            objAB.Status = 4;
                                            objAB.ReviewedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                            objAB.ReviewedDate = DateTime.Now;
                                            objAB.VerifiedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                            objAB.VerifiedDate = DateTime.Now;
                                            foreach (SampleParameter sp in ObjectSpace.GetObjects<SampleParameter>().Where(j => j.QCBatchID != null && j.QCBatchID.qcseqdetail.Oid == objAB.Oid).ToList())
                                            {
                                                getstatus(sp, defaultSetting, resultValidationSetting, resultApprovalSetting);
                                            }
                                        }

                                        StatusDefinition objStatus = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 19"));
                                        if (objStatus != null)
                                        {
                                            string[] ids = objAB.Jobid.Split(';');
                                            foreach (string obj in ids)
                                            {
                                                Samplecheckin objSamplecheckin = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", obj));
                                                if (objSamplecheckin != null)
                                                {
                                                    IList<SampleParameter> lstSamples = ObjectSpace.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                                    if (lstSamples.Where(j => j.Status == Samplestatus.PendingEntry).Count() == 0)
                                                    {
                                                        objSamplecheckin.Index = objStatus;
                                                    }
                                                }
                                            }
                                        }
                                        ObjectSpace.CommitChanges();
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "reviewsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    }
                                    else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview")
                                    {
                                        objAB.Status = 4;
                                        objAB.VerifiedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objAB.VerifiedDate = DateTime.Now;
                                        foreach (SampleParameter sp in ObjectSpace.GetObjects<SampleParameter>().Where(j => j.QCBatchID != null && j.QCBatchID.qcseqdetail.Oid == objAB.Oid).ToList())
                                        {
                                            getstatus(sp, defaultSetting, resultValidationSetting, resultApprovalSetting);
                                        }

                                        StatusDefinition objStatus = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 20"));
                                        if (objStatus != null)
                                        {
                                            string[] ids = objAB.Jobid.Split(';');
                                            foreach (string obj in ids)
                                            {
                                                Samplecheckin objSamplecheckin = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", obj));
                                                if (objSamplecheckin != null)
                                                {
                                                    IList<SampleParameter> lstSamples = View.ObjectSpace.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                                    if (lstSamples.Where(j => j.Status == Samplestatus.PendingEntry).Count() == 0)
                                                    {
                                                        objSamplecheckin.Index = objStatus;
                                                    }
                                                }
                                            }
                                        }
                                        ObjectSpace.CommitChanges();
                                        Application.ShowViewStrategy.ShowMessage("Verified successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    }
                                }
                                else
                                {
                                    DataTable dtselectedsamplefields = Getdtparsingsamplefields(objAB.TemplateID);
                                    if (dtselectedsamplefields != null && dtselectedsamplefields.Rows.Count > 0)
                                    {
                                        DataRow[] ExportColumns = dtselectedsamplefields.Select("[ExportSample] <> '' and [ExportSample] is not null");
                                        IList<SpreadSheetEntry> spreadSheets = ObjectSpace.GetObjects<SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", objAB.Oid));
                                        bool insertresult = false;
                                        if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview")
                                        {
                                            objAB.Status = 3;
                                            foreach (SpreadSheetEntry spreadSheet in spreadSheets)
                                            {
                                                spreadSheet.ReviewedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                spreadSheet.ReviewedDate = DateTime.Now;
                                                if (spreadSheet.uqSampleParameterID != null)
                                                {
                                                    spreadSheet.uqSampleParameterID.Status = Samplestatus.PendingVerify;
                                                    spreadSheet.uqSampleParameterID.OSSync = true;
                                                }
                                                if (defaultSetting.Verify == EnumRELevelSetup.No)
                                                {
                                                    objAB.Status = 4;
                                                    spreadSheet.IsExported = true;
                                                    spreadSheet.VerifiedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                    spreadSheet.VerifiedDate = DateTime.Now;
                                                    getstatus(spreadSheet.uqSampleParameterID, defaultSetting, resultValidationSetting, resultApprovalSetting);
                                                    insertresult = Inserttosampleparameter(spreadSheet, ExportColumns);
                                                }
                                                else
                                                {
                                                    insertresult = true;
                                                }
                                            }
                                            if (insertresult)
                                            {
                                                if (objAB.Status == 3)
                                                {
                                                    objAB.ReviewedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                    objAB.ReviewedDate = DateTime.Now;
                                                }
                                                else
                                                if (objAB.Status == 4)
                                                {
                                                    objAB.ReviewedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                    objAB.ReviewedDate = DateTime.Now;
                                                    objAB.VerifiedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                    objAB.VerifiedDate = DateTime.Now;
                                                }
                                                ObjectSpace.CommitChanges();
                                                if (defaultSetting.Review == EnumRELevelSetup.No)
                                                {
                                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Reviewexported"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                                }
                                                else
                                                {
                                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "reviewsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                                }
                                                ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                                                foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                                                {
                                                    if (parent.Id == "Data Review")
                                                    {
                                                        IObjectSpace os = Application.CreateObjectSpace();
                                                        foreach (ChoiceActionItem child in parent.Items)
                                                        {
                                                            if (child.Id == "RawDataLevel2BatchReview ")
                                                            {
                                                                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                                                                var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                                if (currentUser.Roles.FirstOrDefault(a => a.IsAdministrative == true) != null)
                                                                {
                                                                    //var count = os.GetObjectsCount(typeof(SpreadSheetEntry_AnalyticalBatch), CriteriaOperator.Parse("[Status] = 2"));
                                                                    //cap = child.Caption.Split(new string[] { "(" }, StringSplitOptions.None);
                                                                    //if (count > 0)
                                                                    //{
                                                                    //    child.Caption = cap[0] + "(" + count + ")";
                                                                    //    break;
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    child.Caption = cap[0];
                                                                    //    break;
                                                                    //}
                                                                }
                                                                else
                                                                {
                                                                    IList<AnalysisDepartmentChain> lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] =True", currentUser.Oid));
                                                                    List<Guid> lstTestMethodOid = new List<Guid>();
                                                                    if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                                                                    {
                                                                        foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                                                        {
                                                                            foreach (TestMethod testMethod in departmentChain.TestMethods)
                                                                            {
                                                                                if (!lstTestMethodOid.Contains(testMethod.Oid))
                                                                                {
                                                                                    lstTestMethodOid.Add(testMethod.Oid);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    //IList<SpreadSheetEntry_AnalyticalBatch> lstABS = os.GetObjects<SpreadSheetEntry_AnalyticalBatch>(new InOperator("qcbatchID.Test.Oid", lstTestMethodOid));
                                                                    //if (lstABS.Count > 0)
                                                                    //{
                                                                    //    int count = lstABS.Where(a => a.Status == 2).Select(a => a.Oid).Count();
                                                                    //if (count > 0)
                                                                    //{
                                                                    //    child.Caption = cap[0] + "(" + count + ")";
                                                                    //    break;
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    child.Caption = cap[0];
                                                                    //    break;
                                                                    //}
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    child.Caption = cap[0];
                                                                    //}

                                                                }
                                                                //var count = os.GetObjectsCount(typeof(SpreadSheetEntry_AnalyticalBatch), CriteriaOperator.Parse("[Status] = 2"));
                                                                //var cap = child.Caption.Split(new string[] { "(" }, StringSplitOptions.None);
                                                                //if (count > 0)
                                                                //{
                                                                //    child.Caption = cap[0] + "(" + count + ")";
                                                                //    break;
                                                                //}
                                                                //else
                                                                //{
                                                                //    child.Caption = cap[0];
                                                                //    break;
                                                                //}
                                                            }
                                                        }

                                                    }

                                                }
                                            }

                                            Samplecheckin objSamplecheckin = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", objAB.Jobid));

                                            IList<SampleParameter> lstSamples = View.ObjectSpace.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                            if (lstSamples.Where(j => j.Status == Samplestatus.PendingEntry).Count() == 0)
                                            {
                                                StatusDefinition objStatus = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 19"));
                                                if (objStatus != null)
                                                {
                                                    objSamplecheckin.Index = objStatus;
                                                    ObjectSpace.CommitChanges();

                                                }

                                            }
                                        }
                                        else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview")
                                        {
                                            objAB.Status = 4;
                                            foreach (SpreadSheetEntry spreadSheet in spreadSheets)
                                            {
                                                spreadSheet.IsExported = true;
                                                spreadSheet.VerifiedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                spreadSheet.VerifiedDate = DateTime.Now;
                                                getstatus(spreadSheet.uqSampleParameterID, defaultSetting, resultValidationSetting, resultApprovalSetting);
                                                insertresult = Inserttosampleparameter(spreadSheet, ExportColumns);
                                            }
                                            if (insertresult)
                                            {
                                                if (objAB.Status == 4)
                                                {
                                                    objAB.VerifiedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                    objAB.VerifiedDate = DateTime.Now;
                                                }
                                                ObjectSpace.CommitChanges();
                                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Verifyexported"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                            }
                                            Samplecheckin objSamplecheckin = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", objAB.Jobid));
                                            IList<SampleParameter> lstSamples = View.ObjectSpace.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                            if (lstSamples.Where(j => j.Status == Samplestatus.PendingEntry).Count() == 0)
                                            {
                                                StatusDefinition objStatus = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 20"));
                                                if (objStatus != null)
                                                {
                                                    objSamplecheckin.Index = objStatus;
                                                    ObjectSpace.CommitChanges();

                                                }

                                            }

                                        }
                                        //DataReviewModuleNavigationCount();
                                    }
                                }
                            }
                        }
                        ((ListView)View).ObjectSpace.Refresh();
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

        private void DataReviewBatchDetailsAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (e.CurrentObject != null)
                {
                    SpreadSheetEntry_AnalyticalBatch objABID = (SpreadSheetEntry_AnalyticalBatch)e.CurrentObject;
                    if (objABID != null)
                    {
                        if (objABID.Test != null && objABID.Test.IsPLMTest)
                        {
                            qcbatchinfo.QCBatchOid = objABID.Oid;
                            qcbatchinfo.qcstatus = objABID.Status;
                            ShowViewParameters showViewParameters = new ShowViewParameters(Application.CreateDashboardView(Application.CreateObjectSpace(), "PLM", false));
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
                        else
                        {
                            IObjectSpace os = Application.CreateObjectSpace(typeof(SDMSDCSpreadsheet));
                            SDMSDCSpreadsheet objSheet = os.CreateObject<SDMSDCSpreadsheet>();
                            objSheet.Data = objABID.SpreadSheet;
                            ShowViewParameters showViewParameters = new ShowViewParameters(Application.CreateDetailView(os, "SDMSDCSpreadsheet_DetailView_RawDataBatchReview", false, objSheet));
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
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void getstatus(SampleParameter sp, DefaultSetting defaultSetting, DefaultSetting resultValidationSetting, DefaultSetting resultApprovalSetting)
        {
            try
            {
                //if (defaultSetting.REValidate == EnumRELevelSetup.Yes)
                if (resultValidationSetting != null && resultValidationSetting.Select && sp != null)
                {
                    sp.Status = Samplestatus.PendingValidation;
                    sp.OSSync = true;
                }
                //else if (defaultSetting.REValidate == EnumRELevelSetup.No && defaultSetting.REApprove == EnumRELevelSetup.Yes)
                else if ((resultValidationSetting == null || !resultValidationSetting.Select) && (resultApprovalSetting != null && resultApprovalSetting.Select) && sp != null)
                {
                    sp.Status = Samplestatus.PendingApproval;
                    sp.OSSync = true;
                    sp.ValidatedDate = DateTime.Now;
                    sp.ValidatedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                }
                //else if (defaultSetting.REValidate == EnumRELevelSetup.No && defaultSetting.REApprove == EnumRELevelSetup.No)
                else if ((resultValidationSetting == null || !resultValidationSetting.Select) && (resultApprovalSetting == null || !resultApprovalSetting.Select) && sp != null)
                {
                    sp.Status = Samplestatus.PendingReporting;
                    sp.OSSync = true;
                    sp.ValidatedDate = DateTime.Now;
                    sp.ValidatedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    sp.AnalyzedDate = DateTime.Now;
                    sp.AnalyzedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private bool Inserttosampleparameter(SpreadSheetEntry spreadSheet, DataRow[] ExportColumns)
        {
            try
            {
                if (spreadSheet.RunNo == 1)
                {
                    for (int i = 0; i < ExportColumns.Length; i++)
                    {
                        if (spreadSheet.uqSampleParameterID != null)
                        {
                            var sproperty = spreadSheet.uqSampleParameterID.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name == ExportColumns[i]["ExportSample"].ToString()).ToList();
                            IObjectSpace os = Application.MainWindow.View.ObjectSpace;
                            if (sproperty.Count == 1)
                            {

                                string strFieldName = ExportColumns[i]["FieldName"].ToString();
                                if (strFieldName[0] == '%')
                                {
                                    strFieldName = strFieldName.Replace(@"%", "P_");
                                }

                                List<XPMemberInfo> tmproperty = new List<XPMemberInfo>();
                                tmproperty = spreadSheet.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name == strFieldName).ToList();
                                if (strFieldName == "Units" && tmproperty.Count == 1)
                                {
                                    Unit uni = os.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]=?", tmproperty[0].GetValue(spreadSheet)));
                                    if (uni != null)
                                    {
                                        sproperty[0].SetValue(spreadSheet.uqSampleParameterID, uni);
                                    }
                                }
                                else if (tmproperty.Count == 1)
                                {
                                    sproperty[0].SetValue(spreadSheet.uqSampleParameterID, tmproperty[0].GetValue(spreadSheet));
                                }
                                spreadSheet.uqSampleParameterID.Rollback = string.Empty;
                                //if (ExportColumns[i]["FieldName"].ToString()[0] == '%')
                                //{
                                //    ExportColumns[i]["FieldName"] = ExportColumns[i]["FieldName"].ToString().Replace(@"%", "P_");
                                //}
                                //else
                                //{
                                //    ExportColumns[i]["FieldName"] = ExportColumns[i]["FieldName"].ToString();
                                //}
                                //List<XPMemberInfo> tmproperty = new List<XPMemberInfo>();
                                //tmproperty = spreadSheet.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name == ExportColumns[i]["FieldName"].ToString()).ToList();
                                //if (ExportColumns[i]["FieldName"].ToString() == "Units" && tmproperty.Count == 1)
                                //{
                                //    Unit uni = os.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]=?", tmproperty[0].GetValue(spreadSheet)));
                                //    if (uni != null)
                                //    {
                                //        sproperty[0].SetValue(spreadSheet.uqSampleParameterID, uni);
                                //    }
                                //}
                                //else if (tmproperty.Count == 1)
                                //{
                                //    sproperty[0].SetValue(spreadSheet.uqSampleParameterID, tmproperty[0].GetValue(spreadSheet));
                                //}
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Exportcolumnissue"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return false;
                            }
                        }
                    }
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

        private DataTable Getdtparsingsamplefields(int templateid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SpreadSheetEntry_GetTemplateInfo_SP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] param = new SqlParameter[1];
                        param[0] = new SqlParameter("@TEMPLATEID", templateid);
                        cmd.Parameters.AddRange(param);
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[1].Copy();
                            }
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        private void DataReviewBatchRollbackAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                bool result = true;
                foreach (SpreadSheetEntry_AnalyticalBatch objAB in e.SelectedObjects)
                {
                    if (objAB.Test != null && objAB.Test.IsPLMTest)
                    {
                        if (ObjectSpace.GetObjects<SampleParameter>().Where(j => j.QCBatchID != null && j.QCBatchID.qcseqdetail.Oid == objAB.Oid && j.Status == Samplestatus.Reported).Count() > 0)
                        {
                            result = false;
                            break;
                        }
                    }
                    else
                    {
                        IList<SpreadSheetEntry> spreadSheets = ObjectSpace.GetObjects<SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", objAB.Oid));
                        if (spreadSheets != null && spreadSheets.FirstOrDefault(i => i.uqSampleParameterID != null && i.uqSampleParameterID.Status == Samplestatus.Reported) != null)
                        {
                            result = false;
                            break;
                        }
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
                    showViewParameters.CreatedView.Caption = "Raw Data Batch Rollback";
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += Dc_Accepting;
                    dc.ViewClosed += Dc_ViewClosed;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    foreach (SpreadSheetEntry_AnalyticalBatch objAB in e.SelectedObjects)
                    {

                        string[] ids = objAB.Jobid.Split(';');
                        foreach (string obj in ids)
                        {

                            Samplecheckin objSamplecheckin = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", obj));
                            IList<SampleParameter> lstSamples = ObjectSpace.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                            if (lstSamples.Where(i => i.Status == Samplestatus.PendingEntry).Count() == 0)
                            {
                                if (objSamplecheckin != null)
                                {
                                    StatusDefinition objStatus = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 16"));
                                    if (objStatus != null)
                                    {
                                        objSamplecheckin.Index = objStatus;
                                        ObjectSpace.CommitChanges();

                                    }

                                }
                            }
                        }


                    }
                }
                else
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

        private void Dc_ViewClosed(object sender, EventArgs e)
        {
            try
            {
                Application.MainWindow.View.ObjectSpace.Refresh();
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
                string strreason = ((SDMSRollback)e.AcceptActionArgs.CurrentObject).PopupRollBackReason;
                if (!string.IsNullOrEmpty(strreason))
                {
                    if (Application.MainWindow.View.SelectedObjects.Count > 0)
                    {
                        IObjectSpace os = Application.MainWindow.View.ObjectSpace;
                        foreach (SpreadSheetEntry_AnalyticalBatch objAB in Application.MainWindow.View.SelectedObjects)
                        {
                            if (objAB.Test != null && objAB.Test.IsPLMTest)
                            {
                                foreach (SampleParameter sp in ObjectSpace.GetObjects<SampleParameter>().Where(j => j.QCBatchID != null && j.QCBatchID.qcseqdetail.Oid == objAB.Oid).ToList())
                                {
                                    SDMSRollback objrollback = os.CreateObject<SDMSRollback>();
                                    objrollback.QCType = sp.QCBatchID.QCType.QCTypeName;
                                    objrollback.SampleLoginID = sp.Samplelogin;
                                    objrollback.TestMethodID = sp.QCBatchID.qcseqdetail.Test;
                                    objrollback.PreviousStatus = sp.Status.ToString();
                                    objrollback.CurrentStatus = Samplestatus.PendingEntry.ToString();
                                    objrollback.RollbackBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    objrollback.RollbackDate = DateTime.Now;
                                    objrollback.ResultEnteredBy = sp.EnteredBy;
                                    objrollback.ResultEnteredDate = (DateTime)sp.EnteredDate;
                                    objrollback.SampleParameterID = sp;
                                    objrollback.RollBackReason = strreason;
                                    objrollback.AnalyticalBatchID = objAB.AnalyticalBatchID;
                                    sp.ApprovedBy = null;
                                    sp.ApprovedDate = null;
                                    sp.ValidatedBy = null;
                                    sp.ValidatedDate = null;
                                    sp.OSSync = true;
                                    sp.Status = Samplestatus.PendingEntry;
                                }
                                objAB.ReviewedBy = null;
                                objAB.ReviewedDate = null;
                                objAB.VerifiedBy = null;
                                objAB.VerifiedDate = null;
                                objAB.Status = 1;
                            }
                            else
                            {
                                DataTable dtselectedsamplefields = Getdtparsingsamplefields(objAB.TemplateID);
                                if (dtselectedsamplefields != null && dtselectedsamplefields.Rows.Count > 0)
                                {
                                    DataRow[] ExportColumns = dtselectedsamplefields.Select("[ExportSample] <> '' and [ExportSample] is not null");
                                    IList<SpreadSheetEntry> spreadSheets = os.GetObjects<SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", objAB.Oid));
                                    foreach (SpreadSheetEntry spreadSheet in spreadSheets)
                                    {
                                        SDMSRollback objrollback = os.CreateObject<SDMSRollback>();
                                        //objrollback.QCBatchID = spreadSheet.uqQCBatchID.QCBatchID;
                                        objrollback.QCType = spreadSheet.uqQCTypeID.QCTypeName;
                                        objrollback.SampleLoginID = spreadSheet.uqSampleParameterID.Samplelogin;
                                        objrollback.TestMethodID = spreadSheet.UQTESTPARAMETERID.TestMethod;
                                        objrollback.PreviousStatus = spreadSheet.uqSampleParameterID.Status.ToString();
                                        objrollback.CurrentStatus = Samplestatus.PendingEntry.ToString();
                                        objrollback.RollbackBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objrollback.RollbackDate = DateTime.Now;
                                        objrollback.ResultEnteredBy = spreadSheet.EnteredBy;
                                        objrollback.ResultEnteredDate = (DateTime)spreadSheet.EnteredDate;
                                        objrollback.SampleParameterID = spreadSheet.uqSampleParameterID;
                                        objrollback.RollBackReason = strreason;
                                        objrollback.AnalyticalBatchID = objAB.AnalyticalBatchID;
                                        if (spreadSheet.IsExported && spreadSheet.uqSampleParameterID != null)
                                        {
                                            Deleteresultinsampleparameter(spreadSheet, ExportColumns);
                                            spreadSheet.uqSampleParameterID.ApprovedBy = null;
                                            spreadSheet.uqSampleParameterID.ApprovedDate = null;
                                            spreadSheet.uqSampleParameterID.ValidatedBy = null;
                                            spreadSheet.uqSampleParameterID.ValidatedDate = null;
                                            spreadSheet.uqSampleParameterID.OSSync = true;
                                        }
                                        if (spreadSheet.uqSampleParameterID != null)
                                        {
                                            spreadSheet.uqSampleParameterID.Status = Samplestatus.PendingEntry;
                                        }
                                        spreadSheet.IsComplete = false;
                                        spreadSheet.IsExported = false;
                                        spreadSheet.ReviewedBy = null;
                                        spreadSheet.ReviewedDate = null;
                                        spreadSheet.VerifiedBy = null;
                                        spreadSheet.VerifiedDate = null;
                                    }
                                    objAB.ReviewedBy = null;
                                    objAB.ReviewedDate = null;
                                    objAB.VerifiedBy = null;
                                    objAB.VerifiedDate = null;
                                    objAB.Status = 1;
                                }
                            }
                        }
                        os.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                        //ShowNavigationItemController ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
                        //if (ShowNavigationController != null && ShowNavigationController.ShowNavigationItemAction != null)
                        //{
                        //    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                        //    {
                        //        if (parent.Id == "Reporting")
                        //        {
                        //            foreach (ChoiceActionItem child in parent.Items)
                        //            {
                        //                if (child.Id == "Custom Reporting")
                        //                {
                        //                    int count = 0;
                        //                    IObjectSpace objSpace = Application.CreateObjectSpace();
                        //                    using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                        //                    {
                        //                        lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingReporting' And Not IsNullOrEmpty([Samplelogin.JobID.JobID])");
                        //                        lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                        //                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        //                        List<object> jobid = new List<object>();
                        //                        if (lstview != null)
                        //                        {
                        //                            foreach (ViewRecord rec in lstview)
                        //                                jobid.Add(rec["Toid"]);
                        //                        }

                        //                        count = jobid.Count;
                        //                    }
                        //                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        //                    if (count > 0)
                        //                    {
                        //                        child.Caption = cap[0] + " (" + count + ")";
                        //                    }
                        //                    else
                        //                    {
                        //                        child.Caption = cap[0];
                        //                    }
                        //                    break;
                        //                }
                        //            }
                        //            break;
                        //        }
                        //    }
                        //}
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

        private void DataReviewBatchHistoryAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SpreadSheetEntry_AnalyticalBatch));
                CollectionSource cs = new CollectionSource(objectSpace, typeof(SpreadSheetEntry_AnalyticalBatch));
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview")
                {
                    cs.Criteria["Filter"] = CriteriaOperator.Parse("[Status] > 2");
                    Frame.SetView(Application.CreateListView("SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview_History", cs, true));
                }
                else
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview")
                {
                    cs.Criteria["Filter"] = CriteriaOperator.Parse("[Status] > 3");
                    Frame.SetView(Application.CreateListView("SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview_History", cs, true));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private bool Deleteresultinsampleparameter(SpreadSheetEntry spreadSheet, DataRow[] ExportColumns)
        {
            try
            {
                if (spreadSheet.RunNo == 1)
                {
                    for (int i = 0; i < ExportColumns.Length; i++)
                    {
                        var sproperty = spreadSheet.uqSampleParameterID.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name == ExportColumns[i]["ExportSample"].ToString()).ToList();
                        if (sproperty.Count == 1)
                        {
                            sproperty[0].SetValue(spreadSheet.uqSampleParameterID, null);
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Exportcolumnissue"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return false;
                        }
                    }
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
        //private void AnalyticalBatchDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView")
        //        {
        //            if (e.SelectedChoiceActionItem.Id == "1M")
        //            {
        //                ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffDay(CreatedDate, Now()) <= 30 And [CreatedDate] Is Not Null");
        //            }
        //            else if (e.SelectedChoiceActionItem.Id == "3M")
        //            {
        //                ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 3 And [CreatedDate] Is Not Null");
        //            }
        //            else if (e.SelectedChoiceActionItem.Id == "6M")
        //            {
        //                ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 6 And [CreatedDate] Is Not Null");
        //            }
        //            else if (e.SelectedChoiceActionItem.Id == "1Y")
        //            {
        //                ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");
        //            }
        //            else if (e.SelectedChoiceActionItem.Id == "2Y")
        //            {
        //                ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 2 And [CreatedDate] Is Not Null");
        //            }
        //            else if (e.SelectedChoiceActionItem.Id == "ALL")
        //            {
        //                ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[CreatedDate] Is Not Null");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void DataReviewBatchHistoryDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview_History")
                {
                    if (e.SelectedChoiceActionItem.Id == "1M")
                    {
                        ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");
                    }
                    else if (e.SelectedChoiceActionItem.Id == "3M")
                    {
                        ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 3 And [CreatedDate] Is Not Null");
                    }
                    else if (e.SelectedChoiceActionItem.Id == "6M")
                    {
                        ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 6 And [CreatedDate] Is Not Null");
                    }
                    else if (e.SelectedChoiceActionItem.Id == "1Y")
                    {
                        ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");
                    }
                    else if (e.SelectedChoiceActionItem.Id == "2Y")
                    {
                        ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 2 And [CreatedDate] Is Not Null");
                    }
                    else if (e.SelectedChoiceActionItem.Id == "ALL")
                    {
                        ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[CreatedDate] Is Not Null");
                    }
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview_History")
                {
                    if (e.SelectedChoiceActionItem.Id == "1M")
                    {
                        ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");
                    }
                    else if (e.SelectedChoiceActionItem.Id == "3M")
                    {
                        ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 3 And [CreatedDate] Is Not Null");
                    }
                    else if (e.SelectedChoiceActionItem.Id == "6M")
                    {
                        ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate, Now()) <= 6 And [CreatedDate] Is Not Null");
                    }
                    else if (e.SelectedChoiceActionItem.Id == "1Y")
                    {
                        ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 1 And [CreatedDate] Is Not Null");
                    }
                    else if (e.SelectedChoiceActionItem.Id == "2Y")
                    {
                        ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate, Now()) <= 2 And [CreatedDate] Is Not Null");
                    }
                    else if (e.SelectedChoiceActionItem.Id == "ALL")
                    {
                        ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("[CreatedDate] Is Not Null");
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DataReviewModuleNavigationCount()
        {
            ChoiceActionItem DataReview = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "Data Review");
            if (DataReview != null)
            {
                ChoiceActionItem resultValidation = DataReview.Items.FirstOrDefault(i => i.Id == "Result Validation");
                if (resultValidation != null)
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    int count = 0;
                    IList<SampleParameter> lstSamples = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status]='PendingValidation' And [GCRecord] IS NULL AND ([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.GCRecord] IS NULL And [QCBatchID.QCType] Is Not Null) AND ([SubOut] is null Or [SubOut]=False Or ([SubOut] = True And [IsExportedSuboutResult] = True))"));
                    if (lstSamples != null && lstSamples.Count > 0)
                    {
                        string strJobID = string.Join(";", lstSamples.Where(i => !string.IsNullOrEmpty(i.JobID)).Select(i => i.JobID.Trim()));
                        string[] arrJobID = strJobID.Split(';');
                        var a = arrJobID.Distinct();
                        count = arrJobID.Distinct().Count();
                    }

                    var cap = resultValidation.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    if (count > 0)
                    {
                        resultValidation.Caption = cap[0] + " (" + count + ")";
                    }
                    else
                    {
                        resultValidation.Caption = cap[0];
                    }
                }
                ChoiceActionItem resultApproval = DataReview.Items.FirstOrDefault(i => i.Id == "Result Approval");
                if (resultApproval != null)
                {
                    int count = 0;
                    IObjectSpace objSpace = Application.CreateObjectSpace();
                    IList<SampleParameter> lstSamples = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status]='PendingApproval' AND ([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null) AND ([SubOut] is null Or [SubOut]=False Or ([SubOut] = True And [IsExportedSuboutResult] = True)) And [GCRecord] IS NULL"));
                    if (lstSamples != null && lstSamples.Count > 0)
                    {
                        string strJobID = string.Join(";", lstSamples.Where(i => !string.IsNullOrEmpty(i.JobID)).Select(i => i.JobID.Trim()));
                        string[] arrJobID = strJobID.Split(';');
                        count = arrJobID.Distinct().Count();
                    }
                    var cap = resultApproval.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    if (count > 0)
                    {
                        resultApproval.Caption = cap[0] + " (" + count + ")";
                    }
                    else
                    {
                        resultApproval.Caption = cap[0];
                    }
                }

            }
        }
    }
}
