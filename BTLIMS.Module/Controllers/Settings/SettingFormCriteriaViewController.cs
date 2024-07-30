using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;
using Container = Modules.BusinessObjects.Setting.Container;

namespace Modules.Controllers.Setting
{
    public class TestMethodViewController : ViewController, IXafCallbackHandler
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        private System.ComponentModel.IContainer components;
        TestMethodInfo objInfo = new TestMethodInfo();
        ContainerSettingInfo consetinfo = new ContainerSettingInfo();
        TestInfo objtestinfo = new TestInfo();
        bool Isparameter = false;
        private SimpleAction Save_btn;
        Guid tempclient = Guid.Empty;
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        private PopupWindowShowAction ImportFilesNew;
        VersionControlInfo objVersion = new VersionControlInfo();
        spreadsheetitemsltno itemsltno = new spreadsheetitemsltno();
        string strTestname = string.Empty;
        string strMethodName = string.Empty;
        string strMatrix = string.Empty;
        string strVisualMatrix = string.Empty;
        string strTestCode = string.Empty;
        private SimpleAction ExportTest;
        uint intDaykeepSamples = 0;
        string strComponent = string.Empty;
        string strClauseSubject = string.Empty;
        string strComment = string.Empty;
        string strMethodNumber = string.Empty;
        string strCategory = string.Empty;
        string strQcType = string.Empty;
         bool strIsfieldTest = false;
        string strParameter = string.Empty;
        string strQCRole = string.Empty;
        string strQCSource = string.Empty;
        string strQCRootRole = string.Empty;
        AuditInfo objAuditInfo = new AuditInfo();


        NavigationInfo objNavInfo = new NavigationInfo();
        #endregion

        #region Constructor
        public TestMethodViewController()
        {
            InitializeComponent();
            this.TargetViewId = "Project_ListView_samplesite;" + "SampleSites_DetailView;" + "SampleSites_ListView;" + "TestMethod_DetailView;" + "QCType_DetailView;" + "TestMethod_ListView;" + "QCType_ListView;" + "Project_LookupListView_Copy_SampleCheckIn;" + "Contact_LookupListView_Copy_SampleCheckin;" + "Method_LookupListView;" + "TestMethod_LookupListView_Copy_GroupTest;" + "Parameter_LookupListView;" + "Testparameter_ListView_Copy;" + "GroupTest_DetailView;" + "TestMethod_LookupListView_Copy_GroupTest;" + "Customer_LookupListView_Copy_SampleCheckin;"
                + "Method_ListView_Copy_TestMethod;" + "Testparameter_ListView_Test_SampleParameter;" + "Testparameter_ListView_Test_QCSampleParameter;" + "QCType_LookupListView_Copy_TestMethod;" + "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault;"
                + "Testparameter_ListView_Test_InternalStandards;" + "TestMethod_surrogates_ListView;" + "Testparameter_LookupListView_Component;" + "Component_ListView_Test;" + "Testparameter_ListView_Test_Surrogates;"
                + "TestMethod_QCTypes_ListView;" + "ContainerSettings_ListView;" + "ContainerSettings_ListView_testmethod;" + "Preservative_ListView_ContainerSetting;" + "Container_ListView_ContainerSettings;"
                + "ContainerSettings_ListView_testmethod_Edit;" + "ContainerSettings_DetailView;" + "DataEntry_DetailView;" + "TestMethod_LookupListView_DataEntry;" + "TestMethod_LookupListView_DataEntry_Copy"
                + "TestMethod_QCTypes_ListView;" + "TestMethod_TestGuides_ListView;" + "TestMethod_PrepMethods_ListView;" + "TestMethod_SamplingMethods_ListView;" + "TestMethod_ListView_Copyto;" + "TestMethod_DetailView_CopyTest;"
                + "SamplePrepBatch_DetailView;" + "HelpCenterAttachments_DetailView;" + "VersionLog_DetailView;" + "InstrumentSoftware_DetailView;" + "UnFollowSettings_DetailView;" + "UnFollowSettings_ListView;" + "ScreenAutoLock_DetailView;" + "TestMethod_ListView_Copy_Copy;" + "Matrix_ListView;" + "VisualMatrix_ListView;" + "QCType_ListView;" + "Method_ListView;" + "Parameter_ListView;" + "QCType_ListView;" + "MCLAndSCLLimits_ListView";
            Save_btn.TargetViewId = "ContainerSettings_ListView_testmethod;" + "SampleSites_ListView;" + "TestMethod_ListView_Copy_Copy;";
            ImportFilesNew.TargetViewId = "TestMethod_ListView;" + "Matrix_ListView;" + "VisualMatrix_ListView;" + "QCType_ListView;" + "Method_ListView;" + "Parameter_ListView;" + "QCType_ListView;";
            ExportTest.TargetViewId = "TestMethod_ListView;" + "Matrix_ListView;" + "VisualMatrix_ListView;" + "QCType_ListView;" + "Method_ListView;" + "Parameter_ListView;" + "QCType_ListView;";
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (View != null && View.ObjectTypeInfo.Type == typeof(TestMethod))
                {
                    ObjectSpace.Committing += new EventHandler<System.ComponentModel.CancelEventArgs>(ObjectSpace_Committing);
                }
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                if (View.Id == "ContainerSettings_ListView" || View.Id == "ContainerSettings_DetailView" || View.Id == "Component_ListView_Test")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    if (View.Id == "ContainerSettings_ListView" || View.Id == "ContainerSettings_DetailView")
                    {
                        Frame.GetController<NewObjectViewController>().NewObjectAction.Execute += NewObjectAction_Execute;
                    }
                }
                else if (View.Id == "SampleSites_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executing += SaveAction_Executing;
                }
                else if (View.Id == "TestMethod_DetailView")
                {
                    objAuditInfo.currentViewOid = null;
                    TestMethod obj = (TestMethod)View.CurrentObject;
                    SRInfo.CurrentTest = obj;
                    if (obj != null)
                    {
                        objAuditInfo.currentViewOid = obj.Oid;

                    }

                    Frame.GetController<ModificationsController>().SaveAction.Execute += SaveAction_Execute;

                }
                if (View.Id == "ContainerSettings_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("Hide", false);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("Hide", false);
                }
                if (View.Id == "ContainerSettings_ListView_testmethod")
                {
                    if (consetinfo.lsthtvalues == null)
                    {
                        consetinfo.lsthtvalues = new List<string>();
                    }
                    IList<TestMethod> lsttestmed = View.ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[IsGroup] <> True Or [IsGroup] Is Null"));
                    if (lsttestmed != null && lsttestmed.Count > 0)
                    {
                        foreach (TestMethod objtm in lsttestmed.ToList())
                        {
                            ContainerSettings chkconset = ObjectSpace.FindObject<ContainerSettings>(CriteriaOperator.Parse("[Test.Oid] = ?", objtm.Oid));
                            if (chkconset == null)
                            {
                                ContainerSettings tempcrtconsettings = View.ObjectSpace.CreateObject<ContainerSettings>();
                                tempcrtconsettings.Matrix = View.ObjectSpace.GetObject(objtm.MatrixName);
                                tempcrtconsettings.Test = View.ObjectSpace.GetObject(objtm);
                                tempcrtconsettings.Method = View.ObjectSpace.GetObject(objtm);
                                ((ListView)View).CollectionSource.Add(tempcrtconsettings);
                            }
                        }
                    }
                }
                if (View.Id == "Testparameter_LookupListView_Component")
                {
                    TestMethod objTest = ObjectSpace.GetObject((TestMethod)Application.MainWindow.View.CurrentObject);
                    if (objTest != null && objTest.MethodName != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[QCType.QCTypeName] = 'Sample' And [TestMethod]=?", objTest.Oid);
                        //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod.MethodName.MethodName] = ? And [TestMethod.MethodName.MethodNumber] = ?", objTest.MethodName.MethodName, objTest.MethodName.MethodNumber);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is null");
                    }
                }


                else if (View.Id == "Component_ListView_Test")
                {
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    TestMethod objTest = ObjectSpace.GetObject((TestMethod)Application.MainWindow.View.CurrentObject);
                    if (objTest != null && objTest.MethodName != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod]=?", objTest.Oid);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is null");
                    }
                }
                if (View.Id == "UnFollowSettings_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("Hide", false);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("Hide", false);
                    IObjectSpace os = Application.CreateObjectSpace();
                    List<UnFollowSettings> lstunfollow = os.GetObjects<UnFollowSettings>(CriteriaOperator.Parse("")).ToList();
                    if (lstunfollow != null && lstunfollow.Count == 0)
                    {
                        UnFollowSettings crtunfollowset = os.CreateObject<UnFollowSettings>();
                        crtunfollowset.UnfollowedClient = 0;
                        crtunfollowset.UnfollowedProspect = 0;
                        os.CommitChanges();
                    }
                    // View.Close();
                    List<UnFollowSettings> lstchkunfollow = os.GetObjects<UnFollowSettings>(CriteriaOperator.Parse("")).ToList();
                    if (lstchkunfollow != null && lstchkunfollow.Count == 1)
                    {
                        foreach (UnFollowSettings objunfol in lstchkunfollow.ToList())
                        {
                            View.CurrentObject = View.ObjectSpace.GetObject(objunfol);
                            //DetailView dvunfol = Application.CreateDetailView(os, "UnFollowSettings_DetailView_Copy", false, objunfol);
                            //dvunfol.ViewEditMode = ViewEditMode.View;
                            //Frame.SetView(dvunfol);
                        }
                    }
                }

                if ((View is ListView) && (View.ObjectTypeInfo.Type == typeof(TestMethod)))
                {
                    ((ListView)View).CollectionSource.Criteria["Filter2"] = CriteriaOperator.Parse("[MethodName.GCRecord] IS NULL");
                    ((ListView)View).CollectionSource.Criteria["Filter3"] = CriteriaOperator.Parse("[MatrixName.GCRecord] IS NULL");
                }
                else if ((View is ListView) && (View.ObjectTypeInfo.Type == typeof(QCType)))
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[QCTypeName] <> ?", "Sample");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "SampleSites_DetailView")
                {
                    //SampleSites objsmplsite = (SampleSites)View.CurrentObject;
                    //if (objsmplsite != null && objsmplsite.Client != null)
                    //{
                    //    objsmplsite.Customer = objsmplsite.Client;
                    //}
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

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupControl_CustomizePopupWindowSize(object sender, CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "ReportDesigner_DetailView")
                {
                    e.Width = 800;
                    e.Height = 900;
                    e.Handled = true;
                }
                else if (View != null && View.Id == "SamplePrepBatch_DetailView")
                {
                    e.Width = 800;
                    e.Height = 300;
                    e.Handled = true;
                }
                else if (View != null && View.Id == "HelpCenterAttachments_DetailView")
                {
                    e.Width = 800;
                    e.Height = 300;
                    e.Handled = true;
                }
                else if (View != null && View.Id == "VersionLog_DetailView")
                {
                    e.Width = 800;
                    e.Height = 300;
                    e.Handled = true;
                }
                else if (View != null && View.Id == "InstrumentSoftware_DetailView")
                {
                    e.Width = 800;
                    e.Height = 300;
                    e.Handled = true;
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
                if (View.Id == "ContainerSettings_ListView" || View.Id == "ContainerSettings_DetailView")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(ContainerSettings));
                    ListView lvtestmethod = Application.CreateListView("ContainerSettings_ListView_testmethod", cs, true);
                    e.ShowViewParameters.CreatedView = lvtestmethod;
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
                if (View.Id == "Component_ListView_Test")
                {
                    e.Cancel = true;
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    Modules.BusinessObjects.Setting.Component objComponent = objectSpace.GetObject((Modules.BusinessObjects.Setting.Component)View.CurrentObject);
                    DetailView dv = Application.CreateDetailView(objectSpace, "Component_DetailView_Test", true, objComponent);
                    dv.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters();
                    showViewParameters.CreatedView = dv;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    //dc.SaveOnAccept = false;
                    //dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += Dc_Accepting;
                    dc.AcceptAction.Executed += AcceptAction_Executed;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                else if (View.Id == "ContainerSettings_ListView")
                {
                    if (View.CurrentObject != null)
                    {
                        ContainerSettings objcurt = (ContainerSettings)View.CurrentObject;
                        DetailView objdv = Application.CreateDetailView(ObjectSpace, objcurt);
                        objdv.ViewEditMode = ViewEditMode.Edit;
                        Frame.SetView(objdv);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        private void NewObjectAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "ContainerSettings_ListView" || View.Id == "ContainerSettings_DetailView")
                {
                    e.Cancel = true;
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(ContainerSettings));
                    //cs.Criteria["filter"] = CriteriaOperator.Parse("Oid is Null");
                    ListView lvtestmethod = Application.CreateListView("ContainerSettings_ListView_testmethod", cs, true);
                    Frame.SetView(lvtestmethod);
                }
                if (View.Id == "Component_ListView_Test")
                {
                    e.Cancel = true;
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    Modules.BusinessObjects.Setting.Component obj = objectSpace.CreateObject<Modules.BusinessObjects.Setting.Component>();
                    DetailView dv = Application.CreateDetailView(objectSpace, "Component_DetailView", true, obj);
                    ShowViewParameters showViewParameters = new ShowViewParameters();
                    showViewParameters.CreatedView = dv;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    //dc.SaveOnAccept = false;
                    //dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += Dc_Accepting;
                    dc.AcceptAction.Executed += AcceptAction_Executed;
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

        private void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                View.ObjectSpace.Refresh();
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
                DialogController objDialog = (DialogController)sender as DialogController;
                if (objDialog != null && objDialog.Frame.View is DetailView)
                {
                    DetailView view = (DetailView)objDialog.Frame.View;
                    Modules.BusinessObjects.Setting.Component objCombonent = ((Modules.BusinessObjects.Setting.Component)e.AcceptActionArgs.CurrentObject);
                    if (objCombonent != null && objCombonent.TestMethod == null)
                    {
                        TestMethod objTest = ((TestMethod)Application.MainWindow.View.CurrentObject);
                        if (objTest != null)
                        {
                            objCombonent.TestMethod = view.ObjectSpace.GetObjectByKey<TestMethod>(objTest.Oid);
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

        private void SaveAndCloseAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "TestMethod_DetailView")
                {
                    ListPropertyEditor lstMethod = ((DetailView)Application.MainWindow.View).FindItem("Methods") as ListPropertyEditor;
                    ListPropertyEditor lstPrepMethod = ((DetailView)Application.MainWindow.View).FindItem("PrepMethods") as ListPropertyEditor;
                    ListPropertyEditor lstSamplingMethod = ((DetailView)Application.MainWindow.View).FindItem("SamplingMethods") as ListPropertyEditor;
                    ListPropertyEditor lstQcTypes = ((DetailView)Application.MainWindow.View).FindItem("QCTypes") as ListPropertyEditor;
                    DashboardViewItem InternalStandardsLv = ((DetailView)View).FindItem("InternalStandards") as DashboardViewItem;
                    DashboardViewItem QCSampleParameterLv = ((DetailView)View).FindItem("QCSampleParameter") as DashboardViewItem;
                    DashboardViewItem SampleParameterLv = ((DetailView)View).FindItem("SampleParameter") as DashboardViewItem;
                    DashboardViewItem ComponentsLv = ((DetailView)View).FindItem("Components") as DashboardViewItem;
                    if (lstMethod != null && lstMethod.ListView != null)
                    {
                        ((ASPxGridListEditor)((ListView)lstMethod.ListView).Editor).Grid.UpdateEdit();
                    }
                    if (lstPrepMethod != null && lstPrepMethod.ListView != null)
                    {
                        ((ASPxGridListEditor)((ListView)lstPrepMethod.ListView).Editor).Grid.UpdateEdit();
                    }
                    if (lstSamplingMethod != null && lstSamplingMethod.ListView != null)
                    {
                        ((ASPxGridListEditor)((ListView)lstSamplingMethod.ListView).Editor).Grid.UpdateEdit();
                    }
                    if (lstQcTypes != null && lstQcTypes.ListView != null)
                    {
                        ((ASPxGridListEditor)((ListView)lstQcTypes.ListView).Editor).Grid.UpdateEdit();
                    }
                    if (InternalStandardsLv != null && InternalStandardsLv.InnerView != null)
                    {
                        ((ASPxGridListEditor)((ListView)InternalStandardsLv.InnerView).Editor).Grid.UpdateEdit();
                    }
                    if (QCSampleParameterLv != null && QCSampleParameterLv.InnerView != null)
                    {
                        ((ASPxGridListEditor)((ListView)QCSampleParameterLv.InnerView).Editor).Grid.UpdateEdit();
                    }
                    if (SampleParameterLv != null && SampleParameterLv.InnerView != null)
                    {
                        ((ASPxGridListEditor)((ListView)SampleParameterLv.InnerView).Editor).Grid.UpdateEdit();
                    }
                    if (ComponentsLv != null && ComponentsLv.InnerView != null)
                    {
                        ((ASPxGridListEditor)((ListView)ComponentsLv.InnerView).Editor).Grid.UpdateEdit();
                    }
                }
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
                if (View.Id == "TestMethod_DetailView")
                {
                    ListPropertyEditor lstMethod = ((DetailView)Application.MainWindow.View).FindItem("Methods") as ListPropertyEditor;
                    ListPropertyEditor lstPrepMethod = ((DetailView)Application.MainWindow.View).FindItem("PrepMethods") as ListPropertyEditor;
                    ListPropertyEditor lstSamplingMethod = ((DetailView)Application.MainWindow.View).FindItem("SamplingMethods") as ListPropertyEditor;
                    ListPropertyEditor lstQcTypes = ((DetailView)Application.MainWindow.View).FindItem("QCTypes") as ListPropertyEditor;
                    DashboardViewItem InternalStandardsLv = ((DetailView)View).FindItem("InternalStandards") as DashboardViewItem;
                    DashboardViewItem QCSampleParameterLv = ((DetailView)View).FindItem("QCSampleParameter") as DashboardViewItem;
                    DashboardViewItem SampleParameterLv = ((DetailView)View).FindItem("SampleParameter") as DashboardViewItem;
                    DashboardViewItem ComponentsLv = ((DetailView)View).FindItem("Components") as DashboardViewItem;
                    if (lstMethod != null && lstMethod.ListView != null)
                    {
                        if (((ASPxGridListEditor)((ListView)lstMethod.ListView).Editor).Grid != null)
                        {
                            ((ASPxGridListEditor)((ListView)lstMethod.ListView).Editor).Grid.UpdateEdit();
                        }
                    }
                    if (lstPrepMethod != null && lstPrepMethod.ListView != null)
                    {
                        if (((ASPxGridListEditor)((ListView)lstPrepMethod.ListView).Editor).Grid != null)
                        {
                            ((ASPxGridListEditor)((ListView)lstPrepMethod.ListView).Editor).Grid.UpdateEdit();
                        }
                    }
                    if (lstSamplingMethod != null && lstSamplingMethod.ListView != null)
                    {
                        if (((ASPxGridListEditor)((ListView)lstSamplingMethod.ListView).Editor).Grid != null)
                        {
                            ((ASPxGridListEditor)((ListView)lstSamplingMethod.ListView).Editor).Grid.UpdateEdit();
                        }
                    }
                    if (lstQcTypes != null && lstQcTypes.ListView != null)
                    {
                        if (((ASPxGridListEditor)((ListView)lstQcTypes.ListView).Editor).Grid != null)
                        {
                            ((ASPxGridListEditor)((ListView)lstQcTypes.ListView).Editor).Grid.UpdateEdit();
                        }
                    }
                    if (InternalStandardsLv != null && InternalStandardsLv.InnerView != null)
                    {
                        if (((ASPxGridListEditor)((ListView)InternalStandardsLv.InnerView).Editor).Grid != null)
                        {
                            ((ASPxGridListEditor)((ListView)InternalStandardsLv.InnerView).Editor).Grid.UpdateEdit();
                        }
                    }
                    if (QCSampleParameterLv != null && QCSampleParameterLv.InnerView != null)
                    {
                        if (((ASPxGridListEditor)((ListView)QCSampleParameterLv.InnerView).Editor).Grid != null)
                        {
                            ((ASPxGridListEditor)((ListView)QCSampleParameterLv.InnerView).Editor).Grid.UpdateEdit();
                        }
                    }
                    if (SampleParameterLv != null && SampleParameterLv.InnerView != null)
                    {
                        if (((ASPxGridListEditor)((ListView)SampleParameterLv.InnerView).Editor).Grid != null)
                        {
                            ((ASPxGridListEditor)((ListView)SampleParameterLv.InnerView).Editor).Grid.UpdateEdit();
                        }
                    }
                    if (ComponentsLv != null && ComponentsLv.InnerView != null)
                    {
                        if (((ASPxGridListEditor)((ListView)ComponentsLv.InnerView).Editor).Grid != null)
                        {
                            ((ASPxGridListEditor)((ListView)ComponentsLv.InnerView).Editor).Grid.UpdateEdit();
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
                if (View.Id == "SampleSites_ListView")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    }
                }
                else if (View.Id == "TestMethod_SamplingMethods_ListView" || View.Id == "TestMethod_PrepMethods_ListView" || View.Id == "TestMethod_TestGuides_ListView" || View.Id == "TestMethod_QCTypes_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        //gridListEditor.Grid.Settings.VerticalScrollableHeight = 150;
                        //if (((ListView)View).CollectionSource.GetCount()>10)
                        //{
                        //    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        //}
                    }
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.ConfirmationMessage = "You are about to remove selected record(s). Do you want to proceed?";
                    //SimpleAction objDelete = Frame.GetController<DeleteObjectsViewController>().DeleteAction;
                    //if (objDelete != null)
                    //{
                    //    objDelete.Caption = "Delete";
                    //    objDelete.ImageName = "Action_Delete";
                    //    objDelete.ConfirmationMessage = @"You are about to delete the selected record(s). Do you want to proceed?";
                    //}
                }
                else if (View.Id == "ContainerSettings_DetailView")
                {
                    foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.IsCaptionVisible == true))
                    {
                        if (item is ASPxStringPropertyEditor)
                        {
                            ASPxStringPropertyEditor editor = (ASPxStringPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxIntPropertyEditor)
                        {
                            ASPxIntPropertyEditor editor = (ASPxIntPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxDoublePropertyEditor)
                        {
                            ASPxDoublePropertyEditor editor = (ASPxDoublePropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxDecimalPropertyEditor)
                        {
                            ASPxDecimalPropertyEditor editor = (ASPxDecimalPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxDateTimePropertyEditor)
                        {
                            ASPxDateTimePropertyEditor editor = (ASPxDateTimePropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxLookupPropertyEditor)
                        {
                            ASPxLookupPropertyEditor editor = (ASPxLookupPropertyEditor)item;
                            if (editor != null && editor.DropDownEdit != null && editor.DropDownEdit.DropDown != null)
                            {
                                editor.DropDownEdit.DropDown.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxBooleanPropertyEditor)
                        {
                            ASPxBooleanPropertyEditor editor = (ASPxBooleanPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxEnumPropertyEditor)
                        {
                            ASPxEnumPropertyEditor editor = (ASPxEnumPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxGridLookupPropertyEditor)
                        {
                            ASPxGridLookupPropertyEditor editor = (ASPxGridLookupPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                    }
                }
                else if (View.Id == "ContainerSettings_ListView_testmethod")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("ContainerSetting", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        ASPxGridView gv = gridListEditor.Grid;
                        if (gv != null)
                        {
                            gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            gv.HtmlDataCellPrepared += Gv_HtmlDataCellPrepared;
                        }
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                    window.setTimeout(function() {   
                    var htprep = s.batchEditApi.GetCellValue(e.visibleIndex, 'HTBeforePrep');
                    var htanalysis = s.batchEditApi.GetCellValue(e.visibleIndex, 'HTBeforeAnalysis');
                    if (e.visibleIndex != '-1')
                    {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) 
                        {
                                      if(s.batchEditApi.HasChanges(e.visibleIndex, 'HTBeforePrep')) 
                                      {
                                            RaiseXafCallback(globalCallbackControl,'ContainerSetting' , 'htvalue|'+ Oidvalue+'|'+htprep+'|'+htanalysis, '', false);
                                      }
                                      else if(s.batchEditApi.HasChanges(e.visibleIndex, 'HTBeforeAnalysis'))
                                      {
                                            RaiseXafCallback(globalCallbackControl,'ContainerSetting' , 'htvalue|'+ Oidvalue+'|'+htprep+'|'+htanalysis, '', false);
                                      }
                        });
                       
                    }                                  
                    }, 20);}";
                    }
                }
                else if (View.Id == "ContainerSettings_ListView")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("ContainerSettingEdit", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        ASPxGridView gv = gridListEditor.Grid;
                        if (gv != null)
                        {
                            gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            gv.HtmlDataCellPrepared += Gv_HtmlDataCellPrepared;
                        }
                    }
                }
                else if (View.Id == "ContainerSettings_ListView_testmethod_Edit")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("ContainerSetting", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        ASPxGridView gv = gridListEditor.Grid;
                        if (gv != null)
                        {
                            gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            gv.HtmlDataCellPrepared += Gv_HtmlDataCellPrepared;
                        }
                    }
                    foreach (ContainerSettings objconset in ((ListView)View).CollectionSource.List)
                    {
                        consetinfo.strcontainer = objconset.Container;
                        consetinfo.strpreservative = objconset.Preservative;
                    }
                }
                else if (View.Id == "Testparameter_ListView_Test_SampleParameter" || View.Id == "Testparameter_ListView_Test_QCSampleParameter"
              || View.Id == "Testparameter_ListView_Test_InternalStandards" || View.Id == "TestMethod_surrogates_ListView" || View.Id == "Testparameter_ListView_Test_Surrogates" || View.Id == "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        ListView lv = (ListView)this.View;
                        // ((IModelListViewWeb)lv.Model).PageSize = 5;
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 150;
                        gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                    }
                }
                if (View.Id == "Component_ListView_Test")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        ASPxGridView gridView = gridListEditor.Grid;
                        if (gridView != null)
                        {
                            List<TabFieldConfiguration> lstFields = ObjectSpace.GetObjects<TabFieldConfiguration>(CriteriaOperator.Parse("[TabName] = 'Components'")).ToList();
                            if (lstFields.Count > 0)
                            {
                                foreach (WebColumnBase column in gridView.Columns)
                                {
                                    if (column.Name == "SelectionCommandColumn" || column.Name == "Parameter" || column.Name == "Edit")
                                    {
                                        gridView.VisibleColumns[column.Name].FixedStyle = GridViewColumnFixedStyle.Left;
                                    }
                                    else
                                    {
                                        IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                        if (columnInfo != null)
                                        {
                                            if (lstFields != null)
                                            {
                                                TabFieldConfiguration curField = lstFields.FirstOrDefault(i => i.FieldID == columnInfo.Model.Id);
                                                if (curField != null)
                                                {
                                                    column.Visible = true;
                                                    if (!string.IsNullOrEmpty(curField.FieldCustomCaption))
                                                    {
                                                        column.Caption = curField.FieldCustomCaption;
                                                    }
                                                    else
                                                    {
                                                        column.Caption = curField.FieldCaption;
                                                    }
                                                    if (curField.SortOrder > 0)
                                                    {
                                                        column.VisibleIndex = curField.SortOrder + 1;
                                                    }
                                                    if (curField.Freeze)
                                                    {
                                                        gridView.VisibleColumns[columnInfo.Model.Id].FixedStyle = GridViewColumnFixedStyle.Left;
                                                    }
                                                    if (curField.Width > 0)
                                                    {
                                                        column.Width = curField.Width;
                                                    }
                                                }
                                                else
                                                {
                                                    column.Visible = false;
                                                }
                                            }
                                            else
                                            {
                                                column.Visible = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (View.Id == "Method_ListView_Copy_TestMethod")
                {
                    TestMethod objTest = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (objTest != null && objTest.MethodName != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[MethodName] = ? And[MethodNumber] = ?", objTest.MethodName.MethodName, objTest.MethodName.MethodNumber);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is null");
                    }
                }
                else if (View.Id == "Testparameter_ListView_Test_SampleParameter")
                {
                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(TestMethod))
                    {
                        TestMethod objTest = (TestMethod)Application.MainWindow.View.CurrentObject;
                        if (objTest != null && !((ListView)View).CollectionSource.Criteria.ContainsKey("Filter"))
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[QCType.QCTypeName] ='Sample' And [TestMethod] = ? And Component.Components='Default'", objTest.Oid);
                        }
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            ASPxGridView gridView = gridListEditor.Grid;
                            if (gridView != null)
                            {
                                //List<TabFieldConfiguration> lstFields = ObjectSpace.GetObjects<TabFieldConfiguration>(CriteriaOperator.Parse("[TabName] = 'SampleParameters'")).ToList();
                                //foreach (WebColumnBase column in gridView.Columns)
                                //{
                                //    if (column.Name == "SelectionCommandColumn" || column.Name == "Parameter" || column.Name == "Edit")
                                //    {
                                //        gridView.VisibleColumns[column.Name].FixedStyle = GridViewColumnFixedStyle.Left;
                                //    }
                                //    else
                                //    {
                                //        IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                //        if (columnInfo != null)
                                //        {
                                //            if (lstFields != null)
                                //            {
                                //                TabFieldConfiguration curField = lstFields.FirstOrDefault(i => i.FieldID == columnInfo.Model.Id);
                                //                if (curField != null)
                                //                {
                                //                    column.Visible = true;
                                //                    if (!string.IsNullOrEmpty(curField.FieldCustomCaption))
                                //                    {
                                //                        column.Caption = curField.FieldCustomCaption;
                                //                    }
                                //                    else
                                //                    {
                                //                        column.Caption = curField.FieldCaption;
                                //                    }
                                //                    if (curField.SortOrder > 0)
                                //                    {
                                //                        column.VisibleIndex = curField.SortOrder + 1;
                                //                    }
                                //                    if (curField.Freeze)
                                //                    {
                                //                        gridView.VisibleColumns[columnInfo.Model.Id].FixedStyle = GridViewColumnFixedStyle.Left;
                                //                    }
                                //                    if (curField.Width > 0)
                                //                    {
                                //                        column.Width = curField.Width;
                                //                    }
                                //                }
                                //                else
                                //                {
                                //                    column.Visible = false;
                                //                }
                                //            }
                                //            else
                                //            {
                                //                column.Visible = false;
                                //            }
                                //        }
                                //    }
                                //}
                            }
                        }
                    }
                    else
                    {
                        TestMethod objTest = (TestMethod)View.CurrentObject;
                        if (objTest != null && !((ListView)View).CollectionSource.Criteria.ContainsKey("Filter"))
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[QCType.QCTypeName] ='Sample' And [TestMethod] = ? And Component.Components='Default'", objTest.Oid);
                        }
                    }
                }
                else if (View.Id == "Testparameter_ListView_Test_QCSampleParameter")
                {
                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(TestMethod))
                    {
                        TestMethod objTest = (TestMethod)Application.MainWindow.View.CurrentObject;
                        if (objTest != null && !((ListView)View).CollectionSource.Criteria.ContainsKey("Filter"))
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[QCType.QCTypeName] <> 'Sample' And[TestMethod] = ?", objTest.Oid);
                        }
                    }
                    else
                    {
                        TestMethod objTest = (TestMethod)View.CurrentObject;
                        if (objTest != null && !((ListView)View).CollectionSource.Criteria.ContainsKey("Filter"))
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[QCType.QCTypeName] <> 'Sample' And[TestMethod] = ?", objTest.Oid);
                        }
                    }
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        ASPxGridView gridView = gridListEditor.Grid;
                        if (gridView != null)
                        {
                            //List<TabFieldConfiguration> lstFields = ObjectSpace.GetObjects<TabFieldConfiguration>(CriteriaOperator.Parse("[TabName] = 'QCParameters'")).ToList();
                            //foreach (WebColumnBase column in gridView.Columns)
                            //{
                            //    if (column.Name == "SelectionCommandColumn" || column.Name == "Parameter")
                            //    {
                            //        gridView.VisibleColumns[column.Name].FixedStyle = GridViewColumnFixedStyle.Left;
                            //    }
                            //    else
                            //    {
                            //        IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                            //        if (columnInfo != null)
                            //        {
                            //            if (lstFields != null)
                            //            {
                            //                TabFieldConfiguration curField = lstFields.FirstOrDefault(i => i.FieldID == columnInfo.Model.Id);
                            //                if (curField != null)
                            //                {
                            //                    column.Visible = true;
                            //                    if (!string.IsNullOrEmpty(curField.FieldCustomCaption))
                            //                    {
                            //                        column.Caption = curField.FieldCustomCaption;
                            //                    }
                            //                    else
                            //                    {
                            //                        column.Caption = curField.FieldCaption;
                            //                    }
                            //                    if (curField.SortOrder > 0)
                            //                    {
                            //                        column.VisibleIndex = curField.SortOrder + 1;
                            //                    }
                            //                    if (curField.Freeze)
                            //                    {
                            //                        gridView.VisibleColumns[columnInfo.Model.Id].FixedStyle = GridViewColumnFixedStyle.Left;
                            //                    }
                            //                    if (curField.Width > 0)
                            //                    {
                            //                        column.Width = curField.Width;
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    column.Visible = false;
                            //                }
                            //            }
                            //            else
                            //            {
                            //                column.Visible = false;
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                else if (View.Id == "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault")
                {
                    TestMethod objTest = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (objTest != null && !((ListView)View).CollectionSource.Criteria.ContainsKey("Filter"))
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[QCType.QCTypeName] <> 'Sample' And[TestMethod] = ?", objTest.Oid);
                    }
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        ASPxGridView gridView = gridListEditor.Grid;
                        if (gridView != null)
                        {
                            //List<TabFieldConfiguration> lstFields = ObjectSpace.GetObjects<TabFieldConfiguration>(CriteriaOperator.Parse("[TabName] = 'QCParameterDefaults'")).ToList();
                            //foreach (WebColumnBase column in gridView.Columns)
                            //{
                            //    if (column.Name == "SelectionCommandColumn" || column.Name == "Parameter")
                            //    {
                            //        gridView.VisibleColumns[column.Name].FixedStyle = GridViewColumnFixedStyle.Left;
                            //    }
                            //    else
                            //    {
                            //        IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                            //        if (columnInfo != null)
                            //        {
                            //            if (lstFields != null)
                            //            {
                            //                TabFieldConfiguration curField = lstFields.FirstOrDefault(i => i.FieldID == columnInfo.Model.Id);
                            //                if (curField != null)
                            //                {
                            //                    column.Visible = true;
                            //                    if (!string.IsNullOrEmpty(curField.FieldCustomCaption))
                            //                    {
                            //                        column.Caption = curField.FieldCustomCaption;
                            //                    }
                            //                    else
                            //                    {
                            //                        column.Caption = curField.FieldCaption;
                            //                    }
                            //                    if (curField.SortOrder > 0)
                            //                    {
                            //                        column.VisibleIndex = curField.SortOrder + 1;
                            //                    }
                            //                    if (curField.Freeze)
                            //                    {
                            //                        gridView.VisibleColumns[columnInfo.Model.Id].FixedStyle = GridViewColumnFixedStyle.Left;
                            //                    }
                            //                    if (curField.Width > 0)
                            //                    {
                            //                        column.Width = curField.Width;
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    column.Visible = false;
                            //                }
                            //            }
                            //            else
                            //            {
                            //                column.Visible = false;
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                else if (View.Id == "Testparameter_ListView_Test_InternalStandards")
                {
                    TestMethod objTest = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (objTest != null && !((ListView)View).CollectionSource.Criteria.ContainsKey("Filter"))
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod] = ?", objTest.Oid);
                    }
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        ASPxGridView gridView = gridListEditor.Grid;
                        //if (gridView != null)
                        //{
                        //    List<TabFieldConfiguration> lstFields = ObjectSpace.GetObjects<TabFieldConfiguration>(CriteriaOperator.Parse("[TabName] = 'InternalStandards'")).ToList();
                        //    foreach (WebColumnBase column in gridView.Columns)
                        //    {
                        //        if (column.Name == "SelectionCommandColumn" || column.Name == "Parameter")
                        //        {
                        //            gridView.VisibleColumns[column.Name].FixedStyle = GridViewColumnFixedStyle.Left;
                        //        }
                        //        else
                        //        {
                        //            IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                        //            if (columnInfo != null)
                        //            {
                        //                if (lstFields != null)
                        //                {
                        //                    //TabFieldConfiguration curField = lstFields.FirstOrDefault(i => i.FieldID == columnInfo.Model.Id);
                        //                    //if (curField != null)
                        //                    //{
                        //                    //    column.Visible = true;
                        //                    //    if (!string.IsNullOrEmpty(curField.FieldCustomCaption))
                        //                    //    {
                        //                    //        column.Caption = curField.FieldCustomCaption;
                        //                    //    }
                        //                    //    else
                        //                    //    {
                        //                    //        column.Caption = curField.FieldCaption;
                        //                    //    }
                        //                    //    if (curField.SortOrder > 0)
                        //                    //    {
                        //                    //        column.VisibleIndex = curField.SortOrder + 1;
                        //                    //    }
                        //                    //    if (curField.Freeze)
                        //                    //    {
                        //                    //        gridView.VisibleColumns[columnInfo.Model.Id].FixedStyle = GridViewColumnFixedStyle.Left;
                        //                    //    }
                        //                    //    if (curField.Width > 0)
                        //                    //    {
                        //                    //        column.Width = curField.Width;
                        //                    //    }
                        //                    //}
                        //                    //else
                        //                    //{
                        //                    //    column.Visible = false;
                        //                    //}
                        //                }
                        //                else
                        //                {
                        //                    column.Visible = false;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
                else if (View.Id == "Testparameter_ListView_Test_Surrogates")
                {
                    TestMethod objTest = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (objTest != null && !((ListView)View).CollectionSource.Criteria.ContainsKey("Filter"))
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod] = ?", objTest.Oid);
                    }
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //if (gridListEditor != null && gridListEditor.Grid != null)
                    //{
                    //    ASPxGridView gridView = gridListEditor.Grid;
                    //    if (gridView != null)
                    //    {
                    //        List<TabFieldConfiguration> lstFields = ObjectSpace.GetObjects<TabFieldConfiguration>(CriteriaOperator.Parse("[TabName] = 'Surrogates'")).ToList();
                    //        foreach (WebColumnBase column in gridView.Columns)
                    //        {
                    //            if (column.Name == "SelectionCommandColumn" || column.Name == "Parameter")
                    //            {
                    //                gridView.VisibleColumns[column.Name].FixedStyle = GridViewColumnFixedStyle.Left;
                    //            }
                    //            else
                    //            {
                    //                IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                    //                if (columnInfo != null)
                    //                {
                    //                    if (lstFields != null)
                    //                    {
                    //                        TabFieldConfiguration curField = lstFields.FirstOrDefault(i => i.FieldID == columnInfo.Model.Id);
                    //                        if (curField != null)
                    //                        {
                    //                            column.Visible = true;
                    //                            if (!string.IsNullOrEmpty(curField.FieldCustomCaption))
                    //                            {
                    //                                column.Caption = curField.FieldCustomCaption;
                    //                            }
                    //                            else
                    //                            {
                    //                                column.Caption = curField.FieldCaption;
                    //                            }
                    //                            if (curField.SortOrder > 0)
                    //                            {
                    //                                column.VisibleIndex = curField.SortOrder + 1;
                    //                            }
                    //                            if (curField.Freeze)
                    //                            {
                    //                                gridView.VisibleColumns[columnInfo.Model.Id].FixedStyle = GridViewColumnFixedStyle.Left;
                    //                            }
                    //                            if (curField.Width > 0)
                    //                            {
                    //                                column.Width = curField.Width;
                    //                            }
                    //                        }
                    //                        else
                    //                        {
                    //                            column.Visible = false;
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        column.Visible = false;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
                else if (View.Id == "QCType_LookupListView_Copy_TestMethod")
                {
                    TestMethod objTest = (TestMethod)Application.MainWindow.View.CurrentObject;
                    if (objTest != null && objTest.QCTypes != null && objTest.QCTypes.Count > 0)
                    {
                        List<string> lstQCTypes = objTest.QCTypes.Select(i => i.QCTypeName).ToList();
                        ((ListView)View).CollectionSource.Criteria["Filter"] = new NotOperator(new InOperator("QCTypeName", lstQCTypes));
                    }
                }
                else if (View.Id == "TestMethod_ListView_Copyto")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    }
                }
                if (View.Id == "Container_ListView_ContainerSettings")
                {
                    ASPxGridListEditor gridlist = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlist != null)
                    {
                        if (Isparameter == false)
                        {
                            gridlist.Grid.Load += Grid_Load;
                        }
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("ContainerSettingsContainerSelection", this);
                        gridlist.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                            RaiseXafCallback(globalCallbackControl, 'ContainerSettingsContainerSelection', 'Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'ContainerSettingsContainerSelection', 'UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                        RaiseXafCallback(globalCallbackControl, 'ContainerSettingsContainerSelection', 'Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'ContainerSettingsContainerSelection', 'UNSelectall', '', false);                        
                      }                      
                    }";

                    }
                }
                else if (View.Id == "Preservative_ListView_ContainerSetting")
                {
                    ASPxGridListEditor gridlist = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlist != null)
                    {
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("ContainerSettingsPreservativeSelection", this);
                        gridlist.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                            RaiseXafCallback(globalCallbackControl, 'ContainerSettingsPreservativeSelection', 'Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'ContainerSettingsPreservativeSelection', 'UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                        RaiseXafCallback(globalCallbackControl, 'ContainerSettingsPreservativeSelection', 'Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'ContainerSettingsPreservativeSelection', 'UNSelectall', '', false);                        
                      }                      
                    }";
                        gridlist.Grid.Load += Grid_Load;
                    }
                }
                if (View.Id == "MCLAndSCLLimits_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    XafCallbackManager parameter = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    parameter.RegisterHandler("PTStudyLogTypePopup", this);
                    //gridListEditor.Grid.ClientInstanceName = "PTStudyLogType";
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                        if( e.focusedColumn.fieldName == 'AnalyteNotes')
                                             {
                                              e.cancel = true;
                                              }
                                          else
                                               {
                                                    e.cancel = false;
                                               }
                                           }";                  
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
                if (View.Id == "MCLAndSCLLimits_ListView")
                {
                    if (e.DataColumn.FieldName == "AnalyteNotes")
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'PTStudyLogTypePopup', 'AnalyteNotes|'+{0}, '', false)", e.VisibleIndex));
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
                ASPxGridView gridViews = sender as ASPxGridView;
                ASPxGridView gridview = (ASPxGridView)sender;
                if (View.Id == "Container_ListView_ContainerSettings" && Isparameter == false)
                {
                    gridview.JSProperties["cpVisibleRowCount"] = gridview.VisibleRowCount;
                    List<string> lststrcontainer = new List<string>();
                    consetinfo.lstcontainer = new List<string>();
                    string stroid = HttpContext.Current.Session["rowid"].ToString();
                    if (!string.IsNullOrEmpty(stroid))
                    {
                        if (Application.MainWindow.View.Id == "ContainerSettings_ListView_testmethod")
                        {
                            ContainerSettings objconset = Application.MainWindow.View.ObjectSpace.FindObject<ContainerSettings>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objconset != null && !string.IsNullOrEmpty(objconset.Container))
                            {
                                string[] strarr = objconset.Container.Split(';');
                                foreach (string objstr in strarr)
                                {
                                    Container objcon = View.ObjectSpace.FindObject<Container>(CriteriaOperator.Parse("[Oid] = ?", new Guid(objstr)));
                                    if (objcon != null && !lststrcontainer.Contains(objcon.ContainerName))
                                    {
                                        lststrcontainer.Add(objcon.ContainerName);
                                    }
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(consetinfo.strcontainer))
                        {
                            string[] strarr = consetinfo.strcontainer.Split(';');
                            foreach (string objstr in strarr)
                            {
                                lststrcontainer.Add(objstr.Trim());
                            }
                        }
                        for (int i = 0; i <= gridview.VisibleRowCount - 1; i++)
                        {
                            string strcontainer = gridview.GetRowValues(i, "ContainerName").ToString();
                            if (!string.IsNullOrEmpty(strcontainer) && lststrcontainer.Contains(strcontainer.Trim()))
                            {
                                gridview.Selection.SelectRow(i);
                                if (!consetinfo.lstcontainer.Contains(strcontainer.Trim()))
                                {
                                    consetinfo.lstcontainer.Add(strcontainer.Trim());
                                }
                            }
                        }
                    }
                }
                else
                    if (View.Id == "Preservative_ListView_ContainerSetting" && Isparameter == false)
                {
                    gridview.JSProperties["cpVisibleRowCount"] = gridview.VisibleRowCount;
                    consetinfo.lstpreservative = new List<string>();
                    List<string> lststrpreservative = new List<string>();
                    if (Application.MainWindow.View.Id == "ContainerSettings_ListView_testmethod")
                    {

                        ContainerSettings objconset = Application.MainWindow.View.ObjectSpace.FindObject<ContainerSettings>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (objconset != null && !string.IsNullOrEmpty(objconset.Preservative))
                        {
                            string[] strarr = objconset.Preservative.Split(';');
                            foreach (string objstr in strarr)
                            {
                                Preservative objpre = View.ObjectSpace.FindObject<Preservative>(CriteriaOperator.Parse("[Oid] = ?", new Guid(objstr)));
                                if (objpre != null && !lststrpreservative.Contains(objpre.PreservativeName))
                                {
                                    lststrpreservative.Add(objpre.PreservativeName);
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(consetinfo.strpreservative))
                    {
                        string[] strarr = consetinfo.strpreservative.Split(';');
                        foreach (string objstr in strarr)
                        {
                            lststrpreservative.Add(objstr.Trim());
                        }
                    }
                    for (int i = 0; i <= gridview.VisibleRowCount - 1; i++)
                    {
                        string strPreservative = gridview.GetRowValues(i, "PreservativeName").ToString();
                        if (!string.IsNullOrEmpty(strPreservative) && lststrpreservative.Contains(strPreservative.Trim()))
                        {
                            gridview.Selection.SelectRow(i);
                            if (!consetinfo.lstpreservative.Contains(strPreservative.Trim()))
                            {
                                consetinfo.lstpreservative.Add(strPreservative.Trim());
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

        private void Gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id == "ContainerSettings_ListView_testmethod" /*|| View.Id == "ContainerSettings_ListView_testmethod_Edit"*/)
                {

                    if (e.DataColumn.FieldName == "Container")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'ContainerSetting', '{0}|{1}', '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                    if (e.DataColumn.FieldName == "Preservative")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'ContainerSetting', '{0}|{1}', '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                }
                //if(View.Id == "ContainerSettings_ListView")
                //{
                //    e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'ContainerSettingEdit', 'Edit|'+{0}, '', false)", e.VisibleIndex));
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
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                string[] param = parameter.Split('|');
                if (param[0] == "htvalue")
                {
                    if (consetinfo.lsthtvalues.Count > 0)
                    {
                        foreach (string objstr in consetinfo.lsthtvalues.ToList())
                        {
                            string[] strarr = objstr.Split('|');
                            if (strarr[1].Trim().ToString() == param[1].Trim().ToString())
                            {
                                consetinfo.lsthtvalues.Remove(objstr);
                                if (!consetinfo.lsthtvalues.Contains(parameter))
                                {
                                    consetinfo.lsthtvalues.Add(parameter);
                                }
                            }
                            else
                            {
                                if (!consetinfo.lsthtvalues.Contains(parameter))
                                {
                                    consetinfo.lsthtvalues.Add(parameter);
                                }
                            }
                        }
                    }
                    else
                    if (consetinfo.lsthtvalues.Count == 0)
                    {
                        consetinfo.lsthtvalues.Add(parameter);
                    }
                    ////if(!consetinfo.lsthtvalues.Contains(parameter))
                    ////{
                    ////    consetinfo.lsthtvalues.Add(parameter);
                    ////}
                }
                if (param[0] == "Container")
                {
                    List<Guid> lstguid = new List<Guid>();
                    HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                    ContainerSettings objcontainer = Application.MainWindow.View.ObjectSpace.FindObject<ContainerSettings>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(Container));
                    if (objcontainer != null && objcontainer.Test != null && objcontainer.Matrix != null && objcontainer.Method != null)
                    {
                        TestMethod objtm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And  [TestName] = ? And [MethodName.MethodNumber] = ?", objcontainer.Matrix.MatrixName, objcontainer.Test.TestName, objcontainer.Method.MethodName.MethodNumber));
                        if (objtm != null)
                        {
                            List<TestGuide> lsttestguid = ObjectSpace.GetObjects<TestGuide>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid)).ToList();
                            foreach (TestGuide ojtstguid in lsttestguid.ToList())
                            {
                                if (ojtstguid.Container != null && !lstguid.Contains(ojtstguid.Container.Oid))
                                {
                                    lstguid.Add(ojtstguid.Container.Oid);
                                }
                            }
                        }
                    }
                    if (lstguid.Count > 0)
                    {
                        cs.Criteria["filter"] = new InOperator("Oid", lstguid);
                    }
                    else
                    {
                        cs.Criteria["filter"] = CriteriaOperator.Parse("Oid Is Null");
                    }
                    ListView lvcontainer = Application.CreateListView("Container_ListView_ContainerSettings", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(lvcontainer);
                    showViewParameters.CreatedView = lvcontainer;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += DcContainer_Accepting;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                else if (param[0] == "Preservative")
                {
                    List<Guid> lstguid = new List<Guid>();
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(Preservative));
                    ContainerSettings objPreservative = Application.MainWindow.View.ObjectSpace.FindObject<ContainerSettings>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    if (objPreservative != null && objPreservative.Test != null && objPreservative.Matrix != null && objPreservative.Method != null)
                    {
                        TestMethod objtm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And  [TestName] = ? And [MethodName.MethodNumber] = ?", objPreservative.Matrix.MatrixName, objPreservative.Test.TestName, objPreservative.Method.MethodName.MethodNumber));
                        if (objtm != null)
                        {
                            List<TestGuide> lsttestguid = ObjectSpace.GetObjects<TestGuide>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objtm.Oid)).ToList();
                            foreach (TestGuide ojtstguid in lsttestguid.ToList())
                            {
                                if (ojtstguid.Preservative != null && !lstguid.Contains(ojtstguid.Preservative.Oid))
                                {
                                    lstguid.Add(ojtstguid.Preservative.Oid);
                                }
                            }
                        }
                    }
                    if (lstguid.Count > 0)
                    {
                        cs.Criteria["filter"] = new InOperator("Oid", lstguid);
                    }
                    else
                    {
                        cs.Criteria["filter"] = CriteriaOperator.Parse("Oid Is Null");
                    }
                    ListView dv = Application.CreateListView("Preservative_ListView_ContainerSetting", cs, true);
                    ShowViewParameters showViewParameters = new ShowViewParameters();
                    showViewParameters.CreatedView = dv;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += DcPreservative_Accepting;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                if (View.Id == "Container_ListView_ContainerSettings")
                {
                    if (param[0] == "Selected")
                    {
                        Container selobj = View.ObjectSpace.FindObject<Container>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[1])));
                        if (selobj != null && !string.IsNullOrEmpty(selobj.ContainerName) && !consetinfo.lstcontainer.Contains(selobj.ContainerName.Trim()))
                        {
                            consetinfo.lstcontainer.Add(selobj.ContainerName.Trim());
                            gridListEditor.Grid.Selection.SelectRowByKey(selobj.Oid);
                        }
                    }
                    else if (param[0] == "UNSelected")
                    {
                        Container unselobj = View.ObjectSpace.FindObject<Container>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[1])));
                        if (unselobj != null && !string.IsNullOrEmpty(unselobj.ContainerName) && consetinfo.lstcontainer.Contains(unselobj.ContainerName.Trim()))
                        {
                            consetinfo.lstcontainer.Remove(unselobj.ContainerName.Trim());
                            gridListEditor.Grid.Selection.UnselectRowByKey(unselobj.Oid);
                        }
                    }
                    else if (param[0] == "Selectall")
                    {
                        foreach (Container objcon in ((ListView)View).CollectionSource.List.Cast<Container>().OrderBy(i => i.ContainerName).ToList())
                        {
                            if (objcon != null && !string.IsNullOrEmpty(objcon.ContainerName) && !consetinfo.lstcontainer.Contains(objcon.ContainerName.Trim()))
                            {
                                consetinfo.lstcontainer.Add(objcon.ContainerName.Trim());
                                gridListEditor.Grid.Selection.SelectAll();
                            }
                        }
                    }
                    else if (param[0] == "UNSelectall")
                    {
                        gridListEditor.Grid.Selection.UnselectAll();
                        consetinfo.lstcontainer.Clear();
                    }
                    Isparameter = true;
                }
                if (View.Id == "Preservative_ListView_ContainerSetting")
                {
                    if (param[0] == "Selected")
                    {
                        Preservative selobj = View.ObjectSpace.FindObject<Preservative>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[1])));
                        if (selobj != null && !string.IsNullOrEmpty(selobj.PreservativeName) && !consetinfo.lstpreservative.Contains(selobj.PreservativeName.Trim()))
                        {
                            consetinfo.lstpreservative.Add(selobj.PreservativeName.Trim());
                            gridListEditor.Grid.Selection.SelectRowByKey(selobj.Oid);
                        }
                    }
                    else if (param[0] == "UNSelected")
                    {
                        Preservative unselobj = View.ObjectSpace.FindObject<Preservative>(CriteriaOperator.Parse("[Oid] = ?", new Guid(param[1])));
                        if (unselobj != null && !string.IsNullOrEmpty(unselobj.PreservativeName) && consetinfo.lstpreservative.Contains(unselobj.PreservativeName.Trim()))
                        {
                            consetinfo.lstpreservative.Remove(unselobj.PreservativeName.Trim());
                            gridListEditor.Grid.Selection.UnselectRowByKey(unselobj.Oid);
                        }
                    }
                    else if (param[0] == "Selectall")
                    {
                        foreach (Preservative objcon in ((ListView)View).CollectionSource.List.Cast<Preservative>().OrderBy(i => i.PreservativeName).ToList())
                        {
                            if (objcon != null && !string.IsNullOrEmpty(objcon.PreservativeName) && !consetinfo.lstpreservative.Contains(objcon.PreservativeName.Trim()))
                            {
                                consetinfo.lstpreservative.Add(objcon.PreservativeName.Trim());
                                gridListEditor.Grid.Selection.SelectAll();
                            }
                        }
                    }
                    else if (param[0] == "UNSelectall")
                    {
                        consetinfo.lstpreservative.Clear();
                        gridListEditor.Grid.Selection.UnselectAll();
                    }
                    Isparameter = true;
                }
                if (View.Id == "MCLAndSCLLimits_ListView") 
                {
                    if (gridListEditor != null)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        string strGuid = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid").ToString();
                        HttpContext.Current.Session["rowid"] = strGuid;
                        IList<MCLAndSCLLimits> objResults = os.GetObjects<MCLAndSCLLimits>(CriteriaOperator.Parse("Oid= ?", new Guid(strGuid)));

                        if (param[0] == "AnalyteNotes" && !string.IsNullOrEmpty(strGuid))
                        {

                            if (objResults != null)
                            {
                                if (View.Id == "MCLAndSCLLimits_ListView")
                                {

                                    foreach (MCLAndSCLLimits objre in objResults.ToList())
                                    {
                                        MCLAndSCLLimitsAnalyteNotes obj = os.CreateObject<MCLAndSCLLimitsAnalyteNotes>();
                                        obj.Comment = objre.Comment;
                                        DetailView createdView = Application.CreateDetailView(os, "MCLAndSCLLimitsAnalyteNotes_DetailView", true, obj);
                                        createdView.ViewEditMode = ViewEditMode.Edit;
                                        ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                                        showViewParameters.Context = TemplateContext.NestedFrame;
                                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                        showViewParameters.CreatedView = createdView;
                                        DialogController dc = Application.CreateController<DialogController>();
                                        dc.SaveOnAccept = false;
                                        // dc.Accepting += RollBack_Accepting;
                                        dc.CloseOnCurrentObjectProcessing = false;
                                        showViewParameters.Controllers.Add(dc);
                                        dc.Accepting += Dc_Accepting1;
                                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                    }
                                }
                            }
                        }
                    }
                }
                ////if(View.Id == "ContainerSettings_ListView" && param[0] == "Edit")
                ////{
                ////    HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                ////    IObjectSpace objectSpace = Application.CreateObjectSpace();
                ////    CollectionSource cs = new CollectionSource(objectSpace, typeof(ContainerSettings));
                ////    cs.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] = ?",new Guid(HttpContext.Current.Session["rowid"].ToString()));
                ////    ListView lvcontainer = Application.CreateListView("ContainerSettings_ListView_testmethod_Edit", cs, true);
                ////    ShowViewParameters showViewParameters = new ShowViewParameters(lvcontainer);
                ////    showViewParameters.CreatedView = lvcontainer;
                ////    showViewParameters.Context = TemplateContext.PopupWindow;
                ////    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                ////    DialogController dc = Application.CreateController<DialogController>();
                ////    dc.SaveOnAccept = false;
                ////    dc.CloseOnCurrentObjectProcessing = false;
                ////    dc.Accepting += DcContainerSettingsEdit_Accepting;
                ////    showViewParameters.Controllers.Add(dc);
                ////    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                ////}
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
                DialogController objDialog = (DialogController)sender as DialogController;
                //MCLAndSCLLimits objPT = Application.MainWindow.View.CurrentObject as MCLAndSCLLimits;
                ASPxStringPropertyEditor obj = ((DetailView)objDialog.Frame.View).FindItem("Comment") as ASPxStringPropertyEditor;
                if (objDialog != null && objDialog.Frame != null && objDialog.Frame.View != null)
                {
                    DevExpress.ExpressApp.View views = objDialog.Frame.View;
                    if (obj.ControlValue != null)
                    {
                        if (obj != null && !string.IsNullOrEmpty(obj.ControlValue.ToString()))
                        {
                            if (HttpContext.Current.Session["rowid"] != null)
                            {
                                MCLAndSCLLimits objResult = View.ObjectSpace.FindObject<MCLAndSCLLimits>(CriteriaOperator.Parse("Oid=?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                if (objResult.Analyte != null)
                                {
                                    if (objResult != null)
                                    {
                                        objResult.Comment = obj.ControlValue.ToString();
                                        objResult.AnalyteNotes = true;
                                    }
                                    View.ObjectSpace.CommitChanges();
                                    View.ObjectSpace.Refresh();
                                    //((ListView)View).CollectionSource.Reload();
                                }
                            }
                        }
                    }
                    else
                    {
                        MCLAndSCLLimits objResult = View.ObjectSpace.FindObject<MCLAndSCLLimits>(CriteriaOperator.Parse("Oid=?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (objResult.Analyte != null)
                        {
                            if (objResult != null)
                            {
                                objResult.Comment = null;
                                objResult.AnalyteNotes = false;
                            }
                            View.ObjectSpace.CommitChanges();
                            View.ObjectSpace.Refresh();
                            //((ListView)View).CollectionSource.Reload();
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

        private void DcContainerSettingsEdit_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DialogController dc = (DialogController)sender;
                    if (dc != null && dc.Window != null && dc.Window.View != null && dc.Window.View.Id == "ContainerSettings_ListView_testmethod_Edit")
                    {
                        ((ListView)dc.Window.View).ObjectSpace.CommitChanges();
                        View.ObjectSpace.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DcPreservative_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                string strpreservative = string.Empty;
                foreach (string objstrpre in consetinfo.lstpreservative.ToList())
                {
                    Preservative objpreservative = ObjectSpace.FindObject<Preservative>(CriteriaOperator.Parse("[PreservativeName] = ?", objstrpre));
                    if (objpreservative != null && !string.IsNullOrEmpty(objpreservative.PreservativeName))
                    {
                        if (string.IsNullOrEmpty(strpreservative))
                        {
                            strpreservative = objpreservative.Oid.ToString();
                        }
                        else if (!string.IsNullOrEmpty(strpreservative))
                        {
                            strpreservative = strpreservative + "; " + objpreservative.Oid.ToString();
                        }
                    }
                }
                ContainerSettings objconset = View.ObjectSpace.FindObject<ContainerSettings>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                if (objconset != null)
                {
                    objconset.Preservative = strpreservative;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DcContainer_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                string strContainer = string.Empty;
                foreach (string objstrpre in consetinfo.lstcontainer.ToList())
                {
                    Container objcontainer = ObjectSpace.FindObject<Container>(CriteriaOperator.Parse("[ContainerName] = ?", objstrpre));
                    if (objcontainer != null && !string.IsNullOrEmpty(objcontainer.ContainerName))
                    {
                        if (string.IsNullOrEmpty(strContainer))
                        {
                            strContainer = objcontainer.Oid.ToString();
                        }
                        else if (!string.IsNullOrEmpty(strContainer))
                        {
                            strContainer = strContainer + "; " + objcontainer.Oid.ToString();
                        }
                    }
                }
                if (!string.IsNullOrEmpty(strContainer))
                {
                    ContainerSettings objconset = View.ObjectSpace.FindObject<ContainerSettings>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                    if (objconset != null)
                    {
                        objconset.Container = strContainer;
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Select atleast one containers.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
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
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                if (View != null && View.ObjectTypeInfo.Type == typeof(TestMethod))
                {
                    ObjectSpace.Committing -= new EventHandler<System.ComponentModel.CancelEventArgs>(ObjectSpace_Committing);
                }
                ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                if (View.Id == "Component_ListView_Test")
                {
                    Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                }
                if (View.Id == "ContainerSettings_ListView" || View.Id == "ContainerSettings_DetailView" || View.Id == "Component_ListView_Test")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    if (View.Id == "ContainerSettings_ListView" || View.Id == "ContainerSettings_DetailView")
                    {
                        Frame.GetController<NewObjectViewController>().NewObjectAction.Execute -= NewObjectAction_Execute;
                    }
                }
                else if (View.Id == "Container_ListView_ContainerSettings" || View.Id == "Preservative_ListView_ContainerSetting")
                {
                    Isparameter = false;
                }
                else if (View.Id == "ContainerSettings_ListView_testmethod")
                {
                    if (consetinfo.lsthtvalues != null)
                    {
                        consetinfo.lsthtvalues.Clear();
                    }
                }
                else if (View.Id == "SampleSites_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executing -= SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing -= SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executing -= SaveAction_Executing;
                }
                else if (View.Id == "TestMethod_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Execute -= SaveAction_Execute;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        #endregion

        #region Designer
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Save_btn = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ImportFilesNew = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ExportTest = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Save_btn
            // 
            this.Save_btn.Caption = "Save";
            this.Save_btn.Category = "RecordEdit";
            this.Save_btn.ConfirmationMessage = null;
            this.Save_btn.Id = "Save_btn";
            this.Save_btn.ImageName = "Save_16x16";
            this.Save_btn.ToolTip = null;
            this.Save_btn.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Save_btn_Execute);
            // 
            // ImportFilesNew
            // 
            this.ImportFilesNew.AcceptButtonCaption = null;
            this.ImportFilesNew.CancelButtonCaption = null;
            this.ImportFilesNew.Caption = "Import File ";
            this.ImportFilesNew.ConfirmationMessage = null;
            this.ImportFilesNew.Id = "ImportfilesNew";
            this.ImportFilesNew.ToolTip = null;
            this.ImportFilesNew.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ImportFilesNew_CustomizePopupWindowParams);
            this.ImportFilesNew.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ImportFilesNew_Execute);
            // 
            // ExportTest
            // 
            this.ExportTest.Caption = "Template Download";
            this.ExportTest.ConfirmationMessage = null;
            this.ExportTest.Id = "ExportTest";
            this.ExportTest.ToolTip = null;
            this.ExportTest.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportTest_Execute);
            // 
            // TestMethodViewController
            // 
            this.Actions.Add(this.Save_btn);
            this.Actions.Add(this.ImportFilesNew);
            this.Actions.Add(this.ExportTest);
            this.ViewControlsCreated += new System.EventHandler(this.TestMethodViewController_ViewControlsCreated);

        }
        #endregion

        #region Events
        private void TestMethodViewController_ViewControlsCreated(object sender, EventArgs e)
        {
            try
            {
                //if (View is ListView && View.Id == "Project_LookupListView_Copy_SampleCheckIn")// (View.ObjectTypeInfo.Type == typeof(Project)))
                //{
                //    //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[customername.CustomerName]='" + objInfo.ClientName + "'");
                //}
                //if ((View is ListView) && (View.Id == "Customer_LookupListView_Copy_SampleCheckin"))// (View.ObjectTypeInfo.Type == typeof(Project)))
                //{
                //    ////((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[CustomerName]='" + objInfo.ClientName + "'");
                //    //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Projects][[ProjectName] = ?]", objInfo.ProjectName);
                //}
                if ((View is ListView) && (View.Id == "Contact_LookupListView_Copy_SampleCheckin"))// (View.ObjectTypeInfo.Type == typeof(Contact)))
                {
                    if (objInfo.ClientName != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Customer.CustomerName]='" + objInfo.ClientName + "'");
                    }
                }
                else if ((View is ListView) && (View.Id == "Method_LookupListView"))
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("([RetireDate] IS NULL OR [RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')");
                }
                else if ((View is ListView) && (View.Id == "Parameter_LookupListView"))
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("([RetireDate] IS NULL OR [RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')");
                }
                else if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Testparameter) && View.Id == "TestMethod_LookupListView_Copy_GroupTest")
                {
                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[GCRecord] IS NULL AND ([RetireDate] IS NULL OR [RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "') " +
                            "AND ([MethodName.RetireDate] IS NULL OR[MethodName.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "') ");
                }
                else if (View != null && View.Id == "TestMethod_LookupListView_Copy_GroupTest")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("([RetireDate] IS NULL OR [RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "') AND ([MethodName.RetireDate] IS NULL OR [MethodName.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')");
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
                if (View.Id == "ContainerSettings_ListView_testmethod")
                {
                    ContainerSettings objconset = (ContainerSettings)e.Object;
                    if (objconset != null && objconset.Container != null)
                    {
                        List<string> lstcontainer = new List<string>();
                        string[] strconarr = objconset.Container.Split(';');
                        if (strconarr != null && strconarr.Length == 1)
                        {
                            Container objcontain = View.ObjectSpace.FindObject<Container>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strconarr[0].ToString())));
                            if (objcontain != null)
                            {
                                objconset.DefaultContainer = objcontain;
                            }
                        }
                    }
                }
                if (e.NewValue != null)
                {
                    if ((View.CurrentObject == e.Object) && (e.PropertyName == "MatrixName") && (e.NewValue != e.OldValue) && (View.ObjectTypeInfo.Type == typeof(TestMethod)))
                    {
                        if (View.ObjectTypeInfo.Type == typeof(TestMethod))
                        {
                            TestMethod tm = (TestMethod)e.Object;
                            objInfo.MatrixName = tm.MatrixName.Oid;
                        }
                    }
                    if ((View.CurrentObject == e.Object) && (e.PropertyName == "TestName") && (e.NewValue != e.OldValue) && (View.ObjectTypeInfo.Type == typeof(TestMethod)))
                    {
                        if (View.ObjectTypeInfo.Type == typeof(TestMethod))
                        {
                            TestMethod tm = (TestMethod)e.Object;
                            //////objInfo.TestName = tm.TestName.Oid;
                        }
                    }
                }
                if (e.PropertyName == "Sort" && e.Object.GetType() == typeof(QCType))
                {
                    QCType objqctype = (QCType)e.Object;
                    if (objqctype != null)
                    {
                        if (objqctype.Sort < 0)
                        {
                            objqctype.Sort = 0;
                            Application.ShowViewStrategy.ShowMessage("Sort value should not be less than zero.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
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

        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if ((View is DetailView) && (View.ObjectTypeInfo.Type == typeof(TestMethod)))
                {
                    using (IObjectSpace os = Application.CreateObjectSpace())
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("MatrixName=='" + objInfo.MatrixName + "' AND TestName=='" + objInfo.TestName + "' ");
                        TestMethod tm = os.FindObject<TestMethod>(criteria);
                        if (tm != null)
                        {
                            e.Cancel = true;
                            try
                            {
                                throw new UserFriendlyException("Matrix and Test are already saved . Please note your changes have not been saved.");
                            }
                            catch (UserFriendlyException ex)
                            {
                                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }

                        }
                    }

                }
                if (View != null && View.ObjectTypeInfo.Type == typeof(TestMethod))
                {
                    TestMethod tm = (TestMethod)View.CurrentObject;
                    if (tm != null)
                    {
                        Guid id = tm.Oid;
                        if (tm.TestCode != string.Empty)
                        {
                            if (!tm.IsDeleted)
                            {
                                var obj = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[Oid]='" + tm.Oid + "'"));
                                if (obj.Count == 0)
                                {
                                    CriteriaOperator criteria = null;
                                    criteria = CriteriaOperator.Parse("TestCode='" + tm.TestCode + "'");
                                    bool exists = Convert.ToBoolean(ObjectSpace.Evaluate(typeof(TestMethod), (new AggregateOperand("", Aggregate.Exists)), (criteria)));
                                    if (exists)
                                    {
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "testcodexists"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                        Session currentsession = ((XPObjectSpace)(ObjectSpace)).Session;
                                        SelectedData sproc = currentsession.ExecuteSproc("GetTestuqID", "");
                                        if (sproc.ResultSet[0].Rows[0] != null)
                                            tm.TestCode = sproc.ResultSet[0].Rows[0].Values[0].ToString();
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
        #endregion

        private void Save_btn_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SampleSites_ListView")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.UpdateEdit();
                    }
                }
                else if (View.Id == "ContainerSettings_ListView_testmethod")
                {
                    if (View.Id == "ContainerSettings_ListView_testmethod" && View.SelectedObjects.Count > 0)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        string strcpcode = string.Empty;
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        foreach (ContainerSettings objcontainer in View.SelectedObjects)
                        {
                            if (!string.IsNullOrEmpty(objcontainer.Container))
                            {
                                CriteriaOperator cpcode = CriteriaOperator.Parse("Max(CPCode)");
                                //CriteriaOperator estfilter = CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test] = ? And[Method.MethodName.MethodNumber] = ? ", objcontainer.Matrix.MatrixName, objcontainer.Test, objcontainer.Method.MethodName.MethodNumber);
                                //string Newcpcode = ((XPObjectSpace)ObjectSpace).Session.Evaluate<ContainerSettings>(cpcode, estfilter).ToString();
                                var Newcpcode = ((XPObjectSpace)ObjectSpace).Session.Evaluate<ContainerSettings>(cpcode, null);
                                if (Newcpcode != null)
                                {
                                    string strnewcpcode = Newcpcode.ToString();
                                    strcpcode = strnewcpcode.Replace("CS", "");
                                    if (!string.IsNullOrEmpty(strcpcode))
                                    {
                                        int intcpcode = Convert.ToInt32(strcpcode);
                                        intcpcode = intcpcode + 1;
                                        strcpcode = intcpcode.ToString();
                                        if (strcpcode.Length == 1)
                                        {
                                            strcpcode = "00" + strcpcode;
                                        }
                                        else if (strcpcode.Length == 2)
                                        {
                                            strcpcode = "0" + strcpcode;
                                        }
                                        strcpcode = "CS" + strcpcode;
                                    }
                                }
                                else
                                {
                                    strcpcode = "CS001";
                                }
                                ContainerSettings crtconsettings = os.CreateObject<ContainerSettings>();
                                crtconsettings.CPCode = strcpcode;
                                crtconsettings.Matrix = os.GetObject(objcontainer.Matrix);
                                crtconsettings.Test = os.GetObject(objcontainer.Test);
                                crtconsettings.Method = os.GetObject(objcontainer.Method);
                                crtconsettings.Container = objcontainer.Container;
                                crtconsettings.DefaultContainer = os.GetObject(objcontainer.DefaultContainer);
                                crtconsettings.Preservative = objcontainer.Preservative;
                                if (consetinfo.lsthtvalues != null && consetinfo.lsthtvalues.Count > 0)
                                {
                                    foreach (string strhtvalue in consetinfo.lsthtvalues.ToList())
                                    {
                                        string[] strparam = strhtvalue.Split('|');
                                        ContainerSettings objconset = View.ObjectSpace.FindObject<ContainerSettings>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strparam[1].Trim().ToString())));
                                        if (objconset != null && objconset.Test.Oid == crtconsettings.Test.Oid)
                                        {
                                            string strprep = strparam[2];
                                            string stranaly = strparam[3];
                                            string htprep = string.Empty;
                                            string htanaly = string.Empty;
                                            if (!string.IsNullOrEmpty(strprep) && strprep != "null")
                                            {
                                                HoldingTimes objht = ObjectSpace.FindObject<HoldingTimes>(CriteriaOperator.Parse("[Oid] =?", new Guid(strprep.Trim().ToString())));
                                                if (objht != null)
                                                {
                                                    crtconsettings.HTBeforePrep = os.GetObject(objht);
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(stranaly) && stranaly != "null")
                                            {
                                                HoldingTimes objht = ObjectSpace.FindObject<HoldingTimes>(CriteriaOperator.Parse("[Oid] =?", new Guid(stranaly.Trim().ToString())));
                                                if (objht != null)
                                                {
                                                    crtconsettings.HTBeforeAnalysis = os.GetObject(objht);
                                                }
                                            }
                                        }
                                    }
                                }
                                crtconsettings.SetPreTimeAsAnalysisTime = objcontainer.SetPreTimeAsAnalysisTime;
                                os.CommitChanges();
                            }
                            else if (string.IsNullOrEmpty(objcontainer.Container))
                            {
                                Application.ShowViewStrategy.ShowMessage("Select atleast one containers.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                                break;
                            }
                        }
                        View.Close();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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




        private void ImportFilesNew_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                ResourceManager rmEnglish = new ResourceManager("Resources.LocalizeResourcesEnglish", Assembly.Load("App_GlobalResources"));
                ResourceManager rmChinese = new ResourceManager("Resources.LocalizeResourcesChinese", Assembly.Load("App_GlobalResources"));
                TestFileUpload testFile = (TestFileUpload)e.PopupWindowViewCurrentObject;
                if (testFile.InputFile != null && testFile.InputFile.FileName != null)
                {
                    byte[] file = testFile.InputFile.Content;
                    string fileExtension = Path.GetExtension(testFile.InputFile.FileName);
                    DevExpress.Spreadsheet.Workbook workbook = new DevExpress.Spreadsheet.Workbook();
                    if (fileExtension == ".xlsx")
                    {
                        workbook.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                    }
                    else if (fileExtension == ".xls")
                    {
                        workbook.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xls);
                    }
                    DevExpress.Spreadsheet.WorksheetCollection worksheets = workbook.Worksheets;
                    int index = 0;
                    for (int worksheetIndex = 0; worksheetIndex < workbook.Worksheets.Count; worksheetIndex++)
                    {
                        DevExpress.Spreadsheet.Worksheet worksheet = workbook.Worksheets[worksheetIndex];
                        CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                        DataTable dt = worksheet.CreateDataTable(range, true);
                        for (int col = 0; col < range.ColumnCount; col++)
                        {
                            CellValueType cellType = range[0, col].Value.Type;
                            for (int r = 1; r < range.RowCount; r++)
                            {
                                if (cellType != range[r, col].Value.Type)
                                {
                                    dt.Columns[col].DataType = typeof(string);
                                    break;
                                }
                            }
                        }
                        DevExpress.Spreadsheet.Export.DataTableExporter tblExport = worksheet.CreateDataTableExporter(range, dt, false);
                        tblExport.Export();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataRow row1 = dt.Rows[0];
                                if (row1[0].ToString() == dt.Columns[0].Caption)
                                {
                                    row1.Delete();
                                    dt.AcceptChanges();
                                }
                                foreach (DataColumn c in dt.Columns)
                                    c.ColumnName = c.ColumnName.ToString().Trim();
                            }
                            if (itemsltno.Items == null)
                            {
                                itemsltno.Items = new List<string>();
                            }


                            if (worksheet.Name == "Matrix")
                            {
                                List<Matrix> matrices = new List<Matrix>();

                                foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                                {
                                    var isEmpty = row.ItemArray.All(c => c is DBNull);
                                    if (isEmpty != null)
                                    {
                                        List<string> errorlist = new List<string>();
                                        DateTime dateTime;


                                        if (dt.Columns.Contains(rmChinese.GetString("MatrixName")) && !row.IsNull(rmChinese.GetString("MatrixName")))
                                        {
                                            strMatrix = row[rmChinese.GetString("MatrixName")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("MatrixName")) && !row.IsNull(rmEnglish.GetString("MatrixName")))
                                        {
                                            strMatrix = row[rmEnglish.GetString("MatrixName")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("MatrixName");
                                        }
                                        if (strMatrix.Contains("'"))
                                        {
                                            if (strMatrix.EndsWith("'"))
                                            {
                                                strMatrix = strMatrix.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strMatrix.StartsWith("'"))
                                            {
                                                strMatrix = strMatrix.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strMatrix = strMatrix.Replace("'", "'+''''+'");
                                            }
                                        }


                                        if (errorlist.Count == 0)
                                        {
                                            Matrix objmatrix = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]=?", strMatrix));
                                            if (objmatrix == null)
                                            {
                                                if (matrices.FirstOrDefault(i => i.MatrixName == strMatrix) == null)
                                                {
                                                    IObjectSpace tempos = Application.CreateObjectSpace();
                                                    string strComment = string.Empty;
                                                    Matrix matrix = ObjectSpace.CreateObject<Matrix>();
                                                    matrix.MatrixName = strMatrix;


                                                    if (dt.Columns.Contains(rmChinese.GetString("Comment")) && !row.IsNull(rmChinese.GetString("Comment")))
                                                    {
                                                        strComment = row[rmChinese.GetString("Comment")].ToString().Trim();
                                                    }
                                                    else if (dt.Columns.Contains(rmEnglish.GetString("Comment")) && !row.IsNull(rmEnglish.GetString("Comment")))
                                                    {
                                                        strComment = row[rmEnglish.GetString("Comment")].ToString().Trim();
                                                    }
                                                    else
                                                    {
                                                        strComment = string.Empty;
                                                    }

                                                    //Imported

                                                    ObjectSpace.CommitChanges();
                                                    ObjectSpace.Refresh();
                                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Imported"), InformationType.Success, 4000, InformationPosition.Top);

                                                }

                                            }
                                        }


                                    }
                                }
                            }

                            else if (worksheet.Name == "Sample Matrix")
                            {
                                List<VisualMatrix> vismatrices = new List<VisualMatrix>();


                                foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                                {
                                    var isEmpty = row.ItemArray.All(c => c is DBNull);
                                    if (isEmpty != null)
                                    {
                                        List<string> errorlist = new List<string>();
                                        DateTime dateTime;


                                        if (dt.Columns.Contains(rmChinese.GetString("VisualMatrixName")) && !row.IsNull(rmChinese.GetString("VisualMatrixName")))
                                        {
                                            strVisualMatrix = row[rmChinese.GetString("VisualMatrixName")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("VisualMatrixName")) && !row.IsNull(rmEnglish.GetString("VisualMatrixName")))
                                        {
                                            strVisualMatrix = row[rmEnglish.GetString("VisualMatrixName")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("VisualMatrixName");
                                        }
                                        if (strVisualMatrix.Contains("'"))
                                        {
                                            if (strVisualMatrix.EndsWith("'"))
                                            {
                                                strVisualMatrix = strVisualMatrix.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strVisualMatrix.StartsWith("'"))
                                            {
                                                strVisualMatrix = strVisualMatrix.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strVisualMatrix = strVisualMatrix.Replace("'", "'+''''+'");
                                            }
                                        }


                                        if (dt.Columns.Contains(rmChinese.GetString("MatrixName")) && !row.IsNull(rmChinese.GetString("MatrixName")))
                                        {
                                            strMatrix = row[rmChinese.GetString("MatrixName")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("MatrixName")) && !row.IsNull(rmEnglish.GetString("MatrixName")))
                                        {
                                            strMatrix = row[rmEnglish.GetString("MatrixName")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("MatrixName");
                                        }
                                        if (strMatrix.Contains("'"))
                                        {
                                            if (strMatrix.EndsWith("'"))
                                            {
                                                strMatrix = strMatrix.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strMatrix.StartsWith("'"))
                                            {
                                                strMatrix = strMatrix.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strMatrix = strMatrix.Replace("'", "'+''''+'");
                                            }
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("DaysSampleKeeping")) && !row.IsNull(rmChinese.GetString("DaysSampleKeeping")))
                                        {
                                            intDaykeepSamples = (uint)System.Convert.ToInt32(row[rmChinese.GetString("DaysSampleKeeping")].ToString().Trim());
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("DaysSampleKeeping")) && !row.IsNull(rmEnglish.GetString("DaysSampleKeeping")))
                                        {
                                            intDaykeepSamples = (uint)System.Convert.ToInt32(row[rmEnglish.GetString("DaysSampleKeeping")].ToString().Trim());
                                        }
                                        else
                                        {
                                            errorlist.Add("DaysSampleKeeping");
                                        }
                                        //if (intDaykeepSamples.Contains("'"))
                                        //{
                                        //    if (intDaykeepSamples.EndsWith("'"))
                                        //    {
                                        //        intDaykeepSamples = strMatrix.Replace("'", "'+'''");
                                        //    }
                                        //    else
                                        //        if (intDaykeepSamples.StartsWith("'"))
                                        //    {
                                        //        intDaykeepSamples = intDaykeepSamples.Replace("'", "'''+'");
                                        //    }
                                        //    else
                                        //    {
                                        //        intDaykeepSamples = intDaykeepSamples.Replace("'", "'+''''+'");
                                        //    }
                                        //}


                                        if (errorlist.Count == 0)
                                        {
                                            VisualMatrix objmatrix = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[VisualMatrixName]='" + strVisualMatrix + "'And [MatrixName.MatrixName]='" + strMatrix + "'"));
                                            if (objmatrix == null)
                                            {
                                                if (vismatrices.FirstOrDefault(i => i.VisualMatrixName == strVisualMatrix) == null)
                                                {
                                                    IObjectSpace tempos = Application.CreateObjectSpace();
                                                    string strRetriedBy = string.Empty;
                                                    VisualMatrix vsmatrix2 = ObjectSpace.CreateObject<VisualMatrix>();
                                                    vsmatrix2.VisualMatrixName = strVisualMatrix;
                                                    vsmatrix2.DaysSampleKeeping = intDaykeepSamples;

                                                    if (strMatrix != null)
                                                    {
                                                        Matrix matrix = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]='" + strMatrix + "'"));
                                                        if (matrix != null)
                                                        {
                                                            vsmatrix2.MatrixName = matrix;
                                                        }
                                                        else
                                                        {
                                                            Matrix matrix1 = tempos.CreateObject<Matrix>();
                                                            matrix1.MatrixName = strMatrix;
                                                            tempos.CommitChanges();
                                                            vsmatrix2.MatrixName = ObjectSpace.GetObject<Matrix>(matrix1);

                                                        }
                                                    }

                                                    ObjectSpace.CommitChanges();
                                                    ObjectSpace.Refresh();
                                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Imported"), InformationType.Success, 4000, InformationPosition.Top);


                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", errorlist.FirstOrDefault(i=>i.E)), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                            //return;
                                            var error = "Error in columns - " + string.Join(", ", errorlist) + " of row number - " + (dt.Rows.IndexOf(row) + 1);
                                            Application.ShowViewStrategy.ShowMessage(error, InformationType.Error, 6000, InformationPosition.Top);
                                            break;
                                        }


                                    }
                                }
                            }

                            else if (worksheet.Name == "Method")
                            {
                                List<Method> listmethods = new List<Method>();


                                foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                                {
                                    var isEmpty = row.ItemArray.All(c => c is DBNull);
                                    if (isEmpty != null)
                                    {
                                        List<string> errorlist = new List<string>();
                                        DateTime dateTime;


                                        if (dt.Columns.Contains(rmChinese.GetString("MethodName")) && !row.IsNull(rmChinese.GetString("MethodName")))
                                        {
                                            strMethodName = row[rmChinese.GetString("MethodName")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("MethodName")) && !row.IsNull(rmEnglish.GetString("MethodName")))
                                        {
                                            strMethodName = row[rmEnglish.GetString("MethodName")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("MethodName");
                                        }
                                        if (strMethodName.Contains("'"))
                                        {
                                            if (strMethodName.EndsWith("'"))
                                            {
                                                strMethodName = strMethodName.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strMethodName.StartsWith("'"))
                                            {
                                                strMethodName = strMethodName.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strMethodName = strMethodName.Replace("'", "'+''''+'");
                                            }
                                        }


                                        if (dt.Columns.Contains(rmChinese.GetString("MethodNumber")) && !row.IsNull(rmChinese.GetString("MethodNumber")))
                                        {
                                            strMethodNumber = row[rmChinese.GetString("MethodNumber")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("MethodNumber")) && !row.IsNull(rmEnglish.GetString("MethodNumber")))
                                        {
                                            strMethodNumber = row[rmEnglish.GetString("MethodNumber")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("MethodNumber");
                                        }
                                        if (strMethodNumber.Contains("'"))
                                        {
                                            if (strMethodNumber.EndsWith("'"))
                                            {
                                                strMethodNumber = strMethodNumber.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strMethodNumber.StartsWith("'"))
                                            {
                                                strMethodNumber = strMethodNumber.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strMethodNumber = strMethodNumber.Replace("'", "'+''''+'");
                                            }
                                        }
                                        if (dt.Columns.Contains(rmChinese.GetString("MethodCategory")) && !row.IsNull(rmChinese.GetString("MethodCategory")))
                                        {
                                            strCategory = row[rmChinese.GetString("MethodCategory")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("MethodCategory")) && !row.IsNull(rmEnglish.GetString("MethodCategory")))
                                        {
                                            strCategory = row[rmEnglish.GetString("MethodCategory")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("MethodCategory");
                                        }
                                        if (strCategory.Contains("'"))
                                        {
                                            if (strCategory.EndsWith("'"))
                                            {
                                                strCategory = strCategory.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strCategory.StartsWith("'"))
                                            {
                                                strCategory = strCategory.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strCategory = strCategory.Replace("'", "'+''''+'");
                                            }
                                        }


                                        if (errorlist.Count == 0)
                                        {
                                            Method objmethod = ObjectSpace.FindObject<Method>(CriteriaOperator.Parse("[MethodName]='" + strMethodName + "'And [MethodNumber]='" + strMethodNumber + "'"));

                                            {
                                                if (listmethods.FirstOrDefault(i => i.MethodName == strMethodName && i.MethodName != null && i.MethodNumber == strMethodNumber) == null)
                                                {
                                                    IObjectSpace tempos = Application.CreateObjectSpace();
                                                    UnitOfWork uow = new UnitOfWork(((XPObjectSpace)tempos).Session.DataLayer);
                                                    string strStandardClause = string.Empty;
                                                    string strClauseSubject = string.Empty;
                                                    //Nullable<DateTime> strRetireDate = null;
                                                    DateTime strRetireDate = new DateTime();
                                                    DateTime strActiveDate = new DateTime();
                                                    //Nullable<DateTime> strActiveDate = null;
                                                    bool strActive = false;
                                                    string strComment = string.Empty;
                                                    Method method = ObjectSpace.CreateObject<Method>();
                                                    method.MethodNumber = strMethodNumber;
                                                    method.MethodName = strMethodName;

                                                    if (strCategory != null)
                                                    {
                                                        MethodCategory mtdcategory = ObjectSpace.FindObject<MethodCategory>(CriteriaOperator.Parse("[category]='" + strCategory + "'"));
                                                        if (mtdcategory != null)
                                                        {
                                                            method.MethodCategory = mtdcategory;
                                                        }
                                                        else
                                                        {
                                                            MethodCategory objcategory = tempos.CreateObject<MethodCategory>();
                                                            objcategory.category = strCategory;
                                                            tempos.CommitChanges();
                                                            method.MethodCategory = ObjectSpace.GetObject<MethodCategory>(objcategory);

                                                        }
                                                    }

                                                    if (dt.Columns.Contains(rmChinese.GetString("StandardClause")) && !row.IsNull(rmChinese.GetString("StandardClause")))
                                                    {
                                                        strStandardClause = row[rmChinese.GetString("StandardClause")].ToString().Trim();
                                                    }
                                                    else if (dt.Columns.Contains(rmEnglish.GetString("StandardClause")) && !row.IsNull(rmEnglish.GetString("StandardClause")))
                                                    {
                                                        strStandardClause = row[rmEnglish.GetString("StandardClause")].ToString().Trim();
                                                    }
                                                    else
                                                    {
                                                        strStandardClause = string.Empty;
                                                    }

                                                    if (dt.Columns.Contains(rmChinese.GetString("ClauseSubject")) && !row.IsNull(rmChinese.GetString("ClauseSubject")))
                                                    {
                                                        strClauseSubject = row[rmChinese.GetString("ClauseSubject")].ToString().Trim();
                                                    }
                                                    else if (dt.Columns.Contains(rmEnglish.GetString("StandardClause")) && !row.IsNull(rmEnglish.GetString("ClauseSubject")))
                                                    {
                                                        strClauseSubject = row[rmEnglish.GetString("StandardClause")].ToString().Trim();
                                                    }
                                                    else
                                                    {
                                                        strClauseSubject = string.Empty;
                                                    }

                                                    if (dt.Columns.Contains(rmChinese.GetString("Comment")) && !row.IsNull(rmChinese.GetString("Comment")))
                                                    {
                                                        strComment = row[rmChinese.GetString("Comment")].ToString().Trim();
                                                    }
                                                    else if (dt.Columns.Contains(rmEnglish.GetString("Comment")) && !row.IsNull(rmEnglish.GetString("Comment")))
                                                    {
                                                        strComment = row[rmEnglish.GetString("Comment")].ToString().Trim();
                                                    }
                                                    else
                                                    {
                                                        strComment = string.Empty;
                                                    }

                                                    if (dt.Columns.Contains("RetireDate") && !row.IsNull("RetireDate"))
                                                    {
                                                        if (row["RetireDate"].GetType() == typeof(DateTime))
                                                        {
                                                            strRetireDate = Convert.ToDateTime(row["RetireDate"]);
                                                        }
                                                        else if (row["RetireDate"].GetType() == typeof(string))
                                                        {
                                                            string strFollowUpDate = row["RetireDate"].ToString();
                                                            if (strFollowUpDate.Contains("/"))
                                                            {
                                                                string[] arrFollowUpDate = strFollowUpDate.Split('/');
                                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                                {
                                                                    if (arrFollowUpDate[0].Length <= 2)
                                                                    {
                                                                        strRetireDate = Convert.ToDateTime(strFollowUpDate);
                                                                    }
                                                                    else
                                                                    {
                                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                                        strRetireDate = date;
                                                                    }
                                                                }
                                                            }
                                                            else if (strFollowUpDate.Contains("-"))
                                                            {
                                                                string[] arrFollowUpDate = strFollowUpDate.Split('-');
                                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                                {
                                                                    if (arrFollowUpDate[0].Length <= 2)
                                                                    {
                                                                        strRetireDate = Convert.ToDateTime(strFollowUpDate);
                                                                    }
                                                                    else
                                                                    {
                                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                                        strRetireDate = date;
                                                                    }
                                                                }
                                                            }
                                                            else if (strFollowUpDate.Contains("."))
                                                            {
                                                                string[] arrFollowUpDate = strFollowUpDate.Split('.');
                                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                                {
                                                                    if (arrFollowUpDate[0].Length <= 2)
                                                                    {
                                                                        strRetireDate = Convert.ToDateTime(strFollowUpDate);
                                                                    }
                                                                    else
                                                                    {
                                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                                        strRetireDate = date;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                    }

                                                    if (dt.Columns.Contains("ActiveDate") && !row.IsNull("ActiveDate"))
                                                    {
                                                        if (row["ActiveDate"].GetType() == typeof(DateTime))
                                                        {
                                                            strActiveDate = Convert.ToDateTime(row["ActiveDate"]);
                                                        }
                                                        else if (row["ActiveDate"].GetType() == typeof(string))
                                                        {
                                                            string strFollowUpDate = row["ActiveDate"].ToString();
                                                            if (strFollowUpDate.Contains("/"))
                                                            {
                                                                string[] arrFollowUpDate = strFollowUpDate.Split('/');
                                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                                {
                                                                    if (arrFollowUpDate[0].Length <= 2)
                                                                    {
                                                                        strActiveDate = Convert.ToDateTime(strFollowUpDate);
                                                                    }
                                                                    else
                                                                    {
                                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                                        strActiveDate = date;
                                                                    }
                                                                }
                                                            }
                                                            else if (strFollowUpDate.Contains("-"))
                                                            {
                                                                string[] arrFollowUpDate = strFollowUpDate.Split('-');
                                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                                {
                                                                    if (arrFollowUpDate[0].Length <= 2)
                                                                    {
                                                                        strActiveDate = Convert.ToDateTime(strFollowUpDate);
                                                                    }
                                                                    else
                                                                    {
                                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                                        strActiveDate = date;
                                                                    }
                                                                }
                                                            }
                                                            else if (strFollowUpDate.Contains("."))
                                                            {
                                                                string[] arrFollowUpDate = strFollowUpDate.Split('.');
                                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                                {
                                                                    if (arrFollowUpDate[0].Length <= 2)
                                                                    {
                                                                        strActiveDate = Convert.ToDateTime(strFollowUpDate);
                                                                    }
                                                                    else
                                                                    {
                                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                                        strActiveDate = date;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                    }

                                                    method.RetireDate = strRetireDate;
                                                    method.ActiveDate = strActiveDate;
                                                    ObjectSpace.CommitChanges();
                                                    ObjectSpace.Refresh();
                                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Imported"), InformationType.Success, 4000, InformationPosition.Top);

                                                }
                                            }
                                        }



                                    }
                                }
                            }

                            else if (worksheet.Name == "Parameter")
                            {
                                List<Modules.BusinessObjects.Setting.Parameter> listmethods = new List<Modules.BusinessObjects.Setting.Parameter>();
                                string strMeltingPoint = string.Empty;
                                string strFlashPoint = string.Empty;
                                string strBoilingPoint = string.Empty;
                                string strEquivalent = string.Empty;
                                string strFormula = string.Empty;
                                string strSynonym = string.Empty;
                                DateTime strRetireDate = new DateTime();

                                foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                                {
                                    var isEmpty = row.ItemArray.All(c => c is DBNull);
                                    if (isEmpty != null)
                                    {
                                        List<string> errorlist = new List<string>();
                                        DateTime dateTime;


                                        if (dt.Columns.Contains(rmChinese.GetString("ParameterName")) && !row.IsNull(rmChinese.GetString("ParameterName")))
                                        {
                                            strParameter = row[rmChinese.GetString("ParameterName")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("ParameterName")) && !row.IsNull(rmEnglish.GetString("ParameterName")))
                                        {
                                            strParameter = row[rmEnglish.GetString("ParameterName")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("ParameterName");
                                        }
                                        if (strParameter.Contains("'"))
                                        {
                                            if (strParameter.EndsWith("'"))
                                            {
                                                strParameter = strParameter.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strParameter.StartsWith("'"))
                                            {
                                                strParameter = strParameter.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strParameter = strParameter.Replace("'", "'+''''+'");
                                            }
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("Formula")) && !row.IsNull(rmChinese.GetString("Formula")))
                                        {
                                            strFormula = row[rmChinese.GetString("Formula")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Formula")) && !row.IsNull(rmEnglish.GetString("Formula")))
                                        {
                                            strFormula = row[rmEnglish.GetString("Formula")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strFormula = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("Synonym")) && !row.IsNull(rmChinese.GetString("Synonym")))
                                        {
                                            strSynonym = row[rmChinese.GetString("Synonym")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Synonym")) && !row.IsNull(rmEnglish.GetString("Synonym")))
                                        {
                                            strSynonym = row[rmEnglish.GetString("Synonym")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strSynonym = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("MeltingPoint")) && !row.IsNull(rmChinese.GetString("MeltingPoint")))
                                        {
                                            strMeltingPoint = row[rmChinese.GetString("MeltingPoint")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("MeltingPoint")) && !row.IsNull(rmEnglish.GetString("MeltingPoint")))
                                        {
                                            strMeltingPoint = row[rmEnglish.GetString("MeltingPoint")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strMeltingPoint = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("BoilingPoint")) && !row.IsNull(rmChinese.GetString("BoilingPoint")))
                                        {
                                            strBoilingPoint = row[rmChinese.GetString("BoilingPoint")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("BoilingPoint")) && !row.IsNull(rmEnglish.GetString("BoilingPoint")))
                                        {
                                            strBoilingPoint = row[rmEnglish.GetString("BoilingPoint")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strBoilingPoint = string.Empty;
                                        }
                                        if (dt.Columns.Contains(rmChinese.GetString("FlashPoint")) && !row.IsNull(rmChinese.GetString("FlashPoint")))
                                        {
                                            strFlashPoint = row[rmChinese.GetString("FlashPoint")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("FlashPoint")) && !row.IsNull(rmEnglish.GetString("FlashPoint")))
                                        {
                                            strFlashPoint = row[rmEnglish.GetString("FlashPoint")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strFlashPoint = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("Equivalent")) && !row.IsNull(rmChinese.GetString("Equivalent")))
                                        {
                                            strEquivalent = row[rmChinese.GetString("Equivalent")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Equivalent")) && !row.IsNull(rmEnglish.GetString("Equivalent")))
                                        {
                                            strEquivalent = row[rmEnglish.GetString("Equivalent")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strEquivalent = string.Empty;
                                        }





                                        if (errorlist.Count == 0)
                                        {
                                            Modules.BusinessObjects.Setting.Parameter objparameter = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.Parameter>(CriteriaOperator.Parse("[ParameterName]='" + strParameter + "'"));
                                            if (objparameter == null)
                                            {
                                                if (listmethods.FirstOrDefault(i => i.ParameterName == strParameter && i.ParameterName != null) == null)
                                                {
                                                    IObjectSpace tempos = Application.CreateObjectSpace();

                                                    Modules.BusinessObjects.Setting.Parameter parameter = ObjectSpace.CreateObject<Modules.BusinessObjects.Setting.Parameter>();
                                                    parameter.ParameterName = strParameter;
                                                    parameter.Equivalent = strEquivalent;
                                                    parameter.FlashPoint = strFlashPoint;
                                                    parameter.BoilingPoint = strBoilingPoint;
                                                    parameter.MeltingPoint = strMeltingPoint;
                                                    parameter.Formula = strFormula;
                                                    parameter.Synonym = strSynonym;
                                                    parameter.RetireDate = strRetireDate;


                                                    //if (strCategory != null)
                                                    //{
                                                    //    MethodCategory mtdcategory = ObjectSpace.FindObject<MethodCategory>(CriteriaOperator.Parse("[category]='" + strCategory + "'"));
                                                    //    if (mtdcategory != null)
                                                    //    {
                                                    //        method.MethodCategory = mtdcategory;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        MethodCategory objcategory = tempos.CreateObject<MethodCategory>();
                                                    //        objcategory.category = strCategory;
                                                    //        tempos.CommitChanges();
                                                    //        method.MethodCategory = ObjectSpace.GetObject<MethodCategory>(objcategory);

                                                    //    }
                                                    //}


                                                    ObjectSpace.CommitChanges();
                                                    ObjectSpace.Refresh();
                                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Imported"), InformationType.Success, 4000, InformationPosition.Top);



                                                }
                                            }

                                        }
                                        else
                                        {
                                            //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", errorlist.FirstOrDefault(i => i.E)), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                            //return;
                                            var error = "Error in columns - " + string.Join(", ", errorlist) + " of row number - " + (dt.Rows.IndexOf(row) + 1);
                                            Application.ShowViewStrategy.ShowMessage(error, InformationType.Error, 6000, InformationPosition.Top);
                                            break;
                                        }




                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "uploadfile"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }
                                }
                            }

                            else if (worksheet.Name == "QCType")
                            {
                                List<QCType> listmethods = new List<QCType>();


                                foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                                {
                                    var isEmpty = row.ItemArray.All(c => c is DBNull);
                                    if (isEmpty != null)
                                    {
                                        List<string> errorlist = new List<string>();
                                        DateTime dateTime;


                                        if (dt.Columns.Contains(rmChinese.GetString("QCTypeName")) && !row.IsNull(rmChinese.GetString("QCTypeName")))
                                        {
                                            strQcType = row[rmChinese.GetString("QCTypeName")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("QCTypeName")) && !row.IsNull(rmEnglish.GetString("QCTypeName")))
                                        {
                                            strQcType = row[rmEnglish.GetString("QCTypeName")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("QCTypeName");
                                        }
                                        if (strParameter.Contains("'"))
                                        {
                                            if (strParameter.EndsWith("'"))
                                            {
                                                strParameter = strParameter.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strParameter.StartsWith("'"))
                                            {
                                                strParameter = strParameter.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strParameter = strParameter.Replace("'", "'+''''+'");
                                            }
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("QCSource")) && !row.IsNull(rmChinese.GetString("QCSource")))
                                        {
                                            strQCSource = row[rmChinese.GetString("QCSource")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("QCSource")) && !row.IsNull(rmEnglish.GetString("QCSource")))
                                        {
                                            strQCSource = row[rmEnglish.GetString("QCSource")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("QCSource");
                                        }
                                        if (strQCSource.Contains("'"))
                                        {
                                            if (strQCSource.EndsWith("'"))
                                            {
                                                strQCSource = strQCSource.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strQCSource.StartsWith("'"))
                                            {
                                                strQCSource = strQCSource.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strQCSource = strQCSource.Replace("'", "'+''''+'");
                                            }
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("QcRole")) && !row.IsNull(rmChinese.GetString("QcRole")))
                                        {
                                            strQCRole = row[rmChinese.GetString("QcRole")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("QcRole")) && !row.IsNull(rmEnglish.GetString("QcRole")))
                                        {
                                            strQCRole = row[rmEnglish.GetString("QcRole")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("QcRole");
                                        }
                                        if (strQCRole.Contains("'"))
                                        {
                                            if (strQCRole.EndsWith("'"))
                                            {
                                                strQCRole = strQCRole.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strQCRole.StartsWith("'"))
                                            {
                                                strQCRole = strQCRole.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strQCRole = strQCRole.Replace("'", "'+''''+'");
                                            }
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("QCRootRole")) && !row.IsNull(rmChinese.GetString("QCRootRole")))
                                        {
                                            strQCRootRole = row[rmChinese.GetString("QCRootRole")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("QCRootRole")) && !row.IsNull(rmEnglish.GetString("QCRootRole")))
                                        {
                                            strQCRootRole = row[rmEnglish.GetString("QCRootRole")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("QCRootRole");
                                        }
                                        if (strQCRootRole.Contains("'"))
                                        {
                                            if (strQCRootRole.EndsWith("'"))
                                            {
                                                strQCRootRole = strQCRootRole.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strQCRootRole.StartsWith("'"))
                                            {
                                                strQCRootRole = strQCRootRole.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strQCRootRole = strQCRootRole.Replace("'", "'+''''+'");
                                            }
                                        }





                                        if (errorlist.Count == 0)
                                        {
                                            QCType objQctype = ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName]='" + strQcType + "'And [QCRootRole.QCRoot_Role]='" + strQCRootRole + "'And[QcRole.QC_Role]='" + strQCRole + "'And[QCSource.QC_Source]='" + strQCSource + "'"));
                                            //TestMethod test = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName]='" + strTestname + "'And [MatrixName.MatrixName]='" + strMatrix + "' And [MethodName.MethodName]='" + strMethodName + "'"));

                                            if (objQctype == null)
                                            {
                                                if (listmethods.FirstOrDefault(i => i.QCTypeName == strParameter && i.QCTypeName != null) == null)
                                                {
                                                    IObjectSpace tempos = Application.CreateObjectSpace();
                                                    //string strMeltingPoint = string.Empty;
                                                    //string strFlashPoint = string.Empty;
                                                    //string strBoilingPoint = string.Empty;
                                                    //string strEquivalent = string.Empty;
                                                    //string strFormula = string.Empty;
                                                    //string strSynonym = string.Empty;
                                                    QCType qCType = ObjectSpace.CreateObject<QCType>();
                                                    qCType.QCTypeName = strQcType;

                                                    if (strQCRole != null)
                                                    {
                                                        QcRole qcRole = ObjectSpace.FindObject<QcRole>(CriteriaOperator.Parse("[QC_Role]='" + strQCRole + "'"));
                                                        if (qcRole != null)
                                                        {
                                                            qCType.QcRole = qcRole;
                                                        }
                                                        else
                                                        {
                                                            QcRole objcategory = tempos.CreateObject<QcRole>();
                                                            objcategory.QC_Role = strQCRole;
                                                            tempos.CommitChanges();
                                                            qCType.QcRole = ObjectSpace.GetObject<QcRole>(objcategory);

                                                        }
                                                    }
                                                    if (strQCRootRole != null)
                                                    {
                                                        QCRootRole qcRootRole = ObjectSpace.FindObject<QCRootRole>(CriteriaOperator.Parse("[QCRoot_Role]='" + strQCRootRole + "'"));
                                                        if (qcRootRole != null)
                                                        {
                                                            qCType.QCRootRole = qcRootRole;
                                                        }
                                                        else
                                                        {
                                                            QCRootRole objqCRootRole = tempos.CreateObject<QCRootRole>();
                                                            objqCRootRole.QCRoot_Role = strQCRootRole;
                                                            tempos.CommitChanges();
                                                            qCType.QCRootRole = ObjectSpace.GetObject<QCRootRole>(objqCRootRole);

                                                        }
                                                    }
                                                    if (strQCSource != null)
                                                    {
                                                        QCSource qCSource = ObjectSpace.FindObject<QCSource>(CriteriaOperator.Parse("[QC_Source]='" + strQCSource + "'"));
                                                        if (qCSource != null)
                                                        {
                                                            qCType.QCSource = qCSource;
                                                        }
                                                        else
                                                        {
                                                            QCSource objQCSource = tempos.CreateObject<QCSource>();
                                                            objQCSource.QC_Source = strQCSource;
                                                            tempos.CommitChanges();
                                                            qCType.QCSource = ObjectSpace.GetObject<QCSource>(objQCSource);

                                                        }
                                                    }




                                                    ObjectSpace.CommitChanges();
                                                    ObjectSpace.Refresh();
                                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Imported"), InformationType.Success, 4000, InformationPosition.Top);


                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            else if (worksheet.Name == "Test Parameter")
                            {

                                List<TestMethod> testMethods = new List<TestMethod>();
                                string strComponent = string.Empty;
                                bool strIsGroupTest = false;// Incomplete
                                bool strSurroagate = false;// Incomplete
                                bool strInternalStandard = false;// Incomplete
                                bool strGrouptm = false;// Incomplete
                                int strSort = 0;
                                string strDefaultResult = string.Empty;
                                string strDefaultUnit = string.Empty;
                                string strFinalDefaultResult = string.Empty;
                                string strFinalDefaultUnit = string.Empty;
                                string strRPLimit = string.Empty;
                                string strMDL = string.Empty;
                                string strRSD = string.Empty;
                                string strRegulatoryLimit = string.Empty;
                                string strMCL = string.Empty;
                                string strLOQ = string.Empty;
                                string strUQL = string.Empty;
                                string strLowCLimit = string.Empty;
                                string strHighCLimit = string.Empty;
                                string strRELCLimit = string.Empty;
                                string strREHCLimit = string.Empty;
                                string strSigfig = string.Empty;
                                string strCutOff = string.Empty;
                                string strDecimal = string.Empty;
                                string strComment = string.Empty;
                                double strSpikeAmount = 0;
                                double strSurrogateAmount = 0;
                                string strSpikeAmountUnits = string.Empty;
                                string strSurrogateLLimit = string.Empty;
                                string strSurrogateULimit = string.Empty;
                                string strRecLCLimit = string.Empty;
                                string strRecHCLimit = string.Empty;
                                //string strMDL = string.Empty;








                                foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                                {
                                    var isEmpty = row.ItemArray.All(c => c is DBNull);
                                    if (isEmpty != null)
                                    {
                                        List<string> errorlist = new List<string>();
                                        DateTime dateTime;



                                        if (dt.Columns.Contains(rmChinese.GetString("TestName")) && !row.IsNull(rmChinese.GetString("TestName")))
                                        {
                                            strTestname = row[rmChinese.GetString("TestName")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("TestName")) && !row.IsNull(rmEnglish.GetString("TestName")))
                                        {
                                            strTestname = row[rmEnglish.GetString("TestName")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("TestName");
                                        }
                                        if (strTestname.Contains("'"))
                                        {
                                            if (strTestname.EndsWith("'"))
                                            {
                                                strTestname = strTestname.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strTestname.StartsWith("'"))
                                            {
                                                strTestname = strTestname.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strTestname = strTestname.Replace("'", "'+''''+'");
                                            }
                                        }


                                        if (dt.Columns.Contains(rmChinese.GetString("MethodName")) && !row.IsNull(rmChinese.GetString("MethodName")))
                                        {
                                            strMethodName = row[rmChinese.GetString("MethodName")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("MethodName")) && !row.IsNull(rmEnglish.GetString("MethodName")))
                                        {
                                            strMethodName = row[rmEnglish.GetString("MethodName")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("MethodName");
                                        }
                                        if (strMethodName.Contains("'"))
                                        {

                                            if (strMethodName.EndsWith("'"))
                                            {
                                                strMethodName = strMethodName.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strMethodName.StartsWith("'"))
                                            {
                                                strMethodName = strMethodName.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strMethodName = strMethodName.Replace("'", "'+''''+'");
                                            }
                                        }


                                        if (dt.Columns.Contains(rmChinese.GetString("MethodNumber")) && !row.IsNull(rmChinese.GetString("MethodNumber")))
                                        {
                                            strMethodNumber = row[rmChinese.GetString("MethodNumber")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("MethodNumber")) && !row.IsNull(rmEnglish.GetString("MethodNumber")))
                                        {
                                            strMethodNumber = row[rmEnglish.GetString("MethodNumber")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("MethodNumber");
                                        }
                                        if (strMethodNumber.Contains("'"))
                                        {

                                            if (strMethodNumber.EndsWith("'"))
                                            {
                                                strMethodNumber = strMethodNumber.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strMethodNumber.StartsWith("'"))
                                            {
                                                strMethodNumber = strMethodNumber.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strMethodNumber = strMethodNumber.Replace("'", "'+''''+'");
                                            }
                                        }



                                        if (dt.Columns.Contains(rmChinese.GetString("MatrixName")) && !row.IsNull(rmChinese.GetString("MatrixName")))
                                        {
                                            strMatrix = row[rmChinese.GetString("MatrixName")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("MatrixName")) && !row.IsNull(rmEnglish.GetString("MatrixName")))
                                        {
                                            strMatrix = row[rmEnglish.GetString("MatrixName")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("MatrixName");
                                        }
                                        if (strMethodName.Contains("'"))
                                        {
                                            if (strMethodName.EndsWith("'"))
                                            {
                                                strMethodName = strMethodName.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strMethodName.StartsWith("'"))
                                            {
                                                strMethodName = strMethodName.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strMethodName = strMethodName.Replace("'", "'+''''+'");
                                            }
                                        }


                                        if (dt.Columns.Contains(rmChinese.GetString("QCType")) && !row.IsNull(rmChinese.GetString("QCType")))
                                        {
                                            strQcType = row[rmChinese.GetString("QCType")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("QCType")) && !row.IsNull(rmEnglish.GetString("QCType")))
                                        {
                                            strQcType = row[rmEnglish.GetString("QCType")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("QCType");
                                        }
                                        if (strQcType.Contains("'"))
                                        {
                                            if (strQcType.EndsWith("'"))
                                            {
                                                strQcType = strQcType.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strQcType.StartsWith("'"))
                                            {
                                                strQcType = strQcType.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strQcType = strQcType.Replace("'", "'+''''+'");
                                            }
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("QCSource")) && !row.IsNull(rmChinese.GetString("QCSource")))
                                        {
                                            strQCSource = row[rmChinese.GetString("QCSource")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("QCSource")) && !row.IsNull(rmEnglish.GetString("QCSource")))
                                        {
                                            strQCSource = row[rmEnglish.GetString("QCSource")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strQCSource = string.Empty;
                                        }



                                        if (dt.Columns.Contains(rmChinese.GetString("QcRole")) && !row.IsNull(rmChinese.GetString("QcRole")))
                                        {
                                            strQCRole = row[rmChinese.GetString("QcRole")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("QcRole")) && !row.IsNull(rmEnglish.GetString("QcRole")))
                                        {
                                            strQCRole = row[rmEnglish.GetString("QcRole")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strQCRole = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("QCRootRole")) && !row.IsNull(rmChinese.GetString("QCRootRole")))
                                        {
                                            strQCRootRole = row[rmChinese.GetString("QCRootRole")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("QCRootRole")) && !row.IsNull(rmEnglish.GetString("QCRootRole")))
                                        {
                                            strQCRootRole = row[rmEnglish.GetString("QCRootRole")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strQCRootRole = string.Empty;
                                        }




                                        if (dt.Columns.Contains(rmChinese.GetString("Parameter")) && !row.IsNull(rmChinese.GetString("Parameter")))
                                        {
                                            strParameter = row[rmChinese.GetString("Parameter")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Parameter")) && !row.IsNull(rmEnglish.GetString("Parameter")))
                                        {
                                            strParameter = row[rmEnglish.GetString("Parameter")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("Parameter");
                                        }
                                        if (strParameter.Contains("'"))
                                        {
                                            if (strParameter.EndsWith("'"))
                                            {
                                                strParameter = strParameter.Replace("'", "'+'''");
                                            }
                                            else
                                                if (strParameter.StartsWith("'"))
                                            {
                                                strParameter = strParameter.Replace("'", "'''+'");
                                            }
                                            else
                                            {
                                                strParameter = strParameter.Replace("'", "'+''''+'");
                                            }
                                        }



                                        if (dt.Columns.Contains(rmChinese.GetString("Component")) && !row.IsNull(rmChinese.GetString("Component")))
                                        {
                                            strComponent = row[rmChinese.GetString("Component")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Component")) && !row.IsNull(rmEnglish.GetString("Component")))
                                        {
                                            strComponent = row[rmEnglish.GetString("Component")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strComponent = string.Empty;
                                        }


                                        if (dt.Columns.Contains(rmChinese.GetString("IsGroupTest")) && !row.IsNull(rmChinese.GetString("IsGroupTest")))
                                        {
                                            strIsGroupTest = Convert.ToBoolean(row[rmChinese.GetString("IsGroupTest")]);
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("IsGroupTest")) && !row.IsNull(rmEnglish.GetString("IsGroupTest")))
                                        {
                                            strIsGroupTest = Convert.ToBoolean(row[rmEnglish.GetString("IsGroupTest")]);
                                        }
                                        else
                                        {
                                            strIsGroupTest = false;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("Grouptm")) && !row.IsNull(rmChinese.GetString("Grouptm")))
                                        {
                                            strGrouptm = Convert.ToBoolean(row[rmChinese.GetString("Grouptm")]);
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Grouptm")) && !row.IsNull(rmEnglish.GetString("Grouptm")))
                                        {
                                            strGrouptm = Convert.ToBoolean(row[rmEnglish.GetString("Grouptm")]);
                                        }
                                        else
                                        {
                                            strGrouptm = false;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("Surroagate")) && !row.IsNull(rmChinese.GetString("Surroagate")))
                                        {
                                            strSurroagate = Convert.ToBoolean(row[rmChinese.GetString("Surroagate")]);
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Surroagate")) && !row.IsNull(rmEnglish.GetString("Surroagate")))
                                        {
                                            strSurroagate = Convert.ToBoolean(row[rmEnglish.GetString("Surroagate")]);
                                        }
                                        else
                                        {
                                            strSurroagate = false;
                                        }
                                        if (dt.Columns.Contains(rmChinese.GetString("InternalStandard")) && !row.IsNull(rmChinese.GetString("InternalStandard")))
                                        {
                                            strInternalStandard = Convert.ToBoolean(row[rmChinese.GetString("InternalStandard")]);
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("InternalStandard")) && !row.IsNull(rmEnglish.GetString("InternalStandard")))
                                        {
                                            strInternalStandard = Convert.ToBoolean(row[rmEnglish.GetString("InternalStandard")]);
                                        }
                                        else
                                        {
                                            strInternalStandard = false;
                                        }


                                        if (dt.Columns.Contains(rmChinese.GetString("Sort")) && !row.IsNull(rmChinese.GetString("Sort")))
                                        {
                                            strSort = Convert.ToInt32(row[rmChinese.GetString("Sort")]);
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Sort")) && !row.IsNull(rmEnglish.GetString("Sort")))
                                        {
                                            strSort = Convert.ToInt32(row[rmEnglish.GetString("Sort")]);
                                        }




                                        if (dt.Columns.Contains(rmChinese.GetString("DefaultUnit")) && !row.IsNull(rmChinese.GetString("DefaultUnit")))
                                        {
                                            strDefaultUnit = row[rmChinese.GetString("DefaultUnit")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("DefaultUnit")) && !row.IsNull(rmEnglish.GetString("DefaultUnit")))
                                        {
                                            strDefaultUnit = row[rmEnglish.GetString("DefaultUnit")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strDefaultUnit = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("FinalDefaultResult")) && !row.IsNull(rmChinese.GetString("FinalDefaultResult")))
                                        {
                                            strFinalDefaultResult = row[rmChinese.GetString("FinalDefaultResult")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("FinalDefaultResult")) && !row.IsNull(rmEnglish.GetString("FinalDefaultResult")))
                                        {
                                            strFinalDefaultResult = row[rmEnglish.GetString("FinalDefaultResult")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strFinalDefaultResult = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("FinalDefaultUnit")) && !row.IsNull(rmChinese.GetString("FinalDefaultUnit")))
                                        {
                                            strFinalDefaultUnit = row[rmChinese.GetString("FinalDefaultUnit")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("FinalDefaultUnit")) && !row.IsNull(rmEnglish.GetString("FinalDefaultUnit")))
                                        {
                                            strFinalDefaultUnit = row[rmEnglish.GetString("FinalDefaultUnit")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strFinalDefaultUnit = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("RPLimit")) && !row.IsNull(rmChinese.GetString("RPLimit")))
                                        {
                                            strRPLimit = row[rmChinese.GetString("RPLimit")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("RPLimit")) && !row.IsNull(rmEnglish.GetString("RPLimit")))
                                        {
                                            strRPLimit = row[rmEnglish.GetString("RPLimit")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strRPLimit = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("MDL")) && !row.IsNull(rmChinese.GetString("MDL")))
                                        {
                                            strMDL = row[rmChinese.GetString("MDL")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("MDL")) && !row.IsNull(rmEnglish.GetString("MDL")))
                                        {
                                            strMDL = row[rmEnglish.GetString("MDL")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strMDL = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("RSD")) && !row.IsNull(rmChinese.GetString("RSD")))
                                        {
                                            strRSD = row[rmChinese.GetString("RSD")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("RSD")) && !row.IsNull(rmEnglish.GetString("RSD")))
                                        {
                                            strRSD = row[rmEnglish.GetString("RSD")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strRSD = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("RegulatoryLimit")) && !row.IsNull(rmChinese.GetString("RegulatoryLimit")))
                                        {
                                            strRegulatoryLimit = row[rmChinese.GetString("RegulatoryLimit")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("RegulatoryLimit")) && !row.IsNull(rmEnglish.GetString("RegulatoryLimit")))
                                        {
                                            strRegulatoryLimit = row[rmEnglish.GetString("RegulatoryLimit")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strRegulatoryLimit = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("MCL")) && !row.IsNull(rmChinese.GetString("MCL")))
                                        {
                                            strMCL = row[rmChinese.GetString("MCL")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("MCL")) && !row.IsNull(rmEnglish.GetString("MCL")))
                                        {
                                            strMCL = row[rmEnglish.GetString("MCL")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strMCL = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("LOQ")) && !row.IsNull(rmChinese.GetString("LOQ")))
                                        {
                                            strLOQ = row[rmChinese.GetString("LOQ")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("LOQ")) && !row.IsNull(rmEnglish.GetString("LOQ")))
                                        {
                                            strLOQ = row[rmEnglish.GetString("LOQ")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strLOQ = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("UQL")) && !row.IsNull(rmChinese.GetString("UQL")))
                                        {
                                            strUQL = row[rmChinese.GetString("UQL")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("UQL")) && !row.IsNull(rmEnglish.GetString("UQL")))
                                        {
                                            strUQL = row[rmEnglish.GetString("UQL")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strUQL = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("LowCLimit")) && !row.IsNull(rmChinese.GetString("LowCLimit")))
                                        {
                                            strLowCLimit = row[rmChinese.GetString("LowCLimit")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("LowCLimit")) && !row.IsNull(rmEnglish.GetString("LowCLimit")))
                                        {
                                            strLowCLimit = row[rmEnglish.GetString("LowCLimit")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strLowCLimit = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("HighCLimit")) && !row.IsNull(rmChinese.GetString("HighCLimit")))
                                        {
                                            strHighCLimit = row[rmChinese.GetString("HighCLimit")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("HighCLimit")) && !row.IsNull(rmEnglish.GetString("HighCLimit")))
                                        {
                                            strHighCLimit = row[rmEnglish.GetString("HighCLimit")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strHighCLimit = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("REHCLimit")) && !row.IsNull(rmChinese.GetString("REHCLimit")))
                                        {
                                            strREHCLimit = row[rmChinese.GetString("REHCLimit")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("REHCLimit")) && !row.IsNull(rmEnglish.GetString("REHCLimit")))
                                        {
                                            strREHCLimit = row[rmEnglish.GetString("REHCLimit")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strREHCLimit = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("SigFig")) && !row.IsNull(rmChinese.GetString("SigFig")))
                                        {
                                            strSigfig = row[rmChinese.GetString("SigFig")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("SigFig")) && !row.IsNull(rmEnglish.GetString("SigFig")))
                                        {
                                            strSigfig = row[rmEnglish.GetString("SigFig")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strSigfig = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("CutOff")) && !row.IsNull(rmChinese.GetString("CutOff")))
                                        {
                                            strCutOff = row[rmChinese.GetString("CutOff")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("CutOff")) && !row.IsNull(rmEnglish.GetString("CutOff")))
                                        {
                                            strCutOff = row[rmEnglish.GetString("CutOff")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strCutOff = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("Decimal")) && !row.IsNull(rmChinese.GetString("Decimal")))
                                        {
                                            strDecimal = row[rmChinese.GetString("Decimal")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Decimal")) && !row.IsNull(rmEnglish.GetString("Decimal")))
                                        {
                                            strDecimal = row[rmEnglish.GetString("Decimal")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strDecimal = string.Empty;
                                        }
                                        if (dt.Columns.Contains(rmChinese.GetString("Comment")) && !row.IsNull(rmChinese.GetString("Comment")))
                                        {
                                            strComment = row[rmChinese.GetString("Comment")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("Comment")) && !row.IsNull(rmEnglish.GetString("Comment")))
                                        {
                                            strComment = row[rmEnglish.GetString("Comment")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strComment = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("SpikeAmount")) && !row.IsNull(rmChinese.GetString("SpikeAmount")))
                                        {
                                            //doubleunitprice = Convert.ToDouble(row[rmChinese.GetString("UnitPrice")]);

                                            string strUnitPrice = string.Empty;
                                            strUnitPrice = Convert.ToString(row[rmChinese.GetString("SpikeAmount")]);
                                            if (strUnitPrice.Contains("$"))
                                            {
                                                strUnitPrice = strUnitPrice.Replace("$", "");
                                            }

                                            strSpikeAmount = Convert.ToDouble(strUnitPrice);

                                            if (strSpikeAmount < 1)
                                            {
                                                strSpikeAmount = 1;
                                            }


                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("SpikeAmount")) && !row.IsNull(rmEnglish.GetString("SpikeAmount")))
                                        {

                                            string strUnitPrice = string.Empty;
                                            strUnitPrice = Convert.ToString(row[rmEnglish.GetString("SpikeAmount")]);
                                            if (strUnitPrice.Contains("$"))
                                            {
                                                strUnitPrice = strUnitPrice.Replace("$", "");
                                            }

                                            strSpikeAmount = Convert.ToDouble(strUnitPrice);
                                            if (strSpikeAmount < 1)
                                            {
                                                strSpikeAmount = 1;
                                            }

                                        }
                                        else
                                        {
                                            strSpikeAmount = Convert.ToDouble(1);
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("SurrogateAmount")) && !row.IsNull(rmChinese.GetString("SurrogateAmount")))
                                        {
                                            string strAmountUnits = string.Empty;
                                            strAmountUnits = Convert.ToString(row[rmChinese.GetString("SurrogateAmount")]);
                                            if (strAmountUnits.Contains("$"))
                                            {
                                                strAmountUnits = strAmountUnits.Replace("$", "");
                                            }

                                            strSurrogateAmount = Convert.ToDouble(strAmountUnits);

                                            if (strSurrogateAmount < 1)
                                            {
                                                strSurrogateAmount = 1;
                                            }
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("SurrogateAmount")) && !row.IsNull(rmEnglish.GetString("SurrogateAmount")))
                                        {


                                            string strAmountUnits = string.Empty;
                                            strAmountUnits = Convert.ToString(row[rmEnglish.GetString("SurrogateAmount")]);
                                            if (strAmountUnits.Contains("$"))
                                            {
                                                strAmountUnits = strAmountUnits.Replace("$", "");
                                            }

                                            strSurrogateAmount = Convert.ToDouble(strAmountUnits);
                                            if (strSurrogateAmount < 1)
                                            {
                                                strSurrogateAmount = 1;
                                            }
                                        }
                                        else
                                        {
                                            strSurrogateAmount = Convert.ToDouble(1);
                                        }


                                        if (dt.Columns.Contains(rmChinese.GetString("SpikeAmountUnits")) && !row.IsNull(rmChinese.GetString("SpikeAmountUnits")))
                                        {
                                            strSpikeAmountUnits = row[rmChinese.GetString("SpikeAmountUnits")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("SurrogateAmount")) && !row.IsNull(rmEnglish.GetString("SurrogateAmount")))
                                        {
                                            strSpikeAmountUnits = row[rmEnglish.GetString("SpikeAmountUnits")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strSpikeAmountUnits = string.Empty;
                                        }
                                        if (dt.Columns.Contains(rmChinese.GetString("SurrogateLLimit")) && !row.IsNull(rmChinese.GetString("SurrogateLLimit")))
                                        {
                                            strSurrogateLLimit = row[rmChinese.GetString("SurrogateLLimit")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("SurrogateLLimit")) && !row.IsNull(rmEnglish.GetString("SurrogateLLimit")))
                                        {
                                            strSurrogateLLimit = row[rmEnglish.GetString("SurrogateLLimit")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strSurrogateLLimit = string.Empty;
                                        }
                                        if (dt.Columns.Contains(rmChinese.GetString("SurrogateULimit")) && !row.IsNull(rmChinese.GetString("SurrogateULimit")))
                                        {
                                            strSurrogateULimit = row[rmChinese.GetString("SurrogateULimit")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("SurrogateULimit")) && !row.IsNull(rmEnglish.GetString("SurrogateULimit")))
                                        {
                                            strSurrogateULimit = row[rmEnglish.GetString("SurrogateULimit")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strSurrogateULimit = string.Empty;
                                        }
                                        if (dt.Columns.Contains(rmChinese.GetString("RecHCLimit")) && !row.IsNull(rmChinese.GetString("RecHCLimit")))
                                        {
                                            strRecHCLimit = row[rmChinese.GetString("RecHCLimit")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("RecHCLimit")) && !row.IsNull(rmEnglish.GetString("RecHCLimit")))
                                        {
                                            strRecHCLimit = row[rmEnglish.GetString("RecHCLimit")].ToString().Trim();
                                        }
                                        else
                                        {
                                            strRecHCLimit = string.Empty;
                                        }

                                        if (dt.Columns.Contains(rmChinese.GetString("VisualMatrixName")) && !row.IsNull(rmChinese.GetString("VisualMatrixName")))
                                        {
                                            strVisualMatrix = row[rmChinese.GetString("VisualMatrixName")].ToString().Trim();
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("VisualMatrixName")) && !row.IsNull(rmEnglish.GetString("VisualMatrixName")))
                                        {
                                            strVisualMatrix = row[rmEnglish.GetString("VisualMatrixName")].ToString().Trim();
                                        }
                                        else
                                        {
                                            errorlist.Add("VisualMatrixName");
                                        }


                                        if (dt.Columns.Contains(rmChinese.GetString("DaysSampleKeeping")) && !row.IsNull(rmChinese.GetString("DaysSampleKeeping")))
                                        {
                                            intDaykeepSamples = (uint)System.Convert.ToInt32(row[rmChinese.GetString("DaysSampleKeeping")].ToString().Trim());
                                        }
                                        else if (dt.Columns.Contains(rmEnglish.GetString("DaysSampleKeeping")) && !row.IsNull(rmEnglish.GetString("DaysSampleKeeping")))
                                        {
                                            intDaykeepSamples = (uint)System.Convert.ToInt32(row[rmEnglish.GetString("DaysSampleKeeping")].ToString().Trim());
                                        }
                                        else
                                        {
                                            errorlist.Add("DaysSampleKeeping");
                                        }


                                        if (errorlist.Count == 0)
                                        {
                                            List<TestMethod> lstAlltest = ObjectSpace.GetObjects<TestMethod>().ToList();
                                            //TestMethod testMethod1 =lstAlltest.FirstOrDefault(i=>i.TestName == strTestname && i.Test.MatrixName.MatrixName =strMatrix &&)
                                            TestMethod test = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName]='" + strTestname + "'And [MatrixName.MatrixName]='" + strMatrix + "' And [MethodName.MethodName]='" + strMethodName + "'"));
                                            if (test == null)
                                            {
                                                if (testMethods.FirstOrDefault(i => i.TestName == strTestname && i.MatrixName != null && i.MatrixName.MatrixName == strMatrix && i.MethodName != null && i.Method.MethodName.MethodName == strMethodName) == null)
                                                {
                                                    IObjectSpace tempos = Application.CreateObjectSpace();
                                                    DateTime obtRetireDate = new DateTime();

                                                    TestMethod testMethod = tempos.CreateObject<TestMethod>();
                                                    testMethod.TestName = strTestname;
                                                    testMethod.IsFieldTest = strIsfieldTest;
                                                    testMethod.IsGroup = strIsGroupTest;





                                                    if (strMatrix != null)
                                                    {
                                                        Matrix matrix = tempos.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]='" + strMatrix + "'"));
                                                        if (matrix != null)
                                                        {

                                                            testMethod.MatrixName = matrix;
                                                        }
                                                        else
                                                        {
                                                            Matrix matrix1 = tempos.CreateObject<Matrix>();
                                                            matrix1.MatrixName = strMatrix;
                                                            tempos.CommitChanges();
                                                            testMethod.MatrixName = tempos.GetObject<Matrix>(matrix1);

                                                        }
                                                    }
                                                    if (strVisualMatrix != null)
                                                    {
                                                        VisualMatrix objmatrix = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[VisualMatrixName]='" + strVisualMatrix + "'And [MatrixName.MatrixName]='" + strMatrix + "'"));
                                                        if (objmatrix == null)
                                                        {

                                                            VisualMatrix vsmatrix2 = tempos.CreateObject<VisualMatrix>();
                                                            vsmatrix2.VisualMatrixName = strVisualMatrix;
                                                            vsmatrix2.DaysSampleKeeping = intDaykeepSamples;
                                                            vsmatrix2.MatrixName = testMethod.MatrixName;

                                                            //if (strMatrix != null)
                                                            //{
                                                            //    Matrix matrix = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]='" + strMatrix + "'"));
                                                            //    if (matrix != null)
                                                            //    {
                                                            //        vsmatrix2.MatrixName = matrix;
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        Matrix matrix1 = tempos.CreateObject<Matrix>();
                                                            //        matrix1.MatrixName = strMatrix;
                                                            //        tempos.CommitChanges();
                                                            //        vsmatrix2.MatrixName = ObjectSpace.GetObject<Matrix>(matrix1);

                                                            //    }
                                                            //}

                                                        }
                                                    }



                                                    if (strMethodName != null)
                                                    {
                                                        Method method = tempos.FindObject<Method>(CriteriaOperator.Parse("[MethodName] = ? AND [MethodNumber] = ?", strMethodName, strMethodNumber));
                                                        if (method != null)
                                                        {
                                                            testMethod.MethodName = method;
                                                        }
                                                        else
                                                        {
                                                            Method method1 = tempos.CreateObject<Method>();
                                                            method1.MethodName = strMethodName;
                                                            method1.MethodNumber = strMethodNumber;
                                                            tempos.CommitChanges();
                                                            testMethod.MethodName = tempos.GetObject<Method>(method1);
                                                            //testMethod.Method.MethodName = tempos.GetObject<Method>(method1);
                                                        }
                                                    }

                                                    if (strQcType != null)
                                                    {

                                                        QCType qCType = tempos.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName]= ? And [QcRole.QC_Role]= ? And [QCRootRole.QCRoot_Role]= ? And [QCSource.QC_Source]= ?", strQcType, strQCRole, strQCRootRole, strQCSource));

                                                        if (row["QCType"].GetType() == typeof(string))
                                                        {
                                                            string strFollowUpDate = row["QCType"].ToString();

                                                            if (qCType != null)
                                                            {
                                                                testMethod.QCTypes.Add(qCType);
                                                            }

                                                            else
                                                            {
                                                                qCType = tempos.CreateObject<QCType>();
                                                                qCType.QCTypeName = strQcType;

                                                                if (strQCSource != string.Empty)
                                                                {
                                                                    QCSource qcsource = tempos.CreateObject<QCSource>();
                                                                    qcsource.QC_Source = strQCSource;
                                                                    qCType.QCSource = qcsource;
                                                                }
                                                                else
                                                                {
                                                                    qCType.QCSource = null;
                                                                }
                                                                if (strQCRole != string.Empty)
                                                                {
                                                                    QcRole qcRole = tempos.CreateObject<QcRole>();
                                                                    qcRole.QC_Role = strQCRole;
                                                                    qCType.QcRole = qcRole;
                                                                    //= tempos.GetObject<QCRole>(qcRole);
                                                                }
                                                                else
                                                                {
                                                                    qCType.QcRole = null;

                                                                }

                                                                if (strQCRootRole != string.Empty)
                                                                {
                                                                    QCRootRole qcRootRole = tempos.CreateObject<QCRootRole>();
                                                                    qcRootRole.QCRoot_Role = strQCRootRole;
                                                                    qCType.QCRootRole = qcRootRole;
                                                                }
                                                                else
                                                                {
                                                                    qCType.QCRootRole = null;

                                                                }
                                                            }


                                                            testMethod.QCTypes.Add(qCType);
                                                            tempos.CommitChanges();
                                                        }

                                                    }


                                                    if (strParameter != null)
                                                    {
                                                        Modules.BusinessObjects.Setting.Parameter parameter = tempos.FindObject<Modules.BusinessObjects.Setting.Parameter>(CriteriaOperator.Parse("[ParameterName]='" + strParameter + "'"));
                                                        if (parameter != null)
                                                        {
                                                            Testparameter testparameter1 = tempos.FindObject<Testparameter>(CriteriaOperator.Parse("[Parameter.ParameterName]= ? And [Test.TestName]= ?", strParameter, strTestname));
                                                            if (testparameter1 == null)
                                                            {
                                                                Testparameter testparameter = tempos.CreateObject<Testparameter>();
                                                                testparameter.Parameter = parameter;
                                                                testparameter.IsGrouptm = strGrouptm;
                                                                testparameter.Sort = strSort;
                                                                testparameter.DefaultResult = strDefaultResult;
                                                                testparameter.FinalDefaultResult = strFinalDefaultResult;
                                                                testparameter.RptLimit = strRPLimit;
                                                                testparameter.MDL = strMDL;
                                                                testparameter.RSD = strRSD;
                                                                testparameter.RegulatoryLimit = strRegulatoryLimit;
                                                                testparameter.MCL = strMCL;
                                                                testparameter.LOQ = strLOQ;
                                                                testparameter.UQL = strUQL;
                                                                testparameter.LowCLimit = strLowCLimit;
                                                                testparameter.HighCLimit = strHighCLimit;
                                                                testparameter.RELCLimit = strRELCLimit;
                                                                testparameter.SigFig = strSigfig;
                                                                testparameter.CutOff = strCutOff;
                                                                testparameter.Decimal = strDecimal;
                                                                testparameter.Comment = strComment;
                                                                testparameter.SpikeAmount = strSpikeAmount;
                                                                testparameter.SurrogateAmount = strSurrogateAmount;
                                                                testparameter.SurrogateLowLimit = strSurrogateLLimit;
                                                                testparameter.SurrogateHighLimit = strSurrogateULimit;
                                                                testparameter.RecLCLimit = strRecLCLimit;
                                                                testparameter.RecHCLimit = strRecHCLimit;
                                                                testparameter.Surroagate = strSurroagate;
                                                                testparameter.InternalStandard = strInternalStandard;



                                                                if (strComponent != null)
                                                                {
                                                                    Modules.BusinessObjects.Setting.Component component = tempos.FindObject<Modules.BusinessObjects.Setting.Component>(CriteriaOperator.Parse("[Components]='" + strComponent + "'"));
                                                                    if (component != null)
                                                                    {
                                                                        testparameter.Component = component;
                                                                    }
                                                                    else
                                                                    {
                                                                        component = tempos.CreateObject<Modules.BusinessObjects.Setting.Component>();
                                                                        component.Components = "Default";
                                                                        testparameter.Component = component;
                                                                    }
                                                                }
                                                                if (strDefaultUnit != null)
                                                                {
                                                                    Modules.BusinessObjects.Setting.Unit unit = tempos.FindObject<Modules.BusinessObjects.Setting.Unit>(CriteriaOperator.Parse("[UnitName]='" + strFinalDefaultUnit + "'"));
                                                                    if (unit != null)
                                                                    {
                                                                        testparameter.FinalDefaultUnits.UnitName = unit.UnitName;
                                                                        testparameter.DefaultUnits.UnitName = unit.UnitName;
                                                                        testparameter.SpikeAmountUnit.UnitName = unit.UnitName;
                                                                    }
                                                                    else
                                                                    {
                                                                        unit = tempos.CreateObject<Unit>();
                                                                        unit.UnitName = strFinalDefaultResult;
                                                                        unit.UnitName = strDefaultUnit;
                                                                        unit.UnitName = strSpikeAmountUnits;
                                                                        testparameter.FinalDefaultUnits = unit;
                                                                        testparameter.DefaultUnits = unit;
                                                                        testparameter.SpikeAmountUnit = unit;
                                                                    }

                                                                }

                                                                if (strQcType != null)
                                                                {
                                                                    QCType qCType = tempos.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName]= ? ", strQcType));
                                                                    if (qCType != null)
                                                                    {
                                                                        testparameter.QCType = qCType;
                                                                    }
                                                                    else
                                                                    {
                                                                        qCType = tempos.CreateObject<QCType>();
                                                                        qCType.QCTypeName = strQcType;
                                                                        testparameter.QCType = qCType;
                                                                    }

                                                                }
                                                                testMethod.TestParameter.Add(testparameter);
                                                            }
                                                            else
                                                            {
                                                                testMethod.Parameters.Add(parameter);

                                                            }

                                                        }
                                                        else
                                                        {
                                                            parameter = tempos.CreateObject<Modules.BusinessObjects.Setting.Parameter>();
                                                            parameter.ParameterName = strParameter;
                                                            if (parameter != null)
                                                            {
                                                                Testparameter testparameter = tempos.CreateObject<Testparameter>();
                                                                testparameter.Parameter = parameter;
                                                                testparameter.Sort = strSort;
                                                                testparameter.IsGrouptm = strGrouptm;
                                                                testparameter.DefaultResult = strDefaultResult;
                                                                testparameter.FinalDefaultResult = strFinalDefaultResult;
                                                                testparameter.RptLimit = strRPLimit;
                                                                testparameter.MDL = strMDL;
                                                                testparameter.RSD = strRSD;
                                                                testparameter.RegulatoryLimit = strRegulatoryLimit;
                                                                testparameter.MCL = strMCL;
                                                                testparameter.LOQ = strLOQ;
                                                                testparameter.UQL = strUQL;
                                                                testparameter.LowCLimit = strLowCLimit;
                                                                testparameter.HighCLimit = strHighCLimit;
                                                                testparameter.RELCLimit = strRELCLimit;
                                                                testparameter.SigFig = strSigfig;
                                                                testparameter.CutOff = strCutOff;
                                                                testparameter.Decimal = strDecimal;
                                                                testparameter.Comment = strComment;
                                                                testparameter.SpikeAmount = strSpikeAmount;
                                                                testparameter.SurrogateAmount = strSurrogateAmount;
                                                                testparameter.SurrogateLowLimit = strSurrogateLLimit;
                                                                testparameter.SurrogateHighLimit = strSurrogateULimit;
                                                                testparameter.RecLCLimit = strRecLCLimit;
                                                                testparameter.RecHCLimit = strRecHCLimit;
                                                                testparameter.Surroagate = strSurroagate;
                                                                testparameter.InternalStandard = strInternalStandard;



                                                                if (strComponent != null)
                                                                {
                                                                    Modules.BusinessObjects.Setting.Component component = tempos.FindObject<Modules.BusinessObjects.Setting.Component>(CriteriaOperator.Parse("[Components]='" + strComponent + "'"));
                                                                    if (component != null)
                                                                    {
                                                                        testparameter.Component = component;
                                                                    }
                                                                    else
                                                                    {
                                                                        component = tempos.CreateObject<Modules.BusinessObjects.Setting.Component>();
                                                                        component.Components = "Default";
                                                                        testparameter.Component = component;
                                                                    }
                                                                }
                                                                if (strDefaultUnit != null)
                                                                {
                                                                    Modules.BusinessObjects.Setting.Unit unit = tempos.FindObject<Modules.BusinessObjects.Setting.Unit>(CriteriaOperator.Parse("[UnitName]='" + strFinalDefaultUnit + "'"));
                                                                    if (unit != null)
                                                                    {
                                                                        testparameter.FinalDefaultUnits.UnitName = unit.UnitName;
                                                                        testparameter.DefaultUnits.UnitName = unit.UnitName;
                                                                        testparameter.SpikeAmountUnit.UnitName = unit.UnitName;
                                                                    }
                                                                    else
                                                                    {
                                                                        unit = tempos.CreateObject<Unit>();
                                                                        unit.UnitName = strFinalDefaultResult;
                                                                        unit.UnitName = strDefaultUnit;
                                                                        unit.UnitName = strSpikeAmountUnits;
                                                                        testparameter.FinalDefaultUnits = unit;
                                                                        testparameter.DefaultUnits = unit;
                                                                        testparameter.SpikeAmountUnit = unit;
                                                                    }

                                                                }

                                                                if (strQcType != null)
                                                                {
                                                                    QCType qCType = tempos.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName]= ? ", strQcType));
                                                                    if (qCType != null)
                                                                    {
                                                                        testparameter.QCType = qCType;
                                                                    }
                                                                    else
                                                                    {
                                                                        qCType = tempos.CreateObject<QCType>();
                                                                        qCType.QCTypeName = strQcType;
                                                                        testparameter.QCType = qCType;
                                                                    }

                                                                }
                                                                testMethod.TestParameter.Add(testparameter);

                                                            }

                                                        }
                                                        tempos.CommitChanges();
                                                        tempos.Refresh();
                                                    }

                                                }
                                            }
                                            ObjectSpace.Refresh();
                                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Imported"), InformationType.Success, 4000, InformationPosition.Top);

                                        }

                                        else
                                        {
                                            var error = "Error in columns - " + string.Join(", ", errorlist) + " of row number - " + (dt.Rows.IndexOf(row) + 1);
                                            Application.ShowViewStrategy.ShowMessage(error, InformationType.Error, 6000, InformationPosition.Top);
                                            break;
                                        }


                                    }


                                }




                            }





                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "uploadfile"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            return;
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

        private void ImportFilesNew_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace sheetObjectSpace = Application.CreateObjectSpace(typeof(TestFileUpload));
                TestFileUpload spreadSheet = sheetObjectSpace.CreateObject<TestFileUpload>();
                DetailView createdView = Application.CreateDetailView(sheetObjectSpace, spreadSheet);
                createdView.ViewEditMode = ViewEditMode.Edit;
                e.DialogController.SaveOnAccept = false;
                e.View = createdView;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ExportTest_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string localserver = null;
                string server = null;
                if (objNavInfo.SelectedNavigationCaption == "Tests")
                {
                    localserver = HttpContext.Current.Server.MapPath(@"~\ExportToExcel\TestParameter.xlsx");
                    server = "TestParameter.xlsx";
                }
                else if (objNavInfo.SelectedNavigationCaption == "Matrix")
                {
                    localserver = HttpContext.Current.Server.MapPath(@"~\ExportToExcel\matrix.xlsx");
                    server = "matrix.xlsx";
                }

                else if (objNavInfo.SelectedNavigationCaption == "Sample Matrices")
                {
                    localserver = HttpContext.Current.Server.MapPath(@"~\ExportToExcel\samplesmatrix.xlsx");
                    server = "samplesmatrix.xlsx";
                }
                else if (objNavInfo.SelectedNavigationCaption == "Methods")
                {
                    localserver = HttpContext.Current.Server.MapPath(@"~\ExportToExcel\Method.xlsx");
                    server = "Method.xlsx";
                }
                else if (objNavInfo.SelectedNavigationCaption == "QC Type")
                {
                    localserver = HttpContext.Current.Server.MapPath(@"~\ExportToExcel\QCType.xlsx");
                    server = "QCType.xlsx";
                }
                else if (objNavInfo.SelectedNavigationCaption == "Parameter Library")
                {
                    localserver = HttpContext.Current.Server.MapPath(@"~\ExportToExcel\parameter.xlsx");
                    server = "parameter.xlsx";
                }

                if (File.Exists(localserver) == true)
                {
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachement; filename=\"" + server + "\"");
                    byte[] filepath = File.ReadAllBytes((localserver));
                    HttpContext.Current.Response.BinaryWrite((filepath));
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

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
