using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    [NonPersistent]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ICMRollBack : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        #region Consturcotr
        public ICMRollBack(Session session)
            : base(session)
        {
        }
        #endregion

        #region Default Methods
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #endregion

        #region Reason
        private string _Reason;
        [NonPersistent]
        [Size(1000)]
        public string Reason
        {
            get { return _Reason; }
            set { SetPropertyValue("Reason", ref _Reason, value); }
        }
        #endregion
    }
}