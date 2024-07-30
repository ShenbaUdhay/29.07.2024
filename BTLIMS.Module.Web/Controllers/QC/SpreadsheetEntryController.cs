using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Chart.Web;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.Office.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.AuditTrail;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using DevExpress.Web;
using DevExpress.Web.ASPxSpreadsheet;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using DevExpress.XtraCharts.Web;
using DevExpress.XtraReports.UI;
using DynamicReportBusinessLayer;
using LDM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using Microsoft.VisualBasic;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Mold;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.QC;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.SDMS;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.Mold;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace LDM.Module.Web.Controllers.QC
{
    public partial class SpreadsheetEntryController : ViewController, IXafCallbackHandler
    {
        int sdmsstatus = 0;
        bool exportfile = false;
        IObjectSpace os;
        IObjectSpace Popupos;
        bool ISfullscreen;
        bool IsRefresh = false;
        bool IStransfered = false;
        MessageTimer timer = new MessageTimer();
        public static Dictionary<string, string> diSSColumnsToExportColumns = new Dictionary<string, string>();
        public static Dictionary<int, string> diIndexToColumn = new Dictionary<int, string>();
        Qcbatchinfo qcbatchinfo = new Qcbatchinfo();
        string constr = string.Empty;
        string CurrentLanguage = string.Empty;
        ICallbackManagerHolder sheet;
        DataTable dtDetailData;
        DataTable dtHeaderData;
        DataTable dtDetailDataNew;
        DataRow[] drImportDatasource;
        DataTable dtReporting;
        ResourceManager rm;
        public XtraReport xrReport = new XtraReport();
        FileDataPropertyEditor FilePropertyEditor;
        PermissionInfo objPermissionInfo = new PermissionInfo();
        SDMSInfo sdmsinfo = new SDMSInfo();
        Workbook wb = new Workbook();
        string strCommonTemplateName = "Universal";
        bool testing;
        DataTable dtInstrumentVOC = new DataTable();
        AnaliytialBatchinfo objABinfo = new AnaliytialBatchinfo();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        CaseNarativeInfo CNInfo = new CaseNarativeInfo();
        ShowNavigationItemController ShowNavigationController;

        public SpreadsheetEntryController()
        {
            InitializeComponent();
            TargetViewId = "SDMSDCSpreadsheet_DetailView;"
                + "SDMSDCImport_DetailView;"
                + "SDMSDCAB_ListView;"
                + "SDMSDCImport_ListView;"
                + "SDMSReportPopupDC_ListView;"
                + "ImportFiles;"
                + "SDMSDCImport_DetailView;"
                + "SDMSDCImport_ListView;"
                + "UploadImage;"
                + "SDMSUploadImage_DetailView;"
                + "SDMSUploadImage_ListView;";
            //+ "SDMS;";
            Import.TargetViewId = "ImportFiles";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ((WebApplication)Application).ObjectSpaceCreated += SpreadsheetEntryController_ObjectSpaceCreated;
                AuditTrailService.Instance.SaveAuditTrailData += Instance_SaveAuditTrailData;
                constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                os = Application.CreateObjectSpace();
                os.Reloaded += Os_Reloaded;
                SelectedData sproc = ((XPObjectSpace)(os)).Session.ExecuteSproc("getCurrentLanguage", "");
                CurrentLanguage = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                rm = new ResourceManager("Resources.SDMS", Assembly.Load("App_GlobalResources"));
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;

                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser != null && View != null)
                {

                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.SDMSIsCreate = false;
                        objPermissionInfo.SDMSIsWrite = false;
                        objPermissionInfo.SDMSIsDelete = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.SDMSIsCreate = true;
                            objPermissionInfo.SDMSIsWrite = true;
                            objPermissionInfo.SDMSIsDelete = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Spreadsheet" && i.Create == true) != null)
                                {
                                    objPermissionInfo.SDMSIsCreate = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Spreadsheet" && i.Write == true) != null)
                                {
                                    objPermissionInfo.SDMSIsWrite = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Spreadsheet" && i.Delete == true) != null)
                                {
                                    objPermissionInfo.SDMSIsDelete = true;
                                    //return;
                                }
                            }
                        }
                    }
                }

                if (View.Id == "SDMSDCSpreadsheet_DetailView")
                {
                    ISfullscreen = true;
                    ASPxSpreadsheetPropertyEditor Spreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                    if (Spreadsheet != null)
                    {
                        Spreadsheet.ControlCreated += Spreadsheet_ControlCreated;
                    }
                    if (diSSColumnsToExportColumns.Count == 0 && diIndexToColumn.Count == 0)
                    {
                        intializeExportIndexColumns();
                    }
                }
                else if (View.Id == "SDMSDCImport_DetailView")
                {
                    FilePropertyEditor = ((DetailView)View).FindItem("Import") as FileDataPropertyEditor;
                    if (FilePropertyEditor != null)
                    {
                        FilePropertyEditor.ControlCreated += FilePropertyEditor_ControlCreated;
                    }
                }
                else if (View.Id == "UploadImage")
                {
                    Application.DetailViewCreating += Application_DetailViewCreating;
                }
                else if (View.Id == "SDMSUploadImage_DetailView")
                {
                    FilePropertyEditor = ((DetailView)View).FindItem("Image") as FileDataPropertyEditor;
                    if (FilePropertyEditor != null)
                    {
                        FilePropertyEditor.ControlCreated += FilePropertyEditor_ControlCreated;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Application_DetailViewCreating(object sender, DetailViewCreatingEventArgs e)
        {
            try
            {
                Application.DetailViewCreating -= Application_DetailViewCreating;
                SDMSUploadImage image = os.CreateObject<SDMSUploadImage>();
                e.View = Application.CreateDetailView(os, image);
                e.View.ViewEditMode = ViewEditMode.Edit;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FilePropertyEditor_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                FileDataEdit FileControl = ((FileDataPropertyEditor)sender).Editor;
                if (FileControl != null)
                {
                    FileControl.UploadControlCreated += FileControl_UploadControlCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FileControl_UploadControlCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxUploadControl FileUploadControl = ((FileDataEdit)sender).UploadControl;
                FileUploadControl.FileUploadComplete += FileUploadControl_FileUploadComplete;
                if (View.Id == "SDMSDCImport_DetailView")
                {
                    FileUploadControl.ValidationSettings.AllowedFileExtensions = new String[] { ".xlsx", ".xls", ".txt" };
                }
                else if (View.Id == "SDMSUploadImage_DetailView")
                {
                    FileUploadControl.ValidationSettings.AllowedFileExtensions = new String[] { ".img", ".png" };
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FileUploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                if (e.UploadedFile.ContentLength > 0)
                {
                    if (View.Id == "SDMSDCImport_DetailView")
                    {
                        SDMSDCImport objimport = new SDMSDCImport();
                        objimport.FileName = e.UploadedFile.FileName;
                        objimport.Data = e.UploadedFile.FileBytes;
                        DashboardViewItem SDMSImport = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("SDMSImportList") as DashboardViewItem;
                        if (SDMSImport != null && SDMSImport.InnerView != null)
                        {
                            if (!((ListView)SDMSImport.InnerView).IsControlCreated)
                            {
                                ((ListView)SDMSImport.InnerView).CreateControls();
                            }
                            if (((ListView)SDMSImport.InnerView).CollectionSource.List.Cast<SDMSDCImport>().ToList().Where(a => a.FileName == objimport.FileName).Count() == 0)
                            {
                                ((ListView)SDMSImport.InnerView).CollectionSource.Add(objimport);
                                ((ListView)SDMSImport.InnerView).Refresh();
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Duplicatefile"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        ((SDMSDCImport)((DetailView)View).CurrentObject).Import = new NPFileData();
                    }
                    else if (View.Id == "SDMSUploadImage_DetailView")
                    {
                        DashboardViewItem SDMSImage = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("SDMSUploadImageList") as DashboardViewItem;
                        if (SDMSImage != null && SDMSImage.InnerView != null)
                        {
                            if (!((ListView)SDMSImage.InnerView).IsControlCreated)
                            {
                                ((ListView)SDMSImage.InnerView).CreateControls();
                            }
                            if (((ListView)SDMSImage.InnerView).CollectionSource.List.Cast<SDMSUploadImage>().ToList().Where(a => a.FileName == e.UploadedFile.FileName).Count() == 0)
                            {
                                SDMSUploadImage objimage = SDMSImage.InnerView.ObjectSpace.CreateObject<SDMSUploadImage>();
                                objimage.FileName = e.UploadedFile.FileName;
                                objimage.Data = e.UploadedFile.FileBytes;
                                objimage.ABID = SDMSImage.InnerView.ObjectSpace.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID] = ?", qcbatchinfo.strAB));
                                SDMSImage.InnerView.ObjectSpace.CommitChanges();
                                ((ListView)SDMSImage.InnerView).Refresh();
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Duplicatefile"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        ((SDMSUploadImage)((DetailView)View).CurrentObject).Image = new NPFileData();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Import_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            DashboardViewItem SDMSImport = ((DashboardView)View).FindItem("SDMSImportList") as DashboardViewItem;
            IStransfered = false;
            if (qcbatchinfo.strDataTransfer != null && SDMSImport != null && SDMSImport.InnerView != null && ((ListView)SDMSImport.InnerView).CollectionSource.List.Count > 0)
            {
                InstrumentImportFromExcel(((ListView)SDMSImport.InnerView).CollectionSource);
                if (IStransfered)
                {
                    Frame.GetController<DialogController>().AcceptAction.Active.SetItemValue("enb", true);
                    Frame.GetController<DialogController>().AcceptAction.DoExecute();
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "datafailed"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            else
            {
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "datanotgiven"), InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Os_Reloaded(object sender, EventArgs e)
        {
            try
            {
                ((XPObjectSpace)sender).Session.LockingOption = DevExpress.Xpo.LockingOption.None;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SpreadsheetEntryController_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        {
            try
            {
                if (e.ObjectSpace is XPObjectSpace)
                {
                    ((XPObjectSpace)e.ObjectSpace).Session.LockingOption = DevExpress.Xpo.LockingOption.None;
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
                e.PopupControl.CustomizePopupControl += PopupControl_CustomizePopupControl;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupControl_CustomizePopupControl(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupControlEventArgs e)
        {
            try
            {
                e.PopupControl.AllowResize = false;
                e.PopupControl.ShowMaximizeButton = false;
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
                if (e.PopupFrame.View.Id == "SDMSRollback_DetailView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(800);
                    e.Height = new System.Web.UI.WebControls.Unit(400);
                }
                else if (e.PopupFrame.View.Id == "SDMSReportPopupDC_ListView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(500);
                    e.Height = new System.Web.UI.WebControls.Unit(400);
                }
                else if (e.PopupFrame.View.Id == "ImportFiles")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(500);
                    e.Height = new System.Web.UI.WebControls.Unit(500);
                }
                else if (e.PopupFrame.View.Id == "UploadImage")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(500);
                    e.Height = new System.Web.UI.WebControls.Unit(500);
                }
                else
                {
                    e.Width = new System.Web.UI.WebControls.Unit(1050);
                    e.Height = new System.Web.UI.WebControls.Unit(630);
                }
                e.Handled = true;
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
                spreadsheet.Callback += Spreadsheet_Callback;
                spreadsheet.Load += Spreadsheet_Load;
                sheet = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                sheet.CallbackManager.RegisterHandler("clrbtnvalidation", this);
                spreadsheet.RibbonMode = SpreadsheetRibbonMode.Ribbon;
                spreadsheet.RibbonTabs.Add("SDMS");
                spreadsheet.RibbonTabs.FindByName("SDMS").Index = 0;
                //spreadsheet.RibbonTabs.FindByName("SDMS").ActiveTabStyle.Height = new System.Web.UI.WebControls.Unit(100);
                spreadsheet.ActiveTabIndex = 0;
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.Add("group0");
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Text = "  ";
                var Itemmode = new RibbonComboBoxItem() { Name = "Mode", Text = rm.GetString("Mode_" + CurrentLanguage), };
                Itemmode.Items.Add(rm.GetString("View_" + CurrentLanguage), "View");
                Itemmode.Items.Add(rm.GetString("Enter_" + CurrentLanguage), "Enter");
                DefaultSetting defaultSetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Spreadsheet'"));
                if (defaultSetting != null)
                {
                    if (defaultSetting.Review == EnumRELevelSetup.Yes)
                    {
                        Itemmode.Items.Add(rm.GetString("Review_" + CurrentLanguage), "Review");
                    }
                    if (defaultSetting.Verify == EnumRELevelSetup.Yes)
                    {
                        Itemmode.Items.Add(rm.GetString("Verify_" + CurrentLanguage), "Verify");
                    }
                }
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.Add(Itemmode);
                var ItemTest = new RibbonComboBoxItem() { Name = "Test", Text = rm.GetString("Test_" + CurrentLanguage) };
                ItemTest.PropertiesComboBox.Columns.Add("Test", rm.GetString("Test_" + CurrentLanguage));
                ItemTest.PropertiesComboBox.Columns.Add("Matrix", rm.GetString("Matrix_" + CurrentLanguage));
                ItemTest.PropertiesComboBox.Columns.Add("Method", rm.GetString("Method_" + CurrentLanguage));
                ItemTest.PropertiesComboBox.Columns.Add("TemplateName", rm.GetString("TemplateName_" + CurrentLanguage));
                ItemTest.PropertiesComboBox.Columns.Add("Sx");
                ItemTest.PropertiesComboBox.Columns.Add("TestOid");
                ItemTest.PropertiesComboBox.Columns[5].ClientVisible = false;
                ItemTest.PropertiesComboBox.Columns[2].Width = 350;
                ItemTest.PropertiesComboBox.ItemStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                ItemTest.PropertiesComboBox.ValueField = "Test";
                ItemTest.PropertiesComboBox.TextFormatString = "{0}";
                ItemTest.PropertiesComboBox.Columns.Add("TemplateID");
                ItemTest.PropertiesComboBox.Columns[6].Visible = false;
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.Add(ItemTest);
                ItemTest.PropertiesComboBox.ClientSideEvents.SelectedIndexChanged = @"function(s, e) { RaiseXafCallback(globalCallbackControl, 'itemname', 'Test|' + s.GetValue() + ';' + s.GetItem(s.GetSelectedIndex()).GetColumnText('TestOid'), '', false); }";
                //var ItemAnalyticalBatch = new RibbonComboBoxItem() { Name = "AnalyticalBatch", Text = rm.GetString("AnalyticalBatch_" + CurrentLanguage) };
                var ItemAnalyticalBatch = new RibbonComboBoxItem() { Name = "AnalyticalBatch", Text = rm.GetString("QCBatchID_" + CurrentLanguage) };
                ItemAnalyticalBatch.PropertiesComboBox.Columns.Add("JobID", rm.GetString("JobID_" + CurrentLanguage));
                ItemAnalyticalBatch.PropertiesComboBox.Columns.Add("Test", rm.GetString("Test_" + CurrentLanguage));
                ItemAnalyticalBatch.PropertiesComboBox.Columns.Add("Matrix", rm.GetString("Matrix_" + CurrentLanguage));
                ItemAnalyticalBatch.PropertiesComboBox.Columns.Add("Method", rm.GetString("Method_" + CurrentLanguage));
                ItemAnalyticalBatch.PropertiesComboBox.Columns.Add("Template", rm.GetString("Template_" + CurrentLanguage));
                //ItemAnalyticalBatch.PropertiesComboBox.Columns.Add("ProjectID");
                //ItemAnalyticalBatch.PropertiesComboBox.Columns.Add("ProjectName");
                //ItemAnalyticalBatch.PropertiesComboBox.Columns.Add("Client");
                //ItemAnalyticalBatch.PropertiesComboBox.Columns.Add("ABID");
                ItemAnalyticalBatch.PropertiesComboBox.Columns.Add("ABID", rm.GetString("QCBatchID_" + CurrentLanguage));
                ItemAnalyticalBatch.PropertiesComboBox.Columns.Add("Status", rm.GetString("Status_" + CurrentLanguage));
                ItemAnalyticalBatch.PropertiesComboBox.ValueField = "ABID";
                ItemAnalyticalBatch.PropertiesComboBox.TextFormatString = "{5}";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.Add(ItemAnalyticalBatch);
                //var ItemCalibrationID = new RibbonComboBoxItem() { Name = "CalibrationID", Text = rm.GetString("CalibrationID_" + CurrentLanguage) };
                //ItemCalibrationID.PropertiesComboBox.Columns.Add("CalibrationID", rm.GetString("CalibrationID_" + CurrentLanguage));
                //ItemCalibrationID.PropertiesComboBox.Columns.Add("CalibratedDate", rm.GetString("CalibratedDate_" + CurrentLanguage));
                //ItemCalibrationID.PropertiesComboBox.Columns.Add("CalibratedBy", rm.GetString("CalibratedBy_" + CurrentLanguage));
                //ItemCalibrationID.PropertiesComboBox.Columns.Add("ISABIDLink", rm.GetString("ISABIDLink_" + CurrentLanguage));
                //ItemCalibrationID.PropertiesComboBox.ValueField = "CalibrationID";
                //ItemCalibrationID.PropertiesComboBox.TextFormatString = "{0}";
                //spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.Add(ItemCalibrationID);
                //JobID
                var ItemJobID = new RibbonComboBoxItem() { Name = "JobID", Text = rm.GetString("JobID_" + CurrentLanguage) };
                //ItemJobID.PropertiesComboBox.Columns.Add("JobID", rm.GetString("JobID_" + CurrentLanguage));
                //ItemJobID.PropertiesComboBox.ValueField = "JobID";
                //ItemJobID.PropertiesComboBox.TextFormatString = "{0}";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.Add(ItemJobID);
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("JobID").Visible = false;
                //SampleID
                var ItemSampleID = new RibbonComboBoxItem() { Name = "SampleID", Text = rm.GetString("SampleID_" + CurrentLanguage) };
                //ItemSampleID.PropertiesComboBox.Columns.Add("SampleID", rm.GetString("SampleID_" + CurrentLanguage));
                //ItemSampleID.PropertiesComboBox.ValueField = "SampleID";
                //ItemSampleID.PropertiesComboBox.TextFormatString = "{0}";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.Add(ItemSampleID);

                //Nav Item
                //var ItemNav = new RibbonDataFields() { }

                //Prev
                var ItemPrevious = new RibbonButtonItem() { Name = "Previous", Text = rm.GetString("Previous_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemPrevious.LargeImage.Url = "~/Images/Previous.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.Add(ItemPrevious);

                //Next
                var ItemNext = new RibbonButtonItem() { Name = "Next", Text = rm.GetString("Next_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemNext.LargeImage.Url = "~/Images/Next.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.Add(ItemNext);

                //ItemCalibrationID.PropertiesComboBox.ClientSideEvents.SelectedIndexChanged = @"function(s, e) { RaiseXafCallback(globalCallbackControl, 'itemname', 'CalibrationID|' + s.GetItem(s.GetSelectedIndex()).GetColumnText('CalibrationID'), '', false); }";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.Add("group1");
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Text = "  ";
                var ItemRetrieve = new RibbonButtonItem() { Name = "Retrieve", Text = rm.GetString("Retrieve_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemRetrieve.LargeImage.Url = "~/Images/Find.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.Add(ItemRetrieve);
                var ItemComplete = new RibbonButtonItem() { Name = "Complete", Text = rm.GetString("Complete_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemComplete.LargeImage.Url = "~/Images/Save All.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.Add(ItemComplete);
                var ItemSave = new RibbonButtonItem() { Name = "Save", Text = rm.GetString("Save_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemSave.LargeImage.Url = "~/Images/Save.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.Add(ItemSave);
                var ItemNewCalibration = new RibbonButtonItem() { Name = "NewCalibration", Text = rm.GetString("NewCalibration_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemNewCalibration.LargeImage.Url = "~/Images/New.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.Add(ItemNewCalibration);
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.Add("group2");
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Text = "  ";
                var ItemRollBack = new RibbonButtonItem() { Name = "RollBack", Text = rm.GetString("RollBack_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemRollBack.LargeImage.IconID = "actions_resetchanges_32x32devav";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.Add(ItemRollBack);
                var ItemDelete = new RibbonButtonItem() { Name = "Delete", Text = rm.GetString("Delete_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemDelete.LargeImage.Url = "~/Images/Delete-Red.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.Add(ItemDelete);
                var ItemDataPackage = new RibbonButtonItem() { Name = "DataPackage", Text = rm.GetString("DataPackage_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemDataPackage.LargeImage.Url = "~/Images/Datapackage.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.Add(ItemDataPackage);
                var ItemClear = new RibbonButtonItem() { Name = "Clear", Text = rm.GetString("Clear_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemClear.LargeImage.Url = "~/Images/Clear.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.Add(ItemClear);
                var ItemEdit = new RibbonButtonItem() { Name = "Edit", Text = rm.GetString("Edit_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemEdit.LargeImage.Url = "~/Images/Document-Edit.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.Add(ItemEdit);
                //var ItemRefresh = new RibbonButtonItem() { Name = "Refresh", Text = "Refresh", Size = RibbonItemSize.Large };
                //ItemRefresh.LargeImage.Url = "~/Images/Refresh.png";
                //spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.Add(ItemRefresh);
                var ItemBindGrid = new RibbonButtonItem() { Name = "BindGrid", Text = rm.GetString("BindGrid_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemBindGrid.LargeImage.Url = "~/Images/dbTable-Link.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.Add(ItemBindGrid);
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.Add("group3");
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Text = "  ";
                var ItemRecalculate = new RibbonButtonItem() { Name = "Recalculate", Text = rm.GetString("Recalculate_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemRecalculate.LargeImage.Url = "~/Images/Recalculate.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.Add(ItemRecalculate);
                var ItemImportFiles = new RibbonButtonItem() { Name = "ImportFiles", Text = rm.GetString("ImportFiles_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemImportFiles.LargeImage.Url = "~/Images/dbTable-Import.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.Add(ItemImportFiles);
                var ItemUploadImage = new RibbonButtonItem() { Name = "UploadImage", Text = rm.GetString("UploadImage_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemUploadImage.LargeImage.Url = "~/Images/uploadimage.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.Add(ItemUploadImage);
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.Add("group4");
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group4").Text = "  ";
                var ItemReport = new RibbonButtonItem() { Name = "GenerateReports", Text = rm.GetString("GenerateReports_" + CurrentLanguage), Size = RibbonItemSize.Large };
                ItemReport.LargeImage.Url = "~/Images/report_32x32.png";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group4").Items.Add(ItemReport);
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.Add("group5");
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group5").Text = "  ";
                spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group5").Items.Add(new SRFullScreenCommand() { Name = "FullScreen", Text = "Full Screen" });
                var ItemFullcreen = new RibbonButtonItem() { Name = "FullScreen", Text = rm.GetString("Full Screen" + CurrentLanguage), Size = RibbonItemSize.Large };

                //File
                {
                    spreadsheet.RibbonTabs[0].Groups[5].Items[0].Text = rm.GetString("FullScreen_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[1].Text = rm.GetString("File_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[1].Groups[0].Text = rm.GetString("Common_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[1].Groups[0].Items[0].Text = rm.GetString("Open_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[1].Groups[0].Items[1].Text = rm.GetString("Save_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[1].Groups[0].Items[2].Text = rm.GetString("" + CurrentLanguage);
                    spreadsheet.RibbonTabs[1].Groups[0].Items[3].Text = rm.GetString("Open_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[1].Groups[0].Items[4].Text = rm.GetString("Save_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[1].Groups[0].Items[5].Text = rm.GetString("SaveAs_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[1].Groups[0].Items[6].Text = rm.GetString("Print_" + CurrentLanguage);
                }
                //Home
                {
                    spreadsheet.RibbonTabs[2].Text = rm.GetString("Home_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[0].Text = rm.GetString("Undo_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[0].Items[0].Text = rm.GetString("Undo_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[0].Items[1].Text = rm.GetString("Redo_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[1].Text = rm.GetString("Clipboard_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[1].Items[0].Text = rm.GetString("Cut_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[1].Items[1].Text = rm.GetString("Copy_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[1].Items[2].Text = rm.GetString("Paste_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Text = rm.GetString("Font_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Items[0].Text = rm.GetString("FontName_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Items[1].Text = rm.GetString("FontSize_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Items[2].Text = rm.GetString("IncreaseFontSize_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Items[3].Text = rm.GetString("DecreaseFontSize_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Items[4].Text = rm.GetString("FontBold_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Items[5].Text = rm.GetString("FontItalic_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Items[6].Text = rm.GetString("FontUnderline_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Items[7].Text = rm.GetString("FontStrikeOut_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Items[8].Text = rm.GetString("Borders_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Items[9].Text = rm.GetString("FillColor_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Items[10].Text = rm.GetString("FontColor_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[2].Items[11].Text = rm.GetString("BorderLineColor_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[3].Text = rm.GetString("Alignment_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[3].Items[0].Text = rm.GetString("AlignmentTop_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[3].Items[1].Text = rm.GetString("AlignmentMiddle_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[3].Items[2].Text = rm.GetString("AlignmentBottom_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[3].Items[3].Text = rm.GetString("AlignmentLeft_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[3].Items[4].Text = rm.GetString("AlignmentCenter_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[3].Items[5].Text = rm.GetString("AlignmentRight_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[3].Items[6].Text = rm.GetString("DecreaseIndent_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[3].Items[7].Text = rm.GetString("IncreaseIndent_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[3].Items[8].Text = rm.GetString("Wrap Text_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[3].Items[9].Text = rm.GetString("Merge Cells_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[4].Text = rm.GetString("Number_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[4].Items[0].Text = rm.GetString("Accounting_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[4].Items[1].Text = rm.GetString("Percent_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[4].Items[2].Text = rm.GetString("CommaStyle_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[4].Items[3].Text = rm.GetString("IncreaseDecimal_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[4].Items[4].Text = rm.GetString("DecreaseDecimal_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[5].Text = rm.GetString("Cells_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[5].Items[0].Text = rm.GetString("Insert_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[5].Items[1].Text = rm.GetString("Delete_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[5].Items[2].Text = rm.GetString("Format_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[6].Text = rm.GetString("Editing_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[6].Items[0].Text = rm.GetString("AutoSum_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[6].Items[1].Text = rm.GetString("Fill_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[6].Items[2].Text = rm.GetString("Clear_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[6].Items[3].Text = rm.GetString("SortAndFilter_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[6].Items[4].Text = rm.GetString("Find_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[7].Text = rm.GetString("Styles_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[2].Groups[7].Items[0].Text = rm.GetString("Format as Table_" + CurrentLanguage);
                }
                //Insert
                {
                    spreadsheet.RibbonTabs[3].Text = rm.GetString("Insert_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[0].Text = rm.GetString("Table_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[0].Items[0].Text = rm.GetString("PivotTable_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[0].Items[1].Text = rm.GetString("Table_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[1].Text = rm.GetString("Illustrations_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[1].Items[0].Text = rm.GetString("Picture_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[2].Text = rm.GetString("Charts_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[2].Items[0].Text = rm.GetString("Column_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[2].Items[1].Text = rm.GetString("Line_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[2].Items[2].Text = rm.GetString("Pie_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[2].Items[3].Text = rm.GetString("Bar_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[2].Items[4].Text = rm.GetString("Area_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[2].Items[5].Text = rm.GetString("Scatter_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[2].Items[6].Text = rm.GetString("Other Charts_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[3].Text = rm.GetString("Links_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[3].Groups[3].Items[0].Text = rm.GetString("Hyperlink_" + CurrentLanguage);
                }

                //Pagelayout
                {
                    spreadsheet.RibbonTabs[4].Text = rm.GetString("PageLayout_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[4].Groups[0].Text = rm.GetString("PageSetup_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[4].Groups[0].Items[0].Text = rm.GetString("Margins_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[4].Groups[0].Items[1].Text = rm.GetString("Orientation_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[4].Groups[0].Items[2].Text = rm.GetString("Size_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[4].Groups[1].Text = rm.GetString("Print_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[4].Groups[1].Items[0].Text = rm.GetString("Gridlines_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[4].Groups[1].Items[1].Text = rm.GetString("Headings_" + CurrentLanguage);
                }
                //Formulas
                {
                    spreadsheet.RibbonTabs[5].Text = rm.GetString("Formulas_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[0].Text = rm.GetString("FunctionLibrary_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[0].Items[0].Text = rm.GetString("AutoSum_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[0].Items[1].Text = rm.GetString("Financial_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[0].Items[2].Text = rm.GetString("Logical_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[0].Items[3].Text = rm.GetString("Text_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[0].Items[4].Text = rm.GetString("DateAndTime_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[0].Items[5].Text = rm.GetString("LookupAndReference_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[0].Items[6].Text = rm.GetString("MathAndTrigonometry_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[0].Items[7].Text = rm.GetString("More_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[1].Text = rm.GetString("Calculation_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[1].Items[0].Text = rm.GetString("Calculation Options_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[1].Items[1].Text = rm.GetString("Calculate Now_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[5].Groups[1].Items[2].Text = rm.GetString("Calculate Sheet_" + CurrentLanguage);
                }
                //Data
                {
                    spreadsheet.RibbonTabs[6].Text = rm.GetString("Data_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[6].Groups[0].Text = rm.GetString("SortAndFilter_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[6].Groups[0].Items[0].Text = rm.GetString("Sort A to Z_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[6].Groups[0].Items[1].Text = rm.GetString("Sort Z to A_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[6].Groups[0].Items[2].Text = rm.GetString("Filter_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[6].Groups[0].Items[3].Text = rm.GetString("Clear_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[6].Groups[0].Items[4].Text = rm.GetString("ReApply_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[6].Groups[1].Text = rm.GetString("Tools_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[6].Groups[1].Items[0].Text = rm.GetString("Data Validation_" + CurrentLanguage);
                }
                //Review
                {
                    spreadsheet.RibbonTabs[7].Text = rm.GetString("Review_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[7].Groups[0].Text = rm.GetString("SortAndFilter_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[7].Groups[0].Items[0].Text = rm.GetString("New Comment_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[7].Groups[0].Items[1].Text = rm.GetString("EditComment_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[7].Groups[0].Items[2].Text = rm.GetString("Delete_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[7].Groups[0].Items[3].Text = rm.GetString("ShowHideComment_" + CurrentLanguage);
                }
                //View
                {
                    spreadsheet.RibbonTabs[8].Text = rm.GetString("View_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[8].Groups[0].Text = rm.GetString("DocumentViews_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[8].Groups[0].Items[0].Text = rm.GetString("Editing_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[8].Groups[0].Items[1].Text = rm.GetString("ReadingView_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[8].Groups[1].Text = rm.GetString("Show_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[8].Groups[1].Items[0].Text = rm.GetString("Gridlines_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[8].Groups[1].Items[1].Text = rm.GetString("Headings_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[8].Groups[2].Text = rm.GetString("View_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[8].Groups[2].Items[0].Text = rm.GetString("FullScreen_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[8].Groups[3].Text = rm.GetString("Window_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[8].Groups[3].Items[0].Text = rm.GetString("FreezePanes_" + CurrentLanguage);
                }
                //ReadingView
                {
                    spreadsheet.RibbonTabs[9].Text = rm.GetString("ReadingView_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[9].Groups[0].Text = rm.GetString("ReadingView_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[9].Groups[0].Items[0].Text = rm.GetString("" + CurrentLanguage);
                    spreadsheet.RibbonTabs[9].Groups[0].Items[1].Text = rm.GetString("ViewToggleEditingView_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[9].Groups[0].Items[2].Text = rm.GetString("FilePrint_" + CurrentLanguage);
                    spreadsheet.RibbonTabs[9].Groups[0].Items[3].Text = rm.GetString("EditingFindAndSelect_" + CurrentLanguage);
                }

                ItemAnalyticalBatch.Value = null;
                IObjectSpace obs = Application.CreateObjectSpace();
                if (qcbatchinfo.strMode == null || qcbatchinfo.canfilter)
                {
                    DataTable table = CreateTDT();
                    DataTable ABtable = CreateABDT();

                    IList<TestMethod> tests = obs.GetObjects<TestMethod>(CriteriaOperator.Parse(""));
                    bool isAdministrator = false;
                    List<Guid> lstTestMethodOid = new List<Guid>();

                    CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        isAdministrator = true;
                    }
                    else
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = obs.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] =True", currentUser.Oid));
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
                    foreach (TestMethod test in tests.OrderBy(a => a.TestName).ToList())
                    {
                        if (isAdministrator || !isAdministrator && lstTestMethodOid.Contains(test.Oid))
                        {
                            SpreadSheetBuilder_TestParameter objID = obs.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID]=?", test.Oid));
                            if (objID != null)
                            {
                                //IList<SampleParameter> objsp = obs.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [Samplelogin] Is Not Null And ([QCBatchID] Is Null or [UQABID.Status] = 1)", test.Oid));

                                //celin changes
                                //IList<SampleParameter> objsp = obs.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ?  And [QCBatchID] Is Null And IsNullOrEmpty([ResultNumeric]) And [Samplelogin.IsNotTransferred] = false", test.Oid));
                                ////

                                //IList<SampleLogIn> objdistsl = objsp.Select(s => s.Samplelogin).Distinct().ToList();
                                //if (objdistsl.Count > 0)
                                //{

                                //    SpreadSheetBuilder_TemplateInfo objName = obs.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID]=?", objID.TemplateID));
                                //    if (objName != null)
                                //    {
                                //        table.Rows.Add(new object[] { test.TestName, test.MatrixName.MatrixName, test.MethodName.MethodNumber, objName.TemplateName, objdistsl.Count, test.Oid });
                                //    }
                                //}
                                #region jayakumarChanges
                                IList<SpreadSheetBuilder_TestParameter> lstObjID = os.GetObjects<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID]=?", test.Oid));
                                if (lstObjID != null)
                                {
                                    foreach (SpreadSheetBuilder_TestParameter objTP in lstObjID)
                                    {
                                        SpreadSheetBuilder_TemplateInfo objName = os.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID]=?", objTP.TemplateID));
                                        if (objName != null)
                                        {
                                            if (table.Select("TemplateID = '" + objName.TemplateID.ToString() + "' ").Count() > 0)
                                            {
                                            }
                                            else
                                            {
                                                table.Rows.Add(new object[] { test.TestName, test.MatrixName.MatrixName, test.MethodName.MethodNumber, objName.TemplateName, lstObjID.Count, test.Oid, objName.TemplateID });
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    ItemTest.PropertiesComboBox.DataSource = table; //qcbatchinfo.dtTestdatasource =
                    if (qcbatchinfo.qcstatus == 0 || qcbatchinfo.qcstatus == 1)
                    {
                        Itemmode.Value = qcbatchinfo.strMode = "Enter";
                        if (qcbatchinfo.strTest != null && qcbatchinfo.OidTestMethod != null)
                        {
                            ItemTest.Value = qcbatchinfo.strTest;
                            SelectedData result = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SpreadSheetEntry_FillAnalyticalBatchID_SP", new SprocParameter("@Mode", qcbatchinfo.strMode), new SprocParameter("@TestMethodID", qcbatchinfo.OidTestMethod));
                            foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                            {
                                ABtable.Rows.Add(new object[] { (string)row.Values[4], (string)row.Values[5], (string)row.Values[6], (string)row.Values[7], (string)row.Values[8], (string)row.Values[9], null });//null, null, null,
                            }
                            ItemAnalyticalBatch.PropertiesComboBox.Columns[6].Visible = false;
                        }
                    }
                    else if (qcbatchinfo.qcstatus > 1)
                    {
                        DefaultSetting setting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse(""));
                        if (setting != null)
                        {
                            if ((setting.Review == EnumRELevelSetup.No && setting.Verify == EnumRELevelSetup.No) || qcbatchinfo.qcstatus == 4)
                            {
                                Itemmode.Value = qcbatchinfo.strMode = "View";
                                ItemTest.Value = qcbatchinfo.strTest;
                                SelectedData result = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SpreadSheetEntry_FillAnalyticalBatchID_SP", new SprocParameter("@Mode", qcbatchinfo.strMode));
                                foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                                {
                                    ABtable.Rows.Add(new object[] { (string)row.Values[4], (string)row.Values[5], (string)row.Values[6], (string)row.Values[7], (string)row.Values[8], (string)row.Values[9], (string)row.Values[10] });
                                }
                                ItemAnalyticalBatch.PropertiesComboBox.Columns[6].Visible = true;
                            }
                            else if (setting.Review == EnumRELevelSetup.Yes && qcbatchinfo.qcstatus == 2)
                            {
                                Itemmode.Value = qcbatchinfo.strMode = "Review";
                                SelectedData result = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SpreadSheetEntry_FillAnalyticalBatchID_SP", new SprocParameter("@Mode", qcbatchinfo.strMode));
                                foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                                {
                                    ABtable.Rows.Add(new object[] { (string)row.Values[4], (string)row.Values[5], (string)row.Values[6], (string)row.Values[7], (string)row.Values[8], (string)row.Values[9], null });
                                }
                                ItemAnalyticalBatch.PropertiesComboBox.Columns[6].Visible = false;
                            }
                            else if (setting.Verify == EnumRELevelSetup.Yes && qcbatchinfo.qcstatus == 3)
                            {
                                Itemmode.Value = qcbatchinfo.strMode = "Verify";
                                SelectedData result = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SpreadSheetEntry_FillAnalyticalBatchID_SP", new SprocParameter("@Mode", qcbatchinfo.strMode));
                                foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                                {
                                    ABtable.Rows.Add(new object[] { (string)row.Values[4], (string)row.Values[5], (string)row.Values[6], (string)row.Values[7], (string)row.Values[8], (string)row.Values[9], null });
                                }
                                ItemAnalyticalBatch.PropertiesComboBox.Columns[6].Visible = false;
                            }
                        }
                    }
                    modechange(spreadsheet, qcbatchinfo.strMode);
                    ItemAnalyticalBatch.PropertiesComboBox.DataSource = ABtable;
                    ItemAnalyticalBatch.Value = qcbatchinfo.strAB;
                    spreadsheet.FullscreenMode = ISfullscreen = true;
                    if (objPermissionInfo.SDMSIsWrite)
                    {
                        spreadsheet.JSProperties["cpisedit"] = false;
                    }
                    else
                    {
                        spreadsheet.JSProperties["cpisedit"] = true;
                    }
                }
                if (IsRefresh)
                {
                    fillcombo(spreadsheet);
                    IsRefresh = false;
                }
                spreadsheet.JSProperties["cpMode"] = qcbatchinfo.strMode;
                ICallbackManagerHolder holder = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                holder.CallbackManager.RegisterHandler("itemname", this);
                string customCommandClickHandler = @"function(s, e) 
                { 
                if(e.commandName != 'AnalyticalBatch')
                {
                    if(e.commandName == 'Mode')
                    {
                         RaiseXafCallback(globalCallbackControl, 'itemname', e.commandName + '|' + s.isInFullScreenMode + '|' + e.item.GetValue(), '', false);  
                    }
                    else if (e.commandName == 'SampleID')
                    {
                        RaiseXafCallback(globalCallbackControl, 'itemname', e.commandName + '|' + s.isInFullScreenMode + '|' + e.item.GetValue(), '', false);  
                    }
                    else if (e.commandName == 'JobID')
                    {
                        RaiseXafCallback(globalCallbackControl, 'itemname', e.commandName + '|' + s.isInFullScreenMode + '|' + e.item.GetValue(), '', false);  
                    }
                    else
                    {
                        if(e.commandName != 'Test')
                        {
                            RaiseXafCallback(globalCallbackControl, 'itemname', e.commandName + '|' + s.isInFullScreenMode, '', false);  
                        }
                        else
                        {
                            s.PerformCallback(e.commandName + '|' + s.isInFullScreenMode);
                        }
                    }
                }
                else
                {
                    s.PerformCallback(e.commandName + '|' + s.isInFullScreenMode + '|' + e.item.GetValue()); 
                }
                }";
                string custominit = @"function(s,e)
                {
                    comboresize();                                        
                    if(s.cpMode != null && s.GetRibbon().tabs[0].groups[0].items[0].GetValue() == null)
                    {
                        RaiseXafCallback(globalCallbackControl, 'itemname', 'Reload|' + s.isInFullScreenMode, '', false);  
                    }
                }";
                ClientSideEventsHelper.AssignClientHandlerSafe(spreadsheet, "CustomCommandExecuted", customCommandClickHandler, "Spreadsheettemplate1");
                ClientSideEventsHelper.AssignClientHandlerSafe(spreadsheet, "Init", custominit, "Spreadsheettemplate2");
                ClientSideEventsHelper.AssignClientHandlerSafe(spreadsheet, "CellBeginEdit", "function(s,e) { if(s.cpisedit != null) { e.cancel = s.cpisedit; } }", "Spreadsheettemplate3");
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
                spreadsheet.FullscreenMode = ISfullscreen;
                spreadsheet.Document.ActiveSheetChanging += Document_ActiveSheetChanging;
                if (qcbatchinfo.canfilter)
                {
                    qcbatchinfo.canfilter = false;
                    if (qcbatchinfo.qcstatus == 0)
                    {
                        if (qcbatchinfo.IsMoldTest)
                        {
                            GenerateMailMergeMold(spreadsheet);
                            //Assign datasource for Mold Combo Boxes
                            //fillComboMold(spreadsheet);
                            //((RibbonButtonItem)spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).ClientEnabled = false;
                            //if(spreadsheet.Document.Worksheets.Count == 2)
                            //{
                            //    ((RibbonButtonItem)spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = false;
                            //}
                            //else
                            //{
                            //    ((RibbonButtonItem)spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = true;
                            //}
                        }
                        else
                        {
                            GenerateMailMerge(spreadsheet);
                        }
                    }
                    else if (qcbatchinfo.qcstatus != 0 && qcbatchinfo.strAB != null)
                    {
                        SpreadSheetEntry_AnalyticalBatch batch = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                        if (batch != null)
                        {
                            LoadfromAB(spreadsheet, batch);
                            ABGridrefresh();
                            CalibRefresh();
                            if (qcbatchinfo.IsMoldTest)
                            {
                                GenerateMailMergeMold(spreadsheet);
                                //Assign datasource for Mold Combo Boxes
                                fillComboMold(spreadsheet);
                                //((RibbonButtonItem)spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).ClientEnabled = false;
                                //if (spreadsheet.Document.Worksheets.Count == 2)
                                //{
                                //    ((RibbonButtonItem)spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = false;
                                //}
                                //else
                                //{
                                //    ((RibbonButtonItem)spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = true;
                                //}
                            }
                        }
                    }
                    if (qcbatchinfo.strMode == "Enter")
                    {
                        if (objPermissionInfo.SDMSIsWrite)
                        {
                            spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("ImportFiles").ClientEnabled = true;
                        }
                        else
                        {
                            spreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("ImportFiles").ClientEnabled = false;
                        }
                    }
                    MultipleSheetSupport(spreadsheet);
                    AddCustomFunction(spreadsheet);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Document_ActiveSheetChanging(object sender, ActiveSheetChangingEventArgs e)
        {
            try
            {
                //e.Cancel = true;
                ASPxSpreadsheetPropertyEditor aSPxSpreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                if (aSPxSpreadsheet != null)
                {
                    string strActiveSheetName = e.NewActiveSheetName;
                    ((RibbonComboBoxItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Value = strActiveSheetName;
                    int itemIndex = ((RibbonComboBoxItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Items.IndexOfText(strActiveSheetName);
                    //if (itemIndex == 0)
                    //{
                    //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).ClientEnabled = false;
                    //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = true;
                    //}
                    //else if (itemIndex == ((RibbonComboBoxItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Items.Count - 1)
                    //{
                    //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).ClientEnabled = true;
                    //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = false;
                    //}
                    //else
                    //{
                    //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).ClientEnabled = true;
                    //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = true;
                    //}
                    //fillcombo(aSPxSpreadsheet.ASPxSpreadsheetControl);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AddCustomFunction(ASPxSpreadsheet spreadsheet)
        {
            try
            {
                SignificantNotation sigFunction = new SignificantNotation();
                DevExpress.Spreadsheet.IWorkbook objworkbook = spreadsheet.Document;
                if (objworkbook != null && !objworkbook.Functions.GlobalCustomFunctions.Contains(sigFunction.Name))
                {
                    objworkbook.Functions.GlobalCustomFunctions.Add(sigFunction);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void GenerateMailMerge(ASPxSpreadsheet spreadsheet, bool IStransfered = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(qcbatchinfo.strTestMethodMatrixName) && !string.IsNullOrEmpty(qcbatchinfo.strTestMethodTestName) && !string.IsNullOrEmpty(qcbatchinfo.strTestMethodMethodNumber) && (objABinfo.dtQCdatatable != null))
                {
                    SpreadSheetBuilder_TestParameter testParameter = null;
                    TestMethod objtestmethod = os.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [MethodName.Oid] = ?", qcbatchinfo.strTestMethodMatrixName, qcbatchinfo.strTestMethodTestName, qcbatchinfo.strTestMethodMethodNumberOid));
                    if (objtestmethod != null)
                    {
                        testParameter = os.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID]=?", objtestmethod.Oid));

                    }
                    if (testParameter != null)
                    {
                        SpreadSheetBuilder_TemplateInfo templateInfo = os.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID]=?", testParameter.TemplateID));
                        if (templateInfo != null)
                        {
                            qcbatchinfo.templateid = testParameter.TemplateID;
                            spreadsheet.Document.CreateNewDocument();
                            IWorkbook objworkbook = spreadsheet.Document;
                            objworkbook.LoadDocument(templateInfo.SpreadSheet.ToArray(), DocumentFormat.OpenXml);
                            //objworkbook.Worksheets[0].Cells.FillColor = Color.Transparent;
                            if (!IStransfered)
                            {
                                qcbatchinfo.dtsample = objABinfo.dtQCdatatable;
                                //Getdtsamplerunnosource(qcbatchinfo.strqcid);
                                //Getdtsamplerunnosource(qcbatchinfo.QCBatchOid);
                                foreach (DataColumn column in qcbatchinfo.dtsample.Columns)
                                {
                                    column.ColumnName = column.ColumnName.ToUpper();
                                }
                            }
                            qcbatchinfo.dtCalibration = new DataTable { TableName = "CalibrationTableDataSource" };
                            qcbatchinfo.dtDataParsing = new DataTable();
                            Getdtcalibrationsource(testParameter.TemplateID, qcbatchinfo.OidTestMethod);
                            Getdtparsingsamplefields(testParameter.TemplateID);
                            drImportDatasource = qcbatchinfo.dtselectedsamplefields.Select("ImportDataSource is Not Null and ImportField is not null and len(ImportField) > 0 ");
                            if (drImportDatasource != null && drImportDatasource.Length > 0)
                            {
                                ImportDatasource(testParameter.TemplateID);
                            }

                            DataRow[] drrSampleSingle = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable'");
                            DataRow[] drrCalibration = qcbatchinfo.dtDataParsing.Select("RunType = 'CalibrationTable'");
                            int intCalibrationSheetID = -1;
                            int intDTSheetID = -1;
                            if (drrSampleSingle.Length > 0 && drrCalibration.Length > 0)
                            {
                                if (drrSampleSingle[0]["SheetID"].ToString() != drrCalibration[0]["SheetID"].ToString())
                                {
                                    intCalibrationSheetID = Convert.ToInt16(drrCalibration[0]["SheetID"].ToString()) - 1;
                                }
                            }
                            if (!string.IsNullOrEmpty(templateInfo.DTSheetID.ToString()) && Convert.ToInt16(templateInfo.DTSheetID) > 0)
                            {
                                intDTSheetID = Convert.ToInt16(templateInfo.DTSheetID.ToString()) - 1;
                            }

                            DataSet dsSpreadSheetDataSource = new DataSet("SpreadSheetDataSource");
                            dsSpreadSheetDataSource.Tables.Add(qcbatchinfo.dtsample.Copy());
                            dsSpreadSheetDataSource.Tables.Add(qcbatchinfo.dtCalibration.Copy());
                            objworkbook.MailMergeDataSource = dsSpreadSheetDataSource;
                            objworkbook.MailMergeDataMember = "RawDataTableDataSource";

                            IWorkbook objworkbooktemp = null;
                            for (int i = 0; i < objworkbook.Worksheets.Count; i++)
                            {
                                objworkbook.Worksheets.ActiveWorksheet = objworkbook.Worksheets[i];
                                if (intCalibrationSheetID != i && intDTSheetID != i)
                                {
                                    IList<IWorkbook> resultWorkbooks = objworkbook.GenerateMailMergeDocuments();
                                    foreach (IWorkbook workbook in resultWorkbooks)
                                    {
                                        if (objworkbooktemp == null)
                                        {
                                            objworkbooktemp = workbook;
                                            if (objworkbooktemp.Worksheets[0].Name != objworkbook.Worksheets[0].Name)
                                            {
                                                //objworkbooktemp.Worksheets[0].Cells.FillColor = Color.Transparent;
                                                objworkbooktemp.Worksheets[0].Name = objworkbook.Worksheets[0].Name;
                                                objworkbooktemp.Worksheets[0].Visible = objworkbook.Worksheets[0].Visible;
                                            }
                                        }
                                        else
                                        {
                                            objworkbooktemp.Worksheets.Add(objworkbook.Worksheets[i].Name);
                                            objworkbooktemp.Worksheets[i].CopyFrom(workbook.Worksheets.ActiveWorksheet);
                                            objworkbooktemp.Worksheets[i].Visible = objworkbook.Worksheets[i].Visible;
                                        }
                                    }
                                }
                                else
                                {
                                    if (objworkbooktemp == null)
                                    {
                                        objworkbooktemp = objworkbook;
                                        if (objworkbooktemp.Worksheets[0].Name != objworkbook.Worksheets[0].Name)
                                        {
                                            objworkbooktemp.Worksheets[0].Name = objworkbook.Worksheets[0].Name;
                                        }
                                        objworkbooktemp.Worksheets[0].Visible = objworkbook.Worksheets[0].Visible;
                                    }
                                    else
                                    {
                                        objworkbooktemp.Worksheets.Add(objworkbook.Worksheets[i].Name);
                                        objworkbooktemp.Worksheets[i].CopyFrom(objworkbook.Worksheets.ActiveWorksheet);
                                        objworkbooktemp.Worksheets[i].Visible = objworkbook.Worksheets[i].Visible;
                                    }
                                }
                                var activeSheet = objworkbooktemp.Worksheets.ActiveWorksheet;
                                if (activeSheet != null && activeSheet.IsProtected == false)
                                {
                                    activeSheet["$A:$XFD"].Protection.Locked = false;
                                    IEnumerable<Cell> existingCells = activeSheet.GetExistingCells();
                                    objworkbooktemp.BeginUpdate();
                                    foreach (Cell cell in existingCells)
                                    {
                                        if (cell.HasFormula)
                                        {
                                            activeSheet[cell.GetReferenceA1()].Protection.Locked = true;
                                        }
                                    }
                                    objworkbooktemp.EndUpdate();
                                    activeSheet.Protect("password", WorksheetProtectionPermissions.SelectLockedCells);
                                }
                            }
                            objworkbooktemp.Worksheets.ActiveWorksheet = objworkbooktemp.Worksheets[0];
                            spreadsheet.Document.LoadDocument(objworkbooktemp.SaveDocument(DocumentFormat.OpenXml), DocumentFormat.OpenXml);
                            //objworkbook.Worksheets[0].Cells.FillColor = Color.Transparent;
                            qcbatchinfo.IsSheetloaded = true;
                            ABGridrefresh();
                            CalibRefresh();

                            //int intCalibrationLevel = templateInfo.CalibrationLevelNo;
                            //bool bolOrientation = templateInfo.Orientation;
                            //if (intCalibrationLevel > 0 && (intAnalyticalBatchID > 0 || (intAnalyticalBatchID == 0 && intCalibrationID > 0)))
                            //  BindCalibrationToSpreadSheetByParameter();
                        }
                    }
                    else
                    {
                        //SpreadSheetBuilder_TemplateInfo templateInfo = os.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID]=?", 7098));
                        SpreadSheetBuilder_TemplateInfo templateInfo = os.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateName]=?", strCommonTemplateName));
                        if (templateInfo != null)
                        {
                            qcbatchinfo.templateid = templateInfo.TemplateID;
                            spreadsheet.Document.CreateNewDocument();
                            DevExpress.Spreadsheet.IWorkbook objworkbook = spreadsheet.Document;
                            objworkbook.LoadDocument(templateInfo.SpreadSheet.ToArray(), DocumentFormat.OpenXml);
                            if (!IStransfered)
                            {
                                qcbatchinfo.dtsample = objABinfo.dtQCdatatable;
                                //qcbatchinfo.dtsample = new DataTable { TableName = "RawDataTableDataSource" };
                                //Getdtsamplerunnosource(qcbatchinfo.strqcid);
                                Getdtsamplerunnosource(qcbatchinfo.QCBatchOid);
                                foreach (DataColumn column in qcbatchinfo.dtsample.Columns)
                                {
                                    column.ColumnName = column.ColumnName.ToUpper();
                                }
                            }
                            qcbatchinfo.dtCalibration = new DataTable { TableName = "CalibrationTableDataSource" };
                            qcbatchinfo.dtDataParsing = new DataTable();
                            Getdtcalibrationsource(templateInfo.TemplateID, qcbatchinfo.OidTestMethod);
                            Getdtparsingsamplefields(templateInfo.TemplateID);
                            drImportDatasource = qcbatchinfo.dtselectedsamplefields.Select("ImportDataSource is Not Null and ImportField is not null and len(ImportField) > 0 ");
                            if (drImportDatasource != null && drImportDatasource.Length > 0)
                            {
                                ImportDatasource(templateInfo.TemplateID);
                            }

                            DataRow[] drrSampleSingle = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable'");
                            DataRow[] drrCalibration = qcbatchinfo.dtDataParsing.Select("RunType = 'CalibrationTable'");
                            int intCalibrationSheetID = -1;
                            int intDTSheetID = -1;
                            if (drrSampleSingle.Length > 0 && drrCalibration.Length > 0)
                            {
                                if (drrSampleSingle[0]["SheetID"].ToString() != drrCalibration[0]["SheetID"].ToString())
                                {
                                    intCalibrationSheetID = Convert.ToInt16(drrCalibration[0]["SheetID"].ToString()) - 1;
                                }
                            }
                            if (!string.IsNullOrEmpty(templateInfo.DTSheetID.ToString()) && Convert.ToInt16(templateInfo.DTSheetID) > 0)
                            {
                                intDTSheetID = Convert.ToInt16(templateInfo.DTSheetID.ToString()) - 1;
                            }

                            DataSet dsSpreadSheetDataSource = new DataSet("SpreadSheetDataSource");
                            dsSpreadSheetDataSource.Tables.Add(qcbatchinfo.dtsample.Copy());
                            dsSpreadSheetDataSource.Tables.Add(qcbatchinfo.dtCalibration.Copy());
                            objworkbook.MailMergeDataSource = dsSpreadSheetDataSource;
                            objworkbook.MailMergeDataMember = "RawDataTableDataSource";

                            DevExpress.Spreadsheet.IWorkbook objworkbooktemp = null;
                            for (int i = 0; i < objworkbook.Worksheets.Count; i++)
                            {
                                objworkbook.Worksheets.ActiveWorksheet = objworkbook.Worksheets[i];
                                if (intCalibrationSheetID != i && intDTSheetID != i)
                                {
                                    IList<DevExpress.Spreadsheet.IWorkbook> resultWorkbooks = objworkbook.GenerateMailMergeDocuments();
                                    foreach (DevExpress.Spreadsheet.IWorkbook workbook in resultWorkbooks)
                                    {
                                        if (objworkbooktemp == null)
                                        {
                                            objworkbooktemp = workbook;
                                            if (objworkbooktemp.Worksheets[0].Name != objworkbook.Worksheets[0].Name)
                                            {
                                                //objworkbooktemp.Worksheets[0].Cells.FillColor = Color.Transparent;
                                                objworkbooktemp.Worksheets[0].Name = objworkbook.Worksheets[0].Name;
                                                objworkbooktemp.Worksheets[0].Visible = objworkbook.Worksheets[0].Visible;
                                            }
                                        }
                                        else
                                        {
                                            objworkbooktemp.Worksheets.Add(objworkbook.Worksheets[i].Name);
                                            objworkbooktemp.Worksheets[i].CopyFrom(workbook.Worksheets.ActiveWorksheet);
                                            objworkbooktemp.Worksheets[i].Visible = objworkbook.Worksheets[i].Visible;
                                        }
                                    }
                                }
                                else
                                {
                                    if (objworkbooktemp == null)
                                    {
                                        objworkbooktemp = objworkbook;
                                        if (objworkbooktemp.Worksheets[0].Name != objworkbook.Worksheets[0].Name)
                                        {
                                            objworkbooktemp.Worksheets[0].Name = objworkbook.Worksheets[0].Name;
                                        }
                                        objworkbooktemp.Worksheets[0].Visible = objworkbook.Worksheets[0].Visible;
                                    }
                                    else
                                    {
                                        objworkbooktemp.Worksheets.Add(objworkbook.Worksheets[i].Name);
                                        objworkbooktemp.Worksheets[i].CopyFrom(objworkbook.Worksheets.ActiveWorksheet);
                                        objworkbooktemp.Worksheets[i].Visible = objworkbook.Worksheets[i].Visible;
                                    }
                                }
                                var activeSheet = objworkbooktemp.Worksheets.ActiveWorksheet;
                                if (activeSheet != null && activeSheet.IsProtected == false)
                                {
                                    activeSheet["$A:$XFD"].Protection.Locked = false;
                                    IEnumerable<Cell> existingCells = activeSheet.GetExistingCells();
                                    objworkbooktemp.BeginUpdate();
                                    foreach (Cell cell in existingCells)
                                    {
                                        if (cell.HasFormula)
                                        {
                                            activeSheet[cell.GetReferenceA1()].Protection.Locked = true;
                                        }
                                    }
                                    objworkbooktemp.EndUpdate();
                                    activeSheet.Protect("password", WorksheetProtectionPermissions.SelectLockedCells);
                                }
                            }
                            objworkbooktemp.Worksheets.ActiveWorksheet = objworkbooktemp.Worksheets[0];
                            spreadsheet.Document.LoadDocument(objworkbooktemp.SaveDocument(DocumentFormat.OpenXml), DocumentFormat.OpenXml);
                            qcbatchinfo.IsSheetloaded = true;
                            ABGridrefresh();
                            //int intCalibrationLevel = templateInfo.CalibrationLevelNo;
                            //bool bolOrientation = templateInfo.Orientation;
                            //if (intCalibrationLevel > 0 && (intAnalyticalBatchID > 0 || (intAnalyticalBatchID == 0 && intCalibrationID > 0)))
                            //  BindCalibrationToSpreadSheetByParameter();
                        }
                    }
                    if (View.Id == "SDMSDCSpreadsheet_DetailView")
                    {
                        ASPxSpreadsheetPropertyEditor aSPxSpreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                        if (qcbatchinfo.IsSheetloaded)
                        {
                            //BindSampleToGrid(aSPxSpreadsheet);
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

        private void MultipleSheetSupport(ASPxSpreadsheet spreadsheet)
        {
            try
            {
                if (spreadsheet != null)
                {
                    IWorkbook objIWorkbook = spreadsheet.Document;
                    string strFiler = "(RunType = 'RawDataTable' OR RunType ='CalibrationTable') AND FORMULA LIKE '%!%'";
                    //DataRow[] drMultiSheetFormula = qcbatchinfo.dtDataParsing.Select(strFiler);
                    if (qcbatchinfo.dtDataParsing != null && qcbatchinfo != null && qcbatchinfo.dtsample != null)
                    {
                        DataRow[] drMultiSheetFormula = qcbatchinfo.dtDataParsing.Select(strFiler);
                        if (drMultiSheetFormula != null && drMultiSheetFormula.Length > 0 && qcbatchinfo.dtsample != null && qcbatchinfo.dtsample.Rows.Count > 0)
                        {
                            foreach (DataRow dr in drMultiSheetFormula)
                            {
                                string strRange = Regex.Replace(dr["Position"].ToString(), @"[^A-Z]+", "");
                                string strRangeEnd = Regex.Replace(dr["Position"].ToString(), @"[^\d]", "");
                                if (dr["RunType"].ToString() == "RawDataTable")
                                {
                                    strRangeEnd = strRange + (Convert.ToInt16(strRangeEnd) + qcbatchinfo.dtsample.Rows.Count - 1).ToString();
                                }
                                else if (dr["RunType"].ToString() == "CalibrationTable")
                                {
                                    strRangeEnd = strRange + (Convert.ToInt16(strRangeEnd) + qcbatchinfo.dtCalibration.Rows.Count - 1).ToString();
                                }
                                if (Convert.ToBoolean(dr["Continuous"]))
                                    objIWorkbook.Worksheets[Convert.ToInt16(dr["SheetID"].ToString()) - 1].Range[string.Format("{0}:{1}", dr["Position"], strRangeEnd)].ExistingCells.ToList<Cell>().ForEach(x => x.Formula = x.Formula);
                                else
                                    objIWorkbook.Worksheets[Convert.ToInt16(dr["SheetID"].ToString()) - 1].Cells[dr["Position"].ToString()].Formula = objIWorkbook.Worksheets[Convert.ToInt16(dr["SheetID"].ToString()) - 1].Cells[dr["Position"].ToString()].Formula;
                            }
                        }
                        if (objIWorkbook.Worksheets.Count > 0)
                            objIWorkbook.Worksheets.ActiveWorksheet = objIWorkbook.Worksheets[0];
                        //objIWorkbook.Worksheets[0].Cells.FillColor = Color.Transparent;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Spreadsheet_Callback(object sender, CallbackEventArgsBase e)
        {
            try
            {
                string[] paramsplit = e.Parameter.Split('|');
                if (paramsplit[0] == "AnalyticalBatch")
                {
                    qcbatchinfo.strAB = paramsplit[2];
                }
                else if (paramsplit[0] == "Test")
                {
                    ISfullscreen = Convert.ToBoolean(paramsplit[1]);
                }
                else if (paramsplit[0] == "JobID")
                {

                }
                else if (paramsplit[0] == "SampleID")
                {

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
                    string[] Param = parameter.Split('|');
                    ASPxSpreadsheetPropertyEditor aSPxSpreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                    if (aSPxSpreadsheet != null)
                    {
                        ASPxSpreadsheet spreadsheet = ((ASPxSpreadsheetPropertyEditor)aSPxSpreadsheet).ASPxSpreadsheetControl;
                        if (bool.TryParse(parameter, out bool Clear))
                        {
                            if (Clear)
                            {
                                clearcontrols(aSPxSpreadsheet.ASPxSpreadsheetControl);
                            }
                            aSPxSpreadsheet.ASPxSpreadsheetControl.FullscreenMode = ISfullscreen;
                        }
                        else
                        {
                            string[] paramsplit = parameter.Split('|');
                            if (paramsplit[0] == "Test")
                            {
                                aSPxSpreadsheet.ASPxSpreadsheetControl.FullscreenMode = ISfullscreen;
                            }
                            else if (paramsplit[0] != "CalibrationID")
                            {
                                aSPxSpreadsheet.ASPxSpreadsheetControl.FullscreenMode = Convert.ToBoolean(paramsplit[1]);
                            }
                            if (paramsplit[0] == "Retrieve")
                            {
                                if (qcbatchinfo.strMode == "Enter" && qcbatchinfo.strAB == null && objPermissionInfo.SDMSIsWrite)
                                {
                                    if (qcbatchinfo.strTest != null)
                                    {
                                        qcbatchinfo.strqcid = null;
                                    }
                                    else
                                    {
                                        qcbatchinfo.strqcid = null;
                                        qcbatchinfo.OidTestMethod = null;
                                    }
                                    ShowViewParameters showViewParameters = new ShowViewParameters(Application.CreateDashboardView(Application.CreateObjectSpace(), "QCBatchsequence", true));
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
                                else if (qcbatchinfo.strMode != null && qcbatchinfo.strAB != null)
                                {
                                    SpreadSheetEntry_AnalyticalBatch batch = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                                    if (batch != null)
                                    {
                                        qcbatchinfo.strqcid = batch.AnalyticalBatchID;
                                        qcbatchinfo.QCBatchOid = batch.Oid;
                                        qcbatchinfo.strAB = batch.AnalyticalBatchID;
                                        if (batch.Calibration != null)
                                        {
                                            qcbatchinfo.strCB = batch.Calibration.CalibrationID;
                                        }
                                        if (batch.Test.TestName != null && batch.Test.TestName.StartsWith("PLM"))
                                        {
                                            qcbatchinfo.IsPLMTest = true;

                                        }
                                        else
                                        {
                                            qcbatchinfo.IsPLMTest = false;
                                        }
                                        if (batch.Test != null && batch.Test.TestName.StartsWith("Mold"))
                                        {
                                            qcbatchinfo.IsMoldTest = true;
                                        }
                                        else
                                        {
                                            qcbatchinfo.IsMoldTest = false;
                                        }
                                        //qcbatchinfo.canfilter = true;
                                        LoadfromAB(aSPxSpreadsheet.ASPxSpreadsheetControl, batch);
                                        ABGridrefresh();
                                        CalibRefresh();
                                        AddCustomFunction(spreadsheet);
                                    }
                                }
                            }
                            else if (paramsplit[0] == "Next" || paramsplit[0] == "Previous")
                            {
                                string strActiveSheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet.Name;
                                int sheetIndex = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet.Index;
                                if (paramsplit[0] == "Next")
                                {
                                    sheetIndex = sheetIndex + 1;
                                }
                                else
                                {
                                    sheetIndex = sheetIndex - 1;
                                }
                                if (sheetIndex >= 0 && sheetIndex <= aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets.Count - 2)
                                {
                                    aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets[sheetIndex];
                                    ((RibbonComboBoxItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Value = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet.Name;
                                }
                                //if (sheetIndex == 0)
                                //{
                                //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).ClientEnabled = false;
                                //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = true;
                                //}
                                //else if (sheetIndex == aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets.Count -2)
                                //{
                                //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).ClientEnabled = true;
                                //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = false;
                                //}
                                //else
                                //{
                                //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).ClientEnabled = true;
                                //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = true;
                                //}
                            }
                            else if (paramsplit[0] == "BindGrid")
                            {
                                if (qcbatchinfo.IsSheetloaded)
                                {
                                    BindSampleToGrid(aSPxSpreadsheet);
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotDataLoaded"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else if (paramsplit[0] == "Clear")
                            {
                                if (qcbatchinfo.Isedit)
                                {
                                    qcbatchinfo.Isedit = false;
                                    SpreadSheetEntry_AnalyticalBatch batch = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                                    if (batch != null)
                                    {
                                        LoadfromAB(aSPxSpreadsheet.ASPxSpreadsheetControl, batch);
                                        ABGridrefresh();
                                        CalibRefresh();
                                    }
                                }
                                else
                                {
                                    WebWindow.CurrentRequestWindow.RegisterClientScript("Openspreadsheet", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('All Unssaved changes will be cleared. Do you want to continue?'); {0}", sheet.CallbackManager.GetScript("clrbtnvalidation", "openconfirm")));
                                }
                            }
                            else if (paramsplit[0] == "Edit")
                            {
                                //if (qcbatchinfo.IsSheetloaded)
                                //{
                                //    if (qcbatchinfo.Isedit == false)
                                //    {
                                //        QCBatch qC = os.FindObject<QCBatch>(CriteriaOperator.Parse("[QCBatchID]=?", qcbatchinfo.strqcid));
                                //        if (qC != null && qC.ABID.Status != 4)
                                //        {
                                //            qcbatchinfo.Isedit = true;
                                //        }
                                //        else
                                //        {
                                //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Editfail"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        qcbatchinfo.Isedit = false;
                                //        if (BindSampleToGrid(aSPxSpreadsheet))
                                //        {
                                //            if (qcbatchinfo.strqcid != null)
                                //            {
                                //                QCBatch qC = os.FindObject<QCBatch>(CriteriaOperator.Parse("[QCBatchID]=?", qcbatchinfo.strqcid));
                                //                if (qC != null)
                                //                {
                                //                    qC.ABID.SpreadSheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.SaveDocument(DocumentFormat.OpenXml);
                                //                    IList<SpreadSheetEntry> spreadSheets = os.GetObjects<SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", qC.ABID.Oid));
                                //                    ABCreateUpdate(qC.ABID, spreadSheets);
                                //                    os.CommitChanges();
                                //                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Updatesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                //                }
                                //            }
                                //        }
                                //        else
                                //        {
                                //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Bindgridfail"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                //        }
                                //    }
                                //}
                                //else
                                //{
                                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotDataLoaded"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                //}
                                if (qcbatchinfo.IsSheetloaded)
                                {
                                    if (qcbatchinfo.Isedit == false)
                                    {
                                        //SpreadSheetEntry_AnalyticalBatch qC = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[QCBatchID]=?", qcbatchinfo.strqcid));
                                        SpreadSheetEntry_AnalyticalBatch qC = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strqcid));
                                        if (qC != null && qC.Status != 4)
                                        {
                                            qcbatchinfo.Isedit = true;
                                        }
                                        else
                                        {
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Editfail"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        }
                                    }
                                    else
                                    {
                                        qcbatchinfo.Isedit = false;
                                        if (BindSampleToGrid(aSPxSpreadsheet))
                                        {
                                            if (qcbatchinfo.strqcid != null)
                                            {
                                                //SpreadSheetEntry_AnalyticalBatch qC = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[QCBatchID]=?", qcbatchinfo.strqcid));
                                                SpreadSheetEntry_AnalyticalBatch qC = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strqcid));
                                                if (qC != null)
                                                {
                                                    qC.SpreadSheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.SaveDocument(DocumentFormat.OpenXml);
                                                    IList<SpreadSheetEntry> spreadSheets = os.GetObjects<SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", qC.Oid));
                                                    ABCreateUpdate(qC, spreadSheets);
                                                    os.CommitChanges();
                                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Updatesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Bindgridfail"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        }
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotDataLoaded"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else if (paramsplit[0] == "Save")
                            {
                                if (qcbatchinfo.IsSheetloaded)
                                {
                                    if (BindSampleToGrid(aSPxSpreadsheet))
                                    {
                                        bool isNew = false;
                                        SpreadSheetEntry_AnalyticalBatch batch = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                                        if (batch == null)
                                        {
                                            isNew = true;
                                            string tempqc = string.Empty;
                                            UnitOfWork uow = new UnitOfWork(((XPObjectSpace)os).Session.DataLayer);
                                            batch = os.CreateObject<SpreadSheetEntry_AnalyticalBatch>();
                                            string tempab = string.Empty;
                                            string userid = "_" + ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                                            var curdate = DateTime.Now.ToString("yyMMdd");
                                            //IList<SpreadSheetEntry_AnalyticalBatch> spreadSheets = os.GetObjects<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("SUBSTRING([AnalyticalBatchID], 2, 9)=?", curdate + userid)); 
                                            IList<SpreadSheetEntry_AnalyticalBatch> spreadSheets = os.GetObjects<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("SUBSTRING([AnalyticalBatchID], 2, 6)=?", curdate));
                                            if (spreadSheets.Count > 0)
                                            {
                                                spreadSheets = spreadSheets.OrderBy(a => a.AnalyticalBatchID).ToList();
                                                tempab = "QB" + curdate + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].AnalyticalBatchID.Substring(8, 2)) + 1).ToString("00") + userid;
                                            }
                                            else
                                            {
                                                tempab = "QB" + curdate + "01" + userid;
                                            }
                                            batch.AnalyticalBatchID = qcbatchinfo.strqcid = qcbatchinfo.strAB = tempab;
                                            batch.Humidity = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Humidity;
                                            batch.Roomtemp = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Roomtemp;
                                            batch.Instrument = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument;
                                            batch.Jobid = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Jobid;
                                            batch.TemplateID = qcbatchinfo.templateid;
                                            Employee objE = os.FindObject<Employee>(CriteriaOperator.Parse(" [FullName] = ?", objABinfo.lstSpreadSheetEntry_AnalyticalBatch.CreatedBy.FullName));
                                            batch.CreatedBy = objE;
                                            qcbatchinfo.AnalyticalQCBatchOid = batch.Oid;
                                            //qcbatchinfo.QCBatchOid = curqC.Oid;
                                            batch.Matrix = os.FindObject<Matrix>(CriteriaOperator.Parse("[Oid]=?", objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Matrix.Oid));
                                            batch.Method = os.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid]=?", objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Method.Oid));
                                            batch.Test = batch.Method;
                                            batch.Comments = objABinfo.Comments;
                                            string[] ids = batch.Instrument.Split(';');
                                            foreach (string id in ids)
                                            {
                                                QCBatchInstrument qcinstrument = os.CreateObject<QCBatchInstrument>();
                                                qcinstrument.QCBatchID = batch;
                                                qcinstrument.LabwareID = os.FindObject<Labware>(CriteriaOperator.Parse("[Oid] = ?", new Guid(id.Replace(" ", ""))));
                                            }
                                            os.CommitChanges();
                                            if (qcbatchinfo.dtsample.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in qcbatchinfo.dtsample.Rows)
                                                {
                                                    dr["QCBATCHID"] = batch.AnalyticalBatchID;
                                                }
                                                qcbatchinfo.dtsample.AcceptChanges();
                                            }
                                            List<string> strdilution = new List<string>();
                                            List<string> strdilutionno = new List<string>();
                                            int sort = 0;
                                            Guid qcseqoid = Guid.Empty;
                                            //IList<SampleParameter> lstsampleparam = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? and [Testparameter.TestMethod.Oid] =? and [Testparameter.QCType.Oid] =? And [SignOff] = True", strsamplid[0], qC1.Test.Oid, sequence.QCType.Oid)); //sequence.SampleID.SampleID//and [Testparameter.TestMethod.Oid] =? , qC.Test.Oid
                                            foreach (QCBatchSequence sequence1 in objABinfo.lstQCBatchSequence.OrderBy(i => i.Sort).ToList())
                                            {
                                                QCBatchSequence sequence = os.CreateObject<QCBatchSequence>();
                                                sequence.SYSSamplecode = sequence1.SYSSamplecode;
                                                sequence.QCType = os.FindObject<QCType>(CriteriaOperator.Parse("[Oid]=?", sequence1.QCType.Oid));
                                                sequence.Runno = sequence1.Runno;
                                                sequence.SampleAmount = sequence1.SampleAmount;
                                                sequence.Sort = sequence1.Sort;
                                                sequence.StrSampleID = sequence1.StrSampleID;
                                                sequence.SystemID = sequence1.SystemID;
                                                sequence.batchno = sequence1.batchno;
                                                sequence.IsDilution = sequence1.IsDilution;
                                                sequence.Dilution = sequence1.Dilution;
                                                if (sequence1.SampleID != null)
                                                {
                                                    sequence.SampleID = os.FindObject<SampleLogIn>(CriteriaOperator.Parse("[Oid]=?", sequence1.SampleID.Oid));
                                                }
                                                sequence.qcseqdetail = batch;
                                                if (sequence1.SampleID != null)
                                                {
                                                    sequence.SampleID = os.FindObject<SampleLogIn>(CriteriaOperator.Parse("[Oid]=?", sequence1.SampleID.Oid));
                                                }
                                                sequence.IsReport = true;
                                                os.CommitChanges();
                                                if (sequence.IsDilution == true)
                                                {
                                                    string[] strsamplid = sequence.SYSSamplecode.Split('R');
                                                    //SampleParameter sampleparams = qclist.InnerView.ObjectSpace.FindObject<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? and [Testparameter.TestMethod.Oid] =? And [SignOff] = True", strsamplid[0].ToString(), qC.Test.Oid)); //sequence.SampleID.SampleID
                                                    //if(strsamplid != null && strsamplid.Length == 1)
                                                    {
                                                        IList<SampleParameter> lstsampleparams = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? and [Testparameter.TestMethod.Oid] =? and [Testparameter.QCType.Oid] =? And [SignOff] = True", strsamplid[0], batch.Test.Oid, sequence.QCType.Oid)); //sequence.SampleID.SampleID//and [Testparameter.TestMethod.Oid] =? , qC.Test.Oid
                                                        if (lstsampleparams != null && lstsampleparams.Count > 0)
                                                        {
                                                            foreach (SampleParameter sampleparams in lstsampleparams.ToList())
                                                            {
                                                                {
                                                                    SampleParameter newsample = os.CreateObject<SampleParameter>();
                                                                    newsample.QCBatchID = sequence;
                                                                    newsample.QCSort = sort;
                                                                    newsample.SignOff = true;
                                                                    newsample.Samplelogin = os.GetObject(sampleparams.Samplelogin);
                                                                    newsample.Testparameter = os.GetObject(sampleparams.Testparameter);
                                                                    newsample.SamplePrepBatchID = os.GetObject(sampleparams.SamplePrepBatchID);
                                                                    newsample.UQABID = batch;
                                                                    if (qcbatchinfo.lststrseqstringdilution != null && qcbatchinfo.lststrseqstringdilution.Count > 0)
                                                                    {
                                                                        foreach (string objstrdil in qcbatchinfo.lststrseqstringdilution.ToList())
                                                                        {
                                                                            string[] strdil = objstrdil.Split('|');
                                                                            if (strdil[0].Contains(sequence.Oid.ToString()))
                                                                            {
                                                                                newsample.Dilution = strdil[1];
                                                                            }
                                                                        }
                                                                    }
                                                                    sort++;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (sequence.QCType != null && sequence.QCType.QCTypeName != "Sample")
                                                {
                                                    IList<Testparameter> testparams = os.GetObjects<Testparameter>(CriteriaOperator.Parse("[QCType.Oid]=? and [TestMethod.Oid]=?", sequence.QCType.Oid, batch.Test.Oid));
                                                    //foreach (Testparameter testparam in testparams.OrderBy(a => a.Sort).ThenBy(i => i.Parameter.ParameterName).ToList())
                                                    //{
                                                    //    SampleParameter newsample = qclist.InnerView.ObjectSpace.CreateObject<SampleParameter>();
                                                    //    newsample.QCBatchID = sequence;
                                                    //    newsample.Testparameter = testparam;
                                                    //    newsample.QCSort = sort;
                                                    //    newsample.SignOff = true;
                                                    //    //newsample.SignOffBy = qclist.InnerView.ObjectSpace.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                                                    //    //newsample.SignOffDate = DateTime.Now;
                                                    //    sort++;
                                                    //}
                                                    foreach (Testparameter testparam in testparams.OrderBy(a => a.Parameter.ParameterName).ToList())
                                                    {
                                                        SampleParameter newsample = os.CreateObject<SampleParameter>();
                                                        newsample.QCBatchID = sequence;
                                                        newsample.Testparameter = testparam;
                                                        newsample.QCSort = sort;
                                                        newsample.SignOff = true;
                                                        newsample.UQABID = batch;
                                                        sort++;
                                                        DataRow[] drrFilter = null;
                                                        if (!string.IsNullOrEmpty(sequence1.SystemID))
                                                        {
                                                            drrFilter = qcbatchinfo.dtsample.Select("SystemID = '" + sequence1.SystemID + "' and  SYSSamplecode= '" + sequence1.SYSSamplecode + "' and  PARAMETER= '" + testparam.Parameter.ParameterName + "'");
                                                        }
                                                        else
                                                        {
                                                            drrFilter = qcbatchinfo.dtsample.Select("(SystemID is null or SystemID = '') and  SYSSamplecode= '" + sequence1.SYSSamplecode + "' and  PARAMETER= '" + testparam.Parameter.ParameterName + "'");
                                                        }
                                                        if (drrFilter != null && drrFilter.Length > 0)
                                                        {
                                                            foreach (DataRow drParam in drrFilter)
                                                            {
                                                                drParam["UQSAMPLEPARAMETERID"] = newsample.Oid;
                                                            }
                                                        }
                                                        qcbatchinfo.dtsample.AcceptChanges();

                                                    }
                                                }
                                                else
                                                {
                                                    IList<SampleParameter> sampleparams = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? and [Testparameter.TestMethod.Oid] =? And [SignOff] = True", sequence.SampleID.SampleID, batch.Test.Oid));
                                                    foreach (SampleParameter sampleparam in sampleparams.OrderBy(a => a.Testparameter.Sort).ThenBy(a => a.Testparameter.Parameter.ParameterName).ToList())
                                                    {
                                                        sampleparam.QCBatchID = sequence;
                                                        sampleparam.QCSort = sort;
                                                        sort++;
                                                    }
                                                    foreach (SampleParameter sampleparam in sampleparams.OrderBy(a => a.Testparameter.Parameter.ParameterName).ToList())
                                                    {
                                                        if (qcbatchinfo.lststrseqstringdilution != null && qcbatchinfo.lststrseqstringdilution.Count > 0)
                                                        {
                                                            foreach (string objstrdil in qcbatchinfo.lststrseqstringdilution.ToList())
                                                            {
                                                                string[] strdil = objstrdil.Split('|');
                                                                if (strdil[0].Contains(sequence.Oid.ToString()))
                                                                {
                                                                    sampleparam.Dilution = strdil[1];
                                                                }
                                                            }
                                                        }
                                                        sampleparam.QCBatchID = sequence;
                                                        sampleparam.QCSort = sort;
                                                        sampleparam.UQABID = batch;
                                                        sort++;
                                                    }
                                                }
                                            }
                                            os.CommitChanges();
                                            //ResetNavigationCount();
                                        }
                                        if (qcbatchinfo.strAB != null)
                                        {
                                            SpreadSheetEntry_AnalyticalBatch ABID = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                                            if (ABID != null && ABID.Status == 1)
                                            {
                                                ////qcbatchinfo.QCBatchOid = ABID.Oid;
                                                ABID.SpreadSheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.SaveDocument(DocumentFormat.OpenXml);
                                                IList<SpreadSheetEntry> spreadSheets = os.GetObjects<SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", ABID.Oid));
                                                ABCreateUpdate(ABID, spreadSheets);
                                                os.CommitChanges();
                                                if (qcbatchinfo.IsMoldTest)
                                                {
                                                    InsertUpdateMoldResults();
                                                }
                                                if (qcbatchinfo.IsPLMTest)
                                                    InsertUpdatePLMResults();
                                                if (ABID.Template != null && ABID.Template.CalibrationLevelNo > 0)
                                                {
                                                    if (ABID.Calibration == null)
                                                    {
                                                        NewCBID(ABID.TemplateID, true);
                                                        Calibration calib = os.FindObject<Calibration>(CriteriaOperator.Parse("[CalibrationID] = ?", qcbatchinfo.strCB));
                                                        ABID.Calibration = calib;
                                                        os.CommitChanges();
                                                    }
                                                    else
                                                    {
                                                        UpdateCBID(ABID.Calibration);
                                                        os.CommitChanges();
                                                    }
                                                }
                                                if (!isNew)
                                                {
                                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Updatesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                                }
                                                else
                                                {
                                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "QCBatchIDCreated"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                                }
                                                ////Getdtsamplesource(qcbatchinfo.QCBatchOid);
                                            }
                                        }
                                        ABGridrefresh();
                                        CalibRefresh();
                                        CalibAndABIDSupport(aSPxSpreadsheet.ASPxSpreadsheetControl);
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Bindgridfail"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotDataLoaded"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else if (paramsplit[0] == "Complete")
                            {
                                if (qcbatchinfo.IsSheetloaded)
                                {
                                    bool Savecomplete = true;
                                    DefaultSetting defaultSetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Spreadsheet'"));
                                    DataRow[] ExportColumns = qcbatchinfo.dtselectedsamplefields.Select("[ExportSample] <> '' and [ExportSample] is not null");
                                    if (defaultSetting != null)
                                    {
                                        //if (qcbatchinfo.strAB == null)
                                        //{
                                        //    Savecomplete = false;
                                        //    if (BindSampleToGrid(aSPxSpreadsheet))
                                        //    {
                                        //        if (qcbatchinfo.strqcid != null)
                                        //        {
                                        //            SpreadSheetEntry_AnalyticalBatch qC = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[QCBatchID]=?", qcbatchinfo.strqcid));
                                        //            if (qC != null)
                                        //            {
                                        //                if (qC.ABID == null)
                                        //                {
                                        //                    NewABID(aSPxSpreadsheet.ASPxSpreadsheetControl, qC);
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        if (qcbatchinfo.strAB != null)
                                        {
                                            SpreadSheetEntry_AnalyticalBatch ABspreadSheet = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                                            if (ABspreadSheet != null)
                                            {
                                                IList<SpreadSheetEntry> spreadSheets = os.GetObjects<SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", ABspreadSheet.Oid));
                                                IList<SpreadsheetEntry_MoldResults> moldResults = os.GetObjects<SpreadsheetEntry_MoldResults>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", ABspreadSheet.Oid));
                                                bool insertresult = false;
                                                if (qcbatchinfo.strMode == "Enter")
                                                {
                                                    if (Savecomplete)
                                                    {
                                                        ABspreadSheet.SpreadSheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.SaveDocument(DocumentFormat.OpenXml);
                                                        ABCreateUpdate(ABspreadSheet, spreadSheets);
                                                    }
                                                    if (ValidateGridResults())
                                                    {
                                                        ABspreadSheet.Status = 2;
                                                        ABspreadSheet.DPStatus = DataPackageStatus.PendingSubmission;
                                                        foreach (SpreadSheetEntry spreadSheet in spreadSheets)
                                                        {
                                                            spreadSheet.IsComplete = true;
                                                            if (spreadSheet.uqSampleParameterID != null)
                                                            {
                                                                spreadSheet.uqSampleParameterID.Status = Samplestatus.PendingReview;
                                                                spreadSheet.uqSampleParameterID.OSSync = true;
                                                            }
                                                            if (defaultSetting.Review == EnumRELevelSetup.No && defaultSetting.Verify == EnumRELevelSetup.No)
                                                            {
                                                                ABspreadSheet.Status = 4;
                                                                spreadSheet.IsExported = true;
                                                                spreadSheet.ReviewedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                                spreadSheet.ReviewedDate = DateTime.Now;
                                                                spreadSheet.VerifiedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                                spreadSheet.VerifiedDate = DateTime.Now;
                                                                if (moldResults != null)
                                                                {
                                                                    foreach (SpreadsheetEntry_MoldResults moldResult in moldResults)
                                                                    {
                                                                        moldResult.ReviewedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                                        moldResult.ReviewedDate = DateTime.Now;
                                                                        moldResult.VerifiedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                                        moldResult.VerifiedDate = DateTime.Now;
                                                                    }
                                                                }
                                                                getstatus(spreadSheet, defaultSetting);
                                                                insertresult = Inserttosampleparameter(spreadSheet, ExportColumns);
                                                                if (sdmsinfo.lstqcbatcseqoid != null && sdmsinfo.lstqcbatcseqoid.Count > 0)
                                                                {
                                                                    IObjectSpace ossamplpara = Application.CreateObjectSpace(typeof(SampleParameter));
                                                                    foreach (Guid objqcseqoid in sdmsinfo.lstqcbatcseqoid.ToList())
                                                                    {
                                                                        SampleParameter objsmplpara = os.FindObject<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.Oid] = ?", objqcseqoid));
                                                                        if (objsmplpara != null)
                                                                        {
                                                                            objsmplpara.IsExported = true;
                                                                        }
                                                                        os.CommitChanges();
                                                                    }
                                                                }
                                                            }
                                                            else if (defaultSetting.Review == EnumRELevelSetup.No && defaultSetting.Verify == EnumRELevelSetup.Yes)
                                                            {
                                                                ABspreadSheet.Status = 3;
                                                                if (spreadSheet.uqSampleParameterID != null)
                                                                {
                                                                    spreadSheet.uqSampleParameterID.Status = Samplestatus.PendingVerify;
                                                                    spreadSheet.uqSampleParameterID.OSSync = true;
                                                                }
                                                                spreadSheet.ReviewedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                                spreadSheet.ReviewedDate = DateTime.Now;
                                                                if (moldResults != null)
                                                                {
                                                                    foreach (SpreadsheetEntry_MoldResults moldResult in moldResults)
                                                                    {
                                                                        moldResult.ReviewedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                                        moldResult.ReviewedDate = DateTime.Now;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                insertresult = true;
                                                            }
                                                        }
                                                        if (qcbatchinfo.IsMoldTest)
                                                        {
                                                            bindMoldSample(spreadsheet);
                                                        }
                                                        if (defaultSetting.Review == EnumRELevelSetup.No && defaultSetting.Verify == EnumRELevelSetup.No)
                                                        {
                                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Completeexport"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                                        }
                                                        else
                                                        {
                                                            //ResetNavigationCount();
                                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CompleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                                        }
                                                        if (ABspreadSheet.Status == 3)
                                                        {
                                                            ABspreadSheet.ReviewedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                            ABspreadSheet.ReviewedDate = DateTime.Now;
                                                        }
                                                        if (ABspreadSheet.Status == 4)
                                                        {
                                                            ABspreadSheet.ReviewedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                            ABspreadSheet.ReviewedDate = DateTime.Now;
                                                            ABspreadSheet.VerifiedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                            ABspreadSheet.VerifiedDate = DateTime.Now;
                                                        }
                                                        os.CommitChanges();

                                                        changetoviewmode();
                                                        //if (insertresult)
                                                        //{
                                                        //    if (defaultSetting.Review == EnumRELevelSetup.No)
                                                        //    {
                                                        //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Completeexport"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CompleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                                        //    }
                                                        //    if (ABspreadSheet.Status == 3)
                                                        //    {
                                                        //        ABspreadSheet.ReviewedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                        //        ABspreadSheet.ReviewedDate = DateTime.Now;
                                                        //    }
                                                        //    if (ABspreadSheet.Status == 4)
                                                        //    {
                                                        //        ABspreadSheet.ReviewedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                        //        ABspreadSheet.ReviewedDate = DateTime.Now;
                                                        //        ABspreadSheet.VerifiedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                        //        ABspreadSheet.VerifiedDate = DateTime.Now;
                                                        //    }
                                                        //    os.CommitChanges();
                                                        //    //clearcontrols(aSPxSpreadsheet.ASPxSpreadsheetControl);                                                       
                                                        //    changetoviewmode();
                                                        //}
                                                    }
                                                    string[] ids = ABspreadSheet.Jobid.Split(';');
                                                    foreach (string obj in ids)
                                                    {
                                                        Samplecheckin objSamplecheckin = os.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", obj));
                                                    IList<SampleParameter> lstSamples = os.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                                    if (lstSamples.Where(i => i.Status == Samplestatus.PendingEntry).Count() == 0)
                                                    {
                                                        StatusDefinition objStatus = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 18"));
                                                        if (objStatus != null)
                                                        {
                                                            objSamplecheckin.Index = objStatus;
                                                        }
                                                        os.CommitChanges();
                                                    }

                                                    }
                                                    //ResetNavigationCount();
                                                }
                                                else if (qcbatchinfo.strMode == "Review")
                                                {
                                                    ABspreadSheet.Status = 3;
                                                    ABspreadSheet.ReviewedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                    ABspreadSheet.ReviewedDate = DateTime.Now;
                                                    foreach (SpreadSheetEntry spreadSheet in spreadSheets)
                                                    {
                                                        spreadSheet.ReviewedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                        spreadSheet.ReviewedDate = DateTime.Now;
                                                        if (spreadSheet.uqSampleParameterID != null)
                                                        {
                                                            spreadSheet.uqSampleParameterID.Status = Samplestatus.PendingVerify;
                                                            spreadSheet.uqSampleParameterID.OSSync = true;
                                                        }
                                                        if (defaultSetting.Verify == EnumRELevelSetup.No)
                                                        {
                                                            ABspreadSheet.Status = 4;
                                                            spreadSheet.IsExported = true;
                                                            spreadSheet.VerifiedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                            spreadSheet.VerifiedDate = DateTime.Now;
                                                            getstatus(spreadSheet, defaultSetting);
                                                            insertresult = Inserttosampleparameter(spreadSheet, ExportColumns);
                                                        }
                                                        else
                                                        {
                                                            insertresult = true;
                                                        }
                                                    }
                                                    if (moldResults != null)
                                                    {
                                                        foreach (SpreadsheetEntry_MoldResults moldResult in moldResults)
                                                        {
                                                            moldResult.ReviewedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                            moldResult.ReviewedDate = DateTime.Now;
                                                            if (defaultSetting.Verify == EnumRELevelSetup.No)
                                                            {
                                                                moldResult.VerifiedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                                moldResult.VerifiedDate = DateTime.Now;
                                                            }
                                                        }
                                                    }
                                                    if (insertresult)
                                                    {
                                                        if (defaultSetting.Review == EnumRELevelSetup.No)
                                                        {
                                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Reviewexported"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                                        }
                                                        else
                                                        {
                                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "reviewsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                                        }
                                                        if (ABspreadSheet.Status == 4)
                                                        {
                                                            ABspreadSheet.ReviewedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                            ABspreadSheet.ReviewedDate = DateTime.Now;
                                                            ABspreadSheet.VerifiedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                            ABspreadSheet.VerifiedDate = DateTime.Now;
                                                        }
                                                        os.CommitChanges();
                                                        qcbatchinfo.strTest = ABspreadSheet.Test.TestName;
                                                        //clearcontrols(aSPxSpreadsheet.ASPxSpreadsheetControl);
                                                        changetoviewmode();
                                                    }
                                                    ShowNavigationItemController ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
                                                    if (ShowNavigationController != null && ShowNavigationController.ShowNavigationItemAction != null)
                                                    {
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
                                                                            IList<SpreadSheetEntry_AnalyticalBatch> lstABS = os.GetObjects<SpreadSheetEntry_AnalyticalBatch>(new InOperator("Test.Oid", lstTestMethodOid));
                                                                            //if (lstABS.Count > 0)
                                                                            //{
                                                                            //    int count = lstABS.Where(a => a.Status == 2).Select(a => a.Oid).Count();
                                                                            //    if (count > 0)
                                                                            //    {
                                                                            //        child.Caption = cap[0] + "(" + count + ")";
                                                                            //        break;
                                                                            //    }
                                                                            //    else
                                                                            //    {
                                                                            //        child.Caption = cap[0];
                                                                            //        break;
                                                                            //    }
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
                                                }
                                                else if (qcbatchinfo.strMode == "Verify")
                                                {
                                                    ABspreadSheet.Status = 4;
                                                    ABspreadSheet.VerifiedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                    ABspreadSheet.VerifiedDate = DateTime.Now;
                                                    foreach (SpreadSheetEntry spreadSheet in spreadSheets)
                                                    {
                                                        spreadSheet.IsExported = true;
                                                        spreadSheet.VerifiedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                        spreadSheet.VerifiedDate = DateTime.Now;
                                                        getstatus(spreadSheet, defaultSetting);
                                                        insertresult = Inserttosampleparameter(spreadSheet, ExportColumns);

                                                        if (qcbatchinfo.dtDataParsing != null)
                                                        {
                                                            string strisReport = string.Empty;
                                                            DataRow[] drrSelect = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable'");
                                                            strisReport = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "IsReport").Select(r => r["Position"].ToString()).SingleOrDefault();
                                                            if (string.IsNullOrWhiteSpace(strisReport))
                                                            {
                                                                ////if (ValidateGridIsReport())
                                                                ////{
                                                                ////    if (sdmsinfo.lstqcbatcseqoid != null && sdmsinfo.lstqcbatcseqoid.Count > 0)
                                                                ////    {
                                                                ////        IObjectSpace ossamplpara = Application.CreateObjectSpace(typeof(SampleParameter));
                                                                ////        foreach (Guid objqcseqoid in sdmsinfo.lstqcbatcseqoid.ToList())
                                                                ////        {
                                                                ////            SampleParameter objsmplpara = os.FindObject<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.Oid] = ?", objqcseqoid));
                                                                ////            if (objsmplpara != null)
                                                                ////            {
                                                                ////                objsmplpara.IsExported = true;
                                                                ////            }
                                                                ////            os.CommitChanges();
                                                                ////        }
                                                                ////    }
                                                                ////}
                                                            }
                                                        }

                                                    }
                                                    if (insertresult)
                                                    {
                                                        if (ABspreadSheet.Status == 4)
                                                        {
                                                            ABspreadSheet.VerifiedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                            ABspreadSheet.VerifiedDate = DateTime.Now;
                                                        }
                                                        os.CommitChanges();
                                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Verifyexported"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                                        //clearcontrols(aSPxSpreadsheet.ASPxSpreadsheetControl);
                                                        changetoviewmode();
                                                    }
                                                    qcbatchinfo.strTest = ABspreadSheet.Test.TestName;
                                                    ShowNavigationItemController ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
                                                    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                                                    {
                                                        if (parent.Id == "Data Review")
                                                        {
                                                            foreach (ChoiceActionItem child in parent.Items)
                                                            {
                                                                if (child.Id == "Result Validation")
                                                                {
                                                                    int count = 0;
                                                                    IObjectSpace objSpace = Application.CreateObjectSpace();
                                                                    using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                                                                    {
                                                                        lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingValidation' And[SignOff] = True And([QCBatchID] Is Null Or[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] = 'Sample')");
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
                                                                    //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                                    //if (count > 0)
                                                                    //{
                                                                    //    child.Caption = cap[0] + " (" + count + ")";
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    child.Caption = cap[0];
                                                                    //}
                                                                }
                                                                else if (child.Id == "Result Approval")
                                                                {
                                                                    int count = 0;
                                                                    IObjectSpace objSpace = Application.CreateObjectSpace();
                                                                    using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                                                                    {
                                                                        lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingApproval' And[SignOff] = True And([QCBatchID] Is Null Or[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] = 'Sample')");
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
                                                                    //if (count > 0)
                                                                    //{
                                                                    //    child.Caption = cap[0] + " (" + count + ")";
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    child.Caption = cap[0];
                                                                    //}
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                ABGridrefresh();
                                            }
                                        }
                                        else
                                        {
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "QCBatchIDnotgenerated"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        }
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotDataLoaded"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else if (paramsplit[0] == "RollBack")
                            {
                                if (qcbatchinfo.strAB != null && qcbatchinfo.IsSheetloaded)
                                {
                                    SpreadSheetEntry_AnalyticalBatch ABspreadSheet = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                                    if (ABspreadSheet != null)
                                    {
                                        bool result = true;
                                        IList<SpreadSheetEntry> spreadSheets = os.GetObjects<SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", ABspreadSheet.Oid));
                                        foreach (SpreadSheetEntry spreadSheet in spreadSheets)
                                        {
                                            if (spreadSheet.uqSampleParameterID != null && spreadSheet.uqSampleParameterID.Status == Samplestatus.Reported)
                                            {
                                                result = false;
                                            }
                                        }
                                        if (result)
                                        {
                                            Popupos = Application.CreateObjectSpace();
                                            object objToShow = Popupos.CreateObject(typeof(SDMSRollback));
                                            DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                                            CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                                            ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                                            showViewParameters.Context = TemplateContext.PopupWindow;
                                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                            showViewParameters.CreatedView.Caption = "SDMS";
                                            DialogController dc = Application.CreateController<DialogController>();
                                            dc.SaveOnAccept = false;
                                            dc.CloseOnCurrentObjectProcessing = false;
                                            dc.Accepting += Dc_Accepting;
                                            showViewParameters.Controllers.Add(dc);
                                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                        }
                                        else
                                        {
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbackfailed"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        }
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotDataLoaded"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else if (paramsplit[0] == "DataPackage")
                            {
                                if (qcbatchinfo.strAB != null && qcbatchinfo.IsSheetloaded)
                                {
                                    SpreadSheetEntry_AnalyticalBatch ABspreadSheet = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                                    if (ABspreadSheet != null)
                                    {
                                        bool result = true;
                                        {
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            SpreadSheetEntry_AnalyticalBatch objSSEAB = objectSpace.GetObject<SpreadSheetEntry_AnalyticalBatch>(ABspreadSheet);
                                            if (objSSEAB.DPStatus == DataPackageStatus.PendingSubmission)
                                            {
                                                DetailView dv = Application.CreateDetailView(objectSpace, "SpreadSheetEntry_AnalyticalBatch_DetailView_DataPackage_Queue", true, objSSEAB);
                                                dv.ViewEditMode = ViewEditMode.Edit;
                                                Application.MainWindow.SetView(dv);
                                            }
                                            else
                                            if (objSSEAB.DPStatus == DataPackageStatus.PendingReview)
                                            {
                                                DetailView dv = Application.CreateDetailView(objectSpace, "SpreadSheetEntry_AnalyticalBatch_DetailView_DataPackage_Review", true, objSSEAB);
                                                dv.ViewEditMode = ViewEditMode.Edit;
                                                Application.MainWindow.SetView(dv);
                                            }
                                            else
                                            if (objSSEAB.DPStatus == DataPackageStatus.Submitted)
                                            {
                                                DetailView dv = Application.CreateDetailView(objectSpace, "SpreadSheetEntry_AnalyticalBatch_DetailView_DataPackage_History_Queue", true, objSSEAB);
                                                dv.ViewEditMode = ViewEditMode.Edit;
                                                Application.MainWindow.SetView(dv);
                                            }
                                            else
                                            if (objSSEAB.DPStatus == DataPackageStatus.Reviewed)
                                            {
                                                DetailView dv = Application.CreateDetailView(objectSpace, "SpreadSheetEntry_AnalyticalBatch_DetailView_DataPackage_History_Review", true, objSSEAB);
                                                dv.ViewEditMode = ViewEditMode.Edit;
                                                Application.MainWindow.SetView(dv);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotDataLoaded"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else if (paramsplit[0] == "Delete")
                            {
                                if (qcbatchinfo.strAB != null && qcbatchinfo.IsSheetloaded)
                                {
                                    SpreadSheetEntry_AnalyticalBatch ABspreadSheet = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                                    if (ABspreadSheet != null)
                                    {
                                        IList<SpreadSheetEntry> spreadSheets = os.GetObjects<SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", ABspreadSheet.Oid));
                                        foreach (SpreadSheetEntry spreadSheet in spreadSheets)
                                        {
                                            if (spreadSheet.uqSampleParameterID != null && !string.IsNullOrEmpty(spreadSheet.uqSampleParameterID.ABID))
                                            {
                                                spreadSheet.uqSampleParameterID.ABID = null;
                                                spreadSheet.uqSampleParameterID.UQABID = null;
                                                spreadSheet.uqSampleParameterID.Result = null;
                                                spreadSheet.uqSampleParameterID.ResultNumeric = null;
                                                spreadSheet.uqSampleParameterID.FinalResult = null;
                                                spreadSheet.uqSampleParameterID.QCBatchID = null;
                                                spreadSheet.uqSampleParameterID.QCSort = 0;
                                                spreadSheet.uqSampleParameterID.Status = Samplestatus.PendingEntry;
                                                spreadSheet.uqSampleParameterID.OSSync = true;
                                            }
                                        }
                                        if (qcbatchinfo.IsMoldTest)
                                        {
                                            DeleteMold(ABspreadSheet);
                                        }
                                        if (qcbatchinfo.IsPLMTest)
                                            DeletePLM(ABspreadSheet);
                                        os.Delete(spreadSheets);
                                        ABspreadSheet.AnalyticalBatchID = null;
                                        os.Delete(ABspreadSheet);
                                        os.CommitChanges();
                                        Notes notes = os.FindObject<Notes>(CriteriaOperator.Parse("[SourceID] =? AND [NoteSource] = 'SDMS' ", qcbatchinfo.strAB));
                                        os.Delete(notes);
                                        os.CommitChanges();
                                        IList<SampleParameter> lstSamples = os.GetObjects<SampleParameter>(new InOperator("Samplelogin.JobID.JobID", ABspreadSheet.Jobid.Split(';').ToList().Where(i => i != null).Select(i => i.Trim()).ToList())).ToList();
                                        //Samplecheckin objSamplecheckin = os.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", ABspreadSheet.Jobid));
                                        //IList<SampleParameter> lstSamples = os.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                        //if (lstSamples.Where(i => i.Status == Samplestatus.PendingEntry).Count() == 0)
                                        //{
                                        //    StatusDefinition objStatus = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Index] = '118'"));
                                        //    if (objStatus != null)
                                        //    {
                                        //        objSamplecheckin.Index = objStatus;
                                        //    }
                                        //    os.CommitChanges();
                                        //}

                                        if (lstSamples.FirstOrDefault(i => i.PrepMethodCount > 0) != null)
                                            {
                                                StatusDefinition statusDefinition = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=16"));
                                                if (statusDefinition != null)
                                                {
                                                    lstSamples.FirstOrDefault().Samplelogin.JobID.Index = statusDefinition;
                                                    os.CommitChanges();
                                                }
                                            }
                                            else
                                            {
                                                StatusDefinition statusDefinition = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=10"));
                                                if (statusDefinition != null)
                                                {
                                                    lstSamples.FirstOrDefault().Samplelogin.JobID.Index = statusDefinition;
                                                    os.CommitChanges();
                                                }
                                            }
                                            View.ObjectSpace.CommitChanges();
                                      




                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                        clearcontrols(aSPxSpreadsheet.ASPxSpreadsheetControl);
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotDataLoaded"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else if (paramsplit[0] == "Mode")
                            {
                                qcbatchinfo.strMode = paramsplit[2];
                                clearcontrols(aSPxSpreadsheet.ASPxSpreadsheetControl);
                                //setRibbonForMold(aSPxSpreadsheet.ASPxSpreadsheetControl);
                                //fillComboMold(aSPxSpreadsheet.ASPxSpreadsheetControl);
                            }
                            else if (paramsplit[0] == "Test")
                            {
                                string[] testsplit = paramsplit[1].Split(';');
                                qcbatchinfo.strTest = testsplit[0];
                                qcbatchinfo.OidTestMethod = new Guid(testsplit[1]);
                                qcbatchinfo.strAB = null;
                                qcbatchinfo.strCB = null;
                                qcbatchinfo.strqcid = null;
                                qcbatchinfo.qcstatus = 0;
                            }
                            else if (paramsplit[0] == "JobID")
                            {
                                ((RibbonComboBoxItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("JobID")).Value = paramsplit[2];
                                if (qcbatchinfo.dtsample != null && qcbatchinfo.dtsample.Rows.Count > 0)
                                {
                                    DataRow drSamples = qcbatchinfo.dtsample.Select("JOBID = " + paramsplit[2]).FirstOrDefault();
                                    string strSysSampleCode = drSamples["SYSSAMPLECODE"].ToString();
                                    ((RibbonComboBoxItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Value = strSysSampleCode;
                                    if (aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets.Contains(strSysSampleCode))
                                    {
                                        aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets[strSysSampleCode];
                                    }
                                }
                            }
                            else if (paramsplit[0] == "SampleID")
                            {
                                ((RibbonComboBoxItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Value = paramsplit[2];
                                if (aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets.Contains(paramsplit[2]))
                                {
                                    aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets[paramsplit[2]];
                                    int sheetIndex = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets.ActiveWorksheet.Index;
                                    //if (sheetIndex == 0)
                                    //{
                                    //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).ClientEnabled = false;
                                    //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = true;
                                    //}
                                    //else if (sheetIndex == aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets.Count - 2)
                                    //{
                                    //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).ClientEnabled = true;
                                    //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = false;
                                    //}
                                    //else
                                    //{
                                    //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).ClientEnabled = true;
                                    //    ((RibbonButtonItem)aSPxSpreadsheet.ASPxSpreadsheetControl.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).ClientEnabled = true;
                                    //}
                                }
                            }
                            else if (paramsplit[0] == "ImportFiles")
                            {
                                if (qcbatchinfo.dtDataTransfer.Rows.Count > 0)
                                {
                                    qcbatchinfo.strDataTransfer = qcbatchinfo.dtDataTransfer.Rows[0]["Name"].ToString();
                                    NonPersistentObjectSpace nos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(SDMSDCImport));
                                    DashboardView dashboard = Application.CreateDashboardView(nos, "ImportFiles", false);
                                    ShowViewParameters showViewParameters = new ShowViewParameters(dashboard);
                                    showViewParameters.Context = TemplateContext.PopupWindow;
                                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                    showViewParameters.CreatedView.Caption = rm.GetString("ImportInstrumentFile_" + CurrentLanguage);
                                    DialogController dc = Application.CreateController<DialogController>();
                                    dc.SaveOnAccept = false;
                                    dc.CloseOnCurrentObjectProcessing = false;
                                    dc.AcceptAction.Active.SetItemValue("enb", false);
                                    dc.CancelAction.Active.SetItemValue("enb", false);
                                    showViewParameters.Controllers.Add(dc);
                                    dc.Accepting += Dc_AcceptingImport;
                                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "nodatatransfertemplate"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else if (paramsplit[0] == "UploadImage")
                            {
                                DashboardView dashboard = Application.CreateDashboardView(Application.CreateObjectSpace(), "UploadImage", false);
                                ShowViewParameters showViewParameters = new ShowViewParameters(dashboard);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                showViewParameters.CreatedView.Caption = "SDMS";
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.SaveOnAccept = false;
                                dc.CloseOnCurrentObjectProcessing = false;
                                dc.AcceptAction.Active.SetItemValue("enb", false);
                                dc.CancelAction.Caption = dc.CancelAction.ToolTip = rm.GetString("Close_" + CurrentLanguage);
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            }
                            else if (paramsplit[0] == "GenerateReports")
                            {
                                if (qcbatchinfo.strAB != null && qcbatchinfo.IsSheetloaded)
                                {
                                    SpreadSheetEntry_AnalyticalBatch ABspreadSheet = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                                    if (ABspreadSheet != null)
                                    {
                                        if (qcbatchinfo.strTest.StartsWith("Mold"))
                                        {
                                            BindMoldSampleToReport(aSPxSpreadsheet.ASPxSpreadsheetControl, ABspreadSheet.Oid);
                                        }
                                        else
                                        {
                                            BindSampleToReport(aSPxSpreadsheet, ABspreadSheet.Oid);
                                        }
                                        if (dtDetailDataNew != null && dtDetailDataNew.Rows.Count > 0)
                                        {
                                            dtReporting = new DataTable();
                                            dtReporting = GetReportingInfo(ABspreadSheet.TemplateID);
                                            if (dtReporting != null && dtReporting.Rows.Count > 0)
                                            {
                                                if (dtReporting.Rows.Count == 1)
                                                {
                                                    if (dtReporting.Rows[0]["FileType"].ToString() == "DEV")
                                                    {
                                                        ReportPreview(dtReporting.Rows[0]);
                                                    }
                                                }
                                                else if (dtReporting.Rows.Count > 1)
                                                {
                                                    IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SDMSReportPopupDC));
                                                    CollectionSource cs = new CollectionSource(objectSpace, typeof(SDMSReportPopupDC));
                                                    foreach (DataRow dr in dtReporting.Rows)
                                                    {
                                                        if (dr["FileType"].ToString() == "DEV")
                                                        {
                                                            SDMSReportPopupDC obj = new SDMSReportPopupDC();
                                                            obj.ID = Convert.ToInt32(dr["uqReportDesignID"]);
                                                            obj.Report = dr["Name"].ToString();
                                                            cs.Add(obj);
                                                        }
                                                    }
                                                    ListView CreatedListView = Application.CreateListView("SDMSReportPopupDC_ListView", cs, true);
                                                    ShowViewParameters showViewParameters = new ShowViewParameters(CreatedListView);
                                                    showViewParameters.Context = TemplateContext.PopupWindow;
                                                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                                    showViewParameters.CreatedView.Caption = "SDMS";
                                                    DialogController dc = Application.CreateController<DialogController>();
                                                    dc.SaveOnAccept = false;
                                                    dc.CancelAction.Active.SetItemValue("disable", false);
                                                    dc.CloseOnCurrentObjectProcessing = false;
                                                    dc.Accepting += Dc_AcceptingReport;
                                                    showViewParameters.Controllers.Add(dc);
                                                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                                }
                                            }
                                            else
                                            {
                                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "noRawDataReportTemplate"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (qcbatchinfo.strAB == null)
                                    {
                                        //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Abidnotgenerated"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "QCBatchIDnotgenerated"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotDataLoaded"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    }
                                }
                            }
                            else if (paramsplit[0] == "NewCalibration")
                            {
                                //int rowcount = qcbatchinfo.dtCalibration.Rows.Count;
                                if (qcbatchinfo.OidTestMethod != null)
                                {
                                    if (qcbatchinfo.templateid > 0)
                                    {
                                        NewCBID(qcbatchinfo.templateid, IsCommit: true);
                                    }
                                    else
                                    {
                                        SpreadSheetBuilder_TestParameter testParameter = os.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID]=?", qcbatchinfo.OidTestMethod));
                                        if (testParameter != null)
                                        {
                                            SpreadSheetBuilder_TemplateInfo templateInfo = os.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID]=?", testParameter.TemplateID));
                                            if (templateInfo != null)
                                            {
                                                NewCBID(templateInfo.TemplateID, IsCommit: true);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Calibnotavailable"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else if (paramsplit[0] == "CalibrationID")
                            {
                                string strCBID = paramsplit[1].ToString();
                                if (!string.IsNullOrEmpty(strCBID) && strCBID.ToLower() != "true" && strCBID.ToLower() != "false")
                                {
                                    qcbatchinfo.strCB = paramsplit[1];
                                }
                                else
                                {
                                    qcbatchinfo.strCB = string.Empty;
                                }
                            }
                            else if (paramsplit[0] == "ImportExlFiles")
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                Object objToShow = os.CreateObject(typeof(ItemsFileUpload));
                                DetailView CreatedDetailView = Application.CreateDetailView(os, "ItemsFileUpload_DetailView", true, objToShow);
                                CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                                ShowViewParameters showViewParametersimport = new ShowViewParameters(CreatedDetailView);
                                showViewParametersimport.Context = TemplateContext.NestedFrame;
                                showViewParametersimport.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController importdc = Application.CreateController<DialogController>();
                                importdc.SaveOnAccept = false;
                                importdc.Accepting += Dc_AcceptingImportfile;
                                importdc.CloseOnCurrentObjectProcessing = false;
                                showViewParametersimport.Controllers.Add(importdc);
                                Application.ShowViewStrategy.ShowView(showViewParametersimport, new ShowViewSource(null, null));
                            }
                            else if (paramsplit[0] == "ExportExlFiles")
                            {
                                //using (MemoryStream documentContentAsStream = new MemoryStream())
                                //{
                                //    spreadsheet.save(documentContentAsStream, DocumentFormat.Xlsx);
                                //    // Your custom logic to save a document to a custom storage
                                //}
                                //string custominit = @"function(s,e)
                                //{
                                //   comboresize();                                        
                                //   if(s.cpMode != null && s.GetRibbon().tabs[0].groups[0].items[0].GetValue() == null)
                                //   {
                                //      RaiseXafCallback(globalCallbackControl, 'itemname', 'Reload|' + s.isInFullScreenMode, '', false);  
                                //   }
                                //}";
                                //ClientSideEventsHelper.AssignClientHandlerSafe(spreadsheet, "Init", custominit, "Spreadsheettemplate22");
                                if (qcbatchinfo.strMode != null && qcbatchinfo.strAB != null)
                                {
                                    SpreadSheetEntry_AnalyticalBatch batch = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                                    if (batch != null)
                                    {
                                        exportfile = true;
                                        qcbatchinfo.strqcid = batch.AnalyticalBatchID;
                                        qcbatchinfo.QCBatchOid = batch.Oid;
                                        LoadfromAB(aSPxSpreadsheet.ASPxSpreadsheetControl, batch);
                                    }
                                }
                            }
                            else if (paramsplit[0] == "Recalculate")
                            {
                                Recalculate(aSPxSpreadsheet.ASPxSpreadsheetControl);
                            }
                        }
                        if (Param[0] != "DataPackage")
                        {
                            fillcombo(aSPxSpreadsheet.ASPxSpreadsheetControl);
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

        private void UpdateCBID(Calibration calibration)
        {
            try
            {
                if (qcbatchinfo.dtCalibration != null && qcbatchinfo.dtCalibration.Rows != null && qcbatchinfo.dtCalibration.Columns != null && qcbatchinfo.dtCalibration.Rows.Count > 0 && qcbatchinfo.dtCalibration.Columns.Count > 0)
                {
                    foreach (CalibrationInfo objCalibInfo in calibration.CalibrationInfos)
                    {
                        DataRow[] arrDR = qcbatchinfo.dtCalibration.Select("LEVELNO = " + objCalibInfo.LevelNo);
                        if (arrDR != null && arrDR.Count() > 0)
                        {
                            DataRow dr = arrDR.FirstOrDefault();
                            if (qcbatchinfo.dtCalibration.Columns.Contains("INTERCEPT") && dr["INTERCEPT"] != null && dr["INTERCEPT"].GetType() == typeof(string))
                            {
                                objCalibInfo.Intercept = dr["INTERCEPT"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("SLOPE") && dr["SLOPE"] != null && dr["SLOPE"].GetType() == typeof(string))
                            {
                                objCalibInfo.Slope = dr["SLOPE"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("RCAP2") && dr["RCAP2"] != null && dr["RCAP2"].GetType() == typeof(string))
                            {
                                objCalibInfo.RCAP2 = dr["RCAP2"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("CONCENTRATION") && dr["CONCENTRATION"] != null && dr["CONCENTRATION"].GetType() == typeof(string))
                            {
                                objCalibInfo.Conc = dr["CONCENTRATION"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("READING") && dr["READING"] != null && dr["READING"].GetType() == typeof(string))
                            {
                                objCalibInfo.Reading = dr["READING"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("VS") && dr["VS"] != null && dr["VS"].GetType() == typeof(string))
                            {
                                objCalibInfo.VS = dr["VS"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("VC1") && dr["VC1"] != null && dr["VC1"].GetType() == typeof(string))
                            {
                                objCalibInfo.VC1 = dr["VC1"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("VC2") && dr["VC2"] != null && dr["VC2"].GetType() == typeof(string))
                            {
                                objCalibInfo.VC2 = dr["VC2"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("VC") && dr["VC"] != null && dr["VC"].GetType() == typeof(string))
                            {
                                objCalibInfo.VC = dr["VC"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("ABSORBANCE") && dr["ABSORBANCE"] != null && dr["ABSORBANCE"].GetType() == typeof(string))
                            {
                                objCalibInfo.ABSORBANCE = dr["ABSORBANCE"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("F") && dr["F"] != null && dr["F"].GetType() == typeof(string))
                            {
                                objCalibInfo.F = dr["F"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("TITRANT1") && dr["TITRANT1"] != null && dr["TITRANT1"].GetType() == typeof(string))
                            {
                                objCalibInfo.Titrant1 = dr["TITRANT1"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("TITRANT2") && dr["TITRANT2"] != null && dr["TITRANT2"].GetType() == typeof(string))
                            {
                                objCalibInfo.Titrant2 = dr["TITRANT2"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("UNIT") && dr["UNIT"] != null && dr["UNIT"].GetType() == typeof(string))
                            {
                                objCalibInfo.Unit = dr["UNIT"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("ASSIGNEDPH") && dr["ASSIGNEDPH"] != null && dr["ASSIGNEDPH"].GetType() == typeof(string))
                            {
                                objCalibInfo.AssignedpH = dr["ASSIGNEDPH"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("READINGAFTER") && dr["READINGAFTER"] != null && dr["READINGAFTER"].GetType() == typeof(string))
                            {
                                objCalibInfo.ReadingAfter = dr["READINGAFTER"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("READINGBEFORE") && dr["READINGBEFORE"] != null && dr["READINGBEFORE"].GetType() == typeof(string))
                            {
                                objCalibInfo.ReadingBefore = dr["READINGBEFORE"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("REAGENTID") && dr["REAGENTID"] != null && dr["REAGENTID"].GetType() == typeof(string))
                            {
                                objCalibInfo.ReagentID = dr["REAGENTID"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("BUFFER") && dr["BUFFER"] != null && dr["BUFFER"].GetType() == typeof(string))
                            {
                                objCalibInfo.Buffer = dr["BUFFER"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("CURVELIMIT") && dr["CURVELIMIT"] != null && dr["CURVELIMIT"].GetType() == typeof(string))
                            {
                                objCalibInfo.CurveLimit = dr["CURVELIMIT"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("STDCONC") && dr["STDCONC"] != null && dr["STDCONC"].GetType() == typeof(string))
                            {
                                objCalibInfo.StdConc = dr["STDCONC"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("STDCONCVOLUSED") && dr["STDCONCVOLUSED"] != null && dr["STDCONCVOLUSED"].GetType() == typeof(string))
                            {
                                objCalibInfo.StdConcVolUsed = dr["STDCONCVOLUSED"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED1") && dr["USERDEFINED1"] != null && dr["USERDEFINED1"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined1 = dr["USERDEFINED1"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED2") && dr["USERDEFINED2"] != null && dr["USERDEFINED2"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined2 = dr["USERDEFINED2"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED3") && dr["USERDEFINED3"] != null && dr["USERDEFINED3"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined3 = dr["USERDEFINED3"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED4") && dr["USERDEFINED4"] != null && dr["USERDEFINED4"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined4 = dr["USERDEFINED4"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED5") && dr["USERDEFINED5"] != null && dr["USERDEFINED5"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined5 = dr["USERDEFINED5"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED6") && dr["USERDEFINED6"] != null && dr["USERDEFINED6"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined6 = dr["USERDEFINED6"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED7") && dr["USERDEFINED7"] != null && dr["USERDEFINED7"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined7 = dr["USERDEFINED7"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED8") && dr["USERDEFINED8"] != null && dr["USERDEFINED8"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined8 = dr["USERDEFINED8"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED9") && dr["USERDEFINED9"] != null && dr["USERDEFINED9"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined9 = dr["USERDEFINED9"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED10") && dr["USERDEFINED10"] != null && dr["USERDEFINED10"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined10 = dr["USERDEFINED10"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED11") && dr["USERDEFINED11"] != null && dr["USERDEFINED11"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined11 = dr["USERDEFINED11"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED12") && dr["USERDEFINED12"] != null && dr["USERDEFINED12"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined12 = dr["USERDEFINED12"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED13") && dr["USERDEFINED13"] != null && dr["USERDEFINED13"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined13 = dr["USERDEFINED13"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED14") && dr["USERDEFINED14"] != null && dr["USERDEFINED14"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined14 = dr["USERDEFINED14"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED15") && dr["USERDEFINED15"] != null && dr["USERDEFINED15"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined15 = dr["USERDEFINED15"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED16") && dr["USERDEFINED16"] != null && dr["USERDEFINED16"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined16 = dr["USERDEFINED16"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED17") && dr["USERDEFINED17"] != null && dr["USERDEFINED17"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined17 = dr["USERDEFINED17"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED18") && dr["USERDEFINED18"] != null && dr["USERDEFINED18"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined18 = dr["USERDEFINED18"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED19") && dr["USERDEFINED19"] != null && dr["USERDEFINED19"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined19 = dr["USERDEFINED19"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED20") && dr["USERDEFINED20"] != null && dr["USERDEFINED20"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined20 = dr["USERDEFINED20"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED21") && dr["USERDEFINED21"] != null && dr["USERDEFINED21"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined21 = dr["USERDEFINED21"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED22") && dr["USERDEFINED22"] != null && dr["USERDEFINED22"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined22 = dr["USERDEFINED22"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED23") && dr["USERDEFINED23"] != null && dr["USERDEFINED23"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined23 = dr["USERDEFINED23"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED24") && dr["USERDEFINED24"] != null && dr["USERDEFINED24"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined24 = dr["USERDEFINED24"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED25") && dr["USERDEFINED25"] != null && dr["USERDEFINED25"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined25 = dr["USERDEFINED25"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED26") && dr["USERDEFINED26"] != null && dr["USERDEFINED26"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined26 = dr["USERDEFINED26"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED27") && dr["USERDEFINED27"] != null && dr["USERDEFINED27"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined27 = dr["USERDEFINED27"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED28") && dr["USERDEFINED28"] != null && dr["USERDEFINED28"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined28 = dr["USERDEFINED28"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED29") && dr["USERDEFINED29"] != null && dr["USERDEFINED29"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined29 = dr["USERDEFINED29"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED30") && dr["USERDEFINED30"] != null && dr["USERDEFINED30"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined30 = dr["USERDEFINED30"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED31") && dr["USERDEFINED31"] != null && dr["USERDEFINED31"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined31 = dr["USERDEFINED31"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED32") && dr["USERDEFINED32"] != null && dr["USERDEFINED32"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined32 = dr["USERDEFINED32"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED33") && dr["USERDEFINED33"] != null && dr["USERDEFINED33"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined33 = dr["USERDEFINED33"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED34") && dr["USERDEFINED34"] != null && dr["USERDEFINED34"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined34 = dr["USERDEFINED34"].ToString();
                            }
                            if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED35") && dr["USERDEFINED35"] != null && dr["USERDEFINED35"].GetType() == typeof(string))
                            {
                                objCalibInfo.UserDefined35 = dr["USERDEFINED35"].ToString();
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

        private void Dc_AcceptingImportfile(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                ASPxSpreadsheetPropertyEditor aSPxSpreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                ItemsFileUpload itemsFile = (ItemsFileUpload)e.AcceptActionArgs.CurrentObject;
                if (itemsFile.InputFile != null)
                {
                    byte[] file = itemsFile.InputFile.Content;
                    string fileExtension = Path.GetExtension(itemsFile.InputFile.FileName);
                    if (fileExtension == ".xlsx")
                    {
                        wb.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                    }
                    else if (fileExtension == ".xls")
                    {
                        wb.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xls);
                    }

                    //DevExpress.Spreadsheet.WorksheetCollection worksheets = workbook.Worksheets;
                    if (qcbatchinfo.strMode != null && qcbatchinfo.strAB != null)
                    {
                        SpreadSheetEntry_AnalyticalBatch batch = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                        if (batch != null)
                        {
                            exportfile = true;
                            qcbatchinfo.strqcid = batch.AnalyticalBatchID;
                            qcbatchinfo.QCBatchOid = batch.Oid;
                            LoadfromAB(aSPxSpreadsheet.ASPxSpreadsheetControl, batch);
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
        private void Dc_AcceptingImport(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                ASPxSpreadsheetPropertyEditor aSPxSpreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                if (aSPxSpreadsheet != null)
                {
                    GenerateMailMerge(aSPxSpreadsheet.ASPxSpreadsheetControl, true);
                    IsRefresh = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_AcceptingReport(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs != null && e.AcceptActionArgs.CurrentObject != null)
                {
                    DataRow row = dtReporting.Select("Name = '" + ((SDMSReportPopupDC)(e.AcceptActionArgs.CurrentObject)).Report + "'").FirstOrDefault();
                    if (row != null)
                    {
                        ReportPreview(row);
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectreport"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
                IsRefresh = true;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ReportPreview(DataRow dr)
        {
            try
            {
                string strConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                string[] connectionstring = strConnectionString.Split(';');
                string strLDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                string strLDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                string strLDMSQLUserID = connectionstring[3].Split('=').GetValue(1).ToString();
                string strLDMSQLPassword = connectionstring[4].Split('=').GetValue(1).ToString();
                DynamicReportBusinessLayer.BLCommon.SetDBConnection(strLDMSQLServerName, strLDMSQLDatabaseName, strLDMSQLUserID, strLDMSQLPassword);
                DataSet dsSource = new DataSet("DynamicDataSource");
                DataTable dtSource = new DataTable("SampleInfo");
                ////DataTable dtCalib = new DataTable("dtCalib");
                dtSource = dtDetailDataNew.Copy();
                ////dtCalib = qcbatchinfo.dtCalibration.Copy();
                dsSource.Tables.Add(dtSource);
                ////dsSource.Tables.Add(dtCalib);
                MemoryStream oMemoryStream = new MemoryStream();
                StreamWriter oStreamWriter = new StreamWriter(oMemoryStream, Encoding.UTF8);
                oStreamWriter.Write(dr["Layout"].ToString().ToCharArray());
                oStreamWriter.Flush();
                oMemoryStream.Seek(0, SeekOrigin.Begin);
                xrReport = new XtraReport();
                xrReport = XtraReport.FromStream(oMemoryStream, true);
                xrReport.DataSource = dsSource;
                xrReport.DataMember = "RawDataTableDataSource";
                ////ReportDesignGenerator.GetSetColumnsData(Convert.ToInt32(dr["uqReportDesignID"]));
                string strReportName = dr["Name"].ToString();
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".pdf");
                xrReport.ExportToPdf(strExportedPath);
                string[] path = strExportedPath.Split('\\');
                int arrcount = path.Count();
                int sc = arrcount - 2;
                string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Instance_SaveAuditTrailData(object sender, SaveAuditTrailDataEventArgs e)
        {
            e.Handled = true;
        }

        private void getstatus(SpreadSheetEntry spreadSheet, DefaultSetting defaultSetting)
        {
            try
            {
                if (spreadSheet.uqSampleParameterID != null)
                {
                    if (defaultSetting.REValidate == EnumRELevelSetup.Yes)
                    {
                        spreadSheet.uqSampleParameterID.Status = Samplestatus.PendingValidation;
                        spreadSheet.uqSampleParameterID.OSSync = true;
                    }
                    else if (defaultSetting.REValidate == EnumRELevelSetup.No && defaultSetting.REApprove == EnumRELevelSetup.Yes)
                    {
                        spreadSheet.uqSampleParameterID.Status = Samplestatus.PendingApproval;
                        spreadSheet.uqSampleParameterID.ValidatedDate = DateTime.Now;
                        spreadSheet.uqSampleParameterID.OSSync = true;
                        spreadSheet.uqSampleParameterID.ValidatedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    }
                    else if (defaultSetting.REValidate == EnumRELevelSetup.No && defaultSetting.REApprove == EnumRELevelSetup.No)
                    {
                        spreadSheet.uqSampleParameterID.Status = Samplestatus.PendingReporting;
                        spreadSheet.uqSampleParameterID.ValidatedDate = DateTime.Now;
                        spreadSheet.uqSampleParameterID.OSSync = true;
                        spreadSheet.uqSampleParameterID.ValidatedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        spreadSheet.uqSampleParameterID.AnalyzedDate = DateTime.Now;
                        spreadSheet.uqSampleParameterID.AnalyzedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
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
                string strreason = ((SDMSRollback)e.AcceptActionArgs.CurrentObject).PopupRollBackReason;
                DataRow[] ExportColumns = qcbatchinfo.dtselectedsamplefields.Select("[ExportSample] <> '' and [ExportSample] is not null");
                if (!string.IsNullOrEmpty(strreason))
                {
                    Popupos.RemoveFromModifiedObjects(e.AcceptActionArgs.CurrentObject);
                    Popupos.Dispose();
                    SpreadSheetEntry_AnalyticalBatch ABspreadSheet = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                    if (ABspreadSheet != null)
                    {
                        IList<SpreadSheetEntry> spreadSheets = os.GetObjects<SpreadSheetEntry>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", ABspreadSheet.Oid));
                        IList<SpreadsheetEntry_MoldResults> moldResults = os.GetObjects<SpreadsheetEntry_MoldResults>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", ABspreadSheet.Oid));
                        int DOCCount = 0;
                        foreach (SpreadSheetEntry spreadSheet in spreadSheets)
                        {
                            SDMSRollback objrollback = os.CreateObject<SDMSRollback>();
                            //if (spreadSheet.uqQCBatchID != null)
                            //{
                            //    objrollback.QCBatchID = spreadSheet.uqQCBatchID.QCBatchID;
                            //}
                            objrollback.QCType = spreadSheet.uqQCTypeID.QCTypeName;
                            if (spreadSheet.uqSampleParameterID != null)
                            {
                                objrollback.SampleLoginID = spreadSheet.uqSampleParameterID.Samplelogin;
                            }
                            objrollback.TestMethodID = spreadSheet.UQTESTPARAMETERID.TestMethod;
                            if (spreadSheet.uqSampleParameterID != null)
                            {
                                objrollback.PreviousStatus = spreadSheet.uqSampleParameterID.Status.ToString();
                            }
                            objrollback.CurrentStatus = Samplestatus.PendingEntry.ToString();
                            objrollback.RollbackBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objrollback.RollbackDate = DateTime.Now;
                            objrollback.ResultEnteredBy = spreadSheet.EnteredBy;
                            objrollback.ResultEnteredDate = (DateTime)spreadSheet.EnteredDate;
                            objrollback.SampleParameterID = spreadSheet.uqSampleParameterID;
                            objrollback.RollBackReason = strreason;
                            objrollback.AnalyticalBatchID = qcbatchinfo.strAB;
                            if (spreadSheet.IsExported)
                            {
                                Deleteresultinsampleparameter(spreadSheet, ExportColumns);
                                if (spreadSheet.uqSampleParameterID != null)
                                {
                                    spreadSheet.uqSampleParameterID.ApprovedBy = null;
                                    spreadSheet.uqSampleParameterID.ApprovedDate = null;
                                    spreadSheet.uqSampleParameterID.ValidatedBy = null;
                                    spreadSheet.uqSampleParameterID.ValidatedDate = null;
                                }
                            }
                            spreadSheet.IsComplete = false;
                            spreadSheet.IsExported = false;
                            if (spreadSheet.uqSampleParameterID != null)
                            {
                                spreadSheet.uqSampleParameterID.Status = Samplestatus.PendingEntry;
                                if (spreadSheet.uqSampleParameterID.DOCDetail != null && spreadSheet.uqSampleParameterID.DOCDetail.DOC != null)
                                {
                                    DOC objDOC = os.GetObjectByKey<DOC>(spreadSheet.uqSampleParameterID.DOCDetail.DOC.Oid);
                                    if (objDOC!=null)
                                    {
                                        if (spreadSheet.uqQCTypeID != null && spreadSheet.uqQCTypeID.QCTypeName == "LCS")
                                        {
                                            List<DOCDetails> lstDOC = os.GetObjects<DOCDetails>(CriteriaOperator.Parse("[DOC]=?", objDOC)).ToList();
                                            if (lstDOC.Count > 0)
                                            {
                                                List<SampleParameter> lstsamplepar = os.GetObjects<SampleParameter>(new InOperator("DOCDetail", lstDOC)).ToList();
                                                lstsamplepar.ForEach(i => i.DOCDetail = null);
                                                lstsamplepar.ForEach(i => os.Delete(i.DOCDetail));
                                                if (objDOC != null/* && lstsamplepar.Count == 1*/)
                                                {
                                                    os.Delete(objDOC);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            spreadSheet.uqSampleParameterID.DOCDetail = null;
                                            List<DOCDetails> lstDOC = os.GetObjects<DOCDetails>(CriteriaOperator.Parse("[DOC]=?", objDOC)).ToList();
                                            if (lstDOC.Count > 0)
                                            {
                                                List<SampleParameter> lstsamplepar = os.GetObjects<SampleParameter>(new InOperator("DOCDetail", lstDOC)).ToList();
                                                if(DOCCount==0)
                                                {
                                                    DOCCount = lstsamplepar.Count;
                                                }
                                                else
                                                {
                                                    DOCCount -= 1;
                                                }
                                                if (objDOC != null && DOCCount == 1)
                                                {
                                                    os.Delete(objDOC);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if(spreadSheet.uqSampleParameterID.Samplelogin!=null&& spreadSheet.uqSampleParameterID.Samplelogin.JobID!=null)
                                {
                                    DOC objDOC = os.FindObject<DOC>(CriteriaOperator.Parse("[JobID]=?", spreadSheet.uqSampleParameterID.Samplelogin.JobID));
                                    if (objDOC!=null)
                                    {
                                        List<DOCDetails> lstDOC = os.GetObjects<DOCDetails>(CriteriaOperator.Parse("[DOC]=?", objDOC)).ToList();
                                        if (lstDOC.Count > 0)
                                        {
                                            List<SampleParameter> lstsamplepar = os.GetObjects<SampleParameter>(new InOperator("DOCDetail", lstDOC)).ToList();
                                            if (objDOC != null && lstsamplepar.Count == 1)
                                            {
                                                os.Delete(objDOC);
                                            }
                                        } 
                                        else
                                        {
                                            os.Delete(objDOC);
                                        }
                                    }
                                }
                            }
                            spreadSheet.ReviewedBy = null;
                            spreadSheet.ReviewedDate = null;
                            spreadSheet.VerifiedBy = null;
                            spreadSheet.VerifiedDate = null;
                            Notes notes = os.FindObject<Notes>(CriteriaOperator.Parse("[SourceID] =? AND [NoteSource] = 'SDMS' ", ABspreadSheet.AnalyticalBatchID));
                            if (notes != null)
                            {
                                os.Delete(notes);
                            }
                            
                        }
                        ABspreadSheet.ReviewedBy = null;
                        ABspreadSheet.ReviewedDate = null;
                        ABspreadSheet.VerifiedBy = null;
                        ABspreadSheet.VerifiedDate = null;
                        ABspreadSheet.Status = 1;
                        IsRefresh = true;
                        //Rollback MoldResults
                        if (moldResults != null)
                        {
                            foreach (SpreadsheetEntry_MoldResults moldResult in moldResults)
                            {
                                moldResult.ReviewedBy = null;
                                moldResult.ReviewedDate = null;
                                moldResult.VerifiedBy = null;
                                moldResult.VerifiedDate = null;
                            }
                        }
                        os.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        ASPxSpreadsheetPropertyEditor aSPxSpreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                        if (aSPxSpreadsheet != null)
                        {
                            clearcontrols(aSPxSpreadsheet.ASPxSpreadsheetControl);
                        }
                        //Frame.GetController<Module.Controllers.ICM.NavigationRefreshController>().Navigationrefresh();
                        ShowNavigationItemController ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
                        if (ShowNavigationController != null && ShowNavigationController.ShowNavigationItemAction != null)
                        {
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
                                                lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingReporting' And Not IsNullOrEmpty([Samplelogin.JobID.JobID])");
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

        private void ImportDatasource(int inttemplateid)
        {
            try
            {
                //DataTable dtImportDatasource = new DataTable();
                //string strJobID = string.Join(",", qcbatchinfo.dtsample.AsEnumerable().Select(s => s.Field<string>("JOBID")).Distinct().ToArray());
                //dtImportDatasource = GetImportDataSource(strJobID, inttemplateid);
                //foreach (DataRow drImport in drImportDatasource)
                //{
                //    foreach (DataRow drImportResult in dtImportDatasource.Rows)
                //    {
                //        DataRow[] drrSample = qcbatchinfo.dtsample.Select("SAMPLEID = '" + drImportResult["SampleID"].ToString() + "' ");
                //        foreach (DataRow drSample in drrSample)
                //        {
                //            drSample[drImportResult["FieldName"].ToString()] = drImportResult[drImport["ImportField"].ToString()];
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

        private bool ValidateGridResults()
        {
            try
            {
                DataRow[] dr = qcbatchinfo.dtsample.Select("Result is  NUll");
                if (dr.Length > 0)
                {
                    DataTable dt = dr.CopyToDataTable();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ResultNull"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    return false;
                }
                else
                {
                    string strisReport = string.Empty;
                    if (qcbatchinfo.dtDataParsing != null)
                    {
                        DataRow[] drrSelect = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable'");
                        strisReport = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "IsReport").Select(r => r["Position"].ToString()).SingleOrDefault();

                    }
                    sdmsinfo.lstnonexpqcbatcseqoid = new List<Guid>();
                    sdmsinfo.lstqcbatcseqoid = new List<Guid>();
                    foreach (DataRow drImportResult in qcbatchinfo.dtsample.Rows)
                    {
                        string strIsRep = string.Empty;
                        if (!string.IsNullOrWhiteSpace(strisReport))
                        {
                            strIsRep = drImportResult["IsReport"].ToString();
                        }

                        string strsyscode = drImportResult["SysSampleCode"].ToString();
                        string strqctype = drImportResult["QCType"].ToString();
                        if (strqctype == "Sample")
                        {
                            if (strIsRep == "True" && !string.IsNullOrEmpty(strIsRep) && !string.IsNullOrEmpty(strsyscode))
                            {
                                IObjectSpace osqcseq = Application.CreateObjectSpace(typeof(QCBatchSequence));
                                QCBatchSequence objqcseq = os.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[SYSSamplecode]= ?", strsyscode));
                                if (objqcseq != null)
                                {
                                    objqcseq.IsReport = true;
                                    sdmsinfo.lstqcbatcseqoid.Add(objqcseq.Oid);
                                    os.CommitChanges();
                                }
                            }
                            else if (strIsRep == "False" && !string.IsNullOrEmpty(strIsRep) && !string.IsNullOrEmpty(strsyscode))
                            {
                                IObjectSpace osqcseq = Application.CreateObjectSpace(typeof(QCBatchSequence));
                                QCBatchSequence objqcseq = os.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[SYSSamplecode]= ?", strsyscode));
                                if (objqcseq != null)
                                {
                                    objqcseq.IsReport = false;
                                    sdmsinfo.lstnonexpqcbatcseqoid.Add(objqcseq.Oid);
                                    os.CommitChanges();
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return false;
            }
        }

        ////private bool ValidateGridIsReport()
        ////{
        ////    try
        ////    {
        ////        DataRow[] dr = qcbatchinfo.dtsample.Select("Result is  NUll");
        ////        if (dr.Length > 0)
        ////        {
        ////            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ResultNull"), InformationType.Error, timer.Seconds, InformationPosition.Top);
        ////            return false;
        ////        }
        ////        else
        ////        {
        ////            string strisReport = string.Empty;
        ////            if (qcbatchinfo.dtDataParsing != null)
        ////            {
        ////                DataRow[] drrSelect = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable'");
        ////                strisReport = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "IsReport").Select(r => r["Position"].ToString()).SingleOrDefault();

        ////            }
        ////            sdmsinfo.lstqcbatcseqoid = new List<Guid>();
        ////            foreach (DataRow drImportResult in qcbatchinfo.dtsample.Rows)
        ////            {
        ////                string strIsRep = string.Empty;
        ////                if (!string.IsNullOrWhiteSpace(strisReport))
        ////                {
        ////                     strIsRep = drImportResult["IsReport"].ToString();
        ////                    string strsyscode = drImportResult["SysSampleCode"].ToString();
        ////                    string strqctype = drImportResult["QCType"].ToString();
        ////                    if (strqctype == "Sample")
        ////                    {
        ////                        if (strIsRep != "False" && !string.IsNullOrEmpty(strIsRep) && !string.IsNullOrEmpty(strsyscode))
        ////                        {
        ////                            IObjectSpace osqcseq = Application.CreateObjectSpace(typeof(QCBatchSequence));
        ////                            QCBatchSequence objqcseq = os.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[SYSSamplecode]= ?", strsyscode));
        ////                            if (objqcseq != null)
        ////                            {
        ////                                objqcseq.IsReport = true;
        ////                                sdmsinfo.lstqcbatcseqoid.Add(objqcseq.Oid);
        ////                                os.CommitChanges();
        ////                            }
        ////                        }
        ////                        else if (!string.IsNullOrEmpty(strsyscode))
        ////                        {
        ////                            IObjectSpace osqcseq = Application.CreateObjectSpace(typeof(QCBatchSequence));
        ////                            QCBatchSequence objqcseq = osqcseq.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[SYSSamplecode]= ?", strsyscode));
        ////                            if (objqcseq != null && objqcseq.IsReport == true)
        ////                            {
        ////                                sdmsinfo.lstqcbatcseqoid.Add(objqcseq.Oid);
        ////                            }
        ////                        }
        ////                    }
        ////                }


        ////            }
        ////            return true;
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        ////        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        ////        return false;
        ////    }
        ////}

        private DataTable CreateTDT()
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("Test");
                table.Columns.Add("Matrix");
                table.Columns.Add("Method");
                table.Columns.Add("TemplateName");
                table.Columns.Add("Sx");
                table.Columns.Add("TestOid");
                table.Columns.Add("TemplateID");
                return table;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        private bool Inserttosampleparameter(SpreadSheetEntry spreadSheet, DataRow[] ExportColumns)
        {
            try
            {
                if (spreadSheet.uqSampleParameterID != null && spreadSheet.RunNo == 1)
                {
                    if (spreadSheet.Remark != null)
                    {
                        Notes notes = os.FindObject<Notes>(CriteriaOperator.Parse("[SourceID] =? AND [NoteSource] = 'SDMS' ", spreadSheet.uqAnalyticalBatchID.AnalyticalBatchID));
                        if (notes == null)
                        {
                            notes = os.CreateObject<Notes>();
                            if (notes != null)
                            {
                                notes.SourceID = spreadSheet.uqAnalyticalBatchID.AnalyticalBatchID;
                                notes.NoteSource = "SDMS";
                                Samplecheckin checkin = os.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID] = ?", spreadSheet.uqAnalyticalBatchID.Jobid));
                                if (checkin != null)
                                {
                                    if (!string.IsNullOrEmpty(checkin.SampleMatries))
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        foreach (string strMatrix in checkin.SampleMatries.Split(';'))
                                        {
                                            VisualMatrix objSM = os.GetObjectByKey<VisualMatrix>(new Guid(strMatrix));
                                            if (sb.Length > 0)
                                            {
                                                sb.Append(";"); // Add semicolon before appending the next name
                                            }
                                            sb.Append(objSM.VisualMatrixName);
                                        }
                                        notes.NameSource = sb.ToString();
                                    }
                                }
                                notes.Text = spreadSheet.Remark;
                                notes.Samplecheckin = os.GetObjectByKey<Samplecheckin>(checkin.Oid);
                            }
                        }
                    }
                    for (int i = 0; i < ExportColumns.Length; i++)
                    {
                        var sproperty = spreadSheet.uqSampleParameterID.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name == ExportColumns[i]["ExportSample"].ToString()).ToList();
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


        private bool Deleteresultinsampleparameter(SpreadSheetEntry spreadSheet, DataRow[] ExportColumns)
        {
            try
            {
                if (spreadSheet.uqSampleParameterID != null && spreadSheet.RunNo == 1)
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

        private DataTable CreateABDT()
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("JobID");
                table.Columns.Add("Test");
                table.Columns.Add("Matrix");
                table.Columns.Add("Method");
                table.Columns.Add("Template");
                //table.Columns.Add("ProjectID");
                //table.Columns.Add("ProjectName");
                //table.Columns.Add("Client");
                table.Columns.Add("ABID");
                table.Columns.Add("Status");
                return table;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        private DataTable CreateCBDT()
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("uqCalibrationID", typeof(int));
                table.Columns.Add("CalibrationID", typeof(String));
                table.Columns.Add("CalibratedDate", typeof(DateTime));
                table.Columns.Add("CalibratedBy", typeof(String));
                table.Columns.Add("ISABIDLink", typeof(String));
                return table;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View is ListView)
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        if (View.Id == "SDMSDCAB_ListView")
                        {
                            //gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridListEditor.Grid.Load += Grid_Load;
                            gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                            gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                            gridListEditor.Grid.Settings.VerticalScrollableHeight = 600;
                            Frame.GetController<NewObjectViewController>().NewObjectAction.Active.SetItemValue("NewObjectAction", false);
                            Frame.GetController<RefreshController>().RefreshAction.Active.SetItemValue("RefreshAction", false);
                            Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("DeleteAction", false);
                        }
                        else if (View.Id == "SDMSReportPopupDC_ListView")
                        {
                            gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                        }
                        else if (View.Id == "SDMSDCImport_ListView")
                        {
                            gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                            Frame.GetController<NewObjectViewController>().NewObjectAction.Active.SetItemValue("NewObjectAction", false);
                            Frame.GetController<RefreshController>().RefreshAction.Active.SetItemValue("RefreshAction", false);
                            Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("DeleteAction", false);

                        }
                        else if (View.Id == "SDMSUploadImage_ListView")
                        {
                            gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ABID.AnalyticalBatchID] = ?", qcbatchinfo.strAB);
                        }
                    }
                }
                else
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Active.SetItemValue("NewObjectAction", false);
                    Frame.GetController<RefreshController>().RefreshAction.Active.SetItemValue("RefreshAction", false);
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("DeleteAction", false);
                    //if (View.Id == "SDMSDCImport_DetailView")
                    //{
                    //    AspxComboboxPropertyEditor propertyEditor = ((DetailView)View).FindItem("InstFileType") as AspxComboboxPropertyEditor;
                    //    if (propertyEditor != null && propertyEditor.Editor != null)
                    //    {
                    //        ASPxComboBox comboBox = (ASPxComboBox)propertyEditor.Editor;
                    //        if (comboBox != null)
                    //        {
                    //            foreach (DataRow row in qcbatchinfo.dtDataTransfer.Rows)
                    //            {
                    //                comboBox.Items.Add(row["Name"].ToString());
                    //            }
                    //            comboBox.Callback += comboBox_Callback;
                    //            comboBox.JSProperties["cpselText"] = qcbatchinfo.strDataTransfer;
                    //            comboBox.ClientSideEvents.Init = @"function(s,e) { s.SetText(s.cpselText); }";
                    //            comboBox.ClientSideEvents.SelectedIndexChanged = @"function(s,e) { s.PerformCallback(s.lastSuccessText); e.processOnServer = true; }";
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

        //private void comboBox_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    try
        //    {
        //        ASPxComboBox comboBox = (ASPxComboBox)sender;
        //        comboBox.JSProperties["cpselText"] = qcbatchinfo.strDataTransfer = e.Parameter;
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                e.Cell.Attributes.Add("onclick", "event.stopPropagation();");
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
                ASPxGridView grid = sender as ASPxGridView;
                griddataload(grid);
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
                //foreach (GridViewDataColumn column in grid.Columns.Cast<GridViewDataColumn>().ToList())
                //    foreach (GridViewDataActionColumn column in grid.Columns.Cast<GridViewDataActionColumn>().ToList())
                //    {
                //    grid.Columns.Remove(column);

                //}

                if (qcbatchinfo.dtsample != null && qcbatchinfo.dtselectedsamplefields != null && qcbatchinfo.dtsample.Rows.Count > 0 && qcbatchinfo.dtselectedsamplefields.Rows.Count > 0)
                {
                    if (grid != null && grid.Columns.Count > 0)
                    {
                        foreach (WebColumnBase column in grid.VisibleColumns)
                        {
                            column.Visible = false;
                        }
                    }
                    foreach (DataRow drSelected in qcbatchinfo.dtselectedsamplefields.Rows)
                    {
                        if (qcbatchinfo.dtsample.Columns.Contains(drSelected["FieldName"].ToString()))
                        {
                            GridViewDataColumn data_column = new GridViewDataTextColumn();
                            data_column.FieldName = drSelected["FieldName"].ToString().ToUpper();
                            if (CurrentLanguage == "En")
                            {
                                if (!string.IsNullOrEmpty(drSelected["Caption_EN"].ToString()))
                                    //data_column.Caption = drSelected["Caption_EN"].ToString().ToUpper();
                                    data_column.Caption = drSelected["Caption_EN"].ToString();
                                //if (data_column.Caption == "Status")
                                //{
                                //    continue;
                                //}
                                //else if (data_column.Caption == "Samplestatus")
                                //{
                                //    data_column.Caption = "Status";
                                //}
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(drSelected["Caption_CN"].ToString()))
                                    //data_column.Caption = drSelected["Caption_CN"].ToString().ToUpper();
                                    data_column.Caption = drSelected["Caption_CN"].ToString();
                                //if (data_column.Caption == "状态")
                                //{
                                //    continue;
                                //}
                                //else if (data_column.Caption == "Samplestatus")
                                //{
                                //    data_column.Caption = "Status";
                                //}
                            }
                            if (!string.IsNullOrEmpty(drSelected["Width"].ToString()))
                                data_column.Width = Convert.ToInt16(drSelected["Width"]);
                            if (!string.IsNullOrEmpty(drSelected["Visible"].ToString()))
                                data_column.Visible = Convert.ToBoolean(drSelected["Visible"]);
                            if (!string.IsNullOrEmpty(drSelected["Sort"].ToString()) && data_column.Visible == true)
                                data_column.VisibleIndex = Convert.ToInt16(drSelected["Sort"]) + 1;
                            data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                            data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                            data_column.ShowInCustomizationForm = false;
                            data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                            grid.Columns.Add(data_column);
                        }
                    }
                    //foreach (DataColumn column in qcbatchinfo.dtsample.Columns)
                    //{
                    //    if (column.Caption.ToUpper() == "STATUS")
                    //    {
                    //        qcbatchinfo.dtsample.Columns.Remove(column);
                    //        break;
                    //    }
                    //}
                    //foreach (DataColumn column in qcbatchinfo.dtsample.Columns)
                    //{
                    //    if (column.Caption.ToUpper() == "SAMPLESTATUS")
                    //    {
                    //        column.ColumnName = "STATUS";
                    //        column.Caption = "STATUS";
                    //        GridViewDataColumn data_column = new GridViewDataTextColumn();
                    //        data_column.Caption = "STATUS";
                    //        data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    //        data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    //        data_column.ShowInCustomizationForm = false;
                    //        data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                    //        grid.Columns.Add(data_column);
                    //    }
                    //}
                    //qcbatchinfo.dtsample.AcceptChanges();
                    grid.KeyFieldName = "SYSSAMPLECODE";
                    grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                    grid.ClientSideEvents.Init = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');
                        if(nav != null && sep != null) 
                        {
                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                           s.SetWidth(totusablescr - 125); 
                        }                                            
                    }";
                    //foreach (DataRow dr in qcbatchinfo.dtsample.Rows)
                    //{
                    //    if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 0)
                    //    {
                    //        dr["Status"] = "Pending Entry";// Samplestatus.PendingEntry;
                    //    }
                    //    else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 1)
                    //    {
                    //        dr["Status"] = Samplestatus.PendingValidation;
                    //    }
                    //    else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 2)
                    //    {
                    //        dr["Status"] = Samplestatus.PendingApproval;
                    //    }
                    //    else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 3)
                    //    {
                    //        dr["Status"] = Samplestatus.PendingReporting;
                    //    }
                    //    else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 4)
                    //    {
                    //        dr["Status"] = Samplestatus.PendingReportValidation;
                    //    }
                    //    else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 5)
                    //    {
                    //        dr["Status"] = Samplestatus.PendingReportApproval;
                    //    }
                    //    else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 6)
                    //    {
                    //        dr["Status"] = Samplestatus.Approved;
                    //    }
                    //    else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 7)
                    //    {
                    //        dr["Status"] = Samplestatus.PendingReview;
                    //    }
                    //    else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 8)
                    //    {
                    //        dr["Status"] = Samplestatus.PendingVerify;
                    //    }
                    //    else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 9)
                    //    {
                    //        dr["Status"] = Samplestatus.ReportApproved;
                    //    }
                    //    else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 10)
                    //    {
                    //        dr["Status"] = Samplestatus.Reported;
                    //    }
                    //    else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 11)
                    //    {
                    //        dr["Status"] = Samplestatus.SuboutPendingValidation;
                    //    }
                    //    else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 12)
                    //    {
                    //        dr["Status"] = Samplestatus.SuboutPendingApproval;
                    //    }
                    //}
                    grid.DataSource = qcbatchinfo.dtsample;
                    grid.DataBind();
                    if (grid.Columns["QCTYPE"] != null)
                    {
                        grid.VisibleColumns["QCTYPE"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (grid.Columns["SAMPLEID"] != null)
                    {
                        grid.Columns["SAMPLEID"].Width = 125;
                    }
                    if (grid.Columns["SYSSAMPLECODE"] != null)
                    {
                        grid.Columns["SYSSAMPLECODE"].Width = 145;
                    }
                    if (grid.Columns["QCBATCHID"] != null)
                    {
                        grid.Columns["QCBATCHID"].Width = 145;
                    }
                    foreach (WebColumnBase column in grid.VisibleColumns)
                    {
                        if (column.Caption.EndsWith("Date"))
                        {
                            grid.DataColumns[column.ToString()].PropertiesEdit.DisplayFormatString = "MM/dd/yyyy HH:mm";
                        }
                    }
                }
                else
                {
                    DataTable dttemp = new DataTable();
                    dttemp.Columns.Add("NoData");
                    GridViewDataColumn data_column = new GridViewDataTextColumn();
                    data_column.FieldName = "NoData";
                    data_column.Caption = " ";
                    data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    data_column.ShowInCustomizationForm = false;
                    data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.False;
                    grid.Columns.Add(data_column);
                    grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Hidden;
                    grid.ClientSideEvents.Init = null;
                    grid.DataSource = dttemp;
                    grid.DataBind();
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
                ((WebApplication)Application).ObjectSpaceCreated -= SpreadsheetEntryController_ObjectSpaceCreated;
                AuditTrailService.Instance.SaveAuditTrailData -= Instance_SaveAuditTrailData;
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                os.Reloaded -= Os_Reloaded;
                os.Dispose();
                if (View.Id == "SDMSDCSpreadsheet_DetailView")
                {
                    ASPxSpreadsheetPropertyEditor Spreadsheet = ((DetailView)View).FindItem("Data") as ASPxSpreadsheetPropertyEditor;
                    if (Spreadsheet != null)
                    {
                        Spreadsheet.ControlCreated -= Spreadsheet_ControlCreated;
                        qcbatchinfo.qcstatus = 0;
                        qcbatchinfo.templateid = 0;
                        qcbatchinfo.Isedit = false;
                        qcbatchinfo.strqcid = null;
                        qcbatchinfo.QCBatchOid = null;
                        qcbatchinfo.strMode = null;
                        qcbatchinfo.strTest = null;
                        qcbatchinfo.strAB = null;
                        qcbatchinfo.strCB = null;
                        qcbatchinfo.IsSheetloaded = false;
                        qcbatchinfo.Isedit = false;
                        qcbatchinfo.OidTestMethod = null;
                        qcbatchinfo.dtsample = null;
                        qcbatchinfo.dtDataParsing = null;
                        qcbatchinfo.dtCalibration = null;
                        qcbatchinfo.dtTestdatasource = null;
                        qcbatchinfo.dtselectedsamplefields = null;
                    }
                    //IObjectSpace qcos = Application.CreateObjectSpace(typeof(SpreadSheetEntry_AnalyticalBatch));
                    //SpreadSheetEntry_AnalyticalBatch objQCBatch = qcos.GetObjectByKey<SpreadSheetEntry_AnalyticalBatch>(qcbatchinfo.AnalyticalQCBatchOid);
                    //if (objQCBatch != null && !qcos.IsDeletedObject(objQCBatch) && !string.IsNullOrEmpty(objQCBatch.AnalyticalBatchID) && objQCBatch.AnalyticalBatchID.StartsWith("AB"))
                    //{
                    //    IList<SampleParameter> lstSampleParameter = qcos.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail.Oid] = ?", objQCBatch.Oid));
                    //    for (int i = 0; i < lstSampleParameter.Count; i++)
                    //    {
                    //        if (lstSampleParameter[i] != null && lstSampleParameter[i].UQABID == null)
                    //        {
                    //            lstSampleParameter[i].QCBatchID = null;
                    //        }
                    //    }
                    //    //if (objQCBatch.ABID == null)
                    //    //{
                    //    //    SpreadSheetEntry_AnalyticalBatch AnalyticalBatch = qcos.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[qcbatchID.Oid]=?", objQCBatch.Oid));
                    //    //    if (AnalyticalBatch == null)
                    //    //    {
                    //    //        IList<QCBatchSequence> lstQCBatchSequence = qcos.GetObjects<QCBatchSequence>(CriteriaOperator.Parse("[qcseqdetail.Oid] = ?", objQCBatch.Oid));
                    //    //        if (lstQCBatchSequence != null && lstQCBatchSequence.Count > 0)
                    //    //        {
                    //    //            qcos.Delete(lstQCBatchSequence);
                    //    //        }
                    //    //    }
                    //    //}
                    //    //IList<Modules.BusinessObjects.QC.QCBatchSequence> lstQC = qcos.GetObjects<Modules.BusinessObjects.QC.QCBatchSequence>(CriteriaOperator.Parse("[qcseqdetail.Oid] = ?", objQCBatch.Oid));
                    //    //if (lstQC != null && lstQC.Count > 0)
                    //    //{
                    //    //    qcos.Delete(lstQC);
                    //    //    //List<Modules.BusinessObjects.QC.QCBatchSequence> lstQC = ilstQC.ToList();
                    //    //    //for (int i = 0; i < lstQC.Count; i++)
                    //    //    //{
                    //    //    //    if (lstQC[i] != null)
                    //    //    //    {
                    //    //    //        qcos.Delete(lstQC[i]);
                    //    //    //    }
                    //    //    //}
                    //    //}
                    //    qcos.Delete(objQCBatch);
                    //    qcos.CommitChanges();
                    //    qcbatchinfo.QCBatchOid = null;
                    //    qcbatchinfo.AnalyticalQCBatchOid = null;
                    //}
                }
                else if (View.Id == "SDMSDCImport_DetailView")
                {
                    FilePropertyEditor.ControlCreated -= FilePropertyEditor_ControlCreated;
                }
                //else if (View.Id == "SDMS")
                //{
                //    IObjectSpace qcos = Application.CreateObjectSpace(typeof(SpreadSheetEntry_AnalyticalBatch));
                //    SpreadSheetEntry_AnalyticalBatch objQCBatch = qcos.GetObjectByKey<SpreadSheetEntry_AnalyticalBatch>(qcbatchinfo.AnalyticalQCBatchOid);
                //    if (objQCBatch != null && !qcos.IsDeletedObject(objQCBatch) && !string.IsNullOrEmpty(objQCBatch.AnalyticalBatchID) && objQCBatch.AnalyticalBatchID.StartsWith("AB"))
                //    {
                //        IList<SampleParameter> lstSampleParameter = qcos.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail.Oid] = ?", objQCBatch.Oid));
                //        for (int i = 0; i < lstSampleParameter.Count; i++)
                //        {
                //            if (lstSampleParameter[i] != null && lstSampleParameter[i].UQABID == null)
                //            {
                //                lstSampleParameter[i].QCBatchID = null;
                //            }
                //        }
                //        //if (objQCBatch.ABID == null)
                //        //{
                //        //    IList<QCBatchSequence> lstQCBatchSequence = qcos.GetObjects<QCBatchSequence>(CriteriaOperator.Parse("[qcseqdetail.Oid] = ?", objQCBatch.Oid));
                //        //    if (lstQCBatchSequence != null && lstQCBatchSequence.Count > 0)
                //        //    {
                //        //        qcos.Delete(lstQCBatchSequence);
                //        //    }
                //        //}
                //        //IList<Modules.BusinessObjects.QC.QCBatchSequence> lstQC = qcos.GetObjects<Modules.BusinessObjects.QC.QCBatchSequence>(CriteriaOperator.Parse("[qcseqdetail.Oid] = ?", objQCBatch.Oid));
                //        //if (lstQC != null && lstQC.Count > 0)
                //        //{
                //        //    qcos.Delete(lstQC);
                //        //    //List<Modules.BusinessObjects.QC.QCBatchSequence> lstQC = ilstQC.ToList();
                //        //    //for (int i = 0; i < lstQC.Count; i++)
                //        //    //{
                //        //    //    if (lstQC[i] != null)
                //        //    //    {
                //        //    //        qcos.Delete(lstQC[i]);
                //        //    //    }
                //        //    //}
                //        //}
                //        qcos.Delete(objQCBatch);
                //        qcos.CommitChanges();
                //        qcbatchinfo.QCBatchOid = null;
                //        qcbatchinfo.AnalyticalQCBatchOid = null;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private bool BindSampleToGrid(ASPxSpreadsheetPropertyEditor aSPxSpreadsheet)
        {
            try
            {
                if (qcbatchinfo.dtsample != null && qcbatchinfo.dtDataParsing != null)
                {
                    DataRow[] drrSample = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable' AND Write = TRUE AND Continuous= TRUE");
                    DataRow[] drrSampleSingle = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable' AND Write = TRUE AND Continuous= FALSE");
                    if (drrSample.Length == 0) return false;
                    DataRow[] drrSelect = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable'");
                    string strSampleID = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "SampleID").Select(r => r["Position"].ToString()).SingleOrDefault();
                    string strRunNo = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "RunNo").Select(r => r["Position"].ToString()).SingleOrDefault();
                    string strParameter = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "Parameter").Select(r => r["Position"].ToString()).SingleOrDefault();
                    string strQCType = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "QCType").Select(r => r["Position"].ToString()).SingleOrDefault();
                    string strsyssamplecode = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "SysSampleCode").Select(r => r["Position"].ToString()).SingleOrDefault();
                    string strisReport = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "IsReport").Select(r => r["Position"].ToString()).SingleOrDefault();
                    string strSampleLayer = string.Empty;
                    string strSampleLayerColumnName = string.Empty;
                    if (qcbatchinfo.IsPLMTest)
                    {
                        strSampleLayer = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "SampleLayerID").Select(r => r["Position"].ToString()).SingleOrDefault();
                        strSampleLayer = Regex.Replace(strSampleLayer, @"[^A-Z]+", String.Empty);
                        strSampleLayerColumnName = diSSColumnsToExportColumns[strSampleLayer];
                    }
                    int intStartRow = int.Parse(Regex.Replace(strSampleID, @"[^\d]", ""));
                    strSampleID = Regex.Replace(strSampleID, @"[^A-Z]+", String.Empty);
                    strRunNo = !string.IsNullOrWhiteSpace(strRunNo) ? Regex.Replace(strRunNo, @"[^A-Z]+", String.Empty) : String.Empty;
                    strParameter = !string.IsNullOrWhiteSpace(strParameter) ? Regex.Replace(strParameter, @"[^A-Z]+", String.Empty) : String.Empty;
                    strQCType = !string.IsNullOrWhiteSpace(strQCType) ? Regex.Replace(strQCType, @"[^A-Z]+", String.Empty) : String.Empty;
                    strsyssamplecode = !string.IsNullOrWhiteSpace(strsyssamplecode) ? Regex.Replace(strsyssamplecode, @"[^A-Z]+", String.Empty) : String.Empty;
                    strisReport = !string.IsNullOrWhiteSpace(strisReport) ? Regex.Replace(strisReport, @"[^A-Z]+", String.Empty) : String.Empty;
                    string strSampleIDColumnName = diSSColumnsToExportColumns[strSampleID];
                    string strRunNoColumnName = !string.IsNullOrWhiteSpace(strRunNo) ? diSSColumnsToExportColumns[strRunNo] : String.Empty;
                    string strParameterColumnName = !string.IsNullOrWhiteSpace(strParameter) ? diSSColumnsToExportColumns[strParameter] : string.Empty;
                    string strQCTypColumnName = !string.IsNullOrWhiteSpace(strQCType) ? diSSColumnsToExportColumns[strQCType] : string.Empty;
                    string strsyssamplecodecolumnname = !string.IsNullOrWhiteSpace(strsyssamplecode) ? diSSColumnsToExportColumns[strsyssamplecode] : string.Empty;
                    string strisReportcolumnname = !string.IsNullOrWhiteSpace(strisReport) ? diSSColumnsToExportColumns[strisReport] : string.Empty;
                    Worksheet worksheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets[int.Parse(drrSample[0]["SheetID"].ToString()) - 1];
                    CellRange RanData = worksheet.GetDataRange();
                    int intlastUsedRow = RanData.BottomRowIndex + 1;
                    int intlastUsedColumn = RanData.RightColumnIndex + 1;

                    CellRange range = worksheet.Range[string.Format("A{0}:{01}{02}", intStartRow, diIndexToColumn[intlastUsedColumn], intlastUsedRow)];
                    bool rangeHasHeaders = false;
                    DataTable dataTable = worksheet.CreateDataTable(range, false);
                    string strnonsamplecellid = string.Empty;
                    string cellvalue = string.Empty;
                    int cellid = intStartRow;
                    string strchar = "A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z";
                    string[] strarr = strchar.Split('|');
                    int syscodeclm = 0;
                    List<string> lststrtemp = new List<string>();
                    if (!string.IsNullOrEmpty(strsyssamplecode) && !string.IsNullOrEmpty(strisReport))
                    {
                        foreach (string objstr in strarr)
                        {
                            if (strsyssamplecode == objstr)
                            {
                                break;
                            }
                            syscodeclm++;
                        }
                        for (int r = 0; r < range.RowCount; r++)
                        {
                            if (range[r, syscodeclm].Value.TextValue != null)
                            {
                                cellvalue = range[r, syscodeclm].Value.TextValue.ToString();
                            }
                            string[] strsyscodearr = cellvalue.Split('R');
                            if (strsyscodearr != null && strsyscodearr.Length > 1)
                            {
                                if (!lststrtemp.Contains(strsyscodearr[0].ToString()))
                                {
                                    lststrtemp.Add(strsyscodearr[0].ToString());
                                }

                                if (strnonsamplecellid == string.Empty)
                                {
                                    strnonsamplecellid = strisReport + cellid.ToString();
                                }
                                else
                                {
                                    strnonsamplecellid = strnonsamplecellid + "," + strisReport + cellid.ToString();
                                }
                            }
                            cellid++;
                        }
                        cellid = intStartRow;
                        for (int r = 0; r < range.RowCount; r++)
                        {
                            if (range[r, syscodeclm].Value.TextValue != null)
                            {
                                cellvalue = range[r, syscodeclm].Value.TextValue.ToString();
                            }
                            if (lststrtemp.Contains(cellvalue))
                            {
                                if (strnonsamplecellid == string.Empty)
                                {
                                    strnonsamplecellid = strisReport + cellid.ToString();
                                }
                                else
                                {
                                    strnonsamplecellid = strnonsamplecellid + "," + strisReport + cellid.ToString();
                                }
                            }
                            cellid++;
                        }
                    }


                    for (int col = 0; col < range.ColumnCount; col++)
                    {
                        CellValueType cellType = range[0, col].Value.Type;
                        for (int r = 1; r < range.RowCount; r++)
                        {
                            if (cellType != range[r, col].Value.Type)
                            {
                                dataTable.Columns[col].DataType = typeof(string);
                                break;
                            }
                        }
                    }

                    DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dataTable, rangeHasHeaders);
                    exporter.CellValueConversionError += Exporter_CellValueConversionError;
                    exporter.Export();

                    DataRow[] drrSampleData = qcbatchinfo.dtsample.Select("QCType IS NULL OR QCType='' OR QCType = 'SAMPLE'");
                    foreach (DataRow drSample in drrSampleData)
                    {
                        DataRow[] drCurrentRow = null;
                        if (!string.IsNullOrEmpty(strsyssamplecode) && !string.IsNullOrWhiteSpace(strsyssamplecode))
                        {
                            if (!string.IsNullOrWhiteSpace(strParameterColumnName) && !string.IsNullOrWhiteSpace(strRunNoColumnName) && !string.IsNullOrWhiteSpace(strQCTypColumnName) && !string.IsNullOrWhiteSpace(strsyssamplecodecolumnname))
                                drCurrentRow = dataTable.Select(string.Format("{0} = '{1}' AND {2} = '{3}' AND {4} = '{5}' AND {6} = '{7}'AND {8} = '{9}'", strSampleIDColumnName, drSample["SampleID"], strRunNoColumnName, drSample["RunNo"], strParameterColumnName, drSample["Parameter"], strQCTypColumnName, "SAMPLE", strsyssamplecodecolumnname, drSample["SysSampleCode"]));
                            else if (!string.IsNullOrWhiteSpace(strParameterColumnName) && !string.IsNullOrWhiteSpace(strQCTypColumnName) && !string.IsNullOrWhiteSpace(strsyssamplecodecolumnname))
                                drCurrentRow = dataTable.Select(string.Format("{0} = '{1}' AND {2} = '{3}' AND {4} = '{5}' AND {6} = '{7}'", strSampleIDColumnName, drSample["SampleID"], strParameterColumnName, drSample["Parameter"], strQCTypColumnName, "SAMPLE", strsyssamplecodecolumnname, drSample["SysSampleCode"]));
                            else if (!string.IsNullOrWhiteSpace(strsyssamplecodecolumnname))
                                drCurrentRow = dataTable.Select(string.Format("{0} = '{1}' AND {2} = '{3}' ", strSampleIDColumnName, drSample["SampleID"], strsyssamplecodecolumnname, drSample["SysSampleCode"]));
                            else
                                drCurrentRow = dataTable.Select(string.Format("{0} = '{1}'", strsyssamplecodecolumnname, drSample["SysSampleCode"]));
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(strParameterColumnName) && !string.IsNullOrWhiteSpace(strRunNoColumnName) && !string.IsNullOrWhiteSpace(strQCTypColumnName) && !string.IsNullOrWhiteSpace(strSampleLayerColumnName))
                                drCurrentRow = dataTable.Select(string.Format("{0} = '{1}' AND {2} = '{3}' AND {4} = '{5}' AND {6} = '{7}' AND {8} = '{9}'", strSampleIDColumnName, drSample["SampleID"], strRunNoColumnName, drSample["RunNo"], strParameterColumnName, drSample["Parameter"], strQCTypColumnName, "SAMPLE", strSampleLayerColumnName, drSample["SampleLayerID"]));
                            else if (!string.IsNullOrWhiteSpace(strParameterColumnName) && !string.IsNullOrWhiteSpace(strRunNoColumnName) && !string.IsNullOrWhiteSpace(strQCTypColumnName))
                                drCurrentRow = dataTable.Select(string.Format("{0} = '{1}' AND {2} = '{3}' AND {4} = '{5}' AND {6} = '{7}'", strSampleIDColumnName, drSample["SampleID"], strRunNoColumnName, drSample["RunNo"], strParameterColumnName, drSample["Parameter"], strQCTypColumnName, "SAMPLE"));
                            else if (!string.IsNullOrWhiteSpace(strParameterColumnName) && !string.IsNullOrWhiteSpace(strQCTypColumnName))
                                drCurrentRow = dataTable.Select(string.Format("{0} = '{1}' AND {2} = '{3}' AND {4} = '{5}'", strSampleIDColumnName, drSample["SampleID"], strParameterColumnName, drSample["Parameter"], strQCTypColumnName, "SAMPLE"));
                            else if (!string.IsNullOrWhiteSpace(strSampleIDColumnName))
                                drCurrentRow = dataTable.Select(string.Format("{0} = '{1}' ", strSampleIDColumnName, drSample["SampleID"]));
                        }
                        if (drCurrentRow != null && drCurrentRow.Length > 0)
                        {
                            foreach (DataRow drBind in drrSample)
                            {
                                if (qcbatchinfo.dtsample.Columns.Contains(drBind["FieldName"].ToString().ToUpper()))
                                {
                                    if (!string.IsNullOrEmpty(drBind["Format"].ToString()) && !string.IsNullOrEmpty(drCurrentRow[0][diSSColumnsToExportColumns[Regex.Replace(drBind["Position"].ToString(), @"[^A-Z]+", String.Empty)]].ToString()))
                                    {
                                        string strValue = drCurrentRow[0][diSSColumnsToExportColumns[Regex.Replace(drBind["Position"].ToString(), @"[^A-Z]+", String.Empty)]].ToString();
                                        double dblValue;
                                        DateTime dtValue;
                                        bool bolIsNumeric = double.TryParse(strValue, out dblValue);
                                        bool bolIsDateTime = DateTime.TryParse(strValue, out dtValue);
                                        if (bolIsNumeric == true)
                                        {
                                            //DateTime datetime = DateTime.FromOADate(dblValue);
                                            //drSample[drBind["FieldName"].ToString().ToUpper()] = datetime;
                                            drSample[drBind["FieldName"].ToString().ToUpper()] = string.Format("{0:" + drBind["Format"].ToString() + "}", dblValue);
                                        }
                                        else if (bolIsDateTime == true && (DateTime.TryParseExact(strValue, drBind["Format"].ToString(), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dtValue)))
                                        {
                                            drSample[drBind["FieldName"].ToString().ToUpper()] = dtValue;
                                        }
                                        else
                                            drSample[drBind["FieldName"].ToString().ToUpper()] = string.Format("{0:" + drBind["Format"].ToString() + "}", strValue);
                                    }
                                    else if (!string.IsNullOrEmpty(drCurrentRow[0][diSSColumnsToExportColumns[Regex.Replace(drBind["Position"].ToString(), @"[^A-Z]+", String.Empty)]].ToString()))
                                    {
                                        drSample[drBind["FieldName"].ToString().ToUpper()] = drCurrentRow[0][diSSColumnsToExportColumns[Regex.Replace(drBind["Position"].ToString(), @"[^A-Z]+", String.Empty)]];
                                    }
                                    else
                                    {
                                        drSample[drBind["FieldName"].ToString().ToUpper()] = DBNull.Value;
                                    }
                                }
                            }
                        }
                    }
                    DataRow[] drrQCSampleData = qcbatchinfo.dtsample.Select("QCType IS NOT NULL AND QCType <> 'SAMPLE' AND QCType <> ''");
                    if (drrQCSampleData.Length > 0)
                    {
                        string strQCTypeID = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "QCType").Select(r => r["Position"].ToString()).SingleOrDefault();
                        string strSystemID = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "SystemID").Select(r => r["Position"].ToString()).SingleOrDefault();
                        if (!string.IsNullOrEmpty(strQCTypeID))
                        {
                            string strSystemIDColumnName = string.Empty;
                            strQCTypeID = Regex.Replace(strQCTypeID, @"[^A-Z]+", String.Empty);
                            if (!string.IsNullOrEmpty(strSystemID))
                            {
                                strSystemID = Regex.Replace(strSystemID, @"[^A-Z]+", String.Empty);
                                strSystemIDColumnName = diSSColumnsToExportColumns[strSystemID];
                            }

                            string strQCTypeColumnName = diSSColumnsToExportColumns[strQCTypeID];
                            foreach (DataRow drSample in drrQCSampleData)
                            {
                                DataRow[] drCurrentRow = null;
                                if (!string.IsNullOrWhiteSpace(strParameterColumnName) && !string.IsNullOrWhiteSpace(strRunNoColumnName))
                                    drCurrentRow = dataTable.Select(string.Format("{0} = '{1}' AND {2} = '{3}' AND {4} = '{5}' AND {6} = '{7}' ", strQCTypeColumnName, drSample["QCType"], strRunNoColumnName, drSample["RunNo"], strSystemIDColumnName, drSample["SystemID"], strParameterColumnName, drSample["Parameter"]));
                                else if (!string.IsNullOrWhiteSpace(strParameterColumnName))
                                    drCurrentRow = dataTable.Select(string.Format("{0} = '{1}' AND {2} = '{3}' ", strQCTypeColumnName, drSample["QCType"], strParameterColumnName, drSample["Parameter"]));
                                else
                                    drCurrentRow = dataTable.Select(string.Format("{0} = '{1}'", strQCTypeColumnName, drSample["QCType"]));
                                if (drCurrentRow != null && drCurrentRow.Length > 0)
                                {
                                    foreach (DataRow drBind in drrSample)
                                    {
                                        if (qcbatchinfo.dtsample.Columns.Contains(drBind["FieldName"].ToString().ToUpper()))
                                        {
                                            if (!string.IsNullOrEmpty(drBind["Format"].ToString()) && !string.IsNullOrEmpty(drCurrentRow[0][diSSColumnsToExportColumns[Regex.Replace(drBind["Position"].ToString(), @"[^A-Z]+", String.Empty)]].ToString()))
                                            {
                                                string strValue = drCurrentRow[0][diSSColumnsToExportColumns[Regex.Replace(drBind["Position"].ToString(), @"[^A-Z]+", String.Empty)]].ToString();
                                                double dblValue = 0.00;
                                                DateTime dtValue;
                                                bool bolIsNumeric = double.TryParse(strValue, out dblValue);
                                                bool bolIsDateTime = DateTime.TryParse(strValue, out dtValue);
                                                if (bolIsNumeric == true)
                                                {
                                                    //DateTime datetime = DateTime.FromOADate(dblValue); 
                                                    drSample[drBind["FieldName"].ToString().ToUpper()] = string.Format("{0:" + drBind["Format"].ToString() + "}", dblValue); //datetime;

                                                    //drSample[drBind["FieldName"].ToString().ToUpper()] = string.Format("{0:" + drBind["Format"].ToString() + "}", dblValue);

                                                }
                                                else if (bolIsDateTime == true && (DateTime.TryParseExact(strValue, drBind["Format"].ToString(), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dtValue)))
                                                {
                                                    drSample[drBind["FieldName"].ToString().ToUpper()] = dtValue;
                                                }
                                                else
                                                    drSample[drBind["FieldName"].ToString().ToUpper()] = string.Format("{0:" + drBind["Format"].ToString() + "}", strValue);
                                            }
                                            else if (!string.IsNullOrEmpty(drCurrentRow[0][diSSColumnsToExportColumns[Regex.Replace(drBind["Position"].ToString(), @"[^A-Z]+", String.Empty)]].ToString()))
                                            {
                                                drSample[drBind["FieldName"].ToString().ToUpper()] = drCurrentRow[0][diSSColumnsToExportColumns[Regex.Replace(drBind["Position"].ToString(), @"[^A-Z]+", String.Empty)]];
                                            }
                                            else
                                            {
                                                drSample[drBind["FieldName"].ToString().ToUpper()] = DBNull.Value;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(strnonsamplecellid))
                    {
                        worksheet.DataValidations.Add(worksheet[strnonsamplecellid], DataValidationType.List, ValueObject.FromRange(worksheet["AA1:AA2"].GetRangeWithAbsoluteReference()));
                    }
                    foreach (DataRow drBindSingle in drrSampleSingle)
                    {
                        worksheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets[int.Parse(drBindSingle["SheetID"].ToString()) - 1];
                        foreach (DataRow drSample in qcbatchinfo.dtsample.Rows)
                        {
                            if (qcbatchinfo.dtsample.Columns.Contains(drBindSingle["FieldName"].ToString().ToUpper()))
                            {
                                if (string.IsNullOrEmpty(worksheet.Cells[drBindSingle["Position"].ToString()].DisplayText) || worksheet.Cells[drBindSingle["Position"].ToString()].DisplayText == "#DIV/0!")
                                    drSample[drBindSingle["FieldName"].ToString().ToUpper()] = DBNull.Value;
                                else
                                {
                                    string strValue = worksheet.Cells[drBindSingle["Position"].ToString()].Value.ToString();
                                    double dblValue;
                                    DateTime dtValue;
                                    bool bolIsNumeric = double.TryParse(strValue, out dblValue);
                                    bool bolIsDateTime = DateTime.TryParse(strValue, out dtValue);
                                    if (bolIsNumeric == true)
                                    {
                                        //DateTime datetime = DateTime.FromOADate(dblValue);
                                        drSample[drBindSingle["FieldName"].ToString().ToUpper()] = dblValue;
                                        //drSample[drBindSingle["FieldName"].ToString().ToUpper()] = string.Format("{0:" + drBindSingle["Format"].ToString() + "}", dblValue);
                                    }

                                    else if (bolIsDateTime == true && (DateTime.TryParseExact(strValue, drBindSingle["Format"].ToString(), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dtValue)))
                                    {
                                        drSample[drBindSingle["FieldName"].ToString().ToUpper()] = dtValue;
                                    }
                                    else
                                        drSample[drBindSingle["FieldName"].ToString().ToUpper()] = string.Format("{0:" + drBindSingle["Format"].ToString() + "}", strValue);
                                }
                            }
                        }
                    }
                    ABGridrefresh();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return false;
            }
        }

        private void CalibAndABIDSupport(ASPxSpreadsheet sscResultEntry)
        {
            try
            {
                string strFiler = "RunType = 'RawDataTable' AND FieldName ='QCBATCHID'";
                //strFiler = "RunType = 'RawDataTable' AND FieldName ='CalibrationID'";
                //strFiler = "RunType = 'CalibrationTable' AND FieldName ='CalibrationID'";
                //
                if (qcbatchinfo.dtDataParsing == null || qcbatchinfo.dtDataParsing.Rows.Count == 0) return;
                DataRow[] drrCalibration = qcbatchinfo.dtDataParsing.Select(strFiler);
                if (drrCalibration != null && drrCalibration.Length > 0)
                {
                    IWorkbook wbResultEntry = sscResultEntry.Document;
                    Worksheet wsSheet1 = wbResultEntry.Worksheets[int.Parse(drrCalibration[0]["SheetID"].ToString()) - 1];
                    foreach (DataRow drCalibration in drrCalibration)
                    {
                        if (string.IsNullOrEmpty(drCalibration["Continuous"].ToString()) == false && Convert.ToBoolean(drCalibration["Continuous"]) == true)
                        {
                            if (string.IsNullOrEmpty(drCalibration["Read"].ToString()) == false && Convert.ToBoolean(drCalibration["Read"]) == true)
                            {
                                string strColumn = drCalibration["Position"].ToString().Substring(0, 1);
                                int intStartRow = int.Parse(drCalibration["Position"].ToString().Substring(1, drCalibration["Position"].ToString().Length - 1));
                                foreach (DataRow drData in qcbatchinfo.dtsample.Rows)
                                {
                                    string strValue = drData[drCalibration["FieldName"].ToString().ToUpper()].ToString();
                                    wsSheet1.Cells[strColumn + intStartRow.ToString()].Value = strValue;
                                    intStartRow += 1;
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(drCalibration["Continuous"].ToString()) == false && Convert.ToBoolean(drCalibration["Continuous"]) == false)
                        {
                            if (string.IsNullOrEmpty(drCalibration["Read"].ToString()) == false && Convert.ToBoolean(drCalibration["Read"]) == true)
                            {

                                foreach (DataRow drData in qcbatchinfo.dtsample.Rows)
                                {
                                    string strValue = drData[drCalibration["FieldName"].ToString().ToUpper()].ToString();
                                    wsSheet1.Cells[drCalibration["Position"].ToString()].Value = strValue;
                                    break;
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
            finally
            {
            }

        }

        private void Exporter_CellValueConversionError(object sender, CellValueConversionErrorEventArgs e)
        {
            try
            {
                e.DataTableValue = null;
                e.Action = DataTableExporterAction.Continue;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void LoadfromAB(ASPxSpreadsheet aSPxSpreadsheet, SpreadSheetEntry_AnalyticalBatch batch)
        {
            try
            {
                IWorkbook objworkbook = aSPxSpreadsheet.Document;
                //objworkbook.Worksheets[0].Cells.FillColor = Color.Transparent;
                objworkbook.LoadDocument(batch.SpreadSheet.ToArray(), DocumentFormat.OpenXml);
                objworkbook.Worksheets[0].Cells.Flags.Fill = false;
                qcbatchinfo.IsSheetloaded = true;
                qcbatchinfo.dtsample = new DataTable { TableName = "RawDataTableDataSource" };
                qcbatchinfo.dtselectedsamplefields = new DataTable();
                qcbatchinfo.dtDataParsing = new DataTable();
                //Getdtsamplesource(qcbatchinfo.strqcid);
                Getdtsamplesource(qcbatchinfo.QCBatchOid);
                Getdtparsingsamplefields(batch.TemplateID);
                CalibAndABIDSupport(aSPxSpreadsheet);
                //qcbatchinfo.strTestMethodMatrixName = batch.Test.MatrixName.MatrixName;
                //qcbatchinfo.strTestMethodTestName = batch.Test.TestName;
                //qcbatchinfo.strTestMethodMethodNumber = batch.Test.MethodName.MethodNumber;
                //if (qcbatchinfo.dtsample != null && qcbatchinfo.dtsample.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in qcbatchinfo.dtsample.Rows)
                //    {
                //        if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 0)
                //        {
                //            dr["Status"] = Samplestatus.PendingEntry;
                //        }
                //        else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 1)
                //        {
                //            dr["Status"] = Samplestatus.PendingValidation;
                //        }
                //        else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 2)
                //        {
                //            dr["Status"] = Samplestatus.PendingApproval;
                //        }
                //        else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 3)
                //        {
                //            dr["Status"] = Samplestatus.PendingReporting;
                //        }
                //        else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 4)
                //        {
                //            dr["Status"] = Samplestatus.PendingReportValidation;
                //        }
                //        else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 5)
                //        {
                //            dr["Status"] = Samplestatus.PendingReportApproval;
                //        }
                //        else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 6)
                //        {
                //            dr["Status"] = Samplestatus.Approved;
                //        }
                //        else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 7)
                //        {
                //            dr["Status"] = Samplestatus.PendingReview;
                //        }
                //        else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 8)
                //        {
                //            dr["Status"] = Samplestatus.PendingVerify;
                //        }
                //        else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 9)
                //        {
                //            dr["Status"] = Samplestatus.ReportApproved;
                //        }
                //        else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 10)
                //        {
                //            dr["Status"] = Samplestatus.Reported;
                //        }
                //        else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 11)
                //        {
                //            dr["Status"] = Samplestatus.SuboutPendingValidation;
                //        }
                //        else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) && Convert.ToInt32(dr["Status"]) == 12)
                //        {
                //            dr["Status"] = Samplestatus.SuboutPendingApproval;
                //        }
                //    }
                //}
                foreach (DataColumn column in qcbatchinfo.dtsample.Columns)
                {
                    column.ColumnName = column.ColumnName.ToUpper();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void Getdtsamplerunnosource(string qcid)
        public string connectionstring = "@";
        private void Getdtsamplerunnosource(Guid? oid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("LDMSPREADSHEETENTRY_SELECT_Runno_SP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] param = new SqlParameter[1];
                        //param[0] = new SqlParameter("@QCBATCHID", qcid);
                        param[0] = new SqlParameter("@QCBATCHID", oid);
                        cmd.Parameters.AddRange(param);
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(qcbatchinfo.dtsample);
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
        //private void Getdtsamplesource(string qcid)
        private void Getdtsamplesource(Guid? oid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("LDMSPREADSHEETENTRY_SELECT_SP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] param = new SqlParameter[1];
                        //param[0] = new SqlParameter("@QCBATCHID", qcid);
                        param[0] = new SqlParameter("@QCBATCHID", oid);
                        cmd.Parameters.AddRange(param);
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(qcbatchinfo.dtsample);
                        }
                        if (qcbatchinfo.dtsample.Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            foreach (DataColumn dtsampcol in qcbatchinfo.dtsample.Columns)
                            {
                                if (dtsampcol.ColumnName.ToUpper() == "STATUS")
                                {
                                    dt.Columns.Add(dtsampcol.ColumnName, typeof(string));
                                }
                                else
                                {
                                    dt.Columns.Add(dtsampcol.ColumnName, dtsampcol.DataType);
                                }
                            }
                            foreach (DataRow dr in qcbatchinfo.dtsample.Rows)
                            {
                                dt.ImportRow(dr);
                            }
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["NUMERICRESULT"] != null && dr["NUMERICRESULT"].GetType() != typeof(DBNull))
                                {
                                    decimal deci = Convert.ToDecimal(dr["NUMERICRESULT"]);
                                    dr["NUMERICRESULT"] = Math.Round(deci, 2).ToString();
                                }
                                if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) /*&& dr["Status"].ToString() == "0"*/)
                                {
                                    dr["Status"] = dr["Samplestatus"].ToString();
                                }
                                //else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull)/* && dr["Status"].ToString() == "1"*/)
                                //{
                                //    dr["Status"] = dr["Samplestatus"].ToString();
                                //}
                                //else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) /*&& dr["Status"].ToString() == "2"*/)
                                //{
                                //    dr["Status"] = dr["Samplestatus"].ToString();
                                //}
                                //else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull)/* && dr["Status"].ToString() == "3"*/)
                                //{
                                //    dr["Status"] = dr["Samplestatus"].ToString();
                                //}
                                //else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull)/* && dr["Status"].ToString() == "4"*/)
                                //{
                                //    dr["Status"] = "Pending Report Validation";
                                //}
                                //else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) /*&& dr["Status"].ToString() == "5"*/)
                                //{
                                //    dr["Status"] = "Pending Report Approval";
                                //}
                                //else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) /*&& dr["Status"].ToString() == "6"*/)
                                //{
                                //    dr["Status"] = "Approved";
                                //}
                                //else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) /*&& dr["Status"].ToString() == "7"*/)
                                //{
                                //    dr["Status"] = "Pending Review";
                                //}
                                //else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) /*&& dr["Status"].ToString() == "8"*/)
                                //{
                                //    dr["Status"] = "Pending Verify";
                                //}
                                //else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) /*&& dr["Status"].ToString() == "9"*/)
                                //{
                                //    dr["Status"] = "Report Approved";
                                //}
                                //else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) /*&& dr["Status"].ToString() == "10"*/)
                                //{
                                //    dr["Status"] = "Reported";
                                //}
                                //else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) /*&& dr["Status"].ToString() == "11"*/)
                                //{
                                //    dr["Status"] = "Subout Pending Validation";
                                //}
                                //else if (dr["Status"] != null && dr["Status"].GetType() != typeof(DBNull) /*&& dr["Status"].ToString() == "12"*/)
                                //{
                                //    dr["Status"] = "Subout Pending Approval";
                                //}
                            }

                            qcbatchinfo.dtsample = dt;
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

        public DataTable GetQCTypeMatch()
        {
            try
            {
                DataTable dtQuery = new DataTable();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand Command = new SqlCommand("SpreadSheetEntry_SelectQCtypeMatch_SP", con))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.CommandTimeout = 0;
                        SqlDataAdapter DBAdapter = new SqlDataAdapter(Command);
                        DBAdapter.Fill(dtQuery);
                    }
                }
                return dtQuery;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        public DataTable GetParameterMatch()
        {
            try
            {
                DataTable dtQuery = new DataTable();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand Command = new SqlCommand("SpreadSheetEntry_SelectParameterMatch_SP", con))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.CommandTimeout = 0;
                        SqlDataAdapter DBAdapter = new SqlDataAdapter(Command);
                        DBAdapter.Fill(dtQuery);
                    }
                }
                return dtQuery;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        public DataTable GetQCTypesByTestMethod(string intTestMethodID)
        {
            try
            {
                DataTable dtQuery = new DataTable();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand Command = new SqlCommand("SpreadSheetEntry_QCTypeByTestMethod_SP", con))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.CommandTimeout = 0;
                        SqlParameter Param = new SqlParameter("@TestMethodID", intTestMethodID);
                        Command.Parameters.Add(Param);
                        SqlDataAdapter DBAdapter = new SqlDataAdapter(Command);
                        DBAdapter.Fill(dtQuery);
                    }
                }
                return dtQuery;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        private DataTable GetReportingInfo(int intTemplateID)
        {
            try
            {
                DataTable dtQuery = new DataTable();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand Command = new SqlCommand("SpreadSheetEntry_Reporting_SP", con))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.CommandTimeout = 0;
                        SqlParameter Param = new SqlParameter("@TemplateID", intTemplateID);
                        Command.Parameters.Add(Param);
                        SqlDataAdapter DBAdapter = new SqlDataAdapter(Command);
                        DBAdapter.Fill(dtQuery);
                    }
                }
                return dtQuery;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        private void Getdtcalibrationsource(int templateid, Guid? testmethod)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SPREADSHEETENTRY_CALIBRATION_SELECT_SP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] param = new SqlParameter[4];
                        param[0] = new SqlParameter("@CALIBRATIONID", DBNull.Value);
                        param[1] = new SqlParameter("@TEMPLATEID", templateid);
                        param[2] = new SqlParameter("@USERNAME", SecuritySystem.CurrentUserName);
                        param[3] = new SqlParameter("@TestMethodID", testmethod);
                        cmd.Parameters.AddRange(param);
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            if (qcbatchinfo.dtCalibration == null)
                            {
                                qcbatchinfo.dtCalibration = CreateCBDT();
                            }
                            sda.Fill(qcbatchinfo.dtCalibration);
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

        private DataTable GetImportDataSource(string JobID, int TemplateID)
        {
            try
            {
                DataTable dtImportDataSource = new DataTable();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SpreadSheetEntry_ImportDataSource_Sp", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] param = new SqlParameter[2];
                        param[0] = new SqlParameter("@colJobID", JobID);
                        param[1] = new SqlParameter("@TemplateID", TemplateID);
                        cmd.Parameters.AddRange(param);
                        SqlDataAdapter DBAdapter = new SqlDataAdapter(cmd);
                        DBAdapter.Fill(dtImportDataSource);
                    }
                }
                return dtImportDataSource;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        private void Getdtparsingsamplefields(int templateid)
        {
            try
            {
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
                                qcbatchinfo.dtTemplateInfo = ds.Tables[0].Copy();
                                qcbatchinfo.dtselectedsamplefields = ds.Tables[1].Copy();
                                qcbatchinfo.dtDataParsing = ds.Tables[3].Copy();
                                qcbatchinfo.dtDataTransfer = ds.Tables[4].Copy();
                                qcbatchinfo.dtHeader = ds.Tables[7].Copy();
                                qcbatchinfo.dtDetail = ds.Tables[8].Copy();
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

        private void NewABID(ASPxSpreadsheet aSPxSpreadsheet, SpreadSheetEntry_AnalyticalBatch qC)
        {
            try
            {
                SpreadSheetEntry_AnalyticalBatch objAB = os.CreateObject<SpreadSheetEntry_AnalyticalBatch>();
                string tempab = string.Empty;
                string userid = "_" + ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                var curdate = DateTime.Now.ToString("yyMMdd");
                //IList<SpreadSheetEntry_AnalyticalBatch> spreadSheets = os.GetObjects<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("SUBSTRING([AnalyticalBatchID], 2, 9)=?", curdate + userid)); 
                IList<SpreadSheetEntry_AnalyticalBatch> spreadSheets = os.GetObjects<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("SUBSTRING([AnalyticalBatchID], 2, 6)=?", curdate));
                if (spreadSheets.Count > 0)
                {
                    spreadSheets = spreadSheets.OrderBy(a => a.AnalyticalBatchID).ToList();
                    tempab = "QB" + curdate + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].AnalyticalBatchID.Substring(8, 2)) + 1).ToString("00") + userid;
                }
                else
                {
                    tempab = "QB" + curdate + "01" + userid;
                }
                objAB.AnalyticalBatchID = tempab;

                //CriteriaOperator abct = CriteriaOperator.Parse("Max(SUBSTRING(AnalyticalBatchID, 2, 8))");
                //string tempab = (Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(SpreadSheetEntry_AnalyticalBatch), abct, null)) + 1).ToString();
                //var curdate = DateTime.Now.ToString("yyMMdd");
                //if (tempab != "1")
                //{
                //    var predate = tempab.Substring(0, 6);
                //    if (predate == curdate)
                //    {
                //        tempab = "AB" + tempab;
                //    }
                //    else
                //    {
                //        tempab = "AB" + curdate + "01";
                //    }
                //}
                //else
                //{
                //    tempab = "AB" + curdate + "01";
                //}
                //objAB.AnalyticalBatchID = tempab + "_" + SecuritySystem.CurrentUserName;

                objAB.SpreadSheet = aSPxSpreadsheet.Document.SaveDocument(DocumentFormat.OpenXml);
                objAB.CreatedDate = DateTime.Now;
                objAB.TemplateID = qcbatchinfo.templateid;
                objAB.Instrument = qC.Instrument;
                objAB.CreatedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                objAB.Status = 1;
                //objAB.qcbatchID = qC;
                objAB.Calibration = NewCBID(qcbatchinfo.templateid, IsCommit: false);
                if (objAB.Calibration != null)
                {
                    objAB.uqCalibrationID = objAB.Calibration.uqID;
                }
                //qC.ABID = objAB;
                objAB.Comments = qC.Comments;
                IObjectSpace os1 = Application.CreateObjectSpace(typeof(Notes));
                Notes note = os1.CreateObject<Notes>();
                if (note != null)
                {
                    note.SourceID = CNInfo.SDMSJobId;
                    note.NameSource = CNInfo.SDMSSampleMatries;
                    note.Text = qC.Comments;
                    note.Samplecheckin = os1.GetObjectByKey<Samplecheckin>(CNInfo.SCoidValue);
                }
                os1.CommitChanges();
                objAB.Instrument = qC.Instrument;
                if (qC.Instruments.Count > 0)
                {
                    foreach (Labware obj in qC.Instruments)
                    {
                        objAB.Instruments.Add(obj);
                    }
                }
                if (qC.Reagents.Count > 0)
                {
                    foreach (Reagent obj in qC.Reagents)
                    {
                        objAB.Reagents.Add(obj);
                    }
                }
                ////System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                ////TimeSpan span;
                ////stopwatch.Start();
                ABCreateUpdate(objAB);
                ////stopwatch.Stop();
                ////span = stopwatch.Elapsed;
                ////Frame.GetController<ExceptionTrackingViewController>().Logtime(objAB.AnalyticalBatchID + " (Bindgrid Process)", span.ToString(@"hh\:mm\:ss\.ff"));
                ////stopwatch.Restart();
                //WebWindow.CurrentRequestPage.Session["ABID"] = objAB.AnalyticalBatchID;
                //WebWindow.CurrentRequestPage.Session["ABOid"] = objAB.Oid;
                //os.Committing += Os_Committing;
                //os.Committed += Os_Committed;
                os.CommitChanges();
                ////stopwatch.Stop();
                ////span = stopwatch.Elapsed;
                ////Frame.GetController<ExceptionTrackingViewController>().Logtime(objAB.AnalyticalBatchID + " (Save Process)", span.ToString(@"hh\:mm\:ss\.ff"));
                qcbatchinfo.strAB = objAB.AnalyticalBatchID;
                if (objAB.Calibration != null)
                {
                    qcbatchinfo.strCB = objAB.Calibration.CalibrationID;
                }
                //WebWindow.CurrentRequestPage.Session.Remove("ABID");
                //WebWindow.CurrentRequestPage.Session.Remove("ABOid");
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void Os_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    os.Committing -= Os_Committing;
        //    CriteriaOperator abct = CriteriaOperator.Parse("Max(SUBSTRING(AnalyticalBatchID, 2, 8))");
        //    string tempab = (Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(SpreadSheetEntry_AnalyticalBatch), abct, null)) + 1).ToString();
        //    var curdate = DateTime.Now.ToString("yyMMdd");
        //    if (tempab != "1")
        //    {
        //        var predate = tempab.Substring(0, 6);
        //        if (predate == curdate)
        //        {
        //            tempab = "AB" + tempab;
        //        }
        //        else
        //        {
        //            tempab = "AB" + curdate + "01";
        //        }
        //    }
        //    else
        //    {
        //        tempab = "AB" + curdate + "01";
        //    }
        //    SpreadSheetEntry_AnalyticalBatch CurABID = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[Oid]=?", new Guid(WebWindow.CurrentRequestPage.Session["ABOid"].ToString())));
        //    if (CurABID != null && CurABID.AnalyticalBatchID != tempab + "_" + SecuritySystem.CurrentUserName)
        //    {
        //        CurABID.AnalyticalBatchID = tempab + "_" + SecuritySystem.CurrentUserName;
        //        WebWindow.CurrentRequestPage.Session["ABID"] = tempab + "_" + SecuritySystem.CurrentUserName;
        //    }
        //}

        //private void Os_Committed(object sender, EventArgs e)
        //{
        //    os.Committed -= Os_Committed;
        //    for (int i = 0; i < 10; i++)
        //    {
        //        IList<SpreadSheetEntry_AnalyticalBatch> ABID = ObjectSpace.GetObjects<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", WebWindow.CurrentRequestPage.Session["ABID"].ToString()));
        //        if (ABID != null && ABID.Count > 1)
        //        {
        //            checkid();
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}

        //private void checkid()
        //{
        //    CriteriaOperator abct = CriteriaOperator.Parse("Max(SUBSTRING(AnalyticalBatchID, 2, 8))");
        //    string tempab = (Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(SpreadSheetEntry_AnalyticalBatch), abct, null)) + 1).ToString();
        //    var curdate = DateTime.Now.ToString("yyMMdd");
        //    if (tempab != "1")
        //    {
        //        var predate = tempab.Substring(0, 6);
        //        if (predate == curdate)
        //        {
        //            tempab = "AB" + tempab;
        //        }
        //        else
        //        {
        //            tempab = "AB" + curdate + "01";
        //        }
        //    }
        //    else
        //    {
        //        tempab = "AB" + curdate + "01";
        //    }
        //    SpreadSheetEntry_AnalyticalBatch CurABID = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[Oid]=?", new Guid(WebWindow.CurrentRequestPage.Session["ABOid"].ToString())));
        //    if (CurABID != null)
        //    {
        //        CurABID.AnalyticalBatchID = tempab + "_" + SecuritySystem.CurrentUserName;
        //        os.CommitChanges();
        //        WebWindow.CurrentRequestPage.Session["ABID"] = CurABID.AnalyticalBatchID;
        //    }
        //}

        private Calibration NewCBID(int templateid, bool IsCommit)
        {
            try
            {
                Getdtcalibrationsource(templateid, qcbatchinfo.OidTestMethod);
                IObjectSpace calibOS = Application.CreateObjectSpace();
                SpreadSheetBuilder_TemplateInfo objTemplate = calibOS.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID] = ?", templateid));
                if (objTemplate != null && objTemplate.CalibrationLevelNo > 0)
                {
                    Calibration objCalib = calibOS.CreateObject<Calibration>();
                    objCalib.CreatedBy = calibOS.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                    objCalib.CreatedDate = Modules.BusinessObjects.Libraries.Library.GetServerTime(calibOS);
                    objCalib.TemplateID = objTemplate;
                    objCalib.uqID = Convert.ToInt32(((XPObjectSpace)calibOS).Session.Evaluate(typeof(Calibration), CriteriaOperator.Parse("Max(uqID)"), null)) + 1;
                    string tempcb = string.Empty;
                    string userid = ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                    var curdate = DateTime.Now.ToString("yyMMdd");
                    IList<Calibration> spreadSheets = calibOS.GetObjects<Calibration>(CriteriaOperator.Parse("SUBSTRING([CalibrationID], 2, 9)=?", curdate + userid));
                    if (spreadSheets.Count > 0)
                    {
                        spreadSheets = spreadSheets.OrderBy(a => a.CalibrationID).ToList();
                        tempcb = "CB" + curdate + userid + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].CalibrationID.Substring(11, 2)) + 1).ToString("00");
                    }
                    else
                    {
                        tempcb = "CB" + curdate + userid + "01";
                    }
                    objCalib.CalibrationID = tempcb;
                    calibOS.CommitChanges();
                    if (!IsCommit)
                    {
                        calibOS = os;
                    }
                    objCalib = calibOS.GetObject<Calibration>(objCalib);
                    DateTime calibdateTime = Modules.BusinessObjects.Libraries.Library.GetServerTime(calibOS);
                    for (int i = 1; i <= objTemplate.CalibrationLevelNo; i++)
                    {
                        CalibrationInfo objCalibInfo = calibOS.CreateObject<CalibrationInfo>();
                        objCalibInfo.LevelNo = i;
                        objCalibInfo.Calibration = objCalib;
                        objCalibInfo.uqTemplateID = calibOS.GetObject<SpreadSheetBuilder_TemplateInfo>(objTemplate);
                        //objCalibInfo.uqID = Convert.ToInt32(((XPObjectSpace)calibOS).Session.Evaluate(typeof(CalibrationInfo), CriteriaOperator.Parse("Max(uqID)"), null)) + 1;
                        SpreadSheetBuilder_TestParameter objID = calibOS.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID]=?", qcbatchinfo.OidTestMethod));
                        if (objID != null)
                        {
                            Testparameter testparameter = calibOS.GetObjectByKey<Testparameter>(objID.TestParameterID);
                            if (testparameter != null)
                            {
                                objCalibInfo.uqTestParameterID = testparameter;
                                if (testparameter.Parameter != null)
                                {
                                    objCalibInfo.Parameter = testparameter.Parameter.ParameterName;
                                    objCalibInfo.ParameterID = testparameter.Parameter.Oid;
                                }
                            }
                        }
                        objCalibInfo.CalibratedBy = calibOS.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                        objCalibInfo.CalibratedDate = calibdateTime;
                        if (qcbatchinfo.dtCalibration != null && qcbatchinfo.dtCalibration.Rows != null && qcbatchinfo.dtCalibration.Columns != null && qcbatchinfo.dtCalibration.Rows.Count > 0 && qcbatchinfo.dtCalibration.Columns.Count > 0)
                        {
                            DataRow[] arrDR = qcbatchinfo.dtCalibration.Select("LEVELNO = " + i);
                            if (arrDR != null && arrDR.Count() > 0)
                            {
                                DataRow dr = arrDR.FirstOrDefault();
                                if (qcbatchinfo.dtCalibration.Columns.Contains("INTERCEPT") && dr["INTERCEPT"] != null && dr["INTERCEPT"].GetType() == typeof(string))
                                {
                                    objCalibInfo.Intercept = dr["INTERCEPT"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("SLOPE") && dr["SLOPE"] != null && dr["SLOPE"].GetType() == typeof(string))
                                {
                                    objCalibInfo.Slope = dr["SLOPE"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("RCAP2") && dr["RCAP2"] != null && dr["RCAP2"].GetType() == typeof(string))
                                {
                                    objCalibInfo.RCAP2 = dr["RCAP2"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("CONCENTRATION") && dr["CONCENTRATION"] != null && dr["CONCENTRATION"].GetType() == typeof(string))
                                {
                                    objCalibInfo.Conc = dr["CONCENTRATION"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("READING") && dr["READING"] != null && dr["READING"].GetType() == typeof(string))
                                {
                                    objCalibInfo.Reading = dr["READING"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("VS") && dr["VS"] != null && dr["VS"].GetType() == typeof(string))
                                {
                                    objCalibInfo.VS = dr["VS"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("VC1") && dr["VC1"] != null && dr["VC1"].GetType() == typeof(string))
                                {
                                    objCalibInfo.VC1 = dr["VC1"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("VC2") && dr["VC2"] != null && dr["VC2"].GetType() == typeof(string))
                                {
                                    objCalibInfo.VC2 = dr["VC2"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("VC") && dr["VC"] != null && dr["VC"].GetType() == typeof(string))
                                {
                                    objCalibInfo.VC = dr["VC"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("ABSORBANCE") && dr["ABSORBANCE"] != null && dr["ABSORBANCE"].GetType() == typeof(string))
                                {
                                    objCalibInfo.ABSORBANCE = dr["ABSORBANCE"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("F") && dr["F"] != null && dr["F"].GetType() == typeof(string))
                                {
                                    objCalibInfo.F = dr["F"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("TITRANT1") && dr["TITRANT1"] != null && dr["TITRANT1"].GetType() == typeof(string))
                                {
                                    objCalibInfo.Titrant1 = dr["TITRANT1"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("TITRANT2") && dr["TITRANT2"] != null && dr["TITRANT2"].GetType() == typeof(string))
                                {
                                    objCalibInfo.Titrant2 = dr["TITRANT2"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("UNIT") && dr["UNIT"] != null && dr["UNIT"].GetType() == typeof(string))
                                {
                                    objCalibInfo.Unit = dr["UNIT"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("ASSIGNEDPH") && dr["ASSIGNEDPH"] != null && dr["ASSIGNEDPH"].GetType() == typeof(string))
                                {
                                    objCalibInfo.AssignedpH = dr["ASSIGNEDPH"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("READINGAFTER") && dr["READINGAFTER"] != null && dr["READINGAFTER"].GetType() == typeof(string))
                                {
                                    objCalibInfo.ReadingAfter = dr["READINGAFTER"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("READINGBEFORE") && dr["READINGBEFORE"] != null && dr["READINGBEFORE"].GetType() == typeof(string))
                                {
                                    objCalibInfo.ReadingBefore = dr["READINGBEFORE"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("REAGENTID") && dr["REAGENTID"] != null && dr["REAGENTID"].GetType() == typeof(string))
                                {
                                    objCalibInfo.ReagentID = dr["REAGENTID"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("BUFFER") && dr["BUFFER"] != null && dr["BUFFER"].GetType() == typeof(string))
                                {
                                    objCalibInfo.Buffer = dr["BUFFER"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("CURVELIMIT") && dr["CURVELIMIT"] != null && dr["CURVELIMIT"].GetType() == typeof(string))
                                {
                                    objCalibInfo.CurveLimit = dr["CURVELIMIT"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("STDCONC") && dr["STDCONC"] != null && dr["STDCONC"].GetType() == typeof(string))
                                {
                                    objCalibInfo.StdConc = dr["STDCONC"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("STDCONCVOLUSED") && dr["STDCONCVOLUSED"] != null && dr["STDCONCVOLUSED"].GetType() == typeof(string))
                                {
                                    objCalibInfo.StdConcVolUsed = dr["STDCONCVOLUSED"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED1") && dr["USERDEFINED1"] != null && dr["USERDEFINED1"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined1 = dr["USERDEFINED1"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED2") && dr["USERDEFINED2"] != null && dr["USERDEFINED2"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined2 = dr["USERDEFINED2"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED3") && dr["USERDEFINED3"] != null && dr["USERDEFINED3"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined3 = dr["USERDEFINED3"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED4") && dr["USERDEFINED4"] != null && dr["USERDEFINED4"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined4 = dr["USERDEFINED4"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED5") && dr["USERDEFINED5"] != null && dr["USERDEFINED5"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined5 = dr["USERDEFINED5"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED6") && dr["USERDEFINED6"] != null && dr["USERDEFINED6"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined6 = dr["USERDEFINED6"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED7") && dr["USERDEFINED7"] != null && dr["USERDEFINED7"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined7 = dr["USERDEFINED7"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED8") && dr["USERDEFINED8"] != null && dr["USERDEFINED8"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined8 = dr["USERDEFINED8"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED9") && dr["USERDEFINED9"] != null && dr["USERDEFINED9"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined9 = dr["USERDEFINED9"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED10") && dr["USERDEFINED10"] != null && dr["USERDEFINED10"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined10 = dr["USERDEFINED10"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED11") && dr["USERDEFINED11"] != null && dr["USERDEFINED11"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined11 = dr["USERDEFINED11"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED12") && dr["USERDEFINED12"] != null && dr["USERDEFINED12"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined12 = dr["USERDEFINED12"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED13") && dr["USERDEFINED13"] != null && dr["USERDEFINED13"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined13 = dr["USERDEFINED13"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED14") && dr["USERDEFINED14"] != null && dr["USERDEFINED14"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined14 = dr["USERDEFINED14"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED15") && dr["USERDEFINED15"] != null && dr["USERDEFINED15"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined15 = dr["USERDEFINED15"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED16") && dr["USERDEFINED16"] != null && dr["USERDEFINED16"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined16 = dr["USERDEFINED16"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED17") && dr["USERDEFINED17"] != null && dr["USERDEFINED17"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined17 = dr["USERDEFINED17"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED18") && dr["USERDEFINED18"] != null && dr["USERDEFINED18"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined18 = dr["USERDEFINED18"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED19") && dr["USERDEFINED19"] != null && dr["USERDEFINED19"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined19 = dr["USERDEFINED19"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED20") && dr["USERDEFINED20"] != null && dr["USERDEFINED20"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined20 = dr["USERDEFINED20"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED21") && dr["USERDEFINED21"] != null && dr["USERDEFINED21"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined21 = dr["USERDEFINED21"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED22") && dr["USERDEFINED22"] != null && dr["USERDEFINED22"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined22 = dr["USERDEFINED22"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED23") && dr["USERDEFINED23"] != null && dr["USERDEFINED23"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined23 = dr["USERDEFINED23"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED24") && dr["USERDEFINED24"] != null && dr["USERDEFINED24"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined24 = dr["USERDEFINED24"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED25") && dr["USERDEFINED25"] != null && dr["USERDEFINED25"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined25 = dr["USERDEFINED25"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED26") && dr["USERDEFINED26"] != null && dr["USERDEFINED26"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined26 = dr["USERDEFINED26"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED27") && dr["USERDEFINED27"] != null && dr["USERDEFINED27"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined27 = dr["USERDEFINED27"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED28") && dr["USERDEFINED28"] != null && dr["USERDEFINED28"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined28 = dr["USERDEFINED28"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED29") && dr["USERDEFINED29"] != null && dr["USERDEFINED29"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined29 = dr["USERDEFINED29"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED30") && dr["USERDEFINED30"] != null && dr["USERDEFINED30"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined30 = dr["USERDEFINED30"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED31") && dr["USERDEFINED31"] != null && dr["USERDEFINED31"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined31 = dr["USERDEFINED31"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED32") && dr["USERDEFINED32"] != null && dr["USERDEFINED32"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined32 = dr["USERDEFINED32"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED33") && dr["USERDEFINED33"] != null && dr["USERDEFINED33"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined33 = dr["USERDEFINED33"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED34") && dr["USERDEFINED34"] != null && dr["USERDEFINED34"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined34 = dr["USERDEFINED34"].ToString();
                                }
                                if (qcbatchinfo.dtCalibration.Columns.Contains("USERDEFINED35") && dr["USERDEFINED35"] != null && dr["USERDEFINED35"].GetType() == typeof(string))
                                {
                                    objCalibInfo.UserDefined35 = dr["USERDEFINED35"].ToString();
                                }
                            }

                            if (IsCommit)
                            {
                                calibOS.CommitChanges();
                                qcbatchinfo.strCB = objCalib.CalibrationID;
                                CalibRefresh();
                            }
                        }
                    }
                    return objCalib;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        private void ABCreateUpdate(SpreadSheetEntry_AnalyticalBatch objAB, IList<SpreadSheetEntry> spreadSheets = null)
        {
            try
            {
                SpreadSheetEntry sheetEntry;
                for (int i = 0; i < qcbatchinfo.dtsample.Rows.Count; i++)
                {
                    if (spreadSheets != null && spreadSheets.Count == qcbatchinfo.dtsample.Rows.Count)
                    {
                        sheetEntry = spreadSheets.Cast<SpreadSheetEntry>().Where(a => a.uqSampleParameterID != null && a.uqSampleParameterID.Oid.ToString() == qcbatchinfo.dtsample.Rows[i]["UQSAMPLEPARAMETERID"].ToString() && a.RunNo == Convert.ToInt32(qcbatchinfo.dtsample.Rows[i]["RunNo"])).FirstOrDefault();
                    }
                    else
                    {
                        sheetEntry = os.CreateObject<SpreadSheetEntry>();
                    }
                    if (sheetEntry != null)
                    {
                        sheetEntry.uqAnalyticalBatchID = objAB;
                        string strColumnName = string.Empty;
                        foreach (DataColumn column in qcbatchinfo.dtsample.Columns)
                        {
                            if (column.ColumnName[0] == '%')
                            {
                                strColumnName = column.ColumnName.Replace(@"%", "P_");
                            }
                            else
                            {
                                strColumnName = column.ColumnName;
                            }
                            //var sproperty = sheetEntry.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name.ToUpper() == column.ColumnName && column.ColumnName != "OID").FirstOrDefault();
                            var sproperty = sheetEntry.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name.ToUpper() == strColumnName && column.ColumnName != "OID").FirstOrDefault();
                            if (sproperty != null && !qcbatchinfo.dtsample.Rows[i].IsNull(column.ColumnName))
                            {
                                if (sproperty.MappingFieldDBType.ToString() == qcbatchinfo.dtsample.Rows[i][column].GetType().Name.ToString())
                                {
                                    if (sproperty.MappingFieldDBType != DBColumnType.Guid)
                                    {
                                        sproperty.SetValue(sheetEntry, qcbatchinfo.dtsample.Rows[i][column]);
                                    }
                                    else
                                    {
                                        var Objrefrence = os.FindObject(Type.GetType(sproperty.ReferenceType.FullName + "," + sproperty.ReferenceType.AssemblyName), CriteriaOperator.Parse("[Oid]=?", qcbatchinfo.dtsample.Rows[i][column]));
                                        if (Objrefrence != null)
                                        {
                                            sproperty.SetValue(sheetEntry, Objrefrence);
                                        }
                                    }
                                }
                                else
                                {
                                    if (sproperty.MappingFieldDBType == DBColumnType.Guid && sproperty.ReferenceType.TableName == "Employee")
                                    {
                                        var Objrefrence = os.FindObject<Employee>(CriteriaOperator.Parse("[DisplayName]=?", qcbatchinfo.dtsample.Rows[i][column]));
                                        if (Objrefrence != null)
                                        {
                                            sproperty.SetValue(sheetEntry, Objrefrence);
                                        }
                                    }
                                    else if (sproperty.MappingFieldDBType != DBColumnType.Guid)
                                    {
                                        Type type = Type.GetType(sproperty.MemberType.FullName);
                                        if (type != null)
                                        {
                                            sproperty.SetValue(sheetEntry, Convert.ChangeType(qcbatchinfo.dtsample.Rows[i][column], type));
                                        }
                                    }
                                }
                            }
                        }
                        if (sheetEntry.uqSampleParameterID != null && string.IsNullOrEmpty(sheetEntry.uqSampleParameterID.ABID))
                        {
                            sheetEntry.uqSampleParameterID.ABID = objAB.AnalyticalBatchID;
                            sheetEntry.uqSampleParameterID.UQABID = objAB;
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

        private void ABGridrefresh()
        {
            try
            {
                if (Frame is NestedFrame)
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView view = nestedFrame.ViewItem.View;
                    if (view != null)
                    {
                        foreach (DashboardViewItem vi in view.Items.Where(i => i.GetType() == typeof(DashboardViewItem) && ((DashboardViewItem)i).InnerView == null))
                        {
                            vi.CreateControl();
                        }
                        foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                        {
                            if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                            {
                                if (frameContainer.Frame.View.Id == "SDMSDCAB_ListView")
                                {
                                    if (((ListView)frameContainer.Frame.View).IsControlCreated)
                                    {
                                        ASPxGridListEditor gridListEditor = ((ListView)frameContainer.Frame.View).Editor as ASPxGridListEditor;
                                        if (gridListEditor != null && gridListEditor.Grid != null)
                                        {
                                            griddataload(gridListEditor.Grid);
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

        private void CalibRefresh()
        {
            try
            {
                NestedFrame nestedFrame = (NestedFrame)Frame;
                CompositeView view = nestedFrame.ViewItem.View;
                foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                {
                    if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                    {
                        if (frameContainer.Frame.View.Id == "Calibration_DetailView_CalibrationCurve")
                        {
                            if (((DetailView)frameContainer.Frame.View).IsControlCreated)
                            {
                                Calibration calib = ((DetailView)frameContainer.Frame.View).ObjectSpace.FindObject<Calibration>(CriteriaOperator.Parse("[CalibrationID] = ?", qcbatchinfo.strCB));
                                //if (calib != null && (((DetailView)frameContainer.Frame.View).CurrentObject == null || (((DetailView)frameContainer.Frame.View).CurrentObject != null && ((Calibration)((DetailView)frameContainer.Frame.View).CurrentObject).Oid != calib.Oid)))
                                if (calib != null)
                                {
                                    ((DetailView)frameContainer.Frame.View).CurrentObject = calib;
                                    ((DetailView)frameContainer.Frame.View).ObjectSpace.ReloadObject(calib);
                                    ((DetailView)frameContainer.Frame.View).Refresh();
                                }
                                else
                                {
                                    ((DetailView)frameContainer.Frame.View).CurrentObject = null;
                                    ((DetailView)frameContainer.Frame.View).Refresh();
                                }
                                DashboardViewItem calibCurve = ((DetailView)frameContainer.Frame.View).FindItem("CalibrationCurve") as DashboardViewItem;
                                if (calibCurve != null && calibCurve.InnerView != null)
                                {
                                    //Getdtcalibrationsource(qcbatchinfo.templateid, qcbatchinfo.OidTestMethod);
                                    if (calibCurve.InnerView is ListView && ((ListView)calibCurve.InnerView).Editor != null && ((ListView)calibCurve.InnerView).Editor is ASPxChartListEditor)
                                    {
                                        ASPxChartListEditor editor = ((ListView)calibCurve.InnerView).Editor as ASPxChartListEditor;
                                        if (editor != null)
                                        {
                                            WebChartControl chart = editor.ChartControl;
                                            if (chart != null)
                                            {
                                                if (!string.IsNullOrEmpty(qcbatchinfo.strCB))
                                                {
                                                    IList<CalibrationInfo> calibInfos = os.GetObjects<CalibrationInfo>(CriteriaOperator.Parse("[Calibration.CalibrationID] = ?", qcbatchinfo.strCB));
                                                    chart.DataSource = calibInfos;
                                                    chart.DataBind();
                                                    chart.RefreshData();
                                                }
                                                else
                                                {
                                                    IList<CalibrationInfo> calibInfos = os.GetObjects<CalibrationInfo>(CriteriaOperator.Parse("Oid is null"));
                                                    chart.DataSource = calibInfos;
                                                    chart.DataBind();
                                                    chart.RefreshData();
                                                }
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

        private void fillcombo(ASPxSpreadsheet aSPxSpreadsheet)
        {
            try
            {
                DataTable ABtable = CreateABDT();
                DataTable CBtable = CreateCBDT();
                modechange(aSPxSpreadsheet, qcbatchinfo.strMode);
                if (qcbatchinfo.strMode == "Enter")
                {
                    if (qcbatchinfo.strTest != null)
                    {
                        SelectedData result = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SpreadSheetEntry_FillAnalyticalBatchID_SP", new SprocParameter("@Mode", qcbatchinfo.strMode), new SprocParameter("@TestMethodID", qcbatchinfo.OidTestMethod));
                        foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                        {
                            ABtable.Rows.Add(new object[] { (string)row.Values[4], (string)row.Values[5], (string)row.Values[6], (string)row.Values[7], (string)row.Values[8], (string)row.Values[9], null });
                        }
                    }
                    if (qcbatchinfo.OidTestMethod != null)
                    {
                        TestMethod test = os.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid]=?", qcbatchinfo.OidTestMethod));
                        if (test != null)
                        {
                            SpreadSheetBuilder_TestParameter objID = os.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID]=?", test.Oid));
                            if (objID != null)
                            {
                                SelectedData result = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SpreadSheetEntry_FillCalibrationID_SP", new SprocParameter("@TemplateID", objID.TemplateID));
                                foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                                {
                                    CBtable.Rows.Add(new object[] { (int)row.Values[0], (string)row.Values[1], (DateTime)row.Values[2], (string)row.Values[3], (string)row.Values[4] });
                                }
                                //((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("CalibrationID")).PropertiesComboBox.DataSource = CBtable;
                            }
                        }
                    }
                    ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("AnalyticalBatch")).PropertiesComboBox.Columns[6].Visible = false;
                    // ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("CalibrationID")).Value = qcbatchinfo.strCB;
                }
                else if (qcbatchinfo.strMode == "View")
                {
                    //SelectedData result = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SpreadSheetEntry_FillAnalyticalBatchID_SP", new SprocParameter("@Mode", qcbatchinfo.strMode));
                    //foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                    //{
                    //    ABtable.Rows.Add(new object[] { (string)row.Values[4], (string)row.Values[5], (string)row.Values[6], (string)row.Values[7], (string)row.Values[8], (string)row.Values[9], (string)row.Values[10] });
                    //}
                    if (qcbatchinfo.strTest != null)
                    {
                        SelectedData result = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SpreadSheetEntry_FillAnalyticalBatchID_SP", new SprocParameter("@Mode", qcbatchinfo.strMode), new SprocParameter("@TestMethodID", qcbatchinfo.OidTestMethod));
                        foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                        {
                            ABtable.Rows.Add(new object[] { (string)row.Values[4], (string)row.Values[5], (string)row.Values[6], (string)row.Values[7], (string)row.Values[8], (string)row.Values[9], (string)row.Values[10] });
                        }
                    }
                    //else
                    //{
                    //    SelectedData result = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SpreadSheetEntry_FillAnalyticalBatchID_SP", new SprocParameter("@Mode", qcbatchinfo.strMode));
                    //    foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                    //    {
                    //        ABtable.Rows.Add(new object[] { (string)row.Values[4], (string)row.Values[5], (string)row.Values[6], (string)row.Values[7], (string)row.Values[8], (string)row.Values[9], (string)row.Values[10] });
                    //    }
                    //}
                    ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Test")).PropertiesComboBox.DataSource = ABtable;
                    ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("AnalyticalBatch")).PropertiesComboBox.Columns[6].Visible = true;
                }
                else if (qcbatchinfo.strMode == "Review" || qcbatchinfo.strMode == "Verify")
                {
                    bool Administrator = false;
                    List<Guid> lstTestMethodOid = new List<Guid>();

                    CustomSystemUser currentUsers = (CustomSystemUser)SecuritySystem.CurrentUser;
                    if (currentUsers.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        Administrator = true;
                    }
                    else
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                        if (qcbatchinfo.strMode == "Review")
                        {
                            lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] =True", currentUsers.Oid));
                        }
                        else if (qcbatchinfo.strMode == "Verify")
                        {
                            lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultApproval] =True", currentUsers.Oid));
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

                    SelectedData result = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SpreadSheetEntry_FillAnalyticalBatchID_SP", new SprocParameter("@Mode", qcbatchinfo.strMode));
                    foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                    {
                        Guid testOid = new Guid(row.Values[3].ToString());
                        if (Administrator)
                        {
                            ABtable.Rows.Add(new object[] { (string)row.Values[4], (string)row.Values[5], (string)row.Values[6], (string)row.Values[7], (string)row.Values[8], (string)row.Values[9], null });
                        }
                        else if (lstTestMethodOid.Contains(testOid))
                        {
                            ABtable.Rows.Add(new object[] { (string)row.Values[4], (string)row.Values[5], (string)row.Values[6], (string)row.Values[7], (string)row.Values[8], (string)row.Values[9], null });
                        }
                    }
                    ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("AnalyticalBatch")).PropertiesComboBox.Columns[6].Visible = false;
                }
                ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("AnalyticalBatch")).PropertiesComboBox.DataSource = ABtable;
                DataTable table = CreateTDT();

                bool isAdministrator = false;
                List<Guid> lstTestMethodOids = new List<Guid>();

                CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                {
                    isAdministrator = true;
                }
                else
                {
                    IObjectSpace permissionos = Application.CreateObjectSpace();
                    IList<AnalysisDepartmentChain> lstAnalysisDepartChain = permissionos.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] =True", currentUser.Oid));
                    if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                    {
                        foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                        {
                            foreach (TestMethod testMethod in departmentChain.TestMethods)
                            {
                                if (!lstTestMethodOids.Contains(testMethod.Oid))
                                {
                                    lstTestMethodOids.Add(testMethod.Oid);
                                }
                            }
                        }
                    }
                }
                IList<TestMethod> tests = os.GetObjects<TestMethod>(CriteriaOperator.Parse(""));
                if (tests != null)
                {
                    foreach (TestMethod test in tests.OrderBy(a => a.TestName).ToList())
                    {
                        if (isAdministrator == true || (isAdministrator == false && lstTestMethodOids.Contains(test.Oid) == true))
                        {
                            SpreadSheetBuilder_TestParameter objID = os.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID]=?", test.Oid));
                            if (objID != null)
                            {
                                //IList<SampleParameter> objsp = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [Samplelogin] Is Not Null And ([QCBatchID] Is Null or [QCBatchID.qcseqdetail.ABID.Status] = 1)", test.Oid));
                                string strCriteria = string.Empty;
                                if (qcbatchinfo.strMode == "Enter")
                                {
                                    strCriteria = "[Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [Samplelogin] Is Not Null And ([QCBatchID] Is Null or [UQABID.Status] = 1)";
                                }
                                else if (qcbatchinfo.strMode == "Review")
                                {
                                    strCriteria = "[Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [Samplelogin] Is Not Null And ([QCBatchID] Is Null or [UQABID.Status] = 2)";
                                }
                                else if (qcbatchinfo.strMode == "Verify")
                                {
                                    strCriteria = "[Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [Samplelogin] Is Not Null And ([QCBatchID] Is Null or [UQABID.Status] = 3)";
                                }
                                else
                                {
                                    strCriteria = "[Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [Samplelogin] Is Not Null And ([QCBatchID] Is Null or [UQABID.Status] > 1)";
                                }
                                IList<SampleParameter> objsp = os.GetObjects<SampleParameter>(CriteriaOperator.Parse(strCriteria, test.Oid));
                                IList<SampleLogIn> objdistsl = objsp.Select(s => s.Samplelogin).Distinct().ToList();
                                //IList<SampleLogIn> objdistsl = new List<SampleLogIn>();
                                //foreach (SampleParameter sample in objsp)
                                //{
                                //    if (!objdistsl.Contains(sample.Samplelogin))
                                //    {
                                //        objdistsl.Add(sample.Samplelogin);
                                //    }
                                //}
                                if (objdistsl.Count > 0)
                                {
                                    SpreadSheetBuilder_TemplateInfo objName = os.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID]=?", objID.TemplateID));
                                    if (objName != null)
                                    {
                                        table.Rows.Add(new object[] { test.TestName, test.MatrixName.MatrixName, test.MethodName.MethodNumber, objName.TemplateName, objdistsl.Count, test.Oid, objName.TemplateID });
                                    }
                                }
                            }
                        }
                    }
                }
                ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Test")).PropertiesComboBox.DataSource = table;
                ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Mode")).Value = qcbatchinfo.strMode;
                ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Test")).Value = qcbatchinfo.strTest;
                ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("AnalyticalBatch")).Value = qcbatchinfo.strAB;
                if (qcbatchinfo.IsMoldTest)
                {
                    //((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("JobID")).Visible = true;
                    //((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("JobID")).ClientEnabled = true;
                    ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Visible = true;
                    ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).ClientEnabled = true;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous").Visible = true;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next").Visible = true;

                    fillComboMold(aSPxSpreadsheet);
                    //setRibbonForMold(aSPxSpreadsheet);
                    //if(qcbatchinfo.strAB != null)
                    //{
                    //    SpreadSheetEntry_AnalyticalBatch objAB = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                    //    if(objAB != null && objAB.Jobid != null)
                    //    {
                    //        string[] JobIDs = objAB.Jobid.Split(';');
                    //        foreach (string strJobID in JobIDs)
                    //        {
                    //            ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("JobID")).Items.Add(strJobID);
                    //        }
                    //        IList<QCBatchSequence> lstQCBatchSeq = os.GetObjects<QCBatchSequence>(CriteriaOperator.Parse("[qcseqdetail]=?", objAB.Oid));
                    //        {
                    //            if(lstQCBatchSeq != null)
                    //            {
                    //                string[] SampleIDs = lstQCBatchSeq.Select(s => s.SYSSamplecode).Distinct().ToArray();
                    //                foreach (string strSampleID in SampleIDs)
                    //                {
                    //                    ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Items.Add(strSampleID);
                    //                }
                    //            }
                    //        }

                    //    }
                    ////}

                }
                else
                {
                    ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("JobID")).Visible = false;
                    ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("JobID")).ClientEnabled = false;
                    ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Visible = false;
                    ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous").Visible = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next").Visible = false;
                }
                if (qcbatchinfo.strMode == "Enter" || (qcbatchinfo.strMode == "View" && qcbatchinfo.Isedit == true))
                {
                    if (objPermissionInfo.SDMSIsWrite)
                    {
                        aSPxSpreadsheet.JSProperties["cpisedit"] = false;
                    }
                    else
                    {
                        aSPxSpreadsheet.JSProperties["cpisedit"] = true;
                    }
                }
                else
                {
                    aSPxSpreadsheet.JSProperties["cpisedit"] = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void modechange(ASPxSpreadsheet aSPxSpreadsheet, string Mode)
        {
            try
            {
                if (Mode == "Enter")
                {
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Test").Visible = true;
                    // aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("CalibrationID").Visible = true;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("RollBack").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("DataPackage").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Clear").ClientEnabled = true;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Edit").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("Recalculate").Visible = true;
                    if (objPermissionInfo.SDMSIsWrite)
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Complete").ClientEnabled = true;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Save").ClientEnabled = true;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("BindGrid").ClientEnabled = true;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("Recalculate").ClientEnabled = true;
                    }
                    else
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Complete").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Save").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("BindGrid").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("Recalculate").ClientEnabled = false;
                    }

                    if (objPermissionInfo.SDMSIsDelete)
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Delete").ClientEnabled = true;
                    }
                    else
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Delete").ClientEnabled = false;
                    }
                    if (objPermissionInfo.SDMSIsCreate)
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("NewCalibration").ClientEnabled = true;
                    }
                    else
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("NewCalibration").ClientEnabled = false;
                    }

                    if (qcbatchinfo.IsSheetloaded)
                    {
                        if (objPermissionInfo.SDMSIsWrite)
                        {
                            aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("ImportFiles").ClientEnabled = true;
                        }
                        else
                        {
                            aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("ImportFiles").ClientEnabled = false;
                        }

                        if (!string.IsNullOrEmpty(qcbatchinfo.strAB))
                        {
                            if (objPermissionInfo.SDMSIsWrite)
                            {
                                aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("UploadImage").ClientEnabled = true;
                            }
                            else
                            {
                                aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("UploadImage").ClientEnabled = false;
                            }
                        }
                        else
                        {
                            aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("UploadImage").ClientEnabled = false;
                        }
                    }
                    else
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("ImportFiles").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("UploadImage").ClientEnabled = false;
                    }
                }
                else if (Mode == "View")
                {
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Test").Visible = true;
                    // aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("CalibrationID").Visible = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Complete").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Save").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Delete").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("ImportFiles").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("UploadImage").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("NewCalibration").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("Recalculate").Visible = true;
                    Editcontrols();
                    if (objPermissionInfo.SDMSIsWrite && sdmsstatus <= 2)
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Edit").ClientEnabled = true;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("Recalculate").ClientEnabled = true;
                    }
                    else
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Edit").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("Recalculate").ClientEnabled = false;
                    }

                    if (qcbatchinfo.Isedit == true)
                    {
                        ((RibbonButtonItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Edit")).Text = rm.GetString("Update_" + CurrentLanguage);
                        ((RibbonButtonItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Edit")).LargeImage.Url = "~/Images/recurrence_32x32.png";
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("RollBack").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("DataPackage").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Retrieve").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Clear").ClientEnabled = true;
                        ((RibbonButtonItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Clear")).Text = rm.GetString("Cancel_" + CurrentLanguage);
                        ((RibbonButtonItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Clear")).LargeImage.Url = "~/Images/cancel_32x32.png";

                        if (objPermissionInfo.SDMSIsWrite)
                        {
                            aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("BindGrid").ClientEnabled = true;
                            aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("Recalculate").ClientEnabled = true;
                        }
                        else
                        {
                            aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("BindGrid").ClientEnabled = false;
                            aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("Recalculate").ClientEnabled = false;
                        }
                    }
                    else
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Retrieve").ClientEnabled = true;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Clear").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("BindGrid").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("Recalculate").ClientEnabled = false;
                        aSPxSpreadsheet.ReadOnly = true;
                        if (objPermissionInfo.SDMSIsWrite)
                        {
                            aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("RollBack").ClientEnabled = true;
                        }
                        else
                        {
                            aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("RollBack").ClientEnabled = false;
                        }
                    }

                    if (objnavigationRefresh.ClickedNavigationItem == "SDMSBatchResults")
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Test").Visible = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Mode").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("AnalyticalBatch").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("Recalculate").Visible = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Retrieve").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("RollBack").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("DataPackage").ClientEnabled = false;
                    }
                }
                else if (Mode == "Review" || Mode == "Verify")
                {
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Test").Visible = false;
                    //aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("CalibrationID").Visible = false;

                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Save").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Delete").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Clear").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("Edit").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("BindGrid").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("ImportFiles").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("UploadImage").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("NewCalibration").ClientEnabled = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Complete").Text = rm.GetString(Mode + "_" + CurrentLanguage);
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group3").Items.FindByName("Recalculate").Visible = false;
                    if (objPermissionInfo.SDMSIsWrite)
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Complete").ClientEnabled = true;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("RollBack").ClientEnabled = true;
                    }
                    else
                    {
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group1").Items.FindByName("Complete").ClientEnabled = false;
                        aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group2").Items.FindByName("RollBack").ClientEnabled = false;
                    }
                }
                if (qcbatchinfo != null && qcbatchinfo.IsMoldTest)
                {
                    //aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("JobID").Visible = true;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID").Visible = true;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous").Visible = true;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next").Visible = true;
                    //fillComboMold(aSPxSpreadsheet);
                }
                else
                {
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("JobID").Visible = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID").Visible = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous").Visible = false;
                    aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next").Visible = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Editcontrols()
        {
            if (qcbatchinfo.strAB != null)
            {
                SpreadSheetEntry_AnalyticalBatch objabid = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB)); //[qcbatchID.Oid] = ?
                if (objabid != null)
                {
                    sdmsstatus = objabid.Status;
                }
            }
        }
        private void clearcontrols(ASPxSpreadsheet aSPxSpreadsheet)
        {
            try
            {
                qcbatchinfo.OidTestMethod = null;
                qcbatchinfo.qcstatus = 0;
                qcbatchinfo.strAB = null;
                qcbatchinfo.strCB = null;
                qcbatchinfo.strqcid = null;
                qcbatchinfo.strTest = null;
                qcbatchinfo.Isedit = false;
                qcbatchinfo.IsSheetloaded = false;
                qcbatchinfo.IsMoldTest = false;
                qcbatchinfo.templateid = 0;
                qcbatchinfo.dtsample = new DataTable();
                qcbatchinfo.dtCalibration = new DataTable();
                qcbatchinfo.dtselectedsamplefields = new DataTable();
                qcbatchinfo.dtDataParsing = new DataTable();
                qcbatchinfo.dtHeader = new DataTable();
                qcbatchinfo.dtDetail = new DataTable();
                aSPxSpreadsheet.Document.CreateNewDocument();
                ((RibbonComboBoxItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Visible = false;
                ((RibbonButtonItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).Visible = false;
                ((RibbonButtonItem)aSPxSpreadsheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Next")).Visible = false;
                ABGridrefresh();
                CalibRefresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void changetoviewmode()
        {
            try
            {
                qcbatchinfo.dtsample = new DataTable { TableName = "RawDataTableDataSource" };
                //Getdtsamplesource(qcbatchinfo.strqcid);
                Getdtsamplesource(qcbatchinfo.QCBatchOid);
                foreach (DataColumn column in qcbatchinfo.dtsample.Columns)
                {
                    column.ColumnName = column.ColumnName.ToUpper();
                }
                ABGridrefresh();
                qcbatchinfo.strMode = "View";
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void BindSampleToReport(ASPxSpreadsheetPropertyEditor aSPxSpreadsheet, Guid ABID)
        {
            try
            {
                DataRow[] drrSample = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable' AND Write = TRUE AND Continuous= TRUE");
                DataRow[] drrSampleSingle = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable' AND Write = TRUE AND Continuous= FALSE");
                if (drrSample.Length == 0) return;
                DataRow[] drrSelect = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable'");
                string strSampleID = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "SampleID").Select(r => r["Position"].ToString()).SingleOrDefault();
                string strRunNo = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "RunNo").Select(r => r["Position"].ToString()).SingleOrDefault();
                string strParameter = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "Parameter").Select(r => r["Position"].ToString()).SingleOrDefault();
                string strQCType = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "QCType").Select(r => r["Position"].ToString()).SingleOrDefault();
                int intStartRow = int.Parse(Regex.Replace(strSampleID, @"[^\d]", ""));
                strSampleID = Regex.Replace(strSampleID, @"[^A-Z]+", String.Empty);
                strRunNo = !string.IsNullOrWhiteSpace(strRunNo) ? Regex.Replace(strRunNo, @"[^A-Z]+", String.Empty) : String.Empty;
                strParameter = !string.IsNullOrWhiteSpace(strParameter) ? Regex.Replace(strParameter, @"[^A-Z]+", String.Empty) : String.Empty;
                strQCType = !string.IsNullOrWhiteSpace(strQCType) ? Regex.Replace(strQCType, @"[^A-Z]+", String.Empty) : String.Empty;
                string strSampleIDColumnName = diSSColumnsToExportColumns[strSampleID];
                string strRunNoColumnName = !string.IsNullOrWhiteSpace(strRunNo) ? diSSColumnsToExportColumns[strRunNo] : String.Empty;
                string strParameterColumnName = !string.IsNullOrWhiteSpace(strParameter) ? diSSColumnsToExportColumns[strParameter] : string.Empty;
                string strQCTypColumnName = !string.IsNullOrWhiteSpace(strQCType) ? diSSColumnsToExportColumns[strQCType] : string.Empty;

                Worksheet worksheet = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets[int.Parse(drrSample[0]["SheetID"].ToString()) - 1];
                CellRange RanData = worksheet.GetDataRange();
                int intlastUsedRow = RanData.BottomRowIndex + 1;
                int intlastUsedColumn = RanData.RightColumnIndex + 1;
                CellRange range = worksheet.Range[string.Format("A{0}:{01}{02}", intStartRow, diIndexToColumn[intlastUsedColumn], intlastUsedRow)];
                bool rangeHasHeaders = false;
                dtDetailData = worksheet.CreateDataTable(range, false);

                DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dtDetailData, rangeHasHeaders);
                exporter.CellValueConversionError += Exporter_CellValueConversionError;
                exporter.Export();

                //Worksheet worksheetHeader = aSPxSpreadsheet.ASPxSpreadsheetControl.Document.Worksheets[int.Parse(drrSample[0]["SheetID"].ToString()) - 1];
                //CellRange RanDataHeader = worksheet.GetDataRange();

                //CellRange rangeHeader = worksheet.Range[string.Format("A{0}:{01}{02}", 1, diIndexToColumn[intlastUsedColumn], intStartRow - 1)];
                //bool rangeHasHeader = false;
                //DataTable dataTableheader = worksheet.CreateDataTable(rangeHeader, false);

                //DataTableExporter exporter1 = worksheet.CreateDataTableExporter(rangeHeader, dataTableheader, rangeHasHeader);
                //exporter1.CellValueConversionError += Exporter_CellValueConversionError;
                //exporter1.Export();

                DataRow row;
                dtHeaderData = new DataTable();
                foreach (DataRow dr in qcbatchinfo.dtHeader.Rows)
                {
                    var test = dr["PositionMapping"].ToString();
                    if (!string.IsNullOrEmpty(test))
                    {
                        Cell cell = worksheet.Cells[dr["PositionMapping"].ToString()];
                        if (cell != null && cell.Value != null && cell.Value.IsText == false && double.TryParse(worksheet.Cells[dr["PositionMapping"].ToString()].NumberFormat, out double dob))
                        //if(cell != null && cell.Value != null && cell.Value.IsText == false && !string.IsNullOrEmpty(cell.Value.ToString()))
                        //if (!string.IsNullOrEmpty(worksheet.Cells[dr["PositionMapping"].ToString()].NumberFormat) && double.TryParse(worksheet.Cells[dr["PositionMapping"].ToString()].NumberFormat, out double d))
                        {
                            dtHeaderData.Columns.Add(dr["CaptionMapping"].ToString(), typeof(double));
                        }
                        else
                        {
                            dtHeaderData.Columns.Add(dr["CaptionMapping"].ToString());
                        }
                    }

                }

                row = dtHeaderData.NewRow();
                foreach (DataRow dr in qcbatchinfo.dtHeader.Rows)
                {
                    var test = dr["PositionMapping"].ToString();
                    if (!string.IsNullOrEmpty(test))
                    {
                        Cell cell = worksheet.Cells[dr["PositionMapping"].ToString()];
                        if (cell.Value.TextValue != null)
                        {
                            string column = dr["CaptionMapping"].ToString();
                            row[column] = worksheet.Cells[dr["PositionMapping"].ToString()].Value.ToString();
                        }
                        else if (cell.Value.NumericValue != null && cell.Value.NumericValue > 0)
                        {
                            string column = dr["CaptionMapping"].ToString();
                            row[column] = worksheet.Cells[dr["PositionMapping"].ToString()].Value.ToString();
                        }
                    }
                }
                dtHeaderData.Rows.Add(row);

                foreach (DataRow dr in qcbatchinfo.dtDetail.Rows)
                {
                    int columindex = Convert.ToInt32(dr["Columnindex"].ToString());
                    string columnname = dr["ColumnMapping"].ToString().ToUpper();
                    dtDetailData.Columns[columindex].ColumnName = columnname;
                }

                if (dtHeaderData != null && dtHeaderData.Rows.Count > 0)
                {
                    foreach (DataColumn col in dtHeaderData.Columns)
                    {
                        string newColumnName = col.ColumnName;
                        while (dtDetailData.Columns.Contains(newColumnName))
                        {
                            newColumnName = string.Format("{0}", col.ColumnName);
                        }
                        dtDetailData.Columns.Add(newColumnName, col.DataType);
                    }
                }

                foreach (DataRow drHeader in dtHeaderData.Rows)
                {
                    foreach (DataColumn c in drHeader.Table.Columns)
                    {
                        foreach (DataRow rows in dtDetailData.Rows)
                        {
                            rows[rows.Table.Columns[c.ToString()].Ordinal] = drHeader[drHeader.Table.Columns[c.ToString()].Ordinal];
                        }
                    }
                }

                dtDetailDataNew = dtDetailData.Clone();
                int countformat = 0;
                foreach (DataRow dr in dtDetailData.Rows)
                {
                    object[] objData = dr.ItemArray;
                    for (int i = 0; i < objData.Length; i++)
                    {
                        DateTime date;
                        string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                         "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                         "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                         "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                         "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm","yyyy/M/d H:mm:ss", "yyyy/M/d HH:mm:ss"};
                        var val = objData[i].ToString();
                        if (countformat == 0 && DateTime.TryParseExact(val, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            if (countformat == 0)
                            {
                                dtDetailDataNew.Columns[i].DataType = typeof(DateTime);
                            }
                        }
                    }
                    dtDetailDataNew.Rows.Add(objData);
                    countformat = countformat + 1;
                }

                if (!dtDetailDataNew.Columns.Contains("ANALYTICALBATCHID"))
                {
                    dtDetailDataNew.Columns.Add("ANALYTICALBATCHID", typeof(string));
                    if (qcbatchinfo.dtsample.Rows.Count > 0)
                    {
                        dtDetailDataNew.Select("").ToList().ForEach(x => x["ANALYTICALBATCHID"] = qcbatchinfo.dtsample.Rows[0]["QCBATCHID"].ToString());
                    }
                }
                if (!dtDetailDataNew.Columns.Contains("uqAnalyticalBatchID"))
                {
                    dtDetailDataNew.Columns.Add("uqAnalyticalBatchID");
                    foreach (DataRow rows in dtDetailDataNew.Rows)
                    {
                        rows["uqAnalyticalBatchID"] = ABID;
                    }
                }

                DataTable dtsign = new DataTable();
                if (qcbatchinfo.dtsample.Rows.Count > 0)
                {
                    string stranalyzed = qcbatchinfo.dtsample.DefaultView.ToTable(true, "ANALYZEDBY").Rows[0][0].ToString();
                    dtsign = GetUserSign(stranalyzed);
                }

                if (!dtDetailDataNew.Columns.Contains("ANALYSTSIGN"))
                {
                    dtDetailDataNew.Columns.Add("ANALYSTSIGN", typeof(System.Drawing.Image));
                    if (dtsign != null && dtsign.Rows.Count > 0)
                    {
                        if (dtsign.Rows[0][0] != DBNull.Value)
                        {
                            Byte[] sign = (Byte[])dtsign.Rows[0][0];
                            if (sign != null && sign.Length > 0)
                            {
                                MemoryStream ms = new MemoryStream(sign);

                                dtDetailDataNew.Select("").ToList().ForEach(x => x["ANALYSTSIGN"] = new Bitmap(ms));
                            }
                        }
                    }
                }
                if (qcbatchinfo.dtsample.Rows.Count > 0)
                {
                    string strReviewedBy = qcbatchinfo.dtsample.DefaultView.ToTable(true, "ReviewedBy").Rows[0][0].ToString();
                    dtsign = GetUserSign(strReviewedBy);
                }


                if (!dtDetailDataNew.Columns.Contains("REVIEWEDSIGN"))
                {
                    dtDetailDataNew.Columns.Add("REVIEWEDSIGN", typeof(System.Drawing.Image));
                    if (dtsign != null && dtsign.Rows.Count > 0)
                    {
                        if (dtsign.Rows[0][0] != DBNull.Value)
                        {
                            Byte[] sign = (Byte[])dtsign.Rows[0][0];
                            if (sign != null && sign.Length > 0)
                            {
                                MemoryStream ms = new MemoryStream(sign);

                                dtDetailDataNew.Select("").ToList().ForEach(x => x["REVIEWEDSIGN"] = new Bitmap(ms));
                            }
                        }
                    }
                }

                if (qcbatchinfo.dtsample.Rows.Count > 0)
                {
                    string strVerifiedBy = qcbatchinfo.dtsample.DefaultView.ToTable(true, "VerifiedBy").Rows[0][0].ToString();
                    dtsign = GetUserSign(strVerifiedBy);
                }
                if (!dtDetailDataNew.Columns.Contains("VERIFIEDSIGN"))
                {
                    dtDetailDataNew.Columns.Add("VERIFIEDSIGN", typeof(System.Drawing.Image));
                    if (dtsign != null && dtsign.Rows.Count > 0)
                    {
                        if (dtsign.Rows[0][0] != DBNull.Value)
                        {
                            Byte[] sign = (Byte[])dtsign.Rows[0][0];
                            if (sign != null && sign.Length > 0)
                            {
                                MemoryStream ms = new MemoryStream(sign);

                                dtDetailDataNew.Select("").ToList().ForEach(x => x["VERIFIEDSIGN"] = new Bitmap(ms));
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

        private DataTable GetUserSign(string strUserName)
        {
            try
            {
                DataTable dtQuery = new DataTable();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand Command = new SqlCommand("GetUserSign", con))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.CommandTimeout = 0;
                        SqlParameter Param = new SqlParameter("@UserName", strUserName);
                        Command.Parameters.Add(Param);
                        SqlDataAdapter DBAdapter = new SqlDataAdapter(Command);
                        DBAdapter.Fill(dtQuery);
                    }
                }
                return dtQuery;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        private void intializeExportIndexColumns()
        {
            try
            {
                diSSColumnsToExportColumns.Add("A", "Column1");
                diSSColumnsToExportColumns.Add("B", "Column2");
                diSSColumnsToExportColumns.Add("C", "Column3");
                diSSColumnsToExportColumns.Add("D", "Column4");
                diSSColumnsToExportColumns.Add("E", "Column5");
                diSSColumnsToExportColumns.Add("F", "Column6");
                diSSColumnsToExportColumns.Add("G", "Column7");
                diSSColumnsToExportColumns.Add("H", "Column8");
                diSSColumnsToExportColumns.Add("I", "Column9");
                diSSColumnsToExportColumns.Add("J", "Column10");
                diSSColumnsToExportColumns.Add("K", "Column11");
                diSSColumnsToExportColumns.Add("L", "Column12");
                diSSColumnsToExportColumns.Add("M", "Column13");
                diSSColumnsToExportColumns.Add("N", "Column14");
                diSSColumnsToExportColumns.Add("O", "Column15");
                diSSColumnsToExportColumns.Add("P", "Column16");
                diSSColumnsToExportColumns.Add("Q", "Column17");
                diSSColumnsToExportColumns.Add("R", "Column18");
                diSSColumnsToExportColumns.Add("S", "Column19");
                diSSColumnsToExportColumns.Add("T", "Column20");
                diSSColumnsToExportColumns.Add("U", "Column21");
                diSSColumnsToExportColumns.Add("V", "Column22");
                diSSColumnsToExportColumns.Add("W", "Column23");
                diSSColumnsToExportColumns.Add("X", "Column24");
                diSSColumnsToExportColumns.Add("Y", "Column25");
                diSSColumnsToExportColumns.Add("Z", "Column26");
                diSSColumnsToExportColumns.Add("AA", "Column27");
                diSSColumnsToExportColumns.Add("AB", "Column28");
                diSSColumnsToExportColumns.Add("AC", "Column29");
                diSSColumnsToExportColumns.Add("AD", "Column30");
                diSSColumnsToExportColumns.Add("AE", "Column31");
                diSSColumnsToExportColumns.Add("AF", "Column32");
                diSSColumnsToExportColumns.Add("AG", "Column33");
                diSSColumnsToExportColumns.Add("AH", "Column34");
                diSSColumnsToExportColumns.Add("AI", "Column35");
                diSSColumnsToExportColumns.Add("AJ", "Column36");
                diSSColumnsToExportColumns.Add("AK", "Column37");
                diSSColumnsToExportColumns.Add("AL", "Column38");
                diSSColumnsToExportColumns.Add("AM", "Column39");
                diSSColumnsToExportColumns.Add("AN", "Column40");
                diSSColumnsToExportColumns.Add("AO", "Column41");
                diSSColumnsToExportColumns.Add("AP", "Column42");
                diSSColumnsToExportColumns.Add("AQ", "Column43");
                diSSColumnsToExportColumns.Add("AR", "Column44");
                diSSColumnsToExportColumns.Add("AS", "Column45");
                diSSColumnsToExportColumns.Add("AT", "Column46");
                diSSColumnsToExportColumns.Add("AU", "Column47");
                diSSColumnsToExportColumns.Add("AV", "Column48");
                diSSColumnsToExportColumns.Add("AW", "Column49");
                diSSColumnsToExportColumns.Add("AX", "Column50");
                diSSColumnsToExportColumns.Add("AY", "Column51");
                diSSColumnsToExportColumns.Add("AZ", "Column52");
                diSSColumnsToExportColumns.Add("BA", "Column53");
                diSSColumnsToExportColumns.Add("BB", "Column54");
                diSSColumnsToExportColumns.Add("BC", "Column55");
                diSSColumnsToExportColumns.Add("BD", "Column56");
                diSSColumnsToExportColumns.Add("BE", "Column57");
                diSSColumnsToExportColumns.Add("BF", "Column58");
                diSSColumnsToExportColumns.Add("BG", "Column59");
                diSSColumnsToExportColumns.Add("BH", "Column60");
                diSSColumnsToExportColumns.Add("BI", "Column61");
                diSSColumnsToExportColumns.Add("BJ", "Column62");
                diSSColumnsToExportColumns.Add("BK", "Column63");
                diSSColumnsToExportColumns.Add("BL", "Column64");
                diSSColumnsToExportColumns.Add("BM", "Column65");
                diSSColumnsToExportColumns.Add("BN", "Column66");
                diSSColumnsToExportColumns.Add("BO", "Column67");
                diSSColumnsToExportColumns.Add("BP", "Column68");
                diSSColumnsToExportColumns.Add("BQ", "Column69");
                diSSColumnsToExportColumns.Add("BR", "Column70");
                diSSColumnsToExportColumns.Add("BS", "Column71");
                diSSColumnsToExportColumns.Add("BT", "Column72");
                diSSColumnsToExportColumns.Add("BU", "Column73");
                diSSColumnsToExportColumns.Add("BV", "Column74");
                diSSColumnsToExportColumns.Add("BW", "Column75");
                diSSColumnsToExportColumns.Add("BX", "Column76");
                diSSColumnsToExportColumns.Add("BY", "Column77");
                diSSColumnsToExportColumns.Add("BZ", "Column78");
                diSSColumnsToExportColumns.Add("CA", "Column79");
                diSSColumnsToExportColumns.Add("CB", "Column80");
                diSSColumnsToExportColumns.Add("CC", "Column81");
                diSSColumnsToExportColumns.Add("CD", "Column82");
                diSSColumnsToExportColumns.Add("CE", "Column83");
                diSSColumnsToExportColumns.Add("CF", "Column84");
                diSSColumnsToExportColumns.Add("CG", "Column85");
                diSSColumnsToExportColumns.Add("CH", "Column86");
                diSSColumnsToExportColumns.Add("CI", "Column87");
                diSSColumnsToExportColumns.Add("CJ", "Column88");
                diSSColumnsToExportColumns.Add("CK", "Column89");
                diSSColumnsToExportColumns.Add("CL", "Column90");
                diSSColumnsToExportColumns.Add("CM", "Column91");
                diSSColumnsToExportColumns.Add("CN", "Column92");
                diSSColumnsToExportColumns.Add("CO", "Column93");
                diSSColumnsToExportColumns.Add("CP", "Column94");
                diSSColumnsToExportColumns.Add("CQ", "Column95");
                diSSColumnsToExportColumns.Add("CR", "Column96");
                diSSColumnsToExportColumns.Add("CS", "Column97");
                diSSColumnsToExportColumns.Add("CT", "Column98");
                diSSColumnsToExportColumns.Add("CU", "Column99");
                diSSColumnsToExportColumns.Add("CV", "Column100");
                diSSColumnsToExportColumns.Add("CW", "Column101");
                diSSColumnsToExportColumns.Add("CX", "Column102");
                diSSColumnsToExportColumns.Add("CY", "Column103");
                diSSColumnsToExportColumns.Add("CZ", "Column104");

                diSSColumnsToExportColumns.Add("DA", "Column105");
                diSSColumnsToExportColumns.Add("DB", "Column106");
                diSSColumnsToExportColumns.Add("DC", "Column107");
                diSSColumnsToExportColumns.Add("DD", "Column108");
                diSSColumnsToExportColumns.Add("DE", "Column109");
                diSSColumnsToExportColumns.Add("DF", "Column110");
                diSSColumnsToExportColumns.Add("DG", "Column111");
                diSSColumnsToExportColumns.Add("DH", "Column112");
                diSSColumnsToExportColumns.Add("DI", "Column113");
                diSSColumnsToExportColumns.Add("DJ", "Column114");
                diSSColumnsToExportColumns.Add("DK", "Column115");
                diSSColumnsToExportColumns.Add("DL", "Column116");
                diSSColumnsToExportColumns.Add("DM", "Column117");
                diSSColumnsToExportColumns.Add("DN", "Column118");
                diSSColumnsToExportColumns.Add("DO", "Column119");
                diSSColumnsToExportColumns.Add("DP", "Column120");
                diSSColumnsToExportColumns.Add("DQ", "Column121");
                diSSColumnsToExportColumns.Add("DR", "Column122");
                diSSColumnsToExportColumns.Add("DS", "Column123");
                diSSColumnsToExportColumns.Add("DT", "Column124");
                diSSColumnsToExportColumns.Add("DU", "Column125");
                diSSColumnsToExportColumns.Add("DV", "Column126");
                diSSColumnsToExportColumns.Add("DW", "Column127");
                diSSColumnsToExportColumns.Add("DX", "Column128");
                diSSColumnsToExportColumns.Add("DY", "Column129");
                diSSColumnsToExportColumns.Add("DZ", "Column130");

                // Index to Column

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
                diIndexToColumn.Add(79, "CA");
                diIndexToColumn.Add(80, "CB");
                diIndexToColumn.Add(81, "CC");
                diIndexToColumn.Add(82, "CD");
                diIndexToColumn.Add(83, "CE");
                diIndexToColumn.Add(84, "CF");
                diIndexToColumn.Add(85, "CG");
                diIndexToColumn.Add(86, "CH");
                diIndexToColumn.Add(87, "CI");
                diIndexToColumn.Add(88, "CJ");
                diIndexToColumn.Add(89, "CK");
                diIndexToColumn.Add(90, "CL");
                diIndexToColumn.Add(91, "CM");
                diIndexToColumn.Add(92, "CN");
                diIndexToColumn.Add(93, "CO");
                diIndexToColumn.Add(94, "CP");
                diIndexToColumn.Add(95, "CQ");
                diIndexToColumn.Add(96, "CR");
                diIndexToColumn.Add(97, "CS");
                diIndexToColumn.Add(98, "CT");
                diIndexToColumn.Add(99, "CU");
                diIndexToColumn.Add(100, "CV");
                diIndexToColumn.Add(101, "CW");
                diIndexToColumn.Add(102, "CX");
                diIndexToColumn.Add(103, "CY");
                diIndexToColumn.Add(104, "CZ");

                diIndexToColumn.Add(105, "DA");
                diIndexToColumn.Add(106, "DB");
                diIndexToColumn.Add(107, "DC");
                diIndexToColumn.Add(108, "DD");
                diIndexToColumn.Add(109, "DE");
                diIndexToColumn.Add(110, "DF");
                diIndexToColumn.Add(111, "DG");
                diIndexToColumn.Add(112, "DH");
                diIndexToColumn.Add(113, "DI");
                diIndexToColumn.Add(114, "DJ");
                diIndexToColumn.Add(115, "DK");
                diIndexToColumn.Add(116, "DL");
                diIndexToColumn.Add(117, "DM");
                diIndexToColumn.Add(118, "DN");
                diIndexToColumn.Add(119, "DO");
                diIndexToColumn.Add(120, "DP");
                diIndexToColumn.Add(121, "DQ");
                diIndexToColumn.Add(122, "DR");
                diIndexToColumn.Add(123, "DS");
                diIndexToColumn.Add(124, "DT");
                diIndexToColumn.Add(125, "DU");
                diIndexToColumn.Add(126, "DV");
                diIndexToColumn.Add(127, "DW");
                diIndexToColumn.Add(128, "DX");
                diIndexToColumn.Add(129, "DY");
                diIndexToColumn.Add(130, "DZ");

                diIndexToColumn.Add(131, "EA");
                diIndexToColumn.Add(132, "EB");
                diIndexToColumn.Add(133, "EC");
                diIndexToColumn.Add(134, "ED");
                diIndexToColumn.Add(135, "EE");
                diIndexToColumn.Add(136, "EF");
                diIndexToColumn.Add(137, "EG");
                diIndexToColumn.Add(138, "EH");
                diIndexToColumn.Add(139, "EI");
                diIndexToColumn.Add(140, "EJ");
                diIndexToColumn.Add(141, "EK");
                diIndexToColumn.Add(142, "EL");
                diIndexToColumn.Add(143, "EM");
                diIndexToColumn.Add(144, "EN");
                diIndexToColumn.Add(145, "EO");
                diIndexToColumn.Add(146, "EP");
                diIndexToColumn.Add(147, "EQ");
                diIndexToColumn.Add(148, "ER");
                diIndexToColumn.Add(149, "ES");
                diIndexToColumn.Add(150, "ET");
                diIndexToColumn.Add(151, "EU");
                diIndexToColumn.Add(152, "EV");
                diIndexToColumn.Add(153, "EW");
                diIndexToColumn.Add(154, "EX");
                diIndexToColumn.Add(155, "EY");
                diIndexToColumn.Add(156, "EZ");
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void InstrumentImportFromExcel(CollectionSourceBase list)
        {
            try
            {
                DateTime strDate = DateTime.Now;
                DataTable dtSample = qcbatchinfo.dtsample;
                Worksheet worksheet;
                DataTable dtInstrument = new DataTable();
                DataTable dtHeader = new DataTable();
                DataTable dtDetail = new DataTable();
                IWorkbook workbook = new Workbook();
                Dictionary<string, string> diHeaderColumns = new Dictionary<string, string>();
                Dictionary<string, string> diDetailColumns = new Dictionary<string, string>();
                string SheetName = string.Empty;
                DataRow[] drInstrument = qcbatchinfo.dtDataTransfer.Select("Name ='" + qcbatchinfo.strDataTransfer + "'");
                workbook.LoadDocument((byte[])drInstrument[0]["SpreadSheet"], DocumentFormat.OpenXml);
                for (int intSheetCount = 0; intSheetCount <= workbook.Worksheets.Count - 1; intSheetCount++)
                {
                    worksheet = workbook.Worksheets[intSheetCount];
                    SheetName = workbook.Worksheets[intSheetCount].Name;
                    CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                    CellRange HeaderRange = null;
                    CellRange DetailRange = null;
                    if (worksheet.DefinedNames.Contains("HEADERRANGE"))
                    {
                        HeaderRange = worksheet["HEADERRANGE"];
                    }
                    if (worksheet.DefinedNames.Contains("HEADERRANGE"))
                    {
                        DetailRange = worksheet["DETAILRANGE"];
                    }
                    int startRow = 0;
                    if (DetailRange != null)
                        startRow = DetailRange.TopRowIndex;

                    dtInstrument = worksheet.CreateDataTable(range, true);
                    for (int col = 0; col < range.ColumnCount; col++)
                    {
                        CellValueType cellType = range[0, col].Value.Type;
                        for (int r = 1; r < range.RowCount; r++)
                        {
                            if (cellType != range[r, col].Value.Type)
                            {
                                dtInstrument.Columns[col].DataType = typeof(string);
                                break;
                            }
                        }
                    }

                    DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dtInstrument, false);
                    exporter.Export();

                    int IndexNo = 1;
                    diHeaderColumns = new Dictionary<string, string>();
                    diDetailColumns = new Dictionary<string, string>();
                    if (dtInstrument != null && dtInstrument.Rows.Count > 0)
                    {
                        for (int iColumn = 0; iColumn < dtInstrument.Columns.Count; iColumn++)
                        {
                            int iRow = 1;
                            foreach (DataRow drRow in dtInstrument.Rows)
                            {
                                if (!string.IsNullOrEmpty(drRow[iColumn].ToString()) && drRow[iColumn].ToString().Contains("[") && drRow[iColumn].ToString().Contains("]"))
                                {
                                    string strColumnName = drRow[iColumn].ToString().Trim('[', ']');
                                    if (HeaderRange != null && HeaderRange.BottomRowIndex >= iRow - 1)
                                    {
                                        if (diHeaderColumns.ContainsKey(strColumnName))
                                        {
                                            strColumnName = strColumnName + IndexNo;
                                            diHeaderColumns.Add(strColumnName, diIndexToColumn[iColumn + 1] + iRow.ToString());
                                            IndexNo = IndexNo + 1;
                                        }
                                        else
                                        {
                                            diHeaderColumns.Add(strColumnName, diIndexToColumn[iColumn + 1] + iRow.ToString());
                                        }
                                    }
                                    if (DetailRange != null && DetailRange.BottomRowIndex <= iRow - 1 && DetailRange.TopRowIndex <= iRow - 1)
                                    {
                                        if (diDetailColumns.ContainsKey(strColumnName))
                                        {
                                        }
                                        else
                                        {
                                            diDetailColumns.Add(strColumnName, diIndexToColumn[iColumn + 1] + iRow.ToString());
                                        }
                                    }
                                }
                                iRow += 1;
                            }
                        }
                    }

                    foreach (SDMSDCImport import in list.List)
                    {
                        string strExtension = Path.GetExtension(import.FileName.ToString());
                        DevExpress.Spreadsheet.IWorkbook xlWorkBook = new Workbook();

                        if (Path.GetExtension(import.FileName.ToString()).ToUpper() == ".TXT")
                        {
                            //Random objRandom = new Random();
                            //string strFileName = Path.GetFileNameWithoutExtension(import.FileName.ToString());
                            //if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\SDMSResultEntry")) == false)
                            //{
                            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\SDMSResultEntry"));
                            //}
                            //string strDownloadPath = HttpContext.Current.Server.MapPath(@"~\SDMSResultEntry\" + objRandom.Next() + "_" + strFileName + ".xls");
                            //object m_objOpt = System.Reflection.Missing.Value;
                            //Excel.Application m_objExcel = null;
                            //Excel._Workbook m_objBook = null;
                            //Excel.Workbooks m_objBooks = null;
                            //m_objExcel = new Excel.Application();
                            //m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
                            //m_objBooks.OpenText(import.FileName, Excel.XlPlatform.xlWindows, 1,
                            //Excel.XlTextParsingType.xlDelimited, Excel.XlTextQualifier.xlTextQualifierDoubleQuote,
                            //true, true, false, false, true, false, m_objOpt, m_objOpt,
                            //m_objOpt, m_objOpt, m_objOpt);
                            //m_objBook = m_objExcel.ActiveWorkbook;
                            //m_objBook.SaveAs(strDownloadPath, Excel.XlFileFormat.xlWorkbookNormal,
                            //m_objOpt, m_objOpt, m_objOpt, m_objOpt, Excel.XlSaveAsAccessMode.xlNoChange, m_objOpt, m_objOpt,
                            //m_objOpt, m_objOpt);
                            //m_objBook.Close(false, m_objOpt, m_objOpt);
                            //m_objExcel.Quit();
                            //xlWorkBook.LoadDocument(strDownloadPath, DocumentFormat.Undefined);
                            //System.IO.File.Delete(strDownloadPath);
                        }

                        else if (Path.GetExtension(import.FileName.ToString()).ToUpper() == ".XLS")
                        {
                            xlWorkBook.LoadDocument(import.Data, DocumentFormat.Xls);
                        }
                        else if (Path.GetExtension(import.FileName.ToString()).ToUpper() == ".XLSX")
                        {
                            xlWorkBook.LoadDocument(import.Data, DocumentFormat.Xlsx);
                        }
                        else
                        {
                            xlWorkBook.LoadDocument(import.Data, DocumentFormat.Undefined);
                        }
                        //if (xlWorkBook.LoadDocument(import.Data, DocumentFormat.Undefined)==false)
                        //{

                        //}
                        Worksheet xlWorkSheet;
                        if (xlWorkBook.Worksheets.Contains(SheetName))
                        {
                            xlWorkSheet = xlWorkBook.Worksheets[SheetName];
                        }
                        else if (xlWorkBook.Worksheets.Count > intSheetCount)
                        {
                            xlWorkSheet = xlWorkBook.Worksheets[intSheetCount];
                        }
                        else
                        {
                            xlWorkSheet = xlWorkBook.Worksheets[0];
                        }

                        CellRange Importrange = xlWorkSheet.Range.FromLTRB(0, startRow, xlWorkSheet.GetUsedRange().ColumnCount + 1, xlWorkSheet.GetUsedRange().BottomRowIndex);
                        DataTable dtTemp = new DataTable();
                        dtTemp = xlWorkSheet.CreateDataTable(Importrange, false);
                        for (int col = 0; col < Importrange.ColumnCount; col++)
                        {
                            CellValueType cellType = Importrange[0, col].Value.Type;
                            for (int r = 1; r < Importrange.RowCount; r++)
                            {
                                if (cellType != Importrange[r, col].Value.Type)
                                {
                                    dtTemp.Columns[col].DataType = typeof(string);
                                    break;
                                }
                            }
                        }
                        DataTableExporter exportTable = xlWorkSheet.CreateDataTableExporter(Importrange, dtTemp, false);


                        if ((qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"]).ToString() == "Geosmin" || (qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"]).ToString() == "TPH 1005")
                        {
                            DataTable dtImport = new DataTable();
                            if ((qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"]).ToString() == "Geosmin")
                            {
                                dtImport = FillDataFromVOC(import);
                            }
                            else if ((qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"]).ToString() == "TPH 1005")
                            {
                                dtImport = FillDataFromTPH(import);
                            }
                            IWorkbook workbook1 = new Workbook();
                            Worksheet worksheet1 = workbook1.Worksheets[0];
                            worksheet1.Import(dtImport, true, 0, 0);
                            Importrange = worksheet1.Range.FromLTRB(0, startRow, worksheet1.GetUsedRange().ColumnCount + 1, worksheet1.GetUsedRange().BottomRowIndex);
                            dtTemp = worksheet1.CreateDataTable(Importrange, false);
                            for (int col = 0; col < Importrange.ColumnCount; col++)
                            {
                                CellValueType cellType = Importrange[0, col].Value.Type;
                                for (int r = 1; r < Importrange.RowCount; r++)
                                {
                                    if (cellType != Importrange[r, col].Value.Type)
                                    {
                                        dtTemp.Columns[col].DataType = typeof(string);
                                        break;
                                    }
                                }
                            }
                            exportTable = worksheet1.CreateDataTableExporter(Importrange, dtTemp, false);
                        }

                        exportTable.CellValueConversionError += Exporter_CellValueConversionError;
                        exportTable.Export();
                        if ((qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"]).ToString() == "Anions" && intSheetCount == 1)
                        {
                            if (!diDetailColumns.ContainsKey("USERDEFINED1"))
                                diDetailColumns.Add("USERDEFINED1", "A5");
                        }

                        ////if (qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"].ToString() == "Metals")
                        ////{
                        ////    if (dtTemp.Columns.Contains("Column1") && dtTemp.Columns.Contains("Column4"))
                        ////    {
                        ////        dtTemp.DefaultView.RowFilter = "Column1='Average' and Column4='Concentration'";
                        ////        dtTemp = dtTemp.DefaultView.ToTable();
                        ////    }
                        ////}
                        DataTable dtParameterCount = new DataTable();
                        dtParameterCount.Columns.Add("Parameter", typeof(string));
                        string sValue;
                        Dictionary<string, string> diIntrumentValues = new Dictionary<string, string>();
                        if (diHeaderColumns != null && diHeaderColumns.Count > 0)
                        {
                            foreach (string sKey in diHeaderColumns.Keys)
                            {
                                if (String.IsNullOrEmpty(xlWorkSheet.Cells[diHeaderColumns[sKey]].DisplayText) || xlWorkSheet.Cells[diHeaderColumns[sKey]].DisplayText == "#DIV/0!")
                                    sValue = string.Empty;
                                else
                                {
                                    sValue = xlWorkSheet.Cells[diHeaderColumns[sKey]].DisplayText;
                                    if (sKey == "SAMPLEID")
                                    {
                                        sValue = sValue.Replace("样品名称:", "").Replace("Sample Name:", "").Replace("-", ".").Replace("#", "").Trim();
                                    }
                                }
                                diIntrumentValues.Add(sKey, sValue.Trim());
                                if (sKey.StartsWith("PARAMETER"))
                                {
                                    dtParameterCount.Rows.Add(sValue);
                                }
                            }
                        }
                        int intdisCoun = dtParameterCount.DefaultView.ToTable(true, "Parameter").Rows.Count;
                        int intstart = 0;
                        int intcond = 0;
                        DataRow[] drrImport = null;
                        DataRow[] drrQCImport = null;
                        if (dtTemp != null)
                        {
                            DataTable dtQCType = GetQCTypesByTestMethod(qcbatchinfo.OidTestMethod.ToString());
                            DataTable dtQcmatch = GetQCTypeMatch();
                            DataTable dtParamMatch = GetParameterMatch();
                            if (diHeaderColumns != null && diHeaderColumns.ContainsKey("PARAMETER"))
                            {
                                int inti = 0;
                                foreach (string sKey1 in diDetailColumns.Keys)
                                {
                                    if (sKey1 != "SAMPLEID" && sKey1 != "RUNNO" && sKey1 != "PARAMETER")
                                    {
                                        inti += 1;
                                        DataTable dtQcCount = new DataTable();
                                        dtQcCount.Columns.Add("QcType", typeof(string));
                                        int intst = intstart;
                                        foreach (DataRow dr in dtTemp.Rows)
                                        {
                                            intstart = intst;
                                            intcond = 0;
                                            string strSAMPLEID = string.Empty;
                                            string strMiscAnions = string.Empty;
                                            string strDilution = string.Empty;
                                            if (dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["SAMPLEID"], @"[^A-Z]+", String.Empty)]] != null && dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["SAMPLEID"], @"[^A-Z]+", String.Empty)]].ToString().Trim().Length > 0)
                                            {
                                                strSAMPLEID = dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["SAMPLEID"], @"[^A-Z]+", String.Empty)]].ToString().Trim().Replace("#", "");
                                            }
                                            if ((qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"]).ToString() == "Anions")
                                            {
                                                string[] strAnionsSampleID = strSAMPLEID.Split(';');
                                                string[] strAnionsDilution = strSAMPLEID.Split('/');
                                                if (strAnionsSampleID.Length > 1)
                                                {
                                                    strSAMPLEID = strAnionsSampleID[0];
                                                    strMiscAnions = strAnionsSampleID[1];
                                                    if (strAnionsDilution.Length > 0)
                                                    {
                                                        strDilution = strAnionsDilution[strAnionsDilution.Length - 1].Replace("X", "").Trim();
                                                    }
                                                }
                                            }
                                            string strQcType = string.Empty;
                                            strQcType = strSAMPLEID;
                                            foreach (DataRow drQCmatch in dtQcmatch.Rows)
                                            {
                                                string str = drQCmatch["MQcType"].ToString();
                                                if (str == strSAMPLEID)
                                                {
                                                    strQcType = drQCmatch["QcType"].ToString();
                                                    dtQcCount.Rows.Add(strQcType);
                                                    string count = dtQcCount.Select("QcType='" + strQcType + "'").Length.ToString();
                                                    strSAMPLEID = strQcType + count;
                                                    break;
                                                }
                                            }
                                            string strParam = string.Empty;
                                            foreach (string sKey in diIntrumentValues.Keys)
                                            {
                                                strParam = diIntrumentValues[sKey].Trim();
                                                strParam = Regex.Replace(strParam, @"\t|\n|\r", " ");
                                                DataRow[] drrFilter = null;
                                                drrFilter = dtParamMatch.Select("MParameter= '" + strParam + "'");
                                                if (drrFilter != null && drrFilter.Length > 0)
                                                {
                                                    foreach (DataRow drParam in drrFilter)
                                                    {
                                                        DataRow[] drr = null;
                                                        drr = dtSample.Select("PARAMETER= '" + drParam["Parameter"] + "'");
                                                        if (drr != null && drr.Length > 0)
                                                        {
                                                            strParam = drParam["Parameter"].ToString();
                                                            break;
                                                        }
                                                    }
                                                }

                                                string strQC = Regex.Replace(strSAMPLEID.ToUpper(), @"[^A-Z]", "");
                                                string strInsSampleID = Regex.Replace(strSAMPLEID.ToUpper(), @"[A-Z]", "");
                                                DataRow[] drQc = dtQCType.Select("QCType ='" + strQC + "' and (Source<>'' and Source<>'LCS' and Source <> 'Method Blank')");
                                                if (drQc != null && drQc.Length > 0)
                                                {
                                                    if ((drQc[0]["Source"]).ToString() == "Sample")
                                                    {
                                                        DataRow[] drSystem = dtSample.Select("SAMPLEID = '" + strInsSampleID + "' and QCTYPE = '" + strQC + "'");
                                                        if (drSystem.Length > 0)
                                                        {
                                                            strSAMPLEID = drSystem[0]["SYSTEMID"].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        DataRow[] drSystem = dtSample.Select("SYSSAMPLECODE LIKE '" + strInsSampleID.Replace(".", "-") + "%' and QCTYPE = '" + strQC + "'and SAMPLEID like '" + drQc[0]["Source"] + "%'");
                                                        if (drSystem.Length > 0)
                                                        {
                                                            strSAMPLEID = drSystem[0]["SYSTEMID"].ToString();
                                                        }
                                                    }
                                                }

                                                if (diHeaderColumns != null && diDetailColumns != null && diHeaderColumns.ContainsKey("PARAMETER") && diDetailColumns.ContainsKey("RUNNO") && diDetailColumns.ContainsKey("SAMPLEID"))
                                                {
                                                    drrImport = dtSample.Select(string.Format("RUNTYPE ='SAMPLE'  AND SAMPLEID = '{0}' AND RUNNO = '{1}' AND PARAMETER = '{2}'", strSAMPLEID, dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["RUNNO"], @"[^A-Z]+", String.Empty)]], strParam), "");
                                                    drrQCImport = dtSample.Select(string.Format("RUNTYPE <> 'SAMPLE' AND SYSTEMID = '{0}' AND RUNNO = '{1}' AND PARAMETER = '{2}'", strSAMPLEID, dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["RUNNO"], @"[^A-Z]+", String.Empty)]], strParam), "");
                                                }
                                                else if (diHeaderColumns != null && diDetailColumns != null && diDetailColumns.ContainsKey("SAMPLEID") && diHeaderColumns.ContainsKey("PARAMETER") && strDilution.Length > 0)
                                                {
                                                    drrImport = dtSample.Select(string.Format("RUNTYPE ='SAMPLE' AND SAMPLEID = '{0}' AND PARAMETER = '{1}' AND DILUTION = '{2}'", strSAMPLEID, strParam, strDilution), "");
                                                    drrQCImport = dtSample.Select(string.Format("RUNTYPE <> 'SAMPLE' AND SYSTEMID = '{0}' AND PARAMETER = '{1}'", strSAMPLEID, strParam), "");
                                                }
                                                else if (diHeaderColumns != null && diDetailColumns != null && diDetailColumns.ContainsKey("SAMPLEID") && diHeaderColumns.ContainsKey("PARAMETER"))
                                                {
                                                    drrImport = dtSample.Select(string.Format("RUNTYPE ='SAMPLE' AND SAMPLEID = '{0}' AND PARAMETER = '{1}'", strSAMPLEID, strParam), "");
                                                    drrQCImport = dtSample.Select(string.Format("RUNTYPE <> 'SAMPLE' AND SYSTEMID = '{0}' AND PARAMETER = '{1}'", strSAMPLEID, strParam), "");
                                                }

                                                if (intstart <= intcond && intcond < (intdisCoun * inti))
                                                {
                                                    if (drrImport != null && drrImport.Length > 0)
                                                    {
                                                        string strResult = string.Empty;
                                                        foreach (DataRow drSelected in drrImport)
                                                        {
                                                            if (dr[diSSColumnsToExportColumns[Regex.Replace(diHeaderColumns[sKey], @"[^A-Z]+", String.Empty)]] != null && dr[diSSColumnsToExportColumns[Regex.Replace(diHeaderColumns[sKey], @"[^A-Z]+", String.Empty)]].ToString().Trim().Length > 0)
                                                            {
                                                                strResult = dr[diSSColumnsToExportColumns[Regex.Replace(diHeaderColumns[sKey], @"[^A-Z]+", String.Empty)]].ToString().Trim();
                                                            }
                                                            if (strResult != null && strResult.Length > 0)
                                                            {
                                                                drSelected[sKey1] = strResult;
                                                            }
                                                            else
                                                            {
                                                                drSelected[sKey1] = DBNull.Value;
                                                            }
                                                        }
                                                    }

                                                    if (drrQCImport != null && drrQCImport.Length > 0)
                                                    {
                                                        string strResult = string.Empty;
                                                        foreach (DataRow drSelected in drrQCImport)
                                                        {
                                                            if (dr[diSSColumnsToExportColumns[Regex.Replace(diHeaderColumns[sKey], @"[^A-Z]+", String.Empty)]] != null && dr[diSSColumnsToExportColumns[Regex.Replace(diHeaderColumns[sKey], @"[^A-Z]+", String.Empty)]].ToString().Trim().Length > 0)
                                                            {
                                                                strResult = dr[diSSColumnsToExportColumns[Regex.Replace(diHeaderColumns[sKey], @"[^A-Z]+", String.Empty)]].ToString().Trim();
                                                            }
                                                            if (strResult != null && strResult.Length > 0)
                                                            {
                                                                drSelected[sKey1] = strResult;
                                                            }
                                                            else
                                                            {
                                                                drSelected[sKey1] = DBNull.Value;
                                                            }
                                                        }
                                                    }
                                                }
                                                intcond += 1;
                                            }
                                        }
                                        intstart = intst;
                                        intstart += intdisCoun;
                                    }
                                }
                            }
                            else
                            {
                                DataTable dtQcCount = new DataTable();
                                dtQcCount.Columns.Add("QcType", typeof(string));
                                foreach (DataRow dr in dtTemp.Rows)
                                {
                                    string strParam = string.Empty;
                                    if (diDetailColumns != null && diDetailColumns.ContainsKey("PARAMETER"))
                                    {
                                        if (dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["PARAMETER"], @"[^A-Z]+", String.Empty)]] != null)
                                        {
                                            strParam = dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["PARAMETER"], @"[^A-Z]+", String.Empty)]].ToString();
                                            if (strParam != null)
                                            {
                                                DataRow[] drrFilter = null;
                                                drrFilter = dtParamMatch.Select("MParameter= '" + strParam + "'");
                                                if (drrFilter != null && drrFilter.Length > 0)
                                                {
                                                    foreach (DataRow drParam in drrFilter)
                                                    {
                                                        DataRow[] drr = null;
                                                        drr = dtSample.Select("PARAMETER= '" + drParam["Parameter"] + "'");
                                                        if (drr != null && drr.Length > 0)
                                                        {
                                                            strParam = drParam["Parameter"].ToString();
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    string strSAMPLEID = string.Empty;
                                    string strQcType = string.Empty;
                                    string strDilution = string.Empty;
                                    if (diDetailColumns != null && diDetailColumns.ContainsKey("SAMPLEID"))
                                    {
                                        if (dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["SAMPLEID"], @"[^A-Z]+", String.Empty)]] != null && dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["SAMPLEID"], @"[^A-Z]+", String.Empty)]].ToString().Trim().Length > 0)
                                        {
                                            strSAMPLEID = dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["SAMPLEID"], @"[^A-Z]+", String.Empty)]].ToString().Trim().Replace("#", "");
                                        }
                                        if ((qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"]).ToString() == "Anions")
                                        {
                                            string[] strAnionsSampleID = strSAMPLEID.Split(';');
                                            string[] strAnionsDilution = strSAMPLEID.Split('/');
                                            if (strAnionsSampleID.Length > 1)
                                            {
                                                strSAMPLEID = strAnionsSampleID[0];
                                                //strMiscAnions = strAnionsSampleID[1];
                                                if (strAnionsDilution.Length > 0)
                                                {
                                                    strDilution = strAnionsDilution[strAnionsDilution.Length - 1].Replace("X", "").Trim();
                                                }
                                            }
                                        }

                                        if (strSAMPLEID != null)
                                        {
                                            strQcType = strSAMPLEID;
                                            DataRow[] drq = dtSample.Select("QCTYPE= '" + strSAMPLEID + "'");
                                            //foreach (DataRow drQCmatch in dtQcmatch.Rows)
                                            //{
                                            //    string str = drQCmatch["MQcType"].ToString();
                                            //    if (str == strSAMPLEID)
                                            //    {
                                            //        strQcType = drQCmatch["QcType"].ToString();
                                            //        dtQcCount.Rows.Add(strQcType);
                                            //        string count = dtQcCount.Select("QcType='" + strQcType + "'").Length.ToString();
                                            //        strQcType = strQcType + count;
                                            //    }
                                            //}
                                        }
                                    }

                                    //
                                    string strSysSampleID = string.Empty;
                                    if (diDetailColumns != null && diDetailColumns.ContainsKey("SYSSAMPLECODE"))
                                    {
                                        if (dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["SYSSAMPLECODE"], @"[^A-Z]+", String.Empty)]] != null && dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["SYSSAMPLECODE"], @"[^A-Z]+", String.Empty)]].ToString().Trim().Length > 0)
                                        {
                                            strSysSampleID = dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["SYSSAMPLECODE"], @"[^A-Z]+", String.Empty)]].ToString().Trim().Replace(".", "-").Replace("#", "");
                                        }
                                        if (strSysSampleID != null)
                                        {
                                            strQcType = strSysSampleID;
                                            DataRow[] drq = dtSample.Select("QCTYPE= '" + strSysSampleID + "'");
                                            foreach (DataRow drQCmatch in dtQcmatch.Rows)
                                            {
                                                string str = drQCmatch["MQcType"].ToString();
                                                if (str == strSysSampleID)
                                                {
                                                    strQcType = drQCmatch["QcType"].ToString();
                                                    dtQcCount.Rows.Add(strQcType);
                                                    string count = dtQcCount.Select("QcType='" + strQcType + "'").Length.ToString();
                                                    strQcType = strQcType + count;
                                                }
                                            }
                                        }
                                    }
                                    if (diIntrumentValues.ContainsKey("SAMPLEID"))
                                    {
                                        string strQc = Regex.Replace(diIntrumentValues["SAMPLEID"].ToString().ToUpper(), @"[^A-Z]", "");
                                        string strSampleID = Regex.Replace(diIntrumentValues["SAMPLEID"].ToString().ToUpper(), @"[A-Z]", "");
                                        DataRow[] drQc = dtQCType.Select("QCType ='" + strQc + "' and (Source<>'' and Source<>'LCS' and Source <> 'Method Blank')");
                                        if (drQc != null && drQc.Length > 0)
                                        {
                                            if ((drQc[0]["Source"]).ToString() == "Sample")
                                            {
                                                DataRow[] drSystem = dtSample.Select("SAMPLEID = '" + strSampleID + "' and QCTYPE = '" + strQc + "'");
                                                if (drSystem.Length > 0)
                                                {
                                                    diIntrumentValues["SAMPLEID"] = drSystem[0]["SYSTEMID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                DataRow[] drSystem = dtSample.Select("SYSSAMPLECODE LIKE '" + strSampleID.Replace(".", "-") + "%' and QCTYPE = '" + strQc + "'and SAMPLEID like '" + drQc[0]["Source"] + "%'");
                                                if (drSystem.Length > 0)
                                                {
                                                    diIntrumentValues["SAMPLEID"] = drSystem[0]["SYSTEMID"].ToString();
                                                }
                                            }
                                        }
                                    }
                                    else if (strQcType.Length > 0)
                                    {
                                        string strQc = Regex.Replace(strQcType.ToUpper(), @"[^A-Z]", "");
                                        string strSampleID = Regex.Replace(strQcType.ToUpper(), @"[A-Z]", "");
                                        DataRow[] drQc = dtQCType.Select("QCType ='" + strQc + "' and (Source<>'' and Source<>'LCS' and Source <> 'Method Blank')");
                                        if (drQc != null && drQc.Length > 0)
                                        {
                                            if ((drQc[0]["Source"]).ToString() == "Sample")
                                            {
                                                DataRow[] drSystem = dtSample.Select("SAMPLEID = '" + strSampleID + "' and QCTYPE = '" + strQc + "'");
                                                if (drSystem.Length > 0)
                                                {
                                                    strQcType = drSystem[0]["SYSTEMID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                DataRow[] drSystem = dtSample.Select("SYSSAMPLECODE LIKE '" + strSampleID.Replace(".", "-") + "%' and QCTYPE = '" + strQc + "'and SAMPLEID like '" + drQc[0]["Source"] + "%'");
                                                if (drSystem.Length > 0)
                                                {
                                                    strQcType = drSystem[0]["SYSTEMID"].ToString();
                                                }
                                            }
                                        }
                                    }
                                    if (diDetailColumns != null && diDetailColumns.ContainsKey("DILUTION"))
                                    {
                                        if (dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["DILUTION"], @"[^A-Z]+", String.Empty)]] != null && dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["DILUTION"], @"[^A-Z]+", String.Empty)]].ToString().Trim().Length > 0)
                                        {
                                            strDilution = dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["DILUTION"], @"[^A-Z]+", String.Empty)]].ToString().Trim().Replace("#", "");
                                        }
                                    }
                                    //
                                    if (diHeaderColumns != null && diDetailColumns != null && diHeaderColumns.ContainsKey("SAMPLEID") && diHeaderColumns.ContainsKey("RUNNO") && diDetailColumns.ContainsKey("PARAMETER"))
                                    {
                                        drrImport = dtSample.Select(string.Format("RUNTYPE ='SAMPLE' AND SAMPLEID = '{0}' AND RUNNO = '{1}' AND PARAMETER = '{2}'", diIntrumentValues["SAMPLEID"], diIntrumentValues["RUNNO"], strParam), "");
                                        drrQCImport = dtSample.Select(string.Format("RUNTYPE <> 'SAMPLE' AND SYSTEMID = '{0}' AND PARAMETER = '{1}'", diIntrumentValues["SAMPLEID"], strParam), "");
                                    }
                                    else if (diHeaderColumns != null && diDetailColumns != null && diHeaderColumns.ContainsKey("SAMPLEID") && diDetailColumns.ContainsKey("PARAMETER"))
                                    {
                                        drrImport = dtSample.Select(string.Format("RUNTYPE ='SAMPLE' AND SAMPLEID = '{0}' AND PARAMETER = '{1}'", diIntrumentValues["SAMPLEID"], strParam), "");
                                        drrQCImport = dtSample.Select(string.Format("RUNTYPE <> 'SAMPLE' AND SYSTEMID = '{0}' AND PARAMETER = '{1}'", diIntrumentValues["SAMPLEID"], strParam), "");
                                    }
                                    else if (diDetailColumns != null && diDetailColumns.ContainsKey("SAMPLEID") && diDetailColumns.ContainsKey("PARAMETER") && diDetailColumns.ContainsKey("RUNNO"))
                                    {
                                        drrImport = dtSample.Select(string.Format("RUNTYPE ='SAMPLE' AND SAMPLEID = '{0}' AND PARAMETER = '{1}' AND RUNNO = '{2}'", strSAMPLEID, strParam, dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns["RUNNO"], @"[^A-Z]+", String.Empty)]]), "");
                                        drrQCImport = dtSample.Select(string.Format("RUNTYPE <> 'SAMPLE' AND SYSTEMID = '{0}' AND PARAMETER = '{1}' ", strQcType, strParam), "");
                                    }
                                    else if (diDetailColumns != null && diDetailColumns.ContainsKey("SAMPLEID") && diDetailColumns.ContainsKey("PARAMETER") && diDetailColumns.ContainsKey("DILUTION"))
                                    {
                                        drrImport = dtSample.Select(string.Format("RUNTYPE ='SAMPLE' AND SAMPLEID = '{0}' AND PARAMETER = '{1}' AND DILUTION ='{2}'", strSAMPLEID, strParam, strDilution), "");
                                        drrQCImport = dtSample.Select(string.Format("RUNTYPE <> 'SAMPLE' AND SYSTEMID = '{0}' AND PARAMETER = '{1}' ", strQcType, strParam), "");
                                    }
                                    else if (diDetailColumns != null && diDetailColumns.ContainsKey("SAMPLEID") && diDetailColumns.ContainsKey("PARAMETER"))
                                    {
                                        drrImport = dtSample.Select(string.Format("RUNTYPE ='SAMPLE' AND SAMPLEID = '{0}' AND PARAMETER = '{1}'", strSAMPLEID, strParam), "");
                                        drrQCImport = dtSample.Select(string.Format("RUNTYPE <> 'SAMPLE' AND SYSTEMID = '{0}' AND PARAMETER = '{1}' ", strQcType, strParam), "");
                                    }
                                    else if (diDetailColumns != null && diDetailColumns.ContainsKey("SAMPLEID"))
                                    {
                                        drrImport = dtSample.Select(string.Format("RUNTYPE ='SAMPLE' AND SAMPLEID = '{0}'", strSAMPLEID), "");
                                        drrQCImport = dtSample.Select(string.Format("RUNTYPE <> 'SAMPLE' AND SYSTEMID = '{0}'", strQcType), "");
                                    }
                                    else if (diDetailColumns != null && diDetailColumns.ContainsKey("SYSSAMPLECODE"))
                                    {
                                        drrImport = dtSample.Select(string.Format("RUNTYPE ='SAMPLE' AND SYSSAMPLECODE = '{0}'", strSysSampleID), "");
                                        drrQCImport = dtSample.Select(string.Format("RUNTYPE <> 'SAMPLE' AND SYSSAMPLECODE = '{0}'", strQcType), "");
                                    }
                                    if (drrImport != null && drrImport.Length > 0)
                                    {
                                        foreach (DataRow drSelected in drrImport)
                                        {
                                            if (diHeaderColumns != null && diHeaderColumns.Count > 0)
                                            {
                                                foreach (string sKey in diHeaderColumns.Keys)
                                                {
                                                    if (sKey != "SAMPLEID" && sKey != "RUNNO" && sKey != "PARAMETER")
                                                    {
                                                        drSelected[sKey] = diIntrumentValues[sKey];
                                                    }
                                                }
                                            }
                                            if (diDetailColumns != null && diDetailColumns.Count > 0)
                                            {
                                                foreach (string sKey in diDetailColumns.Keys)
                                                {
                                                    if (sKey != "SAMPLEID" && sKey != "RUNNO" && sKey != "PARAMETER" && sKey != "SYSSAMPLECODE")
                                                    {
                                                        drSelected[sKey] = dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns[sKey], @"[^A-Z]+", String.Empty)]];
                                                    }
                                                    if ((qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"]).ToString() == "Anions")
                                                    {
                                                        string strMiscWithSampleID = Regex.Replace(drSelected[sKey].ToString(), @"[^A-Z]+", String.Empty);
                                                        if (strMiscWithSampleID == "ICV" || strMiscWithSampleID == "CCB" || strMiscWithSampleID == "CCV")
                                                        {
                                                            if ((qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"]).ToString() == "Liquid")
                                                            {
                                                                drSelected["VolumeUsed"] = 1;
                                                            }
                                                            else
                                                            {
                                                                drSelected["VolumeUsed"] = 4;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (drSelected[sKey].ToString().Contains(';'))
                                                            {
                                                                //22120498.01; 1ML - 1ML / 1X
                                                                string[] strSplit = drSelected[sKey].ToString().Split(';');
                                                                if (strSplit != null && strSplit.Length > 0)
                                                                {
                                                                    //1ML - 1ML / 1X
                                                                    string strMisc = strSplit[1].ToString().Trim();
                                                                    string[] strVolumeSplit = strMisc.Split('-');
                                                                    if (strVolumeSplit != null && strVolumeSplit.Length > 0)
                                                                    {
                                                                        //1ML                                               
                                                                        string strSampleVolume = Regex.Replace(strVolumeSplit[0], @"[^\d]", "");
                                                                        drSelected["VolumeUsed"] = strSampleVolume;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSelected["VolumeUsed"] = 1;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    drSelected["VolumeUsed"] = 1;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                drSelected["VolumeUsed"] = 1;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (drrQCImport != null && drrQCImport.Length > 0)
                                    {
                                        foreach (DataRow drSelected in drrQCImport)
                                        {
                                            if (diHeaderColumns != null && diHeaderColumns.Count > 0)
                                            {
                                                foreach (string sKey in diHeaderColumns.Keys)
                                                {
                                                    if (sKey != "SAMPLEID" && sKey != "RUNNO" && sKey != "PARAMETER")
                                                    {
                                                        drSelected[sKey] = diIntrumentValues[sKey];
                                                    }
                                                }
                                            }
                                            if (diDetailColumns != null && diDetailColumns.Count > 0)
                                            {
                                                foreach (string sKey in diDetailColumns.Keys)
                                                {
                                                    if (sKey != "SAMPLEID" && sKey != "RUNNO" && sKey != "PARAMETER")
                                                    {
                                                        if ((qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"]).ToString() == "Anions" && drSelected[sKey].GetType().ToString() == "System.DateTime")
                                                        {
                                                            DateTime parsedDate;
                                                            if (dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns[sKey], @"[^A-Z]+", String.Empty)]] != null)
                                                            {
                                                                if (DateTime.TryParseExact(dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns[sKey], @"[^A-Z]+", String.Empty)]].ToString(), "dd.MM.yy HH:mm", null, DateTimeStyles.None, out parsedDate))
                                                                {
                                                                    drSelected[sKey] = parsedDate.ToString("MM/dd/yyyy HH:mm:ss");
                                                                }
                                                                //else if (DateTime.TryParseExact(dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns[sKey], @"[^A-Z]+", String.Empty)]].ToString(), "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out parsedDate))
                                                                //{
                                                                //    drSelected[sKey] = parsedDate;
                                                                //} 
                                                            }
                                                        }
                                                        else
                                                        {
                                                            drSelected[sKey] = dr[diSSColumnsToExportColumns[Regex.Replace(diDetailColumns[sKey], @"[^A-Z]+", String.Empty)]];
                                                            if ((qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"]).ToString() == "Anions")
                                                            {
                                                                string strMiscWithSampleID = Regex.Replace(drSelected[sKey].ToString(), @"[^A-Z]+", String.Empty);
                                                                if (strMiscWithSampleID == "ICV" || strMiscWithSampleID == "CCB" || strMiscWithSampleID == "CCV")
                                                                {
                                                                    if ((qcbatchinfo.dtTemplateInfo.Rows[0]["TemplateName"]).ToString() == "Liquid")
                                                                    {
                                                                        drSelected["VolumeUsed"] = 1;
                                                                    }
                                                                    else
                                                                    {
                                                                        drSelected["VolumeUsed"] = 4;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (drSelected[sKey].ToString().Contains(';'))
                                                                    {
                                                                        //22120498.01; 1ML - 1ML / 1X
                                                                        string[] strSplit = drSelected[sKey].ToString().Split(';');
                                                                        if (strSplit != null && strSplit.Length > 0)
                                                                        {
                                                                            //1ML - 1ML / 1X
                                                                            string strMisc = strSplit[1].ToString().Trim();
                                                                            string[] strVolumeSplit = strMisc.Split('-');
                                                                            if (strVolumeSplit != null && strVolumeSplit.Length > 0)
                                                                            {
                                                                                //1ML                                               
                                                                                string strSampleVolume = Regex.Replace(strVolumeSplit[0], @"[^\d]", "");
                                                                                drSelected["VolumeUsed"] = strSampleVolume;
                                                                            }
                                                                            else
                                                                            {
                                                                                drSelected["VolumeUsed"] = 1;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            drSelected["VolumeUsed"] = 1;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        drSelected["VolumeUsed"] = 1;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                IStransfered = true;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Recalculate(ASPxSpreadsheet spreadsheet)
        {
            try
            {
                IWorkbook workbook = new Workbook();
                SpreadSheetBuilder_TestParameter testParameter = os.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID]=?", qcbatchinfo.OidTestMethod));
                if (testParameter != null)
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    SpreadSheetBuilder_TemplateInfo templateInfo = objectSpace.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID]=?", testParameter.TemplateID));
                    if (templateInfo != null)
                    {
                        workbook.LoadDocument(templateInfo.SpreadSheet, DocumentFormat.OpenXml);
                        int iSheet = 0;
                        foreach (Worksheet worksheet in workbook.Worksheets)
                        {
                            Worksheet sscworksheet = spreadsheet.Document.Worksheets[iSheet];
                            CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                            CellRange HeaderRange = null;
                            CellRange DetailRange = null;
                            if (worksheet.DefinedNames.Contains("HEADERRANGE"))
                            {
                                HeaderRange = worksheet["HEADERRANGE"];
                            }
                            if (worksheet.DefinedNames.Contains("HEADERRANGE"))
                            {
                                DetailRange = worksheet["DETAILRANGE"];
                            }
                            var s = worksheet.GetUsedRange().ExistingCells.ToList<Cell>().Where(x => x.Formula != null && x.Formula != "" && x.DisplayText.Length > 0 && x.DisplayText[0] != '[').ToList<Cell>();
                            foreach (Cell c in s)
                            {
                                if (HeaderRange != null && c.BottomRowIndex > HeaderRange.BottomRowIndex)
                                {
                                    string str = c.GetReferenceA1();
                                    string strRange = Regex.Replace(str, @"[^A-Z]+", "");
                                    string strRangeEnd = Regex.Replace(str, @"[^\d]", "");
                                    string strnext = strRange + (Convert.ToInt16(strRangeEnd) + 1);
                                    strRangeEnd = strRange + (Convert.ToInt16(strRangeEnd) + qcbatchinfo.dtsample.Rows.Count - 1).ToString();
                                    sscworksheet.Range[str].Formula = c.Formula;
                                    sscworksheet.Range[string.Format("{0}:{1}", strnext, strRangeEnd)].CopyFrom(sscworksheet.Range[str]);
                                }
                                else
                                {
                                    sscworksheet.Range[c.GetReferenceA1()].Formula = c.Formula;
                                }
                            }
                            iSheet += 1;
                        }
                    }
                    objectSpace.Dispose();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        #region VOC Import
        private DataTable CreateTempTableVOC_Header()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("All", Type.GetType("System.Boolean"));
                dt.Columns.Add("SampleType", Type.GetType("System.String"));
                dt.Columns.Add("DF", Type.GetType("System.String"));
                dt.Columns.Add("FileName", Type.GetType("System.String"));
                dt.Columns.Add("QcBatchID", Type.GetType("System.String"));
                dt.Columns.Add("RefQcBatchID", Type.GetType("System.String"));
                dt.Columns.Add("Matrix", Type.GetType("System.String"));
                dt.Columns.Add("Test", Type.GetType("System.String"));
                dt.Columns.Add("Method", Type.GetType("System.String"));
                dt.Columns.Add("FilePath", Type.GetType("System.String"));
                dt.Columns.Add("SpikeAmt", Type.GetType("System.String"));
                dt.Columns.Add("PrepAmt", Type.GetType("System.String"));
                dt.Columns.Add("FinalVol", Type.GetType("System.String"));
                dt.Columns.Add("Dilution", Type.GetType("System.String"));
                dt.Columns.Add("InjVol", Type.GetType("System.String"));
                dt.Columns.Add("DeftPrepAmt", Type.GetType("System.String"));
                dt.Columns.Add("DeftFinalVol", Type.GetType("System.String"));
                dt.Columns.Add("DeftInjVol", Type.GetType("System.String"));
                dt.Columns.Add("RestCalcRequired", Type.GetType("System.Boolean"));
                dt.Columns.Add("SurrCalcRequired", Type.GetType("System.Boolean"));
                dt.Columns.Add("GridIndex", Type.GetType("System.String"));
                dt.Columns.Add("ID", Type.GetType("System.String"));
                dt.Columns.Add("AnalystDate", Type.GetType("System.String"));
                dt.Columns.Add("Analyst", Type.GetType("System.String"));
                dt.Columns.Add("PrepInfo", Type.GetType("System.String"));
                dt.Columns.Add("CalibMethod", Type.GetType("System.String"));
                dt.Columns.Add("FilePathF", Type.GetType("System.String"));
                dt.Columns.Add("QCType", Type.GetType("System.String"));
                dt.Columns.Add("SampleWt", Type.GetType("System.String"));
                dt.Columns.Add("InitialVol", Type.GetType("System.String"));
                dt.Columns.Add("PrepAmtUnit", Type.GetType("System.String"));
                dt.Columns.Add("FinalVolUnit", Type.GetType("System.String"));
                dt.Columns.Add("InjVolUnit", Type.GetType("System.String"));
                dt.Columns.Add("ProcessedDate", Type.GetType("System.String"));
                dt.Columns.Add("ProcessedBy", Type.GetType("System.String"));
                dt.Columns.Add("AnalystDate1", Type.GetType("System.String"));
                dt.Columns.Add("Analyst1", Type.GetType("System.String"));
                dt.Columns.Add("Misc", Type.GetType("System.String"));
                return dt;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        private DataTable CreateTempTableVOC_Detail()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("SampleType");
                dt.Columns.Add("Parameter");
                dt.Columns.Add("Result");
                dt.Columns.Add("QCBatchID");
                dt.Columns.Add("Surrogate");
                dt.Columns.Add("DF");
                dt.Columns.Add("RT");
                dt.Columns.Add("Qlon");
                dt.Columns.Add("Response");
                dt.Columns.Add("Units");
                dt.Columns.Add("AnalystDate");
                dt.Columns.Add("Analyst");
                dt.Columns.Add("PrepAmt");
                dt.Columns.Add("DeftPrepAmt");
                dt.Columns.Add("Dilution");
                dt.Columns.Add("ISTD");
                dt.Columns.Add("Misc");
                dt.Columns["ISTD"].DefaultValue = false;
                return dt;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        private void FindFileName(SDMSDCImport import)
        {
            try
            {
                bool f1 = false;
                bool f1RSample = false;
                bool f2 = false;
                bool anadate = false;
                bool f31 = false;
                bool f32 = false;
                bool f33 = false;
                bool f34 = false;
                bool analby = false;
                string stras = "";
                bool bCalInfo = false;
                string sid = "";
                string DF = "";
                string strCalInfo = "";
                string analystby = "";
                string analdate = "";
                string CalibMethod = "";
                string FilepathF = "";
                MemoryStream memory = new MemoryStream(import.Data);
                StreamReader sr = new StreamReader(memory);
                while (sr.Peek() >= 0)
                {
                    string[] kk = Strings.Split(sr.ReadLine(), " ");
                    f31 = false;
                    f32 = false;
                    f33 = false;
                    f34 = false;
                    analby = false;
                    stras = "";
                    foreach (string k in kk)
                    {
                        string[] a1 = Strings.Split(Strings.Trim(k), " ");
                        foreach (string a in a1)
                        {
                            if (f1 == true)
                            {
                                if (a == "Inst" | a == "Misc")
                                {
                                    sid = Strings.Trim(Strings.Replace(Strings.Trim(sid), ":", " "));
                                    f1 = false;
                                    if (a == "Inst")
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    sid = sid + a;
                                }
                            }
                            if (a == "Sample")
                            {
                                if (f1RSample == false)
                                {
                                    f1 = true;
                                }
                                f1RSample = true;
                            }

                            if (analby == true)
                            {
                                analystby = analystby + " " + Strings.Trim(Strings.Replace(Strings.Trim(a), ":", ""));
                            }
                            if (a == "Operator:" | a == "Operator")
                            {
                                analby = true;
                                analystby = "";
                            }

                            if (anadate == true)
                            {
                                if (a == "Operator:" | a == "Operator")
                                {
                                    analdate = Strings.Trim(Strings.Replace(Strings.Trim(analdate), "On    :", " "));
                                    anadate = false;
                                }
                                else
                                    analdate = analdate + " " + a;
                            }
                            if (a == "Acq")
                            {
                                anadate = true;
                                analdate = "";
                            }
                            if (f32 == true)
                            {
                                FilepathF = FilepathF + " " + a;
                                FilepathF = Strings.Trim(Strings.Replace(Strings.Trim(FilepathF), "Path :", " "));
                            }

                            if (a == "Data")
                            {
                                FilepathF = "";
                                f32 = true;
                            }

                            if (f33 == true)
                            {
                                if (Strings.Trim(stras) == "#1 :")
                                {
                                    FilepathF = "";
                                    stras = "";
                                    f34 = true;
                                }
                                else if (Strings.Trim(stras) == "#2 :")
                                {
                                    FilepathF = "";
                                    stras = "";
                                    f34 = true;
                                }
                                if (f34 == true)
                                {
                                    FilepathF = FilepathF + " " + a;
                                    FilepathF = Strings.Trim(Strings.Replace(Strings.Trim(FilepathF), "#1 :", " "));
                                    FilepathF = Strings.Trim(Strings.Replace(Strings.Trim(FilepathF), "#2 :", " "));
                                }

                                stras = stras + " " + a;
                            }

                            if (a == "Signal")
                            {
                                f33 = true;
                            }

                            if (f31 == true)
                            {
                                CalibMethod = CalibMethod + " " + a;
                                CalibMethod = Strings.Trim(Strings.Replace(Strings.Trim(CalibMethod), "Method :", " "));
                            }
                            if (a == "Method")
                            {
                                CalibMethod = "";
                                CalibMethod = a;
                                f31 = true;
                            }

                            if (bCalInfo == true)
                            {
                                if (a == "Multiplr:" | a == "ALS")
                                {
                                    strCalInfo = Strings.Trim(Strings.Replace(Strings.Trim(strCalInfo), ":", " "));
                                    bCalInfo = false;
                                    break;
                                }
                                else
                                {
                                    strCalInfo = strCalInfo + a;
                                }
                            }
                            if (a == "Misc")
                            {
                                bCalInfo = true;
                            }
                        }
                    }
                }
                DataRow drNewRow = dtInstrumentVOC.NewRow();
                string[] s = Strings.Split(sid, ";");
                drNewRow["SampleType"] = s[0].ToString();
                drNewRow["AnalystDate"] = analdate;
                drNewRow["Analyst"] = analystby;
                drNewRow["PrepInfo"] = strCalInfo;
                drNewRow["CalibMethod"] = CalibMethod;
                drNewRow["FilePathF"] = FilepathF;
                string[] s1 = Strings.Split(strCalInfo, ";");
                string[] s2 = Strings.Split(s1[0], "/");
                string[] s3 = Strings.Split(s2[0], "-");
                string tPrepAmt = Strings.Replace(s3[0], "g", "");
                drNewRow["PrepAmt"] = Strings.Replace(tPrepAmt, "ml", "");
                drNewRow["PrepAmt"] = Strings.Replace(drNewRow["PrepAmt"].ToString(), "Tube", "");
                drNewRow["PrepAmt"] = Strings.Replace(drNewRow["PrepAmt"].ToString(), "tube", "");
                drNewRow["FinalVol"] = Strings.Replace(s3[1], "ml", "");
                drNewRow["Dilution"] = Strings.Replace(s2[1], "X", "");
                if (s2.Length == 3)
                {
                    drNewRow["InjVol"] = Strings.Replace(s2[2], "ul", "");
                }
                else
                {
                    drNewRow["InjVol"] = "";
                }
                drNewRow["DF"] = Calculations_VOC("DF", drNewRow, 0);
                drNewRow["Misc"] = s1[0].ToString();
                dtInstrumentVOC.Rows.Add(drNewRow);
                sr.Close();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private String Calculations_VOC(string Calculation, DataRow drRow, Decimal Reading)
        {
            try
            {
                string strOutput = "";
                if (Calculation == "DF")
                {
                    if (drRow != null && drRow["PrepAmt"] != null && drRow["PrepAmt"].ToString().Length > 0 && drRow["FinalVol"] != null && drRow["FinalVol"].ToString().Length > 0 && drRow["Dilution"] != null && drRow["Dilution"].ToString().Length > 0 && drRow["InjVol"] != null && drRow["InjVol"].ToString().Length > 0)
                    {
                        int V1 = int.Parse(drRow["PrepAmt"].ToString());
                        int V2 = int.Parse(drRow["FinalVol"].ToString());
                        int D = int.Parse(drRow["Dilution"].ToString());
                        int V3 = int.Parse(drRow["InjVol"].ToString());
                        strOutput = ((5 / V1) * (V2 / 5) * D * (5 / V3)).ToString();
                    }
                }
                else if (Calculation == "Result")
                {
                    if (drRow != null && drRow["PrepAmt"] != null && drRow["PrepAmt"].ToString().Length > 0 && drRow["DF"] != null && drRow["DF"].ToString().Length > 0)
                    {
                        int V1 = int.Parse(drRow["PrepAmt"].ToString());
                        int DF = int.Parse(drRow["DF"].ToString());
                        strOutput = ((Reading / V1) * DF).ToString();
                    }

                }
                return strOutput;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        private DataTable FillDataFromVOC(SDMSDCImport import)

        {
            try
            {
                DataTable dtSample = qcbatchinfo.dtsample;
                DataTable dtParamMatch = GetParameterMatch();
                dtInstrumentVOC = CreateTempTableVOC_Header();
                FindFileName(import);
                DataTable dt = CreateTempTableVOC_Detail();
                DataRow dr;
                bool bolInternalStandards = false;
                bool bolSMCompounds = false;
                bool bolTargetCompounds = false;
                string ng = "";
                string p = "";
                string SampleID = "";
                string strDF = "";
                string strRT = "";
                string strQlon = "";
                string strResponse = "";
                if (dtInstrumentVOC != null && dtInstrumentVOC.Rows.Count > 0)
                {
                    SampleID = dtInstrumentVOC.Rows[0]["SampleType"].ToString();
                    MemoryStream ms = new MemoryStream(import.Data);
                    StreamReader reader = new StreamReader(ms);
                    while (reader.Peek() >= 0)
                    {
                        string[] kk = Strings.Split(reader.ReadLine(), "");
                        foreach (string k in kk)
                        {
                            // Internal Standard Logic Starts Here.
                            if (bolInternalStandards == true)
                            {
                                if (k == "   ---------------------------------------------------------------------------")
                                {
                                    break;
                                }
                                else
                                {
                                    String[] s1 = k.Split(' ');
                                    int i = 0;
                                    p = "";
                                    ng = "";
                                    string S2Iteration;
                                    bool bol1 = false;
                                    bool bol2 = false;
                                    bool bol3 = false;
                                    foreach (string s2 in s1)
                                    {
                                        if (s2 != "")
                                        {
                                            if (i >= 1)
                                            {
                                                if (Information.IsNumeric(s2) == true || s2 == "ng" || s2 == "nL" || s2 == "ug/L" || s2 == "N.D." || s2 == "ug" || s2 == "ppm")
                                                {
                                                    if (s2 == "ng" || s2 == "nL" || s2 == "ug/L" || s2 == "N.D." || s2 == "ug" || s2 == "ppm")
                                                    {
                                                        dr = dt.NewRow();
                                                        dr["SampleType"] = SampleID;
                                                        if (s2 == "N.D.")
                                                        {
                                                            ng = "0";
                                                        }
                                                        dr["PrepAmt"] = dtInstrumentVOC.Rows[0]["PrepAmt"];
                                                        dr["DeftPrepAmt"] = dtInstrumentVOC.Rows[0]["DeftPrepAmt"];
                                                        dr["Analyst"] = dtInstrumentVOC.Rows[0]["Analyst"];
                                                        dr["AnalystDate"] = dtInstrumentVOC.Rows[0]["AnalystDate"];
                                                        String strParameterName = "";
                                                        p = Strings.Replace(Strings.Trim(p), "**IS**", "").Trim().ToString();
                                                        string[] f = p.Trim().Split(')');
                                                        String param = f.GetValue(0).ToString();
                                                        if (Information.IsNumeric(param) == false)
                                                        {
                                                            strParameterName = p.Trim();
                                                        }
                                                        else
                                                        {
                                                            int intParam = param.Length + 2;
                                                            int intP = p.Length;
                                                            strParameterName = Microsoft.VisualBasic.Strings.Right(p, intP - intParam).Trim();
                                                        }
                                                        if (strParameterName != null)
                                                        {
                                                            DataRow[] drrFilter = null;
                                                            drrFilter = dtParamMatch.Select("MParameter= '" + strParameterName + "'");
                                                            if (drrFilter != null && drrFilter.Length > 0)
                                                            {
                                                                foreach (DataRow drParam in drrFilter)
                                                                {
                                                                    DataRow[] drr = null;
                                                                    drr = dtSample.Select("PARAMETER= '" + drParam["Parameter"] + "'");
                                                                    if (drr != null && drr.Length > 0)
                                                                    {
                                                                        strParameterName = drParam["Parameter"].ToString();
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        dr["Parameter"] = strParameterName;
                                                        try
                                                        {
                                                            ng = Calculations_VOC("Result", dtInstrumentVOC.Rows[0], Convert.ToDecimal(ng));
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                        }
                                                        dr["Result"] = ng;
                                                        dr["DF"] = dtInstrumentVOC.Rows[0]["DF"];
                                                        dr["Dilution"] = dtInstrumentVOC.Rows[0]["Dilution"];
                                                        dr["RT"] = strRT;
                                                        dr["Qlon"] = strQlon;
                                                        dr["Response"] = strResponse;
                                                        dr["Units"] = s2;
                                                        dt.Rows.Add(dr);
                                                        strRT = "";
                                                        strQlon = "";
                                                        strResponse = "";
                                                        p = "";
                                                        ng = "";
                                                        break;
                                                    }
                                                    //Logic for RT Qlon Response
                                                    else if (Information.IsNumeric(s2) == true && i > 1)
                                                    {
                                                        if (bol1 == false && bol2 == false && bol3 == false)
                                                        {
                                                            bol1 = true;
                                                        }
                                                        if (bol1 == true && strRT == "")
                                                        {
                                                            strRT = Strings.Trim(s2);
                                                            bol1 = false;
                                                            bol2 = true;
                                                            bol3 = false;
                                                        }
                                                        else if (bol2 == true && strQlon == "")
                                                        {
                                                            strQlon = Strings.Trim(s2);
                                                            bol1 = false;
                                                            bol2 = false;
                                                            bol3 = true;
                                                        }
                                                        else if (bol3 == true && strResponse == "")
                                                        {
                                                            strResponse = Strings.Trim(s2);
                                                            bol1 = false;
                                                            bol2 = false;
                                                            bol3 = false;
                                                        }
                                                    }
                                                    //
                                                }
                                                else
                                                {
                                                    string strs2 = Strings.Right(s2, 1);
                                                    if (strs2 == "m" || strs2 == "f")
                                                    {
                                                        string str1s2 = Strings.Left(s2, s2.Length - 1);
                                                        if (Information.IsNumeric(str1s2) == true)
                                                        {
                                                            S2Iteration = str1s2;
                                                        }
                                                        else
                                                        {
                                                            if (s2 != "N.D." && s2 != "<MDL")
                                                            {
                                                                p = p + " " + s2;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (s2 != "N.D." && s2 != "<MDL")
                                                        {
                                                            p = p + " " + s2;
                                                        }
                                                    }
                                                }
                                            }
                                            S2Iteration = s2;
                                            ng = S2Iteration;
                                            i = i + 1;
                                        }
                                    }
                                    S2Iteration = "";
                                }
                            }
                            // Internal Standard Logic Ends Here.

                            // Target Compounds Logic Starts Here.
                            if (bolTargetCompounds == true)
                            {
                                if (k == "   ---------------------------------------------------------------------------")
                                {
                                    break;
                                }
                                else
                                {
                                    String[] s1 = k.Split(' ');
                                    int i = 0;
                                    p = "";
                                    ng = "";
                                    string S2Iteration;
                                    bool bol1 = false;
                                    bool bol2 = false;
                                    bool bol3 = false;
                                    foreach (string s2 in s1)
                                    {
                                        if (s2 != "")
                                        {
                                            if (i >= 1)
                                            {
                                                if (Information.IsNumeric(s2) == true || s2 == "ng" || s2 == "nL" || s2 == "ug/L" || s2 == "N.D." || s2 == "ug" || s2 == "ppm")
                                                {
                                                    if (s2 == "ng" || s2 == "nL" || s2 == "ug/L" || s2 == "N.D." || s2 == "ug" || s2 == "ppm")
                                                    {
                                                        dr = dt.NewRow();
                                                        dr["SampleType"] = SampleID;
                                                        if (s2 == "N.D.")
                                                        {
                                                            ng = "0";
                                                        }
                                                        dr["PrepAmt"] = dtInstrumentVOC.Rows[0]["PrepAmt"];
                                                        dr["DeftPrepAmt"] = dtInstrumentVOC.Rows[0]["DeftPrepAmt"];
                                                        dr["Analyst"] = dtInstrumentVOC.Rows[0]["Analyst"];
                                                        dr["AnalystDate"] = dtInstrumentVOC.Rows[0]["AnalystDate"];
                                                        String strParameterName = "";
                                                        string[] f = p.Trim().Split(')');
                                                        String param = f.GetValue(0).ToString();
                                                        if (Information.IsNumeric(param) == false)
                                                        {
                                                            strParameterName = p.Trim();
                                                        }
                                                        else
                                                        {
                                                            int intParam = param.Length + 2;
                                                            int intP = p.Length;
                                                            strParameterName = Microsoft.VisualBasic.Strings.Right(p, intP - intParam).Trim();
                                                        }
                                                        if (strParameterName != null)
                                                        {
                                                            DataRow[] drrFilter = null;
                                                            drrFilter = dtParamMatch.Select("MParameter= '" + strParameterName + "'");
                                                            if (drrFilter != null && drrFilter.Length > 0)
                                                            {
                                                                foreach (DataRow drParam in drrFilter)
                                                                {
                                                                    DataRow[] drr = null;
                                                                    drr = dtSample.Select("PARAMETER= '" + drParam["Parameter"] + "'");
                                                                    if (drr != null && drr.Length > 0)
                                                                    {
                                                                        strParameterName = drParam["Parameter"].ToString();
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        dr["Parameter"] = strParameterName;
                                                        try
                                                        {
                                                            ng = Calculations_VOC("Result", dtInstrumentVOC.Rows[0], Convert.ToDecimal(ng));
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                        }
                                                        dr["Result"] = ng;
                                                        dr["DF"] = dtInstrumentVOC.Rows[0]["DF"];
                                                        dr["Dilution"] = dtInstrumentVOC.Rows[0]["Dilution"];
                                                        dr["RT"] = strRT;
                                                        dr["Qlon"] = strQlon;
                                                        dr["Response"] = strResponse;
                                                        dr["Units"] = s2;
                                                        dr["Misc"] = dtInstrumentVOC.Rows[0]["Misc"];
                                                        dt.Rows.Add(dr);
                                                        strRT = "";
                                                        strQlon = "";
                                                        strResponse = "";
                                                        p = "";
                                                        ng = "";
                                                        break;
                                                    }
                                                    //Logic for RT Qlon Response
                                                    else if (Information.IsNumeric(s2) == true && i > 1)
                                                    {
                                                        if (bol1 == false && bol2 == false && bol3 == false)
                                                        {
                                                            bol1 = true;
                                                        }
                                                        if (bol1 == true && strRT == "")
                                                        {
                                                            strRT = Strings.Trim(s2);
                                                            bol1 = false;
                                                            bol2 = true;
                                                            bol3 = false;
                                                        }
                                                        else if (bol2 == true && strQlon == "")
                                                        {
                                                            strQlon = Strings.Trim(s2);
                                                            bol1 = false;
                                                            bol2 = false;
                                                            bol3 = true;
                                                        }
                                                        else if (bol3 == true && strResponse == "")
                                                        {
                                                            strResponse = Strings.Trim(s2);
                                                            bol1 = false;
                                                            bol2 = false;
                                                            bol3 = false;
                                                        }
                                                    }
                                                    //
                                                }
                                                else
                                                {
                                                    string strs2 = Strings.Right(s2, 1);
                                                    if (strs2 == "m" || strs2 == "f")
                                                    {
                                                        string str1s2 = Strings.Left(s2, s2.Length - 1);
                                                        if (Information.IsNumeric(str1s2) == true)
                                                        {
                                                            S2Iteration = str1s2;
                                                        }
                                                        else
                                                        {
                                                            if (s2 != "N.D." && s2 != "<MDL")
                                                            {
                                                                p = p + " " + s2;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (s2 != "N.D." && s2 != "<MDL")
                                                        {
                                                            p = p + " " + s2;
                                                        }
                                                    }
                                                }
                                            }
                                            S2Iteration = s2;
                                            ng = S2Iteration;
                                            i = i + 1;
                                        } // if (s2 != null)
                                    }
                                    S2Iteration = "";
                                }
                            }
                            // Target Compounds Logic Ends Here.

                            // System Monitoring Compounds Starts Here.
                            if (bolSMCompounds == true)
                            {
                                if (k == "   Target Compounds                                                  Qvalue" || k == "   Target Compounds                                                   Qvalue" || k == "   Target Compounds                                                    ")
                                {
                                    bolSMCompounds = false;
                                }
                                else
                                {
                                    bool bolb = false;
                                    string[] b1 = Strings.Split(Strings.Trim(k), " ");
                                    //string b;
                                    foreach (string b in b1)
                                    {
                                        if (b == "Target")
                                        {
                                            bolSMCompounds = false;
                                            bolb = true;
                                            break;
                                        }
                                    }
                                    if (bolb == false)
                                    {
                                        //string[] s1 = Strings.Split(k, " ");
                                        string[] s1 = Regex.Split((k.Trim()), @"\s+");
                                        int i = 0;
                                        string S2Iteration;
                                        bool bol1 = false;
                                        bool bol2 = false;
                                        bool bol3 = false;
                                        foreach (string s2 in s1)
                                        {
                                            if (s2 != null && s2 != "")
                                            {
                                                if (i >= 1)
                                                {
                                                    if (Information.IsNumeric(s2) == true || s2 == "ng" || s2 == "nL" || s2 == "ppm")
                                                    {
                                                        if (s2 == "ng" || s2 == "nL" || s2 == "ppm")
                                                        {
                                                            if (p.Length > 0)
                                                            {
                                                                dr = dt.NewRow();
                                                                dr["SampleType"] = SampleID;
                                                                dr["PrepAmt"] = dtInstrumentVOC.Rows[0]["PrepAmt"];
                                                                dr["DeftPrepAmt"] = dtInstrumentVOC.Rows[0]["DeftPrepAmt"];
                                                                dr["Analyst"] = dtInstrumentVOC.Rows[0]["Analyst"];
                                                                dr["AnalystDate"] = dtInstrumentVOC.Rows[0]["AnalystDate"];

                                                                string strParameterName;
                                                                string[] f = Strings.Split(p.Trim(), ")");
                                                                string Param = f.GetValue(0).ToString();
                                                                if (Information.IsNumeric(Param) == false)
                                                                {
                                                                    strParameterName = p.Trim();
                                                                }
                                                                else
                                                                {
                                                                    int iParam = Param.Length + 2;
                                                                    int ipa = p.Length;
                                                                    strParameterName = Strings.Trim(Strings.Right(p, ipa - iParam));
                                                                }
                                                                if (strParameterName != null)
                                                                {
                                                                    DataRow[] drrFilter = null;
                                                                    drrFilter = dtParamMatch.Select("MParameter= '" + strParameterName + "'");
                                                                    if (drrFilter != null && drrFilter.Length > 0)
                                                                    {
                                                                        foreach (DataRow drParam in drrFilter)
                                                                        {
                                                                            DataRow[] drr = null;
                                                                            drr = dtSample.Select("PARAMETER= '" + drParam["Parameter"] + "'");
                                                                            if (drr != null && drr.Length > 0)
                                                                            {
                                                                                strParameterName = drParam["Parameter"].ToString();
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                dr["Parameter"] = strParameterName;
                                                                try
                                                                {
                                                                    //strDF = Calculations_VOC("DF", dtInstrumentVOC.Rows[0]);
                                                                    ng = Calculations_VOC("Result", dtInstrumentVOC.Rows[0], Convert.ToDecimal(ng));
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                }
                                                                dr["Result"] = ng;
                                                                dr["DF"] = dtInstrumentVOC.Rows[0]["DF"];
                                                                dr["Dilution"] = dtInstrumentVOC.Rows[0]["Dilution"];
                                                                dr["RT"] = strRT;
                                                                dr["Qlon"] = strQlon;
                                                                dr["Response"] = strResponse;
                                                                dr["Units"] = s2;
                                                                dr["Misc"] = dtInstrumentVOC.Rows[0]["Misc"];
                                                                dt.Rows.Add(dr);
                                                                strRT = "";
                                                                strQlon = "";
                                                                strResponse = "";
                                                                p = "";
                                                                ng = "";
                                                                break;
                                                            }
                                                        }
                                                        //Logic for RT Qlon Response
                                                        else if (Information.IsNumeric(s2) == true && i > 1)
                                                        {
                                                            if (bol1 == false && bol2 == false && bol3 == false)
                                                            {
                                                                bol1 = true;
                                                            }
                                                            if (bol1 == true && strRT == "")
                                                            {
                                                                strRT = Strings.Trim(s2);
                                                                bol1 = false;
                                                                bol2 = true;
                                                                bol3 = false;
                                                            }
                                                            else if (bol2 == true && strQlon == "")
                                                            {
                                                                strQlon = Strings.Trim(s2);
                                                                bol1 = false;
                                                                bol2 = false;
                                                                bol3 = true;
                                                            }
                                                            else if (bol3 == true && strResponse == "")
                                                            {
                                                                strResponse = Strings.Trim(s2);
                                                                bol1 = false;
                                                                bol2 = false;
                                                                bol3 = false;
                                                            }
                                                        }
                                                        //
                                                    }
                                                    else
                                                    {
                                                        string strs2 = Strings.Right(s2, 1);
                                                        if (strs2 == "m" || strs2 == "f")
                                                        {
                                                            string str1s2 = Strings.Left(s2, s2.Length - 1);
                                                            if (Regex.IsMatch(str1s2, @"^\d+$"))
                                                            {
                                                                S2Iteration = str1s2;
                                                            }
                                                            else
                                                            {
                                                                p = p + " " + s2;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            p = p + " " + s2;
                                                        }
                                                    }
                                                }
                                                S2Iteration = s2;
                                                ng = S2Iteration;
                                                i = i + 1;
                                            }
                                        }
                                        S2Iteration = "";
                                    }
                                }
                            }
                            // System Monitoring Compounds Ends Here.

                            if (k == "   System Monitoring Compounds                                        " || k.Contains("System Monitoring Compounds") == true)
                            {
                                bolSMCompounds = true;
                                bolInternalStandards = true;
                            }
                            if (k == "   Internal Standards")
                            {
                                bolInternalStandards = true;
                            }
                            if (k == "   Target Compounds                                                  Qvalue" || k == "   Target Compounds                                                   Qvalue" || k == "   Target Compounds                                                    ")
                            {
                                bolTargetCompounds = true;
                            }
                            else
                            {
                                bool bolb = false;
                                string[] b1 = Strings.Split(k.Trim(), " ");
                                //string b;
                                foreach (string b in b1)
                                {
                                    if (b == "Target")
                                    {
                                        bolTargetCompounds = true;
                                        bolb = true;
                                        break;
                                    }
                                }
                                if (bolb == false)
                                {
                                    bool anadate = false;
                                    bool fana = false;
                                    string[] a1 = Strings.Split(k.Trim(), " ");
                                    string IterationA;
                                    foreach (string a in a1)
                                    {
                                        if (fana == true && a.Trim() != "" && a.Trim() != ":" && a.Trim().Length > 1)
                                        {
                                            if (a.Trim().ToUpper().Contains(@"ABLABS\"))
                                            {
                                                IterationA = a.Replace(@"ABLABS\", "");
                                            }
                                            dtInstrumentVOC.Rows[0]["Analyst"] = a.Trim();
                                            fana = fana;
                                        }

                                        if (anadate == true)
                                        {
                                            if (a == "Operator:" || a == "Operator")
                                            {
                                                dtInstrumentVOC.Rows[0]["AnalystDate"] = Strings.Trim(Strings.Replace(Strings.Trim(dtInstrumentVOC.Rows[0]["AnalystDate"].ToString()), "on", " "));
                                                anadate = false;
                                                fana = true;
                                            }
                                        }
                                        if (a == "Acq")
                                        {
                                            anadate = true;
                                        }
                                    }
                                    IterationA = "";
                                }
                            }
                        }

                    }
                    reader.Close();
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
        #endregion

        #region TPH Import
        private DataTable CreateTempTableTPH()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("SampleID", Type.GetType("System.String"));
                dt.Columns.Add("FileName", Type.GetType("System.String"));
                dt.Columns.Add("MethodName", Type.GetType("System.String"));
                dt.Columns.Add("Misc", Type.GetType("System.String"));
                dt.Columns.Add("PrepAmt", Type.GetType("System.String"));
                dt.Columns.Add("FinalVol", Type.GetType("System.String"));
                dt.Columns.Add("Dilution", Type.GetType("System.String"));
                dt.Columns.Add("InjVol", Type.GetType("System.String"));
                dt.Columns.Add("DF", Type.GetType("System.String"));
                dt.Columns.Add("Surrogate", Type.GetType("System.Boolean"));
                dt.Columns.Add("Parameter", Type.GetType("System.String"));
                dt.Columns.Add("Result", Type.GetType("System.String"));
                dt.Columns.Add("AnalyzedDateTime", Type.GetType("System.String"));
                dt.Columns.Add("Analyst", Type.GetType("System.String"));
                dt.Columns["Surrogate"].DefaultValue = false;
                return dt;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        private void FindFileNameTPH(DataRow drRow)
        {
            try
            {
                //string strReturn = "";
                string strFileName = drRow["FileName"].ToString();
                string[] SSID = Strings.Split(Strings.Trim(strFileName), "\\");
                if (SSID.Length > 0)
                {
                    string[] LSSID = Strings.Split(SSID[SSID.Length - 1], ".");
                    if (Strings.UCase(Strings.Right(LSSID[0], 1)) == "A" ||
                       Strings.UCase(Strings.Right(LSSID[0], 1)) == "B" ||
                       Strings.UCase(Strings.Right(LSSID[0], 1)) == "C" ||
                       Strings.UCase(Strings.Right(LSSID[0], 1)) == "F" ||
                       Strings.UCase(Strings.Right(LSSID[0], 1)) == "E")
                    {
                        LSSID[0] = LSSID[0].Replace("a", "");
                        LSSID[0] = LSSID[0].Replace("A", "");
                        LSSID[0] = LSSID[0].Replace("b", "");
                        LSSID[0] = LSSID[0].Replace("B", "");
                        LSSID[0] = LSSID[0].Replace("c", "");
                        LSSID[0] = LSSID[0].Replace("C", "");
                        LSSID[0] = LSSID[0].Replace("f", "");
                        LSSID[0] = LSSID[0].Replace("F", "");
                        LSSID[0] = LSSID[0].Replace("e", "");
                        LSSID[0] = LSSID[0].Replace("E", "");
                        string strNumber = "";
                        strNumber = Strings.Right(LSSID[0], 2);
                        LSSID[0] = LSSID[0] + "." + strNumber;
                        //strReturn = LSSID[0];

                    }
                }
                //return strReturn;
                string[] s1 = Strings.Split(drRow["SampleID"].ToString(), ";");
                string[] s2 = null;
                string[] s3 = null;
                if (s1 != null && s1.Length > 0 && s1[0] != "" && s1[0].Contains("/"))
                {
                    s2 = Strings.Split(s1[0], "/");
                    drRow["SampleID"] = Strings.Trim(Strings.Replace(s1[1], ".", "-"));
                    drRow["SampleID"] = Strings.Replace(drRow["SampleID"].ToString(), "/", "");
                    drRow["Misc"] = Strings.Trim(s1[0]);
                }
                else
                {
                    drRow["SampleID"] = Strings.Trim(s1[0]);
                }
                if (s2 != null && s2.Length > 0 && s2[0] != "" && s2[0].Contains("-"))
                {
                    s3 = Strings.Split(s2[0], "-");
                    drRow["PrepAmt"] = Strings.Replace(s3[0], "g", "");
                    drRow["PrepAmt"] = Strings.Replace(drRow["PrepAmt"].ToString(), "ml", "");
                }
                if (s3 != null && s3.Length > 1)
                {
                    drRow["FinalVol"] = Strings.Replace(s3[1], "ml", "");
                }
                if (s2 != null && s2.Length > 1)
                {
                    drRow["Dilution"] = Strings.Replace(s2[1], "x", "");
                }
                if (s2 != null && s2.Length > 2)
                {
                    drRow["InjVol"] = Strings.Replace(s2[2], "ml", "");
                    drRow["InjVol"] = Strings.Replace(drRow["InjVol"].ToString(), "ul", "");
                }
                drRow["DF"] = Calculations_VOC("DF", drRow, 0);
            }
            catch (Exception ex)
            {

            }
        }
        private DataTable FillDataFromTPH(SDMSDCImport import)
        {
            try
            {
                DataTable dtSample = qcbatchinfo.dtsample;
                DataTable dtParamMatch = GetParameterMatch();
                DataTable dt = CreateTempTableTPH();
                bool bolDetailRow = false;
                bool bolSurrogate = false;
                Dictionary<string, string> diParamResult = new Dictionary<string, string>();
                int intline = 0;
                MemoryStream ms = new MemoryStream(import.Data);
                StreamReader reader = new StreamReader(ms);
                while (reader.Peek() >= 0)
                {
                    string[] strLine = Strings.Split(reader.ReadLine(), "");
                    foreach (string strSentences in strLine)
                    {
                        string strReplacedWord = Strings.Replace(strSentences, "\t", " ").ToString();
                        // Detail row logic begins here.
                        if (intline > 2)
                        {
                            string strDate = "";
                            string strTime = "";
                            string strSampleID = "";
                            string strFileName = "";
                            string strMethodName = "";
                            string strUserName = "";
                            bool bolDateIdentified = false;
                            bool bolTimeIdentified = false;
                            bool bolDateTimeIdentified = false;
                            bool bol1 = false;
                            bool bol2 = false;
                            bool bol3 = false;
                            bool bolAddRow = false;
                            bool bolSampleID = false;
                            string strParam1 = "";
                            string strParam2 = "";
                            string strParam3 = "";
                            string[] strSentence = Regex.Split((strReplacedWord.Trim()), @"\s+");
                            int intIndex = 0;
                            foreach (string strWord in strSentence)
                            {
                                if (strWord != "" && strWord.Length > 0)
                                {
                                    if (intIndex <= 2)
                                    {
                                        if (strDate == "")
                                        {
                                            strDate = strWord;
                                        }
                                        else
                                        {
                                            strDate = strDate + " " + strWord;
                                            if (strWord == "AM" || strWord == "PM")
                                            {
                                                bolDateTimeIdentified = true;
                                            }
                                        }
                                    }
                                    else if (bolDateTimeIdentified == true)
                                    {
                                        //Parameter Results
                                        if (Microsoft.VisualBasic.Information.IsNumeric(Strings.Trim(strWord)) == true && intIndex > 5)
                                        {
                                            if (bolSurrogate == true)
                                            {
                                                if (bol1 == false && bol2 == false)
                                                {
                                                    bol1 = true;
                                                }
                                                if (bol1 == true && strParam1 == "")
                                                {
                                                    strParam1 = Strings.Trim(strWord);
                                                    diParamResult.Add("1-Chlorooctane", strParam1);
                                                    bol1 = false;
                                                    bol2 = true;
                                                }
                                                else if (bol2 == true && strParam2 == "")
                                                {
                                                    strParam2 = Strings.Trim(strWord);
                                                    diParamResult.Add("1-Chlorooctadecane", strParam2);
                                                    bol1 = false;
                                                    bol2 = false;
                                                    bol3 = false;
                                                }
                                            }
                                            else
                                            {
                                                if (bol1 == false && bol2 == false && bol3 == false)
                                                {
                                                    bol1 = true;
                                                }
                                                if (bol1 == true && strParam1 == "")
                                                {
                                                    strParam1 = Strings.Trim(strWord);
                                                    diParamResult.Add("C-6  to  C-12", strParam1);
                                                    bol1 = false;
                                                    bol2 = true;
                                                    bol3 = false;
                                                }
                                                else if (bol2 == true && strParam2 == "")
                                                {
                                                    strParam2 = Strings.Trim(strWord);
                                                    diParamResult.Add(">C-12  to  C-28", strParam2);
                                                    bol1 = false;
                                                    bol2 = false;
                                                    bol3 = true;
                                                }
                                                else if (bol3 == true && strParam3 == "")
                                                {
                                                    strParam3 = Strings.Trim(strWord);
                                                    diParamResult.Add(">C-28  to  C-35", strParam3);
                                                    bol1 = false;
                                                    bol2 = false;
                                                    bol3 = false;
                                                    bolAddRow = true;
                                                }
                                            }
                                            if (bolAddRow == true && diParamResult.Count > 0)
                                            {

                                                foreach (string key in diParamResult.Keys)
                                                {
                                                    DataRow dr = dt.NewRow();
                                                    dr["SampleID"] = strSampleID;
                                                    dr["FileName"] = strFileName;
                                                    dr["MethodName"] = strMethodName;
                                                    dr["Parameter"] = key;
                                                    dr["Result"] = diParamResult[key].ToString();
                                                    dr["AnalyzedDateTime"] = strDate + " " + strTime;
                                                    dr["Analyst"] = strUserName;
                                                    if (bolSurrogate == true)
                                                    {
                                                        dr["Surrogate"] = true;
                                                    }
                                                    FindFileNameTPH(dr);
                                                    dt.Rows.Add(dr);
                                                }
                                                diParamResult.Clear();
                                                strDate = "";
                                                strTime = "";
                                                strSampleID = "";
                                                strFileName = "";
                                                strMethodName = "";
                                                strUserName = "";
                                                bolDateIdentified = false;
                                                bolTimeIdentified = false;
                                                bolDateTimeIdentified = false;
                                                bol1 = false;
                                                bol2 = false;
                                                bol3 = false;
                                                bolAddRow = false;
                                                bolSampleID = false;
                                                strParam1 = "";
                                                strParam2 = "";
                                                strParam3 = "";
                                            }
                                        }
                                        // FileName & MethodName
                                        else if (strWord.Contains("\\"))
                                        {
                                            bolSampleID = true;
                                            if (strWord.Contains(":") && strFileName == "")
                                            {
                                                strFileName = strWord;
                                            }
                                            else if (strWord.Contains(":") && strMethodName == "")
                                            {
                                                strMethodName = strWord;
                                            }
                                            else if (strFileName.Length > 0 && strMethodName == "")
                                            {
                                                strFileName = strFileName + " " + strWord;
                                            }
                                            else if (strMethodName.Length > 0)
                                            {
                                                strMethodName = strMethodName + " " + strWord;
                                            }
                                        }
                                        //UserName
                                        else if (bolSampleID == true)
                                        {
                                            strUserName = Strings.Trim(strWord);
                                            bolSampleID = false;
                                        }
                                        //SampleID
                                        else
                                        {
                                            strSampleID = strSampleID + " " + strWord;
                                        }
                                    }
                                }
                                intIndex += 1;

                            }
                        }

                        if (strReplacedWord.Contains("Date Time Sample Id File Name Method Name User Name"))
                        {
                            bolDetailRow = true;
                            if (strReplacedWord.Contains("1-Chloro"))
                            {
                                bolSurrogate = true;
                            }
                            else
                            {
                                bolSurrogate = false;
                            }
                        }
                    }
                    intline += 1;
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        private void ResetNavigationCount()
        {
            ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
            ChoiceActionItem dataentryNode = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "DataEntry");
            if (dataentryNode != null)
            {

                ChoiceActionItem child = dataentryNode.Items.FirstOrDefault(i => i.Id == "AnalysisQueue" || i.Id == "AnalysisQueue ");
                if (child != null)
                {
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        int count = 0;
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        IList<SampleParameter> lstTests = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SignOff] = True  And [IsTransferred] = true And ([SubOut] Is Null Or [SubOut] = False) And [UQABID] Is Null And [QCBatchID] Is Null And (([Testparameter.TestMethod.PrepMethods][].Count() > 0 And [SamplePrepBatchID] Is Not Null) Or [Testparameter.TestMethod.PrepMethods][].Count() = 0)"));
                        if (lstTests != null && lstTests.Count > 0)
                        {
                            //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                            count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                        }
                        //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        if (count > 0)
                        {
                            child.Caption = cap[0] + " (" + count + ")";
                        }
                        else
                        {
                            child.Caption = cap[0];
                        }
                    }
                    else
                    {
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = objSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] =True", currentUser.Oid));

                        List<Guid> lstTestMethodOid = new List<Guid>();
                        IList<SampleParameter> lstTests = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SignOff] = True  And [IsTransferred] = true And ([SubOut] Is Null Or [SubOut] = False) And [UQABID] Is Null And [QCBatchID] Is Null And (([Testparameter.TestMethod.PrepMethods][].Count() > 0 And [SamplePrepBatchID] Is Not Null) Or [Testparameter.TestMethod.PrepMethods][].Count() = 0)"));
                        if (lstTests != null && lstTests.Count > 0)
                        {
                            //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                            IList<Guid> lstselTests = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).ToList();
                            if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                            {
                                foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                {
                                    foreach (TestMethod testMethod in departmentChain.TestMethods)
                                    {
                                        if (!lstTestMethodOid.Contains(testMethod.Oid) && lstselTests.Contains(testMethod.Oid))
                                        {
                                            lstTestMethodOid.Add(testMethod.Oid);
                                        }
                                    }
                                }
                            }

                        }

                        if (lstTestMethodOid.Count > 0)
                        {
                            int count = lstTestMethodOid.Count();

                            if (count > 0)
                            {
                                child.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                child.Caption = cap[0];
                            }
                        }
                    }
                }
                ChoiceActionItem childDataPackageQueue = dataentryNode.Items.FirstOrDefault(i => i.Id == "DataPackageQueue");
                if (childDataPackageQueue != null)
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    var count = objectSpace.GetObjectsCount(typeof(SpreadSheetEntry_AnalyticalBatch), CriteriaOperator.Parse("[DPStatus] = 'PendingSubmission'"));
                    var cap = childDataPackageQueue.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    if (count > 0)
                    {
                        childDataPackageQueue.Caption = cap[0] + " (" + count + ")";
                    }
                    else
                    {
                        childDataPackageQueue.Caption = cap[0];
                    }
                }
                ChoiceActionItem childDataPackageReview = dataentryNode.Items.FirstOrDefault(i => i.Id == "DataPackageReview");
                if (childDataPackageReview != null)
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    var count = objectSpace.GetObjectsCount(typeof(SpreadSheetEntry_AnalyticalBatch), CriteriaOperator.Parse("[DPStatus] = 'PendingReview'"));
                    var cap = childDataPackageReview.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    if (count > 0)
                    {
                        childDataPackageReview.Caption = cap[0] + " (" + count + ")";
                    }
                    else
                    {
                        childDataPackageReview.Caption = cap[0];
                    }
                }
            }
            ChoiceActionItem DataReview = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "Data Review");
            ChoiceActionItem level2review = DataReview.Items.FirstOrDefault(i => i.Id == "RawDataLevel2BatchReview ");
            if (level2review != null)
            {
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                var cap = level2review.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                {
                    var count = os.GetObjectsCount(typeof(SpreadSheetEntry_AnalyticalBatch), CriteriaOperator.Parse("[Status] = 2"));
                    cap = level2review.Caption.Split(new string[] { "(" }, StringSplitOptions.None);
                    if (count > 0)
                    {
                        level2review.Caption = cap[0] + "(" + count + ")";
                        //break;
                    }
                    else
                    {
                        level2review.Caption = cap[0];
                        //break;
                    }
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
                    IList<SpreadSheetEntry_AnalyticalBatch> objAB = os.GetObjects<SpreadSheetEntry_AnalyticalBatch>(new InOperator("Test.Oid", lstTestMethodOid));
                    if (objAB.Count > 0)
                    {
                        int count = objAB.Where(i => i.Status == 2).Select(i => i.Oid).Count();
                        if (count > 0)
                        {
                            level2review.Caption = cap[0] + "(" + count + ")";
                            //break;
                        }
                        else
                        {
                            level2review.Caption = cap[0];
                            //break;
                        }
                    }
                    else
                    {
                        level2review.Caption = cap[0];
                    }

                }
            }
        }
        private void InsertUpdatePLMResults()
        {
            DefaultSetting defaultSetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Spreadsheet'"));
            SpreadSheetEntry_AnalyticalBatch objAB = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
            SpreadsheetEntry_PLMResults PLMResult = null;
            IList<SpreadsheetEntry_PLMResults> plm = os.GetObjects<SpreadsheetEntry_PLMResults>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", qcbatchinfo.AnalyticalQCBatchOid));
            //SpreadsheetEntry_PLMResults PLMReslt = plm.Cast<SpreadsheetEntry_PLMResults >().Where(a => a.uqSampleParameterID != null && a.uqSampleParameterID.Oid.ToString() == qcbatchinfo.dtsample.Rows[i]["UQSAMPLEPARAMETERID"].ToString() && a.RunNo == Convert.ToInt32(qcbatchinfo.dtsample.Rows[i]["RunNo"])).FirstOrDefault();
            for (int i = 0; i < qcbatchinfo.dtsample.Rows.Count; i++)
            {
                if (plm != null && plm.Count == qcbatchinfo.dtsample.Rows.Count)
                {
                    PLMResult = plm.Cast<SpreadsheetEntry_PLMResults>().Where(a => a.uqSampleParameterID != null && a.uqSampleParameterID.Oid.ToString() == qcbatchinfo.dtsample.Rows[i]["UQSAMPLEPARAMETERID"].ToString() && a.SampleLayerID == (qcbatchinfo.dtsample.Rows[i]["SampleLayerID"]).ToString()).FirstOrDefault();
                }
                else
                {
                    PLMResult = os.CreateObject<SpreadsheetEntry_PLMResults>();
                }
                if (PLMResult != null)
                {
                    PLMResult.uqAnalyticalBatchID = objAB;
                    string strColumnName = string.Empty;
                    foreach (DataColumn column in qcbatchinfo.dtsample.Columns)
                    {
                        if (column.ColumnName[0] == '%')
                        {
                            strColumnName = column.ColumnName.Replace(@"%", "P_");
                        }
                        else
                        {
                            strColumnName = column.ColumnName;
                        }
                        //var sproperty = PLMReslt.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name.ToUpper() == column.ColumnName && column.ColumnName != "OID").FirstOrDefault();
                        var sproperty = PLMResult.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name.ToUpper() == strColumnName && column.ColumnName != "OID").FirstOrDefault();
                        if (sproperty != null && !qcbatchinfo.dtsample.Rows[i].IsNull(column.ColumnName))
                        {
                            if (sproperty.MappingFieldDBType.ToString() == qcbatchinfo.dtsample.Rows[i][column].GetType().Name.ToString())
                            {
                                if (sproperty.MappingFieldDBType != DBColumnType.Guid)
                                {
                                    sproperty.SetValue(PLMResult, qcbatchinfo.dtsample.Rows[i][column]);
                                }
                                else
                                {
                                    var Objrefrence = os.FindObject(Type.GetType(sproperty.ReferenceType.FullName + "," + sproperty.ReferenceType.AssemblyName), CriteriaOperator.Parse("[Oid]=?", qcbatchinfo.dtsample.Rows[i][column]));
                                    if (Objrefrence != null)
                                    {
                                        sproperty.SetValue(PLMResult, Objrefrence);
                                    }
                                }
                            }
                            else
                            {
                                if (sproperty.MappingFieldDBType == DBColumnType.Guid && sproperty.ReferenceType.TableName == "Employee")
                                {
                                    var Objrefrence = os.FindObject<PermissionPolicyUser>(CriteriaOperator.Parse("[UserName]=?", qcbatchinfo.dtsample.Rows[i][column]));
                                    if (Objrefrence != null)
                                    {
                                        sproperty.SetValue(PLMResult, Objrefrence);
                                    }
                                }
                                else if (sproperty.MappingFieldDBType != DBColumnType.Guid)
                                {
                                    Type type = Type.GetType(sproperty.MemberType.FullName);
                                    if (type != null)
                                    {
                                        sproperty.SetValue(PLMResult, Convert.ChangeType(qcbatchinfo.dtsample.Rows[i][column], type));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            os.CommitChanges();
        }
        private void DeletePLM(SpreadSheetEntry_AnalyticalBatch ABID)
        {
            try
            {
                IList<SpreadsheetEntry_PLMResults> plm = os.GetObjects<SpreadsheetEntry_PLMResults>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", ABID.Oid));
                os.Delete(plm);
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void InsertUpdateMoldResults()
        {
            SpreadSheetEntry_AnalyticalBatch objAB = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
            SpreadsheetEntry_MoldResults MoldResult = null;
            IList<SpreadsheetEntry_MoldResults> plm = os.GetObjects<SpreadsheetEntry_MoldResults>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", qcbatchinfo.AnalyticalQCBatchOid));
            for (int i = 0; i < qcbatchinfo.dtsample.Rows.Count; i++)
            {
                if (plm != null && plm.Count == qcbatchinfo.dtsample.Rows.Count)
                {
                    MoldResult = plm.Cast<SpreadsheetEntry_MoldResults>().Where(a => a.uqSampleParameterID != null && a.uqSampleParameterID.Oid.ToString() == qcbatchinfo.dtsample.Rows[i]["UQSAMPLEPARAMETERID"].ToString() && a.SampleID == (qcbatchinfo.dtsample.Rows[i]["SampleID"]).ToString()).FirstOrDefault();
                }
                else
                {
                    MoldResult = os.CreateObject<SpreadsheetEntry_MoldResults>();
                }
                if (MoldResult != null)
                {
                    MoldResult.uqAnalyticalBatchID = objAB;
                    string strColumnName = string.Empty;
                    foreach (DataColumn column in qcbatchinfo.dtsample.Columns)
                    {
                        if (column.ColumnName[0] == '%')
                        {
                            strColumnName = column.ColumnName.Replace(@"%", "P_");
                        }
                        else
                        {
                            strColumnName = column.ColumnName;
                        }
                        var sproperty = MoldResult.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name.ToUpper() == strColumnName && column.ColumnName != "OID").FirstOrDefault();
                        if (sproperty != null && !qcbatchinfo.dtsample.Rows[i].IsNull(column.ColumnName))
                        {
                            if (sproperty.MappingFieldDBType.ToString() == qcbatchinfo.dtsample.Rows[i][column].GetType().Name.ToString())
                            {
                                if (sproperty.MappingFieldDBType != DBColumnType.Guid)
                                {
                                    sproperty.SetValue(MoldResult, qcbatchinfo.dtsample.Rows[i][column]);
                                }
                                else
                                {
                                    var Objrefrence = os.FindObject(Type.GetType(sproperty.ReferenceType.FullName + "," + sproperty.ReferenceType.AssemblyName), CriteriaOperator.Parse("[Oid]=?", qcbatchinfo.dtsample.Rows[i][column]));
                                    if (Objrefrence != null)
                                    {
                                        sproperty.SetValue(MoldResult, Objrefrence);
                                    }
                                }
                            }
                            else
                            {
                                if (sproperty.MappingFieldDBType == DBColumnType.Guid && sproperty.ReferenceType.TableName == "Employee")
                                {
                                    var Objrefrence = os.FindObject<PermissionPolicyUser>(CriteriaOperator.Parse("[UserName]=?", qcbatchinfo.dtsample.Rows[i][column]));
                                    if (Objrefrence != null)
                                    {
                                        sproperty.SetValue(MoldResult, Objrefrence);
                                    }
                                }
                                else if (sproperty.MappingFieldDBType != DBColumnType.Guid)
                                {
                                    Type type = Type.GetType(sproperty.MemberType.FullName);
                                    if (type != null)
                                    {
                                        sproperty.SetValue(MoldResult, Convert.ChangeType(qcbatchinfo.dtsample.Rows[i][column], type));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            os.CommitChanges();
        }
        private void DeleteMold(SpreadSheetEntry_AnalyticalBatch ABID)
        {
            try
            {
                IList<SpreadsheetEntry_MoldResults> Mold = os.GetObjects<SpreadsheetEntry_MoldResults>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", ABID.Oid));
                foreach (SpreadsheetEntry_MoldResults MoldResult in Mold)
                {
                    IList<SpreadsheetEntry_MoldAirResults> MoldAir = os.GetObjects<SpreadsheetEntry_MoldAirResults>(CriteriaOperator.Parse("[uqMoldResultsID]=?", MoldResult.Oid));
                    if (MoldAir != null)
                    {
                        os.Delete(MoldAir);
                    }
                }
                os.Delete(Mold);
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void GenerateMailMergeMold(ASPxSpreadsheet spreadsheet, bool IStransfered = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(qcbatchinfo.strTestMethodMatrixName) && !string.IsNullOrEmpty(qcbatchinfo.strTestMethodTestName) && !string.IsNullOrEmpty(qcbatchinfo.strTestMethodMethodNumber))
                {
                    SpreadSheetBuilder_TestParameter testParameter = null;
                    TestMethod objtestmethod = os.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [MethodName.MethodNumber] = ?", qcbatchinfo.strTestMethodMatrixName, qcbatchinfo.strTestMethodTestName, qcbatchinfo.strTestMethodMethodNumber));
                    if (objtestmethod != null)
                    {
                        testParameter = os.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID]=?", objtestmethod.Oid));

                    }
                    if (testParameter != null)
                    {
                        SpreadSheetBuilder_TemplateInfo templateInfo = os.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID]=?", testParameter.TemplateID));
                        if (templateInfo != null)
                        {
                            qcbatchinfo.templateid = testParameter.TemplateID;
                            spreadsheet.Document.CreateNewDocument();
                            IWorkbook templateBook = spreadsheet.Document;
                            templateBook.LoadDocument(templateInfo.SpreadSheet.ToArray(), DocumentFormat.OpenXml);
                            if (!IStransfered)
                            {
                                qcbatchinfo.dtsample = objABinfo.dtQCdatatable;
                                foreach (DataColumn column in qcbatchinfo.dtsample.Columns)
                                {
                                    column.ColumnName = column.ColumnName.ToUpper();
                                }
                            }
                            qcbatchinfo.dtCalibration = new DataTable { TableName = "CalibrationTableDataSource" };
                            qcbatchinfo.dtDataParsing = new DataTable();
                            Getdtcalibrationsource(testParameter.TemplateID, qcbatchinfo.OidTestMethod);
                            Getdtparsingsamplefields(testParameter.TemplateID);
                            drImportDatasource = qcbatchinfo.dtselectedsamplefields.Select("ImportDataSource is Not Null and ImportField is not null and len(ImportField) > 0 ");
                            if (drImportDatasource != null && drImportDatasource.Length > 0)
                            {
                                ImportDatasource(testParameter.TemplateID);
                            }
                            DataRow[] drrSampleSingle = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable'");
                            DataRow[] drrCalibration = qcbatchinfo.dtDataParsing.Select("RunType = 'CalibrationTable'");
                            int intCalibrationSheetID = -1;
                            int intDTSheetID = -1;
                            if (drrSampleSingle.Length > 0 && drrCalibration.Length > 0)
                            {
                                if (drrSampleSingle[0]["SheetID"].ToString() != drrCalibration[0]["SheetID"].ToString())
                                {
                                    intCalibrationSheetID = Convert.ToInt16(drrCalibration[0]["SheetID"].ToString()) - 1;
                                }
                            }
                            if (!string.IsNullOrEmpty(templateInfo.DTSheetID.ToString()) && Convert.ToInt16(templateInfo.DTSheetID) > 0)
                            {
                                intDTSheetID = Convert.ToInt16(templateInfo.DTSheetID.ToString()) - 1;
                            }
                            DataSet dsSpreadSheetDataSource = new DataSet("SpreadSheetDataSource");
                            dsSpreadSheetDataSource.Tables.Add(qcbatchinfo.dtsample.Copy());
                            dsSpreadSheetDataSource.Tables.Add(qcbatchinfo.dtCalibration.Copy());
                            templateBook.MailMergeDataSource = dsSpreadSheetDataSource;
                            templateBook.MailMergeDataMember = "RawDataTableDataSource";

                            IWorkbook reportBook = null;
                            foreach (Worksheet templateSheet in templateBook.Worksheets)
                            {
                                string strRange = "";
                                if (templateSheet.CustomCellInplaceEditors.Count > 0)
                                {
                                    strRange = templateSheet.CustomCellInplaceEditors[0].Range.GetReferenceA1();
                                }
                                templateBook.Worksheets.ActiveWorksheet = templateSheet;
                                IWorkbook currentReport = templateBook.GenerateMailMergeDocuments()[0];
                                IList<IWorkbook> resultWorkbooks = templateBook.GenerateMailMergeDocuments();
                                foreach (IWorkbook workbook in resultWorkbooks)
                                {
                                    if (reportBook == null)
                                    {
                                        reportBook = workbook;
                                        int j = 0;
                                        foreach (Worksheet Sheet in reportBook.Worksheets)
                                        {
                                            Sheet.Name = qcbatchinfo.dtsample.Rows[j]["SYSSAMPLECODE"].ToString();
                                            if (strRange != "")
                                            {
                                                Sheet.CustomCellInplaceEditors.Add(Sheet[strRange], CustomCellInplaceEditorType.Custom, "MyGridControl");
                                            }
                                            j++;
                                        }
                                        continue;
                                    }
                                    foreach (Worksheet reportWorksheet in currentReport.Worksheets)
                                    {
                                        if (reportWorksheet.Visible == true)
                                        {
                                            //reportBook.Worksheets.Add().CopyFrom(reportWorksheet);
                                        }
                                    }
                                }
                                var activeSheet = reportBook.Worksheets.ActiveWorksheet;
                                if (activeSheet != null && activeSheet.IsProtected == false)
                                {
                                    activeSheet["$A:$XFD"].Protection.Locked = false;
                                    IEnumerable<Cell> existingCells = activeSheet.GetExistingCells();
                                    reportBook.BeginUpdate();
                                    foreach (Cell cell in existingCells)
                                    {
                                        if (cell.HasFormula)
                                        {
                                            activeSheet[cell.GetReferenceA1()].Protection.Locked = true;
                                        }
                                    }
                                    reportBook.EndUpdate();
                                    activeSheet.Protect("password", WorksheetProtectionPermissions.SelectLockedCells);
                                }
                            }
                            reportBook.Worksheets.ActiveWorksheet = reportBook.Worksheets[0];
                            spreadsheet.Document.LoadDocument(reportBook.SaveDocument(DocumentFormat.OpenXml), DocumentFormat.OpenXml);
                            string strConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            string[] connectionstring = strConnectionString.Split(';');
                            string strLDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                            string strLDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                            string strLDMSQLUserID = connectionstring[3].Split('=').GetValue(1).ToString();
                            string strLDMSQLPassword = connectionstring[4].Split('=').GetValue(1).ToString();
                            BLCommon.SetDBConnection(strLDMSQLServerName, strLDMSQLDatabaseName, strLDMSQLUserID, strLDMSQLPassword);
                            DataTable dtMoldParams = BLCommon.GetQueryData("SDMSMoldParm");
                            if (dtMoldParams != null && dtMoldParams.Rows.Count > 0)
                            {
                                Worksheet wMold = spreadsheet.Document.Worksheets.Add("Mold");
                                wMold.Import(dtMoldParams, false, 0, 0);
                                wMold.Visible = false;
                                for (int i = 0; i <= spreadsheet.Document.Worksheets.Count - 1; i++)
                                {
                                    if (spreadsheet.Document.Worksheets[i].Name != "Mold")
                                    {
                                        spreadsheet.Document.Worksheets[i].DataValidations.Add(spreadsheet.Document.Worksheets[i]["C7:C38"], DataValidationType.List, ValueObject.FromRange(wMold[string.Format("A1:A{0}", dtMoldParams.Rows.Count)]));
                                    }
                                }
                            }
                            spreadsheet.Document.Worksheets.ActiveWorksheet = spreadsheet.Document.Worksheets[0];
                            qcbatchinfo.IsSheetloaded = true;
                            ABGridrefresh();
                            CalibRefresh();
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

        private void BindMoldSampleToReport(ASPxSpreadsheet spreadSheet, Guid ABID)
        {
            try
            {
                mergeSheet(spreadSheet);
                DataRow[] drrSample = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable' AND Write = TRUE AND Continuous= TRUE");
                DataRow[] drrSampleSingle = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable' AND Write = TRUE AND Continuous= FALSE");
                if (drrSample.Length == 0) return;
                DataRow[] drrSelect = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable'");
                string strSampleID = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "SampleID").Select(r => r["Position"].ToString()).SingleOrDefault();
                string strRunNo = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "RunNo").Select(r => r["Position"].ToString()).SingleOrDefault();
                string strParameter = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "Parameter").Select(r => r["Position"].ToString()).SingleOrDefault();
                string strQCType = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "QCType").Select(r => r["Position"].ToString()).SingleOrDefault();
                int intStartRow = int.Parse(Regex.Replace(strSampleID, @"[^\d]", ""));
                strSampleID = Regex.Replace(strSampleID, @"[^A-Z]+", String.Empty);
                strRunNo = !string.IsNullOrWhiteSpace(strRunNo) ? Regex.Replace(strRunNo, @"[^A-Z]+", String.Empty) : String.Empty;
                strParameter = !string.IsNullOrWhiteSpace(strParameter) ? Regex.Replace(strParameter, @"[^A-Z]+", String.Empty) : String.Empty;
                strQCType = !string.IsNullOrWhiteSpace(strQCType) ? Regex.Replace(strQCType, @"[^A-Z]+", String.Empty) : String.Empty;
                string strSampleIDColumnName = diSSColumnsToExportColumns[strSampleID];
                string strRunNoColumnName = !string.IsNullOrWhiteSpace(strRunNo) ? diSSColumnsToExportColumns[strRunNo] : String.Empty;
                string strParameterColumnName = !string.IsNullOrWhiteSpace(strParameter) ? diSSColumnsToExportColumns[strParameter] : string.Empty;
                string strQCTypColumnName = !string.IsNullOrWhiteSpace(strQCType) ? diSSColumnsToExportColumns[strQCType] : string.Empty;
                Worksheet worksheet;
                if (spreadSheet.Document.Worksheets.Contains("Report"))
                {
                    worksheet = spreadSheet.Document.Worksheets["Report"];
                }
                else
                {
                    worksheet = spreadSheet.Document.Worksheets[int.Parse(drrSample[0]["SheetID"].ToString()) - 1];
                }
                CellRange RanData = worksheet.GetDataRange();
                int intlastUsedRow = RanData.BottomRowIndex + 1;
                int intlastUsedColumn = RanData.RightColumnIndex + 1;
                CellRange range = worksheet.Range[string.Format("A{0}:{01}{02}", intStartRow, diIndexToColumn[intlastUsedColumn], intlastUsedRow)];
                bool rangeHasHeaders = false;
                dtDetailData = worksheet.CreateDataTable(range, false);

                DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dtDetailData, rangeHasHeaders);
                exporter.CellValueConversionError += Exporter_CellValueConversionError;
                // Perform the export.
                exporter.Export();

                //Worksheet worksheetHeader = spreadSheet.Document.Worksheets["Report"];
                //CellRange RanDataHeader = worksheet.GetDataRange();

                //CellRange rangeHeader = worksheet.Range[string.Format("A{0}:{01}{02}", 1, diIndexToColumn[intlastUsedColumn], intStartRow - 1)];
                //bool rangeHasHeader = false;
                //DataTable dataTableheader = worksheet.CreateDataTable(rangeHeader, false);

                //DataTableExporter exporter1 = worksheet.CreateDataTableExporter(rangeHeader, dataTableheader, rangeHasHeader);
                //exporter1.CellValueConversionError += Exporter_CellValueConversionError;
                //// Perform the export.
                //exporter1.Export();

                DataRow row;
                dtHeaderData = new DataTable();
                foreach (DataRow dr in qcbatchinfo.dtHeader.Rows)
                {
                    var test = dr["PositionMapping"].ToString();
                    if (!string.IsNullOrEmpty(test))
                    {
                        Cell cell = worksheet.Cells[dr["PositionMapping"].ToString()];
                        if (cell != null && cell.Value != null && cell.Value.IsText == false && double.TryParse(worksheet.Cells[dr["PositionMapping"].ToString()].NumberFormat, out double dob))
                        //if(cell != null && cell.Value != null && cell.Value.IsText == false && !string.IsNullOrEmpty(cell.Value.ToString()))
                        //if (!string.IsNullOrEmpty(worksheet.Cells[dr["PositionMapping"].ToString()].NumberFormat) && double.TryParse(worksheet.Cells[dr["PositionMapping"].ToString()].NumberFormat, out double d))
                        {
                            dtHeaderData.Columns.Add(dr["CaptionMapping"].ToString(), typeof(double));
                        }
                        else
                        {
                            dtHeaderData.Columns.Add(dr["CaptionMapping"].ToString());
                        }
                    }

                }

                row = dtHeaderData.NewRow();
                foreach (DataRow dr in qcbatchinfo.dtHeader.Rows)
                {
                    var test = dr["PositionMapping"].ToString();
                    if (!string.IsNullOrEmpty(test))
                    {
                        Cell cell = worksheet.Cells[dr["PositionMapping"].ToString()];
                        if (cell.Value.TextValue != null)
                        {
                            string column = dr["CaptionMapping"].ToString();
                            row[column] = worksheet.Cells[dr["PositionMapping"].ToString()].Value.ToString();
                        }
                        else if (cell.Value.NumericValue != null && cell.Value.NumericValue > 0)
                        {
                            string column = dr["CaptionMapping"].ToString();
                            row[column] = worksheet.Cells[dr["PositionMapping"].ToString()].Value.ToString();
                        }
                    }
                }
                dtHeaderData.Rows.Add(row);

                foreach (DataRow dr in qcbatchinfo.dtDetail.Rows)
                {

                    int columindex = Convert.ToInt32(dr["Columnindex"].ToString());

                    string columnname = dr["ColumnMapping"].ToString().ToUpper();

                    dtDetailData.Columns[columindex].ColumnName = columnname;
                }


                if (dtHeaderData != null && dtHeaderData.Rows.Count > 0)
                {
                    foreach (DataColumn col in dtHeaderData.Columns)
                    {
                        string newColumnName = col.ColumnName;

                        while (dtDetailData.Columns.Contains(newColumnName))
                        {
                            newColumnName = string.Format("{0}", col.ColumnName);
                        }

                        dtDetailData.Columns.Add(newColumnName, col.DataType);
                    }

                }

                foreach (DataRow drHeader in dtHeaderData.Rows)
                {

                    foreach (DataColumn c in drHeader.Table.Columns)  //loop through the columns. 
                    {

                        foreach (DataRow rows in dtDetailData.Rows)
                        {

                            rows[rows.Table.Columns[c.ToString()].Ordinal] = drHeader[drHeader.Table.Columns[c.ToString()].Ordinal];
                        }

                    }


                }

                dtDetailDataNew = dtDetailData.Clone();
                int countformat = 0;

                foreach (DataRow dr in dtDetailData.Rows)
                {
                    object[] objData = dr.ItemArray;
                    for (int i = 0; i < objData.Length; i++)
                    {
                        DateTime date;
                        string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                         "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                         "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                         "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                         "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};
                        var val = objData[i].ToString();
                        if (countformat == 0 && DateTime.TryParseExact(val, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {

                            if (countformat == 0)
                            {
                                dtDetailDataNew.Columns[i].DataType = typeof(DateTime);
                            }

                        }
                        if (objData[i].ToString() == null)
                        {
                            objData[i] = null;
                        }
                    }
                    if (dr["MOLDPARAMETER"].ToString() != null && dr["MOLDPARAMETER"].ToString().Length > 0)
                    {
                        dtDetailDataNew.Rows.Add(objData);
                    }
                    countformat = countformat + 1;
                }

                if (!dtDetailDataNew.Columns.Contains("ANALYTICALBATCHID"))
                {
                    dtDetailDataNew.Columns.Add("ANALYTICALBATCHID", typeof(string));
                    dtDetailDataNew.Select("").ToList().ForEach(x => x["ANALYTICALBATCHID"] = qcbatchinfo.dtsample.Rows[0]["QCBATCHID"].ToString());
                }
                if (!dtDetailDataNew.Columns.Contains("uqAnalyticalBatchID"))
                {
                    dtDetailDataNew.Columns.Add("uqAnalyticalBatchID");

                    foreach (DataRow rows in dtDetailDataNew.Rows)
                    {

                        rows["uqAnalyticalBatchID"] = ABID;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void
            fillComboMold(ASPxSpreadsheet spreadSheet)
        {
            try
            {
                ((RibbonComboBoxItem)spreadSheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("JobID")).Items.Clear();
                ((RibbonComboBoxItem)spreadSheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Items.Clear();
                if (qcbatchinfo.dtsample != null && qcbatchinfo.dtsample.Rows.Count > 0)
                {
                    DataTable dtJobID = qcbatchinfo.dtsample.DefaultView.ToTable(true, "JOBID");
                    DataTable dtSampleID = qcbatchinfo.dtsample.DefaultView.ToTable(true, "SYSSAMPLECODE");
                    foreach (DataRow drJobID in dtJobID.Rows)
                    {
                        if (drJobID["JOBID"] != null)
                        {
                            ((RibbonComboBoxItem)spreadSheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("JobID")).Items.Add(drJobID["JOBID"].ToString());
                        }
                    }
                    foreach (DataRow drSample in dtSampleID.Rows)
                    {
                        if (drSample["SYSSAMPLECODE"] != null)
                        {
                            ((RibbonComboBoxItem)spreadSheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Items.Add(drSample["SYSSAMPLECODE"].ToString());
                        }
                    }
                    //spreadSheet.Document.Worksheets.ActiveWorksheet = spreadSheet.Document.Worksheets[0];
                    ((RibbonComboBoxItem)spreadSheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Value = spreadSheet.Document.Worksheets.ActiveWorksheet.Name;
                }
                else
                {
                    SpreadSheetEntry_AnalyticalBatch objAB = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                    IList<SpreadsheetEntry_MoldResults> moldResults = os.GetObjects<SpreadsheetEntry_MoldResults>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", objAB.Oid));
                    if (moldResults != null)
                    {
                        foreach (SpreadsheetEntry_MoldResults moldResult in moldResults)
                        {
                            ((RibbonComboBoxItem)spreadSheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("SampleID")).Items.Add(moldResult.SampleID);
                        }
                    }
                }
                //((RibbonButtonItem)spreadSheet.RibbonTabs.FindByName("SDMS").Groups.FindByName("group0").Items.FindByName("Previous")).ClientEnabled = false;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void bindMoldSample(ASPxSpreadsheet spreadSheet)
        {
            try
            {
                // mergeSheet(spreadSheet);
                //DataRow[] drrSample = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable' AND Write = TRUE AND Continuous= TRUE");
                //DataRow[] drrSelect = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable'");
                //string strSampleID = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "SampleID").Select(r => r["Position"].ToString()).SingleOrDefault();
                //int intStartRow = int.Parse(Regex.Replace(strSampleID, @"[^\d]", ""));
                //Worksheet worksheet = spreadSheet.Document.Worksheets["Report"];
                //CellRange RanData = worksheet.GetDataRange();
                //int intlastUsedRow = RanData.BottomRowIndex + 1;
                //int intlastUsedColumn = RanData.RightColumnIndex + 1;
                //CellRange range = worksheet.Range[string.Format("A{0}:{01}{02}", intStartRow - 1, diIndexToColumn[intlastUsedColumn], intlastUsedRow)];
                //DataTable dtMold = worksheet.CreateDataTable(range, true);

                //for (int col = 0; col < range.ColumnCount; col++)
                //{
                //    CellValueType cellType = range[0, col].Value.Type;
                //    for (int r = 1; r < range.RowCount; r++)
                //    {
                //        if (cellType != range[r, col].Value.Type)
                //        {
                //            dtMold.Columns[col].DataType = typeof(string);
                //            break;
                //        }
                //    }
                //}

                //DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dtMold, true);
                //exporter.CellValueConversionError += Exporter_CellValueConversionError;
                //// Perform the export.
                //exporter.Export();
                //qcbatchinfo.dtMold = dtMold;

                for (int i = 0; i <= spreadSheet.Document.Worksheets.Count - 1; i++)
                {
                    if (spreadSheet.Document.Worksheets[i].Name != "Mold" && spreadSheet.Document.Worksheets[i].Name != "Report")
                    {
                        convertSheettoDateTable(spreadSheet.Document.Worksheets[i]);
                    }
                }

                //InsertUpdateMoldAirResults
                if (qcbatchinfo.strTest == "Mold_Air")
                {
                    SpreadSheetEntry_AnalyticalBatch objAB = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strAB));
                    IList<SpreadsheetEntry_MoldResults> MoldResults = os.GetObjects<SpreadsheetEntry_MoldResults>(CriteriaOperator.Parse("[uqAnalyticalBatchID]=?", objAB.Oid));
                    if (MoldResults != null)
                    {
                        foreach (SpreadsheetEntry_MoldResults objMoldResult in MoldResults)
                        {
                            SpreadsheetEntry_MoldAirResults MoldAirResult = null;
                            IList<SpreadsheetEntry_MoldAirResults> AirResults = os.GetObjects<SpreadsheetEntry_MoldAirResults>(CriteriaOperator.Parse("[uqMoldResultsID]=?", objMoldResult.Oid));
                            DataView dvMold = new DataView(qcbatchinfo.dtMold, "uqSampleParameterID = '" + objMoldResult.uqSampleParameterID.Oid + "'", "", DataViewRowState.CurrentRows);
                            for (int i = 0; i < dvMold.ToTable().Rows.Count; i++)
                            {
                                string strMoldParameterName = dvMold.ToTable().Rows[i]["MoldParameter"].ToString();
                                MoldParameters MoldOid = os.FindObject<MoldParameters>(CriteriaOperator.Parse("[MoldParameter]=?", strMoldParameterName));
                                if (AirResults != null && MoldOid != null && AirResults.Cast<SpreadsheetEntry_MoldAirResults>().Where(a => a.uqMoldResultsID != null && a.uqMoldResultsID.Oid.ToString() == MoldOid.Oid.ToString()).Count() > 0)
                                {
                                    MoldAirResult = AirResults.Cast<SpreadsheetEntry_MoldAirResults>().Where(a => a.uqMoldResultsID != null && a.uqMoldResultsID.Oid.ToString() == MoldOid.Oid.ToString()).FirstOrDefault();
                                }
                                else
                                {
                                    MoldAirResult = os.CreateObject<SpreadsheetEntry_MoldAirResults>();
                                }
                                if (MoldAirResult != null)
                                {
                                    MoldAirResult.uqMoldResultsID = objMoldResult;
                                    MoldAirResult.uqMoldParameterID = MoldOid;
                                    string strColumnName = string.Empty;
                                    foreach (DataColumn column in dvMold.ToTable().Columns)
                                    {
                                        if (column.ColumnName[0] == '%')
                                        {
                                            strColumnName = column.ColumnName.Replace(@"%", "P_");
                                        }
                                        else
                                        {
                                            strColumnName = column.ColumnName;
                                        }
                                        var sproperty = MoldAirResult.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name.ToUpper() == strColumnName && column.ColumnName != "OID").FirstOrDefault();
                                        if (sproperty != null && !dvMold.ToTable().Rows[i].IsNull(column.ColumnName))
                                        {
                                            if (sproperty.MappingFieldDBType.ToString() == dvMold.ToTable().Rows[i][column.ColumnName].GetType().Name.ToString())
                                            {
                                                if (sproperty.MappingFieldDBType != DBColumnType.Guid)
                                                {
                                                    sproperty.SetValue(MoldAirResult, dvMold.ToTable().Rows[i][column.ColumnName]);
                                                }
                                                else
                                                {
                                                    var Objrefrence = os.FindObject(Type.GetType(sproperty.ReferenceType.FullName + "," + sproperty.ReferenceType.AssemblyName), CriteriaOperator.Parse("[Oid]=?", dvMold.ToTable().Rows[i][column.ColumnName]));
                                                    if (Objrefrence != null)
                                                    {
                                                        sproperty.SetValue(MoldAirResult, Objrefrence);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (sproperty.MappingFieldDBType == DBColumnType.Guid && sproperty.ReferenceType.TableName == "Employee")
                                                {
                                                    var Objrefrence = os.FindObject<PermissionPolicyUser>(CriteriaOperator.Parse("[UserName]=?", dvMold.ToTable().Rows[i][column.ColumnName]));
                                                    if (Objrefrence != null)
                                                    {
                                                        sproperty.SetValue(MoldAirResult, Objrefrence);
                                                    }
                                                }
                                                else if (sproperty.MappingFieldDBType != DBColumnType.Guid)
                                                {
                                                    Type type = Type.GetType(sproperty.MemberType.FullName);
                                                    if (type != null)
                                                    {
                                                        sproperty.SetValue(MoldAirResult, Convert.ChangeType(dvMold.ToTable().Rows[i][column.ColumnName], type));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    os.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void convertSheettoDateTable(Worksheet wsMold)
        {
            DataRow[] drrSample = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable' AND Write = TRUE AND Continuous= TRUE");
            DataRow[] drrSelect = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable'");
            string strSampleID = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "SampleID").Select(r => r["Position"].ToString()).SingleOrDefault();
            int intStartRow = int.Parse(Regex.Replace(strSampleID, @"[^\d]", ""));
            //Worksheet worksheet = spreadSheet.Document.Worksheets["Report"];
            CellRange RanData = wsMold.GetDataRange();
            int intlastUsedRow = RanData.BottomRowIndex + 1;
            int intlastUsedColumn = RanData.RightColumnIndex + 1;
            CellRange range = wsMold.Range[string.Format("A{0}:{01}{02}", intStartRow - 1, diIndexToColumn[intlastUsedColumn], intlastUsedRow)];
            DataTable dtMold = wsMold.CreateDataTable(range, true);

            for (int col = 0; col < range.ColumnCount; col++)
            {
                CellValueType cellType = range[0, col].Value.Type;
                for (int r = 1; r < range.RowCount; r++)
                {
                    if (cellType != range[r, col].Value.Type)
                    {
                        dtMold.Columns[col].DataType = typeof(string);
                        break;
                    }
                }
            }

            DataTableExporter exporter = wsMold.CreateDataTableExporter(range, dtMold, true);
            exporter.CellValueConversionError += Exporter_CellValueConversionError;
            // Perform the export.
            exporter.Export();
            if (dtMold != null && dtMold.Rows.Count > 0)
            {
                if (qcbatchinfo.dtMold == null)
                {
                    qcbatchinfo.dtMold = dtMold.Clone();
                }
                foreach (DataRow dr in dtMold.Rows)
                {
                    DataRow drNewRow = qcbatchinfo.dtMold.NewRow();
                    foreach (DataColumn column in dtMold.Columns)
                    {
                        drNewRow[column.ColumnName] = dr[column.ColumnName];
                    }
                    qcbatchinfo.dtMold.Rows.Add(drNewRow);
                }
            }
        }
        private void mergeSheet(ASPxSpreadsheet spreadSheet)
        {
            try
            {
                if (spreadSheet.Document.Worksheets.Contains("Report"))
                {
                    spreadSheet.Document.Worksheets.Remove(spreadSheet.Document.Worksheets["Report"]);
                }
                DataRow[] drrSelect = qcbatchinfo.dtDataParsing.Select("RunType = 'RawDataTable'");
                string strSampleID = drrSelect.ToList().Where(r => r["FieldName"].ToString() == "SampleID").Select(r => r["Position"].ToString()).SingleOrDefault();
                int intStartRow = int.Parse(Regex.Replace(strSampleID, @"[^\d]", "")) - 1;
                Worksheet ws = spreadSheet.Document.Worksheets.Add("Report");
                ws.VisibilityType = WorksheetVisibilityType.Hidden;
                Worksheet worksheet = spreadSheet.Document.Worksheets[0];
                CellRange usedRange = worksheet.GetUsedRange();
                CellRange completeRange = worksheet.Range.FromLTRB(0, usedRange.TopRowIndex, 16383, usedRange.BottomRowIndex);
                spreadSheet.Document.Worksheets[spreadSheet.Document.Worksheets.Count - 1].Cells["A1"].CopyFrom(completeRange);
                int intBootomIndex = ws.GetUsedRange().BottomRowIndex;
                for (int intSheet = 1; intSheet <= spreadSheet.Document.Worksheets.Count - 1; intSheet++)
                {
                    Worksheet worksheet1 = spreadSheet.Document.Worksheets[intSheet];
                    if (worksheet1.Name != "Mold" && worksheet1.Name != "Report")
                    {
                        CellRange usedRange1 = worksheet1.GetUsedRange();
                        CellRange completeRange1 = worksheet1.Range.FromLTRB(0, intStartRow, 16383, usedRange1.BottomRowIndex);
                        spreadSheet.Document.Worksheets[spreadSheet.Document.Worksheets.Count - 1].Cells[string.Format("A{0}", intBootomIndex + 2)].CopyFrom(completeRange1);
                        intBootomIndex = ws.GetUsedRange().BottomRowIndex;
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
