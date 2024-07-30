using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using System;

namespace Modules.BusinessObjects.Crm
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class InvoicingContact : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InvoicingContact(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
            LastUpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            LastUpdatedDate = Library.GetServerTime(Session);

            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;

            LastUpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            LastUpdatedDate = Library.GetServerTime(Session);
            if (string.IsNullOrEmpty(InvoiceContactID))
            {
                CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                CreatedDate = Library.GetServerTime(Session);
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(InvoiceContactID, 2))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(InvoicingContact), criteria, null)) + 1).ToString("00000");
                InvoiceContactID = "IC" + tempID;
            }
        }
        #region InvoiceContactID
        private string _InvoiceContactID;
        public string InvoiceContactID
        {
            get { return _InvoiceContactID; }
            set { SetPropertyValue(nameof(InvoiceContactID), ref _InvoiceContactID, value); }
        }
        #endregion
        #region InvoiceContact
        private string _InvoiceContact;
        [RuleRequiredField]
        public string InvoiceContact
        {
            get { return _InvoiceContact; }
            set { SetPropertyValue(nameof(InvoiceContact), ref _InvoiceContact, value); }
        }
        #endregion
        #region InvoiceEmail
        private string _InvoiceEmail;
        public string InvoiceEmail
        {
            get { return _InvoiceEmail; }
            set { SetPropertyValue(nameof(InvoiceEmail), ref _InvoiceEmail, value); }
        }
        #endregion
        #region InvoiceEmailCC
        private string _InvoiceEmailCC;
        public string InvoiceEmailCC
        {
            get { return _InvoiceEmailCC; }
            set { SetPropertyValue(nameof(InvoiceEmailCC), ref _InvoiceEmailCC, value); }
        }
        #endregion
        #region InvoiceEmailBCC
        private string _InvoiceEmailBCC;
        public string InvoiceEmailBCC
        {
            get { return _InvoiceEmailBCC; }
            set { SetPropertyValue(nameof(InvoiceEmailBCC), ref _InvoiceEmailBCC, value); }
        }
        #endregion
        #region InvoicePhone
        private string _InvoicePhone;
        public string InvoicePhone
        {
            get { return _InvoicePhone; }
            set { SetPropertyValue(nameof(InvoicePhone), ref _InvoicePhone, value); }
        }
        #endregion
        #region DefaultContact
        private bool _DefaultContact;
        public bool DefaultContact
        {
            get { return _DefaultContact; }
            set { SetPropertyValue(nameof(DefaultContact), ref _DefaultContact, value); }
        }
        #endregion
        #region LastUpdatedDate
        private DateTime _LastUpdatedDate;
        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { SetPropertyValue(nameof(LastUpdatedDate), ref _LastUpdatedDate, value); }
        }
        #endregion
        #region LastUpdatedBy
        private Employee _LastUpdatedBy;
        public Employee LastUpdatedBy
        {
            get { return _LastUpdatedBy; }
            set { SetPropertyValue(nameof(LastUpdatedBy), ref _LastUpdatedBy, value); }
        }
        #endregion
        #region CreatedBy
        private Employee _CreatedBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion
        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion
        #region Retired
        private bool _Retired;
        public bool Retired
        {
            get { return _Retired; }
            set { SetPropertyValue(nameof(Retired), ref _Retired, value); }
        }
        #endregion
        #region Comment
        private string _Comment;
        [Size(1000)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }
        #endregion
        #region Customer
        private Customer _Customer;
        [Association("Customer-InvoicingContact")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Customer Customer

        {
            get { return _Customer; }
            set { SetPropertyValue(nameof(Customer), ref _Customer, value); }
        }
        #endregion
        #region Notes
        private Notes _Notes;
        [Association("Notes-InvoicingContact")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Notes Notes

        {
            get { return _Notes; }
            set { SetPropertyValue(nameof(Notes), ref _Notes, value); }
        }
        #endregion
    }
}