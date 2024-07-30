using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using DevExpress.Web;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VendorViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        Guid User;
        NavigationItemsInfo NavigationId = new NavigationItemsInfo();

        string strproductcode = string.Empty;
        string strdescription = string.Empty;
        string strcategory = string.Empty;
        decimal doubleprice = 0;



        DateTime ApprovedDate;
        DateTime CertExpDate;
        DateTime RetiredDate;

        string strmobphone = string.Empty;
        string strOffPhone = string.Empty;
        string stracc = string.Empty;
        string strAccount = string.Empty;
        string strAddress1 = string.Empty;
        string strAddress2 = string.Empty;
        string strApprovedBy = string.Empty;
        string strfirstname = string.Empty;
        string strlastname = string.Empty;
        string strCertificate = string.Empty;
        string strComment = string.Empty;
        string strContact = string.Empty;
        string strCountry = string.Empty;
        string strEmail = string.Empty;
        string strFax = string.Empty;

        string strProductOrService = string.Empty;
        string strQualification = string.Empty;
        string strRetiredBy = string.Empty;
        string strState = string.Empty;
        string strStateCB = string.Empty;
        string strVendor = string.Empty;
        string strVendorcode = string.Empty;
        string strWebsite = string.Empty;
        string strZipCode = string.Empty;
        string strCity = string.Empty;
        string strUserName = string.Empty;

        public VendorViewController()
        {
            InitializeComponent();
            TargetViewId = "Vendors_DetailView;" + "Vendors_Evaluation_ListView;" + "VendorEvaluationItem_LookupListView;" + "Vendors_Evaluation_ListView_CopyEdit;" + "Vendors_ListView;" + "PriceList_ListView;";
            AddEvaluationItem.TargetViewId = "Vendors_Evaluation_ListView;";
            //ADDROW.TargetViewId = "Vendors_ListView";
            ImportFile.TargetViewId = "Vendors_ListView;" + "Vendors_DetailView;";
            ADDPrice.TargetViewId = "PriceList_ListView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ADDPrice.Active["ShowAddPrice"] = false; ;
                AddEvaluationItem.Active["ShowAddEvalItem"] = false; ;
                ImportFile.Active["ShowImportFile"] = false; ;
                Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                if (currentUser != null)
                {
                    User = currentUser.Oid;
                    //ViewEvalutationItem.Active.SetItemValue("valEdit", false);
                    //if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    //{
                    //    ViewEvalutationItem.Active.SetItemValue("valEdit", true);
                    //}
                    //else
                    //{
                    //    if (NavigationId.ClickedNavigationItem == "Vendors")
                    //    {
                    //        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                    //        {
                    //            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Vendors" && i.Write == true) != null)
                    //            {
                    //                ViewEvalutationItem.Active.SetItemValue("valEdit", true);
                    //            }
                    //        }
                    //    }
                    //}
                }
                if (View.Id == "Vendors_Evaluation_ListView")
                {
                    // Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Execute += DeleteAction_Execute;
                }
                if (View.Id == "Vendors_DetailView")
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

        private void DeleteAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Vendors_Evaluation_ListView")
                {
                    ObjectSpace.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void EditAction_Executing(object sender, CancelEventArgs e)
        {


        }

        private void NewObjectAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "Vendors_Evaluation_ListView")
                {
                    e.Cancel = true;
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(VendorEvaluationItem));
                    ListView createListview = Application.CreateListView("VendorEvaluationItem_LookupListView", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters();
                    showViewParameters.CreatedView = createListview;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += Dc_Accepting;
                    dc.AcceptAction.Executed += AcceptAction_Executed;
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

        private void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                View.ObjectSpace.Refresh();
                //View.Refresh();
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
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    ObjectSpace.CommitChanges();
                    IObjectSpace os = Application.CreateObjectSpace();
                    Vendors objVendor = os.GetObject((Vendors)Application.MainWindow.View.CurrentObject);
                    if (Application.MainWindow.View.ObjectSpace.IsNewObject(Application.MainWindow.View.CurrentObject))
                    {
                        //View.ObjectSpace.CommitChanges();
                        // ObjectSpace.CommitChanges();
                    }
                    foreach (VendorEvaluationItem objItem in e.AcceptActionArgs.SelectedObjects)
                    {
                        VendorEvaluationItem objVendorEvaluationItem = os.GetObject<VendorEvaluationItem>(objItem);
                        if (objVendorEvaluationItem != null)
                        {
                            VendorEvaluation objEvaluation = os.CreateObject<VendorEvaluation>();
                            ////objEvaluation.EvaluationItem = objVendorEvaluationItem;
                            ////objEvaluation.Author = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            ////objEvaluation.Date = DateTime.Today;
                            ////objEvaluation.Comment = objItem.Comment;
                            ////objEvaluation.Vendors.Add(objVendor);
                            // ((ListView)View).CollectionSource.Add(objEvaluation);
                        }
                    }
                    os.CommitChanges();
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
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
                if (View.Id == "Vendors_DetailView" && View.CurrentObject == e.Object && e.NewValue != e.OldValue)
                {
                    Vendors objVendors = (Vendors)e.Object;

                    if (e.PropertyName == "State")
                    {
                        if (objVendors.State != null)
                        {
                            string objstate = objVendors.State.Trim();
                            CustomState checkstate = ObjectSpace.FindObject<CustomState>(CriteriaOperator.Parse("[LongName] = ? Or [ShortName] = ?", objstate, objstate));
                            if (checkstate != null)
                            {
                                objVendors.StateCB = checkstate;
                                objVendors.State = checkstate.ShortName;
                            }
                        }
                    }
                    if (e.PropertyName == "StateCB")
                    {
                        if (objVendors.StateCB != null)
                        {
                            string objstateshort = objVendors.StateCB.ShortName;
                            string objstatelong = objVendors.StateCB.LongName;
                            CustomState checkstate = ObjectSpace.FindObject<CustomState>(CriteriaOperator.Parse("[LongName] = ? Or [ShortName] = ?", objstatelong, objstateshort));
                            if (checkstate != null)
                            {
                                objVendors.StateCB = checkstate;
                                objVendors.State = checkstate.ShortName;
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
                if (View.Id == "Vendors_DetailView")
                {
                    Vendors objacc = (Vendors)View.CurrentObject;
                    if (objacc.State != null)
                    {
                        string objstate = objacc.State.Trim();
                        CustomState checkstate = ObjectSpace.FindObject<CustomState>(CriteriaOperator.Parse("[LongName] = ? Or [ShortName] = ?", objstate, objstate));
                        if (checkstate != null)
                        {
                            objacc.StateCB = checkstate;
                        }
                    }
                }
                if (View.Id == "VendorEvaluationItem_LookupListView")
                {
                    if (User != null)
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        ASPxGridView gridView = gridListEditor.Grid;
                        if (gridView != null)
                        {
                            gridView.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        }
                        //Vendors objVendor = (Vendors)Application.MainWindow.View.CurrentObject;
                        ////List<Guid> lstEvaluation = objVendor.Evaluation.Where(i => i.Author.Oid == new Guid(SecuritySystem.CurrentUserId.ToString())).Select(i => i.EvaluationItem.Oid).ToList();
                        ////if(lstEvaluation!=null && lstEvaluation.Count>0)
                        ////{
                        ////    ((ListView)View).CollectionSource.Criteria["Filter"] = new NotOperator(new InOperator("Oid", lstEvaluation));
                        ////}

                    }
                }
                //else if(View.Id== "Vendors_Evaluation_ListView")
                //{
                //    Vendors objVendor =(Vendors)Application.MainWindow.View.CurrentObject;
                //    ////List<Guid> objList = objVendor.Evaluation.Select(i => i.Author).Distinct().ToList();
                //    //List<Guid> objList = objVendor.Evaluation.Select(i => i.Author.Oid).Distinct().ToList();
                //    //if (objList != null /*&& objList.Count>0*/)
                //    //{
                //    //    ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Author.Oid", objList);
                //    //}
                //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                //    using (XPView view = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(VendorEvaluation)))
                //    {
                //        view.Criteria = CriteriaOperator.Parse("[Vendors][[Oid] = ?]",objVendor.Oid);
                //        view.Properties.Add(new ViewProperty("Authors", SortDirection.Ascending, "Author", true, true));
                //        //view.Properties.Add(new ViewProperty("Date", SortDirection.Descending, "Date", true, true));
                //        view.Properties.Add(new ViewProperty("TopOid", SortDirection.Ascending, "Max(Oid)", false, true));
                //        List<object> groups = new List<object>();
                //        foreach (ViewRecord rec in view)
                //            groups.Add(rec["TopOid"]);
                //        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                //    }
                //}
                else if (View.Id == "Vendors_Evaluation_ListView_CopyEdit")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = gridListEditor.Grid;
                    if (gridView != null)
                    {
                        gridView.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    }
                }
                else if (View.Id == "Vendors_Evaluation_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        //gridListEditor.Grid.ClientSideEvents.Init = @"function (s, e){ s.GroupBy('Parent', 0,  'Descending');}";
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
                if (View.Id == "Vendors_DetailView")
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

        private void AddEvaluationItem_Execute_1(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Vendors_Evaluation_ListView")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(VendorEvaluationItem));
                    ListView createListview = Application.CreateListView("VendorEvaluationItem_LookupListView", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters();
                    showViewParameters.CreatedView = createListview;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += Dc_Accepting;
                    dc.AcceptAction.Executed += AcceptAction_Executed;
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

        private void ViewEvalutationItem_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                if (View.Id == "Vendors_Evaluation_ListView")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objspace, typeof(VendorEvaluation));
                    //Vendors objVendor = (Vendors)Application.MainWindow.View.CurrentObject;
                    //VendorEvaluation obj = objspace.GetObject((VendorEvaluation)View.CurrentObject);
                    ////List<Guid> lstEvaluation = objVendor.Evaluation.Where(i => i.Author.Oid == new Guid(obj.Author.Oid.ToString())).Select(i => i.Oid).ToList();
                    ////if (lstEvaluation != null && lstEvaluation.Count > 0)
                    ////{
                    ////    cs.Criteria["Filter"] = new InOperator("Oid", lstEvaluation);
                    ////}
                    ListView lv = Application.CreateListView("Vendors_Evaluation_ListView_CopyEdit", cs, true);
                    e.View = lv;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ViewEvalutationItem_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                ((ASPxGridListEditor)((ListView)e.PopupWindowView).Editor).Grid.UpdateEdit();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void ADDROW_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        IObjectSpace os = Application.CreateObjectSpace();
        //        if (View.Id == "Vendors_ListView")  //"DageoursGoodsInfo_DangerousGoods_ListView"
        //        {
        //            Vendors objCatalog = os.CreateObject<Vendors>();

        //            os.CommitChanges();
        //            os.Dispose();
        //            View.ObjectSpace.Refresh();
        //            View.Refresh();
        //        }

        //        View.Refresh();

        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }

        //}

        private void ImportFile_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace sheetObjectSpace = Application.CreateObjectSpace(typeof(ItemsFileUpload));
                ItemsFileUpload spreadSheet = (ItemsFileUpload)sheetObjectSpace.CreateObject<ItemsFileUpload>();
                DetailView createdView = Application.CreateDetailView(sheetObjectSpace, spreadSheet);
                createdView.ViewEditMode = ViewEditMode.Edit;
                //e.ShowViewParameters.CreatedView = createdView;
                ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                showViewParameters.Context = TemplateContext.NestedFrame;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += ImportFile_Execute;
                dc.CloseOnCurrentObjectProcessing = false;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ImportFile_Execute(object sender, DialogControllerAcceptingEventArgs e)
        {

            try
            {
                ResourceManager rmEnglish = new ResourceManager("Resources.LocalizeResourcesVendorsEnglish", Assembly.Load("App_GlobalResources"));
                ResourceManager rmChinese = new ResourceManager("Resources.LocalizeResourcesVendorsChinese", Assembly.Load("App_GlobalResources"));
                ItemsFileUpload itemsFile = (ItemsFileUpload)e.AcceptActionArgs.CurrentObject;
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
                    }
                    foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                    {
                        var isEmpty = row.ItemArray.All(c => c is DBNull);
                        if (!isEmpty)
                        {
                            List<string> errorlist = new List<string>();
                            DateTime dateTime;
                            if (dt.Columns.Contains(rmChinese.GetString("Account")) && !row.IsNull(rmChinese.GetString("Account")))
                            {
                                strAccount = row[rmChinese.GetString("Account")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Account")) && !row.IsNull(rmEnglish.GetString("Account")))
                            {
                                strAccount = row[rmEnglish.GetString("Account")].ToString();
                            }
                            else
                            {
                                strAccount = string.Empty;
                            }

                            //if (dt.Columns.Contains(rmChinese.GetString("ACC")) && !row.IsNull(rmChinese.GetString("ACC")))
                            //{
                            //    stracc = row[rmChinese.GetString("ACC")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("ACC")) && !row.IsNull(rmEnglish.GetString("ACC")))
                            //{
                            //    stracc = row[rmEnglish.GetString("ACC")].ToString();
                            //}
                            //else
                            //{
                            //    stracc = string.Empty;
                            //}

                            //////

                            if (dt.Columns.Contains(rmChinese.GetString("Address")) && !row.IsNull(rmChinese.GetString("Address")))
                            {
                                strAddress1 = row[rmChinese.GetString("Address")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Address")) && !row.IsNull(rmEnglish.GetString("Address")))
                            {
                                strAddress1 = row[rmEnglish.GetString("Address")].ToString();
                            }
                            else
                            {
                                strAddress1 = string.Empty;
                            }


                            if (dt.Columns.Contains(rmChinese.GetString("Address2")) && !row.IsNull(rmChinese.GetString("Address2")))
                            {
                                strAddress2 = row[rmChinese.GetString("Address2")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Address2")) && !row.IsNull(rmEnglish.GetString("Address2")))
                            {
                                strAddress2 = row[rmEnglish.GetString("Address2")].ToString();
                            }
                            else
                            {
                                strAddress2 = string.Empty;
                            }


                            if (dt.Columns.Contains(rmChinese.GetString("ApprovedBy")) && !row.IsNull(rmChinese.GetString("ApprovedBy")))
                            {
                                strApprovedBy = row[rmChinese.GetString("ApprovedBy")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("ApprovedBy")) && !row.IsNull(rmEnglish.GetString("ApprovedBy")))
                            {
                                strApprovedBy = row[rmEnglish.GetString("ApprovedBy")].ToString();
                            }
                            else
                            {
                                strApprovedBy = string.Empty;
                            }

                            if (dt.Columns.Contains(rmChinese.GetString("FirstName")) && !row.IsNull(rmChinese.GetString("FirstName")))
                            {
                                strfirstname = row[rmChinese.GetString("FirstName")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("FirstName")) && !row.IsNull(rmEnglish.GetString("FirstName")))
                            {
                                strfirstname = row[rmEnglish.GetString("FirstName")].ToString();
                            }
                            else
                            {
                                strfirstname = string.Empty;
                            }

                            if (dt.Columns.Contains(rmChinese.GetString("LastName")) && !row.IsNull(rmChinese.GetString("LastName")))
                            {
                                strlastname = row[rmChinese.GetString("LastName")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("LastName")) && !row.IsNull(rmEnglish.GetString("LastName")))
                            {
                                strlastname = row[rmEnglish.GetString("LastName")].ToString();
                            }
                            else
                            {
                                strlastname = string.Empty;
                            }



                            if (dt.Columns.Contains(rmChinese.GetString("CertificateNumber")) && !row.IsNull(rmChinese.GetString("CertificateNumber")))
                            {
                                strCertificate = row[rmChinese.GetString("CertificateNumber")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("CertificateNumber")) && !row.IsNull(rmEnglish.GetString("CertificateNumber")))
                            {
                                strCertificate = row[rmEnglish.GetString("CertificateNumber")].ToString();
                            }
                            else
                            {
                                strCertificate = string.Empty;
                            }


                            if (dt.Columns.Contains(rmChinese.GetString("City")) && !row.IsNull(rmChinese.GetString("City")))
                            {
                                strCity = row[rmChinese.GetString("City")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("City")) && !row.IsNull(rmEnglish.GetString("City")))
                            {
                                strCity = row[rmEnglish.GetString("City")].ToString();
                            }
                            else
                            {
                                strCity = string.Empty;

                            }

                            if (dt.Columns.Contains(rmChinese.GetString("Comment")) && !row.IsNull(rmChinese.GetString("Comment")))
                            {
                                strComment = row[rmChinese.GetString("Comment")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Comment")) && !row.IsNull(rmEnglish.GetString("Comment")))
                            {
                                strComment = row[rmEnglish.GetString("Comment")].ToString();
                            }
                            else
                            {
                                strComment = string.Empty;
                            }


                            if (dt.Columns.Contains(rmChinese.GetString("Contact")) && !row.IsNull(rmChinese.GetString("Contact")))
                            {
                                strContact = row[rmChinese.GetString("Contact")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Contact")) && !row.IsNull(rmEnglish.GetString("Contact")))
                            {
                                strContact = row[rmEnglish.GetString("Contact")].ToString();
                            }
                            else
                            {
                                strContact = string.Empty;
                            }


                            if (dt.Columns.Contains(rmChinese.GetString("Country")) && !row.IsNull(rmChinese.GetString("Country")))
                            {
                                strCountry = row[rmChinese.GetString("Country")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Country")) && !row.IsNull(rmEnglish.GetString("Country")))
                            {
                                strCountry = row[rmEnglish.GetString("Country")].ToString();
                            }
                            else
                            {
                                strCountry = string.Empty;
                            }

                            if (dt.Columns.Contains(rmChinese.GetString("Email")) && !row.IsNull(rmChinese.GetString("Email")))
                            {
                                strEmail = row[rmChinese.GetString("Email")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Email")) && !row.IsNull(rmEnglish.GetString("Email")))
                            {
                                strEmail = row[rmEnglish.GetString("Email")].ToString();
                            }
                            else
                            {
                                strEmail = string.Empty;
                            }

                            if (dt.Columns.Contains(rmChinese.GetString("Fax")) && !row.IsNull(rmChinese.GetString("Fax")))
                            {
                                strFax = row[rmChinese.GetString("Fax")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Fax")) && !row.IsNull(rmEnglish.GetString("Fax")))
                            {
                                strFax = row[rmEnglish.GetString("Fax")].ToString();
                            }
                            else
                            {
                                strFax = string.Empty;
                            }

                            if (dt.Columns.Contains(rmChinese.GetString("OfficePhone")) && !row.IsNull(rmChinese.GetString("OfficePhone")))
                            {
                                strOffPhone = row[rmChinese.GetString("OfficePhone")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("OfficePhone")) && !row.IsNull(rmEnglish.GetString("OfficePhone")))
                            {
                                strOffPhone = row[rmEnglish.GetString("OfficePhone")].ToString();
                            }
                            else
                            {
                                strOffPhone = string.Empty;
                            }

                            if (dt.Columns.Contains(rmChinese.GetString("MobilePhone")) && !row.IsNull(rmChinese.GetString("MobilePhone")))
                            {
                                strmobphone = row[rmChinese.GetString("MobilePhone")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("MobilePhone")) && !row.IsNull(rmEnglish.GetString("MobilePhone")))
                            {
                                strmobphone = row[rmEnglish.GetString("MobilePhone")].ToString();
                            }
                            else
                            {
                                strmobphone = string.Empty;
                            }



                            if (dt.Columns.Contains(rmChinese.GetString("ProductOrService")) && !row.IsNull(rmChinese.GetString("ProductOrService")))
                            {
                                strProductOrService = row[rmChinese.GetString("ProductOrService")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("ProductOrService")) && !row.IsNull(rmEnglish.GetString("ProductOrService")))
                            {
                                strProductOrService = row[rmEnglish.GetString("ProductOrService")].ToString();
                            }
                            else
                            {
                                strProductOrService = string.Empty;
                            }

                            if (dt.Columns.Contains(rmChinese.GetString("Qualification")) && !row.IsNull(rmChinese.GetString("Qualification")))
                            {
                                strQualification = row[rmChinese.GetString("Qualification")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Qualification")) && !row.IsNull(rmEnglish.GetString("Qualification")))
                            {
                                strQualification = row[rmEnglish.GetString("Qualification")].ToString();
                            }
                            else
                            {
                                strQualification = string.Empty;
                            }


                            //if (dt.Columns.Contains(rmChinese.GetString("RetiredBy")) && !row.IsNull(rmChinese.GetString("RetiredBy")))
                            //{
                            //    strRetiredBy = row[rmChinese.GetString("RetiredBy")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("RetiredBy")) && !row.IsNull(rmEnglish.GetString("RetiredBy")))
                            //{
                            //    strRetiredBy = row[rmEnglish.GetString("RetiredBy")].ToString();
                            //}
                            //else
                            //{
                            //    strRetiredBy = string.Empty;
                            //}

                            if (dt.Columns.Contains(rmChinese.GetString("State")) && !row.IsNull(rmChinese.GetString("State")))
                            {
                                strState = row[rmChinese.GetString("State")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("State")) && !row.IsNull(rmEnglish.GetString("State")))
                            {
                                strState = row[rmEnglish.GetString("State")].ToString();
                            }
                            else
                            {
                                strState = string.Empty;
                            }



                            if (dt.Columns.Contains(rmChinese.GetString("Vendor")) && !row.IsNull(rmChinese.GetString("Vendor")))
                            {
                                strVendor = row[rmChinese.GetString("Vendor")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Vendor")) && !row.IsNull(rmEnglish.GetString("Vendor")))
                            {
                                strVendor = row[rmEnglish.GetString("Vendor")].ToString();
                            }
                            else
                            {
                                strVendor = string.Empty;
                            }

                            //if (dt.Columns.Contains(rmChinese.GetString("Vendorcode")) && !row.IsNull(rmChinese.GetString("Vendorcode")))
                            //{
                            //    strVendorcode = row[rmChinese.GetString("Vendorcode")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("Vendorcode")) && !row.IsNull(rmEnglish.GetString("Vendorcode")))
                            //{
                            //    strVendorcode = row[rmEnglish.GetString("Vendorcode")].ToString();
                            //}
                            //else
                            //{
                            //    strVendorcode = string.Empty;
                            //}

                            if (dt.Columns.Contains(rmChinese.GetString("Website")) && !row.IsNull(rmChinese.GetString("Website")))
                            {
                                strWebsite = row[rmChinese.GetString("Website")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Website")) && !row.IsNull(rmEnglish.GetString("Website")))
                            {
                                strWebsite = row[rmEnglish.GetString("Website")].ToString();
                            }
                            else
                            {
                                strWebsite = string.Empty;
                            }

                            if (dt.Columns.Contains(rmChinese.GetString("ZipCode")) && !row.IsNull(rmChinese.GetString("ZipCode")))
                            {
                                strZipCode = row[rmChinese.GetString("ZipCode")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("ZipCode")) && !row.IsNull(rmEnglish.GetString("ZipCode")))
                            {
                                strZipCode = row[rmEnglish.GetString("ZipCode")].ToString();
                            }
                            else
                            {
                                strZipCode = string.Empty;
                            }





                            if ((dt.Columns.Contains(rmChinese.GetString("RetiredDate")) && !row.IsNull(rmChinese.GetString("RetiredDate")) &&
                                    !DateTime.TryParseExact(row[rmChinese.GetString("RetiredDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmChinese.GetString("RetiredDate")].GetType() != typeof(DateTime))
                                   || (dt.Columns.Contains(rmEnglish.GetString("RetiredDate")) && !row.IsNull(rmEnglish.GetString("RetiredDate")) &&
                                   !DateTime.TryParseExact(row[rmEnglish.GetString("RetiredDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmEnglish.GetString("RetiredDate")].GetType() != typeof(DateTime)))
                            {
                                errorlist.Add("RetiredDate");
                            }

                            if ((dt.Columns.Contains(rmChinese.GetString("ApprovedDate")) && !row.IsNull(rmChinese.GetString("ApprovedDate")) &&
                                !DateTime.TryParseExact(row[rmChinese.GetString("ApprovedDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmChinese.GetString("ApprovedDate")].GetType() != typeof(DateTime))
                              || (dt.Columns.Contains(rmEnglish.GetString("ApprovedDate")) && !row.IsNull(rmEnglish.GetString("ApprovedDate")) &&
                              !DateTime.TryParseExact(row[rmEnglish.GetString("ApprovedDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmEnglish.GetString("ApprovedDate")].GetType() != typeof(DateTime)))
                            {
                                errorlist.Add("ApprovedDate");
                            }

                            if ((dt.Columns.Contains(rmChinese.GetString("CertificateExpirationDate")) && !row.IsNull(rmChinese.GetString("CertificateExpirationDate")) &&
                                !DateTime.TryParseExact(row[rmChinese.GetString("CertificateExpirationDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmChinese.GetString("CertificateExpirationDate")].GetType() != typeof(DateTime))
                              || (dt.Columns.Contains(rmEnglish.GetString("CertificateExpirationDate")) && !row.IsNull(rmEnglish.GetString("CertificateExpirationDate")) &&
                              !DateTime.TryParseExact(row[rmEnglish.GetString("CertificateExpirationDate")].ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dateTime) && row[rmEnglish.GetString("CertificateExpirationDate")].GetType() != typeof(DateTime)))
                            {
                                errorlist.Add("CertificateExpirationDate");
                            }

                            if (errorlist.Count == 0)
                            {
                                Vendors findItem = ObjectSpace.FindObject<Vendors>(CriteriaOperator.Parse("[Vendor]  = '" + strVendor + "'")); // And [City.CityName] ='" + strCity + "' And [CustomCountry.Country] ='" + strCountry + "' And [State.LongName] ='" + strState  + "' And [AmountUnit.UnitName] ='" + strAmountUnits + "' AND [Specification]='" + strSpecification + "' AND [Category.Name] = '" + strCategory + "'"));
                                if (findItem == null)
                                {


                                    //Nullable<DateTime> ApprovedDate = null;
                                    //Nullable<DateTime> CertExpDate = null;
                                    //Nullable<DateTime> RetiredDate = null;

                                    Vendors vendor = ObjectSpace.CreateObject<Vendors>();
                                    vendor.Vendor = strVendor;

                                    vendor.ZipCode = strZipCode;
                                    vendor.Website = strWebsite;
                                    ////vendor.ProductOrService = strProductOrService;
                                    vendor.Qualification = strQualification;
                                    vendor.Phone = strOffPhone;
                                    ////vendor.MobilePhone = strmobphone;
                                    vendor.Fax = strFax;
                                    vendor.Email = strEmail;
                                    vendor.Contact = strContact;
                                    vendor.Comment = strComment;
                                    vendor.Address2 = strAddress2;
                                    vendor.Address1 = strAddress1;
                                    vendor.Account = strAccount;
                                    // //vendor.AccountName = strAccount;
                                    vendor.Certificate = strCertificate;
                                    vendor.City = strCity;
                                    vendor.State = strState;



                                    IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Vendors));
                                    CriteriaOperator VendorCode = CriteriaOperator.Parse("Max(Vendorcode)");
                                    string NewVendorCode = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Vendors), VendorCode, null)) + 1).ToString();
                                    if (NewVendorCode.Length == 1)
                                    {
                                        NewVendorCode = "00" + NewVendorCode;
                                    }
                                    else if (NewVendorCode.Length == 2)
                                    {
                                        NewVendorCode = "0" + NewVendorCode;
                                    }
                                    vendor.Vendorcode = NewVendorCode;


                                    //if (strAccount != string.Empty)
                                    //{
                                    //    CRMAccount crmaccount = ObjectSpace.FindObject<CRMAccount>(CriteriaOperator.Parse("[AccountName]='" + strAccount + "'"));
                                    //    if (crmaccount != null)
                                    //    {
                                    //        vendor.Account = crmaccount;
                                    //    }
                                    //    else
                                    //    {
                                    //        CRMAccount createcrmaccount = ObjectSpace.CreateObject<CRMAccount>();
                                    //        createcrmaccount.AccountName = strAccount;
                                    //        vendor.Account = createcrmaccount;

                                    //    }
                                    //}


                                    if (strCity != string.Empty)
                                    {
                                        //City vendorcity = ObjectSpace.FindObject<City>(CriteriaOperator.Parse("[CityName]='" + strCity + "'"));
                                        //if (vendorcity != null)
                                        //{
                                        //    vendor.City = vendorcity;
                                        //}
                                        //else
                                        //{
                                        //    City createcity = ObjectSpace.CreateObject<City>();
                                        //    createcity.CityName = strCity;
                                        //    vendor.City = createcity;

                                        //}
                                    }
                                    if (strState != string.Empty)
                                    {
                                        //State vendorstate = ObjectSpace.FindObject<State>(CriteriaOperator.Parse("[LongName]='" + strState + "'"));
                                        //if (vendorstate != null)
                                        //{
                                        //    vendor.State = vendorstate;
                                        //}
                                        //else
                                        //{
                                        //    State createstate = ObjectSpace.CreateObject<State>();
                                        //    createstate.LongName = strState;
                                        //    vendor.State = createstate;

                                        //}
                                    }
                                    if (strCountry != string.Empty)
                                    {
                                        CustomCountry country = ObjectSpace.FindObject<CustomCountry>(CriteriaOperator.Parse("[EnglishLongName]='" + strCountry + "'"));
                                        if (country != null)
                                        {
                                            vendor.Country = country;
                                            if (country.EnglishLongName.ToString() == "United States of America")
                                            {
                                                string objstate = strState.Trim();
                                                CustomState checkstate = ObjectSpace.FindObject<CustomState>(CriteriaOperator.Parse("[LongName] = ? Or [ShortName] = ?", objstate, objstate));
                                                if (checkstate != null)
                                                {
                                                    vendor.StateCB = checkstate;
                                                    vendor.State = checkstate.ShortName;
                                                }
                                            }
                                        }
                                        //else
                                        //{
                                        //    CustomCountry createcountry = ObjectSpace.CreateObject<CustomCountry>();
                                        //    createcountry.EnglishLongName = strCountry;
                                        //    vendor.Country = createcountry;

                                        //}
                                    }

                                    if (strApprovedBy != string.Empty)
                                    {
                                        Employee Emp = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[FullName]='" + strApprovedBy + "'"));

                                        if (Emp != null)
                                        {
                                            vendor.ApprovedBy = Emp;
                                        }
                                        else
                                        {

                                            Employee createapprovedby = ObjectSpace.CreateObject<Employee>();
                                            createapprovedby.FirstName = strApprovedBy;
                                            createapprovedby.UserName = strApprovedBy;
                                            //string fullname = createapprovedby.FullName;
                                            //Employee emp1 = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[FullName]='" + fullname + "'"));
                                            vendor.ApprovedBy = createapprovedby;
                                        }

                                    }



                                    //if (dt.Columns.Contains(rmChinese.GetString("RetiredDate")) && !row.IsNull(rmChinese.GetString("RetiredDate")))
                                    //{
                                    //    if (row[rmChinese.GetString("RetiredDate")].GetType() == typeof(DateTime))
                                    //    {
                                    //        vendor.RetiredDate = Convert.ToDateTime(row[rmChinese.GetString("RetiredDate")]);
                                    //    }
                                    //    else if (row[rmChinese.GetString("RetiredDate")].GetType() == typeof(string))
                                    //    {
                                    //        string strdate = row[rmChinese.GetString("RetiredDate")].ToString();
                                    //        if (strdate != string.Empty)
                                    //        {
                                    //            vendor.RetiredDate = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                    //        }
                                    //    }
                                    //}
                                    //else if (dt.Columns.Contains(rmEnglish.GetString("RetiredDate")) && !row.IsNull(rmEnglish.GetString("RetiredDate")))
                                    //{
                                    //    if (row[rmEnglish.GetString("RetiredDate")].GetType() == typeof(DateTime))
                                    //    {
                                    //        vendor.RetiredDate = Convert.ToDateTime(row[rmEnglish.GetString("RetiredDate")]);
                                    //    }
                                    //    else if (row[rmEnglish.GetString("RetiredDate")].GetType() == typeof(string))
                                    //    {
                                    //        string strdate = row[rmEnglish.GetString("RetiredDate")].ToString();
                                    //        if (strdate != string.Empty)
                                    //        {
                                    //            vendor.RetiredDate = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                    //        }
                                    //    }
                                    //}


                                    if (dt.Columns.Contains(rmChinese.GetString("ApprovedDate")) && !row.IsNull(rmChinese.GetString("ApprovedDate")))
                                    {
                                        if (row[rmChinese.GetString("ApprovedDate")].GetType() == typeof(DateTime))
                                        {
                                            vendor.ApprovedDate = Convert.ToDateTime(row[rmChinese.GetString("ApprovedDate")]);
                                        }
                                        else if (row[rmChinese.GetString("ApprovedDate")].GetType() == typeof(string))
                                        {
                                            string strdate = row[rmChinese.GetString("ApprovedDate")].ToString();
                                            if (strdate != string.Empty)
                                            {
                                                vendor.ApprovedDate = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                            }
                                        }
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("ApprovedDate")) && !row.IsNull(rmEnglish.GetString("ApprovedDate")))
                                    {
                                        if (row[rmEnglish.GetString("ApprovedDate")].GetType() == typeof(DateTime))
                                        {
                                            vendor.ApprovedDate = Convert.ToDateTime(row[rmEnglish.GetString("ApprovedDate")]);
                                        }
                                        else if (row[rmEnglish.GetString("ApprovedDate")].GetType() == typeof(string))
                                        {
                                            string strdate = row[rmEnglish.GetString("ApprovedDate")].ToString();
                                            if (strdate != string.Empty)
                                            {
                                                vendor.ApprovedDate = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                            }
                                        }
                                    }

                                    if (dt.Columns.Contains(rmChinese.GetString("CertificateExpirationDate")) && !row.IsNull(rmChinese.GetString("CertificateExpirationDate")))
                                    {
                                        if (row[rmChinese.GetString("CertificateExpirationDate")].GetType() == typeof(DateTime))
                                        {
                                            vendor.CertExpDate = Convert.ToDateTime(row[rmChinese.GetString("CertificateExpirationDate")]);
                                        }
                                        else if (row[rmChinese.GetString("CertificateExpirationDate")].GetType() == typeof(string))
                                        {
                                            string strdate = row[rmChinese.GetString("CertificateExpirationDate")].ToString();
                                            if (strdate != string.Empty)
                                            {
                                                vendor.CertExpDate = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                            }
                                        }
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("CertificateExpirationDate")) && !row.IsNull(rmEnglish.GetString("CertificateExpirationDate")))
                                    {
                                        if (row[rmEnglish.GetString("CertificateExpirationDate")].GetType() == typeof(DateTime))
                                        {
                                            vendor.CertExpDate = Convert.ToDateTime(row[rmEnglish.GetString("CertificateExpirationDate")]);
                                        }
                                        else if (row[rmEnglish.GetString("CertificateExpirationDate")].GetType() == typeof(string))
                                        {
                                            string strdate = row[rmEnglish.GetString("CertificateExpirationDate")].ToString();
                                            if (strdate != string.Empty)
                                            {
                                                vendor.CertExpDate = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                            }
                                        }
                                    }
                                    ObjectSpace.CommitChanges();
                                    ObjectSpace.Refresh();
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    //CertExpDate
                                    //itemsltno.Items.Add(items.Oid + ";" + strStorage + ";" + dateExpiry + ";" + intstockqty.ToString()); //// + ";" + strVendorlt);
                                    //processItems(items, dt, row, rmChinese, rmEnglish);
                                    //((ListView)View).CollectionSource.Add(items);
                                }
                                else
                                {

                                    findItem.Certificate = strCertificate;
                                    findItem.ZipCode = strZipCode;
                                    findItem.Website = strWebsite;
                                    ////findItem.ProductOrService = strProductOrService;
                                    findItem.Qualification = strQualification;
                                    ////findItem.OfficePhone = strOffPhone;
                                    ////findItem.MobilePhone = strmobphone;
                                    findItem.Fax = strFax;
                                    findItem.Email = strEmail;
                                    findItem.Contact = strContact;
                                    findItem.Comment = strComment;
                                    findItem.Address2 = strAddress2;
                                    findItem.Address1 = strAddress1;
                                    ////findItem.Account = strAccount;
                                    ////findItem.AccountName = strAccount;
                                    //if (strAccount != string.Empty)
                                    //{
                                    //    CRMAccount crmaccount = ObjectSpace.FindObject<CRMAccount>(CriteriaOperator.Parse("[AccountName]='" + strAccount + "'"));
                                    //    if (crmaccount != null)
                                    //    {
                                    //        findItem.Account = crmaccount;
                                    //    }
                                    //    else
                                    //    {
                                    //        CRMAccount createcrmaccount = ObjectSpace.CreateObject<CRMAccount>();
                                    //        createcrmaccount.AccountName = strAccount;
                                    //        findItem.Account = createcrmaccount;

                                    //    }
                                    //}
                                    //items.Amount = strAmount;
                                    if (strCity != string.Empty)
                                    {
                                        //City vendorcity = ObjectSpace.FindObject<City>(CriteriaOperator.Parse("[CityName]='" + strCity + "'"));
                                        //if (vendorcity != null)
                                        //{
                                        //    findItem.City = vendorcity;
                                        //}
                                        //else
                                        //{
                                        //    City createcity = ObjectSpace.CreateObject<City>();
                                        //    createcity.CityName = strCity;
                                        //    findItem.City = createcity;

                                        //}
                                    }
                                    if (strState != string.Empty)
                                    {
                                        //State vendorstate = ObjectSpace.FindObject<State>(CriteriaOperator.Parse("[LongName]='" + strState + "'"));
                                        //if (vendorstate != null)
                                        //{
                                        //    findItem.State = vendorstate;
                                        //}
                                        //else
                                        //{
                                        //    State createstate = ObjectSpace.CreateObject<State>();
                                        //    createstate.LongName = strState;
                                        //    findItem.State = createstate;

                                        //}
                                    }
                                    if (strCountry != string.Empty)
                                    {
                                        CustomCountry country = ObjectSpace.FindObject<CustomCountry>(CriteriaOperator.Parse("[EnglishLongName]='" + strCountry + "'"));
                                        if (country != null)
                                        {
                                            findItem.Country = country;
                                        }
                                        else
                                        {
                                            CustomCountry createcountry = ObjectSpace.CreateObject<CustomCountry>();
                                            createcountry.EnglishLongName = strCountry;
                                            findItem.Country = createcountry;

                                        }
                                    }

                                    if (strApprovedBy != string.Empty)
                                    {

                                        Employee Emp = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[FullName] ='" + strApprovedBy + "'")); // AND [LastName] ='" + strlastname + "'"));

                                        if (Emp != null)
                                        {
                                            findItem.ApprovedBy = Emp;

                                        }

                                        else
                                        {

                                            Employee createapprovedby = ObjectSpace.CreateObject<Employee>();
                                            createapprovedby.FirstName = strApprovedBy;
                                            createapprovedby.UserName = strApprovedBy;
                                            //string fullname = createapprovedby.FullName;
                                            //Employee emp1 = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[FullName]='" + fullname + "'"));
                                            findItem.ApprovedBy = createapprovedby;

                                        }

                                    }
                                    //if (dt.Columns.Contains(rmChinese.GetString("RetiredDate")) && !row.IsNull(rmChinese.GetString("RetiredDate")))
                                    //{
                                    //    if (row[rmChinese.GetString("RetiredDate")].GetType() == typeof(DateTime))
                                    //    {
                                    //        findItem.RetiredDate = Convert.ToDateTime(row[rmChinese.GetString("RetiredDate")]);
                                    //    }
                                    //    else if (row[rmChinese.GetString("RetiredDate")].GetType() == typeof(string))
                                    //    {
                                    //        string strdate = row[rmChinese.GetString("RetiredDate")].ToString();
                                    //        if (strdate != string.Empty)
                                    //        {
                                    //            findItem.RetiredDate = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                    //        }
                                    //    }
                                    //}
                                    //else if (dt.Columns.Contains(rmEnglish.GetString("RetiredDate")) && !row.IsNull(rmEnglish.GetString("RetiredDate")))
                                    //{
                                    //    if (row[rmEnglish.GetString("RetiredDate")].GetType() == typeof(DateTime))
                                    //    {
                                    //        findItem.RetiredDate = Convert.ToDateTime(row[rmEnglish.GetString("RetiredDate")]);
                                    //    }
                                    //    else if (row[rmEnglish.GetString("RetiredDate")].GetType() == typeof(string))
                                    //    {
                                    //        string strdate = row[rmEnglish.GetString("RetiredDate")].ToString();
                                    //        if (strdate != string.Empty)
                                    //        {
                                    //            findItem.RetiredDate = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                    //        }
                                    //    }
                                    //}


                                    if (dt.Columns.Contains(rmChinese.GetString("ApprovedDate")) && !row.IsNull(rmChinese.GetString("ApprovedDate")))
                                    {
                                        if (row[rmChinese.GetString("ApprovedDate")].GetType() == typeof(DateTime))
                                        {
                                            findItem.ApprovedDate = Convert.ToDateTime(row[rmChinese.GetString("ApprovedDate")]);
                                        }
                                        else if (row[rmChinese.GetString("ApprovedDate")].GetType() == typeof(string))
                                        {
                                            string strdate = row[rmChinese.GetString("ApprovedDate")].ToString();
                                            if (strdate != string.Empty)
                                            {
                                                findItem.ApprovedDate = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                            }
                                        }
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("ApprovedDate")) && !row.IsNull(rmEnglish.GetString("ApprovedDate")))
                                    {
                                        if (row[rmEnglish.GetString("ApprovedDate")].GetType() == typeof(DateTime))
                                        {
                                            findItem.ApprovedDate = Convert.ToDateTime(row[rmEnglish.GetString("ApprovedDate")]);
                                        }
                                        else if (row[rmEnglish.GetString("ApprovedDate")].GetType() == typeof(string))
                                        {
                                            string strdate = row[rmEnglish.GetString("ApprovedDate")].ToString();
                                            if (strdate != string.Empty)
                                            {
                                                findItem.ApprovedDate = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                            }
                                        }
                                    }

                                    if (dt.Columns.Contains(rmChinese.GetString("CertificateExpirationDate")) && !row.IsNull(rmChinese.GetString("CertificateExpirationDate")))
                                    {
                                        if (row[rmChinese.GetString("CertificateExpirationDate")].GetType() == typeof(DateTime))
                                        {
                                            findItem.CertExpDate = Convert.ToDateTime(row[rmChinese.GetString("CertificateExpirationDate")]);
                                        }
                                        else if (row[rmChinese.GetString("CertificateExpirationDate")].GetType() == typeof(string))
                                        {
                                            string strdate = row[rmChinese.GetString("CertificateExpirationDate")].ToString();
                                            if (strdate != string.Empty)
                                            {
                                                findItem.CertExpDate = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                            }
                                        }
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("CertificateExpirationDate")) && !row.IsNull(rmEnglish.GetString("CertificateExpirationDate")))
                                    {
                                        if (row[rmEnglish.GetString("CertificateExpirationDate")].GetType() == typeof(DateTime))
                                        {
                                            findItem.CertExpDate = Convert.ToDateTime(row[rmEnglish.GetString("CertificateExpirationDate")]);
                                        }
                                        else if (row[rmEnglish.GetString("CertificateExpirationDate")].GetType() == typeof(string))
                                        {
                                            string strdate = row[rmEnglish.GetString("CertificateExpirationDate")].ToString();
                                            if (strdate != string.Empty)
                                            {
                                                findItem.CertExpDate = DateTime.ParseExact(strdate, "MM/dd/yyyy", null);
                                            }
                                        }
                                    }
                                    ObjectSpace.CommitChanges();
                                    ObjectSpace.Refresh();
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    ////itemsltno.Items.Add(findItem.Oid + ";" + strStorage + ";" + dateExpiry + ";" + intstockqty.ToString() + ";" + strVendorlt);
                                    ////processItems(findItem, dt, row, rmChinese, rmEnglish);
                                    ////((ListView)View).CollectionSource.Add(findItem);
                                }
                            }
                            else
                            {
                                var error = "Error in columns - " + string.Join(", ", errorlist) + " of row number - " + (dt.Rows.IndexOf(row) + 1);
                                Application.ShowViewStrategy.ShowMessage(error, InformationType.Error, 6000, InformationPosition.Top);
                                break;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, 3000, InformationPosition.Top);
            }

        }

        private void ADDPrice_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace sheetObjectSpace = Application.CreateObjectSpace(typeof(ItemsFileUpload));
                ItemsFileUpload spreadSheet = (ItemsFileUpload)sheetObjectSpace.CreateObject<ItemsFileUpload>();
                DetailView createdView = Application.CreateDetailView(sheetObjectSpace, spreadSheet);
                createdView.ViewEditMode = ViewEditMode.Edit;
                //e.ShowViewParameters.CreatedView = createdView;
                ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                showViewParameters.Context = TemplateContext.NestedFrame;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += Price_ImportFile_Execute;
                dc.CloseOnCurrentObjectProcessing = false;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Price_ImportFile_Execute(object sender, DialogControllerAcceptingEventArgs e)
        {

            try
            {
                ResourceManager rmEnglish = new ResourceManager("Resources.LocalizeResourcesPriceListEnglish", Assembly.Load("App_GlobalResources"));
                ResourceManager rmChinese = new ResourceManager("Resources.LocalizeResourcesPriceListChinese", Assembly.Load("App_GlobalResources"));
                ItemsFileUpload itemsFile = (ItemsFileUpload)e.AcceptActionArgs.CurrentObject;
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
                    }
                    foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                    {
                        var isEmpty = row.ItemArray.All(c => c is DBNull);
                        if (!isEmpty)
                        {
                            List<string> errorlist = new List<string>();
                            DateTime dateTime;
                            if (dt.Columns.Contains(rmChinese.GetString("ProductCode")) && !row.IsNull(rmChinese.GetString("ProductCode")))
                            {
                                strproductcode = row[rmChinese.GetString("ProductCode")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("ProductCode")) && !row.IsNull(rmEnglish.GetString("ProductCode")))
                            {
                                strproductcode = row[rmEnglish.GetString("ProductCode")].ToString();
                            }
                            else
                            {
                                strproductcode = string.Empty;
                            }

                            //////

                            if (dt.Columns.Contains(rmChinese.GetString("Description")) && !row.IsNull(rmChinese.GetString("Description")))
                            {
                                strdescription = row[rmChinese.GetString("Description")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Description")) && !row.IsNull(rmEnglish.GetString("Description")))
                            {
                                strdescription = row[rmEnglish.GetString("Description")].ToString();
                            }
                            else
                            {
                                strdescription = string.Empty;
                            }


                            //if (dt.Columns.Contains(rmChinese.GetString("Price")) && !row.IsNull(rmChinese.GetString("Price")))
                            //{
                            //    strAddress2 = row[rmChinese.GetString("Price")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("Price")) && !row.IsNull(rmEnglish.GetString("Price")))
                            //{
                            //    strAddress2 = row[rmEnglish.GetString("Price")].ToString();
                            //}
                            //else
                            //{
                            //    strAddress2 = string.Empty;
                            //}


                            if (dt.Columns.Contains(rmChinese.GetString("Category")) && !row.IsNull(rmChinese.GetString("Category")))
                            {
                                strcategory = row[rmChinese.GetString("Category")].ToString();
                            }
                            else if (dt.Columns.Contains(rmEnglish.GetString("Category")) && !row.IsNull(rmEnglish.GetString("Category")))
                            {
                                strcategory = row[rmEnglish.GetString("Category")].ToString();
                            }
                            else
                            {
                                strcategory = string.Empty;
                            }
                            if (errorlist.Count == 0)
                            {

                            }
                            else
                            {
                                var error = "Error in columns - " + string.Join(", ", errorlist) + " of row number - " + (dt.Rows.IndexOf(row) + 1);
                                Application.ShowViewStrategy.ShowMessage(error, InformationType.Error, 6000, InformationPosition.Top);
                                break;
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, 3000, InformationPosition.Top);
            }

        }
    }

}
