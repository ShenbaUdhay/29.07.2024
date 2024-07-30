using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class SamplePrepTemplateFields : BaseObject
    {
        public SamplePrepTemplateFields(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #region FieldName
        private string _FieldName;
        [RuleUniqueValue]
        [RuleRequiredField]
        public string FieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                SetPropertyValue<string>(nameof(FieldName), ref _FieldName, value);
            }
        }
        #endregion

        #region FieldCaption
        private string _FieldCaption;
        public string FieldCaption
        {
            get
            {
                return _FieldCaption;
            }
            set
            {
                SetPropertyValue<string>(nameof(FieldCaption), ref _FieldCaption, value);
            }
        }
        #endregion

        #region SelectedFields
        [VisibleInDetailView(false)]
        [Association("SamplePrepTemplates-SamplePrepTemplateFields")]
        public XPCollection<SamplePrepTemplates> Templates
        {
            get { return GetCollection<SamplePrepTemplates>(nameof(Templates)); }
        }
        #endregion
    }
}