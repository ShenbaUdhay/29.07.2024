using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class QcRole : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public QcRole(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }

        private string _QC_Role;
        [ImmediatePostData]
        [RuleRequiredField("Enter the QC Role", DefaultContexts.Save)]
        public string QC_Role
        {
            get { return _QC_Role; }
            set { SetPropertyValue(nameof(QC_Role), ref _QC_Role, value); }
        }
        private bool _IsDuplicate;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool IsDuplicate
        {
            get
            {
                return _IsDuplicate;
            }
            set
            {
                SetPropertyValue<bool>(nameof(IsDuplicate), ref _IsDuplicate, value);
            }
        }

        private bool _IsSpike;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool IsSpike
        {
            get
            {
                return _IsSpike;
            }
            set
            {
                SetPropertyValue<bool>(nameof(IsSpike), ref _IsSpike, value);
            }
        }

        private bool _IsBlank;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool IsBlank
        {
            get
            {
                return _IsBlank;
            }
            set
            {
                SetPropertyValue<bool>(nameof(IsBlank), ref _IsBlank, value);
            }
        }

        private bool _IsStandard;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool IsStandard
        {
            get
            {
                return _IsStandard;
            }
            set
            {
                SetPropertyValue<bool>(nameof(IsStandard), ref _IsStandard, value);
            }
        }
    }
}