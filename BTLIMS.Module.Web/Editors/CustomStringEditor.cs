using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo.DB;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Editors
{

    [PropertyEditor(typeof(String), "ReportTemplatePropertyEditor", false)]
    public class CustomStringEditor : ASPxPropertyEditor
    {
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        ASPxComboBox dropDownControl = null;
        public CustomStringEditor(
        Type objectType, IModelMemberViewItem info) : base(objectType, info) { }
        protected override void SetupControl(WebControl control)
        {
            if (ViewEditMode == ViewEditMode.Edit)
            {
                SelectedData sprocReport = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("SelectReportName_SP");
                if (sprocReport.ResultSet != null)
                {
                    foreach (SelectStatementResultRow row in sprocReport.ResultSet[0].Rows)
                    {
                        ((ASPxComboBox)control).Items.Add(row.Values[0].ToString());
                    }
                }
                if (base.propertyName == "ReportTemplate")
                {
                    SelectedData sprocReportpackage = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("SelectReportPackageName_SP");
                    if (sprocReportpackage.ResultSet != null)
                    {
                        foreach (SelectStatementResultRow row in sprocReportpackage.ResultSet[0].Rows)
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