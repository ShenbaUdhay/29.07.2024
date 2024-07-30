using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SamplingManagement;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CustomDueDate : BaseObject
    {
        TaskManagementInfo taskManagementInfo = new TaskManagementInfo();
        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CustomDueDate(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }
        private Samplecheckin _SampleCheckin;
        [DevExpress.ExpressApp.DC.Aggregated, Association("SampleCheckIn_CustomDueDate")]
        public Samplecheckin SampleCheckin
        {
            get { return _SampleCheckin; }
            set { SetPropertyValue(nameof(SampleCheckin), ref _SampleCheckin, value); }
        }
        //#region SamplingProposal
        //[VisibleInListView(false),VisibleInDetailView(false),VisibleInLookupListView(false)]
        //private SamplingProposal _SamplingProposal;
        //[DevExpress.ExpressApp.DC.Aggregated, Association("SamplingProposal_CustomDueDate")]
        //public SamplingProposal SamplingProposal
        //{
        //    get { return _SamplingProposal; }
        //    set { SetPropertyValue(nameof(SamplingProposal), ref _SamplingProposal, value); }
        //}
        //#endregion
        private COCSettings _COCSettings;
        [DevExpress.ExpressApp.DC.Aggregated, Association("COCSettings_CustomDueDate")]
        public COCSettings COCSettings
        {
            get { return _COCSettings; }
            set { SetPropertyValue(nameof(COCSettings), ref _COCSettings, value); }
        }
        private TestMethod _TestMethod;
        public TestMethod TestMethod
        {
            get { return _TestMethod; }
            set { SetPropertyValue(nameof(TestMethod), ref _TestMethod, value); }
        }
        private Nullable<DateTime> _DueDate;
        //[ImmediatePostData]
        public Nullable<DateTime> DueDate
        {
            get { return _DueDate; }
            set { SetPropertyValue(nameof(DueDate), ref _DueDate, value); }
        }
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        #region TAT
        private TurnAroundTime _TAT;
        public TurnAroundTime TAT
        {
            get { return _TAT; }
            set { SetPropertyValue(nameof(TAT), ref _TAT, value); }
        }
        #endregion

        private VisualMatrix _SampleMatrix;
        public VisualMatrix SampleMatrix
        {
            get { return _SampleMatrix; }
            set { SetPropertyValue(nameof(SampleMatrix), ref _SampleMatrix, value); }
        }
        #region DefaultDueDate
        private DateTime _DefaultDueDate;
        [NonPersistent]
        // [ImmediatePostData]
        public DateTime DefaultDueDate
        {
            get
            {
                //if (SamplingProposal!=null)
                //{
                //    if (SamplingProposal.DueDate != null && SamplingProposal.DueDate!=DateTime.MinValue)
                //    {
                //        _DefaultDueDate =Convert.ToDateTime(SamplingProposal.DueDate);
                //    }
                //}
                //else
                //{

                //}
                if (SampleCheckin != null && SampleCheckin.DueDate != null)
                {
                    _DefaultDueDate = (DateTime)SampleCheckin.DueDate;
                }

                return _DefaultDueDate;
            }
            set { SetPropertyValue(nameof(DefaultDueDate), ref _DefaultDueDate, value); }
        }
        #endregion
        //private Tasks _TaskRegistration;
        //[DevExpress.ExpressApp.DC.Aggregated, Association("TaskRegistration_CustomDueDate")]
        //[VisibleInLookupListView(false), VisibleInListView(false), VisibleInDetailView(false)]
        //public Tasks TaskRegistration
        //{
        //    get { return _TaskRegistration; }
        //    set { SetPropertyValue(nameof(TaskRegistration), ref _TaskRegistration, value); }
        //}
        private string _Parameter;
        public string Parameter
        {
            get { return _Parameter; }
            set { SetPropertyValue(nameof(Parameter), ref _Parameter, value); }
        }

        private string _ParameterDetails;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public string ParameterDetails
        {
            get { return _ParameterDetails; }
            set { SetPropertyValue(nameof(ParameterDetails), ref _ParameterDetails, value); }
        }

        private bool _TestHold;
        public bool TestHold
        {
            get { return _TestHold; }
            set { SetPropertyValue(nameof(TestHold), ref _TestHold, value); }
        }
    }
}