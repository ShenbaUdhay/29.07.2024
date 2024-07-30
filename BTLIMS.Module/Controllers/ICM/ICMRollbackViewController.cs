using BTLIMS.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using static ICM.Module.BusinessObjects.Requisition;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ICMRollbackViewController : ViewController
    {
        RollbackCC RollbackInfo = new RollbackCC();
        #region Declaration
        MessageTimer timer = new MessageTimer();
        ICMinfo objIcmInfo = new ICMinfo();
        ICMinfo Filter = new ICMinfo();
        string AdminFilter;
        ShowNavigationItemController ShowNavigationController;
        PermissionInfo objPermissionInfo = new PermissionInfo();
        curlanguage objLanguage = new curlanguage();

        //ICMinfo Filter = new ICMinfo();
        #endregion

        #region Constructor
        public ICMRollbackViewController()
        {
            InitializeComponent();
            this.TargetViewId = "Requisition_ListView_Review;" + "Requisition_ListView_Approve;" /*+ "Requisition_ListView_Cancelled;"*/ + "Requisition_ListView_ViewMode;" + "ICMRollBack_DetailView;";
            Filter.ApproveFilter = string.Empty;
            AdminFilter = string.Empty;
            //ICMRollBackAction.Category = "RecordEdit";
            //ICMRollBackAction.Model.Index = 5;
        }
        #endregion

        #region Default Methods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                //CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse(""));
                //if (View.Id == "Requisition_ListView_Cancelled")
                //{
                //    ICMRollBackAction.Active["ShowActive"] = false;
                //}
                if (View.Id == "Requisition_ListView_Review")
                {
                    if (objLanguage.strcurlanguage == "En")
                    {
                        ICMRollBackAction.Caption = "Cancel";
                        RollbackInfo.Rollback = "Cancel";
                    }
                    else
                    {
                        ICMRollBackAction.Caption = "取消";
                    }
                    ICMRollBackAction.ToolTip = "Cancel";
                    ICMRollBackAction.ImageName = "State_Validation_Invalid";
                }
                else if (View.Id == "Requisition_ListView_Approve")
                {
                    if (objLanguage.strcurlanguage == "En")
                    {
                        ICMRollBackAction.Caption = "Roll Back";
                    }
                    else
                    {
                        ICMRollBackAction.Caption = "退回";
                    }

                    ICMRollBackAction.ToolTip = "RollBack";
                    ICMRollBackAction.ImageName = "Action_Cancel";
                }
                else if (View.Id == "Requisition_ListView_Cancelled" || View.Id == "Requisition_ListView_ViewMode")
                {
                    ObjectSpace.Committed += ObjectSpace_Committed;
                    if (objLanguage.strcurlanguage == "En")
                    {
                        ICMRollBackAction.Caption = "RollBack";
                        RollbackInfo.Rollback = "Rollback";
                    }
                    else
                    {
                        ICMRollBackAction.Caption = "退回";
                    }

                    ICMRollBackAction.ToolTip = "RollBack";
                    ICMRollBackAction.ImageName = "Action_Cancel";
                }
                //Permisson code
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser != null && View != null && View.Id != null)
                {

                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.RequisitionReviewIsWrite = false;
                        objPermissionInfo.RequisitionApprovalIsWrite = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.RequisitionReviewIsWrite = true;
                            objPermissionInfo.RequisitionApprovalIsWrite = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Review" && i.Write == true) != null)
                                {
                                    objPermissionInfo.RequisitionReviewIsWrite = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "RequisitionApproval" && i.Write == true) != null)
                                {
                                    objPermissionInfo.RequisitionApprovalIsWrite = true;
                                    //return;
                                }
                            }
                        }
                    }
                }
                if (View.Id == "Requisition_ListView_Review")
                {
                    ICMRollBackAction.Active["ShowRollback"] = objPermissionInfo.RequisitionReviewIsWrite;
                }
                else
                if (View.Id == "Requisition_ListView_Approve")
                {
                    ICMRollBackAction.Active["ShowRollback"] = objPermissionInfo.RequisitionApprovalIsWrite;
                }

                //Employee currentUser = SecuritySystem.CurrentUser as Employee;
                //if (currentUser != null && View != null && View.Id != null)
                //{
                //    IObjectSpace objSpace = Application.CreateObjectSpace();
                //    CriteriaOperator criteria = CriteriaOperator.Parse("[User]='" + currentUser.Oid + "'");
                //    UserNavigationPermission userpermission = objSpace.FindObject<UserNavigationPermission>(criteria);
                //    CriteriaOperator navcriteria = CriteriaOperator.Parse("[NavigationView]='" + View.Id + "'");
                //    Modules.BusinessObjects.Setting.NavigationItem navigation = objSpace.FindObject<Modules.BusinessObjects.Setting.NavigationItem>(navcriteria);
                //    if (navigation != null && userpermission != null)
                //    {
                //        CriteriaOperator navpermissioncriteria = CriteriaOperator.Parse("[NavigationItem]='" + navigation.Oid + "' and [UserNavigationPermission]='" + userpermission.Oid + "'");
                //        UserNavigationPermissionDetails navPermissionDetails = objSpace.FindObject<UserNavigationPermissionDetails>(navpermissioncriteria);
                //        if (navPermissionDetails != null && View.Id == "Requisition_ListView_Review")
                //        {
                //            if (navPermissionDetails.Write == true)
                //            {
                //                ICMRollBackAction.Active.SetItemValue("ICMRollbackViewController.ICMRollBackAction", true);

                //            }
                //            else if (navPermissionDetails.Write == false)
                //            {
                //                ICMRollBackAction.Active.SetItemValue("ICMRollbackViewController.ICMRollBackAction", false);
                //            }
                //        }
                //        else if (navPermissionDetails != null && View.Id == "Requisition_ListView_Approve")
                //        {
                //            if (navPermissionDetails.Write == true)
                //            {
                //                ICMRollBackAction.Active.SetItemValue("ICMRollbackViewController.ICMRollBackAction", true);

                //            }
                //            else if (navPermissionDetails.Write == false)
                //            {
                //                ICMRollBackAction.Active.SetItemValue("ICMRollbackViewController.ICMRollBackAction", false);
                //            }
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

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "Requisition_ListView_ViewMode")
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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
                RollbackCC RollbackInfo = new RollbackCC();

                if (View.Id == "ICMRollBack_DetailView" && RollbackInfo.Rollback == "Cancel")
                {
                    //View.Caption = "Item Cancellation Comment";
                    View.Caption = "Item Cancellation Reason";
                }
                if (View.Id == "Requisition_ListView_Approve" || View.Id == "Requisition_ListView_Review")
                {
                    FuncCurrentUserIsAdministrative obj = new FuncCurrentUserIsAdministrative();
                    object val = obj.Evaluate();
                    if ((string)val == "0")
                    {
                        object CurrentUser = SecuritySystem.CurrentUserId;
                        IObjectSpace objectspace = Application.CreateObjectSpace(typeof(WorkflowConfig));
                        CriteriaOperator criteria = CriteriaOperator.Parse("GCRecord IS Null");
                        IList<WorkflowConfig> ICMWorkFlow = objectspace.GetObjects<WorkflowConfig>(criteria);
                        foreach (WorkflowConfig item in ICMWorkFlow)
                        {
                            if (item.Level == 1)
                            {
                                foreach (CustomSystemUser userid in item.User)
                                {

                                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    {
                                        if (Filter.ApproveFilter == string.Empty)
                                        {
                                            Filter.ApproveFilter = "Status='" + TaskStatus.Level1Pending + "'";
                                        }
                                        else
                                        {
                                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level1Pending + "'";
                                        }
                                    }
                                }
                            }
                            if (item.Level == 2)
                            {
                                foreach (CustomSystemUser userid in item.User)
                                {
                                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    {
                                        if (Filter.ApproveFilter == string.Empty)
                                        {
                                            Filter.ApproveFilter = "Status='" + TaskStatus.Level2Pending + "'";
                                        }
                                        else
                                        {
                                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level2Pending + "'";
                                        }
                                    }
                                }
                            }
                            if (item.Level == 3)
                            {
                                foreach (CustomSystemUser userid in item.User)
                                {
                                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    {
                                        if (Filter.ApproveFilter == string.Empty)
                                        {
                                            Filter.ApproveFilter = "Status='" + TaskStatus.Level3Pending + "'";
                                        }
                                        else
                                        {
                                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level3Pending + "'";
                                        }
                                    }
                                }
                            }
                            if (item.Level == 4)
                            {
                                foreach (CustomSystemUser userid in item.User)
                                {
                                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    {
                                        if (Filter.ApproveFilter == string.Empty)
                                        {
                                            Filter.ApproveFilter = "Status='" + TaskStatus.Level4Pending + "'";
                                        }
                                        else
                                        {
                                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level2Pending + "'";
                                        }
                                    }
                                }
                            }
                            if (item.Level == 5)
                            {
                                foreach (CustomSystemUser userid in item.User)
                                {
                                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    {
                                        if (Filter.ApproveFilter == string.Empty)
                                        {
                                            Filter.ApproveFilter = "Status=" + TaskStatus.Level5Pending + "'";
                                        }
                                        else
                                        {
                                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level5Pending + "'";
                                        }
                                    }
                                }
                            }
                            if (item.Level == 6)
                            {
                                foreach (CustomSystemUser userid in item.User)
                                {
                                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    {
                                        if (Filter.ApproveFilter == string.Empty)
                                        {
                                            Filter.ApproveFilter = "Status='" + TaskStatus.Level6Pending + "'";
                                        }
                                        else
                                        {
                                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level6Pending + "'";
                                        }
                                    }
                                }
                            }

                        }
                    }
                    else if ((string)val == "1")
                    {
                        AdminFilter = "Status='" + TaskStatus.Level1Pending + "'|| Status='" + TaskStatus.Level2Pending + "'|| Status='" + TaskStatus.Level3Pending + "'||Status='" + TaskStatus.Level4Pending + "'||Status='" +
                            TaskStatus.Level5Pending + "'|| Status='" + TaskStatus.Level6Pending + "'";
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            //if (View.Id == "Requisition_ListView_Review")
            //{
            //    if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Requisition))
            //    {
            //        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Status] = 'PendingReview'");
            //    }
            //}
            //else if (View.Id == "Requisition_ListView_Approve")
            //{
            //    if (Filter.ApproveFilter != string.Empty)
            //    {
            //        if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Requisition))
            //        {
            //            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(Filter.ApproveFilter);
            //        }
            //    }
            //    else
            //    {
            //        if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Requisition))
            //        {
            //            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(AdminFilter);
            //        }
            //    }
            //}
        }
        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();
                RollbackInfo.Rollback = string.Empty;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events
        private void ICMRollBack_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count == 0)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ICMRollBack_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                //if (ICMRollBackAction.Caption == "Cancel")
                //{
                //    RollbackInfo.Rollback = "Cancel";

                //    //if (View.Id == "ICMRollBack_DetailView" && RollbackInfo.Rollback == "Cancel")
                //    //{

                //    //    View.Caption = "abc";
                //    //}
                //}

                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(ICMRollBack));
                if (objToShow != null)
                {
                    DetailView CreateDetailView = Application.CreateDetailView(objspace, objToShow);
                    CreateDetailView.ViewEditMode = ViewEditMode.Edit;
                    e.DialogController.SaveOnAccept = true;
                    e.DialogController.Accepting += DialogController_Accepting;
                    e.View = CreateDetailView;
                    if (View.Id == "Requisition_ListView_Review")
                    {
                        //CreateDetailView.Caption = "Item Cancellation Comment";
                        CreateDetailView.Caption = "Reason";
                    }
                    else if (View.Id == "Requisition_ListView_ViewMode")
                    {
                        CreateDetailView.Caption = "ICM RollBack";
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (objIcmInfo.RollBackReason == null)
                {

                    if (RollbackInfo.Rollback == "Cancel")
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CancelReason"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreason"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        //Application.ShowViewStrategy.ShowMessage("Please enter the cancel reason", InformationType.Error, timer.Seconds, InformationPosition.Top);

                    }
                    e.Cancel = true;
                }
                else
                {
                    if (RollbackInfo.Rollback == "Cancel")
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cancellsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ICMRollBack_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                if (objIcmInfo.RollBackReason != null)
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        if (base.View != null && base.View.Id == "Requisition_ListView_Review")
                        {
                            foreach (Requisition objReq in View.SelectedObjects)
                            {
                                if (objReq.Status == TaskStatus.PendingReview)
                                {
                                    objReq.ReviewedDate = null;
                                    objReq.ReviewedBy = null;
                                }
                                objReq.Status = Requisition.TaskStatus.Cancelled;
                                objReq.CanceledBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                objReq.CanceledDate = DateTime.Now;
                                objReq.RollbackReason = objIcmInfo.RollBackReason;
                                ObjectSpace.CommitChanges();
                            }
                            View.ObjectSpace.Refresh();
                            View.Refresh();


                            //ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                            //foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                            //{
                            //    if (parent.Id == "InventoryManagement")
                            //    {
                            //        foreach (ChoiceActionItem child in parent.Items)
                            //        {
                            //            if (child.Id == "Operations")
                            //            {
                            //                foreach (ChoiceActionItem subchild in child.Items)
                            //                {
                            //                    if (subchild.Id == "Review")
                            //                    {
                            //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //                        var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                            //                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //                        if (count > 0)
                            //                        {
                            //                            subchild.Caption = cap[0] + " (" + count + ")";
                            //                        }
                            //                        else
                            //                        {
                            //                            subchild.Caption = cap[0];
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                        }
                        else if (base.View != null && base.View.Id == "Requisition_ListView_Approve")
                        {
                            foreach (Requisition objReq in View.SelectedObjects)
                            {
                                objReq.Status = Requisition.TaskStatus.PendingReview;
                                objReq.RollbackReason = objIcmInfo.RollBackReason;
                                objReq.ApprovedDate = null;
                                objReq.ApprovedBy = null;
                                ObjectSpace.CommitChanges();
                            }
                            View.ObjectSpace.Refresh();
                            View.Refresh();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            //ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                            //foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                            //{
                            //    if (parent.Id == "InventoryManagement")
                            //    {
                            //        foreach (ChoiceActionItem child in parent.Items)
                            //        {
                            //            if (child.Id == "Operations")
                            //            {
                            //                foreach (ChoiceActionItem subchild in child.Items)
                            //                {
                            //                    if (subchild.Id == "Review")
                            //                    {
                            //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //                        var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                            //                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //                        if (count > 0)
                            //                        {
                            //                            subchild.Caption = cap[0] + " (" + count + ")";
                            //                        }
                            //                        else
                            //                        {
                            //                            subchild.Caption = cap[0];
                            //                        }
                            //                    }

                            //                    else if (subchild.Id == "RequisitionApproval")
                            //                    {
                            //                        if (Filter.ApproveFilter != string.Empty)
                            //                        {

                            //                            int intValue = 0;
                            //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //                            CriteriaOperator criteria = CriteriaOperator.Parse(Filter.ApproveFilter);
                            //                            IList<Requisition> req = ObjectSpace.GetObjects<Requisition>(criteria);
                            //                            string[] batch = new string[req.Count];
                            //                            foreach (Requisition item in req)
                            //                            {
                            //                                if (!batch.Contains(item.BatchID))
                            //                                {
                            //                                    batch[intValue] = item.BatchID;
                            //                                    intValue = intValue + 1;
                            //                                }
                            //                            }
                            //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //                            if (intValue > 0)
                            //                            {
                            //                                subchild.Caption = cap[0] + " (" + intValue + ")";
                            //                            }
                            //                            else
                            //                            {
                            //                                subchild.Caption = cap[0];
                            //                            }
                            //                        }
                            //                        else
                            //                        {
                            //                            int intValue = 0;
                            //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //                            CriteriaOperator criteria = CriteriaOperator.Parse(AdminFilter);
                            //                            IList<Requisition> req = ObjectSpace.GetObjects<Requisition>(criteria);
                            //                            string[] batch = new string[req.Count];
                            //                            foreach (Requisition item in req)
                            //                            {
                            //                                if (!batch.Contains(item.BatchID))
                            //                                {
                            //                                    batch[intValue] = item.BatchID;
                            //                                    intValue = intValue + 1;
                            //                                }
                            //                            }
                            //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //                            if (intValue > 0)
                            //                            {
                            //                                subchild.Caption = cap[0] + " (" + intValue + ")";
                            //                            }
                            //                            else
                            //                            {
                            //                                subchild.Caption = cap[0];
                            //                            }
                            //                        }

                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                        }
                        else if (base.View != null && base.View.Id == "Requisition_ListView_Cancelled")
                        {
                            foreach (Requisition obj in View.SelectedObjects)
                            {
                                if (obj.Status == TaskStatus.Cancelled)
                                {
                                    obj.Status = TaskStatus.PendingReview;
                                    obj.RollbackReason = objIcmInfo.RollBackReason;
                                    obj.CanceledDate = null;
                                    obj.CanceledBy = null;
                                    ObjectSpace.CommitChanges();
                                }
                            }
                            View.Refresh();
                            ObjectSpace.Refresh();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            //ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                            //foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                            //{
                            //    if (parent.Id == "InventoryManagement")
                            //    {
                            //        foreach (ChoiceActionItem child in parent.Items)
                            //        {
                            //            if (child.Id == "Operations")
                            //            {
                            //                foreach (ChoiceActionItem subchild in child.Items)
                            //                {
                            //                    if (subchild.Id == "Review")
                            //                    {
                            //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //                        var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                            //                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //                        if (count > 0)
                            //                        {
                            //                            subchild.Caption = cap[0] + " (" + count + ")";
                            //                        }
                            //                        else
                            //                        {
                            //                            subchild.Caption = cap[0];
                            //                        }
                            //                    }

                            //                    else if (subchild.Id == "RequisitionApproval")
                            //                    {
                            //                        if (Filter.ApproveFilter != string.Empty)
                            //                        {

                            //                            int intValue = 0;
                            //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //                            CriteriaOperator criteria = CriteriaOperator.Parse(Filter.ApproveFilter);
                            //                            IList<Requisition> req = ObjectSpace.GetObjects<Requisition>(criteria);
                            //                            string[] batch = new string[req.Count];
                            //                            foreach (Requisition item in req)
                            //                            {
                            //                                if (!batch.Contains(item.BatchID))
                            //                                {
                            //                                    batch[intValue] = item.BatchID;
                            //                                    intValue = intValue + 1;
                            //                                }
                            //                            }
                            //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //                            if (intValue > 0)
                            //                            {
                            //                                subchild.Caption = cap[0] + " (" + intValue + ")";
                            //                            }
                            //                            else
                            //                            {
                            //                                subchild.Caption = cap[0];
                            //                            }
                            //                        }
                            //                        else
                            //                        {
                            //                            int intValue = 0;
                            //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //                            CriteriaOperator criteria = CriteriaOperator.Parse(AdminFilter);
                            //                            IList<Requisition> req = ObjectSpace.GetObjects<Requisition>(criteria);
                            //                            string[] batch = new string[req.Count];
                            //                            foreach (Requisition item in req)
                            //                            {
                            //                                if (!batch.Contains(item.BatchID))
                            //                                {
                            //                                    batch[intValue] = item.BatchID;
                            //                                    intValue = intValue + 1;
                            //                                }
                            //                            }
                            //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //                            if (intValue > 0)
                            //                            {
                            //                                subchild.Caption = cap[0] + " (" + intValue + ")";
                            //                            }
                            //                            else
                            //                            {
                            //                                subchild.Caption = cap[0];
                            //                            }
                            //                        }

                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                        }
                        else if (base.View != null && base.View.Id == "Requisition_ListView_ViewMode")
                        {
                            foreach (Requisition obj in View.SelectedObjects)
                            {
                                if (obj.Status == TaskStatus.PendingOrdering)
                                {
                                    obj.Status = TaskStatus.PendingReview;
                                    obj.RollbackReason = objIcmInfo.RollBackReason;
                                    obj.ReviewedBy = null;
                                    obj.ReviewedDate = null;
                                    ObjectSpace.CommitChanges();
                                }
                                else if (obj.Status == TaskStatus.PendingOrderingApproval || obj.Status == TaskStatus.PendingReceived || obj.Status == TaskStatus.Level10Pending || obj.Status == TaskStatus.Level1Pending
                                    || obj.Status == TaskStatus.Level2Pending || obj.Status == TaskStatus.Level3Pending || obj.Status == TaskStatus.Level4Pending || obj.Status == TaskStatus.Level5Pending || obj.Status == TaskStatus.Level6Pending
                                    || obj.Status == TaskStatus.Level7Pending || obj.Status == TaskStatus.Level8Pending || obj.Status == TaskStatus.Level9Pending || obj.Status == TaskStatus.PendingApproval)
                                {
                                    obj.Status = TaskStatus.PendingReview;
                                    obj.RollbackReason = objIcmInfo.RollBackReason;
                                    obj.ReviewedBy = null;
                                    obj.ReviewedDate = null;
                                    ObjectSpace.CommitChanges();
                                }
                            }
                            View.Refresh();
                            ObjectSpace.Refresh();
                            //ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                            //foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                            //{
                            //    if (parent.Id == "InventoryManagement")
                            //    {
                            //        foreach (ChoiceActionItem child in parent.Items)
                            //        {
                            //            if (child.Id == "Operations")
                            //            {
                            //                foreach (ChoiceActionItem subchild in child.Items)
                            //                {
                            //                    if (subchild.Id == "Review")
                            //                    {
                            //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //                        var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                            //                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //                        if (count > 0)
                            //                        {
                            //                            subchild.Caption = cap[0] + " (" + count + ")";
                            //                        }
                            //                        else
                            //                        {
                            //                            subchild.Caption = cap[0];
                            //                        }
                            //                    }

                            //                    else if (subchild.Id == "RequisitionApproval")
                            //                    {
                            //                        if (Filter.ApproveFilter != string.Empty)
                            //                        {

                            //                            int intValue = 0;
                            //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //                            CriteriaOperator criteria = CriteriaOperator.Parse(Filter.ApproveFilter);
                            //                            IList<Requisition> req = ObjectSpace.GetObjects<Requisition>(criteria);
                            //                            string[] batch = new string[req.Count];
                            //                            foreach (Requisition item in req)
                            //                            {
                            //                                if (!batch.Contains(item.BatchID))
                            //                                {
                            //                                    batch[intValue] = item.BatchID;
                            //                                    intValue = intValue + 1;
                            //                                }
                            //                            }
                            //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //                            if (intValue > 0)
                            //                            {
                            //                                subchild.Caption = cap[0] + " (" + intValue + ")";
                            //                            }
                            //                            else
                            //                            {
                            //                                subchild.Caption = cap[0];
                            //                            }
                            //                        }
                            //                        else
                            //                        {
                            //                            int intValue = 0;
                            //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //                            CriteriaOperator criteria = CriteriaOperator.Parse(AdminFilter);
                            //                            IList<Requisition> req = ObjectSpace.GetObjects<Requisition>(criteria);
                            //                            string[] batch = new string[req.Count];
                            //                            foreach (Requisition item in req)
                            //                            {
                            //                                if (!batch.Contains(item.BatchID))
                            //                                {
                            //                                    batch[intValue] = item.BatchID;
                            //                                    intValue = intValue + 1;
                            //                                }
                            //                            }
                            //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                            //                            if (intValue > 0)
                            //                            {
                            //                                subchild.Caption = cap[0] + " (" + intValue + ")";
                            //                            }
                            //                            else
                            //                            {
                            //                                subchild.Caption = cap[0];
                            //                            }
                            //                        }

                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreason"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion
    }
}

