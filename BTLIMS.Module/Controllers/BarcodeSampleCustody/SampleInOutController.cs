using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.BarCodeSampleCustody;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.Barcode_Sample_Custody
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SampleInOutController : ViewController
    {
        ModificationsController mc;
        MessageTimer timer = new MessageTimer();
        public SampleInOutController()
        {
            InitializeComponent();
            TargetViewId = "SampleCustodyTest_DetailView_SampleIn;" + "SampleCustodyTest_DetailView_SampleOut;" + "SampleCustodyTest_DetailView_SampleDisposal;" + "SampleIn_ListView;" + "SampleCustodyTest_DetailView_SampleLocation;"
                + "SampleCustodyTest_SampleIns_ListView_SampleIn;" + "SampleCustodyTest_SampleIns_ListView_SampleOut;" + "SampleCustodyTest_SampleDisposals_ListView;" + "SampleCustodyTest_SampleIns_ListView_Copy_samplelocation;"
                + "SampleCustodyTest_DetailView_SampleLocation_History;" + "SampleCustodyTest_DetailView_SampleDisposal_History;" + "SampleCustodyTest_SampleIns_ListView_SampleDisposal_History;" + "SampleCustodyTest_SampleIns_ListView_SampleDisposal;";
            btn_Disposal_History.TargetViewId = "SampleCustodyTest_DetailView_SampleDisposal;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            try
            {
                if (View.Id == "SampleCustodyTest_DetailView_SampleIn" || View.Id == "SampleCustodyTest_DetailView_SampleOut" || View.Id == "SampleCustodyTest_DetailView_SampleDisposal" || View.Id == "SampleCustodyTest_DetailView_SampleDisposal_History" ||
                    View.Id == "SampleCustodyTest_DetailView_SampleLocation" || View.Id == "SampleCustodyTest_DetailView_SampleLocation_History")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                if (Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleIn" || Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleOut" || Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleDisposal")
                {
                    Frame.GetController<WebLinkUnlinkController>().UnlinkAction.Executing += UnlinkAction_Executing;
                }
                //mc = Frame.GetController<ModificationsController>();
                //if (mc != null && View.Id == "SampleCustodyTest_DetailView_SampleIn" || View.Id == "SampleCustodyTest_DetailView_SampleOut" || View.Id == "SampleCustodyTest_DetailView_SampleDisposal" || View.Id == "SampleCustodyTest_DetailView_SampleDisposal_History")
                //{
                //    mc.SaveAction.Executing += SaveAction_Executing;
                //}
                if (Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleIn" || Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleOut" || Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleDisposal"
                    || Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleLocation" || Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleDisposal_History")
                {
                    Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    Frame.GetController<ModificationsController>().CancelAction.Active["DisableCancel"] = false;
                    Frame.GetController<ModificationsController>().SaveAction.Active["DisableSave"] = false;
                }
                if (View.Id == "SampleIn_ListView")
                {
                    DevExpress.ExpressApp.SystemModule.FilterController filterController = Frame.GetController<DevExpress.ExpressApp.SystemModule.FilterController>();
                    if (filterController != null)
                    {
                        filterController.FullTextFilterAction.Executing += FullTextFilterAction_Executing;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void UnlinkAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.Id == "SampleCustodyTest_SampleIns_ListView_SampleIn" || View.Id == "SampleCustodyTest_SampleIns_ListView_SampleOut" || View.Id == "SampleCustodyTest_SampleIns_ListView_SampleDisposal")
                {
                    bool itemUnlinked = false;
                    IObjectSpace objSpace = Application.MainWindow.View.ObjectSpace;
                    SampleIn objSampleIn = View.CurrentObject as SampleIn;
                    if (objSampleIn != null  && View.SelectedObjects.Count > 0)
        {
                        if (View.Id == "SampleCustodyTest_SampleIns_ListView_SampleIn")
            {
                            foreach (SampleIn itemToUnlink in View.SelectedObjects)
                            {
                                CriteriaOperator criteriaofIn = CriteriaOperator.Parse("[SampleBottleID]='" + itemToUnlink.SampleBottleID + "' AND [Mode]='In'");
                            IList<SampleIn> ListOfIn = ObjectSpace.GetObjects<SampleIn>(criteriaofIn);
                            if (ListOfIn.Count > 0)
                {
                                var sampleIn = View.SelectedObjects.Cast<SampleIn>().Where(i => i.Mode == "In").ToList();
                                ObjectSpace.Delete(sampleIn);
                                itemUnlinked = true;
                            } 
                    }
                           
                        }
                        else if (View.Id == "SampleCustodyTest_SampleIns_ListView_SampleOut")
                    {
                            foreach (SampleIn itemToUnlink in View.SelectedObjects)
                            {
                                CriteriaOperator criteriaofOut = CriteriaOperator.Parse("[SampleBottleID]='" + itemToUnlink.SampleBottleID + "' AND [Mode]='Out'");
                            IList<SampleIn> ListOfOut = ObjectSpace.GetObjects<SampleIn>(criteriaofOut);
                            if (ListOfOut.Count > 0)
                    {
                                var sampleIn = View.SelectedObjects.Cast<SampleIn>().Where(i => i.Mode == "Out").ToList();
                                ObjectSpace.Delete(sampleIn);
                                itemUnlinked = true;
                    }
                }
                        }
                        else if (View.Id == "SampleCustodyTest_SampleIns_ListView_SampleDisposal")
                {
                            foreach (SampleIn itemToUnlink in View.SelectedObjects)
                            {
                                CriteriaOperator criteriaofDisposal = CriteriaOperator.Parse("[SampleBottleID]='" + itemToUnlink.SampleBottleID + "' AND ([Mode]='Disposed' Or [Mode]='Depleted')");
                            IList<SampleIn> ListOfDisposal = ObjectSpace.GetObjects<SampleIn>(criteriaofDisposal);
                            if (ListOfDisposal.Count > 0)
                    {
                                var sampleIn = View.SelectedObjects.Cast<SampleIn>().Where(i => i.Mode == "Disposed" || i.Mode == "Depleted").ToList();
                                ObjectSpace.Delete(sampleIn);
                                itemUnlinked = true;
                    }
                    }
                        }
                        if (itemUnlinked)
                    {
                            ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage("Sample 'Bottle ID' unlinked successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top); 
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
        //private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    try
        //    {
        //        if (View.Id == "SampleCustodyTest_DetailView_SampleIn")
        //        {
        //            SampleCustodyTest sampleCustodyTest = (SampleCustodyTest)View.CurrentObject;
        //            if (string.IsNullOrEmpty(sampleCustodyTest.BarcodeScan) && sampleCustodyTest.SampleIns.Count == 0)
        //            {
        //                e.Cancel = true;
        //                Application.ShowViewStrategy.ShowMessage("Please scan the sample 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
        //            }
        //            else if (sampleCustodyTest.SampleIns.Count == 0)
        //            {
        //                e.Cancel = true;
        //                Application.ShowViewStrategy.ShowMessage("Please enter a valid 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
        //            }
        //            else
        //            {
        //                Application.ShowViewStrategy.ShowMessage("Scanned and saved successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
        //            }
        //        }
        //        if (View.Id == "SampleCustodyTest_DetailView_SampleOut")
                //{
                //    SampleCustodyTest sampleCustodyTest = (SampleCustodyTest)View.CurrentObject;
        //            if (string.IsNullOrEmpty(sampleCustodyTest.BarcodeScan) && sampleCustodyTest.SampleIns.Count == 0)
                //    {
                //        e.Cancel = true;
                //        Application.ShowViewStrategy.ShowMessage("Please scan the sample 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                //    }
                //    else if (sampleCustodyTest.SampleIns.Count == 0)
                //    {
                //        e.Cancel = true;
                //        Application.ShowViewStrategy.ShowMessage("Please enter a valid 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                //    }
                //    else
                //    {
                //        Application.ShowViewStrategy.ShowMessage("Scanned and saved successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                //    }
                //}
        //        if (View.Id == "SampleCustodyTest_DetailView_SampleDisposal")
        //        {
        //            SampleCustodyTest sampleCustodyTest = (SampleCustodyTest)View.CurrentObject;
        //            if (string.IsNullOrEmpty(sampleCustodyTest.BarcodeScan) && sampleCustodyTest.SampleIns.Count == 0)
        //            {
        //                e.Cancel = true;
        //                Application.ShowViewStrategy.ShowMessage("Please scan the sample 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
        //            }
        //            else if (sampleCustodyTest.SampleIns.Count == 0)
        //            {
        //                e.Cancel = true;
        //                Application.ShowViewStrategy.ShowMessage("Please enter a valid 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
        //            }
        //            else
        //            {
        //                Application.ShowViewStrategy.ShowMessage("Scanned and disposed successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
        private void FullTextFilterAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                DevExpress.ExpressApp.SystemModule.FilterController filterController = Frame.GetController<DevExpress.ExpressApp.SystemModule.FilterController>();
                IObjectSpace objSpace = Application.MainWindow.View.ObjectSpace;
                string strsearchtext = filterController.FullTextFilterAction.Value.ToString();
                if (strsearchtext != null)
                {
                    IList<SampleIn> objsmplin = objSpace.GetObjects<SampleIn>(CriteriaOperator.Parse("SampleBottleID='" + strsearchtext + "'"));
                    if (objsmplin.Count > 0)
                    {
                        IList<SampleIn> objsmpldpl = ObjectSpace.GetObjects<SampleIn>(CriteriaOperator.Parse("SampleBottleID='" + strsearchtext + "' AND ([Mode]='Disposed' Or [Mode]='Depleted')"));
                        if (objsmpldpl.Count > 0)
                        {
                            Application.ShowViewStrategy.ShowMessage("Sample 'Bottle ID' has been disposed already.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                        }
                        else
                        {
                            foreach (var i in objsmplin)
                            {
                                objsmplin.Add(i);
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
                if (View != null && View.Id == "SampleCustodyTest_DetailView_SampleIn" && e.Object != null)
                {
                    if ((e.PropertyName == "To" || e.PropertyName == "BarcodeScan") && e.Object.GetType() == typeof(SampleCustodyTest))
                    {
                        IObjectSpace objSpace = Application.MainWindow.View.ObjectSpace;
                        SampleCustodyTest objSampleCustody = (SampleCustodyTest)e.Object;
                        if (objSampleCustody != null)
                        {
                            if (objSampleCustody.To != null && !string.IsNullOrEmpty(objSampleCustody.BarcodeScan))
                            {
                                CriteriaOperator criteria = CriteriaOperator.Parse("[SampleBottleID]='" + objSampleCustody.BarcodeScan + "'");
                                IList<SampleIn> ListObject = ObjectSpace.GetObjects<SampleIn>(criteria);
                                if (ListObject.Count > 0)
                                {
                                    Application.ShowViewStrategy.ShowMessage("This 'Bottle ID' exists already. Please enter a new 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    objSampleCustody.BarcodeScan = "";
                                    return;
                                }
                                else
                                {
                                    SampleIn objSampleIn = objSpace.CreateObject<SampleIn>();
                                    if (objSampleCustody.SampleIns.Count > 0)
                                    {
                                        foreach (SampleIn objsamp in objSampleCustody.SampleIns)
                                        {
                                            if (objsamp.SampleBottleID == objSampleCustody.BarcodeScan)
                                            {
                                                objSampleCustody.SampleIns.Remove(objsamp);
                                                break;
                                            }
                                        }
                                    }
                                    string strValue = objSampleCustody.BarcodeScan;
                                    string[] strBarcode = strValue.Split('-');

                                    if (strBarcode.Length > 1)
                                    {
                                        string strdummy = strBarcode[0] + "-" + strBarcode[1];
                                        Modules.BusinessObjects.SampleManagement.SampleLogIn objSampleID = objSpace.FindObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[SampleID] = ?", strdummy));
                                        if (objSampleID != null && objSampleID.JobID != null)
                                        {
                                            objSampleIn.JobID = objSampleID.JobID;
                                            if (objSampleID.JobID.ClientName != null)
                                            {
                                                objSampleIn.Client = objSampleID.JobID.ClientName;
                                            }
                                            objSampleIn.SampleBottleID = objSampleCustody.BarcodeScan;
                                            objSampleIn.From = SecuritySystem.CurrentUser.ToString();
                                            if (objSampleCustody.To != null)
                                            {
                                                objSampleIn.To = objSampleCustody.To;
                                            }
                                            objSampleIn.DateHandled = DateTime.Now;
                                            objSampleIn.HandledBy = objSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                            objSampleIn.Mode = "In";
                                            objSampleCustody.SampleIns.Add(objSampleIn);
                                            objSpace.CommitChanges();
                                            objSampleCustody.BarcodeScan = "";
                                        }
                                        else
                                        {
                                            Application.ShowViewStrategy.ShowMessage("Please enter a valid 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                            objSampleCustody.BarcodeScan = "";
                                        }
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage("Please enter a valid 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        objSampleCustody.BarcodeScan = "";
                                    }
                                }
                            }
                        }
                        else
                        {
                            objSampleCustody.BarcodeScan = null;
                            Application.ShowViewStrategy.ShowMessage("Column 'To' should not be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                else if (View != null && View.Id == "SampleCustodyTest_DetailView_SampleOut" && e.Object != null)
                {
                    if (e.PropertyName == "BarcodeScan" && e.Object.GetType() == typeof(SampleCustodyTest))
                    {
                        IObjectSpace objSpace = Application.MainWindow.View.ObjectSpace;
                        SampleCustodyTest objSampleCustody = (SampleCustodyTest)e.Object;
                        if (objSampleCustody != null && objSampleCustody.BarcodeScan != "")
                        {
                            CriteriaOperator criteriaofOut = CriteriaOperator.Parse("[SampleBottleID]='" + objSampleCustody.BarcodeScan + "' AND [Mode]='Out'");
                            IList<SampleIn> ListOfOut = ObjectSpace.GetObjects<SampleIn>(criteriaofOut);
                            if (ListOfOut.Count > 0)
                        {
                                Application.ShowViewStrategy.ShowMessage("This 'Bottle ID' exists already. Please enter a new 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                objSampleCustody.BarcodeScan = "";
                                return;
                            }
                            CriteriaOperator criteria = CriteriaOperator.Parse("[SampleBottleID]='" + objSampleCustody.BarcodeScan + "'");
                            IList<SampleIn> ListObject = ObjectSpace.GetObjects<SampleIn>(criteria);
                            if (ListObject.Count > 0)
                            {
                                string maxDateTime = ListObject.Max(x => x.DateHandled).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                CriteriaOperator criteria1 = CriteriaOperator.Parse("[SampleBottleID]='" + objSampleCustody.BarcodeScan + "' AND [DateHandled]='" + maxDateTime + "' AND [Mode]='In'");
                                SampleIn objGetSample = objSpace.FindObject<SampleIn>(criteria1);

                                if (objGetSample != null)
                                {
                                    SampleIn objSampleIn = objSpace.CreateObject<SampleIn>();
                                    objSampleIn.SampleBottleID = objSampleCustody.BarcodeScan;
                                    string strValue = objSampleCustody.BarcodeScan;
                                    string[] strBarcode = strValue.Split('-');
                                    if (strBarcode.Length > 0)
                                    {
                                        string strdummy = strBarcode[0] + "-" + strBarcode[1];
                                        Modules.BusinessObjects.SampleManagement.SampleLogIn objSampleID = objSpace.FindObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[SampleID] = ?", strdummy));
                                        if (objSampleID != null && objSampleID.JobID != null)
                                        {
                                            objSampleIn.JobID = objSampleID.JobID;
                                            if (objSampleID.JobID.ClientName != null)
                                            {
                                                objSampleIn.Client = objSampleID.JobID.ClientName;
                                            }
                                        }
                                    }
                                    objSampleIn.From = objGetSample.To;
                                    objSampleIn.To = objSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).ToString();
                                    objSampleIn.DateHandled = DateTime.Now;
                                    objSampleIn.HandledBy = objSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    objSampleIn.Mode = "Out";
                                    objSampleCustody.SampleIns.Add(objSampleIn);
                                    objSpace.CommitChanges();
                                    objSampleCustody.BarcodeScan = "";
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage("Please enter a valid 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                objSampleCustody.BarcodeScan = "";
                            }
                        }
                    }
                }
                else if (View != null && View.Id == "SampleCustodyTest_DetailView_SampleDisposal" && e.Object != null)
                {
                    if (e.PropertyName == "BarcodeScan" && e.Object.GetType() == typeof(SampleCustodyTest))
                    {
                        IObjectSpace objSpace = Application.MainWindow.View.ObjectSpace;
                        SampleCustodyTest objSampleCustody = (SampleCustodyTest)e.Object;
                        if (objSampleCustody != null && objSampleCustody.BarcodeScan != "")
                        {
                            CriteriaOperator criteriaofDisposed = CriteriaOperator.Parse("[SampleBottleID]='" + objSampleCustody.BarcodeScan + "' AND ([Mode]='Disposed' Or [Mode]='Depleted')");
                            IList<SampleIn> ListOfDisposed = ObjectSpace.GetObjects<SampleIn>(criteriaofDisposed);
                            if (ListOfDisposed.Count > 0)
                        {
                                Application.ShowViewStrategy.ShowMessage("This 'Bottle ID' disposed already. Please enter a new 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                objSampleCustody.BarcodeScan = "";
                                return;
                            }
                            SampleIn objGetSample = objSpace.FindObject<SampleIn>(CriteriaOperator.Parse("SampleBottleID='" + objSampleCustody.BarcodeScan + "'"));
                            string strValue = objSampleCustody.BarcodeScan;
                            string[] strBarcode = strValue.Split('-');
                            if (strBarcode.Length > 0)
                            {
                                string strdummy = strBarcode[0] + "-" + strBarcode[1];
                                Modules.BusinessObjects.SampleManagement.SampleLogIn objJobID = objSpace.FindObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[SampleID] = ?", strdummy));
                                if (objJobID != null && objJobID.JobID != null && objGetSample != null)
                                {
                                    objGetSample.JobID = objJobID.JobID;
                                    if (objJobID.JobID.ClientName != null)
                                    {
                                        objGetSample.Client = objJobID.JobID.ClientName;
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Please enter a valid 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    objSampleCustody.BarcodeScan = "";
                                    return;
                                }
                                if (objGetSample != null && objGetSample.JobID != null && objJobID.VisualMatrix != null && objJobID.VisualMatrix.DaysSampleKeeping > 0)
                                {
                                    DateTime objDt = new DateTime();
                                    uint days = objJobID.VisualMatrix.DaysSampleKeeping;
                                    objDt = objGetSample.JobID.CreatedDate.AddDays(days);
                                    if (objDt.Date < DateTime.Today)
                                    {
                                        SampleIn objExpired = objSpace.CreateObject<SampleIn>();
                                        objExpired.SampleBottleID = objSampleCustody.BarcodeScan;
                                        objExpired.DateDisposed = DateTime.Now;
                                        objExpired.DisposedBy = objSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objExpired.DateHandled = DateTime.Now;
                                        objExpired.HandledBy = objSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objSampleCustody.SampleIns.Add(objExpired);
                                        if (objExpired.Deplete)
                                        {
                                            objExpired.Mode = "Depleted";
                                        }
                                        else
                                            {
                                            objExpired.Mode = "Disposed";
                                            }
                                        objSpace.CommitChanges();
                                        objSampleCustody.BarcodeScan = "";
                                        }
                                        else
                                        {
                                        Application.ShowViewStrategy.ShowMessage("This 'Bottle ID' has not expired yet.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        objSampleCustody.BarcodeScan = "";
                                        return;
                                            }
                                        }
                                else
                                    {
                                    SampleIn objDispose = objSpace.CreateObject<SampleIn>();
                                    objDispose.SampleBottleID = objSampleCustody.BarcodeScan;
                                        objDispose.DateDisposed = DateTime.Now;
                                        objDispose.DisposedBy = objSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    objDispose.DateHandled = DateTime.Now;
                                    objDispose.HandledBy = objSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    objSampleCustody.SampleIns.Add(objDispose);
                                        if (objDispose.Deplete)
                                        {
                                        objDispose.Mode = "Depleted";
                                        }
                                        else
                                        {
                                        objDispose.Mode = "Disposed";
                                        }
                                        objSpace.CommitChanges();
                                    objSampleCustody.BarcodeScan = ""; 
                                } 
                            }
                            else if (objGetSample != null && objGetSample.JobID != null && objGetSample.JobID.SampleMatries != null)
                            {
                                string[] arrSampleMatrices = objGetSample.JobID.SampleMatries.Split(';');
                                IList<VisualMatrix> lstSampleMatrices = objSpace.GetObjects<VisualMatrix>(new InOperator("Oid", arrSampleMatrices.Select(i => new Guid(i)).ToList()));
                                if (lstSampleMatrices != null && lstSampleMatrices.Count > 0)
                                {
                                    uint intDaysSampleKeeping = lstSampleMatrices.Max(i => i.DaysSampleKeeping);
                                    DateTime objDt = new DateTime();
                                    objDt = objGetSample.JobID.CreatedDate.AddDays(intDaysSampleKeeping);
                                    if (objDt.Date < DateTime.Today.Date)
                                    {
                                        SampleIn objDispose = objSpace.CreateObject<SampleIn>();
                                        objDispose.SampleBottleID = objSampleCustody.BarcodeScan;
                                        objDispose.DateDisposed = DateTime.Now;
                                        objDispose.DisposedBy = objSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objDispose.DateHandled = DateTime.Now;
                                        objDispose.HandledBy = objSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objSampleCustody.SampleIns.Add(objDispose);
                                        if (objDispose.Deplete)
                                        {
                                            objDispose.Mode = "Depleted";
                                        }
                                        else
                                        {
                                            objDispose.Mode = "Disposed";
                                        }
                                        objSpace.CommitChanges();
                                        objSampleCustody.BarcodeScan = "";
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage("This 'Bottle ID' has not expired yet.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        objSampleCustody.BarcodeScan = "";
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (View != null && View.Id == "SampleCustodyTest_DetailView_SampleDisposal_History" && e.Object != null)
                {
                    if (e.PropertyName == "BarcodeScan" && e.Object.GetType() == typeof(SampleCustodyTest))
                    {
                        IObjectSpace objSpace = Application.MainWindow.View.ObjectSpace;
                        SampleCustodyTest objSampleCustody = (SampleCustodyTest)e.Object;
                        if (objSampleCustody != null && !string.IsNullOrEmpty(objSampleCustody.BarcodeScan))
                        {
                            IList<SampleIn> objGetSample = objSpace.GetObjects<SampleIn>(CriteriaOperator.Parse("[SampleBottleID]='" + objSampleCustody.BarcodeScan + "'"));
                            if (objGetSample != null && objGetSample.Count > 0)
                            {
                                foreach (var i in objGetSample)
                                {
                                    if (i.Deplete)
                                    {
                                        i.Mode = "Depleted";
                                    }
                                    objSampleCustody.SampleIns.Add(i);
                                }
                                objSampleCustody.BarcodeScan = "";
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage("This 'Bottle ID' has not disposed yet.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                objSampleCustody.BarcodeScan = "";
                                return;
                            }
                        }
                    }
                }
                else if (View != null && View.Id == "SampleCustodyTest_DetailView_SampleLocation" && e.Object != null)
                {
                    if (e.PropertyName == "BarcodeScan" && e.Object.GetType() == typeof(SampleCustodyTest))
                    {
                        IObjectSpace objSpace = Application.MainWindow.View.ObjectSpace;
                        SampleCustodyTest objSampleCustody = (SampleCustodyTest)e.Object;
                        if (objSampleCustody != null && objSampleCustody.BarcodeScan != "")
                        {
                            CriteriaOperator criteria = CriteriaOperator.Parse("[SampleBottleID]='" + objSampleCustody.BarcodeScan + "'");
                            IList<SampleIn> ListObject = ObjectSpace.GetObjects<SampleIn>(criteria);
                            if (ListObject != null && ListObject.Count > 0)
                            {
                                string maxDateTime = ListObject.Max(x => x.DateHandled).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                IList<SampleIn> objGetSample = objSpace.GetObjects<SampleIn>(CriteriaOperator.Parse("[SampleBottleID]='" + objSampleCustody.BarcodeScan + "' AND [DateHandled]='" + maxDateTime + "'"));
                                //CriteriaOperator criteria1 = CriteriaOperator.Parse("[SampleBottleID]='" + objSampleCustody.BarcodeScan + "' AND [DateHandled]='" + maxDateTime + "'");
                                //SampleIn objGetSample = objSpace.FindObject<SampleIn>(criteria1);
                                if (objGetSample != null && objGetSample.Count > 0)
                                {
                                    foreach (SampleIn j in objGetSample)
                                {
                                        CustomSystemUser userName = j.HandledBy;
                                        objSampleCustody.To = j.To;
                                        objSampleCustody.SampleIns.Add(j);
                                        IList<PermissionPolicyUser> objPerUs = objSpace.GetObjects<PermissionPolicyUser>(CriteriaOperator.Parse("UserName='" + objSampleCustody.To + "'"));
                                    if (objPerUs.Count > 0)
                                    {
                                            j.User = objSampleCustody.To;
                                    }
                                    else
                                    {
                                            j.Storage = objSampleCustody.To;
                                    }
                                }
                                    objSampleCustody.BarcodeScan = "";
                                    Application.MainWindow.View.Refresh();
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage("Please enter a valid 'Bottle ID'.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                objSampleCustody.BarcodeScan = "";
                            }
                            Application.MainWindow.View.Refresh();
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
            base.OnViewControlsCreated();
            try
            {
                if (View.Id == "SampleCustodyTest_SampleIns_ListView_SampleIn" || View.Id == "SampleCustodyTest_SampleIns_ListView_SampleOut" || View.Id == "SampleCustodyTest_SampleIns_ListView_SampleDisposal"
                    || View.Id == "SampleCustodyTest_SampleIns_ListView_SampleDisposal_History" || View.Id == "SampleCustodyTest_SampleIns_ListView_Copy_samplelocation")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
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
                if (View.Id == "SampleCustodyTest_DetailView_SampleIn" || View.Id == "SampleCustodyTest_DetailView_SampleOut" || View.Id == "SampleCustodyTest_DetailView_SampleDisposal" || View.Id == "SampleCustodyTest_DetailView_SampleDisposal_History" ||
                    View.Id == "SampleCustodyTest_DetailView_SampleLocation" || View.Id == "SampleCustodyTest_DetailView_SampleLocation_History")
                {
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                }
                if (Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleIn" || Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleOut" || Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleDisposal")
                {
                    Frame.GetController<WebLinkUnlinkController>().UnlinkAction.Executing -= UnlinkAction_Executing;
                }
                //mc = Frame.GetController<ModificationsController>();
                //if (mc != null && View.Id == "SampleCustodyTest_DetailView_SampleIn" || View.Id == "SampleCustodyTest_DetailView_SampleOut" || View.Id == "SampleCustodyTest_DetailView_SampleDisposal" || View.Id == "SampleCustodyTest_DetailView_SampleDisposal_History")
                //{
                //    mc.SaveAction.Executing -= SaveAction_Executing;
                //}
                if (View.Id == "SampleIn_ListView")
                {
                    DevExpress.ExpressApp.SystemModule.FilterController filterController = Frame.GetController<DevExpress.ExpressApp.SystemModule.FilterController>();
                    if (filterController != null)
                    {
                        filterController.FullTextFilterAction.Executing -= FullTextFilterAction_Executing;
                    }
                }
                if (Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleIn" || Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleOut" || Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleDisposal"
                    || Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleLocation" || Application.MainWindow.View.Id == "SampleCustodyTest_DetailView_SampleDisposal_History")
                {
                    Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active["DisableUnsavedChangesNotificationController"] = true;
                    Frame.GetController<ModificationsController>().CancelAction.Active["DisableCancel"] = true;
                    Frame.GetController<ModificationsController>().SaveAction.Active["DisableSave"] = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void btn_Disposal_History_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SampleCustodyTest_DetailView_SampleDisposal")
                {
                    ObjectSpace.CommitChanges();
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    SampleCustodyTest objToShow = objspace.CreateObject<SampleCustodyTest>();
                    if (objToShow != null)
                    {
                        DetailView CreateDetailView = Application.CreateDetailView(objspace, "SampleCustodyTest_DetailView_SampleDisposal_History", false, objToShow);
                        CreateDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                        Frame.SetView(CreateDetailView);
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
