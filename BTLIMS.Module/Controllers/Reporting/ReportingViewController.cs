using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Pdf;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraReports.UI;
using DynamicDesigner;
using iTextSharp.text.pdf;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
//using Rebex.Net;
//using Rebex.Net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Xml;
using WebSupergoo.ABCpdf5;
namespace BTLIMS.Module.Controllers
{
    public partial class ReportingWebController : ViewController, IXafCallbackHandler
    {
        viewInfo strviewid = new viewInfo();
        bool isnotreport = false;
        SampleParameter sampleParameter = null;
        MessageTimer timer = new MessageTimer();
        AuditInfo objAuditInfo = new AuditInfo();
        CaseNarativeInfo CNInfo = new CaseNarativeInfo();


        ReportCommentVM reportcomment = new ReportCommentVM();
        #region Variable Declaration
        XmlDocument xmlDoc = new XmlDocument();
        XmlNodeList nodes = default(XmlNodeList);
        XmlNodeList bnodes = default(XmlNodeList);
        public string strFTPServerName = string.Empty;
        public string strFTPPath = string.Empty;
        public string strFTPUserName = string.Empty;
        public string strFTPPassword = string.Empty;
        public int FTPPort = 0;
        public bool strFTPStatus;
        public bool EditCancel;
        bool boolReportSave = false;
        private SingleChoiceAction cmbReportName;
        string strReportIDT = string.Empty;
        private string uqOid;
        private string JobID;
        private string SampleID;
        private string QcBatchID;
        private string SampleParameterID;
        private string TestMethodID;
        private string ParameterID;
        DynamicReportDesignerConnection ObjReportDesignerInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        SampleRegistrationInfo Sampleinfo = new SampleRegistrationInfo();
        ShowNavigationItemController ShowNavigationController;
        ReportingQueryPanelInfo objRQPInfo = new ReportingQueryPanelInfo();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        DefaultSetting objDefaultReportValidation;
        DefaultSetting objDefaultReportApprove;
        DefaultSetting objDefaultReportDelivery;
        DefaultSetting objDefaultReportArchive;
        DefaultSetting objDefaultReportprintanddownload;
        DefaultSettingInfo objDefaultInfo = new DefaultSettingInfo();
        curlanguage objLanguage = new curlanguage();
        ICallbackManagerHolder SaveReportEdit;
        #endregion

        #region Constructor
        public ReportingWebController()
        {
            InitializeComponent();
            TargetViewId = "Reporting_SampleParameter_ListView;" + "Reporting_ListView;" + "Reporting_ListView_Copy_ReportApproval;" + "Samplecheckin_ListView_Copy_Reporting;" +
                "Reporting_ListView_Copy_ReportView;" + "SampleParameter_ListView_Copy_Reporting_MainView;" + "SampleParameter_ListView_Copy_CustomReporting;" +
                "SampleParameter_ListView_Copy_CustomReporting_Edit;" + "ReportRollbackLog_ListView;" + "Reporting_ListView_Level1Review_View;" +
                "Reporting_ListView_Level2Review_View;" + "Reporting_DetailView_Revision;" + "Reporting_DetailView_Revision_Copy;"
                + "Notes_ListView_CaseNarrative;";
            cmbReportName = new SingleChoiceAction(this, "Report", "View");
            cmbReportName.TargetViewId = "SampleParameter_ListView_Copy_CustomReporting;" + "SampleParameter_ListView_Copy_CustomReporting_Edit;";
            ReportDelete.TargetViewId = "Reporting_ListView;" + "Reporting_ListView_Copy_ReportApproval;" + "Reporting_ListView_Copy_ReportView;" + "Reporting_ListView_Level1Review_View;";
            Reportview.TargetViewId = "SampleParameter_ListView_Copy_Reporting_MainView" + "Samplecheckin_ListView_Copy_Reporting;";
            EditReport.TargetViewId = "Reporting_ListView_Copy_ReportView";
            ReportPreview.TargetViewId = "SampleParameter_ListView_Copy_CustomReporting_Edit;" + "SampleParameter_ListView_Copy_CustomReporting;";
            SaveReport.TargetViewId = /*"SampleParameter_ListView_Copy_CustomReporting_Edit;" + */"SampleParameter_ListView_Copy_CustomReporting;";
            SaveReport.ConfirmationMessage = "Do you want to save the report?";
            SaveReportView.TargetViewId = "SampleParameter_ListView_Copy_CustomReporting_Edit;";
            Comment.TargetViewId = "SampleParameter_ListView_Copy_CustomReporting";
            cmbReportName.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption;
            cmbReportName.Caption = "Report Template";
            cmbReportName.Category = "Edit";
            cmbReportName.Id = "cmbReportName";
            //var item = new ChoiceActionItem();
            //cmbReportName.Items.Add(new ChoiceActionItem("", item));
            DocumentPreview.TargetViewId = "SampleParameter_ListView_Copy_CustomReporting_Edit;" + "SampleParameter_ListView_Copy_CustomReporting;";
            ExcelPreview.TargetViewId = "SampleParameter_ListView_Copy_CustomReporting_Edit;" + "SampleParameter_ListView_Copy_CustomReporting;";
            //ReportRollback.TargetViewId = "Reporting_ListView_Copy_ReportView;";
            PreviewRollbackReport.TargetViewId = "ReportRollbackLog_ListView;";
            Level1ReviewView.TargetViewId = "Reporting_ListView;";
            Level2ReviewView.TargetViewId = "Reporting_ListView_Copy_ReportApproval;";
            Level2ReviewViewDateFilter.TargetViewId = "Reporting_ListView_Copy_ReportView;";
            Retrive.TargetViewId = "Samplecheckin_ListView_Copy_Reporting;";
            CaseNarrative.TargetViewId = "SampleParameter_ListView_Copy_CustomReporting;";
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                objDefaultReportprintanddownload = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportPrintDownload'"));
                objDefaultReportValidation = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportValidation'"));
                objDefaultReportApprove = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportApproval'"));
                objDefaultReportDelivery = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportDelivery'"));
                objDefaultReportArchive = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportArchive'"));
                if (objDefaultReportprintanddownload != null && objDefaultReportprintanddownload.Select == true)
                {
                    objDefaultInfo.boolReportPrintDownload = true;
                }
                if (objDefaultReportValidation != null && objDefaultReportValidation.Select == true)
                {
                    objDefaultInfo.boolReportValidation = true;
                }
                if (objDefaultReportApprove != null && objDefaultReportApprove.Select == true)
                {
                    objDefaultInfo.boolReportApprove = true;
                }
                if (objDefaultReportDelivery != null && objDefaultReportDelivery.Select == true)
                {
                    objDefaultInfo.boolReportdelivery = true;
                }
                if (objDefaultReportArchive != null && objDefaultReportArchive.Select == true)
                {
                    objDefaultInfo.boolReportArchive = true;
                }
                if (View != null && View.Id == "SampleParameter_ListView_Copy_Reporting_MainView" || View.Id == "Samplecheckin_ListView_Copy_Reporting")
                {
                    strviewid.strtempresultentryviewid = View.Id.ToString();
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
                if (View.Id == "SampleParameter_ListView_Copy_CustomReporting_Edit" || View.Id == "SampleParameter_ListView_Copy_CustomReporting")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    ObjectSpace.ObjectChanged += objectSpace_ObjectChanged;
                    ObjectSpace.Committed += objectSpace_Committed;
                    //SaveReportView.ExecuteCanceled += SaveReportView_ExecuteCanceled;
                    Modules.BusinessObjects.SampleManagement.Reporting objReporting = ObjectSpace.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(objRQPInfo.curReport);
                    if (objReporting != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", objReporting.SampleParameter.Select(i => i.Oid));
                    }
                }
                ExcelPreview.Active["ShowExcel"] = false;
                //Session currentSession = ((DevExpress.ExpressApp.Xpo.XPObjectSpace)(this.ObjectSpace)).Session;
                SelectedData sprocPackageName = currentSession.ExecuteSproc("SelectPackageName_SP", "");
                if (sprocPackageName.ResultSet != null)
                {
                    if (cmbReportName != null && cmbReportName.Items != null && cmbReportName.Items.Count > 0)
                    {
                        cmbReportName.Items.Clear();
                    }
                    cmbReportName.Items.Add(new ChoiceActionItem("", new ChoiceActionItem()));
                    foreach (SelectStatementResultRow row in sprocPackageName.ResultSet[0].Rows)
                    {
                        if (cmbReportName.Items.FindItemByID(row.Values[0].ToString()) == null)
                        {
                            var item = new ChoiceActionItem();
                            cmbReportName.Items.Add(new ChoiceActionItem(row.Values[0].ToString(), item));
                        }
                    }
                    if (View.Id == "SampleParameter_ListView_Copy_CustomReporting_Edit" && objRQPInfo.curReport != null)
                    {
                        ChoiceActionItem itemToSelect = cmbReportName.Items.FindItemByID(objRQPInfo.curReport.ReportName);
                        cmbReportName.SelectedItem = (itemToSelect != null) ? itemToSelect : null;
                    }
                }

                Employee currentUser = SecuritySystem.CurrentUser as Employee;
                DynamicDesigner.GlobalReportSourceCode.strReportedBy = Convert.ToString(currentUser.DisplayName);
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
                        }
                        else
                        {
                            foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Custom Reporting" && i.Write == true) != null)
                                {
                                    objPermissionInfo.CustomReportingIsWrite = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Custom Reporting" && i.Delete == true) != null)
                                {
                                    objPermissionInfo.CustomReportingIsDelete = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportValidation" && i.Write == true) != null)
                                {
                                    objPermissionInfo.ReportValidationIsWrite = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportValidation" && i.Delete == true) != null)
                                {
                                    objPermissionInfo.ReportValidationIsDelete = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportApproval" && i.Write == true) != null)
                                {
                                    objPermissionInfo.ReportApprovalIsWrite = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportApproval" && i.Delete == true) != null)
                                {
                                    objPermissionInfo.ReportApprovalIsDelete = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Report View" && i.Write == true) != null)
                                {
                                    objPermissionInfo.ReportViewIsWrite = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Report View" && i.Delete == true) != null)
                                {
                                    objPermissionInfo.ReportViewIsDelete = true;
                                    //return;
                                }
                                if (objPermissionInfo.ResultEntryIsWrite == true && objPermissionInfo.ResultEntryIsDelete == true)
                                {
                                    return;
                                }
                            }
                        }
                    }

                    if (View.Id == "SampleParameter_ListView_Copy_CustomReporting")
                    {
                        SaveReport.Active.SetItemValue("Reportpreview.SaveReport", objPermissionInfo.CustomReportingIsWrite);
                        Comment.Active.SetItemValue("Reportpreview.Comment", objPermissionInfo.CustomReportingIsWrite);
                    }
                    else
                    if (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Level1Review_View")
                    {
                        ReportValidate.Active.SetItemValue("ReportValidation.ReportValidate", objPermissionInfo.ReportValidationIsWrite);
                        ReportDelete.Active.SetItemValue("ShowReportDelete", objPermissionInfo.ReportValidationIsDelete);
                    }
                    else
                    if (View.Id == "Reporting_ListView_Copy_ReportApproval")
                    {
                        ReportApproval.Active.SetItemValue("ReportApprove.ReportApproval", objPermissionInfo.ReportApprovalIsWrite);
                        ReportDelete.Active.SetItemValue("ShowReportDelete", objPermissionInfo.ReportApprovalIsDelete);
                        ReportDelete.Active.SetItemValue("ShowReportDelete", false);
                    }
                    else if (View.Id == "Reporting_ListView_Copy_ReportView")
                    {
                        if (objnavigationRefresh.ClickedNavigationItem == "Custom Reporting")
                        {
                            ReportDelete.Active.SetItemValue("ShowReportDelete", objPermissionInfo.CustomReportingIsDelete);
                            EditReport.Active.SetItemValue("ShowReportEdit", objPermissionInfo.CustomReportingIsWrite);
                            //ReportRollback.Active.SetItemValue("ShowReportRollback", objPermissionInfo.CustomReportingIsWrite);
                        }
                        else
                        {
                            ReportDelete.Active.SetItemValue("ShowReportDelete", objPermissionInfo.ReportViewIsDelete);
                            EditReport.Active.SetItemValue("ShowReportEdit", objPermissionInfo.ReportViewIsWrite);
                            //ReportRollback.Active.SetItemValue("ShowReportRollback", objPermissionInfo.ReportViewIsWrite);
                        }
                        WebWindow.CurrentRequestWindow.PagePreRender += new EventHandler(CurrentRequestWindow_PagePreRender);

                        //if (objDefault != null)
                        //{
                        //    objDefault = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparation'AND [Select]=True"));
                        //    if(objDefault != null)
                        //    {
                        //        ASPxGridListEditor GridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        //        GridViewColumn column in gridListEditor.Grid.VisibleColumns
                        //        if (GridListEditor != null)
                        //        {
                        //            if (column.Caption == "DateValidated")
                        //            {
                        //                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 5].VisibleIndex;
                        //            }
                        //        }

                        //    }
                        //}

                        if (Level2ReviewViewDateFilter.SelectedItem == null)
                        {
                            DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                            DateTime srDateFilter = DateTime.MinValue;
                            if (Level2ReviewViewDateFilter.SelectedItem == null)
                            {
                                if (setting.ReportingWorkFlow == EnumDateFilter.OneMonth)
                                {
                                    srDateFilter = DateTime.Today.AddMonths(-1);
                                    Level2ReviewViewDateFilter.SelectedItem = Level2ReviewViewDateFilter.Items[0];
                                }
                                else if (setting.ReportingWorkFlow == EnumDateFilter.ThreeMonth)
                                {
                                    srDateFilter = DateTime.Today.AddMonths(-3);
                                    Level2ReviewViewDateFilter.SelectedItem = Level2ReviewViewDateFilter.Items[1];
                                }
                                else if (setting.ReportingWorkFlow == EnumDateFilter.SixMonth)
                                {
                                    srDateFilter = DateTime.Today.AddMonths(-6);
                                    Level2ReviewViewDateFilter.SelectedItem = Level2ReviewViewDateFilter.Items[2];
                                }
                                else if (setting.ReportingWorkFlow == EnumDateFilter.OneYear)
                                {
                                    srDateFilter = DateTime.Today.AddYears(-1);
                                    Level2ReviewViewDateFilter.SelectedItem = Level2ReviewViewDateFilter.Items[3];
                                }
                                else if (setting.ReportingWorkFlow == EnumDateFilter.TwoYear)
                                {
                                    srDateFilter = DateTime.Today.AddYears(-2);
                                    Level2ReviewViewDateFilter.SelectedItem = Level2ReviewViewDateFilter.Items[4];
                                }
                                else if (setting.ReportingWorkFlow == EnumDateFilter.FiveYear)
                                {
                                    srDateFilter = DateTime.Today.AddYears(-5);
                                    Level2ReviewViewDateFilter.SelectedItem = Level2ReviewViewDateFilter.Items[5];
                                }
                                else
                                {
                                    srDateFilter = DateTime.MinValue;
                                    Level2ReviewViewDateFilter.SelectedItem = Level2ReviewViewDateFilter.Items[6];
                                }
                                //Level2ReviewViewDateFilter.SelectedItem = Level2ReviewViewDateFilter.Items[0];
                            }
                            if (srDateFilter != DateTime.MinValue)
                            {
                                //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[ReportedDate] BETWEEN('" + srDateFilter + "', '" + DateTime.Now + "')");
                                ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[ReportedDate] >= ? And [ReportedDate] < ?", srDateFilter, DateTime.Now);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria.Clear();
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

     

        private void CurrentRequestWindow_PagePreRender(object sender, EventArgs e)
        {


            //DefaultSetting objdefsetting = (DefaultSetting)View.CurrentObject;
            //if(objdefsetting != null)
            //{
            if(View.Id== "Reporting_ListView_Copy_ReportView")
            {
                DefaultSetting objDefault = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'RawDataLevel2BatchReview'  And [Select] = True"));
                DefaultSetting objDefault1 = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse(" [NavigationItemNameID] = 'RawDataLevel3BatchReview' And [Select] = True"));
                if (objDefault != null || objDefault1 !=null )
                {
                    ASPxGridListEditor GridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    foreach (GridViewColumn column in GridListEditor.Grid.Columns)
                    {
                        if (GridListEditor != null )
                        {
                            if ( objDefault1 != null)
                            {
                                if (column.Caption == "ReportApprovedBy")
                                {
                                    column.VisibleIndex = 8;
                                }
                                if (column.Caption == "ReportApprovedDate")
                                {
                                    column.VisibleIndex = 9;
                                }

                            }
                            if (objDefault != null)
                            {
                                if (column.Caption == "ReportValidatedBy")
                                {
                                    column.VisibleIndex = 10;
                                }
                                if (column.Caption == "ReportValidatedDate")
                                {
                                    column.VisibleIndex = 11;
                                }

                            }
                            if(objDefault != null && objDefault1 != null)
                            {
                                GridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;

                            }

                        }
                    }


                }
            }
           
               // }
                
            
        }

        //private void SaveReportView_ExecuteCanceled(object sender, ActionBaseEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void objectSpace_Committed(object sender, EventArgs e)
        {
            try
            {
                if (View != null)
                {
                    View.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (View.Id == "SampleParameter_ListView_Copy_CustomReporting_Edit" || View.Id == "SampleParameter_ListView_Copy_CustomReporting")
            {
                if (e.PropertyName == "NotReport")
                {

                    SampleParameter objsample = (SampleParameter)e.Object;
                    if (objsample.NotReport == true)
                    {
                        objsample.Status = Samplestatus.Reported;
                        objsample.OSSync = true;
                    }
                    else
                    {
                        objsample.Status = Samplestatus.PendingReporting;
                        objsample.OSSync = true;
                    }
                    //sampleParameter = objsample;
                    //isnotreport = objsample.NotReport;
                }
            }
        }

        protected override void OnViewControlsCreated()
        {
            try
            {
                if (View.Id == "Notes_ListView_CaseNarrative")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        ASPxGridView gridView = gridlisteditor.Grid;
                        if (gridView != null)
                        {
                            gridView.PreRender += GridView_PreRender;
                        }
                    }
                }

                if (View.Id == "SampleParameter_ListView_Copy_CustomReporting_Edit")
                {
                    SaveReportEdit = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    SaveReportEdit.CallbackManager.RegisterHandler("SaveReportEdit", this);
                }

                if (View.Id == "Reporting_DetailView_Revision" || View.Id == "Reporting_DetailView_Revision_Copy")
                {
                    StaticTextViewItem sam = ((DetailView)View).FindItem("ReasonID") as StaticTextViewItem;
                    if (sam != null)
                    {
                        if (((DetailView)View).ViewEditMode == DevExpress.ExpressApp.Editors.ViewEditMode.Edit)
                        {
                            Modules.BusinessObjects.SampleManagement.Reporting objReporting = View.CurrentObject as Modules.BusinessObjects.SampleManagement.Reporting;

                            if (objReporting != null)
                            {
                                sam.Text = "Changes made on " + objReporting.JobID.JobID + ", please provide the comment";
                            }

                        }
                        else
                        {
                            sam.Text = "";
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

        private void GridView_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "Notes_ListView_CaseNarrative")
                {
                    ASPxGridView grid = (ASPxGridView)sender;
                    if (grid != null)
                    {
                        foreach (Notes objPriority in ((ListView)View).CollectionSource.List)
                        {


                            Notes notes = ObjectSpace.FindObject<Notes>(CriteriaOperator.Parse("[Oid]=?",objPriority.Oid));
                            if (notes.IsCaseNarrative==true)
                            {
                                grid.Selection.SelectRowByKey(objPriority.Oid);
                            }
                            else if( notes.IsCaseNarrative==false)
                            {
                                grid.Selection.UnselectRowByKey(objPriority.Oid);

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

        private void Tar_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (View.Id == "SampleParameter_ListView_Copy_Reporting_MainView")
                {
                    //objspace = Application.CreateObjectSpace();
                    SampleParameter obj = (SampleParameter)e.InnerArgs.CurrentObject;
                    IList<SampleLogIn> objsampleLoglist = ObjectSpace.GetObjects<SampleLogIn>(CriteriaOperator.Parse("[JobID.JobID] = ?", obj.Samplelogin.JobID.JobID));
                    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(SampleParameter));
                    cs.Criteria["Filter"] = new InOperator("Samplelogin", objsampleLoglist);
                    if (objRQPInfo.rgFilterByJobID == "PendingJobID")
                    {
                        cs.Criteria["JobID"] = CriteriaOperator.Parse("[Status]='PendingReporting'");
                    }
                    else if (objRQPInfo.rgFilterByJobID == "AllJobID")
                    {
                        cs.Criteria["JobID"] = CriteriaOperator.Parse("[Status]='PendingReporting' or [Status]='Reported'");
                    }
                    e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateListView("SampleParameter_ListView_Copy_CustomReporting", cs, false);
                    e.Handled = true;
                }
                //else if (View.Id == "Samplecheckin_ListView_Copy_Reporting")
                //{
                //    IObjectSpace objspace = Application.CreateObjectSpace();
                //    Samplecheckin obj = (Samplecheckin)e.InnerArgs.CurrentObject;
                //    IList<SampleLogIn> objsampleLoglist = objspace.GetObjects<SampleLogIn>(CriteriaOperator.Parse("[JobID.JobID] = ?", obj.JobID));
                //    CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                //    cs.Criteria["Filter"] = new InOperator("Samplelogin", objsampleLoglist);
                //    if (objRQPInfo.rgFilterByJobID == "PendingJobID")
                //    {
                //        cs.Criteria["JobID"] = CriteriaOperator.Parse("[Status]='PendingReporting' And [Testparameter.GCRecord] Is NULL");
                //    }
                //    else if (objRQPInfo.rgFilterByJobID == "AllJobID")
                //    {
                //        cs.Criteria["JobID"] = CriteriaOperator.Parse("([Status]='PendingReporting' or [Status]='Reported') And [Testparameter.GCRecord] Is NULL");
                //    }
                //    //e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateListView("SampleParameter_ListView_Copy_CustomReporting", cs, false);
                //    //ListView listView = Application.CreateListView("SampleParameter_ListView_Copy_CustomReporting", cs, false);
                //    //Frame.SetView(listView);
                //    Frame.SetView(Application.CreateListView("SampleParameter_ListView_Copy_CustomReporting", cs, true));
                //    e.Handled = true;
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Retrive_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_ListView_Copy_Reporting")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        IObjectSpace objspace = Application.CreateObjectSpace();
                        //  IList<SampleLogIn> sampleLogList = new List<SampleLogIn>();
                        // IList<Samplecheckin> obj = View.SelectedObjects.Cast<Samplecheckin>().ToList();
                        List<Samplecheckin> lstSampleCheckins = e.SelectedObjects.Cast<Samplecheckin>().ToList();
                        var ch = lstSampleCheckins.Select(i => i.JobID).Distinct().ToList();
                        if (ch != null)
                        {
                            foreach (Samplecheckin checkin in lstSampleCheckins.ToList())
                            {
                                if (checkin != null)
                                {
                                    CNInfo.RpJobId = checkin.JobID;
                                    CNInfo.SCoidValue = checkin.Oid;
                                    if (!string.IsNullOrEmpty(checkin.SampleMatries))
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        foreach (string strMatrix in checkin.SampleMatries.Split(';'))
                                        {
                                            VisualMatrix objSM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strMatrix));
                                            if (sb.Length > 0)
                                            {
                                                sb.Append(";"); // Add semicolon before appending the next name
                                            }
                                            sb.Append(objSM.VisualMatrixName);
                                        }
                                        CNInfo.RpSampleMatries = sb.ToString();
                                    }
                                }
                            }
                        }
                        //foreach (Samplecheckin currentSamplecheckin in obj)
                        //{
                        //    //  IList<SampleLogIn> lobjsampleLoglist = objspace.GetObjects<SampleLogIn>(CriteriaOperator.Parse("[JobID.JobID] = ?", currentSamplecheckin.JobID));
                        //    CriteriaOperator lcs = CriteriaOperator.Parse("[JobID.JobID] = ?", currentSamplecheckin.JobID);

                        //}
                        //  ICollection<Guid> jobIds = obj.JobIDs;

                        // Use the InOperator to filter SampleLogIn objects based on multiple JobID values
                        CriteriaOperator lcs = new InOperator("JobID.JobID", ch.ToArray());

                        IList<SampleLogIn> objsampleLoglist = objspace.GetObjects<SampleLogIn>(lcs);

                        //  IList<SampleLogIn> objsampleLoglist = objspace.GetObjects<SampleLogIn>(CriteriaOperator.Parse("[JobID.JobID] = ?", obj));
                        CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                        cs.Criteria["Filter"] = new InOperator("Samplelogin", objsampleLoglist);
                        if (objRQPInfo.rgFilterByJobID == "PendingJobID")
                        {
                            cs.Criteria["JobID"] = CriteriaOperator.Parse("[Status]='PendingReporting' And [Testparameter.GCRecord] Is NULL  And ([TestHold] = False Or [TestHold] Is null)");
                        }
                        else if (objRQPInfo.rgFilterByJobID == "AllJobID")
                        {
                            cs.Criteria["JobID"] = CriteriaOperator.Parse("([Status]='PendingReporting' or [Status]='Reported') And [Testparameter.GCRecord] Is NULL And ([TestHold] = False Or [TestHold] Is null)");
                        }

                        Frame.SetView(Application.CreateListView("SampleParameter_ListView_Copy_CustomReporting", cs, true));

                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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

        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();
                objDefaultInfo.boolReportPrintDownload = false;
                objDefaultInfo.boolReportValidation = false;
                objDefaultInfo.boolReportApprove = false;
                objDefaultInfo.boolReportdelivery = false;
                objDefaultInfo.boolReportArchive = false;
                ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                Frame.GetController<ListViewProcessCurrentObjectController>().CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
                DynamicDesigner.GlobalReportSourceCode.struqSampleParameterID = string.Empty;
                DynamicDesigner.GlobalReportSourceCode.strJobID = string.Empty;
                ReportDelete.Active.RemoveItem("ShowReportDelete");
                if (View.Id == "SampleParameter_ListView_Copy_CustomReporting_Edit" || View.Id == "SampleParameter_ListView_Copy_CustomReporting")
                {
                    objRQPInfo.curReport = null;
                    ObjReportingInfo.strReportID = string.Empty;
                    CNInfo.SCoidValue = Guid.Empty;
                    CNInfo.RpJobId = null;
                    CNInfo.RpSampleMatries = null;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Functions
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

        //public void ReadXmlFile_FTPConc()
        //{
        //    try
        //    {
        //        string[] FTPconnectionstring = ObjReportDesignerInfo.WebConfigFTPConn.Split(';');
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
        //                FTP.Timeout = 6000;
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
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //        return null;
        //    }
        //}
        #endregion

        #region SimpleActionEvents
        private void Level1ReviewView_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Modules.BusinessObjects.SampleManagement.Reporting));
                cs.Criteria["Filter"] = CriteriaOperator.Parse("[ReportStatus]>0");
                Frame.SetView(Application.CreateListView("Reporting_ListView_Level1Review_View", cs, false));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Level2ReviewView_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Modules.BusinessObjects.SampleManagement.Reporting));
                cs.Criteria["Filter"] = CriteriaOperator.Parse("[ReportStatus]>1");
                Frame.SetView(Application.CreateListView("Reporting_ListView_Level2Review_View", cs, false));
            }

            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ReportPreview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                FileData fileData = null;
                ReportCommentVM objinfo = new ReportCommentVM();
                string value = objinfo.Comment;
                bool UnApprove = false;
                byte[] attachedFileBytes;
                bool COCpresent = false;
                //strReportIDT = null;
                //string struqSampleParameterID = string.Empty;
                if (cmbReportName.SelectedItem != null && cmbReportName.SelectedIndex != 0)
                {
                    if (View.Id == "SampleParameter_ListView_Copy_CustomReporting" || View.Id == "SampleParameter_ListView_Copy_CustomReporting_Edit")
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            bool IsAllowMultipleJobID = true;
                            List<string> lstreport = new List<string>();
                            IList<ReportPackage> objrep = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?", cmbReportName.SelectedItem.ToString()));
                            if (objrep != null)
                            {
                                foreach (ReportPackage objrp in objrep.ToList())
                                {
                                    lstreport.Add(objrp.ReportName);
                                }

                            }

                            List<SampleParameter> listSP = View.SelectedObjects.Cast<SampleParameter>().ToList();
                            foreach (SampleParameter objpara in listSP)
                            {
                                IList<Modules.BusinessObjects.SampleManagement.Attachment> objrepCOCs = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Attachment>(CriteriaOperator.Parse("[Samplecheckin]=?", objpara.Samplelogin.JobID.Oid));

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
                            if (/*(jobid.Count() == 1 || jobid.Count() == 0) &&*/ IsAllowMultipleJobID == true)
                            {
                                uqOid = null;
                                foreach (SampleParameter obj in View.SelectedObjects)
                                {
                                    if (obj.NotReport == false)
                                    {
                                        if (uqOid == null)
                                        {
                                            uqOid = "'" + obj.Oid.ToString() + "'";
                                            JobID = "'" + obj.Samplelogin.JobID.JobID + "'";
                                            SampleID = "'" + obj.Samplelogin.SampleID + "'";
                                            //if (obj.QCBatchID != null && obj.QCBatchID.qcseqdetail != null)
                                            //{
                                            //    QcBatchID = "'" + obj.QCBatchID.qcseqdetail.AnalyticalBatchID + "'";
                                            //}
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
                                    //if (obj.Status == "Pending Entry" || obj.Status == "Pending Validation" || obj.Status == "Pending Approval")
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
                                    ReportIDFormat(currentSession);
                                }
                                //if (boolReportSave == true && string.IsNullOrEmpty(strReportIDT))
                                //{
                                //    SelectedData sproc = currentSession.ExecuteSproc("ReportingV5_CreateNewID_SP");
                                //    strReportIDT = sproc.ResultSet[0].Rows[0].Values[0].ToString();
                                //}
                                //ObjReportingInfo.struqSampleParameterID = struqSampleParameterID = uqOid;
                                ObjReportingInfo.struqSampleParameterID = uqOid;
                                ObjReportingInfo.strJobID = JobID;
                                ObjReportingInfo.strSampleID = SampleID;
                                ObjReportingInfo.strLimsReportedDate = DateTime.Now.ToString("MM/dd/yyyy");
                                ObjReportingInfo.strQcBatchID = QcBatchID;
                                if (!string.IsNullOrEmpty(strReportIDT))
                                {
                                    ObjReportingInfo.strReportID = strReportIDT;
                                }
                                ObjReportingInfo.strTestMethodID = TestMethodID;
                                ObjReportingInfo.strParameterID = ParameterID;
                                ObjReportingInfo.strviewid = View.Id.ToString();

                                GlobalReportSourceCode.struqSampleParameterID = ObjReportingInfo.struqSampleParameterID;
                                GlobalReportSourceCode.strTestMethodID = TestMethodID;
                                GlobalReportSourceCode.strParameterID = ParameterID;
                                Company cmp = ObjectSpace.FindObject<Company>(CriteriaOperator.Parse(""));
                                if (cmp != null && cmp.Logo != null)
                                {
                                    GlobalReportSourceCode.strLogo = Convert.ToBase64String(cmp.Logo);
                                }
                                DynamicDesigner.GlobalReportSourceCode.struqQCBatchID = QcBatchID;
                                DynamicDesigner.GlobalReportSourceCode.strJobID = JobID;
                                //DynamicDesigner.GlobalReportSourceCode.strSampleParameterOid = SampleParameterID;
                                List<string> listPage = new List<string>();
                                int pagenumber = 0;
                                //SelectedData sproclang = currentSession.ExecuteSproc("getCurrentLanguage", "");
                                //var CurrentLanguage = sproclang.ResultSet[1].Rows[0].Values[0].ToString();
                                using (MemoryStream newms = new MemoryStream())
                                {
                                    if (objrep != null && objrep.Count > 0)
                                    {
                                        var sortobj = objrep.OrderBy(x => x.sort);
                                        //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                                        //stopwatch.Start();
                                        //List<string> lstElapsedTime = new List<string>();
                                        //TimeSpan timeelapsed2 = new TimeSpan();
                                        DataSet reportDataSet = new DataSet("ReportDataSet");
                                        DataTable reportTable = new DataTable("Reports");
                                        reportTable.Columns.Add("ReportName", typeof(string));
                                        reportTable.Columns.Add("Sort", typeof(int));
                                        reportDataSet.Tables.Add(reportTable);
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
                                                if (report.ReportName == "xrEnvTRRPQCComborptnew" || report.ReportName == "xr_280011_QCReport")
                                                {
                                                    //ObjReportingInfo.strTestMethodID = TestMethodID;
                                                    GlobalReportSourceCode.dsQCDataSource = DynamicReportBusinessLayer.BLCommon.GetQcComboReportTRRp_DataSet("Env_QCPotraitRegular_RPT_SP", ObjReportingInfo.strSampleID, ObjReportingInfo.struqSampleParameterID, ObjReportingInfo.strTestMethodID, ObjReportingInfo.strParameterID);
                                                }
                                                tempxtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(report.ReportName.ToString(), ObjReportingInfo, false);
                                                DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(tempxtraReport, ObjReportingInfo);
                                                tempxtraReport.CreateDocument();
                                                //get temppath
                                                string tempPath = Path.GetTempPath();
                                                // export the dynamicreports into memeory stram and update into data table with temp path
                                                using (MemoryStream ms = new MemoryStream())
                                                {
                                                    tempxtraReport.ExportToPdf(ms);
                                                    string finalFilePath = Path.Combine(Path.GetTempPath(), report.ReportName);
                                                    File.WriteAllBytes(finalFilePath, ms.ToArray());
                                                    DataRow newRow = reportTable.NewRow();
                                                    newRow["ReportName"] = finalFilePath;
                                                    if (report.sort > 0)
                                                    {
                                                        newRow["Sort"] = report.sort;
                                                    }
                                                    else
                                                    {
                                                        newRow["Sort"] = 0;
                                                    }
                                                    if (report.ReportName != "COC")
                                                    {
                                                        reportTable.Rows.Add(newRow);
                                                    }
                                                }
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
                                        //if (UnApprove == true)
                                        //{
                                        //    //xtraReport.Watermark.Text = "NOT APPROVED";
                                        //    //xtraReport.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                                        //    //xtraReport.Watermark.TextTransparency = 200;
                                        //    //xtraReport.Watermark.ShowBehind = true;
                                        //}
                                        IList<ReportPackage> objreps = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?", cmbReportName.SelectedItem.ToString()));
                                        foreach (ReportPackage package in objreps)
                                        {
                                            if (package.NPUserDefinedReportName == "COC")
                                            {
                                                var jobIDs = listSP.Where(i => i.Samplelogin?.JobID != null).Select(i => i.Samplelogin.JobID.Oid).Distinct().ToList();
                                                foreach (Guid oid in jobIDs)
                                                {
                                                    IList<Modules.BusinessObjects.SampleManagement.Attachment> objrepCOCs = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Attachment>(CriteriaOperator.Parse("[Samplecheckin.oid]=? and [Category] = 'COC'", oid));
                                                    foreach (Modules.BusinessObjects.SampleManagement.Attachment attachment in objrepCOCs.Where(i => i.Attachments != null && !string.IsNullOrEmpty(i.Attachments.FileName)))
                                                    {

                                                        FileData lfileData = attachment.Attachments as FileData;
                                                        if (lfileData != null && lfileData.Content != null)
                                                        {
                                                            string fileName = lfileData.FileName;
                                                            byte[] fileContent = lfileData.Content;

                                                            // Create a temporary file
                                                            string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
                                                            File.WriteAllBytes(tempFilePath, fileContent);
                                                            DataRow newRow = reportTable.NewRow();
                                                            newRow["ReportName"] = tempFilePath;
                                                            if (package.ReportName == "COC" && package.sort > 0)
                                                            {
                                                                newRow["Sort"] = package.sort;
                                                            }
                                                            else
                                                            {
                                                                newRow["Sort"] = 0;
                                                            }

                                                            // newRow["ReportContent"] = reportBytes;
                                                            reportTable.Rows.Add(newRow);
                                                        }
                                                    }
                                                }
                                                Doc doc = new Doc();
                                                doc.SetInfo(0, "License", "322-594-815-276-8035-241");
                                                Doc y = new Doc();
                                                y.SetInfo(0, "License", "322-594-815-276-8035-241");
                                                DataView view = reportTable.DefaultView;
                                                view.Sort = "Sort ASC";
                                                DataTable sortedTable = view.ToTable();

                                                int totalPageCount = 0;
                                                foreach (DataRow row in sortedTable.Rows)
                                                {
                                                    string Getpath = row["ReportName"].ToString();
                                                    doc.Read(Getpath);
                                                    //  int intPageNo = 0;
                                                    //  int theCount = doc.PageCount; 
                                                    if (doc.PageCount > 0)
                                                    {
                                                        totalPageCount += doc.PageCount;
                                                    }
                                                }
                                                COCpresent = true;
                                                int intPageNo = 0;
                                                foreach (DataRow row in sortedTable.Rows)
                                                {
                                                    string Getpath = row["ReportName"].ToString();
                                                    doc.Read(Getpath);
                                                    //  int intPageNo = 0;
                                                    //  int theCount = doc.PageCount; 
                                                    if (doc.PageCount > 0)
                                                    {
                                                        for (int i = 1; i <= doc.PageCount; i++)
                                                        {
                                                            intPageNo++;
                                                            y.MediaBox.Position(0, 0);
                                                            y.MediaBox.Height = doc.MediaBox.Height;
                                                            y.MediaBox.Width = doc.MediaBox.Width;
                                                            y.Rect.Width = doc.Rect.Width;
                                                            y.Rect.Height = doc.Rect.Height;
                                                            y.Rect.Left = 10;
                                                            y.Rect.Bottom = 0;
                                                            y.Page = y.AddPage();
                                                            y.AddImageDoc(doc, i, null);
                                                            y.Pos.Y = 20;
                                                            y.Pos.X = doc.MediaBox.Width / 2;
                                                            y.FontSize = 8;
                                                            //  y.Font = y.EmbedFont(Multilanguage.BT_Font, "Unicode", false, true); // Assuming Multilanguage.BT_Font is properly defined

                                                            //  string p = "Page " + intPageNo + " of " + theCount;
                                                            string p = "Page " + intPageNo + " of " + totalPageCount;
                                                            y.AddText(p);
                                                            y.Save(newms);
                                                            string finalFilePath = Path.Combine(Path.GetTempPath(), "lFinalMergedReport.pdf");
                                                            File.WriteAllBytes(finalFilePath, newms.ToArray());
                                                        }
                                                    }

                                                }
                                                PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor();
                                                using (MemoryStream memoryStream = new MemoryStream())
                                                {
                                                    y.Save(memoryStream);
                                                    //  y.Save(newms);
                                                    // File.WriteAllBytes(finalFilePath, newms.ToArray());
                                                    // mergedDoc.Save(newms);
                                                    string finalFilePath = Path.Combine(Path.GetTempPath(), "FinalMergedReport.pdf");
                                                    File.WriteAllBytes(finalFilePath, memoryStream.ToArray());
                                                    documentProcessor.LoadDocument(memoryStream);
                                                    documentProcessor.SaveDocument(newms);
                                                    // File.WriteAllBytes(finalFilePath, newms.ToArray());

                                                    //open into local
                                                    //System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                                    //{
                                                    //    FileName = finalFilePath,
                                                    //    UseShellExecute = true,
                                                    //    // Verb = "open"
                                                    //});
                                                }
                                            }
                                        }

                                        //using (MemoryStream ms = new MemoryStream())
                                        //{
                                        //    xtraReport.ExportToPdf(ms);
                                        //    using (PdfDocumentProcessor source = new PdfDocumentProcessor())
                                        //    {
                                        //        source.LoadDocument(ms);
                                        //        foreach (DevExpress.Pdf.PdfPage page in source.Document.Pages)
                                        //        {
                                        //            var curpageval = listPage[source.Document.Pages.IndexOf(page)];
                                        //            if (curpageval.Length > 0)
                                        //            {
                                        //                using (DevExpress.Pdf.PdfGraphics graphics = source.CreateGraphics())
                                        //                {
                                        //                    DevExpress.Pdf.PdfRectangle rectangle = page.MediaBox;
                                        //                    RectangleF r = new RectangleF((float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);
                                        //                    SolidBrush black = (SolidBrush)Brushes.Black;
                                        //                    using (Font font = new Font("Microsoft Yahei", 11, FontStyle.Regular))
                                        //                    {
                                        //                        string text;
                                        //                        if (objLanguage.strcurlanguage == "En")
                                        //                        {
                                        //                            text = "Total " + pagenumber + " of " + curpageval + " page";
                                        //                        }
                                        //                        else
                                        //                        {
                                        //                            text = "共 " + pagenumber + " 页 第 " + curpageval + " 页";
                                        //                        }
                                        //                        graphics.DrawString(text, font, black, r.Width + 48, 170);
                                        //                    }
                                        //                    graphics.AddToPageForeground(page);
                                        //                }
                                        //            }
                                        //        }
                                        //        source.SaveDocument(newms);
                                        //    }
                                        //}

                                        if (!COCpresent)
                                        {
                                            Doc doc = new Doc();
                                            doc.SetInfo(0, "License", "322-594-815-276-8035-241");
                                            Doc y = new Doc();
                                            y.SetInfo(0, "License", "322-594-815-276-8035-241");
                                            DataView view = reportTable.DefaultView;
                                            view.Sort = "Sort ASC";

                                            DataTable sortedTable = view.ToTable();
                                            int totalPageCount = 0;
                                            foreach (DataRow row in sortedTable.Rows)
                                            {
                                                string Getpath = row["ReportName"].ToString();
                                                doc.Read(Getpath);
                                                //  int intPageNo = 0;
                                                //  int theCount = doc.PageCount; 
                                                if (doc.PageCount > 0)
                                                {
                                                    totalPageCount += doc.PageCount;
                                                }
                                            }
                                            int intPageNo = 0;
                                            foreach (DataRow row in sortedTable.Rows)
                                            {
                                                string Getpath = row["ReportName"].ToString();
                                                doc.Read(Getpath);
                                                // int intPageNo = 0;
                                                int theCount = doc.PageCount; // Assuming this is the total number of pages

                                                if (doc.PageCount > 0)
                                                {
                                                    for (int i = 1; i <= doc.PageCount; i++)
                                                    {
                                                        intPageNo++;

                                                        y.MediaBox.Position(0, 0);
                                                        y.MediaBox.Height = doc.MediaBox.Height;
                                                        y.MediaBox.Width = doc.MediaBox.Width;
                                                        y.Rect.Width = doc.Rect.Width;
                                                        y.Rect.Height = doc.Rect.Height;
                                                        y.Rect.Left = 10;
                                                        y.Rect.Bottom = 0;

                                                        y.Page = y.AddPage();
                                                        y.AddImageDoc(doc, i, null);

                                                        y.Pos.Y = 20;
                                                        y.Pos.X = doc.MediaBox.Width / 2;
                                                        y.FontSize = 8;
                                                        string p = "Page " + intPageNo + " of " + totalPageCount;
                                                        //  string p = "Page " + intPageNo + " of " + theCount;
                                                        y.AddText(p);
                                                        y.Save(newms);
                                                        string finalFilePath = Path.Combine(Path.GetTempPath(), "lFinalMergedReport.pdf");
                                                        File.WriteAllBytes(finalFilePath, newms.ToArray());
                                                    }
                                                }


                                            }
                                            COCpresent = false;
                                            using (MemoryStream ms = new MemoryStream())
                                            {

                                                using (PdfDocumentProcessor source = new PdfDocumentProcessor())
                                                {
                                                    y.Save(ms);
                                                    string finalFilePath = Path.Combine(Path.GetTempPath(), "FinalMergedReport.pdf");
                                                    File.WriteAllBytes(finalFilePath, ms.ToArray());
                                                    source.LoadDocument(ms);
                                                    //source.SaveDocument(newms);
                                                    //xtraReport.ExportToPdf(ms);
                                                    //source.LoadDocument(ms);
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
                                        //stopwatch3.Stop();
                                        //string totaltimeelapsed = (stopwatch.Elapsed.Seconds + stopwatch3.Elapsed.Seconds).ToString();
                                    }
                                    else
                                    {
                                        string strReport = cmbReportName.SelectedItem.ToString();
                                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(strReport, ObjReportingInfo, false);
                                        DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport, ObjReportingInfo);
                                        //if (UnApprove == true)
                                        //{
                                        //    //xtraReport.Watermark.Text = "NOT APPROVED";
                                        //    //xtraReport.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                                        //    //xtraReport.Watermark.TextTransparency = 200;
                                        //    //xtraReport.Watermark.ShowBehind = true;
                                        //}
                                        xtraReport.ExportToPdf(newms);
                                    }
                                    //stopwatch3.Stop();

                                    newms.Position = 0;
                                    if (boolReportSave == true)
                                    {
                                        //Ftp _FTP = GetFTPConnection();
                                        //string strRemotePath = /*"//CONSCI//LDMReports//"*/ConfigurationManager.AppSettings["FinalReportPath"];
                                        //_FTP.Passive = true;
                                        //string currentPath = _FTP.GetCurrentDirectory();
                                        //String[] Char = { "/", "\\" };
                                        //string[] subDirs = strRemotePath.Split(Char, StringSplitOptions.None);
                                        //string Originalpath = string.Empty;
                                        //if (currentPath != "/")
                                        //{
                                        //    Originalpath = currentPath + strRemotePath;
                                        //}
                                        //else
                                        //{
                                        //    Originalpath = strRemotePath;
                                        //}
                                        //bool newPathExist = false;
                                        //if (!_FTP.DirectoryExists(Originalpath))
                                        //{
                                        //    foreach (string subDir in subDirs)
                                        //    {
                                        //        try
                                        //        {
                                        //            if (!string.IsNullOrEmpty(subDir))
                                        //            {
                                        //                currentPath = currentPath + "/" + subDir;
                                        //                _FTP.CreateDirectory(currentPath);
                                        //                newPathExist = true;
                                        //            }
                                        //        }
                                        //        catch (Exception ex)
                                        //        {
                                        //            Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                                        //            Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        //        }
                                        //    }
                                        //}
                                        //string strFilePath = string.Empty;
                                        //if (newPathExist)
                                        //{
                                        //    if (!Originalpath.EndsWith("/") && !Originalpath.EndsWith(@"\"))
                                        //    {
                                        //        strFilePath = Originalpath.Replace(@"\", "//") + "/" + strReportIDT + ".pdf";
                                        //    }
                                        //    else
                                        //    {
                                        //        strFilePath = Originalpath.Replace(@"\", "//") + strReportIDT + ".pdf";
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    if (!Originalpath.EndsWith("/") && !Originalpath.EndsWith(@"\"))
                                        //    {
                                        //        strFilePath = Originalpath.Replace(@"\", "//") + "/" + strReportIDT + ".pdf";
                                        //    }
                                        //    else
                                        //    {
                                        //        strFilePath = Originalpath.Replace(@"\", "//") + strReportIDT + ".pdf";
                                        //    }
                                        //}
                                        //_FTP.PutFile(newms, strFilePath);
                                        Sampleinfo.bytevalues = newms.ToArray();
                                        Sampleinfo.curReportName = cmbReportName.SelectedItem.ToString();
                                        boolReportSave = false;
                                    }
                                    else
                                    {
                                        MemoryStream tempms = new MemoryStream();
                                        // fileData.SaveToStream(tempms);
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
                                        Sampleinfo.curReportName = cmbReportName.SelectedItem.ToString();
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
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectreport"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    return;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (cmbReportName.SelectedItem != null && cmbReportName.SelectedIndex != 0)// null)
                {
                    if (View != null && View.SelectedObjects.Count > 0)
                    {
                        bool IsAllowMultipleJobID = true;
                        List<string> lstreport = new List<string>();
                        IList<ReportPackage> objrep = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?", cmbReportName.SelectedItem.ToString()));
                        foreach (ReportPackage objrp in objrep.ToList())
                        {
                            lstreport.Add(objrp.ReportName);
                        }

                        List<SampleParameter> lstsamparameter = e.SelectedObjects.Cast<SampleParameter>().ToList();
                        var jobid = lstsamparameter.Select(i => i.Samplelogin.JobID).Distinct().ToList();
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

                        if (/*jobid.Count() == 1*/IsAllowMultipleJobID == true)
                        {

                            ////ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                            ////ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            ////SetConnectionString();
                            ////ReadXmlFile_FTPConc();
                            ////Rebex.Net.Ftp _FTP = GetFTPConnection();
                            //////foreach (SampleParameter obj in View.SelectedObjects)
                            //{
                            //    if (obj.Status == Samplestatus.PendingEntry || obj.Status == Samplestatus.PendingValidation || obj.Status == Samplestatus.PendingApproval)
                            //    {
                            //        boolReportSave = false;
                            //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "withoutapprove"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            //        ReportPreview_Execute(new object(), new SimpleActionExecuteEventArgs(null, null));
                            //        return;
                            //    }
                            //}
                            if (View.SelectedObjects.Cast<SampleParameter>().ToList().FirstOrDefault(i => i.Status == Samplestatus.PendingEntry || i.Status == Samplestatus.PendingValidation || i.Status == Samplestatus.PendingApproval) != null)
                            {
                                boolReportSave = false;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "withoutapprove"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                ReportPreview_Execute(new object(), new SimpleActionExecuteEventArgs(null, null));
                                return;
                            }

                            //if ((_FTP.State == FtpState.Ready))
                            //{
                            boolReportSave = true;
                            bool stat;
                            ////ReportPreview_Execute(new object(), new SimpleActionExecuteEventArgs(null, null));

                            IObjectSpace os = this.ObjectSpace;
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                            if (boolReportSave == true)
                            {
                                strReportIDT = null;
                                ReportIDFormat(currentSession);

                            }
                            ReportIDFormat idFormat = ObjectSpace.FindObject<ReportIDFormat>(null);
                            Modules.BusinessObjects.SampleManagement.Reporting objReporting = new Modules.BusinessObjects.SampleManagement.Reporting(currentSession);
                            objReporting.ReportID = strReportIDT;
                            if (idFormat != null)
                            {
                                objReporting.LastSequentialNumber = Convert.ToInt32(idFormat.SequentialNumber);
                            }
                            objReporting.ReportedDate = DateTime.Now;
                            objReporting.ReportedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objReporting.ReportName = cmbReportName.SelectedItem.Caption;
                            objReporting.NewReportFormat = true;
                            objReporting.ReportStatus = ReportStatus.Pending1stReview;
                            if (!string.IsNullOrEmpty(objRQPInfo.RevisionReason))
                            {
                                objReporting.ReportID = strReportIDT = objRQPInfo.PreviousReportID + 'R' + objRQPInfo.Revision.ToString();
                                objReporting.RevisionDate = DateTime.Now;
                                objReporting.RevisionBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.RevisionReason = objRQPInfo.RevisionReason;
                                objReporting.RevisionNo = objRQPInfo.Revision;
                                objReporting.PreviousReportID = objRQPInfo.PreviousReportID;
                                objRQPInfo.RevisionReason = null;
                            }
                            Guid customerOid = View.SelectedObjects.Cast<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.ClientName != null).Select(i => i.Samplelogin.JobID.ClientName.Oid).FirstOrDefault();
                            if (customerOid != null)
                            {
                                Customer objCustomer = os.FindObject<Customer>(CriteriaOperator.Parse("Oid=?", customerOid));
                                //if (objCustomer != null && !objCustomer.Mail)
                                //{
                                //    objReporting.Email = objCustomer.Contacts.Where(i => i.Email != null && i.IsReport == true).Select(i => i.Email).FirstOrDefault();
                                //}
                                if (objCustomer != null && !objCustomer.Mail)
                                {
                                    IList<Contact> objconEmail = os.GetObjects<Contact>(CriteriaOperator.Parse("Not IsNullOrEmpty([Email]) And [Customer.Oid] = ? And ReportDelivery = true", objCustomer.Oid));
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
                                        objReporting.Email = lstmail;
                                    }
                                }
                                else if(objCustomer.Mail)
                                {
                                    objReporting.Mail = true;
                                }
                            }
                            // DefaultSetting defaultSetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse(""));                       
                            //if (defaultSetting.ReportValidate == EnumRELevelSetup.No && defaultSetting.ReportApprove == EnumRELevelSetup.Yes)
                            //{
                            //    objReporting.ReportValidatedDate = DateTime.Now;
                            //    objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //    objReporting.ReportStatus = ReportStatus.Pending2ndReview;
                            //}
                            //else if (defaultSetting.ReportValidate == EnumRELevelSetup.No && defaultSetting.ReportApprove == EnumRELevelSetup.No)
                            //{
                            //    objReporting.ReportValidatedDate = DateTime.Now;
                            //    objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //    objReporting.ReportApprovedDate = DateTime.Now;
                            //    objReporting.ReportApprovedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //    objReporting.ReportStatus = ReportStatus.PendingDelivery;

                            //    stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportPrintDownloadShow"]);
                            //    if (!stat)
                            //    {
                            //        objReporting.DatePrinted = DateTime.Now;
                            //        objReporting.PrintedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //    }
                            //}

                            //if (objDefaultReportValidation !=null && objDefaultReportValidation.Select  == false && objDefaultReportApprove!=null && objDefaultReportApprove.Select ==true)
                            //{
                            //    objReporting.ReportValidatedDate = DateTime.Now;
                            //    objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //    objReporting.ReportStatus = ReportStatus.Pending2ndReview;
                            //}
                            //else if (objDefaultReportValidation !=null && objDefaultReportValidation.Select ==false && objDefaultReportApprove !=null && objDefaultReportApprove.Select==false)
                            //{
                            //    objReporting.ReportValidatedDate = DateTime.Now;
                            //    objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //    objReporting.ReportApprovedDate = DateTime.Now;
                            //    objReporting.ReportApprovedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //    objReporting.ReportStatus = ReportStatus.PendingDelivery;

                            //    stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportPrintDownloadShow"]);
                            //    if (!stat)
                            //    {
                            //        objReporting.DatePrinted = DateTime.Now;
                            //        objReporting.PrintedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //    }
                            //}
                            if (objDefaultInfo.boolReportValidation == true)
                            {
                                //if (objDefaultReportValidation !=null && objDefaultReportValidation.Select  == false && objDefaultReportApprove!=null && objDefaultReportApprove.Select ==true)
                                //{
                                objReporting.ReportStatus = ReportStatus.Pending1stReview;
                                AddReportSign(objReporting, IsReported: true, IsValidated: false, IsApproved: false);
                            }
                            if (objDefaultInfo.boolReportValidation == false && objDefaultInfo.boolReportApprove == true)
                            {
                                objReporting.ReportValidatedDate = DateTime.Now;
                                objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.LastUpdatedDate = DateTime.Now;
                                objReporting.LastUpdatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.ReportStatus = ReportStatus.Pending2ndReview;
                                AddReportSign(objReporting, IsReported: true, IsValidated: true, IsApproved: false);
                            }
                            if (objDefaultInfo.boolReportValidation == false && objDefaultInfo.boolReportApprove == false && objDefaultInfo.boolReportPrintDownload == true)
                            {
                                objReporting.ReportValidatedDate = DateTime.Now;
                                objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.ReportApprovedDate = DateTime.Now;
                                objReporting.ReportApprovedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.LastUpdatedDate = DateTime.Now;
                                objReporting.LastUpdatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.ReportStatus = ReportStatus.PendingPrint;
                            }
                            else if (objDefaultInfo.boolReportValidation == false && objDefaultInfo.boolReportApprove == false && objDefaultInfo.boolReportPrintDownload == false && objDefaultInfo.boolReportdelivery == true)
                            {
                                objReporting.ReportValidatedDate = DateTime.Now;
                                objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.ReportApprovedDate = DateTime.Now;
                                objReporting.ReportApprovedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.ReportStatus = ReportStatus.PendingDelivery;

                                stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportPrintDownloadShow"]);
                                if (!stat)
                                {
                                    objReporting.DatePrinted = DateTime.Now;
                                    objReporting.PrintedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    objReporting.ReportStatus = ReportStatus.PendingDelivery;
                                    ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                                    ////if (!stat)
                                    ////{
                                    ////    objReporting.DateDelivered = DateTime.Now;
                                    ////    objReporting.DeliveredBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    //objReporting.ReportApprovedDate = DateTime.Now;
                                    //objReporting.ReportApprovedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    // stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                                    ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                                    ////if (!stat)
                                    ////{
                                    ////    objReporting.DateArchived = DateTime.Now;
                                    ////    objReporting.ArchivedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    ////    objReporting.ReportStatus = ReportStatus.Archived;
                                }
                                AddReportSign(objReporting, IsReported: true, IsValidated: true, IsApproved: true);
                            }
                            else if (objDefaultInfo.boolReportValidation == false && objDefaultInfo.boolReportApprove == false && objDefaultInfo.boolReportPrintDownload == false && objDefaultInfo.boolReportdelivery == false && objDefaultInfo.boolReportArchive == true)
                            {
                                objReporting.ReportValidatedDate = DateTime.Now;
                                objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.ReportApprovedDate = DateTime.Now;
                                objReporting.ReportApprovedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.ReportStatus = ReportStatus.PendingArchive;
                                // stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                                Modules.BusinessObjects.Setting.DefaultSetting objReportDelivery = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'ReportDelivery'"));
                                if (objReportDelivery.Select == false)
                                {
                                    objReporting.DateDelivered = DateTime.Now;
                                    objReporting.DeliveredBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    objReporting.ReportStatus = ReportStatus.PendingArchive;
                                    ////}
                                    Modules.BusinessObjects.Setting.DefaultSetting objReportArchive = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'ReportArchive'"));
                                    if (objReportArchive.Select == false)
                                    {
                                        objReporting.DateArchived = DateTime.Now;
                                        objReporting.ArchivedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.ReportStatus = ReportStatus.Archived;
                                    }
                                }
                                AddReportSign(objReporting, IsReported: true, IsValidated: true, IsApproved: true);
                            }
                            else if (objDefaultInfo.boolReportValidation == false && objDefaultInfo.boolReportApprove == false && objDefaultInfo.boolReportPrintDownload == false && objDefaultInfo.boolReportdelivery == false && objDefaultInfo.boolReportArchive == false)
                            {
                                objReporting.ReportValidatedDate = DateTime.Now;
                                objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.ReportApprovedDate = DateTime.Now;
                                objReporting.ReportApprovedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.DeliveredBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReporting.ReportStatus = ReportStatus.ReportDelivered;
                                ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                                ////if (!stat)
                                ////{
                                ////    objReporting.DateArchived = DateTime.Now;
                                ////    objReporting.ArchivedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                ////    objReporting.ReportStatus = ReportStatus.Archived;
                                ////}
                                AddReportSign(objReporting, IsReported: true, IsValidated: true, IsApproved: true);
                            }


                            List<string> lstvisualmatrix = new List<string>();
                            foreach (SampleParameter objLineA in View.SelectedObjects)
                            {
                                CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + objLineA.Oid + "'");
                                SampleParameter objSample = objSpace.FindObject<SampleParameter>(criteria);
                                objReporting.SampleParameter.Add(objSample);
                                objSample.Status = Samplestatus.Reported;
                                objSample.OSSync = true;
                                if (objLineA.Samplelogin != null && objLineA.Samplelogin.VisualMatrix != null)
                                {
                                    if (!lstvisualmatrix.Contains(objLineA.Samplelogin.VisualMatrix.VisualMatrixName))
                                    {
                                        lstvisualmatrix.Add(objLineA.Samplelogin.VisualMatrix.VisualMatrixName);
                                    }
                                }
                                objReporting.JobID = objSample.Samplelogin.JobID;
                            }
                            objReporting.SampleType = string.Join(",", lstvisualmatrix);
                            //IObjectSpace oss = Application.CreateObjectSpace();
                            //List<Guid> lstGuid1 = View.SelectedObjects.Cast<SampleParameter>().ToList().Select(i => i.Oid).ToList();
                            if (objRQPInfo.curReport != null)
                            {
                                Modules.BusinessObjects.SampleManagement.Reporting objReporting1 = objSpace.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(objRQPInfo.curReport);
                                if (objReporting.ReportStatus == ReportStatus.Pending1stReview)
                                {
                                    objAuditInfo.SaveData = true;
                                    Frame.GetController<AuditlogViewController>().insertauditdata(objSpace, objReporting.JobID.Oid, OperationType.ValueChanged, "Report Tracking", objReporting.JobID.ToString(), "ReportName", objReporting1.ReportName, objReporting.ReportName, objReporting.RevisionReason);
                                    AuditData Objects = objSpace.FindObject<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                                    if (Objects != null)
                                    {
                                        Objects.CommentProcessed = true;
                                    }
                                }
                            }
                            //oss.Dispose();
                            objSpace.CommitChanges();
                            bool isSave = false;
                            IList<SampleParameter> lstsmpl1 = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Testparameter.QCType.QCTypeName] = 'Sample'", objReporting.JobID.Oid));
                            if (lstsmpl1.Count() == lstsmpl1.Where(i => i.Status == Samplestatus.Reported).Count())
                            {
                                StatusDefinition objStatus = objSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 23"));
                                if (objStatus != null)
                                {
                                    Samplecheckin objJobID = objSpace.GetObjectByKey<Samplecheckin>(objReporting.JobID.Oid);
                                    objJobID.Index = objStatus;
                                    isSave = true;
                                }
                            }
                            if (isSave)
                            {
                                objSpace.CommitChanges();
                            }
                            ReportPreview_Execute(new object(), new SimpleActionExecuteEventArgs(null, null));

                            FileLinkSepDBController obj = Frame.GetController<FileLinkSepDBController>();
                            if (obj != null)
                            {
                                obj.FileLink(strReportIDT, Sampleinfo.bytevalues);
                            }
                            List<SampleParameter> listSP = View.SelectedObjects.Cast<SampleParameter>().ToList();
                            IList<ReportPackage> objreps = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?", cmbReportName.SelectedItem.ToString()));
                            foreach (ReportPackage package in objreps)
                            {
                                if (package.NPUserDefinedReportName == "COC")
                                {
                                    foreach (Guid oid in listSP.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null).Select(i => i.Samplelogin.JobID.Oid).Distinct().ToList())
                                    {
                                        IList<Modules.BusinessObjects.SampleManagement.Attachment> objrepCOCs = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Attachment>(CriteriaOperator.Parse("[Samplecheckin.oid]=? and [Category] = 'COC'", oid));
                                        foreach (Modules.BusinessObjects.SampleManagement.Attachment attachment in objrepCOCs.Where(i => i.Attachments != null && !string.IsNullOrEmpty(i.Attachments.FileName)))
                                        {

                                        }
                                    }

                                }


                            }
                            List<Guid> lstGuid = View.SelectedObjects.Cast<SampleParameter>().ToList().Select(i => i.Oid).ToList();
                            Application.ShowViewStrategy.ShowMessage("Report " + strReportIDT + " saved successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                            ASPxGridListEditor aSPxGridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (aSPxGridListEditor != null)
                            {
                                aSPxGridListEditor.Refresh();

                            }
                            //e.Action.Application.MainWindow.View.ObjectSpace.Refresh();
                            // View.Close();
                            strReportIDT = null;
                            #region SuboutStatus
                            //IObjectSpace oss = Application.CreateObjectSpace();
                            //List<SampleParameter> lstSampleParam = oss.GetObjects<SampleParameter>(new InOperator("Oid", lstGuid)).ToList();
                            //if (lstSampleParam.ToList().FirstOrDefault(i => i.SubOut == true) != null)
                            //{
                            //    foreach (SubOutSampleRegistrations objSubout in lstSampleParam.Where(i => i.SubOut == true).Select(i => i.SuboutSample).Distinct().ToList())
                            //    {
                            //        if (objSubout.SampleParameter.ToList().FirstOrDefault(i => i.Status == Samplestatus.PendingEntry || i.Status == Samplestatus.SuboutPendingValidation || i.Status == Samplestatus.SuboutPendingApproval || i.Status == Samplestatus.PendingReporting) == null)
                            //        {
                            //            if (objSubout.SubOutQcSample.ToList().FirstOrDefault(i => i.Status == Samplestatus.PendingEntry || i.Status == Samplestatus.SuboutPendingValidation || i.Status == Samplestatus.SuboutPendingApproval) == null)
                            //            {
                            //                SubOutSampleRegistrations obj = oss.FindObject<SubOutSampleRegistrations>(CriteriaOperator.Parse("[Oid]=?", objSubout.Oid));
                            //                if (obj != null)
                            //                {
                            //                    obj.SuboutStatus = SuboutTrackingStatus.Reported;
                            //                }
                            //            }
                            //        }
                            //    }
                            //    oss.CommitChanges();
                            //}
                            //oss.Dispose();
                            #endregion
                            //ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                            //ChoiceActionItem parentReporting = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "Reporting");
                            //if(parentReporting!=null)
                            //{
                            //    ChoiceActionItem childCustomReport = parentReporting.Items.FirstOrDefault(i => i.Id == "Custom Reporting");
                            //    if(childCustomReport!=null)
                            //    {
                            //        int count = 0;
                            //        IObjectSpace objSpaceReport = Application.CreateObjectSpace();
                            //        using (XPView lstview = new XPView(((XPObjectSpace)objSpaceReport).Session, typeof(SampleParameter)))
                            //        {
                            //            lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingReporting' And Not IsNullOrEmpty([Samplelogin.JobID.JobID])");
                            //            lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                            //            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            //            List<object> jobid = new List<object>();
                            //            foreach (ViewRecord rec in lstview)
                            //                jobid.Add(rec["Toid"]);
                            //            count = jobid.Count;
                            //        }
                            //        var cap = childCustomReport.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //        if (count > 0)
                            //        {
                            //            childCustomReport.Caption = cap[0] + " (" + count + ")";
                            //        }
                            //        else
                            //        {
                            //            childCustomReport.Caption = cap[0];
                            //        }
                            //    }
                            //}

                            //    //ResetNavigationCount();
                            //}
                            //else
                            //{
                            //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "FTPNotConnected"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            //}
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowMultipleJOBID"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "nodata"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "reportselect"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AddReportSign(Modules.BusinessObjects.SampleManagement.Reporting objReporting, bool IsReported, bool IsValidated, bool IsApproved)
        {
            try
            {
                List<string> listPage = new List<string>();
                XtraReport xtraReport = new XtraReport();
                ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                //ReadXmlFile_FTPConc();
                //Ftp _FTP = GetFTPConnection();
                //string strRemotePath = /*"//CONSCI//LDMReports//";*/  ConfigurationManager.AppSettings["FinalReportPath"];
                //string strExportedPath = strRemotePath.Replace(@"\", "//") + objReporting.ReportID + ".pdf";
                //if (_FTP.FileExists(strExportedPath))
                FileLinkSepDBController objfilelink = Frame.GetController<FileLinkSepDBController>();
                if (objfilelink != null)
                {
                    DataTable dt = objfilelink.GetFileLink(objReporting.ReportID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        byte[] objbyte = (byte[])dt.Rows[0]["FileContent"];
                        MemoryStream ms = new MemoryStream(objbyte);
                        //_FTP.TransferType = FtpTransferType.Binary;
                        //_FTP.GetFile(strExportedPath, ms);

                        #region AddSignatureusingSpiredll
                        //Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                        //doc.LoadFromStream(ms);
                        //PdfTextFind[] results = null;
                        //foreach (PdfPageBase page in doc.Pages)
                        //{
                        //    if (IsReported)
                        //    {
                        //        string strSearchText = "报告人";
                        //        byte[] img;
                        //        if (objReporting.ReportedBy != null)
                        //        {
                        //            img = objReporting.ReportedBy.Signature;
                        //        }
                        //        else
                        //        {
                        //            img = ((Employee)SecuritySystem.CurrentUser).Signature;
                        //        }

                        //        if (page.FindText(strSearchText, TextFindParameter.None) != null)
                        //        {
                        //            results = page.FindText(strSearchText, TextFindParameter.None).Finds;
                        //            foreach (PdfTextFind text in results)
                        //            {
                        //                PointF p = new PointF(text.Position.X + 50, text.Position.Y - 8);
                        //                SizeF imgSize = new SizeF(100.5F, 20F);
                        //                MemoryStream imgms = new MemoryStream(img);
                        //                System.Drawing.Image imgSignature = System.Drawing.Image.FromStream(imgms);
                        //                Spire.Pdf.Graphics.PdfImage pdfimage = Spire.Pdf.Graphics.PdfImage.FromImage(imgSignature);
                        //                page.Canvas.DrawImage(pdfimage, p, imgSize);
                        //            }
                        //        }
                        //    }
                        //    if (IsValidated)
                        //    {
                        //        string strSearchText = "签发人";
                        //        byte[] img;
                        //        if (objReporting.ReportValidatedBy != null)
                        //        {
                        //            img = objReporting.ReportValidatedBy.Signature;
                        //        }
                        //        else
                        //        {
                        //            img = ((Employee)SecuritySystem.CurrentUser).Signature;
                        //        }

                        //        if (page.FindText(strSearchText, TextFindParameter.None) != null)
                        //        {
                        //            results = page.FindText(strSearchText, TextFindParameter.None).Finds;
                        //            foreach (PdfTextFind text in results)
                        //            {
                        //                PointF p = new PointF(text.Position.X + 50, text.Position.Y - 8);
                        //                SizeF imgSize = new SizeF(100.5F, 20F);
                        //                MemoryStream imgms = new MemoryStream(img);
                        //                System.Drawing.Image imgSignature = System.Drawing.Image.FromStream(imgms);
                        //                Spire.Pdf.Graphics.PdfImage pdfimage = Spire.Pdf.Graphics.PdfImage.FromImage(imgSignature);
                        //                page.Canvas.DrawImage(pdfimage, p, imgSize);
                        //            }
                        //        }
                        //    }
                        //    if (IsApproved)
                        //    {
                        //        string strSearchText = "批准人";
                        //        byte[] img;
                        //        if (objReporting.ReportApprovedBy != null)
                        //        {
                        //            img = objReporting.ReportApprovedBy.Signature; 
                        //        }
                        //        else
                        //        {
                        //            img = ((Employee)SecuritySystem.CurrentUser).Signature;
                        //        }

                        //        if (page.FindText(strSearchText, TextFindParameter.None) != null)
                        //        {
                        //            results = page.FindText(strSearchText, TextFindParameter.None).Finds;
                        //            foreach (PdfTextFind text in results)
                        //            {
                        //                PointF p = new PointF(text.Position.X + 50, text.Position.Y - 8);
                        //                SizeF imgSize = new SizeF(100.5F, 20F);
                        //                MemoryStream imgms = new MemoryStream(img);
                        //                System.Drawing.Image imgSignature = System.Drawing.Image.FromStream(imgms);
                        //                Spire.Pdf.Graphics.PdfImage pdfimage = Spire.Pdf.Graphics.PdfImage.FromImage(imgSignature);
                        //                page.Canvas.DrawImage(pdfimage, p, imgSize);
                        //            }
                        //        }
                        //    }
                        //}
                        //doc.SaveToStream(tempms);
                        #endregion

                        #region AddSignatureusingiTextSharpdll
                        //Create an instance of our strategy
                        LDM.Module.BusinessObjects.MyLocationTextExtractionStrategy reportedbyExtractor = new LDM.Module.BusinessObjects.MyLocationTextExtractionStrategy();
                        LDM.Module.BusinessObjects.MyLocationTextExtractionStrategy validatedbyExtractor = new LDM.Module.BusinessObjects.MyLocationTextExtractionStrategy();
                        LDM.Module.BusinessObjects.MyLocationTextExtractionStrategy approvedbyExtractor = new LDM.Module.BusinessObjects.MyLocationTextExtractionStrategy();

                        //reportedbyExtractor.TextToSearchFor = "报告人";
                        //validatedbyExtractor.TextToSearchFor = "签发人";
                        //approvedbyExtractor.TextToSearchFor = "批准人";

                        reportedbyExtractor.TextToSearchFor = "主检：";
                        validatedbyExtractor.TextToSearchFor = "审核：";
                        approvedbyExtractor.TextToSearchFor = "批准：";

                        MemoryStream tempms = new MemoryStream();
                        iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(ms.ToArray());

                        for (int pageno = 1; pageno <= reader.NumberOfPages; pageno++)
                        {
                            reportedbyExtractor.pageno = validatedbyExtractor.pageno = approvedbyExtractor.pageno = pageno;
                            string strReportedResult = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, pageno, reportedbyExtractor);
                            string strValidatedResult = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, pageno, validatedbyExtractor);
                            string strApprovedResult = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, pageno, approvedbyExtractor);
                        }

                        var stamper = new PdfStamper(reader, tempms);
                        for (int pageno = 1; pageno <= reader.NumberOfPages; pageno++)
                        {
                            var pdfContentByte = stamper.GetOverContent(pageno);
                            if (IsReported)
                            {
                                if (reportedbyExtractor != null && reportedbyExtractor.myPoints != null && reportedbyExtractor.myPoints.Count > 0)
                                {
                                    LDM.Module.BusinessObjects.GetTextAndRectangle extractor = reportedbyExtractor.myPoints.FirstOrDefault(i => i.pageno == pageno);
                                    if (extractor != null && extractor.Rect != null)
                                    {
                                        byte[] img;
                                        if (objReporting.ReportedBy != null)
                                        {
                                            img = objReporting.ReportedBy.Signature;
                                        }
                                        else
                                        {
                                            img = ((Employee)SecuritySystem.CurrentUser).Signature;
                                        }
                                        iTextSharp.text.Image sigimage = iTextSharp.text.Image.GetInstance(img);
                                        sigimage.SetAbsolutePosition(extractor.Rect.Left + extractor.Rect.Width + 20F, extractor.Rect.Top + extractor.Rect.Height - 25F);
                                        sigimage.ScaleToFit(105f, 25f);
                                        pdfContentByte.AddImage(sigimage);
                                    }
                                }
                            }
                            if (IsValidated)
                            {
                                if (validatedbyExtractor != null && validatedbyExtractor.myPoints != null && validatedbyExtractor.myPoints.Count > 0)
                                {
                                    LDM.Module.BusinessObjects.GetTextAndRectangle extractor = validatedbyExtractor.myPoints.FirstOrDefault(i => i.pageno == pageno);
                                    if (extractor != null && extractor.Rect != null)
                                    {
                                        byte[] img;
                                        if (objReporting.ReportValidatedBy != null)
                                        {
                                            img = objReporting.ReportValidatedBy.Signature;
                                        }
                                        else
                                        {
                                            img = ((Employee)SecuritySystem.CurrentUser).Signature;
                                        }
                                        iTextSharp.text.Image sigimage = iTextSharp.text.Image.GetInstance(img);
                                        sigimage.SetAbsolutePosition(extractor.Rect.Left + extractor.Rect.Width + 20F, extractor.Rect.Top + extractor.Rect.Height - 25F);
                                        sigimage.ScaleToFit(105f, 25f);
                                        pdfContentByte.AddImage(sigimage);
                                    }
                                }
                            }
                            if (IsApproved)
                            {
                                if (approvedbyExtractor != null && approvedbyExtractor.myPoints != null && approvedbyExtractor.myPoints.Count > 0)
                                {
                                    LDM.Module.BusinessObjects.GetTextAndRectangle extractor = approvedbyExtractor.myPoints.FirstOrDefault(i => i.pageno == pageno);
                                    if (extractor != null && extractor.Rect != null)
                                    {
                                        byte[] img;
                                        if (objReporting.ReportApprovedBy != null)
                                        {
                                            img = objReporting.ReportApprovedBy.Signature;
                                        }
                                        else
                                        {
                                            img = ((Employee)SecuritySystem.CurrentUser).Signature;
                                        }
                                        iTextSharp.text.Image sigimage = iTextSharp.text.Image.GetInstance(img);
                                        sigimage.SetAbsolutePosition(extractor.Rect.Left + extractor.Rect.Width + 20F, extractor.Rect.Top + extractor.Rect.Height - 25F);
                                        sigimage.ScaleToFit(105f, 25f);
                                        pdfContentByte.AddImage(sigimage);
                                    }
                                }
                            }
                        }
                        stamper.Close();
                        reader.Close();
                        #endregion

                        objfilelink.FileLinkUpdate(objReporting.ReportID, tempms.ToArray());
                        //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        //string strFilePath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".pdf");
                        //if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                        //{
                        //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                        //}
                        //File.WriteAllBytes(strFilePath, tempms.ToArray());
                        ////_FTP.DeleteFile(strExportedPath);
                        //////_FTP.PutFile(tempms, strExportedPath);
                        ////_FTP.PutFile(strFilePath, strExportedPath);
                        //FileInfo file = new FileInfo(strFilePath);
                        //if (file.Exists)
                        //{
                        //    file.Delete();
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

        private void ReportValidate_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Reporting_ListView")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        IObjectSpace os = this.ObjectSpace;
                        bool stat;
                        // DefaultSetting defaultSetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse(""));
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                        Session CS = ((XPObjectSpace)(os)).Session;
                        //IList<DefaultSetting> lstnavset = ObjectSpace.GetObjects<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Reporting' And [SortIndex] Is Not Null And [Select] = True"));
                        //var rptstatus = ReportStatus.Pending1stReview;
                        //bool rptval = false;
                        //bool rptpnd = false;
                        //bool rptdeli = false;
                        //if (lstnavset != null && lstnavset.Count > 0)
                        //{
                        //    foreach(DefaultSetting objdefset in lstnavset.ToList())
                        //    {
                        //        if(objdefset.NavigationItemNameID == "ReportValidation")
                        //        {
                        //            rptval = true;
                        //        }
                        //        else if(objdefset.NavigationItemNameID == "ReportPrintDownload")
                        //        {
                        //            rptpnd = true;
                        //        }
                        //        //else if(objdefset.NavigationItemNameID == "ReportDelivery")
                        //        //{
                        //        //    rptdeli = true;
                        //        //}
                        //    }
                        //    if(rptval == true && rptpnd == true)
                        //    {
                        //        rptstatus = ReportStatus.Pending2ndReview;
                        //    }
                        //    else if (rptval == true && rptpnd == false)
                        //    {
                        //        rptstatus = ReportStatus.Pending2ndReview;
                        //    }
                        //    else if (rptval == false && rptpnd == true)
                        //    {
                        //        rptstatus = ReportStatus.PendingPrint;
                        //    }
                        //    else if(rptval == false && rptpnd == false)
                        //    {
                        //        rptstatus = ReportStatus.PendingDelivery;
                        //    }
                        //}
                        foreach (Modules.BusinessObjects.SampleManagement.Reporting objReport in e.SelectedObjects)
                        {
                            objReport.ReportValidatedDate = DateTime.Now;
                            objReport.ReportValidatedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //if (defaultSetting.ReportApprove == EnumRELevelSetup.No)
                            if (objDefaultInfo.boolReportApprove == true)
                            {
                                objReport.LastUpdatedDate = DateTime.Now;
                                objReport.LastUpdatedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReport.ReportStatus = ReportStatus.Pending2ndReview;
                                AddReportSign(objReport, IsReported: true, IsValidated: true, IsApproved: false);
                            }
                            if (objDefaultInfo.boolReportApprove == false && objDefaultInfo.boolReportPrintDownload == true)
                            {
                                objReport.ReportApprovedDate = DateTime.Now;
                                objReport.ReportApprovedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReport.LastUpdatedDate = DateTime.Now;
                                objReport.LastUpdatedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReport.ReportStatus = ReportStatus.PendingPrint;
                            }
                            else if (objDefaultInfo.boolReportApprove == false && objDefaultInfo.boolReportPrintDownload == false && objDefaultInfo.boolReportdelivery == true)
                            {

                                objReport.ReportApprovedDate = DateTime.Now;
                                objReport.ReportApprovedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReport.DatePrinted = DateTime.Now;
                                objReport.PrintedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReport.ReportStatus = ReportStatus.PendingDelivery;
                                ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportPrintDownloadShow"]);
                                ////if (!stat)
                                ////{
                                ////    objReport.DatePrinted = DateTime.Now;
                                ////    objReport.PrintedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                ////    objReport.ReportStatus = ReportStatus.PendingDelivery;
                                ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                                ////if (!stat)
                                ////{
                                ////    objReport.DateDelivered = DateTime.Now;
                                ////    objReport.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                ////    objReport.ReportStatus = ReportStatus.PendingArchive;
                                ////    stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                                ////    if (!stat)
                                ////    {
                                ////        objReport.DateArchived = DateTime.Now;
                                ////        objReport.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                ////        objReport.ReportStatus = ReportStatus.Archived;
                                ////    }
                                ////}
                                ////}
                                AddReportSign(objReport, IsReported: true, IsValidated: true, IsApproved: true);
                                objReport.ReportStatus = ReportStatus.PendingDelivery;
                            }
                            else if (objDefaultInfo.boolReportApprove == false && objDefaultInfo.boolReportPrintDownload == false && objDefaultInfo.boolReportdelivery == false && objDefaultInfo.boolReportArchive == true)
                            {
                                objReport.ReportApprovedDate = DateTime.Now;
                                objReport.ReportApprovedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReport.DateDelivered = DateTime.Now;
                                objReport.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReport.ReportStatus = ReportStatus.PendingArchive;
                                ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                                ////if (!stat)
                                ////{
                                ////    objReport.DateDelivered = DateTime.Now;
                                ////    objReport.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                ////    objReport.ReportStatus = ReportStatus.PendingArchive;
                                ////    stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                                ////    if (!stat)
                                ////    {
                                ////        objReport.DateArchived = DateTime.Now;
                                ////        objReport.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                ////        objReport.ReportStatus = ReportStatus.Archived;
                                ////    }
                                ////}
                                AddReportSign(objReport, IsReported: true, IsValidated: true, IsApproved: true);
                            }
                            else if (objDefaultInfo.boolReportApprove == false && objDefaultInfo.boolReportPrintDownload == false && objDefaultInfo.boolReportdelivery == false && objDefaultInfo.boolReportArchive == false)
                            {
                                objReport.ReportApprovedDate = DateTime.Now;
                                objReport.ReportApprovedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReport.ReportStatus = ReportStatus.ReportDelivered;
                                ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                                ////if (!stat)
                                ////{
                                ////    objReport.DateArchived = DateTime.Now;
                                ////    objReport.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                ////    objReport.ReportStatus = ReportStatus.Archived;
                                ////}
                                AddReportSign(objReport, IsReported: true, IsValidated: true, IsApproved: true);
                            }
                            objReport.LastUpdatedDate = DateTime.Now;
                            objReport.LastUpdatedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        }
                        os.CommitChanges();
                        foreach (Modules.BusinessObjects.SampleManagement.Reporting objSample1 in View.SelectedObjects)
                        {
                            Samplecheckin sample = View.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?", objSample1.JobID.Oid));
                            IList<Modules.BusinessObjects.SampleManagement.Reporting> reportings = View.ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>().Where(i => i.JobID != null && i.JobID.JobID == sample.JobID).ToList();
                            IList<SampleParameter> lstSamples = View.ObjectSpace.GetObjects<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID == sample.JobID).ToList();
                            foreach (SampleParameter sp in lstSamples)
                            {
                                sp.OSSync = true;
                            }
                            //if (lstSamples.Where(i => i.Status == Samplestatus.PendingValidation && i.Status == Samplestatus.PendingReporting).Count() == 0 && lstSamples.Where(j => j.Status == Samplestatus.PendingApproval).Count() == 0 && lstSamples.Where(i => i.Status == Samplestatus.PendingEntry).Count() == 0 && lstSamples.Where(i => i.Status == Samplestatus.PendingReview).Count() == 0)
                            if (lstSamples.Where(i => i.Status != Samplestatus.Reported).Count() == 0 && reportings.Where(i => i.ReportStatus != ReportStatus.PendingDelivery && i.ReportStatus != ReportStatus.ReportDelivered).Count() == 0)
                            {
                                StatusDefinition objStatus = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 24"));
                                if (objStatus != null)
                                {
                                    sample.Index = objStatus;
                                }
                            }
                        }
                        //ResetNavigationCount();
                        View.ObjectSpace.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "reportvalidate"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        ObjectSpace.Refresh();
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
        private void ReportApproval_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Reporting_ListView_Copy_ReportApproval")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        IObjectSpace os = this.ObjectSpace;
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                        Session CS = ((XPObjectSpace)(os)).Session;
                        bool stat;
                        foreach (Modules.BusinessObjects.SampleManagement.Reporting objReport in e.SelectedObjects)
                        {
                            objReport.ReportApprovedDate = DateTime.Now;
                            objReport.ReportApprovedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objReport.LastUpdatedDate = DateTime.Now;
                            objReport.LastUpdatedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            objReport.ReportStatus = ReportStatus.PendingPrint;
                            //stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportPrintDownloadShow"]);
                            //if (!stat)
                            //if (objDefaultInfo.boolReportPrintDownload == false)
                            //{
                            //    objReport.DatePrinted = DateTime.Now;
                            //    objReport.PrintedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //    objReport.ReportStatus = ReportStatus.PendingDelivery;
                            //    stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                            //    if (!stat)
                            //    {
                            //        objReport.DateDelivered = DateTime.Now;
                            //        objReport.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //        objReport.ReportStatus = ReportStatus.PendingArchive;
                            //        stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                            //        if (!stat)
                            //        {
                            //            objReport.DateArchived = DateTime.Now;
                            //            objReport.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //            objReport.ReportStatus = ReportStatus.Archived;
                            //        }
                            //    }
                            //}
                            //AddReportSign(objReport, IsReported: false, IsValidated: false, IsApproved: true);
                            if (objDefaultInfo.boolReportPrintDownload == true)
                            {
                                objReport.ReportStatus = ReportStatus.PendingPrint;
                            }
                            else if (objDefaultInfo.boolReportPrintDownload == false && objDefaultInfo.boolReportdelivery == true)
                            {
                                objReport.DatePrinted = DateTime.Now;
                                objReport.PrintedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReport.ReportStatus = ReportStatus.PendingDelivery;
                                ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportPrintDownloadShow"]);
                                ////if (!stat)
                                ////{
                                ////    objReport.DatePrinted = DateTime.Now;
                                ////    objReport.PrintedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                ////    objReport.ReportStatus = ReportStatus.PendingDelivery;
                                ////    stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                                ////    if (!stat)
                                ////    {
                                ////        objReport.DateDelivered = DateTime.Now;
                                ////        objReport.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                ////        objReport.ReportStatus = ReportStatus.PendingArchive;
                                ////        stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                                ////        if (!stat)
                                ////        {
                                ////            objReport.DateArchived = DateTime.Now;
                                ////            objReport.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                ////            objReport.ReportStatus = ReportStatus.Archived;
                                ////        }
                                ////    }
                                ////}
                                AddReportSign(objReport, IsReported: true, IsValidated: true, IsApproved: true);
                                ////objReport.ReportStatus = ReportStatus.PendingDelivery;
                            }
                            else if (objDefaultInfo.boolReportPrintDownload == false && objDefaultInfo.boolReportdelivery == false && objDefaultInfo.boolReportArchive == true)
                            {
                                objReport.DateDelivered = DateTime.Now;
                                objReport.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReport.ReportStatus = ReportStatus.PendingArchive;
                                ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                                ////if (!stat)
                                ////{
                                ////    objReport.DateDelivered = DateTime.Now;
                                ////    objReport.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                ////    objReport.ReportStatus = ReportStatus.PendingArchive;
                                ////    stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                                ////    if (!stat)
                                ////    {
                                ////        objReport.DateArchived = DateTime.Now;
                                ////        objReport.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                ////        objReport.ReportStatus = ReportStatus.Archived;
                                ////    }
                                ////}
                                AddReportSign(objReport, IsReported: true, IsValidated: true, IsApproved: true);
                            }
                            else if (objDefaultInfo.boolReportPrintDownload == false && objDefaultInfo.boolReportdelivery == false && objDefaultInfo.boolReportArchive == false)
                            {
                                objReport.DateDelivered = DateTime.Now;
                                objReport.DeliveredBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReport.ReportStatus = ReportStatus.ReportDelivered;
                                //stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                                //if (!stat)
                                //{
                                //    objReport.DateArchived = DateTime.Now;
                                //    objReport.ArchivedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                //    objReport.ReportStatus = ReportStatus.Archived;
                                //}
                                AddReportSign(objReport, IsReported: true, IsValidated: true, IsApproved: true);
                            }
                        }
                        os.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "reportapprove"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        ObjectSpace.Refresh();
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
        #endregion

        private void ReportDelete_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.ObjectTypeInfo.Type == typeof(Modules.BusinessObjects.SampleManagement.Reporting))
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        var objecttodelete = ObjectSpace.GetObjectsToDelete(true);
                        foreach (Modules.BusinessObjects.SampleManagement.Reporting obj in View.SelectedObjects)
                        {

                            if (View.Id == "Reporting_ListView_Copy_ReportView")
                            {
                                objRQPInfo.curReport = obj;
                                IObjectSpace os = Application.CreateObjectSpace();
                                Modules.BusinessObjects.SampleManagement.Reporting objReporting = os.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(objRQPInfo.curReport);
                                if (objReporting != null && !EditCancel)
                                {
                                    if (objReporting.RevisionReason != null)
                                    {
                                        objReporting.RevisionReason = null;
                                    }
                                    DetailView detailView = Application.CreateDetailView(os, "Reporting_DetailView_Revision", true, objReporting);
                                    detailView.ViewEditMode = ViewEditMode.Edit;
                                    ShowViewParameters showViewParameters = new ShowViewParameters(detailView);
                                    showViewParameters.Context = TemplateContext.NestedFrame;
                                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                    DialogController dc = Application.CreateController<DialogController>();
                                    dc.SaveOnAccept = false;
                                    dc.CloseOnCurrentObjectProcessing = false;
                                    dc.Accepting += Dc_Accepting; ;
                                    dc.Cancelling += Dc_Cancelling1; ;
                                    //dc.AcceptAction.Executed += Rollback_Executed;
                                    showViewParameters.Controllers.Add(dc);
                                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                    //objRQPInfo.curReport = null;
                                }
                            }
                             IObjectSpace objSpace = Application.CreateObjectSpace();
                            CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + obj.Oid + "'");
                            Modules.BusinessObjects.SampleManagement.Reporting obj1 = objSpace.FindObject<Modules.BusinessObjects.SampleManagement.Reporting>(criteria);
                            Samplecheckin objSampleJobID = obj1.SampleParameter.Select(i => i.Samplelogin.JobID).Distinct().FirstOrDefault();
                            if (obj1 != null && View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Copy_ReportApproval" /*|| View.Id == "Reporting_ListView_Copy_ReportView"*/)
                            {
                                bool iiStatusChanged = false;
                                if (obj1.ReportStatus == ReportStatus.Rollbacked || obj1.ReportStatus == ReportStatus.Pending1stReview || obj1.ReportStatus == ReportStatus.Pending2ndReview || obj1.ReportStatus == ReportStatus.PendingDelivery
                                    || obj1.ReportStatus == ReportStatus.ReportDelivered)
                                {
                                    IList<SampleParameter> lstSamples = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", objSampleJobID.Oid));
                                    if (lstSamples.Count() == lstSamples.Where(i => i.Status == Samplestatus.Reported).Count())
                                    {
                                        iiStatusChanged = true;
                                    }
                                    obj1.ReportApprovedBy = null;
                                    obj1.ReportValidatedBy = null;
                                    obj1.ReportedBy = null;
                                    DateTime? dt = null;
                                    obj1.ReportApprovedDate = DateTime.MinValue;
                                    obj1.ReportValidatedDate = null;
                                    obj1.ReportedDate = DateTime.MinValue;
                                    objSpace.CommitChanges();
                                    foreach (SampleParameter sampleReport in obj1.SampleParameter)
                                    {
                                        sampleReport.Status = Samplestatus.PendingReporting;
                                        sampleReport.OSSync = true;
                                        //IList<SampleParameter> lstSamples = View.ObjectSpace.GetObjects<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID == samplecheckin.JobID).ToList();
                                        //if (lstSamples.Where(i => i.Status == Samplestatus.PendingValidation && i.Status == Samplestatus.PendingReporting && i.Status == Samplestatus.PendingApproval && i.Status == Samplestatus.PendingEntry).Count() == 0)
                                    }
                                    if (iiStatusChanged)
                                    {
                                        StatusDefinition statusDefinition = objSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 22"));
                                        if (statusDefinition != null)
                                        {
                                            Samplecheckin objSamplecheckin = objSpace.GetObjectByKey<Samplecheckin>(objSampleJobID.Oid);
                                            objSamplecheckin.Index = statusDefinition;
                                        }
                                    }
                                    if (objRQPInfo.curReport != null)
                                    {
                                        Modules.BusinessObjects.SampleManagement.Reporting objReporting1 = objSpace.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(objRQPInfo.curReport);
                                        if (obj.ReportStatus == ReportStatus.Pending1stReview)
                                        {
                                            objAuditInfo.SaveData = true;
                                            Frame.GetController<AuditlogViewController>().insertauditdata(objSpace, obj.JobID.Oid, OperationType.ValueChanged, "Report Tracking", obj.JobID.ToString(), "ReportName", objReporting1.ReportName, obj.ReportName, obj.RevisionReason);
                                            AuditData Objects = objSpace.FindObject<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                                            if (Objects != null)
                                            {
                                                Objects.CommentProcessed = true;
                                            }
                                        }
                                    }
                                    objSpace.Delete(obj1);
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    objSpace.CommitChanges();
                                }
                                else if (obj1.ReportStatus == ReportStatus.Pending2ndReview)
                                {
                                    Application.ShowViewStrategy.ShowMessage("The Report cannot be deleted after validated.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                                else //if(obj1.ReportStatus == ReportStatus.PendingArchive)
                                {
                                    Application.ShowViewStrategy.ShowMessage("The Report cannot be deleted after approved. ", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else if (obj1 != null && View.Id == "Reporting_ListView_Level1Review_View")
                            {
                                bool iiStatusChanged = false;
                                if (obj1.ReportStatus == ReportStatus.PendingPrint || obj1.ReportStatus == ReportStatus.PendingDelivery)
                                {
                                    IList<SampleParameter> lstSamples = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", objSampleJobID.Oid));
                                    if (lstSamples.Count() == lstSamples.Where(i => i.Status == Samplestatus.Reported).Count())
                                    {
                                        iiStatusChanged = true;
                                    }
                                    obj1.ReportApprovedBy = null;
                                    obj1.ReportValidatedBy = null;
                                    obj1.ReportedBy = null;
                                    DateTime? dt = null;
                                    obj1.ReportApprovedDate = DateTime.MinValue;
                                    obj1.ReportValidatedDate = null;
                                    obj1.ReportedDate = DateTime.MinValue;
                                    objSpace.CommitChanges();
                                    foreach (SampleParameter sampleReport in obj1.SampleParameter)
                                    {
                                        sampleReport.Status = Samplestatus.PendingReporting;
                                        sampleReport.OSSync = true;
                                        //Samplecheckin samplecheckin = objSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?", obj1.JobID.Oid));
                                        //IList<SampleParameter> lstSamples = View.ObjectSpace.GetObjects<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID == samplecheckin.JobID).ToList();
                                        //if (lstSamples.Where(i => i.Status != Samplestatus.Reported).Count() == 0)
                                        //{
                                        //    StatusDefinition statusDefinition = objSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Index] = '122'"));
                                        //    if (statusDefinition != null)
                                        //    {
                                        //        samplecheckin.Index = statusDefinition;
                                        //    }
                                        //}
                                    }
                                    if (iiStatusChanged)
                                    {
                                        StatusDefinition statusDefinition = objSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 22"));
                                        if (statusDefinition != null)
                                        {
                                            Samplecheckin objSamplecheckin = objSpace.GetObjectByKey<Samplecheckin>(objSampleJobID.Oid);
                                            objSamplecheckin.Index = statusDefinition;
                                        }
                                    }
                                    objSpace.Delete(obj1);
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    objSpace.CommitChanges();
                                }
                                else if (obj1.ReportStatus == ReportStatus.ReportDelivered)
                                {
                                    Application.ShowViewStrategy.ShowMessage("The Report cannot be deleted after delivered.", InformationType.Error, timer.Seconds, InformationPosition.Top);

                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("The Report cannot be deleted after approved. ", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                        }
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
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
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

        private void Dc_Cancelling1(object sender, EventArgs e)
        {
            if (objAuditInfo.SaveData != true)
            {
                objAuditInfo.comment = string.Empty;
                objAuditInfo.SaveData = null;
                objAuditInfo.action = null;
                objAuditInfo.choiceaction = null;
                objAuditInfo.choiceactionitem = null;
                Application.ShowViewStrategy.ShowMessage("Unsaved changes exist", InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    {
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        Modules.BusinessObjects.SampleManagement.Reporting objReporting = objSpace.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(objRQPInfo.curReport);
                        Modules.BusinessObjects.SampleManagement.Reporting objReport = e.AcceptActionArgs.CurrentObject as Modules.BusinessObjects.SampleManagement.Reporting;
                        if (objReport != null && objReport.RevisionReason != null)
                        {
                            foreach (Modules.BusinessObjects.SampleManagement.Reporting obj in View.SelectedObjects)
                            {
                                objRQPInfo.curReport = obj;
                                //IObjectSpace objSpace = Application.CreateObjectSpace();
                                CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + obj.Oid + "'");
                                Modules.BusinessObjects.SampleManagement.Reporting obj1 = objSpace.FindObject<Modules.BusinessObjects.SampleManagement.Reporting>(criteria);
                                Samplecheckin objSampleJobID = obj1.SampleParameter.Select(i => i.Samplelogin.JobID).Distinct().FirstOrDefault();
                                if (View.Id == "Reporting_ListView_Copy_ReportView" && objSampleJobID != null)
                                {
                                    bool iiStatusChanged = false;
                                    if (obj1.ReportStatus == ReportStatus.Rollbacked || obj1.ReportStatus == ReportStatus.Pending1stReview || obj1.ReportStatus == ReportStatus.Pending2ndReview || obj1.ReportStatus == ReportStatus.PendingDelivery
                                        || obj1.ReportStatus == ReportStatus.ReportDelivered)
                                    {
                                        IList<SampleParameter> lstSamples = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", objSampleJobID.Oid));
                                        if (lstSamples.Count() == lstSamples.Where(i => i.Status == Samplestatus.Reported).Count())
                                        {
                                            iiStatusChanged = true;
                                        }
                                        obj1.ReportApprovedBy = null;
                                        obj1.ReportValidatedBy = null;
                                        obj1.ReportedBy = null;
                                        DateTime? dt = null;
                                        obj1.ReportApprovedDate = DateTime.MinValue;
                                        obj1.ReportValidatedDate = null;
                                        obj1.ReportedDate = DateTime.MinValue;
                                        objSpace.CommitChanges();
                                        foreach (SampleParameter sampleReport in obj1.SampleParameter)
                                        {
                                            sampleReport.Status = Samplestatus.PendingReporting;
                                            sampleReport.OSSync = true;
                                            //IList<SampleParameter> lstSamples = View.ObjectSpace.GetObjects<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID == samplecheckin.JobID).ToList();
                                            //if (lstSamples.Where(i => i.Status == Samplestatus.PendingValidation && i.Status == Samplestatus.PendingReporting && i.Status == Samplestatus.PendingApproval && i.Status == Samplestatus.PendingEntry).Count() == 0)
                                        }
                                        if (iiStatusChanged)
                                        {
                                            StatusDefinition statusDefinition = objSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 22"));
                                            if (statusDefinition != null)
                                            {
                                                Samplecheckin objSamplecheckin = objSpace.GetObjectByKey<Samplecheckin>(objSampleJobID.Oid);
                                                objSamplecheckin.Index = statusDefinition;
                                            }
                                        }
                                        if (objRQPInfo.curReport != null)
                                        {
                                            Modules.BusinessObjects.SampleManagement.Reporting objReporting1 = objSpace.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(objRQPInfo.curReport);
                                            //if (obj.ReportStatus == ReportStatus.ReportDelivered)
                                            //{
                                            objAuditInfo.SaveData = true;
                                            Frame.GetController<AuditlogViewController>().insertauditdata(objSpace, obj.JobID.Oid, OperationType.Deleted, "Report Tracking", obj.JobID.ToString(), "ReportName", objReporting1.ReportName, " ", objReport.RevisionReason);
                                            AuditData Objects = objSpace.FindObject<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                                            if (Objects != null)
                                            {
                                                Objects.CommentProcessed = true;
                                            }
                                            //}
                                        }
                                        objSpace.Delete(obj1);                                        
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                        objSpace.CommitChanges();
                                        ObjectSpace.Refresh();
                                        //Application.MainWindow.View.Refresh();
                                        //objSpace.Refresh();
                                    }
                                    else if (obj1.ReportStatus == ReportStatus.Pending2ndReview)
                                    {
                                        Application.ShowViewStrategy.ShowMessage("The Report cannot be deleted after validated.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    }
                                    else //if(obj1.ReportStatus == ReportStatus.PendingArchive)
                                    {
                                        Application.ShowViewStrategy.ShowMessage("The Report cannot be deleted after approved. ", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    }
                                }
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "RevisionReason"), InformationType.Error, timer.Seconds, InformationPosition.Top);

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

        private void Reportview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Modules.BusinessObjects.SampleManagement.Reporting));
                // cs.Criteria["Filter"] = CriteriaOperator.Parse("[Status] <> 'PendingDistribute'");
                Frame.SetView(Application.CreateListView("Reporting_ListView_Copy_ReportView", cs, false));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void EditReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Reporting_ListView_Copy_ReportView")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        if (View.SelectedObjects.Count == 1)
                        {
                            foreach (Modules.BusinessObjects.SampleManagement.Reporting spReport in View.SelectedObjects)
                            {
                                objRQPInfo.curReport = spReport;
                                ObjReportingInfo.strReportID = spReport.ReportID;
                                IObjectSpace objspace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                                IList<SampleLogIn> objsampleLoglist = objspace.GetObjects<SampleLogIn>(CriteriaOperator.Parse("[JobID.JobID] = ?", spReport.JobID.JobID));
                                cs.Criteria["Filter"] = new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin", objsampleLoglist),
                                    CriteriaOperator.Parse("[Status] = 'PendingReporting'Or [Status] = 'Reported'"));
                                ListView CreatedView = Application.CreateListView("SampleParameter_ListView_Copy_CustomReporting_Edit", cs, true);
                                Frame.SetView(CreatedView);
                                //    e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateListView("SampleParameter_ListView_Copy_CustomReporting", cs, false);
                                //foreach (SampleParameter spParam in spReport.SampleParameter)
                                //{
                                //    cs.Criteria["Filter"] = CriteriaOperator.Parse("[Samplelogin] = ?", spParam.Samplelogin.Oid);
                                //    ListView CreatedView = Application.CreateListView("SampleParameter_ListView_Copy_CustomReporting_Edit", cs, true);
                                //    Frame.SetView(CreatedView);
                                //    break;
                                //}
                                //CriteriaOperator criteriaUserPermission = CriteriaOperator.Parse("[Oid]='" + spP + "'");
                                //SampleParameter lstSP = ObjectSpace.FindObject<SampleParameter>(criteriaUserPermission);                              
                            }

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
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Comment_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(ReportComment));
                if (objToShow != null)
                {
                    DetailView CreateDetailView = Application.CreateDetailView(objspace, objToShow);
                    CreateDetailView.ViewEditMode = ViewEditMode.Edit;
                    e.View = CreateDetailView;
                    e.DialogController.Accepting += DialogController_Accepting;

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                ReportComment objreport = (ReportComment)e.AcceptActionArgs.CurrentObject;
                if (objreport != null)
                {
                    if (objreport.Comment != null)
                    {
                        reportcomment.Comment = objreport.Comment;                        
                        IObjectSpace os = Application.CreateObjectSpace(typeof(Notes));
                        Notes note = os.CreateObject<Notes>();
                        if (note != null)
                        {
                            note.Title = objreport.Title;
                            note.SourceID = CNInfo.RpJobId;
                            note.NameSource = CNInfo.RpSampleMatries;
                            note.Text = objreport.Comment;
                            note.Samplecheckin = os.GetObjectByKey<Samplecheckin>(CNInfo.SCoidValue);
                        }
                        os.CommitChanges();
                    }
                    else
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Please enter the comment reason.", InformationType.Info, timer.Seconds, InformationPosition.Top);

                    }
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void Comment_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        //{

        //}

        private void DocumentPreview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ReportCommentVM objinfo = new ReportCommentVM();
                string value = objinfo.Comment;

                Boolean UnApprove = false;
                strReportIDT = null;
                if (cmbReportName.SelectedItem != null && cmbReportName.SelectedIndex != 0)
                {
                    if (View.Id == "SampleParameter_ListView_Copy_CustomReporting" || View.Id == "SampleParameter_ListView_Copy_CustomReporting_Edit")
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            bool IsAllowMultipleJobID = true;
                            List<string> lstreport = new List<string>();
                            IList<ReportPackage> objrep = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?", cmbReportName.SelectedItem.ToString()));
                            foreach (ReportPackage objrp in objrep.ToList())
                            {
                                lstreport.Add(objrp.ReportName);
                            }

                            List<SampleParameter> lstsamparameter = View.SelectedObjects.Cast<SampleParameter>().ToList();
                            var jobid = lstsamparameter.Select(i => i.Samplelogin.JobID).Distinct().ToList();
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

                            if (/*jobid.Count() == 1*/IsAllowMultipleJobID == true)
                            {
                                uqOid = null;
                                foreach (SampleParameter obj in View.SelectedObjects)
                                {
                                    if (uqOid == null)
                                    {
                                        uqOid = "'" + obj.Oid.ToString() + "'";
                                        JobID = "'" + obj.Samplelogin.JobID + "'";

                                    }
                                    else
                                    {
                                        uqOid = uqOid + ",'" + obj.Oid.ToString() + "'";
                                        JobID = JobID + ",'" + obj.Samplelogin.JobID + "'";
                                    }
                                    //if (obj.Status == "Pending Entry" || obj.Status == "Pending Validation" || obj.Status == "Pending Approval")
                                    if (obj.Status == Samplestatus.PendingEntry || obj.Status == Samplestatus.PendingValidation || obj.Status == Samplestatus.PendingApproval)
                                    {
                                        UnApprove = true;
                                    }
                                }

                                string strTempPath = Path.GetTempPath();
                                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                                {
                                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                                }
                                string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".docx");
                                XtraReport xtraReport = new XtraReport();
                                ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                                ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                SetConnectionString();
                                DynamicReportBusinessLayer.BLCommon.SetDBConnection(ObjReportDesignerInfo.LDMSQLServerName, ObjReportDesignerInfo.LDMSQLDatabaseName, ObjReportDesignerInfo.LDMSQLUserID, ObjReportDesignerInfo.LDMSQLPassword);
                                //DynamicDesigner.GlobalReportSourceCode.struqSampleParameterID = uqOid;
                                IObjectSpace objSpace = Application.CreateObjectSpace();
                                Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                                if (boolReportSave == true)
                                {
                                    strReportIDT = null;
                                    ReportIDFormat(currentSession);
                                }
                                ObjReportingInfo.struqSampleParameterID = uqOid;
                                ObjReportingInfo.strJobID = JobID;
                                ObjReportingInfo.strLimsReportedDate = DateTime.Now.ToString("MM/dd/yyyy");
                                ObjReportingInfo.strReportID = strReportIDT;
                                //IList<ReportPackage> objrep = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?", cmbReportName.SelectedItem.ToString()));
                                List<string> listPage = new List<string>();
                                int pagenumber = 0;
                                SelectedData sproclang = currentSession.ExecuteSproc("getCurrentLanguage", "");
                                var CurrentLanguage = sproclang.ResultSet[1].Rows[0].Values[0].ToString();
                                if (objrep != null && objrep.Count > 0)
                                {
                                    var sortobj = objrep.OrderBy(x => x.sort);
                                    foreach (ReportPackage report in sortobj)
                                    {
                                        if (report.ReportName != null)
                                        {
                                            XtraReport tempxtraReport = new XtraReport();
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
                                    if (UnApprove == true)
                                    {
                                        xtraReport.Watermark.Text = "NOT APPROVED";
                                        xtraReport.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                                        xtraReport.Watermark.TextTransparency = 200;
                                        xtraReport.Watermark.ShowBehind = true;
                                    }
                                    xtraReport.ExportToDocx(strExportedPath);
                                }
                                else
                                {
                                    string strReport = cmbReportName.SelectedItem.ToString();
                                    xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(strReport, ObjReportingInfo, false);
                                    DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport, ObjReportingInfo);
                                    if (UnApprove == true)
                                    {
                                        xtraReport.Watermark.Text = "NOT APPROVED";
                                        xtraReport.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                                        xtraReport.Watermark.TextTransparency = 200;
                                        xtraReport.Watermark.ShowBehind = true;
                                    }
                                    xtraReport.ExportToDocx(strExportedPath);
                                }
                                List<SampleParameter> listSP = View.SelectedObjects.Cast<SampleParameter>().ToList();
                                IList<ReportPackage> objreps = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?", cmbReportName.SelectedItem.ToString()));
                                foreach (ReportPackage package in objreps)
                                {
                                    if (package.NPUserDefinedReportName == "COC")
                                    {
                                        foreach (Guid oid in listSP.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null).Select(i => i.Samplelogin.JobID.Oid).Distinct().ToList())
                                        {
                                            IList<Modules.BusinessObjects.SampleManagement.Attachment> objrepCOCs = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Attachment>(CriteriaOperator.Parse("[Samplecheckin.oid]=? and [Category] = 'COC'", oid));
                                            foreach (Modules.BusinessObjects.SampleManagement.Attachment attachment in objrepCOCs.Where(i => i.Attachments != null && !string.IsNullOrEmpty(i.Attachments.FileName)))
                                            {
                                                using (MemoryStream ms = new MemoryStream())
                                                {
                                                    attachment.Attachments.SaveToStream(ms);
                                                    XtraReport tempXtraReport = new XtraReport();
                                                    DetailBand detailBand = new DetailBand();
                                                    detailBand.HeightF = 1000;
                                                    tempXtraReport.Bands.Add(detailBand);
                                                    XRPdfContent xrPdfContent = new XRPdfContent();
                                                    xrPdfContent.Source = ms.ToArray();
                                                    tempXtraReport.Bands[BandKind.Detail].Controls.Add(xrPdfContent);
                                                    //tempXtraReport.LoadLayout(ms);
                                                    xtraReport.Pages.AddRange(tempXtraReport.Pages);
                                                    xtraReport.ExportToPdf(ms);
                                                    using (PdfDocumentProcessor source = new PdfDocumentProcessor())
                                                    {
                                                        source.LoadDocument(ms);
                                                        source.SaveDocument(ms);
                                                    }

                                                }

                                            }
                                        }

                                    }

                                }
                                string[] path = strExportedPath.Split('\\');
                                int arrcount = path.Count();
                                int sc = arrcount - 2;
                                string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                                WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));

                                if (boolReportSave == true)
                                {
                                    //Rebex.Net.Ftp _FTP = GetFTPConnection();
                                    //string strRemotePath = ConfigurationManager.AppSettings["FinalReportPath"];
                                    //if (!_FTP.DirectoryExists(strRemotePath))
                                    //{
                                    //    _FTP.CreateDirectory(strRemotePath);
                                    //}
                                    //string strFilePath = strRemotePath.Replace(@"\", "//") + strReportIDT + ".docx";
                                    //_FTP.PutFile(strExportedPath, strFilePath);
                                    boolReportSave = false;
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotallowMultipleJOBID"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectreport"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    return;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ExcelPreview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ReportCommentVM objinfo = new ReportCommentVM();
                string value = objinfo.Comment;

                Boolean UnApprove = false;
                strReportIDT = null;
                if (cmbReportName.SelectedItem != null && cmbReportName.SelectedIndex != 0)
                {
                    if (View.Id == "SampleParameter_ListView_Copy_CustomReporting" || View.Id == "SampleParameter_ListView_Copy_CustomReporting_Edit")
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            List<SampleParameter> lstsamparameter = e.SelectedObjects.Cast<SampleParameter>().ToList();
                            var jobid = lstsamparameter.Select(i => i.Samplelogin.JobID).Distinct().ToList();
                            if (jobid.Count() == 1)
                            {
                                uqOid = null;
                                foreach (SampleParameter obj in View.SelectedObjects)
                                {
                                    if (uqOid == null)
                                    {
                                        uqOid = "'" + obj.Oid.ToString() + "'";
                                        JobID = "'" + obj.Samplelogin.JobID + "'";

                                    }
                                    else
                                    {
                                        uqOid = uqOid + ",'" + obj.Oid.ToString() + "'";
                                        JobID = JobID + ",'" + obj.Samplelogin.JobID + "'";
                                    }
                                    //if (obj.Status == "Pending Entry" || obj.Status == "Pending Validation" || obj.Status == "Pending Approval")
                                    if (obj.Status == Samplestatus.PendingEntry || obj.Status == Samplestatus.PendingValidation || obj.Status == Samplestatus.PendingApproval)
                                    {
                                        UnApprove = true;
                                    }
                                }

                                string strTempPath = Path.GetTempPath();
                                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                                {
                                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                                }
                                string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".xlsx");
                                XtraReport xtraReport = new XtraReport();
                                ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                                ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                SetConnectionString();
                                DynamicReportBusinessLayer.BLCommon.SetDBConnection(ObjReportDesignerInfo.LDMSQLServerName, ObjReportDesignerInfo.LDMSQLDatabaseName, ObjReportDesignerInfo.LDMSQLUserID, ObjReportDesignerInfo.LDMSQLPassword);
                                //DynamicDesigner.GlobalReportSourceCode.struqSampleParameterID = uqOid;
                                IObjectSpace objSpace = Application.CreateObjectSpace();
                                Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                                if (boolReportSave == true && string.IsNullOrEmpty(strReportIDT))
                                {
                                    strReportIDT = null;
                                    ReportIDFormat(currentSession);
                                }
                                ObjReportingInfo.struqSampleParameterID = uqOid;
                                ObjReportingInfo.strJobID = JobID;
                                ObjReportingInfo.strLimsReportedDate = DateTime.Now.ToString("MM/dd/yyyy");
                                ObjReportingInfo.strReportID = strReportIDT;
                                IList<ReportPackage> objrep = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?", cmbReportName.SelectedItem.ToString()));
                                List<string> listPage = new List<string>();
                                int pagenumber = 0;
                                SelectedData sproclang = currentSession.ExecuteSproc("getCurrentLanguage", "");
                                var CurrentLanguage = sproclang.ResultSet[1].Rows[0].Values[0].ToString();
                                if (objrep != null && objrep.Count > 0)
                                {
                                    var sortobj = objrep.OrderBy(x => x.sort);
                                    foreach (ReportPackage report in sortobj)
                                    {
                                        XtraReport tempxtraReport = new XtraReport();
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
                                    if (UnApprove == true)
                                    {
                                        xtraReport.Watermark.Text = "NOT APPROVED";
                                        xtraReport.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                                        xtraReport.Watermark.TextTransparency = 200;
                                        xtraReport.Watermark.ShowBehind = true;
                                    }
                                    xtraReport.ExportToXlsx(strExportedPath);
                                }
                                else
                                {
                                    string strReport = cmbReportName.SelectedItem.ToString();
                                    xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(strReport, ObjReportingInfo, false);
                                    DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport, ObjReportingInfo);
                                    if (UnApprove == true)
                                    {
                                        xtraReport.Watermark.Text = "NOT APPROVED";
                                        xtraReport.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                                        xtraReport.Watermark.TextTransparency = 200;
                                        xtraReport.Watermark.ShowBehind = true;
                                    }
                                    xtraReport.ExportToXlsx(strExportedPath);
                                }

                                string[] path = strExportedPath.Split('\\');
                                int arrcount = path.Count();
                                int sc = arrcount - 2;
                                string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                                WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));//CONSCI

                                if (boolReportSave == true)
                                {
                                    //Rebex.Net.Ftp _FTP = GetFTPConnection();
                                    //string strRemotePath = ConfigurationManager.AppSettings["FinalReportPath"];
                                    //if (!_FTP.DirectoryExists(strRemotePath))
                                    //{
                                    //    _FTP.CreateDirectory(strRemotePath);
                                    //}
                                    //string strFilePath = strRemotePath.Replace(@"\", "//") + strReportIDT + ".xlsx";
                                    //_FTP.PutFile(strExportedPath, strFilePath);
                                    boolReportSave = false;
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectjobid"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectreport"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    return;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ReportRollback_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Reporting_ListView_Copy_ReportView")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        List<Modules.BusinessObjects.SampleManagement.Reporting> lstSelectedReports = View.SelectedObjects.Cast<Modules.BusinessObjects.SampleManagement.Reporting>().ToList();
                        Modules.BusinessObjects.SampleManagement.Reporting objReporting = os.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(lstSelectedReports.FirstOrDefault(i => i.ReportStatus == ReportStatus.Pending1stReview || i.ReportStatus == ReportStatus.Pending2ndReview));
                        if (objReporting != null)
                        {
                            objReporting.DateRollback = DateTime.Now;
                            objReporting.RollbackedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            DetailView detailView = Application.CreateDetailView(os, "Reporting_DetailView_Rollback", true, objReporting);
                            detailView.ViewEditMode = ViewEditMode.Edit;
                            ShowViewParameters showViewParameters = new ShowViewParameters(detailView);
                            showViewParameters.Context = TemplateContext.NestedFrame;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += Rollback_Accepting;
                            dc.AcceptAction.Executed += Rollback_Executed;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                        else
                        {
                            objReporting = os.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(lstSelectedReports.FirstOrDefault(i => i.ReportStatus != ReportStatus.Pending1stReview && i.ReportStatus != ReportStatus.Pending2ndReview && i.ReportStatus != ReportStatus.Rollbacked));
                            if (objReporting != null)
                            {
                                Application.ShowViewStrategy.ShowMessage("Approved reports cannot be rollbacked", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                //Application.ShowViewStrategy.ShowMessage("Reports cannot be rollbacked after being approved", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            else
                            {
                                objReporting = os.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(lstSelectedReports.FirstOrDefault(i => i.ReportStatus == ReportStatus.Rollbacked));
                                if (objReporting != null)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Report has already been rollbacked", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    //return;
                                }
                            }

                        }
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

        private void Rollback_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Rollback_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.CurrentObject != null)
                {
                    Modules.BusinessObjects.SampleManagement.Reporting objRollbackDetails = (Modules.BusinessObjects.SampleManagement.Reporting)e.AcceptActionArgs.CurrentObject;
                    if (objRollbackDetails != null)
                    {
                        if (!string.IsNullOrEmpty(objRollbackDetails.RollbackReason))
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            bool CanCommit = false;
                            ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                            //ReadXmlFile_FTPConc();
                            //Rebex.Net.Ftp _FTP = GetFTPConnection();
                            List<Modules.BusinessObjects.SampleManagement.Reporting> lstRollbackedReports = new List<Modules.BusinessObjects.SampleManagement.Reporting>();
                            foreach (Modules.BusinessObjects.SampleManagement.Reporting objReporting in View.SelectedObjects)
                            {
                                if (objReporting.ReportStatus == ReportStatus.Pending1stReview || objReporting.ReportStatus == ReportStatus.Pending2ndReview)
                                {
                                    CanCommit = true;
                                    Modules.BusinessObjects.SampleManagement.Reporting rollbackReport = os.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(objReporting);
                                    rollbackReport.DateRollback = objRollbackDetails.DateRollback;
                                    rollbackReport.RollbackedBy = os.GetObject<Employee>(objRollbackDetails.RollbackedBy);
                                    rollbackReport.RollbackReason = objRollbackDetails.RollbackReason;
                                    rollbackReport.ReportStatus = ReportStatus.Rollbacked;
                                    lstRollbackedReports.Add(rollbackReport);

                                    ReportRollbackLog rollbackLog = os.CreateObject<ReportRollbackLog>();
                                    rollbackLog.ReportID = objReporting.ReportID;
                                    rollbackLog.ReportName = objReporting.ReportName;
                                    //rollbackLog.Report = GetReportAsByteArray(objReporting.ReportID, _FTP);
                                    rollbackLog.ReportedDate = objReporting.ReportedDate;
                                    rollbackLog.ReportedBy = os.GetObject<Employee>(objReporting.ReportedBy);
                                    rollbackLog.DateRollback = objRollbackDetails.DateRollback;
                                    rollbackLog.RollbackedBy = os.GetObject<Employee>(objRollbackDetails.RollbackedBy);
                                    rollbackLog.RollbackReason = objRollbackDetails.RollbackReason;
                                    //rollbackLog.JobID.ProjectID = objReporting.JobID.ProjectID;
                                    rollbackLog.JobID = rollbackReport.JobID;
                                }
                                else if (objReporting.ReportStatus == ReportStatus.Rollbacked)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Report has already been rollbacked", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    //return;
                                }
                                else //if(objReporting.ReportStatus == ReportStatus.PendingArchive)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Approved reports cannot be rollbacked", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    //Application.ShowViewStrategy.ShowMessage("Reports cannot be rollbacked after being approved", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    //return;
                                }
                            }
                            if (CanCommit)
                            {
                                os.CommitChanges();
                                CanCommit = false;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                //os.Dispose();
                            }

                            if (lstRollbackedReports != null && lstRollbackedReports.Count > 0)
                            {
                                foreach (Modules.BusinessObjects.SampleManagement.Reporting rollbackedreport in lstRollbackedReports)
                                {
                                    if (rollbackedreport.RollbackedBy != null && !string.IsNullOrEmpty(rollbackedreport.RollbackedBy.Email))
                                    {
                                        SendNotificationEmail(rollbackedreport);
                                    }
                                    ReportRollbackLog rollbackLog = os.FindObject<ReportRollbackLog>(CriteriaOperator.Parse("[ReportID] = ?", rollbackedreport.ReportID));
                                    if (rollbackLog != null)
                                    {
                                        CanCommit = true;
                                        rollbackLog.IsNotificationMailSent = true;
                                    }
                                }

                                if (CanCommit)
                                {
                                    os.CommitChanges();
                                    //os.Dispose();
                                }
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage("Please enter reason to rollback selected reports.", InformationType.Error, timer.Seconds, InformationPosition.Top);
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

        //private byte[] GetReportAsByteArray(string reportID)
        //{
        //    try
        //        string strRemotePath = ConfigurationManager.AppSettings["FinalReportPath"];
        //        string strExportedPath = strRemotePath.Replace(@"\", "//") + reportID + ".pdf";
        //        //if (_FTP.FileExists(strExportedPath))
        //        {
        //            MemoryStream ms = new MemoryStream();
        //            //_FTP.TransferType = FtpTransferType.Binary;
        //            //_FTP.GetFile(strExportedPath, ms);
        //            //_FTP.GetFile(strExportedPath, ms);
        //    return ms.ToArray(); 
        //            //string WatermarkText = ConfigurationManager.AppSettings["ReportWaterMarkText"];

        //            //if (string.IsNullOrEmpty(WatermarkText))
        //            //{
        //            //    return ms.ToArray(); 
        //            //}
        //            //else
        //            //{
        //            //    using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
        //            //    {
        //            //        string fontName = "Microsoft Yahei";
        //            //        int fontSize = 25;
        //            //        PdfStringFormat stringFormat = PdfStringFormat.GenericTypographic;
        //            //        stringFormat.Alignment = PdfStringAlignment.Center;
        //            //        stringFormat.LineAlignment = PdfStringAlignment.Center;
        //            //        MemoryStream tempms = new MemoryStream();
        //            //        documentProcessor.LoadDocument(ms);
        //            //        using (SolidBrush brush = new SolidBrush(Color.FromArgb(63, Color.Black)))
        //            //        {
        //            //            using (Font font = new Font(fontName, fontSize))
        //            //            {
        //            //                foreach (var page in documentProcessor.Document.Pages)
        //            //                {
        //            //                    var watermarkSize = page.CropBox.Width * 0.75;
        //            //                    using (DevExpress.Pdf.PdfGraphics graphics = documentProcessor.CreateGraphics())
        //            //                    {
        //            //                        SizeF stringSize = graphics.MeasureString(WatermarkText, font);
        //            //                        Single scale = Convert.ToSingle(watermarkSize / stringSize.Width);
        //            //                        graphics.TranslateTransform(Convert.ToSingle(page.CropBox.Width * 0.5), Convert.ToSingle(page.CropBox.Height * 0.5));
        //            //                        graphics.RotateTransform(-45);
        //            //                        graphics.TranslateTransform(Convert.ToSingle(-stringSize.Width * scale * 0.5), Convert.ToSingle(-stringSize.Height * scale * 0.5));
        //            //                        using (Font actualFont = new Font(fontName, fontSize * scale))
        //            //                        {
        //            //                            RectangleF rect = new RectangleF(0, 0, stringSize.Width * scale, stringSize.Height * scale);
        //            //                            graphics.DrawString(WatermarkText, actualFont, brush, rect, stringFormat);
        //            //                        }
        //            //                        graphics.AddToPageForeground(page, 72, 72);
        //            //                    }
        //            //                }
        //            //            }
        //            //        }
        //            //        documentProcessor.SaveDocument(tempms);
        //            //        return tempms.ToArray();
        //            //    }
        //            //}                    
        //        //}
        //            return null;
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //        return null;
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //        return null;
        //            }
        //        }

        private void SendNotificationEmail(Modules.BusinessObjects.SampleManagement.Reporting rollbackedreport)
        {
            try
            {
                SmtpClient sc = new SmtpClient();
                string strlogUsername = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).UserName;
                string strlogpassword = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).Password;
                string strSmtpHost = "Smtp.gmail.com";
                string strMailFromUserName = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromUserName"];
                string strMailFromPassword = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromPassword"];
                //string strWebSiteAddress = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["WebSiteAddress"];
                string strMailto = string.Empty;

                string strReportIDT = rollbackedreport.ReportID;
                string strProjectID = string.Empty;
                string strJobID = string.Empty;
                string strRollbackReason = rollbackedreport.RollbackReason;
                string strRollbackBy = rollbackedreport.RollbackedBy.DisplayName;
                string strDateRollback = rollbackedreport.DateRollback.ToString();

                if (rollbackedreport.JobID != null)
                {
                    strJobID = rollbackedreport.JobID.JobID;

                    if (rollbackedreport.JobID.ProjectID != null)
                    {
                        strProjectID = rollbackedreport.JobID.ProjectID.ProjectId;
                    }
                }

                MailMessage m = new MailMessage();
                m.IsBodyHtml = true;
                m.From = new MailAddress(strMailFromUserName);
                m.To.Add(rollbackedreport.ReportedBy.Email);

                m.Subject = "ReportID : " + strReportIDT + " - Report Rollback Notification";
                m.Body = @"ReportID : " + strReportIDT + " has been rollbacked.<br><br>"
                                + "<table border=1 cellpadding=4 cellspacing=0>" +
                                "<tr><td>ReportID:</td>" +
                                "<td>" + strReportIDT + "</td></tr>" +
                                "<tr><td>Project ID:</td>" +
                                "<td>" + strProjectID + "</td></tr>" +
                                "<tr><td>Job ID:</td>" +
                                "<td>" + strJobID + "</td></tr>" +
                                "<tr><td>Rollback Reason:</td>" +
                                "<td>" + strRollbackReason + "</td></tr>" +
                                "<tr><td>Rollback By:</td>" +
                                "<td>" + strRollbackBy + "</td></tr>" +
                                "<tr><td>Date Rollback:</td>" +
                                "<td>" + strDateRollback + "</td></tr>" +
                                "</table>";

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
                    if (m.To != null && m.To.Count > 0)
                    {
                        sc.Send(m);
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
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>()
                    .InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PreviewRollbackReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "ReportRollbackLog_ListView")
                {
                    if (e.SelectedObjects.Count == 1)
                    {
                        ReportRollbackLog objreportrollback = (ReportRollbackLog)e.CurrentObject;
                        NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                        PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                        objToShow.ReportID = objreportrollback.ReportID;
                        //objToShow.PDFData = objreportrollback.Report;
                        objToShow.PDFData = AddWaterMarkToReport(objreportrollback.Report);
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
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectReportID"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private byte[] AddWaterMarkToReport(byte[] report)
        {
            try
            {
                string WatermarkText;
                Session currentSession = ((DevExpress.ExpressApp.Xpo.XPObjectSpace)(this.ObjectSpace)).Session;
                // SelectedData sproclang = currentSession.ExecuteSproc("getCurrentLanguage", "");
                //var CurrentLanguage = sproclang.ResultSet[1].Rows[0].Values[0].ToString();
                if (objLanguage.strcurlanguage == "En")
                {
                    WatermarkText = "UnApproved";
                }
                else
                {
                    WatermarkText = ConfigurationManager.AppSettings["ReportWaterMarkText"];
                }

                if (string.IsNullOrEmpty(WatermarkText))
                {
                    return report;
                }
                else
                {
                    using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
                    {
                        string fontName = "Microsoft Yahei";
                        int fontSize = 25;
                        PdfStringFormat stringFormat = PdfStringFormat.GenericTypographic;
                        stringFormat.Alignment = PdfStringAlignment.Center;
                        stringFormat.LineAlignment = PdfStringAlignment.Center;
                        MemoryStream ms = new MemoryStream(report);
                        MemoryStream tempms = new MemoryStream();
                        documentProcessor.LoadDocument(ms);
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
                        return tempms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        private void Level2ReviewViewDateFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Reporting_ListView_Copy_ReportView")
                {
                    DateTime srDateFilter = DateTime.MinValue;
                    if (Level2ReviewViewDateFilter != null && Level2ReviewViewDateFilter.SelectedItem != null)
                    {
                        if (Level2ReviewViewDateFilter.SelectedItem.Id == "1M")
                        {
                            srDateFilter = DateTime.Today.AddMonths(-1);
                        }
                        if (Level2ReviewViewDateFilter.SelectedItem.Id == "3M")
                        {
                            srDateFilter = DateTime.Today.AddMonths(-3);
                        }
                        else if (Level2ReviewViewDateFilter.SelectedItem.Id == "6M")
                        {
                            srDateFilter = DateTime.Today.AddMonths(-6);
                        }
                        else if (Level2ReviewViewDateFilter.SelectedItem.Id == "1Y")
                        {
                            srDateFilter = DateTime.Today.AddYears(-1);
                        }
                        else if (Level2ReviewViewDateFilter.SelectedItem.Id == "2Y")
                        {
                            srDateFilter = DateTime.Today.AddYears(-2);
                        }
                        else if (Level2ReviewViewDateFilter.SelectedItem.Id == "5Y")
                        {
                            srDateFilter = DateTime.Today.AddYears(-5);
                        }
                    }
                    if (srDateFilter != DateTime.MinValue)
                    {
                        //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[ReportedDate] BETWEEN('" + srDateFilter + "', '" + DateTime.Now + "')");
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[ReportedDate] >= ? And [ReportedDate] < ?", srDateFilter, DateTime.Now);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void ResetNavigationCount()
        {
            try
            {
                ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                ChoiceActionItem parent = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "Reporting");
                if (parent != null)
                {
                    ChoiceActionItem childlevel2ReportReview = parent.Items.FirstOrDefault(i => i.Id == "ReportValidation");
                    if (childlevel2ReportReview != null)
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        var count = objectSpace.GetObjectsCount(typeof(Modules.BusinessObjects.SampleManagement.Reporting), CriteriaOperator.Parse("[ReportStatus] <> ##Enum#Modules.BusinessObjects.Hr.ReportStatus,Rollbacked# AND [ReportValidatedDate] IS NULL AND [ReportValidatedBy] IS NULL"));
                        var cap = childlevel2ReportReview.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        if (count > 0)
                        {
                            childlevel2ReportReview.Caption = cap[0] + " (" + count + ")";
                        }
                        else
                        {
                            childlevel2ReportReview.Caption = cap[0];
                        }
                    }
                    ChoiceActionItem childReportDelivery = parent.Items.FirstOrDefault(i => i.Id == "ReportDelivery");
                    if (childReportDelivery != null)
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        var count = objectSpace.GetObjectsCount(typeof(Modules.BusinessObjects.SampleManagement.Reporting), CriteriaOperator.Parse("[ReportStatus] = ##Enum#Modules.BusinessObjects.Hr.ReportStatus,PendingDelivery#"));
                        //var count = objectSpace.GetObjectsCount(typeof(Modules.BusinessObjects.SampleManagement.Reporting), CriteriaOperator.Parse("[ReportStatus]='PendingDelivery' Or [ReportStatus]='ReportDelivered'"));
                        var cap = childReportDelivery.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        if (count > 0)
                        {
                            childReportDelivery.Caption = cap[0] + " (" + count + ")";
                        }
                        else
                        {
                            childReportDelivery.Caption = cap[0];
                        }
                    }
                }
                ChoiceActionItem parentInvoicing = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "Accounting");
                if (parentInvoicing != null && parentInvoicing.Id == "Accounting")
                {
                    ChoiceActionItem Invoicing = parentInvoicing.Items.FirstOrDefault(i => i.Id == "Invoicing");
                    if (Invoicing != null)
                    {
                        ChoiceActionItem childInvoiceQueue = Invoicing.Items.FirstOrDefault(i => i.Id == "InvoiceQueue" || i.Id == "InvoiceQueue ");
                        if (childInvoiceQueue != null)
                        {
                            int count = 0;
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            List<Guid> lstinvoiceOid = new List<Guid>();
                            IList<SampleParameter> samples = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status] = 'Reported' And ([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null)"));
                            List<Guid> lstScGuid = samples.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && (i.Samplelogin.JobID.ProjectCategory is null || (i.Samplelogin.JobID.ProjectCategory != null && i.Samplelogin.JobID.ProjectCategory.Non_Commercial != Modules.BusinessObjects.Setting.ProjectCategory.CommercialType.Yes))).Select(i => i.Samplelogin.JobID.Oid).Distinct().ToList();
                            foreach (Guid obj in lstScGuid)
                            {
                                IList<SampleParameter> lstsamplesInvoice = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status] <> 'Reported' And [Samplelogin.JobID.Oid]=? ", obj));
                                if (lstsamplesInvoice.Count == 0)
                                {
                                    if (!lstinvoiceOid.Contains(obj))
                                    {
                                        lstinvoiceOid.Add(obj);
                                        count = count + 1;
                                    }
                                }
                            }

                            var cap = childInvoiceQueue.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            if (count > 0)
                            {
                                childInvoiceQueue.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                childInvoiceQueue.Caption = cap[0];
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
        private void SaveReportView_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (cmbReportName.SelectedItem != null && cmbReportName.SelectedIndex != 0)// null)
                {
                    if (View != null && View.SelectedObjects.Count > 0)
                    {
                        //WebWindow.CurrentRequestWindow.RegisterStartupScript("customDialog", GetCustomDialogScript());
                        WebWindow.CurrentRequestWindow.RegisterClientScript("Openspreadsheet", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('Do you want to generate a new report ID ? '); {0}", SaveReportEdit.CallbackManager.GetScript("SaveReportEdit", "openconfirm")));
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "nodata"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "reportselect"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Revision_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                Modules.BusinessObjects.SampleManagement.Reporting objReporting = os.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(objRQPInfo.curReport);
                Modules.BusinessObjects.SampleManagement.Reporting objReport = e.AcceptActionArgs.CurrentObject as Modules.BusinessObjects.SampleManagement.Reporting;
                if (objReport != null && objReport.RevisionReason != null)
                {
                    objRQPInfo.RevisionReason = objReport.RevisionReason;
                    if (objReporting != null)
                    {
                        if (objReporting.RevisionReason == null)
                        {
                            IList<Modules.BusinessObjects.SampleManagement.Reporting> lstRep = os.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[PreviousReportID] = ?", objReporting.ReportID));
                            if (lstRep.Count == 0)
                            {
                                objRQPInfo.Revision = 1;
                            }
                            else
                            {
                                objRQPInfo.Revision = os.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[PreviousReportID] = ?", objReporting.ReportID)).Select(i => i.RevisionNo).Max() + 1;
                            }
                            objRQPInfo.PreviousReportID = objReporting.ReportID;
                        }
                        else
                        {
                            objRQPInfo.Revision = os.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[PreviousReportID] = ?", objReporting.PreviousReportID)).Select(i => i.RevisionNo).Max() + 1;
                            objRQPInfo.PreviousReportID = objReporting.PreviousReportID;
                        }
                    }
                    SaveReport_Execute(new object(), new SimpleActionExecuteEventArgs(null, null));
                    View.Close();
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "RevisionReason"), InformationType.Error, timer.Seconds, InformationPosition.Top);

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
            if (View.Id == "SampleParameter_ListView_Copy_CustomReporting_Edit")
            {
                if (parameter == "true")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    Modules.BusinessObjects.SampleManagement.Reporting objReporting = os.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(objRQPInfo.curReport);
                    if (objReporting != null && !EditCancel)
                    {
                        if (objReporting.RevisionReason != null)
                        {
                            objReporting.RevisionReason = null;
                        }
                        DetailView detailView = Application.CreateDetailView(os, "Reporting_DetailView_Revision", true, objReporting);
                        detailView.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters(detailView);
                        showViewParameters.Context = TemplateContext.NestedFrame;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Revision_Accepting;
                        dc.Cancelling += Dc_Cancelling;
                        //dc.AcceptAction.Executed += Rollback_Executed;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        //objRQPInfo.curReport = null;
                    }
                    else
                    {
                        //IObjectSpace os = Application.CreateObjectSpace();
                        //Modules.BusinessObjects.SampleManagement.Reporting objReporting = os.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(objRQPInfo.curReport);
                        boolReportSave = true;
                        ReportPreview_Execute(new object(), new SimpleActionExecuteEventArgs(null, null));
                        FileLinkSepDBController obj = Frame.GetController<FileLinkSepDBController>();
                        if (obj != null)
                        {
                            obj.FileLinkUpdate(objReporting.ReportID, Sampleinfo.bytevalues);
                        }
                        Frame.GetController<AuditlogViewController>().insertauditdata(os, objReporting.JobID.Oid, OperationType.ValueChanged, "Report Tracking", objReporting.JobID.ToString(), "ReportName", objReporting.ReportName, Sampleinfo.curReportName, "");
                        IList<AuditData> Objects = os.GetObjects<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                        if (Objects.Count > 0)
                        {
                            Frame.GetController<AuditlogViewController>().getcomments(os, Objects.First(), ViewEditMode.Edit);

                        }





                        //DetailView detailView = Application.CreateDetailView(os, "Reporting_DetailView_Revision_Copy", true, objReporting);
                        //detailView.ViewEditMode = ViewEditMode.Edit;
                        //ShowViewParameters showViewParameters = new ShowViewParameters(detailView);
                        //showViewParameters.Context = TemplateContext.NestedFrame;
                        //showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        //DialogController dc = Application.CreateController<DialogController>();
                        //dc.SaveOnAccept = false;
                        //dc.CloseOnCurrentObjectProcessing = false;
                        //dc.Accepting += Revision_Accepting1;
                        ////dc.AcceptAction.Executed += Rollback_Executed;
                        //showViewParameters.Controllers.Add(dc);
                        //Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        ////Frame.GetController<AuditlogViewController>().insertauditdata(os, objReporting.Oid, OperationType.ValueChanged, "Report Tracking", objReporting.JobID.ToString(), "ReportName", objReporting.ReportName, Sampleinfo.curReportName, "");
                        //Frame.GetController<AuditlogViewController>().insertauditdata(os, objReporting.Oid, OperationType.ValueChanged, "Report Tracking", objReporting.JobID.ToString(), "ReportName", objReporting.ReportName, objReporting.ReportName, objReporting.RevisionReason);
                        //AuditData Objects = os.FindObject<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                        //if (Objects != null)
                        //{
                        //    Objects.CommentProcessed = true;
                        //}


                        //os.CommitChanges();
                        //IList<AuditData> Objects = os.GetObjects<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                        //if (Objects.Count > 0)
                        //{
                        //     Frame.GetController<AuditlogViewController>().getcomments(os, Objects.First(), ViewEditMode.Edit);

                        //}
                        //if (!commentsActionCompleted)
                        //{
                        //    Application.ShowViewStrategy.ShowMessage("Report " + objReporting.ReportID + CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "savedsuccessfully"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        //    EditCancel = false;
                        //    View.Close();
                        //}

                        //IList<AuditData> Objects = View.ObjectSpace.GetObjects<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                        //if (Objects.Count() > 0)
                        //{
                        //    Frame.GetController<AuditlogViewController>().commentdialog(sender, Objects.First());
                        //}
                        //Frame.GetController<AuditlogViewController>().insertauditdata(os, objReporting.Oid, OperationType.ValueChanged, "Report Tracking", objReporting.JobID.ToString(), "ReportName", objReporting.ReportName, Sampleinfo.curReportName, "");

                    }
                }
                else
                {
                    if (!EditCancel)
                    {
                        WebWindow.CurrentRequestWindow.RegisterClientScript("Openspreadsheet", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('Do you want to update existing report ID ? '); {0}", SaveReportEdit.CallbackManager.GetScript("SaveReportEdit", "openconfirm")));
                        EditCancel = true;
                    }
                    else
                    {
                        EditCancel = false;
                    }
                }
            }
        }

        //private void Revision_Accepting1(object sender, DialogControllerAcceptingEventArgs e)
        //{
        //    Modules.BusinessObjects.SampleManagement.Reporting objReporting = ObjectSpace.GetObject<Modules.BusinessObjects.SampleManagement.Reporting>(objRQPInfo.curReport);
        //    if(objReporting != null)
        //    {
        //        Application.ShowViewStrategy.ShowMessage("Report " + objReporting.ReportID + CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "savedsuccessfully"), InformationType.Success, timer.Seconds, InformationPosition.Top);
        //        EditCancel = false;
        //        View.Close();
        //    }

        //}

        private void Dc_Cancelling(object sender, EventArgs e)
        {
            if (objAuditInfo.SaveData != true)
            {
                objAuditInfo.comment = string.Empty;
                objAuditInfo.SaveData = null;
                objAuditInfo.action = null;
                objAuditInfo.choiceaction = null;
                objAuditInfo.choiceactionitem = null;
                Application.ShowViewStrategy.ShowMessage("Unsaved changes exist", InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void ReportIDFormat(Session currentSession)
        {
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
                        var latestReport = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>().Where(r => r.JobID.JobID == jd && r.RevisionNo == 0 /*&& r.NewReportFormat is true*/).OrderByDescending(r => r.ReportedDate).FirstOrDefault();
                        if (latestReport != null && latestReport.JobID != null && latestReport.NewReportFormat == true)
                        {
                            //latestReport.NewReportFormat = true;
                            string latestJobID = latestReport.JobID.JobID;
                            bool isJobIDMatch = latestJobID.Contains(jd);
                            if (isJobIDMatch)
                            {
                                //latestReport = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>().Where(r => r.ReportID.Contains(jd)).OrderByDescending(r => r.ReportedDate).FirstOrDefault();
                                if (latestReport.JobID.JobID == jd)
                                {
                                    if (latestReport.LastSequentialNumber == 0 || latestReport.LastSequentialNumber == idFormat.SequentialNumber)
                                    {
                                        string baseValue = latestReport.ReportID.Substring(0, latestReport.ReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                                        string lastDigits = latestReport.ReportID.Substring(latestReport.ReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                                        int nextSequentialNumber = int.Parse(lastDigits) + 1;
                                        strReportIDT += nextSequentialNumber.ToString().PadLeft(Convert.ToInt32(idFormat.SequentialNumber), '0');
                                    }
                                    else
                                    {
                                        string baseValue = latestReport.ReportID.Substring(0, latestReport.ReportID.Length - Convert.ToInt32(latestReport.LastSequentialNumber));
                                        string lastDigits = latestReport.ReportID.Substring(latestReport.ReportID.Length - Convert.ToInt32(latestReport.LastSequentialNumber));
                                        int nextSequentialNumber = int.Parse(lastDigits) + 1;
                                        strReportIDT += nextSequentialNumber.ToString().PadLeft(Convert.ToInt32(latestReport.LastSequentialNumber), '0');
                                    }
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

                        var latestReport = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>().Where(r => r.RevisionNo == 0).OrderByDescending(r => r.ReportedDate).FirstOrDefault(r => r.ReportID.Contains(currentDateSubstring));
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

                            if (latestReport.LastSequentialNumber == 0 || latestReport.LastSequentialNumber == idFormat.SequentialNumber)
                            {
                                string baseValue = latestReport.ReportID.Substring(0, latestReport.ReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                                string lastDigits = latestReport.ReportID.Substring(latestReport.ReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                                int nextSequentialNumber = int.Parse(lastDigits) + 1;
                                strReportIDT += nextSequentialNumber.ToString().PadLeft(Convert.ToInt32(idFormat.SequentialNumber), '0');
                            }
                            else
                            {
                                string baseValue = latestReport.ReportID.Substring(0, latestReport.ReportID.Length - Convert.ToInt32(latestReport.LastSequentialNumber));
                                string lastDigits = latestReport.ReportID.Substring(latestReport.ReportID.Length - Convert.ToInt32(latestReport.LastSequentialNumber));
                                int nextSequentialNumber = int.Parse(lastDigits) + 1;
                                strReportIDT += nextSequentialNumber.ToString().PadLeft(Convert.ToInt32(idFormat.SequentialNumber), '0');
                            }

                            //string baseValue = strReportID.Substring(0, strReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                            //string lastDigits = strReportID.Substring(strReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                            //int nextSequentialNumber = int.Parse(lastDigits) + 1;
                            //strReportIDT += nextSequentialNumber.ToString().PadLeft(Convert.ToInt32(idFormat.SequentialNumber), '0');
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

        private void CaseNarrative_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace(typeof(Notes));
            Notes objcrtdummy = os.CreateObject<Notes>();
            CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Notes));
            cs.Criteria["filter"] = CriteriaOperator.Parse("[Samplecheckin.Oid] = ? and [GCRecord] is NULL", CNInfo.SCoidValue);
            ListView lvparameter = Application.CreateListView("Notes_ListView_CaseNarrative", cs, false);
            ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
            showViewParameters.CreatedView = lvparameter;
            showViewParameters.Context = TemplateContext.PopupWindow;
            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController dc = Application.CreateController<DialogController>();
            dc.SaveOnAccept = false;
            dc.CloseOnCurrentObjectProcessing = false;
            dc.Accepting += CaseDC_Accepting;
            showViewParameters.Controllers.Add(dc);
            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
        }

        private void CaseDC_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            DialogController dc = (DialogController)sender;
            if (dc != null && dc.Window != null && dc.Window.View != null)
            {
                foreach (Notes note in ((ListView)dc.Window.View).CollectionSource.List)
                {
                    if (dc.Window.View.SelectedObjects.Count > 0 && dc.Window.View.SelectedObjects.Cast<Notes>().Select(i => i.Oid).Contains(note.Oid))
            {
                note.IsCaseNarrative = true;

                    }
                    else
                    {
                        note.IsCaseNarrative = false;
                    }
                }
            }

        }
    }
}
