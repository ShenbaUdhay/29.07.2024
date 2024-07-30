using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace ICM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Sort")]
    public class Shippingoptions : BaseObject
    {
        #region Constructor
        public Shippingoptions(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
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

        #region option
        string fOption;
        [RuleRequiredField("option", DefaultContexts.Save)]
        // [RuleStringComparison("RuleStringComparison_ShippingOption_NotNullOrEmpty", DefaultContexts.Save, StringComparisonType.NotEquals, null)]
        [RuleUniqueValue]
        [XafDisplayName("Name")]
        public string option
        {
            get { return fOption; }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetPropertyValue<string>("option", ref fOption, value.Trim());
            }
        }
        #endregion

        #region description
        string fDescription;
        [Size(1000)]
        public string Description
        {
            get { return fDescription; }
            set { SetPropertyValue<string>("Description", ref fDescription, value); }
        }
        #endregion

        #region sort
        private int fSort;
        [VisibleInListView(false)]
        [RuleRequiredField("Sort1", DefaultContexts.Save,"'Name must not be empty'")]
        public int Sort
        {
            get { return fSort; }
            set { SetPropertyValue<int>("Sort", ref fSort, value); }
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

        #region ModifiedBy
        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
        public CustomSystemUser ModifiedBy
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