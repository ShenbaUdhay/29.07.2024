using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo.DB;
using System;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Editors
{

    [PropertyEditor(typeof(string), "SampleToPropertyEditor", false)]
    public class AspxStringSampleToComboBox : ASPxPropertyEditor
    {
        ASPxComboBox dropDownControl = null;
        public AspxStringSampleToComboBox(
        Type objectType, IModelMemberViewItem info) : base(objectType, info) { }
        protected override void SetupControl(WebControl control)
        {
            if (View != null && ViewEditMode == ViewEditMode.Edit)
            {
                SelectedData sprocReport = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("GetToValues");
                if (sprocReport.ResultSet != null)
                {
                    foreach (SelectStatementResultRow row in sprocReport.ResultSet[0].Rows)
                    {
                        ((ASPxComboBox)control).Items.Add(row.Values[0].ToString());
                    }
                }
            }
        }
        protected override WebControl CreateEditModeControlCore()
        {
            dropDownControl = RenderHelper.CreateASPxComboBox();
            dropDownControl.ValueChanged += EditValueChangedHandler;
            dropDownControl.ValueType = typeof(System.String);
            dropDownControl.DropDownStyle = DropDownStyle.DropDown;
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


    //[EditorAlias("CultureInfoPropertyEditor")]
    //public String Code
    //{
    //    get { return GetPropertyValue<String>(nameof(Code)); }
    //    set { SetPropertyValue(nameof(Code), value); }


    //}
}
