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
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.Public;
using LDM.Module.Controllers.SampleRegistration;
using LDM.Module.Controllers.SamplingManagement;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.PLM;
using Modules.BusinessObjects.Setting.SamplesSite;
using Modules.BusinessObjects.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using ListView = DevExpress.ExpressApp.ListView;
namespace BTLIMS.Module.Controllers
{
    public class SampleLogInViewController : ViewController, IXafCallbackHandler
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        private System.ComponentModel.IContainer components;
        bool bolRefresh = false;
        ModificationsController mdcSave;
        ModificationsController mdcSaveClose;
        ModificationsController mdcSaveNew;
        List<string> lstbotleid = new List<string>();
        SampleLogInInfo objSLInfo = new SampleLogInInfo();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        CopyNoOfSamplesPopUp objCopySampleInfo = new CopyNoOfSamplesPopUp();
        private SimpleAction CopySamples;
        SampleCheckInInfo objSCInfo = new SampleCheckInInfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        private SimpleAction Btn_Add_Collector;
        private SimpleAction Reanalysis;
        private SimpleAction SaveSampleLogin;
        private CriteriaOperator cs;
        Guid SampleOid = Guid.Empty;
        #endregion

        #region Constructor
        public SampleLogInViewController()
        {
            InitializeComponent();
            this.TargetViewId = "SampleLogIn_DetailView;" + "TestMethod_LookupListView;" + "SampleLogIn_ListView;" + "Testparameter_LookupListView_Copy_SampleLogin;" + "Testparameter_LookupListView_Copy_SampleLogin_Copy;" //+ "COCTestparameter_LookupListView_Copy_SampleLogin;"
                + "SampleLogIn_Testparameters_ListView;" + "SampleLogIn_LookupListView_Copy_SampleLogin;" + "SampleLogIn_ListView_Copy_SampleRegistration;" + "SampleLogIn_ListView_SourceSample;"+ "SampleSites_LookupListView_Sampling;"
                + "SampleSites_LookupListView_Sampling_StationLocation;"+ "SampleRegistration_SampleLogin;";
            CopySamples.TargetViewId = "SampleLogIn_DetailView;" + "SampleLogIn_ListView;" + "SampleLogIn_ListView_Copy_SampleRegistration;";
            Btn_Add_Collector.TargetViewId = "SampleLogIn_ListView_Copy_SampleRegistration";
            Reanalysis.TargetViewId = "SampleLogIn_ListView_Copy_SampleRegistration";
            Reanalysis.ImageName = "Action_ResetViewSettings";
        }
        #endregion



        #region DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View.ObjectTypeInfo!=null && View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                {
                    //ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                    View.SelectionChanged += new EventHandler(View_SelectionChanged);
                    //View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);
                }
                else if (View.Id == "SampleSites_LookupListView_Sampling" || View.Id == "SampleSites_LookupListView_Sampling_StationLocation")
                {
                    View.ControlsCreated += View_ControlsCreated;
                }
                else if (View.Id == "SampleLogIn_Testparameters_ListView")
                {
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Executing += UnlinkAction_Executing;
                }
                else if(View.Id== "SampleRegistration_SampleLogin")
                {
                    View.Closing += View_Closing;
                }
                #region SampleRegistration Hide Custom NewAction
                Frame.GetController<SampleRegistrationViewController>().Actions["SR_SLDetailViewNew"].Active.SetItemValue("Show", false);
                Frame.GetController<SampleRegistrationViewController>().Actions["SampleRegistrationSL_Save"].Active.SetItemValue("Show", false);
                if (View.Id == "SampleLogIn_ListView_Copy_SampleRegistration")
                {
                    if (objPermissionInfo.SampleRegIsWrite == false)
                    {
                        CopySamples.Active["showCopySamples"] = false;
                        Btn_Add_Collector.Active["showAddCollector"] = false;
                        Reanalysis.Active["btnReanalysis"] = false;
                        SaveSampleLogin.Active["btnSaveSampleLogin"] = false;
                    }
                    else
                    {
                        CopySamples.Active["showCopySamples"] = true;
                        Btn_Add_Collector.Active["showAddCollector"] = true;
                        Reanalysis.Active["btnReanalysis"] = true;
                        SaveSampleLogin.Active["btnSaveSampleLogin"] = true;
                    }
                }
                CopySamples.Executing += CopySamples_Executing;
                #endregion
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void View_Closing(object sender, EventArgs e)
        {
           try
            {
                DashboardViewItem lvSamples = ((DashboardView)View).FindItem("SampleLogin") as DashboardViewItem;
                if (lvSamples != null && lvSamples.InnerView != null && lvSamples.InnerView.ObjectSpace.ModifiedObjects.Count > 0)
                {
                    lvSamples.InnerView.ObjectSpace.CommitChanges();
                    lvSamples.InnerView.ObjectSpace.Refresh();
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void View_ControlsCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (gridListEditor != null && gridListEditor.Grid != null)
                {
                    if (View.Id == "SampleSites_LookupListView_Sampling")
                    {
                        if (HttpContext.Current.Session["AlternativeStation"] != null)
                        {
                            string[] assignedto = HttpContext.Current.Session["AlternativeStation"].ToString().Split(new string[] { "; " }, StringSplitOptions.None);
                            foreach (string val in assignedto.Where(i => !string.IsNullOrEmpty(i)))
                            {
                                SampleSites employee = ObjectSpace.FindObject<SampleSites>(CriteriaOperator.Parse("[Oid]=?", new Guid(val)));
                                if (employee != null)
                                {
                                    gridListEditor.Grid.Selection.SelectRowByKey(employee.Oid);
                                }
                            }
                        }
                    }
                    else if (View.Id == "SampleSites_LookupListView_Sampling_StationLocation")
                    {
                        if (HttpContext.Current.Session["StationLocation"] != null)
                        {
                            string str = HttpContext.Current.Session["StationLocation"].ToString();
                            SampleSites site = ObjectSpace.FindObject<SampleSites>(CriteriaOperator.Parse("[Oid]=?", new Guid(HttpContext.Current.Session["StationLocation"].ToString())));
                            if (site != null)
                            {
                                gridListEditor.Grid.Selection.SelectRowByKey(site.Oid);
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
        private void CopySamples_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View != null && View.SelectedObjects.Count == 0)
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
                else if (View != null && View.SelectedObjects.Count > 1)
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlyonechkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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
                //ObjectSpace.Committing -= ObjectSpace_Committing;
                if (View != null && View.Id == "SampleLogIn_DetailView")
                {
                    //ObjectSpace.Committed -= ObjectSpace_Committed;
                }
                if (View.ObjectTypeInfo!=null && View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                {
                    //ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                    View.SelectionChanged -= new EventHandler(View_SelectionChanged);
                    //View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
                }
                else if (View.Id == "SampleLogIn_Testparameters_ListView")
                {
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Executing -= UnlinkAction_Executing;
                }
                else if (View.Id == "SampleRegistration_SampleLogin")
                {
                    View.Closing -= View_Closing;
                }
                if (View != null && View.Id == "SampleLogIn_DetailView")
                {
                    objSLInfo.SLVisualMatrixName = string.Empty;
                }
                else if (View.Id == "SampleLogIn_ListView_SourceSample")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Load -= Grid_Load;
                        gridListEditor.Grid.SelectionChanged -= Grid_SelectionChanged;
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
                if (View.Id == "Testparameter_LookupListView_Copy_SampleLogin")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.SettingsPager.AlwaysShowPager = true;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e){ 
                s.SetWidth(400); 
                s.RowClick.ClearHandlers();
                }";
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                    //gridListEditor.Grid.SettingsBehavior.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
                    //gridListEditor.Grid.CustomColumnSort += Grid_CustomColumnSort;
                    //gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s, e) { s.PerformCallback(); }";
                }
                else if (View.Id == "SampleLogIn_ListView_Copy_SampleRegistration")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesController");
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                        if( e.focusedColumn.fieldName == 'SampleSource')
                                             {
                                              e.cancel = true;
                                              }
                                          else
                                               {
                                                    e.cancel = false;
                                               }
                                           }";
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("SampleSourcePopup", this);
                    }
                    //gridListEditor.Grid.HtmlCommandCellPrepared += GridView_HtmlCommandCellPrepared;
                }
                else if (View.Id == "SampleLogIn_ListView_SourceSample")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("SampleSourceSelected", this);
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Load += Grid_Load;
                        //gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                        gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                          if (e.visibleIndex != '-1')
                          {
                            s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                             if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                                RaiseXafCallback(globalCallbackControl, 'SampleSourceSelected', 'Selected|' + Oidvalue , '', false);    
                             }
                            }); 
                          }             
                        }";
                    }
                }
                else if (View.Id == "SampleSites_LookupListView_Sampling" || View.Id == "SampleSites_LookupListView_Sampling_StationLocation")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 410;
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
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
                if (View.Id == "SampleLogIn_ListView_SourceSample")
                {
                    ASPxGridView gridView = sender as ASPxGridView;

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
                ASPxGridView gridview = (ASPxGridView)sender;
                if (gridview != null)
                {
                    var selectionBoxColumn = gridview.Columns.OfType<GridViewCommandColumn>().Where(i => i.ShowSelectCheckbox).FirstOrDefault();
                    if (selectionBoxColumn != null)
                    {
                        selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
                    }
                    if (SampleOid != Guid.Empty)
                    {
                        gridview.Selection.UnselectRowByKey(SampleOid);
                        SampleOid = Guid.Empty;
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
                if (View.Id == "SampleLogIn_ListView_Copy_SampleRegistration" && objPermissionInfo.SampleRegIsWrite)
                {
                    if (e.DataColumn.FieldName == "SampleSource" || e.DataColumn.FieldName == "AlternativeStation" || e.DataColumn.FieldName == "StationLocation.Oid"|| e.DataColumn.FieldName == "StationLocationName")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'SampleSourcePopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
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


        #endregion

        

        #region Desginer
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.CopySamples = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Btn_Add_Collector = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Reanalysis = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SaveSampleLogin = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CopySamples
            // 
            this.CopySamples.Caption = "Copy Samples";
            this.CopySamples.ConfirmationMessage = null;
            this.CopySamples.Id = "CopySamples";
            this.CopySamples.TargetViewId = "";
            this.CopySamples.ToolTip = null;
            this.CopySamples.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CopySamples_Execute);
            // 
            // Btn_Add_Collector
            // 
            this.Btn_Add_Collector.Caption = "Add New Collector";
            this.Btn_Add_Collector.ConfirmationMessage = null;
            this.Btn_Add_Collector.Id = "Add_Collector";
            this.Btn_Add_Collector.ImageName = "Add_16x16";
            this.Btn_Add_Collector.ToolTip = null;
            this.Btn_Add_Collector.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Btn_Add_Collector_Execute);
            // 
            // Reanalysis
            // 
            this.Reanalysis.Caption = "Reanalysis";
            this.Reanalysis.Category = "Reports";
            this.Reanalysis.ConfirmationMessage = null;
            this.Reanalysis.Id = "Reanalysis";
            this.Reanalysis.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Reanalysis.TargetViewId = "SampleLogIn_ListView_Copy_SampleRegistration";
            this.Reanalysis.ToolTip = null;
            this.Reanalysis.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Reanalysis_Execute);
            // 
            // Save
            // 
            this.SaveSampleLogin.Caption = "Save";
            this.SaveSampleLogin.Category = "View";
            this.SaveSampleLogin.ConfirmationMessage = null;
            this.SaveSampleLogin.Id = "SaveSampleLogin";
            this.SaveSampleLogin.ImageName = "Action_Save";
            this.SaveSampleLogin.TargetViewId = "SampleLogIn_ListView_Copy_SampleRegistration";
            this.SaveSampleLogin.ToolTip = null;
            this.SaveSampleLogin.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Save_SampleLogin);
            // 
            // SampleLogInViewController
            // 
            this.Actions.Add(this.CopySamples);
            this.Actions.Add(this.Btn_Add_Collector);
            this.Actions.Add(this.Reanalysis);
            this.Actions.Add(this.SaveSampleLogin);
            this.ViewControlsCreated += new System.EventHandler(this.SampleLogInViewController_ViewControlsCreated);

        }
        #endregion

        #region Events
      

        private void ObjectSpace_Committing(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.ObjectTypeInfo.Type == typeof(SampleLogIn) && objSLInfo.boolCopySamples == false)
                {
                    SampleLogIn sl = (SampleLogIn)View.CurrentObject;
                    if (sl != null)
                    {
                        Guid id = sl.Oid;
                        if (sl.SampleID != string.Empty)
                        {
                            if (!sl.IsDeleted)
                            {
                                var obj = ObjectSpace.GetObjects<SampleLogIn>(CriteriaOperator.Parse("[Oid]='" + sl.Oid + "'"));
                                if (obj.Count == 0)
                                {
                                    CriteriaOperator criteria = null;
                                    criteria = CriteriaOperator.Parse("JobID.JobID='" + sl.JobID.JobID + "' AND SampleNo='" + sl.SampleNo + "'");
                                    bool exists = Convert.ToBoolean(ObjectSpace.Evaluate(typeof(SampleLogIn), (new AggregateOperand("", Aggregate.Exists)), (criteria)));
                                    if (exists)
                                    {
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "sampleidexists") + (sl.SampleNo).ToString() + " automatically changed to " + (sl.SampleNo + 1).ToString() + ".", InformationType.Info, timer.Seconds, InformationPosition.Top);
                                        sl.SampleNo = sl.SampleNo + 1;
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
        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "SampleLogIn_DetailView")
                {
                    SampleLogIn sl = (SampleLogIn)View.CurrentObject;
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    if (sl != null)
                    {
                        SelectedData sproc = currentSession.ExecuteSproc("StatusUpdate_SP", new OperandValue(sl.Oid));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void UnlinkAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View != null && View.ObjectTypeInfo.Type == typeof(Testparameter) && View.Id == "SampleLogIn_Testparameters_ListView")
                {
                    foreach (Testparameter obj in View.SelectedObjects)
                    {
                        var os = Application.CreateObjectSpace();
                        IList<SampleParameter> lst = (IList<SampleParameter>)obj.SampleParameter;
                        foreach (var li in lst)
                        {
                            if (obj == li.Testparameter)
                            {
                                if (objSLInfo.JobID == li.Samplelogin.Oid.ToString())
                                {
                                    if (li.Result != null && li.Result != string.Empty)
                                    // if (li.Result != null)
                                    {
                                        try
                                        {
                                            e.Cancel = true;
                                            throw new UserFriendlyException(string.Format("Unable to Unlink Result Have Been Entered For {0}  {1}  {2}  {3}", li.Testparameter.TestMethod.MatrixName.MatrixName, li.Testparameter.TestMethod.TestName, li.Testparameter.TestMethod.MethodName.MethodName, li.Testparameter.Parameter.ParameterName));
                                        }
                                        catch (UserFriendlyException ex)
                                        {
                                            Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                                            Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
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


        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "JobID")
                {
                    if (View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                    {
                        SampleLogIn objSampleLogIn = (SampleLogIn)e.Object;

                        if (objSampleLogIn.JobID != null)
                        {
                            objSLInfo.focusedJobID = objSampleLogIn.JobID.JobID;
                            objSampleLogIn.ProjectID = objSampleLogIn.JobID.ProjectID;
                            Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;

                            SelectedData sproc = currentSession.ExecuteSproc("GetSampleID", new OperandValue(objSampleLogIn.JobID.JobID));
                            if (sproc.ResultSet[1].Rows[0].Values[0].ToString() != null)
                            {
                                objSampleLogIn.SampleNo = Convert.ToInt32(sproc.ResultSet[1].Rows[0].Values[0].ToString());
                                objSampleLogIn.SampleID = string.Format("{0}{1}{2}", objSampleLogIn.JobID.JobID, "-", objSampleLogIn.SampleNo.ToString());
                            }
                            if (objSampleLogIn.QCType != null)
                            {
                                objSampleLogIn.QCType = null;
                            }
                        }
                    }
                }
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "ProjectID")
                {
                    if (View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                    {
                        SampleLogIn objSampleLogIn = (SampleLogIn)e.Object;
                        if (objSampleLogIn.ProjectID != null)
                        {
                            objSampleLogIn.ProjectName = objSampleLogIn.ProjectID.ProjectName;
                        }
                        else
                        {
                            objSampleLogIn.ProjectName = string.Empty;
                        }

                    }
                }
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "VisualMatrix")
                {
                    bool bolTestparam = false;

                    if (View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                    {
                        SampleLogIn objSampleLogIn = (SampleLogIn)e.Object;
                        if (objSampleLogIn.VisualMatrix != null)
                        {
                            string strOldVisualMatrix = objSampleLogIn.VisualMatrix.VisualMatrixName.ToString();
                            if (CheckIFExists(objSampleLogIn))
                            {
                                var objectSpace = Application.CreateObjectSpace();
                                var objLogin = objectSpace.FindObject<SampleLogIn>(CriteriaOperator.Parse("Oid = ?", objSampleLogIn.Oid));
                                if (objLogin == null)
                                {
                                    for (int i = objSampleLogIn.Testparameters.Count - 1; i >= 0; i--)
                                    {
                                        objSampleLogIn.Testparameters.Remove(objSampleLogIn.Testparameters[i]);
                                        bolRefresh = true;
                                    }
                                }
                                else
                                {
                                    foreach (Testparameter objTestParam in objSampleLogIn.Testparameters)
                                    {
                                        var osTestParam = Application.CreateObjectSpace();
                                        IList<SampleParameter> lstSampleParam = (IList<SampleParameter>)objTestParam.SampleParameter;
                                        {
                                            foreach (var li in lstSampleParam)
                                            {
                                                if (objTestParam == li.Testparameter)
                                                {
                                                    if (objSampleLogIn.Oid.ToString() == li.Samplelogin.Oid.ToString())
                                                    {
                                                        if (li.Result != null && li.Result != string.Empty)
                                                        // if (li.Result != null)
                                                        {
                                                            bolTestparam = true;
                                                            try
                                                            {
                                                                throw new UserFriendlyException(string.Format("Result Already Entered Cannot allow to change the Sample Matrix"));

                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                DevExpress.ExpressApp.Web.ErrorHandling.Instance.SetPageError(ex);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    if (bolTestparam == false)
                                    {
                                        for (int i = objSampleLogIn.Testparameters.Count - 1; i >= 0; i--)
                                        {
                                            objSampleLogIn.Testparameters.Remove(objSampleLogIn.Testparameters[i]);
                                            bolRefresh = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objSLInfo.SLVisualMatrixName = objSampleLogIn.VisualMatrix.MatrixName.MatrixName;
                            }
                            if (bolRefresh == true)
                            {
                                View.Refresh();
                                bolRefresh = false;
                            }
                        }
                        else
                        {
                            objSLInfo.SLVisualMatrixName = string.Empty;
                        }
                    }

                }
                //if (View != null && View.Id == "SampleLogIn_ListView_Copy_SampleRegistration" && e.Object != null && (e.PropertyName == "TimeStart" || e.PropertyName == "TimeEnd"))
                //{
                //    if (View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                //    {
                //        SampleLogIn objSampleLogIn = (SampleLogIn)e.Object;
                //        if (objSampleLogIn.TimeStart != null && objSampleLogIn.TimeEnd != null)
                //        {
                //            Nullable<DateTime> TimeStart = objSampleLogIn.TimeStart;
                //            Nullable<DateTime> TimeEnd = objSampleLogIn.TimeEnd;
                //            double t = (TimeEnd - TimeStart).Value.TotalMinutes;
                //            objSampleLogIn.Time = t.ToString();//Convert.ToInt32((t));
                //        }
                //    }
                //}
                //if (View != null && View.Id == "SampleLogIn_ListView_Copy_SampleRegistration" && e.Object != null && (e.PropertyName == "Length" || e.PropertyName == "Width"))
                //{
                //    if (View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                //    {
                //        SampleLogIn objSampleLogIn = (SampleLogIn)e.Object;
                //        if (objSampleLogIn.Length != null && objSampleLogIn.Width != null)
                //        {
                //            int intleng = Convert.ToInt32(objSampleLogIn.Length)
                //            int intwidth = Convert.ToInt32(objSampleLogIn.Width)
                //            int area = intleng * intwidth;
                //            objSampleLogIn.Time = area.ToString();//Convert.ToInt32((t));
                //        }
                //    }
                //}

                //if (View != null && View.Id == "SampleLogIn_ListView_Copy_SampleRegistration" && (e.PropertyName == "Time" || e.PropertyName == "FlowRate"))
                //{
                //    if (View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                //    {
                //        SampleLogIn crtSampleLogIn = (SampleLogIn)e.Object;
                //        IObjectSpace os = Application.CreateObjectSpace();
                //        SampleLogIn objSampleLogIn = os.FindObject<SampleLogIn>(CriteriaOperator.Parse("[Oid] = ?", crtSampleLogIn.Oid));
                //        if (objSampleLogIn != null)
                //        {
                //            int flowrate = 0;
                //            int minutes = 0;
                //            if (!string.IsNullOrEmpty(objSampleLogIn.Time))
                //            {
                //                minutes = Convert.ToInt32(objSampleLogIn.Time);
                //            }
                //            if (!string.IsNullOrEmpty(objSampleLogIn.FlowRate))
                //            {
                //                flowrate = Convert.ToInt32(objSampleLogIn.FlowRate);
                //            }
                //            if (flowrate > 0 && minutes > 0)
                //            {
                //                objSampleLogIn.Volume = (flowrate * minutes).ToString();
                //            }
                //            else
                //            {
                //                objSampleLogIn.Volume = string.Empty;
                //            }
                //        }
                //        os.CommitChanges();
                //        os.Refresh();
                //    }
                //}

                if (View != null && View.Id == "SampleLogIn_ListView_Copy_SampleRegistration" && /*e.GetType() == typeof(SampleLogIn) &&*/ e.PropertyName == "Containers")
                {
                    if (View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                    {
                        SampleLogIn objSampleLogIn = (SampleLogIn)e.Object;
                        if (objSampleLogIn.Containers <= 0)
                        {
                            objSampleLogIn.Containers = 1;
                            Application.ShowViewStrategy.ShowMessage("Containers Should not be less than 1", InformationType.Warning, 3000, InformationPosition.Top);
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
       
        private bool CheckIFExists(SampleLogIn objSL)
        {
            try
            {
                if (objSL.Testparameters.Count > 0)
                {
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
        private void SampleLogInViewController_ViewControlsCreated(object sender, EventArgs e)
        {
            try
            {
                if (View is DetailView && View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                {
                    if (objSCInfo.JobID != string.Empty)
                    {

                        SampleLogIn sl = (SampleLogIn)View.CurrentObject;
                        if (sl != null)
                        {
                            Samplecheckin sc = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]='" + objSCInfo.JobID + "'"));
                            sl.JobID = sc;
                            ObjectChangedEventArgs ee = new ObjectChangedEventArgs(sl.JobID, "JobID", null, sc);
                            objectSpace_ObjectChanged(sl.JobID, ee);
                            objSCInfo.JobID = string.Empty;
                        }
                    }
                    object obj = View.CurrentObject;
                    if (View.CurrentObject != null)
                    {
                        objSLInfo.JobID = View.ObjectSpace.GetKeyValue(View.CurrentObject).ToString();
                        SampleLogIn sl = (SampleLogIn)View.CurrentObject;
                        if (sl.JobID != null)
                        {
                            CopySamples.Enabled["enable"] = true;
                            objSLInfo.focusedJobID = sl.JobID.JobID;
                        }

                        else
                        {
                            CopySamples.Enabled["enable"] = false;
                        }
                    }
                }
                if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Testparameter) && View.Id == "Testparameter_LookupListView_Copy_SampleLogin")
                {

                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName]='" + objSLInfo.SLVisualMatrixName + "' AND [TestMethod.GCRecord] IS NULL AND (([TestMethod.RetireDate] IS NULL OR [TestMethod.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')) AND " +
                       " ([TestMethod.MethodName.RetireDate] IS NULL OR [TestMethod.MethodName.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "') AND ([Parameter.RetireDate] IS NULL OR [Parameter.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')" + "AND([RetireDate] IS NULL OR[RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')" +
                        "AND ([InternalStandard] == False or [InternalStandard] IS NULL ) AND ([Surroagate] == False or [Surroagate] IS NULL)");
                   

                }
                if (View.Id == "Testparameter_LookupListView_Copy_SampleLogin_Copy")
                {
                    //DashboardViewItem TestViewSubChild = ((NestedFrame)Frame).ViewItem.View.FindItem("TestViewSubChild") as DashboardViewItem;
                    //if ((SRInfo.lstTestParameter == null || SRInfo.lstTestParameter.Count == 0) && (TestViewSubChild != null && ((ListView)TestViewSubChild.InnerView).CollectionSource.GetCount() == 0))
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    //}
                }
                if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(SampleLogIn) && View.Id == "SampleLogIn_LookupListView_Copy_SampleLogin")
                {
                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[JobID.JobID]=='" + objSLInfo.focusedJobID + "' AND [GCRecord] IS NULL");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        private void CopySamples_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (gridListEditor != null && gridListEditor.Grid != null)
                {
                    gridListEditor.Grid.UpdateEdit();
                }
                if(View.ObjectSpace.ModifiedObjects.Count>0)
                {
                    View.ObjectSpace.CommitChanges();
                }
                IObjectSpace objspace = Application.CreateObjectSpace();
                SL_CopyNoOfSamples copyNoOfSamples = objspace.CreateObject<SL_CopyNoOfSamples>();
                DetailView dvcopysample = Application.CreateDetailView(objspace, "SL_CopyNoOfSamples_DetailView", false, copyNoOfSamples);
                dvcopysample.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(dvcopysample);
                showViewParameters.CreatedView = dvcopysample;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += CopySamples_Accepting;
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
        private void CopySamples_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (objSLInfo.JobID != string.Empty)
                {
                    if (objCopySampleInfo.NoOfSamples > 0)
                    {
                        objCopySampleInfo.Msgflag = false;
                        bool DBAccess = false;
                        string JobID = string.Empty;
                        int SampleNo = 0;
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        Session currentSession = ((XPObjectSpace)(objectSpace)).Session;
                        UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                        List<Guid> lstnewSamples = new List<Guid>();
                        if (View != null && View.CurrentObject != null && View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                        {
                            SampleLogIn objSLOld = (SampleLogIn)View.CurrentObject;
                            objSLOld = uow.GetObjectByKey<SampleLogIn>(objSLOld.Oid);
                            List<SampleBottleAllocation> smplold = uow.Query<SampleBottleAllocation>().Where(i => i.SampleRegistration != null && i.SampleRegistration.Oid == objSLOld.Oid).ToList();
                            VisualMatrix visualMatrix = null;
                            Collector objcollector = null;
                            Samplecheckin objJobId = uow.GetObjectByKey<Samplecheckin>(objSLOld.JobID.Oid);
                            if (objSLOld.VisualMatrix != null)
                            {
                                visualMatrix = uow.GetObjectByKey<VisualMatrix>(objSLOld.VisualMatrix.Oid);
                            }
                            if (objSLOld.Collector != null)
                            {
                                objcollector = uow.GetObjectByKey<Collector>(objSLOld.Collector.Oid);
                            }
                            for (int i = 1; i <= objCopySampleInfo.NoOfSamples; i++)
                            {
                                SampleLogIn objSLNew = new SampleLogIn(uow);
                                lstnewSamples.Add(objSLNew.Oid);
                                objSLNew.JobID = objJobId;
                                objSLNew.ExcludeInvoice = false;
                                if (DBAccess == false)
                                {
                                    SelectedData sproc = currentSession.ExecuteSproc("GetSampleID", new OperandValue(objSLNew.JobID.ToString()));
                                    if (sproc.ResultSet[1].Rows[0].Values[0] != null)
                                    {
                                        objSLInfo.SampleID = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                                        SampleNo = Convert.ToInt32(objSLInfo.SampleID);
                                        DBAccess = true;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                objSLInfo.boolCopySamples = true;
                                objSLNew.SampleNo = SampleNo;
                                //objSLNew.ClientSampleID = objSLOld.ClientSampleID;
                                objSLNew.Test = true;
                                //objSLNew.VisualMatrix = objSLOld.VisualMatrix;
                                if (visualMatrix != null)
                                {
                                    objSLNew.VisualMatrix = visualMatrix;
                                }
                                //objSLNew.SampleType = objSLOld.SampleType;
                                if (objSLOld.SampleType != null)
                                {
                                    objSLNew.SampleType = uow.GetObjectByKey<SampleType>(objSLOld.SampleType.Oid);
                                }
                                if (objSLOld.SampleStatus != null)
                                {
                                    objSLNew.SampleStatus = uow.GetObjectByKey<SampleStatus>(objSLOld.SampleStatus.Oid);
                                }
                                objSLNew.Qty = objSLOld.Qty;
                                //objSLNew.Storage = objSLOld.Storage;
                                if (objSLOld.Storage != null)
                                {
                                    objSLNew.Storage = uow.GetObjectByKey<Storage>(objSLOld.Storage.Oid);
                                }
                                objSLNew.Preservetives = objSLOld.Preservetives;
                                objSLNew.SamplingLocation = objSLOld.SamplingLocation;
                                //objSLNew.QCType = objSLOld.QCType;
                                if (objSLOld.QCType != null)
                                {
                                    objSLNew.QCType = uow.GetObjectByKey<QCType>(objSLOld.QCType.Oid);
                                }
                                //objSLNew.QCSource = objSLOld.QCSource;
                                if (objSLOld.QCSource != null)
                                {
                                    objSLNew.QCSource = uow.GetObjectByKey<SampleLogIn>(objSLOld.QCSource.Oid);
                                }
                                //objSLNew.Collector = objSLOld.Collector;
                                if (objcollector != null)
                                {
                                    //objSLNew.Collector = uow.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(objSLOld.Collector.Oid);
                                    objSLNew.Collector = objcollector;
                                }
                                if (objSLOld.Client != null)
                                {
                                    objSLNew.Client = uow.GetObjectByKey<Customer>(objSLOld.Client.Oid);
                                }
                                if (objSLOld.Department != null)
                                {

                                    objSLNew.Department = uow.GetObjectByKey<Department>(objSLOld.Department);
                                }
                                if (objSLOld.ProjectID != null)
                                {

                                    objSLNew.ProjectID = uow.GetObjectByKey<Project>(objSLOld.ProjectID.Oid);
                                }
                                if (objSLOld.PreserveCondition != null)
                                {

                                    objSLNew.PreserveCondition = uow.GetObjectByKey<PreserveCondition>(objSLOld.PreserveCondition);
                                }
                                if (objSLOld.StorageID != null)
                                {

                                    objSLNew.StorageID = uow.GetObjectByKey<Storage>(objSLOld.StorageID);
                                }
                                objSLNew.CollectDate = objSLOld.CollectDate;
                                objSLNew.CollectTime = objSLOld.CollectTime;
                                objSLNew.FlowRate = objSLOld.FlowRate;
                                objSLNew.TimeStart = objSLOld.TimeStart;
                                objSLNew.TimeEnd = objSLOld.TimeEnd;
                                objSLNew.Time = objSLOld.Time;
                                objSLNew.Volume = objSLOld.Volume;
                                objSLNew.Address = objSLOld.Address;
                                objSLNew.AreaOrPerson = objSLOld.AreaOrPerson;
                                if (objSLOld.BalanceID != null)
                                {
                                    objSLNew.BalanceID = uow.GetObjectByKey<Labware>(objSLOld.BalanceID.Oid);
                                }
                                objSLNew.AssignTo = objSLOld.AssignTo;
                                objSLNew.Barp = objSLOld.Barp;
                                objSLNew.BatchID = objSLOld.BatchID;
                                objSLNew.BatchSize = objSLOld.BatchSize;
                                objSLNew.BatchSize_pc = objSLOld.BatchSize_pc;
                                objSLNew.BatchSize_Units = objSLOld.BatchSize_Units;
                                objSLNew.Blended = objSLOld.Blended;
                                objSLNew.BottleQty = objSLOld.BottleQty;
                                objSLNew.ClientSampleID = "Sample" + SampleNo.ToString();
                                objSLNew.TestSummary = objSLOld.TestSummary;
                                objSLNew.FieldTestSummary = objSLOld.FieldTestSummary;
                                objSLNew.BuriedDepthOfGroundWater = objSLOld.BuriedDepthOfGroundWater;
                                objSLNew.ChlorineFree = objSLOld.ChlorineFree;
                                objSLNew.ChlorineTotal = objSLOld.ChlorineTotal;
                                objSLNew.City = objSLOld.City;
                                objSLNew.CollectorPhone = objSLOld.CollectorPhone;
                                //objSLNew.CollectTimeDisplay = objSLOld.CollectTimeDisplay;
                                objSLNew.CompositeQty = objSLOld.CompositeQty;
                                objSLNew.DateEndExpected = objSLOld.DateEndExpected;
                                objSLNew.DateStartExpected = objSLOld.DateStartExpected;
                                objSLNew.Depth = objSLOld.Depth;
                                objSLNew.Description = objSLOld.Description;
                                objSLNew.DischargeFlow = objSLOld.DischargeFlow;
                                objSLNew.DischargePipeHeight = objSLOld.DischargePipeHeight;
                                objSLNew.DO = objSLOld.DO;
                                objSLNew.DueDate = objSLOld.DueDate;
                                objSLNew.Emission = objSLOld.Emission;
                                objSLNew.EndOfRoad = objSLOld.EndOfRoad;
                                objSLNew.EquipmentModel = objSLOld.EquipmentModel;
                                objSLNew.EquipmentName = objSLOld.EquipmentName;
                                objSLNew.FacilityID = objSLOld.FacilityID;
                                objSLNew.FacilityName = objSLOld.FacilityName;
                                objSLNew.FacilityType = objSLOld.FacilityType;
                                objSLNew.FinalForm = objSLOld.FinalForm;
                                objSLNew.FinalPackaging = objSLOld.FinalPackaging;
                                objSLNew.FlowRate = objSLOld.FlowRate;
                                objSLNew.FlowRateCubicMeterPerHour = objSLOld.FlowRateCubicMeterPerHour;
                                objSLNew.FlowRateLiterPerMin = objSLOld.FlowRateLiterPerMin;
                                objSLNew.FlowVelocity = objSLOld.FlowVelocity;
                                objSLNew.ForeignMaterial = objSLOld.ForeignMaterial;
                                objSLNew.Frequency = objSLOld.Frequency;
                                objSLNew.GISStatus = objSLOld.GISStatus;
                                objSLNew.GravelContent = objSLOld.GravelContent;
                                objSLNew.GrossWeight = objSLOld.GrossWeight;
                                objSLNew.GroupSample = objSLOld.GroupSample;
                                objSLNew.Hold = objSLOld.Hold;
                                objSLNew.Humidity = objSLOld.Humidity;
                                objSLNew.IceCycle = objSLOld.IceCycle;
                                objSLNew.Increments = objSLOld.Increments;
                                objSLNew.Interval = objSLOld.Interval;
                                objSLNew.IsActive = objSLOld.IsActive;
                                //objSLNew.IsNotTransferred = objSLOld.IsNotTransferred;
                                objSLNew.ItemName = objSLOld.ItemName;
                                objSLNew.KeyMap = objSLOld.KeyMap;
                                objSLNew.LicenseNumber = objSLOld.LicenseNumber;
                                objSLNew.ManifestNo = objSLOld.ManifestNo;
                                objSLNew.MonitoryingRequirement = objSLOld.MonitoryingRequirement;
                                objSLNew.NoOfCollectionsEachTime = objSLOld.NoOfCollectionsEachTime;
                                objSLNew.NoOfPoints = objSLOld.NoOfPoints;
                                objSLNew.Notes = objSLOld.Notes;
                                objSLNew.OriginatingEntiry = objSLOld.OriginatingEntiry;
                                objSLNew.OriginatingLicenseNumber = objSLOld.OriginatingLicenseNumber;
                                objSLNew.PackageNumber = objSLOld.PackageNumber;
                                objSLNew.ParentSampleDate = objSLOld.ParentSampleDate;
                                objSLNew.ParentSampleID = objSLOld.ParentSampleID;
                                objSLNew.PiecesPerUnit = objSLOld.PiecesPerUnit;
                                objSLNew.Preservetives = objSLOld.Preservetives;
                                objSLNew.ProjectName = objSLOld.ProjectName;
                                objSLNew.PurifierSampleID = objSLOld.PurifierSampleID;
                                objSLNew.PWSID = objSLOld.PWSID;
                                if (objSLOld.PWSSystemName!=null)
                                {
                                    objSLNew.PWSSystemName =uow.GetObjectByKey<PWSSystem>(objSLOld.PWSSystemName.Oid); 
                                }
                                objSLNew.RegionNameOfSection = objSLOld.RegionNameOfSection;
                                objSLNew.RejectionCriteria = objSLOld.RejectionCriteria;
                                objSLNew.RepeatLocation = objSLOld.RepeatLocation;
                                objSLNew.RetainedWeight = objSLOld.RetainedWeight;
                                objSLNew.RiverWidth = objSLOld.RiverWidth;
                                objSLNew.RushSample = objSLOld.RushSample;
                                objSLNew.SampleAmount = objSLOld.SampleAmount;
                                objSLNew.SampleCondition = objSLOld.SampleCondition;
                                objSLNew.SampleDescription = objSLOld.SampleDescription;
                                objSLNew.SampleImage = objSLOld.SampleImage;
                               // objSLNew.SampleName = objSLOld.SampleName;
                                objSLNew.SamplePointID = objSLOld.SamplePointID;
                                objSLNew.SamplePointType = objSLOld.SamplePointType;
                                if (!objSLOld.IsReanalysis)
                                {
                                    objSLNew.SampleSource = objSLOld.SampleSource;
                                }
                                objSLNew.SampleTag = objSLOld.SampleTag;
                                objSLNew.SampleWeight = objSLOld.SampleWeight;
                                objSLNew.SamplingAddress = objSLOld.SamplingAddress;
                                objSLNew.SamplingEquipment = objSLOld.SamplingEquipment;
                                objSLNew.SamplingLocation = objSLOld.SamplingLocation;
                                objSLNew.SamplingProcedure = objSLOld.SamplingProcedure;
                                objSLNew.SequenceTestSampleID = objSLOld.SequenceTestSampleID;
                                objSLNew.SequenceTestSortNo = objSLOld.SequenceTestSortNo;
                                objSLNew.ServiceArea = objSLOld.ServiceArea;
                                objSLNew.SiteCode = objSLOld.SiteCode;
                                objSLNew.SiteDescription = objSLOld.SiteDescription;
                                objSLNew.SiteID = objSLOld.SiteID;
                                objSLNew.SiteNameArchived = objSLOld.SiteNameArchived;
                                objSLNew.SiteUserDefinedColumn1 = objSLOld.SiteUserDefinedColumn1;
                                objSLNew.SiteUserDefinedColumn2 = objSLOld.SiteUserDefinedColumn2;
                                objSLNew.SiteUserDefinedColumn3 = objSLOld.SiteUserDefinedColumn3;
                                objSLNew.SubOut = objSLOld.SubOut;
                                if (objSLOld.SystemType!=null)
                                {
                                    objSLNew.SystemType = uow.GetObjectByKey<SystemTypes>(objSLOld.SystemType.Oid);
                                }
                                objSLNew.TargetMGTHC_CBD_mg_pc = objSLOld.TargetMGTHC_CBD_mg_pc;
                                objSLNew.TargetMGTHC_CBD_mg_unit = objSLOld.TargetMGTHC_CBD_mg_unit;
                                objSLNew.TargetPotency = objSLOld.TargetPotency;
                                objSLNew.TargetUnitWeight_g_pc = objSLOld.TargetUnitWeight_g_pc;
                                objSLNew.TargetUnitWeight_g_unit = objSLOld.TargetUnitWeight_g_unit;
                                objSLNew.TargetWeight = objSLOld.TargetWeight;
                                objSLNew.Time = objSLOld.Time;
                                objSLNew.TimeEnd = objSLOld.TimeEnd;
                                objSLNew.TimeStart = objSLOld.TimeStart;
                                objSLNew.TotalSamples = objSLOld.TotalSamples;
                                objSLNew.TotalTimes = objSLOld.TotalTimes;
                                objSLNew.ExcludeInvoice = objSLOld.ExcludeInvoice;
                                if (objSLOld.QCCategory!=null)
                                {
                                    objSLNew.QCCategory = uow.GetObjectByKey<QCCategory>(objSLOld.QCCategory.Oid); 
                                }
                                if (objSLOld.TtimeUnit != null)
                                {
                                    objSLNew.TtimeUnit = uow.GetObjectByKey<Unit>(objSLOld.TtimeUnit.Oid);
                                }
                                if (objSLOld.WaterType!=null)
                                {
                                    objSLNew.WaterType = uow.GetObjectByKey<WaterTypes>(objSLOld.WaterType.Oid); 
                                }
                                objSLNew.ZipCode = objSLOld.ZipCode;
                                //objSLNew.ModifiedBy = objSLOld.ModifiedBy;
                                if (objSLOld.ModifiedBy != null)
                                {
                                    objSLNew.ModifiedBy = uow.GetObjectByKey<Modules.BusinessObjects.Hr.CustomSystemUser>(objSLOld.ModifiedBy.Oid);
                                }
                                objSLNew.ModifiedDate = objSLOld.ModifiedDate;
                                objSLNew.Comment = objSLOld.Comment;
                                objSLNew.Latitude = objSLOld.Latitude;
                                objSLNew.Longitude = objSLOld.Longitude;

                                objSLNew.Remark = objSLOld.Remark;
                                objSLNew.ProjectNumber = objSLOld.ProjectNumber;
                                objSLNew.WaterSource = objSLOld.WaterSource;
                                objSLNew.PreviousSample = objSLOld.PreviousSample;
                                objSLNew.PreviousCollection = objSLOld.PreviousCollection;
                                objSLNew.SamplingEvent = objSLOld.SamplingEvent;
                                objSLNew.Type = objSLOld.Type;


                                if (objSLOld.StationLocation != null)
                                {
                                    objSLNew.StationLocation = uow.GetObjectByKey<SampleSites>(objSLOld.StationLocation.Oid);
                                }
                                objSLNew.AlternativeStation = objSLOld.AlternativeStation;
                                objSLNew.AlternativeStationOid = objSLOld.AlternativeStationOid;
                                List<Testparameter> lsttp = uow.Query<Testparameter>().Where(j => j.QCType.QCTypeName == "Sample" && j.SampleLogIns.Where(a => a.Oid == objSLOld.Oid).Count() > 0).ToList();
                                foreach (var objLineA in lsttp)
                                {
                                    //objSLNew.Testparameters.Add(objLineA);
                                    objSLNew.Testparameters.Add(uow.GetObjectByKey<Testparameter>(objLineA.Oid));
                                }
                                foreach (var objSampleparameter in objSLOld.SampleParameter.Where(a => a.IsGroup == true && a.GroupTest != null).ToList())
                                {
                                    SampleParameter sample = objSLNew.SampleParameter.FirstOrDefault<SampleParameter>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                                    if (objSampleparameter.GroupTest != null && sample != null)
                                    {
                                        sample.IsGroup = true;
                                        sample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objSampleparameter.GroupTest.Oid);
                                        //sample.TestGroup = uow.GetObjectByKey<GroupTest>(objSampleparameter.TestGroup.Oid);
                                    }
                                }
                                foreach (var objSampleparameter in objSLOld.SampleParameter.Where(a => a.TestHold == true).ToList())
                                {
                                    SampleParameter sample = objSLNew.SampleParameter.FirstOrDefault<SampleParameter>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                                    if (sample != null)
                                    {
                                        sample.TestHold = true;
                                    }
                                }
                                foreach (var objSampleparameter in objSLOld.SampleParameter.Where(a => a.SubOut == true).ToList())
                                {
                                    SampleParameter sample = objSLNew.SampleParameter.FirstOrDefault<SampleParameter>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                                    if (sample != null)
                                    {
                                        sample.SubOut = true;
                                    }
                                }
                                DefaultSetting objNavigationView = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='RegistrationSigningOff'"));
                                if (objNavigationView != null && objNavigationView.Select)
                                {
                                    if (objSLNew.JobID.Status == SampleRegistrationSignoffStatus.Signedoff)
                                    {
                                        objSLNew.JobID.Status = SampleRegistrationSignoffStatus.PartiallySignedOff;
                                    }
                                }
                                else
                                {

                                }
                                objSLNew.Save();
                                SampleNo++;
                                if (smplold != null && smplold.Count > 0)
                                {
                                    foreach (SampleBottleAllocation smpl in smplold.ToList())
                                    {
                                        SampleBottleAllocation smplnew = new SampleBottleAllocation(uow);
                                        smplnew.SampleRegistration = objSLNew;
                                        smplnew.TestMethod = uow.GetObjectByKey<TestMethod>(smpl.TestMethod.Oid);
                                        smplnew.BottleID = smpl.BottleID;
                                        if (smpl.Containers != null)
                                        {
                                            smplnew.Containers = uow.GetObjectByKey<Container>(smpl.Containers.Oid);
                                        }
                                        if (smpl.Preservative != null)
                                        {
                                            smplnew.Preservative = uow.GetObjectByKey<Preservative>(smpl.Preservative.Oid);
                                        }
                                        if (smpl.StorageID != null)
                                        {
                                            smplnew.StorageID = uow.GetObjectByKey<Storage>(smpl.StorageID.Oid);
                                        }
                                        if (smpl.StorageCondition != null)
                                        {
                                            smplnew.StorageCondition = uow.GetObjectByKey<PreserveCondition>(smpl.StorageCondition.Oid);
                                        }
                                    }
                                }

                                if (objSLNew.JobID.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                                {
                                    if (objSLNew.JobID.IsSampling)
                                    {
                                        //IList<SampleParameter> parameters = uow.GetObjects(uow.GetClassInfo(typeof(SampleParameter)), CriteriaOperator.Parse("[Samplelogin.Oid]=?", objSLOld.Oid), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                                        //if (parameters.Where(a => a.Testparameter != null && a.Testparameter.TestMethod != null && a.Testparameter.TestMethod.IsFieldTest == true).Count() > 0)
                                        //{
                                            Frame.GetController<FlutterAppViewController>().insertsample(uow, objSLNew);
                                        //}
                                    }
                                    Frame.GetController<AuditlogViewController>().insertauditdata(uow, objSLNew.JobID.Oid, OperationType.Created, "Sample Registration", objSLNew.JobID.JobID, "Samples", "", objSLNew.SampleID, "");
                                }
                                //Frame.GetController<SampleRegistrationViewController>().AssignBottlesToSamples(uow, objSLOld.JobID.JobID, objSLNew.Oid);
                            }
                            //Application.MainWindow.GetController<RegistrationSignOffController>().PendingSigningOffJobIDCount();
                            //Application.MainWindow.GetController<SampleRegistrationViewController>().SuboutCount();
                            //uow.CommitChanges();
                            uow.CommitChanges();
                            if (lstnewSamples != null && lstnewSamples.Count > 0 && objJobId.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                            {
                                if (objJobId != null && objJobId.ProjectCategory != null && (objJobId.ProjectCategory.CategoryName == "PT" || objJobId.ProjectCategory.CategoryName == "DOC" || objJobId.ProjectCategory.CategoryName == "MDL"))
                                {
                                    foreach (Guid objSampleLogInNew in lstnewSamples.ToList())
                                    {
                                        PTStudyLog Objstudylog = uow.FindObject<PTStudyLog>(CriteriaOperator.Parse("[SampleCheckinJobID.JobID]= ?", objJobId.JobID));
                                        if (Objstudylog == null)
                                        {
                                            PTStudyLog objPT = new PTStudyLog(uow);
                                            objPT.JobID = objJobId.JobID;
                                            objPT.DatePTSampleReceived = objJobId.RecievedDate;
                                            objPT.SampleCheckinJobID = objJobId;
                                            objPT.Category = objJobId.ProjectCategory.CategoryName;
                                            objPT.Save();
                                            uow.CommitChanges();
                                            XPClassInfo sampleParameterinfo;
                                            sampleParameterinfo = uow.GetClassInfo(typeof(SampleParameter));
                                            IList<SampleParameter> lstSampleParam = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samplelogin.Oid]=?", objSampleLogInNew), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                                            foreach (SampleParameter objParam in lstSampleParam)
                                            {
                                                PTStudyLogResults objPTRes = new PTStudyLogResults(uow);
                                                SampleParameter objParameter = uow.GetObjectByKey<SampleParameter>(objParam.Oid);
                                                objPTRes.PTStudyLog = objPT;
                                                objPTRes.SampleID = objParameter;
                                                if (objParam.Samplelogin != null)
                                                {
                                                    objPTRes.SampleLogin = uow.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(objParam.Samplelogin.Oid);
                                                }
                                                objPTRes.Save();
                                            }
                                            uow.CommitChanges();
                                        }
                                        else
                                        {
                                            Objstudylog.JobID = objJobId.JobID;
                                            Objstudylog.DatePTSampleReceived = objJobId.RecievedDate;
                                            Objstudylog.SampleCheckinJobID = objJobId;
                                            Objstudylog.Category = objJobId.ProjectCategory.CategoryName;
                                            uow.CommitChanges();
                                            XPClassInfo sampleParameterinfo;
                                            sampleParameterinfo = uow.GetClassInfo(typeof(SampleParameter));
                                            IList<SampleParameter> lstSampleParam = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samplelogin.Oid]=?", objSampleLogInNew), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                                            foreach (SampleParameter objParam in lstSampleParam)
                                            {
                                                PTStudyLogResults objPTRes = new PTStudyLogResults(uow);
                                                SampleParameter objParameter = uow.GetObjectByKey<SampleParameter>(objParam.Oid);
                                                objPTRes.PTStudyLog = Objstudylog;
                                                objPTRes.SampleID = objParameter;
                                                if (objParam.Samplelogin != null)
                                                {
                                                    objPTRes.SampleLogin = uow.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(objParam.Samplelogin.Oid);
                                                }
                                                objPTRes.Save();
                                            }
                                            uow.CommitChanges();
                                        }
                                    }
                                }
                            }
                        }
                        SelectedData updateStatusProc = currentSession.ExecuteSproc("StatusUpdate_SP");
                        //ObjectSpace.Refresh();                        
                        objCopySampleInfo.NoOfSamples = 0;
                        objSLInfo.boolCopySamples = false;
                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            CompositeView view = nestedFrame.ViewItem.View;
                            foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                            {
                                if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                                {
                                    //frameContainer.Frame.View.ObjectSpace.Refresh();
                                    if (frameContainer.Frame.View is DetailView)
                                    {
                                        frameContainer.Frame.View.ObjectSpace.ReloadObject(frameContainer.Frame.View.CurrentObject);
                                    }
                                    else
                                    {
                                        (frameContainer.Frame.View as DevExpress.ExpressApp.ListView).CollectionSource.Reload();
                                    }
                                    frameContainer.Frame.View.Refresh();
                                }
                            }
                        }
                        View.Refresh();
                        View.RefreshDataSource();
                        Frame.GetController<SampleRegistrationViewController>().UpdateStatusInJobID();
                        //objectSpace.Dispose();
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

        private void CopySamples_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(SL_CopyNoOfSamples));
                if (objToShow != null)
                {
                    DetailView CreateDetailView = Application.CreateDetailView(objspace, objToShow);
                    CreateDetailView.ViewEditMode = ViewEditMode.Edit;
                    e.View = CreateDetailView;
                }
                if (View.SelectedObjects.Count == 0)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    e.Action.RaiseCancel();
                }
                else if (View.SelectedObjects.Count > 1)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlyonechkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    e.Action.RaiseCancel();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void View_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                {
                    if (View.CurrentObject != null)
                    {
                        SampleLogIn sl = (SampleLogIn)View.CurrentObject;
                        if (sl.VisualMatrix != null)
                        {
                            objSLInfo.SLVisualMatrixName = sl.VisualMatrix.MatrixName.MatrixName;
                        }
                        if (sl.JobID != null)
                        {
                            objSLInfo.JobID = View.ObjectSpace.GetKeyValue(View.CurrentObject).ToString();
                            objSLInfo.focusedJobID = sl.JobID.JobID;
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

        private void SaveAndNewAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                CopySamples.Enabled["enable"] = true;
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAndCloseAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                CopySamples.Enabled["enable"] = true;
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Save_SampleLogin(object sender, SimpleActionExecuteEventArgs e)
        {
           try
            {
                if (((ListView)View).CollectionSource.List.Cast<SampleLogIn>().FirstOrDefault(i => i.StationLocation == null && i.JobID.IsSampling) != null)
                {
                    Application.ShowViewStrategy.ShowMessage("Enter the station location.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    return;
                }
                else
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        gridListEditor.Grid.UpdateEdit();
                        if (View.ObjectSpace.ModifiedObjects.Count > 0)
                        {
                            View.ObjectSpace.CommitChanges();
                            View.ObjectSpace.Refresh();
                        }
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                CopySamples.Enabled["enable"] = true;
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Btn_Add_Collector_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SampleLogIn_ListView_Copy_SampleRegistration")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //Collector objAttachment = objectSpace.GetObject((Collector)View.CurrentObject);
                    Collector objCollector = objectSpace.CreateObject<Collector>();
                    CriteriaOperator cs = CriteriaOperator.Parse("JobID=?", SRInfo.strJobID);
                    Samplecheckin objSamplecheckin = objectSpace.FindObject<Samplecheckin>(cs);
                    if (objSamplecheckin != null)
                    {
                        objCollector.CustomerName = objSamplecheckin.ClientName;
                    }
                    DetailView dv = Application.CreateDetailView(objectSpace, "Collector_DetailView_SampleLogin", true, objCollector);
                    dv.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters();
                    showViewParameters.CreatedView = dv;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    //dc.ViewClosed += Dc_ViewClosedCollector;
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

        private void Dc_ViewClosedCollector(object sender, EventArgs e)
        {
            try
            {
                Frame.GetController<RefreshController>().RefreshAction.DoExecute();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Reanalysis_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                CriteriaOperator cs = CriteriaOperator.Parse("JobID=?", SRInfo.strJobID);
                Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                Modules.BusinessObjects.SampleManagement.SampleLogIn objCurrent = (Modules.BusinessObjects.SampleManagement.SampleLogIn)View.CurrentObject;
                Modules.BusinessObjects.SampleManagement.SampleLogIn objSLOld = uow.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(objCurrent.Oid); ;
                Modules.BusinessObjects.SampleManagement.Samplecheckin objsamplecheckin = uow.FindObject<Modules.BusinessObjects.SampleManagement.Samplecheckin>(cs);
                Modules.BusinessObjects.SampleManagement.SampleLogIn objSLNew = new Modules.BusinessObjects.SampleManagement.SampleLogIn(uow);
                objSLNew.JobID = objsamplecheckin;
                objSLNew.IsReanalysis = true;
                List<SampleBottleAllocation> smplold = uow.Query<SampleBottleAllocation>().Where(i => i.SampleRegistration != null && i.SampleRegistration.Oid == objSLOld.Oid).ToList();
                int sampleno = 0;
                SelectedData sproc = currentSession.ExecuteSproc("GetSampleID", new OperandValue(objsamplecheckin.JobID));
                if (sproc.ResultSet[1].Rows[0].Values[0].ToString() != null)
                {
                    sampleno = (int)sproc.ResultSet[1].Rows[0].Values[0];
                }
                objSLNew.SampleNo = sampleno;
                int sysSampleno = 0;
                if (objCurrent != null && !objCurrent.IsReanalysis)
                {
                    sysSampleno = uow.Query<SampleLogIn>().Where(i => i.SampleSource != null && i.SampleSource == objCurrent.SampleID).ToList().Count();
                    objSLNew.SampleSource = objSLOld.SampleID;
                }
                else
                {
                    sysSampleno = uow.Query<SampleLogIn>().Where(i => i.SampleSource != null && i.SampleSource == objCurrent.SampleSource).ToList().Count();
                    objSLNew.SampleSource = objSLOld.SampleSource;
                }
                sysSampleno++;
                objSLNew.SysSampleCode = objSLOld.SampleID + "LR" + sysSampleno;
                objSLNew.ExcludeInvoice = true;
                if (objSLOld.VisualMatrix != null)
                {
                    objSLNew.VisualMatrix = uow.GetObjectByKey<VisualMatrix>(objSLOld.VisualMatrix.Oid); ;
                }
                if (objSLOld.SampleType != null)
                {
                    objSLNew.SampleType = uow.GetObjectByKey<SampleType>(objSLOld.SampleType.Oid);
                }
                objSLNew.Qty = objSLOld.Qty;
                //objSLNew.Storage = objSLOld.Storage;
                if (objSLOld.Storage != null)
                {
                    objSLNew.Storage = uow.GetObjectByKey<Storage>(objSLOld.Storage.Oid);
                }
                objSLNew.Preservetives = objSLOld.Preservetives;
                objSLNew.SamplingLocation = objSLOld.SamplingLocation;
                //objSLNew.QCType = objSLOld.QCType;
                if (objSLOld.QCType != null)
                {
                    objSLNew.QCType = uow.GetObjectByKey<QCType>(objSLOld.QCType.Oid);
                }
                //objSLNew.QCSource = objSLOld.QCSource;
                if (objSLOld.QCSource != null)
                {
                    objSLNew.QCSource = uow.GetObjectByKey<SampleLogIn>(objSLOld.QCSource.Oid);
                }
                if (objSLOld.Collector != null)
                {
                    objSLNew.Collector = uow.GetObjectByKey<Collector>(objSLOld.Collector.Oid);
                }
                if (objSLOld.Client != null)
                {
                    objSLNew.Client = uow.GetObjectByKey<Customer>(objSLOld.Client.Oid);
                }
                if (objSLOld.Department != null)
                {

                    objSLNew.Department = uow.GetObjectByKey<Department>(objSLOld.Department);
                }
                if (objSLOld.ProjectID != null)
                {

                    objSLNew.ProjectID = uow.GetObjectByKey<Project>(objSLOld.ProjectID.Oid);
                }
                if (objSLOld.PreserveCondition != null)
                {

                    objSLNew.PreserveCondition = uow.GetObjectByKey<PreserveCondition>(objSLOld.PreserveCondition);
                }
                if (objSLOld.StorageID != null)
                {

                    objSLNew.StorageID = uow.GetObjectByKey<Storage>(objSLOld.StorageID);
                }
                objSLNew.CollectDate = objSLOld.CollectDate;
                objSLNew.CollectTime = objSLOld.CollectTime;
                objSLNew.FlowRate = objSLOld.FlowRate;
                objSLNew.TimeStart = objSLOld.TimeStart;
                objSLNew.TimeEnd = objSLOld.TimeEnd;
                objSLNew.Time = objSLOld.Time;
                objSLNew.Volume = objSLOld.Volume;
                objSLNew.Address = objSLOld.Address;
                objSLNew.AreaOrPerson = objSLOld.AreaOrPerson;
                if (objSLOld.BalanceID != null)
                {
                    objSLNew.BalanceID = uow.GetObjectByKey<Labware>(objSLOld.BalanceID.Oid);
                }
                objSLNew.AssignTo = objSLOld.AssignTo;
                objSLNew.Barp = objSLOld.Barp;
                objSLNew.BatchID = objSLOld.BatchID;
                objSLNew.BatchSize = objSLOld.BatchSize;
                objSLNew.BatchSize_pc = objSLOld.BatchSize_pc;
                objSLNew.BatchSize_Units = objSLOld.BatchSize_Units;
                objSLNew.Blended = objSLOld.Blended;
                objSLNew.BottleQty = objSLOld.BottleQty;
                objSLNew.BuriedDepthOfGroundWater = objSLOld.BuriedDepthOfGroundWater;
                objSLNew.ChlorineFree = objSLOld.ChlorineFree;
                objSLNew.ChlorineTotal = objSLOld.ChlorineTotal;
                objSLNew.City = objSLOld.City;
                objSLNew.CollectorPhone = objSLOld.CollectorPhone;
                objSLNew.ClientSampleID = "Sample" + sampleno.ToString();
                objSLNew.TestSummary = objSLOld.TestSummary;
                objSLNew.FieldTestSummary = objSLOld.FieldTestSummary;
                //objSLNew.CollectTimeDisplay = objSLOld.CollectTimeDisplay;
                objSLNew.CompositeQty = objSLOld.CompositeQty;
                objSLNew.DateEndExpected = objSLOld.DateEndExpected;
                objSLNew.DateStartExpected = objSLOld.DateStartExpected;
                objSLNew.Depth = objSLOld.Depth;
                objSLNew.Description = objSLOld.Description;
                objSLNew.DischargeFlow = objSLOld.DischargeFlow;
                objSLNew.DischargePipeHeight = objSLOld.DischargePipeHeight;
                objSLNew.DO = objSLOld.DO;
                objSLNew.DueDate = objSLOld.DueDate;
                objSLNew.Emission = objSLOld.Emission;
                objSLNew.EndOfRoad = objSLOld.EndOfRoad;
                objSLNew.EquipmentModel = objSLOld.EquipmentModel;
                objSLNew.EquipmentName = objSLOld.EquipmentName;
                objSLNew.FacilityID = objSLOld.FacilityID;
                objSLNew.FacilityName = objSLOld.FacilityName;
                objSLNew.FacilityType = objSLOld.FacilityType;
                objSLNew.FinalForm = objSLOld.FinalForm;
                objSLNew.FinalPackaging = objSLOld.FinalPackaging;
                objSLNew.FlowRate = objSLOld.FlowRate;
                objSLNew.FlowRateCubicMeterPerHour = objSLOld.FlowRateCubicMeterPerHour;
                objSLNew.FlowRateLiterPerMin = objSLOld.FlowRateLiterPerMin;
                objSLNew.FlowVelocity = objSLOld.FlowVelocity;
                objSLNew.ForeignMaterial = objSLOld.ForeignMaterial;
                objSLNew.Frequency = objSLOld.Frequency;
                objSLNew.GISStatus = objSLOld.GISStatus;
                objSLNew.GravelContent = objSLOld.GravelContent;
                objSLNew.GrossWeight = objSLOld.GrossWeight;
                objSLNew.GroupSample = objSLOld.GroupSample;
                objSLNew.Hold = objSLOld.Hold;
                objSLNew.Humidity = objSLOld.Humidity;
                objSLNew.IceCycle = objSLOld.IceCycle;
                objSLNew.Increments = objSLOld.Increments;
                objSLNew.Interval = objSLOld.Interval;
                objSLNew.IsActive = objSLOld.IsActive;
                //objSLNew.IsNotTransferred = objSLOld.IsNotTransferred;
                objSLNew.ItemName = objSLOld.ItemName;
                objSLNew.KeyMap = objSLOld.KeyMap;
                objSLNew.LicenseNumber = objSLOld.LicenseNumber;
                objSLNew.ManifestNo = objSLOld.ManifestNo;
                objSLNew.MonitoryingRequirement = objSLOld.MonitoryingRequirement;
                objSLNew.NoOfCollectionsEachTime = objSLOld.NoOfCollectionsEachTime;
                objSLNew.NoOfPoints = objSLOld.NoOfPoints;
                objSLNew.Notes = objSLOld.Notes;
                objSLNew.OriginatingEntiry = objSLOld.OriginatingEntiry;
                objSLNew.OriginatingLicenseNumber = objSLOld.OriginatingLicenseNumber;
                objSLNew.PackageNumber = objSLOld.PackageNumber;
                objSLNew.ParentSampleDate = objSLOld.ParentSampleDate;
                objSLNew.ParentSampleID = objSLOld.ParentSampleID;
                objSLNew.PiecesPerUnit = objSLOld.PiecesPerUnit;
                objSLNew.Preservetives = objSLOld.Preservetives;
                objSLNew.ProjectName = objSLOld.ProjectName;
                objSLNew.PurifierSampleID = objSLOld.PurifierSampleID;
                objSLNew.PWSID = objSLOld.PWSID;
                if (objSLOld.PWSSystemName!=null)
                {
                    objSLNew.PWSSystemName =uow.GetObjectByKey<PWSSystem>(objSLOld.PWSSystemName.Oid); 
                }
                objSLNew.RegionNameOfSection = objSLOld.RegionNameOfSection;
                objSLNew.RejectionCriteria = objSLOld.RejectionCriteria;
                objSLNew.RepeatLocation = objSLOld.RepeatLocation;
                objSLNew.RetainedWeight = objSLOld.RetainedWeight;
                objSLNew.RiverWidth = objSLOld.RiverWidth;
                objSLNew.RushSample = objSLOld.RushSample;
                objSLNew.SampleAmount = objSLOld.SampleAmount;
                objSLNew.SampleCondition = objSLOld.SampleCondition;
                objSLNew.SampleDescription = objSLOld.SampleDescription;
                objSLNew.SampleImage = objSLOld.SampleImage;
                //objSLNew.SampleName = objSLOld.SampleName;
                objSLNew.SamplePointID = objSLOld.SamplePointID;
                objSLNew.SamplePointType = objSLOld.SamplePointType;
                objSLNew.SampleTag = objSLOld.SampleTag;
                objSLNew.SampleWeight = objSLOld.SampleWeight;
                objSLNew.SamplingAddress = objSLOld.SamplingAddress;
                objSLNew.SamplingEquipment = objSLOld.SamplingEquipment;
                objSLNew.SamplingLocation = objSLOld.SamplingLocation;
                objSLNew.SamplingProcedure = objSLOld.SamplingProcedure;
                objSLNew.SequenceTestSampleID = objSLOld.SequenceTestSampleID;
                objSLNew.SequenceTestSortNo = objSLOld.SequenceTestSortNo;
                objSLNew.ServiceArea = objSLOld.ServiceArea;
                objSLNew.SiteCode = objSLOld.SiteCode;
                objSLNew.SiteDescription = objSLOld.SiteDescription;
                objSLNew.SiteID = objSLOld.SiteID;
                objSLNew.SiteNameArchived = objSLOld.SiteNameArchived;
                objSLNew.SiteUserDefinedColumn1 = objSLOld.SiteUserDefinedColumn1;
                objSLNew.SiteUserDefinedColumn2 = objSLOld.SiteUserDefinedColumn2;
                objSLNew.SiteUserDefinedColumn3 = objSLOld.SiteUserDefinedColumn3;
                objSLNew.SubOut = objSLOld.SubOut;
                if (objSLOld.SystemType!=null)
                {
                    objSLNew.SystemType =uow.GetObjectByKey<SystemTypes>(objSLOld.SystemType.Oid); 
                }
                objSLNew.TargetMGTHC_CBD_mg_pc = objSLOld.TargetMGTHC_CBD_mg_pc;
                objSLNew.TargetMGTHC_CBD_mg_unit = objSLOld.TargetMGTHC_CBD_mg_unit;
                objSLNew.TargetPotency = objSLOld.TargetPotency;
                objSLNew.TargetUnitWeight_g_pc = objSLOld.TargetUnitWeight_g_pc;
                objSLNew.TargetUnitWeight_g_unit = objSLOld.TargetUnitWeight_g_unit;
                objSLNew.TargetWeight = objSLOld.TargetWeight;
                objSLNew.Time = objSLOld.Time;
                objSLNew.TimeEnd = objSLOld.TimeEnd;
                objSLNew.TimeStart = objSLOld.TimeStart;
                objSLNew.TotalSamples = objSLOld.TotalSamples;
                objSLNew.TotalTimes = objSLOld.TotalTimes;
                if (objSLOld.TtimeUnit != null)
                {
                    objSLNew.TtimeUnit = uow.GetObjectByKey<Unit>(objSLOld.TtimeUnit.Oid);
                }
                if (objSLOld.WaterType!=null)
                {
                    objSLNew.WaterType = uow.GetObjectByKey<WaterTypes>(objSLOld.WaterType.Oid); 
                }
                objSLNew.ZipCode = objSLOld.ZipCode;
                //objSLNew.ModifiedBy = objSLOld.ModifiedBy;
                if (objSLOld.ModifiedBy != null)
                {
                    objSLNew.ModifiedBy = uow.GetObjectByKey<Modules.BusinessObjects.Hr.CustomSystemUser>(objSLOld.ModifiedBy.Oid);
                }
                objSLNew.ModifiedDate = objSLOld.ModifiedDate;
                objSLNew.Comment = objSLOld.Comment;
                objSLNew.Latitude = objSLOld.Latitude;
                objSLNew.Longitude = objSLOld.Longitude;
                if (objSLOld.StationLocation!=null)
                {
                    objSLNew.StationLocation = uow.GetObjectByKey<SampleSites>(objSLOld.StationLocation.Oid);

                }
                objSLNew.AlternativeStation = objSLOld.AlternativeStation;
                objSLNew.AlternativeStationOid = objSLOld.AlternativeStationOid;
                List<Testparameter> lsttp = uow.Query<Testparameter>().Where(j => j.QCType.QCTypeName == "Sample" && j.SampleLogIns.Where(a => a.Oid == objSLOld.Oid).Count() > 0).ToList();
                foreach (var objLineA in lsttp)
                {
                    //objSLNew.Testparameters.Add(objLineA);
                    objSLNew.Testparameters.Add(uow.GetObjectByKey<Testparameter>(objLineA.Oid));
                }
                foreach (var objSampleparameter in objSLOld.SampleParameter.Where(a => a.IsGroup == true && a.GroupTest != null).ToList())
                {
                    SampleParameter sample = objSLNew.SampleParameter.FirstOrDefault<SampleParameter>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                    if (objSampleparameter.GroupTest != null && sample != null)
                    {
                        sample.IsGroup = true;
                        sample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objSampleparameter.GroupTest.Oid);
                        //sample.TestGroup = uow.GetObjectByKey<GroupTest>(objSampleparameter.TestGroup.Oid);
                    }
                }

                foreach (var objSampleparameter in objSLOld.SampleParameter.Where(a => a.TestHold == true).ToList())
                {
                    SampleParameter sample = objSLNew.SampleParameter.FirstOrDefault<SampleParameter>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                    if (sample != null)
                    {
                        sample.TestHold = true;
                    }
                }

                foreach (var objSampleparameter in objSLOld.SampleParameter.Where(a => a.SubOut == true).ToList())
                {
                    SampleParameter sample = objSLNew.SampleParameter.FirstOrDefault<SampleParameter>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                    if (sample != null)
                    {
                        sample.SubOut = true;
                    }
                }
                if (smplold != null && smplold.Count > 0)
                {
                    foreach (SampleBottleAllocation smpl in smplold.ToList())
                    {
                        SampleBottleAllocation smplnew = new SampleBottleAllocation(uow);
                        smplnew.SampleRegistration = objSLNew;
                        smplnew.TestMethod = uow.GetObjectByKey<TestMethod>(smpl.TestMethod.Oid);
                        smplnew.BottleID = smpl.BottleID;
                        if (smpl.Containers != null)
                        {
                            smplnew.Containers = uow.GetObjectByKey<Container>(smpl.Containers.Oid);
                        }
                        if (smpl.Preservative != null)
                        {
                            smplnew.Preservative = uow.GetObjectByKey<Preservative>(smpl.Preservative.Oid);
                        }
                        if (smpl.StorageID != null)
                        {
                            smplnew.StorageID = uow.GetObjectByKey<Storage>(smpl.StorageID.Oid);
                        }
                        if (smpl.StorageCondition != null)
                        {
                            smplnew.StorageCondition = uow.GetObjectByKey<PreserveCondition>(smpl.StorageCondition.Oid);
                        }
                        smplnew.Save();
                    }
                }
                if (objSLNew.JobID.Status == SampleRegistrationSignoffStatus.Signedoff)
                {
                    objSLNew.JobID.Status = SampleRegistrationSignoffStatus.PartiallySignedOff;
                }
                if (objSLNew.JobID.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                {
                    if (objSLNew.JobID.IsSampling)
                    {
                        //IList<SampleParameter> parameters = uow.GetObjects(uow.GetClassInfo(typeof(SampleParameter)), CriteriaOperator.Parse("[Samplelogin.Oid]=?", objSLOld.Oid), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                        //if (parameters.Where(a => a.Testparameter != null && a.Testparameter.TestMethod != null && a.Testparameter.TestMethod.IsFieldTest == true).Count() > 0)
                        //{
                            Frame.GetController<FlutterAppViewController>().insertsample(uow, objSLNew);
                        //}
                    }
                    Frame.GetController<AuditlogViewController>().insertauditdata(uow, objSLNew.JobID.Oid, OperationType.Created, "Sample Login", objSLNew.SampleSource, "Reanalysis", objSLNew.SampleID, "", "");
                }
                objSLNew.Save();
                uow.CommitChanges();
                ((ListView)View).CollectionSource.Add(((ListView)View).ObjectSpace.GetObject(objSLNew));
                View.Refresh();
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
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    string[] param = parameter.Split('|');
                    if (param[0] == "SampleSource")
                    {
                        if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                        {
                            object currentOid = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            IObjectSpace os = Application.CreateObjectSpace();
                            SampleLogIn objSample = os.GetObjectByKey<SampleLogIn>(currentOid);
                            if (objSample != null && objSample.IsReanalysis)
                            {
                                ShowViewParameters showViewParameters = new ShowViewParameters();
                                IObjectSpace labwareObjectSpace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(labwareObjectSpace, typeof(SampleLogIn));
                                List<SampleLogIn> lstSamples = os.GetObjects<SampleLogIn>(CriteriaOperator.Parse("JobID.Oid=?", objSample.JobID.Oid)).ToList();
                                //cs.Criteria["Filter"] = CriteriaOperator.Parse("JobID.JobID='" + objSample.JobID.JobID + "' AND [SampleNo]<'" +Convert.ToInt32(objSample.SampleNo) + "'");
                                cs.Criteria["Filter"] = CriteriaOperator.Parse("JobID.JobID='" + objSample.JobID.JobID + "'");
                                cs.Criteria["Filter2"] = new InOperator("Oid", lstSamples.Where(i => Convert.ToInt32(i.SampleNo) < Convert.ToInt32(objSample.SampleNo)).Select(i => i.Oid));
                                if (!string.IsNullOrEmpty(objSample.SampleSource))
                                {
                                    cs.Criteria["Filter1"] = CriteriaOperator.Parse("[SampleID] <> ?", objSample.SampleSource);
                                }
                                showViewParameters.CreatedView = Application.CreateListView("SampleLogIn_ListView_SourceSample", cs, false);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.Accepting += Dc_Accepting;
                                dc.SaveOnAccept = false;
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            }
                        }
                    }
                    else if (param[0] == "Selected")
                    {
                        if (HttpContext.Current.Session["rowid"] != null && editor != null)
                        {
                            Guid SampleOid = new Guid(HttpContext.Current.Session["rowid"].ToString());
                            SampleLogIn objSample = View.ObjectSpace.GetObjectByKey<SampleLogIn>(SampleOid);
                            if (objSample != null)
                            {
                                if (objSample.Testparameters.Count > 0)
                                {
                                    SampleLogIn objCurrentSample = (SampleLogIn)View.CurrentObject;
                                    if (objCurrentSample != null)
                                    {
                                        editor.Grid.Selection.UnselectRowByKey(objCurrentSample.Oid);
                                        Application.ShowViewStrategy.ShowMessage("Please removed the test in sample.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else if (param[0] == "AlternativeStation")
                    {
                        if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                        {
                            object currentOid = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            HttpContext.Current.Session["AlternativeStation"] = editor.Grid.GetRowValues(int.Parse(param[1]), "AlternativeStationOid");
                            IObjectSpace os = Application.CreateObjectSpace();
                            SampleLogIn objSample = os.GetObjectByKey<SampleLogIn>(currentOid);
                            if (objSample != null)
                            {
                                ShowViewParameters showViewParameters = new ShowViewParameters();
                                IObjectSpace labwareObjectSpace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(labwareObjectSpace, typeof(SampleSites));
                                cs.Criteria["Filter1"] = CriteriaOperator.Parse("[Client.Oid] = ?", objSample.JobID.ClientName.Oid);
                                showViewParameters.CreatedView = Application.CreateListView("SampleSites_LookupListView_Sampling", cs, false);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.AcceptAction.Execute += AcceptAction_Execute_AlternativeStation;
                                dc.SaveOnAccept = false;
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            }
                        }
                    }
                    else if (param[0] == "StationLocation.Oid"||param[0] == "StationLocationName")
                    {
                        if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                        {
                            object currentOid = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            HttpContext.Current.Session["StationLocation"] = editor.Grid.GetRowValues(int.Parse(param[1]), "StationLocation.Oid");
                            IObjectSpace os = Application.CreateObjectSpace();
                            SampleLogIn objSample = os.GetObjectByKey<SampleLogIn>(currentOid);
                            if (objSample != null)
                            {
                                ShowViewParameters showViewParameters = new ShowViewParameters();
                                IObjectSpace labwareObjectSpace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(labwareObjectSpace, typeof(SampleSites));
                                cs.Criteria["Filter1"] = CriteriaOperator.Parse("[Client.Oid] = ?", objSample.JobID.ClientName.Oid);
                                showViewParameters.CreatedView = Application.CreateListView("SampleSites_LookupListView_Sampling_StationLocation", cs, false);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.AcceptAction.Execute += AcceptAction_Execute_StationLocation;
                                dc.Accepting += Dc_Accepting1;
                                dc.SaveOnAccept = false;
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            }
                        }
                    }
                    else if (param[0] == "KeyMap")
                    {
                        if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                        {
                            object currentOid = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            HttpContext.Current.Session["StationLocation"] = editor.Grid.GetRowValues(int.Parse(param[1]), "StationLocation.Oid");
                            IObjectSpace os = Application.CreateObjectSpace();
                            SampleLogIn objSample = os.GetObjectByKey<SampleLogIn>(currentOid);
                            if (objSample != null)
                            {
                                ShowViewParameters showViewParameters = new ShowViewParameters();
                                IObjectSpace labwareObjectSpace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(labwareObjectSpace, typeof(SampleLogIn));
                                IList<SampleLogIn> lstSamples= View.ObjectSpace.GetObjects<SampleLogIn>(CriteriaOperator.Parse("")).Where(i=> !string.IsNullOrEmpty(i.KeyMap)).GroupBy(i => i.KeyMap).Select(grp => grp.FirstOrDefault()).ToList();
                                cs.Criteria["Filter1"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", lstSamples.Select(i=>i.Oid))) + ")");
                                // cs.Criteria["Filter1"] = CriteriaOperator.Parse("[Oid] = ?", lstSamples.Where(i=>!string.IsNullOrEmpty(i.KeyMap)).GroupBy(i=>i.KeyMap).FirstOrDefault().Select(i=>i.Oid));
                                showViewParameters.CreatedView = Application.CreateListView("SampleLogIn_LookupListView_KeyMap", cs, false);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.AcceptAction.Execute += AcceptAction_Execute;
                                dc.Accepting += Dc_Accepting1;
                                dc.SaveOnAccept = false;
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

        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string assigned = string.Empty;
                DialogController dc = sender as DialogController;
                if(dc!=null&& dc.Window!=null && dc.Window.View!=null)
                {
                    //SampleLogIn objSample=e.SelectedObjects.
                    if (dc.Window.View.Id== "SampleLogIn_LookupListView_KeyMap")
                    {
                        if (HttpContext.Current.Session["rowid"] != null)
                        {
                            SampleLogIn objsampling = ((ListView)View).CollectionSource.List.Cast<SampleLogIn>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                            if (objsampling != null)
                            {
                                //objsampling.KeyMap = objSample.KeyMap;
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

        private void Dc_Accepting1(object sender, DialogControllerAcceptingEventArgs e)
        {
           try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 1)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Accepting_StationLocation(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 1)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void AcceptAction_Execute_StationLocation(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (HttpContext.Current.Session["rowid"] != null)
                {
                    SampleLogIn objsampling = ((ListView)View).CollectionSource.List.Cast<SampleLogIn>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                    SampleSites objSite = e.SelectedObjects.Cast<SampleSites>().FirstOrDefault();
                    if (objSite != null)
                    {
                        objSite = View.ObjectSpace.GetObjectByKey<SampleSites>(objSite.Oid);
                        if (objsampling != null)
                        {
                            objsampling.StationLocation = objSite;
                            objsampling.PWSID = objSite.PWSID;
                            objsampling.KeyMap = objSite.KeyMap;
                            objsampling.Address = objSite.Address;
                            objsampling.SamplePointID = objSite.SamplePointID;
                            objsampling.SamplePointType = objSite.SamplePointType;
                            objsampling.SamplingAddress = objSite.SamplingAddress;
                            objsampling.StationLocationName = objSite.SiteName;
                            if (objSite.SystemType!=null)
                            {
                                objsampling.SystemType = View.ObjectSpace.GetObjectByKey<SystemTypes>(objSite.SystemType.Oid); 
                            }
                            if (objSite.PWSSystemName!=null)
                            {
                                objsampling.PWSSystemName = View.ObjectSpace.GetObjectByKey<PWSSystem>(objSite.PWSSystemName.Oid); 
                            }
                            objsampling.RejectionCriteria = objSite.RejectionCriteria;
                            if (objSite.WaterType!=null)
                            {
                                objsampling.WaterType = View.ObjectSpace.GetObjectByKey<WaterTypes>(objSite.WaterType.Oid);
                            }
                            objsampling.ParentSampleID = objSite.ParentSampleID;
                            objsampling.ParentSampleDate = objSite.ParentSampleDate;
                        }
                    }
                    else
                    {
                        objsampling.StationLocationName = null;
                        objsampling.StationLocation = null;
                        objsampling.PWSID = null;
                        objsampling.KeyMap = null;
                        objsampling.Address = null;
                        objsampling.SamplePointID = null;
                        objsampling.SamplePointType = null;
                        objsampling.SystemType = null;
                        objsampling.PWSSystemName = null;
                        objsampling.RejectionCriteria = null;
                        objsampling.WaterType = null;
                        objsampling.ParentSampleID = null;
                        objsampling.ParentSampleDate = null;
                        objsampling.SamplingAddress = null;
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

        private void AcceptAction_Execute_AlternativeStation(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string assigned = string.Empty;
                string assignedOid = string.Empty;
                assigned = string.Join("; ", e.SelectedObjects.Cast<SampleSites>().Where(i => i.SiteName != null && !string.IsNullOrEmpty(i.SiteName)).Select(i => i.SiteName).Distinct().ToList());
                assignedOid = string.Join("; ", e.SelectedObjects.Cast<SampleSites>().Where(i => i.SiteName != null && !string.IsNullOrEmpty(i.SiteName)).Select(i => i.Oid).Distinct().ToList());
                if (HttpContext.Current.Session["rowid"] != null)
                {
                    SampleLogIn objsampling = ((ListView)View).CollectionSource.List.Cast<SampleLogIn>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                    if (objsampling != null)
                    {
                        objsampling.AlternativeStation = assigned;
                        objsampling.AlternativeStationOid = assignedOid;
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
        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count == 1)
                {
                    SampleLogIn objSample = (SampleLogIn)e.AcceptActionArgs.CurrentObject;
                    Session currentSession = ((XPObjectSpace)(View.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    if (objSample != null && HttpContext.Current.Session["rowid"] != null)
                    {
                        SampleLogIn objSampleLogin = uow.FindObject<SampleLogIn>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (objSampleLogin != null && objSampleLogin.SampleSource != objSample.SampleID)
                        {
                            int sysSampleno = uow.Query<SampleLogIn>().Where(i => i.SampleSource != null && i.SampleSource == objSample.SampleID).ToList().Count();
                            sysSampleno++;
                            objSampleLogin.SampleSource = objSample.SampleID;
                            objSampleLogin.SysSampleCode = objSample.SampleID + "LR" + sysSampleno;
                            List<Testparameter> lsttp = uow.Query<Testparameter>().Where(j => j.QCType != null && j.QCType.QCTypeName == "Sample" && j.SampleLogIns.Where(a => a.Oid == objSample.Oid).Count() > 0).ToList();
                            foreach (var objLineA in lsttp)
                            {
                                objSampleLogin.Testparameters.Add(uow.GetObjectByKey<Testparameter>(objLineA.Oid));
                            }
                            uow.CommitChanges();
                            View.ObjectSpace.CommitChanges();
                            View.ObjectSpace.Refresh();
                        }
                    }
                }
                else
                {
                    if (e.AcceptActionArgs.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        #endregion
    }

}
