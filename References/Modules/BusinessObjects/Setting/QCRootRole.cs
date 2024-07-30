using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [DefaultProperty("QCRoot_Role")]

    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class QCRootRole : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public QCRootRole(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }

        private string _QCRootRole;
        [ImmediatePostData]
        [RuleRequiredField("Enter the QCRoot Role", DefaultContexts.Save, "'QC Root Role must not be empty'")]
        public string QCRoot_Role
        {
            get { return _QCRootRole; }
            set { SetPropertyValue(nameof(QCRoot_Role), ref _QCRootRole, value); }
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