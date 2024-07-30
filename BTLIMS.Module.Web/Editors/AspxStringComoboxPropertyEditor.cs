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

    [PropertyEditor(typeof(string), "StringTestComoboxPropertyEditor", false)]
    public class AspxStringComoboxPropertyEditor : ASPxPropertyEditor
    {
        ASPxComboBox dropDownControl = null;
        public AspxStringComoboxPropertyEditor(
        Type objectType, IModelMemberViewItem info) : base(objectType, info) { }
        protected override void SetupControl(WebControl control)
        {
            if (View != null && ViewEditMode == ViewEditMode.Edit)
            {
                if (View.Id == "DWQRReportTemplateSetup_DetailView" || View.Id == "DrinkingWaterQualityReports_DetailView")
                {
                    SelectedData sprocReport = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("SP_MergePackageAndReportName");
                    if (sprocReport.ResultSet != null)
                    {
                        foreach (SelectStatementResultRow row in sprocReport.ResultSet[0].Rows)
                        {
                            ((ASPxComboBox)control).Items.Add(row.Values[0].ToString());
                        }
                    } 
                }
                else if(View.Id != "Samplecheckin_DetailView_Copy_SampleRegistration")
                {
                SelectedData sprocReport = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("SelectTestName_SP");
                if (sprocReport.ResultSet != null)
                {
                    foreach (SelectStatementResultRow row in sprocReport.ResultSet[0].Rows)
                    {
                        ((ASPxComboBox)control).Items.Add(row.Values[0].ToString());
                        }
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
