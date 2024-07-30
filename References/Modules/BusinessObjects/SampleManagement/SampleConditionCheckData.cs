using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.SampleManagement
{
    public enum PickAnswer
    {
        Yes,
        No,
        [XafDisplayName("N/A")]
        None
    }
    [DefaultClassOptions]
    public class SampleConditionCheckData : BaseObject, ICheckedListBoxItemsProvider
    {
        public SampleConditionCheckData(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        #region CheckPoint
        private string _CheckPoint;
        [RuleRequiredField]
        [RuleUniqueValue]
        public string CheckPoint
        {
            get
            {
                return _CheckPoint;
            }
            set
            {
                SetPropertyValue<string>("CheckPoint", ref _CheckPoint, value);
            }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue("CreatedBy", ref _CreatedBy, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }
        }
        #endregion

        #region ModifiedBy
        private Employee _ModifiedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue("ModifiedBy", ref _ModifiedBy, value); }
        }

        #endregion

        #region Sort
        private int _Sort;
        // [ModelDefault("DisplayFormat", "0 item(s)")] // Display format for the property
        //  [ModelDefault("EditMask", "0 item(s)")]
        [ImmediatePostData] // Optional: Updates the UI immediately after changing the value
                            // [RuleRange(1, 100)]
                            // [DefaultValue(1)]
                            // [DefaultValue(typeof(int), "1")]
        [RuleRange(1, int.MaxValue)]
        public int Sort

        {
            get
            {
                if (_Sort <= 0)
                {
                    _Sort = 1;
                }
                return _Sort;
            }
            set { SetPropertyValue("Sort", ref _Sort, value); }
        }
        #endregion
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<VisualMatrix> SampleMatrixes
        {
            get
            {
                return new XPCollection<VisualMatrix>(Session, CriteriaOperator.Parse("[IsRetired] <> True Or [IsRetired] Is Null"));
            }
        }
        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "SampleMatrices" && SampleMatrixes != null && SampleMatrixes.Count > 0)
            {
                foreach (VisualMatrix objSampleMatrix in SampleMatrixes.Where(i => i.VisualMatrixName != null).OrderBy(i => i.VisualMatrixName).ToList())
                {
                    if (!Properties.ContainsKey(objSampleMatrix.VisualMatrixName))
                    {
                        Properties.Add(objSampleMatrix.VisualMatrixName, objSampleMatrix.VisualMatrixName);
                    }
                }
            }

            return Properties;
        }
        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }

        #endregion
        private string _SampleMatrices;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string SampleMatrices
        {
            get { return _SampleMatrices; }
            set { SetPropertyValue(nameof(SampleMatrices), ref _SampleMatrices, value); }
        }
        private PickAnswer _PickAnswer;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public PickAnswer PickAnswer
        {
            get { return _PickAnswer; }
            set { SetPropertyValue(nameof(PickAnswer), ref _PickAnswer, value); }
        }
    }
}