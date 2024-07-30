using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LDM.Module.Controllers.Settings
{
    public partial class DataEntryViewController : ViewController, IXafCallbackHandler
    {
        SimpleAction DataEntrySave;
        MessageTimer timer = new MessageTimer();
        UADECInfo objdataentryInfo = new UADECInfo();
        public DataEntryViewController()
        {
            InitializeComponent();
            TargetViewId = "DataEntry_ListView;"
                + "DataEntry_ListView_Copy;"
                + "DataEntry_ListView_Copy_Edit;"
                + "TestMethod_LookupListView_DataEntry;";
            DataEntrySave = new SimpleAction(this, "DataEntrySave", PredefinedCategory.RecordEdit);
            DataEntrySave.Caption = "Save";
            DataEntrySave.ToolTip = "Save";
            DataEntrySave.Category = "Edit";
            DataEntrySave.ImageName = "Action_Save";
            DataEntrySave.TargetViewId = "DataEntry_ListView_Copy;" + "DataEntry_ListView_Copy_Edit;";
            DataEntrySave.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            DataEntrySave.Executing += DataEntrySave_Executing;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "DataEntry_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Execute += NewObjectAction_Execute;
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                }
                else if (View.Id == "DataEntry_ListView_Copy")
                {
                    IList<UnconventionalAnalysisDataEntryConfigBOSourceCodeCaption> litcaption = new List<UnconventionalAnalysisDataEntryConfigBOSourceCodeCaption>();
                    ((ListView)View).CollectionSource.Criteria["BOSourceCodeCaption"] = CriteriaOperator.Parse("1=2");
                    IList<DataEntry> dataEntries = View.ObjectSpace.GetObjects<DataEntry>(CriteriaOperator.Parse(""));
                    if (dataEntries.Count > 0)
                    {
                        litcaption = View.ObjectSpace.GetObjects<UnconventionalAnalysisDataEntryConfigBOSourceCodeCaption>(CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", dataEntries.ToList().Select(x => x.BOSourceCodeCaption.Oid))) + ")"));
                    }
                    else
                    {
                        litcaption = View.ObjectSpace.GetObjects<UnconventionalAnalysisDataEntryConfigBOSourceCodeCaption>(CriteriaOperator.Parse(""));
                    }
                    foreach (UnconventionalAnalysisDataEntryConfigBOSourceCodeCaption objUAD in litcaption)
                    {
                        DataEntry objde = View.ObjectSpace.CreateObject<DataEntry>();
                        objde.BOSourceCodeCaption = objUAD;
                        ((ListView)View).CollectionSource.Add(objde);
                    }
                    ((ListView)View).Refresh();
                }
                else if (View.Id == "TestMethod_LookupListView_DataEntry")
                {
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(TestMethod)))
                    {
                        lstview.Properties.Add(new ViewProperty("TestName", SortDirection.Ascending, "TestName", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void NewObjectAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "DataEntry_ListView")
                {
                    ListView CreateListView = Application.CreateListView("DataEntry_ListView_Copy", new CollectionSource(Application.CreateObjectSpace(), typeof(DataEntry)), true);
                    e.ShowViewParameters.CreatedView = CreateListView;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void EditAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "DataEntry_ListView")
                {
                    e.Cancel = true;
                    if (View.SelectedObjects.Count == 1)
                    {
                        DataEntry objDataEntry = (DataEntry)View.CurrentObject;
                        if (objDataEntry != null)
                        {
                            CollectionSource cs = new CollectionSource(Application.CreateObjectSpace(), typeof(DataEntry));
                            cs.Criteria["filter"] = CriteriaOperator.Parse("[Oid]=?", objDataEntry.Oid);
                            ListView CreateListView = Application.CreateListView("DataEntry_ListView_Copy_Edit", cs, false);
                            Frame.SetView(CreateListView);
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

        private void DataEntrySave_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                bool Test = true;
                if (View.Id == "DataEntry_ListView_Copy")
                {
                    var selection = View.SelectedObjects;
                    foreach (DataEntry entry in selection)
                    {
                        if (entry.Test == null)
                        {
                            Test = false;
                        }
                    }
                    if (Test == true)
                    {
                        if (selection.Count > 0)
                        {
                            foreach (DataEntry obj in ((ListView)View).CollectionSource.List.Cast<DataEntry>().ToList())
                            {
                                if (!selection.Contains(obj))
                                {
                                    ((ListView)View).CollectionSource.Remove(obj);
                                    View.ObjectSpace.RemoveFromModifiedObjects(obj);
                                }
                            }
                            View.ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage("Saved successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                            View.Close();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Please select atleast one check box.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Enter the test.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }
                else if (View.Id == "DataEntry_ListView_Copy_Edit")
                {
                    foreach (DataEntry obj in ((ListView)View).CollectionSource.List.Cast<DataEntry>().ToList())
                    {
                        if (string.IsNullOrEmpty(obj.Test))
                        {
                            View.ObjectSpace.Delete(obj);
                        }
                    }
                    View.ObjectSpace.CommitChanges();
                    Application.ShowViewStrategy.ShowMessage("Saved successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                    View.Close();
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
                if (View is ListView)
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (View.Id == "DataEntry_ListView_Copy" || View.Id == "DataEntry_ListView_Copy_Edit")
                    {
                        XafCallbackManager parameter = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                        parameter.RegisterHandler("DataEntryType", this);
                        gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    }
                    else if (View.Id == "TestMethod_LookupListView_DataEntry")
                    {
                        gridListEditor.Grid.PreRender += Grid_PreRender;
                    }
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
                string[] param = parameter.Split('|');
                if (!string.IsNullOrEmpty(parameter) && View.Id == "DataEntry_ListView_Copy" || View.Id == "DataEntry_ListView_Copy_Edit")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                    {
                        string Currrow = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid").ToString();
                        if (param[0] == "Test" && !string.IsNullOrEmpty(Currrow))
                        {
                            DataEntry objdataentry = ((ListView)View).CollectionSource.List.Cast<DataEntry>().Where(a => a.Oid.ToString() == Currrow).FirstOrDefault();
                            if (objdataentry != null)
                            {
                                objdataentryInfo.CurDataEntry = objdataentry.Oid;
                                if (objdataentry.Test != null)
                                {
                                    objdataentryInfo.TestName = objdataentry.Test.Split(';').Select(i => i.ToString()).ToList();
                                }
                                IObjectSpace objspace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(objspace, typeof(TestMethod));
                                ListView createListView = Application.CreateListView("TestMethod_LookupListView_DataEntry", cs, false);
                                ShowViewParameters showViewParameters = new ShowViewParameters(createListView);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.Accepting += Dc_Accepting;
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

        private void Grid_PreRender(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView grid = (ASPxGridView)sender;
                if (View.Id == "TestMethod_LookupListView_DataEntry")
                {
                    if (grid != null && objdataentryInfo.TestName != null && objdataentryInfo.TestName.Count > 0)
                    {
                        foreach (TestMethod obj in ((ListView)View).CollectionSource.List)
                        {
                            if (objdataentryInfo.TestName.Contains(obj.TestName))
                            {
                                grid.Selection.SelectRowByKey(obj.Oid);
                            }
                        }
                        objdataentryInfo.TestName = null;
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
                if (View.Id == "DataEntry_ListView_Copy" || View.Id == "DataEntry_ListView_Copy_Edit" && e.DataColumn.FieldName == "Test")
                {
                    e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'DataEntryType', 'Test|'+{0}, '', false)", e.VisibleIndex));
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
                if (View.Id == "DataEntry_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Execute -= NewObjectAction_Execute;
                    Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;
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
                DataEntry objdataentry = ((ListView)View).CollectionSource.List.Cast<DataEntry>().Where(a => a.Oid == objdataentryInfo.CurDataEntry).FirstOrDefault();
                if (objdataentry != null)
                {
                    string CurrentTestOid = null;
                    foreach (TestMethod objTM in e.AcceptActionArgs.SelectedObjects)
                    {
                        IList<TestMethod> lstTM = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName] = ?", objTM.TestName));
                        if (lstTM.Count > 0)
                        {
                            CurrentTestOid = CurrentTestOid + (string.IsNullOrEmpty(CurrentTestOid) ? "" : ";") + string.Join(";", lstTM.Cast<TestMethod>().Select(i => i.Oid).ToList());
                        }
                    }
                    objdataentry.Test = string.Join(";", e.AcceptActionArgs.SelectedObjects.Cast<TestMethod>().Select(i => i.TestName).ToList());
                    objdataentry.TestNameOid = CurrentTestOid;
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
