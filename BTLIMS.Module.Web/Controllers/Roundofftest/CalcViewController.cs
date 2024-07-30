using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using E_RoundOff;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Linq;

namespace LDM.Module.Web.Controllers.Roundofftest
{
    public partial class CalcViewController : ViewController
    {
        Roundoff Roundoff = new Roundoff();
        MessageTimer timer = new MessageTimer();
        string validatejs = @"function(s, e){
                                var regex = /[0-9]|\.|\+|\-/;   
                                if (!regex.test(e.htmlEvent.key)) {
                                    e.htmlEvent.returnValue = false;
                                }}";
        public CalcViewController()
        {
            InitializeComponent();
            TargetViewId = "Roundofftest_DetailView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                Frame.GetController<ViewNavigationController>().NavigateBackAction.Active.SetItemValue("hideback", false);
                Frame.GetController<ViewNavigationController>().NavigateForwardAction.Active.SetItemValue("hideforward", false);
                ASPxStringPropertyEditor propertyEditorSigFigures = ((DetailView)View).FindItem("SigFigures") as ASPxStringPropertyEditor;
                if (propertyEditorSigFigures.Id == "SigFigures")
                {
                    ASPxTextBox textBox = (ASPxTextBox)propertyEditorSigFigures.Editor;
                    if (textBox != null)
                        textBox.ClientSideEvents.Init = @"function(s, e){
                                                            var x = s.GetInputElement();
                                                            x.focus();
                                                            }";
                    textBox.ClientSideEvents.KeyPress = validatejs;
                }
                ASPxStringPropertyEditor propertyEditorCutOff = ((DetailView)View).FindItem("CutOff") as ASPxStringPropertyEditor;
                if (propertyEditorCutOff.Id == "CutOff")
                {
                    ASPxTextBox textBox = (ASPxTextBox)propertyEditorCutOff.Editor;
                    if (textBox != null)
                    {
                        textBox.ClientSideEvents.KeyPress = validatejs;
                    }
                }
                ASPxStringPropertyEditor propertyEditorDecimals = ((DetailView)View).FindItem("Decimals") as ASPxStringPropertyEditor;
                if (propertyEditorDecimals.Id == "Decimals")
                {
                    ASPxTextBox textBox = (ASPxTextBox)propertyEditorDecimals.Editor;
                    if (textBox != null)
                    {
                        textBox.ClientSideEvents.KeyPress = validatejs;
                    }
                }
                //ASPxStringPropertyEditor propertyEditorDefaultResult = ((DetailView)View).FindItem("DefaultResult") as ASPxStringPropertyEditor;
                //if (propertyEditorDefaultResult.Id == "DefaultResult")
                //{
                //    ASPxTextBox textBox = (ASPxTextBox)propertyEditorDefaultResult.Editor;
                //    if (textBox != null)
                //    {
                //        textBox.ClientSideEvents.KeyPress = @"function(s, e){
                //                var regex = /[0-9]|\.|\<|\+|\-/;   
                //                if (!regex.test(e.htmlEvent.key)) {
                //                    e.htmlEvent.returnValue = false;
                //                }}";
                //    }
                //}
                ASPxStringPropertyEditor propertyEditorRptLimit = ((DetailView)View).FindItem("RptLimit") as ASPxStringPropertyEditor;
                if (propertyEditorRptLimit.Id == "RptLimit")
                {
                    ASPxTextBox textBox = (ASPxTextBox)propertyEditorRptLimit.Editor;
                    if (textBox != null)
                    {
                        textBox.ClientSideEvents.KeyPress = validatejs;
                    }
                }
                ASPxStringPropertyEditor propertyEditorNumericResult1 = ((DetailView)View).FindItem("NumericResult1") as ASPxStringPropertyEditor;
                if (propertyEditorNumericResult1.Id == "NumericResult1")
                {
                    ASPxTextBox textBox = (ASPxTextBox)propertyEditorNumericResult1.Editor;
                    if (textBox != null)
                    {
                        textBox.ClientSideEvents.KeyPress = validatejs;
                    }
                }
                ASPxStringPropertyEditor propertyEditorNumericResult2 = ((DetailView)View).FindItem("NumericResult2") as ASPxStringPropertyEditor;
                if (propertyEditorNumericResult2.Id == "NumericResult2")
                {
                    ASPxTextBox textBox = (ASPxTextBox)propertyEditorNumericResult2.Editor;
                    if (textBox != null)
                    {
                        textBox.ClientSideEvents.KeyPress = validatejs;
                    }
                }
                ASPxStringPropertyEditor propertyEditorNumericResult3 = ((DetailView)View).FindItem("NumericResult3") as ASPxStringPropertyEditor;
                if (propertyEditorNumericResult3.Id == "NumericResult3")
                {
                    ASPxTextBox textBox = (ASPxTextBox)propertyEditorNumericResult3.Editor;
                    if (textBox != null)
                    {
                        textBox.ClientSideEvents.KeyPress = validatejs;
                    }
                }
                ASPxStringPropertyEditor propertyEditorNumericResult4 = ((DetailView)View).FindItem("NumericResult4") as ASPxStringPropertyEditor;
                if (propertyEditorNumericResult4.Id == "NumericResult4")
                {
                    ASPxTextBox textBox = (ASPxTextBox)propertyEditorNumericResult4.Editor;
                    if (textBox != null)
                    {
                        textBox.ClientSideEvents.KeyPress = validatejs;
                    }
                }
                ASPxStringPropertyEditor propertyEditorSciNotationDecimals = ((DetailView)View).FindItem("SciNotationDecimals") as ASPxStringPropertyEditor;
                if (propertyEditorSciNotationDecimals.Id == "SciNotationDecimals")
                {
                    ASPxTextBox textBox = (ASPxTextBox)propertyEditorSciNotationDecimals.Editor;
                    if (textBox != null)
                    {
                        textBox.ClientSideEvents.KeyPress = @"function(s, e){
                                var regex = /[0-9]/;   
                                if (!regex.test(e.htmlEvent.key)) {
                                    e.htmlEvent.returnValue = false;
                                }}";
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

        private void Roundoffcalc_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Roundofftest_DetailView")
                {
                    Modules.BusinessObjects.Setting.Roundofftest objround = (Modules.BusinessObjects.Setting.Roundofftest)View.CurrentObject;
                    objround.DisplayResult1 = null;
                    objround.ScientificResult1 = null;
                    objround.DisplayResult2 = null;
                    objround.ScientificResult2 = null;
                    objround.DisplayResult3 = null;
                    objround.ScientificResult3 = null;
                    objround.DisplayResult4 = null;
                    objround.ScientificResult4 = null;
                    if (objround.SigFigures != null && objround.Decimals != null && objround.CutOff == null)
                    {
                        Application.ShowViewStrategy.ShowMessage("SigFig and Decimal cannot coexist without a CutOff values entered!", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        if (objround.NumericResult1 != null && objround.NumericResult1.Length > 0)
                        {
                            var Result = Resultcalc(objround.NumericResult1);
                            if (Result != null && Result.Contains("|"))
                            {
                                var Resultsplit = Result.Split('|');
                                objround.DisplayResult1 = Resultsplit[0];
                                objround.ScientificResult1 = Resultsplit[1];
                            }
                            else
                            {
                                objround.DisplayResult1 = Result;
                            }
                        }
                        if (objround.NumericResult2 != null && objround.NumericResult2.Length > 0)
                        {
                            var Result = Resultcalc(objround.NumericResult2);
                            if (Result != null && Result.Contains("|"))
                            {
                                var Resultsplit = Result.Split('|');
                                objround.DisplayResult2 = Resultsplit[0];
                                objround.ScientificResult2 = Resultsplit[1];
                            }
                            else
                            {
                                objround.DisplayResult2 = Result;
                            }
                        }
                        if (objround.NumericResult3 != null && objround.NumericResult3.Length > 0)
                        {
                            var Result = Resultcalc(objround.NumericResult3);
                            if (Result != null && Result.Contains("|"))
                            {
                                var Resultsplit = Result.Split('|');
                                objround.DisplayResult3 = Resultsplit[0];
                                objround.ScientificResult3 = Resultsplit[1];
                            }
                            else
                            {
                                objround.DisplayResult3 = Result;
                            }
                        }
                        if (objround.NumericResult4 != null && objround.NumericResult4.Length > 0)
                        {
                            var Result = Resultcalc(objround.NumericResult4);
                            if (Result != null && Result.Contains("|"))
                            {
                                var Resultsplit = Result.Split('|');
                                objround.DisplayResult4 = Resultsplit[0];
                                objround.ScientificResult4 = Resultsplit[1];
                            }
                            else
                            {
                                objround.DisplayResult4 = Result;
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

        private string Resultcalc(string resultnumeric)
        {
            try
            {
                string result;
                string resultoutput = string.Empty;
                string resultnotation = string.Empty;
                Modules.BusinessObjects.Setting.Roundofftest objround = (Modules.BusinessObjects.Setting.Roundofftest)View.CurrentObject;
                if (objround.RptLimit != null && Convert.ToDouble(resultnumeric) >= Convert.ToDouble(objround.RptLimit))
                {
                    if (objround.CutOff != null && objround.CutOff.Length > 0)
                    {
                        if (objround.SigFigures != null && objround.SigFigures.Length > 0 && Convert.ToDouble(resultnumeric) >= Convert.ToDouble(objround.CutOff))
                        {
                            var Cal = Roundoff.RoundoffInput(resultnumeric, objround.SigFigures).Split('|');
                            result = Cal[0];
                            if (objround.SciNotationDecimals != null && objround.SciNotationDecimals.Length > 0)
                            {
                                var notation = Cal[1].Split(' ');
                                notation[0] = Roundoff.FormatDecimalValue(notation[0], Convert.ToInt32(objround.SciNotationDecimals));
                                resultnotation = string.Join(" ", notation.ToArray());
                            }
                            else
                            {
                                resultnotation = Cal[1];
                            }
                        }
                        else if (objround.Decimals != null && objround.Decimals.Length > 0 && Convert.ToDouble(resultnumeric) < Convert.ToDouble(objround.CutOff))
                        {
                            result = Roundoff.FormatDecimalValue(resultnumeric, Convert.ToInt32(objround.Decimals));
                        }
                        else
                        {
                            result = resultnumeric;
                        }
                    }
                    else
                    {
                        if (objround.SigFigures != null && objround.SigFigures.Length > 0)
                        {
                            var Cal = Roundoff.RoundoffInput(resultnumeric, objround.SigFigures).Split('|');
                            result = Cal[0];
                            if (objround.SciNotationDecimals != null && objround.SciNotationDecimals.Length > 0)
                            {
                                var notation = Cal[1].Split(' ');
                                notation[0] = Roundoff.FormatDecimalValue(notation[0], Convert.ToInt32(objround.SciNotationDecimals));
                                resultnotation = string.Join(" ", notation.ToArray());
                            }
                            else
                            {
                                resultnotation = Cal[1];
                            }
                        }
                        else if (objround.Decimals != null && objround.Decimals.Length > 0)
                        {
                            result = Roundoff.FormatDecimalValue(resultnumeric, Convert.ToInt32(objround.Decimals));
                        }
                        else
                        {
                            result = resultnumeric;
                        }
                    }
                }
                else
                {
                    result = resultnumeric;
                }
                if (objround.RptLimit != null && Convert.ToDouble(result) < Convert.ToDouble(objround.RptLimit))
                {
                    resultoutput = objround.DefaultResult;
                }
                else if (objround.RptLimit != null && Convert.ToDouble(result) >= Convert.ToDouble(objround.RptLimit))
                {
                    resultoutput = result + "|" + resultnotation;
                }
                else
                {
                    resultoutput = result;
                }
                return resultoutput;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }

        private void Roundoffclear_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            View.CurrentObject = View.ObjectTypeInfo.CreateInstance();
        }
    }
}
