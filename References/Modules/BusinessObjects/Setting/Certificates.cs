using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.Hr
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Certificates : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Certificates(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            LastUpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            LastUpdatedDate = Library.GetServerTime(Session);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            LastUpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            LastUpdatedDate = Library.GetServerTime(Session);
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
        #region Item
        private string _Item;
        public string Item
        {
            get
            {
                return _Item;
            }
            set
            {
                SetPropertyValue("Item", ref _Item, value);
            }
        }
        #endregion

        #region Text
        private string _Text;
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                SetPropertyValue("Text", ref _Text, value);
            }
        }
        #endregion

        #region ExpirationDate
        private DateTime _ExpirationDate;
        public DateTime ExpirationDate
        {
            get
            {
                return _ExpirationDate;
            }
            set
            {
                SetPropertyValue("ExpirationDate", ref _ExpirationDate, value);
            }
        }
        #endregion

        #region Comment
        private string _Comment;
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                SetPropertyValue("Comment", ref _Comment, value);
            }
        }
        #endregion

        #region LastUpdatedDate
        private DateTime _LastUpdatedDate;
        public DateTime LastUpdatedDate
        {
            get
            {
                return _LastUpdatedDate;
            }
            set
            {
                SetPropertyValue("LastUpdatedDate", ref _LastUpdatedDate, value);
            }
        }
        #endregion

        #region LastUpdatedBy
        private Employee _LastUpdatedBy;
        public Employee LastUpdatedBy
        {
            get
            {
                return _LastUpdatedBy;
            }
            set
            {
                SetPropertyValue("LastUpdatedBy", ref _LastUpdatedBy, value);
            }
        }
        #endregion

        private Company Companies;
        [Association]
        public Company Company
        {
            get { return Companies; }
            set { SetPropertyValue(nameof(Company), ref Companies, value); }
        }
    }
}