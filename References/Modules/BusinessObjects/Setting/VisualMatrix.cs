using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [RuleCriteria("", DefaultContexts.Save, "DaysSampleKeeping > 0", "Days Sample Keeping should be greater than 0", SkipNullOrEmptyValues = false)]
    //[RuleCombinationOfPropertiesIsUnique("VisualMatrix", DefaultContexts.Save, "VisualMatrixName", SkipNullOrEmptyValues = false)]
    public class VisualMatrix : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public VisualMatrix(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);


        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (string.IsNullOrEmpty(VisualMatrixCode))
            {
                SelectedData sproc = Session.ExecuteSproc("GetVisualMatrixCode", "");
                if (sproc.ResultSet[0].Rows[0] != null)
                    VisualMatrixCode = sproc.ResultSet[0].Rows[0].Values[0].ToString();
            }
            if (IsRetired == true)
            {
                RetiredBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                RetiredDate = DateTime.Now;
            }
            else
            {
                RetiredBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                RetiredDate = DateTime.Now;
            }

        }

        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                foreach (BaseObject obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.Oid != null)
                    {
                        Exception ex = new Exception("Unable to delete since the data already used in another form");
                        throw ex;
                        break;

                    }
                }
            }
        }

        #region Code
        private string fVisualMatrixCode;
        //[Browsable(false)]
        public string VisualMatrixCode
        {
            get
            {
                return fVisualMatrixCode;
            }
            set
            {
                SetPropertyValue("VisualMatrixCode", ref fVisualMatrixCode, value);
            }
        }
        #endregion

        #region Name
        private string fVisualMatrixName;
        //[Browsable(false)]
        //[RuleRequiredField("VisualMatrixName", DefaultContexts.Save)]
        [RuleRequiredField("VisualMatrixName", DefaultContexts.Save, "Sample Matrix must not be empty")]
        public string VisualMatrixName
        {
            get
            {
                return fVisualMatrixName;
            }
            set
            {
                SetPropertyValue("VisualMatrixName", ref fVisualMatrixName, value.Trim());
            }
        }
        #endregion

        #region Matrix
        private Matrix fMatrixName;
        //[Browsable(false)]
        //[RuleRequiredField("MatrixName3", DefaultContexts.Save)]
        [RuleRequiredField("MatrixName4", DefaultContexts.Save, "Matrix must not be empty")]
        public Matrix MatrixName
        {
            get
            {
                return fMatrixName;
            }
            set
            {
                SetPropertyValue("MatrixName", ref fMatrixName, value);
            }
        }
        #endregion

        #region Comment
        private string fComment;
        [Size(1000)]
        //[Browsable(false)]
        public string Comment
        {
            get
            {
                return fComment;
            }
            set
            {
                SetPropertyValue("Comment", ref fComment, value);
            }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        [Appearance("MB10", Enabled = false, Context = "DetailView")]
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
        [Appearance("MD10", Enabled = false, Context = "DetailView")]
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

        //#region Relation
        //private Samplecheckin _Samplecheckin;
        //[Association("SampleCheckin-VisualMatrix")]
        //// [DataSourceCriteria("[Company] = CurrentCompanyOid()")]
        //public Samplecheckin Samplecheckin
        //{
        //    get
        //    {
        //        return _Samplecheckin;
        //    }
        //    set
        //    {
        //        SetPropertyValue("Samplecheckin", ref _Samplecheckin, value);
        //    }
        //}
        //#endregion

        //#region SampleCheckin-VisualMatrix Relation
        ////[RuleRequiredField("Samplecheckins", DefaultContexts.Save)] 
        //[Association("SamplecheckinVisualMatrix", UseAssociationNameAsIntermediateTableName = true)]
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        //public XPCollection<Samplecheckin> Samplecheckins
        //{
        //    get { return GetCollection<Samplecheckin>("Samplecheckins"); }
        //}
        //#endregion


        #region DaysSampleKeeping
        private uint _DaysSampleKeeping;
        public uint DaysSampleKeeping
        {
            get
            {
                return _DaysSampleKeeping;
            }
            set
            {
                SetPropertyValue<uint>(nameof(DaysSampleKeeping), ref _DaysSampleKeeping, value);
            }
        }
        #endregion

        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int NoSamples
        {
            get
            {
                return new XPCollection<SampleLogIn>(Session, CriteriaOperator.Parse("[VisualMatrix.Oid] = ?", Oid)).Count();
            }
        }
        #region SetupFields
        [Association("VisualMatrix-SampleMatrixSetupFields")]
        public XPCollection<SampleMatrixSetupFields> SetupFields
        {
            get
            {
                return GetCollection<SampleMatrixSetupFields>(nameof(SetupFields));
            }
        }
        #endregion
        #region SamplingFieldConfiguration
        [Association("VisualMatrix-SamplingFieldConfiguration")]
        public XPCollection<SamplingFieldConfiguration> SamplingFieldConfiguration
        {
            get
            {
                return GetCollection<SamplingFieldConfiguration>(nameof(SamplingFieldConfiguration));
            }
        }
        #endregion
//#region SetupFields
//        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
//        [Association("VisualMatrix-SamplingMatrixSetupFields")]
//        public XPCollection<SamplingMatrixSetupFields> SamplingSetupFields
//        {
//            get
//            {
//                return GetCollection<SamplingMatrixSetupFields>(nameof(SamplingSetupFields));
//            }
//        }
//        #endregion
        //#region Sampling-VisualMatrix Relation
        ////[RuleRequiredField("Samplecheckins", DefaultContexts.Save)] 
        //[Association("SamplingVisualMatrix", UseAssociationNameAsIntermediateTableName = true)]
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        //public XPCollection<Sampling> Sampling
        //{
        //    get { return GetCollection<Sampling>("Sampling"); }
        //}
        //#endregion
        #region IsRetired
        private bool _IsRetired;
        public bool IsRetired
        {
            get { return _IsRetired; }
            set { SetPropertyValue(nameof(IsRetired), ref _IsRetired, value); }
        }
        #endregion
        #region RetiredBy
        private PermissionPolicyUser _RetiredBy;
        [Browsable(false)]
        public PermissionPolicyUser RetiredBy
        {
            get { return _RetiredBy; }
            set { SetPropertyValue(nameof(RetiredBy), ref _RetiredBy, value); }
        }
        #endregion
        #region RetiredDate
        private DateTime _RetiredDate;
        [Browsable(false)]
        public DateTime RetiredDate
        {
            get { return _RetiredDate; }
            set { SetPropertyValue(nameof(RetiredDate), ref _RetiredDate, value); }
        }
        #endregion

    }
}