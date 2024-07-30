using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
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
    public class QCResultValidationQueryPanel : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public QCResultValidationQueryPanel(Session session)
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
        #region JobID
        private Samplecheckin _JobID;
        [ImmediatePostData]
        public Samplecheckin JobID
        {
            get
            {
                return _JobID;
            }
            set
            {
                SetPropertyValue<Samplecheckin>("JobID", ref _JobID, value);
            }
        }
        #endregion

        #region Test
        private TestMethod fTest;
        //[Browsable(false)]
        public TestMethod TestName
        {
            get
            {
                return fTest;
            }
            set
            {
                SetPropertyValue("TestName", ref fTest, value);
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

        #region SampleID
        private SampleLogIn fSampleID;
        //[Browsable(false)]
        public SampleLogIn SampleID
        {
            get
            {
                return fSampleID;
            }
            set
            {
                SetPropertyValue("SampleID", ref fSampleID, value);
            }
        }
        #endregion

        #region strSampleID
        private string _strSampleID;
        public string strSampleID
        {
            get { return _strSampleID; }
            set
            {
                SetPropertyValue<string>("strSampleID", ref _strSampleID, value);
            }
        }

        #endregion

        #region QuerySelectionMode
        private QueryMode _SelectionMode;
        [XafDisplayName("Select")]
        [VisibleInDashboards(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ImmediatePostData]
        public QueryMode SelectionMode
        {
            get { return _SelectionMode; }
            set { SetPropertyValue<QueryMode>(nameof(SelectionMode), ref _SelectionMode, value); }
        }
        #endregion


    }
}