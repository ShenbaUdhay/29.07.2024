using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.PLM
{
    [DefaultClassOptions]
    [Persistent("SpreadsheetEntry_PLMResults")]
    public class SpreadsheetEntry_PLMResults : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SpreadsheetEntry_PLMResults(Session session)
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

        string fSampleLayerID;
        public string SampleLayerID
        {
            get { return fSampleLayerID; }
            set { SetPropertyValue<string>(nameof(SampleLayerID), ref fSampleLayerID, value); }
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

        string fMaterial;
        public string Material
        {
            get { return fMaterial; }
            set { SetPropertyValue<string>(nameof(Material), ref fMaterial, value); }
        }
        string fNAPositiveStop;
        public string NAPositiveStop
        {
            get { return fNAPositiveStop; }
            set { SetPropertyValue<string>(nameof(NAPositiveStop), ref fNAPositiveStop, value); }
        }
        string fFriable;
        public string Friable
        {
            get { return fFriable; }
            set { SetPropertyValue<string>(nameof(Friable), ref fFriable, value); }
        }

        string fTexture;
        public string Texture
        {
            get { return fTexture; }
            set { SetPropertyValue<string>(nameof(Texture), ref fTexture, value); }
        }

        string fVisualGross;
        public string VisualGross
        {
            get { return fVisualGross; }
            set { SetPropertyValue<string>(nameof(VisualGross), ref fVisualGross, value); }
        }

        string fColor;
        public string Color
        {
            get { return fColor; }
            set { SetPropertyValue<string>(nameof(Color), ref fColor, value); }
        }

        string fNonAsbestosValue1;
        public string NonAsbestosValue1
        {
            get { return fNonAsbestosValue1; }
            set { SetPropertyValue<string>(nameof(NonAsbestosValue1), ref fNonAsbestosValue1, value); }
        }

        string fNonAsbestosType1;
        public string NonAsbestosType1
        {
            get { return fNonAsbestosType1; }
            set { SetPropertyValue<string>(nameof(NonAsbestosType1), ref fNonAsbestosType1, value); }
        }

        string fNonAsbestosValue2;
        public string NonAsbestosValue2
        {
            get { return fNonAsbestosValue2; }
            set { SetPropertyValue<string>(nameof(NonAsbestosValue2), ref fNonAsbestosValue2, value); }
        }

        string fNonAsbestosType2;
        public string NonAsbestosType2
        {
            get { return fNonAsbestosType2; }
            set { SetPropertyValue<string>(nameof(NonAsbestosType2), ref fNonAsbestosType2, value); }
        }

        string fNonAsbestosValue3;
        public string NonAsbestosValue3
        {
            get { return fNonAsbestosValue3; }
            set { SetPropertyValue<string>(nameof(NonAsbestosValue3), ref fNonAsbestosValue3, value); }
        }

        string fNonAsbestosType3;
        public string NonAsbestosType3
        {
            get { return fNonAsbestosType3; }
            set { SetPropertyValue<string>(nameof(NonAsbestosType3), ref fNonAsbestosType3, value); }
        }

        string fNonAsbestosType4;
        public string NonAsbestosType4
        {
            get { return fNonAsbestosType4; }
            set { SetPropertyValue<string>(nameof(NonAsbestosType4), ref fNonAsbestosType4, value); }
        }

        string fPointCount;
        public string PointCount
        {
            get { return fPointCount; }
            set { SetPropertyValue<string>(nameof(PointCount), ref fPointCount, value); }
        }
        //Asbestos Type
        string fAsbestosValue1;
        public string AsbestosValue1
        {
            get { return fAsbestosValue1; }
            set { SetPropertyValue<string>(nameof(AsbestosValue1), ref fAsbestosValue1, value); }
        }


        string fAsbestosType1;
        public string AsbestosType1
        {
            get { return fAsbestosType1; }
            set { SetPropertyValue<string>(nameof(AsbestosType1), ref fAsbestosType1, value); }
        }

        string fAsbestosValue2;
        public string AsbestosValue2
        {
            get { return fAsbestosValue2; }
            set { SetPropertyValue<string>(nameof(AsbestosValue2), ref fAsbestosValue2, value); }
        }

        string fAsbestosType2;
        public string AsbestosType2
        {
            get { return fAsbestosType2; }
            set { SetPropertyValue<string>(nameof(AsbestosType2), ref fAsbestosType2, value); }
        }

        string fAsbestosValue3;
        public string AsbestosValue3
        {
            get { return fAsbestosValue3; }
            set { SetPropertyValue<string>(nameof(AsbestosValue3), ref fAsbestosValue3, value); }
        }

        string fAsbestosType3;
        public string AsbestosType3
        {
            get { return fAsbestosType3; }
            set { SetPropertyValue<string>(nameof(AsbestosType3), ref fAsbestosType3, value); }
        }

        string fNonFibrousValue1;
        public string NonFibrousValue1
        {
            get { return fNonFibrousValue1; }
            set { SetPropertyValue<string>(nameof(NonFibrousValue1), ref fNonFibrousValue1, value); }
        }

        string fNonFibrousType1;
        public string NonFibrousType1
        {
            get { return fNonFibrousType1; }
            set { SetPropertyValue<string>(nameof(NonFibrousType1), ref fNonFibrousType1, value); }
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