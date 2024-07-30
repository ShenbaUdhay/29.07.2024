using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.SDA
{
    [DefaultClassOptions]
    public class FlutterDefaultSettings : BaseObject
    {
        public FlutterDefaultSettings(Session session) : base(session) { }

        private bool _DetectStation;
        public bool DetectStation
        {
            get { return _DetectStation; }
            set { SetPropertyValue(nameof(DetectStation), ref _DetectStation, value); }
        }

        private bool _AlternateStation;
        public bool AlternateStation
        {
            get { return _AlternateStation; }
            set { SetPropertyValue(nameof(AlternateStation), ref _AlternateStation, value); }
        }

        private bool _Review;
        public bool Review
        {
            get { return _Review; }
            set { SetPropertyValue(nameof(Review), ref _Review, value); }
        }
    }
}