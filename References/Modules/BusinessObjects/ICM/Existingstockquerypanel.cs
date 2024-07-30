using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;

namespace Modules.BusinessObjects.ICM
{
    [NonPersistent]
    public class Existingstockquerypanel : BaseObject
    {
        public Existingstockquerypanel(Session session) : base(session) { }

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

        //#region itemname
        //private ExistingStock _ItemName;
        //public ExistingStock ItemName
        //{
        //    get { return _ItemName; }
        //    set
        //    {
        //        SetPropertyValue<ExistingStock>("ItemName", ref _ItemName, value);
        //    }
        //}
        //#endregion

        #region ESID
        private ExistingStock _ESID;
        [ImmediatePostData]
        public ExistingStock ESID
        {
            get { return _ESID; }
            set
            {
                SetPropertyValue<ExistingStock>("ESID", ref _ESID, value);
            }
        }
        #endregion

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}