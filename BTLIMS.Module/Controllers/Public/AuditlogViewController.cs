using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.SamplingManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.TaskManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PopupWindow = DevExpress.ExpressApp.Web.PopupWindow;

namespace LDM.Module.Controllers.Public
{
    public partial class AuditlogViewController : ViewController, IXafCallbackHandler
    {
        NavigationInfo objNavInfo = new NavigationInfo();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        MessageTimer timer = new MessageTimer();
        AuditInfo objAuditInfo = new AuditInfo();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        ReportingQueryPanelInfo objRQPInfo = new ReportingQueryPanelInfo();
        private object lastProcessedObject = null;
        private string lastProcessedPropertyName = null;
        public bool EditCancel;

        //AuditInfo objAuditInfo = new AuditInfo();

        TestInfo objtestinfo = new TestInfo();
        public AuditlogViewController()
        {
            InitializeComponent();
            Audit.TargetViewId = "Samplecheckin_DetailView_Copy_SampleRegistration;"
                + "Reporting_ListView_Copy_ReportView;"
                + "SamplingProposal_DetailView;"
                + "Samplecheckin_DetailView_FieldDataEntry_History;"
                + "TaskJobIDAutomation_ListView;"
                + "TestMethod_DetailView;"
                + "SampleBottleAllocation_DetailView_SampleTransfer;"
                + "SampleSites_DetailView;"
                + "SamplePrepBatch_DetailView_Copy_History;";
        }

        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                ModificationsController controller = Frame.GetController<ModificationsController>();
                if (controller != null)
                {
                    controller.SaveAction.Executing += SaveAction_Executing;
                    controller.SaveAndCloseAction.Executing += SaveAction_Executing;
                    controller.SaveAndNewAction.Executing += SaveAction_Executing;
                }
                Frame.GetController<LinkUnlinkController>().UnlinkAction.Execute += UnlinkAction_Execute; 
                Frame.GetController<LinkUnlinkController>().LinkAction.Execute += LinkAction_Execute;
                //LinkUnlinkController linkUnlink = Frame.GetController<LinkUnlinkController>();
                //if (linkUnlink != null )
                //{
                //    linkUnlink.UnlinkAction.Execute += UnlinkAction_Execute;
                //}
                //else
                //{
                //    linkUnlink.LinkAction.Execute += LinkAction_Execute;
                //}
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                ObjectSpace.Committed += ObjectSpace_Committed;
                ObjectSpace.ObjectDeleting += ObjectSpace_ObjectDeleting;
              
                if (View.Id == "AuditData_DetailView")
                {
                    View.Closing += View_Closing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        void View_Closing1(object sender, CancelEventArgs e)
        {
            
        }
        //private void Grid_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        //{

        //    IList<AuditData> objAudit = View.ObjectSpace.GetObjects<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
        //    if (objAudit.Count>0 )
        //    {
        //        getcomments(View.ObjectSpace, objAudit.First());

        //    }
        //}



        private void LinkAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
               if (e.PopupWindowViewSelectedObjects.Count > 0)
               {
                    if (objNavInfo.SelectedNavigationCaption == "Tests")
                    {
                        foreach (var objs in e.PopupWindowViewSelectedObjects)
                        {
                            if (objs.GetType() == typeof(QCType))
                            {
                                TestMethod samplecheckin = SRInfo.CurrentTest;
                                QCType qCType = (QCType)objs;

                                if (samplecheckin != null)
                                {
                                    createaudit(ObjectSpace, samplecheckin.Oid, samplecheckin.TestCode, "QCTypes", qCType.QCTypeName);
                                }
                            }
                            if (objs.GetType() == typeof(Method))
                            {
                                TestMethod samplecheckin = SRInfo.CurrentTest;
                                Method method = (Method)objs;

                                if (samplecheckin != null)
                                {
                                    createaudit(ObjectSpace, samplecheckin.Oid, samplecheckin.TestCode, "Method", method.MethodName);
                                }
                            }
                        }
                    }
                    if (objNavInfo.SelectedNavigationCaption == "Sample Prep Batches")
                    {
                        foreach (var objs in e.PopupWindowViewSelectedObjects)
                        {
                            if (objs.GetType() == typeof(Reagent))
                            {
                                SamplePrepBatch samplePrepBatch = SRInfo.currentPrepbatchID;
                                Reagent objReagent = (Reagent)objs;

                                if (samplePrepBatch != null)
                                {
                                    createaudit(ObjectSpace, samplePrepBatch.Oid, samplePrepBatch.PrepBatchID, "Reagent", objReagent.ReagentName);
                                }
                            }
                            if (objs.GetType() == typeof(Labware))
                            {
                                SamplePrepBatch samplePrepBatch = SRInfo.currentPrepbatchID;
                                Labware objLabware = (Labware)objs;

                                if (samplePrepBatch != null)
                                {
                                    createaudit(ObjectSpace, samplePrepBatch.Oid, samplePrepBatch.PrepBatchID, "Labware", objLabware.FullName);
                                }
                            }
                        }
                    }

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        private void UnlinkAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                var Objects = e.SelectedObjects;
                if (Objects.Count > 0)
                {
                    if (objNavInfo.SelectedNavigationCaption == "Sample Registration")
                    {
                        foreach (var objs in Objects)
                        {
                            if (objs.GetType() == typeof(Image))
                            {
                                Samplecheckin samplecheckin = SRInfo.CurrentJob;
                                if (samplecheckin != null && samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                                {
                                    createdeleteaudit(ObjectSpace, samplecheckin.Oid, samplecheckin.JobID, "Image", objs.ToString());
                                }
                            }
                        }
                    }
                    if (objNavInfo.SelectedNavigationCaption == "Tests")
                    {
                        foreach (var objs in Objects)
                        {
                            if (objs.GetType() == typeof(QCType))
                            {
                                TestMethod samplecheckin = SRInfo.CurrentTest;
                                QCType qCType = (QCType)objs;

                                if (samplecheckin != null )
                                {
                                    createdeleteaudit(ObjectSpace, samplecheckin.Oid, samplecheckin.TestCode, "QCTypes", qCType.QCTypeName);
                                }
                            }

                            if (objs.GetType() == typeof(Method))
                            {
                                TestMethod samplecheckin = SRInfo.CurrentTest;
                                Method method = (Method)objs;

                                if (samplecheckin != null)
                                {
                                    createdeleteaudit(ObjectSpace, samplecheckin.Oid, samplecheckin.TestCode, "Method", method.MethodName);
                                }
                            }
                        }
                    }

                    if (objNavInfo.SelectedNavigationCaption == "Sample Prep Batches")
                    {
                        foreach (var objs in Objects)
                        {
                            if (objs.GetType() == typeof(Reagent))
                            {
                                SamplePrepBatch samplePrepBatch = SRInfo.currentPrepbatchID;
                                Reagent objReagent = (Reagent)objs;

                                if (samplePrepBatch != null)
                                {
                                    createdeleteaudit(ObjectSpace, samplePrepBatch.Oid, samplePrepBatch.PrepBatchID, "Reagent", objReagent.ReagentName);
                                }
                            }
                            if (objs.GetType() == typeof(Labware))
                            {
                                SamplePrepBatch samplePrepBatch = SRInfo.currentPrepbatchID;
                                Labware objLabware = (Labware)objs;

                                if (samplePrepBatch != null)
                                {
                                    createdeleteaudit(ObjectSpace, samplePrepBatch.Oid, samplePrepBatch.PrepBatchID, "Labware", objLabware.FullName);
                                }
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void commentdialog(object sender, AuditData data)
        {
            try
            {
                if (objAuditInfo.SaveData == null)
                {
                    objAuditInfo.SaveData = false;
                    objAuditInfo.action = (SimpleAction)sender;
                    getcomments(View.ObjectSpace, data);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void commentdialog(object sender, System.ComponentModel.CancelEventArgs e, AuditData data)
        
        {
            try
            {
                e.Cancel = true;
                if (objAuditInfo.SaveData == null)
                {
                    objAuditInfo.SaveData = false;
                    if (sender.GetType()==typeof(SimpleAction))
                    {
                        objAuditInfo.action = (SimpleAction)sender; 
                    }
                    else if (sender.GetType() == typeof(SingleChoiceAction))
                    {
                        objAuditInfo.choiceaction = (SingleChoiceAction)sender;  
                    }

                    getcomments(View.ObjectSpace, data);


                
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public bool processnav(CustomShowNavigationItemEventArgs e)
        {
            try
            {
                if (ObjectSpace != null && objAuditInfo.SaveData != true && string.IsNullOrEmpty(objAuditInfo.comment))
                {
                    IList<AuditData> Objects = ObjectSpace.GetObjects<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId));
                    if (Objects.Count() > 0)
                    {
                        if (objAuditInfo.SaveData == null)
                        {
                            objAuditInfo.SaveData = false;
                            objAuditInfo.choiceaction = (SingleChoiceAction)e.ActionArguments.Action;
                            objAuditInfo.choiceactionitem = e.ActionArguments.SelectedChoiceActionItem;
                            if (objtestinfo.isTestsave == false)
                            {
                            getcomments(View.ObjectSpace, Objects.First());
                            }
                            e.Handled = true;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return false;
            }
        }

        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (objAuditInfo.SaveData != true && string.IsNullOrEmpty(objAuditInfo.comment) && Frame is PopupWindow is false && ObjectSpace.IsModified /*&& ObjectSpace.ModifiedObjects.Count >0*/ && e.Cancel==false)
                {
                    IList<AuditData> Objects = ObjectSpace.GetObjects<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                    if (Objects.Count() > 0)
                        {
                            commentdialog(sender, e, Objects.First());
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void getcomments(IObjectSpace os, AuditData obj, DevExpress.ExpressApp.Editors.ViewEditMode mode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit)
        {
            try
            {
                DetailView dv = Application.CreateDetailView(os, "AuditData_DetailView", false, obj);
                dv.Caption = "Audit Trail Comment";
                dv.ViewEditMode = mode;
                ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.CloseOnCurrentObjectProcessing = false;
                //dc.CloseOnCurrentObjectProcessing = false;
                //dc.CloseAction.Active.SetItemValue("enable", false);
                if (mode == DevExpress.ExpressApp.Editors.ViewEditMode.View)
                {
                    dc.AcceptAction.Active.SetItemValue("enb", false);
                    dc.CancelAction.Active.SetItemValue("enb", false);
                }
                else
                {
                    dc.Accepting += Dc_Accepting;
                    dc.Cancelling += Dc_Cancelling;
                }
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Cancelling(object sender, EventArgs e)
        {
            try
            {
                if (objAuditInfo.SaveData != true)
                {
                    objAuditInfo.comment = string.Empty;
                    objAuditInfo.SaveData = null;
                    objAuditInfo.action = null;
                    objAuditInfo.choiceaction = null;
                    objAuditInfo.choiceactionitem = null;
                    Application.ShowViewStrategy.ShowMessage("Unsaved changes exist.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void View_Closing(object sender, EventArgs e)
        {
            try
            {
                if (objAuditInfo.SaveData != true && ((DetailView)View).ViewEditMode == DevExpress.ExpressApp.Editors.ViewEditMode.Edit)
                {
                    objAuditInfo.comment = string.Empty;
                    objAuditInfo.SaveData = null;
                    objAuditInfo.action = null;
                    objAuditInfo.choiceaction = null;
                    objAuditInfo.choiceactionitem = null;
                    Application.ShowViewStrategy.ShowMessage("Unsaved changes exist.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
                else if (objAuditInfo.SaveData == true)
                {
                    objAuditInfo.SaveData = null;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void clearaudit()
        {
            try
            {
                objAuditInfo.action = null;
                objAuditInfo.Auditedlist = null;
                objAuditInfo.comment = string.Empty;
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
                auditaccepting(e);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void auditaccepting(DialogControllerAcceptingEventArgs e)
        {
            try
            {
                AuditData data = (AuditData)e.AcceptActionArgs.CurrentObject;
                if (data != null)
                {
                    if (string.IsNullOrEmpty(data.Comment))
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Enter the comment.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        IList<AuditData> Objects = ObjectSpace.GetObjects<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                        if (Objects.Count() > 0)
                        {
                            foreach (AuditData audit in Objects.Where(a => a.IsDeleted == false).ToList())
                            {
                                if (audit.Source == Guid.Empty)
                                {
                                    objAuditInfo.comment = data.Comment;
                                    View.ObjectSpace.Delete(audit);
                                }
                                else
                                {
                                    audit.CommentProcessed = true;
                                    audit.Comment = data.Comment;
                                }
                            }
                            objAuditInfo.SaveData = true;
                            if (objAuditInfo.action != null)
                            {
                                if (objAuditInfo.action.Id == "STDelete")
                                {
                                    ObjectSpace.CommitChanges();
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    Frame.SetView(Application.CreateListView("SampleBottleAllocation_ListView_SampleTransferMain", new CollectionSource(Application.CreateObjectSpace(), typeof(SampleBottleAllocation)), true));
                                }
                                                                else if (objAuditInfo.action.Id == "STRollback")
{
                                    ObjectSpace.CommitChanges();
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    Frame.SetView(Application.CreateListView("SampleBottleAllocation_ListView_SampleTransferMain_History", new CollectionSource(Application.CreateObjectSpace(), typeof(SampleBottleAllocation)), true));
                                }
                                else
                                {
                                objAuditInfo.action.DoExecute();
                            }
                            }
                            else if (objAuditInfo.choiceaction != null)
                            {
                                ObjectSpace.CommitChanges();
                                objAuditInfo.choiceaction.DoExecute(objAuditInfo.choiceactionitem);
                            }
                            else if (objnavigationRefresh.ClickedNavigationItem == "FieldDataEntry" && Application.MainWindow.View.Id.Contains("History"))
                            {
                                ObjectSpace.CommitChanges();
                            }
                            objAuditInfo.choiceaction = null;
                            objAuditInfo.choiceactionitem = null;
                        }
                    }
                    //if ( View is ListView)
                    //{
                    //    SampleParameter objReporting = View.ObjectSpace.GetObjectByKey<SampleParameter>(((AuditData)View.CurrentObject).Source);
                    //    if (objReporting != null)
                    //    {
                    //        Application.ShowViewStrategy.ShowMessage("Report " + objReporting.Reportings. + CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "savedsuccessfully"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    //        EditCancel = false;
                    //        View.Close();

                    //    }
                    //}
                    //Modules.BusinessObjects.SampleManagement.Reporting objReporting = View.ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[Oid]=?"),((AuditData)View.CurrentObject).Source;
                    IList<AuditData> Objects1 = ObjectSpace.GetObjects<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);

                 
                        if (data.FormName == "Report Tracking")
                        {
                            Modules.BusinessObjects.SampleManagement.Reporting objReporting = View.ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.Reporting>(CriteriaOperator.Parse("[Oid] = ?", data.Source));
                            if (objReporting != null)
                            {
                                data.CommentProcessed = true;
                                Application.ShowViewStrategy.ShowMessage("Report " + objReporting.ReportID + CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "savedsuccessfully"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                EditCancel = false;
                                View.Close();

                            }
                        }
                        else if (data.FormName == "Job ID Automation")
                        {
                            //TaskJobIDAutomation objAutomation = View.ObjectSpace.FindObject<TaskJobIDAutomation>(CriteriaOperator.Parse("[Oid] = ?", data.Source));

                           
                            IList<AuditData> Objects = ObjectSpace.GetObjects<AuditData>(CriteriaOperator.Parse("[CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                            if (Objects.Count() > 0)
                            {
                                foreach (AuditData audit in Objects.Where(a => a.IsDeleted == false).ToList())
                                {
                                    //data.CommentProcessed = true;
                                    audit.Comment=data.Comment;
                                ObjectSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "savedsuccessfully"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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

        public void insertauditdata(IObjectSpace os, Guid source, OperationType type, string formname, string id, string propertyname, string oldval, string newval, string comment)
        {
            try
            {
             createauditdata(os.CreateObject<AuditData>(), source, type, formname, id, propertyname, oldval, newval, comment, os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void insertauditdata(UnitOfWork os, Guid source, OperationType type, string formname, string id, string propertyname, string oldval, string newval, string comment)
        {
            try
            {
             createauditdata(new AuditData(os), source, type, formname, id, propertyname, oldval, newval, comment, os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void createauditdata(AuditData obj, Guid source, OperationType type, string formname, string id, string propertyname, string oldval, string newval, string comment, Employee data)
        {
            try
            {
                obj.Source = source;
                obj.OperationType = type;
                obj.FormName = formname;
                obj.ID = id;
                obj.PropertyName = propertyname;
                obj.Oldvalue = oldval;
                obj.Newvalue = newval;
                obj.Comment = comment;
                obj.CommentProcessed = false;
                obj.CreatedBy = data;
                obj.CreatedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_ObjectDeleting(object sender, ObjectsManipulatingEventArgs e)
        {
            try
            {
                var Objects = e.Objects.Cast<object>().Where(a => a.GetType() != typeof(AuditData));
                if (Objects.Count() > 0)
                {
                    if (objNavInfo.SelectedNavigationCaption == "Sample Registration")
                    {
                        foreach (var objs in Objects)
                        {
                            if (objs.GetType() == typeof(SampleConditionCheck))
                            {
                                SampleConditionCheck sample = objs as SampleConditionCheck;
                                if (sample != null && sample.SampleRegistration.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                                {
                                    createdeleteaudit(sender, sample.SampleRegistration.Oid, sample.SampleRegistration.JobID, "SampleConditionCheck", sample.Oid.ToString());
                                }
                            }
                            else if (objs.GetType() == typeof(SampleConditionCheckComment))
                            {
                                Samplecheckin samplecheckin = SRInfo.CurrentJob;
                                SampleConditionCheckComment sample = objs as SampleConditionCheckComment;
                                if (samplecheckin != null && sample != null && samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                                {
                                    createdeleteaudit(sender, samplecheckin.Oid, samplecheckin.JobID, "SampleConditionCheckcomment", sample.Oid.ToString());
                                }
                            }
                            else if (objs.GetType() == typeof(Attachment))
                            {
                                Attachment sample = objs as Attachment;
                                if (sample != null && sample.Samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                                {
                                    createdeleteaudit(sender, sample.Samplecheckin.Oid, sample.Samplecheckin.JobID, "Attachment", sample.Name);
                                }
                            }
                            else if (objs.GetType() == typeof(Notes))
                            {
                                Notes sample = objs as Notes;
                                if (sample != null && sample.Samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                                {
                                    createdeleteaudit(sender, sample.Samplecheckin.Oid, sample.Samplecheckin.JobID, "Notes", sample.Title);
                                }
                            }
                        }
                    }
                    if (objNavInfo.SelectedNavigationCaption == "Sample Prep Batches")
                    {
                        foreach (var objs in Objects)
                        {
                            if (objs.GetType() == typeof(SampleConditionCheck))
                            {
                                SampleConditionCheck sample = objs as SampleConditionCheck;
                                if (sample != null && sample.SampleRegistration.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                                {
                                    createdeleteaudit(sender, sample.SampleRegistration.Oid, sample.SampleRegistration.JobID, "SampleConditionCheck", sample.Oid.ToString());
                                }
                            }
                            else if (objs.GetType() == typeof(SampleConditionCheckComment))
                            {
                                Samplecheckin samplecheckin = SRInfo.CurrentJob;
                                SampleConditionCheckComment sample = objs as SampleConditionCheckComment;
                                if (samplecheckin != null && sample != null && samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                                {
                                    createdeleteaudit(sender, samplecheckin.Oid, samplecheckin.JobID, "SampleConditionCheckcomment", sample.Oid.ToString());
                                }
                            }
                            else if (objs.GetType() == typeof(Attachment))
                            {
                                Attachment sample = objs as Attachment;
                                if (sample != null && sample.Samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                                {
                                    createdeleteaudit(sender, sample.Samplecheckin.Oid, sample.Samplecheckin.JobID, "Attachment", sample.Name);
                                }
                            }
                            else if (objs.GetType() == typeof(Notes))
                            {
                                Notes sample = objs as Notes;
                                if (sample != null && sample.Samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                                {
                                    createdeleteaudit(sender, sample.Samplecheckin.Oid, sample.Samplecheckin.JobID, "Notes", sample.Title);
                                }
                            }
                        }
                    }
                    //else if(objNavInfo.SelectedNavigationCaption == "Task JobID Automation")
                    //{
                    //    foreach (var objs in Objects)
                    //    {
                    //        if (objs.GetType() == typeof(TaskJobIDAutomation))
                    //        {
                    //            TaskJobIDAutomation sample = objs as TaskJobIDAutomation;
                    //            if (sample != null /*&& sample.SamplingProposal.Status != RegistrationStatus.PendingSubmission*/)
                    //            {
                    //                createdeleteaudit(sender, sample.Oid, sample.SRID.RegistrationID.ToString(), "DaysinAdvance", sample.DaysinAdvance.ToString());
                    //            }
                    //        }
                           
                    //    }
                    //}
               
                    else if(objNavInfo.SelectedNavigationCaption == "Report Tracking")
                    {
                        foreach (var objs in Objects)
                        {
                            if (objs.GetType() == typeof(Modules.BusinessObjects.SampleManagement.Reporting))
                            {
                                Modules.BusinessObjects.SampleManagement.Reporting sample = objs as Modules.BusinessObjects.SampleManagement.Reporting;
                                if (sample != null /*&& sample.SamplingProposal.Status != RegistrationStatus.PendingSubmission*/)
                                {
                                    createdeleteaudit(sender, sample.JobID.Oid, sample.JobID.ToString(), "ReportName", sample.ReportName.ToString());
                                }
                            }
                           
                        }
                    }

                    else if (objNavInfo.SelectedNavigationCaption == "Sample Sites")
                    {
                        foreach (var objs in Objects)
                        {
                            if (objs.GetType() == typeof(SampleSites))
                            {
                                SampleSites sample = objs as SampleSites;
                                if (sample != null)
                                {
                                    createdeleteaudit(sender, sample.Oid, sample.SiteID, "SampleSites", sample.SiteName.ToString());
                                }
                            }
                        }
                        }
                    else  if (objNavInfo.SelectedNavigationCaption == "Tests")
                    {
                        foreach (var objs in Objects)
                        {
                        //    if (objs.GetType() == typeof(TestGuide))
                        //    {
                        //        TestMethod testMethod = SRInfo.CurrentTest;

                        //        TestGuide testGuide = objs as TestGuide;
                        //        SRInfo.booltest = false;
                        //        if (testGuide != null  && SRInfo.booltest ==false/*&& sample.SamplingProposal.Status != RegistrationStatus.PendingSubmission*/)
                        //        {
                        //            //createdeleteaudit(sender, testGuide.TestMethod.Oid, testGuide.TestMethod.TestCode, "TestGudie", testGuide.Container.ContainerName);
                        //            createdeleteaudit(sender, testMethod.Oid, testMethod.TestCode, "TestGudie", testGuide.Container.ContainerName.ToString());
                        //            //processobjectchange(sender, e, testMethod.Oid, typeof(TestGuide), "TestGuide", testMethod.TestCode, e.PropertyName);


                        //        }
                        //    }
                            //else if (objs.GetType() == typeof(PrepMethod))
                            //{
                            //    TestMethod testMethod = SRInfo.CurrentTest;

                            //    PrepMethod prepMethod = objs as PrepMethod;

                            //    if (prepMethod != null /*&& sample.SamplingProposal.Status != RegistrationStatus.PendingSubmission*/)
                            //    {
                            //        //createdeleteaudit(sender, prepMethod.TestMethod.Oid, prepMethod.TestMethod.TestCode, "PrepMethod", prepMethod.PrepType.SamplePrepType);
                            //        //createdeleteaudit(sender, testMethod.Oid, testMethod.TestCode, "PrepMethod", prepMethod.PrepType.SamplePrepType);
                            //        createdeleteaudit(sender, testMethod.Oid,testMethod.TestCode, "PrepMethod", prepMethod.PrepType.ToString());

                            //        // processobjectdelete(sender, e, testMethod.Oid, typeof(QCType), "PrepMethod", prepMethod.PrepType.SamplePrepType.ToString(), e.PropertyName);
                            //        // processobjectchange(sender, e, testMethod.Oid, typeof(PrepMethod), objNavInfo.SelectedNavigationCaption, testMethod.TestCode, e.PropertyName);
                            //        //processobjectchange(sender, e, testMethod.Oid, typeof(PrepMethod), objNavInfo.SelectedNavigationCaption, testMethod.TestCode, e.PropertyName);



                            //    }
                            //}

                        }
                    }
                    
                        //else lseif (objNavInfo.SelectedNavigationCaption == "Sampling Proposal")
                        //{
                        //    foreach (var objs in Objects)
                        //    {
                        //        if (objs.GetType() == typeof(Attachment))
                        //        {
                        //            Attachment sample = objs as Attachment;
                        //            if (sample != null && sample.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                        //            {
                        //                createdeleteaudit(sender, sample.SamplingProposal.Oid, sample.SamplingProposal.RegistrationID, "Attachment", sample.Name);
                        //            }
                        //        }
                        //        else if (objs.GetType() == typeof(Notes))
                        //        {
                        //            Notes sample = objs as Notes;
                        //            if (sample != null && sample.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                        //            {
                        //                createdeleteaudit(sender, sample.SamplingProposal.Oid, sample.SamplingProposal.RegistrationID, "Notes", sample.Title);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void createdeleteaudit(object sender, Guid sourceID, string ID, string propertyname, string value)
        {
            try
            {
                if (objAuditInfo.Auditedlist == null)
                {
                    objAuditInfo.Auditedlist = new System.Collections.Generic.List<AuditData>();
                }
                //AuditData oldobj = objAuditInfo.Auditedlist.Where(a => a.Source == sourceID && a.PropertyName == propertyname).FirstOrDefault();
                //if (oldobj != null)
                //{
                //    return;
                //}
                AuditData obj = ((XPObjectSpace)sender).CreateObject<AuditData>();
                createauditdata(obj, sourceID, OperationType.Deleted, objNavInfo.SelectedNavigationCaption, ID, propertyname, value, "", "", ((XPObjectSpace)sender).GetObjectByKey<Employee>(SecuritySystem.CurrentUserId));
                objAuditInfo.Auditedlist.Add(obj);
               
              
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void createaudit(object sender, Guid sourceID, string ID, string propertyname, string value)
        {
            try
            {
                if (objAuditInfo.Auditedlist == null)
                {
                    objAuditInfo.Auditedlist = new System.Collections.Generic.List<AuditData>();
                }
                //AuditData oldobj = objAuditInfo.Auditedlist.Where(a => a.Source == sourceID && a.PropertyName == propertyname).FirstOrDefault();
                //if (oldobj != null)
                //{
                //    return;
                //}
                AuditData obj = ((XPObjectSpace)sender).CreateObject<AuditData>();
                createauditdata(obj, sourceID, OperationType.Created, objNavInfo.SelectedNavigationCaption, ID, propertyname, "", value, "", ((XPObjectSpace)sender).GetObjectByKey<Employee>(SecuritySystem.CurrentUserId));
                objAuditInfo.Auditedlist.Add(obj);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            try
            {
                clearaudit();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void processobjectchange(object sender, ObjectChangedEventArgs e, Guid sourceID, Type type, string formname, string id, string propertyname)
        {
            try
            {
                if (objAuditInfo.Auditedlist == null)
                {
                    objAuditInfo.Auditedlist = new System.Collections.Generic.List<AuditData>();
                }
                AuditData oldobj = objAuditInfo.Auditedlist.Where(a => a.Source == sourceID && a.PropertyName == propertyname && a.ID == id).FirstOrDefault();
                if (oldobj != null)
                {
                    objAuditInfo.Auditedlist.Remove(oldobj);
                    AuditData oldobj1=((XPObjectSpace)sender).GetObject(oldobj);
                    if (oldobj1!=null)
                    {
                        ((XPObjectSpace)sender).Delete(oldobj); 
                    }
                }
                AuditData obj = ((XPObjectSpace)sender).CreateObject<AuditData>();
                if (obj != null)
                {
                    obj.Source = sourceID;
                    obj.OperationType = OperationType.ValueChanged;
                    obj.FormName = formname;
                    obj.ID = id;
                    obj.PropertyName = propertyname;
                    if ((e.OldValue != null && e.OldValue.ToString().Where(a => a == '-').Count() >= 4) || (e.NewValue != null && e.NewValue.ToString().Where(a => a == '-').Count() >= 4))
                    {
                        IModelClass modelClass = Application.Model.BOModel.GetClass(type);
                        if (modelClass != null)
                        {
                            IModelMember modelMember = modelClass.OwnMembers.FirstOrDefault(member => member.Name == e.PropertyName);
                            if (modelMember != null)
                            {
                                ITypeInfo typeInfo = XafTypesInfo.Instance.FindTypeInfo(modelMember.MemberInfo.MemberTypeInfo.FullName);
                                if (typeInfo != null)
                                {
                                    IMemberInfo memberInfo = typeInfo.FindMember(!string.IsNullOrEmpty(modelMember.LookupProperty) ? modelMember.LookupProperty : e.PropertyName);
                                    if (memberInfo != null)
                                    {
                                        obj.Oldvalue = memberInfo.GetValue(e.OldValue)?.ToString();
                                        obj.Newvalue = memberInfo.GetValue(e.NewValue)?.ToString();
                                    }
                                    else
                                    {
                                        if ((e.PropertyName == "SampleMatries" || e.PropertyName == "SampleCategory" || e.PropertyName == "TestName" || e.PropertyName == "Matrix" /*|| e.PropertyName == "PrepType"*/ || e.PropertyName == "PrepMethod" || e.PropertyName == "Container" || e.PropertyName == "Preservative" || e.PropertyName == "Instrument") && (type == typeof(Samplecheckin) || type == typeof(TestMethod) /*|| type == typeof(PrepTypes)*/ || (type == typeof(PrepMethod) || (type == typeof(SamplePrepBatch)))))
                                        {
                                            obj.Oldvalue = !string.IsNullOrEmpty(e.OldValue?.ToString()) ? loopval(e.PropertyName, e.OldValue.ToString().Split(new string[] { "; " }, StringSplitOptions.None)) : null;
                                            obj.Newvalue = !string.IsNullOrEmpty(e.NewValue?.ToString()) ? loopval(e.PropertyName, e.NewValue.ToString().Split(new string[] { "; " }, StringSplitOptions.None)) : null;
                                        }
                                        else if ( e.PropertyName == "PrepType")
                                        {
                                            if (e.NewValue is PrepTypes newPrepTypes)
                                            {
                                                obj.Newvalue = newPrepTypes.SamplePrepType;
                                            }

                                            else if (e.OldValue is PrepTypes oldPrepTypes)
                                            {
                                                obj.Newvalue = oldPrepTypes.SamplePrepType;
                                            }

                                        }
                                        else
                                        {
                                            obj.Oldvalue = e.OldValue?.ToString();
                                            obj.Newvalue = e.NewValue?.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        obj.Oldvalue = e.OldValue?.ToString();
                        obj.Newvalue = e.NewValue?.ToString();
                    }
                    obj.Comment = "";
                    obj.CommentProcessed = false;
                    obj.CreatedBy = ((XPObjectSpace)sender).GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    obj.CreatedDate = DateTime.Now;
                    objAuditInfo.Auditedlist.Add(obj);
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
                if (e.PropertyName != "GCRecord" && e.GetType() != typeof(AuditData))
                {
                    if (objNavInfo.SelectedNavigationCaption == "Sample Registration")
                    {
                        if (e.Object.GetType() == typeof(Samplecheckin) && e.PropertyName != "Status" && e.PropertyName != "Index" && e.PropertyName != "Isinvoicesummary")
                        {
                            Samplecheckin samplecheckin = e.Object as Samplecheckin;
                            if (!string.IsNullOrEmpty(e.PropertyName) && samplecheckin != null && samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                            {
                                processobjectchange(sender, e, samplecheckin.Oid, typeof(Samplecheckin), objNavInfo.SelectedNavigationCaption, samplecheckin.JobID, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(Modules.BusinessObjects.SampleManagement.SampleLogIn))
                        {
                            Modules.BusinessObjects.SampleManagement.SampleLogIn samplecheckin = e.Object as Modules.BusinessObjects.SampleManagement.SampleLogIn;
                            if (!string.IsNullOrEmpty(e.PropertyName) && samplecheckin != null && samplecheckin.JobID.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                            {
                                processobjectchange(sender, e, samplecheckin.JobID.Oid, typeof(Modules.BusinessObjects.SampleManagement.SampleLogIn), objNavInfo.SelectedNavigationCaption, samplecheckin.SampleID, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(SampleConditionCheck))
                        {
                            SampleConditionCheck samplecheckin = e.Object as SampleConditionCheck;
                            if (!string.IsNullOrEmpty(e.PropertyName) && samplecheckin != null && samplecheckin.SampleRegistration != null && samplecheckin.SampleRegistration.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                            {
                                processobjectchange(sender, e, samplecheckin.SampleRegistration.Oid, typeof(SampleConditionCheck), "SampleConditionCheck", samplecheckin.JobID, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(SampleConditionCheckComment))
                        {
                            Samplecheckin samplecheckin = SRInfo.CurrentJob;
                            if (samplecheckin != null && samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                            {
                                processobjectchange(sender, e, samplecheckin.Oid, typeof(SampleConditionCheck), "SampleConditionCheckComment", samplecheckin.JobID, e.PropertyName);
                            }
                        }

                        else if (e.Object.GetType() == typeof(SampleCheckinItemChargePricing))
                        {
                            Samplecheckin samplecheckin = SRInfo.CurrentJob;
                            if (samplecheckin != null && samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                            {
                                processobjectchange(sender, e, samplecheckin.Oid, typeof(SampleConditionCheck), "ItemChargePricing", samplecheckin.JobID, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(SampleConditionCheckPoint))
                        {
                            SampleConditionCheckPoint samplecheckin = e.Object as SampleConditionCheckPoint;
                            if (!string.IsNullOrEmpty(e.PropertyName) && samplecheckin != null && samplecheckin.SampleConditionCheck.Count > 0 && samplecheckin.SampleConditionCheck[0].SampleRegistration != null && samplecheckin.SampleConditionCheck[0].SampleRegistration.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                            {
                                processobjectchange(sender, e, samplecheckin.SampleConditionCheck[0].SampleRegistration.Oid, typeof(SampleConditionCheck), "SampleConditionCheckpoints", samplecheckin.SampleConditionCheck[0].JobID, samplecheckin.CheckPoint.CheckPoint);
                            }
                        }
                        else if (e.Object.GetType() == typeof(Image))
                        {
                            Samplecheckin samplecheckin = SRInfo.CurrentJob;
                            if (!string.IsNullOrEmpty(e.PropertyName) && samplecheckin != null && samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                            {
                                processobjectchange(sender, e, samplecheckin.Oid, typeof(Image), "Image", samplecheckin.JobID, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(Attachment))
                        {
                            Samplecheckin samplecheckin = SRInfo.CurrentJob;
                            if (!string.IsNullOrEmpty(e.PropertyName) && e.PropertyName != "Samplecheckin" && samplecheckin != null && samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                            {
                                processobjectchange(sender, e, samplecheckin.Oid, typeof(Attachment), "Attachment", samplecheckin.JobID, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(Notes))
                        {
                            Samplecheckin samplecheckin = SRInfo.CurrentJob;
                            if (!string.IsNullOrEmpty(e.PropertyName) && samplecheckin != null && samplecheckin.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                            {
                                processobjectchange(sender, e, samplecheckin.Oid, typeof(Notes), "Notes", samplecheckin.JobID, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(SampleBottleAllocation))
                        {
                            SampleBottleAllocation samplecheckin = e.Object as SampleBottleAllocation;
                            if (!string.IsNullOrEmpty(e.PropertyName) && samplecheckin != null && samplecheckin.SampleRegistration != null && samplecheckin.SampleRegistration.JobID.Status != Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSubmit)
                            {
                                processobjectchange(sender, e, samplecheckin.SampleRegistration.JobID.Oid, typeof(SampleBottleAllocation), "SampleBottleAllocation", samplecheckin.SampleRegistration.SampleID, e.PropertyName);
                            }
                        }
                    }
                    else if (objNavInfo.SelectedNavigationCaption == "Tests")
                    {

                        if (e.Object.GetType() == typeof(TestMethod) && e.PropertyName != "Status" && e.PropertyName != "Index" && e.PropertyName != "Isinvoicesummary" && e.PropertyName != "QCtypesCombo")
                        {
                            TestMethod samplecheckin = e.Object as TestMethod;
                            if (!string.IsNullOrEmpty(e.PropertyName) && samplecheckin != null)
                            {
                                processobjectchange(sender, e, samplecheckin.Oid, typeof(TestMethod), objNavInfo.SelectedNavigationCaption, samplecheckin.TestCode, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(QCType))
                        {
                            TestMethod samplecheckin = SRInfo.CurrentTest;
                            if (!string.IsNullOrEmpty(e.PropertyName))
                            {
                                processobjectchange(sender, e, samplecheckin.Oid, typeof(QCType), "Qctype", samplecheckin.QCTypes.ToString(), e.PropertyName);
                            }
                        }

                        else if (e.Object.GetType() == typeof(Method))
                        {
                            TestMethod testMethod = SRInfo.CurrentTest;
                            if (!string.IsNullOrEmpty(e.PropertyName) && testMethod != null)
                            {
                                processobjectchange(sender, e, testMethod.Oid, typeof(TestMethod), objNavInfo.SelectedNavigationCaption, testMethod.TestCode, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(TestGuide))
                        {
                            TestMethod testMethod = SRInfo.CurrentTest;
                            TestGuide testGuide = (TestGuide)e.Object;

                            //if (!string.IsNullOrEmpty(e.PropertyName) && testMethod != null )

                            if (!string.IsNullOrEmpty(e.PropertyName) && testMethod != null && (e.OldValue == null || ObjectSpace.ModifiedObjects.Count > 0 && e.OldValue != null))
                            {
                                processobjectchange(sender, e, testMethod.Oid, typeof(TestGuide), "TestGuide", testMethod.TestCode, e.PropertyName);

                            }

                        }
                        else if (e.Object.GetType() == typeof(PrepMethod))
                        {
                            TestMethod testMethod = SRInfo.CurrentTest;
                            PrepMethod prepmethod = (PrepMethod)e.Object;

                            if (!string.IsNullOrEmpty(e.PropertyName) && testMethod != null && prepmethod.PrepType != null && (e.OldValue == null || ObjectSpace.ModifiedObjects.Count > 0 && e.OldValue != null))
                            {
                                processobjectchange(sender, e, testMethod.Oid, typeof(PrepMethod), "PrepMethod", testMethod.TestCode, e.PropertyName);
                                // createaudit(ObjectSpace, testMethod.Oid, testMethod.TestCode, "PrepMethod", prepmethod.PrepType.SamplePrepType);


                            }
                        }
                        //else if (e.Object.GetType() == typeof(Testparameter))
                        //{
                        //    TestMethod samplecheckin = SRInfo.CurrentTest;
                        //    if (!string.IsNullOrEmpty(e.PropertyName))
                        //    {
                        //        if (e.PropertyName == "lAccrediation")
                        //        {
                        //            string modifiedPropertyName = e.PropertyName.Substring(1);
                        //            processobjectchange(sender, e, samplecheckin.Oid, typeof(Testparameter), "Sample Parameter", samplecheckin.TestCode.ToString(), modifiedPropertyName);

                        //        }
                        //        else
                        //        {
                        //        processobjectchange(sender, e, samplecheckin.Oid, typeof(Testparameter), "Sample Parameter", samplecheckin.TestCode.ToString(), e.PropertyName);

                        //        }
                        //    }
                        //}


                    }

                    else if (objNavInfo.SelectedNavigationCaption == "Job ID Automation")
                    {
                        if (e.Object.GetType() == typeof(TaskJobIDAutomation) && e.PropertyName == "DaysinAdvance")
                        {
                            TaskJobIDAutomation taskJobIDAutomation = e.Object as TaskJobIDAutomation;
                            Session currentSessions = ((XPObjectSpace)(this.ObjectSpace)).Session;
                            XPMemberInfo optimisticLock = currentSessions.GetClassInfo(taskJobIDAutomation).OptimisticLockField;
                            object cValue = optimisticLock.GetValue(taskJobIDAutomation);
                            if (taskJobIDAutomation.COCID != null)
                            {
                                if (!string.IsNullOrEmpty(e.PropertyName) && taskJobIDAutomation != null && cValue != null && (int)cValue > 1)
                                {
                                    processobjectchange(sender, e, taskJobIDAutomation.Oid, typeof(TaskJobIDAutomation), objNavInfo.SelectedNavigationCaption, taskJobIDAutomation.COCID.ToString(), e.PropertyName);


                                }
                            }
                        }
                    }


                    else if (objNavInfo.SelectedNavigationCaption == "Sample Sites")
                    {
                        if (e.Object.GetType() == typeof(SampleSites))
                        {
                            SampleSites samplingsite = e.Object as SampleSites;
                            if (samplingsite.SiteID != null)
                            {
                                if (!string.IsNullOrEmpty(e.PropertyName) && samplingsite != null)
                                {
                                    processobjectchange(sender, e, samplingsite.Oid, typeof(SampleSites), objNavInfo.SelectedNavigationCaption, samplingsite.SiteID.ToString(), e.PropertyName);


                                }
                            }
                        }
                    }
                    //}
                    //else if (objNavInfo.SelectedNavigationCaption == "Sampling Proposal")
                    //{
                    //    if (e.Object.GetType() == typeof(SamplingProposal) && e.PropertyName != "Status" && e.PropertyName != "Index" && e.PropertyName != "Isinvoicesummary")
                    //    {
                    //        SamplingProposal samplingroposal = e.Object as SamplingProposal;
                    //        if (!string.IsNullOrEmpty(e.PropertyName) && samplingroposal != null && samplingroposal.Status != RegistrationStatus.PendingSubmission)
                    //        {
                    //            processobjectchange(sender, e, samplingroposal.Oid, typeof(SamplingProposal), objNavInfo.SelectedNavigationCaption, samplingroposal.RegistrationID, e.PropertyName);
                    //        }
                    //    }
                    //    else if (e.Object.GetType() == typeof(Sampling))
                    //    {
                    //        Sampling samplingroposal = e.Object as Sampling;
                    //        if (!string.IsNullOrEmpty(e.PropertyName) && samplingroposal != null && samplingroposal.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                    //        {
                    //            processobjectchange(sender, e, samplingroposal.SamplingProposal.Oid, typeof(Sampling), objNavInfo.SelectedNavigationCaption, samplingroposal.SampleID, e.PropertyName);
                    //        }
                    //    }

                    //    else if (e.Object.GetType() == typeof(Attachment))
                    //    {
                    //        SamplingProposal samplecheckin = objSMInfo.CurrentJob;
                    //        if (!string.IsNullOrEmpty(e.PropertyName) && e.PropertyName != "SamplingProposal" && samplecheckin != null && samplecheckin.Status != RegistrationStatus.PendingSubmission)
                    //        {
                    //            processobjectchange(sender, e, samplecheckin.Oid, typeof(Attachment), "Attachment", samplecheckin.RegistrationID, e.PropertyName);
                    //        }
                    //    }
                    //    else if (e.Object.GetType() == typeof(Notes))
                    //    {
                    //        SamplingProposal samplecheckin = objSMInfo.CurrentJob;
                    //        if (!string.IsNullOrEmpty(e.PropertyName) && e.PropertyName != "SamplingProposal" && samplecheckin != null && samplecheckin.Status != RegistrationStatus.PendingSubmission)
                    //        {
                    //            processobjectchange(sender, e, samplecheckin.Oid, typeof(Notes), "Notes", samplecheckin.RegistrationID, e.PropertyName);
                    //        }
                    //    }
                    //    else if (e.Object.GetType() == typeof(SamplingBottleAllocation))
                    //    {
                    //        SamplingBottleAllocation samplecheckin = e.Object as SamplingBottleAllocation;
                    //        if (!string.IsNullOrEmpty(e.PropertyName) && samplecheckin != null && samplecheckin.Sampling != null && samplecheckin.Sampling.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                    //        {
                    //            processobjectchange(sender, e, samplecheckin.Sampling.SamplingProposal.Oid, typeof(SamplingBottleAllocation), "SampleBottleAllocation", samplecheckin.Sampling.SampleID, e.PropertyName);
                    //        }
                    //    }
                    //}
                    else if (objNavInfo.SelectedNavigationCaption == "Field Data Entry" && Application.MainWindow.View.Id.Contains("History") && View.Id == "SampleLogIn_ListView_FieldDataEntry_Station")
                    {
                        if (e.Object.GetType() == typeof(Modules.BusinessObjects.SampleManagement.SampleLogIn))
                        {
                            Modules.BusinessObjects.SampleManagement.SampleLogIn samplecheckin = e.Object as Modules.BusinessObjects.SampleManagement.SampleLogIn;
                            if (!string.IsNullOrEmpty(e.PropertyName) && samplecheckin != null)
                            {
                                processobjectchange(sender, e, samplecheckin.JobID.Oid, typeof(Modules.BusinessObjects.SampleManagement.SampleLogIn), objNavInfo.SelectedNavigationCaption, samplecheckin.SampleID, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(SampleParameter))
                        {
                            SampleParameter samplecheckin = e.Object as SampleParameter;
                            if (!string.IsNullOrEmpty(e.PropertyName) && samplecheckin != null)
                            {
                                processobjectchange(sender, e, samplecheckin.Samplelogin.JobID.Oid, typeof(SampleParameter), objNavInfo.SelectedNavigationCaption, samplecheckin.Samplelogin.SampleID, e.PropertyName);
                            }
                        }
                    }
                    else if (objNavInfo.SelectedNavigationCaption == "Sample Prep Batches" && Application.MainWindow.View.Id == "SamplePrepBatch_DetailView_Copy_History")
                    {
                        if (e.Object.GetType() == typeof(SamplePrepBatch) && e.PropertyName != "NPJobid")
                        {
                            SamplePrepBatch objSamplePrepBatch = e.Object as SamplePrepBatch;
                            if (!string.IsNullOrEmpty(e.PropertyName) && objSamplePrepBatch != null)
                            {
                                processobjectchange(sender, e, objSamplePrepBatch.Oid, typeof(SamplePrepBatch), objNavInfo.SelectedNavigationCaption, objSamplePrepBatch.PrepBatchID, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(SamplePrepBatchSequence))
                        {
                            SamplePrepBatch objSamplePrepSequence = e.Object as SamplePrepBatch;
                            if (!string.IsNullOrEmpty(e.PropertyName) && objSamplePrepSequence != null)
                            {
                                processobjectchange(sender, e, objSamplePrepSequence.Oid, typeof(SamplePrepBatchSequence), objNavInfo.SelectedNavigationCaption, objSamplePrepSequence.PrepBatchID, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(QCType))
                        {
                            SamplePrepBatch objQCType = e.Object as SamplePrepBatch;
                            if (!string.IsNullOrEmpty(e.PropertyName) && objQCType != null)
                            {
                                processobjectchange(sender, e, objQCType.Oid, typeof(QCType), objNavInfo.SelectedNavigationCaption, objQCType.PrepBatchID, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(Reagent))
                        {
                            SamplePrepBatch objReagent = e.Object as SamplePrepBatch;
                            if (!string.IsNullOrEmpty(e.PropertyName) && objReagent != null)
                            {
                                processobjectchange(sender, e, objReagent.Oid, typeof(Reagent), objNavInfo.SelectedNavigationCaption, objReagent.PrepBatchID, e.PropertyName);
                            }
                        }
                        else if (e.Object.GetType() == typeof(Labware))
                        {
                            SamplePrepBatch objLabware = e.Object as SamplePrepBatch;
                            if (!string.IsNullOrEmpty(e.PropertyName) && objLabware != null)
                            {
                                processobjectchange(sender, e, objLabware.Oid, typeof(Labware), objNavInfo.SelectedNavigationCaption, objLabware.PrepBatchID, e.PropertyName);
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

        private string loopval(string property, string[] ids)
        {
            string val = string.Empty;
            foreach (string id in ids)
            {
                val += getreferencevalues(property, id);
                if (id != ids.Last())
                {
                    val += "; ";
                }
            }
            return val;
        }

        private string getreferencevalues(string property, string id)
        {
            if (property == "SampleMatries")
            {
                VisualMatrix obj = View.ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid]=?", new Guid(id)));
                if (obj != null)
                {
                    return obj.VisualMatrixName;
                }
            }
            else if (property == "SampleCategory")
            {
                SampleCategory obj = View.ObjectSpace.FindObject<SampleCategory>(CriteriaOperator.Parse("[Oid]=?", new Guid(id)));
                if (obj != null)
                {
                    return obj.SampleCategoryName;
                }
            }
            else if (property == "TestName")
            {
                TestMethod obj = View.ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid]=?", new Guid(id)));
                if (obj != null)
                {
                    return obj.TestName;
                }
            }
            else if (property == "Matrix")
            {
                TestMethod obj = View.ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid]=?", new Guid(id)));
                if (obj != null)
                {
                    return obj.TestName;
                }
            }
            else if (property == "Method")
            {
                TestMethod obj = View.ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid]=?", new Guid(id)));
                if (obj != null)
                {
                    return obj.TestName;
                }
            }
            else if (property == "Instrument")
            {
                Labware obj = View.ObjectSpace.FindObject<Labware>(CriteriaOperator.Parse("[Oid]=?", new Guid(id)));
                if (obj != null)
                {
                    return obj.AssignedName;
                }
            }
            //else if (property == "QCTypes")
            //{
            //    TestMethod obj = View.ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid]=?", new Guid(id)));
            //    if (obj != null)
            //    {
            //        return obj.TestName;
            //    }
            //}
            return string.Empty;
        }

        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                if (View.Id == "AuditData_ListView")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("AuditLog", this);
                        editor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                        editor.Grid.SettingsPager.AlwaysShowPager = false;
                        editor.Grid.SettingsPager.PageSizeItemSettings.Visible = false;
                        editor.Grid.Settings.VerticalScrollableHeight = 450;
                        editor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        if (editor.Grid.Columns["CreatedDate"] != null)
                        {
                            editor.Grid.Columns["CreatedDate"].Width = System.Web.UI.WebControls.Unit.Percentage(12);
                        }
                        if (editor.Grid.Columns["FormName"] != null)
                        {
                            editor.Grid.Columns["FormName"].Width = System.Web.UI.WebControls.Unit.Percentage(12);
                        }
                        if (editor.Grid.Columns["OperationType"] != null)
                        {
                            editor.Grid.Columns["OperationType"].Width = System.Web.UI.WebControls.Unit.Percentage(10);
                        }
                        if (editor.Grid.Columns["ID"] != null)
                        {
                            editor.Grid.Columns["ID"].Width = System.Web.UI.WebControls.Unit.Percentage(8);
                        }
                        if (editor.Grid.Columns["PropertyName"] != null)
                        {
                            editor.Grid.Columns["PropertyName"].Width = System.Web.UI.WebControls.Unit.Percentage(12);
                        }
                        if (editor.Grid.Columns["Oldvalue"] != null)
                        {
                            editor.Grid.Columns["Oldvalue"].Width = System.Web.UI.WebControls.Unit.Percentage(15);
                        }
                        if (editor.Grid.Columns["Newvalue"] != null)
                        {
                            editor.Grid.Columns["Newvalue"].Width = System.Web.UI.WebControls.Unit.Percentage(15);
                        }
                        if (editor.Grid.Columns["IsComment"] != null)
                        {
                            editor.Grid.Columns["IsComment"].Width = System.Web.UI.WebControls.Unit.Percentage(10);
                        }
                    }
                }
                else if (View.Id == "AuditData_DetailView")
                {
                    StaticTextViewItem sam = ((DetailView)View).FindItem("MSG") as StaticTextViewItem;
                    if (sam != null)
                    {
                        if (((DetailView)View).ViewEditMode == DevExpress.ExpressApp.Editors.ViewEditMode.Edit)
                        {
                            //if(View.CurrentObject is Samplecheckin)
                            //{
                            Samplecheckin checkin = View.ObjectSpace.GetObjectByKey<Samplecheckin>(((AuditData)View.CurrentObject).Source);
                            if (checkin != null)
                            {
                                sam.Text = "Changes made on " + checkin.JobID + ", please provide the comment";
                            }
                            
                            TestMethod testMethod = View.ObjectSpace.GetObjectByKey<TestMethod>(((AuditData)View.CurrentObject).Source);
                            if (testMethod != null)
                            {
                                sam.Text = "Changes made on " + testMethod.TestCode + ", please provide the comment";

                            }

                            Modules.BusinessObjects.SampleManagement.Reporting objReporting = View.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.SampleManagement.Reporting>(((AuditData)View.CurrentObject).Source);

                            if (objReporting != null)
                            {
                                sam.Text = "Changes made on " + objReporting.JobID.JobID + ", please provide the comment";

                            } 
                            
                           TaskJobIDAutomation objTaskJobIDAutomation = View.ObjectSpace.GetObjectByKey<TaskJobIDAutomation>(((AuditData)View.CurrentObject).Source);

                            if (objTaskJobIDAutomation != null)
                            {
                                sam.Text = "Changes made on " +"COC"+ objTaskJobIDAutomation.COCID.COC_ID + ", please provide the comment";

                            }

                            
                            //}


                        }
                        else
                        {
                            sam.Text = "";
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

        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (e.DataColumn.FieldName == "IsComment" && (bool)e.CellValue == true)
                {
                    e.Cell.ToolTip = ((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "Comment").ToString();
                    e.Cell.Attributes.Add("onclick", "RaiseXafCallback(globalCallbackControl, 'AuditLog'," + e.VisibleIndex + " , '', false);");
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
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        AuditData obj = View.ObjectSpace.FindObject<AuditData>(CriteriaOperator.Parse("[Oid]=?", gridListEditor.Grid.GetRowValues(int.Parse(parameter), "Oid")));
                        if (obj != null)
                        {
                            getcomments(View.ObjectSpace, obj, DevExpress.ExpressApp.Editors.ViewEditMode.View);
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

        private void Audit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {

                // if (View.Id == "TaskJobIDAutomation_ListView")

                    //{
                    //   IList<Guid> lstSampleLogIn = ((ListView)View).CollectionSource.List.Cast<TaskJobIDAutomation>().Select(i => i.Oid).ToList();
                    //    if (lstSampleLogIn != null)
                    //    {
                    //        CollectionSource cs = new CollectionSource(Application.CreateObjectSpace(), typeof(AuditData));                     
                    //        cs.Criteria["filter"] = new InOperator("Source", lstSampleLogIn);
                    //        ListView listview = Application.CreateListView("AuditData_ListView", cs, false);
                    //        listview.Caption = "Audit Trail";
                    //        ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                    //        showViewParameters.Context = TemplateContext.PopupWindow;
                    //        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //        DialogController dc = Application.CreateController<DialogController>();
                    //        dc.AcceptAction.Active.SetItemValue("disable", false);
                    //        dc.CancelAction.Active.SetItemValue("disable", false);
                    //        showViewParameters.Controllers.Add(dc);
                    //        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    //    }


                    //}
                    //else if (View.Id == "Reporting_ListView_Copy_ReportView")
                    //{
                    //    if (View.SelectedObjects.Count > 0)
                    //    {
                    //        IList<Guid> lstReporting1 = View.SelectedObjects.Cast<Modules.BusinessObjects.SampleManagement.Reporting>().Where(i => i.JobID != null).Select(i => i.JobID.Oid).ToList();
                    //        CollectionSource cs = new CollectionSource(Application.CreateObjectSpace(), typeof(AuditData));
                    //        cs.Criteria["filter"] = new InOperator("Source", lstReporting1);
                    //        ListView listview = Application.CreateListView("AuditData_ListView", cs, false);
                    //        listview.Caption = "Audit Trail";
                    //        ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                    //        showViewParameters.Context = TemplateContext.PopupWindow;
                    //        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //        DialogController dc = Application.CreateController<DialogController>();
                    //        dc.AcceptAction.Active.SetItemValue("disable", false);
                    //        dc.CancelAction.Active.SetItemValue("disable", false);
                    //        showViewParameters.Controllers.Add(dc);
                    //        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));


                    //    }
                    //    else
                    //    {
                    //        IList<Guid> lstReporting = ((ListView)View).CollectionSource.List.Cast<Modules.BusinessObjects.SampleManagement.Reporting>().Select(i => i.JobID.Oid).ToList();
                    //        if (lstReporting != null)
                    //        {
                    //            CollectionSource cs = new CollectionSource(Application.CreateObjectSpace(), typeof(AuditData));
                    //            cs.Criteria["filter"] = new InOperator("Source", lstReporting);
                    //            ListView listview = Application.CreateListView("AuditData_ListView", cs, false);
                    //            listview.Caption = "Audit Trail";
                    //            ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                    //            showViewParameters.Context = TemplateContext.PopupWindow;
                    //            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //            DialogController dc = Application.CreateController<DialogController>();
                    //            dc.AcceptAction.Active.SetItemValue("disable", false);
                    //            dc.CancelAction.Active.SetItemValue("disable", false);
                    //            showViewParameters.Controllers.Add(dc);
                    //            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    //        }
                    //    }
                    //}

                //if (View.Id == "TestMethod_DetailView")
                //{
                //    CollectionSource cs = new CollectionSource(Application.CreateObjectSpace(), typeof(AuditData));
                //    //cs.Criteria["filter"] = CriteriaOperator.Parse("[Source] = ?", ((Samplecheckin)View.CurrentObject).Oid);
                //    if (objtestinfo.objCurrentTest != null)
                //    {
                //        //cs.Criteria["filter"] = CriteriaOperator.Parse("[Source] = ?", objtestinfo.objCurrentTest);
                //        cs.Criteria["filter"] = new InOperator("Source", objtestinfo.objCurrentTest.Oid);
                //    }
                //    ListView listview = Application.CreateListView("AuditData_ListView", cs, false);
                //    listview.Caption = "Audit Trail";
                //    ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                //    showViewParameters.Context = TemplateContext.PopupWindow;
                //    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                //    DialogController dc = Application.CreateController<DialogController>();
                //    dc.AcceptAction.Active.SetItemValue("disable", false);
                //    dc.CancelAction.Active.SetItemValue("disable", false);
                //    showViewParameters.Controllers.Add(dc);
                //    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                //}



                //else
                //{
                    CollectionSource cs = new CollectionSource(Application.CreateObjectSpace(), typeof(AuditData));
                //cs.Criteria["filter"] = CriteriaOperator.Parse("[Source] = ?", ((Samplecheckin)View.CurrentObject).Oid);

                if (View is DetailView)
                {
                    if (objAuditInfo.currentViewOid != null)
                {
                        cs.Criteria["filter"] = CriteriaOperator.Parse("[Source] = ?", objAuditInfo.currentViewOid);
                    }
                    else if (objtestinfo.objCurrentTest != null)
                    {
                        cs.Criteria["filter"] = CriteriaOperator.Parse("[Source] = ?", objtestinfo.objCurrentTest);

                    }
                        ListView listview = Application.CreateListView("AuditData_ListView", cs, false);
                        listview.Caption = "Audit Trail";
                        ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.AcceptAction.Active.SetItemValue("disable", false);
                        dc.CancelAction.Active.SetItemValue("disable", false);
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                else if (View is ListView)
                {
                    if (View.Id == "Reporting_ListView_Copy_ReportView")
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            IList<Guid> lstReporting1 = View.SelectedObjects.Cast<Modules.BusinessObjects.SampleManagement.Reporting>().Where(i => i.Oid != null).Select(i => i.Oid).ToList();
                            CollectionSource cs2 = new CollectionSource(Application.CreateObjectSpace(), typeof(AuditData));
                            cs2.Criteria["filter"] = new InOperator("Source", lstReporting1);
                            ListView listview = Application.CreateListView("AuditData_ListView", cs2, false);
                            listview.Caption = "Audit Trail";
                            ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.AcceptAction.Active.SetItemValue("disable", false);
                            dc.CancelAction.Active.SetItemValue("disable", false);
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));


                        }
                        else
                        {
                            IList<Guid> lstReporting = ((ListView)View).CollectionSource.List.Cast<Modules.BusinessObjects.SampleManagement.Reporting>().Select(i => i.Oid).ToList();
                            if (lstReporting != null)
                            {
                                CollectionSource cs1 = new CollectionSource(Application.CreateObjectSpace(), typeof(AuditData));
                                cs1.Criteria["filter"] = new InOperator("Source", lstReporting);
                                ListView listview = Application.CreateListView("AuditData_ListView", cs1, false);
                                listview.Caption = "Audit Trail";
                                ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.AcceptAction.Active.SetItemValue("disable", false);
                                dc.CancelAction.Active.SetItemValue("disable", false);
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            }
                        }
                    }
                    else if (View.Id == "TaskJobIDAutomation_ListView")
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            IList<Guid> lstJobIDAutomation = View.SelectedObjects.Cast<TaskJobIDAutomation>().Where(i => i.Oid != null).Select(i => i.Oid).ToList();
                            CollectionSource cs2 = new CollectionSource(Application.CreateObjectSpace(), typeof(AuditData));
                            cs2.Criteria["filter"] = new InOperator("Source", lstJobIDAutomation);
                            ListView listview = Application.CreateListView("AuditData_ListView", cs2, false);
                            listview.Caption = "Audit Trail";
                            ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.AcceptAction.Active.SetItemValue("disable", false);
                            dc.CancelAction.Active.SetItemValue("disable", false);
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                        else
                        {
                            IList<Guid> lstJobIdautomation = ((ListView)View).CollectionSource.List.Cast<TaskJobIDAutomation>().Select(i => i.Oid).ToList();
                            CollectionSource collection = new CollectionSource(Application.CreateObjectSpace(), typeof(AuditData));
                            collection.Criteria["Filter"] = new InOperator("Source", lstJobIdautomation);
                            ListView list = Application.CreateListView("AuditData_ListView", collection, false);
                            list.Caption = "Audit Trail";
                            ShowViewParameters showViewParameters = new ShowViewParameters(list);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.AcceptAction.Active.SetItemValue("disable", false);
                            dc.CancelAction.Active.SetItemValue("disable", false);
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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
                ModificationsController controller = Frame.GetController<ModificationsController>();
                if (controller != null)
                {
                    controller.SaveAction.Executing -= SaveAction_Executing;
                    controller.SaveAndCloseAction.Executing -= SaveAction_Executing;
                    controller.SaveAndNewAction.Executing -= SaveAction_Executing;
                }
                LinkUnlinkController linkUnlink = Frame.GetController<LinkUnlinkController>();
                if (linkUnlink != null)
                {
                    linkUnlink.UnlinkAction.Execute -= UnlinkAction_Execute;
                }
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                ObjectSpace.Committed -= ObjectSpace_Committed;
                ObjectSpace.ObjectDeleting -= ObjectSpace_ObjectDeleting;
                if (View.Id == "AuditData_DetailView")
                {
                    View.Closing -= View_Closing;
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
