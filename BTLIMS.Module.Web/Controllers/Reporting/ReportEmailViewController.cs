using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using LDM.Module.Controllers.Public.FTPSetup;
using Modules.BusinessObjects.Accounting.Receivables;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.Invoicing;
using Modules.BusinessObjects.SuboutTracking;
using Rebex.Net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace LDM.Module.Web.Controllers.Reporting
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ReportEmailViewController : ViewController, IXafCallbackHandler
    {
        DynamicReportDesignerConnection ObjReportDesignerInfo = new DynamicReportDesignerConnection();
        MessageTimer timer = new MessageTimer();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        FtpInfo objFTP = new FtpInfo();
        public string strFTPServerName = string.Empty;
        public string strFTPPath = string.Empty;
        public string strFTPUserName = string.Empty;
        public string strFTPPassword = string.Empty;
        public int FTPPort = 0;
        public bool strFTPStatus;
        NavigationRefresh v = new NavigationRefresh();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        public ReportEmailViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "Reporting_ListView_Delivery;" + "Invoicing_ListView_Delivery;" + "Reporting_ListView_Deliveired;";
            ReportSentEmail.TargetViewId = "Invoicing_ListView_Delivery;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                Employee currentUser = SecuritySystem.CurrentUser as Employee;
                ReportSentEmail.Active.SetItemValue("invoicedelivery", false);
                if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                {
                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            ReportSentEmail.Active.SetItemValue("invoicedelivery", true);

                        }
                        else
                        {
                            foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (objnavigationRefresh.ClickedNavigationItem == "ReportDelivery")
                                {
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportDelivery" && i.NavigationItem.IsDeleted == false && i.Write == true) != null)
                                    {
                                        ReportSentEmail.Active.SetItemValue("invoicedelivery", true);
                                    }
                                }
                                else if (objnavigationRefresh.ClickedNavigationItem == "InvoiceDelivery")
                                {
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "InvoiceDelivery" && i.NavigationItem.IsDeleted == false && i.Write == true) != null)
                                    {
                                        ReportSentEmail.Active.SetItemValue("invoicedelivery", true);
                                    }
                                }
                            }
                        }

                    }





                    //Employee currentUser = SecuritySystem.CurrentUser as Employee;
                    //if (currentUser != null && View != null && View.Id != null)
                    //{
                    //    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    //    {
                    //        objPermissionInfo.ReportDeliveryIsWrite = false;
                    //        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    //        {
                    //            objPermissionInfo.ReportDeliveryIsWrite = true;
                    //        }
                    //        else
                    //        {
                    //            foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                    //            {
                    //                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReportDelivery" && i.Write == true) != null)
                    //                {
                    //                    objPermissionInfo.ReportDeliveryIsWrite = true;
                    //                }
                    //                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "InvoiceDelivery" && i.Write == true) != null)
                    //                {
                    //                    objPermissionInfo.ResultValidationIsWrite = true;
                    //                    //return;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    if (View.Id == "Reporting_ListView_Delivery"|| View.Id =="Invoicing_ListView_Delivery")
                    //    {
                    //        ReportSentEmail.Active["ShowReportSent"] = objPermissionInfo.ReportDeliveryIsWrite;

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
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            try
            {
                if (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired" || View.Id == "Invoicing_ListView_Delivery")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("Contact", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;

                    if (gridListEditor != null)
                    {
                        ASPxGridView gv = gridListEditor.Grid;
                        if (gv != null)
                        {
                            gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            gv.HtmlDataCellPrepared += Gv_HtmlDataCellPrepared; ;
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

        private void Gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired" || View.Id == "Invoicing_ListView_Delivery")
                {
                    if (e.DataColumn.FieldName == "Email")
                    {
                        if (View.Id== "Reporting_ListView_Delivery")
                        {
                        bool mail =Convert.ToBoolean(gridListEditor.Grid.GetRowValues(e.VisibleIndex, "Mail"));
                        bool donotdeliver = Convert.ToBoolean(gridListEditor.Grid.GetRowValues(e.VisibleIndex, "DoNotDeliver"));
                        if (!mail && !donotdeliver)
                        {
                            e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'Contact', 'Email|'+{0}, '', false)", e.VisibleIndex)); 
                        }
                    }
                        else
                        {
                            e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'Contact', 'Email|'+{0}, '', false)", e.VisibleIndex));
                        }
                    }
                }
                if (View.Id == "Reporting_ListView_Delivery")
                {
                    if (e.DataColumn.FieldName == "DoNotDeliver")
                    {
                        //e.Cell.ToolTip = ((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "De").ToString();
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'Contact', 'DoNotDeliver|'+{0}, '', false)", e.VisibleIndex));

                    }
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
                if (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired")
                {
                    string[] values = parameter.Split('|');
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (values[0] == "Email")
                    {
                        string[] param = parameter.Split('|');
                        HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        //CollectionSource cs = new CollectionSource(objectSpace, typeof(ReportingContact));
                        CollectionSource cs = new CollectionSource(objectSpace, typeof(Contact));
                        Modules.BusinessObjects.SampleManagement.Reporting objRpt = View.ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (objRpt != null && objRpt.JobID.ClientName != null)
                        {
                            cs.Criteria["Filter"] = CriteriaOperator.Parse("Not IsNullOrEmpty([Email]) And [IsReport] = True And [Customer.Oid] = ? And ReportDelivery = true", objRpt.JobID.ClientName.Oid);
                        }

                        ListView lvcontact = Application.CreateListView("Contact_ListView_Email", cs, false);
                        //ListView lvcontact = Application.CreateListView("ReportingContact_LookupListView_Email", cs, false);
                        ShowViewParameters showViewParameters = new ShowViewParameters(lvcontact);
                        showViewParameters.CreatedView = lvcontact;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Dc_Acceptingmailselection;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    else if (values[0] == "DoNotDeliver")
                    {
                        HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(values[1]), "Oid");
                        if (HttpContext.Current.Session["rowid"]!=null)
                        {
                            Modules.BusinessObjects.SampleManagement.Reporting objreporting = View.ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[Oid]=?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            IObjectSpace os = Application.CreateObjectSpace();
                            string strGuid = editor.Grid.GetRowValues(int.Parse(values[1]), "Oid").ToString();
                            HttpContext.Current.Session["rowid"] = strGuid;
                            Modules.BusinessObjects.SampleManagement.Reporting obj = os.CreateObject<Modules.BusinessObjects.SampleManagement.Reporting>();
                            obj.DeliveryComments = objreporting.DeliveryComments;
                            DetailView createdView = Application.CreateDetailView(os, "Reporting_DetailView_DoNotDeliver_Reason", false, obj);
                            createdView.ViewEditMode = ViewEditMode.Edit;
                            ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                            showViewParameters.Context = TemplateContext.NestedFrame;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView = createdView;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            showViewParameters.Controllers.Add(dc);
                            dc.Accepting += Dc_Accepting1;
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                        }
                    }

                }
                else if (View.Id == "Invoicing_ListView_Delivery")
                {
                    string[] values = parameter.Split('|');
                    if (values[0] == "Email")
                    {
                        string[] param = parameter.Split('|');
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        //CollectionSource cs = new CollectionSource(objectSpace, typeof(InvoicingContact));
                        CollectionSource cs = new CollectionSource(objectSpace, typeof(Contact));
                        Modules.BusinessObjects.Setting.Invoicing.Invoicing objInv = View.ObjectSpace.FindObject<Modules.BusinessObjects.Setting.Invoicing.Invoicing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (objInv != null && objInv.Client.Oid != null)
                        {
                            cs.Criteria["Filter"] = CriteriaOperator.Parse("Not IsNullOrEmpty([Email]) And [IsInvoice] = True  And [Customer.Oid] = ?", objInv.Client.Oid);
                        }

                        //ListView lvcontact = Application.CreateListView("InvoicingContact_LookupListView_Email", cs, false);
                        ListView lvcontact = Application.CreateListView("Contact_ListView_Email", cs, false);
                        ShowViewParameters showViewParameters = new ShowViewParameters(lvcontact);
                        showViewParameters.CreatedView = lvcontact;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Dc_Acceptingmailselection;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Dc_Accepting1(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                DialogController objDialog = (DialogController)sender as DialogController;
                if (objDialog != null && objDialog.Frame != null && objDialog.Frame.View != null)
                {
                    Modules.BusinessObjects.SampleManagement.Reporting obj = (Modules.BusinessObjects.SampleManagement.Reporting)objDialog.Frame.View.CurrentObject;
                    DevExpress.ExpressApp.View views = objDialog.Frame.View;
                    if (HttpContext.Current.Session["rowid"] != null)
                    {
                        Modules.BusinessObjects.SampleManagement.Reporting objreporting = View.ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("Oid=?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (!string.IsNullOrEmpty(obj.DeliveryComments))
                        {
                            objreporting.DeliveryComments = obj.DeliveryComments;
                            objreporting.DoNotDeliver = true;
                            objreporting.Email = null;
                            objreporting.Mail = false;
                        }
                        else
                        {
                            if (objreporting != null)
                            {
                                objreporting.DeliveryComments = null;
                                objreporting.DoNotDeliver = false;
                                if (objreporting != null && objreporting.JobID.ClientName.Oid != null && (objreporting.Email == null || objreporting.Email.Trim().Length == 0))
                                {
                                    IList<Contact> objconEmail = View.ObjectSpace.GetObjects<Contact>(CriteriaOperator.Parse("Not IsNullOrEmpty([Email]) And [Customer.Oid] = ? And ReportDelivery = true", objreporting.JobID.ClientName.Oid));
                                    if (objconEmail != null && objconEmail.Count > 0)
                                    {
                                        string lstmail = string.Empty;
                                        foreach (Contact objContact in objconEmail)
                                        {
                                            if (!string.IsNullOrEmpty(objContact.Email))
                                            {
                                                if (string.IsNullOrEmpty(lstmail))
                                                {
                                                    lstmail = objContact.Email;
                                                }
                                                else if (!string.IsNullOrEmpty(lstmail))
                                                {
                                                    lstmail = lstmail + "; " + objContact.Email;
                                                }
                                            }
                                        }
                                        objreporting.Email = lstmail;
                                    }
                                }
                            }
                        }
                        if (((ASPxGridListEditor)((ListView)View).Editor).Grid != null)
                        {
                            ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                        }
                        View.Refresh();
                        View.ObjectSpace.CommitChanges();
                    }

                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Acceptingmailselection(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired")
                {
                    if (sender != null)
                    {
                        DialogController dc = (DialogController)sender;
                        if (dc.Window.View != null && dc.Window.View.SelectedObjects.Count == 1)
                        {
                            Modules.BusinessObjects.SampleManagement.Reporting objRpt = ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objRpt != null)
                            {
                                //foreach (ReportingContact objcontact in dc.Window.View.SelectedObjects)
                                foreach (Contact objcontact in dc.Window.View.SelectedObjects)
                                {
                                    //objRpt.EmailList = ObjectSpace.GetObject(objcontact);
                                    objRpt.Email = objcontact.Email;
                                    ObjectSpace.CommitChanges();
                                }
                            }
                        }
                        else if (dc.Window.View != null && dc.Window.View.SelectedObjects.Count > 1)
                        {
                            Modules.BusinessObjects.SampleManagement.Reporting objRpt = View.ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objRpt != null)
                            {
                                string lstmail = string.Empty;
                                //foreach (ReportingContact objContact in dc.Window.View.SelectedObjects)
                                foreach (Contact objContact in dc.Window.View.SelectedObjects)
                                {
                                    if (!string.IsNullOrEmpty(objContact.Email))
                                    {
                                        if (string.IsNullOrEmpty(lstmail))
                                        {
                                            lstmail = objContact.Email;
                                        }
                                        else if (!string.IsNullOrEmpty(lstmail))
                                        {
                                            lstmail = lstmail + ", " + objContact.Email;
                                        }
                                    }
                                }
                                objRpt.Email = lstmail;
                                View.ObjectSpace.CommitChanges();
                                View.ObjectSpace.Refresh();

                            }
                        }
                        else if (dc.Window.View != null && dc.Window.View.SelectedObjects.Count == 0)
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage("Select email ID to continue.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                else if (View.Id == "Invoicing_ListView_Delivery")
                {
                    if (sender != null)
                    {
                        DialogController dc = (DialogController)sender;
                        if (dc.Window.View != null && dc.Window.View.SelectedObjects.Count == 1)
                        {
                            Modules.BusinessObjects.Setting.Invoicing.Invoicing objInv = View.ObjectSpace.FindObject<Modules.BusinessObjects.Setting.Invoicing.Invoicing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objInv != null)
                            {
                                foreach (Contact objcontact in dc.Window.View.SelectedObjects)
                                {
                                    //objRpt.EmailList = ObjectSpace.GetObject(objcontact);
                                    objInv.Email = objcontact.Email;
                                    View.ObjectSpace.CommitChanges();
                                    View.Refresh();
                                }
                            }
                        }
                        else if (dc.Window.View != null && dc.Window.View.SelectedObjects.Count > 1)
                        {
                            Modules.BusinessObjects.Setting.Invoicing.Invoicing objInv = View.ObjectSpace.FindObject<Modules.BusinessObjects.Setting.Invoicing.Invoicing>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                            if (objInv != null)
                            {
                                string lstmail = string.Empty;
                                foreach (Contact objContact in dc.Window.View.SelectedObjects)
                                {
                                    if (!string.IsNullOrEmpty(objContact.Email))
                                    {
                                        if (string.IsNullOrEmpty(lstmail))
                                        {
                                            lstmail = objContact.Email;
                                        }
                                        else if (!string.IsNullOrEmpty(lstmail))
                                        {
                                            lstmail = lstmail + ", " + objContact.Email;
                                        }
                                    }
                                }
                                objInv.Email = lstmail;
                                View.ObjectSpace.CommitChanges();
                                View.ObjectSpace.Refresh();

                            }
                        }
                        else if (dc.Window.View != null && dc.Window.View.SelectedObjects.Count == 0)
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage("Select email ID to continue.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                //string strEmail = string.Empty;
                //foreach (string stremail in)
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
        }

        #region Functions
        private void SetConnectionString()
        {
            try
            {
                AppSettingsReader config = new AppSettingsReader();
                string serverType, server, database, user, password;
                string[] connectionstring = ObjReportDesignerInfo.WebConfigConn.Split(';');
                ObjReportDesignerInfo.LDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLUserID = connectionstring[2].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLPassword = connectionstring[3].Split('=').GetValue(1).ToString();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void ReadXmlFile_FTPConc()
        {
            try
            {
                string[] FTPconnectionstring = ObjReportDesignerInfo.WebConfigFTPConn.Split(';');
                strFTPServerName = FTPconnectionstring[0].Split('=').GetValue(1).ToString();
                strFTPUserName = FTPconnectionstring[1].Split('=').GetValue(1).ToString();
                strFTPPassword = FTPconnectionstring[2].Split('=').GetValue(1).ToString();
                strFTPPath = FTPconnectionstring[3].Split('=').GetValue(1).ToString();
                FTPPort = Convert.ToInt32(FTPconnectionstring[4].ToString().Split('=').GetValue(1).ToString());
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        //public Rebex.Net.Ftp GetFTPConnection()
        //{
        //    try
        //    {
        //        Rebex.Net.Ftp FTP = new Rebex.Net.Ftp();
        //        FTP.TransferType = FtpTransferType.Binary;
        //        if ((!(strFTPServerName == null)
        //                    && ((strFTPServerName.Length > 0)
        //                    && (!(strFTPUserName == null)
        //                    && (strFTPUserName.Length > 0)))))
        //        {
        //            try
        //            {
        //                FTP.Timeout = 6000;
        //                FTP.Connect(strFTPServerName, FTPPort);
        //                FTP.Login(strFTPUserName, strFTPPassword);
        //                strFTPStatus = true;
        //            }
        //            catch (Exception ex)
        //            {
        //                strFTPStatus = false;
        //                return new Rebex.Net.Ftp();
        //            }
        //        }
        //        else
        //        {
        //            strFTPStatus = false;
        //            return new Rebex.Net.Ftp();
        //        }

        //        return FTP;
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //        return null;
        //    }
        //}
        #endregion

        private void ReportSentEmail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        foreach (Modules.BusinessObjects.SampleManagement.Reporting objReport in e.SelectedObjects)
                        {
                            SmtpClient sc = new SmtpClient();
                            Employee currentUser = SecuritySystem.CurrentUser as Employee;
                            string strlogUsername = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).UserName;
                            string strlogpassword = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).Password;
                            string strSmtpHost = "Smtp.gmail.com";
                            string strMailFromUserName = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromUserName"];
                            strMailFromUserName = currentUser.Email;
                            string strMailFromPassword = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromPassword"];
                            strMailFromPassword = currentUser.Password;
                            //string strWebSiteAddress = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["WebSiteAddress"];
                            string strMailto = string.Empty;
                            string strJobID = string.Empty;
                            string reportID = objReport.ReportID;
                            //string FilePath = objFTP.GetDocument(reportID);
                            string strBody;
                            MailMessage message = new MailMessage();
                            message.IsBodyHtml = true;
                            message.From = new MailAddress(strMailFromUserName);
                            eNotificationContentTemplate objEnct = objSpace.FindObject<eNotificationContentTemplate>(CriteriaOperator.Parse("[Reporting] =? ", objReport.Oid));
                            if (objEnct == null)
                            {
                                CriteriaOperator cs = CriteriaOperator.Parse("[SampleCheckin] Is  Null");
                                eNotificationContentTemplate objent = ObjectSpace.FindObject<eNotificationContentTemplate>(cs);
                                if (objent == null)
                                {
                                    objent = objSpace.CreateObject<eNotificationContentTemplate>();
                                    objent.Body = "Report Delivery";
                                }
                                strBody = objent.Body;
                                if (strBody.ToUpper().Contains("@JOBID") && objReport.JobID != null)
                                {
                                    strBody = strBody.Replace("@JobID", objReport.JobID.JobID);
                                }
                                if (strBody.ToUpper().Contains("@TAT") && objReport.JobID != null && objReport.JobID.TAT != null)
                                {
                                    strBody = strBody.Replace("@TAT", objReport.JobID.TAT.TAT);
                                }
                                if (strBody.ToUpper().Contains("@PROJECTID") && objReport.JobID != null && objReport.JobID.ProjectID != null)
                                {
                                    strBody = strBody.Replace("@ProjectID", objReport.JobID.ProjectID.ProjectId);
                                }
                                if (strBody.ToUpper().Contains("@PROJECTNAME") && objReport.JobID != null && objReport.JobID.ProjectName != null)
                                {
                                    strBody = strBody.Replace("@ProjectName", objReport.JobID.ProjectName);
                                }
                                if (strBody.ToUpper().Contains("@RECEIVEDDATE") && objReport.JobID != null && objReport.JobID.RecievedDate != null)
                                {
                                    strBody = strBody.Replace("@ReceivedDate", objReport.JobID.RecievedDate.ToString());
                                }
                                if (objent.ContentType == TypeofContent.None)
                                {
                                    strBody = "The Attached PDF Report is for Samples received on " + objReport.JobID.RecievedDate.ToShortDateString() + " submitted with Chain of Custody " + objReport.JobID.JobID + " processed by the Red River Authority of Texas Environmental Services Laboratory in Wichita Falls, Texas.";

                                }
                                objEnct = objSpace.CreateObject<eNotificationContentTemplate>();
                                objEnct.Body = strBody;
                                objEnct.Subject = objent.Subject;
                                objEnct.SampleCheckin = objSpace.GetObject(objReport.JobID);
                                message.Subject = objEnct.Subject;
                                message.Body = objEnct.Body;
                            }
                            else
                            {
                                message.Subject = objEnct.Subject;
                                strBody = objEnct.Body;
                                message.Body = objEnct.Body;
                            }
                            //Modules.BusinessObjects.SampleManagement.Reporting objrept = (Modules.BusinessObjects.SampleManagement.Reporting)View.CurrentObject;
                            if (objReport != null && !string.IsNullOrEmpty(objReport.Email))
                            {
                                string[] strrptemailarr = objReport.Email.Split(';');
                                foreach (string stremail in strrptemailarr)
                                {
                                    message.To.Add(stremail);
                                }
                            }
                            Contact con = objSpace.GetObject<Contact>(objReport.JobID.ContactName);
                            if (con != null && !string.IsNullOrEmpty(con.Email))
                            {
                                message.To.Add(con.Email);
                            }
                            if (message.To.Count == 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "EmailAddress"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }

                            //ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                            //ReadXmlFile_FTPConc();
                            ////Rebex.Net.Ftp _FTP = GetFTPConnection();
                            //string strRemotePath = ConfigurationManager.AppSettings["FinalReportPath"];
                            //string strExportedPath = strRemotePath.Replace(@"\", "//") + reportID + ".pdf";
                            FileLinkSepDBController objfilelink = Frame.GetController<FileLinkSepDBController>();
                            if (objfilelink!= null)
                            {
                                DataTable dt = objfilelink.GetFileLink(objReport.ReportID);

                                byte[] objbyte = (byte[])dt.Rows[0]["FileContent"];

                                MemoryStream ms = new MemoryStream(objbyte);
                                //_FTP.TransferType = FtpTransferType.Binary;
                                //_FTP.GetFile(strExportedPath, ms);
                                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain);

                                MemoryStream pdfstream = new MemoryStream(ms.ToArray());
                                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(pdfstream, reportID + ".pdf");
                                //attachment.ContentDisposition.FileName = reportID + ".pdf";
                                message.Attachments.Add(attachment);


                            }
                            NetworkCredential credential = new NetworkCredential();
                            credential.UserName = strMailFromUserName;
                            credential.Password = strMailFromPassword;
                            sc.UseDefaultCredentials = true;
                            sc.Host = strSmtpHost;
                            //sc.Port = 25;
                            sc.Port = Convert.ToInt32(((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailPort"]);
                            sc.EnableSsl = true;
                            sc.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                            sc.DeliveryFormat = SmtpDeliveryFormat.SevenBit;
                            sc.Credentials = new NetworkCredential(strMailFromUserName, strMailFromPassword);


                            //sc.EnableSsl = true;
                            //NetworkCredential credential = new NetworkCredential();
                            //credential.UserName = strMailFromUserName;
                            //credential.Password = strMailFromPassword;
                            //sc.Credentials = credential;
                            //sc.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                            //sc.DeliveryFormat = SmtpDeliveryFormat.SevenBit;
                            //sc.Host = strSmtpHost;
                            ////sc.Port = 25;
                            //sc.Port = Convert.ToInt32(((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailPort"]);

                            //sc.EnableSsl = true;

                            //sc.Port = 587;
                            try
                            {
                                if (message.To != null && message.To.Count > 0)
                                {
                                    sc.Send(message);
                                }
                            }
                            catch (SmtpFailedRecipientsException ex)
                            {
                                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                                {
                                    SmtpStatusCode exstatus = ex.InnerExceptions[i].StatusCode;
                                    if (exstatus == SmtpStatusCode.GeneralFailure || exstatus == SmtpStatusCode.ServiceNotAvailable || exstatus == SmtpStatusCode.SyntaxError || exstatus == SmtpStatusCode.SystemStatus || exstatus == SmtpStatusCode.TransactionFailed)
                                    {
                                        Application.ShowViewStrategy.ShowMessage(ex.Message);
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage(ex.InnerExceptions[i].FailedRecipient);
                                    }
                                }
                            }
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "mailsendsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            objSpace.CommitChanges();
                            View.ObjectSpace.CommitChanges();

                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
                else if (View.Id == "Invoicing_ListView_Delivery")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        foreach (Modules.BusinessObjects.Setting.Invoicing.Invoicing objInvoice in e.SelectedObjects)
                        {
                            SmtpClient sc = new SmtpClient();
                            Employee currentUser = SecuritySystem.CurrentUser as Employee;
                            string strlogUsername = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).UserName;
                            string strlogpassword = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).Password;
                            string strSmtpHost = "Smtp.gmail.com";
                            string strMailFromUserName = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromUserName"];
                            //strMailFromUserName = currentUser.Email;
                            string strMailFromPassword = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromPassword"];
                            //strMailFromPassword = currentUser.Password;
                            //string strWebSiteAddress = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["WebSiteAddress"];
                            string strMailto = string.Empty;
                            string strJobID = string.Empty;
                            string invoiceID = objInvoice.InvoiceID;
                            //string FilePath = objFTP.GetDocument(invoiceID);
                            string strBody = string.Empty;
                            MailMessage message = new MailMessage();
                            message.IsBodyHtml = true;
                            message.From = new MailAddress(strMailFromUserName);
                            eNotificationContentTemplate objEnct = objSpace.FindObject<eNotificationContentTemplate>(CriteriaOperator.Parse("[Invoice] =? ", objInvoice.Oid));

                            if (objEnct == null)
                            {
                                CriteriaOperator cs = CriteriaOperator.Parse("[Invoice] Is Null And [ContentType] = 'Invoice'");
                                eNotificationContentTemplate objent = objSpace.FindObject<eNotificationContentTemplate>(cs);
                                if (objent == null)
                                {
                                    objent = objSpace.CreateObject<eNotificationContentTemplate>();
                                    objent.Body = "Invoice Deliverd Successfully.";
                                    // objent.Body = "The Attached PDF Report is for Samples received on " + objInvoice.DateReceived.ToShortDateString() + " submitted with Chain of Custody " + objInvoice.JobID + " processed by the Red River Authority of Texas Environmental Services Laboratory in Wichita Falls, Texas.";

                                }
                                strBody = objent.Body;
                                if (strBody.ToUpper().Contains("@JOBID") && objInvoice.JobID != null)
                                {
                                    strBody = strBody.Replace("@JobID", objInvoice.JobID);
                                }
                                if (strBody.ToUpper().Contains("@INVOICEID") && objInvoice.InvoiceID != null)
                                {
                                    strBody = strBody.Replace("@InvoiceID", objInvoice.InvoiceID);
                                }
                                if (strBody.ToUpper().Contains("@QUOTEID") && objInvoice.QuoteID != null)
                                {
                                    strBody = strBody.Replace("@QuoteID", objInvoice.QuoteID.QuoteID);
                                }
                                //objEnct.Body = objent.Body;
                                //objEnct.Subject = objent.Subject;
                                objent.Invoice = objSpace.GetObject(objInvoice);
                                message.Subject = objent.Subject;
                                message.Body = objent.Body;
                            }
                            else
                            {
                                message.Subject = objEnct.Subject;
                                message.Body = objEnct.Body;
                            }

                            //Modules.BusinessObjects.Setting.Invoicing.Invoicing objInv = (Modules.BusinessObjects.Setting.Invoicing.Invoicing)View.CurrentObject;
                            if (objInvoice != null && !string.IsNullOrEmpty(objInvoice.Email))
                            {
                                string[] strrptemailarr = objInvoice.Email.Split(';');
                                foreach (string stremail in strrptemailarr)
                                {
                                    message.To.Add(stremail);
                                }
                            }

                            //foreach (Contact con in objInvoice.Client.Contacts.ToList().Where(i => !string.IsNullOrEmpty(i.Email)))
                            //{
                            //    message.To.Add(con.Email);
                            //}
                            if (message.To.Count == 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "mailnotsent"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            //ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                            //ReadXmlFile_FTPConc();
                            //Rebex.Net.Ftp _FTP = GetFTPConnection();
                            //string strRemotePath = ConfigurationManager.AppSettings["FinalReportPath"];
                            //string strExportedPath = strRemotePath.Replace(@"\", "//") + invoiceID + ".pdf";
                            ////if (/*_FTP.FileExists(strExportedPath)*/)
                            {
                                MemoryStream ms = new MemoryStream();
                                //_FTP.TransferType = FtpTransferType.Binary;
                                //_FTP.GetFile(strExportedPath, ms);
                                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain);

                                MemoryStream pdfstream = new MemoryStream(ms.ToArray());
                                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(pdfstream, invoiceID + ".pdf");
                                //attachment.ContentDisposition.FileName = reportID + ".pdf";
                                message.Attachments.Add(attachment);

                                //using (FileStream fs = File.OpenRead(_FTP.GetFile(strExportedPath,fs))
                                //{
                                //    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(fs, ct);
                                //    attachment.ContentDisposition.FileName = reportID + ".pdf";
                                //    message.Attachments.Add(attachment);
                                //    //message.Attachment.SaveToStream(ms); for get the fileData into Stream
                                //}
                            }
                            if (objInvoice.Report != null)
                            {
                                MemoryStream pdfstream = new MemoryStream(objInvoice.Report);
                                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(pdfstream, invoiceID + ".pdf");
                                //attachment.ContentDisposition.FileName = reportID + ".pdf";
                                message.Attachments.Add(attachment);
                            }

                            NetworkCredential credential = new NetworkCredential();
                            credential.UserName = strMailFromUserName;
                            credential.Password = strMailFromPassword;
                            sc.UseDefaultCredentials = true;
                            sc.Host = strSmtpHost;
                            //sc.Port = 25;
                            sc.Port = Convert.ToInt32(((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailPort"]);
                            sc.EnableSsl = true;
                            sc.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                            sc.DeliveryFormat = SmtpDeliveryFormat.SevenBit;
                            sc.Credentials = new NetworkCredential(strMailFromUserName, strMailFromPassword);

                            //sc.EnableSsl = true;
                            //sc.UseDefaultCredentials = false;
                            //NetworkCredential credential = new NetworkCredential();
                            //credential.UserName = strMailFromUserName;
                            //credential.Password = strMailFromPassword;
                            //sc.Credentials = credential;
                            //sc.Host = strSmtpHost;
                            //sc.Port = 25;
                            //sc.Port = Convert.ToInt32(((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailPort"]);
                            ////sc.Port = 587;
                            try
                            {
                                if (message.To != null && message.To.Count > 0)
                                {
                                    sc.Send(message);
                                    objInvoice.Status = Modules.BusinessObjects.Setting.Invoicing.InviceStatus.Delivered;
                                    objInvoice.DateSend = DateTime.Now;
                                    objInvoice.RollbackedBy = null;
                                    objInvoice.RollbackedDate = DateTime.MinValue;
                                    objInvoice.RollbackReason = null;
                                    objInvoice.SentBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    Session currentSession = ((XPObjectSpace)objSpace).Session;
                                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                    Deposits objOldDeposit = uow.FindObject<Deposits>(CriteriaOperator.Parse("[InvoiceID]=?", objInvoice.Oid));
                                    if (objOldDeposit == null)
                                    {
                                        Invoicing objOldInvoice = uow.GetObjectByKey<Invoicing>(objInvoice.Oid);
                                        Deposits objNewDeposit = new Deposits(uow);
                                        objNewDeposit.InvoiceID = objOldInvoice;
                                        objNewDeposit.DueDate = objOldInvoice.DueDate;
                                        objNewDeposit.Amount = objOldInvoice.Amount;
                                        objNewDeposit.Client = objOldInvoice.Client;
                                        //objNewDeposit.Status = DepositStatus.Unpaid;
                                        objNewDeposit.DateInvoiced = objOldInvoice.DateInvoiced;
                                        objNewDeposit.Save();
                                        uow.CommitChanges();
                                    }
                                    else
                                    {
                                        Invoicing objOldInvoice = uow.GetObjectByKey<Invoicing>(objInvoice.Oid);
                                        objOldDeposit.InvoiceID = objOldInvoice;
                                        objOldDeposit.DueDate = objOldInvoice.DueDate;
                                        objOldDeposit.Amount = objOldInvoice.Amount;
                                        objOldDeposit.Client = objOldInvoice.Client;
                                        //objOldDeposit.Status = DepositStatus.Unpaid;
                                        objOldDeposit.DateInvoiced = objOldInvoice.DateInvoiced;
                                        objOldDeposit.Save();
                                        uow.CommitChanges();
                                    }
                                }
                            }
                            catch (SmtpFailedRecipientsException ex)
                            {
                                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                                {
                                    SmtpStatusCode exstatus = ex.InnerExceptions[i].StatusCode;
                                    if (exstatus == SmtpStatusCode.GeneralFailure || exstatus == SmtpStatusCode.ServiceNotAvailable || exstatus == SmtpStatusCode.SyntaxError || exstatus == SmtpStatusCode.SystemStatus || exstatus == SmtpStatusCode.TransactionFailed)
                                    {
                                        Application.ShowViewStrategy.ShowMessage(ex.Message);
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage(ex.InnerExceptions[i].FailedRecipient);
                                    }
                                }
                            }
                            Modules.BusinessObjects.SampleManagement.Samplecheckin objSamplecheckin = ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", objInvoice.JobID));
                            IList<SampleParameter> lstSamples = View.ObjectSpace.GetObjects<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                            if (lstSamples.Where(i => i.Status == Samplestatus.PendingValidation).Count() == 0 && lstSamples.Where(i => i.Status == Samplestatus.PendingReporting).Count() == 0 && lstSamples.Where(i => i.Status == Samplestatus.PendingApproval).Count() == 0 && lstSamples.Where(i => i.Status == Samplestatus.PendingEntry).Count() == 0)
                            {
                                StatusDefinition objStatus = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 28"));
                                if (objStatus != null)
                                {
                                    objSamplecheckin.Index = objStatus;
                                }
                                ObjectSpace.CommitChanges();
                            }

                            #region SuboutStatus
                            //List<Samplecheckin> lstSampleCheckins = objSpace.GetObjects<Samplecheckin>(new InOperator("JobID", objInvoice.JobID.Split(';').ToList().Select(i => i = i.Trim()).ToList())).ToList();
                            //IList<SampleParameter> lstSample = objSpace.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin.JobID.Oid", lstSampleCheckins.Select(i => i.Oid).Distinct().ToList()), (CriteriaOperator.Parse("[Status] = 'Reported' And [InvoiceIsDone] = True  And [Samplelogin.ExcludeInvoice] = False"))));
                            //if (lstSample.ToList().FirstOrDefault(i => i.SubOut == true) != null)
                            //{
                            //    foreach (SubOutSampleRegistrations objSubout in lstSample.Where(i => i.SubOut == true).Select(i => i.SuboutSample).Distinct().ToList())
                            //    {
                            //        if (objSubout.SampleParameter.ToList().FirstOrDefault(i => i.Status == Samplestatus.PendingEntry || i.Status == Samplestatus.SuboutPendingValidation || i.Status == Samplestatus.SuboutPendingApproval || i.Status == Samplestatus.PendingReporting) == null)
                            //        {
                            //            if (objSubout.SubOutQcSample.ToList().FirstOrDefault(i => i.Status == Samplestatus.PendingEntry || i.Status == Samplestatus.SuboutPendingValidation || i.Status == Samplestatus.SuboutPendingApproval) == null)
                            //            {
                            //                SubOutSampleRegistrations obj = objSpace.FindObject<SubOutSampleRegistrations>(CriteriaOperator.Parse("[Oid]=?", objSubout.Oid));
                            //                if (obj != null)
                            //                {
                            //                    obj.SuboutStatus = SuboutTrackingStatus.InvoiceDelivered;
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                            #endregion
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "mailsendsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        objSpace.CommitChanges();
                        ObjectSpace.CommitChanges();
                        ObjectSpace.Refresh();
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>()
                    .InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
