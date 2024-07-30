using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.ReagentPreparation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.ReagentPreparation
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ReagentPrepCalculationEditorViewController : ViewController/*,IXafCallbackHandler*/
    {
        MessageTimer timer = new MessageTimer();
        public ReagentPrepCalculationEditorViewController()
        {
            InitializeComponent();
            TargetViewId = "RegentPrepCalculationEditor;" + "NonPersistentReagent_ListView;" + "ReagentOperator_ListView_Formula;" + "RegentPrepCalculationEditor_DetailView;"
                + "NonPersistentReagent_DetailView;" + "VariableAndUnits_ListView";
            ReagentPrepFormula.TargetViewId = "RegentPrepCalculationEditor_DetailView;";

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "NonPersistentReagent_DetailView")
                {
                    View.CurrentObject = View.ObjectTypeInfo.CreateInstance();
                    ((DetailView)View).ViewEditMode = ViewEditMode.Edit;
                    RegentPrepCalculationEditor objRegent = (RegentPrepCalculationEditor)Application.MainWindow.View.CurrentObject;
                    NonPersistentReagent objFormula = (NonPersistentReagent)View.CurrentObject;
                    if (objRegent != null && string.IsNullOrEmpty(objRegent.Formula) && objRegent.CalculationApproch != null)
                    {
                        string str = objRegent.CalculationApproch.Approach.Split(' ').ToList().LastOrDefault().ToString();
                        if (str == "C1")
                        {
                            objFormula.Formula = str + "(" + objRegent.C1Units.Units + ")" + "=";
                        }
                        else if (str == "C2")
                        {
                            objFormula.Formula = str + "(" + objRegent.C2Units.Units + ")" + "=";
                        }
                        else if (str == "V1")
                        {
                            objFormula.Formula = str + "(" + objRegent.V1Units.Units + ")" + "=";
                        }
                        else if (str == "V2")
                        {
                            objFormula.Formula = str + "(" + objRegent.V2Units.Units + ")" + "=";
                        }
                        else if (str == "W1")
                        {
                            objFormula.Formula = str + "(" + objRegent.W1Units.Units + ")" + "=";
                        }
                    }
                    else if (objRegent != null && !string.IsNullOrEmpty(objRegent.Formula))
                    {
                        objFormula.Formula = objRegent.Formula;
                    }
                }
                else if (View.Id == "VariableAndUnits_ListView")
                {
                    ListViewProcessCurrentObjectController listProcessController = Frame.GetController<ListViewProcessCurrentObjectController>();
                    if (listProcessController != null)
                    {
                        listProcessController.CustomProcessSelectedItem += ProcessListViewRowController_CustomProcessSelectedItem;
                    }
                    IList<VariableAndUnits> lstVariable = View.ObjectSpace.GetObjects<VariableAndUnits>(CriteriaOperator.Parse(""));
                    if (lstVariable.Count > 0)
                    {
                        foreach (VariableAndUnits objVar in lstVariable.ToList())
                        {
                            View.ObjectSpace.Delete(objVar);
                        }
                        View.ObjectSpace.CommitChanges();
                    }
                    RegentPrepCalculationEditor objRegent = (RegentPrepCalculationEditor)Application.MainWindow.View.CurrentObject;
                    if (objRegent != null && objRegent.CalculationApproch != null)
                    {
                        if (objRegent.C1Units != null && "C1" != objRegent.CalculationApproch.Approach.Split(' ').ToList().LastOrDefault())
                        {
                            VariableAndUnits newObj = View.ObjectSpace.CreateObject<VariableAndUnits>();
                            newObj.Variable = "C1";
                            newObj.Unit = objRegent.C1Units.Units;
                            ((ListView)View).CollectionSource.Add(newObj);
                        }
                        if (objRegent.C2Units != null && "C2" != objRegent.CalculationApproch.Approach.Split(' ').ToList().LastOrDefault())
                        {
                            VariableAndUnits newObj = View.ObjectSpace.CreateObject<VariableAndUnits>();
                            newObj.Variable = "C2";
                            newObj.Unit = objRegent.C2Units.Units;
                            ((ListView)View).CollectionSource.Add(newObj);
                        }
                        if (objRegent.V1Units != null && "V1" != objRegent.CalculationApproch.Approach.Split(' ').ToList().LastOrDefault())
                        {
                            VariableAndUnits newObj = View.ObjectSpace.CreateObject<VariableAndUnits>();
                            newObj.Variable = "V1";
                            newObj.Unit = objRegent.V1Units.Units;
                            ((ListView)View).CollectionSource.Add(newObj);
                        }
                        if (objRegent.V2Units != null && "V2" != objRegent.CalculationApproch.Approach.Split(' ').ToList().LastOrDefault())
                        {
                            VariableAndUnits newObj = View.ObjectSpace.CreateObject<VariableAndUnits>();
                            newObj.Variable = "V2";
                            newObj.Unit = objRegent.V2Units.Units;
                            ((ListView)View).CollectionSource.Add(newObj);
                        }
                        if (objRegent.W1Units != null && "W1" != objRegent.CalculationApproch.Approach.Split(' ').ToList().LastOrDefault())
                        {
                            VariableAndUnits newObj = View.ObjectSpace.CreateObject<VariableAndUnits>();
                            newObj.Variable = "W1";
                            newObj.Unit = objRegent.W1Units.Units;
                            ((ListView)View).CollectionSource.Add(newObj);
                        }
                    }
                }
                else if (View.Id == "ReagentOperator_ListView_Formula")
                {
                    ListViewProcessCurrentObjectController listProcessController = Frame.GetController<ListViewProcessCurrentObjectController>();
                    if (listProcessController != null)
                    {
                        listProcessController.CustomProcessSelectedItem += ProcessListViewRowController_CustomProcessSelectedItem;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }

        private void ProcessListViewRowController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject.GetType() == typeof(VariableAndUnits))
                {
                    if (((VariableAndUnits)e.InnerArgs.CurrentObject).Variable != null)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView view = nestedFrame.ViewItem.View;
                        DashboardViewItem dvFormula = ((DashboardView)view).FindItem("Formula") as DashboardViewItem;
                        if (dvFormula != null && dvFormula.InnerView != null)
                        {
                            VariableAndUnits obj = (VariableAndUnits)e.InnerArgs.CurrentObject;
                            NonPersistentReagent objCurrObj = (NonPersistentReagent)dvFormula.InnerView.CurrentObject;
                            objCurrObj.Formula = objCurrObj.Formula + obj.Variable + "(" + obj.Unit + ")";
                            dvFormula.InnerView.Refresh();
                        }
                        e.Handled = true;
                    }
                }
                if (e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject.GetType() == typeof(ReagentOperator))
                {
                    if (((ReagentOperator)e.InnerArgs.CurrentObject).Operator != null)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView view = nestedFrame.ViewItem.View;
                        DashboardViewItem dvFormula = ((DashboardView)view).FindItem("Formula") as DashboardViewItem;
                        if (dvFormula != null && dvFormula.InnerView != null)
                        {
                            ReagentOperator obj = (ReagentOperator)e.InnerArgs.CurrentObject;
                            NonPersistentReagent objCurrObj = (NonPersistentReagent)dvFormula.InnerView.CurrentObject;
                            if (objCurrObj != null)
                            {
                                objCurrObj.Formula = objCurrObj.Formula + obj.Operator;
                                dvFormula.InnerView.Refresh();
                            }
                        }
                        e.Handled = true;
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
            try
            {
                if (View.Id == "VariableAndUnits_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 295;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    //gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');
                        if(nav != null && sep != null) {
                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                           s.SetWidth((totusablescr / 100) * 28); 
                        }
                        else {
                            s.SetWidth(250); 
                        }                        
                    }";
                    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectByRowClick = true;
                    //gridListEditor.Grid.Load += Grid_Load;
                }
                else if (View.Id == "ReagentOperator_ListView_Formula")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 295;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectByRowClick = true;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');
                        if(nav != null && sep != null) {
                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                           s.SetWidth((totusablescr / 100) * 8); 
                        }
                        else {
                            s.SetWidth(120); 
                        }                        
                    }";
                }
                else if (View.Id == "NonPersistentReagent_DetailView")
                {
                    //string js = @"function(s,e) 
                    //{
                    //    var nav = document.getElementById('LPcell');
                    //    var sep = document.getElementById('separatorCell');                      
                    //    if(nav != null && sep != null) 
                    //    {
                    //        var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                    //        s.SetWidth(totusablescr /2);
                    //    }
                    //    else 
                    //    {
                    //        s.SetWidth(145); 
                    //    }                      
                    //}";
                    //foreach (ViewItem item in ((DetailView)View).Items)
                    //{
                    //    if (item.GetType() == typeof(ASPxStringPropertyEditor))
                    //    {
                    //        ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                    //        if (propertyEditor != null && propertyEditor.Editor != null)
                    //        {
                    //            ASPxTextBox textBox = (ASPxTextBox)propertyEditor.Editor;
                    //            if (textBox != null)
                    //            {
                    //                textBox.ClientSideEvents.Init = js;
                    //                textBox.ClientInstanceName = propertyEditor.Id;
                    //            }
                    //        }
                    //    }
                    //}
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Access and customize the target View control.
        }
        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (e.DataColumn.FieldName != "Variable" && e.DataColumn.FieldName != "Unit") return;
                e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'VariableUnits', 'Units|'+{0}, '', false)", e.VisibleIndex));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            try
            {
                if (View.Id == "ReagentOperator_ListView_Formula" || View.Id == "VariableAndUnits_ListView")
                {
                    ListViewProcessCurrentObjectController listProcessController = Frame.GetController<ListViewProcessCurrentObjectController>();
                    if (listProcessController != null)
                    {
                        listProcessController.CustomProcessSelectedItem -= ProcessListViewRowController_CustomProcessSelectedItem;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Formula_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                RegentPrepCalculationEditor objCalcEditor = (RegentPrepCalculationEditor)View.CurrentObject;
                if (objCalcEditor != null && objCalcEditor.C2Units != null && objCalcEditor.C1Units != null && objCalcEditor.V2Units != null && ((objCalcEditor.HideV1Unit == false && objCalcEditor.V1Units != null) || (objCalcEditor.HideW1Unit == false && objCalcEditor.W1Units != null)))
                {
                    DashboardView dashboard = Application.CreateDashboardView(Application.CreateObjectSpace(), "RegentPrepCalculationEditor", false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(dashboard);
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    showViewParameters.CreatedView.Caption = "Result Correction Formula";
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += Dc_Accepting;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectUnits"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    return;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null)
                {
                    RegentPrepCalculationEditor objCal = (RegentPrepCalculationEditor)View.CurrentObject;
                    if (objCal != null)
                    {
                        DialogController dc = (DialogController)sender as DialogController;
                        DashboardViewItem dvFormula = ((DashboardView)dc.Frame.View).FindItem("Formula") as DashboardViewItem;
                        if (dvFormula != null && dvFormula.InnerView != null)
                        {
                            NonPersistentReagent objReag = (NonPersistentReagent)dvFormula.InnerView.CurrentObject;
                            objCal.Formula = objReag.Formula;
                            View.Refresh();
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

        public void ProcessAction(string parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(parameter))
                {
                    string[] param = parameter.Split('|');
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && param[0] == "Units")
                    {
                        string strVariable = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Variable").ToString(); ;
                        string strUnit = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Unit").ToString();
                        if (!string.IsNullOrEmpty(strVariable) && !string.IsNullOrEmpty(strUnit))
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            CompositeView view = nestedFrame.ViewItem.View;
                            DashboardViewItem dvFormula = ((DashboardView)view).FindItem("Formula") as DashboardViewItem;
                            if (dvFormula != null && dvFormula.InnerView != null)
                            {
                                NonPersistentReagent objCurrObj = (NonPersistentReagent)dvFormula.InnerView.CurrentObject;
                                objCurrObj.Formula = objCurrObj.Formula + strVariable + "(" + strUnit + ")";
                                dvFormula.InnerView.Refresh();
                                dvFormula.Refresh();
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
    }
}
