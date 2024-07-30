using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting.CCID;
using System;

namespace Modules.BusinessObjects.Setting.NCAID
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CorrectiveActionVerification : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        CorrectiveActionVerificationInfo objCAVInfo = new CorrectiveActionVerificationInfo();
        public CorrectiveActionVerification(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            NonConformityInitiation = Session.GetObjectByKey<NonConformityInitiation>(objCAVInfo.CorrectiveActionVerificationOid);
            CompliantInitiation = Session.GetObjectByKey<CompliantInitiation>(objCAVInfo.CorrectiveActionVerificationOid);
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            Author = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            Date = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { SetPropertyValue(nameof(Date), ref _Date, value); }
        }
        private ActionCategory _ActionCategory;
        public ActionCategory ActionCategory
        {
            get { return _ActionCategory; }
            set { SetPropertyValue(nameof(ActionCategory), ref _ActionCategory, value); }
        }
        private string _Text;
        [Size(SizeAttribute.Unlimited)]
        public string Text
        {
            get { return _Text; }
            set { SetPropertyValue(nameof(Text), ref _Text, value); }
        }
        private Employee _Author;
        [ImmediatePostData]
        public Employee Author
        {
            get { return _Author; }
            set { SetPropertyValue(nameof(Author), ref _Author, value); }
        }
        private Position _Position;
        public Position Position
        {
            get
            {
                if (Author != null)
                {
                    _Position = Author.Position;
                }
                return _Position;
            }
            set { SetPropertyValue(nameof(Position), ref _Position, value); }
        }

        private FileData _Attachment;
        public FileData Attachment
        {
            get { return _Attachment; }
            set { SetPropertyValue(nameof(Attachment), ref _Attachment, value); }
        }
        private NonConformityInitiation _NonConformityInitiation;
        [Association("NonConformityInitiation-CorrectiveActionVerification")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public NonConformityInitiation NonConformityInitiation
        {
            get
            {
                return _NonConformityInitiation;
            }
            set
            {
                SetPropertyValue("NonConformityInitiation", ref _NonConformityInitiation, value);
            }
        }
        private CompliantInitiation _CompliantInitiation;
        [Association("CompliantInitiation-CorrectiveActionVerification")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public CompliantInitiation CompliantInitiation
        {
            get
            {
                return _CompliantInitiation;
            }
            set
            {
                SetPropertyValue("CompliantInitiation", ref _CompliantInitiation, value);
            }
        }
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
    }
}