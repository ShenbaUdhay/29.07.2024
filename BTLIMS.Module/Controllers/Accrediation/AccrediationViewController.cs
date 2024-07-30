using System;
using System.Collections.Generic;
using System.Globalization;
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
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;

namespace LDM.Module.Controllers.Accrediation
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AccrediationViewController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        ICallbackManagerHolder sheet;
        public AccrediationViewController()
        {
            InitializeComponent();
            TargetViewId = "Accrediation_DetailView;" + "Accrediation_ListView;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            // Perform various tasks depending on the target View.
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                 if (View != null && (View.Id == "Accrediation_DetailView") || (View.Id== "Accrediation_ListView"))
                 {
                    Modules.BusinessObjects.Setting.Accrediation objaccrediation = View.CurrentObject as Modules.BusinessObjects.Setting.Accrediation;
                    if (e.PropertyName == "lAccrediation" || e.PropertyName== "Accrediation")
                    {
                     
                      //  Modules.BusinessObjects.Setting.Accrediation objaccrediation = View.CurrentObject as Modules.BusinessObjects.Setting.Accrediation;
                        string oldvalues =(string)e.OldValue;
                        if (!string.IsNullOrEmpty(oldvalues)) 
                        {
                            CriteriaOperator criteria = CriteriaOperator.Parse("IsNullOrEmpty([GCRecord])");
                            IList<Testparameter> lsttestparameter = View.ObjectSpace.GetObjects<Testparameter>(criteria);
                            // List<string> findaccrediation = testparameter.Where(i => i.lAccrediation != null).Select(i => i.lAccrediation).ToList();
                            var findaccrediation = lsttestparameter.Where(i => i.lAccrediation != null).SelectMany(i => i.lAccrediation.Split(';')).ToList();
                            var accresult = findaccrediation.Where(i => i.Contains(oldvalues)).ToList();
                            if (accresult.Count > 0)
                            {
                                objaccrediation.lAccrediation = oldvalues;
                                Application.ShowViewStrategy.ShowMessage("Value already used can't edit.", InformationType.Info, 3000, InformationPosition.Top);
                            }
                        }
                       
                    }
                    if(e.PropertyName == "DefaultAccrediation")
                    {
                        if (objaccrediation != null && objaccrediation.Oid != null)
                        {
                            Modules.BusinessObjects.Setting.Accrediation tatobj = View.ObjectSpace.FindObject<Modules.BusinessObjects.Setting.Accrediation>(CriteriaOperator.Parse("[DefaultAccrediation] = True And [Oid]<>?", objaccrediation.Oid));
                            if (tatobj != null && tatobj.DefaultAccrediation)
                            {
                                WebWindow.CurrentRequestWindow.RegisterClientScript("Openspreadsheet", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('Default has been already enabled, Do you want this Accrediation to be enabled as default'); {0}", sheet.CallbackManager.GetScript("openspreadsheet", "openconfirm")));

                    }
                        }
                    }
                   

                 }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                sheet = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                sheet.CallbackManager.RegisterHandler("openspreadsheet", this);
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);

            }

            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            try
            {
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ProcessAction(string parameter)
        {
            try
            {
                if (parameter == "true")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    Modules.BusinessObjects.Setting.Accrediation selectedObject = View.CurrentObject as Modules.BusinessObjects.Setting.Accrediation;
                    if (selectedObject != null)
                    {
                        Modules.BusinessObjects.Setting.Accrediation accobj = View.ObjectSpace.FindObject<Modules.BusinessObjects.Setting.Accrediation>(CriteriaOperator.Parse("[DefaultAccrediation] = True And [Oid]<>?", selectedObject.Oid));

                        if (accobj != null)
                        {

                            accobj.DefaultAccrediation = false;
                        }
                    }
                }
                else
                {
                    Modules.BusinessObjects.Setting.Accrediation selectedObject = View.CurrentObject as Modules.BusinessObjects.Setting.Accrediation;
                    if (selectedObject != null)
                    {
                        selectedObject.DefaultAccrediation = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);

            }
        }
    }
}
