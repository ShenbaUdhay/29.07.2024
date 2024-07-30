using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Labmaster.Module.Controllers.TaskManagement
{
    public partial class SamplingFieldConfigurationViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        bool IsSamplingFieldConfiguration = false;
        bool IsStationFieldConfiguration = false;
        bool IsMonitoringFieldConfiguration = false;
        bool CanWrite = false;
        public SamplingFieldConfigurationViewController()
        {
            InitializeComponent();
            TargetViewId = "VisualMatrix_ListView_SamplingFieldsConfiguration;"
                + "VisualMatrix_DetailView_FieldConfiguration;"
                + "NPSamplingFieldConfiguration_ListView;"
                + "NPSamplingFieldConfiguration_ListView_Station;"
                + "NPSamplingFieldConfiguration_ListView_Test;"
                + "VisualMatrix_SamplingFieldConfiguration_ListView;"
                + "VisualMatrix_SamplingFieldConfiguration_ListView_Station;"
                + "VisualMatrix_SamplingFieldConfiguration_ListView_Test;";
            addFieldConfiguration.TargetViewId = "NPSamplingFieldConfiguration_ListView;";
            removeFieldConfiguration.TargetViewId = "VisualMatrix_SamplingFieldConfiguration_ListView;";
            addStationFieldConfiguration.TargetViewId = "NPSamplingFieldConfiguration_ListView_Station;";
            removeStationFieldConfiguration.TargetViewId = "VisualMatrix_SamplingFieldConfiguration_ListView_Station;";
            addTestConfiguration.TargetViewId = "NPSamplingFieldConfiguration_ListView_Test;";
            removeTestConfiguration.TargetViewId = "VisualMatrix_SamplingFieldConfiguration_ListView_Test;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
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
                if (View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView" || View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView_Station" || View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView_Test")
                {
                    Frame.GetController<ExportController>().ExportAction.Active["ShowExport"] = false;
                }
                if (View.Id == "VisualMatrix_DetailView_FieldConfiguration")
                {
                    IsStationFieldConfiguration = false;
                    Frame.GetController<RefreshController>().RefreshAction.Executed += RefreshAction_Executed;
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        modificationController.SaveAndNewAction.Active.SetItemValue("SampleMatrixsavenew.visible", false);
                    }
                    if (!CanWrite)
                    {
                        addFieldConfiguration.Active["CanAddFields"] = false;
                        removeFieldConfiguration.Active["CanRemoveFields"] = false;
                        addStationFieldConfiguration.Active["CanAddFields"] = false;
                        removeStationFieldConfiguration.Active["CanRemoveFields"] = false;
                        addTestConfiguration.Active["CanAddFields"] = false;
                        removeTestConfiguration.Active["CanRemoveFields"] = false;
                    }
                    else
                    {
                        if (addFieldConfiguration.Active.Contains("CanAddFields"))
                        {
                            addFieldConfiguration.Active.RemoveItem("CanAddFields");
                        }
                        if (removeFieldConfiguration.Active.Contains("CanRemoveFields"))
                        {
                            removeFieldConfiguration.Active.RemoveItem("CanRemoveFields");
                        }
                        if (addStationFieldConfiguration.Active.Contains("CanAddFields"))
                        {
                            addStationFieldConfiguration.Active.RemoveItem("CanAddFields");
                        }
                        if (removeStationFieldConfiguration.Active.Contains("CanRemoveFields"))
                        {
                            removeStationFieldConfiguration.Active.RemoveItem("CanRemoveFields");
                        }
                        if (addTestConfiguration.Active.Contains("CanAddFields"))
                        {
                            addTestConfiguration.Active.RemoveItem("CanAddFields");
                        }
                        if (removeTestConfiguration.Active.Contains("CanRemoveFields"))
                        {
                            removeTestConfiguration.Active.RemoveItem("CanRemoveFields");
                        }
                    }
                    DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("AvailableFields") as DashboardViewItem;
                    if (viAvailableFields != null)
                    {
                        viAvailableFields.ControlCreated += ViAvailableFields_ControlCreated;
                    }
                    DashboardViewItem viAvailableStationFields = ((DetailView)View).FindItem("AvailableStationFields") as DashboardViewItem;
                    if (viAvailableStationFields != null)
                    {
                        viAvailableStationFields.ControlCreated += ViAvailableStationFields_ControlCreated;
                    }
                    DashboardViewItem viAvailableTestFields = ((DetailView)View).FindItem("AvailableTestFields") as DashboardViewItem;
                    if (viAvailableTestFields != null)
                    {
                        viAvailableTestFields.ControlCreated += ViAvailableTestFields_ControlCreated;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RefreshAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                IsMonitoringFieldConfiguration = false;
                IsStationFieldConfiguration = false;
                IsSamplingFieldConfiguration = false;
                DashboardViewItem viAvailableTestFields = ((DetailView)View).FindItem("AvailableTestFields") as DashboardViewItem;
                if (!IsMonitoringFieldConfiguration && viAvailableTestFields != null && viAvailableTestFields.InnerView != null && viAvailableTestFields.InnerView is ListView)
                {
                    ListView liAvailableFields = viAvailableTestFields.InnerView as ListView;
                    if (liAvailableFields != null)
                    {
                        DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                        if (model != null)
                        {
                            DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                            if (lstViews != null)
                            {
                                DevExpress.ExpressApp.Model.IModelListView lvSampling = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "SampleParameter_ListView");
                                if (lvSampling != null)
                                {
                                    List<string> lstSamplingExistingFields = new List<string>();
                                    VisualMatrix setup = (VisualMatrix)View.CurrentObject;
                                    if (setup != null)
                                    {
                                        lstSamplingExistingFields = setup.SamplingFieldConfiguration.Where(i => !string.IsNullOrEmpty(i.FieldID) && i.FieldClass == FieldClass.Test).Select(i => i.FieldID).ToList();
                                        lstSamplingExistingFields.Add("Oid");
                                        lstSamplingExistingFields.Add("Sampling");
                                        lstSamplingExistingFields.Add("Testparameter");
                                        lstSamplingExistingFields.Add("Bottle");
                                    }
                                    foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSampling.Columns.Where(i => !lstSamplingExistingFields.Contains(i.Id)))
                                    {
                                        NPSamplingFieldConfiguration slField = liAvailableFields.ObjectSpace.CreateObject<NPSamplingFieldConfiguration>();
                                        slField.FieldID = col.Id;
                                        slField.FieldCaption = col.Caption;
                                        liAvailableFields.CollectionSource.Add(slField);
                                        liAvailableFields.Refresh();
                                    }
                                    IsMonitoringFieldConfiguration = true;
                                }
                            }
                        }
                    }
                }
                DashboardViewItem viAvailableStationFields = ((DetailView)View).FindItem("AvailableStationFields") as DashboardViewItem;
                if (!IsStationFieldConfiguration && viAvailableStationFields != null && viAvailableStationFields.InnerView != null && viAvailableStationFields.InnerView is ListView)
                {
                    ListView liAvailableFields = viAvailableStationFields.InnerView as ListView;
                    if (liAvailableFields != null)
                    {
                        DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                        if (model != null)
                        {
                            DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                            if (lstViews != null)
                            {
                                DevExpress.ExpressApp.Model.IModelListView lvSampling = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "SampleLogIn_ListView");
                                if (lvSampling != null)
                                {
                                    List<string> lstSamplingExistingFields = new List<string>();
                                    VisualMatrix setup = (VisualMatrix)View.CurrentObject;
                                    if (setup != null)
                                    {
                                        lstSamplingExistingFields = setup.SamplingFieldConfiguration.Where(i => !string.IsNullOrEmpty(i.FieldID) && i.FieldClass == FieldClass.Station).Select(i => i.FieldID).ToList();
                                        lstSamplingExistingFields.Add("Oid");
                                        lstSamplingExistingFields.Add("Tasks");
                                    }
                                    foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSampling.Columns.Where(i => !lstSamplingExistingFields.Contains(i.Id)))
                                    {
                                        NPSamplingFieldConfiguration slField = liAvailableFields.ObjectSpace.CreateObject<NPSamplingFieldConfiguration>();
                                        slField.FieldID = col.Id;
                                        slField.FieldCaption = col.Caption;
                                        liAvailableFields.CollectionSource.Add(slField);
                                        liAvailableFields.Refresh();
                                    }
                                    IsStationFieldConfiguration = true;
                                }
                            }
                        }
                    }
                }
                DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("AvailableFields") as DashboardViewItem;
                if (!IsSamplingFieldConfiguration && viAvailableFields != null && viAvailableFields.InnerView != null && viAvailableFields.InnerView is ListView)
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
                                DevExpress.ExpressApp.Model.IModelListView lvSampling = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "SampleLogIn_ListView");
                                if (lvSampling != null)
                                {
                                    List<string> lstSamplingExistingFields = new List<string>();
                                    VisualMatrix setup = (VisualMatrix)View.CurrentObject;
                                    if (setup != null)
                                    {
                                        lstSamplingExistingFields = setup.SamplingFieldConfiguration.Where(i => !string.IsNullOrEmpty(i.FieldID) && i.FieldClass == FieldClass.Sampling).Select(i => i.FieldID).ToList();
                                        lstSamplingExistingFields.Add("Oid");
                                        lstSamplingExistingFields.Add("Tasks");
                                        lstSamplingExistingFields.Add("StationID");
                                        lstSamplingExistingFields.Add("SamplingLocation");
                                        lstSamplingExistingFields.Add("Department");
                                        lstSamplingExistingFields.Add("Frequency");
                                        lstSamplingExistingFields.Add("JobSampleID");
                                        lstSamplingExistingFields.Add("QcType");
                                        lstSamplingExistingFields.Add("QcSource");
                                        lstSamplingExistingFields.Add("AssignedBy");
                                        lstSamplingExistingFields.Add("AssignTo");
                                        lstSamplingExistingFields.Add("DateAssigned");
                                        lstSamplingExistingFields.Add("IsTransferred");
                                        lstSamplingExistingFields.Add("SampleLogin");
                                    }
                                    foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSampling.Columns.Where(i => !lstSamplingExistingFields.Contains(i.Id)))
                                    {
                                        NPSamplingFieldConfiguration slField = liAvailableFields.ObjectSpace.CreateObject<NPSamplingFieldConfiguration>();
                                        slField.FieldID = col.Id;
                                        slField.FieldCaption = col.Caption;
                                        liAvailableFields.CollectionSource.Add(slField);
                                        liAvailableFields.Refresh();
                                    }
                                    IsSamplingFieldConfiguration = true;
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

        private void ViAvailableTestFields_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("AvailableTestFields") as DashboardViewItem;
                if (!IsMonitoringFieldConfiguration && viAvailableFields != null && viAvailableFields.InnerView != null && viAvailableFields.InnerView is ListView)
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
                                DevExpress.ExpressApp.Model.IModelListView lvSampling = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "SampleParameter_ListView");
                                if (lvSampling != null)
                                {
                                    List<string> lstSamplingExistingFields = new List<string>();
                                    VisualMatrix setup = (VisualMatrix)View.CurrentObject;
                                    if (setup != null)
                                    {
                                        lstSamplingExistingFields = setup.SamplingFieldConfiguration.Where(i => !string.IsNullOrEmpty(i.FieldID) && i.FieldClass == FieldClass.Test).Select(i => i.FieldID).ToList();
                                        lstSamplingExistingFields.Add("Oid");
                                        lstSamplingExistingFields.Add("Sampling");
                                        lstSamplingExistingFields.Add("Testparameter");
                                        lstSamplingExistingFields.Add("Parent");
                                    }
                                    foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSampling.Columns.Where(i => !lstSamplingExistingFields.Contains(i.Id)))
                                    {
                                        NPSamplingFieldConfiguration slField = liAvailableFields.ObjectSpace.CreateObject<NPSamplingFieldConfiguration>();
                                        slField.FieldID = col.Id;
                                        slField.FieldCaption = col.Caption;
                                        liAvailableFields.CollectionSource.Add(slField);
                                    }
                                    IsMonitoringFieldConfiguration = true;
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

        private void ViAvailableStationFields_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("AvailableStationFields") as DashboardViewItem;
                if (!IsStationFieldConfiguration && viAvailableFields != null && viAvailableFields.InnerView != null && viAvailableFields.InnerView is ListView)
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
                                DevExpress.ExpressApp.Model.IModelListView lvSampling = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "SampleLogIn_ListView");
                                if (lvSampling != null)
                                {
                                    List<string> lstSamplingExistingFields = new List<string>();
                                    VisualMatrix setup = (VisualMatrix)View.CurrentObject;
                                    if (setup != null)
                                    {
                                        lstSamplingExistingFields = setup.SamplingFieldConfiguration.Where(i => !string.IsNullOrEmpty(i.FieldID) && i.FieldClass == FieldClass.Station).Select(i => i.FieldID).ToList();
                                        lstSamplingExistingFields.Add("Oid");
                                        lstSamplingExistingFields.Add("Tasks");
                                    }
                                    foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSampling.Columns.Where(i => !lstSamplingExistingFields.Contains(i.Id)))
                                    {
                                        NPSamplingFieldConfiguration slField = liAvailableFields.ObjectSpace.CreateObject<NPSamplingFieldConfiguration>();
                                        slField.FieldID = col.Id;
                                        slField.FieldCaption = col.Caption;
                                        liAvailableFields.CollectionSource.Add(slField);
                                    }
                                    IsStationFieldConfiguration = true;
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

        private void ViAvailableFields_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("AvailableFields") as DashboardViewItem;
                if (!IsSamplingFieldConfiguration && viAvailableFields != null && viAvailableFields.InnerView != null && viAvailableFields.InnerView is ListView)
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
                                DevExpress.ExpressApp.Model.IModelListView lvSampling = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "SampleLogIn_ListView");
                                if (lvSampling != null)
                                {
                                    List<string> lstSamplingExistingFields = new List<string>();
                                    VisualMatrix setup = (VisualMatrix)View.CurrentObject;
                                    if (setup != null)
                                    {
                                        lstSamplingExistingFields = setup.SamplingFieldConfiguration.Where(i => !string.IsNullOrEmpty(i.FieldID) && i.FieldClass == FieldClass.Sampling).Select(i => i.FieldID).ToList();
                                        lstSamplingExistingFields.Add("Oid");
                                        lstSamplingExistingFields.Add("Tasks");
                                        lstSamplingExistingFields.Add("StationID");
                                        lstSamplingExistingFields.Add("SamplingLocation");
                                        lstSamplingExistingFields.Add("Department");
                                        lstSamplingExistingFields.Add("Frequency");
                                        lstSamplingExistingFields.Add("JobSampleID");
                                        lstSamplingExistingFields.Add("QcType");
                                        lstSamplingExistingFields.Add("QcSource");
                                        lstSamplingExistingFields.Add("AssignedBy");
                                        lstSamplingExistingFields.Add("AssignTo");
                                        lstSamplingExistingFields.Add("DateAssigned");
                                        lstSamplingExistingFields.Add("IsTransferred");
                                        lstSamplingExistingFields.Add("SampleLogin");
                                        lstSamplingExistingFields.Add("CollectTime");
                                    }
                                    foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSampling.Columns.Where(i => !lstSamplingExistingFields.Contains(i.Id)))
                                    {
                                        NPSamplingFieldConfiguration slField = liAvailableFields.ObjectSpace.CreateObject<NPSamplingFieldConfiguration>();
                                        slField.FieldID = col.Id;
                                        slField.FieldCaption = col.Caption;
                                        liAvailableFields.CollectionSource.Add(slField);
                                    }
                                    IsSamplingFieldConfiguration = true;
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

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View.Id == "NPSamplingFieldConfiguration_ListView" || View.Id == "NPSamplingFieldConfiguration_ListView_Station" || View.Id == "NPSamplingFieldConfiguration_ListView_Test")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 320;
                        gridListEditor.Grid.SettingsBehavior.AllowSelectByRowClick = false;
                        gridListEditor.Grid.Load += Grid_Load;
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
                else if (View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView" || View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView_Station" || View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView_Test")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 320;
                        gridListEditor.Grid.Load += Grid_Load;
                        gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                        { 
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');
                        if(nav != null && sep != null) {
                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                           s.SetWidth((totusablescr / 100) * 65);         
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

        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (View.Id == "NPSamplingFieldConfiguration_ListView" || View.Id == "NPSamplingFieldConfiguration_ListView_Station" || View.Id == "NPSamplingFieldConfiguration_ListView_Test")
                {
                    GridViewColumn addFieldConfiguration = gridView.Columns.Cast<GridViewColumn>().Where(i => i.Name == "addFieldConfiguration").ToList().FirstOrDefault();
                    GridViewColumn addStationFieldConfiguration = gridView.Columns.Cast<GridViewColumn>().Where(i => i.Name == "addStationFieldConfiguration").ToList().FirstOrDefault();
                    GridViewColumn addTestConfiguration = gridView.Columns.Cast<GridViewColumn>().Where(i => i.Name == "addTestConfiguration").ToList().FirstOrDefault();
                    if (addFieldConfiguration != null)
                    {
                        addFieldConfiguration.Width = 40;
                    }
                    else if (addStationFieldConfiguration != null)
                    {
                        addStationFieldConfiguration.Width = 40;
                    }
                    else if (addTestConfiguration != null)
                    {
                        addTestConfiguration.Width = 40;
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
                        }
                    }
                }
                else if (View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView" || View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView_Station" || View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView_Test")
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
            base.OnDeactivated();
            if ((View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView" || View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView_Station" || View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView_Test") && Frame.GetController<ExportController>().ExportAction.Active.Contains("ShowExport"))
            {
                Frame.GetController<ExportController>().ExportAction.Active.RemoveItem("ShowExport");
            }
            if (View.Id == "VisualMatrix_DetailView_FieldConfiguration")
            {
                IsSamplingFieldConfiguration = false;
                IsStationFieldConfiguration = false;
                IsMonitoringFieldConfiguration = false;
                Frame.GetController<RefreshController>().RefreshAction.Executed -= RefreshAction_Executed;
                ModificationsController modificationController = Frame.GetController<ModificationsController>();
                if (modificationController != null)
                {
                    if (modificationController.SaveAndNewAction.Active.Contains("SampleMatrixsavenew.visible"))
                    {
                        modificationController.SaveAndNewAction.Active.RemoveItem("SampleMatrixsavenew.visible");
                    }
                }
            }
        }

        private void addFieldConfiguration_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "NPSamplingFieldConfiguration_ListView")
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView view = nestedFrame.ViewItem.View;
                    DashboardViewItem viAddFields = ((DetailView)view).FindItem("AvailableFields") as DashboardViewItem;
                    DevExpress.ExpressApp.Editors.ListPropertyEditor viSelectedFields = ((DetailView)view).FindItem("SamplingFieldConfiguration") as DevExpress.ExpressApp.Editors.ListPropertyEditor;
                    VisualMatrix setup = view.CurrentObject as VisualMatrix;
                    if (viAddFields != null && viAddFields.InnerView != null && viSelectedFields != null && viSelectedFields.ListView != null && setup != null)
                    {
                        ListView lvAddFields = (ListView)viAddFields.InnerView;
                        ListView lvSelFields = viSelectedFields.ListView;
                        if (lvAddFields != null && lvSelFields != null)
                        {
                            if (lvAddFields.CurrentObject != null)
                            {
                                NPSamplingFieldConfiguration lstAvailSampleFields = (NPSamplingFieldConfiguration)lvAddFields.CurrentObject;
                                if (lstAvailSampleFields != null)
                                {
                                    SamplingFieldConfiguration field = lvSelFields.ObjectSpace.FindObject<SamplingFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [FieldClass] = 'Sampling' And ([VisualMatrix] Is Null Or [VisualMatrix.Oid] = ?)", lstAvailSampleFields.FieldID, setup.Oid));
                                    if (field == null)
                                    {
                                        field = lvSelFields.ObjectSpace.CreateObject<SamplingFieldConfiguration>();
                                        field.FieldID = lstAvailSampleFields.FieldID;
                                        field.FieldCaption = lstAvailSampleFields.FieldCaption;
                                        field.VisualMatrix = lvSelFields.ObjectSpace.GetObject<VisualMatrix>(setup);
                                        field.FieldClass = FieldClass.Sampling;
                                        lvSelFields.CollectionSource.Add(field);
                                        lvAddFields.CollectionSource.Remove(lstAvailSampleFields);
                                    }
                                    else
                                    {
                                        field.FieldCaption = lstAvailSampleFields.FieldCaption;
                                        lvSelFields.CollectionSource.Add(field);
                                        lvAddFields.CollectionSource.Remove(lstAvailSampleFields);
                                    }
                                    lvSelFields.Refresh();
                                    lvAddFields.Refresh();
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
        private void removeFieldConfiguration_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView")
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView view = nestedFrame.ViewItem.View;
                    DashboardViewItem viAddFields = ((DetailView)view).FindItem("AvailableFields") as DashboardViewItem;
                    DevExpress.ExpressApp.Editors.ListPropertyEditor viSelectedFields = ((DetailView)view).FindItem("SamplingFieldConfiguration") as DevExpress.ExpressApp.Editors.ListPropertyEditor;
                    if (viAddFields != null && viAddFields.InnerView != null && viSelectedFields != null && viSelectedFields.ListView != null)
                    {
                        ListView lvAddFields = (ListView)viAddFields.InnerView;
                        ListView lvSelFields = viSelectedFields.ListView;
                        if (lvAddFields != null && lvSelFields != null)
                        {
                            if (lvSelFields.CurrentObject != null)
                            {
                                SamplingFieldConfiguration lstSelSampleFields = (SamplingFieldConfiguration)lvSelFields.CurrentObject;
                                if (lstSelSampleFields != null)
                                {
                                    NPSamplingFieldConfiguration field = lvAddFields.ObjectSpace.CreateObject<NPSamplingFieldConfiguration>();
                                    field.FieldID = lstSelSampleFields.FieldID;
                                    field.FieldCaption = lstSelSampleFields.FieldCaption;
                                    lvSelFields.CollectionSource.Remove(lstSelSampleFields);
                                    lvAddFields.CollectionSource.Add(field);
                                }
                                lvSelFields.Refresh();
                                lvAddFields.Refresh();
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
        private void AddStationFieldConfiguration_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "NPSamplingFieldConfiguration_ListView_Station")
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView view = nestedFrame.ViewItem.View;
                    DashboardViewItem viAddFields = ((DetailView)view).FindItem("AvailableStationFields") as DashboardViewItem;
                    DevExpress.ExpressApp.Editors.ListPropertyEditor viSelectedFields = ((DetailView)view).FindItem("SamplingFieldConfiguration_Station") as DevExpress.ExpressApp.Editors.ListPropertyEditor;
                    VisualMatrix setup = view.CurrentObject as VisualMatrix;
                    if (viAddFields != null && viAddFields.InnerView != null && viSelectedFields != null && viSelectedFields.ListView != null && setup != null)
                    {
                        ListView lvAddFields = (ListView)viAddFields.InnerView;
                        ListView lvSelFields = viSelectedFields.ListView;
                        if (lvAddFields != null && lvSelFields != null)
                        {
                            if (lvAddFields.CurrentObject != null)
                            {
                                NPSamplingFieldConfiguration lstAvailSampleFields = (NPSamplingFieldConfiguration)lvAddFields.CurrentObject;
                                if (lstAvailSampleFields != null)
                                {
                                    SamplingFieldConfiguration field = lvSelFields.ObjectSpace.FindObject<SamplingFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [FieldClass] = 'Station' And ([VisualMatrix] Is Null Or [VisualMatrix.Oid] = ?)", lstAvailSampleFields.FieldID, setup.Oid));
                                    if (field == null)
                                    {
                                        field = lvSelFields.ObjectSpace.CreateObject<SamplingFieldConfiguration>();
                                        field.FieldID = lstAvailSampleFields.FieldID;
                                        field.FieldCaption = lstAvailSampleFields.FieldCaption;
                                        field.VisualMatrix = lvSelFields.ObjectSpace.GetObject<VisualMatrix>(setup);
                                        field.FieldClass = FieldClass.Station;
                                        lvSelFields.CollectionSource.Add(field);
                                        lvAddFields.CollectionSource.Remove(lstAvailSampleFields);
                                    }
                                    else
                                    {
                                        field.FieldCaption = lstAvailSampleFields.FieldCaption;
                                        lvSelFields.CollectionSource.Add(field);
                                        lvAddFields.CollectionSource.Remove(lstAvailSampleFields);
                                    }
                                }
                                lvSelFields.Refresh();
                                lvAddFields.Refresh();
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

        private void RemoveStationFieldConfiguration_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView_Station")
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView view = nestedFrame.ViewItem.View;
                    DashboardViewItem viAddFields = ((DetailView)view).FindItem("AvailableStationFields") as DashboardViewItem;
                    DevExpress.ExpressApp.Editors.ListPropertyEditor viSelectedFields = ((DetailView)view).FindItem("SamplingFieldConfiguration_Station") as DevExpress.ExpressApp.Editors.ListPropertyEditor;
                    if (viAddFields != null && viAddFields.InnerView != null && viSelectedFields != null && viSelectedFields.ListView != null)
                    {
                        ListView lvAddFields = (ListView)viAddFields.InnerView;
                        ListView lvSelFields = viSelectedFields.ListView;
                        if (lvAddFields != null && lvSelFields != null)
                        {
                            if (lvSelFields.CurrentObject != null)
                            {
                                SamplingFieldConfiguration lstSelSampleFields = (SamplingFieldConfiguration)lvSelFields.CurrentObject;
                                if (lstSelSampleFields != null)
                                {
                                    NPSamplingFieldConfiguration field = lvAddFields.ObjectSpace.CreateObject<NPSamplingFieldConfiguration>();
                                    field.FieldID = lstSelSampleFields.FieldID;
                                    field.FieldCaption = lstSelSampleFields.FieldCaption;
                                    lvSelFields.CollectionSource.Remove(lstSelSampleFields);
                                    lvAddFields.CollectionSource.Add(field);
                                }
                                lvSelFields.Refresh();
                                lvAddFields.Refresh();
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
        private void AddTestConfiguration_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "NPSamplingFieldConfiguration_ListView_Test")
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView view = nestedFrame.ViewItem.View;
                    DashboardViewItem viAddFields = ((DetailView)view).FindItem("AvailableTestFields") as DashboardViewItem;
                    DevExpress.ExpressApp.Editors.ListPropertyEditor viSelectedFields = ((DetailView)view).FindItem("SamplingFieldConfiguration_Test") as DevExpress.ExpressApp.Editors.ListPropertyEditor;
                    VisualMatrix setup = view.CurrentObject as VisualMatrix;
                    if (viAddFields != null && viAddFields.InnerView != null && viSelectedFields != null && viSelectedFields.ListView != null && setup != null)
                    {
                        ListView lvAddFields = (ListView)viAddFields.InnerView;
                        ListView lvSelFields = viSelectedFields.ListView;
                        if (lvAddFields != null && lvSelFields != null)
                        {
                            if (lvAddFields.CurrentObject != null)
                            {
                                NPSamplingFieldConfiguration lstAvailSampleFields = (NPSamplingFieldConfiguration)lvAddFields.CurrentObject;
                                if (lstAvailSampleFields != null)
                                {
                                    SamplingFieldConfiguration field = lvSelFields.ObjectSpace.FindObject<SamplingFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [FieldClass] = 'Test' And ([VisualMatrix] Is Null Or [VisualMatrix.Oid] = ?)", lstAvailSampleFields.FieldID, setup.Oid));
                                    if (field == null)
                                    {
                                        field = lvSelFields.ObjectSpace.CreateObject<SamplingFieldConfiguration>();
                                        field.FieldID = lstAvailSampleFields.FieldID;
                                        field.FieldCaption = lstAvailSampleFields.FieldCaption;
                                        field.VisualMatrix = lvSelFields.ObjectSpace.GetObject<VisualMatrix>(setup);
                                        field.FieldClass = FieldClass.Test;
                                        lvSelFields.CollectionSource.Add(field);
                                        lvAddFields.CollectionSource.Remove(lstAvailSampleFields);
                                    }
                                    else
                                    {
                                        field.FieldCaption = lstAvailSampleFields.FieldCaption;
                                        lvSelFields.CollectionSource.Add(field);
                                        lvAddFields.CollectionSource.Remove(lstAvailSampleFields);
                                    }
                                    lvSelFields.Refresh();
                                    lvAddFields.Refresh();
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

        private void RemoveTestConfiguration_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "VisualMatrix_SamplingFieldConfiguration_ListView_Test")
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView view = nestedFrame.ViewItem.View;
                    DashboardViewItem viAddFields = ((DetailView)view).FindItem("AvailableTestFields") as DashboardViewItem;
                    DevExpress.ExpressApp.Editors.ListPropertyEditor viSelectedFields = ((DetailView)view).FindItem("SamplingFieldConfiguration_Test") as DevExpress.ExpressApp.Editors.ListPropertyEditor;
                    if (viAddFields != null && viAddFields.InnerView != null && viSelectedFields != null && viSelectedFields.ListView != null)
                    {
                        ListView lvAddFields = (ListView)viAddFields.InnerView;
                        ListView lvSelFields = viSelectedFields.ListView;
                        if (lvAddFields != null && lvSelFields != null)
                        {
                            if (lvSelFields.CurrentObject != null)
                            {
                                SamplingFieldConfiguration lstSelSampleFields = (SamplingFieldConfiguration)lvSelFields.CurrentObject;
                                if (lstSelSampleFields != null)
                                {
                                    NPSamplingFieldConfiguration field = lvAddFields.ObjectSpace.CreateObject<NPSamplingFieldConfiguration>();
                                    field.FieldID = lstSelSampleFields.FieldID;
                                    field.FieldCaption = lstSelSampleFields.FieldCaption;
                                    lvSelFields.CollectionSource.Remove(lstSelSampleFields);
                                    lvAddFields.CollectionSource.Add(field);
                                }
                                lvSelFields.Refresh();
                                lvAddFields.Refresh();
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
