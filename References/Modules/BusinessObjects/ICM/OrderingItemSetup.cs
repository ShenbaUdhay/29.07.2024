using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class OrderingItemSetup : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        #region Constructor
        public OrderingItemSetup(Session session)
            : base(session)
        {
        }
        #endregion

        #region DefaultMethods
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #endregion

        #region OrderingItemOn
        private bool _OrderingItemon;
        public bool OrderingItemon
        {
            get { return _OrderingItemon; }
            set { SetPropertyValue("OrderingItemOn", ref _OrderingItemon, value); }
        }
        #endregion

        //#region OrderingITemOff
        //private bool _OrderingItemoff;
        //public bool OrderingItemoff
        //{
        //    get { return _OrderingItemoff; }
        //    set { SetPropertyValue("OrderingItemoff", ref _OrderingItemoff, value); }
        //}
        //#endregion
    }
}