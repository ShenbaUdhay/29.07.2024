using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
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
    //[RuleCriteria("", DefaultContexts.Save, "DaysSampleKeeping > 0", "Days Sample Keeping should be greater than 0", SkipNullOrEmptyValues = false)]
    //if(!string.IsNullOrEmpty(DaysSampleKeeping)
    //{
    //   // [RuleCriteria("", DefaultContexts.Save, "DaysSampleKeeping > 0", "Days Sample Keeping should be greater than 0", SkipNullOrEmptyValues = false)]

    //}
    //[RuleCombinationOfPropertiesIsUnique("Project", DefaultContexts.Save, "ProjectId", SkipNullOrEmptyValues = false)]
    public class Project : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Project(Session session)
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
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}

        #region ProjectID
        private string _ProjectId;
        [RuleRequiredField("ProjectId", DefaultContexts.Save)]
        [XafDisplayName("ProjectID")]
        public string ProjectId
        {
            get { return _ProjectId; }
            set { SetPropertyValue("ProjectId", ref _ProjectId, value); }
        }
        #endregion

        #region Project Location
        private string _projectLocation;
        [Size(300)]
        public string ProjectLocation
        {
            get => _projectLocation;
            set => SetPropertyValue(nameof(ProjectLocation), ref _projectLocation, value);
        }
        #endregion

        #region ProjectName 
        private string _ProjectName;
        //[RuleRequiredField("ProjectName", DefaultContexts.Save)]
        [XafDisplayName("ProjectName")]
        public string ProjectName
        {
            get { return _ProjectName; }
            set { SetPropertyValue("ProjectName", ref _ProjectName, value); }
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
        [Appearance("MD4", Enabled = false, Context = "DetailView")]
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
        [Appearance("MB4", Enabled = false, Context = "DetailView")]
        public CustomSystemUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue("ModifiedBy", ref _ModifiedBy, value); }

        }
        #endregion

        #region Customer

        private Customer _customername;
        [Association("Customer-Projects")]
        public Customer customername
        {
            get { return _customername; }
            set { SetPropertyValue("CustomerName", ref _customername, value); }
        }

        #endregion
        #region ProspectClient

        private ProspectClient _ProspectClient;
        [Association("ProspectClient-Projects")]
        public ProspectClient ProspectClient
        {
            get { return _ProspectClient; }
            set { SetPropertyValue("ProspectClient", ref _ProspectClient, value); }
        }

        #endregion


        #region ProjectCategory
        private ProjectCategory _ProjectCategory;
        public ProjectCategory ProjectCategory
        {
            get { return _ProjectCategory; }
            set { SetPropertyValue("ProjectCategory", ref _ProjectCategory, value); }
        }
        #endregion

        #region ProjectCity
        private string _ProjectCity;
        public string ProjectCity
        {
            get { return _ProjectCity; }
            set { SetPropertyValue("ProjectCity", ref _ProjectCity, value); }
        }
        #endregion

        #region ProjectOverView
        private string _ProjectOverview;
        public string ProjectOverview
        {
            get { return _ProjectOverview; }
            set { SetPropertyValue("ProjectOverview", ref _ProjectOverview, value); }
        }
        #endregion

        #region ProjectSource
        private string _ProjectSource;
        public string ProjectSource
        {
            get { return _ProjectSource; }
            set { SetPropertyValue("ProjectSource", ref _ProjectSource, value); }
        }
        #endregion
        #region Customer-Project Relation
        [Association("Customer-Project")]
        public XPCollection<Customer> Customer
        {
            get { return GetCollection<Customer>("Customer"); }
        }
        #endregion


        #region DaysSampleKeeping
        private string _DaysSampleKeeping;
        public string DaysSampleKeeping
        {
            get
            {
                return _DaysSampleKeeping;
            }
            set
            {
                SetPropertyValue<string>(nameof(DaysSampleKeeping), ref _DaysSampleKeeping, value);
            }
        }
        #endregion

    }
}