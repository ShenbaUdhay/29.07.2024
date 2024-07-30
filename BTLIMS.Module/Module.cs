using BTLIMS.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using LDM.Module.Controllers.Metrc;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Login;
using Modules.BusinessObjects.Metrc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web;
//using BTLIMS.Module.Controllers.LogOn;

namespace BTLIMS.Module
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class BTLIMSModule : ModuleBase
    {
        int OtpCount = 0;
        Metrcinfo metrcinfo = new Metrcinfo();
        public BTLIMSModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
            //注册自定义函数参数
            FuncCurrentCompanyOid.Register();//当前用户所在公司
            FuncCurrentUserName.Register();//当前用户名
            FuncCurrentUserIsAdministrative.Register();//当前用户是否超级管理员
            FuncCurrentUserIsAdmin.Register();//当前用户是否管理员非超级管理员
            FuncCurrentCompanyName.Register();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        //public override IList<PopupWindowShowAction> GetStartupActions()
        //{
        //    Employee currentUser = SecuritySystem.CurrentUser as Employee;
        //    List<PopupWindowShowAction> actions = new List<PopupWindowShowAction>(base.GetStartupActions());
        //    //if (currentUser.IsNewUser)
        //    //{
        //    //    IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(LoginOneTimePin));
        //    //    PopupWindowShowAction startupAction = new PopupWindowShowAction();
        //    //    startupAction.CustomizePopupWindowParams +=
        //    //        delegate (Object sender, CustomizePopupWindowParamsEventArgs e)
        //    //        {
        //    //            LoginOneTimePin parameters = new LoginOneTimePin();
        //    //            DetailView detailView = Application.CreateDetailView(objectSpace, parameters, true);
        //    //            detailView.ViewEditMode = ViewEditMode.Edit;
        //    //            e.Context = TemplateContext.PopupWindow;
        //    //            e.View = detailView;
        //    //        };
        //    //    startupAction.Cancel += StartupAction_Cancel;
        //    //    startupAction.Execute += StartupAction_Execute;
        //    //    startupAction.IsSizeable = true;
        //    //    SendOTPEmail(currentUser);
        //    //    actions.Add(startupAction);
        //    //    return actions;
        //    //}
        //    //else
        //    //{
        //    //    return actions;
        //    //}
        //}
        private void StartupAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            LoginOneTimePin confirmationWindowParameters = e.PopupWindowViewCurrentObject as LoginOneTimePin;
            TimeSpan ts = DateTime.Now - Convert.ToDateTime(HttpContext.Current.Session["Time"]);
            if (ts.TotalMinutes < 5)
            {
                if (confirmationWindowParameters.OTP == null)
                {
                    throw new UserFriendlyException("The OTP is null. Please enter a valid OTP.");
                }

                else if (confirmationWindowParameters.OTP != HttpContext.Current.Session["OTP"].ToString())
                {
                    OtpCount++;
                    if (OtpCount == 3)
                    {
                        Application.LogOff();
                        WebApplication.Redirect("CustomError.aspx", true);
                    }
                    throw new UserFriendlyException("The OTP is wrong! Try again.");
                }
                else
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    UnitOfWork uow = new UnitOfWork(((XPObjectSpace)os).Session.DataLayer);
                    Employee currentUser = SecuritySystem.CurrentUser as Employee;
                    Employee objEmp = uow.GetObjectByKey<Employee>(currentUser.Oid);
                    if (objEmp != null)
                    {
                        objEmp.IsNewUser = false;
                        uow.CommitChanges();
                    }
                }
            }
            else
            {
                throw new UserFriendlyException("The OTP is Expired! Try again.");
            }
        }
        private void SendOTPEmail(Employee CurrentUser)
        {
            try
            {
                SmtpClient sc = new SmtpClient();
                string strMailFromUserName = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromUserName"];
                string strMailFromPassword = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromPassword"];
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                Random random = new Random();
                HttpContext.Current.Session["OTP"] = random.Next(100001, 999999);
                message.From = new MailAddress(strMailFromUserName);
                message.To.Add(CurrentUser.UserName);
                message.IsBodyHtml = true;
                message.Subject = "OTP for ALPACALIMS - Login User";
                message.Body = "Thank you for creating a user in ALPACA LIMS.  To complete this process, enter a one-time password that is valid for 5 minutes.<br/><br/><br/>"
                    + "OTP: " + HttpContext.Current.Session["OTP"].ToString();
                sc.UseDefaultCredentials = true;
                sc.Host = "smtp.gmail.com";
                //sc.Port = 587;
                sc.Port = Convert.ToInt32(((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailPort"]);
                sc.EnableSsl = true;
                sc.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                sc.DeliveryFormat = SmtpDeliveryFormat.SevenBit;
                sc.Credentials = new NetworkCredential(strMailFromUserName, strMailFromPassword);
                if (message.To != null && message.To.Count > 0)
                {
                    sc.Send(message);
                    HttpContext.Current.Session["Time"] = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                Application.LogOff();
            }
        }
        private void StartupAction_Cancel(object sender, EventArgs e)
        {
            Application.LogOff();
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.  
            application.SetupComplete += Application_SetupComplete;
            //application.CreateCustomLogonWindowControllers += application_CreateCustomLogonWindowControllers;
        }

        private void Application_SetupComplete(object sender, EventArgs e)
        {
            Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
        }

        private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        {
            var nonPersistentObjectSpace = e.ObjectSpace as NonPersistentObjectSpace;
            if (nonPersistentObjectSpace != null)
            {
                nonPersistentObjectSpace.ObjectsGetting += NonPersistentObjectSpace_ObjectsGetting;
            }
        }

        private void NonPersistentObjectSpace_ObjectsGetting(object sender, ObjectsGettingEventArgs e)
        {
            if (e.ObjectType == typeof(MetrcFacility))
            {
                if (metrcinfo.dtFacilitydatasource == null)
                {
                    MetrcController metrc = new MetrcController();
                    e.Objects = metrc.getfacility();
                }
                else if (metrcinfo.dtFacilitydatasource.Count > 0)
                {
                    e.Objects = metrcinfo.dtFacilitydatasource;
                }
            }
        }

        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
            //for auditTrial
            //DevExpress.ExpressApp.Xpo.XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary.GetClassInfo(typeof(AuditDataItemPersistent)).GetMember("AuditedObject").RemoveAttribute(typeof(MemberDesignTimeVisibilityAttribute));
            //typesInfo.RefreshInfo(typeof(AuditDataItemPersistent));

            //DevExpress.Xpo.Metadata.XPClassInfo ci = DevExpress.ExpressApp.Xpo.XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary.GetClassInfo(typeof(AuditDataItemPersistent));
            //ci.GetMember("AuditedObject").RemoveAttribute(typeof(MemberDesignTimeVisibilityAttribute));
            //ci.CreateAliasedMember("AuditedObjectTypeName", typeof(string), "AuditedObject.TargetType.TypeName");
            //typesInfo.RefreshInfo(typeof(AuditDataItemPersistent));



            //End
            //XPAliasedMemberInfo mi;
            //ci.CreateAliasedMember

        }

        //private void application_CreateCustomLogonWindowControllers(object sender, CreateCustomLogonWindowControllersEventArgs e)
        //{
        //    e.Controllers.Add(((XafApplication)sender).CreateController<LogOnViewController>());
        //}
    }
}
