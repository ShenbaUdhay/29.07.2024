using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;

namespace LDM.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PermissionViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public PermissionViewController()
        {
            InitializeComponent();
            TargetViewId = "PermissionPolicyRole_LookupListView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View.Id != null && View.Id == "PermissionPolicyRole_LookupListView")
                {
                    IList<CustomSystemRole> objrole = ObjectSpace.GetObjects<CustomSystemRole>(CriteriaOperator.Parse("[ISNavigationPermission] = '0' Or [ISNavigationPermission] Is Null"));
                    if (objrole.Count > 0)
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", objrole);
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
