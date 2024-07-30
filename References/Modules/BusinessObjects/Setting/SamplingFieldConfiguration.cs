using DevExpress.ExpressApp;
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
    public class SamplingFieldConfiguration : BaseObject
    {
        public SamplingFieldConfiguration(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        #region FieldID
        private string _FieldID;
        public string FieldID
        {
            get
            {
                return _FieldID;
            }
            set
            {
                SetPropertyValue<string>(nameof(FieldID), ref _FieldID, value);
            }
        }
        #endregion

        #region FieldCaption
        private string _FieldCaption;
        public string FieldCaption
        {
            get
            {
                return _FieldCaption;
            }
            set
            {
                SetPropertyValue<string>(nameof(FieldCaption), ref _FieldCaption, value);
            }
        }
        #endregion

        #region FieldCustomCaption
        private string _FieldCustomCaption;
        public string FieldCustomCaption
        {
            get
            {
                return _FieldCustomCaption;
            }
            set
            {
                SetPropertyValue<string>(nameof(FieldCustomCaption), ref _FieldCustomCaption, value);
            }
        }
        #endregion

        #region Freeze
        private bool _Freeze;
        public bool Freeze
        {
            get
            {
                return _Freeze;
            }
            set
            {
                SetPropertyValue<bool>(nameof(Freeze), ref _Freeze, value);
            }
        }
        #endregion

        #region Width
        private int _Width;
        [RuleValueComparison("valSamplingWidth", DefaultContexts.Save, ValueComparisonType.GreaterThan, -1)]
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                SetPropertyValue<int>(nameof(Width), ref _Width, value);
            }
        }
        #endregion

        #region SortOrder
        private int _SortOrder;
        [RuleValueComparison("valSamplingSort", DefaultContexts.Save, ValueComparisonType.GreaterThan, -1)]
        public int SortOrder
        {
            get
            {
                return _SortOrder;
            }
            set
            {
                SetPropertyValue<int>(nameof(SortOrder), ref _SortOrder, value);
            }
        }
        #endregion

        #region FieldSetup
        private VisualMatrix _VisualMatrix;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("VisualMatrix-SamplingFieldConfiguration")]
        public VisualMatrix VisualMatrix
        {
            get
            {
                return _VisualMatrix;
            }
            set
            {
                SetPropertyValue(nameof(VisualMatrix), ref _VisualMatrix, value);
            }
        }
        #endregion

        #region CreatedBy
        private Employee _createdBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee CreatedBy
        {
            get
            {
                return _createdBy;
            }
            set
            {
                SetPropertyValue(nameof(CreatedBy), ref _createdBy, value);
            }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get
            {
                return _CreatedDate;
            }
            set
            {
                SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value);
            }
        }
        #endregion

        #region ModifiedBy
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee ModifiedBy
        {
            get
            {
                return _ModifiedBy;
            }
            set
            {
                SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value);
            }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get
            {
                return _ModifiedDate;
            }
            set
            {
                SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value);
            }
        }
        #endregion

        #region Type
        private FieldClass _fieldClass;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public FieldClass FieldClass
        {
            get
            {
                return _fieldClass;
            }
            set
            {
                SetPropertyValue(nameof(FieldClass), ref _fieldClass, value);
            }
        }
        #endregion

        #region IsMandatory
        private bool _IsMandatory;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool IsMandatory
        {
            get
            {
                return _IsMandatory;
            }
            set
            {
                SetPropertyValue(nameof(IsMandatory), ref _IsMandatory, value);
            }
        }
        #endregion

        #region IsReadonly
        private bool _IsReadonly;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool IsReadonly
        {
            get
            {
                return _IsReadonly;
            }
            set
            {
                SetPropertyValue(nameof(IsReadonly), ref _IsReadonly, value);
            }
        }
        #endregion
    }

    public enum FieldClass
    {
        Sampling,
        Station,
        Test
    }
}