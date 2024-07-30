using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using Module.BusinessObjects.Settings;
using Modules.BusinessObjects.BarCodeSampleCustody;
//using Modules.BusinessObjects.BarCodeSampleCustody;
//using Modules.BusinessObjects.ClientServiceManagement;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.TrendAnalysis;
//using Modules.BusinessObjects.Setting.CalibrationNotebook;
//using Modules.BusinessObjects.Setting.CCID;
//using Modules.BusinessObjects.Setting.Invoicing;
//using Modules.BusinessObjects.Setting.NCAID;
//using Modules.BusinessObjects.TaskManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
//using XCRM.Module.BusinessObjects;
//using static Modules.BusinessObjects.Setting.NCAID.NonConformityInitiation;
using static Modules.BusinessObjects.Setting.ProjectCategory;

namespace LDM.Module.Controllers.ICM
{
    public partial class NavigationRefreshController : WindowController
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        ShowNavigationItemController ShowNavigationController;
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        PermissionInfo objpermission = new PermissionInfo();
        ICMinfo Filter = new ICMinfo();
        TestInfo testInfo = new TestInfo();
        ResultEntryQueryPanelInfo objQPInfo = new ResultEntryQueryPanelInfo();
        QCResultValidationQueryPanelInfo objDRQPInfo = new QCResultValidationQueryPanelInfo();
        ReportingQueryPanelInfo objRQPInfo = new ReportingQueryPanelInfo();
        NavigationInfo objNavInfo = new NavigationInfo();
        public const string EnabledKeyShowDetailView = "ShowDetailViewFromListViewController";
        #endregion

        #region Constructor
        public NavigationRefreshController()
        {
            InitializeComponent();
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                string strProductName = ConfigurationManager.AppSettings["ProductName"];
                AboutInfo.Instance.ProductName = strProductName;
                AboutInfo.Instance.Copyright = "Powered by BTSoft";
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
        #endregion

        #region Function
        protected override void OnFrameAssigned()
        {

            try
            {
                base.OnFrameAssigned();
                UnSubscribeEvent();
                SubscribeEvent();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }


        }



        private void SubscribeEvent()
        {
            try
            {
                ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                if (ShowNavigationController != null)
                {
                    ShowNavigationController.CustomShowNavigationItem += ShowNavigationController_CustomShowNavigationItem;
                    ShowNavigationController.ItemsInitialized += ShowNavigationController_ItemsInitialized;
                    ShowNavigationController.NavigationItemCreated += ShowNavigationItemController_NavigationItemCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void UnSubscribeEvent()
        {
            try
            {
                ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                if (ShowNavigationController != null)
                {
                    ShowNavigationController.CustomShowNavigationItem -= ShowNavigationController_CustomShowNavigationItem;
                    ShowNavigationController.ItemsInitialized -= ShowNavigationController_ItemsInitialized;
                    ShowNavigationController.NavigationItemCreated -= ShowNavigationItemController_NavigationItemCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events

        private void ShowNavigationController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            try
            {

                if (Frame.GetController<AuditlogViewController>().processnav(e)) return;
                //System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                //stopWatch.Start();
                //if (e.ActionArguments.SelectedChoiceActionItem.ParentItem.Id == "Sampling Management" || e.ActionArguments.SelectedChoiceActionItem.ParentItem.Id == "SampleManagement")
                //{
                //    Frame.GetController<FlutterAppViewController>().processsubmitteddata();
                //}

                objnavigationRefresh.SelectedNavigationItem = e.ActionArguments.SelectedChoiceActionItem.Id;
                if (e.ActionArguments.SelectedChoiceActionItem != null && e.ActionArguments.SelectedChoiceActionItem.Model is IModelNavigationItem)
                {
                    IModelNavigationItem navItem = (IModelNavigationItem)e.ActionArguments.SelectedChoiceActionItem.Model;
                    if (navItem != null && !string.IsNullOrEmpty(navItem.Id))
                    {
                        objnavigationRefresh.ClickedNavigationItem = navItem.Id;
                        Employee objEmployee = (Employee)SecuritySystem.CurrentUser;
                        if (objEmployee != null && objEmployee.Roles.Count > 0 && objEmployee.Roles.FirstOrDefault(i => i.IsAdministrative == true) == null)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            Modules.BusinessObjects.Setting.NavigationItem navigation = os.FindObject<Modules.BusinessObjects.Setting.NavigationItem>(CriteriaOperator.Parse("[NavigationId] = ?", navItem.Id));
                            if (navigation != null && objEmployee.RolePermissions.Count > 0)
                            {
                                bool IsDelete = false;
                                bool IsCreate = false;
                                bool IsWrite = false;

                                foreach (RoleNavigationPermission obj in objEmployee.RolePermissions)
                                {
                                    if (obj != null && obj.RoleNavigationPermissionDetails.Count > 0)
                                    {
                                        if (obj.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.NavigationId == navItem.Id && i.Create == true) != null)
                                        {
                                            IsCreate = true;
                                        }
                                        if (obj.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.NavigationId == navItem.Id && i.Write == true) != null)
                                        {
                                            IsWrite = true;
                                        }
                                        if (obj.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.NavigationId == navItem.Id && i.Delete == true) != null)
                                        {
                                            IsDelete = true;
                                        }

                                        foreach (RoleNavigationPermissionDetails permission in obj.RoleNavigationPermissionDetails.Where(i => i.NavigationItem != null && i.NavigationItem.NavigationId == navItem.Id))
                                        {
                                            if (!string.IsNullOrEmpty(permission.NavigationItem.NavigationModelClass))
                                            {
                                                AssignObjectPermission(objEmployee.Roles, IsCreate, IsWrite, IsDelete, permission.NavigationItem.NavigationModelClass, false);
                                            }
                                            if (permission.NavigationItem.LinkedClasses.Count > 0)
                                            {
                                                foreach (LinkedClasses objLinkedClass in permission.NavigationItem.LinkedClasses)
                                                {
                                                    if (!string.IsNullOrEmpty(objLinkedClass.ClassName))
                                                    {
                                                        AssignObjectPermission(objEmployee.Roles, IsCreate, IsWrite, IsDelete, objLinkedClass.ClassName, true);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                Frame.GetController<NewObjectViewController>().NewObjectAction.Active["New"] = IsCreate;
                                Frame.GetController<ListViewController>().EditAction.Active["Edit"] = IsWrite;
                                Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active["Delete"] = IsDelete;
                                objpermission.NavigationIsCreate = IsCreate;
                                objpermission.NavigationIsWrite = IsWrite;
                                objpermission.NavigationIsDelete = IsDelete;

                            }
                        }

                        if (objEmployee.FullName != null && (objEmployee.FullName.Trim() == "Admin" || objEmployee.FullName.Trim() == "行政"))//&& objEmployee.Roles.Count > 0 && objEmployee.Roles.FirstOrDefault(i => i.IsAdministrative == false) == null)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            Modules.BusinessObjects.Setting.NavigationItem navigation = os.FindObject<Modules.BusinessObjects.Setting.NavigationItem>(CriteriaOperator.Parse("[NavigationId] = ?", navItem.Id));
                            if (navigation != null)
                            {
                                bool IsDelete = true;
                                bool IsCreate = true;
                                bool IsWrite = true;

                                Frame.GetController<NewObjectViewController>().NewObjectAction.Active["New"] = IsCreate;
                                Frame.GetController<ListViewController>().EditAction.Active["Edit"] = IsWrite;
                                Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active["Delete"] = IsDelete;
                                objpermission.NavigationIsCreate = IsCreate;
                                objpermission.NavigationIsWrite = IsWrite;
                                objpermission.NavigationIsDelete = IsDelete;

                            }
                        }
                    }
                }

                if (e.ActionArguments.SelectedChoiceActionItem != null && e.ActionArguments.SelectedChoiceActionItem.ParentItem != null)
                {
                    Frame.GetController<AuditlogViewController>().clearaudit();
                    objNavInfo.SelectedNavigationCaption = e.ActionArguments.SelectedChoiceActionItem.Caption;
                    if (e.ActionArguments.SelectedChoiceActionItem.ParentItem.Id == "Operations" || e.ActionArguments.SelectedChoiceActionItem.ParentItem.Id == "Reporting" || e.ActionArguments.SelectedChoiceActionItem.ParentItem.Id == "Alert" || e.ActionArguments.SelectedChoiceActionItem.ParentItem.Id == "MyDesktop")
                    {
                        //Navigationrefresh();
                        WebWindow.CurrentRequestWindow.RegisterClientScript("xml", "sessionStorage.clear();");
                        if (e.ActionArguments.SelectedChoiceActionItem.ParentItem.Id == "Reporting")
                        {
                            if (e.ActionArguments.SelectedChoiceActionItem.Id == "BatchReporting")
                            {
                                IObjectSpace objspace = Application.CreateObjectSpace();
                                BatchReporting objToShow = objspace.CreateObject<BatchReporting>();
                                if (objToShow != null)
                                {
                                    //objRQPInfo.CurrentViewID = "ReportingQueryPanel_DetailView_Validation";
                                    DetailView CreateDetailView = Application.CreateDetailView(objspace, "BatchReporting_DetailView", false, objToShow);
                                    CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                                    Frame.SetView(CreateDetailView);
                                    e.Handled = true;
                                }
                            }
                            //if (e.ActionArguments.SelectedChoiceActionItem.Id == "ReportValidation")
                            //{
                            //    IObjectSpace objspace = Application.CreateObjectSpace();
                            //    ReportingQueryPanel objToShow = objspace.CreateObject<ReportingQueryPanel>();
                            //    if (objToShow != null)
                            //    {
                            //        //objRQPInfo.CurrentViewID = "ReportingQueryPanel_DetailView_Validation";
                            //        DetailView CreateDetailView = Application.CreateDetailView(objspace, "ReportingQueryPanel_DetailView_Validation", false, objToShow);
                            //        CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                            //        Frame.SetView(CreateDetailView);
                            //        e.Handled = true;
                            //    }
                            //}
                            //else 
                            //if (e.ActionArguments.SelectedChoiceActionItem.Id == "ReportApproval")
                            //{
                            //    IObjectSpace objspace = Application.CreateObjectSpace();
                            //    ReportingQueryPanel objToShow = objspace.CreateObject<ReportingQueryPanel>();
                            //    if (objToShow != null)
                            //    {
                            //        DetailView CreateDetailView = Application.CreateDetailView(objspace, "ReportingQueryPanel_DetailView", false, objToShow);
                            //        //objRQPInfo.CurrentViewID = CreateDetailView.Id;
                            //        CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                            //        Frame.SetView(CreateDetailView);
                            //        e.Handled = true;
                            //    }
                            //}
                            //else 
                            //if (e.ActionArguments.SelectedChoiceActionItem.Id == "Report View")
                            //{
                            //    IObjectSpace objspace = Application.CreateObjectSpace();
                            //    ReportingQueryPanel objToShow = objspace.CreateObject<ReportingQueryPanel>();
                            //    if (objToShow != null)
                            //    {
                            //        DetailView CreateDetailView = Application.CreateDetailView(objspace, "ReportingQueryPanel_DetailView_ResultView", false, objToShow);
                            //        //objRQPInfo.CurrentViewID = CreateDetailView.Id;
                            //        CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                            //        Frame.SetView(CreateDetailView);
                            //        e.Handled = true;
                            //    }
                            //}
                        }
                    }
                    else if (e.ActionArguments.SelectedChoiceActionItem.ParentItem.Id == "Data Review" || e.ActionArguments.SelectedChoiceActionItem.ParentItem.Id == "DataEntry" || e.ActionArguments.SelectedChoiceActionItem.ParentItem.Id == "Barcode Sample Custody" || e.ActionArguments.SelectedChoiceActionItem.ParentItem.Id == "TrendAnalysis")
                    {
                        if (e.ActionArguments.SelectedChoiceActionItem.Id == "RawDataResultReview" || e.ActionArguments.SelectedChoiceActionItem.Id == "RawDataResultVerify" || e.ActionArguments.SelectedChoiceActionItem.Id == "SDMS")
                        {
                            objnavigationRefresh.Refresh = "1";
                        }
                        else if (e.ActionArguments.SelectedChoiceActionItem.Id == "Result Entry")
                        {
                            objnavigationRefresh.ClickedNavigationItem = e.ActionArguments.SelectedChoiceActionItem.Id;
                            ResultEntryQueryPanelInfo resultEntryQueryPanelInfo = new ResultEntryQueryPanelInfo();
                            objQPInfo.IsQueryPanelOpened = true;
                            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ResultEntryQueryPanel));
                            ResultEntryQueryPanel newQuery = objectSpace.CreateObject<ResultEntryQueryPanel>();
                            //newQuery.FilterDataByMonth = FilterByMonthEN._1M;
                            //newQuery.SelectionMode = QueryMode.Job;
                            //objQPInfo.SelectMode = QueryMode.Job;
                            resultEntryQueryPanelInfo.ResultEntryCurrentobject = newQuery;
                            DetailView detailView = Application.CreateDetailView(objectSpace, "ResultEntryQueryPanel_DetailView_Copy", true, newQuery);
                            detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                            Frame.SetView(detailView);
                            e.Handled = true;
                        }
                        else if (e.ActionArguments.SelectedChoiceActionItem.Id == "ResultView")
                        {
                            objnavigationRefresh.ClickedNavigationItem = e.ActionArguments.SelectedChoiceActionItem.Id;
                            ResultEntryQueryPanelInfo resultEntryQueryPanelInfo = new ResultEntryQueryPanelInfo();
                            objQPInfo.IsQueryPanelOpened = true;
                            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ResultEntryQueryPanel));
                            ResultEntryQueryPanel newQuery = objectSpace.CreateObject<ResultEntryQueryPanel>();
                            //newQuery.FilterDataByMonth = FilterByMonthEN._1M;
                            //newQuery.SelectionMode = QueryMode.Job;
                            //objQPInfo.SelectMode = QueryMode.Job;
                            resultEntryQueryPanelInfo.ResultEntryCurrentobject = newQuery;
                            DetailView detailView = Application.CreateDetailView(objectSpace, "ResultViewQueryPanel_DetailView", false, newQuery);
                            detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                            Frame.SetView(detailView);
                            e.Handled = true;
                        }
                        else if (e.ActionArguments.SelectedChoiceActionItem.Id == "Sample In")
                        {
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            SampleCustodyTest objToShow = objspace.CreateObject<SampleCustodyTest>();
                            if (objToShow != null)
                            {
                                DetailView CreateDetailView = Application.CreateDetailView(objspace, "SampleCustodyTest_DetailView_SampleIn", true, objToShow);
                                //objRQPInfo.CurrentViewID = CreateDetailView.Id;
                                CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                                Frame.SetView(CreateDetailView);
                                e.Handled = true;
                            }
                        }
                        else if (e.ActionArguments.SelectedChoiceActionItem.Id == "Sample Out")
                        {
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            SampleCustodyTest objToShow = objspace.CreateObject<SampleCustodyTest>();
                            if (objToShow != null)
                            {
                                DetailView CreateDetailView = Application.CreateDetailView(objspace, "SampleCustodyTest_DetailView_SampleOut", true, objToShow);
                                //objRQPInfo.CurrentViewID = CreateDetailView.Id;
                                CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                                Frame.SetView(CreateDetailView);
                                e.Handled = true;
                            }
                        }
                        else if (e.ActionArguments.SelectedChoiceActionItem.Id == "Sample Disposal")
                        {
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            SampleCustodyTest objToShow = objspace.CreateObject<SampleCustodyTest>();
                            if (objToShow != null)
                            {
                                DetailView CreateDetailView = Application.CreateDetailView(objspace, "SampleCustodyTest_DetailView_SampleDisposal", true, objToShow);
                                //objRQPInfo.CurrentViewID = CreateDetailView.Id;
                                CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                                Frame.SetView(CreateDetailView);
                                e.Handled = true;
                            }
                        }
                        else if (e.ActionArguments.SelectedChoiceActionItem.Id == "Sample Location")
                        {
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            SampleCustodyTest objToShow = objspace.CreateObject<SampleCustodyTest>();
                            if (objToShow != null)
                            {
                                DetailView CreateDetailView = Application.CreateDetailView(objspace, "SampleCustodyTest_DetailView_SampleLocation", true, objToShow);
                                //objRQPInfo.CurrentViewID = CreateDetailView.Id;
                                CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                                Frame.SetView(CreateDetailView);
                                e.Handled = true;
                            }
                        }
                        else if (e.ActionArguments.SelectedChoiceActionItem.Id == "TrendAnalysis")
                        {
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            TrendAnalysis objToShow = objspace.CreateObject<TrendAnalysis>();
                            if (objToShow != null)
                            {
                                DetailView CreateDetailView = Application.CreateDetailView(objspace, "TrendAnalysis_DetailView", true, objToShow);
                                //objRQPInfo.CurrentViewID = CreateDetailView.Id;
                                CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                                Frame.SetView(CreateDetailView);
                                e.Handled = true;
                            }
                        }
                        //else if (e.ActionArguments.SelectedChoiceActionItem.Id == "Result Validation")
                        //{
                        //    objDRQPInfo.IsQueryPanelOpened = true;
                        //    IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(QCResultValidationQueryPanel));
                        //    QCResultValidationQueryPanel newQuery = objectSpace.CreateObject<QCResultValidationQueryPanel>();
                        //    newQuery.FilterDataByMonth = FilterByMonthEN._1M;
                        //    newQuery.SelectionMode = QueryMode.Job;
                        //    objDRQPInfo.SelectMode = QueryMode.Job;
                        //    DetailView detailView = Application.CreateDetailView(objectSpace, "ResultValidationQueryPanel_DetailView_ResultValidation", false, newQuery);
                        //    detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                        //    Frame.SetView(detailView);
                        //    e.Handled = true;
                        //}
                        //else if (e.ActionArguments.SelectedChoiceActionItem.Id == "Result Approval")
                        //{
                        //    objDRQPInfo.IsQueryPanelOpened = true;
                        //    IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(QCResultValidationQueryPanel));
                        //    QCResultValidationQueryPanel newQuery = objectSpace.CreateObject<QCResultValidationQueryPanel>();
                        //    newQuery.FilterDataByMonth = FilterByMonthEN._1M;
                        //    newQuery.SelectionMode = QueryMode.Job;
                        //    objDRQPInfo.SelectMode = QueryMode.Job;
                        //    DetailView detailView = Application.CreateDetailView(objectSpace, "ResultValidationQueryPanel_DetailView_ResultApproval", false, newQuery);
                        //    detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                        //    Frame.SetView(detailView);
                        //    e.Handled = true;
                        //}
                    }
                    else if (e.ActionArguments.SelectedChoiceActionItem.ParentItem.Id == "Settings")
                    {
                        if (e.ActionArguments.SelectedChoiceActionItem.Id == "ParameterDefault")
                        {
                            testInfo.CurrentTest = null;
                            testInfo.AllSelAvailableTestParam = null;
                            testInfo.CurrentQcType = null;
                            testInfo.ModifiedQCTypes = null;
                            testInfo.IsSaved = false;
                            testInfo.OpenSettings = false;
                            testInfo.NewTestParameters = null;
                            testInfo.RemovedTestParameters = null;
                            testInfo.ExistingTestParameters = null;
                            DashboardView dvDashboardView = Application.CreateDashboardView(Application.CreateObjectSpace(), "TestParameterSettings", false);
                            Frame.SetView(dvDashboardView);
                            e.Handled = true;
                        }
                    }

                }
                //stopWatch.Stop();
                //TimeSpan ts = stopWatch.Elapsed;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void AssignObjectPermission(XPCollection<PermissionPolicyRole> roles, bool isCreate, bool isWrite, bool isDelete, string navigationModelClass, bool isLinked)
        {
            try
            {
                if (isLinked)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    Modules.BusinessObjects.Setting.NavigationItem navigationItem = os.GetObjectByKey<Modules.BusinessObjects.Setting.NavigationItem>(new Guid(navigationModelClass));
                    navigationModelClass = navigationItem.NavigationModelClass;
                    os.Dispose();
                }
                if (XafTypesInfo.Instance.FindTypeInfo(navigationModelClass) != null)
                {
                    System.Type type = XafTypesInfo.Instance.FindTypeInfo(navigationModelClass).Type;
                    if (type != null)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        foreach (PermissionPolicyRole role in roles)
                        {
                            CustomSystemRole currentRole = os.GetObjectByKey<CustomSystemRole>(role.Oid);
                            if (currentRole != null)
                            {
                                IList<PermissionPolicyTypePermissionObject> lstPermissions = os.GetObjects<PermissionPolicyTypePermissionObject>(CriteriaOperator.Parse("[Role.Oid] = ?", role.Oid));
                                if (lstPermissions != null && lstPermissions.Count > 0)
                                {
                                    PermissionPolicyTypePermissionObject TypePermission = lstPermissions.FirstOrDefault<PermissionPolicyTypePermissionObject>(i => i.TargetType != null && i.TargetType.FullName == type.FullName);
                                    if (TypePermission != null)
                                    {
                                        TypePermission.ReadState = SecurityPermissionState.Allow;
                                        TypePermission.WriteState = isWrite ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                        //TypePermission.NavigateState = permission.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                        TypePermission.DeleteState = isDelete ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                        TypePermission.CreateState = isCreate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                    }
                                }
                            }
                        }
                        os.CommitChanges();
                        os.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ShowNavigationController_ItemsInitialized(object sender, EventArgs e)
        {
            //Navigationrefresh();
        }

        public void Navigationrefresh()
        {
            try
            {
                foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                {
                    if (parent.Id == "TaskManagement")
                    {
                        ChoiceActionItem childTaskRegistration = parent.Items.FirstOrDefault(i => i.Id == "TaskRegistration");
                        if (childTaskRegistration != null)
                        {
                            //int count = 0;
                            //IObjectSpace objSpace = Application.CreateObjectSpace();
                            //IList<Tasks> objTask = objSpace.GetObjects<Tasks>(CriteriaOperator.Parse("[Status] = 'PendingSubmit'"));
                            //var cap = childTaskRegistration.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //if (objTask != null && objTask.Count > 0)
                            //{
                            //    count = objTask.Count;
                            //    if (count > 0)
                            //    {
                            //        childTaskRegistration.Caption = cap[0] + " (" + count + ")";
                            //    }
                            //    else
                            //    {
                            //        childTaskRegistration.Caption = cap[0];
                            //    }
                            //}
                            //else
                            //{
                            //    childTaskRegistration.Caption = cap[0];
                            //}
                        }
                        foreach (ChoiceActionItem child in parent.Items)
                        {
                            if (child.Id == "RegistrationValidation ")
                            {
                                //int intOrderValue = 0;
                                //IObjectSpace objectSpace = Application.CreateObjectSpace();
                                //var count = objectSpace.GetObjectsCount(typeof(Tasks), CriteriaOperator.Parse("[Status] = 'PendingTaskRegistrationValidation'"));
                                //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                //if (count > 0)
                                //{
                                //    child.Caption = cap[0] + " (" + count + ")";
                                //}
                                //else
                                //{
                                //    child.Caption = cap[0];
                                //}
                            }
                        }
                        ChoiceActionItem childTaskRelease = parent.Items.FirstOrDefault(i => i.Id == "TaskRelease");
                        if (childTaskRelease != null)
                        {
                            //IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //var count = objectSpace.GetObjectsCount(typeof(Tasks), CriteriaOperator.Parse("[Status] = 'PendingRelease'"));
                            //var cap = childTaskRelease.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //if (count > 0)
                            //{
                            //    childTaskRelease.Caption = cap[0] + " (" + count + ")";
                            //}
                            //else
                            //{
                            //    childTaskRelease.Caption = cap[0];
                            //}
                        }
                        ChoiceActionItem childTaskAcceptance = parent.Items.FirstOrDefault(i => i.Id == "TaskAcceptance");
                        if (childTaskAcceptance != null)
                        {
                            //IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //var count = objectSpace.GetObjectsCount(typeof(Tasks), CriteriaOperator.Parse("[Status] = 'PendingAcceptance' And [Departments][].Count() > 0"));
                            //var cap = childTaskAcceptance.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //if (count > 0)
                            //{
                            //    childTaskAcceptance.Caption = cap[0] + " (" + count + ")";
                            //}
                            //else
                            //{
                            //    childTaskAcceptance.Caption = cap[0];
                            //}
                        }
                    }
                    else if (parent.Id == "TaskDeliveryService")
                    {
                        ChoiceActionItem childDeliveryTasks = parent.Items.FirstOrDefault(i => i.Id == "DeliveryTasks");
                        if (childDeliveryTasks != null)
                        {
                            //int count = 0;
                            //IObjectSpace objSpace = Application.CreateObjectSpace();
                            //IList<Tasks> objDeliveryTasks = objSpace.GetObjects<Tasks>(CriteriaOperator.Parse("[Status] = 'PendingDelivery' And[BottlesOrders][[Status] = 'PendingDelivery'].Count() > 0"));
                            //var cap = childDeliveryTasks.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //if (objDeliveryTasks != null && objDeliveryTasks.Count > 0)
                            //{
                            //    count = objDeliveryTasks.Count;
                            //    if (count > 0)
                            //    {
                            //        childDeliveryTasks.Caption = cap[0] + " (" + count + ")";
                            //    }
                            //    else
                            //    {
                            //        childDeliveryTasks.Caption = cap[0];
                            //    }
                            //}
                            //else
                            //{
                            //    childDeliveryTasks.Caption = cap[0];
                            //}
                        }
                    }
                    else if (parent.Id == "MyDesktop")
                    {
                        foreach (ChoiceActionItem child in parent.Items)
                        {
                            if (child.Id == "TaskTracking")
                            {
                                //IObjectSpace objectSpace = Application.CreateObjectSpace();
                                //int objsampCount = objectSpace.GetObjects<Samplecheckin>().ToList().Where(i => i.ProgressStatus < 100.0).Count();
                                //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                //if (objsampCount > 0)
                                //{
                                //    child.Caption = cap[0] + " (" + objsampCount + ")";
                                //}
                                //else
                                //{
                                //    child.Caption = cap[0];
                                //}
                            }
                            else if (child.Id == "Project Tracking")
                            {
                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                var count = objectSpace.GetObjectsCount(typeof(Samplecheckin), CriteriaOperator.Parse(""));
                                //int objsampCount = objectSpace.GetObjects<Samplecheckin>().ToList().Where(i => i.ProgressStatus < 100.0).Count();
                                var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                if (count > 0)
                                {
                                    child.Caption = cap[0] + " (" + count + ")";
                                }
                                else
                                {
                                    child.Caption = cap[0];
                                }
                            }
                        }
                    }
                    else if (parent.Id == "Reporting")
                    {
                        foreach (ChoiceActionItem child in parent.Items)
                        {
                            if (child.Id == "ReportDelivery")
                            {
                                ChoiceActionItem childReportDelivery = parent.Items.FirstOrDefault(i => i.Id == "ReportDelivery");
                                if (childReportDelivery != null)
                                {
                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                    var count = objectSpace.GetObjectsCount(typeof(Modules.BusinessObjects.SampleManagement.Reporting), CriteriaOperator.Parse("[ReportStatus] = ##Enum#Modules.BusinessObjects.Hr.ReportStatus,PendingDelivery#"));
                                    //var count = objectSpace.GetObjectsCount(typeof(Modules.BusinessObjects.SampleManagement.Reporting), CriteriaOperator.Parse("[ReportStatus]='PendingDelivery' Or [ReportStatus]='ReportDelivered'"));
                                    var cap = childReportDelivery.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    if (count > 0)
                                    {
                                        childReportDelivery.Caption = cap[0] + " (" + count + ")";
                                    }
                                    else
                                    {
                                        childReportDelivery.Caption = cap[0];
                                    }
                                }
                            }
                            if (child.Id == "Custom Reporting")
                            {
                                int count = 0;
                                IObjectSpace objSpace = Application.CreateObjectSpace();
                                using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                                {
                                    lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingReporting' And Not IsNullOrEmpty([Samplelogin.JobID.JobID])");
                                    lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                                    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                    List<object> jobid = new List<object>();
                                    if (lstview != null)
                                    {
                                        foreach (ViewRecord rec in lstview)
                                            jobid.Add(rec["Toid"]);
                                    }

                                    count = jobid.Count;
                                }
                                var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                if (count > 0)
                                {
                                    child.Caption = cap[0] + " (" + count + ")";
                                }
                                else
                                {
                                    child.Caption = cap[0];
                                }
                            }
                        }
                        ChoiceActionItem childlevel2ReportReview = parent.Items.FirstOrDefault(i => i.Id == "ReportValidation");
                        if (childlevel2ReportReview != null)
                        {
                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                            var count = objectSpace.GetObjectsCount(typeof(Modules.BusinessObjects.SampleManagement.Reporting), CriteriaOperator.Parse("[ReportStatus] <> ##Enum#Modules.BusinessObjects.Hr.ReportStatus,Rollbacked# AND [ReportValidatedDate] IS NULL AND [ReportValidatedBy] IS NULL"));
                            var cap = childlevel2ReportReview.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            if (count > 0)
                            {
                                childlevel2ReportReview.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                childlevel2ReportReview.Caption = cap[0];
                            }
                        }
                    }
                    else if (parent.Id == "Data Review")
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        Session currentSession = ((XPObjectSpace)(os)).Session;
                        SelectedData sproc = currentSession.ExecuteSproc("Spreadsheetentry_SelectResultBatchReview_sp", new OperandValue(Convert.ToDateTime("1/1/1753 12:00:00")), new OperandValue(DateTime.Now.Date));
                        var Reviewcount = 0;
                        var Verifycount = 0;
                        foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                        {
                            if (row.Values[13] != null)
                            {
                                if (row.Values[13].ToString() == "Pending Review")
                                {
                                    Reviewcount += 1;
                                }
                                else if (row.Values[13].ToString() == "Pending Verify")
                                {
                                    Verifycount += 1;
                                }
                            }
                        }
                        foreach (ChoiceActionItem child in parent.Items)
                        {
                            if (child.Id == "RawDataResultReview")
                            {
                                var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                if (Reviewcount > 0)
                                {
                                    child.Caption = cap[0] + " (" + Reviewcount + ")";
                                }
                                else
                                {
                                    child.Caption = cap[0];
                                }
                            }
                            else if (child.Id == "RawDataResultVerify")
                            {
                                var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                if (Verifycount > 0)
                                {
                                    child.Caption = cap[0] + " (" + Verifycount + ")";
                                }
                                else
                                {
                                    child.Caption = cap[0];
                                }
                            }
                            else if (child.Id == "Result Validation")
                            {
                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                ////var count = objectSpace.GetObjectsCount(typeof(SpreadSheetEntry), CriteriaOperator.Parse("[Status]='PendingValidation' AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]=1 or [UQTESTPARAMETERID.InternalStandard]=1)"));
                                //var count = objectSpace.GetObjectsCount(typeof(SampleParameter), CriteriaOperator.Parse("[Status] = 'PendingValidation' And[SignOff] = True And([QCBatchID] Is Null Or[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] = 'Sample')"));
                                //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                //if (count > 0)
                                //{
                                //    child.Caption = cap[0] + " (" + count + ")";
                                //}
                                //else
                                //{
                                //    child.Caption = cap[0];
                                //}
                                int count = 0;
                                //IObjectSpace objSpace = Application.CreateObjectSpace();
                                //using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                                //{
                                //    //lstview.Criteria = CriteriaOperator.Parse("([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample') AND ([SubOut] is null Or [SubOut]=False Or ([SubOut] = True And [IsExportedSuboutResult] = True)) And [GCRecord] IS NULL");
                                //    lstview.Criteria = CriteriaOperator.Parse("([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null) AND ([SubOut] is null Or [SubOut]=False Or ([SubOut] = True And [IsExportedSuboutResult] = True)) And [GCRecord] IS NULL");
                                //    lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                                //    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                //    List<object> jobid = new List<object>();
                                //    if (lstview != null)
                                //    {
                                //        foreach (ViewRecord rec in lstview)
                                //            jobid.Add(rec["Toid"]);
                                //    }

                                //    count = jobid.Count;
                                //}

                                IList<SampleParameter> lstSamples = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status]='PendingValidation' And [GCRecord] IS NULL AND ([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.GCRecord] IS NULL And [QCBatchID.QCType] Is Not Null) AND ([SubOut] is null Or [SubOut]=False Or ([SubOut] = True And [IsExportedSuboutResult] = True))"));
                                if (lstSamples != null && lstSamples.Count > 0)
                                {
                                    //string strJobID = string.Join(";", lstSamples.Where(i => !string.IsNullOrEmpty(i.JobID)).Select(i => i.JobID.Trim()));
                                    //string[] arrJobID = strJobID.Split(';');
                                    //var a = arrJobID.Distinct();
                                    //count = arrJobID.Distinct().Count();
                                }

                                //IList<Samplecheckin> lstJobID = objectSpace.GetObjects<Samplecheckin>();
                                //if (lstJobID != null && lstJobID.Count > 0)
                                //{
                                //    count = lstJobID.Where(i => i.PendingValidationSamplesCount > 0).Count();
                                //}

                                var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                if (count > 0)
                                {
                                    child.Caption = cap[0] + " (" + count + ")";
                                }
                                else
                                {
                                    child.Caption = cap[0];
                                }
                            }
                            else if (child.Id == "Result Approval")
                            {
                                //IObjectSpace objectSpace = Application.CreateObjectSpace();
                                //var count = objectSpace.GetObjectsCount(typeof(SpreadSheetEntry), CriteriaOperator.Parse("[Status]='PendingApproval' AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]='1' or [UQTESTPARAMETERID.InternalStandard]='1')"));
                                //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                //if (count > 0)
                                //{
                                //    child.Caption = cap[0] + " (" + count + ")";
                                //}
                                //else
                                //{
                                //    child.Caption = cap[0];
                                //}
                                int count = 0;
                                IObjectSpace objSpace = Application.CreateObjectSpace();
                                //using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                                //{
                                //    //lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingApproval' And[SignOff] = True And([QCBatchID] Is Null Or[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] = 'Sample')");
                                //    lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingApproval' And[SignOff] = True And([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null)");
                                //    lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                                //    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                //    List<object> jobid = new List<object>();
                                //    if (lstview != null)
                                //    {
                                //        foreach (ViewRecord rec in lstview)
                                //            jobid.Add(rec["Toid"]);
                                //    }

                                //    count = jobid.Count;
                                //}
                                IList<SampleParameter> lstSamples = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status]='PendingApproval' AND ([QCBatchID] Is Null Or [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null) AND ([SubOut] is null Or [SubOut]=False Or ([SubOut] = True And [IsExportedSuboutResult] = True)) And [GCRecord] IS NULL"));
                                if (lstSamples != null && lstSamples.Count > 0)
                                {
                                    //string strJobID = string.Join(";", lstSamples.Where(i => !string.IsNullOrEmpty(i.JobID)).Select(i => i.JobID.Trim()));
                                    //string[] arrJobID = strJobID.Split(';');
                                    //count = arrJobID.Distinct().Count();
                                }
                                var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                if (count > 0)
                                {
                                    child.Caption = cap[0] + " (" + count + ")";
                                }
                                else
                                {
                                    child.Caption = cap[0];
                                }
                            }
                            else if (child.Id == "RawDataLevel2BatchReview ")
                            {

                                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                                var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                                {
                                    var count = os.GetObjectsCount(typeof(SpreadSheetEntry_AnalyticalBatch), CriteriaOperator.Parse("[Status] = 2"));
                                    cap = child.Caption.Split(new string[] { "(" }, StringSplitOptions.None);
                                    if (count > 0)
                                    {
                                        child.Caption = cap[0] + "(" + count + ")";
                                        //break;
                                    }
                                    else
                                    {
                                        child.Caption = cap[0];
                                        //break;
                                    }
                                }
                                else
                                {
                                    IList<AnalysisDepartmentChain> lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] =True", currentUser.Oid));
                                    List<Guid> lstTestMethodOid = new List<Guid>();
                                    if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                                    {
                                        foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                        {
                                            foreach (TestMethod testMethod in departmentChain.TestMethods)
                                            {
                                                if (!lstTestMethodOid.Contains(testMethod.Oid))
                                                {
                                                    lstTestMethodOid.Add(testMethod.Oid);
                                                }
                                            }
                                        }
                                    }
                                    IList<SpreadSheetEntry_AnalyticalBatch> objAB = os.GetObjects<SpreadSheetEntry_AnalyticalBatch>(new InOperator("Test.Oid", lstTestMethodOid));
                                    if (objAB.Count > 0)
                                    {
                                        //int count = objAB.Where(i => i.Status == 2).Select(i => i.Oid).Count();
                                        //if (count > 0)
                                        //{
                                        //    child.Caption = cap[0] + "(" + count + ")";
                                        //    //break;
                                        //}
                                        //else
                                        //{
                                        //    child.Caption = cap[0];
                                        //    //break;
                                        //}
                                    }
                                    else
                                    {
                                        child.Caption = cap[0];
                                    }

                                }
                                //IList<SpreadSheetEntry_AnalyticalBatch> objAnBatch = os.GetObjects<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[Status] = 2"));
                            }
                        }
                    }
                    else if (parent.Id == "ICM" || parent.Id == "InventoryManagement")
                    {
                        foreach (ChoiceActionItem child in parent.Items)
                        {
                            if (child.Id == "Operations")
                            {
                                foreach (ChoiceActionItem subchild in child.Items)
                                {
                                    if (subchild.Id == "Review")
                                    {
                                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                                        var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                        if (count > 0)
                                        {
                                            subchild.Caption = cap[0] + " (" + count + ")";
                                        }
                                        else
                                        {
                                            subchild.Caption = cap[0];
                                        }
                                    }
                                    //else if (subchild.Id == "RequisitionApproval")
                                    //{
                                    //    FuncCurrentUserIsAdministrative obj = new FuncCurrentUserIsAdministrative();
                                    //    object val = obj.Evaluate();
                                    //    if ((string)val == "0")
                                    //    {
                                    //        object CurrentUser = SecuritySystem.CurrentUserId;
                                    //        IObjectSpace objectspace = Application.CreateObjectSpace(typeof(WorkflowConfig));
                                    //        CriteriaOperator criteria = CriteriaOperator.Parse("GCRecord IS Null");
                                    //        IList<WorkflowConfig> ICMWorkFlow = objectspace.GetObjects<WorkflowConfig>(criteria);
                                    //        foreach (WorkflowConfig item in ICMWorkFlow)
                                    //        {
                                    //            if (item.Level == 1)
                                    //            {
                                    //                foreach (CustomSystemUser userid in item.User)
                                    //                {

                                    //                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    //                    {
                                    //                        if (Filter.ApproveFilter == string.Empty)
                                    //                        {
                                    //                            Filter.ApproveFilter = "Status='" + ICM.Module.BusinessObjects.TaskStatus.Level1Pending + "'";
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level1Pending + "'";
                                    //                        }
                                    //                    }
                                    //                }
                                    //            }
                                    //            if (item.Level == 2)
                                    //            {
                                    //                foreach (CustomSystemUser userid in item.User)
                                    //                {
                                    //                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    //                    {
                                    //                        if (Filter.ApproveFilter == string.Empty)
                                    //                        {
                                    //                            Filter.ApproveFilter = "Status='" + TaskStatus.Level2Pending + "'";
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level2Pending + "'";
                                    //                        }
                                    //                    }
                                    //                }
                                    //            }
                                    //            if (item.Level == 3)
                                    //            {
                                    //                foreach (CustomSystemUser userid in item.User)
                                    //                {
                                    //                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    //                    {
                                    //                        if (Filter.ApproveFilter == string.Empty)
                                    //                        {
                                    //                            Filter.ApproveFilter = "Status='" + TaskStatus.Level3Pending + "'";
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level3Pending + "'";
                                    //                        }
                                    //                    }
                                    //                }
                                    //            }
                                    //            if (item.Level == 4)
                                    //            {
                                    //                foreach (CustomSystemUser userid in item.User)
                                    //                {
                                    //                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    //                    {
                                    //                        if (Filter.ApproveFilter == string.Empty)
                                    //                        {
                                    //                            Filter.ApproveFilter = "Status='" + TaskStatus.Level4Pending + "'";
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level2Pending + "'";
                                    //                        }
                                    //                    }
                                    //                }
                                    //            }
                                    //            if (item.Level == 5)
                                    //            {
                                    //                foreach (CustomSystemUser userid in item.User)
                                    //                {
                                    //                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    //                    {
                                    //                        if (Filter.ApproveFilter == string.Empty)
                                    //                        {
                                    //                            Filter.ApproveFilter = "Status=" + TaskStatus.Level5Pending + "'";
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level5Pending + "'";
                                    //                        }
                                    //                    }
                                    //                }
                                    //            }
                                    //            if (item.Level == 6)
                                    //            {
                                    //                foreach (CustomSystemUser userid in item.User)
                                    //                {
                                    //                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    //                    {
                                    //                        if (Filter.ApproveFilter == string.Empty)
                                    //                        {
                                    //                            Filter.ApproveFilter = "Status='" + TaskStatus.Level6Pending + "'";
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level6Pending + "'";
                                    //                        }
                                    //                    }
                                    //                }
                                    //            }

                                    //        }
                                    //    }
                                    //    else if ((string)val == "1")
                                    //    {
                                    //        string AdminFilter = "Status='" + TaskStatus.Level1Pending + "'|| Status='" + TaskStatus.Level2Pending + "'|| Status='" + TaskStatus.Level3Pending + "'||Status='" + TaskStatus.Level4Pending + "'||Status='" + TaskStatus.Level5Pending + "'|| Status='" + TaskStatus.Level6Pending + "'";
                                    //        //IObjectSpace objectSpace = Application.CreateObjectSpace();
                                    //        //var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse(AdminFilter));


                                    //        //var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //        //if (count > 0)
                                    //        //{
                                    //        //    subchild.Caption = cap[0] + " (" + count + ")";
                                    //        //}
                                    //        //else
                                    //        //{
                                    //        //    subchild.Caption = cap[0];
                                    //        //}
                                    //        int intValue = 0;
                                    //        IObjectSpace objectSpace = Application.CreateObjectSpace();
                                    //        CriteriaOperator criteria = CriteriaOperator.Parse(AdminFilter);
                                    //        IList<Requisition> req = objectSpace.GetObjects<Requisition>(criteria);
                                    //        string[] batch = new string[req.Count];
                                    //        foreach (Requisition item in req)
                                    //        {
                                    //            if (!batch.Contains(item.BatchID))
                                    //            {
                                    //                batch[intValue] = item.BatchID;
                                    //                intValue = intValue + 1;
                                    //            }
                                    //        }
                                    //        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //        if (intValue > 0)
                                    //        {
                                    //            subchild.Caption = cap[0] + " (" + intValue + ")";
                                    //        }
                                    //        else
                                    //        {
                                    //            subchild.Caption = cap[0];
                                    //        }
                                    //    }
                                    //    if (Filter.ApproveFilter != string.Empty)
                                    //    {
                                    //        int intValue = 0;
                                    //        IObjectSpace objectSpace = Application.CreateObjectSpace();
                                    //        CriteriaOperator criteria = CriteriaOperator.Parse(Filter.ApproveFilter);
                                    //        IList<Requisition> req = objectSpace.GetObjects<Requisition>(criteria);
                                    //        string[] batch = new string[req.Count];
                                    //        foreach (Requisition item in req)
                                    //        {
                                    //            if (!batch.Contains(item.BatchID))
                                    //            {
                                    //                batch[intValue] = item.BatchID;
                                    //                intValue = intValue + 1;
                                    //            }
                                    //        }
                                    //        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //        if (intValue > 0)
                                    //        {
                                    //            subchild.Caption = cap[0] + " (" + intValue + ")";
                                    //        }
                                    //        else
                                    //        {
                                    //            subchild.Caption = cap[0];
                                    //        }
                                    //    }
                                    //}
                                    else if (subchild.Id == "OrderingItems")
                                    {
                                        int intOrderValue = 0;
                                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                                        var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingOrdering'"));
                                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                        CriteriaOperator criteria = CriteriaOperator.Parse("[Status] = 'PendingOrdering'");
                                        IList<Requisition> req = objectSpace.GetObjects<Requisition>(criteria);
                                        string[] strVendor = new string[req.Count];
                                        foreach (Requisition item in req)
                                        {
                                            if (!strVendor.Contains(item.Vendor.Vendor))
                                            {
                                                strVendor[intOrderValue] = item.Vendor.Vendor;
                                                intOrderValue = intOrderValue + 1;
                                            }
                                        }


                                        if (intOrderValue > 0)
                                        {
                                            subchild.Caption = cap[0] + " (" + intOrderValue + ")";
                                        }
                                        else
                                        {
                                            subchild.Caption = cap[0];
                                        }
                                    }
                                    //else if (subchild.Id == "OrderingApproval")
                                    //{
                                    //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                    //    var count = objectSpace.GetObjectsCount(typeof(Purchaseorder), CriteriaOperator.Parse("IsNullOrEmpty([ApprovedBy]) And [ApprovedDate] Is Null"));
                                    //    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //    if (count > 0)
                                    //    {
                                    //        subchild.Caption = cap[0] + " (" + count + ")";
                                    //    }
                                    //    else
                                    //    {
                                    //        subchild.Caption = cap[0];
                                    //    }
                                    //}
                                    else if (subchild.Id == "Receive Order")
                                    {

                                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                                        CollectionSource cs = new CollectionSource(objectSpace, typeof(Requisition));
                                        cs.Criteria["FilterPOID"] = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
                                        List<string> listpoid = new List<string>();
                                        foreach (Requisition reqobjvendor in cs.List)
                                        {
                                            if (reqobjvendor.POID != null)
                                            {
                                                if (!listpoid.Contains(reqobjvendor.POID.POID))
                                                {
                                                    listpoid.Add(reqobjvendor.POID.POID);
                                                }
                                            }
                                        }
                                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                        if (listpoid.Count > 0)
                                        {
                                            subchild.Caption = cap[0] + " (" + listpoid.Count + ")";
                                        }
                                        else
                                        {
                                            subchild.Caption = cap[0];
                                        }

                                        //var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'"));

                                    }
                                    else if (subchild.Id == "DistributionItem")
                                    {
                                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                                        var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[Status] = 'PendingDistribute'"));
                                        //var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[LT] Is Null"));
                                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                        int intReceive = 0;
                                        //CriteriaOperator criteria = CriteriaOperator.Parse("[LT] Is Null");
                                        //IList<Distribution> req = objectSpace.GetObjects<Distribution>(criteria);
                                        //string[] ReceiveID = new string[req.Count];

                                        //foreach (Distribution item in req)
                                        //{
                                        //    if (!ReceiveID.Contains(item.ReceiveID))
                                        //    {
                                        //        ReceiveID[intReceive] = item.ReceiveID;
                                        //        intReceive = intReceive + 1;

                                        //    }
                                        //}

                                        using (XPView lstview = new XPView(((XPObjectSpace)objectSpace).Session, typeof(Distribution)))
                                        {
                                            lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingDistribute'");
                                            lstview.Properties.Add(new ViewProperty("TReceiveID", SortDirection.Ascending, "ReceiveID", true, true));
                                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                            if (lstview != null && lstview.Count > 0)
                                            {
                                                intReceive = lstview.Count;
                                            }
                                        }

                                        if (count > 0)
                                        {
                                            subchild.Caption = cap[0] + " (" + intReceive + ")";
                                        }
                                        else
                                        {
                                            subchild.Caption = cap[0];
                                        }
                                    }
                                    //else if (subchild.Id == "ConsumptionItems")
                                    //{
                                    //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                    //    var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[Status] = 'PendingConsume' And ([ExpiryDate] Is Null or [ExpiryDate] > ?)", DateTime.Now));
                                    //    //var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[ConsumptionBy] Is Null And [ConsumptionDate] Is Null And ([ExpiryDate] Is Null or [ExpiryDate] > ?)", DateTime.Now));
                                    //    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //    if (count > 0)
                                    //    {
                                    //        subchild.Caption = cap[0] + " (" + count + ")";
                                    //    }
                                    //    else
                                    //    {
                                    //        subchild.Caption = cap[0];
                                    //    }
                                    //}
                                    else if (subchild.Id == "DisposalItems")
                                    {
                                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("([Status] == 'PendingDispose' OR [Status] == 'PendingConsume') And [ExpiryDate] <= ?", DateTime.Now);
                                        var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] = 'PendingDispose' OR [Status] = 'PendingConsume') And [ExpiryDate] < ?", DateTime.Today));
                                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                        if (count > 0)
                                        {
                                            subchild.Caption = cap[0] + " (" + count + ")";
                                        }
                                        else
                                        {
                                            subchild.Caption = cap[0];
                                        }
                                    }
                                    else if (subchild.Id == "VendorsReagentCertificate")
                                    {
                                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                                        var count = objectSpace.GetObjectsCount(typeof(VendorReagentCertificate), CriteriaOperator.Parse("[LoadedDate] IS NULL AND [LoadedBy] IS NULL"));
                                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                        if (count > 0)
                                        {
                                            subchild.Caption = cap[0] + " (" + count + ")";
                                        }
                                        else
                                        {
                                            subchild.Caption = cap[0];
                                        }
                                    }

                                }
                            }
                            else if (child.Id == "Alert")
                            {
                                foreach (ChoiceActionItem subchild in child.Items)
                                {
                                    //if (subchild.Id == "Stock Watch")
                                    //{
                                    //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                    //    var count = objectSpace.GetObjectsCount(typeof(Items), CriteriaOperator.Parse("[StockQty] > 0"));
                                    //    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //    if (count > 0)
                                    //    {
                                    //        subchild.Caption = cap[0] + " (" + count + ")";
                                    //    }
                                    //    else
                                    //    {
                                    //        subchild.Caption = cap[0];
                                    //    }
                                    //}
                                    if (subchild.Id == "StockAlert" || subchild.Id == "Stock Alert")
                                    {
                                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                                        var count = objectSpace.GetObjectsCount(typeof(Items), CriteriaOperator.Parse("[StockQty] <= [AlertQty]"));
                                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                        if (count > 0)
                                        {
                                            subchild.Caption = cap[0] + " (" + count + ")";
                                        }
                                        else
                                        {
                                            subchild.Caption = cap[0];
                                        }
                                    }
                                    else if (subchild.Id == "Expiration" || subchild.Id == "Expiration Alert")
                                    {
                                        DateTime TodayDate = DateTime.Now;
                                        TodayDate = TodayDate.AddDays(7);
                                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                                        var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] == 'PendingDispose' OR [Status] == 'PendingConsume') And [ExpiryDate] <= ?", TodayDate));
                                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                        if (count > 0)
                                        {
                                            subchild.Caption = cap[0] + " (" + count + ")";
                                        }
                                        else
                                        {
                                            subchild.Caption = cap[0];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (parent.Id == "SampleSubOutTracking")
                    {
                        foreach (ChoiceActionItem child in parent.Items)
                        {
                            //if (child.Id == "SuboutSampleRegistration")
                            //{
                            //    //int intOrderValue = 0;
                            //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //    IList<SampleParameter> objParam = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SubOut] = True And [SuboutSample] Is Null And [Samplelogin] Is Not Null And [Testparameter] Is Not Null And [Testparameter.Parameter] Is Not Null And Not IsNullOrEmpty([Testparameter.Parameter.ParameterName])"));
                            //    if (objParam != null && objParam.Count > 0)
                            //    {
                            //        //var count = objParam.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID != null).Select(i => i.Samplelogin.JobID.JobID).Distinct().Count();
                            //        var count = objParam.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).GroupBy(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                            //        if (count > 0)
                            //        {
                            //            child.Caption = cap[0] + " (" + count + ")";
                            //        }
                            //        else
                            //        {
                            //            child.Caption = cap[0];
                            //        }
                            //    }
                            //    else
                            //    {
                            //        child.Caption = cap[0];
                            //    }

                            //}
                            ////else if (child.Id == "SuboutSampleResultEntry")
                            ////{
                            ////    IObjectSpace objectSpace = Application.CreateObjectSpace();
                            ////    IList<SampleParameter> objParam = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SubOut] = True And [IsExportedSuboutResult] = False  And [SuboutSignOff] = True And [Status] = 'PendingEntry'"));
                            ////    if (objParam != null && objParam.Count > 0)
                            ////    {
                            ////        //var count = objParam.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID != null).Select(i => i.Samplelogin.JobID.JobID).Distinct().Count();
                            ////        var count = objParam.Where(i => i.SuboutSample != null && i.SuboutSample.SuboutOrderID != null).Select(i => i.SuboutSample.SuboutOrderID).Distinct().Count();
                            ////        var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            ////        if (count > 0)
                            ////        {
                            ////            child.Caption = cap[0] + " (" + count + ")";
                            ////        }
                            ////        else
                            ////        {
                            ////            child.Caption = cap[0];
                            ////        }
                            ////    }
                            ////}
                            ////else if (child.Id == "SuboutNotificationQueue")
                            ////{
                            ////    IObjectSpace objectSpace = Application.CreateObjectSpace();

                            ////    var count = objectSpace.GetObjectsCount(typeof(SubOutSampleRegistrations), CriteriaOperator.Parse("[SuboutNotificationStatus] = 'Waiting'"));
                            ////    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            ////    if (count > 0)
                            ////    {
                            ////        child.Caption = cap[0] + " (" + count + ")";
                            ////    }
                            ////    else
                            ////    {
                            ////        child.Caption = cap[0];
                            ////    }
                            ////}
                            ////else if (child.Id == "SuboutRegistrationSigningOff")
                            ////{
                            ////    IObjectSpace objectSpace = Application.CreateObjectSpace();
                            ////    var count = objectSpace.GetObjectsCount(typeof(SubOutSampleRegistrations), CriteriaOperator.Parse("[Status] = 'PendingSigningOff'"));
                            ////    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            ////    if (count > 0)
                            ////    {
                            ////        child.Caption = cap[0] + " (" + count + ")";
                            ////    }
                            ////    else
                            ////    {
                            ////        child.Caption = cap[0];
                            ////    }
                            ////}
                        }
                    }
                    else if (parent.Id == "SamplePreparationRootNode")
                    {
                        ChoiceActionItem child = parent.Items.FirstOrDefault(i => i.Id == "SamplePreparation");
                        if (child != null)
                        {
                            //IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //int objperpCount = objectSpace.GetObjects<TestMethod>().ToList().Where(i => i.NoOfPrepSamples > 0).Count();
                            //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //if (objperpCount > 0)
                            //{
                            //    child.Caption = cap[0] + " (" + objperpCount + ")";
                            //}
                            //else
                            //{
                            //    child.Caption = cap[0];
                            //}
                            //int count = 0;
                            //IObjectSpace objSpace = Application.CreateObjectSpace();
                            //using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                            //{
                            //    lstview.Criteria = CriteriaOperator.Parse("[Testparameter.TestMethod.PrepMethods][].Count() > 0 And [Samplelogin] Is Not Null And [SamplePrepBatchID] Is Null And [Status] = 'PendingEntry'");
                            //    lstview.Properties.Add(new ViewProperty("TestOid", SortDirection.Ascending, "Testparameter.TestMethod.Oid", true, true));
                            //    List<object> jobid = new List<object>();
                            //    if (lstview != null)
                            //    {
                            //        foreach (ViewRecord rec in lstview)
                            //            jobid.Add(rec["TestOid"]);
                            //    }

                            //    count = jobid.Count;
                            //}
                            //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //if (count > 0)
                            //{
                            //    child.Caption = cap[0] + " (" + count + ")";
                            //}
                            //else
                            //{
                            //    child.Caption = cap[0];
                            //}
                        }
                    }
                    else if (parent.Id == "SampleManagement")
                    {
                        ChoiceActionItem childRegistration = parent.Items.FirstOrDefault(i => i.Id == "SampleRegistration");
                        if (childRegistration != null)
                        {
                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                            var count = objectSpace.GetObjectsCount(typeof(Samplecheckin), CriteriaOperator.Parse("[Status] =  'PendingSubmit'"));
                            var cap = childRegistration.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            if (count > 0)
                            {
                                childRegistration.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                childRegistration.Caption = cap[0];
                            }
                        }
                        ChoiceActionItem childRegSignOff = parent.Items.FirstOrDefault(i => i.Id == "RegistrationSigningOff");
                        if (childRegSignOff != null)
                        {
                            int count = 0;
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            //IList<Samplecheckin> lstPendingRegistrationSignOff = objSpace.GetObjects<Samplecheckin>(CriteriaOperator.Parse("[Status] = 'PendingSigningOff' Or [Status] = 'PartiallySignedOff'"));

                            using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(Modules.BusinessObjects.SampleManagement.SampleParameter)))
                            {
                                lstview.Criteria = CriteriaOperator.Parse("([Samplelogin.JobID.Status] = 'PendingSigningOff' Or [Samplelogin.JobID.Status] = 'PartiallySignedOff') And Not IsNullOrEmpty([Samplelogin.JobID.JobID]) And ([SubOut] Is Null Or [SubOut] = False) and [Samplelogin.GCRecord] is NULL and [Samplelogin.JobID.GCRecord] is NULL");
                                lstview.Properties.Add(new ViewProperty("JobID", DevExpress.Xpo.SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                                lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                                List<object> jobid = new List<object>();
                                if (lstview != null)
                                {
                                    //foreach (ViewRecord rec in lstview)
                                    //    jobid.Add(rec["Toid"]);
                                    foreach (ViewRecord rec in lstview)
                                    {
                                        SampleParameter objsample = objSpace.FindObject<SampleParameter>(CriteriaOperator.Parse("[Oid]= ?", new Guid(rec["Toid"].ToString())));
                                        if (objsample != null && objsample.Samplelogin.Oid != null)
                                        {
                                            IList<SampleBottleAllocation> ObjbottleAllocation = objSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.JobID.Oid] = ? And [SignOffDate] Is Null And [SignOffBy] Is Null", objsample.Samplelogin.JobID.Oid));
                                            foreach (SampleBottleAllocation objSamplebottle in ObjbottleAllocation)
                                            {
                                                if (objSamplebottle != null)
                                                {
                                                    if (!jobid.Contains(rec["Toid"]))
                                                    {
                                                        jobid.Add(rec["Toid"]);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                count = jobid.Count;
                            }
                            var cap = childRegSignOff.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            if (count > 0)
                            {
                                childRegSignOff.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                childRegSignOff.Caption = cap[0];
                            }
                        }

                        ChoiceActionItem childSampleReceipt = parent.Items.FirstOrDefault(i => i.Id == "SampleReceiptNotification");
                        if (childSampleReceipt != null)
                        {
                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //var count = objectSpace.GetObjectsCount(typeof(Samplecheckin), CriteriaOperator.Parse("[Status] = 'Signedoff'"));
                            var count = objectSpace.GetObjectsCount(typeof(Samplecheckin), CriteriaOperator.Parse("[Status] = 'Signedoff' And [MailStatus] = 'InQueue' Or [MailStatus] = 'Failed'"));
                            var cap = childSampleReceipt.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            if (count > 0)
                            {
                                childSampleReceipt.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                childSampleReceipt.Caption = cap[0];
                            }
                        }
                        ChoiceActionItem childPendingDisposal = parent.Items.FirstOrDefault(i => i.Id == "SampleDisposition");
                        if (childPendingDisposal != null)
                        {
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            Session currentsession = ((XPObjectSpace)objSpace).Session;
                            int count = Convert.ToInt32(currentsession.ExecuteScalar("GetPendingDisposalSamplesCount"));
                            var cap = childPendingDisposal.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            if (count > 0)
                            {
                                childPendingDisposal.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                childPendingDisposal.Caption = cap[0];
                            }
                        }
                    }
                    else if (parent.Id == "DataEntry")
                    {
                        ChoiceActionItem dataentryNode = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "DataEntry");
                        if (dataentryNode != null)
                        {
                            ChoiceActionItem child = dataentryNode.Items.FirstOrDefault(i => i.Id == "AnalysisQueue" || i.Id == "AnalysisQueue ");
                            if (child != null)
                            {
                                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                                var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                if (currentUser != null && currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                                {
                                    int count = 0;
                                    IObjectSpace objSpace = Application.CreateObjectSpace();
                                    IList<SampleParameter> lstTests = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SignOff] = True  And [IsTransferred] = true And ([SubOut] Is Null Or [SubOut] = False) And [UQABID] Is Null And [QCBatchID] Is Null And (([Testparameter.TestMethod.PrepMethods][].Count() > 0 And [SamplePrepBatchID] Is Not Null) Or [Testparameter.TestMethod.PrepMethods][].Count() = 0)"));
                                    if (lstTests != null && lstTests.Count > 0)
                                    {
                                        //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                                        count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                                    }
                                    //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    if (count > 0)
                                    {
                                        child.Caption = cap[0] + " (" + count + ")";
                                    }
                                    else
                                    {
                                        child.Caption = cap[0];
                                    }
                                }
                                else
                                {
                                    IObjectSpace objSpace = Application.CreateObjectSpace();
                                    IList<AnalysisDepartmentChain> lstAnalysisDepartChain = objSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] =True", currentUser.Oid));

                                    List<Guid> lstTestMethodOid = new List<Guid>();
                                    IList<SampleParameter> lstTests = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SignOff] = True And [IsTransferred] = true And ([SubOut] Is Null Or [SubOut] = False) And [UQABID] Is Null And [QCBatchID] Is Null And (([Testparameter.TestMethod.PrepMethods][].Count() > 0 And [SamplePrepBatchID] Is Not Null) Or [Testparameter.TestMethod.PrepMethods][].Count() = 0)"));
                                    if (lstTests != null && lstTests.Count > 0)
                                    {
                                        //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                                        IList<Guid> lstselTests = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).ToList();
                                        if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                                        {
                                            foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                            {
                                                foreach (TestMethod testMethod in departmentChain.TestMethods)
                                                {
                                                    if (!lstTestMethodOid.Contains(testMethod.Oid) && lstselTests.Contains(testMethod.Oid))
                                                    {
                                                        lstTestMethodOid.Add(testMethod.Oid);
                                                    }
                                                }
                                            }
                                        }

                                    }

                                    if (lstTestMethodOid.Count > 0)
                                    {
                                        int count = lstTestMethodOid.Count();

                                        if (count > 0)
                                        {
                                            child.Caption = cap[0] + " (" + count + ")";
                                        }
                                        else
                                        {
                                            child.Caption = cap[0];
                                        }
                                    }
                                }
                            }
                            ChoiceActionItem childDataPackageQueue = dataentryNode.Items.FirstOrDefault(i => i.Id == "DataPackageQueue");
                            if (childDataPackageQueue != null)
                            {
                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                var count = objectSpace.GetObjectsCount(typeof(SpreadSheetEntry_AnalyticalBatch), CriteriaOperator.Parse("[DPStatus] = 'PendingSubmission'"));
                                var cap = childDataPackageQueue.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                if (count > 0)
                                {
                                    childDataPackageQueue.Caption = cap[0] + " (" + count + ")";
                                }
                                else
                                {
                                    childDataPackageQueue.Caption = cap[0];
                                }
                            }
                            ChoiceActionItem childDataPackageReview = dataentryNode.Items.FirstOrDefault(i => i.Id == "DataPackageReview");
                            if (childDataPackageReview != null)
                            {
                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                var count = objectSpace.GetObjectsCount(typeof(SpreadSheetEntry_AnalyticalBatch), CriteriaOperator.Parse("[DPStatus] = 'PendingReview'"));
                                var cap = childDataPackageReview.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                if (count > 0)
                                {
                                    childDataPackageReview.Caption = cap[0] + " (" + count + ")";
                                }
                                else
                                {
                                    childDataPackageReview.Caption = cap[0];
                                }
                            }
                        }
                    }
                    else if (parent.Id == "InternalAudit")
                    {
                        ChoiceActionItem dataentryNode = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "InternalAudit");
                        if (dataentryNode != null)
                        {

                            ChoiceActionItem child = dataentryNode.Items.FirstOrDefault(i => i.Id == "NonconformityCorrectiveAction");
                            ChoiceActionItem Complaintchild = dataentryNode.Items.FirstOrDefault(i => i.Id == "ClientCompliant");
                            if (child != null)
                            {
                                ChoiceActionItem child1 = child.Items.FirstOrDefault(i => i.Id == "NonconformityInitiation");
                                ChoiceActionItem child2 = child.Items.FirstOrDefault(i => i.Id == "CorrectiveActionVerificationNonconformity");
                                if (child1 != null)
                                {
                                    //int count = 0;
                                    //var cap = child1.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //IObjectSpace objSpace = Application.CreateObjectSpace();
                                    //IList<NonConformityInitiation> lstTests = objSpace.GetObjects<NonConformityInitiation>(CriteriaOperator.Parse(""));
                                    //if (lstTests != null && lstTests.Count > 0)
                                    //{
                                    //    //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                                    //    count = lstTests.Where(i => i.Status == NCAStatus.PendingSubmission).Count();
                                    //}
                                    ////var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //if (count > 0)
                                    //{
                                    //    child1.Caption = cap[0] + " (" + count + ")";
                                    //}
                                    //else
                                    //{
                                    //    child1.Caption = cap[0];
                                    //}
                                }
                                if (child2 != null)
                                {
                                    //int count = 0;
                                    //var cap = child2.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //IObjectSpace objSpace = Application.CreateObjectSpace();
                                    //IList<NonConformityInitiation> lstTests = objSpace.GetObjects<NonConformityInitiation>(CriteriaOperator.Parse(""));
                                    //if (lstTests != null && lstTests.Count > 0)
                                    //{
                                    //    //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                                    //    count = lstTests.Where(i => i.Status == NCAStatus.PendingVerification).Count();
                                    //}
                                    ////var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //if (count > 0)
                                    //{
                                    //    child2.Caption = cap[0] + " (" + count + ")";
                                    //}
                                    //else
                                    //{
                                    //    child2.Caption = cap[0];
                                    //}
                                }


                            }
                            if (Complaintchild != null)
                            {
                                ChoiceActionItem child1 = Complaintchild.Items.FirstOrDefault(i => i.Id == "CompliantInitiation");
                                ChoiceActionItem child2 = Complaintchild.Items.FirstOrDefault(i => i.Id == "CorrectiveActionVerificationCompliant");
                                if (child1 != null)
                                {
                                    //int count = 0;
                                    //var cap = child1.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //IObjectSpace objSpace = Application.CreateObjectSpace();
                                    //IList<CompliantInitiation> lstTests = objSpace.GetObjects<CompliantInitiation>(CriteriaOperator.Parse(""));
                                    //if (lstTests != null && lstTests.Count > 0)
                                    //{
                                    //    //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                                    //    count = lstTests.Where(i => i.Status == CompliantInitiation.NCAStatus.PendingSubmission).Count();
                                    //}
                                    ////var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //if (count > 0)
                                    //{
                                    //    child1.Caption = cap[0] + " (" + count + ")";
                                    //}
                                    //else
                                    //{
                                    //    child1.Caption = cap[0];
                                    //}
                                }
                                if (child2 != null)
                                {
                                    //int count = 0;
                                    //var cap = child2.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //IObjectSpace objSpace = Application.CreateObjectSpace();
                                    //IList<CompliantInitiation> lstTests = objSpace.GetObjects<CompliantInitiation>(CriteriaOperator.Parse(""));
                                    //if (lstTests != null && lstTests.Count > 0)
                                    //{
                                    //    //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                                    //    count = lstTests.Where(i => i.Status == CompliantInitiation.NCAStatus.PendingVerification).Count();
                                    //}
                                    ////var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    //if (count > 0)
                                    //{
                                    //    child2.Caption = cap[0] + " (" + count + ")";
                                    //}
                                    //else
                                    //{
                                    //    child2.Caption = cap[0];
                                    //}
                                }
                            }
                        }
                    }
                }
                ChoiceActionItem parentCrm = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "Crm");
                if (parentCrm.Id == "Crm")
                {
                    foreach (ChoiceActionItem child in parentCrm.Items)
                    {
                        if (child.Id == "Accounts")
                        {
                            foreach (ChoiceActionItem subchild in child.Items)
                            {
                                if (subchild.Id == "Quotes")
                                {
                                    foreach (ChoiceActionItem subchilditem in subchild.Items)
                                    {
                                        if (subchilditem.Id == "OpenQuotes")
                                        {
                                            //IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            //var count = objectSpace.GetObjectsCount(typeof(CRMQuotes), CriteriaOperator.Parse("([Status] = 0) And [Cancel] = False And [ExpirationDate] >= ?", DateTime.Today));
                                            //var cap = subchilditem.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            //if (count > 0)
                                            //{
                                            //    subchilditem.Caption = cap[0] + " (" + count + ")";
                                            //}
                                            //else
                                            //{
                                            //    subchilditem.Caption = cap[0];
                                            //}
                                        }
                                        if (subchilditem.Id == "QuotesReview")
                                        {
                                            //IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            //var count = objectSpace.GetObjectsCount(typeof(CRMQuotes), CriteriaOperator.Parse("([Status] = 1) And [Cancel] = False And [ExpirationDate] >= ?", DateTime.Today));
                                            //var cap = subchilditem.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            //if (count > 0)
                                            //{
                                            //    subchilditem.Caption = cap[0] + " (" + count + ")";
                                            //}
                                            //else
                                            //{
                                            //    subchilditem.Caption = cap[0];
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ChoiceActionItem parentInvoicing = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "Accounting");
                if (parentInvoicing.Id == "Accounting")
                {
                    //ChoiceActionItem quotes = parentInvoicing.Items.FirstOrDefault(i => i.Id == "Quotes");
                    ////if (parent.Id == "Accounting")
                    //{
                    //    foreach (ChoiceActionItem child in quotes.Items)
                    //    {
                    //        if (child.Id == "OpenQuotes")
                    //        {
                    //            IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //            var count = objectSpace.GetObjectsCount(typeof(CRMQuotes), CriteriaOperator.Parse("([Status] = 0) And [Cancel] = False And [ExpirationDate] >= ?", DateTime.Today));
                    //            var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //            if (count > 0)
                    //            {
                    //                child.Caption = cap[0] + " (" + count + ")";
                    //            }
                    //            else
                    //            {
                    //                child.Caption = cap[0];
                    //            }
                    //        }
                    //        if (child.Id == "QuotesReview")
                    //        {
                    //            IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //            var count = objectSpace.GetObjectsCount(typeof(CRMQuotes), CriteriaOperator.Parse("([Status] = 1) And [Cancel] = False And [ExpirationDate] >= ?", DateTime.Today));
                    //            var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //            if (count > 0)
                    //            {
                    //                child.Caption = cap[0] + " (" + count + ")";
                    //            }
                    //            else
                    //            {
                    //                child.Caption = cap[0];
                    //            }
                    //        }
                    //    }
                    //}

                    ChoiceActionItem Invoicing = parentInvoicing.Items.FirstOrDefault(i => i.Id == "Invoicing");
                    if (Invoicing != null)
                    {
                        ChoiceActionItem childInvoiceQueue = Invoicing.Items.FirstOrDefault(i => i.Id == "InvoiceQueue" || i.Id == "InvoiceQueue ");
                        if (childInvoiceQueue != null)
                        {
                            int count = 0;
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            List<Guid> lstinvoiceOid = new List<Guid>();
                            IList<SampleParameter> samples = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status] = 'Reported' And ([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null)"));
                            List<Guid> lstScGuid = samples.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && (i.Samplelogin.JobID.ProjectCategory is null || (i.Samplelogin.JobID.ProjectCategory != null && i.Samplelogin.JobID.ProjectCategory.Non_Commercial != CommercialType.Yes))).Select(i => i.Samplelogin.JobID.Oid).Distinct().ToList();
                            foreach (Guid obj in lstScGuid)
                            {
                                IList<SampleParameter> lstsamplesInvoice = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Status] <> 'Reported' And [Samplelogin.JobID.Oid]=? ", obj));
                                if (lstsamplesInvoice.Count == 0)
                                {
                                    if (!lstinvoiceOid.Contains(obj))
                                    {
                                        lstinvoiceOid.Add(obj);
                                        count = count + 1;
                                    }
                                }
                            }

                            var cap = childInvoiceQueue.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            if (count > 0)
                            {
                                childInvoiceQueue.Caption = cap[0] + " (" + count + ")";
                            }
                            else
                            {
                                childInvoiceQueue.Caption = cap[0];
                            }
                        }
                        ChoiceActionItem childInvoiceSubmi = Invoicing.Items.FirstOrDefault(i => i.Id == "InvoiceSubmission");
                        if (childInvoiceSubmi != null)
                        {
                            //int count = 0;
                            //IObjectSpace objSpace = Application.CreateObjectSpace();
                            //IList<Invoicing> lstCount = objSpace.GetObjects<Invoicing>(CriteriaOperator.Parse("[Status] = 'PendingSubmit'"));
                            //var cap = childInvoiceSubmi.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //if (lstCount.Count > 0)
                            //{
                            //    count = lstCount.Count();
                            //    childInvoiceSubmi.Caption = cap[0] + " (" + count + ")";
                            //}
                            //else
                            //{
                            //    childInvoiceSubmi.Caption = cap[0];
                            //}
                        }
                        ChoiceActionItem childInvoiceReview = Invoicing.Items.FirstOrDefault(i => i.Id == "InvoiceReview");
                        if (childInvoiceReview != null)
                        {
                            //int count = 0;
                            //IObjectSpace objSpace = Application.CreateObjectSpace();
                            //IList<Invoicing> lstCount = objSpace.GetObjects<Invoicing>(CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                            //var cap = childInvoiceReview.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //if (lstCount.Count > 0)
                            //{
                            //    count = lstCount.Count();
                            //    childInvoiceReview.Caption = cap[0] + " (" + count + ")";
                            //}
                            //else
                            //{
                            //    childInvoiceReview.Caption = cap[0];
                            //}
                        }
                        ChoiceActionItem childInvoiceDelivery = Invoicing.Items.FirstOrDefault(i => i.Id == "InvoiceDelivery");
                        if (childInvoiceDelivery != null)
                        {
                            //int count = 0;
                            //IObjectSpace objSpace = Application.CreateObjectSpace();
                            //IList<Invoicing> lstCount = objSpace.GetObjects<Invoicing>(CriteriaOperator.Parse("[Status] = 'PendingDelivery'"));
                            //var cap = childInvoiceDelivery.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //if (lstCount.Count > 0)
                            //{
                            //    count = lstCount.Count();
                            //    childInvoiceDelivery.Caption = cap[0] + " (" + count + ")";
                            //}
                            //else
                            //{
                            //    childInvoiceDelivery.Caption = cap[0];
                            //}
                        }
                        ChoiceActionItem childInvoiceEDDExport = Invoicing.Items.FirstOrDefault(i => i.Id == "InvoiceEDDExport");
                        if (childInvoiceEDDExport != null)
                        {
                            //int count = 0;
                            //IObjectSpace objSpace = Application.CreateObjectSpace();
                            //IList<InvoicingEDDExport> lstCount = objSpace.GetObjects<InvoicingEDDExport>(CriteriaOperator.Parse("Not IsNullOrEmpty([EDDID])"));
                            //var cap = childInvoiceEDDExport.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //if (lstCount.Count > 0)
                            //{
                            //    count = lstCount.Count();
                            //    childInvoiceEDDExport.Caption = cap[0] + " (" + count + ")";
                            //}
                            //else
                            //{
                            //    childInvoiceEDDExport.Caption = cap[0];
                            //}
                        }

                    }
                    ChoiceActionItem Receivables = parentInvoicing.Items.FirstOrDefault(i => i.Id == "Receivables");
                    if (Receivables != null)
                    {
                        ChoiceActionItem childDeposit = Receivables.Items.FirstOrDefault(i => i.Id == "Deposit");
                        if (childDeposit != null)
                        {
                            //int count = 0;
                            //IObjectSpace objSpace = Application.CreateObjectSpace();
                            //IList<Deposits> lstCount = objSpace.GetObjects<Deposits>(CriteriaOperator.Parse("[Status] = 'Unpaid' Or [Status] = 'PartiallyPaid'"));
                            //var cap = childDeposit.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //if (lstCount.Count > 0)
                            //{
                            //    count = lstCount.Count();
                            //    childDeposit.Caption = cap[0] + " (" + count + ")";
                            //}
                            //else
                            //{
                            //    childDeposit.Caption = cap[0];
                            //}
                        }
                        ChoiceActionItem childDepositEDDExport = Receivables.Items.FirstOrDefault(i => i.Id == "DepositEDDExport");
                        if (childDepositEDDExport != null)
                        {
                            //int count = 0;
                            //IObjectSpace objSpace = Application.CreateObjectSpace();
                            //IList<DepositEDDExport> lstCount = objSpace.GetObjects<DepositEDDExport>(CriteriaOperator.Parse("[Oid] Is Not Null"));
                            //var cap = childDepositEDDExport.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //if (lstCount.Count > 0)
                            //{
                            //    count = lstCount.Count();
                            //    childDepositEDDExport.Caption = cap[0] + " (" + count + ")";
                            //}
                            //else
                            //{
                            //    childDepositEDDExport.Caption = cap[0];
                            //}
                        }
                    }

                }
                ChoiceActionItem DeliveryService = parentInvoicing.Items.FirstOrDefault(i => i.Id == "DeliveryService");
                if (DeliveryService != null)
                {
                    ChoiceActionItem childDeliveryTasks = DeliveryService.Items.FirstOrDefault(i => i.Id == "DeliveryTasks");
                    if (childDeliveryTasks != null)
                    {
                        //int count = 0;
                        //IObjectSpace objSpace = Application.CreateObjectSpace();
                        //IList<ClientRequest> objDeliveryTasks = objSpace.GetObjects<ClientRequest>(CriteriaOperator.Parse("[Status] = 'PendingDelivery' And[DeliveryBottleOrders][[Status] = 'PendingDelivery'].Count() > 0"));
                        //var cap = childDeliveryTasks.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        //if (objDeliveryTasks != null && objDeliveryTasks.Count > 0)
                        //{
                        //    count = objDeliveryTasks.Count;
                        //    if (count > 0)
                        //    {
                        //        childDeliveryTasks.Caption = cap[0] + " (" + count + ")";
                        //    }
                        //    else
                        //    {
                        //        childDeliveryTasks.Caption = cap[0];
                        //    }
                        //}
                        //else
                        //{
                        //    childDeliveryTasks.Caption = cap[0];
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                //Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ShowNavigationItemController_NavigationItemCreated(object sender, NavigationItemCreatedEventArgs e)
        {
            try
            {
                ChoiceActionItem parentChoiceActionItem = e.NavigationItem;
                if (parentChoiceActionItem.Id == "DocumentManagement")
                {
                    if (parentChoiceActionItem.Items.FirstOrDefault(i => i.Id == "DocumentsManagement") != null)
                    {
                        ChoiceActionItem childChoiceActionItem = parentChoiceActionItem.Items.FirstOrDefault(i => i.Id == "DocumentsManagement");
                        if (childChoiceActionItem != null)
                        {
                            IObjectSpace ObjectSpace = Application.CreateObjectSpace();
                            Employee objEmployee = (Employee)SecuritySystem.CurrentUser;
                            if (objEmployee != null && objEmployee.UserName != "Administrator" && objEmployee.UserName != "Service")
                            {
                                List<string> lstRoles = objEmployee.RoleNames.Split(',').ToList();
                                foreach (string strrole in lstRoles)
                                {
                                    RoleNavigationPermission objRNP = ObjectSpace.FindObject<RoleNavigationPermission>(CriteriaOperator.Parse("[RoleName] = ?", strrole));
                                    if (objRNP != null)
                                    {
                                        RoleNavigationPermissionDetails objRNPD = objRNP.RoleNavigationPermissionDetails.Where(i => i.NavigationItem.NavigationId == "DocumentCreator").FirstOrDefault(i => i.NavigationItem.NavigationId == "DocumentCreator");
                                        if (objRNPD != null)
                                        {
                                            IList<DocumentCategory> objlist = ObjectSpace.GetObjects<DocumentCategory>();
                                            foreach (DocumentCategory documentCategory in objlist.OrderBy(i => i.Name))
                                            {
                                                IList<Manual> lstManuals = ObjectSpace.GetObjects<Manual>(CriteriaOperator.Parse("[Category.Name] = ?  And ([IsRetire] = False or [IsRetire] = True And [DateRetired] > CURRENTDATETIME()) And [Attachments][[IsActive] = True].Count() > 0", documentCategory.Name));
                                                if (lstManuals.Count > 0 && childChoiceActionItem.Items.FirstOrDefault(i => i.Id == documentCategory.Name) == null)
                                                {
                                                    ViewShortcut viCategory = new ViewShortcut("Manual_ListView_Category", string.Empty);
                                                    ChoiceActionItem subChildChoiceActionItem = new ChoiceActionItem(documentCategory.Name, documentCategory.Name, viCategory);
                                                    subChildChoiceActionItem.ImageName = "BO_Note";
                                                    childChoiceActionItem.Items.Add(subChildChoiceActionItem);
                                                }
                                            }
                                        }
                                    }
                                    return;
                                }
                            }
                            else
                            {
                                IList<DocumentCategory> objlist = ObjectSpace.GetObjects<DocumentCategory>();
                                foreach (DocumentCategory documentCategory in objlist.OrderBy(i => i.Name))
                                {
                                    IList<Manual> lstManuals = ObjectSpace.GetObjects<Manual>(CriteriaOperator.Parse("[Category.Name] = ? And ([IsRetire] = False or [IsRetire] = True And [DateRetired] > CURRENTDATETIME()) And [Attachments][[IsActive] = True].Count() > 0", documentCategory.Name));
                                    if (lstManuals.Count > 0 && childChoiceActionItem.Items.FirstOrDefault(i => i.Id == documentCategory.Name) == null)
                                    {
                                        ViewShortcut viCategory = new ViewShortcut("Manual_ListView_Category", string.Empty);
                                        ChoiceActionItem subChildChoiceActionItem = new ChoiceActionItem(documentCategory.Name, documentCategory.Name, viCategory);
                                        subChildChoiceActionItem.ImageName = "BO_Note";
                                        childChoiceActionItem.Items.Add(subChildChoiceActionItem);
                                    }
                                }
                            }
                        }
                    }
                    //if (parentChoiceActionItem.Items.FirstOrDefault(i => i.Id == "DocumentsManagement") != null)
                    //{
                    //    ChoiceActionItem childChoiceActionItem = parentChoiceActionItem.Items.FirstOrDefault(i => i.Id == "DocumentsManagement");
                    //    if (childChoiceActionItem != null)
                    //    {
                    //        //IObjectSpace objectspace = Application.CreateObjectSpace();
                    //        //IList<DocumentCategory> objlist = objectspace.GetObjects<DocumentCategory>();
                    //        //foreach (DocumentCategory documentCategory in objlist)
                    //        //{
                    //        //    IList<Manual> lstManuals = objectspace.GetObjects<Manual>(CriteriaOperator.Parse("[Category.Category] = ? And [Attachments][[IsActive] = True].Count() > 0", documentCategory.Category));
                    //        //    if (lstManuals.Count > 0)
                    //        //    {
                    //        //        ViewShortcut viCategory = new ViewShortcut("Manual_ListView_Category", string.Empty);
                    //        //        ChoiceActionItem subChildChoiceActionItem = new ChoiceActionItem(documentCategory.Category, documentCategory.Category, viCategory);
                    //        //        childChoiceActionItem.Items.Add(subChildChoiceActionItem);
                    //        //    }
                    //        //}
                    //    }
                    //}
                }
                //if (parentChoiceActionItem.Id == "CalibrationLog")
                //{
                //    if (parentChoiceActionItem.Items.FirstOrDefault(i => i.Id == "CalibrationLog") != null)
                //    {
                //        ChoiceActionItem childChoiceActionItem = parentChoiceActionItem.Items.FirstOrDefault(i => i.Id == "CalibrationLog");
                //        if (childChoiceActionItem != null)
                //        {
                //            //IObjectSpace objectspace = Application.CreateObjectSpace();
                //            //IList<CalibrationCategory> objcalicat = objectspace.GetObjects<CalibrationCategory>();
                //            //foreach (CalibrationCategory CaliCategory in objcalicat.OrderBy(i => i.CategoryName))
                //            //{
                //            //    IList<NotebookBuilder> lstManuals = objectspace.GetObjects<NotebookBuilder>(CriteriaOperator.Parse(""));
                //            //    if (lstManuals.Count > 0)
                //            //    {
                //            //        ViewShortcut viCategory = new ViewShortcut("NotebookBuilder_ListView_CalibrationQueue", string.Empty);
                //            //        ChoiceActionItem subChildChoiceActionItem = new ChoiceActionItem(CaliCategory.CategoryName, CaliCategory.CategoryName, viCategory);
                //            //        childChoiceActionItem.Items.Add(subChildChoiceActionItem);
                //            //        //childChoiceActionItem.ImageName = "BO_Unknown";
                //            //        subChildChoiceActionItem.ImageName = "BO_Unknown";
                //            //    }
                //            //}
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion
    }
}
