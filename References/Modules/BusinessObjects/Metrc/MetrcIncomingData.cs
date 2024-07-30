using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Metrc
{
    [DefaultClassOptions]
    public class MetrcIncomingHistory : BaseObject
    {
        public MetrcIncomingHistory(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string _data;
        [Size(SizeAttribute.Unlimited)]
        public string data
        {
            get { return _data; }
            set { SetPropertyValue("data", ref _data, value); }
        }

        private int _hash;
        public int hash
        {
            get { return _hash; }
            set { SetPropertyValue("hash", ref _hash, value); }
        }
    }
}