using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class ScreenAutoLock : BaseObject
    {
        public enum Minutes
        {
            [XafDisplayName("30 Minutes")]
            ThirtyMinutes,
            [XafDisplayName("60 Minutes")]
            SixtyMinutes,
            [XafDisplayName("90 Minutes")]
            NinetyMinutes,
            [XafDisplayName("120 Minutes")]
            OneHundredandTwentyMinutes,
        }
        public ScreenAutoLock(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private Minutes _TimeOut = Minutes.SixtyMinutes;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.RadioButtonListTimeoutEnumPropertyEditor")]
        public Minutes TimeOut
        {
            get { return _TimeOut; }
            set { SetPropertyValue("TimeOut", ref _TimeOut, value); }
        }
    }
}