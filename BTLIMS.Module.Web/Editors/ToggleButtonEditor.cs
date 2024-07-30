using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using System;
using System.Web.UI.WebControls;

namespace Labmaster.Module.Web.Editors
{
    [PropertyEditor(typeof(bool), false)]
    public class ToggleButtonEditor : ASPxPropertyEditor
    {
        ASPxCheckBox togglebutton;
        public ToggleButtonEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        protected override WebControl CreateEditModeControlCore()
        {
            togglebutton = new ASPxCheckBox();
            togglebutton.ToggleSwitchDisplayMode = ToggleSwitchDisplayMode.Always;
            togglebutton.ValueChanged += Togglebutton_ValueChanged;
            return togglebutton;
        }

        private void Togglebutton_ValueChanged(object sender, EventArgs e)
        {
            EditValueChangedHandler(sender, e);
        }

        protected override object GetControlValueCore()
        {
            return ((ASPxCheckBox)Editor).Value;
        }

        protected override void ReadEditModeValueCore()
        {
            ((ASPxCheckBox)Editor).Checked = (bool)PropertyValue;
        }

        protected override void SetImmediatePostDataScript(string script)
        {
            base.SetImmediatePostDataScript(script);
            togglebutton.ClientSideEvents.CheckedChanged = script;
            //togglebutton.ClientSideEvents.ValueChanged = script;
        }

    }
}
