using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.Web;
using DevExpress.XtraReports.UI;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Report;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.DOC
{
    public partial class DOCViewController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        ICallbackManagerHolder sheet;
        TestMethodInfo objInfo = new TestMethodInfo();
        DOCInfo objDocinfo = new DOCInfo();

        public DOCViewController()
        {
            InitializeComponent();
            TargetViewId = "DOC_ListView;" + "DOC_DetailView_Copy;" + "DOC_DetailView_Comment;" + "SampleParameter_ListView_Copy_DOC;" + "DOC_DetailView_Copy_DV;" + "DOC_DetailView_Deleting_Reason;";
            //Save.TargetViewId = "DOC_ListView;"
            DOCRollBack.TargetViewId = Submit.TargetViewId = Validate.TargetViewId = Input.TargetViewId = "DOC_ListView;";
            DOCCertificate.TargetViewId  = "DOC_ListView;";
            DOCReport.TargetViewId  = "DOC_ListView;";
            Comment.TargetViewId = "DOC_DetailView_Copy";
            Calculate.TargetViewId = "SampleParameter_ListView_Copy_DOC;";
        }
        protected override void OnActivated()
        {
            try
            {
            base.OnActivated();
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
            if (View.Id == "DOC_ListView")
            {
                WebWindow.CurrentRequestWindow.PagePreRender += new EventHandler(CurrentRequestWindow_PagePreRender);
                Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing; ;
                //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executed += DeleteAction_Executed;
                Frame.GetController<DeleteObjectsViewController>().DeleteAction.TargetObjectsCriteria = "[Status] = 'PendingSubmission'";
                    Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                    if (user.Roles != null && user.Roles.Count > 0)
                    {
                        if (user.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objDocinfo.IsWrite = true;
                        }
                        else
                        {
                            foreach (RoleNavigationPermission role in user.RolePermissions)
                            {

                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationModelClass == View.ObjectTypeInfo.FullName && i.NavigationItem.IsDeleted == false && i.Write == true) != null)
                                {
                                    objDocinfo.IsWrite = true;
                                }
                            }
                        }
                    }
                    if(!objDocinfo.IsWrite)
                    {
                        DOCRollBack.Active["hideDOCRollBack"] = false;
                        Submit.Active["hideSubmit"] = false;
                        Validate.Active["hideValidate"] = false;
                    }
            }
                //if (View.Id == "SampleParameter_ListView_Copy_DOC")
                //{
                //    View.ControlsCreated += View_ControlsCreated;
                //}
                if (View.Id == "DOC_DetailView_Copy_DV")
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

        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            e.PopupControl.CustomizePopupControl += PopupControl_CustomizePopupControl;
        }

        private void PopupControl_CustomizePopupControl(object sender, CustomizePopupControlEventArgs e)
        {
            if (e.PopupFrame.View.Id== "SampleParameter_ListView_Copy_DOC")
            {
                e.PopupControl.ShowCloseButton = false; 
            }
        }
        private void DeleteAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View != null && View.SelectedObjects.Count == 1)
                {                   
                    IObjectSpace os = Application.CreateObjectSpace(typeof(Modules.BusinessObjects.Setting.DOC));
                    Modules.BusinessObjects.Setting.DOC obj = os.CreateObject<Modules.BusinessObjects.Setting.DOC>();
                    if (obj.Status == Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingSubmission)
                    {
                        DetailView createdView = Application.CreateDetailView(os, "DOC_DetailView_Deleting_Reason", true, obj);
                        createdView.Caption = "Deleting Reason:";
                        createdView.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                        showViewParameters.Context = TemplateContext.NestedFrame;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.Accepting += DeletingReason_Accepting;
                        //dc.AcceptAction.Execute += DeletingReason_AcceptAction_Execute1;
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        e.Cancel = true;
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void DeletingReason_AcceptAction_Execute1(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
            
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void DeleteAction_Executed(object sender, ActionBaseEventArgs e)
        //{

        //}
        private void DeletingReason_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null)
                {

                    Modules.BusinessObjects.Setting.DOC DOC1 = (Modules.BusinessObjects.Setting.DOC)e.AcceptActionArgs.CurrentObject;
                    if (DOC1 != null && string.IsNullOrEmpty(DOC1.DeletedReason))
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DOC", "MustDeleteReason"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        if (e.AcceptActionArgs.CurrentObject != null)
            {
                    Modules.BusinessObjects.Setting.DOC DOC = (Modules.BusinessObjects.Setting.DOC)View.CurrentObject;

                //Modules.BusinessObjects.Setting.DOC DOC1 = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.DOC>(CriteriaOperator.Parse("[Oid]=?", DOC.Oid));

                if (DOC != null /*&& DOC.Status == Modules.BusinessObjects.Setting.DOC.DOCstatus.Fail*/)
                {
                        IList<SampleParameter> lstSamples = null;
                        List<string> lstQC = DOC.QCBatches.Split(';').ToList();
                        if (DOC.IsQCBatchID)
                        {
                            lstSamples = ObjectSpace.GetObjects<SampleParameter>((new GroupOperator(GroupOperatorType.And, new InOperator("UQABID.AnalyticalBatchID", lstQC), CriteriaOperator.Parse("[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] == 'LCS'"))));
                        }
                        else
                        {
                            lstSamples = ObjectSpace.GetObjects<SampleParameter>((new GroupOperator(GroupOperatorType.And, new InOperator("UQABID.AnalyticalBatchID", lstQC), CriteriaOperator.Parse("[Samplelogin] Is Not Null And [Samplelogin.QCCategory] Is Not Null And ([Samplelogin.QCCategory.QCCategoryName] =='DOC' OR [Samplelogin.QCCategory.QCCategoryName] =='MDL' OR [Samplelogin.QCCategory.QCCategoryName] =='PT') AND [Status] = 'PendingReporting'"))));
                        }
                        foreach (SampleParameter param in lstSamples)
                    {
                        if (param != null && param.DOCDetail != null)
                        {
                            Modules.BusinessObjects.Setting.DOCDetails objDetails = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.DOCDetails>(CriteriaOperator.Parse("[Oid]= ?", param.DOCDetail.Oid));
                            param.DOCDetail = null;
                                if (objDetails!=null)
                                {
                            objDetails.Delete();
                        }
                        }
                        //DOC1.DeletedBy = ObjectSpace.GetObjectByKey<Employee>(param.AnalyzedBy);
                    }
                        DOC.DeletedDate = DateTime.Now;
                        DOC.DeletedReason = DOC1.DeletedReason;
                        DOC.Delete();
                        ObjectSpace.CommitChanges();
                        View.ObjectSpace.Refresh();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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
        private void NewObjectAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
            if (View.Id == "DOC_ListView")
            {
                e.Cancel = true;
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                Modules.BusinessObjects.Setting.DOC objDOC = objectSpace.CreateObject<Modules.BusinessObjects.Setting.DOC>();
                objDOC.IsQCBatchID = true;
                DetailView dv = Application.CreateDetailView(objectSpace, "DOC_DetailView_Copy_DV", true, objDOC);
                ShowViewParameters showViewParameters = new ShowViewParameters();
                showViewParameters.CreatedView = dv;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                //dc.SaveOnAccept = false;
                dc.CloseOnCurrentObjectProcessing = false;
                dc.Accepting += Dc_Accepting;
                //dc.ViewClosing += Dc_ViewClosing;
                //dc.CancelAction.Active.SetItemValue("enb", false);
                //dc.ViewClosed += Dc_ViewClosed;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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
                if (base.View != null && e.NewValue != e.OldValue && View.Id == "DOC_DetailView_Copy_DV")
                {
                    if (View != null && View.CurrentObject == e.Object)
                    {
                        Modules.BusinessObjects.Setting.DOC objDoc = (Modules.BusinessObjects.Setting.DOC)e.Object;
                        string strCriteria = string.Empty;
                        if (e.PropertyName == "Matrix")
                        {
                            if (e.NewValue != null && objDoc.Matrix != null)
                            {
                                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Matrix")
                                {
                                    if (objDoc != null)
                                    {
                                        objDoc.Method = null;
                                        objDoc.Test = null;

                                    }
                                }
                                else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Test")
                                {
                                    if (objDoc != null)
                                    {
                                        objDoc.Method = null;
                                        objDoc.QCBatches = null;
                                    }
                                }
                            }
                            objDoc.QCBatches = null;
                        }
                        if (e.PropertyName == "Test")
                        {
                            if (e.NewValue != null && objDoc.Test != null)
                            {
                                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Test")
                                {
                                    if (objDoc != null)
                                    {
                                        objDoc.Method = null;
                                        objDoc.QCBatches = null;
                                    }
                                }
                                else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Method")
                                {
                                    if (objDoc != null)
                                    {
                                        objDoc.QCBatches = null;
                                    }
                                }
                            }
                        }
                        else if (e.PropertyName == "Method")
                        {
                            //if (objDoc.Matrix != null && objDoc.Test != null && objDoc.Method != null)
                            //{
                            //    //List<SampleParameter> lstSamples1 = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[UQABID] Is Not Null AND [Testparameter.TestMethod] Is Not Null And [Testparameter.TestMethod.MatrixName] Is Not Null And [Testparameter.TestMethod.MethodName] Is Not Null AND [Testparameter.TestMethod.MatrixName.Oid] = ? AND [Testparameter.TestMethod.Oid] = ? AND [Testparameter.TestMethod.MethodName.Oid] = ?", objDoc.Matrix.Oid, objDoc.Test.Oid, objDoc.Method.Oid)).GroupBy(sample => sample.UQABID.AnalyticalBatchID).Select(group => group.First()).Distinct().ToList();

                            //}
                            //if (objDoc.Matrix != null && objDoc.Test != null && objDoc.Method != null)
                            //{
                            //    ASPxCheckedLookupStringPropertyEditor lvspDOC = ((DetailView)View).FindItem("QCBatches") as ASPxCheckedLookupStringPropertyEditor;
                            //    //ListPropertyEditor lvspDOC = ((DetailView)View).FindItem("QCBatches") as ListPropertyEditor;
                            //    if (lvspDOC != null)
                            //    {
                            //        ListView lstspDOC = lvspDOC.ListView;
                            //        List<SampleParameter> lstSamples1 = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[UQABID] Is Not Null AND [Testparameter.TestMethod] Is Not Null And [Testparameter.TestMethod.MatrixName] Is Not Null And [Testparameter.TestMethod.MethodName] Is Not Null AND [Testparameter.TestMethod.MatrixName.Oid] = ? AND [Testparameter.TestMethod.Oid] = ? AND [Testparameter.TestMethod.MethodName.Oid] = ?", objDoc.Matrix.Oid, objDoc.Test.Oid, objDoc.Method.Oid)).GroupBy(sample => sample.UQABID.AnalyticalBatchID).Select(group => group.First()).Distinct().ToList();

                            //        if (objDoc.Matrix != null && objDoc.Test != null)
                            //        {
                            //            foreach (SampleParameter sampleParam in lstSamples1)
                            //            {                                            
                            //                lstspDOC.CollectionSource.Add(sampleParam);
                            //            }
                            //        }
                            //        lstspDOC.Refresh();
                            //    }
                            //}
                            //else
                            //{
                            //    ListPropertyEditor lvspDOC = ((DetailView)View).FindItem("QCBatches") as ListPropertyEditor;
                            //    if (lvspDOC != null)
                            //    {
                            //        ListView lstspDOC = lvspDOC.ListView;
                            //        foreach (SampleParameter sampleParam in lstspDOC.CollectionSource.List.Cast<SampleParameter>().ToList())
                            //        {
                            //            lstspDOC.CollectionSource.Remove(sampleParam);
                            //        }
                            //        lstspDOC.Refresh();
                            //    }
                            //}
                        }
                        else if (View.Id == "DOC_DetailView_Copy_DV" && e.PropertyName == "TrainingEndDate" && e.NewValue != null)
                        {
                            if (objDoc.Matrix != null && objDoc.Test != null && objDoc.Method != null && objDoc.TrainingStartDate != null && objDoc.TrainingEndDate != null)
                            {
                                if (objDoc.TrainingStartDate != DateTime.MinValue && objDoc.TrainingEndDate < DateTime.Today)
                                {
                                    ListPropertyEditor lvspDOC = ((DetailView)View).FindItem("SampleParameter") as ListPropertyEditor;
                                    if (lvspDOC != null)
                                    {
                                        ListView lstspDOC = lvspDOC.ListView;
                                        List<SampleParameter> lstSamples1 = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[AnalyzedDate] Between('" + objDoc.TrainingStartDate + "','" + objDoc.TrainingEndDate + "') AND [UQABID] Is Not Null AND [Testparameter.TestMethod] Is Not Null And [Testparameter.TestMethod.MatrixName] Is Not Null And [Testparameter.TestMethod.MethodName] Is Not Null AND [Testparameter.TestMethod.MatrixName.Oid] = ? AND [Testparameter.TestMethod.Oid] = ? AND [Testparameter.TestMethod.MethodName.Oid] = ?", objDoc.Matrix.Oid, objDoc.Test.Oid, objDoc.Method.Oid)).GroupBy(sample => sample.UQABID.AnalyticalBatchID).Select(group => group.First()).Distinct().ToList();

                                        if (objDoc.Matrix != null && objDoc.Test != null)
                                        {
                                            foreach (SampleParameter sampleParam in lstSamples1)
                                            {
                                                lstspDOC.CollectionSource.Add(sampleParam);
                                            }
                                        }
                                        lstspDOC.Refresh();
                                    }
                                }

                            }
                            else
                            {
                                ListPropertyEditor lvspDOC = ((DetailView)View).FindItem("SampleParameter") as ListPropertyEditor;
                                if (lvspDOC != null)
                                {
                                    ListView lstspDOC = lvspDOC.ListView;
                                    foreach (SampleParameter sampleParam in lstspDOC.CollectionSource.List.Cast<SampleParameter>().ToList())
                                    {
                                        lstspDOC.CollectionSource.Remove(sampleParam);
                                    }
                                    lstspDOC.Refresh();
                                }
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

        private void View_ControlsCreated(object sender, EventArgs e)
        {
            //if (View.Id == "SampleParameter_ListView_Copy_DOC")
            //{
            //    IObjectSpace os = Application.CreateObjectSpace();
            //    //Modules.BusinessObjects.Setting.DOC DOC = (Modules.BusinessObjects.Setting.DOC)Application.MainWindow.View.CurrentObject;
            //    Modules.BusinessObjects.Setting.DOC DOC = null;
            //    if (HttpContext.Current.Session["rowid"]!=null)
            //    {
            //        DOC = View.ObjectSpace.FindObject<Modules.BusinessObjects.Setting.DOC>(CriteriaOperator.Parse("[Oid]=?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
            //    }
            //    else
            //    {
            //        DOC = (Modules.BusinessObjects.Setting.DOC)Application.MainWindow.View.CurrentObject;
            //    }
            //    //Modules.BusinessObjects.Setting.DOC DOC = View.ObjectSpace.FindObject<Modules.BusinessObjects.Setting.DOC>(CriteriaOperator.Parse("[Oid]=?",new Guid()));
            //    if (DOC != null)
            //    {
            //        IList<SampleParameter> lstSamples = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And ([QCBatchID.QCType.QCTypeName] =='DOC' OR [QCBatchID.QCType.QCTypeName] =='MDL' OR [QCBatchID.QCType.QCTypeName] =='PT') AND [Status] = 'PendingReporting' AND Contains([UQABID.Jobid], ?)", DOC.JobID.JobID)).GroupBy(sample => sample.Testparameter.Parameter).Select(group => group.First()).ToList();
            //        IList<SampleParameter> lstSamples2 = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin] Is Not Null And [Samplelogin.QCCategory] Is Not Null And ([Samplelogin.QCCategory.QCCategoryName] =='DOC' OR [Samplelogin.QCCategory.QCCategoryName] =='MDL' OR [Samplelogin.QCCategory.QCCategoryName] =='PT') AND [Status] = 'PendingReporting' AND Contains([UQABID.Jobid], ?)", DOC.JobID.JobID)).GroupBy(sample => sample.Testparameter.Parameter).Select(group => group.First()).ToList();

            //        foreach (var qcparam in lstSamples2)
            //        {
            //            if (qcparam.DOCDetail == null)
            //            {
            //                List<SampleParameter> lstSamples1 = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? AND [Testparameter.Parameter.Oid] = ? AND [Samplelogin.JobID.GCRecord] is NULL", DOC.JobID.Oid, qcparam.Testparameter.Parameter.Oid)).ToList();
            //                int columnIndex = 1;
            //                DOCDetails objPT = os.CreateObject<DOCDetails>();
            //                qcparam.DOCDetail = objPT;
            //                foreach (var param in lstSamples1)
            //                {
            //                    if (qcparam.Testparameter.Parameter.ToString() == param.Testparameter.Parameter.ToString())
            //                    {
            //                        var property = objPT.GetType().GetProperty($"Result{columnIndex}");
            //                        var property1 = objPT.GetType().GetProperty($"Rec{columnIndex}");
            //                        if (property != null)
            //                        {
            //                            property.SetValue(objPT, param.Result);
            //                            property1.SetValue(objPT, param.Rec);
            //                            columnIndex++;
            //                            if (columnIndex > 8)
            //                            {
            //                                break;
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        os.CommitChanges();
            //        foreach (var selectedObject in lstSamples2)
            //        {

            //            if (selectedObject is SampleParameter spm)
            //            {
            //                DOCDetails docDetails = spm.DOCDetail;
            //                int countResult = 0;
            //                double sumResult = 0;
            //                int countRec = 0;
            //                double sumRec = 0;
            //                for (int i = 1; i <= 8; i++)
            //                {
            //                    string resultPropertyName = $"Result{i}";
            //                    string recPropertyName = $"Rec{i}";
            //                    var resultValue = docDetails.GetType().GetProperty(resultPropertyName)?.GetValue(docDetails, null);
            //                    var recValue = docDetails.GetType().GetProperty(recPropertyName)?.GetValue(docDetails, null);

            //                    if (resultValue != null)
            //                    {
            //                        countResult++;
            //                        sumResult += Convert.ToDouble(resultValue);
            //                    }
            //                    if (recValue != null)
            //                    {
            //                        countRec++;
            //                        sumRec += Convert.ToDouble(recValue);

            //                    }
            //                    else if (recValue == null && resultValue != null && spm.SpikeAmount != null)
            //                    {
            //                        recValue = (Convert.ToDouble(resultValue) / Convert.ToDouble(spm.SpikeAmount) * 100);
            //                        countRec++;
            //                        sumRec += Convert.ToDouble(recValue);
            //                    }
            //                }
            //                if (countResult > 0)
            //                {
            //                    double averageResult = sumResult / countResult;
            //                    spm.Average = averageResult;
            //                    double sumSquaredDeviations = 0;
            //                    for (int i = 1; i <= 8; i++)
            //                    {
            //                        string resultPropertyName = $"Result{i}";
            //                        var resultValue = docDetails.GetType().GetProperty(resultPropertyName)?.GetValue(docDetails, null);

            //                        if (resultValue != null)
            //                        {
            //                            double deviation = Convert.ToDouble(resultValue) - averageResult;
            //                            sumSquaredDeviations += Math.Pow(deviation, 2);
            //                        }
            //                    }

            //                    double variance = sumSquaredDeviations / (countResult - 1);
            //                    double standardDeviation = Math.Sqrt(variance);
            //                    double roundedSD = Math.Round(standardDeviation, 4);
            //                    if (roundedSD>0)
            //                    {
            //                    spm.SD = roundedSD;
            //                    }
            //                    if (sumSquaredDeviations > 0)
            //                    {
            //                        spm.RSD = (decimal)(roundedSD / spm.Average * 100);
            //                    }
            //                    else
            //                    {
            //                        spm.RSD = 0;
            //                    }
            //                }

            //                if (countRec > 0)
            //                {
            //                    decimal averageRec = (decimal)(sumRec / countRec);
            //                    spm.AvgRec = averageRec;
            //                }
            //                ObjectSpace.CommitChanges();
            //                //Application.ShowViewStrategy.ShowMessage("Calculated successfully.", InformationType.Success, 3000, InformationPosition.Top);
            //                os.CommitChanges();
            //            }
            //        }
            //        //View.ObjectSpace.Refresh();
            //        //objectSpace.Refresh();
            //        //os.CommitChanges();
                    //View.ObjectSpace.Refresh();
            //        Application.MainWindow.View.Refresh();
            //        os.Refresh();
            //    }

            //    //Modules.BusinessObjects.Setting.DOC DOC1 = (Modules.BusinessObjects.Setting.DOC)View.CurrentObject;
            //    //Modules.BusinessObjects.Setting.DOC DOC2 = (Modules.BusinessObjects.Setting.DOC);

            //}
            //else if (View.Id == "SampleParameter_ListView_Copy_DOC_Copy")
            //{
            //    IObjectSpace os = Application.CreateObjectSpace();

            //}
        }

        private void CurrentRequestWindow_PagePreRender(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "DOC_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                        foreach (GridViewColumn column in gridListEditor.Grid.VisibleColumns)
                        {
                            if (column.Name == "SelectionCommandColumn")
                            {
                                gridListEditor.Grid.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (column is GridViewDataActionColumn && ((GridViewDataActionColumn)column).Action.Id == "Input")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 13].VisibleIndex;
                                column.Width = 45;
                                column.Caption = "Input";
                            }
                            if (column is GridViewDataActionColumn && ((GridViewDataActionColumn)column).Action.Id == "DOCCertificate")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 2].VisibleIndex;
                                column.Width = 100;
                                column.Caption = "Certificate";
                            }
                            if (column is GridViewDataActionColumn && ((GridViewDataActionColumn)column).Action.Id == "DOCReport")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 3].VisibleIndex;
                                column.Width = 100;
                                column.Caption = "DOCReport";
                            }
                            if (column.Caption == "DOCID")
                            {
                                gridListEditor.Grid.VisibleColumns["DOCID"].FixedStyle = GridViewColumnFixedStyle.Left;
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 14].VisibleIndex;
                            }
                            if (column.Caption == "Test")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 12].VisibleIndex;
                            }
                            if (column.Caption == "Method")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 11].VisibleIndex;
                            }
                            if (column.Caption == "Matrix")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 10].VisibleIndex;
                            }
                            if (column.Caption == "strJobID")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 9].VisibleIndex;
                            }
                            if (column.Caption == "Analyst")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 8].VisibleIndex;
                            }
                            if (column.Caption == "DateAnalyzed")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 7].VisibleIndex;
                            }
                            if (column.Caption == "DateSubmitted")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 6].VisibleIndex;
                            }
                            if (column.Caption == "DateValidated")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 5].VisibleIndex;
                            }
                            if (column.Caption == "ValidatedBy")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 4].VisibleIndex;
                                //column.Width = 100;
                            }
                            if (column.Caption == "Status")
                            {
                                column.VisibleIndex = gridListEditor.Grid.VisibleColumns[gridListEditor.Grid.VisibleColumns.Count - 1].VisibleIndex;
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
        protected override void OnViewControlsCreated()
        {
            try
            {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
                sheet = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                sheet.CallbackManager.RegisterHandler("DOCIDPopup", this);
            if (View.Id == "DOC_ListView")
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (gridListEditor != null)
                {
                    ASPxGridView grid = gridListEditor.Grid;
                    if (grid != null)
                    {
                        grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    }
                }
                    if(objDocinfo.ViewClose)
                    {
                        View.ObjectSpace.Refresh();
                        Application.MainWindow.View.ObjectSpace.Refresh();
                        View.Refresh();
                        Frame.GetController<RefreshController>().RefreshAction.DoExecute();
                        objDocinfo.ViewClose = false;
                    }
            }
                else if(View.Id== "SampleParameter_ListView_Copy_DOC")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        ASPxGridView grid = gridListEditor.Grid;
                        if (grid != null)
                        {
                            grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            if (HttpContext.Current.Session["rowid"] != null)
                            {
                                Modules.BusinessObjects.Setting.DOC Objstudylog = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.DOC>(CriteriaOperator.Parse("[Oid]= ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                if (Objstudylog != null)
                                {
                                    if (Objstudylog.Status != Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingSubmission)
                                    {
                                        ((ListView)View).AllowEdit["Validation"] = false;
                                    }
                        }
                    }
                }
                    }
                    
                }
            if (View.Id == "DOC_DetailView_Copy_DV"||View.Id== "SampleParameter_ListView_Copy_DOC")
            {
                    //if (Frame is DevExpress.ExpressApp.Web.PopupWindow)
                    //{
                    //    if (Frame is DevExpress.ExpressApp.Web.PopupWindow popupWindow)
                    //    {
                    //        WebWindow.CurrentRequestPage.Load += CurrentRequestPage_Load;
                    //    }
                    //    //(Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(); 
                    //}
                if (View is DetailView)
                {
                    //Modules.BusinessObjects.Setting.DOC objSample = (Modules.BusinessObjects.Setting.DOC)View.CurrentObject;
                       
                    //foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "QCBatches" && i.GetType() == typeof(AspxGridLookupCustomEditor)))
                    //{
                    //    AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                    //    if (propertyEditor != null && propertyEditor.Editor != null)
                    //    {
                    //        ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                    //        gridLookup.GridView.CustomCallback += GridView_CustomCallback;
                    //        gridLookup.GridView.Load += GridView_Load;
                    //        Modules.BusinessObjects.Setting.DOC SC = (Modules.BusinessObjects.Setting.DOC)View.CurrentObject;
                    //        if (SC != null)
                    //        {
                    //            if (gridLookup != null)
                    //            {
                    //                gridLookup.GridViewProperties.Settings.ShowFilterRow = true;
                    //                if (!View.ObjectSpace.IsNewObject(SC))
                    //                {
                    //                    List<Modules.BusinessObjects.SampleManagement.SampleLogIn> lstSamples = View.ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("JobID.Oid=?", SC.Oid)).ToList();
                    //                    //IList<SampleParameter> parameters = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Samplelogin.JobID.Oid=?", SC.Oid));
                    //                    if (lstSamples.Count > 0) //&& string.IsNullOrEmpty(SC.NPTest)
                    //                    {
                    //                        SRInfo.strNPTest = "Disable";
                    //                    }
                    //                    else
                    //                    {
                    //                        SRInfo.strNPTest = string.Empty;
                    //                    }
                    //                }
                    //                gridLookup.JSProperties["cpTest"] = SC.NPTest;
                    //                gridLookup.ClientInstanceName = propertyEditor.Id;
                    //                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                    //                        {
                    //                        s.SetText(s.cpTest);
                    //                        }";
                    //                gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                    //                gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                    //                gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                    //                gridLookup.ValueChanged += GridLookup_ValueChanged;
                    //                gridLookup.GridView.CommandButtonInitialize += GridView_CommandButtonInitialize;
                    //                gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                    //                gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                    //                gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Test" });
                    //                gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Method" });
                    //                gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "IsGroup" });
                    //                gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "TestName" });
                    //                gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "TextView" });
                    //                gridLookup.GridView.Columns["Test"].Width = 120;
                    //                gridLookup.GridView.Columns["Method"].Width = 200;
                    //                gridLookup.GridView.Columns["IsGroup"].Width = 100;
                    //                gridLookup.GridView.VisibleColumns["TestName"].Visible = false;
                    //                gridLookup.GridView.VisibleColumns["TextView"].Visible = false;
                    //                gridLookup.GridView.KeyFieldName = "TextView";
                    //                if (SRInfo.isNoOfSampleDisable)
                    //                {
                    //                    gridLookup.GridView.ClientSideEvents.RowClick = "function(s,e){e.cancel = true;}";
                    //                }
                    //                else
                    //                {
                    //                    gridLookup.GridView.ClientSideEvents.RowClick = "function(s,e){e.cancel = false;}";
                    //                }
                    //                gridLookup.TextFormatString = "{4}";
                    //                DataTable table = new DataTable();
                    //                table.Columns.Add("Test");
                    //                table.Columns.Add("Method");
                    //                table.Columns.Add("IsGroup", typeof(bool));
                    //                table.Columns.Add("TestName");
                    //                table.Columns.Add("TextView");
                    //                gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    //                gridLookup.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    //                gridLookup.GridView.Settings.VerticalScrollableHeight = 200;
                    //                if (SC.SampleMatries != null && !string.IsNullOrEmpty(SC.SampleMatries))
                    //                {
                    //                    List<string> lstMatrix = SC.SampleMatries.Split(';').ToList().Select(i => i.Trim()).ToList();
                    //                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    //                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    //                    XPClassInfo testMatrixInfo;
                    //                    testMatrixInfo = uow.GetClassInfo(typeof(VisualMatrix));
                    //                    List<VisualMatrix> lstVisuaMatrix = uow.GetObjects(testMatrixInfo, new InOperator("Oid", lstMatrix.Select(i => new Guid(i.Trim()))), null, int.MaxValue, false, true).Cast<VisualMatrix>().ToList();
                    //                    //IList<VisualMatrix> lstVisuaMatrix = ObjectSpace.GetObjects<VisualMatrix>(new InOperator("Oid", lstMatrix.Select(i => new Guid(i.Trim()))));
                    //                    XPClassInfo testParameterInfo;
                    //                    testParameterInfo = uow.GetClassInfo(typeof(Testparameter));
                    //                    IList<Testparameter> lstTests = uow.GetObjects(testParameterInfo, new GroupOperator(GroupOperatorType.And, new InOperator("TestMethod.MatrixName.Oid", lstVisuaMatrix.Select(i => i.MatrixName)), CriteriaOperator.Parse("TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null")), new SortingCollection(), 0, 0, false, true).Cast<Testparameter>().ToList();
                    //                    //IList<Testparameter> lstTests = ObjectSpace.GetObjects<Testparameter>(new GroupOperator(GroupOperatorType.And, new InOperator("TestMethod.MatrixName.Oid", lstVisuaMatrix.Select(i => i.MatrixName)), CriteriaOperator.Parse("TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null")));
                    //                    if (lstTests != null && lstTests.Count > 0)
                    //                    {
                    //                        int i = 0;
                    //                        string[] strtestarr = SC.NPTest.Split(';');
                    //                        foreach (Testparameter objTest in lstTests.Where(a => a.TestMethod != null && a.IsGroup == false && a.TestMethod.MatrixName != null && a.TestMethod.MethodName != null).GroupBy(p => new { p.TestMethod.TestName, p.TestMethod.MethodName.MethodNumber, p.TestMethod.IsGroup }).Select(grp => grp.FirstOrDefault()).OrderBy(a => a.TestMethod.TestName).ThenBy(a => a.TestMethod.MethodName.MethodNumber).ToList())
                    //                        {
                    //                            table.Rows.Add(new object[] { objTest.TestMethod.TestName, objTest.TestMethod.MethodName.MethodNumber, objTest.TestMethod.IsGroup, objTest.TestMethod.TestName + "|" + objTest.TestMethod.MethodName.MethodNumber + "|" + objTest.TestMethod.IsGroup, objTest.TestMethod.TestName + "|" + objTest.TestMethod.MethodName.MethodNumber });
                    //                            string strtestview = objTest.TestMethod.TestName.ToString() + "_" + objTest.TestMethod.MethodName.MethodNumber.ToString();
                    //                            if (strtestarr.Contains(strtestview))
                    //                            {
                    //                                SRInfo.lstTestgridviewrow.Add(i.ToString());
                    //                            }
                    //                            i++;
                    //                        }
                    //                        foreach (Testparameter objTest in lstTests.Where(a => a.TestMethod != null && a.IsGroup == true && a.TestMethod.MatrixName != null && a.TestMethod.TestName != null).GroupBy(p => new { p.TestMethod.TestName, p.TestMethod.IsGroup }).Select(grp => grp.FirstOrDefault()).OrderBy(a => a.TestMethod.TestName).ThenBy(a => a.TestMethod.IsGroup).ToList())
                    //                        {
                    //                            table.Rows.Add(new object[] { objTest.TestMethod.TestName, " ", objTest.TestMethod.IsGroup, objTest.TestMethod.TestName + "|" + " " + "|" + objTest.TestMethod.IsGroup, objTest.TestMethod.TestName });
                    //                        }
                    //                        DataView dv = new DataView(table);
                    //                        dv.Sort = "Test Asc";
                    //                        table = dv.ToTable();
                    //                    }
                    //                }
                    //                gridLookup.GridView.DataSource = table;
                    //                SRInfo.dtTest = table;
                    //                gridLookup.GridView.DataBind();
                    //                ;
                    //            }
                    //        }
                    //    }
                    //    if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                    //    {
                    //        propertyEditor.Editor.BackColor = Color.LightYellow;
                    //    }
                    //}
                }
            }
        }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Comment_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.Setting.DOC objDOC = (Modules.BusinessObjects.Setting.DOC)e.CurrentObject;
                if (objDOC != null)
                {
                    DetailView dvInput = Application.CreateDetailView(View.ObjectSpace, "DOC_DetailView_Comment", false, objDOC);
                    dvInput.Caption = "DOC Comment";
                    if (objDOC.Status == Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingSubmission && objDocinfo.IsWrite)
                    {
                    dvInput.ViewEditMode = ViewEditMode.Edit;
                    }
                    else
                    {
                        dvInput.ViewEditMode = ViewEditMode.View;
                    }
                    ShowViewParameters showViewParameters = new ShowViewParameters(dvInput);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.CreatedView = dvInput;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = true;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.AcceptAction.Execute += AcceptAction_Execute;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.Setting.DOC doc = (Modules.BusinessObjects.Setting.DOC)e.CurrentObject;
                Modules.BusinessObjects.Setting.DOC obj = (Modules.BusinessObjects.Setting.DOC)View.CurrentObject;
                if (doc != null && !string.IsNullOrEmpty(doc.Comment))
                {
                    obj.Comment = doc.Comment;
                    obj.Commentbool = true;
                    View.Refresh();
                }
                else
                {
                    obj.Comment = doc.Comment;
                    obj.Commentbool = false;
                    View.Refresh();
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Calculate_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
            if (e.SelectedObjects.Count > 0)
            {
                IObjectSpace objectSpace = View.ObjectSpace;
                foreach (var selectedObject in View.SelectedObjects)
                {

                    if (selectedObject is SampleParameter spm)
                    {
                        DOCDetails docDetails = spm.DOCDetail;
                        int countResult = 0;
                        double sumResult = 0;
                        int countRec = 0;
                        double sumRec = 0;
                        for (int i = 1; i <= 8; i++)
                        {
                            string resultPropertyName = $"Result{i}";
                            string recPropertyName = $"Rec{i}";
                            var resultValue = docDetails.GetType().GetProperty(resultPropertyName)?.GetValue(docDetails, null);
                            var recValue = docDetails.GetType().GetProperty(recPropertyName)?.GetValue(docDetails, null);

                            if (resultValue != null && double.TryParse(resultValue.ToString(), out _))
                            {
                                countResult++;
                                sumResult += Convert.ToDouble(resultValue);
                            }
                            if (recValue != null)
                            {
                                countRec++;
                                sumRec += Convert.ToDouble(recValue);
                            }
                            else if (recValue == null && resultValue != null && double.TryParse(resultValue.ToString(), out _) && spm.SpikeAmount != null)
                            {
                                recValue = (Convert.ToDouble(resultValue) / Convert.ToDouble(spm.SpikeAmount) * 100);
                                countRec++;
                                sumRec += Convert.ToDouble(recValue);
                            }
                        }
                        if (countResult > 0)
                        {
                            double averageResult = sumResult / countResult;
                            spm.Average = averageResult;
                            double sumSquaredDeviations = 0;
                            for (int i = 1; i <= 8; i++)
                            {
                                string resultPropertyName = $"Result{i}";
                                var resultValue = docDetails.GetType().GetProperty(resultPropertyName)?.GetValue(docDetails, null);

                                if (resultValue != null && double.TryParse(resultValue.ToString(), out _))
                                {
                                    double deviation = Convert.ToDouble(resultValue) - averageResult;
                                    sumSquaredDeviations += Math.Pow(deviation, 2);
                                }
                            }

                            double variance = sumSquaredDeviations / (countResult - 1);
                            double standardDeviation = Math.Sqrt(variance);
                            double roundedSD = Math.Round(standardDeviation, 4);
                            if (roundedSD>0)
                            {
                            spm.SD = roundedSD;
                            }
                            if (sumSquaredDeviations > 0)
                            {
                                spm.RSD = (decimal)(roundedSD / spm.Average * 100);
                            }
                            else
                            {
                                spm.RSD = 0;
                            }
                        }

                        if (countRec > 0)
                        {
                            decimal averageRec = (decimal)(sumRec / countRec);
                            spm.AvgRec = averageRec;
                        }
                        ObjectSpace.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage("Calculated successfully.", InformationType.Success, 3000, InformationPosition.Top);

                    }
                }
                View.ObjectSpace.Refresh();
                objectSpace.Refresh();
            }
            else
            {
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                return;
            }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        private void Validate_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "DOC_ListView")
                {
                    if (e.SelectedObjects.Count > 0)
                    {
                        Modules.BusinessObjects.Setting.DOC objDOC = View.CurrentObject as Modules.BusinessObjects.Setting.DOC;
                        if (objDOC.DOCID != null)
                        {
                            if (objDOC.Status == Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingValidation)
                            {
                                foreach (Modules.BusinessObjects.Setting.DOC objSelectedDOC in e.SelectedObjects)
                                {
                                    Modules.BusinessObjects.Setting.DOC Objdoclog = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.DOC>(CriteriaOperator.Parse("Oid =?", objSelectedDOC.Oid));
                                    Objdoclog.Status = Modules.BusinessObjects.Setting.DOC.DOCstatus.Pass;
                                    if (Objdoclog.ValidatedBy == null)
                                    {
                                        Objdoclog.DateValidated = DateTime.Now;
                                        Objdoclog.ValidatedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    }
                                    ObjectSpace.CommitChanges();
                                }
                                View.ObjectSpace.Refresh();
                                Application.ShowViewStrategy.ShowMessage("Validated successfully.", InformationType.Success, 3000, InformationPosition.Top);
                                return;
                            }
                        }
                        //Application.ShowViewStrategy.ShowMessage("Please submit the DOCID before Validating.", InformationType.Error, 3000, InformationPosition.Top);
                        //return;

                    }
                    //Application.ShowViewStrategy.ShowMessage("Select the check box.", InformationType.Error, 3000, InformationPosition.Top);
                    //return;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Submit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "DOC_ListView")
                {
                    if (e.SelectedObjects.Count > 0)
                    {
                        foreach (Modules.BusinessObjects.Setting.DOC objSelectedDOC in e.SelectedObjects)
                        {
                            IList<SampleParameter> lstSamples1 = null;
                            List<string> lstQC = objSelectedDOC.QCBatches.Split(';').ToList();
                            if (objSelectedDOC.IsQCBatchID)
                            {
                                lstSamples1 = ObjectSpace.GetObjects<SampleParameter>((new GroupOperator(GroupOperatorType.And, new InOperator("UQABID.AnalyticalBatchID", lstQC), CriteriaOperator.Parse("[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] == 'LCS'"))));
                            }
                            else
                            {
                                lstSamples1 = ObjectSpace.GetObjects<SampleParameter>((new GroupOperator(GroupOperatorType.And, new InOperator("UQABID.AnalyticalBatchID", lstQC), CriteriaOperator.Parse("[Samplelogin] Is Not Null And [Samplelogin.QCCategory] Is Not Null And ([Samplelogin.QCCategory.QCCategoryName] =='DOC' OR [Samplelogin.QCCategory.QCCategoryName] =='MDL' OR [Samplelogin.QCCategory.QCCategoryName] =='PT') AND [Status] = 'PendingReporting'"))));
                            }
                            if(lstSamples1.Count>8)
                            {
                                lstSamples1 = lstSamples1.Where(i => i.DOCDetail != null).ToList();
                            }
                            if (lstSamples1.Where(i => i.DOCDetail == null).FirstOrDefault() == null)
                            {

                                if (objSelectedDOC.DOCID != null)
                                {
                                    if (objSelectedDOC.Status == Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingSubmission)
                                    {
                                        Modules.BusinessObjects.Setting.DOC Objdoclog = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.DOC>(CriteriaOperator.Parse("Oid =?", objSelectedDOC.Oid));
                                        Objdoclog.Status = Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingValidation;
                                        Objdoclog.DateSubmitted = DateTime.Now;
                                        ObjectSpace.CommitChanges();
                                    }
                                    View.ObjectSpace.Refresh();
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DOC", "CalculationMust"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            //Application.ShowViewStrategy.ShowMessage("Please Create the DOCID before submitting.", InformationType.Error, 3000, InformationPosition.Top);
                            //return;
                        }
                    }
                    //Application.ShowViewStrategy.ShowMessage("Select the check box.", InformationType.Error, 3000, InformationPosition.Top);
                    //return;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Save_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "DOC_ListView")
                {
                    if (e.SelectedObjects.Count > 0)
                    {
                        foreach (Modules.BusinessObjects.Setting.DOC objSelectedDOC in e.SelectedObjects)
                        {
                            if (objSelectedDOC.DOCID == null)
                            {
                                if (objSelectedDOC.Status == Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingSubmission)
                                {
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    Modules.BusinessObjects.Setting.DOC Objdoclog = os.FindObject<Modules.BusinessObjects.Setting.DOC>(CriteriaOperator.Parse("Oid =?", objSelectedDOC.Oid));
                                    Objdoclog.Status = Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingSubmission;
                                    Objdoclog.DateSubmitted = DateTime.Now;
                                    os.CommitChanges();
                                    Application.MainWindow.View.ObjectSpace.CommitChanges();
                                }

                            }
                            View.ObjectSpace.Refresh();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                    }
                    Application.ShowViewStrategy.ShowMessage("Select the check box.", InformationType.Error, 3000, InformationPosition.Top);
                    return;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                //ASPxGridView grid = sender as ASPxGridView;

                if (View.Id == "DOC_ListView")
                {
                    if (e.DataColumn.FieldName == "DOCID")
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'DOCIDPopup', 'DOCID|'+{0}, '', false)", e.VisibleIndex));
                    }
                    else if (e.DataColumn.FieldName == "DOCReport")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'DOCIDPopup', 'DOCReport|'+{0}, '', false)", e.VisibleIndex));
                    }
                    else if (e.DataColumn.FieldName == "Certificate")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'DOCIDPopup', 'Certificate|'+{0}, '', false)", e.VisibleIndex));
                    }
                    else
                    {
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Input_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.Setting.DOC objDOC = (Modules.BusinessObjects.Setting.DOC)e.CurrentObject;
                if (objDOC != null)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    objDOC = os.GetObjectByKey<Modules.BusinessObjects.Setting.DOC>(objDOC.Oid);
                    DetailView dvInput = Application.CreateDetailView(os, "DOC_DetailView_Copy", false, objDOC);
                    dvInput.Caption = "Input";
                    if (objDOC.Status==Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingSubmission && objDocinfo.IsWrite)
                    {
                    dvInput.ViewEditMode = ViewEditMode.Edit;
                    }
                    else
                    {
                        dvInput.ViewEditMode = ViewEditMode.View;
                    }
                    ShowViewParameters showViewParameters = new ShowViewParameters(dvInput);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.CreatedView = dvInput;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    //dc.SaveOnAccept = true;
                    //dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += Dc_Input;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Input(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
            if (View != null || e.AcceptActionArgs.CurrentObject != null)
            {
                DialogController DC = sender as DialogController;
                DC.Window.View.ObjectSpace.CommitChanges();
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
                //}
            base.OnDeactivated();
            if (View.Id == "DOC_ListView")
            {
                //ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //if (gridListEditor != null && gridListEditor.Grid != null)
                //{
                //    ASPxGridView grid = gridListEditor.Grid;
                //    //grid.HtmlDataCellPrepared -= Grid_HtmlDataCellPrepared;
                   
                //}
                    WebWindow.CurrentRequestWindow.PagePreRender -= new EventHandler(CurrentRequestWindow_PagePreRender);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                    //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executed -= DeleteAction_Executed;
                Frame.GetController<DeleteObjectsViewController>().DeleteAction.TargetObjectsCriteria = null;
                    Submit.Active.RemoveItem("hideSubmit");
                    Validate.Active.RemoveItem("hideValidate");
                    DOCRollBack.Active.RemoveItem("hideDOCRollBack");
                    objDocinfo.IsWrite = false;
                }
                //if (View.Id == "SampleParameter_ListView_Copy_DOC")
                //{
                //    View.ControlsCreated -= View_ControlsCreated;
                //}
                if (View.Id == "DOC_DetailView_Copy_DV")
            {
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
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
                    if (View.Id == "DOC_ListView")
                    {
                        string[] param = parameter.Split('|');
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            string strGuid = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid").ToString();
                            HttpContext.Current.Session["rowid"] = strGuid;
                            if (param[0] == "DOCID" && !string.IsNullOrEmpty(strGuid))
                            {
                                Modules.BusinessObjects.Setting.DOC Objstudylog = os.FindObject<Modules.BusinessObjects.Setting.DOC>(CriteriaOperator.Parse("[Oid]= ?", new Guid(strGuid)));
                                IList<SampleParameter> lstSamples = null;
                                IList<Guid> Oid = new List<Guid>();
                                IList<string> lstparam = new List<string>();
                                List<string> lstQC = Objstudylog.QCBatches.Split(';').ToList();
                                if (Objstudylog.IsQCBatchID)
                                {
                                    lstSamples = os.GetObjects<SampleParameter>((new GroupOperator(GroupOperatorType.And, new InOperator("UQABID.AnalyticalBatchID", lstQC), CriteriaOperator.Parse("[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] == 'LCS'"))));
                                }
                                else
                                {
                                    lstSamples = os.GetObjects<SampleParameter>((new GroupOperator(GroupOperatorType.And, new InOperator("UQABID.AnalyticalBatchID", lstQC), CriteriaOperator.Parse("[Samplelogin] Is Not Null And [Samplelogin.QCCategory] Is Not Null And ([Samplelogin.QCCategory.QCCategoryName] =='DOC' OR [Samplelogin.QCCategory.QCCategoryName] =='MDL' OR [Samplelogin.QCCategory.QCCategoryName] =='PT') AND [Status] = 'PendingReporting'"))));
                                }
                                //IList<SampleParameter> lstSamples = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And ([QCBatchID.QCType.QCTypeName] =='DOC' OR [QCBatchID.QCType.QCTypeName] =='MDL' OR [QCBatchID.QCType.QCTypeName] =='PT') AND [Status] = 'PendingReporting' AND Contains([UQABID.Jobid], ?)", Objstudylog.JobID.JobID)).GroupBy(sample => sample.Testparameter.Parameter).Select(group => group.First()).ToList();
                                if (Objstudylog != null)
                                {
                                    if (lstSamples.Count > 0)
                                    {
                                        DOCDetails objPT = null;
                                        foreach (string parameter1 in lstSamples.Where(i => i.Testparameter != null && i.Testparameter.Parameter != null).Select(i => i.Testparameter.Parameter.ParameterName).Distinct())
                                        {
                                        int columnIndex = 1;
                                            if (lstSamples.Where(i => i.Testparameter != null && i.Testparameter.Parameter != null && i.Testparameter.Parameter.ParameterName == parameter1).FirstOrDefault(i => i.DOCDetail != null) == null)
                                        {
                                            objPT = os.CreateObject<DOCDetails>();
                                            objPT.DOC = os.GetObject<Modules.BusinessObjects.Setting.DOC>(Objstudylog);
                                        }
                                        else
                                        {
                                                objPT = lstSamples.Where(i => i.Testparameter != null && i.Testparameter.Parameter != null && i.Testparameter.Parameter.ParameterName == parameter1).FirstOrDefault(i => i.DOCDetail != null).DOCDetail;
                                            objPT.DOC = os.GetObject<Modules.BusinessObjects.Setting.DOC>(Objstudylog);
                                        }
                                            foreach (SampleParameter qcparam in lstSamples.Where(i => i.Testparameter != null && i.Testparameter.Parameter != null && i.Testparameter.Parameter.ParameterName == parameter1).OrderBy(i => i.SysSampleCode))
                                        {
                                                if (columnIndex <= 8)
                                            {
                                            qcparam.DOCDetail = objPT;
                                            var property = objPT.GetType().GetProperty($"Result{columnIndex}");
                                            var property1 = objPT.GetType().GetProperty($"Rec{columnIndex}");
                                            if (property != null)
                                            {
                                                        if (double.TryParse(qcparam.Result, out double doubleValue))
                                                        {
                                                            double roundedValue = Math.Round(doubleValue, 2);
                                                            string result = roundedValue.ToString("0.00");
                                                            property.SetValue(objPT, result);
                                                property1.SetValue(objPT, qcparam.Rec);
                                                columnIndex++;
                                                    }
                                                }
                                            }
                                            }
                                            if (columnIndex < 8)
                                            {
                                                for (int i = columnIndex; i <= 8; i++)
                                                {
                                                    var property = objPT.GetType().GetProperty($"Result{columnIndex}");
                                                    var property1 = objPT.GetType().GetProperty($"Rec{columnIndex}");
                                                    if (property != null)
                                                    {
                                                        property.SetValue(objPT, null);
                                                        property1.SetValue(objPT, null);
                                                        columnIndex++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    os.CommitChanges();
                                    foreach (var selectedObject in lstSamples.OrderBy(i => i.SysSampleCode))
                                    {

                                        if (selectedObject is SampleParameter spm && !lstparam.Contains(selectedObject.Testparameter.Parameter.ParameterName))
                                        {
                                            lstparam.Add(selectedObject.Testparameter.Parameter.ParameterName);
                                            DOCDetails docDetails = spm.DOCDetail;
                                            int countResult = 0;
                                            double sumResult = 0;
                                            int countRec = 0;
                                            double sumRec = 0;
                                            for (int i = 1; i <= 8; i++)
                                            {
                                                string resultPropertyName = $"Result{i}";
                                                string recPropertyName = $"Rec{i}";
                                                var resultValue = docDetails.GetType().GetProperty(resultPropertyName)?.GetValue(docDetails, null);
                                                var recValue = docDetails.GetType().GetProperty(recPropertyName)?.GetValue(docDetails, null);

                                                if (resultValue!=null && double.TryParse(resultValue.ToString(), out _))
                                                {
                                                    countResult++;
                                                    sumResult += Convert.ToDouble(resultValue);
                                                }
                                                if (recValue != null)
                                                {
                                                    countRec++;
                                                    sumRec += Convert.ToDouble(recValue);

                                                }
                                                else if (recValue == null && resultValue != null && double.TryParse(resultValue.ToString(), out _) && spm.SpikeAmount != null)
                                                {
                                                    recValue = (Convert.ToDouble(resultValue) / Convert.ToDouble(spm.SpikeAmount) * 100);
                                                    countRec++;
                                                    sumRec += Convert.ToDouble(recValue);
                                                }
                                            }
                                            if (countResult > 0)
                                            {
                                                double averageResult = sumResult / countResult;
                                                spm.Average = averageResult;
                                                double sumSquaredDeviations = 0;
                                                for (int i = 1; i <= 8; i++)
                                                {
                                                    string resultPropertyName = $"Result{i}";
                                                    var resultValue = docDetails.GetType().GetProperty(resultPropertyName)?.GetValue(docDetails, null);

                                                    if (resultValue != null && double.TryParse(resultValue.ToString(), out _))
                                                    {
                                                        double deviation = Convert.ToDouble(resultValue) - averageResult;
                                                        sumSquaredDeviations += Math.Pow(deviation, 2);
                                                    }
                                                }

                                                double variance = sumSquaredDeviations / (countResult - 1);
                                                double standardDeviation = Math.Sqrt(variance);
                                                double roundedSD = Math.Round(standardDeviation, 4);
                                                if (roundedSD > 0)
                                                {
                                                    spm.SD = roundedSD;
                                                }
                                                if (sumSquaredDeviations > 0)
                                                {
                                                    spm.RSD = (decimal)(roundedSD / spm.Average * 100);
                                                }
                                                else
                                                {
                                                    spm.RSD = 0;
                                                }
                                            }

                                            if (countRec > 0)
                                            {
                                                decimal averageRec = (decimal)(sumRec / countRec);
                                                spm.AvgRec = averageRec;
                                            }
                                            ObjectSpace.CommitChanges();
                                            os.CommitChanges();
                                            Oid.Add(spm.Oid);
                                        }
                                    }
                                    View.ObjectSpace.Refresh();
                                    Application.MainWindow.View.Refresh();
                                    os.Refresh();
                                }
                                CollectionSource cs = new CollectionSource(os, typeof(SampleParameter));
                                cs.Criteria["Filter"] = new InOperator("Oid", Oid);
                                //if (lstSamples.Count > 0)
                                //{
                                //cs.Criteria["Filter"] = new InOperator("Oid", lstSamples.Select(i => i.Oid).Distinct().ToList());
                                //}
                                //else
                                //{
                                //    cs.Criteria["Filter"] = new InOperator("Oid", lstSamples.Select(i => i.Oid).Distinct().ToList());
                                //}
                                ListView DOClv = Application.CreateListView("SampleParameter_ListView_Copy_DOC", cs, true);
                                ShowViewParameters showViewParameters = new ShowViewParameters(DOClv);
                                showViewParameters.CreatedView = DOClv;
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.SaveOnAccept = false;
                                dc.Accepting += Dc_Accepting;
                                dc.CancelAction.Active.SetItemValue("enb", false);
                                dc.CloseOnCurrentObjectProcessing = false;
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            }
                            #region Report
                            //else if (param[0] == "DOCReport" && !string.IsNullOrEmpty(strGuid))
                            //{
                            //    try
                            //    {
                            //        if (View != null)
                            //        {
                            //            string DOCID;
                            //            DOCID = string.Empty;
                            //            Modules.BusinessObjects.Setting.DOC obj = os.FindObject<Modules.BusinessObjects.Setting.DOC>(CriteriaOperator.Parse("[Oid]= ?", new Guid(strGuid)));
                            //            if (obj.Status != Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingSubmission && obj.Status != Modules.BusinessObjects.Setting.DOC.DOCstatus.Fail)
                            //            {
                            //                IList<SampleParameter> lstSamples = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And ([QCBatchID.QCType.QCTypeName] =='DOC' OR [QCBatchID.QCType.QCTypeName] =='MDL' OR [QCBatchID.QCType.QCTypeName] =='PT') AND [DOCDetail] Is Not Null AND [Status] = 'PendingReporting' AND Contains([UQABID.Jobid], ?)", obj.JobID.JobID)).GroupBy(sample => sample.Testparameter.Parameter).Select(group => group.First()).ToList();
                            //                IList<SampleParameter> lstSamples1 = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin] Is Not Null And [Samplelogin.QCCategory] Is Not Null And ([Samplelogin.QCCategory.QCCategoryName] =='DOC' OR [Samplelogin.QCCategory.QCCategoryName] =='MDL' OR [Samplelogin.QCCategory.QCCategoryName] =='PT') AND [Status] = 'PendingReporting' AND Contains([UQABID.Jobid], ?)", obj.JobID.JobID)).GroupBy(sample => sample.Testparameter.Parameter).Select(group => group.First()).ToList();
                            //                if (View.Id == "DOC_ListView")
                            //                {
                            //                    if (DOCID == string.Empty)
                            //                    {
                            //                        DOCID = "'" + obj.DOCID.ToString() + "'";
                            //                    }
                            //                    else
                                                    //{
                            //                        if (!DOCID.Contains(obj.DOCID.ToString()))
                            //                            DOCID = DOCID + ",'" + obj.DOCID.ToString() + "'";
                                                    //}
                            //                }

                            //                string strTempPath = Path.GetTempPath();
                            //                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            //                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                            //                {
                            //                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                            //                }

                            //                string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".pdf");
                            //                XtraReport xtraReport = new XtraReport();
                            //                DataView dv = new DataView();
                            //                DataTable dataTable = new DataTable();
                            //                dataTable.Columns.Add("DOCID");
                            //                dataTable.Columns.Add("TestName");
                            //                dataTable.Columns.Add("MethodNumber");
                            //                dataTable.Columns.Add("MatrixName");
                            //                dataTable.Columns.Add("SamplePrepType");
                            //                dataTable.Columns.Add("AnalysisName");
                            //                dataTable.Columns.Add("PreparedBy");
                                                //dataTable.Columns.Add("DatePrepared");
                            //                dataTable.Columns.Add("PrepBatchID");
                            //                dataTable.Columns.Add("QCBatchID");
                                                //dataTable.Columns.Add("JobID");
                            //                dataTable.Columns.Add("SpikeAmount");
                            //                dataTable.Columns.Add("SpikePrepInfo");
                                                //dataTable.Columns.Add("SpikeConcentration");
                                                //dataTable.Columns.Add("SpikeStandardID");
                            //                dataTable.Columns.Add("SpikeStandardName");
                            //                dataTable.Columns.Add("SpikeUnits");
                            //                dataTable.Columns.Add("PrepInstrument");
                            //                dataTable.Columns.Add("DateAnalyzed");
                                                //dataTable.Columns.Add("Parameter");
                                                //dataTable.Columns.Add("SPKAmt");
                                                //dataTable.Columns.Add("Result1");
                                                //dataTable.Columns.Add("Result2");
                                                //dataTable.Columns.Add("Result3");
                                                //dataTable.Columns.Add("Result4");
                                                //dataTable.Columns.Add("Result5");
                                                //dataTable.Columns.Add("Result6");
                                                //dataTable.Columns.Add("Result7");
                                                //dataTable.Columns.Add("Average");
                                                //dataTable.Columns.Add("%RecAve");
                                                //dataTable.Columns.Add("SD");
                                                //dataTable.Columns.Add("RSD");
                                                //dataTable.Columns.Add("MDL");
                                                //dataTable.Columns.Add("MDL1");
                            //                dataTable.Columns.Add("RptLimit");
                                                //dataTable.Columns.Add("Ratio");
                            //                dataTable.Columns.Add("QCInstruments");
                                                //dataTable.Columns.Add("DateApproved");
                            //                dataTable.Columns.Add("ApprovedBy");
                            //                dataTable.Columns.Add("Status");
                                                //dataTable.Columns.Add("LLimit");
                                                //dataTable.Columns.Add("HLimit");
                            //                dataTable.Columns.Add("MDLCheck");
                            //                dataTable.Columns.Add("Comments");
                            //                dataTable.Columns.Add("PrepMethod");
                            //                foreach (SampleParameter spm in lstSamples1)
                            //                {
                            //                    if (spm != null && spm.QCBatchID != null && spm.QCBatchID.QCType != null && spm.Samplelogin != null && (spm.Samplelogin.QCCategory != null && spm.Samplelogin.QCCategory.QCCategoryName == "DOC" || spm.QCBatchID.QCType.QCTypeName == "LCS"))
                            //                    {
                                                
                            //                        DataRow dr = dataTable.NewRow();
                            //                        dr["DOCID"] = obj.DOCID;
                            //                        if (obj.Test != null)
                            //                        {
                            //                            //dr["TestName"] = ObjectSpace.GetObjectByKey<TestMethod>(obj.Test.Oid);
                            //                            dr["TestName"] = obj.Test.TestName;
                            //                        }
                            //                        if (obj.Method != null)
                            //                        {
                            //                            dr["MethodNumber"] = obj.Method.MethodNumber;
                            //                        }
                            //                        if (obj.Matrix != null)
                            //                        {
                            //                            dr["MatrixName"] = obj.Matrix.MatrixName;
                            //                        }
                            //                        if (obj.Analyst != null)
                            //                        {
                            //                            dr["AnalysisName"] = obj.Analyst.DisplayName;
                            //                            //objPT.DateAnalyzed = objSampleResult.AnalyzedDate;
                            //                        }
                            //                        //dr["TestName"] = obj.Test;
                            //                        //dr["MethodNumber"] = obj.Method;
                            //                        //dr["TestName"] = ObjectSpace.GetObjectByKey<TestMethod>(obj.Test.Oid);
                            //                        //objPT.DateAnalyzed = objSampleResult.AnalyzedDate;
                            //                        if (spm.UQABID != null)
                            //                        {
                            //                            dr["QCBatchID"] = spm.UQABID.AnalyticalBatchID;
                            //                        }
                            //                        if (spm.UQABID != null)
                            //                        {
                            //                            dr["QCInstruments"] = spm.UQABID.strInstrument;
                            //                        }
                            //                        dr["JobID"] = obj.JobID.JobID;
                            //                        dr["SpikeAmount"] = spm.SpikeAmount;
                            //                        if (spm.SpikeAmountUnit != null)
                            //                        {
                            //                            dr["SpikeUnits"] = spm.SpikeAmountUnit.UnitName;
                            //                        }
                            //                        dr["DateAnalyzed"] = obj.DateAnalyzed;
                            //                        dr["SamplePrepType"] = spm.PrepBatchID;
                            //                        //dr["SpikePrepInfo"] = obj;
                            //                        dr["SpikeConcentration"] = obj.StandardConcentration;
                            //                        dr["SpikeStandardID"] = obj.StandardID;
                            //                        dr["SpikeStandardName"] = obj.StandardName;
                            //                        dr["PrepInstrument"] = obj.PrepInstrument;
                            //                        dr["Parameter"] = spm.Testparameter.Parameter.ParameterName;
                            //                        dr["SPKAmt"] = spm.SpikeAmount;
                            //                        if (obj.PreparedBy != null)
                            //                        {
                            //                            dr["PreparedBy"] = obj.PreparedBy.DisplayName;

                            //                        }
                            //                        dr["DatePrepared"] = obj.DatePrepared;
                            //                        if (spm.PrepBatchID != null)
                            //                        {
                            //                            List<string> lst = spm.PrepBatchID.Split(';').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                            //                            if (lst.Contains(obj.Oid.ToString()))
                            //                            {
                            //                                lst.Add(obj.Oid.ToString());
                            //                            }
                            //                            dr["PrepBatchID"] = lst.FirstOrDefault();

                            //                        }
                            //                        Modules.BusinessObjects.Setting.DOCDetails objDetails = os.FindObject<Modules.BusinessObjects.Setting.DOCDetails>(CriteriaOperator.Parse("[Oid]= ?", spm.DOCDetail.Oid));

                            //                        if (objDetails != null)
                            //                        {
                            //                            dr["Result1"] = objDetails.Result1;
                            //                            dr["Result2"] = objDetails.Result2;
                            //                            dr["Result3"] = objDetails.Result3;
                            //                            dr["Result4"] = objDetails.Result4;
                            //                            dr["Result5"] = objDetails.Result5;
                            //                            dr["Result6"] = objDetails.Result6;
                            //                            dr["Result7"] = objDetails.Result7;
                            //                        }
                            //                        dr["Average"] = spm.Average;
                            //                        dr["%RecAve"] = spm.AvgRec;
                            //                        dr["SD"] = spm.SD;
                            //                        dr["RSD"] = spm.RSD;
                            //                        //dr["MDL"] = obj;
                            //                        //dr["MDL1"] = obj;
                            //                        dr["RptLimit"] = spm.RptLimit;
                            //                        //dr["Ratio"] = spm;
                            //                        if (spm.UQABID != null && spm.UQABID.Instrument != null)
                            //                        {
                            //                            //List<string> lst = spm.UQABID.Instrument.Split(';').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                            //                            //if (lst.Contains(spm.UQABID.Instrument.ToString()))
                            //                            //{
                            //                            //    dr["QCInstruments"] = os.GetObjectByKey<Labware>(spm.UQABID.Instrument);
                            //                            //    lst.Add(spm.UQABID.Instrument.ToString());
                            //                            //}
                            //                            //dr["QCInstruments"] = lst.FirstOrDefault();
                                                
                            //                        }
                            //                        //dr["QCInstruments"] = spm.UQABID.Instrument;
                            //                        dr["DateApproved"] = spm.ApprovedDate;
                            //                        if (spm.ApprovedBy != null)
                            //                        {
                            //                            dr["ApprovedBy"] = spm.ApprovedBy.DisplayName;
                            //                        }
                            //                        //dr["Status"] = obj;
                            //                        dr["LLimit"] = spm.Testparameter.RPDLCLimit;
                            //                        dr["HLimit"] = spm.Testparameter.RPDHCLimit;
                            //                        //dr["MDLCheck"] = obj;
                            //                        dr["Comments"] = obj.Comment;
                            //                        dataTable.Rows.Add(dr);
                                        //String strProjectName = ConfigurationManager.AppSettings["ProjectName"];
                                        //if (strProjectName == "Consci")
                                        //{


                                        //    XrDOCReport sampleLabelReport1 = new XrDOCReport();
                                        //    sampleLabelReport1.DataSource = dataTable;
                                        //    xtraReport = sampleLabelReport1;
                                        //    sampleLabelReport1.DataBindingsToReport();

                                        //}
                                        //else
                                        //{
                                        //    XrDOCReport sampleLabelReport = new XrDOCReport();
                                        //    sampleLabelReport.DataSource = dataTable;
                                        //    xtraReport = sampleLabelReport;
                                        //    sampleLabelReport.DataBindingsToReport();
                                        //}
                                        //xtraReport.ExportToPdf(strExportedPath);
                                        //string[] path = strExportedPath.Split('\\');
                                        //int arrcount = path.Count();
                                        //int sc = arrcount - 2;
                                        //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                                        //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));

                            //                    }
                            //                    else if (spm != null && spm.Samplelogin != null && spm.Samplelogin.QCCategory != null && spm.Samplelogin.QCCategory.QCCategoryName == "MDL")
                            //                    {
                            //                        //List<string> lst = spm.UQABID.Instrument.Split(';').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                            //                        //if (lst.Contains(spm.UQABID.Instrument.ToString()))
                            //                        //{
                            //                        //    dr["QCInstruments"] = os.GetObjectByKey<Labware>(spm.UQABID.Instrument);
                            //                        //    lst.Add(spm.UQABID.Instrument.ToString());
                            //                        //}
                            //                        //dr["QCInstruments"] = lst.FirstOrDefault();
                            //                        //dataTable.Columns.Add("DatePrepared");
                            //                        //dataTable.Columns.Add("TCLPPrepMethod");
                            //                        //dataTable.Columns.Add("TCLPPrepedBy");
                            //                        //dataTable.Columns.Add("DateTCLPPrepared");
                            //                        //dataTable.Columns.Add("DateAnalyzed");
                            //                        //dataTable.Columns.Add("AnalyzedBy");
                            //                        //dataTable.Columns.Add("QCInstruments");
                            //                        //dataTable.Columns.Add("JobID");
                            //                        //dataTable.Columns.Add("SpikeUnits");
                            //                        //dataTable.Columns.Add("SpikeConcentration");
                            //                        //dataTable.Columns.Add("SpikePrepInfo");
                            //                        //dataTable.Columns.Add("SpikeStandardID");
                            //                        //dataTable.Columns.Add("Parameter");
                            //                        //dataTable.Columns.Add("SPKAmt");
                            //                        //dataTable.Columns.Add("Result1");
                            //                        //dataTable.Columns.Add("Result2");
                            //                        //dataTable.Columns.Add("Result3");
                            //                        //dataTable.Columns.Add("Result4");
                            //                        //dataTable.Columns.Add("Result5");
                            //                        //dataTable.Columns.Add("Result6");
                            //                        //dataTable.Columns.Add("Result7");
                            //                        //dataTable.Columns.Add("Average");
                            //                        //dataTable.Columns.Add("%RecAve");
                            //                        //dataTable.Columns.Add("SD");
                            //                        //dataTable.Columns.Add("RSD");
                            //                        //dataTable.Columns.Add("MDL");
                            //                        //dataTable.Columns.Add("MDL1");
                            //                        //dataTable.Columns.Add("MDLCheck");
                            //                        //dataTable.Columns.Add("Ratio");
                            //                        //dataTable.Columns.Add("RptLimit");
                            //                        //dataTable.Columns.Add("DateApproved");
                            //                        //dataTable.Columns.Add("Comments");
                            //                        //dataTable.Columns.Add("LLimit");
                            //                        //dataTable.Columns.Add("HLimit");
                            //                        DataRow dr = dataTable.NewRow();
                            //                        dr["DOCID"] = obj.DOCID;

                            //                        if (obj.Test != null)
                            //                        {
                            //                            dr["TestName"] = obj.Test.TestName;
                            //                        }
                            //                        if (obj.Method != null)
                            //                        {
                            //                            dr["MethodNumber"] = obj.Method.MethodNumber;
                            //                        }
                            //                        if (obj.Matrix != null)
                            //                        {
                            //                            dr["MatrixName"] = obj.Matrix.MatrixName;
                            //                        }
                            //                        dr["PrepMethod"] = spm.PrepBatchID;

                            //                        dr["PrepInstrument"] = obj.PrepInstrument;

                            //                        if (obj.PreparedBy != null)
                            //                        {
                            //                            dr["PreparedBy"] = obj.PreparedBy.DisplayName;

                            //                        }
                            //                        dr["DatePrepared"] = obj.DatePrepared;
                            //                        dr["TCLPPrepMethod"] = obj.TCLPPrepedMethod;
                            //                        dr["TCLPPrepedBy"] = obj.TCLPPrepedBy;


                            //                        dr["DateTCLPPrepared"] = obj.DateTCLPPreped;

                            //                        if (obj.Analyst != null)
                            //                        {
                            //                            dr["AnalyzedBy"] = obj.Analyst.DisplayName;
                            //                        }
                            //                        dr["DateAnalyzed"] = obj.DateAnalyzed;
                            //                        if (spm.UQABID != null)
                            //                        {
                            //                            dr["QCInstruments"] = spm.UQABID.strInstrument;
                            //                        }
                            //                        dr["JobID"] = obj.JobID.JobID;
                            //                        if (spm.SpikeAmountUnit != null)
                            //                        {
                            //                            dr["SpikeUnits"] = spm.SpikeAmountUnit.UnitName;
                            //                        }
                            //                        dr["SpikeConcentration"] = obj.StandardConcentration;
                            //                        //dr["SpikePrepInfo"] = spm.SpikeAmount;
                            //                        dr["SpikeStandardID"] = obj.StandardID;
                            //                        dr["Parameter"] = spm.Testparameter.Parameter.ParameterName;
                            //                        dr["SPKAmt"] = spm.SpikeAmount;
                            //                        Modules.BusinessObjects.Setting.DOCDetails objDetails = os.FindObject<Modules.BusinessObjects.Setting.DOCDetails>(CriteriaOperator.Parse("[Oid]= ?", spm.DOCDetail.Oid));

                            //                        if (objDetails != null)
                            //                        {
                            //                            dr["Result1"] = objDetails.Result1;
                            //                            dr["Result2"] = objDetails.Result2;
                            //                            dr["Result3"] = objDetails.Result3;
                            //                            dr["Result4"] = objDetails.Result4;
                            //                            dr["Result5"] = objDetails.Result5;
                            //                            dr["Result6"] = objDetails.Result6;
                            //                            dr["Result7"] = objDetails.Result7;
                            //                        }
                            //                        dr["Average"] = spm.Average;
                            //                        dr["%RecAve"] = spm.AvgRec;
                            //                        dr["SD"] = spm.SD;
                            //                        dr["RSD"] = spm.RSD;
                            //                        dr["MDL"] = spm.MDL;
                            //                        dr["MDL1"] = spm.MDL;
                            //                        dr["MDLCheck"] = spm.MDL;
                            //                        //dr["Ratio"] = spm.Testparamete;
                            //                        dr["RptLimit"] = spm.RptLimit;
                            //                        dr["DateApproved"] = spm.ApprovedDate;
                            //                        dr["LLimit"] = spm.RPDLCLimit;
                            //                        dr["HLimit"] = spm.RPDHCLimit;
                            //                        dr["Comments"] = obj.Comment;
                            //                        dataTable.Rows.Add(dr);

                            //                        String strProjectName = ConfigurationManager.AppSettings["ProjectName"];
                            //                        if (strProjectName == "Consci")
                                            //{
                            //                            XrMDLReport sampleLabelReport1 = new XrMDLReport();
                            //                            sampleLabelReport1.DataSource = dataTable;
                            //                            xtraReport = sampleLabelReport1;
                            //                            sampleLabelReport1.MDLDataBindingsToReport();
                            //                        }
                            //                        else
                                            //    {
                            //                            XrMDLReport sampleLabelReport = new XrMDLReport();
                            //                            sampleLabelReport.DataSource = dataTable;
                            //                            xtraReport = sampleLabelReport;
                            //                            sampleLabelReport.MDLDataBindingsToReport();
                            //                        }
                            //                        xtraReport.ExportToPdf(strExportedPath);
                            //                        string[] path = strExportedPath.Split('\\');
                            //                        int arrcount = path.Count();
                            //                        int sc = arrcount - 2;
                            //                        string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                            //                        WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));

                            //                    }
                            //                }

                            //                //string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            //                //var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                            //                //string serverName = connectionStringBuilder.DataSource.Trim();
                            //                //string databaseName = connectionStringBuilder.InitialCatalog.Trim();
                            //                //string userID = connectionStringBuilder.UserID.Trim();
                            //                //string password = connectionStringBuilder.Password.Trim();
                            //                ////string sqlSelect = "Select * from QAQC_DOCCertificateRPT_SP where [JobID] in(" + DOCID + ")";
                            //                //string sqlSelect = "exec QAQC_DOCCertificateRPT_SP @DOCID = " + DOCID + "";
                            //                //SqlConnection sqlConnection = new SqlConnection(connectionStringBuilder.ToString());
                            //                //SqlCommand sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                            //                //SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand);
                            //                //sqlDa.Fill(dataTable);
                            //                //String strProjectName = ConfigurationManager.AppSettings["ProjectName"];
                            //                //if (strProjectName == "Consci")
                            //                //{


                            //                //    XrDOCReport sampleLabelReport1 = new XrDOCReport();
                            //                //    sampleLabelReport1.DataSource = dataTable;
                            //                //    xtraReport = sampleLabelReport1;
                            //                //    sampleLabelReport1.DataBindingsToReport();

                            //                //}
                            //                //else
                            //                //{
                            //                //    XrDOCReport sampleLabelReport = new XrDOCReport();
                            //                //    sampleLabelReport.DataSource = dataTable;
                            //                //    xtraReport = sampleLabelReport;
                            //                //    sampleLabelReport.DataBindingsToReport();
                            //                //}
                            //                //xtraReport.ExportToPdf(strExportedPath);
                            //                //string[] path = strExportedPath.Split('\\');
                            //                //int arrcount = path.Count();
                            //                //int sc = arrcount - 2;
                            //                //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                            //                //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                                            //    }
                                            //    else
                                            //    {
                            //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DOC", "ReportAvailable"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            //            }
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                            //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                                            //    }
                                            //}
                            //else if (param[0] == "Certificate" && !string.IsNullOrEmpty(strGuid))
                            //{
                            //    try
                            //    {
                            //        if (View != null)
                            //        {
                            //            string DOCID;
                            //            DOCID = string.Empty;
                            //            Modules.BusinessObjects.Setting.DOC obj = os.FindObject<Modules.BusinessObjects.Setting.DOC>(CriteriaOperator.Parse("[Oid]= ?", new Guid(strGuid)));
                            //            if (obj.Status != Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingSubmission && obj.Status != Modules.BusinessObjects.Setting.DOC.DOCstatus.Fail)
                            //            {
                            //                if (View.Id == "DOC_ListView")
                                        //    {
                                        //        if (DOCID == string.Empty)
                                        //        {
                                        //            DOCID = "'" + obj.DOCID.ToString() + "'";
                                        //        }
                                        //        else
                                        //        {
                                            //        if (!DOCID.Contains(obj.DOCID.ToString()))
                                        //            DOCID = DOCID + ",'" + obj.DOCID.ToString() + "'";
                                        //        }
                            //                    //foreach (Modules.BusinessObjects.Setting.DOC obj in View.SelectedObjects)
                            //                    //{
                            //                    //    if (DOCID == string.Empty)
                            //                    //    {
                            //                    //        DOCID = "'" + obj.DOCID.ToString() + "'";
                            //                    //    }
                            //                    //    else
                            //                    //    {
                            //                    //        if (!DOCID.Contains(obj.DOCID.ToString()))
                            //                    //            DOCID = DOCID + ",'" + obj.DOCID.ToString() + "'";
                            //                    //    }
                            //                    //}
                            //                }
                            //                //    foreach (Modules.BusinessObjects.Setting.DOC obj in View.SelectedObjects)
                            //                //    {
                            //                //        if (DOCID == string.Empty)
                            //                //        {
                            //                //            DOCID = "'" + obj.DOCID.ToString() + "'";
                            //                //        }
                            //                //        else
                            //                //        {
                            //                //        if (!DOCID.Contains(obj.DOCID.ToString()))
                            //                //            DOCID = DOCID + ",'" + obj.DOCID.ToString() + "'";
                            //                //        }
                            //                //    }
                            //                //}
                            //                //}
                            //                string strTempPath = Path.GetTempPath();
                            //                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            //                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                            //                {
                            //                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                            //                }

                            //                string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".pdf");
                            //                XtraReport xtraReport = new XtraReport();
                            //                DataView dv = new DataView();
                            //                DataTable dataTable = new DataTable();
                            //                dataTable.Columns.Add("DOCID");
                            //                dataTable.Columns.Add("TestName");
                            //                dataTable.Columns.Add("MethodNumber");
                            //                dataTable.Columns.Add("MatrixName");
                            //                dataTable.Columns.Add("SamplePrepType");
                            //                dataTable.Columns.Add("AnalysisName");
                            //                dataTable.Columns.Add("PreparedBy");
                            //                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            //                var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                            //                string serverName = connectionStringBuilder.DataSource.Trim();
                            //                string databaseName = connectionStringBuilder.InitialCatalog.Trim();
                            //                string userID = connectionStringBuilder.UserID.Trim();
                            //                string password = connectionStringBuilder.Password.Trim();
                            //                //string sqlSelect = "Select * from QAQC_DOCCertificateRPT_SP where [JobID] in(" + DOCID + ")";
                            //                string sqlSelect = "exec QAQC_DOCCertificateRPT_SP @DOCID = " + DOCID + "";
                            //                SqlConnection sqlConnection = new SqlConnection(connectionStringBuilder.ToString());
                            //                SqlCommand sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                            //                SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand);
                            //                sqlDa.Fill(dataTable);
                            //                String strProjectName = ConfigurationManager.AppSettings["ProjectName"];
                            //                if (strProjectName == "Consci")
                            //                {


                            //                    DOCCertificate sampleLabelReport1 = new DOCCertificate();
                            //                    sampleLabelReport1.DataSource = dataTable;
                            //                    xtraReport = sampleLabelReport1;
                            //                    sampleLabelReport1.DataBindingsToReport();

                            //                }
                            //                else
                            //                {
                            //                    DOCCertificate sampleLabelReport = new DOCCertificate();
                                        //sampleLabelReport.DataSource = dataTable;
                                        //xtraReport = sampleLabelReport;
                                        //sampleLabelReport.DataBindingsToReport();
                            //                }

                            //                //FolderLabelReport sampleLabelReport = new FolderLabelReport();
                            //                //sampleLabelReport.DataSource = dataTable;
                            //                //xtraReport = sampleLabelReport;
                            //                //sampleLabelReport.DataBindingsToReport();
                            //                xtraReport.ExportToPdf(strExportedPath);
                            //                string[] path = strExportedPath.Split('\\');
                            //                int arrcount = path.Count();
                            //                int sc = arrcount - 2;
                            //                string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                            //                WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                            //                //if (View.SelectedObjects.Count > 0)
                            //                //{

                            //                //}
                            //                //else
                            //                //{
                            //                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            //                //}
                                        //}
                                        //else
                                        //{
                            //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DOC", "CertificateAvailable"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                        //}
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                            //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                            //    }
                            //} 
                            #endregion
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

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                DialogController DC = sender as DialogController;
                
                if (e.AcceptActionArgs.CurrentObject != null && View.Id == "DOC_ListView")
                {
                    if (DC.Window.View.Id == "DOC_DetailView_Copy_DV")
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        Modules.BusinessObjects.Setting.DOC objdoC = (Modules.BusinessObjects.Setting.DOC)DC.Window.View.CurrentObject;
                        if (objdoC != null && !string.IsNullOrEmpty(objdoC.QCBatches) && objdoC.Matrix != null && objdoC.Test != null && objdoC.Method != null)
                        {
                            //SampleParameter sampleparameter = DC.Window.View.ObjectSpace.FindObject<SampleParameter>(CriteriaOperator.Parse("[UQABID.AnalyticalBatchID] = ? AND [Samplelogin] Is Not Null", objdoC.QCBatches));
                            //if (sampleparameter != null && sampleparameter.Samplelogin != null && sampleparameter.Samplelogin.JobID != null)
                            //{
                            //    Samplecheckin lstobj = DC.Window.View.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]= ?", sampleparameter.Samplelogin.JobID.Oid));
                           
                            if (DC != null && DC.Window != null && DC.Window.View != null)
                            {
                                if (View.Id == "DOC_ListView")
                                {
                                    DetailView detailView = (DetailView)DC.Window.View;
                                    if (detailView != null)
                                    {
                                        IList<Guid> Oid = new List<Guid>();
                                        IList<string> param = new List<string>();
                                        List<string> lstQC = objdoC.QCBatches.Split(';').ToList();
                                        if (lstQC.Count() > 0)
                                        {
                                            IList<SampleParameter> lstSamples = os.GetObjects<SampleParameter>((new GroupOperator(GroupOperatorType.And, new InOperator("UQABID.AnalyticalBatchID", lstQC),CriteriaOperator.Parse("[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] == 'LCS'"))));
                                            objdoC.strJobID = string.Join(";", lstSamples.Select(i => i.JobID).Distinct());
                                            DC.Window.View.ObjectSpace.CommitChanges();
                                            if (lstSamples.Count > 0)
                                            {
                                                DOCDetails objPT = null;
                                                if (lstSamples.FirstOrDefault(i => i.PrepBatchID != null) != null && lstSamples.FirstOrDefault(i => i.PrepBatchID != null).PrepBatchID != null)
                                                {
                                                    List<Guid> lstpep = lstSamples.Where(i => i.PrepBatchID != null).SelectMany(i => i.PrepBatchID.Split(';')).Select(Guid.Parse).Distinct().ToList();
                                                    if (lstpep.Count > 0)
                                                    {
                                                        IList<SamplePrepBatch> lstPrep = ObjectSpace.GetObjects<SamplePrepBatch>(new InOperator("Oid", lstpep));
                                                        if (lstpep.Count > 0)
                                                        {
                                                            objdoC.PrepBatchID = string.Join(";", lstPrep.Select(i => i.PrepBatchID));
                                                        }
                                                    }
                                                }
                                                if (lstSamples.FirstOrDefault(i => i.UQABID != null) != null && lstSamples.FirstOrDefault(i => i.UQABID != null).UQABID != null)
                                                {
                                                    objdoC.AnalyticalInstrument = string.Join(";", lstSamples.Where(i => i.UQABID != null && !string.IsNullOrEmpty(i.UQABID.strInstrument)).SelectMany(i => i.UQABID.strInstrument.Split(';')).Distinct());

                                                }
                                                foreach (string parameter in lstSamples.Where(i=>i.Testparameter!=null&&i.Testparameter.Parameter!=null).Select(i=> i.Testparameter.Parameter.ParameterName).Distinct())
                                                {
                                                int columnIndex = 1;
                                                    if (lstSamples.Where(i => i.Testparameter != null && i.Testparameter.Parameter != null&&i.Testparameter.Parameter.ParameterName== parameter).FirstOrDefault(i => i.DOCDetail != null) == null)
                                                {
                                                    objPT = os.CreateObject<DOCDetails>();
                                                    objPT.DOC = os.GetObject<Modules.BusinessObjects.Setting.DOC>(objdoC);
                                                }
                                                else
                                                {
                                                        objPT = lstSamples.Where(i => i.Testparameter != null && i.Testparameter.Parameter != null && i.Testparameter.Parameter.ParameterName == parameter).FirstOrDefault(i => i.DOCDetail != null).DOCDetail;
                                                    objPT.DOC = os.GetObject<Modules.BusinessObjects.Setting.DOC>(objdoC);
                                                }
                                                    foreach (SampleParameter qcparam in lstSamples.Where(i => i.Testparameter != null && i.Testparameter.Parameter != null && i.Testparameter.Parameter.ParameterName == parameter).OrderBy(i => i.SysSampleCode))
                                                    {
                                                        if (columnIndex<=8)
                                                {
                                                    qcparam.DOCDetail = objPT;
                                                    var property = objPT.GetType().GetProperty($"Result{columnIndex}");
                                                    var property1 = objPT.GetType().GetProperty($"Rec{columnIndex}");
                                                    if (property != null)
                                                    {
                                                        property.SetValue(objPT, qcparam.Result);
                                                        property1.SetValue(objPT, qcparam.Rec);
                                                        columnIndex++;
                                                        }
                                                    }
                                                }
                                            }

                                            }
                                            os.CommitChanges();
                                            Application.ShowViewStrategy.ShowMessage("DOC '"+ objdoC.DOCID +"' created successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                                            foreach (var selectedObject in lstSamples.OrderBy(i => i.SysSampleCode))
                                            {
                                                if (selectedObject is SampleParameter spm && selectedObject.Testparameter!=null&& selectedObject.Testparameter.Parameter!=null && !param.Contains(selectedObject.Testparameter.Parameter.ParameterName))
                                                {
                                                    param.Add(selectedObject.Testparameter.Parameter.ParameterName);
                                                    DOCDetails docDetails = spm.DOCDetail;
                                                    int countResult = 0;
                                                    double sumResult = 0;
                                                    int countRec = 0;
                                                    double sumRec = 0;
                                                    for (int i = 1; i <= 8; i++)
                                                    {
                                                        string resultPropertyName = $"Result{i}";
                                                        string recPropertyName = $"Rec{i}";
                                                        var resultValue = docDetails.GetType().GetProperty(resultPropertyName)?.GetValue(docDetails, null);
                                                        var recValue = docDetails.GetType().GetProperty(recPropertyName)?.GetValue(docDetails, null);

                                                        if (resultValue != null && double.TryParse(resultValue.ToString(), out _))
                                                        {
                                                            countResult++;
                                                            sumResult += Convert.ToDouble(resultValue);
                                                        }
                                                        if (recValue != null)
                                                        {
                                                            countRec++;
                                                            sumRec += Convert.ToDouble(recValue);

                                                        }
                                                        else if (recValue == null && resultValue != null && double.TryParse(resultValue.ToString(), out _) && spm.SpikeAmount != null)
                                                        {
                                                            recValue = (Convert.ToDouble(resultValue) / Convert.ToDouble(spm.SpikeAmount) * 100);
                                                            countRec++;
                                                            sumRec += Convert.ToDouble(recValue);
                                                        }
                                                    }
                                                    if (countResult > 0)
                                                    {
                                                        double averageResult = sumResult / countResult;
                                                        spm.Average = averageResult;
                                                        double sumSquaredDeviations = 0;
                                                        for (int i = 1; i <= 8; i++)
                                                        {
                                                            string resultPropertyName = $"Result{i}";
                                                            var resultValue = docDetails.GetType().GetProperty(resultPropertyName)?.GetValue(docDetails, null);

                                                            if (resultValue != null && double.TryParse(resultValue.ToString(), out _))
                                                            {
                                                                double deviation = Convert.ToDouble(resultValue) - averageResult;
                                                                sumSquaredDeviations += Math.Pow(deviation, 2);
                                                            }
                                                        }

                                                        double variance = sumSquaredDeviations / (countResult - 1);
                                                        double standardDeviation = Math.Sqrt(variance);
                                                        double roundedSD = Math.Round(standardDeviation, 4);
                                                        if (roundedSD > 0)
                                                        {
                                                            spm.SD = roundedSD;
                                                        }
                                                        if (sumSquaredDeviations > 0)
                                                        {
                                                            spm.RSD = (decimal)(roundedSD / spm.Average * 100);
                                                        }
                                                        else
                                                        {
                                                            spm.RSD = 0;
                                                        }
                                                    }

                                                    if (countRec > 0)
                                                    {
                                                        decimal averageRec = (decimal)(sumRec / countRec);
                                                        spm.AvgRec = averageRec;
                                                    }
                                                    ObjectSpace.CommitChanges();
                                                    os.CommitChanges();
                                                    DC.Window.View.ObjectSpace.CommitChanges();
                                                    Oid.Add(spm.Oid);
                                                }
                                            }
                                            //foreach (string strQC in lstQC)
                                            //{
                                            //    IList<SampleParameter> lstSamples = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] =='LCS'  AND  Contains([UQABID.AnalyticalBatchID], ?)", strQC)).ToList();
                                            //    if (objdoC != null && lstSamples.Count > 0)
                                            //    {
                                            //        Samplecheckin lstobj = DC.Window.View.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]= ?", lstSamples.Where(i => !string.IsNullOrEmpty(i.JobID)).Select(i => i.JobID).First()));
                                            //        objdoC.JobID = lstobj;
                                            //        foreach (var qcparam in lstSamples.Where(i => i.DOCDetail == null))
                                            //        {
                                            //            if (qcparam.DOCDetail == null)
                                            //            {
                                            //                List<SampleParameter> lstSamples2 = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] =='LCS'  AND  Contains([UQABID.AnalyticalBatchID], ?)", strQC)).ToList();
                                            //                int columnIndex = 1;
                                            //                DOCDetails objPT = os.CreateObject<DOCDetails>();
                                            //                qcparam.DOCDetail = objPT;
                                            //                foreach (var tparam in lstSamples2)
                                            //                {
                                            //                    if (qcparam.Testparameter.Parameter.ToString() == tparam.Testparameter.Parameter.ToString())
                                            //                    {
                                            //                        var property = objPT.GetType().GetProperty($"Result{columnIndex}");
                                            //                        var property1 = objPT.GetType().GetProperty($"Rec{columnIndex}");
                                            //                        if (property != null)
                                            //                        {
                                            //                            property.SetValue(objPT, tparam.Result);
                                            //                            property1.SetValue(objPT, tparam.Rec);
                                            //                            columnIndex++;
                                            //                            if (columnIndex > 8)
                                            //                            {
                                            //                                break;
                                            //                            }
                                            //                        }
                                            //                    }
                                            //                }
                                            //            }
                                            //        }
                                            //        os.CommitChanges();
                                            //        foreach (var selectedObject in lstSamples)
                                            //        {

                                            //            if (selectedObject is SampleParameter spm)
                                            //            {
                                            //                DOCDetails docDetails = spm.DOCDetail;
                                            //                int countResult = 0;
                                            //                double sumResult = 0;
                                            //                int countRec = 0;
                                            //                double sumRec = 0;
                                            //                for (int i = 1; i <= 8; i++)
                                            //                {
                                            //                    string resultPropertyName = $"Result{i}";
                                            //                    string recPropertyName = $"Rec{i}";
                                            //                    var resultValue = docDetails.GetType().GetProperty(resultPropertyName)?.GetValue(docDetails, null);
                                            //                    var recValue = docDetails.GetType().GetProperty(recPropertyName)?.GetValue(docDetails, null);

                                            //                    if (resultValue != null)
                                            //                    {
                                            //                        countResult++;
                                            //                        sumResult += Convert.ToDouble(resultValue);
                                            //                    }
                                            //                    if (recValue != null)
                                            //                    {
                                            //                        countRec++;
                                            //                        sumRec += Convert.ToDouble(recValue);

                                            //                    }
                                            //                    else if (recValue == null && resultValue != null && spm.SpikeAmount != null)
                                            //                    {
                                            //                        recValue = (Convert.ToDouble(resultValue) / Convert.ToDouble(spm.SpikeAmount) * 100);
                                            //                        countRec++;
                                            //                        sumRec += Convert.ToDouble(recValue);
                                            //                    }
                                            //                }
                                            //                if (countResult > 0)
                                            //                {
                                            //                    double averageResult = sumResult / countResult;
                                            //                    spm.Average = averageResult;
                                            //                    double sumSquaredDeviations = 0;
                                            //                    for (int i = 1; i <= 8; i++)
                                            //                    {
                                            //                        string resultPropertyName = $"Result{i}";
                                            //                        var resultValue = docDetails.GetType().GetProperty(resultPropertyName)?.GetValue(docDetails, null);

                                            //                        if (resultValue != null)
                                            //                        {
                                            //                            double deviation = Convert.ToDouble(resultValue) - averageResult;
                                            //                            sumSquaredDeviations += Math.Pow(deviation, 2);
                                            //                        }
                                            //                    }

                                            //                    double variance = sumSquaredDeviations / (countResult - 1);
                                            //                    double standardDeviation = Math.Sqrt(variance);
                                            //                    double roundedSD = Math.Round(standardDeviation, 4);
                                            //                    if (roundedSD > 0)
                                            //                    {
                                            //                        spm.SD = roundedSD;
                                            //                    }
                                            //                    if (sumSquaredDeviations > 0)
                                            //                    {
                                            //                        spm.RSD = (decimal)(roundedSD / spm.Average * 100);
                                            //                    }
                                            //                    else
                                            //                    {
                                            //                        spm.RSD = 0;
                                            //                    }
                                            //                }

                                            //                if (countRec > 0)
                                            //                {
                                            //                    decimal averageRec = (decimal)(sumRec / countRec);
                                            //                    spm.AvgRec = averageRec;
                                            //                }
                                            //                ObjectSpace.CommitChanges();
                                            //                os.CommitChanges();
                                            //                DC.Window.View.ObjectSpace.CommitChanges();
                                            //                Oid.Add(spm.Oid);
                                            //            }
                                            //        }
                                            //e.ShowViewParameters.CreatedView = DOClv;
                                            //(Frame as DevExpress.ExpressApp.Web.PopupWindow).View.Closed += View_Closed; ;
                                        }
                                        //View.ObjectSpace.Refresh();
                                        //Application.MainWindow.View.Refresh();
                                        //os.Refresh();
                                        //e.Cancel = true;
                                        CollectionSource cs = new CollectionSource(os, typeof(SampleParameter));
                                        //cs.Criteria["Filter"] = new InOperator("Oid", lstSamples.Select(i => i.Oid).Distinct().ToList());
                                        cs.Criteria["Filter"] = new InOperator("Oid", Oid);
                                        ListView DOClv = Application.CreateListView("SampleParameter_ListView_Copy_DOC", cs, false);
                                        e.ShowViewParameters.CreatedView = DOClv;
                                        DialogController dc = Application.CreateController<DialogController>();
                                        dc.Accepting += Dc_Accepting;
                                        dc.ViewClosing += Dc_ViewClosing;
                                        //dc.Cancelling += Dc_Cancelling;
                                        dc.CancelAction.Active.SetItemValue("enb", false);
                                        e.ShowViewParameters.Controllers.Add(dc);
                                        objDocinfo.ViewClose = true;
                                        //ShowViewParameters showViewParameters = new ShowViewParameters();
                                        //showViewParameters.CreatedView = DOClv;
                                        //showViewParameters.Context = TemplateContext.PopupWindow;
                                        //showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                        //DialogController dc = Application.CreateController<DialogController>();
                                        ////ShowViewParameters showViewParameters = new ShowViewParameters();
                                        //dc.CloseOnCurrentObjectProcessing = false;
                                        ////dc.ViewClosing += Dc_ViewClosing;
                                        //dc.Accepting += Dc_Accepting;
                                        //showViewParameters.Controllers.Add(dc);
                                        //Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                        ////(Frame as DevExpress.ExpressApp.Web.PopupWindow).View.Closed += View_Closed; ;
                                        //Application.MainWindow.View.Refresh();
                                    }
                                }
                            }
                            //View.ObjectSpace.Refresh();
                            //}
                        }
                        //else
                        //{
                        //    e.Cancel = true;
                        //    if(objdoC.Matrix == null || objdoC.Test == null || objdoC.Method == null)
                        //    {
                        //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DOC", "choosequery"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        //    }
                        //    else if (string.IsNullOrEmpty(objdoC.QCBatches))
                        //    {
                        //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DOC", "NeedQCBatchID"), InformationType.Info, timer.Seconds, InformationPosition.Top); 
                        //    }
                        //}
                    }
                }
                else if (DC.Window.View.Id == "SampleParameter_ListView_Copy_DOC")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)DC.Window.View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        ASPxGridView grid = gridListEditor.Grid;
                        if (grid != null)
                        {
                            grid.UpdateEdit();
                        }
                    }
                    objDocinfo.ViewClose = true;
                    View.ObjectSpace.Refresh();
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void Dc_Cancelling(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        objDocinfo.ViewClose = true;
        //        View.ObjectSpace.Refresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void Dc_ViewClosing(object sender, EventArgs e)
        {
            try
            {
                //objDocinfo.ViewClose = true;
                //View.ObjectSpace.Refresh();
                //Application.MainWindow.View.ObjectSpace.Refresh();
                //View.Refresh();
                Frame.GetController<RefreshController>().RefreshAction.DoExecute();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void Dc_ViewClosed(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        objDocinfo.ViewClose = true;
        //        View.ObjectSpace.Refresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void View_Closed(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        View.Refresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
        private void DOCCertificate_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string DOCID;
                DOCID = string.Empty;
                Modules.BusinessObjects.Setting.DOC obj = e.CurrentObject as Modules.BusinessObjects.Setting.DOC;
                if (obj.Status != Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingSubmission && obj.Status != Modules.BusinessObjects.Setting.DOC.DOCstatus.Fail)
                {
                    if (View.Id == "DOC_ListView")
                    {
                        if (DOCID == string.Empty)
                        {
                            DOCID = "'" + obj.DOCID.ToString() + "'";
                        }
                        else
                        {
                            if (!DOCID.Contains(obj.DOCID.ToString()))
                                DOCID = DOCID + ",'" + obj.DOCID.ToString() + "'";
                        }
                        //foreach (Modules.BusinessObjects.Setting.DOC obj in View.SelectedObjects)
                        //{
                        //    if (DOCID == string.Empty)
                        //    {
                            //showViewParameters.CreatedView = DOClv;
                            //showViewParameters.Context = TemplateContext.PopupWindow;
                            //showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            //DialogController dc = Application.CreateController<DialogController>();
                            //dc.SaveOnAccept = false;
                            //dc.CloseOnCurrentObjectProcessing = false;
                            //showViewParameters.Controllers.Add(dc);
                            //Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    //    foreach (Modules.BusinessObjects.Setting.DOC obj in View.SelectedObjects)
                    //    {
                    //        if (DOCID == string.Empty)
                    //        {
                    //            DOCID = "'" + obj.DOCID.ToString() + "'";
                    //        }
                    //        else
                    //        {
                    //        if (!DOCID.Contains(obj.DOCID.ToString()))
                    //            DOCID = DOCID + ",'" + obj.DOCID.ToString() + "'";
                    //        }
                    //    }
                    //}
                    //}
                    string strTempPath = Path.GetTempPath();
                    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                    }

                    string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".pdf");
                    XtraReport xtraReport = new XtraReport();
                    DataView dv = new DataView();
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("DOCID");
                    dataTable.Columns.Add("TestName");
                    dataTable.Columns.Add("MethodNumber");
                    dataTable.Columns.Add("MatrixName");
                    dataTable.Columns.Add("SamplePrepType");
                    dataTable.Columns.Add("AnalysisName");
                    dataTable.Columns.Add("PreparedBy");
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                    string serverName = connectionStringBuilder.DataSource.Trim();
                    string databaseName = connectionStringBuilder.InitialCatalog.Trim();
                    string userID = connectionStringBuilder.UserID.Trim();
                    string password = connectionStringBuilder.Password.Trim();
                    //string sqlSelect = "Select * from QAQC_DOCCertificateRPT_SP where [JobID] in(" + DOCID + ")";
                    string sqlSelect = "exec QAQC_DOCCertificateRPT_SP @DOCID = " + DOCID + "";
                    SqlConnection sqlConnection = new SqlConnection(connectionStringBuilder.ToString());
                    SqlCommand sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand);
                    sqlDa.Fill(dataTable);
                    String strProjectName = ConfigurationManager.AppSettings["ProjectName"];
                    if (strProjectName == "Consci")
                    {


                        DOCCertificate sampleLabelReport1 = new DOCCertificate();
                        sampleLabelReport1.DataSource = dataTable;
                        xtraReport = sampleLabelReport1;
                        sampleLabelReport1.DataBindingsToReport();

                    }
                    else
                    {
                        DOCCertificate sampleLabelReport = new DOCCertificate();
                        sampleLabelReport.DataSource = dataTable;
                        xtraReport = sampleLabelReport;
                        sampleLabelReport.DataBindingsToReport();
                    }

                    //FolderLabelReport sampleLabelReport = new FolderLabelReport();
                    //sampleLabelReport.DataSource = dataTable;
                    //xtraReport = sampleLabelReport;
                    //sampleLabelReport.DataBindingsToReport();
                    if (obj.Status == Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingValidation)
                    {
                        xtraReport.Watermark.Text = "NOT APPROVED";
                        xtraReport.Watermark.ForeColor = Color.SlateGray;
                        xtraReport.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                        xtraReport.Watermark.TextTransparency = 200;
                        xtraReport.Watermark.ShowBehind = true;
                    }
                    xtraReport.ExportToPdf(strExportedPath);
                    string[] path = strExportedPath.Split('\\');
                    int arrcount = path.Count();
                    int sc = arrcount - 2;
                    string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                    WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                    //if (View.SelectedObjects.Count > 0)
                    //{

                    //}
                    //else
                    //{
                    //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    //}
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DOC", "CertificateAvailable"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
                
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DOCReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                    string DOCID;
                    DOCID = string.Empty;
                    Modules.BusinessObjects.Setting.DOC obj = e.CurrentObject as Modules.BusinessObjects.Setting.DOC;
                if (obj.Status != Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingSubmission && obj.Status != Modules.BusinessObjects.Setting.DOC.DOCstatus.Fail)
                {
                    IList<SampleParameter> lstSamples1 = null;
                    if (obj.QCBatches!=null)
                    {
                    List<string> lstQC = obj.QCBatches.Split(';').ToList();
                    if (obj.IsQCBatchID)
                    {
                        lstSamples1 = ObjectSpace.GetObjects<SampleParameter>((new GroupOperator(GroupOperatorType.And, new InOperator("UQABID.AnalyticalBatchID", lstQC), CriteriaOperator.Parse("[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] == 'LCS'"))));
                    }
                    else
                    {
                        lstSamples1 = ObjectSpace.GetObjects<SampleParameter>((new GroupOperator(GroupOperatorType.And, new InOperator("UQABID.AnalyticalBatchID", lstQC), CriteriaOperator.Parse("[Samplelogin] Is Not Null And [Samplelogin.QCCategory] Is Not Null And ([Samplelogin.QCCategory.QCCategoryName] =='DOC' OR [Samplelogin.QCCategory.QCCategoryName] =='MDL' OR [Samplelogin.QCCategory.QCCategoryName] =='PT') AND [Status] = 'PendingReporting'"))));
                    }
                    }
                   
                    if (View.Id == "DOC_ListView")
                    {
                        if (DOCID == string.Empty)
                        {
                            DOCID = "'" + obj.DOCID.ToString() + "'";
                        }
                        else
                        {
                            if (!DOCID.Contains(obj.DOCID.ToString()))
                                DOCID = DOCID + ",'" + obj.DOCID.ToString() + "'";
                        }
                    }

                    string strTempPath = Path.GetTempPath();
                    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                    }

                    string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".pdf");
                    XtraReport xtraReport = new XtraReport();
                    DataView dv = new DataView();
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("DOCID");
                    dataTable.Columns.Add("TestName");
                    dataTable.Columns.Add("MethodNumber");
                    dataTable.Columns.Add("MatrixName");
                    dataTable.Columns.Add("SamplePrepType");
                    dataTable.Columns.Add("AnalysisName");
                    dataTable.Columns.Add("PreparedBy");
                    dataTable.Columns.Add("DatePrepared");
                    dataTable.Columns.Add("PrepBatchID");
                    dataTable.Columns.Add("QCBatchID");
                    dataTable.Columns.Add("JobID");
                    dataTable.Columns.Add("SpikeAmount");
                    dataTable.Columns.Add("SpikePrepInfo");
                    dataTable.Columns.Add("SpikeConcentration");
                    dataTable.Columns.Add("SpikeStandardID");
                    dataTable.Columns.Add("SpikeStandardName");
                    dataTable.Columns.Add("SpikeUnits");
                    dataTable.Columns.Add("PrepInstrument");
                    dataTable.Columns.Add("DateAnalyzed");
                    dataTable.Columns.Add("Parameter");
                    dataTable.Columns.Add("SPKAmt");
                    dataTable.Columns.Add("Result1");
                    dataTable.Columns.Add("Result2");
                    dataTable.Columns.Add("Result3");
                    dataTable.Columns.Add("Result4");
                    dataTable.Columns.Add("Result5");
                    dataTable.Columns.Add("Result6");
                    dataTable.Columns.Add("Result7");
                    dataTable.Columns.Add("Average");
                    dataTable.Columns.Add("%RecAve");
                    dataTable.Columns.Add("SD");
                    dataTable.Columns.Add("RSD");
                    dataTable.Columns.Add("MDL");
                    dataTable.Columns.Add("MDL1");
                    dataTable.Columns.Add("RptLimit");
                    dataTable.Columns.Add("Ratio");
                    dataTable.Columns.Add("QCInstruments");
                    dataTable.Columns.Add("DateApproved");
                    dataTable.Columns.Add("ApprovedBy");
                    dataTable.Columns.Add("Status");
                    dataTable.Columns.Add("LLimit");
                    dataTable.Columns.Add("HLimit");
                    dataTable.Columns.Add("MDLCheck");
                    dataTable.Columns.Add("Comments");
                    dataTable.Columns.Add("PrepMethod");
                    dataTable.Columns.Add("TCLPPrepMethod");
                    dataTable.Columns.Add("TCLPPreparedby");
                    dataTable.Columns.Add("DateTCLPPrepared");
                    dataTable.Columns.Add("AnalyzedBy");
                    dataTable.Columns.Add("AnalyticalInstrument");
                    IList<string> param = new List<string>();
                    if (lstSamples1 != null)
                    {
                    foreach (SampleParameter spm in lstSamples1)
                    {
                        if (spm != null && spm.QCBatchID != null && spm.QCBatchID.QCType != null && (spm.Samplelogin != null && spm.Samplelogin.QCCategory != null && spm.Samplelogin.QCCategory.QCCategoryName == "DOC" || spm.QCBatchID.QCType.QCTypeName == "LCS") && !param.Contains(spm.Testparameter.Parameter.ParameterName))
                        {
                            param.Add(spm.Testparameter.Parameter.ParameterName);
                            DataRow dr = dataTable.NewRow();
                            dr["DOCID"] = obj.DOCID;
                            if (obj.Test != null)
                            {
                                //dr["TestName"] = ObjectSpace.GetObjectByKey<TestMethod>(obj.Test.Oid);
                                dr["TestName"] = obj.Test.TestName;
                            }
                            if (obj.Method != null)
                            {
                                dr["MethodNumber"] = obj.Method.MethodNumber;
                            }
                            if (obj.Matrix != null)
                            {
                                dr["MatrixName"] = obj.Matrix.MatrixName;
                            }
                            if (obj.Analyst != null)
                            {
                                dr["AnalysisName"] = obj.Analyst.DisplayName;
                                //objPT.DateAnalyzed = objSampleResult.AnalyzedDate;
                            }
                            //dr["TestName"] = obj.Test;
                            //dr["MethodNumber"] = obj.Method;
                            //dr["TestName"] = ObjectSpace.GetObjectByKey<TestMethod>(obj.Test.Oid);
                            //objPT.DateAnalyzed = objSampleResult.AnalyzedDate;
                                if (spm.UQABID!=null)
                                {
                            dr["QCBatchID"] = spm.UQABID.AnalyticalBatchID;
                                }
                            dr["JobID"] = obj.strJobID;
                            dr["SpikeAmount"] = obj.SpikeAmount;
                            dr["SpikeUnits"] = obj.SpikeUnits;
                            //if (spm.SpikeAmountUnit != null)
                            //{
                            //    dr["SpikeUnits"] = spm.SpikeAmountUnit.UnitName;
                            //}
                            dr["DateAnalyzed"] = obj.DateAnalyzed;
                            dr["SamplePrepType"] = spm.PrepBatchID;
                            //dr["SpikePrepInfo"] = obj;
                            dr["SpikeConcentration"] = obj.StandardConcentration;
                            dr["SpikeStandardID"] = obj.StandardID;
                            dr["SpikeStandardName"] = obj.StandardName;
                            dr["PrepInstrument"] = obj.PrepInstrument;
                                if (spm.Testparameter!=null&& spm.Testparameter.Parameter!=null)
                                {
                            dr["Parameter"] = spm.Testparameter.Parameter.ParameterName;
                                }
                            dr["SPKAmt"] = spm.SpikeAmount;
                            if (obj.PreparedBy != null)
                            {
                                dr["PreparedBy"] = obj.PreparedBy.DisplayName;

                            }
                            dr["DatePrepared"] = obj.DatePrepared;
                            dr["PrepBatchID"] = obj.PrepBatchID;
                            //if (spm.PrepBatchID != null)
                            //{
                            //    List<string> lst = spm.PrepBatchID.Split(';').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                            //    if (lst.Contains(obj.Oid.ToString()))
                            //    {
                            //        lst.Add(obj.Oid.ToString());
                            //    }
                            //    dr["PrepBatchID"] = lst.FirstOrDefault();

                            //}
                            Modules.BusinessObjects.Setting.DOCDetails objDetails = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.DOCDetails>(CriteriaOperator.Parse("[Oid]= ?", spm.DOCDetail.Oid));

                            if (objDetails != null)
                            {
                                dr["Result1"] = objDetails.Result1;
                                dr["Result2"] = objDetails.Result2;
                                dr["Result3"] = objDetails.Result3;
                                dr["Result4"] = objDetails.Result4;
                                dr["Result5"] = objDetails.Result5;
                                dr["Result6"] = objDetails.Result6;
                                dr["Result7"] = objDetails.Result7;
                            }
                            dr["Average"] = spm.Average;
                            dr["%RecAve"] = spm.AvgRec;
                            dr["SD"] = spm.SD;
                            dr["RSD"] = spm.RSD;
                            //dr["MDL"] = obj;
                            //dr["MDL1"] = obj;
                            dr["RptLimit"] = spm.RptLimit;
                            //dr["Ratio"] = spm;
                            if (spm.UQABID != null && spm.UQABID.Instrument != null)
                            {
                                //List<string> lst = spm.UQABID.Instrument.Split(';').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                                //if (lst.Contains(spm.UQABID.Instrument.ToString()))
                                //{
                                //    dr["QCInstruments"] = os.GetObjectByKey<Labware>(spm.UQABID.Instrument);
                                //    lst.Add(spm.UQABID.Instrument.ToString());
                                //}
                                //dr["QCInstruments"] = lst.FirstOrDefault();

                            }
                                if (spm.UQABID!=null)
                                {
                            dr["QCInstruments"] = spm.UQABID.strInstrument;
                                }
                            dr["DateApproved"] = spm.ApprovedDate;
                                if (spm.ApprovedBy!=null)
                                {
                            dr["ApprovedBy"] = spm.ApprovedBy.DisplayName;
                                }
                            //dr["Status"] = obj;
                                if (spm.Testparameter!=null)
                                {
                            dr["LLimit"] = spm.Testparameter.RPDLCLimit;
                            dr["HLimit"] = spm.Testparameter.RPDHCLimit;
                                }
                            //dr["MDLCheck"] = obj;
                            dr["Comments"] = obj.Comment;
                            dr["PrepMethod"] = obj.PrepMethod;
                            if (spm.UQABID!=null)
                            {
                                dr["AnalyticalInstrument"] = spm.UQABID.Instrument; 
                            }
                            dr["DateTCLPPrepared"] = obj.DateTCLPPreped;
                            dr["TCLPPrepMethod"] = obj.TCLPPrepedMethod;
                            if (obj.TCLPPrepedBy!=null)
                            {
                                dr["TCLPPreparedby"] = obj.TCLPPrepedBy.DisplayName; 
                            }
                            dataTable.Rows.Add(dr);
                            String strProjectName = ConfigurationManager.AppSettings["ProjectName"];
                            if (strProjectName == "Consci")
                            {
                                XrDOCReport sampleLabelReport1 = new XrDOCReport();
                                sampleLabelReport1.DataSource = dataTable;
                                xtraReport = sampleLabelReport1;
                                sampleLabelReport1.DataBindingsToReport();
                            }
                            else
                            {
                                XrDOCReport sampleLabelReport = new XrDOCReport();
                                sampleLabelReport.DataSource = dataTable;
                                xtraReport = sampleLabelReport;
                                sampleLabelReport.DataBindingsToReport();
                            }
                            xtraReport.ExportToPdf(strExportedPath);
                            string[] path = strExportedPath.Split('\\');
                            int arrcount = path.Count();
                            int sc = arrcount - 2;
                            string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                            WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));

                        }
                        else if (spm != null && spm.Samplelogin != null && spm.Samplelogin.QCCategory != null && spm.Samplelogin.QCCategory.QCCategoryName == "MDL"&& !param.Contains(spm.Testparameter.Parameter.ParameterName))
                        {
                            param.Add(spm.Testparameter.Parameter.ParameterName);
                            //List<string> lst = spm.UQABID.Instrument.Split(';').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                            //if (lst.Contains(spm.UQABID.Instrument.ToString()))
                            //{
                            //    dr["QCInstruments"] = os.GetObjectByKey<Labware>(spm.UQABID.Instrument);
                            //    lst.Add(spm.UQABID.Instrument.ToString());
                            //}
                            //dr["QCInstruments"] = lst.FirstOrDefault();
                            //dataTable.Columns.Add("DatePrepared");
                            //dataTable.Columns.Add("TCLPPrepMethod");
                            //dataTable.Columns.Add("TCLPPrepedBy");
                            //dataTable.Columns.Add("DateTCLPPrepared");
                            //dataTable.Columns.Add("DateAnalyzed");
                            //dataTable.Columns.Add("AnalyzedBy");
                            //dataTable.Columns.Add("QCInstruments");
                            //dataTable.Columns.Add("JobID");
                            //dataTable.Columns.Add("SpikeUnits");
                            //dataTable.Columns.Add("SpikeConcentration");
                            //dataTable.Columns.Add("SpikePrepInfo");
                            //dataTable.Columns.Add("SpikeStandardID");
                            //dataTable.Columns.Add("Parameter");
                            //dataTable.Columns.Add("SPKAmt");
                            //dataTable.Columns.Add("Result1");
                            //dataTable.Columns.Add("Result2");
                            //dataTable.Columns.Add("Result3");
                            //dataTable.Columns.Add("Result4");
                            //dataTable.Columns.Add("Result5");
                            //dataTable.Columns.Add("Result6");
                            //dataTable.Columns.Add("Result7");
                            //dataTable.Columns.Add("Average");
                            //dataTable.Columns.Add("%RecAve");
                            //dataTable.Columns.Add("SD");
                            //dataTable.Columns.Add("RSD");
                            //dataTable.Columns.Add("MDL");
                            //dataTable.Columns.Add("MDL1");
                            //dataTable.Columns.Add("MDLCheck");
                            //dataTable.Columns.Add("Ratio");
                            //dataTable.Columns.Add("RptLimit");
                            //dataTable.Columns.Add("DateApproved");
                            //dataTable.Columns.Add("Comments");
                            //dataTable.Columns.Add("LLimit");
                            //dataTable.Columns.Add("HLimit");
                            DataRow dr = dataTable.NewRow();
                            dr["DOCID"] = obj.DOCID;

                            if (obj.Test != null)
                            {
                                dr["TestName"] = obj.Test.TestName;
                            }
                            if (obj.Method != null)
                            {
                                dr["MethodNumber"] = obj.Method.MethodNumber;
                                }
                            if (obj.Matrix != null)
                            {
                                dr["MatrixName"] = obj.Matrix.MatrixName;
                            }  
                            dr["PrepMethod"] = obj.PrepMethod;

                            dr["PrepInstrument"] = obj.PrepInstrument;

                            if (obj.PreparedBy != null)
                            {
                                dr["PreparedBy"] = obj.PreparedBy.DisplayName;

                        }
                            dr["DatePrepared"] = obj.DatePrepared;
                            dr["DateTCLPPrepared"] = obj.DateTCLPPreped;
                            dr["TCLPPrepMethod"] = obj.TCLPPrepedMethod;
                            if (obj.TCLPPrepedBy != null)
                            {
                                dr["TCLPPreparedby"] = obj.TCLPPrepedBy.DisplayName;
                            }

                            if (obj.Analyst != null)
                            {
                                dr["AnalyzedBy"] = obj.Analyst.DisplayName;
                            }
                            dr["DateAnalyzed"] = obj.DateAnalyzed;
                                if (spm.UQABID!=null)
                                {
                            dr["QCInstruments"] = spm.UQABID.Instrument;
                                }
                            dr["JobID"] = obj.JobID.JobID;
                            if (spm.SpikeAmountUnit != null)
                            {
                                dr["SpikeUnits"] = spm.SpikeAmountUnit.UnitName;
                            }
                            if (obj.SpikeUnits != null)
                            {
                                dr["SpikeUnits"] = obj.SpikeUnits;
                            }
                            dr["SpikeConcentration"] = obj.StandardConcentration;
                            //dr["SpikePrepInfo"] = spm.SpikeAmount;
                            dr["SpikeStandardID"] = obj.StandardID;
                                if (spm.Testparameter!=null&& spm.Testparameter.Parameter!=null)
                                {
                            dr["Parameter"] = spm.Testparameter.Parameter.ParameterName;
                                }
                            dr["SPKAmt"] = spm.SpikeAmount;
                            Modules.BusinessObjects.Setting.DOCDetails objDetails = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.DOCDetails>(CriteriaOperator.Parse("[Oid]= ?", spm.DOCDetail.Oid));

                            if (objDetails != null)
                            {
                                dr["Result1"] = objDetails.Result1;
                                dr["Result2"] = objDetails.Result2;
                                dr["Result3"] = objDetails.Result3;
                                dr["Result4"] = objDetails.Result4;
                                dr["Result5"] = objDetails.Result5;
                                dr["Result6"] = objDetails.Result6;
                                dr["Result7"] = objDetails.Result7;
                            }
                            dr["Average"] = spm.Average;
                            dr["%RecAve"] = spm.AvgRec;
                            dr["SD"] = spm.SD;
                            dr["RSD"] = spm.RSD;
                            dr["MDL"] = spm.MDL;
                            dr["MDL1"] = spm.MDL;
                            dr["MDLCheck"] = spm.MDL;
                            //dr["Ratio"] = spm.Testparamete;
                            dr["RptLimit"] = spm.RptLimit;
                            dr["DateApproved"] = spm.ApprovedDate;
                            dr["LLimit"] = spm.RPDLCLimit;
                            dr["HLimit"] = spm.RPDHCLimit;
                            dr["Comments"] = obj.Comment;
                                if (spm.UQABID!=null)
                                {
                            dr["QCInstruments"] = spm.UQABID.strInstrument;
                                }
                            dataTable.Rows.Add(dr);

                            String strProjectName = ConfigurationManager.AppSettings["ProjectName"];
                            if (strProjectName == "Consci")
                            {
                                XrMDLReport sampleLabelReport1 = new XrMDLReport();
                                sampleLabelReport1.DataSource = dataTable;
                                xtraReport = sampleLabelReport1;
                                sampleLabelReport1.MDLDataBindingsToReport();
                    }
                    else
                    {
                                XrMDLReport sampleLabelReport = new XrMDLReport();
                                sampleLabelReport.DataSource = dataTable;
                                xtraReport = sampleLabelReport;
                                sampleLabelReport.MDLDataBindingsToReport();
                            }
                            xtraReport.ExportToPdf(strExportedPath);
                            string[] path = strExportedPath.Split('\\');
                            int arrcount = path.Count();
                            int sc = arrcount - 2;
                            string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                            WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));

                        }
                    } 

                    }
                    //string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    //var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                    //string serverName = connectionStringBuilder.DataSource.Trim();
                    //string databaseName = connectionStringBuilder.InitialCatalog.Trim();
                    //string userID = connectionStringBuilder.UserID.Trim();
                    //string password = connectionStringBuilder.Password.Trim();
                    ////string sqlSelect = "Select * from QAQC_DOCCertificateRPT_SP where [JobID] in(" + DOCID + ")";
                    //string sqlSelect = "exec QAQC_DOCCertificateRPT_SP @DOCID = " + DOCID + "";
                    //SqlConnection sqlConnection = new SqlConnection(connectionStringBuilder.ToString());
                    //SqlCommand sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                    //SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand);
                    //sqlDa.Fill(dataTable);
                    //String strProjectName = ConfigurationManager.AppSettings["ProjectName"];
                    //if (strProjectName == "Consci")
                    //{


                    //    XrDOCReport sampleLabelReport1 = new XrDOCReport();
                    //    sampleLabelReport1.DataSource = dataTable;
                    //    xtraReport = sampleLabelReport1;
                    //    sampleLabelReport1.DataBindingsToReport();

                    //}
                    //else
                    //{
                    //    XrDOCReport sampleLabelReport = new XrDOCReport();
                    //    sampleLabelReport.DataSource = dataTable;
                    //    xtraReport = sampleLabelReport;
                    //    sampleLabelReport.DataBindingsToReport();
                    //}
                    //xtraReport.ExportToPdf(strExportedPath);
                    //string[] path = strExportedPath.Split('\\');
                    //int arrcount = path.Count();
                    //int sc = arrcount - 2;
                    //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                    //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DOC", "ReportAvailable"), InformationType.Info, timer.Seconds, InformationPosition.Top);
            }
                
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        //private void CurrentRequestPage_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (objDocinfo.ViewClose)
        //        {
        //            objDocinfo.ViewClose = false;
        //            (Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
        private void DOCRollBack_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && (View.SelectedObjects.Count > 0 || View.CurrentObject != null))
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(Modules.BusinessObjects.Setting.DOC));
                    Modules.BusinessObjects.Setting.DOC obj = os.GetObject(e.CurrentObject as Modules.BusinessObjects.Setting.DOC);
                    obj.RollBackReason = null;
                    
                    DetailView createdView = Application.CreateDetailView(os, "DOC_DetailView_RollBack_Reason", true, obj);
                    createdView.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    //dc.SaveOnAccept = false;
                    dc.Accepting += DOCRollBack_Accepting;
                    //dc.ViewClosed += View_Closed;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DOCRollBack_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.Setting.DOC objDOC = e.AcceptActionArgs.CurrentObject as Modules.BusinessObjects.Setting.DOC;
                if (objDOC!=null && !string.IsNullOrEmpty(objDOC.RollBackReason))
                {
                    DialogController DC = sender as DialogController;
                    if (DC!=null)
                    {
                        objDOC.RollBackBy = DC.Frame.View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        objDOC.RollBackDate = DateTime.Now;
                        objDOC.Status = Modules.BusinessObjects.Setting.DOC.DOCstatus.PendingSubmission; 
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        //View.Refresh();
                        View.ObjectSpace.Refresh();
                    }
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreason"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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
