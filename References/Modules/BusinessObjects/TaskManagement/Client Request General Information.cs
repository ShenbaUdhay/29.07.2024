using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;

namespace Modules.BusinessObjects.TaskManagement
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Client_Request_General_Information : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Client_Request_General_Information(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _RequestId;
        public string RequestId
        {
            get { return _RequestId; }
            set { SetPropertyValue<string>(nameof(RequestId), ref _RequestId, value); }
        }
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { SetPropertyValue<DateTime>(nameof(Date), ref _Date, value); }
        }
        private string _CallReceivedBy;
        public string CallReceivedBy
        {
            get { return _CallReceivedBy; }
            set { SetPropertyValue<string>(nameof(CallReceivedBy), ref _CallReceivedBy, value); }
        }
        private string _Client;
        public string Client
        {
            get { return _Client; }
            set { SetPropertyValue<string>(nameof(Client), ref _Client, value); }
        }
        private string _ClientLicense;
        public string ClientLicense
        {
            get { return _ClientLicense; }
            set { SetPropertyValue<string>(nameof(ClientLicense), ref _ClientLicense, value); }
        }
        private string _RequestedBy;
        public string RequestedBy
        {
            get { return _RequestedBy; }
            set { SetPropertyValue<string>(nameof(RequestedBy), ref _RequestedBy, value); }
        }
        private string _ProjectId;
        public string ProjectId
        {
            get { return _ProjectId; }
            set { SetPropertyValue<string>(nameof(ProjectId), ref _ProjectId, value); }
        }
        private string _ProjectName;
        public string ProjectName
        {
            get { return _ProjectName; }
            set { SetPropertyValue<string>(nameof(ProjectName), ref _ProjectName, value); }
        }
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue<string>(nameof(Description), ref _Description, value); }
        }
        private string _SiteAddress;
        public string SiteAddress
        {
            get { return _SiteAddress; }
            set { SetPropertyValue<string>(nameof(SiteAddress), ref _SiteAddress, value); }
        }
        private DateTime _SamplingDate;
        public DateTime SamplingDate
        {
            get { return _SamplingDate; }
            set { SetPropertyValue<DateTime>(nameof(SamplingDate), ref _SamplingDate, value); }
        }
        private Modules.BusinessObjects.Hr.Priority _Priority;
        public Modules.BusinessObjects.Hr.Priority Priority
        {
            get { return _Priority; }
            set { SetPropertyValue(nameof(Priority), ref _Priority, value); }
        }
        private string _Contact;
        public string Contact
        {
            get { return _Contact; }
            set { SetPropertyValue<string>(nameof(Contact), ref _Contact, value); }
        }
        private string _ContactPhone;
        public string ContactPhone
        {
            get { return _ContactPhone; }
            set { SetPropertyValue<string>(nameof(ContactPhone), ref _ContactPhone, value); }
        }

        private FileData _Attachment;
        public FileData Attachment
        {
            get { return _Attachment; }
            set { SetPropertyValue<FileData>(nameof(Attachment), ref _Attachment, value); }
        }
        private string _Status;
        public string Status
        {
            get { return _Status; }
            set { SetPropertyValue<string>(nameof(Status), ref _Status, value); }
        }
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue<string>(nameof(Remark), ref _Remark, value); }
        }

        [Association("Client_Request_General_Information-Client_Request_GridView_Control")]
        public XPCollection<Client_Request_GridView_Control> Client_Request_GridView_Control
        {
            get
            {
                return GetCollection<Client_Request_GridView_Control>(nameof(Client_Request_GridView_Control));
            }
        }
    }
    public enum Priority
    {
        //[DisplayName("N/A")]
        NA,
        [XafDisplayName("As Scheduled")]
        AsScheduled,
        Immediately,
        Regular, Rush
    }
}


