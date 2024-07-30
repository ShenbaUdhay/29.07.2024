using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;

namespace Modules.BusinessObjects.Setting.SDMS
{
    public partial class SpreadSheetBuilder_SampleQCTestRun : XPLiteObject
    {
        public SpreadSheetBuilder_SampleQCTestRun(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        Guid fuqSampleQCTestRunID;
        [Key(true)]
        public Guid uqSampleQCTestRunID
        {
            get { return fuqSampleQCTestRunID; }
            set { SetPropertyValue<Guid>(nameof(uqSampleQCTestRunID), ref fuqSampleQCTestRunID, value); }
        }
        int fuqSequencePatternID;
        public int uqSequencePatternID
        {
            get { return fuqSequencePatternID; }
            set { SetPropertyValue<int>(nameof(uqSequencePatternID), ref fuqSequencePatternID, value); }
        }
        QCType fuqQCTypeID;
        public QCType uqQCTypeID
        {
            get { return fuqQCTypeID; }
            set { SetPropertyValue(nameof(uqQCTypeID), ref fuqQCTypeID, value); }
        }
        int fOrder;
        [ImmediatePostData(true)]
        public int Order
        {
            get { return fOrder; }
            set { SetPropertyValue<int>(nameof(Order), ref fOrder, value); }
        }

        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool HasSampleAsQCSource
        {
            get
            {
                if (uqQCTypeID != null)
                {
                    if (uqQCTypeID.QCSource != null && uqQCTypeID.QCSource.QC_Source == "Sample")
                    {
                        return true;
                    }
                    else if (uqQCTypeID.QCSource != null)
                    {
                        QCType objQCType = Session.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName] = ?", uqQCTypeID.QCSource.QC_Source));
                        if (objQCType != null && objQCType.QCSource != null && objQCType.QCSource.QC_Source == "Sample")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }
}