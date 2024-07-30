using DevExpress.ExpressApp;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class FileUploadFilterController : ViewController
    {
        FileDataPropertyEditor FilePropertyEditor;
        MessageTimer timer = new MessageTimer();

        public FileUploadFilterController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "ItemsFileUpload_DetailView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.

            try
            {
                if (View.Id == "ItemsFileUpload_DetailView")
                {
                    FilePropertyEditor = ((DetailView)View).FindItem("InputFile") as FileDataPropertyEditor;
                    if (FilePropertyEditor != null)
                        FilePropertyEditor.ControlCreated += FilePropertyEditor_ControlCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FilePropertyEditor_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                FileDataEdit FileControl = ((FileDataPropertyEditor)sender).Editor;
                if (FileControl != null)
                    FileControl.UploadControlCreated += control_UploadControlCreated;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void control_UploadControlCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxUploadControl FileUploadControl = ((FileDataEdit)sender).UploadControl;
                FileUploadControl.ValidationSettings.AllowedFileExtensions = new String[] { ".xlsx", ".xls" };
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
