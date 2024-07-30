using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;

namespace Modules.BusinessObjects
{
    [DefaultClassOptions]

    public class Messages : BaseObject
    {
        public Messages(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }
        private string _MessageEN;
        [Size(SizeAttribute.Unlimited)]
        public string MessageEN
        {
            get { return _MessageEN; }
            set { SetPropertyValue(nameof(MessageEN), ref _MessageEN, value); }
        }

        private string _MessageCN;
        [Size(SizeAttribute.Unlimited)]
        public string MessageCN
        {
            get { return _MessageCN; }
            set { SetPropertyValue(nameof(MessageCN), ref _MessageCN, value); }
        }

        private string _MessageKey;
        public string MessageKey
        {
            get { return _MessageKey; }
            set { SetPropertyValue(nameof(MessageKey), ref _MessageKey, value); }
        }

        private string _Module;
        public string Module
        {
            get { return _Module; }
            set { SetPropertyValue(nameof(Module), ref _Module, value); }
        }

        private string _BusinessObject;
        [Browsable(false)]
        public string BusinessObject
        {
            get { return _BusinessObject; }
            set { SetPropertyValue(nameof(BusinessObject), ref _BusinessObject, value); }
        }

        private string _ActionType;
        //[Browsable(false)]
        public string ActionType
        {
            get { return _ActionType; }
            set { SetPropertyValue(nameof(ActionType), ref _ActionType, value); }
        }

        private string _ViewName;
        [Browsable(false)]
        public string ViewName
        {
            get { return _ViewName; }
            set { SetPropertyValue(nameof(ViewName), ref _ViewName, value); }
        }

        private string _Businuss_Object;
        public string Businuss_Object
        {
            get { return _Businuss_Object; }
            set { SetPropertyValue(nameof(Businuss_Object), ref _Businuss_Object, value); }
        }

    }
}