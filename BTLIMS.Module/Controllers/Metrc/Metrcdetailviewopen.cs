using DevExpress.ExpressApp;

namespace LDM.Module.Controllers.Metrc
{
    public partial class detailviewopen : ObjectViewController<DetailView, Modules.BusinessObjects.Metrc.MetrcIncoming>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            if ((ObjectSpace is NonPersistentObjectSpace) && (View.CurrentObject == null))
            {
                View.CurrentObject = View.ObjectTypeInfo.CreateInstance();
                View.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
