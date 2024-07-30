using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    //[NonPersistent]
    //[DomainComponent]
    public class ResultEntryQueryPanel : BaseObject
    {
        ResultEntryQueryPanelInfo objQPInfo = new ResultEntryQueryPanelInfo();
        public ResultEntryQueryPanel(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            FilterDataByMonth = FilterByMonthEN._1M;
            SelectionMode = QueryMode.Job;
            objQPInfo.SelectMode = QueryMode.Job;
        }
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
        #region Matrix
        private Matrix fMatrix;
        //[Browsable(false)]
        public Matrix MatrixName
        {
            get
            {
                return fMatrix;
            }
            set
            {
                SetPropertyValue("MatrixName", ref fMatrix, value);
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

        #region Method
        private Modules.BusinessObjects.Setting.Method fMethod;
        //[Browsable(false)]
        public Modules.BusinessObjects.Setting.Method MethodName
        {
            get
            {
                return fMethod;
            }
            set
            {
                SetPropertyValue("MethodName", ref fMethod, value);
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
        #region ProjectID
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
        #region ReceivedDateFrom
        private DateTime? _ReceivedDateFrom;
        public DateTime? ReceivedDateFrom
        {
            get { return _ReceivedDateFrom; }
            set
            {
                SetPropertyValue<DateTime?>("ReceivedDateFrom", ref _ReceivedDateFrom, value);
            }
        }
        #endregion
        #region ReceivedDateTo
        private DateTime? _ReceivedDateTo;
        public DateTime? ReceivedDateTo
        {
            get { return _ReceivedDateTo; }
            set
            {
                SetPropertyValue<DateTime?>("ReceivedDateTo", ref _ReceivedDateTo, value);
            }
        }
        #endregion
        #region AnalyzedDateFrom
        private DateTime? _AnalyzedDateFrom;
        public DateTime? AnalyzedDateFrom
        {
            get { return _AnalyzedDateFrom; }
            set
            {
                SetPropertyValue<DateTime?>("AnalyzedDateFrom", ref _AnalyzedDateFrom, value);
            }
        }
        #endregion
        #region AnalyzedDateTo
        private DateTime? _AnalyzedDateTo;
        public DateTime? AnalyzedDateTo
        {
            get { return _AnalyzedDateTo; }
            set
            {
                SetPropertyValue<DateTime?>("AnalyzedDateTo", ref _AnalyzedDateTo, value);
            }
        }
        #endregion
        //#region ReportId
        //private Reporting _ReportID;
        //public Reporting ReportID
        //{
        //    get
        //    {
        //        return _ReportID;
        //    }
        //    set
        //    {
        //        SetPropertyValue<Reporting>("ReportID", ref _ReportID, value);
        //    }
        //}
        //#endregion
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

    public enum QueryMode
    {
        [XafDisplayName("JobID")]
        Job = 0,
        //[XafDisplayName("QCBatchID")]
        //QC = 1,
        [XafDisplayName("QCBatchID")]
        ABID = 2
    }
}