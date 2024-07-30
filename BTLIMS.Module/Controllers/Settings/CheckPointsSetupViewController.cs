using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CheckPointsSetupViewController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        public CheckPointsSetupViewController()
        {
            InitializeComponent();
            TargetViewId = "SampleConditionCheckData_ListView;" + "VisualMatrix_LookupListView_CheckListSetup;" + "SampleConditionCheckData_DetailView;";
            CheckPointSetupSave.TargetViewId = "SampleConditionCheckData_ListView;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "VisualMatrix_LookupListView_CheckListSetup")
                {
                    View.ControlsCreated += View_ControlsCreated;

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
                if (View.Id == "VisualMatrix_LookupListView_CheckListSetup")
                {
                    View.ControlsCreated -= View_ControlsCreated;
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        if (HttpContext.Current.Session["CheckListSetup"] != null)
                        {
                            string[] SampleMatrix = HttpContext.Current.Session["CheckListSetup"].ToString().Split(new string[] { "; " }, StringSplitOptions.None);
                            foreach (string val in SampleMatrix)
                            {
                                VisualMatrix SM = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[VisualMatrixName]=?", val.Trim()));
                                if (SM != null)
                                {
                                    gridListEditor.Grid.Selection.SelectRowByKey(SM.Oid);
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
                if (View.Id == "SampleConditionCheckData_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ICallbackManagerHolder parameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    parameter.CallbackManager.RegisterHandler("CheckListSetup", this);
                    gridListEditor.Grid.ClientInstanceName = "CheckList";
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                }
                else if (View.Id == "SampleConditionCheckData_DetailView")
                {
                    ASPxCheckedLookupStringPropertyEditor lookup = ((DetailView)View).FindItem("SampleMatrices") as ASPxCheckedLookupStringPropertyEditor;
                    if (lookup != null)
                    {
                        ASPxGridLookup editor = (ASPxGridLookup)lookup.Editor;
                        if (editor != null)
                        {
                            editor.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            editor.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            editor.GridView.Settings.VerticalScrollableHeight = 200;
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

        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                try
                {
                    if (View.Id == "SampleConditionCheckData_ListView")
                    {
                        if (e.DataColumn.FieldName != "SampleMatrices") return;
                        e.Cell.Attributes.Add("onclick", "RaiseXafCallback(globalCallbackControl, 'CheckListSetup'," + e.VisibleIndex + " , '', false);");
                    }
                }
                catch (Exception ex)
                {
                    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
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
        }

        public void ProcessAction(string parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(parameter))
                {
                    if (View.Id == "SampleConditionCheckData_ListView")
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null)
                        {
                            HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(parameter), "Oid");
                            HttpContext.Current.Session["CheckListSetup"] = gridListEditor.Grid.GetRowValues(int.Parse(parameter), "SampleMatrices");
                            IObjectSpace os = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(os, typeof(VisualMatrix));
                            ListView lv = Application.CreateListView("VisualMatrix_LookupListView_CheckListSetup", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.AcceptAction.Execute += SampleMatrixAccept_Execute;
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

        private void SampleMatrixAccept_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string SampleMatrix = string.Empty;
                foreach (VisualMatrix SM in e.SelectedObjects)
                {
                    if (SampleMatrix == string.Empty)
                    {
                        SampleMatrix = SM.VisualMatrixName;
                    }
                    else
                    {
                        SampleMatrix = SampleMatrix + "; " + SM.VisualMatrixName;
                    }
                }
                if (HttpContext.Current.Session["rowid"] != null)
                {
                    SampleConditionCheckData objCheckData = ((ListView)View).CollectionSource.List.Cast<SampleConditionCheckData>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                    if (objCheckData != null)
                    {
                        objCheckData.SampleMatrices = SampleMatrix;
                    }
                   ((ListView)View).Refresh();
                    //  ((ListView)View).RefreshDataSource();
                    View.Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        private void CheckPointSetupSave_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                ObjectSpace.CommitChanges();
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
