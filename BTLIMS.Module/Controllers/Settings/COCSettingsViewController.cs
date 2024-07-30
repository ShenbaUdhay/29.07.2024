using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class COCSettingsViewController : ViewController, IXafCallbackHandler
    {
        bool bolRefresh = false;
        PermissionInfo objPermissionInfo = new PermissionInfo();
        MessageTimer timer = new MessageTimer();
        COCSettingsSampleCheckInInfo objCOCInfo = new COCSettingsSampleCheckInInfo();
        COCSettingsRegistrationInfo COCsr = new COCSettingsRegistrationInfo();
        COCSettingsSampleInfo objCOCSampleinfo = new COCSettingsSampleInfo();
        curlanguage objLanguage = new curlanguage();
        TestMethodInfo objInfo = new TestMethodInfo();
        viewInfo tempviewinfo = new viewInfo();
        CopyNoOfSamplesPopUp objCopySampleInfo = new CopyNoOfSamplesPopUp();
        private StaticText staticText;
        bool IsDisableCheckBox = false;

        SimpleAction btnDeleteSamplesandTest;
        string jScript = @"
                        
                       Grid.UpdateEdit();
                       ";

        public COCSettingsViewController()
        {
            InitializeComponent();
            TargetViewId = "COCSettings_DetailView_Copy_SampleRegistration;" + "COCSettings_ListView;" + "COCSettingsSamples_ListView_Copy_SampleRegistration;" + "COCSettingsSampleRegistration;"
                 + "COCTest;" + "COCSettingsTest_ListView_Copy_SampleRegistration;" + "Testparameter_LookupListView_Copy_COCSample;" + "Testparameter_LookupListView_Copy_COCSample_Copy;" + "Testparameter_LookupListView_Copy_COCSample_Copy_Parameter;" + "ProjectCategory_DetailView;";
            Sample.TargetViewId = "COCSettings_DetailView_Copy_SampleRegistration";
            Test.TargetViewId = Containers.TargetViewId = "COCSettingsSamples_ListView_Copy_SampleRegistration";
            COCSR_SLListViewEdit.TargetViewId = "COCSettingsSamples_ListView_Copy_SampleRegistration";
            AddSample.TargetViewId = "COCSettingsSamples_ListView_Copy_SampleRegistration;" + "COCSettingsSamples_DetailView;" + "COCSettingsSamples_ListView;";
            //CopyTest.TargetViewId = "COCSettingsSamples_ListView_Copy_SampleRegistration";
            TestGroup.TargetViewId = "COCSettingsSamples_ListView_Copy_SampleRegistration";
            TestSelectionAdd.TargetViewId = TestSelectionRemove.TargetViewId = TestSelectionSave.TargetViewId = "COCTest";

            SimpleAction btnSampleTest = new SimpleAction(this, "btnCOCSampleTest", PredefinedCategory.Unspecified)
            {
                Caption = "Tests"
            };
            btnSampleTest.TargetViewId = "COCSettings_DetailView_Copy_SampleRegistration";
            btnSampleTest.Execute += btnSampleTest_Execute;
            btnSampleTest.Category = "catCOCSample";
            SimpleAction btnBottleAllocation = new SimpleAction(this, "btnCOCBottleAllocation", PredefinedCategory.Unspecified)
            {
                Caption = "Containers"
            };
            btnBottleAllocation.TargetViewId = "COCSettings_DetailView_Copy_SampleRegistration";
            btnBottleAllocation.Execute += btnBottleAllocation_Execute;
            btnBottleAllocation.Category = "catCOCSample";

            btnDeleteSamplesandTest = new SimpleAction(this, "btnDeleteSamplesandTest1", PredefinedCategory.ObjectsCreation)
            {
                Caption = "Delete"
            };
            btnDeleteSamplesandTest.TargetViewId = /*"COCSettingsSamples_ListView_Copy_SampleRegistration;" +*/ "COCSettingsTest_ListView_Copy_SampleRegistration;";
            btnDeleteSamplesandTest.Executing += DeleteAction_Executing;
            btnDeleteSamplesandTest.Execute += DeleteAction_Execute;
            btnDeleteSamplesandTest.ImageName = "Action_Delete";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        private void DeleteAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        List<COCSettingsSamples> lstcocSampleLogin = View.SelectedObjects.Cast<COCSettingsSamples>().ToList();
                        IObjectSpace os = Application.CreateObjectSpace();
                        Session currentSession = ((XPObjectSpace)os).Session;
                        UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                        foreach (COCSettingsSamples objcocSampleLogin in lstcocSampleLogin)
                        {
                            COCSettingsSamples obj = uow.GetObjectByKey<COCSettingsSamples>(objcocSampleLogin.Oid);
                            if (obj.Testparameters.Count > 0)
                            {
                                bool IsDeleted = false;
                                List<COCSettingsTest> lstSampleParameters = obj.COCSettingsTests.Cast<COCSettingsTest>().ToList();
                                foreach (COCSettingsTest objSampleParam in lstSampleParameters)
                                {
                                    COCSettingsTest sampleParam = uow.GetObjectByKey<COCSettingsTest>(objSampleParam.Oid);
                                    uow.Delete(uow.GetObjectByKey<COCSettingsTest>(sampleParam.Oid));
                                    IsDeleted = true;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                                if (IsDeleted)
                                {
                                    uow.CommitChanges();
                                }
                                int spCount = View.ObjectSpace.GetObjectsCount(typeof(COCSettingsTest), CriteriaOperator.Parse("[COCSettingsSamples.Oid] = ?", obj.Oid));
                                if (spCount == 0)
                                {
                                    XPClassInfo BottleSetupinfo;
                                    XPClassInfo SampleBottleAllocationinfo;
                                    SampleBottleAllocationinfo = uow.GetClassInfo(typeof(COCSettingsBottleAllocation));
                                    IList<COCSettingsBottleAllocation> lstbottleAllocation = uow.GetObjects(SampleBottleAllocationinfo, CriteriaOperator.Parse("COCSettingsRegistration=?", obj.Oid), new SortingCollection(), 0, 0, false, true).Cast<COCSettingsBottleAllocation>().ToList();
                                    if (lstbottleAllocation.Count > 0)
                                    {
                                        foreach (COCSettingsBottleAllocation objSamplebottleAll in lstbottleAllocation.ToList())
                                        {
                                            COCSettingsBottleAllocation objbottleAll = uow.GetObjectByKey<COCSettingsBottleAllocation>(objSamplebottleAll.Oid);
                                            uow.Delete(objbottleAll);
                                        }
                                    }
                                    uow.Delete(obj);
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletesample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else
                            {
                                //base.Delete(args);
                                uow.Delete(obj);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        uow.CommitChanges();
                        if (Application.MainWindow.View is DashboardView)
                        {
                            DashboardViewItem dvSampleCheckin = ((DashboardView)Application.MainWindow.View).FindItem("COCSettings") as DashboardViewItem;
                            if (dvSampleCheckin != null && dvSampleCheckin.InnerView != null)
                            {
                                COCSettings objCurrent = (COCSettings)dvSampleCheckin.InnerView.CurrentObject;
                                if (objCurrent != null)
                                {
                                    List<COCSettingsSamples> lstSamples = uow.Query<COCSettingsSamples>().Where(i => i.COCID != null && i.COCID.Oid == objCurrent.Oid).ToList();
                                    if (lstSamples.Count == 0)
                                    {
                                        COCSettings objCurre = uow.GetObjectByKey<COCSettings>(objCurrent.Oid);
                                        if (objCurre != null)
                                        {
                                            uow.CommitChanges();
                                        }
                                    }
                                }
                            }
                        }
                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            if (nestedFrame != null)
                            {
                                CompositeView view = nestedFrame.ViewItem.View;
                                foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                                {
                                    if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                                    {
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
                        }
                        Application.MainWindow.View.ObjectSpace.Refresh();
                    }
                }
                else if (View != null && View.Id == "COCSettingsTest_ListView_Copy_SampleRegistration")
                {
                    List<Tuple<string, Guid, Guid>> lstDeletedSampleParameters = new List<Tuple<string, Guid, Guid>>();
                    bool IsDeleted = false;
                    var os = Application.CreateObjectSpace();
                    foreach (COCSettingsTest obj in View.SelectedObjects)
                    {
                        if (obj != null)
                        {
                            //base.Delete(args);
                            os.Delete(os.GetObject<COCSettingsTest>(obj));
                            if (lstDeletedSampleParameters.FirstOrDefault(i => i.Item2 == obj.COCSettingsSamples.Oid && i.Item3 == obj.Testparameter.TestMethod.Oid) == null)
                            {
                                Tuple<string, Guid, Guid> tupDeletedSampleTest = new Tuple<string, Guid, Guid>(obj.COCSettingsSamples.SampleID, obj.COCSettingsSamples.Oid, obj.Testparameter.TestMethod.Oid);
                                lstDeletedSampleParameters.Add(tupDeletedSampleTest);
                            }
                            IsDeleted = true;

                        }
                    }
                    IList<COCSettingsTest> distinctSample = ((ListView)View).SelectedObjects.Cast<COCSettingsTest>().ToList().GroupBy(p => new { p.Testparameter.TestMethod, p.COCSettingsSamples }).Select(g => g.First()).ToList();
                    foreach (COCSettingsTest objs in distinctSample)
                    {
                        COCSettingsBottleAllocation objAllocation = os.FindObject<COCSettingsBottleAllocation>(CriteriaOperator.Parse("[COCSettingsRegistration.Oid]=? and [TestMethod.Oid]=?", objs.COCSettingsSamples.Oid, objs.Testparameter.TestMethod.Oid));
                        if (objAllocation != null)
                        {
                            os.Delete(objAllocation);
                            IsDeleted = true;
                        }

                    }
                    if (IsDeleted == true)
                    {
                        os.CommitChanges();

                        //foreach (Tuple<string, Guid, Guid> tupDeletedSampleTest in lstDeletedSampleParameters)
                        //{
                        //    IList<SampleParameter> lstSamples = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid] = ? And [Testparameter.TestMethod.Oid] = ?", tupDeletedSampleTest.Item2, tupDeletedSampleTest.Item3));
                        //    if (lstSamples != null && lstSamples.Count == 0)
                        //    {

                        //    }
                        //}
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            if (nestedFrame != null)
                            {
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
                        }
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
        private void DeleteAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void btnBottleAllocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ObjectSpace.CommitChanges();
                Save();
                {
                    COCSettings objsmplcheckin = (COCSettings)View.CurrentObject;
                    if (objsmplcheckin != null)
                    {
                        COCsr.strCOCID = objsmplcheckin.COC_ID;
                        string[] strvmarr = objsmplcheckin.SampleMatries.Split(';');
                        COCsr.lstCOCvisualmat = new List<VisualMatrix>();
                        foreach (string strvmoid in strvmarr.ToList())
                        {
                            VisualMatrix lstvmatobj = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strvmoid)));
                            if (lstvmatobj != null)
                            {
                                COCsr.lstCOCvisualmat.Add(lstvmatobj);
                            }
                        }
                        //IObjectSpace os = Application.CreateObjectSpace();
                        COCSettingsBottleAllocation newsmplbtlalloc = View.ObjectSpace.CreateObject<COCSettingsBottleAllocation>();
                        DetailView dvbottleAllocation = Application.CreateDetailView(View.ObjectSpace, "COCSettingsBottleAllocation_DetailView_Copy_SampleRegistration", false, newsmplbtlalloc);
                        ShowViewParameters showViewParameters = new ShowViewParameters(dvbottleAllocation);
                        showViewParameters.CreatedView = dvbottleAllocation;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.AcceptAction.Active["OkayBtn"] = false;
                        dc.CancelAction.Active["CancelBtn"] = false;
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
        private void btnSampleTest_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (ObjectSpace.IsModified)
                {
                    ObjectSpace.CommitChanges();
                }
                Save();
                if (!COCsr.isNoOfSampleDisable)
                {
                    InsertSamplesInCOCSettingsSample();
                }
                COCSettings objsmplcheckin = (COCSettings)View.CurrentObject;
                if (objsmplcheckin != null)
                {
                    COCsr.strCOCID = objsmplcheckin.COC_ID;
                    string[] strvmarr = objsmplcheckin.SampleMatries.Split(';');
                    COCsr.lstCOCvisualmat = new List<VisualMatrix>();
                    foreach (string strvmoid in strvmarr.ToList())
                    {
                        VisualMatrix lstvmatobj = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strvmoid)));
                        if (lstvmatobj != null)
                        {
                            COCsr.lstCOCvisualmat.Add(lstvmatobj);
                        }
                    }
                    CollectionSource cs = new CollectionSource(View.ObjectSpace, typeof(COCSettingsTest));
                    cs.Criteria["filter"] = CriteriaOperator.Parse("[COCSettingsSamples.COCID.COC_ID] = ? AND [COCSettingsSamples.COCID.GCRecord] is NULL /*AND [COCSettingsSamples.GCRecord] is NULL*/", COCsr.strCOCID);
                    ListView dvbottleAllocation = Application.CreateListView("COCSettingsTest_ListView_Copy_SampleRegistration", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(dvbottleAllocation);
                    showViewParameters.CreatedView = dvbottleAllocation;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.AcceptAction.Active["OkayBtn"] = false;
                    dc.CancelAction.Active["CancelBtn"] = false;
                    dc.CloseOnCurrentObjectProcessing = false;
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
        protected override void OnActivated()
        {
            base.OnActivated();
            if (View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
            {
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                COCSettings obj = (COCSettings)View.CurrentObject;
                if (!View.ObjectSpace.IsNewObject(obj))
                {
                    List<COCSettingsSamples> lstSamples = View.ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("COCID.Oid=?", obj.Oid)).ToList();
                    if (lstSamples.Count == 0)
                    {
                        obj.NoOfSamples = 1;
                    }
                }
            }
            if (View != null && (View.Id == "COCSettings_ListView" || View.Id == "COCSettings_DetailView_Copy_SampleRegistration"))
            {
                objPermissionInfo.COCSettingsIsWrite = false;
                objPermissionInfo.COCSettingsIsDelete = false;
                Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                if (user.Roles != null && user.Roles.Count > 0)
                {
                    if (user.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        objPermissionInfo.COCSettingsIsWrite = true;
                        objPermissionInfo.COCSettingsIsDelete = true;
                        objPermissionInfo.COCSettingsIsCreate = true;
                        Frame.GetController<NewObjectViewController>().Active["showNew"] = true;
                    }
                    else
                    {
                        foreach (RoleNavigationPermission role in user.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationModelClass == View.ObjectTypeInfo.FullName && i.NavigationItem.IsDeleted == false && i.Create == true) != null)
                            {
                                objPermissionInfo.COCSettingsIsCreate = true;
                                //return;
                                //break;
                            }
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationModelClass == View.ObjectTypeInfo.FullName && i.NavigationItem.IsDeleted == false && i.Write == true) != null)
                            {
                                objPermissionInfo.COCSettingsIsWrite = true;
                                //return;
                                //break;
                            }
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationModelClass == View.ObjectTypeInfo.FullName && i.NavigationItem.IsDeleted == false && i.Delete == true) != null)
                            {
                                objPermissionInfo.COCSettingsIsDelete = true;
                                //return;
                                //break;
                            }
                        }
                        if (objPermissionInfo.COCSettingsIsWrite)
                        {
                            Frame.GetController<NewObjectViewController>().Active["showNew"] = true;
                        }
                        else
                        {
                            Frame.GetController<NewObjectViewController>().Active["showNew"] = false;
                        }
                        if (objPermissionInfo.COCSettingsIsDelete)
                        {
                            Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active["ShowDelete"] = true;
                        }
                        else
                        {
                            Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active["ShowDelete"] = false;
                        }
                    }
                }
                if (View.CurrentObject != null)
                {
                    COCsr.CurrentCOC = View.CurrentObject as COCSettings;
                    COCsr.NewClient = null;
                    COCsr.NewProject = null;
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Execute += SaveAction_Execute;
                        modificationController.SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
                        modificationController.SaveAndNewAction.Executing += SaveAndNewAction_Executing;
                    }
                    if (View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                    {
                        //DefaultSetting objSampleTracking = View.ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = ? AND [IsModule]= False AND [Select] = False AND NavigationItemName=?", "Settings", "COC Settings"));
                        //if (objSampleTracking != null)
                        //{
                        //    btnImportSamples.Active["hideCoCbutton"] = false;
                        //}
                        //else if (objSampleTracking == null)
                        //{
                        //    btnImportSamples.Active.RemoveItem("hideCoCbutton");
                        //}
                    }

                }
                ObjectSpace.Committing += ObjectSpace_Committing; ;
                //ObjectSpace.Committed += ObjectSpace_Committed;
                if (objLanguage.strcurlanguage != "En")
                {
                    //Sample.Caption = "样品(0)";
                    Sample.Caption = "样液样品(0)";
                }
                else
                {
                    Sample.Caption = "Samples(0)";
                }
                if (objPermissionInfo.COCSettingsIsWrite)
                {
                    //SRSubmit.Active["showsubmit"] = true;
                    btnDeleteSamplesandTest.Active["delete"] = true;
                }
                else
                {
                    //SRSubmit.Active["showsubmit"] = false;
                    btnDeleteSamplesandTest.Active["delete"] = false;
                }
            }
            if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration" && !objCOCSampleinfo.IsColumnsCustomized && !string.IsNullOrEmpty(COCsr.strCOCID))
            {
                COCsr.canGridRefresh = true;
                objCOCSampleinfo.IsColumnsCustomized = true;
                WebWindow.CurrentRequestWindow.PagePreRender += CurrentRequestWindow_PagePreRender;
                Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                if (user.Roles != null && user.Roles.Count > 0)
                {
                    if (user.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null && Application.MainWindow.View.ObjectTypeInfo.Type == typeof(COCSettings) && ((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit)
                    {
                        objPermissionInfo.COCSettingsIsWrite = true;
                    }
                    else
                    {
                        foreach (RoleNavigationPermission role in user.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationView == View.Id && i.Write == true) != null)
                            {
                                if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(COCSettings) && ((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit)
                                {
                                    objPermissionInfo.COCSettingsIsWrite = true;
                                }
                            }
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationModelClass == View.ObjectTypeInfo.FullName && i.Delete == true) != null)
                            {
                                if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(COCSettings) && ((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit)
                                {
                                    objPermissionInfo.COCSettingsIsWrite = true;
                                }
                            }
                        }
                    }
                }
            }
            COCsr.lstObjectsToShow = new List<Guid>();
            TestGroup.Active.SetItemValue("TestGroup", false);
            if (Frame is DevExpress.ExpressApp.Web.PopupWindow)
            {
                DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                if (popupWindow != null)
                {
                    popupWindow.RefreshParentWindowOnCloseButtonClick = true;// This is for the cross (X) button of ASPxPopupControl.  
                    DialogController dc = popupWindow.GetController<DialogController>();
                    //if (dc != null)
                    //{
                    //    dc.ViewClosing += Dc_ViewClosing;
                    //}
                }
            }
            //CopySamples.Executing += CopySamples_Executing;
            if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration")
            {
                COCsr.CanRefresh = true;
                //ObjectSpace.Committing += ObjectSpace_Committing;
                Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                Frame.GetController<DeleteObjectsViewController>().DeleteAction.Category = "PopupActions";
            }
            if (View.Id == "COCSettingsSampleRegistration")
            {
                View.Closing += View_Closing;
                View.Closed += View_Closed;
            }
            if (View.Id == "COCTest")
            {
                staticText = (StaticText)((DashboardView)this.View).FindItem("SampleMatrix");

            }
            if (View.Id == "COCSettingsTest_ListView_Copy_SampleRegistration")
            {
                if (objPermissionInfo.COCSettingsIsDelete)
                {
                    //COCsr.CanRefresh = true;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                }
            }

            // Perform various tasks depending on the target View.
        }

        private void ObjectSpace_Committing(object sender, CancelEventArgs e)
        {
            try
            {
                COCSettings obj = (COCSettings)View.CurrentObject;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);

            }
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (base.View != null && e.NewValue != e.OldValue && base.View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                {
                    if (View != null && View.CurrentObject == e.Object)
                    {
                        COCSettings objCOCsettings = (COCSettings)e.Object;
                        COCsr.CurrentCOC = objCOCsettings;
                        if (e.PropertyName != "NoOfSamples")
                        {
                            COCsr.IsSamplePopupClose = false;
                        }
                        if (e.PropertyName == "ClientName")
                        {
                            if (e.NewValue != null && objCOCsettings.ClientName != null)
                            {
                                objInfo.ClientName = objCOCsettings.ClientName.CustomerName;
                            }
                            else
                            {
                                objInfo.ClientName = null;
                            }
                            objCOCsettings.QuoteID = null;
                            objCOCsettings.ClientContact = null;
                            objCOCsettings.ProjectID = null;
                            ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("ProjectID") as ASPxGridLookupPropertyEditor;
                            if (objCOCsettings.ClientName != null)
                            {
                                propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("[customername.Oid] = ? ", objCOCsettings.ClientName.Oid);
                            }
                            else
                            {
                                propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                            }
                            propertyEditor.Refresh();
                            propertyEditor.RefreshDataSource();
                        }
                        else if (e.PropertyName == "NPTest")
                        {
                            COCSettings objTask = (COCSettings)e.Object;

                            if (!string.IsNullOrEmpty(objTask.NPTest) && !string.IsNullOrEmpty(objTask.SampleMatries))
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                HttpContext.Current.Session["Test"] = objTask.NPTest;
                                if (HttpContext.Current.Session["Test"] != null)
                                {
                                    List<VisualMatrix> lstVM = new List<VisualMatrix>();
                                    foreach (string strMatrix in objTask.SampleMatries.Split(';'))
                                    {
                                        VisualMatrix objSM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strMatrix));
                                        if (objSM != null)
                                        {
                                            lstVM.Add(objSM);
                                        }
                                    }
                                    string[] TestOid = HttpContext.Current.Session["Test"].ToString().Split(new string[] { ";" }, StringSplitOptions.None);
                                    if (objCOCInfo.lstTestOid != null && objCOCInfo.lstTestOid.Count > 0)
                                    {
                                        objCOCInfo.lstTestOid.Clear();
                                    }

                                    if (TestOid != null && TestOid.Count() > 0)
                                    {
                                        foreach (string strTest in TestOid)
                                        {
                                            List<string> lstTestName = strTest.Split('|').ToList();
                                            if (lstTestName.Count == 2)
                                            {
                                                IList<TestMethod> lstTests = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [MethodName.MethodNumber] = ?", lstTestName[0], lstTestName[1]));
                                                if (objCOCInfo.lstTestOid == null)
                                                {
                                                    objCOCInfo.lstTestOid = new List<Guid>();
                                                    foreach (TestMethod obj in lstTests.ToList())
                                                    {
                                                        if (!objCOCInfo.lstTestOid.Contains(obj.Oid))
                                                        {
                                                            objCOCInfo.lstTestOid.Add(obj.Oid);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (TestMethod obj in lstTests.ToList())
                                                    {
                                                        if (!objCOCInfo.lstTestOid.Contains(obj.Oid))
                                                        {
                                                            objCOCInfo.lstTestOid.Add(obj.Oid);
                                                        }
                                                    }
                                                }
                                            }
                                            else if (lstTestName.Count == 1)
                                            {
                                                IList<TestMethod> lstTests = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [IsGroup]=true", lstTestName[0]));
                                                if (objCOCInfo.lstTestOid == null)
                                                {
                                                    objCOCInfo.lstTestOid = new List<Guid>();
                                                    foreach (TestMethod obj in lstTests.ToList())
                                                    {
                                                        if (!objCOCInfo.lstTestOid.Contains(obj.Oid))
                                                        {
                                                            objCOCInfo.lstTestOid.Add(obj.Oid);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (TestMethod obj in lstTests.ToList())
                                                    {
                                                        if (!objCOCInfo.lstTestOid.Contains(obj.Oid))
                                                        {
                                                            objCOCInfo.lstTestOid.Add(obj.Oid);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    ListPropertyEditor lstccd = ((DetailView)View).FindItem("CustomDueDates") as ListPropertyEditor;
                                    if (lstccd != null && lstccd.ListView == null)
                                    {
                                        lstccd.CreateControl();
                                    }
                                    ListView lstcustomduedate = lstccd.ListView;
                                    if (lstcustomduedate != null && lstcustomduedate.CollectionSource.List.Count > 0)
                                    {
                                        string[] arrNewTests = e.NewValue.ToString().Split(new string[] { ";" }, StringSplitOptions.None);
                                        string[] arrOldTests = e.OldValue.ToString().Split(new string[] { ";" }, StringSplitOptions.None);
                                        List<Guid> lstOid = new List<Guid>();
                                        foreach (string strOid in arrOldTests)
                                        {
                                            //Guid oid = new Guid(strOid);
                                            List<string> lstTest = strOid.Split('|').ToList();
                                            if (lstTest.Count == 2)
                                            {
                                                IList<TestMethod> lstTests = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [MethodName.MethodNumber] = ?", lstTest[0], lstTest[1]));
                                                foreach (TestMethod obj in lstTests.ToList())
                                                {
                                                    if (arrNewTests.FirstOrDefault(i => i == strOid) == null && !lstOid.Contains(obj.Oid))
                                                    {
                                                        lstOid.Add(obj.Oid);
                                                    }
                                                }
                                            }
                                            else if (lstTest.Count == 1)
                                            {
                                                IList<TestMethod> lstTests = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [IsGroup] =true", lstTest[0]));
                                                foreach (TestMethod obj in lstTests.ToList())
                                                {
                                                    if (arrNewTests.FirstOrDefault(i => i == strOid) == null && !lstOid.Contains(obj.Oid))
                                                    {
                                                        lstOid.Add(obj.Oid);
                                                    }
                                                }
                                            }

                                        }
                                        //var objlst = obj2.ToList().ForEach(i => obj1);
                                        foreach (CustomDueDate clr in lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().Where(i => lstOid.Contains(i.TestMethod.Oid)).ToList())
                                        {
                                            lstcustomduedate.CollectionSource.Remove(clr);
                                        }
                                    }
                                    if (objCOCInfo.lstTestOid != null && objCOCInfo.lstTestOid.Count > 0)
                                    {
                                        foreach (string val in objCOCInfo.lstTestOid.Select(i => i.ToString()).ToList())
                                        {
                                            if (!string.IsNullOrEmpty(val))
                                            {
                                                TestMethod objTestMethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", new Guid(val)));
                                                //if (objTestMethod != null && objTask.CustomDueDates.FirstOrDefault(i => i.TestMethod != null && i.TestMethod.Oid == new Guid(val) && i.TestMethod.TestName == objTestMethod.TestName) == null)
                                                if (objTestMethod != null)
                                                {
                                                    if (lstcustomduedate != null)
                                                    {
                                                        var lst = lstVM.Where(i => i.MatrixName.Oid == objTestMethod.MatrixName.Oid);
                                                        if (lst != null && lst.Count() > 0)
                                                        {
                                                            foreach (VisualMatrix objSM in lst.ToList())
                                                            {
                                                                if (objTask.CustomDueDates.FirstOrDefault(i => i.TestMethod.Oid == objTestMethod.Oid && i.SampleMatrix.Oid == objSM.Oid) == null)
                                                                {
                                                                    CustomDueDate objDate = ObjectSpace.CreateObject<CustomDueDate>();
                                                                    objDate.TestMethod = objTestMethod;
                                                                    objDate.COCSettings = objTask;
                                                                    objDate.SampleMatrix = objSM;
                                                                    objDate.Parameter = "AllParam";
                                                                    if (objCOCsettings != null && objCOCsettings.TAT != null)
                                                                    {
                                                                        //objDate.DueDate = objCOCsettings.DueDate;
                                                                        objDate.TAT = objCOCsettings.TAT;
                                                                    }
                                                                    lstcustomduedate.CollectionSource.Add(objDate);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                ListPropertyEditor lstccd = ((DetailView)View).FindItem("CustomDueDates") as ListPropertyEditor;
                                ListView lstcustomduedate = lstccd.ListView;
                                if (lstcustomduedate != null)
                                {
                                    foreach (CustomDueDate clr in lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().ToList())
                                    {
                                        lstcustomduedate.CollectionSource.Remove(clr);
                                    }
                                }
                            }
                        }
                        else if (e.PropertyName == "SampleMatries")
                        {
                            COCSettings objTask = (COCSettings)e.Object;

                            if (!string.IsNullOrEmpty(objTask.NPTest) && !string.IsNullOrEmpty(objTask.SampleMatries))
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                HttpContext.Current.Session["Test"] = objTask.NPTest;
                                if (HttpContext.Current.Session["Test"] != null)
                                {
                                    List<VisualMatrix> lstVM = new List<VisualMatrix>();
                                    foreach (string strMatrix in objTask.SampleMatries.Split(';'))
                                    {
                                        VisualMatrix objSM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strMatrix));
                                        if (objSM != null)
                                        {
                                            lstVM.Add(objSM);
                                        }
                                    }
                                    //CustomDueDate cdd = View.CurrentObject as CustomDueDate;
                                    if (objCOCInfo.lstTestOid != null && objCOCInfo.lstTestOid.Count > 0)
                                    {
                                        objCOCInfo.lstTestOid.Clear();
                                    }
                                    string[] TestOid = HttpContext.Current.Session["Test"].ToString().Split(new string[] { ";" }, StringSplitOptions.None);
                                    if (TestOid != null && TestOid.Count() > 0)
                                    {
                                        foreach (string strTest in TestOid)
                                        {
                                            List<string> lstTestName = strTest.Split('|').ToList();
                                            if (lstTestName.Count == 2)
                                            {
                                                IList<TestMethod> lstTests = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [MethodName.MethodNumber] = ?", lstTestName[0], lstTestName[1]));
                                                if (objCOCInfo.lstTestOid == null)
                                                {
                                                    objCOCInfo.lstTestOid = new List<Guid>();
                                                    foreach (TestMethod obj in lstTests.ToList())
                                                    {
                                                        if (!objCOCInfo.lstTestOid.Contains(obj.Oid))
                                                        {
                                                            objCOCInfo.lstTestOid.Add(obj.Oid);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (TestMethod obj in lstTests.ToList())
                                                    {
                                                        if (!objCOCInfo.lstTestOid.Contains(obj.Oid))
                                                        {
                                                            objCOCInfo.lstTestOid.Add(obj.Oid);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    ListPropertyEditor lstccd = ((DetailView)View).FindItem("CustomDueDates") as ListPropertyEditor;
                                    if (lstccd != null && lstccd.ListView == null)
                                    {
                                        lstccd.CreateControl();
                                    }
                                    ListView lstcustomduedate = lstccd.ListView;
                                    if (lstcustomduedate != null && lstcustomduedate.CollectionSource.List.Count > 0)
                                    {
                                        List<Guid> lstOid = new List<Guid>();
                                        foreach (CustomDueDate strOid in lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().ToList())
                                        {
                                            if (lstVM.FirstOrDefault(i => i.MatrixName.Oid == strOid.TestMethod.MatrixName.Oid) == null)
                                            {
                                                lstOid.Add(strOid.TestMethod.MatrixName.Oid);
                                            }
                                        }
                                        foreach (CustomDueDate clr in lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().ToList().Where(i => lstOid.Contains(i.TestMethod.MatrixName.Oid)).ToList())
                                        {
                                            lstcustomduedate.CollectionSource.Remove(clr);
                                        }
                                    }
                                    if (objCOCInfo.lstTestOid.Count > 0)
                                    {
                                        foreach (string val in objCOCInfo.lstTestOid.Select(i => i.ToString()))
                                        {
                                            if (!string.IsNullOrEmpty(val))
                                            {
                                                TestMethod objTestMethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", new Guid(val)));
                                                //if (objTestMethod != null && objTask.CustomDueDates.FirstOrDefault(i => i.TestMethod != null && i.TestMethod.Oid == new Guid(val) && i.TestMethod.TestName == objTestMethod.TestName) == null)
                                                if (objTestMethod != null)
                                                {
                                                    if (lstcustomduedate != null)
                                                    {
                                                        var lst = lstVM.Where(i => i.MatrixName.Oid == objTestMethod.MatrixName.Oid);
                                                        if (lst != null && lst.Count() > 0)
                                                        {
                                                            foreach (VisualMatrix objSM in lst.ToList())
                                                            {
                                                                if (objTask.CustomDueDates.FirstOrDefault(i => i.SampleMatrix != null && i.TestMethod.Oid == objTestMethod.Oid && i.SampleMatrix.Oid == objSM.Oid) == null)
                                                                {
                                                                    CustomDueDate objDate = ObjectSpace.CreateObject<CustomDueDate>();
                                                                    objDate.TestMethod = objTestMethod;
                                                                    objDate.COCSettings = objTask;
                                                                    objDate.SampleMatrix = objSM;
                                                                    objDate.Parameter = "AllParam";
                                                                    if (objCOCsettings != null && objCOCsettings.TAT != null)
                                                                    {
                                                                        //objDate.DueDate = objsamplecheckin.DueDate;
                                                                        objDate.TAT = objCOCsettings.TAT;
                                                                    }
                                                                    lstcustomduedate.CollectionSource.Add(objDate);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                ListPropertyEditor lstccd = ((DetailView)View).FindItem("CustomDueDates") as ListPropertyEditor;
                                ListView lstcustomduedate = lstccd.ListView;
                                if (lstcustomduedate != null)
                                {
                                    foreach (CustomDueDate clr in lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().ToList())
                                    {
                                        lstcustomduedate.CollectionSource.Remove(clr);
                                    }
                                }
                            }
                        }
                        //else if (e.PropertyName == "QuoteID")
                        //{

                        //    ListPropertyEditor lstItemPrice = ((DetailView)View).FindItem("SCItemCharges") as ListPropertyEditor;
                        //    if (lstItemPrice != null && lstItemPrice.ListView == null)
                        //    {
                        //        lstItemPrice.CreateControl();
                        //    }
                        //    if (lstItemPrice != null && lstItemPrice.ListView != null)
                        //    {
                        //        foreach (COCSettingsItemChargePricing obj in ((ListView)lstItemPrice.ListView).CollectionSource.List.Cast<COCSettingsItemChargePricing>().ToList())
                        //        {
                        //            ((ListView)lstItemPrice.ListView).CollectionSource.Remove(obj);
                        //        }
                        //    }
                        //    IObjectSpace os = lstItemPrice.ListView.ObjectSpace;
                        //    COCSettings objTask = (COCSettings)e.Object;
                        //    if (objTask.QuoteID != null)
                        //    {
                        //        CRMQuotes objQuote = os.GetObjectByKey<CRMQuotes>(objTask.QuoteID.Oid);
                        //        if (objQuote != null && objQuote.QuotesItemChargePrice.Count > 0)
                        //        {
                        //            foreach (QuotesItemChargePrice obj in objQuote.QuotesItemChargePrice.ToList())
                        //            {
                        //                COCSettingsItemChargePricing objNewItem = os.CreateObject<COCSettingsItemChargePricing>();
                        //                objNewItem.ItemPrice = os.GetObjectByKey<ItemChargePricing>(obj.ItemPrice.Oid);
                        //                objNewItem.Qty = obj.Qty;
                        //                objNewItem.UnitPrice = obj.UnitPrice;
                        //                objNewItem.Amount = obj.Amount;
                        //                objNewItem.FinalAmount = obj.FinalAmount;
                        //                objNewItem.Discount = obj.Discount;
                        //                objNewItem.Description = obj.Description;
                        //                objNewItem.NpUnitPrice = obj.NpUnitPrice;
                        //                ((ListView)lstItemPrice.ListView).CollectionSource.Add(objNewItem);
                        //            }
                        //        }
                        //         ((ListView)lstItemPrice.ListView).Refresh();
                        //    }

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
        private void View_Closed(object sender, EventArgs e)
        {
            try
            {
                if (Application != null && Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                {
                    COCSettings cocSettings = (COCSettings)Application.MainWindow.View.CurrentObject;
                    if (cocSettings != null)
                    {
                        int sampleno = Application.MainWindow.View.ObjectSpace.GetObjectsCount(typeof(COCSettingsSamples), CriteriaOperator.Parse("[COCID.Oid] = ?", cocSettings.Oid));

                        if (sampleno == 0)
                        {
                            cocSettings.NoOfSamples = 1;
                        }
                        else
                        {
                            cocSettings.NoOfSamples = Convert.ToUInt16(sampleno);
                        }
                    }
                    Application.MainWindow.View.RefreshDataSource();
                    COCsr.IsSamplePopupClose = true;
                }
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
                if (View.Id == "COCSettingsSampleRegistration")
                {
                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(COCSettings))
                    {
                        // Application.MainWindow.View.ObjectSpace.Refresh();
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
            ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
            if (gridListEditor != null)
            {
                gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
            }
        }
        private void GridView_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    //CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse(""));
                    //if (currentLanguage != null && currentLanguage.Chinese)
                    GridViewContextMenuItem item = null;
                    item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                    {
                        e.Items.Remove(item);
                    }
                    if (objLanguage.strcurlanguage != "En")
                    {
                        e.Items.Add("复制到所有单元格", "CopyToAllCell");
                    }
                    else
                    {
                        e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    }
                    GridViewContextMenuItem Edititem = e.Items.FindByName("EditRow");
                    if (Edititem != null)
                        Edititem.Visible = false;
                    item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";
                    e.Items.Remove(e.Items.FindByText("Edit"));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAndNewAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (ObjectSpace.IsModified)
                {
                    ObjectSpace.CommitChanges();
                }
                Save();
                if (View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                {
                    COCSettings objCOCSettings = (COCSettings)Application.MainWindow.View.CurrentObject;
                    if (!COCsr.isNoOfSampleDisable)
                    {
                        InsertSamplesInCOCSettingsSample();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Save()
        {
            try
            {
                if (View != null && View.CurrentObject != null && View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                {
                    //ListPropertyEditor liDueDate = ((DetailView)View).FindItem("CustomDueDates") as ListPropertyEditor;
                    //if (liDueDate != null && liDueDate.ListView != null)
                    //{
                    //    ASPxGridListEditor editor = (liDueDate.ListView).Editor as ASPxGridListEditor;
                    //    if (editor != null && editor.Grid != null)
                    //    {
                    //        editor.Grid.UpdateEdit();
                    //    }
                    //}
                    COCSettings objCOCsettings = (COCSettings)View.CurrentObject;
                    //HttpContext.Current.Session["Test"] = objCOCsettings.NPTest;
                    //if (objCOCInfo.lstTestOid != null && objCOCInfo.lstTestOid.Count > 0)
                    //{
                    //    objCOCInfo.lstTestOid.Clear();
                    //}
                    //if (HttpContext.Current.Session["Test"] != null)
                    //{
                    //    string[] TestOid = HttpContext.Current.Session["Test"].ToString().Split(new string[] { ";" }, StringSplitOptions.None);

                    //    //if (TestOid != null && TestOid.Count() > 0)
                    //    //{
                    //    //    foreach (string strTest in TestOid)
                    //    //    {
                    //    //        List<string> lstTestName = strTest.Split('_').ToList();
                    //    //        if (lstTestName.Count == 2)
                    //    //        {
                    //    //            IList<TestMethod> lstTests = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [MethodName.MethodNumber] = ?", lstTestName[0], lstTestName[1]));
                    //    //            if (objCOCInfo.lstTestOid == null)
                    //    //            {
                    //    //                objCOCInfo.lstTestOid = new List<Guid>();
                    //    //                foreach (TestMethod obj in lstTests.ToList())
                    //    //                {
                    //    //                    if (!objCOCInfo.lstTestOid.Contains(obj.Oid))
                    //    //                    {
                    //    //                        objCOCInfo.lstTestOid.Add(obj.Oid);
                    //    //                    }
                    //    //                }
                    //    //            }
                    //    //            else
                    //    //            {
                    //    //                foreach (TestMethod obj in lstTests.ToList())
                    //    //                {
                    //    //                    if (!objCOCInfo.lstTestOid.Contains(obj.Oid))
                    //    //                    {
                    //    //                        objCOCInfo.lstTestOid.Add(obj.Oid);
                    //    //                    }
                    //    //                }
                    //    //            }
                    //    //        }
                    //    //        else if (lstTestName.Count == 1)
                    //    //        {
                    //    //            IList<TestMethod> lstTests = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [IsGroup] = true", lstTestName[0]));
                    //    //            if (objCOCInfo.lstTestOid == null)
                    //    //            {
                    //    //                objCOCInfo.lstTestOid = new List<Guid>();
                    //    //                foreach (TestMethod obj in lstTests.ToList())
                    //    //                {
                    //    //                    if (!objCOCInfo.lstTestOid.Contains(obj.Oid))
                    //    //                    {
                    //    //                        objCOCInfo.lstTestOid.Add(obj.Oid);
                    //    //                    }
                    //    //                }
                    //    //            }
                    //    //            else
                    //    //            {
                    //    //                foreach (TestMethod obj in lstTests.ToList())
                    //    //                {
                    //    //                    if (!objCOCInfo.lstTestOid.Contains(obj.Oid))
                    //    //                    {
                    //    //                        objCOCInfo.lstTestOid.Add(obj.Oid);
                    //    //                    }
                    //    //                }
                    //    //            }
                    //    //        }
                    //    //    }                            
                    //    //}
                    //    //if (objCOCInfo.lstTestOid != null && objCOCInfo.lstTestOid.Count > 0)
                    //    //{
                    //    //    foreach (string val in objCOCInfo.lstTestOid.Select(i => i.ToString()).ToList())
                    //    //    {
                    //    //        if (!string.IsNullOrEmpty(val))
                    //    //        {
                    //    //            TestMethod objTestMethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", new Guid(val)));
                    //    //            if (objTestMethod != null && objCOCsettings.CustomDueDates.FirstOrDefault(i => i.TestMethod != null && i.TestMethod.Oid == new Guid(val) && i.TestMethod.TestName == objTestMethod.TestName) == null)
                    //    //            {
                    //    //                CustomDueDate objDate = ObjectSpace.CreateObject<CustomDueDate>();
                    //    //                objDate.TestMethod = objTestMethod;
                    //    //                objDate.COCSettings = objCOCsettings;
                    //    //                objDate.Parameter = "AllParam";
                    //    //            }
                    //    //        }
                    //    //    }
                    //    //}
                    //    //ObjectSpace.CommitChanges();
                    //    //IList<CustomDueDate> lstDuedate = ObjectSpace.GetObjects<CustomDueDate>(CriteriaOperator.Parse("[COCSettings.Oid]=?", objCOCsettings.Oid));
                    //    //if (lstDuedate.Count != TestOid.Count())
                    //    //{
                    //    //    bool isTrue = false;
                    //    //    foreach (CustomDueDate obj in lstDuedate.ToList())
                    //    //    {
                    //    //        foreach (string val in TestOid)
                    //    //        {
                    //    //            List<String> lstTestOid = val.Split(',').ToList();
                    //    //            if (lstTestOid.Count == 2)
                    //    //            {
                    //    //                TestMethod objTestMethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [MethodName.MethodNumber] = ?", lstTestOid[0], lstTestOid[1]));
                    //    //                if (!string.IsNullOrEmpty(val) && obj.TestMethod != null && objTestMethod != null && obj.TestMethod.Oid == objTestMethod.Oid)
                    //    //                {
                    //    //                    isTrue = true;
                    //    //                }
                    //    //            }
                    //    //            else if (lstTestOid.Count == 1)
                    //    //            {
                    //    //                TestMethod objTestMethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [IsGroup]= true", lstTestOid[0]));
                    //    //                if (!string.IsNullOrEmpty(val) && obj.TestMethod != null && objTestMethod != null && obj.TestMethod.Oid == objTestMethod.Oid)
                    //    //                {
                    //    //                    isTrue = true;
                    //    //                }
                    //    //            }
                    //    //        }
                    //    //        if (!isTrue)
                    //    //        {
                    //    //            ObjectSpace.Delete(obj);
                    //    //            ObjectSpace.CommitChanges();
                    //    //        }
                    //    //        isTrue = false;
                    //    //    }
                    //    //}
                    //}                                    

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
                if (View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                {
                    if (ObjectSpace.IsModified)
                    {
                        ObjectSpace.CommitChanges();
                    }
                    Save();
                    if (View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                    {
                        COCSettings objCOCSettings = (COCSettings)Application.MainWindow.View.CurrentObject;
                        if (!COCsr.isNoOfSampleDisable)
                        {
                            InsertSamplesInCOCSettingsSample();
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
        private void SaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                {
                    Save();
                    if (View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                    {
                        COCSettings objCOCSettings = (COCSettings)Application.MainWindow.View.CurrentObject;
                        if (!COCsr.isNoOfSampleDisable)
                        {
                            InsertSamplesInCOCSettingsSample();
                            foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "NoOfSamples"))
                            {
                                if (item.GetType() == typeof(ASPxIntPropertyEditor))
                                {
                                    ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        List<COCSettingsSamples> lstSamples = View.ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("COCID.Oid=?", objCOCSettings.Oid)).ToList();
                                        if (lstSamples.Count > 0)
                                        {
                                            propertyEditor.Editor.ForeColor = Color.Black;
                                            propertyEditor.Editor.BackColor = Color.White;
                                            propertyEditor.AllowEdit.SetItemValue("stat", false);
                                            COCsr.isNoOfSampleDisable = true;
                                        }


                                    }
                                }
                            }
                        }
                        int sampleno = View.ObjectSpace.GetObjectsCount(typeof(COCSettingsSamples), CriteriaOperator.Parse("[COCID.Oid] = ?", objCOCSettings.Oid));
                        Sample.Caption = "Samples" + "(" + sampleno + ")";
                        this.Actions["btnCOCSampleTest"].Caption = "Tests" + "(" + View.ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[COCSettingsSamples.COCID.Oid] = ?", objCOCSettings.Oid)).Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count() + ")";
                        this.Actions["btnCOCBottleAllocation"].Caption = "Containers" + "(" + View.ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("[COCID.Oid] = ? And [GCRecord] Is Null", objCOCSettings.Oid)).Sum(i => i.Qty) + ")";
                        Application.MainWindow.View.ObjectSpace.Refresh();
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
            // Access and customize the target View control.
            if (View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
            {
                if (View is DetailView)
                {
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {

                        if (item.Id == "ProjectCategory")
                        {
                            ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                        }
                    }

                }
                COCSettings objCOCreg = (COCSettings)View.CurrentObject;
                ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("ProjectID") as ASPxGridLookupPropertyEditor;
                if (propertyEditor != null && propertyEditor.CollectionSource != null)
                {
                    if (objCOCreg != null && objCOCreg.ClientName != null)
                    {
                        propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("[customername.Oid] = ? ", objCOCreg.ClientName.Oid);
                    }
                    else
                    {
                        propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                    }
                }
                ASPxGridLookupPropertyEditor SCpropertyEditor = ((DetailView)View).FindItem("SampleCategory") as ASPxGridLookupPropertyEditor;
                if (propertyEditor != null && propertyEditor.CollectionSource != null)
                {
                    if (objCOCreg != null && objCOCreg.ClientName != null)
                    {
                        propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("[customername.Oid] = ? ", objCOCreg.ClientName.Oid);
                    }
                    else
                    {
                        propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                    }
                }
                List<COCSettingsSamples> lstSamples = View.ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("COCID.Oid=?", objCOCreg.Oid)).ToList();
                if (lstSamples.Count > 0)
                {
                    COCsr.isNoOfSampleDisable = true;
                }
                else
                {
                    COCsr.isNoOfSampleDisable = false;
                    if (COCsr.IsSamplePopupClose)
                    {
                        ASPxIntPropertyEditor noOfSamplespropertyEditor = ((DetailView)View).FindItem("NoOfSamples") as ASPxIntPropertyEditor;
                        if (noOfSamplespropertyEditor != null)
                        {
                            COCSettings cocobj = (COCSettings)noOfSamplespropertyEditor.CurrentObject;
                            if (cocobj != null)
                            {
                                cocobj.NoOfSamples = 1;
                            }
                            //noOfSamplespropertyEditor.CurrentObject = Convert.ToUInt16(1);
                            Application.MainWindow.View.Refresh();
                        }
                        //COCsr.IsSamplePopupClose = false;
                    }
                }
            }
            if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration")
            {
                tempviewinfo.strtempviewid = View.Id;
                ((ASPxGridListEditor)((ListView)View).Editor).Grid.BatchUpdate += Grid_BatchUpdate;
                ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[COCID.COC_ID] = ? AND [COCID.GCRecord] is NULL", COCsr.strCOCID);
                COCSettings objCOCsmpl = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("[Oid] = ?", COCsr.CurrentCOC.Oid));
                string[] strvmarr = objCOCsmpl.SampleMatries.Split(';');
                COCsr.lstCOCvisualmat = new List<VisualMatrix>();
                foreach (string strvmoid in strvmarr.ToList())
                {
                    VisualMatrix lstvmatobj = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strvmoid)));
                    if (lstvmatobj != null)
                    {
                        COCsr.lstCOCvisualmat.Add(lstvmatobj);
                    }
                }
                Frame.GetController<COCSettingsViewController>().Actions["COCSR_SLListViewEdit"].Active.SetItemValue("", false);
                ICallbackManagerHolder sampleid = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                sampleid.CallbackManager.RegisterHandler("id", this);
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                gridListEditor.Grid.JSProperties["cpsuboutremove"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Suboutremove");
                gridListEditor.Grid.JSProperties["cpcollectdatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SLCollectdate");
                gridListEditor.Grid.JSProperties["cpReceiveddatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SLReceiveddate");
                gridListEditor.Grid.JSProperties["cpCollecteddateTimemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SLCollectdateTime");
                gridListEditor.Grid.JSProperties["cpReceiveddateTimemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SLReceiveddateTime");
                if (objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit)
                {
                    gridListEditor.Grid.JSProperties["cpviewid"] = "ViewEditMode_Edit";
                }
                else
                {
                    gridListEditor.Grid.JSProperties["cpviewid"] = "ViewEditMode_View";
                }
                gridListEditor.Grid.HtmlCommandCellPrepared += Grid_HtmlCommandCellPrepared;
                gridListEditor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                gridListEditor.Grid.Load += Grid_Load;
                gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;

                if (COCsr.EditColumnName == null)
                {
                    COCsr.EditColumnName = new List<string>();
                    foreach (ColumnWrapper wrapper in gridListEditor.Columns)
                    {
                        IModelColumn columnModel = ((ListView)View).Model.Columns[wrapper.PropertyName];
                        if (columnModel != null && columnModel.AllowEdit == true && !COCsr.EditColumnName.Contains(columnModel.Id + ".Oid") && columnModel.PropertyEditorType == typeof(ASPxLookupPropertyEditor))
                        {
                            COCsr.EditColumnName.Add(columnModel.Id + ".Oid");
                        }
                        else if (columnModel != null && columnModel.AllowEdit == true && !COCsr.EditColumnName.Contains(columnModel.Id) && columnModel.PropertyEditorType != typeof(ASPxLookupPropertyEditor))
                        {
                            COCsr.EditColumnName.Add(columnModel.Id);
                        }
                    }
                }
                if (COCsr.EditColumnName.Count > 0)
                {
                    gridListEditor.Grid.JSProperties["cpeditcolumnname"] = COCsr.EditColumnName;
                }
                if (COCsr.IsTestAssignmentClosed)
                {
                    COCsr.IsTestAssignmentClosed = false;
                }
                if (AddSample != null)
                {
                    AddSample.Active["showAddSample"] = objPermissionInfo.COCSettingsIsWrite;
                }
                if (objPermissionInfo.COCSettingsIsWrite == false)
                {
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e){
                e.cancel = true;
                 
                }";
                }
                else
                {
                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                        {
                            var fieldName = e.cellInfo.column.fieldName;
                            sessionStorage.setItem('SampleRegistrationFocusedColumn', fieldName);
                            sessionStorage.setItem('CanChangeVisualMatrix', '');
                            s.GetRowValues(e.cellInfo.rowVisibleIndex, 'CanChangeVisualMatrix',function GetVisualMatrixChange(value) 
                            {
                                sessionStorage.setItem('CanChangeVisualMatrix', value);
                            });                            
                        }";
                    //gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e)
                    //    {
                    //        if(s.cpviewid == 'ViewEditMode_View')
                    //        {
                    //             e.cancel = true;
                    //        }
                    //        else
                    //        {
                    //            if(e.focusedColumn.fieldName == 'VisualMatrix.Oid' && s.batchEditApi.GetCellValue(e.visibleIndex, 'VisualMatrix.Oid') != null)
                    //            {
                    //                var val = sessionStorage.getItem('CanChangeVisualMatrix');
                    //                if (val == 'CannotChange')
                    //                {
                    //                    e.cancel = true;
                    //                }
                    //                else if(val == 'DeleteSampleParamsAndChange')
                    //                {
                    //                    s.GetRowValues(e.visibleIndex, 'Test;Oid', OnGetRowValues);
                    //                }
                    //            }
                    //            else if(e.focusedColumn.fieldName == 'SampleID')
                    //            {
                    //                e.cancel = true;
                    //            }
                    //        }


                    //    }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) 
                                                {
                                                    s.timerHandle = setTimeout(function() 
                                                    {
                                                     var timestart = s.batchEditApi.GetCellValue(e.visibleIndex, 'TimeStart');
                                                     var timeend = s.batchEditApi.GetCellValue(e.visibleIndex, 'TimeEnd');
                                                     if(timestart != null && timeend != null)
                                                     {
                                                        if (timestart > timeend) {
                                                            alert('The TimeStart must be less than the TimeEnd.');
                                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'TimeStart', null);
                                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'TimeEnd', null);
                                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Time',null); 
                                                         }
                                                      }
                             var time =0;
                             var datecollected = s.batchEditApi.GetCellValue(i, 'CollectDate',false);
                             var datereceived = s.batchEditApi.GetCellValue(i, 'JobID.RecievedDate',false);
                             var dt = new Date();
                             
                             if(dt != null && datecollected != null && datereceived != null)
                             {
                                var collectedYear = datecollected.getFullYear();
                                var collectedMonth = datecollected.getMonth();
                                var collectedDay = datecollected.getDate();
                                var collectedHours = datecollected.getHours();
                                var collectedMinutes = datecollected.getMinutes();
                                var receivedYear = datereceived.getFullYear();
                               var receivedMonth = datereceived.getMonth();
                               var receivedDay = datereceived.getDate();
                               var receivedHours = datereceived.getHours();
                               var receivedMinutes = datereceived.getMinutes();                                 
                                 if(collectedYear != null && receivedYear != null && collectedMonth != null && receivedMonth != null && collectedDay != null && receivedDay != null 
                                    && collectedHours != null && receivedHours != null && collectedMinutes != null && receivedMinutes != null)
                                {
                                    if (collectedYear > receivedYear ||(collectedYear === receivedYear && 
                                        (collectedMonth > receivedMonth || (collectedMonth === receivedMonth &&  
                                        (collectedDay > receivedDay || (collectedDay === receivedDay && 
                                        (collectedHours > receivedHours ||  (collectedHours === receivedHours && 
                                            collectedMinutes >= receivedMinutes))))))))
                                    {
                                       alert(s.cpReceiveddatemsg);
                                       s.batchEditApi.SetCellValue(i, 'CollectDate', null); 
                                    }
                                }
                             }
                             if(timestart != null && timeend != null)
                             {
                                 if (timestart < timeend) 
                                 {
                                     time =  (timeend-timestart)/1000;
                                     time = (time/60);

                                     s.batchEditApi.SetCellValue(e.visibleIndex, 'Time',time); 
                                  }
                              }
                                                        var flowrate = s.batchEditApi.GetCellValue(e.visibleIndex, 'FlowRate');
                                                        var timemin = s.batchEditApi.GetCellValue(e.visibleIndex, 'Time');
                                                        var vol = 0;
                                                        if(flowrate != null && flowrate > 0 && timemin != null &&timemin > 0)
                                                        {
                                                            vol = flowrate * timemin;
                                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Volume',vol); 
                                                        }
                                                        else if(flowrate != null && flowrate < 1 || timemin != null && timemin < 1)
                                                        {
                                                       //alert('0');
                                                       var volum = '';
                                                       //alert(volum)
                                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Volume', volum); 
                                                        }
                                                                                     var val = s.batchEditApi.GetCellValue(e.visibleIndex, 'Containers');
                                                                                                              if(val == 0 )
                                                                                                              {
                                                                                                              alert('#Containers must not be in 0');
                                                                                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Containers',1); 
                                                                                                              }
                                                                                     var val = s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty');
                                                                                                              if(val == 0 )
                                                                                                              {
                                                                                                              alert('Qty must not be in 0');
                                                                                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Qty',1); 
                                                                                                              }
                                                        //s.UpdateEdit();
                                                    }, 100); 
                                                }";
                }
                gridListEditor.Grid.ClientSideEvents.BatchEditChangesSaving = @"function(s, e)
                    {
                        for (var i in e.updatedValues) 
                        { 
                             var datecollected = s.batchEditApi.GetCellValue(i, 'CollectDate',false);
                             var datereceived = s.batchEditApi.GetCellValue(i, 'JobID.RecievedDate',false);
                             var dt = new Date();
                             
                             if(dt != null && datecollected != null && datereceived != null)
                             {
                                var collectedYear = datecollected.getFullYear();
                                var collectedMonth = datecollected.getMonth();
                                var collectedDay = datecollected.getDate();
                                var collectedHours = datecollected.getHours();
                                var collectedMinutes = datecollected.getMinutes();
                                var receivedYear = datereceived.getFullYear();
                               var receivedMonth = datereceived.getMonth();
                               var receivedDay = datereceived.getDate();
                               var receivedHours = datereceived.getHours();
                               var receivedMinutes = datereceived.getMinutes();
                                 
                                 if(collectedYear != null && receivedYear != null && collectedMonth != null && receivedMonth != null && collectedDay != null && receivedDay != null 
                                    && collectedHours != null && receivedHours != null && collectedMinutes != null && receivedMinutes != null)
                                {
                                    if (collectedYear > receivedYear ||(collectedYear === receivedYear && 
                                        (collectedMonth > receivedMonth || (collectedMonth === receivedMonth &&  
                                        (collectedDay > receivedDay || (collectedDay === receivedDay && 
                                        (collectedHours > receivedHours ||  (collectedHours === receivedHours && 
                                            collectedMinutes >= receivedMinutes))))))))
                                    {
                                       alert(s.cpReceiveddatemsg);
                                       s.batchEditApi.SetCellValue(i, 'CollectDate', null); 
                                    }
                                }
                             }
                             
                        }  
                    }";
                gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function (s, e) {
                        if(e.cellInfo.column.fieldName=='FlowRate')
                          {
                             s.GetEditor('FlowRate').KeyPress.AddHandler(OnSLFlowRateTimeChanged);
                          }
                       if(e.cellInfo.column.fieldName=='Time')
                          {
                           s.GetEditor('Time').KeyPress.AddHandler(OnSLFlowRateTimeChanged);
                          }
                       if(e.cellInfo.column.fieldName=='Volume')
                          {
                             s.GetEditor('Volume').KeyPress.AddHandler(OnSLFlowRateTimeChanged);
                          }
                           sessionStorage.setItem('SampleRegistrationCopyFocusedColumn', null);  
                           if((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit'))
                            {  
                              e.cancel = true;
                            }                  
                            else
                             {
                                 
                                   if (s.cpeditcolumnname.includes(e.cellInfo.column.fieldName))
                                    {
                                     var fieldName = e.cellInfo.column.fieldName; 
                                    sessionStorage.setItem('SampleRegistrationCopyFocusedColumn', fieldName); 
                                    }
                                    else
                                     {
                                          e.cancel=true;
                                     }
                             } 
                        }";

                if (objPermissionInfo.COCSettingsIsWrite)
                {
                    gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                { 
                                    var FocusedColumn = sessionStorage.getItem('SampleRegistrationCopyFocusedColumn');  
                                    var text;
                                    if(FocusedColumn.includes('.'))
                                    {  
                                        oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                        text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                        if (e.item.name =='CopyToAllCell')
                                        {
                                            if(FocusedColumn=='StationLocation.Oid')
                                            {
                                              var address= s.batchEditApi.GetCellValue(e.elementIndex,'Address');
                                              var pwsid= s.batchEditApi.GetCellValue(e.elementIndex,'PWSID');
                                              var KeyMap= s.batchEditApi.GetCellValue(e.elementIndex,'KeyMap');
                                              var SamplePointID= s.batchEditApi.GetCellValue(e.elementIndex,'SamplePointID');
                                              var SamplePointType= s.batchEditApi.GetCellValue(e.elementIndex,'SamplePointType');
                                               var SystemTypeOid= s.batchEditApi.GetCellValue(e.elementIndex,'SystemType.Oid',false);
                                             if (SystemTypeOid!=null)
	                                          {
		                                         var SystemTypetext = s.batchEditApi.GetCellTextContainer(e.elementIndex,'SystemType.Oid').innerText;  
	                                          }
                                                var PWSSystemNameOid= s.batchEditApi.GetCellValue(e.elementIndex,'PWSSystemName.Oid',false);
                                               if(PWSSystemNameOid!=null)
                                                {
                                                     var PWSSystemNametext = s.batchEditApi.GetCellTextContainer(e.elementIndex,'PWSSystemName.Oid').innerText;  
                                                }
                                              var RejectionCriteria= s.batchEditApi.GetCellValue(e.elementIndex,'RejectionCriteria');
                                              var WaterTypeOid = s.batchEditApi.GetCellValue(e.elementIndex,'WaterType.Oid',false);
                                              if (WaterTypeOid!=null)
                                               	{
		                                           var WaterTypetext = s.batchEditApi.GetCellTextContainer(e.elementIndex,'WaterType.Oid').innerText;  
	                                            }
                                              var ParentSampleID= s.batchEditApi.GetCellValue(e.elementIndex,'ParentSampleID');
                                              var ParentSampleDate= s.batchEditApi.GetCellValue(e.elementIndex,'ParentSampleDate');
                                            for(var i = 0; i < s.cpVisibleRowCount; i++)
                                                 { 
                                                if (s.IsRowSelectedOnPage(i)) 
                                                  {
                                                       s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                                        s.batchEditApi.SetCellValue(i,'Address',address); 
                                                       s.batchEditApi.SetCellValue(i,'PWSID',pwsid); 
                                                       s.batchEditApi.SetCellValue(i,'KeyMap',KeyMap);
                                                       s.batchEditApi.SetCellValue(i,'SamplePointID',SamplePointID);
                                                       s.batchEditApi.SetCellValue(i,'SamplePointType',SamplePointType);
                                                       s.batchEditApi.SetCellValue(i,'SystemType.Oid',SystemTypeOid,SystemTypetext,false);
                                                       s.batchEditApi.SetCellValue(i,'PWSSystemName.Oid',PWSSystemNameOid,PWSSystemNametext,false);
                                                       s.batchEditApi.SetCellValue(i,'RejectionCriteria',RejectionCriteria);
                                                       s.batchEditApi.SetCellValue(i,'WaterType.Oid',WaterTypeOid,WaterTypetext,false);
                                                       s.batchEditApi.SetCellValue(i,'ParentSampleID',ParentSampleID);
                                                       s.batchEditApi.SetCellValue(i,'ParentSampleDate',ParentSampleDate);
                                                   }
                                                }
                                            }
                                            else
                                            {
                                            for(var i = 0; i < s.cpVisibleRowCount; i++)
                                            { 
                                                if (s.IsRowSelectedOnPage(i)) 
                                                {                                               
                                                    s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                                }
                                            }
                                        }        
                                            
                                        }        
                                    }
                                else{
                                var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                if (e.item.name =='CopyToAllCell')
                                {
                                    for(var i = 0; i < s.cpVisibleRowCount; i++)
                                    { 
                                        if (s.IsRowSelectedOnPage(i)) 
                                        {
                                            s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                            
                                        }
                                    }
                                }    
                             }
                                }
                                e.processOnServer = false;
                            }";
                    //gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                    //        { 
                    //            if (s.IsRowSelectedOnPage(e.elementIndex))  
                    //            { 
                    //                var FocusedColumn = sessionStorage.getItem('SampleRegistrationCopyFocusedColumn');  
                    //                var text;
                    //                if(FocusedColumn.includes('.'))
                    //                {  
                    //                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                    //                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                    //                    if (e.item.name =='CopyToAllCell')
                    //                    {
                    //                        for(var i = 0; i < s.cpVisibleRowCount; i++)
                    //                        { 
                    //                            if (s.IsRowSelectedOnPage(i)) 
                    //                            {                                               
                    //                                s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                    //                            }
                    //                        }
                    //                    }        
                    //                }
                    //            else{
                    //            var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                    //            if (e.item.name =='CopyToAllCell')
                    //            {
                    //                for(var i = 0; i < s.cpVisibleRowCount; i++)
                    //                { 
                    //                    if (s.IsRowSelectedOnPage(i)) 
                    //                    {
                    //                        s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);

                    //                    }
                    //                }
                    //            }    
                    //         }
                    //            }
                    //            e.processOnServer = false;
                    //        }";
                    if (COCsr.canGridRefresh == true)
                    {

                        COCsr.canGridRefresh = false;
                    }
                }
                CriteriaOperator cs = CriteriaOperator.Parse("COC_ID=?", COCsr.strCOCID);
                Modules.BusinessObjects.Setting.COCSettings objCOCsample = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.COCSettings>(cs);
                if (gridListEditor != null && objCOCsample != null)
                {
                    List<SampleMatrixSetupFields> lstFields = new List<SampleMatrixSetupFields>();
                    if (!string.IsNullOrEmpty(objCOCsample.SampleMatries))
                    {
                        List<string> lstSMOid = objCOCsample.SampleMatries.Split(';').ToList();
                        foreach (string strOid in lstSMOid)
                        {
                            VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strOid.Trim()));
                            if (objVM != null && objVM.SetupFields.Count > 0)
                            {
                                foreach (SampleMatrixSetupFields objField in objVM.SetupFields)
                                {
                                    if (lstFields.FirstOrDefault(i => i.Oid == objField.Oid) == null)
                                    {
                                        lstFields.Add(objField);
                                    }
                                }
                            }
                        }
                    }
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        ASPxGridView gridView = (ASPxGridView)gridListEditor.Grid;
                        if (gridView != null)
                        {
                            gridView.Settings.UseFixedTableLayout = true;
                            gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                            gridView.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                            foreach (WebColumnBase column in gridView.Columns)
                            {
                                if (column.Name == "SelectionCommandColumn" || column.Name == "Test" || column.Name == "Containers")
                                {
                                    //gridView.VisibleColumns[column.Name].FixedStyle = GridViewColumnFixedStyle.Left;
                                    //column.Width = 5;
                                }
                                else
                                {
                                    IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                    if (columnInfo != null)
                                    {
                                        SampleMatrixSetupFields curField = lstFields.FirstOrDefault(i => i.FieldID == columnInfo.Model.Id);
                                        if (curField != null)
                                        {
                                            if (columnInfo.Model.Id == "Containers" || columnInfo.Model.Id == "BottleQty")
                                            {
                                                column.Visible = false;
                                            }
                                            else
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
                                                    //column.VisibleIndex = curField.SortOrder + 3;
                                                    column.SetColVisibleIndex(curField.SortOrder + 3);
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
                                        }
                                        else
                                        {
                                            if (lstFields.Count==0)
                                            {
                                            if (columnInfo.Model.Id == "SampleID" || columnInfo.Model.Id == "SampleName" || columnInfo.Model.Id == "VisualMatrix"
                                               || columnInfo.Model.Id == "ClientSampleID" || columnInfo.Model.Id == "StationLocation")
                                            {
                                                column.Visible = true;
                                            }
                                            else if (columnInfo.Model.Id == "RecievedDate" || columnInfo.Model.Id == "CollectDate" || columnInfo.Model.Id == "CollectTimeDisplay"
                                                || columnInfo.Model.Id == "Collector" || columnInfo.Model.Id == "TimeEnd" || columnInfo.Model.Id == "TimeStart" || columnInfo.Model.Id == "Time")
                                            {
                                                column.Visible = false;
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

            }
            else if (base.View != null && base.View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
            {
                COCSettings objCOCreg = (COCSettings)View.CurrentObject;
                if (objCOCreg.NPTest == null)
                {
                    ListPropertyEditor lstccd = ((DetailView)View).FindItem("CustomDueDates") as ListPropertyEditor;
                    ListView lstcustomduedate = lstccd.ListView;
                    if (lstcustomduedate != null && lstcustomduedate.CollectionSource.List.Count > 0)
                    {
                        foreach (CustomDueDate clr in lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().ToList())
                        {
                            lstcustomduedate.CollectionSource.Remove(clr);
                        }
                    }
                }
                if (objPermissionInfo.COCBottleIsWrite == true && ((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    objPermissionInfo.COCBottleIsWrite = false;
                }
                if (objInfo.ClientName == null || objInfo.ClientName.Length == 0)
                {
                    COCSettings objCOCcheckin = (COCSettings)View.CurrentObject;
                    if (objCOCcheckin != null && objCOCcheckin.ClientName != null)
                    {
                        objInfo.ClientName = objCOCcheckin.ClientName.CustomerName;
                    }
                    if (objCOCcheckin != null && objCOCcheckin.ProjectID != null)
                    {
                        objInfo.ProjectName = objCOCcheckin.ProjectID.ProjectName;
                    }
                }
                if (View.CurrentObject != null)
                {
                    COCSettings COCsample = View.ObjectSpace.GetObject((COCSettings)View.CurrentObject);
                    if (COCsample != null)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        int sampleno = View.ObjectSpace.GetObjectsCount(typeof(COCSettingsSamples), CriteriaOperator.Parse("[COCID.Oid] = ?", COCsample.Oid));
                        if (sampleno == 0)
                        {
                            COCsample.NoOfSamples = 1;
                        }
                        else
                        {
                            COCsample.NoOfSamples = Convert.ToUInt16(sampleno);
                        }
                        //COCsample.NoOfSamples = Convert.ToUInt32(sampleno);
                        if (objLanguage.strcurlanguage != "En")
                        {
                            Sample.Caption = "样液样品" + "(" + sampleno + ")";
                        }
                        else
                        {
                            Sample.Caption = "Samples" + "(" + sampleno + ")";
                            this.Actions["btnCOCSampleTest"].Caption = "Tests" + "(" + View.ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[COCSettingsSamples.COCID.Oid] = ?", COCsample.Oid)).Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count() + ")";
                            int finalamt = 0;
                            //DashboardViewItem dv = ((DetailView)View).FindItem("SampleIDs") as DashboardViewItem;
                            //if (dv != null && dv.InnerView != null)
                            //{
                            //    finalamt = Convert.ToInt16(((ListView)dv.InnerView).CollectionSource.List.Cast<COCSettingsSamples>().Sum(i => i.Qty));
                            //    }
                            //this.Actions["btnCOCBottleAllocation"].Caption = "Containers" + "(" + finalamt + ")";
                            this.Actions["btnCOCBottleAllocation"].Caption = "Containers" + "(" + os.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("[COCID.Oid] = ? And [GCRecord] Is Null", COCsample.Oid)).Sum(i => i.Qty) + ")";
                            //Application.MainWindow.View.Refresh();

                        }
                        os.Dispose();
                        //if (objPermissionInfo.SampleRegIsWrite == false)
                        //{
                        //    //btnImportSamples.Active["ShowImport"] = false;
                        //    //btnImportBasicInformationAction.Active["ShowImportBasicInfo"] = false;
                        //}
                        //else
                        //{
                        //    //btnImportSamples.Active["showImport"] = true;
                        //    //btnImportBasicInformationAction.Active["ShowImportBasicInfo"] = true;
                        //    //if (sampleno == 0)
                        //    //{
                        //    //    btnImportSamples.Active["ShowImport"] = true;
                        //    //    COCsr.ImportToNewJob = false;
                        //    //}
                        //    //else if (sampleno == 1)
                        //    //{
                        //    //    COCSettingsSamples samplelogin = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[JobID.Oid] = ?", sample.Oid));
                        //    //    if (samplelogin != null && samplelogin.VisualMatrix != null)
                        //    //    {
                        //    //        btnImportSamples.Active["ShowImport"] = false;
                        //    //    }
                        //    //    else
                        //    //    {
                        //    //        btnImportSamples.Active["ShowImport"] = true;
                        //    //        COCsr.ImportToNewJob = false;
                        //    //    }
                        //    //}
                        //    //else
                        //    //{
                        //    //    btnImportSamples.Active["ShowImport"] = false;
                        //    //}
                        //}
                    }
                    //else
                    //{
                    //    //if (((Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser).Roles.FirstOrDefault(i => i.IsAdministrative) == null && objPermissionInfo.COCBottleIsWrite == false)
                    //    //{
                    //    //    btnImportSamples.Active["ShowImport"] = false;
                    //    //    btnImportBasicInformationAction.Active["ShowImportBasicInfo"] = false;
                    //    //}
                    //    //else
                    //    //{
                    //    //    btnImportSamples.Active["ShowImport"] = true;
                    //    //    COCsr.ImportToNewJob = true;
                    //    //    btnImportBasicInformationAction.Active["ShowImportBasicInfo"] = true;
                    //    //}
                    //    //btnImportSamples.Active["ShowImport"] = true;
                    //}
                    //os.Dispose();
                }
                COCsr.ViewEditMode = (View as DetailView).ViewEditMode;
                ICallbackManagerHolder clause = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                clause.CallbackManager.RegisterHandler("MethodChanged", this);
                foreach (ViewItem item in ((DetailView)View).Items)
                {
                    if (item.GetType() == typeof(ASPxStringPropertyEditor))
                    {
                        ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                        if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.BackColor = Color.LightYellow;
                            ASPxEditBase editor = (ASPxEditBase)propertyEditor.Editor;
                            if (editor != null)
                            {
                                editor.ClientInstanceName = propertyEditor.Model.Id;
                            }
                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }
                    }
                    else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                    {
                        ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                        if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.BackColor = Color.LightYellow;
                            ASPxEditBase editor = (ASPxEditBase)propertyEditor.Editor;
                            if (editor != null)
                            {
                                editor.ClientInstanceName = propertyEditor.Model.Id;
                            }
                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }
                    }
                    else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                    {
                        ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                        if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.BackColor = Color.LightYellow;
                            if (propertyEditor.Control != null)
                            {
                                AssignClientSideLogic(propertyEditor);
                            }
                            //else
                            //{
                            //    propertyEditor.ControlCreated += new EventHandler<EventArgs>(propertyEditor_ControlCreated);
                            //}
                            if (propertyEditor.PropertyName == "TAT")
                            {
                                ASPxGridLookup editor = (ASPxGridLookup)propertyEditor.Editor;
                                if (editor != null)
                                {
                                    editor.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                    editor.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                    editor.GridView.Settings.VerticalScrollableHeight = 200;
                                }
                            }
                            if (propertyEditor.PropertyName == "ClientName")
                            {
                                ASPxGridLookup editor = (ASPxGridLookup)propertyEditor.Editor;
                                if (editor != null)
                                {
                                    editor.GridView.Settings.VerticalScrollableHeight = 270;
                                }
                            }
                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }
                    }
                    else if (item.GetType() == typeof(FileDataPropertyEditor))
                    {
                        FileDataPropertyEditor propertyEditor = item as FileDataPropertyEditor;
                        if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.BackColor = Color.LightYellow;
                        }
                    }
                    else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                    {
                        ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
                        if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.BackColor = Color.LightYellow;
                            ASPxEditBase editor = (ASPxEditBase)propertyEditor.Editor;
                            if (editor != null)
                            {
                                editor.ClientInstanceName = propertyEditor.Model.Id;
                            }
                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }
                    }
                    else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                    {
                        ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                        if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        {
                            if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                            {
                                propertyEditor.FindEdit.Editor.BackColor = Color.LightYellow;

                                ASPxEditBase editor = (ASPxEditBase)propertyEditor.Editor;
                                if (editor != null)
                                {
                                    editor.ClientInstanceName = propertyEditor.Model.Id;
                                }
                            }
                            else if (propertyEditor.DropDownEdit != null)
                            {
                                propertyEditor.DropDownEdit.DropDown.BackColor = Color.LightYellow;
                            }
                            else
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        if (propertyEditor != null && propertyEditor.DropDownEdit != null && propertyEditor.DropDownEdit.DropDown != null)
                        {
                            propertyEditor.DropDownEdit.DropDown.ForeColor = Color.Black;
                        }
                    }
                    else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                    {
                        ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                        if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        {
                            if (propertyEditor.Editor is ASPxEditBase)
                            {
                                ASPxEditBase editor = (ASPxEditBase)propertyEditor.Editor;
                                if (editor != null)
                                {
                                    editor.ClientInstanceName = propertyEditor.Model.Id;
                                }
                            }
                        }
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                            if (propertyEditor.Id == "NoOfSamples")
                            {
                                if (COCsr.isNoOfSampleDisable)
                                {
                                    propertyEditor.AllowEdit.SetItemValue("stat", false);
                                }
                                else
                                {
                                    propertyEditor.AllowEdit.SetItemValue("stat", true);
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                                if ((item as ASPxIntPropertyEditor).Editor != null)
                                    (item as ASPxIntPropertyEditor).Editor.Load += Editor_Load;
                            }
                        }
                    }
                    else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                    {
                        ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                        if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.BackColor = Color.LightYellow;
                        }
                        ASPxGridLookup lookup = (ASPxGridLookup)propertyEditor.Editor;
                        if (lookup != null && propertyEditor.Id == "SampleMatries")
                        {
                            //lookup.GridViewProperties.Settings.ShowFilterRow = true;
                            lookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            lookup.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            lookup.GridView.Settings.VerticalScrollableHeight = 200;
                            lookup.GridViewProperties.SettingsSearchPanel.Visible = true;
                            foreach (WebColumnBase columns in lookup.GridView.VisibleColumns)
                            {
                                if (columns.Index == 1)
                                {
                                    columns.Caption = "Sample Matrices";
                                }
                            }
                        }
                        else if (lookup != null && propertyEditor.Id == "SampleCategory")
                        {
                            foreach (WebColumnBase columns in lookup.GridView.VisibleColumns)
                            {
                                if (columns.Index == 1)
                                {
                                    columns.Caption = "Sample Category";
                                }
                            }
                        }
                    }

                }
                if (Frame != null && Frame is NestedFrame)
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    if (nestedFrame != null && nestedFrame.ViewItem != null)
                    {
                        CompositeView cv = nestedFrame.ViewItem.View;
                        if (cv != null && cv.Id == "SampleRegistration")
                        {
                            Sample.Active["ShowSample"] = false;
                        }
                    }
                }
                else
                {
                    Sample.Active["ShowSample"] = true;
                }
            }
            if (View.Id == "Testparameter_LookupListView_Copy_COCSample_Copy")
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                gridListEditor.Grid.SettingsPager.AlwaysShowPager = true;
                gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                ICallbackManagerHolder seltest = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                seltest.CallbackManager.RegisterHandler("Subout", this);
                string script = seltest.CallbackManager.GetScript();
                script = string.Format(CultureInfo.InvariantCulture, @"
                        function(s, e) {{ 
                            var xafCallback = function() {{
                            s.EndCallback.RemoveHandler(xafCallback);
                            {0}
                            }};
                            s.EndCallback.AddHandler(xafCallback);
                        }}
                    ", script);
                gridListEditor.Grid.ClientSideEvents.SelectionChanged = script;
                gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e)
                    { 
                    s.SetWidth(400); 
                    s.RowClick.ClearHandlers();
                    if(s.cpCanGridRefresh)
                    {
                        s.Refresh();
                        s.cpCanGridRefresh = false;
                    }
                    }";
                gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) 
                                                                                {
                                                                                    var value= s.batchEditApi.GetCellValue(e.visibleIndex,'SubOut');
                                                                                    //alert(value);
                                                                                    if(value == true)
                                                                                    {
                                                                                        RaiseXafCallback(globalCallbackControl, 'Subout', 'SuboutSelected|'+e.visibleIndex, '', false);  
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        RaiseXafCallback(globalCallbackControl, 'Subout', 'SuboutUnSelected|'+e.visibleIndex, '', false);  
                                                                                    }
                                                                                }";

                gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
            }
            if (View.Id == "Testparameter_LookupListView_Copy_COCSample_Copy_Parameter")
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                selparameter.CallbackManager.RegisterHandler("Test", this);
                gridListEditor.Grid.SettingsPager.AlwaysShowPager = true;
                gridListEditor.Grid.CommandButtonInitialize += Grid_CommandButtonInitialize;
                gridListEditor.Grid.Load += Grid_Load;
                gridListEditor.Grid.SettingsBehavior.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
                gridListEditor.Grid.CustomColumnSort += Grid_CustomColumnSort;
                gridListEditor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e){ 
                    s.SetWidth(400); 
                    s.RowClick.ClearHandlers();
                    }";
                gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e){
                      if(e.visibleIndex != '-1')
                      {                       
                        if (s.IsRowSelectedOnPage(e.visibleIndex)) {   
                            var value = 'Testselection|Selected|' + s.GetRowKey(e.visibleIndex);
                            RaiseXafCallback(globalCallbackControl, 'Test', value, '', false);    
                        }else{
                            var value = 'Testselection|UNSelected|' + s.GetRowKey(e.visibleIndex);
                            RaiseXafCallback(globalCallbackControl, 'Test', value, '', false);    
                        }
                     }
                     else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                     {        
                        RaiseXafCallback(globalCallbackControl, 'Test', 'Testselection|Selectall', '', false);                        
                     }   
                     else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                     {
                        RaiseXafCallback(globalCallbackControl, 'Test', 'Testselection|UNSelectall', '', false);                        
                     }
                     else if(e.visibleIndex == '-1' && s.cpFilterRowCount == s.cpVisibleRowCount)
                     {        
                        RaiseXafCallback(globalCallbackControl, 'Test', 'Testselection|Selectall', '', false);                        
                     }   
                     else if(e.visibleIndex == '-1' && s.cpFilterRowCount == 0)
                     {
                        RaiseXafCallback(globalCallbackControl, 'Test', 'Testselection|UNSelectall', '', false);                        
                     }
                    }";
            }
            if (objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.View && View is DetailView)
            {
                AddSample.Active["btnAddSample"] = false;
                TestSelectionAdd.Active["btnAddSample"] = false;
                TestSelectionRemove.Active["btnAddSample"] = false;
                TestSelectionSave.Active["btnAddSample"] = false;
                //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active["ShowDelete"] = false;
            }
            else if (objPermissionInfo.COCSettingsIsWrite)
            {
                AddSample.Active["btnAddSample"] = true;
                TestSelectionAdd.Active["btnAddSample"] = true;
                TestSelectionRemove.Active["btnAddSample"] = true;
                TestSelectionSave.Active["btnAddSample"] = true;
                Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active["ShowDelete"] = true;
            }
            if (View.Id == "COCSettingsTest_ListView_Copy_SampleRegistration")
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (gridListEditor != null && gridListEditor.Grid != null)
                {
                    gridListEditor.Grid.SettingsBehavior.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
                    gridListEditor.Grid.CustomColumnSort += Grid_CustomColumnSort;
                    gridListEditor.Grid.CommandButtonInitialize += Grid_CommandButtonInitialize;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                }
            }
            if (View.Id == "COCTest" && COCsr.IsTestcanFilter)
            {
                COCsr.IsTestcanFilter = false;
                List<object> groups = new List<object>();
                DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
                DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
                UnitOfWork uow = new UnitOfWork(((XPObjectSpace)this.ObjectSpace).Session.DataLayer);
                using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Testparameter)))
                {
                    string criteria = string.Empty;
                    if (COCsr.lstdupfilterstr != null && COCsr.lstdupfilterstr.Count > 0)
                    {
                        foreach (string test in COCsr.lstdupfilterstr)
                        {
                            var testsplit = test.Split('|');
                            if (testsplit.Length == 4)
                            {
                                XPClassInfo TestParameterinfo = uow.GetClassInfo(typeof(Testparameter));
                                IList<Testparameter> testparameters = uow.GetObjects(TestParameterinfo, CriteriaOperator.Parse("([TestMethod.TestName] ='" + testsplit[0] + "' and [TestMethod.MethodName.MethodNumber] ='" + testsplit[1] + "' and [TestMethod.MatrixName.MatrixName] ='" + testsplit[2] + "' and [Component.Components] ='" + testsplit[3] + "') And TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null And TestMethod.MatrixName.GCRecord Is Null And [QCType.QCTypeName] = 'Sample'"), null, int.MaxValue, false, true).Cast<Testparameter>().ToList();
                                if (criteria == string.Empty)
                                {
                                    criteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testparameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                }
                                else
                                {
                                    criteria = criteria + "and Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testparameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                }
                            }
                            else if (testsplit.Length == 3)
                            {
                                XPClassInfo TestParameterinfo = uow.GetClassInfo(typeof(Testparameter));
                                IList<Testparameter> testparameters = uow.GetObjects(TestParameterinfo, CriteriaOperator.Parse("([TestMethod.TestName] ='" + testsplit[0] + "' and [TestMethod.MatrixName.MatrixName] ='" + testsplit[1] + "' and [Component.Components] ='" + testsplit[2] + "') And TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null And TestMethod.MatrixName.GCRecord Is Null And [QCType.QCTypeName] = 'Sample'"), null, int.MaxValue, false, true).Cast<Testparameter>().ToList();
                                if (criteria == string.Empty)
                                {
                                    criteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testparameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                }
                                else
                                {
                                    criteria = criteria + "and Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testparameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(criteria))
                    {
                        lstview.Criteria = new GroupOperator(GroupOperatorType.And,
                                   CriteriaOperator.Parse(criteria)
                                   , CriteriaOperator.Parse("TestMethod.GCRecord Is Null And TestMethod.MatrixName.GCRecord Is Null And (([IsGroup] = False And [TestMethod.MethodName.GCRecord] Is Null And  [QCType.QCTypeName] = 'Sample') Or [IsGroup] = True) "));
                    }
                    else
                    {
                        lstview.Criteria = CriteriaOperator.Parse("TestMethod.GCRecord Is Null And TestMethod.MatrixName.GCRecord Is Null And (([IsGroup] = False And [TestMethod.MethodName.GCRecord] Is Null And  [QCType.QCTypeName] = 'Sample') Or [IsGroup] = True) ");
                    }
                    lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.TestName", true, true));
                    lstview.Properties.Add(new ViewProperty("TMethodName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.MethodName.MethodNumber", true, true));
                    lstview.Properties.Add(new ViewProperty("TMatrixName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.MatrixName.MatrixName", true, true));
                    lstview.Properties.Add(new ViewProperty("TComponentName", DevExpress.Xpo.SortDirection.Ascending, "Component.Components", true, true));
                    lstview.Properties.Add(new ViewProperty("TIsGroup", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.IsGroup", true, true));
                    lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                    foreach (ViewRecord rec in lstview)
                        groups.Add(rec["Toid"]);
                    if (COCsr.lstTestParameter != null && COCsr.lstTestParameter.Count > 0)
                    {
                        if (COCsr.lstdupfilterguid != null && COCsr.lstdupfilterguid.Count > 0)
                        {
                            foreach (Guid guid in COCsr.lstdupfilterguid)
                            {
                                groups.Add(guid);
                            }
                        }
                        ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", COCsr.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", COCsr.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                    }

                    ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"] = new InOperator("Oid", groups);
                    ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["dupfilter"] = new InOperator("Oid", groups);
                    ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                }
                if (objCOCSampleinfo.COCVisualMatrixName != null && staticText != null)
                {
                    staticText.Text = objCOCSampleinfo.COCVisualMatrixName;
                }
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

        private void PopupControl_CustomizePopupWindowSize(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame.View.Id == "ProjectCategory_DetailView")
                {
                    //  e.vi
                    e.Width = 700;
                    e.Height = 400;
                    e.Handled = true;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void TestSelectionAdd_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
                DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
                if (TestViewMain != null && ((ListView)TestViewMain.InnerView).SelectedObjects.Count > 0)
                {
                    List<Guid> lstSubOutTestOid = new List<Guid>();
                    if (COCsr.lstSubOutTest == null)
                    {
                        COCsr.lstSubOutTest = new List<Guid>();
                    }
                    if (COCsr.lstdupfilterSuboutstr == null)
                    {
                        COCsr.lstdupfilterSuboutstr = new List<string>();
                    }
                    COCSettings objCOCID = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("[COC_ID] = ? AND [GCRecord] is NULL", COCsr.strCOCID));

                    COCSettingsSamples objcocSL = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ? AND [GCRecord] is NULL", COCsr.SampleOid));
                    foreach (Testparameter testparameter in ((ListView)TestViewMain.InnerView).SelectedObjects)
                    {
                        if (testparameter.IsGroup == false)
                        {
                            IList<Testparameter> listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodNumber]=? and [TestMethod.MatrixName.MatrixName] = ? and Component.Components=? And TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null And TestMethod.MatrixName.GCRecord Is Null And [QCType.QCTypeName] = 'Sample'", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components));
                            foreach (Testparameter test in listseltest)
                            {
                                if (lstSubOutTestOid != null && lstSubOutTestOid.Contains(test.TestMethod.Oid))
                                {
                                    if (!COCsr.lstSubOutTest.Contains(test.TestMethod.Oid))
                                    {
                                        COCsr.lstSubOutTest.Add(test.TestMethod.Oid);
                                        if (!COCsr.lstdupfilterSuboutstr.Contains(test.TestMethod.TestName + "|" + test.TestMethod.MethodName + "|" + test.TestMethod.MatrixName.MatrixName + "|" + test.Component))
                                        {
                                            COCsr.lstdupfilterSuboutstr.Add(test.TestMethod.TestName + "|" + test.TestMethod.MethodName + "|" + test.TestMethod.MatrixName.MatrixName + "|" + test.Component);
                                        }
                                    }
                                }
                                if (!COCsr.lstTestParameter.Contains(test.Oid))
                                {
                                    COCsr.lstTestParameter.Add(test.Oid);
                                }
                                if (objCOCID != null && objcocSL.SubOut == true)
                                {
                                    if (!COCsr.lstdupfilterSuboutstr.Contains(test.TestMethod.TestName + "|" + test.TestMethod.MethodName.MethodNumber + "|" + test.TestMethod.MatrixName.MatrixName + "|" + test.Component.Components))
                                    {
                                        COCsr.lstdupfilterSuboutstr.Add(test.TestMethod.TestName + "|" + test.TestMethod.MethodName.MethodNumber + "|" + test.TestMethod.MatrixName.MatrixName + "|" + test.Component.Components);
                                    }
                                    COCsr.lstSubOutTest.Add(test.TestMethod.Oid);
                                }
                            }
                        }
                        else
                        {
                            IList<Testparameter> listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MatrixName.MatrixName] = ? ", testparameter.TestMethod.TestName, testparameter.TestMethod.MatrixName.MatrixName));
                            foreach (Testparameter test in listseltest)
                            {
                                if (lstSubOutTestOid != null && lstSubOutTestOid.Contains(test.TestMethod.Oid))
                                {
                                    if (!COCsr.lstSubOutTest.Contains(test.TestMethod.Oid))
                                    {
                                        COCsr.lstSubOutTest.Add(test.TestMethod.Oid);
                                        if (!COCsr.lstdupfilterSuboutstr.Contains(test.TestMethod.TestName + "|" + test.TestMethod.MethodName + "|" + test.TestMethod.MatrixName.MatrixName + "|" + test.Component))
                                        {
                                            COCsr.lstdupfilterSuboutstr.Add(test.TestMethod.TestName + "|" + test.TestMethod.MethodName + "|" + test.TestMethod.MatrixName.MatrixName + "|" + test.Component);
                                        }
                                    }
                                }
                                if (!COCsr.lstTestParameter.Contains(test.Oid))
                                {
                                    COCsr.lstTestParameter.Add(test.Oid);
                                }
                            }
                        }
                    }
                    if (TestViewSub != null && COCsr.lstTestParameter != null && COCsr.lstTestParameter.Count > 0)
                    {
                        ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", COCsr.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", COCsr.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                    ((ASPxGridListEditor)((ListView)TestViewSub.InnerView).Editor).Grid.JSProperties["cpCanGridRefresh"] = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void TestSelectionRemove_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
                DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
                if (TestViewMain != null && TestViewSub != null && ((ListView)TestViewSub.InnerView).SelectedObjects.Count > 0)
                {
                    if (COCsr.lstRemoveTestParameter == null)
                    {
                        COCsr.lstRemoveTestParameter = new List<Guid>();
                    }
                    foreach (Testparameter testparameter in ((ListView)TestViewSub.InnerView).SelectedObjects)
                    {
                        IList<Testparameter> listseltest = new List<Testparameter>();
                        COCSettingsSamples sampleLog = TestViewSub.InnerView.ObjectSpace.GetObjectByKey<COCSettingsSamples>(COCsr.SampleOid);
                        if (sampleLog != null)
                        {
                            IList<COCSettingsTest> lstSample = null;
                            if (testparameter.IsGroup != true)
                            {
                                lstSample = TestViewSub.InnerView.ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[COCSettingsSamples.Oid]=? And [Testparameter.TestMethod.TestName]=? and [Testparameter.TestMethod.MethodName.MethodNumber]=? and [Testparameter.TestMethod.MatrixName.MatrixName] = ? and [Testparameter.Component.Components]=?", sampleLog.Oid, testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components));
                            }
                            else
                            {
                                lstSample = TestViewSub.InnerView.ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[COCSettingsSamples.Oid]=? And [Testparameter.TestMethod.TestName]=?  and [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [IsGroup]=true And [GroupTest.TestMethod.Oid]=? ", sampleLog.Oid, testparameter.TestMethod.TestName, testparameter.TestMethod.MatrixName.MatrixName, testparameter.TestMethod.Oid));
                            }
                            if (lstSample != null && lstSample.Count > 0 || lstSample.Count == 0)
                            {
                                if (testparameter.IsGroup != true)
                                {
                                    listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodNumber]=? and [TestMethod.MatrixName.MatrixName] = ? and [Component.Components]=?", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components));
                                }
                                else
                                {
                                    if (COCsr.lstTestParameter.Contains(testparameter.Oid))
                                    {
                                        COCsr.lstTestParameter.Remove(testparameter.Oid);
                                    }
                                    IList<GroupTestMethod> lstgrouptestmed = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", testparameter.TestMethod.Oid));
                                    foreach (GroupTestMethod objgtm in lstgrouptestmed.ToList())
                                    {
                                        IList<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objgtm.TestParameter.TestMethod.Oid));
                                        foreach (Testparameter paramgtm in lsttestpara.ToList())
                                        {
                                            listseltest.Add(paramgtm);
                                        }
                                    }
                                }
                                foreach (Testparameter test in listseltest)
                                {
                                    if (COCsr.lstTestParameter.Contains(test.Oid))
                                    {
                                        COCsr.lstTestParameter.Remove(test.Oid);
                                    }
                                    if (!COCsr.lstRemoveTestParameter.Contains(test.Oid))
                                    {
                                        COCsr.lstRemoveTestParameter.Add(test.Oid);
                                    }
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "testcannotremove"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }

                        }



                    }
                    if (COCsr.lstTestParameter.Count != 0 && TestViewSubChild != null)
                    {
                        ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", COCsr.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", COCsr.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                    else
                    {
                        ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("");
                        ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void TestSelectionSave_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
                DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                if (COCsr.lstRemoveTestParameter != null && COCsr.lstRemoveTestParameter.Count > 0)
                {
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    COCSettingsSamples objCOCSample = uow.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", COCsr.SampleOid));
                    foreach (Guid objTestParameter in COCsr.lstRemoveTestParameter)
                    {
                        Testparameter param = uow.GetObjectByKey<Testparameter>(objTestParameter);
                        if (COCsr.lstRemoveTestParameter.Contains(objTestParameter) && param != null)
                        {
                            objCOCSample.Testparameters.Remove(param);
                            IObjectSpace os = Application.CreateObjectSpace(typeof(COCSettingsTest));
                            Session currentSessions = ((XPObjectSpace)(os)).Session;
                            UnitOfWork uows = new UnitOfWork(currentSession.DataLayer);
                            COCSettingsTest objsmpltest = uows.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.Oid] = ? And [COCSettingsSamples.Oid]= ?", objTestParameter, objCOCSample.Oid));
                            if (objsmpltest != null)
                            {
                                uows.Delete(objsmpltest);
                                uows.CommitChanges();
                            }
                        }
                    }
                    COCsr.lstRemoveTestParameter.Clear();
                    objCOCSample.Save();
                }
                if (COCsr.lstTestParameter != null && COCsr.lstTestParameter.Count > 0)
                {
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    COCSettingsSamples sampleLog = uow.GetObjectByKey<COCSettingsSamples>(COCsr.SampleOid);

                    foreach (Guid objtestparameter in COCsr.lstTestParameter)
                    {
                        Testparameter param = uow.GetObjectByKey<Testparameter>(objtestparameter);
                        if (param.IsGroup == false)
                        {
                            if (!COCsr.lstSavedTestParameter.Contains(objtestparameter) && param != null && param.QCType != null && param.QCType.QCTypeName == "Sample")
                            {
                                COCSettingsTest objsp = ObjectSpace.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [COCSettingsSamples.Oid] = ?", objtestparameter, sampleLog.Oid));
                                if (objsp == null)
                                {
                                    COCSettingsTest newsample = new COCSettingsTest(uow);
                                    newsample.COCSettingsSamples = sampleLog;
                                    newsample.Testparameter = param;
                                    sampleLog.Test = true;
                                    newsample.Save();
                                }
                            }
                            else
                            {
                                COCSettingsTest sample = uow.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid]=? And [COCSettingsSamples.Oid]=? And [Testparameter.Parameter.Oid] = ?", param.TestMethod.Oid, sampleLog.Oid, param.Parameter.Oid));
                                if (sample != null)
                                {
                                    sample.Save();
                                }
                            }
                        }
                        else if (param.IsGroup == true)
                        {
                            IList<GroupTestMethod> lstgrouptestmed = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", param.TestMethod.Oid));
                            foreach (GroupTestMethod objgtm in lstgrouptestmed.ToList())
                            {
                                IList<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objgtm.TestParameter.TestMethod.Oid));
                                foreach (Testparameter param1 in lsttestpara.ToList())
                                {
                                    COCSettingsTest objsp = ObjectSpace.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [COCSettingsSamples.Oid] = ?", param1.Oid, sampleLog.Oid));
                                    if (objsp == null)
                                    {
                                        COCSettingsTest newsample = new COCSettingsTest(uow);
                                        newsample.COCSettingsSamples = sampleLog;
                                        newsample.Testparameter = uow.GetObjectByKey<Testparameter>(param1.Oid);
                                        sampleLog.Test = true;
                                        newsample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objgtm.Oid);
                                        newsample.IsGroup = true;
                                        newsample.Save();
                                        uow.CommitChanges();
                                    }
                                    else
                                    {
                                        COCSettingsTest sample = uow.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid]=? And [COCSettingsSamples.Oid]=? And [Testparameter.Parameter.Oid] = ?", param1.TestMethod.Oid, sampleLog.Oid, param1.Parameter.Oid));
                                        if (sample != null)
                                        {
                                            sample.Save();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    sampleLog.TestSummary = string.Join("; ", new XPQuery<COCSettingsTest>(uow).Where(i => i.COCSettingsSamples.Oid == sampleLog.Oid && i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.TestName).Distinct().ToList());
                    sampleLog.FieldTestSummary = string.Join("; ", new XPQuery<COCSettingsTest>(uow).Where(i => i.COCSettingsSamples.Oid == sampleLog.Oid && i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsFieldTest == true).Select(i => i.Testparameter.TestMethod.TestName).Distinct().ToList());
                    TestViewMain.InnerView.ObjectSpace.CommitChanges();
                    TestViewSub.InnerView.ObjectSpace.CommitChanges();
                    uow.CommitChanges();
                    AssignBottleAllocationToSamples(uow, COCsr.SampleOid);
                }
                (Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(true);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            try
            {
                if (View.Id == "COCSettingsTest_ListView_Copy_SampleRegistration")
                {
                    if (e.Column != null & e.Column.FieldName == "Parent")
                    {
                        object SampleNo1 = e.GetRow1Value("COCSettingsSamples.SampleNo");
                        object SampleNo2 = e.GetRow2Value("COCSettingsSamples.SampleNo");
                        int res = Comparer.Default.Compare(SampleNo1, SampleNo2);
                        if (res == 0)
                        {
                            object Parent1 = e.Value1;
                            object Parent2 = e.Value2;
                            res = Comparer.Default.Compare(Parent1, Parent2);
                        }
                        e.Result = res;
                        e.Handled = true;
                    }
                }
                else
                if (View.Id == "Testparameter_LookupListView_Copy_COCSample_Copy_Parameter" /*|| View.Id == "Testparameter_ListView_Parameter"*/)
                {
                    ASPxGridView grid = sender as ASPxGridView;
                    bool isRow1Selected = grid.Selection.IsRowSelectedByKey(e.GetRow1Value(grid.KeyFieldName));
                    bool isRow2Selected = grid.Selection.IsRowSelectedByKey(e.GetRow2Value(grid.KeyFieldName));
                    e.Handled = isRow1Selected != isRow2Selected;
                    if (e.Handled)
                    {
                        if (e.SortOrder == DevExpress.Data.ColumnSortOrder.Descending)
                            e.Result = isRow1Selected ? 1 : -1;
                        else
                            e.Result = isRow1Selected ? -1 : 1;
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
                ASPxGridView gridView = sender as ASPxGridView;
                if (View.Id == "Testparameter_LookupListView_Copy_COCSample_Copy")
                {
                    bool chksubout = false;
                    IsDisableCheckBox = false;
                    if (Frame != null && Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                        {
                            CompositeView cv = nestedFrame.ViewItem.View;
                            if (cv != null && cv is DashboardView)
                            {
                                DashboardViewItem SLListView = (DashboardViewItem)cv.FindItem("COCSettingsSample");
                            }
                        }
                    }
                    //Testparameter testparameter = (Testparameter)View.CurrentObject;
                    DashboardViewItem TestViewSubChild = ((NestedFrame)Frame).ViewItem.View.FindItem("TestViewSubChild") as DashboardViewItem;
                    DashboardViewItem TestViewSub = ((NestedFrame)Frame).ViewItem.View.FindItem("TestViewSub") as DashboardViewItem;
                    if (TestViewSub != null && TestViewSubChild != null && COCsr.UseSelchanged)
                    {
                        if (TestViewSub.InnerView.SelectedObjects.Count > 0)
                        {
                            List<Guid> lstTestOid = new List<Guid>();
                            foreach (Testparameter testparameter in TestViewSub.InnerView.SelectedObjects.Cast<Testparameter>().ToList().Where(i => i.IsGroup == false))
                            {
                                IList<Testparameter> listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodNumber]=? and [TestMethod.MatrixName.MatrixName] = ? and Component.Components=? And TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null And TestMethod.MatrixName.GCRecord Is Null And [QCType.QCTypeName] = 'Sample'", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components));
                                foreach (Guid obj in listseltest.ToList().Select(i => i.Oid))
                                {
                                    if (!lstTestOid.Contains(obj))
                                    {
                                        lstTestOid.Add(obj);
                                    }
                                }
                            }
                          ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstTestOid);
                        }
                        else
                        {
                            ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                        }
                        //if (testparameter != null && testparameter.TestMethod != null && testparameter.TestMethod.MethodName != null && !string.IsNullOrEmpty(testparameter.TestMethod.TestName) && testparameter.TestMethod.MatrixName != null && testparameter.Component != null)
                        //{
                        //    ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [TestMethod.MatrixName.MatrixName] = ? And [QCType.QCTypeName] = 'Sample' And [Component.Components]=?", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components);
                        //}

                        COCsr.strSelectionMode = "Selected";
                    }
                    else
                    {
                        COCsr.UseSelchanged = true;
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
                if (COCsr.lstTestParameter != null && COCsr.lstTestParameter.Count > 0 && COCsr.strSelectionMode == "Selected")
                {
                    foreach (Guid obj in COCsr.lstTestParameter)
                    {
                        gridView.Selection.SelectRowByKey(obj);
                    }
                    COCsr.strSelectionMode = string.Empty;
                }
                //if (View != null && View.Id == "COCSettingsSamples_ListView_COCBottle")
                //{
                //    if (((ListView)View).CollectionSource.List.Count == 1)
                //    {
                //        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                //        {
                //            gridView.Selection.SelectRow(i);
                //        }
                //    }
                //    else if (((ListView)View).CollectionSource.List.Count > 1)
                //    {
                //        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                //        {
                //            if (samplingfirstdefault == true)
                //            {
                //                i = 1;
                //                break;
                //                gridView.Selection.SelectRow(i);
                //                samplingfirstdefault = false;
                //            }
                //            else if (!string.IsNullOrEmpty(gridView.GetRowValues(i, "SampleID").ToString()))
                //            {
                //                //string strbottleid = gridView.GetRowValues(i, "SampleID").ToString();
                //                //COCSettingsSamples objsmpling = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.SamplingGuid));
                //                //if (objsmpling != null && objsmpling.SampleID == strbottleid)
                //                //{
                //                //    gridView.Selection.SelectRow(i);
                //                //}
                //            }
                //        }
                //    }
                //    gridView.Selection.SelectRowByKey(COCInfo.SamplingGuid);
                //}
                //else 
                if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration" && !objCOCSampleinfo.IsColumnsCustomized && !string.IsNullOrEmpty(COCsr.strCOCID))
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.UpdateEdit();
                    }
                    CriteriaOperator cs = CriteriaOperator.Parse("COCID=?", COCsr.strCOCID);
                    COCSettings objCOCsample = ObjectSpace.FindObject<COCSettings>(cs);
                    if (gridListEditor != null && objCOCsample != null)
                    {
                        List<SampleMatrixSetupFields> lstFields = new List<SampleMatrixSetupFields>();
                        if (!string.IsNullOrEmpty(objCOCsample.SampleMatries))
                        {
                            List<string> lstSMOid = objCOCsample.SampleMatries.Split(';').ToList();
                            foreach (string strOid in lstSMOid)
                            {
                                VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strOid.Trim()));
                                if (objVM != null && objVM.SetupFields.Count > 0)
                                {
                                    foreach (SampleMatrixSetupFields objField in objVM.SetupFields)
                                    {
                                        if (lstFields.FirstOrDefault(i => i.Oid == objField.Oid) == null)
                                        {
                                            lstFields.Add(objField);
                                        }
                                    }
                                }
                            }
                        }
                        foreach (WebColumnBase column in gridView.Columns)
                        {
                            if (column.Name == "SelectionCommandColumn" || column.Name == "Test" || column.Name == "Containers")
                            {

                            }
                            else
                            {
                                IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                if (columnInfo != null)
                                {
                                    SampleMatrixSetupFields curField = lstFields.FirstOrDefault(i => i.FieldID == columnInfo.Model.Id);
                                    if (curField != null)
                                    {
                                        if (columnInfo.Model.Id == "Containers" || columnInfo.Model.Id == "BottleQty")
                                        {
                                            column.Visible = false;
                                        }
                                        else
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
                                                column.VisibleIndex = curField.SortOrder + 3;
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
                                    }
                                    else
                                    {
                                        if (columnInfo.Model.Id == "SampleID" || columnInfo.Model.Id == "SampleName" || columnInfo.Model.Id == "VisualMatrix"
                                               || columnInfo.Model.Id == "ClientSampleID" || columnInfo.Model.Id == "SamplingLocation")
                                        {
                                            column.Visible = true;
                                        }
                                        else if (columnInfo.Model.Id == "RecievedDate" || columnInfo.Model.Id == "CollectDate" || columnInfo.Model.Id == "CollectTimeDisplay"
                                            || columnInfo.Model.Id == "Collector" || columnInfo.Model.Id == "TimeEnd" || columnInfo.Model.Id == "TimeStart" || columnInfo.Model.Id == "Time")
                                        {
                                            column.Visible = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    objCOCSampleinfo.IsColumnsCustomized = true;
                }
                if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration")
                {
                    if (gridView.Columns["SelectionCommandColumn"] != null)
                    {
                        gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.Columns["Test"] != null)
                    {
                        gridView.VisibleColumns["COCTestbtn"].FixedStyle = GridViewColumnFixedStyle.Left;
                        gridView.VisibleColumns["COCTestbtn"].Width = 60;
                    }
                    if (gridView.Columns["Containers"] != null)
                    {
                        gridView.VisibleColumns["Containers1"].FixedStyle = GridViewColumnFixedStyle.Left;
                        gridView.VisibleColumns["Containers1"].Width = 60;
                    }
                    if (gridView.VisibleColumns["SampleID"] != null)
                    {
                        gridView.VisibleColumns["SampleID"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                }
                if (View.Id == "Testparameter_LookupListView_Copy_COCSample_Copy_Parameter")
                {
                    if (IsDisableCheckBox)
                    {
                        var selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(i => i.ShowSelectCheckbox).FirstOrDefault();
                        if (selectionBoxColumn != null)
                        {
                            selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
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
        private void Grid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
                {
                    if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration" || View.Id == "COCSettingsTest_ListView_Copy_SampleRegistration")
                    {
                        if (objPermissionInfo.COCSettingsIsWrite == false && objPermissionInfo.COCSettingsIsDelete == false)
                        {
                            e.Enabled = false;
                            IsDisableCheckBox = true;
                        }
                    }
                    else
                    {
                        //COCSettingsSamples objCOCSamplelogin = View.ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", COCsr.SampleOid));
                        //var curOid = gridView.GetRowValues(e.VisibleIndex, "Oid");
                        //if (objCOCSamplelogin != null && curOid != null)
                        //{
                        //    COCSettingsTest objsmpltest = View.ObjectSpace.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.Oid] = ? And [COCSettingsSamples.Oid]= ?", curOid, objCOCSamplelogin.Oid));
                        //    if (COCsr != null && COCsr.lstSavedTestParameter != null && COCsr.lstSavedTestParameter.Count > 0 && e.VisibleIndex != -1 && COCsr.lstSavedTestParameter.Contains((Guid)curOid) && objsmpltest != null)
                        //    {
                        //        e.Enabled = false;
                        //        IsDisableCheckBox = true;
                        //    }
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
        private void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                e.Properties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                e.Properties["cpFilterRowCount"] = gridView.Selection.FilteredCount;

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
        {
            if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration")
            {
                if (e.CommandCellType == GridViewTableCommandCellType.Data)
                {
                    if (e.CommandColumn.Name == "Test")
                    {
                        e.Cell.Attributes.Add("onclick", jScript);
                        if (objPermissionInfo.COCSettingsIsWrite == false)
                        {
                            ((System.Web.UI.WebControls.WebControl)e.Cell.Controls[0]).Enabled = false;
                        }
                    }
                    else if (e.CommandColumn.Name == "Containers")
                    {
                        e.Cell.Attributes.Add("onclick", jScript);
                        if (objPermissionInfo.COCSettingsIsWrite == false)
                        {
                            ((System.Web.UI.WebControls.WebControl)e.Cell.Controls[0]).Enabled = false;
                        }
                    }
                }
            }

        }
        private void Grid_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        {
            UnitOfWork uow = new UnitOfWork(((XPObjectSpace)this.ObjectSpace).Session.DataLayer);
            List<Modules.BusinessObjects.Setting.COCSettingsSamples> lstSampleLogIn = ((ListView)View).CollectionSource.List.Cast<Modules.BusinessObjects.Setting.COCSettingsSamples>().Where(i => i.SubOut == true).ToList();
            foreach (Modules.BusinessObjects.Setting.COCSettingsSamples objSampleLogIn in lstSampleLogIn)
            {
                List<COCSettingsTest> objsample = uow.Query<COCSettingsTest>().Where(i => i.COCSettingsSamples != null && i.COCSettingsSamples.Oid == objSampleLogIn.Oid).ToList();
                objsample.ForEach(i => i.SubOut = true);
            }
            uow.CommitChanges();
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            if (View != null && View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
            {
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                COCsr.bolNewCOCID = false;
                objInfo.ClientName = null;
                objInfo.ProjectName = null;
                COCsr.NewClient = null;
                COCsr.NewProject = null;
                //ObjectSpace.Committed -= ObjectSpace_Committed;
                //ObjectSpace.Committing -= ObjectSpace_Committing;
                if (View != null && View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                {
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Execute -= SaveAction_Execute;
                        modificationController.SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                        modificationController.SaveAndNewAction.Executing -= SaveAndNewAction_Executing;
                    }
                }
            }
            if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration" || View.Id == "COCSettingsTest_ListView_Copy_SampleRegistration")
            {
                COCsr.strCOCID = null;
            }
            if (View != null && View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration")
            {
                objCOCSampleinfo.IsColumnsCustomized = false;
                //ObjectSpace.Committing -= ObjectSpace_Committing;
                COCsr.canGridRefresh = false;
                Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                WebWindow.CurrentRequestWindow.PagePreRender -= CurrentRequestWindow_PagePreRender;
            }
            if (View.Id == "COCSettingsSampleRegistration")
            {
                View.Closing -= View_Closing;
                View.Closed -= View_Closed;
            }
            if (View.Id == "COCSettingsTest_ListView_Copy_SampleRegistration")
            {
                Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                COCsr.strCOCID = null;
            }
            if (View is ListView || View is DetailView)
            {
                if (Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.Contains("ShowDelete"))
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.RemoveItem("ShowDelete");
                }
                if (Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.Contains("Deletebtn"))
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.RemoveItem("Deletebtn");
                }
            }
        }
        private void COCSLListViewEdit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration" && e.CurrentObject != null)
                {
                    DashboardView dv = (DashboardView)Application.MainWindow.View;
                    DashboardViewItem SLDetailView = (DashboardViewItem)dv.FindItem("COCSettingsSamples_DetailView");
                    DashboardViewItem SLListView = (DashboardViewItem)dv.FindItem("COCSettingsSamples_ListView");
                    if (SLListView != null && SLListView.InnerView != null && SLListView.InnerView.CurrentObject != null)
                    {
                        COCSettingsSamples SLObj = (COCSettingsSamples)e.CurrentObject;
                        if (SLObj.COCID != null)
                        {
                            (SLDetailView).Frame.GetController<ModificationsController>().Active["SL"] = false;
                            ((DetailView)SLDetailView.InnerView).ViewEditMode = ViewEditMode.Edit;
                            ((DetailView)SLDetailView.InnerView).ObjectSpace.Rollback();//there is an current object is already active in Detailview so before changing the unsaved object to new object we must rollback the ObjectSpace otherwise it will check the old object validation rules.
                            ((DetailView)SLDetailView.InnerView).CurrentObject = ((DetailView)SLDetailView.InnerView).ObjectSpace.GetObject<COCSettingsSamples>(SLObj);
                            (SLDetailView).Frame.GetController<ModificationsController>().Active["SL"] = true;
                            ((DetailView)SLDetailView.InnerView).Refresh();
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
        private void AssignClientSideLogic(ASPxGridLookupPropertyEditor propertyEditor)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Editor_Load(object sender, EventArgs e)
        {
            try
            {
                DevExpress.Web.ASPxSpinEdit editor = sender as DevExpress.Web.ASPxSpinEdit;
                editor.MinValue = 1;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Sample_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                COCsr.IsSamplePopupClose = false;
                if (ObjectSpace.IsModified)
                {
                    ObjectSpace.CommitChanges();
                }
                Save();
                COCSettings objCOC = (COCSettings)e.CurrentObject;
                if (!COCsr.isNoOfSampleDisable)
                {
                    InsertSamplesInCOCSettingsSample();
                }
                COCsr.strCOCID = null;
                if (COCsr.strCOCID == null || COCsr.strCOCID != objCOC.COC_ID || Application.MainWindow.View.Id == "COCSettings_DetailView_Copy_SampleRegistration")
                {

                    COCsr.strCOCID = objCOC.COC_ID;
                    objCOCSampleinfo.focusedCOCID = objCOC.COC_ID;
                    IObjectSpace os = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(View.ObjectSpace, typeof(COCSettingsSamples));
                    cs.Criteria["Filter"] = CriteriaOperator.Parse("[COCID.COC_ID] = ? AND [COCID.GCRecord] is NULL", COCsr.strCOCID);
                    DashboardView lstsmpllogin = Application.CreateDashboardView(os, "COCSettingsSampleRegistration", false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(lstsmpllogin);
                    showViewParameters.CreatedView = lstsmpllogin;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.AcceptAction.Active["OkayBtn"] = false;
                    dc.CancelAction.Active["CancelBtn"] = false;
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
        private void InsertSamplesInCOCSettingsSample()
        {
            try
            {
                COCSettings objCOCSettings = (COCSettings)Application.MainWindow.View.CurrentObject;
                if (objCOCSettings != null && objCOCSettings.NoOfSamples > 0)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    UnitOfWork uow = new UnitOfWork(((XPObjectSpace)os).Session.DataLayer);
                    Session currentSession = ((XPObjectSpace)os).Session;
                    COCSettings obj = uow.GetObjectByKey<COCSettings>(objCOCSettings.Oid);

                    bool DBAccess = false;
                    int SampleNo = 0;
                    for (int i = 1; i <= objCOCSettings.NoOfSamples; i++)
                    {
                        COCSettingsSamples objCOCNew = new COCSettingsSamples(uow);
                        //objCOCNew.JobID = uow.GetObjectByKey<Samplecheckin>(objCOCSettings.Oid);
                        objCOCNew.COCID = obj;
                        if (string.IsNullOrEmpty(objCOCSettings.SampleMatries) == false)
                        {
                            string[] strSamplematrix = objCOCSettings.SampleMatries.Split(';');
                            if (strSamplematrix.Count() == 1)
                            {
                                objCOCNew.VisualMatrix = uow.GetObjectByKey<Modules.BusinessObjects.Setting.VisualMatrix>(new Guid(strSamplematrix[0].Trim()));
                            }
                        }
                        objCOCNew.BatchID = objCOCSettings.BatchID;
                        objCOCNew.COCID = obj;
                        objCOCNew.PackageNumber = objCOCSettings.PackageNo;
                        //if (obj.DateCollected != null && obj.DateCollected != DateTime.MinValue)
                        //{
                        //    //objCOCNew.CollectDate = (DateTime)obj.DateCollected;
                        //    objCOCNew.CollectDate = Convert.ToDateTime(obj.DateCollected);
                        //}
                        //if (objCollector != null)
                        //{
                        //    objCOCNew.Collector = objCollector;
                        //}
                        if (DBAccess == false)
                        {
                            SelectedData sprocs = currentSession.ExecuteSproc("GetCOCSampleID", new OperandValue(objCOCNew.COCID.ToString()));
                            if (sprocs.ResultSet[1].Rows[0].Values[0] != null)
                            {
                                objCOCSampleinfo.SampleID = sprocs.ResultSet[1].Rows[0].Values[0].ToString();
                                SampleNo = Convert.ToInt32(objCOCSampleinfo.SampleID);
                                DBAccess = true;
                            }
                            else
                            {
                                return;
                            }
                        }
                        objCOCNew.SampleNo = SampleNo;
                        objCOCNew.Save();
                        uow.CommitChanges();
                        if (!string.IsNullOrEmpty(objCOCSettings.NPTest) && objCOCNew.VisualMatrix != null)
                        {
                            List<CustomDueDate> lstcustomrequest = uow.Query<CustomDueDate>().Where(j => j.COCSettings.Oid == objCOCSettings.Oid).ToList();
                            VisualMatrix objVisualMatrix = uow.GetObjectByKey<VisualMatrix>(objCOCNew.VisualMatrix.Oid);
                            List<string> lstTestNames = objCOCSettings.NPTest.Split(';').ToList();

                            foreach (string objTest in lstTestNames.ToList())
                            {
                                List<string> lstTestMethodCompo = objTest.Split('|').ToList();
                                if (lstTestMethodCompo.Count == 2)
                                {
                                    CustomDueDate custom = lstcustomrequest.Where(j => j.TestMethod != null && j.TestMethod.MatrixName != null && j.TestMethod.MethodName != null && j.TestMethod.MatrixName.MatrixName == objVisualMatrix.MatrixName.MatrixName && j.TestMethod.TestName == lstTestMethodCompo[0] && j.TestMethod.MethodName.MethodNumber == lstTestMethodCompo[1]).FirstOrDefault();
                                    List<Testparameter> lstTestParam = uow.Query<Testparameter>().Where(j => j.TestMethod != null && j.TestMethod.MatrixName != null && j.TestMethod.MethodName != null && j.Component != null && j.TestMethod.MatrixName.MatrixName == objVisualMatrix.MatrixName.MatrixName && j.TestMethod.TestName == lstTestMethodCompo[0] && j.TestMethod.MethodName.MethodNumber == lstTestMethodCompo[1] && j.Component.Components == "Default" && j.QCType != null && j.QCType.QCTypeName == "Sample").ToList();
                                    if (lstTestParam.Count > 0 && custom != null)
                                    {
                                        if (custom.Parameter == null || custom.Parameter == "AllParam")
                                        {
                                            foreach (Testparameter objTestParam in lstTestParam.ToList())
                                            {
                                                COCSettingsTest objsp = ObjectSpace.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [COCSettingsSamples.Oid] = ?", objTestParam.Oid, objCOCNew.Oid));
                                                if (objsp == null)
                                                {
                                                    COCSettingsTest newsample = new COCSettingsTest(uow);
                                                    newsample.COCSettingsSamples = uow.GetObjectByKey<COCSettingsSamples>(objCOCNew.Oid);
                                                    newsample.Testparameter = objTestParam;
                                                    objCOCNew.Test = true;
                                                    newsample.Save();
                                                }
                                                else
                                                {
                                                    objsp.Save();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string[] param = custom.ParameterDetails.Split(',');
                                            foreach (Testparameter objTestParam in lstTestParam.ToList())
                                            {
                                                if (param.Contains(objTestParam.Oid.ToString()))
                                                {
                                                    COCSettingsTest objsp = ObjectSpace.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [COCSettingsSamples.Oid] = ?", objTestParam.Oid, objCOCNew.Oid));
                                                    if (objsp == null)
                                                    {
                                                        COCSettingsTest newsample = new COCSettingsTest(uow);
                                                        newsample.COCSettingsSamples = uow.GetObjectByKey<COCSettingsSamples>(objCOCNew.Oid);
                                                        newsample.Testparameter = objTestParam;
                                                        objCOCNew.Test = true;
                                                        newsample.Save();
                                                    }
                                                    else
                                                    {
                                                        objsp.Save();
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                                else if (lstTestMethodCompo.Count == 1)
                                {
                                    TestMethod objTm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [IsGroup]=true And [MethodName.GCRecord] Is Null", lstTestMethodCompo[0]));
                                    if (objTm != null)
                                    {
                                        IList<GroupTestMethod> lstgrouptestmed = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objTm.Oid));
                                        foreach (GroupTestMethod objgtm in lstgrouptestmed.ToList())
                                        {
                                            CustomDueDate custom = lstcustomrequest.Where(j => j.TestMethod != null && j.TestMethod.MatrixName != null && j.TestMethod.MatrixName.MatrixName == objVisualMatrix.MatrixName.MatrixName && j.TestMethod.Oid == objgtm.TestMethod.Oid).FirstOrDefault();
                                            IList<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objgtm.TestParameter.TestMethod.Oid));
                                            if (custom != null && custom.Parameter == null || custom != null && custom.Parameter == "AllParam")
                                            {
                                                foreach (Testparameter param1 in lsttestpara.ToList())
                                                {
                                                    COCSettingsTest objsp = ObjectSpace.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [COCSettingsSamples.Oid] = ?", param1.Oid, objCOCNew.Oid));
                                                    if (objsp == null)
                                                    {
                                                        COCSettingsTest newsample = new COCSettingsTest(uow);
                                                        newsample.COCSettingsSamples = uow.GetObjectByKey<COCSettingsSamples>(objCOCNew.Oid);
                                                        newsample.Testparameter = uow.GetObjectByKey<Testparameter>(param1.Oid);
                                                        newsample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objgtm.Oid);
                                                        newsample.IsGroup = true;

                                                        newsample.Save();
                                                        uow.CommitChanges();
                                                    }
                                                    else
                                                    {
                                                        COCSettingsTest sample = uow.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid]=? And [COCSettingsSamples.Oid]=? And [Testparameter.Parameter.Oid] = ?", param1.TestMethod.Oid, objCOCNew.Oid, param1.Parameter.Oid));
                                                        if (sample != null)
                                                        {
                                                            sample.Save();
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (custom != null && custom.ParameterDetails != null)
                                                {
                                                    string[] param = custom.ParameterDetails.Split(',');
                                                    foreach (Testparameter param1 in lsttestpara.ToList())
                                                    {
                                                        if (param.Contains(param1.Oid.ToString()))
                                                        {
                                                            COCSettingsTest objsp = ObjectSpace.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [COCSettingsSamples.Oid] = ?", param1.Oid, objCOCNew.Oid));
                                                            if (objsp == null)
                                                            {
                                                                COCSettingsTest newsample = new COCSettingsTest(uow);
                                                                newsample.COCSettingsSamples = uow.GetObjectByKey<COCSettingsSamples>(objCOCNew.Oid);
                                                                newsample.Testparameter = uow.GetObjectByKey<Testparameter>(param1.Oid);
                                                                newsample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objgtm.Oid);
                                                                newsample.IsGroup = true;
                                                                newsample.Save();
                                                                uow.CommitChanges();
                                                            }
                                                            else
                                                            {
                                                                COCSettingsTest sample = uow.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid]=? And [COCSettingsSamples.Oid]=? And [Testparameter.Parameter.Oid] = ?", param1.TestMethod.Oid, objCOCNew.Oid, param1.Parameter.Oid));
                                                                if (sample != null)
                                                                {
                                                                    sample.Save();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            uow.CommitChanges();
                            AssignBottleAllocationToSamples(uow, objCOCNew.Oid);
                            objCOCNew.TestSummary = string.Join("; ", new XPQuery<COCSettingsTest>(uow).Where(j => j.COCSettingsSamples.Oid == objCOCNew.Oid && j.Testparameter != null && j.Testparameter.TestMethod != null).Select(j => j.Testparameter.TestMethod.TestName).Distinct().ToList());
                            //BottleIDUpdate(uow);
                        }
                        objCOCNew.Save();
                        SampleNo++;
                        uow.CommitChanges();
                        //AssignBottleAllocationToSamples(uow, objCOCNew.Oid);
                    }
                    os.Refresh();
                    os.Dispose();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void AssignBottleAllocationToSamples(UnitOfWork uow, Guid sampleOid)
        {
            try
            {
                COCSettingsSamples objSample = uow.GetObjectByKey<COCSettingsSamples>(sampleOid);
                IList<COCSettingsBottleAllocation> objCOCBottle = uow.GetObjects(uow.GetClassInfo(typeof(COCSettingsBottleAllocation)), CriteriaOperator.Parse("[COCSettingsRegistration]=?", sampleOid), null, int.MaxValue, false, true).Cast<COCSettingsBottleAllocation>().ToList();
                IList<COCSettingsTest> objCOCSampleParameters = uow.GetObjects(uow.GetClassInfo(typeof(COCSettingsTest)), CriteriaOperator.Parse("[COCSettingsSamples]=?", objSample.Oid), null, int.MaxValue, false, true).Cast<COCSettingsTest>().ToList();
                IList<TestMethod> lstTest = objCOCSampleParameters.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod).Distinct().ToList();
                if (objSample != null && objCOCBottle != null && objCOCBottle.Count == 0)
                {
                    foreach (TestMethod test in lstTest.Where(i => i.IsFieldTest != true))
                    {
                        addnewtestbottles(uow, test, objSample);
                    }
                    uow.CommitChanges();
                }
                else if (objCOCBottle != null && objCOCBottle.Count > 0)
                {
                    foreach (TestMethod test in lstTest.Where(i => i.IsFieldTest != true))
                    {
                        if (objCOCBottle.Where(a => a.TestMethod == test).ToList().Count == 0)
                        {
                            addnewtestbottles(uow, test, objSample);
                        }
                    }
                    foreach (COCSettingsBottleAllocation sampleBottle in objCOCBottle.ToList())
                    {
                        if (lstTest.Where(a => a == sampleBottle.TestMethod).ToList().Count == 0)
                        {
                            uow.Delete(sampleBottle);
                        }
                    }
                    uow.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void addnewtestbottles(UnitOfWork uow, TestMethod test, COCSettingsSamples objSample)
        {
            List<Guid> lstContainer = new List<Guid>();
            List<Guid> lstPreservative = new List<Guid>();
            IList<Guid> containerNames = test.TestGuides.Where(i => i.Container != null).Select(i => i.Container.Oid).ToList();
            lstContainer.AddRange(containerNames.Except(lstContainer).ToList());
            IList<Guid> Preservative = test.TestGuides.Where(i => i.Preservative != null).Select(i => i.Preservative.Oid).ToList();
            lstPreservative.AddRange(Preservative.Except(lstPreservative).ToList());

            COCSettingsBottleAllocation objNewBottle = new COCSettingsBottleAllocation(uow);
            objNewBottle.COCSettingsRegistration = objSample;
            objNewBottle.BottleID = "A";
            objNewBottle.TestMethod = test;
            if (lstContainer.Count == 1)
            {
                Modules.BusinessObjects.Setting.Container objContainer = uow.FindObject<Modules.BusinessObjects.Setting.Container>(CriteriaOperator.Parse("Oid=?", lstContainer[0]));
                if (objContainer != null)
                {
                    objNewBottle.Containers = objContainer;
                }
            }
            if (lstPreservative.Count == 1)
            {
                Preservative objpreservative = uow.FindObject<Preservative>(CriteriaOperator.Parse("Oid=?", lstPreservative[0]));
                if (objpreservative != null)
                {
                    objNewBottle.Preservative = objpreservative;
                }
            }
        }
        private void Test_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork(((XPObjectSpace)this.ObjectSpace).Session.DataLayer);
                Modules.BusinessObjects.Setting.COCSettingsSamples COCsamplelogIn = (Modules.BusinessObjects.Setting.COCSettingsSamples)e.CurrentObject;
                if (COCsamplelogIn != null && COCsamplelogIn.VisualMatrix != null)
                //if (COCsr.strSampleID != "error")
                {
                    COCsr.IsTestAssignmentClosed = false;
                    objCOCInfo.COCVisualMatrixName = COCsamplelogIn.VisualMatrix.MatrixName.MatrixName;
                    DashboardView dashboard = Application.CreateDashboardView(ObjectSpace, "COCTest", false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(dashboard);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    showViewParameters.CreatedView.Closed += CreatedView_Closed;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.AcceptAction.Active.SetItemValue("disable", false);
                    dc.CancelAction.Active.SetItemValue("disable", false);
                    dc.CloseOnCurrentObjectProcessing = false;
                    //dc.Accepting += Dc_Accepting;
                    showViewParameters.Controllers.Add(dc);
                    //CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse(""));
                    if (objLanguage.strcurlanguage != "En")
                    {
                        showViewParameters.CreatedView.Caption = "选择检测项目 - " + COCsamplelogIn.SampleID;
                    }
                    else
                    {
                        showViewParameters.CreatedView.Caption = "Test Assignment - " + COCsamplelogIn.SampleID;
                    }
                    COCsr.SampleOid = COCsamplelogIn.Oid;
                    COCsr.lstTestParameter = new List<Guid>();
                    COCsr.lstSavedTestParameter = new List<Guid>();
                    COCsr.lstdupfilterguid = new List<Guid>();
                    COCsr.lstdupfilterstr = new List<string>();
                    COCsr.lstSubOutTest = new List<Guid>();
                    COCsr.lstdupfilterSuboutstr = new List<string>();
                    COCsr.lstRemoveTestParameter = new List<Guid>();
                    List<COCSettingsTest> objsample = uow.Query<COCSettingsTest>().Where(i => i.COCSettingsSamples != null && i.COCSettingsSamples.Oid == COCsamplelogIn.Oid).ToList();
                    if (objsample != null && objsample.Count > 0)
                    {
                        foreach (COCSettingsTest sample in objsample.ToList())
                        {
                            if (!COCsr.lstTestParameter.Contains(sample.Testparameter.Oid))
                            {
                                if (sample.IsGroup != true)
                                {
                                    if (sample.Testparameter.TestMethod != null && sample.Testparameter.TestMethod.MethodName != null && sample.Testparameter.TestMethod.MatrixName != null && sample.Testparameter.Component != null)
                                    {
                                        if (!COCsr.lstdupfilterstr.Contains(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components))
                                        {
                                            COCsr.lstdupfilterstr.Add(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components);
                                            COCsr.lstdupfilterguid.Add(sample.Testparameter.Oid);
                                        }
                                    }
                                    else if (sample.Testparameter.TestMethod != null && sample.Testparameter.TestMethod.MatrixName != null && sample.Testparameter.Component != null)
                                    {
                                        if (!COCsr.lstdupfilterstr.Contains(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components))
                                        {
                                            COCsr.lstdupfilterstr.Add(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components);
                                            COCsr.lstdupfilterguid.Add(sample.Testparameter.Oid);
                                        }
                                    }
                                    COCsr.lstSavedTestParameter.Add(sample.Testparameter.Oid);
                                    COCsr.lstTestParameter.Add(sample.Testparameter.Oid);
                                    if (sample.SubOut == true)
                                    {
                                        if (!COCsr.lstSubOutTest.Contains(sample.Testparameter.TestMethod.Oid))
                                        {
                                            if (!COCsr.lstdupfilterSuboutstr.Contains(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components))
                                            {
                                                COCsr.lstdupfilterSuboutstr.Add(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components);
                                            }
                                            COCsr.lstSubOutTest.Add(sample.Testparameter.TestMethod.Oid);
                                        }
                                    }
                                    else if (sample.COCSettingsSamples.SubOut == true)
                                    {
                                        if (!COCsr.lstdupfilterSuboutstr.Contains(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components))
                                        {
                                            COCsr.lstdupfilterSuboutstr.Add(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components);
                                        }
                                        COCsr.lstSubOutTest.Add(sample.Testparameter.TestMethod.Oid);
                                    }
                                }
                                else
                                {
                                    GroupTestMethod objgtm = uow.FindObject<GroupTestMethod>(CriteriaOperator.Parse("[Oid] =?", sample.GroupTest.Oid));
                                    if (objgtm != null && objgtm.TestMethod != null && objgtm.TestMethod.Oid != null)
                                    {
                                        List<Testparameter> testparameters = uow.Query<Testparameter>().Where(i => i.TestMethod != null && i.TestMethod.Oid == objgtm.TestMethod.Oid).ToList();
                                        //IList<Testparameter> testparameters = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objgtm.TestMethod.Oid));
                                        foreach (Testparameter objtp in testparameters.ToList())
                                        {
                                            if (!COCsr.lstdupfilterguid.Contains(objtp.Oid))
                                            {
                                                COCsr.lstdupfilterguid.Add(objtp.Oid);
                                            }
                                            if (!COCsr.lstSavedTestParameter.Contains(objtp.Oid))
                                            {
                                                COCsr.lstSavedTestParameter.Add(objtp.Oid);
                                            }
                                            if (!COCsr.lstTestParameter.Contains(objtp.Oid))
                                            {
                                                COCsr.lstTestParameter.Add(objtp.Oid);
                                            }
                                        }
                                    }
                                    if (sample.SubOut == true)
                                    {
                                        if (!COCsr.lstSubOutTest.Contains(sample.Testparameter.TestMethod.Oid))
                                        {
                                            if (!COCsr.lstdupfilterSuboutstr.Contains(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components))
                                            {
                                                COCsr.lstdupfilterSuboutstr.Add(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components);
                                            }
                                            COCsr.lstSubOutTest.Add(sample.Testparameter.TestMethod.Oid);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    COCsr.IsTestcanFilter = true;
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                }
                else
                {
                    COCsr.strSampleID = "error";
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectmatrix"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        private void CreatedView_Closed(object sender, EventArgs e)
        {
            try
            {
                if (Frame is NestedFrame)
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    if (nestedFrame != null)
                    {
                        CompositeView view = nestedFrame.ViewItem.View;
                        foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                        {
                            if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                            {
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
                        COCsr.IsTestAssignmentClosed = true;
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
                if (parameter != "error")
                {
                    string[] values = parameter.Split('|');
                    if (values[0] == "Testselection")
                    {
                        if (values[1] == "Selected")
                        {
                            Guid curguid = new Guid(values[2]);
                            //COCsr.strSelectionMode = values[1];
                            if (!COCsr.lstTestParameter.Contains(curguid))
                            {
                                COCsr.lstTestParameter.Add(curguid);
                                if (COCsr.lstRemoveTestParameter.Contains(curguid))
                                {
                                    COCsr.lstRemoveTestParameter.Remove(curguid);
                                }
                            }
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            CompositeView view = nestedFrame.ViewItem.View;
                            Testparameter testparameter = ObjectSpace.GetObjectByKey<Testparameter>(curguid);
                            DashboardViewItem TestViewMain = ((DashboardView)view).FindItem("TestViewMain") as DashboardViewItem;
                            DashboardViewItem TestViewSub = ((DashboardView)view).FindItem("TestViewSub") as DashboardViewItem;
                            DashboardViewItem TestViewSubChild = ((DashboardView)view).FindItem("TestViewSubChild") as DashboardViewItem;
                            bool Oidchange = true;
                            Guid curusedguid = new Guid();
                            foreach (Testparameter objtestparameter in ((ListView)TestViewSub.InnerView).CollectionSource.List)
                            {
                                if (objtestparameter.Oid == testparameter.Oid)
                                {
                                    Oidchange = false;
                                }
                                if (Oidchange && objtestparameter.TestMethod.TestName == testparameter.TestMethod.TestName && objtestparameter.TestMethod.MethodName.MethodNumber == testparameter.TestMethod.MethodName.MethodNumber)
                                {
                                    curusedguid = objtestparameter.Oid;
                                }
                            }
                            if (Oidchange && TestViewSubChild != null && TestViewSubChild.InnerView.SelectedObjects.Count == 1)
                            {
                                Testparameter addnewtestparameter = (Testparameter)TestViewSubChild.InnerView.SelectedObjects[0];
                                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"] = ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["dupfilter"] =
                                CriteriaOperator.Parse(((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"].ToString().Replace(curusedguid.ToString(), addnewtestparameter.Oid.ToString()));
                                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", COCsr.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", COCsr.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                ASPxGridListEditor gridListEditor = ((ListView)TestViewSub.InnerView).Editor as ASPxGridListEditor;
                                if (gridListEditor != null && gridListEditor.Grid != null)
                                {
                                    COCsr.UseSelchanged = false;
                                    gridListEditor.Grid.Selection.SelectRowByKey(addnewtestparameter.Oid);
                                }
                            }
                        }
                        else if (values[1] == "UNSelected")
                        {
                            Guid curguid = new Guid(values[2]);
                            //COCsr.strSelectionMode = values[1];
                            if (COCsr.lstTestParameter.Contains(curguid))
                            {
                                COCsr.lstTestParameter.Remove(curguid);
                                if (!COCsr.lstRemoveTestParameter.Contains(curguid))
                                {
                                    COCsr.lstRemoveTestParameter.Add(curguid);
                                }
                            }
                            //NestedFrame nestedFrame = (NestedFrame)Frame;
                            //CompositeView view = nestedFrame.ViewItem.View;
                            //Testparameter testparameter = ObjectSpace.GetObjectByKey<Testparameter>(curguid);
                            //DashboardViewItem TestViewMain = ((DashboardView)view).FindItem("TestViewMain") as DashboardViewItem;
                            //DashboardViewItem TestViewSub = ((DashboardView)view).FindItem("TestViewSub") as DashboardViewItem;
                            //DashboardViewItem TestViewSubChild = ((DashboardView)view).FindItem("TestViewSubChild") as DashboardViewItem;
                            //Testparameter addnewtestparameter = null;
                            //foreach (Testparameter objtestparameter in ((ListView)TestViewSub.InnerView).CollectionSource.List)
                            //{
                            //    if (testparameter != null && objtestparameter.Oid == testparameter.Oid)
                            //    {
                            //        if (TestViewSubChild != null && TestViewSubChild.InnerView.SelectedObjects.Count > 0)
                            //        {
                            //            addnewtestparameter = (Testparameter)TestViewSubChild.InnerView.SelectedObjects[0];
                            //        }
                            //    }
                            //}
                            //if (addnewtestparameter != null)
                            //{
                            //    ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"] = ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["dupfilter"] =
                            //    CriteriaOperator.Parse(((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"].ToString().Replace(curguid.ToString(), addnewtestparameter.Oid.ToString()));
                            //    ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", COCsr.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                            //    ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", COCsr.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                            //    ASPxGridListEditor gridListEditor = ((ListView)TestViewSub.InnerView).Editor as ASPxGridListEditor;
                            //    if (gridListEditor != null && gridListEditor.Grid != null)
                            //    {
                            //        COCsr.UseSelchanged = false;
                            //        gridListEditor.Grid.Selection.SelectRowByKey(addnewtestparameter.Oid);
                            //    }
                            //}
                        }
                        else if (values[1] == "Selectall")
                        {
                            ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (editor != null && editor.Grid != null)
                            {
                                for (int i = 0; i < editor.Grid.VisibleRowCount; i++)
                                {
                                    Guid curguid = new Guid(editor.Grid.GetRowValues(i, "Oid").ToString());
                                    //COCsr.strSelectionMode = "Selected";
                                    if (!COCsr.lstTestParameter.Contains(curguid))
                                    {
                                        COCsr.lstTestParameter.Add(curguid);
                                    }
                                    if (COCsr.lstRemoveTestParameter.Contains(curguid))
                                    {
                                        COCsr.lstRemoveTestParameter.Remove(curguid);
                                    }
                                }
                            }
                        }
                        else if (values[1] == "UNSelectall")
                        {
                            ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (editor != null && editor.Grid != null)
                            {
                                for (int i = 0; i < editor.Grid.VisibleRowCount; i++)
                                {
                                    Guid curguid = new Guid(editor.Grid.GetRowValues(i, "Oid").ToString());
                                    //COCsr.strSelectionMode = "UNSelected";
                                    if (COCsr.lstTestParameter.Contains(curguid) /*&& !COCsr.lstSavedTestParameter.Contains(curguid)*/)
                                    {
                                        COCsr.lstTestParameter.Remove(curguid);
                                    }
                                    if (!COCsr.lstRemoveTestParameter.Contains(curguid))
                                    {
                                        COCsr.lstRemoveTestParameter.Add(curguid);
                                    }
                                }
                            }
                        }
                    }
                    else if (values[0] == "Parameter")
                    {
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(values[1]), "Oid");
                            if (HttpContext.Current.Session["rowid"] != null)
                            {
                                COCsr.lstSelParameter = new List<string>();
                                CustomDueDate objsampling = ((ListView)View).CollectionSource.List.Cast<CustomDueDate>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                                if (objsampling != null)
                                {
                                    List<COCSettingsSamples> lstcocSamples = View.ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("COCID.Oid=?", objsampling.COCSettings.Oid)).ToList();
                                    if (lstcocSamples.Count == 0)
                                    {
                                        CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Testparameter));
                                        cs.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid]=? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objsampling.TestMethod.Oid);
                                        COCsr.Totparam = cs.GetCount();
                                        if (objsampling.Parameter == "AllParam")
                                        {
                                            foreach (Testparameter strbotid in cs.List)
                                            {
                                                COCsr.lstSelParameter.Add(strbotid.Oid.ToString());
                                            }
                                        }
                                        else if (objsampling.ParameterDetails != null)
                                        {
                                            string[] strbottleid = objsampling.ParameterDetails.Split(',');
                                            foreach (var strbotid in strbottleid)
                                            {
                                                COCsr.lstSelParameter.Add(strbotid.Trim());
                                            }
                                        }
                                        ListView lv = Application.CreateListView("Testparameter_ListView_Parameter", cs, false);
                                        ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                                        showViewParameters.Context = TemplateContext.PopupWindow;
                                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                        showViewParameters.CreatedView.Caption = "Parameter";
                                        DialogController dc = Application.CreateController<DialogController>();
                                        dc.SaveOnAccept = false;
                                        dc.CloseOnCurrentObjectProcessing = false;
                                        dc.Accepting += Dc_Accepting2;
                                        showViewParameters.Controllers.Add(dc);
                                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                    }
                                }
                            }
                        }
                    }
                    else if (values[0] == "Registration")
                    {
                        objCOCSampleinfo.TestCOCID = values[1];
                    }
                    else if (values[0] == "SuboutSelected" || values[0] == "SuboutUnSelected")
                    {
                        ListView listView = null;
                        if (View is DashboardView)
                        {
                            DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                            if (TestViewSub != null && TestViewSub.InnerView != null)
                            {
                                listView = TestViewSub.InnerView as ListView;
                            }
                        }
                        else if (View is ListView)
                        {
                            if (View.Id == "Testparameter_LookupListView_Copy_COCSample_Copy")
                            {
                                listView = View as ListView;
                            }
                            else
                            {
                                if (Frame != null && Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                                    {
                                        CompositeView cv = nestedFrame.ViewItem.View;
                                        if (cv != null && cv is DashboardView)
                                        {
                                            DashboardViewItem TestViewSub = ((DashboardView)cv).FindItem("TestViewSub") as DashboardViewItem;
                                            if (TestViewSub != null && TestViewSub.InnerView != null)
                                            {
                                                listView = TestViewSub.InnerView as ListView;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (listView != null && listView.CollectionSource.GetCount() > 0)
                        {
                            ASPxGridListEditor editor = (ASPxGridListEditor)listView.Editor;
                            if (editor != null)
                            {
                                object curoid = editor.Grid.GetRowValues(int.Parse(values[1]), "Oid");
                                Testparameter param = ObjectSpace.GetObjectByKey<Testparameter>(curoid);
                                if (param != null && param.TestMethod != null)
                                {
                                    if (COCsr.lstSubOutTest == null)
                                    {
                                        COCsr.lstSubOutTest = new List<Guid>();
                                    }
                                    if (values[0] == "SuboutSelected")
                                    {
                                        if (!COCsr.lstSubOutTest.Contains(param.TestMethod.Oid))
                                        {
                                            COCsr.lstSubOutTest.Add(param.TestMethod.Oid);
                                        }
                                    }
                                    else
                                    {
                                        if (COCsr.lstSubOutTest.Contains(param.TestMethod.Oid))
                                        {
                                            COCsr.lstSubOutTest.Remove(param.TestMethod.Oid);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (values[0] == "BottleAllocationQtyUpdate")
                    {
                        if (View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration")
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            CompositeView view = nestedFrame.ViewItem.View;
                            foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                            {
                                if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                                {
                                    frameContainer.Frame.View.ObjectSpace.Refresh();
                                }
                            }
                        }
                    }
                    //else if (values[0] == "TAT")
                    //{
                    //    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //    if (gridListEditor != null)
                    //    {
                    //        object Guid = gridListEditor.Grid.GetRowValues(int.Parse(values[1]), "Oid");
                    //        if (Guid != null)
                    //        {
                    //            CustomDueDate objDate = View.ObjectSpace.FindObject<CustomDueDate>(CriteriaOperator.Parse("Oid=?", new Guid(Guid.ToString())));
                    //            COCSettings objcocsamplecheckin = (COCSettings)Application.MainWindow.View.CurrentObject;
                    //            if (objDate != null && objcocsamplecheckin != null && objcocsamplecheckin.TAT != null)
                    //            {
                    //                //if (objDate.TAT.Count <= objcocsamplecheckin.TAT.Count)
                    //                //{
                    //                //    int tatHour = objDate.TAT.Count;
                    //                //    int Day = 0;
                    //                //    if (tatHour >= 24)
                    //                //    {
                    //                //        Day = tatHour / 24;
                    //                //        objDate.DueDate = AddWorkingDays(DateTime.Now, Day);
                    //                //    }
                    //                //    else
                    //                //    {
                    //                //        objDate.DueDate = AddWorkingHours(DateTime.Now, tatHour);
                    //                //    }
                    //                //     ((ListView)View).Refresh();
                    //                //}
                    //                //else if (objDate.TAT.Count != objsamplecheckin.TAT.Count)
                    //                //{
                    //                //    TurnAroundTime objTAT = View.ObjectSpace.GetObjectByKey<TurnAroundTime>(objsamplecheckin.TAT.Oid);
                    //                //    objDate.TAT = objTAT;
                    //                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CRDueDate"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //                //    ((ListView)View).Refresh();
                    //                //    return;
                    //                //}

                    //            }
                    //        }
                    //    }
                    //}
                    else
                    {
                        if (View is ListView)
                        {
                            ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                        }
                        if (values[0] == "Delete" && !string.IsNullOrEmpty(values[1]))
                        {
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            IList<COCSettingsTest> objsample = objspace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("COCSettingsSamples=?", new Guid(values[1])));
                            COCSettingsSamples sampleLog = objspace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("Oid=?", new Guid(values[1])));
                            objspace.Delete(objsample);
                            sampleLog.Test = false;
                            objspace.CommitChanges();
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
                        else
                        {
                            if (values[0] != "VisualMatrix")
                            {
                                COCsr.strSampleID = values[0].Substring(values[0].IndexOf("-") + 1);
                            }
                            else
                            {
                                COCsr.strSampleID = null;
                            }
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            VisualMatrix newobj = objspace.FindObject<VisualMatrix>(CriteriaOperator.Parse("Oid = ?", new Guid(values[1])));
                            if (newobj != null)
                            {
                                objCOCSampleinfo.COCVisualMatrixName = newobj.MatrixName.MatrixName;
                            }
                            objCOCSampleinfo.COCOid = values[2];
                        }
                    }
                }
                else
                {
                    COCsr.strSampleID = "error";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }


        }
        private void Dc_Accepting2(object sender, DialogControllerAcceptingEventArgs e)
        {
            CustomDueDate objsampling = ((ListView)View).CollectionSource.List.Cast<CustomDueDate>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
            if (objsampling != null)
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    objsampling.ParameterDetails = string.Join(",", e.AcceptActionArgs.SelectedObjects.Cast<Testparameter>().OrderBy(a => a.Sort).Select(a => a.Oid));
                }
                else
                {
                    //objsampling.ParameterDetails = null;
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectparameter"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    return;
                }
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    if (e.AcceptActionArgs.SelectedObjects.Count == COCsr.Totparam)
                    {
                        objsampling.Parameter = "AllParam";
                    }
                    else
                    {
                        objsampling.Parameter = "Customised";
                    }
                }
                else
                {
                    objsampling.Parameter = null;
                }
            }
        }
        private void TestGroup_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }
        private void SimpleAction1_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                CriteriaOperator cs = CriteriaOperator.Parse("COC_ID=?", COCsr.strCOCID);
                Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                COCSettings objCOCsample = uow.FindObject<COCSettings>(cs);
                COCSettingsSamples objCOCcheckin = new COCSettingsSamples(uow);
                objCOCcheckin.COCID = objCOCsample;
                int sampleno = 0;
                SelectedData sproc = currentSession.ExecuteSproc("GetCOCSampleID", new OperandValue(objCOCsample.COC_ID));
                if (sproc.ResultSet[1].Rows[0].Values[0].ToString() != null)
                {
                    sampleno = (int)sproc.ResultSet[1].Rows[0].Values[0];
                }
                if (objCOCsample != null && !string.IsNullOrEmpty(objCOCsample.SampleMatries) && !objCOCsample.SampleMatries.Contains(";"))
                {
                    VisualMatrix vs = uow.GetObjectByKey<VisualMatrix>(new Guid(objCOCsample.SampleMatries.Trim()));
                    objCOCcheckin.VisualMatrix = vs;
                }
                objCOCcheckin.SampleNo = sampleno;
                //if (!string.IsNullOrEmpty(objCOCsample.NPTest) && objCOCcheckin.VisualMatrix != null)
                //{
                //    List<CustomDueDate> lstcustomrequest = uow.Query<CustomDueDate>().Where(i => i.COCSettings.Oid == objCOCsample.Oid).ToList();
                //    VisualMatrix objVisualMatrix = uow.GetObjectByKey<VisualMatrix>(objCOCcheckin.VisualMatrix.Oid);
                //    List<string> lstTestNames = objCOCsample.NPTest.Split(';').ToList();

                //    foreach (string objTest in lstTestNames.ToList())
                //    {
                //        List<string> lstTestMethodCompo = objTest.Split('|').ToList();
                //        if (lstTestMethodCompo.Count == 2)
                //        {
                //            CustomDueDate custom = lstcustomrequest.Where(i => i.TestMethod != null && i.TestMethod.MatrixName != null && i.TestMethod.MethodName != null && i.TestMethod.MatrixName.MatrixName == objVisualMatrix.MatrixName.MatrixName && i.TestMethod.TestName == lstTestMethodCompo[0] && i.TestMethod.MethodName.MethodNumber == lstTestMethodCompo[1]).FirstOrDefault();
                //            List<Testparameter> lstTestParam = uow.Query<Testparameter>().Where(i => i.TestMethod != null && i.TestMethod.MatrixName != null && i.TestMethod.MethodName != null && i.Component != null && i.TestMethod.MatrixName.MatrixName == objVisualMatrix.MatrixName.MatrixName && i.TestMethod.TestName == lstTestMethodCompo[0] && i.TestMethod.MethodName.MethodNumber == lstTestMethodCompo[1] && i.Component.Components == "Default" && i.QCType != null && i.QCType.QCTypeName == "Sample").ToList();
                //            if (lstTestParam.Count > 0 && custom != null)
                //            {
                //                if (custom.Parameter == null || custom.Parameter == "AllParam")
                //                {
                //                    foreach (Testparameter objTestParam in lstTestParam.ToList())
                //                    {
                //                        COCSettingsTest objsp = ObjectSpace.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [COCSettingsSamples.Oid] = ?", objTestParam.Oid, objCOCcheckin.Oid));
                //                        if (objsp == null)
                //                        {
                //                            COCSettingsTest newsample = new COCSettingsTest(uow);
                //                            newsample.COCSettingsSamples = objCOCcheckin;
                //                            newsample.Testparameter = objTestParam;
                //                            objCOCcheckin.Test = true;
                //                            newsample.Save();
                //                        }
                //                        else
                //                        {
                //                            objsp.Save();
                //                        }

                //                    }
                //                }
                //                else
                //                {
                //                    string[] param = custom.ParameterDetails.Split(',');
                //                    foreach (Testparameter objTestParam in lstTestParam.ToList())
                //                    {
                //                        if (param.Contains(objTestParam.Oid.ToString()))
                //                        {
                //                            COCSettingsTest objsp = ObjectSpace.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [COCSettingsSamples.Oid] = ?", objTestParam.Oid, objCOCcheckin.Oid));
                //                            if (objsp == null)
                //                            {
                //                                COCSettingsTest newsample = new COCSettingsTest(uow);
                //                                newsample.COCSettingsSamples = objCOCcheckin;
                //                                newsample.Testparameter = objTestParam;
                //                                objCOCcheckin.Test = true;
                //                                newsample.Save();
                //                            }
                //                            else
                //                            {
                //                                objsp.Save();
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        else if (lstTestMethodCompo.Count == 1)
                //        {
                //            TestMethod objTm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [IsGroup]=true And [MethodName.GCRecord] Is Null", lstTestMethodCompo[0]));
                //            if (objTm != null)
                //            {
                //                IList<GroupTestMethod> lstgrouptestmed = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objTm.Oid));
                //                foreach (GroupTestMethod objgtm in lstgrouptestmed.ToList())
                //                {
                //                    CustomDueDate custom = lstcustomrequest.Where(i => i.TestMethod != null && i.TestMethod.MatrixName != null && i.TestMethod.MatrixName.MatrixName == objVisualMatrix.MatrixName.MatrixName && i.TestMethod.Oid == objgtm.TestMethod.Oid).FirstOrDefault();
                //                    IList<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objgtm.TestParameter.TestMethod.Oid));
                //                    if (custom != null && (custom.Parameter == null || custom.Parameter == "AllParam"))
                //                    {
                //                        foreach (Testparameter param1 in lsttestpara.ToList())
                //                        {
                //                            COCSettingsTest objsp = ObjectSpace.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [COCSettingsSamples.Oid] = ?", param1.Oid, objCOCcheckin.Oid));
                //                            if (objsp == null)
                //                            {
                //                                COCSettingsTest newsample = new COCSettingsTest(uow);
                //                                newsample.COCSettingsSamples = objCOCcheckin;
                //                                newsample.Testparameter = uow.GetObjectByKey<Testparameter>(param1.Oid);
                //                                newsample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objgtm.Oid);
                //                                newsample.IsGroup = true;
                //                                newsample.Save();
                //                                uow.CommitChanges();
                //                            }
                //                            else
                //                            {
                //                                COCSettingsTest sample = uow.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid]=? And [COCSettingsSamples.Oid]=? And [Testparameter.Parameter.Oid] = ?", param1.TestMethod.Oid, objCOCcheckin.Oid, param1.Parameter.Oid));
                //                                if (sample != null)
                //                                {
                //                                    sample.Save();
                //                                }
                //                            }
                //                        }
                //                    }
                //                    else
                //                    {
                //                        if (custom != null && custom.ParameterDetails != null)
                //                        {
                //                            string[] param = custom.ParameterDetails.Split(',');
                //                            foreach (Testparameter param1 in lsttestpara.ToList())
                //                            {
                //                                if (param.Contains(param1.Oid.ToString()))
                //                                {
                //                                    COCSettingsTest objsp = ObjectSpace.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [COCSettingsSamples.Oid] = ?", param1.Oid, objCOCcheckin.Oid));
                //                                    if (objsp == null)
                //                                    {
                //                                        COCSettingsTest newsample = new COCSettingsTest(uow);
                //                                        newsample.COCSettingsSamples = objCOCcheckin;
                //                                        newsample.Testparameter = uow.GetObjectByKey<Testparameter>(param1.Oid);
                //                                        newsample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objgtm.Oid);
                //                                        newsample.IsGroup = true;
                //                                        newsample.Save();
                //                                        uow.CommitChanges();
                //                                    }
                //                                    else
                //                                    {
                //                                        COCSettingsTest sample = uow.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid]=? And [COCSettingsSamples.Oid]=? And [Testparameter.Parameter.Oid] = ?", param1.TestMethod.Oid, objCOCcheckin.Oid, param1.Parameter.Oid));
                //                                        if (sample != null)
                //                                        {

                //                                            sample.Save();
                //                                        }
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                objCOCcheckin.Save();
                uow.CommitChanges();
                //if (!string.IsNullOrEmpty(objCOCsample.NPTest) && objCOCcheckin.VisualMatrix != null)
                //{
                //    AssignBottleAllocationToSamples(uow, objCOCcheckin.Oid);
                //    //BottleIDUpdate(uow);
                //    //Application.MainWindow.GetController<RegistrationSignOffController>().PendingSigningOffJobIDCount();
                //}
                ((ListView)View).CollectionSource.Add(((ListView)View).ObjectSpace.GetObject(objCOCcheckin));
                View.Refresh();
            }

            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Containers_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ObjectSpace.CommitChanges();
                Save();
                {
                    COCSettings objCOCsettings = COCsr.CurrentCOC;
                    if (objCOCsettings != null)
                    {
                        COCsr.strCOCID = objCOCsettings.COC_ID;
                        string[] strvmarr = objCOCsettings.SampleMatries.Split(';');
                        COCsr.lstCOCvisualmat = new List<VisualMatrix>();
                        foreach (string strvmoid in strvmarr.ToList())
                        {
                            VisualMatrix lstvmatobj = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strvmoid)));
                            if (lstvmatobj != null)
                            {
                                COCsr.lstCOCvisualmat.Add(lstvmatobj);
                            }
                        }
                        COCSettingsBottleAllocation newsmplbtlalloc = View.ObjectSpace.CreateObject<COCSettingsBottleAllocation>();
                        DetailView dvbottleAllocation = Application.CreateDetailView(View.ObjectSpace, "COCSettingsBottleAllocation_DetailView_Copy_SampleRegistration", false, newsmplbtlalloc);
                        ShowViewParameters showViewParameters = new ShowViewParameters(dvbottleAllocation);
                        showViewParameters.CreatedView = dvbottleAllocation;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.AcceptAction.Active["OkayBtn"] = false;
                        dc.CancelAction.Active["CancelBtn"] = false;
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

    }
}
