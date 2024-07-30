using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    //[RuleCombinationOfPropertiesIsUnique("SampleCategory", DefaultContexts.Save, "SampleCategoryName", SkipNullOrEmptyValues = false)]
    public class SampleCategory : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SampleCategory(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region SampleCategory
        private string _SampleCategory;
        //[RuleRequiredField("SampleCategoryName", DefaultContexts.Save)]
        [RuleRequiredField("SampleCategory.SampleCategoryName", DefaultContexts.Save, "Sample Category must not be empty")]
        public string SampleCategoryName
        {
            get { return _SampleCategory; }
            set { SetPropertyValue("SampleCategory", ref _SampleCategory, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        [Size(1000)]
        public String Comment
        {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }

        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("MD6", Enabled = false, Context = "DetailView")]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }

        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("MB6", Enabled = false, Context = "DetailView")]
        public CustomSystemUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue("ModifiedBy", ref _ModifiedBy, value); }

        }
        #endregion


    }
}