using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using ICM.Module.BusinessObjects;
using Modules.BusinessObjects.Hr;

namespace Modules.BusinessObjects.ICM
{
    [NonPersistent]
    public class POquerypanel : BaseObject
    {
        public POquerypanel(Session session) : base(session) { }

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

        #region vendor
        private Vendors _Vendor;
        public Vendors Vendor
        {
            get { return _Vendor; }
            set
            {
                SetPropertyValue<Vendors>("Vendor", ref _Vendor, value);
            }
        }
        #endregion

        //#region ViewAll
        //private Boolean _ViewAll;
        //public Boolean ViewAll
        //{
        //    get { return _ViewAll; }
        //    set
        //    {
        //        SetPropertyValue<Boolean>("ViewAll", ref _ViewAll, value);
        //    }
        //}
        //#endregion
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}