using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [NonPersistent]
    [DomainComponent]
    public class DummyClass : BaseObject
    {
        public DummyClass(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue(nameof(Name), ref _Name, value); }
        }

        private string _NPTemplateName;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string NpTemplateName
        {
            get { return _NPTemplateName; }
            set { SetPropertyValue("NpTemplateName", ref _NPTemplateName, value); }
        }
        private string _NPTest;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string NPTest
        {
            get { return _NPTest; }
            set { SetPropertyValue("NPTest", ref _NPTest, value); }
        }

        private string _NPMethod;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string NPMethod
        {
            get { return _NPMethod; }
            set { SetPropertyValue("NPMethod", ref _NPMethod, value); }
        }

        private string _NPMatrix;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string NPMatrix
        {
            get { return _NPMatrix; }
            set { SetPropertyValue("NPMatrix", ref _NPMatrix, value); }
        }
        private string _NPTempType;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string NPTempType
        {
            get { return _NPTempType; }
            set { SetPropertyValue("NPTempType", ref _NPTempType, value); }
        }
        private string _NPsource;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string NPsource
        {
            get { return _NPsource; }
            set { SetPropertyValue("NPsource", ref _NPsource, value); }
        }

        private int _NPTempID;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public int NPTempID
        {
            get { return _NPTempID; }
            set { SetPropertyValue("NPTempID", ref _NPTempID, value); }
        }

        private string _Reason;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Reason
        {
            get { return _Reason; }
            set { SetPropertyValue("Reason", ref _Reason, value); }
        }
    }
}