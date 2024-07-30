using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using Modules.BusinessObjects.QC;

namespace LDM.Module.Web.Controllers.QC
{
    public partial class ImportFileopenController : ObjectViewController<DetailView, SDMSDCImport>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            if ((ObjectSpace is NonPersistentObjectSpace) && (View.CurrentObject == null))
            {
                View.CurrentObject = View.ObjectTypeInfo.CreateInstance();
                View.ViewEditMode = ViewEditMode.Edit;
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
