using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.Crm
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ReportingContact : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ReportingContact(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
            LastUpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            LastUpdatedDate = Library.GetServerTime(Session);

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            LastUpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            LastUpdatedDate = Library.GetServerTime(Session);
            if (string.IsNullOrEmpty(ReportContactID))
            {
                CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                CreatedDate = Library.GetServerTime(Session);
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(ReportContactID, 2))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(ReportingContact), criteria, null)) + 1).ToString("00000");
                ReportContactID = "RC" + tempID;
            }
        }
        //protected override void OnSaved()
        //{
        //    base.OnSaved();

        //}

        #region ReportContactID
        private string _ReportContactID;
        public string ReportContactID
        {
            get { return _ReportContactID; }
            set { SetPropertyValue(nameof(ReportContactID), ref _ReportContactID, value); }
        }
        #endregion
        #region ReportContact
        private string _ReportContact;
        [RuleRequiredField]
        public string ReportContact
        {
            get { return _ReportContact; }
            set { SetPropertyValue(nameof(ReportContact), ref _ReportContact, value); }
        }
        #endregion
        #region ReportEmail
        private string _ReportEmail;
        public string ReportEmail
        {
            get { return _ReportEmail; }
            set { SetPropertyValue(nameof(ReportEmail), ref _ReportEmail, value); }
        }
        #endregion
        #region ReportEmailCC
        private string _ReportEmailCC;
        public string ReportEmailCC
        {
            get { return _ReportEmailCC; }
            set { SetPropertyValue(nameof(ReportEmailCC), ref _ReportEmailCC, value); }
        }
        #endregion
        #region ReportEmailBCC
        private string _ReportEmailBCC;
        public string ReportEmailBCC
        {
            get { return _ReportEmailBCC; }
            set { SetPropertyValue(nameof(ReportEmailBCC), ref _ReportEmailBCC, value); }
        }
        #endregion
        #region ReportPhone
        private string _ReportPhone;
        public string ReportPhone
        {
            get { return _ReportPhone; }
            set { SetPropertyValue(nameof(ReportPhone), ref _ReportPhone, value); }
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
        [Association("Customer-ReportingContact")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Customer Customer

        {
            get { return _Customer; }
            set { SetPropertyValue(nameof(Customer), ref _Customer, value); }
        }
        #endregion
    }
}