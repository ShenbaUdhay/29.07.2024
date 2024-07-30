using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SamplingManagement.Settings;
using Modules.BusinessObjects.Setting;

namespace LDM.Module.Controllers.SamplingManagement.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SamplingMatrixFieldStupViewController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        SamplingMatrixSetupFieldsinfo lstsmoid = new SamplingMatrixSetupFieldsinfo();
        bool IsFieldsPopulated = false;
        bool CanWrite = false;
        public SamplingMatrixFieldStupViewController()
        {
            InitializeComponent();
            TargetViewId = "NPSampleFields_ListView_SamplingProposal;" + "VisualMatrix_DetailView_SamplingFieldSetup;" + "VisualMatrix_ListView_FieldSetup_Copy_From_Sampling;" +
                "VisualMatrix_ListView_SamplingFieldSetup;" + "VisualMatrix_SamplingSetupFields_ListView;" + "VisualMatrix_DetailView_FieldSetup_Copy_FromTO_Sampling;" + "VisualMatrix_ListView_FieldSetup_Copy_To_Sampling;";
            AddFieldsSampling.TargetViewId = "VisualMatrix_DetailView_SamplingFieldSetup;"; 
            RemoveFieldsSampling.TargetViewId = "VisualMatrix_DetailView_SamplingFieldSetup;";

            SimpleAction Copytofield = new SimpleAction(this, "CopytofieldSampling", PredefinedCategory.View)
            {
                Caption = "Copy Fields",
                ImageName = "Action_Copy"
            };
            Copytofield.TargetViewId = "VisualMatrix_DetailView_SamplingFieldSetup;"; 
            Copytofield.Execute += Copytofield_Execute;
        }

      

        private void Copytofield_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "VisualMatrix_DetailView_SamplingFieldSetup")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    VisualMatrix objvm = os.CreateObject<VisualMatrix>();
                    DetailView createdv = Application.CreateDetailView(os, "VisualMatrix_DetailView_FieldSetup_Copy_FromTO_Sampling", true, objvm);
                    createdv.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showviewparameter = new ShowViewParameters(createdv);
                    showviewparameter.Context = TemplateContext.PopupWindow;
                    showviewparameter.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += dc_Accepting;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showviewparameter.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showviewparameter, new ShowViewSource(null, null));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (Application.MainWindow.View.Id == "VisualMatrix_DetailView_SamplingFieldSetup")
                {
                    ObjectSpace.CommitChanges();
                    VisualMatrix crtobj = (VisualMatrix)Application.MainWindow.View.CurrentObject;
                    if (lstsmoid.lstsamplematrixguid != null && lstsmoid.lstsamplematrixguid.Count > 0)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        VisualMatrix objsamchk = os.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", crtobj.Oid));
                        if (objsamchk != null)
                        {
                            IList<SamplingMatrixSetupFields> objmat = os.GetObjects<SamplingMatrixSetupFields>(CriteriaOperator.Parse("[VisualMatrix.Oid] =?", crtobj.Oid));
                            IList<VisualMatrix> lstSampleLocation = os.GetObjects<VisualMatrix>(CriteriaOperator.Parse("[Oid] In (" + string.Format("'{0}'", string.Join("','", lstsmoid.lstsamplematrixguid.Select(i => i.ToString().Replace("'", "''")))) + ")"));

                            foreach (VisualMatrix obj in lstSampleLocation.ToList())
                            {
                                IList<SamplingMatrixSetupFields> lstselsamplemat = os.GetObjects<SamplingMatrixSetupFields>(CriteriaOperator.Parse("[VisualMatrix.Oid] = ?", obj.Oid));
                                os.Delete(lstselsamplemat);
                                //foreach (SamplingMatrixSetupFields removesammat in lstselsamplemat.ToList())
                                //{
                                //    os.Delete(removesammat);
                                //}
                                os.CommitChanges();
                                foreach (SamplingMatrixSetupFields objcopyfrom in objmat.ToList())
                                {
                                    SamplingMatrixSetupFields objcopyto = os.FindObject<SamplingMatrixSetupFields>(CriteriaOperator.Parse("[FieldID] = ? And [VisualMatrix.Oid] = ?", objcopyfrom.FieldID, obj.Oid)); //[VisualMatrix] Is Null Or
                                    if (objcopyto == null)
                                    {
                                        objcopyto = os.CreateObject<SamplingMatrixSetupFields>();
                                        objcopyto.FieldID = objcopyfrom.FieldID;
                                        objcopyto.FieldCaption = objcopyfrom.FieldCaption;
                                        objcopyto.Freeze = objcopyfrom.Freeze;
                                        objcopyto.FieldCustomCaption = objcopyfrom.FieldCustomCaption;
                                        objcopyto.VisualMatrix = os.GetObject<VisualMatrix>(obj);
                                        objcopyto.Width = objcopyfrom.Width;
                                        objcopyto.SortOrder = objcopyfrom.SortOrder;
                                    }
                                    else
                                    {
                                        objcopyto.FieldCaption = objcopyfrom.FieldCaption;
                                        objcopyto.Width = objcopyfrom.Width;
                                        objcopyto.SortOrder = objcopyfrom.SortOrder;
                                        //objcopyto.FieldID = objcopyfrom.FieldID;
                                        objcopyto.FieldCaption = objcopyfrom.FieldCaption;
                                        objcopyto.FieldCustomCaption = objcopyfrom.FieldCustomCaption;
                                        //objcopyto.VisualMatrix = os.GetObject<VisualMatrix>(obj);
                                        objcopyto.Width = objcopyfrom.Width;
                                        objcopyto.SortOrder = objcopyfrom.SortOrder;
                                    }
                                }
                                os.CommitChanges();

                            }
                            Application.ShowViewStrategy.ShowMessage("Field setup copied success", InformationType.Success, 3000, InformationPosition.Top);
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Please add and save current matrix setup", InformationType.Error, 3000, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Select Checkbox", InformationType.Error, timer.Seconds, InformationPosition.Top);
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
            try
            {
                base.OnActivated();
                ObjectSpace.Committed += ObjectSpace_Committed;
                Employee currentUser = SecuritySystem.CurrentUser as Employee;
                if (currentUser != null && View != null && View.Id != null && currentUser.Roles != null && currentUser.Roles.Count > 0)
                {
                    CanWrite = false;
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        CanWrite = true;
                    }
                    else
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SampleMatrixFieldSetup" && i.Write == true) != null)
                            {
                                CanWrite = true;
                                break;
                            }
                        }
                    }
                }
                if (View.Id == "VisualMatrix_DetailView_SamplingFieldSetup")
                {
                    IsFieldsPopulated = false;
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    Frame.GetController<RefreshController>().RefreshAction.Executed += RefreshAction_Executing;
                    if (modificationController != null)
                    {
                        modificationController.SaveAndNewAction.Active.SetItemValue("SampleMatrixsavenew.visible", false);
                    }
                    if (!CanWrite)
                    {
                        AddFieldsSampling.Active["CanAddFields"] = false;
                        RemoveFieldsSampling.Active["CanRemoveFields"] = false;
                    }
                    else
                    {
                        if (AddFieldsSampling.Active.Contains("CanAddFields"))
                        {
                            AddFieldsSampling.Active.RemoveItem("CanAddFields");
                        }
                        if (RemoveFieldsSampling.Active.Contains("CanRemoveFields"))
                        {
                            RemoveFieldsSampling.Active.RemoveItem("CanRemoveFields");
                        }
                    }
                    DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("AvailableFields") as DashboardViewItem;
                    if (viAvailableFields != null)
                    {
                        viAvailableFields.ControlCreated += ViAvailableFields_ControlCreated;
                    }
                }

                if (View.Id == "VisualMatrix_ListView_FieldSetup_Copy_From_Sampling")
                {
                    lstsmoid.IsSampleMatrixSetupFieldsCallBackSelectAll = false;
                    Frame.GetController<ListViewController>().EditAction.Active["Edit"] = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ViAvailableFields_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("AvailableFields") as DashboardViewItem;
                if (viAvailableFields != null && viAvailableFields.InnerView != null && viAvailableFields.InnerView is ListView && IsFieldsPopulated == false)
                {
                    ListView liAvailableFields = viAvailableFields.InnerView as ListView;
                    if (liAvailableFields != null)
                    {

                        DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                        if (model != null)
                        {
                            DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                            if (lstViews != null)
                            {
                                DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Sampling_ListView_SamplingProposal");
                                if (lvSamplelogin != null)
                                {
                                    List<string> lstExistingFields = new List<string>();
                                    VisualMatrix setup = (VisualMatrix)View.CurrentObject;
                                    if (setup != null)
                                    {
                                        lstExistingFields = setup.SamplingSetupFields.Where(i => !string.IsNullOrEmpty(i.FieldID)).Select(i => i.FieldID).ToList();
                                        lstExistingFields.Add("Oid");
                                        lstExistingFields.Add("CanChangeVisualMatrix");
                                        lstExistingFields.Add("ModifiedBy");
                                        lstExistingFields.Add("ModifiedDate");
                                        lstExistingFields.Add("IsNotTransferred");
                                        lstExistingFields.Add("CollectTime");
                                        lstExistingFields.Add("SiteDescription");
                                        //lstExistingFields.Add("SiteName");
                                        //lstExistingFields.Add("ClientSampleID");
                                        lstExistingFields.Add("LastUpdatedDate");
                                        lstExistingFields.Add("LastUpdatedBy");
                                        lstExistingFields.Add("SettingsSource.SampleName");
                                        lstExistingFields.Add("SamplingProposal");
                                        lstExistingFields.Add("Hold");
                                    }
                                    foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)).OrderBy(i => i.Caption))
                                    {
                                        NPSampleFields slField = liAvailableFields.ObjectSpace.CreateObject<NPSampleFields>();
                                        slField.FieldID = col.Id.Replace(" ", "");
                                        slField.FieldCaption = col.Caption.Replace(" ", ""); ;
                                        liAvailableFields.CollectionSource.Add(slField);
                                    }
                                    IsFieldsPopulated = true;
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
            if (View.Id == "VisualMatrix_DetailView_SamplingFieldSetup")
            {
                ListPropertyEditor viSelectedFields = ((DetailView)View).FindItem("SamplingSetupFields") as ListPropertyEditor;
                ListView lstvi = viSelectedFields.ListView;
                ASPxGridListEditor listEditor = ((ListView)lstvi).Editor as ASPxGridListEditor;
                if (listEditor == null)
                    return;
                ASPxGridView gridView = (ASPxGridView)listEditor.Grid;
                if (gridView != null)
                {
                    gridView.Selection.UnselectAll();
                }
            }
            else if (View.Id == "VisualMatrix_ListView_FieldSetup_Copy_From_Sampling")
            {
                Application.ShowViewStrategy.ShowMessage("Fields Copied Success", InformationType.Success, 3000, InformationPosition.Top);
            }

        }
        private void RefreshAction_Executing(object sender, ActionBaseEventArgs e)
        {
            try
            {
                DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("AvailableFields") as DashboardViewItem;
                if (viAvailableFields != null && viAvailableFields.InnerView != null && viAvailableFields.InnerView is ListView)
                {
                    ListView liAvailableFields = viAvailableFields.InnerView as ListView;
                    if (liAvailableFields != null)
                    {
                        DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                        if (model != null)
                        {
                            DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                            if (lstViews != null)
                            {
                                DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "SampleLogIn_ListView_Copy_SampleRegistration");
                                if (lvSamplelogin != null)
                                {
                                    List<string> lstExistingFields = new List<string>();
                                    VisualMatrix setup = (VisualMatrix)View.CurrentObject;
                                    if (setup != null /*&& setup.SetupFields.Count > 0*/)
                                    {
                                        lstExistingFields = setup.SamplingSetupFields.Where(i => !string.IsNullOrEmpty(i.FieldID)).Select(i => i.FieldID).ToList();
                                        lstExistingFields.Add("Oid");
                                        lstExistingFields.Add("CanChangeVisualMatrix");
                                        lstExistingFields.Add("ModifiedBy");
                                        lstExistingFields.Add("ModifiedDate");
                                        lstExistingFields.Add("IsNotTransferred");
                                        lstExistingFields.Add("CollectTime");
                                        lstExistingFields.Add("SiteDescription");
                                       // lstExistingFields.Add("SiteName");
                                        lstExistingFields.Add("Station");
                                        //lstExistingFields.Add("ClientSampleID");
                                        lstExistingFields.Add("SettingsSource.SampleName");
                                        lstExistingFields.Add("SamplingProposal");
                                    }
                                    foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                    {
                                        NPSampleFields slField = liAvailableFields.ObjectSpace.CreateObject<NPSampleFields>();
                                        slField.FieldID = col.Id.Replace(" ", "");
                                        slField.FieldCaption = col.Caption.Replace(" ", "");
                                        liAvailableFields.CollectionSource.Add(slField);
                                    }
                                    IsFieldsPopulated = true;
                                }
                            }
                        }

                    }
                }
                View.Refresh();
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
                if (View.Id == "VisualMatrix_ListView_FieldSetup_Copy_From_Sampling")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("VisualMatrix", this);
                        if (editor != null && editor.Grid != null)
                        {
                            editor.Grid.Load += Grid_Load;
                            editor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                            editor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
var chkselect = s.cpEndCallbackHandlers;
                        if (e.visibleIndex != '-1')
                        {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                            RaiseXafCallback(globalCallbackControl, 'VisualMatrix', 'Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'VisualMatrix', 'UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                        }
                        else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                        {        
if(chkselect != 'selectall')
{
RaiseXafCallback(globalCallbackControl, 'VisualMatrix', 'SelectAll', '', false);      
}

                        //RaiseXafCallback(globalCallbackControl, 'VisualMatrix', 'SelectAll', '', false);     
                        }
                         else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                         {
                        RaiseXafCallback(globalCallbackControl, 'VisualMatrix', 'UNSelectAll', '', false);                        
                        }                      
                    }";
                            // editor.Grid.ClientSideEvents.Init = js;
                        }
                    }
                }
                else if (View.Id == "VisualMatrix_DetailView_FieldSetup_Copy_FromTO_Sampling")
                {
                    DashboardViewItem DVcopyfieldsfrom = ((DetailView)View).FindItem("CopyFieldsFrom") as DashboardViewItem;
                    ListView innerListView1 = DVcopyfieldsfrom.InnerView as ListView;
                    DashboardViewItem dvcopyto = ((DetailView)View).FindItem("CopyFieldsTo") as DashboardViewItem;
                    VisualMatrix crtsamplematrix = (VisualMatrix)View.CurrentObject;
                    if (Application.MainWindow.View.Id == "VisualMatrix_DetailView_SamplingFieldSetup")
                    {
                        VisualMatrix objsamplematrix = (VisualMatrix)Application.MainWindow.View.CurrentObject;
                        if (objsamplematrix != null)
                        {
                            crtsamplematrix.VisualMatrixName = objsamplematrix.VisualMatrixName;
                            ((ListView)DVcopyfieldsfrom.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[VisualMatrixName] <> ?", objsamplematrix.VisualMatrixName);
                        }
                    }
                }
                else if (View.Id == "VisualMatrix_DetailView_SamplingFieldSetup" && !IsFieldsPopulated)
                {
                    DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("AvailableFields") as DashboardViewItem;
                    if (viAvailableFields != null && viAvailableFields.InnerView != null && viAvailableFields.InnerView is ListView)
                    {
                        ListView liAvailableFields = viAvailableFields.InnerView as ListView;
                        if (liAvailableFields != null)
                        {

                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "SampleLogIn_ListView_Copy_SampleRegistration");
                                    if (lvSamplelogin != null)
                                    {
                                        List<string> lstExistingFields = new List<string>();
                                        VisualMatrix setup = (VisualMatrix)View.CurrentObject;
                                        if (setup != null)
                                        {
                                            lstExistingFields = setup.SamplingSetupFields.Where(i => !string.IsNullOrEmpty(i.FieldID)).Select(i => i.FieldID).ToList();
                                            lstExistingFields.Add("Oid");
                                            lstExistingFields.Add("CanChangeVisualMatrix");
                                            lstExistingFields.Add("ModifiedBy");
                                            lstExistingFields.Add("ModifiedDate");
                                            lstExistingFields.Add("IsNotTransferred");
                                            lstExistingFields.Add("CollectTime");
                                            lstExistingFields.Add("SiteDescription");
                                            lstExistingFields.Add("SiteName");
                                            lstExistingFields.Add("ClientSampleID");
                                            lstExistingFields.Add("SettingsSource.SampleName");
                                        }
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            NPSampleFields slField = liAvailableFields.ObjectSpace.CreateObject<NPSampleFields>();
                                            slField.FieldID = col.Id.Replace(" ", "");
                                            slField.FieldCaption = col.Caption.Replace(" ", ""); ;
                                            liAvailableFields.CollectionSource.Add(slField);
                                        }
                                        IsFieldsPopulated = true;
                                    }
                                }
                            }

                        }
                    }
                }
                else if (View.Id == "NPSampleFields_ListView_SamplingProposal")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 320;
                        gridListEditor.Grid.SettingsBehavior.AllowSelectByRowClick = false;

                        gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                            {
                            var nav = document.getElementById('LPcell');
                            var sep = document.getElementById('separatorCell');
                            if(nav != null && sep != null) {
                               var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                               s.SetWidth((totusablescr / 100) * 18); 
                            }
                            else {
                                
                                s.SetWidth(180); 
                            }                        
                        }";
                    }
                }
                else if (View.Id == "VisualMatrix_SamplingSetupFields_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 320;
                        gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    {                 
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');
                        if(nav != null && sep != null) {
                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                           s.SetWidth((totusablescr / 100) * 62);         
                        }
                        else {
                            s.SetWidth(700); 
                        }                  
                    }";
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
                e.Properties["cpIsSelectAll"] = lstsmoid.IsSampleMatrixSetupFieldsCallBackSelectAll;
                if (lstsmoid.IsSampleMatrixSetupFieldsCallBackSelectAll == true)
                {
                    e.Properties["cpEndCallbackHandlers"] = "selectall";
                }
                else
                {
                    e.Properties["cpEndCallbackHandlers"] = null;
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
                    if (View.Id == "VisualMatrix_ListView_FieldSetup_Copy_From_Sampling")
                    {
                        string[] arrParams = parameter.Split('|');
                        IObjectSpace space = Application.CreateObjectSpace();
                        if (lstsmoid.lstsamplematrixguid == null)
                        {
                            lstsmoid.lstsamplematrixguid = new List<Guid>();
                        }

                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView view = nestedFrame.ViewItem.View;
                        DashboardViewItem objCopyFieldsFrom = (DashboardViewItem)((DetailView)view).FindItem("CopyFieldsFrom");
                        if (objCopyFieldsFrom != null && objCopyFieldsFrom.InnerView != null && objCopyFieldsFrom.InnerView is ListView)
                        {
                            ASPxGridListEditor editor = ((ListView)objCopyFieldsFrom.InnerView).Editor as ASPxGridListEditor;
                            if (editor != null && editor.Grid != null)
                            {
                                if (arrParams[0] == "Selected" || arrParams[0] == "UNSelected")
                                {
                                    if (arrParams[0] == "Selected")
                                    {
                                        lstsmoid.IsSampleMatrixSetupFieldsCallBackSelectAll = false;
                                        if (lstsmoid.lstsamplematrixguid != null && !lstsmoid.lstsamplematrixguid.Contains(new Guid(arrParams[1])))
                                        {
                                            lstsmoid.lstsamplematrixguid.Add(new Guid(arrParams[1]));
                                        }
                                    }
                                    else if (arrParams[0] == "UNSelected")
                                    {
                                        lstsmoid.IsSampleMatrixSetupFieldsCallBackSelectAll = false;
                                        if (lstsmoid.lstsamplematrixguid != null && lstsmoid.lstsamplematrixguid.Contains(new Guid(arrParams[1])))
                                        {
                                            lstsmoid.lstsamplematrixguid.Remove(new Guid(arrParams[1]));
                                        }
                                    }
                                }
                                else if (arrParams[0] == "SelectAll" && lstsmoid.IsSampleMatrixSetupFieldsCallBackSelectAll == false)
                                {
                                    lstsmoid.IsSampleMatrixSetupFieldsCallBackSelectAll = true;
                                    lstsmoid.lstsamplematrixguid.Clear();
                                    if (((ListView)objCopyFieldsFrom.InnerView).CollectionSource.List.Count > 0)
                                    {
                                        IList<VisualMatrix> lstoid = ObjectSpace.GetObjects<VisualMatrix>(CriteriaOperator.Parse("Not IsNullOrEmpty([VisualMatrixName])"));
                                        if (lstoid != null && lstoid.Count > 0)
                                        {
                                            foreach (VisualMatrix objoid in lstoid.ToList())
                                            {
                                                if (lstsmoid.lstsamplematrixguid != null && !lstsmoid.lstsamplematrixguid.Contains(objoid.Oid))
                                                {
                                                    lstsmoid.lstsamplematrixguid.Add(objoid.Oid);
                                                }
                                            }
                                        }
                                        VisualMatrix objVisualMatrix = (VisualMatrix)Application.MainWindow.View.CurrentObject;
                                        if (objVisualMatrix != null)
                                        {
                                            if (lstsmoid.lstsamplematrixguid.Contains(objVisualMatrix.Oid))
                                            {
                                                lstsmoid.lstsamplematrixguid.Remove(objVisualMatrix.Oid);
                                            }
                                        }
                                    }
                                }
                                else if (arrParams[0] == "UNSelectAll")
                                {
                                    lstsmoid.lstsamplematrixguid.Clear();
                                    lstsmoid.IsSampleMatrixSetupFieldsCallBackSelectAll = false;
                                }
                                foreach (Guid oid in lstsmoid.lstsamplematrixguid.ToList())
                                {
                                    editor.Grid.Selection.SelectRowByKey(oid);
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

        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (View.Id == "NPSampleFields_ListView_SamplingProposal")
                {
                    GridViewColumn AddField = gridView.Columns.Cast<GridViewColumn>().Where(i => i.Name == "AddField").ToList()[0];
                    if (AddField != null)
                    {
                        AddField.Width = 40;
                    }
                    foreach (GridViewColumn column in gridView.Columns)
                    {
                        if (column.Name == "SelectionCommandColumn")
                        {
                            column.Visible = false;
                        }
                        else if (column.Caption == "Name")
                        {
                            column.FixedStyle = GridViewColumnFixedStyle.Left;
                            //column.Width = 120;
                        }
                    }
                }
                else if (View.Id == "VisualMatrix_SamplingSetupFields_ListView")
                {
                    foreach (GridViewColumn column in gridView.Columns)
                    {
                        if (column.Name == "SelectionCommandColumn")
                        {
                            column.Visible = false;
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
            try
            {
                base.OnDeactivated();
                if (View.Id == "VisualMatrix_DetailView_SamplingFieldSetup")
                {
                    IsFieldsPopulated = false;
                    Frame.GetController<RefreshController>().RefreshAction.Executed -= RefreshAction_Executing;
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        if (modificationController.SaveAndNewAction.Active.Contains("SampleMatrixsavenew.visible"))
                        {
                            modificationController.SaveAndNewAction.Active.RemoveItem("SampleMatrixsavenew.visible");
                        }
                    }
                }
                if (View.Id == "VisualMatrix_DetailView_FieldSetup_Copy_FromTO_Sampling")
                {
                    lstsmoid.lstsamplematrixguid = null;
                    lstsmoid.IsSampleMatrixSetupFieldsCallBackSelectAll = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AddFields_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "VisualMatrix_DetailView_SamplingFieldSetup")
                {
                    DashboardViewItem viAddFields = ((DetailView)View).FindItem("AvailableFields") as DashboardViewItem;
                    DevExpress.ExpressApp.Editors.ListPropertyEditor viSelectedFields = ((DetailView)View).FindItem("SamplingSetupFields") as DevExpress.ExpressApp.Editors.ListPropertyEditor;
                    VisualMatrix setup = View.CurrentObject as VisualMatrix;
                    if (viAddFields != null && viAddFields.InnerView != null && viSelectedFields != null && viSelectedFields.ListView != null && setup != null)
                    {
                        ListView lvAddFields = (ListView)viAddFields.InnerView;
                        ListView lvSelFields = viSelectedFields.ListView;
                        if (lvAddFields != null && lvSelFields != null)
                        {
                            if (lvAddFields.SelectedObjects.Count > 0)
                            {
                                List<NPSampleFields> lstAvailSampleFields = lvAddFields.SelectedObjects.Cast<NPSampleFields>().ToList();
                                if (lstAvailSampleFields != null && lstAvailSampleFields.Count > 0)
                                {

                                    foreach (NPSampleFields availField in lstAvailSampleFields)
                                    {
                                        SamplingMatrixSetupFields field = lvSelFields.ObjectSpace.FindObject<SamplingMatrixSetupFields>(CriteriaOperator.Parse("[FieldID] = ? And ([VisualMatrix] Is Null Or [VisualMatrix.Oid] = ?)", availField.FieldID, setup.Oid));
                                        if (field == null)
                                        {
                                            field = lvSelFields.ObjectSpace.CreateObject<SamplingMatrixSetupFields>();
                                            field.FieldID = availField.FieldID.Replace(" ", "");
                                            field.FieldCaption = availField.FieldCaption.Replace(" ", "");
                                            field.VisualMatrix = lvSelFields.ObjectSpace.GetObject<VisualMatrix>(setup); field.Width = 100;
                                            int sortvalue = ((ListView)viSelectedFields.ListView).CollectionSource.List.Count;
                                            field.SortOrder = sortvalue + 1;
                                            lvSelFields.CollectionSource.Add(field);
                                            lvAddFields.CollectionSource.Remove(availField);
                                        }
                                        else
                                        {
                                            SamplingMatrixSetupFields objfield = lvSelFields.ObjectSpace.FindObject<SamplingMatrixSetupFields>(CriteriaOperator.Parse("([VisualMatrix.VisualMatrixName] = ?)", setup.VisualMatrixName));
                                            field.FieldCaption = availField.FieldCaption;
                                            field.Width = 100;
                                            int sortvalue = ((ListView)viSelectedFields.ListView).CollectionSource.List.Count;
                                            field.SortOrder = sortvalue + 1;
                                            lvSelFields.CollectionSource.Add(field);
                                            lvAddFields.CollectionSource.Remove(availField);
                                        }

                                    }
                                    lvSelFields.Refresh();
                                    lvAddFields.Refresh();
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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

        private void RemoveFields_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "VisualMatrix_DetailView_SamplingFieldSetup")
                {
                    DashboardViewItem viAddFields = ((DetailView)View).FindItem("AvailableFields") as DashboardViewItem;
                    DevExpress.ExpressApp.Editors.ListPropertyEditor viSelectedFields = ((DetailView)View).FindItem("SamplingSetupFields") as DevExpress.ExpressApp.Editors.ListPropertyEditor;
                    if (viAddFields != null && viAddFields.InnerView != null && viSelectedFields != null && viSelectedFields.ListView != null)
                    {
                        ListView lvAddFields = (ListView)viAddFields.InnerView;
                        ListView lvSelFields = viSelectedFields.ListView;
                        if (lvAddFields != null && lvSelFields != null)
                        {
                            if (lvSelFields.SelectedObjects.Count > 0)
                            {
                                List<SamplingMatrixSetupFields> lstSelSampleFields = lvSelFields.SelectedObjects.Cast<SamplingMatrixSetupFields>().ToList();
                                if (lstSelSampleFields != null && lstSelSampleFields.Count > 0)
                                {
                                    foreach (SamplingMatrixSetupFields selFields in lstSelSampleFields)
                                    {
                                        NPSampleFields field = lvAddFields.ObjectSpace.CreateObject<NPSampleFields>();
                                        field.FieldID = selFields.FieldID.Replace(" ", "");
                                        field.FieldCaption = selFields.FieldCaption.Replace(" ", "");
                                        lvSelFields.CollectionSource.Remove(selFields);
                                        lvAddFields.CollectionSource.Add(field);
                                    }
                                    lvSelFields.Refresh();
                                    lvAddFields.Refresh();
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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
