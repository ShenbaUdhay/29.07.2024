using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.IO;
using System.Net.Mail;

namespace Modules.BusinessObjects.E_Mail
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [FileAttachment("Attachment")]
    public class Email : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Email(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            //from = "mathanagopalv.cvr@gmail.com";

            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            from = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Email;
            if (From == null)
            {
                WebWindow.CurrentRequestWindow.Application.ShowViewStrategy.ShowMessage("The user doesn't have EmailID", InformationType.Warning, 3000, InformationPosition.Top);
                return;
            }


        }
        protected override void OnSaving()
        {
            base.OnSaving();

        }
        //private Label label;
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
        [Persistent("From")]

        private string from;

        [PersistentAlias("from")]

        public string From
        {

            get
            {
                if (from != null)
                {
                    return from;
                }
                else
                {
                    //label.Text
                    return null;
                    //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "reportvalidate"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                }


            }


        }

        [Association("To-Contact", UseAssociationNameAsIntermediateTableName = true)]

        public XPCollection<Contact> To
        {

            get { return GetCollection<Contact>("To"); }

        }

        [Association("CC-Contact", UseAssociationNameAsIntermediateTableName = true)]

        public XPCollection<Contact> CC
        {

            get { return GetCollection<Contact>("CC"); }

        }

        [Association("Bcc-Contact", UseAssociationNameAsIntermediateTableName = true)]

        public XPCollection<Contact> Bcc
        {

            get { return GetCollection<Contact>("Bcc"); }

        }

        public string Subject
        {

            get { return GetPropertyValue<string>("Subject"); }

            set
            {

                SetPropertyValue<string>("Subject", value);

            }

        }

        [Size(SizeAttribute.Unlimited)]

        public string Body
        {

            get { return GetPropertyValue<string>("Body"); }

            set
            {

                SetPropertyValue<string>("Body", value);

            }

        }

        //[DevExpress.Xpo.Aggregated]

        public FileData Attachment
        {

            get { return GetPropertyValue<FileData>("Attachment"); }

            set
            {

                SetPropertyValue<FileData>("Attachment", value);

            }
        }

        public MailStatus Status
        {

            get { return GetPropertyValue<MailStatus>("Status"); }

            set
            {

                SetPropertyValue<MailStatus>("Status", value);

            }

        }
        #region ModifiedBy
        private Employee fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee ModifiedBy
        {
            get
            {
                return fModifiedBy;
            }
            set
            {
                SetPropertyValue("ModifiedBy", ref fModifiedBy, value);
            }
        }
        #endregion




        #region ModifiedDate
        private DateTime fModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get
            {
                return fModifiedDate;
            }
            set
            {
                SetPropertyValue("ModifiedDate", ref fModifiedDate, value);
            }
        }
        #endregion

        public string SendMail()
        {

            try
            {

                SmtpClient smtp = new SmtpClient();

                MailMessage message = new MailMessage();

                // Add hostname smtpserver.

                smtp.Host = "Smtp.gmail.com";

                // Add smtp server port.

                smtp.Port = 25;

                // Add credentials if the SMTP server requires them.

                //smtp.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential("mathanagopalv.cvr@gmail.com", "btlims@1");

                message.Subject = Subject;

                message.Body = Body;

                message.From = new MailAddress(From, "LDM");

                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                message.Priority = MailPriority.Normal;

                // Make the list of recipients.

                if (To.Count > 0)
                {

                    foreach (Contact item in To)
                    {

                        message.To.Add(new MailAddress(item.Email, item.FullName));

                    }

                }

                if (CC.Count > 0)
                {

                    foreach (Contact item in CC)
                    {

                        message.CC.Add(new MailAddress(item.Email, item.FullName));

                    }

                }

                if (Bcc.Count > 0)
                {

                    foreach (Contact item in Bcc)
                    {

                        message.Bcc.Add(new MailAddress(item.Email, item.FullName));

                    }

                }

                // Create the file attachment (from the File property) for this e-mail message.

                if (Attachment != null)
                {

                    string tempFileName = Oid.ToString();

                    using (FileStream fileStream = new FileStream(tempFileName, FileMode.OpenOrCreate))
                    {

                        Attachment.SaveToStream(fileStream);

                        fileStream.Position = 0;

                        // Create attachment by using existing fileStream.

                        Attachment data = new Attachment(fileStream, System.Net.Mime.MediaTypeNames.Application.Octet);

                        // Add time stamp information for the file.

                        System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;

                        disposition.FileName = Attachment.FileName;

                        disposition.Size = fileStream.Length;

                        disposition.CreationDate = System.IO.File.GetCreationTime(tempFileName);

                        disposition.ModificationDate = System.IO.File.GetLastWriteTime(tempFileName);

                        disposition.ReadDate = System.IO.File.GetLastAccessTime(tempFileName);

                        // Add the attachment to this message.

                        message.Attachments.Add(data);

                        // Send the message.

                        smtp.Send(message);
                        Status = MailStatus.Success;
                        ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        ModifiedDate = Library.GetServerTime(Session);
                        data.Dispose();

                        // Delete temp file.

                        if (System.IO.File.Exists(tempFileName))
                        {

                            System.IO.File.Delete(tempFileName);

                        }

                    }

                }

            }
            catch (SmtpException E)
            {
                Status = MailStatus.Failed;
                return "Mail send failed with message: " + E.Message;

            }

            return "Mail was send successfully";

        }
    }


    public enum MailStatus
    {
        None = 0,
        Success = 1,
        Failed = 2
    }
}