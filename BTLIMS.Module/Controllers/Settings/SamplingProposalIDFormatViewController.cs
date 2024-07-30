using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SamplingProposalIDFormatViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        SamplingManagementInfo objSMInfo = new SamplingManagementInfo();
        string validatejs = @"function(s, e){
                                var regex = /[0-9]/;   
                                if (!regex.test(e.htmlEvent.key) || s.GetInputElement().value.length == parseInt(sessionStorage.getItem('SequentialNumber'))) {
                                    e.htmlEvent.returnValue = false;
                                }}";
        public SamplingProposalIDFormatViewController()
        {
            InitializeComponent();
            TargetViewId = "SamplingProposalIDFormat_DetailView";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ModificationsController modificationcontroller = Frame.GetController<ModificationsController>();
                if (modificationcontroller != null)
                {
                    modificationcontroller.SaveAction.Executing += SaveAction_Executing;
                    modificationcontroller.SaveAndCloseAction.Executing += SaveAction_Executing;
                    modificationcontroller.SaveAndNewAction.Executing += SaveAction_Executing;
                }
                NewObjectViewController newController = Frame.GetController<NewObjectViewController>();
                newController.NewObjectAction.Active["RemoveNewButton"] = false;
                DeleteObjectsViewController deleteObjectsView = Frame.GetController<DeleteObjectsViewController>();
                deleteObjectsView.DeleteAction.Active["RemoveDeleteButton"] = false;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "SamplingProposalIDFormat_DetailView")
                {
                    SamplingProposalIDFormat jobid = (SamplingProposalIDFormat)View.CurrentObject;
                    if (jobid != null && jobid.Prefix == YesNoFilter.Yes && jobid.PrefixValue == null)
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("PrefixValue should not be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    if (jobid != null)
                    {
                        objSMInfo.SamplingIDDigit = jobid.SampleIDDigit;
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
            try
            {
                base.OnViewControlsCreated();
                ASPxIntPropertyEditor propertyEditorCutOff = ((DetailView)View).FindItem("NumberStart") as ASPxIntPropertyEditor;
                if (propertyEditorCutOff != null)
                {
                    ASPxSpinEdit textBox = propertyEditorCutOff.Editor;
                    if (textBox != null)
                    {
                        SamplingProposalIDFormat format = (SamplingProposalIDFormat)View.CurrentObject;
                        if (format != null)
                        {
                            textBox.ClientSideEvents.Init = @"function(s, e){ if(sessionStorage.getItem('SequentialNumber') == null) { sessionStorage.setItem('SequentialNumber', " + format.SequentialNumber + "); } }";
                            textBox.JSProperties["cpSequenceNumber"] = format.SequentialNumber;
                        }
                        textBox.ClientSideEvents.KeyPress = validatejs;
                    }
                }
                ASPxIntPropertyEditor propertyEditor = ((DetailView)View).FindItem("SequentialNumber") as ASPxIntPropertyEditor;
                if (propertyEditor != null)
                {
                    ASPxSpinEdit textBox = propertyEditor.Editor;
                    if (textBox != null)
                    {
                        textBox.ClientSideEvents.ValueChanged = @"function(s, e){ sessionStorage.setItem('SequentialNumber', s.GetInputElement().value); }";
                    }
                }
                foreach (ViewItem item in ((DetailView)View).Items.Where(a => a.GetType() == typeof(ASPxStringPropertyEditor)))
                {
                    if (item.Id != "JobIDFormatDisplay" && item.Id != "SampleIDFormatDisplay")
                    {
                        ASPxStringPropertyEditor propEditor = item as ASPxStringPropertyEditor;
                        if (propEditor != null && propEditor.Editor != null)
                        {
                            ASPxTextEdit TextBox = propEditor.Editor;
                            if (TextBox != null)
                            {
                                TextBox.ControlStyle.CssClass = "JobFormat";
                            }
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
            try
            {
                base.OnDeactivated();
                ModificationsController modificationcontroller = Frame.GetController<ModificationsController>();
                if (modificationcontroller != null)
                {
                    modificationcontroller.SaveAction.Executing -= SaveAction_Executing;
                    modificationcontroller.SaveAndCloseAction.Executing -= SaveAction_Executing;
                    modificationcontroller.SaveAndNewAction.Executing -= SaveAction_Executing;
                }
                NewObjectViewController newController = Frame.GetController<NewObjectViewController>();
                newController.NewObjectAction.Active.RemoveItem("RemoveNewButton");
                DeleteObjectsViewController deleteObjectsView = Frame.GetController<DeleteObjectsViewController>();
                deleteObjectsView.DeleteAction.Active.RemoveItem("RemoveDeleteButton");
                WebWindow.CurrentRequestWindow.RegisterClientScript("alrt", "sessionStorage.removeItem('SequentialNumber');");
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
