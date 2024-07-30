using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using System;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Editors
{
    [PropertyEditor(typeof(double), Modules.BusinessObjects.SampleManagement.Samplecheckin.ProgressPropertyAlias, false)]
    public class WebProgressPropertyEditor : ASPxPropertyEditor
    {
        public WebProgressPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }

        private void SetProgressValue()
        {
            TaskProgressBar progressBar = InplaceViewModeEditor as TaskProgressBar;
            if (progressBar == null) progressBar = Editor as TaskProgressBar;
            if (progressBar != null)
            {
                progressBar.IndicatorStyle.BackColor = System.Drawing.Color.LightGreen;
                progressBar.ProgressValue = PropertyValue;
            }
        }
        protected override WebControl CreateEditModeControlCore()
        {
            TaskProgressBar result = new TaskProgressBar();
            result.Width = Unit.Percentage(100);
            result.ID = "TaskProgressBar";
            return result;
        }
        protected override WebControl CreateViewModeControlCore()
        {
            return CreateEditModeControlCore();
        }
        protected override void ReadViewModeValueCore()
        {
            base.ReadViewModeValueCore();
            SetProgressValue();
        }
        protected override void ReadEditModeValueCore()
        {
            base.ReadEditModeValueCore();
            SetProgressValue();
        }
    }
    public class TaskProgressBar : ASPxProgressBar
    {
        private double progressValue = 0;
        public object ProgressValue
        {
            get { return progressValue; }
            set
            {
                progressValue = (double)value;
                this.Value = progressValue;
            }
        }
    }
}
