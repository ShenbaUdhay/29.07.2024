using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    
    public class ResultEntryDefaultSettings : BaseObject
    { 
        public ResultEntryDefaultSettings(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
          
        }
       
        public enum YesNoFilter
        {
            [XafDisplayName("Yes")]
            Yes,
            [XafDisplayName("No")]
            No
        }
        private YesNoFilter _SDMS;
        public YesNoFilter SDMS
        {
            get { return _SDMS; }
            set { SetPropertyValue("SDMS", ref _SDMS, value); }
        }

        private YesNoFilter _Leve2DataReview;
        [Appearance("Leve2DataReview", Context = "DetailView", Criteria = "SDMS ='No'", AppearanceItemType = "ViewItem", Enabled = false)]
        public YesNoFilter Leve2DataReview
        {
            get { return _Leve2DataReview; }
            set { SetPropertyValue("Leve2DataReview", ref _Leve2DataReview, value); }
        }
        private YesNoFilter _Leve3DataReview;
        [Appearance("Leve3DataReview", Context = "DetailView", Criteria = "Leve2DataReview = 'No' Or SDMS ='No'", AppearanceItemType = "ViewItem", Enabled = false)]
        public YesNoFilter Leve3DataReview
        {
            get { return _Leve3DataReview; }
            set { SetPropertyValue("Leve3DataReview", ref _Leve3DataReview, value); }
        }

        private YesNoFilter _ResultValidation;
        public YesNoFilter ResultValidation
        {
            get { return _ResultValidation; }
            set { SetPropertyValue("ResultValidation", ref _ResultValidation, value); }
        }
        
        private YesNoFilter _ResultApproval;
        //[Appearance("ResultApproval", Context = "DetailView", Criteria = "ResultValidation = 'No'", AppearanceItemType = "ViewItem", Enabled = false)]
        public YesNoFilter ResultApproval
        {
            get { return _ResultApproval; }
            set { SetPropertyValue("ResultApproval", ref _ResultApproval, value); }
        }

        private YesNoFilter _QuoteReview;
        public YesNoFilter QuoteReview
        {
            get { return _QuoteReview; }
            set { SetPropertyValue("QuoteReview", ref _QuoteReview, value); }
        }

        private YesNoFilter _PLMDefaultValue;
        public YesNoFilter PLMDefaultValue
        {
            get { return _PLMDefaultValue; }
            set { SetPropertyValue("PLMDefaultValue", ref _PLMDefaultValue, value); }
        }

        public string label1
        {
            get { return "If No, 2, 3 are disabled"; }
        }

        public string label2
        {
            get { return "If No, 3 is disabled"; }
        }
        public string lblAnalysis
        {
            get { return "Analysis"; }
        }
        public string lblSDMSValidate
        {
            get { return "SDMS Validate"; }
        }
        public string lblSDMSApprove
        {
            get { return "SDMS Approve"; }
        }
        public string lblLIMSValidate
        {
            get { return "LIMS Validate"; }
        }

        public string lblLIMSApprove
        {
            get { return "LIMS Approve"; }
        }
        public string lblQuoteReview
        {
            get { return "Quote Review"; }
        }
    }
}