using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;

namespace Modules.BusinessObjects.ICM
{
    [NonPersistent]
    public class DisposalQuerypanel : BaseObject
    {
        #region Constructor
        public DisposalQuerypanel(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #endregion

        #region LT
        private Distribution _LT;
        public Distribution LT
        {
            get { return _LT; }
            set
            {
                SetPropertyValue<Distribution>("LT", ref _LT, value);
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