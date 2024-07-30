using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting.SDMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.SDMS
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ScientificDataTableController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        ScientificDataInfo objSInfo = new ScientificDataInfo();
        string OldColumnName = string.Empty;
        List<Tuple<string, string, ScientificDataAction, bool, int>> columnList;
        bool IsLoad = false;
        public ScientificDataTableController()
        {
            InitializeComponent();
            TargetViewId = "ScientificDataTable;" + "SpreadSheetBuilder_ScientificData_ListView_RawData;" + "SpreadSheetBuilder_ScientificData_ListView_Calibration;";
            AddRawData.TargetViewId = "SpreadSheetBuilder_ScientificData_ListView_RawData;";
            SaveRawData.TargetViewId = "SpreadSheetBuilder_ScientificData_ListView_RawData;";
            AddCalibration.TargetViewId = "SpreadSheetBuilder_ScientificData_ListView_Calibration;";
            DeleteRawData.TargetViewId = "SpreadSheetBuilder_ScientificData_ListView_RawData";
            SaveCalibration.TargetViewId = "SpreadSheetBuilder_ScientificData_ListView_Calibration;";
            DeleteCalibrationData.TargetViewId = "SpreadSheetBuilder_ScientificData_ListView_Calibration";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                // Perform various tasks depending on the target View.
                if (View.Id == "SpreadSheetBuilder_ScientificData_ListView_RawData" || View.Id == "SpreadSheetBuilder_ScientificData_ListView_Calibration")
                {
                    ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
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
                if (View.Id == "SpreadSheetBuilder_ScientificData_ListView_RawData" || View.Id == "SpreadSheetBuilder_ScientificData_ListView_Calibration")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                        editor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        editor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            s.timerHandle = setTimeout(function() {  
                                 if (s.batchEditApi.HasChanges()) {  
                                   s.UpdateEdit();  
                                 } 
                               }, 20);}";
                    }

                    if (View.Id == "SpreadSheetBuilder_ScientificData_ListView_RawData")
                    {
                        if (IsLoad == false)
                        {
                            if (((ListView)View).CollectionSource.List.Count > 0)
                            {
                                columnList = ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_ScientificData>().ToList().Select(x => Tuple.Create(x.FieldName, x.DataType, ScientificDataAction.None, x.SaveInTable, x.uqID)).ToList();
                                //if (columnList != null)
                                //{

                                //}
                            }
                            IsLoad = true;
                        }
                    }
                }
                // Access and customize the target View control.
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
                // Unsubscribe from previously subscribed events and release other references and resources.
                base.OnDeactivated();
                if (View.Id == "SpreadSheetBuilder_ScientificData_ListView_RawData" || View.Id == "SpreadSheetBuilder_ScientificData_ListView_Calibration")
                {
                    ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
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
            try
            {
                if (View != null && View.Id == "SpreadSheetBuilder_ScientificData_ListView_RawData")
                {
                    SpreadSheetBuilder_ScientificData objCurrentData = (SpreadSheetBuilder_ScientificData)e.Object;
                    if (objCurrentData != null)
                    {
                        if (e.PropertyName == "FieldName" && objCurrentData.Action != ScientificDataAction.New && objCurrentData.SaveInTable == true)
                        {
                            OldColumnName = e.OldValue.ToString();
                            objCurrentData.Action = ScientificDataAction.Edit;
                        }

                        if (e.PropertyName == "Action" && objCurrentData.Action == ScientificDataAction.Edit)
                        {
                            if (string.IsNullOrEmpty(objCurrentData.OldColumnName))
                            {
                                objCurrentData.OldColumnName = OldColumnName;
                            }
                        }

                        if (e.PropertyName == "NonPersistantDataType")
                        {
                            objCurrentData.DataType = string.Empty;
                            if (objCurrentData.Action != ScientificDataAction.New)
                            {
                                objCurrentData.Action = ScientificDataAction.Edit;
                            }
                        }

                        if (e.PropertyName == "DataType")
                        {
                            if (objCurrentData.NonPersistantDataType == ScientificDataTypes.bigint)
                            {
                                objCurrentData.DataType = "bigint";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.numeric)
                            {
                                objCurrentData.DataType = "numeric";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.bit)
                            {
                                objCurrentData.DataType = "bit";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.smallint)
                            {
                                objCurrentData.DataType = "smallint";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.decimaltype)
                            {
                                objCurrentData.DataType = "decimal";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.smallmoney)
                            {
                                objCurrentData.DataType = "smallmoney";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.inttype)
                            {
                                objCurrentData.DataType = "int";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.tinyint)
                            {
                                objCurrentData.DataType = "tinyint";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.money)
                            {
                                objCurrentData.DataType = "money";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.floattype)
                            {
                                objCurrentData.DataType = "float";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.real)
                            {

                                objCurrentData.DataType = "real";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.date)
                            {
                                objCurrentData.DataType = "date";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.datetime2)
                            {
                                objCurrentData.DataType = "datetime2";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.datetime)
                            {
                                objCurrentData.DataType = "datetime";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.datetimeoffset)
                            {
                                objCurrentData.DataType = "datetimeoffset";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.smalldatetime)
                            {
                                objCurrentData.DataType = "smalldatetime";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.time)
                            {
                                objCurrentData.DataType = "time";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.chartype)
                            {
                                objCurrentData.DataType = "char";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.varchar10)
                            {
                                objCurrentData.DataType = "varchar(10)";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.varchar50)
                            {
                                objCurrentData.DataType = "varchar(50)";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.varchar100)
                            {
                                objCurrentData.DataType = "varchar(100)";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.text)
                            {
                                objCurrentData.DataType = "text";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.nchar)
                            {
                                objCurrentData.DataType = "nchar";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.nvarchar10)
                            {
                                objCurrentData.DataType = "nvarchar(10)";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.nvarchar50)
                            {
                                objCurrentData.DataType = "nvarchar(50)";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.nvarchar100)
                            {
                                objCurrentData.DataType = "nvarchar(100)";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.ntext)
                            {
                                objCurrentData.DataType = "ntext";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.binary)
                            {
                                objCurrentData.DataType = "binary";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.varbinary)
                            {
                                objCurrentData.DataType = "varbinary";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.image)
                            {
                                objCurrentData.DataType = "image";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.xml)
                            {
                                objCurrentData.DataType = "xml";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.timestamp)
                            {
                                objCurrentData.DataType = "timestamp";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.None)
                            {
                                objCurrentData.DataType = string.Empty;
                            }
                        }
                    }
                }

                if (View != null && View.Id == "SpreadSheetBuilder_ScientificData_ListView_Calibration")
                {
                    SpreadSheetBuilder_ScientificData objCurrentData = (SpreadSheetBuilder_ScientificData)e.Object;
                    if (objCurrentData != null)
                    {
                        if (e.PropertyName == "NonPersistantDataType")
                        {
                            if (objCurrentData.NonPersistantDataType == ScientificDataTypes.bigint)
                            {
                                objCurrentData.DataType = "bigint";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.numeric)
                            {
                                objCurrentData.DataType = "numeric";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.bit)
                            {
                                objCurrentData.DataType = "bit";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.smallint)
                            {
                                objCurrentData.DataType = "smallint";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.decimaltype)
                            {
                                objCurrentData.DataType = "decimal";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.smallmoney)
                            {
                                objCurrentData.DataType = "smallmoney";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.inttype)
                            {
                                objCurrentData.DataType = "int";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.tinyint)
                            {
                                objCurrentData.DataType = "tinyint";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.money)
                            {
                                objCurrentData.DataType = "money";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.floattype)
                            {
                                objCurrentData.DataType = "float";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.real)
                            {

                                objCurrentData.DataType = "real";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.date)
                            {
                                objCurrentData.DataType = "date";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.datetime2)
                            {
                                objCurrentData.DataType = "datetime2";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.datetime)
                            {
                                objCurrentData.DataType = "datetime";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.datetimeoffset)
                            {
                                objCurrentData.DataType = "datetimeoffset";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.smalldatetime)
                            {
                                objCurrentData.DataType = "smalldatetime";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.time)
                            {
                                objCurrentData.DataType = "time";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.chartype)
                            {
                                objCurrentData.DataType = "char";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.varchar10)
                            {
                                objCurrentData.DataType = "varchar(10)";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.varchar50)
                            {
                                objCurrentData.DataType = "varchar(50)";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.varchar100)
                            {
                                objCurrentData.DataType = "varchar(100)";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.text)
                            {
                                objCurrentData.DataType = "text";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.nchar)
                            {
                                objCurrentData.DataType = "nchar";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.nvarchar10)
                            {
                                objCurrentData.DataType = "nvarchar(10)";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.nvarchar50)
                            {
                                objCurrentData.DataType = "nvarchar(50)";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.nvarchar100)
                            {
                                objCurrentData.DataType = "nvarchar(100)";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.ntext)
                            {
                                objCurrentData.DataType = "ntext";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.binary)
                            {
                                objCurrentData.DataType = "binary";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.varbinary)
                            {
                                objCurrentData.DataType = "varbinary";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.image)
                            {
                                objCurrentData.DataType = "image";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.xml)
                            {
                                objCurrentData.DataType = "xml";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.timestamp)
                            {
                                objCurrentData.DataType = "timestamp";
                            }
                            else if (objCurrentData.NonPersistantDataType == ScientificDataTypes.None)
                            {
                                objCurrentData.DataType = string.Empty;
                            }
                            objCurrentData.Action = ScientificDataAction.Edit;
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

        #region Event
        private void SaveRawData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardView dv = (DashboardView)Application.MainWindow.View;
                DashboardViewItem rawDataListview = (DashboardViewItem)dv.FindItem("RawData"); // Calibration
                ListView rawLView = (ListView)rawDataListview.InnerView;
                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                if (rawLView != null)
                {

                    bool IsFieldEmpty = rawLView.CollectionSource.List.Cast<SpreadSheetBuilder_ScientificData>().ToList().Any(x => x.FieldName == null || x.FieldName == string.Empty);
                    bool IsDataTypeEmpty = rawLView.CollectionSource.List.Cast<SpreadSheetBuilder_ScientificData>().ToList().Any(x => x.DataType == null || x.FieldName == string.Empty);
                    if (!IsFieldEmpty && !IsDataTypeEmpty)
                    {
                        ((ASPxGridListEditor)rawLView.Editor).Grid.UpdateEdit();
                        rawLView.ObjectSpace.CommitChanges();
                        ObjectSpace.CommitChanges();
                        IList<SpreadSheetBuilder_ScientificData> lstScientificData = rawLView.CollectionSource.List.Cast<SpreadSheetBuilder_ScientificData>().Where(x => x.Action != ScientificDataAction.None).ToList();
                        if (lstScientificData != null && lstScientificData.Count > 0)
                        {
                            string strDataType = string.Empty;
                            foreach (SpreadSheetBuilder_ScientificData obj in lstScientificData)
                            {
                                //if (obj.Action == ScientificDataAction.New && obj.SaveInTable)
                                //{
                                //    SelectedData sproc = currentSession.ExecuteSproc("SpreadSheetBuilder_AutoCreateColumn", new OperandValue(obj.ResultType), new OperandValue(1), new OperandValue(0)
                                //    , new OperandValue(string.Empty), new OperandValue(obj.FieldName), new OperandValue(strDataType));
                                //    obj.Action = ScientificDataAction.None;
                                //}
                                //else if (obj.Action == ScientificDataAction.Edit && obj.SaveInTable)
                                //{
                                //    SelectedData sproc = currentSession.ExecuteSproc("SpreadSheetBuilder_AutoCreateColumn", new OperandValue(obj.ResultType), new OperandValue(0), new OperandValue(0)
                                // , new OperandValue(obj.OldColumnName), new OperandValue(obj.FieldName), new OperandValue(strDataType));
                                //    obj.Action = ScientificDataAction.None;
                                //}

                                if (obj.Action == ScientificDataAction.New && obj.SaveInTable)
                                {
                                    SelectedData sproc = currentSession.ExecuteSproc("SpreadSheetBuilder_AutoCreateColumn", new OperandValue(obj.ResultType), new OperandValue(1), new OperandValue(0)
                                    , new OperandValue(string.Empty), new OperandValue(obj.FieldName), new OperandValue(obj.DataType));

                                }
                                else if (obj.Action == ScientificDataAction.Edit)
                                {
                                    //x.FieldName, x.DataType, ScientificDataAction.None, x.SaveInTable, x.Oid
                                    for (int i = 0; i < columnList.Count; i++)
                                    {
                                        if (columnList[i].Item5 == obj.uqID)
                                        {
                                            if (columnList[i].Item4 != obj.SaveInTable && obj.SaveInTable == true)
                                            {
                                                SelectedData sproc = currentSession.ExecuteSproc("SpreadSheetBuilder_AutoCreateColumn", new OperandValue(obj.ResultType), new OperandValue(1), new OperandValue(0)
                                                , new OperandValue(string.Empty), new OperandValue(obj.FieldName), new OperandValue(obj.DataType));
                                            }
                                            else if (columnList[i].Item4 != obj.SaveInTable && obj.SaveInTable == false)
                                            {
                                                SelectedData sproc = currentSession.ExecuteSproc("SpreadSheetBuilder_AutoCreateColumn", new OperandValue(obj.ResultType), new OperandValue(0), new OperandValue(1)
                                               , new OperandValue(string.Empty), new OperandValue(obj.FieldName), new OperandValue(obj.DataType));
                                            }
                                            else if (columnList[i].Item4 == obj.SaveInTable)
                                            {
                                                SelectedData sproc = currentSession.ExecuteSproc("SpreadSheetBuilder_AutoCreateColumn", new OperandValue(obj.ResultType), new OperandValue(0), new OperandValue(0)
                                              , new OperandValue(columnList[i].Item1), new OperandValue(obj.FieldName), new OperandValue(obj.DataType));
                                            }
                                            break;
                                        }
                                    }


                                }
                                obj.Action = ScientificDataAction.None;
                            }

                        }
                        IsLoad = false;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        if (IsFieldEmpty && IsDataTypeEmpty)
                        {
                            Application.ShowViewStrategy.ShowMessage("FieldName && DataType should not be empty.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        else if (IsFieldEmpty)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotFieldNameEmpty"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        else if (IsDataTypeEmpty)
                        {
                            Application.ShowViewStrategy.ShowMessage("DataType should not be empty.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                            return;
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
        private void SaveCalibration_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardView dv = (DashboardView)Application.MainWindow.View;
                DashboardViewItem CalibrationDataListview = (DashboardViewItem)dv.FindItem("Calibration");
                ListView rawLView = (ListView)CalibrationDataListview.InnerView;
                if (rawLView != null)
                {
                    ((ASPxGridListEditor)rawLView.Editor).Grid.UpdateEdit();
                    bool IsFieldEmpty = rawLView.CollectionSource.List.Cast<SpreadSheetBuilder_ScientificData>().ToList().Any(x => x.FieldName == null || x.FieldName == string.Empty);
                    if (!IsFieldEmpty)
                    {
                        rawLView.ObjectSpace.CommitChanges();
                        ObjectSpace.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "NotFieldNameEmpty"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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

        private void AddCalibration_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SpreadSheetBuilder_ScientificData objSData = ((ListView)View).ObjectSpace.CreateObject<SpreadSheetBuilder_ScientificData>();
                objSData.NonPersistantDataType = ScientificDataTypes.None;
                objSData.ResultType = ScientificDataInfo.Calibration;
                objSData.Action = ScientificDataAction.New;
                ((ListView)View).CollectionSource.Add(ObjectSpace.GetObject(objSData));
                ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_ScientificData>().OrderBy(a => a.Sort).ToList();
                ((ListView)View).Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AddRawData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SpreadSheetBuilder_ScientificData objSData = ((ListView)View).ObjectSpace.CreateObject<SpreadSheetBuilder_ScientificData>();
                objSData.NonPersistantDataType = ScientificDataTypes.None;
                objSData.ResultType = ScientificDataInfo.RawData;
                objSData.Action = ScientificDataAction.New;
                objSData.SaveInTable = true;
                ((ListView)View).CollectionSource.Add(ObjectSpace.GetObject(objSData));
                ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_ScientificData>().OrderBy(a => a.Sort).ToList();
                ((ListView)View).Refresh();
                if (columnList != null)
                {
                    columnList.Add(Tuple.Create(objSData.FieldName, objSData.DataType, objSData.Action, objSData.SaveInTable, objSData.uqID));
                }
                else
                {
                    columnList = new List<Tuple<string, string, ScientificDataAction, bool, int>>();
                    columnList.Add(Tuple.Create(objSData.FieldName, objSData.DataType, objSData.Action, objSData.SaveInTable, objSData.uqID));
                    IsLoad = true;
                }
                // ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;

            }

            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }



        #endregion

        private void DeleteCalibrationData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = ((ListView)View).ObjectSpace;
                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                SpreadSheetBuilder_ScientificData objDelete = (SpreadSheetBuilder_ScientificData)e.CurrentObject;
                if (objDelete != null)
                {
                    SpreadSheetBuilder_ScientificData obj = os.FindObject<SpreadSheetBuilder_ScientificData>(CriteriaOperator.Parse("[uqID] = ?", objDelete.uqID));
                    os.Delete(obj);
                    ((ListView)View).CollectionSource.Remove(os.GetObject(obj));
                    os.CommitChanges();
                    ((ListView)View).Refresh();
                    Application.ShowViewStrategy.ShowMessage("Deleted Successfully", InformationType.Success, 3000, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DeleteRawData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = ((ListView)View).ObjectSpace;
                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                SpreadSheetBuilder_ScientificData objDelete = (SpreadSheetBuilder_ScientificData)e.CurrentObject;
                if (objDelete != null)
                {

                    SpreadSheetBuilder_ScientificData obj = os.FindObject<SpreadSheetBuilder_ScientificData>(CriteriaOperator.Parse("[uqID] = ?", objDelete.uqID));
                    os.Delete(obj);
                    ((ListView)View).CollectionSource.Remove(os.GetObject(obj));
                    os.CommitChanges();
                    ((ListView)View).Refresh();

                    for (int i = 0; i < columnList.Count; i++)
                    {
                        if (columnList[i].Item5 == obj.uqID)
                        {
                            SelectedData sproc = currentSession.ExecuteSproc("SpreadSheetBuilder_AutoCreateColumn", new OperandValue(obj.ResultType), new OperandValue(0), new OperandValue(1)
                            , new OperandValue(columnList[i].Item1), new OperandValue(obj.FieldName), new OperandValue(obj.DataType));
                            break;
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
