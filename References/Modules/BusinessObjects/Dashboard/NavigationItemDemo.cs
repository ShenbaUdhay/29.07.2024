using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.BaseImpl;


namespace Modules.BusinessObjects.Dashboard
{
    class NavigationItemDemo : BaseObject, IModelApplicationNavigationItems
    {
        private IModelRootNavigationItems _NavigationItems;
        public IModelRootNavigationItems NavigationItems
        {
            get
            {
                return _NavigationItems;
                //throw new NotImplementedException();
            }
            set
            {

            }
        }

    }
}
