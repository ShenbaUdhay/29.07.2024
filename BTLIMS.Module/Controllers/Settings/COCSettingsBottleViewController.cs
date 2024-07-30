using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.Templates.ActionContainers.Menu;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
//using Modules.BusinessObjects.TaskManagement;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Container = Modules.BusinessObjects.Setting.Container;
using SortDirection = DevExpress.Xpo.SortDirection;

namespace Labmaster.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class COCSettingsBottleViewController : ViewController, IXafCallbackHandler
    {
        SimpleAction COCPrevious;
        SimpleAction COCNext;
        SimpleAction COCSaveall;
        SimpleAction COCClearall;
        SimpleAction COCDistribution;
        SimpleAction COCDistributionNext;
        SimpleAction COCDistributionPrevious;
        SimpleAction COCDistributionClose;
        SimpleAction BottleAllocationAddCOC;
        SimpleAction BottleAllocationRemovecoc;
        private const string Key = "hide action in COCSetting";
        PermissionInfo objPermissionInfo = new PermissionInfo();
        COCSettingsInfo COCInfo = new COCSettingsInfo();
        TaskManagementInfo TMInfo = new TaskManagementInfo();
        COCSettingsRegistrationInfo COCsr = new COCSettingsRegistrationInfo();
        MessageTimer timer = new MessageTimer();
        bool IsQtyChanged = false;
        bool samplingfirstdefault = false;
        bool visualmatfirstdefault = false;
        bool IsbtlIDchanged = false;
        bool IsBtlRemove = false;
        bool IsAdd = false;
        ICallbackManagerHolder BottleDelcallbackManager;
        public COCSettingsBottleViewController()
        {
            InitializeComponent();
            TargetViewId = "COCSettingsSamples_ListView;" + "COCSettingsBottleAllocation_DetailView_Copy_SampleRegistration;" + "COCBottleSetup_ListView;" + "COCSettings_DetailView;" + "TestMethod_LookupListView_COCBottlesetup;" + "COCBottleSetup_Test_ListView;"
                + "NPCOCSettingsSample_Bottle_DetailView;" + "COCBottleSetup_DetailView;" + "COCBottlesetupdistribution_ListView;" + "COCSettingsSamples_ListView_Copy_Bottle;" + "COCSettingsTest_ListView_Bottle;" + "SampleBottleAllocation_ListView_COCSettings;"
                + "VisualMatrix_ListView_COCSettings_SampleMatrix;" + "COCSettings;" + "DummyClass_ListView_COCSettings;" + "DummyClass_ListView_COCSetting_TEST_POPUP;"
                + "TestMethod_ListView_COCBA_Popup;" + "TestMethod_ListView_COC_BA;" + "COCSettingsSamples_ListView_COCBottle_SelectedSampleID;" + "COCSettingsSamples_DetailView_CopytoSampleID;"
                + "COCSettingsSamples_ListView_COCBottle_CopyTOBottleAllocation;" + "COCSettingsBottleAllocation_ListView_Copy_Sampleregistration;" + "DummyClass_ListView_COCSettingsSample;";
            COCCopyBottleSet.TargetViewId = "SampleBottleAllocation_ListView_COCSettings";
            //COCBottleSetup.TargetViewId = "COCSettingsSamples_ListView;";
            COCBottleDeleteSetup.TargetViewId = "COCBottleSetup_ListView";
            COCPrevious = new SimpleAction(this, "COCPrevious", PredefinedCategory.PopupActions);
            COCPrevious.Caption = "< Previous";
            COCNext = new SimpleAction(this, "COCNext", PredefinedCategory.PopupActions);
            COCNext.Caption = "Next >";
            COCSaveall = new SimpleAction(this, "COCSaveall", PredefinedCategory.PopupActions);
            COCSaveall.Caption = "Save";
            COCClearall = new SimpleAction(this, "COCClearall", PredefinedCategory.PopupActions);
            COCClearall.Caption = "Cancel";
            COCDistributionClose = new SimpleAction(this, "COCDistributionClose", PredefinedCategory.Options);
            COCDistributionClose.Caption = "Close";
            COCDistribution = new SimpleAction(this, "COCDistribution", PredefinedCategory.ObjectsCreation);
            COCDistribution.Caption = "Distribution";
            COCDistributionPrevious = new SimpleAction(this, "COCDistributionPrevious", PredefinedCategory.ObjectsCreation);
            COCDistributionPrevious.Caption = "<";
            COCDistributionPrevious.ToolTip = "Previous";
            COCDistributionNext = new SimpleAction(this, "COCDistributionNext", PredefinedCategory.ObjectsCreation);
            COCDistributionNext.Caption = ">";
            COCDistributionNext.ToolTip = "Next";
            COCPrevious.TargetViewId = COCNext.TargetViewId = COCSaveall.TargetViewId = COCClearall.TargetViewId = COCDistribution.TargetViewId = "COCBottleSetup_ListView;";
            COCDistributionNext.TargetViewId = COCDistributionPrevious.TargetViewId = COCDistributionClose.TargetViewId = "BottleSetupDistributionCOC";
            btnAssignbottles_BottleAllocation.TargetViewId = btnCopybottles_BottleAllocation.TargetViewId = btnResetbottles_BottleAllocation.TargetViewId = "COCSettingsBottleAllocation_ListView_Copy_Sampleregistration;";

            /*ADD*/
            BottleAllocationAddCOC = new SimpleAction(this, "BottleAllocationAddCOC", PredefinedCategory.RecordEdit);
            BottleAllocationAddCOC.Caption = "Add";
            BottleAllocationAddCOC.ToolTip = "Add";
            BottleAllocationAddCOC.ImageName = "Add.png";
            BottleAllocationAddCOC.TargetViewId = "SampleBottleAllocation_ListView_COCSettings";
            BottleAllocationAddCOC.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            /*RemoveButtonProperties*/
            BottleAllocationRemovecoc = new SimpleAction(this, "BottleAllocationRemovecoc", PredefinedCategory.RecordEdit);
            BottleAllocationRemovecoc.Caption = "Remove";
            BottleAllocationRemovecoc.ToolTip = "Remove";
            BottleAllocationRemovecoc.ImageName = "Remove.png";
            BottleAllocationRemovecoc.TargetViewId = "SampleBottleAllocation_ListView_COCSettings";
            BottleAllocationRemovecoc.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            COCNext.Execute += Next_Execute;
            COCPrevious.Execute += Previous_Execute;
            COCSaveall.Execute += Saveall_Execute;
            COCClearall.Execute += Clearall_Execute;
            COCDistribution.Execute += Distribution_Execute;
            COCDistributionPrevious.Execute += DistributionPrevious_Execute;
            COCDistributionNext.Execute += DistributionNext_Execute;
            COCDistributionClose.Execute += DistributionClose_Execute;
            BottleAllocationAddCOC.Execute += BottleAllocationAdd_Execute;
            BottleAllocationRemovecoc.Execute += BottleAllocationRemove_Execute;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        private void BottleAllocationcoc_CustomizeControl(object sender, CustomizeControlEventArgs e)
        {
            try
            {
                SimpleActionMenuActionItem button = e.Control as SimpleActionMenuActionItem;
                if (button != null)
                {
                    button.MenuItem.ItemStyle.Width = 10;
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
                Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                ObjectSpace.Committed += ObjectSpace_Committed;
                ObjectSpace.Committing += ObjectSpace_Committing;
                COCNext.Active[Key] = objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit;
                COCPrevious.Active[Key] = objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit;
                COCSaveall.Active[Key] = objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit;
                COCClearall.Active[Key] = objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit;
                COCBottleDeleteSetup.Active[Key] = objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit;
                COCDistribution.Active[Key] = objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit;
                var linkunlink = Frame.GetController<LinkUnlinkController>();
                if (linkunlink != null)
                {
                    linkunlink.LinkAction.Active[Key] = objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit;
                    linkunlink.UnlinkAction.Active[Key] = objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit;
                    linkunlink.LinkAction.Execute += LinkAction_Execute;
                    linkunlink.UnlinkAction.Execute += UnlinkAction_Execute;
                    linkunlink.LinkAction.CustomizePopupWindowParams += LinkAction_CustomizePopupWindowParams;
                }
                if (View.Id == "Matrix_LookupListView_Bottlesetup")
                {
                    if (objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[MatrixName] In (" + string.Format("'{0}'", string.Join("','", COCInfo.lstMatrix.Select(i => i.Replace("'", "''")))) + ")");
                    }
                }
                if (View.Id == "DummyClass_ListView_COCSettingsSample")
                {
                    COCsr.selectionhideGuid = new List<string>();
                    COCsr.lstviewselected = new List<DummyClass>();
                }
                if (View is DashboardView && View.Id == "BottleSetupDistributionCOC")
                {
                    View.ControlsCreated += View_ControlsCreated;
                }

                if (View.Id == "VisualMatrix_ListView_COCSettings_SampleMatrix")
                {
                    visualmatfirstdefault = true;
                    if (COCInfo.strcocID == null)
                    {
                        COCInfo.strcocID = string.Empty;
                    }
                    View.SelectionChanged += View_VisualMat_SelectionChanged;
                    View.ControlsCreated += View_ControlsCreated;
                    List<Guid> vmguid = new List<Guid>();
                    COCSettings objTasks = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("COC_ID=" + COCInfo.strcocID));
                    //IList<COCSettingsSamples> lstsampling = ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("COC.Oid=?", objTasks.Oid));
                    //if (lstsampling != null && lstsampling.Count > 0)
                    //{
                    //    foreach (COCSettingsSamples objsampling in lstsampling.ToList())
                    //    {
                    //        if (objsampling.VisualMatrix != null && objsampling.VisualMatrix.Oid != null)
                    //        {
                    //            if (!vmguid.Contains(objsampling.VisualMatrix.Oid))
                    //            {
                    //                vmguid.Add(objsampling.VisualMatrix.Oid);
                    //            }
                    //        }
                    //    }
                    //    if (vmguid != null && vmguid.Count > 0)
                    //    {
                    //        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", vmguid.Select(i => i.ToString().Replace("'", "''")))) + ")");
                    //    }
                    //    else
                    //    {
                    //        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                    //    }
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                    //}
                }
                if (View.Id == "TestMethod_ListView_COC_BA" || View.Id == "SampleBottleAllocation_ListView_COCSettings" || View.Id == "COCSettingsSamples_ListView_COCBottle" || View.Id == "COCSettingsBottleAllocation_ListView_Copy_Sampleregistration")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                }
                if (View.Id == "COCSettingsBottleAllocation_ListView_Copy_Sampleregistration")
                {
                    if (COCsr.lstsmplbtlallo == null)
                    {
                        COCsr.lstsmplbtlallo = new List<COCSettingsBottleAllocation>();
                    }
                    foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions.Where(i => i.RoleNavigationPermissionDetails.FirstOrDefault(x => x.NavigationItem.NavigationId == "COCSettings") != null))
                    {
                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "COCSettings" && i.Create == true) != null)
                        {
                            objPermissionInfo.COCBottleIsCreate = true;
                            btnAssignbottles_BottleAllocation.Active["DisableAssign"] = true;
                            btnCopybottles_BottleAllocation.Active["DisableCopySet"] = true;
                            btnResetbottles_BottleAllocation.Active["DisableReset"] = true;
                            return;
                        }
                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "COCSettings" && i.Write == true) != null)
                        {
                            objPermissionInfo.COCBottleIsWrite = true;
                            btnAssignbottles_BottleAllocation.Active["DisableAssign"] = true;
                            btnCopybottles_BottleAllocation.Active["DisableCopySet"] = true;
                            btnResetbottles_BottleAllocation.Active["DisableReset"] = true;
                            return;
                        }
                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "COCSettings" && i.Read == true) != null)
                        {
                            objPermissionInfo.COCBottleIsRead = true;
                            btnAssignbottles_BottleAllocation.Active["DisableAssign"] = false;
                            btnCopybottles_BottleAllocation.Active["DisableCopySet"] = false;
                            btnResetbottles_BottleAllocation.Active["DisableReset"] = false;
                            return;
                        }
                    }
                }
                if (View.Id == "DummyClass_ListView_COCSettings")
                {
                    COCInfo.selectionhideGuid = new List<string>();
                    //COCInfo.lstviewselected = new List<DummyClass>();
                }
                if (View.Id == "SampleBottleAllocation_ListView_COCSettings")
                {
                    COCInfo.lstsmplbtlallo = new List<SampleBottleAllocation>();
                    foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                    {
                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "COCSettings" && i.Create == true) != null)
                        {
                            objPermissionInfo.COCBottleIsCreate = true;
                            BottleAllocationAddCOC.Active["DisableAddButton"] = true;
                            BottleAllocationRemovecoc.Active["DisableRemoveButton"] = true;
                            COCCopyBottleSet.Active["DisableCopySet"] = true;
                            return;
                        }
                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "COCSettings" && i.Write == true) != null)
                        {
                            objPermissionInfo.COCBottleIsWrite = true;
                            COCCopyBottleSet.Active["DisableCopySet"] = true;
                            BottleAllocationAddCOC.Active["DisableButton"] = false;
                            BottleAllocationRemovecoc.Active["DisableRemoveButton"] = false;
                            return;
                        }
                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "COCSettings" && i.Read == true) != null)
                        {
                            objPermissionInfo.COCBottleIsRead = true;
                            BottleAllocationAddCOC.Active["DisableButton"] = false;
                            BottleAllocationRemovecoc.Active["DisableRemoveButton"] = false;
                            COCCopyBottleSet.Active["DisableCopySet"] = false;
                            return;
                        }
                    }
                }
                if (View.Id == "TestMethod_ListView_COCBA_Popup")
                {
                    COCInfo.lstdummytests = new List<Guid>();
                }
                if (View.Id == "COCSettingsSamples_ListView_COCBottle")
                {
                    View.SelectionChanged += View_Sampling_SelectionChanged;
                    samplingfirstdefault = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }

        private void btnAssignbottles_BottleAllocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (COCsr.SamplingGuid != null)
                {
                    int i = 65;
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    List<COCSettingsBottleAllocation> lstcurrentBottle = uow.Query<COCSettingsBottleAllocation>().Where(a => a.COCSettingsRegistration != null && a.COCSettingsRegistration.Oid == COCsr.SamplingGuid).ToList();
                    //if (lstcurrentBottle.Where(a => a.SignOffBy != null).ToList().Count > 0)
                    //{
                    //    Application.ShowViewStrategy.ShowMessage("Bottle cannot be assigned", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //    return;
                    //}

                    lstcurrentBottle.ForEach(a => a.BottleID = string.Empty);
                    if (lstcurrentBottle.Count > 0)
                    {
                        Guid matrix = lstcurrentBottle.Select(a => a.COCSettingsRegistration.VisualMatrix.Oid).FirstOrDefault();
                        IList<string> Testguids = lstcurrentBottle.Select(a => a.TestMethod.Oid.ToString()).ToList();
                        List<BottleSharing> lstBottlesharing = uow.Query<BottleSharing>().Where(a => a.SampleMatrix.Oid == matrix).ToList();
                        foreach (BottleSharing bottleSharing in lstBottlesharing)
                        {
                            string[] sharingtest = bottleSharing.Tests.Split(new string[] { "; " }, StringSplitOptions.None);
                            if (sharingtest.Count(a => Testguids.Contains(a)) > 1)
                            {
                                foreach (COCSettingsBottleAllocation sample in lstcurrentBottle.Where(a => sharingtest.Contains(a.TestMethod.Oid.ToString())).ToList())
                                {
                                    if (sample.BottleID == string.Empty)
                                    {
                                        sample.BottleID = ((char)i).ToString();
                                    }
                                    else
                                    {
                                        //sample.BottleID += ", " + ((char)i).ToString();
                                        COCSettingsBottleAllocation objbottle = uow.FindObject<COCSettingsBottleAllocation>(CriteriaOperator.Parse("[COCSettingsRegistration.Oid] = ? and [TestMethod.Oid] = ? And [BottleID]=?", sample.COCSettingsRegistration.Oid, sample.TestMethod.Oid, ((char)i).ToString(), true));
                                        if (objbottle == null)
                                        {
                                            COCSettingsBottleAllocation objNewBottle = new COCSettingsBottleAllocation(uow);
                                            objNewBottle.BottleID = ((char)i).ToString();
                                            objNewBottle.COCSettingsRegistration = uow.GetObjectByKey<COCSettingsSamples>(sample.COCSettingsRegistration.Oid);
                                            objNewBottle.TestMethod = uow.GetObjectByKey<TestMethod>(sample.TestMethod.Oid);
                                            if (sample.Containers != null)
                                            {
                                                objNewBottle.Containers = uow.GetObjectByKey<Container>(sample.Containers.Oid);
                                            }
                                            if (sample.Preservative != null)
                                            {
                                                objNewBottle.Preservative = uow.GetObjectByKey<Preservative>(sample.Preservative.Oid);
                                            }
                                            if (sample.StorageID != null)
                                            {
                                                objNewBottle.StorageID = uow.GetObjectByKey<Storage>(sample.StorageID.Oid);
                                            }
                                            if (sample.StorageCondition != null)
                                            {
                                                objNewBottle.StorageCondition = uow.GetObjectByKey<PreserveCondition>(sample.StorageCondition.Oid);
                                            }
                                        }
                                    }
                                }
                            }
                            i++;
                        }
                        IList<COCSettingsBottleAllocation> EBottle = lstcurrentBottle.Where(a => string.IsNullOrEmpty(a.BottleID)).ToList();
                        if (EBottle.Count > 0)
                        {
                            foreach (COCSettingsBottleAllocation bottle in EBottle)
                            {
                                bottle.BottleID = ((char)i).ToString();
                            }
                        }
                        else
                        {
                            i--;
                        }
                        lstcurrentBottle.FirstOrDefault().COCSettingsRegistration.Qty = (uint)(i - 64);
                        uow.CommitChanges();
                        View.ObjectSpace.Refresh();
                        samplegridrefresh();
                        Application.ShowViewStrategy.ShowMessage("Bottle assigned successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Select the sample id", InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void samplegridrefresh()
        {
            CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
            if (cv != null)
            {
                DashboardViewItem dvsampleids = ((DetailView)cv).FindItem("SampleIDs") as DashboardViewItem;
                if (dvsampleids != null && dvsampleids.InnerView != null)
                {
                    dvsampleids.InnerView.ObjectSpace.Refresh();
                }
            }
        }

        private void btnResetbottles_BottleAllocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                List<COCSettingsBottleAllocation> lstBottle = uow.Query<COCSettingsBottleAllocation>().Where(i => i.COCSettingsRegistration != null && i.COCSettingsRegistration.COCID.COC_ID == COCsr.strCOCID /*&& i.SignOffBy == null*/).ToList();
                foreach (var sample in lstBottle.GroupBy(a => new { a.COCSettingsRegistration, a.TestMethod.Oid }))
                {
                    IList<COCSettingsBottleAllocation> bottles = lstBottle.Where(a => a.TestMethod.Oid == sample.Key.Oid && a.COCSettingsRegistration.Oid == sample.Key.COCSettingsRegistration.Oid).OrderBy(a => a.BottleID).ToList();
                    if (bottles.Count == 1)
                    {
                        bottles[0].BottleID = "A";
                        bottles[0].COCSettingsRegistration.Qty = 1;
                        bottles[0].Containers = null;
                        bottles[0].Preservative = null;
                        bottles[0].StorageID = null;
                        bottles[0].StorageCondition = null;
                    }
                    else if (bottles.Count > 1)
                    {
                        foreach (COCSettingsBottleAllocation allocation in bottles.ToList())
                        {
                            if (allocation == bottles.First())
                            {
                                allocation.BottleID = "A";
                                allocation.COCSettingsRegistration.Qty = 1;
                                allocation.Containers = null;
                                allocation.Preservative = null;
                                allocation.StorageID = null;
                                allocation.StorageCondition = null;
                            }
                            else
                {
                                uow.Delete(allocation);
                            }
                        }
                    }
                }
                uow.CommitChanges();
                View.ObjectSpace.Refresh();
                samplegridrefresh();
                Application.ShowViewStrategy.ShowMessage("Bottles reset successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void btnCopybottles_BottleAllocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                if (cv != null)
                {
                    DashboardViewItem dvsampleids = ((DetailView)cv).FindItem("SampleIDs") as DashboardViewItem;
                    if (dvsampleids != null && dvsampleids.InnerView != null)
                    {
                        if (((ListView)dvsampleids.InnerView).CollectionSource.GetCount() > 1)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            NPCOCSettingsSample_Bottle btle = os.CreateObject<NPCOCSettingsSample_Bottle>();
                            DetailView dv = Application.CreateDetailView(os, btle);
                            dv.ViewEditMode = ViewEditMode.Edit;
                            ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView.Caption = "Copy Bottle Allocation";
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += CopyToSampleIDs_AcceptAction_Execute;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            COCsr.Ispopup = false;
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Add atleast two sampleID's", InformationType.Warning, timer.Seconds, InformationPosition.Top);
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

        private void CopyToSampleIDs_AcceptAction_Execute(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DialogController dialog = (DialogController)sender;
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    NPCOCSettingsSample_Bottle logIn_Bottle = (NPCOCSettingsSample_Bottle)dialog.Window.View.CurrentObject;
                    List<COCSettingsBottleAllocation> lstcurrentBottle = uow.Query<COCSettingsBottleAllocation>().Where(i => i.COCSettingsRegistration != null && i.COCSettingsRegistration.Oid == logIn_Bottle.From.Oid).ToList();
                    if (logIn_Bottle != null && !string.IsNullOrEmpty(logIn_Bottle.To))
                    {
                        List<COCSettingsBottleAllocation> lstFromBottle = uow.Query<COCSettingsBottleAllocation>().Where(i => i.COCSettingsRegistration != null && i.COCSettingsRegistration.Oid == logIn_Bottle.From.Oid).ToList();
                        foreach (string oid in logIn_Bottle.To.Split(new string[] { "; " }, StringSplitOptions.None))
                        {
                            foreach (COCSettingsBottleAllocation CurFromBottle in lstFromBottle.GroupBy(a => a.TestMethod.Oid).Select(a => a.First()).ToList())
                            {
                                IList<COCSettingsBottleAllocation> objSamplebottle = uow.Query<COCSettingsBottleAllocation>().Where(a => a.COCSettingsRegistration.Oid == new Guid(oid) && a.TestMethod.Oid == CurFromBottle.TestMethod.Oid).OrderBy(a => a.BottleID).ToList();
                                if (objSamplebottle.Count > 0)
                                {
                                    IList<COCSettingsBottleAllocation> bottles = lstFromBottle.Where(a => a.TestMethod.Oid == CurFromBottle.TestMethod.Oid).OrderBy(a => a.BottleID).ToList();
                                    if (bottles.Count == 1)
                                    {
                                        objSamplebottle[0].BottleID = CurFromBottle.BottleID;
                                        if (CurFromBottle.Containers != null)
                                        {
                                            objSamplebottle[0].Containers = uow.GetObjectByKey<Container>(CurFromBottle.Containers.Oid);
                                        }
                                        if (CurFromBottle.Preservative != null)
                                        {
                                            objSamplebottle[0].Preservative = uow.GetObjectByKey<Preservative>(CurFromBottle.Preservative.Oid);
                                        }
                                        if (CurFromBottle.StorageID != null)
                                        {
                                            objSamplebottle[0].StorageID = uow.GetObjectByKey<Storage>(CurFromBottle.StorageID.Oid);
                                        }
                                        if (CurFromBottle.StorageCondition != null)
                                        {
                                            objSamplebottle[0].StorageCondition = uow.GetObjectByKey<PreserveCondition>(CurFromBottle.StorageCondition.Oid);
                                        }
                                        objSamplebottle[0].COCSettingsRegistration.Qty = CurFromBottle.COCSettingsRegistration.Qty;

                                    }
                                    else if (bottles.Count > 1)
                                    {
                                        foreach (COCSettingsBottleAllocation bottle in bottles)
                            {
                                            if (bottles.First() == bottle)
                                {
                                                objSamplebottle[0].BottleID = bottle.BottleID;
                                                if (CurFromBottle.Containers != null)
                                    {
                                                    objSamplebottle[0].Containers = uow.GetObjectByKey<Container>(CurFromBottle.Containers.Oid);
                                    }
                                                if (CurFromBottle.Preservative != null)
                                    {
                                                    objSamplebottle[0].Preservative = uow.GetObjectByKey<Preservative>(CurFromBottle.Preservative.Oid);
                                                }
                                                if (CurFromBottle.StorageID != null)
                                                {
                                                    objSamplebottle[0].StorageID = uow.GetObjectByKey<Storage>(CurFromBottle.StorageID.Oid);
                                                }
                                                if (CurFromBottle.StorageCondition != null)
                                                {
                                                    objSamplebottle[0].StorageCondition = uow.GetObjectByKey<PreserveCondition>(CurFromBottle.StorageCondition.Oid);
                                                }
                                            }
                                            else
                                            {
                                                COCSettingsBottleAllocation objbottle = uow.FindObject<COCSettingsBottleAllocation>(CriteriaOperator.Parse("[COCSettingsRegistration.Oid] = ? and [TestMethod.Oid] = ? And [BottleID]=?", objSamplebottle[0].COCSettingsRegistration.Oid, objSamplebottle[0].TestMethod.Oid, bottle.BottleID, true));
                                                if (objbottle == null)
                                                {
                                                    COCSettingsBottleAllocation objNewBottle = new COCSettingsBottleAllocation(uow);
                                                    objNewBottle.BottleID = bottle.BottleID;
                                                    objNewBottle.COCSettingsRegistration = uow.GetObjectByKey<COCSettingsSamples>(objSamplebottle[0].COCSettingsRegistration.Oid);
                                                    objNewBottle.TestMethod = uow.GetObjectByKey<TestMethod>(objSamplebottle[0].TestMethod.Oid);
                                                    if (CurFromBottle.Containers != null)
                                                    {
                                                        objNewBottle.Containers = uow.GetObjectByKey<Container>(CurFromBottle.Containers.Oid);
                                                    }
                                                    if (CurFromBottle.Preservative != null)
                                                    {
                                                        objNewBottle.Preservative = uow.GetObjectByKey<Preservative>(CurFromBottle.Preservative.Oid);
                                    }
                                                    if (CurFromBottle.StorageID != null)
                                    {
                                                        objNewBottle.StorageID = uow.GetObjectByKey<Storage>(CurFromBottle.StorageID.Oid);
                                    }
                                                    if (CurFromBottle.StorageCondition != null)
                                    {
                                                        objNewBottle.StorageCondition = uow.GetObjectByKey<PreserveCondition>(CurFromBottle.StorageCondition.Oid);
                                                    }
                                                }
                                            }
                                        }
                                        objSamplebottle[0].COCSettingsRegistration.Qty = CurFromBottle.COCSettingsRegistration.Qty;
                                    }
                                }
                            }
                        }
                        uow.CommitChanges();
                        samplegridrefresh();
                        Application.ShowViewStrategy.ShowMessage("Bottle set copied successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_Committing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "SampleBottleAllocation_ListView_COCSettings" && IsQtyChanged == true && IsbtlIDchanged == false)
                {
                    //if (IsAdd == false)
                    //{
                    //    BottleIDUpdate();
                    //    IsQtyChanged = false;
                    //}
                    IsAdd = false;

                }
                else if (IsbtlIDchanged == true && IsBtlRemove == false)
                {
                    e.Cancel = true;
                    ObjectSpace.Refresh();
                    IsbtlIDchanged = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void View_VisualMat_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "VisualMatrix_ListView_COCSettings_SampleMatrix")
                {
                    VisualMatrix objvm = (VisualMatrix)View.CurrentObject;
                    if (objvm != null)
                    {
                        COCInfo.visualmatrixGuid = objvm.Oid;
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        gridListEditor.Grid.Load += Grid_Load;
                        DashboardViewItem lvSampleID = ((DashboardView)Application.MainWindow.View).FindItem("SampleID") as DashboardViewItem;
                        {
                            if (lvSampleID != null && lvSampleID.InnerView != null)
                            {
                                ((ListView)lvSampleID.InnerView).CollectionSource.Criteria.Clear();
                                ((ListView)lvSampleID.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[COC.Oid] = ? And[VisualMatrix.Oid] = ?", COCInfo.COCOid, COCInfo.visualmatrixGuid);//CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", objsmplID.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                //COCInfo.isSampleloop = true;
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
        private void View_Sampling_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "COCSettingsSamples_ListView_COCBottle"/* && COCInfo.isSampleloop == true*/)
                {
                    //COCSettingsSamples objsmpling = (COCSettingsSamples)View.CurrentObject;
                    //if (objsmpling != null)
                    //{
                    //    COCInfo.SamplingGuid = objsmpling.Oid;
                    //    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //    ASPxGridView gv = (ASPxGridView)gridListEditor.Grid;
                    //    gridListEditor.Grid.Load += Grid_Load;
                    //    //gridListEditor.Grid.Selection.SelectRowByKey(SInfo.SamplingGuid);
                    //    gridListEditor.Grid.ClientSideEvents.Init = @"function(s, e) {
                    //        window.setTimeout(function() { s.Refresh(); }, 20); }";
                    //    DashboardViewItem DVBotallocation = ((DashboardView)Application.MainWindow.View).FindItem("BottleAllocation") as DashboardViewItem;
                    //    if (DVBotallocation != null && DVBotallocation.InnerView != null)
                    //    {
                    //        ((ListView)DVBotallocation.InnerView).CollectionSource.Criteria.Clear();
                    //        ((ListView)DVBotallocation.InnerView).CollectionSource.Criteria["criteria"] = new InOperator("COCSettings.Oid", COCInfo.SamplingGuid);
                    //        //COCInfo.isSampleloop = false;
                    //    }
                    //    View.Refresh();
                    //}
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
                if (View != null && View.Id == "COCSettingsBottleAllocation_ListView_Copy_Sampleregistration")
                {
                    WebWindow.CurrentRequestWindow.RegisterClientScript("SampleBottleAllocationRefresh", "RaiseXafCallback(globalCallbackControl, 'SamplingBottleIDPopup', 'BottleAllocationRefresh', false);");

                    //IsBtlRemove = false;
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
                e.PopupControl.CustomizePopupControl += PopupControl_CustomizePopupControl;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void PopupControl_CustomizePopupControl(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupControlEventArgs e)
        {
            try
            {
                e.PopupControl.ShowMaximizeButton = false;
                e.PopupControl.CloseOnEscape = false;
                if (View is DashboardView && View.Id == "BottleSetupDistributionCOC")
                {
                    e.PopupControl.ShowCloseButton = false;
                }
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
                if (e.PopupFrame.View.Id == "COCTest")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(1200);
                    e.Height = new System.Web.UI.WebControls.Unit(648);
                    e.Handled = true;
                }
                if (e.PopupFrame.View.Id == "COCBottleSetup_ListView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(1330);
                    e.Height = new System.Web.UI.WebControls.Unit(600);
                    e.Handled = true;
                }
                if (e.PopupFrame.View.Id == "DummyClass_ListView_COCSettingsSample")
                {
                    COCsr.Ispopup = true;
                }
                if (e.PopupFrame.View.Id == "DummyClass_ListView_COCSettings")
                {
                    COCInfo.Ispopup = true;
                }
                if (e.PopupFrame.View.Id == "TestMethod_ListView_COCBA_Popup")
                {
                    COCInfo.Ispopup = true;
                }
                if (e.PopupFrame.View.Id == "SampleBottleAllocation_DetailView")
                {
                    COCInfo.Ispopup = true;
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
                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesController"] = false;
                if (View.Id == "COCSettingsBottleAllocation_ListView_Copy_Sampleregistration")
                {
                    BottleDelcallbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    BottleDelcallbackManager.CallbackManager.RegisterHandler("CanDeleteTaskBottleAllocation", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = gridListEditor.Grid;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Visible;
                    gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 270;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.ClientInstanceName = "sampleid";
                    BottleDelcallbackManager.CallbackManager.RegisterHandler("SamplingBottleIDPopup", this);
                    BottleDelcallbackManager.CallbackManager.RegisterHandler("SamplingBottleSelection", this);
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                    gridListEditor.Grid.BatchUpdate += Grid_BatchUpdate;
                    gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                    gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;



                    gridListEditor.Grid.Load += Grid_Load;
                    if (objPermissionInfo.SampleBottleIsRead == true)
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) 
                        {
                           e.cancel = true;
                        }";


                    }
                    gridListEditor.Grid.JSProperties["cpVisibleRowCount"] = gridListEditor.Grid.VisibleRowCount;
                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                           {
                                if(sessionStorage.getItem('CurrFocusedColumn') == null)
                                {
                                    sessionStorage.setItem('PrevFocusedColumn', e.cellInfo.column.fieldName);
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }
                                else
                                {
                                    var precolumn = sessionStorage.getItem('CurrFocusedColumn');
                                    sessionStorage.setItem('PrevFocusedColumn', precolumn);                           
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }                                 
                           }";
                    gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s, e) { 
                    if (s.IsRowSelectedOnPage(e.elementIndex)) 
                    { 
                         var FocusedColumn = sessionStorage.getItem('CurrFocusedColumn');                                
                         var oid;
                         var text;

                         //console.log('FocusedColumn:', FocusedColumn);

                         if (FocusedColumn && FocusedColumn.includes('.')) 
                         {                                       
                              oid = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn, false);
                              text = s.batchEditApi.GetCellTextContainer(e.elementIndex, FocusedColumn).innerText;                                                     
                              //console.log('oid:', oid);
                              //console.log('text:', text);

                              if (e.item.name == 'CopyToAllCell')
                               { 
                                  if (FocusedColumn=='Containers.Oid' || FocusedColumn=='Preservative.Oid' || FocusedColumn=='StorageID.Oid')
	                              {
                                 
                                    for (var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                    { 
                                       if (s.IsRowSelectedOnPage(i))
                                       {                                               
                                         s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                                       }                                            
                                    }
                                   //console.log('CopyValue:', FocusedColumn);

                                 }
                                 
                               }        
                         } 
                         else if (FocusedColumn) 
                         {                                                             
                            var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn);                            
                            //console.log('CopyValue:', CopyValue);

                            if (e.item.name == 'CopyToAllCell')
                             {
                                 if (FocusedColumn=='Containers.Oid' || FocusedColumn=='Preservative.Oid' || FocusedColumn=='StorageID.Oid')
	                             {
                                   for (var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                   { 
                                     if (s.IsRowSelectedOnPage(i)) 
                                     {
                                        s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                                     }
                                   }
                                 }
                                //console.log('CopyValue:', FocusedColumn);
                                
                              }                            
                          }
                    
                    }
                     e.processOnServer = false;
                     }";

                }
                if (View != null && (View.Id == "COCSettings_DetailView" || View.Id == "COCSettings_DetailView_Copy"))
                {
                    if (View.Id == "COCSettings_DetailView")
                    {
                        COCSettings objcocSettings = (COCSettings)View.CurrentObject;
                        if (objcocSettings != null)
                        {
                            COCInfo.strcocID = objcocSettings.COC_ID;
                        }
                        objPermissionInfo.COCSettingsViewEditMode = ((DetailView)View).ViewEditMode;
                    }
                }
                if (View.Id == "COCBottleSetup_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    ((ListView)View).CurrentObject = null;
                    if (((ListView)View).CollectionSource.GetCount() > 0)
                    {
                        bool distributevisible = false;
                        //foreach (COCBottleSetup bottle in ((ListView)View).CollectionSource.List)
                        //{
                        //    if (bottle.IsDuplicate)
                        //    {
                        //        distributevisible = true;
                        //    }
                        //}
                        //COCDistribution.Enabled["Enabled"] = distributevisible;
                    }
                    else
                    {
                        COCDistribution.Enabled["Enabled"] = false;
                    }
                    ASPxSplitter splitter = ((Control)View.Control).Controls[0] as ASPxSplitter;
                    if (splitter != null)
                    {
                        foreach (SplitterPane pane in splitter.Panes)
                        {
                            pane.AutoWidth = false;
                            if (pane.Index == 0)
                            {
                                pane.Size = new System.Web.UI.WebControls.Unit(71, UnitType.Percentage);
                            }
                            else
                            {
                                pane.Size = new System.Web.UI.WebControls.Unit(29, UnitType.Percentage);
                            }
                        }
                    }
                    //if (gridListEditor != null)
                    //{
                    //    //gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                    //    //gridListEditor.Grid.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    //    foreach (WebColumnBase column in gridListEditor.Grid.Columns)
                    //    {
                    //        //IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                    //        if (column.Name == "SampleContainer")
                    //        {
                    //            //IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                    //            column.Width = System.Web.UI.WebControls.Unit.Pixel(200);
                    //        }
                    //    }
                    //}
                }
                else if (View.Id == "TestMethod_LookupListView_COCBottlesetup")
                {
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(TestMethod)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[MatrixName.MatrixName] =? And [MethodName.Oid] In (" + string.Format("'{0}'", string.Join("','", COCInfo.lstMethod.Select(i => i.ToString().Replace("'", "''")))) + ") And [TestName] In (" + string.Format("'{0}'", string.Join("','", COCInfo.lstTest.Select(i => i.Replace("'", "''")))) + ")", COCInfo.strMatrix);
                        lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestName", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", groups);
                    }
                }
                else if (View.Id == "COCBottleSetup_Test_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(TestMethod)))
                    {
                        lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestName", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", groups);
                    }
                }
                else if (View.Id == "COCBottleSetup_DetailView")
                {
                    if (objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.View)
                    {
                        View.AllowEdit["Forced"] = false;
                    }
                }
                else if (View.Id == "COCBottlesetupdistribution_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor.Grid != null)
                    {
                        ICallbackManagerHolder Duplicate = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        Duplicate.CallbackManager.RegisterHandler("Qty", this);
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) { if(e.focusedColumn.fieldName == 'SampleID') { e.cancel = true; } }";
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s,e) { 
                        window.setTimeout(function() {  
                            if(s.batchEditApi.HasChanges(e.visibleIndex)) 
                            {
                                var totqty = 0;
                                for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                {
                                    totqty = totqty + s.batchEditApi.GetCellValue(i, 'Qty');
                                }
                                var value = s.batchEditApi.GetCellValue(e.visibleIndex, 'SampleID') + '|' + s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty') +'|' + totqty;
                                s.batchEditApi.ResetChanges(e.visibleIndex);     
                                RaiseXafCallback(globalCallbackControl, 'Qty', value, '', false);
                            }
                         }, 10); } ";
                    }
                }
                if (View.Id == "SampleBottleAllocation_ListView_COCSettings")
                {
                    if (IsbtlIDchanged == true)
                    {
                        IsbtlIDchanged = false;
                    }
                    BottleDelcallbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    BottleDelcallbackManager.CallbackManager.RegisterHandler("CanDeleteTaskBottleAllocation", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 270;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.ClientInstanceName = "sampleid";
                    //XafCallbackManager parameter = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    BottleDelcallbackManager.CallbackManager.RegisterHandler("COCBottleIDPopup", this);
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');
                        if(nav != null && sep != null) {
                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                           s.SetWidth((totusablescr / 100) * 70); 
                        }
                        else {
                            s.SetWidth(800); 
                        }                        
                    }";
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            s.timerHandle = setTimeout(function() {  
                                 if (s.batchEditApi.HasChanges()) {  
                                   s.UpdateEdit();  
                                 } 
                               }, 20);}";
                    if (objPermissionInfo.COCBottleIsRead == true)
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) 
                    { if(e.focusedColumn.fieldName == 'BottleSet' || e.focusedColumn.fieldName == 'SharedTests' ||e.focusedColumn.fieldName == 'Qty' ||e.focusedColumn.fieldName == 'BottleID' ||e.focusedColumn.fieldName == 'Containers'||e.focusedColumn.fieldName == 'Preservative') 
                        { e.cancel = true;  
                        } 
                    }";
                    }
                }
                if (View.Id == "COCSettingsSamples_ListView_COCBottle")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectByRowClick = true;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.ClientInstanceName = "bottleallocation";
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');
                        if(nav != null && sep != null) {
                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                           s.SetWidth((totusablescr / 100) * 16); 
                        }
                        else {
                        s.SetWidth(200); 
                        }                        
                    }";
                    gridListEditor.Grid.Load += Grid_Load;
                    //if (((ListView)View).CollectionSource.List.Count == 1)
                    //{
                    //    foreach (COCSettingsSamples objcocsamples in ((ListView)View).CollectionSource.List)
                    //    {
                    //        COCInfo.SamplingGuid = objcocsamples.Oid;
                    //    }
                    //    COCSettingsSamples objsampling = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.SamplingGuid));
                    //    IList<COCSettingsTest> lstsamplingtest = ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[Samples.Oid] = ?", objsampling.Oid));
                    //    DashboardViewItem lvBottleAllocation = ((DashboardView)Application.MainWindow.View).FindItem("BottleAllocation") as DashboardViewItem;
                    //    if (lvBottleAllocation != null && lvBottleAllocation.InnerView != null)
                    //    {
                    //        IList<SampleBottleAllocation> lstsmplbotallow = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[COCSettings.Oid] = ?", objsampling.Oid));
                    //        if (lstsmplbotallow != null && lstsmplbotallow.Count > 0)
                    //        {
                    //            List<Guid> lstsmplallocation = new List<Guid>();
                    //            foreach (SampleBottleAllocation objsamplingallocation in lstsmplbotallow.ToList())
                    //            {
                    //                if (!lstsmplallocation.Contains(objsamplingallocation.Oid))
                    //                {
                    //                    lstsmplallocation.Add(objsamplingallocation.Oid);
                    //                }
                    //            }
                    //            ((ListView)lvBottleAllocation.InnerView).CollectionSource.Criteria.Clear();
                    //            ((ListView)lvBottleAllocation.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", lstsmplallocation.Select(i => i.ToString().Replace("'", "''")))) + ")");
                    //            ObjectSpace.Refresh();
                    //        }
                    //        else
                    //        {
                    //            ((ListView)lvBottleAllocation.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is Null");
                    //        }
                    //    }

                    //}
                    //else
                    //{
                    //    if (samplingfirstdefault == true)
                    //    {
                    //        COCSettingsSamples objsmpl = ((ListView)View).CollectionSource.List.Cast<COCSettingsSamples>().FirstOrDefault();
                    //        if (objsmpl != null)
                    //        {
                    //            COCInfo.SamplingGuid = objsmpl.Oid;
                    //            DashboardViewItem DVBotallocation = ((DashboardView)Application.MainWindow.View).FindItem("BottleAllocation") as DashboardViewItem;
                    //            if (DVBotallocation != null && DVBotallocation.InnerView != null)
                    //            {
                    //                ((ListView)DVBotallocation.InnerView).CollectionSource.Criteria.Clear();
                    //                ((ListView)DVBotallocation.InnerView).CollectionSource.Criteria["criteria"] = new InOperator("COCSettings.Oid", objsmpl.Oid);
                    //            }
                    //        }
                    //        samplingfirstdefault = false;
                    //    }
                    //    //BottleDelcallbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    //    //BottleDelcallbackManager.CallbackManager.RegisterHandler("SampleIDHandler", this);
                    //    //gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e)
                    //    //                                                            {
                    //    //                                                              if(e.visibleIndex != '-1')
                    //    //                                                              {
                    //    //                                                                s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                    //    //                                                                if (s.IsRowSelectedOnPage(e.visibleIndex)) {   
                    //    //                                                                    var value = 'SampleIDHandler|Selected|' + Oidvalue;
                    //    //                                                                    RaiseXafCallback(globalCallbackControl, 'SampleIDHandler', value, '', false);    
                    //    //                                                                }else{
                    //    //                                                                    var value = 'ProductTemplateselection|UNSelected|' + Oidvalue;
                    //    //                                                                    RaiseXafCallback(globalCallbackControl, 'SampleIDHandler', value, '', false);    
                    //    //                                                                }
                    //    //                                                              });
                    //    //                                                             }
                    //    //                                                            }";
                    //}
                }
                if (View.Id == "VisualMatrix_ListView_COCSettings_SampleMatrix")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectByRowClick = true;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.Load += Grid_Load;
                    if (((ListView)View).CollectionSource.List.Count == 1)
                    {
                        foreach (VisualMatrix objvisualmat in ((ListView)View).CollectionSource.List)
                        {
                            COCInfo.visualmatrixGuid = objvisualmat.Oid;
                        }
                        COCSettings objTasks = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("COC_ID=" + COCInfo.strcocID));
                        //IList<COCSettingsSamples> lstsampling = ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("[COC.Oid] = ? And[VisualMatrix.Oid] = ?", objTasks.Oid, COCInfo.visualmatrixGuid));
                        //DashboardViewItem lvSampleID = ((DashboardView)Application.MainWindow.View).FindItem("SampleID") as DashboardViewItem;
                        //if (lstsampling != null && lstsampling.Count > 0)
                        //{
                        //    if (lvSampleID != null && lvSampleID.InnerView != null)
                        //    {
                        //        List<Guid> objsmplID = new List<Guid>();
                        //        foreach (COCSettingsSamples objsampling in lstsampling.ToList())
                        //        {
                        //            if (objsampling.SampleID != null)
                        //            {
                        //                if (!objsmplID.Contains(objsampling.Oid))
                        //                {
                        //                    objsmplID.Add(objsampling.Oid);
                        //                }
                        //            }
                        //        }
                        //        ((ListView)lvSampleID.InnerView).CollectionSource.Criteria.Clear();
                        //        ((ListView)lvSampleID.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", objsmplID.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        //        ObjectSpace.Refresh();
                        //    }
                        //    else if (lvSampleID != null && lvSampleID.InnerView == null)
                        //    {
                        //        lvSampleID.ControlCreated += DVSampleID_ControlsCreated;
                        //    }
                        //}
                    }
                    else
                    {
                        if (visualmatfirstdefault == true)
                        {
                            if (View != null && ((ListView)View).CollectionSource.List.Count > 0)
                            {
                                VisualMatrix objvm = ((ListView)View).CollectionSource.List.Cast<VisualMatrix>().First();
                                if (objvm != null)
                                {
                                    COCInfo.visualmatrixGuid = objvm.Oid;
                                }
                                //IList<COCSettingsSamples> lstsampling = ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("[COC.Oid] = ? And[VisualMatrix.Oid] = ?", COCInfo.COCOid, COCInfo.visualmatrixGuid));
                                //DashboardViewItem lvSampleID = ((DashboardView)Application.MainWindow.View).FindItem("SampleID") as DashboardViewItem;
                                //if (lstsampling != null && lstsampling.Count > 0)
                                //{
                                //    if (lvSampleID != null && lvSampleID.InnerView != null && lvSampleID.InnerView.Id == "COCSettingsSamples_ListView_COCBottle")
                                //    {
                                //        List<Guid> objsmplID = new List<Guid>();
                                //        foreach (COCSettingsSamples objsampling in lstsampling.ToList())
                                //        {
                                //            if (objsampling.SampleID != null)
                                //            {
                                //                if (!objsmplID.Contains(objsampling.Oid))
                                //                {
                                //                    objsmplID.Add(objsampling.Oid);
                                //                }
                                //            }
                                //        }
                                //        ((ListView)lvSampleID.InnerView).CollectionSource.Criteria.Clear();
                                //        ((ListView)lvSampleID.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", objsmplID.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                //        ObjectSpace.Refresh();
                                //    }
                                //    else if (lvSampleID != null && lvSampleID.InnerView == null)
                                //    {
                                //        lvSampleID.ControlCreated += DVSampleID_ControlsCreated;
                                //    }
                                //    visualmatfirstdefault = false;
                                //}
                            }
                        }
                        //BottleDelcallbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        //BottleDelcallbackManager.CallbackManager.RegisterHandler("SampleMatrixHandler", this);
                        //gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e)
                        //                                                            {
                        //                                                              if(e.visibleIndex != '-1')
                        //                                                              {
                        //                                                                s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                        //                                                                if (s.IsRowSelectedOnPage(e.visibleIndex)) {   
                        //                                                                    var value = 'SampleMatrixHandler|Selected|' + Oidvalue;
                        //                                                                    RaiseXafCallback(globalCallbackControl, 'SampleMatrixHandler', value, '', false);    
                        //                                                                }else{
                        //                                                                    var value = 'ProductTemplateselection|UNSelected|' + Oidvalue;
                        //                                                                    RaiseXafCallback(globalCallbackControl, 'SampleMatrixHandler', value, '', false);    
                        //                                                                }
                        //                                                              });
                        //                                                             }
                        //                                                            }";
                    }

                }
                if (View.Id == "DummyClass_ListView_COCSettings")
                {
                    List<string> lststrbottle = new List<string>();
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //COCSettingsSamples objsampling = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.SamplingGuid));
                    //gridListEditor.Grid.HtmlCommandCellPrepared += GridView_HtmlCommandCellPrepared;
                    ////SInfo.strbottleid = objsmplallo.BottleID;
                    //IList<SampleBottleAllocation> lstsmplallocation = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[COCSettings.Oid] = ?", objsampling.Oid));
                    //foreach (SampleBottleAllocation objsmp in lstsmplallocation.ToList())
                    //{
                    //    if (objsmp != null && objsmp.BottleID != null)
                    //    {
                    //        string[] bottleid = objsmp.BottleID.Split(',');

                    //        foreach (string strbottle in bottleid)
                    //        {
                    //            if (!lststrbottle.Contains(strbottle))
                    //            {
                    //                lststrbottle.Add(strbottle);
                    //            }
                    //        }
                    //    }

                    //}
                    BottleDelcallbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    BottleDelcallbackManager.CallbackManager.RegisterHandler("COCDummyClass", this);
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                            RaiseXafCallback(globalCallbackControl, 'COCDummyClass', 'Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'COCDummyClass', 'UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                        RaiseXafCallback(globalCallbackControl, 'COCDummyClass', 'Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'COCDummyClass', 'UNSelectall', '', false);                        
                      }                      
                    }";
                    //if (COCInfo.Ispopup == true)
                    //{
                    //    int cntbottleid = 0;
                    //    int strbottleid = 0;
                    //    List<string> lstbtlid = new List<string>();
                    //    COCInfo.lstbottleid = new List<DummyClass>();
                    //    int strbottleqty = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(SampleBottleAllocation), CriteriaOperator.Parse("SUM(Qty)"), CriteriaOperator.Parse("[COCSettings.Oid]=?", objsampling.Oid))));
                    //    if (strbottleqty > 0)
                    //    {
                    //        IList<SampleBottleAllocation> lstsmplbtl = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[COCSettings.Oid]=?", objsampling.Oid));
                    //        foreach (SampleBottleAllocation objsmplbtl in lstsmplbtl.ToList())
                    //        {
                    //            if (objsampling != null && objsmplbtl.BottleID != null)
                    //            {
                    //                string[] arystrbottleid = objsmplbtl.BottleID.Split(',');
                    //                strbottleid = strbottleid + arystrbottleid.Count();
                    //                foreach (string strboltid in arystrbottleid)
                    //                {
                    //                    if (!string.IsNullOrEmpty(strboltid))
                    //                    {
                    //                        lstbtlid.Add(strboltid.Trim());
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        if (strbottleid == strbottleqty)
                    //        {
                    //            const string letterseql = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    //            string valueeql = "";
                    //            for (int i = 0; i <= strbottleqty - 1; i++)
                    //            {
                    //                valueeql = "";
                    //                if (i >= letterseql.Length)
                    //                    valueeql += letterseql[i / letterseql.Length - 1];

                    //                valueeql += letterseql[i % letterseql.Length];
                    //                if (lstbtlid.Contains(valueeql))
                    //                {
                    //                    DummyClass objdc = ObjectSpace.CreateObject<DummyClass>();
                    //                    COCInfo.lstbottleid.Add(objdc);
                    //                    objdc.Name = valueeql.ToString();
                    //                    ((ListView)View).CollectionSource.Add(objdc);
                    //                }
                    //                else
                    //                {
                    //                    strbottleqty = strbottleqty + 1;
                    //                    continue;
                    //                }
                    //            }
                    //        }
                    //        else if (strbottleqty > strbottleid)
                    //        {
                    //            if (lstbtlid.Count > 0)
                    //            {
                    //                foreach (string objsmplbtlid in lstbtlid)
                    //                {
                    //                    DummyClass objdc = ObjectSpace.CreateObject<DummyClass>();
                    //                    COCInfo.lstbottleid.Add(objdc);
                    //                    objdc.Name = objsmplbtlid.ToString();
                    //                    ((ListView)View).CollectionSource.Add(objdc);
                    //                }
                    //            }
                    //            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    //            string value = "";
                    //            for (int i = 0; i <= strbottleqty - lstbtlid.Count - 1; i++)
                    //            {
                    //                value = "";
                    //                if (i >= letters.Length)
                    //                {
                    //                    value += letters[i / letters.Length - 1];
                    //                }

                    //                value += letters[i % letters.Length];
                    //                if (!lstbtlid.Contains(value))
                    //                {
                    //                    DummyClass objdc = ObjectSpace.CreateObject<DummyClass>();
                    //                    COCInfo.lstbottleid.Add(objdc);
                    //                    objdc.Name = value.ToString();
                    //                    ((ListView)View).CollectionSource.Add(objdc);
                    //                }
                    //                else
                    //                {
                    //                    strbottleqty = strbottleqty + 1;
                    //                    continue;
                    //                }
                    //            }
                    //        }
                    //        gridListEditor.Grid.Load += Grid_Load;
                    //    }
                    //}

                }
                if (View.Id == "DummyClass_ListView_COCSettingsSample")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    BottleDelcallbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    BottleDelcallbackManager.CallbackManager.RegisterHandler("SampleDummyClass", this);
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                            RaiseXafCallback(globalCallbackControl, 'SampleDummyClass', 'Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'SampleDummyClass', 'UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                        RaiseXafCallback(globalCallbackControl, 'SampleDummyClass', 'Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'SampleDummyClass', 'UNSelectall', '', false);                        
                      }                      
                    }";
                    COCSettingsSamples objsampling = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", COCsr.SamplingGuid));
                    if (COCsr.Ispopup == true && objsampling != null)
                    {
                        int strbottleid = 0;
                        List<string> lstbtlid = new List<string>();
                        COCsr.lstbottleid = new List<DummyClass>();
                        int strbottleqty = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(COCSettingsSamples), CriteriaOperator.Parse("SUM(Qty)"), CriteriaOperator.Parse("[Oid]=?", objsampling.Oid))));
                        if (strbottleqty > 0)
                        {
                            IList<COCSettingsBottleAllocation> lstsmplbtl = ObjectSpace.GetObjects<COCSettingsBottleAllocation>(CriteriaOperator.Parse("[COCSettingsRegistration.Oid]=?", objsampling.Oid));
                            foreach (COCSettingsBottleAllocation objsmplbtl in lstsmplbtl.ToList())
                            {
                                if (objsmplbtl != null && objsmplbtl.BottleID != null)
                                {
                                    string[] arystrbottleid = objsmplbtl.BottleID.Split(',');
                                    foreach (string strboltid in arystrbottleid)
                                    {
                                        if (!string.IsNullOrEmpty(strboltid) && !lstbtlid.Contains(strboltid.Trim()))
                                        {
                                            lstbtlid.Add(strboltid.Trim());
                                        }
                                    }
                                }
                            }
                            strbottleid = lstbtlid.Count();
                            if (strbottleid == strbottleqty)
                            {
                                const string letterseql = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                string valueeql = "";
                                for (int i = 0; i <= strbottleqty - 1; i++)
                                {
                                    valueeql = "";
                                    if (i >= letterseql.Length)
                                    {
                                        valueeql += letterseql[i / letterseql.Length - 1];
                                    }
                                    valueeql += letterseql[i % letterseql.Length];
                                    if (lstbtlid.Contains(valueeql))
                                    {
                                        DummyClass objdc = ObjectSpace.CreateObject<DummyClass>();
                                        COCsr.lstbottleid.Add(objdc);
                                        objdc.Name = valueeql.ToString();
                                        ((ListView)View).CollectionSource.Add(objdc);
                                    }
                                    else
                                    {
                                        strbottleqty = strbottleqty + 1;
                                        continue;
                                    }

                                }
                            }
                            else if (strbottleqty > strbottleid)
                            {
                                if (lstbtlid.Count > 0)
                                {
                                    foreach (string objsmplbtlid in lstbtlid)
                                    {
                                        DummyClass objdc = ObjectSpace.CreateObject<DummyClass>();
                                        COCsr.lstbottleid.Add(objdc);
                                        objdc.Name = objsmplbtlid.ToString();
                                        ((ListView)View).CollectionSource.Add(objdc);
                                    }
                                }
                                const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                string value = "";
                                for (int i = 0; i <= strbottleqty - lstbtlid.Count - 1; i++)
                                {
                                    value = "";
                                    if (i >= letters.Length)
                                    {
                                        value += letters[i / letters.Length - 1];
                                    }

                                    value += letters[i % letters.Length];
                                    if (!lstbtlid.Contains(value))
                                    {
                                        DummyClass objdc = ObjectSpace.CreateObject<DummyClass>();
                                        COCsr.lstbottleid.Add(objdc);
                                        objdc.Name = value.ToString();
                                        ((ListView)View).CollectionSource.Add(objdc);
                                    }
                                    else
                                    {
                                        strbottleqty = strbottleqty + 1;
                                        continue;
                                    }
                                }
                            }
                        }
                    }

                }

                //if (View.Id == "TestMethod_ListView_COCBA_Popup")
                //{
                //    if (COCInfo.Ispopup == true)
                //    {
                //        List<object> groups = new List<object>();
                //        List<Guid> lsttestmethodguid = new List<Guid>();
                //        List<string> lsttestname = new List<string>();
                //        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //        string[] arrsmpltest = null;
                //        IList<SampleBottleAllocation> lstsmplaloca = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[COCSettings.Oid] = ?", COCInfo.SamplingGuid));
                //        if (lstsmplaloca != null && lstsmplaloca.Count > 0)
                //        {
                //            string strsmpltst = string.Empty;
                //            List<object> lststrsmpltst = new List<object>();
                //            foreach (SampleBottleAllocation objsmplbtl in lstsmplaloca.ToList())
                //            {
                //                if (objsmplbtl != null && objsmplbtl.SharedTestsGuid != null)
                //                {
                //                    if (string.IsNullOrEmpty(strsmpltst))
                //                    {
                //                        strsmpltst = objsmplbtl.SharedTestsGuid;
                //                    }
                //                    else
                //                    {
                //                        strsmpltst = strsmpltst + ";" + objsmplbtl.SharedTestsGuid;
                //                    }
                //                }
                //            }
                //            arrsmpltest = strsmpltst.Split(';');
                //            foreach (string objtestguid in arrsmpltest.ToList())
                //            {
                //                if (!string.IsNullOrEmpty(objtestguid))
                //                {
                //                    if (lsttestmethodguid.Contains(new Guid(objtestguid.Trim())))
                //                    {
                //                        lsttestmethodguid.Remove(new Guid(objtestguid.Trim()));
                //                    }
                //                }
                //            }
                //        }

                //        string[] arrstrtest = null;
                //        IObjectSpace ossmpl = Application.CreateObjectSpace(typeof(SampleBottleAllocation));
                //        COCInfo.lstsmplbtlalloGuid = new Guid(HttpContext.Current.Session["rowid"].ToString());
                //        if (COCInfo.lstsmplbtlalloGuid != Guid.Empty)
                //        {
                //            SampleBottleAllocation objsmplbtl = ossmpl.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.lstsmplbtlalloGuid));
                //            if (objsmplbtl != null && !string.IsNullOrEmpty(objsmplbtl.SharedTestsGuid))
                //            {
                //                arrstrtest = objsmplbtl.SharedTestsGuid.Split(';');
                //            }
                //            IObjectSpace ostst = Application.CreateObjectSpace(typeof(TestMethod));
                //            COCInfo.lstavailtest = new List<string>();
                //            if (arrstrtest != null && arrstrtest.Length > 0)
                //            {
                //                foreach (string strtest in arrstrtest.ToList())
                //                {
                //                    TestMethod objtm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strtest.Trim())));
                //                    if (objtm != null && objtm.TestName != null)
                //                    {
                //                        COCInfo.lstavailtest.Add(objtm.TestName.Trim());
                //                        lsttestmethodguid.Add(objtm.Oid);
                //                    }
                //                }
                //            }
                //        }
                //        if (lsttestmethodguid != null && lsttestmethodguid.Count > 0)
                //        {
                //            ((ListView)View).CollectionSource.Criteria.Clear();
                //            ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lsttestmethodguid);
                //        }
                //        else
                //        {
                //            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid Is Null");
                //        }
                //        gridListEditor.Grid.Load += Grid_Load;
                //    }
                //}
                //if (View.Id == "COCSettingsSamples_ListView_COCBottle_CopyTOBottleAllocation")
                //{
                //    List<Guid> lstsmplidguid = new List<Guid>();
                //    DashboardViewItem DVSampleID = ((DashboardView)Application.MainWindow.View).FindItem("SampleID") as DashboardViewItem;
                //    //if (DVSampleID != null && DVSampleID.InnerView != null)
                //    //{
                //    //    foreach (COCSettingsSamples objsmpl in ((ListView)DVSampleID.InnerView).CollectionSource.List)
                //    //    {
                //    //        lstsmplidguid.Add(objsmpl.Oid);
                //    //    }
                //    //    if (lstsmplidguid.Contains(COCInfo.SamplingGuid))
                //    //    {
                //    //        lstsmplidguid.Remove(COCInfo.SamplingGuid);
                //    //    }
                //    //    if (lstsmplidguid.Count > 0)
                //    //    {
                //    //        ((ListView)View).CollectionSource.Criteria.Clear();
                //    //        ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstsmplidguid);
                //    //    }
                //    //    else
                //    //    {
                //    //        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid Is Null");
                //    //    }
                //    //}
                //    //ASPxGridListEditor gridlist = ((ListView)View).Editor as ASPxGridListEditor;
                //    //if (gridlist != null && gridlist.Grid != null)
                //    //{
                //    //    gridlist.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                //    //    BottleDelcallbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                //    //    BottleDelcallbackManager.CallbackManager.RegisterHandler("CopytoCOCSampleID", this);
                //    //    //gridlist.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                //    //    //  if (e.visibleIndex != '-1')
                //    //    //  {
                //    //    //    s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                //    //    //     if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                //    //    //        RaiseXafCallback(globalCallbackControl, 'CopytoCOCSampleID', 'Selected|' + Oidvalue , '', false);    
                //    //    //     }else{
                //    //    //        RaiseXafCallback(globalCallbackControl, 'CopytoCOCSampleID', 'UNSelected|' + Oidvalue, '', false);    
                //    //    //     }
                //    //    //    }); 
                //    //    //  }
                //    //    //  else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                //    //    //  {        
                //    //    //    RaiseXafCallback(globalCallbackControl, 'CopytoCOCSampleID', 'Selectall', '', false);     
                //    //    //  }
                //    //    //  else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                //    //    //  {
                //    //    //    RaiseXafCallback(globalCallbackControl, 'CopytoCOCSampleID', 'UNSelectall', '', false);                        
                //    //    //  }                      
                //    //    //}";
                //    //}                 
                //}
                else if (View.Id == "COCSettingsSamples_ListView_Copy_Bottle")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 310;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                    gridListEditor.Grid.Load += Grid_Load;
                    ICallbackManagerHolder seltest = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    seltest.CallbackManager.RegisterHandler("Bottle", this);
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
                    s.RowClick.ClearHandlers();
                    }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) 
                        { 
                        if(e.focusedColumn.fieldName != 'Qty') 
                        { 
                          e.cancel = true;  
                        } 
                        else
                          {
                              var Qty = s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty');
                              sessionStorage.setItem('valQty', Qty);
                              s.Qty=Qty;
                          }
                        }";
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            s.timerHandle = setTimeout(function() {  
                                 if (s.batchEditApi.HasChanges()) {
                                    //var valqty =  s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty');
                                    //if(valqty <= 0)
                                    //{
                                    //    alert('Qty must be greater than 0');
                                    //    qty = 1;
                                    //    s.batchEditApi.SetCellValue(e.visibleIndex, 'Qty', qty);
                                    //}
                                   s.UpdateEdit();  
                                    var newQty = s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty');
                                   var oldQty = sessionStorage.getItem('valQty');
                                   if(newQty!=null && oldQty!=null && oldQty > newQty)
                                      {
                                           RaiseXafCallback(globalCallbackControl,'Bottle', 'Qty|'+e.visibleIndex+'|'+oldQty+'|'+newQty, '', false);
                                      }
                                 } 
                               }, 20);}";
                    if (!((ListView)View).CollectionSource.Criteria.ContainsKey("matrix"))
                    {
                        ((ListView)View).CollectionSource.Criteria["matrix"] = CriteriaOperator.Parse("[COCID.COC_ID] = ?", COCsr.strCOCID);
                    }
                }
                if (View.Id == "NPCOCSettingsSample_Bottle_DetailView")
                {
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                        {
                            ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                //  propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                            ASPxGridLookup lookup = (ASPxGridLookup)propertyEditor.Editor;
                            if (lookup != null)
                            {
                                lookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                lookup.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                lookup.GridView.Settings.VerticalScrollableHeight = 200;
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
            // Access and customize the target View control.
        }

        private void Grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            try
            {
                foreach (var args in e.UpdateValues)
                {
                    UpdateItem(args.Keys, args.NewValues);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected void UpdateItem(OrderedDictionary keys, OrderedDictionary newValues)
        {
            try
            {
                var id = Convert.ToString(keys["Oid"]);
                var val = Convert.ToString(newValues["NPBottleID"]);
                if (val.Contains(", "))
                {
                    COCSettingsBottleAllocation sample = ((ListView)View).CollectionSource.List.Cast<COCSettingsBottleAllocation>().Where(a => a.Oid == new Guid(id)).First();
                    if (sample != null)
                    {
                        using (IObjectSpace os = Application.CreateObjectSpace())
                        {
                            IList<COCSettingsBottleAllocation> samples = os.GetObjects<COCSettingsBottleAllocation>(CriteriaOperator.Parse("[COCSettingsRegistration.Oid]=? And [TestMethod.Oid]=? And [Oid]<>?", sample.COCSettingsRegistration.Oid, sample.TestMethod.Oid, new Guid(id))).ToList();
                            foreach (COCSettingsBottleAllocation allocation in samples)
                            {
                                allocation.StorageID = newValues["StorageID.Oid"] != null ? os.GetObjectByKey<Storage>(new Guid(newValues["StorageID.Oid"].ToString())) : null;
                                allocation.Containers = newValues["Containers.Oid"] != null ? os.GetObjectByKey<Container>(new Guid(newValues["Containers.Oid"].ToString())) : null;
                                allocation.Preservative = newValues["Preservative.Oid"] != null ? os.GetObjectByKey<Preservative>(new Guid(newValues["Preservative.Oid"].ToString())) : null;
                                allocation.StorageCondition = newValues["StorageCondition.Oid"] != null ? os.GetObjectByKey<PreserveCondition>(new Guid(newValues["StorageCondition.Oid"].ToString())) : null;
                            }
                            os.CommitChanges();
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

        private void GridView_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse("Oid is null"));
                    if (currentLanguage != null && currentLanguage.Chinese)
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
                    GridViewContextMenuItem item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";//"navigation_home_16x16";
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
                if (View.Id == "COCSettingsSamples_ListView_Copy_Bottle")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                        if (cv != null)
                        {
                            DashboardViewItem dvBottleAllocation = ((DetailView)cv).FindItem("COCSettingsBottleAllocation_SampleRegistration") as DashboardViewItem;
                            if (dvBottleAllocation != null && dvBottleAllocation.InnerView != null)
                            {
                                if ((COCSettingsSamples)View.CurrentObject != null)
                                {
                                    COCSettingsSamples logIn = (COCSettingsSamples)View.CurrentObject;
                                    if (logIn != null)
                                    {
                                        ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria.Clear();
                                        List<object> OidTask = new List<object>();
                                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(COCSettingsBottleAllocation)))
                                        {
                                            lstview.Criteria = CriteriaOperator.Parse("[COCSettingsRegistration.Oid] = ?", logIn.Oid);
                                            lstview.Properties.Add(new ViewProperty("group", SortDirection.Ascending, "TestMethod.Oid", true, true));
                                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                            foreach (ViewRecord Vrec in lstview)
                                                OidTask.Add(Vrec["Toid"]);
                                        }
                                        ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", OidTask);
                                        // ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[COCSettingsRegistration.Oid] = ?", logIn.Oid);
                                        ((ListView)dvBottleAllocation.InnerView).Refresh();
                                        if (COCsr.counter == 0)
                                        {
                                            COCsr.SamplingGuid = logIn.Oid;
                                            COCsr.strselSample = logIn.Oid.ToString();
                                        }
                                    }
                                }
                                else if (View.SelectedObjects.Count == 0 || (View.SelectedObjects.Count == gridListEditor.Grid.VisibleRowCount && gridListEditor.Grid.VisibleRowCount != 2))
                                {
                                    ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria.Clear();
                                    ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] is null");
                                    ((ListView)dvBottleAllocation.InnerView).Refresh();
                                    COCsr.SamplingGuid = new Guid();
                                    COCsr.strselSample = null;
                                    COCsr.counter = 0;
                                    COCsr.CanProcess = null;
                                }
                                else if (COCsr.CanProcess == true)
                                {
                                    if (COCsr.counter == 0)
                                    {
                                        COCsr.counter = 1;
                                    }
                                    else
                                    {
                                        COCsr.counter = 0;
                                        COCsr.CanProcess = false;
                                        gridListEditor.Grid.Selection.UnselectRowByKey(COCsr.strselSample);
                                    }
                                }
                                else if (COCsr.CanProcess == false)
                                {
                                    COCSettingsSamples logIn = View.SelectedObjects.Cast<COCSettingsSamples>().Where(a => a.Oid.ToString() != COCsr.strselSample).FirstOrDefault();
                                    if (logIn != null)
                                    {
                                        COCsr.CanProcess = true;
                                        gridListEditor.Grid.Selection.UnselectRowByKey(logIn.Oid);
                                    }
                                }
                                else if (COCsr.CanProcess == null && COCsr.strselSample != null && (COCSettingsSamples)View.CurrentObject == null && View.SelectedObjects.Count > 1)
                                {
                                    if (COCsr.counter == 0)
                                    {
                                        COCsr.counter = 1;
                                    }
                                    else
                                    {
                                        COCsr.counter = 0;
                                        COCsr.CanProcess = true;
                                        gridListEditor.Grid.Selection.UnselectRowByKey(COCsr.strselSample);
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

        //private void SampleAllocationcreation()
        //{
        //    try
        //    {
        //        DashboardViewItem DVBotallocation = ((DashboardView)Application.MainWindow.View).FindItem("BottleAllocation") as DashboardViewItem;
        //        if (DVBotallocation != null && DVBotallocation.InnerView != null)
        //        {
        //            if (((ListView)DVBotallocation.InnerView).CollectionSource.List.Count == 1)
        //            {
        //                foreach (SampleBottleAllocation objsmplbtl in ((ListView)DVBotallocation.InnerView).CollectionSource.List)
        //                {
        //                    if (objsmplbtl.Qty == 1)
        //                    {
        //                        objsmplbtl.BottleID = "A";
        //                        ObjectSpace.CommitChanges();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
        private void DVSampleID_ControlsCreated(object sender, EventArgs e)
        {
            try
            {
                COCSettings objTasks = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("COC_ID=" + COCInfo.strcocID));
                //IList<COCSettingsSamples> lstsampling = ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("[COC.Oid] = ? And[VisualMatrix.Oid] = ?", objTasks.Oid, COCInfo.visualmatrixGuid));
                //DashboardViewItem lvSampleID = ((DashboardView)Application.MainWindow.View).FindItem("SampleID") as DashboardViewItem;
                //if (lstsampling != null && lstsampling.Count > 0)
                //{
                //    if (lvSampleID != null && lvSampleID.InnerView != null)
                //    {
                //        List<Guid> objsmplID = new List<Guid>();
                //        foreach (COCSettingsSamples objsampling in lstsampling.ToList())
                //        {
                //            if (objsampling.SampleID != null)
                //            {
                //                if (!objsmplID.Contains(objsampling.Oid))
                //                {
                //                    objsmplID.Add(objsampling.Oid);
                //                }
                //            }
                //        }
                //        ((ListView)lvSampleID.InnerView).CollectionSource.Criteria.Clear();
                //        ((ListView)lvSampleID.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", objsmplID.Select(i => i.ToString().Replace("'", "''")))) + ")");
                //        ObjectSpace.Refresh();
                //    }
                //}
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
                if (View.Id == "DummyClass_ListView_COCSettingsSample")
                {
                    if (COCsr.lstcrtbottleid != null && COCsr.lstcrtbottleid.Count > 0)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            if (!string.IsNullOrEmpty(gridView.GetRowValues(i, "Name").ToString()))
                            {
                                string strbottleid = gridView.GetRowValues(i, "Name").ToString();
                                if (COCsr.lstcrtbottleid.Contains(strbottleid))
                                {
                                    gridView.Selection.SelectRow(i);
                                }
                            }
                        }
                    }
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                    COCsr.lstcrtbottleid = new List<string>();
                    COCsr.Ispopup = false;
                }
                if (View.Id == "COCSettingsBottleAllocation_ListView_Copy_Sampleregistration")
                {
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                }
                if (View.Id == "VisualMatrix_ListView_COCSettings_SampleMatrix")
                {
                    if (((ListView)View).CollectionSource.List.Count == 1)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            gridView.Selection.SelectRow(i);
                        }
                    }
                    else if (((ListView)View).CollectionSource.List.Count > 1)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            //if (visualmatfirstdefault == true)
                            //{
                            //    i = 1;
                            //    break;
                            //    gridView.Selection.SelectRow(i);
                            //    if (!string.IsNullOrEmpty(gridView.GetRowValues(i, "VisualMatrixName").ToString()))
                            //    {
                            //        string strobjVisualMatrix = gridView.GetRowValues(i, "VisualMatrixName").ToString();
                            //        VisualMatrix objVisualMatrix = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", SInfo.visualmatrixGuid));
                            //        if (objVisualMatrix != null && objVisualMatrix.VisualMatrixName == strobjVisualMatrix)
                            //        {
                            //            SInfo.visualmatrixGuid = objVisualMatrix.Oid;
                            //        }                                        
                            //    }
                            //}
                            //else
                            if (!string.IsNullOrEmpty(gridView.GetRowValues(i, "VisualMatrixName").ToString()))
                            {
                                string strobjVisualMatrix = gridView.GetRowValues(i, "VisualMatrixName").ToString();
                                VisualMatrix objVisualMatrix = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.visualmatrixGuid));
                                if (objVisualMatrix != null && objVisualMatrix.VisualMatrixName == strobjVisualMatrix)
                                {
                                    gridView.Selection.SelectRow(i);
                                }
                            }
                        }
                    }
                }
                if (View.Id == "COCSettingsSamples_ListView_COCBottle")
                {
                    if (((ListView)View).CollectionSource.List.Count == 1)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            gridView.Selection.SelectRow(i);
                        }
                    }
                    else if (((ListView)View).CollectionSource.List.Count > 1)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            if (samplingfirstdefault == true)
                            {
                                i = 1;
                                break;
                                gridView.Selection.SelectRow(i);
                                samplingfirstdefault = false;
                            }
                            else if (!string.IsNullOrEmpty(gridView.GetRowValues(i, "SampleID").ToString()))
                            {
                                //string strbottleid = gridView.GetRowValues(i, "SampleID").ToString();
                                //Sampling objsmpling = ObjectSpace.FindObject<Sampling>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.SamplingGuid));
                                //if (objsmpling != null && objsmpling.SampleID == strbottleid)
                                //{
                                //    gridView.Selection.SelectRow(i);
                                //}
                            }
                        }
                    }
                }
                if (View.Id == "DummyClass_ListView_COCSettings")
                {
                    if (COCInfo.lstcrtbottleid != null && COCInfo.lstcrtbottleid.Count > 0)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            if (!string.IsNullOrEmpty(gridView.GetRowValues(i, "Name").ToString()))
                            {
                                string strbottleid = gridView.GetRowValues(i, "Name").ToString();
                                if (COCInfo.lstcrtbottleid.Contains(strbottleid) && !COCInfo.lstotherbottleid.Contains(strbottleid))
                                {
                                    gridView.Selection.SelectRow(i);
                                }
                            }
                        }
                    }
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                    COCInfo.Ispopup = false;
                }
                if (View.Id == "TestMethod_ListView_COCBA_Popup" && COCInfo.Ispopup == true)
                {
                    COCInfo.Ispopup = false;
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                    //for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                    //{
                    //    gridView.Selection.SelectRow(i);
                    //}
                    if (COCInfo.lstavailtest != null && COCInfo.lstavailtest.Count > 0)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            if (!string.IsNullOrEmpty(gridView.GetRowValues(i, "TestName").ToString()))
                            {
                                string stravailabletest = gridView.GetRowValues(i, "TestName").ToString();
                                //TestMethod objtm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName] = ?", stravailabletest));
                                if (COCInfo.lstavailtest.Contains(stravailabletest))
                                {
                                    gridView.Selection.SelectRow(i);
                                }
                            }
                        }
                    }
                }
                if (View.Id == "TestMethod_ListView_COC_BA")
                {
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                }
                if (View.Id == "SampleBottleAllocation_ListView_COCSettings")
                {
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                }
                if (View.Id == "COCSettingsSamples_ListView_COCBottle_SelectedSampleID")
                {
                    for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                    {
                        gridView.Selection.SelectRow(i);
                    }
                }
                if (View.Id == "COCSettingsSamples_ListView_Copy_Bottle")
                {
                    if (gridView != null)
                    {
                        GridViewCommandColumn selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
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

        private void GridView_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (gridView != null)
                {
                    if (e.CommandCellType == GridViewTableCommandCellType.Data)
                    {
                        if (e.CommandColumn.Name == "SelectionCommandColumn"/* || e.CommandColumn.Name == "InlineEditCommandColumn"*/)
                        {
                            string strbottleid = gridView.GetRowValuesByKeyValue(e.KeyValue, "Name").ToString();
                            if (COCInfo.lstotherbottleid.Contains(strbottleid))
                            {
                                COCInfo.selectionhideGuid.Add(gridView.GetRowValuesByKeyValue(e.KeyValue, "Oid").ToString());
                                ((System.Web.UI.WebControls.WebControl)e.Cell.Controls[0]).Visible = false;
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

        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (objPermissionInfo.COCBottleIsRead == false)
                {
                    if (e.DataColumn.FieldName == "NPBottleID")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'SamplingBottleIDPopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                    //if (e.DataColumn.FieldName == "BottleID")
                    //{
                    //    e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'COCBottleIDPopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    //}

                    //if (e.DataColumn.FieldName == "SharedTests")
                    //{
                    //    e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'COCBottleIDPopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    //}
                }
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
                if ((e.PropertyName == "Bottleid" || e.PropertyName == "BottleID") && e.NewValue != null && View.Id == "COCBottleSetup_ListView")
                {
                    COCInfo.IsBottleidvalid = true;
                    if (((ListView)View).CollectionSource.GetCount() > 0)
                    {
                        //foreach (COCBottleSetup bottle in ((ListView)View).CollectionSource.List)
                        //{
                        //    if (bottle.Bottleid == e.NewValue.ToString() && ((ListView)View).EditView.CurrentObject != null && ((ListView)View).EditView.CurrentObject != bottle)
                        //    {
                        //        COCBottleSetup setup = (COCBottleSetup)((ListView)View).EditView.CurrentObject;
                        //        if (setup.Bottlesharingname == bottle.Bottlesharingname)
                        //        {
                        //            COCInfo.IsBottleidvalid = false;
                        //            WebWindow.CurrentRequestWindow.RegisterClientScript("save", "alert('Bottle ID already exist, please enter new Bottle ID')");
                        //            break;
                        //        }
                        //    }
                        //}
                    }
                }
                //else if (e.PropertyName == "Duplicate" && e.NewValue != null && View.Id == "COCBottleSetup_ListView")
                //{
                //    COCBottleSetup bottle = (COCBottleSetup)((ListView)View).EditView.CurrentObject;
                //    if (bottle.Test.Count == 0)
                //    {
                //        WebWindow.CurrentRequestWindow.RegisterClientScript("save", "alert('Select the Test and Try again')");
                //        bottle.Duplicate = 0;
                //    }
                //    else
                //    {
                //        List<string> SampleContainer = new List<string>();
                //        COCSettings objsamplecheckin = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("COC_ID=" + COCInfo.strcocID));
                //        IList<COCSettingsSamples> objsampleLogin = ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("COC=?", objsamplecheckin.Oid));
                //        foreach (COCSettingsSamples sampleLog in objsampleLogin)
                //        {
                //            IList<COCSettingsTest> objsample = ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("Samples=?", sampleLog.Oid));
                //            foreach (COCSettingsTest sample in objsample.ToList())
                //            {
                //                if (bottle.Test.Contains(sample.Testparameter.TestMethod))
                //                {
                //                    if (!SampleContainer.Contains(sampleLog.SampleID))
                //                    {
                //                        SampleContainer.Add(sampleLog.SampleID);
                //                    }
                //                }
                //            }
                //        }
                //        if (Convert.ToInt32(e.NewValue) <= SampleContainer.Count)
                //        {
                //            bottle.Duplicate = SampleContainer.Count;
                //            //Distribution.Enabled["Enabled"] = false;
                //            bottle.IsDuplicate = false;
                //        }
                //        else
                //        {
                //            bottle.Duplicate = Convert.ToInt32(e.NewValue);
                //            //Distribution.Enabled["Enabled"] = true;
                //            bottle.IsDuplicate = true;
                //        }
                //        if (((ListView)View).CollectionSource.GetCount() > 0)
                //        {
                //            bool distributevisible = false;
                //            foreach (COCBottleSetup objcollbottle in ((ListView)View).CollectionSource.List)
                //            {
                //                if (objcollbottle.IsDuplicate)
                //                {
                //                    distributevisible = true;
                //                }
                //            }
                //            COCDistribution.Enabled["Enabled"] = distributevisible;
                //        }
                //        else
                //        {
                //            COCDistribution.Enabled["Enabled"] = false;
                //        }
                //        if (SampleContainer.Count > 0)
                //        {
                //            SampleContainer = SampleContainer.OrderBy(s => s).ToList();
                //            bottle.SampleContainer = null;
                //            if (bottle.Duplicate <= SampleContainer.Count)
                //            {
                //                foreach (string sample in SampleContainer)
                //                {
                //                    if (bottle.SampleContainer == null || bottle.SampleContainer.Length == 0)
                //                    {
                //                        bottle.SampleContainer = sample + "(1)";
                //                    }
                //                    else
                //                    {
                //                        if (!bottle.SampleContainer.Contains(sample))
                //                        {
                //                            bottle.SampleContainer = bottle.SampleContainer + ", " + sample + "(1)";
                //                        }
                //                    }
                //                }
                //            }
                //            else if (bottle.Duplicate > SampleContainer.Count)
                //            {
                //                foreach (string sample in SampleContainer)
                //                {
                //                    if (bottle.SampleContainer == null || bottle.SampleContainer.Length == 0)
                //                    {
                //                        bottle.SampleContainer = sample + "(" + ((bottle.Duplicate - SampleContainer.Count) + 1).ToString() + ")";
                //                    }
                //                    else
                //                    {
                //                        if (!bottle.SampleContainer.Contains(sample))
                //                        {
                //                            bottle.SampleContainer = bottle.SampleContainer + ", " + sample + "(1)";
                //                        }
                //                    }
                //                }
                //            }
                //            ((ListView)View).Refresh();
                //        }
                //    }
                //}

                //else if (e.PropertyName == "Qty" && View.Id == "SampleBottleAllocation_ListView_COCSettings")
                //{
                //    SampleBottleAllocation objsmplbtl = (SampleBottleAllocation)e.Object;
                //    if (objsmplbtl.Qty < 1)
                //    {
                //        Application.ShowViewStrategy.ShowMessage("Qty should not be lessthan 1", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //        objsmplbtl.Qty = 1;
                //    }
                //    IsQtyChanged = true;
                //}

                else if (e.PropertyName == "BottleID" && View.Id == "SampleBottleAllocation_ListView_COCSettings" && IsBtlRemove == false)
                {
                    IsbtlIDchanged = true;
                    IsQtyChanged = true;
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
                Frame.GetController<LinkUnlinkController>().UnlinkAction.Execute -= UnlinkAction_Execute;
                Frame.GetController<LinkUnlinkController>().LinkAction.Execute -= LinkAction_Execute;
                Frame.GetController<LinkUnlinkController>().LinkAction.CustomizePopupWindowParams -= LinkAction_CustomizePopupWindowParams;
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                if (View is DashboardView && View.Id == "BottleSetupDistributionCOC")
                {
                    View.ControlsCreated -= View_ControlsCreated;
                }
                if (View.Id == "DummyClass_ListView_COCSettings")
                {
                    COCInfo.selectionhideGuid.Clear();
                    //COCInfo.lstviewselected.Clear();
                }
                if (View.Id == "SampleBottleAllocation_ListView_COCSettings")
                {
                    COCInfo.lstsmplbtlallo.Clear();
                }
                if (View.Id == "TestMethod_ListView_COCBA_Popup")
                {
                    COCInfo.lstdummytests.Clear();
                    //COCInfo.lstdummycreationtests.Clear();
                }
                if (View.Id == "COCSettingsBottleAllocation_ListView_Copy_Sampleregistration")
                {
                    COCsr.lstsmplbtlallo.Clear();
                }
                if (View.Id == "DummyClass_ListView_COCSettingsSample")
                {
                    COCsr.selectionhideGuid.Clear();
                    COCsr.lstviewselected.Clear();
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
                string[] samplesplit = parameter.Split('|');
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (Frame is NestedFrame)
                {
                    if (View is DashboardView)
                    {
                        DashboardView distributiondashboard = (DashboardView)((NestedFrame)Frame).ViewItem.View;
                        DashboardViewItem Bottledetail = distributiondashboard.FindItem("BottleDetail") as DashboardViewItem;
                        //if (Bottledetail != null)
                        //{
                        //    BottleSetup objbottle = (BottleSetup)Bottledetail.InnerView.CurrentObject;
                        //    if (gridListEditor != null && parameter != null)
                        //    {

                        //        //if (Convert.ToInt32(samplesplit[2]) <= objbottle.Duplicate)
                        //        //{
                        //        string newsamplecontainer = string.Empty;
                        //        foreach (COCBottlesetupdistribution obj in ((ListView)View).CollectionSource.List)
                        //        {
                        //            if (obj.SampleID == samplesplit[0])
                        //            {
                        //                obj.Qty = Convert.ToInt32(samplesplit[1]);
                        //                if (newsamplecontainer == string.Empty)
                        //                {
                        //                    newsamplecontainer = samplesplit[0] + "(" + samplesplit[1] + ")";
                        //                }
                        //                else
                        //                {
                        //                    newsamplecontainer = newsamplecontainer + ", " + samplesplit[0] + "(" + samplesplit[1] + ")";
                        //                }
                        //                ((ListView)View).Refresh();
                        //            }
                        //            else
                        //            {
                        //                if (newsamplecontainer == string.Empty)
                        //                {
                        //                    newsamplecontainer = obj.SampleID + "(" + obj.Qty + ")";
                        //                }
                        //                else
                        //                {
                        //                    newsamplecontainer = newsamplecontainer + ", " + obj.SampleID + "(" + obj.Qty + ")";
                        //                }
                        //            }
                        //        }
                        //        objbottle.SampleContainer = newsamplecontainer;
                        //        //}
                        //        //else
                        //        //{
                        //        //high
                        //        //}
                        //    }
                        //}
                    }
                }

                if (gridListEditor != null && parameter != null && View.Id == "VisualMatrix_ListView_COCSettings_SampleMatrix")
                {
                    if (!string.IsNullOrEmpty(parameter))
                    {
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            string[] values = parameter.Split('|');
                            if (values[0] == "SampleMatrixHandler")
                            {
                                if (values[1] == "Selected")
                                {
                                    string strSelProductsCriteria = string.Empty;
                                    Guid curguid = new Guid(values[2]);
                                    List<Guid> lstSelProducts = new List<Guid>();
                                    //if (Frame is NestedFrame && ((NestedFrame)Frame).ViewItem != null && ((NestedFrame)Frame).ViewItem.View != null)
                                    //{
                                    //    CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                                    //    if (cv != null)
                                    //    {
                                    //        COCSettings objTasks = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("COC_ID=" + COCInfo.strcocID));
                                    //        IList<COCSettingsSamples> lstsampling = ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("[COC.Oid] = ? And[VisualMatrix.Oid] = ?", objTasks.Oid, curguid));
                                    //        DashboardViewItem lvSampleID = ((DashboardView)cv).FindItem("SampleID") as DashboardViewItem;
                                    //        if (lstsampling != null && lstsampling.Count > 0)
                                    //        {
                                    //            if (lvSampleID != null && lvSampleID.InnerView != null)
                                    //            {
                                    //                List<Guid> objsmplID = new List<Guid>();
                                    //                foreach (COCSettingsSamples objsampling in lstsampling.ToList())
                                    //                {
                                    //                    if (objsampling.SampleID != null)
                                    //                    {
                                    //                        if (!objsmplID.Contains(objsampling.Oid))
                                    //                        {
                                    //                            objsmplID.Add(objsampling.Oid);
                                    //                        }
                                    //                    }
                                    //                }
                                    //                ((ListView)lvSampleID.InnerView).CollectionSource.Criteria.Clear();
                                    //                ((ListView)lvSampleID.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", objsmplID.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                    //                ObjectSpace.Refresh();
                                    //            }
                                    //            editor.Grid.Selection.SelectRowByKey(curguid);
                                    //        }
                                    //    }
                                    //    if (curguid == Guid.Empty)
                                    //    {
                                    //        COCInfo.visualmatrixGuid = curguid;
                                    //    }
                                    //}
                                }
                            }
                        }
                    }
                }
                if (gridListEditor != null && parameter != null && View.Id == "COCSettingsSamples_ListView_COCBottle")
                {
                    if (!string.IsNullOrEmpty(parameter))
                    {
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            string[] values = parameter.Split('|');
                            if (values[0] == "SampleIDHandler")
                            {
                                if (values[1] == "Selected")
                                {
                                    COCInfo.lstsmplbtlallo.Clear();
                                    string strSelProductsCriteria = string.Empty;
                                    Guid curguid = new Guid(values[2]);
                                    if (curguid != null)
                                    {
                                        gridListEditor.Grid.Selection.SelectRowByKey(curguid);
                                        COCInfo.SamplingGuid = curguid;
                                    }
                                    List<Guid> lstSelProducts = new List<Guid>();
                                    if (Frame is NestedFrame && ((NestedFrame)Frame).ViewItem != null && ((NestedFrame)Frame).ViewItem.View != null)
                                    {
                                        CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                                        //if (cv != null)
                                        //{
                                        //    COCSettingsSamples objsampling = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", curguid));
                                        //    IList<COCSettingsTest> lstsamplingtest = ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[Samples.Oid] = ?", objsampling.Oid));
                                        //    DashboardViewItem lvBottleAllocation = ((DashboardView)cv).FindItem("BottleAllocation") as DashboardViewItem;
                                        //    if (lvBottleAllocation != null && lvBottleAllocation.InnerView != null)
                                        //    {
                                        //        IList<SampleBottleAllocation> lstsmplbotallow = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[COCSettings.Oid] = ?", objsampling.Oid));
                                        //        if (lstsmplbotallow != null && lstsmplbotallow.Count > 0)
                                        //        {
                                        //            List<Guid> lstsmplallocation = new List<Guid>();
                                        //            foreach (SampleBottleAllocation objsamplingallocation in lstsmplbotallow.ToList())
                                        //            {
                                        //                if (!lstsmplallocation.Contains(objsamplingallocation.Oid))
                                        //                {
                                        //                    lstsmplallocation.Add(objsamplingallocation.Oid);
                                        //                }
                                        //            }
                                        //            ((ListView)lvBottleAllocation.InnerView).CollectionSource.Criteria.Clear();
                                        //            ((ListView)lvBottleAllocation.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", lstsmplallocation.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                        //            ObjectSpace.Refresh();
                                        //        }
                                        //        else
                                        //        {
                                        //            ((ListView)lvBottleAllocation.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is Null");
                                        //        }
                                        //    }
                                        //}
                                    }
                                }
                            }
                        }
                    }
                }
                else if (gridListEditor != null && parameter != null && View.Id == "SampleBottleAllocation_ListView_COCSettings")
                {
                    if (gridListEditor != null && gridListEditor.Grid != null && parameter != "true" && parameter != "false")
                    {
                        if (parameter == "Selectall")
                        {
                            if (gridListEditor.Grid.VisibleRowCount == gridListEditor.Grid.Selection.Count)
                            {
                                foreach (SampleBottleAllocation objdc in ((ListView)View).CollectionSource.List.Cast<SampleBottleAllocation>().ToList())
                                {
                                    if (!COCInfo.lstsmplbtlallo.Contains(objdc))
                                    {
                                        COCInfo.lstsmplbtlallo.Add(objdc);
                                    }
                                }
                            }
                        }
                        else if (parameter == "UNSelectall")
                        {
                            foreach (SampleBottleAllocation objdc in ((ListView)View).CollectionSource.List.Cast<SampleBottleAllocation>().ToList())
                            {
                                if (COCInfo.lstsmplbtlallo.Contains(objdc))
                                {
                                    COCInfo.lstsmplbtlallo.Remove(objdc);
                                }
                            }
                        }
                        else if (samplesplit[0] == "Selected" || samplesplit[0] == "UNSelected")
                        {
                            if (!string.IsNullOrEmpty(samplesplit[1]))
                            {
                                Guid objguid = new Guid(samplesplit[1]);
                                if (samplesplit[0] == "Selected")
                                {
                                    SampleBottleAllocation objsmplbtlallo = View.ObjectSpace.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[Oid]=?", new Guid(samplesplit[1])), true);
                                    if (objsmplbtlallo != null)
                                    {
                                        if (!COCInfo.lstsmplbtlallo.Contains(objsmplbtlallo))
                                        {
                                            COCInfo.lstsmplbtlallo.Add(objsmplbtlallo);
                                        }
                                    }
                                }
                                else if (samplesplit[0] == "UNSelected")
                                {
                                    SampleBottleAllocation objsmplbtlallo = View.ObjectSpace.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[Oid]=?", new Guid(samplesplit[1])), true);
                                    if (objsmplbtlallo != null)
                                    {
                                        if (COCInfo.lstsmplbtlallo.Contains(objsmplbtlallo))
                                        {
                                            COCInfo.lstsmplbtlallo.Remove(objsmplbtlallo);
                                        }
                                    }
                                }
                            }
                        }
                        if (objPermissionInfo.COCBottleIsRead == false)
                        {
                            //if (samplesplit[0] == "BottleID")
                            //{
                            //    HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(samplesplit[1]), "Oid");
                            //    //string SampleID = gridListEditor.Grid.GetRowValues(int.Parse(samplesplit[1]), "SampleID").ToString();
                            //    IObjectSpace os = Application.CreateObjectSpace();
                            //    HttpContext.Current.Session["BottleID"] = gridListEditor.Grid.GetRowValues(int.Parse(samplesplit[1]), "BottleID");
                            //    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(DummyClass));
                            //    ListView lv = Application.CreateListView("DummyClass_ListView_COCSettings", cs, false);
                            //    ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                            //    showViewParameters.Context = TemplateContext.PopupWindow;
                            //    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            //    showViewParameters.CreatedView.Caption = "BottleID";
                            //    DialogController dc = Application.CreateController<DialogController>();
                            //    dc.SaveOnAccept = false;
                            //    dc.CloseOnCurrentObjectProcessing = false;
                            //    dc.Accepting += AcceptAction_Accepting;
                            //    showViewParameters.Controllers.Add(dc);
                            //    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                            //    if (HttpContext.Current.Session["rowid"] != null)
                            //    {
                            //        COCInfo.lstcrtbottleid = new List<string>();
                            //        COCInfo.lstotherbottleid = new List<string>();
                            //        SampleBottleAllocation objsampling = ((ListView)View).CollectionSource.List.Cast<SampleBottleAllocation>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                            //        if (objsampling != null && objsampling.BottleID != null)
                            //        {
                            //            string[] strbottleid = objsampling.BottleID.Split(',');
                            //            foreach (var strbotid in strbottleid)
                            //            {
                            //                COCInfo.lstcrtbottleid.Add(strbotid.Trim());
                            //            }
                            //        }
                            //        if (objsampling != null && objsampling.Oid != null)
                            //        {
                            //            COCInfo.lstotherbottleid = new List<string>();
                            //            foreach (SampleBottleAllocation objsmpalloc in ((ListView)View).CollectionSource.List.Cast<SampleBottleAllocation>().Where(i => i.Oid != objsampling.Oid))
                            //            {
                            //                if (objsmpalloc != null && objsmpalloc.BottleID != null)
                            //                {
                            //                    string[] strbottleid = objsmpalloc.BottleID.Split(',');
                            //                    foreach (var strbotid in strbottleid)
                            //                    {
                            //                        COCInfo.lstotherbottleid.Add(strbotid.Trim());
                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                            //if (samplesplit[0] == "SharedTests")
                            //{
                            //    HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(samplesplit[1]), "Oid");
                            //    IObjectSpace os = Application.CreateObjectSpace();
                            //    HttpContext.Current.Session["SharedTests"] = gridListEditor.Grid.GetRowValues(int.Parse(samplesplit[1]), "SharedTests");
                            //    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(TestMethod));
                            //    ListView lv = Application.CreateListView("TestMethod_ListView_COCBA_Popup", cs, false);
                            //    ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                            //    showViewParameters.Context = TemplateContext.PopupWindow;
                            //    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            //    showViewParameters.CreatedView.Caption = "Tests";
                            //    DialogController dc = Application.CreateController<DialogController>();
                            //    dc.SaveOnAccept = false;
                            //    dc.CloseOnCurrentObjectProcessing = false;
                            //    dc.Accepting += Tests_AcceptAction_Execute;
                            //    showViewParameters.Controllers.Add(dc);
                            //    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                            //    if (HttpContext.Current.Session["rowid"] != null)
                            //    {
                            //        COCInfo.lstcrttests = new List<string>();
                            //        SampleBottleAllocation objsampling = ((ListView)View).CollectionSource.List.Cast<SampleBottleAllocation>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                            //        if (objsampling != null && objsampling.SharedTests != null)
                            //        {
                            //            string[] strSharedTests = objsampling.SharedTests.Split(',');
                            //            foreach (var strtests in strSharedTests)
                            //            {
                            //                COCInfo.lstcrttests.Add(strtests.Trim());
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                    else if (bool.TryParse(parameter, out bool CanDeleteTaskBottleAllocation))
                    {
                        if (CanDeleteTaskBottleAllocation)
                        {
                            IsBtlRemove = true;
                            foreach (SampleBottleAllocation objdelsmplbtl in View.SelectedObjects) /*SInfo.lstsmplbtlallo.ToList())*/
                            {
                                ObjectSpace.Delete(objdelsmplbtl);
                            }
                            ObjectSpace.CommitChanges();
                            ObjectSpace.Refresh();
                            COCInfo.lstsmplbtlallo.Clear();
                            //BottleIDUpdate();
                            Application.ShowViewStrategy.ShowMessage("Delete successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {

                        }
                    }
                }
                else if (gridListEditor != null && parameter != null && View.Id == "DummyClass_ListView_COCSettings")
                {
                    //if (gridListEditor != null && gridListEditor.Grid != null)
                    //{
                    //    if (parameter == "Selectall")
                    //    {
                    //        if (gridListEditor.Grid.VisibleRowCount == gridListEditor.Grid.Selection.Count)
                    //        {
                    //            foreach (DummyClass objdc in ((ListView)View).CollectionSource.List.Cast<DummyClass>().ToList())
                    //            {
                    //                if (!COCInfo.selectionhideGuid.Contains(objdc.Oid.ToString()))
                    //                {
                    //                    if (!COCInfo.lstviewselected.Contains(objdc))
                    //                    {
                    //                        COCInfo.lstviewselected.Add(objdc);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else if (parameter == "UNSelectall")
                    //    {
                    //        COCInfo.lstviewselected.Clear();
                    //    }
                    //    else
                    //    {
                    //        string[] splparm = parameter.Split('|');
                    //        if (!string.IsNullOrEmpty(splparm[1]))
                    //        {
                    //            Guid objguid = new Guid(splparm[1]);
                    //            if (splparm[0] == "Selected")
                    //            {
                    //                foreach (DummyClass objdc in View.SelectedObjects)
                    //                {
                    //                    if (!COCInfo.selectionhideGuid.Contains(objdc.Oid.ToString()))
                    //                    {
                    //                        if (!COCInfo.lstviewselected.Contains(objdc))
                    //                        {
                    //                            COCInfo.lstviewselected.Add(objdc);
                    //                        }
                    //                    }
                    //                }

                    //            }
                    //            else if (splparm[0] == "UNSelected")
                    //            {
                    //                COCInfo.lstviewselected.Clear();
                    //                foreach (DummyClass objdc in View.SelectedObjects)
                    //                {
                    //                    if (!COCInfo.selectionhideGuid.Contains(objdc.Oid.ToString()))
                    //                    {
                    //                        if (!COCInfo.lstviewselected.Contains(objdc))
                    //                        {
                    //                            COCInfo.lstviewselected.Add(objdc);
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    ((ListView)View).Refresh();
                }
                else if (gridListEditor != null && parameter != null && View.Id == "TestMethod_ListView_COCBA_Popup")
                {
                    if (gridListEditor != null && gridListEditor.Grid != null && parameter != "true" && parameter != "false")
                    {
                        if (parameter == "Selectall")
                        {
                            if (gridListEditor.Grid.VisibleRowCount == gridListEditor.Grid.Selection.Count)
                            {
                                foreach (TestMethod objdc in ((ListView)View).CollectionSource.List.Cast<TestMethod>().ToList())
                                {
                                    if (!COCInfo.lstdummytests.Contains(objdc.Oid))
                                    {
                                        COCInfo.lstdummytests.Add(objdc.Oid);
                                    }
                                }
                            }
                        }
                        else if (parameter == "UNSelectall")
                        {
                            foreach (TestMethod objdc in View.SelectedObjects)
                            {
                                if (COCInfo.lstdummytests.Contains(objdc.Oid))
                                {
                                    COCInfo.lstdummytests.Add(objdc.Oid);
                                }
                            }
                        }
                        else
                        {
                            string[] splparm = parameter.Split('|');
                            if (!string.IsNullOrEmpty(splparm[1]))
                            {
                                Guid objguid = new Guid(splparm[1]);
                                if (splparm[0] == "Selected")
                                {
                                    foreach (TestMethod objdc in View.SelectedObjects)
                                    {
                                        if (!COCInfo.lstdummytests.Contains(objdc.Oid))
                                        {
                                            COCInfo.lstdummytests.Add(objdc.Oid);
                                        }
                                    }

                                }
                                else if (splparm[0] == "UNSelected")
                                {
                                    //List<DummyClass> unselect = new List<DummyClass>();
                                    //COCInfo.lstdummytests.Clear();
                                    //foreach (TestMethod objdc in View.SelectedObjects)
                                    //{
                                    //    if (!COCInfo.lstdummytests.Contains(objdc.Oid))
                                    //    {
                                    //        COCInfo.lstdummytests.Add(objdc.Oid);
                                    //    }
                                    //}
                                }
                            }
                        }
                    }
                    else if (bool.TryParse(parameter, out bool CanDeleteTaskBottleAllocation))
                    {
                        if (CanDeleteTaskBottleAllocation)
                        {
                            List<object> lstsharedtest = new List<object>();
                            string sharedtest = string.Empty;
                            foreach (TestMethod objdeltest in View.SelectedObjects)
                            {
                                if (string.IsNullOrEmpty(sharedtest))
                                {
                                    sharedtest = objdeltest.Oid.ToString();
                                }
                                else
                                {
                                    sharedtest = sharedtest + "| " + objdeltest.Oid.ToString();
                                }
                                lstsharedtest.Add(objdeltest.Oid);
                            }
                            if (!string.IsNullOrEmpty(sharedtest))
                            {
                                SampleBottleAllocation objsmp = ObjectSpace.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                if (objsmp != null)
                                {
                                    ////objsmp.SharedTestsGuid = sharedtest;
                                    ObjectSpace.CommitChanges();
                                    ObjectSpace.Refresh();
                                }
                            }
                            else
                            {
                                SampleBottleAllocation objsmp = ObjectSpace.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                if (objsmp != null)
                                {
                                    ObjectSpace.Delete(objsmp);
                                    ObjectSpace.CommitChanges();
                                    ObjectSpace.Refresh();
                                }
                            }
                            if (lstsharedtest != null && lstsharedtest.Count > 0)
                            {
                                ((ListView)View).CollectionSource.Criteria.Clear();
                                ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsharedtest);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                            }
                            //Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "SamplingProposal", false));
                            Application.ShowViewStrategy.ShowMessage("Remove successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {

                        }
                    }
                }
                //else if (gridListEditor != null && parameter != null && View.Id == "COCSettingsSamples_ListView_COCBottle_CopyTOBottleAllocation")
                //{
                //    if (gridListEditor != null && gridListEditor.Grid != null)
                //    {
                //        if (COCInfo.lstcopytosampleID == null)
                //        {
                //            COCInfo.lstcopytosampleID = new List<Guid>();
                //        }
                //        if (samplesplit[0] == "Selectall")
                //        {
                //            COCInfo.lstcopytosampleID.Clear();
                //            foreach (COCSettingsSamples objsampling in ((ListView)View).CollectionSource.List.Cast<COCSettingsSamples>().ToList())
                //            {
                //                if (!COCInfo.lstcopytosampleID.Contains(objsampling.Oid))
                //                {
                //                    COCInfo.lstcopytosampleID.Add(objsampling.Oid);
                //                    //gridListEditor.Grid.Selection.SelectRowByKey(objsampling.Oid);
                //                    //qctypeinfo.chkselectall = "selectedall";
                //                }
                //            }
                //        }
                //        else if (samplesplit[0] == "UNSelectall")
                //        {
                //            //foreach (COCSettingsSamples objsampling in ((ListView)View).CollectionSource.List.Cast<COCSettingsSamples>().ToList())
                //            //{
                //            //    if (SInfo.lstcopytosampleID.Contains(objsampling.Oid))
                //            //    {
                //            //        SInfo.lstcopytosampleID.Remove(objsampling.Oid);
                //            //    }
                //            //}
                //            //qctypeinfo.chkselectall = null;
                //        }
                //        else if (samplesplit[0] == "Selected" || samplesplit[0] == "UNSelected")
                //        {
                //            if (samplesplit[0] == "Selected")
                //            {
                //                COCSettingsSamples objsampling = View.ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid]=?", new Guid(samplesplit[1])), true);
                //                if (objsampling != null)
                //                {
                //                    if (!COCInfo.lstcopytosampleID.Contains(objsampling.Oid))
                //                    {
                //                        COCInfo.lstcopytosampleID.Add(objsampling.Oid);
                //                    }
                //                }
                //            }
                //            else if (samplesplit[0] == "UNSelected")
                //            {
                //                COCSettingsSamples objsampling = View.ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid]=?", new Guid(samplesplit[1])), true);
                //                if (objsampling != null)
                //                {
                //                    if (COCInfo.lstcopytosampleID.Contains(objsampling.Oid))
                //                    {
                //                        COCInfo.lstcopytosampleID.Remove(objsampling.Oid);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                if (gridListEditor != null && parameter != null && View.Id == "COCSettingsBottleAllocation_ListView_Copy_Sampleregistration")
                {
                    if (objPermissionInfo.SampleBottleIsRead == false)
                    {
                        if (samplesplit[0] == "NPBottleID")
                        {
                            HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(samplesplit[1]), "Oid");
                            if (HttpContext.Current.Session["rowid"] != null)
                            {
                                COCsr.lstcrtbottleid = new List<string>();
                                COCSettingsBottleAllocation objsampling = ((ListView)View).CollectionSource.List.Cast<COCSettingsBottleAllocation>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                                if (objsampling != null && objsampling.BottleID != null)
                                {
                                    string[] strbottleid = objsampling.NPBottleID.Split(',');
                                    foreach (var strbotid in strbottleid)
                                    {
                                        COCsr.lstcrtbottleid.Add(strbotid.Trim());
                                    }
                                }
                            }
                            CollectionSource cs = new CollectionSource(ObjectSpace, typeof(DummyClass));
                            ListView lv = Application.CreateListView("DummyClass_ListView_COCSettingsSample", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView.Caption = "BottleID";
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += AcceptAction_Accepting;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                }
                else if (gridListEditor != null && parameter != null && View.Id == "DummyClass_ListView_COCSettingsSample")
                {
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        if (parameter == "Selectall")
                        {
                            if (gridListEditor.Grid.VisibleRowCount == gridListEditor.Grid.Selection.Count)
                            {
                                foreach (DummyClass objdc in ((ListView)View).CollectionSource.List.Cast<DummyClass>().ToList())
                                {
                                    if (!COCsr.selectionhideGuid.Contains(objdc.Oid.ToString()))
                                    {
                                        if (!COCsr.lstviewselected.Contains(objdc))
                                        {
                                            COCsr.lstviewselected.Add(objdc);
                                        }
                                    }
                                }
                            }
                        }
                        else if (parameter == "UNSelectall")
                        {
                            COCsr.lstviewselected.Clear();
                        }
                        else
                        {
                            string[] splparm = parameter.Split('|');
                            if (!string.IsNullOrEmpty(splparm[1]))
                            {
                                Guid objguid = new Guid(splparm[1]);
                                if (splparm[0] == "Selected")
                                {
                                    foreach (DummyClass objdc in View.SelectedObjects)
                                    {
                                        if (!COCsr.selectionhideGuid.Contains(objdc.Oid.ToString()))
                                        {
                                            if (!COCsr.lstviewselected.Contains(objdc))
                                            {
                                                COCsr.lstviewselected.Add(objdc);
                                            }
                                        }
                                    }
                                }
                                else if (splparm[0] == "UNSelected")
                                {
                                    COCsr.lstviewselected.Clear();
                                    foreach (DummyClass objdc in View.SelectedObjects)
                                    {
                                        if (!COCsr.selectionhideGuid.Contains(objdc.Oid.ToString()))
                                        {
                                            if (!COCsr.lstviewselected.Contains(objdc))
                                            {
                                                COCsr.lstviewselected.Add(objdc);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                 ((ListView)View).Refresh();
                }
                else if (gridListEditor != null && parameter != null && View.Id == "COCSettingsSamples_ListView_Copy_Bottle")
                {
                    if (samplesplit[0] == "Qty")
                    {
                        HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(samplesplit[1]), "Oid");
                        if (HttpContext.Current.Session["rowid"] != null)
                        {
                            COCSettingsSamples objsample = ((ListView)View).CollectionSource.List.Cast<COCSettingsSamples>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                            if (objsample != null)
                            {
                                ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                if (objsample.Qty <= 0)
                                {
                                    COCSettingsSamples objSampleLogin = uow.GetObjectByKey<COCSettingsSamples>(objsample.Oid);
                                    objSampleLogin.Qty = Convert.ToUInt16(samplesplit[2]);
                                    uow.CommitChanges();
                                    View.ObjectSpace.Refresh();
                                    Application.ShowViewStrategy.ShowMessage("Qty must be greater than 0", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                                else
                                {
                                    List<string> lstqtyBottle = new List<string>();
                                    const string letterseql = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                    string value = "";
                                    for (int i = 0; i <= objsample.Qty - 1; i++)
                                    {
                                        value = "";
                                        if (i >= letterseql.Length)
                                        {
                                            value += letterseql[i / letterseql.Length - 1];
                                        }

                                        value += letterseql[i % letterseql.Length];
                                        if (!string.IsNullOrEmpty(value))
                                        {
                                            lstqtyBottle.Add(value);
                                        }
                                    }
                                    List<string> lstAssignedBttle = new List<string>();
                                    List<COCSettingsBottleAllocation> lstBottle = uow.Query<COCSettingsBottleAllocation>().Where(i => i.COCSettingsRegistration != null && i.COCSettingsRegistration.Oid == objsample.Oid).ToList();
                                    foreach (COCSettingsBottleAllocation sample in lstBottle)
                                    {
                                        List<string> lstBottles = sample.BottleID.Split(',').ToList().Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                                        lstAssignedBttle.AddRange(lstBottles);
                                    }
                                    int qty = 0;
                                    foreach (string objLetter in lstqtyBottle.ToList())
                                    {
                                        qty = qty + lstAssignedBttle.Where(i => i == objLetter).Count();
                                    }
                                    if (qty != lstAssignedBttle.Count)
                                    {
                                        COCSettingsSamples objSampleLogin = uow.GetObjectByKey<COCSettingsSamples>(objsample.Oid);
                                        objSampleLogin.Qty = Convert.ToUInt16(samplesplit[2]);
                                        uow.CommitChanges();
                                        View.ObjectSpace.Refresh();
                                        Application.ShowViewStrategy.ShowMessage("The test has already been assigned to bottles; please remove the bottle ID.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    }
                                }

                                //uow.CommitChanges();
                                //CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                                //if (cv != null)
                                //{
                                //    DashboardViewItem dvsampleids = ((DetailView)cv).FindItem("SampleBottleAllocation_SampleRegistration") as DashboardViewItem;
                                //    if (dvsampleids != null && dvsampleids.InnerView != null)
                                //    {
                                //        dvsampleids.InnerView.ObjectSpace.Refresh();
                                //    }
                                //}
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
        private void Tests_AcceptAction_Execute(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                IObjectSpace ossmpl = Application.CreateObjectSpace(typeof(SampleBottleAllocation));
                string strtest = string.Empty;
                string strtestname = string.Empty;
                List<Guid> lstContainer = new List<Guid>();
                List<Guid> lstPreservative = new List<Guid>();

                if (sender != null)
                {
                    DialogController objDialog = (DialogController)sender as DialogController; ;
                    if (objDialog.Window.View.Id == "TestMethod_ListView_COCBA_Popup")
                    {
                        if (objDialog.Window.View.SelectedObjects.Count > 0)
                        {
                            foreach (TestMethod objtm in objDialog.Window.View.SelectedObjects)
                            {
                                if (objtm != null && string.IsNullOrEmpty(strtest))
                                {
                                    strtest = objtm.Oid.ToString();
                                    strtestname = objtm.TestName;
                                }
                                else
                                {
                                    strtestname = strtestname + "; " + objtm.TestName;
                                    strtest = strtest + "; " + objtm.Oid.ToString();
                                }
                                IList<Guid> containerNames = objtm.TestGuides.Where(i => i.Container != null).Select(i => i.Container.Oid).ToList();
                                if (containerNames != null && containerNames.Count > 0)
                                {
                                    foreach (Guid objContainer in containerNames)
                                    {
                                        if (!lstContainer.Contains(objContainer))
                                        {
                                            lstContainer.Add(objContainer);
                                        }
                                    }
                                }
                                IList<Guid> Preservative = objtm.TestGuides.Where(i => i.Preservative != null).Select(i => i.Preservative.Oid).ToList();
                                if (Preservative != null && Preservative.Count > 0)
                                {
                                    foreach (Guid objPreservative in Preservative)
                                    {
                                        if (!lstPreservative.Contains(objPreservative))
                                        {
                                            lstPreservative.Add(objPreservative);
                                        }
                                    }
                                }
                            }
                            SampleBottleAllocation objsmplbtl = ObjectSpace.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.lstsmplbtlalloGuid));
                            if (objsmplbtl != null)
                            {
                                if (lstContainer.Count == 1)
                                {
                                    Modules.BusinessObjects.Setting.Container objContainer = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.Container>(CriteriaOperator.Parse("Oid=?", lstContainer[0]));
                                    if (objContainer != null)
                                    {
                                        objsmplbtl.Containers = objContainer;
                                    }
                                }
                                if (lstPreservative.Count == 1)
                                {
                                    Preservative objpreservative = ObjectSpace.FindObject<Preservative>(CriteriaOperator.Parse("Oid=?", lstPreservative[0]));
                                    if (objpreservative != null)
                                    {
                                        objsmplbtl.Preservative = objpreservative;
                                    }
                                }
                                ObjectSpace.CommitChanges();
                                ObjectSpace.Refresh();
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Select checkbox", InformationType.Info, timer.Seconds, InformationPosition.Top);
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

        //private void BottleIDUpdate()
        //{
        //    try
        //    {
        //        if (View.Id == "SampleBottleAllocation_ListView_COCSettings")
        //        {
        //            IObjectSpace os = Application.CreateObjectSpace(typeof(SampleBottleAllocation));
        //            List<string> lstbtlid = new List<string>();
        //            foreach (SampleBottleAllocation objsmpl in ((ListView)View).CollectionSource.List.Cast<SampleBottleAllocation>())
        //            {
        //                if (objsmpl != null)
        //                {
        //                    uint strbottleqty = objsmpl.Qty;
        //                    string strbtlID = string.Empty;
        //                    const string letterseql = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //                    string valueeql = "";
        //                    for (int i = 0; i <= strbottleqty - 1; i++)
        //                    {
        //                        valueeql = "";
        //                        if (i >= letterseql.Length)
        //                            valueeql += letterseql[i / letterseql.Length - 1];
        //                        valueeql += letterseql[i % letterseql.Length];
        //                        if (!lstbtlid.Contains(valueeql))
        //                        {
        //                            if (String.IsNullOrEmpty(strbtlID))
        //                            {
        //                                strbtlID = valueeql;
        //                            }
        //                            else
        //                            {
        //                                strbtlID = strbtlID + ", " + valueeql;
        //                            }
        //                            lstbtlid.Add(valueeql);
        //                        }
        //                        else
        //                        {
        //                            strbottleqty = strbottleqty + 1;
        //                            continue;
        //                        }
        //                    }
        //                    objsmpl.BottleID = strbtlID;
        //                }
        //            }
        //            if (IsQtyChanged != true)
        //            {
        //                ObjectSpace.CommitChanges();
        //                ObjectSpace.Refresh();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void Tests_AcceptAction_Accepting(object sender, DialogControllerAcceptingEventArgs e)
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
        private void AcceptAction_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                COCSettingsBottleAllocation objsampling = ((ListView)View).CollectionSource.List.Cast<COCSettingsBottleAllocation>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                string assigned = string.Empty;
                DialogController objDialog = (DialogController)sender;
                ListView view = null;
                if (objDialog != null && objDialog.Frame.View is ListView)
                {
                    view = (ListView)objDialog.Frame.View;
                }
                if (view.SelectedObjects.Count > 0)
                {
                    if (objsampling.COCSettingsRegistration.Qty >= COCsr.lstviewselected.Count)
                    {
                        if (HttpContext.Current.Session["rowid"] != null)
                        {
                            if (objsampling != null)
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                COCSettingsBottleAllocation objBottle = os.GetObjectByKey<COCSettingsBottleAllocation>(objsampling.Oid);
                                if (objBottle != null)
                                {
                                    if (view.SelectedObjects.Count == 1)
                                    {
                                        objBottle.BottleID = ((DummyClass)view.SelectedObjects[0]).Name;
                                        IList<COCSettingsBottleAllocation> samples = os.GetObjects<COCSettingsBottleAllocation>(CriteriaOperator.Parse("[COCSettingsRegistration.Oid]=? And [TestMethod.Oid]=?", objBottle.COCSettingsRegistration.Oid, objBottle.TestMethod.Oid)).ToList();
                                        if (samples != null && samples.Count > 1)
                                        {
                                            foreach (COCSettingsBottleAllocation sampleBottle in samples)
                                            {
                                                if (sampleBottle.Oid != objBottle.Oid)
                                                {
                                                    os.Delete(sampleBottle);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                foreach (DummyClass objdc in view.SelectedObjects.Cast<DummyClass>().OrderBy(a => a.Name).ToList())
                {
                    if (!assigned.Contains(objdc.Name))
                    {
                        if (assigned == string.Empty)
                        {
                                                    objBottle.BottleID = assigned = objdc.Name;
                        }
                        else
                        {
                            assigned = assigned + ", " + objdc.Name;
                                                    IList<COCSettingsBottleAllocation> samples = os.GetObjects<COCSettingsBottleAllocation>(CriteriaOperator.Parse("[COCSettingsRegistration.Oid]=? And [TestMethod.Oid]=? And [BottleID]=?", objBottle.COCSettingsRegistration.Oid, objBottle.TestMethod.Oid, objdc.Name), true).ToList();
                                                    if (samples.Count == 0)
                                                    {
                                                        COCSettingsBottleAllocation objNewBottle = os.CreateObject<COCSettingsBottleAllocation>();
                                                        objNewBottle.BottleID = objdc.Name;
                                                        objNewBottle.COCSettingsRegistration = os.GetObjectByKey<COCSettingsSamples>(objBottle.COCSettingsRegistration.Oid);
                                                        objNewBottle.TestMethod = os.GetObjectByKey<TestMethod>(objBottle.TestMethod.Oid);
                                                        if (objBottle.Containers != null)
                                                        {
                                                            objNewBottle.Containers = os.GetObjectByKey<Container>(objBottle.Containers.Oid);
                        }
                                                        if (objBottle.Preservative != null)
                                                        {
                                                            objNewBottle.Preservative = os.GetObjectByKey<Preservative>(objBottle.Preservative.Oid);
                    }
                                                        if (objBottle.StorageID != null)
                                                        {
                                                            objNewBottle.StorageID = os.GetObjectByKey<Storage>(objBottle.StorageID.Oid);
                }
                                                        if (objBottle.StorageCondition != null)
                {
                                                            objNewBottle.StorageCondition = os.GetObjectByKey<PreserveCondition>(objBottle.StorageCondition.Oid);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        IList<COCSettingsBottleAllocation> delsamples = os.GetObjects<COCSettingsBottleAllocation>(CriteriaOperator.Parse("[COCSettingsRegistration.Oid]=? And [TestMethod.Oid]=? And Not [BottleID] In (" + string.Format("'{0}'", assigned.Replace(", ", "','")) + ")", objBottle.COCSettingsRegistration.Oid, objBottle.TestMethod.Oid)).ToList();
                                        foreach (COCSettingsBottleAllocation sample in delsamples)
                    {
                                            os.Delete(sample);
                                        }
                                    }
                                    HttpContext.Current.Session["Assign"] = assigned;
                                    os.CommitChanges();
                                    os.Refresh();
                                    View.ObjectSpace.Refresh();
                                }
                            }
                           ((ListView)View).Refresh();
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Selected bottleid count greather than Qty", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Select BottleID check box", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void LinkAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                //COCBottleSetup bottle = (COCBottleSetup)((NestedFrame)Frame).ViewItem.CurrentObject;
                //if (bottle.Matrix != null)
                //{
                //    COCInfo.strMatrix = bottle.Matrix.MatrixName;
                //}
                //else
                //{
                //    COCInfo.strMatrix = string.Empty;
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void LinkAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                //if (e.PopupWindowViewSelectedObjects.Count > 0)
                //{
                //    IList<TestMethod> Methods = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[MethodName.Oid] In (" + string.Format("'{0}'", string.Join("','", COCInfo.lstMethod.Select(i => i.ToString().Replace("'", "''")))) + ")"));
                //    COCBottleSetup setup = (COCBottleSetup)((NestedFrame)Frame).ViewItem.CurrentObject;
                //    if (setup != null && Methods != null)
                //    {
                //        foreach (TestMethod test in e.PopupWindowViewSelectedObjects)
                //        {
                //            foreach (TestMethod method in Methods)
                //            {
                //                //if (method.TestName == test.TestName && test.MethodName.Oid != method.MethodName.Oid)
                //                if (method.TestName == test.TestName && method.Oid != test.Oid)
                //                {
                //                    setup.Test.Add(method);
                //                }
                //            }
                //        }
                //        List<string> SampleQty = new List<string>();
                //        COCSettings objsamplecheckin = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("COC_ID=" + COCInfo.strcocID));
                //        IList<COCSettingsSamples> objsampleLogin = ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("COC=?", objsamplecheckin.Oid));
                //        foreach (COCSettingsSamples sampleLog in objsampleLogin)
                //        {
                //            IList<COCSettingsTest> objsample = ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("Samples=?", sampleLog.Oid));
                //            foreach (COCSettingsTest sample in objsample.ToList())
                //            {
                //                if (setup.Test.Contains(sample.Testparameter.TestMethod))
                //                {
                //                    if (!SampleQty.Contains(sampleLog.SampleID))
                //                    {
                //                        SampleQty.Add(sampleLog.SampleID);
                //                    }
                //                }
                //            }
                //        }
                //        setup.Duplicate = SampleQty.Count;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void UnlinkAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //IList<TestMethod> Methods = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[MethodName.Oid] In (" + string.Format("'{0}'", string.Join("','", COCInfo.lstMethod.Select(i => i.ToString().Replace("'", "''")))) + ")"));
                //BottleSetup setup = (BottleSetup)((NestedFrame)Frame).ViewItem.CurrentObject;
                //foreach (TestMethod test in e.SelectedObjects)
                //{
                //    if (!COCInfo.lstTest.Contains(test.TestName))
                //    {
                //        COCInfo.lstTest.Add(test.TestName);
                //    }
                //    foreach (TestMethod method in Methods)
                //    {
                //        if (method.TestName == test.TestName && test.Oid != method.Oid)
                //        {
                //            setup.Test.Remove(method);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void COCBottleSetup_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {

                DialogController dc = Application.CreateController<DialogController>();
                if (objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit)
                {
                    ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                    ObjectSpace.Refresh();
                    COCInfo.IsBottleidvalid = true;
                    COCInfo.strMatrix = string.Empty;
                    ListView createdlistview;
                    findtest();
                    //if (COCInfo.lstTest != null && COCInfo.lstTest.Count > 0)
                    //{
                    //    CollectionSource csnew = new CollectionSource(ObjectSpace, typeof(COCBottleSetup));
                    //    csnew.Criteria["filter"] = CriteriaOperator.Parse("[Bottlesharingname]=?", COCInfo.strcocID);
                    //    if (csnew.GetCount() == 0)
                    //    {
                    //        csnew.Criteria["filter"] = CriteriaOperator.Parse("IsNullOrEmpty([Bottlesharingname])");
                    //        if (csnew.GetCount() > 0)
                    //        {
                    //            IList<COCBottleSetup> bottles = ObjectSpace.GetObjects<COCBottleSetup>(CriteriaOperator.Parse("IsNullOrEmpty([BottleSharingName])"));
                    //            foreach (COCBottleSetup setup in bottles.ToList())
                    //            {
                    //                csnew.Remove(setup);
                    //                ObjectSpace.Delete(setup);
                    //            }
                    //        }
                    //        createdlistview = Application.CreateListView("COCBottleSetup_ListView", csnew, false);
                    //        COCBottleSetup bottle = ObjectSpace.CreateObject<COCBottleSetup>();
                    //        bottle.Bottleid = "A";
                    //        bottle.Bottlesharingname = COCInfo.strcocID;
                    //        if (COCInfo.lstMatrix != null && COCInfo.lstMatrix.Count == 1)
                    //        {
                    //            Matrix objmatrix = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]=?", COCInfo.lstMatrix[0]));
                    //            if (objmatrix != null)
                    //            {
                    //                bottle.Matrix = objmatrix;
                    //            }
                    //        }

                    //        createdlistview.EditView.CurrentObject = bottle;
                    //    }
                    //    else
                    //    {
                    //        createdlistview = Application.CreateListView("COCBottleSetup_ListView", csnew, false);
                    //        foreach (COCBottleSetup setup in csnew.List)
                    //        {
                    //            foreach (TestMethod objtest in setup.Test)
                    //            {
                    //                if (COCInfo.lstTest.Contains(objtest.TestName))
                    //                {
                    //                    COCInfo.lstTest.Remove(objtest.TestName);
                    //                }
                    //            }
                    //        }
                    //        if (COCInfo.lstTest.Count > 0)
                    //        {
                    //            COCBottleSetup newbottle = ObjectSpace.CreateObject<COCBottleSetup>();
                    //            newbottle.Bottleid = Createbottleid(csnew);
                    //            newbottle.Bottlesharingname = COCInfo.strcocID;
                    //            if (COCInfo.lstMatrix.Count == 1)
                    //            {
                    //                Matrix objmatrix = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]=?", COCInfo.lstMatrix[0]));
                    //                if (objmatrix != null)
                    //                {
                    //                    newbottle.Matrix = objmatrix;
                    //                }
                    //            }
                    //            createdlistview.EditView.CurrentObject = newbottle;
                    //        }
                    //    }
                    //    ShowViewParameters showViewParameters = new ShowViewParameters(createdlistview);
                    //    showViewParameters.Context = TemplateContext.PopupWindow;
                    //    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //    dc.SaveOnAccept = false;
                    //    dc.AcceptAction.Active.SetItemValue("disable", false);
                    //    dc.CancelAction.Active.SetItemValue("disable", false);
                    //    dc.ViewClosed += Dc_ViewClosing;
                    //    showViewParameters.Controllers.Add(dc);
                    //    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    //}
                    //else
                    //{
                    //    Application.ShowViewStrategy.ShowMessage("Add Test for Samples and try again", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //}
                }
                else
                {
                    //ObjectSpace.Refresh();
                    //CollectionSource cs = new CollectionSource(ObjectSpace, typeof(COCBottleSetup));
                    //cs.Criteria["filter"] = CriteriaOperator.Parse("[BottleSharingName]=?", COCInfo.strcocID);
                    //if (cs.GetCount() > 0)
                    //{
                    //    e.ShowViewParameters.Context = TemplateContext.PopupWindow;
                    //    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //    dc.SaveOnAccept = false;
                    //    dc.AcceptAction.Active.SetItemValue("disable", false);
                    //    dc.CancelAction.Active.SetItemValue("disable", false);
                    //    e.ShowViewParameters.Controllers.Add(dc);
                    //    e.ShowViewParameters.CreatedView = Application.CreateListView("COCBottleSetup_ListView", cs, false);
                    //}
                    //else
                    //{
                    //    Application.ShowViewStrategy.ShowMessage("Bottle setup not applied", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //}
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Dc_ViewClosing(object sender, EventArgs e)
        {
            try
            {
                foreach (object obj in ObjectSpace.ModifiedObjects)
                {
                    ObjectSpace.RemoveFromModifiedObjects(obj);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private string Createbottleid(CollectionSourceBase cs)
        {
            try
            {
                if (cs.GetCount() > 0)
                {
                    List<string> allbottle = new List<string>();
                    //foreach (COCBottleSetup bottle in cs.List)
                    //{
                    //    allbottle.Add(bottle.Bottleid);
                    //}
                    allbottle.Sort((a, b) => a.CompareTo(b));
                    foreach (string botid in allbottle)
                    {
                        if (botid != ((char)(allbottle.IndexOf(botid) + 65)).ToString())
                        {
                            return ((char)(allbottle.IndexOf(botid) + 65)).ToString();
                        }
                    }
                    return ((char)(allbottle.Count + 65)).ToString();
                }
                else
                {
                    return "A";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return string.Empty;
            }
        }
        private void findtest()
        {
            try
            {
                COCInfo.lstMatrix = new List<string>();
                COCInfo.lstTest = new List<string>();
                COCInfo.lstMethod = new List<Guid>();
                COCSettings objcocid = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.COCOid));
                //IList<COCSettingsSamples> objcocsamples = ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("[COC.Oid] = ?", objcocid.Oid));
                //foreach (COCSettingsSamples sampleLog in objcocsamples)
                //{
                //    if (sampleLog.VisualMatrix != null && !COCInfo.lstMatrix.Contains(sampleLog.VisualMatrix.MatrixName.MatrixName))
                //    {
                //        COCInfo.lstMatrix.Add(sampleLog.VisualMatrix.MatrixName.MatrixName);
                //    }
                //    IList<COCSettingsTest> objsample = ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[Samples.Oid] = ?", sampleLog.Oid));
                //    foreach (COCSettingsTest sample in objsample.ToList())
                //    {
                //        if (!COCInfo.lstMethod.Contains(sample.Testparameter.TestMethod.MethodName.Oid))
                //        {
                //            COCInfo.lstMethod.Add(sample.Testparameter.TestMethod.MethodName.Oid);
                //        }
                //        if (!COCInfo.lstTest.Contains(sample.Testparameter.TestMethod.TestName))
                //        {
                //            COCInfo.lstTest.Add(sample.Testparameter.TestMethod.TestName);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Previous_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (((ListView)View).EditView.CurrentObject != null && View.ObjectSpace.IsNewObject(((ListView)View).EditView.CurrentObject))
                {
                    View.ObjectSpace.RemoveFromModifiedObjects(((ListView)View).EditView.CurrentObject);
                }
                ASPxGridListEditor gridlst = (ASPxGridListEditor)((ListView)View).Editor;
                if (gridlst.Grid.VisibleRowCount > 0)
                {
                    if (gridlst.Grid.Selection.Count > 0)
                    {
                        var selectedrow = gridlst.GetSelectedObjects();
                        if (selectedrow.Count == 1)
                        {
                            //COCBottleSetup bottle = (COCBottleSetup)selectedrow[0];
                            //int i = gridlst.Grid.FindVisibleIndexByKeyValue(bottle.Oid);
                            //if (i > 0)
                            //{
                            //    gridlst.Grid.Selection.UnselectRow(i);
                            //    gridlst.Grid.Selection.SelectRow((i - 1));
                            //    ((ListView)View).EditView.CurrentObject = gridlst.Grid.GetRow((i - 1));
                            //}
                            //else
                            //{
                            //    WebWindow.CurrentRequestWindow.RegisterClientScript("nobottleid", "alert('No more bottleid available to display')");
                            //}
                        }
                    }
                    else
                    {
                        int i = gridlst.Grid.VisibleRowCount - 1;
                        gridlst.Grid.Selection.SelectRow(i);
                        ((ListView)View).EditView.CurrentObject = gridlst.Grid.GetRow(i);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Next_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //try
            //{
            //    COCBottleSetup bottle = (COCBottleSetup)((ListView)View).EditView.CurrentObject;
            //    if (bottle != null)
            //    {
            //        if (!((ListView)View).CollectionSource.List.Contains(bottle))
            //        {
            //            if (COCInfo.IsBottleidvalid == true)
            //            {
            //                if (bottle.Test.Count > 0)
            //                {
            //                    foreach (TestMethod obj in bottle.Test.ToList())
            //                    {
            //                        if (bottle.TestGroup == null || bottle.TestGroup.Length == 0)
            //                        {
            //                            bottle.TestGroup = obj.TestName;
            //                        }
            //                        else
            //                        {
            //                            if (!bottle.TestGroup.Contains(obj.TestName))
            //                            {
            //                                bottle.TestGroup = bottle.TestGroup + "," + obj.TestName;
            //                            }
            //                        }
            //                        COCInfo.lstTest.Remove(obj.TestName);
            //                    }
            //                    ((ListView)View).CollectionSource.Add(bottle);
            //                    ((ListView)View).Refresh();
            //                    ObjectSpace.CommitChanges();
            //                    if (((ListView)View).CollectionSource.GetCount() > 0)
            //                    {
            //                        bool distributevisible = false;
            //                        foreach (COCBottleSetup objcollbottle in ((ListView)View).CollectionSource.List)
            //                        {
            //                            if (objcollbottle.IsDuplicate)
            //                            {
            //                                distributevisible = true;
            //                            }
            //                        }
            //                        COCDistribution.Enabled["Enabled"] = distributevisible;
            //                    }
            //                    else
            //                    {
            //                        COCDistribution.Enabled["Enabled"] = false;
            //                    }
            //                    if (COCInfo.lstTest.Count > 0)
            //                    {
            //                        COCBottleSetup newbottle = ObjectSpace.CreateObject<COCBottleSetup>();
            //                        newbottle.Bottleid = Createbottleid(((ListView)View).CollectionSource);
            //                        newbottle.Bottlesharingname = COCInfo.strcocID;
            //                        if (COCInfo.lstMatrix.Count == 1)
            //                        {
            //                            Matrix objmatrix = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]=?", COCInfo.lstMatrix[0]));
            //                            if (objmatrix != null)
            //                            {
            //                                newbottle.Matrix = objmatrix;
            //                            }
            //                        }
            //                        ((ListView)View).EditView.CurrentObject = newbottle;
            //                    }
            //                    else
            //                    {
            //                        ((ListView)View).EditView.CurrentObject = null;
            //                        WebWindow.CurrentRequestWindow.RegisterClientScript("nobottleid", "alert('No more bottle ID can be created, Either Save or Cancel');");
            //                    }
            //                }
            //                else
            //                {
            //                    WebWindow.CurrentRequestWindow.RegisterClientScript("nobottleid", "alert('Select the Test to add in Bottle Id');");
            //                }
            //            }
            //            else
            //            {
            //                WebWindow.CurrentRequestWindow.RegisterClientScript("save", "alert('Bottle ID already exist, please enter new Bottle ID')");
            //            }
            //        }
            //        else
            //        {
            //            if (ObjectSpace.ModifiedObjects.Contains(bottle))
            //            {
            //                bottle.TestGroup = null;
            //                foreach (TestMethod obj in bottle.Test.ToList())
            //                {
            //                    if (bottle.TestGroup == null || bottle.TestGroup.Length == 0)
            //                    {
            //                        bottle.TestGroup = obj.TestName;
            //                    }
            //                    else
            //                    {
            //                        if (!bottle.TestGroup.Contains(obj.TestName))
            //                        {
            //                            bottle.TestGroup = bottle.TestGroup + "," + obj.TestName;
            //                        }
            //                    }
            //                    COCInfo.lstTest.Remove(obj.TestName);
            //                }
            //                ObjectSpace.CommitChanges();
            //            }
            //            ASPxGridListEditor gridlst = (ASPxGridListEditor)((ListView)View).Editor;
            //            if (gridlst.Grid.Selection.Count > 0)
            //            {
            //                var selectedrow = gridlst.GetSelectedObjects();
            //                if (selectedrow.Count == 1)
            //                {
            //                    COCBottleSetup objbottle = (COCBottleSetup)selectedrow[0];
            //                    int i = gridlst.Grid.FindVisibleIndexByKeyValue(objbottle.Oid);
            //                    if (i < (gridlst.Grid.VisibleRowCount - 1))
            //                    {
            //                        gridlst.Grid.Selection.UnselectRow(i);
            //                        gridlst.Grid.Selection.SelectRow((i + 1));
            //                        ((ListView)View).EditView.CurrentObject = gridlst.Grid.GetRow((i + 1));
            //                    }
            //                    else
            //                    {
            //                        gridlst.Grid.Selection.UnselectRow(i);
            //                        if (COCInfo.lstTest.Count > 0)
            //                        {
            //                            COCBottleSetup newbottle = ObjectSpace.CreateObject<COCBottleSetup>();
            //                            newbottle.Bottleid = Createbottleid(((ListView)View).CollectionSource);
            //                            newbottle.Bottlesharingname = COCInfo.strcocID;
            //                            if (COCInfo.lstMatrix.Count == 1)
            //                            {
            //                                Matrix objmatrix = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]=?", COCInfo.lstMatrix[0]));
            //                                if (objmatrix != null)
            //                                {
            //                                    newbottle.Matrix = objmatrix;
            //                                }
            //                            }
            //                            ((ListView)View).EditView.CurrentObject = newbottle;
            //                        }
            //                        else
            //                        {
            //                            ((ListView)View).EditView.CurrentObject = null;
            //                            foreach (COCBottleSetup modbottle in ((ListView)View).CollectionSource.List)
            //                            {
            //                                if (ObjectSpace.ModifiedObjects.Contains(modbottle))
            //                                {
            //                                    WebWindow.CurrentRequestWindow.RegisterClientScript("nobottleid", "alert('No more bottle ID can be created, Either Save or Cancel');");
            //                                    break;
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        bool alertshow = true;
            //        foreach (COCBottleSetup modbottle in ((ListView)View).CollectionSource.List)
            //        {
            //            if (ObjectSpace.ModifiedObjects.Contains(modbottle))
            //            {
            //                WebWindow.CurrentRequestWindow.RegisterClientScript("nobottleid", "alert('No more bottle ID can be created, Either Save or Cancel');");
            //                alertshow = false;
            //                break;
            //            }
            //        }
            //        if (alertshow)
            //        {
            //            WebWindow.CurrentRequestWindow.RegisterClientScript("nobottleid", "alert('No more bottleid available to display')");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            //}
        }

        private void Saveall_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //COCBottleSetup bottle = (COCBottleSetup)((ListView)View).EditView.CurrentObject;
                //if ((((ListView)View).CollectionSource.GetCount() > 0) || (bottle != null && bottle.Test.Count > 0))
                //{
                //    if (COCInfo.IsBottleidvalid == true)
                //    {
                //        if (((ListView)View).EditView.CurrentObject != null)
                //        {
                //            if (((ListView)View).CollectionSource.List.Contains(bottle))
                //            {
                //                bottle.TestGroup = string.Empty;
                //                foreach (TestMethod obj in bottle.Test.ToList())
                //                {
                //                    if (bottle.TestGroup == null || bottle.TestGroup.Length == 0)
                //                    {
                //                        bottle.TestGroup = obj.TestName;
                //                    }
                //                    else
                //                    {
                //                        if (!bottle.TestGroup.Contains(obj.TestName))
                //                        {
                //                            bottle.TestGroup = bottle.TestGroup + "," + obj.TestName;
                //                        }
                //                    }
                //                    COCInfo.lstTest.Remove(obj.TestName);
                //                }
                //            }
                //            else
                //            {
                //                if (bottle.Test.Count == 0)
                //                {
                //                    View.ObjectSpace.RemoveFromModifiedObjects(((ListView)View).EditView.CurrentObject);
                //                }
                //                else
                //                {
                //                    bottle.TestGroup = string.Empty;
                //                    foreach (TestMethod obj in bottle.Test.ToList())
                //                    {
                //                        if (bottle.TestGroup == null || bottle.TestGroup.Length == 0)
                //                        {
                //                            bottle.TestGroup = obj.TestName;
                //                        }
                //                        else
                //                        {
                //                            if (!bottle.TestGroup.Contains(obj.TestName))
                //                            {
                //                                bottle.TestGroup = bottle.TestGroup + "," + obj.TestName;
                //                            }
                //                        }
                //                        COCInfo.lstTest.Remove(obj.TestName);
                //                    }
                //                    ((ListView)View).CollectionSource.Add(bottle);
                //                }
                //            }
                //        }
                //        COCSettings objsamplecheckin = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("COC_ID=" + COCInfo.strcocID));
                //        IList<COCSettingsSamples> objsampleLogin = ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("COC.Oid=?", objsamplecheckin.Oid));
                //        foreach (COCSettingsSamples sampleLog in objsampleLogin)
                //        {
                //            List<string> container = new List<string>();
                //            IList<COCSettingsTest> objsample = ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("Samples=?", sampleLog.Oid));
                //            foreach (COCSettingsTest sample in objsample.ToList())
                //            {
                //                sample.Bottle = null;
                //                sample.DupBottle = 0;
                //                foreach (COCBottleSetup objbottle in ((ListView)View).CollectionSource.List)
                //                {
                //                    if (objbottle.Test.Contains(sample.Testparameter.TestMethod) && objbottle.SampleContainer.Contains(sampleLog.SampleID))
                //                    {
                //                        string id = objbottle.SampleContainer.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).First(a => a.Split('(').FirstOrDefault() == sampleLog.SampleID);
                //                        if (id != null)
                //                        {
                //                            sample.DupBottle = Convert.ToInt32(id.Split('(').Skip(1).FirstOrDefault().Replace(")", ""));
                //                        }
                //                        sample.Bottle = objbottle;
                //                        if (!container.Contains(objbottle.Bottleid))
                //                        {
                //                            container.Add(objbottle.Bottleid);
                //                        }
                //                        break;
                //                    }
                //                }
                //            }
                //            sampleLog.Containers = (uint)container.Count;
                //        }
                //        ObjectSpace.CommitChanges();
                //        (Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(true);
                //        Application.ShowViewStrategy.ShowMessage("Saved successfully!", InformationType.Success, timer.Seconds, InformationPosition.Top);
                //    }
                //    else
                //    {
                //        WebWindow.CurrentRequestWindow.RegisterClientScript("save", "alert('Bottle ID already exist, please enter new Bottle ID')");
                //    }
                //}
                //else
                //{
                //    WebWindow.CurrentRequestWindow.RegisterClientScript("nosave", "alert('Create bottle id to save')");
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Clearall_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.ObjectSpace.ModifiedObjects.Count > 0)
                {
                    View.ObjectSpace.Rollback(false);
                    COCSaveall.ConfirmationMessage = null;
                    COCInfo.strMatrix = string.Empty;
                    COCInfo.lstTest = new List<string>();
                    IList<TestMethod> objTest = new List<TestMethod>();
                    Samplecheckin objsamplecheckin = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("JobID=?", COCInfo.strcocID));
                    IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> objsampleLogin = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("JobID=?", objsamplecheckin.Oid));
                    foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn sampleLog in objsampleLogin)
                    {
                        IList<SampleParameter> objsample = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Samplelogin=?", sampleLog.Oid));
                        foreach (SampleParameter sample in objsample.ToList())
                        {
                            if (!objTest.Contains(sample.Testparameter.TestMethod))
                            {
                                objTest.Add(sample.Testparameter.TestMethod);
                                COCInfo.lstTest.Add(sample.Testparameter.TestMethod.TestName);
                            }
                        }
                    }
                    foreach (TestMethod objbottlecoll in objTest.ToList())
                    {
                        //foreach (COCBottleSetup setup in ((ListView)View).CollectionSource.List)
                        //{
                        //    if (setup.Test.Contains(objbottlecoll))
                        //    {
                        //        objTest.Remove(objbottlecoll);
                        //        COCInfo.lstTest.Remove(objbottlecoll.TestName);
                        //    }
                        //}
                    }
                    if (objTest.Count > 0)
                    {
                        //COCBottleSetup newbottle = ObjectSpace.CreateObject<COCBottleSetup>();
                        //newbottle.Bottleid = Createbottleid(((ListView)View).CollectionSource);
                        //newbottle.Bottlesharingname = COCInfo.strcocID;
                        //if (COCInfo.lstMatrix.Count == 1)
                        //{
                        //    Matrix objmatrix = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]=?", COCInfo.lstMatrix[0]));
                        //    if (objmatrix != null)
                        //    {
                        //        newbottle.Matrix = objmatrix;
                        //    }
                        //}
                        //((ListView)View).EditView.CurrentObject = newbottle;
                    }
                    else
                    {
                        ((ListView)View).CurrentObject = null;
                        ((ListView)View).EditView.CurrentObject = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DistributionNext_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem Bottledetail = ((DashboardView)View).FindItem("BottleDetail") as DashboardViewItem;
                DashboardViewItem BottleList = ((DashboardView)View).FindItem("BottleList") as DashboardViewItem;
                StaticTextViewItem BottlePage = ((DashboardView)View).FindItem("BottlePage") as StaticTextViewItem;
                //if (Bottledetail != null && BottleList != null)
                //{
                //    int totqty = 0;
                //    COCBottleSetup bottle = (COCBottleSetup)((DetailView)Bottledetail.InnerView).CurrentObject;
                //    foreach (COCBottlesetupdistribution objdis in ((ListView)BottleList.InnerView).CollectionSource.List)
                //    {
                //        totqty += objdis.Qty;
                //    }
                //    if (totqty == bottle.Duplicate)
                //    {
                //        if (((DetailView)Bottledetail.InnerView).ObjectSpace.IsModified)
                //            Bottledetail.InnerView.ObjectSpace.CommitChanges();
                //    }
                //    else
                //    {
                //        WebWindow.CurrentRequestWindow.RegisterClientScript("save", "alert('Qty for this container is not correct, fix this to continue')");
                //        return;
                //    }
                //    IList<COCBottleSetup> bottles = Bottledetail.InnerView.ObjectSpace.GetObjects<COCBottleSetup>(CriteriaOperator.Parse("[BottleSharingName] =? and [IsDuplicate] = True", COCInfo.strcocID), true);
                //    var newbottles = bottles.OrderBy(x => x.Bottleid);
                //    var curbottle = ((DetailView)Bottledetail.InnerView).CurrentObject;
                //    int index = newbottles.ToList().FindIndex(a => a == curbottle);
                //    if (index < newbottles.Count() - 1)
                //    {
                //        ((ListView)BottleList.InnerView).CollectionSource.ResetCollection();
                //        BottlePage.Text = (index + 2).ToString() + " of " + bottles.Count.ToString();
                //        ((DetailView)Bottledetail.InnerView).CurrentObject = newbottles.ToList()[index + 1];
                //        string[] sample = newbottles.ToList()[index + 1].SampleContainer.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                //        Random random = new Random();
                //        foreach (string str in sample)
                //        {
                //            COCBottlesetupdistribution objbot = new COCBottlesetupdistribution();
                //            objbot.ID = random.Next();
                //            objbot.SampleID = str.Split('(').FirstOrDefault();
                //            objbot.Qty = Convert.ToInt32(str.Split('(').Skip(1).FirstOrDefault().Replace(")", ""));
                //            ((ListView)BottleList.InnerView).CollectionSource.Add(objbot);
                //            ((ListView)BottleList.InnerView).Refresh();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DistributionPrevious_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem Bottledetail = ((DashboardView)View).FindItem("BottleDetail") as DashboardViewItem;
                DashboardViewItem BottleList = ((DashboardView)View).FindItem("BottleList") as DashboardViewItem;
                StaticTextViewItem BottlePage = ((DashboardView)View).FindItem("BottlePage") as StaticTextViewItem;
                //if (Bottledetail != null && BottleList != null)
                //{
                //    int totqty = 0;
                //    COCBottleSetup bottle = (COCBottleSetup)((DetailView)Bottledetail.InnerView).CurrentObject;
                //    foreach (COCBottlesetupdistribution objdis in ((ListView)BottleList.InnerView).CollectionSource.List)
                //    {
                //        totqty += objdis.Qty;
                //    }
                //    if (totqty == bottle.Duplicate)
                //    {
                //        if (((DetailView)Bottledetail.InnerView).ObjectSpace.IsModified)
                //            Bottledetail.InnerView.ObjectSpace.CommitChanges();
                //    }
                //    else
                //    {
                //        WebWindow.CurrentRequestWindow.RegisterClientScript("save", "alert('Qty for this container is not correct, fix this to continue')");
                //        return;
                //    }
                //    IList<COCBottleSetup> bottles = Bottledetail.InnerView.ObjectSpace.GetObjects<COCBottleSetup>(CriteriaOperator.Parse("[BottleSharingName] =? and [IsDuplicate] = True", COCInfo.strcocID), true);
                //    var newbottles = bottles.OrderBy(x => x.Bottleid);
                //    var curbottle = ((DetailView)Bottledetail.InnerView).CurrentObject;
                //    int index = newbottles.ToList().FindIndex(a => a == curbottle);
                //    if (index > 0)
                //    {
                //        ((ListView)BottleList.InnerView).CollectionSource.ResetCollection();
                //        BottlePage.Text = ((index + 1) - 1).ToString() + " of " + bottles.Count.ToString();
                //        ((DetailView)Bottledetail.InnerView).CurrentObject = newbottles.ToList()[index - 1];
                //        string[] sample = newbottles.ToList()[index - 1].SampleContainer.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                //        Random random = new Random();
                //        foreach (string str in sample)
                //        {
                //            COCBottlesetupdistribution objbot = new COCBottlesetupdistribution();
                //            objbot.ID = random.Next();
                //            objbot.SampleID = str.Split('(').FirstOrDefault();
                //            objbot.Qty = Convert.ToInt32(str.Split('(').Skip(1).FirstOrDefault().Replace(")", ""));
                //            ((ListView)BottleList.InnerView).CollectionSource.Add(objbot);
                //            ((ListView)BottleList.InnerView).Refresh();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Distribution_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //COCBottleSetup bottle = (COCBottleSetup)((ListView)View).EditView.CurrentObject;
            //if (((ListView)View).EditView.CurrentObject != null)
            //{
            //    if (((ListView)View).CollectionSource.List.Contains(bottle))
            //    {
            //        bottle.TestGroup = string.Empty;
            //        foreach (TestMethod obj in bottle.Test.ToList())
            //        {
            //            if (bottle.TestGroup == null || bottle.TestGroup.Length == 0)
            //            {
            //                bottle.TestGroup = obj.TestName;
            //            }
            //            else
            //            {
            //                if (!bottle.TestGroup.Contains(obj.TestName))
            //                {
            //                    bottle.TestGroup = bottle.TestGroup + "," + obj.TestName;
            //                }
            //            }
            //            COCInfo.lstTest.Remove(obj.TestName);
            //        }
            //    }
            //    else
            //    {
            //        if (bottle.Test.Count == 0)
            //        {
            //            View.ObjectSpace.RemoveFromModifiedObjects(((ListView)View).EditView.CurrentObject);
            //        }
            //        else
            //        {
            //            bottle.TestGroup = string.Empty;
            //            foreach (TestMethod obj in bottle.Test.ToList())
            //            {
            //                if (bottle.TestGroup == null || bottle.TestGroup.Length == 0)
            //                {
            //                    bottle.TestGroup = obj.TestName;
            //                }
            //                else
            //                {
            //                    if (!bottle.TestGroup.Contains(obj.TestName))
            //                    {
            //                        bottle.TestGroup = bottle.TestGroup + "," + obj.TestName;
            //                    }
            //                }
            //                COCInfo.lstTest.Remove(obj.TestName);
            //            }
            //            ((ListView)View).CollectionSource.Add(bottle);
            //        }
            //    }
            //}
            //View.ObjectSpace.CommitChanges();
            //DashboardView dashboardView = Application.CreateDashboardView(ObjectSpace, "BottleSetupDistributionCOC", false);
            //ShowViewParameters showViewParameters = new ShowViewParameters(dashboardView);
            //showViewParameters.Context = TemplateContext.PopupWindow;
            //showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            //DialogController dc = Application.CreateController<DialogController>();
            //dc.SaveOnAccept = false;
            //dc.AcceptAction.Active.SetItemValue("disable", false);
            //dc.CancelAction.Active.SetItemValue("disable", false);
            //dc.Accepting += Dc_Accepting;
            //showViewParameters.Controllers.Add(dc);
            //Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //(Frame as DevExpress.ExpressApp.Web.PopupWindow).SetView(dashboardView);
        }
        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                View.ObjectSpace.Refresh();
                ((ListView)View).CollectionSource.ResetCollection();
                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[BottleSharingName]=?", COCInfo.strcocID);
                if (COCInfo.lstTest.Count > 0)
                {
                    //COCBottleSetup newbottle = ObjectSpace.CreateObject<COCBottleSetup>();
                    //newbottle.Bottleid = Createbottleid(((ListView)View).CollectionSource);
                    //newbottle.Bottlesharingname = COCInfo.strcocID;
                    //if (COCInfo.lstMatrix.Count == 1)
                    //{
                    //    Matrix objmatrix = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]=?", COCInfo.lstMatrix[0]));
                    //    if (objmatrix != null)
                    //    {
                    //        newbottle.Matrix = objmatrix;
                    //    }
                    //}
                    //((ListView)View).EditView.CurrentObject = newbottle;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DistributionClose_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem Bottledetail = ((DashboardView)View).FindItem("BottleDetail") as DashboardViewItem;
                DashboardViewItem BottleList = ((DashboardView)View).FindItem("BottleList") as DashboardViewItem;
                if (Bottledetail != null && BottleList != null)
                {
                    //int totqty = 0;
                    //COCBottleSetup bottle = (COCBottleSetup)((DetailView)Bottledetail.InnerView).CurrentObject;
                    //foreach (COCBottlesetupdistribution objdis in ((ListView)BottleList.InnerView).CollectionSource.List)
                    //{
                    //    totqty += objdis.Qty;
                    //}
                    //if (totqty == bottle.Duplicate)
                    //{
                    //    if (((DetailView)Bottledetail.InnerView).ObjectSpace.IsModified)
                    //    {
                    //        Bottledetail.InnerView.ObjectSpace.CommitChanges();
                    //    }
                    //    Frame.GetController<DialogController>().AcceptAction.Active.SetItemValue("disable", true);
                    //    Frame.GetController<DialogController>().AcceptAction.DoExecute();
                    //    //(Frame as DevExpress.ExpressApp.Web.PopupWindow).Close();
                    //}
                    //else
                    //{
                    //    WebWindow.CurrentRequestWindow.RegisterClientScript("save", "alert('Qty for this container is not correct, fix this to continue')");
                    //    return;
                    //}
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
                if (View.Id == "VisualMatrix_ListView_COCSettings_SampleMatrix")
                {
                    View.ControlsCreated += View_ControlsCreated;
                    List<Guid> vmguid = new List<Guid>();
                    COCSettings objTasks = ObjectSpace.FindObject<COCSettings>(CriteriaOperator.Parse("COC_ID=" + COCInfo.strcocID));
                    //IList<COCSettingsSamples> lstsampling = ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("COC.Oid=?", objTasks.Oid));
                    //if (lstsampling != null && lstsampling.Count > 0)
                    //{
                    //    foreach (COCSettingsSamples objsampling in lstsampling.ToList())
                    //    {
                    //        if (objsampling.VisualMatrix != null && objsampling.VisualMatrix.Oid != null)
                    //        {
                    //            if (!vmguid.Contains(objsampling.VisualMatrix.Oid))
                    //            {
                    //                vmguid.Add(objsampling.VisualMatrix.Oid);
                    //            }
                    //        }
                    //    }
                    //    if (vmguid != null && vmguid.Count > 0)
                    //    {
                    //        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", vmguid.Select(i => i.ToString().Replace("'", "''")))) + ")");
                    //    }
                    //    else
                    //    {
                    //        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                    //    }
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                    //}
                }
                else
                {
                    DashboardViewItem Bottledetail = ((DashboardView)View).FindItem("BottleDetail") as DashboardViewItem;
                    DashboardViewItem BottleList = ((DashboardView)View).FindItem("BottleList") as DashboardViewItem;
                    StaticTextViewItem BottlePage = ((DashboardView)View).FindItem("BottlePage") as StaticTextViewItem;
                    //if (Bottledetail != null && BottleList != null && BottlePage != null && ((DetailView)Bottledetail.InnerView).CurrentObject == null)
                    //{
                    //    IList<COCBottleSetup> bottles = Bottledetail.InnerView.ObjectSpace.GetObjects<COCBottleSetup>(CriteriaOperator.Parse("[BottleSharingName] =? and [IsDuplicate] = True", COCInfo.strcocID), true);
                    //    var newbottles = bottles.OrderBy(x => x.Bottleid);
                    //    BottlePage.Text = "1 of " + bottles.Count.ToString();
                    //    ((DetailView)Bottledetail.InnerView).CurrentObject = newbottles.FirstOrDefault();
                    //    if (newbottles != null && !string.IsNullOrEmpty(newbottles.FirstOrDefault().SampleContainer))
                    //    {
                    //        string[] sample = newbottles.FirstOrDefault().SampleContainer.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    //        Random random = new Random();
                    //        foreach (string str in sample)
                    //        {
                    //            COCBottlesetupdistribution objbot = new COCBottlesetupdistribution();
                    //            objbot.ID = random.Next();
                    //            objbot.SampleID = str.Split('(').FirstOrDefault();
                    //            objbot.Qty = Convert.ToInt32(str.Split('(').Skip(1).FirstOrDefault().Replace(")", ""));
                    //            ((ListView)BottleList.InnerView).CollectionSource.Add(objbot);
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void COCBottleDeleteSetup_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //COCBottleSetup curbottle = (COCBottleSetup)View.CurrentObject;
                //foreach (TestMethod test in curbottle.Test.ToList())
                //{
                //    //curbottle.Test.Remove(test);
                //    if (!COCInfo.lstTest.Contains(test.TestName))
                //    {
                //        COCInfo.lstTest.Add(test.TestName);
                //    }
                //}
                //((ListView)View).CollectionSource.Remove(curbottle);
                //  ((ListView)View).CurrentObject = null;
                //  ObjectSpace.Delete(curbottle);
                //  ObjectSpace.CommitChanges();
                //  if (((ListView)View).CollectionSource.GetCount() > 0)
                //  {
                //      bool distributevisible = false;
                //      foreach (COCBottleSetup objcollbottle in ((ListView)View).CollectionSource.List)
                //      {
                //          if (objcollbottle.IsDuplicate)
                //          {
                //              distributevisible = true;
                //          }
                //      }
                //      COCDistribution.Enabled["Enabled"] = distributevisible;
                //  }
                //  else
                //  {
                //      COCDistribution.Enabled["Enabled"] = false;
                //  }
                //  if (COCInfo.lstTest.Count > 0)
                //  {
                //      COCBottleSetup newbottle = ObjectSpace.CreateObject<COCBottleSetup>();
                //      newbottle.Bottleid = Createbottleid(((ListView)View).CollectionSource);
                //      newbottle.Bottlesharingname = COCInfo.strcocID;
                //      if (COCInfo.lstMatrix.Count == 1)
                //      {
                //          Matrix objmatrix = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]=?", COCInfo.lstMatrix[0]));
                //          if (objmatrix != null)
                //          {
                //              newbottle.Matrix = objmatrix;
                //          }
                //      }
                //      ((ListView)View).EditView.CurrentObject = newbottle;
                //  }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void BottleAllocationRemove_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string msg;
                if (View != null && View.SelectedObjects.Count > 0)
                {
                    msg = "Do you want to delete bottle allocation set";
                    WebWindow.CurrentRequestWindow.RegisterClientScript("CloseWeighingBatch", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('" + msg + "'); {0}", BottleDelcallbackManager.CallbackManager.GetScript("CanDeleteTaskBottleAllocation", "openconfirm")));
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Select bottle allocation check box", InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void BottleAllocationAdd_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (COCInfo.SamplingGuid != Guid.Empty && COCInfo.SamplingGuid != null)
                {
                    int testcnt = 0;
                    List<Guid> lsttestmethodguid = new List<Guid>();
                    List<string> lsttestname = new List<string>();
                    //IList<COCSettingsTest> lstsmpltest = ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[Samples.Oid] = ?", COCInfo.SamplingGuid));
                    //if (lstsmpltest != null && lstsmpltest.Count > 0)
                    //{
                    //    foreach (COCSettingsTest objsmpltest in lstsmpltest.ToList())
                    //    {
                    //        if (!lsttestname.Contains(objsmpltest.Testparameter.TestMethod.TestName))
                    //        {
                    //            lsttestname.Add(objsmpltest.Testparameter.TestMethod.TestName);
                    //            lsttestmethodguid.Add(objsmpltest.Testparameter.TestMethod.Oid);
                    //        }
                    //    }
                    //}
                    //testcnt = lsttestmethodguid.Count(); if (lstsmpltest != null && lstsmpltest.Count > 0)
                    //{
                    //    IList<SampleBottleAllocation> lstsmplbtlalc = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[COCSettings.Oid] = ?", COCInfo.SamplingGuid));
                    //    if (lstsmplbtlalc != null && lstsmplbtlalc.Count > 0) //)
                    //    {
                    //        string strsharedtest = string.Empty;
                    //        foreach (SampleBottleAllocation objsmplbtlalc in lstsmplbtlalc.ToList())
                    //        {
                    //            if (objsmplbtlalc != null && !string.IsNullOrEmpty(objsmplbtlalc.SharedTestsGuid))
                    //            {
                    //                if (string.IsNullOrEmpty(strsharedtest))
                    //                {
                    //                    strsharedtest = objsmplbtlalc.SharedTestsGuid;
                    //                }
                    //                else
                    //                {
                    //                    strsharedtest = strsharedtest + ";" + objsmplbtlalc.SharedTestsGuid;
                    //                }
                    //            }
                    //        }
                    //        if (!string.IsNullOrEmpty(strsharedtest))
                    //        {
                    //            string[] arrsharedtest = strsharedtest.Split(';');
                    //            if (testcnt > arrsharedtest.Count())
                    //            {
                    //                COCSettingsSamples objsampling = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.SamplingGuid));
                    //                IObjectSpace ossmpl = Application.CreateObjectSpace(typeof(SampleBottleAllocation));
                    //                SampleBottleAllocation objsmpl = ObjectSpace.CreateObject<SampleBottleAllocation>();
                    //                objsmpl.BottleSet = (Convert.ToInt32(((XPObjectSpace)ossmpl).Session.Evaluate(typeof(SampleBottleAllocation), CriteriaOperator.Parse("MAX(BottleSet)"), CriteriaOperator.Parse("[COCSettings.Oid]=?", objsampling.Oid))) + 1); ;
                    //                objsmpl.Qty = 1;
                    //                objsmpl.COCSettings = objsampling;
                    //                ObjectSpace.CommitChanges();
                    //                ((ListView)View).CollectionSource.Criteria.Clear();
                    //                ((ListView)View).CollectionSource.Add(objsmpl);
                    //                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[COCSettings.Oid] = ?", COCInfo.SamplingGuid);
                    //                //Application.ShowViewStrategy.ShowMessage("Row added successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
                    //            }
                    //            else
                    //            {
                    //                Application.ShowViewStrategy.ShowMessage("Selected sampleID's available test have an empty, please add test and continue.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            Application.ShowViewStrategy.ShowMessage("Empty row already added", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //        }
                    //    }
                    //    else if (lstsmplbtlalc != null && lstsmplbtlalc.Count == 0)
                    //    {
                    //        COCSettingsSamples objsampling = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.SamplingGuid));
                    //        IObjectSpace ossmpl = Application.CreateObjectSpace(typeof(SampleBottleAllocation));
                    //        SampleBottleAllocation objsmpl = ObjectSpace.CreateObject<SampleBottleAllocation>();
                    //        objsmpl.BottleSet = (Convert.ToInt32(((XPObjectSpace)ossmpl).Session.Evaluate(typeof(SampleBottleAllocation), CriteriaOperator.Parse("MAX(BottleSet)"), CriteriaOperator.Parse("[COCSettings.Oid]=?", objsampling.Oid))) + 1); ;
                    //        objsmpl.Qty = 1;
                    //        objsmpl.COCSettings = objsampling;
                    //        ObjectSpace.CommitChanges();
                    //        ((ListView)View).CollectionSource.Criteria.Clear();
                    //        ((ListView)View).CollectionSource.Add(objsmpl);
                    //        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[COCSettings.Oid] = ?", COCInfo.SamplingGuid);
                    //    }
                    //}
                    //else
                    //{
                    //    Application.ShowViewStrategy.ShowMessage("Selected sampleID doesn't have a test, please add test and continue.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //}
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Select sampleID check box", InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
                //COCSettingsSamples objsampling = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.SamplingGuid));
                //IObjectSpace ossmpl = Application.CreateObjectSpace(typeof(SampleBottleAllocation));
                //SampleBottleAllocation objsmpl = ObjectSpace.CreateObject<SampleBottleAllocation>();
                //objsmpl.BottleSet = (Convert.ToInt32(((XPObjectSpace)ossmpl).Session.Evaluate(typeof(SampleBottleAllocation), CriteriaOperator.Parse("MAX(BottleSet)"), CriteriaOperator.Parse("[COCSettings.Oid]=?", objsampling.Oid))) + 1); ;
                //objsmpl.Qty = 1;
                //objsmpl.COCSettings = objsampling;
                //ObjectSpace.CommitChanges();
                //((ListView)View).CollectionSource.Criteria.Clear();
                //((ListView)View).CollectionSource.Add(objsmpl);
                //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[COCSettings.Oid] = ?", COCInfo.SamplingGuid);
                //Application.ShowViewStrategy.ShowMessage("Row added successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void COCCopyBottleSet_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem lvSampleID = ((DashboardView)Application.MainWindow.View).FindItem("SampleID") as DashboardViewItem;
                //if (lvSampleID != null && lvSampleID.InnerView != null && ((ListView)lvSampleID.InnerView).CollectionSource.List.Count > 1)
                //{
                //    IObjectSpace os = Application.CreateObjectSpace();
                //    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(COCSettingsSamples));
                //    ListView lv = Application.CreateListView("COCSettingsSamples_ListView_COCBottle_CopyTOBottleAllocation", cs, false);
                //    ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                //    showViewParameters.Context = TemplateContext.PopupWindow;
                //    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                //    showViewParameters.CreatedView.Caption = "SampleID";
                //    DialogController dc = Application.CreateController<DialogController>();
                //    dc.SaveOnAccept = false;
                //    dc.CloseOnCurrentObjectProcessing = false;
                //    //dc.AcceptAction.Active.SetItemValue("OK", false);
                //    dc.Accepting += CopyToSampleID_AcceptAction_Execute;
                //    showViewParameters.Controllers.Add(dc);
                //    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                //    COCInfo.Ispopup = false;
                //}
                //else //if(lvSampleID != null && lvSampleID.InnerView != null && ((ListView)lvSampleID.InnerView).CollectionSource.List.Count == 0)
                //{
                //    Application.ShowViewStrategy.ShowMessage("Add atleast two sampleID's", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopyToSampleID_AcceptAction_Execute(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DialogController dialog = (DialogController)sender;
                    if (dialog.Window.View.Id == "COCSettingsSamples_ListView_COCBottle_CopyTOBottleAllocation")
                    {
                        IList<SampleBottleAllocation> lstextobjsmplbtl = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[COCSettings.Oid] = ?", COCInfo.SamplingGuid));
                        //DashboardViewItem DVCopyToSampleID = ((DetailView)dialog.Window.View).FindItem("CopyToBottleAllocation") as DashboardViewItem;
                        //if (dialog.Window.View.SelectedObjects.Count > 0)
                        //{
                        //    foreach (COCSettingsSamples objcocsamples in dialog.Window.View.SelectedObjects)
                        //    {
                        //        IList<SampleBottleAllocation> lstdelobjsmplbtl = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[COCSettings.Oid] = ?", objcocsamples));
                        //        if (lstdelobjsmplbtl != null && lstdelobjsmplbtl.Count > 0)
                        //        {
                        //            ObjectSpace.Delete(lstdelobjsmplbtl);
                        //            ObjectSpace.CommitChanges();
                        //        }
                        //        foreach (SampleBottleAllocation objextsmplbtl in lstextobjsmplbtl.ToList())
                        //        {
                        //            COCSettingsSamples objsampling = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", objcocsamples));
                        //            SampleBottleAllocation objcrtsmplbtl = ObjectSpace.CreateObject<SampleBottleAllocation>();
                        //            objcrtsmplbtl.BottleSet = objextsmplbtl.BottleSet;
                        //            objcrtsmplbtl.SharedTestsGuid = objextsmplbtl.SharedTestsGuid;
                        //            objcrtsmplbtl.SharedTests = objextsmplbtl.SharedTests;
                        //            objcrtsmplbtl.Qty = objextsmplbtl.Qty;
                        //            objcrtsmplbtl.BottleID = objextsmplbtl.BottleID;
                        //            objcrtsmplbtl.Containers = objextsmplbtl.Containers;
                        //            objcrtsmplbtl.Preservative = objextsmplbtl.Preservative;
                        //            objcrtsmplbtl.COCSettings = objsampling;
                        //        }
                        //    }

                        //    Application.ShowViewStrategy.ShowMessage("Bottle allocation copied successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
                        //    ObjectSpace.CommitChanges();
                        //    ObjectSpace.Refresh();
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
    }
}
