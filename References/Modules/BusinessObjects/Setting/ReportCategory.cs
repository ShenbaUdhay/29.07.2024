using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    [DefaultProperty("Category")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ReportCategory : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ReportCategory(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
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
        string fCategory;
        [RuleRequiredField("Report.category", DefaultContexts.Save)]
        [RuleUniqueValue]
        public string Category
        {
            get { return fCategory; }
            set { SetPropertyValue<string>("Category", ref fCategory, value); }
        }
        #endregion

        #region Description
        string fDescription;
        [Size(1000)]
        public string Description
        {
            get { return fDescription; }
            set { SetPropertyValue<string>("Description", ref fDescription, value); }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
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