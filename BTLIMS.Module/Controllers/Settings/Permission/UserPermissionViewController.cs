using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class UserPermissionViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        ModificationsController modificationController;
        bool bolSave = false;
        object strViewID;
        bool boolCommit = false;
        DeleteObjectsViewController DeleteController;
        CopyPermissioninfo objCopyUserPermissionInfo = new CopyPermissioninfo();

        #region Consturctor
        public UserPermissionViewController()
        {
            InitializeComponent();
            this.TargetViewId = "UserNavigationPermission_DetailView;" + "UserNavigationPermission_ListView;" + "UserNavigationPermission_NavigationItems_ListView;" + "UserNavigationPermission_DetailView_Copy;";
            copyPermission.TargetViewId = "UserNavigationPermission_ListView";
        }
        #endregion

        #region Default Methods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                modificationController = Frame.GetController<ModificationsController>();
                DeleteController = Frame.GetController<DeleteObjectsViewController>();
                if (modificationController != null)
                {
                    modificationController.SaveAction.Execute += SaveAction_Execute;
                    modificationController.SaveAndCloseAction.Active.SetItemValue("userpermissionviewsaveclose.visible", false);
                    modificationController.SaveAndNewAction.Active.SetItemValue("userpermissionviewsavenew.visible", false);
                }
                if (DeleteController != null)
                {
                    DeleteController.DeleteAction.Executing += DeleteAction_Executing;
                }
                ObjectSpace.Committing += ObjectSpace_Committing;
                View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);
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
                if (objCopyUserPermissionInfo.BUpdated == false)
                {
                    if (View != null && View.Id == "UserNavigationPermission_NavigationItems_ListView")
                    {
                        if (((ASPxGridListEditor)((ListView)View).Editor).Grid != null)
                        {
                            boolCommit = true;
                            objCopyUserPermissionInfo.BUpdated = true;
                            ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
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
            base.OnDeactivated();
            try
            {
                if (DeleteController != null)
                {
                    DeleteController.DeleteAction.Executing -= DeleteAction_Executing;
                }
                ObjectSpace.Committing -= ObjectSpace_Committing;
                View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        #endregion

        #region Events
        private void DeleteAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.Id != null && View.SelectedObjects.Count > 0)
                {
                    foreach (UserNavigationPermission objuserpermission in View.SelectedObjects)
                    {
                        PermissionPolicyUser CurrentUser = objuserpermission.User;
                        XPCollection<PermissionPolicyRole> UserRole = CurrentUser.Roles;
                        foreach (PermissionPolicyRole Role in UserRole)
                        {
                            if (Role.Oid != null)
                            {
                                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                                SelectedData sproc = currentSession.ExecuteSproc("DeleteRole", new OperandValue(Role.Oid));
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
        private void SaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (objCopyUserPermissionInfo.DUpdated == false)
                {
                    if (View != null && View.Id == "UserNavigationPermission_DetailView")
                    {
                        UserNavigationPermission objsl = (UserNavigationPermission)View.CurrentObject;
                        if (objsl != null)
                        {
                            bolSave = false;
                            IList<UserNavigationPermissionDetails> objNP = (IList<UserNavigationPermissionDetails>)objsl.UserNavigationPermissionDetails;
                            IList<Modules.BusinessObjects.Setting.NavigationItem> objUNP = (IList<Modules.BusinessObjects.Setting.NavigationItem>)objsl.NavigationItems;
                            foreach (UserNavigationPermissionDetails objNavigate in objNP)
                            {
                                foreach (Modules.BusinessObjects.Setting.NavigationItem objUserNavigate in objUNP)
                                {
                                    if (objUserNavigate.Oid == objNavigate.NavigationItem.Oid)
                                    {
                                        objNavigate.Read = true;
                                        objNavigate.Write = objUserNavigate.Write;
                                        objNavigate.Navigate = objUserNavigate.Navigate;
                                        objNavigate.Delete = objUserNavigate.Delete;
                                        objNavigate.Create = objUserNavigate.Create;
                                        ObjectSpace.CommitChanges();
                                        if (objUserNavigate.NavigationModelClass != null)
                                        {
                                            if (XafTypesInfo.Instance.FindTypeInfo(objUserNavigate.NavigationModelClass) != null)
                                            {
                                                Type type = XafTypesInfo.Instance.FindTypeInfo(objUserNavigate.NavigationModelClass).Type;
                                                CustomSystemRole Role = ObjectSpace.FindObject<CustomSystemRole>(
                                                 new BinaryOperator("Name", objUserNavigate.NavigationId + '_' + objsl.User));
                                                if (Role == null)
                                                {
                                                    Role = ObjectSpace.CreateObject<CustomSystemRole>();
                                                    Role.Name = objUserNavigate.NavigationId + '_' + objsl.User;
                                                    Role.PermissionPolicy = SecurityPermissionPolicy.AllowAllByDefault;
                                                    Role.ISNavigationPermission = true;
                                                    Role.Users.Add(objsl.User);

                                                    PermissionPolicyTypePermissionObject myDetailsPermission = ObjectSpace.CreateObject<PermissionPolicyTypePermissionObject>();
                                                    myDetailsPermission.TargetType = type;
                                                    myDetailsPermission.Role = Role;
                                                    myDetailsPermission.ReadState = SecurityPermissionState.Allow;
                                                    myDetailsPermission.WriteState = objUserNavigate.Write ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                    myDetailsPermission.NavigateState = objUserNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                    myDetailsPermission.DeleteState = objUserNavigate.Delete ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                    myDetailsPermission.CreateState = objUserNavigate.Create ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;

                                                    if (objUserNavigate.Itempath != null)
                                                    {
                                                        PermissionPolicyNavigationPermissionObject NavigatePermission = ObjectSpace.CreateObject<PermissionPolicyNavigationPermissionObject>();
                                                        NavigatePermission.Role = Role;
                                                        NavigatePermission.ItemPath = objUserNavigate.Itempath;
                                                        NavigatePermission.NavigateState = objUserNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                        ObjectSpace.CommitChanges();
                                                    }
                                                }
                                                else
                                                {
                                                    PermissionPolicyTypePermissionObject TypePermission = ObjectSpace.FindObject<PermissionPolicyTypePermissionObject>(
                                                 new BinaryOperator("Role", Role));
                                                    if (TypePermission == null)
                                                    {
                                                        if (Role.Users.Count == 0)
                                                        {
                                                            Role.Users.Add(objsl.User);
                                                        }

                                                        PermissionPolicyTypePermissionObject myDetailsPermission = ObjectSpace.CreateObject<PermissionPolicyTypePermissionObject>();
                                                        myDetailsPermission.TargetType = type;
                                                        myDetailsPermission.Role = Role;
                                                        myDetailsPermission.ReadState = SecurityPermissionState.Allow;
                                                        myDetailsPermission.WriteState = objUserNavigate.Write ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                        myDetailsPermission.NavigateState = objUserNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                        myDetailsPermission.DeleteState = objUserNavigate.Delete ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                        myDetailsPermission.CreateState = objUserNavigate.Create ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                        ObjectSpace.CommitChanges();
                                                    }
                                                    else
                                                    {
                                                        if (Role.Users.Count == 0)
                                                        {
                                                            Role.Users.Add(objsl.User);
                                                        }
                                                        TypePermission.TargetType = type;
                                                        TypePermission.Role = Role;
                                                        TypePermission.ReadState = SecurityPermissionState.Allow;
                                                        TypePermission.WriteState = objUserNavigate.Write ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                        TypePermission.NavigateState = objUserNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                        TypePermission.DeleteState = objUserNavigate.Delete ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                        TypePermission.CreateState = objUserNavigate.Create ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                        ObjectSpace.CommitChanges();
                                                    }
                                                    CriteriaOperator criteria = null;
                                                    criteria = CriteriaOperator.Parse("Role='" + Role.Oid + "' AND ItemPath='" + objUserNavigate.Itempath + "'");
                                                    bool exists = Convert.ToBoolean(ObjectSpace.Evaluate(typeof(PermissionPolicyNavigationPermissionObject), (new AggregateOperand("", Aggregate.Exists)), (criteria)));
                                                    PermissionPolicyNavigationPermissionObject navigatePermission = ObjectSpace.FindObject<PermissionPolicyNavigationPermissionObject>(criteria);
                                                    if (exists == false && objUserNavigate.Itempath != null)
                                                    {
                                                        PermissionPolicyNavigationPermissionObject NavigatePermission = ObjectSpace.CreateObject<PermissionPolicyNavigationPermissionObject>();
                                                        NavigatePermission.Role = Role;
                                                        NavigatePermission.ItemPath = objUserNavigate.Itempath;
                                                        NavigatePermission.NavigateState = objUserNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                        ObjectSpace.CommitChanges();
                                                    }
                                                    else if (objUserNavigate.Itempath != null)
                                                    {
                                                        navigatePermission.NavigateState = objUserNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                        ObjectSpace.CommitChanges();
                                                    }
                                                }
                                                objCopyUserPermissionInfo.DUpdated = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            //boolCommit = false;
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
        private void View_CurrentObjectChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && (View.Id == "UserNavigationPermission_DetailView" || View.Id == "UserNavigationPermission_DetailView_Copy"))
                {
                    UserNavigationPermission objsl = (UserNavigationPermission)View.CurrentObject;
                    if (strViewID != View.CurrentObject && bolSave == true)
                    {
                        bolSave = false;
                    }
                    strViewID = View.CurrentObject;
                    if (objsl != null && bolSave == false)
                    {
                        bolSave = true;
                        IList<UserNavigationPermissionDetails> objNP = (IList<UserNavigationPermissionDetails>)objsl.UserNavigationPermissionDetails;
                        IList<Modules.BusinessObjects.Setting.NavigationItem> objUNP = (IList<Modules.BusinessObjects.Setting.NavigationItem>)objsl.NavigationItems;
                        foreach (UserNavigationPermissionDetails objNavigate in objNP)
                        {
                            foreach (Modules.BusinessObjects.Setting.NavigationItem objUserNavigate in objUNP)
                            {
                                if (objUserNavigate.Oid == objNavigate.NavigationItem.Oid)
                                {
                                    objUserNavigate.Read = objNavigate.Read;
                                    objUserNavigate.Write = objNavigate.Write;
                                    objUserNavigate.Navigate = objNavigate.Navigate;
                                    objUserNavigate.Delete = objNavigate.Delete;
                                    objUserNavigate.Create = objNavigate.Create;
                                    break;
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
            try
            {
                if (View != null && (View.Id == "UserNavigationPermission_DetailView" || View.Id == "UserNavigationPermission_DetailView_Copy"))
                {
                    if (View != null && View.CurrentObject != null)
                    {
                        UserNavigationPermission UserPermission = (UserNavigationPermission)View.CurrentObject;
                        IList<Modules.BusinessObjects.Setting.NavigationItem> obj1 = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.NavigationItem>();

                        if (obj1 != null)
                        {
                            if (UserPermission.NavigationItems.Count != obj1.Count)
                            {
                                foreach (Modules.BusinessObjects.Setting.NavigationItem item in obj1)
                                {
                                    if (!UserPermission.NavigationItems.Contains(item))
                                    {
                                        UserPermission.NavigationItems.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                UserNavigationPermission objsl = (UserNavigationPermission)View.CurrentObject;
                                if (strViewID != View.CurrentObject && bolSave == true)
                                {
                                    bolSave = false;
                                }
                                strViewID = View.CurrentObject;
                                if (objsl != null && bolSave == false)
                                {
                                    bolSave = true;
                                    IList<UserNavigationPermissionDetails> objNP = (IList<UserNavigationPermissionDetails>)objsl.UserNavigationPermissionDetails;
                                    IList<Modules.BusinessObjects.Setting.NavigationItem> objUNP = (IList<Modules.BusinessObjects.Setting.NavigationItem>)objsl.NavigationItems;
                                    foreach (UserNavigationPermissionDetails objNavigate in objNP)
                                    {
                                        foreach (Modules.BusinessObjects.Setting.NavigationItem objUserNavigate in objUNP)
                                        {
                                            if (objUserNavigate.Oid == objNavigate.NavigationItem.Oid)
                                            {
                                                objUserNavigate.Read = objNavigate.Read;
                                                objUserNavigate.Write = objNavigate.Write;
                                                objUserNavigate.Navigate = objNavigate.Navigate;
                                                objUserNavigate.Delete = objNavigate.Delete;
                                                objUserNavigate.Create = objNavigate.Create;
                                                break;

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (View is DetailView && View.Id == "UserNavigationPermission_DetailView")
                    {
                        ((DetailView)View).FindItem("NavigationItems").Refresh();
                        UserNavigationPermission UserPermission = (UserNavigationPermission)View.CurrentObject;
                        if (UserPermission.User != null)
                            ObjectSpace.Refresh();
                    }
                }
                else if (View != null && View.Id == "UserNavigationPermission_NavigationItems_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion



        private void copyPermission_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject != null)
                {
                    UserNavigationPermission obj = (UserNavigationPermission)View.CurrentObject;
                    if (obj != null && obj.User != null)
                    {
                        objCopyUserPermissionInfo.SelectedUser = obj.User;
                    }
                }
                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(CopyPermission));
                if (objToShow != null)
                {
                    DetailView CreateDetailView = Application.CreateDetailView(objspace, objToShow);
                    CreateDetailView.ViewEditMode = ViewEditMode.Edit;
                    e.View = CreateDetailView;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopyPermission_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject != null && View.ObjectTypeInfo.Type == typeof(UserNavigationPermission))
                {

                    if (View.SelectedObjects != null && View.SelectedObjects.Count == 1 && objCopyUserPermissionInfo.lstSelectedUser.Count > 0)
                    {
                        foreach (var User in objCopyUserPermissionInfo.lstSelectedUser)
                        {
                            CriteriaOperator criteriaPermission = CriteriaOperator.Parse("[UserName]='" + User + "'");
                            var oss = Application.CreateObjectSpace();
                            PermissionPolicyUser lstpermission = oss.FindObject<PermissionPolicyUser>(criteriaPermission);


                            CriteriaOperator criteriaUser = CriteriaOperator.Parse("[User]='" + lstpermission.Oid + "'");
                            var os = Application.CreateObjectSpace();
                            var lst = os.FindObject<UserNavigationPermission>(criteriaUser);
                            if (lst == null)
                            {
                                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                                UserNavigationPermission objNPOld = (UserNavigationPermission)View.CurrentObject;
                                CriteriaOperator criteriaUserPermission = CriteriaOperator.Parse("[UserNavigationPermission]='" + objNPOld.Oid + "'");
                                IList<UserNavigationPermissionDetails> lstUserpermission = ObjectSpace.GetObjects<UserNavigationPermissionDetails>(criteriaUserPermission).ToList();
                                UserNavigationPermission objnew = ObjectSpace.CreateObject<UserNavigationPermission>();
                                if (objNPOld != null)
                                {
                                    objnew.User = ObjectSpace.GetObjectByKey<CustomSystemUser>(lstpermission.Oid);
                                    //IList<UserNavigationPermissionDetails> objNP = (IList<UserNavigationPermissionDetails>)lstUserpermission.ToList();
                                    foreach (UserNavigationPermissionDetails item in lstUserpermission)
                                    {
                                        UserNavigationPermissionDetails objnewPermissionDeail = ObjectSpace.CreateObject<UserNavigationPermissionDetails>();

                                        //objnewPermissionDeail.UserNavigationPermission = objnew;
                                        //objnewPermissionDeail.NavigationItem = item.NavigationItem;
                                        objnewPermissionDeail.Read = item.Read;
                                        objnewPermissionDeail.Write = item.Write;
                                        objnewPermissionDeail.Create = item.Create;
                                        objnewPermissionDeail.Navigate = item.Navigate;
                                        objnewPermissionDeail.Delete = item.Delete;
                                        ObjectSpace.CommitChanges();
                                        //objnew.UserNavigationPermissionDetails.Add(ObjectSpace.GetObjectByKey<UserNavigationPermissionDetails>(item.Oid));
                                    }
                                }
                                if (objnew != null)
                                {
                                    bolSave = false;
                                    IList<UserNavigationPermissionDetails> objNP = (IList<UserNavigationPermissionDetails>)objnew.UserNavigationPermissionDetails;
                                    IList<Modules.BusinessObjects.Setting.NavigationItem> objUNP = (IList<Modules.BusinessObjects.Setting.NavigationItem>)objnew.NavigationItems;
                                    foreach (UserNavigationPermissionDetails objNavigate in objNP)
                                    {
                                        foreach (Modules.BusinessObjects.Setting.NavigationItem objUserNavigate in objUNP)
                                        {
                                            if (objUserNavigate.Oid == objNavigate.NavigationItem.Oid)
                                            {
                                                //objNavigate.Read = true;
                                                //objNavigate.Write = objUserNavigate.Write;
                                                //objNavigate.Navigate = objUserNavigate.Navigate;
                                                //objNavigate.Delete = objUserNavigate.Delete;
                                                //objNavigate.Create = objUserNavigate.Create;
                                                //ObjectSpace.CommitChanges();
                                                if (objUserNavigate.NavigationModelClass != null)
                                                {
                                                    if (XafTypesInfo.Instance.FindTypeInfo(objUserNavigate.NavigationModelClass) != null)
                                                    {
                                                        Type type = XafTypesInfo.Instance.FindTypeInfo(objUserNavigate.NavigationModelClass).Type;
                                                        CustomSystemRole Role = ObjectSpace.FindObject<CustomSystemRole>(
                                                         new BinaryOperator("Name", objUserNavigate.NavigationId + '_' + objnew.User));
                                                        if (Role == null)
                                                        {
                                                            Role = ObjectSpace.CreateObject<CustomSystemRole>();
                                                            Role.Name = objUserNavigate.NavigationId + '_' + objnew.User;
                                                            Role.PermissionPolicy = SecurityPermissionPolicy.AllowAllByDefault;
                                                            Role.ISNavigationPermission = true;
                                                            Role.Users.Add(objnew.User);

                                                            PermissionPolicyTypePermissionObject myDetailsPermission = ObjectSpace.CreateObject<PermissionPolicyTypePermissionObject>();
                                                            myDetailsPermission.TargetType = type;
                                                            myDetailsPermission.Role = Role;
                                                            myDetailsPermission.ReadState = SecurityPermissionState.Allow;
                                                            myDetailsPermission.WriteState = objUserNavigate.Write ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                            myDetailsPermission.NavigateState = objUserNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                            myDetailsPermission.DeleteState = objUserNavigate.Delete ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                            myDetailsPermission.CreateState = objUserNavigate.Create ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;

                                                            if (objUserNavigate.Itempath != null)
                                                            {
                                                                PermissionPolicyNavigationPermissionObject NavigatePermission = ObjectSpace.CreateObject<PermissionPolicyNavigationPermissionObject>();
                                                                NavigatePermission.Role = Role;
                                                                NavigatePermission.ItemPath = objUserNavigate.Itempath;
                                                                NavigatePermission.NavigateState = objUserNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                                ObjectSpace.CommitChanges();
                                                            }

                                                        }
                                                        else
                                                        {
                                                            PermissionPolicyTypePermissionObject TypePermission = ObjectSpace.FindObject<PermissionPolicyTypePermissionObject>(
                                                         new BinaryOperator("Role", Role));
                                                            if (TypePermission == null)
                                                            {
                                                                if (Role.Users.Count == 0)
                                                                {
                                                                    Role.Users.Add(objnew.User);
                                                                }

                                                                PermissionPolicyTypePermissionObject myDetailsPermission = ObjectSpace.CreateObject<PermissionPolicyTypePermissionObject>();
                                                                myDetailsPermission.TargetType = type;
                                                                myDetailsPermission.Role = Role;
                                                                myDetailsPermission.ReadState = SecurityPermissionState.Allow;
                                                                myDetailsPermission.WriteState = objUserNavigate.Write ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                                myDetailsPermission.NavigateState = objUserNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                                myDetailsPermission.DeleteState = objUserNavigate.Delete ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                                myDetailsPermission.CreateState = objUserNavigate.Create ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                                ObjectSpace.CommitChanges();
                                                            }
                                                            else
                                                            {
                                                                if (Role.Users.Count == 0)
                                                                {
                                                                    Role.Users.Add(objnew.User);
                                                                }
                                                                TypePermission.TargetType = type;
                                                                TypePermission.Role = Role;
                                                                TypePermission.ReadState = SecurityPermissionState.Allow;
                                                                TypePermission.WriteState = objUserNavigate.Write ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                                TypePermission.NavigateState = objUserNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                                TypePermission.DeleteState = objUserNavigate.Delete ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                                TypePermission.CreateState = objUserNavigate.Create ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                                ObjectSpace.CommitChanges();
                                                            }
                                                            CriteriaOperator criteria = null;
                                                            criteria = CriteriaOperator.Parse("Role='" + Role.Oid + "' AND ItemPath='" + objUserNavigate.Itempath + "'");
                                                            bool exists = Convert.ToBoolean(ObjectSpace.Evaluate(typeof(PermissionPolicyNavigationPermissionObject), (new AggregateOperand("", Aggregate.Exists)), (criteria)));
                                                            PermissionPolicyNavigationPermissionObject navigatePermission = ObjectSpace.FindObject<PermissionPolicyNavigationPermissionObject>(criteria);
                                                            if (exists == false && objUserNavigate.Itempath != null)
                                                            {
                                                                PermissionPolicyNavigationPermissionObject NavigatePermission = ObjectSpace.CreateObject<PermissionPolicyNavigationPermissionObject>();
                                                                NavigatePermission.Role = Role;
                                                                NavigatePermission.ItemPath = objUserNavigate.Itempath;
                                                                NavigatePermission.NavigateState = objUserNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                                ObjectSpace.CommitChanges();
                                                            }
                                                            else if (objUserNavigate.Itempath != null)
                                                            {
                                                                navigatePermission.NavigateState = objUserNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                                                                ObjectSpace.CommitChanges();
                                                            }
                                                        }
                                                        break;
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    ObjectSpace.Refresh();
                                }
                            }

                            else
                            {

                            }

                        }
                    }
                    else if (View.SelectedObjects.Count == 0)
                    {

                    }
                    else if (View.SelectedObjects.Count > 1)
                    {

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
