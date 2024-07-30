using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo.DB;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Editors
{
    [PropertyEditor(typeof(String), "BusinessObjectPropertyEditor", false)]
    public class BusinessObjectCustomstringComboPropertyEditor : ASPxPropertyEditor
    {
        ASPxComboBox dropDownControl = null;
        CalibrationLogbookInfo CLBinfo = new CalibrationLogbookInfo();
        public BusinessObjectCustomstringComboPropertyEditor(
        Type objectType, IModelMemberViewItem info) : base(objectType, info) { }
        protected override void SetupControl(WebControl control)
        {
            if (ViewEditMode == ViewEditMode.Edit)
            {
                if (View.ObjectTypeInfo.Type == typeof(HelpCenter))
                {
                    HelpCenter objHelp = (HelpCenter)View.CurrentObject;
                    if (objHelp != null && !string.IsNullOrEmpty(objHelp.Module))
                    {
                        SelectedData sprocReport = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("SelectBusinessObject_Sp", new OperandValue(objHelp.Module));
                        if (sprocReport.ResultSet != null)
                        {
                            foreach (SelectStatementResultRow row in sprocReport.ResultSet[0].Rows)
                            {
                                ((ASPxComboBox)control).Items.Add(row.Values[0].ToString());
                            }
                        }
                    }
                }
                else if (View.ObjectTypeInfo.Type == typeof(CustomReportBuilder))
                {
                    CustomReportBuilder objCRB = (CustomReportBuilder)View.CurrentObject;
                    if (objCRB != null && !string.IsNullOrEmpty(objCRB.Module))
                    {
                        SelectedData sprocReport = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("SelectBusinessObject_Sp", new OperandValue(objCRB.Module));
                        if (sprocReport.ResultSet != null)
                        {
                            foreach (SelectStatementResultRow row in sprocReport.ResultSet[0].Rows)
                            {
                                ((ASPxComboBox)control).Items.Add(row.Values[0].ToString());
                            }
                        }
                    }
                }
                ////else if (View.ObjectTypeInfo.Type == typeof(CalibrationTrending))
                ////{
                ////    if(CLBinfo.NotebookBuilderOid!=null)
                ////    {
                ////        List<string> lstValue = View.ObjectSpace.GetObjects<CalibrationSettings>(CriteriaOperator.Parse("[NotebookBuilder]=?", CLBinfo.NotebookBuilderOid.Oid)).ToList().Where(i=>i.ReferenceValue!=null).Select(i=>i.ReferenceValue).Distinct().ToList();
                ////        if (lstValue != null && lstValue.Count>0)
                ////        {
                ////            foreach (string row in lstValue.ToList())
                ////            {
                ////                ((ASPxComboBox)control).Items.Add(row);
                ////            }
                ////        }
                ////    }

                ////}
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