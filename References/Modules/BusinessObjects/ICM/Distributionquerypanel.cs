using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;

namespace Modules.BusinessObjects.ICM
{
    [NonPersistent]
    public class Distributionquerypanel : BaseObject
    {
        #region Constructor
        public Distributionquerypanel(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #endregion

        #region Item
        private Distribution _Item;
        public Distribution Item
        {
            get { return _Item; }
            set
            {
                SetPropertyValue<Distribution>("Item", ref _Item, value);
            }
        }
        #endregion

        #region rcid
        private Distribution _RCID;
        public Distribution RCID
        {
            get { return _RCID; }
            set
            {
                SetPropertyValue<Distribution>("RCID", ref _RCID, value);
            }
        }
        #endregion

        #region vendor
        private Distribution _Vendor;
        public Distribution Vendor
        {
            get { return _Vendor; }
            set
            {
                SetPropertyValue<Distribution>("Vendor", ref _Vendor, value);
            }
        }
        #endregion

        #region Mode
        private ENMode _Mode = ENMode.Enter;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.QueryPanel.RadioButtonListEnumPropertyEditor")]
        [ImmediatePostData]
        public ENMode Mode
        {
            get { return _Mode; }
            set { SetPropertyValue("Mode", ref _Mode, value); }
        }
        #endregion

    }
}