using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SamplingManagement;
using Modules.BusinessObjects.SamplingManagement.Settings;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.Quotes;
using Modules.BusinessObjects.TaskManagement;
using Container = Modules.BusinessObjects.Setting.Container;

namespace LDM.Module.Controllers.SamplingManagement
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TaskManagementViewController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        SamplingManagementInfo objSMInfo = new SamplingManagementInfo();
        SamplingInfo objSamplingInfo = new SamplingInfo();
        TestMethodInfo objInfo = new TestMethodInfo();
        PermissionInfo objPmsInfo = new PermissionInfo();
        curlanguage objLanguage = new curlanguage();
        string strJobID;
        string[] strSampleMatrix;
        string jScript = @"
                       Grid.UpdateEdit();
                       ";
        bool samplingfirstdefault = false;
        bool IsDisableCheckBox = false;
        private StaticText staticText;
        ASPxLookupPropertyEditor collector;
        AuditInfo objAuditInfo = new AuditInfo();
        public TaskManagementViewController()
        {
            InitializeComponent();
            TargetViewId = "SamplingProposal_DetailView;"+ "SamplingProposal_SampleLogin;"+ "Sampling_ListView_SamplingProposal;"+ "SamplingBottleAllocation_DetailView_Sampling;"
                + "SamplingTest;"+ "Testparameter_LookupListView_Sampling_AvailableTest;"+ "Testparameter_LookupListView_Sampling_SeletectedTest;"
                + "Testparameter_LookupListView_Sampling_Parameter;"+ "SamplingParameter_ListView_SamplingProposal;"+ "SamplingProposal_ListView;"
                + "SamplingProposal_CustomDueDates_ListView;"+ "Testparameter_ListView_Parameter_Sampling;"+ "COCSettings_ListView_SamplingProposal_ImportCOC;" + "SamplingProposal_DetailView_CopyRecurrence;"
                + "AnalysisPricing_ListView_Quotes_SamplingProposal;"+ "CRMQuotes_DetailView_SamplingProposal;"+ "SamplingProposal_ListView_History;";
            SamplingSample.TargetViewId = "SamplingProposal_DetailView";
            SamplingAddSample.TargetViewId = "Sampling_ListView_SamplingProposal";
            SamplingTest.TargetViewId=SamplingContainers.TargetViewId = "Sampling_ListView_SamplingProposal";
            btnSamplingQuoteImportSamples.TargetViewId = "SamplingProposal_DetailView;";
            SamplingTestSelectionAdd.TargetViewId = SamplingTestSelectionRemove.TargetViewId = SamplingTestSelectionSave.TargetViewId = "SamplingTest";
            SPSubmit.TargetViewId = "SamplingProposal_ListView;"+ "SamplingProposal_DetailView;";
            SPSubmit.TargetObjectsCriteria = "Not IsNullOrEmpty([RegistrationID]) And [Status] = 'PendingSubmission'";
            SPSaveAs.TargetViewId = "SamplingProposal_ListView;"+ "SamplingProposal_DetailView;";
            SPSaveAs.TargetObjectsCriteria= "Not IsNullOrEmpty([RegistrationID])";
            SPCancel.TargetViewId = "SamplingProposal_DetailView;";
            SPCancel.TargetObjectsCriteria = "[Status] = 'Submitted'";
            SamplingProposalDateFilterAction.TargetViewId = "SamplingProposal_ListView;";
            HistoryOfSamplingProposal.TargetViewId = "SamplingProposal_ListView;";
            CopyRecurrence.TargetViewId = "SamplingProposal_ListView";
            SamplingProposalHistoryDateFilterAction.TargetViewId = "SamplingProposal_ListView_History;";

            SimpleAction btnBottleAllocation = new SimpleAction(this, "btnSamplingBottleAllocation", PredefinedCategory.Unspecified)
            {
                Caption = "Containers"
            };
            btnBottleAllocation.TargetViewId = "SamplingProposal_DetailView;";
            btnBottleAllocation.Execute += btnBottleAllocation_Execute;
            btnBottleAllocation.Category = "Sample";

            SimpleAction btnSampleTest = new SimpleAction(this, "btnSamplingTest", PredefinedCategory.Unspecified)
            {
                Caption = "Tests"
            };
            btnSampleTest.TargetViewId = "SamplingProposal_DetailView;";
            btnSampleTest.Execute += btnSampleTest_Execute;
            btnSampleTest.Category = "Sample";

            #region COCImport
            SimpleAction btnSamplingCOCImport = new SimpleAction(this, "btnSamplingCOCImport", PredefinedCategory.Unspecified)
            {
                Caption = "Import COC"
            };
            btnSamplingCOCImport.TargetViewId = "SamplingProposal_DetailView;";
            btnSamplingCOCImport.Execute += btnCOCImport_Execute;
            btnSamplingCOCImport.ImageName = "Down_16x16";
            btnSamplingCOCImport.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            btnSamplingCOCImport.Category = "ImportCOCSamples";
            btnSamplingCOCImport.TargetObjectsCriteria = "[SampleCount] = 0";
            #endregion
            #region BtnDeleteParameter
            SimpleAction btnDeleteSamplingSamplesandTest = new SimpleAction(this, "btnDeleteSamplingSamplesandTest", PredefinedCategory.ObjectsCreation)
            {
                Caption = "Delete"
            };
            btnDeleteSamplingSamplesandTest.TargetViewId = "SamplingParameter_ListView_SamplingProposal;";
            btnDeleteSamplingSamplesandTest.Execute += DeleteAction_Execute;
            btnDeleteSamplingSamplesandTest.ImageName = "Action_Delete";
            #endregion
        }
        private void DeleteAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Sampling_ListView_SamplingProposal")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        List<Sampling> lstSampleLogin = View.SelectedObjects.Cast<Sampling>().ToList();
                        IObjectSpace os = Application.CreateObjectSpace();
                        Session currentSession = ((XPObjectSpace)os).Session;
                        UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                        foreach (Sampling objSampleLogin in lstSampleLogin)
                        {
                            Sampling obj = uow.GetObjectByKey<Sampling>(objSampleLogin.Oid);
                            if (obj.Testparameters.Count > 0)
                            {
                                List<SamplingParameter> lstSampleParameters = obj.SamplingParameter.Cast<SamplingParameter>().ToList();
                                foreach (SamplingParameter objSampleParam in lstSampleParameters)
                                {
                                    SamplingParameter sampleParam = uow.GetObjectByKey<SamplingParameter>(objSampleParam.Oid);
                                    uow.Delete(uow.GetObjectByKey<SamplingParameter>(sampleParam.Oid));
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    uow.CommitChanges();
                                }
                                int spCount = View.ObjectSpace.GetObjectsCount(typeof(SamplingParameter), CriteriaOperator.Parse("[Sampling.Oid] = ?", obj.Oid));
                                if (spCount == 0)
                                {
                                    XPClassInfo SampleBottleAllocationinfo;
                                    SampleBottleAllocationinfo = uow.GetClassInfo(typeof(SamplingBottleAllocation));
                                    IList<SamplingBottleAllocation> lstbottleAllocation = uow.GetObjects(SampleBottleAllocationinfo, CriteriaOperator.Parse("Sampling=?", obj.Oid), new SortingCollection(), 0, 0, false, true).Cast<SamplingBottleAllocation>().ToList();
                                    if (lstbottleAllocation.Count > 0)
                                    {
                                        foreach (SamplingBottleAllocation objSamplebottleAll in lstbottleAllocation.ToList())
                                        {
                                            SamplingBottleAllocation objbottleAll = uow.GetObjectByKey<SamplingBottleAllocation>(objSamplebottleAll.Oid);
                                            uow.Delete(objbottleAll);
                                        }
                                    }
                                    uow.Delete(obj);
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletesample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }

                            }
                            else
                            {
                                uow.Delete(obj);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        uow.CommitChanges();
                        View.Refresh();
                        View.RefreshDataSource();
                    }
                }
                else if (View != null && View.Id == "SamplingParameter_ListView_SamplingProposal")
                {
                    List<Tuple<string, Guid, Guid>> lstDeletedSampleParameters = new List<Tuple<string, Guid, Guid>>();
                    bool IsDeleted = false;
                    var os = Application.CreateObjectSpace();
                    foreach (SamplingParameter obj in View.SelectedObjects)
                    {
                        os.Delete(os.GetObject<SamplingParameter>(obj));

                    }
                    IList<SamplingParameter> distinctSample = ((ListView)View).SelectedObjects.Cast<SamplingParameter>().ToList().GroupBy(p => new { p.Testparameter.TestMethod, p.Sampling }).Select(g => g.First()).ToList();
                    foreach (SamplingParameter objs in distinctSample)
                    {
                        SamplingBottleAllocation objAllocation = os.FindObject<SamplingBottleAllocation>(CriteriaOperator.Parse("[Sampling.Oid]=? and [TestMethod.Oid]=?", objs.Sampling.Oid, objs.Testparameter.TestMethod.Oid));
                        if (objAllocation != null)
                        {
                            os.Delete(objAllocation);
                            IsDeleted = true;
                            if (objs.Sampling.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                            {
                                Frame.GetController<AuditlogViewController>().insertauditdata(os, objs.Sampling.SamplingProposal.Oid, OperationType.Deleted, "Sampling Proposal", objs.Sampling.SampleID, "Test", objs.Testparameter.TestMethod.TestName + " | " + objs.Testparameter.Parameter.ParameterName, "", "");
                            }
                        }
                    }
                    if (IsDeleted == true)
                    {
                        os.CommitChanges();

                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                    }
                    View.Refresh();
                    View.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void btnCOCImport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SamplingProposal objsc = (SamplingProposal)View.CurrentObject;
                if (objsc != null && objsc.ProjectID != null)
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objspace, typeof(COCSettings));
                    cs.Criteria["Filter"] = CriteriaOperator.Parse(string.Format("[ProjectID] = '{0}' AND [ClientName] = '{1}' AND ([RetireDate] >= '{2}' Or [RetireDate] Is Null)", objsc.ProjectID.Oid, objsc.ClientName.Oid, DateTime.Today));
                    ListView CreateListView = Application.CreateListView("COCSettings_ListView_SamplingProposal_ImportCOC", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(CreateListView);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.CreatedView.Caption = "Import COC";
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += btncocImporting_Accepting;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("ProjectID must not be empty.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void btncocImporting_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count == 1)
                {
                    bool DBAccess = false;
                    string strjobid = null;
                    IObjectSpace os = Application.CreateObjectSpace();
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    int SampleNo = 0;
                    COCSettings objCOCSettings = (COCSettings)e.AcceptActionArgs.CurrentObject;

                    List<COCSettingsSamples> lstcocSample = View.ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("[COCID] = ?", objCOCSettings.Oid)).ToList();
                    SamplingProposal objsamplecheckin = (SamplingProposal)View.CurrentObject;
                    if (objCOCSettings != null && objsamplecheckin != null)
                    {
                        foreach (ViewItem item in ((DetailView)Application.MainWindow.View).Items.Where(i => i.Id == "RegistrationID"))
                        {
                            if (item.GetType() == typeof(ASPxStringPropertyEditor))
                            {
                                ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                                if (propertyEditor.AllowEdit == true && string.IsNullOrEmpty(objsamplecheckin.RegistrationID))
                                {
                                    Application.ShowViewStrategy.ShowMessage("JobID must not be empty.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                                }
                                else if (propertyEditor.AllowEdit == false)
                                {
                                    //SelectedData sproc = currentSession.ExecuteSproc("GetRegistrationID", new OperandValue("Normal"));
                                    //strjobid = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                                    //objsamplecheckin.RegistrationID = strjobid;
                                    var curdate = DateTime.Now;
                                    strjobid = string.Empty;
                                    int formatlen = 0;
                                    SamplingProposalIDFormat objJDformat = os.FindObject<SamplingProposalIDFormat>(CriteriaOperator.Parse(""));
                                    if (objJDformat!=null)
                                    {
                                        if (objJDformat.Year == YesNoFilter.Yes)
                                        {
                                            strjobid += curdate.ToString(objJDformat.YearFormat.ToString());
                                            formatlen = objJDformat.YearFormat.ToString().Length;
                                        }
                                        if (objJDformat.Month == YesNoFilter.Yes)
                                        {
                                            strjobid += curdate.ToString(objJDformat.MonthFormat.ToUpper());
                                            formatlen = formatlen + objJDformat.MonthFormat.Length;
                                        }
                                        if (objJDformat.Day == YesNoFilter.Yes)
                                        {
                                            strjobid += curdate.ToString(objJDformat.DayFormat);
                                            formatlen = formatlen + objJDformat.DayFormat.Length;
                                        }
                                        CriteriaOperator sam = objJDformat.Prefix == YesNoFilter.Yes ? CriteriaOperator.Parse("Max(SUBSTRING(RegistrationID, " + objJDformat.PrefixValue.ToString().Length + "))") : CriteriaOperator.Parse("Max(SUBSTRING(RegistrationID, 0))");
                                        CriteriaOperator filternew = CriteriaOperator.Parse("[IsAlpacJobid]=1");
                                        string tempid = (Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(SamplingProposal), sam, filternew)) + 1).ToString();
                                        if (tempid != "1")
                                        {
                                            var predate = tempid.Substring(0, formatlen);
                                            if (predate == strjobid)
                                            {
                                                if (objJDformat.Prefix == YesNoFilter.Yes)
                                                {
                                                    if (!string.IsNullOrEmpty(objJDformat.PrefixValue))
                                                    {
                                                        strjobid = objJDformat.PrefixValue + tempid;
                                                    }
                                                }
                                                else
                                                {
                                                    strjobid = tempid;
                                                }
                                            }
                                            else
                                            {
                                                if (objJDformat.Prefix == YesNoFilter.Yes)
                                                {
                                                    if (!string.IsNullOrEmpty(objJDformat.PrefixValue))
                                                    {
                                                        strjobid = objJDformat.PrefixValue + strjobid;
                                                    }
                                                }
                                                if (objJDformat.SequentialNumber > 1)
                                                {
                                                    if (objJDformat.NumberStart > 0)
                                                    {
                                                        strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - objJDformat.NumberStart.ToString().Length)), '0') + objJDformat.NumberStart;
                                                    }
                                                    else
                                                    {
                                                        strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - 1)), '0') + "1";
                                                    }
                                                }
                                                else
                                                {
                                                    if (objJDformat.NumberStart > 0 && objJDformat.NumberStart < 10)
                                                    {
                                                        strjobid = strjobid + objJDformat.NumberStart;
                                                    }
                                                    else
                                                    {
                                                        strjobid = strjobid + "1";
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (objJDformat.Prefix == YesNoFilter.Yes)
                                            {
                                                if (!string.IsNullOrEmpty(objJDformat.PrefixValue))
                                                {
                                                    strjobid = objJDformat.PrefixValue + strjobid;
                                                }
                                            }
                                            if (objJDformat.SequentialNumber > 1)
                                            {
                                                if (objJDformat.NumberStart > 0)
                                                {
                                                    strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - objJDformat.NumberStart.ToString().Length)), '0') + objJDformat.NumberStart;
                                                }
                                                else
                                                {
                                                    strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - 1)), '0') + "1";
                                                }
                                            }
                                            else
                                            {
                                                if (objJDformat.NumberStart > 0 && objJDformat.NumberStart < 10)
                                                {
                                                    strjobid = strjobid + objJDformat.NumberStart;
                                                }
                                                else
                                                {
                                                    strjobid = strjobid + "1";
                                                }
                                            }
                                        }
                                        objsamplecheckin.RegistrationID = strJobID = strjobid; 
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(objsamplecheckin.RegistrationID))
                        {
                            objsamplecheckin.SampleMatries = objCOCSettings.SampleMatries;
                            if (objCOCSettings.TAT != null)
                            {
                                objsamplecheckin.TAT = View.ObjectSpace.GetObjectByKey<TurnAroundTime>(objCOCSettings.TAT.Oid);
                                int tatHour = objCOCSettings.TAT.Count;
                                int Day = 0;
                                if (tatHour >= 24)
                                {
                                    Day = tatHour / 24;
                                    objsamplecheckin.DueDate = AddWorkingDays(DateTime.Now, Day);
                                }
                                else
                                {
                                    objsamplecheckin.DueDate = AddWorkingHours(DateTime.Now, tatHour);
                                }
                            }

                            objsamplecheckin.BalanceID = objCOCSettings.BalanceID;
                            objsamplecheckin.BatchID = objCOCSettings.BatchID;
                            objsamplecheckin.Comment = objCOCSettings.Comment;
                            objsamplecheckin.Remark = objCOCSettings.Comment;
                            objsamplecheckin.IsAlpacJobid = objCOCSettings.IsAlpacCOCid;
                            objsamplecheckin.NoOfSamples = objCOCSettings.NoOfSamples;
                            objsamplecheckin.NPTest = objCOCSettings.NPTest;
                            objsamplecheckin.PackageNo = objCOCSettings.PackageNo;
                            objsamplecheckin.SampleCategory = objCOCSettings.SampleCategory;
                            objsamplecheckin.ReportTemplate = objCOCSettings.ReportTemplate;
                            objsamplecheckin.Test = objCOCSettings.NPTest;
                            objsamplecheckin.TestName = objCOCSettings.NPTest;
                            objsamplecheckin.COCSource =View.ObjectSpace.GetObjectByKey<COCSettings>(objCOCSettings.Oid);
                            ObjectSpace.CommitChanges();
                            if (objCOCSettings.ClientContact != null)
                            {
                                objsamplecheckin.ClientContact = View.ObjectSpace.GetObjectByKey<Contact>(objCOCSettings.ClientContact.Oid);
                            }
                            if (objCOCSettings.ClientName != null)
                            {
                                objsamplecheckin.ClientName = View.ObjectSpace.GetObjectByKey<Customer>(objCOCSettings.ClientName.Oid);
                                objsamplecheckin.ClientAddress = objCOCSettings.ClientAddress;
                                objsamplecheckin.ClientAddress2 = objCOCSettings.ClientAddress2;
                                objsamplecheckin.ClientPhone = objCOCSettings.ClientPhone;
                            }
                            if (objCOCSettings.ProjectID != null)
                            {
                                objsamplecheckin.ProjectID = View.ObjectSpace.GetObjectByKey<Project>(objCOCSettings.ProjectID.Oid);
                                objsamplecheckin.ProjectCity = objCOCSettings.ProjectCity;
                                objsamplecheckin.ProjectOverview = objCOCSettings.ProjectOverview;
                                objsamplecheckin.ProjectSource = objCOCSettings.ProjectSource;
                                if (objCOCSettings.ProjectCategory != null)
                                {
                                    objsamplecheckin.ProjectCategory = View.ObjectSpace.GetObjectByKey<ProjectCategory>(objCOCSettings.ProjectCategory.Oid);
                                }
                            }
                            if (objCOCSettings.QuoteID != null)
                            {
                                objsamplecheckin.QuoteID = View.ObjectSpace.GetObjectByKey<CRMQuotes>(objCOCSettings.QuoteID.Oid);
                                //ListPropertyEditor lstItemPrice = ((DetailView)View).FindItem("SCItemCharges") as ListPropertyEditor;
                                //if (objCOCSettings.QuoteID != null)
                                //{
                                //    CRMQuotes objQuote = uow.GetObjectByKey<CRMQuotes>(objCOCSettings.QuoteID.Oid);
                                //    if (objQuote != null && objQuote.QuotesItemChargePrice.Count > 0)
                                //    {
                                //        foreach (QuotesItemChargePrice obj in objQuote.QuotesItemChargePrice.ToList())
                                //        {
                                //            //SampleCheckinItemChargePricing objNewItem = os.CreateObject<SampleCheckinItemChargePricing>();
                                //            SampleCheckinItemChargePricing objNewItem = new SampleCheckinItemChargePricing(uow);
                                //            objNewItem.ItemPrice = uow.GetObjectByKey<ItemChargePricing>(obj.ItemPrice.Oid);
                                //            objNewItem.Qty = obj.Qty;
                                //            objNewItem.UnitPrice = obj.UnitPrice;
                                //            objNewItem.Amount = obj.Amount;
                                //            objNewItem.FinalAmount = obj.FinalAmount;
                                //            objNewItem.Discount = obj.Discount;
                                //            objNewItem.Description = obj.Description;
                                //            objNewItem.NpUnitPrice = obj.NpUnitPrice;
                                //            objNewItem.SampleCheckin = uow.GetObjectByKey<Samplecheckin>(objsamplecheckin.Oid);
                                //            objsamplecheckin.sa.Add(View.ObjectSpace.GetObject(objNewItem));
                                //            //objNewItem.Save();
                                //        }
                                //    }
                                //}
                            }
                            if (objCOCSettings.Attachment != null && objCOCSettings.Attachment.Count > 0)
                            {
                                List<Attachment> lstAttachment = View.ObjectSpace.GetObjects<Attachment>(CriteriaOperator.Parse("[COCSettings] = ?", objCOCSettings.Oid)).ToList();
                                foreach (Attachment objAttachment in lstAttachment.ToList())
                                {
                                    if (lstAttachment != null)
                                    {
                                        Attachment objNewAttachment = new Attachment(uow);
                                        Attachment oldAttachment = uow.GetObjectByKey<Attachment>(objAttachment.Oid);
                                        if (oldAttachment != null)
                                        {
                                            objNewAttachment.Name = oldAttachment.Name;
                                            objNewAttachment.Category = oldAttachment.Category;
                                            objNewAttachment.Date = oldAttachment.Date;
                                            if (oldAttachment.Operator != null)
                                            {
                                                objNewAttachment.Operator = uow.GetObjectByKey<Employee>(oldAttachment.Operator.Oid);
                                            }
                                            objNewAttachment.Comment = oldAttachment.Comment;
                                            objNewAttachment.SamplingProposal = uow.GetObjectByKey<SamplingProposal>(objsamplecheckin.Oid);
                                            objNewAttachment.Attachments = oldAttachment.Attachments;
                                            objsamplecheckin.Attachments.Add(View.ObjectSpace.GetObject(objNewAttachment));
                                        }
                                    }
                                }
                            }
                            if (objCOCSettings.Note != null && objCOCSettings.Note.Count > 0)
                            {
                                List<Notes> lstNotes = View.ObjectSpace.GetObjects<Notes>(CriteriaOperator.Parse("[COCSettings] = ?", objCOCSettings.Oid)).ToList();
                                foreach (Notes objNotes in lstNotes.ToList())
                                {
                                    Notes oldNotes = uow.GetObjectByKey<Notes>(objNotes.Oid);
                                    if (oldNotes != null)
                                    {
                                        Notes objNewNotes = new Notes(uow);
                                        objNewNotes.Title = oldNotes.Title;
                                        objNewNotes.Attachment = oldNotes.Attachment;
                                        objNewNotes.Text = oldNotes.Text;
                                        if (oldNotes.Author != null)
                                        {
                                            objNewNotes.Author = uow.GetObjectByKey<Employee>(oldNotes.Author.Oid);
                                        }
                                        objNewNotes.Date = oldNotes.Date;
                                        objNewNotes.SamplingProposal = uow.GetObjectByKey<SamplingProposal>(objsamplecheckin.Oid);
                                        objNewNotes.FollowUpDate = oldNotes.FollowUpDate;
                                        objsamplecheckin.Notes.Add(View.ObjectSpace.GetObject(objNewNotes));
                                    }
                                }
                            }
                            foreach (COCSettingsSamples cocSS in lstcocSample.OrderBy(i => i.SampleNo).ToList())
                            {
                                Sampling objSLNew = new Sampling(uow);
                                objSLNew.SamplingProposal = uow.GetObjectByKey<SamplingProposal>(objsamplecheckin.Oid);
                                if (objSLNew != null)
                                {
                                    if (DBAccess == false)
                                    {
                                        SelectedData sproc = currentSession.ExecuteSproc("GetSamplingSampleID", new OperandValue(objsamplecheckin.RegistrationID.ToString()));
                                        if (sproc.ResultSet[1].Rows[0].Values[0] != null)
                                        {
                                            objSMInfo.SampleID = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                                            SampleNo = Convert.ToInt32(objSMInfo.SampleID);
                                            DBAccess = true;
                                        }
                                        else
                                        {
                                            return;
                                        }
                                    }
                                    objSLNew.SampleNo = SampleNo;
                                    objSLNew.ClientSampleID = cocSS.ClientSampleID;
                                    objSLNew.Test = true;
                                    if (cocSS.VisualMatrix != null)
                                    {
                                        objSLNew.VisualMatrix = uow.GetObjectByKey<VisualMatrix>(cocSS.VisualMatrix.Oid);
                                    }
                                    if (cocSS.SampleType != null)
                                    {
                                        objSLNew.SampleType = uow.GetObjectByKey<SampleType>(cocSS.SampleType.Oid);
                                    }
                                    objSLNew.Qty = cocSS.Qty;
                                    if (cocSS.Storage != null)
                                    {
                                        objSLNew.Storage = uow.GetObjectByKey<Storage>(cocSS.Storage.Oid);
                                    }
                                    objSLNew.Preservetives = cocSS.Preservetives;
                                    objSLNew.SamplingLocation = cocSS.SamplingLocation;
                                    if (cocSS.QCType != null)
                                    {
                                        objSLNew.QCType = uow.GetObjectByKey<QCType>(cocSS.QCType.Oid);
                                    }
                                    if (cocSS.QCSource != null)
                                    {
                                        objSLNew.QCSource = uow.GetObjectByKey<Sampling>(cocSS.QCSource.Oid);
                                    }
                                    if (cocSS.Client != null)
                                    {
                                        objSLNew.Client = uow.GetObjectByKey<Customer>(cocSS.Client.Oid);
                                    }
                                    if (cocSS.Department != null)
                                    {
                                        objSLNew.Department = uow.GetObjectByKey<Department>(cocSS.Department.Oid);
                                    }
                                    if (cocSS.ProjectID != null)
                                    {
                                        objSLNew.ProjectID = uow.GetObjectByKey<Project>(cocSS.ProjectID.Oid);
                                    }
                                    if (cocSS.PreserveCondition != null)
                                    {
                                        objSLNew.PreserveCondition = uow.GetObjectByKey<PreserveCondition>(cocSS.PreserveCondition.Oid);
                                    }
                                    if (cocSS.StorageID != null)
                                    {
                                        objSLNew.StorageID = uow.GetObjectByKey<Storage>(cocSS.StorageID.Oid);
                                    }
                                    if(cocSS.SiteName!=null)
                                    {
                                        objSLNew.StationLocation = uow.GetObjectByKey<SampleSites>(cocSS.SiteName.Oid);
                                    }
                                    objSLNew.FlowRate = cocSS.FlowRate;
                                    objSLNew.TimeStart = cocSS.TimeStart;
                                    objSLNew.TimeEnd = cocSS.TimeEnd;
                                    objSLNew.Time = cocSS.Time;
                                    objSLNew.Volume = cocSS.Volume;
                                    objSLNew.Address = cocSS.Address;
                                    objSLNew.AreaOrPerson = cocSS.AreaOrPerson;
                                    if (cocSS.BalanceID != null)
                                    {
                                        objSLNew.BalanceID = uow.GetObjectByKey<Modules.BusinessObjects.Assets.Labware>(cocSS.BalanceID.Oid);
                                    }
                                    objSLNew.AssignTo = cocSS.AssignTo;
                                    objSLNew.Barp = cocSS.Barp;
                                    objSLNew.BatchID = cocSS.BatchID;
                                    objSLNew.BatchSize = cocSS.BatchSize;
                                    objSLNew.BatchSize_pc = cocSS.BatchSize_pc;
                                    objSLNew.BatchSize_Units = cocSS.BatchSize_Units;
                                    objSLNew.Blended = cocSS.Blended;
                                    objSLNew.BottleQty = cocSS.BottleQty;
                                    objSLNew.BuriedDepthOfGroundWater = cocSS.BuriedDepthOfGroundWater;
                                    objSLNew.ChlorineFree = cocSS.ChlorineFree;
                                    objSLNew.ChlorineTotal = cocSS.ChlorineTotal;
                                    objSLNew.City = cocSS.City;
                                    objSLNew.CompositeQty = cocSS.CompositeQty;
                                    objSLNew.DateEndExpected = cocSS.DateEndExpected;
                                    objSLNew.DateStartExpected = cocSS.DateStartExpected;
                                    objSLNew.ClientSampleID = cocSS.ClientSampleID;
                                    objSLNew.Comment = cocSS.Comment;
                                    objSLNew.Containers = cocSS.Containers;
                                    objSLNew.Depth = cocSS.Depth;
                                    objSLNew.Description = cocSS.Description;
                                    objSLNew.DischargeFlow = cocSS.DischargeFlow;
                                    objSLNew.DischargePipeHeight = cocSS.DischargePipeHeight;
                                    objSLNew.DO = cocSS.DO;
                                    objSLNew.Emission = cocSS.Emission;
                                    objSLNew.EndOfRoad = cocSS.EndOfRoad;
                                    objSLNew.EquipmentModel = cocSS.EquipmentModel;
                                    objSLNew.EquipmentName = cocSS.EquipmentName;
                                    objSLNew.FacilityID = cocSS.FacilityID;
                                    objSLNew.FacilityName = cocSS.FacilityName;
                                    objSLNew.FacilityType = cocSS.FacilityType;
                                    objSLNew.FinalForm = cocSS.FinalForm;
                                    objSLNew.FinalPackaging = cocSS.FinalPackaging;
                                    objSLNew.FlowRate = cocSS.FlowRate;
                                    objSLNew.FlowRateCubicMeterPerHour = cocSS.FlowRateCubicMeterPerHour;
                                    objSLNew.FlowRateLiterPerMin = cocSS.FlowRateLiterPerMin;
                                    objSLNew.FlowVelocity = cocSS.FlowVelocity;
                                    objSLNew.ForeignMaterial = cocSS.ForeignMaterial;
                                    objSLNew.Frequency = cocSS.Frequency;
                                    objSLNew.GISStatus = cocSS.GISStatus;
                                    objSLNew.GravelContent = cocSS.GravelContent;
                                    objSLNew.GrossWeight = cocSS.GrossWeight;
                                    objSLNew.GroupSample = cocSS.GroupSample;
                                    objSLNew.Hold = cocSS.Hold;
                                    objSLNew.Humidity = cocSS.Humidity;
                                    objSLNew.IceCycle = cocSS.IceCycle;
                                    objSLNew.Increments = cocSS.Increments;
                                    objSLNew.Interval = cocSS.Interval;
                                    objSLNew.IsActive = cocSS.IsActive;
                                   // objSLNew.IsNotTransferred = cocSS.IsNotTransferred;
                                    objSLNew.ItemName = cocSS.ItemName;
                                    objSLNew.KeyMap = cocSS.KeyMap;
                                    objSLNew.LicenseNumber = cocSS.LicenseNumber;
                                    objSLNew.ManifestNo = cocSS.ManifestNo;
                                    objSLNew.MonitoryingRequirement = cocSS.MonitoryingRequirement;
                                    objSLNew.NoOfCollectionsEachTime = cocSS.NoOfCollectionsEachTime;
                                    objSLNew.NoOfPoints = cocSS.NoOfPoints;
                                    objSLNew.Notes = cocSS.Notes;
                                    objSLNew.OriginatingEntiry = cocSS.OriginatingEntiry;
                                    objSLNew.OriginatingLicenseNumber = cocSS.OriginatingLicenseNumber;
                                    objSLNew.PackageNumber = cocSS.PackageNumber;
                                    objSLNew.ParentSampleDate = cocSS.ParentSampleDate;
                                    objSLNew.ParentSampleID = cocSS.ParentSampleID;
                                    objSLNew.PiecesPerUnit = cocSS.PiecesPerUnit;
                                    objSLNew.Preservetives = cocSS.Preservetives;
                                    objSLNew.ProjectName = cocSS.ProjectName;
                                    objSLNew.PurifierSampleID = cocSS.PurifierSampleID;
                                    objSLNew.PWSID = cocSS.PWSID;
                                    objSLNew.PWSSystemName = cocSS.PWSSystemName;
                                    objSLNew.RegionNameOfSection = cocSS.RegionNameOfSection;
                                    objSLNew.RejectionCriteria = cocSS.RejectionCriteria;
                                    objSLNew.RepeatLocation = cocSS.RepeatLocation;
                                    objSLNew.RetainedWeight = cocSS.RetainedWeight;
                                    objSLNew.RiverWidth = cocSS.RiverWidth;
                                    objSLNew.RushSample = cocSS.RushSample;
                                    objSLNew.SampleAmount = cocSS.SampleAmount;
                                    objSLNew.SampleCondition = cocSS.SampleCondition;
                                    objSLNew.SampleDescription = cocSS.SampleDescription;
                                    objSLNew.SampleImage = cocSS.SampleImage;
                                    objSLNew.SampleName = cocSS.SampleName;
                                    objSLNew.SamplePointID = cocSS.SamplePointID;
                                    objSLNew.SamplePointType = cocSS.SamplePointType;
                                    objSLNew.SampleSource = cocSS.SampleSource;
                                    objSLNew.SampleTag = cocSS.SampleTag;
                                    objSLNew.SampleWeight = cocSS.SampleWeight;
                                    objSLNew.SamplingAddress = cocSS.SamplingAddress;
                                    objSLNew.SamplingEquipment = cocSS.SamplingEquipment;
                                    objSLNew.SamplingLocation = cocSS.SamplingLocation;
                                    objSLNew.SamplingProcedure = cocSS.SamplingProcedure;
                                    objSLNew.SequenceTestSampleID = cocSS.SequenceTestSampleID;
                                    objSLNew.SequenceTestSortNo = cocSS.SequenceTestSortNo;
                                    objSLNew.ServiceArea = cocSS.ServiceArea;
                                    objSLNew.SiteCode = cocSS.SiteCode;
                                    objSLNew.SiteDescription = cocSS.SiteDescription;
                                    objSLNew.SiteID = cocSS.SiteID;
                                    objSLNew.SiteNameArchived = cocSS.SiteNameArchived;
                                    objSLNew.SiteUserDefinedColumn1 = cocSS.SiteUserDefinedColumn1;
                                    objSLNew.SiteUserDefinedColumn2 = cocSS.SiteUserDefinedColumn2;
                                    objSLNew.SiteUserDefinedColumn3 = cocSS.SiteUserDefinedColumn3;
                                    objSLNew.SubOut = cocSS.SubOut;
                                    objSLNew.SystemType = cocSS.SystemType;
                                    objSLNew.TargetMGTHC_CBD_mg_pc = cocSS.TargetMGTHC_CBD_mg_pc;
                                    objSLNew.TargetMGTHC_CBD_mg_unit = cocSS.TargetMGTHC_CBD_mg_unit;
                                    objSLNew.TargetPotency = cocSS.TargetPotency;
                                    objSLNew.TargetUnitWeight_g_pc = cocSS.TargetUnitWeight_g_pc;
                                    objSLNew.TargetUnitWeight_g_unit = cocSS.TargetUnitWeight_g_unit;
                                    objSLNew.TargetWeight = cocSS.TargetWeight;
                                    objSLNew.Time = cocSS.Time;
                                    objSLNew.TimeEnd = cocSS.TimeEnd;
                                    objSLNew.TimeStart = cocSS.TimeStart;
                                    objSLNew.TotalSamples = cocSS.TotalSamples;
                                    objSLNew.TotalTimes = cocSS.TotalTimes;
                                    if (cocSS.TtimeUnit != null)
                                    {
                                        objSLNew.TtimeUnit = uow.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(cocSS.TtimeUnit.Oid);
                                    }
                                    objSLNew.WaterType = cocSS.WaterType;
                                    objSLNew.ZipCode = cocSS.ZipCode;
                                    //if (cocSS.ModifiedBy != null)
                                    //{
                                    //    objSLNew.ModifiedBy = os.GetObjectByKey<Modules.BusinessObjects.Hr.CustomSystemUser>(cocSS.ModifiedBy.Oid);
                                    //}
                                    //objSLNew.ModifiedDate = cocSS.ModifiedDate;
                                    objSLNew.Comment = cocSS.Comment;
                                    objSLNew.Latitude = cocSS.Latitude;
                                    objSLNew.Longitude = cocSS.Longitude;
                                    List<COCSettingsTest> lstcocTest = View.ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[COCSettingsSamples] = ?", cocSS.Oid)).ToList();
                                    foreach (COCSettingsTest cocT in lstcocTest.ToList())
                                    {
                                        SamplingParameter objSP = new SamplingParameter(uow);
                                        if (objSP != null)
                                        {
                                            if (cocT.Testparameter != null)
                                            {
                                                objSP.Testparameter = uow.GetObjectByKey<Testparameter>(cocT.Testparameter.Oid);
                                            }
                                            if (cocT.COCSettingsSamples != null)
                                            {
                                                objSP.Sampling = objSLNew;
                                            }
                                            if (objCOCSettings.TAT != null)
                                            {
                                                objSP.TAT = uow.GetObjectByKey<TurnAroundTime>(objCOCSettings.TAT.Oid);
                                            }
                                        }
                                        objSP.Save();
                                    }
                                    objSLNew.Save();
                                    SampleNo++;
                                    List<COCSettingsBottleAllocation> lstcocBottle = View.ObjectSpace.GetObjects<COCSettingsBottleAllocation>(CriteriaOperator.Parse("[COCSettingsRegistration] = ?", cocSS.Oid)).ToList();
                                    foreach (COCSettingsBottleAllocation cocBA in lstcocBottle.Where(i=>i.TestMethod.IsFieldTest!=true).ToList())
                                    {
                                        SamplingBottleAllocation smplnew = new SamplingBottleAllocation(uow);
                                        smplnew.Sampling = objSLNew;
                                        smplnew.TestMethod = uow.GetObjectByKey<TestMethod>(cocBA.TestMethod.Oid);
                                        smplnew.BottleID = cocBA.BottleID;
                                        if (cocBA.Containers != null)
                                        {
                                            smplnew.Containers = uow.GetObjectByKey<Modules.BusinessObjects.Setting.Container>(cocBA.Containers.Oid);
                                        }
                                        if (cocBA.Preservative != null)
                                        {
                                            smplnew.Preservative = uow.GetObjectByKey<Preservative>(cocBA.Preservative.Oid);
                                        }
                                        if (cocBA.StorageID != null)
                                        {
                                            smplnew.StorageID = uow.GetObjectByKey<Storage>(cocBA.StorageID.Oid);
                                        }
                                        if (cocBA.StorageCondition != null)
                                        {
                                            smplnew.StorageCondition = uow.GetObjectByKey<PreserveCondition>(cocBA.StorageCondition.Oid);
                                        }
                                        smplnew.Save();
                                    }
                                }
                                uow.CommitChanges();
                            }
                            objSMInfo.boolCopySamples = false;
                            ObjectSpace.CommitChanges();
                            ObjectSpace.Refresh();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "COCSettingsImportSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                else if (e.AcceptActionArgs.SelectedObjects.Count > 1)
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void btnSampleTest_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (ObjectSpace.IsModified)
                {
                    ObjectSpace.CommitChanges();
                }
                if (!objSMInfo.isNoOfSampleDisable)
                {
                    InsertSamplesInSampleLogin();
                }
                SamplingProposal objsmplcheckin = (SamplingProposal)View.CurrentObject;
                if (objsmplcheckin != null)
                {
                    objSMInfo.strJobID = objsmplcheckin.RegistrationID;
                    string[] strvmarr = objsmplcheckin.SampleMatries.Split(';');
                    objSMInfo.lstSRvisualmat = new List<VisualMatrix>();
                    foreach (string strvmoid in strvmarr.ToList())
                    {
                        VisualMatrix lstvmatobj = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strvmoid)));
                        if (lstvmatobj != null)
                        {
                            objSMInfo.lstSRvisualmat.Add(lstvmatobj);
                        }
        }
                    CollectionSource cs = new CollectionSource(View.ObjectSpace, typeof(SamplingParameter));
                    cs.Criteria["filter"] = CriteriaOperator.Parse("[Sampling.SamplingProposal.RegistrationID] = ? AND [Sampling.SamplingProposal.GCRecord] is NULL", objSMInfo.strJobID);
                    ListView dvbottleAllocation = Application.CreateListView("SamplingParameter_ListView_SamplingProposal", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(dvbottleAllocation);
                    showViewParameters.CreatedView = dvbottleAllocation;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.AcceptAction.Active["OkayBtn"] = false;
                    dc.CancelAction.Active["CancelBtn"] = false;
                    dc.CloseOnCurrentObjectProcessing = false;
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

        private void btnBottleAllocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (ObjectSpace.IsModified)
                {
                    ObjectSpace.CommitChanges();
                }
                if (!objSMInfo.isNoOfSampleDisable)
                {
                    InsertSamplesInSampleLogin();
                }
                SamplingProposal objsmplcheckin = (SamplingProposal)View.CurrentObject;
                if (objsmplcheckin != null)
                {
                    objSMInfo.strJobID = objsmplcheckin.RegistrationID;
                    string[] strvmarr = objsmplcheckin.SampleMatries.Split(';');
                    objSMInfo.lstSRvisualmat = new List<VisualMatrix>();
                    foreach (string strvmoid in strvmarr.ToList())
                    {
                        VisualMatrix lstvmatobj = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strvmoid)));
                        if (lstvmatobj != null)
                        {
                            objSMInfo.lstSRvisualmat.Add(lstvmatobj);
                        }
                    }
                    SamplingBottleAllocation newsmplbtlalloc = View.ObjectSpace.CreateObject<SamplingBottleAllocation>();
                    newsmplbtlalloc.DefaultContainerQty = 1;
                    DetailView dvbottleAllocation = Application.CreateDetailView(View.ObjectSpace, "SamplingBottleAllocation_DetailView_Sampling", false, newsmplbtlalloc);
                    ShowViewParameters showViewParameters = new ShowViewParameters(dvbottleAllocation);
                    showViewParameters.CreatedView = dvbottleAllocation;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.AcceptAction.Active["OkayBtn"] = false;
                    dc.CancelAction.Active["CancelBtn"] = false;
                    dc.CloseOnCurrentObjectProcessing = false;
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

        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (Frame is DevExpress.ExpressApp.Web.PopupWindow)
                {
                    DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                    if (popupWindow != null)
                    {
                        popupWindow.RefreshParentWindowOnCloseButtonClick = true;
                        DialogController dc = popupWindow.GetController<DialogController>();
                    }
                }
                if (View.Id== "SamplingProposal_DetailView")
                {
                    if(((DetailView)View).ViewEditMode==ViewEditMode.View)
                    {
                        SPSaveAs.Active["valSaveAs"] =false;
                        SPSubmit.Active["valSubmit"] =false;
                        //objPmsInfo.SamplingProposalIsWrite = false;
                    }
                    SamplingProposal obj = (SamplingProposal)View.CurrentObject;
                    objAuditInfo.currentViewOid = obj.Oid;
                    ObjectSpace.Committing += ObjectSpace_Committing;
                    DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule += RuleSet_CustomNeedToValidateRule;
                    View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    objSMInfo.CurrentJob = View.CurrentObject as SamplingProposal;
                    objSMInfo.NewClient = null;
                    objSMInfo.NewProject = null;
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Execute += SaveAction_Execute;
                        modificationController.SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
                        modificationController.SaveAndNewAction.Executing += SaveAndNewAction_Executing;
                    }
                    Frame.GetController<WebModificationsController>().EditAction.Executed += EditAction_Executed;
                    if (View.ObjectSpace.IsNewObject(obj))
                    {
                        if (obj.TAT != null)
                        {
                            int tatHour = obj.TAT.Count;
                            int Day = 0;
                            if (tatHour >= 24)
                            {
                                Day = tatHour / 24;
                                obj.DueDate = AddWorkingDays(obj.RecievedDate, Day);
                            }
                            else
                            {
                                obj.DueDate = AddWorkingHours(obj.RecievedDate, tatHour);
                            }
                        }
                    }
                    ViewItem JobID = ((DetailView)View).FindItem("RegistrationID");
                    JobIDFormat objformat = View.ObjectSpace.FindObject<JobIDFormat>(CriteriaOperator.Parse(""));
                    if (objformat != null && JobID != null)
                    {
                        ASPxStringPropertyEditor editor = (ASPxStringPropertyEditor)JobID;
                        if (editor != null)
                        {
                            if (objformat.Dynamic == false && obj.Status == RegistrationStatus.PendingSubmission)
                            {
                                editor.AllowEdit.SetItemValue("enb", true);
                            }
                            else
                            {
                                editor.AllowEdit.SetItemValue("enb", false);
                                editor.NullText = "Autogenerate";
                            }
                        }
                    }
                    collector = ((DetailView)View).FindItem("Collector") as ASPxLookupPropertyEditor;
                    if (collector != null)
                    {
                        collector.ValueRead += Collector_ValueRead;
                    }
                    if (!View.ObjectSpace.IsNewObject(obj))
                    {
                        List<Sampling> lstSamples = View.ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("SamplingProposal.Oid=?", obj.Oid)).ToList();
                        if (lstSamples.Count == 0)
                        {
                            obj.NoOfSamples = 1;
                        }
                    }
                }
                else if (View.Id == "SamplingTest")
                {
                    staticText = (StaticText)((DashboardView)this.View).FindItem("SampleMatrix");
                }
                else if (View != null && View.Id == "SamplingProposal_ListView")
                {
                    objPmsInfo.SamplingProposalIsWrite = false;
                    objPmsInfo.SamplingProposalIsDelete = false;
                    Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                    if (user.Roles != null && user.Roles.Count > 0)
                    {
                        if (user.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPmsInfo.SamplingProposalIsWrite = true;
                            objPmsInfo.SamplingProposalIsDelete = true;
                            objPmsInfo.SamplingProposalIsCreate = true;
                        }
                        else
                        {
                            foreach (RoleNavigationPermission role in user.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationModelClass == View.ObjectTypeInfo.FullName && i.Create == true) != null)
                                {
                                    objPmsInfo.SamplingProposalIsWrite = true;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationModelClass == View.ObjectTypeInfo.FullName && i.Write == true) != null)
                                {
                                    objPmsInfo.SamplingProposalIsWrite = true;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationModelClass == View.ObjectTypeInfo.FullName && i.Delete == true) != null)
                                {
                                    objPmsInfo.SamplingProposalIsDelete = true;
                                }
                            }
                        }
                    }
                    SPSaveAs.Active["valSaveAs"] = objPmsInfo.SamplingProposalIsWrite;
                    SPSubmit.Active["valSubmit"] = objPmsInfo.SamplingProposalIsWrite;
                    if (View.CurrentObject != null)
                    {
                        objSMInfo.CurrentJob = View.CurrentObject as SamplingProposal;
                        objSMInfo.NewClient = null;
                        objSMInfo.NewProject = null;
                    }
                    if (SamplingProposalDateFilterAction != null && SamplingProposalDateFilterAction.SelectedItem == null)
                    {
                        SamplingProposalDateFilterAction.SelectedItem = SamplingProposalDateFilterAction.Items[0];
                        ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[RecievedDate] >= ? and [RecievedDate] <= ?", DateTime.Today.AddMonths(-3), DateTime.Today);
                        SamplingProposalDateFilterAction.SelectedItemChanged += DateFilterAction_SamplingProposal_SelectedItemChanged;
                    }
                }
                else if (View.Id == "AnalysisPricing_ListView_Quotes_SamplingProposal")
                {
                   SamplingProposal objsamplecheckin = ((SamplingProposal)Application.MainWindow.View.CurrentObject);
                    string straMatrix = string.Empty;
                    if (objsamplecheckin.SampleMatries != null)
                    {
                        strSampleMatrix = objsamplecheckin.SampleMatries.Split(';');
                        List<Matrix> lstSRvisualmat = new List<Matrix>();
                        foreach (string strvmoid in strSampleMatrix.ToList())
                        {
                            VisualMatrix lstvmatobj = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strvmoid)));
                            if (lstvmatobj != null)
                            {
                                lstSRvisualmat.Add(lstvmatobj.MatrixName);
                            }
                        }
                        straMatrix = string.Join("','", lstSRvisualmat.Select(i => i.MatrixName).ToList().ToArray());
                        straMatrix = "'" + straMatrix + "'";
                    }
                  ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(string.Format("[Matrix.MatrixName] in ( {0} )", straMatrix));
                   
                }
                else if(View.Id== "Sampling_ListView_SamplingProposal")
                {
                    ((XPObjectSpace)this.ObjectSpace).Session.TrackPropertiesModifications = true;
                    View.ObjectSpace.Committing += ObjectSpace_Committing;
                }
                else if (View.Id == "SamplingProposal_DetailView_CopyRecurrence")
                {
                    ObjectSpace.Committed += ObjectSpace_Committed;
                    View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                else if(View.Id== "SamplingProposal_ListView_History")
                {
                    if (SamplingProposalHistoryDateFilterAction != null && SamplingProposalHistoryDateFilterAction.SelectedItem == null)
                    {
                        SamplingProposalHistoryDateFilterAction.SelectedItem = SamplingProposalDateFilterAction.Items[0];
                        ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[RecievedDate] >= ? and [RecievedDate] <= ?", DateTime.Today.AddMonths(-3), DateTime.Today);
                        SamplingProposalHistoryDateFilterAction.SelectedItemChanged += DateFilterAction_History_SamplingProposal_SelectedItemChanged;
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
          
        }

        private void DateFilterAction_History_SamplingProposal_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "SamplingProposal_ListView_History")
                {
                    DateTime strDateFilter = DateTime.MinValue;
                    if (SamplingProposalHistoryDateFilterAction != null && SamplingProposalHistoryDateFilterAction.SelectedItem != null)
                    {
                        if (SamplingProposalHistoryDateFilterAction.SelectedItem.Id == "3M")
                        {
                            strDateFilter = DateTime.Today.AddMonths(-3);
                        }
                        else if (SamplingProposalHistoryDateFilterAction.SelectedItem.Id == "6M")
                        {
                            strDateFilter = DateTime.Today.AddMonths(-6);
                        }
                        else if (SamplingProposalHistoryDateFilterAction.SelectedItem.Id == "1Y")
                        {
                            strDateFilter = DateTime.Today.AddYears(-1);
                        }
                        else if (SamplingProposalHistoryDateFilterAction.SelectedItem.Id == "2Y")
                        {
                            strDateFilter = DateTime.Today.AddYears(-2);
                        }
                    }
                    if (strDateFilter != DateTime.MinValue)
                    {
                        ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[RecievedDate] >= ? and [RecievedDate] <= ?", strDateFilter, DateTime.Now);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            if(View != null )
            {
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

            }
        }

        private void DateFilterAction_SamplingProposal_SelectedItemChanged(object sender, EventArgs e)
        {
           try
            {
                if (View!=null && View.Id == "SamplingProposal_ListView")
                {
                    DateTime srDateFilter = DateTime.MinValue;
                    if (SamplingProposalDateFilterAction != null && SamplingProposalDateFilterAction.SelectedItem != null)
                    {
                        if (SamplingProposalDateFilterAction.SelectedItem.Id == "3M")
                        {
                            srDateFilter = DateTime.Today.AddMonths(-3);
                        }
                        else if (SamplingProposalDateFilterAction.SelectedItem.Id == "6M")
                        {
                            srDateFilter = DateTime.Today.AddMonths(-6);
                        }
                        else if (SamplingProposalDateFilterAction.SelectedItem.Id == "1Y")
                        {
                            srDateFilter = DateTime.Today.AddYears(-1);
                        }
                        else if (SamplingProposalDateFilterAction.SelectedItem.Id == "2Y")
                        {
                            srDateFilter = DateTime.Today.AddYears(-2);
                        }
                    }
                    if (srDateFilter != DateTime.MinValue)
                    {
                        ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[RecievedDate] >= ? and [RecievedDate] <= ?", srDateFilter,DateTime.Now);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void EditAction_Executed(object sender, ActionBaseEventArgs e)
        {
           try
            {
                SPSaveAs.Active["valSaveAs"] = true;
                SPSubmit.Active["valSubmit"] = true;
                //objPmsInfo.SamplingProposalIsWrite = false;
                Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                if (user.Roles != null && user.Roles.Count > 0)
                {
                    if (user.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        objPmsInfo.SamplingProposalIsWrite = true;
                        objPmsInfo.SamplingProposalIsDelete = true;
                        objPmsInfo.SamplingProposalIsCreate = true;
                    }
                    else
                    {
                        foreach (RoleNavigationPermission role in user.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationModelClass == View.ObjectTypeInfo.FullName && i.Create == true) != null)
                            {
                                objPmsInfo.SamplingProposalIsWrite = true;
                            }
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationModelClass == View.ObjectTypeInfo.FullName && i.Write == true) != null)
                            {
                                objPmsInfo.SamplingProposalIsWrite = true;
                            }
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationModelClass == View.ObjectTypeInfo.FullName && i.Delete == true) != null)
                            {
                                objPmsInfo.SamplingProposalIsDelete = true;
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

        private void Collector_ValueRead(object sender, EventArgs e)
        {
            try
            {
                SamplingProposal obj = (SamplingProposal)Application.MainWindow.View.CurrentObject;
                if (obj != null && obj.ClientName != null)
                {
                    if (collector.Frame != null)
                    {
                        collector.Frame.GetController<NewObjectViewController>().NewObjectAction.Active["asd"] = true;
                    }
                }
                else
                {
                    if (collector.Frame != null)
                    {
                        collector.Frame.GetController<NewObjectViewController>().NewObjectAction.Active["asd"] = false;
                    }
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
            try
            {
                e.PopupControl.CustomizePopupWindowSize += PopupControl_CustomizePopupWindowSize;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void PopupControl_CustomizePopupWindowSize(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame != null)
                {
                    if (e.PopupFrame.View.Id == "SampleLogIn_ListView_Copy_SampleRegistration" || e.PopupFrame.View.Id == "SamplingProposal_SampleLogin")
                    {
                        e.PopupFrame.View.Caption = "Samples";
                        e.Width = new System.Web.UI.WebControls.Unit(1200);
                        e.Height = new System.Web.UI.WebControls.Unit(700);
                        e.Handled = true;
                    }
                    if (e.PopupFrame.View.Id == "SamplingParameter_ListView_SamplingProposal")
                    {
                        e.PopupFrame.View.Caption = "Tests";
                        e.Width = new System.Web.UI.WebControls.Unit(1200);
                        e.Height = new System.Web.UI.WebControls.Unit(700);
                        e.Handled = true;
                    }
                    if (e.PopupFrame.View.Id == "SamplingBottleAllocation_DetailView_Sampling")
                    {
                        e.PopupFrame.View.Caption = "Containers";
                        e.Width = new System.Web.UI.WebControls.Unit(1210);
                        e.Height = new System.Web.UI.WebControls.Unit(520);
                        e.Handled = true;
                    }
                    if (e.PopupFrame.View.Id == "SamplingTest")
                    {
                        e.Width = new System.Web.UI.WebControls.Unit(1500);
                        e.Height = new System.Web.UI.WebControls.Unit(648);
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
        private void SaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SamplingProposal_DetailView")
                {
                    SamplingProposal objSampleCheckIn = (SamplingProposal)Application.MainWindow.View.CurrentObject;
                    if (!objSMInfo.isNoOfSampleDisable)
                    {
                        InsertSamplesInSampleLogin();
                        foreach (ViewItem item in ((DetailView)View).Items.Where(i => i.Id == "NoOfSamples"))
                        {
                            if (item.GetType() == typeof(ASPxIntPropertyEditor))
                            {
                                ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    List<Sampling> lstSamples = View.ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("SamplingProposal.Oid=?", objSampleCheckIn.Oid)).ToList();
                                    if (lstSamples.Count > 0)
                                    {
                                        propertyEditor.Editor.ForeColor = Color.Black;
                                        propertyEditor.Editor.BackColor = Color.White;
                                        propertyEditor.AllowEdit.SetItemValue("stat", false);
                                        objSMInfo.isNoOfSampleDisable = true;
                                    }
                                }
                            }
                        }
                    }
                    if(objSampleCheckIn.RetiredBy!=null && objSampleCheckIn.RetireDate!=DateTime.MinValue)
                    {
                        objSampleCheckIn.Status = RegistrationStatus.Retired;
                        View.ObjectSpace.CommitChanges();
                    }
                    int sampleno = View.ObjectSpace.GetObjectsCount(typeof(Sampling), CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", objSampleCheckIn.Oid));
                    SamplingSample.Caption = "Samples" + "(" + sampleno + ")";
                    Application.MainWindow.View.ObjectSpace.Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        private void SaveAndCloseAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "SamplingProposal_DetailView")
                {
                    if (View.ObjectSpace.ModifiedObjects.Count > 0)
                    {
                        View.ObjectSpace.CommitChanges();
                    }
                    SamplingProposal objSampleCheckIn = (SamplingProposal)Application.MainWindow.View.CurrentObject;
                    if (!objSMInfo.isNoOfSampleDisable)
                    {
                        InsertSamplesInSampleLogin();
                    }
                    if (objSampleCheckIn.RetiredBy != null && objSampleCheckIn.RetireDate != DateTime.MinValue)
                    {
                        objSampleCheckIn.Status = RegistrationStatus.Retired;
                    }
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAndNewAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "SamplingProposal_DetailView")
                {
                    if (View.ObjectSpace.ModifiedObjects.Count > 0)
                    {
                        View.ObjectSpace.CommitChanges();
                    }
                    SamplingProposal objSampleCheckIn = (SamplingProposal)Application.MainWindow.View.CurrentObject;
                    if (!objSMInfo.isNoOfSampleDisable)
                    {
                        InsertSamplesInSampleLogin();
                    }
                    if (objSampleCheckIn.RetiredBy != null && objSampleCheckIn.RetireDate != DateTime.MinValue)
                    {
                        objSampleCheckIn.Status = RegistrationStatus.Retired;
                    }
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
           try
            {
                if (base.View != null && e.NewValue != e.OldValue && View.Id== "SamplingProposal_DetailView")
                {
                    if (View != null && View.CurrentObject == e.Object)
                    {
                        SamplingProposal objsamplecheckin = (SamplingProposal)e.Object;
                        objSMInfo.CurrentJob = objsamplecheckin;
                        if (e.PropertyName != "NoOfSamples")
                        {
                            objSMInfo.IsSamplePopupClose = false;
                        }
                        if (e.PropertyName == "ClientName")
                        {
                            if (e.NewValue != null && objsamplecheckin.ClientName != null)
                            {
                                objInfo.ClientName = objsamplecheckin.ClientName.CustomerName;
                                if (objsamplecheckin.Collector != null && objsamplecheckin.Collector.CustomerName != null && objsamplecheckin.Collector.CustomerName.Oid != objsamplecheckin.ClientName.Oid)
                                {
                                    objsamplecheckin.Collector = null;
                                }
                                else if (objsamplecheckin.Collector != null && objsamplecheckin.Collector.CustomerName == null)
                                {
                                    objsamplecheckin.Collector = null;
                                }
                            }
                            else
                            {
                                objInfo.ClientName = null;
                                objsamplecheckin.Collector = null;
                            }
                            objsamplecheckin.ContactName = null;
                            objsamplecheckin.ProjectID = null;
                            ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("ProjectID") as ASPxGridLookupPropertyEditor;
                            if (objsamplecheckin.ClientName != null && propertyEditor!=null)
                            {
                                propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("[customername.Oid] = ? ", objsamplecheckin.ClientName.Oid);
                            }
                            else
                            {
                                propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                            }
                            propertyEditor.Refresh();
                            propertyEditor.RefreshDataSource();

                            
                            ASPxGridLookupPropertyEditor peQuoteID = ((DetailView)View).FindItem("QuoteID") as ASPxGridLookupPropertyEditor;
                            if (peQuoteID != null && peQuoteID.GridListEditor.Grid != null && peQuoteID.CollectionSource != null)
                            {
                                peQuoteID.GridListEditor.Grid.KeyFieldName = "Oid";
                                if (objsamplecheckin != null && objsamplecheckin.ClientName != null)
                                {
                                    peQuoteID.CollectionSource.Criteria["QuoteID"] = CriteriaOperator.Parse("[Client.Oid] = ?  and Status =2 and [ExpirationDate] >= ? ", objsamplecheckin.ClientName.Oid, DateTime.Today);
                                }
                                else
                                {
                                    peQuoteID.CollectionSource.Criteria["QuoteID"] = CriteriaOperator.Parse("1=2");
                                }
                                peQuoteID.Refresh();
                                peQuoteID.RefreshDataSource();
                            }
                            //End
                        }
                        else if (e.PropertyName == "QuoteID")
                        {
                            if (objsamplecheckin != null && objsamplecheckin.QuoteID != null && objsamplecheckin.QuoteID.ProjectID != null)
                            {
                                objsamplecheckin.ProjectID = objsamplecheckin.QuoteID.ProjectID;
                            }
                            if (objsamplecheckin != null && objsamplecheckin.QuoteID != null && objsamplecheckin.QuoteID.TAT != null)
                            {
                                objsamplecheckin.TAT = objsamplecheckin.QuoteID.TAT;
                            }
                            if (objsamplecheckin != null && objsamplecheckin.QuoteID != null && objsamplecheckin.QuoteID != null)
                            {
                                List<AnalysisPricing> lstanalysisprice = View.ObjectSpace.GetObjects<AnalysisPricing>(CriteriaOperator.Parse("[CRMQuotes.Oid] = ? and SampleMatrix is not null", objsamplecheckin.QuoteID.Oid)).ToList();

                                foreach (VisualMatrix objVM in lstanalysisprice.Select(s => s.SampleMatrix).Distinct().ToList())
                                {
                                    objsamplecheckin.SampleMatrixes.Add(objVM);
                                    if (objsamplecheckin.SampleMatries == null)
                                        objsamplecheckin.SampleMatries = objVM.Oid.ToString();
                                    else
                                        objsamplecheckin.SampleMatries = objsamplecheckin.SampleMatries + ";" + objVM.Oid.ToString();
                                }
                            }

                        }
                        else if (e.PropertyName == "NPTest")
                        {
                            SamplingProposal objTask = (SamplingProposal)e.Object;
                            if (!string.IsNullOrEmpty(objTask.NPTest) && !string.IsNullOrEmpty(objTask.SampleMatries))
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                HttpContext.Current.Session["Test"] = objTask.NPTest;
                                if (HttpContext.Current.Session["Test"] != null)
                                {
                                    List<VisualMatrix> lstVM = new List<VisualMatrix>();
                                    foreach (string strMatrix in objTask.SampleMatries.Split(';'))
                                    {
                                        VisualMatrix objSM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strMatrix));
                                        if (objSM != null)
                                        {
                                            lstVM.Add(objSM);
                                        }
                                    }
                                    //CustomDueDate cdd = View.CurrentObject as CustomDueDate;
                                    string[] TestOid = HttpContext.Current.Session["Test"].ToString().Split(new string[] { ";" }, StringSplitOptions.None);
                                    if (objSMInfo.lstTestOid != null && objSMInfo.lstTestOid.Count > 0)
                                    {
                                        objSMInfo.lstTestOid.Clear();
                                    }

                                    if (TestOid != null && TestOid.Count() > 0)
                                    {
                                        foreach (string strTest in TestOid)
                                        {
                                            List<string> lstTestName = strTest.Split('|').ToList();
                                            if (lstTestName.Count == 1)
                                            {
                                                //COCSettings Form Testname split
                                                lstTestName = strTest.Split('_').ToList();
                                            }
                                            if (lstTestName.Count == 2)
                                            {
                                                IList<TestMethod> lstTests = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [MethodName.MethodNumber] = ? And [MethodName.GCRecord] Is Null ", lstTestName[0], lstTestName[1]));
                                                if (objSMInfo.lstTestOid == null)
                                                {
                                                    objSMInfo.lstTestOid = new List<Guid>();
                                                    foreach (TestMethod obj in lstTests.ToList())
                                                    {
                                                        if (!objSMInfo.lstTestOid.Contains(obj.Oid))
                                                        {
                                                            objSMInfo.lstTestOid.Add(obj.Oid);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (TestMethod obj in lstTests.ToList())
                                                    {
                                                        if (!objSMInfo.lstTestOid.Contains(obj.Oid))
                                                        {
                                                            objSMInfo.lstTestOid.Add(obj.Oid);
                                                        }
                                                    }
                                                }
                                            }
                                            else if (lstTestName.Count == 1)
                                            {
                                                IList<TestMethod> lstTests = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [IsGroup]=true", lstTestName[0]));
                                                if (objSMInfo.lstTestOid == null)
                                                {
                                                    objSMInfo.lstTestOid = new List<Guid>();
                                                    foreach (TestMethod obj in lstTests.ToList())
                                                    {
                                                        if (!objSMInfo.lstTestOid.Contains(obj.Oid))
                                                        {
                                                            objSMInfo.lstTestOid.Add(obj.Oid);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (TestMethod obj in lstTests.ToList())
                                                    {
                                                        if (!objSMInfo.lstTestOid.Contains(obj.Oid))
                                                        {
                                                            objSMInfo.lstTestOid.Add(obj.Oid);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    ListPropertyEditor lstccd = ((DetailView)View).FindItem("CustomDueDates") as ListPropertyEditor;
                                    if (lstccd != null && lstccd.ListView == null)
                                    {
                                        lstccd.CreateControl();
                                    }
                                    ListView lstcustomduedate = lstccd.ListView;
                                    if (lstcustomduedate != null && lstcustomduedate.CollectionSource.List.Count > 0)
                                    {
                                        string[] arrNewTests = e.NewValue.ToString().Split(new string[] { ";" }, StringSplitOptions.None);
                                        string[] arrOldTests = e.OldValue.ToString().Split(new string[] { ";" }, StringSplitOptions.None);
                                        List<Guid> lstOid = new List<Guid>();
                                        foreach (string strOid in arrOldTests)
                                        {
                                            //Guid oid = new Guid(strOid);
                                            List<string> lstTest = strOid.Split('|').ToList();
                                            if (lstTest.Count == 2)
                                            {
                                                IList<TestMethod> lstTests = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [MethodName.MethodNumber] = ?", lstTest[0], lstTest[1]));
                                                foreach (TestMethod obj in lstTests.ToList())
                                                {
                                                    if (arrNewTests.FirstOrDefault(i => i == strOid) == null && !lstOid.Contains(obj.Oid))
                                                    {
                                                        lstOid.Add(obj.Oid);
                                                    }
                                                }
                                            }
                                            else if (lstTest.Count == 1)
                                            {
                                                IList<TestMethod> lstTests = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [IsGroup] =true", lstTest[0]));
                                                foreach (TestMethod obj in lstTests.ToList())
                                                {
                                                    if (arrNewTests.FirstOrDefault(i => i == strOid) == null && !lstOid.Contains(obj.Oid))
                                                    {
                                                        lstOid.Add(obj.Oid);
                                                    }
                                                }
                                            }

                                        }
                                        foreach (CustomDueDate clr in lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().Where(i => lstOid.Contains(i.TestMethod.Oid)).ToList())
                                        {
                                            lstcustomduedate.CollectionSource.Remove(clr);
                                        }
                                    }
                                    if (objSMInfo.lstTestOid != null && objSMInfo.lstTestOid.Count > 0)
                                    {
                                        foreach (string val in objSMInfo.lstTestOid.Select(i => i.ToString()).ToList())
                                        {
                                            if (!string.IsNullOrEmpty(val))
                                            {
                                                TestMethod objTestMethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", new Guid(val)));
                                                if (objTestMethod != null)
                                                {
                                                    if (lstcustomduedate != null)
                                                    {
                                                        var lst = lstVM.Where(i => i.MatrixName.Oid == objTestMethod.MatrixName.Oid);
                                                        if (lst != null && lst.Count() > 0)
                                                        {
                                                            foreach (VisualMatrix objSM in lst.Where(i => i.MatrixName != null).GroupBy(p => p.MatrixName.Oid).Select(grp => grp.FirstOrDefault()))
                                                            {
                                                                if (objTask.CustomDueDates.FirstOrDefault(i => i.TestMethod.Oid == objTestMethod.Oid && i.TestMethod.MatrixName.Oid == objSM.MatrixName.Oid) == null)
                                                                {
                                                                    CustomDueDate objDate = ObjectSpace.CreateObject<CustomDueDate>();
                                                                    objDate.TestMethod = objTestMethod;
                                                                    objDate.SamplingProposal = objTask;
                                                                    objDate.SampleMatrix = objSM;
                                                                    if (!objTestMethod.IsGroup)
                                                                    {
                                                                        objDate.Parameter = "AllParam";
                                                                    }
                                                                    if (objsamplecheckin != null && objsamplecheckin.TAT != null)
                                                                    {
                                                                        objDate.DueDate = objsamplecheckin.DueDate;
                                                                        objDate.TAT = objsamplecheckin.TAT;
        }
                                                                    lstcustomduedate.CollectionSource.Add(objDate);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                ListPropertyEditor lstccd = ((DetailView)View).FindItem("CustomDueDates") as ListPropertyEditor;
                                ListView lstcustomduedate = lstccd.ListView;
                                if (lstcustomduedate != null)
                                {
                                    foreach (CustomDueDate clr in lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().ToList())
                                    {
                                        lstcustomduedate.CollectionSource.Remove(clr);
                                    }
                                }
                            }
                        }
                        else if (e.PropertyName == "SampleMatries")
                        {
                            SamplingProposal objTask = (SamplingProposal)e.Object;
                            
                            if (!string.IsNullOrEmpty(objTask.NPTest) && !string.IsNullOrEmpty(objTask.SampleMatries))
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                HttpContext.Current.Session["Test"] = objTask.NPTest;
                                if (HttpContext.Current.Session["Test"] != null)
                                {
                                    List<VisualMatrix> lstVM = new List<VisualMatrix>();
                                    foreach (string strMatrix in objTask.SampleMatries.Split(';'))
                                    {
                                        VisualMatrix objSM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strMatrix));
                                        if (objSM != null)
                                        {
                                            lstVM.Add(objSM);
                                        }
                                    }
                                    //CustomDueDate cdd = View.CurrentObject as CustomDueDate;
                                    if (objSMInfo.lstTestOid != null && objSMInfo.lstTestOid.Count > 0)
        {
                                        objSMInfo.lstTestOid.Clear();
        }
                                    string[] TestOid = HttpContext.Current.Session["Test"].ToString().Split(new string[] { ";" }, StringSplitOptions.None);
                                    if (TestOid != null && TestOid.Count() > 0)
                                    {
                                        foreach (string strTest in TestOid)
                                        {
                                            List<string> lstTestName = strTest.Split('|').ToList();
                                            if (lstTestName.Count == 2)
                                            {
                                                IList<TestMethod> lstTests = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [MethodName.MethodNumber] = ?", lstTestName[0], lstTestName[1]));
                                                if (objSMInfo.lstTestOid == null)
        {
                                                    objSMInfo.lstTestOid = new List<Guid>();
                                                    foreach (TestMethod obj in lstTests.ToList())
                                                    {
                                                        if (!objSMInfo.lstTestOid.Contains(obj.Oid))
                                                        {
                                                            objSMInfo.lstTestOid.Add(obj.Oid);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (TestMethod obj in lstTests.ToList())
                                                    {
                                                        if (!objSMInfo.lstTestOid.Contains(obj.Oid))
                                                        {
                                                            objSMInfo.lstTestOid.Add(obj.Oid);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    ListPropertyEditor lstccd = ((DetailView)View).FindItem("CustomDueDates") as ListPropertyEditor;
                                    if (lstccd != null && lstccd.ListView == null)
                                    {
                                        lstccd.CreateControl();
                                    }
                                    ListView lstcustomduedate = lstccd.ListView;
                                    if (lstcustomduedate != null && lstcustomduedate.CollectionSource.List.Count > 0)
                                    {
                                        List<Guid> lstOid = new List<Guid>();
                                        foreach (CustomDueDate strOid in lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().ToList())
                                        {
                                            if (lstVM.FirstOrDefault(i => i.MatrixName.Oid == strOid.TestMethod.MatrixName.Oid) == null)
                                            {
                                                lstOid.Add(strOid.TestMethod.MatrixName.Oid);
                                            }
                                        }
                                        foreach (CustomDueDate clr in lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().ToList().Where(i => lstOid.Contains(i.TestMethod.MatrixName.Oid)).ToList())
                                        {
                                            lstcustomduedate.CollectionSource.Remove(clr);
                                        }
                                    }
                                    if (objSMInfo.lstTestOid != null && objSMInfo.lstTestOid.Count > 0)
                                    {
                                        foreach (string val in objSMInfo.lstTestOid.Select(i => i.ToString()))
                                        {
                                            if (!string.IsNullOrEmpty(val))
                                            {
                                                TestMethod objTestMethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", new Guid(val)));
                                                //if (objTestMethod != null && objTask.CustomDueDates.FirstOrDefault(i => i.TestMethod != null && i.TestMethod.Oid == new Guid(val) && i.TestMethod.TestName == objTestMethod.TestName) == null)
                                                if (objTestMethod != null)
                                                {
                                                    if (lstcustomduedate != null)
                                                    {
                                                        var lst = lstVM.Where(i => i.MatrixName.Oid == objTestMethod.MatrixName.Oid);
                                                        if (lst != null && lst.Count() > 0)
                                                        {
                                                            foreach (VisualMatrix objSM in lst.Where(i => i.MatrixName != null).GroupBy(p => p.MatrixName.Oid).Select(grp => grp.FirstOrDefault()))
                                                            {
                                                                if (objTask.CustomDueDates.FirstOrDefault(i => i.TestMethod.Oid == objTestMethod.Oid && i.TestMethod.MatrixName.Oid == objSM.MatrixName.Oid) == null)
                                                                {
                                                                    CustomDueDate objDate = ObjectSpace.CreateObject<CustomDueDate>();
                                                                    objDate.TestMethod = objTestMethod;
                                                                    objDate.SamplingProposal = objTask;
                                                                    objDate.SampleMatrix = objSM;
                                                                    if (!objTestMethod.IsGroup)
                                                                    {
                                                                        objDate.Parameter = "AllParam";
                                                                    }
                                                                    if (objsamplecheckin != null && objsamplecheckin.TAT != null)
                                                                    {
                                                                        objDate.DueDate = objsamplecheckin.DueDate;
                                                                        objDate.TAT = objsamplecheckin.TAT;
                                                                    }
                                                                    lstcustomduedate.CollectionSource.Add(objDate);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                ListPropertyEditor lstccd = ((DetailView)View).FindItem("CustomDueDates") as ListPropertyEditor;
                                ListView lstcustomduedate = lstccd.ListView;
                                if (lstcustomduedate != null)
                                {
                                    foreach (CustomDueDate clr in lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().ToList())
                                    {
                                        lstcustomduedate.CollectionSource.Remove(clr);
                                    }
                                }
                            }
                        }
                        else if (e.PropertyName == "TAT" || e.PropertyName == "RecievedDate")
                        {
                            if (objsamplecheckin != null && objsamplecheckin.TAT != null)
                            {
                                int tatHour = objsamplecheckin.TAT.Count;
                                DateTime dateTime = objsamplecheckin.RecievedDate;
                                int Day = 0;

                                if (tatHour >= 24)
                                {
                                    Day = tatHour / 24;
                                    objsamplecheckin.DueDate = AddWorkingDays(objsamplecheckin.RecievedDate, Day);
                                }
                                else
                                {
                                    objsamplecheckin.DueDate = AddWorkingHours(objsamplecheckin.RecievedDate, tatHour);
                                }
                                ListPropertyEditor lstccd = ((DetailView)View).FindItem("CustomDueDates") as ListPropertyEditor;
                                ListView lstcustomduedate = lstccd.ListView;
                                if (lstcustomduedate != null)
                                {
                                    lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().ToList().ForEach(i => { i.TAT = objsamplecheckin.TAT; i.DueDate = objsamplecheckin.DueDate; });
                                    ((ListView)lstcustomduedate).Refresh();
                                }
                               

                            }
                        }

                        else if (e.PropertyName == "QuoteID")
                        {

                            ListPropertyEditor lstItemPrice = ((DetailView)View).FindItem("SCItemCharges") as ListPropertyEditor;
                            if (lstItemPrice != null && lstItemPrice.ListView == null)
                            {
                                lstItemPrice.CreateControl();
                            }
                            if (lstItemPrice != null && lstItemPrice.ListView != null)
                            {
                                foreach (SampleCheckinItemChargePricing obj in ((ListView)lstItemPrice.ListView).CollectionSource.List.Cast<SampleCheckinItemChargePricing>().ToList())
                                {
                                    ((ListView)lstItemPrice.ListView).CollectionSource.Remove(obj);
                                }
                            }
                            IObjectSpace os = lstItemPrice.ListView.ObjectSpace;
                            SamplingProposal objTask = (SamplingProposal)e.Object;
                            if (objTask.QuoteID != null)
                            {
                                CRMQuotes objQuote = os.GetObjectByKey<CRMQuotes>(objTask.QuoteID.Oid);
                                if (objQuote != null && objQuote.QuotesItemChargePrice.Count > 0)
                                {
                                    foreach (QuotesItemChargePrice obj in objQuote.QuotesItemChargePrice.ToList())
                                    {
                                        SampleCheckinItemChargePricing objNewItem = os.CreateObject<SampleCheckinItemChargePricing>();
                                        objNewItem.ItemPrice = os.GetObjectByKey<ItemChargePricing>(obj.ItemPrice.Oid);
                                        objNewItem.Qty = obj.Qty;
                                        objNewItem.UnitPrice = obj.UnitPrice;
                                        objNewItem.Amount = obj.Amount;
                                        objNewItem.FinalAmount = obj.FinalAmount;
                                        objNewItem.Discount = obj.Discount;
                                        objNewItem.Description = obj.Description;
                                        objNewItem.NpUnitPrice = obj.NpUnitPrice;
                                        ((ListView)lstItemPrice.ListView).CollectionSource.Add(objNewItem);
                                    }
                                }
                                 ((ListView)lstItemPrice.ListView).Refresh();
                            }

                        }
                        else if (e.PropertyName == "DateCollected")
                        {
                            if (objsamplecheckin.DateCollected != DateTime.MinValue && (objsamplecheckin.DateCollected > objsamplecheckin.RecievedDate || Convert.ToDateTime(objsamplecheckin.DateCollected).Day == objsamplecheckin.RecievedDate.Day && Convert.ToDateTime(objsamplecheckin.DateCollected).Minute == objsamplecheckin.RecievedDate.Minute))
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SLReceiveddate"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                objsamplecheckin.DateCollected = null;
                                return;
                            }
                        }
                        else if(e.PropertyName== "RetireDate")
                        {
                            if (objsamplecheckin.RetireDate!=null && objsamplecheckin.RetireDate!=DateTime.MinValue)
                            {
                                objsamplecheckin.RetiredBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            }
                            else
                            {
                                objsamplecheckin.RetiredBy = null;
                            }
                        }
                    }
                }
                if (e.PropertyName == "Copyfrom")
                {
                    SamplingProposal sampling = View.CurrentObject as SamplingProposal;
                    DashboardViewItem lstItemPrice = ((DetailView)View).FindItem("CopyTo") as DashboardViewItem;
                    if (lstItemPrice != null && lstItemPrice.InnerView != null && sampling.Copyfrom!=null)
                    {

                        ((ListView)lstItemPrice.InnerView).CollectionSource.Criteria["filter"] = new NotOperator(new InOperator("Oid", sampling.Copyfrom.RegistrationID.Oid));
                    }
                    else
                    {

                        ((ListView)lstItemPrice.InnerView).CollectionSource.Criteria["filter"] = new NotOperator(new InOperator("Oid", sampling.Oid));

                    }

                }
               

            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private DateTime AddWorkingDays(DateTime date, int daysToAdd)
        {
            try
            {
                while (daysToAdd > 0)
                {
                    date = date.AddDays(1);
                    IList<Holidays> lstHoliday = ObjectSpace.GetObjects<Holidays>(CriteriaOperator.Parse("Oid is Not Null"));
                    Holidays objHoliday = lstHoliday.FirstOrDefault(i => i.HolidayDate != DateTime.MinValue && i.HolidayDate.Day.Equals(date.Day));
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && objHoliday == null)
                    {
                        daysToAdd -= 1;
                    }
                }
                return date;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return DateTime.Now;
            }
        }
        private DateTime AddWorkingHours(DateTime date, int daysToAdd)
        {
            try
            {
                while (daysToAdd > 0)
                {
                    date = date.AddHours(1);
                    IList<Holidays> lstHoliday = ObjectSpace.GetObjects<Holidays>(CriteriaOperator.Parse("Oid is Not Null"));
                    Holidays objHoliday = lstHoliday.FirstOrDefault(i => i.HolidayDate != DateTime.MinValue && i.HolidayDate.Day.Equals(date.Day));
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && objHoliday == null)
                    {
                        daysToAdd -= 1;
                    }
                }
                return date;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return DateTime.Now;
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
              if(View.Id== "SamplingProposal_DetailView")
                {
                    if (Application != null && Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.ObjectTypeInfo.Type == typeof(SamplingProposal))
                    {
                        if (((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit)
                        {
                            objSMInfo.SamplingProposalViewEditMode = ViewEditMode.Edit;
                        }
                        else
                        {
                            objSMInfo.SamplingProposalViewEditMode = ViewEditMode.View;
                        }
                    }
                    SamplingProposal objsampreg = (SamplingProposal)View.CurrentObject;
                    ASPxStringPropertyEditor JobIDpropertyEditor = ((DetailView)View).FindItem("RegistrationID") as ASPxStringPropertyEditor;
                    if (JobIDpropertyEditor != null && JobIDpropertyEditor.Editor != null)
                    {
                        ASPxTextBox editor = (ASPxTextBox)JobIDpropertyEditor.Editor;
                        if (editor != null)
                        {
                            editor.ClientSideEvents.KeyPress = @"function(s, e){
                                                                                var regex = /[0-9]/;   
                                                                                if (!regex.test(e.htmlEvent.key)) {
                                                                                e.htmlEvent.returnValue = false;
                                                                                }}";
                        }
                    }
                    ASPxGridLookupPropertyEditor propertyEditorProjectID = ((DetailView)View).FindItem("ProjectID") as ASPxGridLookupPropertyEditor;
                    if (propertyEditorProjectID != null && propertyEditorProjectID.CollectionSource != null)
                    {
                        if (objsampreg != null && objsampreg.ClientName != null)
                        {
                            propertyEditorProjectID.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("[customername.Oid] = ? ", objsampreg.ClientName.Oid);
                        }
                        else
                        {
                            propertyEditorProjectID.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                        }
                    }
                    ASPxGridLookupPropertyEditor peQuoteID = ((DetailView)View).FindItem("QuoteID") as ASPxGridLookupPropertyEditor;
                    if (peQuoteID != null && peQuoteID.GridListEditor != null && peQuoteID.GridListEditor.Grid != null && peQuoteID.CollectionSource != null)
                    {
                        peQuoteID.GridListEditor.Grid.KeyFieldName = "Oid";
                        peQuoteID.GridListEditor.Grid.Settings.ShowFilterRow = true;
                        if (objsampreg != null && objsampreg.ClientName != null)
                        {
                            peQuoteID.CollectionSource.Criteria["QuoteID"] = CriteriaOperator.Parse("[Client.Oid] = ? and Status =2 and [ExpirationDate] >= ?", objsampreg.ClientName.Oid, DateTime.Today);
                            peQuoteID.RefreshDataSource();
                            peQuoteID.GridListEditor.Grid.DataBind();
                        }
                        else
                        {
                            peQuoteID.CollectionSource.Criteria["QuoteID"] = CriteriaOperator.Parse("1=2");
                        }
                        if (peQuoteID.GridListEditor.Grid.Columns["QuoteID"] != null)
                        {
                            peQuoteID.GridListEditor.Grid.Columns["QuoteID"].Width = 110;
                        }
                    }
                    List<Sampling> lstSamples = View.ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("SamplingProposal.Oid=?", objsampreg.Oid)).ToList();
                    if (lstSamples.Count > 0)
                    {
                        objSMInfo.isNoOfSampleDisable = true;
                    }
                    else
                    {
                        objSMInfo.isNoOfSampleDisable = false;
                        if (objSMInfo.IsSamplePopupClose)
                        {
                            objSMInfo.IsSamplePopupClose = false;
                        }
                    }
                    if (objsampreg.NPTest == null)
                    {
                        ListPropertyEditor lstccd = ((DetailView)View).FindItem("CustomDueDates") as ListPropertyEditor;
                        ListView lstcustomduedate = lstccd.ListView;
                        if (lstcustomduedate != null && lstcustomduedate.CollectionSource.List.Count > 0)
                        {
                            foreach (CustomDueDate clr in lstcustomduedate.CollectionSource.List.Cast<CustomDueDate>().ToList())
                            {
                                lstcustomduedate.CollectionSource.Remove(clr);
                            }
                        }
                    }
                    if (objPmsInfo.SamplingProposalIsWrite == true && ((DetailView)View).ViewEditMode == ViewEditMode.View)
                    {
                        objPmsInfo.SamplingProposalIsWrite = false;
                    }
                    if (objInfo.ClientName == null || objInfo.ClientName.Length == 0)
                    {
                        SamplingProposal objsamplecheckin = (SamplingProposal)View.CurrentObject;
                        if (objsamplecheckin != null && objsamplecheckin.ClientName != null)
                        {
                            objInfo.ClientName = objsamplecheckin.ClientName.CustomerName;
                        }
                        if (objsamplecheckin != null && objsamplecheckin.ProjectID != null)
                        {
                            objInfo.ProjectName = objsamplecheckin.ProjectID.ProjectName;
                        }
                    }
                    if(objsampreg!=null)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        int sampleno = View.ObjectSpace.GetObjectsCount(typeof(Sampling), CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", objsampreg.Oid));
                        SamplingSample.Caption = "Samples" + "(" + sampleno + ")";
                        if (this.Actions["btnSamplingTest"] != null)
                        {
                            this.Actions["btnSamplingTest"].Caption = "Tests" + "(" + View.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[Sampling.SamplingProposal.Oid] = ?", objsampreg.Oid)).Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count() + ")";
                        }
                        if (this.Actions["btnSamplingBottleAllocation"] != null)
                        {
                            this.Actions["btnSamplingBottleAllocation"].Caption = "Containers" + "(" + os.GetObjects<Sampling>(CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", objsampreg.Oid)).Sum(i => i.Qty) + ")";
                        }
                    }
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                                ASPxEditBase editor = (ASPxEditBase)propertyEditor.Editor;
                                if (editor != null)
                                {
                                    editor.ClientInstanceName = propertyEditor.Model.Id;
                                }
                            }
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                                ASPxEditBase editor = (ASPxEditBase)propertyEditor.Editor;
                                if (editor != null)
                                {
                                    editor.ClientInstanceName = propertyEditor.Model.Id;
                                }
                            }
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                        {
                            ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                                if (propertyEditor.PropertyName == "TAT")
                                {
                                    ASPxGridLookup editor = (ASPxGridLookup)propertyEditor.Editor;
                                    if (editor != null)
                                    {
                                        editor.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                        editor.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                        editor.GridView.Settings.VerticalScrollableHeight = 200;
                                    }
                                }
                                if (propertyEditor.PropertyName == "ClientName")
                                {
                                    ASPxGridLookup editor = (ASPxGridLookup)propertyEditor.Editor;
                                    if (editor != null)
                                    {
                                        editor.GridView.Settings.VerticalScrollableHeight = 270;
                                    }
                                }
                            }
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(FileDataPropertyEditor))
                        {
                            FileDataPropertyEditor propertyEditor = item as FileDataPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                        {
                            ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                                ASPxEditBase editor = (ASPxEditBase)propertyEditor.Editor;
                                if (editor != null)
                                {
                                    editor.ClientInstanceName = propertyEditor.Model.Id;
                                }
                            }
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                        {
                            ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                {
                                    propertyEditor.FindEdit.Editor.BackColor = Color.LightYellow;

                                    ASPxEditBase editor = (ASPxEditBase)propertyEditor.Editor;
                                    if (editor != null)
                                    {
                                        editor.ClientInstanceName = propertyEditor.Model.Id;
                                    }
                                }
                                else if (propertyEditor.DropDownEdit != null)
                                {
                                    propertyEditor.DropDownEdit.DropDown.BackColor = Color.LightYellow;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                            }
                            if (propertyEditor != null && propertyEditor.DropDownEdit != null && propertyEditor.DropDownEdit.DropDown != null)
                            {
                                propertyEditor.DropDownEdit.DropDown.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                        {
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.Editor is ASPxEditBase)
                                {
                                    ASPxEditBase editor = (ASPxEditBase)propertyEditor.Editor;
                                    if (editor != null)
                                    {
                                        editor.ClientInstanceName = propertyEditor.Model.Id;
                                    }
                                }
                            }
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                                if (propertyEditor.Id == "NoOfSamples")
                                {
                                    if (objSMInfo.isNoOfSampleDisable)
                                    {
                                        propertyEditor.AllowEdit.SetItemValue("stat", false);
                                    }
                                    else
                                    {
                                        propertyEditor.AllowEdit.SetItemValue("stat", true);
                                        propertyEditor.Editor.BackColor = Color.LightYellow;
                                    }
                                    if ((item as ASPxIntPropertyEditor).Editor != null)
                                        (item as ASPxIntPropertyEditor).Editor.Load += Editor_Load;
                                }
                            }
                        }
                        else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                        {
                            ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                            ASPxGridLookup lookup = (ASPxGridLookup)propertyEditor.Editor;
                            if (lookup != null && propertyEditor.Id == "SampleMatries")
                            {
                                //lookup.GridViewProperties.Settings.ShowFilterRow = true;
                                lookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                lookup.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                lookup.GridView.Settings.VerticalScrollableHeight = 200;
                                lookup.GridViewProperties.SettingsSearchPanel.Visible = true;
                                foreach (WebColumnBase columns in lookup.GridView.VisibleColumns)
                                {
                                    if (columns.Index == 1)
                                    {
                                        columns.Caption = "Sample Matrix";
                                    }
                                }
                            }
                            else if (lookup != null && propertyEditor.Id == "SampleCategory")
                            {
                                foreach (WebColumnBase columns in lookup.GridView.VisibleColumns)
                                {
                                    if (columns.Index == 1)
                                    {
                                        columns.Caption = "Sample Category";
                                    }
                                }
                            }
                        }

                    }
                }
              else if(View.Id== "Sampling_ListView_SamplingProposal")
                {
                   // ((ASPxGridListEditor)((ListView)View).Editor).Grid.BatchUpdate += Grid_BatchUpdate;
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SamplingProposal.RegistrationID] = ? AND [SamplingProposal.GCRecord] is NULL", objSMInfo.strJobID);
                    SamplingProposal objsmplcheckin = ObjectSpace.FindObject<SamplingProposal>(CriteriaOperator.Parse("[Oid] = ?", objSMInfo.CurrentJob.Oid));
                    string[] strvmarr = objsmplcheckin.SampleMatries.Split(';');
                    objSMInfo.lstSRvisualmat = new List<VisualMatrix>();
                    foreach (string strvmoid in strvmarr.ToList())
                    {
                        VisualMatrix lstvmatobj = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strvmoid)));
                        if (lstvmatobj != null)
                        {
                            objSMInfo.lstSRvisualmat.Add(lstvmatobj);
                        }
                    }
                    ICallbackManagerHolder sampleid = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    sampleid.CallbackManager.RegisterHandler("id", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.JSProperties["cpsuboutremove"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Suboutremove");
                    gridListEditor.Grid.JSProperties["cpcollectdatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SLCollectdate");
                    gridListEditor.Grid.JSProperties["cpReceiveddatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SLReceiveddate");
                    gridListEditor.Grid.JSProperties["cpCollecteddateTimemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SLCollectdateTime");
                    gridListEditor.Grid.JSProperties["cpReceiveddateTimemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SLReceiveddateTime");
                    if (objSMInfo.SamplingProposalViewEditMode == ViewEditMode.Edit)
                    {
                        gridListEditor.Grid.JSProperties["cpviewid"] = "ViewEditMode_Edit";
                    }
                    else
                    {
                        gridListEditor.Grid.JSProperties["cpviewid"] = "ViewEditMode_View";
                    }
                    gridListEditor.Grid.HtmlCommandCellPrepared += Grid_HtmlCommandCellPrepared;
                    gridListEditor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                    gridListEditor.Grid.Load += Grid_Load;
                    //gridListEditor.Grid.RowValidating += Grid_RowValidating;
                    gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                    gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                    gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                    gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                    if (objSMInfo.EditColumnName == null)
                    {
                        objSMInfo.EditColumnName = new List<string>();
                        foreach (ColumnWrapper wrapper in gridListEditor.Columns)
                        {
                            IModelColumn columnModel = ((ListView)View).Model.Columns[wrapper.PropertyName];
                            if (columnModel != null && columnModel.AllowEdit == true && !objSMInfo.EditColumnName.Contains(columnModel.Id + ".Oid") && columnModel.PropertyEditorType == typeof(ASPxLookupPropertyEditor))
                            {
                                objSMInfo.EditColumnName.Add(columnModel.Id + ".Oid");
                            }
                            else if (columnModel != null && columnModel.AllowEdit == true && !objSMInfo.EditColumnName.Contains(columnModel.Id) && columnModel.PropertyEditorType != typeof(ASPxLookupPropertyEditor))
                            {
                                objSMInfo.EditColumnName.Add(columnModel.Id);
                            }
                        }
                    }
                    if (objSMInfo.EditColumnName.Count > 0)
                    {
                        gridListEditor.Grid.JSProperties["cpeditcolumnname"] = objSMInfo.EditColumnName;
                    }
                    if (objSMInfo.IsTestAssignmentClosed)
                    {
                        objSMInfo.IsTestAssignmentClosed = false;
                    }

                    if (SamplingAddSample != null)
                    {
                        SamplingAddSample.Active["showAddSample"] = objPmsInfo.SamplingProposalIsWrite;
                    }
                    if (objPmsInfo.SamplingProposalIsWrite == false)
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e){
                       e.cancel = true;
                 
                }";
                    }
                    else
                    {
                        gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                        {
                            var fieldName = e.cellInfo.column.fieldName;
                            sessionStorage.setItem('SampleRegistrationFocusedColumn', fieldName);
                            sessionStorage.setItem('CanChangeVisualMatrix', '');
                            s.GetRowValues(e.cellInfo.rowVisibleIndex, 'CanChangeVisualMatrix',function GetVisualMatrixChange(value) 
                            {
                                sessionStorage.setItem('CanChangeVisualMatrix', value);
                            });
                        }";
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e)
                        {
                            if(s.cpviewid == 'ViewEditMode_View')
                            {
                                 e.cancel = true;
                            }
                            else
                            {
                                if(e.focusedColumn.fieldName == 'VisualMatrix.Oid' && s.batchEditApi.GetCellValue(e.visibleIndex, 'VisualMatrix.Oid') != null)
                                {
                                    var val = sessionStorage.getItem('CanChangeVisualMatrix');
                                    if (val == 'CannotChange')
                                    {
                                        e.cancel = true;
                                    }
                                    else if(val == 'DeleteSampleParamsAndChange')
                                    {
                                        s.GetRowValues(e.visibleIndex, 'Test;Oid', OnGetRowValues);
                                    }
                                }
                                else if(e.focusedColumn.fieldName == 'SampleID')
                                {
                                    e.cancel = true;
                                }
                            }
                        }";
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) 
                                                {
                                                    s.timerHandle = setTimeout(function() 
                                                    {



var timestart = s.batchEditApi.GetCellValue(e.visibleIndex, 'TimeStart');
var timeend = s.batchEditApi.GetCellValue(e.visibleIndex, 'TimeEnd');
if(timestart != null && timeend != null)
{
if (timestart > timeend) {
    alert('The TimeStart must be less than the TimeEnd.');
    
    s.batchEditApi.SetCellValue(e.visibleIndex, 'TimeStart', null);
    s.batchEditApi.SetCellValue(e.visibleIndex, 'TimeEnd', null);
s.batchEditApi.SetCellValue(e.visibleIndex, 'Time',null); 
}
}
var time =0;


                             var datecollected = s.batchEditApi.GetCellValue(i, 'CollectDate',false);
                             var datereceived = s.batchEditApi.GetCellValue(i, 'SamplingProposal.RecievedDate',false);
                             var dt = new Date();
                             
                             if(dt != null && datecollected != null && datereceived != null)
                             {
                                var collectedYear = datecollected.getFullYear();
                                var collectedMonth = datecollected.getMonth();
                                var collectedDay = datecollected.getDate();
                                var collectedHours = datecollected.getHours();
                                var collectedMinutes = datecollected.getMinutes();
                                var receivedYear = datereceived.getFullYear();
                               var receivedMonth = datereceived.getMonth();
                               var receivedDay = datereceived.getDate();
                               var receivedHours = datereceived.getHours();
                               var receivedMinutes = datereceived.getMinutes();
                                 if(collectedYear != null && receivedYear != null && collectedMonth != null && receivedMonth != null && collectedDay != null && receivedDay != null 
                                    && collectedHours != null && receivedHours != null && collectedMinutes != null && receivedMinutes != null)
                                {
                                    if (collectedYear > receivedYear ||(collectedYear === receivedYear && 
                                        (collectedMonth > receivedMonth || (collectedMonth === receivedMonth &&  
                                        (collectedDay > receivedDay || (collectedDay === receivedDay && 
                                        (collectedHours > receivedHours ||  (collectedHours === receivedHours && 
                                            collectedMinutes >= receivedMinutes))))))))
                                    {
                                       alert(s.cpReceiveddatemsg);
                                       s.batchEditApi.SetCellValue(i, 'CollectDate', null); 
                                    }
                                }
                             }

if(timestart != null && timeend != null)
{
if (timestart < timeend) 
{
    time =  (timeend-timestart)/1000;
time = (time/60);

s.batchEditApi.SetCellValue(e.visibleIndex, 'Time',time); 
}
}
                                                        var flowrate = s.batchEditApi.GetCellValue(e.visibleIndex, 'FlowRate');
                                                        var timemin = s.batchEditApi.GetCellValue(e.visibleIndex, 'Time');
                                                        var vol = 0;
                                                        if(flowrate != null && flowrate > 0 && timemin != null &&timemin > 0)
                                                        {
                                                            vol = flowrate * timemin;
                                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Volume',vol); 
                                                        }
                                                        else if(flowrate != null && flowrate < 1 || timemin != null && timemin < 1)
                                                        {
                                                       var volum = '';
                                                       //alert(volum)
                                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Volume', volum); 
                                                        }
                                                                                     var val = s.batchEditApi.GetCellValue(e.visibleIndex, 'Containers');
                                                                                                              if(val == 0 )
                                                                                                              {
                                                                                                              alert('#Containers must not be in 0');
                                                                                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Containers',1); 
                                                                                                              }
                                                                                     var val = s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty');
                                                                                                              if(val == 0 )
                                                                                                              {
                                                                                                              alert('Qty must not be in 0');
                                                                                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Qty',1); 
                                                                                                              }
                                                        //s.UpdateEdit();
                                                    }, 100); 
                                                }";
                    }

                    gridListEditor.Grid.ClientSideEvents.BatchEditChangesSaving = @"function(s, e)
                    {
                        for (var i in e.updatedValues) 
                        { 
                             var datecollected = s.batchEditApi.GetCellValue(i, 'CollectDate',false);
                             var datereceived = s.batchEditApi.GetCellValue(i, 'SamplingProposal.RecievedDate',false);
                             var dt = new Date();
                             
                             if(dt != null && datecollected != null && datereceived != null)
                             {
                                var collectedYear = datecollected.getFullYear();
                                var collectedMonth = datecollected.getMonth();
                                var collectedDay = datecollected.getDate();
                                var collectedHours = datecollected.getHours();
                                var collectedMinutes = datecollected.getMinutes();
                                var receivedYear = datereceived.getFullYear();
                               var receivedMonth = datereceived.getMonth();
                               var receivedDay = datereceived.getDate();
                               var receivedHours = datereceived.getHours();
                               var receivedMinutes = datereceived.getMinutes();
                                 if(collectedYear != null && receivedYear != null && collectedMonth != null && receivedMonth != null && collectedDay != null && receivedDay != null 
                                    && collectedHours != null && receivedHours != null && collectedMinutes != null && receivedMinutes != null)
                                {
                                    if (collectedYear > receivedYear ||(collectedYear === receivedYear && 
                                        (collectedMonth > receivedMonth || (collectedMonth === receivedMonth &&  
                                        (collectedDay > receivedDay || (collectedDay === receivedDay && 
                                        (collectedHours > receivedHours ||  (collectedHours === receivedHours && 
                                            collectedMinutes >= receivedMinutes))))))))
                                    {
                                       alert(s.cpReceiveddatemsg);
                                       s.batchEditApi.SetCellValue(i, 'CollectDate', null); 
                                    }
                                }
                             }
                        }  
                    }";
                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function (s, e) {
                        if(e.cellInfo.column.fieldName=='FlowRate')
                          {
                             s.GetEditor('FlowRate').KeyPress.AddHandler(OnSLFlowRateTimeChanged);
                          }
                       if(e.cellInfo.column.fieldName=='Time')
                          {
                           s.GetEditor('Time').KeyPress.AddHandler(OnSLFlowRateTimeChanged);
                          }
                       if(e.cellInfo.column.fieldName=='Volume')
                          {
                             s.GetEditor('Volume').KeyPress.AddHandler(OnSLFlowRateTimeChanged);
                          }
                           sessionStorage.setItem('SampleRegistrationCopyFocusedColumn', null);  
                           if((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit'))
                            {  
                              e.cancel = true;
                            }                  
                            else
                             {
                                 
                                   if (s.cpeditcolumnname.includes(e.cellInfo.column.fieldName))
                                    {
                                     var fieldName = e.cellInfo.column.fieldName; 
                                    sessionStorage.setItem('SampleRegistrationCopyFocusedColumn', fieldName); 
                                    }
                                    else
                                     {
                                          e.cancel=true;
                                     }
                             }                
                        }";
                    if (objPmsInfo.SamplingProposalIsWrite)
                    {
                        gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                { 
                                    var FocusedColumn = sessionStorage.getItem('SampleRegistrationCopyFocusedColumn');  
                                    var text;
                                    if(FocusedColumn.includes('.'))
                                    {  
                                        oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                        text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                        if (e.item.name =='CopyToAllCell')
                                        {
                                            if(FocusedColumn=='StationLocation.Oid')
                                            {
                                              var address= s.batchEditApi.GetCellValue(e.elementIndex,'Address');
                                              var pwsid= s.batchEditApi.GetCellValue(e.elementIndex,'PWSID');
                                              var KeyMap= s.batchEditApi.GetCellValue(e.elementIndex,'KeyMap');
                                              var SamplePointID= s.batchEditApi.GetCellValue(e.elementIndex,'SamplePointID');
                                              var SamplePointType= s.batchEditApi.GetCellValue(e.elementIndex,'SamplePointType');
                                              var SystemType= s.batchEditApi.GetCellValue(e.elementIndex,'SystemType');
                                              var PWSSystemName= s.batchEditApi.GetCellValue(e.elementIndex,'PWSSystemName');
                                              var RejectionCriteria= s.batchEditApi.GetCellValue(e.elementIndex,'RejectionCriteria');
                                              var WaterType= s.batchEditApi.GetCellValue(e.elementIndex,'WaterType');
                                              var ParentSampleID= s.batchEditApi.GetCellValue(e.elementIndex,'ParentSampleID');
                                              var ParentSampleDate= s.batchEditApi.GetCellValue(e.elementIndex,'ParentSampleDate');
                                            for(var i = 0; i < s.cpVisibleRowCount; i++)
                                                 { 
                                                if (s.IsRowSelectedOnPage(i)) 
                                                  {
                                                       s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                                        s.batchEditApi.SetCellValue(i,'Address',address); 
                                                       s.batchEditApi.SetCellValue(i,'PWSID',pwsid); 
                                                       s.batchEditApi.SetCellValue(i,'KeyMap',KeyMap);
                                                       s.batchEditApi.SetCellValue(i,'SamplePointID',SamplePointID);
                                                       s.batchEditApi.SetCellValue(i,'SamplePointType',SamplePointType);
                                                       s.batchEditApi.SetCellValue(i,'SystemType',SystemType);
                                                       s.batchEditApi.SetCellValue(i,'PWSSystemName',PWSSystemName);
                                                       s.batchEditApi.SetCellValue(i,'RejectionCriteria',RejectionCriteria);
                                                       s.batchEditApi.SetCellValue(i,'WaterType',WaterType);
                                                       s.batchEditApi.SetCellValue(i,'ParentSampleID',ParentSampleID);
                                                       s.batchEditApi.SetCellValue(i,'ParentSampleDate',ParentSampleDate);
                                                   }
                                                }
                                            }
                                            else
                                            {
                                               for(var i = 0; i < s.cpVisibleRowCount; i++)
                                                { 
                                                   if (s.IsRowSelectedOnPage(i)) 
                                                   {                                               
                                                      s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                                  }
                                               }
                                            }
                                            
                                        }        
                                    }
                                else{
                                var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                if (e.item.name =='CopyToAllCell')
                                {
                                    for(var i = 0; i < s.cpVisibleRowCount; i++)
                                    { 
                                        if (s.IsRowSelectedOnPage(i)) 
                                        {
                                            s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                            
                                        }
                                    }
                                }    
                             }
                                }
                                e.processOnServer = false;
                            }";
                        //gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                        //    { 
                        //        if (s.IsRowSelectedOnPage(e.elementIndex))  
                        //        { 
                        //            var FocusedColumn = sessionStorage.getItem('SampleRegistrationCopyFocusedColumn');  
                        //            var text;
                        //            if(FocusedColumn.includes('.'))
                        //            {  
                        //                oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                        //                text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                        //                if (e.item.name =='CopyToAllCell')
                        //                {
                        //                    for(var i = 0; i < s.cpVisibleRowCount; i++)
                        //                    { 
                        //                        if (s.IsRowSelectedOnPage(i)) 
                        //                        {                                               
                        //                            s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                        //                        }
                        //                    }
                        //                }        
                        //            }
                        //        else{
                        //        var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                        //        if (e.item.name =='CopyToAllCell')
                        //        {
                        //             console.log(FocusedColumn);
                        //             if(FocusedColumn=='StationLocation.Oid')
                        //                      {
                                          
                                    
                        //                      }
                        //                      else
                        //                      {
                        //                          for(var i = 0; i < s.cpVisibleRowCount; i++)
                        //                         { 
                        //                        if (s.IsRowSelectedOnPage(i)) 
                        //                          {
                        //                               s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                             
                        //                           }
                        //                        }
                                                 
                        //                    }
                                     
                                   
                        //        }    
                        //     }
                        //        }
                        //        e.processOnServer = false;
                        //    }";
                    }
                    CriteriaOperator cs = CriteriaOperator.Parse("RegistrationID=?", objSMInfo.strJobID);
                    SamplingProposal objsamplecheckin = ObjectSpace.FindObject<SamplingProposal>(cs);
                    if (gridListEditor != null && objsamplecheckin != null)
                    {
                        List<SamplingMatrixSetupFields> lstFields = new List<SamplingMatrixSetupFields>();
                        if (!string.IsNullOrEmpty(objsamplecheckin.SampleMatries))
                        {
                            List<string> lstSMOid = objsamplecheckin.SampleMatries.Split(';').ToList();
                            foreach (string strOid in lstSMOid)
                            {
                                VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strOid.Trim()));
                                if (objVM != null && objVM.SamplingSetupFields.Count > 0)
                                {
                                    foreach (SamplingMatrixSetupFields objField in objVM.SamplingSetupFields)
                                    {
                                        if (lstFields.FirstOrDefault(i => i.Oid == objField.Oid) == null)
                                        {
                                            lstFields.Add(objField);
                                        }
                                    }
                                }
                            }
                        }
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            ASPxGridView gridView = (ASPxGridView)gridListEditor.Grid;
                            if (gridView != null)
                            {
                                gridView.Settings.UseFixedTableLayout = true;
                                gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                                gridView.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                foreach (WebColumnBase column in gridView.Columns)
                                {
                                    if (column.Name == "SelectionCommandColumn" || column.Name == "Test" || column.Name == "Containers")
                                    {
                                        //gridView.VisibleColumns[column.Name].FixedStyle = GridViewColumnFixedStyle.Left;
                                        //column.Width = 5;
                                    }
                                    else
                                    {
                                        IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                        if (columnInfo != null)
                                        {
                                            SamplingMatrixSetupFields curField = lstFields.FirstOrDefault(i => i.FieldID == columnInfo.Model.Id);
                                            if (curField != null)
                                            {
                                                if (columnInfo.Model.Id == "Containers" || columnInfo.Model.Id == "BottleQty")
                                                {
                                                    column.Visible = false;
                                                }
                                                else
                                                {
                                                    column.Visible = true;
                                                    if (!string.IsNullOrEmpty(curField.FieldCustomCaption))
                                                    {
                                                        column.Caption = curField.FieldCustomCaption;
                                                    }
                                                    else
                                                    {
                                                        column.Caption = curField.FieldCaption;
                                                    }
                                                    if (curField.SortOrder > 0)
                                                    {
                                                        column.VisibleIndex = curField.SortOrder + 3;
                                                    }
                                                    if (curField.Freeze)
                                                    {
                                                        gridView.VisibleColumns[columnInfo.Model.Id].FixedStyle = GridViewColumnFixedStyle.Left;
                                                    }
                                                    if (curField.Width > 0)
                                                    {
                                                        column.Width = curField.Width;
                                                    }
                                                }
                                            }

                                            else
                                            {
                                                if (columnInfo.Model.Id == "SampleID" || columnInfo.Model.Id == "SampleName" || columnInfo.Model.Id == "VisualMatrix" || columnInfo.Model.Id == "RecievedDate"
                                                   || columnInfo.Model.Id == "ClientSampleID" || columnInfo.Model.Id == "StationLocation" || columnInfo.Model.Id == "CollectDate" || columnInfo.Model.Id == "SampleSource" || columnInfo.Model.Id == "SysSampleCode"
                                                   || columnInfo.Model.Id == "Department")
                                                {
                                                    column.Visible = true;
                                                }
                                                else
                                                {
                                                    column.Visible = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
              else if (View.Id == "SamplingTest" && objSMInfo.IsTestcanFilter)
                {
                    objSMInfo.IsTestcanFilter = false;
                    List<object> groups = new List<object>();
                    DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
                    DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                    DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
                    UnitOfWork uow = new UnitOfWork(((XPObjectSpace)this.ObjectSpace).Session.DataLayer);
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Testparameter)))
                    {
                        string criteria = string.Empty;
                        if (objSMInfo.lstdupfilterstr != null && objSMInfo.lstdupfilterstr.Count > 0)
                        {
                            foreach (string test in objSMInfo.lstdupfilterstr)
                            {
                                var testsplit = test.Split('|');
                                if (testsplit.Length == 4)
                                {
                                    XPClassInfo TestParameterinfo = uow.GetClassInfo(typeof(Testparameter));
                                    IList<Testparameter> testparameters = uow.GetObjects(TestParameterinfo, CriteriaOperator.Parse("([TestMethod.TestName] ='" + testsplit[0] + "' and [TestMethod.MethodName.MethodNumber] ='" + testsplit[1] + "' and [TestMethod.MatrixName.MatrixName] ='" + testsplit[2] + "' and [Component.Components] ='" + testsplit[3] + "') And TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null And TestMethod.MatrixName.GCRecord Is Null And [QCType.QCTypeName] = 'Sample'"), null, int.MaxValue, false, true).Cast<Testparameter>().ToList();
                                    
                                    if (criteria == string.Empty)
                                    {
                                        criteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testparameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                    }
                                    else
                                    {
                                        criteria = criteria + "and Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testparameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                    }
                                }
                                else if (testsplit.Length == 3)
                                {
                                    XPClassInfo TestParameterinfo = uow.GetClassInfo(typeof(Testparameter));
                                    IList<Testparameter> testparameters = uow.GetObjects(TestParameterinfo, CriteriaOperator.Parse("([TestMethod.TestName] ='" + testsplit[0] + "' and [TestMethod.MatrixName.MatrixName] ='" + testsplit[1] + "' and [Component.Components] ='" + testsplit[2] + "') And TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null And TestMethod.MatrixName.GCRecord Is Null And [QCType.QCTypeName] = 'Sample'"), null, int.MaxValue, false, true).Cast<Testparameter>().ToList();
                                    if (criteria == string.Empty)
                                    {
                                        criteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testparameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                    }
                                    else
                                    {
                                        criteria = criteria + "and Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testparameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(criteria))
                        {
                            lstview.Criteria = new GroupOperator(GroupOperatorType.And,
                                       CriteriaOperator.Parse(criteria)
                                       , CriteriaOperator.Parse("TestMethod.GCRecord Is Null And TestMethod.MatrixName.GCRecord Is Null And (([IsGroup] = False And [TestMethod.MethodName.GCRecord] Is Null And  [QCType.QCTypeName] = 'Sample') Or [IsGroup] = True) "));
                        }
                        else
                        {
                            lstview.Criteria = CriteriaOperator.Parse("TestMethod.GCRecord Is Null And TestMethod.MatrixName.GCRecord Is Null And (([IsGroup] = False And [TestMethod.MethodName.GCRecord] Is Null And  [QCType.QCTypeName] = 'Sample') Or [IsGroup] = True) ");
                        }
                        lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.TestName", true, true));
                        lstview.Properties.Add(new ViewProperty("TMethodName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.MethodName.MethodNumber", true, true));
                        lstview.Properties.Add(new ViewProperty("TMatrixName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.MatrixName.MatrixName", true, true));
                        lstview.Properties.Add(new ViewProperty("TComponentName", DevExpress.Xpo.SortDirection.Ascending, "Component.Components", true, true));
                        lstview.Properties.Add(new ViewProperty("TIsGroup", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.IsGroup", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        if (objSMInfo.lstTestParameter != null && objSMInfo.lstTestParameter.Count > 0)
                        {
                            if (objSMInfo.lstdupfilterguid != null && objSMInfo.lstdupfilterguid.Count > 0)
                            {
                                foreach (Guid guid in objSMInfo.lstdupfilterguid)
                                {
                                    groups.Add(guid);
                                }
                            }
                            ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", objSMInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                            ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", objSMInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        }
                        else
                        {
                            ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                        }
                        ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"] = new InOperator("Oid", groups);
                        ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["dupfilter"] = new InOperator("Oid", groups);
                        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");

                        foreach (Testparameter testparameter in ((ListView)TestViewSub.InnerView).CollectionSource.List.Cast<Testparameter>().ToList())
                        {
                            SamplingParameter objsampleparameter = TestViewSub.InnerView.ObjectSpace.FindObject<SamplingParameter>(CriteriaOperator.Parse("[Sampling.Oid] = ? And [Testparameter.Oid] = ?", objSMInfo.SampleOid, testparameter.Oid));
                            if (objsampleparameter != null)
                            {
                                testparameter.TAT = objsampleparameter.TAT;
                            }
                        }
                    }
                    if (objSamplingInfo.SLVisualMatrixName != null && staticText != null)
                    {
                        staticText.Text = objSamplingInfo.SLVisualMatrixName;
                    }
                }
              else if(View.Id== "Testparameter_LookupListView_Sampling_AvailableTest")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsPager.AlwaysShowPager = true;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e){ 
                s.SetWidth(380); 
                s.RowClick.ClearHandlers();
                }";
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName]='" + objSamplingInfo.SLVisualMatrixName + "' AND [TestMethod.GCRecord] IS NULL AND (([TestMethod.RetireDate] IS NULL OR [TestMethod.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')) AND " +
                      " ([TestMethod.MethodName.RetireDate] IS NULL OR [TestMethod.MethodName.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "') AND ([Parameter.RetireDate] IS NULL OR [Parameter.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')" + "AND([RetireDate] IS NULL OR[RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')" +
                       "AND ([InternalStandard] == False or [InternalStandard] IS NULL ) AND ([Surroagate] == False or [Surroagate] IS NULL)");
                }
              else if (View.Id == "Testparameter_LookupListView_Sampling_SeletectedTest")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsPager.AlwaysShowPager = true;
                    gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                    ICallbackManagerHolder seltest = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    seltest.CallbackManager.RegisterHandler("Subout", this);
                    string script = seltest.CallbackManager.GetScript();
                    script = string.Format(CultureInfo.InvariantCulture, @"
                        function(s, e) {{ 
                            var xafCallback = function() {{
                            s.EndCallback.RemoveHandler(xafCallback);
                            {0}
                            }};
                            s.EndCallback.AddHandler(xafCallback);
                        }}
                    ", script);
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = script;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e)
                    { 
                    s.SetWidth(500); 
                    s.RowClick.ClearHandlers();
                    if(s.cpCanGridRefresh)
                    {
                        s.Refresh();
                        s.cpCanGridRefresh = false;
                    }
                    }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e){
                              if(e.focusedColumn.fieldName == 'SubOut')
                                {
                                   var IsAttched= s.batchEditApi.GetCellValue(e.visibleIndex,'IsSubutAttached');
                                   if(IsAttched!=null && IsAttched)
                                     {
                                          e.cancel = true;
                                          alert('Test alredy attached in subout.')
                                     }
                                   
                                }
                 
                      }";

                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) 
                                                                                {


                    window.setTimeout(function() {
                    s.UpdateEdit();
                    }, 100);
                                                                                    var value= s.batchEditApi.GetCellValue(e.visibleIndex,'SubOut');
                                                                                    if(value == true)
                                                                                    {
                                                                                        RaiseXafCallback(globalCallbackControl, 'Subout', 'SuboutSelected|'+e.visibleIndex, '', false);  
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        RaiseXafCallback(globalCallbackControl, 'Subout', 'SuboutUnSelected|'+e.visibleIndex, '', false);  
                                                                                    }
                                                                                }";
                    if (objSMInfo.lstdupfilterSuboutstr != null && objSMInfo.lstdupfilterSuboutstr.Count > 0)
                    {
                        foreach (string test in objSMInfo.lstdupfilterSuboutstr)
                        {
                            var testsplit = test.Split('|');
                            IList<Testparameter> testparameters = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("([TestMethod.TestName] ='" + testsplit[0] + "' and [TestMethod.MethodName.MethodNumber] ='" + testsplit[1] + "' and [TestMethod.MatrixName.MatrixName] ='" + testsplit[2] + "')"));
                            foreach (Testparameter obj in ((ListView)View).CollectionSource.List.Cast<Testparameter>().ToList())
                            {
                                if (testparameters.Contains(obj))
                                {
                                    obj.SubOut = true;
                                }
                            }
                        }
                    }
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                }
              else if (View.Id == "Testparameter_LookupListView_Sampling_Parameter")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("Test", this);
                    gridListEditor.Grid.SettingsPager.AlwaysShowPager = true;
                    gridListEditor.Grid.CommandButtonInitialize += Grid_CommandButtonInitialize;
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.SettingsBehavior.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
                    gridListEditor.Grid.CustomColumnSort += Grid_CustomColumnSort;
                    gridListEditor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e){ 
                    s.SetWidth(350); 
                    s.RowClick.ClearHandlers();
                    }";
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e){
                      if(e.visibleIndex != '-1')
                      {                       
                        if (s.IsRowSelectedOnPage(e.visibleIndex)) {   
                            var value = 'Testselection|Selected|' + s.GetRowKey(e.visibleIndex);
                            RaiseXafCallback(globalCallbackControl, 'Test', value, '', false);    
                        }else{
                            var value = 'Testselection|UNSelected|' + s.GetRowKey(e.visibleIndex);
                            RaiseXafCallback(globalCallbackControl, 'Test', value, '', false);    
                        }
                     }
                     else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                     {        
                        RaiseXafCallback(globalCallbackControl, 'Test', 'Testselection|Selectall', '', false);                        
                     }   
                     else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                     {
                        RaiseXafCallback(globalCallbackControl, 'Test', 'Testselection|UNSelectall', '', false);                        
                     }
                     else if(e.visibleIndex == '-1' && s.cpFilterRowCount == s.cpVisibleRowCount)
                     {        
                        RaiseXafCallback(globalCallbackControl, 'Test', 'Testselection|Selectall', '', false);                        
                     }   
                     else if(e.visibleIndex == '-1' && s.cpFilterRowCount == 0)
                     {
                        RaiseXafCallback(globalCallbackControl, 'Test', 'Testselection|UNSelectall', '', false);                        
                     }
                    }";
                }
              else if(View.Id== "SamplingProposal_CustomDueDates_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("CustomDueDatesSampling", this);
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.JSProperties["cpDefaultduedatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CRDueDate");
                    gridListEditor.Grid.JSProperties["cpduedatemsg"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "validduedate");
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e){s.UpdateEdit();}";

                    if (objPmsInfo.SamplingProposalIsWrite == false)
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e){
                          e.cancel = true;
                                }";
                    }
                    else
                    {
                        gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s, e) 
                        {
                            if(sessionStorage.getItem('CurrFocusedColumn') == null)
                                {
                                    sessionStorage.setItem('PrevFocusedColumn', e.cellInfo.column.fieldName);
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }
                                else
                                {
                                    var precolumn = sessionStorage.getItem('CurrFocusedColumn');
                                    sessionStorage.setItem('PrevFocusedColumn', precolumn);                           
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }             
                        }";
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) 
                                                {
                                                    s.timerHandle = setTimeout(function() 
                                                    {

                        var duedate = s.batchEditApi.GetCellValue(e.visibleIndex, 'DueDate');
                        var defaultduedate = s.batchEditApi.GetCellValue(e.visibleIndex, 'DefaultDueDate');
                        var dt = new Date();
                        var today=dt.toISOString().split('T')[0];
                        var fieldName = sessionStorage.getItem('PrevFocusedColumn');
                        if(dt != null && duedate != null && defaultduedate != null)
                          {  
                             var duedates=duedate.toISOString().split('T')[0];
                             var defaultduedates=defaultduedate.toISOString().split('T')[0];
                             if( defaultduedates< duedates)  
                                {
                                    alert(s.cpDefaultduedatemsg);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DueDate', null);
                                    s.UpdateEdit();
                                } 
                              else if(today > duedate)
                                {
                                    alert(s.cpduedatemsg);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DueDate', null);
                                    s.UpdateEdit();
                                }
                          }
                           if(fieldName == 'DueDate' && s.batchEditApi.HasChanges(e.visibleIndex))
                            {
                              RaiseXafCallback(globalCallbackControl, 'CustomDueDatesSampling', 'DueDate' +'|'+e.visibleIndex, '', false)
                            }  
                          else if(fieldName == 'TAT.Oid' && s.batchEditApi.HasChanges(e.visibleIndex))
                           {
                              RaiseXafCallback(globalCallbackControl, 'CustomDueDatesSampling', 'TAT' +'|'+e.visibleIndex, '', false)
                           }                  
                       }, 100); 
                    }";
                        gridListEditor.Grid.ClientSideEvents.BatchEditChangesSaving = @"function(s, e){
                        for (var i in e.updatedValues) { 
var duedate = s.batchEditApi.GetCellValue(i, 'DueDate',false);
var defaultduedate = s.batchEditApi.GetCellValue(i, 'DefaultDueDate',false);
var dt = new Date();
var today=dt.toISOString().split('T')[0];
    if(dt != null && duedate != null && defaultduedate != null)
    {    
         var duedates=duedate.toISOString().split('T')[0];
         var defaultduedates=defaultduedate.toISOString().split('T')[0];
        if( defaultduedates< duedates)  
              {
                alert(s.cpDefaultduedatemsg);
                s.batchEditApi.SetCellValue(e.visibleIndex, 'DueDate', null);
                s.UpdateEdit();
              } 
              else if(today > duedate)
              {
              alert(s.cpduedatemsg);
              s.batchEditApi.SetCellValue(e.visibleIndex, 'DueDate', null);
              s.UpdateEdit();
              }
    }
                           }  
                        }";
                    }
                }
              else if (View.Id == "COCSettings_ListView_SamplingProposal_ImportCOC")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            s.timerHandle = setTimeout(function() {  
                                 if (s.batchEditApi.HasChanges()) {  
                                   s.UpdateEdit();  
                                 } 
                               }, 20);}";
                }
              else if(View.Id== "AnalysisPricing_ListView_Quotes_SamplingProposal")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.Load += Grid_Load;
                }
              else if(View.Id== "CRMQuotes_DetailView_SamplingProposal")
                {
                    if (((DetailView)View).CurrentObject==null)
                    {
                        SamplingProposal maincurrentobj = (SamplingProposal)Application.MainWindow.View.CurrentObject;
                        if (maincurrentobj != null)
                        {
                            ((DetailView)View).CurrentObject= View.ObjectSpace.GetObjectByKey<CRMQuotes>(maincurrentobj.QuoteID.Oid);
                            //DevExpress.ExpressApp.Web.PopupWindow nestedFrame = (DevExpress.ExpressApp.Web.PopupWindow)Frame;
                            //nestedFrame.View.CurrentObject = View.ObjectSpace.GetObjectByKey<CRMQuotes>(maincurrentobj.QuoteID.Oid);
                        } 
                    }
                }
              else if(View.Id== "SamplingParameter_ListView_SamplingProposal")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.CommandButtonInitialize += Grid_CommandButtonInitialize;
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 450;
                    }
                }

            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
           try
            {
                ASPxGridView grid = sender as ASPxGridView;
                if(e.NewValues["StationLocation"]==null)
                {
                    e.RowError = "Enter the stationlocation.";
                    grid.JSProperties["cpHasErrors"] = true;
                }
               
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (objPmsInfo.SamplingProposalIsWrite == true)
                {
                    if (e.DataColumn.FieldName == "Parameter")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'CustomDueDatesSampling', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
                {
                    if (View.Id == "Sampling_ListView_SamplingProposal" || View.Id == "SamplingParameter_ListView_SamplingProposal")
                    {
                        if (objPmsInfo.SamplingProposalIsWrite == false && objPmsInfo.SampleRegIsDelete == false)
                        {
                            e.Enabled = false;
                            IsDisableCheckBox = true;
                        }
                    }
                    else
                    {
                        Sampling objSamplelogin = View.ObjectSpace.FindObject<Sampling>(CriteriaOperator.Parse("[Oid] = ?", objSMInfo.SampleOid));
                        var curOid = gridView.GetRowValues(e.VisibleIndex, "Oid");
                        if (objSamplelogin != null && curOid != null)
                        {
                            SamplingParameter objsmpltest = View.ObjectSpace.FindObject<SamplingParameter>(CriteriaOperator.Parse("[Testparameter.Oid] = ? And [Sampling.Oid]= ?", curOid, objSamplelogin.Oid));
                            if (objSMInfo != null && objSMInfo.lstSavedTestParameter != null && objSMInfo.lstSavedTestParameter.Count > 0 && e.VisibleIndex != -1 && objSMInfo.lstSavedTestParameter.Contains((Guid)curOid) && objsmpltest != null && (!string.IsNullOrEmpty(objsmpltest.PrepBatchID) || !string.IsNullOrEmpty(objsmpltest.ResultNumeric)))
                            {
                                e.Enabled = false;
                                IsDisableCheckBox = true;
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
        private void Grid_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            try
            {
                if (View.Id == "SamplingParameter_ListView_SamplingProposal")
                {
                    if (e.Column != null & e.Column.FieldName == "Parent")
                    {
                        object SampleNo1 = e.GetRow1Value("Samplelogin.SampleNo");
                        object SampleNo2 = e.GetRow2Value("Samplelogin.SampleNo");
                        int res = Comparer.Default.Compare(SampleNo1, SampleNo2);
                        if (res == 0)
                        {
                            object Parent1 = e.Value1;
                            object Parent2 = e.Value2;
                            res = Comparer.Default.Compare(Parent1, Parent2);
                        }
                        e.Result = res;
                        e.Handled = true;
                    }
                }
                else if (View.Id == "Testparameter_LookupListView_Sampling_Parameter" || View.Id == "Testparameter_ListView_Parameter_Sampling")
                {
                    ASPxGridView grid = sender as ASPxGridView;
                    bool isRow1Selected = grid.Selection.IsRowSelectedByKey(e.GetRow1Value(grid.KeyFieldName));
                    bool isRow2Selected = grid.Selection.IsRowSelectedByKey(e.GetRow2Value(grid.KeyFieldName));
                    e.Handled = isRow1Selected != isRow2Selected;
                    if (e.Handled)
                    {
                        if (e.SortOrder == DevExpress.Data.ColumnSortOrder.Descending)
                            e.Result = isRow1Selected ? 1 : -1;
                        else
                            e.Result = isRow1Selected ? -1 : 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (View.Id == "Testparameter_LookupListView_Sampling_SeletectedTest")
                {
                    IsDisableCheckBox = false;
                    if (Frame != null && Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                        {
                            CompositeView cv = nestedFrame.ViewItem.View;
                            if (cv != null && cv is DashboardView)
                            {
                                DashboardViewItem SLListView = (DashboardViewItem)cv.FindItem("SampleLogin");
                            }
                        }
                    }
                    DashboardViewItem TestViewSubChild = ((NestedFrame)Frame).ViewItem.View.FindItem("TestViewSubChild") as DashboardViewItem;
                    DashboardViewItem TestViewSub = ((NestedFrame)Frame).ViewItem.View.FindItem("TestViewSub") as DashboardViewItem;
                    if (TestViewSubChild != null && TestViewSub != null && objSMInfo.UseSelchanged)
                    {
                        if (TestViewSub.InnerView.SelectedObjects.Count > 0)
                        {
                            List<Guid> lstTestOid = new List<Guid>();
                            foreach (Testparameter testparameter in TestViewSub.InnerView.SelectedObjects.Cast<Testparameter>().ToList().Where(i => i.IsGroup == false))
                            {
                                IList<Testparameter> listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodNumber]=? and [TestMethod.MatrixName.MatrixName] = ? and Component.Components=? And TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null And TestMethod.MatrixName.GCRecord Is Null And [QCType.QCTypeName] = 'Sample'", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components));
                                foreach (Guid obj in listseltest.ToList().Select(i => i.Oid))
                                {
                                    if (!lstTestOid.Contains(obj))
                                    {
                                        lstTestOid.Add(obj);
                                    }
                                }
                            }
                          ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstTestOid);
                        }
                        else
                        {
                            ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                        }
                        objSMInfo.strSelectionMode = "Selected";
                    }
                    else
                    {
                        objSMInfo.UseSelchanged = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void GridView_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    GridViewContextMenuItem item = null;
                    item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                    {
                        e.Items.Remove(item);
                    }
                    if (objLanguage.strcurlanguage != "En")
                    {
                        e.Items.Add("复制到所有单元格", "CopyToAllCell");
                    }
                    else
                    {
                        e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    }
                    GridViewContextMenuItem Edititem = e.Items.FindByName("EditRow");
                    if (Edititem != null)
                        Edititem.Visible = false;
                    item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";
                    e.Items.Remove(e.Items.FindByText("Edit"));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (objSMInfo.lstTestParameter != null && objSMInfo.lstTestParameter.Count > 0 && objSMInfo.strSelectionMode == "Selected")
                {
                    foreach (Guid obj in objSMInfo.lstTestParameter)
                    {
                        gridView.Selection.SelectRowByKey(obj);
                    }
                    objSMInfo.strSelectionMode = string.Empty;
                }
                else if (View.Id == "Testparameter_ListView_Parameter_Sampling" && objSMInfo.lstSelParameter.Count > 0)
                {
                    foreach (string obj in objSMInfo.lstSelParameter)
                    {
                        gridView.Selection.SelectRowByKey(obj);
                    }
                    objSMInfo.lstSelParameter.Clear();
                    View.Refresh();
                }
                else if (View.Id == "SampleLogIn_ListView_SampleRegistration_Bottle")
                {
                    if (((ListView)View).CollectionSource.List.Count == 1)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            gridView.Selection.SelectRow(i);
                        }
                    }
                    else if (((ListView)View).CollectionSource.List.Count > 1)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            if (samplingfirstdefault == true)
                            {
                                i = 1;
                                break;
                            }
                        }
                    }
                    gridView.Selection.SelectRowByKey(objSMInfo.SamplingGuid);
                }
                else if (View.Id == "Sampling_ListView_SamplingProposal")
                {
                    if (gridView.Columns["SelectionCommandColumn"] != null)
                    {
                        gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.Columns["SamplingTest"] != null)
                    {
                        gridView.VisibleColumns["SamplingTest"].FixedStyle = GridViewColumnFixedStyle.Left;
                        gridView.VisibleColumns["SamplingTest"].Width = 60;
                    }
                    if (gridView.Columns["SamplingContainers"] != null)
                    {
                        gridView.VisibleColumns["SamplingContainers"].FixedStyle = GridViewColumnFixedStyle.Left;
                        gridView.VisibleColumns["SamplingContainers"].Width = 60;
                    }
                    if (gridView.Columns["SampleID"] != null)
                    {
                        gridView.VisibleColumns["SampleID"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                }
                else if (View.Id == "Testparameter_LookupListView_Copy_SampleLogin_Copy_Parameter")
                {
                    if (IsDisableCheckBox)
                    {
                        var selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(i => i.ShowSelectCheckbox).FirstOrDefault();
                        if (selectionBoxColumn != null)
                        {
                            selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
                        }
                    }
                }
                else if (View.Id == "AnalysisPricing_ListView_Quotes_SamplingProposal")
                {
                    ASPxGridView gridview = (ASPxGridView)sender as ASPxGridView;
                    gridview.Selection.SelectAll();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                e.Properties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                e.Properties["cpFilterRowCount"] = gridView.Selection.FilteredCount;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Grid_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
        {
            try
            {
                if (View.Id == "Sampling_ListView_SamplingProposal")
                {
                    if (e.CommandCellType == GridViewTableCommandCellType.Data)
                    {
                        if (e.CommandColumn.Name == "SamplingTest")
                        {
                            e.Cell.Attributes.Add("onclick", jScript);
                            if (objPmsInfo.SamplingProposalIsWrite == false)
                            {
                                ((System.Web.UI.WebControls.WebControl)e.Cell.Controls[0]).Enabled = false;
                            }
                        }
                        else if (e.CommandColumn.Name == "SamplingContainers")
                        {
                            e.Cell.Attributes.Add("onclick", jScript);
                            if (objPmsInfo.SamplingProposalIsWrite == false)
                            {
                                ((System.Web.UI.WebControls.WebControl)e.Cell.Controls[0]).Enabled = false;
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
        private void Grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                if (grid.JSProperties.ContainsKey("cpHasErrors") && (bool)grid.JSProperties["cpHasErrors"])
                {
                    grid.JSProperties["cpHasErrors"] = true;
                    e.Handled = true;
                    return;
                }
                //UnitOfWork uow = new UnitOfWork(((XPObjectSpace)this.ObjectSpace).Session.DataLayer);
                //List<Sampling> lstSampleLogIn = ((ListView)View).CollectionSource.List.Cast<Sampling>().Where(i => i.SubOut == true).ToList();
                //foreach (Sampling objSampleLogIn in lstSampleLogIn)
                //{
                //    IList<SamplingParameter> objsample = uow.GetObjects(uow.GetClassInfo(typeof(SamplingParameter)), CriteriaOperator.Parse("[Sampling.Oid]=?", objSampleLogIn.Oid), null, int.MaxValue, false, true).Cast<SamplingParameter>().ToList();
                //    //List<SamplingTest> objsample = uow.Query<SamplingTest>().Where(i => i.Sampling != null && i.Sampling.Oid == objSampleLogIn.Oid).ToList();
                //    objsample.Where(i => i.Sampling != null && i.Sampling.SubOut == true).ToList().ForEach(i => i.SubOut = true);
                //}
                //uow.CommitChanges();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Editor_Load(object sender, EventArgs e)
        {
            try
            {
                DevExpress.Web.ASPxSpinEdit editor = sender as DevExpress.Web.ASPxSpinEdit;
                editor.MinValue = 1;
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
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                if (View.Id== "SamplingProposal_DetailView")
                {
                    ObjectSpace.Committing -= ObjectSpace_Committing;
                    DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule -= RuleSet_CustomNeedToValidateRule;
                    View.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Execute -= SaveAction_Execute;
                        modificationController.SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                        modificationController.SaveAndNewAction.Executing -= SaveAndNewAction_Executing;
                    }
                    Frame.GetController<WebModificationsController>().EditAction.Executed -= EditAction_Executed;
                }
                else if(View.Id== "SamplingProposal_ListView")
                {
                    if (SamplingProposalDateFilterAction!=null && SamplingProposalDateFilterAction.SelectedItem!=null)
                    {
                        SamplingProposalDateFilterAction.SelectedItemChanged -= DateFilterAction_SamplingProposal_SelectedItemChanged; 
                    }
                }
                else if(View.Id== "SamplingProposal_ListView_History")
                {
                    if (SamplingProposalHistoryDateFilterAction != null && SamplingProposalHistoryDateFilterAction.SelectedItem != null)
                    {
                        SamplingProposalHistoryDateFilterAction.SelectedItemChanged -= DateFilterAction_History_SamplingProposal_SelectedItemChanged;
                    }
                }
                else if (View.Id == "Sampling_ListView_SamplingProposal")
                {
                    ((XPObjectSpace)this.ObjectSpace).Session.TrackPropertiesModifications = false;
                    View.ObjectSpace.Committing -= ObjectSpace_Committing;
                }
                else if (View.Id == "SamplingProposal_DetailView_CopyRecurrence")
                {
                    ObjectSpace.Committed -= ObjectSpace_Committed;
                    View.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void RuleSet_CustomNeedToValidateRule(object sender, DevExpress.Persistent.Validation.CustomNeedToValidateRuleEventArgs e)
        {
            try
            {
                if (e.Rule.Id == "RegistrationID")
                {
                    SamplingProposalIDFormat objformat = View.ObjectSpace.FindObject<SamplingProposalIDFormat>(CriteriaOperator.Parse(""));
                    if (objformat != null && objformat.Dynamic == true)
                    {
                        e.NeedToValidateRule = false;
                        e.Handled = !e.NeedToValidateRule;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                var os = Application.CreateObjectSpace();
                Session currentSession = ((XPObjectSpace)(os)).Session;
                if (View.CurrentObject != null && View.Id == "SamplingProposal_DetailView")
                {
                    SamplingProposalIDFormat objJDformat = os.FindObject<SamplingProposalIDFormat>(CriteriaOperator.Parse(""));
                    if (objJDformat != null)
                    {
                        SamplingProposal obj = (SamplingProposal)View.CurrentObject;
                        if ((objSMInfo.bolNewJobID == true && View.ObjectSpace.IsNewObject(obj)) || string.IsNullOrEmpty(obj.RegistrationID))
                        {
                            objSMInfo.bolNewJobID = false;
                            if (objJDformat.Dynamic == true)
                            {
                                SelectedData sproc = currentSession.ExecuteSproc("GetRegistrationID", new OperandValue("Normal"));
                                strJobID = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                                if (!string.IsNullOrEmpty(obj.RegistrationID))
                                {
                                    if (obj.RegistrationID != strJobID)
                                    {
                                        var curdate = DateTime.Now;
                                        string strjobid = string.Empty;
                                        int formatlen = 0;

                                        if (objJDformat.Year == YesNoFilter.Yes)
                                        {
                                            strjobid += curdate.ToString(objJDformat.YearFormat.ToString());
                                            formatlen = objJDformat.YearFormat.ToString().Length;
                                        }
                                        if (objJDformat.Month == YesNoFilter.Yes)
                                        {
                                            strjobid += curdate.ToString(objJDformat.MonthFormat.ToUpper());
                                            formatlen = formatlen + objJDformat.MonthFormat.Length;
                                        }
                                        if (objJDformat.Day == YesNoFilter.Yes)
                                        {
                                            strjobid += curdate.ToString(objJDformat.DayFormat);
                                            formatlen = formatlen + objJDformat.DayFormat.Length;
                                        }
                                        CriteriaOperator sam = objJDformat.Prefix == YesNoFilter.Yes ? CriteriaOperator.Parse("Max(SUBSTRING(RegistrationID, " + objJDformat.PrefixValue.ToString().Length + "))") : CriteriaOperator.Parse("Max(SUBSTRING(RegistrationID, 0))");
                                        CriteriaOperator filternew = CriteriaOperator.Parse("[IsAlpacJobid]=1");
                                        string tempid = (Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(SamplingProposal), sam, filternew)) + 1).ToString();
                                      
                                        if (tempid != "1")
                                        {
                                            var predate = tempid.Substring(0, formatlen);
                                            if (predate == strjobid)
                                            {
                                                if (objJDformat.Prefix == YesNoFilter.Yes)
                                                {
                                                    if (!string.IsNullOrEmpty(objJDformat.PrefixValue))
                                                    {
                                                        strjobid = objJDformat.PrefixValue + tempid;
                                                    }
                                                }
                                                else
                                                {
                                                    strjobid = tempid;
                                                }
                                            }
                                            else
                                            {
                                                if (objJDformat.Prefix == YesNoFilter.Yes)
                                                {
                                                    if (!string.IsNullOrEmpty(objJDformat.PrefixValue))
                                                    {
                                                        strjobid = objJDformat.PrefixValue + strjobid;
                                                    }
                                                }
                                                if (objJDformat.SequentialNumber > 1)
                                                {
                                                    if (objJDformat.NumberStart > 0)
                                                    {
                                                        strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - objJDformat.NumberStart.ToString().Length)), '0') + objJDformat.NumberStart;
                                                    }
                                                    else
                                                    {
                                                        strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - 1)), '0') + "1";
                                                    }
                                                }
                                                else
                                                {
                                                    if (objJDformat.NumberStart > 0 && objJDformat.NumberStart < 10)
                                                    {
                                                        strjobid = strjobid + objJDformat.NumberStart;
                                                    }
                                                    else
                                                    {
                                                        strjobid = strjobid + "1";
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (objJDformat.Prefix == YesNoFilter.Yes)
                                            {
                                                if (!string.IsNullOrEmpty(objJDformat.PrefixValue))
                                                {
                                                    strjobid = objJDformat.PrefixValue + strjobid;
                                                }
                                            }
                                            if (objJDformat.SequentialNumber > 1)
                                            {
                                                if (objJDformat.NumberStart > 0)
                                                {
                                                    strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - objJDformat.NumberStart.ToString().Length)), '0') + objJDformat.NumberStart;
                                                }
                                                else
                                                {
                                                    strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - 1)), '0') + "1";
                                                }
                                            }
                                            else
                                            {
                                                if (objJDformat.NumberStart > 0 && objJDformat.NumberStart < 10)
                                                {
                                                    strjobid = strjobid + objJDformat.NumberStart;
                                                }
                                                else
                                                {
                                                    strjobid = strjobid + "1";
                                                }
                                            }
                                        }
                                        obj.RegistrationID = strJobID = strjobid;
                                        Application.ShowViewStrategy.ShowMessage("This RegistrationID has been used. The Newly Generated RegistrationID =" + strJobID, InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    }
                                }
                                else
                                {
                                    var curdate = DateTime.Now;
                                    string strjobid = string.Empty;
                                    int formatlen = 0;

                                    if (objJDformat.Year == YesNoFilter.Yes)
                                    {
                                        strjobid += curdate.ToString(objJDformat.YearFormat.ToString());
                                        formatlen = objJDformat.YearFormat.ToString().Length;
                                    }
                                    if (objJDformat.Month == YesNoFilter.Yes)
                                    {
                                        strjobid += curdate.ToString(objJDformat.MonthFormat.ToUpper());
                                        formatlen = formatlen + objJDformat.MonthFormat.Length;
                                    }
                                    if (objJDformat.Day == YesNoFilter.Yes)
                                    {
                                        strjobid += curdate.ToString(objJDformat.DayFormat);
                                        formatlen = formatlen + objJDformat.DayFormat.Length;
                                    }
                                    CriteriaOperator sam = objJDformat.Prefix == YesNoFilter.Yes ? CriteriaOperator.Parse("Max(SUBSTRING(RegistrationID, " + objJDformat.PrefixValue.ToString().Length + "))") : CriteriaOperator.Parse("Max(SUBSTRING(RegistrationID, 0))");
                                    CriteriaOperator filternew = CriteriaOperator.Parse("[IsAlpacJobid]=1");
                                    string tempid = (Convert.ToInt32(((XPObjectSpace)os).Session.Evaluate(typeof(SamplingProposal), sam, filternew)) + 1).ToString();
                                    if (tempid != "1")
                                    {
                                        var predate = tempid.Substring(0, formatlen);
                                        if (predate == strjobid)
                                        {
                                            if (objJDformat.Prefix == YesNoFilter.Yes)
                                            {
                                                if (!string.IsNullOrEmpty(objJDformat.PrefixValue))
                                                {
                                                    strjobid = objJDformat.PrefixValue + tempid;
                                                }
                                            }
                                            else
                                            {
                                                strjobid = tempid;
                                            }
                                        }
                                        else
                                        {
                                            if (objJDformat.Prefix == YesNoFilter.Yes)
                                            {
                                                if (!string.IsNullOrEmpty(objJDformat.PrefixValue))
                                                {
                                                    strjobid = objJDformat.PrefixValue + strjobid;
                                                }
                                            }
                                            if (objJDformat.SequentialNumber > 1)
                                            {
                                                if (objJDformat.NumberStart > 0)
                                                {
                                                    strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - objJDformat.NumberStart.ToString().Length)), '0') + objJDformat.NumberStart;
                                                }
                                                else
                                                {
                                                    strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - 1)), '0') + "1";
                                                }
                                            }
                                            else
                                            {
                                                if (objJDformat.NumberStart > 0 && objJDformat.NumberStart < 10)
                                                {
                                                    strjobid = strjobid + objJDformat.NumberStart;
                                                }
                                                else
                                                {
                                                    strjobid = strjobid + "1";
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (objJDformat.Prefix == YesNoFilter.Yes)
                                        {
                                            if (!string.IsNullOrEmpty(objJDformat.PrefixValue))
                                            {
                                                strjobid = objJDformat.PrefixValue + strjobid;
                                            }
                                        }
                                        if (objJDformat.SequentialNumber > 1)
                                        {
                                            if (objJDformat.NumberStart > 0)
                                            {
                                                strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - objJDformat.NumberStart.ToString().Length)), '0') + objJDformat.NumberStart;
                                            }
                                            else
                                            {
                                                strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - 1)), '0') + "1";
                                            }
                                        }
                                        else
                                        {
                                            if (objJDformat.NumberStart > 0 && objJDformat.NumberStart < 10)
                                            {
                                                strjobid = strjobid + objJDformat.NumberStart;
                                            }
                                            else
                                            {
                                                strjobid = strjobid + "1";
                                            }
                                        }
                                    }
                                    obj.RegistrationID = strJobID = strjobid;
                                }
                            }
                        }
                        if (obj != null && obj.CustomDueDates.Count > 0)
                        {
                            foreach (CustomDueDate objCDD in obj.CustomDueDates)
                            {
                                if (objCDD.TestMethod.IsGroup == false)
                                {
                                    if (objCDD.TestHold == true)
                                    {
                                        IList<SamplingParameter> lstSP = View.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[Sampling.SamplingProposal.RegistrationID] = ? And [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.TestMethod.TestName] = ?",
                                        obj.RegistrationID, objCDD.TestMethod.MatrixName.MatrixName, objCDD.TestMethod.MethodName.MethodNumber, objCDD.TestMethod.TestName));
                                        IList<SamplingParameter> lstQCSP = View.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail.Jobid] = ? And [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.TestMethod.TestName] = ?",
                                        obj.RegistrationID, objCDD.TestMethod.MatrixName.MatrixName, objCDD.TestMethod.MethodName.MethodNumber, objCDD.TestMethod.TestName));
                                        lstSP.ToList().ForEach(i => i.TestHold = true);
                                        lstQCSP.ToList().ForEach(i => i.TestHold = true);

                                    }
                                    else
                                    {
                                        IList<SamplingParameter> lstSP = View.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[Sampling.SamplingProposal.RegistrationID] = ? And [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.TestMethod.TestName] = ?",
                                        obj.RegistrationID, objCDD.TestMethod.MatrixName.MatrixName, objCDD.TestMethod.MethodName.MethodNumber, objCDD.TestMethod.TestName));
                                        IList<SamplingParameter> lstQCSP = View.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail.Jobid] = ? And [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.TestMethod.TestName] = ?",
                                        obj.RegistrationID, objCDD.TestMethod.MatrixName.MatrixName, objCDD.TestMethod.MethodName.MethodNumber, objCDD.TestMethod.TestName));
                                        lstSP.ToList().ForEach(i => i.TestHold = false);
                                        lstQCSP.ToList().ForEach(i => i.TestHold = false);
                                    }
                                }
                                else
                                {
                                    TestMethod objTm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [IsGroup]=true And [MethodName.GCRecord] Is Null", objCDD.TestMethod.TestName));
                                    if (objTm != null)
                                    {
                                        IList<GroupTestMethod> lstgrouptestmed = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objTm.Oid));
                                        foreach (GroupTestMethod objgtm in lstgrouptestmed.ToList())
                                        {
                                            if (objCDD.TestHold == true)
                                            {
                                                IList<SamplingParameter> lstSP = View.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[Sampling.SamplingProposal.RegistrationID] = ? And [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.TestMethod.TestName] = ?",
                                                obj.RegistrationID, objgtm.Tests.MatrixName.MatrixName, objgtm.Tests.MethodName.MethodNumber, objgtm.Tests.TestName));
                                                IList<SamplingParameter> lstQCSP = View.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail.Jobid] = ? And [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.TestMethod.TestName] = ?",
                                                obj.RegistrationID, objgtm.Tests.MatrixName.MatrixName, objgtm.Tests.MethodName.MethodNumber, objgtm.Tests.TestName));
                                                lstSP.ToList().ForEach(i => i.TestHold = true);
                                                lstQCSP.ToList().ForEach(i => i.TestHold = true);

                                            }
                                            else
                                            {
                                                IList<SamplingParameter> lstSP = View.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[Sampling.SamplingProposal.RegistrationID] = ? And [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.TestMethod.TestName] = ?",
                                                obj.RegistrationID, objgtm.Tests.MatrixName.MatrixName, objgtm.Tests.MethodName.MethodNumber, objgtm.Tests.TestName));
                                                IList<SamplingParameter> lstQCSP = View.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail.Jobid] = ? And [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.TestMethod.TestName] = ?",
                                                obj.RegistrationID, objgtm.Tests.MatrixName.MatrixName, objgtm.Tests.MethodName.MethodNumber, objgtm.Tests.TestName));
                                                lstSP.ToList().ForEach(i => i.TestHold = false);
                                                lstQCSP.ToList().ForEach(i => i.TestHold = false);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if(View.Id== "Sampling_ListView_SamplingProposal")
                {
                    if(View.ObjectSpace.ModifiedObjects.Count>0)
                    {
                        Type type = ((ListView)View).ObjectTypeInfo.Type;
                        if (type!=null)
                        {
                            XPClassInfo classInfo = ((XPObjectSpace)(View.ObjectSpace)).Session.GetClassInfo(type);
                            if (classInfo != null && View.ObjectSpace.ModifiedObjects.GetType()==typeof(Sampling))
                            {
                                XPMemberInfo memberInfo = classInfo.GetMember("SubOut");
                                if (memberInfo != null)
                                {
                                    if (View.ObjectSpace.ModifiedObjects.Cast<Sampling>().FirstOrDefault(i => PersistentBase.GetModificationsStore(i).GetPropertyModified(memberInfo)) != null)
                                    {
                                        foreach (Sampling objSample in View.ObjectSpace.ModifiedObjects.Cast<Sampling>().Where(i => PersistentBase.GetModificationsStore(i).GetPropertyModified(memberInfo)))
                                        {
                                            IList<SamplingParameter> lstParam = View.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[Sampling]=?", objSample.Oid));
                                            lstParam.ToList().ForEach(i => { i.SubOut = objSample.SubOut; });
                                        }
                                    }
                                }
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

        private void Sample_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                objSMInfo.IsSamplePopupClose = false;
                if (ObjectSpace.IsModified)
                {
                    ObjectSpace.CommitChanges();
                }
                SamplingProposal objsamplecheckin = (SamplingProposal)e.CurrentObject;
                if (!objSMInfo.isNoOfSampleDisable)
                {
                    InsertSamplesInSampleLogin();
                }
                objSMInfo.strJobID = null;
                if (objSMInfo.strJobID == null || objSMInfo.strJobID != objsamplecheckin.RegistrationID)
                {
                    objSMInfo.strJobID = objsamplecheckin.RegistrationID;
                    objSMInfo.focusedJobID = objsamplecheckin.RegistrationID;
                    IObjectSpace os = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(View.ObjectSpace, typeof(Sampling));
                    cs.Criteria["Filter"] = CriteriaOperator.Parse("[SamplingProposal.RegistrationID] = ? AND [SamplingProposal.GCRecord] is NULL", objSMInfo.strJobID);
                    //ListView lstsmpllogin = Application.CreateListView("SampleLogIn_ListView_Copy_SampleRegistration", cs,false);
                    DashboardView lstsmpllogin = Application.CreateDashboardView(os, "SamplingProposal_SampleLogin", false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(lstsmpllogin);
                    showViewParameters.CreatedView = lstsmpllogin;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.AcceptAction.Active["OkayBtn"] = false;
                    dc.CancelAction.Active["CancelBtn"] = false;
                    dc.ViewClosed += Dc_ViewClosed;
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

        private void Dc_ViewClosed(object sender, EventArgs e)
        {
           try
            {
                if(Application.MainWindow.View.Id== "SamplingProposal_DetailView")
                {
                    SamplingProposal objSamplingProposal = (SamplingProposal)Application.MainWindow.View.CurrentObject;
                    if(objSamplingProposal!=null)
                    {
                        List<Sampling> lstSamples = View.ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("SamplingProposal.Oid=?", objSamplingProposal.Oid)).ToList();
                        if(lstSamples.Count==0)
                        {
                            objSamplingProposal.NoOfSamples = 1;
                            Application.MainWindow.View.Refresh();
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

        private void AddSample_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                CriteriaOperator cs = CriteriaOperator.Parse("RegistrationID=?", objSMInfo.strJobID);
                Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                SamplingProposal objsamplecheckin = uow.FindObject<SamplingProposal>(cs);
                Sampling objcheckin = new Sampling(uow);
                objcheckin.SamplingProposal = objsamplecheckin;
                int sampleno = 0;
                SelectedData sproc = currentSession.ExecuteSproc("GetSamplingSampleID", new OperandValue(objsamplecheckin.RegistrationID));
                if (sproc.ResultSet[1].Rows[0].Values[0].ToString() != null)
                {
                    sampleno = (int)sproc.ResultSet[1].Rows[0].Values[0];
                }
                if (objsamplecheckin.DateCollected != null && objsamplecheckin.DateCollected != DateTime.MinValue)
                {
                    objcheckin.CollectDate = (DateTime)objsamplecheckin.DateCollected;
                }
                if (objsamplecheckin.Collector != null)
                {
                    objcheckin.Collector = uow.GetObjectByKey<Collector>(objsamplecheckin.Collector.Oid);
                }
                if (objsamplecheckin != null && !string.IsNullOrEmpty(objsamplecheckin.SampleMatries) && !objsamplecheckin.SampleMatries.Contains(";"))
                {
                    VisualMatrix vs = uow.GetObjectByKey<VisualMatrix>(new Guid(objsamplecheckin.SampleMatries.Trim()));
                    objcheckin.VisualMatrix = vs;
                }
                objcheckin.SampleNo = sampleno;
               
                objcheckin.Save();
                if (objcheckin.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                {
                    Frame.GetController<AuditlogViewController>().insertauditdata(uow, objcheckin.SamplingProposal.Oid, OperationType.Created, "Sampling Proposal", objcheckin.SamplingProposal.RegistrationID, "Samples", "", objcheckin.SampleID, "");
                }
                uow.CommitChanges();
                ((ListView)View).CollectionSource.Add(((ListView)View).ObjectSpace.GetObject(objcheckin));
                View.Refresh();
            }

            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Test_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                try
                {
                    UnitOfWork uow = new UnitOfWork(((XPObjectSpace)this.ObjectSpace).Session.DataLayer);
                    Sampling samplelogIn = (Sampling)e.CurrentObject;
                    //if (samplelogIn != null && samplelogIn.Department == null)
                    //{
                    //    objSMInfo.strSampleID = "error";
                    //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectdept"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //    return;
                    //}
                    if (samplelogIn != null && samplelogIn.VisualMatrix != null)
                    {
                        objSMInfo.IsTestAssignmentClosed = false;
                        objSamplingInfo.SLVisualMatrixName = samplelogIn.VisualMatrix.MatrixName.MatrixName;
                        DashboardView dashboard = Application.CreateDashboardView(ObjectSpace, "SamplingTest", false);
                        ShowViewParameters showViewParameters = new ShowViewParameters(dashboard);
                        showViewParameters.Context = TemplateContext.NestedFrame;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        showViewParameters.CreatedView.Closed += CreatedView_Closed;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.AcceptAction.Active.SetItemValue("disable", false);
                        dc.CancelAction.Active.SetItemValue("disable", false);
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        if (objLanguage.strcurlanguage != "En")
                        {
                            showViewParameters.CreatedView.Caption = "选择检测项目 - " + samplelogIn.SampleID;
                        }
                        else
                        {
                            showViewParameters.CreatedView.Caption = "Test Assignment - " + samplelogIn.SampleID;
                        }
                        objSMInfo.SampleOid = samplelogIn.Oid;
                        objSMInfo.lstTestParameter = new List<Guid>();
                        objSMInfo.lstSavedTestParameter = new List<Guid>();
                        objSMInfo.lstdupfilterguid = new List<Guid>();
                        objSMInfo.lstdupfilterstr = new List<string>();
                        objSMInfo.lstSubOutTest = new List<Guid>();
                        objSMInfo.lstdupfilterSuboutstr = new List<string>();
                        objSMInfo.lstRemoveTestParameter = new List<Guid>();
                        IList<SamplingParameter> objsample = uow.GetObjects(uow.GetClassInfo(typeof(SamplingParameter)), CriteriaOperator.Parse("[Sampling]=? And [Testparameter.GCRecord] is null", samplelogIn.Oid), null, int.MaxValue, false, true).Cast<SamplingParameter>().ToList();
                        //List<SamplingTest> objsample = uow.Query<SamplingTest>().Where(i => i.Sampling != null && i.Sampling.Oid == samplelogIn.Oid).ToList();
                        if (objsample != null && objsample.Count > 0)
                        {
                            foreach (SamplingParameter sample in objsample.ToList())
                            {
                                if (!objSMInfo.lstTestParameter.Contains(sample.Testparameter.Oid))
                                {
                                    if (sample.IsGroup != true)
                                    {
                                        if (sample.Testparameter.TestMethod != null && sample.Testparameter.TestMethod.MethodName != null && sample.Testparameter.TestMethod.MatrixName != null && sample.Testparameter.Component != null)
                                        {
                                            if (!objSMInfo.lstdupfilterstr.Contains(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components))
                                            {
                                                objSMInfo.lstdupfilterstr.Add(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components);
                                                objSMInfo.lstdupfilterguid.Add(sample.Testparameter.Oid);
                                            }
                                        }
                                        else if (sample.Testparameter.TestMethod != null && sample.Testparameter.TestMethod.MatrixName != null && sample.Testparameter.Component != null)
                                        {
                                            if (!objSMInfo.lstdupfilterstr.Contains(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components))
                                            {
                                                objSMInfo.lstdupfilterstr.Add(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components);
                                                objSMInfo.lstdupfilterguid.Add(sample.Testparameter.Oid);
                                            }
                                        }
                                        objSMInfo.lstSavedTestParameter.Add(sample.Testparameter.Oid);
                                        objSMInfo.lstTestParameter.Add(sample.Testparameter.Oid);
                                        if (sample.SubOut == true)
                                        {
                                            if (!objSMInfo.lstSubOutTest.Contains(sample.Testparameter.TestMethod.Oid))
                                            {
                                                if (!objSMInfo.lstdupfilterSuboutstr.Contains(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components))
                                                {
                                                    objSMInfo.lstdupfilterSuboutstr.Add(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components);
                                                }
                                                objSMInfo.lstSubOutTest.Add(sample.Testparameter.TestMethod.Oid);
                                            }
                                        }
                                        else if (sample.Sampling.SubOut == true)
                                        {
                                            if (!objSMInfo.lstdupfilterSuboutstr.Contains(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components))
                                            {
                                                objSMInfo.lstdupfilterSuboutstr.Add(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components);
                                            }
                                            objSMInfo.lstSubOutTest.Add(sample.Testparameter.TestMethod.Oid);
                                        }
                                    }
                                    else
                                    {
                                        GroupTestMethod objgtm = uow.FindObject<GroupTestMethod>(CriteriaOperator.Parse("[Oid] =?", sample.GroupTest.Oid));
                                        if (objgtm != null && objgtm.TestMethod != null && objgtm.TestMethod.Oid != null)
                                        {
                                            IList<Testparameter> testparameters = uow.GetObjects(uow.GetClassInfo(typeof(Testparameter)), CriteriaOperator.Parse("[TestMethod.Oid]=?", objgtm.TestMethod.Oid), null, int.MaxValue, false, true).Cast<Testparameter>().ToList();
                                            //List<Testparameter> testparameters = uow.Query<Testparameter>().Where(i => i.TestMethod != null && i.TestMethod.Oid == objgtm.TestMethod.Oid).ToList();
                                            foreach (Testparameter objtp in testparameters.ToList())
                                            {
                                                if (!objSMInfo.lstdupfilterguid.Contains(objtp.Oid))
                                                {
                                                    objSMInfo.lstdupfilterguid.Add(objtp.Oid);
                                                }
                                                if (!objSMInfo.lstSavedTestParameter.Contains(objtp.Oid))
                                                {
                                                    objSMInfo.lstSavedTestParameter.Add(objtp.Oid);
                                                }
                                                if (!objSMInfo.lstTestParameter.Contains(objtp.Oid))
                                                {
                                                    objSMInfo.lstTestParameter.Add(objtp.Oid);
                                                }
                                            }
                                        }
                                        if (sample.SubOut == true)
                                        {
                                            if (!objSMInfo.lstSubOutTest.Contains(sample.Testparameter.TestMethod.Oid))
                                            {
                                                if (!objSMInfo.lstdupfilterSuboutstr.Contains(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components))
                                                {
                                                    objSMInfo.lstdupfilterSuboutstr.Add(sample.Testparameter.TestMethod.TestName + "|" + sample.Testparameter.TestMethod.MethodName.MethodNumber + "|" + sample.Testparameter.TestMethod.MatrixName.MatrixName + "|" + sample.Testparameter.Component.Components);
                                                }
                                                objSMInfo.lstSubOutTest.Add(sample.Testparameter.TestMethod.Oid);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        objSMInfo.IsTestcanFilter = true;
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                    }
                    else
                    {
                        objSMInfo.strSampleID = "error";
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectmatrix"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
                catch (Exception ex)
                {
                    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        private void CreatedView_Closed(object sender, EventArgs e)
        {
            try
            {
                if (Frame is NestedFrame)
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    if (nestedFrame != null)
                    {
                        CompositeView view = nestedFrame.ViewItem.View;
                        foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                        {
                            if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                            {
                                if (frameContainer.Frame.View is DetailView)
                                {
                                    frameContainer.Frame.View.ObjectSpace.ReloadObject(frameContainer.Frame.View.CurrentObject);
                                }
                                else
                                {
                                    (frameContainer.Frame.View as DevExpress.ExpressApp.ListView).CollectionSource.Reload();
                                }
                                frameContainer.Frame.View.Refresh();
                            }
                        }
                        objSMInfo.IsTestAssignmentClosed = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void InsertSamplesInSampleLogin()
        {
            try
            {
                SamplingProposal objSampleCheckin = (SamplingProposal)Application.MainWindow.View.CurrentObject;
                if (objSampleCheckin != null && objSampleCheckin.NoOfSamples > 0)
                {
                    UnitOfWork uow = new UnitOfWork(((XPObjectSpace)this.ObjectSpace).Session.DataLayer);
                    Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                    SamplingProposal obj = uow.GetObjectByKey<SamplingProposal>(objSampleCheckin.Oid);
                    Collector objCollector = null;
                    if (objSampleCheckin.Collector != null)
                    {
                        objCollector = uow.GetObjectByKey<Collector>(objSampleCheckin.Collector.Oid);
                    }
                    bool DBAccess = true;
                    int SampleNo = 0;
                    for (int i = 1; i <= objSampleCheckin.NoOfSamples; i++)
                    {
                        Sampling objSLNew = new Sampling(uow);
                        objSLNew.SamplingProposal = obj;
                        if (string.IsNullOrEmpty(objSampleCheckin.SampleMatries) == false)
                        {
                            string[] strSamplematrix = objSampleCheckin.SampleMatries.Split(';');
                            if (strSamplematrix.Count() == 1)
                            {
                                objSLNew.VisualMatrix = uow.GetObjectByKey<Modules.BusinessObjects.Setting.VisualMatrix>(new Guid(strSamplematrix[0].Trim()));
                            }
                        }
                        objSLNew.BatchID = objSampleCheckin.BatchID;
                        objSLNew.PackageNumber = objSampleCheckin.PackageNo;
                        if (obj.DateCollected != null && obj.DateCollected != DateTime.MinValue)
                        {
                            objSLNew.CollectDate = Convert.ToDateTime(obj.DateCollected);
                        }
                        if (objCollector != null)
                        {
                            objSLNew.Collector = objCollector;
                        }
                        if (DBAccess)
                        {
                            SelectedData sprocs = currentSession.ExecuteSproc("GetSamplingSampleID", new OperandValue(objSLNew.SamplingProposal.RegistrationID.ToString()));
                            if (sprocs.ResultSet[1].Rows[0].Values[0] != null)
                            {
                                objSMInfo.SampleID = sprocs.ResultSet[1].Rows[0].Values[0].ToString();
                                SampleNo = Convert.ToInt32(objSMInfo.SampleID);
                                DBAccess = false;
                            }
                            else
                            {
                                return;
                            }
                        }
                        objSLNew.SampleNo = SampleNo;
                        uow.CommitChanges();
                        if (!string.IsNullOrEmpty(objSampleCheckin.NPTest) && objSLNew.VisualMatrix != null)
                        {
                            List<CustomDueDate> lstcustomrequest = uow.Query<CustomDueDate>().Where(j => j.SamplingProposal.Oid == objSampleCheckin.Oid).ToList();
                            VisualMatrix objVisualMatrix = uow.GetObjectByKey<VisualMatrix>(objSLNew.VisualMatrix.Oid);
                            List<string> lstTestNames = objSampleCheckin.NPTest.Split(';').ToList();

                            foreach (string objTest in lstTestNames.ToList())
                            {
                                List<string> lstTestMethodCompo = objTest.Split('|').ToList();
                                if (lstTestMethodCompo.Count == 2)
                                {
                                    CustomDueDate custom = lstcustomrequest.Where(j => j.TestMethod != null && j.TestMethod.MatrixName != null && j.TestMethod.MethodName != null && j.TestMethod.MatrixName.MatrixName == objVisualMatrix.MatrixName.MatrixName && j.TestMethod.TestName == lstTestMethodCompo[0] && j.TestMethod.MethodName.MethodNumber == lstTestMethodCompo[1]).FirstOrDefault();
                                    List<Testparameter> lstTestParam = uow.Query<Testparameter>().Where(j => j.TestMethod != null && j.TestMethod.MatrixName != null && j.TestMethod.MethodName != null && j.Component != null && j.TestMethod.MatrixName.MatrixName == objVisualMatrix.MatrixName.MatrixName && j.TestMethod.TestName == lstTestMethodCompo[0] && j.TestMethod.MethodName.MethodNumber == lstTestMethodCompo[1] && j.Component.Components == "Default" && j.QCType != null && j.QCType.QCTypeName == "Sample").ToList();
                                    if (lstTestParam.Count > 0 && custom != null)
                                    {
                                        if (custom.Parameter == null || custom.Parameter == "AllParam")
                                        {
                                            foreach (Testparameter objTestParam in lstTestParam.ToList())
                                            {
                                                SamplingParameter objsp = ObjectSpace.FindObject<SamplingParameter>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [Sampling.Oid] = ?", objTestParam.Oid, objSLNew.Oid));
                                                if (objsp == null)
                                                {
                                                    SamplingParameter newsample = new SamplingParameter(uow);
                                                    newsample.Sampling = uow.GetObjectByKey<Sampling>(objSLNew.Oid);
                                                    newsample.Testparameter = objTestParam;
                                                    newsample.Status = Modules.BusinessObjects.Hr.Samplestatus.PendingEntry;
                                                    objSLNew.Test = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string[] param = custom.ParameterDetails.Split(',');
                                            foreach (Testparameter objTestParam in lstTestParam.ToList())
                                            {
                                                if (param.Contains(objTestParam.Oid.ToString()))
                                                {
                                                    SamplingParameter objsp = ObjectSpace.FindObject<SamplingParameter>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [Sampling.Oid] = ?", objTestParam.Oid, objSLNew.Oid));
                                                    if (objsp == null)
                                                    {
                                                        SamplingParameter newsample = new SamplingParameter(uow);
                                                        newsample.Sampling = uow.GetObjectByKey<Sampling>(objSLNew.Oid);
                                                        newsample.Testparameter = objTestParam;
                                                        newsample.Status = Modules.BusinessObjects.Hr.Samplestatus.PendingEntry;
                                                        objSLNew.Test = true;
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                                else if (lstTestMethodCompo.Count == 1)
                                {
                                    TestMethod objTm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [IsGroup]=true And [MethodName.GCRecord] Is Null", lstTestMethodCompo[0]));
                                    if (objTm != null)
                                    {
                                        IList<GroupTestMethod> lstgrouptestmed = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objTm.Oid));
                                        foreach (GroupTestMethod objgtm in lstgrouptestmed.ToList())
                                        {
                                            CustomDueDate custom = lstcustomrequest.Where(j => j.TestMethod != null && j.TestMethod.MatrixName != null && j.TestMethod.MatrixName.MatrixName == objVisualMatrix.MatrixName.MatrixName && j.TestMethod.Oid == objgtm.TestMethod.Oid).FirstOrDefault();
                                            IList<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objgtm.TestParameter.TestMethod.Oid));
                                            if (custom != null && custom.Parameter == null || custom != null && custom.Parameter == "AllParam")
                                            {
                                                foreach (Testparameter param1 in lsttestpara.ToList())
                                                {
                                                    SamplingParameter objsp = ObjectSpace.FindObject<SamplingParameter>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [Sampling.Oid] = ?", param1.Oid, objSLNew.Oid));
                                                    if (objsp == null)
                                                    {
                                                        SamplingParameter newsample = new SamplingParameter(uow);
                                                        newsample.Sampling = uow.GetObjectByKey<Sampling>(objSLNew.Oid);
                                                        newsample.Testparameter = uow.GetObjectByKey<Testparameter>(param1.Oid);
                                                        newsample.Status = Modules.BusinessObjects.Hr.Samplestatus.PendingEntry;
                                                        newsample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objgtm.Oid);
                                                        newsample.IsGroup = true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (custom != null && custom.ParameterDetails != null)
                                                {
                                                    string[] param = custom.ParameterDetails.Split(',');
                                                    foreach (Testparameter param1 in lsttestpara.ToList())
                                                    {
                                                        if (param.Contains(param1.Oid.ToString()))
                                                        {
                                                            SamplingParameter objsp = ObjectSpace.FindObject<SamplingParameter>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [Sampling.Oid] = ?", param1.Oid, objSLNew.Oid));
                                                            if (objsp == null)
                                                            {
                                                                SamplingParameter newsample = new SamplingParameter(uow);
                                                                newsample.Sampling = uow.GetObjectByKey<Sampling>(objSLNew.Oid);
                                                                newsample.Testparameter = uow.GetObjectByKey<Testparameter>(param1.Oid);
                                                                newsample.Status = Modules.BusinessObjects.Hr.Samplestatus.PendingEntry;
                                                                newsample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objgtm.Oid);
                                                                newsample.IsGroup = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        SampleNo++;
                    }
                    uow.CommitChanges();
                    IList<Sampling> samples = ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", objSampleCheckin.Oid));
                    foreach (Sampling sample in samples)
                    {
                        AssignBottleAllocationToSamples(uow, sample.Oid);
                    }
                    uow.CommitChanges();
                    if (this.Actions["btnSamplingTest"] != null)
                    {
                        this.Actions["btnSamplingTest"].Caption = "Tests" + "(" + View.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[Sampling.SamplingProposal.Oid] = ?", objSampleCheckin.Oid)).Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count() + ")";
                    }
                    if (this.Actions["btnSamplingBottleAllocation"] != null)
                    {
                        this.Actions["btnSamplingBottleAllocation"].Caption = "Containers" + "(" + View.ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", objSampleCheckin.Oid)).Sum(i => i.Qty) + ")";
                    }
                    
                }
            }


            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void AssignBottleAllocationToSamples(UnitOfWork uow, Guid sampleOid)
        {
            try
            {
                Sampling objSample = uow.GetObjectByKey<Sampling>(sampleOid);
                IList<SamplingBottleAllocation> objSampleBottle = uow.GetObjects(uow.GetClassInfo(typeof(SamplingBottleAllocation)), CriteriaOperator.Parse("[Sampling]=?", sampleOid), null, int.MaxValue, false, true).Cast<SamplingBottleAllocation>().ToList();
                IList<SamplingParameter> objSampleParameters = uow.GetObjects(uow.GetClassInfo(typeof(SamplingParameter)), CriteriaOperator.Parse("[Sampling]=?", objSample.Oid), null, int.MaxValue, false, true).Cast<SamplingParameter>().ToList();
                IList<TestMethod> lstTest = objSampleParameters.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod).Distinct().ToList();
                if (objSample != null && objSampleBottle != null && objSampleBottle.Count == 0)
                {
                    foreach (TestMethod test in lstTest.Where(i=>i.IsFieldTest!=true))
                    {
                        addnewtestbottles(uow, test, objSample);
                    }
                }
                else if (objSampleBottle != null && objSampleBottle.Count > 0)
                {
                    foreach (TestMethod test in lstTest.Where(i => i.IsFieldTest != true))
                    {
                        if (objSampleBottle.Where(a => a.TestMethod == test).ToList().Count == 0)
                        {
                            addnewtestbottles(uow, test, objSample);
                        }
                    }
                    foreach (SamplingBottleAllocation sampleBottle in objSampleBottle.ToList())
                    {
                        if (lstTest.Where(a => a == sampleBottle.TestMethod).ToList().Count == 0)
                        {
                            uow.Delete(sampleBottle);
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
        private void addnewtestbottles(UnitOfWork uow, TestMethod test, Sampling objSample)
        {
            List<Guid> lstContainer = new List<Guid>();
            List<Guid> lstPreservative = new List<Guid>();
            IList<Guid> containerNames = test.TestGuides.Where(i => i.Container != null).Select(i => i.Container.Oid).ToList();
            lstContainer.AddRange(containerNames.Except(lstContainer).ToList());
            IList<Guid> Preservative = test.TestGuides.Where(i => i.Preservative != null).Select(i => i.Preservative.Oid).ToList();
            lstPreservative.AddRange(Preservative.Except(lstPreservative).ToList());

            SamplingBottleAllocation objNewBottle = new SamplingBottleAllocation(uow);
            objNewBottle.Sampling = objSample;
            objNewBottle.BottleID = "A";
            objNewBottle.TestMethod = test;
            if (lstContainer.Count == 1)
            {
                Modules.BusinessObjects.Setting.Container objContainer = uow.FindObject<Modules.BusinessObjects.Setting.Container>(CriteriaOperator.Parse("Oid=?", lstContainer[0]));
                if (objContainer != null)
                {
                    objNewBottle.Containers = objContainer;
                }
            }
            if (lstPreservative.Count == 1)
            {
                Preservative objpreservative = uow.FindObject<Preservative>(CriteriaOperator.Parse("Oid=?", lstPreservative[0]));
                if (objpreservative != null)
                {
                    objNewBottle.Preservative = objpreservative;
                }
            }
            objNewBottle.Save();
        }
        private void btnQuoteImportSamples_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SamplingProposal_DetailView")
                {
                    SamplingProposal objSample = (SamplingProposal)View.CurrentObject;
                    if (objSample == null || objSample.QuoteID == null)
                    {
                        Application.ShowViewStrategy.ShowMessage("Quote ID cannot be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    if (objSample == null || objSample.TAT == null)
                    {
                        Application.ShowViewStrategy.ShowMessage("TAT cannot be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    if (objSample == null || objSample.SampleMatries == null)
                    {
                        Application.ShowViewStrategy.ShowMessage("Sample Matrices cannot be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    SamplingProposal objsamplecheckinloc = View.ObjectSpace.GetObject<SamplingProposal>((SamplingProposal)View.CurrentObject);
                    if (objsamplecheckinloc != null)
                    {
                        int objtestcount = View.ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("[SamplingProposal.Oid]=?", objsamplecheckinloc.Oid)).ToList().Count();
                        if (objtestcount > 0)
                        {
                            Application.ShowViewStrategy.ShowMessage("Sample details should be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;

                        }
                    }
                    IObjectSpace objspaceQ = Application.CreateObjectSpace();
                    CRMQuotes objToShow = objspaceQ.CreateObject<CRMQuotes>();
                    DetailView createDetailView = Application.CreateDetailView(objspaceQ, "CRMQuotes_DetailView_SamplingProposal", false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(createDetailView);
                    showViewParameters.CreatedView = createDetailView;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += dc_Accepting;
                    dc.CloseOnCurrentObjectProcessing = false;
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
        private void dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (View.Id== "SamplingProposal_DetailView")
                {
                    DevExpress.ExpressApp.SystemModule.DialogController dcCRM = (DevExpress.ExpressApp.SystemModule.DialogController)sender;
                    if (dcCRM != null && dcCRM.Frame != null && dcCRM.Frame.View != null)
                    {
                        ListPropertyEditor listAnalysis = ((DetailView)dcCRM.Frame.View).FindItem("AnalysisPricing") as ListPropertyEditor;
                        ListPropertyEditor listItemCharges = ((DetailView)dcCRM.Frame.View).FindItem("QuotesItemChargePrice") as ListPropertyEditor;
                        if (listAnalysis != null && listAnalysis.ListView != null && listAnalysis.ListView.SelectedObjects.Count > 0)
                        {
                            SamplingProposal objCurrent = (SamplingProposal)View.CurrentObject;
                            if (objCurrent != null)
                            {
                                int objtestcount = ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("[SamplingProposal.Oid]=?", objCurrent.Oid)).ToList().Count();
                                if (objtestcount > 0)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Sample details should be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                                string NPTest = string.Empty;
                                foreach (AnalysisPricing objAP in listAnalysis.ListView.SelectedObjects)
                                {
                                    if (NPTest.Length > 0)
                                    {
                                        if (objAP.Method != null)
                                            NPTest += objAP.Test.TestName + "|" + objAP.Method.MethodNumber + ";";
                                        else
                                            NPTest += objAP.Test.TestName + ";";
                                    }
                                    else
                                    {
                                        if (objAP.Method != null)
                                            NPTest = objAP.Test.TestName + "|" + objAP.Method.MethodNumber + ";";
                                        else
                                            NPTest = objAP.Test.TestName + ";";
                                    }
                                }
                               
                                objCurrent.NPTest = NPTest;
                                if (View.CurrentObject != null && ((SamplingProposal)View.CurrentObject).ClientName != null)
                                {
                                    objCurrent.ClientName = View.ObjectSpace.GetObject<Modules.BusinessObjects.Crm.Customer>(((SamplingProposal)View.CurrentObject).ClientName);
                                }
                                if (View.CurrentObject != null && ((SamplingProposal)View.CurrentObject).ProjectID != null)
                                {
                                    objCurrent.ProjectID = View.ObjectSpace.GetObject<Modules.BusinessObjects.Setting.Project>(((SamplingProposal)View.CurrentObject).ProjectID);
                                }
                                View.ObjectSpace.CommitChanges();
                                objCurrent = View.ObjectSpace.GetObject<SamplingProposal>((SamplingProposal)View.CurrentObject);
                                Session currentSession = ((XPObjectSpace)(View.ObjectSpace)).Session;
                                strSampleMatrix = objCurrent.SampleMatries.Split(';');
                                String strSampleMx = string.Join("','", strSampleMatrix).Replace(",' ", ",'");
                                strSampleMx = "'" + strSampleMx + "'";
                                DataTable dtSample = new DataTable();
                                dtSample.Columns.Add("Test");
                                dtSample.Columns.Add("Method");
                                dtSample.Columns.Add("Matrix");
                                dtSample.Columns.Add("VisualMatrix");
                                dtSample.Columns.Add("Comp");
                                dtSample.Columns.Add("Qty", typeof(Int32));
                                dtSample.Columns.Add("IsGroup", typeof(bool));

                                foreach (AnalysisPricing obj in listAnalysis.ListView.SelectedObjects)
                                {
                                    DataRow dr = dtSample.NewRow();
                                    dr["Test"] = obj.Test.TestName;
                                    if (obj.Method != null)
                                        dr["Method"] = obj.Method.MethodNumber;
                                    if (obj.SampleMatrix == null)
                                    {
                                        VisualMatrix lstviMx = ObjectSpace.FindObject<VisualMatrix>(CriteriaOperator.Parse(string.Format(" [MatrixName.MatrixName] ='{0}' and Oid in ({1}) ", obj.Matrix.MatrixName, strSampleMx)));
                                        dr["VisualMatrix"] = lstviMx.VisualMatrixName;
                                    }
                                    else
                                    {
                                        dr["VisualMatrix"] = obj.SampleMatrix.VisualMatrixName;
                                    }
                                    dr["Matrix"] = obj.Matrix.MatrixName;
                                    if (obj.Component != null)
                                        dr["Comp"] = obj.Component.Components;
                                    dr["Qty"] = obj.Qty;
                                    dr["IsGroup"] = obj.IsGroup;
                                    dtSample.Rows.Add(dr);
                                }

                                DataTable dtSampleQtyV = dtSample.DefaultView.ToTable(true, "VisualMatrix");
                                DataTable dtVMatrixQ = new DataTable();
                                dtVMatrixQ.Columns.Add("VisualMatrix");
                                dtVMatrixQ.Columns.Add("Qty", typeof(Int32));
                                foreach (DataRow drRow in dtSampleQtyV.Rows)
                                {

                                    DataRow drV = dtVMatrixQ.NewRow();
                                    drV["VisualMatrix"] = drRow["VisualMatrix"];
                                    drV["Qty"] = dtSample.Select("VisualMatrix = '" + drRow["VisualMatrix"] + "'").Max(r => r["Qty"]);
                                    dtVMatrixQ.Rows.Add(drV);
                                }

                                // =========================================Sampling Samples============================================
                                bool DBAccess = false;
                                string JobID = string.Empty;
                                int SampleNo = 0;
                                Session currSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                                UnitOfWork uow = new UnitOfWork(currSession.DataLayer);
                                foreach (DataRow drVM in dtVMatrixQ.Rows)
                                {
                                    for (int i = 1; i <= (int)drVM["Qty"]; i++)
                                    {
                                        Sampling objSLNew = new Sampling(uow);
                                        objSLNew.SamplingProposal = uow.GetObjectByKey<SamplingProposal>(objCurrent.Oid);
                                        if (DBAccess == false)
                                        {
                                            SelectedData sproc = currentSession.ExecuteSproc("GetSamplingSampleID", new OperandValue(objSLNew.SamplingProposal.RegistrationID.ToString()));
                                            if (sproc.ResultSet[1].Rows[0].Values[0] != null)
                                            {
                                                objSMInfo.SampleID = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                                                SampleNo = Convert.ToInt32(objSMInfo.SampleID);
                                                DBAccess = true;
                                            }
                                            else
                                            {
                                                return;
                                            }
                                        }
                                        objSLNew.SampleNo = SampleNo;

                                        objSLNew.Test = true;


                                        objSLNew.VisualMatrix = uow.FindObject<VisualMatrix>(CriteriaOperator.Parse(string.Format("VisualMatrixName = '{0}'", drVM["VisualMatrix"])));

                                        objSLNew.Qty = 1;

                                        objSLNew.QCType = null;


                                        if (objCurrent.ClientName != null)
                                        {
                                            objSLNew.Client = uow.GetObjectByKey<Customer>(objCurrent.ClientName.Oid);
                                        }
                                        if (objCurrent.DateCollected != null)
                                        {
                                            objSLNew.CollectDate = (DateTime)objCurrent.DateCollected;
                                        }
                                        if (objCurrent.Collector != null)
                                        {
                                            objSLNew.Collector = uow.GetObjectByKey<Collector>(objCurrent.Collector.Oid);
                                        }
                                        objSLNew.BottleQty = 1;
                                        objSLNew.SubOut = false;
                                        objSLNew.ModifiedBy = uow.GetObjectByKey<Employee>(((Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser).Oid);  /* as Modules.BusinessObjects.Hr.Employee;*/
                                        objSLNew.ModifiedDate = DateTime.Now;
                                        DataRow[] drrTest = dtSample.Select("VisualMatrix = '" + drVM["VisualMatrix"] + "' and Qty > 0");
                                        foreach (DataRow drTest in drrTest)
                                        {
                                            if (!(bool)drTest["IsGroup"])
                                            {
                                                List<Testparameter> lsttp = uow.Query<Testparameter>().Where(j => j.QCType.QCTypeName == "Sample").Where(k => k.TestMethod.TestName == drTest["Test"].ToString()).Where(l => l.Component.Components == drTest["Comp"].ToString()).ToList().Where(m => m.TestMethod.MethodName.MethodNumber == drTest["Method"].ToString()).Where(n => n.TestMethod.MatrixName.MatrixName == drTest["Matrix"].ToString()).ToList();
                                                foreach (var objLineA in lsttp)
                                                {
                                                    objSLNew.Testparameters.Add(uow.GetObjectByKey<Testparameter>(objLineA.Oid));
                                                }
                                                SamplingBottleAllocation smplnew = new SamplingBottleAllocation(uow);
                                                smplnew.Sampling = objSLNew;
                                                smplnew.TestMethod = uow.GetObjectByKey<TestMethod>(lsttp[0].TestMethod.Oid);
                                                smplnew.BottleID = "A";
                                                if (objSLNew.SamplingProposal.CustomDueDates.FirstOrDefault(m => m.TestMethod.Oid == smplnew.TestMethod.Oid && m.TestMethod.MatrixName.Oid == smplnew.TestMethod.MatrixName.Oid) == null)
                                                {
                                                    CustomDueDate cdnew = new CustomDueDate(uow);
                                                    cdnew.SamplingProposal = objSLNew.SamplingProposal;
                                                    cdnew.TestMethod = smplnew.TestMethod;
                                                    cdnew.DueDate = objCurrent.DueDate;
                                                    cdnew.CreatedDate = DateTime.Now;
                                                    cdnew.CreatedBy = (Modules.BusinessObjects.Hr.Employee)objSLNew.ModifiedBy;
                                                    cdnew.TAT = uow.GetObjectByKey<TurnAroundTime>(objCurrent.TAT.Oid);
                                                    cdnew.SampleMatrix = objSLNew.VisualMatrix;
                                                    cdnew.Parameter = "AllParam";
                                                }
                                            }
                                            else
                                            {
                                                TestMethod objTm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName]=? And [IsGroup]=true And [MethodName.GCRecord] Is Null", drTest["Test"].ToString()));
                                                if (objTm != null)
                                                {
                                                    IList<GroupTestMethod> lstgrouptestmed = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objTm.Oid));
                                                    foreach (GroupTestMethod objgtm in lstgrouptestmed.ToList())
                                                    {
                                                        IList<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objgtm.TestParameter.TestMethod.Oid));
                                                        if (lsttestpara != null)
                                                        {
                                                            foreach (Testparameter param1 in lsttestpara.ToList())
                                                            {
                                                                SamplingParameter objsp = ObjectSpace.FindObject<SamplingParameter>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [Sampling.Oid] = ?", param1.Oid, objSLNew.Oid));
                                                                if (objsp == null)
                                                                {
                                                                    SamplingParameter newsample = new SamplingParameter(uow);
                                                                    newsample.Sampling = objSLNew;
                                                                    newsample.Testparameter = uow.GetObjectByKey<Testparameter>(param1.Oid);
                                                                    newsample.Status = Modules.BusinessObjects.Hr.Samplestatus.PendingEntry;
                                                                    newsample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objgtm.Oid);
                                                                    newsample.IsGroup = true;
                                                                }
                                                            }
                                                        }
                                                        SamplingBottleAllocation smplnew = new SamplingBottleAllocation(uow);
                                                        smplnew.Sampling = objSLNew;
                                                        smplnew.TestMethod = uow.GetObjectByKey<TestMethod>(lsttestpara[0].TestMethod.Oid);
                                                        smplnew.BottleID = "A";
                                                        if (objSLNew.SamplingProposal.CustomDueDates.FirstOrDefault(m => m.TestMethod.Oid == objTm.Oid && m.TestMethod.MatrixName.Oid == objTm.MatrixName.Oid) == null)
                                                        {
                                                            CustomDueDate cdnew = new CustomDueDate(uow);
                                                            cdnew.SamplingProposal = objSLNew.SamplingProposal;
                                                            cdnew.TestMethod = uow.GetObjectByKey<TestMethod>(objTm.Oid);
                                                            cdnew.DueDate = objCurrent.DueDate;
                                                            cdnew.CreatedDate = DateTime.Now;
                                                            cdnew.CreatedBy = (Modules.BusinessObjects.Hr.Employee)objSLNew.ModifiedBy;
                                                            cdnew.TAT = uow.GetObjectByKey<TurnAroundTime>(objCurrent.TAT.Oid);
                                                            cdnew.SampleMatrix = objSLNew.VisualMatrix;
                                                            cdnew.Parameter = "AllParam";
                                                        }
                                                    }
                                                }
                                            }
                                            drTest["Qty"] = (int)drTest["Qty"] - 1;
                                        }
                                        objSLNew.Save();
                                        SampleNo++;
                                    }
                                }
                                uow.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage("Imported successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                                if (((SamplingProposal)View.CurrentObject).Oid == objCurrent.Oid)
                                {
                                    View.CurrentObject = View.ObjectSpace.GetObject<SamplingProposal>(objCurrent);
                                }
                                else
                                {
                                    IObjectSpace objSpace = Application.CreateObjectSpace();
                                    DetailView dv = Application.CreateDetailView(objSpace, "SamplingProposal_DetailView", true, objSpace.GetObject<SamplingProposal>(objCurrent));
                                    dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                                    Frame.SetView(dv);
                                }
                                View.ObjectSpace.Refresh();
                            }
                                
                            
                        }
                        else
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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
                if (parameter != "error")
                {
                    string[] values = parameter.Split('|');
                    if (values[0] == "Testselection")
                    {
                        if (values[1] == "Selected")
                        {
                            Guid curguid = new Guid(values[2]);
                            if (!objSMInfo.lstTestParameter.Contains(curguid))
                            {
                                objSMInfo.lstTestParameter.Add(curguid);
                                if (objSMInfo.lstRemoveTestParameter.Contains(curguid))
                                {
                                    objSMInfo.lstRemoveTestParameter.Remove(curguid);
                                }
                            }
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            CompositeView view = nestedFrame.ViewItem.View;
                            Testparameter testparameter = ObjectSpace.GetObjectByKey<Testparameter>(curguid);
                            DashboardViewItem TestViewMain = ((DashboardView)view).FindItem("TestViewMain") as DashboardViewItem;
                            DashboardViewItem TestViewSub = ((DashboardView)view).FindItem("TestViewSub") as DashboardViewItem;
                            DashboardViewItem TestViewSubChild = ((DashboardView)view).FindItem("TestViewSubChild") as DashboardViewItem;
                            bool Oidchange = true;
                            Guid curusedguid = new Guid();
                            Guid tatoid = new Guid();
                            foreach (Testparameter objtestparameter in ((ListView)TestViewSub.InnerView).CollectionSource.List)
                            {
                                if (objtestparameter.Oid == testparameter.Oid)
                                {
                                    Oidchange = false;
                                }
                                if (Oidchange && objtestparameter.TestMethod.TestName == testparameter.TestMethod.TestName && objtestparameter.TestMethod.MethodName.MethodNumber == testparameter.TestMethod.MethodName.MethodNumber)
                                {
                                    curusedguid = objtestparameter.Oid;
                                    tatoid = objtestparameter.TAT.Oid;
                                }
                            }
                            if (Oidchange && TestViewSubChild != null && TestViewSubChild.InnerView.SelectedObjects.Count == 1)
                            {
                                Testparameter addnewtestparameter = (Testparameter)TestViewSubChild.InnerView.SelectedObjects[0];
                                TurnAroundTime tat = ((ListView)TestViewSub.InnerView).ObjectSpace.GetObjectByKey<TurnAroundTime>(tatoid);
                                foreach (Testparameter lobjtestparameter in ((ListView)TestViewSub.InnerView).CollectionSource.List)
                                {
                                    if (lobjtestparameter != null)
                                    {
                                        if (testparameter.Oid == lobjtestparameter.Oid)
                                            lobjtestparameter.TAT = tat;
                                    }
                                }
                                ASPxGridListEditor gridListEditor = ((ListView)TestViewSub.InnerView).Editor as ASPxGridListEditor;
                                if (gridListEditor != null && gridListEditor.Grid != null)
                                {
                                    objSMInfo.UseSelchanged = false;
                                    gridListEditor.Grid.Selection.SelectRowByKey(addnewtestparameter.Oid);
                                }
                            }
                        }
                        else if (values[1] == "UNSelected")
                        {
                            Guid curguid = new Guid(values[2]);
                            if (objSMInfo.lstTestParameter.Contains(curguid))
                            {
                                objSMInfo.lstTestParameter.Remove(curguid);
                                if (!objSMInfo.lstRemoveTestParameter.Contains(curguid))
                                {
                                    objSMInfo.lstRemoveTestParameter.Add(curguid);
                                }
                            }
                        }
                        else if (values[1] == "Selectall")
                        {
                            ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (editor != null && editor.Grid != null)
                            {
                                for (int i = 0; i < editor.Grid.VisibleRowCount; i++)
                                {
                                    Guid curguid = new Guid(editor.Grid.GetRowValues(i, "Oid").ToString());
                                    if (!objSMInfo.lstTestParameter.Contains(curguid))
                                    {
                                        objSMInfo.lstTestParameter.Add(curguid);
                                    }
                                    if (objSMInfo.lstRemoveTestParameter.Contains(curguid))
                                    {
                                        objSMInfo.lstRemoveTestParameter.Remove(curguid);
                                    }
                                }
                            }
                        }
                        else if (values[1] == "UNSelectall")
                        {
                            ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (editor != null && editor.Grid != null)
                            {
                                for (int i = 0; i < editor.Grid.VisibleRowCount; i++)
                                {
                                    Guid curguid = new Guid(editor.Grid.GetRowValues(i, "Oid").ToString());
                                    if (objSMInfo.lstTestParameter.Contains(curguid))
                                    {
                                        objSMInfo.lstTestParameter.Remove(curguid);
                                    }
                                    if (!objSMInfo.lstRemoveTestParameter.Contains(curguid))
                                    {
                                        objSMInfo.lstRemoveTestParameter.Add(curguid);
                                    }
                                }
                            }
                        }
                    }
                    else if (values[0] == "Parameter")
                    {
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(values[1]), "Oid");
                            if (HttpContext.Current.Session["rowid"] != null)
                            {
                                objSMInfo.lstSelParameter = new List<string>();
                                CustomDueDate objsampling = ((ListView)View).CollectionSource.List.Cast<CustomDueDate>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                                if (objsampling != null)
                                {
                                    List<Sampling> lstSamples = View.ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("SamplingProposal.Oid=?", objsampling.SamplingProposal.Oid)).ToList();
                                    if (lstSamples.Count == 0)
                                    {
                                        CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Testparameter));
                                        cs.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.Oid]=? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objsampling.TestMethod.Oid);
                                        objSMInfo.Totparam = cs.GetCount();
                                        if (objsampling.Parameter == "AllParam")
                                        {
                                            foreach (Testparameter strbotid in cs.List)
                                            {
                                                objSMInfo.lstSelParameter.Add(strbotid.Oid.ToString());
                                            }
                                        }
                                        else if (objsampling.ParameterDetails != null)
                                        {
                                            string[] strbottleid = objsampling.ParameterDetails.Split(',');
                                            foreach (var strbotid in strbottleid)
                                            {
                                                objSMInfo.lstSelParameter.Add(strbotid.Trim());
                                            }
                                        }
                                        ListView lv = Application.CreateListView("Testparameter_ListView_Parameter_Sampling", cs, false);
                                        ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                                        showViewParameters.Context = TemplateContext.PopupWindow;
                                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                        showViewParameters.CreatedView.Caption = "Parameter";
                                        DialogController dc = Application.CreateController<DialogController>();
                                        dc.SaveOnAccept = false;
                                        dc.CloseOnCurrentObjectProcessing = false;
                                        dc.Accepting += Dc_Accepting2;
                                        showViewParameters.Controllers.Add(dc);
                                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                    }
                                }
                            }
                        }
                    }
                    else if (values[0] == "DueDate")
                    {
                        ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (editor != null && editor.Grid != null)
                        {
                            object currentOid = editor.Grid.GetRowValues(int.Parse(values[1]), "Oid");
                            CustomDueDate objDueDate = View.ObjectSpace.FindObject<CustomDueDate>(CriteriaOperator.Parse("Oid=?", new Guid(currentOid.ToString())));
                            
                            SamplingProposal objTask = null;
                            if (Application.MainWindow.View is DetailView)
                            {
                                objTask = (SamplingProposal)Application.MainWindow.View.CurrentObject;
                            }
                            if (objDueDate != null)
                            {
                                if (objDueDate.DueDate != DateTime.MinValue && objDueDate.DueDate < DateTime.Today)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "validduedate"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    objDueDate.DueDate = null;
                                    return;
                                }
                                else if (objDueDate.DueDate != DateTime.MinValue && objTask != null && objDueDate.DueDate > objTask.DueDate)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ValidateDuedateless"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    objDueDate.DueDate = null;
                                    return;
                                }
                                else if (objDueDate.DueDate >= DateTime.Today)
                                {
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    IList<Holidays> objHoliday = os.GetObjects<Holidays>(CriteriaOperator.Parse("Oid is Not Null"));
                                    var TAT = ((DateTime)objDueDate.DueDate).Subtract(DateTime.Today).Days;
                                    var dic = new Dictionary<DateTime, DayOfWeek>();
                                    for (int i = 0; i < TAT + 1; i++)
                                    {
                                        if (!objHoliday.Any(x => x.HolidayDate == DateTime.Today.AddDays(i)))
                                            dic.Add(DateTime.Today.AddDays(i), DateTime.Now.AddDays(i).DayOfWeek);
                                    }
                                    int CountExceptHolidays = dic.Where(x => x.Value != DayOfWeek.Saturday && x.Value != DayOfWeek.Sunday).Count();
                                    if (CountExceptHolidays > 1)
                                    {
                                        int TATdays = Convert.ToInt32(CountExceptHolidays - 1);
                                        var days = 0;
                                        var years = 0;
                                        var weeks = 0;
                                        var months = 0;
                                        string temptat = string.Empty;
                                        string stryears = string.Empty;
                                        string strmonths = string.Empty;
                                        string strweeks = string.Empty;
                                        string strdays = string.Empty;
                                        years = (TATdays / 365);
                                        months = (TATdays % 365) / 30;
                                        weeks = (TATdays % 365) / 7;
                                        days = TATdays - ((years * 365) + (weeks * 7));
                                        //years
                                        if (years == 1)
                                        {
                                            stryears = years + " " + "Year";
                                        }
                                        else if (years > 1)
                                        {
                                            stryears = years + " " + "Years";
                                        }
                                        //months
                                        if (months == 1)
                                        {
                                            strmonths = months + " " + "Month";
                                        }
                                        else if (months > 1)
                                        {
                                            strmonths = months + " " + "Months";
                                        }
                                        //week
                                        if (weeks == 1)
                                        {
                                            strweeks = weeks + " " + "Week";
                                        }
                                        else if (weeks > 1)
                                        {
                                            strweeks = weeks + " " + "Weeks";
                                        }
                                        //Days
                                        if (TATdays == 1)
                                        {
                                            strdays = TATdays + " " + "Day";
                                        }
                                        else if (TATdays > 1)
                                        {
                                            strdays = TATdays + " " + "Days";
                                        }

                                        if (years > 0 && months <= 12 && weeks <= 4 && days == 0)
                                        {
                                            temptat = stryears;
                                        }
                                        else
                                        if (months > 0 && weeks <= 4 && years == 0 && days <= 3)
                                        {
                                            temptat = strmonths;
                                        }
                                        else
                                        if (weeks > 0 && months == 0 && years == 0 && days == 0)
                                        {
                                            temptat = strweeks;
                                        }
                                        else
                                        {
                                            temptat = strdays;
                                        }

                                        TurnAroundTime objTAT = os.FindObject<TurnAroundTime>(CriteriaOperator.Parse("TAT=?", temptat));
                                        if (objTAT == null)
                                        {
                                            objTAT = os.CreateObject<TurnAroundTime>();
                                            objTAT.Count = TATdays * 24;
                                            objTAT.TAT = temptat;

                                            os.CommitChanges();
                                        }
                                        objDueDate.TAT = View.ObjectSpace.GetObjectByKey<TurnAroundTime>(objTAT.Oid);
                                    }
                                    else
                                    {
                                        string sameDay;
                                        if (objLanguage.strcurlanguage != "En")
                                        {
                                            sameDay = "同一天";
                                        }
                                        else
                                        {
                                            sameDay = "Same Day";
                                        }

                                        TurnAroundTime objTAT = os.FindObject<TurnAroundTime>(CriteriaOperator.Parse("TAT=?", sameDay));
                                        if (objTAT == null)
                                        {
                                            objTAT = os.CreateObject<TurnAroundTime>();
                                            objTAT.TAT = sameDay;
                                            os.CommitChanges();
                                        }
                                        objDueDate.TAT = View.ObjectSpace.GetObjectByKey<TurnAroundTime>(objTAT.Oid);
                                    }
                                    ((ListView)View).Refresh();
                                }
                            }
                        }
                    }
                    else if (values[0] == "SuboutSelected" || values[0] == "SuboutUnSelected")
                    {
                        ListView listView = null;
                        if (View is DashboardView)
                        {
                            DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                            if (TestViewSub != null && TestViewSub.InnerView != null)
                            {
                                listView = TestViewSub.InnerView as ListView;
                            }
                        }
                        else if (View is ListView)
                        {
                            if (View.Id == "Testparameter_LookupListView_Copy_SampleLogin_Copy")
                            {
                                listView = View as ListView;
                            }
                            else
                            {
                                if (Frame != null && Frame is NestedFrame)
                                {
                                    NestedFrame nestedFrame = (NestedFrame)Frame;
                                    if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                                    {
                                        CompositeView cv = nestedFrame.ViewItem.View;
                                        if (cv != null && cv is DashboardView)
                                        {
                                            DashboardViewItem TestViewSub = ((DashboardView)cv).FindItem("TestViewSub") as DashboardViewItem;
                                            if (TestViewSub != null && TestViewSub.InnerView != null)
                                            {
                                                listView = TestViewSub.InnerView as ListView;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (listView != null && listView.CollectionSource.GetCount() > 0)
                        {
                            ASPxGridListEditor editor = (ASPxGridListEditor)listView.Editor;
                            if (editor != null)
                            {
                                object curoid = editor.Grid.GetRowValues(int.Parse(values[1]), "Oid");
                                Testparameter param = ObjectSpace.GetObjectByKey<Testparameter>(curoid);
                                if (param != null && param.TestMethod != null)
                                {
                                    if (objSMInfo.lstSubOutTest == null)
                                    {
                                        objSMInfo.lstSubOutTest = new List<Guid>();
                                    }
                                    if (values[0] == "SuboutSelected")
                                    {
                                        if (!objSMInfo.lstSubOutTest.Contains(param.TestMethod.Oid))
                                        {
                                            objSMInfo.lstSubOutTest.Add(param.TestMethod.Oid);
                                        }
                                    }
                                    else
                                    {
                                        if (objSMInfo.lstSubOutTest.Contains(param.TestMethod.Oid))
                                        {
                                            objSMInfo.lstSubOutTest.Remove(param.TestMethod.Oid);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (values[0] == "TAT")
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null)
                        {
                            object Guid = gridListEditor.Grid.GetRowValues(int.Parse(values[1]), "Oid");
                            if (Guid != null)
                            {
                                CustomDueDate objDate = View.ObjectSpace.FindObject<CustomDueDate>(CriteriaOperator.Parse("Oid=?", new Guid(Guid.ToString())));
                                SamplingProposal objsamplecheckin = (SamplingProposal)Application.MainWindow.View.CurrentObject;
                                if (objDate != null && objsamplecheckin != null && objsamplecheckin.TAT != null)
                                {
                                    if (objDate.TAT.Count <= objsamplecheckin.TAT.Count)
                                    {
                                        int tatHour = objDate.TAT.Count;
                                        int Day = 0;
                                        if (tatHour >= 24)
                                        {
                                            Day = tatHour / 24;
                                            objDate.DueDate = AddWorkingDays(DateTime.Now, Day);
                                        }
                                        else
                                        {
                                            objDate.DueDate = AddWorkingHours(DateTime.Now, tatHour);
                                        }
                                         ((ListView)View).Refresh();
                                    }
                                    else if (objDate.TAT.Count != objsamplecheckin.TAT.Count)
                                    {
                                        TurnAroundTime objTAT = View.ObjectSpace.GetObjectByKey<TurnAroundTime>(objsamplecheckin.TAT.Oid);
                                        objDate.TAT = objTAT;
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CRDueDate"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        ((ListView)View).Refresh();
                                        return;
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    objSMInfo.strSampleID = "error";
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Dc_Accepting2(object sender, DialogControllerAcceptingEventArgs e)
        {
            CustomDueDate objsampling = ((ListView)View).CollectionSource.List.Cast<CustomDueDate>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
            if (objsampling != null)
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    objsampling.ParameterDetails = string.Join(",", e.AcceptActionArgs.SelectedObjects.Cast<Testparameter>().OrderBy(a => a.Sort).Select(a => a.Oid));
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectparameter"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    return;
                }
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    if (e.AcceptActionArgs.SelectedObjects.Count == objSMInfo.Totparam)
                    {
                        objsampling.Parameter = "AllParam";
                    }
                    else
                    {
                        objsampling.Parameter = "Customised";
                    }
                }
                else
                {
                    objsampling.Parameter = null;
                }
            }
        }
        private void Containers_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
           try
            {
                ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                SamplingProposal objsmplcheckin = (SamplingProposal)Application.MainWindow.View.CurrentObject;
                if (objsmplcheckin != null)
                {
                    objSMInfo.strJobID = objsmplcheckin.RegistrationID;
                    SamplingBottleAllocation newsmplbtlalloc = View.ObjectSpace.CreateObject<SamplingBottleAllocation>();
                    newsmplbtlalloc.DefaultContainerQty = 1;
                    DetailView dvbottleAllocation = Application.CreateDetailView(View.ObjectSpace, "SamplingBottleAllocation_DetailView_Sampling", false, newsmplbtlalloc);
                    ShowViewParameters showViewParameters = new ShowViewParameters(dvbottleAllocation);
                    showViewParameters.CreatedView = dvbottleAllocation;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.AcceptAction.Active["OkayBtn"] = false;
                    dc.CancelAction.Active["CancelBtn"] = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void TestSelectionAdd_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
                DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
                if (TestViewMain != null && ((ListView)TestViewMain.InnerView).SelectedObjects.Count > 0)
                {
                    List<Guid> lstSubOutTestOid = new List<Guid>();
                    if (objSMInfo.lstSubOutTest == null)
                    {
                        objSMInfo.lstSubOutTest = new List<Guid>();
                    }
                    if (objSMInfo.lstdupfilterSuboutstr == null)
                    {
                        objSMInfo.lstdupfilterSuboutstr = new List<string>();
                    }
                    SamplingProposal objJobID = ObjectSpace.FindObject<SamplingProposal>(CriteriaOperator.Parse("[RegistrationID] = ? AND [GCRecord] is NULL", objSMInfo.strJobID));
                                     
                    Sampling objSL = ObjectSpace.FindObject<Sampling>(CriteriaOperator.Parse("[Oid] = ? AND [GCRecord] is NULL", objSMInfo.SampleOid));
                    foreach (Testparameter testparameter in ((ListView)TestViewMain.InnerView).SelectedObjects)
                    {
                        if (testparameter.IsGroup == false)
                        {
                            IList<Testparameter> listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodNumber]=? and [TestMethod.MatrixName.MatrixName] = ? and Component.Components=? And TestMethod.GCRecord Is Null And TestMethod.MethodName.GCRecord Is Null And TestMethod.MatrixName.GCRecord Is Null And [QCType.QCTypeName] = 'Sample'", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components));
                           
                            foreach (Testparameter test in listseltest)
                            {
                                if (lstSubOutTestOid != null && lstSubOutTestOid.Contains(test.TestMethod.Oid))
                                {
                                    if (!objSMInfo.lstSubOutTest.Contains(test.TestMethod.Oid))
                                    {
                                        objSMInfo.lstSubOutTest.Add(test.TestMethod.Oid);
                                        if (!objSMInfo.lstdupfilterSuboutstr.Contains(test.TestMethod.TestName + "|" + test.TestMethod.MethodName + "|" + test.TestMethod.MatrixName.MatrixName + "|" + test.Component))
                                        {
                                            objSMInfo.lstdupfilterSuboutstr.Add(test.TestMethod.TestName + "|" + test.TestMethod.MethodName + "|" + test.TestMethod.MatrixName.MatrixName + "|" + test.Component);
                                        }
                                    }
                                }
                                if (!objSMInfo.lstTestParameter.Contains(test.Oid))
                                {
                                    objSMInfo.lstTestParameter.Add(test.Oid);
                                    if(objSMInfo.lstRemoveTestParameter.Contains(test.Oid))
                                    {
                                        objSMInfo.lstRemoveTestParameter.Remove(test.Oid);
                                    }
                                }
                                if (objJobID != null && objSL.SubOut == true)
                                {
                                    if (!objSMInfo.lstdupfilterSuboutstr.Contains(test.TestMethod.TestName + "|" + test.TestMethod.MethodName.MethodNumber + "|" + test.TestMethod.MatrixName.MatrixName + "|" + test.Component.Components))
                                    {
                                        objSMInfo.lstdupfilterSuboutstr.Add(test.TestMethod.TestName + "|" + test.TestMethod.MethodName.MethodNumber + "|" + test.TestMethod.MatrixName.MatrixName + "|" + test.Component.Components);
                                    }
                                    objSMInfo.lstSubOutTest.Add(test.TestMethod.Oid);
                                }
                            }
                        }
                        else
                        {
                            IList<Testparameter> listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MatrixName.MatrixName] = ? ", testparameter.TestMethod.TestName, testparameter.TestMethod.MatrixName.MatrixName));
                            foreach (Testparameter test in listseltest)
                            {
                                if (lstSubOutTestOid != null && lstSubOutTestOid.Contains(test.TestMethod.Oid))
                                {
                                    if (!objSMInfo.lstSubOutTest.Contains(test.TestMethod.Oid))
                                    {
                                        objSMInfo.lstSubOutTest.Add(test.TestMethod.Oid);
                                        if (!objSMInfo.lstdupfilterSuboutstr.Contains(test.TestMethod.TestName + "|" + test.TestMethod.MethodName + "|" + test.TestMethod.MatrixName.MatrixName + "|" + test.Component))
                                        {
                                            objSMInfo.lstdupfilterSuboutstr.Add(test.TestMethod.TestName + "|" + test.TestMethod.MethodName + "|" + test.TestMethod.MatrixName.MatrixName + "|" + test.Component);
                                        }
                                    }
                                }
                                if (!objSMInfo.lstTestParameter.Contains(test.Oid))
                                {
                                    objSMInfo.lstTestParameter.Add(test.Oid);
                                }
                            }
                        }
                    }
                    if (TestViewSub != null && objSMInfo.lstTestParameter != null && objSMInfo.lstTestParameter.Count > 0)
                    {
                        ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", objSMInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", objSMInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                    if (objSL != null && objSL.SubOut == true)
                    {
                        ((ListView)TestViewSub.InnerView).CollectionSource.List.Cast<Testparameter>().ToList().ForEach(i => i.SubOut = true);
                    }
                    TurnAroundTime tat = ((ListView)TestViewSub.InnerView).ObjectSpace.GetObjectByKey<TurnAroundTime>(objJobID.TAT.Oid);
                    if (TestViewSub != null)
                    {
                        if (TestViewSub.InnerView is ListView innerListView)
                        {
                            foreach (Testparameter testparameter in innerListView.CollectionSource.List.Cast<Testparameter>().ToList().Where(i => i.TAT == null))
                            {
                                if (testparameter.TAT == null)
                                    testparameter.TAT = tat;
                            }
                        }
                    }
                    ((ASPxGridListEditor)((ListView)TestViewSub.InnerView).Editor).Grid.JSProperties["cpCanGridRefresh"] = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void TestSelectionRemove_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                bool IsRemoved = true;
                DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
                DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
                if (TestViewMain != null && TestViewSub != null && ((ListView)TestViewSub.InnerView).SelectedObjects.Count > 0)
                {
                    if (objSMInfo.lstRemoveTestParameter == null)
                    {
                        objSMInfo.lstRemoveTestParameter = new List<Guid>();
                    }
                    foreach (Testparameter testparameter in ((ListView)TestViewSub.InnerView).SelectedObjects)
                    {
                        IList<Testparameter> listseltest = new List<Testparameter>();
                        Sampling sampleLog = TestViewSub.InnerView.ObjectSpace.GetObjectByKey<Sampling>(objSMInfo.SampleOid);
                        if (sampleLog != null)
                        {
                            IList<SamplingParameter> lstSample = null;
                            if (testparameter.IsGroup != true)
                            {
                                lstSample = TestViewSub.InnerView.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[Sampling.Oid]=? And [Testparameter.TestMethod.TestName]=? and [Testparameter.TestMethod.MethodName.MethodNumber]=? and [Testparameter.TestMethod.MatrixName.MatrixName] = ? and [Testparameter.Component.Components]=?", sampleLog.Oid, testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components));
                            }
                            else
                            {
                                lstSample = TestViewSub.InnerView.ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[Sampling.Oid]=? And [Testparameter.TestMethod.TestName]=?  and [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [IsGroup]=true And [GroupTest.TestMethod.Oid]=? ", sampleLog.Oid, testparameter.TestMethod.TestName, testparameter.TestMethod.MatrixName.MatrixName, testparameter.TestMethod.Oid));
                            }
                            if ((lstSample != null && lstSample.Count > 0 && lstSample.FirstOrDefault(i => i.SignOff == true) == null) || lstSample.Count == 0)
                            {
                                if (testparameter.IsGroup != true)
                                {
                                    listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodNumber]=? and [TestMethod.MatrixName.MatrixName] = ? and [Component.Components]=?", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components));
                                }
                                else
                                {
                                    if (objSMInfo.lstTestParameter.Contains(testparameter.Oid))
                                    {
                                        objSMInfo.lstTestParameter.Remove(testparameter.Oid);
                                    }
                                    IList<GroupTestMethod> lstgrouptestmed = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", testparameter.TestMethod.Oid));
                                    foreach (GroupTestMethod objgtm in lstgrouptestmed.ToList())
                                    {
                                        IList<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objgtm.TestParameter.TestMethod.Oid));
                                        foreach (Testparameter paramgtm in lsttestpara.ToList())
                                        {
                                            listseltest.Add(paramgtm);
                                        }
                                    }
                                }
                                foreach (Testparameter test in listseltest)
                                {
                                    if (objSMInfo.lstTestParameter.Contains(test.Oid))
                                    {
                                        objSMInfo.lstTestParameter.Remove(test.Oid);
                                    }
                                    if (!objSMInfo.lstRemoveTestParameter.Contains(test.Oid))
                                    {
                                        objSMInfo.lstRemoveTestParameter.Add(test.Oid);
                                    }
                                }
                            }
                            else
                            {
                                DefaultSetting objNavigationView = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='RegistrationSigningOff'"));
                                DefaultSetting objSamplePreparation = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparationRootNode'"));
                                if (objNavigationView != null && objNavigationView.Select)
                                {
                                    IsRemoved = false;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "testcannotremove"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                                if (objSamplePreparation != null && objSamplePreparation.Select)
                                {
                                    if (lstSample.FirstOrDefault(i => !string.IsNullOrEmpty(i.PrepBatchID)) != null)
                                    {
                                        IsRemoved = false;
                                        Application.ShowViewStrategy.ShowMessage("Already used in samplepreparation cannot be removed.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }
                                }
                                if (lstSample.FirstOrDefault(i => !string.IsNullOrEmpty(i.ResultNumeric)) != null)
                                {
                                    IsRemoved = false;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletetest"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                                if (lstSample.FirstOrDefault(i => i.UQABID != null) != null)
                                {
                                    IsRemoved = false;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletesample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                                if (IsRemoved)
                                {
                                    if (testparameter.IsGroup != true)
                                    {
                                        listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodNumber]=? and [TestMethod.MatrixName.MatrixName] = ? and [Component.Components]=?", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components));
                                    }
                                    else
                                    {
                                        if (objSMInfo.lstTestParameter.Contains(testparameter.Oid))
                                        {
                                            objSMInfo.lstTestParameter.Remove(testparameter.Oid);
                                        }
                                        IList<GroupTestMethod> lstgrouptestmed = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", testparameter.TestMethod.Oid));
                                        foreach (GroupTestMethod objgtm in lstgrouptestmed.ToList())
                                        {
                                            IList<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objgtm.TestParameter.TestMethod.Oid));
                                            foreach (Testparameter paramgtm in lsttestpara.ToList())
                                            {
                                                listseltest.Add(paramgtm);
                                            }
                                        }
                                    }
                                    foreach (Testparameter test in listseltest)
                                    {
                                        if (objSMInfo.lstTestParameter.Contains(test.Oid))
                                        {
                                            objSMInfo.lstTestParameter.Remove(test.Oid);
                                        }
                                        if (!objSMInfo.lstRemoveTestParameter.Contains(test.Oid))
                                        {
                                            objSMInfo.lstRemoveTestParameter.Add(test.Oid);
                                        }
                                    }
                                }
                            }

                        }



                    }
                    if (objSMInfo.lstTestParameter.Count != 0 && TestViewSubChild != null)
                    {
                        ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", objSMInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", objSMInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                    else
                    {
                        ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("");
                        ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

       

        private void TestSelectionSave_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
                DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                if (objSMInfo.lstRemoveTestParameter != null && objSMInfo.lstRemoveTestParameter.Count > 0)
                {
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    Sampling objSamplelogin = uow.FindObject<Sampling>(CriteriaOperator.Parse("[Oid] = ?", objSMInfo.SampleOid));
                    foreach (Guid objTestParameter in objSMInfo.lstRemoveTestParameter)
                    {
                        Testparameter param = uow.GetObjectByKey<Testparameter>(objTestParameter);
                        if (objSMInfo.lstRemoveTestParameter.Contains(objTestParameter) && param != null)
                        {
                            objSamplelogin.Testparameters.Remove(param);
                            IObjectSpace os = Application.CreateObjectSpace(typeof(SamplingParameter));
                            Session currentSessions = ((XPObjectSpace)(os)).Session;
                            UnitOfWork uows = new UnitOfWork(currentSession.DataLayer);
                            SamplingParameter objsmpltest = uows.FindObject<SamplingParameter>(CriteriaOperator.Parse("[Testparameter.Oid] = ? And [Sampling.Oid]= ?", objTestParameter, objSamplelogin.Oid));
                            if (objsmpltest != null)
                            {
                                if (objSamplelogin.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                                {
                                    Frame.GetController<AuditlogViewController>().insertauditdata(uows, objSamplelogin.SamplingProposal.Oid, OperationType.Deleted, "Sampling Proposal", objSamplelogin.SampleID, "Test", param.TestMethod.TestName + " | " + param.Parameter.ParameterName, "", "");
                                }
                                uows.Delete(objsmpltest);
                                uows.CommitChanges();
                            }
                        }
                    }
                    objSMInfo.lstRemoveTestParameter.Clear();
                    objSamplelogin.Save();
                }
                if (objSMInfo.lstTestParameter != null && objSMInfo.lstTestParameter.Count > 0)
                {
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    Sampling sampleLog = uow.GetObjectByKey<Sampling>(objSMInfo.SampleOid);

                    foreach (Guid objtestparameter in objSMInfo.lstTestParameter)
                    {
                        Testparameter param = uow.GetObjectByKey<Testparameter>(objtestparameter);
                        if (param.IsGroup == false)
                        {
                            if (!objSMInfo.lstSavedTestParameter.Contains(objtestparameter) && param != null && param.QCType != null && param.QCType.QCTypeName == "Sample")
                            {
                                SamplingParameter objsp = ObjectSpace.FindObject<SamplingParameter>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [Sampling.Oid] = ?", objtestparameter, sampleLog.Oid));
                                if (objsp == null)
                                {
                                    SamplingParameter newsample = new SamplingParameter(uow);
                                    newsample.Sampling = sampleLog;
                                    newsample.Testparameter = param;
                                    newsample.Status = Modules.BusinessObjects.Hr.Samplestatus.PendingEntry;
                                    sampleLog.Test = true;
                                    Testparameter objTestparam = ((ListView)TestViewSub.InnerView).CollectionSource.List.Cast<Testparameter>().Where(i => i.TestMethod != null && i.TestMethod.MatrixName != null && i.TestMethod.TestName == param.TestMethod.TestName && i.TestMethod.MatrixName.MatrixName == param.TestMethod.MatrixName.MatrixName && i.TestMethod.MethodName.MethodNumber == param.TestMethod.MethodName.MethodNumber && i.Component.Components == param.Component.Components).FirstOrDefault();
                                    if (objTestparam != null && objTestparam.TAT != null)
                                    {
                                        newsample.TAT = uow.GetObjectByKey<TurnAroundTime>(objTestparam.TAT.Oid);
                                    }
                                    if (objSMInfo.lstSubOutTest != null && objSMInfo.lstSubOutTest.Contains(param.TestMethod.Oid))
                                    {
                                        newsample.SubOut = true;
                                    }
                                    else
                                    {
                                        newsample.SubOut = false;
                                    }
                                    newsample.Save();
                                    if (sampleLog.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                                    {
                                        Frame.GetController<AuditlogViewController>().insertauditdata(uow, sampleLog.SamplingProposal.Oid, OperationType.Created, "Sampling Proposal", sampleLog.SampleID, "Test", "", param.TestMethod.TestName + " | " + param.Parameter.ParameterName, "");
                                    }
                                }
                            }
                            else
                            {
                                SamplingParameter sample = uow.FindObject<SamplingParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid]=? And [Sampling.Oid]=? And [Testparameter.Parameter.Oid] = ?", param.TestMethod.Oid, sampleLog.Oid, param.Parameter.Oid));
                                if (sample != null)
                                {
                                    Testparameter objTestparam = ((ListView)TestViewSub.InnerView).CollectionSource.List.Cast<Testparameter>().Where(i => i.TestMethod != null && i.TestMethod.MatrixName != null && i.TestMethod.TestName == param.TestMethod.TestName && i.TestMethod.MatrixName.MatrixName == param.TestMethod.MatrixName.MatrixName && i.TestMethod.MethodName.MethodNumber == param.TestMethod.MethodName.MethodNumber && i.Component.Components == param.Component.Components).FirstOrDefault();
                                    if (objTestparam != null && objTestparam.TAT != null)
                                    {
                                        sample.TAT = uow.GetObjectByKey<TurnAroundTime>(objTestparam.TAT.Oid);
                                    }
                                    if (objSMInfo.lstSubOutTest != null && objSMInfo.lstSubOutTest.Contains(param.TestMethod.Oid))
                                    {
                                        sample.SubOut = true;
                                    }
                                    else
                                    {
                                        sample.SubOut = false;
                                    }
                                    sample.Save();
                                }
                            }
                        }
                        else if (param.IsGroup == true)
                        {
                            IList<GroupTestMethod> lstgrouptestmed = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", param.TestMethod.Oid));
                            foreach (GroupTestMethod objgtm in lstgrouptestmed.ToList())
                            {
                                IList<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objgtm.TestParameter.TestMethod.Oid));
                                foreach (Testparameter param1 in lsttestpara.ToList())
                                {
                                    SamplingParameter objsp = ObjectSpace.FindObject<SamplingParameter>(CriteriaOperator.Parse("[Testparameter.Oid] = ? and [Sampling.Oid] = ?", param1.Oid, sampleLog.Oid));
                                    if (objsp == null)
                                    {
                                        SamplingParameter newsample = new SamplingParameter(uow);
                                        newsample.Sampling = sampleLog;
                                        newsample.Testparameter = uow.GetObjectByKey<Testparameter>(param1.Oid);
                                        newsample.Status = Modules.BusinessObjects.Hr.Samplestatus.PendingEntry;
                                        sampleLog.Test = true;
                                        newsample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objgtm.Oid);
                                        newsample.IsGroup = true;
                                        if (objSMInfo.lstSubOutTest != null && objSMInfo.lstSubOutTest.Contains(param.TestMethod.Oid))
                                        {
                                            newsample.SubOut = true;
                                        }
                                        else
                                        {
                                            newsample.SubOut = false;
                                        }
                                        newsample.Save();
                                        if (sampleLog.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                                        {
                                            Frame.GetController<AuditlogViewController>().insertauditdata(uow, sampleLog.SamplingProposal.Oid, OperationType.Created, "Sampling Proposal", sampleLog.SampleID, "Test", "", param.TestMethod.TestName, "");
                                        }
                                        uow.CommitChanges();
                                    }
                                    else
                                    {
                                        SamplingParameter sample = uow.FindObject<SamplingParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid]=? And [Samplelogin.Oid]=? And [Testparameter.Parameter.Oid] = ?", param1.TestMethod.Oid, sampleLog.Oid, param1.Parameter.Oid));
                                        if (sample != null)
                                        {
                                            if (objSMInfo.lstSubOutTest != null && objSMInfo.lstSubOutTest.Contains(param1.TestMethod.Oid))
                                            {
                                                sample.SubOut = true;
                                            }
                                            else
                                            {
                                                sample.SubOut = false;
                                            }
                                            sample.Save();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    TestViewMain.InnerView.ObjectSpace.CommitChanges();
                    TestViewSub.InnerView.ObjectSpace.CommitChanges();
                    uow.CommitChanges();
                    AssignBottleAllocationToSamples(uow, objSMInfo.SampleOid);
                    uow.CommitChanges();
                }
                int testCount = View.ObjectSpace.GetObjectsCount(typeof(SamplingParameter), CriteriaOperator.Parse("[Sampling.Oid] = ?", objSMInfo.SampleOid));
                if (testCount == 0)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assigntesttosample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    return;
                }
                (Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(true);
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
                if(View.ObjectSpace.ModifiedObjects.Count>0)
                {
                    View.ObjectSpace.CommitChanges();
                }
                bool IsSubmit = true;
                if(View is ListView)
                {
                    if(View.SelectedObjects.Count>0)
                    {
                        foreach(SamplingProposal obj in View.SelectedObjects)
                        {
                            int sampleno = ObjectSpace.GetObjectsCount(typeof(Sampling), CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", obj.Oid));
                            if(sampleno>0)
                            {
                                int objtestcount = ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("[SamplingProposal.Oid]=?", obj.Oid)).ToList().Where(i => i.Testparameters == null || i.Testparameters.Count == 0).Count();
                                if(objtestcount==0)
                                {
                                    obj.Status = RegistrationStatus.Submitted;
                                }
                                else
                                {
                                    IsSubmit = false;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assigntesttosample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                            }
                            else
                            {
                                IsSubmit = false;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assignsample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            
                        }
                        if(IsSubmit)
                        {
                            View.ObjectSpace.CommitChanges();
                            View.ObjectSpace.Refresh();
                            Application.ShowViewStrategy.ShowMessage("Submitted successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    View.ObjectSpace.CommitChanges();
                    SamplingProposal objProposal = View.CurrentObject as SamplingProposal;
                    int sampleno = ObjectSpace.GetObjectsCount(typeof(Sampling), CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", objProposal.Oid));
                    if (sampleno>0)
                    {
                        int objtestcount = ObjectSpace.GetObjects<Sampling>(CriteriaOperator.Parse("[SamplingProposal.Oid]=?", objProposal.Oid)).ToList().Where(i => i.Testparameters == null || i.Testparameters.Count == 0).Count();
                        if (objtestcount == 0)
                        {
                            objProposal.Status = RegistrationStatus.Submitted;
                            View.ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage("Submitted successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(objspace, typeof(SamplingProposal));
                            ListView createListview = Application.CreateListView("SamplingProposal_ListView", cs, true);
                            Frame.SetView(createListview);
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assigntesttosample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assignsample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void CopyReccurenceSave_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "SamplingProposal_DetailView_CopyRecurrence")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    SamplingProposal sampling = View.CurrentObject as SamplingProposal;
                    if (sampling != null)
                    {
                        //var objrecrrence = sampling.Copyfrom.RecurrenceInfoXml;
                        DashboardViewItem objsample = ((DetailView)View).FindItem("CopyTo") as DashboardViewItem;
                        if(objsample!= null && objsample.InnerView!= null)
                        {
                            if (sampling.Copyfrom != null)
                            {
                                if (sampling.Copyfrom.RegistrationID != null)
                                {
                                    TaskRecurranceSetup objTaskRecurrance = objectSpace.FindObject<TaskRecurranceSetup>(CriteriaOperator.Parse("[RegistrationID.Oid] =? ", sampling.Copyfrom.RegistrationID.Oid));
                                    if (objsample.InnerView.SelectedObjects.Count > 0)
                                    {
                            foreach (SamplingProposal task in objsample.InnerView.SelectedObjects)
                            {
                                            TaskRecurranceSetup objCopyToTaskRecurrance = objsample.InnerView.ObjectSpace.FindObject<TaskRecurranceSetup>(CriteriaOperator.Parse("[RegistrationID.Oid] =? ", task.Oid));
                                            if (objCopyToTaskRecurrance != null)
                                            {
                                                //task.Copyfrom.RecurrenceInfoXml = objsample.InnerView.ObjectSpace.GetObject(sampling.Copyfrom.RecurrenceInfoXml);
                                                objCopyToTaskRecurrance.RecurrenceInfoXml = objTaskRecurrance.RecurrenceInfoXml; 
                                            }
                                            else
                                            {
                                                TaskRecurranceSetup objTask = objsample.InnerView.ObjectSpace.CreateObject<TaskRecurranceSetup>();
                                                objTask.RegistrationID = task;
                                                objTask.RecurrenceInfoXml = objTaskRecurrance.RecurrenceInfoXml;

                                            }
                                        }
                                 objsample.InnerView.ObjectSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Copy"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);

                            }
                        }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Please add the Registration ID", InformationType.Error, timer.Seconds, InformationPosition.Top);
                          
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage("Please select the copy from", InformationType.Error, timer.Seconds, InformationPosition.Top);

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

        private void CopyRecurrence_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "SamplingProposal_ListView")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    SamplingProposal taskRecurranceSetup = os.CreateObject<SamplingProposal>();
                    if (taskRecurranceSetup != null)
                    {
                        DetailView createdView = Application.CreateDetailView(os, "SamplingProposal_DetailView_CopyRecurrence", true, taskRecurranceSetup);
                        ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        showViewParameters.CreatedView.Caption = "Copy Recurrence";
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.AcceptAction.Active.SetItemValue("enb", false);
                        dc.CancelAction.Active.SetItemValue("enb", false);
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                    }



                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAs_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    foreach (SamplingProposal objOldSp in View.SelectedObjects)
                    {
                        Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                        UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);

                        #region SamplingProposalInfo
                        SamplingProposal objNewSp = new SamplingProposal(uow);

                        /*General Information*/
                        if (objOldSp.ClientName != null)
                        {
                            objNewSp.ClientName = uow.GetObjectByKey<Customer>(objOldSp.ClientName.Oid);
                        }
                        objNewSp.ClientAddress = objOldSp.ClientAddress;
                        objNewSp.ClientPhone = objOldSp.ClientPhone;
                        if (objOldSp.ClientContact != null)
                        {
                            objNewSp.ClientContact = uow.GetObjectByKey<Contact>(objOldSp.ClientContact.Oid);
                        }
                        objNewSp.PO = objOldSp.PO;
                        if (objOldSp.PaymentStatus != null)
                        {
                            objNewSp.PaymentStatus = uow.GetObjectByKey<PaymentStatus>(objOldSp.PaymentStatus.Oid);
                        }
                        objNewSp.DateExpect = objOldSp.DateExpect;
                        if (objOldSp.ProjectID != null)
                        {
                            objNewSp.ProjectID = uow.GetObjectByKey<Project>(objOldSp.ProjectID.Oid);
                        }
                        if (objOldSp.COCSource != null)
                        {
                            objNewSp.COCSource = uow.GetObjectByKey<COCSettings>(objOldSp.COCSource.Oid);
                        }

                        objNewSp.RetireDate = objOldSp.RetireDate;
                        //objNewTask.Status = objTask.Status;
                        objNewSp.Status = RegistrationStatus.PendingSubmission;

                        /*Sample Information*/
                        objNewSp.NoOfSamples = objOldSp.NoOfSamples;

                        if (objOldSp.SampleMatries != null)
                        {
                            objNewSp.SampleMatries = objOldSp.SampleMatries;
                        }

                        if (objOldSp.SampleCategory != null)
                        {
                            objNewSp.SampleCategory = objOldSp.SampleCategory;
                        }

                        objNewSp.TestName = objOldSp.TestName;
                        objNewSp.NPTest = objOldSp.NPTest;
                        /*Report Requirement*/
                       // objNewSp.DueDate = objOldSp.DueDate;
                        if (objOldSp.TAT != null)
                        {
                            objNewSp.TAT = uow.GetObjectByKey<TurnAroundTime>(objOldSp.TAT.Oid);
                            int tatHour = objOldSp.TAT.Count;
                            int Day = 0;
                            if (tatHour >= 24)
                            {
                                Day = tatHour / 24;
                                objNewSp.DueDate = AddWorkingDays(DateTime.Now, Day);
                            }
                            else
                            {
                                objNewSp.DueDate = AddWorkingHours(DateTime.Now, tatHour);
                            }
                        }
                        objNewSp.ReportTemplate = objOldSp.ReportTemplate;
                        CreateNewRegistrationID(uow, objNewSp);
                        objNewSp.Save();
                        uow.CommitChanges();
                        #endregion

                        #region AttahmentInfo
                        /*Attachment */
                        XPClassInfo attachmentInfo;
                        attachmentInfo = uow.GetClassInfo(typeof(Attachment));
                        IList<Attachment> objAttachment = uow.GetObjects(attachmentInfo, CriteriaOperator.Parse("[SamplingProposal]=?", objOldSp.Oid), null, int.MaxValue, false, true).Cast<Attachment>().ToList();
                        if (objAttachment != null && objAttachment.Count > 0)
                        {
                            foreach (Attachment objAttch in objAttachment)
                            {
                                Attachment objNewAttachment = new Attachment(uow);
                                objNewAttachment.Name = objAttch.Name;
                                objNewAttachment.Attachments = objAttch.Attachments;
                                objNewAttachment.Date = objAttch.Date;
                                objNewAttachment.Operator = objAttch.Operator;
                                objNewAttachment.CreatedDate = DateTime.Now;
                                objNewAttachment.CreatedBy = objAttch.CreatedBy;
                                if (objNewSp != null)
                                {
                                    objNewAttachment.SamplingProposal = uow.GetObjectByKey<SamplingProposal>(objNewSp.Oid);
                                }
                                objNewAttachment.Save();
                            }
                        }
                        #endregion
                        #region NotesInfo
                        /*Notes*/
                        XPClassInfo NotesInfo;
                        NotesInfo = uow.GetClassInfo(typeof(Notes));
                        IList<Notes> objnotes = uow.GetObjects(NotesInfo, CriteriaOperator.Parse("[SamplingProposal]=?", objOldSp.Oid), null, int.MaxValue, false, true).Cast<Notes>().ToList();
                        if (objnotes != null && objnotes.Count > 0)
                        {
                            foreach (Notes objN in objnotes)
                            {
                                Notes objNewNotes = new Notes(uow);
                                if (objN.Author != null)
                                {
                                    objNewNotes.Author = uow.GetObjectByKey<Employee>(objN.Author.Oid);
                                }
                                objNewNotes.Date = DateTime.Now;
                                objNewNotes.SamplingProposal = objNewSp;
                                objNewNotes.Title = objN.Title;

                                if (objN.Attachment != null)
                                {
                                    objNewNotes.Attachment = objN.Attachment;
                                }
                                objNewNotes.Text = objN.Text;
                                objNewNotes.Save();
                            }
                        }
                        #endregion
                        #region SamplesInfo
                        //*****SamplesInfo******//
                        XPClassInfo samplingInfo;
                        samplingInfo = uow.GetClassInfo(typeof(Sampling));

                        IList<Sampling> lstSampling = uow.GetObjects(samplingInfo, CriteriaOperator.Parse("[SamplingProposal]=?", objOldSp.Oid), null, int.MaxValue, false, true).Cast<Sampling>().ToList();

                        SamplingProposal objJobId = uow.GetObjectByKey<SamplingProposal>(objNewSp.Oid);
                        foreach (Sampling objOldSampling in lstSampling.ToList().OrderBy(i => i.SampleNo))
                        {
                            Sampling objNewSampling = new Sampling(uow);
                            if (objOldSampling.VisualMatrix != null)
                            {
                                objNewSampling.VisualMatrix = uow.GetObjectByKey<VisualMatrix>(objOldSampling.VisualMatrix.Oid);
                            }
                            if (objOldSampling.Collector != null)
                            {
                                objNewSampling.Collector = uow.GetObjectByKey<Collector>(objOldSampling.Collector.Oid);
                            }
                            objNewSampling.SamplingProposal = objJobId;
                            objNewSampling.ExcludeInvoice = false;
                            objNewSampling.SampleNo = objOldSampling.SampleNo;
                            objNewSampling.Test = true;
                            objNewSampling.AlternativeStation = objOldSampling.AlternativeStation;
                            objNewSampling.AlternativeStationOid = objOldSampling.AlternativeStationOid;
                            if (objOldSampling.SampleType != null)
                            {
                                objNewSampling.SampleType = uow.GetObjectByKey<SampleType>(objOldSampling.SampleType.Oid);
                            }
                            objNewSampling.Qty = objOldSampling.Qty;
                            if (objOldSampling.Storage != null)
                            {
                                objNewSampling.Storage = uow.GetObjectByKey<Storage>(objOldSampling.Storage.Oid);
                            }
                            if (objOldSampling.StationLocation != null)
                            {
                                objNewSampling.StationLocation = uow.GetObjectByKey<SampleSites>(objOldSampling.StationLocation.Oid);
                            }
                            objNewSampling.Preservetives = objOldSampling.Preservetives;
                            objNewSampling.SamplingLocation = objOldSampling.SamplingLocation;
                            if (objOldSampling.QCType != null)
                            {
                                objNewSampling.QCType = uow.GetObjectByKey<QCType>(objOldSampling.QCType.Oid);
                            }
                            if (objOldSampling.QCSource != null)
                            {
                                objNewSampling.QCSource = uow.GetObjectByKey<Sampling>(objOldSampling.QCSource.Oid);
                            }
                            if (objOldSampling.Client != null)
                            {
                                objNewSampling.Client = uow.GetObjectByKey<Customer>(objOldSampling.Client.Oid);
                            }
                            if (objOldSampling.Department != null)
                            {
                                objNewSampling.Department = uow.GetObjectByKey<Department>(objOldSampling.Department.Oid);
                            }
                            if (objOldSampling.ProjectID != null)
                            {

                                objNewSampling.ProjectID = uow.GetObjectByKey<Project>(objOldSampling.ProjectID.Oid);
                            }
                            if (objOldSampling.PreserveCondition != null)
                            {

                                objNewSampling.PreserveCondition = uow.GetObjectByKey<PreserveCondition>(objOldSampling.PreserveCondition.Oid);
                            }
                            if (objOldSampling.StorageID != null)
                            {
                                objNewSampling.StorageID = uow.GetObjectByKey<Storage>(objOldSampling.StorageID.Oid);
                            }
                            objNewSampling.CollectDate = objOldSampling.CollectDate;
                            objNewSampling.CollectTime = objOldSampling.CollectTime;
                            objNewSampling.FlowRate = objOldSampling.FlowRate;
                            objNewSampling.TimeStart = objOldSampling.TimeStart;
                            objNewSampling.TimeEnd = objOldSampling.TimeEnd;
                            objNewSampling.Time = objOldSampling.Time;
                            objNewSampling.Volume = objOldSampling.Volume;
                            objNewSampling.Address = objOldSampling.Address;
                            objNewSampling.AreaOrPerson = objOldSampling.AreaOrPerson;
                            if (objOldSampling.BalanceID != null)
                            {
                                objNewSampling.BalanceID = uow.GetObjectByKey<Labware>(objOldSampling.BalanceID.Oid);
                            }
                            objNewSampling.AssignTo = objOldSampling.AssignTo;
                            objNewSampling.Barp = objOldSampling.Barp;
                            objNewSampling.BatchID = objOldSampling.BatchID;
                            objNewSampling.BatchSize = objOldSampling.BatchSize;
                            objNewSampling.BatchSize_pc = objOldSampling.BatchSize_pc;
                            objNewSampling.BatchSize_Units = objOldSampling.BatchSize_Units;
                            objNewSampling.Blended = objOldSampling.Blended;
                            objNewSampling.BottleQty = objOldSampling.BottleQty;
                            objNewSampling.BuriedDepthOfGroundWater = objOldSampling.BuriedDepthOfGroundWater;
                            objNewSampling.ChlorineFree = objOldSampling.ChlorineFree;
                            objNewSampling.ChlorineTotal = objOldSampling.ChlorineTotal;
                            objNewSampling.City = objOldSampling.City;
                            objNewSampling.CollectorPhone = objOldSampling.CollectorPhone;
                            objNewSampling.CompositeQty = objOldSampling.CompositeQty;
                            objNewSampling.DateEndExpected = objOldSampling.DateEndExpected;
                            objNewSampling.DateStartExpected = objOldSampling.DateStartExpected;
                            objNewSampling.Depth = objOldSampling.Depth;
                            objNewSampling.Description = objOldSampling.Description;
                            objNewSampling.DischargeFlow = objOldSampling.DischargeFlow;
                            objNewSampling.DischargePipeHeight = objOldSampling.DischargePipeHeight;
                            objNewSampling.DO = objOldSampling.DO;
                            objNewSampling.DueDate = objOldSampling.DueDate;
                            objNewSampling.Emission = objOldSampling.Emission;
                            objNewSampling.EndOfRoad = objOldSampling.EndOfRoad;
                            objNewSampling.EquipmentModel = objOldSampling.EquipmentModel;
                            objNewSampling.EquipmentName = objOldSampling.EquipmentName;
                            objNewSampling.FacilityID = objOldSampling.FacilityID;
                            objNewSampling.FacilityName = objOldSampling.FacilityName;
                            objNewSampling.FacilityType = objOldSampling.FacilityType;
                            objNewSampling.FinalForm = objOldSampling.FinalForm;
                            objNewSampling.FinalPackaging = objOldSampling.FinalPackaging;
                            objNewSampling.FlowRate = objOldSampling.FlowRate;
                            objNewSampling.FlowRateCubicMeterPerHour = objOldSampling.FlowRateCubicMeterPerHour;
                            objNewSampling.FlowRateLiterPerMin = objOldSampling.FlowRateLiterPerMin;
                            objNewSampling.FlowVelocity = objOldSampling.FlowVelocity;
                            objNewSampling.ForeignMaterial = objOldSampling.ForeignMaterial;
                            objNewSampling.Frequency = objOldSampling.Frequency;
                            objNewSampling.GISStatus = objOldSampling.GISStatus;
                            objNewSampling.GravelContent = objOldSampling.GravelContent;
                            objNewSampling.GrossWeight = objOldSampling.GrossWeight;
                            objNewSampling.GroupSample = objOldSampling.GroupSample;
                            objNewSampling.Hold = objOldSampling.Hold;
                            objNewSampling.Humidity = objOldSampling.Humidity;
                            objNewSampling.IceCycle = objOldSampling.IceCycle;
                            objNewSampling.Increments = objOldSampling.Increments;
                            objNewSampling.Interval = objOldSampling.Interval;
                            objNewSampling.IsActive = objOldSampling.IsActive;
                            objNewSampling.ItemName = objOldSampling.ItemName;
                            objNewSampling.KeyMap = objOldSampling.KeyMap;
                            objNewSampling.LicenseNumber = objOldSampling.LicenseNumber;
                            objNewSampling.ManifestNo = objOldSampling.ManifestNo;
                            objNewSampling.MonitoryingRequirement = objOldSampling.MonitoryingRequirement;
                            objNewSampling.NoOfCollectionsEachTime = objOldSampling.NoOfCollectionsEachTime;
                            objNewSampling.NoOfPoints = objOldSampling.NoOfPoints;
                            objNewSampling.Notes = objOldSampling.Notes;
                            objNewSampling.OriginatingEntiry = objOldSampling.OriginatingEntiry;
                            objNewSampling.OriginatingLicenseNumber = objOldSampling.OriginatingLicenseNumber;
                            objNewSampling.PackageNumber = objOldSampling.PackageNumber;
                            objNewSampling.ParentSampleDate = objOldSampling.ParentSampleDate;
                            objNewSampling.ParentSampleID = objOldSampling.ParentSampleID;
                            objNewSampling.PiecesPerUnit = objOldSampling.PiecesPerUnit;
                            objNewSampling.Preservetives = objOldSampling.Preservetives;
                            objNewSampling.ProjectName = objOldSampling.ProjectName;
                            objNewSampling.PurifierSampleID = objOldSampling.PurifierSampleID;
                            objNewSampling.PWSID = objOldSampling.PWSID;
                            objNewSampling.PWSSystemName = objOldSampling.PWSSystemName;
                            objNewSampling.RegionNameOfSection = objOldSampling.RegionNameOfSection;
                            objNewSampling.RejectionCriteria = objOldSampling.RejectionCriteria;
                            objNewSampling.RepeatLocation = objOldSampling.RepeatLocation;
                            objNewSampling.RetainedWeight = objOldSampling.RetainedWeight;
                            objNewSampling.RiverWidth = objOldSampling.RiverWidth;
                            objNewSampling.RushSample = objOldSampling.RushSample;
                            objNewSampling.SampleAmount = objOldSampling.SampleAmount;
                            objNewSampling.SampleCondition = objOldSampling.SampleCondition;
                            objNewSampling.SampleDescription = objOldSampling.SampleDescription;
                            objNewSampling.SampleImage = objOldSampling.SampleImage;
                            objNewSampling.SampleName = objOldSampling.SampleName;
                            objNewSampling.SamplePointID = objOldSampling.SamplePointID;
                            objNewSampling.SamplePointType = objOldSampling.SamplePointType;
                            if (!objOldSampling.IsReanalysis)
                            {
                                objNewSampling.SampleSource = objOldSampling.SampleSource;
                            }
                            objNewSampling.SampleTag = objOldSampling.SampleTag;
                            objNewSampling.SampleWeight = objOldSampling.SampleWeight;
                            objNewSampling.SamplingAddress = objOldSampling.SamplingAddress;
                            objNewSampling.SamplingEquipment = objOldSampling.SamplingEquipment;
                            objNewSampling.SamplingLocation = objOldSampling.SamplingLocation;
                            objNewSampling.SamplingProcedure = objOldSampling.SamplingProcedure;
                            objNewSampling.SequenceTestSampleID = objOldSampling.SequenceTestSampleID;
                            objNewSampling.SequenceTestSortNo = objOldSampling.SequenceTestSortNo;
                            objNewSampling.ServiceArea = objOldSampling.ServiceArea;
                            objNewSampling.SiteCode = objOldSampling.SiteCode;
                            objNewSampling.SiteDescription = objOldSampling.SiteDescription;
                            objNewSampling.SiteID = objOldSampling.SiteID;
                            objNewSampling.SiteNameArchived = objOldSampling.SiteNameArchived;
                            objNewSampling.SiteUserDefinedColumn1 = objOldSampling.SiteUserDefinedColumn1;
                            objNewSampling.SiteUserDefinedColumn2 = objOldSampling.SiteUserDefinedColumn2;
                            objNewSampling.SiteUserDefinedColumn3 = objOldSampling.SiteUserDefinedColumn3;
                            objNewSampling.SubOut = objOldSampling.SubOut;
                            objNewSampling.SystemType = objOldSampling.SystemType;
                            objNewSampling.TargetMGTHC_CBD_mg_pc = objOldSampling.TargetMGTHC_CBD_mg_pc;
                            objNewSampling.TargetMGTHC_CBD_mg_unit = objOldSampling.TargetMGTHC_CBD_mg_unit;
                            objNewSampling.TargetPotency = objOldSampling.TargetPotency;
                            objNewSampling.TargetUnitWeight_g_pc = objOldSampling.TargetUnitWeight_g_pc;
                            objNewSampling.TargetUnitWeight_g_unit = objOldSampling.TargetUnitWeight_g_unit;
                            objNewSampling.TargetWeight = objOldSampling.TargetWeight;
                            objNewSampling.Time = objOldSampling.Time;
                            objNewSampling.TimeEnd = objOldSampling.TimeEnd;
                            objNewSampling.TimeStart = objOldSampling.TimeStart;
                            objNewSampling.TotalSamples = objOldSampling.TotalSamples;
                            objNewSampling.TotalTimes = objOldSampling.TotalTimes;
                            if (objOldSampling.TtimeUnit != null)
                            {
                                objNewSampling.TtimeUnit = uow.GetObjectByKey<Unit>(objOldSampling.TtimeUnit.Oid);
                            }
                            objNewSampling.WaterType = objOldSampling.WaterType;
                            objNewSampling.ZipCode = objOldSampling.ZipCode;
                            //objNewSampling.ModifiedBy = objOldSampling.ModifiedBy;
                            if (objOldSampling.ModifiedBy != null)
                            {
                                objNewSampling.ModifiedBy = uow.GetObjectByKey<Modules.BusinessObjects.Hr.CustomSystemUser>(objOldSampling.ModifiedBy.Oid);
                            }
                            objNewSampling.ModifiedDate = objOldSampling.ModifiedDate;
                            objNewSampling.Comment = objOldSampling.Comment;
                            objNewSampling.Latitude = objOldSampling.Latitude;
                            objNewSampling.Longitude = objOldSampling.Longitude;
                            objNewSampling.WindDirection = objOldSampling.WindDirection;
                            objNewSampling.Temp = objOldSampling.Temp;
                            objNewSampling.WeatherCondition = objOldSampling.WeatherCondition;
                            objNewSampling.Transparencyk = objOldSampling.Transparencyk;
                            objNewSampling.Transparencyk1 = objOldSampling.Transparencyk1;
                            objNewSampling.Transparencyk2 = objOldSampling.Transparencyk2;
                            List<Testparameter> lsttp = uow.Query<Testparameter>().Where(j => j.QCType.QCTypeName == "Sample" && j.Sampling.Where(a => a.Oid == objOldSampling.Oid).Count() > 0).ToList();
                            foreach (var objLineA in lsttp)
                            {
                                objNewSampling.Testparameters.Add(uow.GetObjectByKey<Testparameter>(objLineA.Oid));
                            }
                            foreach (var objSampleparameter in objOldSampling.SamplingParameter.Where(a => a.IsGroup == true && a.GroupTest != null).ToList())
                            {
                                SamplingParameter sample = objNewSampling.SamplingParameter.FirstOrDefault<SamplingParameter>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                                if (objSampleparameter.GroupTest != null && sample != null)
                                {
                                    sample.IsGroup = true;
                                    sample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objSampleparameter.GroupTest.Oid);
                                }
                            }
                            foreach (var objSampleparameter in objOldSampling.SamplingParameter.Where(a => a.SubOut == true).ToList())
                            {
                                SamplingParameter sample = objNewSampling.SamplingParameter.FirstOrDefault<SamplingParameter>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                                if (sample != null)
                                {
                                    sample.SubOut = true;
                                }
                            }
                            objNewSampling.Save();
                            List<SamplingBottleAllocation> smplold = uow.Query<SamplingBottleAllocation>().Where(i => i.Sampling != null && i.Sampling.Oid == objOldSampling.Oid).ToList();
                            if (smplold != null && smplold.Count > 0)
                            {
                                foreach (SamplingBottleAllocation smpl in smplold.ToList())
                                {
                                    SamplingBottleAllocation smplnew = new SamplingBottleAllocation(uow);
                                    smplnew.Sampling = objNewSampling;
                                    smplnew.TestMethod = uow.GetObjectByKey<TestMethod>(smpl.TestMethod.Oid);
                                    smplnew.BottleID = smpl.BottleID;
                                    if (smpl.Containers != null)
                                    {
                                        smplnew.Containers = uow.GetObjectByKey<Container>(smpl.Containers.Oid);
                                    }
                                    if (smpl.Preservative != null)
                                    {
                                        smplnew.Preservative = uow.GetObjectByKey<Preservative>(smpl.Preservative.Oid);
                                    }
                                    if (smpl.StorageID != null)
                                    {
                                        smplnew.StorageID = uow.GetObjectByKey<Storage>(smpl.StorageID.Oid);
                                    }
                                    if (smpl.StorageCondition != null)
                                    {
                                        smplnew.StorageCondition = uow.GetObjectByKey<PreserveCondition>(smpl.StorageCondition.Oid);
                                    }
                                }
                            }
                        }
                        #endregion
                        uow.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        DateTime srDateFilter = DateTime.MinValue;
                        if (SamplingProposalDateFilterAction != null && SamplingProposalDateFilterAction.SelectedItem != null)
                        {
                            if (SamplingProposalDateFilterAction.SelectedItem.Id == "3M")
                            {
                                srDateFilter = DateTime.Today.AddMonths(-3);
                            }
                            else if (SamplingProposalDateFilterAction.SelectedItem.Id == "6M")
                            {
                                srDateFilter = DateTime.Today.AddMonths(-6);
                            }
                            else if (SamplingProposalDateFilterAction.SelectedItem.Id == "1Y")
                            {
                                srDateFilter = DateTime.Today.AddYears(-1);
                            }
                            else if (SamplingProposalDateFilterAction.SelectedItem.Id == "2Y")
                            {
                                srDateFilter = DateTime.Today.AddYears(-2);
                            }
                        }
                        if (srDateFilter != DateTime.MinValue)
                        {
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[RecievedDate] >= ? and [RecievedDate] <= ?", srDateFilter, DateTime.Now);
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                        }
                        View.ObjectSpace.Refresh();

                    }
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
        private void CreateNewRegistrationID(UnitOfWork uow,SamplingProposal objSamplingProposal)
        {
            try
            {
                var curdate = DateTime.Now;
                string strjobid = string.Empty;
                int formatlen = 0;
                SamplingProposalIDFormat objJDformat = uow.FindObject<SamplingProposalIDFormat>(CriteriaOperator.Parse(""));
                if (objJDformat != null)
                {
                    if (objJDformat.Year == YesNoFilter.Yes)
                    {
                        strjobid += curdate.ToString(objJDformat.YearFormat.ToString());
                        formatlen = objJDformat.YearFormat.ToString().Length;
                    }
                    if (objJDformat.Month == YesNoFilter.Yes)
                    {
                        strjobid += curdate.ToString(objJDformat.MonthFormat.ToUpper());
                        formatlen = formatlen + objJDformat.MonthFormat.Length;
                    }
                    if (objJDformat.Day == YesNoFilter.Yes)
                    {
                        strjobid += curdate.ToString(objJDformat.DayFormat);
                        formatlen = formatlen + objJDformat.DayFormat.Length;
                    }
                    CriteriaOperator sam = objJDformat.Prefix == YesNoFilter.Yes ? CriteriaOperator.Parse("Max(SUBSTRING(RegistrationID, " + objJDformat.PrefixValue.ToString().Length + "))") : CriteriaOperator.Parse("Max(SUBSTRING(RegistrationID, 0))");
                    CriteriaOperator filternew = CriteriaOperator.Parse("[IsAlpacJobid]=1");
                    //string tempid = (Convert.ToInt32(((XPObjectSpace)this.ObjectSpace).Session.Evaluate(typeof(SamplingProposal), sam, filternew)) + 1).ToString();
                    string tempid = (Convert.ToInt32(uow.Evaluate(typeof(SamplingProposal), sam, filternew)) + 1).ToString();
                    if (tempid != "1")
                    {
                        var predate = tempid.Substring(0, formatlen);
                        if (predate == strjobid)
                        {
                            if (objJDformat.Prefix == YesNoFilter.Yes)
                            {
                                if (!string.IsNullOrEmpty(objJDformat.PrefixValue))
                                {
                                    strjobid = objJDformat.PrefixValue + tempid;
                                }
                            }
                            else
                            {
                                strjobid = tempid;
                            }
                        }
                        else
                        {
                            if (objJDformat.Prefix == YesNoFilter.Yes)
                            {
                                if (!string.IsNullOrEmpty(objJDformat.PrefixValue))
                                {
                                    strjobid = objJDformat.PrefixValue + strjobid;
                                }
                            }
                            if (objJDformat.SequentialNumber > 1)
                            {
                                if (objJDformat.NumberStart > 0)
                                {
                                    strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - objJDformat.NumberStart.ToString().Length)), '0') + objJDformat.NumberStart;
                                }
                                else
                                {
                                    strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - 1)), '0') + "1";
                                }
                            }
                            else
                            {
                                if (objJDformat.NumberStart > 0 && objJDformat.NumberStart < 10)
                                {
                                    strjobid = strjobid + objJDformat.NumberStart;
                                }
                                else
                                {
                                    strjobid = strjobid + "1";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (objJDformat.Prefix == YesNoFilter.Yes)
                        {
                            if (!string.IsNullOrEmpty(objJDformat.PrefixValue))
                            {
                                strjobid = objJDformat.PrefixValue + strjobid;
                            }
                        }
                        if (objJDformat.SequentialNumber > 1)
                        {
                            if (objJDformat.NumberStart > 0)
                            {
                                strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - objJDformat.NumberStart.ToString().Length)), '0') + objJDformat.NumberStart;
                            }
                            else
                            {
                                strjobid = strjobid.PadRight(Convert.ToInt32(strjobid.Length + (objJDformat.SequentialNumber - 1)), '0') + "1";
                            }
                        }
                        else
                        {
                            if (objJDformat.NumberStart > 0 && objJDformat.NumberStart < 10)
                            {
                                strjobid = strjobid + objJDformat.NumberStart;
                            }
                            else
                            {
                                strjobid = strjobid + "1";
                            }
                        }
                    }
                    objSamplingProposal.RegistrationID = strJobID = strjobid;
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Cancel_RegistrationID_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
           try
            {

            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void History_Excecute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SamplingProposal_ListView")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(os, typeof(SamplingProposal));
                    ListView lvhistory = Application.CreateListView("SamplingProposal_ListView_History", cs, true);
                    Frame.SetView(lvhistory);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>()
                   .InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
