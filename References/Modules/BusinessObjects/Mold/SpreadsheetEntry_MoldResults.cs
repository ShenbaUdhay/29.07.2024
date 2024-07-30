using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.Mold
{
    [DefaultClassOptions]
    [Persistent("SpreadsheetEntry_MoldResults")]
    public class SpreadsheetEntry_MoldResults : BaseObject
    {
        public SpreadsheetEntry_MoldResults(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        SpreadSheetEntry_AnalyticalBatch fuqAnalyticalBatchID;
        public SpreadSheetEntry_AnalyticalBatch uqAnalyticalBatchID
        {
            get { return fuqAnalyticalBatchID; }
            set { SetPropertyValue(nameof(uqAnalyticalBatchID), ref fuqAnalyticalBatchID, value); }
        }
        SampleParameter fuqSampleParameterID;
        public SampleParameter uqSampleParameterID
        {
            get { return fuqSampleParameterID; }
            set { SetPropertyValue<SampleParameter>(nameof(uqSampleParameterID), ref fuqSampleParameterID, value); }
        }

        string fSampleID;
        public string SampleID
        {
            get { return fSampleID; }
            set { SetPropertyValue<string>(nameof(SampleID), ref fSampleID, value); }
        }

        QCType fuqQCTypeID;
        public QCType uqQCTypeID
        {
            get { return fuqQCTypeID; }
            set { SetPropertyValue<QCType>(nameof(uqQCTypeID), ref fuqQCTypeID, value); }
        }
        string fSystemID;
        [Size(50)]
        public string SystemID
        {
            get { return fSystemID; }
            set { SetPropertyValue<string>(nameof(SystemID), ref fSystemID, value); }
        }

        Employee fAnalyzedBy;
        public Employee AnalyzedBy
        {
            get { return fAnalyzedBy; }
            set { SetPropertyValue<Employee>(nameof(AnalyzedBy), ref fAnalyzedBy, value); }
        }
        DateTime? fAnalyzedDate;
        public DateTime? AnalyzedDate
        {
            get { return fAnalyzedDate; }
            set { SetPropertyValue<DateTime?>(nameof(AnalyzedDate), ref fAnalyzedDate, value); }
        }
        Employee fReviewedBy;
        public Employee ReviewedBy
        {
            get { return fReviewedBy; }
            set { SetPropertyValue<Employee>(nameof(ReviewedBy), ref fReviewedBy, value); }
        }
        DateTime? fReviewedDate;
        public DateTime? ReviewedDate
        {
            get { return fReviewedDate; }
            set { SetPropertyValue<DateTime?>(nameof(ReviewedDate), ref fReviewedDate, value); }
        }
        Employee fVerifiedBy;
        public Employee VerifiedBy
        {
            get { return fVerifiedBy; }
            set { SetPropertyValue<Employee>(nameof(VerifiedBy), ref fVerifiedBy, value); }
        }
        DateTime? fVerifiedDate;
        public DateTime? VerifiedDate
        {
            get { return fVerifiedDate; }
            set { SetPropertyValue<DateTime?>(nameof(VerifiedDate), ref fVerifiedDate, value); }
        }

        string fAnalyticalBatchID;
        [Size(50)]
        public string AnalyticalBatchID
        {
            get { return fAnalyticalBatchID; }
            set { SetPropertyValue<string>(nameof(AnalyticalBatchID), ref fAnalyticalBatchID, value); }
        }
    }
}