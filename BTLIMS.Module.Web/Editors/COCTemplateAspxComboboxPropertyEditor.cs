using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Editors
{
    [PropertyEditor(typeof(String), "COCTemplatePropertyEditor", false)]
    public class COCTemplateAspxComboboxPropertyEditor : ASPxPropertyEditor
    {
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        ASPxComboBox dropDownControl = null;
        public COCTemplateAspxComboboxPropertyEditor(
        Type objectType, IModelMemberViewItem info) : base(objectType, info) { }
        protected override void SetupControl(WebControl control)
        {
            if (ViewEditMode == ViewEditMode.Edit)
            {

            }
        }
        protected override WebControl CreateEditModeControlCore()
        {
            dropDownControl = RenderHelper.CreateASPxComboBox();
            dropDownControl.ValueChanged += EditValueChangedHandler;
            return dropDownControl;
        }
        public override void BreakLinksToControl(bool unwireEventsOnly)
        {
            if (dropDownControl != null)
            {
                dropDownControl.ValueChanged -= new EventHandler(EditValueChangedHandler);
            }
            base.BreakLinksToControl(unwireEventsOnly);
        }
    }
}
