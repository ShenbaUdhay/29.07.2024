using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    [DefaultProperty("storage")]

    public class ICMStorage : BaseObject
    {
        #region Constructor
        public ICMStorage(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        #endregion

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

        #region storage
        string fStorage;
        [RuleRequiredField("storage", DefaultContexts.Save,"Storage must not be empty")]
        // [RuleStringComparison("RuleStringComparison_Storage_NotNullOrEmpty", DefaultContexts.Save, StringComparisonType.NotEquals, null)]
        [RuleUniqueValue]
        public string storage
        {
            get { return fStorage; }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetPropertyValue<string>("storage", ref fStorage, value.Trim());
            }
        }
        #endregion

        #region location
        string fLocation;
        [RuleRequiredField("Location", DefaultContexts.Save)]
        public string Location
        {
            get { return fLocation; }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetPropertyValue<string>("Location", ref fLocation, value.Trim());
            }
        }
        #endregion

        #region comment
        string fComment;
        [Size(1000)]
        public string Comment
        {
            get { return fComment; }
            set { SetPropertyValue<string>("Comment", ref fComment, value); }
        }
        #endregion

        #region active
        private Boolean fActive;
        public Boolean Active
        {
            get { return fActive; }
            set { SetPropertyValue<Boolean>("Active", ref fActive, value); }
        }
        #endregion

        #region ModifiedBy
        private Employee fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
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
        //[Browsable(false)]
        //[Appearance("MD9", Enabled = false, Context = "DetailView")]       
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
    }
}