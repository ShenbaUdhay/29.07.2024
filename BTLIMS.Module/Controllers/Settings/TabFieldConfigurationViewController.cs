using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TabFieldConfigurationViewController : ViewController
    {
        Tabinfo objtabinfo = new Tabinfo();
        MessageTimer timer = new MessageTimer();
        bool IsSampletabFieldsPopulated = false;
        bool IsSurrogateFieldsPopulated = false;
        bool IsInternalStdFieldsPopulated = false;
        bool IsQcParamsFieldsPopulated = false;
        bool IsComponentFieldsPopulated = false;
        private SimpleAction btnSaveTabControls;
        private System.ComponentModel.IContainer components;
        bool IsQcparamsDefaultFieldsPopulated = false;

        public TabFieldConfigurationViewController()
        {
            InitializeComponent();
            TargetViewId = "NpTabfields_ListView;" + "TabFields_ListView_Components;" + "TabFields_ListView_InternalStandards;" + "TabFields_ListView_QCParameterDefaults;" + "TabFields_ListView_QCParameters;"
                    + "TabFields_ListView_SampleParameters;" + "TabFields_ListView_Surrogates;" + "TabFieldConfiguration_DetailView_TestTab;" + "NPTabFieldConfiguration_ListView_AvailableFields;" + "TabControls_ListView;"
                    + "TabFieldConfiguration_ListView_Components;" + "TabFieldConfiguration_ListView_InternalStandards;" + "TabFieldConfiguration_ListView_QCParameterDefaults;" + "TabFieldConfiguration_ListView_QCParameters;"
                    + "TabFieldConfiguration_ListView_SampleParameters;" + "TabFieldConfiguration_ListView_Surrogates;" + "TestMethod_ListView;";
            btnTestConfigAdd.TargetViewId = "TabFieldConfiguration_DetailView_TestTab;";
            btnTestConfigRemove.TargetViewId = "TabFieldConfiguration_DetailView_TestTab;";

            SimpleAction btnSaveTabControls = new SimpleAction(this, "btnSaveTabControls", PredefinedCategory.Unspecified);
            btnSaveTabControls.Caption = "Save";
            btnSaveTabControls.ImageName = "Action_Save";
            btnSaveTabControls.TargetViewId = "TabControls_ListView;";
            btnSaveTabControls.Execute += BtnSaveTabControls_Execute;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        private void BtnSaveTabControls_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TabControls_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.UpdateEdit();
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
                if (View.Id == "TabFieldConfiguration_DetailView_TestTab" || View.Id == "TestMethod_ListView")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    string strtabname = string.Empty;
                    List<string> lststrtabname = new List<string>();
                    List<string> lstExistingFields = new List<string>();
                    lststrtabname.Add("Components");
                    lststrtabname.Add("InternalStandards");
                    lststrtabname.Add("QCParameterDefaults");
                    lststrtabname.Add("QCParameters");
                    lststrtabname.Add("SampleParameters");
                    lststrtabname.Add("Surrogates");
                    foreach (string objstrtabname in lststrtabname.ToList())
                    {
                        List<string> lstcomponentfieldlist = new List<string>();
                        if (objstrtabname == "SampleParameters")
                        {
                            strtabname = objstrtabname;
                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Testparameter_ListView_Test_SampleParameter");
                                    if (lvSamplelogin != null)
                                    {
                                        lstExistingFields = os.GetObjects<TabFields>(CriteriaOperator.Parse("[TabName] = ?", strtabname)).Select(i => i.FieldID).ToList();
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            if (!lstcomponentfieldlist.Contains(col.Id) && col.Index >= 0)
                                            {
                                                TabFieldConfiguration chksledFields = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", col.Id, strtabname));
                                                if (chksledFields == null)
                                                {
                                                    TabFieldConfiguration sledField = os.CreateObject<TabFieldConfiguration>();
                                                    sledField.FieldID = col.Id;
                                                    lstcomponentfieldlist.Add(col.Id);
                                                    sledField.FieldCaption = col.Caption;
                                                    sledField.TabName = strtabname;
                                                    sledField.Width = 100;
                                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                                    sledField.SortOrder = sort;
                                                    //liAvailableFields.CollectionSource.Add(sledField);
                                                    os.CommitChanges();
                                                }
                                            }
                                        }
                                        os.Refresh();
                                    }
                                }
                            }
                        }
                        if (objstrtabname == "Surrogates")
                        {
                            strtabname = objstrtabname;
                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Testparameter_ListView_Test_Surrogates");
                                    if (lvSamplelogin != null)
                                    {
                                        lstExistingFields = os.GetObjects<TabFields>(CriteriaOperator.Parse("[TabName] = ?", strtabname)).Select(i => i.FieldID).ToList();
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            if (!lstcomponentfieldlist.Contains(col.Id) && col.Index >= 0)
                                            {
                                                TabFieldConfiguration chksledFields = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", col.Id, strtabname));
                                                if (chksledFields == null)
                                                {
                                                    TabFieldConfiguration sledField = os.CreateObject<TabFieldConfiguration>();
                                                    sledField.FieldID = col.Id;
                                                    lstcomponentfieldlist.Add(col.Id);
                                                    sledField.FieldCaption = col.Caption;
                                                    sledField.TabName = strtabname;
                                                    sledField.Width = 100;
                                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                                    sledField.SortOrder = sort;
                                                    //liAvailableFields.CollectionSource.Add(sledField);
                                                    os.CommitChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (objstrtabname == "InternalStandards")
                        {
                            strtabname = objstrtabname;
                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Testparameter_ListView_Test_InternalStandards");
                                    if (lvSamplelogin != null)
                                    {
                                        lstExistingFields = os.GetObjects<TabFields>(CriteriaOperator.Parse("[TabName] = ?", strtabname)).Select(i => i.FieldID).ToList();
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            if (!lstcomponentfieldlist.Contains(col.Id) && col.Index >= 0)
                                            {
                                                TabFieldConfiguration chksledFields = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", col.Id, strtabname));
                                                if (chksledFields == null)
                                                {
                                                    TabFieldConfiguration sledField = os.CreateObject<TabFieldConfiguration>();
                                                    sledField.FieldID = col.Id;
                                                    lstcomponentfieldlist.Add(col.Id);
                                                    sledField.FieldCaption = col.Caption;
                                                    sledField.TabName = strtabname;
                                                    sledField.Width = 100;
                                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                                    sledField.SortOrder = sort;
                                                    //liAvailableFields.CollectionSource.Add(sledField);
                                                    os.CommitChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (objstrtabname == "QCParameters")
                        {
                            strtabname = objstrtabname;
                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Testparameter_ListView_Test_QCSampleParameter");
                                    if (lvSamplelogin != null)
                                    {
                                        lstExistingFields = os.GetObjects<TabFields>(CriteriaOperator.Parse("[TabName] = ?", strtabname)).Select(i => i.FieldID).ToList();
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            if (!lstcomponentfieldlist.Contains(col.Id) && col.Index >= 0)
                                            {
                                                TabFieldConfiguration chksledFields = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", col.Id, strtabname));
                                                if (chksledFields == null)
                                                {
                                                    TabFieldConfiguration sledField = os.CreateObject<TabFieldConfiguration>();
                                                    sledField.FieldID = col.Id;
                                                    lstcomponentfieldlist.Add(col.Id);
                                                    sledField.FieldCaption = col.Caption;
                                                    sledField.TabName = strtabname;
                                                    sledField.Width = 100;
                                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                                    sledField.SortOrder = sort;
                                                    //liAvailableFields.CollectionSource.Add(sledField);
                                                    os.CommitChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (objstrtabname == "Components")
                        {
                            strtabname = objstrtabname;
                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Component_ListView_Test");
                                    if (lvSamplelogin != null)
                                    {
                                        lstExistingFields = os.GetObjects<TabFields>(CriteriaOperator.Parse("[TabName] = ?", strtabname)).Select(i => i.FieldID).ToList();
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            if (!lstcomponentfieldlist.Contains(col.Id) && col.Index >= 0)
                                            {
                                                TabFieldConfiguration chksledFields = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", col.Id, strtabname));
                                                if (chksledFields == null)
                                                {
                                                    TabFieldConfiguration sledField = os.CreateObject<TabFieldConfiguration>();
                                                    sledField.FieldID = col.Id;
                                                    lstcomponentfieldlist.Add(col.Id);
                                                    sledField.FieldCaption = col.Caption;
                                                    sledField.TabName = strtabname;
                                                    sledField.Width = 100;
                                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                                    sledField.SortOrder = sort;
                                                    //liAvailableFields.CollectionSource.Add(sledField);
                                                    os.CommitChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (objstrtabname == "QCParameterDefaults")
                        {
                            strtabname = objstrtabname;
                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault");
                                    if (lvSamplelogin != null)
                                    {
                                        lstExistingFields = os.GetObjects<TabFields>(CriteriaOperator.Parse("[TabName] = ?", strtabname)).Select(i => i.FieldID).ToList();
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            if (!lstcomponentfieldlist.Contains(col.Id) && col.Index >= 0)
                                            {
                                                TabFieldConfiguration chksledFields = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", col.Id, strtabname));
                                                if (chksledFields == null)
                                                {
                                                    TabFieldConfiguration sledField = os.CreateObject<TabFieldConfiguration>();
                                                    sledField.FieldID = col.Id;
                                                    lstcomponentfieldlist.Add(col.Id);
                                                    sledField.FieldCaption = col.Caption;
                                                    sledField.TabName = strtabname;
                                                    sledField.Width = 100;
                                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                                    sledField.SortOrder = sort;
                                                    //liAvailableFields.CollectionSource.Add(sledField);
                                                    os.CommitChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                if (View.Id == "TabFieldConfiguration_DetailView_TestTab")
                {
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("SaveAndCloseAction", false);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("ShowSaveAndNewAction", false);
                    Frame.GetController<ModificationsController>().SaveAction.Execute += SaveAction_Execute;
                    ////DashboardViewItem viAvailableFieldsComponents = ((DetailView)View).FindItem("DVAvailableFields_Components") as DashboardViewItem;
                    ////if (viAvailableFieldsComponents != null)
                    ////{
                    ////    viAvailableFieldsComponents.ControlCreated += viAvailableFieldsComponents_ControlCreated;
                    ////}
                    ////DashboardViewItem viAvailableFieldsInternalStandards = ((DetailView)View).FindItem("DVAvailableFields_InternalStandards") as DashboardViewItem;
                    ////if (viAvailableFieldsInternalStandards != null)
                    ////{
                    ////    viAvailableFieldsInternalStandards.ControlCreated += viAvailableFieldsInternalStandards_ControlCreated;
                    ////}
                    ////DashboardViewItem viAvailableFieldsQCParameterDefaults = ((DetailView)View).FindItem("DVAvailableFields_QCParameterDefaults") as DashboardViewItem;
                    ////if (viAvailableFieldsQCParameterDefaults != null)
                    ////{
                    ////    viAvailableFieldsQCParameterDefaults.ControlCreated += viAvailableFieldsQCParameterDefaults_ControlCreated;
                    ////}
                    ////DashboardViewItem viAvailableFieldsQCParameters = ((DetailView)View).FindItem("DVAvailableFields_QCParameters") as DashboardViewItem;
                    ////if (viAvailableFieldsQCParameters != null)
                    ////{
                    ////    viAvailableFieldsQCParameters.ControlCreated += viAvailableFieldsQCParameters_ControlCreated;
                    ////}
                    ////DashboardViewItem viAvailableFieldsSampleParameters = ((DetailView)View).FindItem("DVAvailableFields_SampleParameters") as DashboardViewItem;
                    ////if (viAvailableFieldsSampleParameters != null)
                    ////{
                    ////    viAvailableFieldsSampleParameters.ControlCreated += viAvailableFieldsSampleParameters_ControlCreated;
                    ////}
                    ////DashboardViewItem viAvailableFieldsSurrogates = ((DetailView)View).FindItem("DVAvailableFields_Surrogates") as DashboardViewItem;
                    ////if (viAvailableFieldsSurrogates != null)
                    ////{
                    ////    viAvailableFieldsSurrogates.ControlCreated += viAvailableFieldsSurrogates_ControlCreated;
                    ////}
                }
                if (View.Id == "TabControls_ListView" || View.Id == "TestMethod_ListView")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    List<string> strtablist = new List<string>();
                    strtablist.Add("Sampling Method");
                    strtablist.Add("Surrogates");
                    strtablist.Add("Internal Standards");
                    strtablist.Add("Components");
                    foreach (string strtabname in strtablist.ToList())
                    {
                        TabControls objtabcontrol = os.FindObject<TabControls>(CriteriaOperator.Parse("[TabName] = ?", strtabname));
                        if (objtabcontrol == null)
                        {
                            TabControls objcrttabcontrol = os.CreateObject<TabControls>();
                            objcrttabcontrol.TabName = strtabname;
                            objcrttabcontrol.IsVisible = true;
                            os.CommitChanges();
                            os.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }

        private void SaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem dvselectedcomp = ((DetailView)View).FindItem("DVComponents") as DashboardViewItem;
                if (dvselectedcomp != null && dvselectedcomp.InnerView == null)
                {
                    dvselectedcomp.CreateControl();
                }
                if (dvselectedcomp != null && dvselectedcomp.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)dvselectedcomp.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
                DashboardViewItem viAvailableFieldscomp = ((DetailView)View).FindItem("DVAvailableFields_Components") as DashboardViewItem;
                if (viAvailableFieldscomp != null && viAvailableFieldscomp.InnerView == null)
                {
                    viAvailableFieldscomp.CreateControl();
                }
                if (viAvailableFieldscomp != null && viAvailableFieldscomp.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)viAvailableFieldscomp.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
                DashboardViewItem dvselectedInternalstd = ((DetailView)View).FindItem("DVInternalStandards") as DashboardViewItem;
                if (dvselectedInternalstd != null && dvselectedInternalstd.InnerView == null)
                {
                    dvselectedInternalstd.CreateControl();
                }
                if (dvselectedInternalstd != null && dvselectedInternalstd.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)dvselectedInternalstd.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
                DashboardViewItem viAvailableFieldsInternalstd = ((DetailView)View).FindItem("DVAvailableFields_InternalStandards") as DashboardViewItem;
                if (viAvailableFieldsInternalstd != null && viAvailableFieldsInternalstd.InnerView == null)
                {
                    viAvailableFieldsInternalstd.CreateControl();
                }
                if (viAvailableFieldsInternalstd != null && viAvailableFieldsInternalstd.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)viAvailableFieldsInternalstd.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
                DashboardViewItem dvselectedQcparamsdef = ((DetailView)View).FindItem("DVQCParameterDefaults") as DashboardViewItem;
                if (dvselectedQcparamsdef != null && dvselectedQcparamsdef.InnerView == null)
                {
                    dvselectedQcparamsdef.CreateControl();
                }
                if (dvselectedQcparamsdef != null && dvselectedQcparamsdef.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)dvselectedQcparamsdef.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
                DashboardViewItem viAvailableFieldsQcparamsdef = ((DetailView)View).FindItem("DVAvailableFields_QCParameterDefaults") as DashboardViewItem;
                if (viAvailableFieldsQcparamsdef != null && viAvailableFieldsQcparamsdef.InnerView == null)
                {
                    viAvailableFieldsQcparamsdef.CreateControl();
                }
                if (viAvailableFieldsQcparamsdef != null && viAvailableFieldsQcparamsdef.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)viAvailableFieldsQcparamsdef.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
                DashboardViewItem dvselectedqcparams = ((DetailView)View).FindItem("DVQCParameters") as DashboardViewItem;
                if (dvselectedqcparams != null && dvselectedqcparams.InnerView == null)
                {
                    dvselectedqcparams.CreateControl();
                }
                if (dvselectedqcparams != null && dvselectedqcparams.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)dvselectedqcparams.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
                DashboardViewItem viAvailableFieldsqcparams = ((DetailView)View).FindItem("DVAvailableFields_QCParameters") as DashboardViewItem;
                if (viAvailableFieldsqcparams != null && viAvailableFieldsqcparams.InnerView == null)
                {
                    viAvailableFieldsqcparams.CreateControl();
                }
                if (viAvailableFieldsqcparams != null && viAvailableFieldsqcparams.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)viAvailableFieldsqcparams.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
                DashboardViewItem dvselectedsmpleparams = ((DetailView)View).FindItem("DVSampleParameters") as DashboardViewItem;
                if (dvselectedsmpleparams != null && dvselectedsmpleparams.InnerView == null)
                {
                    dvselectedsmpleparams.CreateControl();
                }
                if (dvselectedsmpleparams != null && dvselectedsmpleparams.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)dvselectedsmpleparams.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
                DashboardViewItem viAvailableFieldssmpleparams = ((DetailView)View).FindItem("DVAvailableFields_SampleParameters") as DashboardViewItem;
                if (viAvailableFieldssmpleparams != null && viAvailableFieldssmpleparams.InnerView == null)
                {
                    viAvailableFieldssmpleparams.CreateControl();
                }
                if (viAvailableFieldssmpleparams != null && viAvailableFieldssmpleparams.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)viAvailableFieldssmpleparams.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
                DashboardViewItem dvselectedsurrogates = ((DetailView)View).FindItem("DvSurrogates") as DashboardViewItem;
                if (dvselectedsurrogates != null && dvselectedsurrogates.InnerView == null)
                {
                    dvselectedsurrogates.CreateControl();
                }
                if (dvselectedsurrogates != null && dvselectedsurrogates.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)dvselectedsurrogates.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
                DashboardViewItem viAvailableFieldssurrogates = ((DetailView)View).FindItem("DVAvailableFields_Surrogates") as DashboardViewItem;
                if (viAvailableFieldssurrogates != null && viAvailableFieldssurrogates.InnerView == null)
                {
                    viAvailableFieldssurrogates.CreateControl();
                }
                if (viAvailableFieldssurrogates != null && viAvailableFieldssurrogates.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)viAvailableFieldssurrogates.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void viAvailableFieldsComponents_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                string strtabname = string.Empty;
                if (!string.IsNullOrEmpty(objtabinfo.strtabname))
                {
                    strtabname = objtabinfo.strtabname;
                }
                else
                {
                    strtabname = "Components";
                }
                if (strtabname == "Components")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    List<string> lstcomponentfieldlist = new List<string>();
                    List<string> lstExistingFields = new List<string>();
                    DashboardViewItem dvselected = ((DetailView)View).FindItem("DVAvailableFields_Components") as DashboardViewItem;
                    DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("DVAvailableFields_Components") as DashboardViewItem;
                    if (viAvailableFields != null && viAvailableFields.InnerView == null)
                    {
                        viAvailableFields.CreateControl();
                    }
                    if (dvselected != null && dvselected.InnerView == null)
                    {
                        dvselected.CreateControl();
                    }
                    if (viAvailableFields != null && viAvailableFields.InnerView != null)
                    {
                        lstExistingFields = ((ListView)viAvailableFields.InnerView).CollectionSource.List.Cast<TabFields>().Select(i => i.FieldID).ToList();
                    }
                    if (dvselected != null && dvselected.InnerView != null && dvselected.InnerView is ListView && IsComponentFieldsPopulated == false)
                    {
                        ListView liAvailableFields = dvselected.InnerView as ListView;
                        if (liAvailableFields != null)
                        {

                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Component_ListView_Test");
                                    if (lvSamplelogin != null)
                                    {
                                        {
                                            lstExistingFields.Add("Oid");
                                            lstExistingFields.Add("ModifiedBy");
                                            lstExistingFields.Add("ModifiedDate");
                                        }
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            if (!lstcomponentfieldlist.Contains(col.Id) && col.Index >= 0)
                                            {
                                                TabFieldConfiguration chksledFields = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", col.Id, strtabname));
                                                if (chksledFields == null)
                                                {
                                                    TabFieldConfiguration sledField = os.CreateObject<TabFieldConfiguration>();
                                                    sledField.FieldID = col.Id;
                                                    lstcomponentfieldlist.Add(col.Id);
                                                    sledField.FieldCaption = col.Caption;
                                                    sledField.TabName = strtabname;
                                                    sledField.Width = 100;
                                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                                    sledField.SortOrder = sort;
                                                    //liAvailableFields.CollectionSource.Add(sledField);
                                                    os.CommitChanges();
                                                }
                                            }
                                        }
                                        dvselected.InnerView.Refresh();
                                        viAvailableFields.InnerView.Refresh();
                                        IsComponentFieldsPopulated = true;
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
        private void viAvailableFieldsInternalStandards_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                string strtabname = string.Empty;
                if (!string.IsNullOrEmpty(objtabinfo.strtabname))
                {
                    strtabname = objtabinfo.strtabname;
                }
                else
                {
                    strtabname = "InternalStandards";
                }
                if (strtabname == "InternalStandards")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    List<string> lstcomponentfieldlist = new List<string>();
                    List<string> lstExistingFields = new List<string>();
                    DashboardViewItem dvselected = ((DetailView)View).FindItem("DVAvailableFields_InternalStandards") as DashboardViewItem;
                    DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("DVAvailableFields_InternalStandards") as DashboardViewItem;
                    if (viAvailableFields != null && viAvailableFields.InnerView == null)
                    {
                        viAvailableFields.CreateControl();
                    }
                    if (dvselected != null && dvselected.InnerView == null)
                    {
                        dvselected.CreateControl();
                    }
                    if (viAvailableFields != null && viAvailableFields.InnerView != null)
                    {
                        lstExistingFields = ((ListView)viAvailableFields.InnerView).CollectionSource.List.Cast<TabFields>().Select(i => i.FieldID).ToList();
                    }
                    if (dvselected != null && dvselected.InnerView != null && dvselected.InnerView is ListView && IsInternalStdFieldsPopulated == false)
                    {
                        ListView liAvailableFields = dvselected.InnerView as ListView;
                        if (liAvailableFields != null)
                        {
                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Testparameter_ListView_Test_InternalStandards");
                                    if (lvSamplelogin != null)
                                    {
                                        {
                                            lstExistingFields.Add("Oid");
                                            lstExistingFields.Add("ModifiedBy");
                                            lstExistingFields.Add("ModifiedDate");
                                        }
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            if (!lstcomponentfieldlist.Contains(col.Id) && col.Index >= 0)
                                            {
                                                TabFieldConfiguration chksledFields = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", col.Id, strtabname));
                                                if (chksledFields == null)
                                                {
                                                    TabFieldConfiguration sledField = os.CreateObject<TabFieldConfiguration>();
                                                    sledField.FieldID = col.Id;
                                                    lstcomponentfieldlist.Add(col.Id);
                                                    sledField.FieldCaption = col.Caption;
                                                    sledField.TabName = strtabname;
                                                    sledField.Width = 100;
                                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                                    sledField.SortOrder = sort;
                                                    //liAvailableFields.CollectionSource.Add(sledField);
                                                    os.CommitChanges();
                                                }
                                            }
                                        }
                                        dvselected.InnerView.Refresh();
                                        viAvailableFields.InnerView.Refresh();
                                        IsInternalStdFieldsPopulated = true;
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
        private void viAvailableFieldsQCParameterDefaults_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                string strtabname = string.Empty;
                if (!string.IsNullOrEmpty(objtabinfo.strtabname))
                {
                    strtabname = objtabinfo.strtabname;
                }
                else
                {
                    strtabname = "QCParameterDefaults";
                }
                if (strtabname == "QCParameterDefaults")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    List<string> lstcomponentfieldlist = new List<string>();
                    List<string> lstExistingFields = new List<string>();
                    DashboardViewItem dvselected = ((DetailView)View).FindItem("DVQCParameterDefaults") as DashboardViewItem;
                    DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("DVAvailableFields_QCParameterDefaults") as DashboardViewItem;
                    if (viAvailableFields != null && viAvailableFields.InnerView == null)
                    {
                        viAvailableFields.CreateControl();
                    }
                    if (dvselected != null && dvselected.InnerView == null)
                    {
                        dvselected.CreateControl();
                    }
                    if (viAvailableFields != null && viAvailableFields.InnerView != null)
                    {
                        lstExistingFields = ((ListView)viAvailableFields.InnerView).CollectionSource.List.Cast<TabFields>().Select(i => i.FieldID).ToList();
                    }
                    if (dvselected != null && dvselected.InnerView != null && dvselected.InnerView is ListView && IsQcparamsDefaultFieldsPopulated == false)
                    {
                        ListView liAvailableFields = dvselected.InnerView as ListView;
                        if (liAvailableFields != null)
                        {

                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault");
                                    if (lvSamplelogin != null)
                                    {
                                        {
                                            lstExistingFields.Add("Oid");
                                            lstExistingFields.Add("ModifiedBy");
                                            lstExistingFields.Add("ModifiedDate");
                                        }
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            if (!lstcomponentfieldlist.Contains(col.Id) && col.Index >= 0)
                                            {
                                                TabFieldConfiguration chksledFields = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", col.Id, strtabname));
                                                if (chksledFields == null)
                                                {
                                                    TabFieldConfiguration sledField = os.CreateObject<TabFieldConfiguration>();
                                                    sledField.FieldID = col.Id;
                                                    lstcomponentfieldlist.Add(col.Id);
                                                    sledField.FieldCaption = col.Caption;
                                                    sledField.TabName = strtabname;
                                                    sledField.Width = 100;
                                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                                    sledField.SortOrder = sort;
                                                    //liAvailableFields.CollectionSource.Add(sledField);
                                                    os.CommitChanges();
                                                }
                                            }
                                        }
                                        dvselected.InnerView.Refresh();
                                        viAvailableFields.InnerView.Refresh();
                                        IsQcparamsDefaultFieldsPopulated = true;
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
        private void viAvailableFieldsQCParameters_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                string strtabname = string.Empty;
                if (!string.IsNullOrEmpty(objtabinfo.strtabname))
                {
                    strtabname = objtabinfo.strtabname;
                }
                else
                {
                    strtabname = "QCParameters";
                }
                if (strtabname == "QCParameters")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    List<string> lstcomponentfieldlist = new List<string>();
                    List<string> lstExistingFields = new List<string>();
                    DashboardViewItem dvselected = ((DetailView)View).FindItem("DVQCParameters") as DashboardViewItem;
                    DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("DVAvailableFields_QCParameters") as DashboardViewItem;
                    if (viAvailableFields != null && viAvailableFields.InnerView == null)
                    {
                        viAvailableFields.CreateControl();
                    }
                    if (dvselected != null && dvselected.InnerView == null)
                    {
                        dvselected.CreateControl();
                    }
                    if (viAvailableFields != null && viAvailableFields.InnerView != null)
                    {
                        lstExistingFields = ((ListView)viAvailableFields.InnerView).CollectionSource.List.Cast<TabFields>().Select(i => i.FieldID).ToList();
                    }
                    if (dvselected != null && dvselected.InnerView != null && dvselected.InnerView is ListView && IsQcParamsFieldsPopulated == false)
                    {
                        ListView liAvailableFields = dvselected.InnerView as ListView;
                        if (liAvailableFields != null)
                        {
                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Testparameter_ListView_Test_QCSampleParameter");
                                    if (lvSamplelogin != null)
                                    {
                                        {
                                            lstExistingFields.Add("Oid");
                                            lstExistingFields.Add("ModifiedBy");
                                            lstExistingFields.Add("ModifiedDate");
                                        }
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            if (!lstcomponentfieldlist.Contains(col.Id) && col.Index >= 0)
                                            {
                                                TabFieldConfiguration chksledFields = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", col.Id, strtabname));
                                                if (chksledFields == null)
                                                {
                                                    TabFieldConfiguration sledField = os.CreateObject<TabFieldConfiguration>();
                                                    sledField.FieldID = col.Id;
                                                    lstcomponentfieldlist.Add(col.Id);
                                                    sledField.FieldCaption = col.Caption;
                                                    sledField.TabName = strtabname;
                                                    sledField.Width = 100;
                                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                                    sledField.SortOrder = sort;
                                                    //liAvailableFields.CollectionSource.Add(sledField);
                                                    os.CommitChanges();
                                                }
                                            }
                                        }
                                        dvselected.InnerView.Refresh();
                                        viAvailableFields.InnerView.Refresh();
                                        IsQcParamsFieldsPopulated = true;
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
        private void viAvailableFieldsSampleParameters_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                string strtabname = string.Empty;
                if (!string.IsNullOrEmpty(objtabinfo.strtabname))
                {
                    strtabname = objtabinfo.strtabname;
                }
                else
                {
                    strtabname = "SampleParameters";
                }
                if (strtabname == "SampleParameters")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    List<string> lstcomponentfieldlist = new List<string>();
                    List<string> lstExistingFields = new List<string>();
                    DashboardViewItem dvselected = ((DetailView)View).FindItem("DVSampleParameters") as DashboardViewItem;
                    DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("DVAvailableFields_SampleParameters") as DashboardViewItem;
                    if (viAvailableFields != null && viAvailableFields.InnerView == null)
                    {
                        viAvailableFields.CreateControl();
                    }
                    if (dvselected != null && dvselected.InnerView == null)
                    {
                        dvselected.CreateControl();
                    }
                    if (viAvailableFields != null && viAvailableFields.InnerView != null)
                    {
                        lstExistingFields = ((ListView)viAvailableFields.InnerView).CollectionSource.List.Cast<TabFields>().Select(i => i.FieldID).ToList();
                    }
                    if (dvselected != null && dvselected.InnerView != null && dvselected.InnerView is ListView && IsSampletabFieldsPopulated == false)
                    {
                        ListView liAvailableFields = dvselected.InnerView as ListView;
                        if (liAvailableFields != null)
                        {

                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Testparameter_ListView_Test_SampleParameter");
                                    if (lvSamplelogin != null)
                                    {
                                        {
                                            lstExistingFields.Add("Oid");
                                            lstExistingFields.Add("ModifiedBy");
                                            lstExistingFields.Add("ModifiedDate");
                                        }
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            if (!lstcomponentfieldlist.Contains(col.Id) && col.Index >= 0)
                                            {
                                                TabFieldConfiguration chksledFields = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", col.Id, strtabname));
                                                if (chksledFields == null)
                                                {
                                                    TabFieldConfiguration sledField = os.CreateObject<TabFieldConfiguration>();
                                                    sledField.FieldID = col.Id;
                                                    lstcomponentfieldlist.Add(col.Id);
                                                    sledField.FieldCaption = col.Caption;
                                                    sledField.TabName = strtabname;
                                                    sledField.Width = 100;
                                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                                    sledField.SortOrder = sort;
                                                    //liAvailableFields.CollectionSource.Add(sledField);
                                                    os.CommitChanges();
                                                }
                                            }
                                        }
                                        dvselected.InnerView.Refresh();
                                        viAvailableFields.InnerView.Refresh();
                                        IsSampletabFieldsPopulated = true;
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
        private void viAvailableFieldsSurrogates_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                string strtabname = string.Empty;
                if (!string.IsNullOrEmpty(objtabinfo.strtabname))
                {
                    strtabname = objtabinfo.strtabname;
                }
                else
                {
                    strtabname = "Surrogates";
                }
                if (strtabname == "Surrogates")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    List<string> lstcomponentfieldlist = new List<string>();
                    List<string> lstExistingFields = new List<string>();
                    DashboardViewItem dvselected = ((DetailView)View).FindItem("DvSurrogates") as DashboardViewItem;
                    DashboardViewItem viAvailableFields = ((DetailView)View).FindItem("DVAvailableFields_Surrogates") as DashboardViewItem;
                    if (viAvailableFields != null && viAvailableFields.InnerView == null)
                    {
                        viAvailableFields.CreateControl();
                    }
                    if (dvselected != null && dvselected.InnerView == null)
                    {
                        dvselected.CreateControl();
                    }
                    if (viAvailableFields != null && viAvailableFields.InnerView != null)
                    {
                        lstExistingFields = ((ListView)viAvailableFields.InnerView).CollectionSource.List.Cast<TabFields>().Select(i => i.FieldID).ToList();
                    }
                    if (dvselected != null && dvselected.InnerView != null && dvselected.InnerView is ListView && IsSurrogateFieldsPopulated == false)
                    {
                        ListView liAvailableFields = dvselected.InnerView as ListView;
                        if (liAvailableFields != null)
                        {

                            DevExpress.ExpressApp.Model.IModelApplication model = DevExpress.ExpressApp.Web.WebApplication.Instance.Model;
                            if (model != null)
                            {
                                DevExpress.ExpressApp.Model.IModelViews lstViews = model.Views;
                                if (lstViews != null)
                                {
                                    DevExpress.ExpressApp.Model.IModelListView lvSamplelogin = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == "Testparameter_ListView_Test_Surrogates");
                                    if (lvSamplelogin != null)
                                    {
                                        {
                                            lstExistingFields.Add("Oid");
                                            lstExistingFields.Add("ModifiedBy");
                                            lstExistingFields.Add("ModifiedDate");
                                        }
                                        foreach (DevExpress.ExpressApp.Model.IModelColumn col in lvSamplelogin.Columns.Where(i => !lstExistingFields.Contains(i.Id)))
                                        {
                                            if (!lstcomponentfieldlist.Contains(col.Id) && col.Index >= 0)
                                            {
                                                TabFieldConfiguration chksledFields = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", col.Id, strtabname));
                                                if (chksledFields == null)
                                                {
                                                    TabFieldConfiguration sledField = os.CreateObject<TabFieldConfiguration>();
                                                    sledField.FieldID = col.Id;
                                                    lstcomponentfieldlist.Add(col.Id);
                                                    sledField.FieldCaption = col.Caption;
                                                    sledField.TabName = strtabname;
                                                    sledField.Width = 100;
                                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                                    sledField.SortOrder = sort;
                                                    //liAvailableFields.CollectionSource.Add(sledField);
                                                    os.CommitChanges();
                                                }
                                            }
                                        }
                                        dvselected.InnerView.Refresh();
                                        viAvailableFields.InnerView.Refresh();
                                        IsSurrogateFieldsPopulated = true;
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

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View != null && View.GetType() == typeof(TabFields) && View is ListView)
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor.Grid.Columns["FieldID"] != null)
                    {
                        gridListEditor.Grid.Columns["FieldID"].Width = 120;
                    }
                }
                if (View is ListView)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.Settings.ShowStatusBar = DevExpress.Web.GridViewStatusBarMode.Hidden;
                    }
                }
                if (View.Id == "TabFields_ListView_Components" || View.Id == "TabFields_ListView_InternalStandards" || View.Id == "TabFields_ListView_QCParameterDefaults" || View.Id == "TabFields_ListView_QCParameters"
                    || View.Id == "TabFields_ListView_SampleParameters" || View.Id == "TabFields_ListView_Surrogates")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        gridlisteditor.Grid.SettingsPager.Mode = GridViewPagerMode.EndlessPaging;
                        gridlisteditor.Grid.Settings.ShowStatusBar = DevExpress.Web.GridViewStatusBarMode.Hidden;
                        gridlisteditor.Grid.Settings.VerticalScrollableHeight = 363;
                        gridlisteditor.Grid.Settings.VerticalScrollBarMode = DevExpress.Web.ScrollBarMode.Visible;
                        gridlisteditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                            {
                            var nav = document.getElementById('LPcell');
                            var sep = document.getElementById('separatorCell');
                            if(nav != null && sep != null) {
                               var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                               s.SetWidth((totusablescr / 100) * 20); 
                            }
                            else {
                                
                                s.SetWidth(180); 
                            }                        
                        }";
                    }
                }
                if (View.Id == "TabFieldConfiguration_ListView_Components" || View.Id == "TabFieldConfiguration_ListView_InternalStandards" || View.Id == "TabFieldConfiguration_ListView_QCParameterDefaults"
                    || View.Id == "TabFieldConfiguration_ListView_QCParameters" || View.Id == "TabFieldConfiguration_ListView_SampleParameters" || View.Id == "TabFieldConfiguration_ListView_Surrogates")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.Settings.ShowStatusBar = DevExpress.Web.GridViewStatusBarMode.Hidden;
                        gridlisteditor.Grid.Settings.VerticalScrollableHeight = 325;
                        gridlisteditor.Grid.Settings.VerticalScrollBarMode = DevExpress.Web.ScrollBarMode.Visible;
                        gridlisteditor.Grid.ClientSideEvents.Init = @"function(s,e) 
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
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            try
            {
                IsSampletabFieldsPopulated = false;
                IsSurrogateFieldsPopulated = false;
                IsInternalStdFieldsPopulated = false;
                IsQcParamsFieldsPopulated = false;
                IsComponentFieldsPopulated = false;
                IsQcparamsDefaultFieldsPopulated = false;
                objtabinfo.strtabname = string.Empty;
                if (View.Id == "TabFieldConfiguration_DetailView_TestTab")
                {
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("SaveAndCloseAction", true);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("ShowSaveAndNewAction", true);
                    //Frame.GetController<ModificationsController>().SaveAction.Active.SetItemValue("ShowSaveAndNewAction", true);
                    //Frame.GetController<ModificationsController>().CancelAction.Active.SetItemValue("Showcancel", true);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void BtnTestConfigRemove_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TabFieldConfiguration_DetailView_TestTab")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    string strtabname = string.Empty;
                    if (objtabinfo.strtabname == "SampleParameters" || string.IsNullOrEmpty(objtabinfo.strtabname))
                    {
                        objtabinfo.strtabname = "SampleParameters";
                        strtabname = objtabinfo.strtabname;
                        DashboardViewItem dvavailable = ((DetailView)View).FindItem("DVAvailableFields_SampleParameters") as DashboardViewItem;
                        DashboardViewItem dvselected = ((DetailView)View).FindItem("DVSampleParameters") as DashboardViewItem;
                        if (dvavailable != null && dvavailable.InnerView == null)
                        {
                            dvavailable.CreateControl();
                        }
                        if (dvselected != null && dvselected.InnerView == null)
                        {
                            dvselected.CreateControl();

                        }
                        if (dvavailable != null && dvavailable.InnerView != null && dvselected != null && dvselected.InnerView != null && dvselected.InnerView.SelectedObjects.Count > 0)
                        {
                            List<string> lststrfieldid = new List<string>();
                            foreach (TabFieldConfiguration objseltabfield in dvselected.InnerView.SelectedObjects)
                            {
                                TabFields objtabfields = os.FindObject<TabFields>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", objseltabfield.FieldID, strtabname));
                                if (objtabfields == null)
                                {
                                    TabFields crttabfield = os.CreateObject<TabFields>();
                                    crttabfield.FieldID = objseltabfield.FieldID;
                                    crttabfield.FieldCaption = objseltabfield.FieldCaption;
                                    crttabfield.TabName = objtabinfo.strtabname;
                                    os.CommitChanges();
                                    lststrfieldid.Add(objseltabfield.FieldID);
                                    dvselected.InnerView.ObjectSpace.Delete(objseltabfield);
                                    ((ListView)dvavailable.InnerView).CollectionSource.Add(dvavailable.InnerView.ObjectSpace.GetObject(crttabfield));
                                }
                            }
                            dvselected.InnerView.ObjectSpace.CommitChanges();
                            ((ListView)dvavailable.InnerView).Refresh();
                            ((ListView)dvselected.InnerView).Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectSelectedFieldData"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    if (objtabinfo.strtabname == "Surrogates")
                    {
                        strtabname = objtabinfo.strtabname;
                        DashboardViewItem dvavailable = ((DetailView)View).FindItem("DVAvailableFields_Surrogates") as DashboardViewItem;
                        DashboardViewItem dvselected = ((DetailView)View).FindItem("DvSurrogates") as DashboardViewItem;
                        if (dvavailable != null && dvavailable.InnerView == null)
                        {
                            dvavailable.CreateControl();
                        }
                        if (dvselected != null && dvselected.InnerView == null)
                        {
                            dvselected.CreateControl();

                        }
                        if (dvavailable != null && dvavailable.InnerView != null && dvselected != null && dvselected.InnerView != null && dvselected.InnerView.SelectedObjects.Count > 0)
                        {
                            List<string> lststrfieldid = new List<string>();
                            foreach (TabFieldConfiguration objseltabfield in dvselected.InnerView.SelectedObjects)
                            {
                                TabFields objtabfields = os.FindObject<TabFields>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", objseltabfield.FieldID, strtabname));
                                if (objtabfields == null)
                                {
                                    TabFields crttabfield = os.CreateObject<TabFields>();
                                    crttabfield.FieldID = objseltabfield.FieldID;
                                    crttabfield.FieldCaption = objseltabfield.FieldCaption;
                                    crttabfield.TabName = objtabinfo.strtabname;
                                    os.CommitChanges();
                                    lststrfieldid.Add(objseltabfield.FieldID);
                                    dvselected.InnerView.ObjectSpace.Delete(objseltabfield);
                                    ((ListView)dvavailable.InnerView).CollectionSource.Add(dvavailable.InnerView.ObjectSpace.GetObject(crttabfield));
                                }
                            }
                            dvselected.InnerView.ObjectSpace.CommitChanges();
                            ((ListView)dvavailable.InnerView).Refresh();
                            ((ListView)dvselected.InnerView).Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectSelectedFieldData"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    if (objtabinfo.strtabname == "InternalStandards")
                    {
                        strtabname = objtabinfo.strtabname;
                        DashboardViewItem dvavailable = ((DetailView)View).FindItem("DVAvailableFields_InternalStandards") as DashboardViewItem;
                        DashboardViewItem dvselected = ((DetailView)View).FindItem("DVInternalStandards") as DashboardViewItem;
                        if (dvavailable != null && dvavailable.InnerView == null)
                        {
                            dvavailable.CreateControl();
                        }
                        if (dvselected != null && dvselected.InnerView == null)
                        {
                            dvselected.CreateControl();

                        }
                        if (dvavailable != null && dvavailable.InnerView != null && dvselected != null && dvselected.InnerView != null && dvselected.InnerView.SelectedObjects.Count > 0)
                        {
                            List<string> lststrfieldid = new List<string>();
                            foreach (TabFieldConfiguration objseltabfield in dvselected.InnerView.SelectedObjects)
                            {
                                TabFields objtabfields = os.FindObject<TabFields>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", objseltabfield.FieldID, strtabname));
                                if (objtabfields == null)
                                {
                                    TabFields crttabfield = os.CreateObject<TabFields>();
                                    crttabfield.FieldID = objseltabfield.FieldID;
                                    crttabfield.FieldCaption = objseltabfield.FieldCaption;
                                    crttabfield.TabName = objtabinfo.strtabname;
                                    os.CommitChanges();
                                    lststrfieldid.Add(objseltabfield.FieldID);
                                    dvselected.InnerView.ObjectSpace.Delete(objseltabfield);
                                    ((ListView)dvavailable.InnerView).CollectionSource.Add(dvavailable.InnerView.ObjectSpace.GetObject(crttabfield));
                                }
                            }
                            dvselected.InnerView.ObjectSpace.CommitChanges();
                            ((ListView)dvavailable.InnerView).Refresh();
                            ((ListView)dvselected.InnerView).Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectSelectedFieldData"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    if (objtabinfo.strtabname == "QCParameters")
                    {
                        strtabname = objtabinfo.strtabname;
                        DashboardViewItem dvavailable = ((DetailView)View).FindItem("DVAvailableFields_QCParameters") as DashboardViewItem;
                        DashboardViewItem dvselected = ((DetailView)View).FindItem("DVQCParameters") as DashboardViewItem;
                        if (dvavailable != null && dvavailable.InnerView == null)
                        {
                            dvavailable.CreateControl();
                        }
                        if (dvselected != null && dvselected.InnerView == null)
                        {
                            dvselected.CreateControl();

                        }
                        if (dvavailable != null && dvavailable.InnerView != null && dvselected != null && dvselected.InnerView != null && dvselected.InnerView.SelectedObjects.Count > 0)
                        {
                            List<string> lststrfieldid = new List<string>();
                            foreach (TabFieldConfiguration objseltabfield in dvselected.InnerView.SelectedObjects)
                            {
                                TabFields objtabfields = os.FindObject<TabFields>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", objseltabfield.FieldID, strtabname));
                                if (objtabfields == null)
                                {
                                    TabFields crttabfield = os.CreateObject<TabFields>();
                                    crttabfield.FieldID = objseltabfield.FieldID;
                                    crttabfield.FieldCaption = objseltabfield.FieldCaption;
                                    crttabfield.TabName = objtabinfo.strtabname;
                                    os.CommitChanges();
                                    lststrfieldid.Add(objseltabfield.FieldID);
                                    dvselected.InnerView.ObjectSpace.Delete(objseltabfield);
                                    ((ListView)dvavailable.InnerView).CollectionSource.Add(dvavailable.InnerView.ObjectSpace.GetObject(crttabfield));
                                }
                            }
                            dvselected.InnerView.ObjectSpace.CommitChanges();
                            ((ListView)dvavailable.InnerView).Refresh();
                            ((ListView)dvselected.InnerView).Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectSelectedFieldData"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    if (objtabinfo.strtabname == "Components")
                    {
                        strtabname = objtabinfo.strtabname;
                        DashboardViewItem dvavailable = ((DetailView)View).FindItem("DVAvailableFields_Components") as DashboardViewItem;
                        DashboardViewItem dvselected = ((DetailView)View).FindItem("DVComponents") as DashboardViewItem;
                        if (dvavailable != null && dvavailable.InnerView == null)
                        {
                            dvavailable.CreateControl();
                        }
                        if (dvselected != null && dvselected.InnerView == null)
                        {
                            dvselected.CreateControl();

                        }
                        if (dvavailable != null && dvavailable.InnerView != null && dvselected != null && dvselected.InnerView != null && dvselected.InnerView.SelectedObjects.Count > 0)
                        {
                            List<string> lststrfieldid = new List<string>();
                            foreach (TabFieldConfiguration objseltabfield in dvselected.InnerView.SelectedObjects)
                            {
                                TabFields objtabfields = os.FindObject<TabFields>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", objseltabfield.FieldID, strtabname));
                                if (objtabfields == null)
                                {
                                    TabFields crttabfield = os.CreateObject<TabFields>();
                                    crttabfield.FieldID = objseltabfield.FieldID;
                                    crttabfield.FieldCaption = objseltabfield.FieldCaption;
                                    crttabfield.TabName = objtabinfo.strtabname;
                                    os.CommitChanges();
                                    lststrfieldid.Add(objseltabfield.FieldID);
                                    dvselected.InnerView.ObjectSpace.Delete(objseltabfield);
                                    ((ListView)dvavailable.InnerView).CollectionSource.Add(dvavailable.InnerView.ObjectSpace.GetObject(crttabfield));
                                }
                            }
                            dvselected.InnerView.ObjectSpace.CommitChanges();
                            ((ListView)dvavailable.InnerView).Refresh();
                            ((ListView)dvselected.InnerView).Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectSelectedFieldData"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    if (objtabinfo.strtabname == "QCParameterDefaults")
                    {
                        strtabname = objtabinfo.strtabname;
                        DashboardViewItem dvavailable = ((DetailView)View).FindItem("DVAvailableFields_QCParameterDefaults") as DashboardViewItem;
                        DashboardViewItem dvselected = ((DetailView)View).FindItem("DVQCParameterDefaults") as DashboardViewItem;
                        if (dvavailable != null && dvavailable.InnerView == null)
                        {
                            dvavailable.CreateControl();
                        }
                        if (dvselected != null && dvselected.InnerView == null)
                        {
                            dvselected.CreateControl();

                        }
                        if (dvavailable != null && dvavailable.InnerView != null && dvselected != null && dvselected.InnerView != null && dvselected.InnerView.SelectedObjects.Count > 0)
                        {
                            List<string> lststrfieldid = new List<string>();
                            foreach (TabFieldConfiguration objseltabfield in dvselected.InnerView.SelectedObjects)
                            {
                                TabFields objtabfields = os.FindObject<TabFields>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", objseltabfield.FieldID, strtabname));
                                if (objtabfields == null)
                                {
                                    TabFields crttabfield = os.CreateObject<TabFields>();
                                    crttabfield.FieldID = objseltabfield.FieldID;
                                    crttabfield.FieldCaption = objseltabfield.FieldCaption;
                                    crttabfield.TabName = objtabinfo.strtabname;
                                    os.CommitChanges();
                                    lststrfieldid.Add(objseltabfield.FieldID);
                                    dvselected.InnerView.ObjectSpace.Delete(objseltabfield);
                                    ((ListView)dvavailable.InnerView).CollectionSource.Add(dvavailable.InnerView.ObjectSpace.GetObject(crttabfield));
                                }
                            }
                            dvselected.InnerView.ObjectSpace.CommitChanges();
                            ((ListView)dvavailable.InnerView).Refresh();
                            ((ListView)dvselected.InnerView).Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectSelectedFieldData"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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

        private void BtnTestConfigAdd_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                string strtabname = string.Empty;
                if (View.Id == "TabFieldConfiguration_DetailView_TestTab")
                {
                    if (objtabinfo.strtabname == "SampleParameters" || string.IsNullOrEmpty(objtabinfo.strtabname))
                    {
                        objtabinfo.strtabname = "SampleParameters";
                        strtabname = objtabinfo.strtabname;
                        DashboardViewItem dvavailable = ((DetailView)View).FindItem("DVAvailableFields_SampleParameters") as DashboardViewItem;
                        DashboardViewItem dvselected = ((DetailView)View).FindItem("DVSampleParameters") as DashboardViewItem;
                        if (dvavailable != null && dvavailable.InnerView == null)
                        {
                            dvavailable.CreateControl();
                        }
                        if (dvselected != null && dvselected.InnerView == null)
                        {
                            dvselected.CreateControl();
                        }
                        if (dvavailable != null && dvavailable.InnerView != null && dvavailable.InnerView.SelectedObjects.Count > 0)
                        {
                            foreach (TabFields objavailfield in dvavailable.InnerView.SelectedObjects)
                            {
                                TabFieldConfiguration objtabfildconfig = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", objavailfield.FieldID, strtabname));
                                if (objtabfildconfig == null)
                                {
                                    TabFieldConfiguration crttabfield = os.CreateObject<TabFieldConfiguration>();
                                    crttabfield.FieldID = objavailfield.FieldID;
                                    crttabfield.FieldCaption = objavailfield.FieldID;
                                    crttabfield.FieldCustomCaption = "";
                                    crttabfield.Freeze = false;
                                    crttabfield.Width = 100;
                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                    crttabfield.SortOrder = sort;
                                    crttabfield.TabName = objtabinfo.strtabname;
                                    os.CommitChanges();
                                    dvavailable.InnerView.ObjectSpace.Delete(objavailfield);
                                    ((ListView)dvselected.InnerView).CollectionSource.Add(dvselected.InnerView.ObjectSpace.GetObject(crttabfield));
                                }
                            }
                            dvavailable.InnerView.ObjectSpace.CommitChanges();
                            os.Refresh();
                            ((ListView)dvavailable.InnerView).Refresh();
                            ((ListView)dvselected.InnerView).Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectAvailableFieldData"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    if (objtabinfo.strtabname == "Surrogates")
                    {
                        strtabname = objtabinfo.strtabname;
                        DashboardViewItem dvavailable = ((DetailView)View).FindItem("DVAvailableFields_Surrogates") as DashboardViewItem;
                        DashboardViewItem dvselected = ((DetailView)View).FindItem("DvSurrogates") as DashboardViewItem;
                        if (dvavailable != null && dvavailable.InnerView == null)
                        {
                            dvavailable.CreateControl();
                        }
                        if (dvselected != null && dvselected.InnerView == null)
                        {
                            dvselected.CreateControl();

                        }
                        if (dvavailable != null && dvavailable.InnerView != null && dvselected != null && dvselected.InnerView != null && dvavailable.InnerView.SelectedObjects.Count > 0)
                        {
                            foreach (TabFields objavailfield in dvavailable.InnerView.SelectedObjects)
                            {
                                TabFieldConfiguration objtabfildconfig = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", objavailfield.FieldID, strtabname));
                                if (objtabfildconfig == null)
                                {
                                    TabFieldConfiguration crttabfield = os.CreateObject<TabFieldConfiguration>();
                                    crttabfield.FieldID = objavailfield.FieldID;
                                    crttabfield.FieldCaption = objavailfield.FieldID;
                                    crttabfield.FieldCustomCaption = "";
                                    crttabfield.Freeze = false;
                                    crttabfield.Width = 100;
                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                    crttabfield.SortOrder = sort;
                                    crttabfield.TabName = objtabinfo.strtabname;
                                    os.CommitChanges();
                                    dvavailable.InnerView.ObjectSpace.Delete(objavailfield);
                                    ((ListView)dvselected.InnerView).CollectionSource.Add(dvselected.InnerView.ObjectSpace.GetObject(crttabfield));
                                }
                            }
                            dvavailable.InnerView.ObjectSpace.CommitChanges();
                            os.Refresh();
                            ((ListView)dvavailable.InnerView).Refresh();
                            ((ListView)dvselected.InnerView).Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectAvailableFieldData"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    if (objtabinfo.strtabname == "InternalStandards")
                    {
                        strtabname = objtabinfo.strtabname;
                        DashboardViewItem dvavailable = ((DetailView)View).FindItem("DVAvailableFields_InternalStandards") as DashboardViewItem;
                        DashboardViewItem dvselected = ((DetailView)View).FindItem("DVInternalStandards") as DashboardViewItem;
                        if (dvavailable != null && dvavailable.InnerView == null)
                        {
                            dvavailable.CreateControl();
                        }
                        if (dvselected != null && dvselected.InnerView == null)
                        {
                            dvselected.CreateControl();

                        }
                        if (dvavailable != null && dvavailable.InnerView != null && dvselected != null && dvselected.InnerView != null && dvavailable.InnerView.SelectedObjects.Count > 0)
                        {
                            foreach (TabFields objavailfield in dvavailable.InnerView.SelectedObjects)
                            {
                                TabFieldConfiguration objtabfildconfig = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", objavailfield.FieldID, strtabname));
                                if (objtabfildconfig == null)
                                {
                                    TabFieldConfiguration crttabfield = os.CreateObject<TabFieldConfiguration>();
                                    crttabfield.FieldID = objavailfield.FieldID;
                                    crttabfield.FieldCaption = objavailfield.FieldID;
                                    crttabfield.FieldCustomCaption = "";
                                    crttabfield.Freeze = false;
                                    crttabfield.Width = 100;
                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                    crttabfield.SortOrder = sort;
                                    crttabfield.TabName = objtabinfo.strtabname;
                                    os.CommitChanges();
                                    dvavailable.InnerView.ObjectSpace.Delete(objavailfield);
                                    ((ListView)dvselected.InnerView).CollectionSource.Add(dvselected.InnerView.ObjectSpace.GetObject(crttabfield));
                                }
                            }
                            dvavailable.InnerView.ObjectSpace.CommitChanges();
                            os.Refresh();
                            ((ListView)dvavailable.InnerView).Refresh();
                            ((ListView)dvselected.InnerView).Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectAvailableFieldData"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    if (objtabinfo.strtabname == "QCParameters")
                    {
                        strtabname = objtabinfo.strtabname;
                        DashboardViewItem dvavailable = ((DetailView)View).FindItem("DVAvailableFields_QCParameters") as DashboardViewItem;
                        DashboardViewItem dvselected = ((DetailView)View).FindItem("DVQCParameters") as DashboardViewItem;
                        if (dvavailable != null && dvavailable.InnerView == null)
                        {
                            dvavailable.CreateControl();
                        }
                        if (dvselected != null && dvselected.InnerView == null)
                        {
                            dvselected.CreateControl();

                        }
                        if (dvavailable != null && dvavailable.InnerView != null && dvselected != null && dvselected.InnerView != null && dvavailable.InnerView.SelectedObjects.Count > 0)
                        {
                            foreach (TabFields objavailfield in dvavailable.InnerView.SelectedObjects)
                            {
                                TabFieldConfiguration objtabfildconfig = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", objavailfield.FieldID, strtabname));
                                if (objtabfildconfig == null)
                                {
                                    TabFieldConfiguration crttabfield = os.CreateObject<TabFieldConfiguration>();
                                    crttabfield.FieldID = objavailfield.FieldID;
                                    crttabfield.FieldCaption = objavailfield.FieldID;
                                    crttabfield.FieldCustomCaption = "";
                                    crttabfield.Freeze = false;
                                    crttabfield.Width = 100;
                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                    crttabfield.SortOrder = sort;
                                    crttabfield.TabName = objtabinfo.strtabname;
                                    os.CommitChanges();
                                    dvavailable.InnerView.ObjectSpace.Delete(objavailfield);
                                    ((ListView)dvselected.InnerView).CollectionSource.Add(dvselected.InnerView.ObjectSpace.GetObject(crttabfield));
                                }
                            }
                            dvavailable.InnerView.ObjectSpace.CommitChanges();
                            os.Refresh();
                            ((ListView)dvavailable.InnerView).Refresh();
                            ((ListView)dvselected.InnerView).Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectAvailableFieldData"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    if (objtabinfo.strtabname == "Components")
                    {
                        strtabname = objtabinfo.strtabname;
                        DashboardViewItem dvavailable = ((DetailView)View).FindItem("DVAvailableFields_Components") as DashboardViewItem;
                        DashboardViewItem dvselected = ((DetailView)View).FindItem("DVComponents") as DashboardViewItem;
                        if (dvavailable != null && dvavailable.InnerView == null)
                        {
                            dvavailable.CreateControl();
                        }
                        if (dvselected != null && dvselected.InnerView == null)
                        {
                            dvselected.CreateControl();

                        }
                        if (dvavailable != null && dvavailable.InnerView != null && dvselected != null && dvselected.InnerView != null && dvavailable.InnerView.SelectedObjects.Count > 0)
                        {
                            foreach (TabFields objavailfield in dvavailable.InnerView.SelectedObjects)
                            {
                                TabFieldConfiguration objtabfildconfig = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", objavailfield.FieldID, strtabname));
                                if (objtabfildconfig == null)
                                {
                                    TabFieldConfiguration crttabfield = os.CreateObject<TabFieldConfiguration>();
                                    crttabfield.FieldID = objavailfield.FieldID;
                                    crttabfield.FieldCaption = objavailfield.FieldID;
                                    crttabfield.FieldCustomCaption = "";
                                    crttabfield.Freeze = false;
                                    crttabfield.Width = 100;
                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                    crttabfield.SortOrder = sort;
                                    crttabfield.TabName = objtabinfo.strtabname;
                                    os.CommitChanges();
                                    dvavailable.InnerView.ObjectSpace.Delete(objavailfield);
                                    ((ListView)dvselected.InnerView).CollectionSource.Add(dvselected.InnerView.ObjectSpace.GetObject(crttabfield));
                                }
                            }
                            dvavailable.InnerView.ObjectSpace.CommitChanges();
                            os.Refresh();
                            ((ListView)dvavailable.InnerView).Refresh();
                            ((ListView)dvselected.InnerView).Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectAvailableFieldData"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    if (objtabinfo.strtabname == "QCParameterDefaults")
                    {
                        strtabname = objtabinfo.strtabname;
                        DashboardViewItem dvavailable = ((DetailView)View).FindItem("DVAvailableFields_QCParameterDefaults") as DashboardViewItem;
                        DashboardViewItem dvselected = ((DetailView)View).FindItem("DVQCParameterDefaults") as DashboardViewItem;
                        if (dvavailable != null && dvavailable.InnerView == null)
                        {
                            dvavailable.CreateControl();
                        }
                        if (dvselected != null && dvselected.InnerView == null)
                        {
                            dvselected.CreateControl();

                        }
                        if (dvavailable != null && dvavailable.InnerView != null && dvselected != null && dvselected.InnerView != null && dvavailable.InnerView.SelectedObjects.Count > 0)
                        {
                            foreach (TabFields objavailfield in dvavailable.InnerView.SelectedObjects)
                            {
                                TabFieldConfiguration objtabfildconfig = os.FindObject<TabFieldConfiguration>(CriteriaOperator.Parse("[FieldID] = ? And [TabName] = ?", objavailfield.FieldID, strtabname));
                                if (objtabfildconfig == null)
                                {
                                    TabFieldConfiguration crttabfield = os.CreateObject<TabFieldConfiguration>();
                                    crttabfield.FieldID = objavailfield.FieldID;
                                    crttabfield.FieldCaption = objavailfield.FieldID;
                                    crttabfield.FieldCustomCaption = "";
                                    crttabfield.Freeze = false;
                                    crttabfield.Width = 100;
                                    int sort = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(TabFieldConfiguration), CriteriaOperator.Parse("MAX(SortOrder)"), CriteriaOperator.Parse("[TabName] = ?", strtabname))) + 1);
                                    crttabfield.SortOrder = sort;
                                    crttabfield.TabName = objtabinfo.strtabname;
                                    os.CommitChanges();
                                    dvavailable.InnerView.ObjectSpace.Delete(objavailfield);
                                    ((ListView)dvselected.InnerView).CollectionSource.Add(dvselected.InnerView.ObjectSpace.GetObject(crttabfield));
                                }
                            }
                            dvavailable.InnerView.ObjectSpace.CommitChanges();
                            os.Refresh();
                            ((ListView)dvavailable.InnerView).Refresh();
                            ((ListView)dvselected.InnerView).Refresh();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectAvailableFieldData"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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
