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
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class NavigationViewCaptionController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        public NavigationViewCaptionController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                DefaultSetting objdefsett = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = ? and GCRecord Is Null", objnavigationRefresh.ClickedNavigationItem));
                //if (View is ListView && objdefsett != null && !string.IsNullOrEmpty(objdefsett.NavigationCaption))
                //{
                //    View.Caption = objdefsett.NavigationCaption;
                //}
                //NavigationItem objnavitem = ObjectSpace.FindObject<NavigationItem>(CriteriaOperator.Parse("[NavigationView] = ? and GCRecord Is Null", View.Id));
                //if(objnavitem != null && objnavitem.NavigationId != null)
                //{

                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
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
