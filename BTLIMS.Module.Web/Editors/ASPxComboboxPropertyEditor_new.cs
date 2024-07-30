using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Linq;
using System.Web.UI.WebControls;

[PropertyEditor(typeof(String), "ASPxComboBoxEditor", false)]
public class ASPxComboBoxEditor : ASPxPropertyEditor
{
    ASPxComboBox dropDownControl = null;
    PLMInfo plmInfo = new PLMInfo();
    public ASPxComboBoxEditor(
    Type objectType, IModelMemberViewItem info) : base(objectType, info) { }
    protected override void SetupControl(WebControl control)
    {
        if (ViewEditMode == ViewEditMode.Edit)
        {
            if (View.Id == "PLMCopyToCombo_DetailView")
            {
                foreach (var pLM in plmInfo.lstPLMSte.OrderBy(a => a.Value).ToDictionary(a => a.Key, a => a.Value))
                {
                    ((ASPxComboBox)control).Items.Add(pLM.Value);
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