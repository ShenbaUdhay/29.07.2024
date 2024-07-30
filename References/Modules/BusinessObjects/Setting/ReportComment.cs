using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [NonPersistent]
    public class ReportComment : BaseObject
    {
        public ReportComment(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string _Title;
        [RuleRequiredField]
        public string Title
        {
            get { return _Title; }
            set { SetPropertyValue(nameof(Title), ref _Title, value); }
        }
        #region Comment
        private string fComment;
        [RuleRequiredField]

        [Size(1000)]
        public string Comment
        {
            get
            {
                return fComment;
            }
            set
            {
                SetPropertyValue("Comment", ref fComment, value);
            }
        }
        #endregion
    }
}