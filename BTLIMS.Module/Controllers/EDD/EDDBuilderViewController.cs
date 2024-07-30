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
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using DevExpress.Web;
using DevExpress.Web.ASPxSpreadsheet;
using DevExpress.Xpo;
using DevExpress.XtraPrinting;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.EDD;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.QC;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class EDDBuilderViewController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        Qcbatchinfo qcbatchinfo = new Qcbatchinfo();
        string CurrentLanguage = string.Empty;
        DynamicReportDesignerConnection ObjReportDesignerInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        EDDInfo objEDDInfo = new EDDInfo();
        private readonly string handlerId;
        public EDDBuilderViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "EDDBuilder_DetailView;" + "EDDReportGenerator_DetailView;" + "EDDReportGenerator_ListView;" + "EDDReportGenerator_DetailView_popup;" + "SDMSDCAB_ListView_EDDBuilder;" + "EDDBuilder_ListView;"
                + "SDMSDCAB_ListView_EDDReportGenerator;" + "EDDBuilder_EDDFields_ListView;" + "EDDQueryBuilder_EDDFieldEditors_ListView;" + "EDDBuilder_EDDQueryBuilders_ListView;" + "EDDBuilder;" + "EDDQueryBuilder_DetailView_SheetEDD;"
                + "SDMSDCSpreadsheet_DetailView_EDDReportGenerator;"+ "SDMSDCSpreadsheet_DetailView_EDDReportGenerator_Copy;";
            handlerId = "EDDBuilderViewController;" + GetHashCode();
            //Preview_EDD.TargetViewId = "EDDReportGenerator_ListView";
            Export_EDD.TargetViewId = "EDDReportGenerator_DetailView;"+ "SDMSDCSpreadsheet_DetailView_EDDReportGenerator;";
            ExportToEDD.TargetViewId = "EDDReportGenerator_DetailView;"+ "SDMSDCSpreadsheet_DetailView_EDDReportGenerator;";
           


        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            try
            {
                WebApplication app = (WebApplication)Application;
                //app.PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (View.Id == "EDDBuilder")
                {
                    DashboardViewItem DVEDDBuilder = ((DashboardView)View).FindItem("EDDBuilder") as DashboardViewItem;
                    if (DVEDDBuilder != null && DVEDDBuilder.InnerView == null)
                    {
                        DVEDDBuilder.CreateControl();
                    }
                    if (DVEDDBuilder != null && DVEDDBuilder.InnerView != null)
                    {
                        if (((DetailView)DVEDDBuilder.InnerView).CurrentObject == null)
                        {
                            IObjectSpace oseddbuilder = Application.CreateObjectSpace();
                            EDDBuilder eDDBuilder = oseddbuilder.CreateObject<EDDBuilder>();
                            ((DetailView)DVEDDBuilder.InnerView).CurrentObject = Application.CreateObjectSpace().GetObject(eDDBuilder);
                        }
                    }
                    DashboardViewItem DVEDDqueryBuilder = ((DashboardView)View).FindItem("EDDQueryBuilder") as DashboardViewItem;
                    if (DVEDDqueryBuilder != null && DVEDDqueryBuilder.InnerView == null)
                    {
                        DVEDDqueryBuilder.CreateControl();
                    }
                    if (DVEDDqueryBuilder != null && DVEDDqueryBuilder.InnerView != null)
                    {
                        if (((DetailView)DVEDDqueryBuilder.InnerView).CurrentObject == null)
                        {
                            IObjectSpace osqueryeddbuilder = Application.CreateObjectSpace();
                            EDDQueryBuilder eDDqueryBuilder = osqueryeddbuilder.CreateObject<EDDQueryBuilder>();
                            ((DetailView)DVEDDqueryBuilder.InnerView).CurrentObject = Application.CreateObjectSpace().GetObject(eDDqueryBuilder); ;
                        }
                    }
                }
                if (View.Id == "EDDReportGenerator_ListView" || View.Id == "EDDBuilder_EDDQueryBuilders_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    //tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
                if (View.Id == "EDDBuilder_DetailView" || View.Id == "EDDBuilder_ListView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                }
                if (View.Id == "EDDReportGenerator_DetailView")
                {
                    View.Closing += View_Closing;
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    Frame.GetController<ModificationsController>().SaveAction.Execute += SaveAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Execute += SaveAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Execute += SaveAction_Execute;
                    //Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                    //Frame.GetController<ModificationsController>().SaveAction.Active.SetItemValue("btnsave", false);
                    //Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("btnsaveclose", false);
                    //Frame.GetController<WebExportController>().SaveAndCloseAction.Active.SetItemValue("btnsaveclose", false);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("btnsavenew", false);
                    //DevExpress.ExpressApp.Web.SystemModule.WebExportController exportcontroller = Frame.GetController<DevExpress.ExpressApp.Web.SystemModule.WebExportController>();
                    //if (exportcontroller != null)
                    //{

                    //}
                    EDDReportGenerator objERG = (EDDReportGenerator)View.CurrentObject;
                    if (objERG != null)
                    {
                        objERG.EDDQueryBuilder = objERG.EDDQueryBuilderDataSource.FirstOrDefault();

                        //string strQuery = null;
                        //string strjobid = null;
                        //string strjobOid = null;
                        //string strclient = null;
                        //string strprojectid = null;
                        //string strprojectname = null;
                        //string strsamplecategory = null;
                        //string strtest = null;
                        //string objmethod = null;
                        //string strEDDBuildoid = null;
                        //string strTemplateName = null;
                        //string strqueryname = null;

                        //if (objERG != null && objERG.EDDQueryBuilder != null)
                        //{
                        //    strqueryname = objERG.EDDQueryBuilder.QueryName;
                        //    strQuery = objERG.EDDQueryBuilder.QueryBuilder;
                        //}
                        //if (!string.IsNullOrEmpty(objERG.JobID))
                        //{
                        //    strjobOid = objERG.JobID;
                        //    List<string> lst = strjobOid.Split(';').ToList();
                        //    string strNewJobid = null;
                        //    foreach (string s in lst)
                        //    {
                        //        CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid]=?", new Guid(s));
                        //        Samplecheckin objGetSample = ObjectSpace.FindObject<Samplecheckin>(criteria1);
                        //        if (lst.Count > 1 && strNewJobid != null)
                        //        {
                        //            strNewJobid = objGetSample.JobID;
                        //            strjobid = strjobid + ",N'" + strNewJobid + "'";
                        //        }
                        //        else
                        //        {
                        //            strNewJobid = objGetSample.JobID;
                        //            strjobid = "N'" + strNewJobid + "'";
                        //        }
                        //    }
                        //}
                        //if (!string.IsNullOrEmpty(objERG.Client))
                        //{
                        //    strclient = objERG.Client;
                        //}
                        //if (!string.IsNullOrEmpty(objERG.ProjectID))
                        //{
                        //    strprojectid = objERG.ProjectID;
                        //}
                        //if (!string.IsNullOrEmpty(objERG.ProjectName))
                        //{
                        //    strprojectname = objERG.ProjectName;
                        //}
                        //if (!string.IsNullOrEmpty(objERG.SampleCategory))
                        //{
                        //    strsamplecategory = objERG.SampleCategory;
                        //}
                        //if (!string.IsNullOrEmpty(objERG.Test))
                        //{
                        //    strtest = objERG.Test;
                        //}
                        //if (!string.IsNullOrEmpty(objERG.Method))
                        //{
                        //    objmethod = objERG.Method;
                        //}
                        //if (objERG.EddTemplate != null && !string.IsNullOrEmpty(objERG.EddTemplate.EDDID))
                        //{
                        //    strEDDBuildoid = objERG.EddTemplate.Oid.ToString();
                        //}
                        //if (objERG.EddTemplate != null && !string.IsNullOrEmpty(objERG.EddTemplate.EDDName))
                        //{
                        //    strTemplateName = objERG.EddTemplate.EDDName;
                        //}

                        //if (strQuery != null)
                        //{
                        //    ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        //    SetConnectionString();
                        //    using (SqlConnection con = new SqlConnection(ObjReportDesignerInfo.WebConfigConn.ToString()))
                        //    {
                        //        using (SqlCommand cmd = new SqlCommand("EDD_RG_SelectDataByQuery_SP", con))
                        //        {
                        //            cmd.CommandType = CommandType.StoredProcedure;
                        //            SqlParameter[] param = new SqlParameter[15];
                        //            param[0] = new SqlParameter("@QueryID", strEDDBuildoid);
                        //            param[1] = new SqlParameter("@TemplateName", strTemplateName);
                        //            param[2] = new SqlParameter("@JobID", strjobOid);
                        //            param[3] = new SqlParameter("@ClientName", strclient);
                        //            param[4] = new SqlParameter("@ProjectID", strprojectid);
                        //            param[5] = new SqlParameter("@ProjectName", strprojectname);
                        //            param[6] = new SqlParameter("@SampleCategory", strsamplecategory);
                        //            param[7] = new SqlParameter("@Test", strtest);
                        //            param[8] = new SqlParameter("@Method", objmethod);
                        //            param[9] = new SqlParameter("@DateReceivedFrom", DBNull.Value);
                        //            param[10] = new SqlParameter("@DateReceivedTo", DBNull.Value);
                        //            param[11] = new SqlParameter("@DateCollectedFrom", DBNull.Value);
                        //            param[12] = new SqlParameter("@DateCollectedTo", DBNull.Value);
                        //            param[13] = new SqlParameter("@QueryName", strqueryname);
                        //            param[14] = new SqlParameter("@JobID_2", strjobid);
                        //            cmd.Parameters.AddRange(param);
                        //            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        //            {
                        //                DataTable dt = new DataTable();
                        //                sda.Fill(dt);
                        //                objEDDInfo.dtsample = dt.Copy();
                        //                objEDDInfo.dtsample.AcceptChanges();
                        //                DashboardViewItem dashboardView = ((DetailView)Application.MainWindow.View).FindItem("EDD_Report_Generator_GridView") as DashboardViewItem;
                        //                if (dashboardView != null && dashboardView.InnerView != null)
                        //                {
                        //                    ASPxGridListEditor gridListEditor = ((ListView)dashboardView.InnerView).Editor as ASPxGridListEditor;
                        //                    if (gridListEditor != null && gridListEditor.Grid != null)
                        //                    {
                        //                        gridListEditor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
                else if(View.Id== "SDMSDCSpreadsheet_DetailView_EDDReportGenerator"||View.Id== "SDMSDCSpreadsheet_DetailView_EDDReportGenerator_Copy")
                {
                    ASPxSpreadsheetPropertyEditor Spreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                    if (Spreadsheet != null)
                    {
                        Spreadsheet.ControlCreated += Spreadsheet_ControlCreated;
                    }
                    if(View.Id == "SDMSDCSpreadsheet_DetailView_EDDReportGenerator")
                    {
                        Export_EDD.Caption = "";
                    }
                }
                //if (View.Id == "SDMSDCAB_ListView_EDDBuilder")
                //{
                //    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //    if(gridListEditor.Grid!=null)
                //    {
                //        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                //        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                //        gridListEditor.Grid.Settings.VerticalScrollableHeight = 600;
                //    }
                //}
                //if (View.Id == "EDDReportGenerator_DetailView")
                //{
                //    objEDDInfo.EDDDataSource = new DataTable();
                //}
                //if (View.Id== "SDMSDCAB_ListView_EDDBuilder")
                //{
                //    Frame.GetController<DevExpress.ExpressApp.Web.SystemModule.WebExportController>().ExportAction.Execute += ExportAction_Execute;
                //    Frame.GetController<DevExpress.ExpressApp.Web.SystemModule.WebExportController>().ExportAction.SelectedItemChanged += ExportAction_SelectedItemChanged;
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                EDDReportGenerator objERG = (EDDReportGenerator)e.CurrentObject;
                if (objERG != null && objERG.EddTemplate != null)
                {
                    Application.ShowViewStrategy.ShowMessage("EDDReportID - " + objERG.EddReportID + " save successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
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
                    spreadsheet.RibbonTabs.Where(ribbonTab => ribbonTab != null).ToList().ForEach(ribbonTab => ribbonTab.Visible = false);
                }
                if (Frame is NestedFrame)
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    if (nestedFrame != null)
                    {
                        CompositeView view = nestedFrame.ViewItem.View;
                        if (view != null)
                        {
                            EDDReportGenerator objERG = view.CurrentObject as EDDReportGenerator;
                            if (objERG != null)
                            {
                                objEDDInfo.EDDID = objERG;
                                SDMSDCSpreadsheet objSheet = View.CurrentObject as SDMSDCSpreadsheet;
                                if (objSheet != null&& objSheet.Data==null)
                                {
                                    objSheet.Data = objERG.ExcelFile.Content;
                                    
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

        private void Spreadsheet_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxSpreadsheet spreadsheet = (ASPxSpreadsheet)sender;
                if (Frame is NestedFrame)
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    if (nestedFrame != null)
                    {
                        CompositeView view = nestedFrame.ViewItem.View;
                        if (view != null)
                        {
                            EDDReportGenerator objERG = view.CurrentObject as EDDReportGenerator;
                            if (objERG != null&&objERG.EddTemplate!=null)
                            {

                                spreadsheet.ReadOnly = true;
                                spreadsheet.RibbonMode = SpreadsheetRibbonMode.None;
                                spreadsheet.ShowFormulaBar = false;
                                //spreadsheet.ShowSheetTabs = false;
                                //spreadsheet.Document.Worksheets.ActiveWorksheet.Cells.FillColor = Color.Transparent;
                                //spreadsheet.Document.Worksheets.ActiveWorksheet.ActiveView.ShowGridlines = false;
                                //spreadsheet.Document.Worksheets.ActiveWorksheet.ActiveView.ShowHeadings = false;
                                IWorkbook objworkbook = spreadsheet.Document;
                                //objworkbook.Worksheets[0].Cells.FillColor = Color.Transparent;
                                //objworkbook.LoadDocument(objERG.ExcelFile.Content, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                                foreach(Worksheet worksheet in objworkbook.Worksheets)
                                {
                                    EDDQueryBuilder objEDDQUery = view.ObjectSpace.FindObject<EDDQueryBuilder>(CriteriaOperator.Parse("[EDDBuilder] = ? And [SheetName] = ?", objERG.EddTemplate, worksheet.Name));
                                    if (objEDDQUery!=null)
                                    {
                                        ColumnCollection columns = worksheet.Columns;
                                        for (int i = 0; i <= columns[worksheet.Columns.LastUsedIndex].Index; i++)
                                        {
                                            Column column = columns[i];
                                            EDDFieldEditor objFE = view.ObjectSpace.FindObject<EDDFieldEditor>(CriteriaOperator.Parse("[FieldName]=? and [EDDQueryBuilder]=?", worksheet.Cells[column.Heading +"1"].Value.ToString(), objEDDQUery));
                                            if (objFE!=null)
                                            {
                                                column.Width = objFE.Width;
                                                worksheet.Cells[column.Heading + "1"].Value = objFE.Caption;
                                                if (objFE.Frozen)
                                                {
                                                    worksheet.FreezeColumns(i);  
                                                }
                                                worksheet.Cells[column.Heading + "1"].Font.Bold = true;
                                                //worksheet.Cells[column.Heading + "1"].Font.Size = 12;
                                                //worksheet.Cells[column.Heading + "1"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                                                CellRange range = worksheet.Range.FromLTRB(column.Index, 0, column.Index, worksheet.Rows.LastUsedIndex);
                                                Borders borders = range.Borders;
                                                borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                            }
                                        } 
                                    }
                                }
                                //spreadsheet.Document.LoadDocument(objERG.ExcelFile.Content, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                            }
                        }
                    }
                }
                else
                {
                    if (objEDDInfo.EDDID!=null)
                    {
                        EDDReportGenerator objERG = View.ObjectSpace.GetObject<EDDReportGenerator>(objEDDInfo.EDDID);
                        if (objERG != null && objERG.EddTemplate != null)
                        {

                            spreadsheet.ReadOnly = true;
                            spreadsheet.RibbonMode = SpreadsheetRibbonMode.None;
                            spreadsheet.ShowFormulaBar = false;
                            IWorkbook objworkbook = spreadsheet.Document;
                            //objworkbook.LoadDocument(objERG.ExcelFile.Content, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                            foreach (Worksheet worksheet in objworkbook.Worksheets)
                            {
                                EDDQueryBuilder objEDDQUery = objERG.EddTemplate.EDDQueryBuilders.FirstOrDefault(i=>i.SheetName== worksheet.Name);
                                //EDDQueryBuilder objEDDQUery = View.ObjectSpace.FindObject<EDDQueryBuilder>(CriteriaOperator.Parse("[EDDBuilder] = ? And [SheetName] = ?", objERG.EddTemplate, worksheet.Name));
                                if (objEDDQUery != null)
                                {
                                    ColumnCollection columns = worksheet.Columns;
                                    for (int i = 0; i <= columns[worksheet.Columns.LastUsedIndex].Index; i++)
                                    {
                                        Column column = columns[i];
                                        EDDFieldEditor objFE = objEDDQUery.EDDFieldEditors.FirstOrDefault(Edd =>Edd.FieldName == worksheet.Cells[column.Heading + "1"].Value.ToString());
                                        //EDDFieldEditor objFE = View.ObjectSpace.FindObject<EDDFieldEditor>(CriteriaOperator.Parse("[FieldName]=? and [EDDQueryBuilder]=?", worksheet.Cells[column.Heading + "1"].Value.ToString(), objEDDQUery));
                                        if (objFE != null)
                                        {
                                            column.Width = objFE.Width;
                                            worksheet.Cells[column.Heading + "1"].Value = objFE.Caption;
                                            if (objFE.Frozen)
                                            {
                                                worksheet.FreezeColumns(i);
                                            }
                                            worksheet.Cells[column.Heading + "1"].Font.Bold = true;
                                            CellRange range = worksheet.Range.FromLTRB(column.Index, 0, column.Index, worksheet.Rows.LastUsedIndex);
                                            Borders borders = range.Borders;
                                            borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
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

        private void DeleteAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (View != null && View.Id == "EDDBuilder_ListView" && View.SelectedObjects.Count > 0)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                foreach (EDDBuilder objEDDBuilder in View.SelectedObjects)
                {
                    IList<EDDQueryBuilder> lstEDDQueryBuilder = os.GetObjects<EDDQueryBuilder>(CriteriaOperator.Parse("[EDDBuilder]=?", objEDDBuilder.Oid));
                    foreach (EDDQueryBuilder objEDDQueryBuilder in lstEDDQueryBuilder.ToList())
                    {
                        os.Delete(objEDDQueryBuilder);
                    }
                }
                os.CommitChanges();
            }
            if (View.Id != null && View.Id == "EDDBuilder_DetailView")
            {
                IObjectSpace os = Application.CreateObjectSpace();
                EDDBuilder objEDDBuilder = (EDDBuilder)View.CurrentObject;
                IList<EDDQueryBuilder> lstEDDQueryBuilder = os.GetObjects<EDDQueryBuilder>(CriteriaOperator.Parse("[EDDBuilder]=?", objEDDBuilder.Oid));
                foreach (EDDQueryBuilder objEDDQueryBuilder in lstEDDQueryBuilder.ToList())
                {
                    os.Delete(objEDDQueryBuilder);
                }
                os.CommitChanges();
            }
        }

        private void View_Closing(object sender, EventArgs e)
        {
            try
            {
                //IObjectSpace objectSpace = Application.CreateObjectSpace();
                //CollectionSource cs = new CollectionSource(objectSpace, typeof(EDDReportGenerator));
                //ListView listView = Application.CreateListView("EDDReportGenerator_ListView", cs, true);
                //Frame.SetView(listView);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        //{
        //    try
        //    {
        //        e.PopupControl.CustomizePopupControl += PopupControl_CustomizePopupControl;
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
        //private void PopupControl_CustomizePopupControl(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupControlEventArgs e)
        //{
        //    try
        //    {
        //        if (View != null && View.Id == "EDDReportGenerator_DetailView_popup")
        //        {
        //            XafPopupWindowControl popupControl = (XafPopupWindowControl)sender;
        //            popupControl.CustomizePopupControl -= PopupControl_CustomizePopupControl;
        //            //string script = CallbackManager.GetScript(handlerId, string.Empty);
        //            //ClientSideEventsHelper.AssignClientHandlerSafe(e.PopupControl, "Closing", $"function(s, e) {{ if(e.closeReason==='CloseButton') {{ {script} }} }}", "DeliverTaskViewController");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
        //protected XafCallbackManager CallbackManager
        //{
        //    get { return ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager; }
        //}
        //void IXafCallbackHandler.ProcessAction(string parameter)
        //{
        //    try
        //    {
        //        WebWindow.CurrentRequestWindow.Close(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                #region EDDQuerycombo
                //if (View.Id == "EDDReportGenerator_DetailView" && e.Object != null)
                //{
                //    if (e.PropertyName == "EDDQueryBuilder")
                //    {
                //        //string strQuery = null;
                //        //string strjobid = null;
                //        //string strjobOid = null;
                //        //string strclient = null;
                //        //string strprojectid = null;
                //        //string strprojectname = null;
                //        //string strsamplecategory = null;
                //        //string strtest = null;
                //        //string objmethod = null;
                //        //string strEDDBuildoid = null;
                //        //string strTemplateName = null;
                //        string strqueryname = null;
                //        ////if (objEDDInfo.EDDRptGtr != null)
                //        ////{

                //        ////}
                //        ////EDDReportGenerator objERG = objEDDInfo.EDDRptGtr;
                //        EDDReportGenerator objERG = (EDDReportGenerator)e.Object;
                //        if (objERG != null && objERG.EDDQueryBuilder != null)
                //        {
                //            strqueryname = objERG.EDDQueryBuilder.QueryName;
                //            //strQuery = objERG.EDDQueryBuilder.QueryBuilder;
                //            objEDDInfo.EddBuildOid = objERG.EDDQueryBuilder.Oid;
                //        }
                //        //if (!string.IsNullOrEmpty(objERG.JobID))
                //        //{
                //        //    strjobOid = objERG.JobID;
                //        //    List<string> lst = strjobOid.Split(';').ToList();
                //        //    string strNewJobid = null;
                //        //    foreach (string s in lst)
                //        //    {
                //        //        CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid]=?", new Guid(s));
                //        //        Samplecheckin objGetSample = ObjectSpace.FindObject<Samplecheckin>(criteria1);
                //        //        if (lst.Count > 1 && strNewJobid != null)
                //        //        {
                //        //            strNewJobid = objGetSample.JobID;
                //        //            strjobid = strjobid + ",N'" + strNewJobid + "'";
                //        //        }
                //        //        else
                //        //        {
                //        //            strNewJobid = objGetSample.JobID;
                //        //            strjobid = "N'" + strNewJobid + "'";
                //        //        }
                //        //    }
                //        //}
                //        //if (!string.IsNullOrEmpty(objERG.Client))
                //        //{
                //        //    strclient = objERG.Client;
                //        //    //List<string> lst = strclient.Split(';').ToList();
                //        //    //string strNewClient = null;
                //        //    //foreach (string s in lst)
                //        //    //{
                //        //    //    CriteriaOperator criteria1 = CriteriaOperator.Parse("[ClientName]=?", new Guid(s));
                //        //    //    Samplecheckin objGetSample = ObjectSpace.FindObject<Samplecheckin>(criteria1);
                //        //    //    if (lst.Count > 1 && strNewClient != null)
                //        //    //    {
                //        //    //        strNewClient = objGetSample.ClientName .CustomerName;
                //        //    //        strclient = strclient + ",N'" + strNewClient + "'";
                //        //    //    }
                //        //    //    else
                //        //    //    {
                //        //    //        strNewClient = objGetSample.ClientName.CustomerName;
                //        //    //        strclient = "N'" + strNewClient + "'";
                //        //    //    }
                //        //    //}
                //        //}
                //        //if (!string.IsNullOrEmpty(objERG.ProjectID))
                //        //{
                //        //    strprojectid = objERG.ProjectID;
                //        //    //List<string> lst = strprojectid.Split(';').ToList();
                //        //    //string strNewProjectID = null;
                //        //    //foreach (string s in lst)
                //        //    //{
                //        //    //    CriteriaOperator criteria1 = CriteriaOperator.Parse("[ProjectID]=?", new Guid(s));
                //        //    //    Samplecheckin objGetSample = ObjectSpace.FindObject<Samplecheckin>(criteria1);
                //        //    //    if (lst.Count > 1 && strNewProjectID != null)
                //        //    //    {
                //        //    //        strNewProjectID = objGetSample.ProjectID.ProjectId ;
                //        //    //        strprojectid = strprojectid + ",N'" + strNewProjectID + "'";
                //        //    //    }
                //        //    //    else
                //        //    //    {
                //        //    //        strNewProjectID = objGetSample.ProjectID.ProjectId;
                //        //    //        strprojectid = "N'" + strNewProjectID + "'";
                //        //    //    }
                //        //    //}
                //        //}
                //        //if (!string.IsNullOrEmpty(objERG.ProjectName))
                //        //{
                //        //    strprojectname = objERG.ProjectName;
                //        //}
                //        //if (!string.IsNullOrEmpty(objERG.SampleCategory))
                //        //{
                //        //    strsamplecategory = objERG.SampleCategory;
                //        //}
                //        //if (!string.IsNullOrEmpty(objERG.Test))
                //        //{
                //        //    strtest = objERG.Test;
                //        //}
                //        //if (!string.IsNullOrEmpty(objERG.Method))
                //        //{
                //        //    objmethod = objERG.Method;
                //        //}
                //        //if (objERG.EddTemplate != null && !string.IsNullOrEmpty(objERG.EddTemplate.EDDID))
                //        //{
                //        //    strEDDBuildoid = objERG.EddTemplate.Oid.ToString();
                //        //}
                //        //if (objERG.EddTemplate != null && !string.IsNullOrEmpty(objERG.EddTemplate.EDDName))
                //        //{
                //        //    strTemplateName = objERG.EddTemplate.EDDName;
                //        //}

                //        if (objERG.ExcelFile != null && !string.IsNullOrEmpty(strqueryname))
                //        {
                //            byte[] file = objERG.ExcelFile.Content;
                //            string fileExtension = Path.GetExtension(objERG.ExcelFile.FileName);
                //            DevExpress.Spreadsheet.Workbook workbook = new DevExpress.Spreadsheet.Workbook();
                //            if (fileExtension == ".xlsx")
                //            {
                //                workbook.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                //            }
                //            else if (fileExtension == ".xls")
                //            {
                //                workbook.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xls);
                //            }
                //            WorksheetCollection worksheets = workbook.Worksheets;
                //            DevExpress.Spreadsheet.Worksheet worksheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == strqueryname);
                //            if (worksheet != null)
                //            {
                //                CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                //                DataTable dt = worksheet.CreateDataTable(range, true);
                //                for (int col = 0; col < range.ColumnCount; col++)
                //                {
                //                    CellValueType cellType = range[0, col].Value.Type;
                //                    for (int r = 1; r < range.RowCount; r++)
                //                    {
                //                        if (cellType != range[r, col].Value.Type)
                //                        {
                //                            if (true)
                //                            {
                //                                dt.Columns[col].DataType = typeof(string);
                //                                break; 
                //                            }
                //                        }
                //                    }
                //                }
                //                DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
                //                exporter.Export();

                //                if (dt != null && dt.Rows.Count > 0)
                //                {
                //                    DataRow row1 = dt.Rows[0];
                //                    if (row1[0].ToString() == dt.Columns[0].Caption)
                //                    {
                //                        row1.Delete();
                //                        dt.AcceptChanges();
                //                    }

                //                    foreach (DataColumn c in dt.Columns)
                //                        c.ColumnName = c.ColumnName.ToString().Trim();
                //                }
                //                objEDDInfo.dtsample = dt.Copy();
                //                objEDDInfo.dtsample.AcceptChanges();
                //                DashboardViewItem dashboardView = ((DetailView)Application.MainWindow.View).FindItem("EDD_Report_Generator_GridView") as DashboardViewItem;
                //                if (dashboardView != null && dashboardView.InnerView != null)
                //                {
                //                    ASPxGridListEditor gridListEditor = ((ListView)dashboardView.InnerView).Editor as ASPxGridListEditor;
                //                    if (gridListEditor != null && gridListEditor.Grid != null)
                //                    {
                //                        gridListEditor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                //                    }
                //                }
                //            }

                //        }
                //        //if (strQuery != null)
                //        //{

                //        //    //ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                //        //    //SetConnectionString();
                //        //    //using (SqlConnection con = new SqlConnection(ObjReportDesignerInfo.WebConfigConn.ToString()))
                //        //    //{
                //        //    //    using (SqlCommand cmd = new SqlCommand("EDD_RG_SelectDataByQuery_SP", con))
                //        //    //    {
                //        //    //        cmd.CommandType = CommandType.StoredProcedure;
                //        //    //        SqlParameter[] param = new SqlParameter[15];
                //        //    //        param[0] = new SqlParameter("@QueryID", strEDDBuildoid);
                //        //    //        param[1] = new SqlParameter("@TemplateName", strTemplateName);
                //        //    //        param[2] = new SqlParameter("@JobID", strjobOid);
                //        //    //        param[3] = new SqlParameter("@ClientName", strclient);
                //        //    //        param[4] = new SqlParameter("@ProjectID", strprojectid);
                //        //    //        param[5] = new SqlParameter("@ProjectName", strprojectname);
                //        //    //        param[6] = new SqlParameter("@SampleCategory", strsamplecategory);
                //        //    //        param[7] = new SqlParameter("@Test", strtest);
                //        //    //        param[8] = new SqlParameter("@Method", objmethod);
                //        //    //        param[9] = new SqlParameter("@DateReceivedFrom", DBNull.Value);
                //        //    //        param[10] = new SqlParameter("@DateReceivedTo", DBNull.Value);
                //        //    //        param[11] = new SqlParameter("@DateCollectedFrom", DBNull.Value);
                //        //    //        param[12] = new SqlParameter("@DateCollectedTo", DBNull.Value);
                //        //    //        param[13] = new SqlParameter("@QueryName", strqueryname);
                //        //    //        param[14] = new SqlParameter("@JobID_2", strjobid);
                //        //    //        cmd.Parameters.AddRange(param);
                //        //    //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //        //    //        {
                //        //    //            con.Open();
                //        //    //            DataTable dt = new DataTable();
                //        //    //            sda.Fill(dt);
                //        //    //            objEDDInfo.dtsample = dt.Copy();
                //        //    //            objEDDInfo.dtsample.AcceptChanges();
                //        //    //            DashboardViewItem dashboardView = ((DetailView)Application.MainWindow.View).FindItem("EDD_Report_Generator_GridView") as DashboardViewItem;
                //        //    //            if (dashboardView != null && dashboardView.InnerView != null)
                //        //    //            {
                //        //    //                ASPxGridListEditor gridListEditor = ((ListView)dashboardView.InnerView).Editor as ASPxGridListEditor;
                //        //    //                if (gridListEditor != null && gridListEditor.Grid != null)
                //        //    //                {
                //        //    //                    gridListEditor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                //        //    //                }
                //        //    //            }
                //        //    //        }
                //        //    //    }
                //        //    //}
                //        //}
                //    }
                //} 
                #endregion
                if (View.Id == "EDDBuilder_DetailView" && e.Object.GetType() == typeof(EDDBuilder))
                {
                    EDDBuilder objEDDB = (EDDBuilder)e.Object;
                    if (e.PropertyName == "Retire")
                    {
                        if (objEDDB.Retire == true)
                        {
                            objEDDB.Active = false;
                        }
                    }
                    if (e.PropertyName == "Active")
                    {
                        if (objEDDB.Active == true)
                        {
                            objEDDB.Retire = false;
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

        //private void ExportAction_SelectedItemChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ASPxGridListEditor liEditor = ((ListView)View).Editor as ASPxGridListEditor;
        //    }
        //    catch(Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }

        //}

        //private void ExportAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        ASPxGridListEditor liEditor = ((ListView)View).Editor as ASPxGridListEditor;
        //        if (liEditor != null)
        //        {
        //            if (e.SelectedChoiceActionItem.Caption.ToUpper().Contains("XLS"))
        //            {
        //                liEditor.ASPxGridViewExporter.WriteXlsToResponse();
        //            }
        //            if (e.SelectedChoiceActionItem.Caption.ToUpper().Contains("CSV"))
        //            {
        //                liEditor.ASPxGridViewExporter.WriteCsvToResponse();
        //            }
        //            if(e.SelectedChoiceActionItem.Caption.ToUpper().Contains("PDF"))
        //            {
        //                liEditor.ASPxGridViewExporter.WritePdfToResponse();
        //            }
        //            if(e.SelectedChoiceActionItem.Caption.ToUpper().Contains("RTF"))
        //            {
        //                liEditor.ASPxGridViewExporter.WriteRtfToResponse();
        //            }
        //            if (e.SelectedChoiceActionItem.Caption.ToUpper().Contains("DOCX"))
        //            {
        //                liEditor.ASPxGridViewExporter.WriteDocxToResponse();
        //            }
        //            if (e.SelectedChoiceActionItem.Caption.ToUpper().Contains("XLSX"))
        //            {
        //                liEditor.ASPxGridViewExporter.WriteXlsxToResponse();
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void NewObjectAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {

                if (View.Id == "EDDBuilder_EDDQueryBuilders_ListView")
                {
                    if (Application != null && Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "EDDBuilder_DetailView")
                    {
                        EDDBuilder objedd = (EDDBuilder)Application.MainWindow.View.CurrentObject;
                        if (objedd != null)
                        {
                            objEDDInfo.EDDBuilderQuerycount = Convert.ToUInt32(objedd.EDDQueryBuilders.Count + 1);
                        }
                    }
                    else if (Frame is DevExpress.ExpressApp.Web.PopupWindow)
                    {
                        DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                        if (popupWindow != null && popupWindow.View != null && popupWindow.View.Id == "EDDBuilder_DetailView")
                        {
                            EDDBuilder objedd = (EDDBuilder)popupWindow.View.CurrentObject;
                            if (objedd != null)
                            {
                                objEDDInfo.EDDBuilderQuerycount = Convert.ToUInt32(objedd.EDDQueryBuilders.Count + 1);
                            }
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    IObjectSpace os = Application.CreateObjectSpace();
                    EDDReportGenerator objErg = os.CreateObject<EDDReportGenerator>();
                    DetailView createdv = Application.CreateDetailView(os, "EDDReportGenerator_DetailView_popup", true, objErg);
                    createdv.Caption = "EDD Report Query";
                    createdv.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showviewparameter = new ShowViewParameters(createdv);
                    showviewparameter.Context = TemplateContext.PopupWindow;
                    showviewparameter.TargetWindow = TargetWindow.NewModalWindow;
                    //showviewparameter.CreatedView.Closing += CreatedView_Closed;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += Dc_Accepting;
                    //dc.CancelAction.Executing += CancelAction_Executing;
                    //dc.Cancelling += Dc_Cancelling;
                    //dc.AcceptAction.Execute += AcceptAction_Execute;
                    dc.CancelAction.Active.SetItemValue("disable", false);
                    dc.CancelAction.Caption = "Clear";
                    dc.CloseOnCurrentObjectProcessing = false;
                    showviewparameter.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showviewparameter, new ShowViewSource(null, null));
                    objEDDInfo.IsViewClose = true;
                    //objEDDInfo.dtsample =null;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void Dc_Cancelling(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DialogController DC = sender as DialogController;
        //        if (DC != null)
        //        {
        //            EDDReportGenerator objERG = (EDDReportGenerator)DC.Frame.View.CurrentObject;
        //            if (objERG != null)
        //            {
        //                objERG.EddTemplate = null;
        //                objERG.ALLEDDTemplateJobID = null;
        //                objERG.ALLTemplateEDDClient = null;
        //                objERG.ALLTemplateEDDMethod = null;
        //                objERG.ALLTemplateEDDProjectID = null;
        //                objERG.ALLTemplateEDDProjectName = null;
        //                objERG.ALLTemplateEDDSampleCategory = null;
        //                objERG.ALLTemplateEDDTest = null;
        //                objERG.DateCollectedFrom = DateTime.MinValue;
        //                objERG.DateCollectedTo = DateTime.MinValue;
        //                objERG.DateReceivedFrom = DateTime.MinValue;
        //                objERG.DateReceivedTo = DateTime.MinValue;

        //            }
        //        }
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void CancelAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    try
        //    {
        //        e.Cancel = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}


        //private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{

        //}

        private void CreatedView_Closed(object sender, EventArgs e)
        {
            try
            {
                if (objEDDInfo.IsViewClose)
                {
                    if (Application.MainWindow.View.Id != "EDDReportGenerator_ListView")
                    {
                        Application.MainWindow.View.Close();
                    }
                    Application.MainWindow.View.Refresh();
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(EDDReportGenerator));
                    ListView listView = Application.CreateListView("EDDReportGenerator_ListView", cs, true);
                    Frame.SetView(listView);
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
                DialogController DC = sender as DialogController;

                if (DC != null)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    EDDReportGenerator objERG = (EDDReportGenerator)e.AcceptActionArgs.CurrentObject;
                    DialogController dc = sender as DialogController;
                    if (objERG != null && objERG.EddTemplate != null && dc!=null )
                    {
                        if ((objERG.DateReceivedTo!=DateTime.MinValue && objERG.DateReceivedFrom == DateTime.MinValue)|| (objERG.DateReceivedTo != DateTime.MinValue && (objERG.DateReceivedFrom > objERG.DateReceivedTo)))
                        {
                            if (objERG.DateReceivedFrom == DateTime.MinValue && objERG.DateReceivedTo != DateTime.MinValue)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "FillReceivedDateFrom"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                e.Cancel = true;
                            }
                            //else if (objERG.DateReceivedFrom != DateTime.MinValue && objERG.DateReceivedTo == DateTime.MinValue)
                            //{
                            //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "FillReceivedDateTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            //}
                            else if (objERG.DateReceivedFrom > objERG.DateReceivedTo)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "lessFromdate"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                e.Cancel = true;
                            }
                        }
                        else if ((objERG.DateCollectedTo!=DateTime.MinValue && objERG.DateCollectedFrom == DateTime.MinValue)|| (objERG.DateCollectedTo != DateTime.MinValue && (objERG.DateCollectedFrom > objERG.DateCollectedTo)))
                        {
                            if (objERG.DateCollectedFrom == DateTime.MinValue && objERG.DateCollectedTo != DateTime.MinValue)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "FillDateCollectedFrom"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                e.Cancel = true;
                            }
                            //else if (objERG.DateCollectedFrom != DateTime.MinValue && objERG.DateCollectedTo == DateTime.MinValue)
                            //{
                            //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "FillReceivedDateTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            //}
                            else if (objERG.DateCollectedFrom > objERG.DateCollectedTo)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "!DateCollectedFrom>DateCollectedTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            //string strQuery = null;
                            string strjobid = null;
                            string strjobOid = null;
                            string strclient = null;
                            string strprojectid = null;
                            string strprojectname = null;
                            string strsamplecategory = null;
                            string strtest = null;
                            string objmethod = null;
                            string strEDDBuildoid = null;
                            string strTemplateName = null;
                            string strqueryname = null;
                            if (!string.IsNullOrEmpty(objERG.JobID))
                            {
                                //strjobOid = objERG.JobID.Replace(';',',');
                                strjobOid = objERG.JobID;
                                List<string> lst = strjobOid.Split(';').Select(s => s.Trim()).ToList();
                                StringBuilder strNewJobid = new StringBuilder();
                                strjobOid = "";
                                for (int i = 0; i < lst.Count; i++)
                                {
                                    string s = lst[i];
                                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[JobID]=?", s);
                                    Samplecheckin objGetSample = ObjectSpace.FindObject<Samplecheckin>(criteria1);
                                    if (objGetSample != null)
                                    {
                                        if (i > 0)
                                        {
                                            strNewJobid.Append(",");
                                        }
                                        strNewJobid.Append(objGetSample.JobID);
                                    }
                                }

                                strjobid = strNewJobid.ToString();
                                //strjobid = strjobOid;

                            }
                            if (!string.IsNullOrEmpty(objERG.Client))
                            {
                                strclient = objERG.Client;
                                List<string> lst = strclient.Split(';').Select(s => s.Trim()).ToList();
                                StringBuilder strNewClient = new StringBuilder();
                                strtest = "";
                                for (int i = 0; i < lst.Count; i++)
                                {
                                    string s = lst[i];
                                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[CustomerName]=?", s);
                                    Customer objGetCustomer = ObjectSpace.FindObject<Customer>(criteria1);
                                    if (objGetCustomer != null)
                                    {
                                        if (i > 0)
                                        {
                                            strNewClient.Append(",");
                                        }
                                        strNewClient.Append(objGetCustomer.CustomerName);
                                    }
                                }

                                strclient = strNewClient.ToString();
                            }
                            if (!string.IsNullOrEmpty(objERG.ProjectID))
                            {
                                strprojectid = objERG.ProjectID;
                                List<string> lst = strprojectid.Split(';').Select(s => s.Trim()).ToList();
                                StringBuilder strNewProjectID = new StringBuilder();
                                strprojectid = "";
                                for (int i = 0; i < lst.Count; i++)
                                {
                                    string s = lst[i];
                                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[ProjectId]=?", s);
                                    Project objGetProjectID = ObjectSpace.FindObject<Project>(criteria1);
                                    if (objGetProjectID != null)
                                    {
                                        if (i > 0)
                                        {
                                            strNewProjectID.Append(",");
                                        }
                                        strNewProjectID.Append(objGetProjectID.ProjectId);
                                    }
                                }

                                strprojectid = strNewProjectID.ToString();
                            }
                            if (!string.IsNullOrEmpty(objERG.ProjectName))
                            {
                                strprojectname = objERG.ProjectName;
                                List<string> lst = strprojectname.Split(';').Select(s => s.Trim()).ToList();
                                StringBuilder strNewProjectID = new StringBuilder();
                                strprojectname = "";
                                for (int i = 0; i < lst.Count; i++)
                                {
                                    string s = lst[i];
                                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid]=?", new Guid(s));
                                    Project objGetProjectID = ObjectSpace.FindObject<Project>(criteria1);
                                    if (objGetProjectID != null)
                                    {
                                        if (i > 0)
                                        {
                                            strNewProjectID.Append(",");
                                        }
                                        strNewProjectID.Append(objGetProjectID.ProjectName);
                                    }
                                }

                                strprojectname = strNewProjectID.ToString();
                            }
                            if (!string.IsNullOrEmpty(objERG.SampleCategory))
                            {
                                strsamplecategory = objERG.SampleCategory;
                                List<string> lst = strsamplecategory.Split(';').Select(s => s.Trim()).ToList();
                                StringBuilder strSampleCategory = new StringBuilder();
                                strsamplecategory = "";
                                for (int i = 0; i < lst.Count; i++)
                                {
                                    string s = lst[i];
                                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid]=?", new Guid(s));
                                    SampleCategory objGetSampleCategory = ObjectSpace.FindObject<SampleCategory>(criteria1);
                                    if (objGetSampleCategory != null)
                                    {
                                        if (i > 0)
                                        {
                                            strSampleCategory.Append(",");
                                        }
                                        strSampleCategory.Append(objGetSampleCategory.SampleCategoryName);
                                    }
                                }

                                strsamplecategory = strSampleCategory.ToString();
                            }
                            if (!string.IsNullOrEmpty(objERG.Test))
                            {
                                strtest = objERG.Test;
                                List<string> lst = strtest.Split(';').Select(s => s.Trim()).ToList();
                                StringBuilder strNewTest = new StringBuilder();
                                strtest = "";
                                for (int i = 0; i < lst.Count; i++)
                                {
                                    string s = lst[i];
                                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid]=?", new Guid(s));
                                    TestMethod objGetTest = ObjectSpace.FindObject<TestMethod>(criteria1);
                                    if (objGetTest != null)
                                    {
                                        if (i > 0)
                                        {
                                            strNewTest.Append(",");
                                        }
                                        strNewTest.Append(objGetTest.TestName);
                                    }
                                }

                                strtest = strNewTest.ToString();
                            }
                            if (!string.IsNullOrEmpty(objERG.Method))
                            {
                                objmethod = objERG.Method;
                                List<string> lst = objmethod.Split(';').Select(s => s.Trim()).ToList();
                                StringBuilder strNewMethod = new StringBuilder();
                                objmethod = "";
                                for (int i = 0; i < lst.Count; i++)
                                {
                                    string s = lst[i];
                                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid]=?", new Guid(s));
                                    Method objGetSample = ObjectSpace.FindObject<Method>(criteria1);
                                    if (objGetSample != null)
                                    {
                                        if (i > 0)
                                        {
                                            strNewMethod.Append(",");
                                        }
                                        strNewMethod.Append(objGetSample.MethodNumber);
                                    }
                                }

                                objmethod = strNewMethod.ToString();
                            }
                            if (objERG.EddTemplate != null && !string.IsNullOrEmpty(objERG.EddTemplate.EDDID))
                            {
                                strEDDBuildoid = objERG.EddTemplate.Oid.ToString();
                            }
                            if (objERG.EddTemplate != null && !string.IsNullOrEmpty(objERG.EddTemplate.EDDName))
                            {
                                strTemplateName = objERG.EddTemplate.EDDName;
                            }
                            objERG.DateCreated = DateTime.Now;
                            objERG.CreatedBy = DC.Frame.View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            if (objERG.EDDQueryBuilderDataSource.Count > 0)
                            {
                                using (ExcelPackage package = new ExcelPackage())
                                {
                                    foreach (EDDQueryBuilder objEDD in objERG.EDDQueryBuilderDataSource.OrderBy(i=>i.QueryName))
                                    {
                                        DataTable dt = new DataTable();
                                        using (SqlConnection con = new SqlConnection(ObjReportDesignerInfo.WebConfigConn.ToString()))
                                        {
                                            using (SqlCommand cmd = new SqlCommand("EDD_RG_SelectDataByQuery_SP", con))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                SqlParameter[] param = new SqlParameter[14];
                                                param[0] = new SqlParameter("@QueryID", strEDDBuildoid);
                                                param[1] = new SqlParameter("@TemplateName", strTemplateName);
                                                //if (objEDD.QueryBuilder.Contains("JobID"))
                                                //{
                                                //    param[2] = new SqlParameter("@JobID", strjobOid);
                                                //}
                                                //else
                                                //{
                                                //    param[2] = new SqlParameter("@JobID", DBNull.Value);
                                                //}
                                                if (objEDD.QueryBuilder.Contains("ClientName"))
                                                {
                                                    param[2] = new SqlParameter("@ClientName", strclient);
                                                }
                                                else
                                                {
                                                    param[2] = new SqlParameter("@ClientName", DBNull.Value);
                                                }
                                                if (objEDD.QueryBuilder.Contains("ProjectID"))
                                                {
                                                    param[3] = new SqlParameter("@ProjectID", strprojectid);
                                                }
                                                else
                                                {
                                                    param[3] = new SqlParameter("@ProjectID", DBNull.Value);
                                                }
                                                if (objEDD.QueryBuilder.Contains("ProjectName"))
                                                {
                                                    param[4] = new SqlParameter("@ProjectName", strprojectname);
                                                }
                                                else
                                                {
                                                    param[4] = new SqlParameter("@ProjectName", DBNull.Value);
                                                }
                                                if (objEDD.QueryBuilder.Contains("SampleCategory"))
                                                {
                                                    param[5] = new SqlParameter("@SampleCategory", strsamplecategory);
                                                }
                                                else
                                                {
                                                    param[5] = new SqlParameter("@SampleCategory", DBNull.Value);
                                                }
                                                if (objEDD.QueryBuilder.Contains("Test"))
                                                {
                                                    param[6] = new SqlParameter("@Test", strtest);
                                                }
                                                else
                                                {
                                                    param[6] = new SqlParameter("@Test", DBNull.Value);
                                                }
                                                if (objEDD.QueryBuilder.Contains("Method"))
                                                {
                                                    param[7] = new SqlParameter("@Method", objmethod);
                                                }
                                                else
                                                {
                                                    param[7] = new SqlParameter("@Method", DBNull.Value);
                                                }
                                                if (objEDD.QueryBuilder.Contains("DateReceived") && objERG.DateReceivedFrom != DateTime.MinValue)
                                                {
                                                    if (objERG.DateReceivedTo == DateTime.MinValue)
                                                    {
                                                        objERG.DateReceivedTo = DateTime.Now;
                                                    }
                                                    else
                                                    {
                                                        DateTime nonNullableDateTime = objERG.DateReceivedTo;
                                                        if (nonNullableDateTime != DateTime.MinValue && nonNullableDateTime.TimeOfDay == TimeSpan.Zero)
                                                        {
                                                            TimeSpan timeSpan = nonNullableDateTime.Subtract(objERG.DateReceivedTo);
                                                            if (timeSpan == TimeSpan.Zero)
                                                            {
                                                                TimeSpan newTime = new TimeSpan(23, 59, 0);
                                                                objERG.DateReceivedTo = objERG.DateReceivedTo.Add(newTime);
                                                            }
                                                        }
                                                    }
                                                    param[8] = new SqlParameter("@DateReceivedFrom", objERG.DateReceivedFrom);
                                                    param[9] = new SqlParameter("@DateReceivedTo", objERG.DateReceivedTo);
                                                }
                                                else
                                                {
                                                    param[8] = new SqlParameter("@DateReceivedFrom", DBNull.Value);
                                                    param[9] = new SqlParameter("@DateReceivedTo", DBNull.Value);
                                                }
                                                if (objEDD.QueryBuilder.Contains("DateCollected") && objERG.DateCollectedFrom != DateTime.MinValue)
                                                {
                                                    if(objERG.DateCollectedTo==DateTime.MinValue)
                                                    {
                                                        objERG.DateCollectedTo = DateTime.Now;
                                                    }
                                                    else
                                                    {
                                                        DateTime nonNullableDateTime = objERG.DateCollectedTo;
                                                        if (nonNullableDateTime != DateTime.MinValue && nonNullableDateTime.TimeOfDay == TimeSpan.Zero)
                                                        {
                                                            TimeSpan timeSpan = nonNullableDateTime.Subtract(objERG.DateCollectedTo);
                                                            if (timeSpan == TimeSpan.Zero)
                                                            {
                                                                TimeSpan newTime = new TimeSpan(23, 59, 0);
                                                                objERG.DateCollectedTo = objERG.DateCollectedTo.Add(newTime);
                                                            }
                                                        }
                                                    }
                                                    param[10] = new SqlParameter("@DateCollectedFrom", objERG.DateCollectedFrom);
                                                    param[11] = new SqlParameter("@DateCollectedTo", objERG.DateCollectedTo);
                                                }
                                                else
                                                {
                                                    param[10] = new SqlParameter("@DateCollectedFrom", DBNull.Value);
                                                    param[11] = new SqlParameter("@DateCollectedTo", DBNull.Value);
                                                }
                                                param[12] = new SqlParameter("@QueryName", objEDD.QueryName);
                                                if (objEDD.QueryBuilder.Contains("JobID"))
                                                {
                                                    param[13] = new SqlParameter("@JobID_2", strjobid);
                                                }
                                                else
                                                {
                                                    param[13] = new SqlParameter("@JobID_2", DBNull.Value);
                                                }
                                                cmd.Parameters.AddRange(param);
                                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                                {
                                                    con.Open();
                                                    sda.Fill(dt);
                                                    con.Close();
                                                }
                                            }
                                        }
                                        // Add the first DataTable to the first worksheet
                                        ExcelWorksheet worksheet1 = package.Workbook.Worksheets.Add(objEDD.SheetName);
                                        worksheet1.Cells["A1"].LoadFromDataTable(dt, true);
                                        for (int i = 0; i < dt.Columns.Count; i++)
                                        {
                                            DataColumn column = dt.Columns[i];
                                            if (column.DataType == typeof(DateTime))
                                            {
                                                int excelColumnIndex = i + 1; // Convert to 1-based index for Excel
                                                worksheet1.Column(excelColumnIndex).Style.Numberformat.Format = "yyyy-mm-dd hh:mm";
                                            }
                                        }
                                        // Add the second DataTable to the second worksheet
                                        //ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add(dt2.TableName);
                                        //worksheet2.Cells["A1"].LoadFromDataTable(dt2, true);

                                        // Save the package to a file
                                        //string filePath = "C:\\path\\to\\your\\output\\file.xlsx";
                                        //FileInfo file = new FileInfo(filePath);
                                        //package.SaveAs(file);
                                    }
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        package.SaveAs(memoryStream);
                                        memoryStream.Position = 0;
                                        if (objERG.ExcelFile == null)
                                        {
                                            objERG.ExcelFile = new FileData(objERG.Session);
                                        }
                                        objERG.ExcelFile.FileName = "DataTables.xlsx";
                                        objERG.ExcelFile.LoadFromStream("DataTables.xlsx", memoryStream);
                                        //objERG.Save();
                                    }
                                }
                            }
                            IObjectSpace os = Application.CreateObjectSpace(typeof(EDDReportGenerator));
                            EDDReportGenerator objErg = os.CreateObject<EDDReportGenerator>();
                            if (objERG != null)
                            {
                                objErg.ALLEDDTemplateJobID = objERG.ALLEDDTemplateJobID;
                                objErg.ALLTemplateEDDSampleCategory = objERG.ALLTemplateEDDSampleCategory;
                                objErg.Client = objERG.Client;
                                objErg.DateCollectedFrom = objERG.DateCollectedFrom;
                                objErg.DateCollectedTo = objERG.DateCollectedTo;
                                objErg.DateReceivedFrom = objERG.DateReceivedFrom;
                                objErg.DateReceivedTo = objERG.DateReceivedTo;
                                objErg.EddTemplate = os.GetObject(objERG.EddTemplate);
                                if (objErg.ExcelFile == null)
                                {
                                    objErg.ExcelFile = new FileData(objErg.Session);
                                }
                                objErg.ExcelFile.FileName = "DataTables.xlsx";
                                objErg.ExcelFile.Content = objERG.ExcelFile.Content;
                                objErg.JobID = objERG.JobID;
                                objErg.Method = objERG.Method;
                                objErg.ProjectID = objERG.ProjectID;
                                objErg.ProjectName = objERG.ProjectName;
                                objErg.SampleCategory = objERG.SampleCategory;
                                objErg.Test = objERG.Test;
                                objErg.CreatedBy = os.GetObject(objERG.CreatedBy);
                                objErg.DateCreated = objERG.DateCreated;
                            }
                            DetailView CreateDetailView = Application.CreateDetailView(os, "EDDReportGenerator_DetailView", true, objErg);
                            CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                            Frame.SetView(CreateDetailView);
                            //Frame.View.CreateControls(); 
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Please select EDD template.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                    }
                }
                //objEDDInfo.IsViewClose = false;
                //DataSet ds = new DataSet();
                //IObjectSpace os = Application.CreateObjectSpace();
                //EDDBuilder objED = os.CreateObject<EDDBuilder>();
                //EDDReportGenerator objERG = e.AcceptActionArgs.CurrentObject as EDDReportGenerator;
                //if (objERG.EddTemplate != null)
                //{
                //    EDDBuilder objed = View.ObjectSpace.GetObject(objERG.EddTemplate);
                //    if (objed != null)
                //    {
                //        objEDDInfo.EddBuildOid = objed.Oid;
                //        objEDDInfo.EDDRptGtr = objERG;
                //        string strQuery = null;
                //        string strjobid = null;
                //        string strclient = null;
                //        string strprojectid = null;
                //        string strprojectname = null;
                //        string strsamplecategory = null;
                //        string strtest = null;
                //        string objmethod = null;
                //        string strEDDBuildoid = null;
                //        string strTemplateName = null;
                //        string strqueryname = null;
                //        foreach (EDDQueryBuilder eddqb in objERG.EddTemplate.EDDQueryBuilders.OrderBy(i => i.SheetName))
                //        {
                //            if (!string.IsNullOrEmpty(eddqb.QueryName))
                //            {
                //                strqueryname = eddqb.QueryName;
                //            }
                //            if (!string.IsNullOrEmpty(eddqb.QueryBuilder))
                //            {
                //                strQuery = eddqb.QueryBuilder;
                //            }
                //            if (!string.IsNullOrEmpty(objERG.JobID))
                //            {
                //                strjobid = objERG.JobID;
                //            }
                //            if (!string.IsNullOrEmpty(objERG.Client))
                //            {
                //                strclient = objERG.Client;
                //            }
                //            if (!string.IsNullOrEmpty(objERG.ProjectID))
                //            {
                //                strprojectid = objERG.ProjectID;
                //            }
                //            if (!string.IsNullOrEmpty(objERG.ProjectName))
                //            {
                //                strprojectname = objERG.ProjectName;
                //            }
                //            if (!string.IsNullOrEmpty(objERG.SampleCategory))
                //            {
                //                strsamplecategory = objERG.SampleCategory;
                //            }
                //            if (!string.IsNullOrEmpty(objERG.Test))
                //            {
                //                strtest = objERG.Test;
                //            }
                //            if (!string.IsNullOrEmpty(objERG.Method))
                //            {
                //                objmethod = objERG.Method;
                //            }
                //            if (objERG.EddTemplate != null && !string.IsNullOrEmpty(objERG.EddTemplate.EDDID))
                //            {
                //                strEDDBuildoid = objERG.EddTemplate.Oid.ToString();
                //            }
                //            if (objERG.EddTemplate != null && !string.IsNullOrEmpty(objERG.EddTemplate.EDDName))
                //            {
                //                strTemplateName = objERG.EddTemplate.EDDName;
                //            }
                //            if (strQuery != null)
                //            {
                //                ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                //                SetConnectionString();
                //                using (SqlConnection con = new SqlConnection(ObjReportDesignerInfo.WebConfigConn.ToString()))
                //                {
                //                    using (SqlCommand cmd = new SqlCommand("EDD_RG_SelectDataByQuery_SP", con))
                //                    {
                //                        cmd.CommandType = CommandType.StoredProcedure;
                //                        SqlParameter[] param = new SqlParameter[14];
                //                        param[0] = new SqlParameter("@QueryID", strEDDBuildoid);
                //                        param[1] = new SqlParameter("@TemplateName", strTemplateName);
                //                        param[2] = new SqlParameter("@JobID", strjobid);
                //                        param[3] = new SqlParameter("@ClientName", strclient);
                //                        param[4] = new SqlParameter("@ProjectID", strprojectid);
                //                        param[5] = new SqlParameter("@ProjectName", strprojectname);
                //                        param[6] = new SqlParameter("@SampleCategory", strsamplecategory);
                //                        param[7] = new SqlParameter("@Test", strtest);
                //                        param[8] = new SqlParameter("@Method", objmethod);
                //                        param[9] = new SqlParameter("@DateReceivedFrom", DBNull.Value);
                //                        param[10] = new SqlParameter("@DateReceivedTo", DBNull.Value);
                //                        param[11] = new SqlParameter("@DateCollectedFrom", DBNull.Value);
                //                        param[12] = new SqlParameter("@DateCollectedTo", DBNull.Value);
                //                        param[13] = new SqlParameter("@QueryName", strqueryname);
                //                        cmd.Parameters.AddRange(param);
                //                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //                        {
                //                            DataTable dt = new DataTable();
                //                            sda.Fill(dt);
                //                            dt.TableName = eddqb.SheetName;
                //                            ds.Tables.Add(dt);
                //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                //                            EDDReportGenerator eDDReportGenerator = objectSpace.CreateObject<EDDReportGenerator>();
                //                            DetailView detailView = Application.CreateDetailView(objectSpace, "EDDReportGenerator_DetailView",true, eDDReportGenerator);
                //                            Frame.SetView(detailView);
                //                            Application.MainWindow.View.Refresh();
                //                            DashboardViewItem dashboardView = ((DetailView)Application.MainWindow.View).FindItem("EDD_Report_Generator_GridView") as DashboardViewItem;
                //                            if (dashboardView != null && dashboardView.InnerView != null)
                //                            {
                //                                ASPxGridListEditor gridListEditor = ((ListView)dashboardView.InnerView).Editor as ASPxGridListEditor;
                //                                if (gridListEditor != null && gridListEditor.Grid != null)
                //                                {
                //                                    gridListEditor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        objEDDInfo.EDDDataSet = ds;
                //    }
                //    Workbook wb = new Workbook();
                //    int sheetno = 0;
                //    for (int i = 0; i <= objEDDInfo.EDDDataSet.Tables.Count - 1; i++)
                //    {
                //        wb.Worksheets[i].Name = i.ToString() + "x";
                //        wb.Worksheets.Add();
                //    }
                //    foreach (DataTable dataTable in objEDDInfo.EDDDataSet.Tables)
                //    {
                //        wb.Worksheets[sheetno].Name = dataTable.TableName;
                //        wb.Worksheets[sheetno].Import(dataTable, true, 0, 0);
                //        sheetno++;
                //    }
                //    //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                //    //string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\"+ objED.EDDName + timeStamp + ".xlsx");
                //    MemoryStream ms = new MemoryStream();
                //    wb.SaveDocument(ms, format: DevExpress.Spreadsheet.DocumentFormat.Xls);
                //    objERG.Report = ms.ToArray();
                //    objERG.Save();
                //    objEDDInfo.EDDRptGtr.Report = ms.ToArray();
                //    objEDDInfo.EDDRptGtr.Save();
                //}
                //else
                //{
                //    e.Cancel = true;
                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectEddTemplate"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                //}
                //DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                //if (model != null)
                //{
                //    DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                //    if (lstViews != null)
                //    {
                //        if (lstViews.FirstOrDefault(i => i.Id == "DVTesting") == null)
                //        {
                //            IModelDashboardView DashViewNode = (IModelDashboardView)lstViews.AddNode<IModelDashboardView>("DVTesting");
                //            IModelDashboardViewItem dashboardViewItem1 = (IModelDashboardViewItem)DashViewNode.AddNode<IModelDashboardViewItem>("A");
                //            IModelListView listViewNode1 = (IModelListView)lstViews.AddNode<IModelListView>("One");
                //            dashboardViewItem1.View = listViewNode1;
                //            IModelDashboardViewItem dashboardViewItem2 = (IModelDashboardViewItem)DashViewNode.AddNode<IModelDashboardViewItem>("B");
                //            IModelListView listViewNode2 = (IModelListView)lstViews.AddNode<IModelListView>("Two");
                //            dashboardViewItem1.View = listViewNode2;
                //            (((DashboardView)DashViewNode).LayoutManager).Items.Add("Tab1", null);// += ChangeLayoutGroupCaptionViewController_ItemCreated;
                //            DashboardView dashboardView = Application.CreateDashboardView(Application.CreateObjectSpace(), DashViewNode.Id, true);
                //            Frame.SetView(dashboardView);
                //        }                        
                //    }
                //}

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
            // Access and customize the target View control.
            try
            {
                //if (View != null && View.Id == "EDDReportGenerator_DetailView")
                //{
                //    Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                //}
                if (View.Id == "EDDReportGenerator_DetailView_popup")
                {
                    EDDReportGenerator objERG = View.CurrentObject as EDDReportGenerator;
                    //if (objERG.EddTemplate == null)
                    //{
                        foreach (ViewItem item in ((DetailView)View).Items)
                        {
                            if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                            {
                                ASPxCheckedLookupStringPropertyEditor propertyeditor = item as ASPxCheckedLookupStringPropertyEditor;
                                if (propertyeditor != null && propertyeditor.AllowEdit && propertyeditor.Editor != null)
                                {
                                    ASPxGridLookup editor = (ASPxGridLookup)propertyeditor.Editor;
                                    if (editor != null)
                                    {
                                        //if (propertyeditor.Id=="JobID"&& !string.IsNullOrEmpty(objERG.ALLEDDTemplateJobID))
                                        //{
                                        //    editor.Enabled = true; 
                                        //}
                                        //else
                                        //{
                                        //    editor.Enabled = false;
                                        //}
                                        //if (propertyeditor.Id=="JobID"&& !string.IsNullOrEmpty(objERG.ALLTemplateEDDClient))
                                        //{
                                        //    editor.Enabled = true; 
                                        //}
                                        //else
                                        //{
                                        //    editor.Enabled = false;
                                        //}
                                        //if (propertyeditor.Id=="JobID"&& !string.IsNullOrEmpty(objERG.ALLTemplateEDDProjectID))
                                        //{
                                        //    editor.Enabled = true; 
                                        //}
                                        //else
                                        //{
                                        //    editor.Enabled = false;
                                        //}
                                        if (propertyeditor.Id == "ProjectName")
                                        {
                                            if (!string.IsNullOrEmpty(objERG.ALLTemplateEDDProjectName))
                                            {
                                                editor.Enabled = true;
                                            }
                                            else
                                            {
                                                editor.Enabled = false;
                                            }
                                        }
                                        if (propertyeditor.Id == "SampleCategory")
                                        {
                                            if (!string.IsNullOrEmpty(objERG.ALLTemplateEDDSampleCategory))
                                            {
                                                editor.Enabled = true;
                                            }
                                            else
                                            {
                                                editor.Enabled = false;
                                            }
                                        }
                                        if (propertyeditor.Id == "Test")
                                        {
                                            if (!string.IsNullOrEmpty(objERG.ALLTemplateEDDTest))
                                            {
                                                editor.Enabled = true;
                                            }
                                            else
                                            {
                                                editor.Enabled = false;
                                            }
                                        }
                                        if (propertyeditor.Id == "Method")
                                        {
                                            if (!string.IsNullOrEmpty(objERG.ALLTemplateEDDMethod))
                                            {
                                                editor.Enabled = true;
                                            }
                                            else
                                            {
                                                editor.Enabled = false;
                                            }
                                        }

                                        editor.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                        editor.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                        editor.GridView.Settings.VerticalScrollableHeight = 90;
                                    }
                                }
                            }
                            if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                            {
                                ASPxDateTimePropertyEditor propertyeditor = item as ASPxDateTimePropertyEditor;
                                if (propertyeditor != null && propertyeditor.AllowEdit && propertyeditor.Editor != null)
                                {
                                    if (propertyeditor.Id == "DateReceivedFrom" || propertyeditor.Id == "DateReceivedTo")
                                    {
                                        if (objERG.IsDateReceived)
                                        {
                                            propertyeditor.AllowEdit.SetItemValue("DateReceived", true);
                                        }
                                        else
                                        {
                                            propertyeditor.AllowEdit.SetItemValue("DateReceived", false);
                                        }
                                    }
                                    if (propertyeditor.Id == "DateCollectedFrom" || propertyeditor.Id == "DateCollectedTo")
                                    {
                                        if (objERG.IsDateCollected)
                                        {
                                            propertyeditor.AllowEdit.SetItemValue("DateCollected", true);
                                        }
                                        else
                                        {
                                            propertyeditor.AllowEdit.SetItemValue("DateCollected", false);
                                        }
                                    }
                                }
                                
                            }
                        }
                        //CallbackManager.RegisterHandler(handlerId, this);
                    //}
                }
                else if (View.Id == "SDMSDCAB_ListView_EDDReportGenerator")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    string strscreenheight = System.Web.HttpContext.Current.Request.Cookies.Get("screenheight").Value;
                    if (!string.IsNullOrEmpty(strscreenheight))
                    {
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = Convert.ToInt32(strscreenheight) - (Convert.ToInt32(strscreenheight) * 40 / 100);
                    }
                    else
                    {
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 700;
                    }
                }
                else if (View.Id == "EDDBuilder_EDDFields_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 400;
                    }
                }
                else if (View.Id == "EDDQueryBuilder_EDDFieldEditors_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        gridListEditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                        gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                        gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 400;
                        gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                        {                        
                            sessionStorage.setItem('FieldFocusedColumn', null); 
                            if(e.cellInfo.column.name.indexOf('Command') !== -1)
                            {                              
                                e.cancel = true;
                            }
                            else
                            {
                                var fieldName = e.cellInfo.column.fieldName;                       
                                sessionStorage.setItem('FieldFocusedColumn', fieldName);  
                            }
                                          
                        }";
                        gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                        { 
                            if (s.IsRowSelectedOnPage(e.elementIndex))  
                            { 
                                var FocusedColumn = sessionStorage.getItem('FieldFocusedColumn');                                
                                var text;                                                            
                                var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn); 
                                if (e.item.name =='CopyToAllCell')
                                {
                                    if (FocusedColumn=='Frozen' || FocusedColumn=='DateTimeNeed' || FocusedColumn=='Visible'|| FocusedColumn=='Width')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        { 
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {
                                                s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                            }
                                        }
                                    }
                                }                                 
                             }
                             e.processOnServer = false;
                        }";
                    }
                }
                else if(View.Id== "EDDReportGenerator_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = gridListEditor.Grid;
                    gridView.ClientInstanceName = View.Id;
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("ReportView", this);
                    gridView.HtmlDataCellPrepared += GridView_HtmlDataCellPrepared;
                }
                //else if(View.Id== "SDMSDCAB_ListView_EDDBuilder"&& objEDDInfo.QueryData!=null)
                //{
                //    ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //    int i = 1;
                //    foreach (DataColumn row in objEDDInfo.QueryData.Columns)
                //{
                //        //EDDFieldEditor objEDDFE = ObjectSpace.CreateObject<EDDFieldEditor>();
                //        //if (objED.EDDFieldEditors.FirstOrDefault(j => j.FieldName == row.ColumnName) == null)
                ////{
                //        //    objEDDFE.FieldName = row.ColumnName;
                //        //    objEDDFE.Caption = row.Caption;
                //        //    objEDDFE.Visible = true;
                //        //    objEDDFE.Width = 100;
                //        //    objEDDFE.Sort = i;
                //        //    objED.EDDFieldEditors.Add(objEDDFE);
                ////}
                //        GridViewDataColumn data_column = new GridViewDataTextColumn();
                //        data_column.FieldName = row.ColumnName;
                //        data_column.Caption = row.Caption;
                //        if (row.DataType == typeof(System.DateTime))
                //        {
                //            data_column.PropertiesEdit.DisplayFormatString = "MM/dd/yyyy";
                //        }
                //        data_column.MinWidth = 150;
                //        data_column.VisibleIndex = i;
                //        data_column.Visible = true;
                //        data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                //        data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                //        data_column.ShowInCustomizationForm = false;
                //        data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                //        i++;
                //        listEditor.Grid.Columns.Add(data_column);
                //        listEditor.Grid.Settings.VerticalScrollableHeight = 100;
                //        listEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                //    }
                //    //if (listEditor.Columns.Count > 10)
                //    {
                //        listEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                //    }
                //    ////else
                //    ////{
                //    ////    listEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Hidden;
                //    ////}
                //    listEditor.Grid.DataSource = objEDDInfo.QueryData;
                //    listEditor.Grid.DataBind();
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void GridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                if (e.DataColumn.FieldName != "EddReportID") return;
                e.Cell.Attributes.Add("ondblclick", "RaiseXafCallback(globalCallbackControl, 'ReportView', this.innerText, '', false);");
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {

                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    e.Items.Remove(e.Items.FindByText("Edit"));
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

        //private void Tar_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        //{
        //    if (View.Id == "EDDReportGenerator_ListView")
        //    {
        //        IObjectSpace objspace = Application.CreateObjectSpace();
        //        EDDReportGenerator objToShow = e.InnerArgs.CurrentObject as EDDReportGenerator;
        //        EDDReportGenerator objERG = objspace.GetObject<EDDReportGenerator>(objToShow);
        //        if (objERG != null)
        //        {
        //            DetailView CreateDetailView = Application.CreateDetailView(objspace, "EDDReportGenerator_DetailView", false, objERG);
        //            CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
        //            Frame.SetView(CreateDetailView);
        //        }
        //        e.Handled = true;
        //    }
        //    //DataTable dt = new DataTable();
        //    //IObjectSpace os = Application.CreateObjectSpace();
        //    //EDDBuilder objED = os.CreateObject<EDDBuilder>();
        //    //EDDReportGenerator objERG = e.InnerArgs.CurrentObject as EDDReportGenerator;
        //    //if (objERG.EddTemplate != null)
        //    //{
        //    //    EDDBuilder objed = View.ObjectSpace.GetObject(objERG.EddTemplate);
        //    //    string strQuery = objed.QueryBuilder;
        //    //    string strjobid = objERG.JobID;
        //    //    string strclient = objERG.Client;
        //    //    string strprojectid = objERG.ProjectID;
        //    //    string strprojectname = objERG.ProjectName;
        //    //    string strsamplecategory = objERG.SampleCategory;
        //    //    string strtest = objERG.Test;
        //    //    string objmethod = objERG.Method;
        //    //    string strQueryID = objed.EDDID;
        //    //    string strTemplateName = objed.EDDName;
        //    //    if (strQuery != null)
        //    //    {
        //    //        ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //    //        SetConnectionString();
        //    //        using (SqlConnection con = new SqlConnection(ObjReportDesignerInfo.WebConfigConn.ToString()))
        //    //        {
        //    //            using (SqlCommand cmd = new SqlCommand("EDD_RG_SelectDataByQuery_SP", con))
        //    //            {
        //    //                cmd.CommandType = CommandType.StoredProcedure;
        //    //                SqlParameter[] param = new SqlParameter[13];
        //    //                param[0] = new SqlParameter("@QueryID", strQueryID);
        //    //                param[1] = new SqlParameter("@TemplateName", strTemplateName);
        //    //                param[2] = new SqlParameter("@JobID", strjobid);
        //    //                param[3] = new SqlParameter("@ClientName", strclient);
        //    //                param[4] = new SqlParameter("@ProjectID", strprojectid);
        //    //                param[5] = new SqlParameter("@ProjectName", strprojectname);
        //    //                param[6] = new SqlParameter("@SampleCategory", strsamplecategory);
        //    //                param[7] = new SqlParameter("@Test", strtest);
        //    //                param[8] = new SqlParameter("@Method", objmethod);
        //    //                param[9] = new SqlParameter("@DateReceivedFrom", DBNull.Value);
        //    //                param[10] = new SqlParameter("@DateReceivedTo", DBNull.Value);
        //    //                param[11] = new SqlParameter("@DateCollectedFrom", DBNull.Value);
        //    //                param[12] = new SqlParameter("@DateCollectedTo", DBNull.Value);
        //    //                cmd.Parameters.AddRange(param);
        //    //                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //    //                {
        //    //                    sda.Fill(dt);
        //    //                    objEDDInfo.dtsample = dt.Copy();
        //    //                    objEDDInfo.EddBuildOid = objed.Oid;
        //    //                    //objEDDInfo.dtsample.AcceptChanges();
        //    //                }
        //    //            }
        //    //        }
        //    //    }

        //    //}
        //}
        private void Grid_Load(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            griddataload(grid);
        }
        private void griddataload(ASPxGridView grid)
        {
            try
            {
                if (View.Id == "SDMSDCAB_ListView_EDDReportGenerator")
                {
                    if (objEDDInfo.dtsample != null/* && objEDDInfo.dtsample.Rows.Count > 0*/)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        EDDReportGenerator objErg = os.CreateObject<EDDReportGenerator>();
                        ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        int i = 1;
                        foreach (DataColumn row in objEDDInfo.dtsample.Columns)
                        {
                            GridViewDataColumn data_column = new GridViewDataTextColumn();
                            data_column.FieldName = row.ColumnName;
                            data_column.Caption = row.Caption;
                            if (row.DataType == typeof(System.DateTime))
                            {
                                data_column.PropertiesEdit.DisplayFormatString = "MM/dd/yyyy";
                            }
                            EDDFieldEditor objEFE = os.FindObject<EDDFieldEditor>(CriteriaOperator.Parse("[EDDQueryBuilder.Oid] = ? And [FieldName] = ?", objEDDInfo.EddBuildOid, row.ColumnName));
                            //IList<EDDFieldEditor> lstFiledEditor = os.GetObjects<EDDFieldEditor>(CriteriaOperator.Parse(""));
                            //EDDFieldEditor obj = lstFiledEditor.FirstOrDefault(a => a.EDDQueryBuilder != null && a.FieldName != null && a.FieldName == row.ColumnName && a.EDDQueryBuilder.EDDBuilder != null && a.EDDQueryBuilder.EDDBuilder.Oid == objEDDInfo.EddBuildOid);
                            if (objEFE != null)
                            {
                                data_column.VisibleIndex = objEFE.Sort;
                                data_column.Visible = objEFE.Visible;
                                data_column.Width = objEFE.Width;
                                data_column.Caption = objEFE.Caption;
                                if (objEFE.Frozen == true)
                                {
                                    data_column.FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                                else
                                {
                                    data_column.FixedStyle = GridViewColumnFixedStyle.None;
                                }
                            }
                            else
                            {
                                data_column.MinWidth = 150;
                                data_column.VisibleIndex = i;
                                data_column.Visible = true;
                            }
                            data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                            data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                            data_column.ShowInCustomizationForm = false;
                            data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                            i++;
                            listEditor.Grid.Columns.Add(data_column);
                            //listEditor.Grid.Settings.VerticalScrollableHeight = 100;
                        }
                        if (listEditor.Columns.Count > 10)
                        {
                            listEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                        }
                        else
                        {
                            listEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Hidden;
                        }
                        listEditor.Grid.DataSource = objEDDInfo.dtsample;
                        listEditor.Grid.DataBind();
                        if (listEditor != null && listEditor.Grid != null)
                        {
                            Session currentSession = ((DevExpress.ExpressApp.Xpo.XPObjectSpace)(os)).Session;
                            UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                            if (objErg.Report == null)
                            {
                                MemoryStream ms = new MemoryStream();
                                ASPxGridView gridView = listEditor.Grid;
                                gridView.TotalSummary.Clear();
                                gridView.ExportToXlsx(ms);
                                if (objEDDInfo != null && objEDDInfo.EDDRptGtr != null)
                                {
                                    EDDReportGenerator newEddRptGtr = uow.GetObjectByKey<EDDReportGenerator>(objEDDInfo.EDDRptGtr.Oid);
                                    if (newEddRptGtr != null)
                                    {
                                        newEddRptGtr.Report = ms.ToArray();
                                        newEddRptGtr.Save();
                                    }
                                }
                            }
                            else
                            {
                                MemoryStream ms = new MemoryStream();
                                ASPxGridView gridView = listEditor.Grid;
                                gridView.TotalSummary.Clear();
                                gridView.ExportToXlsx(ms);
                                objErg.Report = ms.ToArray();
                                objErg.Save();
                                objEDDInfo.EDDRptGtr.Report = ms.ToArray();
                                objEDDInfo.EDDRptGtr.Save();
                            }
                            uow.CommitChanges();
                        }
                        objEDDInfo.dtsample = null;
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
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            try
            {
                //WebApplication app = (WebApplication)Application;
                //app.PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                if (View.Id == "EDDReportGenerator_DetailView")
                {
                    View.Closing -= View_Closing;
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    //Frame.GetController<ModificationsController>().SaveAction.Active.SetItemValue("btnsave", true);
                    //Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("btnsaveclose", true);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("btnsavenew", true);
                    Frame.GetController<ModificationsController>().SaveAction.Execute += SaveAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Execute += SaveAction_Execute;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Execute += SaveAction_Execute;
                }
                if (View.Id == "EDDReportGenerator_ListView" || View.Id == "EDDBuilder_EDDQueryBuilders_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    //tar.CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
                }
                if (View.Id == "EDDReportGenerator_DetailView_popup" || View.Id == "EDDBuilder_DetailView" || View.Id == "EDDBuilder_ListView")
                {
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                }
                else if (View.Id == "SDMSDCSpreadsheet_DetailView_EDDReportGenerator")
                {
                    ASPxSpreadsheetPropertyEditor Spreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                    if (Spreadsheet != null)
                    {
                        Spreadsheet.ControlCreated -= Spreadsheet_ControlCreated;
                    }
                }
                else if (View.Id == "SDMSDCSpreadsheet_DetailView_EDDReportGenerator")
                {
                    Export_EDD.Caption = "Select For Format";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Execute_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                if (Frame is DevExpress.ExpressApp.Web.PopupWindow) //DevExpress.ExpressApp.Web.PopupWindow
                {
                    DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                    if (popupWindow != null && popupWindow.View.Id == "EDDQueryBuilder_DetailView_SheetEDD")
                    {
                        EDDQueryBuilder objED = popupWindow.View.CurrentObject as EDDQueryBuilder;
                        string strQuery = string.Empty;
                        strQuery = objED.QueryBuilder;
                        ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        SetConnectionString();
                        IObjectSpace os = popupWindow.View.ObjectSpace;
                        if (objED.QueryBuilder != null)
                        {
                            DashboardViewItem listResultview = ((DetailView)popupWindow.View).FindItem("EDDBuilder_Dummy") as DashboardViewItem;
                            ASPxGridListEditor listEditor = ((ListView)listResultview.InnerView).Editor as ASPxGridListEditor;
                            ListPropertyEditor liFields = ((DetailView)popupWindow.View).FindItem("EDDFields") as ListPropertyEditor;
                            foreach (GridViewDataColumn column in listEditor.Grid.Columns.Cast<GridViewDataColumn>().ToList())
                            {
                                listEditor.Grid.Columns.Remove(column);
                            }
                            using (SqlConnection con = new SqlConnection(ObjReportDesignerInfo.WebConfigConn.ToString()))
                            {
                                using (SqlCommand cmd = new SqlCommand("EDD_ExecuteQuery_SP", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    SqlParameter[] param = new SqlParameter[1];
                                    param[0] = new SqlParameter("@colQuery", strQuery);
                                    cmd.Parameters.AddRange(param);
                                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                    {
                                        sda.Fill(dt);
                                        int i = 1;
                                        List<string> lstDataTableColumnName = dt.Columns.Cast<DataColumn>().Select(j => j.ColumnName).ToList();
                                        List<string> lstFieldName = objED.EDDFieldEditors.Cast<EDDFieldEditor>().Select(j => j.FieldName).ToList();
                                        List<string> lstUnMatchingFields = lstFieldName.Except(lstDataTableColumnName).ToList();
                                        //foreach (EDDFieldEditor objField in objED.EDDFieldEditors.ToList())
                                        //{
                                        //    if (lstFieldName.Contains(objField.FieldName))
                                        //    {
                                        //        objED.EDDFieldEditors.Remove(objField);
                                        //    }
                                        //}
                                        foreach (DataColumn row in dt.Columns)
                                        {
                                            EDDFieldEditor objEDDFE = os.CreateObject<EDDFieldEditor>();
                                            EDDFieldEditor objEDDFE1 = os.FindObject<EDDFieldEditor>(CriteriaOperator.Parse("[FieldName]=? and [EDDQueryBuilder]=?", row.ColumnName, objED));
                                            //if (objED.EDDFieldEditors.FirstOrDefault(j => j.FieldName == row.ColumnName) == null)
                                            if (objEDDFE1 == null)
                                            {
                                                objEDDFE.FieldName = row.ColumnName;
                                                objEDDFE.Caption = row.Caption;
                                                objEDDFE.Visible = true;
                                                objEDDFE.Width = 400;
                                                objEDDFE.Sort = i;
                                                objED.EDDFieldEditors.Add(objEDDFE);
                                            }
                                            GridViewDataColumn data_column = new GridViewDataTextColumn();
                                            data_column.FieldName = row.ColumnName;
                                            data_column.Caption = row.Caption;
                                            if (row.DataType == typeof(System.DateTime))
                                            {
                                                data_column.PropertiesEdit.DisplayFormatString = "MM/dd/yyyy";
                                            }
                                            data_column.MinWidth = 150;
                                            data_column.VisibleIndex = i;
                                            data_column.Visible = true;
                                            data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                                            data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                                            data_column.ShowInCustomizationForm = false;
                                            data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                                            i++;
                                            listEditor.Grid.Columns.Add(data_column);
                                            listEditor.Grid.Settings.VerticalScrollableHeight = 100;
                                        }
                                        List<string> missingColumns = objED.EDDFieldEditors.Where(eddfe => !dt.Columns.Cast<DataColumn>().Any(column => column.ColumnName.Equals(eddfe.FieldName, StringComparison.OrdinalIgnoreCase))).Select(eddfe => eddfe.FieldName).ToList();
                                        if (missingColumns.Count>0)
                                        {
                                            IList<EDDFieldEditor> lstdeleteFields = os.GetObjects<EDDFieldEditor>(new InOperator("FieldName", missingColumns));
                                            foreach(EDDFieldEditor objEDD in lstdeleteFields)
                                            {
                                                objED.EDDFieldEditors.Remove(objEDD);
                                            }
                                        }
                                        //if (listEditor.Columns.Count > 10)
                                        {
                                            listEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                                        }
                                        ////else
                                        ////{
                                        ////    listEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Hidden;
                                        ////}
                                        listEditor.Grid.DataSource = dt;
                                        objEDDInfo.QueryData = dt;
                                        listEditor.Grid.DataBind();
                                        liFields.Refresh();
                                    }
                                }
                            }
                        }
                    }
                }

                //if(Frame is DevExpress.ExpressApp.Web.PopupWindow) //DevExpress.ExpressApp.Web.PopupWindow
                //{
                //    DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                //    if (popupWindow != null && popupWindow.View.Id == "EDDBuilder_DetailView")
                //    {
                //        EDDBuilder objED = popupWindow.View.CurrentObject as EDDBuilder;
                //        string strQuery = string.Empty;
                //        strQuery = objED.QueryBuilder;
                //        ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                //        SetConnectionString();
                //        IObjectSpace os = popupWindow.View.ObjectSpace;
                //        if (objED.QueryBuilder != null)
                //        {
                //            DashboardViewItem listResultview = ((DetailView)popupWindow.View).FindItem("EDDBuilder_Dummy") as DashboardViewItem;
                //            ASPxGridListEditor listEditor = ((ListView)listResultview.InnerView).Editor as ASPxGridListEditor;
                //            ListPropertyEditor liFields = ((DetailView)popupWindow.View).FindItem("EDDFields") as ListPropertyEditor;
                //            foreach (GridViewDataColumn column in listEditor.Grid.Columns.Cast<GridViewDataColumn>().ToList())
                //            {
                //                listEditor.Grid.Columns.Remove(column);
                //            }
                //            using (SqlConnection con = new SqlConnection(ObjReportDesignerInfo.WebConfigConn.ToString()))
                //            {
                //                using (SqlCommand cmd = new SqlCommand("EDD_ExecuteQuery_SP", con))
                //                {
                //                    cmd.CommandType = CommandType.StoredProcedure;
                //                    SqlParameter[] param = new SqlParameter[1];
                //                    param[0] = new SqlParameter("@colQuery", strQuery);
                //                    cmd.Parameters.AddRange(param);
                //                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //                    {
                //                        sda.Fill(dt);
                //                        int i = 1;
                //                        List<string> lstDataTableColumnName = dt.Columns.Cast<DataColumn>().Select(j => j.ColumnName).ToList();
                //                        List<string> lstFieldName = objED.EDDFields.Cast<EDDFieldEditor>().Select(j => j.FieldName).ToList();
                //                        List<string> lstUnMatchingFields = lstFieldName.Except(lstDataTableColumnName).ToList();
                //                        foreach (EDDFieldEditor objField in objED.EDDFields.ToList())
                //                        {
                //                            if (lstFieldName.Contains(objField.FieldName))
                //                            {
                //                                objED.EDDFields.Remove(objField);
                //                            }
                //                        }
                //                        foreach (DataColumn row in dt.Columns)
                //                        {
                //                            EDDFieldEditor objEDDFE = os.CreateObject<EDDFieldEditor>();
                //                            if (objED.EDDFields.FirstOrDefault(j => j.FieldName == row.ColumnName) == null)
                //                            {
                //                                objEDDFE.FieldName = row.ColumnName;
                //                                objEDDFE.Caption = row.Caption;
                //                                objEDDFE.Visible = true;
                //                                objEDDFE.Width = 100;
                //                                objEDDFE.Sort = i;
                //                                objED.EDDFields.Add(objEDDFE);
                //                            }
                //                            GridViewDataColumn data_column = new GridViewDataTextColumn();
                //                            data_column.FieldName = row.ColumnName;
                //                            data_column.Caption = row.Caption;
                //                            if (row.DataType == typeof(System.DateTime))
                //                            {
                //                                data_column.PropertiesEdit.DisplayFormatString = "MM/dd/yyyy";
                //                            }
                //                            data_column.MinWidth = 150;
                //                            data_column.VisibleIndex = i;
                //                            data_column.Visible = true;
                //                            data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                //                            data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                //                            data_column.ShowInCustomizationForm = false;
                //                            data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                //                            i++;
                //                            listEditor.Grid.Columns.Add(data_column);
                //                            listEditor.Grid.Settings.VerticalScrollableHeight = 100;
                //                        }
                //                        //if (listEditor.Columns.Count > 10)
                //                        {
                //                            listEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                //                        }
                //                        ////else
                //                        ////{
                //                        ////    listEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Hidden;
                //                        ////}
                //                        listEditor.Grid.DataSource = dt;
                //                        listEditor.Grid.DataBind();
                //                        liFields.Refresh();
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                //if (Application != null && Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "EDDBuilder_DetailView")
                //{
                //    EDDBuilder objED = Application.MainWindow.View.CurrentObject as EDDBuilder;
                //    string strQuery = string.Empty;
                //    strQuery = objED.QueryBuilder;
                //    ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                //    SetConnectionString();
                //    IObjectSpace os = Application.MainWindow.View.ObjectSpace;
                //    if (objED.QueryBuilder != null)
                //    {
                //        DashboardViewItem listResultview = ((DetailView)Application.MainWindow.View).FindItem("EDDBuilder_Dummy") as DashboardViewItem;
                //        ASPxGridListEditor listEditor = ((ListView)listResultview.InnerView).Editor as ASPxGridListEditor;
                //        ListPropertyEditor liFields = ((DetailView)Application.MainWindow.View).FindItem("EDDFields") as ListPropertyEditor;
                //        foreach (GridViewDataColumn column in listEditor.Grid.Columns.Cast<GridViewDataColumn>().ToList())
                //        {
                //            listEditor.Grid.Columns.Remove(column);
                //        }
                //        using (SqlConnection con = new SqlConnection(ObjReportDesignerInfo.WebConfigConn.ToString()))
                //        {
                //            using (SqlCommand cmd = new SqlCommand("EDD_ExecuteQuery_SP", con))
                //            {
                //                cmd.CommandType = CommandType.StoredProcedure;
                //                SqlParameter[] param = new SqlParameter[1];
                //                param[0] = new SqlParameter("@colQuery", strQuery);
                //                cmd.Parameters.AddRange(param);
                //                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //                {
                //                    sda.Fill(dt);
                //                    int i = 1;
                //                    List<string> lstDataTableColumnName = dt.Columns.Cast<DataColumn>().Select(j => j.ColumnName).ToList();
                //                    List<string> lstFieldName = objED.EDDFields.Cast<EDDFieldEditor>().Select(j => j.FieldName).ToList();
                //                    List<string> lstUnMatchingFields = lstFieldName.Except(lstDataTableColumnName).ToList();
                //                    foreach (EDDFieldEditor objField in objED.EDDFields.ToList())
                //                    {
                //                        if (lstFieldName.Contains(objField.FieldName))
                //                        {
                //                            objED.EDDFields.Remove(objField);
                //                        }
                //                    }
                //                    foreach (DataColumn row in dt.Columns)
                //                    {
                //                        EDDFieldEditor objEDDFE = os.CreateObject<EDDFieldEditor>();
                //                        if (objED.EDDFields.FirstOrDefault(j => j.FieldName == row.ColumnName) == null)
                //                        {
                //                            objEDDFE.FieldName = row.ColumnName;
                //                            objEDDFE.Caption = row.Caption;
                //                            objEDDFE.Visible = true;
                //                            objEDDFE.Width = 100;
                //                            objEDDFE.Sort = i;
                //                            objED.EDDFields.Add(objEDDFE);
                //                        }
                //                        GridViewDataColumn data_column = new GridViewDataTextColumn();
                //                        data_column.FieldName = row.ColumnName;
                //                        data_column.Caption = row.Caption;
                //                        if (row.DataType == typeof(System.DateTime))
                //                        {
                //                            data_column.PropertiesEdit.DisplayFormatString = "MM/dd/yyyy";
                //                        }
                //                        data_column.MinWidth = 150;
                //                        data_column.VisibleIndex = i;
                //                        data_column.Visible = true;
                //                        data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                //                        data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                //                        data_column.ShowInCustomizationForm = false;
                //                        data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                //                        i++;
                //                        listEditor.Grid.Columns.Add(data_column);
                //                        listEditor.Grid.Settings.VerticalScrollableHeight = 100;
                //                    }
                //                    //if (listEditor.Columns.Count > 10)
                //                    {
                //                        listEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                //                    }
                //                    ////else
                //                    ////{
                //                    ////    listEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Hidden;
                //                    ////}
                //                    listEditor.Grid.DataSource = dt;
                //                    listEditor.Grid.DataBind();
                //                    liFields.Refresh();
                //                }
                //            }
                //        }
                //    }
                //}
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
                ObjReportDesignerInfo.LDMSQLUserID = connectionstring[3].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLPassword = connectionstring[4].Split('=').GetValue(1).ToString();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Preview_EDD_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //try
            //{
            //    IObjectSpace os = Application.CreateObjectSpace();
            //    EDDReportGenerator objERG = e.CurrentObject as EDDReportGenerator;
            //    if (e.SelectedObjects.Count == 1)
            //    {
            //        if (objERG != null && objERG.EddTemplate != null && objERG.EddTemplate.EDDQueryBuilders != null)
            //        {
            //            Workbook wb = new Workbook();
            //            DataSet ds = new DataSet();
            //            EDDBuilder objed = View.ObjectSpace.GetObject(objERG.EddTemplate);
            //            if (objed != null)
            //            {
            //                string strQuery = null;
            //                string strjobid = null;
            //                string strclient = null;
            //                string strprojectid = null;
            //                string strprojectname = null;
            //                string strsamplecategory = null;
            //                string strtest = null;
            //                string objmethod = null;
            //                string strEDDBuildoid = null;
            //                string strTemplateName = null;
            //                string strqueryname = null;
            //                foreach (EDDQueryBuilder eddqb in objERG.EddTemplate.EDDQueryBuilders.OrderBy(i => i.SheetName))
            //                {
            //                    if (!string.IsNullOrEmpty(eddqb.QueryName))
            //                    {
            //                        strqueryname = eddqb.QueryName;
            //                    }
            //                    if (!string.IsNullOrEmpty(eddqb.QueryBuilder))
            //                    {
            //                        strQuery = eddqb.QueryBuilder;
            //                    }
            //                    if (!string.IsNullOrEmpty(objERG.JobID))
            //                    {
            //                        strjobid = objERG.JobID;
            //                    }
            //                    if (!string.IsNullOrEmpty(objERG.Client))
            //                    {
            //                        strclient = objERG.Client;
            //                    }
            //                    if (!string.IsNullOrEmpty(objERG.ProjectID))
            //                    {
            //                        strprojectid = objERG.ProjectID;
            //                    }
            //                    if (!string.IsNullOrEmpty(objERG.ProjectName))
            //                    {
            //                        strprojectname = objERG.ProjectName;
            //                    }
            //                    if (!string.IsNullOrEmpty(objERG.SampleCategory))
            //                    {
            //                        strsamplecategory = objERG.SampleCategory;
            //                    }
            //                    if (!string.IsNullOrEmpty(objERG.Test))
            //                    {
            //                        strtest = objERG.Test;
            //                    }
            //                    if (!string.IsNullOrEmpty(objERG.Method))
            //                    {
            //                        objmethod = objERG.Method;
            //                    }
            //                    if (objERG.EddTemplate != null && !string.IsNullOrEmpty(objERG.EddTemplate.EDDID))
            //                    {
            //                        strEDDBuildoid = objERG.EddTemplate.Oid.ToString();
            //                    }
            //                    if (objERG.EddTemplate != null && !string.IsNullOrEmpty(objERG.EddTemplate.EDDName))
            //                    {
            //                        strTemplateName = objERG.EddTemplate.EDDName;
            //                    }
            //                    if (strQuery != null)
            //                    {
            //                        ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //                        SetConnectionString();
            //                        using (SqlConnection con = new SqlConnection(ObjReportDesignerInfo.WebConfigConn.ToString()))
            //                        {
            //                            using (SqlCommand cmd = new SqlCommand("EDD_RG_SelectDataByQuery_SP", con))
            //                            {
            //                                DataTable dt = new DataTable();
            //                                cmd.CommandType = CommandType.StoredProcedure;
            //                                SqlParameter[] param = new SqlParameter[14];
            //                                param[0] = new SqlParameter("@QueryID", strEDDBuildoid);
            //                                param[1] = new SqlParameter("@TemplateName", strTemplateName);
            //                                param[2] = new SqlParameter("@JobID", strjobid);
            //                                param[3] = new SqlParameter("@ClientName", strclient);
            //                                param[4] = new SqlParameter("@ProjectID", strprojectid);
            //                                param[5] = new SqlParameter("@ProjectName", strprojectname);
            //                                param[6] = new SqlParameter("@SampleCategory", strsamplecategory);
            //                                param[7] = new SqlParameter("@Test", strtest);
            //                                param[8] = new SqlParameter("@Method", objmethod);
            //                                param[9] = new SqlParameter("@DateReceivedFrom", DBNull.Value);
            //                                param[10] = new SqlParameter("@DateReceivedTo", DBNull.Value);
            //                                param[11] = new SqlParameter("@DateCollectedFrom", DBNull.Value);
            //                                param[12] = new SqlParameter("@DateCollectedTo", DBNull.Value);
            //                                param[13] = new SqlParameter("@QueryName", strqueryname);
            //                                cmd.Parameters.AddRange(param);
            //                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            //                                {
            //                                    sda.Fill(dt);
            //                                    dt.TableName = eddqb.SheetName;
            //                                    ds.Tables.Add(dt);
            //                                }
            //                                //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            //                                //string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".csv");
            //                                //Workbook wb1 = new Workbook();
            //                                //Worksheet worksheet0 = wb1.Worksheets[0];
            //                                //worksheet0.Name = "data";
            //                                //wb1.Worksheets[0].Import(dt, true, 0, 0);
            //                                //wb1.SaveDocument(strExportedPath);
            //                                //string[] path = strExportedPath.Split('\\');
            //                                //int arrcount = path.Count();
            //                                //int sc = arrcount - 2;
            //                                //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
            //                                //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));

            //                            }
            //                        }
            //                    }
            //                }
            //            }

            //            int sheetno = 0;
            //            for (int i = 0; i <= ds.Tables.Count - 1; i++)
            //            {
            //                wb.Worksheets[i].Name = i.ToString() + "x";
            //                wb.Worksheets.Add();
            //            }
            //            foreach (DataTable dataTable in ds.Tables)
            //            {
            //                wb.Worksheets[sheetno].Name = dataTable.TableName;
            //                wb.Worksheets[sheetno].Import(dataTable, true, 0, 0);
            //                sheetno++;
            //            }
            //            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            //            if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\")) == false)
            //            {
            //                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\"));
            //            }
            //            string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\" + objERG.EddTemplate.EDDName + timeStamp + ".xlsx");
            //            wb.SaveDocument(strExportedPath);
            //            FileInfo fileInfo = new FileInfo(strExportedPath);
            //            if (fileInfo.Exists)
            //            {
            //                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            //                HttpContext.Current.Response.AppendHeader("Content-Disposition", "filename=EDDReportGenerator.xlsx");
            //                HttpContext.Current.Response.BinaryWrite(File.ReadAllBytes(strExportedPath));
            //                HttpContext.Current.Response.Flush();
            //                //HttpContext.Current.Response.End();
            //                HttpContext.Current.Response.SuppressContent = true;
            //                HttpContext.Current.ApplicationInstance.CompleteRequest();
            //            }
            //        }
            //        //if (objErg.Report != null)
            //        //{
            //        //    //HttpContext.Current.Response.Clear();
            //        //    //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";//"application/excel";
            //        //    //HttpContext.Current.Response.AddHeader("Content-disposition", "filename=EDDReportGenerator.xlsx");
            //        //    //HttpContext.Current.Response.OutputStream.Write(objErg.Report, 0, objErg.Report.Length);
            //        //    //HttpContext.Current.Response.OutputStream.Flush();
            //        //    //HttpContext.Current.Response.OutputStream.Close();
            //        //    //HttpContext.Current.Response.Flush();
            //        //    //HttpContext.Current.Response.Close();
            //        //}
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            //}
        }
        private void EDDExport_SelectedItemChanged(object sender, System.EventArgs e)
        {
            try
            {
                //if (View != null && (View.Id == "SDMSDCSpreadsheet_DetailView_EDDReportGenerator"||View.Id== "EDDReportGenerator_DetailView") && Export_EDD.SelectedItem!=null /*&& Export_EDD.SelectedItem.Id!="NonAvalable"*/)
                //{
                //    IObjectSpace os = Application.CreateObjectSpace();
                //    SDMSDCSpreadsheet objERG = null;
                //    ASPxSpreadsheetPropertyEditor Spreadsheet = null;
                //    if(View.Id=="EDDReportGenerator_DetailView")
                //    {
                //        DashboardViewItem DBView= ((DetailView)View).FindItem("EDD_Report_Generator_GridView") as DashboardViewItem;
                //        if(DBView != null&& DBView.InnerView == null)
                //        {
                //            DBView.CreateControl();
                //        }
                //        if (DBView!=null)
                //        {
                //            objERG = (SDMSDCSpreadsheet)DBView.InnerView.CurrentObject;
                //            Spreadsheet = ((DetailView)DBView.InnerView).FindItem("Data") as ASPxSpreadsheetPropertyEditor; 
                //        }
                //    }
                //    else
                //    {
                //         objERG = (SDMSDCSpreadsheet)View.CurrentObject;
                //         Spreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                //    }
                //    Workbook workbook = new Workbook();
                //    workbook.LoadDocument(objERG.Data, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                //    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                //    string rootPath = HttpContext.Current.Server.MapPath("~/App_Data/");
                //    string FileName = objEDDInfo.EDDID.EddTemplate.EDDName;
                //    if (Spreadsheet!=null)
                //    {
                //        if (Export_EDD.SelectedItem.Id == "ExporttoExcelAllSheet")
                //        {
                //            string selectedPath = string.Empty;
                //            Thread t = new Thread((ThreadStart)(() =>
                //            {
                //                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                //                saveFileDialog1.FileName = FileName + timeStamp;
                //                //saveFileDialog1.Filter = "Execl files (*.xls)|*.xls";
                //                saveFileDialog1.Filter = "Excel Files (*.xlsx)|*.xlsx";
                //                saveFileDialog1.FilterIndex = 2;
                //                saveFileDialog1.RestoreDirectory = true;

                //                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //                {
                //                    selectedPath = saveFileDialog1.FileName;
                //                }
                //            }));
                //            t.SetApartmentState(ApartmentState.STA);
                //            t.Start();
                //            t.Join();
                //            workbook.SaveDocument(selectedPath);
                //            FileInfo fileInfo = new FileInfo(selectedPath);
                //            if (fileInfo.Exists)
                //            {
                //                Application.ShowViewStrategy.ShowMessage("File download completed", InformationType.Success, 3000, InformationPosition.Top);
                //            }
                //            //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                //            //if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\")) == false)
                //            //{
                //            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\"));
                //            //}
                //            //string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\" + objEDDInfo.EDDID.EddTemplate.EDDName + timeStamp + ".xlsx");
                //            //workbook.SaveDocument(strExportedPath);
                //            //string[] path = strExportedPath.Split('\\');
                //            //int arrcount = path.Count();
                //            //int sc = arrcount - 3;
                //            //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                //            //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                //        }
                //        else if (Export_EDD.SelectedItem.Id == "ExportToExcelSingleSheet")
                //        {
                //            Worksheet originalSheet = Spreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet;
                //            Workbook singleSheetWorkbook = new Workbook();
                //            Worksheet newSheet = singleSheetWorkbook.Worksheets.Add(originalSheet.Name);
                //            newSheet.CopyFrom(originalSheet);
                //            string selectedPath = string.Empty;
                //            Thread t = new Thread((ThreadStart)(() =>
                //            {
                //                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                //                saveFileDialog1.FileName = FileName + timeStamp;
                //                saveFileDialog1.Filter = "Excel Files (*.xlsx)|*.xlsx";
                //                saveFileDialog1.FilterIndex = 2;
                //                saveFileDialog1.RestoreDirectory = true;

                //                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //                {
                //                    selectedPath = saveFileDialog1.FileName;
                //                }
                //            }));
                //            t.SetApartmentState(ApartmentState.STA);
                //            t.Start();
                //            t.Join();
                //            singleSheetWorkbook.SaveDocument(selectedPath);
                //            FileInfo fileInfo = new FileInfo(selectedPath);
                //            if (fileInfo.Exists)
                //            {
                //                Application.ShowViewStrategy.ShowMessage("File download completed", InformationType.Success, 3000, InformationPosition.Top);
                //            }
                //            //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                //            //if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\")) == false)
                //            //{
                //            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\"));
                //            //}
                //            //string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\" + objEDDInfo.EDDID.EddTemplate.EDDName + timeStamp + ".xlsx");
                //            //singleSheetWorkbook.SaveDocument(strExportedPath);
                //            //string[] path = strExportedPath.Split('\\');
                //            //int arrcount = path.Count();
                //            //int sc = arrcount - 3;
                //            //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                //            //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));


                //        }
                //        else if (Export_EDD.SelectedItem.Id == "ExporttoCSV")
                //        {

                //            if (Export_EDD.SelectedItem.Id == "ExporttoCSV")
                //            {
                //                Worksheet originalSheet = Spreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet;
                //                Workbook singleSheetWorkbook = new Workbook();
                //                Worksheet newSheet = singleSheetWorkbook.Worksheets.Add(originalSheet.Name);
                //                newSheet.CopyFrom(originalSheet);
                //                string selectedPath = string.Empty;
                //                Thread t = new Thread((ThreadStart)(() =>
                //                {
                //                    System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                //                    saveFileDialog1.FileName = FileName + timeStamp;
                //                    saveFileDialog1.Filter = "CSV files (*.csv)|*.csv";
                //                    saveFileDialog1.FilterIndex = 1;
                //                    saveFileDialog1.RestoreDirectory = true;

                //                    if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //                    {
                //                        selectedPath = saveFileDialog1.FileName;
                //                    }
                //                }));
                //                t.SetApartmentState(ApartmentState.STA);
                //                t.Start();
                //                t.Join();
                //                singleSheetWorkbook.SaveDocument(selectedPath);
                //                FileInfo fileInfo = new FileInfo(selectedPath);
                //                if (fileInfo.Exists)
                //                {
                //                    Application.ShowViewStrategy.ShowMessage("File download completed", InformationType.Success, 3000, InformationPosition.Top);
                //                }
                //            }

                //            //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                //            //string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\" + objEDDInfo.EDDID.EddTemplate.EDDName + timeStamp + ".csv");
                //            //singleSheetWorkbook.SaveDocument(strExportedPath);
                //            //string[] path = strExportedPath.Split('\\');
                //            //int arrcount = path.Count();
                //            //int sc = arrcount - 3;
                //            //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                //            //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                //        }
                //        else if (Export_EDD.SelectedItem.Id == "ExporttoTXT")
                //        {
                //            Worksheet originalSheet = Spreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet;
                //            Workbook singleSheetWorkbook = new Workbook();
                //            Worksheet newSheet = singleSheetWorkbook.Worksheets.Add(originalSheet.Name);
                //            newSheet.CopyFrom(originalSheet);
                //            string selectedPath = string.Empty;
                //            Thread t = new Thread((ThreadStart)(() =>
                //            {
                //                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                //                saveFileDialog1.FileName = FileName + timeStamp;
                //                saveFileDialog1.Filter = "Text files (*.txt)|*.txt";
                //                saveFileDialog1.FilterIndex = 1;
                //                saveFileDialog1.RestoreDirectory = true;

                //                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //                {
                //                    selectedPath = saveFileDialog1.FileName;
                //                }
                //            }));
                //            t.SetApartmentState(ApartmentState.STA);
                //            t.Start();
                //            t.Join();
                //            singleSheetWorkbook.SaveDocument(selectedPath);
                //            FileInfo fileInfo = new FileInfo(selectedPath);
                //            if (fileInfo.Exists)
                //            {
                //                Application.ShowViewStrategy.ShowMessage("File download completed", InformationType.Success, 3000, InformationPosition.Top);
                //            }

                //        } 
                //    }
                    
                //}

                //}
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
                if (parameter.Contains("ED"))
                {
                    
                    EDDReportGenerator objEDD = ObjectSpace.FindObject<EDDReportGenerator>(CriteriaOperator.Parse("[EddReportID]=?", parameter));
                    if (objEDD != null)
                    {
                        objEDDInfo.EDDID = objEDD;
                        IObjectSpace os = Application.CreateObjectSpace(typeof(SDMSDCSpreadsheet));
                        SDMSDCSpreadsheet objSheet = os.CreateObject<SDMSDCSpreadsheet>();
                        objSheet.Data = objEDD.ExcelFile.Content;
                        DetailView dv = Application.CreateDetailView(os, "SDMSDCSpreadsheet_DetailView_EDDReportGenerator", false, objSheet);
                        dv.Caption = "EDDID - "+parameter;
                        ShowViewParameters showViewParameters = new ShowViewParameters(dv);
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
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ExportToEDD_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && (View.Id == "SDMSDCSpreadsheet_DetailView_EDDReportGenerator" || View.Id == "EDDReportGenerator_DetailView") && Export_EDD.SelectedItem != null /*&& Export_EDD.SelectedItem.Id!="NonAvalable"*/)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    SDMSDCSpreadsheet objERG = null;
                    ASPxSpreadsheetPropertyEditor Spreadsheet = null;
                    if (View.Id == "EDDReportGenerator_DetailView")
                    {
                        DashboardViewItem DBView = ((DetailView)View).FindItem("EDD_Report_Generator_GridView") as DashboardViewItem;
                        if (DBView != null && DBView.InnerView == null)
                        {
                            DBView.CreateControl();
                        }
                        if (DBView != null)
                        {
                            objERG = (SDMSDCSpreadsheet)DBView.InnerView.CurrentObject;
                            Spreadsheet = ((DetailView)DBView.InnerView).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                        }
                    }
                    else
                    {
                        objERG = (SDMSDCSpreadsheet)View.CurrentObject;
                        Spreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                    }
                    Workbook workbook = new Workbook();
                    workbook.LoadDocument(objERG.Data, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string rootPath = HttpContext.Current.Server.MapPath("~/App_Data/");
                    string FileName = objEDDInfo.EDDID.EddTemplate.EDDName;
                    if (Spreadsheet != null)
                    {
                        if (Export_EDD.SelectedItem.Id == "ExporttoExcelAllSheet")
                        {
                            string selectedPath = string.Empty;
                            Thread t = new Thread((ThreadStart)(() =>
                            {
                                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                                saveFileDialog1.FileName = FileName + timeStamp;
                                //saveFileDialog1.Filter = "Execl files (*.xls)|*.xls";
                                saveFileDialog1.Filter = "Excel Files (*.xlsx)|*.xlsx";
                                saveFileDialog1.FilterIndex = 2;
                                saveFileDialog1.RestoreDirectory = true;

                                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    selectedPath = saveFileDialog1.FileName;
                                }
                            }));
                            t.SetApartmentState(ApartmentState.STA);
                            t.Start();
                            t.Join();
                            workbook.SaveDocument(selectedPath);
                            FileInfo fileInfo = new FileInfo(selectedPath);
                            if (fileInfo.Exists)
                            {
                                Application.ShowViewStrategy.ShowMessage("File download completed", InformationType.Success, 3000, InformationPosition.Top);
                            }
                            //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            //if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\")) == false)
                            //{
                            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\"));
                            //}
                            //string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\" + objEDDInfo.EDDID.EddTemplate.EDDName + timeStamp + ".xlsx");
                            //workbook.SaveDocument(strExportedPath);
                            //string[] path = strExportedPath.Split('\\');
                            //int arrcount = path.Count();
                            //int sc = arrcount - 3;
                            //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                            //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        }
                        else if (Export_EDD.SelectedItem.Id == "ExportToExcelSingleSheet")
                        {
                            Worksheet originalSheet = Spreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet;
                            Workbook singleSheetWorkbook = new Workbook();
                            Worksheet newSheet = singleSheetWorkbook.Worksheets.Add(originalSheet.Name);
                            newSheet.CopyFrom(originalSheet);
                            string selectedPath = string.Empty;
                            Thread t = new Thread((ThreadStart)(() =>
                            {
                                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                                saveFileDialog1.FileName = FileName + timeStamp;
                                saveFileDialog1.Filter = "Excel Files (*.xlsx)|*.xlsx";
                                saveFileDialog1.FilterIndex = 2;
                                saveFileDialog1.RestoreDirectory = true;

                                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    selectedPath = saveFileDialog1.FileName;
                                }
                            }));
                            t.SetApartmentState(ApartmentState.STA);
                            t.Start();
                            t.Join();
                            singleSheetWorkbook.SaveDocument(selectedPath);
                            FileInfo fileInfo = new FileInfo(selectedPath);
                            if (fileInfo.Exists)
                            {
                                Application.ShowViewStrategy.ShowMessage("File download completed", InformationType.Success, 3000, InformationPosition.Top);
                            }
                            //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            //if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\")) == false)
                            //{
                            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\"));
                            //}
                            //string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\" + objEDDInfo.EDDID.EddTemplate.EDDName + timeStamp + ".xlsx");
                            //singleSheetWorkbook.SaveDocument(strExportedPath);
                            //string[] path = strExportedPath.Split('\\');
                            //int arrcount = path.Count();
                            //int sc = arrcount - 3;
                            //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                            //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));


                        }
                        else if (Export_EDD.SelectedItem.Id == "ExporttoCSV")
                        {

                            if (Export_EDD.SelectedItem.Id == "ExporttoCSV")
                            {
                                Worksheet originalSheet = Spreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet;
                                Workbook singleSheetWorkbook = new Workbook();
                                Worksheet newSheet = singleSheetWorkbook.Worksheets.Add(originalSheet.Name);
                                newSheet.CopyFrom(originalSheet);
                                string selectedPath = string.Empty;
                                Thread t = new Thread((ThreadStart)(() =>
                                {
                                    System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                                    saveFileDialog1.FileName = FileName + timeStamp;
                                    saveFileDialog1.Filter = "CSV files (*.csv)|*.csv";
                                    saveFileDialog1.FilterIndex = 1;
                                    saveFileDialog1.RestoreDirectory = true;

                                    if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        selectedPath = saveFileDialog1.FileName;
                                    }
                                }));
                                t.SetApartmentState(ApartmentState.STA);
                                t.Start();
                                t.Join();
                                singleSheetWorkbook.SaveDocument(selectedPath);
                                FileInfo fileInfo = new FileInfo(selectedPath);
                                if (fileInfo.Exists)
                                {
                                    Application.ShowViewStrategy.ShowMessage("File download completed", InformationType.Success, 3000, InformationPosition.Top);
                                }
                            }

                            //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            //string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\EDD\" + objEDDInfo.EDDID.EddTemplate.EDDName + timeStamp + ".csv");
                            //singleSheetWorkbook.SaveDocument(strExportedPath);
                            //string[] path = strExportedPath.Split('\\');
                            //int arrcount = path.Count();
                            //int sc = arrcount - 3;
                            //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                            //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        }
                        else if (Export_EDD.SelectedItem.Id == "ExporttoTXT")
                        {
                            Worksheet originalSheet = Spreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet;
                            Workbook singleSheetWorkbook = new Workbook();
                            Worksheet newSheet = singleSheetWorkbook.Worksheets.Add(originalSheet.Name);
                            newSheet.CopyFrom(originalSheet);
                            string selectedPath = string.Empty;
                            Thread t = new Thread((ThreadStart)(() =>
                            {
                                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                                saveFileDialog1.FileName = FileName + timeStamp;
                                saveFileDialog1.Filter = "Text files (*.txt)|*.txt";
                                saveFileDialog1.FilterIndex = 1;
                                saveFileDialog1.RestoreDirectory = true;

                                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    selectedPath = saveFileDialog1.FileName;
                                }
                            }));
                            t.SetApartmentState(ApartmentState.STA);
                            t.Start();
                            t.Join();
                            singleSheetWorkbook.SaveDocument(selectedPath);
                            FileInfo fileInfo = new FileInfo(selectedPath);
                            if (fileInfo.Exists)
                            {
                                Application.ShowViewStrategy.ShowMessage("File download completed", InformationType.Success, 3000, InformationPosition.Top);
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
    }
}
