using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting.PermitLibraries;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PermitViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public PermitViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "PermitLibrary_DetailView;"+ "PermitLibrary_Permitsetups_ListView;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            if(View.Id== "PermitLibrary_DetailView")
            {
                Frame.GetController<ModificationsController>().SaveAction.Executing += SaveAction_Executing;
            }
            else if(View.Id== "PermitLibrary_Permitsetups_ListView")
            {
                Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing; ;
            }
        }

        private void DeleteAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                foreach(Permitsetup objPS in View.SelectedObjects)
                {
                    Modules.BusinessObjects.Setting.Parameter objparam = ObjectSpace.GetObject<Modules.BusinessObjects.Setting.Parameter>(objPS.Parameter);
                    if(objparam!=null)
                    {
                        Permitsetup objPSs = ObjectSpace.FindObject<Permitsetup>(CriteriaOperator.Parse("[Parameter] = ? And [PermitLibrary] <> ?", objparam, objPS.PermitLibrary));
                        if(objPSs==null)
                        {
                            objparam.Limit = false;
                        }
                    }
                }
                //PermitLibrary objPL = View.CurrentObject as PermitLibrary;
                //if (objPL != null && objPL.Permitsetups.Count > 0)
                //{
                //    IList<Modules.BusinessObjects.Setting.Parameter> lstparam = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Parameter>(new InOperator("Oid", objPL.Permitsetups.Select(i => i.Parameter.Oid)));
                //    foreach (Modules.BusinessObjects.Setting.Parameter objpara in lstparam.Where(i => !i.Limit))
                //    {
                //        objpara.Limit = true;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        { 
            try
            {
                PermitLibrary objPL = View.CurrentObject as PermitLibrary;
                if (objPL!=null&& objPL.Permitsetups.Count>0)
                {
                    IList<Modules.BusinessObjects.Setting.Parameter> lstparam = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Parameter>(new InOperator("Oid", objPL.Permitsetups.Select(i=>i.Parameter.Oid))); 
                    foreach(Modules.BusinessObjects.Setting.Parameter objpara in lstparam.Where(i=>!i.Limit))
                    {
                        objpara.Limit = true;
                    }
                }
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
            if (View.Id == "PermitLibrary_DetailView")
            {
                Frame.GetController<ModificationsController>().SaveAction.Executing -= SaveAction_Executing;
            }
            else if (View.Id == "PermitLibrary_Permitsetups_ListView")
            {
                Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing; ;
            }
        }
    }
}
