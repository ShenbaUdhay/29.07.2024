using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Utils;
using LDM.Module.Controllers.Public;
using LDM.Module.Controllers.Public.FTPSetup;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.E_Mail;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
////using Rebex.Net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace LDM.Module.Web.Controllers.E_mail
{    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class EmailViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        DynamicReportDesignerConnection ObjReportDesignerInfo = new DynamicReportDesignerConnection();
        FtpInfo objFTP = new FtpInfo();
        public string strFTPServerName = string.Empty;
        public string strFTPPath = string.Empty;
        public string strFTPUserName = string.Empty;
        public string strFTPPassword = string.Empty;
        public int FTPPort = 0;
        public bool strFTPStatus;


        public EmailViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Email);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            //if(View is DetailView && ((DetailView)View).ViewEditMode==ViewEditMode.View)
            //{
            //    PropertyEditor PE = ((DetailView)View).FindItem("Status") as PropertyEditor;
            //    if(PE !=null)
            //    {
            //        ASPxEnumPropertyEditor EPE =(ASPxEnumPropertyEditor)PE.Control;
            //        E
            //    }
            //}
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void Sent_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string results = string.Empty;

                if (View.SelectedObjects.Count > 0)
                {
                    IObjectSpace objSpace = Application.CreateObjectSpace();
                    foreach (Email email in e.SelectedObjects)
                    {
                        SmtpClient sc = new SmtpClient();
                        string strlogUsername = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).UserName;
                        string strlogpassword = ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).Password;
                        string strSmtpHost = "Smtp.gmail.com";
                        string strMailFromUserName = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromUserName"];
                        string strMailFromPassword = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailFromPassword"];
                        //string strWebSiteAddress = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["WebSiteAddress"];
                        string strMailto = string.Empty;
                        string from = email.From;
                        //string FilePath = objFTP.GetDocument(from);
                        string strBody;
                        MailMessage message = new MailMessage();
                        message.IsBodyHtml = true;
                        message.From = new MailAddress(strMailFromUserName);
                        //objInvoice.Status = InviceStatus.Delivered;
                        message.Subject = email.Subject;
                        message.Body = email.Body;
                        //if (email != null && !string.IsNullOrEmpty(email.From))
                        //{
                        //    string[] strrptemailarr = email.From.Split(';');
                        //    foreach (string stremail in strrptemailarr)
                        //    {
                        //        message.To.Add(stremail);
                        //    }
                        //}
                        foreach (Contact objContact in email.To)
                        {
                            if (objContact != null && !string.IsNullOrEmpty(objContact.Email))
                            {
                                message.To.Add(objContact.Email);
                            }
                            if (message.To.Count == 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "EmailAddress"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }



                        //ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                        //ReadXmlFile_FTPConc();
                        ////Rebex.Net.Ftp _FTP = GetFTPConnection();
                        //string strRemotePath = ConfigurationManager.AppSettings["FinalReportPath"];
                        //string strExportedPath = strRemotePath.Replace(@"\", "//") + "" + ".pdf";
                        FileLinkSepDBController objfilelink = Frame.GetController<FileLinkSepDBController>();
                        if (objfilelink != null)
                        {
                            //IList<Modules.BusinessObjects.SampleManagement.Reporting> lstReporting = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[JobID.JobID] = ?", objsamplecheckin.JobID));
                            //if (_FTP.FileExists(strExportedPath))
                            //foreach (Modules.BusinessObjects.SampleManagement.Reporting objReport in lstReporting)
                            {
                                //DataTable dt = objfilelink.GetFileLink(objReport.ReportID);

                                // List<string> lstreport = new List<string>();
                                //lstreport.Add(Convert.ToString(dt.Rows[0]["FileContent"]));
                                //byte[] objbyte = (byte[])dt.Rows[0]["FileContent"];
                                //objbyte.ToArray();
                                //_FTP.TransferType = FtpTransferType.Binary;
                                //_FTP.GetFile(strExportedPath, ms);
                                ////MemoryStream ms = new MemoryStream();
                                //////_FTP.TransferType = FtpTransferType.Binary;
                                //////_FTP.GetFile(strExportedPath, ms);
                                ////System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain);

                                MemoryStream pdfstream = new MemoryStream();
                            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(pdfstream, "" + ".pdf");
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
                        }

                           
                        if (email.From != null)
                        {
                            MemoryStream pdfstream = new MemoryStream();
                            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(pdfstream, "" + ".pdf");
                            //attachment.ContentDisposition.FileName = reportID + ".pdf";
                            message.Attachments.Add(attachment);
                        }
                        sc.EnableSsl = true;
                        sc.UseDefaultCredentials = false;
                        NetworkCredential credential = new NetworkCredential();
                        credential.UserName = strMailFromUserName;
                        credential.Password = strMailFromPassword;
                        sc.Credentials = credential;
                        sc.Host = strSmtpHost;
                        sc.Port = 25;
                        sc.Port = Convert.ToInt32(((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailPort"]);
                        //sc.Port = 587;
                        try
                        {

                            sc.Send(message);
                            //email.Status = MailStatus.Failed;                                
                            //Application.ShowViewStrategy.ShowMessage(results, InformationType.Error, timer.Seconds, InformationPosition.Top);



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
                        email.Status = MailStatus.Success;
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
            ////objSpace.CommitChanges();
            //ObjectSpace.CommitChanges();
            //ObjectSpace.Refresh();


            //results += "* " + message.Subject + ": " + message.SendMail() + Environment.NewLine;
            //if (message.Status == MailStatus.Success)
            //{
            //    Application.ShowViewStrategy.ShowMessage(results, InformationType.Success, timer.Seconds, InformationPosition.Top);
            //}
            //else if (message.Status == MailStatus.Failed)
            //{
            //    Application.ShowViewStrategy.ShowMessage(results, InformationType.Error, timer.Seconds, InformationPosition.Top);
            //}
            //ObjectSpace.CommitChanges();
            //}
            //}
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ReadXmlFile_FTPConc()
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

        ////private Ftp GetFTPConnection()
        ////{
        ////    try
        ////    {
        ////        Rebex.Net.Ftp FTP = new Rebex.Net.Ftp();
        ////        FTP.TransferType = FtpTransferType.Binary;
        ////        if ((!(strFTPServerName == null)
        ////                    && ((strFTPServerName.Length > 0)
        ////                    && (!(strFTPUserName == null)
        ////                    && (strFTPUserName.Length > 0)))))
        ////        {
        ////            try
        ////            {
        ////                FTP.Timeout = 6000;
        ////                FTP.Connect(strFTPServerName, FTPPort);
        ////                FTP.Login(strFTPUserName, strFTPPassword);
        ////                strFTPStatus = true;
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                strFTPStatus = false;
        ////                return new Rebex.Net.Ftp();
        ////            }
        ////        }
        ////        else
        ////        {
        ////            strFTPStatus = false;
        ////            return new Rebex.Net.Ftp();
        ////        }

        ////        return FTP;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        ////        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        ////        return null;
        ////    }
        //}
    }
}

