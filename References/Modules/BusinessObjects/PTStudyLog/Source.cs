using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]

    public class Source : BaseObject
    {
        public Source(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }
        private string _SourceName;
        public string SourceName
        {
            get { return _SourceName; }
            set { SetPropertyValue(nameof(SourceName), ref _SourceName, value); }
        }
    }
}