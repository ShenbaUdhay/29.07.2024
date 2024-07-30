using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Editors
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    [PropertyEditor(typeof(Enum), true)]
    public partial class EnumRadioButtonPropertyEditor : WebPropertyEditor
    {
        private EnumDescriptor enumDescriptor;
        private Dictionary<ASPxRadioButton, object> controlsHash = new Dictionary<ASPxRadioButton, object>();
        public EnumRadioButtonPropertyEditor(Type objectType, IModelMemberViewItem info)
            : base(objectType, info)
        {
            this.enumDescriptor = new EnumDescriptor(MemberInfo.MemberType);
        }
        protected override WebControl CreateEditModeControlCore()
        {
            Panel placeHolder = new Panel();
            controlsHash.Clear();

            foreach (object enumValue in enumDescriptor.Values)
            {
                if (enumValue != null && (View.Id == "NPPLMStereoscopicObservation_DetailView" && enumDescriptor.GetCaption(enumValue) != "Dummy"))
                {
                    ASPxRadioButton radioButton = new ASPxRadioButton();
                    radioButton.ID = "radioButton_" + enumValue.ToString();
                    radioButton.Text = enumDescriptor.GetCaption(enumValue); //enumValue ? "DayScholar" : "Hostellar";
                    controlsHash.Add(radioButton, enumValue);
                    radioButton.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
                    radioButton.GroupName = propertyName;
                    placeHolder.Controls.Add(radioButton);
                }
            }
            return placeHolder;
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            EditValueChangedHandler(sender, e);
        }

        protected override string GetPropertyDisplayValue()
        {
            return enumDescriptor.GetCaption(PropertyValue);
        }

        protected override void ReadEditModeValueCore()
        {
            object value = PropertyValue;
            if (value != null)
            {
                foreach (ASPxRadioButton radioButton in Editor.Controls)
                {
                    radioButton.Checked = value.Equals(controlsHash[radioButton]);
                }
            }
        }

        protected override object GetControlValueCore()
        {
            object result = null;
            foreach (ASPxRadioButton radioButton in Editor.Controls)
            {
                if (radioButton.Checked)
                {
                    result = controlsHash[radioButton];
                    break;
                }
            }
            return result;
        }

        public override void BreakLinksToControl(bool unwireEventsOnly)
        {
            if (Editor != null)
            {
                foreach (ASPxRadioButton radioButton in Editor.Controls)
                {
                    radioButton.CheckedChanged -= new EventHandler(radioButton_CheckedChanged);
                }
                if (!unwireEventsOnly)
                {
                    controlsHash.Clear();
                }
            }
            base.BreakLinksToControl(unwireEventsOnly);
        }

        protected override void SetImmediatePostDataScript(string script)
        {
            foreach (ASPxRadioButton radioButton in controlsHash.Keys)
            {
                radioButton.ClientSideEvents.CheckedChanged = script;
            }
        }

        public new Panel Editor
        {
            get { return (Panel)base.Editor; }
        }
    }
}
