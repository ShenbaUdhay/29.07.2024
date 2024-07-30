using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.SampleManagement
{
    public enum SampleSourceMode
    {
        No,Yes
    }
    public enum SampleSource
    {
        [XafDisplayName("Regular")]
        DropOff,
        [XafDisplayName("Pre-scheduled")]
        Sampling
    }
    public enum COCSamples
    {
        //[XafDisplayName("Walk-in")]
        VerticalBarcode, MRFVerticalBarcode, LCRVerticalBarcode
    }
    [DefaultClassOptions]
     
    public class SampleSourceSetup : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SampleSourceSetup(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private SampleSourceMode _NeedToActivateSampleSourceMode;
        [ImmediatePostData(true)]
        public SampleSourceMode NeedToActivateSampleSourceMode
        {
            get { return _NeedToActivateSampleSourceMode; }
            set { SetPropertyValue(nameof(NeedToActivateSampleSourceMode), ref _NeedToActivateSampleSourceMode, value); }
        }
        private SampleSource _SampleSource;
        [Appearance("SampleSourceView", Visibility = ViewItemVisibility.Show, Criteria = "[NeedToActivateSampleSourceMode] = 'Yes'", Context = "DetailView")]
        [Appearance("SampleSourceHide", Visibility = ViewItemVisibility.Hide, Criteria = "[NeedToActivateSampleSourceMode] = 'No'", Context = "DetailView")]
        public SampleSource SampleSource
        {
            get { return _SampleSource; }
            set { SetPropertyValue(nameof(SampleSource), ref _SampleSource, value); }
        }

        private SampleSourceMode _SampleTransfer;
        [Appearance("SampleTransferView", Visibility = ViewItemVisibility.Show, Criteria = "[NeedToActivateSampleSourceMode] = 'Yes'", Context = "DetailView")]
        [Appearance("SampleTransferHide", Visibility = ViewItemVisibility.Hide, Criteria = "[NeedToActivateSampleSourceMode] = 'No'", Context = "DetailView")]
        public SampleSourceMode SampleTransfer
        {
            get { return _SampleTransfer; }
            set { SetPropertyValue(nameof(SampleTransfer), ref _SampleTransfer, value); }
        }

        private COCSamples _COCSamples;
        [Appearance("COCSamplesView", Visibility = ViewItemVisibility.Show, Criteria = "[NeedToActivateSampleSourceMode] = 'Yes'", Context = "DetailView")]
        [Appearance("COCSamplesHide", Visibility = ViewItemVisibility.Hide, Criteria = "[NeedToActivateSampleSourceMode] = 'No'", Context = "DetailView")]
        public COCSamples COCSamples
        {
            get { return _COCSamples; }
            set { SetPropertyValue(nameof(COCSamples), ref _COCSamples, value); }
        }
    }
    [DomainComponent]
    public class COCSample 
    {

        public COCSamples COCSamples { get; set; }



    }
}