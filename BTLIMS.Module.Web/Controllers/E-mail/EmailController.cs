using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using LDM.Module.Controllers.Public;
using Module.BusinessObjects.Accounts;
using Modules.BusinessObjects.Accounts;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using System;

namespace XCRM.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class EmailController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        IObjectSpace os;
        public EmailController()
        {
            InitializeComponent();

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                os = Application.CreateObjectSpace();
                // Perform various tasks depending on the target View.
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
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        public void CheckUserEmailPermission(string UserID, string Action, object objForm, string strSubject)
        {
            try
            {
                IObjectSpace objSpace = Application.CreateObjectSpace();
                EmailAction objAction = objSpace.FindObject<EmailAction>(CriteriaOperator.Parse("Name='" + Action + "'"));
                // EmailUser objUser = objSpace.FindObject<EmailUser>(CriteriaOperator.Parse("DCUser='" + UserID + "'"));
                if (objAction != null)
                {
                    EmailSetting objEmailSetting = ObjectSpace.FindObject<EmailSetting>(CriteriaOperator.Parse("EmailAction='" + objAction.Oid + "'"));
                    if (objEmailSetting != null)
                    {
                        foreach (EmailUser objUser in objEmailSetting.EmailUser)
                        {
                            if (objUser.Employee != null)
                            {
                                Employee objEmployee = objSpace.FindObject<Employee>(CriteriaOperator.Parse("Oid='" + objUser.Employee.Oid + "'"));
                                if (objEmployee != null && !string.IsNullOrEmpty(objEmployee.Email))
                                {
                                    SendMail(Action, objForm, objEmployee.Email, strSubject);
                                }
                            }
                        }
                    }

                    //DCUser objDCUser = objSpace.FindObject<DCUser>(CriteriaOperator.Parse("Oid='" + UserID + "'"));
                    //if (objDCUser.Email != null)
                    //{
                    //    IList<ActionEmail> objActionEmail = objSpace.GetObjects<ActionEmail>(CriteriaOperator.Parse("EmailAction='" + objAction.Oid + "'"));
                    //    foreach (ActionEmail objEmailSet in objActionEmail)
                    //    {
                    //        //EmailUser objEmailSetting = objSpace.FindObject<EmailUser>(CriteriaOperator.Parse("EmailSetting ='" + objEmailSet.EmailSetting.Oid + "' and DCUser='" + UserID + "'"));
                    //        IList<EmailUser> 
                    //        if (objEmailSetting != null)
                    //        {
                    //            SendMail(Action, objForm, objDCUser.Email,strSubject);
                    //            break;
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
        public void SendMail(string Action, object objForm, string ToMail, string strSubject)
        {
            try
            {
                if (Action == "Qualify Leads")
                {
                    //CRMLead objLead = (CRMLead)objForm;
                    //// SelectedData sproc = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SendEmail_Action", new SprocParameter("@Recipients",ToMail),new SprocParameter("@Subject",objLead.Topic + " is Qualified"),new SprocParameter("@Body","Lead Name =" + objLead.Topic ));
                    //EmailStatus objEmailStatus = ObjectSpace.CreateObject<EmailStatus>();
                    //objEmailStatus.From = "info@btsoft.com";
                    //objEmailStatus.To = ToMail;
                    //objEmailStatus.Body = "Lead Name : " + objLead.Name + "<br>Street : " + objLead.Street1 +
                    //    "<br>City : " + objLead.City + "<br>State : " + objLead.State + "<br>Country : " + objLead.Country;
                    //objEmailStatus.Subject = strSubject;
                    //objEmailStatus.MailCreatedDate = DateTime.Now;
                    //objEmailStatus.Status = "Processing";
                    //objEmailStatus.IsMailSent = false;
                    //ObjectSpace.CommitChanges();
                }
                else if (Action == "New Account Notification")
                {
                    Customer objcustomer = (Customer)objForm;
                    // SelectedData sproc = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SendEmail_Action", new SprocParameter("@Recipients",ToMail),new SprocParameter("@Subject",objLead.Topic + " is Qualified"),new SprocParameter("@Body","Lead Name =" + objLead.Topic ));
                    EmailStatus objEmailStatus = ObjectSpace.CreateObject<EmailStatus>();
                    objEmailStatus.From = "info@btsoft.com";
                    objEmailStatus.To = ToMail;
                    objEmailStatus.Body = "Client : " + objcustomer.CustomerName + "<br>Street : " + objcustomer.Address +
                        "<br>City : " + objcustomer.City.CityName + "<br>State : " + objcustomer.State.LongName + "<br>Country : " + objcustomer.Country.EnglishLongName;
                    objEmailStatus.Subject = strSubject;
                    objEmailStatus.MailCreatedDate = DateTime.Now;
                    objEmailStatus.Status = "Processing";
                    objEmailStatus.IsMailSent = false;
                    ObjectSpace.CommitChanges();
                }
                else if (Action == "Opportunity Created")
                {
                    CRMProspects objOpportunity = (CRMProspects)objForm;
                    // SelectedData sproc = ((XPObjectSpace)os).Session.ExecuteSprocParametrized("SendEmail_Action", new SprocParameter("@Recipients",ToMail),new SprocParameter("@Subject",objLead.Topic + " is Qualified"),new SprocParameter("@Body","Lead Name =" + objLead.Topic ));
                    EmailStatus objEmailStatus = ObjectSpace.CreateObject<EmailStatus>();
                    objEmailStatus.From = "info@btsoft.com";
                    objEmailStatus.To = ToMail;
                    objEmailStatus.Body = "Opportunity : " + objOpportunity.Name + "<br>Street : " + objOpportunity.Street1 +
                        "<br>City : " + objOpportunity.City + "<br>State : " + objOpportunity.State + "<br>Country : " + objOpportunity.Country;
                    objEmailStatus.Subject = "A New Prospects is added";
                    objEmailStatus.MailCreatedDate = DateTime.Now;
                    objEmailStatus.Status = "Processing";
                    objEmailStatus.IsMailSent = false;
                    ObjectSpace.CommitChanges();
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
