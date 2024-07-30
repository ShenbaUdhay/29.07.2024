using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    [NonPersistent]
    [DomainComponent]
    public class BatchReporting : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BatchReporting(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #region DateReceivedFrom
        private DateTime _DateReceivedFrom;
        [Appearance("DiableDateReceivedFrom", Enabled = false, Criteria = "All = true", Context = "DetailView")]
        [Appearance("EnableDateReceivedFrom", Enabled = true, Criteria = "All = false", Context = "DetailView")]
        public DateTime DateReceivedFrom
        {
            get 
            {
                if (All)
                {
                    _DateReceivedFrom = DateTime.MinValue;
                }
                return _DateReceivedFrom;
            }
            set { SetPropertyValue(nameof(DateReceivedFrom), ref _DateReceivedFrom, value); }
        }
        #endregion
        #region DateReceivedTo
        private DateTime _DateReceivedTo;
        [Appearance("DiableDateReceivedTo", Enabled = false, Criteria = "All = true", Context = "DetailView")]
        [Appearance("EnableDateReceivedTo", Enabled = true, Criteria = "All = false", Context = "DetailView")]
        public DateTime DateReceivedTo
        {
            get 
            {
                if (All)
                {
                    _DateReceivedTo = DateTime.MinValue;
                }
                if (_DateReceivedTo != DateTime.MinValue)
                {
                    TimeSpan newTime = new TimeSpan(23, 59, 0);
                    _DateReceivedTo = _DateReceivedTo + newTime;
                }
                return _DateReceivedTo; 
            }
            set { SetPropertyValue(nameof(DateReceivedTo), ref _DateReceivedTo, value); }
        }
        #endregion
        #region SampleMatrix
        private VisualMatrix _SampleMatrix;
        [ImmediatePostData]
        [DataSourceProperty("SampleMatrixes")]
        [Appearance("DiableSampleMatrix", Enabled = false, Criteria = "All = true", Context = "DetailView")]
        [Appearance("EnableSampleMatrix", Enabled = true, Criteria = "All = false", Context = "DetailView")]
        public VisualMatrix SampleMatrix
        {
            get 
            {
                if (All)
                {
                    _SampleMatrix = null;
                }
                return _SampleMatrix; 
            }
            set { SetPropertyValue(nameof(SampleMatrix), ref _SampleMatrix, value); }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<VisualMatrix> SampleMatrixes
        {
            get
            {
                XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session);
                return new XPCollection<VisualMatrix>(Session, new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[IsRetired] <> True Or [IsRetired] Is Null"), new InOperator("MatrixName.Oid", tests.Where(i => i.MatrixName != null).Select(i => i.MatrixName.Oid).Distinct().ToList())));
            }
        }
        #endregion 
        #region Test
        private TestMethod _Test;
        //[DataSourceProperty("TestDataSource")]
        [Appearance("DiableTest", Enabled = false, Criteria = "All = true", Context = "DetailView")]
        [Appearance("EnableTest", Enabled = true, Criteria = "All = false", Context = "DetailView")]
        public TestMethod Test
        {
            get
            {
                if (All)
                {
                    _Test = null;
                }
                return _Test;
            }
            set { SetPropertyValue(nameof(Test), ref _Test, value); }
        }
        //[Browsable(false)]
        //[NonPersistent]
        //public XPCollection<TestMethod> TestDataSource
        //{
        //    get
        //    {
        //        if (SampleMatrix != null)
        //        {
        //            XPView testsView = new XPView(Session, typeof(TestMethod));
        //            testsView.Criteria = CriteriaOperator.Parse("[MatrixName]=? and [MethodName] is Not null and [MethodName.GCRecord] is null", SampleMatrix.MatrixName);
        //            testsView.Properties.Add(new ViewProperty("TJobID", SortDirection.Ascending, "TestName", true, true));
        //            testsView.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
        //            List<object> groups = new List<object>();
        //            foreach (ViewRecord rec in testsView)
        //                groups.Add(rec["Toid"]);
        //            return new XPCollection<TestMethod>(Session, new InOperator("Oid", groups));
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        #endregion
        #region SampleCategory
        private SampleCategory _SampleCategory;
        [Appearance("DiableSampleCategory", Enabled = false, Criteria = "All = true", Context = "DetailView")]
        [Appearance("EnableSampleCategory", Enabled = true, Criteria = "All = false", Context = "DetailView")]
        public SampleCategory SampleCategory
        {
            get
            {
                if (All)
                {
                    _SampleCategory = null;
                }
                return _SampleCategory;
            }
            set { SetPropertyValue(nameof(SampleCategory), ref _SampleCategory, value); }
        } 
        #endregion    
        #region Client
        private Customer _Client;
        [ImmediatePostData(true)]
        [Appearance("DiableClient", Enabled = false, Criteria = "All = true", Context = "DetailView")]
        [Appearance("EnableClient", Enabled = true, Criteria = "All = false", Context = "DetailView")]
        public Customer Client
        {
            get
            {
                if (All)
                {
                    _Client = null;
                }
                return _Client;
            }
            set { SetPropertyValue(nameof(Client), ref _Client, value); }
        } 
        #endregion 
        #region ProjectID
        private Project _ProjectID;
        [Appearance("DiableProjectID", Enabled = false, Criteria = "All = true", Context = "DetailView")]
        [Appearance("EnableProjectID", Enabled = true, Criteria = "All = false", Context = "DetailView")]
        public Project ProjectID
        {
            get
            {
                if (All)
                {
                    _ProjectID = null;
                }
                return _ProjectID;
            }
            set { SetPropertyValue(nameof(ProjectID), ref _ProjectID, value); }
        } 
        #endregion 
        #region All
        private bool _All;
        [ImmediatePostData(true)]
        public bool All
        {
            get { return _All; }
            set { SetPropertyValue(nameof(All), ref _All, value); }
        } 
        #endregion
    }
}