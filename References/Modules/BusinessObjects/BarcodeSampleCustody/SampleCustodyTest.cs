using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.BarCodeSampleCustody
{
    [DefaultClassOptions]

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SampleCustodyTest : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SampleCustodyTest(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _BarcodeScan;
        public string BarcodeScan
        {
            get { return _BarcodeScan; }
            set { SetPropertyValue(nameof(BarcodeScan), ref _BarcodeScan, value); }
        }

        private string _To;
        [EditorAlias("SampleToPropertyEditor")]
        public string To
        {
            get { return _To; }
            set { SetPropertyValue(nameof(To), ref _To, value); }
        }

        private string _From;

        [EditorAlias("SampleToPropertyEditor")]
        public string From
        {
            get { return _From; }
            set { SetPropertyValue(nameof(From), ref _From, value); }
        }

        #region SampleIn
        [Association("SampleCustodyTest-SampleIn")]
        public XPCollection<SampleIn> SampleIns
        {
            get
            {
                return GetCollection<SampleIn>("SampleIns");
            }
        }
        #endregion

        //#region SampleDisposal
        //[Association("SampleCustodyTest-SampleDisposal")]
        //public XPCollection<SampleDisposal> SampleDisposals
        //{
        //    get
        //    {
        //        return GetCollection<SampleDisposal>("SampleDisposals");
        //    }
        //}
        //#endregion

    }
}