using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using ICM.Module.BusinessObjects;
using Modules.BusinessObjects.Hr;

namespace Modules.BusinessObjects.ICM
{
    [NonPersistent]
    public class Receivequerypanel : BaseObject
    {
        #region Constructor
        public Receivequerypanel(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #endregion

        #region poid
        private Requisition _POID;
        public Requisition POID
        {
            get { return _POID; }
            set
            {
                SetPropertyValue<Requisition>("POID", ref _POID, value);
            }
        }
        #endregion

        #region vendor
        private Requisition _Vendor;
        [ImmediatePostData]
        public Requisition Vendor
        {
            get { return _Vendor; }
            set
            {
                SetPropertyValue<Requisition>("Vendor", ref _Vendor, value);
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