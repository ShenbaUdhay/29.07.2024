using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Setting.Mold;

namespace Modules.BusinessObjects.Mold
{
    [DefaultClassOptions]

    public class SpreadsheetEntry_MoldAirResults : BaseObject
    {
        public SpreadsheetEntry_MoldAirResults(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        SpreadsheetEntry_MoldResults fuqMoldResultsID;
        public SpreadsheetEntry_MoldResults uqMoldResultsID
        {
            get { return fuqMoldResultsID; }
            set { SetPropertyValue(nameof(uqMoldResultsID), ref fuqMoldResultsID, value); }
        }

        MoldParameters fuqMoldParameterID;
        public MoldParameters uqMoldParameterID
        {
            get { return fuqMoldParameterID; }
            set { SetPropertyValue(nameof(uqMoldParameterID), ref fuqMoldParameterID, value); }
        }

        string fMoldHold;
        public string MoldHold
        {
            get { return fMoldHold; }
            set { SetPropertyValue(nameof(MoldHold), ref fMoldHold, value); }
        }

        string fTotalCount;
        public string TotalCount
        {
            get { return fTotalCount; }
            set { SetPropertyValue(nameof(TotalCount), ref fTotalCount, value); }
        }

        string fCountM3;
        public string CountM3
        {
            get { return fCountM3; }
            set { SetPropertyValue(nameof(CountM3), ref fCountM3, value); }
        }

        string fTotalPercent;
        public string TotalPercent
        {
            get { return fTotalPercent; }
            set { SetPropertyValue(nameof(TotalPercent), ref fTotalPercent, value); }
        }

        string fComment;
        [Size(500)]
        public string Comment
        {
            get { return fComment; }
            set { SetPropertyValue(nameof(Comment), ref fComment, value); }
        }

        string fInterpretation;
        public string Interpretation
        {
            get { return fInterpretation; }
            set { SetPropertyValue(nameof(Interpretation), ref fInterpretation, value); }
        }

        string fImageSource;
        public string ImageSource
        {
            get { return fImageSource; }
            set { SetPropertyValue(nameof(ImageSource), ref fImageSource, value); }
        }
    }
}