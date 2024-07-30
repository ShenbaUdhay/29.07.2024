using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Accounting.Receivables;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting.Invoicing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.Accounting.Receivables
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DepositsViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        decimal OldAmountReceived = 0;
        public DepositsViewController()
        {
            InitializeComponent();
            TargetViewId = "DepositPayment_DetailView;" + "Deposits_ListView;" + "Deposits_DepositPayments_ListView;" + "InvoicingContact_LookupListView_DepositNotes;"
                + "Deposits_ListView_History;" + "Deposits_DetailView_History;" + "Deposits_DetailView;" + "DepositEDDExport_ListView;";
            DepositHistory.TargetViewId = "Deposits_ListView;";
            DepositRollback.TargetViewId = "Deposits_ListView_History;" + "Deposits_DetailView_History;";
            DepositEDDDetail.TargetViewId = "DepositEDDExport_ListView;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "DepositPayment_DetailView")
                {
                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(Deposits))
                    {
                        Deposits objCurrentDeposit = (Deposits)Application.MainWindow.View.CurrentObject;
                        ListPropertyEditor lvDeposit = ((DetailView)Application.MainWindow.View).FindItem("DepositPayments") as ListPropertyEditor;
                        if (objCurrentDeposit != null && lvDeposit != null && lvDeposit.ListView != null)
                        {
                            decimal SumAmountReceived = Convert.ToDecimal(((ListView)lvDeposit.ListView).CollectionSource.List.Cast<DepositPayment>().ToList().Sum(i => i.AmountReceived));
                            DepositPayment objDepoPay = (DepositPayment)View.CurrentObject;
                            if (objDepoPay != null)
                            {
                                objDepoPay.InvoiceAmuont = objCurrentDeposit.Amount;
                                objDepoPay.SumAmountReceived = SumAmountReceived;
                                objDepoPay.Balance = objDepoPay.InvoiceAmuont - SumAmountReceived;
                                OldAmountReceived = objDepoPay.AmountReceived;
                            }
                        }
                    }
                    View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                    if (popupWindow != null)
                    {
                        DialogController dc = popupWindow.GetController<DialogController>();
                        if (dc != null)
                        {
                            dc.AcceptAction.Execute += AcceptAction_Execute;
                            dc.AcceptAction.Executed += AcceptAction_Executed;
                        }
                    }
                }
                else if (View.Id == "Deposits_DepositPayments_ListView")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executed += DeleteAction_Executed;
                }
                else if (View.Id == "InvoicingContact_LookupListView_DepositNotes")
                {
                    Deposits objCurrentDeposit = (Deposits)Application.MainWindow.View.CurrentObject;
                    if (objCurrentDeposit != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Customer.Oid=?", objCurrentDeposit.Client.Oid);
                    }
                }
                else if (View.Id == "Deposits_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executed += SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAction.Executing += SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executed += SaveAndCloseAction_Executed;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }

        private void SaveAndCloseAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                ReceivaleNavigationCount();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAndCloseAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.Id == "Deposits_DetailView" && View.CurrentObject != null)
                {
                    ListPropertyEditor lvDeposit = ((DetailView)Application.MainWindow.View).FindItem("DepositPayments") as ListPropertyEditor;
                    if (lvDeposit != null && lvDeposit.ListView != null)
                    {
                        ASPxGridListEditor gridlisteditor = ((ListView)lvDeposit.ListView).Editor as ASPxGridListEditor;
                        Deposits objDeposit = (Deposits)Application.MainWindow.View.CurrentObject;
                        if (gridlisteditor != null && gridlisteditor.Grid != null && objDeposit.Status == DepositStatus.Paid)
                        {
                            Session currentSession = ((XPObjectSpace)lvDeposit.ListView.ObjectSpace).Session;
                            UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                            DepositEDDExport depositEDDExport = uow.FindObject<DepositEDDExport>(CriteriaOperator.Parse("[DepositID]=?", objDeposit.Oid));
                            if (depositEDDExport == null)
                            {
                                MemoryStream ms = new MemoryStream();
                                ASPxGridView gridView = gridlisteditor.Grid;
                                gridView.TotalSummary.Clear();
                                gridView.ExportToCsv(ms);
                                DepositEDDExport newDepEDDExp = new DepositEDDExport(uow);
                                newDepEDDExp.InvoiceID = uow.GetObjectByKey<Invoicing>(objDeposit.InvoiceID.Oid);
                                newDepEDDExp.DepositID = uow.GetObjectByKey<Deposits>(objDeposit.Oid);
                                newDepEDDExp.EDDDetail = ms.ToArray();
                                newDepEDDExp.Save();

                            }
                            else
                            {
                                MemoryStream ms = new MemoryStream();
                                ASPxGridView gridView = gridlisteditor.Grid;
                                gridView.TotalSummary.Clear();
                                gridView.ExportToCsv(ms);
                                depositEDDExport.EDDDetail = ms.ToArray();
                                depositEDDExport.Save();
                            }
                            uow.CommitChanges();
                        }
                    }
                    ////Deposits objDeposits = (Deposits)View.CurrentObject;
                    ////if (objDeposits != null && objDeposits.Client != null && objDeposits.CreditRating != null && objDeposits.Client.CreditRating == null || objDeposits.Client.CreditRating != objDeposits.CreditRating)
                    ////{
                    ////    Customer objCustomer = View.ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[Oid]=?", objDeposits.Client.Oid));
                    ////    if (objCustomer != null)
                    ////    {
                    ////        objCustomer.CreditRating = objDeposits.CreditRating;
                    ////        //View.ObjectSpace.CommitChanges();
                    ////    }
                    ////}

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                ////if (View.Id == "Deposits_DetailView" && View.CurrentObject != null)
                ////{
                ////    Deposits objDeposit = (Deposits)View.CurrentObject;
                ////    if (objDeposit != null && objDeposit.Client != null && objDeposit.CreditRating != null && objDeposit.Client.CreditRating == null || objDeposit.Client.CreditRating != objDeposit.CreditRating)
                ////    {
                ////        Customer objCustomer = View.ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("[Oid]=?", objDeposit.Client.Oid));
                ////        if (objCustomer != null)
                ////        {
                ////            objCustomer.CreditRating = objDeposit.CreditRating;
                ////            //View.ObjectSpace.CommitChanges();
                ////        }
                ////    }

                ////}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View.Id == "Deposits_DetailView")
                {
                    ListPropertyEditor lvDeposit = ((DetailView)Application.MainWindow.View).FindItem("DepositPayments") as ListPropertyEditor;
                    if (lvDeposit != null && lvDeposit.ListView != null)
                    {
                        ASPxGridListEditor gridlisteditor = ((ListView)lvDeposit.ListView).Editor as ASPxGridListEditor;
                        Deposits objDeposit = (Deposits)Application.MainWindow.View.CurrentObject;
                        if (gridlisteditor != null && gridlisteditor.Grid != null && objDeposit.Status == DepositStatus.Paid)
                        {
                            Session currentSession = ((XPObjectSpace)lvDeposit.ListView.ObjectSpace).Session;
                            UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                            DepositEDDExport depositEDDExport = uow.FindObject<DepositEDDExport>(CriteriaOperator.Parse("[DepositID]=?", objDeposit.Oid));
                            if (depositEDDExport == null)
                            {
                                MemoryStream ms = new MemoryStream();
                                ASPxGridView gridView = gridlisteditor.Grid;
                                gridView.TotalSummary.Clear();
                                gridView.ExportToXlsx(ms);
                                DepositEDDExport newDepEDDExp = new DepositEDDExport(uow);
                                newDepEDDExp.InvoiceID = uow.GetObjectByKey<Invoicing>(objDeposit.InvoiceID.Oid);
                                newDepEDDExp.DepositID = uow.GetObjectByKey<Deposits>(objDeposit.Oid);
                                newDepEDDExp.EDDDetail = ms.ToArray();
                                newDepEDDExp.Save();

                            }
                            else
                            {
                                MemoryStream ms = new MemoryStream();
                                ASPxGridView gridView = gridlisteditor.Grid;
                                gridView.TotalSummary.Clear();
                                gridView.ExportToXlsx(ms);
                                depositEDDExport.EDDDetail = ms.ToArray();
                                depositEDDExport.Save();
                            }
                            uow.CommitChanges();
                            ReceivaleNavigationCount();
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

        private void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                ListPropertyEditor lvDeposit = ((DetailView)Application.MainWindow.View).FindItem("DepositPayments") as ListPropertyEditor;
                Deposits objDeposit = (Deposits)Application.MainWindow.View.CurrentObject;
                if (lvDeposit != null && lvDeposit.ListView != null)
                {
                    decimal sumOfAmount = 0;
                    foreach (DepositPayment objDepositPay in ((ListView)lvDeposit.ListView).CollectionSource.List.Cast<DepositPayment>().OrderBy(i => i.Date).ToList())
                    {
                        sumOfAmount = sumOfAmount + objDepositPay.AmountReceived;
                        objDepositPay.SumAmountReceived = sumOfAmount;
                        objDepositPay.Balance = objDepositPay.InvoiceAmuont - sumOfAmount;
                    }
                    DepositPayment objDepoPay = ((ListView)lvDeposit.ListView).CollectionSource.List.Cast<DepositPayment>().FirstOrDefault(i => i.SumAmountReceived == ((ListView)lvDeposit.ListView).CollectionSource.List.Cast<DepositPayment>().Max(a => a.SumAmountReceived));
                    if (objDepoPay != null)
                    {
                        if (objDeposit != null)
                        {
                            objDeposit.AmountPaid = objDepoPay.SumAmountReceived;
                            objDeposit.Balance = objDepoPay.Balance;
                        }
                        if (objDepoPay.InvoiceAmuont == objDepoPay.SumAmountReceived || objDepoPay.InvoiceAmuont < objDepoPay.SumAmountReceived)
                        {
                            objDeposit.Status = DepositStatus.Paid;
                        }
                        else if (objDepoPay.SumAmountReceived == 0)
                        {
                            objDeposit.Status = DepositStatus.Unpaid;
                        }
                        else if (objDepoPay.InvoiceAmuont > objDepoPay.SumAmountReceived)
                        {
                            objDeposit.Status = DepositStatus.PartiallyPaid;
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

        private void DeleteAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Deposits_DepositPayments_ListView")
                {
                    decimal sumOfAmount = 0;
                    foreach (DepositPayment objDepositPay in ((ListView)View).CollectionSource.List.Cast<DepositPayment>().OrderBy(i => i.Date).ToList())
                    {
                        sumOfAmount = sumOfAmount + objDepositPay.AmountReceived;
                        objDepositPay.SumAmountReceived = sumOfAmount;
                        objDepositPay.Balance = objDepositPay.InvoiceAmuont - sumOfAmount;
                    }
                    View.ObjectSpace.CommitChanges();
                    View.ObjectSpace.Refresh();
                    DepositPayment objDepoPay = ((ListView)View).CollectionSource.List.Cast<DepositPayment>().FirstOrDefault(i => i.SumAmountReceived == ((ListView)View).CollectionSource.List.Cast<DepositPayment>().Max(a => a.SumAmountReceived));
                    if (objDepoPay != null)
                    {
                        Deposits objDeposit = (Deposits)Application.MainWindow.View.CurrentObject;
                        if (objDeposit != null)
                        {
                            objDeposit.AmountPaid = objDepoPay.SumAmountReceived;
                            objDeposit.Balance = objDepoPay.Balance;
                        }
                    }
                    else
                    {
                        Deposits objDeposit = (Deposits)Application.MainWindow.View.CurrentObject;
                        if (objDeposit != null)
                        {
                            objDeposit.AmountPaid = 0;
                            objDeposit.Balance = 0;
                            objDeposit.Status = DepositStatus.Unpaid;
                        }
                    }
                    Application.MainWindow.View.ObjectSpace.CommitChanges();
                    Application.MainWindow.View.ObjectSpace.Refresh();
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
                //DepositPayment objDepositPayment = (DepositPayment)e.CurrentObject;
                //Deposits objDeposit = (Deposits)Application.MainWindow.View.CurrentObject;
                //if (objDepositPayment!=null)
                //{
                //    decimal sumOfAmount = 0;
                //    foreach (DepositPayment objDepositPay in ((ListView)View).CollectionSource.List.Cast<DepositPayment>().OrderBy(i => i.Date).ToList())
                //    {
                //        sumOfAmount = sumOfAmount + objDepositPay.AmountReceived;
                //        objDepositPay.SumAmountReceived = sumOfAmount;
                //        objDepositPay.Balance = objDepositPay.InvoiceAmuont - sumOfAmount;
                //    }
                //    DepositPayment objDepoPay = ((ListView)View).CollectionSource.List.Cast<DepositPayment>().FirstOrDefault(i => i.SumAmountReceived == ((ListView)View).CollectionSource.List.Cast<DepositPayment>().Max(a => a.SumAmountReceived));
                //    if (objDepoPay != null)
                //    {
                //        if (objDeposit != null)
                //        {
                //            objDeposit.AmountPaid = objDepoPay.SumAmountReceived;
                //            objDeposit.Balance = objDepoPay.Balance;
                //        }
                //        if (objDepoPay.InvoiceAmuont == objDepoPay.SumAmountReceived)
                //        {
                //            objDeposit.Status = DepositStatus.Paid;
                //        }
                //        else if (objDepoPay.SumAmountReceived == 0)
                //        {
                //            objDeposit.Status = DepositStatus.Unpaid;
                //        }
                //        else if (objDepoPay.InvoiceAmuont > objDepoPay.SumAmountReceived)
                //        {
                //            objDeposit.Status = DepositStatus.PartiallyPaid;
                //        }
                //    }


                //}
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
                if (View.Id == "DepositPayment_DetailView" && e.PropertyName == "AmountReceived" && e.NewValue != e.OldValue)
                {
                    DepositPayment depositPayment = (DepositPayment)e.Object;
                    ListPropertyEditor lvDeposit = ((DetailView)Application.MainWindow.View).FindItem("DepositPayments") as ListPropertyEditor;
                    if (depositPayment != null && lvDeposit != null && lvDeposit.ListView != null)
                    {
                        decimal totalAmount;
                        decimal SumAmountReceived = Convert.ToDecimal(((ListView)lvDeposit.ListView).CollectionSource.List.Cast<DepositPayment>().ToList().Where(i => i.Oid != depositPayment.Oid).Sum(i => i.AmountReceived));
                        totalAmount = SumAmountReceived + depositPayment.AmountReceived;
                        if (totalAmount > depositPayment.InvoiceAmuont)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "receivedamountnotequalinvoiceamount"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            depositPayment.AmountReceived = OldAmountReceived;
                            return;
                        }
                        depositPayment.SumAmountReceived = SumAmountReceived + depositPayment.AmountReceived;
                        depositPayment.Balance = depositPayment.InvoiceAmuont - depositPayment.SumAmountReceived;
                        Deposits objDeposit = (Deposits)Application.MainWindow.View.CurrentObject;
                        if (objDeposit != null)
                        {
                            objDeposit.AmountPaid = depositPayment.SumAmountReceived;
                            objDeposit.Balance = depositPayment.Balance;
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
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            try
            {
                if (View.Id == "DepositPayment_DetailView")
                {
                    View.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                }
                else if (View.Id == "Deposits_DepositPayments_ListView")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executed -= DeleteAction_Executed;
                }
                else if (View.Id == "Deposits_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executed -= SaveAction_Executed;
                    Frame.GetController<ModificationsController>().SaveAction.Executing -= SaveAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Executed -= SaveAndCloseAction_Executed;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void HistoryDeposit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Deposits));
                ListView createListview = Application.CreateListView("Deposits_ListView_History", cs, true);
                Frame.SetView(createListview);

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Rollback_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(Deposits));
                    Deposits obj = os.CreateObject<Deposits>();
                    DetailView createdView = Application.CreateDetailView(os, "Deposits_DetailView_Rollback", true, obj);
                    createdView.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += RollBackReason_Accepting;
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

        private void RollBackReason_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                Deposits objCurrentObject = (Deposits)e.AcceptActionArgs.CurrentObject;
                if (objCurrentObject != null && !string.IsNullOrEmpty(objCurrentObject.RollbackReason) && !string.IsNullOrWhiteSpace(objCurrentObject.RollbackReason))
                {
                    foreach (Deposits objDeposit in View.SelectedObjects)
                    {
                        foreach (DepositPayment objDepPay in objDeposit.DepositPayments.ToList())
                        {
                            View.ObjectSpace.Delete(objDepPay);
                        }
                        foreach (Notes objNotes in objDeposit.Note.ToList())
                        {
                            View.ObjectSpace.Delete(objNotes);
                        }
                        objDeposit.Balance = 0;
                        objDeposit.AmountPaid = 0;
                        objDeposit.Status = DepositStatus.Unpaid;
                        objDeposit.RollbackReason = objCurrentObject.RollbackReason;
                        objDeposit.RollbackedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        objDeposit.RollbackedDate = DateTime.Now;
                        DepositEDDExport objOldDepositEdd = View.ObjectSpace.FindObject<DepositEDDExport>(CriteriaOperator.Parse("[DepositID]=?", objDeposit.Oid));
                        if (objOldDepositEdd != null)
                        {
                            View.ObjectSpace.Delete(objOldDepositEdd);
                        }
                    }
                    View.ObjectSpace.CommitChanges();
                    View.ObjectSpace.Refresh();
                    ReceivaleNavigationCount();
                    if (View is DetailView)
                    {
                        IObjectSpace objspace = Application.CreateObjectSpace();
                        CollectionSource cs = new CollectionSource(objspace, typeof(Deposits));
                        ListView createListview = Application.CreateListView("Deposits_ListView_History", cs, true);
                        Frame.SetView(createListview);
                    }
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreason"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void EDDDetail_Excecute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null)
                {
                    DepositEDDExport objExport = (DepositEDDExport)View.CurrentObject;
                    //if (objExport != null)
                    //{
                    //    HttpContext.Current.Response.Clear();
                    //    HttpContext.Current.Response.ContentType = "application/excel";
                    //    HttpContext.Current.Response.AddHeader("Content-disposition", "filename=DepositEDDDetails.CSV");
                    //    HttpContext.Current.Response.OutputStream.Write(objExport.EDDDetail, 0, objExport.EDDDetail.Length);
                    //    HttpContext.Current.Response.OutputStream.Flush();
                    //    HttpContext.Current.Response.OutputStream.Close();
                    //    HttpContext.Current.Response.Flush();
                    //    HttpContext.Current.Response.Close();
                    //}
                    string strEDDId = string.Empty;
                    strEDDId = string.Empty;
                    if (objExport != null)
                    {
                        string strTempPath = Path.GetTempPath();
                        String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                        }
                        string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\DepositEDD" + timeStamp + ".xlsx");
                        //var workbook = new ExcelFile();
                        //var worksheet = workbook.Worksheets.Add("DataTable to Sheet");
                        DataView dv = new DataView();
                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("Name");
                        dataTable.Columns.Add("InvoiceAmuont");
                        dataTable.Columns.Add("AmountReceived");
                        dataTable.Columns.Add("SumAmountReceived");
                        dataTable.Columns.Add("Balance");
                        dataTable.Columns.Add("Date");
                        dataTable.Columns.Add("Region");
                        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                        string serverName = connectionStringBuilder.DataSource.Trim();
                        string databaseName = connectionStringBuilder.InitialCatalog.Trim();
                        string userID = connectionStringBuilder.UserID.Trim();
                        string password = connectionStringBuilder.Password.Trim();
                        string sqlSelect = "Select * from DepositEDDExport_View where [Deposits] in('" + objExport.DepositID.Oid + "')";
                        SqlConnection sqlConnection = new SqlConnection(connectionStringBuilder.ToString());
                        SqlCommand sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                        SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand);
                        sqlDa.Fill(dataTable);
                        dataTable.Columns.Remove("Deposits");
                        Workbook wb = new Workbook();
                        Worksheet worksheet0 = wb.Worksheets[0];
                        worksheet0.Name = "data";
                        wb.Worksheets[0].Import(dataTable, true, 0, 0);
                        wb.SaveDocument(strExportedPath);
                        string[] path = strExportedPath.Split('\\');
                        int arrcount = path.Count();
                        int sc = arrcount - 2;
                        string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                        WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));

                        //var fileBytes = Encoding.UTF8.GetBytes(dataTable.ToString());
                        //HttpContext.Current.Response.Clear();
                        //HttpContext.Current.Response.Buffer = true;
                        //HttpContext.Current.Response.ContentType = "text/csv";
                        //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=data.csv");
                        //HttpContext.Current.Response.BinaryWrite(fileBytes);
                        //HttpContext.Current.Response.Flush();
                        //HttpContext.Current.Response.Close();
                        //dataTable.Exp
                        //Workbook wb =  Workbook.();
                        //Worksheet ws = wb.Worksheets[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ReceivaleNavigationCount()
        {
            ShowNavigationItemController ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
            ChoiceActionItem Accounting = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "Accounting");
            if (Accounting != null)
            {
                ChoiceActionItem Receivables = Accounting.Items.FirstOrDefault(i => i.Id == "Receivables");
                if (Receivables != null)
                {
                    ChoiceActionItem childDeposit = Receivables.Items.FirstOrDefault(i => i.Id == "Deposit");
                    if (childDeposit != null)
                    {
                        int count = 0;
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        IList<Deposits> lstCount = objSpace.GetObjects<Deposits>(CriteriaOperator.Parse("[Status] = 'Unpaid' Or [Status] = 'PartiallyPaid'"));
                        var cap = childDeposit.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        if (lstCount.Count > 0)
                        {
                            count = lstCount.Count();
                            childDeposit.Caption = cap[0] + " (" + count + ")";
                        }
                        else
                        {
                            childDeposit.Caption = cap[0];
                        }
                    }
                    ChoiceActionItem childDepositEDDExport = Receivables.Items.FirstOrDefault(i => i.Id == "DepositEDDExport");
                    if (childDepositEDDExport != null)
                    {
                        int count = 0;
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        IList<DepositEDDExport> lstCount = objSpace.GetObjects<DepositEDDExport>(CriteriaOperator.Parse("[Oid] Is Not Null"));
                        var cap = childDepositEDDExport.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        if (lstCount.Count > 0)
                        {
                            count = lstCount.Count();
                            childDepositEDDExport.Caption = cap[0] + " (" + count + ")";
                        }
                        else
                        {
                            childDepositEDDExport.Caption = cap[0];
                        }
                    }
                }

            }
        }
    }
}
