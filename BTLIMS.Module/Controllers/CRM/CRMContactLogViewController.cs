using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Accounts;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Linq;
using XCRM.Module.BusinessObjects;

namespace LDM.Module.Controllers.CRM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CRMContactLogViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        DialogController dcOK;
        public String strCurrentUserId = string.Empty;
        LeadInfo objLeadinfo = new LeadInfo();
        ClientInfo objClientinfo = new ClientInfo();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        public CRMContactLogViewController()
        {
            InitializeComponent();
            TargetViewId = "Customer_Notes_ListView_ContactLog_ClientNoteLog;" + "CRMProspects_Notes_ListView_ContactLog_ProspectNoeLog;"
                + "Notes_DetailView_Client_CallLog;" + "Notes_DetailView_Prospect;" + "Employee_LookupListView_CallLog;" + "Customer_Notes_ListView_ContactLog_DVCollection;"
                + "Notes_DetailView_Client_CallLog_Popup;" + "Customer_Notes_ListView_ProspectCallLog_DVCollection;" + "Contact_DetailView_ClientPopup;" + "Contact_DetailView;";
            Notify.TargetViewId = "Notes_DetailView_Client_CallLog;" + "Notes_DetailView_Prospect;";
            ADD.TargetViewId = "Customer_Notes_ListView_ContactLog_DVCollection;" + "Customer_Notes_ListView_ProspectCallLog_DVCollection;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View.Id == "Notes_DetailView_Client_CallLog" || View.Id == "Notes_DetailView_Prospect")
                {
                    DashboardViewItem lvNotes = ((DetailView)View).FindItem("NoteLog") as DashboardViewItem;
                    if (lvNotes != null && lvNotes.InnerView != null)
                    {
                        lvNotes.InnerView.ObjectSpace.Refresh();
                        Application.MainWindow.View.Refresh();
                    }

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
            try
            {
                Employee currentUser = SecuritySystem.CurrentUser as Employee;
                if (currentUser != null && View != null && View.Id != null)
                {
                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.ClientCallLogIsWrite = false;
                        objPermissionInfo.ProspectsCallLogIsWrite = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.ClientCallLogIsWrite = true;
                            objPermissionInfo.ProspectsCallLogIsWrite = true;
                        }
                        else
                        {
                            foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "NotesinClient" && i.Write == true) != null)
                                {
                                    objPermissionInfo.ClientCallLogIsWrite = true;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "NotesinProspects" && i.Write == true) != null)
                                {
                                    objPermissionInfo.ProspectsCallLogIsWrite = true;
                                }
                            }
                        }
                    }
                    if (View.Id == "Notes_DetailView_Client_CallLog" || View.Id == "Customer_Notes_ListView_ContactLog_DVCollection")
                    {
                        Notify.Active["showNotify"] = objPermissionInfo.ClientCallLogIsWrite;
                        ADD.Active["showAdd"] = objPermissionInfo.ClientCallLogIsWrite;
                    }
                    if (View.Id == "Notes_DetailView_Prospect" || View.Id == "Customer_Notes_ListView_ProspectCallLog_DVCollection")
                    {
                        Notify.Active["showNotify"] = objPermissionInfo.ProspectsCallLogIsWrite;
                        ADD.Active["showAdd"] = objPermissionInfo.ProspectsCallLogIsWrite;
                    }
                }


                strCurrentUserId = Application.Security.UserId.ToString();
                if (View.Id == "Customer_Notes_ListView_ContactLog_ClientNoteLog")
                {
                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(Notes))
                    {
                        Notes objNotes = (Notes)Application.MainWindow.View.CurrentObject;
                        if (objNotes != null && objNotes.Customer != null)
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Customer.Oid] =?", objNotes.Customer.Oid);
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid Is null");
                        }
                    }
                }
                else if (View.Id == "CRMProspects_Notes_ListView_ContactLog_ProspectNoeLog")
                {
                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(Notes))
                    {
                        Notes objNotes = (Notes)Application.MainWindow.View.CurrentObject;
                        if (objNotes != null && objNotes.CRMProspects != null)
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[CRMProspects.Oid] = ?", objNotes.CRMProspects.Oid);
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid Is null");
                        }
                    }
                }
                else if (View.Id == "Notes_DetailView_Client_CallLog" || View.Id == "Notes_DetailView_Prospect" || View.Id == "Contact_DetailView")
                {
                    View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    Frame.GetController<ModificationsController>().SaveAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAction.Executed += SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executing += SaveAndNewAction_Executing;
                    if (View.Id == "Notes_DetailView_Client_CallLog")
                    {
                        if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(Notes))
                        {
                            Notes objNotes = (Notes)Application.MainWindow.View.CurrentObject;
                            //if(ObjectSpace.IsNewObject(objNotes)==true)
                            //{
                            //    ADD.Enabled["CRMADD"] = false;
                            //}
                            DashboardViewItem lvNotes = ((DetailView)View).FindItem("NoteLog") as DashboardViewItem;
                            if (lvNotes != null && lvNotes.InnerView == null)
                            {
                                lvNotes.CreateControl();
                            }
                            if (objNotes != null && lvNotes != null /*&& lvNotes.InnerView != null*/)
                            {
                                if (objNotes != null && objNotes.Customer != null)
                                {
                                    ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Customer.Oid] =?", objNotes.Customer.Oid);
                                }
                                else
                                {
                                    ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid Is null");
                                }
                            }
                        }
                    }

                    else if (View.Id == "Notes_DetailView_Prospect")
                    {
                        if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(Notes))
                        {
                            Notes objNotes = (Notes)Application.MainWindow.View.CurrentObject;
                            DashboardViewItem lvNotes = ((DetailView)View).FindItem("NoteLog") as DashboardViewItem;
                            if (lvNotes != null && lvNotes.InnerView == null)
                            {
                                lvNotes.CreateControl();
                            }
                            if (objNotes != null && lvNotes != null /*&& lvNotes.InnerView != null*/)
                            {
                                if (objNotes != null && objNotes.CRMProspects != null)
                                {
                                    ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[CRMProspects.Oid] =?", objNotes.CRMProspects.Oid);
                                }

                                else
                                {
                                    ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid Is null");
                                }
                            }
                        }
                    }
                }
                else if (View.Id == "Customer_Notes_ListView_ContactLog_DVCollection" || View.Id == "Customer_Notes_ListView_ProspectCallLog_DVCollection")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.ImageName = "Remove";
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Caption = "Remove";
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Execute += DeleteAction_Execute;
                    Notes objNotes = (Notes)Application.MainWindow.View.CurrentObject;
                    if (Application.MainWindow.View.ObjectSpace.IsNewObject(objNotes) == true)
                    {
                        ADD.Enabled["CRMADD"] = false;
                    }
                }
                else if (View.Id == "Employee_LookupListView_CallLog")
                {
                    View.ControlsCreated += View_ControlsCreated;
                }
                else if (View.Id == "Contact_DetailView_ClientPopup")
                {
                    Notes objNotes = (Notes)Application.MainWindow.View.CurrentObject;
                    Contact objContact = (Contact)View.CurrentObject;
                    if (objNotes != null && objContact != null)
                    {
                        objContact.Customer = View.ObjectSpace.GetObject(objNotes.Customer);
                    }


                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }

        private void DeleteAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Notes notes = View.SelectedObjects as Notes;

            if(e.SelectedObjects.Count>0)
            {
                ObjectSpace.Delete(e.SelectedObjects);
                Application.ShowViewStrategy.ShowMessage("Removed successfuly.", InformationType.Success, 3000, InformationPosition.Top);
                return;
            }
                
        }

        private void SaveAndNewAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Contact_DetailView")
                {
                    Contact objContact = (Contact)View.CurrentObject;
                    if (objContact != null)
                    {
                        objContact.IsReport = true;
                        objContact.IsInvoice = false;
                        ObjectSpace.CommitChanges();
                    }
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
                View.ControlsCreated -= View_ControlsCreated;
                if (View.Id == "Employee_LookupListView_CallLog")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    Notes objNotes = (Notes)Application.MainWindow.View.CurrentObject;
                    if (objNotes != null)
                    {
                        foreach (Employee objemp in objNotes.Employee.ToList())
                        {
                            gridListEditor.Grid.Selection.SelectRowByKey(objemp.Oid);
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
        private void SaveAndCloseAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Contact_DetailView")
                {
                    Contact objContact = (Contact)View.CurrentObject;
                    if (objContact != null)
                    {
                        objContact.IsReport = true;
                        objContact.IsInvoice = false;
                        ObjectSpace.CommitChanges();
                    }
                }
                if (View != null && View.Id == "Notes_DetailView_Client_CallLog" || View.Id == "Notes_DetailView_Prospect")
                {
                    Notes objNote = (Notes)View.CurrentObject;
                    NoteInfo objNoteInfo = new NoteInfo();
                    if (objNote != null)
                    {
                        if (objnavigationRefresh.ClickedNavigationItem == "NotesinClient")
                        {
                            objNoteInfo.FormName = "Client";
                        }
                        else
                        {
                            objNoteInfo.FormName = "Prospect";
                        }
                        AlertNotification objAlert = ObjectSpace.CreateObject<AlertNotification>();
                        objAlert.Subject = "New Note Created for " + objNoteInfo.FormName + "-" + objNote.Title + "," + objNote.Text;
                        objAlert.StartDate = DateTime.Now;
                        //objAlert.RemindIn = TimeSpan.FromMinutes(5);
                        objAlert.RemindIn = TimeSpan.FromHours(4);
                        objAlert.NoteID = objNote;
                        objAlert.Description = objNote.Text;
                        objAlert.User = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("Oid =?", SecuritySystem.CurrentUserId));
                        objAlert.Category = NotificationCategory.NotesAlert;

                        ObjectSpace.CommitChanges();
                        //if (objNote.Customer == null && Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "Customer_DetailView")
                        //{
                        //    Customer objcust = (Customer)Application.MainWindow.View.CurrentObject;
                        //    if (objcust != null)
                        //    {
                        //        objNote.Customer = View.ObjectSpace.GetObject<Customer>(objcust);
                        //    }
                        //}
                        if (objNote.Customer != null)
                        {
                            foreach (Employee objDCUser in objNote.Employee)
                            {
                                EmailStatus objEmailStatus = ObjectSpace.CreateObject<EmailStatus>();
                                objEmailStatus.From = "info@btsoft.com";
                                objEmailStatus.To = objDCUser.Email;
                                string strDescription;
                                strDescription = "Date : " + objNote.Date.Date.ToShortDateString() + System.Environment.NewLine + "<br>Author : " + Application.Security.UserName.ToString() + System.Environment.NewLine +
                                "<br>Source : AlpacaLims/" + objNoteInfo.FormName.ToString() + "/Note" + System.Environment.NewLine + "<br>Subject : " + objNote.Title.ToString() + System.Environment.NewLine + "<br>Customer : " + objNoteInfo.Customer + System.Environment.NewLine + "<br>Text : " + objNote.Text;
                                // objEmailStatus.Body = objNote.Description;
                                //objNote.Description = "Date :" + objNote.Date.Date.ToShortDateString() + System.Environment.NewLine + "Author :" + Application.Security.UserName.ToString() + System.Environment.NewLine +
                                //"Source From : BTSoft CRM/" + objNoteInfo.FormName.ToString()+"/Note" + System.Environment.NewLine + "Subject :" + objNote.Title.ToString() + System.Environment.NewLine + "Customer :" + objNoteInfo.Customer+ System.Environment.NewLine + "Text :" + objNote.Description;

                                objEmailStatus.Body = strDescription;

                                objEmailStatus.Subject = "AlpacaLims Notification/" + objNote.Title;
                                objEmailStatus.MailCreatedDate = DateTime.Now;
                                objEmailStatus.Status = "Processing";
                                objEmailStatus.IsMailSent = false;
                                ObjectSpace.CommitChanges();
                            }
                        }
                        if (objNote.FollowUpDate != DateTime.MinValue)
                        {
                            Activity objActivity = ObjectSpace.FindObject<Activity>(CriteriaOperator.Parse("NoteID='" + objNote.Oid + "'"));
                            if (objActivity == null)
                            {
                                Activity objAct = ObjectSpace.CreateObject<Activity>();
                                string strdate = objNote.FollowUpDate.ToShortDateString();
                                string strStartdate = strdate + " " + "10:00 AM";
                                string strEnddate = strdate + " " + "02:00 PM";
                                objAct.NoteID = objNote;
                                //DateTime dt = DateTime.ParseExact(strdate, "MM/dd/yyyy hh:mm", System.Globalization.CultureInfo.InvariantCulture);
                                Employee objUser = ObjectSpace.GetObjectByKey<Employee>(new Guid(strCurrentUserId));
                                objAct.Owner = objUser;
                                //objAct.Subject = "Follow Up Calls, " + objUser.UserName;
                                objAct.Subject = "Follow Up Calls, " + objUser.FirstName;
                                objAct.StartDateOn = Convert.ToDateTime(strStartdate);
                                objAct.EndDateOn = Convert.ToDateTime(strEnddate);
                                objAct.Description = objNote.Text + System.Environment.NewLine +
                                            objUser.FirstName + ", Please give a follow up call to " + objClientinfo.ClientName;
                                //if (Application.MainWindow.View.Id == "CRMLead_DetailView")
                                //{
                                //    objAct.Description = objNote.Text + System.Environment.NewLine +
                                //         objUser.FirstName + ", Please give a follow up call to " + objLeadinfo.LeadName;
                                //    //objUser.UserName + ", Please give a follow up call to " + objLeadinfo.LeadName;
                                //}
                                //if (Application.MainWindow.View.Id == "Customer_DetailView")
                                //{

                                //    //objUser.UserName + ", Please give a follow up call to " + objClientinfo.ClientName;

                                //}
                                if (objNote.Employee.Count > 0)
                                {
                                    foreach (Employee objUsers in objNote.Employee)
                                    {
                                        objAct.Employee.Add(objUsers);
                                    }
                                }
                            }
                            else
                            {
                                // CRMActivity objAct = ObjectSpace.CreateObject<CRMActivity>();
                                string strdate = objNote.FollowUpDate.ToShortDateString();
                                string strStartdate = strdate + " " + "10:00 AM";
                                string strEnddate = strdate + " " + "02:00 PM";
                                objActivity.NoteID = objNote;
                                //DateTime dt = DateTime.ParseExact(strdate, "MM/dd/yyyy hh:mm", System.Globalization.CultureInfo.InvariantCulture);
                                Employee objUser = ObjectSpace.GetObjectByKey<Employee>(new Guid(strCurrentUserId));
                                objActivity.Owner = objUser;
                                //objActivity.Subject = "Follow Up Calls, " + objUser.UserName;
                                objActivity.Subject = "Follow Up Calls, " + objUser.FirstName;
                                objActivity.StartDateOn = Convert.ToDateTime(strStartdate);
                                objActivity.EndDateOn = Convert.ToDateTime(strEnddate);

                                objActivity.Description = objNote.Text + System.Environment.NewLine +
                                            objUser.FirstName + ", Please give a follow up call to " + objClientinfo.ClientName;
                                //if (Application.MainWindow.View.Id == "Customer_DetailView")
                                //{

                                //    //objUser.UserName + ", Please give a follow up call to " + objClientinfo.ClientName;

                                //}
                                if (objNote.Employee.Count > 0)
                                {
                                    foreach (Employee objUsers in objNote.Employee)
                                    {
                                        objActivity.Employee.Add(objUsers);
                                    }
                                }
                            }
                            ObjectSpace.CommitChanges();
                            ObjectSpace.Refresh();
                        }
                        Application.MainWindow.GetController<DevExpress.ExpressApp.Notifications.Web.WebNotificationsController>().RefreshNotifications();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Contact_DetailView")
                {
                    Contact objContact = (Contact)View.CurrentObject;
                    if (objContact != null)
                    {
                        objContact.IsReport = true;
                        objContact.IsInvoice = false;
                        ObjectSpace.CommitChanges();
                    }
                }
                if (View != null && View.Id == "Notes_DetailView_Client_CallLog" || View.Id == "Notes_DetailView_Prospect")
                {
                    Notes objNote = (Notes)View.CurrentObject;
                    NoteInfo objNoteInfo = new NoteInfo();
                    if (objNote != null)
                    {
                        if (objnavigationRefresh.ClickedNavigationItem == "NotesinClient")
                        {
                            objNoteInfo.FormName = "Client";
                        }
                        else
                        {
                            objNoteInfo.FormName = "Prospect";
                        }
                        AlertNotification objAlert = ObjectSpace.CreateObject<AlertNotification>();
                        objAlert.Subject = "New Note Created for " + objNoteInfo.FormName + "-" + objNote.Title + "," + objNote.Text;
                        objAlert.StartDate = DateTime.Now;
                        //objAlert.RemindIn = TimeSpan.FromMinutes(5);
                        objAlert.RemindIn = TimeSpan.FromHours(4);
                        objAlert.NoteID = objNote;
                        objAlert.Description = objNote.Text;
                        objAlert.User = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("Oid =?", SecuritySystem.CurrentUserId));
                        objAlert.Category = NotificationCategory.NotesAlert;

                        ObjectSpace.CommitChanges();
                        //if (objNote.Customer == null && Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "Customer_DetailView")
                        //{
                        //    Customer objcust = (Customer)Application.MainWindow.View.CurrentObject;
                        //    if (objcust != null)
                        //    {
                        //        objNote.Customer = View.ObjectSpace.GetObject<Customer>(objcust);
                        //    }
                        //}
                        if (objNote.Customer != null)
                        {
                            foreach (Employee objDCUser in objNote.Employee)
                            {
                                EmailStatus objEmailStatus = ObjectSpace.CreateObject<EmailStatus>();
                                objEmailStatus.From = "info@btsoft.com";
                                objEmailStatus.To = objDCUser.Email;
                                string strDescription;
                                strDescription = "Date : " + objNote.Date.Date.ToShortDateString() + System.Environment.NewLine + "<br>Author : " + Application.Security.UserName.ToString() + System.Environment.NewLine +
                                "<br>Source : AlpacaLims/" + objNoteInfo.FormName.ToString() + "/Note" + System.Environment.NewLine + "<br>Subject : " + objNote.Title.ToString() + System.Environment.NewLine + "<br>Customer : " + objNoteInfo.Customer + System.Environment.NewLine + "<br>Text : " + objNote.Text;
                                // objEmailStatus.Body = objNote.Description;
                                //objNote.Description = "Date :" + objNote.Date.Date.ToShortDateString() + System.Environment.NewLine + "Author :" + Application.Security.UserName.ToString() + System.Environment.NewLine +
                                //"Source From : BTSoft CRM/" + objNoteInfo.FormName.ToString()+"/Note" + System.Environment.NewLine + "Subject :" + objNote.Title.ToString() + System.Environment.NewLine + "Customer :" + objNoteInfo.Customer+ System.Environment.NewLine + "Text :" + objNote.Description;

                                objEmailStatus.Body = strDescription;

                                objEmailStatus.Subject = "AlpacaLims Notification/" + objNote.Title;
                                objEmailStatus.MailCreatedDate = DateTime.Now;
                                objEmailStatus.Status = "Processing";
                                objEmailStatus.IsMailSent = false;
                                ObjectSpace.CommitChanges();
                            }
                        }
                        if (objNote.FollowUpDate != DateTime.MinValue)
                        {
                            Activity objActivity = ObjectSpace.FindObject<Activity>(CriteriaOperator.Parse("NoteID='" + objNote.Oid + "'"));
                            if (objActivity == null)
                            {
                                Activity objAct = ObjectSpace.CreateObject<Activity>();
                                string strdate = objNote.FollowUpDate.ToShortDateString();
                                string strStartdate = strdate + " " + "10:00 AM";
                                string strEnddate = strdate + " " + "02:00 PM";
                                objAct.NoteID = objNote;
                                //DateTime dt = DateTime.ParseExact(strdate, "MM/dd/yyyy hh:mm", System.Globalization.CultureInfo.InvariantCulture);
                                Employee objUser = ObjectSpace.GetObjectByKey<Employee>(new Guid(strCurrentUserId));
                                objAct.Owner = objUser;
                                //objAct.Subject = "Follow Up Calls, " + objUser.UserName;
                                objAct.Subject = "Follow Up Calls, " + objUser.FirstName;
                                objAct.StartDateOn = Convert.ToDateTime(strStartdate);
                                objAct.EndDateOn = Convert.ToDateTime(strEnddate);
                                objAct.Description = objNote.Text + System.Environment.NewLine +
                                            objUser.FirstName + ", Please give a follow up call to " + objClientinfo.ClientName;
                                //if (Application.MainWindow.View.Id == "CRMLead_DetailView")
                                //{
                                //    objAct.Description = objNote.Text + System.Environment.NewLine +
                                //         objUser.FirstName + ", Please give a follow up call to " + objLeadinfo.LeadName;
                                //    //objUser.UserName + ", Please give a follow up call to " + objLeadinfo.LeadName;
                                //}
                                //if (Application.MainWindow.View.Id == "Customer_DetailView")
                                //{

                                //    //objUser.UserName + ", Please give a follow up call to " + objClientinfo.ClientName;

                                //}
                                if (objNote.Employee.Count > 0)
                                {
                                    foreach (Employee objUsers in objNote.Employee)
                                    {
                                        objAct.Employee.Add(objUsers);
                                    }
                                }
                            }
                            else
                            {
                                // CRMActivity objAct = ObjectSpace.CreateObject<CRMActivity>();
                                string strdate = objNote.FollowUpDate.ToShortDateString();
                                string strStartdate = strdate + " " + "10:00 AM";
                                string strEnddate = strdate + " " + "02:00 PM";
                                objActivity.NoteID = objNote;
                                //DateTime dt = DateTime.ParseExact(strdate, "MM/dd/yyyy hh:mm", System.Globalization.CultureInfo.InvariantCulture);
                                Employee objUser = ObjectSpace.GetObjectByKey<Employee>(new Guid(strCurrentUserId));
                                objActivity.Owner = objUser;
                                //objActivity.Subject = "Follow Up Calls, " + objUser.UserName;
                                objActivity.Subject = "Follow Up Calls, " + objUser.FirstName;
                                objActivity.StartDateOn = Convert.ToDateTime(strStartdate);
                                objActivity.EndDateOn = Convert.ToDateTime(strEnddate);

                                objActivity.Description = objNote.Text + System.Environment.NewLine +
                                            objUser.FirstName + ", Please give a follow up call to " + objClientinfo.ClientName;
                                //if (Application.MainWindow.View.Id == "Customer_DetailView")
                                //{

                                //    //objUser.UserName + ", Please give a follow up call to " + objClientinfo.ClientName;

                                //}
                                if (objNote.Employee.Count > 0)
                                {
                                    foreach (Employee objUsers in objNote.Employee)
                                    {
                                        objActivity.Employee.Add(objUsers);
                                    }
                                }
                            }
                            ObjectSpace.CommitChanges();
                            ObjectSpace.Refresh();
                        }
                        Application.MainWindow.GetController<DevExpress.ExpressApp.Notifications.Web.WebNotificationsController>().RefreshNotifications();
                    }
                    if (View.Id == "Notes_DetailView_Client_CallLog")
                    {
                        if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(Notes))
                        {
                            Notes objNotes = (Notes)Application.MainWindow.View.CurrentObject;
                            DashboardViewItem lvNotes = ((DetailView)View).FindItem("NoteLog") as DashboardViewItem;
                            if (lvNotes != null && lvNotes.InnerView == null)
                            {
                                lvNotes.CreateControl();
                            }
                            if (objNotes != null && lvNotes != null /*&& lvNotes.InnerView != null*/)
                            {
                                if (objNotes != null && objNotes.Customer != null)
                                {
                                    ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Customer.Oid] =?", objNotes.Customer.Oid);
                                }
                                else
                                {
                                    ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid Is null");
                                }
                            }
                        }
                    }
                    else if (View.Id == "Notes_DetailView_Prospect")
                    {
                        if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(Notes))
                        {
                            Notes objNotes = (Notes)Application.MainWindow.View.CurrentObject;
                            DashboardViewItem lvNotes = ((DetailView)View).FindItem("NoteLog") as DashboardViewItem;
                            if (lvNotes != null && lvNotes.InnerView == null)
                            {
                                lvNotes.CreateControl();
                            }
                            if (objNotes != null && lvNotes != null /*&& lvNotes.InnerView != null*/)
                            {
                                if (objNotes != null && objNotes.CRMProspects != null)
                                {
                                    ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[CRMProspects.Oid] =?", objNotes.CRMProspects.Oid);
                                }

                                else
                                {
                                    ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid Is null");
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

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (base.View != null && e.NewValue != e.OldValue && (base.View.Id == "Notes_DetailView_Client_CallLog") && e.PropertyName == "Customer")
                {

                    Notes objNotes = (Notes)e.Object;
                    DashboardViewItem lvNotes = ((DetailView)View).FindItem("NoteLog") as DashboardViewItem;
                    if (objNotes != null && lvNotes != null && lvNotes.InnerView != null)
                    {
                        if (objNotes != null && objNotes.Customer != null)
                        {
                            ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Customer.Oid] =?", objNotes.Customer.Oid);
                            lvNotes.Frame.GetController<CRMContactLogViewController>().ADD.Enabled.RemoveItem("CRMADD");
                        }
                        else
                        {
                            ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid Is null");
                        }
                    }
                }
                else if (base.View != null && e.NewValue != e.OldValue && (base.View.Id == "Notes_DetailView_Prospect") && e.PropertyName == "CRMProspects")
                {
                    Notes objNotes = (Notes)e.Object;
                    DashboardViewItem lvNotes = ((DetailView)View).FindItem("NoteLog") as DashboardViewItem;
                    if (objNotes != null && lvNotes != null && lvNotes.InnerView != null)
                    {
                        if (objNotes != null && objNotes.CRMProspects != null)
                        {
                            ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[CRMProspects.Oid] = ?", objNotes.CRMProspects.Oid);
                            lvNotes.Frame.GetController<CRMContactLogViewController>().ADD.Enabled.RemoveItem("CRMADD");
                        }
                        else
                        {
                            ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid Is null");
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
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            try
            {
                base.OnDeactivated();
                if (View.Id == "Notes_DetailView_Client_CallLog" || View.Id == "Notes_DetailView_Prospect" || View.Id == "Contact_DetailView")
                {
                    View.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    Frame.GetController<ModificationsController>().SaveAction.Executing -= SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Executing -= SaveAndNewAction_Executing;

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Notify_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objectSpace, typeof(Employee));
                ListView createListview = Application.CreateListView("Employee_LookupListView_CallLog", cs, false);
                ShowViewParameters showViewParameters = new ShowViewParameters();
                showViewParameters.CreatedView = createListview;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.CloseOnCurrentObjectProcessing = false;
                dc.Accepting += Dc_Accepting;
                //dc.AcceptAction.Executed += AcceptAction_Executed;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    Notes obj = (Notes)View.CurrentObject;
                    if (obj != null)
                    {
                        foreach (Employee objNotesEmp in obj.Employee.ToList())
                        {
                            if (!e.AcceptActionArgs.SelectedObjects.Contains(objNotesEmp))
                            {
                                Employee objemp = View.ObjectSpace.GetObjectByKey<Employee>(objNotesEmp.Oid);
                                if (objemp != null)
                                {
                                    obj.Employee.Remove(objemp);
                                }
                            }
                        }
                        foreach (Employee objEmployee in e.AcceptActionArgs.SelectedObjects)
                        {
                            if (!obj.Employee.Contains(objEmployee))
                            {
                                Employee objemp = View.ObjectSpace.GetObjectByKey<Employee>(objEmployee.Oid);
                                if (objemp != null)
                                {
                                    obj.Employee.Add(objemp);
                                }

                            }
                        }
                        View.ObjectSpace.CommitChanges();
                    }
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ADD_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                if (View.Id == "Customer_Notes_ListView_ContactLog_DVCollection")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    Notes ObjDvNotes = (Notes)Application.MainWindow.View.CurrentObject;
                    Notes objNotes = os.CreateObject<Notes>();
                    objNotes.Customer = os.GetObject(ObjDvNotes.Customer);
                    e.View = Application.CreateDetailView(os, "Notes_DetailView_Client_CallLog_Popup", true, objNotes);
                    ((DetailView)e.View).ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;

                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(Notes))
                    {
                        //Notes objNotes = (Notes)Application.MainWindow.View.CurrentObject;
                        DashboardViewItem lvNotes = ((DetailView)Application.MainWindow.View).FindItem("NoteLog") as DashboardViewItem;
                        if (lvNotes != null && lvNotes.InnerView == null)
                        {
                            lvNotes.CreateControl();
                        }
                        if (ObjDvNotes != null && lvNotes != null /*&& lvNotes.InnerView != null*/)
                        {
                            if (ObjDvNotes != null && ObjDvNotes.Customer != null)
                            {
                                ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Customer.Oid] =?", ObjDvNotes.Customer.Oid);
                            }
                            else
                            {
                                ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid Is null");
                            }
                        }
                    }
                }
                else if (View.Id == "Customer_Notes_ListView_ProspectCallLog_DVCollection")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    Notes ObjDvNotes = (Notes)Application.MainWindow.View.CurrentObject;
                    Notes objNotes = os.CreateObject<Notes>();
                    objNotes.CRMProspects = os.GetObject(ObjDvNotes.CRMProspects);
                    e.View = Application.CreateDetailView(os, "Notes_DetailView_Prospect_CallLog_Popup", true, objNotes);
                    ((DetailView)e.View).ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;

                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(Notes))
                    {
                        //Notes objNotes = (Notes)Application.MainWindow.View.CurrentObject;
                        DashboardViewItem lvNotes = ((DetailView)Application.MainWindow.View).FindItem("NoteLog") as DashboardViewItem;
                        if (lvNotes != null && lvNotes.InnerView == null)
                        {
                            lvNotes.CreateControl();
                        }
                        if (ObjDvNotes != null && lvNotes != null /*&& lvNotes.InnerView != null*/)
                        {
                            if (ObjDvNotes != null && ObjDvNotes.CRMProspects != null)
                            {
                                ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[CRMProspects.Oid] =?", ObjDvNotes.CRMProspects.Oid);
                            }

                            else
                            {
                                ((ListView)lvNotes.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid Is null");
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
    }
}
