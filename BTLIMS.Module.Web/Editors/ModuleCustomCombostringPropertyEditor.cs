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

    [PropertyEditor(typeof(String), "ModuleNamePropertyEditor", false)]
    public class ModuleCustomCombostringPropertyEditor : ASPxPropertyEditor
    {
        ASPxComboBox dropDownControl = null;
        public ModuleCustomCombostringPropertyEditor(
        Type objectType, IModelMemberViewItem info) : base(objectType, info) { }
        protected override void SetupControl(WebControl control)
        {
            if (ViewEditMode == ViewEditMode.Edit)
            {
                SelectedData sprocReport = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("SelectModuleName_Sp");
                if (sprocReport.ResultSet != null)
                {
                    foreach (SelectStatementResultRow row in sprocReport.ResultSet[0].Rows)
                    {
                        ((ASPxComboBox)control).Items.Add(row.Values[0].ToString());
                    }
                }
                //if (SRInfo.lstReportName!=null &&SRInfo.lstReportName.Count>0)
                //{
                //    foreach (string row in SRInfo.lstReportName)
                //    {
                //        ((ASPxComboBox)control).Items.Add(row);
                //    }
                //}
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