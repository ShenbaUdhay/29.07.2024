using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting.Invoicing
{
    [DefaultClassOptions]
    public class InvoicingEDDExport : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InvoicingEDDExport(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
        }
        protected override void OnSaving()
        {
            if (string.IsNullOrEmpty(EDDID))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(EDDID, 3))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(InvoicingEDDExport), criteria, null)) + 1).ToString();
                if (tempID != "1")
                {
                    tempID = "IED" + tempID;
                }
                else
                {
                    tempID = "IED" + "1001";
                }
                EDDID = tempID;
            }
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }
        #region EDDID
        private string _EDDID;
        public string EDDID
        {
            get { return _EDDID; }
            set { SetPropertyValue(nameof(EDDID), ref _EDDID, value); }
        }
        #endregion
        #region InvoiceID
        private Invoicing _InvoiceID;
        public Invoicing InvoiceID
        {
            get { return _InvoiceID; }
            set { SetPropertyValue(nameof(InvoiceID), ref _InvoiceID, value); }
        }
        #endregion
        #region DateSent
        private DateTime _DateSent;
        public DateTime DateSent
        {
            get { return _DateSent; }
            set { SetPropertyValue(nameof(DateSent), ref _DateSent, value); }
        }
        #endregion
        #region SentBy
        private Employee _SentBy;
        public Employee SentBy
        {
            get { return _SentBy; }
            set { SetPropertyValue(nameof(SentBy), ref _SentBy, value); }
        }
        #endregion
        #region EDDDetail
        private byte[] _EDDDetail;
        [Size(SizeAttribute.Unlimited)]
        [Browsable(false)]
        public byte[] EDDDetail
        {
            get { return _EDDDetail; }
            set { SetPropertyValue(nameof(EDDDetail), ref _EDDDetail, value); }
        }
        #endregion
        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion
        #region CreatedBy
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion
        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion
        #region ModifiedBy
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        #endregion


    }
}