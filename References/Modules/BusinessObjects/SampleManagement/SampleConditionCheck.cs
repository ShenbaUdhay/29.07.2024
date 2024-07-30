using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SamplingManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.SampleManagement
{
    //public enum SampleConditionChk
    //{
    //    Yes=0,
    //    No=1,
    //    [XafDisplayName("N/A")]
    //    None=2
    //}
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SampleConditionCheck : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SampleConditionCheck(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CheckInBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CheckInDate = DateTime.Now;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        private String _JobID;
        [NonPersistent]
        public String JobID
        {
            get
            {
                if (SampleRegistration != null)
                {
                    _JobID = SampleRegistration.JobID;
                }
                //else if (TaskRegistration != null)
                //{
                //    _JobID = TaskRegistration.JobId.JobID;
                //}
                return _JobID;
            }
            set { SetPropertyValue(nameof(JobID), ref _JobID, value); }
        }
        private Customer _ClientName;
        [NonPersistent]
        public Customer ClientName
        {
            get
            {
                if (SampleRegistration != null && SampleRegistration.ClientName != null)
                {
                    _ClientName = SampleRegistration.ClientName;
                }
                return _ClientName;
            }
            set { SetPropertyValue(nameof(ClientName), ref _ClientName, value); }
        }

        private Employee _CheckInBy;
        public Employee CheckInBy
        {
            get { return _CheckInBy; }
            set { SetPropertyValue(nameof(_CheckInBy), ref _CheckInBy, value); }
        }
        private DateTime _CheckInDate;
        public DateTime CheckInDate
        {
            get { return _CheckInDate; }
            set { SetPropertyValue(nameof(_CheckInDate), ref _CheckInDate, value); }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<VisualMatrix> SampleMatrixes
        {
            get
            {
                return new XPCollection<VisualMatrix>(Session, CriteriaOperator.Parse(""));
            }
        }
        #region ICheckedListBoxItemsProvider Members

        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }

        Dictionary<object, string> ICheckedListBoxItemsProvider.GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "VisualMatrix" && SampleMatrixes != null && SampleMatrixes.Count > 0)
            {
                foreach (VisualMatrix objSampleMatrix in SampleMatrixes.Where(i => i.VisualMatrixName != null).OrderBy(i => i.VisualMatrixName).ToList())
                {
                    if (!Properties.ContainsKey(objSampleMatrix.Oid))
                    {
                        Properties.Add(objSampleMatrix.Oid, objSampleMatrix.VisualMatrixName);
                    }
                }
            }

            return Properties;
        }
        #endregion
        private string _VisualMatrix;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string VisualMatrix
        {
            get { return _VisualMatrix; }
            set { SetPropertyValue(nameof(VisualMatrix), ref _VisualMatrix, value); }
        }
        private string _Temperature;
        public string Temperature
        {
            get { return _Temperature; }
            set { SetPropertyValue(nameof(Temperature), ref _Temperature, value); }
        }
        private string _ThermometerID;
        public string ThermometerID
        {
            get { return _ThermometerID; }
            set { SetPropertyValue(nameof(ThermometerID), ref _ThermometerID, value); }
        }
        private string _SamplePH;
        public string SamplePH
        {
            get { return _SamplePH; }
            set { SetPropertyValue(nameof(SamplePH), ref _SamplePH, value); }
        }
        private string _PHPaperID;
        public string PHPaperID
        {
            get { return _PHPaperID; }
            set { SetPropertyValue(nameof(PHPaperID), ref _PHPaperID, value); }
        }
        private Modules.BusinessObjects.SampleManagement.Samplecheckin _SampleRegistration;
        [Association("SampleRegistration-SampleConditionCheck")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Modules.BusinessObjects.SampleManagement.Samplecheckin SampleRegistration
        {
            get { return _SampleRegistration; }
            set { SetPropertyValue(nameof(SampleRegistration), ref _SampleRegistration, value); }
        }
        //#region SamplingProposal
        //private SamplingProposal _SamplingProposal;
        //[Association("SamplingProposal-SampleConditionCheck")]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public SamplingProposal SamplingProposal
        //{
        //    get { return _SamplingProposal; }
        //    set { SetPropertyValue(nameof(SamplingProposal), ref _SamplingProposal, value); }
        //}
        //#endregion
        #region SampleConditionCheckPoint
        [Association("SampleConditionCheck-SampleConditionCheckPoint")]

        public XPCollection<SampleConditionCheckPoint> SampleConditionCheckPoint
        {
            get { return GetCollection<SampleConditionCheckPoint>("SampleConditionCheckPoint"); }
        }
        #endregion

        #region SampleConditionCheckPoint
        [Association("SampleConditionCheck-SampleConditionCheckComment")]

        public XPCollection<SampleConditionCheckComment> SampleConditionCheckComment
        {
            get { return GetCollection<SampleConditionCheckComment>("SampleConditionCheckComment"); }
        }
        #endregion


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
        //#region TaskRegistration
        //private Tasks _TaskRegistration;
        //[Association("TaskRegistration-SampleConditionCheck")]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public Tasks TaskRegistration
        //{
        //    get { return _TaskRegistration; }
        //    set { SetPropertyValue(nameof(TaskRegistration), ref _TaskRegistration, value); }
        //}
        //#endregion
        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}