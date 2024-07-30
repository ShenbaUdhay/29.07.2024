using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.Base;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using LDM.Module.Web.Editors;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TestInstrumentsController : ViewController, IXafCallbackHandler
    {
        Guid curTestOid;
        TestmethodQctypeinfo objtmqc = new TestmethodQctypeinfo();
        TestInstrumentClass testInstrument = new TestInstrumentClass();
        MessageTimer timer = new MessageTimer();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        SimpleAction CopyInstrument;

        public TestInstrumentsController()
        {
            InitializeComponent();
            TargetViewId = "TestMethod_ListView_TestInstrument;" + "TestMethod_ListView_SamplePrep_TestInstrument;" + "Labware_ListView_Instrument;" + "TestMethod_ListView_CopyToInstrument;" + "TestMethod_DetailView_InstrumentCopyTo;" + "TestMethod_ListView_FieldInstrument;";
            CopyInstrument = new SimpleAction(this, "CopyInstrument", PredefinedCategory.ObjectsCreation);
            CopyInstrument.Caption = "Copy Instrument";
            CopyInstrument.ImageName = "Action_Copy";
            CopyInstrument.TargetViewId = "TestMethod_ListView_TestInstrument;" + "TestMethod_ListView_SamplePrep_TestInstrument;" + "TestMethod_ListView_FieldInstrument;";
            CopyInstrument.Execute += CopyInstrument_Execute;
        }

        private void CopyInstrument_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count == 0)
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                if (View.SelectedObjects.Count == 1)
                {
                    foreach (TestMethod maintest in View.SelectedObjects)
                    {
                        if (Application.MainWindow.View.Id == "TestMethod_ListView_TestInstrument" ||Application.MainWindow.View.Id== "TestMethod_ListView_FieldInstrument")
                        {
                            objtmqc.applicationviewid = Application.MainWindow.View.Id.ToString();
                            if (maintest.Labwares.Count > 0)
                            {
                                objtmqc.strinstruments = string.Join(";", maintest.Labwares.Select(i => i.AssignedName));
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Instrumentsempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        else if (Application.MainWindow.View.Id == "TestMethod_ListView_SamplePrep_TestInstrument")
                        {
                            objtmqc.applicationviewid = Application.MainWindow.View.Id.ToString();
                            if (maintest.SamplePrepLabwares.Count > 0)
                            {
                                objtmqc.strinstruments = string.Join(";", maintest.SamplePrepLabwares.Select(i => i.AssignedName));
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Instrumentsempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                    }
                    IObjectSpace os = Application.CreateObjectSpace();
                    TestMethod objtm = os.CreateObject<TestMethod>();
                    DetailView createdv = Application.CreateDetailView(os, "TestMethod_DetailView_InstrumentCopyTo", false, objtm);
                    ShowViewParameters showviewparameter = new ShowViewParameters(createdv);
                    showviewparameter.Context = TemplateContext.PopupWindow;
                    showviewparameter.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += dc_Accepting;
                    //dc.AcceptAction.Executed += AcceptAction_Executed;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showviewparameter.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showviewparameter, new ShowViewSource(null, null));
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlyonechkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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
                //IObjectSpace os = Application.CreateObjectSpace();
                //TestMethod maincurrentobj = (TestMethod)Application.MainWindow.View.CurrentObject;
                //DialogController dc = (DialogController)sender;
                //if (maincurrentobj != null)
                //{

                //    DashboardViewItem dvi = (DashboardViewItem)((DetailView)dc.Window.View).FindItem("TestMethodTo");
                //    Session currentSession = ((XPObjectSpace)(os)).Session;
                //    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                //    if (((ListView)dvi.InnerView).SelectedObjects.Count > 0)
                //    {
                //        TestMethod objtestMethod = os.GetObjectByKey<TestMethod>(maincurrentobj.Oid);

                //        foreach (TestMethod objTMSelected in ((ListView)dvi.InnerView).SelectedObjects)
                //        {
                //            foreach (Labware objTestInstr in objtestMethod.Labwares)
                //            {
                //                TestMethod testMethod = new TestMethod(uow);
                //                Labware addedInstrument = uow.GetObjectByKey<Labware>(objTestInstr.Oid);
                //                objTMSelected.Labwares.Add(addedInstrument);
                //            }
                //        }

                //        os.CommitChanges();
                //        View.ObjectSpace.Refresh();
                //    }

                //}

                if (View.Id == "TestMethod_ListView_TestInstrument" ||View.Id== "TestMethod_ListView_FieldInstrument")
                {
                    TestMethod mainTestMethod = (TestMethod)Application.MainWindow.View.CurrentObject;
                    DialogController dc = (DialogController)sender;
                    if (mainTestMethod != null)
                    {
                        DashboardViewItem dvi = (DashboardViewItem)((DetailView)dc.Window.View).FindItem("TestMethodTo");
                        if (dvi != null && dvi.InnerView != null && dvi.InnerView.SelectedObjects.Count > 0)
                        {
                            TestMethod objTest = dvi.InnerView.ObjectSpace.GetObjectByKey<TestMethod>(mainTestMethod.Oid);
                            if (objTest != null)
                            {
                                foreach (TestMethod obj in dvi.InnerView.SelectedObjects)
                                {
                                    foreach (Modules.BusinessObjects.Assets.Labware objLabware in objTest.Labwares.ToList())
                                    {
                                        obj.Labwares.Add(objLabware);
                                    }
                                }
                                dvi.InnerView.ObjectSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "copysuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }

                        View.ObjectSpace.Refresh();
                        //}
                    }

                    else
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }

                }
                else if (View.Id == "TestMethod_ListView_SamplePrep_TestInstrument")
                {
                    TestMethod mainTestMethod = (TestMethod)Application.MainWindow.View.CurrentObject;
                    DialogController dc = (DialogController)sender;
                    if (mainTestMethod != null)
                    {
                        DashboardViewItem dvi = (DashboardViewItem)((DetailView)dc.Window.View).FindItem("TestMethodTo");
                        if (dvi != null && dvi.InnerView != null && dvi.InnerView.SelectedObjects.Count > 0)
                        {
                            TestMethod objTest = dvi.InnerView.ObjectSpace.GetObjectByKey<TestMethod>(mainTestMethod.Oid);
                            if (objTest != null)
                            {
                                foreach (TestMethod obj in dvi.InnerView.SelectedObjects)
                                {
                                    foreach (Modules.BusinessObjects.Assets.Labware objLabware in objTest.SamplePrepLabwares.ToList())
                                    {
                                        obj.SamplePrepLabwares.Add(objLabware);
                                    }
                                }
                                dvi.InnerView.ObjectSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Copy"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                        View.ObjectSpace.Refresh();
                        //}
                    }
                    else
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }

                //ObjectSpace.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Parameter.Oid] = ? And [Surroagate] = true", objCurTest.Oid, tp.Parameter.Oid)) == null)
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
                if (View.Id == "TestMethod_ListView_TestInstrument" ||View.Id== "TestMethod_ListView_FieldInstrument")
                {
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    if (currentUser != null && View != null)
                    {
                        if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                        {
                            objPermissionInfo.TestInstrumentsIsWrite = false;
                            objPermissionInfo.IsSamplePrepInstruments = false;
                            CopyInstrument.Active.SetItemValue("btncopy", false);
                            if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                            {
                                objPermissionInfo.TestInstrumentsIsWrite = true;
                                objPermissionInfo.IsSamplePrepInstruments = true;
                                CopyInstrument.Active.SetItemValue("btncopy", true);
                            }
                            else
                            {
                                foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                                {
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.NavigationId == "TestInstrument" && i.Write == true) != null)
                                    {
                                        objPermissionInfo.TestInstrumentsIsWrite = true;
                                        CopyInstrument.Active.SetItemValue("btncopy", true);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (View.Id == "TestMethod_ListView_SamplePrep_TestInstrument")
                {
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    if (currentUser != null && View != null)
                    {
                        if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                        {
                            objPermissionInfo.TestInstrumentsIsWrite = false;
                            objPermissionInfo.IsSamplePrepInstruments = false;
                            CopyInstrument.Active.SetItemValue("btncopy", false);
                            if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                            {
                                objPermissionInfo.TestInstrumentsIsWrite = true;
                                objPermissionInfo.IsSamplePrepInstruments = true;
                                CopyInstrument.Active.SetItemValue("btncopy", true);
                            }
                            else
                            {
                                foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                                {
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.NavigationId == "SamplePrepbatch_TestInstrument" && i.Write == true) != null)
                                    {
                                        objPermissionInfo.IsSamplePrepInstruments = true;
                                        CopyInstrument.Active.SetItemValue("btncopy", true);
                                    }
                                }
                            }
                        }
                    }
                }
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;


                if (View.Id == "TestMethod_DetailView_InstrumentCopyTo")
                {
                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(TestMethod))
                    {
                        if (Application.MainWindow.View.Id == "TestMethod_ListView_TestInstrument" ||Application.MainWindow.View.Id== "TestMethod_ListView_FieldInstrument")
                        {
                            TestMethod testMethod = (TestMethod)Application.MainWindow.View.CurrentObject;
                            DashboardViewItem lvtest = ((DetailView)View).FindItem("TestMethodTo") as DashboardViewItem;
                            if (lvtest != null && lvtest.InnerView == null)
                            {
                                lvtest.CreateControl();
                            }
                            if (lvtest != null)
                            {
                                ((ListView)lvtest.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Labwares][].Count() = 0");
                            }
                        }
                        else if (Application.MainWindow.View.Id == "TestMethod_ListView_SamplePrep_TestInstrument")
                        {
                            TestMethod testMethod = (TestMethod)Application.MainWindow.View.CurrentObject;
                            DashboardViewItem lvtest = ((DetailView)View).FindItem("TestMethodTo") as DashboardViewItem;
                            if (lvtest != null && lvtest.InnerView == null)
                            {
                                lvtest.CreateControl();
                            }
                            if (lvtest != null)
                            {
                                ((ListView)lvtest.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SamplePrepLabwares][].Count() = 0");
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
            try
            {
                base.OnViewControlsCreated();

                if (View.Id == "TestMethod_ListView_TestInstrument" || View.Id == "TestMethod_ListView_SamplePrep_TestInstrument" ||View.Id== "TestMethod_ListView_FieldInstrument")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("TestInstrumentHandler", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = gridListEditor.Grid;
                    if (gridView != null)
                    {
                        gridView.HtmlDataCellPrepared += GridView_TestInstrument;
                        gridView.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    }
                }
                else if (View.Id == "Labware_ListView_Instrument")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = gridListEditor.Grid;
                    if (gridView != null)
                    {
                        gridView.PreRender += GridView_PreRender;
                    }
                }
                else if (View.Id == "TestMethod_ListView_CopyToInstrument")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor.AllowEdit != null && editor.Grid != null)
                    {
                        foreach (GridViewColumn column in editor.Grid.Columns)
                        {
                            if (column.Name == "InlineEditCommandColumn" || column.Name == "Edit")
                            {
                                column.Visible = false;
                            }
                        }
                    }
                    Frame.GetController<RefreshController>().RefreshAction.Active.SetItemValue("RefreshAction", false);
                    TestMethod maincurrentobj = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (maincurrentobj != null)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView view = nestedFrame.ViewItem.View;
                        if (view != null && view is DetailView)
                        {
                            TestMethod currentobj = (TestMethod)view.CurrentObject;
                            currentobj.TestName = maincurrentobj.TestName;
                        }
                    }
                }
                else if (View.Id == "TestMethod_DetailView_InstrumentCopyTo")
                {
                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(TestMethod))
                    {
                        foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "TestName" || i.Id == "NPInstrument"))
                        {
                            if (item.GetType() == typeof(AspxStringComoboxPropertyEditor))
                            {
                                AspxStringComoboxPropertyEditor customStringCombo = ((DetailView)View).FindItem("TestName") as AspxStringComoboxPropertyEditor;
                                if (customStringCombo != null && customStringCombo.Editor != null)
                                {
                                    customStringCombo.Editor.ForeColor = Color.Black;
                                }
                            }
                            if (item.GetType() == typeof(ASPxStringPropertyEditor))
                            {
                                ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;
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
        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            try
            {
                e.PopupControl.CustomizePopupWindowSize += XafPopupWindowControl_CustomizePopupWindowSize;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void XafPopupWindowControl_CustomizePopupWindowSize(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (View != null && (View.Id == "Labware_ListView_Instrument" || View.Id == "TestMethod_DetailView_InstrumentCopyTo"))
                {
                    e.Width = 1000;
                    e.Height = 600;
                    e.Handled = true;
                }
                //else
                //{
                //    e.Width = new System.Web.UI.WebControls.Unit(1200);
                //    e.Height = new System.Web.UI.WebControls.Unit(850);
                //    e.Handled = true;
                //}
                //e.Width = new System.Web.UI.WebControls.Unit(1200);
                //e.Height = new System.Web.UI.WebControls.Unit(850);
                //e.Handled = true;
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
                if (View.Id == "Labware_ListView_Instrument")
                {
                    ASPxGridView grid = (ASPxGridView)sender;
                    if (grid != null)
                    {
                        foreach (Modules.BusinessObjects.Assets.Labware objInstrument in ((ListView)View).CollectionSource.List)
                        {
                            if (testInstrument.lstExistingInstruments.Contains(objInstrument.Oid))
                            {
                                grid.Selection.SelectRowByKey(objInstrument.Oid);
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

        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();
                if (View.Id == "Labware_ListView_Instrument")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = gridListEditor.Grid;
                    if (gridView != null)
                    {
                        gridView.PreRender -= GridView_PreRender;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        void GridView_TestInstrument(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id == "TestMethod_ListView_TestInstrument" || View.Id== "TestMethod_ListView_FieldInstrument")
                {
                    if (e.DataColumn.FieldName == "Instrument" && objPermissionInfo.TestInstrumentsIsWrite == true)
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'TestInstrumentHandler', 'InstrumentId|'+{0}, '', false)", e.VisibleIndex));
                    }
                    else
                    {
                        return;
                    }
                }
                else if (View.Id == "TestMethod_ListView_SamplePrep_TestInstrument")
                {
                    if (e.DataColumn.FieldName == "SamplePrepInstrument" && objPermissionInfo.IsSamplePrepInstruments == true)
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'TestInstrumentHandler', 'InstrumentId|'+{0}, '', false)", e.VisibleIndex));
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

        public void ProcessAction(string parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(parameter) && (View.Id == "TestMethod_ListView_TestInstrument"||View.Id== "TestMethod_ListView_FieldInstrument"))
                {
                    string[] param = parameter.Split('|');
                    if (param[0] == "InstrumentId")
                    {
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                        {
                            editor.Grid.UpdateEdit();
                            object currentOid = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            IObjectSpace os = Application.CreateObjectSpace();
                            Modules.BusinessObjects.Setting.TestMethod objTestMethod = os.GetObjectByKey<Modules.BusinessObjects.Setting.TestMethod>(currentOid);

                            if (objTestMethod != null)
                            {
                                curTestOid = objTestMethod.Oid;
                                testInstrument.lstExistingInstruments = objTestMethod.Labwares.Select(i => i.Oid).ToList();
                                ShowViewParameters showViewParameters = new ShowViewParameters();
                                IObjectSpace labwareObjectSpace = Application.CreateObjectSpace();
                                CollectionSource labwareCollectionSource = new CollectionSource(labwareObjectSpace, typeof(Modules.BusinessObjects.Assets.Labware));
                                showViewParameters.CreatedView = Application.CreateListView("Labware_ListView_Instrument", labwareCollectionSource, false);
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
                else if (!string.IsNullOrEmpty(parameter) && View.Id == "TestMethod_ListView_SamplePrep_TestInstrument")
                {
                    string[] param = parameter.Split('|');
                    if (param[0] == "InstrumentId")
                    {
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                        {
                            editor.Grid.UpdateEdit();
                            object currentOid = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            IObjectSpace os = Application.CreateObjectSpace();
                            Modules.BusinessObjects.Setting.TestMethod objTestMethod = os.GetObjectByKey<Modules.BusinessObjects.Setting.TestMethod>(currentOid);

                            if (objTestMethod != null)
                            {
                                curTestOid = objTestMethod.Oid;
                                testInstrument.lstExistingInstruments = objTestMethod.SamplePrepLabwares.Select(i => i.Oid).ToList();
                                ShowViewParameters showViewParameters = new ShowViewParameters();
                                IObjectSpace labwareObjectSpace = Application.CreateObjectSpace();
                                CollectionSource labwareCollectionSource = new CollectionSource(labwareObjectSpace, typeof(Modules.BusinessObjects.Assets.Labware));
                                showViewParameters.CreatedView = Application.CreateListView("Labware_ListView_Instrument", labwareCollectionSource, false);
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
        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (View.Id == "TestMethod_ListView_TestInstrument" || View.Id== "TestMethod_ListView_FieldInstrument")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    TestMethod objtestMethod = os.GetObjectByKey<TestMethod>(curTestOid);
                    List<Guid> lstAddedInstruments = e.AcceptActionArgs.SelectedObjects.Cast<Modules.BusinessObjects.Assets.Labware>().Select(i => i.Oid).ToList();
                    List<Guid> lstExistingInstruments = objtestMethod.Labwares.Select(i => i.Oid).ToList();
                    List<Guid> lstRemovedIntruments = lstExistingInstruments.Except(lstAddedInstruments).ToList();

                    foreach (Guid oid in lstAddedInstruments)
                    {
                        Modules.BusinessObjects.Assets.Labware addedInstrument = os.GetObjectByKey<Modules.BusinessObjects.Assets.Labware>(oid);
                        if (addedInstrument != null && !objtestMethod.Labwares.Contains(addedInstrument))
                        {
                            objtestMethod.Labwares.Add(addedInstrument);
                        }
                    }

                    foreach (Guid oid in lstRemovedIntruments)
                    {
                        Modules.BusinessObjects.Assets.Labware removedInstrument = os.GetObjectByKey<Modules.BusinessObjects.Assets.Labware>(oid);
                        if (removedInstrument != null && objtestMethod.Labwares.Contains(removedInstrument))
                        {
                            objtestMethod.Labwares.Remove(removedInstrument);
                        }
                    }
                    os.CommitChanges();
                    View.ObjectSpace.Refresh();
                }
                else if (View.Id == "TestMethod_ListView_SamplePrep_TestInstrument")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    TestMethod objtestMethod = os.GetObjectByKey<TestMethod>(curTestOid);
                    List<Guid> lstAddedInstruments = e.AcceptActionArgs.SelectedObjects.Cast<Modules.BusinessObjects.Assets.Labware>().Select(i => i.Oid).ToList();
                    List<Guid> lstExistingInstruments = objtestMethod.SamplePrepLabwares.Select(i => i.Oid).ToList();
                    List<Guid> lstRemovedIntruments = lstExistingInstruments.Except(lstAddedInstruments).ToList();

                    foreach (Guid oid in lstAddedInstruments)
                    {
                        Modules.BusinessObjects.Assets.Labware addedInstrument = os.GetObjectByKey<Modules.BusinessObjects.Assets.Labware>(oid);
                        if (addedInstrument != null && !objtestMethod.SamplePrepLabwares.Contains(addedInstrument))
                        {
                            objtestMethod.SamplePrepLabwares.Add(addedInstrument);
                        }
                    }

                    foreach (Guid oid in lstRemovedIntruments)
                    {
                        Modules.BusinessObjects.Assets.Labware removedInstrument = os.GetObjectByKey<Modules.BusinessObjects.Assets.Labware>(oid);
                        if (removedInstrument != null && objtestMethod.SamplePrepLabwares.Contains(removedInstrument))
                        {
                            objtestMethod.SamplePrepLabwares.Remove(removedInstrument);
                        }
                    }
                    os.CommitChanges();
                    View.ObjectSpace.Refresh();
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

