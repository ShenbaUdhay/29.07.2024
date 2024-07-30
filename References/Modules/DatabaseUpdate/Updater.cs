using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
//using DevExpress.ExpressApp.Dashboards;

namespace Modules.DatabaseUpdate
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
            //     DashboardsModule.AddDashboardData<DashboardData>(
            //ObjectSpace, "My Dashboard", Resources.MyDashboard);
            //     DashboardsModule.AddDashboardData<DashboardData>(
            //ObjectSpace, "My Dashboard1", Resources.Dashboard1);

            //初始化角色
            CustomSystemRole administrativeRole = ObjectSpace.FindObject<CustomSystemRole>(
                new BinaryOperator("Name", SecurityStrategy.AdministratorRoleName));
            if (administrativeRole == null)
            {
                administrativeRole = ObjectSpace.CreateObject<CustomSystemRole>();
                administrativeRole.Name = SecurityStrategy.AdministratorRoleName;
                administrativeRole.IsAdministrative = true;
            }
            //初始化用户
            const string adminName = "Administrator";
            Employee administratorUser = ObjectSpace.FindObject<Employee>(
                new BinaryOperator("UserName", adminName));
            if (administratorUser == null)
            {
                administratorUser = ObjectSpace.CreateObject<Employee>();
                administratorUser.UserName = adminName;
                administratorUser.IsActive = true;
                administratorUser.SetPassword("");
                administratorUser.FirstName = "Admin";
                administratorUser.LastName = "User";
                administratorUser.Roles.Add(administrativeRole);
            }

            //初始化用户
            const string ServiceName = "Service";
            Employee ServiceUser = ObjectSpace.FindObject<Employee>(
                new BinaryOperator("UserName", ServiceName));
            if (ServiceUser == null)
            {
                ServiceUser = ObjectSpace.CreateObject<Employee>();
                ServiceUser.UserName = ServiceName;
                ServiceUser.IsActive = true;
                //ServiceUser.SetPassword("AlpacaLabMaster@q322");
                ServiceUser.FirstName = "Service";
                ServiceUser.LastName = "User";
                ServiceUser.Roles.Add(administrativeRole);
            }
            else if (ServiceUser != null && ServiceUser.Roles.Count == 0)
            {
                ServiceUser.Roles.Add(administrativeRole);
                ObjectSpace.CommitChanges();
            }

            //初始化公司
            if (ObjectSpace.FindObject<Company>(null) == null)
            {
                Company company1 = ObjectSpace.CreateObject<Company>();
                company1.CompanyName = "BTLIMS";
                company1.Language = Language.Chinese;
                company1.IsValid = true;
                company1.Employees.Add(administratorUser);
                administrativeRole.Company = company1;
                administratorUser.Company = company1;
            }
            if (ObjectSpace.FindObject<QCType>(null) == null)
            {
                QCType QCType1 = ObjectSpace.CreateObject<QCType>();
                QCType1.QCTypeName = "Sample";
            }
            if (ObjectSpace.FindObject<WorkflowConfig>(null) == null)
            {
                WorkflowConfig ICMlevel1 = ObjectSpace.CreateObject<WorkflowConfig>();
                ICMlevel1.Level = 1;
                ICMlevel1.ActivationOn = true;
                ICMlevel1.NextLevel = 2;
                WorkflowConfig ICMlevel2 = ObjectSpace.CreateObject<WorkflowConfig>();
                ICMlevel2.Level = 2;
                ICMlevel2.ActivationOn = true;
                ICMlevel2.NextLevel = 3;
                WorkflowConfig ICMlevel3 = ObjectSpace.CreateObject<WorkflowConfig>();
                ICMlevel3.Level = 3;
                ICMlevel3.ActivationOn = true;
                ICMlevel3.NextLevel = 4;
                WorkflowConfig ICMlevel4 = ObjectSpace.CreateObject<WorkflowConfig>();
                ICMlevel4.Level = 4;
                ICMlevel4.ActivationOn = true;
                ICMlevel4.NextLevel = 5;
                WorkflowConfig ICMlevel5 = ObjectSpace.CreateObject<WorkflowConfig>();
                ICMlevel5.Level = 5;
                ICMlevel5.ActivationOn = true;
                ICMlevel5.NextLevel = 6;
                WorkflowConfig ICMlevel6 = ObjectSpace.CreateObject<WorkflowConfig>();
                ICMlevel6.Level = 6;
                ICMlevel6.ActivationOn = true;
                ICMlevel6.NextLevel = 0;
            }
            if (ObjectSpace.FindObject<OrderingItemSetup>(null) == null)
            {
                OrderingItemSetup ICMOrdering = ObjectSpace.CreateObject<OrderingItemSetup>();
                ICMOrdering.OrderingItemon = true;
            }
            if (ObjectSpace.FindObject<PrepTypes>(null) == null)
            {
                List<Tuple<uint, string>> lstPrepType = new List<Tuple<uint, string>>();
                lstPrepType.Add(new Tuple<uint, string>(2, "Digestion"));
                lstPrepType.Add(new Tuple<uint, string>(2, "Extraction"));
                lstPrepType.Add(new Tuple<uint, string>(2, "Sample Preparation"));
                lstPrepType.Add(new Tuple<uint, string>(1, "SPLP VOC"));
                lstPrepType.Add(new Tuple<uint, string>(1, "SPLPExtraction"));
                lstPrepType.Add(new Tuple<uint, string>(1, "TCLP Extraction"));
                lstPrepType.Add(new Tuple<uint, string>(1, "TCLP VOC"));
                UnitOfWork uow = new UnitOfWork(((XPObjectSpace)ObjectSpace).Session.DataLayer);
                foreach (Tuple<uint, string> objvalue in lstPrepType)
                {
                    PrepTypes newprep = new PrepTypes(uow);
                    newprep.Tier = objvalue.Item1;
                    newprep.SamplePrepType = objvalue.Item2;
                    newprep.Save();
                }
                uow.CommitChanges();
            }

        }



        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
        private CustomSystemRole CreateDefaultRole()
        {
            CustomSystemRole defaultRole = ObjectSpace.FindObject<CustomSystemRole>(new BinaryOperator("Name", "Default"));
            if (defaultRole == null)
            {
                defaultRole = ObjectSpace.CreateObject<CustomSystemRole>();
                defaultRole.Name = "Default";


            }
            return defaultRole;
        }
    }
}
