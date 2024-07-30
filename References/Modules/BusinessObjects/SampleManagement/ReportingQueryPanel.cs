using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Setting;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    [NonPersistent]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ReportingQueryPanel : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ReportingQueryPanel(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
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
        #region ReportID
        private Reporting _ReportID;
        public Reporting ReportID
        {
            get { return _ReportID; }
            set
            {
                SetPropertyValue<Reporting>("ReportID", ref _ReportID, value);
            }
        }
        #endregion
        #region JobID
        private Samplecheckin _JobID;
        public Samplecheckin JobID
        {
            get { return _JobID; }
            set
            {
                SetPropertyValue<Samplecheckin>("JobID", ref _JobID, value);
            }
        }
        #endregion
        #region ProjectName
        private Project _ProjectName;
        public Project ProjectName
        {
            get { return _ProjectName; }
            set { SetPropertyValue<Project>("ProjectName", ref _ProjectName, value); }
        }
        #endregion
        #region PeojectID
        private Project _ProjectID;
        public Project ProjectID
        {
            get { return _ProjectID; }
            set
            {
                SetPropertyValue<Project>("ProjectID", ref _ProjectID, value);
            }
        }
        #endregion
        #region ClientName
        private Customer _ClientName;
        public Customer ClientName
        {
            get { return _ClientName; }
            set
            {
                SetPropertyValue<Customer>("ClientName", ref _ClientName, value);
            }
        }
        #endregion

        #region FilterByMonth
        private FilterByMonthEN _FilterDataByMonth = FilterByMonthEN._1M;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public FilterByMonthEN FilterDataByMonth
        {
            get { return _FilterDataByMonth; }
            set { SetPropertyValue("FilterDataByMonth", ref _FilterDataByMonth, value); }
        }
        #endregion

        #region strJobID
        private string _strJobID;
        public string strJobID
        {
            get { return _strJobID; }
            set
            {
                SetPropertyValue<string>("strJobID", ref _strJobID, value);
            }
        }

        #endregion
    }
}