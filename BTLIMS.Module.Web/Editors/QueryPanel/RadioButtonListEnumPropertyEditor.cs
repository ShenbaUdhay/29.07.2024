using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.Web;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Editors.QueryPanel
{
    [PropertyEditor(typeof(Enum), false)]
    public class RadioButtonListEnumPropertyEditor : WebPropertyEditor
    {
        private EnumDescriptor enumDescriptor;
        private Dictionary<ASPxRadioButtonList, object> controlsHash = new Dictionary<ASPxRadioButtonList, object>();
        private Dictionary<string, object> controlsHash2 = new Dictionary<string, object>();
        private ASPxRadioButtonList rbl;
        curlanguage objLanguage = new curlanguage();

        public RadioButtonListEnumPropertyEditor(Type objectType, IModelMemberViewItem info)
            : base(objectType, info)
        {
            this.enumDescriptor = new EnumDescriptor(MemberInfo.MemberType);
        }

        protected override WebControl CreateEditModeControlCore()
        {
            Panel placeHolder = new Panel();
            controlsHash.Clear();

            rbl = new ASPxRadioButtonList();
            rbl.ID = "radiobuttonlist" + base.propertyName;
            rbl.ValueType = enumDescriptor.EnumType;
            if (View.Id == "JobIDFormat_DetailView" || View.Id == "SamplingProposalIDFormat_DetailView" || View.Id == "ResultEntryDefaultSettings_DetailView" || View.Id == "FieldDataEntryDefaultSettings_DetailView" || View.Id == "QuotesDefaultSettings_DetailView")
            {
                rbl.Border.BorderWidth = 0;
                rbl.Paddings.PaddingLeft = 0;
                //radiobuttonlistPrefix
            }
            foreach (object enumValue in enumDescriptor.Values)
            {
                string[] enumString;
                string enumCaption = string.Empty;
                //int i = 0;
                if (enumDescriptor.GetCaption(enumValue).Contains("_"))
                {
                    enumString = enumDescriptor.GetCaption(enumValue).Split('_');
                    if (enumString.Length > 0)
                    {
                        enumCaption = enumString[1];
                    }

                }

                rbl.Items.Add(enumDescriptor.GetCaption(enumValue), enumValue);
                controlsHash2.Add(enumDescriptor.GetCaption(enumValue), enumValue);
                rbl.SelectedIndexChanged += rbl_SelectedIndexChanged;
            }

            //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //conn.Open();
            //SqlCommand cmd = new SqlCommand("SELECT [dbo].[getCurrentLanguage] ()", conn);
            //string value = cmd.ExecuteScalar().ToString();
            //conn.Close();

            if (objLanguage.strcurlanguage == "En")
            {
                if (rbl.Items.Count >= 4)
                {
                    if (rbl.Items[0] != null)
                    {
                        rbl.Items[0].Text = "1M";
                    }
                    if (rbl.Items[1] != null)
                    {
                        rbl.Items[1].Text = "3M";
                    }
                    if (rbl.Items[2] != null)
                    {
                        rbl.Items[2].Text = "6M";
                    }
                    if (rbl.Items[3] != null)
                    {
                        rbl.Items[3].Text = "1Y";
                    }
                    rbl.Items[0].Selected = true;
                }
            }
            else
            {
                if (rbl.Items.Count >= 4)
                {
                    if (rbl.Items[0] != null)
                    {
                        rbl.Items[0].Text = "1月";
                    }
                    if (rbl.Items[1] != null)
                    {
                        rbl.Items[1].Text = "3月";
                    }
                    if (rbl.Items[2] != null)
                    {
                        rbl.Items[2].Text = "6月";
                    }
                    if (rbl.Items[3] != null)
                    {
                        rbl.Items[3].Text = "1年";
                    }
                    rbl.Items[0].Selected = true;
                }
            }

            #region hide DevExpress Code Original
            //foreach (object enumValue in enumDescriptor.Values)
            //{

            //    ASPxRadioButton radioButton = new ASPxRadioButton();
            //    radioButton.ID = "radioButton_" + enumValue.ToString();
            //    controlsHash.Add(radioButton, enumValue);
            //    radioButton.Text = enumDescriptor.GetCaption(enumValue);
            //    radioButton.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            //    radioButton.GroupName = propertyName;

            //    placeHolder.Controls.Add(radioButton);
            //}
            #endregion
            controlsHash.Add(rbl, rbl.Items);
            placeHolder.Controls.Add(rbl);
            rbl.RepeatDirection = RepeatDirection.Horizontal;


            return placeHolder;
        }

        private void rbl_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditValueChangedHandler(sender, e);
        }

        #region hide DevExpress Code Original

        //void radioButton_CheckedChanged(object sender, EventArgs e)
        //{
        //    EditValueChangedHandler(sender, e);
        //}

        #endregion

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

        #region hide DevExpress Code Original

        //protected override void ReadEditModeValueCore()
        //{
        //    object value = PropertyValue;
        //    if (value != null)
        //    {
        //        foreach (ASPxRadioButton radioButton in Editor.Controls)
        //        {
        //            radioButton.Checked = value.Equals(controlsHash[radioButton]);
        //        }
        //    }
        //}

        //protected override object GetControlValueCore()
        //{
        //    object result = null;
        //    foreach (ASPxRadioButton radioButton in Editor.Controls)
        //    {
        //        if (radioButton.Checked)
        //        {
        //            result = controlsHash[radioButton];
        //            break;
        //        }
        //    }
        //    return result;
        //}

        #endregion

        protected override object GetControlValueCore()
        {
            object result = null;
            foreach (ASPxRadioButtonList radioButton in Editor.Controls)
            {
                if (radioButton.SelectedItem.Selected)
                {
                    result = radioButton.SelectedItem.Value;
                    break;
                }
            }
            return result;
        }
        //protected override void WriteValueCore()
        //{
        //   // base.WriteValueCore();
        //    object value = PropertyValue;
        //    if (value != null)
        //    {
        //        foreach (ASPxRadioButton radioButton in Editor.Controls)
        //        {
        //            if (radioButton.Text == enumDescriptor.GetCaption(PropertyValue))
        //            {
        //                radioButton.Checked = value.Equals(controlsHash[radioButton]);
        //            }
        //        }
        //    }
        //}

        #region Hide DevExpress Code Original

        //public override void BreakLinksToControl(bool unwireEventsOnly)
        //{
        //    if (Editor != null)
        //    {
        //        foreach (ASpx radioButton in Editor.Controls)
        //        {
        //            radioButton.CheckedChanged -= new EventHandler(rbl_SelectedIndexChanged);
        //        }
        //        if (!unwireEventsOnly)
        //        {
        //            controlsHash.Clear();
        //        }
        //    }
        //    base.BreakLinksToControl(unwireEventsOnly);
        //}

        #endregion

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
        #region hide DevExpress Code Original

        //public override void BreakLinksToControl(bool unwireEventsOnly)
        //{
        //    if (Editor != null)
        //    {
        //        foreach (ASPxRadioButton radioButton in Editor.Controls)
        //        {
        //            radioButton.CheckedChanged -= new EventHandler(radioButton_CheckedChanged);
        //        }
        //        if (!unwireEventsOnly)
        //        {
        //            controlsHash.Clear();
        //        }
        //    }
        //    base.BreakLinksToControl(unwireEventsOnly);
        //}

        #endregion

        protected override void SetImmediatePostDataScript(string script)
        {
            foreach (ASPxRadioButtonList radioButton in controlsHash.Keys)
            {
                radioButton.ClientSideEvents.SelectedIndexChanged = script;
            }
            //foreach (ASPxRadioButton radioButton in controlsHash.Keys)
            //{
            //    radioButton.ClientSideEvents.CheckedChanged = script;
            //}
        }

        public new Panel Editor
        {
            get { return (Panel)base.Editor; }
        }
    }
}
