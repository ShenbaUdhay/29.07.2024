using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class COCSettingsCopyTestGroupTestViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        List<Guid> TPOid = new List<Guid>();
        string SLOid = string.Empty;
        string FocusedJobID = string.Empty;
        COCSettingsSampleInfo objCSInfo = new COCSettingsSampleInfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        COCSettingsSamples objParentCS;


        public COCSettingsCopyTestGroupTestViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "COCSettingsSamples_ListView_Copy_SampleRegistration;" + "COCSettingsSamples_DetailView;" + "COCSettingsSamples_ListView;" + "COCSettingsSamples_LookupListView_Copy_COCSamples_Copy_CopyTest;" + "COCSettings_Testparameters_ListView;";
            COC_CopyTest.TargetViewId = "COCSettingsSamples_ListView_Copy_SampleRegistration;" + "COCSettings_Testparameters_ListView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        private void COC_CopyTest_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                List<string> FocusedMatrix = new List<string>();
                if (View != null && View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration" && View.SelectedObjects.Count == 1)
                {
                    foreach (COCSettingsSamples objtp in View.SelectedObjects)
                    {
                        foreach (Testparameter testparameter in objtp.Testparameters)
                        {
                            TPOid.Add(testparameter.Oid);
                            FocusedMatrix.Add(testparameter.TestMethod.MatrixName.MatrixName.ToString());
                        }
                        objCSInfo.COCOid = objtp.Oid.ToString();
                    }
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    object objToShow = objspace.CreateObject(typeof(COCSettingsSamples));
                    if (objToShow != null)
                    {
                        CollectionSource cs = new CollectionSource(objspace, typeof(COCSettingsSamples));
                        cs.Criteria.Clear();
                        if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration" && !string.IsNullOrEmpty(objCSInfo.COCOid))
                        {
                            cs.Criteria["filter1"] = CriteriaOperator.Parse("[COCID.COC_ID]='" + objCSInfo.focusedCOCID + "' and Oid <> ?", new Guid(objCSInfo.COCOid));
                        }
                        else
                        {
                            cs.Criteria["filter1"] = CriteriaOperator.Parse("[COCID.COC_ID]='" + objCSInfo.focusedCOCID + "'");
                        }
                        cs.Criteria["filter2"] = new InOperator("VisualMatrix.MatrixName.MatrixName", FocusedMatrix);
                        ListView dvbottleAllocation = Application.CreateListView("COCSettingsSamples_LookupListView_Copy_COCSamples_Copy_CopyTest", cs, false);
                        ShowViewParameters showViewParameters = new ShowViewParameters(dvbottleAllocation);
                        showViewParameters.CreatedView = dvbottleAllocation;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.Accepting += CopyTest_Accepting;
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void CopyTest_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                ObjectSpace.CommitChanges();
                IObjectSpace os = Application.CreateObjectSpace();
                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                UnitOfWork uow = new UnitOfWork(((XPObjectSpace)os).Session.DataLayer);
                IList<Testparameter> objtp = null;
                if (TPOid.Count > 0)
                {
                    //objtp = os.GetObjects<Testparameter>(new InOperator("Oid", TPOid));
                    XPClassInfo TestParameterinfo;
                    TestParameterinfo = uow.GetClassInfo(typeof(Testparameter));
                    objtp = uow.GetObjects(TestParameterinfo, new InOperator("Oid", TPOid), null, int.MaxValue, false, true).Cast<Testparameter>().ToList();
                    CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + objCSInfo.COCOid + "'");
                    objParentCS = uow.FindObject<COCSettingsSamples>(criteria);
                }
                foreach (COCSettingsSamples obj in e.AcceptActionArgs.SelectedObjects)
                {
                    if (objtp != null)
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + obj.Oid + "'");
                        COCSettingsSamples objSL = uow.FindObject<COCSettingsSamples>(criteria);
                        IList<COCSettingsTest> objsp = (IList<COCSettingsTest>)objParentCS.COCSettingsTests;
                        if (objsp != null)
                        {
                            foreach (Testparameter objtestperam in objtp)
                            {
                                if (!objSL.Testparameters.Contains(objtestperam))
                                {
                                    foreach (COCSettingsTest sp in objsp)
                                    {
                                        if (sp != null)
                                        {
                                            if (objtestperam.Oid == sp.Testparameter.Oid)
                                            {
                                                objSL.Testparameters.Add(objtestperam);
                                                if (sp.IsGroup == true && sp.GroupTest != null)
                                                {
                                                    COCSettingsTest sample = objSL.COCSettingsTests.FirstOrDefault<COCSettingsTest>(i => i.Testparameter.Oid == sp.Testparameter.Oid);
                                                    if (sample != null)
                                                    {
                                                        sample.IsGroup = true;
                                                        sample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(sp.GroupTest.Oid);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            objSL.Save();
                            uow.CommitChanges();
                        }
                        uow.CommitChanges();
                        Frame.GetController<Settings.COCSettingsViewController>().AssignBottleAllocationToSamples(uow, objSL.Oid);
                    }
                }
                if (TPOid.Count > 0)
                {
                    TPOid.Clear();
                }

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
            // Access and customize the target View control.
            try
            {
                COC_CopyTest.Executing += COC_CopyTest_Executing;
                if (View != null && View.CurrentObject != null && View.Id == "COCSettingsSamples_DetailView")
                {
                    objCSInfo.COCOid = ObjectSpace.GetKeyValueAsString((COCSettingsSamples)View.CurrentObject);
                    if (((DetailView)View).ViewEditMode == DevExpress.ExpressApp.Editors.ViewEditMode.View)
                    {
                        COC_CopyTest.Active.SetItemValue("COC_CopyTest", false);
                        //SL_GroupTest.Active.SetItemValue("SL_GroupTest", false);
                    }
                    else
                    {
                        COC_CopyTest.Active.SetItemValue("COC_CopyTest", true);
                        // SL_GroupTest.Active.SetItemValue("SL_GroupTest", true);
                    }
                    ((DetailView)View).FindItem("Testparameters").Refresh();
                }
                else if (View != null && View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration")
                {
                    if (objPermissionInfo.COCSettingsIsWrite == false)
                    {
                        COC_CopyTest.Active["showCopyTest"] = false;
                    }
                    else
                    {
                        COC_CopyTest.Active["showCopyTest"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void COC_CopyTest_Executing(object sender, CancelEventArgs e)
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
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
