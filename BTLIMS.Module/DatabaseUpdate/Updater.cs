using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using System;

namespace BTLIMS.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            ////     DashboardsModule.AddDashboardData<DashboardData>(
            ////ObjectSpace, "My Dashboard", Resources.MyDashboard);
            ////     DashboardsModule.AddDashboardData<DashboardData>(
            ////ObjectSpace, "My Dashboard1", Resources.Dashboard1);
            ////初始化角色
            //CustomSystemRole administrativeRole = ObjectSpace.FindObject<CustomSystemRole>(
            //    new BinaryOperator("Name", SecurityStrategy.AdministratorRoleName));
            //if (administrativeRole == null)
            //{
            //    administrativeRole = ObjectSpace.CreateObject<CustomSystemRole>();
            //    administrativeRole.Name = SecurityStrategy.AdministratorRoleName;
            //    administrativeRole.IsAdministrative = true;
            //}
            ////初始化用户
            //const string adminName = "Administrator";
            //Employee administratorUser = ObjectSpace.FindObject<Employee>(
            //    new BinaryOperator("UserName", adminName));
            //if (administratorUser == null)
            //{
            //    administratorUser = ObjectSpace.CreateObject<Employee>();
            //    administratorUser.UserName = adminName;
            //    administratorUser.IsActive = true;
            //    administratorUser.SetPassword("");
            //    administratorUser.FirstName = "Sam";
            //    administratorUser.LastName = "Peterson";
            //    administratorUser.Roles.Add(administrativeRole);
            //}
            ////初始化公司
            //if (ObjectSpace.FindObject<Company>(null) == null)
            //{
            //    Company company1 = ObjectSpace.CreateObject<Company>();
            //    company1.CompanyName = "BTLIMS";
            //    company1.Language = Language.Chinese;
            //    company1.IsValid = true;
            //    company1.Employees.Add(administratorUser);
            //    administrativeRole.Company = company1;
            //    administratorUser.Company = company1;
            //}
            //if (ObjectSpace.FindObject<QCType>(null) == null)
            //{
            //    QCType QCType1 = ObjectSpace.CreateObject<QCType>();
            //    QCType1.QCTypeName = "Sample";
            //}
            //if (ObjectSpace.FindObject<WorkflowConfig>(null) == null)
            //{
            //    WorkflowConfig ICMlevel1 = ObjectSpace.CreateObject<WorkflowConfig>();
            //    ICMlevel1.Level = 1;
            //    ICMlevel1.ActivationOn = true;
            //    ICMlevel1.NextLevel = 2;
            //    WorkflowConfig ICMlevel2 = ObjectSpace.CreateObject<WorkflowConfig>();
            //    ICMlevel2.Level = 2;
            //    ICMlevel2.ActivationOn = true;
            //    ICMlevel2.NextLevel = 3;
            //    WorkflowConfig ICMlevel3 = ObjectSpace.CreateObject<WorkflowConfig>();
            //    ICMlevel3.Level = 3;
            //    ICMlevel3.ActivationOn = true;
            //    ICMlevel3.NextLevel = 4;
            //    WorkflowConfig ICMlevel4 = ObjectSpace.CreateObject<WorkflowConfig>();
            //    ICMlevel4.Level = 4;
            //    ICMlevel4.ActivationOn = true;
            //    ICMlevel4.NextLevel = 5;
            //    WorkflowConfig ICMlevel5 = ObjectSpace.CreateObject<WorkflowConfig>();
            //    ICMlevel5.Level = 5;
            //    ICMlevel5.ActivationOn = true;
            //    ICMlevel5.NextLevel = 6;
            //    WorkflowConfig ICMlevel6 = ObjectSpace.CreateObject<WorkflowConfig>();
            //    ICMlevel6.Level = 6;
            //    ICMlevel6.ActivationOn = true;
            //    ICMlevel6.NextLevel = 0;
            //}
            //if (ObjectSpace.FindObject<OrderingItemSetup>(null) == null)
            //{
            //    OrderingItemSetup ICMOrdering = ObjectSpace.CreateObject<OrderingItemSetup>();
            //    ICMOrdering.OrderingItemon = true;
            //}
            ////XafApplication Application=null;
            ////    IModelApplicationNavigationItems navigationItem = WebApplication.Instance.Model as IModelApplicationNavigationItems;
            ////    if (navigationItem != null)
            ////    {
            ////        foreach (IModelNavigationItem item in navigationItem.NavigationItems.AllItems)             
            ////        {                                    
            ////            string parent = string.Empty;                                         
            ////            if (item.Parent.Parent.GetValue<string>("Caption") != null)
            ////            {

            ////                string strPath = item.GetValue<string>("ItemPath");
            ////                //string[] strItem = strPath.Split(new[] { "/Items/" }, StringSplitOptions.None);                
            ////                parent = Findcaption(item.Parent.Parent, parent);                      
            ////                Modules.BusinessObjects.Setting.NavigationItem UserNavigation = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.NavigationItem>(
            ////                      new BinaryOperator("NavigationId", item.Id));
            ////                if (UserNavigation != null)
            ////                {
            ////                    UserNavigation.Exclude = !item.Visible;
            ////                }
            ////                    if (UserNavigation == null)
            ////                {
            ////                    UserNavigation = ObjectSpace.CreateObject<Modules.BusinessObjects.Setting.NavigationItem>();
            ////                    UserNavigation.NavigationId = item.Id;
            ////                    UserNavigation.NavigationCaption = item.Caption;
            ////                    if (item.View.AsObjectView != null)
            ////                    {
            ////                        UserNavigation.NavigationModelClass = item.View.AsObjectView.ModelClass.Name;
            ////                    }
            ////                    UserNavigation.NavigationView = item.View.Id.ToString();
            ////                    UserNavigation.Itempath = strPath;
            ////                    UserNavigation.Parent = parent;
            ////                    UserNavigation.Exclude = !item.Visible;
            ////                }
            ////                else
            ////                {
            ////                    UserNavigation.NavigationCaption = item.Caption;
            ////                    if (item.View.AsObjectView != null)
            ////                    {
            ////                        UserNavigation.NavigationModelClass = item.View.AsObjectView.ModelClass.Name;
            ////                    }
            ////                    UserNavigation.NavigationView = item.View.Id.ToString();
            ////                    UserNavigation.Itempath = strPath;
            ////                    UserNavigation.Parent = parent;
            ////                    UserNavigation.Exclude = !item.Visible;
            ////                }
            ////                ObjectSpace.CommitChanges();
            ////            }
            ////        }
            ////    }
        }

        //private static string Findcaption(IModelNode values, string strParent)
        //{
        //    if (values.Parent != null)
        //    {
        //        if (values.GetValue<string>("Id") != "Items" && values.GetValue<string>("Id") != "Application" && values.GetValue<string>("Id") != "NavigationItems")
        //        {
        //            if (strParent.Length == 0)
        //            { strParent = values.GetValue<string>("Caption"); }
        //            else
        //            { strParent = values.GetValue<string>("Caption") + @"\" + strParent; }
        //        }
        //        strParent = Findcaption(values.Parent, strParent);
        //        return strParent;
        //    }
        //    else
        //    {
        //        return strParent;
        //    }
        //}

        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
        ////private CustomSystemRole CreateDefaultRole()
        ////{
        ////    CustomSystemRole defaultRole = ObjectSpace.FindObject<CustomSystemRole>(new BinaryOperator("Name", "Default"));
        ////    if (defaultRole == null)
        ////    {
        ////        defaultRole = ObjectSpace.CreateObject<CustomSystemRole>();
        ////        defaultRole.Name = "Default";

        ////        //defaultRole.AddObjectPermission<CustomSystemUser>(SecurityOperations.ReadOnlyAccess, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
        ////        //defaultRole.AddMemberPermission<CustomSystemUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
        ////        //defaultRole.AddMemberPermission<CustomSystemUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
        ////        //defaultRole.AddTypePermissionsRecursively<CustomSystemRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
        ////        //defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
        ////        //defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
        ////    }
        ////    return defaultRole;
        ////}
    }
}
