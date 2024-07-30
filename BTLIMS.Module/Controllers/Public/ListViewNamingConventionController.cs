using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.Public
{
    public partial class ListViewNamingConventionController : ViewController<ListView>
    {
        MessageTimer timer = new MessageTimer();
        public ListViewNamingConventionController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                if (View.Editor is ASPxGridListEditor GridListEditor)
                {
                    if (GridListEditor.Grid == null)
                    {
                        return;
                    }
                    foreach (GridViewColumn col in GridListEditor.Grid.Columns)
                    {
                        if (!col.Name.Contains("Edit") && !col.Name.Contains("Command") && !string.IsNullOrWhiteSpace(col.Caption))
                        {
                            col.Caption = col.Caption.Replace(" ", string.Empty);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
