using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Login;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class EmployeeController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        bool IsAdministrator=false;
        public EmployeeController()
        {
            InitializeComponent();
            TargetViewId = "Employee_DetailView;" + "PermissionPolicyUser_Roles_ListView;" + "Employee_ListView;"+ "LoginOneTimePin_DetailView;" + "Employee_ListView_EmailProfile;";
            linkRoleToEmployeeAction.TargetViewId = "PermissionPolicyUser_Roles_ListView;";
            SetPassword.TargetViewId = "Employee_DetailView;";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View.Id == "Employee_DetailView")
                {
                    Employee objEmp = (Employee)View.CurrentObject;
                    Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                    Frame.GetController<ResetPasswordController>().ResetPasswordAction.Active.SetItemValue("valReset",false);
                    Frame.GetController<ChangePasswordController>().ChangeMyPasswordAction.CustomizePopupWindowParams += ChangeMyPasswordAction_CustomizePopupWindowParams;
                    Frame.GetController<ChangePasswordController>().ChangeMyPasswordAction.Executed += ChangeMyPasswordAction_Executed;
                    if (user.Roles != null && user.Roles.Count > 0)
                    {
                        if (user.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            if (objEmp.IsPasswordNotSet)
                    {
                        SetPassword.Enabled.SetItemValue("valSetPassword", true);
                                SetPassword.Caption = "Set Password";
                                SetPassword.ToolTip = "Set Password";
                            }
                            else
                            {
                                SetPassword.Enabled.SetItemValue("valSetPassword", true);
                                SetPassword.Caption = "Reset Password";
                                SetPassword.ToolTip = "Reset Password";
                            }
                            IsAdministrator = true;
                    }
                    else
                    {
                        if(objEmp.IsPasswordNotSet)
                        {
                                SetPassword.Enabled.SetItemValue("valSetPassword", false);
                                SetPassword.Caption = "Set Password";
                                SetPassword.ToolTip = "Set Password";
                        }
                        else
                        {
                            SetPassword.Enabled.SetItemValue("valSetPassword", false);
                                SetPassword.Caption = "Reset Password";
                                SetPassword.ToolTip = "Reset Password";
                            }
                        }
                        IsAdministrator = false;
                    }
                    View.ObjectSpace.Committed += ObjectSpace_Committed;
                    View.ObjectSpace.Committing += ObjectSpace_Committing;
                    Frame.GetController<ModificationsController>().SaveAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAction.Executed += SaveAction_Executed;
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated += ChangeLayoutGroupCaptionViewController_ItemCreated;
                }
                else
                if (View.Id == "Employee_RolePermissions_ListView;")
                {
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Executing += UnlinkAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                }
                else
                if (View.Id == "Employee_ListView")
                {//[RolePermissions][[RoleNavigationPermissionDetails][EndsWith([NavigationItem.NavigationView], 'ListView')]]
                    Employee curUser = (Employee)SecuritySystem.CurrentUser;
                    if (curUser.RolePermissions.FirstOrDefault(i => i.IsAdministrative == true) == null && curUser.RolePermissions.FirstOrDefault(i => i.AdministrativePrivilege == AdministrativePrivilege.ClientAdministrator) != null)
                    {
                        IList<Employee> lstAdministrativeUsers = ObjectSpace.GetObjects<Employee>(CriteriaOperator.Parse("[Roles][[IsAdministrative] = True]"));
                        if (lstAdministrativeUsers != null)
                        {
                            string strCriteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", lstAdministrativeUsers.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(strCriteria);
                        }
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[UserName] = CurrentUserName() And IsAdministrative() = '0' Or IsAdministrative() = '1'");
                    }
                }
                else if (View != null && View.Id == "Employee_ListView_EmailProfile")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    ListViewController listViewController = Frame.GetController<ListViewController>();
                    if (listViewController != null)
                    {
                        listViewController.EditAction.Active["HideEditActionController"] = false;
                    }

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
                Employee  objEmp1 = (Employee )e.Object;
                if (e.PropertyName == "SampleRegistrationDefault")
                {

                    Employee Empobj = View.ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[SampleRegistrationDefault] = True And [Oid]<>?", objEmp1.Oid));
                    if ( Empobj != null && Empobj.SampleRegistrationDefault)
                    {
                        Application.ShowViewStrategy.ShowMessage("Default has been already enabled.", InformationType.Info , timer.Seconds, InformationPosition.Top);
                        objEmp1.SampleRegistrationDefault = false; 

                    }
                }
                else if (e.PropertyName == "ReportDeliveryDefault")
                {
                    Employee Empobj = View.ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[ReportDeliveryDefault] = True And [Oid]<>?", objEmp1.Oid));
                    if (Empobj != null && Empobj.ReportDeliveryDefault)
                    {
                        Application.ShowViewStrategy.ShowMessage("Default has been already enabled.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                        objEmp1.ReportDeliveryDefault = false;

                    }
                }
                else if (e.PropertyName == "InvoiceDeliveryDefault")
                {
                    Employee Empobj = View.ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[InvoiceDeliveryDefault] = True And [Oid]<>?", objEmp1.Oid));
                    if (Empobj != null && Empobj.InvoiceDeliveryDefault)
                    {
                        Application.ShowViewStrategy.ShowMessage("Default has been already enabled.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                        objEmp1.InvoiceDeliveryDefault = false;

                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ChangeMyPasswordAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                Application.ShowViewStrategy.ShowMessage("Password changed successfully.", InformationType.Success, 3000, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ChangeMyPasswordAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                e.DialogController.Accepting += DialogController_Accepting;
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
                ChangePasswordParameters obj = (ChangePasswordParameters)e.AcceptActionArgs.CurrentObject;
                Employee objEmp = (Employee)Application.MainWindow.View.CurrentObject;
                if(obj!=null && objEmp!=null)
                {
                    if(string.IsNullOrEmpty(obj.OldPassword) && !objEmp.ComparePassword(obj.OldPassword))
                    {
                        Application.ShowViewStrategy.ShowMessage("Enter the old password.", InformationType.Error, 3000, InformationPosition.Top);
                        e.Cancel = true;
                        return;
                    }
                    if(!objEmp.ComparePassword(obj.OldPassword))
                    {
                        Application.ShowViewStrategy.ShowMessage("Old password is wrong.", InformationType.Error, 3000, InformationPosition.Top);
                        e.Cancel = true;
                        return;
                    }
                    if (string.IsNullOrEmpty(obj.NewPassword))
                    {
                        Application.ShowViewStrategy.ShowMessage("Enter the new password.", InformationType.Error, 3000, InformationPosition.Top);
                        e.Cancel = true;
                        return;
                    }
                    if (string.IsNullOrEmpty(obj.ConfirmPassword))
                    {
                        Application.ShowViewStrategy.ShowMessage("Enter the confirm password.", InformationType.Error, 3000, InformationPosition.Top);
                        e.Cancel = true;
                        return;
                    }
                    if (obj.ConfirmPassword!=obj.NewPassword)
                    {
                        Application.ShowViewStrategy.ShowMessage("New password and confirm password not match.", InformationType.Error, 3000, InformationPosition.Top);
                        e.Cancel = true;
                        return;
                    }
                    if (objEmp.ComparePassword(obj.NewPassword))
                    {
                        Application.ShowViewStrategy.ShowMessage("Old and new password cannot be same.", InformationType.Error, 3000, InformationPosition.Top);
                        e.Cancel = true;
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
        private void ChangeLayoutGroupCaptionViewController_ItemCreated(object sender, ItemCreatedEventArgs e)
        {
            try
            {
                if (e.ViewItem is PropertyEditor && (((PropertyEditor)e.ViewItem).PropertyName == "SetPassword"))
                {
                    e.TemplateContainer.Load += (s, args) =>
                    {
                        if (e.TemplateContainer.CaptionControl != null)
                        {
                            e.TemplateContainer.CaptionControl.ForeColor = Color.Red;
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
           try
            {
                Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                if (user.Roles != null && user.Roles.Count > 0)
                {
                    if (user.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        if (user.IsPasswordNotSet)
                        {
                            SetPassword.Enabled.SetItemValue("valSetPassword", true);
                            SetPassword.Caption = "Set Password";
                            SetPassword.ToolTip = "Set Password";
                        }
                        else
                        {
                            SetPassword.Enabled.SetItemValue("valSetPassword", true);
                            SetPassword.Caption = "Reset Password";
                            SetPassword.ToolTip = "Reset Password";
                        }
                    }
                    else
                    {
                        if (user.IsPasswordNotSet)
                        {
                            SetPassword.Enabled.SetItemValue("valSetPassword", false);
                            SetPassword.Caption = "Set Password";
                            SetPassword.ToolTip = "Set Password";
                        }
                        else
                        {
                SetPassword.Enabled.SetItemValue("valSetPassword", false);
                            SetPassword.Caption = "Reset Password";
                            SetPassword.ToolTip = "Reset Password";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Accepting1(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                DialogController dc = sender as DialogController;
                if (dc!=null && dc.Window!=null && dc.Window.View!=null)
                {
                    LoginOneTimePin obj = (LoginOneTimePin)dc.Window.View.CurrentObject;
                    Employee objEmp = (Employee)Application.MainWindow.View.CurrentObject;
                    if (obj != null && objEmp != null)
                    {
                        if (string.IsNullOrEmpty(obj.NewPassword))
                        {
                            Application.ShowViewStrategy.ShowMessage("Enter the new password.", InformationType.Error, 3000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                        else if (string.IsNullOrEmpty(obj.ConfirmPassword))
                        {
                            Application.ShowViewStrategy.ShowMessage("Enter the confirm password.", InformationType.Error, 3000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                        else if (obj.NewPassword != obj.ConfirmPassword)
                        {
                            Application.ShowViewStrategy.ShowMessage("New password and confirm password not match.", InformationType.Error, 3000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                        objEmp.SetPassword(obj.NewPassword);
                    } 
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
                if (View.CurrentObject != null)
                {
                    Employee objEmp = (Employee)View.CurrentObject;
                    if(objEmp!=null && View.ObjectSpace.IsNewObject(objEmp))
                    {
                        if(objEmp.IsPasswordNotSet && !string.IsNullOrEmpty(objEmp.FirstName) && !string.IsNullOrEmpty(objEmp.LastName)&& !string.IsNullOrEmpty(objEmp.DisplayName) )
                        {
                            Application.ShowViewStrategy.ShowMessage("Set the password.", InformationType.Error, 3000, InformationPosition.Top);
                            e.Cancel = true;
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

        private void ObjectSpace_Committing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null)
                {
                    Employee objEmp = (Employee)View.CurrentObject;
                    if (objEmp.UserID == null || objEmp.UserID == 0)
                    {
                        IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Employee));
                        CriteriaOperator userId = CriteriaOperator.Parse("Max(UserID)");
                        int NewItemCode = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Employee), userId, null)) + 1);
                        objEmp.UserID = NewItemCode;
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
                if (Frame is NestedFrame)
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                    {
                        CompositeView cv = (CompositeView)nestedFrame.ViewItem.View;
                        if (cv != null && cv.CurrentObject != null)
                        {
                            Employee emp = (Employee)cv.CurrentObject;
                            if (emp != null)
                            {
                                foreach (RoleNavigationPermission permission in emp.RolePermissions)
                                {
                                    foreach (PermissionPolicyRole role in emp.Roles)
                                    {
                                        CustomSystemRole customSystemRole = cv.ObjectSpace.GetObjectByKey<CustomSystemRole>(role.Oid);
                                        if (customSystemRole != null && customSystemRole.RoleNavigationPermission != null && customSystemRole.RoleNavigationPermission.Oid == permission.Oid)
                                        {
                                            emp.Roles.Remove(role);
                                        }
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

        private void UnlinkAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (Frame is NestedFrame)
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                    {
                        CompositeView cv = (CompositeView)nestedFrame.ViewItem.View;
                        if (cv != null && cv.CurrentObject != null)
                        {
                            Employee emp = (Employee)cv.CurrentObject;
                            if (emp != null)
                            {
                                foreach (RoleNavigationPermission permission in emp.RolePermissions)
                                {
                                    foreach (PermissionPolicyRole role in emp.Roles)
                                    {
                                        CustomSystemRole customSystemRole = cv.ObjectSpace.GetObjectByKey<CustomSystemRole>(role.Oid);
                                        if (customSystemRole != null && customSystemRole.RoleNavigationPermission != null && customSystemRole.RoleNavigationPermission.Oid == permission.Oid)
                                        {
                                            emp.Roles.Remove(role);
                                        }
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

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "Employee_DetailView" && View.CurrentObject != null)
                {
                    Employee objEmployee = (Employee)View.CurrentObject;
                    if (objEmployee != null && objEmployee.RolePermissions.Count > 0)
                    {
                        foreach (RoleNavigationPermission objRolePermission in objEmployee.RolePermissions)
                        {
                            AssignPermissionToUser(objEmployee, objRolePermission);
                        }

                        //if (objEmployee.Roles.Count > 0 && objEmployee.Roles.FirstOrDefault(i => i.IsAdministrative == true) == null)
                        if (objEmployee.Roles.Count > 0)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            objEmployee = os.GetObject<Employee>(objEmployee);
                            string strPermissionCriteria = "Not [RoleNavigationPermission.Oid] In(" + string.Format("'{0}'", string.Join("','", objEmployee.RolePermissions.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                            string strRolesCriteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", objEmployee.Roles.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";


                            GroupOperator criteria = new GroupOperator(GroupOperatorType.And,
                                //CriteriaOperator.Parse(strRolesCriteria),
                                new InOperator("Oid", objEmployee.Roles.Select(i => i.Oid)),
                                new GroupOperator(GroupOperatorType.Or,
                                CriteriaOperator.Parse("[RoleNavigationPermission] Is Null"),
                                //CriteriaOperator.Parse(strPermissionCriteria))
                                new NotOperator(new InOperator("RoleNavigationPermission.Oid", objEmployee.RolePermissions.Select(i => i.Oid))))
                                );

                            IList<CustomSystemRole> lstRoles = os.GetObjects<CustomSystemRole>(criteria);
                            if (lstRoles != null && lstRoles.Count > 0)
                            {
                                foreach (CustomSystemRole role in lstRoles.ToList())
                                {
                                    if (objEmployee.UserName != "Administrator" && role.Name != "Administrator" || (objEmployee.UserName == "Administrator" && role.Name != "Administrator"))
                                    {
                                        objEmployee.Roles.Remove(os.GetObjectByKey<PermissionPolicyRole>(role.Oid));
                                    }
                                    //os.Delete(role);
                                }
                                os.CommitChanges();
                            }
                            //os.Dispose();
                        }
                    }
                    else if (objEmployee.Roles.Count > 0)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        objEmployee = os.GetObject<Employee>(objEmployee);
                        if (objEmployee != null)
                        {
                            foreach (CustomSystemRole role in objEmployee.Roles.ToList())
                            {
                                if (objEmployee.UserName != "Administrator" || (objEmployee.UserName == "Administrator" && role.Name != "Administrator"))
                                {
                                    objEmployee.Roles.Remove(os.GetObjectByKey<PermissionPolicyRole>(role.Oid));
                                    //os.Delete(role); 
                                }
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

        public void AssignPermissionToUser(Employee objEmployee, RoleNavigationPermission objRolePermission)
        {
            try
            {
                if (objRolePermission.RoleNavigationPermissionDetails.Count > 0)
                {
                    foreach (RoleNavigationPermissionDetails permission in objRolePermission.RoleNavigationPermissionDetails)
                    {
                        if (permission.NavigationItem != null)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            CustomSystemRole objRole = os.FindObject<CustomSystemRole>(
                                         new BinaryOperator("Name", permission.NavigationItem.NavigationId + '_' + objEmployee.UserName));
                            if (objRole == null)
                            {
                                objRole = os.CreateObject<CustomSystemRole>();
                                objRole.Name = permission.NavigationItem.NavigationId + '_' + objEmployee.UserName;
                                objRole.PermissionPolicy = SecurityPermissionPolicy.AllowAllByDefault;
                                objRole.ISNavigationPermission = true;
                                objRole.IsAdministrative = objRolePermission.IsAdministrative;
                                objRole.RoleNavigationPermission = os.GetObject<RoleNavigationPermission>(permission.RoleNavigationPermission);
                                objRole.Users.Add(os.GetObject<Employee>(objEmployee));
                            }
                            else
                            {
                                objRole.PermissionPolicy = SecurityPermissionPolicy.AllowAllByDefault;
                                objRole.ISNavigationPermission = true;
                                objRole.IsAdministrative = objRolePermission.IsAdministrative;
                                objRole.RoleNavigationPermission = os.GetObject<RoleNavigationPermission>(permission.RoleNavigationPermission);
                                objRole.Users.Add(os.GetObject<Employee>(objEmployee));
                            }
                            os.CommitChanges();
                            //os.Dispose();

                            if (!string.IsNullOrEmpty(permission.NavigationItem.NavigationModelClass))
                            {
                                AssignObjectPermission(permission, permission.NavigationItem.NavigationModelClass, objRole, false);
                            }

                            if (permission.NavigationItem.LinkedClasses.Count > 0)
                            {
                                foreach (LinkedClasses objLinkedClass in permission.NavigationItem.LinkedClasses)
                                {
                                    if (!string.IsNullOrEmpty(objLinkedClass.ClassName))
                                    {
                                        AssignObjectPermission(permission, objLinkedClass.ClassName, objRole, true);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (objRolePermission.IsAdministrative == true && objRolePermission.RoleNavigationPermissionDetails.Count == 0)
                {
                    //IObjectSpace os = Application.CreateObjectSpace();
                    //Employee objUser = os.GetObject<Employee>(objEmployee);
                    //PermissionPolicyRole role = os.FindObject<PermissionPolicyRole>(CriteriaOperator.Parse("[Name] = 'Administrator'"));
                    //if (objUser != null && role != null)
                    //{
                    //    objUser.Roles.Add(role);
                    //    os.CommitChanges();
                    //}
                    //os.Dispose();
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    Employee objUser = uow.GetObjectByKey<Employee>(objEmployee.Oid);
                    PermissionPolicyRole role = uow.FindObject<PermissionPolicyRole>(CriteriaOperator.Parse("[Name] = 'Administrator'"));
                    if (objUser != null && role != null && !objUser.Roles.Contains(role))
                    {
                        objUser.Roles.Add(role);
                        uow.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AssignObjectPermission(RoleNavigationPermissionDetails objNavigate, string navigationModelClass, CustomSystemRole role, bool isLinked)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                CustomSystemRole currentRole = os.GetObject<CustomSystemRole>(role);
                if (isLinked)
                {
                    Modules.BusinessObjects.Setting.NavigationItem navigationItem = os.GetObjectByKey<Modules.BusinessObjects.Setting.NavigationItem>(new Guid(navigationModelClass));
                    navigationModelClass = navigationItem.NavigationModelClass;
                }
                if (!string.IsNullOrEmpty(navigationModelClass) && XafTypesInfo.Instance.FindTypeInfo(navigationModelClass) != null)
                {
                    Type type = XafTypesInfo.Instance.FindTypeInfo(navigationModelClass).Type;
                    IList<PermissionPolicyTypePermissionObject> lstPermissions = os.GetObjects<PermissionPolicyTypePermissionObject>(CriteriaOperator.Parse("[Role.Oid] = ?", role.Oid));
                    PermissionPolicyTypePermissionObject TypePermission = null;
                    if (lstPermissions != null && lstPermissions.Count > 0)
                    {
                        TypePermission = lstPermissions.FirstOrDefault<PermissionPolicyTypePermissionObject>(i => i.TargetType != null && i.TargetType.FullName == type.FullName);
                    }

                    if (TypePermission == null)
                    {
                        TypePermission = os.CreateObject<PermissionPolicyTypePermissionObject>();
                        TypePermission.TargetType = type;
                        TypePermission.Role = currentRole;
                        TypePermission.ReadState = SecurityPermissionState.Allow;
                        TypePermission.WriteState = objNavigate.Write ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                        TypePermission.NavigateState = objNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                        TypePermission.DeleteState = objNavigate.Delete ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                        TypePermission.CreateState = objNavigate.Create ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                    }
                    else
                    {
                        TypePermission.ReadState = SecurityPermissionState.Allow;
                        TypePermission.WriteState = objNavigate.Write ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                        TypePermission.NavigateState = objNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                        TypePermission.DeleteState = objNavigate.Delete ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                        TypePermission.CreateState = objNavigate.Create ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                    }

                    if (!string.IsNullOrEmpty(objNavigate.NavigationItem.Itempath))
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("Role='" + currentRole.Oid + "' AND ItemPath='" + objNavigate.NavigationItem.Itempath + "'");
                        PermissionPolicyNavigationPermissionObject navigatePermission = os.FindObject<PermissionPolicyNavigationPermissionObject>(criteria);
                        if (navigatePermission == null)
                        {
                            navigatePermission = os.CreateObject<PermissionPolicyNavigationPermissionObject>();
                            navigatePermission.Role = currentRole;
                            navigatePermission.ItemPath = objNavigate.NavigationItem.Itempath;
                            navigatePermission.NavigateState = objNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                        }
                        else
                        {
                            navigatePermission.NavigateState = objNavigate.Navigate ? SecurityPermissionState.Allow : SecurityPermissionState.Deny;
                        }
                    }
                    os.CommitChanges();
                    //os.Dispose();
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
                if (View.Id == "Employee_DetailView")
                {
                    if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                    {
                        SetPassword.Enabled.SetItemValue("valSetPassword", false);
                    }
                    else
                    {
                        SetPassword.Enabled.SetItemValue("valSetPassword", true);
                    }
                }
                else if (View.Id == "Employee_ListView_EmailProfile")
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null)
                    gridlisteditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("EmailProfile", this);
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
            if (View.Id == "Employee_ListView_EmailProfile")
            {
                if (e.DataColumn.FieldName == "Default")
                {
                    e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'EmailProfile', '{0}|{1}' , '', false)", e.CellValue , e.VisibleIndex));
                }
            }
         }

        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();
                if (View.Id == "Employee_DetailView")
                {
                    View.ObjectSpace.Committed -= ObjectSpace_Committed;
                    View.ObjectSpace.Committing -= ObjectSpace_Committing;
                    Frame.GetController<ModificationsController>().SaveAction.Executing -= SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAction.Executed -= SaveAction_Executed;
                    Frame.GetController<ChangePasswordController>().ChangeMyPasswordAction.CustomizePopupWindowParams -= ChangeMyPasswordAction_CustomizePopupWindowParams;
                    Frame.GetController<ChangePasswordController>().ChangeMyPasswordAction.Executed -= ChangeMyPasswordAction_Executed;
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated -= ChangeLayoutGroupCaptionViewController_ItemCreated;
                }
                else
                if (View.Id == "Employee_RolePermissions_ListView;")
                {
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Executing -= UnlinkAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                }
                else if (View != null && View.Id == "Employee_ListView_EmailProfile")
                {
                    ListViewController listViewController = Frame.GetController<ListViewController>();
                    if (listViewController != null)
                    {
                        listViewController.EditAction.Active.RemoveItem("HideEditActionController");
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void linkRoleToEmployeeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "PermissionPolicyUser_Roles_ListView")
                {
                    string strCriteria = string.Empty;
                    if (Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (nestedFrame != null && nestedFrame.ViewItem != null)
                        {
                            CompositeView cv = nestedFrame.ViewItem.View;
                            if (cv != null && cv.Id == "Employee_DetailView" && cv.CurrentObject != null)
                            {
                                Employee emp = (Employee)cv.CurrentObject;
                                if (emp != null && emp.Roles != null && emp.Roles.Count > 0)
                                {
                                    strCriteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", emp.Roles.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                }
                            }
                        }
                    }
                    CollectionSource cs = new CollectionSource(Application.CreateObjectSpace(), typeof(CustomSystemRole));
                    if (!string.IsNullOrEmpty(strCriteria))
                    {
                        cs.Criteria["IgnoreExistingRoles"] = CriteriaOperator.Parse(strCriteria);
                    }
                    ListView createdListView = Application.CreateListView("CustomSystemRole_ListView_Employee", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters();
                    showViewParameters.CreatedView = createdListView;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += Dc_Accepting;
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

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (View != null && e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    if (Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (nestedFrame != null && nestedFrame.ViewItem != null)
                        {
                            CompositeView cv = nestedFrame.ViewItem.View;
                            if (cv != null && cv.Id == "Employee_DetailView" && cv.CurrentObject != null)
                            {
                                Employee emp = (Employee)cv.CurrentObject;
                                if (emp != null)
                                {
                                    foreach (CustomSystemRole systemRole in e.AcceptActionArgs.SelectedObjects)
                                    {
                                        PermissionPolicyRole role = cv.ObjectSpace.GetObjectByKey<PermissionPolicyRole>(systemRole.Oid);
                                        if (role != null && !emp.Roles.Contains(role))
                                        {
                                            emp.Roles.Add(role);

                                        }

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
        private void SetPassword_Excecute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                NonPersistentObjectSpace os = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(LoginOneTimePin));
                LoginOneTimePin obj = (LoginOneTimePin)os.CreateObject(typeof(LoginOneTimePin));
                DetailView CreatedDetailView = Application.CreateDetailView(os, obj);
                CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                showViewParameters.CreatedView.Caption =SetPassword.Caption;
                dc.Accepting += Dc_Accepting1;
                dc.CloseOnCurrentObjectProcessing = false;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void ProcessAction(string parameter)
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
    }
}
