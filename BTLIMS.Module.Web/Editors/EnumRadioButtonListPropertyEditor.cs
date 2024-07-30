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
    [PropertyEditor(typeof(Enum), false)]
    public class EnumRadioButtonListPropertyEditor : WebPropertyEditor
    {
        private EnumDescriptor enumDescriptor;
        private Dictionary<ASPxRadioButtonList, object> controlsHash = new Dictionary<ASPxRadioButtonList, object>();
        private Dictionary<string, object> controlsHash2 = new Dictionary<string, object>();
        private ASPxRadioButtonList rbl;

        public EnumRadioButtonListPropertyEditor(Type objectType, IModelMemberViewItem info)
            : base(objectType, info)
        {
            this.enumDescriptor = new EnumDescriptor(MemberInfo.MemberType);
        }

        protected override WebControl CreateEditModeControlCore()
        {
            Panel placeHolder = new Panel();
            controlsHash.Clear();

            rbl = new ASPxRadioButtonList();
            rbl.ID = "enumradiobuttonlist" + base.propertyName;
            rbl.ValueType = enumDescriptor.EnumType;
            foreach (object enumValue in enumDescriptor.Values)
            {
                var id = this.Model.ModelMember.Type;
                if (this.Model.ModelMember.Type != typeof(Modules.BusinessObjects.SampleManagement.QueryMode) || (this.Model.ModelMember.Type == typeof(Modules.BusinessObjects.SampleManagement.QueryMode) && enumValue.ToString() != "QC"))
                {
                    string[] enumString;
                    string enumCaption = string.Empty;
                    if (enumDescriptor.GetCaption(enumValue).Contains("_"))
                    {
                        enumString = enumDescriptor.GetCaption(enumValue).Split('_');
                        if (enumString.Length > 0)
                        {
                            enumCaption = enumString[1];
                        }
                    }

                    //if (View.Id == "ResultEntryQueryPanel_DetailView_Copy" || View.Id == "ResultEntryQueryPanel_DetailView" || View.Id == "ResultEntry_Enter" || View.Id == "SampleParameter_ListView_Copy_ResultEntry")
                    //{
                    //    if (enumDescriptor.GetCaption(enumValue) == "ABID")
                    //    {
                    //        break;
                    //    }
                    //}
                    //var sam = View;
                    rbl.Items.Add(enumDescriptor.GetCaption(enumValue), enumValue);
                    controlsHash2.Add(enumDescriptor.GetCaption(enumValue), enumValue);
                    rbl.SelectedIndexChanged += rbl_SelectedIndexChanged;
                }
            }

            controlsHash.Add(rbl, rbl.Items);
            placeHolder.Controls.Add(rbl);
            rbl.RepeatDirection = RepeatDirection.Horizontal;


            return placeHolder;
        }

        private void rbl_SelectedIndexChanged(object sender, EventArgs e)
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
                foreach (ListEditItem item in rbl.Items)
                {
                    if (item.Text == enumDescriptor.GetCaption(PropertyValue))
                    {
                        rbl.SelectedItem = item;
                    }
                }
            }
        }

        protected override object GetControlValueCore()
        {
            object result = null;
            foreach (ASPxRadioButtonList radioButton in Editor.Controls)
            {
                if (radioButton.SelectedItem != null)
                {
                    if (radioButton.SelectedItem.Selected)
                    {
                        result = radioButton.SelectedItem.Value;
                        break;
                    }
                }
            }
            return result;
        }

        // get error when set the value of radiobutton
        public override void BreakLinksToControl(bool unwireEventsOnly)
        {
            if (Editor != null)
            {
                foreach (ASPxRadioButtonList radioButton in Editor.Controls)
                {
                    //radioButton.CheckedChanged -= new EventHandler(rbl_SelectedIndexChanged);
                }
                if (!unwireEventsOnly)
                {
                    rbl = null;
                    controlsHash2.Clear();
                }
            }
            base.BreakLinksToControl(unwireEventsOnly);
        }

        protected override void SetImmediatePostDataScript(string script)
        {
            foreach (ASPxRadioButtonList radioButton in controlsHash.Keys)
            {
                radioButton.ClientSideEvents.SelectedIndexChanged = script;
            }
        }

        public new Panel Editor
        {
            get { return (Panel)base.Editor; }
        }
    }
}
