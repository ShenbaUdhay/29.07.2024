using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class UnconventionalAnalysisDataEntryConfigBOSourceCodeCaption : BaseObject
    {
        public UnconventionalAnalysisDataEntryConfigBOSourceCodeCaption(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #region BOSourceCodeCaption
        private string _BOSourceCodeCaption;
        //[RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The data entry type already saved.")]
        [RuleRequiredField]
        [RuleUniqueValue]
        public string BOSourceCodeCaption
        {
            get
            {
                return _BOSourceCodeCaption;
            }
            set { SetPropertyValue(nameof(BOSourceCodeCaption), ref _BOSourceCodeCaption, value); }
        }
        #endregion
    }
}