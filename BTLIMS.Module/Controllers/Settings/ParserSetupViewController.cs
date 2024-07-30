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
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ParserSetupViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public ParserSetupViewController()
        {
            InitializeComponent();
            TargetViewId = "InstrumentSoftware_ListView;" + "InstrumentSoftware_DetailView;"; 
        }
        protected override void OnActivated()
        {
            base.OnActivated();            
            try
            {
                if (View.Id == "InstrumentSoftware_ListView")
                {
                    if (ShowRetired.Caption == "Hide Retired")
                    {
                        ((ListView)View).CollectionSource.Criteria.Clear();
                        ((ListView)View).CollectionSource.Criteria["HideRetired"] = CriteriaOperator.Parse("[Retire] = False");
                        ShowRetired.Caption = "Show Retired";
                        ShowRetired.ToolTip = "Show Retired";
                        ShowRetired.ImageName = "Action_ShowItemOnDashboard";
                    }
                    Frame.GetController<NewObjectViewController>().NewObjectAction.CustomGetTotalTooltip += NewObjectAction_CustomGetTotalTooltip;
                }
                if (View.Id == "InstrumentSoftware_DetailView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void NewObjectAction_CustomGetTotalTooltip(object sender, CustomGetTotalTooltipEventArgs e)
        {
            try
            {
                e.Tooltip = ((ActionBase)sender).Enabled ? "New Parser" : null;
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
            try
            {
                if (View.Id == "InstrumentSoftware_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null && gridListEditor.Grid.Columns != null)
                    {
                        if (gridListEditor.Grid.Columns["Retire"].Visible == true)
                        {
                            gridListEditor.Grid.Columns["Retire"].Visible = false;
                        }
                        if (gridListEditor.Grid.Columns["DateRetired"].Visible == true)
                        {
                            gridListEditor.Grid.Columns["DateRetired"].Visible = false;
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
            try
            {
                if (View.Id == "InstrumentSoftware_DetailView")
                {
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                }
                Frame.GetController<NewObjectViewController>().NewObjectAction.CustomGetTotalTooltip -= NewObjectAction_CustomGetTotalTooltip;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ShowRetired_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "InstrumentSoftware_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    bool showRetired = ShowRetired.Caption == "Show Retired";
                    if (showRetired)
                    {
                        ((ListView)View).CollectionSource.Criteria.Clear();
                        ((ListView)View).CollectionSource.Criteria["ShowRetired"] = CriteriaOperator.Parse("[Retire] = True");                       
                        ShowRetired.Caption = "Hide Retired";
                        ShowRetired.ToolTip = "Hide Retired";
                        ShowRetired.ImageName = "State_ItemVisibility_Hide";

                        if (gridListEditor != null)
                        {
                            if (gridListEditor.Grid.Columns["Retire"] != null && gridListEditor.Grid.Columns["Retire"].Visible == false)
                            {
                                gridListEditor.Grid.Columns["Retire"].Visible = true;
                            }
                            if (gridListEditor.Grid.Columns["DateRetired"] != null && gridListEditor.Grid.Columns["DateRetired"].Visible == false)
                            {
                                gridListEditor.Grid.Columns["DateRetired"].Visible = true;
                            } 
                        }
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria.Clear();
                        ((ListView)View).CollectionSource.Criteria["HideRetired"] = CriteriaOperator.Parse("[Retire] = False");
                        ShowRetired.Caption = "Show Retired";
                        ShowRetired.ToolTip = "Show Retired";
                        ShowRetired.ImageName = "Action_ShowItemOnDashboard";

                        if (gridListEditor != null)
                        {
                            if (gridListEditor.Grid.Columns["Retire"] != null && gridListEditor.Grid.Columns["Retire"].Visible == true)
                            {
                                gridListEditor.Grid.Columns["Retire"].Visible = false;
                            }
                            if (gridListEditor.Grid.Columns["DateRetired"] != null && gridListEditor.Grid.Columns["DateRetired"].Visible == true)
                            {
                                gridListEditor.Grid.Columns["DateRetired"].Visible = false;
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

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "InstrumentSoftware_DetailView" && e.PropertyName == "Retire" && e.OldValue != e.NewValue)
                {
                    InstrumentSoftware objSampleCheckin = (InstrumentSoftware)View.CurrentObject;
                    if (objSampleCheckin != null)
                    {
                        if (e.NewValue != null && objSampleCheckin.Retire == true)
                        {
                            objSampleCheckin.DateRetired = DateTime.Now;
                        }
                        else
                        {
                            objSampleCheckin.DateRetired = DateTime.MinValue;
                        }
                    }
                    //ObjectSpace.CommitChanges();
                    View.Refresh();
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
