using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace LDM.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class MessagesController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public MessagesController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Messages);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

        }
        protected override void OnDeactivated()
        {

            base.OnDeactivated();
        }
        private void ImportFileAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {

                ItemsFileUpload itemsFile = (ItemsFileUpload)e.PopupWindowViewCurrentObject;
                if (itemsFile.InputFile != null)
                {
                    byte[] file = itemsFile.InputFile.Content;
                    string fileExtension = Path.GetExtension(itemsFile.InputFile.FileName);
                    DevExpress.Spreadsheet.Workbook workbook = new DevExpress.Spreadsheet.Workbook();
                    if (fileExtension == ".xlsx")
                    {
                        workbook.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                    }
                    else if (fileExtension == ".xls")
                    {
                        workbook.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xls);
                    }
                    DevExpress.Spreadsheet.WorksheetCollection worksheets = workbook.Worksheets;
                    DevExpress.Spreadsheet.Worksheet worksheet = workbook.Worksheets[0];
                    CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                    DataTable dt = worksheet.CreateDataTable(range, true);
                    for (int col = 0; col < range.ColumnCount; col++)
                    {
                        CellValueType cellType = range[0, col].Value.Type;
                        for (int r = 1; r < range.RowCount; r++)
                        {
                            if (cellType != range[r, col].Value.Type)
                            {
                                dt.Columns[col].DataType = typeof(string);
                                break;
                            }
                        }
                    }
                    DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
                    IObjectSpace os = Application.CreateObjectSpace();
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(((XPObjectSpace)os).Session.DataLayer);
                    exporter.Export();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow row1 = dt.Rows[0];
                        if (row1[0].ToString() == dt.Columns[0].Caption)
                        {
                            row1.Delete();
                            dt.AcceptChanges();
                        }
                        foreach (DataColumn c in dt.Columns)
                            c.ColumnName = c.ColumnName.ToString().Trim();

                        foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                        {
                            if (dt.Columns.Contains("MessageEn") && !row.IsNull("MessageEn") || dt.Columns.Contains("ViewName") && !row.IsNull("ViewName") || dt.Columns.Contains("MessageCN") && !row.IsNull("MessageCN") || dt.Columns.Contains("MessageKey") && !row.IsNull("MessageKey") || dt.Columns.Contains("BusinessObject") && !row.IsNull("BusinessObject") || dt.Columns.Contains("Module") && !row.IsNull("Module") || dt.Columns.Contains("ActionType") && !row.IsNull("ActionType") || dt.Columns.Contains("Businuss_Object") && !row.IsNull("Businuss_Object"))
                            {
                                string strEn = row["MessageEn"].ToString().Trim();
                                string strViewName = row["ViewName"].ToString().Trim();
                                string strMessageCN = row["MessageCN"].ToString().Trim();
                                string strMessageKey = row["MessageKey"].ToString().Trim();
                                string strBusinessObject = row["BusinessObject"].ToString().Trim();
                                string strModule = row["Module"].ToString().Trim();
                                string strActionType = row["ActionType"].ToString().Trim();
                                string strBusiness_Object = row["Businuss_Object"].ToString().Trim();

                                Messages objMessages = new Messages(uow);
                                objMessages.MessageEN = strEn;
                                objMessages.ViewName = strViewName;
                                objMessages.MessageCN = strMessageCN;
                                objMessages.MessageKey = strMessageKey;
                                objMessages.BusinessObject = strBusinessObject;
                                objMessages.Module = strModule;
                                objMessages.ActionType = strActionType;
                                objMessages.Businuss_Object = strBusiness_Object;


                                objMessages.Save();
                            }
                        }

                        uow.CommitChanges();
                    }
                    View.ObjectSpace.CommitChanges();
                }
                View.ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ImportMessagesFromFileAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace sheetObjectSpace = Application.CreateObjectSpace(typeof(ItemsFileUpload));
                ItemsFileUpload spreadSheet = sheetObjectSpace.CreateObject<ItemsFileUpload>();
                DetailView createdView = Application.CreateDetailView(sheetObjectSpace, spreadSheet);
                createdView.ViewEditMode = ViewEditMode.Edit;
                e.View = createdView;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}

