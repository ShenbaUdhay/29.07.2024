using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Modules.BusinessObjects.Hr;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [DefaultProperty("SampleMatrix")]


    public class BottleSharing : BaseObject, ICheckedListBoxItemsProvider
    {
        public BottleSharing(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        private string fSharingCode;
        [ReadOnly(true)]
        public string SharingCode
        {
            get { return fSharingCode; }
            set { SetPropertyValue<string>("SharingCode", ref fSharingCode, value); }
        }

        private VisualMatrix fSampleMatrix;
        [ImmediatePostData(true)]
        public VisualMatrix SampleMatrix
        {
            get { return fSampleMatrix; }
            set { SetPropertyValue<VisualMatrix>("SampleMatrix", ref fSampleMatrix, value); fMatrix = null; }
        }

        private Matrix fMatrix;
        [DataSourceProperty("MatrixDataSource")]
        [ImmediatePostData(true)]
        public Matrix Matrix
        {
            get { return fMatrix; }
            set { SetPropertyValue<Matrix>("Matrix", ref fMatrix, value); fTests = string.Empty; }
        }

        private string fTests;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [RuleRequiredField("Tests", DefaultContexts.Save, "Tests must not be empty")]
        public string Tests
        {
            get { return fTests; }
            set { SetPropertyValue<string>("Tests", ref fTests, value); }
        }

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion

        #region ModifiedBy
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        #endregion

        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }

        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> properties = new Dictionary<object, string>();
            if (targetMemberName == "Tests" && Matrix != null)
            {
                XPCollection<TestMethod> TestDataSource = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.Oid] = ?", Matrix.Oid), new SortProperty("TestName", SortingDirection.Ascending));
                foreach (TestMethod objlab in TestDataSource)
                {
                    if (!properties.ContainsKey(objlab.Oid) && !string.IsNullOrEmpty(objlab.TestName))
                    {
                        properties.Add(objlab.Oid, objlab.TestName);
                    }
                }
            }
            return properties;
        }

        [Browsable(false)]
        public XPCollection<Matrix> MatrixDataSource
        {
            get
            {
                if (SampleMatrix != null)
                {
                    return new XPCollection<Matrix>(Session, CriteriaOperator.Parse("[Oid] = ?", SampleMatrix.MatrixName.Oid), new SortProperty("MatrixName", SortingDirection.Ascending));
                }
                else
                {
                    return null;
                }
            }
        }

        protected override void OnSaving()
        {
            if (string.IsNullOrEmpty(SharingCode))
            {
                SharingCode = (Convert.ToInt32(Session.Evaluate(typeof(BottleSharing), CriteriaOperator.Parse("Max(SharingCode)"), null)) + 1).ToString("000");
                CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                CreatedDate = DateTime.Now;
            }
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }
    }
}