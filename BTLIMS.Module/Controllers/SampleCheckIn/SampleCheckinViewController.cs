using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraGrid.Views.Grid;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.Quotes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BTLIMS.Module.Controllers.SampleCheckIn
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SampleCheckinUploadImageController : ViewController
    {

        #region Declaration
        MessageTimer timer = new MessageTimer();
        ModificationsController modificationController;
        DeleteObjectsViewController DeleteController;
        bool boledit = false;
        bool bolRefresh = false;
        SampleCheckInInfo objSCInfo = new SampleCheckInInfo();
        TestMethodInfo objInfo = new TestMethodInfo();
        #endregion

        #region Constructor
        public SampleCheckinUploadImageController()
        {
            InitializeComponent();
            this.TargetViewId = "Samplecheckin_DetailView;" + "Image_LookupListView;" + "Samplecheckin_DetailView_Copy;" + "Testparameter_LookupListView_Copy_GroupSampleLogin;" + "Samplecheckin_ListView_Copy;" + "Samplecheckin_ListView;" + "Samplecheckin_DetailView_Copy_SampleRegistration;" + "Samplecheckin_DetailView_Copy_RegistrationSigningOff;"
                + "Samplecheckin_SCItemCharges_ListView;" + "ItemChargePricing_LookupListView_Samplecheckin;" + "Collector_DetailView_SampleCheckIn;";
            AddSCItemCharge.TargetViewId = "Samplecheckin_SCItemCharges_ListView;";
            RemoveSCItemCharge.TargetViewId = "Samplecheckin_SCItemCharges_ListView;";
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                //ObjectSpace.Committed += ObjectSpace_Committed;
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                modificationController = Frame.GetController<ModificationsController>();
                //DeleteController = Frame.GetController<DeleteObjectsViewController>();
                if (modificationController != null)
                {
                    modificationController.SaveAction.Execute += SaveAction_Execute;
                    modificationController.SaveAndCloseAction.Execute += SaveAndCloseAction_Execute;
                    modificationController.SaveAndNewAction.Execute += SaveAndNewAction_Execute;
                }
                if (View != null && View.Id == "Samplecheckin_DetailView_Copy")
                {
                    objSCInfo.SCVisualMatrixName = string.Empty;
                }
                else if (View.Id == "ItemChargePricing_LookupListView_Samplecheckin")
                {
                    View.ControlsCreated += View_ControlsCreated;
                }
                else if (View.Id == "Collector_DetailView_SampleCheckIn")
                {
                    if (Application.MainWindow.View.ObjectTypeInfo.Type == typeof(Samplecheckin))
                    {
                        Samplecheckin samplecheckin = (Samplecheckin)Application.MainWindow.View.CurrentObject;
                        if (samplecheckin != null && samplecheckin.ClientName != null)
                        {
                            Collector objCillector = (Collector)View.CurrentObject;
                            objCillector.CustomerName = View.ObjectSpace.GetObjectByKey<Customer>(samplecheckin.ClientName.Oid);
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
            try
            {
                if (View.Id == "ItemChargePricing_LookupListView_Samplecheckin")
                {
                    ListPropertyEditor liItemCharges = ((DetailView)Application.MainWindow.View).FindItem("SCItemCharges") as ListPropertyEditor;
                    if (liItemCharges != null && liItemCharges.ListView != null)
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            foreach (SampleCheckinItemChargePricing obj in ((ListView)liItemCharges.ListView).CollectionSource.List.Cast<SampleCheckinItemChargePricing>().ToList())
                            {
                                gridListEditor.Grid.Selection.SelectRowByKey(obj.ItemPrice.Oid);

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
            base.OnViewControlsCreated();
            try
            {

                if (View.Id == "Samplecheckin_SCItemCharges_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = DevExpress.Web.GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
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
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            window.setTimeout(function() { 
                            var fieldName = sessionStorage.getItem('PrevFocusedColumn');
                            if(s.batchEditApi.HasChanges(e.visibleIndex) && (fieldName == 'UnitPrice' || fieldName== 'Qty' || fieldName=='Discount'))
                            {
                               var unitprice = s.batchEditApi.GetCellValue(e.visibleIndex, 'UnitPrice');
                               var amount = s.batchEditApi.GetCellValue(e.visibleIndex, 'Amount');
                               var npunitprice = s.batchEditApi.GetCellValue(e.visibleIndex, 'NpUnitPrice');
                               var discount = s.batchEditApi.GetCellValue(e.visibleIndex, 'Discount');  
                               var qty = s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty');
                               console.log(unitprice);
                               console.log(discount);
                                var totalamount=0
                               if(discount>0)
                               {
                                 var discutamt = unitprice * (discount / 100);
                                 totalamount=qty * (unitprice - discutamt);
                               }
                               else if(discount<0)
                               {
                                var discoamt = unitprice * (discount / 100);
                                totalamount= ((unitprice) - (discoamt)) * qty;
                               }
                               else
                               {
                                 totalamount=qty * unitprice ;
                               }
                               var amount=unitprice * qty;
                               s.batchEditApi.SetCellValue(e.visibleIndex, 'FinalAmount',Math.round(totalamount)); 
                               s.batchEditApi.SetCellValue(e.visibleIndex, 'Amount',amount);
                               
                               //var discount=((npunitprice - unitprice) / npunitprice) * 100 ;
                               //s.batchEditApi.SetCellValue(e.visibleIndex, 'FinalAmount',Math.round(finalamt)); 
                               //s.batchEditApi.SetCellValue(e.visibleIndex, 'Discount',Math.round(discount));
                              
                           }

                          else if (s.batchEditApi.HasChanges(e.visibleIndex) && (fieldName=='FinalAmount'))
                          {
                                  var finalAmount = s.batchEditApi.GetCellValue(e.visibleIndex,'FinalAmount');
                                  var unitPrice = s.batchEditApi.GetCellValue(e.visibleIndex,'Amount');
                                  var qty = s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty');
                                  if(finalAmount < unitPrice)
                                  {         
                                     var discount= (finalAmount / unitPrice)* 100;                                          
                                     var discAmount= Math.round(100 - discount);
                                  }
                                  else if (finalAmount > unitPrice)
                                  {    
                                      var discount= (unitPrice / finalAmount ) * 100; 
                                      var discAmount= Math.round(discount - 100);
                                  }
                                 s.batchEditApi.SetCellValue(e.visibleIndex, 'Discount',discAmount);

                          }
                           
                            }, 20); }";
                    if (gridListEditor.Grid.Columns["Qty"] != null)
                    {
                        gridListEditor.Grid.Columns["Qty"].Width = 40;
                    }
                    if (gridListEditor.Grid.Columns["NpUnitPrice"] != null)
                    {
                        gridListEditor.Grid.Columns["NpUnitPrice"].Width = 0;
                    }
                    //if (gridListEditor.Grid.Columns["Amount"] != null)
                    //{
                    //    gridListEditor.Grid.Columns["Amount"].Width = 0;
                    //}
                }
                if (View.Id == "Samplecheckin_SCItemCharges_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        gridListEditor.Grid.SettingsBehavior.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
                        gridListEditor.Grid.CustomColumnSort += Grid_CustomColumnSort;
                    }
                }
                else if (View.Id == "ItemChargePricing_LookupListView_Samplecheckin")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        gridListEditor.Grid.SettingsBehavior.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
                        gridListEditor.Grid.CustomColumnSort += Grid_CustomColumnSort;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_CustomColumnSort(object sender, DevExpress.Web.CustomColumnSortEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (View.Id == "ItemChargePricing_LookupListView_Samplecheckin")
                {
                    if (e.Column != null & e.Column.FieldName == "ItemCode")
                    {
                        object SampleNo1 = e.GetRow1Value("ItemCode");
                        object SampleNo2 = e.GetRow2Value("ItemCode");
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
                else if (View.Id == "Samplecheckin_SCItemCharges_ListView")
                {
                    if (e.Column != null & e.Column.FieldName == "ItemCode")
                    {
                        object SampleNo1 = e.GetRow1Value("ItemPrice.ItemCode");
                        object SampleNo2 = e.GetRow2Value("ItemPrice.ItemCode");
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
                if (View != null && View.Id == "Samplecheckin_DetailView_Copy")
                {
                    objSCInfo.SCVisualMatrixName = string.Empty;
                }
                //ObjectSpace.Committed -= ObjectSpace_Committed;
                ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                if (modificationController != null)
                {
                    modificationController.SaveAction.Execute -= SaveAction_Execute;
                    modificationController.SaveAndCloseAction.Execute -= SaveAndCloseAction_Execute;
                    modificationController.SaveAndNewAction.Execute -= SaveAndNewAction_Execute;
                }
                else if (View.Id == "ItemChargePricing_LookupListView_Samplecheckin")
                {
                    View.ControlsCreated -= View_ControlsCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region ActionEvents
        private void SaveAndNewAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Samplecheckin_DetailView_Copy" && boledit == false)
                {
                    if (View.AllowEdit == false)
                    {
                        MessageOptions options = new MessageOptions();
                        options.Duration = 5000;
                        options.Message = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "alreadysave");
                        options.Type = InformationType.Success;
                        options.Web.Position = InformationPosition.Top;
                        Application.ShowViewStrategy.ShowMessage(options);
                    }
                    if (boledit == true)
                    {
                        boledit = false;
                        MessageOptions options = new MessageOptions();
                        options.Duration = 5000;
                        options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                        options.Type = InformationType.Success;
                        options.Web.Position = InformationPosition.Top;
                        Application.ShowViewStrategy.ShowMessage(options);
                    }
                }
                else if (View.Id == "Samplecheckin_DetailView_Copy_SampleRegistration" ||View.Id == "Samplecheckin_ListView_Copy_Registration" || View.Id == "Samplecheckin_DetailView_Copy_RegistrationSigningOff")
                {

                    MessageOptions options = new MessageOptions();
                    options.Duration = 1000;
                    options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                    options.Type = InformationType.Success;
                    options.Web.Position = InformationPosition.Top;
                    Application.ShowViewStrategy.ShowMessage(options);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAndCloseAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Samplecheckin_DetailView_Copy")
                {
                    if (View.AllowEdit == false && boledit == false)
                    {
                        MessageOptions options = new MessageOptions();
                        options.Duration = 5000;
                        options.Message = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "alreadysave");
                        options.Type = InformationType.Success;
                        options.Web.Position = InformationPosition.Top;
                        Application.ShowViewStrategy.ShowMessage(options);
                    }
                    if (boledit == true)
                    {
                        boledit = false;
                        MessageOptions options = new MessageOptions();
                        options.Duration = 5000;
                        options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                        options.Type = InformationType.Success;
                        options.Web.Position = InformationPosition.Top;
                        Application.ShowViewStrategy.ShowMessage(options);
                    }
                }
                else if (View.Id == "Samplecheckin_DetailView_Copy_SampleRegistration" || View.Id == "Samplecheckin_DetailView_Copy_RegistrationSigningOff")
                {

                    MessageOptions options = new MessageOptions();
                    options.Duration = 1000;
                    options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                    options.Type = InformationType.Success;
                    options.Web.Position = InformationPosition.Top;
                    Application.ShowViewStrategy.ShowMessage(options);
                }
                else if (View.Id == "Samplecheckin_ListView_Copy_Registration")
                {
                    MessageOptions options = new MessageOptions();
                    options.Duration = 1000;
                    options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                    options.Type = InformationType.Success;
                    options.Web.Position = InformationPosition.Top;
                    Application.ShowViewStrategy.ShowMessage(options);
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
                if (View != null && View.Id == "Samplecheckin_DetailView_Copy")
                {
                    modificationController.SaveAndCloseAction.Active.SetItemValue("", false);
                    modificationController.SaveAndNewAction.Active.SetItemValue("", false);
                    if (boledit == true)
                    {
                        boledit = false;

                        MessageOptions options = new MessageOptions();
                        options.Duration = 1000;
                        options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                        options.Type = InformationType.Success;
                        options.Web.Position = InformationPosition.Top;
                        Application.ShowViewStrategy.ShowMessage(options);
                    }
                }
                else if (View.Id == "Samplecheckin_DetailView_Copy_SampleRegistration" || View.Id == "Samplecheckin_DetailView_Copy_RegistrationSigningOff")
                {

                    MessageOptions options = new MessageOptions();
                    options.Duration = 1000;
                    options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                    options.Type = InformationType.Success;
                    options.Web.Position = InformationPosition.Top;
                    Application.ShowViewStrategy.ShowMessage(options);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events
        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            try
            {
                if (View != null)
                {
                    Session currentsession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    SelectedData sproc = currentsession.ExecuteSproc("DeleteUnUsedImages");
                }
                if (View != null && View.Id == "Samplecheckin_DetailView_Copy" && View.AllowEdit == true)
                {

                    Session currentsession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    Samplecheckin objSamplecheckin = (Samplecheckin)View.CurrentObject;
                    double seconds = objSamplecheckin.CollectionTime.TotalSeconds;
                    DateTime? dtCollectTime = null;
                    Guid? guCollector = null;
                    SelectedData sproc = currentsession.ExecuteSproc("InsertGroupSamplelogin", new OperandValue(objSamplecheckin.JobID), new OperandValue(objSamplecheckin.ClientSampleID != null ? objSamplecheckin.ClientSampleID : null), new OperandValue(objSamplecheckin.CollectionDate != DateTime.MinValue ? objSamplecheckin.CollectionDate : dtCollectTime),
                        new OperandValue(seconds), new OperandValue(objSamplecheckin.Collector != null ? objSamplecheckin.Collector.Oid : guCollector), new OperandValue(objSamplecheckin.SampleLocation != null ? objSamplecheckin.SampleLocation : null));
                    boledit = true;
                    View.AllowEdit["Forced"] = false;

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        private void SampleCheckinUploadImageController_ViewControlsCreated(object sender, EventArgs e)
        {
            try
            {
                if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Testparameter) && View.Id == "Testparameter_LookupListView_Copy_GroupSampleLogin")
                {
                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("'" + objSCInfo.SCVisualMatrixName + "'==[TestMethod.MatrixName.MatrixName] AND [Parameter.Surroagate]=False");
                }
                //else if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Samplecheckin) && View.Id == "Samplecheckin_ListView")
                //{
                //    //Guid company=((Employee)SecuritySystem.CurrentUser).UserName;
                //    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Employee)))
                //    {
                //        lstview.Criteria = CriteriaOperator.Parse("[Company.Oid]='" + ((Employee)SecuritySystem.CurrentUser).Company.Oid +"'");
                //        lstview.Properties.Add(new ViewProperty("Oid", SortDirection.Ascending, "Oid", true, true));
                //        List<object> UserName = new List<object>();
                //        foreach (ViewRecord rec in lstview)
                //            UserName.Add(rec["Oid"]);
                //        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("ModifiedBy", UserName);
                //    }
                //    //Samplecheckin obj = new Samplecheckin();
                //    //((Employee)obj.ModifiedBy.UserName).Company.Oid;
                //    //((Employee)ModifiedBy.UserName).Company.Oid
                //    //((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(+((Employee) + "ModifiedBy.UserName" + ).Company.Oid + "'== " + company + "'" );
                //}
                else if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Samplecheckin) && View.Id == "Samplecheckin_ListView_Copy")
                {
                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[GroupVisualMatrix] IS NOT NULL");
                }
                else if (View is DevExpress.ExpressApp.DetailView && View.ObjectTypeInfo.Type == typeof(Samplecheckin) && View.Id == "Samplecheckin_DetailView_Copy")
                {
                    Samplecheckin sc = (Samplecheckin)View.CurrentObject;

                    if (sc != null && sc.JobID == null)
                    {
                        View.AllowEdit["Forced"] = true;
                    }
                    else
                    {
                        View.AllowEdit["Forced"] = false;
                    }
                }
                if (View != null && View.Id == "Samplecheckin_DetailView")
                {
                    if (View.CurrentObject != null)
                    {
                        Samplecheckin obj = (Samplecheckin)View.CurrentObject;
                        if (obj.ClientName != null)
                        {
                            objInfo.ClientName = obj.ClientName.CustomerName;
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
        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (View != null && View.CurrentObject == e.Object && e.PropertyName == "GroupVisualMatrix")
            {
                if (View.ObjectTypeInfo.Type == typeof(Samplecheckin))
                {
                    Samplecheckin objSampleLogIn = (Samplecheckin)e.Object;
                    if (objSampleLogIn.GroupVisualMatrix != null)
                    {
                        if (CheckIFExists(objSampleLogIn))
                        {
                            var objectSpace = Application.CreateObjectSpace();
                            var objLogin = objectSpace.FindObject<SampleLogIn>(CriteriaOperator.Parse("Oid = ?", objSampleLogIn.Oid));
                            if (objLogin == null)
                            {
                                for (int i = objSampleLogIn.Testparameters.Count - 1; i >= 0; i--)
                                {
                                    objSampleLogIn.Testparameters.Remove(objSampleLogIn.Testparameters[i]);
                                    bolRefresh = true;
                                }
                            }
                            else
                            {
                                foreach (Testparameter objTestParam in objSampleLogIn.Testparameters)
                                {
                                    var osTestParam = Application.CreateObjectSpace();
                                    IList<SampleParameter> lstSampleParam = (IList<SampleParameter>)objTestParam.SampleParameter;
                                    {
                                        foreach (var li in lstSampleParam)
                                        {
                                            if (objTestParam == li.Testparameter)
                                            {
                                                if (objSampleLogIn.Oid.ToString() == li.Samplelogin.Oid.ToString())
                                                {
                                                    if (li.Result != null && li.Result != string.Empty)
                                                    // if (li.Result != null)
                                                    {
                                                        throw new UserFriendlyException(string.Format("Result Already Entered Cannot allow to change the Sample Matrix"));
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
                            objSCInfo.SCVisualMatrixName = objSampleLogIn.GroupVisualMatrix.MatrixName.MatrixName;
                            if (bolRefresh == true)
                            {
                                View.Refresh();
                                bolRefresh = false;
                            }
                        }
                    }
                    else
                    {
                        objSCInfo.SCVisualMatrixName = string.Empty;
                    }
                }
            }
            if (View.CurrentObject == e.Object && e.PropertyName == "ClientName")
            {
                if (View.ObjectTypeInfo.Type == typeof(Samplecheckin))
                {
                    Samplecheckin objSampleCheckin = (Samplecheckin)e.Object;

                    if (objSampleCheckin.ClientName != null)
                    {
                        objInfo.ClientName = objSampleCheckin.ClientName.CustomerName;

                        if (objSampleCheckin.ClientName.Address != null)
                        {
                            objSampleCheckin.ClientAddress = objSampleCheckin.ClientName.Address.ToString();
                        }
                        if (objSampleCheckin.ClientName.Address1 != null)
                        {
                            objSampleCheckin.ClientAddress2 = objSampleCheckin.ClientName.Address1.ToString();
                        }
                        if (objSampleCheckin.ClientName.OfficePhone != null)
                        {
                            objSampleCheckin.ClientPhone = objSampleCheckin.ClientName.OfficePhone.ToString();
                        }
                    }
                    else
                    {
                        objSampleCheckin.ClientAddress = "";
                        objSampleCheckin.ClientAddress2 = "";
                        objSampleCheckin.ClientPhone = "";
                        objSampleCheckin.ContactName = null;
                    }
                }
            }
            if (View.CurrentObject == e.Object && e.PropertyName == "ProjectID")
            {
                if (View.ObjectTypeInfo.Type == typeof(Samplecheckin))
                {
                    //Samplecheckin objSampleCheckin = (Samplecheckin)e.Object;
                    //if (objSampleCheckin.ProjectID != null)
                    //{
                    //    objSampleCheckin.ProjectName = objSampleCheckin.ProjectID.ProjectName;
                    //    //objSampleCheckin.ProjectLocation = objSampleCheckin.ProjectID.ProjectLocation;
                    //}
                    //else
                    //{
                    //    objSampleCheckin.ProjectName = "";
                    //    objSampleCheckin.ProjectLocation = "";
                    //}
                }
            }

        }
        #endregion

        #region Functions
        private bool CheckIFExists(Samplecheckin objSL)
        {
            try
            {
                if (objSL.Testparameters.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return false;
            }
        }
        #endregion
        private void AddItemCharge_Exceute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_SCItemCharges_ListView")
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(ItemChargePricing));
                    ItemChargePricing objcrtdummy = os.CreateObject<ItemChargePricing>();
                    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(ItemChargePricing));
                    ListView lvparameter = Application.CreateListView("ItemChargePricing_LookupListView_Samplecheckin", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                    showViewParameters.CreatedView = lvparameter;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += DcItemCharge_Accepting;
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

        private void DcItemCharge_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    Samplecheckin objSamplecheckin = (Samplecheckin)Application.MainWindow.View.CurrentObject;
                    foreach (ItemChargePricing obj in e.AcceptActionArgs.SelectedObjects)
                    {
                        if (((ListView)View).CollectionSource.List.Cast<SampleCheckinItemChargePricing>().FirstOrDefault(i => i.ItemPrice.Oid == obj.Oid) == null)
                        {
                            if (objSamplecheckin != null && objSamplecheckin.QuoteID != null)
                            {
                                CRMQuotes objQuote = View.ObjectSpace.GetObjectByKey<CRMQuotes>(objSamplecheckin.QuoteID.Oid);
                                IList<QuotesItemChargePrice> lstquoteItemCharge = View.ObjectSpace.GetObjects<QuotesItemChargePrice>(CriteriaOperator.Parse("CRMQuotes=?", objQuote.Oid));
                                if (objQuote.QuotesItemChargePrice.FirstOrDefault(i => i.ItemPrice.Oid == obj.Oid) != null)
                                {
                                    QuotesItemChargePrice objprice = objQuote.QuotesItemChargePrice.FirstOrDefault(i => i.ItemPrice.Oid == obj.Oid);
                                    SampleCheckinItemChargePricing objNewItem = View.ObjectSpace.CreateObject<SampleCheckinItemChargePricing>();
                                    objNewItem.ItemPrice = View.ObjectSpace.GetObjectByKey<ItemChargePricing>(objprice.ItemPrice.Oid);
                                    objNewItem.Qty = objprice.Qty;
                                    objNewItem.UnitPrice = objprice.UnitPrice;
                                    objNewItem.NpUnitPrice = objprice.NpUnitPrice;
                                    objNewItem.FinalAmount = objprice.FinalAmount;
                                    objNewItem.Amount = objprice.Amount;
                                    objNewItem.Discount = objprice.Discount;
                                    ((ListView)View).CollectionSource.Add(objNewItem);
                                    objSamplecheckin.SCItemCharges.Add(objNewItem);
                                    objNewItem.Description = obj.Description;
                                    objNewItem.Remark = obj.Remark;
                                }
                                else
                                {
                                    SampleCheckinItemChargePricing objNewItem = View.ObjectSpace.CreateObject<SampleCheckinItemChargePricing>();
                                    objNewItem.ItemPrice = View.ObjectSpace.GetObjectByKey<ItemChargePricing>(obj.Oid);
                                    objNewItem.Qty = obj.Qty;
                                    objNewItem.UnitPrice = obj.UnitPrice;
                                    objNewItem.NpUnitPrice = obj.UnitPrice;
                                    objNewItem.FinalAmount = obj.UnitPrice;
                                    objNewItem.Amount = obj.Qty * obj.UnitPrice;
                                    objNewItem.Discount = obj.Discount;
                                    ((ListView)View).CollectionSource.Add(objNewItem);
                                    objSamplecheckin.SCItemCharges.Add(objNewItem);
                                    objNewItem.Description = obj.Description;
                                    objNewItem.Remark = obj.Remark;
                                }


                            }
                            else
                            {
                                SampleCheckinItemChargePricing objNewItems = View.ObjectSpace.CreateObject<SampleCheckinItemChargePricing>();
                                objNewItems.ItemPrice = View.ObjectSpace.GetObjectByKey<ItemChargePricing>(obj.Oid);
                                objNewItems.Qty = obj.Qty;
                                objNewItems.UnitPrice = obj.UnitPrice;
                                objNewItems.NpUnitPrice = obj.UnitPrice;
                                objNewItems.FinalAmount = obj.UnitPrice;
                                objNewItems.Amount = obj.Qty * obj.UnitPrice;
                                objNewItems.Discount = obj.Discount;
                                ((ListView)View).CollectionSource.Add(objNewItems);
                                objSamplecheckin.SCItemCharges.Add(objNewItems);
                                objNewItems.Description = obj.Description;
                                objNewItems.Remark = obj.Remark;
                            }
                        }
                    }
                    ((ListView)View).Refresh();
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
        private void RemoveItemCharge_Exceute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    foreach (SampleCheckinItemChargePricing objItemCharge in View.SelectedObjects)
                    {
                        ((ListView)View).CollectionSource.Remove(objItemCharge);
                    }
                    ((ListView)View).Refresh();
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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
