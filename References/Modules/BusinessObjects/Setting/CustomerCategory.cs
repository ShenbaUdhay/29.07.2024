using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Crm
{
    [DefaultClassOptions]
    [DefaultProperty("Category")]

    public class CustomerCategory : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CustomerCategory(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            EnteredBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            EnteredDate = Library.GetServerTime(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }
        #region OnDelete
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                foreach (BaseObject obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.Oid != null)
                    {
                        Exception ex = new Exception("Already Used Can't allow to Delete");
                        throw ex;
                        break;

                    }
                }
            }
        }
        #endregion
        #region Category
        private string _Category;
        [RuleRequiredField]
        [RuleUniqueValue]
        public string Category
        {
            get
            {
                return _Category;
            }
            set
            {
                SetPropertyValue("Category", ref _Category, value.Trim());
            }
        }
        #endregion

        #region Description
        private string _description;
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                SetPropertyValue("Description", ref _description, value);
            }
        }
        #endregion

        #region EnteredBy
        private Employee _EnteredBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee EnteredBy
        {
            get
            {
                return _EnteredBy;
            }
            set
            {
                SetPropertyValue("EnteredBy", ref _EnteredBy, value);
            }
        }
        #endregion

        #region EnteredDate
        private DateTime __EnteredBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime EnteredDate
        {
            get
            {
                return __EnteredBy;
            }
            set
            {
                SetPropertyValue("EnteredDate", ref __EnteredBy, value);
            }
        }
        #endregion

        #region ModifiedBy
        private Employee _modifiedby;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee ModifiedBy
        {
            get
            {
                return _modifiedby;
            }
            set
            {
                SetPropertyValue("ModifiedBy", ref _modifiedby, value);
            }
        }
        #endregion

        #region ModifiedDate
        private DateTime _modifieddate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get
            {
                return _modifieddate;
            }
            set
            {
                SetPropertyValue("ModifiedDate", ref _modifieddate, value);
            }
        }
        #endregion
    }
}